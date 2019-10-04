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

using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using Telerik.Windows.Controls.Charting;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls;

namespace YidanEHRApplication.Views.ReportForms
{
    public partial class RptPathStatistics : Page
    {
        ObservableCollection<CheckBox> m_cmbCheckBox = new ObservableCollection<CheckBox>();

        public RptPathStatistics()
        {
            InitializeComponent();
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /// <summary>
        /// 获取临床路径执行概括统计数据列表
        /// </summary>
        private void GetRptPathStatisticList()
        {

            if ((dtpStartDate.SelectedDate > dtpEndDate.SelectedDate) ||
                dtpStartDate.SelectedDate.ToString().Trim() == "" ||
                dtpEndDate.SelectedDate.ToString().Trim() == "")
            {
                PublicMethod.RadAlterBox("开始日期必须小于或等于结束日期，且不能为空！", "提示");
            }
            // modified by zhouhui  科室和临床路径可以为空
            //else if (cmbOffice.SelectedIndex < 0)
            //{
            //    PublicMethod.RadAlterBox("请选择临床科室！", "提示");
            //}
            //else if (cmbPath.EmptyText == "" || cmbPath.EmptyText == null)
            //{
            //    PublicMethod.RadAlterBox("请选择路径名称！", "提示");
            //}
            else
            {
                string path = "";
                string deptInfo = (cmbOffice.SelectedItem != null) ? (cmbOffice.SelectedItem as CP_DepartmentList).Ksdm : string.Empty;
                string startdate = dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                string enddate = ((DateTime)dtpEndDate.SelectedDate).AddDays(1).ToString("yyyy-MM-dd");
                foreach (CheckBox check in m_cmbCheckBox)
                {
                    if (check.IsChecked == true)
                    {
                        path += "~" + check.Tag.ToString() + "~,";
                    }
                }
                path = (path.Length == 0) ? "" : path.Substring(0, path.Length - 1);
                if (path.Length > 1000)
                {
                    PublicMethod.RadAlterBox("选择临床路径项太多！", "提示");
                    return;
                }
                radBusyIndicator.IsBusy = true;
                YidanEHRDataServiceClient GetRptPathStatisticList = PublicMethod.YidanClient;
                GetRptPathStatisticList.GetRptPathStatisticCompleted +=
                (obj, e) =>
                {
                    radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        GridViewPathStat.ItemsSource = e.Result.ToList();

                        //ObservableCollection<RPT_PathStatistic> inpatientFee = e.Result;
                        List<RPT_PathStatistic> rptList = e.Result.ToList();
                        //if (rptList.Count == 0)
                        //{
                        //    RadChartFee.DefaultView.ChartArea = null;
                        //    RadChartDays.DefaultView.ChartArea = null;
                        //    RadChartRate.DefaultView.ChartArea = null;
                        //    //RadChartFee.DefaultView.ChartArea.NoDataString = "无数据...";
                        //    //RadChartDays.DefaultView.ChartArea.NoDataString = "无数据...";
                        //    //RadChartRate.DefaultView.ChartArea.NoDataString = "无数据...";
                        //    return;
                        //}
                        //else
                        //{
                        ShowChartFee(rptList);
                        ShowChartDays(rptList);
                        ShowChartRate(rptList);
                        //}
                        Expander1.Header = "显示图表1";
                        Expander2.Header = "显示图表2";
                        Expander3.Header = "显示图表3";
                        Expander4.Header = "显示全部图表";
                        RadChartFee.Visibility = System.Windows.Visibility.Collapsed;
                        RadChartDays.Visibility = System.Windows.Visibility.Collapsed;
                        RadChartRate.Visibility = System.Windows.Visibility.Collapsed;
                        HideRow0.MaxHeight = 0;
                        HideRow1.MaxHeight = 0;
                        HideRow2.MaxHeight = 0;
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
                //GetRptPathStatisticList.GetRptPathStatisticAsync(
                //   "2054", "~P.K62.001~", dtpStartDate.SelectedDate.ToString(), dtpEndDate.SelectedDate.ToString());
                GetRptPathStatisticList.GetRptPathStatisticAsync(deptInfo, path, startdate, enddate);
                GetRptPathStatisticList.CloseAsync();

            }

        }




        /// <summary>
        /// 显示各临床路径平均住院费用图表
        /// </summary>
        /// <param name="rptList"></param>
        private void ShowChartFee(List<RPT_PathStatistic> rptList)
        {
            RadChartFee.SeriesMappings.Clear();

            //线图表
            SeriesMapping seriesMappingBar = new SeriesMapping();
            seriesMappingBar.LegendLabel = "平均住院费用";
            seriesMappingBar.SeriesDefinition = new SplineSeriesDefinition();
            seriesMappingBar.SeriesDefinition.ItemLabelFormat = "#Y{￥0.00}";
            seriesMappingBar.SeriesDefinition.ShowItemToolTips = true;
            seriesMappingBar.CollectionIndex = 0;

            //选中特效
            seriesMappingBar.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
            seriesMappingBar.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
            seriesMappingBar.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

            seriesMappingBar.SeriesDefinition.ItemToolTipFormat = "平均住院费用：#Jzyfy{￥0.00}";
            ItemMapping itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.YValue;
            itemMapping.FieldName = "Jzyfy";
            seriesMappingBar.ItemMappings.Add(itemMapping);
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.XCategory;
            itemMapping.FieldName = "Ljmc";
            seriesMappingBar.ItemMappings.Add(itemMapping);

            //划线图表
            SeriesMapping seriesMappingLine = new SeriesMapping();
            seriesMappingLine.LegendLabel = "标准住院费用";
            seriesMappingLine.SeriesDefinition = new SplineSeriesDefinition();
            seriesMappingLine.SeriesDefinition.ItemLabelFormat = "#Y{￥0.00}";
            seriesMappingLine.SeriesDefinition.ShowItemToolTips = true;
            seriesMappingLine.CollectionIndex = 1;

            //选中特效
            seriesMappingLine.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
            seriesMappingLine.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
            seriesMappingLine.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

            seriesMappingLine.SeriesDefinition.ItemToolTipFormat = "标准住院费用：#Jcfy{￥0.00}";
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.YValue;
            itemMapping.FieldName = "Jcfy";
            seriesMappingLine.ItemMappings.Add(itemMapping);
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.XCategory;
            itemMapping.FieldName = "Ljmc";
            seriesMappingLine.ItemMappings.Add(itemMapping);

            //Chart加入柱状和线状图表
            RadChartFee.SeriesMappings.Add(seriesMappingBar);
            RadChartFee.SeriesMappings.Add(seriesMappingLine);

            RadChartFee.DefaultView.ChartArea.AxisY.Title = "住院费用";
            RadChartFee.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Visible;
            RadChartFee.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = System.Windows.Visibility.Visible;
            RadChartFee.DefaultView.ChartArea.AxisX.MinorGridLinesVisibility = System.Windows.Visibility.Collapsed;
            RadChartFee.DefaultView.ChartTitle.Visibility = System.Windows.Visibility.Visible;
            RadChartFee.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;
            //LabelRotationAngle
            RadChartFee.DefaultView.ChartArea.AxisX.LabelRotationAngle = 10;

            List<List<RPT_PathStatistic>> itemsSource = new List<List<RPT_PathStatistic>>();
            itemsSource.Add(rptList);
            itemsSource.Add(rptList);
            RadChartFee.ItemsSource = itemsSource;
        }

        /// <summary>
        /// 显示各临床路径平均住院天数图表
        /// </summary>
        /// <param name="eptList"></param>
        private void ShowChartDays(List<RPT_PathStatistic> rptList)
        {
            RadChartDays.SeriesMappings.Clear();

            //线图表
            SeriesMapping seriesMappingBar = new SeriesMapping();
            seriesMappingBar.LegendLabel = "平均住院天数";
            seriesMappingBar.SeriesDefinition = new SplineSeriesDefinition();
            seriesMappingBar.SeriesDefinition.ItemLabelFormat = "#Y";
            seriesMappingBar.SeriesDefinition.ShowItemToolTips = true;
            seriesMappingBar.CollectionIndex = 0;

            //选中特效
            seriesMappingBar.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
            seriesMappingBar.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
            seriesMappingBar.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

            seriesMappingBar.SeriesDefinition.ItemToolTipFormat = "平均住院天数：#Jzyts";
            ItemMapping itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.YValue;
            itemMapping.FieldName = "Jzyts";
            seriesMappingBar.ItemMappings.Add(itemMapping);
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.XCategory;
            itemMapping.FieldName = "Ljmc";
            seriesMappingBar.ItemMappings.Add(itemMapping);

            //划线图表
            SeriesMapping seriesMappingLine = new SeriesMapping();
            seriesMappingLine.LegendLabel = "标准住院天数";
            seriesMappingLine.SeriesDefinition = new SplineSeriesDefinition();
            seriesMappingLine.SeriesDefinition.ItemLabelFormat = "#Y";
            seriesMappingLine.SeriesDefinition.ShowItemToolTips = true;
            seriesMappingLine.CollectionIndex = 1;

            //选中特效
            seriesMappingLine.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
            seriesMappingLine.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
            seriesMappingLine.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

            seriesMappingLine.SeriesDefinition.ItemToolTipFormat = "标准住院天数：#Jcts";
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.YValue;
            itemMapping.FieldName = "Jcts";
            seriesMappingLine.ItemMappings.Add(itemMapping);
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.XCategory;
            itemMapping.FieldName = "Ljmc";
            seriesMappingLine.ItemMappings.Add(itemMapping);

            //Chart加入柱状和线状图表
            RadChartDays.SeriesMappings.Add(seriesMappingBar);
            RadChartDays.SeriesMappings.Add(seriesMappingLine);

            RadChartDays.DefaultView.ChartArea.AxisY.Title = "住院天数";
            RadChartDays.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Visible;
            RadChartDays.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = System.Windows.Visibility.Visible;
            RadChartDays.DefaultView.ChartArea.AxisX.MinorGridLinesVisibility = System.Windows.Visibility.Collapsed;
            RadChartDays.DefaultView.ChartTitle.Visibility = System.Windows.Visibility.Visible;
            RadChartDays.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;
            RadChartDays.DefaultView.ChartArea.AxisX.LabelRotationAngle = 10;

            List<List<RPT_PathStatistic>> itemsSource = new List<List<RPT_PathStatistic>>();
            itemsSource.Add(rptList);
            itemsSource.Add(rptList);
            RadChartDays.ItemsSource = itemsSource;

        }

        /// <summary>
        ///  显示临床路径的入径率图表
        /// </summary>
        /// <param name="eptList"></param>
        private void ShowChartRate(List<RPT_PathStatistic> rptList)
        {
            RadChartRate.SeriesMappings.Clear();

            SeriesMapping seriesMapping = new SeriesMapping();
            seriesMapping.LegendLabel = "入径率";
            seriesMapping.SeriesDefinition = new SplineSeriesDefinition(); //BarSeriesDefinition()柱状
            seriesMapping.SeriesDefinition.ItemLabelFormat = "#Y%";
            seriesMapping.SeriesDefinition.ShowItemToolTips = true;

            //选中特效
            seriesMapping.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

            //smart labels bar上显示字的位置
            //((RadialSeriesDefinition)seriesMapping.SeriesDefinition).LabelSettings.LabelOffset = 0.7d;

            seriesMapping.SeriesDefinition.ItemToolTipFormat = "入径率：#Rjl%";
            ItemMapping itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.YValue;
            itemMapping.FieldName = "Rjl";
            seriesMapping.ItemMappings.Add(itemMapping);
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.XCategory;
            itemMapping.FieldName = "Ljmc";
            seriesMapping.ItemMappings.Add(itemMapping);
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.LegendLabel;
            itemMapping.FieldName = "Ljmc";
            seriesMapping.ItemMappings.Add(itemMapping);
            RadChartRate.SeriesMappings.Add(seriesMapping);

            RadChartRate.DefaultView.ChartArea.AxisY.Title = "入径率";
            RadChartRate.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Visible;
            RadChartRate.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = System.Windows.Visibility.Collapsed;
            RadChartRate.DefaultView.ChartArea.AxisX.MinorGridLinesVisibility = System.Windows.Visibility.Visible;
            RadChartRate.DefaultView.ChartTitle.Visibility = System.Windows.Visibility.Visible;
            RadChartRate.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;
            RadChartRate.DefaultView.ChartArea.AxisX.LabelRotationAngle = 10;

            RadChartRate.ItemsSource = rptList;
        }


        private void btnStat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GetRptPathStatisticList();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.dtpStartDate.SelectedDate = DateTime.Now.AddMonths(-1);
                this.dtpEndDate.SelectedDate = DateTime.Now;

                RadChartFee.DefaultView.ChartTitle.Content = "临床路径平均住院费用(图表1)";
                RadChartFee.DefaultView.ChartLegend.Header = "住院费用";
                RadChartDays.DefaultView.ChartTitle.Content = "临床路径平均住院天数(图表2)";
                RadChartDays.DefaultView.ChartLegend.Header = "住院天数";
                RadChartRate.DefaultView.ChartTitle.Content = "临床路径入径率(图表3)";
                RadChartRate.DefaultView.ChartLegend.Header = "入径率";

                RadChartFee.DefaultView.ChartArea.NoDataString = "无数据...";
                RadChartDays.DefaultView.ChartArea.NoDataString = "无数据...";
                RadChartRate.DefaultView.ChartArea.NoDataString = "无数据...";

                GetDepartmentListInfo();
                GetClinicalPathList();

                btnStat_Click(sender, e);


            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        /// <summary>
        /// 获取临床科室列表
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
        /// 获取临床路径列表
        /// </summary>
        private void GetClinicalPathList()
        {
            radBusyIndicator.IsBusy = true;
            string deptInfo = (cmbOffice.SelectedItem != null) ? (cmbOffice.SelectedItem as CP_DepartmentList).Ksdm : string.Empty;
            //string path = (cmbPath.SelectedItem != null) ? (cmbPath.SelectedItem as CP_ClinicalPathList).Ljdm : string.Empty;
            string startdate = dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            string enddate = ((DateTime)dtpEndDate.SelectedDate).AddDays(1).ToString("yyyy-MM-dd");


            YidanEHRDataServiceClient GetClinicalPathListClient = PublicMethod.YidanClient;
            GetClinicalPathListClient.GetClinicalPathListCompleted +=
           (obj, e) =>
           {
               radBusyIndicator.IsBusy = false;
               if (e.Error == null)
               {
                   m_cmbCheckBox.Clear();
                   cmbPath.ItemsSource = e.Result.ToList();
               }
               else
               {
                   PublicMethod.RadWaringBox(e.Error);
               }
           };
            //GetClinicalPathListClient.GetClinicalPathListAsync(
            //    startdate, enddate, deptInfo, string.Empty);
            GetClinicalPathListClient.GetClinicalPathListAsync(string.Empty, string.Empty, deptInfo, string.Empty, string.Empty, string.Empty);
            GetClinicalPathListClient.CloseAsync();
        }



        private void cmbOffice_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                RadComboBox cb = sender as RadComboBox;
                string deptInfo = (cb.SelectedItem != null) ? (cb.SelectedItem as CP_DepartmentList).Ksdm : string.Empty;
                YidanEHRDataServiceClient GetClinicalPathListClient = PublicMethod.YidanClient;
                GetClinicalPathListClient.GetClinicalPathListCompleted +=
               (obj, ea) =>
               {
                   radBusyIndicator.IsBusy = false;
                   if (ea.Error == null)
                   {
                       m_cmbCheckBox.Clear();
                       cmbPath.ItemsSource = ea.Result.ToList();
                   }
                   else
                   {
                       PublicMethod.RadWaringBox(ea.Error);
                   }
               };
                GetClinicalPathListClient.GetClinicalPathListAsync(string.Empty, string.Empty, deptInfo, string.Empty, string.Empty, string.Empty);
                GetClinicalPathListClient.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        /// <summary>
        /// 隐藏图标
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Expander_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch ((sender as System.Windows.Controls.Expander).Name.ToString())
                {
                    case "Expander1":  //隐藏/显示图标1
                        {
                            if (Expander1.Header.ToString() == "隐藏图表1")
                            {
                                Expander1.Header = "显示图表1";
                                Expander4.Header = "显示全部图表";
                                RadChartFee.Visibility = System.Windows.Visibility.Collapsed;
                                HideRow0.MaxHeight = 0;
                                //HideRow2.MaxHeight = 0;
                            }
                            else
                            {
                                Expander1.Header = "隐藏图表1";
                                Expander4.Header = "隐藏全部图表";
                                RadChartFee.Visibility = System.Windows.Visibility.Visible;
                                HideRow0.MaxHeight = 200;
                                //HideRow2.MaxHeight = 20;
                            }
                            break;
                        }
                    case "Expander2":  //隐藏/显示图标2
                        {
                            if (Expander2.Header.ToString() == "隐藏图表2")
                            {
                                Expander2.Header = "显示图表2";
                                Expander4.Header = "显示全部图表";
                                RadChartDays.Visibility = System.Windows.Visibility.Collapsed;
                                HideRow1.MaxHeight = 0;
                                //HideRow4.MaxHeight = 0;
                            }
                            else
                            {
                                Expander2.Header = "隐藏图表2";
                                Expander4.Header = "隐藏全部图表";
                                RadChartDays.Visibility = System.Windows.Visibility.Visible;
                                HideRow1.MaxHeight = 200;
                            }
                            break;
                        }
                    case "Expander3":  //隐藏/显示图标3
                        {
                            if (Expander3.Header.ToString() == "隐藏图表3")
                            {
                                Expander3.Header = "显示图表3";
                                Expander4.Header = "显示全部图表";
                                RadChartRate.Visibility = System.Windows.Visibility.Collapsed;
                                HideRow2.MaxHeight = 0;
                            }
                            else
                            {
                                Expander3.Header = "隐藏图表3";
                                Expander4.Header = "隐藏全部图表";
                                RadChartRate.Visibility = System.Windows.Visibility.Visible;
                                HideRow2.MaxHeight = 200;
                            }
                            break;
                        }
                    case "Expander4":  //隐藏/显示全部图标
                        {
                            if (Expander4.Header.ToString() == "隐藏全部图表")
                            {
                                Expander1.Header = "显示图表1";
                                Expander2.Header = "显示图表2";
                                Expander3.Header = "显示图表3";
                                Expander4.Header = "显示全部图表";
                                RadChartFee.Visibility = System.Windows.Visibility.Collapsed;
                                RadChartDays.Visibility = System.Windows.Visibility.Collapsed;
                                RadChartRate.Visibility = System.Windows.Visibility.Collapsed;
                                HideRow0.MaxHeight = 0;
                                HideRow1.MaxHeight = 0;
                                HideRow2.MaxHeight = 0;
                            }
                            else
                            {
                                Expander1.Header = "隐藏图表1";
                                Expander2.Header = "隐藏图表2";
                                Expander3.Header = "隐藏图表3";
                                Expander4.Header = "隐藏全部图表";
                                RadChartFee.Visibility = System.Windows.Visibility.Visible;
                                RadChartDays.Visibility = System.Windows.Visibility.Visible;
                                RadChartRate.Visibility = System.Windows.Visibility.Visible;
                                HideRow0.MaxHeight = 200;
                                HideRow1.MaxHeight = 200;
                                HideRow2.MaxHeight = 200;
                            }
                            break;
                        }
                }
                if (Expander1.Header.ToString() == "隐藏图表1" &&
                    Expander2.Header.ToString() == "隐藏图表2" &&
                    Expander3.Header.ToString() == "隐藏图表3")
                {
                    Expander4.Header = "隐藏全部图表";
                }
                if (Expander1.Header.ToString() == "显示图表1" &&
                    Expander2.Header.ToString() == "显示图表2" &&
                    Expander3.Header.ToString() == "显示图表3")
                {
                    Expander4.Header = "显示全部图表";

                    gridchat.Visibility = Visibility.Collapsed;

                    HideRow0.MaxHeight = 0;
                    //LayoutRow.Height = new GridLength(0, GridUnitType.Star);
                }
                else
                {
                    gridchat.Visibility = Visibility.Visible;

                    //LayoutRow.Height = new GridLength(1, GridUnitType.Star);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void chkPath_Loaded(object sender, RoutedEventArgs e)
        {
            if (!m_cmbCheckBox.Contains(sender as CheckBox))
            {
                m_cmbCheckBox.Add(sender as CheckBox);
            }

            //if (pathname.Contains((sender as CheckBox).Content.ToString() + ";"))
            //{
            //    m_cmbCheckBox.Add(sender as CheckBox);
            //}

        }

        /// <summary>
        /// 取消选择临床路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPath_Unchecked(object sender, RoutedEventArgs e)
        {
            string path = "";
            if (this.cmbPath.ItemsSource == null)
                return;
            foreach (CheckBox check in m_cmbCheckBox)
            {
                if (check.Tag.ToString() == (sender as CheckBox).Tag.ToString())
                {
                    check.IsChecked = false;
                }
                if (check.IsChecked == true)
                {
                    path += check.Content.ToString() + ";";
                }
            }

            path = (path.Length > 10) ? path.Substring(0, 10) + "..." : path;
            cmbPath.EmptyText = path;

        }
        /// <summary>
        /// 临床路径集合
        /// </summary>
        String CPList = String.Empty;
        /// <summary>
        /// 选择临床路径
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void chkPath_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                string path = "";
                CPList = String.Empty;
                if (this.cmbPath.ItemsSource == null)
                    return;
                foreach (CheckBox check in m_cmbCheckBox)
                {
                    if (check.Tag.ToString() == (sender as CheckBox).Tag.ToString())
                    {
                        check.IsChecked = true;
                    }
                    if (check.IsChecked == true)
                    {
                        path += check.Content.ToString() + ";";
                        CPList += check.Tag.ToString() + ",";
                    }
                }
                CPList = (path.Length > 0) ? CPList.Substring(0, CPList.Length - 1) : CPList;
                path = (path.Length > 10) ? path.Substring(0, 10) + "..." : path;
                cmbPath.EmptyText = path;

                //path = (path.Length > 10) ? path.Substring(0, 10) + "..." : path;
                //cmbPath.EmptyText = path;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void Expander1_Expanded(object sender, RoutedEventArgs e)
        {
            try
            {
                Expander_Click(sender, e);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void Expander1_Collapsed(object sender, RoutedEventArgs e)
        {
            Expander_Click(sender, e);
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if ((dtpStartDate.SelectedDate > dtpEndDate.SelectedDate) ||
                    dtpStartDate.SelectedDate.ToString().Trim() == "" ||
                    dtpEndDate.SelectedDate.ToString().Trim() == "")
                {
                    PublicMethod.RadAlterBox("开始日期必须小于或等于结束日期，且不能为空！", "提示");
                }

                else
                {
                    string path = "";
                    string deptInfo = (cmbOffice.SelectedItem != null) ? (cmbOffice.SelectedItem as CP_DepartmentList).Ksdm : string.Empty;
                    string startdate = dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                    string enddate = ((DateTime)dtpEndDate.SelectedDate).AddDays(1).ToString("yyyy-MM-dd");

                    string pathName = "";
                    foreach (CheckBox check in m_cmbCheckBox)
                    {
                        if (check.IsChecked == true)
                        {
                            path += "~" + check.Tag.ToString() + "~,";

                            pathName += check.Content.ToString() + ",";
                        }
                    }
                    path = (path.Length == 0) ? "" : path.Substring(0, path.Length - 1);
                    if (path.Length > 1000)
                    {
                        PublicMethod.RadAlterBox("选择临床路径项太多！", "提示");
                        return;
                    }

                    string deptName = (cmbOffice.SelectedItem != null) ? (cmbOffice.SelectedItem as CP_DepartmentList).Name : string.Empty;

                    RptPathStatisticsPrint statisprint = new RptPathStatisticsPrint();
                    statisprint.m_BeginTime = startdate;
                    statisprint.m_EndTime = enddate;
                    statisprint.m_Ljdm = path;
                    statisprint.m_Dept = deptInfo;
                    statisprint.m_DeptName = deptName;
                    statisprint.m_LjdmName = pathName;
                    statisprint.WindowState = WindowState.Maximized;
                    statisprint.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cmbOffice.SelectedIndex = -1;
            cmbPath.Text = "";
            foreach (CheckBox check in m_cmbCheckBox)
            {
                check.IsChecked = false;
            }
        }

        private void cmbPath_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            cmbPath.Text = "";
        }

        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RPT_PathStatistic ps = (RPT_PathStatistic)this.GridViewPathStat.SelectedItem;
                if (ps == null)
                {
                    PublicMethod.RadAlterBox("请选择一条临床路径！", "提示");
                    return;
                }
                YidanEHRApplication.Views.PrintForm.RptPathStatisticDetail pathDetail = new PrintForm.RptPathStatisticDetail(ps.Ljdm, dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd"), dtpEndDate.SelectedDate.Value.ToString("yyyy-MM-dd"), ps.Ljmc);
                pathDetail.ShowDialog();
            }
            catch (Exception)
            {
                throw;
            }

        }



    }
}
