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
using System.Collections.ObjectModel;
using YidanEHRApplication.DataService;
using Telerik.Windows.Controls;
using YidanEHRApplication.Models;
using System.Text;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Views.ChildWindows;


namespace YidanEHRApplication.Views.UserControls
{
    public partial class UCOtherOrder : UserControl
    {
        private const string m_strTitle = "医嘱提示"; //定义弹出框标题栏

        //add by luff 20130628
        int irVal = -1;//判断是否成套
        public string s_xmdm;//项目代码名称
        public int sxVal=0;//刷新路径维护或路径执行列表的标识


        YidanEHRDataServiceClient serviceCon;
             

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



        private OrderItemCategory m_OrderCategory = OrderItemCategory.Meal;
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

        private OrderPanelBarCategory m_PanelCategory = OrderPanelBarCategory.Meal;
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

        //add by luff 20130628
        public ObservableCollection<CP_DoctorOrder> m_Orderlist = new ObservableCollection<CP_DoctorOrder>();
        public ObservableCollection<CP_DoctorOrder> m_Orderlist2 = new ObservableCollection<CP_DoctorOrder>();
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
        public UCOtherOrder()
        {
            InitializeComponent();
        }
        private  bool isLoad = true; 
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        { 
            try
            {
                //if (!isLoad)
                //{
                //    //isLoad = true;
                //    return;
                //}
                OnAfterDrugLoadedEvent(e);
                //isLoad = false;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void InitPage()
        {
            InitOrderTypeInfo(cbxMDYZBZ); //初始化医嘱类别（临时医嘱，长期医嘱） 
            InitDrugInfo();//初始化药品信息//项目下拉框

            //add by luff 20130118 初始化计价类型和执行科室数据
            IntiComboBoxDept();
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
            //add by luff 20130313 获得配置表关于医嘱可选不算变异参数 若值为1表示可选，0表示必须
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
                radkx.Visibility = Visibility.Collapsed;
                radbx.Visibility = Visibility.Collapsed;
                this.radbx.IsChecked = true;
                this.radkx.IsEnabled = false;
            }


            //InitJJTypeInfo(cbxJJLX);
            //cbxJJLX.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化医嘱标志数据
        /// </summary>
        /// <param name="radcombobox"></param>
        private void InitOrderTypeInfo(RadComboBox radcombobox)
        {
            radcombobox.EmptyText = "请选择医嘱类别";
            List<OrderTypeName> iList = new List<OrderTypeName>();
            iList.Add(new OrderTypeName("临时医嘱", 2702));
            iList.Add(new OrderTypeName("长期医嘱", 2703));
            radcombobox.ItemsSource = iList;
            radcombobox.SelectedIndex = 0;
            radcombobox.IsEnabled = false;
            autoCompleteBoxOrder.Focus();
            //txtZTNR.Focus();

        }
        /// <summary>
        /// 初始化计费类型数据 add by luff 20130118
        /// </summary>
        /// <param name="radcombobox"></param>
        private void InitJJTypeInfo(RadComboBox radcombobox)
        {
            //radcombobox.EmptyText = "请选择计价类型";
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
        /// <summary>
        /// 初始化药品数据
        /// </summary>
        public void InitDrugInfo()
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.GetNormalOrderInfoCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        autoCompleteBoxOrder.ItemsSource = e.Result;
                        autoCompleteBoxOrder.ItemFilter = OrderFilter;
                        //下面这段应该可以去除。。。CHECK下
                        //if (_cp_AdviceGroupDetail.OrderGuid != null && this.ManualType == Helpers.ManualType.Edit)
                        //    InitModifyOrder();
                        if (this.ManualType == Helpers.ManualType.Edit)
                            InitModifyOrder();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                        return;
                    }
                };
            client.GetNormalOrderInfoAsync((int)OrderCategory);
            client.CloseAsync();
        }



