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
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;

using System.Collections.ObjectModel;
using YidanSoft.Tool;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Views
{
    /// <summary>
    /// 医嘱套餐分类维护
    /// </summary>
    public partial class W_AdviceSuitCategory : Page
    {
       
        #region 事件

        /// <summary>
        /// 页面加载事件
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            GetCP_AdviceSuitType();
        }
        /// <summary>
        /// girdview选则改变
        /// </summary>
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
            #endregion
        }
        /// <summary>
        /// 删除按钮事件
        /// </summary>
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.None;
                BindBtnState();
                if (adviceSuitCategory == null)
                {
                    PublicMethod.RadAlterBox("请选中一条数据!", "提示");
                    return;
                }
                //DialogParameters parameters = new DialogParameters();/* update by dxj 2011/7/26 删除提示*/
                //parameters.Content = String.Format("{0}", "确定删除吗？删除后不能恢复!");
                //parameters.Header = "提示";
                //parameters.IconContent = null;
                //parameters.OkButtonContent = "确定";
                //parameters.CancelButtonContent = "取消";
                //parameters.Closed = OnDeleteMaster;
                //RadWindow.Confirm(parameters); 
                YidanPathWayMessageBox mess = new YidanPathWayMessageBox("确定删除吗？删除后不能恢复!", "提示", YiDanMessageBoxButtons.YesNo);
                mess.ShowDialog();
                mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
                
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
                        if (rea.Result > 0)
                        {
                            PublicMethod.RadAlterBox("该分类已在其他地方使用，无法删除!", "提示");
                            return;
                        }
                        client.DeleteCP_AdviceSuitCategoryCompleted += (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {

                                cp_AdviceSuitTypeList.Remove(adviceSuitCategory);
                                adviceSuitCategory = null;
                                ClearTextBox();
                                PublicMethod.RadAlterBox("删除成功!", "提示");
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

        void OnDeleteMaster(object sender, WindowClosedEventArgs e)/* update by dxj 2011/7/26 删除提示*/
        {
            if (e.DialogResult == true)
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.DeleteCheckInfoCompleted += (objc, rea) =>
                {
                    if (rea.Result > 0)
                    {
                        PublicMethod.RadAlterBox("该分类已在其他地方使用，无法删除!", "提示");
                        return;
                    }
                    client.DeleteCP_AdviceSuitCategoryCompleted += (obj, ea) =>
                    {
                        if (ea.Error == null)
                        {
                            
                            cp_AdviceSuitTypeList.Remove(adviceSuitCategory);
                            adviceSuitCategory = null;
                            ClearTextBox();
                            PublicMethod.RadAlterBox("删除成功!", "提示");
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

        /// <summary>
        /// 修改按钮事件
        /// </summary>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (adviceSuitCategory==null)
                {
                    PublicMethod.RadAlterBox("请选择一行数据!", "提示");
                    return;
                }
                m_funstate = EditState.Edit;
                BindBtnState();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 添加按钮事件
        /// </summary>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.Add;
                BindBtnState();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 取消按钮事件
        /// </summary>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.None;
                BindBtnState();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 保存按钮事件
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtName.Text.Trim()==String.Empty)
                {
                    PublicMethod.RadAlterBox("类别名称不能为空！", "提示");
                    return;
                }
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                if (m_funstate == EditState.Add)
                {
                    #region 实体赋值
                    CP_AdviceSuitCategory adviceSuitCategory = new CP_AdviceSuitCategory();
                    adviceSuitCategory.CategoryId = ConvertMy.ToString(Guid.NewGuid());
                    adviceSuitCategory.Name = txtName.Text;
                    adviceSuitCategory.Memo = txtMemo.Text;
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
                                    if (rea.Result.ToList()!=null)
                                    {
                                        
                                        cp_AdviceSuitTypeList = rea.Result;
                                        gvW_AdviceSuitCategory.ItemsSource = cp_AdviceSuitTypeList;
                                        m_funstate = EditState.None;
                                        BindBtnState();
                                        ClearTextBox();
                                        adviceSuitCategory = null;
                                        PublicMethod.RadAlterBox("添加成功！", "提示");
                                    }
                                };
                            client.InsertAndSelectCP_AdviceSuitCategoryAsync(adviceSuitCategory,String.Empty);
                            #endregion
                        };
                    client.InsertCheckInfoAsync(Name);
                }
                if (m_funstate==EditState.Edit)
                {
                    #region 改变实体的值
                    adviceSuitCategory.Name = txtName.Text;
                    adviceSuitCategory.Yxjl = "1";
                    adviceSuitCategory.Memo = txtMemo.Text;
                    #endregion
                    client.UpdateCheckInfoCompleted += (obj, ea) =>
                        {
                            if (ea.Result > 0)
                            {
                                PublicMethod.RadAlterBox("该分类名称已存在！", "提示");
                                return;
                            }
                            #region 修改分类
                            client.UpdateCP_AdviceSuitCategoryCompleted += (objc, rea) =>
                                {
                                    if (rea.Error == null)
                                    {
                                        
                                        m_funstate = EditState.None;
                                        BindBtnState();
                                        PublicMethod.RadAlterBox("修改成功！", "提示");
                                    }
                                    else
                                    {
                                        PublicMethod.RadAlterBox(rea.Error.ToString(), "提示");
                                    }
                                };
                            client.UpdateCP_AdviceSuitCategoryAsync(adviceSuitCategory);
                            #endregion
                        };
                    client.UpdateCheckInfoAsync(adviceSuitCategory.Name, adviceSuitCategory.CategoryId);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region 方法
        /// <summary>
        /// 获取分类信息
        /// </summary>
        private void GetCP_AdviceSuitType()
        {
            try
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                CP_AdviceSuitCategory AdviceSuitCategory = new CP_AdviceSuitCategory();
                AdviceSuitCategory.CategoryId=String.Empty;
                AdviceSuitCategory.Name = String.Empty;
                AdviceSuitCategory.Memo = String.Empty;
                AdviceSuitCategory.Zgdm = String.Empty;
                client.InsertAndSelectCP_AdviceSuitCategoryCompleted += (obj, e) => 
                {
                    if (e.Error==null)
                    {
                        cp_AdviceSuitTypeList = e.Result;
                        if (cp_AdviceSuitTypeList!=null)
                        {
                            gvW_AdviceSuitCategory.ItemsSource = cp_AdviceSuitTypeList;
                        }
                    }
                };
                client.InsertAndSelectCP_AdviceSuitCategoryAsync(AdviceSuitCategory,String.Empty);
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        public W_AdviceSuitCategory()
        {
            InitializeComponent();
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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
        ObservableCollection<CP_AdviceSuitCategory> cp_AdviceSuitTypeList;//医嘱套餐分类集合
        CP_AdviceSuitCategory adviceSuitCategory;//gridview选中行
        #endregion
    }
}
