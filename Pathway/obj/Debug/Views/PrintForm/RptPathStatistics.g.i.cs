﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\Views\PrintForm\RptPathStatistics.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "D312EDEDD018C7FC5E7879CC0DE4B939"
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
    
    
    public partial class RptPathStatistics : System.Windows.Controls.Page {
        
        internal Telerik.Windows.Controls.RadBusyIndicator radBusyIndicator;
        
        internal System.Windows.Controls.ScrollViewer PageScrollViewer;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Telerik.Windows.Controls.RadDateTimePicker dtpStartDate;
        
        internal Telerik.Windows.Controls.RadDateTimePicker dtpEndDate;
        
        internal Telerik.Windows.Controls.RadComboBox cmbOffice;
        
        internal Telerik.Windows.Controls.RadComboBox cmbPath;
        
        internal System.Windows.Controls.Button btnStat;
        
        internal System.Windows.Controls.Button btnClear;
        
        internal System.Windows.Controls.Button btnPrint;
        
        internal System.Windows.Controls.Button btnDetail;
        
        internal Telerik.Windows.Controls.RadDataPager radDataPager;
        
        internal Telerik.Windows.Controls.RadGridView GridViewPathStat;
        
        internal System.Windows.Controls.Expander Expander1;
        
        internal System.Windows.Controls.Expander Expander2;
        
        internal System.Windows.Controls.Expander Expander3;
        
        internal System.Windows.Controls.Expander Expander4;
        
        internal System.Windows.Controls.Grid gridchat;
        
        internal System.Windows.Controls.RowDefinition HideRow0;
        
        internal System.Windows.Controls.RowDefinition HideRow1;
        
        internal System.Windows.Controls.RowDefinition HideRow2;
        
        internal Telerik.Windows.Controls.RadChart RadChartFee;
        
        internal Telerik.Windows.Controls.RadChart RadChartDays;
        
        internal Telerik.Windows.Controls.RadChart RadChartRate;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/Views/PrintForm/RptPathStatistics.xaml", System.UriKind.Relative));
            this.radBusyIndicator = ((Telerik.Windows.Controls.RadBusyIndicator)(this.FindName("radBusyIndicator")));
            this.PageScrollViewer = ((System.Windows.Controls.ScrollViewer)(this.FindName("PageScrollViewer")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.dtpStartDate = ((Telerik.Windows.Controls.RadDateTimePicker)(this.FindName("dtpStartDate")));
            this.dtpEndDate = ((Telerik.Windows.Controls.RadDateTimePicker)(this.FindName("dtpEndDate")));
            this.cmbOffice = ((Telerik.Windows.Controls.RadComboBox)(this.FindName("cmbOffice")));
            this.cmbPath = ((Telerik.Windows.Controls.RadComboBox)(this.FindName("cmbPath")));
            this.btnStat = ((System.Windows.Controls.Button)(this.FindName("btnStat")));
            this.btnClear = ((System.Windows.Controls.Button)(this.FindName("btnClear")));
            this.btnPrint = ((System.Windows.Controls.Button)(this.FindName("btnPrint")));
            this.btnDetail = ((System.Windows.Controls.Button)(this.FindName("btnDetail")));
            this.radDataPager = ((Telerik.Windows.Controls.RadDataPager)(this.FindName("radDataPager")));
            this.GridViewPathStat = ((Telerik.Windows.Controls.RadGridView)(this.FindName("GridViewPathStat")));
            this.Expander1 = ((System.Windows.Controls.Expander)(this.FindName("Expander1")));
            this.Expander2 = ((System.Windows.Controls.Expander)(this.FindName("Expander2")));
            this.Expander3 = ((System.Windows.Controls.Expander)(this.FindName("Expander3")));
            this.Expander4 = ((System.Windows.Controls.Expander)(this.FindName("Expander4")));
            this.gridchat = ((System.Windows.Controls.Grid)(this.FindName("gridchat")));
            this.HideRow0 = ((System.Windows.Controls.RowDefinition)(this.FindName("HideRow0")));
            this.HideRow1 = ((System.Windows.Controls.RowDefinition)(this.FindName("HideRow1")));
            this.HideRow2 = ((System.Windows.Controls.RowDefinition)(this.FindName("HideRow2")));
            this.RadChartFee = ((Telerik.Windows.Controls.RadChart)(this.FindName("RadChartFee")));
            this.RadChartDays = ((Telerik.Windows.Controls.RadChart)(this.FindName("RadChartDays")));
            this.RadChartRate = ((Telerik.Windows.Controls.RadChart)(this.FindName("RadChartRate")));
        }
    }
}

