﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\WorkFlow\Designer\Container.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "3FC9535F5E3FF0D4F3CF63655E257DEF"
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
using YidanEHRApplication.WorkFlow.Control;
using YidanEHRApplication.WorkFlow.Designer;


namespace YidanEHRApplication.WorkFlow.Designer {
    
    
    public partial class Container : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock tbWorkFlowName;
        
        internal System.Windows.Controls.TextBox txtWorkFlowName;
        
        internal Telerik.Windows.Controls.RadButton btnAddActivity;
        
        internal Telerik.Windows.Controls.RadButton btnSave;
        
        internal Telerik.Windows.Controls.RadButton btnClearContainer;
        
        internal Telerik.Windows.Controls.RadButton btnPrevious;
        
        internal Telerik.Windows.Controls.RadButton btnNext;
        
        internal Telerik.Windows.Controls.RadButton btnAddRule;
        
        internal Telerik.Windows.Controls.RadButton btnAddLabel;
        
        internal Telerik.Windows.Controls.RadButton btnExportToXml;
        
        internal Telerik.Windows.Controls.RadButton btnImportFromXml;
        
        internal System.Windows.Controls.TextBlock tbShowGridLines;
        
        internal System.Windows.Controls.CheckBox cbShowGridLines;
        
        internal System.Windows.Controls.TextBlock tbContainerWidth;
        
        internal System.Windows.Controls.Slider sliWidth;
        
        internal System.Windows.Controls.TextBlock tbContainerHeight;
        
        internal System.Windows.Controls.Slider sliHeight;
        
        internal System.Windows.Controls.TextBlock tbZoom;
        
        internal System.Windows.Controls.Slider sliZoom;
        
        internal System.Windows.Controls.TextBlock btZoomValue;
        
        internal System.Windows.Controls.ScrollViewer svContainer;
        
        internal System.Windows.Controls.Canvas cnsDesignerContainer;
        
        internal System.Windows.Controls.Canvas menuAndSetting;
        
        internal YidanEHRApplication.WorkFlow.Control.ActivitySetting siActivitySetting;
        
        internal YidanEHRApplication.WorkFlow.Control.RuleSetting siRuleSetting;
        
        internal YidanEHRApplication.WorkFlow.Designer.ActivityMenu menuActivity;
        
        internal YidanEHRApplication.WorkFlow.Designer.RuleMenu menuRule;
        
        internal YidanEHRApplication.WorkFlow.Designer.LabelMenu menuLabel;
        
        internal YidanEHRApplication.WorkFlow.Designer.ContainerMenu menuContainer;
        
        internal System.Windows.Controls.Canvas canContainerCover;
        
        internal System.Windows.Media.Animation.Storyboard sbContainerCoverDisplay;
        
        internal System.Windows.Media.Animation.Storyboard sbContainerCoverClose;
        
        internal System.Windows.Controls.Canvas MessageBody;
        
        internal System.Windows.Controls.TextBlock MessageTitle;
        
        internal System.Windows.Controls.Button btnCloseMessage;
        
        internal System.Windows.Controls.Canvas XmlContainer;
        
        internal System.Windows.Controls.TextBox txtXml;
        
        internal System.Windows.Controls.Button btnCloseXml;
        
        internal System.Windows.Controls.Button btnImportXml;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/WorkFlow/Designer/Container.xaml", System.UriKind.Relative));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.tbWorkFlowName = ((System.Windows.Controls.TextBlock)(this.FindName("tbWorkFlowName")));
            this.txtWorkFlowName = ((System.Windows.Controls.TextBox)(this.FindName("txtWorkFlowName")));
            this.btnAddActivity = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnAddActivity")));
            this.btnSave = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnSave")));
            this.btnClearContainer = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnClearContainer")));
            this.btnPrevious = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnPrevious")));
            this.btnNext = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnNext")));
            this.btnAddRule = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnAddRule")));
            this.btnAddLabel = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnAddLabel")));
            this.btnExportToXml = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnExportToXml")));
            this.btnImportFromXml = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnImportFromXml")));
            this.tbShowGridLines = ((System.Windows.Controls.TextBlock)(this.FindName("tbShowGridLines")));
            this.cbShowGridLines = ((System.Windows.Controls.CheckBox)(this.FindName("cbShowGridLines")));
            this.tbContainerWidth = ((System.Windows.Controls.TextBlock)(this.FindName("tbContainerWidth")));
            this.sliWidth = ((System.Windows.Controls.Slider)(this.FindName("sliWidth")));
            this.tbContainerHeight = ((System.Windows.Controls.TextBlock)(this.FindName("tbContainerHeight")));
            this.sliHeight = ((System.Windows.Controls.Slider)(this.FindName("sliHeight")));
            this.tbZoom = ((System.Windows.Controls.TextBlock)(this.FindName("tbZoom")));
            this.sliZoom = ((System.Windows.Controls.Slider)(this.FindName("sliZoom")));
            this.btZoomValue = ((System.Windows.Controls.TextBlock)(this.FindName("btZoomValue")));
            this.svContainer = ((System.Windows.Controls.ScrollViewer)(this.FindName("svContainer")));
            this.cnsDesignerContainer = ((System.Windows.Controls.Canvas)(this.FindName("cnsDesignerContainer")));
            this.menuAndSetting = ((System.Windows.Controls.Canvas)(this.FindName("menuAndSetting")));
            this.siActivitySetting = ((YidanEHRApplication.WorkFlow.Control.ActivitySetting)(this.FindName("siActivitySetting")));
            this.siRuleSetting = ((YidanEHRApplication.WorkFlow.Control.RuleSetting)(this.FindName("siRuleSetting")));
            this.menuActivity = ((YidanEHRApplication.WorkFlow.Designer.ActivityMenu)(this.FindName("menuActivity")));
            this.menuRule = ((YidanEHRApplication.WorkFlow.Designer.RuleMenu)(this.FindName("menuRule")));
            this.menuLabel = ((YidanEHRApplication.WorkFlow.Designer.LabelMenu)(this.FindName("menuLabel")));
            this.menuContainer = ((YidanEHRApplication.WorkFlow.Designer.ContainerMenu)(this.FindName("menuContainer")));
            this.canContainerCover = ((System.Windows.Controls.Canvas)(this.FindName("canContainerCover")));
            this.sbContainerCoverDisplay = ((System.Windows.Media.Animation.Storyboard)(this.FindName("sbContainerCoverDisplay")));
            this.sbContainerCoverClose = ((System.Windows.Media.Animation.Storyboard)(this.FindName("sbContainerCoverClose")));
            this.MessageBody = ((System.Windows.Controls.Canvas)(this.FindName("MessageBody")));
            this.MessageTitle = ((System.Windows.Controls.TextBlock)(this.FindName("MessageTitle")));
            this.btnCloseMessage = ((System.Windows.Controls.Button)(this.FindName("btnCloseMessage")));
            this.XmlContainer = ((System.Windows.Controls.Canvas)(this.FindName("XmlContainer")));
            this.txtXml = ((System.Windows.Controls.TextBox)(this.FindName("txtXml")));
            this.btnCloseXml = ((System.Windows.Controls.Button)(this.FindName("btnCloseXml")));
            this.btnImportXml = ((System.Windows.Controls.Button)(this.FindName("btnImportXml")));
        }
    }
}

