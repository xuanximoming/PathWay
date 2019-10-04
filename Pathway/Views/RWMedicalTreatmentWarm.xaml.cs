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

using YidanEHRApplication.Models;
using System.Collections.ObjectModel;
using Telerik.Windows.Data;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using YidanSoft.Tool;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Views
{
    public partial class RWMedicalTreatmentWarm
    {

        public String Jddm { get; set; }
        public String Ljdm { get; set; }
        public String Syxh { get; set; }
        public String Ljxh { get; set; }
        public String ID { get; set; }
        List<CP_MedicalTreatmentWarm> CP_MedicalTreatmentWarmTemps = new List<CP_MedicalTreatmentWarm>();
        public RWMedicalTreatmentWarm()
        {
            InitializeComponent();
        }
        public RWMedicalTreatmentWarm(String jddm, String ljdm, String syxh, String ljxh)
        {
            InitializeComponent();
            Jddm = jddm;
            Ljdm = ljdm;
            Syxh = syxh;
            Ljxh = ljxh;

        }
        /// <summary>
        ///表示插入提示信息的方法
        /// </summary>
        /// <returns>插入是否成功</returns>
        public void UpdateAndSelectCP_MedicalTreatmentWarm()
        {
            radBusyIndicator.IsBusy = true;
            Object[] objArr = new Object[2];
           
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            ServiceClient.UpdateAndSelectCP_MedicalTreatmentWarmCompleted += (sender, e) =>
            {
                CP_MedicalTreatmentWarmTemps = e.Result.ToList().Select(s => s).Where(s => s.Txlx == "1" || s.Txlx == "2").ToList();
                //BindmnuWarmContent(CP_MedicalTreatmentWarmGroupByTxlxTemps);
                grdCP_MedicalTreatmentWarm.ItemsSource = CP_MedicalTreatmentWarmTemps;

                List<CP_MedicalTreatmentWarm> CP_MedicalTreatmentWarmTianShuTemps = CP_MedicalTreatmentWarmTemps.Select(s => s).Where(s => s.Txlx == "3").ToList();
                List<CP_MedicalTreatmentWarm> CP_MedicalTreatmentWarmFeiYongTemps = CP_MedicalTreatmentWarmTemps.Select(s => s).Where(s => s.Txlx == "4").ToList();
                CP_MedicalTreatmentWarm tianshu = CP_MedicalTreatmentWarmTianShuTemps.Count > 0 ? CP_MedicalTreatmentWarmTianShuTemps[0] : null;
                CP_MedicalTreatmentWarm feiyong = CP_MedicalTreatmentWarmFeiYongTemps.Count > 0 ? CP_MedicalTreatmentWarmFeiYongTemps[0] : null;
                txttianshu.Text=tianshu==null?"":tianshu.TxlxName+"【"+tianshu.mc+"】"+(ConvertMy.ToInt32( tianshu.ID)>0?tianshu.TxlxName+"超过"+tianshu.ID+"天":"");
                txtfeiyong.Text = feiyong == null ? "" : feiyong.TxlxName + "【" + feiyong.mc + "】" + (ConvertMy.ToInt32(feiyong.ID) > 0 ? feiyong.TxlxName + "超过" + feiyong.ID + "元" : "");
                radBusyIndicator.IsBusy = false;
            };
            ServiceClient.UpdateAndSelectCP_MedicalTreatmentWarmAsync(ID, Jddm, Ljdm, Syxh, Ljxh);
            ServiceClient.CloseAsync();

        }

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            UpdateAndSelectCP_MedicalTreatmentWarm();
        }

        private void chkHead_Checked(object sender, RoutedEventArgs e)
        {
            foreach (CP_MedicalTreatmentWarm item in CP_MedicalTreatmentWarmTemps)
            {

                //if (item.ID==item.ID )
                //{
                    item.TxztBoolean =(Boolean) ((CheckBox)sender).IsChecked;
                //}
            }
            grdCP_MedicalTreatmentWarm.ItemsSource = CP_MedicalTreatmentWarmTemps;
            //grdCP_MedicalTreatmentWarm.UpdateLayout();
        }



        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ID = null;
            foreach (CP_MedicalTreatmentWarm item in CP_MedicalTreatmentWarmTemps)
            {

                if (item.TxztBoolean == true)
                    {
                        if (ID == null)
                            ID = item.ID;
                        else
                            ID = ID + "," + item.ID;
                    }
            }
            if (ID == null)
            { 
                RadWindow wnd=new RadWindow();
                PublicMethod.ShowAlertWindow(ref wnd, "请至少选择一项后再点击保存", "提示", null, null);
            }
            else
            UpdateAndSelectCP_MedicalTreatmentWarm();
            //grdCP_MedicalTreatmentWarm.UpdateLayout();

        }

        private void chkHead_Unchecked(object sender, RoutedEventArgs e)
        {
            foreach (CP_MedicalTreatmentWarm item in CP_MedicalTreatmentWarmTemps)
            {

                item.TxztBoolean = (Boolean)((CheckBox)sender).IsChecked;
            } 
            grdCP_MedicalTreatmentWarm.ItemsSource = CP_MedicalTreatmentWarmTemps;
            //grdCP_MedicalTreatmentWarm.UpdateLayout();
        }

        private void grdCP_MedicalTreatmentWarm_RowLoaded(object sender, RowLoadedEventArgs e)
        {
        }

        private void chkRow_Checked(object sender, RoutedEventArgs e)
        {
            //grdCP_MedicalTreatmentWarm.UpdateLayout();
        }

        private void chkRow_Unchecked(object sender, RoutedEventArgs e)
        {
            //grdCP_MedicalTreatmentWarm.UpdateLayout();
        }



    }
}

