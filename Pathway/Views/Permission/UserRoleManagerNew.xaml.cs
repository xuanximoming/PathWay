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
using YidanEHRApplication.DataService;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;
using Telerik.Windows.Controls.GridView;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls;

namespace YidanEHRApplication.Views.Permission
{
    public partial class UserRoleManagerNew : Page
    {
        YidanEHRDataServiceClient serviceCon;

        /// <summary>
        /// 用于存储列表中选中的行记录
        /// </summary>
        static PE_UserRoleList m_userrole_fun = new PE_UserRoleList();

        EditState m_funstate;
        public UserRoleManagerNew()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(UserRoleManager_Loaded);
        }

        void UserRoleManager_Loaded(object sender, RoutedEventArgs e)
        {
             try{
            m_funstate = EditState.View;
            BindCombox();
            BindGridView();
            BindBtnState(); }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /// <summary>
        /// 绑定页面下拉框值
        /// </summary>
        private void BindCombox()
        {
            BindcbxUser();
            BindcbxRole();
        }

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
            YidanEHRDataServiceClient GetDepartmentListInfoClient = PublicMethod.YidanClient;
            GetDepartmentListInfoClient.GetPE_RoleListCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        this.cbxRole.ItemsSource = e.Result;
                        GridViewRole.ItemsSource = e.Result;
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };

