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
using YidanEHRApplication.Models;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Helpers;
using YidanSoft.Tool;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls;

namespace YidanEHRApplication.Views.ChildWindows
{
    /// <summary>
    /// 表示护理执行类别维护的类
    /// </summary>
    public partial class HlNurExecCategory
    {
        #region 事件
        /// <summary>
        /// 删除按钮事件
        /// </summary>
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.None;
                BindBtnState();
                if (nurExecCategory == null)
                {
                    PublicMethod.RadAlterBox("请选中要删除的行！", "提示");
                    return;
                }
                #region 删除提示
                //DialogParameters parameters = new DialogParameters();/* uodate by dxj 2011-7-26 删除提示 */
                //parameters.Content = String.Format("提示: {0}", "确定删除吗？删除后不能恢复!");
                //parameters.Header = "提示";
                //parameters.IconContent = null;
                //parameters.OkButtonContent = "确定";// @"/Pathway;component/Images/确定.png";
                //parameters.CancelButtonContent = "取消";
                //parameters.Closed = OnDeleteMaster;
                //RadWindow.Confirm(parameters);

                YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                mess.ShowDialog();
                mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
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
                    client.CheckNurExecCategoryDeleteCompleted += (objt, rea) =>
                    {
                        if (rea.Result > 0)
                        {
                            PublicMethod.RadAlterBox("该数据已在其他地方使用，不能删除！", "提示");
                            return;
                        }
                        #region 删除
                        client.DeleteNurExecCategoryCompleted += (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {

                                nurExecCategoryList.Remove(nurExecCategory);
                                ClearTextBox();
                                PublicMethod.RadAlterBox("删除成功！", "提示");
                            }
                            else
                            {
                                PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                            }
                        };
                        client.DeleteNurExecCategoryAsync(nurExecCategory.Lbxh);
                        #endregion
                    };
                    client.CheckNurExecCategoryDeleteAsync(nurExecCategory.Lbxh);

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        void OnDeleteMaster(object sender, WindowClosedEventArgs e)/* uodate by dxj 2011-7-26 删除提示 */
        {
            if (e.DialogResult == true)
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.CheckNurExecCategoryDeleteCompleted += (objt, rea) =>
                {
                    if (rea.Result > 0)
                    {
                        PublicMethod.RadAlterBox("该数据已在其他地方使用，不能删除！", "提示");
                        return;
                    }
                    #region 删除
                    client.DeleteNurExecCategoryCompleted += (obj, ea) =>
                    {
                        if (ea.Error == null)
                        {

                            nurExecCategoryList.Remove(nurExecCategory);
                            ClearTextBox();
                            PublicMethod.RadAlterBox("删除成功！", "提示");
                        }
                        else
                        {
                            PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                        }
                    };
                    client.DeleteNurExecCategoryAsync(nurExecCategory.Lbxh);
                    #endregion
                };
                client.CheckNurExecCategoryDeleteAsync(nurExecCategory.Lbxh);
            }
        }
        /// <summary>
        /// 修改按钮事件
        /// </summary>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.nurExecCategory == null)
                {
                    PublicMethod.RadAlterBox("请选中要修改的行！", "提示");
                    return;
                }
                m_funstate = EditState.Edit;
                BindBtnState();
                txtName.Focus();
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
                txtName.Focus();
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
                rgvExecCategory.SelectedItem = null;
                BindBtnState();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 表示保存按钮点击事件
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtName.Text.Trim() == "")
                {
                    PublicMethod.RadAlterBoxRe("类别名称不能为空！", "提示", txtName); isLoad = false;
                    return;
                }
                if (txtName.Text.Trim().Length >= 12)
                {
                    PublicMethod.RadAlterBoxRe("类别名称长度不能超过12字节！", "提示", txtName); isLoad = false;
                    return;
                }
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                if (m_funstate == EditState.Add)
                {
                    #region 实体赋值
                    CP_NurExecCategory cpnec = new CP_NurExecCategory();
                    cpnec.Lbxh = Guid.NewGuid().ToString();
                    cpnec.Name = txtName.Text;
                    cpnec.Yxjl = "1";
                    cpnec.Xmxh = ConvertMy.ToString(cmbXmxh.SelectedValue);
                    cpnec.Create_User = Global.LogInEmployee.Zgdm;
                    cpnec.Cancel_User = String.Empty;
                    #endregion

                    #region 验证分类名称是否重复
                    client.CheckNurExecCategoryInsertCompleted += (obj, ea) =>
                        {
                            if (ea.Result > 0)
                            {
                                PublicMethod.RadAlterBoxRe("该分类名称已存在！", "提示", txtName); isLoad = false;
                                return;
                            }
                            #region 添加数据
                            client.InsertAndSelectNurExecCategoryCompleted += (obj1, ea1) =>
                                {
                                    if (ea1.Result.ToList() != null)
                                    {

                                        nurExecCategoryList = ea1.Result;
                                        rgvExecCategory.ItemsSource = nurExecCategoryList;
                                        m_funstate = EditState.None;
                                        rgvExecCategory.SelectedItem = null;
                                        BindBtnState();
                                        ClearTextBox();
                                        PublicMethod.RadAlterBox("添加成功！", "提示");
                                    }
                                    else
                                    {
                                        PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                                    }
                                };
                            client.InsertAndSelectNurExecCategoryAsync(cpnec);
                            #endregion
                        };
                    client.CheckNurExecCategoryInsertAsync(cpnec.Name);
                    #endregion
                }
                if (m_funstate == EditState.Edit)
                {
                    #region 验证名称是否重复
                    client.CheckNurExecCategoryUpdateCompleted += (objc, rea) =>
                        {
                            if (rea.Result > 0)
                            {
                                PublicMethod.RadAlterBoxRe("该分类名称已存在！", "提示", txtName);
                                isLoad = false;
                                return;
                            }
                            #region 改变实体的值
                            nurExecCategory.Name = txtName.Text;
                            nurExecCategory.Xmxh = ConvertMy.ToString(cmbXmxh.SelectedValue);
                            nurExecCategory.XmxhName = ConvertMy.ToString(cmbXmxh.SelectionBoxItem);
                            if (cmbYxjl.Text == "无效")
                            {
                                nurExecCategory.Yxjl = "0";
                                nurExecCategory.Yxjlmc = "无效";
                            }
                            else
                            {
                                nurExecCategory.Yxjl = "1";
                                nurExecCategory.Yxjlmc = "有效";
                            }
                            nurExecCategory.Cancel_User = Global.LogInEmployee.Zgdm;
                            nurExecCategory.Cancel_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                            #endregion
                            #region 修改数据
                            client.UpdateNurExecCategoryCompleted += (obj, ea) =>
                                {
                                    if (ea.Error == null)
                                    {

                                        m_funstate = EditState.None;
                                        rgvExecCategory.SelectedItem = null;
                                        BindBtnState();
                                        PublicMethod.RadAlterBox("修改成功！", "提示");
                                    }
                                    else
                                    {
                                        PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                                    }
                                };
                            client.UpdateNurExecCategoryAsync(nurExecCategory);
                            #endregion
                        };
                    client.CheckNurExecCategoryUpdateAsync(txtName.Text, nurExecCategory.Lbxh);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        public bool isLoad = true;
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoad)
            {
                isLoad = true;
                return;
            }
            BindcmbXmxh();
            GetExecCategory();
        }

        /// <summary>
        /// 行选中改变事件
        /// </summary>
        private void rgvExecCategory_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                if (m_funstate == EditState.Add)
                {
                    return;
                }
                nurExecCategory = (CP_NurExecCategory)rgvExecCategory.SelectedItem;
                if (nurExecCategory == null)
                {
                    return;
                }
                #region 文本框赋值
                txtLbxh.Text = nurExecCategory.Lbxh;
                txtName.Text = nurExecCategory.Name;
                //cmbYxjl.Text = nurExecCategory.Yxjlmc;
                cmbXmxh.SelectedValue = nurExecCategory.Xmxh;
                #endregion
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #region  输入框加回车事件
        private void RegisterKeyEvent()
        {
            txtLbxh.KeyUp += new KeyEventHandler(txtLbxh_KeyUp);
            txtName.KeyUp += new KeyEventHandler(txtName_KeyUp);
            cmbXmxh.KeyUp += new KeyEventHandler(cmbXmxh_KeyUp);

            cmbYxjl.KeyUp += new KeyEventHandler(cmbYxjl_KeyUp);
            btnSave.KeyUp += new KeyEventHandler(btnSave_KeyUp);

        }

        private void txtLbxh_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtName.Focus();
        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cmbXmxh.Focus();
        }

        private void cmbXmxh_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cmbYxjl.Focus();
        }

        private void cmbYxjl_KeyUp(object sender, KeyEventArgs e)
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

        #region 方法
        public HlNurExecCategory()
        {
            InitializeComponent();
            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
            };
            RegisterKeyEvent();
        }

        /// <summary>
        /// 为cmbXmxh绑定值
        /// </summary>
        private void BindcmbXmxh()
        {
            try
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.GetNurExecItemListCompleted += (obj, ea) =>
                    {
                        if (ea.Error == null)
                        {
                            cmbXmxh.ItemsSource = ea.Result.ToList();
                            cmbXmxh.SelectedValuePath = "Xmxh";
                            cmbXmxh.DisplayMemberPath = "Name";
                            if (ea.Result.Count > 0)
                            {
                                cmbXmxh.SelectedIndex = 0;
                            }
                        }
                    };
                client.GetNurExecItemListAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        private void GetExecCategory()
        {
            try
            {
                CP_NurExecCategory cnec = new CP_NurExecCategory();
                cnec.Lbxh = String.Empty;
                cnec.Name = String.Empty;
                cnec.Xmxh = String.Empty;
                cnec.Yxjl = String.Empty;
                cnec.Create_User = String.Empty;
                cnec.Cancel_User = String.Empty;
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.InsertAndSelectNurExecCategoryCompleted += (obj, ea) =>
                {
                    if (ea.Error == null)
                    {
                        nurExecCategoryList = ea.Result;
                        rgvExecCategory.ItemsSource = nurExecCategoryList;
                    }
                };
                client.InsertAndSelectNurExecCategoryAsync(cnec);
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 设置启用状态
        /// </summary>
        /// <param name="bl1">状态1</param>
        /// <param name="bl2">状态2</param>
        private void SetEnabled(Boolean bl1, Boolean bl2)
        {
            txtName.IsEnabled = cmbXmxh.IsEnabled = btnSave.IsEnabled = bl1;
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
            txtLbxh.Text = "自动生成";
            txtName.Text = String.Empty;
            cmbXmxh.SelectedIndex = 0;
            cmbYxjl.SelectedIndex = 0;
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
                cmbYxjl.IsEnabled = false;
            }
            else if (m_funstate == EditState.Edit)
            {
                SetEnabled(true, false);
                cmbYxjl.IsEnabled = true;
            }
            else
            {
                SetEnabled(false, true);
                cmbYxjl.IsEnabled = false;
            }
        }
        #endregion

        #region 变量
        /// <summary>
        /// 检查项分类集合
        /// </summary>
        ObservableCollection<CP_NurExecCategory> nurExecCategoryList;
        /// <summary>
        /// 按钮状态
        /// </summary>
        EditState m_funstate;
        /// <summary>
        /// 选中的检查项分类信息
        /// </summary>
        CP_NurExecCategory nurExecCategory;
        #endregion

        private void btnHlxmEdit_Click(object sender, RoutedEventArgs e)
        {

            HlXMNurExecCategory hlxm = new HlXMNurExecCategory();
            hlxm.Closed += new EventHandler<WindowClosedEventArgs>(hlxm_Closed);
            hlxm.ResizeMode = ResizeMode.NoResize;
            hlxm.ShowDialog();
        }
        private void hlxm_Closed(object sender, EventArgs e)
        {
            BindcmbXmxh();
        }
    }
}

