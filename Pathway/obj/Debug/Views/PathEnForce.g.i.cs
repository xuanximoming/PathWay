﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\Views\PathEnForce.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "6906C0D6724DAC5186DC04E2F05B0CBF"
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
using YidanEHRApplication.Views.UserControls;


namespace YidanEHRApplication.Views {
    
    
    public partial class PathEnForce : System.Windows.Controls.Page {
        
        internal Telerik.Windows.Controls.RadBusyIndicator radBusyIndicator;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal System.Windows.Controls.TextBlock textname;
        
        internal Telerik.Windows.Controls.RadComboBox autoDiagName;
        
        internal System.Windows.Controls.TextBlock txtZyhm;
        
        internal System.Windows.Controls.TextBlock txtBrxb;
        
        internal System.Windows.Controls.TextBlock txtXsnl;
        
        internal System.Windows.Controls.TextBlock txtCsrq;
        
        internal System.Windows.Controls.TextBlock txtRyrq;
        
        internal System.Windows.Controls.TextBlock txtDays;
        
        internal System.Windows.Controls.TextBlock txtRyzd;
        
        internal Telerik.Windows.Controls.RadButton btnLongOrderPrint;
        
        internal Telerik.Windows.Controls.RadButton btnTempOrderPrint;
        
        internal Telerik.Windows.Controls.RadButton btnLeadIn;
        
        internal System.Windows.Controls.TextBlock textBlockPath;
        
        internal System.Windows.Controls.Expander expShowImage;
        
        internal System.Windows.Controls.Button btnShowDetail;
        
        internal System.Windows.Controls.Button btn_PathSummary;
        
        internal System.Windows.Controls.Button btnAdviceList;
        
        internal System.Windows.Controls.Button btnSave;
        
        internal System.Windows.Controls.Button btnSendOrder;
        
        internal System.Windows.Controls.Button btnNext;
        
        internal System.Windows.Controls.Button btnComplete;
        
        internal System.Windows.Controls.Button btnViewPre;
        
        internal System.Windows.Controls.Button btnViewNext;
        
        internal System.Windows.Controls.TextBlock textBlockLock;
        
        internal System.Windows.Controls.Button btnQuit;
        
        internal System.Windows.Controls.Canvas cnvDragContain;
        
        internal System.Windows.Controls.Canvas cnvDrag;
        
        internal System.Windows.Controls.Canvas cnvDragTitle;
        
        internal System.Windows.Controls.Grid gridWorkFlowShow;
        
        internal System.Windows.Controls.RadioButton chkKeShi;
        
        internal System.Windows.Controls.RadioButton chkGeRen;
        
        internal System.Windows.Controls.ListBox lstAdviceSuit;
        
        internal Telerik.Windows.Controls.RadTreeView tvList;
        
        internal Telerik.Windows.Controls.RadTabControl radTabControlPathManager;
        
        internal Telerik.Windows.Controls.RadTabItem Order;
        
        internal Telerik.Windows.Controls.RadGridView radGridViewOrderList;
        
        internal Telerik.Windows.Controls.RadButton btnNewOrder;
        
        internal Telerik.Windows.Controls.RadButton btnModifyOrder;
        
        internal Telerik.Windows.Controls.RadButton btnDelOrder;
        
        internal Telerik.Windows.Controls.GroupBox groupBox1;
        
        internal System.Windows.Controls.Grid radioButtonGrid;
        
        internal System.Windows.Controls.RadioButton radioDrug;
        
        internal System.Windows.Controls.RadioButton radioRisLis;
        
        internal System.Windows.Controls.RadioButton radioMeal;
        
        internal System.Windows.Controls.RadioButton radioObservation;
        
        internal System.Windows.Controls.RadioButton radioActivity;
        
        internal System.Windows.Controls.RadioButton radioChunOrder;
        
        internal System.Windows.Controls.RadioButton radioCare;
        
        internal System.Windows.Controls.RadioButton radioOther;
        
        internal System.Windows.Controls.RadioButton radioOper;
        
        internal System.Windows.Controls.Grid drugGrid;
        
        internal YidanEHRApplication.Views.UserControls.UCDrug drugOrderControl;
        
        internal System.Windows.Controls.Grid rislisGrid;
        
        internal YidanEHRApplication.Views.UserControls.UCRISLISOrder risLisOrderControl;
        
        internal System.Windows.Controls.Grid mealGrid;
        
        internal YidanEHRApplication.Views.UserControls.UCOtherOrder foodOrderControl;
        
        internal System.Windows.Controls.Grid observationGrid;
        
        internal YidanEHRApplication.Views.UserControls.UCOtherOrder observationOrderControl;
        
        internal System.Windows.Controls.Grid activityGrid;
        
