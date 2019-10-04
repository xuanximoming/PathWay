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
using System.Windows.Browser;

namespace YidanEHRApplication.Helpers
{
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
        /// 获取窗口对象的客户端宽度
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
        /// 获取窗口对象的客户端高度
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
        /// 获取当前水平滚动抵消
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
        /// 获取当前垂直滚动抵消     
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
        /// 得到整个显示的宽度    
        /// </summary>     
        public static double ScreenWidth
        {
            get
            {
                return (double)Screen.GetProperty("width");
            }
        }

        /// <summary>     
        /// 得到整个显示的高度   
        /// </summary>     
        public static double ScreenHeight
        {
            get
            {
                return (double)Screen.GetProperty("height");
            }
        }

        /// <summary>     
        /// 得到了宽度可用的屏幕空间,不含码头或任务栏     
        /// </summary>     
        public static double AvailableScreenWidth
        {
            get
            {
                return (double)Screen.GetProperty("availWidth");
            }
        }

        /// <summary>     
        /// 得到了高度可用的屏幕空间,不含码头或任务栏     
        /// </summary>     
        public static double AvailableScreenHeight
        {
            get
            {
                return (double)Screen.GetProperty("availHeight");
            }
        }

        /// <summary>     
        /// 得到了绝对的左窗口的像素位置在显示坐标
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
        /// 得到了绝对的最高像素位置的窗口显示坐标     
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