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
using Telerik.Windows.Controls;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Controls
{
    public partial class UCChildEditableMessageBox : UserControl
    {
        RadWindow window = new RadWindow();

        /// <summary>
        /// 页面中输入的信息
        /// </summary>
        public string InputMessageInfo { get; set; }
        public string RequestString = string.Empty;

        public string Title { get; set; }

        /// <summary>
        /// 定义文本框内容是否必须填写，调用时只需赋‘false’or 'true' 可以验证是否输入内容
        /// </summary>
        public bool NotEmpty { get; set; }


        public UCChildEditableMessageBox()
        {
            InitializeComponent();
            window.Width = 500;
            window.Height = 400;
            window.Content = this;

            this.Loaded += new RoutedEventHandler(ChildMessageBox_Loaded);
            this.Unloaded += new RoutedEventHandler(ChildMessageBox_Unloaded);
            window.Closed += new EventHandler<WindowClosedEventArgs>(window_Closed);
        }


        public void Show()
        {
            this.window.ShowDialog();
        }


        public EventHandler Closed;

        protected void OnWindowClosed()
        {
            if (Closed != null)
                Closed(this, new EventArgs());

        }

        void window_Closed(object sender, WindowClosedEventArgs e)
        {

        }

        void ChildMessageBox_Unloaded(object sender, RoutedEventArgs e)
        {
            window.Close();

        }

        void ChildMessageBox_Loaded(object sender, RoutedEventArgs e)
        {
            this.window.Header = Title;
            this.window.WindowStartupLocation = Telerik.Windows.Controls.WindowStartupLocation.CenterScreen;
            //this.window.TopOffset = -40;
            this.window.Show();
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if ((NotEmpty) && (string.IsNullOrWhiteSpace(InputMessage.Text)))
                {
                    labErr.Visibility = Visibility.Visible;
                    labErr.Content = "* 文本框内容必须填写";
                    return;
                }
                this.window.Close();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.window.Close();

        }

        private void InputMessage_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (InputMessage.Focus())
            {
                labErr.Visibility = Visibility.Collapsed;
            }
        }
    }
}
