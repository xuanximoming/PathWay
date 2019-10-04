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
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Controls.Charting;

namespace YidanEHRApplication.Views.ReportForms
{
    /// <summary>
    /// 表示结算费用统计的类
    /// </summary>
    public partial class RptInPathPatientFeePercent : Page
    {
        #region 事件
        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /// <summary>
        /// 表示窗体加载的事件
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetClinicalPathList();//绑定临床路径下拉框
                dtpStartDate.SelectedValue = DateTime.Now.AddMonths(-1);//当前日期的前一个月
                dtpEndDate.SelectedValue = DateTime.Now;//当前日期


                rctFybj.DefaultView.ChartTitle.Content = "总费用与标准值的比较";
                rctFybj.DefaultView.ChartLegend.Header = "统计项目";
                rctFybj.DefaultView.ChartArea.NoDataString = "无数据...";

                rctTsbj.DefaultView.ChartTitle.Content = "住院天数与标准值的比较";
                rctTsbj.DefaultView.ChartLegend.Header = "统计项目";
                rctTsbj.DefaultView.ChartArea.NoDataString = "无数据...";

                rctFybl.DefaultView.ChartTitle.Content = "结算费用比例图";
                rctFybl.DefaultView.ChartLegend.Header = "统计项目";
                rctFybl.DefaultView.ChartArea.NoDataString = "无数据...";

                btnStat_Click(sender, e);
                ExpTb1_Collapsed(sender, e);
                ExpTb2_Collapsed(sender, e);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 表示查询按钮的事件
        /// </summary>
        private void btnStat_Click(object sender, RoutedEventArgs e)
        {
            YidanEHRDataServiceClient pathClient = PublicMethod.YidanClient;

            if ((dtpStartDate.SelectedDate > dtpEndDate.SelectedDate)
                       || dtpStartDate.SelectedDate.ToString().Trim() == ""
                       || dtpEndDate.SelectedDate.ToString().Trim() == "")
            {
                PublicMethod.RadAlterBox("开始日期必须小于或等于结束日期，且不能为空！", "提示");
                return;
            }
            radBusyIndicator.IsBusy = true;
            String begindate = dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");//开始时间
            String enddate = ((DateTime)dtpEndDate.SelectedDate).AddDays(1).ToString("yyyy-MM-dd");//结束时间
            String ljdm = (autoPath.SelectedItem == null) ? String.Empty : (autoPath.SelectedItem as CP_ClinicalPathList).Ljdm;//路径代码
            String Ljzt = String.Empty;//路径状态
            switch (cmbLjzt.SelectionBoxItem.ToString())
            {
                case "在径":
                    Ljzt = "1";
                    break;
                case "完成":
                    Ljzt = "3";
                    break;
                case "退出":
                    Ljzt = "2";
                    break;
                default:
                    Ljzt = String.Empty;
                    break;
            }
            pathClient.GetRpt_InPathPatientFeePercentCompleted += (obj, ea) =>
            {
                radBusyIndicator.IsBusy = false;
                if (ea.Error == null)
                {
                    gvInPathPatientFee.ItemsSource = ea.Result.ToList();
                }
            };
            pathClient.GetRpt_InPathPatientFeePercentAsync(begindate, enddate, Ljzt, ljdm);
        }

        /// <summary>
        /// 表示隐藏图表1的事件
        /// </summary>
        private void ExpTb1_Collapsed(object sender, RoutedEventArgs e)
        {
            ExpTb1.Header = "显示图表1";
            HideRow0.MaxHeight = 0;
        }

        /// <summary>
        /// 表示显示图表1的事件
        /// </summary>
        private void ExpTb1_Expanded(object sender, RoutedEventArgs e)
        {
            ExpTb1.Header = "隐藏图表1";
            HideRow0.MaxHeight = 200;
        }

