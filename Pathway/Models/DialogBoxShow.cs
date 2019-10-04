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

//using YidanEHRApplication.Models;
using Telerik.Windows.Controls;

namespace YidanEHRApplication.Models
{
    public class DialogBoxShow
    {
        //调用对话框的对象
        private object m_EventSource = null;

        public DialogBoxShow()
        {
            m_EventSource = this;
        }

        /// <summary>
        /// 调用对话框的对象
        /// </summary>        
        public object EventSource
        {
            get { return m_EventSource; }
            set { m_EventSource = value; }
        }

        //定义事件数据源
        public class OpreateEventArgs : RoutedEventArgs
        {
            public bool bResult { get; set; }  //事件操作类型：true页面导航，false数据捆绑
            public OpreateEventArgs(bool bSelectedResult)
            {
                bResult = bSelectedResult;
            }
        }

        //定义对话框选择结果事件
        public delegate void SelectedResultEvent(object sender, RoutedEventArgs e);
        public event SelectedResultEvent SelectedResult;
        protected virtual void OnSelectedResultEvent(OpreateEventArgs e)
        {
            if (SelectedResult != null)
            {
                SelectedResult(m_EventSource, e);
            }
        }


        /// <summary>
        /// 显示确定/取消提示框
        /// </summary>
        /// <param name="Content"></param>
        public void ShowDialogBox(string Content)        {
           
            //DialogParameters parameters = new DialogParameters();
            //parameters.Content = String.Format("提示: {0}", Content);
            //parameters.Header = "提示";
            //parameters.IconContent = null;
            //parameters.OkButtonContent = "确定";
            //parameters.CancelButtonContent = "取消";
            //parameters.Closed = OnDelAdviceGroupDetail;//***close处理***
            //RadWindow.Confirm(parameters);


            YidanPathWayMessageBox mess = new YidanPathWayMessageBox(Content, "提示", YiDanMessageBoxButtons.YesNo);
            mess.ShowDialog();
            mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
        }

        void mess_PageClosedEvent(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    //返回确定True/取消False结果
                    OnSelectedResultEvent(new OpreateEventArgs((bool)e));
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void OnDelAdviceGroupDetail(object sender, WindowClosedEventArgs e)
        {   
            //返回确定True/取消False结果
            OnSelectedResultEvent(new OpreateEventArgs((bool)e.DialogResult));
           
        }

        /// <summary>
        /// 显示确定/取消提示框
        /// </summary>
        /// <param name="Content"></param>
        public void ShowDialogBox(string Content, object EventSource = null)
        {
            m_EventSource = EventSource;//设置调用对话框的对象
            ShowDialogBox(Content);
        }
    }
}
