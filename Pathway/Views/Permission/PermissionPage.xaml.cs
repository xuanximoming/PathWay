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
using YidanEHRApplication.Models;
using YidanEHRApplication.DataService;
namespace YidanEHRApplication.Views.Permission
{
    public partial class PermissionPage : Page
    {
        public PermissionPage()
        {
            InitializeComponent();
            ShowCyXDF();
            radBusyIndicator.IsBusy = false;
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            
        }

        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                foreach (RadMenuItem rad in radMenu.Items)
                {
                    rad.Background = new SolidColorBrush(Colors.Transparent);
                }


                RadMenuItem radMenuItem = sender as RadMenuItem;
                if (radMenuItem.Tag.ToString() == string.Empty)
                    return;

                radMenuItem.Background = new SolidColorBrush(Colors.White);
                string strUri;


                strUri = "/" + radMenuItem.Tag;

                ContentFrame.Refresh();
                ContentFrame.Navigate(new Uri(radMenuItem.Tag.ToString(), UriKind.Relative));
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void ContentFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }

        private void ContentFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            e.Handled = true;
            PublicMethod.ClientException(e.Exception, this.GetType().FullName, true);
        }

        /// <summary>
        /// 根据配置表判断是否显示草药医嘱相关内容
        /// </summary>
        private void ShowCyXDF()
        {
            #region add by luff 20130604 根据配置表判断是否显示草药医嘱相关内容
            try
            {
                List<APPCFG> t_listApp = Global.mAppCfg.Select(s => s).Where(s => s.Configkey.IndexOf("showCyXDF") > -1).ToList();
                if (t_listApp.Count > 0)
                {
                    if (t_listApp[0].Value == "1")//表示显示草药医嘱
                    {
                        Cyfitem.Visibility = Visibility.Visible;

                    }
                    else
                    {
                        Cyfitem.Visibility = Visibility.Collapsed;

                    }
                }
                else
                {
                    Cyfitem.Visibility = Visibility.Collapsed;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
            #endregion
        }
    }
}
