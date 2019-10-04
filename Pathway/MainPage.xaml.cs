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
using YidanEHRApplication.Models;
using YidanEHRApplication.DataService;
using YidanEHRApplication;
using Telerik.Windows.Controls;
using YidanEHRApplication.Views.Login;
using System.Windows.Browser;
using System.Windows.Media.Imaging;

namespace YidanEHRApplication
{
    public partial class MainPage : UserControl
    {


        string tempusername = string.Empty;
        string temppassword = string.Empty;
        string sStat = string.Empty;
        private const string labinfo = "  欢迎您: {0} 工号：{1}   科室：{2}  病区：{3}";
        /// <summary>
        /// 页面加载事件
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //默认左边菜单隐藏
                ExpTb_Collapsed(sender, e);

            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex,this.GetType().FullName);
            }
        }

        public MainPage()
        {
            try
            {
                InitializeComponent();
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex,this.GetType().FullName);
            }
        }

        public MainPage(List<V_PermissionListFather> v)
        {
            try
            {
                //StyleManager.ApplicationTheme = new Telerik.Windows.Controls.Office_BlueTheme();
                InitializeComponent();
                BindLogInfo();
                Loaded += new RoutedEventHandler(Page_Loaded);
                BindMenu(v);
                radMenu2.Items.Clear();
                //BindMenu2(v);
                GetStartPage();
            }
            catch (Exception ex)
            {

                YiDanMessageBox.Show(ex,this.GetType().FullName);
            }

        }

        /// <summary>
        /// 绑定菜单
        /// </summary>
        /// <param name="v">一级菜单列表</param>
        void BindMenu(List<V_PermissionListFather> v)
        {
            try
            {
                foreach (V_PermissionListFather pf in v)
                {
                    if (pf.FunFatherName != "路径裁剪")
                    {
                        RadMenuItem m = new RadMenuItem();

                        m.Name = pf.FunCodeFather;
                        m.Header = pf.FunFatherName;
                        m.Tag = pf.FunURLFather;
                        m.Click += new Telerik.Windows.RadRoutedEventHandler(m_Click);
                        radMenu1.Items.Add(m);
                        foreach (V_PermissionList p in pf.pList)
                        {
                            if (pf.pList.Count == 1)//某个一级菜单节点只包含一个二级菜单，就把二级菜单提升为一级菜单
                            {
                                m.Name = p.FunCode;
                                m.Header = p.FunName;
                                m.Tag = p.FunURL;
                                continue;
                            }
                            //if (p.FunCodeFather.Trim() == "07" || p.FunCodeFather.Trim() == "15")
                            //{
                            //    break;
                            //}
                            if (p.FunCodeFather.Trim() != "")//当某个二级菜单FunCodeFather不为空就跳出循环，目的是不让显示下拉菜单
                            {
                                break;
                            }
                            RadMenuItem mc = new RadMenuItem();
                            mc.Name = p.FunCode;
                            mc.Header = p.FunName;
                            mc.Tag = p.FunURL;
                            mc.Click += new Telerik.Windows.RadRoutedEventHandler(m_Click);
                            m.Items.Add(mc);
                        }
                    }

                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// 绑定二级菜单列表
        /// </summary>
        /// <param name="v">二级菜单列表</param>
        /// <param name="sFname">父菜单第一个二级菜单</param>
        private string BindMenu2(List<V_PermissionListFather> v, string sFname)
        {
            try
            {
                string iVal = string.Empty;
                foreach (V_PermissionListFather pf in v)
                {
                    if (pf.FunCodeFather.Trim() == sFname.Trim())
                    {

                        RadMenuItem m;
                        foreach (V_PermissionList p in pf.pList)
                        {
                            m = new RadMenuItem();
                            if (pf.pList.Count > 1)//某个一级菜单节点包含多个二级菜单
                            {
                                m.Name = p.FunCode;
                                m.Header = p.FunName;
                                m.Tag = p.FunURL;
                                m.Click += new Telerik.Windows.RadRoutedEventHandler(m2_Click);

                                radMenu2.Items.Add(m);
                                iVal = pf.pList[0].FunURL.Trim();
                                //iVal=iVal + 1;

                            }
                            else
                            {
                                //iVal = 0;
                                iVal = string.Empty;

                            }

                        }
                    }

                }
                return iVal;
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void BindLogInfo()
        {
            try
            {
                //当前用户有可能不存成病区或科室的情况
                string dept = String.IsNullOrEmpty(Global.LogInEmployee.Ksdm) ? "" : Global.LogInEmployee.Department.Name;
                string ward = string.IsNullOrEmpty(Global.LogInEmployee.Bqdm) ? "" : Global.LogInEmployee.Ward.Name;

                LoginInfo.Text = string.Format(labinfo, Global.LogInEmployee.Name, Global.LogInEmployee.Zgdm, dept, ward);
            }
            catch (Exception e)
            {

                throw e;
            }

        }


        private void GetStartPage()
        {
            try
            {
                //todo 根据角色启动不同的页面
                //ContentFrame.Refresh();
                ContentFrame.Navigate(new Uri("/Views/InpatientList.xaml", UriKind.Relative));

                foreach (RadMenuItem rad in radMenu1.Items)
                {
                    if (rad.Tag.ToString() == "/Views/InpatientList.xaml")
                    {
                        rad.Background = new SolidColorBrush(Colors.White);
                        rad.Foreground = new SolidColorBrush(Colors.Black);
                        return;
                    }
                }
            }
            catch (Exception e)
            {

                throw e;
            }

        }


        public void NavigateView(string url)
        {
            //ContentFrame.Navigate(new Uri(url, UriKind.Relative));
        }

        /// <summary>
        /// 横向父菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                foreach (RadMenuItem rad in radMenu1.Items)
                {
                    rad.Background = new SolidColorBrush(Colors.Transparent);
                    rad.Foreground = new SolidColorBrush(Colors.White);
                }



                RadMenuItem item = sender as RadMenuItem;
                //if (item == null || item.Tag == null || item.Tag.ToString().Trim() == "")
                if (item == null)
                {
                    return;
                }
                if (item.Tag.ToString().Trim() == "")//有子菜单的，就生成子类菜单
                {
                    radMenu2.Items.Clear();
                    YidanEHRDataServiceClient serviceCon;

                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.GetV_PermissionListFatherCompleted +=
                      (obj, ea) =>
                      {
                          if (ea.Error == null)
                          {
                              List<V_PermissionListFather> V_PermissionListFather;
                              V_PermissionListFather = ea.Result.ToList();
                              string sTemp = BindMenu2(V_PermissionListFather, item.Name.ToString());
                              if (sTemp != "")
                              {
                                  this.radMenu2.Visibility = Visibility.Visible;
                                  this.ExpTb.Visibility = Visibility.Visible;

                                  if (radMenu2.Items == null)
                                  {
                                      return;
                                  }

                                  //设置第一个子菜单的属性
                                  RadMenuItem rmenu = (RadMenuItem)radMenu2.Items[0];
                                  rmenu.Tag = sTemp.Trim();
                                  rmenu.Background = new SolidColorBrush(Colors.White);
                                  rmenu.Foreground = new SolidColorBrush(Colors.Black);
                                  rmenu.Margin = new Thickness(0, 20, 0, 0);
                                  item.Background = new SolidColorBrush(Color.FromArgb(239, 247, 255, 255));
                                  item.Foreground = new SolidColorBrush(Colors.Black);
                                  //默认跳转第一个子菜单页面
                                  this.ContentFrame.Source = new Uri(sTemp.Trim(), UriKind.Relative);
                              }
                              else
                              {
                                  this.radMenu2.Visibility = Visibility.Collapsed;
                                  this.ExpTb.Visibility = Visibility.Collapsed;
                              }

                          }

                      };
                    serviceCon.GetV_PermissionListFatherAsync(Global.LogInEmployee.Zgdm);
                    serviceCon.CloseAsync();

                }
                else
                {

                    item.Background = new SolidColorBrush(Color.FromArgb(239, 247, 255, 255));
                    item.Foreground = new SolidColorBrush(Colors.Black);
                    this.radMenu2.Visibility = Visibility.Collapsed;
                    this.ExpTb.Visibility = Visibility.Collapsed;
                    ContentFrame.Refresh();
                    ContentFrame.Navigate(new Uri("/" + item.Tag.ToString(), UriKind.Relative));
                }
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex,this.GetType().FullName);
            }


        }

        /// <summary>
        /// 纵向子菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m2_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                foreach (RadMenuItem rad in radMenu2.Items)
                {
                    rad.Background = new SolidColorBrush(Colors.Transparent);
                    //rad.Foreground = new SolidColorBrush(Colors.White);
                }



                RadMenuItem item = sender as RadMenuItem;
                if (item == null || item.Tag == null || item.Tag.ToString().Trim() == "")
                { return; }


                item.Background = new SolidColorBrush(Color.FromArgb(239, 247, 255, 255));
                item.Foreground = new SolidColorBrush(Colors.Black);

                ContentFrame.Refresh();
                ContentFrame.Navigate(new Uri(item.Tag.ToString(), UriKind.Relative));
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex,this.GetType().FullName);
            }


        }

        /// <summary>
        /// After the Frame navigates, ensure the <see cref="HyperlinkButton"/> representing the current page is selected
        /// </summary>
        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {
        }

        /// <summary>
        /// If an error occurs during navigation, show an error window
        /// </summary>
        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            try
            {
                e.Handled = true;
                PublicMethod.ClientException(e.Exception, ((System.Windows.Navigation.NavigationService)(sender)).CurrentSource.OriginalString, true);
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex,this.GetType().FullName);
            }
        }

        private void Link2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                YidanPathWayMessageBox mess = new YidanPathWayMessageBox("确认退出临床路径系统吗？", "退出提示", YiDanMessageBoxButtons.YesNo);
                mess.ShowDialog();
                mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);

            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex,this.GetType().FullName);
            }
        }

        void mess_PageClosedEvent(object sender, bool e)
        {
            try
            {
                if (e == true)
                {

                    try
                    {
                        this.Content = new LogInPage();

                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }





        //#region 
        ///// <summary>
        ///// 绑定菜单
        ///// </summary>
        ///// <param name="v">一级菜单列表</param>
        //void BindMenu(List<V_PermissionListFather> v)
        //{
        //    foreach (V_PermissionListFather pf in v)
        //    {
        //        MenuBarItem item = new MenuBarItem();
        //        item.Name = pf.FunCodeFather;
        //        item.MenuText = pf.FunFatherName;
        //        item.Tag = pf.FunURLFather;
        //        menubar.items.Add(item);
        //        foreach (V_PermissionList p in pf.pList)
        //        {
        //            MenuControl.MenuItem mc = new MenuControl.MenuItem();

        //            mc.Name = p.FunCode;
        //            mc.MenuText = p.FunName;
        //            mc.Tag = p.FunURL;
        //            mc.Click += new RoutedEventHandler(mc_Click);
        //            item.items.Add(mc);
        //        }

        //    }

        //}

        //void mc_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        MenuControl.MenuItem item = sender as MenuControl.MenuItem;
        //        if (item == null || item.Tag == null || item.Tag.ToString().Trim() == "") return;

        //        frame1.Refresh();
        //        frame1.Navigate(new Uri("/" + item.Tag.ToString(), UriKind.Relative));
        //    }
        //    catch (Exception ex)
        //    {
        //        PublicMethod.ClientException(ex, this.GetType().FullName, true);
        //    }
        //}





        //#endregion
        /// <summary>
        /// 隐藏左边菜单事件
        /// </summary>
        private void ExpTb_Collapsed(object sender, RoutedEventArgs e)
        {
            try
            {
                ExpTb.Header = "";
                radMenu2.Visibility = System.Windows.Visibility.Collapsed;
                //this.ExpTb.ExpandDirection = System.Windows.Controls.ExpandDirection.Left;
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex,this.GetType().FullName);
            }

        }

        /// <summary>
        /// 显示左边菜单事件
        /// </summary>
        private void ExpTb_Expanded(object sender, RoutedEventArgs e)
        {
            try
            {
                ExpTb.Header = "";

                radMenu2.Visibility = System.Windows.Visibility.Visible;
                //this.ExpTb.ExpandDirection = System.Windows.Controls.ExpandDirection.Right;
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex,this.GetType().FullName);
            }

        }

        //#region yxy 修改密码

        private void LinkChangePwd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //ChildWindow changepwd = new ChildWindow();

                RWChangePassword changepwd = new RWChangePassword();
                changepwd.Width = 300;
                changepwd.Height = 200;
                changepwd.ShowDialog();
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex,this.GetType().FullName);
            }

        }

        //#endregion

        private void LayoutRoot_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }



        private void Link_help_Click(object sender, RoutedEventArgs e)
        {
            HtmlPage.Window.Navigate(new Uri("../Help/临床路径使用手册.html", UriKind.RelativeOrAbsolute), "_blank");
        }



        private void Grid_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {

            if (this.radMenu2.Visibility == Visibility.Visible)
            {
                //隐藏左边菜单
                ExpTb_Collapsed(sender, e);
                this.ExpTb.ExpandDirection = System.Windows.Controls.ExpandDirection.Right;
            }
            else
            {
                //显示左边菜单
                ExpTb_Expanded(sender, e);
                this.ExpTb.ExpandDirection = System.Windows.Controls.ExpandDirection.Left;
            }
        }
    }
}
