﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\Views\PrintForm\FromManager.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "ABBD469885ADB7B0D5FF84C469BD74BE"
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
    
    
    public partial class FromManager : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid Layout;
        
        internal Telerik.Windows.Controls.RadMenu radMenu;
        
        internal System.Windows.Controls.Frame ContentFrame;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/Views/PrintForm/FromManager.xaml", System.UriKind.Relative));
            this.Layout = ((System.Windows.Controls.Grid)(this.FindName("Layout")));
            this.radMenu = ((Telerik.Windows.Controls.RadMenu)(this.FindName("radMenu")));
            this.ContentFrame = ((System.Windows.Controls.Frame)(this.FindName("ContentFrame")));
        }
    }
}

