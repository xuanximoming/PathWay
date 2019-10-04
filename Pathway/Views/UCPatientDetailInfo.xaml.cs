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
using YidanEHRApplication.DataService;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using YidanEHRApplication.Models;
using Telerik.Windows.Controls.Charting;
using System.Text;
using System.Windows.Markup;
using System.Windows.Browser;
using Telerik.Windows.Controls.GridView;
using System.Windows.Data;

namespace YidanEHRApplication.Views
{
    public partial class UCPatientDetailInfo : UserControl
    {
        #region  vars

        private double m_ConfigValue = 5;
        #endregion

        /// <summary>
        /// ctor
        /// </summary>
        public UCPatientDetailInfo()
        {
            InitializeComponent();

            InitVars();
        }

        #region  private methods

        private void InitVars()
        {
            LayoutRoot.Height = Browser.AvailableScreenHeight - 100;
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.GetBaseSettingStrCompleted +=
                    (obj, e) =>
                    {

                        if (e.Error == null)
                        {
                            string cfg = e.Result;
                            if (string.IsNullOrEmpty(cfg))
                                cfg = "5";
                            m_ConfigValue = Convert.ToDouble(cfg);

                            InitBaseInfo();
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }

                    };
            client.GetBaseSettingStrAsync("DEFAULTSTATDAY");
            client.CloseAsync();

        }

        /// <summary>
        /// 初始化上半部分信息
        /// </summary>
        private void InitBaseInfo()
        {
            InitDateTimePicker();
            InitPatFee();
            InitPhySign();
        }

         
	



        #endregion

        #region  xjt

