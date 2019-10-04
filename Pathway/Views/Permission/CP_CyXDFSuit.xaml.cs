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
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;
using YidanEHRApplication.Views.ChildWindows;
using System.Collections.ObjectModel;
using System.Text;
using YidanSoft.Tool;
using YidanEHRApplication.DataService;
using YidanEHRApplication;
using Telerik.Windows.Controls;

namespace YidanEHRApplication.Views.Permission
{
    public partial class CP_CyXDFSuit : Page
    {

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



        YidanEHRDataServiceClient serviceCon;
        //OperationState _CurrentState = OperationState.VIEW;
        //OperationState CurrentState
        //{
        //    get { return _CurrentState; }
        //    set
        //    {
        //        _CurrentState = value;

        //        if (value == OperationState.NEW)
        //        {


        //            autoCompleteBoxCyf.IsEnabled = true;
        //            autoCompleteBoxOrder.IsEnabled = true;
        //            nudMDSL.IsEnabled = true;
        //            cbxMDDW.IsEnabled = true;
        //            cbxJJLX.IsEnabled = true;
        //            cbxMDYF.IsEnabled = true;
        //            cbxPC.IsEnabled = true;
        //            cbxSJ.IsEnabled = true;
        //            autoCompleteBoxDept.IsEnabled = true;
        //            txtZTNR.IsEnabled = true;

        //            this.btnAdd.IsEnabled = false;
        //            this.btnDel.IsEnabled = false;
        //            this.btnUpdate.IsEnabled = false;

        //            this.btnClear.IsEnabled = true;

        //            this.btnSave.IsEnabled = true;


        //        }
        //        else if (value == OperationState.EDIT)
        //        {
        //            autoCompleteBoxCyf.IsEnabled = false;
        //            autoCompleteBoxOrder.IsEnabled = true;
        //            nudMDSL.IsEnabled = true;
        //            cbxMDDW.IsEnabled = true;
        //            cbxJJLX.IsEnabled = true;
        //            cbxMDYF.IsEnabled = true;
        //            cbxPC.IsEnabled = true;
        //            cbxSJ.IsEnabled = true;
        //            autoCompleteBoxDept.IsEnabled = true;
        //            txtZTNR.IsEnabled = true;

        //            this.btnAdd.IsEnabled = false;
        //            this.btnDel.IsEnabled = false;
        //            this.btnUpdate.IsEnabled = false;

        //            this.btnClear.IsEnabled = true;

        //            this.btnSave.IsEnabled = true;

        //        }
        //        else
        //        {

        //            autoCompleteBoxCyf.IsEnabled = false;
        //            autoCompleteBoxOrder.IsEnabled = false;
        //            nudMDSL.IsEnabled = false;
        //            cbxMDDW.IsEnabled = false;
        //            cbxJJLX.IsEnabled = false;
        //            cbxMDYF.IsEnabled = false;
        //            cbxPC.IsEnabled = false;
        //            cbxSJ.IsEnabled = false;
        //            autoCompleteBoxDept.IsEnabled = false;
        //            txtZTNR.IsEnabled = false;

        //            this.btnAdd.IsEnabled = true;
        //            this.btnDel.IsEnabled = true;
        //            this.btnUpdate.IsEnabled = true;

        //            this.btnClear.IsEnabled = false;

        //            this.btnSave.IsEnabled = false;
        //        }


        //    }
        //}
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

        public CP_CyXDFSuit()
        {
            InitializeComponent();
        }

       

        /// <summary>
        /// 当前树节点选中的节点
        /// </summary>
        RadTreeViewItem m_Selectitem = null;

        List<CP_CYXDF> CyXDFList = new List<CP_CYXDF>();

        /// <summary>
        /// 用于存储列表中选中的行记录
        /// </summary>
        static CP_CYXDF cyxdf = new CP_CYXDF();
        private CP_CYXDFMX m_cymxdetail = new CP_CYXDFMX();

        EditState m_funstate;
        
        void CP_CyXDFSuit_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                m_funstate = EditState.View;
                LoadData();
                BindListBox(0);
                //CurrentState = OperationState.VIEW;
                 
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
                txtZTNR.KeyUp += new KeyEventHandler(btnSave_KeyUp);
                #endregion
                 
                this.cbxJJLX.Visibility = Visibility.Visible;

                InitJJTypeInfo(cbxJJLX);
                cbxJJLX.SelectedIndex = 0;
               
