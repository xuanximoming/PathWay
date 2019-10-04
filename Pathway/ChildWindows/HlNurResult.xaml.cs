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
using YidanEHRApplication.Models;
using YidanEHRApplication.DataService;
using System.Collections.ObjectModel;
using YidanSoft.Tool;
using Telerik.Windows.Controls;

namespace YidanEHRApplication.Views.ChildWindows
{
    /// <summary>
    /// 维护护理结果
    /// </summary>
    public partial class HlNurResult
    {
        #region 事件
        public bool isLoad = true;
        /// <summary>
        /// 表示窗体加载事件
        /// </summary>
        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoad)
            {
                isLoad = true;
                return;
            }
            BindNurResult();
        }
        /// <summary>
        /// 网格行选择改变事件
        /// </summary>
        private void rgvNurResult_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                if (m_funstate == EditState.Add)
                {
                    return;
                }
                nurResult = (CP_NurResult)rgvNurResult.SelectedItem;
                if (nurResult == null)
                {
                    return;
                }
                #region 文本框赋值
                txtJgbh.Text = nurResult.Jgbh;
                txtName.Text = nurResult.Name;
                cmbYxjl.Text = nurResult.Yxjlmc;
                #endregion
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 表示保存按钮的点击事件     
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtName.Text.Trim() == "")
                {
                    PublicMethod.RadAlterBoxRe("结果名称不能为空！", "提示", txtName);
                    isLoad = false;
                    return;
                }
                if (txtName.Text.Trim().Length>12)
                {
                    PublicMethod.RadAlterBoxRe("结果名称长度不能超过12个字节！", "提示", txtName);
                    isLoad = false;
                    return;
                }
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                if (m_funstate == EditState.Add)
                {
                    #region 实体赋值
                    CP_NurResult nr = new CP_NurResult();
                    nr.Name = txtName.Text;
                    nr.Yxjl = "1";
                    nr.Create_User = Global.LogInEmployee.Zgdm;
                    nr.Update_User = String.Empty;
                    #endregion

                    #region 验证结果名称是否重复
                    client.CheckNurResultInsertCompleted += (obj, ea) =>
                    {
                        if (ea.Result > 0)
                        {
                            PublicMethod.RadAlterBoxRe("该结果名称已存在！", "提示", txtName);
                            isLoad = false;
                            return;
                        }
                        #region 添加数据
                        client.InsertAndSelectNurResultCompleted += (objt, rea) =>
                        {
                            if (rea.Result.ToList() != null)
                            {

                                nurResultList = rea.Result;
                                rgvNurResult.ItemsSource = nurResultList;
                                m_funstate = EditState.None;
                                BindBtnState();
                                ClearTextBox();
                                rgvNurResult.SelectedItem = null;
                                PublicMethod.RadAlterBox("添加成功！", "提示");
                            }
                            else
                            {
                                PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                            }
                        };
                        client.InsertAndSelectNurResultAsync(nr);
                        #endregion
                    };
                    client.CheckNurResultInsertAsync(nr.Name);
                    #endregion
                }
                if (m_funstate == EditState.Edit)
                {
                    #region 验证名称是否重复
                    client.CheckNurReaultUpdateCompleted += (objc, rea) =>
                    {
                        if (rea.Result > 0)
                        {
                            PublicMethod.RadAlterBoxRe("该结果名称已存在！", "提示", txtName);
                            isLoad = false;
                            return;
                        }
                        #region 改变实体的值
                        nurResult.Name = txtName.Text;
                        if (cmbYxjl.Text == "无效")
                        {
                            nurResult.Yxjl = "0";
                            nurResult.Yxjlmc = "无效";
                        }
                        else
                        {
                            nurResult.Yxjl = "1";
                            nurResult.Yxjlmc = "有效";
                        }
                        nurResult.Update_User = Global.LogInEmployee.Zgdm;
                        nurResult.Update_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                        #endregion
                        #region 修改数据
                        client.UpdateNurResultCompleted += (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {

                                m_funstate = EditState.None;
                                rgvNurResult.SelectedItem = null;
                                BindBtnState();
                                PublicMethod.RadAlterBox("修改成功！", "提示");
                            }
                            else
                            {
                                PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                            }
                        };
                        client.UpdateNurResultAsync(nurResult);
                        #endregion
                    };
                    client.CheckNurReaultUpdateAsync(txtName.Text, nurResult.Jgbh);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 表示删除按钮的点击事件
        /// </summary>
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.None;
                BindBtnState();
                if (nurResult == null)
                {
                    PublicMethod.RadAlterBox("请选中要删除的行！", "提示");
                    return;
                }
                #region 删除提示
                //DialogParameters parameters = new DialogParameters();/* uodate by dxj 2011-7-26 删除提示 */
                //parameters.Content = String.Format("提示: {0}", "确定删除吗？删除后不能恢复!");
                //parameters.Header = "提示";
                //parameters.IconContent = null;
                //parameters.OkButtonContent = "确定";
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
                    client.CheckNurResultDeleteCompleted += (objt, rea) =>
                    {
                        if (rea.Result > 0)
                        {
                            PublicMethod.RadAlterBox("该数据已在其他地方使用，不能删除！", "提示");
                            return;
                        }
                        #region 删除数据
                        client.DeleteNurResultCompleted += (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {

                                nurResultList.Remove(nurResult);
                                ClearTextBox();
                                PublicMethod.RadAlterBox("删除成功！", "提示");
                            }
                            else
                            {
                                PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                            }
                        };
                        client.DeleteNurResultAsync(nurResult.Jgbh);
                        #endregion
                    };
                    client.CheckNurResultDeleteAsync(nurResult.Jgbh);

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
                client.CheckNurResultDeleteCompleted += (objt, rea) =>
                {
                    if (rea.Result > 0)
                    {
                        PublicMethod.RadAlterBox("该数据已在其他地方使用，不能删除！", "提示");
                        return;
                    }
                    #region 删除数据
                    client.DeleteNurResultCompleted += (obj, ea) =>
                    {
                        if (ea.Error == null)
                        {

                            nurResultList.Remove(nurResult);
                            ClearTextBox();
                            PublicMethod.RadAlterBox("删除成功！", "提示");
                        }
                        else
                        {
                            PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                        }
                    };
                    client.DeleteNurResultAsync(nurResult.Jgbh);
                    #endregion
                };
                client.CheckNurResultDeleteAsync(nurResult.Jgbh);
            }
        }
        /// <summary>
        /// 表示修改按钮的点击事件 
        /// </summary>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.nurResult == null)
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
        /// 表示添加按钮的点击事件
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
        /// 表示取消按钮的点击事件
        /// </summary>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.None;
                rgvNurResult.SelectedItem = null;
                BindBtnState();
                
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #region  输入框加回车事件
        private void RegisterKeyEvent()
        {
            txtJgbh.KeyUp += new KeyEventHandler(txtJgbh_KeyUp);
            txtName.KeyUp += new KeyEventHandler(txtName_KeyUp);

            cmbYxjl.KeyUp += new KeyEventHandler(cmbYxjl_KeyUp);
            btnSave.KeyUp += new KeyEventHandler(btnSave_KeyUp);

        }

        private void txtJgbh_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtName.Focus();
        }

        private void txtName_KeyUp(object sender, KeyEventArgs e)
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
        /// <summary>
        /// 构造函数HlNurResult
        /// </summary>
        public HlNurResult()
        {
            InitializeComponent();
            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };

            RegisterKeyEvent();
        }

        /// <summary>
        /// 设置启用状态
        /// </summary>
        /// <param name="bl1">状态1</param>
        /// <param name="bl2">状态2</param>
        private void SetEnabled(Boolean bl1, Boolean bl2)
        {
            txtName.IsEnabled = btnSave.IsEnabled = bl1;
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
            txtJgbh.Text = "自动生成";
            txtName.Text = String.Empty;
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

        /// <summary>
        /// 绑定护理结果信息
        /// </summary>
        private void BindNurResult()
        {
            try
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                CP_NurResult nr = new CP_NurResult();
                nr.Name = String.Empty;
                nr.Create_User = String.Empty;
                nr.Update_User = String.Empty;
                nr.Yxjl = "1";
                client.InsertAndSelectNurResultCompleted += (obj, ea) =>
                {
                    if (ea.Error == null)
                    {
                        nurResultList = ea.Result;
                        rgvNurResult.ItemsSource = nurResultList;
                    }
                    else
                    {
                        PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                    }
                };
                client.InsertAndSelectNurResultAsync(nr);
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion

        #region 变量
        /// <summary>
        /// 护理结果集合
        /// </summary>
        ObservableCollection<CP_NurResult> nurResultList;
        /// <summary>
        /// 按钮状态
        /// </summary>
        EditState m_funstate;
        /// <summary>
        /// 选中的护理结果信息
        /// </summary>
        CP_NurResult nurResult;
        #endregion
    }
}

