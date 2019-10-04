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

namespace YidanEHRApplication.Views.ReportForms
{
    public partial class RptPathVariation : Page
    {
        YidanEHRDataServiceClient serviceCon;
        public RptPathVariation()
        {
            InitializeComponent();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.radbegintime.SelectedDate = DateTime.Now.AddMonths(-1);
                this.radendtime.SelectedDate = DateTime.Now;

                RadChart1.DefaultView.ChartTitle.Content = "临床路径已经完成变异情况统计";
                RadChart1.DefaultView.ChartLegend.Header = "变异类别";

                BindCombox();

                btnQuery_Click(null, null);
                btnHide_Click(sender, e);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.radbegintime.SelectedDate == null || this.radendtime.SelectedDate == null)
                {
                    PublicMethod.RadAlterBox("时间不能为空！", "提示信息！");
                    return;
                }
                DateTime begintime = (DateTime)this.radbegintime.SelectedDate;
                DateTime endtime = (DateTime)this.radendtime.SelectedDate;
                if (begintime > endtime)
                {
                    PublicMethod.RadAlterBox("开始时间不能大于结束时间！", "提示信息！");
                }

                string Ljdm = this.autoPath.SelectedItem == null ? "" : (this.autoPath.SelectedItem as CP_ClinicalPathList).Ljdm;
                string Dept = this.cbxDeptName.SelectedItem == null ? "" : (this.cbxDeptName.SelectedItem as CP_DepartmentList).Ksdm;
                BindGrid(begintime.ToString("yyyy-MM-dd") + "00:00:00", endtime.ToString("yyyy-MM-dd") + "23:59:59", Ljdm, Dept);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        public void BindCombox()
        {
            GetDepartmentListInfo();
            GetClinicalPathList();
        }