                this.autoCompleteBoxCyf.Focus();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        private void btnSave_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                    btnSave_Click(null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        #region 函数


        private void LoadData()
        {
            try
            {
                YidanEHRDataServiceClient serviceCon = PublicMethod.YidanClient;
                serviceCon.GetCyxdfInfoCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            CyXDFList = e.Result.ToList();
                            InitTreeView();

                            txtQuery.ItemsSource = CyXDFList;
                            txtQuery.ItemFilter = Filter;

                        }
                    };
                serviceCon.GetCyxdfInfoAsync();
                serviceCon.CloseAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private bool Filter(string strFilter, object item)
        {
            CP_CYXDF deptList = (CP_CYXDF)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())) || deptList.ID.ToString().StartsWith(strFilter.ToUpper()) || deptList.ID.ToString().Contains(strFilter.ToUpper()));
        }

        HashSet<string> rooth;
        HashSet<string> userh;
        Dictionary<string, RadTreeViewItem> dicCyXDF;
        RadTreeViewItem rootItem;
        RadTreeViewItem userItem;
        private void InitTreeView()
        {
            try
            {
                treeViewUser.Items.Clear();
                rooth = new HashSet<string>();
                userh = new HashSet<string>();
                dicCyXDF = new Dictionary<string, RadTreeViewItem>();
                rootItem = null;
                userItem = null;
                foreach (CP_CYXDF pu in CyXDFList)
                {
                    if (!rooth.Contains(pu.ID.ToString().Trim()))
                    {
                         
                        rooth.Add(pu.cfmc.Trim());
                        rootItem = AddItem(pu.cfmc, pu, null);
                        
                    }
                    //if (!dicCyXDF.ContainsKey(pu.UserID))
                    //{
                    //    userh.Add(pu.UserName);
                    //    userItem = AddItem(pu.UserID + " " + pu.UserName, pu, rootItem);
                    //    dicCyXDF.Add(pu.UserID, userItem);
                    //}
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private RadTreeViewItem AddItem(string header, CP_CYXDF cyxdf, RadTreeViewItem treeViewItem)
        {

            RadTreeViewItem radTreeItem = (new RadTreeViewItem()
            {
                Header = header,
            });
            try
            {
                radTreeItem.Tag = cyxdf;
                if (treeViewItem == null)
                {
                    treeViewItem = radTreeItem;
                    this.treeViewUser.Items.Add(radTreeItem);
                }
                else
                {
                    treeViewItem.Items.Add(radTreeItem);
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
            return radTreeItem;


        }



        private void InitUserInfo(CP_CYXDF cyxdf)
        {
            try
            {
                
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void GridViewCymx_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            //if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
            //CP_CYXDFMX t = (CP_CYXDFMX)e.DataElement;
            //List<CheckBox> listtest = (List<CheckBox>)(this.GridViewCymx.ChildrenOfType<CheckBox>().ToList());
            //if (listtest.Count > 0)
            //    if (t.IsCheck == 1)
            //        listtest[listtest.Count - 1].IsChecked = true;
        }

        /// <summary>
        /// 绑定页面中编辑信息
        /// </summary>
        /// <param name="cyfmx">实体，如果为空编辑区域值为空</param>
        private void Bindcyfmx(CP_CYXDF cyxdf)
        {
            if (cyxdf != null)
            {
                BindListBox(cyxdf.ID);
            }
            else
            {
                cyxdf = new CP_CYXDF();
                //this.txtDept.Text = null;
                //this.txtName.Text = null;
            }
        }

        /// <summary>
        /// 根据草药协定方编号ID绑定草药明细列表
        /// </summary>
        /// <param name="UserID"></param>
        private void BindListBox(int iID)
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetCyxdfMXInfoByIdCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        GridViewCymx.ItemsSource = e.Result.ToList();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            serviceCon.GetCyxdfMXInfoByIdAsync(iID);
            serviceCon.CloseAsync();
        }

        /// <summary>
        /// 根据草药协定方编号ID绑定草药明细列表
        /// </summary>
        /// <param name="UserID"></param>
        private void BindGridView()
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetCyxdfMXInfoCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        GridViewCymx.ItemsSource = e.Result.ToList();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            serviceCon.GetCyxdfMXInfoAsync();
            serviceCon.CloseAsync();
        }

        /// <summary>
        /// 重新加载刷新树
        /// </summary>
        /// <param name="UserID"></param>
        private void BindGridTree()
        {
            //treeViewUser.SelectedItem = null;
            LoadData();
            InitCyfInfo();//初始化草药处方信息
        }
        #endregion

        #region 函数

       
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

        /// <summary>
        /// 添加按钮事件 
        /// </summary>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                #region
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
                this.btnUpdate.IsEnabled = false;
                this.ScrollViewerRole.IsEnabled = true;
                this.btnCancel.IsEnabled = true;
                this.btnSave.IsEnabled = true;

                #endregion
                if (treeViewUser.SelectedItem == null)
                {
                    NewAdviceGroupDetail();
                    BindGridView();
                }
                else
                {
                     int  iCyxdfID =0; 
                    RadTreeViewItem item = this.treeViewUser.SelectedItem as RadTreeViewItem;
                    CP_CYXDF cyxdf = item.Tag as CP_CYXDF;
                    iCyxdfID = cyxdf.ID;
                    BindListBox(iCyxdfID);
                    autoCompleteBoxCyf.SelectedItem = ((ObservableCollection<CP_CYXDF>)autoCompleteBoxCyf.ItemsSource).First(cp => cp.ID.Equals(cyxdf.ID));
                    autoCompleteBoxCyf.IsEnabled = false;
                    autoCompleteBoxOrder.SelectedItem = null;
                    autoCompleteBoxOrder.Text = "";
                    nudMDSL.Value = 1;
                    //nudMDTS.Value = 1;
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

                
               
               
               
                autoCompleteBoxCyf.Focus();
                m_funstate = EditState.Add;
                

            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            
            try
            {
                if (this.GridViewCymx.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选中要修改的行！", "提示");
                    return;
                }

                m_cymxdetail = (CP_CYXDFMX)GridViewCymx.SelectedItem;
                m_funstate = EditState.Edit;

                #region
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
                this.btnUpdate.IsEnabled = false;
                this.ScrollViewerRole.IsEnabled = true;
                this.btnCancel.IsEnabled = true;
                this.btnSave.IsEnabled = true;

                #endregion

                autoCompleteBoxCyf.Focus();
                
                

            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (this.treeViewUser.SelectedItem == null)
                //{
                //    PublicMethod.RadAlterBox("请选择草药协定方！", "提示");
                //    return;
                //}
                int CyxdfID = 0;
                if (this.treeViewUser.SelectedItem != null)
                {
                    RadTreeViewItem item = this.treeViewUser.SelectedItem as RadTreeViewItem;
                    CP_CYXDF cyxdf = item.Tag as CP_CYXDF;
                    CyxdfID = cyxdf.ID;
                }
                

                CP_CYXDFMX _cymxdetail = new CP_CYXDFMX();
                #region 验证数据并初始化

                if (autoCompleteBoxCyf.SelectedItem == null)
                {
                    //PublicMethod.RadAlterBoxRe("请选择草药处方名", "提示", autoCompleteBoxCyf);
                    //isTrue = false;
                    YiDanMessageBox.Show("请选择草药处方名！", autoCompleteBoxCyf);
                    return;
                }
                if (autoCompleteBoxOrder.SelectedItem == null)
                {
                    //PublicMethod.RadAlterBoxRe("请选择草药明细名称", "提示", autoCompleteBoxOrder);
                    //isTrue = false;
                    YiDanMessageBox.Show("请选择草药明细名称！", autoCompleteBoxOrder);
                    return;
                }

                if (cbxMDYF.SelectedItem == null || cbxMDYF.Text == "")
                {
                    //PublicMethod.RadAlterBoxRe("请输入草药药品用法", "提示", cbxMDYF);
                    //isTrue = false;
                    YiDanMessageBox.Show("请输入草药药品用法！", cbxMDYF);
                    return;
                }
                if (cbxPC.SelectedItem == null || cbxPC.Text == "")
                {
                    //PublicMethod.RadAlterBoxRe("请输入草药使用频次", "提示", cbxPC);
                    //isTrue = false;
                    YiDanMessageBox.Show("请输入草药使用频次！", cbxPC);
                    return;
                }


                if (cbxSJ.SelectedItem == null || cbxSJ.Text == "")
                {
                    //PublicMethod.RadAlterBoxRe("请输入草药使用周期", "提示", cbxSJ);
                    //isTrue = false;
                    YiDanMessageBox.Show("请输入草药使用周期！", cbxSJ);
                    return;
                }

               
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
                if (m_funstate == EditState.Add)
                {


                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.InsertCYXDFMXCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Result == 1)
                            {
                                
                                //PublicMethod.RadAlterBox("保存成功！", "提示");
                                YiDanMessageBox.Show("保存成功！", YiDanMessageBoxButtons.Ok);
                                if (CyxdfID == 0)
                                {
                                    BindGridView();
                                }
                                else
                                {
                                    BindListBox(CyxdfID);
                                }
                                m_funstate = EditState.View;
                                this.btnAdd.IsEnabled = true;
                                this.btnUpdate.IsEnabled = true;
                                this.btnSave.IsEnabled = false;
                                this.ScrollViewerRole.IsEnabled = true;
                                this.btnCancel.IsEnabled = false;
                                //清空控件
                                NewAdviceGroupDetail();
                            }
                            
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.InsertCYXDFMXAsync(_cymxdetail);
                    serviceCon.CloseAsync();


                   // m_funstate = EditState.View;
                }
                else if (m_funstate == EditState.Edit)
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
                            m_funstate = EditState.View;
                            this.btnAdd.IsEnabled = true;
                            this.btnUpdate.IsEnabled = true;
                            this.btnSave.IsEnabled = false;
                            this.ScrollViewerRole.IsEnabled = true;
                            this.btnCancel.IsEnabled = false;

                            //PublicMethod.RadAlterBox("修改成功", "提示");
                            YiDanMessageBox.Show("修改成功！", YiDanMessageBoxButtons.Ok);
                            if (CyxdfID == 0)
                            {
                                BindGridView();
                            }
                            else
                            {
                                BindListBox(CyxdfID);
                            }
                            NewAdviceGroupDetail();
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(ea.Error);
                        }
                    };
                    Client.UpdateCYXDFMXAsync(_cymxdetail);


                  
                }
                // 
                else
                {
                    //PublicMethod.RadAlterBox(" 请选择草药协定方！", "提示");
                    YiDanMessageBox.Show("请选择草药协定方！", YiDanMessageBoxButtons.Ok);
                    return;
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {

            #region
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
            this.btnUpdate.IsEnabled = true;
            this.btnSave.IsEnabled = false;
            this.ScrollViewerRole.IsEnabled = true;
            this.btnCancel.IsEnabled = false;
            m_funstate = EditState.View;
            #endregion
            
        }

        private void Query_Click(object sender, RoutedEventArgs e)
        {
            if (this.txtQuery.Text == "" || this.txtQuery.Text == null)
            {
                //PublicMethod.RadAlterBox("请输入草药协定方！", "提示");
                return;
            }
            string key = this.txtQuery.Text;
            foreach (CP_CYXDF pu in CyXDFList)
            {
                if (dicCyXDF.ContainsKey(key))
                {
                    this.treeViewUser.SelectedItem = dicCyXDF[key];
                    return;
                }
                if (pu.cfmc == key)
                {
                    this.treeViewUser.SelectedItem = dicCyXDF[pu.ID.ToString()];
                    return;
                }

            }
        }

        /// <summary>
        /// 查询下拉框锁定对应树节点中的草药方
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtQuery_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (txtQuery.SelectedItem == null)
                    return;
                CP_CYXDF cyxdfs = (CP_CYXDF)txtQuery.SelectedItem;

                //定位treeview中对应节点
                RadTreeViewItem item1 = treeViewUser.GetItemByPath(string.Format("{0}\\{1}", cyxdfs.ID.ToString(), cyxdfs.cfmc));
                if (item1 != null)
                {
                    treeViewUser.CollapseAll();
                    treeViewUser.SelectedItem = item1;

                    txtQuery.Text = cyxdfs.cfmc;
                    return;
                }
                //if (item1 != null && item1.Tag != null)
                //{
                //    treeViewUser.CollapseAll();
                //    treeViewUser.SelectedItem = item1;

                //    txtQuery.Text = cyxdfs.cfmc;
                //    return;
                //}
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }

        /// <summary>
        /// 树节点中切换草药协定方明细绑定对应信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void treeViewUser_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (treeViewUser.SelectedItem == null)
                    return;
                RadTreeViewItem item = (RadTreeViewItem)treeViewUser.SelectedItem;
                //判断如果当前在编辑状态则不能选中树节点
                if (m_funstate != EditState.View && treeViewUser.SelectedItem != m_Selectitem)
                {
                    treeViewUser.SelectedItem = m_Selectitem;
                    YiDanMessageBox.Show("当前为编辑状态不能切换，如需切换草药方明细请保存或者取消更改！", YiDanMessageBoxButtons.Ok);
                    return;
                }

                //if (item.Tag == null) return;


                CP_CYXDF cyxdf = item.Tag as CP_CYXDF;
                m_Selectitem = item;
                InitUserInfo(cyxdf);
                Bindcyfmx(cyxdf);
                //NewAdviceGroupDetail();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

       
        /// <summary>
        /// 行选中改变事件
        /// </summary>
        private void GridViewCymx_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {

                if (GridViewCymx.SelectedItem == null)
                {
                    return;
                }
                if (m_funstate == EditState.View)
                {
                    m_cymxdetail = (CP_CYXDFMX)GridViewCymx.SelectedItem;
                    Bind_Data(m_cymxdetail);
                }

            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
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
            //nudMDTS.Value = 1;
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
            //刷新树
            BindGridTree();

        }

    }
}
