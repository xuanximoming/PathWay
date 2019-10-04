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
using YidanEHRApplication.Models;
using YidanEHRApplication.DataService;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.Charting;
using Telerik.Windows.Controls.GridView;

namespace YidanEHRApplication.Views.ReportForms
{
    /// <summary>
    /// 表示入径统计界面的类
    /// </summary>
    public partial class RptPathEnterStatistics : Page
    {
        #region 事件

        Int32 index = 0;
        /// <summary>
        /// 页面加载事件
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                dtpStartDate.SelectedValue = DateTime.Now.AddMonths(-1);
                dtpEndDate.SelectedValue = DateTime.Now;
                GetDepartmentListInfo();//获取临床科室
                GetClinicalPathList();//获取临床路径
                GetBzList();//获取病种列表  
                index = 1;

                rctEnterPathRj.DefaultView.ChartTitle.Content = "临床路径入径统计";
                rctEnterPathRj.DefaultView.ChartLegend.Header = "统计类型";
                rctEnterPathRj.DefaultView.ChartArea.NoDataString = "无数据...";
                rctEnterPathWc.DefaultView.ChartTitle.Content = "临床路径完成统计";
                rctEnterPathWc.DefaultView.ChartLegend.Header = "统计类型";
                rctEnterPathWc.DefaultView.ChartArea.NoDataString = "无数据...";

                btnStat_Click(sender, e);
                ExpTb_Collapsed(sender, e);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 科室选择改变事件
        /// </summary>
        private void cmbOffice_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                GetClinicalPathList();
                GetBzList();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 查询按钮事件
        /// </summary>
        private void btnStat_Click(object sender, RoutedEventArgs e)
        {
            //if ()
            //{
            //    PublicMethod.RadAlterBox("时间不能为空！", "提示信息！");
            //    return;
            //}

            if (dtpStartDate.SelectedDate > dtpEndDate.SelectedDate || this.dtpStartDate.SelectedDate == null || this.dtpEndDate.SelectedDate == null)
            {
                PublicMethod.RadAlterBox("开始日期必须小于或等于结束日期,且不能为空！", "提示");
                return;
            }
            radBusyIndicator.IsBusy = true;
            YidanEHRDataServiceClient pathClient = PublicMethod.YidanClient;
            String begindate = dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");//开始时间
            String enddate = ((DateTime)dtpEndDate.SelectedDate).AddDays(1).ToString("yyyy-MM-dd");//结束时间
            //String begindate = dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            //String enddate = ((DateTime)dtpEndDate.SelectedDate).AddDays(1).ToString("yyyy-MM-dd");
            String dept = (cmbOffice.SelectedItem == null) ? String.Empty : (cmbOffice.SelectedItem as CP_DepartmentList).Ksdm;
            String ljdm = (autoPath.SelectedItem == null) ? String.Empty : (autoPath.SelectedItem as CP_ClinicalPathList).Ljdm;
            String bzdm = (cmbBz.SelectedItem == null) ? String.Empty : cmbBz.SelectedValue.ToString();
            String getType = String.Empty;//查询类型，默认按照路径查询
            String Ljzt = String.Empty;//路径状态
            if (cmbGetType.SelectionBoxItem.ToString() == "根据路径")
            {
                getType = "1";//按照路径查询
                bzdm = String.Empty;
                String ljztText = cmbLjzt.SelectionBoxItem.ToString();
                switch (cmbLjzt.SelectionBoxItem.ToString())
                {
                    case "审核":
                        Ljzt = "3";
                        break;
                    case "停止":
                        Ljzt = "2";
                        break;
                    default:
                        break;
                }
            }
            else
            {
                getType = "2";//按照病种查询
                ljdm = String.Empty;
            }
            pathClient.GetRpt_PathEnterStatisticsCompleted +=
                (obj, ea) =>
                {
                    radBusyIndicator.IsBusy = false;
                    if (ea.Error == null)
                    {
                        gvPathStat.ItemsSource = ea.Result.ToList();
                    }
                };
            pathClient.GetRpt_PathEnterStatisticsAsync(begindate, enddate, dept, Ljzt, ljdm, bzdm, getType);
        }

        /// <summary>
        /// 图表隐藏事件
        /// </summary>
        private void ExpTb_Collapsed(object sender, RoutedEventArgs e)
        {
            ExpTb.Header = "显示图表";
            //Row6.Height = new GridLength(0, GridUnitType.Pixel);
            rctEnterPathRj.Visibility = System.Windows.Visibility.Collapsed;
            rctEnterPathWc.Visibility = System.Windows.Visibility.Collapsed;
            Row6.MaxHeight = 0;
        }

