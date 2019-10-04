using DrectSoft.Tool;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Media;
using Telerik.Windows.Controls;
using YidanEHRApplication.ChildWindows;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;
using YidanEHRApplication.Views.ChildWindows;
using YidanEHRApplication.Views.NursingNotes;


namespace YidanEHRApplication.Controls
{
    public partial class UCInpatientListControl : UserControl
    {
        List<int> itemCount = new List<int>();//用于DataPager的数据提供

        string ljzt = string.Empty;

        //2013-05-16,WangGuojin
        public int m_iCurrentPage = 1;
        //
        public RWAccessPath2 accessWindow;
        #region 事件
        protected virtual void OnNavigateToPage(OpreateEventArgs e)
        {
            if (AfterNavigateToPage != null)
            {
                AfterNavigateToPage(this, e);
            }
        }
        public UCInpatientListControl()
        {
            InitializeComponent();
            //PageScrollViewer.MouseRightButtonDown += (sender, e) =>
            //{
            //    e.Handled = true;
            //    //自己的菜单
            //};
            Loaded += new RoutedEventHandler(UCInpatientListControl_Loaded);

        }

        //add by luff 20130308
        void UCInpatientListControl_Loaded(object sender, RoutedEventArgs e)
        {
            QcEventArgs qcargs = new QcEventArgs();
            //初始DataGrid双击事件
            DataGridDoubleClick();

            if (qcargs.Brzt == "1503" || qcargs.Brzt == "1502")
            {
                //this.PathZx.IsEnabled = false;
                this.rabInPath.IsEnabled = false;
            }
            else
            {
                this.rabInPath.IsEnabled = true;
            }
        }



