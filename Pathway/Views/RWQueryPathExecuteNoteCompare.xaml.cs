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
using YidanEHRApplication.Models;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows.Data;

namespace YidanEHRApplication.Views
{
    public partial class RWQueryPathExecuteNoteCompare 
    {
        String noteGUID = "";

        public RWQueryPathExecuteNoteCompare(String NoteGUID, String RyzdName)
        {
         
            InitializeComponent();
            noteGUID = NoteGUID;
            txbBRQK.Text = txbBRQK.Tag + RyzdName;
        }
        #region 事件
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        { try{
            BindGridView("3100", noteGUID);
            }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }
        
        #endregion
        #region 函数
        public void BindGridView(string selectitem, string nodeid)
        {
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            ServiceClient.GetQueryPathExecuteNoteCompareCompleted +=
            (obj, e) =>
            {
                if (e.Error != null) PublicMethod.RadWaringBox(e.Error);
                else
                    GridView.ItemsSource = e.Result.ToList();
            };
            ServiceClient.GetQueryPathExecuteNoteCompareAsync(nodeid);
            ServiceClient.CloseAsync();
        }
    
      
        #endregion

        private void GridView_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
           CP_QueryPathExecuteNoteCompare pe = (CP_QueryPathExecuteNoteCompare)e.DataElement;
            if (pe.IsNew == "-1")
                e.Row.Background = new SolidColorBrush(Colors.Red);
            if (pe.IsNew == "0")
                e.Row.Background = new SolidColorBrush(Colors.Green);
        }

    }
}

