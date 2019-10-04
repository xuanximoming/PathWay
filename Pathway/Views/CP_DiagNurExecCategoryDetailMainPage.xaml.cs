using DrectSoft.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using YidanEHRApplication.Views.ChildWindows;
namespace YidanEHRApplication.Views
{
    public partial class CP_DiagNurExecCategoryDetailMainPage : Page
    {
        public CP_DiagNurExecCategoryDetailMainPage()
        {
            InitializeComponent();

            RegisterKeyEvent();
        }


        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        #region 变量
        private CP_DiagNurExecCategoryDetail m_dnedetail = new CP_DiagNurExecCategoryDetail();
        /// <summary>
        /// 列表数据源
        /// </summary>
        private ObservableCollection<CP_DiagNurExecCategoryDetail> m_listsouce;


        YidanEHRDataServiceClient serviceCon;
        OperationState _CurrentState = OperationState.VIEW;
        OperationState CurrentState
        {
            get { return _CurrentState; }
            set
            {
                _CurrentState = value;

                if (value == OperationState.NEW)
                {
                    this.btnHlflWh.IsEnabled = true;
                    cbxInsert.IsEnabled = true;
                    txtInsert.IsEnabled = true;
                    txtPy.IsEnabled = true;

                    btnAdd.IsEnabled = false;
                    btnUpdate.IsEnabled = false;
                    this.btnEnableCode.IsEnabled = false;
                    btnDisableCode.IsEnabled = false;
                    this.btnClear.IsEnabled = true;
                    this.btnCreateCode.IsEnabled = true;


                }
                else if (value == OperationState.EDIT)
                {
                    this.btnHlflWh.IsEnabled = false;
                    cbxInsert.IsEnabled = true;
                    txtInsert.IsEnabled = true;
                    txtPy.IsEnabled = true;

                    btnAdd.IsEnabled = false;
                    btnUpdate.IsEnabled = false;
                    this.btnEnableCode.IsEnabled = false;
                    btnDisableCode.IsEnabled = false;
                    this.btnClear.IsEnabled = true;
                    this.btnCreateCode.IsEnabled = true;

                }
                else
                {

                    this.btnHlflWh.IsEnabled = true;
                    cbxInsert.IsEnabled = false;
                    txtInsert.IsEnabled = false;
                    txtPy.IsEnabled = false;
                    this.btnAdd.IsEnabled = true;
                    btnAdd.IsEnabled = true;
                    btnUpdate.IsEnabled = true;
                    this.btnEnableCode.IsEnabled = true;
                    btnDisableCode.IsEnabled = true;
                    this.btnClear.IsEnabled = false;
                    this.btnCreateCode.IsEnabled = false;
                }


            }
        }

        #endregion
        #region 事件

