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
using YidanEHRApplication.YidanEHRServiceReference;
using YidanEHRApplication.Models;
using System.Collections.ObjectModel;
using YidanEHRApplication.DataService;
namespace YidanEHRApplication.Pass
{
    public partial class MedicinesForm : Page
    {
        Dictionary<String, ObservableCollection<String>> dictionary;
        #region 事件
        void btnSearch_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                ServiceClient.GetMedicinesCompleted +=
                   (obj, ea) =>
                   {
                       //throw new NotImplementedException();
                       GridView.ItemsSource = ea.Result;
                   };
                ServiceClient.GetMedicinesAsync(atTxt.Text, cmbCategoryTwo.Text, cmbCategoryThree.Text);
                ServiceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void atTxt_TextChanged(object sender, RoutedEventArgs e)
        {
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            ServiceClient.GetMedicinesNameCompleted +=
              (obj, ea) =>
              {
                  atTxt.ItemsSource = null;
                  atTxt.ItemsSource = ea.Result;
                  atTxt.ValueMemberPath = "Name";
                  atTxt.IsDropDownOpen = true;
              };
            ServiceClient.GetMedicinesNameAsync(((AutoCompleteBox)sender).Text, cmbCategoryTwo.Text, cmbCategoryThree.Text);
            ServiceClient.CloseAsync();
        }
        void cmbCategoryTwo_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {try{
            cmbCategoryThree.ItemsSource = null;
            List<String> lstCategoryThree = new List<string>();
            foreach (var item in dictionary["Three"])
            {
                string[] arr = item.Split('$');
                if (arr[1] == ((String)cmbCategoryTwo.SelectedItem).ToString())
                {
                    lstCategoryThree.Add(arr[0]);
                }
            }
            cmbCategoryThree.Text = "";
            cmbCategoryThree.ItemsSource = lstCategoryThree; }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }
        #endregion
        #region 函数
        public MedicinesForm()
        {
            InitializeComponent();
            BindComboBox();
        }
        void BindComboBox()
        {
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            ServiceClient.GetMedicineCategoryCompleted +=
                (obj, ea) =>
                {
                    Dictionary<String, ObservableCollection<String>> dictionaryTemp = ea.Result;
                    dictionaryTemp["Two"].Insert(0, "全部");
                    cmbCategoryTwo.ItemsSource = dictionaryTemp["Two"];
                    //cmbCategoryThree.ItemsSource = dictionaryTemp["Three"];
                    dictionary = dictionaryTemp;
                };
            ServiceClient.GetMedicineCategoryAsync();
            ServiceClient.CloseAsync();
        }
        
       
        
        #endregion
    }
}