        /// <summary>
        /// 图表显示事件
        /// </summary>
        private void ExpTb_Expanded(object sender, RoutedEventArgs e)
        {
            ExpTb.Header = "隐藏图表";
            //Row6.Height = new GridLength(1, GridUnitType.Star);
            rctEnterPathRj.Visibility = System.Windows.Visibility.Visible;
            rctEnterPathWc.Visibility = System.Windows.Visibility.Visible;
            Row6.MaxHeight = 200;
        }

        /// <summary>
        /// 查询类型下拉框选择改变事件
        /// </summary>
        private void cmbGetType_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (index != 1)
            {
                return;
            }
            String getType = cmbGetType.SelectionBoxItem.ToString();
            if (getType == "根据路径")
            {
                cmbBz.IsEnabled = false;
                cmbLjzt.IsEnabled = true;
                autoPath.IsEnabled = true;
            }
            if (getType == "根据病种")
            {
                cmbBz.IsEnabled = true;
                cmbLjzt.IsEnabled = false;
                autoPath.IsEnabled = false;
            }
        }

        /// <summary>
        /// 表示入径统计的类
        /// </summary>
        public class EnterPathStatistics
        {
            /// <summary>
            /// 统计名称
            /// </summary>
            public String PName { get; set; }
            /// <summary>
            /// 比例
            /// </summary>
            public String PCount { get; set; }
        }

