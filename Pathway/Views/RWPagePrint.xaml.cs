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
    public partial class RWPagePrint
    {

        /// <summary>
        /// 打印页眉信息
        /// </summary>
        public String m_Title = "测试公共打印页面";
        /// <summary>
        /// 显示表头字段 用“，”隔开
        /// </summary>
        public String m_TableTile = "临床路径,病人名称,入径时间,退出时间,医护人员,退出原因";
        /// <summary>
        /// 打印页中表中字段绑定字段，与表头个数位置对应 用“，”隔开
        /// </summary>
        public String m_TableBindCol = "Name,Hzxm,Jrsj,Tcsj,DName,Tcyy";
        /// <summary>
        /// 每列宽度 以厘米为单位   用“，”隔开
        /// </summary>
        public String m_ColWidth = @"3.0,2.0,2.0,2.0,2.0,3.0";

        /// <summary>
        /// 页面中执行的SQL语句
        /// </summary>
        public String m_ExecSQL = @"exec usp_CP_RptPathQuit @begindate='2011-01-17',@enddate='2011-02-18',@dept='2032',@Ljdm='P.K62.001',@Doctor=''";


        public RWPagePrint()
        {
            InitializeComponent();

            this.ReportViewer1.RenderBegin += new RenderBeginEventHandler(ReportViewer1_RenderBegin);
        }

        void ReportViewer1_RenderBegin(object sender, RenderBeginEventArgs args)
        {
            args.ParameterValues["Title"] = m_Title;

            args.ParameterValues["TableTitle"] = m_TableTile;

            args.ParameterValues["TableBindCol"] = m_TableBindCol;

            args.ParameterValues["ColWidth"] = m_ColWidth;

            args.ParameterValues["ExecSQL"] = m_ExecSQL;

            
        }


 
    }
}

