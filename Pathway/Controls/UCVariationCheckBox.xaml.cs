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
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Views.UserControls
{
   public partial class UCVariationCheckBox : UserControl
   {
      private CP_VariationToPathInfo m_PathVariation;
      public CP_VariationToPathInfo PathVariation
      {
         get { return m_PathVariation; }
      }
      public UCVariationCheckBox(CP_VariationToPathInfo info)
      {
         InitializeComponent();
         m_PathVariation = info;
         this.Loaded += new RoutedEventHandler(UCVariationCheckBox_Loaded);
      }

      private void UCVariationCheckBox_Loaded(object sender, RoutedEventArgs e)
      {
         this.DataContext = m_PathVariation;
      }

      private void CheckBox_Checked(object sender, RoutedEventArgs e)
      {try{
         GetModify(); }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

      }

      private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
      {
         GetModify();
      }

      private void GetModify()
      {
         if (!PathVariation.IsNew)
            PathVariation.IsModify = true;
      }
   }
}
