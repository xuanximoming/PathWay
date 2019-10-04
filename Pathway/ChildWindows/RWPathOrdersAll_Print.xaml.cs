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

using Telerik.Windows.Controls;
using Telerik.ReportViewer.Silverlight;

namespace YidanEHRApplication.ChildWindows
{
    /// <summary>
    /// Interaction logic for RWPathOrdersAll_Print.xaml
    /// </summary>
    public partial class RWPathOrdersAll_Print 
    {
        public String Syxh = "";
        public RWPathOrdersAll_Print()
        {
         
            InitializeComponent();
            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };
            this.ReportViewer1.RenderBegin+=new Telerik.ReportViewer.Silverlight.RenderBeginEventHandler(ReportViewer1_RenderBegin);


            //ReportViewer1
            //ReportViewer1.TextResources.ExportToolTip  //; = "第一页";
 
              

        //        Viewer1.Toolbar.Tools(0).ToolTip = "各页目录"
        //Viewer1.Toolbar.Tools(2).Caption = "打印..."
        //Viewer1.Toolbar.Tools(2).ToolTip = "打印报表"
        //Viewer1.Toolbar.Tools(4).ToolTip = "拷贝"
        }
        void ReportViewer1_RenderBegin(object sender, RenderBeginEventArgs args)
        {
            args.ParameterValues["Syxh"] = Syxh;

        }


    }
}
