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
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Views
{
    public partial class ChildWindowChooseNextActivity : ChildWindow
    {
        public List<WorkFlowActivity> NextWorkFlowActivity
        { get; set; }

        private string m_SelectActivityId = string.Empty;
        public string SelectActivityId
        {
            get { return m_SelectActivityId; }
        }
        public ChildWindowChooseNextActivity()
        {
            InitializeComponent();
        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        { try{
            InitPage();
             }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.radComboBoxWorkActivity.SelectedValue == null)
                {
                    PublicMethod.RadAlterBox("请选取路径执行节点", "选取路径执行节点");
                    return;
                }
                m_SelectActivityId = this.radComboBoxWorkActivity.SelectedValue.ToString();
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

        private void InitPage()
        {
            InitCombBox();
        }

        private void InitCombBox()
        {
            radComboBoxWorkActivity.ItemsSource = NextWorkFlowActivity;
            radComboBoxWorkActivity.EmptyText = "选取路径执行节点...";
        }
    }
}