        public bool OrderFilter(string strFilter, object item)
        {
            CP_ChargingMinItem deptList = (CP_ChargingMinItem)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())) || (deptList.Wb.StartsWith(strFilter.ToUpper())) || (deptList.Wb.Contains(strFilter.ToUpper())) || (deptList.Name.StartsWith(strFilter.ToUpper())) || (deptList.Name.Contains(strFilter.ToUpper())));
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
                    YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                    StringBuilder strItems = new StringBuilder();
                    s_xmdm = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Sfxmdm;
                    if (s_xmdm.IndexOf('.') > 0)
                    {
                        #region 先判断是单个还是多个成套
                        serviceCon = PublicMethod.YidanClient;
                        serviceCon.GetCTbySxmdmCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    irVal = ea.Result;
                                }
                            };
                        serviceCon.GetCTbySxmdmAsync(((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Sfxmdm);
                        serviceCon.CloseAsync();
                        #endregion
                    }

                    #region 单位
                    this.txtUnitName.Text = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmdw;
                    #endregion

                    #region 数量
                    nudMDSL.Value = 1;
                    #endregion
                    //add by luff 20121108
                    #region 项目单价
                    this.txtXmdj.Text = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmdj.ToString();
                    //this.autoCompleteBoxDept.SelectedItem = ((ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource).FirstOrDefault(cp => cp.Ksdm.Equals(((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Memo));
                    #endregion
                    client.CloseAsync();
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
                autoCompleteBoxOrder.Focus();
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
                    #region add by luff 检验检查套餐处理
                    int iVal = ShowJyJcCT();
                    if (iVal == 1)
                    {
                        if (s_xmdm.IndexOf('.') > 0)
                        {
                            if (this.ManualType == Helpers.ManualType.New)
                            {
                                #region
                                if (irVal == 2)//多个成套
                                {
                                    YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否添加该成套医嘱？", "提示", YiDanMessageBoxButtons.YesNo);
                                    mess.ShowDialog();
                                    mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
                                }
                                else if (irVal == 1)//单个成套
                                {
                                    YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                                    client.GetJyJcMXInfoCompleted += (obj, ea) =>
                                    {
                                        if (ea.Error == null)
                                        {
                                            m_Orderlist = ea.Result;

                                            InitDoctorDrug4Confirm();
                                            OnAfterDrugClosedEvent(CP_AdviceGroupDetailProptery);
                                            NewAdviceGroupDetail();
                                        }
                                    };
                                    client.GetJyJcMXInfoAsync(s_xmdm);
                                }
                                else
                                {
                                    InitDoctorDrug4Confirm();
                                    OnAfterDrugClosedEvent(CP_AdviceGroupDetailProptery);
                                    NewAdviceGroupDetail();
                                }
                                #endregion

                            }
                            else
                            {
                                InitDoctorDrug4Confirm();
                                OnAfterDrugClosedEvent(CP_AdviceGroupDetailProptery);
                                NewAdviceGroupDetail();
                            }
                        }
                        else
                        {
                            InitDoctorDrug4Confirm();
                            OnAfterDrugClosedEvent(CP_AdviceGroupDetailProptery);
                            NewAdviceGroupDetail();
                        }

                    }
                    else if (iVal == 2)//钟山医院需求 将成套医嘱开在一条项目中
                    {
                        InitDoctorDrug4Confirm();
                        if (s_xmdm.IndexOf('.') > 0)
                        {
                            //CP_AdviceGroupDetailProptery.Ypdm = s_xmdm.Substring(s_xmdm.Trim().IndexOf(".") + 1);
                            //CP_AdviceGroupDetailProptery.Ypmc = CP_AdviceGroupDetailProptery.Ypmc.Substring(0, CP_AdviceGroupDetailProptery.Ypmc.Trim().IndexOf("━") + 1).Substring(0, CP_AdviceGroupDetailProptery.Ypmc.Trim().IndexOf("┏") + 1).Substring(0, CP_AdviceGroupDetailProptery.Ypmc.IndexOf("┗") + 1).Substring(0, CP_AdviceGroupDetailProptery.Ypmc.Trim().IndexOf("┃") + 1);

                            if (CP_AdviceGroupDetailProptery.Ypmc.Trim().IndexOf("━") > 0)
                            {
                                CP_AdviceGroupDetailProptery.Ypmc = CP_AdviceGroupDetailProptery.Ypmc.Substring(0, CP_AdviceGroupDetailProptery.Ypmc.Trim().IndexOf("━"));
                            }
                            else if (CP_AdviceGroupDetailProptery.Ypmc.Trim().IndexOf("┏") > 0)
                            {
                                CP_AdviceGroupDetailProptery.Ypmc = CP_AdviceGroupDetailProptery.Ypmc.Substring(0, CP_AdviceGroupDetailProptery.Ypmc.Trim().IndexOf("┏"));
                            }
                            else if (CP_AdviceGroupDetailProptery.Ypmc.Trim().IndexOf("┗") > 0)
                            {
                                CP_AdviceGroupDetailProptery.Ypmc = CP_AdviceGroupDetailProptery.Ypmc.Substring(0, CP_AdviceGroupDetailProptery.Ypmc.Trim().IndexOf("┗"));
                            }
                            else if (CP_AdviceGroupDetailProptery.Ypmc.Trim().IndexOf("┃") > 0)
                            {
                                CP_AdviceGroupDetailProptery.Ypmc = CP_AdviceGroupDetailProptery.Ypmc.Substring(0, CP_AdviceGroupDetailProptery.Ypmc.Trim().IndexOf("┃"));
                            }
                        }
                        OnAfterDrugClosedEvent(CP_AdviceGroupDetailProptery);
                        NewAdviceGroupDetail();
                    }
                    else
                    {
                        InitDoctorDrug4Confirm();
                        OnAfterDrugClosedEvent(CP_AdviceGroupDetailProptery);
                        NewAdviceGroupDetail();
                    }
                    #endregion

                }
                
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
                    
                    RadWindow rWindow = new RWJyJcMx(s_xmdm);
                    rWindow.Closed += new EventHandler<WindowClosedEventArgs>(rWindow_Closed);
                    rWindow.ShowDialog();
                     
                    //this.Close();
                }
                else
                {
                    //单个套餐处理
                    try
                    {
                        
                        //YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                        //client.GetJyJcMXInfoCompleted += (obj, ea) =>
                        //{
                        //    if (ea.Error == null)
                        //    {
                        //        m_Orderlist = ea.Result;

                        //        InitDoctorDrug4Confirm();
                        //        OnAfterDrugClosedEvent(CP_AdviceGroupDetailProptery);
                        //        NewAdviceGroupDetail();
                        //    }
                        //};
                        //client.GetJyJcMXInfoAsync(s_xmdm);
                        InitDoctorDrug4Confirm();
                        OnAfterDrugClosedEvent(CP_AdviceGroupDetailProptery);
                        NewAdviceGroupDetail();
                        
                    }
                    catch (Exception ex)
                    {
                        PublicMethod.ClientException(ex, this.GetType().FullName, true);
                    }
                    //this.DialogResult = true;
                    //this.Close();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void rWindow_Closed(object sender, WindowClosedEventArgs e)
        {
            try
            {

                if (((RWJyJcMx)sender).DialogResult == true)
                {
                    //多个套餐处理 
                    m_Orderlist = ((RWJyJcMx)sender).m_orderList;

                    InitDoctorDrug4Confirm();
                    OnAfterDrugClosedEvent(CP_AdviceGroupDetailProptery);
                    NewAdviceGroupDetail();
                    sxVal = 2;
                    //this.DialogResult = true;
                    //this.Close();
                }
                else
                {
                    //单个套餐处理
                    try
                    {

                        YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                        client.GetJyJcMXInfoCompleted += (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                m_Orderlist = ea.Result;
                                sxVal = 1;

                                InitDoctorDrug4Confirm();
                                OnAfterDrugClosedEvent(CP_AdviceGroupDetailProptery);
                                NewAdviceGroupDetail();
                            }
                        };
                        client.GetJyJcMXInfoAsync(s_xmdm);
                    }
                    catch (Exception ex)
                    {
                        PublicMethod.ClientException(ex, this.GetType().FullName, true);
                    }
                    //刷新路径执行或路径维护列表



                    //this.DialogResult = false;
                    //this.Close();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private bool Check()
        {
            #region 保存之前判断
            if (autoCompleteBoxOrder.SelectedItem == null || cbxMDYZBZ.SelectedItem == null)
            {
                string AlterMessage =
                      (cbxMDYZBZ.SelectedItem == null ? "\r\n" + "医嘱标志必须选择" : "")
                    + (autoCompleteBoxOrder.SelectedItem == null ? "\r\n" + "项目必须选择" : "");
                //Control ct = cbxMDYZBZ.SelectedItem == null ? cbxMDYZBZ : null;
                //if (ct == null)
                //{
                //    ct = autoCompleteBoxOrder.SelectedItem == null ? autoCompleteBoxOrder : null;
                //}
                PublicMethod.RadAlterBoxRe(AlterMessage, m_strTitle, autoCompleteBoxOrder);
                isLoad = false;
                return false;
            }
            List<CP_AdviceGroupDetail> listJudgeSame = cP_AdviceGroupDetailCollection.ToList<CP_AdviceGroupDetail>(); //用于存放Grid数据源

            //if (((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderValue == 2703)    //在添加长期医嘱的时候需要判断是否存在相同的项目
            //{
            for (int i = 0; i < listJudgeSame.Count; i++)
            {
                if (listJudgeSame[i].Ypdm == ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Sfxmdm
                    && listJudgeSame[i].Yzbz == ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderValue)  //  && CurrentState == PageState.New                        
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
        /// 根据配置表判断检验检查是否成套 反正值为1表示成套，0为无成套
        /// </summary>
        private int ShowJyJcCT()
        {
            int irev = 0;
            #region add by luff 20130628 根据配置表判断检验检查是否成套
            try
            {
                List<APPCFG> t_listApp = Global.mAppCfg.Select(s => s).Where(s => s.Configkey.IndexOf("ShowJyJcMx") > -1).ToList();
                if (t_listApp.Count > 0)
                {
                    if (t_listApp[0].Value == "1")//表示检验检查有成套
                    {

                        irev = 1;
                    }
                    else if (t_listApp[0].Value == "2")//钟山医院需求 有成套医嘱 且成套医嘱只开一条
                    {
                        irev = 2;
                    }
                    else
                    {
                        irev = 0;

                    }
                }
                else
                {
                    irev = 0;
                }
            }
            catch (Exception ex)
            {
                irev = 0;
                throw ex;
            }
            return irev;
            #endregion
        }

        /// <summary>
        /// 点击确定时赋值
        /// </summary>
        private void InitDoctorDrug4Confirm()
        {
            if (this.ManualType == Helpers.ManualType.Edit)
            {
                m_Orderlist2 = new ObservableCollection<CP_DoctorOrder>();
                _cp_AdviceGroupDetail.Yzbz = ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderValue;
                _cp_AdviceGroupDetail.YzbzName = ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderName;

                _cp_AdviceGroupDetail.Cdxh = 0;
                _cp_AdviceGroupDetail.Ggxh = 0;
                _cp_AdviceGroupDetail.Lcxh = 0;
                _cp_AdviceGroupDetail.Ypdm = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Sfxmdm;
                _cp_AdviceGroupDetail.Xmlb = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmlb;
                _cp_AdviceGroupDetail.Ypmc = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Name;
                _cp_AdviceGroupDetail.Zxdw = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmdw;
                _cp_AdviceGroupDetail.Ypgg = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmgg;
                _cp_AdviceGroupDetail.Ypjl = Convert.ToDecimal(nudMDSL.Value);
                _cp_AdviceGroupDetail.Ypjl = Convert.ToDecimal(nudMDSL.Value);
                _cp_AdviceGroupDetail.Jldw = txtUnitName.Text;
                _cp_AdviceGroupDetail.Xmdj = Convert.ToDecimal(this.txtXmdj.Text.Trim());
                //_cp_AdviceGroupDetail.Yfdm = ((CP_DrugUseage)cbxMDYF.SelectedItem).Yfdm;
                //_cp_AdviceGroupDetail.YfdmName = ((CP_DrugUseage)cbxMDYF.SelectedItem).Name;
                //_cp_AdviceGroupDetail.Pcdm = ((CP_AdviceFrequency)cbxPC.SelectedItem).Pcdm;
                //_cp_AdviceGroupDetail.PcdmName = ((CP_AdviceFrequency)cbxPC.SelectedItem).Name;
                _cp_AdviceGroupDetail.Lrysdm = Global.LogInEmployee.Zgdm;
                _cp_AdviceGroupDetail.Ztnr = txtZTNR.Text;
                _cp_AdviceGroupDetail.Yzlb = (int)PanelCategory;
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
                _cp_AdviceGroupDetail.Yznr = _cp_AdviceGroupDetail.Ypmc + " " + _cp_AdviceGroupDetail.Zxdw;

                if (this.radkx.IsChecked == true)
                {
                    _cp_AdviceGroupDetail.Yzkx = 1;
                }
                else
                {
                    _cp_AdviceGroupDetail.Yzkx = 0;
                }
                
                m_Orderlist2.Add(_cp_AdviceGroupDetail);
            }
            else
            {
                m_Orderlist2 = new ObservableCollection<CP_DoctorOrder>();

                if (m_Orderlist.Count >= 1)
                {
                    #region 多个或单个成套医嘱处理
                   
                    foreach (CP_DoctorOrder order in m_Orderlist)
                    {
                        #region Cp_DoctorDrug赋值
                        Strzdm = string.Empty;
                        Strzxsj = string.Empty;
                        _cp_AdviceGroupDetail = new CP_DoctorOrder();
                        _cp_AdviceGroupDetail.Syxh = Global.InpatientListCurrent == null ? 0 : Convert.ToDecimal(Global.InpatientListCurrent.Syxh);
                        _cp_AdviceGroupDetail.Bqdm = Global.LogInEmployee.Bqdm;
                        _cp_AdviceGroupDetail.Ksdm = Global.LogInEmployee.Ksdm;
                        _cp_AdviceGroupDetail.Lrysdm = Global.LogInEmployee.Zgdm;
                        _cp_AdviceGroupDetail.Cdxh = 0;
                        _cp_AdviceGroupDetail.Ggxh = 0;
                        _cp_AdviceGroupDetail.Lcxh = 0;

                        _cp_AdviceGroupDetail.Ypdm = order.Ypdm;
                        _cp_AdviceGroupDetail.Xmlb = order.Xmlb;//((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmlb;
                        _cp_AdviceGroupDetail.Zxdw = order.Zxdw;
                        _cp_AdviceGroupDetail.Ypgg = order.Ypgg;
                        _cp_AdviceGroupDetail.Ypmc = order.Ypmc;
                        _cp_AdviceGroupDetail.Xmdj = order.Xmdj; //Convert.ToDecimal(this.txtXmdj.Text.Trim());


                        _cp_AdviceGroupDetail.Yzbz = ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderValue;
                        _cp_AdviceGroupDetail.YzbzName = ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderName;
                        _cp_AdviceGroupDetail.Ypjl = Convert.ToDecimal(nudMDSL.Value);
                        _cp_AdviceGroupDetail.Jldw = txtUnitName.Text;
                       
                         
                        _cp_AdviceGroupDetail.Ksrq = GetDefaultOrderTime((OrderType)(Convert.ToDecimal(_cp_AdviceGroupDetail.Yzbz)));
                       
                        _cp_AdviceGroupDetail.FromTable = string.Empty;
                        _cp_AdviceGroupDetail.Flag = string.Empty;
                        _cp_AdviceGroupDetail.OrderGuid = Guid.NewGuid().ToString();//
                        _cp_AdviceGroupDetail.Fzbz = 3500;
                        
                        _cp_AdviceGroupDetail.Dwxs = 1;//单位系数不知道为何。。。
                       
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

                        #region 判断his是否支持计价类型
                        
                        this.txtjjlx.Visibility = Visibility.Visible;
                        this.cbxJJLX.Visibility = Visibility.Visible;
                        _cp_AdviceGroupDetail.Jjlx = int.Parse(cbxJJLX.SelectedValue.ToString());
                        _cp_AdviceGroupDetail.Jjlxmc = cbxJJLX.Text.ToString();

                        

                        #endregion
                        _cp_AdviceGroupDetail.Zxksdm = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm;
                        _cp_AdviceGroupDetail.Zxksdmmc = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Name;
                        _cp_AdviceGroupDetail.Yznr = _cp_AdviceGroupDetail.Ypmc + " " + _cp_AdviceGroupDetail.Zxdw;
                        _cp_AdviceGroupDetail.Yzzt = 3200;
                        _cp_AdviceGroupDetail.Fzxh = 0;
                        _cp_AdviceGroupDetail.Yzlb = (int)PanelCategory;
                        _cp_AdviceGroupDetail.Zxts = 0;
                        _cp_AdviceGroupDetail.Ypzsl = 0; //出院带药。
                        _cp_AdviceGroupDetail.Ztnr = txtZTNR.Text;

                        if (this.radkx.IsChecked == true)
                        {
                            _cp_AdviceGroupDetail.Yzkx = 1;
                        }
                        else
                        {
                            _cp_AdviceGroupDetail.Yzkx = 0;
                        }
                        #endregion

                        m_Orderlist2.Add(_cp_AdviceGroupDetail);

                    }
                    m_Orderlist = new ObservableCollection<CP_DoctorOrder>();
                    #endregion
                }
                else
                {

                    m_Orderlist = new ObservableCollection<CP_DoctorOrder>();
                    #region Cp_DoctorDrug赋值
                    Strzdm = string.Empty;
                    Strzxsj = string.Empty;
                    _cp_AdviceGroupDetail = new CP_DoctorOrder();
                    _cp_AdviceGroupDetail.Syxh = Global.InpatientListCurrent == null ? 0 : Convert.ToDecimal(Global.InpatientListCurrent.Syxh);
                    _cp_AdviceGroupDetail.Bqdm = Global.LogInEmployee.Bqdm;
                    _cp_AdviceGroupDetail.Ksdm = Global.LogInEmployee.Ksdm;
                    _cp_AdviceGroupDetail.Lrysdm = Global.LogInEmployee.Zgdm;
                    _cp_AdviceGroupDetail.Cdxh = 0;
                    _cp_AdviceGroupDetail.Ggxh = 0;
                    _cp_AdviceGroupDetail.Lcxh = 0;
                    if (s_xmdm.IndexOf(',') > 0)
                    {
                        //serviceCon = PublicMethod.YidanClient;
                         YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                         client.GetSxmdmbyIDCompleted += (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                   
                                    _cp_AdviceGroupDetail.Ypdm = ea.Result.Trim();
                                   
                                }
                            };
                         client.GetSxmdmbyIDAsync(s_xmdm);
                       
                    }
                    else
                    {
                        _cp_AdviceGroupDetail.Ypdm = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Sfxmdm;
                    }
                    _cp_AdviceGroupDetail.Xmlb = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmlb;
                    _cp_AdviceGroupDetail.Yzbz = ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderValue;
                    _cp_AdviceGroupDetail.YzbzName = ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderName;
                    _cp_AdviceGroupDetail.Ypjl = Convert.ToDecimal(nudMDSL.Value);
                    _cp_AdviceGroupDetail.Jldw = txtUnitName.Text;
                    _cp_AdviceGroupDetail.Xmdj = Convert.ToDecimal(this.txtXmdj.Text.Trim());
                    //_cp_AdviceGroupDetail.Yfdm = ((CP_DrugUseage)cbxMDYF.SelectedItem).Yfdm;
                    //_cp_AdviceGroupDetail.YfdmName = ((CP_DrugUseage)cbxMDYF.SelectedItem).Name;
                    //_cp_AdviceGroupDetail.Pcdm = "00";
                    //_cp_AdviceGroupDetail.PcdmName = ((CP_AdviceFrequency)cbxPC.SelectedItem).Name;
                    _cp_AdviceGroupDetail.Ksrq = GetDefaultOrderTime((OrderType)(Convert.ToDecimal(_cp_AdviceGroupDetail.Yzbz)));
                    _cp_AdviceGroupDetail.Ypmc = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Name;
                    _cp_AdviceGroupDetail.FromTable = string.Empty;
                    _cp_AdviceGroupDetail.Flag = string.Empty;
                    _cp_AdviceGroupDetail.OrderGuid = Guid.NewGuid().ToString();//
                    _cp_AdviceGroupDetail.Fzbz = 3500;
                    _cp_AdviceGroupDetail.Zxdw = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmdw;
                    _cp_AdviceGroupDetail.Ypgg = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmgg;
                    _cp_AdviceGroupDetail.Dwxs = 1;//单位系数不知道为何。。。
                    //_cp_AdviceGroupDetail.Dwlb = cbxMDDW.SelectedValue == null ? 3007 : ((DrugUnitsType)cbxMDDW.SelectedItem).UnitID; ;
                    //_cp_AdviceGroupDetail.Zxcs = ((CP_AdviceFrequency)cbxPC.SelectedItem).Zxcs;
                    //_cp_AdviceGroupDetail.Zxzq = ((CP_AdviceFrequency)cbxPC.SelectedItem).Zxzq;
                    //_cp_AdviceGroupDetail.Zxzqdw = ((CP_AdviceFrequency)cbxPC.SelectedItem).Zxzqdw;
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
                    _cp_AdviceGroupDetail.Yznr = _cp_AdviceGroupDetail.Ypmc + " " + _cp_AdviceGroupDetail.Zxdw;
                    _cp_AdviceGroupDetail.Yzzt = 3200;
                    _cp_AdviceGroupDetail.Fzxh = 0;
                    _cp_AdviceGroupDetail.Yzlb = (int)PanelCategory;
                    _cp_AdviceGroupDetail.Zxts = 0;
                    _cp_AdviceGroupDetail.Ypzsl = 0; //出院带药。
                    _cp_AdviceGroupDetail.Ztnr = txtZTNR.Text;

                    if (this.radkx.IsChecked == true)
                    {
                        _cp_AdviceGroupDetail.Yzkx = 1;
                    }
                    else
                    {
                        _cp_AdviceGroupDetail.Yzkx = 0;
                    }
                    #endregion

                    m_Orderlist2.Add(_cp_AdviceGroupDetail);
                }
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
            }
        }
 
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
            //add by yxy  2013-09-04  修改长期医嘱默认开始时间  根据钟山医院需求将默认时间与临时医嘱默认时间改为一样的
            //if (orderType == OrderType.Long)
            //    strTime = Convert.ToString(DateTime.Now.AddDays(1).Date + new TimeSpan(8, 0, 0));
            //else
            //{
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
            //}
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
                cbxMDYZBZ.IsEnabled = true;
                autoCompleteBoxOrder.IsEnabled = true;
                cbxMDYZBZ.SelectedValue = (short)_cp_AdviceGroupDetail.Yzbz;
                //mod by luff 20130826
                if (_cp_AdviceGroupDetail.Ypdm == null || _cp_AdviceGroupDetail.Ypdm == "")
                {
                    autoCompleteBoxOrder.SelectedItem =  null;
                }
                else
                {
                    autoCompleteBoxOrder.SelectedItem = ((ObservableCollection<CP_ChargingMinItem>)autoCompleteBoxOrder.ItemsSource).First(cp => cp.Sfxmdm.Equals(_cp_AdviceGroupDetail.Ypdm));
                }
                nudMDSL.Value = Convert.ToDouble(_cp_AdviceGroupDetail.Ypjl);
                txtUnitName.Text = _cp_AdviceGroupDetail.Zxdw == null ? "" : _cp_AdviceGroupDetail.Zxdw;
                txtXmdj.Text = _cp_AdviceGroupDetail.Xmdj.ToString();
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
                //add by luff 20130314
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

        /// <summary>
        /// 清空控件，暂时PUBLIC，最好改成属性CHANGED 事件触发
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
                txtUnitName.Text = string.Empty;
                this.txtXmdj.Text = "";
                //cbxPC.SelectedValue = null;
                txtZTNR.Text = "";
                zdm.Clear();
                zxsj.Clear();
                zdm = new List<int>();
                zxsj = new List<string>();
                cbxMDYZBZ.IsEnabled = true;
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
                #endregion
                //autoCompleteBoxOrder.Text = string.Empty;
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
            cbxMDYZBZ.KeyUp += new KeyEventHandler(cbxMDYZBZ_KeyUp);
            radbx.KeyUp += new KeyEventHandler(radbx_KeyUp);
            radkx.KeyUp += new KeyEventHandler(radkx_KeyUp);
            autoCompleteBoxOrder.KeyUp += new KeyEventHandler(autoCompleteBoxOrder_KeyUp);
            nudMDSL.KeyUp += new KeyEventHandler(nudMDSL_KeyUp);
            //txtXmdj.KeyUp += new KeyEventHandler(txtXmdj_KeyUp);
            cbxJJLX.KeyUp += new KeyEventHandler(cbxJJLX_KeyUp);
            autoCompleteBoxDept.KeyUp += new KeyEventHandler(autoCompleteBoxDept_KeyUp);
            txtZTNR.KeyUp += new KeyEventHandler(txtZTNR_KeyUp);
            btnMDQD.KeyUp += new KeyEventHandler(btnMDQD_KeyUp);
        }

        /// <summary>
        /// 医嘱类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMDYZBZ_KeyUp(object sender, KeyEventArgs e)
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
                autoCompleteBoxOrder.Focus();
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
                txtZTNR.Focus();
        }
        //add by luff 20121108
        /// <summary>
        /// 项目单价
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        //private void txtXmdj_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //        txtZTNR.Focus();
        //}
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

        private void btnMDQD_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnMDQD_Click(null, null);
        }

        #endregion
    }
}
