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
using Telerik.Windows.Controls;
using System.Windows.Data;
using System.Reflection;
using Telerik.Windows.Controls.GridView;


namespace YidanEHRApplication.Views.ReportForms
{
    public partial class RptPathFinish : Page
    {
        /// <summary>
        /// 显示的查询周期数，线状图中X列数
        /// </summary>
        static int m_ColCount = 0;

        /// <summary>
        /// 记录下线状图中已经加载的路径完成信息
        /// </summary>
        static List<Rpt_PathFinishImage> m_imagelist = new List<Rpt_PathFinishImage>();

        /// <summary>
        /// 记录查询数据时传回的现状图中的数据源
        /// </summary>
        static List<Rpt_PathFinishImage> m_pathfinishimageList = new List<Rpt_PathFinishImage>();

        YidanEHRDataServiceClient serviceCon;
        public RptPathFinish()
        {
            InitializeComponent();


        }


        /// <summary>
        /// 页面加载事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                this.radbegintime.SelectedDate = DateTime.Now.AddMonths(-1);
                this.radendtime.SelectedDate = DateTime.Now;

                radChart.DefaultView.ChartTitle.Content = "临床路径已经完成统计";
                radChart.DefaultView.ChartLegend.Header = "路径名称";
                radChart.DefaultView.ChartArea.NoDataString = "无数据...";
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

        /// <summary>
        /// 查询按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                    return;
                }
                int Month = 0;

                if ((endtime.Year - begintime.Year) == 0)
                {
                    Month = endtime.Month - begintime.Month;
                }
                if ((endtime.Year - begintime.Year) >= 1)
                {
                    if (endtime.Month - begintime.Month < 0)
                    {
                        Month = (endtime.Year - begintime.Year - 1) * 12 + (12 - begintime.Month) + endtime.Month + 1;
                    }
                    else
                    {
                        Month = (endtime.Year - begintime.Year) * 12 + endtime.Month - begintime.Month + 1;
                    }
                }

                //if (Month > 12)
                //{
                //    PublicMethod.RadAlterBox("时间区间不能超过1年！", "提示信息！");
                //    return;
                //}

                string Ljdm = this.autoPath.SelectedItem == null ? "" : (this.autoPath.SelectedItem as CP_ClinicalPathList).Ljdm;
                string Dept = this.cbxDeptName.SelectedItem == null ? "" : (this.cbxDeptName.SelectedItem as CP_DepartmentList).Ksdm;
                string period = this.cbxPeriod.SelectedIndex.ToString();
                BindGrid(begintime.ToString("yyyy-MM-dd"), endtime.ToString("yyyy-MM-dd"), Ljdm, Dept, period);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        /// <summary>
        /// 绑定页面中下拉框中值
        /// </summary>
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




        public void BindGrid(string begintime, string endtime, string Ljdm, string Dept, string period)
        {
            radBusyIndicator.IsBusy = true;
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetRptPathFinishCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {

                        if (e.Result != null)
                        {
                            RPT_PathFinish pathfinish = e.Result;
                            if (e.Result.Message.Length > 0)
                            {
                                PublicMethod.RadAlterBox(e.Result.Message + "请重新输入选择条件！", "提示");
                                BindGridCol(null);
                                radBusyIndicator.IsBusy = false;
                                return;
                            }
                            BindGridCol(pathfinish);

                        }

                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                    radBusyIndicator.IsBusy = false;
                };
            serviceCon.GetRptPathFinishAsync(begintime, endtime, Ljdm, Dept, period);
            serviceCon.CloseAsync();
        }

