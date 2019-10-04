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
using YidanEHRApplication.Models;
using YidanEHRApplication.Helpers;
using Telerik.Windows.Controls;

namespace YidanEHRApplication.Views.Permission
{

    public partial class FunManager : Page
    {
        YidanEHRDataServiceClient serviceCon;

        static PE_Fun m_pe_fun = new PE_Fun();

        EditState m_funstate;

        public FunManager()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(FunManager_Loaded);
        }
        public bool isLoad = true;
        void FunManager_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isLoad)
                {
                    isLoad = true;
                    return;
                }
                m_funstate = EditState.View;
                BindCombox();
                BindGridView("", "");
                BindBtnState();
                RegisterKeyEvent();
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

        private void BindCombox()
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetPE_FunFatherListCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        cbxFatherCode.ItemsSource = e.Result.ToList();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };

            serviceCon.GetPE_FunFatherListAsync();
            serviceCon.CloseAsync();
        }



        #region 函数
        //YidanEHRServiceReference.yida serviceCon;

        private void BindGridView(string key1, string key2)
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetPE_FunListCompleted +=
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
            serviceCon.GetPE_FunListAsync(key1, key2);
            serviceCon.CloseAsync();
        }




        /// <summary>
        /// 绑定页面中编辑信息
        /// </summary>
        /// <param name="pe_fun">实体，如果为空编辑区域值为空</param>
        private void BindPE_Fun(PE_Fun pe_fun)
        {
            if (pe_fun != null)
            {
                this.txtFunCode.Text = pe_fun.FunCode;
                this.cbxFatherCode.SelectedValue = pe_fun.FunCodeFather;
                this.txtFunName.Text = pe_fun.FunName;
                this.txtFunURL.Text = pe_fun.FunURL;

            }
            else
            {
                m_pe_fun = new PE_Fun();
                this.txtFunCode.Text = "";
                this.cbxFatherCode.SelectedItem = null;
                this.txtFunName.Text = "";
                this.txtFunURL.Text = "";
            }
        }


        private void BindBtnState()
        {
            if (m_funstate == EditState.Add)
            {
                this.txtFunCode.IsEnabled = true;
                this.cbxFatherCode.IsEnabled = true;
                this.txtFunName.IsEnabled = true;
                if (cbxFatherCode.SelectedItem == null)
                {
                    this.txtFunURL.IsEnabled = false;
                }
                else
                {
                    this.txtFunURL.IsEnabled = true;
                }

                this.btnAdd.IsEnabled = false;
                this.btnDel.IsEnabled = false;
                this.btnUpdate.IsEnabled = false;

                this.btnClear.IsEnabled = true;

                this.btnSave.IsEnabled = true;
                this.btnTxtClear.IsEnabled = true;
            }
            else if (m_funstate == EditState.Edit)
            {
                this.txtFunCode.IsEnabled = false;
                this.cbxFatherCode.IsEnabled = true;
                this.txtFunName.IsEnabled = true;
                if (cbxFatherCode.SelectedItem == null)
                {
                    this.txtFunURL.IsEnabled = false;
                    this.cbxFatherCode.IsEnabled = false;
                }
                else
                {
                    this.txtFunURL.IsEnabled = true;
                    this.cbxFatherCode.IsEnabled = true;
                }


                this.btnAdd.IsEnabled = false;
                this.btnDel.IsEnabled = false;
                this.btnUpdate.IsEnabled = false;

                this.btnClear.IsEnabled = true;

                this.btnSave.IsEnabled = true;
                this.btnTxtClear.IsEnabled = false;
            }
            else
            {
                this.txtFunCode.IsEnabled = false;
                this.cbxFatherCode.IsEnabled = false;
                this.txtFunName.IsEnabled = false;
                this.txtFunURL.IsEnabled = false;


                this.btnAdd.IsEnabled = true;
                this.btnDel.IsEnabled = true;
                this.btnUpdate.IsEnabled = true;

                this.btnClear.IsEnabled = false;

                this.btnSave.IsEnabled = false;
                this.btnTxtClear.IsEnabled = false;
            }
        }

        #endregion

        #region
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_pe_fun == null || m_pe_fun.FunCode == null)
                {
                    PublicMethod.RadAlterBox("请选中要删除的行！", "提示");
                }
                else
                {
                    string mess = "";
                    if (m_pe_fun.FunCodeFather.Length == 0)
                    {
                        mess = "请问是否删除选中的父节点信息？";
                    }
                    else
                    {
                        mess = "请问是否删除选中的子节点信息？";
                    }
                    //DialogParameters parameters = new DialogParameters();
                    //parameters.Content = String.Format("提示: {0}", mess);
                    //parameters.Header = "提示";
                    //parameters.IconContent = null;
                    //parameters.OkButtonContent = "确定";
                    //parameters.CancelButtonContent = "取消";
                    //parameters.Closed = OnDelAdviceGroupDetail;//***close处理***
                    //RadWindow.Confirm(parameters);


                    YidanPathWayMessageBox mess1 = new YidanPathWayMessageBox(mess, "提示", YiDanMessageBoxButtons.YesNo);
                    mess1.ShowDialog();
                    mess1.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);

                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void mess_PageClosedEvent(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    string FunCode = m_pe_fun.FunCode;
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.DelPE_FunCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                SQLMessage mess = ea.Result;
                                if (mess.IsSucceed)
                                {
                                    FunManager_Loaded(null, null);
                                }
                                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");

                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.DelPE_FunAsync(FunCode);
                    serviceCon.CloseAsync();

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void OnDelAdviceGroupDetail(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                try
                {
                    string FunCode = m_pe_fun.FunCode;
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.DelPE_FunCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                SQLMessage mess = ea.Result;
                                if (mess.IsSucceed)
                                {
                                    FunManager_Loaded(null, null);
                                }
                                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");

                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.DelPE_FunAsync(FunCode);
                    serviceCon.CloseAsync();
                }
                catch (Exception ex)
                {
                    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            }
        }

        /// <summary>
        /// 将需要修改的功能节点信息绑定到编辑区域，将编辑区域装填修改为可编辑
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_pe_fun == null || m_pe_fun.FunCode == null)
                {
                    PublicMethod.RadAlterBox("请选中要修改的行！", "提示");
                }
                else
                {
                    m_funstate = EditState.Edit;

                    BindPE_Fun(m_pe_fun);

                    BindBtnState();

                    txtFunName.Focus();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 将编辑区域值为可以编辑状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.Add;
                BindPE_Fun(null);
                GridView.SelectedItem = null;
                BindBtnState();
                txtFunCode.Focus();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        /// <summary>
        /// 记录下选中的行
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                if (GridView.SelectedItem == null)
                {
                    m_pe_fun = null;
                    return;
                }
                //在View状态时将信息填到编辑区域
                if (m_funstate == EditState.View)
                {
                    m_pe_fun = (PE_Fun)GridView.SelectedItem;
                    BindPE_Fun(m_pe_fun);
                    BindBtnState();
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
                string FunCodeFather = "";
                if (this.cbxFatherCode.SelectedItem != null)
                {
                    PE_Fun pe_fun = (PE_Fun)cbxFatherCode.SelectedItem;
                    FunCodeFather = pe_fun.FunCode;
                }

                string FunCode = this.txtFunCode.Text.ToString().Trim();

                string FunName = this.txtFunName.Text.ToString().Trim();

                string FunURL = this.txtFunURL.Text.ToString().Trim();
                if (!txtFunURL.IsEnabled)
                {
                    FunURL = "";
                }

                if (FunCode.Length == 0)
                {
                    PublicMethod.RadAlterBox("请填写功能编码！", "提示");
                    return;
                }


                if (FunCode.Length >= 11)
                {
                    PublicMethod.RadAlterBox("功能编码长度超出！", "提示");
                    return;
                }
                try
                {
                    int.Parse(FunCode);
                }
                catch
                {
                    PublicMethod.RadAlterBox("功能编码必须是整数！", "提示");
                    return;
                }
                if (txtFunURL.IsEnabled == true)
                {
                    if (FunURL.Length == 0)
                    {
                        PublicMethod.RadAlterBox("功能Url不能为空！", "提示");
                        return;
                    }
                }
                if (FunName.Length == 0)
                {
                    PublicMethod.RadAlterBox("请填写功能名称！", "提示");
                    return;
                }

                if (m_funstate == EditState.Add)
                {
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.InsertPE_FunCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                SQLMessage mess = ea.Result;
                                if (mess.IsSucceed)
                                {
                                    FunManager_Loaded(null, null);
                                }
                                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.InsertPE_FunAsync(FunCode, FunCodeFather, FunName, FunURL);
                    serviceCon.CloseAsync();
                }
                else
                {
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.UpdatePE_FunCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    SQLMessage mess = ea.Result;
                                    if (mess.IsSucceed)
                                    {
                                        FunManager_Loaded(null, null);

                                    }
                                    PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                                }
                                else
                                {
                                    PublicMethod.RadWaringBox(ea.Error);
                                }
                            };
                    serviceCon.UpdatePE_FunAsync(FunCode, FunCodeFather, FunName, FunURL);
                    serviceCon.CloseAsync();
                }
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
                GridView.SelectedItem = null;
                BindPE_Fun(null);

                BindBtnState();
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

        private void cbxFatherCode_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (cbxFatherCode.SelectedItem == null)
                {
                    //this.txtFunURL.Text = "";
                    this.txtFunURL.IsEnabled = false;
                }
                else
                {
                    this.txtFunURL.IsEnabled = true;
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            BindGridView(tbQuery1.Text.Replace(" ", ""), tbQuery2.Text.Replace(" ", ""));
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            this.tbQuery1.Text = string.Empty;
            this.tbQuery2.Text = string.Empty;
        }

        #region  输入框加回车事件
        private void RegisterKeyEvent()
        {
            txtFunCode.KeyUp += new KeyEventHandler(txtFunCode_KeyUp);
            cbxFatherCode.KeyUp += new KeyEventHandler(cbxFatherCode_KeyUp);
            txtFunName.KeyUp += new KeyEventHandler(txtFunName_KeyUp);

            txtFunURL.KeyUp += new KeyEventHandler(txtFunURL_KeyUp);
            btnSave.KeyUp += new KeyEventHandler(btnSave_KeyUp);

            tbQuery1.KeyUp += new KeyEventHandler(tbQuery1_KeyUp);
            tbQuery2.KeyUp += new KeyEventHandler(tbQuery2_KeyUp);
        }

        private void txtFunCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbxFatherCode.Focus();
        }

        private void cbxFatherCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtFunName.Focus();
        }

        private void txtFunName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtFunURL.Focus();
        }

        private void txtFunURL_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSave.Focus();
        }

        private void btnSave_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSave_Click(null, null);
        }

        private void tbQuery1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnQuery_Click(null, null);
        }

        private void tbQuery2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnQuery_Click(null, null);
        }

        #endregion

        #endregion

    }
}
