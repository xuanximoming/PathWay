using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace YidanEHRApplication.Pass
{
    /// <summary>
    /// Interaction logic for PathMainDetailForm.xaml
    /// </summary>
    public partial class RWPathMainDetailForm
    {
        String m_NavUrl = String.Empty;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="strUrl">导航URL</param>
        /// <param name="strHeader">显示HEADER</param>
        public RWPathMainDetailForm(String strUrl, String strHeader)
        {
            InitializeComponent();
            m_NavUrl = strUrl;
            this.Header = strHeader;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            ContentFrame.Navigate(new Uri(m_NavUrl, UriKind.Relative));
        }
    }
}
