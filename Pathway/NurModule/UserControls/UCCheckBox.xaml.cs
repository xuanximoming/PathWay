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

namespace YidanEHRApplication.NurModule
{
   public partial class UCCheckBox : UserControl
   {
      private CP_NurExecInfo m_NurExecInfo;
      public CP_NurExecInfo NurExecInfo
      {
         get { return m_NurExecInfo; }
      }

      public UCCheckBox(CP_NurExecInfo info)
      {
         InitializeComponent();
         m_NurExecInfo = info;
         this.Loaded += new RoutedEventHandler(UCCheckBox_Loaded);
      }

      private void UCCheckBox_Loaded(object sender, RoutedEventArgs e)
      {
         this.DataContext = m_NurExecInfo;
      }

      private void CheckBox_Checked(object sender, RoutedEventArgs e)
      {
         //MessageBox.Show(m_NurExecInfo.IsSelected.ToString());
         CheckBox ck = sender as CheckBox;
         if (!m_NurExecInfo.IsNew)
            m_NurExecInfo.IsModify = true;
      }

      private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
      {
         //MessageBox.Show(m_NurExecInfo.IsSelected.ToString());
         CheckBox ck = sender as CheckBox;
         if (!m_NurExecInfo.IsNew)
            m_NurExecInfo.IsModify = true;
      }
   }
}