        /// <summary>
        /// 网格行激活事件
        /// </summary>
        private void gvPathStat_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e)
        {
            List<EnterPathStatistics> enterPathRjList = new List<EnterPathStatistics>();//入径统计集合
            List<EnterPathStatistics> enterPathWcList = new List<EnterPathStatistics>();//路径完成统计集合
            GridViewRow row = (GridViewRow)e.Row;
            Rpt_PathEnterStatistics pes = (Rpt_PathEnterStatistics)row.DataContext;
            String Wyr = "0";//未引入路径
            if (pes.Bhzs != "0")
            {
                Wyr = (100 - Convert.ToInt32(pes.Rjl)).ToString();
            }
            EnterPathStatistics eps1 = new EnterPathStatistics();
            eps1.PName = "未引入路径";
            eps1.PCount = Wyr;
            enterPathRjList.Add(eps1);
            EnterPathStatistics eps2 = new EnterPathStatistics();
            eps2.PName = "入径率";
            eps2.PCount = pes.Rjl;
            enterPathRjList.Add(eps2);
            ShowRctEnterPathRj(enterPathRjList);
            EnterPathStatistics eps3 = new EnterPathStatistics();
            eps3.PName = "完成率";
            eps3.PCount = pes.Wcl;
            enterPathWcList.Add(eps3);
            EnterPathStatistics eps4 = new EnterPathStatistics();
            eps4.PName = "退出率";
            eps4.PCount = pes.Tcl;
            enterPathWcList.Add(eps4);
            EnterPathStatistics eps5 = new EnterPathStatistics();
            eps5.PName = "在径率";
            eps5.PCount = pes.Zjl;
            enterPathWcList.Add(eps5);
            ShowRctEnterPathWc(enterPathWcList);
        }

        /// <summary>
        /// 表示gvPathStat行的选择改变事件
        /// </summary>
        private void gvPathStat_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            List<EnterPathStatistics> enterPathRjList = new List<EnterPathStatistics>();//入径统计集合
            List<EnterPathStatistics> enterPathWcList = new List<EnterPathStatistics>();//路径完成统计集合
            Rpt_PathEnterStatistics pes = (Rpt_PathEnterStatistics)gvPathStat.SelectedItem;
            if (pes == null)
            {
                return;
            }
            String Wyr = "0";//未引入路径
            if (pes.Bhzs != "0")
            {
                Wyr = (100 - Convert.ToInt32(pes.Rjl)).ToString();
            }
            EnterPathStatistics eps1 = new EnterPathStatistics();
            eps1.PName = "未引入路径";
            eps1.PCount = Wyr;
            enterPathRjList.Add(eps1);
            EnterPathStatistics eps2 = new EnterPathStatistics();
            eps2.PName = "入径率";
            eps2.PCount = pes.Rjl;
            enterPathRjList.Add(eps2);
            ShowRctEnterPathRj(enterPathRjList);
            EnterPathStatistics eps3 = new EnterPathStatistics();
            eps3.PName = "完成率";
            eps3.PCount = pes.Wcl;
            enterPathWcList.Add(eps3);
            EnterPathStatistics eps4 = new EnterPathStatistics();
            eps4.PName = "退出率";
            eps4.PCount = pes.Tcl;
            enterPathWcList.Add(eps4);
            EnterPathStatistics eps5 = new EnterPathStatistics();
            eps5.PName = "在径率";
            eps5.PCount = pes.Zjl;
            enterPathWcList.Add(eps5);
            ShowRctEnterPathWc(enterPathWcList);
        }

        /// <summary>
        /// 表示打印按钮的事件
        /// </summary>
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((dtpStartDate.SelectedDate > dtpEndDate.SelectedDate)
                       || dtpStartDate.SelectedDate.ToString().Trim() == ""
                       || dtpEndDate.SelectedDate.ToString().Trim() == "")
                {
                    PublicMethod.RadAlterBox("开始日期必须小于或等于结束日期，且不能为空！", "提示");
                    return;
                }
                YidanEHRDataServiceClient pathClient = PublicMethod.YidanClient;
                String BeginTime = dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                String EndTime = ((DateTime)dtpEndDate.SelectedDate).AddDays(1).ToString("yyyy-MM-dd");
                String Dept = (cmbOffice.SelectedItem == null) ? String.Empty : (cmbOffice.SelectedItem as CP_DepartmentList).Ksdm;
                String Ljdm = (autoPath.SelectedItem == null) ? String.Empty : (autoPath.SelectedItem as CP_ClinicalPathList).Ljdm;
                String Bzdm = (cmbBz.SelectedItem == null) ? String.Empty : cmbBz.SelectedValue.ToString();
                String GetType = String.Empty;//查询类型，默认按照路径查询
                String Ljzt = String.Empty;//路径状态
                if (cmbGetType.SelectionBoxItem.ToString() == "根据路径")
                {
                    GetType = "1";//按照路径查询
                    Bzdm = String.Empty;
                    String ljztText = cmbLjzt.SelectionBoxItem.ToString();
                    switch (cmbLjzt.SelectionBoxItem.ToString())
                    {
                        case "审核":
                            Ljzt = "3";
                            break;
                        case "停止":
                            Ljzt = "2";
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    GetType = "2";//按照病种查询
                    Ljdm = String.Empty;
                }

                RptPathEnterStatisticsPrint pageprint = new RptPathEnterStatisticsPrint();

                pageprint.m_BeginTime = BeginTime;
                pageprint.m_EndTime = EndTime;
                pageprint.m_Dept = Dept;
                pageprint.m_GetType = GetType;
                pageprint.m_Ljzt = Ljzt;
                pageprint.m_Ljdm = Ljdm;
                pageprint.m_Bzdm = Bzdm;
                pageprint.WindowState = WindowState.Maximized;
                pageprint.ShowDialog();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #endregion

        #region 方法

        /// <summary>
        ///  构造函数RptPathEnterStatistics
        /// </summary>
        public RptPathEnterStatistics()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 表示显示路径完成比例图的方法
        /// </summary>
        /// <param name="rptList">路径完成报表</param>
        private void ShowRctEnterPathWc(List<EnterPathStatistics> epsList)
        {
            rctEnterPathWc.SeriesMappings.Clear();

            SeriesMapping seriesMapping = new SeriesMapping();
            seriesMapping.SeriesDefinition = new PieSeriesDefinition();
            seriesMapping.SeriesDefinition.ItemLabelFormat = "#Y%";
            seriesMapping.SeriesDefinition.ShowItemToolTips = true;

            //选中特效
            seriesMapping.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

            //smart labels pie上显示字的位置
            ((RadialSeriesDefinition)seriesMapping.SeriesDefinition).LabelSettings.LabelOffset = 0.7d;

            seriesMapping.SeriesDefinition.ItemToolTipFormat = "统计项目：#PName \r\n 比例：#PCount%";
            ItemMapping itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.YValue;
            itemMapping.FieldName = "PCount";
            seriesMapping.ItemMappings.Add(itemMapping);
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.LegendLabel;
            itemMapping.FieldName = "PName";
            seriesMapping.ItemMappings.Add(itemMapping);
            rctEnterPathWc.SeriesMappings.Add(seriesMapping);

            rctEnterPathWc.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Visible;
            rctEnterPathWc.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = System.Windows.Visibility.Collapsed;
            rctEnterPathWc.DefaultView.ChartArea.AxisX.MinorGridLinesVisibility = System.Windows.Visibility.Visible;
            rctEnterPathWc.DefaultView.ChartTitle.Visibility = System.Windows.Visibility.Visible;
            rctEnterPathWc.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;

            rctEnterPathWc.ItemsSource = epsList;
        }

        /// <summary>
        /// 表示显示路径入径比例图的方法
        /// </summary>
        /// <param name="epsList">入径统计表</param>
        private void ShowRctEnterPathRj(List<EnterPathStatistics> epsList)
        {
            rctEnterPathRj.SeriesMappings.Clear();

            SeriesMapping seriesMapping = new SeriesMapping();
            seriesMapping.SeriesDefinition = new PieSeriesDefinition();
            seriesMapping.SeriesDefinition.ItemLabelFormat = "#Y%";
            seriesMapping.SeriesDefinition.ShowItemToolTips = true;

            //选中特效
            seriesMapping.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

            //smart labels pie上显示字的位置
            ((RadialSeriesDefinition)seriesMapping.SeriesDefinition).LabelSettings.LabelOffset = 0.3d;

            seriesMapping.SeriesDefinition.ItemToolTipFormat = "统计项目：#PName \r\n 比例：#PCount%";
            ItemMapping itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.YValue;
            itemMapping.FieldName = "PCount";
            seriesMapping.ItemMappings.Add(itemMapping);
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.LegendLabel;
            itemMapping.FieldName = "PName";
            seriesMapping.ItemMappings.Add(itemMapping);
            rctEnterPathRj.SeriesMappings.Add(seriesMapping);

            rctEnterPathRj.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Visible;
            rctEnterPathRj.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = System.Windows.Visibility.Collapsed;
            rctEnterPathRj.DefaultView.ChartArea.AxisX.MinorGridLinesVisibility = System.Windows.Visibility.Visible;
            rctEnterPathRj.DefaultView.ChartTitle.Visibility = System.Windows.Visibility.Visible;
            rctEnterPathRj.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;

            rctEnterPathRj.ItemsSource = epsList;
        }

        /// <summary>
        /// 表示获取临床科室列表的方法
        /// </summary>
        private void GetDepartmentListInfo()
        {
            radBusyIndicator.IsBusy = true;

            YidanEHRDataServiceClient GetDepartmentListInfoClient = PublicMethod.YidanClient;
            GetDepartmentListInfoClient.GetDepartmentListInfoCompleted +=
          (obj, e) =>
          {
              radBusyIndicator.IsBusy = false;
              if (e.Error == null)
              {
                  this.cmbOffice.ItemsSource = e.Result;
              }
              else
              {
                  PublicMethod.RadWaringBox(e.Error);
              }
          };
            GetDepartmentListInfoClient.GetDepartmentListInfoAsync();
            GetDepartmentListInfoClient.CloseAsync();
        }

        /// <summary>
        /// 表示获取临床路径列表的方法
        /// </summary>
        private void GetClinicalPathList()
        {
            //radBusyIndicator.IsBusy = true;
            String deptInfo = (cmbOffice.SelectedItem != null) ? (cmbOffice.SelectedItem as CP_DepartmentList).Ksdm : String.Empty;

            YidanEHRDataServiceClient GetClinicalPathListClient = PublicMethod.YidanClient;
            GetClinicalPathListClient.GetClinicalPathListCompleted +=
           (obj, e) =>
           {
               //radBusyIndicator.IsBusy = false;
               if (e.Error == null)
               {
                   autoPath.ItemsSource = e.Result.ToList();
               }
               else
               {
                   PublicMethod.RadWaringBox(e.Error);
               }
           };
            GetClinicalPathListClient.GetClinicalPathListAsync(String.Empty, String.Empty, deptInfo, String.Empty, string.Empty, string.Empty);
            GetClinicalPathListClient.CloseAsync();
        }

        /// <summary>
        /// 获取病种列表的方法
        /// </summary>
        private void GetBzList()
        {
            //radBusyIndicator.IsBusy = true;
            YidanEHRDataServiceClient GetBzListClient = PublicMethod.YidanClient;
            GetBzListClient.GetRpt_ClinicalDiagnosisCompleted +=
                (obj, e) =>
                {
                    //radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        cmbBz.ItemsSource = e.Result.ToList();
                        autoPath.ItemFilter = PathFilter;
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            String deptInfo = (cmbOffice.SelectedItem != null) ? (cmbOffice.SelectedItem as CP_DepartmentList).Ksdm : String.Empty;//科室代码
            GetBzListClient.GetRpt_ClinicalDiagnosisAsync(deptInfo);
            GetBzListClient.CloseAsync();
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        public bool PathFilter(string strFilter, object item)
        {
            CP_ClinicalPathList deptList = (CP_ClinicalPathList)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())));
        }
        #endregion

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cmbOffice.SelectedIndex = -1; cmbGetType.SelectedIndex = -1;
            cmbLjzt.SelectedIndex = -1; autoPath.Text = ""; cmbBz.SelectedIndex = -1;

        }

        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Rpt_PathEnterStatistics ps = (Rpt_PathEnterStatistics)this.gvPathStat.SelectedItem;
                if (ps == null)
                {
                    PublicMethod.RadAlterBox("请选择一条临床路径！", "提示");
                    return;
                }
                YidanEHRApplication.Views.PrintForm.RptPathEnterStatisticsDetail pathDetail = new PrintForm.RptPathEnterStatisticsDetail(ps.Ljdm, dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd"), dtpEndDate.SelectedDate.Value.ToString("yyyy-MM-dd"),ps.Ljmc);
                pathDetail.ShowDialog();
            }
            catch (Exception)
            {
                throw;
            }
        }

    }
}
