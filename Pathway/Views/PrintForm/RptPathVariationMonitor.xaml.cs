using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
namespace YidanEHRApplication.Views.ReportForms
{
    public partial class RptPathVariationMonitor : Page
    {
        public RptPathVariationMonitor()
        {
            InitializeComponent();
        }
        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        #region 事件

        /// <summary>
        /// 页面加载
        /// (4.25注释，开始修改)
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.dtpStartDate.SelectedDate = DateTime.Now.AddMonths(-1);
                this.dtpEndDate.SelectedDate = DateTime.Now;

                GetClinicalPathList();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        /// <summary>
        /// 统计按钮
        /// (4.26添加注释)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMonitor_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                GridViewMonitor.ItemsSource = null;                //清空原有数据
                GetRptPathVariationMonitorList();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
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
                    queryCondition.PathName = (autoPath.SelectedItem != null) ? (autoPath.SelectedItem as CP_ClinicalPathList).Name : string.Empty;

                    RptPathVariationMonitorPrint print = new RptPathVariationMonitorPrint();
                    print.printCondition.Stardate = queryCondition.Stardate;
                    print.printCondition.Enddate = queryCondition.Enddate;
                    print.printCondition.Path = queryCondition.Path;
                    print.printCondition.PathName = queryCondition.PathName;
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
        /// 获取临床路径列表
        /// (4.25修改)
        /// </summary>
        private void GetClinicalPathList()
        {
            radBusyIndicator.IsBusy = true;

            YidanEHRDataServiceClient GetClinicalPathListClient = PublicMethod.YidanClient;
            GetClinicalPathListClient.GetClinicalPathListCompleted +=
           (obj, e) =>
           {
               radBusyIndicator.IsBusy = false;
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

            GetClinicalPathListClient.GetClinicalPathListAsync(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            GetClinicalPathListClient.CloseAsync();
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
            else if (autoPath.SelectedItem == null)
            {
                PublicMethod.RadAlterBox("需要先选择一个路径！", "提示");
                return false;
            }
            else
            {
                queryConditioin.Path = (autoPath.SelectedItem != null) ? (autoPath.SelectedItem as CP_ClinicalPathList).Ljdm : string.Empty;
                queryConditioin.Stardate = dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd");
                queryConditioin.Enddate = ((DateTime)dtpEndDate.SelectedDate).AddDays(1).ToString("yyyy-MM-dd");
                pathID = queryConditioin.Path;
                return true;
            }
        }

        private string pathID;
        /// <summary>
        /// 获取月度出径数据列表
        /// (4.26注释，开始编写)
        /// </summary>
        private void GetRptPathVariationMonitorList()
        {
            try
            {
                Rpt_QueryCondition queryConditioin = new Rpt_QueryCondition();

                if (LoadBtnClick(queryConditioin))
                {
                    radBusyIndicator.IsBusy = true;

                    YidanEHRDataServiceClient GetRptPathVariationMonitorList = PublicMethod.YidanClient;

                    GetRptPathVariationMonitorList.GetRptPathVariationMonitorCompleted +=
                    (obj, e) =>
                    {
                        radBusyIndicator.IsBusy = false;
                        if (e.Error == null)
                        {
                            Rpt_PathVariationMonitor rpt = e.Result;

                            if (rpt.PathVariationMonitorList == null)                    //如果经过筛选后数据为空，则需跳出
                            {
                                return;
                            }
                            else
                            {
                                List<Rpt_PathVariationMonitorList> rptList = rpt.PathVariationMonitorList.ToList();          //如果为空不跳出，则会在此处转化NULL报错
                                GridViewMonitor.ItemsSource = rptList;
                            }
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
                    GetRptPathVariationMonitorList.GetRptPathVariationMonitorAsync(queryConditioin.Stardate, queryConditioin.Enddate, queryConditioin.Path);
                    GetRptPathVariationMonitorList.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        #endregion

        private void GridViewMonitor_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
            Rpt_PathVariationMonitorList row = (Rpt_PathVariationMonitorList)e.DataElement;
            GridViewRow item = GridViewMonitor.ItemContainerGenerator.ContainerFromItem(e.Row.Item) as GridViewRow;

            if (item != null)
            {
                coloringExecute(item, row.Per);
            }

        }
        /// <summary>
        /// 变色
        /// </summary>
        private void coloringExecute(GridViewRow container, int key)
        {
            GridViewCellBase per = (from c in container.Cells
                                    where c.Column.UniqueName == "Per"
                                    select c).FirstOrDefault();

            if (per != null)
            {
                String color = "";
                if (key >= 0 && key <= 25)
                {
                    //    color = "66FF66";
                    color = "barOne";
                }
                else if (key >= 26 && key <= 50)
                {
                    //    color = "3399FF";
                    color = "barTwo";
                }
                else if (key >= 51 && key <= 75)
                {
                    //    color = "FFFF33";
                    color = "barThree";
                }
                else
                {
                    //    color = "FF0033";
                    color = "barFour";
                }

                RadProgressBar bar = per.Content as RadProgressBar;
                //bar.Background =
                //bar.Foreground = ConvertColor.GetColorBrushFromHx16(color);
                bar.Style = Resources["bar"] as Style;
            }


        }
        public bool PathFilter(string strFilter, object item)
        {
            CP_ClinicalPathList deptList = (CP_ClinicalPathList)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())));
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            autoPath.Text = "";
        }

        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Rpt_PathVariationMonitorList pv = (Rpt_PathVariationMonitorList)this.GridViewMonitor.SelectedItem;
                if (pv == null)
                {
                    PublicMethod.RadAlterBox("请选择一个节点！", "提示");
                    return;
                }
                YidanEHRApplication.Views.PrintForm.RptPathVariationMonitorDetail pathDetail = new PrintForm.RptPathVariationMonitorDetail(pathID, pv.Ljmc, dtpStartDate.SelectedDate.Value.ToString("yyyy-MM-dd"), dtpEndDate.SelectedDate.Value.ToString("yyyy-MM-dd"), this.autoPath.Text);
                pathDetail.ShowDialog();
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}