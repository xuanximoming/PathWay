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

using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using System.Collections.ObjectModel;
using YidanEHRApplication.Controls;

namespace YidanEHRApplication.NurModule
{
    /// <summary>
    /// Interaction logic for NurExecToPath.xaml
    /// </summary>
    public partial class NurExecToPath
    {
        public NurExecToPath()
        {
            InitializeComponent();
        }
        //22672293
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.inpatientList.HideControl(false);
                this.inpatientList.DoctorID = "";
                this.inpatientList.AfterNavigateToPage += new Controls.UCInpatientListControl.NavigateToPage(inpatientList_AfterNavigateToPage);
                this.inpatientList.InitPage();//初始化数据
                inpatientList.btnEnable.Visibility = Visibility.Visible;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void inpatientList_AfterNavigateToPage(object sender, RoutedEventArgs e)
        {
            UCInpatientListControl.OpreateEventArgs Args = (UCInpatientListControl.OpreateEventArgs)e;
            if ((CP_InpatinetList)Args.Datas != null)
            {
                //nurEnforceTab.IsEnabled = true;
                //this.nurPahtEnforce.InitDate((CP_InpatinetList)Args.Datas);
                if (!this.nurYzzx.InitDate((CP_InpatinetList)Args.Datas))
                {
                    return;
                }
                this.nurHlzx.InitDate((CP_InpatinetList)Args.Datas);
                this.notesInfo.InitDate((CP_InpatinetList)Args.Datas);
                //this.inpatientList.btnEnable.IsEnabled = true;
                nurYzzxTab.IsEnabled = true;
                nurHlzxTab.IsEnabled = true;
                nurHljldTab.IsEnabled = true;
                if (this.radTabControlPathManager.SelectedIndex == 0)
                {
                    this.radTabControlPathManager.SelectedIndex = 1;
                }
                btnEnable_Click(sender, e);
            }
            else
            {
                nurYzzxTab.IsEnabled = false;
                nurHlzxTab.IsEnabled = false;
                nurHljldTab.IsEnabled = false;
            }
        }

        //public void AfterNavigateToPage( RoutedEventArgs e)
        //{
        //    inpatientList_AfterNavigateToPage(null, e);
        //}



        public void btnEnable_Click(object sender, RoutedEventArgs e)
        {
            UCInpatientListControl.OpreateEventArgs Args = (UCInpatientListControl.OpreateEventArgs)e;
            if ((CP_InpatinetList)Args.Datas != null)
            {

                this.nurHlzx.InitDate((CP_InpatinetList)Args.Datas);
                this.notesInfo.InitDate((CP_InpatinetList)Args.Datas);
                if (this.radTabControlPathManager.SelectedIndex == 0)
                {
                    this.radTabControlPathManager.SelectedIndex = 1;
                    nurYzzxTab.IsEnabled = true;
                    nurHlzxTab.IsEnabled = true;
                    nurHljldTab.IsEnabled = true;
                }
                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }


        private List<String> GetCategoryList(ObservableCollection<CP_NurExecInfo> infos)
        {
            List<String> listInfo = new List<string>();
            foreach (CP_NurExecInfo info in infos)
            {
                if (!listInfo.Contains(info.Lbxh))
                    listInfo.Add(info.Lbxh);
            }
            return listInfo;
        }

        private void radTabControlPathManager_SelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {
            
            //if (radTabControlPathManager.SelectedItem == null)
            //    return;

            //foreach (RadTabItem item in radTabControlPathManager.Items)
            //{
            //    item.Background = new SolidColorBrush(Color.FromArgb(181, 214, 239,0));
            //}
            //RadTabItem radtab = (RadTabItem)radTabControlPathManager.SelectedItem;
            //radtab.Background = new SolidColorBrush(Color.FromArgb(239, 247, 255, 0));

        }
    }
}
