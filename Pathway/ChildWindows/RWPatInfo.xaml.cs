using DrectSoft.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Views.ChildWindows
{
    /// <summary>
    /// 表示病人信息维护的类
    /// </summary>
    public partial class RWPatInfo
    {
        bool isTrue = true;
        public int m_iPage = 0;
        public string m_sOld_Auto_Ryzd = "";
        public string m_sNew_Auto_Ryzd = "";
        public CP_InpatinetList m_CurrentPat = null;
        public bool isRyzdChanged = false;

        public RWAccessPath2 accessWindow;

        public int m_iaccess = 0;
        #region 事件

        /// <summary>
        /// 表示窗体加载的事件
        /// </summary>
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isTrue)
                {
                    isTrue = true;
                    return;
                }
                BindList();
                txtSyxh.Text = m_CurrentPat.Syxh;//绑定首页序号
                GetExamDictionaryDetail();//绑定检查项目
                GetPatientExamItem();//绑定病人检查信息


            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /*
        /// <summary>
        /// 
        /// </summary>
        /// <param name="idc"></param>
        /// <returns></returns>
        public string GetIDCName(string idc)
        {
            try
            {
                string sql = "select Name from Diagnosis where ICD =" + "'" +
                    idc + "'";
                DataTable dt = GetTableBySQL(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Name"].ToString();
                }
                return "某某路径";
            }
            catch (Exception)
            {
                throw;
            }
        }
        */
        /// <summary>
        /// 表示保存按钮的点击事件
        /// </summary>
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (ValidControls())
                //{ 
                if (txt_sfzh.Text.Length > 18)
                {
                    PublicMethod.RadAlterBoxRe("身份证号码长度超出!", "提示", txt_sfzh);
                    isTrue = false;
                    return;
                }
                if (txt_dwdh.Text.Length > 18)
                {
                    PublicMethod.RadAlterBoxRe("电话号码长度超出!", "提示", txt_dwdh);
                    isTrue = false;
                    return;
                }
                //if (txt_lxrm.Text.Length == 0)
                //{
                //    PublicMethod.RadAlterBoxRe("姓名不能为空!", "提示", txt_lxrm);
                //    isTrue = false;
                //    return;
                //}
                //if (txt_lxgzdh.Text.Length == 0)
                //{
                //    PublicMethod.RadAlterBoxRe("工作电话不能为空!", "提示", txt_lxgzdh);
                //    isTrue = false;
                //    return;
                //}
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                CP_InPatient patInfo = UpdatePatientInfo();
                client.UpdatePatInfoCompleted +=
                   (obj, ea) =>
                   {

                       if (ea.Error == null) //成功
                       {
                           //**********2013-05-10,WangGuojin***************************
                           this.m_sNew_Auto_Ryzd = this.auto_ryzd.Text.Trim();
                           if (this.m_sNew_Auto_Ryzd != null && this.m_sOld_Auto_Ryzd != this.m_sNew_Auto_Ryzd)
                               this.isRyzdChanged = true;

                           if (this.auto_ryzd.SelectedItem != null)//this.isRyzdChanged == true)
                           {
                               //根据IDC码获取名称,2013-05-10,WangGuojin.
                               //m_CurrentPat.Ryzd = patInfo.Ryzd;
                               m_CurrentPat.RyzdCode = patInfo.Ryzd;// patInfo.RyzdCode;
                               //DialogParameters parameters = new DialogParameters();
                               //parameters.Content = String.Format("{0}", "更新成功. 请问是否需要进入路经评估？");
                               //parameters.Header = "提示";
                               //parameters.IconContent = null;
                               //parameters.OkButtonContent = "是";
                               //parameters.CancelButtonContent = "否";
                               //parameters.Closed = OnInPathDialog;//进入路径
                               //RadWindow.Confirm(parameters);
                               if (this.tabItem1.IsSelected == true && Global.InpatientListCurrent.Ljdm == "")
                               {
                                   YidanPathWayMessageBox mess = new YidanPathWayMessageBox("更新成功. 请问是否需要进入路经评估？", "提示", YiDanMessageBoxButtons.YesNo);
                                   mess.ShowDialog();
                                   mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
                               }
                               else
                               {
                                   PublicMethod.RadAlterBox("更新成功!", "提示");
                                   this.DialogResult = true;
                               }

                           }
                           else
                           {
                               PublicMethod.RadAlterBox("更新成功!", "提示");
                               this.DialogResult = true;
                           }
                           //**********************************************************
                       }
                       else//失败
                       {
                           throw new NotImplementedException();
                       }

                   };
                client.UpdatePatInfoAsync(patInfo);


                ObservableCollection<Modal_PatientContactsInfo> list1 = new ObservableCollection<Modal_PatientContactsInfo>();
                Modal_PatientContactsInfo modalPatientContactsInfo = new Modal_PatientContactsInfo();

                modalPatientContactsInfo.Lxrm = this.txt_lxrm.Text;

                //更新联系人性别到数据库
                modalPatientContactsInfo.Lxrxb = (this.auto_lxrxb.SelectedItem == null) ? string.Empty : ((Modal_Dictionary)this.auto_lxrxb.SelectedItem).Mxdm;
                //更新联系人关系代码到数据库
                modalPatientContactsInfo.Lxgx = (this.auto_lxgx.SelectedItem == null) ? string.Empty : ((Modal_Dictionary)this.auto_lxgx.SelectedItem).Mxdm;



                modalPatientContactsInfo.Lxdz = this.txt_lxdz.Text;
                modalPatientContactsInfo.Lxdw = this.txt_lxdw.Text;
                modalPatientContactsInfo.Lxjtdh = this.txt_lxjtdh.Text;
                modalPatientContactsInfo.Lxgzdh = this.txt_lxgzdh.Text;
                modalPatientContactsInfo.Lxyb = this.txt_lxyb.Text;
                list1.Add(modalPatientContactsInfo);
                client.UpdatePatientContactsInfoCompleted +=
                   (obj, ea) =>
                   {
                       if (ea.Error == null) //成功
                       {

                           YidanEHRDataServiceClient client2 = PublicMethod.YidanClient;

                           client2.SelectFirstPationContactsInfoCompleted +=
                               (obj2, ea2) =>
                               {
                                   if (ea.Error == null)
                                   {
                                       List<Modal_PatientContactsInfo> modal_PatientContactsInfo = ea2.Result.ToList();
                                       if (modal_PatientContactsInfo.Count != 0)
                                       {
                                           //modal_PatientContactsInfo.Where(contacts => contacts.Lxrbz.Equals(1));
                                           modal_PatientContactsInfo = modal_PatientContactsInfo.Where(contacts => contacts.Lxrbz.Equals(1)).ToList();
                                           //this.txt_lxdw.Text = string.IsNullOrEmpty(modal_PatientContactsInfo[0].Lxdw) ? string.Empty : modal_PatientContactsInfo[0].Lxdw;
                                           this.txt_lxdw.Text = string.IsNullOrEmpty(modal_PatientContactsInfo[0].Lxdw) ? string.Empty : modal_PatientContactsInfo[0].Lxdw;
                                           this.txt_lxdz.Text = string.IsNullOrEmpty(modal_PatientContactsInfo[0].Lxdz) ? string.Empty : modal_PatientContactsInfo[0].Lxdz;
                                           if (modalDictionary.Where(lxgx => lxgx.Lbdm.Equals("44") && lxgx.Mxdm.Equals(modal_PatientContactsInfo[0].Lxgx)) == null)
                                           {
                                               this.auto_lxgx.Text = "";
                                           }
                                           else
                                           {
                                               this.auto_lxgx.SelectedItem = modalDictionary.Where(lxgx => lxgx.Lbdm.Equals("44") && lxgx.Mxdm.Equals(modal_PatientContactsInfo[0].Lxgx)).FirstOrDefault();
                                           }

                                           if (modalDictionary.Where(lxrxb => lxrxb.Lbdm.Equals("3") && lxrxb.Mxdm.Equals(modal_PatientContactsInfo[0].Lxrxb)) == null)
                                           {
                                               this.auto_lxrxb.Text = "";
                                           }
                                           else
                                           {
                                               this.auto_lxrxb.SelectedItem = modalDictionary.Where(lxrxb => lxrxb.Lbdm.Equals("3") && lxrxb.Mxdm.Equals(modal_PatientContactsInfo[0].Lxrxb)).FirstOrDefault();
                                           }
                                           this.txt_lxgzdh.Text = string.IsNullOrEmpty(modal_PatientContactsInfo[0].Lxgzdh) ? string.Empty : modal_PatientContactsInfo[0].Lxgzdh;
                                           this.txt_lxjtdh.Text = string.IsNullOrEmpty(modal_PatientContactsInfo[0].Lxjtdh) ? string.Empty : modal_PatientContactsInfo[0].Lxjtdh;
                                           this.txt_lxrm.Text = string.IsNullOrEmpty(modal_PatientContactsInfo[0].Lxrm) ? string.Empty : modal_PatientContactsInfo[0].Lxrm;

                                           this.txt_lxyb.Text = string.IsNullOrEmpty(modal_PatientContactsInfo[0].Lxyb) ? string.Empty : modal_PatientContactsInfo[0].Lxyb;
                                       }
                                   }
                               };
                           client2.SelectFirstPationContactsInfoAsync(Convert.ToDecimal(m_CurrentPat.Syxh));
                           BindList();
                       }
                       else//失败
                       {
                           throw new NotImplementedException();
                       }
                   };
                client.UpdatePatientContactsInfoAsync(list1, Convert.ToDecimal(m_CurrentPat.Syxh));
            }
            //}
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
                    accessWindow = new RWAccessPath2(m_CurrentPat, this.m_iPage);
                    accessWindow.Closed += new EventHandler<WindowClosedEventArgs>(accessWindow_Closed);
                    accessWindow.ShowDialog();
                    accessWindow.Focus();
                    m_iaccess = 1;
                    //this.Close();
                }
                else
                {
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        //2013-05-10,WangGuojin.
        void OnInPathDialog(object sender, WindowClosedEventArgs e)
        {
            try
            {
                if (e.DialogResult == true)
                {

                    accessWindow = new RWAccessPath2(m_CurrentPat, this.m_iPage);
                    accessWindow.Closed += new EventHandler<WindowClosedEventArgs>(accessWindow_Closed);
                    accessWindow.ShowDialog();
                    m_iaccess = 1;
                    //this.Close();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void accessWindow_Closed(object sender, WindowClosedEventArgs e)
        {
            try
            {
                accessWindow = null;
                if (((RWAccessPath2)sender).DialogResult == true)
                {
                    this.DialogResult = true;
                    this.Close();
                }
                else
                {
                    this.DialogResult = false;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 表示取消按钮的点击事件
        /// </summary>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }

        private void Editor_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                Control ctrl = sender as Control;
                if (ctrl != null)
                {
                    ctrl.Focus();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void auto_ryzd_Populating(object sender, PopulatingEventArgs e)
        {
            e.Cancel = false;
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.SelectRyzdListByFilterCompleted +=
              (obj, ea) =>
              {
                  if (ea.Error == null)
                  {
                      List<Modal_Diagnosis> list = ea.Result.ToList();
                      //auto_ryzd.ItemsSource = null;
                      auto_ryzd.ItemsSource = list;
                      //auto_ryzd.PopulateComplete();
                  }
                  else
                  {
                      throw new NotImplementedException();
                  }
              };
            client.SelectRyzdListByFilterAsync(e.Parameter);
        }

        private void auto_hy_TextChanged(object sender, RoutedEventArgs e)
        {
            System.Windows.Controls.AutoCompleteBox s = (AutoCompleteBox)sender;
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Colors.Black;
            s.Foreground = brush;
        }

        /// <summary>
        /// 表示病人检查项-删除按钮的事件
        /// </summary>
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.None;
                BindBtnState();
                if (rpe == null)
                {
                    PublicMethod.RadAlterBox("请选中一条数据!", "提示");
                    return;
                }

                //DialogParameters parameters = new DialogParameters();
                //parameters.Content = String.Format("{0}", "请问是否删除检查项信息？");
                //parameters.Header = "提示";
                //parameters.IconContent = null;
                //parameters.OkButtonContent = "确定"; 
                //parameters.CancelButtonContent = "取消";
                //parameters.Closed = OnDelAdviceGroupDetail;//***close处理***
                //RadWindow.Confirm(parameters);

                YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除检查项信息？", "提示", YiDanMessageBoxButtons.YesNo);
                mess.ShowDialog();
                mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent1);

            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        void mess_PageClosedEvent1(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                    client.DeletePatientExamItemCompleted += (obj, ea) =>
                    {
                        if (ea.Result == 1)
                        {

                            patientExamItemList.Remove(rpe);
                            ClearTextBox();
                            PublicMethod.RadAlterBox("删除成功!", "提示");
                        }
                        else
                        {
                            PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                        }
                    };
                    client.DeletePatientExamItemAsync(rpe.ID);

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
                    YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                    client.DeletePatientExamItemCompleted += (obj, ea) =>
                    {
                        if (ea.Result == 1)
                        {

                            patientExamItemList.Remove(rpe);
                            ClearTextBox();
                            PublicMethod.RadAlterBox("删除成功!", "提示");
                        }
                        else
                        {
                            PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                        }
                    };
                    client.DeletePatientExamItemAsync(rpe.ID);
                }
                catch (Exception ex)
                {
                    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            }
        }

        /// <summary>
        /// 表示病人检查项-修改按钮的事件
        /// </summary>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (gvPatientExamItem.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选中一条数据!", "提示");
                    return;
                }
                m_funstate = EditState.Edit;
                BindBtnState();
                cmbJcxm.Focus();
                cmbJcxm.IsDropDownOpen = false;
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 表示病人检查项-添加按钮的事件
        /// </summary>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.Add;
                BindBtnState();
                cmbJcxm.Focus();
                cmbJcxm.IsDropDownOpen = false;
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 表示病人检查项-取消按钮的事件
        /// </summary>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.None;
                BindBtnState();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 表示病人检查项-保存按钮点击事件
        /// </summary>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cmbJcxm.SelectedItem == null)
                {
                    PublicMethod.RadAlterBoxRe("请选择检查项目！", "提示", cmbJcxm);
                    isTrue = false;
                    return;
                }
                //if (radNumericJcjg.Value <= 0)
                //{
                //    PublicMethod.RadAlterBoxRe("请输入正确的检查结果！", "提示", radNumericJcjg);
                //    isTrue = false;
                //    return;
                //}
                if (txtDw.Text.Trim().Length == 0)
                {
                    PublicMethod.RadAlterBoxRe("请输入单位！", "提示", txtDw);
                    isTrue = false;
                    return;
                }
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                if (m_funstate == EditState.Add)
                {
                    #region 实体赋值
                    RW_PatientExamItem patientExamItem = new RW_PatientExamItem();
                    patientExamItem.Syxh = txtSyxh.Text;
                    patientExamItem.Jcxm = ConvertMy.ToString(cmbJcxm.SelectedValue);
                    patientExamItem.Jcjg = radNumericJcjg.ContentText;
                    if (radNumericJcjg.ContentText == "")
                    {
                        patientExamItem.Jcjg = "0.00";
                    }
                    patientExamItem.Dw = txtDw.Text;
                    patientExamItem.Bz = txtBz.Text;
                    #endregion
                    #region 调用验证重复项方法
                    client.CheckExamItemsCompleted += (obj, ea) =>
                        {
                            if (ea.Result > 0)
                            {
                                PublicMethod.RadAlterBox("该检查项已经存在！", "提示");
                                return;
                            }
                            #region 调用添加方法
                            client.InsertAndSelectPatientExamItemCompleted += (objc, rea) =>
                            {
                                if (rea.Result.ToList() != null)
                                {

                                    patientExamItemList = rea.Result;
                                    gvPatientExamItem.ItemsSource = patientExamItemList;
                                    m_funstate = EditState.None;
                                    gvPatientExamItem.SelectedItem = null;
                                    BindBtnState();
                                    ClearTextBox();
                                    PublicMethod.RadAlterBox("添加成功！", "提示");
                                }
                                else
                                {
                                    PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                                }
                            };
                            client.InsertAndSelectPatientExamItemAsync(patientExamItem);
                            #endregion
                        };
                    client.CheckExamItemsAsync(patientExamItem.Syxh, patientExamItem.Jcxm);
                    #endregion
                }
                if (m_funstate == EditState.Edit)
                {
                    #region 改变实体的值
                    if (rpe == null)
                    {
                        PublicMethod.RadAlterBox("请选则一行数据！", "提示");
                        return;
                    }
                    rpe.Jcxm = ConvertMy.ToString(cmbJcxm.SelectedValue);
                    rpe.Jcmc = ConvertMy.ToString(cmbJcxm.SelectionBoxItem);
                    rpe.Jcjg = radNumericJcjg.ContentText;
                    if (radNumericJcjg.ContentText == "")
                    {
                        rpe.Jcjg = "0.00";
                    }
                    rpe.Dw = txtDw.Text;
                    rpe.Bz = txtBz.Text;
                    #endregion
                    #region 修改的方法
                    client.UpdatePatientExamItemCompleted += (obj, ea) =>
                    {
                        if (ea.Result == 1)
                        {

                            m_funstate = EditState.None;
                            gvPatientExamItem.SelectedItem = null;
                            BindBtnState();
                            PublicMethod.RadAlterBox("修改成功！", "提示");
                        }
                        else
                        {
                            PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                        }
                    };
                    client.UpdatePatientExamItemAsync(rpe);
                    #endregion
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 表示病人检查项-gvPatientExamItem行选择改变事件
        /// </summary>
        private void gvPatientExamItem_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                if (m_funstate == EditState.Add)
                {
                    return;
                }
                rpe = (RW_PatientExamItem)gvPatientExamItem.SelectedItem;
                if (rpe == null)
                {
                    return;
                }
                #region 文本框赋值
                txtID.Text = rpe.ID;
                txtSyxh.Text = rpe.Syxh;
                cmbJcxm.SelectedValue = rpe.Jcxm;
                radNumericJcjg.Value = Convert.ToDouble(rpe.Jcjg);
                txtDw.Text = rpe.Dw;
                txtBz.Text = rpe.Bz;
                #endregion
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void tabRWPatInfo_SelectionChanged(object sender, Telerik.Windows.Controls.RadSelectionChangedEventArgs e)
        {
            if (this.tabRWPatInfo == null)
            {
                return;
            }
            RadTabItem item = (RadTabItem)this.tabRWPatInfo.SelectedItem;
            if (item.Name == "tabPatientExamItem")
            {
                OKButton.Visibility = Visibility.Collapsed; //CancelButton.Visibility =
            }
            else
            {
                OKButton.Visibility = Visibility.Visible;   //CancelButton.Visibility =
            }
        }

        #endregion

        #region 方法

        public RWPatInfo(CP_InpatinetList currentpat)
        {
            InitializeComponent();
            m_CurrentPat = currentpat;
            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };
            RegisterKeyEvent();
        }
        /// <summary>
        /// 新增构造函数，2013-05-10，WangGuojin， in order to show 
        /// </summary>
        public RWPatInfo(CP_InpatinetList currentpat, int iPage = 0)
        {
            InitializeComponent();
            m_CurrentPat = currentpat;
            //PublicMethod.RadAlterBox(m_CurrentPat.Ryzd + "," + m_CurrentPat.RyzdCode, "提示");
            //病人列表页面初次加载患者诊断页面判断
            if (iPage > 0)
            {
                //如果诊断为空给出提示
                if (currentpat.RyzdCode == "")
                {
                    groupBox5.Header = "就诊信息(第一次进入路径需要先设置入院诊断，后根据入院诊断选择进入路径)";
                }
                //m_iPage = iPage;
                //tabItem_jbxx.IsSelected = false;
                this.tabItem1.IsSelected = true;
            }
            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };
            RegisterKeyEvent();

        }

        /// <summary>
        /// 
        /// </summary>
        public RWPatInfo()
        {
            InitializeComponent();
        }

        private bool ValidControls()
        {
            Object[] autoCompletes = new Object[]{this.auto_hy,this.auto_mz,this.auto_gj,
                    this.auto_xb,this.auto_whcd,this.auto_csss,this.auto_csqx,this.auto_jgss,
                this.auto_jgqx,this.auto_brxz,this.auto_zy,this.auto_ryzd, this.auto_lxrxb,
                 this.auto_lxgx};
            foreach (Object autoComplete in autoCompletes)
            {
                if (((AutoCompleteBox)autoComplete).SelectedItem == null && ((AutoCompleteBox)autoComplete).Text != "")
                {


                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = Colors.Red;
                    ((AutoCompleteBox)autoComplete).Foreground = brush;

                    ((AutoCompleteBox)autoComplete).Focus();
                    PublicMethod.RadAlterBox("输入了错误数据!", "提示");
                    return false;
                }
            }
            return true;
        }

        private CP_InPatient UpdatePatientInfo()
        {
            CP_InPatient patInfo = new YidanEHRApplication.DataService.CP_InPatient();
            patInfo.Syxh = Convert.ToDecimal(m_CurrentPat.Syxh);
            patInfo.Hyzk = (this.auto_hy.SelectedItem == null) ? string.Empty : ((Modal_Dictionary)this.auto_hy.SelectedItem).Mxdm;
            patInfo.Mzdm = (this.auto_mz.SelectedItem == null) ? string.Empty : ((Modal_Dictionary)this.auto_mz.SelectedItem).Mxdm;
            patInfo.Gjdm = (this.auto_gj.SelectedItem == null) ? string.Empty : ((Modal_Dictionary)this.auto_gj.SelectedItem).Mxdm;
            patInfo.Brxb = (this.auto_xb.SelectedItem == null) ? string.Empty : ((Modal_Dictionary)this.auto_xb.SelectedItem).Mxdm;
            patInfo.Whcd = (this.auto_whcd.SelectedItem == null) ? string.Empty : ((Modal_Dictionary)this.auto_whcd.SelectedItem).Mxdm;
            patInfo.Ssdm = (this.auto_csss.SelectedItem == null) ? string.Empty : ((Modal_Areas)this.auto_csss.SelectedItem).Dqdm;
            patInfo.Qxdm = (this.auto_csqx.SelectedItem == null) ? string.Empty : ((Modal_Areas)this.auto_csqx.SelectedItem).Dqdm;
            patInfo.Jgssdm = (this.auto_jgss.SelectedItem == null) ? string.Empty : ((Modal_Areas)this.auto_jgss.SelectedItem).Dqdm;
            patInfo.Jgqxdm = (this.auto_jgqx.SelectedItem == null) ? string.Empty : ((Modal_Areas)this.auto_jgqx.SelectedItem).Dqdm;
            patInfo.Brxz = (this.auto_brxz.SelectedItem == null) ? string.Empty : ((Modal_Dictionary)this.auto_brxz.SelectedItem).Mxdm;
            patInfo.Zydm = (this.auto_zy.SelectedItem == null) ? string.Empty : ((Modal_Dictionary)this.auto_zy.SelectedItem).Mxdm;
            //
            patInfo.Ryzd = (this.auto_ryzd.SelectedItem == null) ? string.Empty : ((Modal_Diagnosis)this.auto_ryzd.SelectedItem).Zdbs;
            //***2013-05-10,WangGuojin,add it.获取IDC和IDC码***
            patInfo.RyzdCode = "";// SqlCommand.//(this.auto_ryzd.SelectedItem == null) ? string.Empty : ((Modal_Diagnosis)this.auto_ryzd.SelectedItem).Zdbs;            

            //***********************************
            patInfo.Csrq = this.datePicker1.Text;
            patInfo.Xsnl = this.txt_nl.Text;

            patInfo.Zjxy = this.txt_zjxy.Text;
            patInfo.Sfzh = this.txt_sfzh.Text;


            patInfo.Jynx = this.txt_jynx.Text.Equals(string.Empty) ? 0 : Convert.ToDecimal(this.txt_jynx.Text);


            patInfo.Gzdw = this.txt_gzdw.Text;
            patInfo.Dwdz = this.txt_dwdz.Text;
            patInfo.Dwdh = this.txt_dwdh.Text;

            patInfo.Dwyb = this.txt_dwyb.Text;

            patInfo.Hkdz = this.txt_hkdz.Text;
            patInfo.Hkdh = this.txt_hkdh.Text;
            patInfo.Hkyb = this.txt_hkyb.Text;
            patInfo.Dqdz = this.txt_dqdz.Text;
            int result;
            patInfo.Rycs = int.TryParse(this.txt_rycs.Text, out result) ? result : 0;

            return patInfo;

        }

        private void BindList()
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;

            client.SelectPatInfoCompleted +=
                     (obj, ea) =>
                     {
                         if (ea.Error == null)
                         {
                             Modal_PatientInfo patInfo = ea.Result;
                             modalDictionary = patInfo.CommonDictionary.ToList();
                             modalAreas = patInfo.Areas.ToList();
                             modalDiagnosis = patInfo.Diagnosis.ToList();
                             modalPatientContactsInfo = patInfo.ContactsInfo.ToList();
                             if (modalAreas.Count != 0)
                             {
                                 auto_csss.ItemsSource = modalAreas.Where(areas => areas.Dqlb.Equals("1000"));
                                 auto_csss.ItemFilter = MyItemFilter1;

                                 auto_csqx.ItemsSource = modalAreas.Where(areas => areas.Dqlb.Equals("1001"));
                                 auto_csqx.ItemFilter = MyItemFilter1;

                                 auto_jgss.ItemsSource = modalAreas.Where(areas => areas.Dqlb.Equals("1000"));
                                 auto_jgss.ItemFilter = MyItemFilter1;

                                 auto_jgqx.ItemsSource = modalAreas.Where(areas => areas.Dqlb.Equals("1001"));
                                 auto_jgqx.ItemFilter = MyItemFilter1;
                             }

                             if (modalDictionary.Count != 0)
                             {
                                 auto_hy.ItemsSource = modalDictionary.Where(dictionary => dictionary.Lbdm.Equals("4"));
                                 auto_hy.ItemFilter = MyItemFilter;

                                 auto_mz.ItemsSource = modalDictionary.Where(dictionary => dictionary.Lbdm.Equals("42"));
                                 auto_mz.ItemFilter = MyItemFilter;

                                 auto_gj.ItemsSource = modalDictionary.Where(dictionary => dictionary.Lbdm.Equals("43"));
                                 auto_gj.ItemFilter = MyItemFilter;

                                 auto_xb.ItemsSource = modalDictionary.Where(dictionary => dictionary.Lbdm.Equals("3"));
                                 auto_xb.ItemFilter = MyItemFilter;

                                 auto_whcd.ItemsSource = modalDictionary.Where(dictionary => dictionary.Lbdm.Equals("25"));
                                 auto_whcd.ItemFilter = MyItemFilter;

                                 auto_zy.ItemsSource = modalDictionary.Where(dictionary => dictionary.Lbdm.Equals("41"));
                                 auto_zy.ItemFilter = MyItemFilter;

                                 auto_lxgx.ItemsSource = modalDictionary.Where(dictionary => dictionary.Lbdm.Equals("44"));
                                 auto_lxgx.ItemFilter = MyItemFilter;

                                 auto_brxz.ItemsSource = modalDictionary.Where(dictionary => dictionary.Lbdm.Equals("1"));
                                 auto_brxz.ItemFilter = MyItemFilter;

                                 auto_lxrxb.ItemsSource = modalDictionary.Where(dictionary => dictionary.Lbdm.Equals("3"));
                                 auto_lxrxb.ItemFilter = MyItemFilter;

                             }
                             radGridView1.ItemsSource = modalPatientContactsInfo;

                             GetInpatient();
                             BindFirstPationContactsInfo();


                         }
                     };

            client.SelectPatInfoAsync(m_CurrentPat.Syxh);

        }

        private void GetInpatient()
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;

            client.GetBasicInfoCompleted +=
                     (obj, ea) =>
                     {
                         if (ea.Error == null)
                         {
                             CP_InPatient patInfo = ea.Result;
                             BindPatBasicInfo(patInfo);
                         }
                     };

            client.GetBasicInfoAsync(m_CurrentPat.Syxh);
        }

        /// <summary>
        /// 注册回车事件
        /// </summary>
        private void RegisterKeyEvent()
        {
            auto_hy.KeyUp += new KeyEventHandler(auto_hy_KeyUp);
            auto_mz.KeyUp += new KeyEventHandler(auto_mz_KeyUp);
            auto_gj.KeyUp += new KeyEventHandler(auto_gj_KeyUp);
            datePicker1.KeyUp += new KeyEventHandler(datePicker1_KeyUp);
            txt_nl.KeyUp += new KeyEventHandler(txt_nl_KeyUp);
            auto_xb.KeyUp += new KeyEventHandler(auto_xb_KeyUp);
            txt_zjxy.KeyUp += new KeyEventHandler(txt_zjxy_KeyUp);
            txt_sfzh.KeyUp += new KeyEventHandler(txt_sfzh_KeyUp);
            auto_whcd.KeyUp += new KeyEventHandler(auto_whcd_KeyUp);
            txt_jynx.KeyUp += new KeyEventHandler(txt_jynx_KeyUp);
            auto_csss.KeyUp += new KeyEventHandler(auto_csss_KeyUp);
            auto_csqx.KeyUp += new KeyEventHandler(auto_csqx_KeyUp);
            auto_jgss.KeyUp += new KeyEventHandler(auto_jgss_KeyUp);
            auto_jgqx.KeyUp += new KeyEventHandler(auto_jgqx_KeyUp);
            auto_brxz.KeyUp += new KeyEventHandler(auto_brxz_KeyUp);
            auto_zy.KeyUp += new KeyEventHandler(auto_zy_KeyUp);
            txt_gzdw.KeyUp += new KeyEventHandler(txt_gzdw_KeyUp);
            txt_dwdz.KeyUp += new KeyEventHandler(txt_dwdz_KeyUp);
            txt_dwdh.KeyUp += new KeyEventHandler(txt_dwdh_KeyUp);
            txt_dwyb.KeyUp += new KeyEventHandler(txt_dwyb_KeyUp);
            txt_hkdz.KeyUp += new KeyEventHandler(txt_hkdz_KeyUp);
            txt_hkdh.KeyUp += new KeyEventHandler(txt_hkdh_KeyUp);
            txt_hkyb.KeyUp += new KeyEventHandler(txt_hkyb_KeyUp);
            txt_dqdz.KeyUp += new KeyEventHandler(txt_dqdz_KeyUp);

            txt_lxrm.KeyUp += new KeyEventHandler(txt_lxrm_KeyUp);
            auto_lxgx.KeyUp += new KeyEventHandler(auto_lxgx_KeyUp);
            auto_lxrxb.KeyUp += new KeyEventHandler(auto_lxrxb_KeyUp);
            txt_lxjtdh.KeyUp += new KeyEventHandler(txt_lxjtdh_KeyUp);
            txt_lxgzdh.KeyUp += new KeyEventHandler(txt_lxgzdh_KeyUp);
            txt_lxyb.KeyUp += new KeyEventHandler(txt_lxyb_KeyUp);
            txt_lxdz.KeyUp += new KeyEventHandler(txt_lxdz_KeyUp);
            txt_lxdw.KeyUp += new KeyEventHandler(txt_lxdw_KeyUp);

            auto_ryzd.KeyUp += new KeyEventHandler(auto_ryzd_KeyUp);

            cmbJcxm.KeyUp += new KeyEventHandler(cmbJcxm_KeyUp);
            radNumericJcjg.KeyUp += new KeyEventHandler(radNumericJcjg_KeyUp);
            txtDw.KeyUp += new KeyEventHandler(txtDw_KeyUp);
            txtBz.KeyUp += new KeyEventHandler(txtBz_KeyUp);

            OKButton.KeyUp += new KeyEventHandler(OKButton_KeyUp);
        }
        #region       回车事件实现
        private void auto_hy_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                auto_mz.Focus();
        }
        private void auto_mz_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                auto_gj.Focus();
        }
        private void auto_gj_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                datePicker1.Focus();
        }
        private void datePicker1_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_nl.Focus();
        }
        private void txt_nl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                auto_xb.Focus();
        }
        private void auto_xb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_zjxy.Focus();
        }
        private void txt_zjxy_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_sfzh.Focus();
        }
        private void txt_sfzh_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                auto_whcd.Focus();
        }
        private void auto_whcd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_jynx.Focus();
        }
        private void txt_jynx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                auto_csss.Focus();
        }
        private void auto_csss_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                auto_csqx.Focus();
        }
        private void auto_csqx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                auto_jgss.Focus();
        }
        private void auto_jgss_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                auto_jgqx.Focus();
        }
        private void auto_jgqx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                auto_brxz.Focus();
        }
        private void auto_brxz_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                auto_zy.Focus();
        }
        private void auto_zy_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_gzdw.Focus();
        }
        private void txt_gzdw_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_dwdz.Focus();
        }
        private void txt_dwdz_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_dwdh.Focus();
        }
        private void txt_dwdh_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_dwyb.Focus();
        }
        private void txt_dwyb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_hkdz.Focus();
        }
        private void txt_hkdz_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_hkdh.Focus();
        }
        private void txt_hkdh_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_hkyb.Focus();
        }
        private void txt_hkyb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_dqdz.Focus();
        }
        private void txt_dqdz_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                OKButton.Focus();
        }

        private void OKButton_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                OKButton_Click(null, null);
        }

        private void txt_lxrm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                auto_lxgx.Focus();
        }
        private void auto_lxgx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                auto_lxrxb.Focus();
        }
        private void auto_lxrxb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_lxjtdh.Focus();
        }
        private void txt_lxjtdh_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_lxgzdh.Focus();
        }
        private void txt_lxgzdh_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_lxyb.Focus();
        }
        private void txt_lxyb_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_lxdz.Focus();
        }
        private void txt_lxdz_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_lxdw.Focus();
        }
        private void txt_lxdw_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                OKButton.Focus();
        }

        private void auto_ryzd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                OKButton.Focus();
        }

        private void cmbJcxm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                radNumericJcjg.Focus();
        }
        private void radNumericJcjg_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtDw.Focus();
        }
        private void txtDw_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtBz.Focus();
        }
        private void txtBz_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSave.Focus();
        }




        #endregion






        /// <summary>
        /// 绑定住院病人基本信息
        /// </summary>
        private void BindPatBasicInfo(CP_InPatient patInfo)
        {

            this.txt_xm.Text = m_CurrentPat.Hzxm;

            if (!string.IsNullOrEmpty(patInfo.Hyzk))
            {
                this.auto_hy.SelectedItem = modalDictionary.Where(hy => hy.Lbdm.Equals("4") && hy.Mxdm.Equals(patInfo.Hyzk)).FirstOrDefault();
            }


            if (!string.IsNullOrEmpty(patInfo.Mzdm))
            {
                this.auto_mz.SelectedItem = modalDictionary.Where(mz => mz.Lbdm.Equals("42") && mz.Mxdm.Equals(patInfo.Mzdm)).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(patInfo.Gjdm))
            {
                this.auto_gj.SelectedItem = modalDictionary.Where(gj => gj.Lbdm.Equals("43") && gj.Mxdm.Equals(patInfo.Gjdm)).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(patInfo.Brxb))
            {
                this.auto_xb.SelectedItem = modalDictionary.Where(sex => sex.Lbdm.Equals("3") && sex.Mxdm.Equals(patInfo.Brxb)).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(patInfo.Whcd))
            {
                this.auto_whcd.SelectedItem = modalDictionary.Where(whcd => whcd.Lbdm.Equals("25") && whcd.Mxdm.Equals(patInfo.Whcd)).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(patInfo.Ssdm))
            {
                this.auto_csss.SelectedItem = modalAreas.Where(csss => csss.Dqlb.Equals("1000") && csss.Dqdm.Equals(patInfo.Ssdm)).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(patInfo.Qxdm))
            {
                this.auto_csqx.SelectedItem = modalAreas.Where(csqx => csqx.Dqlb.Equals("1001") && csqx.Dqdm.Equals(patInfo.Qxdm)).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(patInfo.Jgssdm))
            {
                this.auto_jgss.SelectedItem = modalAreas.Where(jgss => jgss.Dqlb.Equals("1000") && jgss.Dqdm.Equals(patInfo.Jgssdm)).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(patInfo.Jgqxdm))
            {
                this.auto_jgqx.SelectedItem = modalAreas.Where(jgqx => jgqx.Dqlb.Equals("1001") && jgqx.Dqdm.Equals(patInfo.Jgqxdm)).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(patInfo.Brxz))
            {
                this.auto_brxz.SelectedItem = modalDictionary.Where(brxz => brxz.Lbdm.Equals("1") && brxz.Mxdm.Equals(patInfo.Brxz)).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(patInfo.Zydm))
            {
                this.auto_zy.SelectedItem = modalDictionary.Where(zy => zy.Lbdm.Equals("41") && zy.Mxdm.Equals(patInfo.Zydm)).FirstOrDefault();
            }

            if (!string.IsNullOrEmpty(patInfo.Ryzd))
            {
                this.auto_ryzd.SelectedItem = modalDiagnosis.Where(ryzd => ryzd.Zdbs.Equals(patInfo.Ryzd)).FirstOrDefault();
            }

            this.datePicker1.Text = string.IsNullOrEmpty(patInfo.Csrq) ? string.Empty : patInfo.Csrq;
            //this.txt_nl.Text = string.IsNullOrEmpty(patInfo.Xsnl) ? string.Empty : patInfo.Xsnl;

            this.txt_zjxy.Text = string.IsNullOrEmpty(patInfo.Zjxy) ? string.Empty : patInfo.Zjxy;
            this.txt_sfzh.Text = string.IsNullOrEmpty(patInfo.Sfzh) ? string.Empty : patInfo.Sfzh;

            this.txt_jynx.Text = string.IsNullOrEmpty(patInfo.Jynx.ToString()) ? string.Empty : patInfo.Jynx.ToString();


            this.txt_gzdw.Text = string.IsNullOrEmpty(patInfo.Gzdw) ? string.Empty : patInfo.Gzdw;
            this.txt_dwdz.Text = string.IsNullOrEmpty(patInfo.Dwdz) ? string.Empty : patInfo.Dwdz;
            this.txt_dwdh.Text = string.IsNullOrEmpty(patInfo.Dwdh) ? string.Empty : patInfo.Dwdh;
            this.txt_dwyb.Text = string.IsNullOrEmpty(patInfo.Dwyb) ? string.Empty : patInfo.Dwyb;
            this.txt_hkdz.Text = string.IsNullOrEmpty(patInfo.Hkdz) ? string.Empty : patInfo.Hkdz;
            this.txt_hkdh.Text = string.IsNullOrEmpty(patInfo.Hkdh) ? string.Empty : patInfo.Hkdh;
            this.txt_hkyb.Text = string.IsNullOrEmpty(patInfo.Hkyb) ? string.Empty : patInfo.Hkyb;
            this.txt_dqdz.Text = string.IsNullOrEmpty(patInfo.Dqdz) ? string.Empty : patInfo.Dqdz;
            this.txt_zyhm.Text = string.IsNullOrEmpty(patInfo.Zyhm) ? string.Empty : patInfo.Zyhm;
            this.txt_bahm.Text = string.IsNullOrEmpty(patInfo.Bahm) ? string.Empty : patInfo.Bahm;
            this.txt_mzh.Text = string.IsNullOrEmpty(patInfo.Mzhm) ? string.Empty : patInfo.Mzhm;
            if (!string.IsNullOrEmpty(patInfo.Brlx) && modalDictionary.Where(brlx => brlx.Lbdm.Equals("45") && brlx.Mxdm.Equals(patInfo.Brlx)).Count() > 0)
            {

                this.txt_brlx.Text = modalDictionary.Where(brlx => brlx.Lbdm.Equals("45") && brlx.Mxdm.Equals(patInfo.Brlx)).First().Name;
            }

            if (!string.IsNullOrEmpty(patInfo.Brly) && modalDictionary.Where(brly => brly.Lbdm.Equals("2") && brly.Mxdm.Equals(patInfo.Brly)).Count() > 0)
            {
                this.txt_brly.Text = modalDictionary.Where(brly => brly.Lbdm.Equals("2") && brly.Mxdm.Equals(patInfo.Brly)).First().Name;
            }

            if (!string.IsNullOrEmpty(patInfo.Mzzd) && modalDiagnosis.Where(mzzd => mzzd.Zddm.Equals(patInfo.Mzzd)).Count() > 0)
            {
                this.txt_mzzd.Text = modalDiagnosis.Where(mzzd => mzzd.Zddm.Equals(patInfo.Mzzd)).First().Name;
            }

            if (!string.IsNullOrEmpty(patInfo.Wzjb) && modalDictionary.Where(wzjb => wzjb.Lbdm.Equals("53") && wzjb.Mxdm.Equals(patInfo.Wzjb)).Count() > 0)
            {
                this.txt_wzjb.Text = modalDictionary.Where(wzjb => wzjb.Lbdm.Equals("53") && wzjb.Mxdm.Equals(patInfo.Wzjb)).First().Name;
            }

            if (!string.IsNullOrEmpty(patInfo.Ryqk) && modalDictionary.Where(ryqk => ryqk.Lbdm.Equals("5") && ryqk.Mxdm.Equals(patInfo.Ryqk)).Count() > 0)
            {
                this.txt_ryqk.Text = modalDictionary.Where(ryqk => ryqk.Lbdm.Equals("5") && ryqk.Mxdm.Equals(patInfo.Ryqk)).First().Name;
            }

            if (!string.IsNullOrEmpty(patInfo.Rytj) && modalDictionary.Where(rytj => rytj.Lbdm.Equals("6") && rytj.Mxdm.Equals(patInfo.Rytj)).Count() > 0)
            {
                this.txt_rytj.Text = modalDictionary.Where(rytj => rytj.Lbdm.Equals("6") && rytj.Mxdm.Equals(patInfo.Rytj)).First().Name;
            }

            this.txt_mzys.Text = patInfo.MzysName;
            this.txt_brzt.Text = patInfo.BrztName;
            this.txt_rycw.Text = string.IsNullOrEmpty(patInfo.Rycw) ? string.Empty : patInfo.Rycw;
            this.txt_ryks.Text = string.IsNullOrEmpty(patInfo.Ryks) ? string.Empty : patInfo.Ryks;
            this.txt_rybq.Text = string.IsNullOrEmpty(patInfo.Rybq) ? string.Empty : patInfo.Rybq;
            this.txt_rycs.Text = string.IsNullOrEmpty(patInfo.Rycs.ToString()) ? string.Empty : patInfo.Rycs.ToString();
            this.txt_ryrq.Text = string.IsNullOrEmpty(patInfo.Ryrq) ? string.Empty : patInfo.Ryrq;
            this.txt_rqrq.Text = string.IsNullOrEmpty(patInfo.Rqrq) ? string.Empty : patInfo.Rqrq;
            this.txt_cyks.Text = string.IsNullOrEmpty(patInfo.Cyks) ? string.Empty : patInfo.Cyks;
            this.txt_cybq.Text = string.IsNullOrEmpty(patInfo.Cybq) ? string.Empty : patInfo.Cybq;
            this.txt_cycw.Text = string.IsNullOrEmpty(patInfo.Cycw) ? string.Empty : patInfo.Cycw;
            this.txt_cyrq.Text = string.IsNullOrEmpty(patInfo.Cyrq) ? string.Empty : patInfo.Cyrq;
            this.txt_cqrq.Text = string.IsNullOrEmpty(patInfo.Cqrq) ? string.Empty : patInfo.Cqrq;

            this.txt_nl.Text = this.m_CurrentPat.Xsnl;
            //2013-05-10,WangGuojin
            m_sOld_Auto_Ryzd = auto_ryzd.Text.Trim();
            m_sNew_Auto_Ryzd = m_sOld_Auto_Ryzd;
            isRyzdChanged = false;
            if (m_iPage > 0)
            {
                this.auto_ryzd.Focus();
            }
        }

        /// <summary>
        /// 绑定第一联系人信息
        /// </summary>
        private void BindFirstPationContactsInfo()
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            //绑定第一联系人信息
            client.SelectFirstPationContactsInfoCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        List<Modal_PatientContactsInfo> modal_PatientContactsInfo = e.Result.ToList();
                        if (modal_PatientContactsInfo.Count != 0)
                        {
                            //modal_PatientContactsInfo.Where(contacts => contacts.Lxrbz.Equals(1));
                            modal_PatientContactsInfo = modal_PatientContactsInfo.Where(contacts => contacts.Lxrbz.Equals(1)).ToList();
                            //this.txt_lxdw.Text = string.IsNullOrEmpty(modal_PatientContactsInfo[0].Lxdw) ? string.Empty : modal_PatientContactsInfo[0].Lxdw;
                            this.txt_lxdw.Text = string.IsNullOrEmpty(modal_PatientContactsInfo[0].Lxdw) ? string.Empty : modal_PatientContactsInfo[0].Lxdw;
                            this.txt_lxdz.Text = string.IsNullOrEmpty(modal_PatientContactsInfo[0].Lxdz) ? string.Empty : modal_PatientContactsInfo[0].Lxdz;
                            if (modalDictionary.Where(lxgx => lxgx.Lbdm.Equals("44") && lxgx.Mxdm.Equals(modal_PatientContactsInfo[0].Lxgx)) == null)
                            {
                                this.auto_lxgx.Text = "";
                            }
                            else
                            {
                                this.auto_lxgx.SelectedItem = modalDictionary.Where(lxgx => lxgx.Lbdm.Equals("44") && lxgx.Mxdm.Equals(modal_PatientContactsInfo[0].Lxgx)).FirstOrDefault();
                            }

                            if (modalDictionary.Where(lxrxb => lxrxb.Lbdm.Equals("3") && lxrxb.Mxdm.Equals(modal_PatientContactsInfo[0].Lxrxb)) == null)
                            {
                                this.auto_lxrxb.Text = "";
                            }
                            else
                            {
                                this.auto_lxrxb.SelectedItem = modalDictionary.Where(lxrxb => lxrxb.Lbdm.Equals("3") && lxrxb.Mxdm.Equals(modal_PatientContactsInfo[0].Lxrxb)).FirstOrDefault();
                            }
                            this.txt_lxgzdh.Text = string.IsNullOrEmpty(modal_PatientContactsInfo[0].Lxgzdh) ? string.Empty : modal_PatientContactsInfo[0].Lxgzdh;
                            this.txt_lxjtdh.Text = string.IsNullOrEmpty(modal_PatientContactsInfo[0].Lxjtdh) ? string.Empty : modal_PatientContactsInfo[0].Lxjtdh;
                            this.txt_lxrm.Text = string.IsNullOrEmpty(modal_PatientContactsInfo[0].Lxrm) ? string.Empty : modal_PatientContactsInfo[0].Lxrm;

                            this.txt_lxyb.Text = string.IsNullOrEmpty(modal_PatientContactsInfo[0].Lxyb) ? string.Empty : modal_PatientContactsInfo[0].Lxyb;
                        }
                    }
                };
            client.SelectFirstPationContactsInfoAsync(Convert.ToDecimal(m_CurrentPat.Syxh));
        }

        //Modal_Dictionary过滤器
        public bool MyItemFilter(string text, object item)
        {
            Modal_Dictionary item_Dictionary = (Modal_Dictionary)item;

            return ((item_Dictionary.Mxdm.StartsWith(text)) || item_Dictionary.Mxdm.Contains(text)
                || (item_Dictionary.Name.StartsWith(text)) || item_Dictionary.Name.Contains(text)
                || (item_Dictionary.Py.StartsWith(text)) || item_Dictionary.Py.Contains(text)
                || (item_Dictionary.Wb.StartsWith(text)) || item_Dictionary.Wb.Contains(text));
        }

        //Modal_Areas过滤器
        public bool MyItemFilter1(string text, object item)
        {
            Modal_Areas item_Areas = (Modal_Areas)item;

            return ((item_Areas.Dqdm.StartsWith(text)) || item_Areas.Dqdm.Contains(text)
                || (item_Areas.Name.StartsWith(text)) || item_Areas.Name.Contains(text)
                || (item_Areas.Py.StartsWith(text)) || item_Areas.Py.Contains(text)
                || (item_Areas.Wb.StartsWith(text)) || item_Areas.Wb.Contains(text));
        }

        /// <summary>
        /// 获取病人检查项信息
        /// </summary>
        private void GetExamDictionaryDetail()
        {
            try
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                PE_ExamDictionaryDetail pedd = new PE_ExamDictionaryDetail();
                pedd.Bz = pedd.Flbm = pedd.Jcbm = pedd.Jcmc = pedd.Jsdw = pedd.Jsfw = pedd.Ksfw = pedd.Mcsx = pedd.Py = pedd.Wb = pedd.Yxjl = String.Empty;
                client.InsertAndSelectExamDictionaryDetailCompleted += (obj, ea) =>
                {
                    if (ea.Error == null)
                    {
                        if (ea.Result.ToList().Count != 0)
                        {
                            cmbJcxm.ItemsSource = ea.Result.ToList();
                            cmbJcxm.SelectedValuePath = "Jcbm";
                            cmbJcxm.DisplayMemberPath = "Jcmc";
                            cmbJcxm.SelectedIndex = 0;
                        }
                    }
                };
                client.InsertAndSelectExamDictionaryDetailAsync(pedd);
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
            radNumericJcjg.IsEnabled = txtDw.IsEnabled = txtBz.IsEnabled = btnSave.IsEnabled = bl1;
            this.btnAdd.IsEnabled = bl2;
            this.btnDel.IsEnabled = bl2;
            this.btnUpdate.IsEnabled = bl2;

            this.btnClear.IsEnabled = bl1;

            this.btnSave.IsEnabled = bl1;
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
                this.cmbJcxm.IsEnabled = true;
            }
            else if (m_funstate == EditState.Edit)
            {
                SetEnabled(true, false);
                //this.cmbJcxm.IsEnabled = false;
                this.cmbJcxm.IsEnabled = true;
            }
            else
            {
                SetEnabled(false, true);
                this.cmbJcxm.IsEnabled = false;
            }
        }

        /// <summary>
        /// 清空文本
        /// </summary>
        private void ClearTextBox()
        {
            txtID.Text = "自动生成";
            txtDw.Text = txtBz.Text = String.Empty;
            radNumericJcjg.Value = 0.00;
        }

        /// <summary>
        /// 绑定病人检查项信息
        /// </summary>
        private void GetPatientExamItem()
        {
            try
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                RW_PatientExamItem examItem = new RW_PatientExamItem();
                examItem.ID = examItem.Jcxm = examItem.Jcjg = examItem.Bz = examItem.Dw = String.Empty;
                examItem.Syxh = txtSyxh.Text;
                client.InsertAndSelectPatientExamItemCompleted += (obj, ea) =>
                    {
                        if (ea.Error == null)
                        {
                            patientExamItemList = ea.Result;
                            gvPatientExamItem.ItemsSource = patientExamItemList;
                        }
                    };
                client.InsertAndSelectPatientExamItemAsync(examItem);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #endregion

        #region 变量
        List<Modal_Dictionary> modalDictionary;
        List<Modal_Areas> modalAreas;
        List<Modal_Diagnosis> modalDiagnosis;
        List<Modal_PatientContactsInfo> modalPatientContactsInfo;
        EditState m_funstate;//按钮状态
        ObservableCollection<RW_PatientExamItem> patientExamItemList;//检查项信息集合
        RW_PatientExamItem rpe;

        /// <summary>
        /// 退出按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void auto_ryzd_LostFocus(object sender, RoutedEventArgs e)
        {
            if (auto_ryzd.SelectedItem == null)
            {
                this.auto_ryzd.Text = "";
            }
        }

        //检测项目change事件 add by luff 2012-09-24 
        private void cmbJcxm_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count > 0)
                {
                    PE_ExamDictionaryDetail patientExamItem = (PE_ExamDictionaryDetail)e.AddedItems[0];

                    this.txtDw.Text = patientExamItem.Jsdw.ToString();

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        //GrideView行集合
        #endregion
    }
}



