using DrectSoft.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Views
{
    public partial class MasterDrugs : Page
    {
        /// <summary>
        /// 列表数据源
        /// </summary>
        private ObservableCollection<CP_MasterDrugs> m_listsouce;

        #region 事件


        public MasterDrugs()
        {
            InitializeComponent();

        }
        OperationState _CurrentState = OperationState.VIEW;
        OperationState CurrentState
        {
            get { return _CurrentState; }
            set
            {
                _CurrentState = value;
                btnAdd.IsEnabled = value == OperationState.VIEW;
                btnUpdate.IsEnabled = value == OperationState.VIEW;
                btnClear.IsEnabled = value != OperationState.VIEW;
                btnDel.IsEnabled = value == OperationState.VIEW;
                autoCompleteBoxOrder.IsEnabled = value == OperationState.NEW;
                btnSave.IsEnabled = value != OperationState.VIEW;
                rdbTxfsSQ.IsEnabled = value != OperationState.VIEW;
                rdbTxfsTX.IsEnabled = value != OperationState.VIEW;

                //btnAlter.IsEnabled = value == OperationState.VIEW;
                //btnAuthorization.IsEnabled = value == OperationState.VIEW;
            }
        }
        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CurrentState = OperationState.NEW;
            this.autoCompleteBoxOrder.Text = "";
            autoCompleteBoxOrder.SelectedItem = null;
            //CP_MasterDrugs drug = new CP_MasterDrugs();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (GridView.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择一个药品", "提示");
                    return;
                }
                CurrentState = OperationState.EDIT;
                autoCompleteBoxOrder.SelectedItem = ((ObservableCollection<CP_PlaceOfDrug>)autoCompleteBoxOrder.ItemsSource).First(cp => cp.Cdxh.ToString().Equals(((CP_MasterDrugs)GridView.SelectedItem).Cdxh));
                if (((CP_MasterDrugs)GridView.SelectedItem).Txfs == "1")
                    rdbTxfsTX.IsChecked = true;
                else
                    rdbTxfsSQ.IsChecked = true;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (GridView.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择一个药品再点击删除", "提示");
                    return;
                }
                //DialogParameters parameters = new DialogParameters();
                //parameters.Content = String.Format("{0}", "确定删除吗？删除后不能恢复!");
                //parameters.Header = "提示";
                //parameters.IconContent = null;
                //parameters.OkButtonContent = "确定";
                //parameters.CancelButtonContent = "取消";
                //parameters.Closed = OnDeleteMasterDrug;//***close处理***
                //RadWindow.Confirm(parameters);
                YidanPathWayMessageBox mess = new YidanPathWayMessageBox("确定删除吗？删除后不能恢复!", "提示", YiDanMessageBoxButtons.YesNo);
                mess.ShowDialog();
                mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
                //PublicMethod.pr
                //if (CurrentState == OperationState.NEW)
                //{
                //    AccessDataBase(drug, Operation.InsertAndSelect);
                //    return;
                //}
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            this.autoCompleteBoxOrder.Text = "";
            autoCompleteBoxOrder.SelectedItem = null;
            CurrentState = OperationState.VIEW;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (autoCompleteBoxOrder.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择药品", "提示");
                    return;
                }
                CP_MasterDrugs drug = new CP_MasterDrugs();
                drug.Cdxh = ((CP_PlaceOfDrug)autoCompleteBoxOrder.SelectedItem).Cdxh.ToString();
                drug.Txfs = rdbTxfsTX.IsChecked == true ? "1" : "2";
                drug.ZgdmCj = Global.LogInEmployee.Zgdm;
                drug.ZgdmXg = Global.LogInEmployee.Zgdm;
                drug.Cjsj = DateTime.Now.ToString();
                drug.Xgsj = DateTime.Now.ToString();

                if (CurrentState == OperationState.NEW)
                {
                    Boolean isRepeat = false;
                    foreach (var item in (ObservableCollection<CP_MasterDrugs>)GridView.ItemsSource)
                    {
                        if (((CP_MasterDrugs)item).Cdxh == drug.Cdxh)
                        {
                            isRepeat = true;
                        }
                    }
                    if (isRepeat)
                    {
                        PublicMethod.RadAlterBox("该药品已经存在！", "提示");
                        return;
                    }
                    AccessDataBase(drug, Operation.InsertAndSelect);
                    return;
                }
                if (CurrentState == OperationState.EDIT)
                {
                    AccessDataBase(drug, Operation.UpdateAndSelect);
                    return;
                }
                AccessDataBase(drug, Operation.Select);
                this.autoCompleteBoxOrder.Text = "";
                autoCompleteBoxOrder.SelectedItem = null;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }


        }

        private void btnTxtClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                AccessDataBase(new CP_MasterDrugs(), Operation.Select);
                InitDrugInfo();
                RegisterKeyEvent();
                CurrentState = OperationState.VIEW;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #endregion
        #region 函数
        public void AccessDataBase(CP_MasterDrugs drug, Operation state)
        {

            YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
            Client.MaintainCP_MasterDrugCompleted += (s, e) =>
            {
                GridView.ItemsSource = e.Result;
                m_listsouce = (ObservableCollection<CP_MasterDrugs>)GridView.ItemsSource;
                CurrentState = OperationState.VIEW;
            };
            Client.MaintainCP_MasterDrugAsync(drug, state.ToString());
            if (CurrentState == OperationState.NEW)
                PublicMethod.RadAlterBox("添加成功！", "提示");
            if (CurrentState == OperationState.EDIT)
                PublicMethod.RadAlterBox("修改成功！", "提示");


        }
        /// <summary>
        /// 初始化药品数据
        /// </summary>
        private void InitDrugInfo()
        {
            YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
            Client.GetDrugInfoCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        autoCompleteBoxOrder.ItemsSource = e.Result;
                        autoCompleteBoxOrder.ItemFilter = OrderFilter;

                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                        return;
                    }
                };
            Client.GetDrugInfoAsync(null);
            Client.CloseAsync();
        }
        public bool OrderFilter(string strFilter, object item)
        {
            CP_PlaceOfDrug deptList = (CP_PlaceOfDrug)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())));

        }

        void mess_PageClosedEvent(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    CP_MasterDrugs temp = new CP_MasterDrugs() { Cdxh = ((CP_MasterDrugs)GridView.SelectedItem).Cdxh };
                    AccessDataBase(temp, Operation.DeleteAndSelect);
                    PublicMethod.RadAlterBox("删除成功！", "提示");
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
                CP_MasterDrugs temp = new CP_MasterDrugs() { Cdxh = ((CP_MasterDrugs)GridView.SelectedItem).Cdxh };
                AccessDataBase(temp, Operation.DeleteAndSelect);
                PublicMethod.RadAlterBox("删除成功！", "提示");
            }
        }
        #endregion

        #region  输入框加回车事件
        /// <summary>
        /// 查询框回车查询事件 add by luff 2012-08-07
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterKeyEvent()
        {

            btnSave.KeyUp += new KeyEventHandler(btnSave_KeyUp);

            this.txt_YPMC.KeyUp += new KeyEventHandler(tbQuery_KeyUp);
            this.txt_CJMC.KeyUp += new KeyEventHandler(tbQuery_KeyUp);
            this.txt_TJR.KeyUp += new KeyEventHandler(tbQuery_KeyUp);
        }

        private void btnSave_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSave_Click(null, null);
        }
        private void tbQuery_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnQuery_Click(null, null);
        }
        #endregion

        #region 查询关键字相关事件方法 add by luff 2012-08-07
        /// <summary>
        /// 查询事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            string para = this.txt_YPMC.Text;
            string para1 = this.txt_CJMC.Text;
            string para2 = this.txt_TJR.Text;
            //集合类型初始化
            List<CP_MasterDrugs> t_listsouce = m_listsouce.ToList();


            if (para.Trim().Length > 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.Ypmc.IndexOf(para) > -1).ToList();

            }
            //else
            //{
            //    //GridView.ItemsSource = m_listsouce.Select(s => s).Where(s => s.Ypmc.IndexOf(para) > 0).ToList();
            //    //List<CP_MasterDrugs> list = ((ObservableCollection<CP_MasterDrugs>)GridView.ItemsSource).ToList();
            //    t_listsouce = m_listsouce.ToList();
            //}
            if (para1.Trim().Length > 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.Cjmc.IndexOf(para1) > -1).ToList();

            }


            if (para2.Trim().Length > 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.ZgdmCjName.IndexOf(para2) > -1).ToList();

            }


            GridView.ItemsSource = t_listsouce.ToList();

            //if(para.Trim().Length==0 && para1.Trim().Length==0 && para2.Trim().Length==0)
            //{
            //    GridView.ItemsSource = m_listsouce;
            //}
        }


        //查询重置
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {

            this.txt_YPMC.Text = string.Empty;
            this.txt_CJMC.Text = string.Empty;
            this.txt_TJR.Text = string.Empty;
        }
        #endregion
    }
}
