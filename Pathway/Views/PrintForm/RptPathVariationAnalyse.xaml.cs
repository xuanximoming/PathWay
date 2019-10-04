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

namespace YidanEHRApplication.Views.ReportForms
{
    public partial class RptPathVariationAnalyse : Page
    {
        public RptPathVariationAnalyse()
        {
            InitializeComponent();
        }
        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        ObservableCollection<CheckBox> m_cmbCheckBox = new ObservableCollection<CheckBox>();

        #region 事件
        /// <summary>
        /// 页面加载
        /// (4.27注释，开始修改)
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.dtpStartDate.SelectedDate = DateTime.Now.AddMonths(-1);
                this.dtpEndDate.SelectedDate = DateTime.Now;
                #region 统计图
                RadChartPie.DefaultView.ChartTitle.Content = "变异情况分析扇形图(图表1)";
                RadChartPie.DefaultView.ChartLegend.Header = "变异情况分析";
                //RadChartPolyline.DefaultView.ChartTitle.Content = "变异情况分析折线图(图表1)";
                //RadChartPolyline.DefaultView.ChartLegend.Header = "变异情况分析";
                //RadChartRate.DefaultView.ChartTitle.Content = "变异情况分析(图表3)";
                //RadChartRate.DefaultView.ChartLegend.Header = "变异情况分析";

