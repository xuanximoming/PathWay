﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace YidanEHRApplication.Views.ReportForms
{
    /// <summary>
    /// Interaction logic for RptPathEnterStatisticsPrint.xaml
    /// </summary>
    public partial class RptPathEnterStatisticsPrint
    {
        public String m_BeginTime = String.Empty;
        public String m_EndTime = String.Empty;
        public String m_Dept = String.Empty;
        public String m_GetType = String.Empty;
        public String m_Ljzt = String.Empty;
        public String m_Ljdm = String.Empty;
        public String m_Bzdm = String.Empty;

        public RptPathEnterStatisticsPrint()
        {
            InitializeComponent();
            this.ReportViewer1.RenderBegin += new Telerik.ReportViewer.Silverlight.RenderBeginEventHandler(ReportViewer1_RenderBegin);
        }

        void ReportViewer1_RenderBegin(object sender, Telerik.ReportViewer.Silverlight.RenderBeginEventArgs args)
        {
            args.ParameterValues["BeginTime"] = m_BeginTime;

            args.ParameterValues["EndTime"] = m_EndTime;

            args.ParameterValues["Dept"] = m_Dept;

            args.ParameterValues["GetType"] = m_GetType;

            args.ParameterValues["Ljzt"] = m_Ljzt;

            args.ParameterValues["Ljdm"] = m_Ljdm;

            args.ParameterValues["Bzdm"] = m_Bzdm;
        }
    }
}
