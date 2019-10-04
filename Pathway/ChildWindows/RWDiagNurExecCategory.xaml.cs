using System;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Views.ChildWindows
{
    public partial class RWDiagNurExecCategory
    {

        //add by luff 20130403
        #region 事件
        /// <summary>
        /// 删除按钮事件
        /// </summary>
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.View;
                BindBtnState();
                if (diagneCategory == null)
                {
                    PublicMethod.RadAlterBox("请选中要删除的行！", "提示");
                    return;
                }
                #region 删除提示
                //DialogParameters parameters = new DialogParameters(); 
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


                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void OnDeleteMaster(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                //YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                //client.CheckNurExecCategoryDeleteCompleted += (objt, rea) =>
                //{
                //    if (rea.Result > 0)
                //    {
                //        PublicMethod.RadAlterBox("该数据已在其他地方使用，不能删除！", "提示");
                //        return;
                //    }
                //    #region 删除
                //    client.DeleteNurExecCategoryCompleted += (obj, ea) =>
                //    {
                //        if (ea.Error == null)
                //        {

                //            diagneCategoryList.Remove(diagneCategory);
                //            ClearTextBox();
                //            PublicMethod.RadAlterBox("删除成功！", "提示");
                //        }
                //        else
                //        {
                //            PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                //        }
                //    };
                //    client.DeleteNurExecCategoryAsync(diagneCategory.Lbxh);
                //    #endregion
                //};
                //client.CheckNurExecCategoryDeleteAsync(diagneCategory.Lbxh);
            }
        }
        /// <summary>
        /// 修改按钮事件
        /// </summary>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.rgvExecCategory.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选中要修改的行！", "提示");
                    return;
                }
                diagneCategory = (CP_DiagNurExecCategory)rgvExecCategory.SelectedItem;
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
                this.rgvExecCategory.SelectedItem = null;
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
                m_funstate = EditState.View;
                //rgvExecCategory.SelectedItem = null;
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
                    PublicMethod.RadAlterBoxRe("项目名称不能为空！", "提示", txtName); isLoad = false;
                    return;
                }
                //foreach (CP_DiagNurExecCategory item in rgvExecCategory.Items)
                //{
                //    if (txtName.Text.Trim() == item.LbName)
                //    {
                //        PublicMethod.RadAlterBoxRe("该项目名称已经存在", "提示", txtName);
                //        return;
                //    }
                //}
                #region 初始化CP_DiagNurExecCategory对象并赋值
                CP_DiagNurExecCategory _diagneCategory = new CP_DiagNurExecCategory();

                _diagneCategory.LbName = txtName.Text;

                if (cmbYxjl.Text == "无效")
                {
                    _diagneCategory.Yxjl = 0;

                }
                else
                {
                    _diagneCategory.Yxjl = 1;

                }
                _diagneCategory.Create_User = Global.LogInEmployee.Zgdm;
                _diagneCategory.Cancel_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _diagneCategory.Create_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _diagneCategory.Cancel_User = String.Empty;
                #endregion
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                if (m_funstate == EditState.Add)
                {

                    #region 验证分类名称是否重复
                    client.InsertDiagNurCategoryCompleted += (obj, ea) =>
                        {
                            if (ea.Result == 2)
                            {
                                PublicMethod.RadAlterBoxRe("该项目已存在，请重新输入！", "提示", txtName); isLoad = false;
                                return;
                            }
                            else if (ea.Result == 1)
                            {
                                //m_funstate = EditState.None;
                                rgvExecCategory.SelectedItem = null;
                                BindBtnState();
                                ClearTextBox();
                                GetExecCategory();
                                PublicMethod.RadAlterBox("添加成功！", "提示");

                            }
                            else
                            {
                                PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                            }

                        };
                    client.InsertDiagNurCategoryAsync(_diagneCategory);
                    m_funstate = EditState.View;
                    #endregion
                }
                if (m_funstate == EditState.Edit)
                {

                    _diagneCategory.Lbxh = diagneCategory.Lbxh;
                    _diagneCategory.Cancel_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    _diagneCategory.Create_Time = diagneCategory.Create_Time;
                    client.UpdateDiagNurExecCategoryCompleted += (obj, ea) =>
                        {
                            //if (ea.Result == 2)
                            //{
                            //    PublicMethod.RadAlterBoxRe("该项目已存在，请重新输入！", "提示", txtName); isLoad = false;
                            //    return;
                            //}
                            if (ea.Result == 1)
                            {

                                rgvExecCategory.SelectedItem = null;
                                NewAdviceGroupDetail();
                                BindBtnState();
                                GetExecCategory();
                                PublicMethod.RadAlterBox("修改成功！", "提示");
                            }
                            else
                            {
                                PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                            }
                        };
                    client.UpdateDiagNurExecCategoryAsync(_diagneCategory);
                    m_funstate = EditState.View;

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
            //BindcmbXmxh();
            //绑定GirdView控件
            GetExecCategory();
            m_funstate = EditState.View;
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
                    diagneCategory = null;
                    return;
                }

                if (rgvExecCategory.SelectedItem == null)
                {
                    return;
                }
                if (m_funstate == EditState.View)
                {
                    #region 文本框赋值
                    diagneCategory = (CP_DiagNurExecCategory)rgvExecCategory.SelectedItem;
                    txtName.Text = diagneCategory.LbName;
                    cmbYxjl.Text = diagneCategory.Yxjl.ToString();
                    #endregion
                }


            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #region  输入框加回车事件
        private void RegisterKeyEvent()
        {

            txtName.KeyUp += new KeyEventHandler(txtName_KeyUp);
            cmbYxjl.KeyUp += new KeyEventHandler(cmbYxjl_KeyUp);
            btnSave.KeyUp += new KeyEventHandler(btnSave_KeyUp);

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
        public RWDiagNurExecCategory()
        {
            InitializeComponent();
            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
            };
            RegisterKeyEvent();
        }



        /// <summary>
        /// 获取诊疗护理执行类别表
        /// </summary>
        private void GetExecCategory()
        {
            try
            {

                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.GetDiagNurExecCategoryCompleted += (obj, ea) =>
                {
                    if (ea.Error == null)
                    {
                        diagneCategoryList = ea.Result;
                        rgvExecCategory.ItemsSource = ea.Result;
                    }
                };
                client.GetDiagNurExecCategoryAsync(1, true);
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
            //txtLbxh.Text = "自动生成";
            txtName.Text = String.Empty;

            cmbYxjl.SelectedIndex = 0;
            //m_funstate = EditState.View;
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
        ObservableCollection<CP_DiagNurExecCategory> diagneCategoryList;
        /// <summary>
        /// 按钮状态
        /// </summary>
        EditState m_funstate;
        /// <summary>
        /// 选中的检查项分类信息
        /// </summary>
        CP_DiagNurExecCategory diagneCategory;
        #endregion
        /// <summary>
        /// 清空控件
        /// </summary>
        void NewAdviceGroupDetail()
        {
            //cbxInsert.SelectedIndex = -1;
            txtName.Text = "";

        }

    }
}

