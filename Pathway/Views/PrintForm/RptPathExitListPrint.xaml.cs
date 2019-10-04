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
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using Telerik.Windows.Controls.Charting;
using System.Windows.Data;
using System.Reflection;
using Telerik.Windows.Controls.GridView;

namespace YidanEHRApplication.Views.ReportForms
{
    public partial class RptPathExitListPrint
    {
        public String m_BeginTime = "";
        public String m_EndTime = "";
       
        public RptPathExitListPrint()
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
            args.ParameterValues["BegTime"] = m_BeginTime;

            args.ParameterValues["EndTime"] = m_EndTime;

        }


    }
}
