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
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using YidanEHRApplication.DataService;


namespace YidanEHRApplication.NurModule
{
   public partial class UCNurExecItem : UserControl
   {
      private ObservableCollection<CP_NurExecInfo> m_NurExecInfo;

      public ObservableCollection<CP_NurExecInfo> NurExecInfo
      {
         get { return m_NurExecInfo; }
      }

      private bool m_ContentLoaded;

      public UCNurExecItem(ObservableCollection<CP_NurExecInfo> info)
      {
         InitializeComponent();
         m_NurExecInfo = info;
         //this.Loaded += new RoutedEventHandler(UCNurExecItem_Loaded);
      }


      private void UCNurExecItem_Loaded(object sender, RoutedEventArgs e)
      {
          try
          {
              if (m_ContentLoaded)
              {
                  return;
              }
              m_ContentLoaded = true;
              InitControl();
          }
          catch (Exception ex)
          {
              YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
          }
      }

      private void InitControl()
      {
         foreach (CP_NurExecInfo info in m_NurExecInfo)
         {
            this.txtCategoryName.Text = info.LbxhName;

            UCCheckBox checkbox = new UCCheckBox(info);
            this.gridCategoryDetail.Children.Add(checkbox);
         }
      }
   }
}
