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

using System.Text.RegularExpressions;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Controls
{
    public partial class UCTextGroupBoxControl : UserControl
    {
        private string m_Befor = "";//人工辅助前大便次数
        private string m_After = "";//人工辅助后大便次数
        private string m_Labour = "";//人工辅助大便次数

        #region 排便次数
        /// <summary>
        /// 排便次数
        /// </summary>
        public string Shit
        {
            get
            {
                if (chknoShit.IsChecked == true)
                {
                    m_Befor ="*";
                    m_Labour = "*";
                    m_After = "*";
                }
                else
                {
                    //string pattern = @"^[0-9]*$";
                    //Match m = Regex.Match(this.txtBefor.Text.Trim(), pattern);   // 匹配正则表达式           
                    //if (!m.Success)//不是数字
                    //{
                    //    PublicMethod.RadAlterBox("人工辅助前大便次数请输入数字", "提示");
                    //}
                    //string pattern = @"^[0-9]*$";
                    //Match m = Regex.Match(this.txtBefor.Text.Trim(), pattern);   // 匹配正则表达式           
                    //if (!m.Success)//不是数字
                    //{
                    //    PublicMethod.RadAlterBox("人工辅助前大便次数请输入数字", "提示");
                    //}
                    m_Befor = txtBefor.Text.Trim();
                    m_Labour = txtLabour.Text.Trim();
                    m_After = txtAfter.Text.Trim();
                }
                return m_Befor + ":" + m_Labour + ":" + m_After;
            }

            set 
            {
                Border1.Color = Colors.White;
                Border2.Color = Colors.White;

                if (value == "*:*:*")
                {
                    m_Befor = "";
                    m_Labour = "";
                    m_After = "";
                    chknoShit.IsChecked = true;

                    txtAfter.Visibility = Visibility.Collapsed;
                    txtBefor.Visibility = Visibility.Collapsed;
                    txtLabour.Visibility = Visibility.Collapsed;
                    Rectangle1.Visibility = Visibility.Collapsed;
                    chknoShit.Visibility = Visibility.Collapsed;
                    txtShowStart.Visibility = Visibility.Visible;

                }
                else
                {
                    string[] str = value.Split(':');
                    if (str.Length == 3 && value != "::")
                    {
                        m_Befor = str[0];
                        m_Labour = str[1];
                        m_After = str[2];

                        txtAfter.Visibility = Visibility.Visible;
                        txtBefor.Visibility = Visibility.Visible;
                        txtLabour.Visibility = Visibility.Visible;
                        Rectangle1.Visibility = Visibility.Visible;
                        chknoShit.Visibility = Visibility.Collapsed;
                        txtShowStart.Visibility = Visibility.Collapsed;
                    }
                    else
                    {
                        m_Befor = "";
                        m_Labour = "";
                        m_After = "";

                        txtAfter.Visibility = Visibility.Collapsed;
                        txtBefor.Visibility = Visibility.Collapsed;
                        txtLabour.Visibility = Visibility.Collapsed;
                        Rectangle1.Visibility = Visibility.Collapsed;
                        chknoShit.Visibility = Visibility.Collapsed;
                        txtShowStart.Visibility = Visibility.Collapsed;
                    }
                    chknoShit.IsChecked = false;
                }
                txtBefor.Text = m_Befor;
                txtLabour.Text=m_Labour ;
                txtAfter.Text=m_After ;

                
            }
        }
        #endregion

        #region 判断是否输入数据
        /// <summary>
        /// 判断是否输入数据
        /// </summary>
        public bool IsInput
        {
            get
            {
                if (chknoShit.IsChecked == true)
                {
                    return true;
                }
                else
                {
                    if ((txtBefor.Text.Trim() == "" && txtAfter.Text.Trim() == "" && txtLabour.Text.Trim() == "") ||
                        (txtBefor.Text.Trim() == "" && txtAfter.Text.Trim() == "" && txtLabour.Text.Trim() != ""))
                    {
                        return false;
                    }
                    else
                    {
                        return true ;
                    }
                }
            }
        }
        #endregion

        #region 数据重置
        /// <summary>
        /// 数据重置
        /// </summary>
        public void DateTextReset()
        {
            m_Befor = "";
            m_Labour = "";
            m_After = "";

            txtAfter.IsEnabled = true;
            txtBefor.IsEnabled = true;
            txtLabour.IsEnabled = true;

            txtBefor.Text = m_Befor;
            txtLabour.Text = m_Labour;
            txtAfter.Text = m_After;

            chknoShit.IsChecked = false;
        }
        #endregion

        public UCTextGroupBoxControl()
        {
            InitializeComponent();
        }

        private void TextNumber_TextChanged(object sender, TextChangedEventArgs e)
        {   
            //string pattern = @"^[0-9]*$";
            //Match m = Regex.Match((sender as TextBox).Text.Trim(), pattern);   // 匹配正则表达式           
            //if (!m.Success)//不是数字
            //{
            //    PublicMethod.RadAlterBox("请输入数字", "提示");
            //    (sender as TextBox).Text = (sender as TextBox).Tag.ToString();   // textBox内容不变

            //    // 将光标定位到文本框的最后
            //    (sender as TextBox).SelectionStart = (sender as TextBox).Text.Length;
            //}
            //else
            //{
            //    (sender as TextBox).Tag=(sender as TextBox).Text ;
            //}
            
        }

        private void TextString_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Regex objRegex = new Regex("^*$");
            //if (!objRegex.IsMatch((sender as TextBox).Text.Trim()))
            //{
            //    PublicMethod.RadAlterBox("请输入数据格式（如：2E)）", "提示");
            //}
        }

        private void chknoShit_Checked(object sender, RoutedEventArgs e)
        {
            txtAfter.IsEnabled = false;
            txtBefor.IsEnabled = false;
            txtLabour.IsEnabled = false;
        }

        private void chknoShit_Unchecked(object sender, RoutedEventArgs e)
        {
            txtAfter.IsEnabled = true;
            txtBefor.IsEnabled = true;
            txtLabour.IsEnabled = true;
        }

        
    }
}
