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
using YidanEHRApplication.Models;
using System.Collections.ObjectModel;
using YidanSoft.Tool;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Views.ChildWindows
{
    /// <summary>
    /// Interaction logic for RadChildWindowQuitPath.xaml
    /// </summary>
    public partial class RWQuitPath
    {
        /// <summary>
        /// 退出时结点ID
        /// </summary>
        private string m_PahtDetailID = string.Empty;

        /// <summary>
        /// 当前病患
        /// </summary>
        private CP_InpatinetList m_InpatientList = new CP_InpatinetList();

        private ObservableCollection<CP_VariantRecords> m_VariantRecords = new ObservableCollection<CP_VariantRecords>();

        /// <summary>
        /// 原因
        /// </summary>
        public ObservableCollection<CP_VariantRecords> VariantRecords
        {
            get { return m_VariantRecords; }
            private set { m_VariantRecords = value; }
        }

        public RWQuitPath(CP_InpatinetList inpatientList, string strPahtDetailID)
        {
            InitializeComponent();
            m_InpatientList = inpatientList;
            m_PahtDetailID = strPahtDetailID;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        { 
        }

        //private void InitPage()
        //{
        //    radBusyIndicator.IsBusy = true;
        //    YidanEHRDataServiceClient client = PublicMethod.YidanClient;
        //    client.GetPathConditionListByLjdmCompleted +=
        //      (obj, ea) =>
        //      {
        //          radBusyIndicator.IsBusy = false;
        //          if (ea.Error == null)
        //          {
        //              List<CP_PathConditionList> listInfo = ea.Result.ToList();
        //              for (int i = listInfo.Count - 1; i >= 0; i--)
        //              {
        //                  CP_PathConditionList info = listInfo[i];
        //                  if (info.Tjlb == 1)
        //                      listInfo.RemoveAt(i);
        //              }
        //              radGridConditonList.ItemsSource = listInfo;
        //          }
        //          else
        //          {
        //              PublicMethod.RadWaringBox(ea.Error);
        //          }
        //      };

        //    client.GetPathConditionListByLjdmAsync(m_InpatientList.Ljdm);
        //    client.CloseAsync();
        //}

        
        private void radButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                VariantRecords = new ObservableCollection<CP_VariantRecords>();
                if (Check())
                {
                    GetVariationConditon();
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    PublicMethod.RadAlterBox("请选择或者填写原因", "退出原因");
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        private void radButtonQuit_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        /// <summary>
        /// CHECK
        /// </summary>
        /// <returns></returns>
        private bool Check()
        {
            if (this.textBoxOterReason.Text.Trim() == string.Empty)
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// 得到所有原因
        /// </summary>
        private void GetVariationConditon()
        {
            //foreach (var items in radGridConditonList.SelectedItems)
            //{
            //    CP_PathConditionList list = items as CP_PathConditionList;
            //    CP_VariantRecords info = new CP_VariantRecords();
            //    info.Syxh = ConvertMy.ToDecimal(m_InpatientList.Syxh);
            //    info.Ljdm = m_InpatientList.Ljdm;
            //    info.Mxdm = m_PahtDetailID;
            //    info.Ypdm = string.Empty;
            //    info.Bylb = Convert.ToString((int)VariationType.Quit);
            //    info.Bylx = Convert.ToString((int)VariationCategory.Quit);
            //    info.Bynr = "退出路径";
            //    info.Bydm = list.Tjdm;
            //    info.Byyy = list.Tjmc;
            //    info.Bysj = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            //    VariantRecords.Add(info);
            //}
            if (!string.IsNullOrEmpty(textBoxOterReason.Text.Trim()))
            {
                CP_VariantRecords info = new CP_VariantRecords();
                info.Syxh = ConvertMy.ToDecimal(m_InpatientList.Syxh);
                info.Ljdm = m_InpatientList.Ljdm;
                info.Mxdm = m_PahtDetailID;
                info.Ypdm = string.Empty;
                info.Bylb = Convert.ToString((int)VariationType.Quit);
                info.Bylx = Convert.ToString((int)VariationCategory.Quit);
                info.Bynr = "退出路径";
                info.Bydm = "9999";
                info.Byyy = textBoxOterReason.Text.Trim();
                info.Bysj = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                VariantRecords.Add(info);
            }
        }
    }
}