            GetDepartmentListInfoClient.GetPE_RoleListAsync();
            GetDepartmentListInfoClient.CloseAsync();
        }

                

        #region 函数

        private void BindGridView()
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetPE_UserRoleListCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            GridView.ItemsSource = e.Result;
                            //将第一条信息绑定到编辑区域
                            //if (e.Result.ToList().Count > 0)
                            //{
                            //    m_pe_fun = e.Result.ToList()[0];

                            BindPE_Fun(null);
                            //}
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };

            serviceCon.GetPE_UserRoleListAsync();
            serviceCon.CloseAsync();
        }

                

        /// <summary>
        /// 绑定页面中编辑信息
        /// </summary>
        /// <param name="pe_fun">实体，如果为空编辑区域值为空</param>
        private void BindPE_Fun(PE_UserRoleList userrole_fun)
        {
            if (userrole_fun != null)
            {
                this.cbxUser.SelectedValue = userrole_fun.Zgdm;
                this.cbxRole.SelectedValue = userrole_fun.RoleCode;
                BindListBox(userrole_fun.Zgdm);

            }
            else
            {
                m_userrole_fun = new PE_UserRoleList();
                this.cbxUser.SelectedItem = null;
                this.cbxRole.SelectedItem = null;
            }
        }

        /// <summary>
        /// 根据UserID绑定用户角色
        /// </summary>
        /// <param name="UserID"></param>
        private void BindListBox(string UserID)
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetUserRoleByUserIDCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        GridViewRole.ItemsSource = e.Result.ToList();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            serviceCon.GetUserRoleByUserIDAsync(UserID);
            serviceCon.CloseAsync();
        }

                
	

        private void BindBtnState()
        {
            if (m_funstate == EditState.Add)
            {
                this.cbxRole.IsEnabled = true;
                this.cbxUser.IsEnabled = true;


                this.btnAdd.IsEnabled = false;
                this.btnDel.IsEnabled = false;
                this.btnUpdate.IsEnabled = false;

                this.btnClear.IsEnabled = true;

                this.btnSave.IsEnabled = true;
                this.btnTxtClear.IsEnabled = true;
            }
            else if (m_funstate == EditState.Edit)
            {
                this.cbxRole.IsEnabled = true;
                this.cbxUser.IsEnabled = false;


                this.btnAdd.IsEnabled = false;
                this.btnDel.IsEnabled = false;
                this.btnUpdate.IsEnabled = false;

                this.btnClear.IsEnabled = true;

                this.btnSave.IsEnabled = true;
                this.btnTxtClear.IsEnabled = false;
            }
            else
            {
                this.cbxRole.IsEnabled = false;
                this.cbxUser.IsEnabled = false;


                this.btnAdd.IsEnabled = true;
                this.btnDel.IsEnabled = true;
                this.btnUpdate.IsEnabled = true;

                this.btnClear.IsEnabled = false;

                this.btnSave.IsEnabled = false;
                this.btnTxtClear.IsEnabled = false;
            }
        }

        #endregion

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_userrole_fun == null || m_userrole_fun.Zgdm == null)
                {
                    PublicMethod.RadAlterBox("请选中要删除的行！", "提示");
                }
                else
                {

                    string UserID = m_userrole_fun.Zgdm;
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.DelPE_UserRoleCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                SQLMessage mess = ea.Result;
                                if (mess.IsSucceed)
                                {
                                    UserRoleManager_Loaded(null, null);
                                }
                                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.DelPE_UserRoleAsync(UserID);
                    serviceCon.CloseAsync();
                }
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
                if (m_userrole_fun == null || m_userrole_fun.Zgdm == null)
                {
                    PublicMethod.RadAlterBox("请选中要修改的行！", "提示");
                }
                else
                {
                    m_funstate = EditState.Edit;

                    BindPE_Fun(m_userrole_fun);

                    BindBtnState();
                }
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
                m_funstate = EditState.Add;
                BindPE_Fun(null);
                BindBtnState();
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
                m_funstate = EditState.View;

                BindPE_Fun(null);

                BindBtnState();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.cbxUser.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择用户！", "提示");
                    return;
                }

                string UserID = this.cbxUser.SelectedItem == null ? "" : (this.cbxUser.SelectedItem as CP_Employee).Zgdm;
                string RoleCode = this.cbxRole.SelectedItem == null ? "" : (this.cbxRole.SelectedItem as PE_Role).RoleCode;


                ObservableCollection<PE_Role> rolelist = new ObservableCollection<PE_Role>();
                //将角色功能信息保存到列表中
                foreach (object obj in GridViewRole.SelectedItems)
                {
                    PE_Role pe_fun = (PE_Role)obj;

                    rolelist.Add(pe_fun);
                }

                if (rolelist.Count > 0)
                {
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.InsertPE_UserRoleCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    SQLMessage mess = ea.Result;
                                    if (mess.IsSucceed)
                                    {
                                        UserRoleManager_Loaded(null, null);
                                    }
                                    PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");

                                }
                                else
                                {
                                    PublicMethod.RadWaringBox(ea.Error);
                                }
                            };
                    serviceCon.InsertPE_UserRoleAsync(UserID, rolelist);
                    serviceCon.CloseAsync();

                }
                //当角色无对应功能权限时为删除该角色对应的功能权限
                else
                {

                    PublicMethod.RadAlterBox("请选择角色！", "提示");
                    return;
                    //serviceCon = PublicMethod.YidanClient;
                    //serviceCon.DelPE_UserRoleCompleted += new EventHandler<DelPE_UserRoleCompletedEventArgs>(DelPE_UserRoleCompleted);
                    //serviceCon.DelPE_RoleFunAsync(UserID);
                    //serviceCon.CloseAsync();
                }
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
                BindPE_Fun(null);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void GridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {try{
            if (GridView.SelectedItem == null)
            {
                m_userrole_fun = null;
                return;
            }
            if (m_funstate == EditState.View)
            {
                m_userrole_fun = (PE_UserRoleList)GridView.SelectedItem;
                BindPE_Fun(m_userrole_fun);
            } }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }



        private void GridViewRole_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
            PE_Role t = (PE_Role)e.DataElement;
            List<CheckBox> listtest = (List<CheckBox>)(this.GridViewRole.ChildrenOfType<CheckBox>().ToList());
            if (listtest.Count > 0)
                if (t.IsCheck == 1)
                    listtest[listtest.Count - 1].IsChecked = true;
        }

    }
}
