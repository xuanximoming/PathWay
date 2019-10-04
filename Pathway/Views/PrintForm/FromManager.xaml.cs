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
using YidanSoft.Tool;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using YidanEHRApplication.DataService;
using YidanEHRApplication;

namespace YidanEHRApplication.Views.ReportForms
{
    public partial class FromManager : Page
    {
        // add by luff 20121012
        List<V_PermissionListFather> v_prlistF = new List<V_PermissionListFather>();
        #region 事件
        /// <summary>
        /// 页面加载事件
        /// </summary>
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                GetListinfo();

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion

        public FromManager()
        {
            InitializeComponent();
        }

        /// <summary>
        /// 绑定菜单
        /// </summary>
        /// <param name="v">二级菜单列表</param>
        void BindMenu(List<V_PermissionListFather> v)
        {
            foreach (V_PermissionListFather pf in v)
            {
                if (pf.FunCodeFather.Trim() == "07")
                {

                    RadMenuItem m;
                    foreach (V_PermissionList p in pf.pList)
                    {
                        m = new RadMenuItem();
                        if (pf.pList.Count > 1)//某个一级菜单节点包含多个二级菜单，并且父类code为07的子菜单
                        {
                            m.Name = p.FunCode;
                            m.Header = p.FunName;
                            m.Tag = p.FunURL;
                            m.Click += new Telerik.Windows.RadRoutedEventHandler(m_Click);
                            
                            radMenu.Items.Add(m);
                        }
         
                    }
                }

            }
        }


        /// <summary>
        /// 菜单点击事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void m_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                foreach (RadMenuItem rad in radMenu.Items)
                {
                    rad.Background = new SolidColorBrush(Colors.Transparent);
                    //rad.Foreground = new SolidColorBrush(Colors.White);
                }



                RadMenuItem item = sender as RadMenuItem;
                if (item == null || item.Tag == null || item.Tag.ToString().Trim() == "") return;

                item.Background = new SolidColorBrush(Color.FromArgb(239, 247, 255, 255));
                item.Foreground = new SolidColorBrush(Colors.Black);
                
                ContentFrame.Refresh();
                ContentFrame.Navigate(new Uri(item.Tag.ToString(), UriKind.Relative));
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
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
            e.Handled = true;
            //PublicMethod.ClientException(e.Exception, ((System.Windows.Navigation.NavigationService)(sender)).CurrentSource.OriginalString, true);
            PublicMethod.ClientException(e.Exception, this.GetType().FullName, true);
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /// <summary>
        /// 异步调用获得二级菜单的列表
        /// </summary>
        public void GetListinfo()
        {
            YidanEHRDataServiceClient serviceCon;

            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetV_PermissionListFatherCompleted +=
              (obj, e) =>
              {
                  if (e.Error == null)
                  {
                      v_prlistF = e.Result.ToList();
                      BindMenu(v_prlistF);
                      //设置第一个子菜单的属性
                      RadMenuItem rmenu = (RadMenuItem)radMenu.Items[0];
                      rmenu.Background = new SolidColorBrush(Colors.White);
                      rmenu.Foreground = new SolidColorBrush(Colors.Black);
                      rmenu.Margin = new Thickness(5,20,0,0);
                  }
                   
              };
            serviceCon.GetV_PermissionListFatherAsync(Global.LogInEmployee.Zgdm);
            serviceCon.CloseAsync();

        }

     

    }
}
