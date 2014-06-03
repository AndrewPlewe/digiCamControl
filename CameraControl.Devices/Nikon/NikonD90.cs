#region Licence

// Distributed under MIT License
// ===========================================================
// 
// digiCamControl - DSLR camera remote control open source software
// Copyright (C) 2014 Duka Istvan
// 
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files (the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions:
// 
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, 
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF 
// MERCHANTABILITY,FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY 
// CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
// THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.

#endregion

#region

using System;
using System.IO;
using CameraControl.Devices.Classes;
using PortableDeviceLib;

#endregion

namespace CameraControl.Devices.Nikon
{
    public class NikonD90 : NikonBase
    {
        public override bool Init(DeviceDescriptor deviceDescriptor)
        {
            bool res = base.Init(deviceDescriptor);
            Capabilities.Clear();
            Capabilities.Add(CapabilityEnum.LiveView);
            Capabilities.Add(CapabilityEnum.RecordMovie);
            Capabilities.Add(CapabilityEnum.CaptureInRam);
            Capabilities.Add(CapabilityEnum.CaptureNoAf);
            return res;
        }

        public override LiveViewData GetLiveViewImage()
        {
            LiveViewData viewData = new LiveViewData();
            viewData.HaveFocusData = true;

            const int headerSize = 128;

            byte[] result = StillImageDevice.ExecuteReadData(CONST_CMD_GetLiveViewImage);
            if (result == null || result.Length <= headerSize)
                return null;
            int cbBytesRead = result.Length;
            GetAditionalLIveViewData(viewData, result);

            MemoryStream copy = new MemoryStream((int) cbBytesRead - headerSize);
            copy.Write(result, headerSize, (int) cbBytesRead - headerSize);
            copy.Close();
            viewData.ImageData = copy.GetBuffer();

            return viewData;
        }

        /// <summary>
        /// Take picture with no autofocus
        /// If live view runnig the live view is stoped after done restarted
        /// </summary>
        public override void CapturePhotoNoAf()
        {
            lock (Locker)
            {
                try
                {
                    IsBusy = true;
                    MTPDataResponse response = ExecuteReadDataEx(CONST_CMD_GetDevicePropValue, CONST_PROP_LiveViewStatus,
                                                                 -1);
                    ErrorCodes.GetException(response.ErrorCode);
                    // test if live view is on 
                    if (response.Data != null && response.Data.Length > 0 && response.Data[0] > 0)
                    {
                        if (CaptureInSdRam)
                        {
                            DeviceReady();
                            ErrorCodes.GetException(ExecuteWithNoData(CONST_CMD_InitiateCaptureRecInSdram, 0xFFFFFFFF));
                            return;
                        }
                        StopLiveView();
                    }
                    // the focus mode can be sett only in host mode
                    LockCamera();
                    byte oldval = 0;
                    byte[] val = StillImageDevice.ExecuteReadData(CONST_CMD_GetDevicePropValue, CONST_PROP_AFModeSelect,
                                                                  -1);
                    if (val != null && val.Length > 0)
                        oldval = val[0];

                    SetProperty(CONST_CMD_SetDevicePropValue, new[] {(byte) 4}, CONST_PROP_AFModeSelect, -1);

                    ErrorCodes.GetException(CaptureInSdRam
                                                ? ExecuteWithNoData(CONST_CMD_InitiateCaptureRecInSdram, 0xFFFFFFFF)
                                                : ExecuteWithNoData(CONST_CMD_InitiateCapture));

                    if (val != null && val.Length > 0)
                        SetProperty(CONST_CMD_SetDevicePropValue, new[] {oldval}, CONST_PROP_AFModeSelect, -1);

                    UnLockCamera();
                }
                catch (Exception)
                {
                    IsBusy = false;
                    throw;
                }

                //if (live != null && live.Length > 0 && live[0] == 1)
                //  StartLiveView();
            }
        }
    }
}