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
using YidanEHRApplication.Helpers;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Views.NursingNotes
{
    public partial class RWInPatientHistory
    {
        bool isTrue = true;
        #region 基础变量
        YidanEHRDataServiceClient serviceCon;

        CP_FamilyHistory m_Familyhistory = new CP_FamilyHistory();

        CP_AllergyHistory m_Allergyhistory = new CP_AllergyHistory();

        CP_SurgeryHistory m_Surgeryhistory = new CP_SurgeryHistory();

        CP_IllnessHistory m_Illnesshistory = new CP_IllnessHistory();

        List<CP_DataCategoryDetail> m_ComboxListSouce = new List<CP_DataCategoryDetail>();

        //个人史使用
        CP_PersonalHistory personal = new CP_PersonalHistory();
        List<Modal_Dictionary> modalDictionary;
        List<Modal_Areas> modalAreas;
        List<CP_PersonalHistory> cp_personal;


        /// <summary>
        /// 页面编辑状态
        /// </summary>
        EditState m_FamilyState;

        EditState m_AllergyState;

        EditState m_SurgeryState;

        EditState m_IllnessState;



        public static string Syxh;

        #endregion
        //public string Syxh = Global.InpatientListCurrent.Syxh == null ? "1" : Global.InpatientListCurrent.Syxh;

        #region 页面初始化

        public RWInPatientHistory()
        {
            InitializeComponent();
            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };
            
            //this.cbx_jzgx.Focus();
            //this.cbx_jzgx.IsDropDownOpen = true;
            //this.datePicker_csrq.DateTimeWatermarkContent = "选择日期...";
            this.datePicker_csrq.SelectedDate = DateTime.Now;
            this.cbx_sfjz.Text = "否";
            this.cbx_sfjz.SelectedItem = 0;
            txt_hzsl.Text = "1";
            this.cbxBfsj.SelectedDate = DateTime.Now;
            //cbxBfsj.DateTimeWatermarkContent = "选择日期...";
        }

        private void PageLoaded(object sender, RoutedEventArgs e)
        {
            if (!isTrue)
            {
                isTrue = true;
                return;
            }
            RegisterKeyEvent();
            PageStart();
        }

        /// <summary>
        /// 页面加载
        /// </summary>
        private void PageStart()
        {
            BindCombox();
            BindFamily();
        }

        #endregion

        #region 绑定下拉框信息
        /// <summary>
        /// 绑定页面下拉框
        /// </summary>
        private void BindCombox()
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetDataCategoryListCompleted +=
                 (obj, e) =>
                 {
                     if (e.Error == null)
                     {
                         List<CP_DataCategoryDetail> dataList = e.Result.ToList();

                         m_ComboxListSouce = e.Result.ToList();

                         //绑定过敏类型
                         cbxGmlx.ItemsSource = dataList.Where(u => u.Lbbh.Equals(60));

                         //绑定过敏程度
                         cbxGmcd.ItemsSource = dataList.Where(u => u.Lbbh.Equals(61));

                         cbx_jzgx.ItemsSource = dataList.Where(u => u.Lbbh.Equals(62));

                     }
                     else
                     {
                         PublicMethod.RadWaringBox(e.Error);
                     }
                 };

            serviceCon.GetDataCategoryListAsync(99999);
            serviceCon.CloseAsync();
        }


        /// <summary>
        /// 绑定病种代码
        /// </summary>
        /// <param name="KeyWord"></param>
        private void BindCP_Diagnosis(string KeyWord)
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetCP_DiagnosisListCompleted += (obj, e) =>
            {
                if (e.Error == null)
                {
                    List<CP_Diagnosis_E> diagList = e.Result.ToList();

                    if (this.radTabControlPathManager.SelectedIndex == 0)
                    {
                        ///////////////绑定家族史中的病中信息
                        this.cbx_Familybzdm.ItemsSource = new List<CP_Diagnosis_E>();
                        this.cbx_Familybzdm.ItemsSource = diagList;
                        this.cbx_Familybzdm.ItemFilter = DrugItemFilter;
                        //如果绑定的为一条数据时，直接将数据设置为选中
                        if (diagList.Count == 1)
                        {
                            this.cbx_Familybzdm.SelectedItem = diagList[0];
                        }
                        else
                        {
                            this.cbx_Familybzdm.SelectedItem = diagList.Where(u => u.Zdbs.Equals(m_Familyhistory.Bzdm)).FirstOrDefault();
                        }
                    }
                    else if (this.radTabControlPathManager.SelectedIndex == 3)
                    {
                        ///////////////绑定过敏史中的病中信息
                        this.cbxBzdm.ItemsSource = new List<CP_Diagnosis_E>();
                        this.cbxBzdm.ItemsSource = diagList;
                        this.cbxBzdm.ItemFilter = DrugItemFilter;
                        //如果绑定的为一条数据时，直接将数据设置为选中
                        if (diagList.Count == 1)
                        {
                            this.cbxBzdm.SelectedItem = diagList[0];
                        }
                        else
                        {
                            this.cbxBzdm.SelectedItem = diagList.Where(u => u.Zdbs.Equals(this.m_Surgeryhistory.Bzdm)).FirstOrDefault();
                            //this.cbx_Familybzdm.SelectedItem = diagList.Where(u => u.Zdbs.Equals(this.m_Surgeryhistory.Bzdm)).FirstOrDefault();
                        }
                    }
                    else if (this.radTabControlPathManager.SelectedIndex == 4)
                    {
                        /////////////////绑定疾病史中的病种信息
                        this.cbxIllnessBzmc.ItemsSource = new List<CP_Diagnosis_E>();
                        this.cbxIllnessBzmc.ItemsSource = diagList;
                        this.cbxIllnessBzmc.ItemFilter = DrugItemFilter;
                        //如果绑定的为一条数据时，直接将数据设置为选中
                        if (diagList.Count == 1)
                        {
                            this.cbxIllnessBzmc.SelectedItem = diagList[0];
                        }
                        else
                        {
                            this.cbxIllnessBzmc.SelectedItem = diagList.Where(u => u.Zdbs.Equals(this.m_Illnesshistory.Bzdm)).FirstOrDefault();
                            //this.cbx_Familybzdm.SelectedItem = diagList.Where(u => u.Zdbs.Equals(this.m_Illnesshistory.Bzdm)).FirstOrDefault();
                        }
                    }

                }
                else
                {
                    PublicMethod.RadWaringBox(e.Error);
                }
            };
            serviceCon.GetCP_DiagnosisListAsync(KeyWord);
            serviceCon.CloseAsync();
        }



        public bool DrugItemFilter(string text, object item)
        {
            CP_Diagnosis_E diag = (CP_Diagnosis_E)item;
            // Call it a match if the typed-in text appears in the product code
            // or at the beginning of the product name.
            return ((diag.Py.StartsWith(text)) || (diag.Py.Contains(text))
                  || (diag.Wb.StartsWith(text)) || (diag.Wb.Contains(text))
                  || (diag.Name.StartsWith(text)) || (diag.Name.Contains(text))
                  || (diag.Zdbs.StartsWith(text)) || (diag.Zdbs.Contains(text))
                  || (diag.Zddm.StartsWith(text)) || (diag.Zddm.Contains(text)));
        }

        /// <summary>
        /// 绑定病种代码
        /// </summary>
        /// <param name="KeyWord"></param>
        private void BindCP_Surgery(string KeyWord)
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetCP_SurgeryListByKeywordCompleted +=
             (obj, e) =>
             {
                 if (e.Error == null)
                 {
                     List<CP_Surgery> diagList = e.Result.ToList();

                     this.cbxSsdm.ItemsSource = new List<CP_Surgery>();
                     this.cbxSsdm.ItemsSource = diagList;
                     this.cbxSsdm.ItemFilter = DrugItemFilterSurgery;

                     if (diagList.Count == 1)
                     {
                         this.cbxSsdm.SelectedItem = diagList[0];
                     }

                 }
                 else
                 {
                     PublicMethod.RadWaringBox(e.Error);
                 }
             };
            serviceCon.GetCP_SurgeryListByKeywordAsync(KeyWord);
            serviceCon.CloseAsync();
        }



        public bool DrugItemFilterSurgery(string text, object item)
        {
            CP_Surgery diag = (CP_Surgery)item;
            // Call it a match if the typed-in text appears in the product code
            // or at the beginning of the product name.
            return ((diag.Py.StartsWith(text)) || (diag.Py.Contains(text))
                  || (diag.Wb.StartsWith(text)) || (diag.Wb.Contains(text))
                  || (diag.Name.StartsWith(text)) || (diag.Name.Contains(text))
                  || (diag.Ssdm.StartsWith(text)) || (diag.Ssdm.Contains(text))
                  || (diag.Ysdm.StartsWith(text)) || (diag.Ysdm.Contains(text))
                  || (diag.Bzdm.StartsWith(text)) || (diag.Bzdm.Contains(text)));
        }

        /// <summary>
        /// 病种信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxBzdm_TextChanged(object sender, RoutedEventArgs e)
        {
            //家族史页面
            if (this.radTabControlPathManager.SelectedIndex == 0)
            {
                if (cbx_Familybzdm.Text.Trim().Length == 1)
                {
                    BindCP_Diagnosis(cbx_Familybzdm.Text.Trim());
                }
            }
            //手术史页面
            else if (this.radTabControlPathManager.SelectedIndex == 3)
            {
                if (cbxBzdm.Text.Trim().Length == 1)
                {
                    BindCP_Diagnosis(cbxBzdm.Text.Trim());
                }
            }
            //疾病史页面
            else if (this.radTabControlPathManager.SelectedIndex == 4)
            {
                if (this.cbxIllnessBzmc.Text.Trim().Length == 1)
                {
                    BindCP_Diagnosis(cbxIllnessBzmc.Text.Trim());
                }
            }
        }

        private void cbxSsdm_TextChanged(object sender, RoutedEventArgs e)
        {
            if (cbxSsdm.Text.Trim().Length == 1)
            {
                BindCP_Surgery(cbxSsdm.Text.Trim());
            }
        }

        #endregion

        #region 选项卡切换

        /// <summary>
        /// 切换选项卡
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTabControlPathManager_SelectionChanged(object sender, RoutedEventArgs e)
        {

            try
            {
                //RadTabControl tabControl = (RadTabControl)sender;
                RadTabControl tabControl = (RadTabControl)sender;

                //当选项卡选择为第二个个人史时候
                if (tabControl.SelectedIndex == 1)
                {
                    AutoComplete();
                }
                //当选项卡选中第3个时候(过敏史)
                if (tabControl.SelectedIndex == 2)
                {
                    BindAllergy();
                }
                //手术史
                else if (tabControl.SelectedIndex == 3)
                {
                    BindSurgery();
                }
                //当前选择的为疾病史页面
                else if (tabControl.SelectedIndex == 4)
                {
                    BindIllness();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        #endregion

        #region 家族史

        /// <summary>
        /// 绑定家族史信息
        /// </summary>
        private void BindFamily()
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetCP_FamilyHistoryListCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        GridView_FamilyHistory.ItemsSource = e.Result.ToList();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            serviceCon.GetCP_FamilyHistoryListAsync(Syxh);

            serviceCon.CloseAsync();

            m_FamilyState = EditState.View;
            BindFamilyBtnState();
        }



        private void BindPatFamilyHistoryDetail(CP_FamilyHistory family)
        {
            if (family == null)
            {
                this.cbx_jzgx.Text = "";
                this.cbx_jzgx.SelectedItem = null;
                this.cbx_sfjz.Text = "否";
                this.cbx_sfjz.SelectedIndex = 0;
                //this.cbx_sfjz.SelectedItem = null;
                this.cbx_Familybzdm.Text = "";
                this.cbx_Familybzdm.SelectedItem = null;
                this.datePicker_csrq.SelectedDate = DateTime.Now;
                //this.datePicker_csrq.SelectedValue = null;
                this.txt_Swyy.Text = "";
            }
            else
            {
                cbx_jzgx.SelectedItem = m_ComboxListSouce.Where(u => u.Mxbh.Equals(family.Jzgx)).FirstOrDefault();



                //cbx_bzdm.SelectedItem = m_ComboxListBzdmSouce.Where(u => u.Zddm.Equals(family.Bzdm)).FirstOrDefault();
                BindCP_Diagnosis(family.Bzdm);


                if (family.Sfjz == 1)
                {
                    cbx_sfjz.SelectedIndex = 1;
                }
                else
                {
                    cbx_sfjz.SelectedIndex = 0;
                }
                this.txt_Swyy.Text = family.Swyy;
                this.datePicker_csrq.SelectedDate = Convert.ToDateTime(family.Csrq);

            }
        }

        private void BindFamilyBtnState()
        {
            if (m_FamilyState == EditState.Add)
            {
                this.cbx_jzgx.IsEnabled = true;
                this.cbx_sfjz.IsEnabled = true;
                this.cbx_Familybzdm.IsEnabled = true;

                this.txt_Swyy.IsEnabled = true;
                this.datePicker_csrq.IsEnabled = true;


                this.btn_FamilyAdd.IsEnabled = false;
                this.btn_FamilyDel.IsEnabled = false;
                this.btn_FamilyUpdate.IsEnabled = false;

                this.btn_FamilyCancel.IsEnabled = true;

                this.btn_FamilySave.IsEnabled = true;
                this.btn_FamilyTxtClear.IsEnabled = true;
            }
            else if (m_FamilyState == EditState.Edit)
            {
                this.cbx_jzgx.IsEnabled = true;
                this.cbx_sfjz.IsEnabled = true;
                this.cbx_Familybzdm.IsEnabled = true;

                this.txt_Swyy.IsEnabled = true;
                this.datePicker_csrq.IsEnabled = true;


                this.btn_FamilyAdd.IsEnabled = false;
                this.btn_FamilyDel.IsEnabled = false;
                this.btn_FamilyUpdate.IsEnabled = false;

                this.btn_FamilyCancel.IsEnabled = true;

                this.btn_FamilySave.IsEnabled = true;
                this.btn_FamilyTxtClear.IsEnabled = false;
            }
            else
            {
                this.cbx_jzgx.IsEnabled = false;
                this.cbx_sfjz.IsEnabled = false;
                this.cbx_Familybzdm.IsEnabled = false;

                this.txt_Swyy.IsEnabled = false;
                this.datePicker_csrq.IsEnabled = false;


                this.btn_FamilyAdd.IsEnabled = true;
                this.btn_FamilyDel.IsEnabled = true;
                this.btn_FamilyUpdate.IsEnabled = true;

                this.btn_FamilyCancel.IsEnabled = false;

                this.btn_FamilySave.IsEnabled = false;
                this.btn_FamilyTxtClear.IsEnabled = false;
            }
        }

        /// <summary>
        /// 将页面中过敏史信息存入到CP_FamilyHistory实体中
        /// </summary>
        /// <returns></returns>
        private CP_FamilyHistory SetFamily()
        {
            if (cbx_jzgx.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择家族关系!", "提示", cbx_jzgx);
                //this.Focus();
                isTrue = false;
                return null;
            }
            if (datePicker_csrq.SelectedValue == null)
            {
                PublicMethod.RadAlterBoxRe("病人家属出生日期不能为空!", "提示", datePicker_csrq); //this.Focus();
                //this.cbx_sfjz.IsDropDownOpen = false;
                isTrue = false;
                return null;
            }
            if (cbx_Familybzdm.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择病种类型!", "提示", cbx_Familybzdm); //this.Focus();
                isTrue = false;
                return null;
            }

            if (cbx_sfjz.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("病人家属是否健在还没选!", "提示", cbx_sfjz); //this.Focus();
                isTrue = false;
                return null;
            }

            if (txt_Swyy.Text.Length > 200)
            {
                PublicMethod.RadAlterBox("死亡原因长度超出", "提示"); //this.Focus();
                isTrue = false;
                return null;
            }
            CP_FamilyHistory family = new CP_FamilyHistory();

            family.Syxh = Convert.ToInt32(Syxh);
            family.Jzgx = ((CP_DataCategoryDetail)cbx_jzgx.SelectedItem).Mxbh;
            family.Bzdm = ((CP_Diagnosis_E)cbx_Familybzdm.SelectedItem).Zdbs.ToString();
            family.Csrq = ((DateTime)this.datePicker_csrq.SelectedDate).ToString("yyyy-MM-dd");
            family.Sfjz = cbx_sfjz.SelectedIndex;
           
            family.Swyy = this.txt_Swyy.Text.ToString().Trim();
            family.Memo = "";
            return family;
        }

        /// <summary>
        ///家族史删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>   
        private void btn_FamilyDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_Familyhistory == null || m_Familyhistory.ID == 0)
                {
                    PublicMethod.RadAlterBox("请选中要删除的行！", "提示");
                    //this.Focus();
                }
                else
                {
                    //DialogParameters parameters = new DialogParameters();
                    //parameters.Content = String.Format("{0}", "请问是否删除选中的家族信息？");
                    //parameters.Header = "提示";
                    //parameters.IconContent = null;
                    //parameters.OkButtonContent = "确定";
                    //parameters.CancelButtonContent = "取消";
                    //parameters.Closed = OnDelAdviceGroupDetail;//***close处理***
                    //RadWindow.Confirm(parameters);
                    //this.Focus();
                    YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的家族信息？", "提示", YiDanMessageBoxButtons.YesNo);
                    mess.ShowDialog();
                    mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);

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
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.OperateCP_FamilyHistoryCompleted +=
                    (obj, ea) =>
                    {
                        if (ea.Error == null)
                        {
                            SQLMessage mess = ea.Result;
                            if (mess.IsSucceed)
                            {
                                BindFamily();
                            }
                            PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                            //this.Focus();
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(ea.Error);
                        }
                    };
                    serviceCon.OperateCP_FamilyHistoryAsync(m_Familyhistory, "Delete");
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
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.OperateCP_FamilyHistoryCompleted +=
                    (obj, ea) =>
                    {
                        if (ea.Error == null)
                        {
                            SQLMessage mess = ea.Result;
                            if (mess.IsSucceed)
                            {
                                BindFamily();
                            }
                            PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                            //this.Focus();
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(ea.Error);
                        }
                    };
                    serviceCon.OperateCP_FamilyHistoryAsync(m_Familyhistory, "Delete");
                    serviceCon.CloseAsync();
                }
                catch (Exception ex)
                {
                    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            }
        }


        /// <summary>
        /// 家族史修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FamilyUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (m_Familyhistory == null || m_Familyhistory.ID == 0)
                {
                    PublicMethod.RadAlterBox("请选中要修改的行！", "提示");
                    //this.Focus();
                }
                else
                {
                    m_FamilyState = EditState.Edit;

                    BindPatFamilyHistoryDetail(m_Familyhistory);

                    BindFamilyBtnState();
                    this.cbx_jzgx.Focus();
                    //this.cbx_jzgx.IsDropDownOpen = true;
                }

               

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 家族史新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FamilyAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_FamilyState = EditState.Add;
                BindPatFamilyHistoryDetail(null);
                BindFamilyBtnState();
                this.cbx_jzgx.Focus();
                //this.cbx_jzgx.IsDropDownOpen = true;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }



        /// <summary>
        /// 家族史取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FamilyCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_FamilyState = EditState.View;

                BindPatFamilyHistoryDetail(null);

                BindFamilyBtnState();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void GridView_FamilyHistory_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            try
            {
                if (GridView_FamilyHistory.SelectedItem == null)
                {
                    m_Familyhistory = null;
                    return;
                }
                if (m_FamilyState == EditState.View)
                {
                    m_Familyhistory = (CP_FamilyHistory)GridView_FamilyHistory.SelectedItem;
                    BindPatFamilyHistoryDetail(m_Familyhistory);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void btn_FamilyTxtClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                BindPatFamilyHistoryDetail(null);
                cbx_jzgx.Focus();
                this.cbx_jzgx.IsDropDownOpen = true;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 家族史保存按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_FamilySave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                CP_FamilyHistory cp_family = SetFamily();
                if (cp_family == null)
                {
                    //this.cbx_sfjz.IsDropDownOpen = false;
                    return;
                }

                if (m_FamilyState == EditState.Add)
                {
                    cp_family.ID = 0;
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.OperateCP_FamilyHistoryCompleted +=

        (obj, ea) =>
        {
            if (ea.Error == null)
            {
                SQLMessage mess = ea.Result;
                if (mess.IsSucceed)
                {
                    BindFamily();
                }
                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                //this.Focus();
            }
            else
            {
                PublicMethod.RadWaringBox(ea.Error);
            }
        };
                    serviceCon.OperateCP_FamilyHistoryAsync(cp_family, "Insert");
                    serviceCon.CloseAsync();
                    //this.Focus();
                }
                else
                {
                    cp_family.ID = m_Familyhistory.ID;
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.OperateCP_FamilyHistoryCompleted +=

        (obj, ea) =>
        {
            if (ea.Error == null)
            {
                SQLMessage mess = ea.Result;
                if (mess.IsSucceed)
                {
                    BindFamily();
                }
                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");

            }
            else
            {
                PublicMethod.RadWaringBox(ea.Error);
                //this.Focus();
            }
        };
                    serviceCon.OperateCP_FamilyHistoryAsync(cp_family, "Update");
                    serviceCon.CloseAsync();
                    //this.Focus();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 选择是否健在时判断是否可以输入死亡原因
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbx_sfjz_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (cbx_sfjz.SelectedIndex == 1)
                {
                    this.txt_Swyy.Text = "";
                    this.txt_Swyy.IsEnabled = false;
                }
                else if (cbx_sfjz.SelectedIndex == 0)
                {
                    if (m_FamilyState == EditState.Add || m_FamilyState == EditState.Edit)
                    {
                        this.txt_Swyy.IsEnabled = true;
                    }
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        #endregion

        #region  过敏史


        /// <summary>
        /// 绑定过敏史信息
        /// </summary>
        private void BindAllergy()
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetCP_AllergyHistoryListCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        GridView_AllergyHistory.ItemsSource = e.Result.ToList();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            serviceCon.GetCP_AllergyHistoryListAsync(Syxh);
            serviceCon.CloseAsync();

            m_AllergyState = EditState.View;
            BindAllergyBtnState();
        }



        private void BindAllergyDetail(CP_AllergyHistory allergy)
        {
            if (allergy == null)
            {
                this.cbxGmcd.Text = "";
                this.cbxGmcd.SelectedItem = null;
                this.cbxGmlx.Text = "";
                this.cbxGmlx.SelectedItem = null;

                this.txtDlys.Text = "";
                this.txtFylx.Text = "";
                this.txtGmbw.Text = "";
            }

            else
            {

                List<CP_DataCategoryDetail> li = m_ComboxListSouce.Where(u => u.Mxbh == Convert.ToInt16(allergy.Gmlx)).ToList();
                //li = m_ComboxListSouce.Where(u => u.Mxbh.Equals(6103)).ToList();
                if (li.Count > 0)
                {
                    cbxGmlx.SelectedItem = li[0];
                }

                //this.cbxGmlx.Text = allergy.Gmlx_Name;
                li = m_ComboxListSouce.Where(u => u.Mxbh == Convert.ToInt16(allergy.Gmcd)).ToList();
                if (li.Count > 0)
                {
                    cbxGmcd.SelectedItem = li[0];
                }
                this.txtDlys.Text = allergy.Dlys;
                this.txtFylx.Text = allergy.Fylx;
                this.txtGmbw.Text = allergy.Gmbw;
            }
        }

        private void BindAllergyBtnState()
        {
            if (m_AllergyState == EditState.Add)
            {
                this.cbxGmcd.IsEnabled = true;
                this.cbxGmlx.IsEnabled = true;

                this.txtDlys.IsEnabled = true;
                this.txtFylx.IsEnabled = true;
                this.txtGmbw.IsEnabled = true;


                this.btnAllergyAdd.IsEnabled = false;
                this.btnAllergyDel.IsEnabled = false;
                this.btnAllergyUpdate.IsEnabled = false;

                this.btnAllergyClear.IsEnabled = true;

                this.btnAllergySave.IsEnabled = true;
                this.btnAllergyTxtClear.IsEnabled = true;
            }
            else if (m_AllergyState == EditState.Edit)
            {
                this.cbxGmcd.IsEnabled = true;
                this.cbxGmlx.IsEnabled = true;

                this.txtDlys.IsEnabled = true;
                this.txtFylx.IsEnabled = true;
                this.txtGmbw.IsEnabled = true;


                this.btnAllergyAdd.IsEnabled = false;
                this.btnAllergyDel.IsEnabled = false;
                this.btnAllergyUpdate.IsEnabled = false;

                this.btnAllergyClear.IsEnabled = true;

                this.btnAllergySave.IsEnabled = true;
                this.btnAllergyTxtClear.IsEnabled = false;
            }
            else
            {
                this.cbxGmcd.IsEnabled = false;
                this.cbxGmlx.IsEnabled = false;

                this.txtDlys.IsEnabled = false;
                this.txtFylx.IsEnabled = false;
                this.txtGmbw.IsEnabled = false;


                this.btnAllergyAdd.IsEnabled = true;
                this.btnAllergyDel.IsEnabled = true;
                this.btnAllergyUpdate.IsEnabled = true;

                this.btnAllergyClear.IsEnabled = false;

                this.btnAllergySave.IsEnabled = false;
                this.btnAllergyTxtClear.IsEnabled = false;
            }
        }

        /// <summary>
        /// 将页面中过敏史信息存入到CP_AllergyHistory实体中
        /// </summary>
        /// <returns></returns>
        private CP_AllergyHistory SetAllergy()
        {
            if (cbxGmlx.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择过敏类型!", "提示", cbxGmlx);
                isTrue = false;//this.Focus();
                return null;
            }

            if (cbxGmcd.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择过敏程度!", "提示", cbxGmcd);
                isTrue = false;//this.Focus();
                return null;
            }
           
            //if (txtDlys.Text.Length == 0)
            //{
            //    PublicMethod.RadAlterBoxRe("代理医生不能为空!", "提示", txtDlys);
            //    isTrue = false;//this.Focus();
            //    return null;
            //}
            if (txtDlys.Text.Length >= 30)
            {
                PublicMethod.RadAlterBoxRe("代理医生输入长度超出!", "提示", txtDlys);
                isTrue = false;//this.Focus();
                return null;
            }
            if (txtGmbw.Text.Length == 0)
            {
                PublicMethod.RadAlterBoxRe("过敏部位不能为空!", "提示", txtGmbw);
                isTrue = false;//this.Focus();
                return null;
            }
            if (txtGmbw.Text.Length >= 60)
            {
                PublicMethod.RadAlterBoxRe("过敏部位输入长度超出!", "提示", txtGmbw);
                isTrue = false;//this.Focus();
                return null;
            }
            //if (txtFylx.Text.Length == 0)
            //{
            //    PublicMethod.RadAlterBoxRe("反应类型不能为空!", "提示", txtFylx);
            //    isTrue = false;//this.Focus();
            //    return null;
            //}
            if (txtFylx.Text.Length >= 100)
            {
                PublicMethod.RadAlterBoxRe("反应类型输入长度超出!", "提示", txtFylx);
                isTrue = false;//this.Focus();
                return null;
            }
            CP_AllergyHistory allergy = new CP_AllergyHistory();

            allergy.Syxh = Syxh;
            allergy.Gmlx = ((CP_DataCategoryDetail)cbxGmlx.SelectedItem).Mxbh.ToString();
            allergy.Gmcd = ((CP_DataCategoryDetail)cbxGmcd.SelectedItem).Mxbh.ToString();
            allergy.Dlys = this.txtDlys.Text.ToString().Trim();
            allergy.Gmbw = this.txtGmbw.Text.ToString().Trim();
            allergy.Fylx = this.txtFylx.Text.ToString().Trim();
            allergy.Memo = "";
            return allergy;
        }

        /// <summary>
        /// 过敏史删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllergyDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_Allergyhistory == null || m_Allergyhistory.ID == null)
                {
                    PublicMethod.RadAlterBox("请选中要删除的行！", "提示"); //this.Focus();
                }
                else
                {
                    //DialogParameters parameters = new DialogParameters();
                    //parameters.Content = String.Format("提示: {0}", "请问是否删除选中的过敏信息？");
                    //parameters.Header = "提示";
                    //parameters.IconContent = null;
                    //parameters.OkButtonContent = "确定";
                    //parameters.CancelButtonContent = "取消";
                    //parameters.Closed = OnDelAllergyDetail;//***close处理***
                    //RadWindow.Confirm(parameters);
                    // this.Focus();
                    YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的过敏信息？", "提示", YiDanMessageBoxButtons.YesNo);
                    mess.ShowDialog();
                    mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent1);

                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        void mess_PageClosedEvent1(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    try
                    {
                        serviceCon = PublicMethod.YidanClient;
                        serviceCon.OperCP_AllergyHistoryCompleted +=

            (obj, ea) =>
            {
                if (ea.Error == null)
                {
                    SQLMessage mess = ea.Result;
                    if (mess.IsSucceed)
                    {
                        BindAllergy();
                    }
                    PublicMethod.RadAlterBox(mess.Message.ToString(), "提示"); //this.Focus();
                }
                else
                {
                    PublicMethod.RadWaringBox(ea.Error);
                }
            };
                        serviceCon.OperCP_AllergyHistoryAsync(m_Allergyhistory, "Delete");
                        serviceCon.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        PublicMethod.ClientException(ex, this.GetType().FullName, true);
                    }

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void OnDelAllergyDetail(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                try
                {
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.OperCP_AllergyHistoryCompleted +=

        (obj, ea) =>
        {
            if (ea.Error == null)
            {
                SQLMessage mess = ea.Result;
                if (mess.IsSucceed)
                {
                    BindAllergy();
                }
                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示"); //this.Focus();
            }
            else
            {
                PublicMethod.RadWaringBox(ea.Error);
            }
        };
                    serviceCon.OperCP_AllergyHistoryAsync(m_Allergyhistory, "Delete");
                    serviceCon.CloseAsync();
                }
                catch (Exception ex)
                {
                    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            }
        }

        /// <summary>
        /// 过敏史修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllergyUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_Allergyhistory == null || m_Allergyhistory.ID == null)
                {
                    PublicMethod.RadAlterBox("请选中要修改的行！", "提示"); //this.Focus();
                }
                else
                {
                    m_AllergyState = EditState.Edit;

                    BindAllergyDetail(m_Allergyhistory);

                    BindAllergyBtnState();
                    cbxGmlx.Focus();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 过敏史新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllergyAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                m_AllergyState = EditState.Add;
                BindAllergyDetail(null);
                BindAllergyBtnState();
                this.cbxGmlx.Focus();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }



        /// <summary>
        /// 过敏史取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAllergyClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_AllergyState = EditState.View;

                BindAllergyDetail(null);

                BindAllergyBtnState();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void GridView_AllergyHistory_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                if (GridView_AllergyHistory.SelectedItem == null)
                {
                    m_Allergyhistory = null;
                    return;
                }
                if (m_AllergyState == EditState.View)
                {
                    m_Allergyhistory = (CP_AllergyHistory)GridView_AllergyHistory.SelectedItem;
                    BindAllergyDetail(m_Allergyhistory);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void btnAllergySave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CP_AllergyHistory cp_allergy = SetAllergy();
                if (cp_allergy == null)
                {
                    return;
                }

                if (m_AllergyState == EditState.Add)
                {
                    cp_allergy.ID = "0";
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.OperCP_AllergyHistoryCompleted +=

        (obj, ea) =>
        {
            if (ea.Error == null)
            {
                SQLMessage mess = ea.Result;

                if (mess.IsSucceed)
                {
                    BindAllergy();
                }
                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                //this.Focus();
            }
            else
            {
                PublicMethod.RadWaringBox(ea.Error);
            }
        };
                    serviceCon.OperCP_AllergyHistoryAsync(cp_allergy, "Insert");
                    serviceCon.CloseAsync();
                }
                else
                {
                    cp_allergy.ID = m_Allergyhistory.ID;
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.OperCP_AllergyHistoryCompleted +=
        (obj, ea) =>
        {
            if (ea.Error == null)
            {
                SQLMessage mess = ea.Result;
                if (mess.IsSucceed)
                {
                    BindAllergy();
                }
                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示"); //this.Focus();
            }
            else
            {
                PublicMethod.RadWaringBox(ea.Error);
            }
        };
                    serviceCon.OperCP_AllergyHistoryAsync(cp_allergy, "Update");
                    serviceCon.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnAllergyTxtClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                BindAllergyDetail(null);
                this.cbxGmlx.Focus();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #endregion

        #region 手术史

        /// <summary>
        /// 绑定手术史信息
        /// </summary>
        private void BindSurgery()
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetCP_SurgeryHistoryListCompleted +=
                  (obj, e) =>
                  {
                      if (e.Error == null)
                      {
                          GridView_SurgeryHistory.ItemsSource = e.Result.ToList();
                      }
                      else
                      {
                          PublicMethod.RadWaringBox(e.Error);
                      }
                  };
            serviceCon.GetCP_SurgeryHistoryListAsync(Syxh);
            serviceCon.CloseAsync();

            m_SurgeryState = EditState.View;
            BindSurgeryBtnState();
        }



        private void BindSurgeryDetail(CP_SurgeryHistory Surgery)
        {
            if (Surgery == null)
            {
                this.cbxSsdm.SelectedItem = null;
                this.cbxSsdm.Text = "";
                this.cbxBzdm.SelectedItem = null;
                this.cbxBzdm.Text = "";

                this.txtSspl.Text = "";
                this.txtSsys.Text = "";
            }

            else
            {

                //this.cbxSsdm.SelectedItem = null;
                BindCP_Surgery(Surgery.Ssdm);

                //this.cbxBzdm.SelectedItem = null;
                BindCP_Diagnosis(Surgery.Bzdm);

                this.txtSspl.Text = Surgery.Sspl;
                this.txtSsys.Text = Surgery.Ssys;

            }
        }

        private void BindSurgeryBtnState()
        {
            if (m_SurgeryState == EditState.Add)
            {
                this.cbxSsdm.IsEnabled = true;
                this.cbxBzdm.IsEnabled = true;

                this.txtSsys.IsEnabled = true;
                this.txtSspl.IsEnabled = true;



                this.btnSurgeryAdd.IsEnabled = false;
                this.btnSurgeryDel.IsEnabled = false;
                this.btnSurgeryUpdate.IsEnabled = false;

                this.btnSurgeryClear.IsEnabled = true;

                this.btnSurgerySave.IsEnabled = true;
                this.btnSurgeryTxtClear.IsEnabled = true;
            }
            else if (m_SurgeryState == EditState.Edit)
            {
                this.cbxSsdm.IsEnabled = true;
                this.cbxBzdm.IsEnabled = true;

                this.txtSsys.IsEnabled = true;
                this.txtSspl.IsEnabled = true;


                this.btnSurgeryAdd.IsEnabled = false;
                this.btnSurgeryDel.IsEnabled = false;
                this.btnSurgeryUpdate.IsEnabled = false;

                this.btnSurgeryClear.IsEnabled = true;

                this.btnSurgerySave.IsEnabled = true;
                this.btnSurgeryTxtClear.IsEnabled = false;
            }
            else
            {
                this.cbxSsdm.IsEnabled = false;
                this.cbxBzdm.IsEnabled = false;

                this.txtSsys.IsEnabled = false;
                this.txtSspl.IsEnabled = false;


                this.btnSurgeryAdd.IsEnabled = true;
                this.btnSurgeryDel.IsEnabled = true;
                this.btnSurgeryUpdate.IsEnabled = true;

                this.btnSurgeryClear.IsEnabled = false;

                this.btnSurgerySave.IsEnabled = false;
                this.btnSurgeryTxtClear.IsEnabled = false;
            }
        }

        /// <summary>
        /// 将页面中手术史信息存入到CP_SurgeryHistory实体中
        /// </summary>
        /// <returns></returns>
        private CP_SurgeryHistory SetSurgery()
        {
            if (cbxSsdm.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择手术名称!", "提示",cbxSsdm);
                this.isTrue = false;//this.Focus();
                return null;
            }
            if (txtSsys.Text.Trim().Length == 0)
            {
                PublicMethod.RadAlterBoxRe("请输入手术医生姓名!", "提示", txtSsys);
                this.isTrue = false;//this.Focus();
                return null;
            }
            
            if (cbxBzdm.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择病种名称!", "提示", cbxBzdm);
                this.isTrue = false;// this.Focus();
                return null;
            }
            if (txtSspl.Text.Length > 200)
            {
                PublicMethod.RadAlterBoxRe("手术评论长度超出!", "提示", txtSspl);
                this.isTrue = false;// this.Focus();
                return null;
            }
            CP_SurgeryHistory Surgery = new CP_SurgeryHistory();

            Surgery.Syxh = Syxh;
            Surgery.Ssdm = ((CP_Surgery)cbxSsdm.SelectedItem).Ssdm.ToString();
            Surgery.Bzdm = ((CP_Diagnosis_E)cbxBzdm.SelectedItem).Zdbs.ToString();
            Surgery.Sspl = this.txtSspl.Text.ToString().Trim();
            Surgery.Ssys = this.txtSsys.Text.ToString().Trim();
            Surgery.Memo = "";
            return Surgery;
        }

        /// <summary>
        /// 手术史删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSurgeryDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_Surgeryhistory == null || m_Surgeryhistory.ID == null)
                {
                    PublicMethod.RadAlterBox("请选中要删除的行！", "提示");// this.Focus();
                }
                else
                {
                    //DialogParameters parameters = new DialogParameters();
                    //parameters.Content = String.Format("提示: {0}", "请问是否删除选中的手术信息？");
                    //parameters.Header = "提示";
                    //parameters.IconContent = null;
                    //parameters.OkButtonContent = "确定";
                    //parameters.CancelButtonContent = "取消";
                    //parameters.Closed = OnDelSurgeryDetail;//***close处理***
                    //RadWindow.Confirm(parameters);
                    // this.Focus();
                    YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的手术信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                    mess.ShowDialog();
                    mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent2);

                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        void mess_PageClosedEvent2(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.OperCP_SurgeryHistoryCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                SQLMessage mess = ea.Result;
                                if (mess.IsSucceed)
                                {
                                    BindSurgery();
                                }
                                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示"); //this.Focus();
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.OperCP_SurgeryHistoryAsync(m_Surgeryhistory, "Delete");
                    serviceCon.CloseAsync();

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        void OnDelSurgeryDetail(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                try
                {
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.OperCP_SurgeryHistoryCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                SQLMessage mess = ea.Result;
                                if (mess.IsSucceed)
                                {
                                    BindSurgery();
                                }
                                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示"); //this.Focus();
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.OperCP_SurgeryHistoryAsync(m_Surgeryhistory, "Delete");
                    serviceCon.CloseAsync();
                }
                catch (Exception ex)
                {
                    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            }
        }

        /// <summary>
        /// 手术史修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSurgeryUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (GridView_SurgeryHistory.SelectedItem==null)
                //{

                //}
                if (m_Surgeryhistory == null || m_Surgeryhistory.ID == null || GridView_SurgeryHistory.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选中要修改的行！", "提示"); //this.Focus();
                }
                else
                {
                    m_SurgeryState = EditState.Edit;

                    BindSurgeryDetail(m_Surgeryhistory);

                    BindSurgeryBtnState();

                    cbxSsdm.Focus();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 手术史新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSurgeryAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_SurgeryState = EditState.Add;
                BindSurgeryDetail(null);
                BindSurgeryBtnState();
                cbxSsdm.Focus();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }




        /// <summary>
        /// 手术史取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSurgeryClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_SurgeryState = EditState.View;

                BindSurgeryDetail(null);

                BindSurgeryBtnState();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void GridView_SurgeryHistory_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                if (GridView_SurgeryHistory.SelectedItem == null)
                {
                    m_Surgeryhistory = null;
                    return;
                }
                if (m_SurgeryState == EditState.View)
                {
                    m_Surgeryhistory = (CP_SurgeryHistory)GridView_SurgeryHistory.SelectedItem;
                    BindSurgeryDetail(m_Surgeryhistory);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void btnSurgerySave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CP_SurgeryHistory cp_Surgery = SetSurgery();
                if (cp_Surgery == null)
                {
                    return;
                }

                if (m_SurgeryState == EditState.Add)
                {
                    cp_Surgery.ID = "0";
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.OperCP_SurgeryHistoryCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                SQLMessage mess = ea.Result;
                                if (mess.IsSucceed)
                                {
                                    BindSurgery();
                                }
                                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");// this.Focus();
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.OperCP_SurgeryHistoryAsync(cp_Surgery, "Insert");
                    serviceCon.CloseAsync();
                }
                else
                {
                    cp_Surgery.ID = m_Surgeryhistory.ID;
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.OperCP_SurgeryHistoryCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                SQLMessage mess = ea.Result;
                                if (mess.IsSucceed)
                                {
                                    BindSurgery();
                                }
                                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");// this.Focus();
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.OperCP_SurgeryHistoryAsync(cp_Surgery, "Update");
                    serviceCon.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnSurgeryTxtClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                BindSurgeryDetail(null);
                this.cbxSsdm.Focus();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }



        #endregion

        #region  疾病史


        /// <summary>
        /// 绑定疾病史信息
        /// </summary>
        private void BindIllness()
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetCP_IllnessHistoryListCompleted +=
        (obj, e) =>
        {
            if (e.Error == null)
            {
                GridView_IllnessHistory.ItemsSource = e.Result.ToList();
            }
            else
            {
                PublicMethod.RadWaringBox(e.Error);
            }
        };
            serviceCon.GetCP_IllnessHistoryListAsync(Syxh);
            serviceCon.CloseAsync();

            m_IllnessState = EditState.View;
            BindIllnessBtnState();
        }



        private void BindIllnessDetail(CP_IllnessHistory illness)
        {
            if (illness == null)
            {

                this.cbxIllnessBzmc.SelectedItem = null;
                this.cbxIllnessBzmc.Text = "";
                this.cbxBfsj.SelectedValue = null;

                this.rbtnIllessNo.IsChecked = false;
                this.rbtnIllessYes.IsChecked = false;
                this.txtJbpl.Text = "";

            }

            else
            {

                this.cbxIllnessBzmc.SelectedItem = null;
                BindCP_Diagnosis(illness.Bzdm);

                this.rbtnIllessYes.IsChecked = false;
                this.rbtnIllessNo.IsChecked = false;
                if (illness.Sfzy == "是")
                {
                    this.rbtnIllessYes.IsChecked = true;
                }
                else
                {
                    this.rbtnIllessNo.IsChecked = true;
                }

                this.cbxBfsj.SelectedDate = Convert.ToDateTime(illness.Bfsj);
                this.txtJbpl.Text = illness.Jbpl;

            }
        }

        private void BindIllnessBtnState()
        {
            if (m_IllnessState == EditState.Add)
            {
                this.cbxIllnessBzmc.IsEnabled = true;
                this.cbxBfsj.IsEnabled = true;

                this.rbtnIllessNo.IsEnabled = true;
                this.rbtnIllessYes.IsEnabled = true;
                this.txtJbpl.IsEnabled = true;



                this.btnIllnessAdd.IsEnabled = false;
                this.btnIllnessDel.IsEnabled = false;
                this.btnIllnessUpdate.IsEnabled = false;

                this.btnIllnessClear.IsEnabled = true;

                this.btnIllnessSave.IsEnabled = true;
                this.btnIllnessTxtClear.IsEnabled = true;
            }
            else if (m_IllnessState == EditState.Edit)
            {
                this.cbxIllnessBzmc.IsEnabled = true;
                this.cbxBfsj.IsEnabled = true;

                this.rbtnIllessNo.IsEnabled = true;
                this.rbtnIllessYes.IsEnabled = true;
                this.txtJbpl.IsEnabled = true;


                this.btnIllnessAdd.IsEnabled = false;
                this.btnIllnessDel.IsEnabled = false;
                this.btnIllnessUpdate.IsEnabled = false;

                this.btnIllnessClear.IsEnabled = true;

                this.btnIllnessSave.IsEnabled = true;
                this.btnIllnessTxtClear.IsEnabled = false;
            }
            else
            {
                this.cbxIllnessBzmc.IsEnabled = false;
                this.cbxBfsj.IsEnabled = false;

                this.rbtnIllessNo.IsEnabled = false;
                this.rbtnIllessYes.IsEnabled = false;
                this.txtJbpl.IsEnabled = false;


                this.btnIllnessAdd.IsEnabled = true;
                this.btnIllnessDel.IsEnabled = true;
                this.btnIllnessUpdate.IsEnabled = true;

                this.btnIllnessClear.IsEnabled = false;

                this.btnIllnessSave.IsEnabled = false;
                this.btnIllnessTxtClear.IsEnabled = false;
            }
        }

        /// <summary>
        /// 将页面中疾病史信息存入到CP_IllnessHistory实体中
        /// </summary>
        /// <returns></returns>
        private CP_IllnessHistory SetIllness()
        {
            if (cbxIllnessBzmc.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择病种名称!", "提示", cbxIllnessBzmc);
                isTrue = false;//this.Focus();
                return null;
            }

            if (this.cbxBfsj.SelectedDate == null)
            {
                PublicMethod.RadAlterBoxRe("请选择病发时间!", "提示", cbxBfsj);
                isTrue = false;//this.Focus();
                return null;
            }

            if (this.rbtnIllessYes.IsChecked == false && this.rbtnIllessNo.IsChecked == false)
            {
                PublicMethod.RadAlterBoxRe("请选择是否治愈!", "提示", rbtnIllessYes);
                isTrue = false;//this.Focus();
                return null;
            }

            if (txtJbpl.Text.Length > 100)
            {
                PublicMethod.RadAlterBoxRe("疾病评论长度超出!", "提示", txtJbpl);
                isTrue = false;//this.Focus();
                return null;
            }
            CP_IllnessHistory illness = new CP_IllnessHistory();

            illness.Syxh = Syxh;
            illness.Bzdm = ((CP_Diagnosis_E)this.cbxIllnessBzmc.SelectedItem).Zdbs.ToString();
            illness.Bfsj = ((DateTime)this.cbxBfsj.SelectedDate).ToString("yyyy-MM-dd");
            illness.Jbpl = this.txtJbpl.Text.ToString().Trim();
            illness.Sfzy = this.rbtnIllessYes.IsChecked == true ? "1" : "0";

            illness.Memo = "";
            return illness;
        }

        /// <summary>
        /// 疾病史删除按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIllnessDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_Illnesshistory == null || m_Illnesshistory.ID == null)
                {
                    PublicMethod.RadAlterBox("请选中要删除的行！", "提示"); //this.Focus();
                }
                else
                {
                    //DialogParameters parameters = new DialogParameters();
                    //parameters.Content = String.Format("{0}", "请问是否删除选中的疾病信息？");
                    //parameters.Header = "提示";
                    //parameters.IconContent = null;
                    //parameters.OkButtonContent = "确定";
                    //parameters.CancelButtonContent = "取消";
                    //parameters.Closed = OnDelIllnessDetail;//***close处理***
                    //RadWindow.Confirm(parameters);
                    //this.Focus();
                    YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的疾病信息？", "提示", YiDanMessageBoxButtons.YesNo);
                    mess.ShowDialog();
                    mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent4);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void mess_PageClosedEvent4(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.OperCP_IllnessHistoryCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    SQLMessage mess = ea.Result;
                                    if (mess.IsSucceed)
                                    {
                                        BindIllness();
                                    }
                                    PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");//this.Focus();
                                }
                                else
                                {
                                    PublicMethod.RadWaringBox(ea.Error);
                                }
                            };
                    serviceCon.OperCP_IllnessHistoryAsync(m_Illnesshistory, "Delete");
                    serviceCon.CloseAsync();

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void OnDelIllnessDetail(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                try
                {
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.OperCP_IllnessHistoryCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    SQLMessage mess = ea.Result;
                                    if (mess.IsSucceed)
                                    {
                                        BindIllness();
                                    }
                                    PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");//this.Focus();
                                }
                                else
                                {
                                    PublicMethod.RadWaringBox(ea.Error);
                                }
                            };
                    serviceCon.OperCP_IllnessHistoryAsync(m_Illnesshistory, "Delete");
                    serviceCon.CloseAsync();
                }
                catch (Exception ex)
                {
                    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            }
        }

        /// <summary>
        /// 疾病史修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIllnessUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_Illnesshistory == null || m_Illnesshistory.ID == null || GridView_IllnessHistory.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选中要修改的行！", "提示"); //this.Focus();
                }
                else
                {
                    m_IllnessState = EditState.Edit;

                    BindIllnessDetail(m_Illnesshistory);

                    BindIllnessBtnState();
                    cbxIllnessBzmc.Focus();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 疾病史新增按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIllnessAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                m_IllnessState = EditState.Add;
                BindIllnessDetail(null);
                BindIllnessBtnState();
                cbxIllnessBzmc.Focus();

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }




        /// <summary>
        /// 疾病史取消按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnIllnessClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_IllnessState = EditState.View;

                BindIllnessDetail(null);

                BindIllnessBtnState();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void GridView_IllnessHistory_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                if (GridView_IllnessHistory.SelectedItem == null)
                {
                    m_Illnesshistory = null;
                    return;
                }
                if (m_IllnessState == EditState.View)
                {
                    m_Illnesshistory = (CP_IllnessHistory)GridView_IllnessHistory.SelectedItem;
                    BindIllnessDetail(m_Illnesshistory);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void btnIllnessSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CP_IllnessHistory cp_Illness = SetIllness();
                if (cp_Illness == null)
                {
                    return;
                }

                if (m_IllnessState == EditState.Add)
                {
                    cp_Illness.ID = "0";
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.OperCP_IllnessHistoryCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    SQLMessage mess = ea.Result;
                                    if (mess.IsSucceed)
                                    {
                                        BindIllness();
                                    }
                                    PublicMethod.RadAlterBox(mess.Message.ToString(), "提示"); //this.Focus();
                                }
                                else
                                {
                                    PublicMethod.RadWaringBox(ea.Error);
                                }
                            };
                    serviceCon.OperCP_IllnessHistoryAsync(cp_Illness, "Insert");
                    serviceCon.CloseAsync();
                }
                else
                {
                    cp_Illness.ID = m_Illnesshistory.ID;
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.OperCP_IllnessHistoryCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    SQLMessage mess = ea.Result;
                                    if (mess.IsSucceed)
                                    {
                                        BindIllness();
                                    }
                                    PublicMethod.RadAlterBox(mess.Message.ToString(), "提示"); //this.Focus();
                                }
                                else
                                {
                                    PublicMethod.RadWaringBox(ea.Error);
                                }
                            };
                    serviceCon.OperCP_IllnessHistoryAsync(cp_Illness, "Update");
                    serviceCon.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnIllnessTxtClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                BindIllnessDetail(null);
                this.cbxIllnessBzmc.Focus();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #endregion

        #region 个人史


        private void AutoComplete()
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.SelectPatInfoCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {

                        Modal_PatientInfo patInfo = e.Result;
                        modalDictionary = patInfo.CommonDictionary.ToList();
                        modalAreas = patInfo.Areas.ToList();
                        if (modalAreas.Count != 0)
                        {
                            autocbx_csd.ItemsSource = modalAreas.Where(csd => csd.Dqlb.Equals("1001"));
                            autocbx_csd.ItemFilter = MyItemFilter1;

                            autocbx_jld.ItemsSource = modalAreas.Where(jld => jld.Dqlb.Equals("1001"));
                            autocbx_jld.ItemFilter = MyItemFilter1;

                            autocbx_hyzk.ItemsSource = modalDictionary.Where(dictionary => dictionary.Lbdm.Equals("4"));

                            //autocbx_hyzk.ItemFilter = MyItemFilter;
                        }

                        BindPersonalInfo();
                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                };
            serviceCon.SelectPatInfoAsync(Syxh.ToString());
            serviceCon.CloseAsync();
        }

        private void BindPersonalInfo()
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetCP_PersonalHistoryListCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        cp_personal = e.Result.ToList();
                        if (cp_personal.Count > 0)
                        {
                            //autocbx_csd.SelectedItem = (autocbx_csd.ItemsSource as List<Modal_Areas>).Where(csd => csd.Dqlb.Equals(1001) && csd.Dqdm.Equals(cp_personal[0].Csd));
                            autocbx_csd.SelectedItem = modalAreas.Where(csd => csd.Dqlb.Equals("1001") && csd.Dqdm.Equals(cp_personal[0].Csd)).FirstOrDefault();
                            autocbx_jld.SelectedItem = modalAreas.Where(jld => jld.Dqlb.Equals("1001") && jld.Dqdm.Equals(cp_personal[0].Jld)).FirstOrDefault();
                            this.autocbx_hyzk.SelectedItem = modalDictionary.Where(hy => hy.Lbdm.Equals("4") && hy.Mxdm.Equals(cp_personal[0].Hyzk.Trim())).FirstOrDefault();
                            this.txt_hzsl.Text = cp_personal[0].Hzsl.ToString();
                            if (personal.Sfyj == 0)
                            {
                                cbx_sfyj.SelectedIndex = 0;
                            }
                            else
                            {
                                cbx_sfyj.SelectedIndex = 1;
                            }

                            if (personal.Sfxy == 0)
                            {
                                cbx_sfxy.SelectedIndex = 0;
                            }
                            else
                            {
                                cbx_sfxy.SelectedIndex = 1;
                            }

                            lstbx_xy.Text = cp_personal[0].Xys;
                            lstbx_yj.Text = cp_personal[0].Yjs;
                            lstbx_gz.Text = cp_personal[0].Zymc;
                        }

                    }
                    else
                    {
                        throw new NotImplementedException();
                    }
                };
            serviceCon.GetCP_PersonalHistoryListAsync(Syxh);
            serviceCon.CloseAsync();
        }





        //Modal_Dictionary过滤器
        public bool MyItemFilter(string text, object item)
        {
            Modal_Dictionary item_Dictionary = (Modal_Dictionary)item;

            return ((item_Dictionary.Mxdm.ToUpper().StartsWith(text.ToUpper())) || item_Dictionary.Mxdm.ToUpper().Contains(text.ToUpper())
                || (item_Dictionary.Name.ToUpper().StartsWith(text.ToUpper())) || item_Dictionary.Name.ToUpper().Contains(text.ToUpper())
                || (item_Dictionary.Py.ToUpper().StartsWith(text.ToUpper())) || item_Dictionary.Py.ToUpper().Contains(text.ToUpper())
                || (item_Dictionary.Wb.ToUpper().StartsWith(text.ToUpper())) || item_Dictionary.Wb.ToUpper().Contains(text.ToUpper()));
        }

        //Modal_Areas过滤器
        public bool MyItemFilter1(string text, object item)
        {
            Modal_Areas item_Areas = (Modal_Areas)item;

            return ((item_Areas.Dqdm.ToUpper().StartsWith(text.ToUpper())) || item_Areas.Dqdm.ToUpper().Contains(text.ToUpper())
                || (item_Areas.Name.ToUpper().StartsWith(text.ToUpper())) || item_Areas.Name.ToUpper().Contains(text.ToUpper())
                || (item_Areas.Py.ToUpper().StartsWith(text.ToUpper())) || item_Areas.Py.ToUpper().Contains(text.ToUpper())
                || (item_Areas.Wb.ToUpper().StartsWith(text.ToUpper())) || item_Areas.Wb.ToUpper().Contains(text.ToUpper()));
        }

        private void btn_personalSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                try
                {
                    if (int.Parse(txt_hzsl.Text) < 0)
                    {
                        //PublicMethod.RadAlterBox("孩子数目有误!", "提示");
                        PublicMethod.RadAlterBoxRe("孩子数目有误!", "提示", this.txt_hzsl);
                        isTrue = false;
                        return;
                    }
                }
                catch
                {
                   
                    PublicMethod.RadAlterBoxRe("孩子数目有误!", "提示", this.txt_hzsl);
                    isTrue = false;
                    throw;
                }

                CP_PersonalHistory person = SetPerson();
                if (person == null)
                {
                    return;
                }
                else
                {


                    person.ID = personal.ID;
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.OperateCP_PersonalHistoryCompleted +=
                         (obj, ea) =>
                         {
                             if (ea.Error == null)
                             {

                                 PublicMethod.RadAlterBox("保存成功!", "提示"); //this.Focus();
                             }
                             else
                             {
                                 PublicMethod.RadWaringBox(ea.Error);
                             }
                         };
                    serviceCon.OperateCP_PersonalHistoryAsync(person, "Update");
                    serviceCon.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }



        private CP_PersonalHistory SetPerson()
        {


            if (this.autocbx_csd.SelectedItem == null)
            {
                //PublicMethod.RadAlterBox();
                PublicMethod.RadAlterBoxRe("请选择出生地!", "提示", this.autocbx_csd);
                isTrue = false;
                //this.Focus();
                return null;
            }

            if (this.autocbx_jld.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择经历地", "提示", autocbx_jld); //this.Focus();
                isTrue = false;
                return null;
            }

            if (this.autocbx_hyzk.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择婚姻状况!", "提示", autocbx_hyzk); //this.Focus();
                isTrue = false;
                return null;
            }

            if (this.txt_hzsl.Text.Trim() == "")
            {
                PublicMethod.RadAlterBoxRe("请输入孩子数量", "提示", txt_hzsl); //this.Focus();
                isTrue = false;
                return null;
            }

            if (this.cbx_sfyj.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择病人是否饮酒", "提示", cbx_sfyj); //this.Focus();
                isTrue = false;
                return null;
            }

            if (this.cbx_sfxy.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择病人是否吸烟", "提示", cbx_sfxy); //this.Focus();
                isTrue = false;
                return null;
            }
            if (this.lstbx_xy.Text.Length > 250)
            {
                PublicMethod.RadAlterBoxRe("吸烟补充说明长度超出", "提示", lstbx_xy);
                isTrue = false;//this.Focus();
                return null;
            }
            if (this.lstbx_yj.Text.Length > 250)
            {
                PublicMethod.RadAlterBoxRe("饮酒补充说明长度超出", "提示", lstbx_yj);
                isTrue = false;//this.Focus();
                return null;
            }
            if (this.lstbx_gz.Text.Length > 250)
            {
                PublicMethod.RadAlterBoxRe("职业经历情况长度超出", "提示", lstbx_gz);
                isTrue = false;//this.Focus();
                return null;
            }
            CP_PersonalHistory person1 = new CP_PersonalHistory();

            person1.Syxh = Convert.ToInt32(Syxh);
            person1.Csd = ((Modal_Areas)this.autocbx_csd.SelectedItem).Dqdm;
            person1.Jld = ((Modal_Areas)this.autocbx_jld.SelectedItem).Dqdm;
            person1.Hyzk = ((Modal_Dictionary)this.autocbx_hyzk.SelectedItem).Mxdm;
            person1.Hzsl = Convert.ToInt32(this.txt_hzsl.Text);
            person1.Sfyj = this.cbx_sfyj.SelectedIndex;
            person1.Sfxy = this.cbx_sfxy.SelectedIndex;
            person1.Zymc = this.lstbx_gz.Text.ToString();
            person1.Yjs = this.lstbx_yj.Text.ToString();
            person1.Xys = this.lstbx_xy.Text.ToString();
            person1.Memo = "";
            return person1;
        }

        #endregion

        private void btn_personalCancel_Click(object sender, RoutedEventArgs e)
        {
            autocbx_csd.Text = string.Empty;
            autocbx_jld.Text = string.Empty;
            //autocbx_hyzk.Text = string.Empty;
            //点击重置的时候将婚姻状况清空
            //修改时间：2013年8月13日 12:02:28
            //修改人：Jhonny
            autocbx_hyzk.SelectedIndex = -1;
            txt_hzsl.Text = string.Empty;

            cbx_sfyj.Text = string.Empty;
            cbx_sfxy.Text = string.Empty;
            lstbx_yj.Text = string.Empty;
            lstbx_xy.Text = string.Empty;
            lstbx_gz.Text = string.Empty;
        }



        private void RegisterKeyEvent()
        {

            cbx_jzgx.KeyUp += new KeyEventHandler(cbx_jzgx_KeyUp);
            datePicker_csrq.KeyUp += new KeyEventHandler(datePicker_csrq_KeyUp);
            cbx_Familybzdm.KeyUp += new KeyEventHandler(cbx_Familybzdm_KeyUp);
            cbx_sfjz.KeyUp += new KeyEventHandler(cbx_sfjz_KeyUp);
            txt_Swyy.KeyUp += new KeyEventHandler(txt_Swyy_KeyUp);

            autocbx_csd.KeyUp += new KeyEventHandler(autocbx_csd_KeyUp);
            autocbx_jld.KeyUp += new KeyEventHandler(autocbx_jld_KeyUp);
            autocbx_hyzk.KeyUp += new KeyEventHandler(autocbx_hyzk_KeyUp);
            txt_hzsl.KeyUp += new KeyEventHandler(txt_hzsl_KeyUp);
            cbx_sfyj.KeyUp += new KeyEventHandler(cbx_sfyj_KeyUp);
            cbx_sfxy.KeyUp += new KeyEventHandler(cbx_sfxy_KeyUp);
            lstbx_yj.KeyUp += new KeyEventHandler(lstbx_yj_KeyUp);
            lstbx_xy.KeyUp += new KeyEventHandler(lstbx_xy_KeyUp);
            lstbx_gz.KeyUp += new KeyEventHandler(lstbx_gz_KeyUp);

            cbxGmlx.KeyUp += new KeyEventHandler(cbxGmlx_KeyUp);
            cbxGmcd.KeyUp += new KeyEventHandler(cbxGmcd_KeyUp);
            txtDlys.KeyUp += new KeyEventHandler(txtDlys_KeyUp);
            txtGmbw.KeyUp += new KeyEventHandler(txtGmbw_KeyUp);
            txtFylx.KeyUp += new KeyEventHandler(txtFylx_KeyUp);

            cbxSsdm.KeyUp += new KeyEventHandler(cbxSsdm_KeyUp);
            cbxBzdm.KeyUp += new KeyEventHandler(cbxBzdm_KeyUp);
            txtSsys.KeyUp += new KeyEventHandler(txtSsys_KeyUp);
            txtSspl.KeyUp += new KeyEventHandler(txtSspl_KeyUp);

            cbxIllnessBzmc.KeyUp += new KeyEventHandler(cbxIllnessBzmc_KeyUp);
            cbxBfsj.KeyUp += new KeyEventHandler(cbxBfsj_KeyUp);
            rbtnIllessYes.KeyUp += new KeyEventHandler(rbtnIllessYes_KeyUp);
            txtJbpl.KeyUp += new KeyEventHandler(txtJbpl_KeyUp);
            //lstbx_gz.KeyUp += new KeyEventHandler(lstbx_gz_KeyUp);

            btn_FamilySave.KeyUp += new KeyEventHandler(btn_FamilySave_KeyUp);
            btn_personalSave.KeyUp += new KeyEventHandler(btn_personalSave_KeyUp);
            btnAllergySave.KeyUp += new KeyEventHandler(btnAllergySave_KeyUp);
            btnSurgerySave.KeyUp += new KeyEventHandler(btnSurgerySave_KeyUp);
            btnIllnessSave.KeyUp += new KeyEventHandler(btnIllnessSave_KeyUp);
        }
        private void cbx_jzgx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                datePicker_csrq.Focus();
           
            
        }
        private void datePicker_csrq_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbx_Familybzdm.Focus();
            this.cbx_sfjz.IsDropDownOpen = false;
        }
        private void cbx_Familybzdm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbx_sfjz.Focus();
        }
        private void cbx_sfjz_KeyUp(object sender, KeyEventArgs e)
        {
            if (cbx_sfjz.SelectedIndex == 0)
                txt_Swyy.Focus();
            else
                btn_FamilySave.Focus();
        }
        private void txt_Swyy_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_FamilySave.Focus();
        }

        private void btn_FamilySave_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_FamilySave_Click(null, null);
        }

        private void autocbx_csd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                autocbx_jld.Focus();
        }
        private void autocbx_jld_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                autocbx_hyzk.Focus();
        }
        private void autocbx_hyzk_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txt_hzsl.Focus();
        }
        private void txt_hzsl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbx_sfyj.Focus();
        }
        private void cbx_sfyj_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbx_sfxy.Focus();
        }
        private void cbx_sfxy_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                lstbx_yj.Focus();
        }
        private void lstbx_yj_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                lstbx_xy.Focus();
        }
        private void lstbx_xy_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                lstbx_gz.Focus();
        }
        private void lstbx_gz_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_personalSave.Focus();
        }


        private void btn_personalSave_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btn_personalSave_Click(null, null);
        }

        private void cbxGmlx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbxGmcd.Focus();
        }
        private void cbxGmcd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtDlys.Focus();
        }
        private void txtDlys_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtGmbw.Focus();
        }
        private void txtGmbw_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtFylx.Focus();
        }
        private void txtFylx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnAllergySave.Focus();
        }

        private void btnAllergySave_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnAllergySave_Click(null, null);
        }


        private void cbxSsdm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbxBzdm.Focus();
        }
        private void cbxBzdm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtSsys.Focus();
        }
        private void txtSsys_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtSspl.Focus();
        }
        private void txtSspl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSurgerySave.Focus();
        }

        private void btnSurgerySave_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSurgerySave_Click(null, null);
        }


        private void cbxIllnessBzmc_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbxBfsj.Focus();
        }
        private void cbxBfsj_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                rbtnIllessYes.Focus();
                rbtnIllessYes.IsChecked = true;
            }
        }
        private void rbtnIllessYes_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtJbpl.Focus();
        }
        private void txtJbpl_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnIllnessSave.Focus();
        }

        private void btnIllnessSave_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnIllnessSave_Click(null, null);
        }


        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

    }
}