        private void radDatePickerBegin_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count == 1)
                {
                    this.radDatePickerEnd.SelectionChanged -= new Telerik.Windows.Controls.SelectionChangedEventHandler(radDatePickerEnd_SelectionChanged);
                    this.radDatePickerBegin.SelectionChanged -= new Telerik.Windows.Controls.SelectionChangedEventHandler(radDatePickerBegin_SelectionChanged);
                    radDatePickerEnd.SelectedDate = radDatePickerBegin.SelectedDate.Value.AddDays(m_ConfigValue);
                    InitPhySign();
                    this.radDatePickerEnd.SelectionChanged += new Telerik.Windows.Controls.SelectionChangedEventHandler(radDatePickerEnd_SelectionChanged);
                    this.radDatePickerBegin.SelectionChanged += new Telerik.Windows.Controls.SelectionChangedEventHandler(radDatePickerBegin_SelectionChanged);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void radDatePickerEnd_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count == 1)
                {
                    this.radDatePickerEnd.SelectionChanged -= new Telerik.Windows.Controls.SelectionChangedEventHandler(radDatePickerEnd_SelectionChanged);
                    this.radDatePickerBegin.SelectionChanged -= new Telerik.Windows.Controls.SelectionChangedEventHandler(radDatePickerBegin_SelectionChanged);
                    radDatePickerBegin.SelectedDate = radDatePickerEnd.SelectedDate.Value.AddDays(Convert.ToDouble(0 - m_ConfigValue));
                    InitPhySign();
                    this.radDatePickerEnd.SelectionChanged += new Telerik.Windows.Controls.SelectionChangedEventHandler(radDatePickerEnd_SelectionChanged);
                    this.radDatePickerBegin.SelectionChanged += new Telerik.Windows.Controls.SelectionChangedEventHandler(radDatePickerBegin_SelectionChanged);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        /// <summary>
        /// 初始化时间控件
        /// </summary>
        private void InitDateTimePicker()
        {
            this.radDatePickerEnd.SelectionChanged -= new Telerik.Windows.Controls.SelectionChangedEventHandler(radDatePickerEnd_SelectionChanged);
            this.radDatePickerBegin.SelectionChanged -= new Telerik.Windows.Controls.SelectionChangedEventHandler(radDatePickerBegin_SelectionChanged);
            this.radDatePickerBegin.SelectedDate = DateTime.Now.AddDays(Convert.ToDouble(0 - m_ConfigValue));
            this.radDatePickerEnd.SelectedDate = DateTime.Now;
            this.radDatePickerEnd.SelectionChanged += new Telerik.Windows.Controls.SelectionChangedEventHandler(radDatePickerEnd_SelectionChanged);
            this.radDatePickerBegin.SelectionChanged += new Telerik.Windows.Controls.SelectionChangedEventHandler(radDatePickerBegin_SelectionChanged);
        }

        /// <summary>
        /// 费用饼状图
        /// </summary>
        private void InitPatFee()
        {
            RadChartFee.DefaultView.ChartArea.NoDataString = "无数据...";
            RadChartFee.DefaultView.ChartLegend.Header = "类型...";
            //费用饼状图,xjt
            //YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            //RadChartFee.DefaultView.ChartArea.NoDataString = "数据生成中...";
            //RadChartFee.DefaultView.ChartTitle.Content = "病患费用统计";/* update by dxj 2011/7/27 显示标题*/
            //RadChartFee.DefaultView.ChartTitle.Visibility = System.Windows.Visibility.Visible;
            //RadChartFee.DefaultView.ChartLegend.Header = "费用统计";
            //client.GetInpatientFeeInfoCompleted +=
            //        (obj, e) =>
            //        {
            //            if (e.Error != null)
            //            {
            //                PublicMethod.RadWaringBox(e.Error);
            //                return;
            //            }
            //            ObservableCollection<CP_InpatientFeeInfo> inpatientFee = e.Result;
            //            RadChartFee.DefaultView.ChartArea.Margin = new Thickness(0, 0, 0, 0);
            //            if (inpatientFee == null || inpatientFee.Count == 0)
            //            {
            //                RadChartFee.DefaultView.ChartArea.NoDataString = "无数据...";
            //                return;
            //            }


            //            SeriesMapping seriesMapping = new SeriesMapping();
            //            seriesMapping.SeriesDefinition = new PieSeriesDefinition();
            //            seriesMapping.SeriesDefinition.ItemLabelFormat = "#Y{C}"; //"金额：#Y{C0.00}\r\n比例：#%{p0}"
            //            seriesMapping.SeriesDefinition.ShowItemToolTips = true;

            //            //选中特效
            //            seriesMapping.SeriesDefinition.InteractivitySettings.HoverScope = InteractivityScope.Item;
            //            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionScope = InteractivityScope.Item;
            //            seriesMapping.SeriesDefinition.InteractivitySettings.SelectionMode = ChartSelectionMode.Single;

            //            //smart labels pie上显示字的位置
            //            ((RadialSeriesDefinition)seriesMapping.SeriesDefinition).LabelSettings.LabelOffset = 0.7d;

            //            seriesMapping.SeriesDefinition.ItemToolTipFormat = "项目: #Xmmc\r\n比例：#%{p0}\r\n总计：#Zj"; //"项目: #Xmmc\r\n金额: #Y{0.00}"
            //            ItemMapping itemMapping = new ItemMapping();

            //            itemMapping.DataPointMember = DataPointMember.YValue;
            //            itemMapping.FieldName = "Xmje";
            //            seriesMapping.ItemMappings.Add(itemMapping);


            //            itemMapping = new ItemMapping();
            //            itemMapping.DataPointMember = DataPointMember.LegendLabel;
            //            itemMapping.FieldName = "Xmmc";
            //            seriesMapping.ItemMappings.Add(itemMapping);

            //            RadChartFee.SeriesMappings.Add(seriesMapping);



            //            RadChartFee.DefaultView.ChartLegend.Visibility = System.Windows.Visibility.Visible;
            //            RadChartFee.DefaultView.ChartArea.AxisX.MajorGridLinesVisibility = System.Windows.Visibility.Visible;
            //            RadChartFee.DefaultView.ChartArea.AxisX.MinorGridLinesVisibility = System.Windows.Visibility.Collapsed;
            //            RadChartFee.DefaultView.ChartArea.LabelFormatBehavior = LabelFormatBehavior.None;

            //            RadChartFee.ItemsSource = inpatientFee;
            //            //RadChartFee.DefaultView.ChartLegend.Style = Resources["CustomLegendStyle"] as Style;

            //        };
            //client.GetInpatientFeeInfoAsync(int.Parse(Global.InpatientListCurrent.Syxh));
            //client.CloseAsync();
        }
        /// <summary>
        /// 体征信息
        /// </summary>
        private void InitPhySign()
        {
            //<uc:PatientDetailVitalSigns x:Name="vitalSigns" StartDay="" EndDay="" ></uc:PatientDetailVitalSigns>
            this.phySingCanvas.Children.Clear();
            UCPatientDetailVitalSigns vitalSigns = new UCPatientDetailVitalSigns();
            vitalSigns.Margin = new Thickness(0, 0, 0, 0);
            vitalSigns.StartDay= radDatePickerBegin.SelectedDate.Value;
            vitalSigns.EndDay = radDatePickerEnd.SelectedDate.Value;
            vitalSigns.Zyhm = Global.InpatientListCurrent.Zyhm;
            vitalSigns.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            vitalSigns.VerticalAlignment = System.Windows.VerticalAlignment.Top;
            this.phySingCanvas.Children.Add(vitalSigns);
        }

        private void radGridViewPathInfo_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            foreach (GridViewCellBase cell in e.Row.Cells)
            {
                cell.SetValue(ToolTipService.ToolTipProperty, ((TextBlock)(((ContentControl)(cell)).Content)).Text);
            }
            #region
            //if (e.Row is GridViewRow)
            //{
            //    GridViewRow headerRow = e.Row as GridViewRow;
            //    foreach (GridViewCellBase cell in headerRow.Cells)
            //    {
            //        cell.SetValue(ToolTipService.ToolTipProperty, ((TextBlock)(((ContentControl)(cell)).Content)).Text);
            //    }
            //}
            #endregion
        }

        #endregion
    }

}
