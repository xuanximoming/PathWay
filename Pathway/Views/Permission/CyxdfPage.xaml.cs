using DrectSoft.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using YidanEHRApplication.Views.ChildWindows;

namespace YidanEHRApplication.Views.Permission
{
    public partial class CyxdfPage : Page
    {
        bool isTrue = true;
        static CP_CYXDFMX m_CP_CYXDFMX = new CP_CYXDFMX();
        /// <summary>
        /// 列表数据源
        /// </summary>
        private ObservableCollection<CP_CYXDFMX> m_listsouce;

        #region 属性变量
        /// <summary>
        /// 用于存储列表中选中的行记录
        /// </summary>
        List<int> zdm = new List<int>();  //存储周代码，存储当前选中的星期
        List<string> zxsj = new List<string>();//存储执行时间，存储当前选中的时间
        string Strzdm = string.Empty;    //zdm字符串形式
        string Strzxsj = string.Empty; //zxsj字符串形式
        ObservableCollection<CP_CYXDFMX> CP_CYXDFMXCollection = new ObservableCollection<CP_CYXDFMX>();
        List<CP_PCSJ> cplist = new List<CP_PCSJ>();

        private CP_CYXDFMX m_cymxdetail = new CP_CYXDFMX();

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


                    autoCompleteBoxCyf.IsEnabled = true;
                    autoCompleteBoxOrder.IsEnabled = true;
                    nudMDSL.IsEnabled = true;
                    cbxMDDW.IsEnabled = true;
                    cbxJJLX.IsEnabled = true;
                    cbxMDYF.IsEnabled = true;
                    cbxPC.IsEnabled = true;
                    cbxSJ.IsEnabled = true;
                    autoCompleteBoxDept.IsEnabled = true;
                    txtZTNR.IsEnabled = true;

                    this.btnAdd.IsEnabled = false;
                    this.btnDel.IsEnabled = false;
                    this.btnUpdate.IsEnabled = false;

                    this.btnClear.IsEnabled = true;

