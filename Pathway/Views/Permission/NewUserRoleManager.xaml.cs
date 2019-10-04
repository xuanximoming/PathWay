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
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Views.Permission
{
    public partial class NewUserRoleManager : Page
    {
        YidanEHRDataServiceClient serviceCon;

        /// <summary>
        /// 当前树节点选中的节点
        /// </summary>
        RadTreeViewItem m_Selectitem = null;

        List<PE_CompleteUser> UserList = new List<PE_CompleteUser>();

        /// <summary>
        /// 用于存储列表中选中的行记录
        /// </summary>
        static PE_CompleteUser user = new PE_CompleteUser();

        EditState m_funstate;
        public NewUserRoleManager()
        {
            InitializeComponent();
        }

        void NewUserRoleManager_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.View;
                LoadData();
                BindListBox("");
                treeViewUser.IsEnabled = true;
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


        private void LoadData()
        {
            try
            {
                YidanEHRDataServiceClient serviceCon = PublicMethod.YidanClient;
                serviceCon.GetCompleteEmployeeWardCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            UserList = e.Result.ToList();
                            InitTreeView();

                            txtQuery.ItemsSource = UserList;
                            txtQuery.ItemFilter = Filter;

                        }
                    };
                serviceCon.GetCompleteEmployeeWardAsync();
                serviceCon.CloseAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private bool Filter(string strFilter, object item)
        {
            PE_CompleteUser deptList = (PE_CompleteUser)item;
            return ((deptList.Py.StartsWith(strFilter.ToLower())) || (deptList.Py.Contains(strFilter.ToLower())) || deptList.UserID.StartsWith(strFilter.ToLower()) || deptList.UserID.Contains(strFilter.ToLower()));
        }

        HashSet<string> rooth;
        HashSet<string> userh;
        Dictionary<string, RadTreeViewItem> dicUser;
        RadTreeViewItem rootItem;
        RadTreeViewItem userItem;
        private void InitTreeView()
        {
            try
            {
                treeViewUser.Items.Clear();
                treeViewUser.IsEnabled = true;
                rooth = new HashSet<string>();
                userh = new HashSet<string>();
                dicUser = new Dictionary<string, RadTreeViewItem>();
                rootItem = null;
                userItem = null;
                foreach (PE_CompleteUser pu in UserList)
                {
                    if (!rooth.Contains(pu.UserDept.Trim()))
                    {
                        rooth.Add(pu.UserDept.Trim());
                        rootItem = AddItem(pu.UserDept, null, null);
                    }
                    if (!dicUser.ContainsKey(pu.UserID))
                    {
                        userh.Add(pu.UserName);
                        userItem = AddItem(pu.UserID + " " + pu.UserName, pu, rootItem);
                        dicUser.Add(pu.UserID, userItem);
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private RadTreeViewItem AddItem(string header, PE_CompleteUser user, RadTreeViewItem treeViewItem)
        {

            RadTreeViewItem radTreeItem = (new RadTreeViewItem()
            {
                Header = header,
            });
            try
            {
                radTreeItem.Tag = user;
                if (treeViewItem == null)
                {
                    treeViewItem = radTreeItem;
                    this.treeViewUser.Items.Add(radTreeItem);
                }
                else
                {
                    treeViewItem.Items.Add(radTreeItem);
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
            return radTreeItem;


        }



        private void InitUserInfo(PE_CompleteUser user)
        {
            try
            {
                txtName.Text = user.UserName;
                txtDept.Text = user.UserDept;
                txtSexy.Text = user.Sexy;
                textBox1.Text = user.Marital;
                textBox2.Text = user.Birth;
                textBox3.Text = user.DocGrade;
                textBox5.Text = user.Ward;
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
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

        /// <summary>
        /// 绑定页面中编辑信息
        /// </summary>
        /// <param name="pe_fun">实体，如果为空编辑区域值为空</param>
        private void BindPE_Fun(PE_CompleteUser user)
        {
            if (user != null)
            {
                BindListBox(user.UserID);
            }
            else
            {
                user = new PE_CompleteUser();
                this.txtDept.Text = null;
                this.txtName.Text = null;
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

        #endregion

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            this.btnUpdate.IsEnabled = false;
            this.btnSave.IsEnabled = true;
            this.ScrollViewerRole.IsEnabled = true;
            this.btnCancel.IsEnabled = true;
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.treeViewUser.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择用户！", "提示");
                    return;
                }
                RadTreeViewItem item = this.treeViewUser.SelectedItem as RadTreeViewItem;
                PE_CompleteUser user = item.Tag as PE_CompleteUser;
                string UserID = user.UserID;

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
                                        m_funstate = EditState.View;
                                        //NewUserRoleManager_Loaded(null, null);
                                    }

                                    this.btnUpdate.IsEnabled = true;
                                    this.btnSave.IsEnabled = false;
                                    this.ScrollViewerRole.IsEnabled = false;
                                    this.btnCancel.IsEnabled = false;
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
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.btnUpdate.IsEnabled = true;
            this.btnSave.IsEnabled = false;
            this.ScrollViewerRole.IsEnabled = false;
            this.btnCancel.IsEnabled = false;
        }

        private void Query_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtQuery.Text == "" || this.txtQuery.Text == null)
            {
                //PublicMethod.RadAlterBox("请输入用户工号！", "提示");
                return;
            }
            string key = this.txtQuery.Text;
            foreach (PE_CompleteUser pu in UserList)
            {
                if (dicUser.ContainsKey(key))
                {
                    this.treeViewUser.SelectedItem = dicUser[key];
                    return;
                }
                if (pu.UserName == key)
                {
                    this.treeViewUser.SelectedItem = dicUser[pu.UserID];
                    return;
                }

            }
        }

        /// <summary>
        /// 查询下拉框锁定对应树节点中人员 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQuery_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (txtQuery.SelectedItem == null)
                return;
            PE_CompleteUser users = (PE_CompleteUser)txtQuery.SelectedItem;
            treeViewUser.IsEnabled = true;
            // 定位treeview中对应节点
            RadTreeViewItem item1 = treeViewUser.GetItemByPath(string.Format("{0}\\{1} {2}", users.UserDept, users.UserID, users.UserName));
            if (item1 != null && item1.Tag != null)
            {
                treeViewUser.CollapseAll();
                treeViewUser.SelectedItem = item1;
                txtQuery.Text = users.UserName;
            }
            text.Visibility = System.Windows.Visibility.Collapsed;
        }

        /// <summary>
        /// 树节点中切换人绑定对应信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewUser_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                RadTreeViewItem item = (RadTreeViewItem)treeViewUser.SelectedItem;
                //判断如果当前在编辑状态则不能选中树节点
                if (!this.btnUpdate.IsEnabled && treeViewUser.SelectedItem != m_Selectitem)
                {
                    treeViewUser.SelectedItem = m_Selectitem;
                    YiDanMessageBox.Show("当前为编辑状态不能切换职工，如需切换职工请保存或者取消更改！", YiDanMessageBoxButtons.Ok);
                    return;
                }

                if (item.Tag == null) return;

                PE_CompleteUser user = item.Tag as PE_CompleteUser;
                m_Selectitem = item;
                InitUserInfo(user);
                BindPE_Fun(user);
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 查询框下拉框打开事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQuery_DropDownOpened(object sender, RoutedPropertyChangedEventArgs<bool> e)
        {
            if (txtQuery.IsDropDownOpen)
            {
                //treeViewUser.IsEnabled = false;
                text.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
                //treeViewUser.IsEnabled = true;
                //text.Visibility = System.Windows.Visibility.Collapsed;
            }
        }




    }
}
