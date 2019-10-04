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
    public partial class RptPathVariationPrint
    {
        
        public DateTime m_BeginTime = DateTime.Now.AddMonths(-1);
        public DateTime m_EndTime = DateTime.Now;
        public String m_Ljdm = "";
        public String m_Dept = "";
        public String m_LjdmName = "";
        public String m_DeptName = "";

        public RptPathVariationPrint()
        {
            InitializeComponent();

            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };

            this.ReportViewer1.RenderBegin += new RenderBeginEventHandler(ReportViewer1_RenderBegin);
        }

        void ReportViewer1_RenderBegin(object sender, RenderBeginEventArgs args)
        {
            args.ParameterValues["BeginTime"] = m_BeginTime;

            args.ParameterValues["EndTime"] = m_EndTime;

            args.ParameterValues["Ljdm"] = m_Ljdm;

            args.ParameterValues["Dept"] = m_Dept;

            args.ParameterValues["LjdmName"] = m_LjdmName;

            args.ParameterValues["DeptName"] = m_DeptName;
        }
 
    }
}

