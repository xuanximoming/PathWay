﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\Views\UCPatientDetailPhySign.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "997055F2CB1AB407ADEB2B7C7B4C2C75"
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
    
    
    public partial class UCPatientDetailPhySign : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Telerik.Windows.Controls.RadChart RadChartPhySign;
        
        internal System.Windows.Controls.Canvas BackCanvas;
        
        internal System.Windows.Controls.Canvas parentCanvas;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/Views/UCPatientDetailPhySign.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.RadChartPhySign = ((Telerik.Windows.Controls.RadChart)(this.FindName("RadChartPhySign")));
            this.BackCanvas = ((System.Windows.Controls.Canvas)(this.FindName("BackCanvas")));
            this.parentCanvas = ((System.Windows.Controls.Canvas)(this.FindName("parentCanvas")));
        }
    }
}