        /// <summary>
        /// 获取临床科室列表
        /// </summary>
        private void GetDepartmentListInfo()
        {
            YidanEHRDataServiceClient GetDepartmentListInfoClient = PublicMethod.YidanClient;
            GetDepartmentListInfoClient.GetDepartmentListInfoCompleted +=

            (obj, e) =>
            {
                if (e.Error == null)
                {
                    this.cbxDeptName.ItemsSource = e.Result;
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
            YidanEHRDataServiceClient GetClinicalPathListClient = PublicMethod.YidanClient;
            GetClinicalPathListClient.GetClinicalPathListCompleted +=
              (obj, e) =>
              {
                  if (e.Error == null)
                  {
                      autoPath.ItemsSource = e.Result;
                      autoPath.ItemFilter = PathFilter;
                  }
                  else
                  {
                      PublicMethod.RadWaringBox(e.Error);
                  }
              };
            GetClinicalPathListClient.GetClinicalPathListAsync(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            GetClinicalPathListClient.CloseAsync();
        }



        public void BindGrid(string begintime, string endtime, string Ljdm, string Dept)
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetRptPathVariationCompleted +=
                (obj, e) =>
                {
                    GridViewPathVariation.ItemsSource = e.Result.ToList();

                    //绑定饼图

                    List<RPT_PathVariation> eptList = e.Result.ToList();
                    if (eptList.Count == 0)
                    {
                        RadChart1.DefaultView.ChartArea.NoDataString = "无数据...";
                        return;
                    }

                    SeriesMapping seriesMapping = new SeriesMapping();
                    seriesMapping.SeriesDefinition = new PieSeriesDefinition();
                    seriesMapping.SeriesDefinition.ItemLabelFormat = "#%{p0}"; //"金额：#Y{C0.00}\r\n比例：#%{p0}"
                    seriesMapping.SeriesDefinition.ShowItemToolTips = true;
                    //seriesMapping.ItemMappings.Clear();
                    //RadChart1.DefaultView.ChartArea.Items.Clear();
                    //RadChart1.DefaultView.ChartLegend.Items.Clear();



                    //选中特效
                    seriesMapping.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
                    seriesMapping.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
                    seriesMapping.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

                    //smart labels pie上显示字的位置
                    ((RadialSeriesDefinition)seriesMapping.SeriesDefinition).LabelSettings.LabelOffset = 0.7d;


                    seriesMapping.SeriesDefinition.ItemToolTipFormat = "变异名称: #VariationName \r\n变异数量：#VariationCount";  //"项目: #Xmmc\r\n比例：#%{p0}\r\n总计：#Zj"; //"项目: #Xmmc\r\n金额: #Y{0.00}"
                    ItemMapping itemMapping = new ItemMapping();
                    itemMapping.DataPointMember = DataPointMember.YValue;
                    itemMapping.FieldName = "VariationCount";
                    seriesMapping.ItemMappings.Add(itemMapping);
                    itemMapping = new ItemMapping();
                    itemMapping.DataPointMember = DataPointMember.LegendLabel;
                    itemMapping.FieldName = "VariationName";
                    seriesMapping.ItemMappings.Add(itemMapping);
                    RadChart1.SeriesMappings.Clear();
                    RadChart1.SeriesMappings.Add(seriesMapping);

                    RadChart1.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Visible;
                    RadChart1.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = System.Windows.Visibility.Visible;
                    RadChart1.DefaultView.ChartArea.AxisX.MinorGridLinesVisibility = System.Windows.Visibility.Collapsed;

                    RadChart1.DefaultView.ChartTitle.Height = 40;
                    RadChart1.DefaultView.ChartTitle.Visibility = System.Windows.Visibility.Visible;
                    //RadChart1.DefaultView.ChartTitle.Visibility = System.Windows.Visibility.Collapsed;
                    RadChart1.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;

                    RadChart1.ItemsSource = eptList;

                };
            serviceCon.GetRptPathVariationAsync(begintime, endtime, Ljdm, Dept);
            serviceCon.CloseAsync();
        }



        private void btnHide_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnHide.Header.ToString() == "隐藏图表")
                {
                    RadChart1.Visibility = System.Windows.Visibility.Collapsed;
                    btnHide.Header = "显示图表";
                }
                else
                {
                    RadChart1.Visibility = System.Windows.Visibility.Visible;
                    btnHide.Header = "隐藏图表";
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                DateTime begintime = (DateTime)this.radbegintime.SelectedDate;
                DateTime endtime = (DateTime)this.radendtime.SelectedDate;
                if (begintime > endtime)
                {
                    PublicMethod.RadAlterBox("开始时间不能大于结束时间！", "提示信息！");
                }

                string Ljdm = this.autoPath.SelectedItem == null ? "" : (this.autoPath.SelectedItem as CP_ClinicalPathList).Ljdm;
                string Dept = this.cbxDeptName.SelectedItem == null ? "" : (this.cbxDeptName.SelectedItem as CP_DepartmentList).Ksdm;

                string LjdmName = this.autoPath.SelectedItem == null ? "" : (this.autoPath.SelectedItem as CP_ClinicalPathList).Name;
                string DeptName = this.cbxDeptName.SelectedItem == null ? "" : (this.cbxDeptName.SelectedItem as CP_DepartmentList).Name;


                RptPathVariationPrint varprint = new RptPathVariationPrint();

                varprint.m_BeginTime = begintime;
                varprint.m_EndTime = endtime;
                varprint.m_Ljdm = Ljdm;
                varprint.m_Dept = Dept;
                varprint.m_LjdmName = LjdmName;
                varprint.m_DeptName = DeptName;
                varprint.WindowState = WindowState.Maximized;
                varprint.ShowDialog();
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
            cbxDeptName.SelectedIndex = -1;
            autoPath.Text = "";
        }

        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RPT_PathVariation pv = (RPT_PathVariation)this.GridViewPathVariation.SelectedItem;
                if (pv == null)
                {
                    PublicMethod.RadAlterBox("请选择一条记录！", "提示");
                    return;
                }
                YidanEHRApplication.Views.PrintForm.RptPathVariationDetail pathDetail = new PrintForm.RptPathVariationDetail(pv.PathID, pv.VariationName, radbegintime.SelectedDate.Value.ToString("yyyy-MM-dd"), radendtime.SelectedDate.Value.ToString("yyyy-MM-dd"));
                pathDetail.ShowDialog();
            }
            catch (Exception)
            {
                throw;
            }

        }
    }
}
