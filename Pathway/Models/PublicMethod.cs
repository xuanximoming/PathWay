using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using YidanEHRApplication.Views;
using Telerik.Windows.Controls;
using System.Collections.Generic;
using System.Text;
using System.IO;


using System.Runtime.InteropServices;
using YidanEHRApplication.Helpers;
using YidanSoft.Tool;
using YidanEHRApplication.DataService;
using YidanEHRApplication;
using System.Collections.ObjectModel;




namespace YidanEHRApplication.Models
{
    public partial class PublicMethod
    {

        /// <summary>
        /// client 实例
        /// </summary>
        public static YidanEHRDataServiceClient YidanClient
        {
            get
            {

                #region MyRegion

#if DEBUG
                YidanEHRDataServiceClient yidanClient = new YidanEHRDataServiceClient();
#else
                YidanEHRDataServiceClient yidanClient = ServiceHelper<YidanEHRDataServiceClient>.GetClientInstance(ServiceType.YidanEHRDataServicesvc);
#endif
                return yidanClient;
                #endregion
            }
        }
        ChildMessageBox messagebox = new ChildMessageBox();
        /// <summary>
        /// 提示框1
        /// </summary>
        /// <param name="title"></param>
        /// <param name="message"></param>
        public void AlterMessageBox(string title, string message)
        {
            YiDanMessageBox.Show(message, title);
            //messagebox.Title = title;
            //messagebox.MessageInfo.FontSize = 13;
            //messagebox.MessageInfo.Text = message;
            //messagebox.Show();
            //this.messagebox.Focus();
        }


        public static ChildMessageBox mesbox = new ChildMessageBox();
        /// <summary>
        /// 提示框2
        /// </summary>
        /// <param name="message">提示消息</param>
        /// <param name="header">标题名称</param>
        public static void RadAlterBox(string message, string header) //
        {
            YiDanMessageBox.Show(message, header);
            //DialogParameters parameters = new DialogParameters();
            //parameters.Header = header;
            ////parameters.Content = String.Format("提示: {0}", message);
            //parameters.Content = String.Format("{0}", message);
            //parameters.IconContent = null;
            //parameters.OkButtonContent = "确定";
            //RadWindow.Alert(parameters);

            ////focus();


        }

        /// <summary>
        /// 提示框3
        /// </summary>
        /// <param name="message">提示消息</param>
        /// <param name="header">标题名称</param>
        public static void RadAlterBoxRe(string message, string header, Control controlFocus) //
        {

            YiDanMessageBox.Show(message,controlFocus, header,YiDanMessageBoxButtons.Ok);
            //DialogParameters parameters = new DialogParameters();
            //parameters.Header = header;
            ////parameters.Content = String.Format("提示: {0}", message);
            //parameters.Content = String.Format("{0}", message);
            //parameters.IconContent = null;
            //parameters.OkButtonContent = "确定";
            ////if (controlFocus != null)
            ////{
            //parameters.Closed += (object sender, WindowClosedEventArgs e) => { controlFocus.Focus(); };
            ////}
            //RadWindow.Alert(parameters);
            //return parameters;
            ////focus();
        }

        public static void FocusChange(object sender, WindowClosedEventArgs e)
        {

        }

