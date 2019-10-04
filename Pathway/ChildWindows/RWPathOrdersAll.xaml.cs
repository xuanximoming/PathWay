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
    public partial class RWPathOrdersAll
    {

        public String Syxh = "";
        public String Ljmc="";
        List<CP_PathOrdersAll> PathOrdersAllList = new List<CP_PathOrdersAll>();
        public CP_InpatinetList m_currentpat;

        public RWPathOrdersAll(String syxh,String ljmc, CP_InpatinetList currentpat)
        {
            m_currentpat = currentpat;
            Ljmc = ljmc;
            Syxh = syxh;
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
                //txtClinicalPath.Text = "       引入路径：" + Ljmc + "   入院诊断:" + m_currentpat.Ryzd + "(" + m_currentpat.RyzdCode + ") ";
                txtClinicalPath.Text = Ljmc;    
                
                //txtPatient.Text = "患者姓名：" + m_currentpat.Hzxm + "      性别：" + m_currentpat.Xb + "    年龄：" + m_currentpat.Nl + "    门诊号：" + m_currentpat.Hissyxh + "      住院号：" + m_currentpat.Zyhm;
                
                txtinpathTime.Text = "患者姓名：" + m_currentpat.Hzxm + "      性别：" + m_currentpat.Brxb + "    年龄：" + m_currentpat.Xsnl + "    门诊号：" + m_currentpat.Hissyxh + "      住院号：" + m_currentpat.Zyhm;
                //txtinpathTime.Text = " 住院日期：" + m_currentpat.Ryrq + "         出院日期：" + m_currentpat.Cyrq;
                BindGrid();
                //路径状态：" + m_currentpat.LjztName;
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
                RadOrdersAll.ItemsSource = PathOrdersAllList;
            };
            GetRWPathSummaryClient.GetPathOrdersAllAsync(Syxh);
          
        }

        private void print_Click(object sender, RoutedEventArgs e)
        {
            RWPathOrdersAll_Print rwPrint = new RWPathOrdersAll_Print();
            rwPrint.Syxh = m_currentpat.Syxh;
            rwPrint.WindowState = System.Windows.WindowState.Maximized;
            rwPrint.ResizeMode = Telerik.Windows.Controls.ResizeMode.NoResize;
            rwPrint.ShowDialog();
        }


    }
}