        void qcPatientInfoControl1_AfterSumaryInfoClicked(object sender, RoutedEventArgs e)
        {

            this.radBusyIndicator.IsBusy = true;
            QcSumaryChanageArgs agrs = e as QcSumaryChanageArgs;

            //bandDataGrid(0, agrs.Status.ToString());


            if (agrs.Status == PathStatus.None)//全部空
            {
                ljzt = string.Empty;
                //radGridViewInpatient.ItemsSource = m_InpatientList;
            }
            else if (agrs.Status == PathStatus.NotIn)
            {
                ljzt = Convert.ToString((int)PathStatus.NotIn);
                //radGridViewInpatient.ItemsSource = m_InpatientList.Where(cp => cp.Ljzt == Convert.ToString((int)PathStatus.NotIn));
            }
            else if (agrs.Status == PathStatus.New)//未引入-1
            {
                ljzt = Convert.ToString((int)PathStatus.New);
                //radGridViewInpatient.ItemsSource = m_InpatientList.Where(cp => cp.Ljzt == Convert.ToString((int)PathStatus.New));
            }
            else if (agrs.Status == PathStatus.InPath)
            {
                ljzt = Convert.ToString((int)PathStatus.InPath);
                //radGridViewInpatient.ItemsSource = m_InpatientList.Where(cp => cp.Ljzt == Convert.ToString((int)PathStatus.InPath));
            }
            else if (agrs.Status == PathStatus.QuitPath)
            {
                ljzt = Convert.ToString((int)PathStatus.QuitPath);
                //radGridViewInpatient.ItemsSource = m_InpatientList.Where(cp => cp.Ljzt == Convert.ToString((int)PathStatus.QuitPath));
            }
            else if (agrs.Status == PathStatus.DonePath)//完成1
            {
                ljzt = Convert.ToString((int)PathStatus.DonePath);
                //radGridViewInpatient.ItemsSource = m_InpatientList.Where(cp => cp.Ljzt == Convert.ToString((int)PathStatus.DonePath));
            }
            QcEventArgs qcargs = new QcEventArgs();

            GetPageCount(qcargs, ljzt);
            bandDataGrid(0, ljzt);
            if (qcargs.Brzt == "1503" || qcargs.Brzt == "1502")
            {
                //this.PathZx.IsEnabled = false;
                this.rabInPath.IsEnabled = false;
            }
            else
            {
                this.rabInPath.IsEnabled = true;
            }

            this.radBusyIndicator.IsBusy = false;
        }
        void queryPathInfoControl1_AfterQuryInfoClicked(object sender, RoutedEventArgs e)
        {

            QcEventArgs qcargs = e as QcEventArgs;
            if (qcargs == null) return;
            this.radBusyIndicator.IsBusy = true;
            GetPageCount(qcargs, ljzt);
            //string ljzt = string.Empty;
            //GetInpatientList2(Global.LogInEmployee.Ksdm, m_DoctorID, qcargs.Hzxm, qcargs.Zyhm, qcargs.BedNo, qcargs.StartDate, qcargs.EndDate);
            bandDataGrid(1, ljzt);

            //QcEventArgs qcargs = e as QcEventArgs;
            //if (qcargs == null) return;
            //this.radBusyIndicator.IsBusy = true;
            //GetInpatientList2(Global.LogInEmployee.Ksdm, m_DoctorID, qcargs.Hzxm, qcargs.Zyhm, qcargs.BedNo, qcargs.StartDate, qcargs.EndDate);
        }
        private void radGridViewInpatient_RowActivated(object sender, Telerik.Windows.Controls.GridView.RowEventArgs e)
        {
            try
            {
                CP_InpatinetList cp = (CP_InpatinetList)radGridViewInpatient.SelectedItem;
                if (cp == null) return;
                Global.InpatientListCurrent = cp;
                //add by luff 201303206  当病人状态为出院时，隐藏相关入径按钮
                if (Global.InpatientListCurrent.Status == "1503" || Global.InpatientListCurrent.Status == "1502")
                {
                    //this.PathZx.IsEnabled = false;
                    this.rabInPath.IsEnabled = false;
                }
                if (Global.LogInEmployee != null)
                    Global.InpatientListCurrent.CurOper = Global.LogInEmployee.Zgdm;
                //判断选中的病人是否为选择了路径
                if (!string.IsNullOrEmpty(Global.InpatientListCurrent.Ljdm))
                {
                    OnNavigateToPage(new OpreateEventArgs(true, cp));
                }
                else
                {
                    //2013-05-10,WangGuojin, add it in order to show inpatient information
                    //PublicMethod.RadAlterBox("请选择已经引入路径的病人!", "提示");
                    //return;
                    RWPatInfo pathInfoView = new RWPatInfo(cp, 2);
                    pathInfoView.ShowDialog();
                    pathInfoView.Closed += new EventHandler<WindowClosedEventArgs>(w_Closed);
                    /*
                    int iChange = 0;
                    if (pathInfoView.isRyzdChanged == true)
                        iChange = 1;
                    PublicMethod.RadAlterBox(pathInfoView.m_iaccess.ToString(), "提示");
                    if (pathInfoView.m_iaccess > 0)
                    {
                        accessWindow = new RWAccessPath2(pathInfoView.m_CurrentPat, pathInfoView.m_iPage);
                        accessWindow.Closed += new EventHandler<WindowClosedEventArgs>(accessWindow_Closed);
                        accessWindow.ShowDialog();
                    }
                    //2013-05-13,WangGuojin
                    //refresh current page if pathinfo.ryzd has been changed.
                    if (iChange > 0)
                    {
                        //
                    }
                    */

                }



            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        //add by luff 20130606 微软自定DataGrid控件双击事件
        private MouseClickManager _gridClickManager;

        private void DataGridDoubleClick()
        {
            //在构造函数中添加响应事件
            _gridClickManager = new MouseClickManager(radGridViewInpatient, 300);
            _gridClickManager.DoubleClick += new System.Windows.Input.MouseButtonEventHandler(_gridClickManager_DoubleClick);
        }
        #region 行双击事件
        private void _gridClickManager_DoubleClick(object sender, MouseButtonEventArgs e)
        {
            try
            {
                CP_InpatinetList cp = (CP_InpatinetList)radGridViewInpatient.SelectedItem;
                if (cp == null) return;
                Global.InpatientListCurrent = cp;
                //add by luff 201303206  当病人状态为出院时，隐藏相关入径按钮
                if (Global.InpatientListCurrent.Status == "1503" || Global.InpatientListCurrent.Status == "1502")
                {
                    //this.PathZx.IsEnabled = false;
                    this.rabInPath.IsEnabled = false;
                }
                if (Global.LogInEmployee != null)
                    Global.InpatientListCurrent.CurOper = Global.LogInEmployee.Zgdm;
                //判断选中的病人是否为选择了路径
                if (!string.IsNullOrEmpty(Global.InpatientListCurrent.Ljdm))
                {
                    OnNavigateToPage(new OpreateEventArgs(true, cp));
                }
                else
                {
                    //2013-05-10,WangGuojin, add it in order to show inpatient information
                    //PublicMethod.RadAlterBox("请选择已经引入路径的病人!", "提示");
                    //return;
                    RWPatInfo pathInfoView = new RWPatInfo(cp, 2);
                    pathInfoView.ShowDialog();
                    pathInfoView.Closed += new EventHandler<WindowClosedEventArgs>(w_Closed);
                    /*
                    int iChange = 0;
                    if (pathInfoView.isRyzdChanged == true)
                        iChange = 1;
                    PublicMethod.RadAlterBox(pathInfoView.m_iaccess.ToString(), "提示");
                    if (pathInfoView.m_iaccess > 0)
                    {
                        accessWindow = new RWAccessPath2(pathInfoView.m_CurrentPat, pathInfoView.m_iPage);
                        accessWindow.Closed += new EventHandler<WindowClosedEventArgs>(accessWindow_Closed);
                        accessWindow.ShowDialog();
                    }
                    //2013-05-13,WangGuojin
                    //refresh current page if pathinfo.ryzd has been changed.
                    if (iChange > 0)
                    {
                        //
                    }
                    */

                }



            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        private void ResultsGrid_LoadingRow(object sender, DataGridRowEventArgs e)
        {
            e.Row.MouseLeftButtonUp += _gridClickManager.HandleClick;

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
                try
                {
                    accessWindow = null;
                    if (e.DialogResult == true)
                    {
                        m_InpatientTemp = Global.InpatientListCurrent;
                        InitPage();
                    }
                }
                catch (Exception ex)
                {
                    YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        private void radGridViewInpatient_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                CP_InpatinetList cp = (CP_InpatinetList)radGridViewInpatient.SelectedItem;
                if (cp == null) return;
                Global.InpatientListCurrent = cp;
                uCPatientBasicInfo1.CurrentPat = Global.InpatientListCurrent;
                if (Global.LogInEmployee != null)
                    Global.InpatientListCurrent.CurOper = Global.LogInEmployee.Zgdm;
                PathStatus status = (PathStatus)Enum.Parse(typeof(PathStatus), cp.Ljzt, true);
                //add by luff 20130306 判断切换病人，若病人状态是出院状态，入径评估按钮不可用
                if (cp.Status == "1503")
                {
                    rabInPath.IsEnabled = false;
                }
                else
                {
                    rabInPath.IsEnabled = (status == PathStatus.New);
                }
                rabOutPath.IsEnabled = (status == PathStatus.InPath);
                btn_OrdersAll.IsEnabled = (cp.Ljzt != "-1");
                //-----------2013-04-15,WangGuojin.---------
                btn_HosReport.IsEnabled = (cp.Ljzt != "-1");
                //------------------------------------------
                rabViewPath.IsEnabled = (Convert.ToInt32(cp.Ljts == "" ? "0" : cp.Ljts) > 0);
                //btn_PathSummary.IsEnabled = !(cp.Ljxh == 0);        //为零则false,否则true
                btn_PathSummary.IsEnabled = (cp.Ljzt != "-1");            //路径状态为退出和完成才可点
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
                if (e.DialogResult == true)
                {
                    m_InpatientTemp = Global.InpatientListCurrent;
                    InitPage();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void rabViewPath_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (radGridViewInpatient.SelectedItem != null)
                {
                    RWPatientPathInfo pathInfoView = new RWPatientPathInfo((CP_InpatinetList)radGridViewInpatient.SelectedItem);
                    pathInfoView.ShowDialog();
                }
                else
                {
                    PublicMethod.RadAlterBox("请选择对应的病人信息!", "提示");
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            // InitPage();
            qcPatientInfoControl1.DoctorID = DoctorID;
        }
        private void btn_ViewPatInfo_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (radGridViewInpatient.SelectedItem != null)
                {
                    CP_InpatinetList currentpat = (CP_InpatinetList)radGridViewInpatient.SelectedItem;
                    RWPatInfo pathInfoView = new RWPatInfo(currentpat);
                    pathInfoView.ShowDialog();
                }
                else
                {
                    PublicMethod.RadAlterBox("请选择对应的病人信息!", "提示");
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btn_ViewPatHistory_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (radGridViewInpatient.SelectedItem != null)
                {
                    CP_InpatinetList currentpat = (CP_InpatinetList)radGridViewInpatient.SelectedItem;
                    RWInPatientHistory patienthistory = new RWInPatientHistory();
                    RWInPatientHistory.Syxh = currentpat.Syxh;
                    patienthistory.ShowDialog();
                }
                else
                {
                    PublicMethod.RadAlterBox("请选择对应的病人信息!", "提示");
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 路径总结按钮（5.10添加）
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_PathSummary_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (radGridViewInpatient.SelectedItem != null)          //判断是否选择
                {
                    CP_InpatinetList currentpat = (CP_InpatinetList)radGridViewInpatient.SelectedItem;
                    RWPathSummary pathSummary = new RWPathSummary(currentpat.Syxh, ConvertMy.ToString(currentpat.Ljxh), currentpat.Ljmc, currentpat.Hzxm, true, 0, currentpat);
                    //pathSummary.checkBoxAll.IsChecked = true;       //全部的
                    pathSummary.ShowDialog();
                }
                else
                {
                    PublicMethod.RadAlterBox("请选择对应的病人信息!", "提示");
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        public void rabInPath2_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                m_InpatientTemp = (CP_InpatinetList)radGridViewInpatient.SelectedItem;
                Global.InpatientListCurrent = m_InpatientTemp;
                if (m_InpatientTemp == null)
                {
                    PublicMethod.RadAlterBox("请选择对应的病人信息!", "提示");
                    return;
                }
                accessWindow = new RWAccessPath2(m_InpatientTemp);
                accessWindow.Closed += new EventHandler<WindowClosedEventArgs>(accessWindow_Closed);
                accessWindow.ShowDialog();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }



        /// <summary>
        /// 绑定DataGrid
        /// </summary>
        /// <param name="pageIndex">当前页码</param>
        void bandDataGrid(int pageIndex, string ljzt)
        {

            this.radBusyIndicator.IsBusy = true;
            YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
            referenceClient.GetInpatientListCompleted +=
                (obj, ea) =>
                {
                    this.radBusyIndicator.IsBusy = false;
                    if (ea.Error == null)
                    {
                        //CP_InpatinetList result = ea.Result;
                        m_InpatientList = ea.Result.ToList();

                        radGridViewInpatient.ItemsSource = m_InpatientList;
                        queryPathInfoControl1.mtxtBedNo.Focus();
                        //qcPatientInfoControl1.InpatientList = m_InpatientList;
                        //qcPatientInfoControl1.UCQcPatientInfoControlBind(result.QuitPath, result.NotIn, result.New, result.InPath, result.DonePath, result.AllCount.ToString());                    //GetTempInpatient();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(ea.Error);
                    }
                };

            int querykind = string.IsNullOrEmpty(DoctorID) ? 3 : 2;
            string hzxm = "";
            string zyhm = "";
            string bedno = "";
            //add by luff 20130227 
            string ksdm = "";
            string brzt = "";
            //referenceClient.GetInpatientListPagingAsync(querykind, strKsdm, strDoctorID, strHzxm, strZyhm, strBedNo, strStartDate, strEndDate);
            //if (queryPathInfoControl1.type == 0) 
            //{ 
            hzxm = queryPathInfoControl1.Hzxm.Replace(" ", "");
            //}if(queryPathInfoControl1.type == 1){
            zyhm = queryPathInfoControl1.Zyhm.Replace(" ", "");
            //}if(queryPathInfoControl1.type == 2){
            bedno = queryPathInfoControl1.BedNo.Replace(" ", "");
            //}

            //若科室代码为空的时候 就用全局的科室代码否则就用子控制的科室代码
            ksdm = queryPathInfoControl1.Ksdm == "" ? Global.LogInEmployee.Ksdm : queryPathInfoControl1.Ksdm;
            brzt = queryPathInfoControl1.Brzt == "" ? "1501" : queryPathInfoControl1.Brzt;
            //add by luff 201303206  当病人状态为出院时，隐藏相关入径按钮
            if (brzt == "1503" || brzt == "1502")
            {
                //this.PathZx.IsEnabled = false;
                this.rabInPath.IsEnabled = false;
            }
            referenceClient.GetInpatientListAsync(querykind, ksdm, m_DoctorID, hzxm, zyhm, bedno, queryPathInfoControl1.StartDate, queryPathInfoControl1.EndDate, brzt, ljzt, 18, pageIndex);
            referenceClient.CloseAsync();
        }

        private void RadDataPager1_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {
            if (datePage == null) return;
            ////if (qcargs == null) return;
            //string ljzt = string.Empty;

            bandDataGrid(e.NewPageIndex + 1, ljzt);

            m_iCurrentPage = e.NewPageIndex + 1;

        }
        #endregion
        #region 委托事件声明
        public delegate void NavigateToPage(object sender, RoutedEventArgs e);
        public event NavigateToPage AfterNavigateToPage;
        #endregion
        #region 属性
        List<CP_InpatinetList> m_InpatientList = null;
        //public RWAccessPath2 accessWindow;
        /// <summary>
        /// 医生ID
        /// </summary>
        private string m_DoctorID;
        public string DoctorID
        {
            get { return m_DoctorID; }
            set { m_DoctorID = value; }
        }
        CP_InpatinetList m_InpatientTemp = new CP_InpatinetList();
        #endregion
        #region 函数

        public void InitPage()
        {
            QcEventArgs qcargs = new QcEventArgs();
            this.radBusyIndicator.IsBusy = true;
            this.queryPathInfoControl1.AfterQuryInfoClicked += new Controls.UCQueryPathInfoControl.QueryInfoClicked(queryPathInfoControl1_AfterQuryInfoClicked);
            this.qcPatientInfoControl1.AfterSumaryInfoClicked += new UCQcPatientInfoControl.SumaryInfoClicked(qcPatientInfoControl1_AfterSumaryInfoClicked);
            GetPageCount(qcargs, ljzt);
            //string ljzt = string.Empty;
            //GetInpatientList(Global.LogInEmployee.Ksdm, m_DoctorID, string.Empty, string.Empty, string.Empty, string.Empty, string.Empty);
            //bandDataGrid(0, ljzt);

        }


        private void GetPageCount(QcEventArgs qcargs, string ljzt)
        {
            itemCount.Clear();
            int count = 0;
            YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
            referenceClient.GetInpatientCountCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        itemCount.Clear();
                        for (int i = 0; i < Convert.ToInt32(e.Result.Split(',')[0]); i++)
                        {

                            itemCount.Add(i);
                        }

                        qcPatientInfoControl1.getCount(e.Result.Split(',')[1], e.Result.Split(',')[2], e.Result.Split(',')[3], e.Result.Split(',')[4], e.Result.Split(',')[5]);
                        //if (qcargs.Hzxm != "" || qcargs.BedNo != "" || qcargs.Zyhm != "")
                        //{
                        //    itemCount.Clear();
                        //    itemCount.Add(1);
                        //}
                        PagedCollectionView pvw = new PagedCollectionView(itemCount);
                        datePage.Source = pvw;
                        //qcPatientInfoControl1.InpatientList = m_InpatientList;
                        //GetTempInpatient();
                        queryPathInfoControl1.mtxtBedNo.Focus();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            int querykind = string.IsNullOrEmpty(DoctorID) ? 3 : 2;
            string hzxm = "";
            string zyhm = "";
            string bedno = "";
            //add by luff 20130227 
            string ksdm = "";
            string brzt = "";//病人状态
            //referenceClient.GetInpatientListPagingAsync(querykind, strKsdm, strDoctorID, strHzxm, strZyhm, strBedNo, strStartDate, strEndDate);
            //if (queryPathInfoControl1.type == 0)
            //{
            hzxm = queryPathInfoControl1.Hzxm.Replace(" ", "");
            //} if (queryPathInfoControl1.type == 1)
            //{
            zyhm = queryPathInfoControl1.Zyhm.Replace(" ", "");
            //} if (queryPathInfoControl1.type == 2)
            //{
            bedno = queryPathInfoControl1.BedNo.Replace(" ", "");
            //}
            //若科室代码为空的时候 就用全局的科室代码否则就用子控制的科室代码
            ksdm = queryPathInfoControl1.Ksdm == "" ? Global.LogInEmployee.Ksdm : queryPathInfoControl1.Ksdm;

            //获得病人状态
            brzt = queryPathInfoControl1.Brzt == "" ? "1501" : queryPathInfoControl1.Brzt;

            referenceClient.GetInpatientCountAsync(querykind, ksdm, m_DoctorID, hzxm, zyhm, bedno, qcargs.StartDate, qcargs.EndDate, brzt, ljzt);
            referenceClient.CloseAsync();

        }


        /// <summary>
        ///  获取病患列表
        ///  add by xjt,20110210
        /// </summary>
        /// <param name="strKsdm">科室代码</param>
        /// <param name="strDoctorID">医生工号</param>
        /// <param name="strHzxm"></param>
        /// <param name="strZyhm"></param>
        /// <param name="strBedNo">床位号</param>
        /// <param name="strStartDate">开始日期</param>
        /// <param name="strEndDate">结束日期</param>
        private void GetInpatientList(String strKsdm, String strDoctorID, String strHzxm, String strZyhm, String strBedNo, String strStartDate, String strEndDate, String strBrzt)
        {
            YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
            referenceClient.GetInpatientListCompleted +=
                (obj, e) =>
                {
                    this.radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        m_InpatientList = e.Result.ToList();
                        radGridViewInpatient.ItemsSource = m_InpatientList;
                        //qcPatientInfoControl1.InpatientList = m_InpatientList;
                        GetTempInpatient();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            int querykind = string.IsNullOrEmpty(strDoctorID) ? 3 : 2;

            referenceClient.GetInpatientListAsync(querykind, strKsdm, strDoctorID, strHzxm, strZyhm, strBedNo, strStartDate, strEndDate, strBrzt, "", 18, 1);
            referenceClient.CloseAsync();
        }

        /// <summary>
        ///  获取病患列表
        ///  add by xjt,20110210
        /// </summary>
        /// <param name="strKsdm">科室代码</param>
        /// <param name="strDoctorID">医生工号</param>
        /// <param name="strHzxm"></param>
        /// <param name="strZyhm"></param>
        /// <param name="strBedNo">床位号</param>
        /// <param name="strStartDate">开始日期</param>
        /// <param name="strEndDate">结束日期</param>
        private void GetInpatientList2(String strKsdm, String strDoctorID, String strHzxm, String strZyhm, String strBedNo, String strStartDate, String strEndDate, String strBrzt)
        {
            YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
            referenceClient.GetInpatientListCompleted +=
                (obj, e) =>
                {
                    this.radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        m_InpatientList = e.Result.ToList();
                        radGridViewInpatient.ItemsSource = m_InpatientList;
                        //qcPatientInfoControl1.InpatientList = m_InpatientList;
                        //GetTempInpatient();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            int querykind = string.IsNullOrEmpty(strDoctorID) ? 3 : 2;

            referenceClient.GetInpatientListAsync(querykind, strKsdm, strDoctorID, strHzxm, strZyhm, strBedNo, strStartDate, strEndDate, strBrzt, "", 18, 0);
            referenceClient.CloseAsync();
        }


        /// <summary>
        /// 成功进入路径后定位病患
        /// </summary>
        private void GetTempInpatient()
        {
            try
            {
                if (string.IsNullOrEmpty(m_InpatientTemp.Syxh))
                    return;
                List<CP_InpatinetList> listInfo = radGridViewInpatient.ItemsSource as List<CP_InpatinetList>;
                CP_InpatinetList clinicalInfo = listInfo.First(delegate(CP_InpatinetList cp)
                {
                    return (cp.Syxh == m_InpatientTemp.Syxh);
                }
                                                              );
                radGridViewInpatient.SelectedItem = clinicalInfo;
                Global.InpatientListCurrent = clinicalInfo;
                if (Global.LogInEmployee != null)
                    Global.InpatientListCurrent.CurOper = Global.LogInEmployee.Zgdm;
                m_InpatientTemp = new CP_InpatinetList();
                OnNavigateToPage(new OpreateEventArgs(true, null));
                //mainpage.ContentFrame.Navigate(new Uri("/Views/PathEnForce.xaml", UriKind.Relative));
            }
            catch (Exception ex)
            {
                radBusyIndicator.IsBusy = false;
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #region 非界面登录时使用
        public void rabInPath2_MockClick()
        {
            if (Global.InpatientListCurrent == null)
            {
                PublicMethod.RadAlterBox("请选择对应的病人信息!", "提示");
                return;
            }
            //accessWindow = new RWAccessPath2(Global.InpatientListCurrent);
            //accessWindow.Closed += new EventHandler<WindowClosedEventArgs>(accessWindow_Closed);
            //accessWindow.ShowDialog();
        }
        #endregion
        /// <summary>
        /// 隐藏部分控件
        /// </summary>
        /// <param name="bState">隐藏部分控件状态，true显示，false隐藏</param>
        public void HideControl(bool bState)
        {
            StackPanelHide.Visibility = bState ? Visibility.Visible : Visibility.Collapsed;
        }

        #endregion

        //定义事件数据源
        public class OpreateEventArgs : RoutedEventArgs
        {
            public bool OpreateType { get; set; }  //事件操作类型：true页面导航，false数据捆绑
            public CP_InpatinetList Datas { get; set; } //数据集
            public OpreateEventArgs(bool otype, CP_InpatinetList cp)
            {
                OpreateType = otype;
                Datas = cp;
            }
        }

        private void datePage_PageIndexChanged(object sender, PageIndexChangedEventArgs e)
        {

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_OrdersAll_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (radGridViewInpatient.SelectedItem != null)          //判断是否选择
                {
                    CP_InpatinetList currentpat = (CP_InpatinetList)radGridViewInpatient.SelectedItem;


                    RWPathOrdersAll PathOrdersAll = new RWPathOrdersAll(currentpat.Syxh, currentpat.Ljmc, currentpat);
                    //RWPathSummary pathSummary = new RWPathSummary(currentpat.Syxh, ConvertMy.ToString(currentpat.Ljxh), currentpat.Ljmc, currentpat.Hzxm, true, 0, currentpat);
                    //pathSummary.checkBoxAll.IsChecked = true;       //全部的
                    PathOrdersAll.ShowDialog();
                }
                else
                {
                    PublicMethod.RadAlterBox("请选择对应的病人信息!", "提示");
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        //2013-04-15,WangGuojin add it. in order to report sickness information in hospital       
        private void btn_HosReport_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (radGridViewInpatient.SelectedItem != null)          //判断是否选择
                {
                    CP_InpatinetList currentpat = (CP_InpatinetList)radGridViewInpatient.SelectedItem;

                    RWPathHosReport pathHosReport = new RWPathHosReport(currentpat.Syxh, currentpat.Ljmc, currentpat);

                    pathHosReport.ShowDialog();
                }
                else
                {
                    PublicMethod.RadAlterBox("请选择对应的病人信息!", "提示");
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnEnable_Click(object sender, RoutedEventArgs e)
        {

            if (radGridViewInpatient.SelectedItem == null)
            {
                PublicMethod.RadAlterBox("请选择对应的病人信息!", "提示");
                return;
            }
            AfterNavigateToPage(this, new OpreateEventArgs(true, Global.InpatientListCurrent));


        }

        /// <summary>
        /// 查看选择项的执行路径事件 add by luff 2012-08-08
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void PathZx_Click(object sender, RoutedEventArgs e)
        {
            if (radGridViewInpatient.SelectedItem == null)
            {
                PublicMethod.RadAlterBox("请选择对应的病人信息!", "提示");
                return;
            }
            //调用行双击事件
            radGridViewInpatient_RowActivated(null, null);
            //调用委托事件
            //AfterNavigateToPage(this, new OpreateEventArgs(true, Global.InpatientListCurrent));
        }


        private void txtLjzt_Loaded(object sender, RoutedEventArgs e)
        {
            TextBox txt = (TextBox)sender;
            if (txt.Text != "")
            {
                if (txt.Text.Trim() == "-1")
                {
                    txt.Background = ConvertColor.GetColorBrushFromHx16("25A0DA");
                    //txt.Foreground = ConvertColor.GetColorBrushFromHx16("25A0DA");
                    txt.Text = "";
                    //txt.Foreground = new SolidColorBrush(Colors.Transparent);

                }
                else if (txt.Text.Trim() == "1")
                {
                    txt.Background = ConvertColor.GetColorBrushFromHx16("CC6752");
                    txt.Foreground = ConvertColor.GetColorBrushFromHx16("CC6752");
                    txt.Text = "";
                }
                else if (txt.Text.Trim() == "2")
                {
                    txt.Background = new SolidColorBrush(Colors.Red);
                    //txt.Foreground = new SolidColorBrush(Colors.Red);
                    txt.Text = "";
                }
                else if (txt.Text.Trim() == "3")
                {
                    txt.Background = new SolidColorBrush(Colors.Green);
                    //txt.Foreground = ConvertColor.GetColorBrushFromHx16("FF53B119");
                    txt.Text = "";
                }
                else if (txt.Text.Trim() == "4")
                {
                    txt.Background = ConvertColor.GetColorBrushFromHx16("FFA112AD");
                    txt.Foreground = ConvertColor.GetColorBrushFromHx16("FFA112AD");
                    txt.Text = "";
                }
                else
                {
                    txt.Background = ConvertColor.GetColorBrushFromHx16("FFFFFF");
                    txt.Foreground = ConvertColor.GetColorBrushFromHx16("FFFFFF");
                    txt.Text = "";
                }
            }
        }


    }
}