        public bool isLoad = true;
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoad)
            {
                isLoad = true;
                return;
            }
            CurrentState = OperationState.VIEW;
            BindGridView();
            BindCbx();

        }
        /// <summary>
        /// 行选中改变事件
        /// </summary>
        private void GridViewNur_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {

                if (GridViewNur.SelectedItem == null)
                {
                    return;
                }
                if (CurrentState == OperationState.VIEW)
                {
                    m_dnedetail = (CP_DiagNurExecCategoryDetail)GridViewNur.SelectedItem;
                    Bind_Data(m_dnedetail);
                }

            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 绑定页面中编辑信息
        /// </summary>
        /// <param name="pe_fun">His接口配置实体，如果为空编辑区域值为空</param>
        private void Bind_Data(CP_DiagNurExecCategoryDetail _cpDigNur)
        {
            if (_cpDigNur != null)
            {

                this.cbxInsert.SelectedItem = ((List<CP_DiagNurExecCategory>)cbxInsert.ItemsSource).First(where => where.Lbxh.Equals(_cpDigNur.Lbxh));
                this.txtInsert.Text = _cpDigNur.Name;
                this.txtPy.Text = _cpDigNur.Py;

            }
            else
            {
                m_dnedetail = new CP_DiagNurExecCategoryDetail();
                this.cbxInsert.SelectedItem = null;
                this.txtInsert.Text = "";
                this.txtPy.Text = "";
            }
        }
        /// <summary>
        /// 修改按钮事件
        /// </summary>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.GridViewNur.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选中要修改的行！", "提示");
                    return;
                }

                m_dnedetail = (CP_DiagNurExecCategoryDetail)GridViewNur.SelectedItem;
                CurrentState = OperationState.EDIT;
                txtInsert.Focus();

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
                CurrentState = OperationState.NEW;
                NewAdviceGroupDetail();
                txtInsert.Focus();

            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {

            //String operation = "select";

            int Lbxh = (cbxQuery.SelectedItem != null) ? (cbxQuery.SelectedItem as CP_DiagNurExecCategory).Lbxh : -1;
            int Yxjl = (cbxYxjlQuery.SelectedItem != null) ? (cbxYxjlQuery.SelectedIndex) : -1;
            int Sfsy = (cbxSfsyQuery.SelectedItem != null) ? (cbxSfsyQuery.SelectedIndex) : -1;

            //集合类型初始化
            List<CP_DiagNurExecCategoryDetail> t_listsouce = m_listsouce.ToList();


            if (Lbxh >= 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.Lbxh == Lbxh).ToList();

            }

            if (Yxjl >= 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.Yxjl == Yxjl).ToList();

            }


            if (Sfsy >= 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.Sfsy == Sfsy).ToList();

            }


            GridViewNur.ItemsSource = t_listsouce.ToList();


        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            //String operation = "insert";
            #region 验证数据并初始化
            if (cbxInsert.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择分类类型", "提示", cbxInsert);
                isLoad = false;
                return;
            }
            if (txtInsert.Text == null || txtInsert.Text == "")
            {
                PublicMethod.RadAlterBoxRe("请输入项目名称", "提示", txtInsert);
                isLoad = false;
                return;
            }
            if (txtInsert.Text.Length > 100)
            {
                PublicMethod.RadAlterBoxRe("项目名称长度不能超出50个汉字", "提示", txtInsert);
                isLoad = false;
                return;
            }

            //foreach (CP_DiagNurExecCategoryDetail item in GridViewNur.Items)
            //{
            //    if (txtInsert.Text.Trim() == item.MxName)
            //    {
            //        PublicMethod.RadAlterBoxRe("该项目名称已经存在", "提示", txtInsert);
            //        isLoad = false;
            //        return;
            //    }
            //}

            //拼音码可自动生成，不需要手动输入和验证
            //修改时间：2013年8月12日 15:31:01
            //修改人：Jhonny
            //if (txtPy.Text == null || txtPy.Text == "")
            //{
            //    PublicMethod.RadAlterBoxRe("请输入项目拼音码简写", "提示", txtInsert);
            //    isLoad = false;
            //    return;
            //}

            CP_DiagNurExecCategoryDetail _dnedetail = new CP_DiagNurExecCategoryDetail();

            _dnedetail.Name = txtInsert.Text.Trim();
            _dnedetail.Lbxh = (cbxInsert.SelectedItem != null) ? (cbxInsert.SelectedItem as CP_DiagNurExecCategory).Lbxh : 1;
            _dnedetail.Yxjl = 1;
            _dnedetail.Sfsy = 0;
            _dnedetail.Create_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            _dnedetail.Create_User = Global.LogInEmployee.Zgdm;
            _dnedetail.Cancel_Time = "";
            _dnedetail.Cancel_User = "";
            _dnedetail.OrderValue = 0;
            _dnedetail.JkType = 0;
            _dnedetail.Tbzd = "";
            _dnedetail.Zdly = "";
            _dnedetail.Py = txtPy.Text.Trim();
            _dnedetail.Wb = "";
            _dnedetail.Jkdm = "";
            _dnedetail.Scdm = "";
            _dnedetail.Memo = "";
            _dnedetail.Extension = "";
            _dnedetail.Extension1 = "";
            _dnedetail.Extension2 = "";


            #endregion

            if (CurrentState == OperationState.NEW)
            {


                serviceCon = PublicMethod.YidanClient;
                serviceCon.InsertDiagNurCategoryDetailsCompleted +=
                    (obj, ea) =>
                    {
                        if (ea.Result == 1)
                        {
                            BindGridView();
                            PublicMethod.RadAlterBox("保存成功！", "提示");
                            //清空控件
                            NewAdviceGroupDetail();
                        }
                        else if (ea.Result == 2)
                        {
                            PublicMethod.RadAlterBox("该项目名称已经存在", "提示");
                            return;
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(ea.Error);
                        }
                    };
                serviceCon.InsertDiagNurCategoryDetailsAsync(_dnedetail);
                serviceCon.CloseAsync();

                CurrentState = OperationState.VIEW;
            }
            if (CurrentState == OperationState.EDIT)
            {

                _dnedetail.Mxxh = m_dnedetail.Mxxh;

                YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                Client.UpdateDiagNurExecCategoryDetailCompleted += (sb, ea) =>
                {
                    if (ea.Result >= 1)
                    {
                        PublicMethod.RadAlterBox("项目名称重复，请检查后重新输入！", "提示");
                        //弹框之后还原数据信息
                        //this.cbxInsert.SelectedItem = null;
                        this.cbxInsert.IsEnabled = true;
                        this.txtInsert.Text = "";
                        this.txtPy.IsEnabled = true;
                    }
                    else
                    {
                        if (ea.Error == null)
                        {
                            BindGridView();
                            PublicMethod.RadAlterBox("更新成功！", "提示");
                            NewAdviceGroupDetail();
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(ea.Error);
                        }
                    }
                };
                Client.UpdateDiagNurExecCategoryDetailAsync(_dnedetail);


                CurrentState = OperationState.VIEW;
            }


        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (GridViewNur.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择一项", "提示");
                    return;
                }

                //YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                //mess.ShowDialog();
                //mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
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
                    int Mxxh = (GridViewNur.SelectedItem as CP_DiagNurExecCategoryDetail).Mxxh;

                    BindGridView();
                    PublicMethod.RadAlterBox("删除成功", "提示");

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void OnDelete(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                //String operation = "delete";
                int Mxxh = (GridViewNur.SelectedItem as CP_DiagNurExecCategoryDetail).Mxxh;

                BindGridView();
                PublicMethod.RadAlterBox("删除成功", "提示");
            }
        }
        /// <summary>
        ///启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnable_Click(object sender, RoutedEventArgs e)
        {
            //String operation = "updateBegin";
            if (GridViewNur.SelectedItem == null)
            {
                PublicMethod.RadAlterBox("请选择一项", "提示");
                return;
            }

            int Mxxh = (GridViewNur.SelectedItem as CP_DiagNurExecCategoryDetail).Mxxh;

            if ((GridViewNur.SelectedItem as CP_DiagNurExecCategoryDetail).Extension2 == "使用中")
            {
                PublicMethod.RadAlterBox("正在使用的信息禁止修改操作", "提示");
                return;
            }

            if (((CP_DiagNurExecCategoryDetail)GridViewNur.SelectedItem).Extension2.ToString() == "停用")
            {

                try
                {

                    YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                    Client.UpdateSfsyStateCompleted +=
                    (obj, ea) =>
                    {

                        if (ea.Error == null)
                        {
                            BindGridView();

                            PublicMethod.RadAlterBox("启用成功", "提示");
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(ea.Error);
                        }
                    };

                    Client.UpdateSfsyStateAsync(((CP_DiagNurExecCategoryDetail)GridViewNur.SelectedItem).Mxxh, 0);
                    Client.CloseAsync();
                }
                catch (Exception ex)
                {
                    YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }

            }
            else
            {
                PublicMethod.RadAlterBox("状态已经为启用", "提示"); return;
            }


        }
        /// <summary>
        /// 停用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            //String operation = "updateEnd";
            if (GridViewNur.SelectedItem == null)
            {
                PublicMethod.RadAlterBox("请选择一项", "提示");
                return;
            }

            int Mxxh = (GridViewNur.SelectedItem as CP_DiagNurExecCategoryDetail).Mxxh;

            if ((GridViewNur.SelectedItem as CP_DiagNurExecCategoryDetail).Extension2 == "使用中")
            {
                PublicMethod.RadAlterBox("正在使用的信息禁止修改操作", "提示");
                return;
            }
            if (((CP_DiagNurExecCategoryDetail)GridViewNur.SelectedItem).Extension2.ToString() == "启用")
            {
                try
                {

                    YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                    Client.UpdateSfsyStateCompleted +=
                    (obj, ea) =>
                    {

                        if (ea.Error == null)
                        {
                            BindGridView();
                            PublicMethod.RadAlterBox("停用成功", "提示");
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(ea.Error);
                        }
                    };

                    Client.UpdateSfsyStateAsync(((CP_DiagNurExecCategoryDetail)GridViewNur.SelectedItem).Mxxh, 1);
                    Client.CloseAsync();
                }
                catch (Exception ex)
                {
                    YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }

            }
            else
            {
                PublicMethod.RadAlterBox("状态已经为停用", "提示"); return;
            }
        }

        /// <summary>
        /// 表示诊疗信息分类维护按钮的点击事件
        /// </summary>
        private void btnHlflWh_Click(object sender, RoutedEventArgs e)
        {
            RWDiagNurExecCategory rwdnec = new RWDiagNurExecCategory();
            rwdnec.Closed += new EventHandler<WindowClosedEventArgs>(rwdnec_Closed);
            rwdnec.ResizeMode = ResizeMode.NoResize;
            rwdnec.ShowDialog();
        }
        private void rwdnec_Closed(object sender, EventArgs e)
        {
            //刷新数据源
            BindGridView();
            BindCbx();
        }


        #region  输入框加回车事件
        private void RegisterKeyEvent()
        {
            cbxInsert.KeyUp += new KeyEventHandler(cbxInsert_KeyUp);
            txtInsert.KeyUp += new KeyEventHandler(txtInsert_KeyUp);
            txtPy.KeyUp += new KeyEventHandler(txtPy_KeyUp);
            //cmbYxjl.KeyUp += new KeyEventHandler(cmbYxjl_KeyUp);
            btnCreateCode.KeyUp += new KeyEventHandler(btnCreateCode_KeyUp);

        }

        private void cbxInsert_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtInsert.Focus();
        }

        private void txtInsert_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtPy.Focus();
        }
        private void txtPy_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnCreateCode.Focus();
        }

        //private void cmbYxjl_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //        btnCreateCode.Focus();
        //}

        private void btnCreateCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnCreate_Click(null, null);
        }

        #endregion
        #endregion

        #region 函数
        /// <summary>
        /// 绑定数据源
        /// </summary>
        private void BindGridView()
        {
            try
            {
                radBusyIndicator.IsBusy = true;


                YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                Client.GetDiagNurExecCategoryDetailsCompleted +=
                (obj, e) =>
                {
                    radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        GridViewNur.ItemsSource = e.Result;
                        //初始化查询数据源
                        m_listsouce = (ObservableCollection<CP_DiagNurExecCategoryDetail>)GridViewNur.ItemsSource;
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };

                Client.GetDiagNurExecCategoryDetailsAsync(1, true);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 绑定分类类型
        /// </summary>
        private void BindCbx()
        {
            try
            {

                YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                Client.GetDiagNurExecCategoryCompleted +=
                (obj, e) =>
                {

                    if (e.Error == null)
                    {
                        cbxQuery.ItemsSource = e.Result.ToList();
                        cbxInsert.ItemsSource = e.Result.ToList();
                        cbxInsert.SelectedIndex = 0;

                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };

                Client.GetDiagNurExecCategoryAsync(1, false);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        #endregion



        private void btnClearCode_Click(object sender, RoutedEventArgs e)
        {
            cbxQuery.SelectedIndex = -1;
            cbxYxjlQuery.SelectedIndex = -1;
            cbxSfsyQuery.SelectedIndex = -1;
            BindGridView();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CurrentState = OperationState.VIEW;

            //NewAdviceGroupDetail();
            //txtInsert.Text = "";
            //txtPy.Text = "";
        }

        /// <summary>
        /// 清空控件
        /// </summary>
        void NewAdviceGroupDetail()
        {
            cbxInsert.SelectedIndex = -1;
            txtInsert.Text = "";
            txtPy.Text = "";
        }
    }
}