        /// <summary>
        /// 表示隐藏图表2的事件
        /// </summary>
        private void ExpTb2_Collapsed(object sender, RoutedEventArgs e)
        {
            ExpTb2.Header = "显示图表2";
            HideRow1.MaxHeight = 0;
        }

        /// <summary>
        /// 表示显示图表2的事件
        /// </summary>
        private void ExpTb2_Expanded(object sender, RoutedEventArgs e)
        {
            ExpTb2.Header = "隐藏图表2";
            HideRow1.MaxHeight = 200;
        }

        /// <summary>
        /// 表示gvInPathPatientFee选择改变的事件
        /// </summary>
        private void gvInPathPatientFee_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            List<Tongji> fyblList = new List<Tongji>();//费用比例集合
            List<Tongji> fybjList = new List<Tongji>();//总计费用与标准值比较
            List<Tongji> tsbjList = new List<Tongji>();//住院天数与标准天数比较
            Rpt_InPathPatientFeePercent ippf = (Rpt_InPathPatientFeePercent)gvInPathPatientFee.SelectedItem;
            if (ippf == null)
            {
                return;
            }
            Tongji qita = new Tongji();
            qita.Tjlb = "其它费用";
            qita.Tjsl = ippf.Qita;
            fyblList.Add(qita);
            Tongji xyf = new Tongji();
            xyf.Tjlb = "西药费";
            xyf.Tjsl = ippf.XyFei;
            fyblList.Add(xyf);
            Tongji zhiliaofei = new Tongji();
            zhiliaofei.Tjlb = "治疗费";
            zhiliaofei.Tjsl = ippf.ZhiliaoFei;
            fyblList.Add(zhiliaofei);
            Tongji jcf = new Tongji();
            jcf.Tjlb = "检查费";
            jcf.Tjsl = ippf.JcFei;
            fyblList.Add(jcf);
            Tongji zlf = new Tongji();
            zlf.Tjlb = "诊疗费";
            zlf.Tjsl = ippf.ZlFei;
            fyblList.Add(zlf);
            Tongji cwf = new Tongji();
            cwf.Tjlb = "床位费";
            cwf.Tjsl = ippf.CwFei;
            fyblList.Add(cwf);
            Tongji hshlf = new Tongji();
            hshlf.Tjlb = "护士护理费";
            hshlf.Tjsl = ippf.HshlFei;
            fyblList.Add(hshlf);

            ShowRctFybl(fyblList);//显示费用比例图表

            Tongji zfy = new Tongji();
            zfy.Tjlb = "总费用";
            zfy.Tjsl = ippf.Zj;
            fybjList.Add(zfy);

            Tongji bzfy = new Tongji();
            bzfy.Tjlb = "标准费用";
            bzfy.Tjsl = ippf.Bzfy;
            fybjList.Add(bzfy);

            ShowRctBzfybj(fybjList);//显示标准费用比较图

            Tongji zyts = new Tongji();
            zyts.Tjlb = "住院天数";
            zyts.Tjsl = ippf.Zyts;
            tsbjList.Add(zyts);

            Tongji bzts = new Tongji();
            bzts.Tjlb = "标准天数";
            bzts.Tjsl = ippf.Bzts;
            tsbjList.Add(bzts);
            ShowRctBztsbj(tsbjList);//显示标准天数比较图
        }


        /// <summary>
        /// 表示打印按钮的点击事件
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

                String startdate = dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                String enddate = dtpEndDate.SelectedDate.Value.AddDays(1).ToString("yyyy-MM-dd");
                String Ljzt = String.Empty;
                switch (cmbLjzt.SelectionBoxItem.ToString())
                {
                    case "全部":
                        Ljzt = String.Empty;
                        break;
                    case "在径":
                        Ljzt = "1";
                        break;
                    case "完成":
                        Ljzt = "3";
                        break;
                    case "退出":
                        Ljzt = "2";
                        break;
                    default:
                        break;
                }
                String Ljdm = (autoPath.SelectedItem != null) ? (autoPath.SelectedItem as CP_ClinicalPathList).Ljdm : string.Empty;
                RptInPathPatientFeePercentPrint pageprint = new RptInPathPatientFeePercentPrint();

