﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\WorkFlow\Control\ActivitySetting.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "2CBE331D7EAFB148450F75A212DC8257"
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


namespace YidanEHRApplication.WorkFlow.Control {
    
    
    public partial class ActivitySetting : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Canvas LayoutRoot;
        
        internal System.Windows.Media.Animation.Storyboard sbActivitySettingDisplay;
        
        internal System.Windows.Media.Animation.Storyboard sbActivitySettingClose;
        
        internal System.Windows.Controls.Grid gridContainer;
        
        internal System.Windows.Controls.TextBlock tbActivityName;
        
        internal System.Windows.Controls.TextBlock tbActivityType;
        
        internal System.Windows.Controls.TextBox txtActivityName;
        
        internal System.Windows.Controls.ComboBox cbActivityType;
        
        internal System.Windows.Controls.TextBlock tbMergePictureRepeatDirection;
        
        internal System.Windows.Controls.ComboBox cbMergePictureRepeatDirection;
        
        internal System.Windows.Controls.TextBlock btSubFlow;
        
        internal System.Windows.Controls.ComboBox cbSubFlowList;
        
        internal System.Windows.Controls.Button btnSave;
        
        internal System.Windows.Controls.Button btnAppay;
        
        internal System.Windows.Controls.Button btnClose;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/WorkFlow/Control/ActivitySetting.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Canvas)(this.FindName("LayoutRoot")));
            this.sbActivitySettingDisplay = ((System.Windows.Media.Animation.Storyboard)(this.FindName("sbActivitySettingDisplay")));
            this.sbActivitySettingClose = ((System.Windows.Media.Animation.Storyboard)(this.FindName("sbActivitySettingClose")));
            this.gridContainer = ((System.Windows.Controls.Grid)(this.FindName("gridContainer")));
            this.tbActivityName = ((System.Windows.Controls.TextBlock)(this.FindName("tbActivityName")));
            this.tbActivityType = ((System.Windows.Controls.TextBlock)(this.FindName("tbActivityType")));
            this.txtActivityName = ((System.Windows.Controls.TextBox)(this.FindName("txtActivityName")));
            this.cbActivityType = ((System.Windows.Controls.ComboBox)(this.FindName("cbActivityType")));
            this.tbMergePictureRepeatDirection = ((System.Windows.Controls.TextBlock)(this.FindName("tbMergePictureRepeatDirection")));
            this.cbMergePictureRepeatDirection = ((System.Windows.Controls.ComboBox)(this.FindName("cbMergePictureRepeatDirection")));
            this.btSubFlow = ((System.Windows.Controls.TextBlock)(this.FindName("btSubFlow")));
            this.cbSubFlowList = ((System.Windows.Controls.ComboBox)(this.FindName("cbSubFlowList")));
            this.btnSave = ((System.Windows.Controls.Button)(this.FindName("btnSave")));
            this.btnAppay = ((System.Windows.Controls.Button)(this.FindName("btnAppay")));
            this.btnClose = ((System.Windows.Controls.Button)(this.FindName("btnClose")));
        }
    }
}

