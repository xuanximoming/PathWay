﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\WorkFlow\Designer\Rule.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "5EF6B37254230661A9413BC821AF5743"
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
using YidanEHRApplication.WorkFlow.Designer;


namespace YidanEHRApplication.WorkFlow.Designer {
    
    
    public partial class Rule : System.Windows.Controls.UserControl {
        
        internal System.Windows.Controls.Canvas cnRuleContainer;
        
        internal System.Windows.Controls.ToolTip ttRuleTip;
        
        internal System.Windows.Media.Animation.Storyboard sbBeginDisplay;
        
        internal System.Windows.Media.Animation.Storyboard sbBeginClose;
        
        internal System.Windows.Shapes.Ellipse begin;
        
        internal System.Windows.Shapes.Polyline line;
        
        internal System.Windows.Controls.Canvas end;
        
        internal YidanEHRApplication.WorkFlow.Designer.Arrowhead endArrow;
        
        internal System.Windows.Shapes.Ellipse endEllipse;
        
        internal System.Windows.Controls.TextBlock tbRuleName;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/WorkFlow/Designer/Rule.xaml", System.UriKind.Relative));
            this.cnRuleContainer = ((System.Windows.Controls.Canvas)(this.FindName("cnRuleContainer")));
            this.ttRuleTip = ((System.Windows.Controls.ToolTip)(this.FindName("ttRuleTip")));
            this.sbBeginDisplay = ((System.Windows.Media.Animation.Storyboard)(this.FindName("sbBeginDisplay")));
            this.sbBeginClose = ((System.Windows.Media.Animation.Storyboard)(this.FindName("sbBeginClose")));
            this.begin = ((System.Windows.Shapes.Ellipse)(this.FindName("begin")));
            this.line = ((System.Windows.Shapes.Polyline)(this.FindName("line")));
            this.end = ((System.Windows.Controls.Canvas)(this.FindName("end")));
            this.endArrow = ((YidanEHRApplication.WorkFlow.Designer.Arrowhead)(this.FindName("endArrow")));
            this.endEllipse = ((System.Windows.Shapes.Ellipse)(this.FindName("endEllipse")));
            this.tbRuleName = ((System.Windows.Controls.TextBlock)(this.FindName("tbRuleName")));
        }
    }
}

