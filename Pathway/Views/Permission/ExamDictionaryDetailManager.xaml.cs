using DrectSoft.Tool;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Views.Permission
{
    /// <summary>
    /// 表示检查项目维护的类
    /// </summary>
    public partial class ExamDictionaryDetailManager : Page
    {
        #region 事件
        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public bool isLoad = true;
        /// <summary>
        /// 表示页面加载的事件
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoad)
            {
                isLoad = true;
                return;
            }
            GetExamDictionaryDetail();
            BindFlbm();
        }

        /// <summary>
        /// 表示添加按钮的点击事件
        /// </summary>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.Add;
                gvExamDictionaryDetail.SelectedItem = null;
                BindBtnState();
                txtJcmc.Focus();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
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
                if (pedd == null)
                {
                    PublicMethod.RadAlterBox("请选中一条数据!", "提示");
                    return;
                }
                DialogParameters parameters = new DialogParameters();/* update by dxj 2011/7/26 删除提示*/
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
                    client.DeleteExamDictionaryDetailCompleted += (obj, ea) =>
                    {
                        if (ea.Result == 1)
                        {

                            pe_ExamDictionaryDetail.Remove(pedd);
                            ClearTextBox();
                            PublicMethod.RadAlterBox("删除成功!", "提示");
                        }
                        else
                        {
                            PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                        }
                    };
                    client.DeleteExamDictionaryDetailAsync(pedd.Jcbm);

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
                client.DeleteExamDictionaryDetailCompleted += (obj, ea) =>
                {
                    if (ea.Result == 1)
                    {

                        pe_ExamDictionaryDetail.Remove(pedd);
                        ClearTextBox();
                        PublicMethod.RadAlterBox("删除成功!", "提示");
                    }
                    else
                    {
                        PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                    }
                };
                client.DeleteExamDictionaryDetailAsync(pedd.Jcbm);
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
                gvExamDictionaryDetail.SelectedItem = null;
                BindBtnState();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 表示修改按钮的点击事件
        /// </summary>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.gvExamDictionaryDetail.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择一条记录!", "提示");
                    return;
                }
                m_funstate = EditState.Edit;
                BindBtnState();

                txtJcmc.Focus();
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
                #region 验证数据合法性
                if (txtJcmc.Text.Trim() == "")
                {
                    PublicMethod.RadAlterBoxRe("检查项目名称不能为空！", "提示", txtJcmc);
                    isLoad = false;
                    return;
                }
                if (ConvertMy.ToDecimal(txtJsfw.ContentText) < ConvertMy.ToDecimal(txtKsfw.ContentText))
                {
                    PublicMethod.RadAlterBoxRe("结束范围必须大于开始范围！", "提示", txtJsfw);
                    isLoad = false;
                    return;
                }
                if (txtJcmc.Text.Trim().Length >= 200)
                {
                    PublicMethod.RadAlterBoxRe("检查项目名称长度超出！", "提示", txtJcmc);
                    isLoad = false;
                    return;
                }

                #endregion

                YidanEHRDataServiceClient client = PublicMethod.YidanClient;

                if (m_funstate == EditState.Add)
                {
                    #region 实体赋值
                    PE_ExamDictionaryDetail examDictionaryDetail = new PE_ExamDictionaryDetail();
                    examDictionaryDetail.Jcbm = "";
                    examDictionaryDetail.Flbm = ConvertMy.ToString(cmbFlbm.SelectedValue);
                    examDictionaryDetail.Jcmc = txtJcmc.Text;
                    examDictionaryDetail.Mcsx = txtMcsx.Text;
                    examDictionaryDetail.Ksfw = (txtKsfw.ContentText == "") ? "0.00" : txtKsfw.ContentText;
                    examDictionaryDetail.Jsfw = (txtJsfw.ContentText == "") ? "0.00" : txtJsfw.ContentText;
                    examDictionaryDetail.Jsdw = txtJsdw.Text;
                    examDictionaryDetail.Yxjl = "1";
                    examDictionaryDetail.Bz = txtBz.Text;
                    #endregion
                    #region 验证是否重复添加
                    client.CheckAddExamDictionaryDetailCompleted += (objc, rea) =>
                    {
                        if (rea.Result > 0)
                        {
                            PublicMethod.RadAlterBox("该检查项已存在！", "提示");
                            return;
                        }
                        #region 添加的方法
                        client.InsertAndSelectExamDictionaryDetailCompleted += (obj, ea) =>
                        {
                            if (ea.Result.ToList() != null)
                            {

                                pe_ExamDictionaryDetail = ea.Result;
                                gvExamDictionaryDetail.ItemsSource = pe_ExamDictionaryDetail;
                                m_funstate = EditState.None;
                                BindBtnState();
                                ClearTextBox();
                                PublicMethod.RadAlterBox("添加成功！", "提示");
                            }
                            else
                            {
                                PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                            }
                        };
                        client.InsertAndSelectExamDictionaryDetailAsync(examDictionaryDetail);
                        #endregion
                    };
                    client.CheckAddExamDictionaryDetailAsync(ConvertMy.ToString(cmbFlbm.SelectedValue), txtJcmc.Text);
                    #endregion
                }
                if (m_funstate == EditState.Edit)
                {
                    #region 改变实体的值
                    pedd.Jcbm = txtJcbm.Text;
                    pedd.Flbm = ConvertMy.ToString(cmbFlbm.SelectedValue);
                    pedd.Flmc = ConvertMy.ToString(cmbFlbm.SelectionBoxItem);
                    pedd.Jcmc = txtJcmc.Text;
                    pedd.Mcsx = txtMcsx.Text;
                    pedd.Ksfw = (txtKsfw.ContentText == "") ? "0.00" : txtKsfw.ContentText;
                    pedd.Jsfw = (txtJsfw.ContentText == "") ? "0.00" : txtJsfw.ContentText;
                    pedd.Jsdw = txtJsdw.Text;
                    pedd.Bz = txtBz.Text;
                    #endregion
                    #region 验证是否重复
                    client.CheckUpdateExamDictionaryDetailCompleted += (objc, rea) =>
                        {
                            if (rea.Result > 0)
                            {
                                PublicMethod.RadAlterBox("该检查项已存在！", "提示");
                                return;
                            }
                            #region 修改的方法
                            client.UpdateExamDictionaryDetailCompleted += (obj, ea) =>
                            {
                                if (ea.Result == 1)
                                {

                                    m_funstate = EditState.None;
                                    BindBtnState();
                                    PublicMethod.RadAlterBox("修改成功！", "提示");
                                }
                                else
                                {
                                    PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                                }
                            };
                            client.UpdateExamDictionaryDetailAsync(pedd, txtJcbm.Text);
                            #endregion
                        };
                    client.CheckUpdateExamDictionaryDetailAsync(pedd.Jcbm, pedd.Flbm, pedd.Jcmc);
                    #endregion
                }
            }

            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 表示gvExamDictionaryDetail行选择改变的事件
        /// </summary>
        private void gvExamDictionaryDetail_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            if (m_funstate == EditState.Add)
            {
                return;
            }
            pedd = (PE_ExamDictionaryDetail)gvExamDictionaryDetail.SelectedItem;
            if (pedd == null)
            {
                return;
            }
            #region 文本框赋值
            txtJcbm.Text = pedd.Jcbm;
            cmbFlbm.SelectedValue = pedd.Flbm;
            txtJcmc.Text = pedd.Jcmc;
            txtMcsx.Text = pedd.Mcsx;
            txtKsfw.Value = Convert.ToDouble(pedd.Ksfw);
            txtJsfw.Value = Convert.ToDouble(pedd.Jsfw);
            txtJsdw.Text = pedd.Jsdw;
            txtBz.Text = pedd.Bz;
            #endregion
        }

        #region  输入框加回车事件
        private void RegisterKeyEvent()
        {
            txtJcbm.KeyUp += new KeyEventHandler(txtJcbm_KeyUp);
            cmbFlbm.KeyUp += new KeyEventHandler(cmbFlbm_KeyUp);
            txtJcmc.KeyUp += new KeyEventHandler(txtJcmc_KeyUp);

            txtMcsx.KeyUp += new KeyEventHandler(txtMcsx_KeyUp);
            txtKsfw.KeyUp += new KeyEventHandler(txtKsfw_KeyUp);
            txtJsfw.KeyUp += new KeyEventHandler(txtJsfw_KeyUp);

            txtJsdw.KeyUp += new KeyEventHandler(txtJsdw_KeyUp);
            txtBz.KeyUp += new KeyEventHandler(txtBz_KeyUp);
            btnSave.KeyUp += new KeyEventHandler(btnSave_KeyUp);
            tbQuery.KeyUp += new KeyEventHandler(tbQuery_KeyUp);
        }

        private void txtJcbm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cmbFlbm.Focus();
        }

        private void cmbFlbm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtJcmc.Focus();
        }

        private void txtJcmc_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtMcsx.Focus();
        }

        private void txtMcsx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtKsfw.Focus();
        }

        private void txtKsfw_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtJsfw.Focus();
        }

        private void txtJsfw_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtJsdw.Focus();
        }

        private void txtJsdw_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtBz.Focus();
        }

        private void txtBz_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSave.Focus();
        }

        private void btnSave_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSave_Click(null, null);
        }

        /// <summary>
        /// 查询框回车查询事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbQuery_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnQuery_Click(null, null);
        }
        #endregion

        #endregion

        #region 方法
        public ExamDictionaryDetailManager()
        {
            InitializeComponent();

            RegisterKeyEvent();
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
        /// 清空文本
        /// </summary>
        private void ClearTextBox()
        {
            txtJcbm.Text = "自动生成";
            txtJcmc.Text = txtMcsx.Text = txtMcsx.Text = txtJsdw.Text = txtBz.Text = String.Empty;
            txtKsfw.Value = txtJsfw.Value = 0;
        }


        private void btnTxtClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ClearTextBox();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        /// <summary>
        /// 设置启用状态
        /// </summary>
        /// <param name="bl1">状态1</param>
        /// <param name="bl2">状态2</param>
        private void SetEnabled(Boolean bl1, Boolean bl2)
        {
            txtJcmc.IsEnabled = txtMcsx.IsEnabled = txtJsfw.IsEnabled = txtKsfw.IsEnabled = txtJsdw.IsEnabled = txtBz.IsEnabled = bl1;
            cmbFlbm.IsEnabled = bl1;
            this.btnAdd.IsEnabled = bl2;
            this.btnDel.IsEnabled = bl2;
            this.btnUpdate.IsEnabled = bl2;

            this.btnClear.IsEnabled = bl1;

            this.btnSave.IsEnabled = btnTxtClear.IsEnabled = bl1;
        }

        /// <summary>
        /// 表示绑定检查项目信息的方法
        /// </summary>
        private void GetExamDictionaryDetail()
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            PE_ExamDictionaryDetail pedd = new PE_ExamDictionaryDetail();
            pedd.Bz = pedd.Flbm = pedd.Jcbm = pedd.Jcmc = pedd.Jsdw = pedd.Jsfw = pedd.Ksfw = pedd.Mcsx = pedd.Py = pedd.Wb = pedd.Yxjl = String.Empty;
            client.InsertAndSelectExamDictionaryDetailCompleted += (obj, ea) =>
            {
                if (ea.Error == null)
                {
                    pe_ExamDictionaryDetail = ea.Result;
                    gvExamDictionaryDetail.ItemsSource = pe_ExamDictionaryDetail;
                }
            };
            client.InsertAndSelectExamDictionaryDetailAsync(pedd);
        }

        /// <summary>
        /// 表示绑定分类编码的方法
        /// </summary>
        private void BindFlbm()
        {
            try
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.GetExamDictionaryCompleted += (obj, ea) =>
                    {
                        if (ea.Error == null)
                        {
                            if (ea.Result.ToList().Count == 0)
                            {
                                return;
                            }
                            cmbFlbm.ItemsSource = ea.Result.ToList();
                            cmbFlbm.DisplayMemberPath = "Jcmc";
                            cmbFlbm.SelectedValuePath = "Jcbm";
                            cmbFlbm.SelectedIndex = 0;
                        }
                    };
                client.GetExamDictionaryAsync();
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #endregion

        #region 变量
        EditState m_funstate;//按钮状态
        ObservableCollection<PE_ExamDictionaryDetail> pe_ExamDictionaryDetail;//检查项信息集合
        PE_ExamDictionaryDetail pedd;

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            GetExamDictionaryDetail(tbQuery.Text.Replace(" ", ""));
        }


        private void GetExamDictionaryDetail(string key)
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.SelectExamDictionaryDetailCompleted += (obj, ea) =>
            {
                if (ea.Error == null)
                {
                    pe_ExamDictionaryDetail = ea.Result;
                    gvExamDictionaryDetail.ItemsSource = pe_ExamDictionaryDetail;
                }
            };
            client.SelectExamDictionaryDetailAsync(key);
            client.CloseAsync();
        }


        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.tbQuery.Text = string.Empty;
        }

        private void btnCodeservice_Click(object sender, RoutedEventArgs e)
        {
            YidanEHRApplication.Views.ChildWindows.ExamCodemanager ecm = new ChildWindows.ExamCodemanager();
            ecm.Closed += new EventHandler<WindowClosedEventArgs>(ecm_Closed);
            ecm.ResizeMode = ResizeMode.NoResize;
            ecm.ShowDialog();
        }//GrideView选中行集合

        private void ecm_Closed(object sender, EventArgs e)
        {
            BindFlbm();
        }
        #endregion
    }
}
