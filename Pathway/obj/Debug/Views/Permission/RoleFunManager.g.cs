﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\Views\Permission\RoleFunManager.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3F12AD02FB0C7B36962434D6CCA4C0E5"
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


namespace YidanEHRApplication.Views.Permission {
    
    
    public partial class RoleFunManager : System.Windows.Controls.Page {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Telerik.Windows.Controls.RadGridView GridView;
        
        internal System.Windows.Controls.TextBlock textBlockb;
        
        internal System.Windows.Controls.ScrollViewer PageScrollViewer;
        
        internal Telerik.Windows.Controls.RadGridView GridViewFun;
        
        internal System.Windows.Controls.Button btnSave;
        
        internal System.Windows.Controls.Button btnClear;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/Views/Permission/RoleFunManager.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.GridView = ((Telerik.Windows.Controls.RadGridView)(this.FindName("GridView")));
            this.textBlockb = ((System.Windows.Controls.TextBlock)(this.FindName("textBlockb")));
            this.PageScrollViewer = ((System.Windows.Controls.ScrollViewer)(this.FindName("PageScrollViewer")));
            this.GridViewFun = ((Telerik.Windows.Controls.RadGridView)(this.FindName("GridViewFun")));
            this.btnSave = ((System.Windows.Controls.Button)(this.FindName("btnSave")));
            this.btnClear = ((System.Windows.Controls.Button)(this.FindName("btnClear")));
        }
    }
}

