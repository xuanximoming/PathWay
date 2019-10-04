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
    public partial class RWCyXDF
    {
       

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
                if (this.revCyXDF.SelectedItem == null)
                {
                    //PublicMethod.RadAlterBox("请选中要删除的行！", "提示");
                    YiDanMessageBox.Show("请选中要删除的行!");
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

                //            cyfCategoryList.Remove(cyxdfCategory);
                //            ClearTextBox();
                //            PublicMethod.RadAlterBox("删除成功！", "提示");
                //        }
                //        else
                //        {
                //            PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                //        }
                //    };
                //    client.DeleteNurExecCategoryAsync(cyxdfCategory.Lbxh);
                //    #endregion
                //};
                //client.CheckNurExecCategoryDeleteAsync(cyxdfCategory.Lbxh);
            }
        }
        /// <summary>
        /// 修改按钮事件
        /// </summary>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.revCyXDF.SelectedItem== null)
                {
                    //PublicMethod.RadAlterBox("请选中要修改的行！", "提示");
                    YiDanMessageBox.Show("请选中要修改的行!");
                    return;
                }
                cyxdfCategory = (CP_CYXDF)revCyXDF.SelectedItem;
                m_funstate = EditState.Edit;
                BindBtnState();
                txtCyXDFName.Focus();
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
                this.revCyXDF.SelectedItem = null;
                BindBtnState();
                txtCyXDFName.Focus();
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
                //revCyXDF.SelectedItem = null;
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
                if (txtCyXDFName.Text.Trim() == "")
                {
                    //PublicMethod.RadAlterBoxRe("处方名称不能为空！", "提示", txtCyXDFName); 
                    YiDanMessageBox.Show("处方名称不能为空！", txtCyXDFName);
                    //isLoad = false;
                    return;
                }
                //if (txtPy.Text.Trim() == "")
                //{
                //    PublicMethod.RadAlterBoxRe("处方拼音不能为空！", "提示", txtPy); isLoad = false;
                //    return;
                //}
                //if (txtWb.Text.Trim() == "")
                //{
                //    PublicMethod.RadAlterBoxRe("处方五笔不能为空！", "提示", txtWb); isLoad = false;
                //    return;
                //}
                if (txtfs.Text.Trim() == "")
                {
                    //PublicMethod.RadAlterBoxRe("处方付数不能为空！", "提示", txtfs); isLoad = false;
                    YiDanMessageBox.Show("处方付数不能为空！", txtfs);
                    return;
                }
                if (autoCompleteBoxDeptYf.Text.Trim() == "" || autoCompleteBoxDeptYf.SelectedItem==null)
                {
                    //PublicMethod.RadAlterBoxRe("默认药房不能为空！", "提示", autoCompleteBoxDeptYf); isLoad = false;
                    YiDanMessageBox.Show("默认药房不能为空！", autoCompleteBoxDeptYf);
                    return;
                }

                #region 初始化CP_CYXDF对象并赋值
                CP_CYXDF _cyxdfdetail = new CP_CYXDF();
                _cyxdfdetail.cfmc = txtCyXDFName.Text;
                _cyxdfdetail.Py = txtPy.Text;
                _cyxdfdetail.Wb = txtWb.Text;
                _cyxdfdetail.cjrq = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                _cyxdfdetail.lrdm = txtlrdm.Text;
                _cyxdfdetail.ylmfl = txtylm.Text;
                _cyxdfdetail.yplh = txtcllx.Text;
                _cyxdfdetail.zxyjr = "0";
                _cyxdfdetail.tsbz = 0;
                _cyxdfdetail.Yfdm = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDeptYf.SelectedItem)).Ksdm;
                _cyxdfdetail.Zxksdm = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm; 
                _cyxdfdetail.mbbz = 0;
                _cyxdfdetail.czyh = Global.LogInEmployee.Zgdm;
                _cyxdfdetail.Ksdm = Global.LogInEmployee.Ksdm;

                _cyxdfdetail.cfts = int.Parse(this.txtfs.Text.Trim());
                _cyxdfdetail.jlzt = 0;
                _cyxdfdetail.jdcfbz =1;
                _cyxdfdetail.Yzkx = 1;
                _cyxdfdetail.Isjj = 0;
                _cyxdfdetail.sqdmbxh = 0;
                _cyxdfdetail.Extension = "";
                _cyxdfdetail.Extension1 = "";
                _cyxdfdetail.Extension2 = "";
                _cyxdfdetail.Extension3 = "";
                _cyxdfdetail.Extension4 = "";
                #endregion
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                if (m_funstate == EditState.Add)
                {
                     
                    #region 验证分类名称是否重复
                     client.InsertCYXDFCompleted += (obj, ea) =>
                         {
                             if (ea.Result == 1)
                             {
                         
                                 revCyXDF.SelectedItem = null;
                                 BindBtnState();
                                 ClearTextBox();
                                 GetCYXDFCategory();
                                 YiDanMessageBox.Show("添加成功！");
                                 //PublicMethod.RadAlterBox("添加成功！", "提示");
                                
                             }
                             //else if (ea.Result == 2)
                             //{

                            
                             //    PublicMethod.RadAlterBoxRe("该项目已存在，请重新输入！", "提示", txtCyXDFName); isLoad = false;
                             //    return;
                             //}
                             else
                             {
                                 PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                             }
                             
                         };
                     client.InsertCYXDFAsync(_cyxdfdetail);
                     m_funstate = EditState.View;
                    #endregion
                }
                if (m_funstate == EditState.Edit)
                {
                    _cyxdfdetail.ID = cyxdfCategory.ID;
                    _cyxdfdetail.cfmc = txtCyXDFName.Text;
                    _cyxdfdetail.Py = txtPy.Text;
                    _cyxdfdetail.Wb = txtWb.Text;
                    _cyxdfdetail.cjrq = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    _cyxdfdetail.lrdm = txtlrdm.Text;
                    _cyxdfdetail.ylmfl = txtylm.Text;
                    _cyxdfdetail.yplh = txtcllx.Text;
                    _cyxdfdetail.Yfdm = ((CP_DepartmentList)(autoCompleteBoxDeptYf.SelectedItem)).Ksdm;
                    _cyxdfdetail.Zxksdm = ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm;
                    _cyxdfdetail.cfts = int.Parse(txtfs.Text.Trim());
                    client.UpdateCYXDFCompleted += (obj, ea) =>
                        {
                            //if (ea.Result == 2)
                            //{
                            //    PublicMethod.RadAlterBoxRe("该项目已存在，请重新输入！", "提示", txtCyXDFName); isLoad = false;
                            //    return;
                            //}
                            if (ea.Result == 1)
                            {
                                
                                revCyXDF.SelectedItem = null;
                                NewAdviceGroupDetail();
                                BindBtnState();
                                GetCYXDFCategory();
                                YiDanMessageBox.Show("修改成功！");
                                //PublicMethod.RadAlterBox("修改成功！", "提示");
                            }
                            else
                            {
                                PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                            }
                        };
                    client.UpdateCYXDFAsync(_cyxdfdetail);
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
            
            //绑定GirdView控件
            IntiComboBoxDeptYf();
            IntiComboBoxDept();
            GetCYXDFCategory();
            m_funstate = EditState.View;
        }

        /// <summary>
        /// 行选中改变事件
        /// </summary>
        private void revCyXDF_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                if (m_funstate == EditState.Add)
                {
                    cyxdfCategory = null;
                    return;
                }

                if (revCyXDF.SelectedItem == null)
                {
                    return;
                }
                if (m_funstate == EditState.View)
                {
                    #region 文本框赋值
                    cyxdfCategory = (CP_CYXDF)revCyXDF.SelectedItem;
                    txtCyXDFName.Text = cyxdfCategory.cfmc;
                    txtPy.Text = cyxdfCategory.Py;
                    txtWb.Text = cyxdfCategory.Wb;
                    txtylm.Text = cyxdfCategory.ylmfl;
                    txtlrdm.Text = cyxdfCategory.lrdm;
                    txtfs.Text = cyxdfCategory.cfts.ToString();
                    autoCompleteBoxDept.SelectedItem = ((ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource).FirstOrDefault(cp => cp.Ksdm.Equals(cyxdfCategory.Zxksdm));
                    autoCompleteBoxDeptYf.SelectedItem = ((ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource).FirstOrDefault(cp => cp.Ksdm.Equals(cyxdfCategory.Yfdm));
                   
                    BindBtnState();
                    //txtCyXDFName.IsEnabled = false;
                    //txtylm.IsEnabled = false;
                    //txtlrdm.IsEnabled = false;
                    //txtfs.IsEnabled = false;
                    //autoCompleteBoxDept.IsEnabled = false;
                    //autoCompleteBoxDeptYf.IsEnabled = false;
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
             
            txtCyXDFName.KeyUp += new KeyEventHandler(txtCyXDFName_KeyUp);
            //txtPy.KeyUp += new KeyEventHandler(txtPy_KeyUp);
            //txtWb.KeyUp += new KeyEventHandler(txtWb_KeyUp);
            txtfs.KeyUp += new KeyEventHandler(txtfs_KeyUp);

            txtlrdm.KeyUp += new KeyEventHandler(txtlrdm_KeyUp);
            autoCompleteBoxDeptYf.KeyUp += new KeyEventHandler(autoCompleteBoxDeptYf_KeyUp);
            autoCompleteBoxDept.KeyUp += new KeyEventHandler(autoCompleteBoxDept_KeyUp);
            txtcllx.KeyUp += new KeyEventHandler(txtcllx_KeyUp);
            txtylm.KeyUp += new KeyEventHandler(txtylm_KeyUp);
            btnSave.KeyUp += new KeyEventHandler(btnSave_KeyUp);

        }

         

        private void txtCyXDFName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtfs.Focus();
        }


        //private void txtPy_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //        txtWb.Focus();
        //}

        //private void txtWb_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //        txtfs.Focus();
        //}
        private void txtfs_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtlrdm.Focus();
        }
        private void txtlrdm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                autoCompleteBoxDeptYf.Focus();
        }
        private void autoCompleteBoxDeptYf_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                autoCompleteBoxDept.Focus();
        }
        private void autoCompleteBoxDept_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtcllx.Focus();
        }
        private void txtcllx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtylm.Focus();
        }
        private void txtylm_KeyUp(object sender, KeyEventArgs e)
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

        public RWCyXDF()
        {
            InitializeComponent();
            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
            };
            RegisterKeyEvent();
        }

        #region 执行科室
        /// <summary>
        /// 执行科室
        /// </summary>
        private void IntiComboBoxDept()
        {
            try
            {
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                referenceClient.GetDepartmentListInfoCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            autoCompleteBoxDept.ItemsSource = e.Result;
                            autoCompleteBoxDept.ItemFilter = DeptFilter;
                            //if (e.Result != null)
                            //{
                            //   
                            //    autoCompleteBoxDept.SelectedItem = ((ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource).First(where => where.Ksdm.Equals("205"));
                            //}
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
                referenceClient.GetDepartmentListInfoAsync();
                referenceClient.CloseAsync();

            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        public bool DeptFilter(string strFilter, object item)
        {
            CP_DepartmentList deptList = (CP_DepartmentList)item;
            return ((deptList.QueryName.StartsWith(strFilter.ToUpper())) || (deptList.QueryName.Contains(strFilter.ToUpper())));
        }
        #endregion
        #region 初始化默认药房
        /// <summary>
        /// 初始化默认药房
        /// </summary>
        private void IntiComboBoxDeptYf()
        {
            try
            {
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                referenceClient.GetDepartmentListInfoCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            autoCompleteBoxDeptYf.ItemsSource = e.Result;
                            autoCompleteBoxDeptYf.ItemFilter = DeptFilterYf;
                            if (e.Result != null)
                            {
                                //默认选中 市区门诊草药房
                                autoCompleteBoxDeptYf.SelectedItem = ((ObservableCollection<CP_DepartmentList>)autoCompleteBoxDeptYf.ItemsSource).First(where => where.Ksdm.Equals(Global.LogInEmployee.Ksdm));
                            }
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
                referenceClient.GetDepartmentListInfoAsync();
                referenceClient.CloseAsync();

            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        public bool DeptFilterYf(string strFilter, object item)
        {
            CP_DepartmentList deptList = (CP_DepartmentList)item;
            return ((deptList.QueryName.StartsWith(strFilter.ToUpper())) || (deptList.QueryName.Contains(strFilter.ToUpper())));
        }
        #endregion

        /// <summary>
        /// 获取草药协定方主表
        /// </summary>
        private void GetCYXDFCategory()
        {
            try
            {

                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.GetCyxdfInfoCompleted += (obj, ea) =>
                {
                    if (ea.Error == null)
                    {
                        cyfCategoryList = ea.Result;
                        revCyXDF.ItemsSource = ea.Result;
                    }
                };
                client.GetCyxdfInfoAsync();
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
            txtCyXDFName.IsEnabled  = btnSave.IsEnabled = bl1;
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
            txtCyXDFName.Text = String.Empty;

            txtPy.Text = String.Empty;
            txtWb.Text = String.Empty;
            txtlrdm.Text = String.Empty;
            txtfs.Text = String.Empty;
            txtcllx.Text = String.Empty;
            autoCompleteBoxDept.SelectedItem = null;
            autoCompleteBoxDept.Text = String.Empty;
            txtylm.Text = String.Empty;
            autoCompleteBoxDeptYf.SelectedItem = null;
            autoCompleteBoxDeptYf.Text = String.Empty;
            
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

                txtCyXDFName.IsEnabled = true;
                //五笔 拼音智能获取
                txtPy.IsEnabled = false;
                txtWb.IsEnabled = false;
                txtlrdm.IsEnabled = true;
                txtfs.IsEnabled = true;
                txtcllx.IsEnabled = true;
                autoCompleteBoxDept.IsEnabled = true;
                txtylm.IsEnabled = true;
                autoCompleteBoxDeptYf.IsEnabled = true;

            }
            else if (m_funstate == EditState.Edit)
            {
                SetEnabled(true, false);
                
                txtCyXDFName.IsEnabled = true;
                txtPy.IsEnabled = false;
                txtWb.IsEnabled = false;
                txtlrdm.IsEnabled = true;
                txtfs.IsEnabled = true;
                txtcllx.IsEnabled = true;
                autoCompleteBoxDept.IsEnabled = true;
                txtylm.IsEnabled = true;
                autoCompleteBoxDeptYf.IsEnabled = true;
            }
            else
            {
                SetEnabled(false, true);
                txtCyXDFName.IsEnabled = false;
                txtPy.IsEnabled = false;
                txtWb.IsEnabled = false;
                txtlrdm.IsEnabled = false;
                txtfs.IsEnabled = false;
                txtcllx.IsEnabled = false;
                autoCompleteBoxDept.IsEnabled = false;
                txtylm.IsEnabled = false;
                autoCompleteBoxDeptYf.IsEnabled = false;

            }
        }
        #endregion

        #region 变量
        /// <summary>
        /// 检查项分类集合
        /// </summary>
        ObservableCollection<CP_CYXDF> cyfCategoryList;
        /// <summary>
        /// 按钮状态
        /// </summary>
        EditState m_funstate;
        /// <summary>
        /// 选中的检查项分类信息
        /// </summary>
        CP_CYXDF cyxdfCategory;
        #endregion
        /// <summary>
        /// 清空控件
        /// </summary>
        void NewAdviceGroupDetail()
        {
            //cbxInsert.SelectedIndex = -1;
            txtCyXDFName.Text = "";
           
        }

    }
}
