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
using Telerik.Windows.Controls;

namespace YidanEHRApplication.Views.Permission
{
    public partial class RoleManeger : Page
    {
        YidanEHRDataServiceClient serviceCon;

        /// <summary>
        /// 用于存储列表中选中的行记录
        /// </summary>
        static PE_Role m_role = new PE_Role();

        EditState m_funstate;

        public RoleManeger()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(RoleManeger_Loaded);
        }

        public bool isLoad = true;
        void RoleManeger_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isLoad)
                {
                    isLoad = true;
                    return;
                }
                BindGridView();
                m_funstate = EditState.View;
                BindBtnState();
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

        #region 函数
        //YidanEHRServiceReference.yida serviceCon;

        private void BindGridView()
        {
            if (!radBusyIndicator.IsBusy)
                radBusyIndicator.IsBusy = true;
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetPE_RoleListCompleted +=
            (obj, e) =>
            {
                radBusyIndicator.IsBusy = false;
                if (e.Error == null)
                {
                    GridView.ItemsSource = e.Result;

                    BindPE_Fun(null);
                }
                else
                {
                    PublicMethod.RadWaringBox(e.Error);
                }
            };
            serviceCon.GetPE_RoleListAsync();
            serviceCon.CloseAsync();
        }



        /// <summary>
        /// 绑定页面中编辑信息
        /// </summary>
        /// <param name="pe_fun">实体，如果为空编辑区域值为空</param>
        private void BindPE_Fun(PE_Role pe_role)
        {
            if (pe_role != null)
            {
                this.txtRoleCode.Text = pe_role.RoleCode;
                this.txtRoleName.Text = pe_role.RoleName;

            }
            else
            {
                m_role = new PE_Role();
                this.txtRoleCode.Text = "";
                this.txtRoleName.Text = "";
            }
        }


        private void BindBtnState()
        {
            if (m_funstate == EditState.Add)
            {
                this.txtRoleCode.IsEnabled = true;
                this.txtRoleName.IsEnabled = true;


                this.btnAdd.IsEnabled = false;
                this.btnDel.IsEnabled = false;
                this.btnUpdate.IsEnabled = false;

                this.btnClear.IsEnabled = true;

                this.btnSave.IsEnabled = true;
                this.btnTxtClear.IsEnabled = true;
            }
            else if (m_funstate == EditState.Edit)
            {
                this.txtRoleCode.IsEnabled = false;
                this.txtRoleName.IsEnabled = true;


                this.btnAdd.IsEnabled = false;
                this.btnDel.IsEnabled = false;
                this.btnUpdate.IsEnabled = false;

                this.btnClear.IsEnabled = true;

                this.btnSave.IsEnabled = true;
                this.btnTxtClear.IsEnabled = false;
            }
            else
            {
                this.txtRoleCode.IsEnabled = false;
                this.txtRoleName.IsEnabled = false;


                this.btnAdd.IsEnabled = true;
                this.btnDel.IsEnabled = true;
                this.btnUpdate.IsEnabled = true;

                this.btnClear.IsEnabled = false;

                this.btnSave.IsEnabled = false;
                this.btnTxtClear.IsEnabled = false;
            }
        }

        #endregion

        #region 事件

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (m_role == null || m_role.RoleCode == null)
                {
                    PublicMethod.RadAlterBox("请选中要删除的行！", "提示");
                }
                else
                {
                    //DialogParameters parameters = new DialogParameters();
                    //parameters.Content = String.Format("提示: {0}", "请问是否删除选中的角色信息？");
                    //parameters.Header = "提示";
                    //parameters.IconContent = null;
                    //parameters.OkButtonContent = "确定";
                    //parameters.CancelButtonContent = "取消";
                    //parameters.Closed = OnDelAdviceGroupDetail;//***close处理***
                    //RadWindow.Confirm(parameters);

                    YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                    mess.ShowDialog();
                    mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);



                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void mess_PageClosedEvent(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    string RoleCode = m_role.RoleCode;
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.DelPE_RoleCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                SQLMessage mess = ea.Result;
                                if (mess.IsSucceed)
                                {
                                    RoleManeger_Loaded(null, null);
                                }
                                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.DelPE_RoleAsync(RoleCode);
                    serviceCon.CloseAsync();

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        void OnDelAdviceGroupDetail(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                try
                {
                    string RoleCode = m_role.RoleCode;
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.DelPE_RoleCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                SQLMessage mess = ea.Result;
                                if (mess.IsSucceed)
                                {
                                    RoleManeger_Loaded(null, null);
                                }
                                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.DelPE_RoleAsync(RoleCode);
                    serviceCon.CloseAsync();
                }
                catch (Exception ex)
                {
                    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                m_funstate = EditState.Add;
                BindPE_Fun(null);
                GridView.SelectedItem = null;
                BindBtnState();
                txtRoleCode.Focus();
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
                if (m_role == null || m_role.RoleCode == null)
                {
                    PublicMethod.RadAlterBox("请选中要修改的行！", "提示");
                }
                else
                {
                    m_funstate = EditState.Edit;

                    BindPE_Fun(m_role);

                    BindBtnState();

                    txtRoleName.Focus();
                }
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
                GridView.SelectedItem = null;
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
                string RoleCode = this.txtRoleCode.Text.ToString().Trim();
                string RoleName = this.txtRoleName.Text.ToString().Trim();

                if (RoleCode.Length == 0)
                {
                    PublicMethod.RadAlterBoxRe("请输入角色代码！", "提示", txtRoleCode);
                    isLoad = false;
                    return;
                }

                if (RoleCode.Length >= 11)
                {
                    PublicMethod.RadAlterBoxRe("角色代码长度超出！", "提示", txtRoleCode);
                    isLoad = false;
                    return;
                }
                try
                {
                    int.Parse(RoleCode);
                }
                catch
                {
                    PublicMethod.RadAlterBoxRe("角色代码必须是整数！", "提示", txtRoleCode);
                    isLoad = false;
                    return;
                }
                if (RoleName.Length == 0)
                {
                    PublicMethod.RadAlterBoxRe("请输入角色名称！", "提示", txtRoleName);
                    isLoad = false;
                    return;
                }

                if (m_funstate == EditState.Add)
                {
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.InsertPE_RoleCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                SQLMessage mess = ea.Result;
                                if (mess.IsSucceed)
                                {
                                    RoleManeger_Loaded(null, null);
                                }
                                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.InsertPE_RoleAsync(RoleCode, RoleName);
                    serviceCon.CloseAsync();
                }
                else
                {
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.UpdatePE_RoleCompleted +=

                    (obj, ea) =>
                    {
                        if (ea.Error == null)
                        {
                            SQLMessage mess = ea.Result;
                            if (mess.IsSucceed)
                            {
                                RoleManeger_Loaded(null, null);
                            }
                            PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(ea.Error);
                        }
                    };
                    serviceCon.UpdatePE_RoleAsync(RoleCode, RoleName);
                    serviceCon.CloseAsync();
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
        {
            try
            {
                if (GridView.SelectedItem == null)
                {
                    m_role = null;
                    return;
                }
                if (m_funstate == EditState.View)
                {
                    m_role = (PE_Role)GridView.SelectedItem;
                    BindPE_Fun(m_role);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        #region  输入框加回车事件
        private void RegisterKeyEvent()
        {
            txtRoleCode.KeyUp += new KeyEventHandler(txtRoleCode_KeyUp);
            txtRoleName.KeyUp += new KeyEventHandler(txtRoleName_KeyUp);
            btnSave.KeyUp += new KeyEventHandler(btnSave_KeyUp);

        }

        private void txtRoleCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtRoleName.Focus();
        }

        private void txtRoleName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSave.Focus();
        }

        private void btnSave_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSave_Click(null, null);
        }

        #endregion

        #endregion
    }
}
