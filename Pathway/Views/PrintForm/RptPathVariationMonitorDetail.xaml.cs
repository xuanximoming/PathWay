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

using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using Telerik.Windows.Controls;
using YidanSoft.Tool;
using System.Windows.Printing;
using YidanEHRApplication.Views.ReportForms;

namespace YidanEHRApplication.Views.PrintForm
{

    public partial class RptPathVariationMonitorDetail
    {
        public RptPathVariationMonitorDetail(String ljmc, String jdmc, String beginDate, String endDate, String ljmcAll)
        {
            BeginDate = beginDate;
            EndDate = endDate;
            Ljmc = ljmc;
            LjmcAll = ljmcAll;
            Jdmc = jdmc;
            InitializeComponent();
            radBusyIndicator.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };
        }

        #region 变量
        public String BeginDate;
        public String EndDate;
        public String Ljmc;
        public String LjmcAll;
        public String Jdmc;

        public List<YidanEHRApplication.DataService.RPT_PathVariationMonitorDetail> pathSummaryDetail_newlist;// RptPathStatisticDetail> 

        public RW_PathSummary pathSummary;                                      //加载窗口时的全类(每次开窗口后值不变（稳定）)
        public String workFlow;                                                 //当前节点(每次开窗口后值不变（稳定）)
        public List<RW_PathSummaryCategory> categoryList;                       //需要显示的字典列表(每次开窗口后值不变（稳定）)
        public List<RW_PathSummaryEnForce> pathEnForceList;                     //全局节点列表(每次开窗口后值不变（稳定）)

        public Int32 IsVariant;        //显示变异

        /// <summary>
        /// 当前选中病人
        /// </summary>
        public CP_InpatinetList m_currentpat;

        #endregion
        #region 事件
        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {

                //txtPatient.Text = "患者姓名：" + m_currentpat.Hzxm + "      性别：" + m_currentpat.Brxb + "    年龄：" + m_currentpat.Xsnl + "    门诊号：" + m_currentpat.Hissyxh + "      住院号：" + m_currentpat.Zyhm;
                txtClinicalPath.Text = "路径名称：" + LjmcAll + "  节点名称：" + Jdmc;
                //txtinpathTime.Text = " 住院日期：" + m_currentpat.Ryrq + "         出院日期：" + m_currentpat.Cyrq;
                GetRWPathSummary_new();

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void checkBoxAll_Click(object sender, RoutedEventArgs e)
        {
            //total.Children.Clear();
            //ShowWorkFlow();
        }
        private void ComboBoxVariant_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //IsVariant = ConvertMy.ToInt32((ComboBoxVariant.SelectedItem as ComboBoxItem).Tag); 

            //total.Children.Clear();
            //ShowWorkFlow();
        }
        #endregion
        #region 方法
        /// <summary>
        /// 获取该病人，该次路径的数据（5.5）
        /// </summary>
        private void GetRWPathSummary_new()
        {
            radBusyIndicator.IsBusy = true;

            YidanEHRDataServiceClient GetRptPathStatisticsDetailClient = PublicMethod.YidanClient;
            GetRptPathStatisticsDetailClient.GetRptPathVariationMonitorDetailCompleted +=
            (obj, e) =>
            {
                try
                {
                    radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        if (e.Result == null)
                        {
                            return;
                        }
                        else
                        {
                            pathSummaryDetail_newlist = null;         //清空
                            pathSummaryDetail_newlist = e.Result.ToList();

                            if (pathSummaryDetail_newlist.Count != 0)
                            {
                                //txtWorkFlow.Text = "        当前步骤：" + pathEnForceList[pathEnForceList.Count - 1].Ljmc;    //当前步骤（如果没有节点，则此处报错）
                                //workFlow = pathEnForceList[pathEnForceList.Count - 1].Jddm;                            //当前节点代码
                                //txtinpathTime.Text = " 住院日期：" + m_currentpat.Ryrq + "        入径日期：" + pathSummaryDetail_newlist[0].JRSJ + "        出院日期：" + m_currentpat.Cyrq;
                                RadGridView1.ItemsSource = pathSummaryDetail_newlist;
                                //ShowWorkFlow();
                            }
                        }
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                }
                catch (Exception ex)
                {
                    YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            };

            GetRptPathStatisticsDetailClient.GetRptPathVariationMonitorDetailAsync(Ljmc, Jdmc, BeginDate, EndDate);
            //MessageBox.Show(Syxh);
            GetRptPathStatisticsDetailClient.CloseAsync();
        }

        /// <summary>
        /// 获取该病人，该次路径的数据（5.5）
        /// </summary>
        private void GetRWPathSummary()
        {
            //radBusyIndicator.IsBusy = true;

            //YidanEHRDataServiceClient GetRWPathSummaryClient = PublicMethod.YidanClient;
            //GetRWPathSummaryClient.GetRW_PathSummaryCompleted +=
            //(obj, e) =>
            //{
            //    try
            //    {
            //        radBusyIndicator.IsBusy = false;
            //        if (e.Error == null)
            //        {
            //            if (e.Result == null)
            //            {
            //                return;
            //            }
            //            else
            //            {
            //                pathSummary = null;         //清空
            //                pathSummary = e.Result;
            //                categoryList = pathSummary.PathSummaryCategory.ToList();
            //                pathEnForceList = pathSummary.PathSummaryEnForce.ToList();

            //                if (pathEnForceList.Count != 0)
            //                {
            //                    //txtWorkFlow.Text = "        当前步骤：" + pathEnForceList[pathEnForceList.Count - 1].Ljmc;    //当前步骤（如果没有节点，则此处报错）
            //                    workFlow = pathEnForceList[pathEnForceList.Count - 1].Jddm;                            //当前节点代码

            //                    //ShowWorkFlow();
            //                }
            //            }
            //        }
            //        else
            //        {
            //            PublicMethod.RadWaringBox(e.Error);
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            //    }
            //};

            //GetRWPathSummaryClient.GetRW_PathSummaryAsync(Syxh, Ljxh);
            //GetRWPathSummaryClient.CloseAsync();
        }

        private int index = 0;
        /// <summary>
        /// 打印功能（暂未实现）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void print_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                //string path = Ljmc;
                //string startdate = BeginDate;
                //string enddate = EndDate;
                //string jdmc = Jdmc;

                RptPathVariationMonitorDetailPrint statisdetailprint = new RptPathVariationMonitorDetailPrint();
                statisdetailprint.m_BeginTime = BeginDate;
                statisdetailprint.m_EndTime = EndDate;
                statisdetailprint.m_Ljdm = Ljmc;
                statisdetailprint.m_Jdmc = Jdmc;
                statisdetailprint.m_Ljmc = LjmcAll;
                statisdetailprint.WindowState = WindowState.Maximized;
                statisdetailprint.ShowDialog();

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        void document_PrintPage(object sender, PrintPageEventArgs e)
        {

        }






        #endregion
    }
}