        /// <summary>
        /// 根据数据源动态绑定数据
        /// </summary>
        /// <param name="List"></param>
        public void BindGridCol(RPT_PathFinish pathfinish)
        {
            if (pathfinish == null)
            {
                return;
            }
            List<RPT_PathFinishList> list_pathfinistlist = pathfinish.PathFinishList.ToList();
            m_pathfinishimageList = pathfinish.PathFinishImage.ToList();

            GridViewPathFinish.Columns.Clear();

            Dictionary<int, string> dict = new Dictionary<int, string>();

            RPT_PathFinishList rptListTitle = new RPT_PathFinishList();
            rptListTitle = (RPT_PathFinishList)list_pathfinistlist[0];
            GridViewDataColumn col = new GridViewDataColumn();

            if (rptListTitle.PathName.Length > 0)
            {
                col.Header = rptListTitle.PathName;
                col.TextAlignment = TextAlignment.Left;
                col.MinWidth = 150;
                col.Width = new GridViewLength(150, GridViewLengthUnitType.Star);
                col.DataMemberBinding = new Binding("PathName");
                col.Width = Telerik.Windows.Controls.GridViewLength.SizeToCells;
                col.HeaderTextAlignment = TextAlignment.Left;
                GridViewPathFinish.Columns.Add(col);
            }

            Type type = rptListTitle.GetType();
            PropertyInfo[] pf = type.GetProperties();
            string property = "";
            //初始化线状图周期数
            m_ColCount = 0;
            //初始化现状图中绑定的数据源
            m_imagelist.Clear();
            for (int i = 0; i < pf.Count(); i++)
            {
                if (pf[i].Name.Contains("Col"))
                {
                    property = pf[i].GetValue(rptListTitle, new object[] { }).ToString();

                    if (property.Length > 0)
                    {
                        col = new GridViewDataColumn();
                        col.Header = property;
                        col.TextAlignment = TextAlignment.Left;
                        col.MinWidth = 100;
                        col.Width = new GridViewLength(150, GridViewLengthUnitType.Star);
                        col.DataMemberBinding = new Binding(pf[i].Name);
                        col.Width = Telerik.Windows.Controls.GridViewLength.SizeToCells;
                        GridViewPathFinish.Columns.Add(col);
                        //记录下一共有多少列有数据
                        m_ColCount += 1;
                    }
                }
            }

            //方便绑定数据，将List中表头信息删除
            list_pathfinistlist.RemoveAt(0);

            //将列表中第一条信息对应的现状图数据存入m_imagelist中
            for (int i = 0; i < m_pathfinishimageList.Count; i++)
            {
                if (m_pathfinishimageList[i].PathID == list_pathfinistlist[0].PathID)
                {
                    m_imagelist.Add(m_pathfinishimageList[i]);
                }
            }


            this.GridViewPathFinish.AutoGenerateColumns = false;

            BindLinChart();

            GridViewPathFinish.ItemsSource = list_pathfinistlist;

            radBusyIndicator.IsBusy = false;
        }





        private void btnHide_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (btnHide == null)
                    return;
                if (btnHide.Header.ToString() == "隐藏图表")
                {
                    ChartControl.Visibility = System.Windows.Visibility.Collapsed;
                    LabTitle.Visibility = System.Windows.Visibility.Collapsed;
                    btnHide.Header = "显示图表";
                }
                else
                {
                    ChartControl.Visibility = System.Windows.Visibility.Visible;
                    LabTitle.Visibility = System.Windows.Visibility.Visible;
                    btnHide.Header = "隐藏图表";
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 双击grid中对应行时将当前行信息加入到下面线状图中显示（根据pathid判断是否已经存在，存在则不加入）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewPathFinish_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e)
        {
            GridViewRow row = (GridViewRow)e.Row;

            RPT_PathFinishList list = (RPT_PathFinishList)row.DataContext;

            bool IsExists = false;

            //循环判断线状态中是否已经存在该路径信息
            for (int i = 0; i < m_imagelist.Count; i++)
            {
                if (m_imagelist[i].PathID == list.PathID)
                {
                    IsExists = true;
                    return;
                }
            }
            if (!IsExists)
            {
                for (int i = 0; i < m_pathfinishimageList.Count; i++)
                {
                    if (m_pathfinishimageList[i].PathID == list.PathID)
                    {
                        m_imagelist.Add(m_pathfinishimageList[i]);
                    }
                }
                BindLinChart();
            }

        }

