using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Collections.ObjectModel;
using System.Text;
using System.Collections;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows;
using YidanEHRApplication.Models;
using YidanEHRApplication.WorkFlow.Designer;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Views.UserControls;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.NurModule;
using YidanEHRApplication.Views.ChildWindows;
using YidanSoft.Tool;
using System.Windows.Media;
namespace YidanEHRApplication.Views
{
    /// <summary>
    /// Interaction logic for PathNodeSettings.xaml
    /// </summary>
    public partial class RWPathCutEdit
    {
        Boolean IsPostBack = false;
        /// <summary>
        /// 路径什么审核标示
        /// </summary>
        bool m_bAduit = false;
        public RWPathCutEdit()
        {
            InitializeComponent();
            this.Width = Application.Current.Host.Content.ActualWidth - 40;
            this.Height = Application.Current.Host.Content.ActualHeight - 40;
        }
        #region 常量初始及构造函数
        #region const
        const string HeaderText = "成套医嘱提示"; //定义弹出框标题栏
        const string NodeUnSelect = "需要先保存节点信息";
        const string DisAdviceGroupFail = "所选数据没有成组医嘱需要取消";
        #endregion
        PageState currentState;
        private PageState CurrentState
        {
            get { return currentState; }
            set { currentState = value; }
        }
        StringBuilder nodestr = new StringBuilder();  //用于保存节点信息 
        private bool m_IsEditEnable = true;
        /// <summary>
        /// 是否可编辑
        /// </summary>
        public bool IsEditEnable
        {
            get { return m_IsEditEnable; }
            set { m_IsEditEnable = value; }
        }
        #region xjt
        private ContainerEditPathCut m_WorkFlowContainerPathCut;
        private CP_ClinicalPathList m_clinicalPathInfo;
        public CP_ClinicalPathList ClinicalPathInfo
        {
            get
            {
                return m_clinicalPathInfo;
            }
        }
        public RWPathCutEdit(CP_ClinicalPathList clinicalPathInfo, List<CP_AdviceGroupPathCut> agPathCut)
            : this()
        {
            m_bAduit = (clinicalPathInfo.Yxjl == "审核") ? true : false;
            m_clinicalPathInfo = clinicalPathInfo;
            _Path = clinicalPathInfo;
            InitOrderControls();
            // RowReorderBehavior.SetIsEnabled(OrderGrid, true);

            adviceGroupPathCut = agPathCut;     //接过医嘱     ZM
            coloringExecute(StackPanelPathCut);//面板色彩       ZM
        }
        /// <summary>
        /// 初始化医嘱输入控件
        /// </summary>
        private void InitOrderControls()
        {
            InitRisLisControl();
            InitMealControl();
            InitObservationControl();
            InitActivityControl();
            InitCareControl();
        }
        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                m_WorkFlowContainerPathCut = new ContainerEditPathCut();
                m_WorkFlowContainerPathCut.WorkFlowUrlID = m_clinicalPathInfo.Ljdm;
                m_WorkFlowContainerPathCut.WorkFlowUrlName = m_clinicalPathInfo.Name;
                m_WorkFlowContainerPathCut.WorkFlowXML = m_clinicalPathInfo.WorkFlowXML;
                m_WorkFlowContainerPathCut.IsEditEnable = IsEditEnable;
                gridWorkFlow.Width = this.Width;
                gridWorkFlow.HorizontalAlignment = HorizontalAlignment.Left;
                gridWorkFlow.VerticalAlignment = VerticalAlignment.Top;
                m_WorkFlowContainerPathCut.Width = this.Width;
                m_WorkFlowContainerPathCut.HorizontalAlignment = HorizontalAlignment.Left;
                m_WorkFlowContainerPathCut.VerticalAlignment = VerticalAlignment.Top;
                gridWorkFlow.Children.Add(m_WorkFlowContainerPathCut);
                m_WorkFlowContainerPathCut.AfterSelectActivityEvent += new ContainerEditPathCut.AfterSelectActivityEventHandler(WorkFlowContainer_AfterSelectActivityEvent);
                m_WorkFlowContainerPathCut.IsEnabled = false;                                      //ZM    6.13添加
                SetControlEnable(IsEditEnable);
                RegisterKeyEvent();
                GetNurBasicInfo();
                GetPathVariationBasicInfo();
                InitCondition();
                m_WorkFlowContainerPathCut.IsEnabled = true;                                       //ZM    6.13添加
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void SetControlEnable(bool isEnable)
        {
            btnSSQD.IsEnabled = IsEditEnable;
            btnMDQD.IsEnabled = IsEditEnable;
            btnNurSave.IsEnabled = IsEditEnable;
            btnVariationSave.IsEnabled = IsEditEnable;
        }
        #region  工作流配置相关
        /// <summary>
        /// 选中工作流的结点
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void WorkFlowContainer_AfterSelectActivityEvent(object sender, ActivitySelectEventArgs e)
        {
            try
            {
                if (e != null)
                {
                    //根据获取的DATA做操作
                    nodestr = new StringBuilder();
                    nodestr.Append(e.SelectAcivity.SelectID);
                    nodestr.Append("&");   //特殊字符区分填写的节点
                    nodestr.Append(e.SelectAcivity.SelectName);
                    GetDrugInfo();
                    GetNurPathInfo(m_NurExecInfos, e.SelectAcivity.SelectID);
                    GetVariationToPathInfo(m_PathVariation, e.SelectAcivity.SelectID);

                    AdviceGroupPathCut("");               //根据节点加载裁剪医嘱    ZM
                }
                else
                {
                    nodestr = new StringBuilder();
                    nodestr.Append(NodeUnSelect);
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        ///  窗体关闭后更新主界面XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadWindow_Closed(object sender, Telerik.Windows.Controls.WindowClosedEventArgs e)
        {
            try
            {
                if (m_WorkFlowContainerPathCut != null && m_clinicalPathInfo != null)
                    m_clinicalPathInfo.WorkFlowXML = m_WorkFlowContainerPathCut.WorkFlowXML;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        #endregion  工作流配置相关
        #endregion
        private List<CP_AdviceGroupPathCut> adviceGroupPathCut; //裁剪医嘱 ZM
        #endregion
        #region 事件&&函数
        #region 药品
        #region 药品变量
        List<int> zdm = new List<int>();  //存储周代码，存储当前选中的星期
        List<string> zxsj = new List<string>();//存储执行时间，存储当前选中的时间
        string Strzdm = string.Empty;    //zdm字符串形式
        string Strzxsj = string.Empty; //zxsj字符串形式
        ObservableCollection<CP_AdviceGroupDetail> cP_AdviceGroupDetailCollection = new ObservableCollection<CP_AdviceGroupDetail>();
        List<CP_PCSJ> cplist = new List<CP_PCSJ>();
        #endregion
        #region 药品属性
        CP_AdviceGroupDetail _cp_AdviceGroupDetail = new CP_AdviceGroupDetail();
        public CP_AdviceGroupDetail CP_AdviceGroupDetailProptery
        {
            get
            {
                #region Cp_AdviceGroupDetail赋值
                Strzdm = string.Empty;
                Strzxsj = string.Empty;
                _cp_AdviceGroupDetail = new CP_AdviceGroupDetail();
                _cp_AdviceGroupDetail.Fzxh = 1;
                _cp_AdviceGroupDetail.Yzbz = ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderValue;
                _cp_AdviceGroupDetail.Ypdm = ((CP_PlaceOfDrug)cbxMDYPMC.SelectedItem).Ypdm;
                _cp_AdviceGroupDetail.Cdxh = ((CP_PlaceOfDrug)cbxMDYPMC.SelectedItem).Cdxh;
                _cp_AdviceGroupDetail.Ggxh = ((CP_PlaceOfDrug)cbxMDYPMC.SelectedItem).Ggxh;
                _cp_AdviceGroupDetail.Lcxh = ((CP_PlaceOfDrug)cbxMDYPMC.SelectedItem).Lcxh;
                _cp_AdviceGroupDetail.Ypmc = ((CP_PlaceOfDrug)cbxMDYPMC.SelectedItem).Ypmc;
                _cp_AdviceGroupDetail.Xmlb = ((CP_PlaceOfDrug)cbxMDYPMC.SelectedItem).Yplb;
                _cp_AdviceGroupDetail.Zxdw = ((CP_PlaceOfDrug)cbxMDYPMC.SelectedItem).Zxdw;
                _cp_AdviceGroupDetail.Ypjl = Convert.ToDecimal(nudMDSL.Value);
                _cp_AdviceGroupDetail.Jldw = cbxMDDW.Text;
                _cp_AdviceGroupDetail.Dwxs = 1;//单位系数不知道为何。。。
                _cp_AdviceGroupDetail.Dwlb = 3007; //此处测试写死
                _cp_AdviceGroupDetail.Yfdm = ((CP_DrugUseage)cbxMDYF.SelectedItem).Yfdm;
                _cp_AdviceGroupDetail.Pcdm = ((CP_AdviceFrequency)cbxPC.SelectedItem).Pcdm;
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

                                _cp_AdviceGroupDetail.Jjlx = cbxJJLX.SelectedIndex == -1 ? 1 : int.Parse(cbxJJLX.SelectedValue.ToString());
                                //_cp_AdviceGroupDetail.Jjlxmc = cbxJJLX.Text.ToString();

                //            }
                //            else
                //            {
                //                this.txtjjlx.Visibility = Visibility.Collapsed;
                //                this.cbxJJLX.Visibility = Visibility.Collapsed;
                //                _cp_AdviceGroupDetail.Jjlx = 1;
                //                //_cp_AdviceGroupDetail.Jjlxmc = "";
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
                //_cp_AdviceGroupDetail.Zxksdmmc = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Name; 
                _cp_AdviceGroupDetail.Zxsj = (zxsj.Count == 0 ? null : Strzxsj);
                _cp_AdviceGroupDetail.Zxts = 0;
                _cp_AdviceGroupDetail.Ypzsl = 0; //出院带药。
                _cp_AdviceGroupDetail.Ztnr = txtZTNR.Text;
                _cp_AdviceGroupDetail.Yzlb = Convert.ToInt16(((RadPanelBarItem)this.OrderPanelBar.SelectedItem).Tag.ToString());

                if (this.radbx.IsChecked == true)
                {
                    _cp_AdviceGroupDetail.Yzkx = 0;
                }
                else
                {
                    _cp_AdviceGroupDetail.Yzkx = 1;
                }

                #endregion
                return _cp_AdviceGroupDetail;
            }
            set
            {
                _cp_AdviceGroupDetail = value;
            }
        }
        #endregion
        #region 药品事件
        /// <summary>
        /// 右键打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMenuOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogParameters parameters = new DialogParameters();
                if (false)
                {
                    RadContextMenu menu = sender as RadContextMenu;
                    menu.IsOpen = false;
                    //parameters.Content = String.Format("提示: {0}", "是否保存当前更改的数据？");
                    //parameters.Header = HeaderText;
                    //parameters.IconContent = null;
                    //parameters.OkButtonContent = "确定";
                    //parameters.CancelButtonContent = "取消";
                    //parameters.Closed = OnEditAdviceGroupDetail;//***close处理***
                    //RadWindow.Confirm(parameters);
                    YidanPathWayMessageBox mess = new YidanPathWayMessageBox("是否保存当前更改的数据？", "提示", YiDanMessageBoxButtons.YesNo);
                    mess.ShowDialog();
                    mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);

                }
                else
                {
                    GridViewRow row = ((RadRoutedEventArgs)e).OriginalSource as GridViewRow;
                    List<CP_AdviceGroupDetail> listsOrder = new List<CP_AdviceGroupDetail>();
                    var RadMenu = sender as RadContextMenu;
                    Boolean IsSelectItemsHaveDefferentGroup = false;
                    Boolean IsSelectItemsHaveGroupItem = false;
                    foreach (RadMenuItem item in RadMenu.Items)
                    {
                        if (row != null && !row.IsSelected)
                        {
                            item.IsEnabled = false;
                        }
                        else
                            if (item.Tag != null)
                            {
                                if ((TagName)item.Tag == TagName.Edit)
                                {
                                    item.IsEnabled = !(this.GridViewYZXX.SelectedItems.Count > 1);
                                }
                                if ((TagName)item.Tag == TagName.Group)
                                {
                                    #region 判断选中的数据中是否同时包含长期医嘱和临时医嘱，是否包含已成组项目
                                    for (int i = 0; i < GridViewYZXX.SelectedItems.Count; i++)
                                    {
                                        if (i > 0 && !IsSelectItemsHaveDefferentGroup)
                                            IsSelectItemsHaveDefferentGroup =
                                                ((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i - 1]).Yzbz != ((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i]).Yzbz;
                                        if (((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i]).Fzbz != 3500) IsSelectItemsHaveGroupItem = true;
                                    }
                                    #endregion
                                    item.IsEnabled = (this.GridViewYZXX.SelectedItems.Count > 1 && !IsSelectItemsHaveDefferentGroup && !IsSelectItemsHaveGroupItem);
                                }
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 右键项目事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMenuItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RadMenuItem clickedItem = ((RadRoutedEventArgs)e).OriginalSource as RadMenuItem;
                if (clickedItem != null)
                {
                    TagName tagName = (TagName)clickedItem.Tag;
                    object selectedItem = GridViewYZXX.SelectedItem;
                    DialogParameters parameters = new DialogParameters();
                    switch (tagName)
                    {
                        case TagName.New:
                            NewAdviceGroupDetail();//清空Form
                            break;
                        case TagName.Edit:
                            CurrentState = PageState.Edit;
                            zdm.Clear();
                            zxsj.Clear();
                            #region 绑定Form
                            _cp_AdviceGroupDetail = this.GridViewYZXX.SelectedItem as CP_AdviceGroupDetail;
                            cbxMDYZBZ.SelectedValue = (short)_cp_AdviceGroupDetail.Yzbz;
                            //cbxMDYPMC.SelectedValue = _cp_AdviceGroupDetail.Ypdm;
                            cbxMDYPMC.SelectedItem = ((ObservableCollection<CP_PlaceOfDrug>)cbxMDYPMC.ItemsSource).First(cp => cp.Ypdm.Equals(_cp_AdviceGroupDetail.Ypdm));
                            nudMDSL.Value = Convert.ToDouble(_cp_AdviceGroupDetail.Ypjl);
                            cbxMDDW.Text = _cp_AdviceGroupDetail.Jldw;
                            cbxMDYF.SelectedValue = _cp_AdviceGroupDetail.Yfdm;
                            cbxPC.SelectedValue = _cp_AdviceGroupDetail.Pcdm;
                            txtZTNR.Text = _cp_AdviceGroupDetail.Ztnr;
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
                            #endregion
                            break;
                        case TagName.Del:
                            foreach (CP_AdviceGroupDetail cp in this.GridViewYZXX.SelectedItems)
                            {
                                if (cp.Flag != "")
                                {
                                    PublicMethod.RadAlterBox("存在分组不能删除", HeaderText);
                                    return;
                                }
                            }
                            //parameters.Content = String.Format("提示: {0}", "确认删除吗？");
                            //parameters.Header = HeaderText;
                            //parameters.IconContent = null;
                            //parameters.OkButtonContent = "确定";
                            //parameters.CancelButtonContent = "取消";
                            //parameters.Closed = OnDelAdviceGroupDetail;//***close处理***
                            //RadWindow.Confirm(parameters);
                            YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                            mess.ShowDialog();
                            mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent3);
                            break;
                        case TagName.Group:
                            #region 处理选择的数据
                            /* Line1014-1042 处理选择头尾数据两行数据，将中间数据放入到List中  */
                            List<CP_AdviceGroupDetail> listsOrder = new List<CP_AdviceGroupDetail>();
                            List<int> arrayList = new List<int>();
                            List<CP_AdviceGroupDetail> lists = new List<CP_AdviceGroupDetail>();
                            int k = 0;
                            lists = cP_AdviceGroupDetailCollection.ToList();
                            foreach (CP_AdviceGroupDetail list in lists)
                            {
                                for (int i = 0; i < GridViewYZXX.SelectedItems.Count; i++)
                                {
                                    if (((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i]).Index == list.Index)
                                    {
                                        listsOrder.Add(list);
                                        arrayList.Add(k);
                                    }
                                }
                                k++;
                            }
                            //if (arrayList.Count > 0)
                            //{
                            //    if ((int)arrayList[arrayList.Count - 1] - (int)arrayList[0] != arrayList.Count - 1)
                            //    {
                            //        listsOrder = new System.Collections.Generic.List<CP_AdviceGroupDetail>();
                            //        for (int j = (int)arrayList[0]; j <= (int)arrayList[arrayList.Count - 1]; j++)
                            //        {
                            //            listsOrder.Add(lists[j]);
                            //        }
                            //    }
                            //}
                            #endregion
                            string FirstSingle = string.Empty;
                            List<decimal> MiddleSingle = new List<decimal>();
                            string LastSingle = string.Empty;
                            if (PublicMethod.CheckCommonPropertiesIsSame(listsOrder) != null) //首先检查是否可以成组
                            {
                                for (int i = 0; i < listsOrder.Count; i++)
                                {
                                    if (i == 0)
                                    {
                                        FirstSingle = listsOrder[i].Ctmxxh.ToString(); //第一条
                                    }
                                    if (i == listsOrder.Count - 1)
                                    {
                                        LastSingle = listsOrder[i].Ctmxxh.ToString(); //最后一条
                                    }
                                    else if (i != 0 && i != listsOrder.Count - 1)
                                    {
                                        MiddleSingle.Add(listsOrder[i].Ctmxxh);//取出中间条数
                                    }
                                }
                                YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                                ServiceClient.AdviceGroupCompleted +=
                                    (obj, ea) =>
                                    {
                                        if (ea.Error == null)
                                        {
                                            
                                            BindGridData(((RadPanelBarItem)this.OrderPanelBar.SelectedItem).Tag.ToString(), nodestr.ToString().Split('&')[0].ToString());
                                            zdm.Clear();
                                            zxsj.Clear();
                                            PublicMethod.RadAlterBox(ea.Result, HeaderText);
                                        }
                                        else
                                        {
                                            PublicMethod.RadWaringBox(ea.Error);
                                        }
                                    };
                                ServiceClient.AdviceGroupAsync(FirstSingle, LastSingle, ToObservableCollection(MiddleSingle));
                            }
                            else
                            {
                                PublicMethod.RadAlterBox("所选医嘱中【用法、频次】存在不一致，无法成组", HeaderText);
                                return;
                            }
                            break;
                        case TagName.DisGroup:
                            List<decimal> FzxhList = new List<decimal>();
                            foreach (CP_AdviceGroupDetail cp in this.GridViewYZXX.SelectedItems)
                            {
                                if (cp.Flag != "")
                                    FzxhList.Add(cp.Fzxh);
                            }
                            if (FzxhList.Count > 0)
                            {
                                YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                                ServiceClient.DisAdviceGroupCompleted +=
                                    (obj, ea) =>
                                    {
                                        if (ea.Error == null)
                                        {
                                            
                                            BindGridData(((RadPanelBarItem)this.OrderPanelBar.SelectedItem).Tag.ToString(), nodestr.ToString().Split('&')[0].ToString());
                                            PublicMethod.RadAlterBox(ea.Result, HeaderText);
                                        }
                                        else
                                        {
                                            PublicMethod.RadWaringBox(ea.Error);
                                        }
                                    };
                                ServiceClient.DisAdviceGroupAsync(ToObservableCollection(FzxhList));
                            }
                            else
                            {
                                PublicMethod.RadAlterBox(DisAdviceGroupFail, HeaderText);
                            }
                            break;
                        case TagName.SelectMuti:
                            GridViewYZXX.UnselectAll();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 药品选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoMDYPMC_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                string SelectItem = ((RadPanelBarItem)this.OrderPanelBar.SelectedItem).Tag.ToString(); //panelbar选择的项目
                if (cbxMDYPMC.SelectedItem != null)
                {
                    StringBuilder StrItems = new StringBuilder();
                    switch (SelectItem)
                    {
                        case "3100":
                            #region 单位
                            //药品医嘱时的单位填充数据为规格单位和最小单位
                            StrItems.Append(((CP_PlaceOfDrug)cbxMDYPMC.SelectedItem).Ggdw.ToString());
                            StrItems.Append(",");
                            StrItems.Append(((CP_PlaceOfDrug)cbxMDYPMC.SelectedItem).Zxdw.ToString());
                            cbxMDDW.Items.Clear();
                            for (int i = 0; i < StrItems.ToString().Split(',').Length; i++)
                            {
                                RadComboBoxItem item = new RadComboBoxItem();
                                if (!string.IsNullOrEmpty(StrItems.ToString().Split(',')[i].ToString()))
                                {
                                    item.Content = StrItems.ToString().Split(',')[i].ToString();
                                    if (i == 0)
                                    {
                                        item.IsSelected = true;
                                    }
                                    cbxMDDW.Items.Add(item);
                                }
                            }
                            #endregion
                            #region 数量
                            nudMDSL.Value = 1;
                            #endregion
                            #region 用法
                            ServiceClient.GetDrugUseageCompleted +=
                                (obj, ea1) =>
                                {
                                    if (ea1.Error == null)
                                    {
                                        cbxMDYF.ItemsSource = ea1.Result;
                                        cbxMDYF.SelectedValue = _cp_AdviceGroupDetail.Yfdm;
                                        if (ea1.Result.Count > 0)
                                        {
                                            cbxMDYF.SelectedIndex = 0;
                                        }
                                    }
                                    else
                                    {
                                        PublicMethod.RadWaringBox(ea1.Error);
                                    }
                                };
                            ServiceClient.GetDrugUseageAsync(((CP_PlaceOfDrug)cbxMDYPMC.SelectedItem).Jxdm);
                            #endregion
                            #region 频次代码
                            if (cbxPC.Items.Count == 0)
                            {
                                ServiceClient.GetAdviceFrequencyCompleted +=
                                    (obj, ea2) =>
                                    {
                                        if (ea2.Error == null)
                                        {
                                            cbxPC.ItemsSource = ea2.Result;
                                            cbxPC.SelectedValue = _cp_AdviceGroupDetail.Pcdm;
                                            if (ea2.Result.Count > 0)
                                            {
                                                cbxPC.SelectedIndex = 0;
                                            }
                                        }
                                        else
                                        {
                                            PublicMethod.RadWaringBox(ea2.Error);
                                        }
                                    };
                                ServiceClient.GetAdviceFrequencyAsync(null);
                            }
                            else
                            {
                                cbxPC.SelectedValue = _cp_AdviceGroupDetail.Pcdm;
                                cbxPC.SelectedIndex = 0;
                            }
                            #endregion
                            break;
                        case "3105":
                            nudMDSL.Value = 1;
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                }
                ServiceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 确定保存医嘱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnMDQD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                #region 保存之前判断
                if (nodestr.ToString() == NodeUnSelect)
                {
                    PublicMethod.RadAlterBox(NodeUnSelect, HeaderText);
                    return;
                }
                if (this.OrderPanelBar.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择医嘱类别如：药品等", HeaderText);
                    return;
                }
                if (cbxMDYPMC.SelectedItem == null || cbxMDYF.SelectedItem == null || cbxPC.SelectedItem == null || cbxMDYZBZ.SelectedItem == null || cbxSJ.SelectedItem == null)
                {
                    string AlterMessage =
                          (cbxMDYZBZ.SelectedItem == null ? "\r\n" + "医嘱标志必须选择" : "")
                        + (cbxMDYPMC.SelectedItem == null ? "\r\n" + "项目必须选择" : "")
                        + (cbxMDYF.SelectedItem == null ? "\r\n" + "用法必须选择" : "")
                        + (cbxPC.SelectedItem == null ? "\r\n" + "频次必须选择" : "")
                         + (cbxSJ.SelectedItem == null ? "\r\n" + "频次时间必须选择" : "");
                    PublicMethod.RadAlterBox(AlterMessage, HeaderText);
                    return;
                }
                if (zdm.Count != GetZdmCount((List<CP_PCSJ>)cbxSJ.ItemsSource))
                {
                    PublicMethod.RadAlterBox("超出或者低于周代码限制数,限制为【" + GetZdmCount((List<CP_PCSJ>)cbxSJ.ItemsSource).ToString() + "】周", HeaderText);
                    return;
                }
                else
                {
                    //if (zxsj.Count != GetZxsjCount((List<CP_PCSJ>)SJ.ItemsSource))
                    //{
                    //    pub.RadAlterBox("超出或低于执行时间限制数,限制为【" + GetZxsjCount((List<CP_PCSJ>)SJ.ItemsSource).ToString() + "】次", HeaderText);
                    //    return;
                    //}
                }
                List<CP_AdviceGroupDetail> listJudgeSame = cP_AdviceGroupDetailCollection.ToList<CP_AdviceGroupDetail>(); //用于存放Grid数据源
                //if (((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderValue == 2703)    //在添加长期医嘱的时候需要判断是否存在相同的项目
                //{
                for (int i = 0; i < listJudgeSame.Count; i++)
                {
                    if (listJudgeSame[i].Ypdm == ((CP_PlaceOfDrug)cbxMDYPMC.SelectedItem).Ypdm
                        && listJudgeSame[i].Yzbz == ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderValue
                        && CurrentState == PageState.New)
                    {
                        PublicMethod.RadAlterBox("存在相同项目医嘱,无法继续添加", HeaderText);
                        return;
                    }
                }
                //}
                #endregion
                Save();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 新医嘱
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
        /// 频 次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMDPC_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (cbxPC.SelectedItem != null)
                {
                    zdm.Clear();
                    zxsj.Clear();
                    string pcdm = ((CP_AdviceFrequency)cbxPC.SelectedItem).Pcdm;
                    YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                    ServiceClient.GetDropDownInfoCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                cbxSJ.ItemsSource = ea.Result.ToList();
                                zdm.Clear();
                                zxsj.Clear();
                                if (ea.Result.Count > 0)
                                {
                                    cbxSJ.SelectedIndex = 0;
                                }
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    ServiceClient.GetDropDownInfoAsync(pcdm);
                    ServiceClient.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #region checkbox
        /// <summary>
        /// 周代码checkBox的加载匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
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
        /// <summary>
        /// 时间CheckBox匹配
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void CheckBox_Loaded_1(object sender, RoutedEventArgs e)
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
        private void CheckBox_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox ck = sender as CheckBox;
            zdm.Add(Convert.ToInt32(ck.Tag.ToString()));
        }
        private void CheckBox_Unchecked(object sender, RoutedEventArgs e)
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
        private void CheckBox_Checked_1(object sender, RoutedEventArgs e)
        {
            CheckBox ck = sender as CheckBox;
            if (!zxsj.Contains(ck.Content.ToString() + ","))
            {
                zxsj.Add(ck.Content.ToString() + ",");
            }
        }
        private void CheckBox_Unchecked_1(object sender, RoutedEventArgs e)
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
        #endregion
        #region 药品函数
        public void Save()
        {
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            if (CurrentState == PageState.Edit)
            {
                if (CP_AdviceGroupDetailProptery.Flag != "")
                {
                    //todo by zhouhui 此处的分组校验的原因是?!
                    //List<CP_AdviceGroupDetail> editlist = new List<CP_AdviceGroupDetail>();
                    //editlist = cP_AdviceGroupDetailCollection.ToList();
                    //CP_AdviceGroupDetail cp = editlist.First(delegate(CP_AdviceGroupDetail a) { return a.Fzxh == CP_AdviceGroupDetailProptery.Fzxh; }); //获取正在编辑数据的对象
                    //CP_AdviceGroupDetail cp = cP_AdviceGroupDetailCollection.Where(c => c.Fzxh.Equals(CP_AdviceGroupDetailProptery.Fzxh)).First();
                    //if (((CP_PlaceOfDrug)cbxMDYPMC.SelectedItem).Yplb != cp.Xmlb)
                    //{
                    //    PublicMethod.RadAlterBox("需要项目类别和同组其它成员一样", HeaderText);
                    //    return;
                    //}
                    //else
                    {
                        ServiceClient.UpdateAdviceGroupDetailCompleted +=
                                (obj, e) =>
                                {
                                    if (e.Error == null)
                                    {
                                        
                                        BindGridData(CP_AdviceGroupDetailProptery.Yzlb.ToString(), nodestr.ToString().Split('&')[0].ToString());
                                        NewAdviceGroupDetail();
                                        PublicMethod.RadAlterBox(e.Result, HeaderText);
                                    }
                                    else
                                    {
                                        PublicMethod.RadWaringBox(e.Error);
                                    }
                                };
                        //ServiceClient.UpdateAdviceGroupDetailAsync(CP_AdviceGroupDetailProptery.Fzxh, CP_AdviceGroupDetailProptery.Ctmxxh, CP_AdviceGroupDetailProptery, CP_AdviceGroupDetailProptery.Flag != "");
                        ServiceClient.UpdateAdviceGroupDetailAsync(CP_AdviceGroupDetailProptery.Fzxh, ((CP_AdviceGroupDetail)this.GridViewYZXX.SelectedItem).Ctmxxh, CP_AdviceGroupDetailProptery, CP_AdviceGroupDetailProptery.Flag != "");
                    }
                }
                else
                {
                    ServiceClient.UpdateAdviceGroupDetailCompleted +=
                            (obj, e) =>
                            {
                                if (e.Error == null)
                                {
                                    
                                    BindGridData(CP_AdviceGroupDetailProptery.Yzlb.ToString(), nodestr.ToString().Split('&')[0].ToString());
                                    NewAdviceGroupDetail();
                                    PublicMethod.RadAlterBox(e.Result, HeaderText);
                                }
                                else
                                {
                                    PublicMethod.RadWaringBox(e.Error);
                                }
                            };
                    ServiceClient.UpdateAdviceGroupDetailAsync(CP_AdviceGroupDetailProptery.Fzxh, CP_AdviceGroupDetailProptery.Ctmxxh, CP_AdviceGroupDetailProptery, CP_AdviceGroupDetailProptery.Flag != "");
                }
            }
            if (CurrentState == PageState.New)
            {
                ServiceClient.InsertIntoAdviceGroupCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            
                            BindGridData(CP_AdviceGroupDetailProptery.Yzlb.ToString(), nodestr.ToString().Split('&')[0].ToString());
                            NewAdviceGroupDetail();
                            PublicMethod.RadAlterBox(e.Result, HeaderText);
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
                ServiceClient.InsertIntoAdviceGroupAsync(CP_AdviceGroupDetailProptery, nodestr.ToString().Split('&')[0].ToString(), 3500, nodestr.ToString().Split('&')[1].ToString(), m_clinicalPathInfo.Ljdm); // 3500分组标志
            }
            ServiceClient.CloseAsync();
        }
        /// <summary>
        /// 药品数据绑定
        /// </summary>
        /// <param name="selectitem"></param>
        /// <param name="nodeid"></param>
        public void BindGridData(string selectitem, string nodeid)
        {
            if (!radBusyIndicator.IsBusy)
                radBusyIndicator.IsBusy = true;
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.GetAdviceGroupDetailInfoCompleted +=
                    (obj, e) =>
                    {
                        radBusyIndicator.IsBusy = false;
                        if (e.Error == null)
                        {
                            string ss = GridViewYZXX.GroupDescriptors.ToString();
                            GridViewYZXX.ItemsSource = SettingSource(e.Result);//对数据源进行设置
                            cP_AdviceGroupDetailCollection = e.Result;
                            zdm = new List<int>();
                            zxsj = new List<string>();
                            if (GridViewYZXX.GroupDescriptors.Count == 0)
                            {
                                Telerik.Windows.Data.GroupDescriptor gd = new Telerik.Windows.Data.GroupDescriptor();
                                gd.Member = "YzbzName";
                                GridViewYZXX.GroupDescriptors.Add(gd);
                            }
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
            client.GetAdviceGroupDetailInfoAsync(selectitem, nodeid,m_clinicalPathInfo.Ljdm);
            client.CloseAsync();
        }
        /// <summary>
        /// 清空控件
        /// </summary>
        void NewAdviceGroupDetail()
        {
            CP_AdviceGroupDetailProptery = new CP_AdviceGroupDetail();
            cbxMDYZBZ.SelectedValue = null;
            //cbxMDYPMC.SelectedValue = null;
            cbxMDYPMC.SelectedItem = null;
            nudMDSL.Value = 0;
            cbxMDDW.Text = "";
            cbxJJLX.SelectedIndex = 0;
            autoCompleteBoxDept.SelectedItem = null;
            autoCompleteBoxDept.Text = "";
            CurrentState = PageState.New;
            cbxMDDW.SelectedValue = null;
            cbxSJ.SelectedValue = null;
            cbxMDYF.SelectedValue = null;
            cbxPC.SelectedValue = null;
            txtZTNR.Text = "";
            zdm.Clear();
            zxsj.Clear();
            zdm = new List<int>();
            zxsj = new List<string>();

            this.radbx.IsChecked = false;
            this.radkx.IsChecked = false;
        }
        /// <summary>
        /// 删除行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDelAdviceGroupDetail(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                List<decimal> listid = new List<decimal>(); //用于保存删除的数据行主键
                for (int i = 0; i < this.GridViewYZXX.SelectedItems.Count; i++)
                {
                    listid.Add(((CP_AdviceGroupDetail)this.GridViewYZXX.SelectedItems[i]).Ctmxxh);//取出要删除数据行的主键。
                }
                YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                ServiceClient.DelAdviceGroupDetailCompleted +=
                     (obj, ea) =>
                     {
                         if (ea.Error == null)
                         {
                             
                             BindGridData(((RadPanelBarItem)this.OrderPanelBar.SelectedItem).Tag.ToString(), nodestr.ToString().Split('&')[0].ToString());
                             PublicMethod.RadAlterBox(ea.Result, HeaderText);
                         }
                         else
                         {
                             PublicMethod.RadWaringBox(ea.Error);
                         }
                     };
                ServiceClient.DelAdviceGroupDetailAsync(ToObservableCollection(listid)); // DelAdviceGroupDetailAsync 函数需要ObservableCollection 类型参数
            }
            else
            {
            }
        }
        void mess_PageClosedEvent3(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    List<decimal> listid = new List<decimal>(); //用于保存删除的数据行主键
                    for (int i = 0; i < this.GridViewYZXX.SelectedItems.Count; i++)
                    {
                        listid.Add(((CP_AdviceGroupDetail)this.GridViewYZXX.SelectedItems[i]).Ctmxxh);//取出要删除数据行的主键。
                    }
                    YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                    ServiceClient.DelAdviceGroupDetailCompleted +=
                         (obj, ea) =>
                         {
                             if (ea.Error == null)
                             {

                                 BindGridData(((RadPanelBarItem)this.OrderPanelBar.SelectedItem).Tag.ToString(), nodestr.ToString().Split('&')[0].ToString());
                                 PublicMethod.RadAlterBox(ea.Result, HeaderText);
                             }
                             else
                             {
                                 PublicMethod.RadWaringBox(ea.Error);
                             }
                         };
                    ServiceClient.DelAdviceGroupDetailAsync(ToObservableCollection(listid)); // DelAdviceGroupDetailAsync 函数需要ObservableCollection 类型参数

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
                    Save();

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void OnEditAdviceGroupDetail(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
                Save();
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

        }
        /// <summary>
        /// 初始化药品数据
        /// </summary>
        private void InitDrugInfo()
        {
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            ServiceClient.GetDrugInfoCompleted +=
                (obj, e) =>
                {
                    //cbxMDYPMC.ItemsSource = e.Result;
                    //cbxMDYPMC.EmptyText = "请选择药品";
                    if (e.Error == null)
                    {
                        cbxMDYPMC.ItemsSource = e.Result;
                        cbxMDYPMC.ItemFilter = OrderFilter;
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                        return;
                    }
                };
            ServiceClient.GetDrugInfoAsync(null);
            ServiceClient.CloseAsync();
        }
        public bool OrderFilter(string strFilter, object item)
        {
            CP_PlaceOfDrug deptList = (CP_PlaceOfDrug)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())));
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
                            autoCompleteBoxDept.ItemFilter = DeptFilterKs;
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
        public bool DeptFilterKs(string strFilter, object item)
        {
            CP_DepartmentList deptList = (CP_DepartmentList)item;
            return ((deptList.QueryName.StartsWith(strFilter.ToUpper())) || (deptList.QueryName.Contains(strFilter.ToUpper())));
        }
        #endregion
        /// <summary>
        /// 格式化Grid数据 加入分组标志
        /// </summary>
        /// <param name="obj">数据源</param>
        /// <returns></returns>
        ObservableCollection<CP_AdviceGroupDetail> SettingSource(ObservableCollection<CP_AdviceGroupDetail> obj)
        {
            List<CP_AdviceGroupDetail> tempsource = new List<CP_AdviceGroupDetail>();
            tempsource = obj.ToList();
            var c = new ObservableCollection<CP_AdviceGroupDetail>();
            foreach (var e in tempsource)
            {
                if (e.Fzbz == 3501)        //组开始
                {
                    e.Flag = "┓";
                }
                else if (e.Fzbz == 3502) //组中间
                {
                    e.Flag = "┃";
                }
                else if (e.Fzbz == 3599) //组结束
                {
                    e.Flag = "┛";
                }
                else
                {
                    e.Flag = "";
                }
                c.Add(e);
            }
            return c;
        }
        #endregion
        #endregion
        #region 手术
        #region 手术变量
        /// <summary>
        /// 用于存放此类型的数据
        /// </summary>
        ObservableCollection<CP_AdviceGroupDetail> CP_AdviceGroupDetailCollection = new ObservableCollection<CP_AdviceGroupDetail>();
        CP_AdviceGroupDetail CP_AdviceGroupDetail = new CP_AdviceGroupDetail();
        #endregion
        #region 手术事件
        /// <summary>
        /// 手术保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSSQD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (nodestr.ToString() == NodeUnSelect)
                {
                    PublicMethod.RadAlterBox(NodeUnSelect, HeaderText);
                    return;
                }
                if (this.OrderPanelBar.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择医嘱类别如：手术等", HeaderText);
                    return;
                }
                if (cbxSSMC.SelectedItem == null || cbxSSMZ.SelectedItem == null)
                {
                    string AlterMessage =
                          (cbxSSMC.SelectedItem == null ? "\r\n" + "项目必须选择" : "")
                        + (cbxSSMZ.SelectedItem == null ? "\r\n" + "麻醉代码必须选择" : "");
                    PublicMethod.RadAlterBox(AlterMessage, HeaderText);
                    return;
                }
                List<CP_AdviceGroupDetail> listJudgeSame = new List<CP_AdviceGroupDetail>();
                foreach (CP_AdviceGroupDetail cp in CP_AdviceGroupDetailCollection)
                {
                    listJudgeSame.Add(cp);
                }
                for (int i = 0; i < listJudgeSame.Count; i++)
                {
                    if (listJudgeSame[i].Ypdm == ((CP_Operation)cbxSSMC.SelectedItem).Ssdm && CurrentState == PageState.New)
                    {
                        PublicMethod.RadAlterBox("存在相同项目医嘱,无法继续添加", HeaderText);
                        return;
                    }
                }
                if (CurrentState == PageState.Edit)
                {
                    YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                    ServiceClient.UpdateAdviceGroupDetailCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                
                               // BindSSGridData(nodestr.ToString().Split('&')[0].ToString());
                                BindGridData(((RadPanelBarItem)this.OrderPanelBar.SelectedItem).Tag.ToString(), nodestr.ToString().Split('&')[0].ToString());
                                NewSSAdviceDetail();
                                PublicMethod.RadAlterBox(ea.Result, HeaderText);
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    CP_AdviceGroupDetail = this.GridViewYZXX.SelectedItem as CP_AdviceGroupDetail;
                    CP_AdviceGroupDetail.Ypdm = ((CP_Operation)cbxSSMC.SelectedItem).Ssdm;
                    CP_AdviceGroupDetail.Ypmc = ((CP_Operation)cbxSSMC.SelectedItem).Name;
                    CP_AdviceGroupDetail.Mzdm = ((CP_Anesthesia)cbxSSMZ.SelectedItem).Mzdm;
                    CP_AdviceGroupDetail.Xmlb = (short)((CP_Operation)cbxSSMC.SelectedItem).Sslb;
                    CP_AdviceGroupDetail.Ztnr = txtSSZTNR.Text;
                    ServiceClient.UpdateAdviceGroupDetailAsync(CP_AdviceGroupDetail.Fzxh, CP_AdviceGroupDetail.Ctmxxh, CP_AdviceGroupDetail, false);
                }
                if (CurrentState == PageState.New)
                {
                    //Insert
                    CP_AdviceGroupDetail = new CP_AdviceGroupDetail();
                    CP_AdviceGroupDetail.Yzbz = ((OrderTypeName)cbxSSYZBZ.SelectedItem).OrderValue;
                    CP_AdviceGroupDetail.Fzxh = 1;
                    CP_AdviceGroupDetail.Ypdm = ((CP_Operation)cbxSSMC.SelectedItem).Ssdm;
                    CP_AdviceGroupDetail.Ypmc = ((CP_Operation)cbxSSMC.SelectedItem).Name;
                    CP_AdviceGroupDetail.Mzdm = ((CP_Anesthesia)cbxSSMZ.SelectedItem).Mzdm;
                    CP_AdviceGroupDetail.Xmlb = (short)((CP_Operation)cbxSSMC.SelectedItem).Sslb;
                    CP_AdviceGroupDetail.Zxdw = null;
                    CP_AdviceGroupDetail.Dwlb = 0;
                    CP_AdviceGroupDetail.Ztnr = txtSSZTNR.Text;
                    CP_AdviceGroupDetail.Yzlb = Convert.ToInt16(((RadPanelBarItem)this.OrderPanelBar.SelectedItem).Tag.ToString());
                    YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                    ServiceClient.InsertIntoAdviceGroupCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                
                                //BindSSGridData(nodestr.ToString().Split('&')[0].ToString());
                                BindGridData(((RadPanelBarItem)this.OrderPanelBar.SelectedItem).Tag.ToString(), nodestr.ToString().Split('&')[0].ToString());
                                PublicMethod.RadAlterBox(ea.Result, HeaderText);
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    ServiceClient.InsertIntoAdviceGroupAsync(CP_AdviceGroupDetail, nodestr.ToString().Split('&')[0].ToString(), 3500, nodestr.ToString().Split('&')[1].ToString(), m_clinicalPathInfo.Ljdm);
                    NewSSAdviceDetail();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnSSXYZ_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewSSAdviceDetail();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 手术右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSSMenuOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogParameters parameters = new DialogParameters();
                if (false)//CurrentState == PageState.Edit)
                {
                    RadContextMenu menu = sender as RadContextMenu;
                    menu.IsOpen = false;
                    //parameters.Content = String.Format("提示: {0}", "是否保存当前更改的数据？");
                    //parameters.Header = HeaderText;
                    //parameters.IconContent = null;
                    //parameters.OkButtonContent = "确定";
                    //parameters.CancelButtonContent = "取消";
                    //parameters.Closed = OnEditSSAdviceDetail;//***close处理***
                    //RadWindow.Confirm(parameters);
                    YidanPathWayMessageBox mess = new YidanPathWayMessageBox("是否保存当前更改的数据？", "提示", YiDanMessageBoxButtons.YesNo);
                    mess.ShowDialog();
                    mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent2);
                }
                else
                {
                    GridViewRow row = ((RadRoutedEventArgs)e).OriginalSource as GridViewRow;
                    var RadMenu = sender as RadContextMenu;
                    //SSState = string.Empty;
                    foreach (RadMenuItem item in RadMenu.Items)
                    {
                        if (item.Tag != null)
                        {
                            if ((TagName)item.Tag == TagName.Edit)
                            {
                                item.IsEnabled = !(this.GridViewYZXX.SelectedItems.Count > 1);
                            }
                        }
                    }
                    if (row != null)
                    {
                        row.IsSelected = row.IsCurrent = true;
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 手术右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnSSMenuItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RadMenuItem clickedItem = ((RadRoutedEventArgs)e).OriginalSource as RadMenuItem;
                if (clickedItem != null)
                {
                    TagName tagName = (TagName)clickedItem.Tag;
                    object selectedItem = GridViewYZXX.SelectedItem;
                    DialogParameters parameters = new DialogParameters();
                    switch (tagName)
                    {
                        case TagName.New:
                            NewSSAdviceDetail();
                            break;
                        case TagName.Edit:
                            CurrentState = PageState.Edit;
                            CP_AdviceGroupDetail = this.GridViewYZXX.SelectedItem as CP_AdviceGroupDetail;
                            cbxSSYZBZ.SelectedValue = (short)CP_AdviceGroupDetail.Yzbz;
                            //cbxSSMC.SelectedValue = CP_AdviceGroupDetail.Ssdm;
                            cbxSSMC.SelectedItem = ((ObservableCollection<CP_Operation>)cbxSSMC.ItemsSource).First(cp => cp.Ssdm.Equals(CP_AdviceGroupDetail.Ypdm));
                            cbxSSMZ.SelectedValue = CP_AdviceGroupDetail.Mzdm;
                            txtSSZTNR.Text = CP_AdviceGroupDetail.Ztnr;
                            break;
                        case TagName.Del:
                            List<CP_AdviceGroupDetail> listdel = new List<CP_AdviceGroupDetail>();
                            foreach (CP_AdviceGroupDetail cp in this.GridViewYZXX.SelectedItems)
                            {
                                listdel.Add(cp);
                            }
                            for (int i = 0; i < listdel.Count; i++)
                            {
                                if (listdel[i].Flag != "")
                                {
                                    PublicMethod.RadAlterBox("存在分组不能删除", HeaderText);
                                    return;
                                }
                            }
                            //parameters.Content = String.Format("提示: {0}", "确认删除吗？");
                            //parameters.Header = HeaderText;
                            //parameters.IconContent = null;
                            //parameters.OkButtonContent = "确定";
                            //parameters.CancelButtonContent = "取消";
                            //parameters.Closed = OnDelAdviceGroupDetail;//***close处理***
                            //RadWindow.Confirm(parameters);
                            YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                            mess.ShowDialog();
                            mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent3);
                            break;
                        case TagName.SelectMuti:
                            GridViewYZXX.UnselectAll();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void cbxSSMC_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            var o = cbxSSMC.SelectedItem;
        }
        #endregion
        #region 手术函数
        /// <summary>
        /// 绑定手术数据
        /// </summary>
        /// <param name="nodeid"></param>
        //public void BindSSGridData(string nodeid)
        //{
        //    YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
        //    ServiceClient.GetAdviceAnesthesiaDetailCompleted +=
        //        (obj, e) =>
        //        {
        //            if (e.Error == null)
        //            {
        //                GridViewYZXX.ItemsSource = e.Result;
        //                CP_AdviceGroupDetailCollection = e.Result;
        //                if (GridViewYZXX.GroupDescriptors.Count == 0)
        //                {
        //                    Telerik.Windows.Data.GroupDescriptor gd = new Telerik.Windows.Data.GroupDescriptor();
        //                    gd.Member = "YzbzName";
        //                    GridViewYZXX.GroupDescriptors.Add(gd);
        //                }
        //            }
        //            else
        //            {
        //                PublicMethod.RadWaringBox(e.Error);
        //            }
        //        };
        //    ServiceClient.GetAdviceAnesthesiaDetailAsync(nodeid);
        //    ServiceClient.CloseAsync();
        //}







        void NewSSAdviceDetail()
        {
            cbxSSMC.SelectedItem = null;
            cbxSSMZ.SelectedValue = null;
            txtSSZTNR.Text = "";
            CurrentState = PageState.New;
        }
        //void OnDelSSAdviceDetail(object sender, WindowClosedEventArgs e)
        //{
        //    if (e.DialogResult == true)
        //    {
        //        List<decimal> listid = new List<decimal>();
        //        for (int i = 0; i < this.GridViewYZXX.SelectedItems.Count; i++)
        //        {
        //            listid.Add(((CP_AdviceGroupDetail)this.GridViewYZXX.SelectedItems[i]).Ctmxxh);
        //        }
        //        YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
        //        ServiceClient.DelSSAdviceDetailCompleted +=
        //        (obj, ea) =>
        //        {
        //            if (ea.Error == null)
        //            {
        //                PublicMethod.RadAlterBox(ea.Result, HeaderText);
        //                //BindSSGridData(nodestr.ToString().Split('&')[0].ToString());
        //                BindGridData(((RadPanelBarItem)this.OrderPanelBar.SelectedItem).Tag.ToString(), nodestr.ToString().Split('&')[0].ToString());
        //            }
        //            else
        //            {
        //                PublicMethod.RadWaringBox(ea.Error);
        //            }
        //        };
        //        ServiceClient.DelSSAdviceDetailAsync(ToObservableCollection(listid));
        //    }
        //    else
        //    {
        //        NewSSAdviceDetail();
        //    }
        //}


        void mess_PageClosedEvent2(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                    ServiceClient.UpdateAdviceGroupDetailCompleted +=
                           (obj, ea) =>
                           {
                               if (ea.Error == null)
                               {

                                   //BindSSGridData(nodestr.ToString().Split('&')[0].ToString());
                                   BindGridData(((RadPanelBarItem)this.OrderPanelBar.SelectedItem).Tag.ToString(), nodestr.ToString().Split('&')[0].ToString());
                                   NewSSAdviceDetail();
                                   PublicMethod.RadAlterBox(ea.Result, HeaderText);
                               }
                               else
                               {
                                   PublicMethod.RadWaringBox(ea.Error);
                               }
                           };
                    CP_AdviceGroupDetail = this.GridViewYZXX.SelectedItem as CP_AdviceGroupDetail;
                    CP_AdviceGroupDetail.Ypdm = ((CP_Operation)cbxSSMC.SelectedItem).Ssdm;
                    CP_AdviceGroupDetail.Ypmc = ((CP_Operation)cbxSSMC.SelectedItem).Name;
                    CP_AdviceGroupDetail.Mzdm = ((CP_Anesthesia)cbxSSMZ.SelectedItem).Mzdm;
                    CP_AdviceGroupDetail.Xmlb = (short)((CP_Operation)cbxSSMC.SelectedItem).Sslb;
                    CP_AdviceGroupDetail.Ztnr = txtSSZTNR.Text;
                    ServiceClient.UpdateAdviceGroupDetailAsync(CP_AdviceGroupDetail.Fzxh, CP_AdviceGroupDetail.Ctmxxh, CP_AdviceGroupDetail, false);

                }
                else
                {
                    NewSSAdviceDetail();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        void OnEditSSAdviceDetail(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                ServiceClient.UpdateAdviceGroupDetailCompleted +=
                       (obj, ea) =>
                       {
                           if (ea.Error == null)
                           {
                               
                               //BindSSGridData(nodestr.ToString().Split('&')[0].ToString());
                               BindGridData(((RadPanelBarItem)this.OrderPanelBar.SelectedItem).Tag.ToString(), nodestr.ToString().Split('&')[0].ToString());
                               NewSSAdviceDetail();
                               PublicMethod.RadAlterBox(ea.Result, HeaderText);
                           }
                           else
                           {
                               PublicMethod.RadWaringBox(ea.Error);
                           }
                       };
                CP_AdviceGroupDetail = this.GridViewYZXX.SelectedItem as CP_AdviceGroupDetail;
                CP_AdviceGroupDetail.Ypdm = ((CP_Operation)cbxSSMC.SelectedItem).Ssdm;
                CP_AdviceGroupDetail.Ypmc = ((CP_Operation)cbxSSMC.SelectedItem).Name;
                CP_AdviceGroupDetail.Mzdm = ((CP_Anesthesia)cbxSSMZ.SelectedItem).Mzdm;
                CP_AdviceGroupDetail.Xmlb = (short)((CP_Operation)cbxSSMC.SelectedItem).Sslb;
                CP_AdviceGroupDetail.Ztnr = txtSSZTNR.Text;
                ServiceClient.UpdateAdviceGroupDetailAsync(CP_AdviceGroupDetail.Fzxh, CP_AdviceGroupDetail.Ctmxxh, CP_AdviceGroupDetail, false);
            }
            else
            {
                NewSSAdviceDetail();
            }
        }
        //CP_AdviceGroupDetail ReturnCP_AdviceGroupDetail()
        //{
        //    CP_AdviceGroupDetail = this.GridViewYZXX.SelectedItem as CP_AdviceGroupDetail;
        //    CP_AdviceGroupDetail.Ssdm = ((CP_Operation)cbxSSMC.SelectedItem).Ssdm;
        //    CP_AdviceGroupDetail.Ypmc = ((CP_Operation)cbxSSMC.SelectedItem).Name;
        //    CP_AdviceGroupDetail.Mzdm = ((CP_Anesthesia)cbxSSMZ.SelectedItem).Mzdm;
        //    CP_AdviceGroupDetail.Xmlb = (short)((CP_Operation)cbxSSMC.SelectedItem).Sslb;
        //    CP_AdviceGroupDetail.Ztnr = txtSSZTNR.Text;
        //    return CP_AdviceGroupDetail;
        //}
        private void InitOperationInfo()
        {
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            ServiceClient.GetOperationInfoCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        cbxSSMC.ItemsSource = e.Result;
                        cbxSSMC.ItemFilter = SSFilter;
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                        return;
                    }
                };
            ServiceClient.GetOperationInfoAsync(null);
        }



        public bool SSFilter(string strFilter, object item)
        {
            //CP_PlaceOfDrug deptList = (CP_PlaceOfDrug)item;
            CP_Operation operList = (CP_Operation)item;
            return ((operList.Py.StartsWith(strFilter)) || (operList.Py.Contains(strFilter)));
        }
        /// <summary>
        /// 麻醉数据
        /// </summary>
        private void InitAnesthesiaInfo()
        {
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            ServiceClient.GetAnesthesiaInfoCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        cbxSSMZ.ItemsSource = e.Result;
                        cbxSSMZ.EmptyText = "请选择麻醉代码";
                        if (e.Result.Count > 0)
                        {
                            cbxSSMZ.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            ServiceClient.GetAnesthesiaInfoAsync(null);
        }



        #endregion
        #endregion
        #region 检验检查
        /// <summary>
        /// 检验检查医嘱输入初始化
        /// </summary>
        private void InitRisLisControl()
        {
            risLisOrderControl.AfterDrugLoadedEvent += new UserControls.UCRISLISOrder.DrugLoaded(risLisOrderControl_AfterDrugLoadedEvent);
            risLisOrderControl.AfterDrugCinfirmeddEvent += new UCRISLISOrder.DrugConfirmed(risLisOrderControl_AfterDrugCinfirmeddEvent);
        }
        /// <summary>
        /// 点击确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void risLisOrderControl_AfterDrugCinfirmeddEvent(object sender, CP_DoctorOrder e)
        {
            try
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                if (CurrentState == PageState.Edit)
                {
                    #region 实例 化新的实体
                    _cp_AdviceGroupDetail.Ypdm = e.Ypdm;
                    _cp_AdviceGroupDetail.Cdxh = e.Cdxh;
                    _cp_AdviceGroupDetail.Ggxh = e.Ggxh;
                    _cp_AdviceGroupDetail.Lcxh = e.Lcxh;
                    _cp_AdviceGroupDetail.Ypmc = e.Ypmc;
                    _cp_AdviceGroupDetail.Xmlb = Int16.Parse(e.Xmlb.ToString());
                    _cp_AdviceGroupDetail.Zxdw = e.Zxdw;
                    _cp_AdviceGroupDetail.Ypjl = e.Ypjl;
                    _cp_AdviceGroupDetail.Jldw = cbxMDDW.Text;
                    _cp_AdviceGroupDetail.Dwxs = e.Dwxs;
                    _cp_AdviceGroupDetail.Dwlb = Int32.Parse(e.Dwlb.ToString());
                    //_cp_AdviceGroupDetail.Yfdm = e.Yfdm;
                    _cp_AdviceGroupDetail.Pcdm = e.Pcdm;
                    _cp_AdviceGroupDetail.Zxcs = Int32.Parse(e.Zxcs.ToString());
                    _cp_AdviceGroupDetail.Zxzq = Int32.Parse(e.Zxzq.ToString());
                    _cp_AdviceGroupDetail.Zxzqdw = Int16.Parse(e.Zxzqdw.ToString());
                    _cp_AdviceGroupDetail.Zdm = e.Zdm;
                    _cp_AdviceGroupDetail.Zxsj = e.Zxsj;
                    _cp_AdviceGroupDetail.Zxts = int.Parse(e.Zxts.ToString());
                    _cp_AdviceGroupDetail.Ypzsl = e.Ypzsl;
                    _cp_AdviceGroupDetail.Ztnr = e.Ztnr;
                    _cp_AdviceGroupDetail.Yzlb = Int16.Parse(e.Yzlb.ToString());

                    _cp_AdviceGroupDetail.Jjlx = e.Jjlx;
                    _cp_AdviceGroupDetail.Zxksdm = e.Zxksdm;

                    _cp_AdviceGroupDetail.Yzkx = e.Yzkx;

                    #endregion

                    client.UpdateAdviceGroupDetailCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                
                                BindGridData(Convert.ToString((int)OrderPanelBarCategory.RisLis), nodestr.ToString().Split('&')[0].ToString());
                                NewAdviceGroupDetail();
                                PublicMethod.RadAlterBox(ea.Result, HeaderText);
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    client.UpdateAdviceGroupDetailAsync(_cp_AdviceGroupDetail.Fzxh, _cp_AdviceGroupDetail.Ctmxxh, _cp_AdviceGroupDetail, false);
                }
                else if (CurrentState == PageState.New)
                {
                    #region 实例 化新的实体
                    CP_AdviceGroupDetail order = new CP_AdviceGroupDetail();
                    order.Fzbz = 1;
                    order.Yzbz = Int16.Parse(e.Yzbz.ToString());
                    order.Ypdm = e.Ypdm;
                    order.Cdxh = e.Cdxh;
                    order.Ggxh = e.Ggxh;
                    order.Lcxh = e.Lcxh;
                    order.Ypmc = e.Ypmc;
                    order.Xmlb = Int16.Parse(e.Xmlb.ToString());
                    order.Zxdw = e.Zxdw;
                    order.Ypjl = e.Ypjl;
                    order.Jldw = cbxMDDW.Text;
                    order.Dwxs = e.Dwxs;
                    order.Dwlb = Int32.Parse(e.Dwlb.ToString());
                    //order.Yfdm = e.Yfdm;
                    order.Pcdm = e.Pcdm;
                    order.Zxcs = Int32.Parse(e.Zxcs.ToString());
                    order.Zxzq = Int32.Parse(e.Zxzq.ToString());
                    order.Zxzqdw = Int16.Parse(e.Zxzqdw.ToString());
                    order.Zdm = e.Zdm;
                    order.Zxsj = e.Zxsj;
                    order.Zxts = int.Parse(e.Zxts.ToString());
                    order.Ypzsl = e.Ypzsl;
                    order.Ztnr = e.Ztnr;
                    order.Yzlb = Int16.Parse(e.Yzlb.ToString());

                    order.Jjlx = e.Jjlx;
                    order.Zxksdm = e.Zxksdm;

                    order.Yzkx = e.Yzkx;
                    #endregion
                    client.InsertIntoAdviceGroupCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                
                                BindGridData(Convert.ToString((int)OrderPanelBarCategory.RisLis), nodestr.ToString().Split('&')[0].ToString());
                                NewAdviceGroupDetail();
                                PublicMethod.RadAlterBox(ea.Result, HeaderText);
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    client.InsertIntoAdviceGroupAsync(order, nodestr.ToString().Split('&')[0].ToString(), 3500, nodestr.ToString().Split('&')[1].ToString(), m_clinicalPathInfo.Ljdm); // 3500分组标志
                }
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }




        private void risLisOrderControl_AfterDrugLoadedEvent(object sender, RoutedEventArgs e)
        {
            //to do
        }
        /// <summary>
        /// RIS LIS右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRISLISMenuItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RadMenuItem clickedItem = ((RadRoutedEventArgs)e).OriginalSource as RadMenuItem;
                if (clickedItem != null)
                {
                    TagName tagName = (TagName)clickedItem.Tag;
                    object selectedItem = GridViewYZXX.SelectedItem;
                    DialogParameters parameters = new DialogParameters();
                    switch (tagName)
                    {
                        case TagName.Edit:
                            CurrentState = PageState.Edit;
                            zdm.Clear();
                            zxsj.Clear();
                            #region AdviceGroupDetail -> CP_DoctorOrder
                            _cp_AdviceGroupDetail = this.GridViewYZXX.SelectedItem as CP_AdviceGroupDetail;
                            CP_DoctorOrder order = new CP_DoctorOrder();
                            order.Ctmxxh = _cp_AdviceGroupDetail.Ctmxxh;
                            order.Cdxh = _cp_AdviceGroupDetail.Cdxh;
                            order.Ggxh = _cp_AdviceGroupDetail.Ggxh;
                            order.Lcxh = _cp_AdviceGroupDetail.Lcxh;
                            order.Ypdm = _cp_AdviceGroupDetail.Ypdm;
                            order.Xmlb = _cp_AdviceGroupDetail.Xmlb;
                            order.Yzlb = _cp_AdviceGroupDetail.Yzlb;
                            order.Yzbz = _cp_AdviceGroupDetail.Yzbz;
                            order.YzbzName = _cp_AdviceGroupDetail.YzbzName;
                            order.Ypjl = _cp_AdviceGroupDetail.Ypjl;
                            order.Jldw = _cp_AdviceGroupDetail.Jldw;
                            order.Yfdm = _cp_AdviceGroupDetail.Yfdm;
                            //order.YfdmName = _cp_AdviceGroupDetail.yfdm
                            order.Pcdm = _cp_AdviceGroupDetail.Pcdm;      //
                            //order.PcdmName = _cp_AdviceGroupDetail.pc
                            //order.Ksrq = \_cp_AdviceGroupDetail.ks
                            order.Ypmc = _cp_AdviceGroupDetail.Ypmc;
                            //order.FromTable = row["FromTable"].ToString();//
                            order.Flag = _cp_AdviceGroupDetail.Flag;
                            order.OrderGuid = Guid.NewGuid().ToString();//注意此处
                            order.Fzbz = _cp_AdviceGroupDetail.Fzbz;
                            order.Fzxh = _cp_AdviceGroupDetail.Fzxh;
                            order.Zxdw = _cp_AdviceGroupDetail.Zxdw;
                            //order.Ypgg = _cp_AdviceGroupDetail.ypg
                            order.Dwxs = _cp_AdviceGroupDetail.Dwxs;
                            order.Dwlb = _cp_AdviceGroupDetail.Dwlb;
                            order.Zxcs = _cp_AdviceGroupDetail.Zxcs;
                            order.Zxzq = _cp_AdviceGroupDetail.Zxzq;
                            order.Zxzqdw = _cp_AdviceGroupDetail.Zxzqdw;
                            order.Zdm = _cp_AdviceGroupDetail.Zdm;
                            order.Zxsj = _cp_AdviceGroupDetail.Zxsj;
                            order.Yznr = _cp_AdviceGroupDetail.Yznr;
                            order.Ztnr = _cp_AdviceGroupDetail.Ztnr;
                            //order.Yzzt = _cp_AdviceGroupDetail.y
                            #endregion
                            risLisOrderControl.ManualType = ManualType.Edit;
                            risLisOrderControl.CP_AdviceGroupDetailProptery = order;
                            risLisOrderControl.InitModifyOrder();
                            break;
                        case TagName.Del:
                            foreach (CP_AdviceGroupDetail cp in this.GridViewYZXX.SelectedItems)
                            {
                                if (cp.Flag != "")
                                {
                                    PublicMethod.RadAlterBox("存在分组不能删除", HeaderText);
                                    return;
                                }
                            }
                            //parameters.Content = String.Format("提示: {0}", "确认删除吗？");
                            //parameters.Header = HeaderText;
                            //parameters.IconContent = null;
                            //parameters.OkButtonContent = "确定";
                            //parameters.CancelButtonContent = "取消";
                            //parameters.Closed = OnDelAdviceGroupDetail;//***close处理***
                            //RadWindow.Confirm(parameters);
                            YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                            mess.ShowDialog();
                            mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent3);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 右键打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnRISLISMenuOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogParameters parameters = new DialogParameters();
                if (false)
                {
                    RadContextMenu menu = sender as RadContextMenu;
                    menu.IsOpen = false;
                    //parameters.Content = String.Format("提示: {0}", "是否保存当前更改的数据？");
                    //parameters.Header = HeaderText;
                    //parameters.IconContent = null;
                    //parameters.OkButtonContent = "确定";
                    //parameters.CancelButtonContent = "取消";
                    //parameters.Closed = OnEditAdviceGroupDetail;//***close处理***
                    //RadWindow.Confirm(parameters);

                    YidanPathWayMessageBox mess = new YidanPathWayMessageBox("是否保存当前更改的数据？", "提示", YiDanMessageBoxButtons.YesNo);
                    mess.ShowDialog();
                    mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);

                }
                else
                {
                    GridViewRow row = ((RadRoutedEventArgs)e).OriginalSource as GridViewRow;
                    List<CP_AdviceGroupDetail> listsOrder = new List<CP_AdviceGroupDetail>();
                    var RadMenu = sender as RadContextMenu;
                    Boolean IsSelectItemsHaveDefferentGroup = false;
                    Boolean IsSelectItemsHaveGroupItem = false;
                    foreach (RadMenuItem item in RadMenu.Items)
                    {
                        if (row != null && !row.IsSelected)
                        {
                            item.IsEnabled = false;
                        }
                        else
                            if (item.Tag != null)
                            {
                                if ((TagName)item.Tag == TagName.Edit)
                                {
                                    item.IsEnabled = !(this.GridViewYZXX.SelectedItems.Count > 1);
                                }
                                if ((TagName)item.Tag == TagName.Group)
                                {
                                    #region 判断选中的数据中是否同时包含长期医嘱和临时医嘱，是否包含已成组项目
                                    for (int i = 0; i < GridViewYZXX.SelectedItems.Count; i++)
                                    {
                                        if (i > 0 && !IsSelectItemsHaveDefferentGroup)
                                            IsSelectItemsHaveDefferentGroup =
                                                ((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i - 1]).Yzbz != ((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i]).Yzbz;
                                        if (((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i]).Fzbz != 3500) IsSelectItemsHaveGroupItem = true;
                                    }
                                    #endregion
                                    item.IsEnabled = (this.GridViewYZXX.SelectedItems.Count > 1 && !IsSelectItemsHaveDefferentGroup && !IsSelectItemsHaveGroupItem);
                                }
                            }
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region 营养膳食
        /// <summary>
        ///   膳食医嘱输入初始化
        /// </summary>
        private void InitMealControl()
        {
            foodOrderControl.PanelCategory = OrderPanelBarCategory.Meal;
            foodOrderControl.OrderCategory = OrderItemCategory.Meal;
            foodOrderControl.AfterDrugLoadedEvent += new UCOtherOrder.DrugLoaded(foodOrderControl_AfterDrugLoadedEvent);
            foodOrderControl.AfterDrugCinfirmeddEvent += new UCOtherOrder.DrugConfirmed(foodOrderControl_AfterDrugCinfirmeddEvent);
        }
        private void foodOrderControl_AfterDrugCinfirmeddEvent(object sender, CP_DoctorOrder e)
        {
            try
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                if (CurrentState == PageState.Edit)
                {
                    #region 实例 化新的实体
                    _cp_AdviceGroupDetail.Yzbz = Int16.Parse(e.Yzbz.ToString());
                    _cp_AdviceGroupDetail.Ypdm = e.Ypdm;
                    _cp_AdviceGroupDetail.Cdxh = e.Cdxh;
                    _cp_AdviceGroupDetail.Ggxh = e.Ggxh;
                    _cp_AdviceGroupDetail.Lcxh = e.Lcxh;
                    _cp_AdviceGroupDetail.Ypmc = e.Ypmc;
                    _cp_AdviceGroupDetail.Xmlb = Int16.Parse(e.Xmlb.ToString());
                    _cp_AdviceGroupDetail.Zxdw = e.Zxdw;
                    _cp_AdviceGroupDetail.Ypjl = e.Ypjl;
                    _cp_AdviceGroupDetail.Jldw = cbxMDDW.Text;
                    _cp_AdviceGroupDetail.Dwxs = e.Dwxs;
                    _cp_AdviceGroupDetail.Dwlb = Int32.Parse(e.Dwlb.ToString());
                    //_cp_AdviceGroupDetail.Yfdm = e.Yfdm;
                    _cp_AdviceGroupDetail.Pcdm = e.Pcdm;
                    _cp_AdviceGroupDetail.Zxcs = Int32.Parse(e.Zxcs.ToString());
                    _cp_AdviceGroupDetail.Zxzq = Int32.Parse(e.Zxzq.ToString());
                    _cp_AdviceGroupDetail.Zxzqdw = Int16.Parse(e.Zxzqdw.ToString());
                    _cp_AdviceGroupDetail.Zdm = e.Zdm;
                    _cp_AdviceGroupDetail.Zxsj = e.Zxsj;
                    _cp_AdviceGroupDetail.Zxts = int.Parse(e.Zxts.ToString());
                    _cp_AdviceGroupDetail.Ypzsl = e.Ypzsl;
                    _cp_AdviceGroupDetail.Ztnr = e.Ztnr;
                    _cp_AdviceGroupDetail.Yzlb = Int16.Parse(e.Yzlb.ToString());
                    #endregion
                    client.UpdateAdviceGroupDetailCompleted +=

                    (obj, ea) =>
                    {
                        if (ea.Error == null)
                        {
                            
                            BindGridData(Convert.ToString((int)OrderPanelBarCategory.Meal), nodestr.ToString().Split('&')[0].ToString());
                            NewAdviceGroupDetail();
                            PublicMethod.RadAlterBox(ea.Result, HeaderText);
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(ea.Error);
                        }
                    };
                    client.UpdateAdviceGroupDetailAsync(_cp_AdviceGroupDetail.Fzxh, _cp_AdviceGroupDetail.Ctmxxh, _cp_AdviceGroupDetail, false);
                }
                else if (CurrentState == PageState.New)
                {
                    #region 实例 化新的实体
                    CP_AdviceGroupDetail order = new CP_AdviceGroupDetail();
                    order.Fzbz = 1;
                    order.Yzbz = Int16.Parse(e.Yzbz.ToString());
                    order.Ypdm = e.Ypdm;
                    order.Cdxh = e.Cdxh;
                    order.Ggxh = e.Ggxh;
                    order.Lcxh = e.Lcxh;
                    order.Ypmc = e.Ypmc;
                    order.Xmlb = Int16.Parse(e.Xmlb.ToString());
                    order.Zxdw = e.Zxdw;
                    order.Ypjl = e.Ypjl;
                    order.Jldw = cbxMDDW.Text;
                    order.Dwxs = e.Dwxs;
                    order.Dwlb = Int32.Parse(e.Dwlb.ToString());
                    //order.Yfdm = e.Yfdm;
                    order.Pcdm = e.Pcdm;
                    order.Zxcs = Int32.Parse(e.Zxcs.ToString());
                    order.Zxzq = Int32.Parse(e.Zxzq.ToString());
                    order.Zxzqdw = Int16.Parse(e.Zxzqdw.ToString());
                    order.Zdm = e.Zdm;
                    order.Zxsj = e.Zxsj;
                    order.Zxts = int.Parse(e.Zxts.ToString());
                    order.Ypzsl = e.Ypzsl;
                    order.Ztnr = e.Ztnr;
                    order.Yzlb = Int16.Parse(e.Yzlb.ToString());
                    #endregion
                    client.InsertIntoAdviceGroupCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    
                                    BindGridData(Convert.ToString((int)OrderPanelBarCategory.Meal), nodestr.ToString().Split('&')[0].ToString());
                                    NewAdviceGroupDetail();
                                    PublicMethod.RadAlterBox(ea.Result, HeaderText);
                                }
                                else
                                {
                                    PublicMethod.RadWaringBox(ea.Error);
                                }
                            };
                    client.InsertIntoAdviceGroupAsync(order, nodestr.ToString().Split('&')[0].ToString(), 3500, nodestr.ToString().Split('&')[1].ToString(), m_clinicalPathInfo.Ljdm); // 3500分组标志
                }
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #region  确定后




        #endregion
        private void foodOrderControl_AfterDrugLoadedEvent(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #region  GRID 鼠标
        /// <summary>
        /// Meal 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMealMenuItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RadMenuItem clickedItem = ((RadRoutedEventArgs)e).OriginalSource as RadMenuItem;
                if (clickedItem != null)
                {
                    TagName tagName = (TagName)clickedItem.Tag;
                    object selectedItem = GridViewYZXX.SelectedItem;
                    DialogParameters parameters = new DialogParameters();
                    switch (tagName)
                    {
                        case TagName.Edit:
                            CurrentState = PageState.Edit;
                            zdm.Clear();
                            zxsj.Clear();
                            #region AdviceGroupDetail -> CP_DoctorOrder
                            _cp_AdviceGroupDetail = this.GridViewYZXX.SelectedItem as CP_AdviceGroupDetail;
                            CP_DoctorOrder order = new CP_DoctorOrder();
                            order.Ctmxxh = _cp_AdviceGroupDetail.Ctmxxh;
                            order.Cdxh = _cp_AdviceGroupDetail.Cdxh;
                            order.Ggxh = _cp_AdviceGroupDetail.Ggxh;
                            order.Lcxh = _cp_AdviceGroupDetail.Lcxh;
                            order.Ypdm = _cp_AdviceGroupDetail.Ypdm;
                            order.Xmlb = _cp_AdviceGroupDetail.Xmlb;
                            order.Yzlb = _cp_AdviceGroupDetail.Yzlb;
                            order.Yzbz = _cp_AdviceGroupDetail.Yzbz;
                            order.YzbzName = _cp_AdviceGroupDetail.YzbzName;
                            order.Ypjl = _cp_AdviceGroupDetail.Ypjl;
                            order.Jldw = _cp_AdviceGroupDetail.Jldw;
                            order.Yfdm = _cp_AdviceGroupDetail.Yfdm;
                            //order.YfdmName = _cp_AdviceGroupDetail.yfdm
                            order.Pcdm = _cp_AdviceGroupDetail.Pcdm;      //
                            //order.PcdmName = _cp_AdviceGroupDetail.pc
                            //order.Ksrq = \_cp_AdviceGroupDetail.ks
                            order.Ypmc = _cp_AdviceGroupDetail.Ypmc;
                            //order.FromTable = row["FromTable"].ToString();//
                            order.Flag = _cp_AdviceGroupDetail.Flag;
                            order.OrderGuid = Guid.NewGuid().ToString();//注意此处
                            order.Fzbz = _cp_AdviceGroupDetail.Fzbz;
                            order.Fzxh = _cp_AdviceGroupDetail.Fzxh;
                            order.Zxdw = _cp_AdviceGroupDetail.Zxdw;
                            //order.Ypgg = _cp_AdviceGroupDetail.ypg
                            order.Dwxs = _cp_AdviceGroupDetail.Dwxs;
                            order.Dwlb = _cp_AdviceGroupDetail.Dwlb;
                            order.Zxcs = _cp_AdviceGroupDetail.Zxcs;
                            order.Zxzq = _cp_AdviceGroupDetail.Zxzq;
                            order.Zxzqdw = _cp_AdviceGroupDetail.Zxzqdw;
                            order.Zdm = _cp_AdviceGroupDetail.Zdm;
                            order.Zxsj = _cp_AdviceGroupDetail.Zxsj;
                            order.Yznr = _cp_AdviceGroupDetail.Yznr;
                            order.Ztnr = _cp_AdviceGroupDetail.Ztnr;
                            //order.Yzzt = _cp_AdviceGroupDetail.y
                            #endregion
                            foodOrderControl.ManualType = ManualType.Edit;
                            foodOrderControl.CP_AdviceGroupDetailProptery = order;
                            foodOrderControl.InitModifyOrder();
                            break;
                        case TagName.Del:
                            foreach (CP_AdviceGroupDetail cp in this.GridViewYZXX.SelectedItems)
                            {
                                if (cp.Flag != "")
                                {
                                    PublicMethod.RadAlterBox("存在分组不能删除", HeaderText);
                                    return;
                                }
                            }
                            //parameters.Content = String.Format("提示: {0}", "确认删除吗？");
                            //parameters.Header = HeaderText;
                            //parameters.IconContent = null;
                            //parameters.OkButtonContent = "确定";
                            //parameters.CancelButtonContent = "取消";
                            //parameters.Closed = OnDelAdviceGroupDetail;//***close处理***
                            //RadWindow.Confirm(parameters);
                            YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                            mess.ShowDialog();
                            mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent3);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 右键打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnMealMenuOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                GridViewRow row = ((RadRoutedEventArgs)e).OriginalSource as GridViewRow;
                List<CP_AdviceGroupDetail> listsOrder = new List<CP_AdviceGroupDetail>();
                var RadMenu = sender as RadContextMenu;
                Boolean IsSelectItemsHaveDefferentGroup = false;
                Boolean IsSelectItemsHaveGroupItem = false;
                foreach (RadMenuItem item in RadMenu.Items)
                {
                    if (row != null && !row.IsSelected)
                    {
                        item.IsEnabled = false;
                    }
                    else
                        if (item.Tag != null)
                        {
                            if ((TagName)item.Tag == TagName.Edit)
                            {
                                item.IsEnabled = !(this.GridViewYZXX.SelectedItems.Count > 1);
                            }
                            if ((TagName)item.Tag == TagName.Group)
                            {
                                #region 判断选中的数据中是否同时包含长期医嘱和临时医嘱，是否包含已成组项目
                                for (int i = 0; i < GridViewYZXX.SelectedItems.Count; i++)
                                {
                                    if (i > 0 && !IsSelectItemsHaveDefferentGroup)
                                        IsSelectItemsHaveDefferentGroup =
                                            ((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i - 1]).Yzbz != ((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i]).Yzbz;
                                    if (((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i]).Fzbz != 3500) IsSelectItemsHaveGroupItem = true;
                                }
                                #endregion
                                item.IsEnabled = (this.GridViewYZXX.SelectedItems.Count > 1 && !IsSelectItemsHaveDefferentGroup && !IsSelectItemsHaveGroupItem);
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #endregion
        #region  观察
        /// <summary>
        ///   观察医嘱输入初始化
        /// </summary>
        private void InitObservationControl()
        {
            observationOrderControl.PanelCategory = OrderPanelBarCategory.Observation;
            observationOrderControl.OrderCategory = OrderItemCategory.Observation;
            observationOrderControl.AfterDrugLoadedEvent += new UCOtherOrder.DrugLoaded(observationOrderControl_AfterDrugLoadedEvent);
            observationOrderControl.AfterDrugCinfirmeddEvent += new UCOtherOrder.DrugConfirmed(observationOrderControl_AfterDrugCinfirmeddEvent);
        }
        private void observationOrderControl_AfterDrugCinfirmeddEvent(object sender, CP_DoctorOrder e)
        {
            try
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                if (CurrentState == PageState.Edit)
                {
                    #region 实例 化新的实体
                    _cp_AdviceGroupDetail.Yzbz = Int16.Parse(e.Yzbz.ToString());
                    _cp_AdviceGroupDetail.Ypdm = e.Ypdm;
                    _cp_AdviceGroupDetail.Cdxh = e.Cdxh;
                    _cp_AdviceGroupDetail.Ggxh = e.Ggxh;
                    _cp_AdviceGroupDetail.Lcxh = e.Lcxh;
                    _cp_AdviceGroupDetail.Ypmc = e.Ypmc;
                    _cp_AdviceGroupDetail.Xmlb = Int16.Parse(e.Xmlb.ToString());
                    _cp_AdviceGroupDetail.Zxdw = e.Zxdw;
                    _cp_AdviceGroupDetail.Ypjl = e.Ypjl;
                    _cp_AdviceGroupDetail.Jldw = cbxMDDW.Text;
                    _cp_AdviceGroupDetail.Dwxs = e.Dwxs;
                    _cp_AdviceGroupDetail.Dwlb = Int32.Parse(e.Dwlb.ToString());
                    //_cp_AdviceGroupDetail.Yfdm = e.Yfdm;
                    _cp_AdviceGroupDetail.Pcdm = e.Pcdm;
                    _cp_AdviceGroupDetail.Zxcs = Int32.Parse(e.Zxcs.ToString());
                    _cp_AdviceGroupDetail.Zxzq = Int32.Parse(e.Zxzq.ToString());
                    _cp_AdviceGroupDetail.Zxzqdw = Int16.Parse(e.Zxzqdw.ToString());
                    _cp_AdviceGroupDetail.Zdm = e.Zdm;
                    _cp_AdviceGroupDetail.Zxsj = e.Zxsj;
                    _cp_AdviceGroupDetail.Zxts = int.Parse(e.Zxts.ToString());
                    _cp_AdviceGroupDetail.Ypzsl = e.Ypzsl;
                    _cp_AdviceGroupDetail.Ztnr = e.Ztnr;
                    _cp_AdviceGroupDetail.Yzlb = Int16.Parse(e.Yzlb.ToString());
                    #endregion
                    client.UpdateAdviceGroupDetailCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                
                                BindGridData(Convert.ToString((int)OrderPanelBarCategory.Observation), nodestr.ToString().Split('&')[0].ToString());
                                NewAdviceGroupDetail();
                                PublicMethod.RadAlterBox(ea.Result, HeaderText);
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    client.UpdateAdviceGroupDetailAsync(_cp_AdviceGroupDetail.Fzxh, _cp_AdviceGroupDetail.Ctmxxh, _cp_AdviceGroupDetail, false);
                }
                else if (CurrentState == PageState.New)
                {
                    #region 实例 化新的实体
                    CP_AdviceGroupDetail order = new CP_AdviceGroupDetail();
                    order.Fzbz = 1;
                    order.Yzbz = Int16.Parse(e.Yzbz.ToString());
                    order.Ypdm = e.Ypdm;
                    order.Cdxh = e.Cdxh;
                    order.Ggxh = e.Ggxh;
                    order.Lcxh = e.Lcxh;
                    order.Ypmc = e.Ypmc;
                    order.Xmlb = Int16.Parse(e.Xmlb.ToString());
                    order.Zxdw = e.Zxdw;
                    order.Ypjl = e.Ypjl;
                    order.Jldw = cbxMDDW.Text;
                    order.Dwxs = e.Dwxs;
                    order.Dwlb = Int32.Parse(e.Dwlb.ToString());
                    //order.Yfdm = e.Yfdm;
                    order.Pcdm = e.Pcdm;
                    order.Zxcs = Int32.Parse(e.Zxcs.ToString());
                    order.Zxzq = Int32.Parse(e.Zxzq.ToString());
                    order.Zxzqdw = Int16.Parse(e.Zxzqdw.ToString());
                    order.Zdm = e.Zdm;
                    order.Zxsj = e.Zxsj;
                    order.Zxts = int.Parse(e.Zxts.ToString());
                    order.Ypzsl = e.Ypzsl;
                    order.Ztnr = e.Ztnr;
                    order.Yzlb = Int16.Parse(e.Yzlb.ToString());
                    #endregion
                    client.InsertIntoAdviceGroupCompleted +=

                    (obj, ea) =>
                    {
                        if (ea.Error == null)
                        {
                            
                            BindGridData(Convert.ToString((int)OrderPanelBarCategory.Observation), nodestr.ToString().Split('&')[0].ToString());
                            NewAdviceGroupDetail();
                            PublicMethod.RadAlterBox(ea.Result, HeaderText);
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(ea.Error);
                        }
                    };
                    client.InsertIntoAdviceGroupAsync(order, nodestr.ToString().Split('&')[0].ToString(), 3500, nodestr.ToString().Split('&')[1].ToString(), m_clinicalPathInfo.Ljdm); // 3500分组标志
                }
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #region 确定后




        #endregion
        private void observationOrderControl_AfterDrugLoadedEvent(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #region GRID 鼠标
        /// <summary>
        /// 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnObservationMenuItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RadMenuItem clickedItem = ((RadRoutedEventArgs)e).OriginalSource as RadMenuItem;
                if (clickedItem != null)
                {
                    TagName tagName = (TagName)clickedItem.Tag;
                    object selectedItem = GridViewYZXX.SelectedItem;
                    DialogParameters parameters = new DialogParameters();
                    switch (tagName)
                    {
                        case TagName.Edit:
                            CurrentState = PageState.Edit;
                            zdm.Clear();
                            zxsj.Clear();
                            #region AdviceGroupDetail -> CP_DoctorOrder
                            _cp_AdviceGroupDetail = this.GridViewYZXX.SelectedItem as CP_AdviceGroupDetail;
                            CP_DoctorOrder order = new CP_DoctorOrder();
                            order.Ctmxxh = _cp_AdviceGroupDetail.Ctmxxh;
                            order.Cdxh = _cp_AdviceGroupDetail.Cdxh;
                            order.Ggxh = _cp_AdviceGroupDetail.Ggxh;
                            order.Lcxh = _cp_AdviceGroupDetail.Lcxh;
                            order.Ypdm = _cp_AdviceGroupDetail.Ypdm;
                            order.Xmlb = _cp_AdviceGroupDetail.Xmlb;
                            order.Yzlb = _cp_AdviceGroupDetail.Yzlb;
                            order.Yzbz = _cp_AdviceGroupDetail.Yzbz;
                            order.YzbzName = _cp_AdviceGroupDetail.YzbzName;
                            order.Ypjl = _cp_AdviceGroupDetail.Ypjl;
                            order.Jldw = _cp_AdviceGroupDetail.Jldw;
                            order.Yfdm = _cp_AdviceGroupDetail.Yfdm;
                            //order.YfdmName = _cp_AdviceGroupDetail.yfdm
                            order.Pcdm = _cp_AdviceGroupDetail.Pcdm;      //
                            //order.PcdmName = _cp_AdviceGroupDetail.pc
                            //order.Ksrq = \_cp_AdviceGroupDetail.ks
                            order.Ypmc = _cp_AdviceGroupDetail.Ypmc;
                            //order.FromTable = row["FromTable"].ToString();//
                            order.Flag = _cp_AdviceGroupDetail.Flag;
                            order.OrderGuid = Guid.NewGuid().ToString();//注意此处
                            order.Fzbz = _cp_AdviceGroupDetail.Fzbz;
                            order.Fzxh = _cp_AdviceGroupDetail.Fzxh;
                            order.Zxdw = _cp_AdviceGroupDetail.Zxdw;
                            //order.Ypgg = _cp_AdviceGroupDetail.ypg
                            order.Dwxs = _cp_AdviceGroupDetail.Dwxs;
                            order.Dwlb = _cp_AdviceGroupDetail.Dwlb;
                            order.Zxcs = _cp_AdviceGroupDetail.Zxcs;
                            order.Zxzq = _cp_AdviceGroupDetail.Zxzq;
                            order.Zxzqdw = _cp_AdviceGroupDetail.Zxzqdw;
                            order.Zdm = _cp_AdviceGroupDetail.Zdm;
                            order.Zxsj = _cp_AdviceGroupDetail.Zxsj;
                            order.Yznr = _cp_AdviceGroupDetail.Yznr;
                            order.Ztnr = _cp_AdviceGroupDetail.Ztnr;
                            //order.Yzzt = _cp_AdviceGroupDetail.y
                            #endregion
                            observationOrderControl.ManualType = ManualType.Edit;
                            observationOrderControl.CP_AdviceGroupDetailProptery = order;
                            observationOrderControl.InitModifyOrder();
                            break;
                        case TagName.Del:
                            foreach (CP_AdviceGroupDetail cp in this.GridViewYZXX.SelectedItems)
                            {
                                if (cp.Flag != "")
                                {
                                    PublicMethod.RadAlterBox("存在分组不能删除", HeaderText);
                                    return;
                                }
                            }
                            //parameters.Content = String.Format("提示: {0}", "确认删除吗？");
                            //parameters.Header = HeaderText;
                            //parameters.IconContent = null;
                            //parameters.OkButtonContent = "确定";
                            //parameters.CancelButtonContent = "取消";
                            //parameters.Closed = OnDelAdviceGroupDetail;//***close处理***
                            //RadWindow.Confirm(parameters);
                            YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                            mess.ShowDialog();
                            mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent3);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 右键打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnObservationMenuOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                GridViewRow row = ((RadRoutedEventArgs)e).OriginalSource as GridViewRow;
                List<CP_AdviceGroupDetail> listsOrder = new List<CP_AdviceGroupDetail>();
                var RadMenu = sender as RadContextMenu;
                Boolean IsSelectItemsHaveDefferentGroup = false;
                Boolean IsSelectItemsHaveGroupItem = false;
                foreach (RadMenuItem item in RadMenu.Items)
                {
                    if (row != null && !row.IsSelected)
                    {
                        item.IsEnabled = false;
                    }
                    else
                        if (item.Tag != null)
                        {
                            if ((TagName)item.Tag == TagName.Edit)
                            {
                                item.IsEnabled = !(this.GridViewYZXX.SelectedItems.Count > 1);
                            }
                            if ((TagName)item.Tag == TagName.Group)
                            {
                                #region 判断选中的数据中是否同时包含长期医嘱和临时医嘱，是否包含已成组项目
                                for (int i = 0; i < GridViewYZXX.SelectedItems.Count; i++)
                                {
                                    if (i > 0 && !IsSelectItemsHaveDefferentGroup)
                                        IsSelectItemsHaveDefferentGroup =
                                            ((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i - 1]).Yzbz != ((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i]).Yzbz;
                                    if (((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i]).Fzbz != 3500) IsSelectItemsHaveGroupItem = true;
                                }
                                #endregion
                                item.IsEnabled = (this.GridViewYZXX.SelectedItems.Count > 1 && !IsSelectItemsHaveDefferentGroup && !IsSelectItemsHaveGroupItem);
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #endregion
        #region  活动
        /// <summary>
        ///   活动医嘱输入初始化
        /// </summary>
        private void InitActivityControl()
        {
            activityOrderControl.PanelCategory = OrderPanelBarCategory.Activity;
            activityOrderControl.OrderCategory = OrderItemCategory.Activity;
            activityOrderControl.AfterDrugLoadedEvent += new UCOtherOrder.DrugLoaded(activityOrderControl_AfterDrugLoadedEvent);
            activityOrderControl.AfterDrugCinfirmeddEvent += new UCOtherOrder.DrugConfirmed(activityOrderControl_AfterDrugCinfirmeddEvent);
        }
        private void activityOrderControl_AfterDrugCinfirmeddEvent(object sender, CP_DoctorOrder e)
        {
            try
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                if (CurrentState == PageState.Edit)
                {
                    #region 实例 化新的实体
                    _cp_AdviceGroupDetail.Yzbz = Int16.Parse(e.Yzbz.ToString());
                    _cp_AdviceGroupDetail.Ypdm = e.Ypdm;
                    _cp_AdviceGroupDetail.Cdxh = e.Cdxh;
                    _cp_AdviceGroupDetail.Ggxh = e.Ggxh;
                    _cp_AdviceGroupDetail.Lcxh = e.Lcxh;
                    _cp_AdviceGroupDetail.Ypmc = e.Ypmc;
                    _cp_AdviceGroupDetail.Xmlb = Int16.Parse(e.Xmlb.ToString());
                    _cp_AdviceGroupDetail.Zxdw = e.Zxdw;
                    _cp_AdviceGroupDetail.Ypjl = e.Ypjl;
                    _cp_AdviceGroupDetail.Jldw = cbxMDDW.Text;
                    _cp_AdviceGroupDetail.Dwxs = e.Dwxs;
                    _cp_AdviceGroupDetail.Dwlb = Int32.Parse(e.Dwlb.ToString());
                    //_cp_AdviceGroupDetail.Yfdm = e.Yfdm;
                    _cp_AdviceGroupDetail.Pcdm = e.Pcdm;
                    _cp_AdviceGroupDetail.Zxcs = Int32.Parse(e.Zxcs.ToString());
                    _cp_AdviceGroupDetail.Zxzq = Int32.Parse(e.Zxzq.ToString());
                    _cp_AdviceGroupDetail.Zxzqdw = Int16.Parse(e.Zxzqdw.ToString());
                    _cp_AdviceGroupDetail.Zdm = e.Zdm;
                    _cp_AdviceGroupDetail.Zxsj = e.Zxsj;
                    _cp_AdviceGroupDetail.Zxts = int.Parse(e.Zxts.ToString());
                    _cp_AdviceGroupDetail.Ypzsl = e.Ypzsl;
                    _cp_AdviceGroupDetail.Ztnr = e.Ztnr;
                    _cp_AdviceGroupDetail.Yzlb = Int16.Parse(e.Yzlb.ToString());
                    #endregion
                    client.UpdateAdviceGroupDetailCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                
                                BindGridData(Convert.ToString((int)OrderPanelBarCategory.Activity), nodestr.ToString().Split('&')[0].ToString());
                                NewAdviceGroupDetail();
                                PublicMethod.RadAlterBox(ea.Result, HeaderText);
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    client.UpdateAdviceGroupDetailAsync(_cp_AdviceGroupDetail.Fzxh, _cp_AdviceGroupDetail.Ctmxxh, _cp_AdviceGroupDetail, false);
                }
                else if (CurrentState == PageState.New)
                {
                    #region 实例 化新的实体
                    CP_AdviceGroupDetail order = new CP_AdviceGroupDetail();
                    order.Fzbz = 1;
                    order.Yzbz = Int16.Parse(e.Yzbz.ToString());
                    order.Ypdm = e.Ypdm;
                    order.Cdxh = e.Cdxh;
                    order.Ggxh = e.Ggxh;
                    order.Lcxh = e.Lcxh;
                    order.Ypmc = e.Ypmc;
                    order.Xmlb = Int16.Parse(e.Xmlb.ToString());
                    order.Zxdw = e.Zxdw;
                    order.Ypjl = e.Ypjl;
                    order.Jldw = cbxMDDW.Text;
                    order.Dwxs = e.Dwxs;
                    order.Dwlb = Int32.Parse(e.Dwlb.ToString());
                    //order.Yfdm = e.Yfdm;
                    order.Pcdm = e.Pcdm;
                    order.Zxcs = Int32.Parse(e.Zxcs.ToString());
                    order.Zxzq = Int32.Parse(e.Zxzq.ToString());
                    order.Zxzqdw = Int16.Parse(e.Zxzqdw.ToString());
                    order.Zdm = e.Zdm;
                    order.Zxsj = e.Zxsj;
                    order.Zxts = int.Parse(e.Zxts.ToString());
                    order.Ypzsl = e.Ypzsl;
                    order.Ztnr = e.Ztnr;
                    order.Yzlb = Int16.Parse(e.Yzlb.ToString());
                    #endregion
                    client.InsertIntoAdviceGroupCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                
                                BindGridData(Convert.ToString((int)OrderPanelBarCategory.Activity), nodestr.ToString().Split('&')[0].ToString());
                                NewAdviceGroupDetail();
                                PublicMethod.RadAlterBox(ea.Result, HeaderText);
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    client.InsertIntoAdviceGroupAsync(order, nodestr.ToString().Split('&')[0].ToString(), 3500, nodestr.ToString().Split('&')[1].ToString(), m_clinicalPathInfo.Ljdm); // 3500分组标志
                }
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #region 确定后





        #endregion
        private void activityOrderControl_AfterDrugLoadedEvent(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #region GRID 鼠标
        /// <summary>
        /// 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnActivityMenuItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RadMenuItem clickedItem = ((RadRoutedEventArgs)e).OriginalSource as RadMenuItem;
                if (clickedItem != null)
                {
                    TagName tagName = (TagName)clickedItem.Tag;
                    object selectedItem = GridViewYZXX.SelectedItem;
                    DialogParameters parameters = new DialogParameters();
                    switch (tagName)
                    {
                        case TagName.Edit:
                            CurrentState = PageState.Edit;
                            zdm.Clear();
                            zxsj.Clear();
                            #region AdviceGroupDetail -> CP_DoctorOrder
                            _cp_AdviceGroupDetail = this.GridViewYZXX.SelectedItem as CP_AdviceGroupDetail;
                            CP_DoctorOrder order = new CP_DoctorOrder();
                            order.Ctmxxh = _cp_AdviceGroupDetail.Ctmxxh;
                            order.Cdxh = _cp_AdviceGroupDetail.Cdxh;
                            order.Ggxh = _cp_AdviceGroupDetail.Ggxh;
                            order.Lcxh = _cp_AdviceGroupDetail.Lcxh;
                            order.Ypdm = _cp_AdviceGroupDetail.Ypdm;
                            order.Xmlb = _cp_AdviceGroupDetail.Xmlb;
                            order.Yzlb = _cp_AdviceGroupDetail.Yzlb;
                            order.Yzbz = _cp_AdviceGroupDetail.Yzbz;
                            order.YzbzName = _cp_AdviceGroupDetail.YzbzName;
                            order.Ypjl = _cp_AdviceGroupDetail.Ypjl;
                            order.Jldw = _cp_AdviceGroupDetail.Jldw;
                            order.Yfdm = _cp_AdviceGroupDetail.Yfdm;
                            //order.YfdmName = _cp_AdviceGroupDetail.yfdm
                            order.Pcdm = _cp_AdviceGroupDetail.Pcdm;      //
                            //order.PcdmName = _cp_AdviceGroupDetail.pc
                            //order.Ksrq = \_cp_AdviceGroupDetail.ks
                            order.Ypmc = _cp_AdviceGroupDetail.Ypmc;
                            //order.FromTable = row["FromTable"].ToString();//
                            order.Flag = _cp_AdviceGroupDetail.Flag;
                            order.OrderGuid = Guid.NewGuid().ToString();//注意此处
                            order.Fzbz = _cp_AdviceGroupDetail.Fzbz;
                            order.Fzxh = _cp_AdviceGroupDetail.Fzxh;
                            order.Zxdw = _cp_AdviceGroupDetail.Zxdw;
                            //order.Ypgg = _cp_AdviceGroupDetail.ypg
                            order.Dwxs = _cp_AdviceGroupDetail.Dwxs;
                            order.Dwlb = _cp_AdviceGroupDetail.Dwlb;
                            order.Zxcs = _cp_AdviceGroupDetail.Zxcs;
                            order.Zxzq = _cp_AdviceGroupDetail.Zxzq;
                            order.Zxzqdw = _cp_AdviceGroupDetail.Zxzqdw;
                            order.Zdm = _cp_AdviceGroupDetail.Zdm;
                            order.Zxsj = _cp_AdviceGroupDetail.Zxsj;
                            order.Yznr = _cp_AdviceGroupDetail.Yznr;
                            order.Ztnr = _cp_AdviceGroupDetail.Ztnr;
                            //order.Yzzt = _cp_AdviceGroupDetail.y
                            #endregion
                            activityOrderControl.ManualType = ManualType.Edit;
                            activityOrderControl.CP_AdviceGroupDetailProptery = order;
                            activityOrderControl.InitModifyOrder();
                            break;
                        case TagName.Del:
                            foreach (CP_AdviceGroupDetail cp in this.GridViewYZXX.SelectedItems)
                            {
                                if (cp.Flag != "")
                                {
                                    PublicMethod.RadAlterBox("存在分组不能删除", HeaderText);
                                    return;
                                }
                            }
                            //parameters.Content = String.Format("提示: {0}", "确认删除吗？");
                            //parameters.Header = HeaderText;
                            //parameters.IconContent = null;
                            //parameters.OkButtonContent = "确定";
                            //parameters.CancelButtonContent = "取消";
                            //parameters.Closed = OnDelAdviceGroupDetail;//***close处理***
                            //RadWindow.Confirm(parameters);
                            YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                            mess.ShowDialog();
                            mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent3);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 右键打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnActivityMenuOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                GridViewRow row = ((RadRoutedEventArgs)e).OriginalSource as GridViewRow;
                List<CP_AdviceGroupDetail> listsOrder = new List<CP_AdviceGroupDetail>();
                var RadMenu = sender as RadContextMenu;
                Boolean IsSelectItemsHaveDefferentGroup = false;
                Boolean IsSelectItemsHaveGroupItem = false;
                foreach (RadMenuItem item in RadMenu.Items)
                {
                    if (row != null && !row.IsSelected)
                    {
                        item.IsEnabled = false;
                    }
                    else
                        if (item.Tag != null)
                        {
                            if ((TagName)item.Tag == TagName.Edit)
                            {
                                item.IsEnabled = !(this.GridViewYZXX.SelectedItems.Count > 1);
                            }
                            if ((TagName)item.Tag == TagName.Group)
                            {
                                #region 判断选中的数据中是否同时包含长期医嘱和临时医嘱，是否包含已成组项目
                                for (int i = 0; i < GridViewYZXX.SelectedItems.Count; i++)
                                {
                                    if (i > 0 && !IsSelectItemsHaveDefferentGroup)
                                        IsSelectItemsHaveDefferentGroup =
                                            ((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i - 1]).Yzbz != ((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i]).Yzbz;
                                    if (((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i]).Fzbz != 3500) IsSelectItemsHaveGroupItem = true;
                                }
                                #endregion
                                item.IsEnabled = (this.GridViewYZXX.SelectedItems.Count > 1 && !IsSelectItemsHaveDefferentGroup && !IsSelectItemsHaveGroupItem);
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #endregion
        #region  护理及宣教
        /// <summary>
        ///   护理及宣教医嘱输入初始化
        /// </summary>
        private void InitCareControl()
        {
            careOrderControl.PanelCategory = OrderPanelBarCategory.Care;
            careOrderControl.OrderCategory = OrderItemCategory.Care;
            careOrderControl.AfterDrugLoadedEvent += new UCOtherOrder.DrugLoaded(careOrderControl_AfterDrugLoadedEvent);
            careOrderControl.AfterDrugCinfirmeddEvent += new UCOtherOrder.DrugConfirmed(careOrderControl_AfterDrugCinfirmeddEvent);
        }
        private void careOrderControl_AfterDrugCinfirmeddEvent(object sender, CP_DoctorOrder e)
        {
            try
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                if (CurrentState == PageState.Edit)
                {
                    #region 实例 化新的实体
                    _cp_AdviceGroupDetail.Yzbz = Int16.Parse(e.Yzbz.ToString());
                    _cp_AdviceGroupDetail.Ypdm = e.Ypdm;
                    _cp_AdviceGroupDetail.Cdxh = e.Cdxh;
                    _cp_AdviceGroupDetail.Ggxh = e.Ggxh;
                    _cp_AdviceGroupDetail.Lcxh = e.Lcxh;
                    _cp_AdviceGroupDetail.Ypmc = e.Ypmc;
                    _cp_AdviceGroupDetail.Xmlb = Int16.Parse(e.Xmlb.ToString());
                    _cp_AdviceGroupDetail.Zxdw = e.Zxdw;
                    _cp_AdviceGroupDetail.Ypjl = e.Ypjl;
                    _cp_AdviceGroupDetail.Jldw = cbxMDDW.Text;
                    _cp_AdviceGroupDetail.Dwxs = e.Dwxs;
                    _cp_AdviceGroupDetail.Dwlb = Int32.Parse(e.Dwlb.ToString());
                    //_cp_AdviceGroupDetail.Yfdm = e.Yfdm;
                    _cp_AdviceGroupDetail.Pcdm = e.Pcdm;
                    _cp_AdviceGroupDetail.Zxcs = Int32.Parse(e.Zxcs.ToString());
                    _cp_AdviceGroupDetail.Zxzq = Int32.Parse(e.Zxzq.ToString());
                    _cp_AdviceGroupDetail.Zxzqdw = Int16.Parse(e.Zxzqdw.ToString());
                    _cp_AdviceGroupDetail.Zdm = e.Zdm;
                    _cp_AdviceGroupDetail.Zxsj = e.Zxsj;
                    _cp_AdviceGroupDetail.Zxts = int.Parse(e.Zxts.ToString());
                    _cp_AdviceGroupDetail.Ypzsl = e.Ypzsl;
                    _cp_AdviceGroupDetail.Ztnr = e.Ztnr;
                    _cp_AdviceGroupDetail.Yzlb = Int16.Parse(e.Yzlb.ToString());
                    #endregion
                    client.UpdateAdviceGroupDetailCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    
                                    BindGridData(Convert.ToString((int)OrderPanelBarCategory.Care), nodestr.ToString().Split('&')[0].ToString());
                                    NewAdviceGroupDetail();
                                    PublicMethod.RadAlterBox(ea.Result, HeaderText);
                                }
                                else
                                {
                                    PublicMethod.RadWaringBox(ea.Error);
                                }
                            };
                    client.UpdateAdviceGroupDetailAsync(_cp_AdviceGroupDetail.Fzxh, _cp_AdviceGroupDetail.Ctmxxh, _cp_AdviceGroupDetail, false);
                }
                else if (CurrentState == PageState.New)
                {
                    #region 实例 化新的实体
                    CP_AdviceGroupDetail order = new CP_AdviceGroupDetail();
                    order.Fzbz = 1;
                    order.Yzbz = Int16.Parse(e.Yzbz.ToString());
                    order.Ypdm = e.Ypdm;
                    order.Cdxh = e.Cdxh;
                    order.Ggxh = e.Ggxh;
                    order.Lcxh = e.Lcxh;
                    order.Ypmc = e.Ypmc;
                    order.Xmlb = Int16.Parse(e.Xmlb.ToString());
                    order.Zxdw = e.Zxdw;
                    order.Ypjl = e.Ypjl;
                    order.Jldw = cbxMDDW.Text;
                    order.Dwxs = e.Dwxs;
                    order.Dwlb = Int32.Parse(e.Dwlb.ToString());
                    //order.Yfdm = e.Yfdm;
                    order.Pcdm = e.Pcdm;
                    order.Zxcs = Int32.Parse(e.Zxcs.ToString());
                    order.Zxzq = Int32.Parse(e.Zxzq.ToString());
                    order.Zxzqdw = Int16.Parse(e.Zxzqdw.ToString());
                    order.Zdm = e.Zdm;
                    order.Zxsj = e.Zxsj;
                    order.Zxts = int.Parse(e.Zxts.ToString());
                    order.Ypzsl = e.Ypzsl;
                    order.Ztnr = e.Ztnr;
                    order.Yzlb = Int16.Parse(e.Yzlb.ToString());
                    #endregion
                    client.InsertIntoAdviceGroupCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                
                                BindGridData(Convert.ToString((int)OrderPanelBarCategory.Care), nodestr.ToString().Split('&')[0].ToString());
                                NewAdviceGroupDetail();
                                PublicMethod.RadAlterBox(ea.Result, HeaderText);
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    client.InsertIntoAdviceGroupAsync(order, nodestr.ToString().Split('&')[0].ToString(), 3500, nodestr.ToString().Split('&')[1].ToString(), m_clinicalPathInfo.Ljdm); // 3500分组标志
                }
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #region 确定后





        #endregion
        private void careOrderControl_AfterDrugLoadedEvent(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #region GRID 鼠标
        /// <summary>
        /// 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCareMenuItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RadMenuItem clickedItem = ((RadRoutedEventArgs)e).OriginalSource as RadMenuItem;
                if (clickedItem != null)
                {
                    TagName tagName = (TagName)clickedItem.Tag;
                    object selectedItem = GridViewYZXX.SelectedItem;
                    DialogParameters parameters = new DialogParameters();
                    switch (tagName)
                    {
                        case TagName.Edit:
                            CurrentState = PageState.Edit;
                            zdm.Clear();
                            zxsj.Clear();
                            #region AdviceGroupDetail -> CP_DoctorOrder
                            _cp_AdviceGroupDetail = this.GridViewYZXX.SelectedItem as CP_AdviceGroupDetail;
                            CP_DoctorOrder order = new CP_DoctorOrder();
                            order.Ctmxxh = _cp_AdviceGroupDetail.Ctmxxh;
                            order.Cdxh = _cp_AdviceGroupDetail.Cdxh;
                            order.Ggxh = _cp_AdviceGroupDetail.Ggxh;
                            order.Lcxh = _cp_AdviceGroupDetail.Lcxh;
                            order.Ypdm = _cp_AdviceGroupDetail.Ypdm;
                            order.Xmlb = _cp_AdviceGroupDetail.Xmlb;
                            order.Yzlb = _cp_AdviceGroupDetail.Yzlb;
                            order.Yzbz = _cp_AdviceGroupDetail.Yzbz;
                            order.YzbzName = _cp_AdviceGroupDetail.YzbzName;
                            order.Ypjl = _cp_AdviceGroupDetail.Ypjl;
                            order.Jldw = _cp_AdviceGroupDetail.Jldw;
                            order.Yfdm = _cp_AdviceGroupDetail.Yfdm;
                            //order.YfdmName = _cp_AdviceGroupDetail.yfdm
                            order.Pcdm = _cp_AdviceGroupDetail.Pcdm;      //
                            //order.PcdmName = _cp_AdviceGroupDetail.pc
                            //order.Ksrq = \_cp_AdviceGroupDetail.ks
                            order.Ypmc = _cp_AdviceGroupDetail.Ypmc;
                            //order.FromTable = row["FromTable"].ToString();//
                            order.Flag = _cp_AdviceGroupDetail.Flag;
                            order.OrderGuid = Guid.NewGuid().ToString();//注意此处
                            order.Fzbz = _cp_AdviceGroupDetail.Fzbz;
                            order.Fzxh = _cp_AdviceGroupDetail.Fzxh;
                            order.Zxdw = _cp_AdviceGroupDetail.Zxdw;
                            //order.Ypgg = _cp_AdviceGroupDetail.ypg
                            order.Dwxs = _cp_AdviceGroupDetail.Dwxs;
                            order.Dwlb = _cp_AdviceGroupDetail.Dwlb;
                            order.Zxcs = _cp_AdviceGroupDetail.Zxcs;
                            order.Zxzq = _cp_AdviceGroupDetail.Zxzq;
                            order.Zxzqdw = _cp_AdviceGroupDetail.Zxzqdw;
                            order.Zdm = _cp_AdviceGroupDetail.Zdm;
                            order.Zxsj = _cp_AdviceGroupDetail.Zxsj;
                            order.Yznr = _cp_AdviceGroupDetail.Yznr;
                            order.Ztnr = _cp_AdviceGroupDetail.Ztnr;
                            //order.Yzzt = _cp_AdviceGroupDetail.y
                            #endregion
                            careOrderControl.ManualType = ManualType.Edit;
                            careOrderControl.CP_AdviceGroupDetailProptery = order;
                            careOrderControl.InitModifyOrder();
                            break;
                        case TagName.Del:
                            foreach (CP_AdviceGroupDetail cp in this.GridViewYZXX.SelectedItems)
                            {
                                if (cp.Flag != "")
                                {
                                    PublicMethod.RadAlterBox("存在分组不能删除", HeaderText);
                                    return;
                                }
                            }
                            //parameters.Content = String.Format("提示: {0}", "确认删除吗？");
                            //parameters.Header = HeaderText;
                            //parameters.IconContent = null;
                            //parameters.OkButtonContent = "确定";
                            //parameters.CancelButtonContent = "取消";
                            //parameters.Closed = OnDelAdviceGroupDetail;//***close处理***
                            //RadWindow.Confirm(parameters);
                            YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                            mess.ShowDialog();
                            mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent3);
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 右键打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnCareMenuOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                GridViewRow row = ((RadRoutedEventArgs)e).OriginalSource as GridViewRow;
                List<CP_AdviceGroupDetail> listsOrder = new List<CP_AdviceGroupDetail>();
                var RadMenu = sender as RadContextMenu;
                Boolean IsSelectItemsHaveDefferentGroup = false;
                Boolean IsSelectItemsHaveGroupItem = false;
                foreach (RadMenuItem item in RadMenu.Items)
                {
                    if (row != null && !row.IsSelected)
                    {
                        item.IsEnabled = false;
                    }
                    else
                        if (item.Tag != null)
                        {
                            if ((TagName)item.Tag == TagName.Edit)
                            {
                                item.IsEnabled = !(this.GridViewYZXX.SelectedItems.Count > 1);
                            }
                            if ((TagName)item.Tag == TagName.Group)
                            {
                                #region 判断选中的数据中是否同时包含长期医嘱和临时医嘱，是否包含已成组项目
                                for (int i = 0; i < GridViewYZXX.SelectedItems.Count; i++)
                                {
                                    if (i > 0 && !IsSelectItemsHaveDefferentGroup)
                                        IsSelectItemsHaveDefferentGroup =
                                            ((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i - 1]).Yzbz != ((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i]).Yzbz;
                                    if (((CP_AdviceGroupDetail)GridViewYZXX.SelectedItems[i]).Fzbz != 3500) IsSelectItemsHaveGroupItem = true;
                                }
                                #endregion
                                item.IsEnabled = (this.GridViewYZXX.SelectedItems.Count > 1 && !IsSelectItemsHaveDefferentGroup && !IsSelectItemsHaveGroupItem);
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #endregion
        #region GridView事件
        private void GridViewYZXX_RowLoaded(object sender, RowLoadedEventArgs e)
        {
            try
            {
                if (this.OrderPanelBar.SelectedItem == null) return;
                OrderPanelBarCategory barItemTag = (OrderPanelBarCategory)(int.Parse(((RadPanelBarItem)this.OrderPanelBar.SelectedItem).Tag.ToString()));
                RadContextMenu rowContextMenu = new RadContextMenu(); //新建一个右键菜单
                switch (barItemTag)
                {
                    #region 药品菜单
                    case OrderPanelBarCategory.Drug:
                        if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
                        rowContextMenu.Width = 200;
                        // rowContextMenu.Items.Add(new RadMenuItem() { Header = "新增医嘱", Tag = TagName.New });
                        rowContextMenu.Items.Add(new RadMenuItem() { Header = "编辑医嘱", Tag = TagName.Edit });
                        rowContextMenu.Items.Add(new RadMenuItem() { IsSeparator = true });
                        rowContextMenu.Items.Add(new RadMenuItem() { Header = "删除医嘱", Tag = TagName.Del });
                        rowContextMenu.Items.Add(new RadMenuItem() { IsSeparator = true });
                        rowContextMenu.Items.Add(new RadMenuItem() { Header = "医嘱成组", Tag = TagName.Group });
                        rowContextMenu.Items.Add(new RadMenuItem() { Header = "取消成组", Tag = TagName.DisGroup });
                        //rowContextMenu.Items.Add(new RadMenuItem() { IsSeparator = true });
                        //  rowContextMenu.Items.Add(new RadMenuItem() { Header = "取消多选", Tag = TagName.SelectMuti });
                        rowContextMenu.AddHandler(RadMenuItem.ClickEvent, new RoutedEventHandler(OnMenuItemClick));
                        rowContextMenu.Opened += new RoutedEventHandler(OnMenuOpened);
                        RadContextMenu.SetContextMenu(e.Row, rowContextMenu);
                        break;
                    #endregion
                    #region 手术菜单
                    case OrderPanelBarCategory.Oper:
                        if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
                        rowContextMenu.Width = 200;
                        rowContextMenu.Items.Add(new RadMenuItem() { Header = "编辑医嘱", Tag = TagName.Edit });
                        rowContextMenu.Items.Add(new RadMenuItem() { IsSeparator = true });
                        rowContextMenu.Items.Add(new RadMenuItem() { Header = "删除医嘱", Tag = TagName.Del });
                        rowContextMenu.AddHandler(RadMenuItem.ClickEvent, new RoutedEventHandler(OnSSMenuItemClick));
                        rowContextMenu.Opened += new RoutedEventHandler(OnSSMenuOpened);//  
                        RadContextMenu.SetContextMenu(e.Row, rowContextMenu);
                        break;
                    #endregion
                    #region 检验检查
                    case OrderPanelBarCategory.RisLis:
                        if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
                        rowContextMenu.Width = 200;
                        rowContextMenu.Items.Add(new RadMenuItem() { Header = "编辑医嘱", Tag = TagName.Edit });
                        rowContextMenu.Items.Add(new RadMenuItem() { IsSeparator = true });
                        rowContextMenu.Items.Add(new RadMenuItem() { Header = "删除医嘱", Tag = TagName.Del });
                        rowContextMenu.AddHandler(RadMenuItem.ClickEvent, new RoutedEventHandler(OnRISLISMenuItemClick));
                        rowContextMenu.Opened += new RoutedEventHandler(OnRISLISMenuOpened);
                        RadContextMenu.SetContextMenu(e.Row, rowContextMenu);
                        break;
                    #endregion
                    #region 营养膳食
                    case OrderPanelBarCategory.Meal:
                        if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
                        rowContextMenu.Width = 200;
                        rowContextMenu.Items.Add(new RadMenuItem() { Header = "编辑医嘱", Tag = TagName.Edit });
                        rowContextMenu.Items.Add(new RadMenuItem() { IsSeparator = true });
                        rowContextMenu.Items.Add(new RadMenuItem() { Header = "删除医嘱", Tag = TagName.Del });
                        rowContextMenu.AddHandler(RadMenuItem.ClickEvent, new RoutedEventHandler(OnMealMenuItemClick));
                        rowContextMenu.Opened += new RoutedEventHandler(OnMealMenuOpened);
                        RadContextMenu.SetContextMenu(e.Row, rowContextMenu);
                        break;
                    #endregion
                    #region 观察
                    case OrderPanelBarCategory.Observation:
                        if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
                        rowContextMenu.Width = 200;
                        rowContextMenu.Items.Add(new RadMenuItem() { Header = "编辑医嘱", Tag = TagName.Edit });
                        rowContextMenu.Items.Add(new RadMenuItem() { IsSeparator = true });
                        rowContextMenu.Items.Add(new RadMenuItem() { Header = "删除医嘱", Tag = TagName.Del });
                        rowContextMenu.AddHandler(RadMenuItem.ClickEvent, new RoutedEventHandler(OnObservationMenuItemClick));
                        rowContextMenu.Opened += new RoutedEventHandler(OnObservationMenuOpened);
                        RadContextMenu.SetContextMenu(e.Row, rowContextMenu);
                        break;
                    #endregion
                    #region 活动
                    case OrderPanelBarCategory.Activity:
                        if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
                        rowContextMenu.Width = 200;
                        rowContextMenu.Items.Add(new RadMenuItem() { Header = "编辑医嘱", Tag = TagName.Edit });
                        rowContextMenu.Items.Add(new RadMenuItem() { IsSeparator = true });
                        rowContextMenu.Items.Add(new RadMenuItem() { Header = "删除医嘱", Tag = TagName.Del });
                        rowContextMenu.AddHandler(RadMenuItem.ClickEvent, new RoutedEventHandler(OnActivityMenuItemClick));
                        rowContextMenu.Opened += new RoutedEventHandler(OnActivityMenuOpened);
                        RadContextMenu.SetContextMenu(e.Row, rowContextMenu);
                        break;
                    #endregion
                    #region 活动
                    case OrderPanelBarCategory.Care:
                        if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
                        rowContextMenu.Width = 200;
                        rowContextMenu.Items.Add(new RadMenuItem() { Header = "编辑医嘱", Tag = TagName.Edit });
                        rowContextMenu.Items.Add(new RadMenuItem() { IsSeparator = true });
                        rowContextMenu.Items.Add(new RadMenuItem() { Header = "删除医嘱", Tag = TagName.Del });
                        rowContextMenu.AddHandler(RadMenuItem.ClickEvent, new RoutedEventHandler(OnCareMenuItemClick));
                        rowContextMenu.Opened += new RoutedEventHandler(OnCareMenuOpened);
                        RadContextMenu.SetContextMenu(e.Row, rowContextMenu);
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region RadPanelBar导航
        /// <summary>
        /// 选择左边栏位
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OrderPanelBar_Selected(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                GetDrugInfo();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void SetGridVisible(OrderPanelBarCategory barItemTag)
        {
            GrdConditonListNode.Visibility = barItemTag == OrderPanelBarCategory.EnterCondition ? Visibility.Visible : Visibility.Collapsed;
            GridViewYZXX.Visibility = GrdConditonListNode.Visibility == Visibility.Visible ? Visibility.Collapsed : Visibility.Visible;
            if (m_bAduit)
            {

            }
            else
            {
                btnConditionMaintain.Visibility = Visibility.Collapsed;
                switch (barItemTag)
                {
                    case OrderPanelBarCategory.Drug:
                        YPGrid.Visibility = Visibility.Visible;
                        SSGrid.Visibility = Visibility.Collapsed;
                        rislisGrid.Visibility = Visibility.Collapsed;
                        mealGrid.Visibility = Visibility.Collapsed;
                        observationGrid.Visibility = Visibility.Collapsed;
                        activityGrid.Visibility = Visibility.Collapsed;
                        careGrid.Visibility = Visibility.Collapsed;
                        break;
                    case OrderPanelBarCategory.Oper:
                        YPGrid.Visibility = Visibility.Collapsed;
                        SSGrid.Visibility = Visibility.Visible;
                        rislisGrid.Visibility = Visibility.Collapsed;
                        mealGrid.Visibility = Visibility.Collapsed;
                        observationGrid.Visibility = Visibility.Collapsed;
                        activityGrid.Visibility = Visibility.Collapsed;
                        careGrid.Visibility = Visibility.Collapsed;
                        break;
                    case OrderPanelBarCategory.RisLis:
                        YPGrid.Visibility = Visibility.Collapsed;
                        SSGrid.Visibility = Visibility.Collapsed;
                        rislisGrid.Visibility = Visibility.Visible;
                        mealGrid.Visibility = Visibility.Collapsed;
                        observationGrid.Visibility = Visibility.Collapsed;
                        activityGrid.Visibility = Visibility.Collapsed;
                        careGrid.Visibility = Visibility.Collapsed;
                        break;
                    case OrderPanelBarCategory.Meal:
                        YPGrid.Visibility = Visibility.Collapsed;
                        SSGrid.Visibility = Visibility.Collapsed;
                        rislisGrid.Visibility = Visibility.Collapsed;
                        mealGrid.Visibility = Visibility.Visible;
                        observationGrid.Visibility = Visibility.Collapsed;
                        activityGrid.Visibility = Visibility.Collapsed;
                        careGrid.Visibility = Visibility.Collapsed;
                        break;
                    case OrderPanelBarCategory.Observation:
                        YPGrid.Visibility = Visibility.Collapsed;
                        SSGrid.Visibility = Visibility.Collapsed;
                        rislisGrid.Visibility = Visibility.Collapsed;
                        mealGrid.Visibility = Visibility.Collapsed;
                        observationGrid.Visibility = Visibility.Visible;
                        activityGrid.Visibility = Visibility.Collapsed;
                        careGrid.Visibility = Visibility.Collapsed;
                        break;
                    case OrderPanelBarCategory.Activity:
                        YPGrid.Visibility = Visibility.Collapsed;
                        SSGrid.Visibility = Visibility.Collapsed;
                        rislisGrid.Visibility = Visibility.Collapsed;
                        mealGrid.Visibility = Visibility.Collapsed;
                        observationGrid.Visibility = Visibility.Collapsed;
                        activityGrid.Visibility = Visibility.Visible;
                        careGrid.Visibility = Visibility.Collapsed;
                        break;
                    case OrderPanelBarCategory.Care:
                        YPGrid.Visibility = Visibility.Collapsed;
                        SSGrid.Visibility = Visibility.Collapsed;
                        rislisGrid.Visibility = Visibility.Collapsed;
                        mealGrid.Visibility = Visibility.Collapsed;
                        observationGrid.Visibility = Visibility.Collapsed;
                        activityGrid.Visibility = Visibility.Collapsed;
                        careGrid.Visibility = Visibility.Visible;
                        break;
                    case OrderPanelBarCategory.EnterCondition:
                           YPGrid.Visibility = Visibility.Collapsed;
                        SSGrid.Visibility = Visibility.Collapsed;
                        rislisGrid.Visibility = Visibility.Collapsed;
                        mealGrid.Visibility = Visibility.Collapsed;
                        observationGrid.Visibility = Visibility.Collapsed;
                        activityGrid.Visibility = Visibility.Collapsed;
                        careGrid.Visibility = Visibility.Collapsed;
                        //GridViewYZXX.Visibility = Visibility.Collapsed;
                        //GrdConditonListNode.Visibility = Visibility.Visible;
                        btnConditionMaintain.Visibility = Visibility.Visible;
                        break;
                    default:
                        YPGrid.Visibility = Visibility.Collapsed;
                        SSGrid.Visibility = Visibility.Collapsed;
                        rislisGrid.Visibility = Visibility.Collapsed;
                        mealGrid.Visibility = Visibility.Collapsed;
                        observationGrid.Visibility = Visibility.Collapsed;
                        activityGrid.Visibility = Visibility.Collapsed;
                        careGrid.Visibility = Visibility.Collapsed;
                        break;
                }
            }
        }
        private void GetDrugInfo()
        {
            RadPanelBarItem barItem = (RadPanelBarItem)this.OrderPanelBar.SelectedItem;
            this.CurrentState = PageState.New;
            if (barItem == null || barItem.Tag == null || barItem.Tag.ToString().Trim() == "")
            {
                //PublicMethod.RadAlterBox("建设中...敬请期待", HeaderText);
                return;
            }
            OrderPanelBarCategory barItemTag = (OrderPanelBarCategory)(int.Parse(barItem.Tag.ToString()));
            if (nodestr.ToString().Split('&')[0].ToString() != "")
            {

                SetGridVisible(barItemTag);
                switch (barItemTag)
                {
                    #region 药品
                    case OrderPanelBarCategory.Drug:
                        #region 后台绑定模板列
                        DataTemplate DT = (DataTemplate)this.FindName("ComboBoxXmTemplate");
                        cbxMDYPMC.ItemTemplate = DT;
                        //cbxMDYPMC.SelectionBoxTemplate = (DataTemplate)this.FindName("ComboBoxXmSimpleTemplate");
                        #endregion
                        InitDrugInfo();//初始化药品信息//项目下拉框
                        InitOrderTypeInfo(cbxMDYZBZ); //初始化医嘱类别（临时医嘱，长期医嘱）

                        InitJJTypeInfo(cbxJJLX);// add by luff 20130121 初始化计价类型
                        IntiComboBoxDept();// add by luff 20130121 初始化科室
                        #region add by luff 20130313 获得配置表关于医嘱可选不算变异参数 若值为1表示可选，0表示必须
                         
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

                        BindGridData(Convert.ToString((int)barItemTag), nodestr.ToString().Split('&')[0].ToString()); //重新加载Grid
                        break;
                    #endregion
                    #region 手术
                    case OrderPanelBarCategory.Oper:
                        InitOrderTypeInfo(cbxSSYZBZ);
                        cbxSSYZBZ.SelectedIndex = 0;
                        cbxSSYZBZ.IsEnabled = false;
                        DataTemplate DTSS = (DataTemplate)this.FindName("ComboBoxSSTemplate");
                        cbxSSMC.ItemTemplate = DTSS;
                        //cbxSSMC.SelectionBoxTemplate = (DataTemplate)this.FindName("ComboBoxSSSimpleTemplate");
                        InitOperationInfo();
                        InitAnesthesiaInfo();
                        //BindSSGridData(nodestr.ToString().Split('&')[0].ToString());
                        BindGridData(((RadPanelBarItem)this.OrderPanelBar.SelectedItem).Tag.ToString(), nodestr.ToString().Split('&')[0].ToString());
                        break;
                    #endregion
                    #region 检验检查
                    case OrderPanelBarCategory.RisLis:
                        BindGridData(Convert.ToString((int)barItemTag), nodestr.ToString().Split('&')[0].ToString()); //重新加载Grid
                        risLisOrderControl.NewAdviceGroupDetail();
                        break;
                    #endregion
                    #region 营养膳食
                    case OrderPanelBarCategory.Meal:
                        BindGridData(Convert.ToString((int)barItemTag), nodestr.ToString().Split('&')[0].ToString()); //重新加载Grid
                        foodOrderControl.NewAdviceGroupDetail();
                        break;
                    #endregion
                    #region 观察
                    case OrderPanelBarCategory.Observation:
                        BindGridData(Convert.ToString((int)barItemTag), nodestr.ToString().Split('&')[0].ToString()); //重新加载Grid 
                        observationOrderControl.NewAdviceGroupDetail();
                        break;
                    #endregion
                    #region 活动
                    case OrderPanelBarCategory.Activity:
                        BindGridData(Convert.ToString((int)barItemTag), nodestr.ToString().Split('&')[0].ToString()); //重新加载Grid 
                        activityOrderControl.NewAdviceGroupDetail();
                        break;
                    #endregion
                    #region 护理及宣教
                    case OrderPanelBarCategory.Care:
                        BindGridData(Convert.ToString((int)barItemTag), nodestr.ToString().Split('&')[0].ToString()); //重新加载Grid
                        careOrderControl.NewAdviceGroupDetail();
                        break;
                    #endregion
                    #region 进入条件
                    case OrderPanelBarCategory.EnterCondition:
                        //btnConditionMaintain.Visibility = Visibility.Visible;
                        //RWEnterConditionMaintainNode RWEnterConditionMaintainNodeTemp = new RWEnterConditionMaintainNode(m_clinicalPathInfo.Ljdm, nodestr.ToString().Split('&')[0].ToString());
                        //RWEnterConditionMaintainNodeTemp.ShowDialog();
                        _LjdmNode = m_clinicalPathInfo.Ljdm;
                        _NodeGUIDNode = nodestr.ToString().Split('&')[0].ToString();
                        //CurrentOperationStateNode = OperationState.NEW;
                        YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                        //#region 绑定类型下拉框
                        //KeyValues keyValues = new KeyValues();
                        //keyValues.Add(new KeyValue("2", "ICD-10"));
                        //keyValues.Add(new KeyValue("1", "检查项"));
                        //cmbConditionTypeNode.ItemsSource = keyValues;
                        //cmbConditionTypeNode.DisplayMemberPath = "Value";
                        //cmbConditionTypeNode.SelectedValuePath = "Key";
                        //cmbConditionTypeNode.SelectedValue = "1";
                        //cmbConditionTypeNode.IsEnabled = false;
                        //#endregion
                        //#region 绑定非ICD自动完成框
                        //referenceClient.GetCP_ExamDictionaryDetailAllCompleted += (send2, ea2) =>
                        //    {
                        //        autoCompleteNonICD10Node.ItemsSource = ea2.Result;
                        //        autoCompleteNonICD10Node.ItemFilter = DeptFilterNonICDNode;
                        //    };
                        //referenceClient.GetCP_ExamDictionaryDetailAllAsync();
                        //#endregion
                        #region 绑定列表
                        BindGridViewNode();
                        #endregion
                        referenceClient.CloseAsync();
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            else
            {
                PublicMethod.RadAlterBox("先选择对应的节点", HeaderText);
                GridViewYZXX.ItemsSource = null;
                return;
            }
        }
        #endregion
        #endregion
        #region KEYUP
        /// <summary>
        /// 注册KEYUP
        /// </summary>
        private void RegisterKeyEvent()
        {
            #region 药品
            cbxMDYZBZ.KeyUp += new KeyEventHandler(cbxMDYZBZ_KeyUp);
            cbxMDYPMC.KeyUp += new KeyEventHandler(cbxMDYPMC_KeyUp);
            nudMDSL.KeyUp += new KeyEventHandler(nudMDSL_KeyUp);
            cbxMDDW.KeyUp += new KeyEventHandler(cbxMDDW_KeyUp);
            cbxMDYF.KeyUp += new KeyEventHandler(cbxMDYF_KeyUp);
            cbxPC.KeyUp += new KeyEventHandler(cbxPC_KeyUp);
            cbxSJ.KeyUp += new KeyEventHandler(cbxSJ_KeyUp);
            txtZTNR.KeyUp += new KeyEventHandler(txtZTNR_KeyUp);
            #endregion
            #region 手术
            cbxSSMC.KeyUp += new KeyEventHandler(cbxSSMC_KeyUp);
            cbxSSMZ.KeyUp += new KeyEventHandler(cbxSSMZ_KeyUp);
            txtSSZTNR.KeyUp += new KeyEventHandler(txtSSZTNR_KeyUp);
            #endregion
        }
        /// <summary>
        /// 项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSSMC_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbxSSMZ.Focus();
        }
        /// <summary>
        /// 麻醉
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSSMZ_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtSSZTNR.Focus();
        }
        /// <summary>
        /// 嘱托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtSSZTNR_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSSQD.Focus();
        }
        #region 药品
        /// <summary>
        /// 医嘱类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMDYZBZ_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbxMDYPMC.Focus();
        }
        /// <summary>
        /// 项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMDYPMC_KeyUp(object sender, KeyEventArgs e)
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
        #region 手术
        #endregion
        #endregion
        #region 其他
        /// <summary>
        /// 页面状态
        /// </summary>
        enum PageState
        {
            New = 0,
            Edit = 1
        }
        /// <summary>
        /// 右键菜单枚举
        /// </summary>
        private enum TagName
        {
            New,
            Edit,
            Del,
            Group,
            DisGroup,
            SelectMuti
        }
        /// <summary>
        /// 添加List 到 ObservableCollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(IEnumerable<T> coll)
        {
            var c = new ObservableCollection<T>();
            foreach (var e in coll)
                c.Add(e);
            return c;
        }
        #endregion
        #region 护理项目,add by xjt,20110214
        ObservableCollection<CP_NurExecInfo> m_NurExecInfos;
        /// <summary>
        /// 获取护理执行基本信息并显示
        /// </summary>
        private void GetNurBasicInfo()
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.GetNurExecInfoAsync();
            radBusyIndicatorNur.IsBusy = true;
            client.GetNurExecInfoCompleted += (s, t) =>
            {
                radBusyIndicatorNur.IsBusy = false;
                if (t.Error == null)
                {
                    m_NurExecInfos = t.Result;
                    List<String> listCateInfo = GetCategoryList(m_NurExecInfos);
                    foreach (String str in listCateInfo)
                    {
                        var items = from info in m_NurExecInfos
                                    where info.Lbxh.Equals(str)
                                    orderby info.MxOrderValue
                                    select info;
                        ObservableCollection<CP_NurExecInfo> infos = new ObservableCollection<CP_NurExecInfo>();
                        items.ToList().ForEach(item => infos.Add(item));
                        UCNurExecItem exec = new UCNurExecItem(infos);
                        this.stpInfo.Children.Add(exec);
                    }
                }
                else
                {
                    PublicMethod.RadWaringBox(t.Error);
                }
            };
            client.CloseAsync();
        }
        /// <summary>
        /// 获取护理执行结点对应的执行信息
        /// </summary>
        /// <param name="listExecInfo"></param>
        /// <param name="strPathDetailId"></param>
        private void GetNurPathInfo(ObservableCollection<CP_NurExecInfo> listExecInfo, String strPathDetailId)
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.GetNurToPathInfoAsync(m_clinicalPathInfo.Ljdm, strPathDetailId);
            radBusyIndicatorNur.IsBusy = true;
            client.GetNurToPathInfoCompleted += (s, e) =>
            {
                radBusyIndicatorNur.IsBusy = false;
                ObservableCollection<CP_NurExecToPath> listInfo = e.Result;
                foreach (CP_NurExecInfo execinfo in listExecInfo)
                {
                    execinfo.ToPathId = 0;
                    execinfo.IsSelected = false;
                    execinfo.IsNew = true;
                    execinfo.IsModify = false;
                }
                foreach (CP_NurExecToPath pathinfo in listInfo)
                {
                    foreach (CP_NurExecInfo execinfo in listExecInfo)
                    {
                        if (execinfo.Mxxh == pathinfo.Mxxh)
                        {
                            execinfo.ToPathId = pathinfo.Id;
                            execinfo.IsSelected = true;
                            execinfo.IsNew = false;
                            break;
                        }
                    }
                }
            };
        }
        /// <summary>
        /// 获取项目名称
        /// </summary>
        /// <param name="infos"></param>
        /// <returns></returns>
        private List<String> GetCategoryList(ObservableCollection<CP_NurExecInfo> infos)
        {
            List<String> listInfo = new List<string>();
            foreach (CP_NurExecInfo info in infos)
            {
                if (!listInfo.Contains(info.Lbxh))
                    listInfo.Add(info.Lbxh);
            }
            return listInfo;
        }
        /// <summary>
        /// 保存护理信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNurSave_Click(object sender, RoutedEventArgs e)
        {
            // to do 获取实体新的保存，非新的且修改过的UPDATE.
            try
            {
                SaveToPahtInfo();
            }
            catch (Exception ex)
            {
                PublicMethod.InsertClientLogException(ex, this.GetType().FullName);
            }
        }
        /// <summary>
        /// 保存护理信息
        /// </summary>
        private void SaveToPahtInfo()
        {
            if (nodestr.ToString().Split('&')[0].ToString() == String.Empty)
            {
                PublicMethod.RadAlterBox("先选择对应的节点", HeaderText);
                return;
            }
            ObservableCollection<CP_NurExecInfo> listInfo = new ObservableCollection<CP_NurExecInfo>();
            foreach (Control ctl in stpInfo.Children)
            {
                if (ctl.GetType() == typeof(UCNurExecItem))
                {
                    ((UCNurExecItem)ctl).NurExecInfo.ToList().ForEach(s =>
                    {
                        s.Ljdm = m_clinicalPathInfo.Ljdm;
                        s.PathDetailId = nodestr.ToString().Split('&')[0].ToString();
                        listInfo.Add(s);
                    });
                }
            }
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.SaveNurExecToPathAsync(listInfo, Global.LogInEmployee.Zgdm);
            radBusyIndicatorNur.IsBusy = true;
            client.SaveNurExecToPathCompleted += (s, e) =>
            {
                radBusyIndicatorNur.IsBusy = false;
                if (e.Error == null)
                {
                    
                    GetNurPathInfo(m_NurExecInfos, nodestr.ToString().Split('&')[0].ToString());
                    PublicMethod.RadAlterBox("保存成功", HeaderText);
                }
                else
                {
                    PublicMethod.RadWaringBox(e.Error);
                }
            };
            client.CloseAsync();
        }
        #endregion
        #region 变异原因
        ObservableCollection<CP_VariationToPathInfo> m_PathVariation;
        /// <summary>
        /// 获取异常基本信息
        /// </summary>
        private void GetPathVariationBasicInfo()
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.GetPathVariationBasInfoAsync();
            radBusyIndicatorVariation.IsBusy = true;
            client.GetPathVariationBasInfoCompleted += (s, t) =>
            {
                radBusyIndicatorVariation.IsBusy = false;
                if (t.Error == null)
                {
                    m_PathVariation = t.Result;
                    foreach (CP_VariationToPathInfo info in m_PathVariation)
                    {
                        UCVariationCheckBox checkBox = new UCVariationCheckBox(info);
                        this.gridVariationDetail.Children.Add(checkBox);
                    }
                }
                else
                {
                    PublicMethod.RadWaringBox(t.Error);
                }
            };
            client.CloseAsync();
        }
        /// <summary>
        /// 获取结点对应的异常信息
        /// </summary>
        /// <param name="strActivityId"></param>
        private void GetVariationToPathInfo(ObservableCollection<CP_VariationToPathInfo> listInfo, String strActivityId)
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.GetVariationToPathInfoAsync(strActivityId, m_clinicalPathInfo.Ljdm);
            radBusyIndicatorVariation.IsBusy = true;
            client.GetVariationToPathInfoCompleted += (s, t) =>
            {
                radBusyIndicatorVariation.IsBusy = false;
                if (t.Error == null)
                {
                    ObservableCollection<CP_VariationToPath> listVarToPath = t.Result;
                    foreach (CP_VariationToPathInfo info in listInfo)
                    {
                        info.ToPathId = 0;
                        info.Ljdm = String.Empty;
                        info.ActivityId = String.Empty;
                        info.IsSelected = false;
                        info.IsNew = true;
                        info.IsModify = false;
                    }
                    foreach (CP_VariationToPath pathinfo in listVarToPath)
                    {
                        foreach (CP_VariationToPathInfo execinfo in listInfo)
                        {
                            if (execinfo.Bydm == pathinfo.Bydm)
                            {
                                execinfo.ToPathId = pathinfo.Id;
                                execinfo.Ljdm = pathinfo.Ljdm;
                                execinfo.ActivityId = pathinfo.ActivityId;
                                execinfo.IsSelected = true;
                                execinfo.IsNew = false;
                                break;
                            }
                        }
                    }
                }
                else
                {
                    PublicMethod.RadWaringBox(t.Error);
                }
            };
            client.CloseAsync();
        }
        /// <summary>
        /// 异常配置保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnVariationSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (nodestr.ToString().Split('&')[0].ToString() == String.Empty)
                {
                    PublicMethod.RadAlterBox("先选择对应的节点", HeaderText);
                    return;
                }
                ObservableCollection<CP_VariationToPathInfo> listInfo = new ObservableCollection<CP_VariationToPathInfo>();
                foreach (Control ctl in gridVariationDetail.Children)
                {
                    if (ctl.GetType() == typeof(UCVariationCheckBox))
                    {
                        CP_VariationToPathInfo info = ((UCVariationCheckBox)ctl).PathVariation;
                        info.ActivityId = nodestr.ToString().Split('&')[0].ToString();
                        info.Ljdm = m_clinicalPathInfo.Ljdm;
                        listInfo.Add(info);
                    }
                }
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.SaveVariationToPathAsync(listInfo, Global.LogInEmployee.Zgdm);
                radBusyIndicatorVariation.IsBusy = true;
                client.SaveVariationToPathCompleted += (s, t) =>
                {
                    radBusyIndicatorVariation.IsBusy = false;
                    if (t.Error == null)
                    {
                        
                        GetVariationToPathInfo(m_PathVariation, nodestr.ToString().Split('&')[0].ToString());
                        PublicMethod.RadAlterBox("保存成功", HeaderText);
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(t.Error);
                    }
                };
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region 路径条件配置
        #region 属性
        CP_ClinicalPathList _Path = null;
        OperationState state = OperationState.NEW;
        public OperationState CurrentOperationState
        {
            get
            {
                return state;
            }
            set
            {
                if (value == OperationState.NEW)
                {
                    txtOperationState.Text = "当前状态：新增";
                    ClearControlValue();
                    cmbConditionType.IsEnabled = true;
                }
                if (value == OperationState.EDIT)
                {
                    txtOperationState.Text = "当前状态：编辑";
                    cmbConditionType.IsEnabled = false;
                }
                state = value;
            }
        }
        CP_PathEnterJudgeCondition _CP_PathEnterJudgeCondition = new CP_PathEnterJudgeCondition();
        String ID;
        #endregion
        #region 事件
        private void InitCondition()
        {
            CurrentOperationState = OperationState.NEW;
            YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
            #region 绑定类型下拉框
            KeyValues keyValues = new KeyValues();
            keyValues.Add(new KeyValue("2", "ICD-10"));
            keyValues.Add(new KeyValue("1", "检查项"));
            keyValues.Add(new KeyValue("3", "描述项"));
            cmbConditionType.ItemsSource = keyValues;
            cmbConditionType.DisplayMemberPath = "Value";
            cmbConditionType.SelectedValuePath = "Key";
            cmbConditionType.SelectedValue = "2";
            #endregion
            #region 绑定ICD自动完成框
            referenceClient.GetCP_PathDiagnosisListAllCompleted += (send, ea) =>
            {
                autoCompleteICD10.ItemsSource = ea.Result;
                autoCompleteICD10.ItemFilter = DeptFilter;
            };
            referenceClient.GetCP_PathDiagnosisListAllAsync();
            #endregion
            #region 绑定非ICD自动完成框
            referenceClient.GetCP_ExamDictionaryDetailAllCompleted += (send2, ea2) =>
            {
                autoCompleteNonICD10.ItemsSource = ea2.Result;
                autoCompleteNonICD10.ItemFilter = DeptFilterNonICD;
            };
            referenceClient.GetCP_ExamDictionaryDetailAllAsync();
            #endregion
            #region 绑定列表
            BindGridView();
            #endregion
            referenceClient.CloseAsync();
        }
        private void autoCompleteNonICD10_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count > 0)
                {
                    CP_ExamDictionaryDetail detail = (CP_ExamDictionaryDetail)e.AddedItems[0];
                    foreach (SuitCrowdMapScope scope in detail.SuitCrowsMapScopes)
                    {
                        if (scope.ExamSyrq.Jlxh == "1")
                        {
                            numStart_1.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_1.Value = Convert.ToDouble(scope.Jsfw);
                        }
                        if (scope.ExamSyrq.Jlxh == "2")
                        {
                            numStart_2.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_2.Value = Convert.ToDouble(scope.Jsfw);
                        }
                        if (scope.ExamSyrq.Jlxh == "3")
                        {
                            numStart_3.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_3.Value = Convert.ToDouble(scope.Jsfw);
                        }
                        if (scope.ExamSyrq.Jlxh == "4")
                        {
                            numStart_4.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_4.Value = Convert.ToDouble(scope.Jsfw);
                        }
                        if (scope.ExamSyrq.Jlxh == "5")
                        {
                            numStart_5.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_5.Value = Convert.ToDouble(scope.Jsfw);
                        }
                        if (scope.ExamSyrq.Jlxh == "6")
                        {
                            numStart_6.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_6.Value = Convert.ToDouble(scope.Jsfw);
                        }
                    }
                    //  Debug.WriteLine("Zdbs:{0},Name:{1}", ((CP_ExamDictionaryDetail)e.AddedItems[0]).Jlxh, ((CP_ExamDictionaryDetail)e.AddedItems[0]).Jcmc);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentOperationState = OperationState.NEW;
                _CP_PathEnterJudgeCondition = new CP_PathEnterJudgeCondition();
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
                _CP_PathEnterJudgeCondition.Ljdm = _Path.Ljdm;
                _CP_PathEnterJudgeCondition.Lb = 1;
                _CP_PathEnterJudgeCondition.Syrq = "";
                _CP_PathEnterJudgeCondition.Ksfw = "";
                _CP_PathEnterJudgeCondition.Jsfw = "";
                if (((KeyValue)cmbConditionType.SelectedItem).Key == "1")//检查
                {
                    if (autoCompleteNonICD10.SelectedItem == null)
                    {
                        PublicMethod.RadAlterBox("检查项必须填写！", "提示");
                        return;
                    }
                    if (numEnd_6.Value < numStart_6.Value || numEnd_5.Value < numStart_5.Value || numEnd_4.Value < numStart_4.Value || numEnd_3.Value < numStart_3.Value || numEnd_2.Value < numStart_2.Value || numEnd_1.Value < numStart_1.Value)
                    {
                        PublicMethod.RadAlterBox("条件范围前面的值必须小于后面的值！", "提示");
                        return;
                    }
                    #region 适用人群和范围
                    if (numEnd_6.Value > numStart_6.Value)
                    {
                        _CP_PathEnterJudgeCondition.Syrq = ",6";
                        _CP_PathEnterJudgeCondition.Ksfw = "," + ConvertMy.ToString(numStart_6.Value);
                        _CP_PathEnterJudgeCondition.Jsfw = "," + ConvertMy.ToString(numEnd_6.Value);
                    }
                    if (numEnd_5.Value > numStart_5.Value)
                    {
                        _CP_PathEnterJudgeCondition.Syrq += ",5";
                        _CP_PathEnterJudgeCondition.Ksfw += "," + ConvertMy.ToString(numStart_5.Value);
                        _CP_PathEnterJudgeCondition.Jsfw += "," + ConvertMy.ToString(numEnd_5.Value);
                    } if (numEnd_4.Value > numStart_4.Value)
                    {
                        _CP_PathEnterJudgeCondition.Syrq += ",4";
                        _CP_PathEnterJudgeCondition.Ksfw += "," + ConvertMy.ToString(numStart_4.Value);
                        _CP_PathEnterJudgeCondition.Jsfw += "," + ConvertMy.ToString(numEnd_4.Value);
                    } if (numEnd_3.Value > numStart_3.Value)
                    {
                        _CP_PathEnterJudgeCondition.Syrq += ",3";
                        _CP_PathEnterJudgeCondition.Ksfw += "," + ConvertMy.ToString(numStart_3.Value);
                        _CP_PathEnterJudgeCondition.Jsfw += "," + ConvertMy.ToString(numEnd_3.Value);
                    } if (numEnd_2.Value > numStart_2.Value)
                    {
                        _CP_PathEnterJudgeCondition.Syrq += ",2";
                        _CP_PathEnterJudgeCondition.Ksfw += "," + ConvertMy.ToString(numStart_2.Value);
                        _CP_PathEnterJudgeCondition.Jsfw += "," + ConvertMy.ToString(numEnd_2.Value);
                    } if (numEnd_1.Value > numStart_1.Value)
                    {
                        _CP_PathEnterJudgeCondition.Syrq += ",1";
                        _CP_PathEnterJudgeCondition.Ksfw += "," + ConvertMy.ToString(ConvertMy.ToDecimal(numStart_1.Value));
                        _CP_PathEnterJudgeCondition.Jsfw += "," + ConvertMy.ToString(ConvertMy.ToDecimal(numEnd_1.Value));
                    }
                    _CP_PathEnterJudgeCondition.Syrq = _CP_PathEnterJudgeCondition.Syrq.IndexOf(',') > -1 ? _CP_PathEnterJudgeCondition.Syrq.Substring(1) : "";
                    _CP_PathEnterJudgeCondition.Ksfw = _CP_PathEnterJudgeCondition.Ksfw.IndexOf(',') > -1 ? _CP_PathEnterJudgeCondition.Ksfw.Substring(1) : "";
                    _CP_PathEnterJudgeCondition.Jsfw = _CP_PathEnterJudgeCondition.Jsfw.IndexOf(',') > -1 ? _CP_PathEnterJudgeCondition.Jsfw.Substring(1) : "";
                    #endregion
                    _CP_PathEnterJudgeCondition.Xmlb = 1;
                    _CP_PathEnterJudgeCondition.Jcxm = ((CP_ExamDictionaryDetail)autoCompleteNonICD10.SelectedItem).Jcbm;
                }
                if (((KeyValue)cmbConditionType.SelectedItem).Key == "2")//ICD
                {
                    if (autoCompleteICD10.SelectedItem == null)
                    {
                        PublicMethod.RadAlterBox("ICD-10必须填写！", "提示");
                        return;
                    }
                    _CP_PathEnterJudgeCondition.Xmlb = 2;
                    _CP_PathEnterJudgeCondition.Jcxm = ((CP_Diagnosis_E)autoCompleteICD10.SelectedItem).Zdbs;
                }
                if (((KeyValue)cmbConditionType.SelectedItem).Key == "3")//ICD
                {
                    if (txtDescrib.Text.Trim() == "")
                    {
                        PublicMethod.RadAlterBox("描述内容必须填写！", "提示");
                        return;
                    }
                    _CP_PathEnterJudgeCondition.Xmlb = 3;
                    _CP_PathEnterJudgeCondition.Jcxm = txtDescrib.Text; //((CP_Diagnosis_E)autoCompleteICD10.SelectedItem).Zdbs;
                }
                if (CurrentOperationState == OperationState.NEW)
                {
                    foreach (var item in (ObservableCollection<CP_PathEnterJudgeCondition>)GrdConditonList.ItemsSource)
                    {
                        if (item.Jcxm == _CP_PathEnterJudgeCondition.Jcxm)
                        {
                            PublicMethod.RadAlterBox("已经存在该条件", "提示");
                            return;
                        }
                    }
                    YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                    referenceClient.GetInsertCP_PathEnterJudgeConditionCompleted += (send, ea) => { BindGridView(); };
                    referenceClient.GetInsertCP_PathEnterJudgeConditionAsync(_CP_PathEnterJudgeCondition);
                    referenceClient.CloseAsync();
                }
                if (CurrentOperationState == OperationState.EDIT)
                {
                    YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                    referenceClient.GetUpdateCP_PathEnterJudgeConditionCompleted += (send, ea) => { BindGridView(); };
                    referenceClient.GetUpdateCP_PathEnterJudgeConditionAsync(_CP_PathEnterJudgeCondition);
                    referenceClient.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnDeleteCondition_Click(object sender, RoutedEventArgs e)
        {
            ID = ((RadButton)sender).Tag.ToString();
            RadWindow.Confirm("确定删除吗？", ConfirmClose);
        }
        void ConfirmClose(object sender, WindowClosedEventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(e.DialogResult))
                {
                    YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                    referenceClient.GetDeleteCP_PathEnterJudgeConditionCompleted += (send, ea) => { BindGridView(); };
                    referenceClient.GetDeleteCP_PathEnterJudgeConditionAsync(ID);
                    referenceClient.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void GrdConditonList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            try
            {
                if (GrdConditonList.SelectedItem == null)
                {
                    return;
                }
                _CP_PathEnterJudgeCondition = (CP_PathEnterJudgeCondition)GrdConditonList.SelectedItem;
                cmbConditionType.SelectedValue = _CP_PathEnterJudgeCondition.Xmlb.ToString();
                CurrentOperationState = OperationState.EDIT;
                if (_CP_PathEnterJudgeCondition.Xmlb == 3)
                {
                    txtDescrib.Text = _CP_PathEnterJudgeCondition.JcxmName;

                }
                if (_CP_PathEnterJudgeCondition.Xmlb == 2)
                {
                    autoCompleteICD10.SelectedItem = ((ObservableCollection<CP_Diagnosis_E>)autoCompleteICD10.ItemsSource).First(cp => cp.Zdbs.Equals(_CP_PathEnterJudgeCondition.Jcxm));
                }
                if (_CP_PathEnterJudgeCondition.Xmlb == 1)
                {
                    ClearControlValue();
                    autoCompleteNonICD10.SelectedItem = ((ObservableCollection<CP_ExamDictionaryDetail>)autoCompleteNonICD10.ItemsSource).First(cp => cp.Jcbm.Equals(_CP_PathEnterJudgeCondition.Jcxm));
                    foreach (SuitCrowdMapScope scope in _CP_PathEnterJudgeCondition.SuitCrowsMapScopes)
                    {
                        if (scope.ExamSyrq.Jlxh == "1")
                        {
                            numStart_1.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_1.Value = Convert.ToDouble(scope.Jsfw);
                        }
                        if (scope.ExamSyrq.Jlxh == "2")
                        {
                            numStart_2.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_2.Value = Convert.ToDouble(scope.Jsfw);
                        }
                        if (scope.ExamSyrq.Jlxh == "3")
                        {
                            numStart_3.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_3.Value = Convert.ToDouble(scope.Jsfw);
                        }
                        if (scope.ExamSyrq.Jlxh == "4")
                        {
                            numStart_4.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_4.Value = Convert.ToDouble(scope.Jsfw);
                        }
                        if (scope.ExamSyrq.Jlxh == "5")
                        {
                            numStart_5.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_5.Value = Convert.ToDouble(scope.Jsfw);
                        }
                        if (scope.ExamSyrq.Jlxh == "6")
                        {
                            numStart_6.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_6.Value = Convert.ToDouble(scope.Jsfw);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void cmbConditionType_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count > 0)
                {
                    Visibility ICD = ((KeyValue)e.AddedItems[0]).Key == "2" ? Visibility.Visible : Visibility.Collapsed;
                    Visibility NonICD = ((KeyValue)e.AddedItems[0]).Key == "1" ? Visibility.Visible : Visibility.Collapsed;
                    Visibility Describ = ((KeyValue)e.AddedItems[0]).Key == "3" ? Visibility.Visible : Visibility.Collapsed;
                    stkICD10.Visibility = ICD;
                    stkNonICD10.Visibility = NonICD;
                    wrpNonICD.Visibility = NonICD;
                    stkDescrib.Visibility = Describ;
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region 函数
        private bool DeptFilter(string strFilter, object item)
        {
            CP_Diagnosis_E deptList = (CP_Diagnosis_E)item;
            return ((deptList.Py.StartsWith(strFilter)) || (deptList.Py.Contains(strFilter)) || deptList.Zdbs.StartsWith(strFilter) || deptList.Zdbs.Contains(strFilter));
        }
        private bool DeptFilterNonICD(string strFilter, object item)
        {
            CP_ExamDictionaryDetail deptList = (CP_ExamDictionaryDetail)item;
            return ((deptList.Py.StartsWith(strFilter)) || (deptList.Py.Contains(strFilter)) || deptList.Jlxh.StartsWith(strFilter) || deptList.Jlxh.Contains(strFilter));
        }
        private void BindGridView()
        {
            if (_Path == null || _Path.Ljdm == null || _Path.Ljdm.ToString().Trim() == "") return;
            YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
            referenceClient.GetPathCP_PathEnterJudgeConditionAllCompleted += (send, ea) =>
            {
                GrdConditonList.ItemsSource = ea.Result;
            };
            referenceClient.GetPathCP_PathEnterJudgeConditionAllAsync(_Path.Ljdm);
            referenceClient.CloseAsync();
        }
        private void ClearControlValue()
        {
            autoCompleteNonICD10.SelectedItem = null;
            numStart_1.Value = 0;
            numStart_2.Value = 0;
            numStart_3.Value = 0;
            numStart_4.Value = 0;
            numStart_5.Value = 0;
            numStart_6.Value = 0;
            numEnd_1.Value = 0;
            numEnd_2.Value = 0;
            numEnd_3.Value = 0;
            numEnd_4.Value = 0;
            numEnd_5.Value = 0;
            numEnd_6.Value = 0;
        }
        #endregion
        #endregion
        #region 节点条件配置
        #region 属性
        String _LjdmNode = null;
        String _NodeGUIDNode = null;
        OperationState stateNode = OperationState.NEW;
        //public OperationState CurrentOperationStateNode
        //{
        //    get
        //    {
        //        return stateNode;
        //    }
        //    set
        //    {
        //        if (value == OperationState.NEW)
        //        {
        //            txtOperationStateNode.Text = "当前状态：新增";
        //            ClearControlValueNode();
        //            //cmbConditionType.IsEnabled = true;
        //        }
        //        if (value == OperationState.EDIT)
        //        {
        //            txtOperationStateNode.Text = "当前状态：编辑";
        //            // cmbConditionType.IsEnabled = false;
        //        }
        //        stateNode = value;
        //    }
        //}
        CP_PathEnterJudgeCondition _CP_PathEnterJudgeConditionNode = new CP_PathEnterJudgeCondition();
        String IDNode;
        #endregion
        private void btnConditionMaintain_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RWEnterConditionMaintainNode RWEnterConditionMaintainNodeTemp = new RWEnterConditionMaintainNode(m_clinicalPathInfo.Ljdm, nodestr.ToString().Split('&')[0].ToString());
                RWEnterConditionMaintainNodeTemp.Closed += (send, ea) =>
                {
                    BindGridViewNode();
                };
                RWEnterConditionMaintainNodeTemp.ShowDialog();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #region 函数
        private void BindGridViewNode()
        {
            if (_NodeGUIDNode == null || _NodeGUIDNode.ToString().Trim() == "") return;
            YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
            referenceClient.GetNodeCP_PathEnterJudgeConditionAllCompleted += (send, ea) =>
            {
                GrdConditonListNode.ItemsSource = ea.Result;
            };
            referenceClient.GetNodeCP_PathEnterJudgeConditionAllAsync(_NodeGUIDNode);
            referenceClient.CloseAsync();
        }
        #endregion
        #endregion
        #region 医嘱裁剪 ZM
        /// <summary>
        /// 面板筛选按钮变色方法
        /// </summary>
        /// <param name="container"></param>
        private void coloringExecute(Panel container)
        {
            UIElementCollection list = container.Children;
            for (int i = 0; i < list.Count; i++)
            {
                object c = list[i];
                if (c is RadButton)
                { 
                    RadButton button = c as RadButton;
                    if (button.Tag != null && button.Tag.ToString().Trim() != "")
                    {
                        String color = "";
                        if (button.Tag.ToString() == "0")
                        {
                            color = "FFCC01";
                        }
                        else
                        {
                            color = "0000FF";
                        }

                        SolidColorBrush brush = ConvertColor.GetColorBrushFromHx16(color);
                        button.Foreground = brush;
                        button.Background = brush;
                    }
                }
            }
        }
        /// <summary>
        /// 加载行时变色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewAdviceGroupPathCut_RowLoaded(object sender, RowLoadedEventArgs e)
        {
            if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
            CP_AdviceGroupPathCut cut = (CP_AdviceGroupPathCut)e.DataElement;
            coloringExecute(e.Row, cut.Ctmxxh.ToString());
        }
        /// <summary>
        /// GridView列变色方法
        /// </summary>
        private void coloringExecute(Control c, string key)
        {
            String color = "";
            if (key == "0")
            {
                color = "FFCC01";
            }
            else
            {
                color = "0000FF";
            }

            SolidColorBrush brush = ConvertColor.GetColorBrushFromHx16(color);
            if (c is RadButton)
            {
                RadButton button = c as RadButton;
                if (button.Tag != null && button.Tag.ToString().Trim() != "")
                {
                    button.Foreground = brush;
                    button.Background = brush;
                }
            }
            if (c is GridViewRowItem)
            {
                GridViewRowItem rowItem = c as GridViewRowItem;
                rowItem.Foreground = brush;
            }
        }
        /// <summary>
        /// 提取按钮
        /// </summary>
        private void btnQuan_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RadButton b = (RadButton)sender;
                if (b == null)
                {
                    return;
                }
                else if (b.Tag == null)
                {
                    AdviceGroupPathCut("");
                }
                else 
                {
                    AdviceGroupPathCut((((RadButton)sender).Tag).ToString());
                }    
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 加载医嘱裁剪6.7
        /// </summary>
        private void AdviceGroupPathCut(String tag)
        {
            GridViewAdviceGroupPathCut.ItemsSource = null;     //先清空

            String jddm = nodestr.ToString().Split('&')[0].ToString();              //获得当前节点

            if (jddm != "")              //点击的节点
            {
                List<CP_AdviceGroupPathCut> newAdviceGroupPathCut = new List<CP_AdviceGroupPathCut>();
                    
                if(tag == "0")            //新增
                {
                    newAdviceGroupPathCut = adviceGroupPathCut.Where(s => s.ActivityId == jddm).Where(s => s.Ctmxxh == 0).ToList();       //提取当前节点医嘱
                }
                else if (tag == "1")           //成套
                {
                    newAdviceGroupPathCut = adviceGroupPathCut.Where(s => s.ActivityId == jddm).Where(s => s.Ctmxxh != 0).ToList();       //提取当前节点医嘱
                }
                else                      //全选
                {
                    newAdviceGroupPathCut = adviceGroupPathCut.Where(s => s.ActivityId == jddm).ToList();       //提取当前节点医嘱
                }

                GridViewAdviceGroupPathCut.ItemsSource = newAdviceGroupPathCut;
            }
        }
        /// <summary>
        /// 裁剪
        /// </summary>
        private void AdviceGroupCut_Click(object sender, RoutedEventArgs e)
        {
            try 
            {
                InsertAdviceGroupPathCut();
            }
            catch(Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
            
        }
        /// <summary>
        /// 插入医嘱6.7
        /// </summary>
        private void InsertAdviceGroupPathCut()
        {
            if (GridViewAdviceGroupPathCut.SelectedItems.Count > 0)                     //选项数目不为零
            {
                String jddm = nodestr.ToString().Split('&')[0].ToString();              //获得当前节点
                String jdmc = nodestr.ToString().Split('&')[1].ToString();              //获得当前节点名称
                if (jddm != "")              //选择节点
                {
                    YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                    client.DeleteAdviceGroupDetailCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            PublicMethod.RadAlterBox("医嘱裁剪成功！", "提示");
                        }
                    };

                    ObservableCollection<CP_AdviceGroupPathCut> newPathCut = new ObservableCollection<CP_AdviceGroupPathCut>();
                    foreach (var item in GridViewAdviceGroupPathCut.SelectedItems)
                    {
                        newPathCut.Add((CP_AdviceGroupPathCut)item);                            //序列化集合以便被服务调用
                    }

                    client.DeleteAdviceGroupDetailAsync(newPathCut, jddm, "DelDetail", jdmc);
                    client.CloseAsync();
                }
            }
            else
            {
                PublicMethod.RadAlterBox("请选择医嘱！", "提示");
            }
        }
        #endregion

 
    }
    //#region 设定临时，长期医嘱类别
    //public class OrderTypeName
    //{
    //    public string OrderName
    //    {
    //        get;
    //        set;
    //    }
    //    public short OrderValue
    //    {
    //        get;
    //        set;
    //    }
    //    public OrderTypeName(string orderName, short orderValue)
    //    {
    //        OrderName = orderName;
    //        OrderValue = orderValue;
    //    }
    //}
    //#endregion
}
/*医嘱新增修改逻辑*/
/*   药品，手术是之前的逻辑;RISLIS单独一个控件，基本属性完全在控件里固定*/
/*   e.g Yzlb = (int)OrderPanelBarCategory.RisLis,初始化ManualType = ManualType.New，在GRIDVIEW右键点击修改时改成*/
/*   ManualType = ManualType.Modify,同时调用InitModifyOrder，将要修改的基本信信息传入*/
/*   用户控件有两个事件AfterDrugLoadedEvent，AfterDrugCinfirmeddEvent，在页面构造函数时注册，其中AfterDrugLoadedEvent是初始化项目基本*/
/*   信息，频次信息，及其它输入控件数据源，AfterDrugCinfirmeddEvent 是在点击确定后回发的事件，可以获得控件里项目的基本信息，同时根据ManualType*/
/*   的类型，进行UPDATE OR INSERT 的动作。需要注意的是，为了用户控件在以后的医嘱执行时也可以使用，传入传出的数据类型是CP_DoctorOder*/
/*   而在此界面里医嘱项目的基本类型为CP_AdviceGroupDetail,所以需要将对应的实体转换一下才能实现*/
/*   综上，用到控件时需要做如下步骤*/
/* 1）在界面LOAD时，注册医嘱输入控件 事件，并控制医嘱 项目初始化型类(对应enum OrderItemCategory） 项目类别初始化型类（对应enum OrderPanelBarCategory) */
/*    如护理和宣教：   careOrderControl.PanelCategory = OrderPanelBarCategory.Care;   */
/*                     careOrderControl.OrderCategory = OrderItemCategory.Care;    */
/*                     careOrderControl.AfterDrugLoadedEvent += new UserControlOtherOrder.DrugLoaded(careOrderControl_AfterDrugLoadedEvent);*/
/*                     careOrderControl.AfterDrugCinfirmeddEvent += new UserControlOtherOrder.DrugConfirmed(careOrderControl_AfterDrugCinfirmeddEvent);*/
/* 2）在点击确定时做CP_DoctorOder -> CP_AdviceGroupDetail*/
/* 3）在点击修改时做CP_AdviceGroupDetail -> CP_DoctorOder*/
/* 待修改的地方*/
/* 1）药品，手术是之前的逻辑，特别是手术，要将数据保存的TABLE给改成CP_AdviceGroupDetail，现在为CP_AdviceGroupDetail*/
/* 2）一些公用方法的提取*/
/* 3）一些逻辑的修改，比如，双击修改的方法在换成用户控件后的实现，最好可以做成对应的属性，一旦属性改变就触发对应的事件，点击右键修改的方法也是，还是对应的清空方法*/
