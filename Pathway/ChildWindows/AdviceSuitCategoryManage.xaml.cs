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
using YidanEHRApplication.Helpers;
using System.Collections.ObjectModel;
using YidanEHRApplication.Models;
using YidanSoft.Tool;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Views.ChildWindows
{
    public partial class AdviceSuitCategoryManage
    {
        #region 事件
        List<RadTreeViewItem> AllTreeVewItems;//所有节点
        /// <summary>
        /// 页面加载
        /// </summary>
        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            GetCP_AdviceSuitCategory();
            this.txtName.Focus();
            ddpParent.IsEnabled = false;
        }
        private void gvW_AdviceSuitCategory_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (m_funstate == EditState.Add)
            {
                return;
            }
            adviceSuitCategory = (CP_AdviceSuitCategory)gvW_AdviceSuitCategory.SelectedItem;
            if (adviceSuitCategory == null)
            {
                return;
            }
            #region 文本框赋值
            txtCategoryId.Text = adviceSuitCategory.CategoryId;
            txtName.Text = adviceSuitCategory.Name;
            txtMemo.Text = adviceSuitCategory.Memo;
            string s = adviceSuitCategory.ParentName;
            ddpParent.Content = s;// SetValue(s);
            #endregion
        }
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.None;
                BindBtnState();
                //if (adviceSuitCategory == null)  修改前
                //更改修改后没有刷新GridView  可以直接删除的BUG
                //2013年7月22日 16:46:37
                //更改人：Jhonny
                if (gvW_AdviceSuitCategory.SelectedItem == null)//修改后
                {
                    PublicMethod.RadAlterBox("请选中一条数据!", "提示");
                    return;
                }
                #region 删除提示
                //DialogParameters parameters = new DialogParameters();/* uodate by dxj 2011-7-26 删除提示 */
                //parameters.Content = String.Format("{0}", "确定删除吗？删除后不能恢复!");
                //parameters.Header = "提示";
                //parameters.IconContent = null;
                //parameters.OkButtonContent = "确定";
                //parameters.CancelButtonContent = "取消";
                //parameters.Closed = OnDeleteMaster;
                //RadWindow.Confirm(parameters);
                YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                mess.ShowDialog();
                //GetCP_AdviceSuitCategory();
                
                mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
                //AddTree(String.Empty, null, result, result);
                #endregion
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void mess_PageClosedEvent(object sender, bool e)
        {
            try
            {
                if (e == true)
                {

                    YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                    client.DeleteCheckInfoCompleted += (objc, rea) =>
                    {
                        if (rea.Error == null)
                        {
                            if (rea.Result > 0)
                            {
                                PublicMethod.RadAlterBox("该分类已在其他地方使用，无法删除!", "提示");
                                return;
                            }

                        }


                        client.DeleteCP_AdviceSuitCategoryCompleted += (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                if (ea.Result > 0)
                                {
                                    cp_AdviceSuitTypeList.Remove(adviceSuitCategory);
                                    adviceSuitCategory = null;
                                    ClearTextBox();
                                    tvParent.Items.Clear();
                                    AddTree(String.Empty, null, cp_AdviceSuitTypeList.ToList(), cp_AdviceSuitTypeList.ToList());
                                    PublicMethod.RadAlterBox("删除成功!", "提示");
                                }
                                else
                                {
                                    PublicMethod.RadAlterBox("删除失败!", "提示");
                                }
                            }
                            else
                            {
                                PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                            }
                        };
                        client.DeleteCP_AdviceSuitCategoryAsync(adviceSuitCategory.CategoryId);
                    };
                    client.DeleteCheckInfoAsync(adviceSuitCategory.CategoryId);
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void OnDeleteMaster(object sender, WindowClosedEventArgs e)/* update by luff 2011-7-26 删除提示 */
        {
            if (e.DialogResult == true)
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.DeleteCheckInfoCompleted += (objc, rea) =>
                {
                    if (rea.Error == null)
                    {
                        if (rea.Result > 0)
                        {
                            PublicMethod.RadAlterBox("该分类已在其他地方使用，无法删除!", "提示");
                            return;
                        }

                    }
                    client.DeleteCP_AdviceSuitCategoryCompleted += (obj, ea) =>
                    {
                        if (ea.Error == null)
                        {
                            if (ea.Result > 0)
                            {
                                cp_AdviceSuitTypeList.Remove(adviceSuitCategory);
                                adviceSuitCategory = null;
                                ClearTextBox();

                                PublicMethod.RadAlterBox("删除成功!", "提示");
                            }
                            else
                            {
                                PublicMethod.RadAlterBox("删除失败!", "提示");
                            }
                        }
                        else
                        {
                            PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                        }
                    };
                    client.DeleteCP_AdviceSuitCategoryAsync(adviceSuitCategory.CategoryId);
                };
                client.DeleteCheckInfoAsync(adviceSuitCategory.CategoryId);
            }
        }
        void OnFocusMaster(object sender, WindowClosedEventArgs e)/* update by luff 2011-7-26 委托获得当前控件焦点 */
        {
            if (e.DialogResult == true)
            {
                this.txtName.Focus();
            }
        }
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                
                //if (adviceSuitCategory == null)  修改前
                //更改修改后还能继续修改的问题
                //时间：2013年7月22日 15:56:29
                //更改人：Jhonny
                if (gvW_AdviceSuitCategory.SelectedItem == null)//修改后
                {
                    PublicMethod.RadAlterBox("请选择一行数据!", "提示");
                    return;
                }
                //AddTree(String.Empty, null, cp_AdviceSuitTypeList.ToList(), cp_AdviceSuitTypeList.ToList());
                ddpParent.IsEnabled = true;
                m_funstate = EditState.Edit;
                BindBtnState();
                txtName.Focus();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ddpParent.IsEnabled = true;
                ddpParent.Content = "请选择父类名称";
                m_funstate = EditState.Add;
                BindBtnState();
                txtName.Focus();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.None;
                ddpParent.Content = "请选择分类名称...";
                ddpParent.IsEnabled = false;
                gvW_AdviceSuitCategory.SelectedItem = null;
                adviceSuitCategory = null;
                txtName.Text = "";
                txtMemo.Text = "";
                BindBtnState();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtName.Text.Trim() == String.Empty)
                {
                    //PublicMethod.RadAlterBox("类别名称不能为空！", "提示");
                    #region  判断类别名称不能为空并获得当前控件焦点
                    DialogParameters parameters = new DialogParameters();/* update by luff 2012-8-30 */
                    parameters.Content = String.Format("{0}", "类别名称不能为空!");
                    parameters.Header = "提示";
                    parameters.IconContent = null;
                    parameters.OkButtonContent = "确定";
                    //parameters.CancelButtonContent = null;
                    parameters.Closed = OnFocusMaster;
                    RadWindow.Alert(parameters);
                    #endregion
                    
                    return;
                }
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                if (m_funstate == EditState.Add)
                {
                    //
                    this.ddpParent.IsEnabled = true;
                    this.ddpParent.Content = "请选择父类名称..";
                    #region 实体赋值
                    CP_AdviceSuitCategory adviceSuitCategory = new CP_AdviceSuitCategory();
                    adviceSuitCategory.CategoryId = ConvertMy.ToString(Guid.NewGuid());
                    adviceSuitCategory.Name = txtName.Text;
                    adviceSuitCategory.Memo = txtMemo.Text;
                    adviceSuitCategory.ParentID = CategoryId;
                    adviceSuitCategory.Zgdm = Global.LogInEmployee.Zgdm;
                    #endregion
                    client.InsertCheckInfoCompleted += (obj, ea) =>
                    {
                        if (ea.Result > 0)
                        {
                            PublicMethod.RadAlterBox("该分类已存在！", "提示");
                            return;
                        }
                        #region 添加分类
                        client.InsertAndSelectCP_AdviceSuitCategoryCompleted += (objc, rea) =>
                        {
                            if (rea.Result.ToList() != null)
                            {
                                cp_AdviceSuitTypeList = rea.Result;
                                gvW_AdviceSuitCategory.ItemsSource = cp_AdviceSuitTypeList;
                                tvParent.Items.Clear();
                                AddTree(String.Empty, null, rea.Result.ToList(), rea.Result.ToList());
                                m_funstate = EditState.None;
                                BindBtnState();
                                ClearTextBox();
                                adviceSuitCategory = null;
                                PublicMethod.RadAlterBox("添加成功！", "提示");
                            }
                        };
                        client.InsertAndSelectCP_AdviceSuitCategoryAsync(adviceSuitCategory, String.Empty);
                        #endregion

                    };
                    client.InsertCheckInfoAsync(txtName.Text.Trim());
                }
                if (m_funstate == EditState.Edit)
                {
                    #region 改变实体的值
                    // 新建对象 ，如果分类名存在则清除数据
                    //修改时间：2013年8月14日 15:35:26
                    //修改人：Jhonny
                    CP_AdviceSuitCategory adviceSuitCategory = new CP_AdviceSuitCategory();
                    adviceSuitCategory.Name = txtName.Text;
                    adviceSuitCategory.Yxjl = "1";
                    adviceSuitCategory.ParentID = row == null ? adviceSuitCategory.ParentID : row.CategoryId;
                    adviceSuitCategory.Memo = txtMemo.Text;
                    #endregion
                    client.UpdateCheckInfoCompleted += (obj, ea) =>
                    {
                        if (ea.Result > 0)
                        {
                            PublicMethod.RadAlterBox("该分类名称已存在！", "提示");
                            return;
                        }
                        else
                        {
                            #region 修改分类
                            client.UpdateCP_AdviceSuitCategoryCompleted += (objc, rea) =>
                            {
                                if (rea.Error == null)
                                {
                                    if (rea.Result.ToList() != null)
                                    {
                                        m_funstate = EditState.None;
                                        BindBtnState();

                                        //添加刷新GridView的方法
                                        //2013年7月22日 16:41:18
                                        //修改人：Jhonny
                                        //在对数据操作完之后能够及时刷新
                                        GetCP_AdviceSuitCategory();
                                        InitTreeView();
                                        //不让产生缓存
                                        row = null;
                                        PublicMethod.RadAlterBox("修改成功！", "提示");
                                    }
                                }
                                else
                                {
                                    PublicMethod.RadAlterBox(rea.Error.ToString(), "提示");
                                }
                            };
                            client.UpdateCP_AdviceSuitCategoryAsync(adviceSuitCategory);
                            #endregion
                        }
                    };
                    client.UpdateCheckInfoAsync(adviceSuitCategory.Name, adviceSuitCategory.CategoryId);
                }
                ddpParent.Content = "请选择分类名称...";
                ddpParent.IsEnabled = false;
                txtName.Text = "";
                txtMemo.Text = "";  
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void tvParent_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            //调用以便增删改后刷新TreeView
            InitTreeView();
        }
        #endregion

        #region 方法

        /// <summary>
        /// 增删改后能够及时的刷新TreeView
        /// 创建：2013年7月23日 17:53:53
        /// 创建人：Jhonny
        /// </summary>
        public void InitTreeView()
        {
            if (tvParent.SelectedItem == null)
            {
                return;
            }
            ddpParent.Content = ((RadTreeViewItem)tvParent.SelectedItem).Header;
            row = ((RadTreeViewItem)tvParent.SelectedItem).Tag as CP_AdviceSuitCategory;
            //adviceSuitCategory.ParentID = row.ParentID;
            this.ddpParent.IsOpen = false;
        }

        public AdviceSuitCategoryManage()
        {
            InitializeComponent();
            //去掉鼠标右键Silverlight 菜单
            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };
        }

        //private void InitDgv()
        //{
        //    try
        //    {
        //        YidanEHRDataServiceClient client = PublicMethod.YidanClient;
        //        CP_AdviceSuitCategory AdviceSuitCategory = new CP_AdviceSuitCategory();
        //        AdviceSuitCategory.Name = txtName.Text;
        //        AdviceSuitCategory.Yxjl = "1";
        //        AdviceSuitCategory.ParentID = row.CategoryId;
        //        AdviceSuitCategory.Memo = txtMemo.Text;
        //        client.InsertAndSelectCP_AdviceSuitCategoryCompleted += (obj, e) =>
        //        {
        //            if (e.Error == null)
        //            {
        //                cp_AdviceSuitTypeList = e.Result;
        //                AddTree(String.Empty, null, e.Result.ToList(), e.Result.ToList());
        //                if (cp_AdviceSuitTypeList != null)
        //                {
        //                    gvW_AdviceSuitCategory.ItemsSource = cp_AdviceSuitTypeList;
        //                }
        //            }
        //        };
        //        client.InsertAndSelectCP_AdviceSuitCategoryAsync(AdviceSuitCategory, String.Empty);
        //    }
        //    catch (Exception ex)
        //    {
        //        PublicMethod.ClientException(ex, this.GetType().FullName, true);
        //    }
        //}

        /// <summary>
        /// 获取分类信息
        /// </summary>
        private void GetCP_AdviceSuitCategory()
        {
            try
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                CP_AdviceSuitCategory AdviceSuitCategory = new CP_AdviceSuitCategory();
                AdviceSuitCategory.CategoryId = String.Empty;
                AdviceSuitCategory.Name = String.Empty;
                AdviceSuitCategory.Memo = String.Empty;
                AdviceSuitCategory.Zgdm = String.Empty;
                
                client.InsertAndSelectCP_AdviceSuitCategoryCompleted += (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        cp_AdviceSuitTypeList = e.Result;
                        if (count ==1)
                        {
                            AddTree(String.Empty, null, e.Result.ToList(), e.Result.ToList());
                        }
                        if (cp_AdviceSuitTypeList != null)
                        {
                            gvW_AdviceSuitCategory.ItemsSource = cp_AdviceSuitTypeList;
                        }
                    }
                };
                client.InsertAndSelectCP_AdviceSuitCategoryAsync(AdviceSuitCategory, String.Empty);
                count++;
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 创建树
        /// </summary>
        /// <param name="parentId">父类编号</param>
        /// <param name="subitem">当前树节点</param>
        /// <param name="CategoryList">数据源</param>
        /// <param name="sunCategoryList">符合条件的数据源</param>
        private void AddTree(String parentId, RadTreeViewItem subitem, List<CP_AdviceSuitCategory> CategoryList, List<CP_AdviceSuitCategory> sunCategoryList)
        {
            if (CategoryList == null)
            {
                return; 
            }
            int i_cout = 0;
            foreach (CP_AdviceSuitCategory row in sunCategoryList.Where(c => c.ParentID.Equals(parentId)))
            {
                RadTreeViewItem item1 = new RadTreeViewItem();
                item1.Header = row.Name;
                item1.Tag = row;

                if (subitem == null)
                {
                    //创建根节点
                    tvParent.Items.Add(item1);
                }
                else
                {
                    //创建当前树节点的子节点
                    subitem.Items.Add(item1);
                    i_cout++;
                }
                if (i_cout < 1)
                {
                    AddTree(row.CategoryId, item1, CategoryList, CategoryList.Where(c => c.ParentID.Equals(row.CategoryId)).ToList());
                }
                else
                {
                    return;
                }
            }
        }

        /// <summary>
        /// 表示控制按钮状态的方法
        /// </summary>
        private void BindBtnState()
        {
            if (m_funstate == EditState.Add)
            {
                ClearTextBox();
                SetEnabled(true, false);
            }
            else if (m_funstate == EditState.Edit)
            {
                SetEnabled(true, false);
            }
            else
            {
                SetEnabled(false, true);
            }
        }
        /// <summary>
        /// 设置启用状态
        /// </summary>
        /// <param name="bl1">状态1</param>
        /// <param name="bl2">状态2</param>
        private void SetEnabled(Boolean bl1, Boolean bl2)
        {
            txtName.IsEnabled = txtMemo.IsEnabled = bl1;
            this.btnAdd.IsEnabled = bl2;
            this.btnDel.IsEnabled = bl2;
            this.btnUpdate.IsEnabled = bl2;
             
            this.btnClear.IsEnabled = bl1;

            this.btnSave.IsEnabled = bl1;
        }
        /// <summary>
        /// 清空文本
        /// </summary>
        private void ClearTextBox()
        {
            txtCategoryId.Text = "自动生成";
            txtName.Text = txtMemo.Text = String.Empty;
        }
        #endregion
        #region 变量
        EditState m_funstate;//按钮状态
        CP_AdviceSuitCategory row = null; //接受ParentID
        ObservableCollection<CP_AdviceSuitCategory> cp_AdviceSuitTypeList;//医嘱套餐分类集合
        //List<CP_AdviceSuitCategory> result = null;
        CP_AdviceSuitCategory adviceSuitCategory;//gridview选中行
        int count = 0; //判断tree是第几次加载
        public String CategoryId
        {
            get
            {
                if (tvParent.SelectedItem == null)
                {
                    return null;
                }
                if (((RadTreeViewItem)tvParent.SelectedItem).Tag is CP_AdviceSuitCategory)
                {
                    CP_AdviceSuitCategory Category = (CP_AdviceSuitCategory)((RadTreeViewItem)tvParent.SelectedItem).Tag;
                    return Category.CategoryId;
                }
                return null;
            }
        }
        #endregion
    }
}

