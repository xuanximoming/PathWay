﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\Views\PrintForm\RptPathVariationMonitorDetail.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "A26604E3D421E53872040069CC400CEC"
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


namespace YidanEHRApplication.Views.PrintForm {
    
    
    public partial class RptPathVariationMonitorDetail : Telerik.Windows.Controls.RadWindow {
        
        internal Telerik.Windows.Controls.RadBusyIndicator radBusyIndicator;
        
        internal System.Windows.Controls.Grid all;
        
        internal System.Windows.Controls.Grid head;
        
        internal System.Windows.Controls.StackPanel stackpanel1;
        
        internal System.Windows.Controls.TextBlock txtClinicalPath;
        
        internal System.Windows.Controls.TextBlock txtPatient;
        
        internal System.Windows.Controls.TextBlock txtinpathTime;
        
        internal System.Windows.Controls.Button print;
        
        internal Telerik.Windows.Controls.RadGridView RadGridView1;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/Views/PrintForm/RptPathVariationMonitorDetail.xaml", System.UriKind.Relative));
            this.radBusyIndicator = ((Telerik.Windows.Controls.RadBusyIndicator)(this.FindName("radBusyIndicator")));
            this.all = ((System.Windows.Controls.Grid)(this.FindName("all")));
            this.head = ((System.Windows.Controls.Grid)(this.FindName("head")));
            this.stackpanel1 = ((System.Windows.Controls.StackPanel)(this.FindName("stackpanel1")));
            this.txtClinicalPath = ((System.Windows.Controls.TextBlock)(this.FindName("txtClinicalPath")));
            this.txtPatient = ((System.Windows.Controls.TextBlock)(this.FindName("txtPatient")));
            this.txtinpathTime = ((System.Windows.Controls.TextBlock)(this.FindName("txtinpathTime")));
            this.print = ((System.Windows.Controls.Button)(this.FindName("print")));
            this.RadGridView1 = ((Telerik.Windows.Controls.RadGridView)(this.FindName("RadGridView1")));
        }
    }
}

