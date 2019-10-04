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
using YidanEHRApplication.Models;


namespace YidanEHRApplication.Views.UserControls
{
    public partial class UCChunOrder : UserControl
    {
        public UCChunOrder()
        {
            InitializeComponent();
        }
        private const string m_strTitle = "医嘱提示"; //定义弹出框标题栏
        //定义一个全局集合类型，用于从检验检测中取纯医嘱的数据源
        private ObservableCollection<CP_ChargingMinItem> m_CP_ChargingMinItemCollection;
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
        
        bool isLoad = true;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isLoad)
                {
                    //isLoad = true;
                    return;
                }
                OnAfterDrugLoadedEvent(e);
                isLoad = false;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void InitPage()
        {
            InitOrderTypeInfo(cbxMDYZBZ); //初始化医嘱类别（临时医嘱，长期医嘱） 
            cbxMDYZBZ.SelectedIndex = 0;
            InitDrugInfo();
            //add by luff 20130315 医嘱是否可选，可选不算变异，前台会显示控件
            #region 
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

            #endregion

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
            this.autoCompleteBoxOrder.Focus();

        }

        /// <summary>
        /// 初始化纯医嘱数据
        /// </summary>
        private void InitDrugInfo()
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.GetNormalOrderInfoCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        
                        m_CP_ChargingMinItemCollection = e.Result;
                        autoCompleteBoxOrder.ItemsSource = (IEnumerable<CP_ChargingMinItem>)m_CP_ChargingMinItemCollection.Select(s => s).Where(s => s.Xmdj == 0);
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
            client.GetNormalOrderInfoAsync((int)OrderItemCategory.Other );
            client.CloseAsync();
        }



        public bool OrderFilter(string strFilter, object item)
        {
            CP_ChargingMinItem deptList = (CP_ChargingMinItem)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())));
        }


        private List<DrugUnitsType> m_ListDrugUnit = new List<DrugUnitsType>();
        private void InitUnitType(string strName, decimal unitID)
        {
            DrugUnitsType type = new DrugUnitsType(strName, unitID);
            m_ListDrugUnit.Add(type);
        }
        
        //重置
        private void btnMDXYZ_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewAdviceGroupDetail();
                this.autoCompleteBoxOrder.Focus();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        //确定
        private void btnMDQD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Check())
                {
                    InitDoctorDrug6Confirm();
                    OnAfterDrugClosedEvent(CP_AdviceGroupDetailProptery);
                    NewAdviceGroupDetail();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        ///  提交纯医嘱之前的判断
        /// </summary>
        /// <returns></returns>
        private bool Check()
        {
            #region 保存之前判断
            if (cbxMDYZBZ.SelectedItem == null || this.autoCompleteBoxOrder.Text == "")
            {
                string AlterMessage =
                      (cbxMDYZBZ.SelectedItem == null ? "\r\n" + "医嘱标志必须选择" : "")
                    + (autoCompleteBoxOrder.Text.Trim() == "" ? "\r\n" + "嘱托内容不能为空" : "");

                PublicMethod.RadAlterBoxRe(AlterMessage, m_strTitle, autoCompleteBoxOrder);
                isLoad = false;
                return false;
            }
            //List<CP_AdviceGroupDetail> listJudgeSame = cP_AdviceGroupDetailCollection.ToList<CP_AdviceGroupDetail>(); //用于存放Grid数据源

            //for (int i = 0; i < listJudgeSame.Count; i++)
            //{
            //    if (listJudgeSame[i].Ztnr.Trim() == this.txtZTNR.Text.Trim())                       
            //    {
            //        PublicMethod.RadAlterBox("存在相同项目医嘱,无法继续添加", m_strTitle);
            //        isLoad = false;
            //        return false;
            //    }
            //}
            //}
            #endregion
            return true;
        }

        private void autoCompleteBoxOrder_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            
        }

        /// <summary>
        /// 点击确定时赋值
        /// </summary>
        private void InitDoctorDrug6Confirm()
        {
            if (this.ManualType == Helpers.ManualType.Edit)
            {
                _cp_AdviceGroupDetail.Yzbz = ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderValue;
                _cp_AdviceGroupDetail.YzbzName = ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderName;

                _cp_AdviceGroupDetail.Cdxh = 0;
                _cp_AdviceGroupDetail.Ggxh = 0;
                _cp_AdviceGroupDetail.Lcxh = 0;
                _cp_AdviceGroupDetail.Ypdm = string.Empty;//((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Sfxmdm;
                _cp_AdviceGroupDetail.Xmlb = 0;// ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmlb;
                _cp_AdviceGroupDetail.Ypmc ="纯医嘱";//((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Name;
                _cp_AdviceGroupDetail.Zxdw = string.Empty;// ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmdw;
                _cp_AdviceGroupDetail.Ypgg = string.Empty;// ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmgg;
                _cp_AdviceGroupDetail.Ypjl = 0;// Convert.ToDecimal(nudMDSL.Value);
                _cp_AdviceGroupDetail.Ypjl = 0;// Convert.ToDecimal(nudMDSL.Value);
                _cp_AdviceGroupDetail.Jldw = string.Empty;// txtUnitName.Text;
                _cp_AdviceGroupDetail.Lrysdm = Global.LogInEmployee.Zgdm;
                //_cp_AdviceGroupDetail.Ztnr = txtZTNR.Text;
                //mod by luff 20130103
                if (autoCompleteBoxOrder.SelectedItem != null)
                {

                    _cp_AdviceGroupDetail.Ztnr = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Name;
                    _cp_AdviceGroupDetail.Yznr = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Name;
                }
                else //用于 手动输入纯医嘱
                {
                    _cp_AdviceGroupDetail.Ztnr = autoCompleteBoxOrder.Text.Trim();
                    _cp_AdviceGroupDetail.Yznr = autoCompleteBoxOrder.Text.Trim();
                }
                _cp_AdviceGroupDetail.Yzlb = (int)PanelCategory;
                //_cp_AdviceGroupDetail.Yznr = txtZTNR.Text;//_cp_AdviceGroupDetail.Ypmc + " " + _cp_AdviceGroupDetail.Zxdw;

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
                _cp_AdviceGroupDetail.Syxh = Global.InpatientListCurrent == null ? 0 : Convert.ToDecimal(Global.InpatientListCurrent.Syxh);
                _cp_AdviceGroupDetail.Bqdm = Global.LogInEmployee.Bqdm;
                _cp_AdviceGroupDetail.Ksdm = Global.LogInEmployee.Ksdm;
                _cp_AdviceGroupDetail.Lrysdm = Global.LogInEmployee.Zgdm;
                _cp_AdviceGroupDetail.Cdxh = 0;
                _cp_AdviceGroupDetail.Ggxh = 0;
                _cp_AdviceGroupDetail.Lcxh = 0;
                _cp_AdviceGroupDetail.Ypdm = string.Empty;//((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Sfxmdm;
                _cp_AdviceGroupDetail.Xmlb = 0;//((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmlb;
                _cp_AdviceGroupDetail.Yzbz = ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderValue;
                _cp_AdviceGroupDetail.YzbzName =((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderName;
                _cp_AdviceGroupDetail.Ypjl = 0;//Convert.ToDecimal(nudMDSL.Value);
                _cp_AdviceGroupDetail.Jldw = string.Empty;//txtUnitName.Text;
                
                _cp_AdviceGroupDetail.Ksrq = GetDefaultOrderTime((OrderType)(Convert.ToDecimal(_cp_AdviceGroupDetail.Yzbz)));
                _cp_AdviceGroupDetail.Ypmc = "纯医嘱";//((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Name;
                _cp_AdviceGroupDetail.FromTable = string.Empty;
                _cp_AdviceGroupDetail.Flag = string.Empty;
                _cp_AdviceGroupDetail.OrderGuid = Guid.NewGuid().ToString(); 
                _cp_AdviceGroupDetail.Fzbz = 3500;
                _cp_AdviceGroupDetail.Zxdw = string.Empty; //((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmdw;
                _cp_AdviceGroupDetail.Ypgg = string.Empty; //((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmgg;
                _cp_AdviceGroupDetail.Dwxs = 1; 
               
                //foreach (int i in zdm)
                //{
                //    Strzdm += i.ToString();
                //}
                //_cp_AdviceGroupDetail.Zdm = (zdm.Count == 0 ? null : Strzdm);
                //foreach (string s in zxsj)
                //{
                //    if (s != "," && s != "")
                //    {
                //        Strzxsj += s;
                //    }
                //}
                _cp_AdviceGroupDetail.Zxsj = (zxsj.Count == 0 ? null : Strzxsj);

                if (this.radkx.IsChecked == true)
                {
                    _cp_AdviceGroupDetail.Yzkx = 1;
                }
                else
                {
                    _cp_AdviceGroupDetail.Yzkx = 0;
                }
               // _cp_AdviceGroupDetail.Yznr = txtZTNR.Text; //_cp_AdviceGroupDetail.Ypmc + " " + _cp_AdviceGroupDetail.Zxdw;
                _cp_AdviceGroupDetail.Yzzt = 3200;
                _cp_AdviceGroupDetail.Fzxh = 0;
                _cp_AdviceGroupDetail.Yzlb = (int)PanelCategory;
                _cp_AdviceGroupDetail.Zxts = 0;
                _cp_AdviceGroupDetail.Ypzsl = 0; //出院带药。
                //_cp_AdviceGroupDetail.Ztnr = txtZTNR.Text;
                //mod by luff 20130103
                if (autoCompleteBoxOrder.SelectedItem != null)
                {

                    _cp_AdviceGroupDetail.Ztnr = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Name;
                    _cp_AdviceGroupDetail.Yznr = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Name;
                }
                else //用于 手动输入纯医嘱
                {
                    _cp_AdviceGroupDetail.Ztnr = autoCompleteBoxOrder.Text.Trim();
                    _cp_AdviceGroupDetail.Yznr = autoCompleteBoxOrder.Text.Trim();
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
        // 控件在父窗体加载窗体时，调用该事件
        protected virtual void OnAfterDrugLoadedEvent(RoutedEventArgs e)
        {
            if (AfterDrugLoadedEvent != null)
            {
                InitPage();//初始化基础数据
                RegisterKeyEvent();
            }
        }
        //定义委托和事件 初始化
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
        /// 初始化需要修改的纯医嘱
        /// </summary>
        public void InitModifyOrder()
        {
            try
            {
                cbxMDYZBZ.IsEnabled = false;
                cbxMDYZBZ.SelectedValue = (short)_cp_AdviceGroupDetail.Yzbz;
                //mod by luff 20130103

                if (((IEnumerable<CP_ChargingMinItem>)autoCompleteBoxOrder.ItemsSource).Select(s => s).Where(s => s.Name == _cp_AdviceGroupDetail.Ztnr).Count() == 0)
                {
                    autoCompleteBoxOrder.SelectionChanged -=  new System.Windows.Controls.SelectionChangedEventHandler(autoCompleteBoxOrder_SelectionChanged);
                    autoCompleteBoxOrder.Text = _cp_AdviceGroupDetail.Ztnr;
                    autoCompleteBoxOrder.DataContext = _cp_AdviceGroupDetail.Ztnr;
                    autoCompleteBoxOrder.FilterMode = AutoCompleteFilterMode.None;
                    autoCompleteBoxOrder.SelectionChanged += new System.Windows.Controls.SelectionChangedEventHandler(autoCompleteBoxOrder_SelectionChanged);
                }
                else  
                {
                    autoCompleteBoxOrder.SelectedItem = ((IEnumerable<CP_ChargingMinItem>)autoCompleteBoxOrder.ItemsSource).First(cp => cp.Name.Equals(_cp_AdviceGroupDetail.Ztnr));
                }
                //txtZTNR.Text = _cp_AdviceGroupDetail.Ztnr == null ? string.Empty : _cp_AdviceGroupDetail.Ztnr;
                //add by luff 20130118
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
                autoCompleteBoxOrder.Text = "";
                autoCompleteBoxOrder.SelectedItem = null;
                //zdm.Clear();
                //zxsj.Clear();
                //zdm = new List<int>();
                //zxsj = new List<string>();
                cbxMDYZBZ.IsEnabled = true;
                //add by luff 20130315 医嘱是否可选，可选不算变异，前台会显示控件
                #region
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

                #endregion
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
            autoCompleteBoxOrder.KeyUp += new KeyEventHandler(autoCompleteBoxOrder_KeyUp);
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
                autoCompleteBoxOrder.Focus();
        }
 
        /// <summary>
        /// 嘱托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoCompleteBoxOrder_KeyUp(object sender, KeyEventArgs e)
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
