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
using System.ServiceModel;
using YidanEHRApplication.Helpers;
using System.Windows.Browser;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using System.Collections.Generic;

namespace YidanEHRApplication
{
    public static class Global
    {
        /// <summary>
        /// 电子病历中通过url访问的时候使用
        /// </summary>
        public static String Syxh="";
     public static    Boolean isNavigate = false;//设置一个值，该值指示当前系统是否使用页面导航方式
        public static  RadTabControl tbc = new RadTabControl();
      
        /// <summary>
        /// 登录用户基本信息
        /// </summary>
        public static CP_Employee LogInEmployee = null;

        /// <summary>
        /// 当前选中病患
        /// </summary>
        public static CP_InpatinetList InpatientListCurrent = null;

        //add by luff 20130227 全局参数信息 
        /// <summary>
        /// 全局参数表信息
        /// </summary>
        public static List<APPCFG> mAppCfg = null;

        /// <summary>
        /// 用户角色信息
        /// </summary>
        public static List<PE_UserRole> UserRole = null;

        /// <summary>
        /// 用户多科室信息
        /// </summary>
        public static List<User2Dept> User2Dept = null;
 
        /// <summary>
        /// 是否使用操作医嘱相关权限
        /// </summary>
        public static bool IsXsyz = false;

        /// <summary>
        /// 关闭client
        /// </summary>
        /// <param name="myServiceClient"></param>
        public static void CloseConnection(this ICommunicationObject myServiceClient)
        {
            if (myServiceClient.State != CommunicationState.Opened)
            {
                return;
            }
            try
            {
                myServiceClient.Close();
            }
            catch (CommunicationException ex)
            {
                myServiceClient.Abort();
                throw ex;
            }
            catch (TimeoutException ex)
            {
                myServiceClient.Abort();
                throw ex;
            }
            catch (Exception ex)
            {
                //Debug.WriteLine(ex.ToString());
                myServiceClient.Abort();
                throw ex;
            }
        }

    }

    /// <summary>
    /// 浏览器屏幕信息类
    /// </summary>
    public static class Browser
    {
        /// <summary>   
        /// During static instantiation, only the Netscape flag is checked   
        /// </summary>   
        static Browser()
        {
            _isNavigator = HtmlPage.BrowserInformation.Name.Contains("Netscape");
        }

        /// <summary>   
        /// Flag indicating Navigator/Firefox/Safari or Internet Explorer   
        /// </summary>   
        private static bool _isNavigator;

        /// <summary>   
        /// Provides quick access to the window.screen ScriptObject   
        /// </summary>   
        private static ScriptObject Screen
        {
            get
            {
                ScriptObject screen = (ScriptObject)HtmlPage.Window.GetProperty("screen");

                if (screen == null)
                {
                    throw new InvalidOperationException();
                }

                return screen;
            }
        }

        /// <summary>   
        /// Gets the window object's client width   
        /// </summary>   
        public static double ClientWidth
        {
            get
            {
                return _isNavigator ? (double)HtmlPage.Window.GetProperty("innerWidth")
                    : (double)HtmlPage.Document.Body.GetProperty("clientWidth");
            }

        }

        /// <summary>   
        /// Gets the window object's client height   
        /// </summary>   
        public static double ClientHeight
        {
            get
            {
                return _isNavigator ? (double)HtmlPage.Window.GetProperty("innerHeight")
                    : (double)HtmlPage.Document.Body.GetProperty("clientHeight");
            }
        }

        /// <summary>   
        /// Gets the current horizontal scrolling offset   
        /// </summary>   
        public static double ScrollLeft
        {
            get
            {
                return _isNavigator ? (double)HtmlPage.Window.GetProperty("pageXOffset")
                    : (double)HtmlPage.Document.Body.GetProperty("scrollLeft");
            }
        }

        /// <summary>   
        /// Gets the current vertical scrolling offset   
        /// </summary>   
        public static double ScrollTop
        {
            get
            {
                return _isNavigator ? (double)HtmlPage.Window.GetProperty("pageYOffset")
                    : (double)HtmlPage.Document.Body.GetProperty("scrollHeight");
            }
        }

        /// <summary>   
        /// Gets the width of the entire display   
        /// </summary>   
        public static double ScreenWidth
        {
            get
            {
                return (double)Screen.GetProperty("width");
            }
        }

        /// <summary>   
        /// Gets the height of the entire display   
        /// </summary>   
        public static double ScreenHeight
        {
            get
            {
                return (double)Screen.GetProperty("height");
            }
        }

        /// <summary>   
        /// Gets the width of the available screen real estate, excluding the dock   
        /// or task bar   
        /// </summary>   
        public static double AvailableScreenWidth
        {
            get
            {
                return (double)Screen.GetProperty("availWidth");
            }
        }

        /// <summary>   
        /// Gets the height of the available screen real estate, excluding the dock /// or task bar   
        /// </summary>   
        public static double AvailableScreenHeight
        {
            get
            {
                return (double)Screen.GetProperty("availHeight");
            }
        }

        /// <summary>   
        /// Gets the absolute left pixel position of the window in display coordinates   
        /// </summary>   
        public static double ScreenPositionLeft
        {
            get
            {
                return _isNavigator ? (double)HtmlPage.Window.GetProperty("screenX")
                    : (double)HtmlPage.Window.GetProperty("screenLeft");
            }
        }

        /// <summary>   
        /// Gets the absolute top pixel position of the window in display coordinates   
        /// </summary>   
        public static double ScreenPositionTop
        {
            get
            {
                return _isNavigator ? (double)HtmlPage.Window.GetProperty("screenY")
                    : (double)HtmlPage.Window.GetProperty("screenTop");
            }
        }
    }



}
