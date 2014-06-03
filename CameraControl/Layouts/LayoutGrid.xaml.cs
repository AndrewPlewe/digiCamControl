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
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using CameraControl.Core;

#endregion

namespace CameraControl.Layouts
{
    /// <summary>
    /// Interaction logic for LayoutGridRight.xaml
    /// </summary>
    public partial class LayoutGrid : LayoutBase
    {
        public LayoutGrid()
        {
            InitializeComponent();
            ImageLIst = ImageLIstBox;
            InitServices();
            ServiceProvider.Settings.PropertyChanged += Settings_PropertyChanged;
            ServiceProvider.Settings.DefaultSession.PropertyChanged += Settings_PropertyChanged;
            folderBrowserComboBox1.SelectedPath = ServiceProvider.Settings.DefaultSession.Folder;
        }

        private void Settings_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            if (e.PropertyName == "DefaultSession" || e.PropertyName == "Folder")
            {
                folderBrowserComboBox1.SelectedPath = ServiceProvider.Settings.DefaultSession.Folder;
            }
        }

        private void folderBrowserComboBox1_ValueChanged(object sender, EventArgs e)
        {
            ServiceProvider.Settings.DefaultSession.Folder = folderBrowserComboBox1.SelectedPath;
            ServiceProvider.QueueManager.Clear();
            ServiceProvider.Settings.DefaultSession.Files.Clear();
            ServiceProvider.Settings.LoadData(ServiceProvider.Settings.DefaultSession);
        }
    }
}