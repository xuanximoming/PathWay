﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\Views\PathManager.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "EAB171041735E8F039C5A4D29CCEF8F4"
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
    
    
    public partial class PathManager : System.Windows.Controls.Page {
        
        internal Telerik.Windows.Controls.RadBusyIndicator radBusyIndicator;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Telerik.Windows.Controls.GroupBox groupBox2;
        
        internal System.Windows.Controls.AutoCompleteBox autoCompleteBoxQueryDept;
        
        internal System.Windows.Controls.AutoCompleteBox autoCompleteBoxQueryPath;
        
        internal Telerik.Windows.Controls.RadComboBox radCmbYxjl;
        
        internal Telerik.Windows.Controls.RadDateTimePicker dateTimeFrom;
        
        internal Telerik.Windows.Controls.RadDateTimePicker dateTimeTo;
        
        internal System.Windows.Controls.Button radbuttonQuery;
        
        internal System.Windows.Controls.Button btnCl;
        
        internal Telerik.Windows.Controls.GroupBox groupBox3;
        
        internal Telerik.Windows.Controls.RadGridView radGridViewPathList;
        
        internal System.Windows.Controls.Button btnAdd;
        
        internal System.Windows.Controls.Button btnUpdate;
        
        internal System.Windows.Controls.Button btnConfig;
        
        internal System.Windows.Controls.Button btnDetail;
        
        internal System.Windows.Controls.Button btnCopy;
        
        internal Telerik.Windows.Controls.GroupBox groupBox1;
        
        internal System.Windows.Controls.TextBox textBoxPathName;
        
        internal Telerik.Windows.Controls.RadNumericUpDown radNumericUpDownVersion;
        
        internal Telerik.Windows.Controls.RadNumericUpDown radNumericUpDownInDays;
        
        internal Telerik.Windows.Controls.RadComboBox radComboBoxStatus;
        
        internal System.Windows.Controls.AutoCompleteBox autoCompleteBoxDept;
        
        internal Telerik.Windows.Controls.RadNumericUpDown radNumericUpDownAvgFee;
        
        internal System.Windows.Controls.Button btnSave;
        
        internal System.Windows.Controls.Button btnReset;
        
        internal System.Windows.Controls.Button btnClear;
        
        internal System.Windows.Controls.Label label1;
        
        internal System.Windows.Controls.Label label2;
        
        internal System.Windows.Controls.Label label3;
        
        internal System.Windows.Controls.Label label4;
        
        internal System.Windows.Controls.Label label5;
        
        internal System.Windows.Controls.Label label6;
        
        internal System.Windows.Controls.Label label7;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/Views/PathManager.xaml", System.UriKind.Relative));
            this.radBusyIndicator = ((Telerik.Windows.Controls.RadBusyIndicator)(this.FindName("radBusyIndicator")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.groupBox2 = ((Telerik.Windows.Controls.GroupBox)(this.FindName("groupBox2")));
            this.autoCompleteBoxQueryDept = ((System.Windows.Controls.AutoCompleteBox)(this.FindName("autoCompleteBoxQueryDept")));
            this.autoCompleteBoxQueryPath = ((System.Windows.Controls.AutoCompleteBox)(this.FindName("autoCompleteBoxQueryPath")));
            this.radCmbYxjl = ((Telerik.Windows.Controls.RadComboBox)(this.FindName("radCmbYxjl")));
            this.dateTimeFrom = ((Telerik.Windows.Controls.RadDateTimePicker)(this.FindName("dateTimeFrom")));
            this.dateTimeTo = ((Telerik.Windows.Controls.RadDateTimePicker)(this.FindName("dateTimeTo")));
            this.radbuttonQuery = ((System.Windows.Controls.Button)(this.FindName("radbuttonQuery")));
            this.btnCl = ((System.Windows.Controls.Button)(this.FindName("btnCl")));
            this.groupBox3 = ((Telerik.Windows.Controls.GroupBox)(this.FindName("groupBox3")));
            this.radGridViewPathList = ((Telerik.Windows.Controls.RadGridView)(this.FindName("radGridViewPathList")));
            this.btnAdd = ((System.Windows.Controls.Button)(this.FindName("btnAdd")));
            this.btnUpdate = ((System.Windows.Controls.Button)(this.FindName("btnUpdate")));
            this.btnConfig = ((System.Windows.Controls.Button)(this.FindName("btnConfig")));
            this.btnDetail = ((System.Windows.Controls.Button)(this.FindName("btnDetail")));
            this.btnCopy = ((System.Windows.Controls.Button)(this.FindName("btnCopy")));
            this.groupBox1 = ((Telerik.Windows.Controls.GroupBox)(this.FindName("groupBox1")));
            this.textBoxPathName = ((System.Windows.Controls.TextBox)(this.FindName("textBoxPathName")));
            this.radNumericUpDownVersion = ((Telerik.Windows.Controls.RadNumericUpDown)(this.FindName("radNumericUpDownVersion")));
            this.radNumericUpDownInDays = ((Telerik.Windows.Controls.RadNumericUpDown)(this.FindName("radNumericUpDownInDays")));
            this.radComboBoxStatus = ((Telerik.Windows.Controls.RadComboBox)(this.FindName("radComboBoxStatus")));
            this.autoCompleteBoxDept = ((System.Windows.Controls.AutoCompleteBox)(this.FindName("autoCompleteBoxDept")));
            this.radNumericUpDownAvgFee = ((Telerik.Windows.Controls.RadNumericUpDown)(this.FindName("radNumericUpDownAvgFee")));
            this.btnSave = ((System.Windows.Controls.Button)(this.FindName("btnSave")));
            this.btnReset = ((System.Windows.Controls.Button)(this.FindName("btnReset")));
            this.btnClear = ((System.Windows.Controls.Button)(this.FindName("btnClear")));
            this.label1 = ((System.Windows.Controls.Label)(this.FindName("label1")));
            this.label2 = ((System.Windows.Controls.Label)(this.FindName("label2")));
            this.label3 = ((System.Windows.Controls.Label)(this.FindName("label3")));
            this.label4 = ((System.Windows.Controls.Label)(this.FindName("label4")));
            this.label5 = ((System.Windows.Controls.Label)(this.FindName("label5")));
            this.label6 = ((System.Windows.Controls.Label)(this.FindName("label6")));
            this.label7 = ((System.Windows.Controls.Label)(this.FindName("label7")));
        }
    }
}

