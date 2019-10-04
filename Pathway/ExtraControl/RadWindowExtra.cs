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
using Telerik.Windows.Controls;

namespace YidanEHRApplication.ExtraControl
{
    /// <summary>
    /// radWindows扩展
    /// 提供提供静态调用方法
    /// </summary>
    public class RadWindowExtra : RadWindow
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Content"></param>
        /// <param name="HeaderText"></param>
        /// <param name="IconContent"></param>
        /// <param name="OkButtonContent"></param>
        /// <param name="CancelButtonContent"></param>
        /// <param name="Closed"></param>
        public static void Confirm(String Content, String HeaderText, Object IconContent
            , String OkButtonContent, String CancelButtonContent, EventHandler<WindowClosedEventArgs> Closed)
        {
            DialogParameters parm = new DialogParameters();
            if (OkButtonContent == null) parm.OkButtonContent = "确定";
            if (CancelButtonContent == null) parm.CancelButtonContent = "取消";
            if (Content == null) parm.Content = "";
            if (HeaderText == null) parm.Header = "";
            if (IconContent == null) parm.IconContent = null;
            parm.Closed = Closed;
            Confirm(parm);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="parm"></param>
        public static void Confirm(DialogParameters parm)
        {
            RadWindow.Confirm(parm);
        }

    }
}
