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
    public partial class RptPathQuitMonthCompare : Page
    {
        public RptPathQuitMonthCompare()
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
        /// (4.19注释，开始修改)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.dtpStartDate.SelectedDate = DateTime.Now;
                #region 统计图
                RadChartPie.DefaultView.ChartTitle.Content = "月度出径环比扇形图(图表1)";
                RadChartPie.DefaultView.ChartLegend.Header = "月度出径环比";
                //RadChartPolyline.DefaultView.ChartTitle.Content = "月度出径环比折线图(图表2)";
                //RadChartPolyline.DefaultView.ChartLegend.Header = "月度出径环比";
                //RadChartRate.DefaultView.ChartTitle.Content = "月度出径环比(图表3)";
                //RadChartRate.DefaultView.ChartLegend.Header = "月度出径环比";

                RadChartPie.DefaultView.ChartArea.NoDataString = "无数据...";
                //RadChartPolyline.DefaultView.ChartArea.NoDataString = "无数据...";
                //RadChartRate.DefaultView.ChartArea.NoDataString = "无数据...";
                #endregion
                GetDepartmentListInfo();
                GetClinicalPathList();

                btnComp_Click(sender, e);

                Expander1_Collapsed(sender, e);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        /// <summary>
        /// 科室选择影响路径加载
        /// (4.19添加注释,4.27修改)
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
            if (!m_cmbCheckBox.Contains(sender as CheckBox))
            {
                m_cmbCheckBox.Add(sender as CheckBox);
            }
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
        /// 统计按钮
        /// (4.19添加注释,4.27修改)
        /// </summary>
        private void btnComp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GridViewComp.ItemsSource = null;                //清空原有数据
                RadChartPie.ItemsSource = null;
                //RadChartPolyline.ItemsSource = null;
                //RadChartRate..ItemsSource = null;

                RadChartPie.DefaultView.ChartArea.NoDataString = "无数据...";
                //RadChartPolyline.DefaultView.ChartArea.NoDataString = "无数据...";
                //RadChartRate.DefaultView.ChartArea.NoDataString = "无数据...";

                GetRptPathQuitMonthList();

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
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
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
        /// 单选时触发（4.22开始编写）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewComp_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            try
            {
                Rpt_PathQuitMonthCompareList rptList = (Rpt_PathQuitMonthCompareList)GridViewComp.SelectedItem;

                List<Rpt_PathQuitMonthCompareImage> rptImgList = new List<Rpt_PathQuitMonthCompareImage>();


                Rpt_PathQuitMonthCompareImage rptImg1 = new Rpt_PathQuitMonthCompareImage();
                rptImg1.Name = "一月";
                rptImg1.Count = rptList.Jan;
                rptImgList.Add(rptImg1);

                Rpt_PathQuitMonthCompareImage rptImg2 = new Rpt_PathQuitMonthCompareImage();
                rptImg2.Name = "二月";
                rptImg2.Count = rptList.Feb;
                rptImgList.Add(rptImg2);

                Rpt_PathQuitMonthCompareImage rptImg3 = new Rpt_PathQuitMonthCompareImage();
                rptImg3.Name = "三月";
                rptImg3.Count = rptList.Mar;
                rptImgList.Add(rptImg3);

                Rpt_PathQuitMonthCompareImage rptImg4 = new Rpt_PathQuitMonthCompareImage();
                rptImg4.Name = "四月";
                rptImg4.Count = rptList.Apr;
                rptImgList.Add(rptImg4);

                Rpt_PathQuitMonthCompareImage rptImg5 = new Rpt_PathQuitMonthCompareImage();
                rptImg5.Name = "五月";
                rptImg5.Count = rptList.May;
                rptImgList.Add(rptImg5);

                Rpt_PathQuitMonthCompareImage rptImg6 = new Rpt_PathQuitMonthCompareImage();
                rptImg6.Name = "六月";
                rptImg6.Count = rptList.Jun;
                rptImgList.Add(rptImg6);

                Rpt_PathQuitMonthCompareImage rptImg7 = new Rpt_PathQuitMonthCompareImage();
                rptImg7.Name = "七月";
                rptImg7.Count = rptList.Jul;
                rptImgList.Add(rptImg7);

                Rpt_PathQuitMonthCompareImage rptImg8 = new Rpt_PathQuitMonthCompareImage();
                rptImg8.Name = "八月";
                rptImg8.Count = rptList.Aug;
                rptImgList.Add(rptImg8);

                Rpt_PathQuitMonthCompareImage rptImg9 = new Rpt_PathQuitMonthCompareImage();
                rptImg9.Name = "九月";
                rptImg9.Count = rptList.Sept;
                rptImgList.Add(rptImg9);

                Rpt_PathQuitMonthCompareImage rptImg10 = new Rpt_PathQuitMonthCompareImage();
                rptImg10.Name = "十月";
                rptImg10.Count = rptList.Oct;
                rptImgList.Add(rptImg10);

                Rpt_PathQuitMonthCompareImage rptImg11 = new Rpt_PathQuitMonthCompareImage();
                rptImg11.Name = "十一";
                rptImg11.Count = rptList.Nov;
                rptImgList.Add(rptImg11);

                Rpt_PathQuitMonthCompareImage rptImg12 = new Rpt_PathQuitMonthCompareImage();
                rptImg12.Name = "十二月";
                rptImg12.Count = rptList.Dec;
                rptImgList.Add(rptImg12);


                //ShowChartPolyline(rptImgList);
                ShowRadChartPie(rptImgList);

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 打印按钮
        /// （4.28修改）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    RptPathQuitMonthComparePrint print = new RptPathQuitMonthComparePrint();
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
            if (dtpStartDate.SelectedDate.ToString().Trim() == "" || dtpStartDate.SelectedDate > DateTime.Now)
            {
                PublicMethod.RadAlterBox("选择日期不能大于当前日期，且不能为空！", "提示");
                return false;
            }
            else
            {
                queryConditioin.Path = "";
                queryConditioin.DeptInfo = (cmbOffice.SelectedItem != null) ? (cmbOffice.SelectedItem as CP_DepartmentList).Ksdm : string.Empty;
                queryConditioin.Stardate = dtpStartDate.SelectedDate.Value.ToString("yyyy");
                queryConditioin.Enddate = (Convert.ToInt32(queryConditioin.Stardate) + 1).ToString();         //加一年

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
                    return false;
                }
                return true;
            }

        }
        /// <summary>
        /// 获取月度出径数据列表
        /// (4.19注释，开始编写)
        /// </summary>
        private void GetRptPathQuitMonthList()
        {
            try
            {
                Rpt_QueryCondition queryConditioin = new Rpt_QueryCondition();

                if (LoadBtnClick(queryConditioin))
                {
                    radBusyIndicator.IsBusy = true;

                    YidanEHRDataServiceClient GetRptPathQuitMonthCompareList = PublicMethod.YidanClient;
                    GetRptPathQuitMonthCompareList.GetRptPathQuitMonthCompareCompleted +=
                    (obj, e) =>
                    {
                        radBusyIndicator.IsBusy = false;
                        if (e.Error == null)
                        {
                            Rpt_PathQuitMonthCompare rpt = e.Result;
                            if (rpt.PathCompareList == null)                    //如果经过筛选后数据为空，则需跳出
                            {
                                return;
                            }
                            else
                            {
                                List<Rpt_PathQuitMonthCompareList> rptList = rpt.PathCompareList.ToList();          //如果为空不跳出，则会在此处转化NULL报错
                                GridViewComp.ItemsSource = rptList;
                            }
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
                    GetRptPathQuitMonthCompareList.GetRptPathQuitMonthCompareAsync(queryConditioin.Stardate, queryConditioin.Enddate, queryConditioin.Path, queryConditioin.DeptInfo);
                    GetRptPathQuitMonthCompareList.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        #region 统计图
        /// <summary>
        /// 加载扇形图(4.22注释，开始编写)
        /// </summary>
        /// <param name="rptList"></param>
        private void ShowRadChartPie(List<Rpt_PathQuitMonthCompareImage> rptImgList)
        {
            RadChartPie.SeriesMappings.Clear();

            SeriesMapping seriesMapping = new SeriesMapping();
            seriesMapping.SeriesDefinition = new PieSeriesDefinition();
            seriesMapping.SeriesDefinition.ItemLabelFormat = "#Y";                     //标签显示
            seriesMapping.SeriesDefinition.ShowItemToolTips = true;

            //选中特效
            seriesMapping.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

            //smart labels pie上显示字的位置
            ((RadialSeriesDefinition)seriesMapping.SeriesDefinition).LabelSettings.LabelOffset = 0.7d;

            seriesMapping.SeriesDefinition.ItemToolTipFormat = "月份：#Name \r\n 数量：#Count";  //"项目: #Xmmc\r\n比例：#%{p0}\r\n总计：#Zj"; //"项目: #Xmmc\r\n金额: #Y{0.00}"
            ItemMapping itemMapping = new ItemMapping();                                    //鼠标放上后突出显示为
            itemMapping.DataPointMember = DataPointMember.YValue;
            itemMapping.FieldName = "Count";
            seriesMapping.ItemMappings.Add(itemMapping);
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.LegendLabel;
            itemMapping.FieldName = "Name";
            seriesMapping.ItemMappings.Add(itemMapping);
            RadChartPie.SeriesMappings.Add(seriesMapping);

            RadChartPie.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Visible;
            RadChartPie.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = System.Windows.Visibility.Collapsed;
            RadChartPie.DefaultView.ChartArea.AxisX.MinorGridLinesVisibility = System.Windows.Visibility.Visible;
            RadChartPie.DefaultView.ChartTitle.Visibility = System.Windows.Visibility.Visible;
            RadChartPie.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;

            RadChartPie.ItemsSource = rptImgList;
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
                Rpt_PathQuitMonthCompareList ps = (Rpt_PathQuitMonthCompareList)this.GridViewComp.SelectedItem;
                if (ps == null)
                {
                    PublicMethod.RadAlterBox("请选择一条临床路径！", "提示");
                    return;
                }
                YidanEHRApplication.Views.PrintForm.RptPathQuitMonthCompareDetail pathDetail = new PrintForm.RptPathQuitMonthCompareDetail(ps.Ljdm, dtpStartDate.SelectedDate.Value.ToString("yyyy"),ps.Ljmc);
                pathDetail.ShowDialog();
            }
            catch (Exception)
            {
                throw;
            }
        }
        ///// <summary>
        ///// 加载折线图(4.22注释，开始编写)
        ///// </summary>
        //private void ShowChartPolyline(List<Rpt_PathQuitMonthCompareImage> rptImgList)
        //{
        //    RadChartPolyline.SeriesMappings.Clear();

        //    SeriesMapping seriesMappingBar = new SeriesMapping();
        //    seriesMappingBar.LegendLabel = "月度出径数";                                //右侧线条描述
        //    seriesMappingBar.SeriesDefinition = new SplineSeriesDefinition();
        //    //seriesMappingBar.SeriesDefinition.ItemLabelFormat = "#Y{0.00}";                //端点LABLE显示方式
        //    seriesMappingBar.SeriesDefinition.ShowItemToolTips = true;                  //是否给绘制的点加LABLE
        //    seriesMappingBar.CollectionIndex = 0;                                       //第几个线！！！（很重要，有0才有1）


        //    //选中特效
        //    seriesMappingBar.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
        //    seriesMappingBar.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
        //    seriesMappingBar.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

        //    seriesMappingBar.SeriesDefinition.ItemToolTipFormat = "月度出径数：#Count{0}";       //鼠标指向后显示
        //    ItemMapping itemMapping = new ItemMapping();
        //    itemMapping.DataPointMember = DataPointMember.YValue;
        //    itemMapping.FieldName = "Count";
        //    seriesMappingBar.ItemMappings.Add(itemMapping);
        //    itemMapping = new ItemMapping();
        //    itemMapping.DataPointMember = DataPointMember.XCategory;
        //    itemMapping.FieldName = "Name";
        //    seriesMappingBar.ItemMappings.Add(itemMapping);

        //    //Chart加入图表
        //    RadChartPolyline.SeriesMappings.Add(seriesMappingBar);

        //    RadChartPolyline.DefaultView.ChartArea.AxisY.Title = "月度出径环比";
        //    RadChartPolyline.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Visible;
        //    RadChartPolyline.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = System.Windows.Visibility.Visible;
        //    RadChartPolyline.DefaultView.ChartArea.AxisX.MinorGridLinesVisibility = System.Windows.Visibility.Collapsed;
        //    RadChartPolyline.DefaultView.ChartTitle.Visibility = System.Windows.Visibility.Visible;
        //    RadChartPolyline.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;


        //    List<List<Rpt_PathQuitMonthCompareImage>> itemsSource = new List<List<Rpt_PathQuitMonthCompareImage>>();
        //    itemsSource.Add(rptImgList); ;
        //    RadChartPolyline.ItemsSource = itemsSource;

        //}

        #endregion

        #endregion

    }
}
