﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\Views\PrintForm\RptPathVariationMonitor.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "27916F653E5808A19154A0589CF50DB0"
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


namespace YidanEHRApplication.Views.ReportForms {
    
    
    public partial class RptPathVariationMonitor : System.Windows.Controls.Page {
        
        internal Telerik.Windows.Controls.RadBusyIndicator radBusyIndicator;
        
        internal System.Windows.Controls.ScrollViewer PageScrollViewer;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Telerik.Windows.Controls.RadDateTimePicker dtpStartDate;
        
        internal Telerik.Windows.Controls.RadDateTimePicker dtpEndDate;
        
        internal System.Windows.Controls.AutoCompleteBox autoPath;
        
        internal System.Windows.Controls.Button btnMonitor;
        
        internal System.Windows.Controls.Button btnClear;
        
        internal System.Windows.Controls.Button btnPrint;
        
        internal System.Windows.Controls.Button btnDetail;
        
        internal Telerik.Windows.Controls.RadDataPager radDataPager;
        
        internal Telerik.Windows.Controls.RadGridView GridViewMonitor;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/Views/PrintForm/RptPathVariationMonitor.xaml", System.UriKind.Relative));
            this.radBusyIndicator = ((Telerik.Windows.Controls.RadBusyIndicator)(this.FindName("radBusyIndicator")));
            this.PageScrollViewer = ((System.Windows.Controls.ScrollViewer)(this.FindName("PageScrollViewer")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.dtpStartDate = ((Telerik.Windows.Controls.RadDateTimePicker)(this.FindName("dtpStartDate")));
            this.dtpEndDate = ((Telerik.Windows.Controls.RadDateTimePicker)(this.FindName("dtpEndDate")));
            this.autoPath = ((System.Windows.Controls.AutoCompleteBox)(this.FindName("autoPath")));
            this.btnMonitor = ((System.Windows.Controls.Button)(this.FindName("btnMonitor")));
            this.btnClear = ((System.Windows.Controls.Button)(this.FindName("btnClear")));
            this.btnPrint = ((System.Windows.Controls.Button)(this.FindName("btnPrint")));
            this.btnDetail = ((System.Windows.Controls.Button)(this.FindName("btnDetail")));
            this.radDataPager = ((Telerik.Windows.Controls.RadDataPager)(this.FindName("radDataPager")));
            this.GridViewMonitor = ((Telerik.Windows.Controls.RadGridView)(this.FindName("GridViewMonitor")));
        }
    }
}

