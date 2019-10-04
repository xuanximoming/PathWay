using System;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Views.ChildWindows
{
    /// <summary>
    /// 表示护理执行类别维护的类
    /// </summary>
    public partial class ExamCodemanager
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
                if (m_code_fun == null || m_code_fun.Jcbm == null)
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
                    string Jcbm = this.txtJcbm.Text;
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.DelCodeListCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                BindGridView();
                                m_funstate = EditState.View;
                                PublicMethod.RadAlterBox("记录删除成功", "提示");
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.DelCodeListAsync(Jcbm);
                    serviceCon.CloseAsync();

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
                string Jcbm = this.txtJcbm.Text;
                serviceCon = PublicMethod.YidanClient;
                serviceCon.DelCodeListCompleted +=
                    (obj, ea) =>
                    {
                        if (ea.Error == null)
                        {
                            BindGridView();
                            m_funstate = EditState.View;
                            PublicMethod.RadAlterBox("记录删除成功", "提示");
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(ea.Error);
                        }
                    };
                serviceCon.DelCodeListAsync(Jcbm);
                serviceCon.CloseAsync();
            }
        }
        /// <summary>
        /// 修改按钮事件
        /// </summary>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_code_fun == null || m_code_fun.Jcbm == null)
                {
                    PublicMethod.RadAlterBox("请选中要修改的行！", "提示");
                }
                else
                {
                    m_funstate = EditState.Edit;

                    BindPE_Fun(m_code_fun);

                    BindBtnState();
                    txtFjd.Focus();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
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
                BindPE_Fun(null);
                GridView.SelectedItem = null;
                BindBtnState();
                txtJcbm.Focus();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
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
                GridView.SelectedItem = null;
                //BindPE_Fun(null);

                BindBtnState();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 表示保存按钮点击事件
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.txtJcbm.Text == "")
                {
                    PublicMethod.RadAlterBoxRe("请输入检查项目编码！", "提示", txtJcbm);
                    isLoad = false;
                    return;
                }
                if (this.txtJcbm.Text.Length >= 50)
                {
                    PublicMethod.RadAlterBoxRe("检查项目编码长度超出！", "提示", txtJcbm);
                    isLoad = false;
                    return;
                }
                if (this.txtJcmc.Text == "")
                {
                    PublicMethod.RadAlterBoxRe("请输入检查项目编码名称！", "提示", txtJcmc);
                    isLoad = false;
                    return;
                }
                if (this.txtJcmc.Text.Length >= 200)
                {
                    PublicMethod.RadAlterBoxRe("检查项目编码名称长度超出！", "提示", txtJcmc);
                    isLoad = false;
                    return;
                }

                //if (this.txtFjd.Text == "")
                //{
                //    PublicMethod.RadAlterBoxRe("请输入父节点编码！", "提示");
                //    return;
                //}
                if (this.txtFjd.Text.Length > 50)
                {
                    PublicMethod.RadAlterBoxRe("父节点编码长度超出！", "提示", txtFjd);
                    isLoad = false;
                    return;
                }
                //if (this.txtMcsx.Text == "")
                //{
                //    PublicMethod.RadAlterBoxRe("请输入名称缩写符号！", "提示");
                //    return;
                //}
                if (this.txtMcsx.Text.Length > 50)
                {
                    PublicMethod.RadAlterBoxRe("名称缩写符号长度超出！", "提示", txtMcsx);
                    isLoad = false;
                    return;
                }

                string Jcbm = this.txtJcbm.Text;
                string Jcmc = this.txtJcmc.Text;
                string Fjd = this.txtFjd.Text;
                string Mcsx = this.txtMcsx.Text;
                string Py = this.txtPy.Text;
                string Wb = this.txtWb.Text;
                string Bz = this.txtBz.Text;
                serviceCon = PublicMethod.YidanClient;

                if (m_funstate == EditState.Add)
                {
                    //验证记录(编码)是否已存在
                    serviceCon.CheckAddCodeBeingCompleted += (objc, rea) =>
                        {
                            if (rea.Result > 0)
                            {
                                //编码存在 
                                PublicMethod.RadAlterBoxRe("该检查项目编码已存在！", "提示", txtJcbm);
                                isLoad = false;
                                return;
                            }
                            //编码不存在可插入数据
                            serviceCon.InsertCodeListCompleted += (obj, ea) =>
                            {
                                if (!ea.Result)
                                {
                                    PublicMethod.RadAlterBox("提交数据失败！", "提示");
                                    return;
                                }
                                //插入成功后刷新数据
                                BindGridView();
                                m_funstate = EditState.View;
                                BindBtnState(); PublicMethod.RadAlterBox("提交成功！", "提示");
                            };
                            serviceCon.InsertCodeListAsync(Jcbm, Jcmc, Fjd, Mcsx, Wb, Bz);
                        };
                    serviceCon.CheckAddCodeBeingAsync(Jcbm);
                }
                if (m_funstate == EditState.Edit)
                {
                    serviceCon.CheckAddCodeBeingCompleted += (objc, rea) =>
                    {
                        if (rea.Error != null)
                        {
                            PublicMethod.RadAlterBox(rea.Error.ToString(), "提示");
                            return;
                        }
                        if (rea.Result > 0)
                        {
                            serviceCon.UpdateCodeListCompleted += (obj, ea) =>
                            {
                                if (ea.Result)
                                {
                                    BindGridView();
                                    m_funstate = EditState.View;
                                    BindBtnState();

                                    PublicMethod.RadAlterBox("修改成功！", "提示");
                                }
                                else
                                {
                                    PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                                }
                            };
                            serviceCon.UpdateCodeListAsync(Jcbm, Jcmc, Fjd, Mcsx, Bz);
                        }
                    };
                    serviceCon.CheckAddCodeBeingAsync(Jcbm);
                    //serviceCon.CloseAsync();
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
            try
            {
                if (!isLoad)
                {
                    isLoad = true;
                    return;
                }
                m_funstate = EditState.View;
                //BindCombox();
                BindGridView();
                BindBtnState();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnTxtClear_Click(object sender, RoutedEventArgs e)
        {
            ClearTextBox();
        }
        /// <summary>
        /// 行选中改变事件
        /// </summary>
        private void GridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                if (GridView.SelectedItem == null)
                {
                    m_code_fun = null;
                    return;
                }
                //if (m_funstate == EditState.View)
                else
                {
                    m_code_fun = (PE_CodeList)GridView.SelectedItem;
                    BindPE_Fun(m_code_fun);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        //#region  输入框加回车事件
        //private void RegisterKeyEvent()
        //{
        //    txtOrderValue.KeyUp += new KeyEventHandler(txtOrderValue_KeyUp);
        //    txtXmxh.KeyUp += new KeyEventHandler(txtXmxh_KeyUp);
        //    txtName.KeyUp += new KeyEventHandler(txtName_KeyUp);

        //    cmbYxjl.KeyUp += new KeyEventHandler(cmbYxjl_KeyUp);
        //    btnSave.KeyUp += new KeyEventHandler(btnSave_KeyUp);
        //}

        //private void txtOrderValue_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //        txtXmxh.Focus();
        //}

        //private void txtXmxh_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //        txtName.Focus();
        //}

        //private void txtName_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //        cmbYxjl.Focus();
        //}

        //private void cmbYxjl_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //        btnSave.Focus();
        //}

        //private void btnSave_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //        btnSave_Click(null, null);
        //}

        //#endregion

        #endregion

        #region 方法
        public ExamCodemanager()
        {
            InitializeComponent();
            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };
            RegisterKeyEvent(); //加回车
        }

        private void RegisterKeyEvent()
        {
            txtJcbm.KeyUp += new KeyEventHandler(txtJcbm_KeyUp);
            txtFjd.KeyUp += new KeyEventHandler(txtFjd_KeyUp);
            txtJcmc.KeyUp += new KeyEventHandler(txtJcmc_KeyUp);

            txtMcsx.KeyUp += new KeyEventHandler(txtMcsx_KeyUp);
            txtBz.KeyUp += new KeyEventHandler(txtBz_KeyUp);
            btnSave.KeyUp += new KeyEventHandler(btnSave_KeyUp);
        }

        private void txtJcbm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtFjd.Focus();
        }

        private void txtFjd_KeyUp(object sender, KeyEventArgs e)
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
        private void BindGridView()
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetPE_CodeListCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            GridView.ItemsSource = e.Result;

                            BindPE_Fun(null);

                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };

            serviceCon.GetPE_CodeListAsync();
            serviceCon.CloseAsync();
        }

        /// <summary>
        /// 绑定页面中编辑信息
        /// </summary>
        /// <param name="pe_fun">实体，如果为空编辑区域值为空</param>
        private void BindPE_Fun(PE_CodeList userrole_fun)
        {
            if (userrole_fun != null)
            {
                this.txtBz.Text = userrole_fun.Bz;
                this.txtFjd.Text = userrole_fun.Fjd;
                this.txtJcbm.Text = userrole_fun.Jcbm;
                this.txtJcmc.Text = userrole_fun.Jcmc;
                this.txtPy.Text = userrole_fun.Py;
                this.txtWb.Text = userrole_fun.Wb;
                this.txtMcsx.Text = userrole_fun.Mcsx;

                //BindListBox(userrole_fun.Zgdm);


            }
            else
            {
                m_code_fun = new PE_CodeList();
                this.txtBz.Text = "";
                this.txtFjd.Text = "";
                this.txtJcbm.Text = "";
                this.txtJcmc.Text = "";
                this.txtPy.Text = "";
                this.txtWb.Text = "";
                this.txtMcsx.Text = "";
            }

        }


        /// <summary>
        /// 清空文本
        /// </summary>
        private void ClearTextBox()
        {
            try
            {
                BindPE_Fun(null);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 表示控制按钮状态的方法
        /// </summary>
        private void BindBtnState()
        {
            if (m_funstate == EditState.Add)
            {
                //this.cbxRole.IsEnabled = true;
                //this.cbxUser.IsEnabled = true;


                this.btnAdd.IsEnabled = false;
                this.btnDel.IsEnabled = false;
                this.btnUpdate.IsEnabled = false;

                this.btnClear.IsEnabled = true;

                this.btnSave.IsEnabled = true;
                this.btnTxtClear.IsEnabled = true;

                this.txtBz.IsEnabled = true;
                this.txtFjd.IsEnabled = true;
                this.txtJcbm.IsEnabled = true;
                this.txtJcmc.IsEnabled = true;
                this.txtPy.IsEnabled = false;
                this.txtWb.IsEnabled = false;
                this.txtMcsx.IsEnabled = true;
            }
            else if (m_funstate == EditState.Edit)
            {
                this.btnAdd.IsEnabled = false;
                this.btnDel.IsEnabled = false;
                this.btnUpdate.IsEnabled = false;

                this.btnClear.IsEnabled = true;

                this.btnSave.IsEnabled = true;
                this.btnTxtClear.IsEnabled = false;

                this.txtBz.IsEnabled = true;
                this.txtFjd.IsEnabled = true;
                this.txtJcbm.IsEnabled = false;
                this.txtJcmc.IsEnabled = true;
                this.txtPy.IsEnabled = false;
                this.txtWb.IsEnabled = false;
                this.txtMcsx.IsEnabled = true;
            }
            else
            {
                //this.cbxRole.IsEnabled = false;
                //this.cbxUser.IsEnabled = false;


                this.btnAdd.IsEnabled = true;
                this.btnDel.IsEnabled = true;
                this.btnUpdate.IsEnabled = true;

                this.btnClear.IsEnabled = false;

                this.btnSave.IsEnabled = false;
                this.btnTxtClear.IsEnabled = false;

                this.txtBz.IsEnabled = false;
                this.txtFjd.IsEnabled = false;
                this.txtJcbm.IsEnabled = false;
                this.txtJcmc.IsEnabled = false;
                this.txtPy.IsEnabled = false;
                this.txtWb.IsEnabled = false;
                this.txtMcsx.IsEnabled = false;
            }
        }
        #endregion

        #region 变量

        /// <summary>
        /// 按钮状态
        /// </summary>
        EditState m_funstate;


        YidanEHRDataServiceClient serviceCon;

        /// <summary>
        /// 用于存储列表中选中的行记录
        /// </summary>
        static PE_CodeList m_code_fun = new PE_CodeList();

        #endregion


    }
}

