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
using Telerik.Windows.Controls.Charting;

namespace YidanEHRApplication.Views.ReportForms
{
    public partial class RptPathQuit : Page
    {
        ObservableCollection<CheckBox> m_cmbCheckBox = new ObservableCollection<CheckBox>();
        public List<Rpt_PathQuitPie> lstPie = new List<Rpt_PathQuitPie>();

        public RptPathQuit()
        {
            InitializeComponent();
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.dtpStartDate.SelectedDate = DateTime.Now.AddMonths(-1);
                this.dtpEndDate.SelectedDate = DateTime.Now;

                RadChartPathQuit.DefaultView.ChartTitle.Content = "临床路径退出原因";
                RadChartPathQuit.DefaultView.ChartLegend.Header = "原因类型";
                RadChartPathQuit.DefaultView.ChartArea.NoDataString = "无数据...";

                GetDepartmentListInfo();
                GetClinicalPathList();
                PathQutiStat();
                Expander1_Collapsed(sender, e);

                //InitComboBoxPath();
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
            string startdate = dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
            string enddate = dtpEndDate.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd");

            YidanEHRDataServiceClient GetClinicalPathListClient = PublicMethod.YidanClient;
            GetClinicalPathListClient.GetClinicalPathListCompleted +=
                (obj, e) =>
                {
                    radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        //m_cmbCheckBox.Clear();
                        autoPath.ItemsSource = e.Result;
                        autoPath.ItemFilter = PathFilter;
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };

            GetClinicalPathListClient.GetClinicalPathListAsync(string.Empty, string.Empty, //开始日期和结束日期
            deptInfo, string.Empty, string.Empty, string.Empty);//临床科室代码和变种代码（为空值）
            GetClinicalPathListClient.CloseAsync();
        }





