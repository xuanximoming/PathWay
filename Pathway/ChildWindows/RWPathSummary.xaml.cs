using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Printing;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Views.ChildWindows
{

    public partial class RWPathSummary
    {
        public RWPathSummary(String syxh, String ljxh, String ljmc, String hzxm, Boolean isAll, Int32 isVariant, CP_InpatinetList currentpat)
        {
            Syxh = syxh;
            Ljxh = ljxh;
            Ljmc = ljmc;
            Hzxm = hzxm;
            IsVariant = isVariant;
            m_currentpat = currentpat;
            InitializeComponent();
            radBusyIndicator.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };
        }

        #region 变量
        public String Syxh;
        public String Ljxh;
        public String Ljmc;
        public String Hzxm;

        public List<RW_PathSummary_new> pathSummary_newlist;

        public RW_PathSummary pathSummary;                                      //加载窗口时的全类(每次开窗口后值不变（稳定）)
        public String workFlow;                                                 //当前节点(每次开窗口后值不变（稳定）)
        public List<RW_PathSummaryCategory> categoryList;                       //需要显示的字典列表(每次开窗口后值不变（稳定）)
        public List<RW_PathSummaryEnForce> pathEnForceList;                     //全局节点列表(每次开窗口后值不变（稳定）)

        public Int32 IsVariant;        //显示变异

        /// <summary>
        /// 当前选中病人
        /// </summary>
        public CP_InpatinetList m_currentpat;

        #endregion
        #region 事件
        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {

            try
            {
                txtPatient.Text = "患者姓名：" + m_currentpat.Hzxm + "      性别：" + m_currentpat.Brxb + "    年龄：" + m_currentpat.Xsnl + "    门诊号：" + m_currentpat.Hissyxh + "      住院号：" + m_currentpat.Zyhm;
                txtClinicalPath.Text = "       引入路径：" + Ljmc + "   入院诊断:" + m_currentpat.Ryzd + "(" + m_currentpat.RyzdCode + ")    路径状态：" + m_currentpat.LjztName;
                txtinpathTime.Text = " 住院日期：" + m_currentpat.Ryrq + "         出院日期：" + m_currentpat.Cyrq;
                GetRWPathSummary_new();

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void checkBoxAll_Click(object sender, RoutedEventArgs e)
        {
            //total.Children.Clear();
            //ShowWorkFlow();
        }
        private void ComboBoxVariant_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //IsVariant = ConvertMy.ToInt32((ComboBoxVariant.SelectedItem as ComboBoxItem).Tag); 

            //total.Children.Clear();
            //ShowWorkFlow();
        }
        #endregion
        #region 方法
        /// <summary>
        /// 获取该病人，该次路径的数据（5.5）
        /// </summary>
        private void GetRWPathSummary_new()
        {
            radBusyIndicator.IsBusy = true;

            YidanEHRDataServiceClient GetRWPathSummaryClient = PublicMethod.YidanClient;
            GetRWPathSummaryClient.GetRW_PathSummary_newCompleted +=
            (obj, e) =>
            {
                try
                {
                    radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        if (e.Result == null)
                        {
                            return;
                        }
                        else
                        {
                            pathSummary_newlist = null;         //清空
                            pathSummary_newlist = e.Result.ToList();

                            if (pathSummary_newlist.Count != 0)
                            {
                                //txtWorkFlow.Text = "        当前步骤：" + pathEnForceList[pathEnForceList.Count - 1].Ljmc;    //当前步骤（如果没有节点，则此处报错）
                                //workFlow = pathEnForceList[pathEnForceList.Count - 1].Jddm;                            //当前节点代码
                                txtinpathTime.Text = " 住院日期：" + m_currentpat.Ryrq + "        入径日期：" + pathSummary_newlist[0].JRSJ + "        出院日期：" + m_currentpat.Cyrq;
                                RadGridView1.ItemsSource = pathSummary_newlist;
                                //ShowWorkFlow();
                            }
                        }
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                }
                catch (Exception ex)
                {
                    YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            };

            GetRWPathSummaryClient.GetRW_PathSummary_newAsync(Syxh);
            //MessageBox.Show(Syxh);
            GetRWPathSummaryClient.CloseAsync();
        }

        /// <summary>
        /// 获取该病人，该次路径的数据（5.5）
        /// </summary>
        private void GetRWPathSummary()
        {
            radBusyIndicator.IsBusy = true;

            YidanEHRDataServiceClient GetRWPathSummaryClient = PublicMethod.YidanClient;
            GetRWPathSummaryClient.GetRW_PathSummaryCompleted +=
            (obj, e) =>
            {
                try
                {
                    radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        if (e.Result == null)
                        {
                            return;
                        }
                        else
                        {
                            pathSummary = null;         //清空
                            pathSummary = e.Result;
                            categoryList = pathSummary.PathSummaryCategory.ToList();
                            pathEnForceList = pathSummary.PathSummaryEnForce.ToList();

                            if (pathEnForceList.Count != 0)
                            {
                                //txtWorkFlow.Text = "        当前步骤：" + pathEnForceList[pathEnForceList.Count - 1].Ljmc;    //当前步骤（如果没有节点，则此处报错）
                                workFlow = pathEnForceList[pathEnForceList.Count - 1].Jddm;                            //当前节点代码

                                //ShowWorkFlow();
                            }
                        }
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                }
                catch (Exception ex)
                {
                    YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            };

            GetRWPathSummaryClient.GetRW_PathSummaryAsync(Syxh, Ljxh);
            GetRWPathSummaryClient.CloseAsync();
        }

        private int index = 0;
        private void print_Click(object sender, RoutedEventArgs e)
        {

            RWPathSummary_Print pageprint = new RWPathSummary_Print();

            pageprint.Syxh = m_currentpat.Syxh;
            pageprint.WindowState = System.Windows.WindowState.Maximized;
            pageprint.ResizeMode = Telerik.Windows.Controls.ResizeMode.NoResize;

            pageprint.ShowDialog();

            ////PrintDocument document = new PrintDocument();
            ////document.PrintPage += new EventHandler<PrintPageEventArgs>(document_PrintPage);
            ////document.Print("Print Image");

            //PrintDocument doc = new PrintDocument();

            //doc.PrintPage += (s, ea) =>
            //{

            //    StackPanel printPanel = new StackPanel();
            //    printPanel = this.stackpanel1;
            //    Grid grid = this.all;
            //    //grid.Width = ea.PrintableArea.Width;
            //    //grid.Height = ea.PrintableArea.Height;
            //    ea.PageVisual = grid;

            //    //MessageBox.Show("index:" + index.ToString());
            //    //MessageBox.Show("grid.Height:" + Convert.ToInt32(this.all.Height));
            //    //MessageBox.Show("pageheight:" + (ea.PrintableArea.Height - ea.PageMargins.Top - ea.PageMargins.Bottom).ToString());
            //        //如果打印的当前行高度不合适的话，则进行分页                
            //        if (grid.Height- (index*(ea.PrintableArea.Height - ea.PageMargins.Top - ea.PageMargins.Bottom)) > (ea.PrintableArea.Height - ea.PageMargins.Top - ea.PageMargins.Bottom))                
            //        {                    
            //            ea.HasMorePages = true;
            //            index++;
            //            return ;                
            //        }
            //        else
            //            ea.HasMorePages = false;


            //    //Thickness margin = new Thickness{
            //    //    Left = Math.Max(0, 96 - ea.PageMargins.Left),
            //    //    Top = Math.Max(0, 96 - ea.PageMargins.Top),
            //    //    Right = Math.Max(0, 96 - ea.PageMargins.Right),
            //    //    Bottom = Math.Max(0, 96 - ea.PageMargins.Bottom)
            //    //};

            //    //grid.Margin = margin;



            //    //ea.HasMorePages = false;

            //};

            //PrinterFallbackSettings settings = new PrinterFallbackSettings();

            //settings.ForceVector = true;


            //settings.OpacityThreshold = 0.5;

            //doc.Print("Silverlight Forced Vector Print", settings);

        }

        void document_PrintPage(object sender, PrintPageEventArgs e)
        {
            //Image imagePrint = new Image(); 
            //imagePrint.Source = img.Source; 
            //imagePrint.Height = e.PrintableArea.Height; 
            //imagePrint.Width = e.PrintableArea.Width; 
            //e.PageVisual = imagePrint; 
            //e.HasMorePages = false; 

            //Image imagePrint = new Image();
            //imagePrint.Source = img.Source;
            //imagePrint.Height = e.PrintableArea.Height;
            //imagePrint.Width = e.PrintableArea.Width;
            //e.PageVisual = this.page;
            //e.HasMorePages = false; 
        }



        /// <summary>
        /// 读取流程图节点，根据节点排列（5.11）
        /// 绘图
        /// </summary>
        //private void ShowWorkFlow()
        //{
        //    //if (pathEnForceList.Count != 0)                                             //须有节点
        //    //{
        //        List<RW_PathSummaryEnForce> newPathEnForceList = new List<RW_PathSummaryEnForce>();
        //        List<RW_PathSummaryOrder> orderList = new List<RW_PathSummaryOrder>();
        //        List<RW_PathSummaryVariation> variationList = new List<RW_PathSummaryVariation>();

        //        //if (!checkBoxAll.IsChecked.Value)         //全局判断开关
        //        //{
        //        //    newPathEnForceList = pathEnForceList.Where(s => s.Jddm == workFlow).ToList();      //筛选改值
        //        //}
        //        //else
        //        //{
        //        //    newPathEnForceList = pathEnForceList;
        //        //}

        //        if (IsVariant == 1)  //全局判断开关
        //        {
        //            orderList = pathSummary.PathSummaryOrderList.ToList();                  //只取执行
        //        }
        //        else if (IsVariant == 2)//全局判断开关 
        //        {
        //            variationList = pathSummary.PathSummaryVariation.ToList();              //只取变异
        //        }
        //        else 
        //        {
        //            orderList = pathSummary.PathSummaryOrderList.ToList();
        //            variationList = pathSummary.PathSummaryVariation.ToList();
        //        }
        //        total = new Grid();
        //        ColumnDefinition colFirst = new ColumnDefinition();
        //        colFirst.Width = new GridLength(150);
        //        ColumnDefinition colSecond = new ColumnDefinition();
        //        GridLength gridLength = new GridLength(1, GridUnitType.Star);
        //        colSecond.Width = gridLength;
        //        total.ColumnDefinitions.Add(colFirst);
        //        total.ColumnDefinitions.Add(colSecond);
        //        foreach (RW_PathSummaryEnForce item in newPathEnForceList)
        //        {
        //            List<RW_PathSummaryOrder> newOrderList = new List<RW_PathSummaryOrder>();
        //            List<RW_PathSummaryVariation> newVariationList = new List<RW_PathSummaryVariation>();

        //            int orderCount;
        //            int variationCount;
        //            newOrderList = orderList.Where(s => s.ActivityId == item.Jddm).ToList();           //筛选改值
        //            orderCount = newOrderList.Count;
        //            newVariationList = variationList.Where(s => s.PahtDetailId == item.Jddm).ToList(); //筛选改值
        //            variationCount = newVariationList.Count;

        //            if (orderCount == 0 && variationCount == 0)         //该节点下没有医嘱和变异记录
        //            {
        //                continue;
        //            }
        //            else
        //            {
        //                RowDefinition Flowrow = new RowDefinition();
        //                Flowrow.MaxHeight = 30;                                                    //行高    
        //                total.RowDefinitions.Add(Flowrow);
        //                StackPanel FlowPanel = new StackPanel();                                   //容器
        //                FlowPanel.Orientation = Orientation.Horizontal;

        //                TextBlock txtFlow = new TextBlock();
        //                TextBlock txtTime = new TextBlock();
        //                txtFlow.Text = item.Ljmc;
        //                txtTime.Text = "    " + item.Jrsj;
        //                txtFlow.FontSize = 14;                                                     //节点名称字体
        //                txtTime.FontSize = 12;
        //                txtFlow.FontWeight = FontWeights.Bold;                                     //节点名称加粗

        //                FlowPanel.Children.Add(txtFlow);
        //                FlowPanel.Children.Add(txtTime);
        //                total.Children.Add(FlowPanel);
        //                FlowPanel.SetValue(Grid.RowProperty, total.RowDefinitions.Count - 1);
        //                FlowPanel.SetValue(Grid.ColumnProperty, 0);
        //                FlowPanel.SetValue(Grid.ColumnSpanProperty, 3);                            //节点文本框跨三列

        //                foreach (RW_PathSummaryCategory category in categoryList)
        //                {
        //                    GetData(category, newOrderList, newVariationList);
        //                }

        //            }
        //        }
        //        PageScrollViewer.Content = null;
        //        PageScrollViewer.Content = total;
        //    //}
        //    //else
        //    //{
        //    //    return;
        //    //}
        //}

        /// <summary>
        /// 模块排序（5.5）
        /// </summary>
        //private void GetData(RW_PathSummaryCategory category, List<RW_PathSummaryOrder> newOrderList, List<RW_PathSummaryVariation> newVariationList)
        //{
        //    int count = 0;
        //    List<RW_PathSummaryOrder> TwoOrderList = new List<RW_PathSummaryOrder>();
        //    List<RW_PathSummaryVariation> TwoVariationList = new List<RW_PathSummaryVariation>();

        //    if (category.Lb == 1)
        //    {
        //        TwoOrderList = newOrderList.Where(s => s.Xmlb == category.Mxbh).ToList();         //筛选改值
        //        count = TwoOrderList.Count;
        //    }
        //    else if (category.Lb == 2)
        //    {
        //        TwoVariationList = newVariationList.Where(s => s.Xmlb == category.Mxbh).ToList(); //筛选改值
        //        count = TwoVariationList.Count;
        //    }

        //    if (count == 0)                                                             //数量为零，
        //    {
        //        return;
        //    }
        //    else
        //    {
        //        RowDefinition row = new RowDefinition();
        //        row.MaxHeight = 100;                                                    //行高                            
        //        total.RowDefinitions.Add(row);

        //        TextBlock txt = new TextBlock();
        //        txt.Text = category.Name;
        //        txt.FontSize = 12;                                                  //字体大小

        //        total.Children.Add(txt);
        //        txt.SetValue(Grid.RowProperty, total.RowDefinitions.Count - 1);
        //        txt.SetValue(Grid.ColumnProperty, 0);
        //        txt.HorizontalAlignment = HorizontalAlignment.Right;                //靠右

        //        ScrollViewer scrollView = new ScrollViewer();
        //        scrollView.Height = 100;                                                //每大项最大高度
        //        Grid temp = new Grid();
        //        scrollView.Content = temp;                                               //加滚动条                                            

        //        total.Children.Add(scrollView);
        //        scrollView.SetValue(Grid.RowProperty, total.RowDefinitions.Count - 1);
        //        scrollView.SetValue(Grid.ColumnProperty, 1);

        //        int rowCount = 0;

        //        if (count % category.Cols == 0)
        //        {
        //            rowCount = count / category.Cols;
        //        }
        //        else
        //        {
        //            rowCount = count / category.Cols + 1;
        //        }

        //        int l = 0;
        //        for (int i = 0; i < rowCount; i++)
        //        {
        //            RowDefinition r = new RowDefinition();
        //            r.Height = new GridLength(30);                                                    //每条目高度
        //            temp.RowDefinitions.Add(r);
        //            for (int j = 0; j < category.Cols; j++)
        //            {
        //                ColumnDefinition c = new ColumnDefinition();
        //                if (category.Lb == 1 && l <= count - 1)
        //                {
        //                    c.Width = new GridLength(300);                                              //每条目宽度
        //                    temp.ColumnDefinitions.Add(c);

        //                    TextBlock t = new TextBlock();
        //                    t.Text = TwoOrderList[l].Yznr.ToString();
        //                    t.FontSize = 12;                                                      //医嘱字体大小
        //                    if (TwoOrderList[l].Yzzt == "3200")//新增
        //                    {
        //                        t.Foreground = ConvertColor.GetColorBrushFromHx16("000000");            //医嘱字体色
        //                    }
        //                    else if (TwoOrderList[l].Yzzt == "3201")//审核
        //                    {
        //                        t.Foreground = ConvertColor.GetColorBrushFromHx16("FFB90F");            //医嘱字体色
        //                    }
        //                    else if (TwoOrderList[l].Yzzt == "3202")//执行
        //                    {
        //                        t.Foreground = ConvertColor.GetColorBrushFromHx16("A2CD5A");            //医嘱字体色
        //                    }
        //                    else if (TwoOrderList[l].Yzzt == "3203")//取消
        //                    {
        //                       t.Foreground = ConvertColor.GetColorBrushFromHx16("FFC0CB");            //医嘱字体色
        //                    }
        //                    else if (TwoOrderList[l].Yzzt == "3204")//停止
        //                    {
        //                        t.Foreground = ConvertColor.GetColorBrushFromHx16("7F7F7F");            //医嘱字体色
        //                    }

        //                    temp.Children.Add(t);
        //                    t.SetValue(Grid.RowProperty, i);
        //                    t.SetValue(Grid.ColumnProperty, j);
        //                    temp.Background = ConvertColor.GetColorBrushFromHx16("ECF5FF");         //医嘱背景色
        //                }
        //                else if (category.Lb == 2 && l <= count - 1)
        //                {
        //                    c.Width = new GridLength(500);                                              //每条目宽度
        //                    temp.ColumnDefinitions.Add(c);

        //                    TextBlock t = new TextBlock();
        //                    t.Text = TwoVariationList[l].Bynr.ToString();
        //                    t.FontSize = 12;                                                      //变异字体大小
        //                    t.Foreground = ConvertColor.GetColorBrushFromHx16("0B333C");            //医嘱字体色
        //                    temp.Children.Add(t);
        //                    t.SetValue(Grid.RowProperty, i);
        //                    t.SetValue(Grid.ColumnProperty, j);
        //                    temp.Background = ConvertColor.GetColorBrushFromHx16("FFCC01");         //变异背景色
        //                }
        //                else
        //                {
        //                    continue;
        //                }

        //                l++;
        //            }
        //        }
        //    }
        //}

        #endregion
    }
}

