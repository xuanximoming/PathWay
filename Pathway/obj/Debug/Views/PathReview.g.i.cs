﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\Views\PathReview.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "87DF69E2E0CEB73061258CB45C579D8C"
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
    
    
    public partial class PathReview : System.Windows.Controls.Page {
        
        internal Telerik.Windows.Controls.RadBusyIndicator radBusyIndicator;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Telerik.Windows.Controls.GroupBox groupBox2;
        
        internal System.Windows.Controls.AutoCompleteBox autoCompleteBoxQueryDept;
        
        internal System.Windows.Controls.AutoCompleteBox autoCompleteBoxPath;
        
        internal Telerik.Windows.Controls.RadComboBox radCmbYxjl;
        
        internal Telerik.Windows.Controls.RadDateTimePicker dateTimeFrom;
        
        internal Telerik.Windows.Controls.RadDateTimePicker dateTimeTo;
        
        internal System.Windows.Controls.Button radbuttonQuery;
        
        internal System.Windows.Controls.Button btnCl;
        
        internal Telerik.Windows.Controls.GroupBox groupBox3;
        
        internal Telerik.Windows.Controls.RadGridView radGridViewPathList;
        
        internal System.Windows.Controls.Button btnReView;
        
        internal System.Windows.Controls.Button btnAntiReview;
        
        internal System.Windows.Controls.Button btnStop;
        
        internal System.Windows.Controls.Button btnDetail;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/Views/PathReview.xaml", System.UriKind.Relative));
            this.radBusyIndicator = ((Telerik.Windows.Controls.RadBusyIndicator)(this.FindName("radBusyIndicator")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.groupBox2 = ((Telerik.Windows.Controls.GroupBox)(this.FindName("groupBox2")));
            this.autoCompleteBoxQueryDept = ((System.Windows.Controls.AutoCompleteBox)(this.FindName("autoCompleteBoxQueryDept")));
            this.autoCompleteBoxPath = ((System.Windows.Controls.AutoCompleteBox)(this.FindName("autoCompleteBoxPath")));
            this.radCmbYxjl = ((Telerik.Windows.Controls.RadComboBox)(this.FindName("radCmbYxjl")));
            this.dateTimeFrom = ((Telerik.Windows.Controls.RadDateTimePicker)(this.FindName("dateTimeFrom")));
            this.dateTimeTo = ((Telerik.Windows.Controls.RadDateTimePicker)(this.FindName("dateTimeTo")));
            this.radbuttonQuery = ((System.Windows.Controls.Button)(this.FindName("radbuttonQuery")));
            this.btnCl = ((System.Windows.Controls.Button)(this.FindName("btnCl")));
            this.groupBox3 = ((Telerik.Windows.Controls.GroupBox)(this.FindName("groupBox3")));
            this.radGridViewPathList = ((Telerik.Windows.Controls.RadGridView)(this.FindName("radGridViewPathList")));
            this.btnReView = ((System.Windows.Controls.Button)(this.FindName("btnReView")));
            this.btnAntiReview = ((System.Windows.Controls.Button)(this.FindName("btnAntiReview")));
            this.btnStop = ((System.Windows.Controls.Button)(this.FindName("btnStop")));
            this.btnDetail = ((System.Windows.Controls.Button)(this.FindName("btnDetail")));
        }
    }
}

