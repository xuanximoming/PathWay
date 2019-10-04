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
using Pathway.DataService;

namespace YidanEHRApplication.Views.ReportForms
{
    public partial class testreportprint
    {
        public Rpt_QueryCondition printCondition = new Rpt_QueryCondition();

        public testreportprint()
        {
            InitializeComponent();

            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };

            //加载报表数据源
            this.ReportViewer1.RenderBegin += new RenderBeginEventHandler(ReportViewer1_RenderBegin);
        }

        void ReportViewer1_RenderBegin(object sender, RenderBeginEventArgs args)
        {
            args.ParameterValues["BeginTime"] = printCondition.Stardate;
            args.ParameterValues["EndTime"] = printCondition.Enddate;
            args.ParameterValues["Ljdm"] = printCondition.Path;
            args.ParameterValues["Bymc"] = printCondition.PathName;
            this.ReportViewer1.Report = "YidanEHRReport.Report3, PathwayReports";
        }
    }
}