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
using YidanEHRApplication.DataService;
using YidanEHRApplication.Helpers;
using System.Windows.Navigation;
using YidanEHRApplication.Models;
using Telerik.Windows.Controls;
using YidanSoft.Tool;
using YidanEHRApplication.Views.ChildWindows;
using YidanEHRApplication.Views.NursingNotes;
using System.Windows.Data;
using YidanEHRApplication.ChildWindows;
using YidanEHRApplication.NurModule;
using System.Threading;


namespace YidanEHRApplication.Controls
{
    public partial class MouseClickManager
    {
        public event MouseButtonEventHandler Click;
        public event MouseButtonEventHandler DoubleClick;

        /// <summary>
        /// 是否触发事件 <see cref="MouseClickManager"/>
        /// </summary>
        /// <value><c>true</c> if clicked; otherwise, <c>false</c>.</value>
        private bool Clicked { get; set; }
 
        /// <summary>
        /// 控件
        /// </summary>
        /// <value></value>
        public Control Control { get; set; }

        /// <summary>
        /// 超时时间
        /// </summary>
        /// <value></value>
        public int Timeout { get; set; }

        /// <summary>
        /// 初始化 <see cref="MouseClickManager"/>
        /// </summary>
        /// <param name="control"></param>
        public MouseClickManager(Control control, int timeout)
        {
            this.Clicked = false;
            this.Control = control;
            this.Timeout = timeout;
        }

        /// <summary>
        /// 委托处理单击
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e"></param>
        public void HandleClick(object sender, MouseButtonEventArgs e)
        {
            lock(this)
            {
                if (this.Clicked)
                {
                    this.Clicked = false;
                    OnDoubleClick(sender, e);
                }
                else
                {
                    this.Clicked = true;
                    ParameterizedThreadStart threadStart = new ParameterizedThreadStart(ResetThread);
                    Thread thread = new Thread(threadStart);
                    thread.Start(e);
                }
            }
        }

        /// <summary>
        /// 重置线程
        /// </summary>
        /// <param name="state">The state.</param>
        private void ResetThread(object state)
        {
            Thread.Sleep(this.Timeout);

            lock (this)
            {
                if (this.Clicked)
                {
                    this.Clicked = false;
                    OnClick(this, (MouseButtonEventArgs)state);
                }
            }
        }

        /// <summary>
        /// 单击事件
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e"></param>
        private void OnClick(object sender, MouseButtonEventArgs e)
        {
            MouseButtonEventHandler handler = Click;

            if (handler != null)
                this.Control.Dispatcher.BeginInvoke(handler, sender, e);
        }

        /// <summary>
        /// 双击事件
        /// </summary>
        /// <param name="sender">The sender.</param>
        private void OnDoubleClick(object sender, MouseButtonEventArgs e)
        {
            MouseButtonEventHandler handler = DoubleClick;

            if (handler != null)
                handler(sender, e);
        }
    
    }
}
