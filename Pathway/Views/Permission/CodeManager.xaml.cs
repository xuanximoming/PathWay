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
    public partial class CodeManager : Page
    {
        YidanEHRDataServiceClient serviceCon;

        /// <summary>
        /// 用于存储列表中选中的行记录
        /// </summary>
        static PE_CodeList m_code_fun = new PE_CodeList();

        EditState m_funstate;
        public CodeManager()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(CodeManager_Loaded);
        }

        void CodeManager_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
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

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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
                            //将第一条信息绑定到编辑区域
                            //if (e.Result.ToList().Count > 0)
                            //{
                            //    m_pe_fun = e.Result.ToList()[0];

                            BindPE_Fun(null);
                            //}
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
        /// 按钮状态
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
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.Add;
                BindPE_Fun(null);
                BindBtnState();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.View;

                BindPE_Fun(null);

                BindBtnState();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_code_fun == null || m_code_fun.Jcbm == null)
                {
                    PublicMethod.RadAlterBox("请选中要删除的行！", "提示");
                }
                else
                {
                    string Jcbm = this.txtJcbm.Text;
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.DelCodeListCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                BindGridView();
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
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.txtJcbm.Text == "")
                {
                    PublicMethod.RadAlterBox("请输入检查项目编码！", "提示");
                    return;
                }
                else if (this.txtJcmc.Text == "")
                {
                    PublicMethod.RadAlterBox("请输入检查项目编码名称！", "提示");
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
                                PublicMethod.RadAlterBox("该检查项目编码已存在！", "提示");
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
                                BindBtnState();
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
                                    PublicMethod.RadAlterBox("修改成功！", "提示");

                                    BindGridView();
                                    m_funstate = EditState.View;
                                    BindBtnState();
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

        private void btnTxtClear_Click(object sender, RoutedEventArgs e)
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

        private void GridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                if (GridView.SelectedItem == null)
                {
                    m_code_fun = null;
                    return;
                }
                if (m_funstate == EditState.View)
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

        private void GridViewRole_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            //if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
            //PE_Role t = (PE_Role)e.DataElement;
            //List<CheckBox> listtest = (List<CheckBox>)(this.GridViewRole.ChildrenOfType<CheckBox>().ToList());
            //if (listtest.Count > 0)
            //    if (t.IsCheck == 1)
            //        listtest[listtest.Count - 1].IsChecked = true;
        }

    }
}
