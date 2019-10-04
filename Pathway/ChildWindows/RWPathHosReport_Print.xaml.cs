using System;
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

using YidanEHRApplication.DataService;

using Telerik.Windows.Controls;
using Telerik.ReportViewer.Silverlight;

namespace YidanEHRApplication.ChildWindows
{
    /// <summary>
    /// Interaction logic for RWPathOrdersAll_Print.xaml
    /// </summary>
    public partial class RWPathHosReport_Print 
    {
        public String Syxh = "";
        public String Ljdm = "";

        public CP_InpatinetList m_currentpat;

        public RWPathHosReport_Print()
        {
         
            InitializeComponent();
            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };
            this.ReportViewer1.RenderBegin+= new Telerik.ReportViewer.Silverlight.RenderBeginEventHandler(ReportViewer1_RenderBegin);

        }

        public RWPathHosReport_Print(CP_InpatinetList currentpat)
        {

            InitializeComponent();
            m_currentpat = currentpat;
            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };
            ReportViewer1.RenderBegin += new Telerik.ReportViewer.Silverlight.RenderBeginEventHandler(ReportViewer1_RenderBegin);

        }

        void ReportViewer1_RenderBegin(object sender, RenderBeginEventArgs args)
        {
            args.ParameterValues["Syxh"] = Syxh;
            args.ParameterValues["Ljdm"] = Ljdm;
        }


    }
}
