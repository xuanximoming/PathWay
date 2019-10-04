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

namespace YidanEHRApplication.Views
{
    public partial class PathEnforceLongOrderPrint
    {
        public String m_Hzxm = "";
        public String m_Bed = "";
        public String m_CyksName = "";
        public String m_Zyhm = "";
        public String m_Syxh = "";
        public String m_Ljxh = "";

        public PathEnforceLongOrderPrint()
        {
            InitializeComponent();

            this.ReportViewer1.RenderBegin += new RenderBeginEventHandler(ReportViewer1_RenderBegin);
        }

        void ReportViewer1_RenderBegin(object sender, RenderBeginEventArgs args)
        {
            args.ParameterValues["Hzxm"] = m_Hzxm;

            args.ParameterValues["Bed"] = m_Bed;

            args.ParameterValues["CyksName"] = m_CyksName;

            args.ParameterValues["Zyhm"] = m_Zyhm;

            args.ParameterValues["Syxh"] = m_Syxh;

            args.ParameterValues["Ljxh"] = m_Ljxh;
        }
    }
}
