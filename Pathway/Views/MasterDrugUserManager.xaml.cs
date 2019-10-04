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
using YidanSoft.Tool;

namespace YidanEHRApplication.Views
{
    public partial class MasterDrugUserManager : Page
    {
        bool isTrue = true;
        YidanEHRDataServiceClient serviceCon;

        List<PE_CompleteUser> UserList = new List<PE_CompleteUser>();

        /// <summary>
        /// 用于存储列表中选中的行记录
        /// </summary>
        static PE_CompleteUser user = new PE_CompleteUser();
        public MasterDrugUserManager()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(MasterDrugUserManager_Loaded);
        }



        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        EditState m_funstate;


        void MasterDrugUserManager_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isTrue)
                {
                    isTrue = true;
                    return;
                }
                m_funstate = EditState.View;
                LoadData();
                BindListBox();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

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
                rooth = new HashSet<string>();
                userh = new HashSet<string>();
                dicUser = new Dictionary<string, RadTreeViewItem>();
                rootItem = null;
                userItem = null;
                foreach (PE_CompleteUser pu in UserList)
                {
                    if (!rooth.Contains(pu.UserDept))
                    {
                        rooth.Add(pu.UserDept);
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
            CP_MasterDrugRoles t = (CP_MasterDrugRoles)e.DataElement;
            List<CheckBox> listtest = (List<CheckBox>)(this.GridViewRole.ChildrenOfType<CheckBox>().ToList());
            if (listtest.Count > 0)
                if (t.IsCheck == "1")
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
                //BindListBox();
                CheckBindList(user);
            }
            else
            {
                user = new PE_CompleteUser();
                this.txtDept.Text = null;
                this.txtName.Text = null;
            }
        }

        /// <summary>
        ///  绑定用户角色 add by luff 2012-08-27
        /// </summary>
        /// <param name="UserID"></param>
        private void BindListBox()
        {

            serviceCon = PublicMethod.YidanClient;
            serviceCon.MaintainCP_MasterDrugRolesCompleted += (s, e) =>
            {
                if (e.Error == null)
                {
                    List<CP_MasterDrugRoles> cP_MasterDrugRoles = new List<CP_MasterDrugRoles>();
                    cP_MasterDrugRoles = e.Result.ToList();


                    GridViewRole.ItemsSource = cP_MasterDrugRoles;
                }
            };
            serviceCon.MaintainCP_MasterDrugRolesAsync(null, Operation.Select.ToString());
        }
        /// <summary>
        /// 选择不同用户绑定对应的角色编码
        /// </summary>
        /// <param name="user"></param>
        private void CheckBindList(PE_CompleteUser user)
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.MaintainP_MasterDrug2UserSelectCompleted += (s, e) =>
            {
                if (e.Error == null)
                {
                    List<CP_MasterDrugRoles> m_MasterDrugRoles = new List<CP_MasterDrugRoles>();

                    List<CP_MasterDrug2User> m_MasterDrug2User = new List<CP_MasterDrug2User>();
                    m_MasterDrug2User = e.Result.ToList();


                    foreach (CP_MasterDrugRoles grid in (List<CP_MasterDrugRoles>)GridViewRole.ItemsSource)
                    {
                        grid.IsCheck = "0";
                        foreach (CP_MasterDrug2User item in m_MasterDrug2User)
                        {
                            if (item.Jsbm == grid.Jsbm)
                            {
                                grid.IsCheck = "1";
                                break;
                            }
                        }
                        m_MasterDrugRoles.Add(grid);
                    }

                    //GridViewRole.ItemsSource = null;
                    GridViewRole.ItemsSource = m_MasterDrugRoles;
                    //GridViewRole.ItemsSource = cP_MasterDrugRoles;
                }
            };
            serviceCon.MaintainP_MasterDrug2UserSelectAsync(user.UserID);
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
            ObservableCollection<CP_MasterDrug2User> Roleusers;

            Roleusers = new ObservableCollection<CP_MasterDrug2User>();
            try
            {
                if (this.treeViewUser.SelectedItem == null)
                {
                    //PublicMethod.RadAlterBox("请选择用户！", "提示");
                    PublicMethod.RadAlterBoxRe("请选择用户！", "提示",this.txtQuery);
                    isTrue = false;
                    return;
                }
                RadTreeViewItem item = this.treeViewUser.SelectedItem as RadTreeViewItem;
                PE_CompleteUser user = item.Tag as PE_CompleteUser;
                string Zgdm = user.UserID;


                //ObservableCollection<PE_Role> rolelist = new ObservableCollection<PE_Role>();
                //将角色功能信息保存到列表中
                //foreach (object obj in GridViewRole.SelectedItems)
                foreach (var _item in this.GridViewRole.SelectedItems)
                {
                    CP_MasterDrugRoles role = ((CP_MasterDrugRoles)_item);
                    CP_MasterDrug2User Roleuser = new CP_MasterDrug2User();
                    Roleuser.Zgdm = Zgdm;
                    Roleuser.Jsbm = role.Jsbm;
                    Roleuser.Cjsj = DateTime.Now.ToString();
                    Roleuser.Xgsj = DateTime.Now.ToString();
                    Roleuser.ZgdmCj = Global.LogInEmployee.Zgdm;
                    Roleuser.ZgdmXg = Global.LogInEmployee.Zgdm;

                    Roleusers.Add(Roleuser);

                }
                if (Roleusers.Count > 0)
                {
                    YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                    Client.MaintainP_MasterDrug2UserUpdateCompleted += (sb, eb) =>
                    {
                        //MasterDrugUserManager_Loaded(null, null);
                        this.btnUpdate.IsEnabled = true;
                        this.btnSave.IsEnabled = false;
                        this.ScrollViewerRole.IsEnabled = false;
                        this.btnCancel.IsEnabled = false;

                        PublicMethod.RadAlterBox("保存成功！", "提示");

                    };
                    Client.MaintainP_MasterDrug2UserUpdateAsync(Roleusers, Zgdm);
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
                if (item.Tag == null) return;
                PE_CompleteUser user = item.Tag as PE_CompleteUser;
                InitUserInfo(user);
                BindPE_Fun(user);

            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }




    }
}
