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
using System.Windows.Browser;

namespace YidanEHRApplication.Pass
{
    public partial class PassMainForm : Page
    {
        public PassMainForm()
        {
            //StyleManager.ApplicationTheme = new Telerik.Windows.Controls.Office_BlueTheme();
            InitializeComponent();

        }

        private void RadMenuItem_Click(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            RadMenuItem item = sender as RadMenuItem;
            if (item == null || item.Tag == null || item.Tag.ToString().Trim() == "") return;
        }

        private void hltExplain_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HtmlPage.PopupWindow(new Uri("http://localhost/Files/Default.aspx", UriKind.Absolute), "_blank",
                                                        new System.Windows.Browser.HtmlPopupWindowOptions()
                                                        {
                                                            Width = 500,
                                                            Height = 500
                                                        });

                //HtmlPage.Window.Eval("window.open(\"http://localhost/Files/Default.aspx\")");
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void HyperlinkButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                HyperlinkButton item = sender as HyperlinkButton;
                if (item == null || item.Tag == null || item.Tag.ToString().Trim() == "")
                    return;
                RWPathMainDetailForm form = new RWPathMainDetailForm(item.Tag.ToString(), item.Content.ToString());
                form.WindowState = WindowState.Maximized;
                form.ShowDialog();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
    }
}
