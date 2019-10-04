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
    public partial class HlXMNurExecCategory
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
                DialogParameters parameters = new DialogParameters();/* uodate by dxj 2011-7-26 删除提示 */
                parameters.Content = String.Format("提示: {0}", "确定删除吗？删除后不能恢复!");
                parameters.Header = "提示";
                parameters.IconContent = null;
                parameters.OkButtonContent = "确定";// @"/Pathway;component/Images/确定.png";
                parameters.CancelButtonContent = "取消";
                parameters.Closed = OnDeleteMaster;
                RadWindow.Confirm(parameters);
                #endregion
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
                //client.CheckNurExecCategoryDeleteCompleted += (objt, rea) =>
                //{
                //    if (rea.Result > 0)
                //    {
                //        PublicMethod.RadAlterBox("该数据已在其他地方使用，不能删除！", "提示");
                //        return;
                //    }
                #region 删除
                client.DelHlxmCodeListCompleted += (obj, ea) =>
                {
                    if (ea.Error == null)
                    {

                        BindGridView();
                        GridView.SelectedItem = null;
                        ClearTextBox();
                        PublicMethod.RadAlterBox("删除成功！", "提示");
                    }
                    else
                    {
                        PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                    }
                };
                client.DelHlxmCodeListAsync(nurExecCategory.Xmxh);
                #endregion
                //};
                //client.CheckNurExecCategoryDeleteAsync(nurExecCategory.Xmxh);
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
                this.txtXmxh.IsEnabled = false;
                txtOrderValue.Focus();
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
                txtOrderValue.Focus();
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
                GridView.SelectedItem = null;
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
                if (txtOrderValue.Text.Trim() == "")
                {
                    PublicMethod.RadAlterBoxRe("排序字段不能为空！", "提示", txtOrderValue);
                    isLoad = false;
                    return;
                }
                int n;
                if (!int.TryParse(txtOrderValue.Text, out  n))
                {
                    PublicMethod.RadAlterBoxRe("排序字段必须为数字！", "提示", txtOrderValue);
                    isLoad = false;
                    return;
                }
                if (txtXmxh.Text.Trim() == "")
                {
                    PublicMethod.RadAlterBoxRe("项目编码不能为空！", "提示", txtXmxh);
                    isLoad = false;
                    return;
                }
                if (txtXmxh.Text.Length > 12)
                {
                    PublicMethod.RadAlterBoxRe("项目编码长度超出范围！", "提示", txtXmxh);
                    isLoad = false;
                    return;
                }
                if (txtName.Text.Trim() == "")
                {
                    PublicMethod.RadAlterBoxRe("项目名称不能为空！", "提示", txtName);
                    isLoad = false;
                    return;
                }
                if (txtName.Text.Length > 12)
                {
                    PublicMethod.RadAlterBoxRe("项目名称长度超出范围！", "提示", txtName);
                    isLoad = false;
                    return;
                }

                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                if (m_funstate == EditState.Add)
                {
                    #region 实体赋值
                    PE_Hlxm h1 = new PE_Hlxm();
                    h1.Xmxh = txtXmxh.Text;
                    h1.Name = txtName.Text;
                    h1.OrderValue = txtOrderValue.Text;
                    h1.Yxjl = "1";
                    //h1.Yxjl = (cmbYxjl.Text == "有效") ? "1" : "0";
                    h1.Create_User = Global.LogInEmployee.Zgdm; //"";//
                    h1.Cancel_User = String.Empty;
                    #endregion

                    #region 验证分类名称是否重复
                    client.CheckAddHlxmCodeBeingCompleted += (obj, ea) =>
                        {
                            if (ea.Result > 0)
                            {
                                PublicMethod.RadAlterBox("该项目编码已存在！", "提示");
                                return;
                            }
                            #region 添加数据
                            client.InsertHlxmCodeListCompleted += (obj1, ea1) =>
                                {
                                    if (ea1.Result)
                                    {
                                        //
                                        BindGridView();
                                        m_funstate = EditState.None;
                                        GridView.SelectedItem = null;
                                        BindBtnState();
                                        ClearTextBox();
                                        PublicMethod.RadAlterBox("添加成功！", "提示");
                                    }
                                    else
                                    {
                                        PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                                    }
                                };
                            client.InsertHlxmCodeListAsync(h1.Xmxh, h1.Name, h1.OrderValue, h1.Yxjl, h1.Create_User);
                            #endregion
                        };
                    client.CheckAddHlxmCodeBeingAsync(h1.Xmxh);
                    #endregion
                }
                if (m_funstate == EditState.Edit)
                {
                    PE_Hlxm h1 = new PE_Hlxm();
                    h1.Xmxh = txtXmxh.Text;
                    h1.Name = txtName.Text;
                    h1.OrderValue = txtOrderValue.Text;
                    h1.Yxjl = "1";
                    //h1.Yxjl = (cmbYxjl.Text == "有效") ? "1" : "0";
                    h1.Cancel_User = Global.LogInEmployee.Zgdm;// "";
                    client.CheckAddHlxmCodeBeingCompleted += (objc, rea) =>
                        {
                            if (rea.Result > 0)
                            {
                                //PublicMethod.RadAlterBox("该护理项目编码已存在！", "提示");
                                //return;

                                client.UpdateHlxmCodeListCompleted += (obj, ea) =>
                                    {
                                        if (ea.Error == null)
                                        {
                                            BindGridView();
                                            m_funstate = EditState.None;
                                            BindBtnState();
                                            GridView.SelectedItem = null;
                                            PublicMethod.RadAlterBox("修改成功！", "提示");
                                        }
                                        else
                                        {
                                            PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                                        }
                                    };
                                client.UpdateHlxmCodeListAsync(h1.Xmxh, h1.Name, h1.OrderValue, h1.Yxjl, h1.Cancel_User);

                            }
                            else
                            { return; }
                        };
                    client.CheckAddHlxmCodeBeingAsync(this.txtXmxh.Text);
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
            BindGridView();
        }

        /// <summary>
        /// 行选中改变事件
        /// </summary>
        private void GridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                if (m_funstate == EditState.Add)
                {
                    return;
                }
                nurExecCategory = (PE_Hlxm)GridView.SelectedItem;
                if (nurExecCategory == null)
                {
                    return;
                }
                #region 文本框赋值
                this.txtOrderValue.Text = nurExecCategory.OrderValue;
                this.txtXmxh.Text = nurExecCategory.Xmxh;
                this.txtName.Text = nurExecCategory.Name;
                //this.cmbYxjl.SelectedValue = nurExecCategory.Yxjl;
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
            txtOrderValue.KeyUp += new KeyEventHandler(txtOrderValue_KeyUp);
            txtXmxh.KeyUp += new KeyEventHandler(txtXmxh_KeyUp);
            txtName.KeyUp += new KeyEventHandler(txtName_KeyUp);

            //cmbYxjl.KeyUp += new KeyEventHandler(cmbYxjl_KeyUp);
            btnSave.KeyUp += new KeyEventHandler(btnSave_KeyUp);
        }

        private void txtOrderValue_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtXmxh.Focus();
        }

        private void txtXmxh_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtName.Focus();
        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
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



        #region 方法
        public HlXMNurExecCategory()
        {
            InitializeComponent();
            LayoutRootXM.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };
            RegisterKeyEvent(); //加回车
        }

        private void BindGridView()
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client = PublicMethod.YidanClient;
            client.GetPE_HlxmListCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            GridView.ItemsSource = e.Result;
                            BindPE_Fun(null);
                            //PublicMethod.RadAlterBox("修改成功！", "提示");
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };

            client.GetPE_HlxmListAsync();
            client.CloseAsync();
        }

        /// <summary>
        /// 绑定页面中编辑信息
        /// </summary>
        /// <param name="pe_fun">实体，如果为空编辑区域值为空</param>
        private void BindPE_Fun(PE_Hlxm userrole_fun)
        {
            if (userrole_fun != null)
            {
                this.txtOrderValue.Text = userrole_fun.OrderValue;
                this.txtXmxh.Text = userrole_fun.Xmxh;
                this.txtName.Text = userrole_fun.Name;
                //this.cmbYxjl.SelectedValue = userrole_fun.Yxjl;
            }
            else
            {
                this.txtOrderValue.Text = "";
                this.txtXmxh.Text = "";
                this.txtName.Text = "";
                //this.cmbYxjl.Text = "";
            }
        }

        /// <summary>
        /// 设置启用状态
        /// </summary>
        /// <param name="bl1">状态1</param>
        /// <param name="bl2">状态2</param>
        private void SetEnabled(Boolean bl1, Boolean bl2)
        {
            txtOrderValue.IsEnabled = txtXmxh.IsEnabled = txtName.IsEnabled = btnSave.IsEnabled = bl1; //cmbYxjl.IsEnabled = 
            this.btnAdd.IsEnabled = bl2;
            this.btnDel.IsEnabled = bl2;
            this.btnUpdate.IsEnabled = bl2;
            this.btnClear.IsEnabled = bl1;
        }
        /// <summary>
        /// 清空文本
        /// </summary>
        private void ClearTextBox()
        {
            //txtLbxh.Text = "自动生成";
            txtOrderValue.Text = txtXmxh.Text = txtName.Text = String.Empty;
            //cmbYxjl.SelectedIndex = 0;
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
                //cmbYxjl.IsEnabled = false;
            }
            else if (m_funstate == EditState.Edit)
            {
                SetEnabled(true, false);
                //cmbYxjl.IsEnabled = true;
            }
            else
            {
                SetEnabled(false, true);
                //cmbYxjl.IsEnabled = false;
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
        PE_Hlxm nurExecCategory;
        #endregion
    }
}

