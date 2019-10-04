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
using System.Windows.Navigation;
using YidanEHRApplication.Models;
using YidanSoft.Tool;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Views
{
    public partial class MasterDrugRoles : Page
    {
        bool isTrue = true;
        private ObservableCollection<CP_MasterDrugRoles> m_listsouceDR;
        #region 事件

        public MasterDrugRoles()
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
                btnSave.IsEnabled = value != OperationState.VIEW;
                txtRoleName.IsEnabled = value != OperationState.VIEW;
            }
        }
        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            //add by luff 2012-080-17
            this.txtRoleName.Text = "";
            CurrentState = OperationState.NEW;
            this.txtRoleName.Focus();
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GridView.SelectedItem == null)
                {
                    //PublicMethod.RadAlterBox("请选择一个角色再点击修改", "提示");
                    PublicMethod.RadAlterBoxRe("请选择一个角色再点击修改", "提示",this.txtRoleName);
                    isTrue = false;
                    return;
                }
                txtRoleName.Text = ((CP_MasterDrugRoles)GridView.SelectedItem).Jsmc;
                CurrentState = OperationState.EDIT;
                this.txtRoleName.Focus();
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
                    PublicMethod.RadAlterBox("请选择一个角色再点击删除", "提示");
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

                this.txtRoleName.Text = "";
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            //add by luff 2012-080-17
            this.txtRoleName.Text = "";
            CurrentState = OperationState.VIEW;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (txtRoleName.Text.Trim() == "")
                {
                    PublicMethod.RadAlterBox("请填写一个角色名称再保存！", "提示");
                    return;
                }
                CP_MasterDrugRoles temp = new CP_MasterDrugRoles();
                temp.ZgdmCj = Global.LogInEmployee.Zgdm;
                temp.ZgdmXg = Global.LogInEmployee.Zgdm;
                temp.Cjsj = DateTime.Now.ToString();
                temp.Xgsj = DateTime.Now.ToString();
                temp.Jsmc = txtRoleName.Text.Trim().ToString();
                if (CurrentState == OperationState.NEW)
                {
                    Boolean isRepeat = false;
                    foreach (var item in (ObservableCollection<CP_MasterDrugRoles>)GridView.ItemsSource)
                    {
                        //add by luff 20121-08-17 判断角色名称是否超过最大输入长度
                        if (txtRoleName.Text.Trim().Length > 20)
                        {
                            if (((CP_MasterDrugRoles)item).Jsmc == txtRoleName.Text.Trim().Substring(0, 20))
                            {
                                isRepeat = true;
                            }
                        }
                        else
                        {
                            isRepeat = false;
                        }
                    }
                    if (isRepeat)
                    {
                        PublicMethod.RadAlterBox("该角色名称已经存在！", "提示");
                        return;
                    }
                    temp.Jsbm = Guid.NewGuid().ToString();
                    AccessDataBase(temp, Operation.InsertAndSelect);
                    return;
                }
                if (CurrentState == OperationState.EDIT)
                {
                    temp.Jsbm = ((CP_MasterDrugRoles)GridView.SelectedItem).Jsbm;
                    AccessDataBase(temp, Operation.UpdateAndSelect);
                    return;
                }
                AccessDataBase(temp, Operation.Select);
                //add by luff 2012-08-17
                this.txtRoleName.Text = string.Empty;
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
                if (!isTrue)
                {
                    isTrue = true;
                    return;
                }
                AccessDataBase(new CP_MasterDrugRoles(), Operation.Select);
                RegisterKeyEvent();
                CurrentState = OperationState.VIEW;
                this.txtRoleName.Focus();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #endregion
        #region 函数
        public void AccessDataBase(CP_MasterDrugRoles parameter, Operation state)
        {

            YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
            Client.MaintainCP_MasterDrugRolesCompleted += (s, e) =>
            {
                GridView.ItemsSource = e.Result;
                //add by luff 2012-08-08
                m_listsouceDR = (ObservableCollection<CP_MasterDrugRoles>)GridView.ItemsSource;
                CurrentState = OperationState.VIEW;
            };
            Client.MaintainCP_MasterDrugRolesAsync(parameter, state.ToString());
            if (CurrentState == OperationState.NEW)
                PublicMethod.RadAlterBox("添加成功！", "提示");
            if (CurrentState == OperationState.EDIT)
                PublicMethod.RadAlterBox("修改成功！", "提示");


        }

        void mess_PageClosedEvent(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    CP_MasterDrugRoles temp = new CP_MasterDrugRoles() { Jsbm = ((CP_MasterDrugRoles)GridView.SelectedItem).Jsbm };
                    YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                    //add by luff 2012-08-08
                    Client.MaintainCP_MasterDrugRolesCompleted += (s, ea) =>
                    {
                        if (ea.Error == null)
                        {
                            if (ea.Result == null)
                            {
                                PublicMethod.RadAlterBox("该角色下有对应功能权限或用户，请先删除相对应的功能权限或用户角色！！", "提示");
                                return;
                            }
                            else
                            {
                                GridView.ItemsSource = ea.Result;

                                m_listsouceDR = (ObservableCollection<CP_MasterDrugRoles>)GridView.ItemsSource;
                                CurrentState = OperationState.VIEW;
                            }
                        }
                    };
                    Client.MaintainCP_MasterDrugRolesAsync(temp, Operation.DeleteAndSelect.ToString());

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
                CP_MasterDrugRoles temp = new CP_MasterDrugRoles() { Jsbm = ((CP_MasterDrugRoles)GridView.SelectedItem).Jsbm };
                YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                //add by luff 2012-08-08
                Client.MaintainCP_MasterDrugRolesCompleted += (s, ea) =>
                {
                    if (ea.Error == null)
                    {
                        if (ea.Result == null)
                        {
                            PublicMethod.RadAlterBox("该角色下有对应功能权限或用户，请先删除相对应的功能权限或用户角色！！", "提示");
                            return;
                        }
                        else
                        {
                            GridView.ItemsSource = ea.Result;
                            
                            m_listsouceDR = (ObservableCollection<CP_MasterDrugRoles>)GridView.ItemsSource;
                            CurrentState = OperationState.VIEW;
                        }
                    }
                };
                Client.MaintainCP_MasterDrugRolesAsync(temp, Operation.DeleteAndSelect.ToString());
               
               
            }
        }
        #endregion

        #region 查询 add by luff 2012-08-07
        //查询重置
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {

            this.txt_JSMC.Text = string.Empty;
            this.txt_TJR.Text = string.Empty;
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            string para = this.txt_JSMC.Text;
      
            string para1 = this.txt_TJR.Text;
            //集合类型初始化
            List<CP_MasterDrugRoles> t_listsouce = m_listsouceDR.ToList();


            if (para.Trim().Length > 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.Jsmc.IndexOf(para) > -1).ToList();

            }
           
            if (para1.Trim().Length > 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.ZgdmCjName.IndexOf(para1) > -1).ToList();

            }

 


            GridView.ItemsSource = t_listsouce.ToList();

        }


        #endregion
        #region  输入框加回车事件 add by luff 2012-080-07
        /// <summary>
        /// 查询框回车查询事件 add by luff 2012-08-07
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RegisterKeyEvent()
        {

            btnSave.KeyUp += new KeyEventHandler(btnSave_KeyUp);

            this.txt_JSMC.KeyUp += new KeyEventHandler(tbQuery_KeyUp);
           
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
    }
}
