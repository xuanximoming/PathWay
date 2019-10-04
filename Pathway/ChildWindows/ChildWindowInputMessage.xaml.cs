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
using Telerik.Windows.Controls;

namespace YidanEHRApplication
{
    public partial class ChildWindowInputMessage : ChildWindow
    {
        /// <summary>
        /// 页面中输入的信息
        /// </summary>
        public string InputMessageInfo { get; set; }
        public string RequestString = string.Empty;
        /// <summary>
        /// 定义文本框内容是否必须填写，调用时只需赋‘false’or 'true' 可以验证是否输入内容
        /// </summary>
        public bool IsNeed;
        public ChildWindowInputMessage()
        {
            InitializeComponent();

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsNeed)
                {
                    if (string.IsNullOrWhiteSpace(InputMessage.Text))
                    {
                        ErrorInfo.Visibility = Visibility.Visible;
                        ErrorInfo.Content = "* 文本框内容必须填写";
                        return;
                    }
                }
                this.DialogResult = true;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;

        }

        private void InputMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            var txt = sender as TextBox;
            if (txt.Focus())
            {
                ErrorInfo.Visibility = Visibility.Collapsed;
            }
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ErrorInfo.Visibility = Visibility.Collapsed;
            InputMessage.Text = string.Empty;
        }


    }
}

