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

namespace YidanEHRApplication.Views
{
    public partial class PathManagerCondition : ChildWindow
    {
        public string TextConditon
        {
            get;
            set;
        }
        public PathManagerCondition()
        {
            InitializeComponent();
        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            this.textBaoxCondition.Text = this.TextConditon;
        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                TextConditon = this.textBaoxCondition.Text;
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

    }
}

