using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using System.Windows.Navigation;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.DataService;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using YidanEHRApplication.Models;

using YidanSoft.Tool;
namespace YidanEHRApplication.Views
{
    /// <summary>
    /// Interaction logic for PathReview.xaml
    /// </summary>
    public partial class PathReview
    {
        #region 事件
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            InitPage();
            RegisterKeyEvent();
        }
        private void m_CompletedTimer_Tick(object sender, EventArgs e)
        {
            if (m_CompletedCount == m_Completed)
            {
                radBusyIndicator.IsBusy = false;
                m_CompletedTimer.Stop();
            }
        }
        /// <summary>
        /// 路径列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radGridViewPathList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            try
            {
                #region
                m_ClinicalPathShowSel = null;
                m_ClinicalDiagnosisListAdd.Clear();
                m_ClinicalDiagnosisListDele.Clear();
                //m_PathconditionListSel = null;
                //this.textBoxConditon.Text = string.Empty;
                #endregion
                if (radGridViewPathList.SelectedItem == null)
                {
                    Reset();
                    return;
                }
                m_ChangedCompletedCount = 3;
                //radBusyIndicator.IsBusy = true;
                //todo 将CP_ClinicalPathList，CP_ClinicalPath，CP_ClinicalPathShowInfo 三个类合并
                CP_ClinicalPathList showInfo = (CP_ClinicalPathList)radGridViewPathList.SelectedItem;
                m_ClinicalPathShowSel = showInfo;
                if (showInfo.YxjlId == (int)PathShStatus.Review)
                {

                    //SetControlEnable(false);
                    this.btnReView.IsEnabled = false;
                    if (showInfo.LjSysl == 0)
                    {
                        this.btnAntiReview.IsEnabled = true;
                        this.btnStop.IsEnabled = true;
                    }
                    else
                    {
                        this.btnAntiReview.IsEnabled = false;
                        this.btnStop.IsEnabled = true;
                    }
                }
                else
                {

                    //SetControlEnable(true);
                    this.btnReView.IsEnabled = true;
                    this.btnAntiReview.IsEnabled = false;
                    this.btnStop.IsEnabled = false;
                }

                //YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                //referenceClient.GetPathConditionListByLjdmCompleted +=
                //    (obj, ea) =>
                //    {
                //        if (ea.Error == null)
                //        {
                //            m_ChangedCompletedCount++;
                //            m_PathconditionList = ea.Result.ToList();
                //            m_PathconditionListTemp = ea.Result.ToList();
                //            //this.radGridViewAddCondtion.ItemsSource = m_PathconditionList;
                //        }
                //        else
                //        {
                //            radBusyIndicator.IsBusy = false;
                //            PublicMethod.RadWaringBox(ea.Error);
                //        }
                //    };
                //referenceClient.GetPathConditionListByLjdmAsync(m_ClinicalPathShowSel.Ljdm);
                //referenceClient.CloseAsync();
                m_ChangedCompletedTimer = new System.Windows.Threading.DispatcherTimer();
                m_ChangedCompletedTimer.Interval = new TimeSpan(0, 0, 1);
                m_ChangedCompletedTimer.Tick += new EventHandler(m_ChangedCompletedTimer_Tick);
                m_ChangedCompletedTimer.Start();
            }
            catch (Exception ex)
            {
                radBusyIndicator.IsBusy = false;
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void pathNode_Closed(object sender, WindowClosedEventArgs e)
        {
            //此处不需要再次赋值，因为对象为同一个
            //string strLjdm = ((PathNodeSetting)sender).ClinicalPathInfo.Ljdm;
            //string strXML = ((PathNodeSetting)sender).ClinicalPathInfo.WorkFlowXML;
            //List<CP_ClinicalPathShowInfo> listPathInfo = (List<CP_ClinicalPathShowInfo>)this.radGridViewPathList.ItemsSource;
            //foreach (CP_ClinicalPathShowInfo info in listPathInfo)
            //{
            //    if (info.Ljdm == strLjdm)
            //    {
            //        info.WorkFlowXML = strXML;
            //        break;
            //    }
            //}
        }
        private void radbuttonQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Reset();
                radBusyIndicator.IsBusy = true;
                m_CompletedCount = 2;
                GetPathList();
                m_CompletedTimer.Start();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnReView_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!GetIsSelectItem())
                {
                    PublicMethod.RadAlterBox("请选择一个路径再点击审核", "提示");
                    return;
                }
                var listInfo = this.radGridViewPathList.SelectedItem as CP_ClinicalPathList;
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.UpdatePathStausCompleted +=
                (obj, ea) =>
                {
                    try
                    {
                        if (ea.Error == null)
                        {
                            radbuttonQuery_Click(null, null);
                            PublicMethod.RadAlterBox("审核成功", this.m_Title);
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(ea.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                    }
                };
                client.UpdatePathStausAsync(listInfo.Ljdm, (int)PathShStatus.Review, DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd"), Global.LogInEmployee.Zgdm);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnAntiReview_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!GetIsSelectItem())
                {
                    PublicMethod.RadAlterBox("请选择一个路径再点击反审核", "提示");
                    return;
                }
                var listInfo = this.radGridViewPathList.SelectedItem as CP_ClinicalPathList;
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.UpdatePathStausCompleted +=
                    (obj, ea) =>
                    {
                        try
                        {
                            if (ea.Error == null)
                            {
                                radbuttonQuery_Click(null, null);
                                PublicMethod.RadAlterBox("反审核成功", this.m_Title);
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                        }
                    };
                client.UpdatePathStausAsync(listInfo.Ljdm, (int)PathShStatus.Valid, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!GetIsSelectItem())
                {
                    PublicMethod.RadAlterBox("请选择一个路径再点击停止", "提示");
                    return;
                }
                var listInfo = this.radGridViewPathList.SelectedItem as CP_ClinicalPathList;
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.UpdatePathStausCompleted +=
                    (obj, ea) =>
                    {
                        if (ea.Error == null)
                        {
                            InitPage();
                            PublicMethod.RadAlterBox("停用成功", this.m_Title);
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(ea.Error);
                        }
                    };
                client.UpdatePathStausAsync(listInfo.Ljdm, (int)PathShStatus.Dc, string.Empty, string.Empty);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void m_ChangedCompletedTimer_Tick(object sender, EventArgs e)
        {
            if (m_ChangedCompletedCount == m_Completed)
            {
                radBusyIndicator.IsBusy = false;
                m_ChangedCompletedTimer.Stop();
            }
        }

        #region  输入框加回车事件
        private void RegisterKeyEvent()
        {


            autoCompleteBoxQueryDept.KeyUp += new KeyEventHandler(tbQuery2_KeyUp);
            autoCompleteBoxPath.KeyUp += new KeyEventHandler(tbQuery2_KeyUp);
            radCmbYxjl.KeyUp += new KeyEventHandler(tbQuery2_KeyUp);
        }




        private void tbQuery2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                radbuttonQuery_Click(null, null);
        }

        #endregion

        #endregion

        #region 属性
        /// <summary>
        /// 路径病种信息(All)
        /// </summary>
        private List<CP_ClinicalDiagnosisList> m_ClinicalDiagnosisList = null;
        /// <summary>
        /// 路径基本信息（ALL)
        /// </summary>
        private List<CP_ClinicalPathList> m_ClinicalPathList = null;
        /// <summary>
        /// 需要新增 病种
        /// </summary>
        private ObservableCollection<CP_ClinicalDiagnosisList> m_ClinicalDiagnosisListAdd = new ObservableCollection<CP_ClinicalDiagnosisList>();
        /// <summary>
        /// 需要删除  病种
        /// </summary>
        private ObservableCollection<CP_ClinicalDiagnosisList> m_ClinicalDiagnosisListDele = new ObservableCollection<CP_ClinicalDiagnosisList>();
        // private CP_PathConditionList m_PathconditionListSel = null;
        // private ObservableCollection<CP_PathConditionList> m_PathconditionListAdd = new ObservableCollection<CP_PathConditionList>();
        //private ObservableCollection<CP_PathConditionList> m_PathconditionListDel = new ObservableCollection<CP_PathConditionList>();
        //private List<CP_PathConditionList> m_PathconditionList = null;
        //private List<CP_PathConditionList> m_PathconditionListTemp = null;
        /// <summary>
        /// 选中的记录
        /// </summary>
        private CP_ClinicalPathList m_ClinicalPathShowSel = null;
        private string m_ConnectionString = string.Empty;
        /// <summary>
        /// 当前赋值的XML
        /// </summary>
        private string m_CopyWorkXml = string.Empty;
        /// <summary>
        /// 初始化的计数器
        /// </summary>
        private int m_CompletedCount = 0;
        /// <summary>
        /// 路径GRID selectchanged的计数器
        /// </summary>
        private int m_ChangedCompletedCount = 0;
        /// <summary>
        /// 初始化全部完成的计数器
        /// </summary>
        private const int m_Completed = 4;
        /// <summary>
        /// 出始化页面时，数据是否完成的标志
        /// </summary>
        System.Windows.Threading.DispatcherTimer m_CompletedTimer;
        /// <summary>
        /// 路径GRID selectchanged，数据是否完成的标志
        /// </summary>
        System.Windows.Threading.DispatcherTimer m_ChangedCompletedTimer;
        private string m_Title = "路径管理";
        #endregion

        #region 函数
        public PathReview()
        {
            ICommand deleteCommand = RadGridViewCommands.Delete;
            ICommand beginInsertCommand = RadGridViewCommands.BeginInsert;
            ICommand cancelRowEditCommand = RadGridViewCommands.CancelRowEdit;
            ICommand commitEditCommand = RadGridViewCommands.CommitEdit;
            InitializeComponent();
        }
        private void InitPage()
        {
            try
            {
                this.dateTimeFrom.DateTimeWatermarkContent = "选择日期...";
                this.dateTimeTo.DateTimeWatermarkContent = "选择日期...";
                m_CompletedCount = 0;
                Clear();
                radBusyIndicator.IsBusy = true;
                InitComboBoxPath();

                IntiComboBoxDept();
                IntiComboBoxCondition();
                InitEntity();
                m_CompletedTimer = new System.Windows.Threading.DispatcherTimer();
                m_CompletedTimer.Interval = new TimeSpan(0, 0, 2);
                m_CompletedTimer.Tick += new EventHandler(m_CompletedTimer_Tick);
                m_CompletedTimer.Start();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void InitComboBoxPath()
        {
            try
            {
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                referenceClient.GetClinicalPathListInfoCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            m_CompletedCount++;
                            autoCompleteBoxPath.ItemsSource = e.Result.ToList();
                            autoCompleteBoxPath.ItemFilter = PathFilter;
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
                referenceClient.GetClinicalPathListInfoAsync();
                referenceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        public bool PathFilter(string strFilter, object item)
        {
            CP_ClinicalPathList info = (CP_ClinicalPathList)item;
            return ((info.QueryName.StartsWith(strFilter.ToUpper())) || (info.QueryName.Contains(strFilter.ToUpper())));
        }
        /// <summary>
        /// 实例化一个新的病种
        /// </summary>
        /// <param name="selectlists"></param>
        /// <param name="strLjdm"></param>
        /// <returns></returns>
        private CP_ClinicalDiagnosisList InitNewDiagnosis(CP_DiagnosisList selectlists, String strLjdm)
        {
            CP_ClinicalDiagnosisList cpList = new CP_ClinicalDiagnosisList();
            cpList.Bzdm = selectlists.Zddm;
            cpList.Bzmc = selectlists.Name;
            cpList.Ljdm = strLjdm;
            return cpList;
        }
        /// <summary>
        /// INIT科室
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
                            m_CompletedCount++;
                            autoCompleteBoxQueryDept.ItemsSource = e.Result;
                            autoCompleteBoxQueryDept.ItemFilter = DeptFilter;
                            //autoCompleteBoxDept.ItemsSource = e.Result;
                            //autoCompleteBoxDept.ItemFilter = DeptFilter;
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
        private void IntiComboBoxCondition()
        {
            try
            {
                List<Status> statusList = new List<Status>();
                statusList.Add(new Status("退出条件", 0));
                statusList.Add(new Status("纳入条件", 1));
                //radComboBoxCondition.ItemsSource = statusList;
                //radComboBoxCondition.SelectedIndex = 0;
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// INIT路径列表的实体，主要是拼显示的病种名称
        /// </summary>
        private void InitEntity()
        {
            YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
            referenceClient.GetClinicalPathListCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            m_CompletedCount++;
                            m_ClinicalPathList = new List<CP_ClinicalPathList>();
                            m_ClinicalPathList = e.Result.ToList();
                            YidanEHRDataServiceClient referenceClient2 = PublicMethod.YidanClient;
                            referenceClient2.GetClinicalDiagnosisCompleted +=
                                (obj2, ea) =>
                                {
                                    if (ea.Error == null)
                                    {
                                        m_CompletedCount++;
                                        m_ClinicalDiagnosisList = new List<CP_ClinicalDiagnosisList>();
                                        m_ClinicalDiagnosisList = ea.Result.ToList();
                                        Entity2Grid();
                                    }
                                    else
                                    {
                                        PublicMethod.RadWaringBox(ea.Error);
                                    }
                                };
                            referenceClient2.GetClinicalDiagnosisAsync();
                            referenceClient2.CloseAsync();
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
            referenceClient.GetClinicalPathListAsync(string.Empty, string.Empty, string.Empty, string.Empty, string.Empty, String.Empty);
            referenceClient.CloseAsync();
        }
        /// <summary>
        /// 将拼好的路径LIST，赋值GRID
        /// </summary>
        private void Entity2Grid()
        {
            try
            {
                foreach (CP_ClinicalPathList path in m_ClinicalPathList)
                {
                    string strLjdm = path.Ljdm;
                    //List<CP_ClinicalDiagnosisList> diagList = (List<CP_ClinicalDiagnosisList> )m_ClinicalDiagnosisList.Select(delegate(CP_ClinicalDiagnosisList t)
                    //{ return (t.Ljdm == strLjdm); });
                    string strBzMc = string.Empty;
                    foreach (CP_ClinicalDiagnosisList list in m_ClinicalDiagnosisList)
                    {
                        if (list.Ljdm == strLjdm)
                            strBzMc += list.Bzmc + ";";
                    }
                    path.Bzmc = strBzMc;
                }
                radGridViewPathList.SelectionChanged -= new EventHandler<SelectionChangeEventArgs>(radGridViewPathList_SelectionChanged);
                radGridViewPathList.ItemsSource = m_ClinicalPathList;
                radGridViewPathList.SelectedItem = null;
                radGridViewPathList.SelectionChanged += new EventHandler<SelectionChangeEventArgs>(radGridViewPathList_SelectionChanged);
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void Reset()
        {
            try
            {
                this.radGridViewPathList.SelectionChanged -= new EventHandler<SelectionChangeEventArgs>(radGridViewPathList_SelectionChanged);

                //textBoxPathName.Text = string.Empty;
                //radNumericUpDownVersion.Value = 0;
                //radNumericUpDownInDays.Value = 0;
                //radNumericUpDownAvgFee.Value = 0;
                //radComboBoxStatus.SelectedValue = 1;
                //autoCompleteBoxDept.SelectedItem = null;
                //autoCompleteBoxDept.SelectedItem = null;
                //autoCompleteBoxDiagnosis.SelectedItem = null;
                //radGridViewAddCondtion.ItemsSource = null;
                //this.radGridViewDisease.ItemsSource = null;
                this.radGridViewPathList.SelectedItem = null;
                m_ClinicalPathShowSel = null;
                m_ClinicalDiagnosisListAdd.Clear();
                m_ClinicalDiagnosisListDele.Clear();
                //m_PathconditionListSel = null;
                //m_PathconditionListAdd.Clear();
                //m_PathconditionListDel.Clear();
                //this.textBoxConditon.Text = string.Empty;
                m_CopyWorkXml = string.Empty;
                this.radGridViewPathList.SelectionChanged += new EventHandler<SelectionChangeEventArgs>(radGridViewPathList_SelectionChanged);
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void Clear()
        {
            m_ClinicalDiagnosisList = null;
            m_ClinicalPathList = null;
            //m_PathconditionList = null;
            //m_PathconditionListTemp = null;
            m_ClinicalDiagnosisListAdd.Clear();
            m_ClinicalDiagnosisListDele.Clear();
            m_ClinicalPathShowSel = null;
            //m_PathconditionListSel = null;
            //m_PathconditionListAdd.Clear();
            //m_PathconditionListDel.Clear();
            //this.radGridViewAddCondtion.ItemsSource = null;
            //this.textBoxConditon.Text = string.Empty;
            //this.textBoxPathName.Text = string.Empty;
        }
        private void ShowWaringInfo(string strWarn)
        {
            PublicMethod.RadAlterBox(strWarn, m_Title);
        }
        private void GetPathList()
        {
            try
            {
                string strTimeFrom = this.dateTimeFrom.SelectedValue == null ? string.Empty : this.dateTimeFrom.SelectedValue.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string strTimeTo = this.dateTimeTo.SelectedValue == null ? string.Empty : this.dateTimeTo.SelectedValue.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string strKsdm = this.autoCompleteBoxQueryDept.SelectedItem == null ? string.Empty : ((CP_DepartmentList)this.autoCompleteBoxQueryDept.SelectedItem).Ksdm;
                string strLjmc = this.autoCompleteBoxPath.SelectedItem == null ? this.autoCompleteBoxPath.Text : ((CP_ClinicalPathList)this.autoCompleteBoxPath.SelectedItem).Name;
                string strYxjl = String.Empty;
                String strName = ConvertMy.ToString(radCmbYxjl.SelectionBoxItem);
                switch (strName)
                {
                    case "有效":
                        strYxjl = "1";
                        break;
                    case "停止":
                        strYxjl = "2";
                        break;
                    case "审核":
                        strYxjl = "3";
                        break;
                    default:
                        strYxjl = string.Empty;
                        break;
                }
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                referenceClient.GetClinicalPathListCompleted +=
                        (obj, e) =>
                        {
                            if (e.Error == null)
                            {
                                m_CompletedCount++;
                                m_ClinicalPathList = new List<CP_ClinicalPathList>();
                                m_ClinicalPathList = e.Result.ToList();
                                YidanEHRDataServiceClient referenceClient2 = PublicMethod.YidanClient;
                                referenceClient2.GetClinicalDiagnosisCompleted +=
                                    (obj2, ea) =>
                                    {
                                        if (ea.Error == null)
                                        {
                                            m_CompletedCount++;
                                            m_ClinicalDiagnosisList = new List<CP_ClinicalDiagnosisList>();
                                            m_ClinicalDiagnosisList = ea.Result.ToList();
                                            Entity2Grid();
                                        }
                                        else
                                        {
                                            PublicMethod.RadWaringBox(ea.Error);
                                        }
                                    };
                                referenceClient2.GetClinicalDiagnosisAsync();
                                referenceClient2.CloseAsync();
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(e.Error);
                            }
                        };
                referenceClient.GetClinicalPathListAsync(strTimeFrom, strTimeTo, strKsdm, string.Empty, strLjmc, strYxjl);
                referenceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// check是否有选中的数据
        /// </summary>
        /// <returns></returns>
        private bool GetIsSelectItem()
        {
            try
            {
                if (this.radGridViewPathList.ItemsSource == null)
                    return false;
                if (this.radGridViewPathList.SelectedItems.Count == 0)
                    return false;
                if ((this.radGridViewPathList.SelectedItem as CP_ClinicalPathList).WorkFlowXML == "")
                {
                    PublicMethod.RadAlterBox("没有配置路径!", "提示");
                    return false;
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
            return true;
        }
        #endregion


        private void btnDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (radGridViewPathList.SelectedItem == null)
                {

                    Reset();
                    PublicMethod.RadAlterBox("请选择一个路径！", "提示");
                    return;
                }
                //string strLjdm = ((RadButton)sender).Tag.ToString();
                // List<CP_ClinicalPathList> listPathInfo = new List<CP_ClinicalPathList>(); //(List<CP_ClinicalPathShowInfo>)this.radGridViewPathList.ItemsSource;
                //foreach (var infos in radGridViewPathList.Items)
                //{
                //    listPathInfo.Add(infos as CP_ClinicalPathList);
                //}
                // var path = listPathInfo.Where(pt => pt.Ljdm.Equals(strLjdm)).FirstOrDefault<CP_ClinicalPathList>();
                var path = radGridViewPathList.SelectedItem as CP_ClinicalPathList;
                 #region  add by luff 20130816 根据配置参数进入第三方控件的路径维护明细页还是微软控件的路径维护明细页
                List<APPCFG> t_listApp = Global.mAppCfg.Select(s => s).Where(s => s.Configkey.IndexOf("PathWh") > -1).ToList();
                if (t_listApp.Count > 0)
                {
                    if (t_listApp[0].Value == "1")//表示进入第三方控件的路径维护明细页
                    {
                        RWPathNodeSetting pathNode = new RWPathNodeSetting(path);
                        pathNode.IsEditEnable = false;
                        //add by luff 20120927
                        pathNode.WindowState = WindowState.Maximized;
                        pathNode.ResizeMode = ResizeMode.NoResize;
                        pathNode.m_bAduit = true;
                        pathNode.CurrentOperationState = OperationState.VIEW;
                        pathNode.ShowDialog();
                    }
                    else
                    {
                        RWPathNodeSettingMS pathNode = new RWPathNodeSettingMS(path);
                        pathNode.IsEditEnable = false;
                        //add by luff 20120927
                        pathNode.WindowState = WindowState.Maximized;
                        pathNode.ResizeMode = ResizeMode.NoResize;
                        pathNode.m_bAduit = true;
                        pathNode.CurrentOperationState = OperationState.VIEW;
                        pathNode.ShowDialog();
                    }
                }
                else
                {
                    RWPathNodeSettingMS pathNode = new RWPathNodeSettingMS(path);
                    pathNode.IsEditEnable = false;
                    //add by luff 20120927
                    pathNode.WindowState = WindowState.Maximized;
                    pathNode.ResizeMode = ResizeMode.NoResize;
                    pathNode.m_bAduit = true;
                    pathNode.CurrentOperationState = OperationState.VIEW;
                    pathNode.ShowDialog();

                }
                 #endregion

                //pathNode.Closed += new EventHandler<WindowClosedEventArgs>(pathNode_Closed);
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnCl_Click(object sender, RoutedEventArgs e)
        {
            autoCompleteBoxQueryDept.Text = "";
            autoCompleteBoxPath.Text = "";
            radCmbYxjl.SelectedIndex = 0;
            dateTimeFrom.SelectedValue = null;
            dateTimeTo.SelectedValue = null;
        }
    }
}
