﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\Views\RWQueryPathExecuteNoteCompare.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "19A00F3E7BE8119FA2C89E71CD1D2379"
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
    
    
    public partial class RWQueryPathExecuteNoteCompare : Telerik.Windows.Controls.RadWindow {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock txbBRQK;
        
        internal Telerik.Windows.Controls.RadButton btnQuit;
        
        internal Telerik.Windows.Controls.RadButton btnComplete;
        
        internal Telerik.Windows.Controls.RadGridView GridView;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/Views/RWQueryPathExecuteNoteCompare.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.txbBRQK = ((System.Windows.Controls.TextBlock)(this.FindName("txbBRQK")));
            this.btnQuit = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnQuit")));
            this.btnComplete = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnComplete")));
            this.GridView = ((Telerik.Windows.Controls.RadGridView)(this.FindName("GridView")));
        }
    }
}

