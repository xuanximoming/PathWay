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
using Telerik.Windows.Controls;
using System.IO;
using System.Windows.Media.Imaging;
using YidanEHRApplication.Views;
using Telerik.Windows.Controls.GridView;
using System.Windows.Data;
using System.Collections.ObjectModel;
using System.Windows.Resources;
using System.Reflection;
using YidanEHRApplication.Models;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Controls;
using System.Text;
using YidanEHRApplication.DataService;
using YidanEHRApplication;

namespace YidanEHRApplication.Views
{
    public partial class InpatientList : Page
    {
        // List<CP_InpatinetList> m_InpatientList = null;

        public InpatientList()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(InpatientList_Loaded);
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        void InpatientList_Loaded(object sender, RoutedEventArgs e)
        {
            //try{
            this.InpatientListControl1.HideControl(true);
            this.InpatientListControl1.DoctorID = "";
            this.InpatientListControl1.AfterNavigateToPage += new Controls.UCInpatientListControl.NavigateToPage(InpatientListControl1_AfterNavigateToPage);
            this.InpatientListControl1.InitPage();//初始化数据  
            this.InpatientListControl1.PathZx.Visibility = Visibility;
      
            #region 非界面登录中使用
            if (!String.IsNullOrWhiteSpace(Global.Syxh))
            {
                if (Global.InpatientListCurrent != null && Global.InpatientListCurrent.Ljdm.Trim() != "")
                {
                    if (Global.LogInEmployee != null)
                        Global.InpatientListCurrent.CurOper = Global.LogInEmployee.Zgdm;

                    //this.NavigationService.Navigate(new Uri("/Views/PathEnForce.xaml", UriKind.Relative));
                    AddTabItem("路径执行", "/Views/PathEnForceZS.xaml", true);
                }
                else
                {

                    this.InpatientListControl1.rabInPath2_MockClick();

                }
            }

            #endregion
            //    }
            //catch (Exception ex)
            //{
            //   YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            //}
        }
        /// <summary>
        /// 调用自定义控件的委托事件，跳转到相对应的页面
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InpatientListControl1_AfterNavigateToPage(object sender, RoutedEventArgs e)
        {
            UCInpatientListControl.OpreateEventArgs Args = (UCInpatientListControl.OpreateEventArgs)e;
            if (Args.OpreateType == true)//页面导航
            {
                //if (Global.isNavigate)
                #region add by luff 20130807 根据配置表判断进入哪个路径执行界面 0或空值进入第三方控件路径执行页面，1则进入微软控件路径执行页面
                try
                {
                    
                    List<APPCFG> t_listApp = Global.mAppCfg.Select(s => s).Where(s => s.Configkey.IndexOf("PathEnter") > -1).ToList();
                    if (t_listApp.Count > 0)
                    {
                        if (t_listApp[0].Value == "1")//表示进入微软控件路径执行页面（钟山医院）
                        {
                            this.NavigationService.Navigate(new Uri("/Views/PathEnForceZS.xaml", UriKind.Relative));
                        }
                        else
                        {
                            this.NavigationService.Navigate(new Uri("/Views/PathEnForce.xaml", UriKind.Relative));

                        }
                    }
                    else
                    {

                        this.NavigationService.Navigate(new Uri("/Views/PathEnForce.xaml", UriKind.Relative));
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }
                
                #endregion
                //else
                //    AddTabItem("路径执行【" + Global.InpatientListCurrent.Hzxm + "】", "/Views/PathEnForce.xaml",true);
            }
            else//数据捆绑
            {
                //patientBasicInfo1.CurrentPat = (CP_InpatinetList)Args.Datas;
            }
        }

        /// <summary>
        /// InpatientListControl1控件的Unloaded事件把已经弹出来的窗口关闭（在非界面登录时用到）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void InpatientListControl1_Unloaded(object sender, RoutedEventArgs e)
        {
            //if (InpatientListControl1.accessWindow != null)
            //    InpatientListControl1.accessWindow.Close();
        }
        public void AddTabItem(String tabName, String uriStr, Boolean isShowDialog)
        {

            if (isShowDialog)
            {
                #region 弹出窗口
                RadWindow wid = new RadWindow();
                System.Reflection.Assembly assTemp = System.Reflection.Assembly.GetExecutingAssembly();
                StringBuilder InstanceStrTemp = new StringBuilder();
                InstanceStrTemp.Append("YidanEHRApplication");
                uriStr = uriStr.Substring(0, uriStr.IndexOf('.'));
                if (uriStr.IndexOf('/') == 0)
                    uriStr = uriStr.Replace('/', '.');
                else
                    uriStr = ("/" + uriStr).Replace('/', '.');
                InstanceStrTemp.Append(uriStr);
                //     /Views/UserCenterManager.xaml
                Object objTemp = assTemp.CreateInstance(InstanceStrTemp.ToString());//必须使用名称空间+类名称
                wid.Content = (UIElement)objTemp;
                wid.Header = "路径执行";
                wid.WindowState = WindowState.Maximized;
                wid.ResizeMode = ResizeMode.NoResize;
                wid.ShowDialog();
                wid.Closed += new EventHandler<WindowClosedEventArgs>(wid_Closed);

                #endregion
            }


            else
            {
                #region 非弹出窗口
                if (IsExistItem(tabName)) return;
                // InpatientListControl1.InitPage();
                #region head
                RadTabItem item = new RadTabItem();
                Grid grd = new Grid();
                ColumnDefinition columen1 = new ColumnDefinition();
                ColumnDefinition columen2 = new ColumnDefinition();
                columen1.Width = new GridLength(tabName.Length * 12);
                columen2.Width = new GridLength(20);
                grd.ColumnDefinitions.Add(columen1);
                grd.ColumnDefinitions.Add(columen2);
                RadButton btn = new RadButton();
                TextBlock txt = new TextBlock();
                txt.SetValue(Grid.ColumnProperty, 0);
                txt.Text = tabName;
                btn.Width = 15;
                btn.Height = 15;
                btn.Content = "×";
                btn.BorderThickness = new Thickness(0);
                btn.FontWeight = System.Windows.FontWeights.Bold;
                btn.SetValue(Grid.ColumnProperty, 1);
                btn.SetValue(RadButton.CornerRadiusProperty, new CornerRadius(30, 30, 30, 30));
                btn.Tag = item;
                btn.Style = (Style)App.Current.Resources["HiddenButtonBackGround"];
                btn.MouseEnter += new MouseEventHandler(btn_MouseEnter);
                btn.MouseLeave += new MouseEventHandler(btn_MouseLeave);
                btn.Click += new RoutedEventHandler(btn_Click);
                grd.Children.Add(btn);
                grd.Children.Add(txt);
                item.Header = grd;
                item.Tag = tabName;
                item.Height = 28;
                #endregion
                #region Content
                System.Reflection.Assembly ass = System.Reflection.Assembly.GetExecutingAssembly();
                StringBuilder InstanceStr = new StringBuilder();
                InstanceStr.Append("YidanEHRApplication");
                uriStr = uriStr.Substring(0, uriStr.IndexOf('.'));
                if (uriStr.IndexOf('/') == 0)
                    uriStr = uriStr.Replace('/', '.');
                else
                    uriStr = ("/" + uriStr).Replace('/', '.');
                InstanceStr.Append(uriStr);
                //     /Views/UserCenterManager.xaml
                Object obj = ass.CreateInstance(InstanceStr.ToString());//必须使用名称空间+类名称
                item.Content = (UIElement)obj;
                #endregion
                item.Width = tabName.Length * 12 + 33;
                Global.tbc.Items.Add(item);
                Global.tbc.SelectedItem = item;
                //App.Current.tb
                //Global.tbc = tbc;
                #endregion
            }

        }

        void wid_Closed(object sender, WindowClosedEventArgs e)
        {
            if (Global.InpatientListCurrent == null) return;
            if (InpatientListControl1.radGridViewInpatient.ItemsSource == null) return;
            foreach (var item in this.InpatientListControl1.radGridViewInpatient.ItemsSource)
            {
                CP_InpatinetList cp = (CP_InpatinetList)item;
                if (cp.Syxh == Global.InpatientListCurrent.Syxh)
                {
                    cp.EnForceWorkFlowXml = Global.InpatientListCurrent.EnForceWorkFlowXml;
                }
            }
        }
        void btn_MouseLeave(object sender, MouseEventArgs e)
        {
            //RadButton btn = (RadButton)sender;
            //btn.Style = (Style)App.Current.Resources["HiddenButtonBackGround"];
        }

        void btn_MouseEnter(object sender, MouseEventArgs e)
        {
            //RadButton btn = (RadButton)sender;
            //btn.Style = new Style(typeof(RadButton));
        }
        void btn_Click(object sender, RoutedEventArgs e)
        {

            Global.tbc.Items.Remove((RadTabItem)((Button)sender).Tag);

        }
        public Boolean IsExistItem(String tabName)
        {
            Boolean flag = false;
            foreach (RadTabItem item in Global.tbc.Items)
            {
                if (item.Tag.ToString() == tabName)
                {
                    Global.tbc.SelectedItem = item;
                    flag = true;
                }
            }
            return flag;
        }

        /// <summary>
        /// 根据配置信息判断是否显示草药处方医嘱
        /// </summary>
        private void CheckEnPath()
        {
            

        }


    }

     
    #region 图片相关与XAML配合使用 暂不使用
    public class GridImageConvert : IValueConverter
    {
        public GridImageConvert()
        {
        }
        public object Convert(object value, Type targetType,
         object parameter,
            System.Globalization.CultureInfo culture)
        {
            // value is the data from the source object.
            string order = value as string;
            SolidColorBrush brush = new SolidColorBrush(Colors.Black);
            BitmapImage imgsrc = null;


            AssemblyName assemblyName = new AssemblyName(typeof(GridImageConvert).Assembly.FullName);
            if (order == "男")
            {
                string resource = "Images/InpatientSexMan.png";
                string resourcePath = "/" + assemblyName.Name + ";component/" + resource;
                Uri resourceUri = new Uri(resourcePath, UriKind.Relative);
                imgsrc = new BitmapImage(resourceUri);
            }
            else if (order == "女")
            {
                string resource = "Images/InpatientSexWoman.png";
                string resourcePath = "/" + assemblyName.Name + ";component/" + resource;
                Uri resourceUri = new Uri(resourcePath, UriKind.Relative);
                imgsrc = new BitmapImage(resourceUri);
            }
            return imgsrc;

        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
    #endregion


}
