using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Drawing;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using YidanEHRApplication;
using Telerik.Windows.Documents.Model;
using System.Reflection;
using YidanEHRApplication.Views;
using System.Windows.Media.Imaging;
using System.Collections.ObjectModel;
using System.Threading;
using Telerik.Windows.Controls;
using YidanEHRApplication.Models;

namespace YidanEHRApplication
{
    public partial class YidanPathWayMessageBox
    {
        #region const
        private const int InitTop = 10;//初始
        private const int InitLeft = 10;
        private const int InitGap = 5;
        private const string s_OkCaption = "确定(&O)";
        private const string s_CancelCaption = "取消(&C)";
        private const string s_YesCaption = "是(&Y)";
        private const string s_NoCaption = "否(&N)";

        private const int s_MinBoxWidth = 250;
        private const int s_MinBoxHeight = 134;
        private const int s_DefaultPad = 3;
        #endregion

        #region fields
        /// <summary>
        /// 窗体最大宽度
        /// </summary>
        private int m_MaxWidth;
        /// <summary>
        /// 窗体最大高度
        /// </summary>
        private int m_MaxHeight;
        private int m_MaxLayoutWidth;
        private int m_MaxLayoutHeight;
        private int m_MinLayoutWidth;
        private int m_MinLayoutHeight;
        /// <summary>
        /// 为消息居中而增加
        /// </summary>
        private bool m_iconshow = false;
        #endregion

        private void LayoutRoot_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            e.Handled = true;
        }

