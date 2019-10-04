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
using Telerik.Windows.Controls.Charting;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Views
{
    public partial class UCPatientDetailVitalSigns : UserControl
    {
        /// <summary>
        /// 开始时间,格式yyyyMMdd
        /// </summary>
        public DateTime StartDay
        { get; set; }

        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime EndDay
        { get; set; }

        /// <summary>
        /// 当前病人的住院号码，目前就‘0267660’有数据
        /// </summary>
        public string Zyhm
        { get; set; }

        private int m_index = 0;//日期索引
        private List<string> m_DaysName = new List<string>();
        List<CP_VitalSignsTwMbHx> m_list = new List<CP_VitalSignsTwMbHx>();

        public UCPatientDetailVitalSigns()
        {
            InitializeComponent();

        }

        #region 绑定RadChart数据
        private void Layout_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetRadChartData();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        private void GetRadChartData()
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.GetNursingNotesVitalSignsCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        m_list = e.Result.ToList();
                        if (m_list.Count == 0)
                        {
                            radChart1.DefaultView.ChartArea.NoDataString = "无数据...";
                            radChart1.DefaultView.ChartLegend.Header = "类型...";
                        }
                        else
                        {
                            radChart1.DefaultView.ChartLegend.Header = "体征信息线状图";
                            radChart1.DefaultView.ChartLegend.Width = 130;
                            //this.btnAllByHour.Content = "查看全部(小时)";
                            this.btnAllByHour.Visibility = System.Windows.Visibility.Visible;
                            this.btnNextDay.Visibility = System.Windows.Visibility.Collapsed;
                            this.radChart1.DefaultView.ChartArea.ItemClick -= new EventHandler<ChartItemClickEventArgs>(ChartArea_ItemClick);
                            radChart1.DefaultView.ChartArea.DataSeries.Clear();
                            radChart1.DefaultView.ChartArea.AdditionalYAxes.Clear();
                            radChart1.DefaultView.ChartArea.AdditionalYAxes.Add(new AxisY());
                            radChart1.DefaultView.ChartArea.AdditionalYAxes.Add(new AxisY());
                            radChart1.DefaultView.ChartArea.AdditionalYAxes[0].AxisName = "YAxisMb";
                            radChart1.DefaultView.ChartArea.AdditionalYAxes[1].AxisName = "YAxisHx";
                            var dates =
                            (
                            from d in m_list
                            select d.Clrq
                            ).Distinct();

                            foreach (var date in dates)
                            {
                                m_DaysName.Add(date);
                            }

                            this.FillWithAllDataByDay();
                            this.radChart1.DefaultView.ChartArea.ItemClick += new EventHandler<ChartItemClickEventArgs>(ChartArea_ItemClick);
                        }
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                };
            client.GetNursingNotesVitalSignsAsync(Zyhm, StartDay.ToString("yyyy/MM/dd"), EndDay.ToString("yyyy/MM/dd"));
            client.CloseAsync();
        }

         
		
        #endregion

        #region 方法
        private void FillRadChartData(string fieldName)
        {
            if (m_list.Count == 0)
            {
                radChart1.DefaultView.ChartArea.NoDataString = "无数据...";
            }
            else
            {
                radChart1.DefaultView.ChartLegend.Header = "体征信息线状图";
                radChart1.DefaultView.ChartLegend.Width = 130;
                radChart1.DefaultView.ChartArea.Margin = new Thickness(0);
                radChart1.DefaultView.ChartArea.NoDataString = "数据生成中...";

                radChart1.DefaultView.ChartArea.AxisY.DefaultLabelFormat = "f1";
                radChart1.DefaultView.ChartArea.AxisX.LayoutMode = AxisLayoutMode.Between;

                DataSeries series = new DataSeries();
                series.Definition = new LineSeriesDefinition();
                series.Definition.ItemLabelFormat = "f1";
                series.Definition.ShowItemToolTips = true;



                FillWithDataByDay(series, m_list, fieldName);//获取数据

                radChart1.DefaultView.ChartArea.DataSeries.Clear();
                radChart1.DefaultView.ChartArea.DataSeries.Add(series);

                if (m_DaysName != null && m_DaysName.Count > 0 && series.Count > 0 && series.Count == m_DaysName.Count)
                {
                    for (int i = 0; i < m_DaysName.Count; i++)
                    {
                        series[i].XCategory = m_DaysName[i];
                    }
                }

                radChart1.DefaultView.ChartArea.ItemToolTipOpening += new ItemToolTipEventHandler(ChartArea_ItemToolTipOpening);
            }
        }

        private void FillWithDataByDay(DataSeries series, List<CP_VitalSignsTwMbHx> list, string fieldName)
        {

            switch (fieldName)
            {
                case "Hztw":

                    series.LegendLabel = "体温记录";
                    if (list.Where(tz => !string.IsNullOrEmpty(tz.Hztw)).Count() > 0)
                    {
                        var avrg_tw =
                            from tz in list
                            group tz by tz.Clrq into temp
                            select temp.Where(a => !string.IsNullOrEmpty(a.Hztw)).Average(a => Convert.ToDouble(a.Hztw));
                       
                        foreach (var tw in avrg_tw)
                        {
                            series.Add(new DataPoint(tw));
                        }
                    }
                    break;
                case "Hzmb":

                    series.LegendLabel = "脉搏记录";
                    if (  list.Where(tz => !string.IsNullOrEmpty(tz.Hzmb)).Count() > 0)
                    {
                        var avrg_mb =
                            from tz in list
                            group tz by tz.Clrq into temp
                            select temp.Where(a => !string.IsNullOrEmpty(a.Hzmb)).Average(a => Convert.ToDouble(a.Hzmb));
                      
                        foreach (var mb in avrg_mb)
                        {
                            series.Add(new DataPoint(mb));
                        }
                    }
                    break;
                case "Hzhx":

                    series.LegendLabel = "呼吸记录";
                    if (list.Where(tz => !string.IsNullOrEmpty(tz.Hzhx)).Count() > 0)
                    {
                        var avrg_hx =
                            from tz in list
                            group tz by tz.Clrq into temp
                            select temp.Where(a => !string.IsNullOrEmpty(a.Hzhx)).Average(a => Convert.ToDouble(a.Hzhx));
                        foreach (var hx in avrg_hx)
                        {
                            series.Add(new DataPoint(hx));
                        }
                    }
                    break;

            }

        }

        public void FillWithDataByHour(DataSeries series, List<CP_VitalSignsTwMbHx> list, string fieldName, int index)
        {
            switch (fieldName)
            {
                case "Hztw":
                    var rq_tw =
                        from tz in list
                        group tz by tz.Clrq into temp
                        select new
                        {
                            Rq = temp.Key,
                            temp
                        };
                    foreach (var tw in rq_tw)
                    {
                        if (tw.Rq.Equals(m_DaysName[index]))
                        {
                            foreach (var t in tw.temp)
                            {
                                if (!string.IsNullOrEmpty(t.Hztw))
                                {
                                    series.Add(new DataPoint(t.Sjd, Convert.ToDouble(t.Hztw)));
                                }
                            }
                        }
                    }
                    break;
                case "Hzmb":
                    var rq_mb =
                         from tz in list
                         group tz by tz.Clrq into temp
                         select new
                         {
                             Rq = temp.Key,
                             temp
                         };
                    foreach (var mb in rq_mb)
                    {
                        if (mb.Rq.Equals(m_DaysName[index]))
                        {
                            foreach (var t in mb.temp)
                            {
                                if (!string.IsNullOrEmpty(t.Hzmb))
                                {
                                    series.Add(new DataPoint(t.Sjd, Convert.ToDouble(t.Hzmb)));
                                }
                            }
                        }
                    }
                    break;
                case "Hzhx":
                    var rq_hx =
                        from tz in list
                        group tz by tz.Clrq into temp
                        select new
                        {
                            Rq = temp.Key,
                            temp
                        };
                    foreach (var hx in rq_hx)
                    {
                        if (hx.Rq.Equals(m_DaysName[index]))
                        {
                            foreach (var t in hx.temp)
                            {
                                if (!string.IsNullOrEmpty(t.Hzhx))
                                {
                                    series.Add(new DataPoint(t.Sjd, Convert.ToDouble(t.Hzhx)));
                                }
                            }
                        }
                    }
                    break;
            }
        }


        private DataSeries GenerateSeriesByDay(string legendLabel, string axisName, ISeriesDefinition definition, string fieldName)
        {
            DataSeries series = new DataSeries();
            series.Definition = definition;
            series.LegendLabel = legendLabel;
            series.Definition.AxisName = axisName;
            series.Definition.ShowItemToolTips = true;
            series.Definition.ItemLabelFormat = "f1";
            FillWithDataByDay(series, m_list, fieldName);//获取数据

            if (m_DaysName != null && m_DaysName.Count > 0 && series.Count > 0 && series.Count == m_DaysName.Count)
            {
                for (int i = 0; i < m_DaysName.Count; i++)
                {
                    series[i].XCategory = m_DaysName[i];
                }
            }
            radChart1.DefaultView.ChartArea.ItemToolTipOpening += new ItemToolTipEventHandler(ChartArea_ItemToolTipOpening);
            return series;
        }

        private DataSeries GenerateSeriesByHour(string legendLabel, string axisName, ISeriesDefinition definition, string fieldName, int index)
        {
            DataSeries series = new DataSeries();
            series.Definition = definition;
            series.LegendLabel = legendLabel;
            series.Definition.AxisName = axisName;
            series.Definition.ShowItemToolTips = false;
            series.Definition.ItemLabelFormat = "f1";
            FillWithDataByHour(series, m_list, fieldName, index);

            return series;
        }


        private void FillWithAllDataByDay()
        {
            radChart1.DefaultView.ChartTitle.Content = string.Format("5天的平均体温 脉搏 呼吸 线状图");
            radChart1.DefaultView.ChartArea.DataSeries.Add(GenerateSeriesByDay("体温记录", string.Empty, new LineSeriesDefinition(), "Hztw"));
            radChart1.DefaultView.ChartArea.DataSeries.Add(GenerateSeriesByDay("脉搏记录", "YAxisMb", new LineSeriesDefinition(), "Hzmb"));
            radChart1.DefaultView.ChartArea.DataSeries.Add(GenerateSeriesByDay("呼吸记录", "YAxisHx", new LineSeriesDefinition(), "Hzhx"));

        }

        private void FillWithAllDataByHour(int index)
        {
            if (index > m_DaysName.Count - 1)/* add by dxj 2011/7/27 索引超出范围*/
            {
                radChart1.DefaultView.ChartArea.NoDataString = "无数据...";
                return;
            }
            radChart1.DefaultView.ChartTitle.Content = string.Format("{0}:详细体温 脉搏 呼吸 线状图", m_DaysName[index]);
            radChart1.DefaultView.ChartArea.DataSeries.Add(GenerateSeriesByHour("体温记录", string.Empty, new LineSeriesDefinition(), "Hztw", index));
            radChart1.DefaultView.ChartArea.DataSeries.Add(GenerateSeriesByHour("脉搏记录", "YAxisMb", new LineSeriesDefinition(), "Hzmb", index));
            radChart1.DefaultView.ChartArea.DataSeries.Add(GenerateSeriesByHour("呼吸记录", "YAxisHx", new LineSeriesDefinition(), "Hzhx", index));
        }

        #endregion


        #region 事件
        private void ChartArea_ItemToolTipOpening(ItemToolTip2D tooltip, ItemToolTipEventArgs e)
        {
            Telerik.Windows.Controls.RadChart chart = new Telerik.Windows.Controls.RadChart();
            chart.Height = 200;
            chart.Width = 300;

            chart.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Collapsed;
            chart.DefaultView.ChartArea.AxisX.LayoutMode = AxisLayoutMode.Inside;
            //string axisItemLabel = radChart1.DefaultView.ChartArea.AxisX.TickPoints[e.ItemIndex].Label;//X轴的坐标

            chart.DefaultView.ChartArea.AxisY.DefaultLabelFormat = "f1";

            DataSeries series = new DataSeries();
            series.Definition = new LineSeriesDefinition();
            series.Definition.ShowItemLabels = false;
            if (e.DataSeries.LegendLabel.Equals("体温记录"))
            {
                FillWithDataByHour(series, m_list, "Hztw", e.ItemIndex);
            }
            else if (e.DataSeries.LegendLabel.Equals("脉搏记录"))
            {
                FillWithDataByHour(series, m_list, "Hzmb", e.ItemIndex);
            }
            else if (e.DataSeries.LegendLabel.Equals("呼吸记录"))
            {
                FillWithDataByHour(series, m_list, "Hzhx", e.ItemIndex);
            }

            chart.DefaultView.ChartArea.DataSeries.Add(series);

            tooltip.Content = chart;
        }


        void ChartArea_ItemClick(object sender, ChartItemClickEventArgs e)
        {try{
            string axisItemLabel = radChart1.DefaultView.ChartArea.AxisX.TickPoints[e.ItemIndex].Label;
            radChart1.DefaultView.ChartArea.ItemClick -= new EventHandler<ChartItemClickEventArgs>(ChartArea_ItemClick); ;
            radChart1.DefaultView.ChartArea.DataSeries.Clear();


            DataSeries series = new DataSeries();
            series.Definition = new LineSeriesDefinition();
            series.LegendLabel = "按小时记录";
            series.Definition.ShowItemToolTips = false;
            if (e.DataSeries.LegendLabel.Equals("体温记录"))
            {
                radChart1.DefaultView.ChartTitle.Content = string.Format("{0}：{1}详细记录", axisItemLabel, "体温");
                FillWithDataByHour(series, m_list, "Hztw", e.ItemIndex);
            }
            else if (e.DataSeries.LegendLabel.Equals("脉搏记录"))
            {
                radChart1.DefaultView.ChartTitle.Content = string.Format("{0}：{1}详细记录", axisItemLabel, "脉搏");
                FillWithDataByHour(series, m_list, "Hzmb", e.ItemIndex);
            }
            else if (e.DataSeries.LegendLabel.Equals("呼吸记录"))
            {
                radChart1.DefaultView.ChartTitle.Content = string.Format("{0}：{1}详细记录", axisItemLabel, "呼吸");
                FillWithDataByHour(series, m_list, "Hzhx", e.ItemIndex);
            }

            radChart1.DefaultView.ChartArea.DataSeries.Add(series);
             }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }

        private void btnTw_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.btnAllByHour.Content = "查看全部(小时)";
                this.btnAllByHour.Visibility = System.Windows.Visibility.Visible;
                this.btnNextDay.Visibility = System.Windows.Visibility.Collapsed;
                this.radChart1.DefaultView.ChartArea.ItemClick -= new EventHandler<ChartItemClickEventArgs>(ChartArea_ItemClick);
                radChart1.DefaultView.ChartArea.AdditionalYAxes.Clear();
                radChart1.DefaultView.ChartTitle.Content = "5天的平均体温记录线状图";
                this.FillRadChartData("Hztw");
                this.radChart1.DefaultView.ChartArea.ItemClick += new EventHandler<ChartItemClickEventArgs>(ChartArea_ItemClick);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnMb_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.btnAllByHour.Content = "查看全部(小时)";
                this.btnAllByHour.Visibility = System.Windows.Visibility.Visible;
                this.btnNextDay.Visibility = System.Windows.Visibility.Collapsed;
                this.radChart1.DefaultView.ChartArea.ItemClick -= new EventHandler<ChartItemClickEventArgs>(ChartArea_ItemClick);
                radChart1.DefaultView.ChartArea.AdditionalYAxes.Clear();
                radChart1.DefaultView.ChartTitle.Content = "5天的平均脉搏记录线状图";
                this.FillRadChartData("Hzmb");
                this.radChart1.DefaultView.ChartArea.ItemClick += new EventHandler<ChartItemClickEventArgs>(ChartArea_ItemClick);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnHx_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.btnAllByHour.Content = "查看全部(小时)";
                this.btnAllByHour.Visibility = System.Windows.Visibility.Visible;
                this.btnNextDay.Visibility = System.Windows.Visibility.Collapsed;
                this.radChart1.DefaultView.ChartArea.ItemClick -= new EventHandler<ChartItemClickEventArgs>(ChartArea_ItemClick);
                radChart1.DefaultView.ChartArea.AdditionalYAxes.Clear();
                radChart1.DefaultView.ChartTitle.Content = "5天的平均呼吸记录线状图";
                this.FillRadChartData("Hzhx");
                this.radChart1.DefaultView.ChartArea.ItemClick += new EventHandler<ChartItemClickEventArgs>(ChartArea_ItemClick);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnAllByDay_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.btnAllByHour.Content = "查看全部(小时)";
                this.btnAllByHour.Visibility = System.Windows.Visibility.Visible;
                this.btnNextDay.Visibility = System.Windows.Visibility.Collapsed;
                this.radChart1.DefaultView.ChartArea.ItemClick -= new EventHandler<ChartItemClickEventArgs>(ChartArea_ItemClick);
                radChart1.DefaultView.ChartArea.DataSeries.Clear();
                radChart1.DefaultView.ChartArea.AdditionalYAxes.Clear();
                radChart1.DefaultView.ChartArea.AdditionalYAxes.Add(new AxisY());
                radChart1.DefaultView.ChartArea.AdditionalYAxes.Add(new AxisY());
                radChart1.DefaultView.ChartArea.AdditionalYAxes[0].AxisName = "YAxisMb";
                radChart1.DefaultView.ChartArea.AdditionalYAxes[1].AxisName = "YAxisHx";
                this.FillWithAllDataByDay();
                this.radChart1.DefaultView.ChartArea.ItemClick += new EventHandler<ChartItemClickEventArgs>(ChartArea_ItemClick);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnAllByHour_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //this.btnAllByHour.Content = "查看下一天";
                this.btnAllByHour.Visibility = System.Windows.Visibility.Collapsed;
                this.btnNextDay.Visibility = System.Windows.Visibility.Visible;
                this.radChart1.DefaultView.ChartArea.ItemClick -= new EventHandler<ChartItemClickEventArgs>(ChartArea_ItemClick);
                radChart1.DefaultView.ChartArea.DataSeries.Clear();
                radChart1.DefaultView.ChartArea.AdditionalYAxes.Clear();
                radChart1.DefaultView.ChartArea.AdditionalYAxes.Add(new AxisY());
                radChart1.DefaultView.ChartArea.AdditionalYAxes.Add(new AxisY());
                radChart1.DefaultView.ChartArea.AdditionalYAxes[0].AxisName = "YAxisMb";
                radChart1.DefaultView.ChartArea.AdditionalYAxes[1].AxisName = "YAxisHx";
                FillWithAllDataByHour(m_index % 5);
                m_index++;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion

    }
}
