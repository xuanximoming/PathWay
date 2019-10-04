using DrectSoft.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
namespace YidanEHRApplication.Views
{
    public partial class MasterDrug2User : Page
    {
        bool isTrue = true;
        #region 属性变量
        YidanEHRDataServiceClient serviceCon;
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
                cbxRole.IsEnabled = value != OperationState.NEW;
                cbxUser.IsEnabled = value != OperationState.NEW;
                //btnDel.IsEnabled = value == OperationState.VIEW;
                btnSave.IsEnabled = value != OperationState.VIEW;
                cbxUser.IsEnabled = value == OperationState.NEW;
                cbxRole.IsEnabled = value == OperationState.NEW;
                this.GridViewRole.IsEnabled = value != OperationState.VIEW;
            }
        }
        static List<CP_MasterDrug2User> CP_MasterDrug2User = new List<CP_MasterDrug2User>();
        #endregion

        #region 事件
        public MasterDrug2User()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(UserRoleManager_Loaded);
            this.cbxUser.IsEnabled = false;
            this.cbxRole.IsEnabled = false;
        }
        void UserRoleManager_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isTrue)
                {
                    isTrue = true;
                    return;
                }
                CurrentState = OperationState.VIEW;
                BindGridView();
                BindcbxRole();
                BindcbxUser();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GridView.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择一行记录，再点击修改！", "提示");
                    return;
                }
                cbxUser.SelectedValue = ((CP_MasterDrug2User)GridView.SelectedItem).Zgdm;
                //BindcbxUser();
                CurrentState = OperationState.EDIT;
                this.cbxUser.Focus();
                cbxUser.IsDropDownOpen = true;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentState = OperationState.NEW;
                cbxUser.SelectedItem = null;
                cbxUser.Text = "";
                this.cbxUser.Focus();
                cbxUser.IsDropDownOpen = true;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentState = OperationState.VIEW;
                cbxUser.SelectedItem = null;
                cbxUser.Text = "";

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            ObservableCollection<CP_MasterDrug2User> Roleusers;
            try
            {
                if (cbxUser.SelectedItem == null)
                {
                    //PublicMethod.RadAlterBox("请选择一个用户！","提示");
                    PublicMethod.RadAlterBoxRe("请选择一个用户！", "提示", cbxUser);
                    isTrue = false;
                    return;
                }
                String Zgdm = ""; ;
                if (CurrentState == OperationState.NEW)
                    Zgdm = ((CP_Employee)cbxUser.SelectedItem).Zgdm;

                Roleusers = new ObservableCollection<CP_MasterDrug2User>();
                foreach (var item in this.GridViewRole.SelectedItems)
                {
                    CP_MasterDrugRoles role = ((CP_MasterDrugRoles)item);
                    CP_MasterDrug2User Roleuser = new CP_MasterDrug2User();
                    Roleuser.Zgdm = Zgdm;
                    Roleuser.Jsbm = role.Jsbm;
                    Roleuser.Cjsj = DateTime.Now.ToString();
                    Roleuser.Xgsj = DateTime.Now.ToString();
                    Roleuser.ZgdmCj = Global.LogInEmployee.Zgdm;
                    Roleuser.ZgdmXg = Global.LogInEmployee.Zgdm;

                    Roleusers.Add(Roleuser);

                }

                if (CurrentState == OperationState.EDIT)
                    Zgdm = ((CP_MasterDrug2User)this.GridView.SelectedItem).Zgdm;
                Roleusers = new ObservableCollection<CP_MasterDrug2User>();
                foreach (var item in this.GridViewRole.SelectedItems)
                {
                    CP_MasterDrugRoles role = ((CP_MasterDrugRoles)item);
                    CP_MasterDrug2User Roleuser = new CP_MasterDrug2User();
                    Roleuser.Zgdm = Zgdm;
                    Roleuser.Jsbm = role.Jsbm;
                    Roleuser.Cjsj = DateTime.Now.ToString();
                    Roleuser.Xgsj = DateTime.Now.ToString();
                    Roleuser.ZgdmCj = Global.LogInEmployee.Zgdm;
                    Roleuser.ZgdmXg = Global.LogInEmployee.Zgdm;

                    Roleusers.Add(Roleuser);

                }
                YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                Client.MaintainP_MasterDrug2UserUpdateCompleted += (sb, eb) =>
                {

                    BindGridView();
                    PublicMethod.RadAlterBox("保存成功！", "提示");

                };
                Client.MaintainP_MasterDrug2UserUpdateAsync(Roleusers, Zgdm);
                CurrentState = OperationState.VIEW;

                cbxUser.SelectedItem = null;
                cbxUser.Text = "";

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnTxtClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void GridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                BindcbxRole();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void GridViewRole_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
            CP_MasterDrugRoles t = (CP_MasterDrugRoles)e.DataElement;
            List<CheckBox> listtest = (List<CheckBox>)(this.GridViewRole.ChildrenOfType<CheckBox>().ToList());
            if (listtest.Count > 0)
                if (t.IsCheck == "1")
                    listtest[listtest.Count - 1].IsChecked = true;
        }
        #endregion

        #region 函数
        /// <summary>
        /// 获取用户角色下拉框
        /// </summary>
        private void BindcbxUser()
        {
            YidanEHRDataServiceClient GetDepartmentListInfoClient = PublicMethod.YidanClient;
            GetDepartmentListInfoClient.GetCP_EmployeeByKeyWordsCompleted +=
            (obj, e) =>
            {
                if (e.Error == null)
                {
                    this.cbxUser.ItemsSource = e.Result;
                }
                else
                {
                    PublicMethod.RadWaringBox(e.Error);
                }
            };
            GetDepartmentListInfoClient.GetCP_EmployeeByKeyWordsAsync("");
            GetDepartmentListInfoClient.CloseAsync();
        }
        /// <summary>
        /// 绑定角色下拉框
        /// </summary>
        private void BindcbxRole()
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.MaintainCP_MasterDrugRolesCompleted += (s, e) =>
            {
                if (e.Error == null)
                {
                    List<CP_MasterDrugRoles> cP_MasterDrugRoles = new List<CP_MasterDrugRoles>();
                    cP_MasterDrugRoles = e.Result.ToList();
                    if (GridView.SelectedItem != null)
                    {
                        foreach (var item in cP_MasterDrugRoles)
                        {
                            foreach (var item2 in CP_MasterDrug2User)
                            {
                                if (item.Jsbm == item2.Jsbm && item2.Zgdm == ((CP_MasterDrug2User)GridView.SelectedItem).Zgdm)
                                {
                                    item.IsCheck = "1";
                                }
                            }
                        }
                    }
                    GridViewRole.ItemsSource = cP_MasterDrugRoles;
                }
            };
            serviceCon.MaintainCP_MasterDrugRolesAsync(null, Operation.Select.ToString());
        }

        private void BindGridView()
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.MaintainP_MasterDrug2UserCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            GridView.ItemsSource = e.Result;
                            CP_MasterDrug2User = e.Result.ToList();

                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
            serviceCon.MaintainP_MasterDrug2UserAsync(null, Operation.Select.ToString());
            serviceCon.CloseAsync();
        }
        #endregion
    }
}
