﻿using DrectSoft.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Views.UserControls
{
    public partial class UCCyXDF : UserControl
    {

        private const string m_strTitle = "医嘱提示"; //定义弹出框标题栏

        private ManualType m_ManualType = ManualType.New;
        /// <summary>
        /// 操作类型
        /// </summary>
        public ManualType ManualType
        {
            get
            {
                return m_ManualType;
            }
            set
            {
                m_ManualType = value;
            }
        }

        #region 药品变量
        List<int> zdm = new List<int>();  //存储周代码，存储当前选中的星期
        List<string> zxsj = new List<string>();//存储执行时间，存储当前选中的时间
        string Strzdm = string.Empty;    //zdm字符串形式
        string Strzxsj = string.Empty; //zxsj字符串形式
        ObservableCollection<CP_AdviceGroupDetail> cP_AdviceGroupDetailCollection = new ObservableCollection<CP_AdviceGroupDetail>();
        List<CP_PCSJ> cplist = new List<CP_PCSJ>();
        #endregion

        #region 药品属性
        CP_DoctorOrder _cp_AdviceGroupDetail = new CP_DoctorOrder();

        public CP_DoctorOrder CP_AdviceGroupDetailProptery
        {
            get
            {
                return _cp_AdviceGroupDetail;
            }
            set
            {
                _cp_AdviceGroupDetail = value;
            }
        }
        #endregion

        private OrderItemCategory m_OrderCategory = OrderItemCategory.CyOrder;
        /// <summary>
        /// 项目初始化型类
        /// </summary>
        public OrderItemCategory OrderCategory
        {
            get
            {
                return m_OrderCategory;
            }
            set
            {
                m_OrderCategory = value;
            }
        }

        private OrderPanelBarCategory m_PanelCategory = OrderPanelBarCategory.CyOrder;
        /// <summary>
        /// 项目类别初始化型类
        /// </summary>
        public OrderPanelBarCategory PanelCategory
        {
            get
            {
                return m_PanelCategory;
            }
            set
            {
                m_PanelCategory = value;
            }
        }

        public UCCyXDF()
        {
            InitializeComponent();
        }
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                OnAfterDrugLoadedEvent(e);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void InitPage()
        {
            InitDrugInfo();//初始化药品信息//项目下拉框

            IntiComboBoxDept();
            //add by luff 20130313 获得配置表关于医嘱可选不算变异参数 若值为1表示可选，0表示必须
            //List<APPCFG> t_listApp = Global.mAppCfg.Select(s => s).Where(s => s.Configkey.IndexOf("Yziskx") > -1).ToList();
            //if (t_listApp.Count > 0)
            //{
            //    if (t_listApp[0].Value == "1")
            //    {
            //        txtisby.Visibility = Visibility.Visible;
            //        radkx.Visibility = Visibility.Visible;
            //        radbx.Visibility = Visibility.Visible;
            //        this.radkx.IsChecked = true;
            //        this.radbx.IsChecked = false;
            //    }
            //    else
            //    {
            //        txtisby.Visibility = Visibility.Collapsed;
            //        radkx.Visibility = Visibility.Collapsed;
            //        radbx.Visibility = Visibility.Collapsed;
            //        this.radbx.IsChecked = true;
            //        this.radkx.IsEnabled = false;
            //    }
            //}
            //else
            //{
            //    txtisby.Visibility = Visibility.Collapsed;
            //    radkx.Visibility = Visibility.Collapsed;
            //    radbx.Visibility = Visibility.Collapsed;
            //    this.radbx.IsChecked = true;
            //    this.radkx.IsEnabled = false;
            //}

            #region 判断his是否支持计价类型
            //try
            //{
            //    YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
            //    referenceClient.GetAppConifgTypeCompleted +=
            //        (obj, e) =>
            //        {
            //            if (e.Error == null && e.Result > -1)
            //            {
            this.txtjjlx.Visibility = Visibility.Visible;
            this.cbxJJLX.Visibility = Visibility.Visible;

            InitJJTypeInfo(cbxJJLX);
            cbxJJLX.SelectedIndex = 0;

            //            }
            //            else
            //            {
            //                this.txtjjlx.Visibility = Visibility.Collapsed;
            //                this.cbxJJLX.Visibility = Visibility.Collapsed;

            //                PublicMethod.RadWaringBox(e.Error);
            //            }
            //        };
            //    referenceClient.GetAppConifgTypeAsync("HisJjlx");
            //    referenceClient.CloseAsync();

            //}
            //catch (Exception ex)
            //{
            //    PublicMethod.ClientException(ex, this.GetType().FullName, true);
            //}

            #endregion

        }

        /// <summary>
        /// 初始化计费类型数据 add by luff 20130118
        /// </summary>
        /// <param name="radcombobox"></param>
        private void InitJJTypeInfo(RadComboBox radcombobox)
        {
            radcombobox.EmptyText = "请选择计价类型";
            List<OrderTypeName> iList = new List<OrderTypeName>();
            iList.Add(new OrderTypeName("正常计价", 1));
            iList.Add(new OrderTypeName("自带药", 2));
            iList.Add(new OrderTypeName("不计价", 3));
            radcombobox.ItemsSource = iList;
            radcombobox.SelectedIndex = 0;
            //autoCompleteBoxOrder.Focus();

        }

        #region 设定计价类型类别
        public class JjTypeName
        {
            public string JjlxName
            {
                get;
                set;
            }
            public short JjlxValue
            {
                get;
                set;
            }
            public JjTypeName(string jjlxName, short jjlxValue)
            {
                JjlxName = jjlxName;
                JjlxValue = jjlxValue;
            }
        }
        #endregion
        /// <summary>
        /// 初始化草药处方和处方明细药品数据 GetCyfUnionInfo
        /// </summary>
        private void InitDrugInfo()
        {
            YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
            Client.GetCyfUnionInfoCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        autoCompleteBoxOrder.ItemsSource = e.Result;
                        autoCompleteBoxOrder.ItemFilter = OrderFilter;
                        if (this.ManualType == Helpers.ManualType.Edit)
                            InitModifyOrder();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                        return;
                    }
                };
            Client.GetCyfUnionInfoAsync();
            Client.CloseAsync();
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

        public bool OrderFilter(string strFilter, object item)
        {
            CP_CYFUnion deptList = (CP_CYFUnion)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())));
        }


        #region 频次后面的周代码和时间
        private void CheckBoxWeek_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cplist = (List<CP_PCSJ>)cbxSJ.ItemsSource;

                for (int i = 0; i < cplist.Count; i++)
                {
                    CheckBox ck = sender as CheckBox;
                    if (cplist[i].DwFlag != "Week") // Week为数据库AS出来的值
                    {
                        ck.IsEnabled = false;
                    }
                    else
                    {
                        for (int j = 0; j < cplist[i].Zdm.Length; j++)
                        {

                            if (ck.Tag.ToString() == cplist[i].Zdm.Substring(j, 1).ToString())
                            {
                                ck.IsChecked = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        private void CheckBoxWeek_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox ck = sender as CheckBox;
            zdm.Add(Convert.ToInt32(ck.Tag.ToString()));
        }

        private void CheckBoxWeek_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox ck = sender as CheckBox;

            if (zdm.Count != 0)
            {
                if (zdm.Contains(Convert.ToInt32(ck.Tag.ToString())))
                {
                    zdm.Remove(Convert.ToInt32(ck.Tag.ToString()));
                }
            }
        }

        private void CheckBoxTime_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cplist = (List<CP_PCSJ>)cbxSJ.ItemsSource;
                for (int i = 0; i < cplist.Count; i++)
                {
                    CheckBox ck = sender as CheckBox;
                    if (cplist[i].DwFlag == "Hour" || cplist[i].DwFlag == "Minutes")  // Hour，Minutes为数据库AS出来的值
                    {
                        ck.IsEnabled = false;
                    }
                    else
                    {
                        for (int j = 0; j < cplist[i].Zxsj.Split(',').Length - 1; j++)
                        {
                            if (ck.Content.ToString() == cplist[i].Zxsj.Split(',')[j].ToString())
                            {
                                ck.IsChecked = true;
                                if (!zxsj.Contains(ck.Content.ToString() + ","))
                                {
                                    zxsj.Add(ck.Content.ToString() + ",");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void CheckBoxTime_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox ck = sender as CheckBox;
            if (!zxsj.Contains(ck.Content.ToString() + ","))
            {
                zxsj.Add(ck.Content.ToString() + ",");
            }
        }

        private void CheckBoxTime_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox ck = sender as CheckBox;
            if (zxsj.Count != 0)
            {
                if (zxsj.Contains(ck.Content.ToString() + ","))
                {
                    zxsj.Remove(ck.Content.ToString() + ",");
                }
            }
        }
        #endregion

        private List<CyDrugUnitsType> m_ListDrugUnit = new List<CyDrugUnitsType>();
        private void InitUnitType(string strName, decimal UnitIDCy)
        {
            CyDrugUnitsType type = new CyDrugUnitsType(strName, UnitIDCy);
            m_ListDrugUnit.Add(type);
        }



        private void autoCompleteBoxOrder_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (autoCompleteBoxOrder.SelectedItem != null)
                {
                    YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                    StringBuilder strItems = new StringBuilder();
                    if (((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).yplh != "草药协定方")
                    {
                        #region 单位
                        //药品医嘱时的单位填充数据为规格单位和最小单位
                        m_ListDrugUnit = new List<CyDrugUnitsType>();
                        InitUnitType(((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Ggdw.ToString(), 3006);
                        InitUnitType(((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Zxdw.ToString(), 3007);
                        cbxMDDW.ItemsSource = null;
                        cbxMDDW.ItemsSource = m_ListDrugUnit;
                        if (m_ListDrugUnit.Count > 0)
                            cbxMDDW.SelectedIndex = 0;
                        #region 暂时保留
                        //strItems.Append(((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Ggdw.ToString());
                        //strItems.Append(",");
                        //strItems.Append(((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Zxdw.ToString());
                        //cbxMDDW.Items.Clear();
                        //for (int i = 0; i < strItems.ToString().Split(',').Length; i++)
                        //{
                        //    RadComboBoxItem item = new RadComboBoxItem();
                        //    if (!string.IsNullOrEmpty(strItems.ToString().Split(',')[i].ToString()))
                        //    {
                        //        item.Content = strItems.ToString().Split(',')[i].ToString();
                        //        if (i == 0)
                        //        {
                        //            item.IsSelected = true;
                        //        }
                        //        cbxMDDW.Items.Add(item);
                        //    }
                        //}
                        #endregion
                        #endregion
                    }
                    else
                    {
                        #region 单位
                        //药品医嘱时的单位填充数据为规格单位和最小单位
                        m_ListDrugUnit = new List<CyDrugUnitsType>();
                        InitUnitType("付", 3006);
                        InitUnitType("付", 3007);
                        cbxMDDW.ItemsSource = null;
                        cbxMDDW.ItemsSource = m_ListDrugUnit;
                        if (m_ListDrugUnit.Count > 0)
                            cbxMDDW.SelectedIndex = 0;

                        #endregion
                    }

                    #region 数量
                    nudMDSL.Value = 1;
                    #endregion

                    #region 用法
                    Client.GetDrugUseageCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                cbxMDYF.ItemsSource = ea.Result;
                                //cbxMDYF.SelectedValue = _cp_AdviceGroupDetail.Yfdm;
                                if (ea.Result.Count > 0)
                                {
                                    cbxMDYF.SelectedIndex = 0;
                                    if (_cp_AdviceGroupDetail.Yfdm != null)
                                    {
                                        cbxMDYF.SelectedValue = _cp_AdviceGroupDetail.Yfdm;
                                        //cbxMDYF.SelectedItem = ((ObservableCollection<CP_DrugUseage>)cbxMDYF.ItemsSource).First(where => where.Yfdm.Equals(_cp_AdviceGroupDetail.Yfdm));   //7.25 序列不包含在元素中     ZM
                                    }
                                }
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    Client.GetDrugUseageAsync(((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Jxdm);
                    #endregion

                    #region 频次代码
                    //if (cbxPC.Items.Count == 0)
                    //{
                    Client.GetAdviceFrequencyCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                cbxPC.ItemsSource = ea.Result;
                                //cbxPC.SelectedValue = _cp_AdviceGroupDetail.Pcdm;
                                if (ea.Result.Count > 0)
                                {
                                    cbxPC.SelectedIndex = 0;
                                    if (_cp_AdviceGroupDetail.Pcdm != null)
                                    {
                                        cbxPC.SelectedItem = ((ObservableCollection<CP_AdviceFrequency>)cbxPC.ItemsSource).First(where => where.Pcdm.Equals(_cp_AdviceGroupDetail.Pcdm));
                                    }
                                }
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    Client.GetAdviceFrequencyAsync(null);
                    //}
                    //else
                    //{
                    //    //cbxPC.SelectedValue = _cp_AdviceGroupDetail.Pcdm;
                    //    cbxPC.SelectedIndex = 0;
                    //}
                    #endregion


                    //this.autoCompleteBoxDept.SelectedItem = ((ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource).FirstOrDefault(cp => cp.Ksdm.Equals(((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Memo));
                    Client.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void cbxMDYPMC_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
        }

        /// <summary>
        /// 频次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPC_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (cbxPC.SelectedItem != null)
                {
                    zdm.Clear();
                    zxsj.Clear();
                    string pcdm = ((CP_AdviceFrequency)cbxPC.SelectedItem).Pcdm;
                    YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                    Client.GetDropDownInfoCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    cbxSJ.ItemsSource = ea.Result.ToList();
                                    zdm.Clear();
                                    zxsj.Clear();
                                    cbxSJ.SelectedIndex = 0;
                                    //xjt,时间必须弹出BUG
                                    cplist = ea.Result.ToList();
                                    for (int i = 0; i < cplist.Count(); i++)
                                    {
                                        for (int j = 0; j < cplist[i].Zxsj.Split(',').Length - 1; j++)
                                        {
                                            if (!zxsj.Contains(cplist[i].Zxsj.Split(',')[j].ToString() + ","))
                                            {
                                                zxsj.Add(cplist[i].Zxsj.Split(',')[j].ToString() + ",");
                                            }
                                        }
                                    }

                                }
                                else
                                {
                                    PublicMethod.RadWaringBox(ea.Error);
                                }

                            };
                    Client.GetDropDownInfoAsync(pcdm);
                    Client.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }



        private void btnMDXYZ_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewAdviceGroupDetail();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 清空控件
        /// </summary>
        public void NewAdviceGroupDetail()
        {
            try
            {
                this.ManualType = Helpers.ManualType.New;//
                _cp_AdviceGroupDetail = new CP_DoctorOrder();
                //cbxMDYZBZ.SelectedValue = null;
                autoCompleteBoxOrder.SelectedItem = null;
                autoCompleteBoxOrder.Text = "";
                nudMDSL.Value = 0;
                //cbxMDDW.Text = "";
                //CurrentState = PageState.New;
                cbxMDDW.SelectedValue = null;
                cbxSJ.SelectedValue = null;
                cbxMDYF.SelectedValue = null;
                cbxPC.SelectedValue = null;
                txtZTNR.Text = "";
                zdm.Clear();
                zxsj.Clear();
                zdm = new List<int>();
                zxsj = new List<string>();
                cbxJJLX.SelectedIndex = 0;
                //add by luff 20130121
                cbxJJLX.SelectedIndex = 0;
                autoCompleteBoxDept.SelectedItem = null;
                autoCompleteBoxDept.Text = "";
                autoCompleteBoxOrder.IsEnabled = true;
                #region
                //add by luff 20130313 获得配置表关于医嘱可选不算变异参数 来判断前台是否显示
                List<APPCFG> t_listApp = Global.mAppCfg.Select(s => s).Where(s => s.Configkey.IndexOf("Yziskx") > -1).ToList();
                if (t_listApp.Count > 0)
                {
                    if (t_listApp[0].Value == "1")
                    {
                        txtisby.Visibility = Visibility.Visible;
                        radkx.Visibility = Visibility.Visible;
                        radbx.Visibility = Visibility.Visible;
                        this.radkx.IsChecked = true;
                        this.radbx.IsChecked = false;
                    }
                    else
                    {
                        txtisby.Visibility = Visibility.Collapsed;
                        radkx.Visibility = Visibility.Collapsed;
                        radbx.Visibility = Visibility.Collapsed;
                        this.radbx.IsChecked = true;
                        this.radkx.IsEnabled = false;
                    }
                }
                else
                {
                    txtisby.Visibility = Visibility.Collapsed;
                    radkx.Visibility = Visibility.Collapsed;
                    radbx.Visibility = Visibility.Collapsed;
                    this.radbx.IsChecked = true;
                    this.radkx.IsEnabled = false;
                }
                autoCompleteBoxOrder.Focus();
                #endregion
                //autoCompleteBoxOrder.Text = string.Empty;
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void btnMDQD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Check())
                {
                    InitDoctorDrug4Confirm();
                    OnAfterDrugClosedEvent(CP_AdviceGroupDetailProptery);
                }
                else
                {

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private bool Check()
        {
            #region 保存之前判断
            if (autoCompleteBoxOrder.SelectedItem == null || cbxMDYF.SelectedItem == null || cbxPC.SelectedItem == null || cbxSJ.SelectedItem == null)
            {
                string AlterMessage =
                     (autoCompleteBoxOrder.SelectedItem == null ? "\r\n" + "草药项目必须选择" : "")
                    + (cbxMDYF.SelectedItem == null ? "\r\n" + "用法必须选择" : "")
                    + (cbxPC.SelectedItem == null ? "\r\n" + "频次必须选择" : "")
                     + (cbxSJ.SelectedItem == null ? "\r\n" + "频次时间必须选择" : "");
                PublicMethod.RadAlterBox(AlterMessage, m_strTitle);
                return false;
            }
            if (zdm.Count != GetZdmCount((List<CP_PCSJ>)cbxSJ.ItemsSource))
            {
                PublicMethod.RadAlterBox("超出或者低于周代码限制数,限制为【" + GetZdmCount((List<CP_PCSJ>)cbxSJ.ItemsSource).ToString() + "】周", m_strTitle);
                return false;

            }

            else
            {
                if (zxsj.Count != GetZxsjCount((List<CP_PCSJ>)cbxSJ.ItemsSource))
                {
                    PublicMethod.RadAlterBox("超出或低于执行时间限制数,限制为【" + GetZxsjCount((List<CP_PCSJ>)cbxSJ.ItemsSource).ToString() + "】次", m_strTitle);
                    return false;
                }
            }
            List<CP_AdviceGroupDetail> listJudgeSame = cP_AdviceGroupDetailCollection.ToList<CP_AdviceGroupDetail>(); //用于存放Grid数据源

            //if (((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderValue == 2703)    //在添加长期医嘱的时候需要判断是否存在相同的项目
            //{
            for (int i = 0; i < listJudgeSame.Count; i++)
            {
                if (listJudgeSame[i].Ypdm == ((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Ypdm
                    && listJudgeSame[i].Yzbz == 2799) //  && CurrentState == PageState.New                        
                {
                    PublicMethod.RadAlterBox("存在相同项目医嘱,无法继续添加", m_strTitle);
                    return false;
                }
            }
            //}
            #endregion
            return true;
        }

        /// <summary>
        /// 点击确定时赋值
        /// </summary>
        private void InitDoctorDrug4Confirm()
        {

            if (this.ManualType == Helpers.ManualType.Edit)//草药房处方修改，需要考虑草药协定方明细去，建议去草药方维护中去修改
            {
                _cp_AdviceGroupDetail.Yzbz = 2799;
                _cp_AdviceGroupDetail.YzbzName = "草药处方项目";

                _cp_AdviceGroupDetail.Cdxh = ConvertMy.ToDecimal(((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Cdxh);
                _cp_AdviceGroupDetail.Ggxh = ConvertMy.ToDecimal(((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Ggxh);
                _cp_AdviceGroupDetail.Lcxh = 0; //((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Lcxh;
                _cp_AdviceGroupDetail.Ypdm = ((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Ypdm;
                _cp_AdviceGroupDetail.Xmlb = (int)OrderCategory;//2403;//((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Yplb;
                _cp_AdviceGroupDetail.Ypmc = ((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Ypmc;
                _cp_AdviceGroupDetail.Zxdw = ((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Zxdw;
                _cp_AdviceGroupDetail.Ypgg = ((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Ypgg;
                _cp_AdviceGroupDetail.Ypjl = Convert.ToDecimal(nudMDSL.Value);
                _cp_AdviceGroupDetail.Ypjl = Convert.ToDecimal(nudMDSL.Value);
                _cp_AdviceGroupDetail.Jldw = (((CyDrugUnitsType)cbxMDDW.SelectedItem).UnitNameCy) == "" ? "g" : ((CyDrugUnitsType)cbxMDDW.SelectedItem).UnitNameCy;
                _cp_AdviceGroupDetail.Yfdm = ((CP_DrugUseage)cbxMDYF.SelectedItem).Yfdm;
                _cp_AdviceGroupDetail.YfdmName = ((CP_DrugUseage)cbxMDYF.SelectedItem).Name;
                _cp_AdviceGroupDetail.Pcdm = ((CP_AdviceFrequency)cbxPC.SelectedItem).Pcdm;
                _cp_AdviceGroupDetail.PcdmName = ((CP_AdviceFrequency)cbxPC.SelectedItem).Name;
                _cp_AdviceGroupDetail.Lrysdm = Global.LogInEmployee.Zgdm;
                _cp_AdviceGroupDetail.Ztnr = txtZTNR.Text;
                string ypgg = _cp_AdviceGroupDetail.Ypgg == "" ? "" : "  [" + _cp_AdviceGroupDetail.Ypgg + "]  ";
                _cp_AdviceGroupDetail.Yznr = _cp_AdviceGroupDetail.Ypmc + ypgg + _cp_AdviceGroupDetail.Ypjl.ToString() + _cp_AdviceGroupDetail.Jldw + "  " + _cp_AdviceGroupDetail.YfdmName + "  " + _cp_AdviceGroupDetail.PcdmName + "   " + _cp_AdviceGroupDetail.Ztnr;
                _cp_AdviceGroupDetail.Zxts = ConvertMy.ToDecimal(nudTS.Value);
                //add by luff 20130118
                #region 判断his是否支持计价类型
                //try
                //{
                //    YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                //    referenceClient.GetAppConifgTypeCompleted +=
                //        (obj, e) =>
                //        {
                //            if (e.Error == null && e.Result > -1)
                //            {
                //                this.txtjjlx.Visibility = Visibility.Visible;
                //                this.cbxJJLX.Visibility = Visibility.Visible;

                _cp_AdviceGroupDetail.Jjlx = int.Parse(cbxJJLX.SelectedValue.ToString());
                _cp_AdviceGroupDetail.Jjlxmc = cbxJJLX.Text.ToString();

                //            }
                //            else
                //            {
                //                this.txtjjlx.Visibility = Visibility.Collapsed;
                //                this.cbxJJLX.Visibility = Visibility.Collapsed;
                //                _cp_AdviceGroupDetail.Jjlx = 1;
                //                _cp_AdviceGroupDetail.Jjlxmc = "";
                //                PublicMethod.RadWaringBox(e.Error);
                //            }
                //        };
                //    referenceClient.GetAppConifgTypeAsync("HisJjlx");
                //    referenceClient.CloseAsync();

                //}
                //catch (Exception ex)
                //{
                //    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                //}

                #endregion
                _cp_AdviceGroupDetail.Zxksdm = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm;
                _cp_AdviceGroupDetail.Zxksdmmc = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Name;

                //草药用法名称
                _cp_AdviceGroupDetail.Extension2 = ((CP_DrugUseage)cbxMDYF.SelectedItem).Name;
                //草药协定方名称
                if (((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).yplh == "草药协定方")
                {
                    _cp_AdviceGroupDetail.Extension3 = ((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Ypmc;
                }
                else
                {
                    _cp_AdviceGroupDetail.Extension3 = ((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Extension4;
                }

                //草药协定方或明细编号
                _cp_AdviceGroupDetail.Extension = ((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).ID.ToString();

                //add by luff 20130314 
                if (this.radkx.IsChecked == true)
                {
                    _cp_AdviceGroupDetail.Yzkx = 1;
                }
                else
                {
                    _cp_AdviceGroupDetail.Yzkx = 0;
                }
            }
            else
            {
                #region Cp_DoctorDrug赋值
                Strzdm = string.Empty;
                Strzxsj = string.Empty;
                _cp_AdviceGroupDetail = new CP_DoctorOrder();
                if (Global.InpatientListCurrent == null)
                    _cp_AdviceGroupDetail.Syxh = 0;
                else
                    _cp_AdviceGroupDetail.Syxh = ConvertMy.ToDecimal(Global.InpatientListCurrent.Syxh);
                _cp_AdviceGroupDetail.Bqdm = Global.LogInEmployee.Bqdm;
                _cp_AdviceGroupDetail.Ksdm = Global.LogInEmployee.Ksdm;
                _cp_AdviceGroupDetail.Lrysdm = Global.LogInEmployee.Zgdm;
                _cp_AdviceGroupDetail.Cdxh = ConvertMy.ToDecimal(((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Cdxh);
                _cp_AdviceGroupDetail.Ggxh = ConvertMy.ToDecimal(((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Ggxh);
                _cp_AdviceGroupDetail.Lcxh = 0;// ((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Lcxh;
                _cp_AdviceGroupDetail.Ypdm = ((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Ypdm;
                _cp_AdviceGroupDetail.Xmlb = (int)OrderCategory;//草药项目类别：2403 //((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Yplb;
                _cp_AdviceGroupDetail.Yzbz = 2799;
                _cp_AdviceGroupDetail.YzbzName = "草药处方项目";

                //草药用法名称
                _cp_AdviceGroupDetail.Extension2 = ((CP_DrugUseage)cbxMDYF.SelectedItem).Name;

                //草药协定方名称
                if (((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).yplh == "草药协定方")
                {
                    _cp_AdviceGroupDetail.Extension3 = ((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Ypmc;
                }
                else
                {
                    _cp_AdviceGroupDetail.Extension3 = ((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Extension4;
                }

                //草药协定方或明细编号
                _cp_AdviceGroupDetail.Extension = ((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).ID.ToString();

                _cp_AdviceGroupDetail.Ypjl = Convert.ToDecimal(nudMDSL.Value);
                _cp_AdviceGroupDetail.Jldw = (((CyDrugUnitsType)cbxMDDW.SelectedItem).UnitNameCy) == "" ? "g" : ((CyDrugUnitsType)cbxMDDW.SelectedItem).UnitNameCy;
                _cp_AdviceGroupDetail.Yfdm = ((CP_DrugUseage)cbxMDYF.SelectedItem).Yfdm;
                _cp_AdviceGroupDetail.YfdmName = ((CP_DrugUseage)cbxMDYF.SelectedItem).Name;
                _cp_AdviceGroupDetail.Pcdm = ((CP_AdviceFrequency)cbxPC.SelectedItem).Pcdm;
                _cp_AdviceGroupDetail.PcdmName = ((CP_AdviceFrequency)cbxPC.SelectedItem).Name;
                _cp_AdviceGroupDetail.Ksrq = GetDefaultOrderTime((OrderType)(Convert.ToDecimal(_cp_AdviceGroupDetail.Yzbz)));
                _cp_AdviceGroupDetail.Ypmc = ((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Ypmc;
                _cp_AdviceGroupDetail.FromTable = string.Empty;
                _cp_AdviceGroupDetail.Flag = string.Empty;
                _cp_AdviceGroupDetail.OrderGuid = Guid.NewGuid().ToString();//
                _cp_AdviceGroupDetail.Fzbz = 3500;
                _cp_AdviceGroupDetail.Zxdw = ((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Zxdw;
                _cp_AdviceGroupDetail.Ypgg = ((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Ypgg;
                _cp_AdviceGroupDetail.Dwxs = 1;//单位系数不知道为何。。。
                _cp_AdviceGroupDetail.Dwlb = cbxMDDW.SelectedValue == null ? 3007 : ((CyDrugUnitsType)cbxMDDW.SelectedItem).UnitIDCy;
                _cp_AdviceGroupDetail.Zxcs = ((CP_AdviceFrequency)cbxPC.SelectedItem).Zxcs;
                _cp_AdviceGroupDetail.Zxzq = ((CP_AdviceFrequency)cbxPC.SelectedItem).Zxzq;
                _cp_AdviceGroupDetail.Zxzqdw = ((CP_AdviceFrequency)cbxPC.SelectedItem).Zxzqdw;
                foreach (int i in zdm)
                {
                    Strzdm += i.ToString();
                }
                _cp_AdviceGroupDetail.Zdm = (zdm.Count == 0 ? null : Strzdm);
                foreach (string s in zxsj)
                {
                    if (s != "," && s != "")
                    {
                        Strzxsj += s;
                    }
                }
                _cp_AdviceGroupDetail.Zxsj = (zxsj.Count == 0 ? null : Strzxsj);

                _cp_AdviceGroupDetail.Yznr = _cp_AdviceGroupDetail.Ypmc + " " + _cp_AdviceGroupDetail.PcdmName + " " + _cp_AdviceGroupDetail.YfdmName;
                _cp_AdviceGroupDetail.Yzzt = 3200;
                _cp_AdviceGroupDetail.Fzxh = 0;
                _cp_AdviceGroupDetail.Yzlb = (int)PanelCategory;//草药医嘱类别
                _cp_AdviceGroupDetail.Zxts = ConvertMy.ToDecimal(nudTS.Value);
                _cp_AdviceGroupDetail.Ypzsl = 0; //出院带药。
                _cp_AdviceGroupDetail.Ztnr = txtZTNR.Text;
                //add by luff 20130118
                #region 判断his是否支持计价类型
                //try
                //{
                //    YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                //    referenceClient.GetAppConifgTypeCompleted +=
                //        (obj, e) =>
                //        {
                //            if (e.Error == null && e.Result > -1)
                //            {
                this.txtjjlx.Visibility = Visibility.Visible;
                this.cbxJJLX.Visibility = Visibility.Visible;

                _cp_AdviceGroupDetail.Jjlx = int.Parse(cbxJJLX.SelectedValue.ToString());
                _cp_AdviceGroupDetail.Jjlxmc = cbxJJLX.Text.ToString();

                //            }
                //            else
                //            {
                //                this.txtjjlx.Visibility = Visibility.Collapsed;
                //                this.cbxJJLX.Visibility = Visibility.Collapsed;
                //                _cp_AdviceGroupDetail.Jjlx = 1;
                //                _cp_AdviceGroupDetail.Jjlxmc = "";
                //                PublicMethod.RadWaringBox(e.Error);
                //            }
                //        };
                //    referenceClient.GetAppConifgTypeAsync("HisJjlx");
                //    referenceClient.CloseAsync();

                //}
                //catch (Exception ex)
                //{
                //    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                //}

                #endregion
                _cp_AdviceGroupDetail.Zxksdm = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm;
                _cp_AdviceGroupDetail.Zxksdmmc = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Name;

                if (this.radkx.IsChecked == true)
                {
                    _cp_AdviceGroupDetail.Yzkx = 1;
                }
                else
                {
                    _cp_AdviceGroupDetail.Yzkx = 0;
                }
                #endregion
            }
        }

        /// <summary>
        /// 获取周代码的数量为了判断是否选择超出或者低于约束的值
        /// </summary>
        /// <param name="pcsj"></param>
        /// <returns></returns>
        private int GetZdmCount(List<CP_PCSJ> pcsj)
        {
            int zdmlength = 0;
            foreach (var c in pcsj)
            {
                zdmlength = c.Zdm.Trim(' ').Length;
            }
            return zdmlength;

        }

        /// <summary>
        /// 获取时间代码的数量为了判断是否选择超出或者低于约束的值
        /// </summary>
        /// <param name="pcsj"></param>
        /// <returns></returns>
        private int GetZxsjCount(List<CP_PCSJ> pcsj)
        {
            int zxsjlength = 0;
            foreach (var c in pcsj)
            {
                zxsjlength = c.Zxsj.Split(',').Length - 1;
            }
            return zxsjlength;
        }

        #region delegate own events
        public delegate void DrugLoaded(object sender, RoutedEventArgs e);
        /// <summary>
        /// 此事件的用是为了能让控件直接在界面上显示
        /// </summary>
        public event DrugLoaded AfterDrugLoadedEvent;

        protected virtual void OnAfterDrugLoadedEvent(RoutedEventArgs e)
        {
            if (AfterDrugLoadedEvent != null)
            {
                InitPage();
                RegisterKeyEvent();
                //new CyEnterToTab(this);
            }
        }

        //定义药品医嘱委托和事件
        public delegate void DrugConfirmed(object sender, CP_DoctorOrder e);
        public event DrugConfirmed AfterDrugCinfirmeddEvent;

        protected virtual void OnAfterDrugClosedEvent(CP_DoctorOrder e)
        {
            if (AfterDrugCinfirmeddEvent != null)
                AfterDrugCinfirmeddEvent(this, e);
        }

        /// <summary>
        /// 路径执行,医嘱默认时间
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        private string GetDefaultOrderTime(OrderType orderType)
        {
            string strTime = string.Empty;
            if (orderType == OrderType.Long)
                strTime = Convert.ToString(DateTime.Now.AddDays(1).Date + new TimeSpan(8, 0, 0));
            else
            {
                int hour = DateTime.Now.Hour;
                int minute = DateTime.Now.Minute;
                if (minute <= 30)
                    minute = 30;
                else
                {
                    hour += 1;
                    minute = 0;
                }
                strTime = Convert.ToString(DateTime.Today + new TimeSpan(hour, minute, 0));
            }
            return strTime;
        }
        #endregion

        /// <summary>
        /// 初始化需要修改的医嘱
        /// </summary>
        public void InitModifyOrder()
        {
            try
            {
                autoCompleteBoxOrder.SelectedItem = null;

                autoCompleteBoxOrder.IsEnabled = false;

                autoCompleteBoxOrder.SelectedItem = ((ObservableCollection<CP_CYFUnion>)autoCompleteBoxOrder.ItemsSource).First(cp => cp.Ypdm.Equals(_cp_AdviceGroupDetail.Ypdm));
                nudMDSL.Value = Convert.ToDouble(_cp_AdviceGroupDetail.Ypjl);
                //cbxMDDW.Text = _cp_AdviceGroupDetail.Jldw;
                /********* Edit by dxj 2011/7/23 修改原因：下拉框加载值错误 *******/
                //cbxMDDW.SelectedValue = _cp_AdviceGroupDetail.Dwlb;

                cbxMDDW.SelectedItem = ((List<CyDrugUnitsType>)cbxMDDW.ItemsSource).First(where => where.UnitNameCy.Equals(_cp_AdviceGroupDetail.Jldw));
                //cbxMDYF.SelectedValue = _cp_AdviceGroupDetail.Yfdm;
                //cbxPC.SelectedValue = _cp_AdviceGroupDetail.Pcdm;
                /************/
                txtZTNR.Text = _cp_AdviceGroupDetail.Ztnr == null ? string.Empty : _cp_AdviceGroupDetail.Ztnr;
                //add by luff 20130118
                cbxJJLX.SelectedValue = (short)_cp_AdviceGroupDetail.Jjlx;
                if (_cp_AdviceGroupDetail.Zxksdm == "")
                {
                    autoCompleteBoxDept.SelectedItem = null;
                }
                else
                {
                    autoCompleteBoxDept.SelectedItem = ((ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource).FirstOrDefault(cp => cp.Ksdm.Equals(_cp_AdviceGroupDetail.Zxksdm));
                }

                if (_cp_AdviceGroupDetail.Yzkx == 0)
                {
                    this.radbx.IsChecked = true;
                    this.radkx.IsChecked = false;
                }
                else
                {
                    this.radbx.IsChecked = false;
                    this.radkx.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #region KEYUP
        /// <summary>
        /// 注册KEYUP
        /// </summary>
        private void RegisterKeyEvent()
        {


            autoCompleteBoxOrder.KeyUp += new KeyEventHandler(autoCompleteBoxOrder_KeyUp);
            nudMDSL.KeyUp += new KeyEventHandler(nudMDSL_KeyUp);
            cbxMDDW.KeyUp += new KeyEventHandler(cbxMDDW_KeyUp);
            cbxMDYF.KeyUp += new KeyEventHandler(cbxMDYF_KeyUp);
            cbxPC.KeyUp += new KeyEventHandler(cbxPC_KeyUp);
            cbxSJ.KeyUp += new KeyEventHandler(cbxSJ_KeyUp);
            cbxJJLX.KeyUp += new KeyEventHandler(cbxJJLX_KeyUp);
            autoCompleteBoxDept.KeyUp += new KeyEventHandler(autoCompleteBoxDept_KeyUp);
            radbx.KeyUp += new KeyEventHandler(radbx_KeyUp);
            radkx.KeyUp += new KeyEventHandler(radkx_KeyUp);
            txtZTNR.KeyUp += new KeyEventHandler(txtZTNR_KeyUp);
        }



        /// <summary>
        /// 项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoCompleteBoxOrder_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                nudMDSL.Focus();
        }


        /// <summary>
        /// 数量-数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudMDSL_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbxMDDW.Focus();
        }

        /// <summary>
        /// 数量-单位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMDDW_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbxMDYF.Focus();
        }

        /// <summary>
        /// 用法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMDYF_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbxPC.Focus();
        }

        /// <summary>
        /// 频次-代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPC_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbxSJ.Focus();
        }

        /// <summary>
        /// 频次-时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSJ_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbxJJLX.Focus();
        }
        /// <summary>
        /// 计价类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxJJLX_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                autoCompleteBoxDept.Focus();
        }
        /// <summary>
        /// 执行科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoCompleteBoxDept_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                radbx.Focus();
        }
        /// <summary>
        /// 医嘱是否变异
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radbx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                radkx.Focus();
        }
        private void radkx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtZTNR.Focus();
        }
        /// <summary>
        /// 嘱托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtZTNR_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnMDQD.Focus();
        }
        #endregion


    }

    /// <summary>
    /// 测试功能类，暂时放这里
    /// </summary>
    public class CyDrugUnitsType
    {
        public string UnitNameCy
        {
            get;
            set;
        }

        public decimal UnitIDCy
        {
            get;
            set;
        }

        public CyDrugUnitsType()
        { }

        public CyDrugUnitsType(string strName, decimal UnitIDCy)
        {
            this.UnitNameCy = strName;
            this.UnitIDCy = UnitIDCy;
        }
    }

    public class CyEnterToTab
    {
        private static List<Type> _ctrlTypesCy = new List<Type>()
        {
            typeof(RadComboBox),
            typeof(AutoCompleteBox),
            typeof(RadNumericUpDown),
            typeof(TextBox),
            typeof(RadButton),
        };
        public static void RegisterType(Type type)
        {
            if (!_ctrlTypesCy.Contains(type))
            {
                _ctrlTypesCy.Add(type);
            }
        }
        public static IEnumerable<T> FindChildrenCy<T>(DependencyObject parent) where T : class
        {
            var count = VisualTreeHelper.GetChildrenCount(parent);
            if (count > 0)
            {
                for (var i = 0; i < count; i++)
                {
                    var child = VisualTreeHelper.GetChild(parent, i);
                    var t = child as T;
                    if (t != null)
                        yield return t;
                    var children = FindChildrenCy<T>(child);
                    foreach (var item in children)
                        yield return item;
                }
            }
        }
        private UIElement _parentCy;
        private List<Control> _controlsCy = new List<Control>();
        public CyEnterToTab(UIElement parent)
        {
            _parentCy = parent;
            // 如果控件还没有加载完就调用 FindChildrenCy() 方法,则不能查找到子控件,
            // 所以用定时器
            DispatcherTimer timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromTicks(100);
            timer.Tick += new EventHandler(TimerCy_Tick);
            timer.Start();
        }
        void TimerCy_Tick(object sender, EventArgs e)
        {
            (sender as DispatcherTimer).Stop();
            FindChildrenCy();
        }
        private bool _isAssociatingCy = false;
        private void FindChildrenCy()
        {
            if (_isAssociatingCy) return;
            _isAssociatingCy = true;
            try
            {
                // 清除原来关联过的
                foreach (var c in _controlsCy)
                {
                    c.KeyUp -= new KeyEventHandler(ControlCy_KeyUp);
                }
                _controlsCy.Clear();
                // 获取可以 Tab 的控件并加入到列表中
                IEnumerable<Control> originals = CyEnterToTab.FindChildrenCy<Control>(_parentCy);
                foreach (var c in originals)
                {
                    if (c.IsTabStop && c.Visibility == Visibility.Visible)// && c.IsEnabled)
                    {
                        var t1 = c.GetType();
                        foreach (var t2 in _ctrlTypesCy)
                        {
                            if (t1.IsAssignableFrom(t2))
                            {
                                c.KeyUp += new KeyEventHandler(ControlCy_KeyUp);
                                _controlsCy.Add(c);
                                break;
                            }
                        }
                    }
                }
                // 根据 TabIndex 的原始值排序
                _controlsCy.Sort(new TabIndexComparer());
            }
            finally
            {
                _isAssociatingCy = false;
            }
        }
        void ControlCy_KeyUp(object sender, KeyEventArgs e)
        {
            if ((e.Key == Key.Enter))
            {
                Control source = (sender as Control);
                int index = _controlsCy.IndexOf(source);
                if (e.Key == Key.Enter || e.Key == Key.Down)
                {
                    if (index < _controlsCy.Count - 1)
                    {
                        _controlsCy[index + 1].Focus();
                    }
                }
                else
                {
                    if (index > 0)
                    {
                        _controlsCy[index - 1].Focus();
                    }
                }
            }
        }
        private class TabIndexComparer : IComparer<Control>
        {
            public int Compare(Control x, Control y)
            {
                if (x == y)
                {
                    return 0;
                }
                if (x.TabIndex <= y.TabIndex)
                {
                    return -1;
                }
                else
                {
                    return 1;
                }
            }
        }
    }


}
