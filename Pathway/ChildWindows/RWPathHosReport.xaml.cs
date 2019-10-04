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
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.ChildWindows
{
    /// <summary>
    /// Interaction logic for RWPathOrdersAll.xaml
    /// </summary>
    public partial class RWPathHosReport
    {

        public String Syxh = "";
        public String Ljmc="";
        public String Ljdm = "";
        List<CP_PathOrdersAll> PathOrdersAllList = new List<CP_PathOrdersAll>();
        public CP_InpatinetList m_currentpat;

        public RWPathHosReport(String syxh, String ljmc, CP_InpatinetList currentpat)
        {
            m_currentpat = currentpat;
            Ljmc = ljmc;
            Syxh = syxh;
            Ljdm = currentpat.Ljdm;
            InitializeComponent();
            radBusyIndicator.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };
        }
        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                txtClinicalPath.Text = Ljmc;                                               
                txtinpathTime.Text = "患者姓名：" + m_currentpat.Hzxm + "      性别：" + m_currentpat.Brxb + "    年龄：" + m_currentpat.Xsnl + "    门诊号：" + m_currentpat.Hissyxh + "      住院号：" + m_currentpat.Zyhm;
                BindGrid();                
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

       
        }
        /// <summary>
        /// 初始化Grid
        /// </summary>
        private void BindGrid()
        {
            YidanEHRDataServiceClient GetRWPathSummaryClient = PublicMethod.YidanClient;
            GetRWPathSummaryClient.GetPathOrdersAllCompleted += (obj, e) =>
            {                
                PathOrdersAllList=e.Result.ToList();
                List<Pathdiagnosis> list=PathOrdersAllList[0].pathdiagnosis.ToList();
                string name = "适用对象：";
                foreach (var item in list)
                {
                    name += item.name + "(" + item.cid + ")";
                }
                txtPatient.Text = name;
                RadHosReport.ItemsSource = PathOrdersAllList;
            };
            GetRWPathSummaryClient.GetPathOrdersAllAsync(Syxh);
          
        }

        private void print_Click(object sender, RoutedEventArgs e)
        {
            RWPathHosReport_Print rwPrint = new RWPathHosReport_Print();
            rwPrint.Syxh = m_currentpat.Syxh;
            rwPrint.Ljdm = m_currentpat.Ljdm;
            rwPrint.WindowState = System.Windows.WindowState.Maximized;
            rwPrint.ResizeMode = Telerik.Windows.Controls.ResizeMode.NoResize;
            rwPrint.ShowDialog();
        }


    }
}
