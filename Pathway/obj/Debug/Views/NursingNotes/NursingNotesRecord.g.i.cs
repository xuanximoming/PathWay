﻿#pragma checksum "F:\Demo\pathway\Pathway\Pathway\Views\NursingNotes\NursingNotesRecord.xaml" "{406ea660-64cf-4c82-b6f0-42d48172a799}" "CC42746DAFD2F409566095BB4BC3DFE3"
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


namespace YidanEHRApplication.Views.NursingNotes {
    
    
    public partial class NursingNotesRecord : System.Windows.Controls.Page {
        
        internal Telerik.Windows.Controls.RadBusyIndicator radBusyIndicator;
        
        internal System.Windows.Controls.Grid LayoutRoot;
        
        internal Telerik.Windows.Controls.RadTabControl rtcNursingNotes;
        
        internal Telerik.Windows.Controls.RadGridView rgvVitalSignsRecord;
        
        internal Telerik.Windows.Controls.RadGridView rgvPatientInRecord;
        
        internal Telerik.Windows.Controls.RadGridView rgvPatientOutRecord;
        
        internal Telerik.Windows.Controls.RadGridView rgvTreatmentFlow;
        
        internal Telerik.Windows.Controls.RadGridView rgvVitalSignSpecialRecord;
        
        internal Telerik.Windows.Controls.RadComboBox rcmbDays;
        
        internal Telerik.Windows.Controls.RadButton rbtnQuery;
        
        internal Telerik.Windows.Controls.RadButton rbtnAdd;
        
        internal Telerik.Windows.Controls.RadButton rbtnCancel;
        
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
            System.Windows.Application.LoadComponent(this, new System.Uri("/Pathway;component/Views/NursingNotes/NursingNotesRecord.xaml", System.UriKind.Relative));
            this.radBusyIndicator = ((Telerik.Windows.Controls.RadBusyIndicator)(this.FindName("radBusyIndicator")));
            this.LayoutRoot = ((System.Windows.Controls.Grid)(this.FindName("LayoutRoot")));
            this.rtcNursingNotes = ((Telerik.Windows.Controls.RadTabControl)(this.FindName("rtcNursingNotes")));
            this.rgvVitalSignsRecord = ((Telerik.Windows.Controls.RadGridView)(this.FindName("rgvVitalSignsRecord")));
            this.rgvPatientInRecord = ((Telerik.Windows.Controls.RadGridView)(this.FindName("rgvPatientInRecord")));
            this.rgvPatientOutRecord = ((Telerik.Windows.Controls.RadGridView)(this.FindName("rgvPatientOutRecord")));
            this.rgvTreatmentFlow = ((Telerik.Windows.Controls.RadGridView)(this.FindName("rgvTreatmentFlow")));
            this.rgvVitalSignSpecialRecord = ((Telerik.Windows.Controls.RadGridView)(this.FindName("rgvVitalSignSpecialRecord")));
            this.rcmbDays = ((Telerik.Windows.Controls.RadComboBox)(this.FindName("rcmbDays")));
            this.rbtnQuery = ((Telerik.Windows.Controls.RadButton)(this.FindName("rbtnQuery")));
            this.rbtnAdd = ((Telerik.Windows.Controls.RadButton)(this.FindName("rbtnAdd")));
            this.rbtnCancel = ((Telerik.Windows.Controls.RadButton)(this.FindName("rbtnCancel")));
        }
    }
}