        internal YidanEHRApplication.Views.UserControls.UCOtherOrder activityOrderControl;
        
        internal System.Windows.Controls.Grid chunGrid;
        
        internal YidanEHRApplication.Views.UserControls.UCChunOrder chunOrderControl;
        
        internal System.Windows.Controls.Grid careGrid;
        
        internal YidanEHRApplication.Views.UserControls.UCOtherOrder careOrderControl;
        
        internal System.Windows.Controls.Grid OtherGrid;
        
        internal YidanEHRApplication.Views.UserControls.UCOtherOrder OtherOrderControl;
        
        internal System.Windows.Controls.Grid operGrid;
        
        internal Telerik.Windows.Controls.RadTabItem CyOrder;
        
        internal Telerik.Windows.Controls.RadGridView radGridViewCYOrderList;
        
        internal Telerik.Windows.Controls.GroupBox groupBox2;
        
        internal System.Windows.Controls.Grid radioButtonGridCY;
        
        internal System.Windows.Controls.RadioButton radioCYOrder;
        
        internal System.Windows.Controls.Grid drugGridCY;
        
        internal YidanEHRApplication.Views.UserControls.UCCyXDF CyfOrderControl;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/Views/PathEnForce.xaml", System.UriKind.Relative));
            this.radBusyIndicator = ((Telerik.Windows.Controls.RadBusyIndicator)(this.FindName("radBusyIndicator")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.textname = ((System.Windows.Controls.TextBlock)(this.FindName("textname")));
            this.autoDiagName = ((Telerik.Windows.Controls.RadComboBox)(this.FindName("autoDiagName")));
            this.txtZyhm = ((System.Windows.Controls.TextBlock)(this.FindName("txtZyhm")));
            this.txtBrxb = ((System.Windows.Controls.TextBlock)(this.FindName("txtBrxb")));
            this.txtXsnl = ((System.Windows.Controls.TextBlock)(this.FindName("txtXsnl")));
            this.txtCsrq = ((System.Windows.Controls.TextBlock)(this.FindName("txtCsrq")));
            this.txtRyrq = ((System.Windows.Controls.TextBlock)(this.FindName("txtRyrq")));
            this.txtDays = ((System.Windows.Controls.TextBlock)(this.FindName("txtDays")));
            this.txtRyzd = ((System.Windows.Controls.TextBlock)(this.FindName("txtRyzd")));
            this.btnLongOrderPrint = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnLongOrderPrint")));
            this.btnTempOrderPrint = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnTempOrderPrint")));
            this.btnLeadIn = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnLeadIn")));
            this.textBlockPath = ((System.Windows.Controls.TextBlock)(this.FindName("textBlockPath")));
            this.expShowImage = ((System.Windows.Controls.Expander)(this.FindName("expShowImage")));
            this.btnShowDetail = ((System.Windows.Controls.Button)(this.FindName("btnShowDetail")));
            this.btn_PathSummary = ((System.Windows.Controls.Button)(this.FindName("btn_PathSummary")));
            this.btnAdviceList = ((System.Windows.Controls.Button)(this.FindName("btnAdviceList")));
            this.btnSave = ((System.Windows.Controls.Button)(this.FindName("btnSave")));
            this.btnSendOrder = ((System.Windows.Controls.Button)(this.FindName("btnSendOrder")));
            this.btnNext = ((System.Windows.Controls.Button)(this.FindName("btnNext")));
            this.btnComplete = ((System.Windows.Controls.Button)(this.FindName("btnComplete")));
            this.btnViewPre = ((System.Windows.Controls.Button)(this.FindName("btnViewPre")));
            this.btnViewNext = ((System.Windows.Controls.Button)(this.FindName("btnViewNext")));
            this.textBlockLock = ((System.Windows.Controls.TextBlock)(this.FindName("textBlockLock")));
            this.btnQuit = ((System.Windows.Controls.Button)(this.FindName("btnQuit")));
            this.cnvDragContain = ((System.Windows.Controls.Canvas)(this.FindName("cnvDragContain")));
            this.cnvDrag = ((System.Windows.Controls.Canvas)(this.FindName("cnvDrag")));
            this.cnvDragTitle = ((System.Windows.Controls.Canvas)(this.FindName("cnvDragTitle")));
            this.gridWorkFlowShow = ((System.Windows.Controls.Grid)(this.FindName("gridWorkFlowShow")));
            this.chkKeShi = ((System.Windows.Controls.RadioButton)(this.FindName("chkKeShi")));
            this.chkGeRen = ((System.Windows.Controls.RadioButton)(this.FindName("chkGeRen")));
            this.lstAdviceSuit = ((System.Windows.Controls.ListBox)(this.FindName("lstAdviceSuit")));
            this.tvList = ((Telerik.Windows.Controls.RadTreeView)(this.FindName("tvList")));
            this.radTabControlPathManager = ((Telerik.Windows.Controls.RadTabControl)(this.FindName("radTabControlPathManager")));
            this.Order = ((Telerik.Windows.Controls.RadTabItem)(this.FindName("Order")));
            this.radGridViewOrderList = ((Telerik.Windows.Controls.RadGridView)(this.FindName("radGridViewOrderList")));
            this.btnNewOrder = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnNewOrder")));
            this.btnModifyOrder = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnModifyOrder")));
            this.btnDelOrder = ((Telerik.Windows.Controls.RadButton)(this.FindName("btnDelOrder")));
            this.groupBox1 = ((Telerik.Windows.Controls.GroupBox)(this.FindName("groupBox1")));
            this.radioButtonGrid = ((System.Windows.Controls.Grid)(this.FindName("radioButtonGrid")));
            this.radioDrug = ((System.Windows.Controls.RadioButton)(this.FindName("radioDrug")));
            this.radioRisLis = ((System.Windows.Controls.RadioButton)(this.FindName("radioRisLis")));
            this.radioMeal = ((System.Windows.Controls.RadioButton)(this.FindName("radioMeal")));
            this.radioObservation = ((System.Windows.Controls.RadioButton)(this.FindName("radioObservation")));
            this.radioActivity = ((System.Windows.Controls.RadioButton)(this.FindName("radioActivity")));
            this.radioChunOrder = ((System.Windows.Controls.RadioButton)(this.FindName("radioChunOrder")));
            this.radioCare = ((System.Windows.Controls.RadioButton)(this.FindName("radioCare")));
            this.radioOther = ((System.Windows.Controls.RadioButton)(this.FindName("radioOther")));
            this.radioOper = ((System.Windows.Controls.RadioButton)(this.FindName("radioOper")));
            this.drugGrid = ((System.Windows.Controls.Grid)(this.FindName("drugGrid")));
            this.drugOrderControl = ((YidanEHRApplication.Views.UserControls.UCDrug)(this.FindName("drugOrderControl")));
            this.rislisGrid = ((System.Windows.Controls.Grid)(this.FindName("rislisGrid")));
            this.risLisOrderControl = ((YidanEHRApplication.Views.UserControls.UCRISLISOrder)(this.FindName("risLisOrderControl")));
            this.mealGrid = ((System.Windows.Controls.Grid)(this.FindName("mealGrid")));
            this.foodOrderControl = ((YidanEHRApplication.Views.UserControls.UCOtherOrder)(this.FindName("foodOrderControl")));
            this.observationGrid = ((System.Windows.Controls.Grid)(this.FindName("observationGrid")));
            this.observationOrderControl = ((YidanEHRApplication.Views.UserControls.UCOtherOrder)(this.FindName("observationOrderControl")));
            this.activityGrid = ((System.Windows.Controls.Grid)(this.FindName("activityGrid")));
            this.activityOrderControl = ((YidanEHRApplication.Views.UserControls.UCOtherOrder)(this.FindName("activityOrderControl")));
            this.chunGrid = ((System.Windows.Controls.Grid)(this.FindName("chunGrid")));
            this.chunOrderControl = ((YidanEHRApplication.Views.UserControls.UCChunOrder)(this.FindName("chunOrderControl")));
            this.careGrid = ((System.Windows.Controls.Grid)(this.FindName("careGrid")));
            this.careOrderControl = ((YidanEHRApplication.Views.UserControls.UCOtherOrder)(this.FindName("careOrderControl")));
            this.OtherGrid = ((System.Windows.Controls.Grid)(this.FindName("OtherGrid")));
            this.OtherOrderControl = ((YidanEHRApplication.Views.UserControls.UCOtherOrder)(this.FindName("OtherOrderControl")));
            this.operGrid = ((System.Windows.Controls.Grid)(this.FindName("operGrid")));
            this.CyOrder = ((Telerik.Windows.Controls.RadTabItem)(this.FindName("CyOrder")));
            this.radGridViewCYOrderList = ((Telerik.Windows.Controls.RadGridView)(this.FindName("radGridViewCYOrderList")));
            this.groupBox2 = ((Telerik.Windows.Controls.GroupBox)(this.FindName("groupBox2")));
            this.radioButtonGridCY = ((System.Windows.Controls.Grid)(this.FindName("radioButtonGridCY")));
            this.radioCYOrder = ((System.Windows.Controls.RadioButton)(this.FindName("radioCYOrder")));
            this.drugGridCY = ((System.Windows.Controls.Grid)(this.FindName("drugGridCY")));
            this.CyfOrderControl = ((YidanEHRApplication.Views.UserControls.UCCyXDF)(this.FindName("CyfOrderControl")));
        }
    }
}

