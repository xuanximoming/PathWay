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
using System.Collections.ObjectModel;
using YidanEHRApplication.DataService;
using Telerik.Windows.Controls.Charting;
using System.Text;
using System.Windows.Markup;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Views
{
    public partial class UCPatientDetailPhySign : UserControl
    {
        /// <summary>
        /// 开始时间,格式yyyyMMdd
        /// </summary>
        public DateTime DateTimeBegin
        { get; set; }
        /// <summary>
        /// 结束时间
        /// </summary>
        public DateTime DateTimeEnd
        { get; set; }
        private int m_Flag = 0;
        private double m_ConfigValue = 1;
        public double ConfigValue
        {
            private get { return m_ConfigValue; }
            set { m_ConfigValue = value; }
        }


        private ObservableCollection<CP_InpatientPhySign> m_InpatientPhySign;

        public UCPatientDetailPhySign()
        {
            InitializeComponent();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {     try{
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            //体征线状图,xjt      
            RadChartPhySign.DefaultView.ChartArea.NoDataString = "数据生成中...";
            RadChartPhySign.DefaultView.ChartLegend.Header = "体征信息";
            client.GetInpatientPhySignCompleted +=
                (obj, ea) =>
                {
                    m_InpatientPhySign = ea.Result;

                    if (m_InpatientPhySign.Count == 0)
                    {
                        this.BackCanvas.SetValue(Canvas.WidthProperty, Convert.ToDouble(20));
                        this.BackCanvas.SetValue(Canvas.BackgroundProperty, new SolidColorBrush(Colors.White));
                        this.parentCanvas.SetValue(Canvas.WidthProperty, RadChartPhySign.DefaultView.ChartArea.ActualWidth + RadChartPhySign.DefaultView.ChartLegend.ActualWidth);
                        this.parentCanvas.SetValue(Canvas.BackgroundProperty, new SolidColorBrush(Colors.White));
                        RadChartPhySign.DefaultView.ChartArea.NoDataString = "无数据...";
                        InitXLineCanvans(m_ConfigValue + 1);
                        return;
                    }

                    RadChartPhySign.SeriesMappings.Clear(); //清空


                    SeriesMapping smTw = new SeriesMapping();
                    smTw.LegendLabel = "体温";
                    smTw.ItemMappings.Add(new ItemMapping("Tw", DataPointMember.YValue));
                    SeriesDefinition definitionTw = new LineSeriesDefinition();
                    definitionTw.ShowItemLabels = false;
                    definitionTw.ShowItemToolTips = true;
                    definitionTw.InteractivitySettings.HoverScope = InteractivityScope.Series;
                    definitionTw.InteractivitySettings.SelectionScope = InteractivityScope.Series;

                    smTw.SeriesDefinition = definitionTw;
                    RadChartPhySign.SeriesMappings.Add(smTw);
                    RadChartPhySign.DefaultView.ChartArea.AxisX.DefaultLabelFormat = "#VAL";

                    SeriesMapping smMb = new SeriesMapping();
                    smMb.LegendLabel = "脉搏";
                    smMb.ItemMappings.Add(new ItemMapping("Mb", DataPointMember.YValue));
                    SeriesDefinition definitionMb = new LineSeriesDefinition();
                    definitionMb.ShowItemLabels = false;
                    definitionMb.ShowItemToolTips = true;
                    definitionMb.InteractivitySettings.HoverScope = InteractivityScope.Series;
                    definitionMb.InteractivitySettings.SelectionScope = InteractivityScope.Series;

                    smMb.SeriesDefinition = definitionMb;
                    RadChartPhySign.SeriesMappings.Add(smMb);


                    SeriesMapping smXl = new SeriesMapping();
                    smXl.LegendLabel = "心率";
                    smXl.ItemMappings.Add(new ItemMapping("Xl", DataPointMember.YValue));
                    SeriesDefinition definitionXl = new LineSeriesDefinition();
                    definitionXl.ShowItemLabels = false;
                    definitionXl.ShowItemToolTips = true;
                    definitionXl.InteractivitySettings.HoverScope = InteractivityScope.Series;
                    definitionXl.InteractivitySettings.SelectionScope = InteractivityScope.Series;

                    smXl.SeriesDefinition = definitionXl;
                    RadChartPhySign.SeriesMappings.Add(smXl);

                    SeriesMapping smXy = new SeriesMapping();
                    smXy.LegendLabel = "血压";
                    smXy.ItemMappings.Add(new ItemMapping("Xy", DataPointMember.YValue));
                    SeriesDefinition definitionXy = new LineSeriesDefinition();
                    definitionXy.ShowItemLabels = false;
                    definitionXy.ShowItemToolTips = true;
                    definitionXy.InteractivitySettings.HoverScope = InteractivityScope.Series;
                    definitionXy.InteractivitySettings.SelectionScope = InteractivityScope.Series;

                    smXy.SeriesDefinition = definitionXy;
                    RadChartPhySign.SeriesMappings.Add(smXy);


                    RadChartPhySign.DefaultView.ChartArea.AxisX.Visibility = System.Windows.Visibility.Collapsed;
                    RadChartPhySign.DefaultView.ChartArea.AxisX.LayoutMode = AxisLayoutMode.Between;

                    RadChartPhySign.DefaultView.ChartArea.AxisY.AutoRange = true;//为FALSE下面的配置才起作用
                    RadChartPhySign.DefaultView.ChartArea.AxisY.MinValue = 0;
                    RadChartPhySign.DefaultView.ChartArea.AxisY.MaxValue = 200;
                    RadChartPhySign.DefaultView.ChartArea.AxisY.Step = 50;

                    RadChartPhySign.ItemsSource = m_InpatientPhySign;

                    InitXLineCanvans(m_InpatientPhySign[0].Staticday);
                };
            //to do 待输入正确病例号
            client.GetInpatientPhySignAsync(1, DateTimeBegin.ToString("yyyyMMdd"), DateTimeEnd.ToString("yyyyMMdd"));
            client.CloseAsync(); }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }

        
		

        private void InitXLineCanvans(double staticDate)
        {
            this.BackCanvas.SetValue(Canvas.WidthProperty, Convert.ToDouble(20));
            this.BackCanvas.SetValue(Canvas.BackgroundProperty, new SolidColorBrush(Colors.White));
            this.parentCanvas.SetValue(Canvas.WidthProperty, RadChartPhySign.DefaultView.ChartArea.ActualWidth + RadChartPhySign.DefaultView.ChartLegend.ActualWidth);
            this.parentCanvas.SetValue(Canvas.BackgroundProperty, new SolidColorBrush(Colors.White));
            double count = 6 * staticDate;//需要构造的小框总数
            double totalWidth = RadChartPhySign.DefaultView.ChartArea.ActualWidth - 48;//chart宽度
            int evenWidth = int.Parse(totalWidth.ToString()) / int.Parse(count.ToString());//每个小框的宽度
            int tableWieht = evenWidth * 6;//一个时间格的宽度
            //构造大框
            for (int i = 0; i < staticDate; i++)
            {
                StringBuilder Xaml6 = new StringBuilder();//构造最上边日期的框的stackpanel
                StringBuilder Xaml7 = new StringBuilder();//构造日期框Rectangle
                Xaml6.Append("<StackPanel Canvas.Left='" + (tableWieht * i + 20) + "' Canvas.Top='20' ");
                Xaml6.Append(" xmlns=\"http://schemas.microsoft.com/client/2007\" >");
                Xaml6.Append("</StackPanel>");
                Xaml7.Append("<Rectangle Width='" + tableWieht + "'");
                Xaml7.Append(" Height='20' Stroke='Black' ");
                Xaml7.Append(" xmlns=\"http://schemas.microsoft.com/client/2007\" > ");
                Xaml7.Append(" </Rectangle>");
                StackPanel StakRec = (StackPanel)XamlReader.Load(Xaml6.ToString());
                this.parentCanvas.Children.Add(StakRec);
                Rectangle rec = (Rectangle)XamlReader.Load(Xaml7.ToString());
                StakRec.Children.Add(rec);

            }
            List<string> listDate = new List<string>();
            if (m_InpatientPhySign.Count != 0)
            {

                for (int i = 0; i < m_InpatientPhySign.Count; i++)
                {
                    string strDate = m_InpatientPhySign[i].Zlrq.Substring(0, 4) + "-" + m_InpatientPhySign[i].Zlrq.Substring(4, 2) + "-" + m_InpatientPhySign[i].Zlrq.Substring(6, 2);
                    if (!listDate.Contains(strDate))
                        listDate.Add(Convert.ToDateTime(strDate).ToString("yyyy-MM-dd"));
                }
            }
            else
            {
                for (int i = 0; i < staticDate; i++)
                {
                    //todo modified by 周辉   此处不明白为什么要如此
                    //string strDate = Convert.ToDateTime(DateTimeBeginFormat).AddDays(i - staticDate + 2).ToString();
                    string strDate = Convert.ToDateTime(DateTimeBegin.ToString("yyyy-MM-dd")).AddDays(i).ToString();
                    if (!listDate.Contains(strDate))
                        listDate.Add(Convert.ToDateTime(strDate).ToString("yyyy-MM-dd"));
                }
            }
            //构造日期
            for (int l = 0; l < staticDate; l++)
            {
                StringBuilder Xaml9 = new StringBuilder();
                Xaml9.Append("<TextBlock Canvas.Top='25' Canvas.Left='" + (75 + tableWieht * l) + "' TextAlignment='Center'");
                Xaml9.Append(" xmlns=\"http://schemas.microsoft.com/client/2007\" >");
                Xaml9.Append(listDate[l].ToString());   //此处放置日期
                Xaml9.Append("</TextBlock>");
                TextBlock textblock = (TextBlock)XamlReader.Load(Xaml9.ToString());
                this.parentCanvas.Children.Add(textblock);
            }
            //构造4,8,12,16....
            for (int k = 0; k < count; k++)
            {
                StringBuilder Xaml8 = new StringBuilder();
                Xaml8.Append("<TextBlock TextAlignment='Center' FontSize='12' Canvas.Top='6' Canvas.Left='" + ((evenWidth / 2) + 10 + evenWidth * k) + "'");
                Xaml8.Append(" xmlns=\"http://schemas.microsoft.com/client/2007\" >");
                m_Flag = m_Flag + 4;
                Xaml8.Append(" " + ((m_Flag > 24) ? m_Flag = 4 : m_Flag) + "</TextBlock>");//m_Flag = 0; 
                TextBlock textblock = (TextBlock)XamlReader.Load(Xaml8.ToString());
                this.parentCanvas.Children.Add(textblock);
            }
            //构造小框
            for (int j = 0; j < count; j++)
            {
                StringBuilder Xaml1 = new StringBuilder();
                StringBuilder Xaml5 = new StringBuilder();//stackpanel 构造字符串
                Xaml5.Append("<StackPanel Canvas.Left='" + (evenWidth * j + 20) + "' Canvas.Top='0' ");
                Xaml5.Append("xmlns=\"http://schemas.microsoft.com/client/2007\" >");
                Xaml5.Append(" </StackPanel>");
                Xaml1.Append("<Rectangle Width='" + evenWidth + "'");
                Xaml1.Append(" Height='20' Stroke='Black' ");
                Xaml1.Append(" xmlns=\"http://schemas.microsoft.com/client/2007\" > ");
                Xaml1.Append(" </Rectangle>");

                StackPanel Stak = (StackPanel)XamlReader.Load(Xaml5.ToString());
                Rectangle rec = (Rectangle)XamlReader.Load(Xaml1.ToString());
                this.parentCanvas.Children.Add(Stak); //添加stackpanel到canvas
                Stak.Children.Add(rec);

            }
        }


    }
}
