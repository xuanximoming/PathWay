﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\ChildWindows\RWAccessNode.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "C939FE1BC59336E4CE06879F9D1DF8F2"
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


namespace YidanEHRApplication {
    
    
    public partial class RWAccessNode : Telerik.Windows.Controls.RadWindow {
        
        internal Telerik.Windows.Controls.RadBusyIndicator radBusyIndicator;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.Grid CheckPg;
        
        internal Telerik.Windows.Controls.RadGridView ConditionGrid;
        
        internal Telerik.Windows.Controls.RadGridView ConditionGridHand;
        
        internal System.Windows.Controls.StackPanel stkReason;
        
        internal System.Windows.Controls.TextBox txtReason;
        
        internal System.Windows.Controls.Button btnAccess;
        
        internal System.Windows.Controls.Button btnCancel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/ChildWindows/RWAccessNode.xaml", System.UriKind.Relative));
            this.radBusyIndicator = ((Telerik.Windows.Controls.RadBusyIndicator)(this.FindName("radBusyIndicator")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.CheckPg = ((System.Windows.Controls.Grid)(this.FindName("CheckPg")));
            this.ConditionGrid = ((Telerik.Windows.Controls.RadGridView)(this.FindName("ConditionGrid")));
            this.ConditionGridHand = ((Telerik.Windows.Controls.RadGridView)(this.FindName("ConditionGridHand")));
            this.stkReason = ((System.Windows.Controls.StackPanel)(this.FindName("stkReason")));
            this.txtReason = ((System.Windows.Controls.TextBox)(this.FindName("txtReason")));
            this.btnAccess = ((System.Windows.Controls.Button)(this.FindName("btnAccess")));
            this.btnCancel = ((System.Windows.Controls.Button)(this.FindName("btnCancel")));
        }
    }
}

