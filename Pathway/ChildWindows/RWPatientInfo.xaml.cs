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

namespace YidanEHRApplication.Views.ChildWindows
{
    /// <summary>
    /// Interaction logic for RadChildWindowPatientInfo.xaml
    /// </summary>
    public partial class RWPatientInfo
    {
        public RWPatientInfo()
        {
            InitializeComponent();
            ContentFrame.Navigate(new Uri("/UCPatientDetailInfo", UriKind.Relative));
            this.PatientDetailInfo.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };
        }

        private void ContentFrame_Navigated(object sender, System.Windows.Navigation.NavigationEventArgs e)
        {

        }

        private void ContentFrame_NavigationFailed(object sender, System.Windows.Navigation.NavigationFailedEventArgs e)
        {

        }
    }
}
