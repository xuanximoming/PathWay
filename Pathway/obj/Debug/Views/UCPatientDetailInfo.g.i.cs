﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\Views\UCPatientDetailInfo.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D09B424D3A5E25BAA2A443672568964E"
//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//     Runtime Version:4.0.30319.42000
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Windows;
using System.Windows.Automation;
using System.Windows.Automation.Peers;
using System.Windows.Automation.Provider;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Markup;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Resources;
using System.Windows.Shapes;
using System.Windows.Threading;


namespace YidanEHRApplication.Views {
    
    
    public partial class UCPatientDetailInfo : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock textBlock1;
        
        internal Telerik.Windows.Controls.RadDatePicker radDatePickerBegin;
        
        internal System.Windows.Controls.TextBlock textBlock2;
        
        internal Telerik.Windows.Controls.RadDatePicker radDatePickerEnd;
        
        internal System.Windows.Controls.Grid phySingCanvas;
        
        internal Telerik.Windows.Controls.RadChart RadChartFee;
        
        private bool _contentLoaded;
        
        /// <summary>
        /// InitializeComponent
        /// </summary>
        [System.Diagnostics.DebuggerNonUserCodeAttribute()]
        public void InitializeComponent() {
            if (_contentLoaded) {
                return;
            }
            _contentLoaded = true;
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/Views/UCPatientDetailInfo.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.textBlock1 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock1")));
            this.radDatePickerBegin = ((Telerik.Windows.Controls.RadDatePicker)(this.FindName("radDatePickerBegin")));
            this.textBlock2 = ((System.Windows.Controls.TextBlock)(this.FindName("textBlock2")));
            this.radDatePickerEnd = ((Telerik.Windows.Controls.RadDatePicker)(this.FindName("radDatePickerEnd")));
            this.phySingCanvas = ((System.Windows.Controls.Grid)(this.FindName("phySingCanvas")));
            this.RadChartFee = ((Telerik.Windows.Controls.RadChart)(this.FindName("RadChartFee")));
        }
    }
}