                    this.btnSave.IsEnabled = true;


                }
                else if (value == OperationState.EDIT)
                {
                    autoCompleteBoxCyf.IsEnabled = false;
                    autoCompleteBoxOrder.IsEnabled = true;
                    nudMDSL.IsEnabled = true;
                    cbxMDDW.IsEnabled = true;
                    cbxJJLX.IsEnabled = true;
                    cbxMDYF.IsEnabled = true;
                    cbxPC.IsEnabled = true;
                    cbxSJ.IsEnabled = true;
                    autoCompleteBoxDept.IsEnabled = true;
                    txtZTNR.IsEnabled = true;

                    this.btnAdd.IsEnabled = false;
                    this.btnDel.IsEnabled = false;
                    this.btnUpdate.IsEnabled = false;

                    this.btnClear.IsEnabled = true;

                    this.btnSave.IsEnabled = true;

                }
                else
                {

                    autoCompleteBoxCyf.IsEnabled = false;
                    autoCompleteBoxOrder.IsEnabled = false;
                    nudMDSL.IsEnabled = false;
                    cbxMDDW.IsEnabled = false;
                    cbxJJLX.IsEnabled = false;
                    cbxMDYF.IsEnabled = false;
                    cbxPC.IsEnabled = false;
                    cbxSJ.IsEnabled = false;
                    autoCompleteBoxDept.IsEnabled = false;
                    txtZTNR.IsEnabled = false;

                    this.btnAdd.IsEnabled = true;
                    this.btnDel.IsEnabled = true;
                    this.btnUpdate.IsEnabled = true;

                    this.btnClear.IsEnabled = false;

                    this.btnSave.IsEnabled = false;
                }


            }
        }
        static List<CP_CYXDFMX> CP_CYXDFMX = new List<CP_CYXDFMX>();

        #region 设定计价类别
        public class OrderTypeName
        {
            public string OrderName
            {
                get;
                set;
            }
            public short OrderValue
            {
                get;
                set;
            }
            public OrderTypeName(string orderName, short orderValue)
            {
                OrderName = orderName;
                OrderValue = orderValue;
            }
        }
        #endregion

        /// <summary>
        /// 测试功能类，暂时放这里
        /// </summary>
        public class DrugUnitsType
        {
            public string UnitName
            {
                get;
                set;
            }

            public decimal UnitID
            {
                get;
                set;
            }

            public DrugUnitsType()
            { }

            public DrugUnitsType(string strName, decimal unitID)
            {
                this.UnitName = strName;
                this.UnitID = unitID;
            }
        }
        #endregion

        public CyxdfPage()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(CyxdfPage_Loaded);
        }

        void CyxdfPage_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isTrue)
                {
                    isTrue = true;
                    return;
                }
                CurrentState = OperationState.VIEW;
                BindGridView();
                InitDrugInfo();//初始化草药药品信息
                InitCyfInfo();//初始化草药处方信息
                IntiComboBoxDept();

                #region 动态创建界面控件的KeyUp事件 add by luff 20130520
                MadeKeyUp keyUp = new MadeKeyUp();
                keyUp.Controls.Add(autoCompleteBoxCyf);
                keyUp.Controls.Add(autoCompleteBoxOrder);
                keyUp.Controls.Add(nudMDSL);
                keyUp.Controls.Add(cbxMDDW);
                keyUp.Controls.Add(cbxJJLX);
                keyUp.Controls.Add(cbxMDYF);
                keyUp.Controls.Add(cbxPC);
                keyUp.Controls.Add(cbxSJ);
                keyUp.Controls.Add(autoCompleteBoxDept);
                keyUp.Controls.Add(txtZTNR);
                keyUp.Made_KeyUp();
                #endregion

                //HtmlPage.Plugin.Focus();
                //this.autoCompleteBoxCyf.SelectAll();
                this.autoCompleteBoxCyf.Focus();


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
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


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
                Client.GetCyxdfMXInfoCompleted +=
                (obj, e) =>
                {
                    radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        GridView.ItemsSource = e.Result;
                        //初始化查询数据源
                        m_listsouce = (ObservableCollection<CP_CYXDFMX>)GridView.ItemsSource;
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };

                Client.GetCyxdfMXInfoAsync();
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 绑定页面中编辑信息
        /// </summary>
        /// <param name="pe_fun">草药方明细为空，如果为空编辑区域值为空</param>
        private void Bind_Data(CP_CYXDFMX _cpCyfMx)
        {
            if (_cpCyfMx != null)
            {

                autoCompleteBoxCyf.SelectedItem = ((ObservableCollection<CP_CYXDF>)autoCompleteBoxCyf.ItemsSource).First(cp => cp.ID.Equals(_cpCyfMx.idm));
                autoCompleteBoxOrder.SelectedItem = ((ObservableCollection<CP_PlaceOfDrug>)autoCompleteBoxOrder.ItemsSource).First(cp => cp.Ypdm.Equals(_cpCyfMx.Ypdm));
                nudMDSL.Value = Convert.ToDouble(_cpCyfMx.Ypjl);
                cbxMDDW.SelectedItem = ((List<DrugUnitsType>)cbxMDDW.ItemsSource).First(where => where.UnitName.Equals(_cpCyfMx.Jldw));
                //cbxPC.SelectedItem = ((ObservableCollection<CP_AdviceFrequency>)cbxPC.ItemsSource).First(cp => cp.Zxcs.Equals(_cpCyfMx.cfts));
                //cbxSJ.SelectedItem = ((ObservableCollection<CP_AdviceFrequency>)cbxSJ.ItemsSource).First(cp => cp.Zxzq.Equals(_cpCyfMx.Zxts));
                cbxPC.SelectedValue = _cpCyfMx.cfts.ToString();
                cbxSJ.SelectedValue = _cpCyfMx.Zxts.ToString();

                this.txtZTNR.Text = _cpCyfMx.Memo;
                cbxJJLX.SelectedValue = (short)_cpCyfMx.Isjj;
                if (_cpCyfMx.Zxksdm == "")
                {
                    autoCompleteBoxDept.SelectedItem = null;
                }
                else
                {
                    autoCompleteBoxDept.SelectedItem = ((ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource).First(cp => cp.Ksdm.Equals(_cpCyfMx.Zxksdm));
                }
            }
            else
            {
                m_cymxdetail = new CP_CYXDFMX();

            }
        }
        #region  初始化草药处方项目
        /// <summary>
        /// 初始化草药处方项目
        /// </summary>
        private void InitCyfInfo()
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.GetCyxdfInfoCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {

                        autoCompleteBoxCyf.ItemsSource = e.Result;
                        autoCompleteBoxCyf.ItemFilter = OrderCyfFilter;

                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                        return;
                    }
                };
            client.GetCyxdfInfoAsync();
            client.CloseAsync();
        }

        public bool OrderCyfFilter(string strFilter, object item)
        {
            CP_CYXDF deptList = (CP_CYXDF)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())) || (deptList.Wb.StartsWith(strFilter.ToUpper())) || (deptList.Wb.Contains(strFilter.ToUpper())));
        }
        #endregion
        /// <summary>
        /// 初始化草药药品
        /// </summary>
        #region 初始化草药药品
        private void InitDrugInfo()
        {
            YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
            Client.GetDrugInfoCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        autoCompleteBoxOrder.ItemsSource = e.Result;
                        autoCompleteBoxOrder.ItemFilter = OrderFilter;
                        //if (this.ManualType == Helpers.ManualType.Edit)
                        //    InitModifyOrder();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                        return;
                    }
                };
            Client.GetDrugInfoAsync(null);
            Client.CloseAsync();
        }
        public bool OrderFilter(string strFilter, object item)
        {
            CP_PlaceOfDrug deptList = (CP_PlaceOfDrug)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())));
        }
        #endregion
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

        private List<DrugUnitsType> m_ListDrugUnit = new List<DrugUnitsType>();
        private void InitUnitType(string strName, decimal unitID)
        {
            DrugUnitsType type = new DrugUnitsType(strName, unitID);
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
                    #region 单位
                    //药品医嘱时的单位填充数据为规格单位和最小单位
                    m_ListDrugUnit = new List<DrugUnitsType>();
                    InitUnitType(((CP_PlaceOfDrug)autoCompleteBoxOrder.SelectedItem).Ggdw.ToString(), 3006);
                    InitUnitType(((CP_PlaceOfDrug)autoCompleteBoxOrder.SelectedItem).Zxdw.ToString(), 3007);
                    cbxMDDW.ItemsSource = null;
                    cbxMDDW.ItemsSource = m_ListDrugUnit;
                    if (m_ListDrugUnit.Count > 0)
                        cbxMDDW.SelectedIndex = 0;
                    #region 暂时保留

                    #endregion
                    #endregion

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
                                //cbxMDYF.SelectedValue = _CP_CYXDFMX.Yfdm;
                                if (ea.Result.Count > 0)
                                {
                                    cbxMDYF.SelectedIndex = 0;
                                    //if (_CP_CYXDFMX.Yfdm != null)
                                    //{
                                    //    cbxMDYF.SelectedValue = _CP_CYXDFMX.Yfdm;
                                    //    //cbxMDYF.SelectedItem = ((ObservableCollection<CP_DrugUseage>)cbxMDYF.ItemsSource).First(where => where.Yfdm.Equals(_CP_CYXDFMX.Yfdm));   //7.25 序列不包含在元素中     ZM
                                    //}
                                }
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    Client.GetDrugUseageAsync(((CP_PlaceOfDrug)autoCompleteBoxOrder.SelectedItem).Jxdm);
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
                                //cbxPC.SelectedValue = _CP_CYXDFMX.Pcdm;
                                if (ea.Result.Count > 0)
                                {
                                    cbxPC.SelectedIndex = 0;
                                    //if (_CP_CYXDFMX.Pcdm != null)
                                    //{
                                    //    cbxPC.SelectedItem = ((ObservableCollection<CP_AdviceFrequency>)cbxPC.ItemsSource).First(where => where.Pcdm.Equals(_CP_CYXDFMX.Pcdm));
                                    //}
                                }
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    Client.GetAdviceFrequencyAsync(null);

                    #endregion

                    //this.autoCompleteBoxDept.SelectedItem = ((ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource).FirstOrDefault(cp => cp.Ksdm.Equals(((CP_PlaceOfDrug)autoCompleteBoxOrder.SelectedItem).Memo));
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

        #endregion

        #region 事件
        private void autoCompleteBoxCyf_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                //if (autoCompleteBoxCyf.SelectedItem != null)
                //{

                //}
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
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
                    return;
                }
                if (CurrentState == OperationState.VIEW)
                {
                    m_cymxdetail = (CP_CYXDFMX)GridView.SelectedItem;
                    Bind_Data(m_cymxdetail);
                }

            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 修改按钮事件
        /// </summary>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.GridView.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选中要修改的行！", "提示");
                    return;
                }

                m_cymxdetail = (CP_CYXDFMX)GridView.SelectedItem;
                CurrentState = OperationState.EDIT;
                autoCompleteBoxCyf.Focus();

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
                autoCompleteBoxCyf.Focus();

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

            string sCyfmc = txt_Cyfmc.Text.Trim();
            string sCyfMxmc = txt_Cyfmxmc.Text.Trim();
            string sZtnr = txt_ztnr.Text.Trim();

            //集合类型初始化
            List<CP_CYXDFMX> t_listsouce = m_listsouce.ToList();


            if (sCyfmc.Length >= 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.Extension4.IndexOf(sCyfmc) > -1).ToList();

            }

            if (sCyfMxmc.Length >= 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.Ypmc.IndexOf(sCyfMxmc) > -1).ToList();

            }


            if (sZtnr.Length >= 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.Memo.IndexOf(sZtnr) > -1).ToList();

            }


            GridView.ItemsSource = t_listsouce.ToList();


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
                //if (GridView.SelectedItem == null)
                //{
                //    PublicMethod.RadAlterBox("请选择一项", "提示");
                //    return;
                //}
                //else
                //{
                //    DialogParameters parameters = new DialogParameters(); 
                //    parameters.Content = String.Format("提示: {0}", "确定删除吗？删除后不能恢复!");
                //    parameters.Header = "提示";
                //    parameters.IconContent = null;
                //    parameters.OkButtonContent = "确定"; 
                //    parameters.CancelButtonContent = "取消";
                //    parameters.Closed = OnDelete;
                //    RadWindow.Confirm(parameters);
                //}
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

                int iID = (GridView.SelectedItem as CP_CYXDFMX).ID;
                YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                Client.DelCYXDFMXCompleted +=
                (obj, ea) =>
                {
                    if (ea.Error == null)
                    {
                        PublicMethod.RadAlterBox("删除成功", "提示");
                        BindGridView();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(ea.Error);
                        return;
                    }
                };

                Client.DelCYXDFMXAsync(iID);
                Client.CloseAsync();
            }
        }

        /// <summary>
        /// 清空控件
        /// </summary>
        void NewAdviceGroupDetail()
        {
            autoCompleteBoxCyf.SelectedItem = null;
            autoCompleteBoxCyf.Text = "";
            autoCompleteBoxOrder.SelectedItem = null;
            autoCompleteBoxOrder.Text = "";
            nudMDSL.Value = 1;
            nudMDTS.Value = 1;
            cbxMDDW.SelectedItem = null;
            cbxMDDW.Text = "";
            cbxJJLX.SelectedIndex = 0;
            //cbxJJLX.SelectedItem = null;
            //cbxJJLX.Text = "";
            cbxMDYF.SelectedItem = null;
            cbxMDYF.Text = "";
            cbxPC.SelectedItem = null;
            cbxPC.Text = "";
            cbxSJ.SelectedItem = null;
            cbxSJ.Text = "";
            autoCompleteBoxDept.SelectedItem = null;
            autoCompleteBoxDept.Text = "";
            txtZTNR.Text = "";
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            #region 验证数据并初始化

            if (autoCompleteBoxCyf.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择草药处方名", "提示", autoCompleteBoxCyf);
                isTrue = false;
                return;
            }
            if (autoCompleteBoxOrder.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择草药明细名称", "提示", autoCompleteBoxOrder);
                isTrue = false;
                return;
            }

            if (cbxMDYF.SelectedItem == null || cbxMDYF.Text == "")
            {
                PublicMethod.RadAlterBoxRe("请输入草药药品用法", "提示", cbxMDYF);
                isTrue = false;
                return;
            }
            if (cbxPC.SelectedItem == null || cbxPC.Text == "")
            {
                PublicMethod.RadAlterBoxRe("请输入草药使用频次", "提示", cbxPC);
                isTrue = false;
                return;
            }


            if (cbxSJ.SelectedItem == null || cbxSJ.Text == "")
            {
                PublicMethod.RadAlterBoxRe("请输入草药使用周期", "提示", cbxSJ);
                isTrue = false;
                return;
            }

            CP_CYXDFMX _cymxdetail = new CP_CYXDFMX();
            _cymxdetail.cfxh = Guid.NewGuid().ToString();
            //_cymxdetail.cfts = Convert.ToInt32(nudMDTS.Value);
            _cymxdetail.idm = ((CP_CYXDF)(autoCompleteBoxCyf.SelectedItem)).ID;
            _cymxdetail.Ypdm = ((CP_PlaceOfDrug)(autoCompleteBoxOrder.SelectedItem)).Ypdm.ToString();
            _cymxdetail.Ypmc = ((CP_PlaceOfDrug)(autoCompleteBoxOrder.SelectedItem)).Ypmc.ToString();
            _cymxdetail.Ypjl = Convert.ToDecimal(nudMDSL.Value);
            _cymxdetail.Jldw = ((DrugUnitsType)cbxMDDW.SelectedItem).UnitName;
            _cymxdetail.Yfdm = ((CP_DrugUseage)cbxMDYF.SelectedItem).Yfdm;
            _cymxdetail.cfts = ((CP_AdviceFrequency)cbxPC.SelectedItem).Zxcs;
            _cymxdetail.Zxts = ((CP_AdviceFrequency)cbxPC.SelectedItem).Zxzq;
            _cymxdetail.ypbz = ((CP_AdviceFrequency)cbxPC.SelectedItem).Zxzqdw;
            _cymxdetail.Pcdm = ((CP_AdviceFrequency)cbxPC.SelectedItem).Pcdm;
            _cymxdetail.Dwlb = 1;
            //用法名称
            _cymxdetail.lcxmdm = ((CP_DrugUseage)cbxMDYF.SelectedItem).Name;
            _cymxdetail.pxxh = 0;
            _cymxdetail.ypbz = 0;
            _cymxdetail.yplh = ((CP_PlaceOfDrug)autoCompleteBoxOrder.SelectedItem).Ggdw;
            _cymxdetail.ekxs = 1;
            _cymxdetail.ekdw = ((DrugUnitsType)cbxMDDW.SelectedItem).UnitName;
            _cymxdetail.ekbz = 0;
            _cymxdetail.Yzkx = 1;

            _cymxdetail.Isjj = cbxJJLX.SelectedIndex == -1 ? 0 : int.Parse(cbxJJLX.SelectedValue.ToString());

            _cymxdetail.Zxksdm = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm;

            _cymxdetail.Py = ((CP_PlaceOfDrug)(autoCompleteBoxOrder.SelectedItem)).Py.ToString();
            _cymxdetail.Wb = ((CP_PlaceOfDrug)(autoCompleteBoxOrder.SelectedItem)).Wb.ToString();
            _cymxdetail.Memo = this.txtZTNR.Text;

            //产地序号
            _cymxdetail.Extension = ((CP_PlaceOfDrug)(autoCompleteBoxOrder.SelectedItem)).Cdxh.ToString();
            //规格序号
            _cymxdetail.Extension1 = ((CP_PlaceOfDrug)(autoCompleteBoxOrder.SelectedItem)).Ggxh.ToString();
            //执行单位
            _cymxdetail.Extension2 = ((CP_PlaceOfDrug)autoCompleteBoxOrder.SelectedItem).Zxdw;
            //剂型代码
            _cymxdetail.Extension3 = ((CP_PlaceOfDrug)autoCompleteBoxOrder.SelectedItem).Jxdm;
            _cymxdetail.Extension4 = ((CP_CYXDF)(autoCompleteBoxCyf.SelectedItem)).cfmc;



            #endregion

            if (CurrentState == OperationState.NEW)
            {


                serviceCon = PublicMethod.YidanClient;
                serviceCon.InsertCYXDFMXCompleted +=
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
                serviceCon.InsertCYXDFMXAsync(_cymxdetail);
                serviceCon.CloseAsync();

                CurrentState = OperationState.VIEW;
            }
            if (CurrentState == OperationState.EDIT)
            {

                _cymxdetail.ID = m_cymxdetail.ID;
                _cymxdetail.idm = ((CP_CYXDF)(autoCompleteBoxCyf.SelectedItem)).ID;
                _cymxdetail.Ypdm = ((CP_PlaceOfDrug)(autoCompleteBoxOrder.SelectedItem)).Ypdm.ToString();
                _cymxdetail.Ypmc = ((CP_PlaceOfDrug)(autoCompleteBoxOrder.SelectedItem)).Ypmc.ToString();
                _cymxdetail.Ypjl = Convert.ToDecimal(nudMDSL.Value);
                _cymxdetail.Jldw = ((DrugUnitsType)cbxMDDW.SelectedItem).UnitName;
                _cymxdetail.Yfdm = ((CP_DrugUseage)cbxMDYF.SelectedItem).Yfdm;
                _cymxdetail.cfts = ((CP_AdviceFrequency)cbxPC.SelectedItem).Zxcs;
                _cymxdetail.Zxts = ((CP_AdviceFrequency)cbxPC.SelectedItem).Zxzq;
                _cymxdetail.ypbz = ((CP_AdviceFrequency)cbxPC.SelectedItem).Zxzqdw;
                _cymxdetail.Pcdm = ((CP_AdviceFrequency)cbxPC.SelectedItem).Pcdm;
                _cymxdetail.Isjj = cbxJJLX.SelectedIndex == -1 ? 0 : int.Parse(cbxJJLX.SelectedValue.ToString());
                _cymxdetail.Zxksdm = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm;
                //用法名称
                _cymxdetail.lcxmdm = ((CP_DrugUseage)cbxMDYF.SelectedItem).Name;
                _cymxdetail.Py = ((CP_PlaceOfDrug)(autoCompleteBoxOrder.SelectedItem)).Py.ToString();
                _cymxdetail.Wb = ((CP_PlaceOfDrug)(autoCompleteBoxOrder.SelectedItem)).Wb.ToString();
                _cymxdetail.Memo = this.txtZTNR.Text;

                //产地序号
                _cymxdetail.Extension = ((CP_PlaceOfDrug)(autoCompleteBoxOrder.SelectedItem)).Cdxh.ToString();
                //规格序号
                _cymxdetail.Extension1 = ((CP_PlaceOfDrug)(autoCompleteBoxOrder.SelectedItem)).Ggxh.ToString();
                //执行单位
                _cymxdetail.Extension2 = ((CP_PlaceOfDrug)autoCompleteBoxOrder.SelectedItem).Zxdw;
                //剂型代码
                _cymxdetail.Extension3 = ((CP_PlaceOfDrug)autoCompleteBoxOrder.SelectedItem).Jxdm;
                _cymxdetail.Extension4 = ((CP_CYXDF)(autoCompleteBoxCyf.SelectedItem)).cfmc;


                YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                Client.UpdateCYXDFMXCompleted += (sb, ea) =>
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
                };
                Client.UpdateCYXDFMXAsync(_cymxdetail);


                CurrentState = OperationState.VIEW;
            }
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {

            txt_Cyfmc.Text = "";
            txt_Cyfmxmc.Text = "";
            txt_ztnr.Text = "";
            BindGridView();
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            CurrentState = OperationState.VIEW;


        }

        /// <summary>
        /// 进入草药协定方窗体事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAddCyXDF_Click(object sender, RoutedEventArgs e)
        {
            RWCyXDF rwcyxdf = new RWCyXDF();
            rwcyxdf.Closed += new EventHandler<WindowClosedEventArgs>(rwcyxdf_Closed);
            rwcyxdf.ResizeMode = ResizeMode.NoResize;
            rwcyxdf.ShowDialog();
        }
        private void rwcyxdf_Closed(object sender, EventArgs e)
        {
            //刷新数据源
            BindGridView();

        }

        #endregion


    }
}
