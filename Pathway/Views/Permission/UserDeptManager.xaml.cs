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
    public partial class UserDeptManager : Page
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

        List<User2Dept> m_user2DeptList = new List<User2Dept>();

        EditState m_funstate;
        public UserDeptManager()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(UserDeptManager_Loaded);
        }

        void UserDeptManager_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.View;
                LoadData();
                BindListBox("");
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

        ///// <summary>
        ///// 绑定页面下拉框值
        ///// </summary>
        //private void BindCombox()
        //{
        //    BindcbxUser();
        //    BindcbxRole();
        //}

        ///// <summary>
        ///// 获取用户角色下拉框
        ///// </summary>
        //private void BindcbxUser()
        //{

        //}
        ///// <summary>
        ///// 绑定角色下拉框
        ///// </summary>
        //private void BindcbxRole()
        //{
        //}



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

        private void GridViewDept_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            //if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
            //User2Dept t = (User2Dept)e.DataElement;
            //List<CheckBox> listtest = (List<CheckBox>)(this.GridViewDept.ChildrenOfType<CheckBox>().ToList());
            //if (listtest.Count > 0)
            //    if (t.IsCheck == 1)
            //        listtest[listtest.Count - 1].IsChecked = true;
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
            serviceCon.GetUserDeptByUserIDCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        m_user2DeptList = e.Result.ToList();
                        BindGridViewSoure(m_user2DeptList);
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            serviceCon.GetUserDeptByUserIDAsync(UserID);
            serviceCon.CloseAsync();
        }

        /// <summary>
        /// 根据数据源绑定科室列表
        /// </summary>
        /// <param name="_list"></param>
        private void BindGridViewSoure(List<User2Dept> _list)
        {
            GridViewDept.ItemsSource = _list.Select(s => s).Where(s => s.IsCheck == 0).ToList();
            GridViewDeptNew.ItemsSource = _list.Select(s => s).Where(s => s.IsCheck == 1).ToList();
        }

        #endregion

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            m_funstate = EditState.Edit;
            BindButState();
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

                ObservableCollection<User2Dept> rolelist = new ObservableCollection<User2Dept>();
                //将角色功能信息保存到列表中
                List<User2Dept> listdept = this.GridViewDeptNew.ItemsSource as List<User2Dept>;
                foreach (object obj in listdept)
                {
                    User2Dept pe_dept = (User2Dept)obj;

                    rolelist.Add(pe_dept);
                }

                if (rolelist.Count > 0)
                {
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.InsertUser2DeptCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    SQLMessage mess = new SQLMessage();
                                    if (ea.Result == true)
                                    {

                                        mess.Message = "保存成功！";
                                        mess.IsSucceed = true;
                                    }
                                    else
                                    {
                                        mess.Message = "保存失败！";
                                        mess.IsSucceed = false;
                                    }

                                    if (mess.IsSucceed)
                                    {
                                        m_funstate = EditState.View;
                                        //NewUserRoleManager_Loaded(null, null);
                                    }
                                    else
                                    {
                                        m_funstate = EditState.Edit;
                                    }

                                    BindButState();

                                    //PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                                    YiDanMessageBox.Show(mess.Message);
                                }
                                else
                                {
                                    //PublicMethod.RadWaringBox(ea.Error);
                                    YiDanMessageBox.Show(ea.Error,this.GetType().FullName);
                                }
                            };
                    serviceCon.InsertUser2DeptAsync(rolelist);
                    serviceCon.CloseAsync();

                }
                //当角色无对应多个科室时删除对应科室信息
                else
                {
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.DelUser2DeptCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    YiDanMessageBox.Show("保存成功！");
                                }
                                else
                                {
                                    YiDanMessageBox.Show(ea.Error, this.GetType().FullName);
                                }
                            };
                    serviceCon.DelUser2DeptAsync(UserID);
                    serviceCon.CloseAsync();
                    m_funstate = EditState.View;
                    BindButState();
                    //PublicMethod.RadAlterBox("请选择角色！", "提示");
                    //YiDanMessageBox.Show("请选择角色！");
                    return;
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void BindButState()
        {
            if (m_funstate == EditState.View)
            {
                this.btnUpdate.IsEnabled = true;
                this.btnSave.IsEnabled = false;
                this.ScrollViewerDept.IsEnabled = false;
                this.ScrollViewerDeptNew.IsEnabled = false;
                this.btnCancel.IsEnabled = false;
                this.btnright.IsEnabled = false;
                this.btnleft.IsEnabled = false;
            }
            else if (m_funstate == EditState.Edit)
            {
                this.btnUpdate.IsEnabled = false;
                this.btnSave.IsEnabled = true;
                this.ScrollViewerDept.IsEnabled = true;
                this.ScrollViewerDeptNew.IsEnabled = true;
                this.btnCancel.IsEnabled = true;
                this.btnright.IsEnabled = true;
                this.btnleft.IsEnabled = true;
            }
            else
            {
                this.btnUpdate.IsEnabled = true;
                this.btnSave.IsEnabled = false;
                this.ScrollViewerDept.IsEnabled = false;
                this.ScrollViewerDeptNew.IsEnabled = false;
                this.btnCancel.IsEnabled = false;
                this.btnright.IsEnabled = false;
                this.btnleft.IsEnabled = false;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            m_funstate = EditState.View;
            BindButState(); 
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

            //儿科\\366 王叶芳   定位treeview中对应节点
            RadTreeViewItem item1 = treeViewUser.GetItemByPath(string.Format("{0}\\{1} {2}", users.UserDept, users.UserID, users.UserName));
            if (item1 != null && item1.Tag != null)
            {
                treeViewUser.CollapseAll();
                treeViewUser.SelectedItem = item1;

                txtQuery.Text = users.UserName;
                return;
            }
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
        /// 双击事件  左侧科室列表中科室添加到右侧表格中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewDept_RowActivated(object sender, RowEventArgs e)
        {
            try
            {
                if (GridViewDept.SelectedItems.Count == 0)
                {
                    return;
                }
                foreach (User2Dept up in GridViewDept.SelectedItems)
                {
                    foreach (User2Dept userdept in m_user2DeptList)
                    {
                        if (up.DeptId == userdept.DeptId)
                        {
                            userdept.IsCheck = 1;
                            continue;
                        }
                    }
                }
                BindGridViewSoure(m_user2DeptList);
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex, this.GetType().FullName);
            }
        }

        /// <summary>
        /// 双击事件  右侧科室列表中科室添加到左侧表格中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewDeptNew_RowActivated(object sender, RowEventArgs e)
        {
            try
            {
                if (GridViewDeptNew.SelectedItems.Count == 0)
                {
                    return;
                }
                foreach (User2Dept up in GridViewDeptNew.SelectedItems)
                {
                    foreach (User2Dept userdept in m_user2DeptList)
                    {
                        if (up.DeptId == userdept.DeptId)
                        {
                            userdept.IsCheck = 0;
                            continue;
                        }
                    }
                }
                BindGridViewSoure(m_user2DeptList);
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex, this.GetType().FullName);
            }
        }

        private void btnright_Click(object sender, RoutedEventArgs e)
        {
            GridViewDept_RowActivated(null, null);
        }

        private void btnleft_Click(object sender, RoutedEventArgs e)
        {
            GridViewDeptNew_RowActivated(null, null);
        }




    }
}
