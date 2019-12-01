using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Views.ChildWindows
{

    public partial class RWPathSummary
    {
        public RWPathSummary(String syxh, String ljxh, String ljmc, String hzxm, Boolean isAll, Int32 isVariant, CP_InpatinetList currentpat)
        {
            Syxh = syxh;
            Ljxh = ljxh;
            Ljmc = ljmc;
            Hzxm = hzxm;
            IsVariant = isVariant;
            m_currentpat = currentpat;
            InitializeComponent();
            radBusyIndicator.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };
        }

        #region 变量
        public String Syxh;
        public String Ljxh;
        public String Ljmc;
        public String Hzxm;

        public List<RW_PathSummary_new> pathSummary_newlist;

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
                txtPatient.Text = "患者姓名：" + m_currentpat.Hzxm + "      性别：" + m_currentpat.Brxb + "    年龄：" + m_currentpat.Xsnl + "    门诊号：" + m_currentpat.Hissyxh + "      住院号：" + m_currentpat.Zyhm;
                txtClinicalPath.Text = "       引入路径：" + Ljmc + "   入院诊断:" + m_currentpat.Ryzd + "(" + m_currentpat.RyzdCode + ")    路径状态：" + m_currentpat.LjztName;
                txtinpathTime.Text = " 住院日期：" + m_currentpat.Ryrq + "         出院日期：" + m_currentpat.Cyrq;
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

            YidanEHRDataServiceClient GetRWPathSummaryClient = PublicMethod.YidanClient;
            GetRWPathSummaryClient.GetRW_PathSummary_newCompleted +=
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
                            pathSummary_newlist = null;         //清空
                            pathSummary_newlist = e.Result.ToList();

                            if (pathSummary_newlist.Count != 0)
                            {
                                //txtWorkFlow.Text = "        当前步骤：" + pathEnForceList[pathEnForceList.Count - 1].Ljmc;    //当前步骤（如果没有节点，则此处报错）
                                //workFlow = pathEnForceList[pathEnForceList.Count - 1].Jddm;                            //当前节点代码
                                txtinpathTime.Text = " 住院日期：" + m_currentpat.Ryrq + "        入径日期：" + pathSummary_newlist[0].JRSJ + "        出院日期：" + m_currentpat.Cyrq;
                                RadGridView1.ItemsSource = pathSummary_newlist;
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

            GetRWPathSummaryClient.GetRW_PathSummary_newAsync(Syxh);
            //MessageBox.Show(Syxh);
            GetRWPathSummaryClient.CloseAsync();
        }

        /// <summary>
        /// 获取该病人，该次路径的数据（5.5）
        /// </summary>
        private void GetRWPathSummary()
        {
            radBusyIndicator.IsBusy = true;

            YidanEHRDataServiceClient GetRWPathSummaryClient = PublicMethod.YidanClient;
            GetRWPathSummaryClient.GetRW_PathSummaryCompleted +=
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
                            pathSummary = null;         //清空
                            pathSummary = e.Result;
                            categoryList = pathSummary.PathSummaryCategory.ToList();
                            pathEnForceList = pathSummary.PathSummaryEnForce.ToList();

                            if (pathEnForceList.Count != 0)
                            {
                                //txtWorkFlow.Text = "        当前步骤：" + pathEnForceList[pathEnForceList.Count - 1].Ljmc;    //当前步骤（如果没有节点，则此处报错）
                                workFlow = pathEnForceList[pathEnForceList.Count - 1].Jddm;                            //当前节点代码

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

            GetRWPathSummaryClient.GetRW_PathSummaryAsync(Syxh, Ljxh);
            GetRWPathSummaryClient.CloseAsync();
        }

        private int index = 0;
        private void print_Click(object sender, RoutedEventArgs e)
        {

            RWPathSummary_Print pageprint = new RWPathSummary_Print();

            pageprint.Syxh = m_currentpat.Syxh;
            pageprint.WindowState = System.Windows.WindowState.Maximized;
            pageprint.ResizeMode = Telerik.Windows.Controls.ResizeMode.NoResize;

            pageprint.ShowDialog();
        }
        #endregion
    }
}