        #region  客户端异常
        /// <summary>
        /// 异常提示
        /// </summary>
        public static void RadWaringBox(System.Exception ex)
        {
            //            DialogParameters parameters = new DialogParameters();
            //            parameters.Header = "系统异常";
            ////#if DEBUG
            //            parameters.Content = String.Format("提示: {0}", ex.Message + "\r\n" + ex.StackTrace);
            ////#else  
            ////            parameters.Content = String.Format("提示: {0}", "系统异常，请联系管理员");
            ////#endif
            //            parameters.IconContent = null;
            //            parameters.OkButtonContent = "确定";
            //            RadWindow.Alert(parameters);
            RadWindow wndTemp = new RadWindow();
            PublicMethod.ShowAlertWindow(ref wndTemp, ex.Message, "提示Message", null, null);
            //PublicMethod.ShowAlertWindow(ref wndTemp, ex.StackTrace, "提示StackTrace", null, null);
            //PublicMethod.ShowAlertWindow(ref wndTemp, ex.InnerException.ToString(), "提示InnerException", null, null);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="ModelName"></param>
        public static void InsertClientLogException(System.Exception ex, string ModelName)
        {
            LoginException fault = new LoginException();
            fault.ErroMsg = ex.Message.Replace("'", "").Replace("\r\n", @"\n") + ex.StackTrace;
            fault.ModelName = ModelName;
            fault.CreateUser = Global.LogInEmployee.Zgdm.ToString();
            fault.HostName = string.Empty;
            // add by yxy
            //fault.Ip = App.Current.Resources["IpAddress"].ToString(); ;
            //fault.MacAddress = App.Current.Resources["MacAddress"].ToString();
#if DEBUG
            YidanEHRDataServiceClient client = new YidanEHRDataServiceClient();
#else
            YidanEHRDataServiceClient client = ServiceHelper<YidanEHRDataServiceClient>.GetClientInstance(ServiceType.YidanEHRDataServicesvc);
#endif
            client.InsertLogExceptionAsync(fault);
            client.CloseAsync();
        }
        #region FQW
        /// <summary>
        /// 记录异常，异常提示
        /// </summary>
        /// <param name="ex">异常实例</param>
        /// <param name="ModelName">异常模块名称</param>
        /// <param name="IsShowMessageBox">是否弹出异常提示</param>
        public static void ClientException(System.Exception ex, string ModelName, Boolean IsShowMessageBox)
        {
            //InsertClientLogException(ex, ModelName);
            if (IsShowMessageBox)
                RadWaringBox(ex);
        }
        #endregion
        #endregion

        /// <summary>
        /// 医嘱成组 用法，频次，药品项目类别判断  
        /// To do:如果自动成组则返回可以成组的List
        /// 此处算法需要重构
        /// </summary>
        /// <returns></returns>
        public static List<CP_AdviceGroupDetail> CheckCommonPropertiesIsSame(List<CP_AdviceGroupDetail> SelectAdviceItems)
        {
            List<CP_AdviceGroupDetail> list = new List<CP_AdviceGroupDetail>();
            for (int i = 0; i < SelectAdviceItems.Count; i++)
            {
                for (int j = i; j < SelectAdviceItems.Count; j++)
                {
                    if (SelectAdviceItems[i].Yfdm != SelectAdviceItems[j].Yfdm //用法判断
                        || SelectAdviceItems[i].Pcdm != SelectAdviceItems[j].Pcdm //频次判断
                    || SelectAdviceItems[i].Xmlb != SelectAdviceItems[j].Xmlb) //药品项目类别判断(西药，草药...)
                    {
                        return null;
                    }
                }
            }
            return SelectAdviceItems;

        }

        /// <summary>
        /// 医嘱成组 用法，频次，药品项目类别判断  
        /// To do:如果自动成组则返回可以成组的List
        /// 此处算法需要重构
        /// </summary>
        /// <returns></returns>
        public static List<CP_DoctorOrder> CheckCommonPropertiesIsSame(List<CP_DoctorOrder> SelectAdviceItems)
        {
            List<CP_DoctorOrder> list = new List<CP_DoctorOrder>();
            for (int i = 0; i < SelectAdviceItems.Count; i++)
            {
                for (int j = i; j < SelectAdviceItems.Count; j++)
                {
                    if (SelectAdviceItems[i].Yfdm != SelectAdviceItems[j].Yfdm //用法判断
                        || SelectAdviceItems[i].Pcdm != SelectAdviceItems[j].Pcdm //频次判断
                    || SelectAdviceItems[i].Xmlb != SelectAdviceItems[j].Xmlb) //药品项目类别判断(西药，草药...)
                    {
                        return null;
                    }
                }
            }
            return SelectAdviceItems;

        }

 
        #region
        public static void RadQueryBox(DialogParameters parameters, string strMessage, string strTitle)
        {
            // if (parameters == null)
            //      parameters = new DialogParameters();
            parameters.Content = String.Format("提示: {0}", strMessage);
            parameters.Header = strTitle;
            parameters.IconContent = null;
            parameters.OkButtonContent = "确定";
            parameters.CancelButtonContent = "取消";
            //parameters.Owner.Focus();
            parameters.Opened = RadQueryBoxOpened;

            RadWindow.Confirm(parameters);


        }
        static void RadQueryBoxOpened(Object o, EventArgs e)
        {
            ((RadWindow)o).BringToFront();
        }
        #endregion

        #region 弹出输入框
        /// <summary>
        /// 显示包含一个输入框的窗口
        /// </summary>
        /// <param name="w">窗口手柄</param>
        /// <param name="Title">窗口标题</param>
        /// <param name="TipString">提示信息</param>
        /// <param name="DefaultText">输入框默认值</param>
        /// <param name="buttonContent">按钮显示的名称</param>
        public void ShowInputWindow(ref RadWindow w, String Title, String TipString, String DefaultText, String buttonContent)
        {
            w.Style = (Style)Application.Current.Resources["RadWindowStyle"];

            Border b = new Border();
            b.Height = 1;
            b.BorderBrush = ConvertColor.GetColorBrushFromHx16("959595");
            b.BorderThickness = new Thickness(1);
            b.HorizontalAlignment = HorizontalAlignment.Stretch;
            b.Margin = new Thickness(0, 20, 0, 5);

            TextBlock txb = new TextBlock();
            if (TipString != null) txb.Text = TipString;
            txb.Margin = new Thickness(10, 5, 0, 0);

            TextBox txt = new TextBox();
            txt.Name = "txt";
            if (DefaultText == null) txt.Text = "医嘱套餐";
            else txt.Text = DefaultText;
            txt.Width = 150;
            txt.HorizontalAlignment = HorizontalAlignment.Left;
            txt.Margin = new Thickness(10, 0, 0, 0);


            RadButton btn = new RadButton();
            if (buttonContent == null) btn.Content = "保存";
            else btn.Content = buttonContent;
            btn.Focus();
            btn.CommandParameter = w;
            btn.Click += new RoutedEventHandler(btn_ShowInputWindowClick);
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.Width = 50;
            btn.Margin = new Thickness(10, 0, 10, 0);

            RadButton btnCancel = new RadButton();
            btnCancel.Content = "取消";

            btnCancel.CommandParameter = w;
            btnCancel.Click += new RoutedEventHandler(btnCancel_ShowInputWindowClick);
            btnCancel.HorizontalAlignment = HorizontalAlignment.Center;
            btnCancel.Width = 50;

            StackPanel spbtnContaim = new StackPanel();
            spbtnContaim.Orientation = Orientation.Horizontal;
            spbtnContaim.Children.Add(btnCancel);
            spbtnContaim.Children.Add(btn);
            spbtnContaim.FlowDirection = FlowDirection.RightToLeft;

            StackPanel sp = new StackPanel();
            sp.Width = 250;
            sp.Height = 100;
            sp.Children.Add(txb);
            sp.Children.Add(txt);
            sp.Children.Add(b);
            sp.Children.Add(spbtnContaim);



            w.Header = Title;
            w.Content = sp;
            w.ShowDialog();

        }
        void btn_ShowInputWindowClick(object sender, RoutedEventArgs e)
        {
            RadButton btn = (RadButton)sender;
            RadWindow w = (RadWindow)(btn.CommandParameter);
            StackPanel sp = (StackPanel)w.Content;
            w.PromptResult = ((TextBox)sp.FindName("txt")).Text;
            ((RadWindow)(btn.CommandParameter)).Close();
        }
        void btnCancel_ShowInputWindowClick(object sender, RoutedEventArgs e)
        {
            RadButton btn = (RadButton)sender;
            ((RadWindow)(btn.CommandParameter)).Close();
        }
        #endregion

        #region 弹出选择框
        public void ShowSelectWindow(ref RadWindow w, List<Pair> list)
        {
            w.Style = (Style)Application.Current.Resources["RadWindowStyle"];
            Border b = new Border();
            b.Height = 1;
            b.BorderBrush = ConvertColor.GetColorBrushFromHx16("959595");
            b.BorderThickness = new Thickness(1);
            b.HorizontalAlignment = HorizontalAlignment.Stretch;
            b.Margin = new Thickness(0, 20, 0, 5);

            RadButton btn = new RadButton();
            btn.Content = "确定";
            btn.Focus();

            btn.CommandParameter = w;
            btn.Click += new RoutedEventHandler(btn_ShowSelectWindowClick);
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.Width = 50;
            btn.Margin = new Thickness(10, 0, 10, 0);

            RadButton btnCancel = new RadButton();
            btnCancel.Content = "取消";
            btnCancel.CommandParameter = w;
            btnCancel.Click += new RoutedEventHandler(btnCancel_ShowSelectWindowClick);
            btnCancel.HorizontalAlignment = HorizontalAlignment.Center;
            btnCancel.Width = 50;

            StackPanel spbtnContaim = new StackPanel();
            spbtnContaim.Orientation = Orientation.Horizontal;
            spbtnContaim.Children.Add(btnCancel);
            spbtnContaim.Children.Add(btn);
            spbtnContaim.FlowDirection = FlowDirection.RightToLeft;

            StackPanel sp = new StackPanel();
            sp.Width = 250;
            sp.Height = 100;

            RadComboBox cbx = new RadComboBox();
            cbx.Margin = new Thickness(10, 10, 0, 0);
            cbx.Name = "cbx";
            cbx.ItemsSource = list;
            cbx.DisplayMemberPath = "Name";
            cbx.SelectedIndex = 0;
            TextBlock textBlock = new TextBlock();
            textBlock.Text = "请选择下一步结点:";
            textBlock.Margin = new Thickness(5, 5, 0, 5);
            textBlock.FontFamily = new FontFamily("Bold");
            sp.Children.Add(textBlock);
            sp.Children.Add(cbx);
            sp.Children.Add(b);
            sp.Children.Add(spbtnContaim);

            //w.Header = "添加医嘱套餐";
            w.Content = sp;
            w.ShowDialog();

        }
        void btn_ShowSelectWindowClick(object sender, RoutedEventArgs e)
        {
            RadButton btn = (RadButton)sender;
            RadWindow w = (RadWindow)(btn.CommandParameter);
            StackPanel sp = (StackPanel)w.Content;
            w.PromptResult = ((Pair)((RadComboBox)sp.FindName("cbx")).SelectedItem).ID;
            ((RadWindow)(btn.CommandParameter)).Close();
        }
        void btnCancel_ShowSelectWindowClick(object sender, RoutedEventArgs e)
        {
            RadButton btn = (RadButton)sender;
            ((RadWindow)(btn.CommandParameter)).Close();
        }
        public class Pair
        {
            public Pair(String id, String name)
            {
                ID = id;
                Name = name;
            }
            String _ID;

            public String ID
            {
                get { return _ID; }
                set { _ID = value; }
            }
            String _Name;

            public String Name
            {
                get { return _Name; }
                set { _Name = value; }
            }
        }
        #endregion

        #region 弹出自定义的Confirm
        public static void ShowConfirmWindow(ref RadWindow w, String ContentString, String HeardString, RoutedEventHandler okEvent, RoutedEventHandler cancleEvent)
        {
            YiDanMessageBox.Show(ContentString, HeardString);
            //w.Style = (Style)Application.Current.Resources["RadWindowStyle"];
            //Border b = new Border();
            //b.Height = 1;
            //b.BorderBrush = ConvertColor.GetColorBrushFromHx16("959595");
            //b.BorderThickness = new Thickness(1);
            //b.HorizontalAlignment = HorizontalAlignment.Stretch;
            //b.Margin = new Thickness(0, 5, 0, 5);

            //RadButton btn = new RadButton();
            //btn.Focus();
            //btn.Content = "确定";
            //btn.Name = "ok";
            //btn.CommandParameter = w;
            //btn.Click += new RoutedEventHandler(CloseShowConfirmWindow);
            //btn.Click += new RoutedEventHandler(okEvent);
            //btn.HorizontalAlignment = HorizontalAlignment.Center;
            //btn.Width = 50;
            //btn.Margin = new Thickness(10, 0, 10, 0);
            //btn.Tag = w;
            //RadButton btnCancel = new RadButton();
            //btnCancel.Content = "取消";
            //btnCancel.CommandParameter = w;
            //btnCancel.Click += new RoutedEventHandler(CloseShowConfirmWindow);
            //btnCancel.Click += new RoutedEventHandler(cancleEvent);
            //btnCancel.HorizontalAlignment = HorizontalAlignment.Center;
            //btnCancel.Width = 50;
            //btnCancel.Name = "cancle";
            //btnCancel.Tag = w;
            //StackPanel spbtnContaim = new StackPanel();
            //spbtnContaim.Orientation = Orientation.Horizontal;
            //spbtnContaim.Children.Add(btnCancel);
            //spbtnContaim.Children.Add(btn);
            //spbtnContaim.FlowDirection = FlowDirection.RightToLeft;
            //spbtnContaim.Margin = new Thickness(0, 10, 20, 0);

            //StackPanel sp = new StackPanel();
            //sp.Width = 400;
            //sp.Height = 200;
            //TextBox textBlock = new TextBox();

            //textBlock.Text = ContentString;
            //textBlock.Height = 155;
            //textBlock.Width = 380;
            //textBlock.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            //textBlock.IsReadOnly = true;
            //textBlock.Margin = new Thickness(5, 5, 0, 0);
            //textBlock.FontFamily = new FontFamily("Bold");
            //sp.Children.Add(textBlock);

            ////sp.Children.Add(b);
            //sp.Children.Add(spbtnContaim);

            //Grid grid = new Grid();
            //grid.Children.Add(sp);

            //w.Header = HeardString;
            //w.Content = grid;
            //w.ShowDialog();
            //w.BringToFront();

        }

        public static void ShowAlertWindow(ref RadWindow w, String ContentString, String HeardString, RoutedEventHandler okEvent, RoutedEventHandler cancleEvent)
        {
            YiDanMessageBox.Show(ContentString, HeardString);
            //w.Style = (Style)Application.Current.Resources["RadWindowStyle"];
            //Border b = new Border();
            //b.Height = 1;
            //b.BorderBrush = ConvertColor.GetColorBrushFromHx16("959595");
            //b.BorderThickness = new Thickness(1);
            //b.HorizontalAlignment = HorizontalAlignment.Stretch;
            //b.Margin = new Thickness(0, 5, 0, 5);

            //RadButton btn = new RadButton();
            //btn.Content = "确定";
            //btn.Focus();
            //btn.Name = "ok";
            //btn.CommandParameter = w;
            //btn.Click += new RoutedEventHandler(CloseShowConfirmWindow);
            //if (okEvent != null)
            //    btn.Click += new RoutedEventHandler(okEvent);
            //btn.HorizontalAlignment = HorizontalAlignment.Center;
            //btn.Width = 50;
            //btn.Margin = new Thickness(10, 0, 10, 0);
            //btn.Tag = w;
            ////RadButton btnCancel = new RadButton();
            ////btnCancel.Content = "取消";
            ////btnCancel.CommandParameter = w;
            ////btnCancel.Click += new RoutedEventHandler(CloseShowConfirmWindow);
            ////btnCancel.Click += new RoutedEventHandler(cancleEvent);
            ////btnCancel.HorizontalAlignment = HorizontalAlignment.Center;
            ////btnCancel.Width = 50;
            ////btnCancel.Name = "cancle";
            ////btnCancel.Tag = w;
            //StackPanel spbtnContaim = new StackPanel();
            //spbtnContaim.Orientation = Orientation.Horizontal;
            ////spbtnContaim.Children.Add(btnCancel);
            //spbtnContaim.Children.Add(btn);
            //spbtnContaim.FlowDirection = FlowDirection.RightToLeft;

            //StackPanel sp = new StackPanel();
            //sp.Width = 400;
            //sp.Height = 200;
            //TextBox textBlock = new TextBox();

            //textBlock.Text = ContentString;
            //textBlock.Height = 155;
            //textBlock.Width = 380;
            //textBlock.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            //textBlock.IsReadOnly = true;
            //textBlock.Margin = new Thickness(5, 5, 0, 0);
            //textBlock.FontFamily = new FontFamily("Bold");
            //sp.Children.Add(textBlock);

            //sp.Children.Add(b);
            //sp.Children.Add(spbtnContaim);

            //Grid grid = new Grid();
            //grid.Children.Add(sp);

            //w.Header = HeardString;
            //w.Content = grid;
            //w.ShowDialog();
            //w.BringToFront();

        }

        public static void ShowWarmWindow(ref RadWindow w, String ContentString, String HeardString, RoutedEventHandler okEvent, RoutedEventHandler cancleEvent)
        {
            YiDanMessageBox.Show(ContentString, HeardString);
            //w.Style = (Style)Application.Current.Resources["RadWindowStyle"];
            //Border b = new Border();
            //b.Height = 1;
            //b.BorderBrush = ConvertColor.GetColorBrushFromHx16("959595");
            //b.BorderThickness = new Thickness(1);
            //b.HorizontalAlignment = HorizontalAlignment.Stretch;
            //b.Margin = new Thickness(0, 5, 0, 5);

            //RadButton btn = new RadButton();
            //btn.Focus();
            //btn.Content = "确定";
            //btn.Name = "ok";
            //btn.CommandParameter = w;
            //btn.Click += new RoutedEventHandler(CloseShowConfirmWindow);
            //if (okEvent != null)
            //    btn.Click += new RoutedEventHandler(okEvent);
            //btn.HorizontalAlignment = HorizontalAlignment.Center;
            //btn.Width = 50;
            //btn.Margin = new Thickness(10, 0, 10, 0);
            //btn.Tag = w;
            ////RadButton btnCancel = new RadButton();
            ////btnCancel.Content = "取消";
            ////btnCancel.CommandParameter = w;
            ////btnCancel.Click += new RoutedEventHandler(CloseShowConfirmWindow);
            ////btnCancel.Click += new RoutedEventHandler(cancleEvent);
            ////btnCancel.HorizontalAlignment = HorizontalAlignment.Center;
            ////btnCancel.Width = 50;
            ////btnCancel.Name = "cancle";
            ////btnCancel.Tag = w;
            //StackPanel spbtnContaim = new StackPanel();
            //spbtnContaim.Orientation = Orientation.Horizontal;
            ////spbtnContaim.Children.Add(btnCancel);
            //spbtnContaim.Children.Add(btn);
            //spbtnContaim.FlowDirection = FlowDirection.RightToLeft;

            //StackPanel sp = new StackPanel();
            //sp.Width = 400;
            //sp.Height = 200;
            //TextBox textBlock = new TextBox();

            //textBlock.Text = ContentString;
            //textBlock.Height = 155;
            //textBlock.Width = 380;
            //textBlock.VerticalScrollBarVisibility = ScrollBarVisibility.Auto;
            //textBlock.IsReadOnly = true;
            //textBlock.Margin = new Thickness(5, 5, 0, 0);
            //textBlock.FontFamily = new FontFamily("Bold");
            //sp.Children.Add(textBlock);

            //sp.Children.Add(b);
            //sp.Children.Add(spbtnContaim);

            //Grid grid = new Grid();
            //grid.Children.Add(sp);

            //w.Header = HeardString;
            //w.Content = grid;
            //w.ShowDialog();
            //w.BringToFront();

        }
        static void CloseShowConfirmWindow(object sender, RoutedEventArgs e)
        {
            ((RadWindow)((RadButton)sender).Tag).Close();
        }
        #endregion
        #region 弹出自定义Alert

        #endregion


    }
}
