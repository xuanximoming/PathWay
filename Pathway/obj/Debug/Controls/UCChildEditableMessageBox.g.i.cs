﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\Controls\UCChildEditableMessageBox.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EBE64BF38EB80C26E443646C3A6CA8D0"
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


namespace YidanEHRApplication.Controls {
    
    
    public partial class UCChildEditableMessageBox : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock MessageInfo;
        
        internal System.Windows.Controls.TextBox InputMessage;
        
        internal Telerik.Windows.Controls.RadButton btnCancel;
        
        internal Telerik.Windows.Controls.RadButton btnOK;
        
        internal System.Windows.Controls.Label labErr;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/Controls/UCChildEditableMessageBox.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.MessageInfo = ((System.Windows.Controls.TextBlock)(this.FindName("MessageInfo")));
            this.InputMessage = ((System.Windows.Controls.TextBox)(this.FindName("InputMessage")));
            this.btnCancel = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnCancel")));
            this.btnOK = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnOK")));
            this.labErr = ((System.Windows.Controls.Label)(this.FindName("labErr")));
        }
    }
}

