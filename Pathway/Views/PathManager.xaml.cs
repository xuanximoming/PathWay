using DrectSoft.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;
using YidanEHRApplication.Views.ChildWindows;
namespace YidanEHRApplication.Views
{
    /// <summary>
    /// 路径配置界面
    /// </summary>
    public partial class PathManager : Page
    {
        public bool isLoad = true;


        #region 事件
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isLoad)
                {
                    isLoad = true;
                    return;
                }
                InitPage();
                CurrentState = OperationState.VIEW;     //zm 8.1
                RegisterKeyEvent();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 查询按钮
        /// 修改时间：2013年8月6日 10:03:58
        /// 修改人：Jhonny
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        /// <summary>
        /// 添加按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CurrentState = OperationState.NEW;
            Reset();
            textBoxPathName.Focus();
        }

        /// <summary>
        /// 修改按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (radGridViewPathList.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择一个路径再点击修改", "提示");
                    Reset();
                    return;
                }
                CurrentState = OperationState.EDIT;
                #region
                m_ClinicalPathShowSel = null;
                m_ClinicalDiagnosisListAdd.Clear();
                m_ClinicalDiagnosisListDele.Clear();
                //m_PathconditionListSel = null;
                //this.textBoxConditon.Text = string.Empty;
                #endregion
                m_ChangedCompletedCount = 3;
                radBusyIndicator.IsBusy = true;
                //todo 将CP_ClinicalPathList，CP_ClinicalPath，CP_ClinicalPathShowInfo 三个类合并
                CP_ClinicalPathList showInfo = (CP_ClinicalPathList)radGridViewPathList.SelectedItem;
                m_ClinicalPathShowSel = showInfo;
                if (showInfo.YxjlId == (int)PathShStatus.Review || showInfo.YxjlId == (int)PathShStatus.Dc)
                {
                    IntiComboBoxStatus(true);
                    btnSave.IsEnabled = false;
                }
                else
                {
                    IntiComboBoxStatus(false);
                    btnSave.IsEnabled = true;
                }
                FillSelectInfo(showInfo.Ljdm);
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                //referenceClient.GetPathConditionListByLjdmCompleted += new EventHandler<GetPathConditionListByLjdmCompletedEventArgs>(referenceClient_GetPathConditionListByLjdmCompleted);
                //referenceClient.GetPathConditionListByLjdmAsync(m_ClinicalPathShowSel.Ljdm);
                //referenceClient.CloseAsync();
                m_ChangedCompletedTimer = new System.Windows.Threading.DispatcherTimer();
                m_ChangedCompletedTimer.Interval = new TimeSpan(0, 0, 1);
                m_ChangedCompletedTimer.Tick += new EventHandler(m_ChangedCompletedTimer_Tick);
                m_ChangedCompletedTimer.Start();
                BindGridView(m_ClinicalPathShowSel);

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
            //textBoxPathName.Focus();
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            CurrentState = OperationState.VIEW;
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            //radBusyIndicator.IsBusy = true; 跟INITPAGE里有冲突，DEBUG时没有错误。暂放
            try
            {
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                if (CurrentState == OperationState.NEW)
                {
                    if (Check())
                    {
                        //GetContiongAddDel();

                        if (m_ClinicalPathShowSel == null)
                        {
                            referenceClient.InsertCPListCompleted +=
                                (obj, ea) =>
                                {
                                    try
                                    {
                                        //radBusyIndicator.IsBusy = false;
                                        if (ea.Error == null)
                                        {
                                            if (ea.Result == "0")
                                            {
                                                String tishi = "路径名称【" + textBoxPathName.Text + "】已经存在,请修改";
                                                PublicMethod.RadAlterBox(tishi, "提示");
                                                Reset();
                                            }
                                            else
                                            {
                                                //radbuttonQuery_Click(null, null);
                                                InitComboBoxPath();
                                                InitEntity();
                                                ShowWaringInfo("保存成功");
                                                CurrentState = OperationState.VIEW;
                                                Reset();
                                                //增加定位新增记录的功能.2013-04-08
                                                //radGridViewPathList                                               
                                            }
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
                            //暂时
                            CP_ClinicalDiagnosisList cpList = new CP_ClinicalDiagnosisList();
                            cpList.Bzdm = Guid.NewGuid().ToString();                //改为自动生成
                            cpList.Bzmc = "";
                            cpList.Ljdm = "";
                            m_ClinicalDiagnosisListAdd.Add(cpList);
                            referenceClient.InsertCPListAsync(this.textBoxPathName.Text, this.textBoxPathName.Text, (double)radNumericUpDownInDays.Value, (double)radNumericUpDownAvgFee.Value,
                                 (double)radNumericUpDownVersion.Value, string.Empty,
                                 (int)radComboBoxStatus.SelectedValue, ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm, m_ClinicalDiagnosisListAdd);
                            referenceClient.CloseAsync();
                        }

                    }
                }

                // 修改数据 add by luff 20120928
                if (CurrentState == OperationState.EDIT)
                {
                    referenceClient.UpdateCPListCompleted +=
                        (obj, ea) =>
                        {
                            try
                            {
                                //radBusyIndicator.IsBusy = false;
                                if (ea.Error == null)
                                {
                                    if (ea.Result == 0)
                                    {
                                        InitComboBoxPath();
                                        InitEntity();
                                        ShowWaringInfo("更新成功");
                                        CurrentState = OperationState.VIEW;
                                        InitComboBoxPath();
                                        InitEntity();
                                    }
                                    else
                                    {
                                        String tishi = "路径名称已经存在,请修改";
                                        PublicMethod.RadAlterBox(tishi, "提示");
                                        Reset();
                                    }
                                    //radbuttonQuery_Click(null, null);

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
                    referenceClient.UpdateCPListAsync(m_ClinicalPathShowSel.Ljdm, this.textBoxPathName.Text, this.textBoxPathName.Text,
                        (double)radNumericUpDownInDays.Value, (double)radNumericUpDownAvgFee.Value,
                            (double)radNumericUpDownVersion.Value, Global.LogInEmployee.Zgdm,
                            (int)radComboBoxStatus.SelectedValue, ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm,
                            m_ClinicalDiagnosisListAdd, m_ClinicalDiagnosisListDele);
                    referenceClient.CloseAsync();
                }

                Reset();

            }
            catch (Exception ex)
            {
                radBusyIndicator.IsBusy = false;
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
            finally
            {
                m_ClinicalDiagnosisListAdd.Clear();
                m_ClinicalDiagnosisListDele.Clear();
            }
        }


        #region 复制路径（路径状态是审核和病人使用中）add by luff 20130423
        private void btnCopy_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RadWindow w = new RWCopyPath(ManualType.New, m_ClinicalPathShowSel.Ljdm, m_ClinicalPathShowSel, (ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource);
                w.ShowDialog();
                w.Closed += new EventHandler<WindowClosedEventArgs>(w_Closed);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        /// <summary>
        /// 弹出窗口关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void w_Closed(object sender, WindowClosedEventArgs e)
        {
            try
            {
                //刷新数据源
                InitComboBoxPath();
                InitEntity();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        #endregion
        /// <summary>
        /// 页面重置按钮事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            CurrentState = OperationState.NEW;
            Reset();
        }
        private void btnConfig_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (radGridViewPathList.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择一个路径再点击配置", "提示");
                    Reset();
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
                #region  add by luff 20130815 根据配置参数进入第三方控件的路径维护明细页还是微软控件的路径维护明细页
                List<APPCFG> t_listApp = Global.mAppCfg.Select(s => s).Where(s => s.Configkey.IndexOf("PathWh") > -1).ToList();
                if (t_listApp.Count > 0)
                {
                    if (t_listApp[0].Value == "1")//表示进入第三方控件的路径维护明细页
                    {
                        RWPathNodeSetting pathNode = new RWPathNodeSetting(path);
                        if (path.YxjlId == (int)PathShStatus.Review || path.YxjlId == (int)PathShStatus.Dc)
                            pathNode.IsEditEnable = false;
                        else
                            pathNode.IsEditEnable = true;
                        pathNode.WindowState = WindowState.Maximized;
                        pathNode.ResizeMode = ResizeMode.NoResize;
                        pathNode.ShowDialog();
                        pathNode.Closed += new EventHandler<WindowClosedEventArgs>(pathNode_Closed);
                    }
                    else//表示进入微软控件路径维护页面
                    {
                        RWPathNodeSettingMS pathNode = new RWPathNodeSettingMS(path);
                        if (path.YxjlId == (int)PathShStatus.Review || path.YxjlId == (int)PathShStatus.Dc)
                            pathNode.IsEditEnable = false;
                        else
                            pathNode.IsEditEnable = true;
                        pathNode.WindowState = WindowState.Maximized;
                        pathNode.ResizeMode = ResizeMode.NoResize;
                        pathNode.ShowDialog();
                        pathNode.Closed += new EventHandler<WindowClosedEventArgs>(pathNode_Closed);
                    }
                }
                else//表示进入微软控件路径维护页面
                {
                    RWPathNodeSettingMS pathNode = new RWPathNodeSettingMS(path);
                    if (path.YxjlId == (int)PathShStatus.Review || path.YxjlId == (int)PathShStatus.Dc)
                        pathNode.IsEditEnable = false;
                    else
                        pathNode.IsEditEnable = true;
                    pathNode.WindowState = WindowState.Maximized;
                    pathNode.ResizeMode = ResizeMode.NoResize;
                    pathNode.ShowDialog();
                    pathNode.Closed += new EventHandler<WindowClosedEventArgs>(pathNode_Closed);
                }

                #endregion


            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void pathNode_Closed(object sender, WindowClosedEventArgs e)
        {
        }
        private void m_ChangedCompletedTimer_Tick(object sender, EventArgs e)
        {
            if (m_ChangedCompletedCount == m_Completed)
            {
                radBusyIndicator.IsBusy = false;
                m_ChangedCompletedTimer.Stop();
            }
            //textBoxPathName.Focus();
        }
        private void m_CompletedTimer_Tick(object sender, EventArgs e)
        {
            if (m_CompletedCount == m_Completed)
            {
                radBusyIndicator.IsBusy = false;
                m_CompletedTimer.Stop();
            }
        }
        private void radGridViewPathListSelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            if (radGridViewPathList.SelectedItem == null)
            {
                return;
            }
            if (CurrentState == OperationState.VIEW)
            {
                CP_ClinicalPathList showInfo = (CP_ClinicalPathList)radGridViewPathList.SelectedItem;

                if (showInfo.YxjlId == (int)PathShStatus.Review || showInfo.YxjlId == (int)PathShStatus.Dc)
                {
                    btnUpdate.IsEnabled = false;
                    btnConfig.IsEnabled = false;
                }
                else
                {
                    btnUpdate.IsEnabled = true; btnConfig.IsEnabled = true;
                }
                // add by luff 20120928
                if (showInfo.YxjlId == 3)
                {
                    this.btnDetail.IsEnabled = true;//.Visibility = Visibility.Visible;
                }
                else
                {
                    this.btnDetail.IsEnabled = false;//.Visibility = Visibility.Collapsed;
                }
                // add by luff 20120928
                if (showInfo.YxjlId == 3 && showInfo.LjSyqk.Trim() == "病患使用中")
                {
                    m_ClinicalPathShowSel = showInfo;
                    this.btnCopy.IsEnabled = true;
                }
                else
                {
                    this.btnCopy.IsEnabled = false;
                }
            }
        }

        #region  输入框加回车事件,验证回车的时候文本框的顺序
        private void RegisterKeyEvent()
        {
            textBoxPathName.KeyUp += new KeyEventHandler(textBoxPathName_KeyUp);
            radNumericUpDownVersion.KeyUp += new KeyEventHandler(radNumericUpDownVersion_KeyUp);
            radNumericUpDownInDays.KeyUp += new KeyEventHandler(radNumericUpDownInDays_KeyUp);

            radComboBoxStatus.KeyUp += new KeyEventHandler(radComboBoxStatus_KeyUp);

            autoCompleteBoxDept.KeyUp += new KeyEventHandler(autoCompleteBoxDept_KeyUp);
            radNumericUpDownAvgFee.KeyUp += new KeyEventHandler(radNumericUpDownAvgFee_KeyUp);
            btnSave.KeyUp += new KeyEventHandler(btnSave_KeyUp);

            autoCompleteBoxQueryDept.KeyUp += new KeyEventHandler(tbQuery2_KeyUp);
            autoCompleteBoxQueryPath.KeyUp += new KeyEventHandler(tbQuery2_KeyUp);
            radCmbYxjl.KeyUp += new KeyEventHandler(tbQuery2_KeyUp);
        }

        private void textBoxPathName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                radNumericUpDownVersion.Focus();
        }

        private void radNumericUpDownVersion_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                radNumericUpDownInDays.Focus();
        }

        private void radNumericUpDownInDays_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                radComboBoxStatus.Focus();
        }

        private void radComboBoxStatus_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                autoCompleteBoxDept.Focus();
        }

        private void autoCompleteBoxDept_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                radNumericUpDownAvgFee.Focus();
        }

        private void radNumericUpDownAvgFee_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSave.Focus();
        }

        private void btnSave_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSave_Click(null, null);
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
        /// <summary>
        /// 选中的记录
        /// </summary>
        private CP_ClinicalPathList m_ClinicalPathShowSel = null;
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
        OperationState _CurrentState = OperationState.VIEW;
        OperationState CurrentState
        {
            get { return _CurrentState; }
            set
            {
                _CurrentState = value;
                btnAdd.IsEnabled = value == OperationState.VIEW;
                btnUpdate.IsEnabled = value == OperationState.VIEW;
                btnClear.IsEnabled = value != OperationState.VIEW;
                btnSave.IsEnabled = value != OperationState.VIEW;
                btnReset.IsEnabled = value != OperationState.VIEW;
                textBoxPathName.IsEnabled = value != OperationState.VIEW;
                radNumericUpDownVersion.IsEnabled = value != OperationState.VIEW;
                radNumericUpDownInDays.IsEnabled = value != OperationState.VIEW;
                radNumericUpDownAvgFee.IsEnabled = value != OperationState.VIEW;
                radComboBoxStatus.IsEnabled = value != OperationState.VIEW;
                autoCompleteBoxDept.IsEnabled = value != OperationState.VIEW;
                btnConfig.IsEnabled = value == OperationState.VIEW;
            }
        }
        #endregion

        #region 函数
        public PathManager()
        {
            ICommand deleteCommand = RadGridViewCommands.Delete;
            ICommand beginInsertCommand = RadGridViewCommands.BeginInsert;
            ICommand cancelRowEditCommand = RadGridViewCommands.CancelRowEdit;
            ICommand commitEditCommand = RadGridViewCommands.CommitEdit;
            InitializeComponent();
        }
        private bool Check()
        {
            if (this.textBoxPathName.Text.Trim() == string.Empty)
            {
                PublicMethod.RadAlterBoxRe("请输入路径名称", m_Title, textBoxPathName);
                isLoad = false;
                return false;
            }
            if (this.textBoxPathName.Text.Trim().Length >= 60)
            {
                PublicMethod.RadAlterBoxRe("路径名称长度不能超过30位", m_Title, textBoxPathName); isLoad = false;
                return false;
            }
            if (this.radComboBoxStatus.SelectedValue == null)
            {
                PublicMethod.RadAlterBoxRe("请选择使用状态", m_Title, radComboBoxStatus); isLoad = false;
                return false;
            }
            if (this.autoCompleteBoxDept.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择科室", m_Title, autoCompleteBoxDept); isLoad = false;
                return false;
            }
            if (this.radNumericUpDownVersion.ContentText.Trim() == string.Empty)
            {
                PublicMethod.RadAlterBoxRe("请选择科室", m_Title, radNumericUpDownVersion); isLoad = false;
                return false;
            }
            if (this.radNumericUpDownInDays.ContentText.Trim() == string.Empty)
            {
                PublicMethod.RadAlterBoxRe("住院天数不能为空", m_Title, radNumericUpDownInDays); isLoad = false;
                return false;
            }
            if (this.radNumericUpDownAvgFee.ContentText.Trim() == string.Empty)
            {
                PublicMethod.RadAlterBoxRe("费用不能为空", m_Title, radNumericUpDownAvgFee); isLoad = false;
                return false;
            }

            //else if (this.radGridViewDisease.Items.Count == 0)
            //{
            //    ShowWaringInfo("请选择病种");
            //    this.autoCompleteBoxDiagnosis.Focus();
            //    return false;
            //}
            //else if (this.radGridViewAddCondtion.Items.Count == 0)
            //{
            //    ShowWaringInfo("请填写路径条件");
            //    this.textBoxConditon.Focus();
            //    return false;
            //}
            //radGridViewPathList
            foreach (CP_ClinicalPathList item in radGridViewPathList.Items)
            {
                if (item.Name == textBoxPathName.Text.Trim())
                {
                    //PublicMethod.RadAlterBox("该路径名称已被使用，请重新命名！", "提示");
                    //this.textBoxPathName.Focus();
                    PublicMethod.RadAlterBoxRe("该路径名称已被使用，请重新命名！", m_Title, textBoxPathName);
                    isLoad = false;
                    return false;
                }
            }
            return true;
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
                IntiComboBoxStatus(false);
                IntiComboBoxDept();
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
        #region dxj 拼音检索路径
        /// <summary>
        /// INIT路径
        /// </summary>
        private void InitComboBoxPath()
        {
            try
            {
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                referenceClient.GetClinicalPathListInfoCompleted += (obj, ea) =>
                {
                    if (ea.Error == null)
                    {
                        m_CompletedCount++;
                        autoCompleteBoxQueryPath.ItemsSource = ea.Result.ToList();
                        autoCompleteBoxQueryPath.ItemFilter = PathFilter;
                    }
                };
                referenceClient.GetClinicalPathListInfoAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 拼音检索方法ToUpper add by luff 2012-08-08
        /// </summary>
        /// <param name="strFilter"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool PathFilter(string strFilter, object item)
        {
            CP_ClinicalPathList info = (CP_ClinicalPathList)item;
            //return ((info.QueryName.StartsWith(strFilter.ToUpper())) || (info.QueryName.Contains(strFilter.ToUpper())) || (info.Py.Contains(strFilter.ToUpper())) || (info.Py.StartsWith(strFilter.ToUpper())));
            return ((info.QueryName.StartsWith(strFilter.ToUpper())) || (info.QueryName.Contains(strFilter.ToUpper())));
        }
        #endregion
        #region 1选择病种   radComboBoxDiagnosis
        public bool DiagnosisFilter(string strFilter, object item)
        {
            CP_DiagnosisList info = (CP_DiagnosisList)item;
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
        #endregion
        #region 路径状态 radComboBoxStatus
        /// <summary>
        ///   INIT路径状态
        /// </summary>
        /// <param name="isReivew">是否要加审核</param>
        private void IntiComboBoxStatus(bool isReivew)
        {
            try
            {
                radComboBoxStatus.ItemsSource = null;
                List<Status> statusList = new List<Status>();
                statusList.Add(new Status("无效", (int)PathShStatus.Cancel));
                statusList.Add(new Status("有效", (int)PathShStatus.Valid));
                statusList.Add(new Status("停止", (int)PathShStatus.Dc));
                if (isReivew)
                    statusList.Add(new Status("审核", (int)PathShStatus.Review));
                radComboBoxStatus.ItemsSource = statusList;
                radComboBoxStatus.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region 2科室   radComboBoxDept
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
                //radComboBoxDept.EmptyText = "数据生成中...";
                //radComboBoxDept.IsEnabled = false;
                //referenceClient.GetDepartment4AutoComboBoxCompleted += new EventHandler<GetDepartment4AutoComboBoxCompletedEventArgs>(referenceClient_GetDepartment4AutoComboBoxCompleted);
                //referenceClient.GetDepartment4AutoComboBoxAsync();
                //referenceClient.CloseAsync();
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
        #region 3,4 路径列表，和 对应的病种
        /// <summary>
        /// INIT路径列表的实体，主要是拼显示的病种名称
        /// </summary>
        private void InitEntity()
        {
            YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
            referenceClient.GetClinicalPathListCompleted +=
                    (obj, ea) =>
                    {
                        if (ea.Error == null)
                        {
                            m_CompletedCount++;
                            m_ClinicalPathList = new List<CP_ClinicalPathList>();
                            m_ClinicalPathList = ea.Result.ToList();
                            YidanEHRDataServiceClient referenceClient2 = PublicMethod.YidanClient;
                            referenceClient2.GetClinicalDiagnosisCompleted +=
                                    (obj2, e) =>
                                    {
                                        if (e.Error == null)
                                        {
                                            m_CompletedCount++;
                                            m_ClinicalDiagnosisList = new List<CP_ClinicalDiagnosisList>();
                                            m_ClinicalDiagnosisList = e.Result.ToList();
                                            Entity2Grid();
                                        }
                                        else
                                        {
                                            PublicMethod.RadWaringBox(e.Error);
                                        }
                                    };
                            referenceClient2.GetClinicalDiagnosisAsync();
                            referenceClient2.CloseAsync();
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(ea.Error);
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
                // radGridViewPathList.SelectionChanged -= new EventHandler<SelectionChangeEventArgs>(radGridViewPathList_SelectionChanged);
                radGridViewPathList.ItemsSource = m_ClinicalPathList;
                //radGridViewPathList.SelectedItem = null;
                radGridViewPathList.SelectedItem = m_ClinicalPathList.Last();
                // radGridViewPathList.SelectionChanged += new EventHandler<SelectionChangeEventArgs>(radGridViewPathList_SelectionChanged);
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void FillSelectInfo(string strLjdm)
        {
            try
            {
                var path = m_ClinicalPathList.Where(pt => pt.Ljdm.Equals(strLjdm)).FirstOrDefault<CP_ClinicalPathList>();
                var dialist = m_ClinicalDiagnosisList.Where(dia => dia.Ljdm.Equals(strLjdm));
                //this.radGridViewDisease.ItemsSource = dialist.ToList<CP_ClinicalDiagnosisList>();
                FillPathBaiscInfo(path);
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void FillPathBaiscInfo(CP_ClinicalPathList pathList)
        {
            try
            {
                textBoxPathName.Text = pathList.Name;
                radNumericUpDownVersion.Value = double.Parse(pathList.Vesion.ToString());
                radNumericUpDownInDays.Value = double.Parse(pathList.Zgts.ToString());
                radNumericUpDownAvgFee.Value = double.Parse(pathList.Jcfy.ToString());
                radComboBoxStatus.SelectedValue = pathList.YxjlId;
                autoCompleteBoxDept.SelectedItem = ((ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource).FirstOrDefault(cp => cp.Ksdm.Equals(pathList.Syks));
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        private void Reset()
        {
            try
            {
                //this.radGridViewPathList.SelectionChanged -= new EventHandler<SelectionChangeEventArgs>(radGridViewPathList_SelectionChanged);
                IntiComboBoxStatus(false);
                textBoxPathName.Text = string.Empty;
                radNumericUpDownVersion.Value = 1;
                radNumericUpDownInDays.Value = 0;
                radNumericUpDownAvgFee.Value = 0;
                radComboBoxStatus.SelectedValue = 1;
                //===================2013-05-06,WangGuojin========================
                autoCompleteBoxDept.SelectedItem = ((ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource).FirstOrDefault(cp => cp.Ksdm.Equals(Global.LogInEmployee.Ksdm));
                //================================================================                
                //autoCompleteBoxDiagnosis.SelectedItem = null;
                //radGridViewAddCondtion.SelectedItem = null;
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
                //this.radGridViewPathList.SelectionChanged += new EventHandler<SelectionChangeEventArgs>(radGridViewPathList_SelectionChanged);
                //GrdConditonList.ItemsSource = null;              

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
            this.textBoxPathName.Text = string.Empty;
        }
        private void ShowWaringInfo(string strWarn)
        {
            PublicMethod.RadAlterBoxRe(strWarn, m_Title, radGridViewPathList);

        }
        /// <summary>
        /// 查看已经审核的路径 add by luff 20120927
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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
                CP_ClinicalPathList _cp_list = (CP_ClinicalPathList)radGridViewPathList.SelectedItem;
                //当该路径是审核状态 add by luff 20120927
                if (_cp_list.YxjlId == 3)
                {
                    //this.btnDetail.Visibility = Visibility.Visible;
                    this.btnDetail.IsEnabled = true;
                    var path = radGridViewPathList.SelectedItem as CP_ClinicalPathList;
                    #region  add by luff 20130815 根据配置参数进入第三方控件的路径维护明细页还是微软控件的路径维护明细页
                    List<APPCFG> t_listApp = Global.mAppCfg.Select(s => s).Where(s => s.Configkey.IndexOf("PathWh") > -1).ToList();
                    if (t_listApp.Count > 0)
                    {
                        if (t_listApp[0].Value == "1")//表示进入第三方控件的路径维护明细页
                        {
                            RWPathNodeSetting pathNode = new RWPathNodeSetting(path);
                            pathNode.IsEditEnable = false;
                            pathNode.WindowState = WindowState.Maximized;
                            pathNode.ResizeMode = ResizeMode.NoResize;
                            pathNode.m_bAduit = true;
                            pathNode.CurrentOperationState = OperationState.VIEW;
                            pathNode.ShowDialog();
                        }
                        else//表示进入微软控件路径维护页面
                        {
                            RWPathNodeSettingMS pathNode = new RWPathNodeSettingMS(path);
                            pathNode.IsEditEnable = false;
                            pathNode.WindowState = WindowState.Maximized;
                            pathNode.ResizeMode = ResizeMode.NoResize;
                            pathNode.m_bAduit = true;
                            pathNode.CurrentOperationState = OperationState.VIEW;
                            pathNode.ShowDialog();
                        }
                    }
                    else//表示进入微软控件路径维护页面
                    {
                        RWPathNodeSettingMS pathNode = new RWPathNodeSettingMS(path);
                        pathNode.IsEditEnable = false;
                        pathNode.WindowState = WindowState.Maximized;
                        pathNode.ResizeMode = ResizeMode.NoResize;
                        pathNode.m_bAduit = true;
                        pathNode.CurrentOperationState = OperationState.VIEW;
                        pathNode.ShowDialog();
                    }
                    #endregion

                }
                else
                {
                    //this.btnDetail.Visibility = Visibility.Collapsed;
                    this.btnDetail.IsEnabled = false;
                }

                //pathNode.Closed += new EventHandler<WindowClosedEventArgs>(pathNode_Closed);
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void GetPathList()
        {
            try
            {
                string strTimeFrom = this.dateTimeFrom.SelectedValue == null ? string.Empty : this.dateTimeFrom.SelectedValue.Value.ToString("yyyy-MM-dd HH:mm:ss");
                string strTimeTo = this.dateTimeTo.SelectedValue == null ? string.Empty : this.dateTimeTo.SelectedValue.Value.ToString("yyyy-MM-dd HH:mm:ss");
                //add by luff 20120928
                if (strTimeFrom != "" && strTimeTo != "")
                {
                    if (DateTime.Parse(strTimeFrom) > DateTime.Parse(strTimeTo))
                    {
                        PublicMethod.RadAlterBox("结束日期不能小于开始日期!", "提示");
                        return;
                    }

                }
                string strKsdm = this.autoCompleteBoxQueryDept.SelectedItem == null ? string.Empty : ((CP_DepartmentList)this.autoCompleteBoxQueryDept.SelectedItem).Ksdm;
                string strLjmc = this.autoCompleteBoxQueryPath.SelectedItem == null ? this.autoCompleteBoxQueryPath.Text : ((CP_ClinicalPathList)this.autoCompleteBoxQueryPath.SelectedItem).Name;
                String strYxjl = String.Empty;
                String strName = ConvertMy.ToString(radCmbYxjl.SelectionBoxItem);
                switch (strName)
                {
                    case "无效":
                        strYxjl = "0";
                        break;
                    case "有效":
                        strYxjl = "1";
                        break;
                    case "停止":
                        strYxjl = "2";
                        break;
                    case "审核":
                        strYxjl = "3";
                        break;
                    //case "无效":
                    //    strYxjl = "4";
                    //    break;
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
        private void BindGridView(CP_ClinicalPathList _Path)
        {
            if (_Path == null || _Path.Ljdm == null || _Path.Ljdm.ToString().Trim() == "") return;
            YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
            referenceClient.GetPathCP_PathEnterJudgeConditionAllCompleted += (send, ea) =>
            {
                //GrdConditonList.ItemsSource = ea.Result;
                radBusyIndicator.IsBusy = false;
            };
            referenceClient.GetPathCP_PathEnterJudgeConditionAllAsync(_Path.Ljdm);
            referenceClient.CloseAsync();
        }
        #endregion

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }

        private void btnCl_Click(object sender, RoutedEventArgs e)
        {
            autoCompleteBoxQueryDept.SelectedItem = null;
            autoCompleteBoxQueryDept.Text = "";
            autoCompleteBoxQueryPath.Text = "";
            autoCompleteBoxQueryPath.SelectedItem = null;
            radCmbYxjl.SelectedIndex = 0;
            dateTimeFrom.SelectedValue = null;
            dateTimeTo.SelectedValue = null;
        }



    }

    /// <summary>
    /// combox 控件 item 
    /// </summary>
    public struct Status
    {
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        private string _name;
        public int Value
        {
            get { return _value; }
            set { _value = value; }
        }
        private int _value;
        /// <summary>
        /// ctor
        /// </summary>
        /// <param name="strName"></param>
        /// <param name="strValue"></param>
        public Status(string strName, int strValue)
        {
            _name = strName;
            _value = strValue;
        }
    }



}
