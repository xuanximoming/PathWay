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

namespace YidanEHRApplication.Views.ReportForms
{
    public partial class RptPathVariationAnalysePrint
    {
        public Rpt_QueryCondition printCondition = new Rpt_QueryCondition();

        public RptPathVariationAnalysePrint()
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
            args.ParameterValues["BeginTime"] = printCondition.Stardate;
            args.ParameterValues["EndTime"] = printCondition.Enddate;
            args.ParameterValues["Ljdm"] = printCondition.Path;
            args.ParameterValues["Dept"] = printCondition.DeptInfo;
            args.ParameterValues["LjdmName"] = printCondition.PathName;
            args.ParameterValues["DeptName"] = printCondition.DeptName;
        }
    }
}