                pageprint.m_BeginTime = startdate;
                pageprint.m_EndTime = enddate;
                pageprint.m_Ljzt = Ljzt;
                pageprint.m_Ljdm = Ljdm;
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
        /// 构造函数RptInPathPatientFeePercent
        /// </summary>
        public RptInPathPatientFeePercent()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 表示绑定临床路径下拉框的方法
        /// </summary>
        private void GetClinicalPathList()
        {
            //radBusyIndicator.IsBusy = true;
            YidanEHRDataServiceClient GetClinicalPathListClient = PublicMethod.YidanClient;
            GetClinicalPathListClient.GetClinicalPathListCompleted +=
           (obj, e) =>
           {
               //radBusyIndicator.IsBusy = false;
               if (e.Error == null)
               {
                   autoPath.ItemsSource = e.Result.ToList();
                   autoPath.ItemFilter = PathFilter;
               }
               else
               {
                   PublicMethod.RadWaringBox(e.Error);
               }
           };
            GetClinicalPathListClient.GetClinicalPathListAsync(String.Empty, String.Empty, String.Empty, String.Empty, string.Empty, string.Empty);
            GetClinicalPathListClient.CloseAsync();
        }

        /// <summary>
        /// 表示显示费用比例图的方法
        /// </summary>
        /// <param name="fyblList">费用比例集合</param>
        private void ShowRctFybl(List<Tongji> fyblList)
        {
            rctFybl.SeriesMappings.Clear();

            SeriesMapping seriesMapping = new SeriesMapping();
            seriesMapping.SeriesDefinition = new PieSeriesDefinition();
            seriesMapping.SeriesDefinition.ItemLabelFormat = "#Y{￥0.00}";
            seriesMapping.SeriesDefinition.ShowItemToolTips = true;

            //选中特效
            seriesMapping.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

            //smart labels pie上显示字的位置
            ((RadialSeriesDefinition)seriesMapping.SeriesDefinition).LabelSettings.LabelOffset = 0.7d;

            seriesMapping.SeriesDefinition.ItemToolTipFormat = "统计项目：#Tjlb \r\n 数量：#Tjsl";
            ItemMapping itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.YValue;
            itemMapping.FieldName = "Tjsl";
            seriesMapping.ItemMappings.Add(itemMapping);
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.LegendLabel;
            itemMapping.FieldName = "Tjlb";
            seriesMapping.ItemMappings.Add(itemMapping);
            rctFybl.SeriesMappings.Add(seriesMapping);

            rctFybl.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Visible;
            rctFybl.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = System.Windows.Visibility.Collapsed;
            rctFybl.DefaultView.ChartArea.AxisX.MinorGridLinesVisibility = System.Windows.Visibility.Visible;
            rctFybl.DefaultView.ChartTitle.Visibility = System.Windows.Visibility.Visible;
            rctFybl.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;

            rctFybl.ItemsSource = fyblList;
        }

