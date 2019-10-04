using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.ReportViewer.Silverlight;

namespace YidanEHRApplication.Views.ReportForms
{
    /// <summary>
    /// Interaction logic for RptInPathPatientFeePercentPrint.xaml
    /// </summary>
    public partial class RptInPathPatientFeePercentPrint
    {
        public String m_BeginTime = String.Empty;
        public String m_EndTime = String.Empty;
        public String m_Ljzt = String.Empty;
        public String m_Ljdm = String.Empty;

        public RptInPathPatientFeePercentPrint()
        {
            InitializeComponent();

            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };

            this.ReportViewer1.RenderBegin += new Telerik.ReportViewer.Silverlight.RenderBeginEventHandler(reportViewer1_RenderBegin);
        }

        void reportViewer1_RenderBegin(object sender, Telerik.ReportViewer.Silverlight.RenderBeginEventArgs args)
        {
            args.ParameterValues["BeginTime"] = m_BeginTime;

            args.ParameterValues["EndTime"] = m_EndTime;

            args.ParameterValues["Ljzt"] = m_Ljzt;

            args.ParameterValues["Ljdm"] = m_Ljdm;
        }
    }
}