                RadChartPie.DefaultView.ChartArea.NoDataString = "无数据...";
                //RadChartPolyline.DefaultView.ChartArea.NoDataString = "无数据..."; 
                //RadChartRate.DefaultView.ChartArea.NoDataString = "无数据...";
                #endregion
                GetDepartmentListInfo();
                GetClinicalPathList();
                btnAnalyse_Click(sender, e);
                Expander1_Collapsed(sender, e);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        /// <summary>
        /// 科室选择影响路径加载
        /// (4.19添加注释，4.27修改)
        /// </summary>
        private void cmbOffice_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            RadComboBox cb = sender as RadComboBox;
            string deptInfo = (cb.SelectedItem != null) ? (cb.SelectedItem as CP_DepartmentList).Ksdm : string.Empty;
            LoadClinicalPathList(deptInfo);
        }
        #region 多选框
        /// <summary>
        /// 路径复选框
        /// (4.19添加注释)
        /// </summary>
        private void chkPath_Loaded(object sender, RoutedEventArgs e)
        {
            //if (!m_cmbCheckBox.Contains(sender as CheckBox))
            //{
            //    m_cmbCheckBox.Add(sender as CheckBox);
            //}
        }
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
                    }
                }
                path = (path.Length > 10) ? path.Substring(0, 10) + "..." : path;
                cmbPath.EmptyText = path;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

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
        #endregion
        /// <summary>
        /// 统计按钮（4.27）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAnalyse_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GridViewAnalyse.ItemsSource = null;                //清空原有数据
                RadChartPie.ItemsSource = null;
                //RadChartPolyline.ItemsSource = null;
                //RadChartRate..ItemsSource = null;

                RadChartPie.DefaultView.ChartArea.NoDataString = "无数据...";
                //RadChartPolyline.DefaultView.ChartArea.NoDataString = "无数据...";
                //RadChartRate.DefaultView.ChartArea.NoDataString = "无数据...";

                GetRptPathVariationAnalyse();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #region 伸缩栏

        /// <summary>
        /// 5.4修改为一栏
        /// </summary>
        private void Expander1_Collapsed(object sender, RoutedEventArgs e)
        {
            Expander1.Header = "显示图表";
            RadChartPie.Visibility = System.Windows.Visibility.Collapsed;
            HideRow0.MaxHeight = 0;
        }
        private void Expander1_Expanded(object sender, RoutedEventArgs e)
        {
            Expander1.Header = "隐藏图表";
            RadChartPie.Visibility = System.Windows.Visibility.Visible;
            HideRow0.MaxHeight = 200;
        }
        ///// <summary>
        ///// 收缩栏
        ///// （4.22添加注释）
        ///// </summary>
        //private void Expander_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        switch ((sender as System.Windows.Controls.Expander).Name.ToString())
        //        {
        //            case "Expander1":  //隐藏/显示图标1
        //                {
        //                    if (Expander1.Header.ToString() == "隐藏图表1")
        //                    {
        //                        Expander1.Header = "显示图表1";
        //                        Expander4.Header = "显示全部图表";
        //                        RadChartPolyline.Visibility = System.Windows.Visibility.Collapsed;
        //                        HideRow0.MaxHeight = 0;
        //                        //HideRow2.MaxHeight = 0;
        //                    }
        //                    else
        //                    {
        //                        Expander1.Header = "隐藏图表1";
        //                        Expander4.Header = "隐藏全部图表";
        //                        RadChartPolyline.Visibility = System.Windows.Visibility.Visible;
        //                        HideRow0.MaxHeight = 200;
        //                        //HideRow2.MaxHeight = 20;
        //                    }
        //                    break;
        //                }
        //            case "Expander2":  //隐藏/显示图标2
        //                {
        //                    if (Expander2.Header.ToString() == "隐藏图表2")
        //                    {
        //                        Expander2.Header = "显示图表2";
        //                        Expander4.Header = "显示全部图表";
        //                        RadChartPie.Visibility = System.Windows.Visibility.Collapsed;
        //                        HideRow1.MaxHeight = 0;
        //                        //HideRow4.MaxHeight = 0;
        //                    }
        //                    else
        //                    {
        //                        Expander2.Header = "隐藏图表2";
        //                        Expander4.Header = "隐藏全部图表";
        //                        RadChartPie.Visibility = System.Windows.Visibility.Visible;
        //                        HideRow1.MaxHeight = 200;
        //                    }
        //                    break;
        //                }
        //            case "Expander3":  //隐藏/显示图标3
        //                {
        //                    if (Expander3.Header.ToString() == "隐藏图表3")
        //                    {
        //                        Expander3.Header = "显示图表3";
        //                        Expander4.Header = "显示全部图表";
        //                        RadChartRate.Visibility = System.Windows.Visibility.Collapsed;
        //                        HideRow2.MaxHeight = 0;
        //                    }
        //                    else
        //                    {
        //                        Expander3.Header = "隐藏图表3";
        //                        Expander4.Header = "隐藏全部图表";
        //                        RadChartRate.Visibility = System.Windows.Visibility.Visible;
        //                        HideRow2.MaxHeight = 200;
        //                    }
        //                    break;
        //                }
        //            case "Expander4":  //隐藏/显示全部图标
        //                {
        //                    if (Expander4.Header.ToString() == "隐藏全部图表")
        //                    {
        //                        Expander1.Header = "显示图表1";
        //                        Expander2.Header = "显示图表2";
        //                        Expander3.Header = "显示图表3";
        //                        Expander4.Header = "显示全部图表";
        //                        RadChartPolyline.Visibility = System.Windows.Visibility.Collapsed;
        //                        RadChartPie.Visibility = System.Windows.Visibility.Collapsed;
        //                        RadChartRate.Visibility = System.Windows.Visibility.Collapsed;
        //                        HideRow0.MaxHeight = 0;
        //                        HideRow1.MaxHeight = 0;
        //                        HideRow2.MaxHeight = 0;
        //                    }
        //                    else
        //                    {
        //                        Expander1.Header = "隐藏图表1";
        //                        Expander2.Header = "隐藏图表2";
        //                        Expander3.Header = "隐藏图表3";
        //                        Expander4.Header = "隐藏全部图表";
        //                        RadChartPolyline.Visibility = System.Windows.Visibility.Visible;
        //                        RadChartPie.Visibility = System.Windows.Visibility.Visible;
        //                        RadChartRate.Visibility = System.Windows.Visibility.Visible;
        //                        HideRow0.MaxHeight = 200;
        //                        HideRow1.MaxHeight = 200;
        //                        HideRow2.MaxHeight = 200;
        //                    }
        //                    break;
        //                }
        //        }
        //        if (Expander1.Header.ToString() == "隐藏图表1" &&
        //            Expander2.Header.ToString() == "隐藏图表2" &&
        //            Expander3.Header.ToString() == "隐藏图表3")
        //        {
        //            Expander4.Header = "隐藏全部图表";
        //        }
        //        if (Expander1.Header.ToString() == "显示图表1" &&
        //            Expander2.Header.ToString() == "显示图表2" &&
        //            Expander3.Header.ToString() == "显示图表3")
        //        {
        //            Expander4.Header = "显示全部图表";

        //            gridchat.Visibility = Visibility.Collapsed;

        //            HideRow0.MaxHeight = 0;
        //            //LayoutRow.Height = new GridLength(0, GridUnitType.Star);
        //        }
        //        else
        //        {
        //            gridchat.Visibility = Visibility.Visible;

        //            //LayoutRow.Height = new GridLength(1, GridUnitType.Star);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
        //    }
        //}
        //private void Expander1_Expanded(object sender, RoutedEventArgs e)
        //{
        //    Expander_Click(sender, e);
        //}
        //private void Expander1_Collapsed(object sender, RoutedEventArgs e)
        //{
        //    Expander_Click(sender, e);
        //}
        #endregion
        /// <summary>
        /// 打印（4.28修改）
        /// </summary>
        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Rpt_QueryCondition queryCondition = new Rpt_QueryCondition();

                if (LoadBtnClick(queryCondition))
                {
                    queryCondition.DeptName = (cmbOffice.SelectedItem != null) ? (cmbOffice.SelectedItem as CP_DepartmentList).Name : string.Empty;
                    queryCondition.PathName = "";

                    foreach (CheckBox check in m_cmbCheckBox)
                    {
                        if (check.IsChecked == true)
                        {
                            queryCondition.PathName += check.Content.ToString() + ",";
                        }
                    }
                    RptPathVariationAnalysePrint print = new RptPathVariationAnalysePrint();
                    print.printCondition.Stardate = queryCondition.Stardate;
                    print.printCondition.Enddate = queryCondition.Enddate;
                    print.printCondition.DeptInfo = queryCondition.DeptInfo;
                    print.printCondition.Path = queryCondition.Path;
                    print.printCondition.PathName = queryCondition.PathName;
                    print.printCondition.DeptName = queryCondition.DeptName;
                    print.WindowState = WindowState.Maximized;
                    print.ShowDialog();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #endregion

        #region 函数

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
        /// 加载路径（4.27）
        /// </summary>
        private void LoadClinicalPathList(string deptInfo)
        {
            try
            {

                YidanEHRDataServiceClient GetClinicalPathListClient = PublicMethod.YidanClient;
                GetClinicalPathListClient.GetClinicalPathListCompleted +=
                (obj, e) =>
                {
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
                GetClinicalPathListClient.GetClinicalPathListAsync(string.Empty, string.Empty, deptInfo, string.Empty, string.Empty, string.Empty);
                GetClinicalPathListClient.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 获取临床路径列表（4.27修改）
        /// </summary>
        private void GetClinicalPathList()
        {
            string deptInfo = (cmbOffice.SelectedItem != null) ? (cmbOffice.SelectedItem as CP_DepartmentList).Ksdm : string.Empty;
            LoadClinicalPathList(deptInfo);
        }
        /// <summary>
        /// 按钮加载事件(4.28)
        /// </summary>
        private bool LoadBtnClick(Rpt_QueryCondition queryConditioin)
        {
            if ((dtpStartDate.SelectedDate > dtpEndDate.SelectedDate) ||
               dtpStartDate.SelectedDate.ToString().Trim() == "" ||
               dtpEndDate.SelectedDate.ToString().Trim() == "")
            {
                PublicMethod.RadAlterBox("开始日期必须小于或等于结束日期，且不能为空！", "提示");
                return false;
            }
            else
            {
                queryConditioin.Path = "";
                queryConditioin.DeptInfo = (cmbOffice.SelectedItem != null) ? (cmbOffice.SelectedItem as CP_DepartmentList).Ksdm : string.Empty;
                queryConditioin.Stardate = dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                queryConditioin.Enddate = ((DateTime)dtpEndDate.SelectedDate).AddDays(1).ToString("yyyy-MM-dd");

                foreach (CheckBox check in m_cmbCheckBox)
                {
                    if (check.IsChecked == true)
                    {
                        queryConditioin.Path += "~" + check.Tag.ToString() + "~,";
                    }
                }
                queryConditioin.Path = (queryConditioin.Path.Length == 0) ? "" : queryConditioin.Path.Substring(0, queryConditioin.Path.Length - 1);
                if (queryConditioin.Path.Length > 1000)
                {
                    PublicMethod.RadAlterBox("选择临床路径项太多！", "提示");
                    return false; ;
                }
                return true;
            }
        }
        /// <summary>
        /// 获取变异情况分析列表（4.27开始编写）
        /// </summary>
        private void GetRptPathVariationAnalyse()
        {
            try
            {
                Rpt_QueryCondition queryConditioin = new Rpt_QueryCondition();

                if (LoadBtnClick(queryConditioin))
                {
                    radBusyIndicator.IsBusy = true;

                    YidanEHRDataServiceClient GetRptPathVariationAnalyseList = PublicMethod.YidanClient;
                    GetRptPathVariationAnalyseList.GetRptPathVariationAnalyseCompleted +=
                    (obj, e) =>
                    {
                        radBusyIndicator.IsBusy = false;
                        if (e.Error == null)
                        {
                            Rpt_PathVariationAnalyse rpt = e.Result;
                            if (rpt.PathVariationAnalyseList == null)                    //如果经过筛选后数据为空，则需跳出
                            {
                                return;
                            }
                            else
                            {
                                List<Rpt_PathVariationAnalyseList> rptList = rpt.PathVariationAnalyseList.ToList();          //如果为空不跳出，则会在此处转化NULL报错
                                GridViewAnalyse.ItemsSource = rptList;
                                ShowRadChartPie(rptList);
                                //ShowChartPolyline(rptList);
                            }
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };

                    GetRptPathVariationAnalyseList.GetRptPathVariationAnalyseAsync(queryConditioin.Stardate, queryConditioin.Enddate, queryConditioin.Path, queryConditioin.DeptInfo);
                    GetRptPathVariationAnalyseList.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        #region 统计图
        /// <summary>
        /// 加载扇形图(4.27注释，开始编写)
        /// </summary>
        private void ShowRadChartPie(List<Rpt_PathVariationAnalyseList> rptList)
        {
            RadChartPie.SeriesMappings.Clear();

            SeriesMapping seriesMapping = new SeriesMapping();
            seriesMapping.SeriesDefinition = new PieSeriesDefinition();
            seriesMapping.SeriesDefinition.ItemLabelFormat = "#Y";
            seriesMapping.SeriesDefinition.ShowItemToolTips = true;

            //选中特效
            seriesMapping.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

            //smart labels pie上显示字的位置
            ((RadialSeriesDefinition)seriesMapping.SeriesDefinition).LabelSettings.LabelOffset = 0.7d;

            seriesMapping.SeriesDefinition.ItemToolTipFormat = "变异编码：#BymcRd \r\n 数量：#VariationCount";  //"项目: #Xmmc\r\n比例：#%{p0}\r\n总计：#Zj"; //"项目: #Xmmc\r\n金额: #Y{0.00}"
            ItemMapping itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.YValue;
            itemMapping.FieldName = "VariationCount";
            seriesMapping.ItemMappings.Add(itemMapping);
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.LegendLabel;
            itemMapping.FieldName = "BymcRd";
            seriesMapping.ItemMappings.Add(itemMapping);
            RadChartPie.SeriesMappings.Add(seriesMapping);

            RadChartPie.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Visible;
            RadChartPie.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = System.Windows.Visibility.Collapsed;
            RadChartPie.DefaultView.ChartArea.AxisX.MinorGridLinesVisibility = System.Windows.Visibility.Visible;
            RadChartPie.DefaultView.ChartTitle.Visibility = System.Windows.Visibility.Visible;
            RadChartPie.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;

            RadChartPie.ItemsSource = rptList;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cmbOffice.SelectedIndex = -1;
            cmbPath.Text = "";
        }

        private void cmbPath_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            cmbPath.Text = "";
        }

        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Rpt_PathVariationAnalyseList pv = (Rpt_PathVariationAnalyseList)this.GridViewAnalyse.SelectedItem;
                if (pv == null)
                {
                    PublicMethod.RadAlterBox("请选择一条记录！", "提示");
                    return;
                }
                YidanEHRApplication.Views.PrintForm.RptPathVariationAnalyseDetail pathDetail = new PrintForm.RptPathVariationAnalyseDetail(pv.Bydm, dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd"), dtpEndDate.SelectedDate.Value.ToString("yyyy-MM-dd"),pv.BymcRd);
                pathDetail.ShowDialog();
            }
            catch (Exception)
            {
                throw;
            }
        }
        ///// <summary>
        ///// 加载折线图(4.27注释，开始编写)
        ///// </summary>
        //private void ShowChartPolyline(List<Rpt_PathVariationAnalyseList> rptList)
        //{
        //    RadChartPolyline.SeriesMappings.Clear();

        //    SeriesMapping seriesMappingBar = new SeriesMapping();
        //    seriesMappingBar.LegendLabel = "变异数";                                //右侧线条描述
        //    seriesMappingBar.SeriesDefinition = new SplineSeriesDefinition();
        //    //seriesMappingBar.SeriesDefinition.ItemLabelFormat = "#Y{0.00}";                //端点LABLE显示方式
        //    seriesMappingBar.SeriesDefinition.ShowItemToolTips = true;                  //是否给绘制的点加LABLE
        //    seriesMappingBar.CollectionIndex = 0;                                       //第几个线！！！（很重要，有0才有1）


        //    //选中特效
        //    seriesMappingBar.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
        //    seriesMappingBar.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
        //    seriesMappingBar.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

        //    seriesMappingBar.SeriesDefinition.ItemToolTipFormat = "变异数：#VariationCount{0}";       //鼠标指向后显示
        //    ItemMapping itemMapping = new ItemMapping();
        //    itemMapping.DataPointMember = DataPointMember.YValue;
        //    itemMapping.FieldName = "VariationCount";
        //    seriesMappingBar.ItemMappings.Add(itemMapping);
        //    itemMapping = new ItemMapping();
        //    itemMapping.DataPointMember = DataPointMember.XCategory;
        //    itemMapping.FieldName = "BymcRd";
        //    seriesMappingBar.ItemMappings.Add(itemMapping);

        //    //Chart加入图表
        //    RadChartPolyline.SeriesMappings.Add(seriesMappingBar);

        //    RadChartPolyline.DefaultView.ChartArea.AxisY.Title = "变异情况分析";
        //    RadChartPolyline.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Visible;
        //    RadChartPolyline.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = System.Windows.Visibility.Visible;
        //    RadChartPolyline.DefaultView.ChartArea.AxisX.MinorGridLinesVisibility = System.Windows.Visibility.Collapsed;
        //    RadChartPolyline.DefaultView.ChartTitle.Visibility = System.Windows.Visibility.Visible;
        //    RadChartPolyline.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;


        //    List<List<Rpt_PathVariationAnalyseList>> itemsSource = new List<List<Rpt_PathVariationAnalyseList>>();
        //    itemsSource.Add(rptList); ;
        //    RadChartPolyline.ItemsSource = itemsSource;
        //}
        #endregion

        #endregion

    }
}
