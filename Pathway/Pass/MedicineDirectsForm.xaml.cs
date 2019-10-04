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
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using YidanEHRApplication.YidanEHRServiceReference;
using YidanEHRApplication.Models;
using YidanEHRApplication.DataService;
namespace YidanEHRApplication.Pass
{
    public partial class MedicineDirectsForm : Page
    {
        #region 函数
        public MedicineDirectsForm()
        {
            InitializeComponent();
        }

     
        #endregion
        #region 事件
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        private void atTxt_TextChanged(object sender, RoutedEventArgs e)
        {
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            ServiceClient.GetMedicinesDirectsTitleCompleted +=
                 (obj, ea) =>
                 {
                     atTxt.ItemsSource = ea.Result;
                     atTxt.ValueMemberPath = "DirectTitle2";
                     atTxt.IsDropDownOpen = true;
                 };
            ServiceClient.GetMedicinesDirectsTitleAsync(((AutoCompleteBox)sender).Text);
            ServiceClient.CloseAsync();
        }
        private void atTxt_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                ServiceClient.GetMedicinesDirectsCompleted +=
                 (obj, ea) =>
                 {
                     GridView.ItemsSource = ea.Result;
                 };
                ServiceClient.GetMedicinesDirectsAsync(((AutoCompleteBox)sender).Text);
                ServiceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        private void GridView_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            try
            {
                String ID = ((MedicineDirect)GridView.SelectedItem).ID;
                RWMedicineDirectDetailForm direct = new RWMedicineDirectDetailForm(ID);
                direct.ShowDialog();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        #endregion
    }
}
