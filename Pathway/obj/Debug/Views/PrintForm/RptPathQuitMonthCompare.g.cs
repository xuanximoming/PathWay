﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\Views\PrintForm\RptPathQuitMonthCompare.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "DECEC6EE06FDE676F3E3BACB7EED03FC"
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
    
    
    public partial class RptPathQuitMonthCompare : System.Windows.Controls.Page {
        
        internal Telerik.Windows.Controls.RadBusyIndicator radBusyIndicator;
        
        internal System.Windows.Controls.ScrollViewer PageScrollViewer;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Telerik.Windows.Controls.RadDateTimePicker dtpStartDate;
        
        internal Telerik.Windows.Controls.RadComboBox cmbOffice;
        
        internal Telerik.Windows.Controls.RadComboBox cmbPath;
        
        internal System.Windows.Controls.Button btnComp;
        
        internal System.Windows.Controls.Button btnClear;
        
        internal System.Windows.Controls.Button btnPrint;
        
        internal System.Windows.Controls.Button btnDetail;
        
        internal Telerik.Windows.Controls.RadDataPager radDataPager;
        
        internal Telerik.Windows.Controls.RadGridView GridViewComp;
        
        internal System.Windows.Controls.Expander Expander1;
        
        internal System.Windows.Controls.Grid gridchat;
        
        internal System.Windows.Controls.RowDefinition HideRow0;
        
        internal Telerik.Windows.Controls.RadChart RadChartPie;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/Views/PrintForm/RptPathQuitMonthCompare.xaml", System.UriKind.Relative));
            this.radBusyIndicator = ((Telerik.Windows.Controls.RadBusyIndicator)(this.FindName("radBusyIndicator")));
            this.PageScrollViewer = ((System.Windows.Controls.ScrollViewer)(this.FindName("PageScrollViewer")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.dtpStartDate = ((Telerik.Windows.Controls.RadDateTimePicker)(this.FindName("dtpStartDate")));
            this.cmbOffice = ((Telerik.Windows.Controls.RadComboBox)(this.FindName("cmbOffice")));
            this.cmbPath = ((Telerik.Windows.Controls.RadComboBox)(this.FindName("cmbPath")));
            this.btnComp = ((System.Windows.Controls.Button)(this.FindName("btnComp")));
            this.btnClear = ((System.Windows.Controls.Button)(this.FindName("btnClear")));
            this.btnPrint = ((System.Windows.Controls.Button)(this.FindName("btnPrint")));
            this.btnDetail = ((System.Windows.Controls.Button)(this.FindName("btnDetail")));
            this.radDataPager = ((Telerik.Windows.Controls.RadDataPager)(this.FindName("radDataPager")));
            this.GridViewComp = ((Telerik.Windows.Controls.RadGridView)(this.FindName("GridViewComp")));
            this.Expander1 = ((System.Windows.Controls.Expander)(this.FindName("Expander1")));
            this.gridchat = ((System.Windows.Controls.Grid)(this.FindName("gridchat")));
            this.HideRow0 = ((System.Windows.Controls.RowDefinition)(this.FindName("HideRow0")));
            this.RadChartPie = ((Telerik.Windows.Controls.RadChart)(this.FindName("RadChartPie")));
        }
    }
}