        /// <summary>
        /// 绑定现状图
        /// </summary>
        public void BindLinChart()
        {

            radChart.SeriesMappings.Clear();

            //划线图表
            List<List<Rpt_PathFinishImage>> source = new List<List<Rpt_PathFinishImage>>();
            //根据数据源循环化出线状图
            for (int i = 0; i < m_imagelist.Count / m_ColCount; i++)
            {
                SeriesMapping seriesMappingLine = new SeriesMapping();
                ItemMapping itemMapping = new ItemMapping();
                seriesMappingLine.LegendLabel = m_imagelist[i * m_ColCount].PathName;
                seriesMappingLine.SeriesDefinition = new SplineSeriesDefinition();
                seriesMappingLine.SeriesDefinition.ItemLabelFormat = "#Rate";


                //选中特效
                SeriesDefinition definition = new LineSeriesDefinition();
                definition.InteractivitySettings.HoverScope = InteractivityScope.Series;
                definition.InteractivitySettings.SelectionScope = InteractivityScope.Series;
                definition.ShowItemToolTips = true;
                definition.ShowItemLabels = false;

                seriesMappingLine.SeriesDefinition = definition;

                seriesMappingLine.SeriesDefinition.ItemToolTipFormat = "#PathName:\r\n #PeriodName \r\n 完成：#Rate%";
                itemMapping = new ItemMapping();
                itemMapping.DataPointMember = DataPointMember.YValue;
                itemMapping.FieldName = "Rate";
                seriesMappingLine.ItemMappings.Add(itemMapping);
                itemMapping = new ItemMapping();
                itemMapping.DataPointMember = DataPointMember.XCategory;
                itemMapping.FieldName = "PeriodName";
                seriesMappingLine.ItemMappings.Add(itemMapping);
                seriesMappingLine.CollectionIndex = i;
                radChart.SeriesMappings.Add(seriesMappingLine);

                //每条线构造一个数据源放入到List中  绑定线时候制定对应数据源
                List<Rpt_PathFinishImage> img_list = new List<Rpt_PathFinishImage>();
                for (int j = (i) * m_ColCount; j < (i + 1) * m_ColCount; j++)
                {
                    //if (j < (i + 1) * m_ColCount && j >= (i) * m_ColCount)
                    //{
                    img_list.Add(m_imagelist[j]);
                    //}
                }
                source.Add(img_list);
            }

            radChart.DefaultView.ChartArea.NoDataString = "无数据...";
            radChart.DefaultView.ChartArea.AxisY.Title = "完成率";
            radChart.DefaultView.ChartArea.AxisX.LayoutMode = AxisLayoutMode.Between;
            radChart.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;
            radChart.DefaultView.ChartArea.MinWidth = 600;


            radChart.ItemsSource = source;
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
                    return;
                }
                int Month = 0;

                if ((endtime.Year - begintime.Year) == 0)
                {
                    Month = endtime.Month - begintime.Month;
                }
                if ((endtime.Year - begintime.Year) >= 1)
                {
                    if (endtime.Month - begintime.Month < 0)
                    {
                        Month = (endtime.Year - begintime.Year - 1) * 12 + (12 - begintime.Month) + endtime.Month + 1;
                    }
                    else
                    {
                        Month = (endtime.Year - begintime.Year) * 12 + endtime.Month - begintime.Month + 1;
                    }
                }

                if (Month > 12)
                {
                    PublicMethod.RadAlterBox("时间区间不能超过1年！", "提示信息！");
                    return;
                }

                string Ljdm = this.autoPath.SelectedItem == null ? "" : (this.autoPath.SelectedItem as CP_ClinicalPathList).Ljdm;
                string Dept = this.cbxDeptName.SelectedItem == null ? "" : (this.cbxDeptName.SelectedItem as CP_DepartmentList).Ksdm;
                string period = this.cbxPeriod.SelectedIndex.ToString();

                RptPathFinishPrint pageprint = new RptPathFinishPrint();

                pageprint.m_BeginTime = begintime;
                pageprint.m_EndTime = endtime;
                pageprint.m_Ljdm = Ljdm;
                pageprint.m_Dept = Dept;
                pageprint.m_Period = period;
                pageprint.WindowState = WindowState.Maximized;
                pageprint.ShowDialog();

                //PagePrint pageprint = new PagePrint();
                //pageprint.ShowDialog();
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
                RPT_PathFinishList ps = (RPT_PathFinishList)this.GridViewPathFinish.SelectedItem;
                if (ps == null)
                {
                    PublicMethod.RadAlterBox("请选择一条临床路径！", "提示");
                    return;
                }
                YidanEHRApplication.Views.PrintForm.RptPathFinishDetail pathDetail = new PrintForm.RptPathFinishDetail(ps.PathID, radbegintime.SelectedDate.Value.ToString("yyyy-MM-dd"), radendtime.SelectedDate.Value.ToString("yyyy-MM-dd"), ps.PathName);
                pathDetail.ShowDialog();
            }
            catch (Exception)
            {
                throw;
            }
        }


    }
}
