﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\Views\RWPathVariationClassCode.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "86B4060C91CEAA3CB5B081E67ABEF031"
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
    
    
    public partial class RWPathVariationClassCode : Telerik.Windows.Controls.RadWindow {
        
        internal Telerik.Windows.Controls.RadBusyIndicator radBusyIndicator;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Telerik.Windows.Controls.RadComboBox cbxVariationType;
        
        internal System.Windows.Controls.Button btnRefresh;
        
        internal System.Windows.Controls.Label lbQuery;
        
        internal System.Windows.Controls.TextBox tbQuery;
        
        internal System.Windows.Controls.Button btnQuery;
        
        internal System.Windows.Controls.Button btnReset;
        
        internal Telerik.Windows.Controls.RadGridView GridViewVariantRecords;
        
        internal System.Windows.Controls.CheckBox checkBoxAll;
        
        internal Telerik.Windows.Controls.RadDataPager radDataPager;
        
        internal System.Windows.Controls.Button CancelButton;
        
        internal System.Windows.Controls.Button btnClassCode;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/Views/RWPathVariationClassCode.xaml", System.UriKind.Relative));
            this.radBusyIndicator = ((Telerik.Windows.Controls.RadBusyIndicator)(this.FindName("radBusyIndicator")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.cbxVariationType = ((Telerik.Windows.Controls.RadComboBox)(this.FindName("cbxVariationType")));
            this.btnRefresh = ((System.Windows.Controls.Button)(this.FindName("btnRefresh")));
            this.lbQuery = ((System.Windows.Controls.Label)(this.FindName("lbQuery")));
            this.tbQuery = ((System.Windows.Controls.TextBox)(this.FindName("tbQuery")));
            this.btnQuery = ((System.Windows.Controls.Button)(this.FindName("btnQuery")));
            this.btnReset = ((System.Windows.Controls.Button)(this.FindName("btnReset")));
            this.GridViewVariantRecords = ((Telerik.Windows.Controls.RadGridView)(this.FindName("GridViewVariantRecords")));
            this.checkBoxAll = ((System.Windows.Controls.CheckBox)(this.FindName("checkBoxAll")));
            this.radDataPager = ((Telerik.Windows.Controls.RadDataPager)(this.FindName("radDataPager")));
            this.CancelButton = ((System.Windows.Controls.Button)(this.FindName("CancelButton")));
            this.btnClassCode = ((System.Windows.Controls.Button)(this.FindName("btnClassCode")));
        }
    }
}

