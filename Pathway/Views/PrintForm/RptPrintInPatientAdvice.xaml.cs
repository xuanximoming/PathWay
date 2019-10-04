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
using System.Windows.Navigation;
using YidanSoft.Tool;
using Telerik.ReportViewer.Silverlight;
using YidanEHRApplication.WorkFlow.Designer;
using YidanEHRApplication.WorkFlow;
using System.Text;

namespace YidanEHRApplication.Views
{
    public partial class RptPrintInPatientAdvice
    {
        static public string Syxh;
        public RptPrintInPatientAdvice()
        {
            InitializeComponent();

            this.ReportViewer1.RenderBegin += new RenderBeginEventHandler(ReportViewer1_RenderBegin);


        }

        void ReportViewer1_RenderBegin(object sender, RenderBeginEventArgs args)
        {
            //single value parameter

            String list = GetDetail(Global.InpatientListCurrent.EnForceWorkFlowXml);

            //传参数到报表中
            args.ParameterValues["Syxh"] = Global.InpatientListCurrent.Syxh;

            args.ParameterValues["DetailID"] = list;

        }

        /// <summary>
        /// 获取已经执行的路径明细ID
        /// </summary>
        /// <param name="strXml"></param>
        /// <returns></returns>
        public String GetDetail(String strXml)
        {
            WorkFlow.WorkFlow m_WorkFlow = new WorkFlow.WorkFlow();
            ContainerShow containerEdit = new ContainerShow();
            containerEdit.IsShowAll = false;
            containerEdit.WorkFlowUrlName = Global.InpatientListCurrent.Ljmc;
            containerEdit.WorkFlowUrlID = Global.InpatientListCurrent.Ljdm;
            containerEdit.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            containerEdit.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            containerEdit.WorkFlowXML = strXml;
            m_WorkFlow.ContainerEdit = containerEdit;
            containerEdit.LayoutRoot_Loaded(null, null);
            Activity _Activity;
            StringBuilder strKist = new StringBuilder();
            foreach (YidanEHRApplication.WorkFlow.Designer.Activity ac in m_WorkFlow.Activitys)
            {
                if (ac.Type == ActivityType.INITIAL)
                {
                    _Activity = ac;
                    while (true)
                    {
                        strKist.Append( _Activity.UniqueID + ",");
                        Activity _Activity3 = null;
                        foreach (Activity _Activity2 in _Activity.NextActivitys)
                        {

                            if (_Activity2.CurrentElementState == ElementState.Pre)
                            {

                                _Activity3 = _Activity2;
                            }
                        }
                        if (_Activity3 == null)
                            break;
                        _Activity = _Activity3;
                    }

                }
            }

            return strKist.ToString();

        }

    }
}