        public YidanPathWayMessageBox()
        {
            InitializeComponent();

            try
            {
                initForm();//初始化
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 消息提示框信息
        /// </summary>
        /// <param name="_msg">页面提示信息</param>
        /// <param name="_caption">提示标题</param>
        /// <param name="_btns">页面展示按钮</param>
        /// <param name="_icontype">提示图标类型</param>
        public YidanPathWayMessageBox(string _msg, string _caption, YiDanMessageBoxButtons _btns, YiDanMessageBoxIcon _icontype)
        {
            InitializeComponent();
            try
            {
                initForm();//初始化
                this.SetIconInfo(_icontype);
                this.SetMessageInfo(_msg);
                this.SetButtonInfo(_btns);
                this.Header = _caption;

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        public YidanPathWayMessageBox(string _msg, string _caption,
            YiDanMessageBoxButtons _btns)
        {
            InitializeComponent();
            try
            {
                initForm();//初始化
                //this.SetIconInfo(_icontype);
                this.SetMessageInfo(_msg);
                this.SetButtonInfo(_btns);
                this.Header = _caption;

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }


        public YidanPathWayMessageBox(string _msg, string _caption)
        {
            InitializeComponent();
            try
            {
                initForm();//初始化
                //this.SetIconInfo(_icontype);
                this.SetMessageInfo(_msg);
                this.SetButtonInfo(YiDanMessageBoxButtons.Ok);
                this.Header = _caption;

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        public YidanPathWayMessageBox(string _msg)
        {
            InitializeComponent();
            try
            {
                initForm();//初始化
                this.SetIconInfo(YiDanMessageBoxIcon.WarningIcon);
                this.SetMessageInfo(_msg);
                this.SetButtonInfo(YiDanMessageBoxButtons.Ok);
                this.Header = "提示";

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        public YidanPathWayMessageBox(Exception _ex)
        {
            InitializeComponent();
            try
            {
                initForm();//初始化
                this.SetIconInfo(YiDanMessageBoxIcon.ErrorIcon);
                //btnShow.Visibility = System.Windows.Visibility.Visible;
                txtDetail.Text = _ex.Message + "\r\n" + _ex.StackTrace;
                //this.SetMessageInfo(_ex.Message);
                this.labelMessage.Text = "系统错误，请联系管理员！";
                //this.txtDetail.Text = _ex.Message+""
                this.SetButtonInfo(YiDanMessageBoxButtons.Ok);
                this.Header = "错误提示";
                
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        public YidanPathWayMessageBox(string _msg, YiDanMessageBoxIcon _icontype)
        {
            InitializeComponent();
            try
            {
                initForm();//初始化
                this.SetIconInfo(_icontype);
                this.SetMessageInfo(_msg);
                this.SetButtonInfo(YiDanMessageBoxButtons.Ok);
                this.Header = "提示";

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 提醒窗体 指定时间后关闭
        /// </summary>
        /// <param name="_msg"></param>
        /// <param name="timerSecond"></param>
        public YidanPathWayMessageBox(string _msg, int timerSecond)
        {
            InitializeComponent();
            try
            {
                if (timerSecond <= 0)
                {
                    new Exception("弹出时间必须大于0");
                }
                initForm();//初始化
                //this.SetIconInfo(YiDanMessageBoxIcon.InformationIcon);
                this.SetMessageInfo(_msg);
                this.SetButtonInfo(YiDanMessageBoxButtons.Ok);
                this.Header = "提示";
                timer.Duration = new TimeSpan(0, 0, 0, 0, timerSecond * 1000); //200毫秒
                timer.Begin();
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        /// <summary>
        /// 带有时间的提示框
        /// </summary>
        /// <param name="_msg"></param>
        /// <param name="_caption"></param>
        /// <param name="timerSecond"></param>
        public YidanPathWayMessageBox(string _msg, string _caption, int timerSecond)
        {
            InitializeComponent();
            try
            {
                if (timerSecond <= 0)
                {
                    new Exception("弹出时间必须大于0");
                }
                initForm();//初始化 
                this.SetMessageInfo(_msg);
                this.SetButtonInfo(YiDanMessageBoxButtons.Ok);
                this.Header = _caption;
                timer.Duration = new TimeSpan(0, 0, 0, 0, timerSecond * 1000); //200毫秒
                timer.Begin();

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }



        void timer_Tick(object sender, EventArgs e)
        {
            //在这里处理定时器事件

            try
            {
                timer.Stop();
                this.Close();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 初始化
        /// </summary>
        private void initForm()
        {
            try
            {
                this.Header = "提示";
                this.KeyDown += new KeyEventHandler(MessageBox_KeyDown);

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }


        /// <summary>
        /// 按Ctrl+C复制ErrMsg至剪粘板
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void MessageBox_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                ModifierKeys keys = Keyboard.Modifiers;
                if (e.Key == Key.C && keys == ModifierKeys.Control)
                {
                    string message = txtDetail.Text;
                    if (message.Trim().Length == 0)
                    {
                        message = labelMessage.Text;
                    }
                    System.Windows.Clipboard.SetText(message);

                }

                
            }
            catch (Exception ce)
            {
                MessageBox.Show(ce.Message.ToString(), "提示", MessageBoxButton.OK);
            }
        }

        /// <summary>
        /// 显示信息
        /// </summary>
        /// <param name="messages"></param>
        private void SetMessageInfo(string messages)
        {
            try
            {
                if (messages == null)
                {
                    labelMessage.Text = String.Empty;
                }
                else
                {
                    //if (messages.Trim().Length > 200)
                    //{
                    //    labelMessage.Text = messages.Trim().Substring(0, 200);
                    //    btnShow.Visibility = System.Windows.Visibility.Visible;
                    //    this.Width = 450;
                    //    txtDetail.Text = messages;
                    //}
                    //else
                    //{
                        labelMessage.Text = messages.Trim();
                    //}
                }

            }
            catch (Exception ce)
            {
                throw ce;
            }

        }



        private void OKButton_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = false;
                this.Close();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
 

        #region set icon info
        private void SetIconInfo(YiDanMessageBoxIcon iconType)
        {
            try
            {
                switch (iconType)
                {
                    case YiDanMessageBoxIcon.ErrorIcon:
                        picMessageBoxError.Visibility = System.Windows.Visibility.Visible;
                        picMessageBoxMessage.Visibility = System.Windows.Visibility.Collapsed;
                        picMessageBoxPrompt.Visibility = System.Windows.Visibility.Collapsed;
                        picMessageBoxWarning.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    case YiDanMessageBoxIcon.InformationIcon:
                        picMessageBoxError.Visibility = System.Windows.Visibility.Collapsed;
                        picMessageBoxMessage.Visibility = System.Windows.Visibility.Visible;
                        picMessageBoxPrompt.Visibility = System.Windows.Visibility.Collapsed;
                        picMessageBoxWarning.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    case YiDanMessageBoxIcon.QuestionIcon: 
                        picMessageBoxError.Visibility = System.Windows.Visibility.Collapsed;
                        picMessageBoxMessage.Visibility = System.Windows.Visibility.Collapsed;
                        picMessageBoxPrompt.Visibility = System.Windows.Visibility.Visible;
                        picMessageBoxWarning.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    case YiDanMessageBoxIcon.WarningIcon:
                        picMessageBoxError.Visibility = System.Windows.Visibility.Collapsed;
                        picMessageBoxMessage.Visibility = System.Windows.Visibility.Collapsed;
                        picMessageBoxPrompt.Visibility = System.Windows.Visibility.Collapsed; 
                        picMessageBoxWarning.Visibility = System.Windows.Visibility.Visible;

                        break;
                    default:
                        break;
                }

            }
            catch (Exception ce)
            {
                picMessageBoxError.Visibility = System.Windows.Visibility.Visible;

                throw ce;
            }
        }

        public void SetButtonInfo(YiDanMessageBoxButtons kind)
        {
            try
            {
                switch (kind)
                {
                    case YiDanMessageBoxButtons.None:
                        //buttonInfoControls.Add(CreateButton(s_OkCaption, DialogResult.OK));
                        this.ButtonCancel.Visibility = System.Windows.Visibility.Collapsed;
                        this.ButtonOK.Visibility = System.Windows.Visibility.Collapsed;
                        break;
                    case YiDanMessageBoxButtons.Ok:
                        //buttonInfoControls.Add(CreateButton(s_OkCaption, DialogResult.OK));
                        //this.ButtonCancel.Visibility = System.Windows.Visibility.Collapsed;
                        this.ButtonCancel.Visibility = System.Windows.Visibility.Collapsed;
                        //ButtonOK.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        //ButtonOK.h.Left = 0;
                        //ButtonOK.Margin.Right = 0;
                        this.DialogResult = true;
                        break;
                    case YiDanMessageBoxButtons.OkCancel:
                        break;
                    case YiDanMessageBoxButtons.YesNo:
                        //buttonInfoControls.Add(CreateButton(s_YesCaption, DialogResult.Yes));
                        //buttonInfoControls.Add(CreateButton(s_NoCaption, DialogResult.No));
                        SetIconInfo(YiDanMessageBoxIcon.QuestionIcon);
                        break;
                    case YiDanMessageBoxButtons.YesNoCancel:
                        //buttonInfoControls.Add(CreateButton(s_YesCaption, DialogResult.Yes));
                        //buttonInfoControls.Add(CreateButton(s_NoCaption, DialogResult.No, false));
                        //buttonInfoControls.Add(CreateButton(s_CancelCaption, DialogResult.Cancel));
                        break;
                    case YiDanMessageBoxButtons.Yes:
                        //this.ButtonCancel.Visibility = System.Windows.Visibility.Collapsed;
                        this.ButtonCancel.Visibility = System.Windows.Visibility.Collapsed;
                        //ButtonOK.HorizontalAlignment = System.Windows.HorizontalAlignment.Center;
                        break;
                    default:
                        break;
                }
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }
        #endregion

        /// <summary>
        /// 显示详细
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (scrDetail.Visibility == System.Windows.Visibility.Visible)
                {
                    //btnShow.Visibility = System.Windows.Visibility.Visible;
                    //btnHide.Visibility = System.Windows.Visibility.Collapsed;
                    scrDetail.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    //btnShow.Visibility = System.Windows.Visibility.Collapsed;
                    //btnHide.Visibility = System.Windows.Visibility.Visible;
                    scrDetail.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region 定义窗体关闭事件，方便选择判断后父页面根据选择结果继续执行
        public delegate void PageClosed(object sender, bool e);

        /// <summary>
        /// 定义窗体关闭事件，方便选择判断后父页面根据选择结果继续执行
        /// </summary>
        public event PageClosed PageClosedEvent;

        protected virtual void OnPageClosedEvent(bool e)
        {
            try
            {
                if (PageClosedEvent != null)
                    PageClosedEvent(this, e);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

        /// <summary>
        /// 窗体关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadWindow_Closed(object sender, WindowClosedEventArgs e)
        {
            try
            {
                if (this.DialogResult == true)
                {
                    OnPageClosedEvent(true);
                }
                else
                {
                    OnPageClosedEvent(false);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.Focus();
        }

        /// <summary>
        /// 点击显示错误信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void picMessageBoxError_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                if (scrDetail.Visibility == System.Windows.Visibility.Visible)
                {
                    //btnShow.Visibility = System.Windows.Visibility.Visible;
                    //btnHide.Visibility = System.Windows.Visibility.Collapsed;
                    scrDetail.Visibility = System.Windows.Visibility.Collapsed;
                }
                else
                {
                    //btnShow.Visibility = System.Windows.Visibility.Collapsed;
                    //btnHide.Visibility = System.Windows.Visibility.Visible;
                    scrDetail.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
    }

    /// <summary>
    /// 自定义消息提示框的类型
    /// </summary>
    public enum YiDanMessageBoxButtons
    {
        /// <summary>
        /// Default value
        /// </summary>
        None = 0,

        /// <summary>
        ///  ok button
        /// </summary>
        Ok = 1,

        /// <summary>
        /// ok + cancel button
        /// </summary>
        OkCancel = 2,

        /// <summary>
        ///  yes + no button
        /// </summary>
        YesNo = 3,

        /// <summary>
        ///  yes + no + cancel button
        /// </summary>
        YesNoCancel = 4,
        /// <summary>
        ///  yes button
        /// </summary>
        Yes = 5
    }
    /// <summary>
    /// 自定义消息提示框的图标
    /// </summary>

    public enum YiDanMessageBoxIcon
    {
        None = 0,
        WarningIcon = 1,// ! icon
        ErrorIcon = 2,// x icon
        QuestionIcon = 3,// ? icon
        InformationIcon = 4// i icon
    }


    public class YiDanMessageBox
    {
        static Control m_focusControl;
        public YiDanMessageBox()
        {
            //
        }

        /// <summary>
        /// 消息提示框
        /// </summary>
        /// <param name="mess">提示框中提示信息</param>
        public static void Show(string mess)
        {
            try
            {
                YidanPathWayMessageBox messagebox = new YidanPathWayMessageBox(mess);
                messagebox.ShowDialog();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 消息提示框
        /// </summary>
        /// <param name="mess">提示信息</param>
        /// <param name="iconType">提示图标</param>
        public static void Show(string mess, YiDanMessageBoxIcon iconType)
        {
            try
            {
                YidanPathWayMessageBox messagebox = new YidanPathWayMessageBox(mess, iconType);
                messagebox.ShowDialog();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 消息提示框
        /// </summary>
        /// <param name="mess">提示框中提示信息</param>
        /// <param name="_timerSecond">页面保留多少秒自动关闭</param>
        public static void Show(string mess,int _timerSecond)
        {
            try
            {
                YidanPathWayMessageBox messagebox = new YidanPathWayMessageBox(mess, _timerSecond);
                messagebox.ShowDialog();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 消息提示框
        /// </summary>
        /// <param name="mess">提示框中提示信息</param>
        /// <param name="_timerSecond">页面保留多少秒自动关闭</param>
        public static void Show(string mess , string _caption,int _timerSecond)
        {
            try
            {
                YidanPathWayMessageBox messagebox = new YidanPathWayMessageBox(mess,_caption, _timerSecond);
                messagebox.ShowDialog();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 消息提示框
        /// </summary>
        /// <param name="mess">异常信息</param>
        public static void Show(Exception _ex, string ModelName)
        {
            try
            {
                //YidanPathWayMessageBox messagebox = new YidanPathWayMessageBox(_ex.Message + "\r\n" + _ex.StackTrace, "错误信息", YiDanMessageBoxButtons.Ok, YiDanMessageBoxIcon.ErrorIcon);

                YidanPathWayMessageBox messagebox = new YidanPathWayMessageBox(_ex);
                messagebox.ShowDialog();

                PublicMethod.InsertClientLogException(_ex, ModelName);
            }
            catch (Exception ex)
            {
                YidanPathWayMessageBox messagebox = new YidanPathWayMessageBox(_ex);
                messagebox.ShowDialog();

                //PublicMethod.InsertClientLogException(_ex, ModelName);
                //Show(ex.Message + "\r\n" + ex.StackTrace, YiDanMessageBoxIcon.ErrorIcon);
            }
        }

        /// <summary>
        /// 消息提示框
        /// </summary>
        /// <param name="_msg">提示框中提示信息</param>
        /// <param name="_caption">提示框信息标题</param>
        public static void Show(string _msg, string _caption)
        {
            try
            {
                YidanPathWayMessageBox messagebox = new YidanPathWayMessageBox(_msg, _caption);
                messagebox.ShowDialog();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 消息提示框
        /// </summary>
        /// <param name="_msg">提示框中提示信息</param>
        /// <param name="_caption">提示框信息标题</param>
        /// <param name="buts">页面展示按钮</param>
        public static void Show(string _msg, string _caption, YiDanMessageBoxButtons _buts)
        {
            try
            {
                YidanPathWayMessageBox messagebox = new YidanPathWayMessageBox(_msg, _caption, _buts);
                messagebox.ShowDialog();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 消息提示框
        /// </summary>
        /// <param name="_msg">提示框中提示信息</param>
        /// <param name="_caption">提示框信息标题</param>
        /// <param name="buts">页面展示按钮</param>
        public static void Show(string _msg,  YiDanMessageBoxButtons _buts)
        {
            try
            {
                YidanPathWayMessageBox messagebox = new YidanPathWayMessageBox(_msg, "提示", _buts);
                messagebox.ShowDialog();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


 
        /// <summary>
        /// 消息提示框
        /// </summary>
        /// <param name="_msg">提示框中提示信息</param>
        /// <param name="_control">关闭提示框后页面需要获取焦点的控件</param>
        public static void Show(string _msg, Control _control)
        {
            try
            {
                m_focusControl = _control;
                YidanPathWayMessageBox messagebox = new YidanPathWayMessageBox(_msg, "提示", YiDanMessageBoxButtons.Ok);
                messagebox.ShowDialog();
                messagebox.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(messagebox_PageClosedEvent);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

 
        /// <summary>
        /// 消息提示框
        /// </summary>
        /// <param name="_msg">提示框中提示信息</param>
        /// <param name="_control">关闭提示框后页面需要获取焦点的控件</param>
        /// <param name="_caption">提示框标题</param>
        public static void Show(string _msg, Control _control, string _caption)
        {
            try
            {
                m_focusControl = _control;
                YidanPathWayMessageBox messagebox = new YidanPathWayMessageBox(_msg, _caption, YiDanMessageBoxButtons.Ok);
                messagebox.ShowDialog();
                messagebox.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(messagebox_PageClosedEvent);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 消息提示框
        /// </summary>
        /// <param name="_msg">提示框中提示信息</param>
        /// <param name="_control">关闭提示框后页面需要获取焦点的控件</param>
        /// <param name="_caption">提示框标题</param>
        /// <param name="_buts">页面显示按钮</param>
        public static void Show(string _msg, Control _control, string _caption,YiDanMessageBoxButtons _buts)
        {
            try
            {
                m_focusControl = _control;
                YidanPathWayMessageBox messagebox = new YidanPathWayMessageBox(_msg, _caption, _buts);
                messagebox.ShowDialog();
                messagebox.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(messagebox_PageClosedEvent);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 消息提示框
        /// </summary>
        /// <param name="_msg">提示框中提示信息</param>
        /// <param name="_control">关闭提示框后页面需要获取焦点的控件</param>
        /// <param name="_timerSecond">页面保留多少秒自动关闭</param> 
        public static void Show(string _msg, Control _control, int _timerSecond)
        {
            try
            {
                m_focusControl = _control;
                YidanPathWayMessageBox messagebox = new YidanPathWayMessageBox(_msg, _timerSecond);
                messagebox.ShowDialog();
                messagebox.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(messagebox_PageClosedEvent);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        static void messagebox_PageClosedEvent(object sender, bool e)
        {
            try
            {
                if (m_focusControl == null)
                {
                    return;
                }
                m_focusControl.Focus();
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }


    }

}