        /// <summary>
        /// 表示显示总费用与标准费用比较图的方法
        /// </summary>
        /// <param name="bzfybjList">费用比较集合</param>
        private void ShowRctBzfybj(List<Tongji> bzfybjList)
        {
            rctFybj.SeriesMappings.Clear();

            SeriesMapping seriesMapping = new SeriesMapping();
            seriesMapping.LegendLabel = "费用";
            seriesMapping.SeriesDefinition = new BarSeriesDefinition();
            seriesMapping.SeriesDefinition.ItemLabelFormat = "#Y{￥0.00}";
            seriesMapping.SeriesDefinition.ShowItemToolTips = true;

            //选中特效
            seriesMapping.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

            //smart labels bar上显示字的位置
            //((RadialSeriesDefinition)seriesMapping.SeriesDefinition).LabelSettings.LabelOffset = 0.7d;

            seriesMapping.SeriesDefinition.ItemToolTipFormat = "数量：#Tjsl";
            ItemMapping itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.YValue;
            itemMapping.FieldName = "Tjsl";
            seriesMapping.ItemMappings.Add(itemMapping);
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.XCategory;
            itemMapping.FieldName = "Tjlb";
            seriesMapping.ItemMappings.Add(itemMapping);
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.LegendLabel;
            itemMapping.FieldName = "Tjlb";
            seriesMapping.ItemMappings.Add(itemMapping);
            rctFybj.SeriesMappings.Add(seriesMapping);

            rctFybj.DefaultView.ChartArea.AxisY.Title = "值";
            rctFybj.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Visible;
            rctFybj.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = System.Windows.Visibility.Collapsed;
            rctFybj.DefaultView.ChartArea.AxisX.MinorGridLinesVisibility = System.Windows.Visibility.Visible;
            rctFybj.DefaultView.ChartTitle.Visibility = System.Windows.Visibility.Visible;
            rctFybj.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;

            rctFybj.ItemsSource = bzfybjList;
        }

        /// <summary>
        /// 表示显示住院天数与标准天数比较图的方法
        /// </summary>
        /// <param name="bztsList">天数比较集合</param>
        private void ShowRctBztsbj(List<Tongji> bztsList)
        {
            rctTsbj.SeriesMappings.Clear();

            SeriesMapping seriesMapping = new SeriesMapping();
            seriesMapping.LegendLabel = "天数";
            seriesMapping.SeriesDefinition = new BarSeriesDefinition();
            seriesMapping.SeriesDefinition.ItemLabelFormat = "#Y";
            seriesMapping.SeriesDefinition.ShowItemToolTips = true;

            //选中特效
            seriesMapping.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

            //smart labels bar上显示字的位置
            //((RadialSeriesDefinition)seriesMapping.SeriesDefinition).LabelSettings.LabelOffset = 0.7d;

            seriesMapping.SeriesDefinition.ItemToolTipFormat = "数量：#Tjsl";
            ItemMapping itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.YValue;
            itemMapping.FieldName = "Tjsl";
            seriesMapping.ItemMappings.Add(itemMapping);
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.XCategory;
            itemMapping.FieldName = "Tjlb";
            seriesMapping.ItemMappings.Add(itemMapping);
            itemMapping = new ItemMapping();
            itemMapping.DataPointMember = DataPointMember.LegendLabel;
            itemMapping.FieldName = "Tjlb";
            seriesMapping.ItemMappings.Add(itemMapping);
            rctTsbj.SeriesMappings.Add(seriesMapping);

            rctTsbj.DefaultView.ChartArea.AxisY.Title = "值";
            rctTsbj.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Visible;
            rctTsbj.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = System.Windows.Visibility.Collapsed;
            rctTsbj.DefaultView.ChartArea.AxisX.MinorGridLinesVisibility = System.Windows.Visibility.Visible;
            rctTsbj.DefaultView.ChartTitle.Visibility = System.Windows.Visibility.Visible;
            rctTsbj.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;

            rctTsbj.ItemsSource = bztsList;
        }
        public bool PathFilter(string strFilter, object item)
        {
            CP_ClinicalPathList deptList = (CP_ClinicalPathList)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())));
        }

        #endregion

        #region 实体

        /// <summary>
        /// 统计表
        /// </summary>
        public class Tongji
        {
            /// <summary>
            /// 统计类别
            /// </summary>
            public String Tjlb { get; set; }
            /// <summary>
            /// 统计数量
            /// </summary>
            public String Tjsl { get; set; }
        }

        #endregion

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cmbLjzt.SelectedIndex = -1;
            autoPath.Text = "";
        }
    }
}
