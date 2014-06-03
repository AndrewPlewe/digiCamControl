﻿#region Licence

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
using System.Collections.Generic;
using System.Linq;
using System.Text;
using CameraControl.Classes;
using CameraControl.Core.Classes;
using CameraControl.Devices.Classes;

#endregion

namespace CameraControl.Actions.Enfuse
{
    public class EnfuseSettings : BaseFieldClass
    {
        private bool _alignImages;

        public bool AlignImages
        {
            get { return _alignImages; }
            set
            {
                _alignImages = value;
                NotifyPropertyChanged("AlignImages");
            }
        }

        private bool _optimizeFiledOfView;

        public bool OptimizeFiledOfView
        {
            get { return _optimizeFiledOfView; }
            set
            {
                _optimizeFiledOfView = value;
                NotifyPropertyChanged("OptimizeFiledOfView");
            }
        }

        private bool _autoCrop;

        public bool AutoCrop
        {
            get { return _autoCrop; }
            set
            {
                _autoCrop = value;
                NotifyPropertyChanged("AutoCrop");
            }
        }

        private double _enfuseExp;

        public double EnfuseExp
        {
            get { return _enfuseExp; }
            set
            {
                _enfuseExp = value;
                if (_enfuseExp < 0)
                    _enfuseExp = 0;
                if (_enfuseExp > 100)
                    _enfuseExp = 100;
                NotifyPropertyChanged("EnfuseExp");
            }
        }

        private double _enfuseCont;

        public double EnfuseCont
        {
            get { return _enfuseCont; }
            set
            {
                _enfuseCont = value;
                if (_enfuseCont < 0)
                    _enfuseCont = 0;
                if (_enfuseCont > 100)
                    _enfuseCont = 100;
                NotifyPropertyChanged("EnfuseCont");
            }
        }

        private double _enfuseSat;

        public double EnfuseSat
        {
            get { return _enfuseSat; }
            set
            {
                _enfuseSat = value;
                if (_enfuseSat < 0)
                    _enfuseSat = 0;
                if (_enfuseSat > 100)
                    _enfuseSat = 100;
                NotifyPropertyChanged("EnfuseSat");
            }
        }

        private double _enfuseEnt;

        public double EnfuseEnt
        {
            get { return _enfuseEnt; }
            set
            {
                _enfuseEnt = value;
                if (_enfuseEnt < 0)
                    _enfuseEnt = 0;
                if (_enfuseEnt > 100)
                    _enfuseEnt = 100;
                NotifyPropertyChanged("EnfuseEnt");
            }
        }

        private double _enfuseSigma;

        public double EnfuseSigma
        {
            get { return _enfuseSigma; }
            set
            {
                _enfuseSigma = value;
                if (_enfuseSigma < 0)
                    _enfuseSigma = 0;
                if (_enfuseSigma > 100)
                    _enfuseSigma = 100;
                NotifyPropertyChanged("EnfuseSigma");
            }
        }

        private bool _hardMask;

        public bool HardMask
        {
            get { return _hardMask; }
            set
            {
                _hardMask = value;
                NotifyPropertyChanged("HardMask");
            }
        }

        private int _contrasWindow;

        public int ContrasWindow
        {
            get { return _contrasWindow; }
            set
            {
                _contrasWindow = value;
                if (_contrasWindow < 3)
                    _contrasWindow = 3;
                NotifyPropertyChanged("ContrasWindow");
            }
        }

        private int _scale;

        public int Scale
        {
            get { return _scale; }
            set
            {
                _scale = value;
                NotifyPropertyChanged("Scale");
            }
        }

        public EnfuseSettings()
        {
            AlignImages = true;
            EnfuseExp = 100;
            EnfuseCont = 0;
            EnfuseEnt = 0;
            EnfuseSat = 20;
            EnfuseSigma = 20;
            HardMask = false;
            ContrasWindow = 5;
            Scale = 0;
        }
    }
}