        //科室下拉框
        private void cmbOffice_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                GetClinicalPathList();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        //统计
        private void btnStat_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                PathQutiStat();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 获取路径退出原因列表和图表数据
        /// </summary>
        private void PathQutiStat()
        {
            if ((dtpStartDate.SelectedDate > dtpEndDate.SelectedDate) ||
                dtpStartDate.SelectedDate.ToString().Trim() == "" ||
                dtpEndDate.SelectedDate.ToString().Trim() == "")
            {
                PublicMethod.RadAlterBox("开始日期必须小于或等于结束日期，且不能为空！", "提示");
            }

            else
            {
                radBusyIndicator.IsBusy = true;
                string deptInfo = (cmbOffice.SelectedItem != null) ? (cmbOffice.SelectedItem as CP_DepartmentList).Ksdm : string.Empty;
                string path = autoPath.SelectedItem != null ? (autoPath.SelectedItem as CP_ClinicalPathList).Ljdm : string.Empty;//(cmbPath.SelectedItem != null) ? (cmbPath.SelectedItem as CP_ClinicalPathList).Ljdm : string.Empty;
                string startdate = dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                string enddate = dtpEndDate.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd");

                YidanEHRDataServiceClient GetRptPathQuitListClient = PublicMethod.YidanClient;
                GetRptPathQuitListClient.GetRptPathQuitListCompleted +=
                    (obj, e) =>
                    {
                        radBusyIndicator.IsBusy = false;
                        if (e.Error == null)
                        {
                            List<Rpt_DataList> lstData = e.Result.ToList<Rpt_DataList>();
                            lstData = (e.Result.ToList<Rpt_DataList>());
                            foreach (Rpt_DataList dlst in lstData)
                            {
                                GridViewPathQuitList.ItemsSource = (dlst as Rpt_DataList).PathQuitList;
                                ShowChartPathQuit((dlst as Rpt_DataList).PathQuitPie.ToList());
                            }

                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
                GetRptPathQuitListClient.GetRptPathQuitListAsync(deptInfo, path, startdate, enddate);
                GetRptPathQuitListClient.CloseAsync();
            }
        }




        /// <summary>
        /// 显示退出原因比例图表
        /// </summary>
        /// <param name="rptList"></param>
        private void ShowChartPathQuit(List<Rpt_PathQuitPie> rptList)
        {
            RadChartPathQuit.SeriesMappings.Clear();

            SeriesMapping seriesMapping = new SeriesMapping();
            seriesMapping.SeriesDefinition = new PieSeriesDefinition();
            seriesMapping.SeriesDefinition.ItemLabelFormat = "#%{p0}";
            seriesMapping.SeriesDefinition.ShowItemToolTips = true;

            //选中特效
            seriesMapping.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

            //smart labels pie上显示字的位置
            ((RadialSeriesDefinition)seriesMapping.SeriesDefinition).LabelSettings.LabelOffset = 0.7d;

            seriesMapping.SeriesDefinition.ItemToolTipFormat = "退出原因：#Tcyy \r\n 数量：#Counts";  //"项目: #Xmmc\r\n比例：#%{p0}\r\n总计：#Zj"; //"项目: #Xmmc\r\n金额: #Y{0.00}"
            ItemMapping itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.YValue;
            itemMapping.FieldName = "Counts";
            seriesMapping.ItemMappings.Add(itemMapping);
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.LegendLabel;
            itemMapping.FieldName = "Tcyy";
            seriesMapping.ItemMappings.Add(itemMapping);
            RadChartPathQuit.SeriesMappings.Add(seriesMapping);

            RadChartPathQuit.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Visible;
            RadChartPathQuit.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = System.Windows.Visibility.Collapsed;
            RadChartPathQuit.DefaultView.ChartArea.AxisX.MinorGridLinesVisibility = System.Windows.Visibility.Visible;
            RadChartPathQuit.DefaultView.ChartTitle.Visibility = System.Windows.Visibility.Visible;
            RadChartPathQuit.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;



            RadChartPathQuit.ItemsSource = rptList;

        }

        private void Expander1_Collapsed(object sender, RoutedEventArgs e)
        {
             Expander1.Header = "显示图表";
            //Row1.Height = new GridLength(0, GridUnitType.Pixel);
            //Row2.Height = new GridLength(0, GridUnitType.Pixel);
            RadChartPathQuit.Visibility = System.Windows.Visibility.Collapsed;
            Row2.MaxHeight = 0;
        }

        private void Expander1_Expanded(object sender, RoutedEventArgs e)
        {
           Expander1.Header = "隐藏图表";
            //Row1.Height = new GridLength(30, GridUnitType.Pixel);
            //Row2.Height = new GridLength(1, GridUnitType.Star);
            RadChartPathQuit.Visibility = System.Windows.Visibility.Visible;
            Row2.MaxHeight = 200;
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
                    string deptInfo = (cmbOffice.SelectedItem != null) ? (cmbOffice.SelectedItem as CP_DepartmentList).Ksdm : string.Empty;
                    string path = autoPath.SelectedItem != null ? (autoPath.SelectedItem as CP_ClinicalPathList).Ljdm : string.Empty;//(cmbPath.SelectedItem != null) ? (cmbPath.SelectedItem as CP_ClinicalPathList).Ljdm : string.Empty;
                    string startdate = dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                    string enddate = dtpEndDate.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd");

                    string deptName = (cmbOffice.SelectedItem != null) ? (cmbOffice.SelectedItem as CP_DepartmentList).Name : string.Empty;
                    string pathName = autoPath.SelectedItem != null ? (autoPath.SelectedItem as CP_ClinicalPathList).Name : string.Empty; //(cmbPath.SelectedItem != null) ? (cmbPath.SelectedItem as CP_ClinicalPathList).Name : string.Empty;

                    RptPathQuitPrint pageprint = new RptPathQuitPrint();

                    pageprint.m_BeginTime = startdate;
                    pageprint.m_EndTime = enddate;
                    pageprint.m_Ljdm = path;
                    pageprint.m_Dept = deptInfo;
                    pageprint.m_LjdmName = pathName;
                    pageprint.m_DeptName = deptName;
                    pageprint.WindowState = WindowState.Maximized;
                    pageprint.ShowDialog();
                }

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        public bool PathFilter(string strFilter, object item)
        {
            CP_ClinicalPathList deptList = (CP_ClinicalPathList)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())));
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cmbOffice.SelectedIndex = -1;
            autoPath.Text = "";
        }


    }
}
