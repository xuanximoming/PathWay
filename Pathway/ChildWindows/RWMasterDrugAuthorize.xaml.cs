using System;
using System.Collections.Generic;
using System.Windows;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
namespace YidanEHRApplication.Views.ChildWindows
{
    public partial class RWMasterDrugAuthorize
    {
        #region 属性变量
        ChildMessageBox messagebox = new ChildMessageBox();
        ChildWindowInputMessage m_InputMessageBox = new ChildWindowInputMessage();
        CP_InpatinetList m_Inpatient;
        String _nodeCode = "";

        List<CP_MasterDrugs> cP_MasterDrugs = new List<CP_MasterDrugs>();

        List<CP_MasterDrugs> _CP_MasterDrugsRemind = new List<CP_MasterDrugs>();
        List<CP_MasterDrugs> _CP_MasterDrugsAuthorize = new List<CP_MasterDrugs>();

        /// <summary>
        /// 提醒列表
        /// </summary>
        public List<CP_MasterDrugs> CP_MasterDrugsRemind
        {
            get
            {
                _CP_MasterDrugsRemind = new List<CP_MasterDrugs>();
                foreach (var item in cP_MasterDrugs)
                {
                    if (item.Txfs == "1")
                    {
                        _CP_MasterDrugsRemind.Add(item);
                    }
                }
                return _CP_MasterDrugsRemind;

            }
        }
        /// <summary>
        /// 授权列表
        /// </summary>
        public List<CP_MasterDrugs> CP_MasterDrugsAuthorize
        {
            get
            {
                _CP_MasterDrugsAuthorize = new List<CP_MasterDrugs>();
                foreach (var item in cP_MasterDrugs)
                {
                    if (item.Txfs == "2")
                    {

                        _CP_MasterDrugsAuthorize.Add(item);
                    }
                }
                return _CP_MasterDrugsAuthorize;
            }
        }
        public Boolean IsDrugsAuthorizeAllPass
        {
            get
            {
                Boolean returnbool = true;
                foreach (var item in CP_MasterDrugsAuthorize)
                {
                    if (!item.IsPass)
                    {
                        returnbool = false;
                    }
                }
                return returnbool;
            }
        }
        public String CurrentCdxh = "";
        #endregion
        #region 事件
        /// <summary>
        /// 进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccess_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            if (IsDrugsAuthorizeAllPass || CP_MasterDrugsAuthorize.Count == 0)
            {
                this.DialogResult = true;
                this.Close();
            }
            else
            {
                //DialogParameters parameters = new DialogParameters();
                //parameters.Content = String.Format("提示: {0}", "部分关键药物还未授权，确定要关闭吗？");
                //parameters.Header = "提示";
                //parameters.IconContent = null;
                //parameters.OkButtonContent = "确定";
                //parameters.CancelButtonContent = "取消";
                //parameters.Closed = OnDeleteMasterDrug;//***close处理***

                //RadWindow.Confirm(parameters);

                YidanPathWayMessageBox mess = new YidanPathWayMessageBox("部分关键药物还未授权，确定要关闭吗？", "提示", YiDanMessageBoxButtons.YesNo);
                mess.ShowDialog();
                mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
            }
            //}
            //catch (Exception ex)
            //{
            //    PublicMethod.ClientException(ex, this.GetType().FullName, true);
            //}
        }
        public RWMasterDrugAuthorize(List<CP_MasterDrugs> cP_MasterDrugsPara)
        {
            InitializeComponent();
            cP_MasterDrugs = cP_MasterDrugsPara;
            this.DialogResult = false;

        }
        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //try
            //{

            this.BindGridViewRemind();
            this.BindGridViewAuthorize();
            //}
            //catch (Exception ex)
            //{
            //    PublicMethod.ClientException(ex, this.GetType().FullName, true);
            //}
        }
        private void RadButtonConfig_Click(object sender, RoutedEventArgs e)
        {
            CurrentCdxh = ((RadButton)sender).Tag.ToString();
            RWMasterDrugAuthorizeEntrance Entrance = new RWMasterDrugAuthorizeEntrance(CurrentCdxh);
            Entrance.Closed += new EventHandler<WindowClosedEventArgs>(Entrance_Closed);
            Entrance.ShowDialog();

        }
        private void Entrance_Closed(object sender, WindowClosedEventArgs e)
        {
            foreach (var item in this.cP_MasterDrugs)
            {
                if (item.Cdxh == CurrentCdxh && e.DialogResult == true)
                {
                    item.IsPass = true;
                }
            }
            this.BindGridViewAuthorize();
        }
        #endregion
        #region 函数
        void mess_PageClosedEvent(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    this.DialogResult = false;
                    this.Close();

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void OnDeleteMasterDrug(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                this.DialogResult = false;
                this.Close();
            }
        }
        public void BindGridViewRemind()
        {
            this.GridViewRemind.ItemsSource = CP_MasterDrugsRemind;
        }
        public void BindGridViewAuthorize()
        {
            this.GridViewAuthorize.ItemsSource = CP_MasterDrugsAuthorize;

        }
        #endregion


    }
}
