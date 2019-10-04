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
using YidanEHRApplication.Helpers;
using YidanEHRApplication.DataService;
using System.ComponentModel;
using System.Windows.Data;
using System.Xml.Linq;
using System.IO;
using System.Collections.ObjectModel;
using YidanEHRApplication.Models;
using YidanEHRApplication.Views.ChildWindows;
using System.ServiceModel;
using Telerik.Windows.Controls.GridView;
using Telerik.Windows;
using System.Collections;
using Telerik.Windows.Controls.DragDrop;
using Telerik.Windows.Controls.TreeView;
using System.Collections.Specialized;
using YidanEHRApplication.WorkFlow.Designer;
using YidanEHRApplication.WorkFlow;
using YidanSoft.Tool;
using System.Windows.Browser;
using YidanEHRApplication;
namespace YidanEHRApplication.Views
{
    public partial class PathEnForceBack : Page
    {
        #region 事件

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if (!IsSelectPatient(false))
                return;
            IntiCtor();
        }// 当用户导航到此页面时执行。
        public PathEnForceBack()
        {
            InitializeComponent();
            if (!IsSelectPatient(false))
                return;
            IntiCtor();
        }//构造
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (IsSelectPatient(true))
                {
                    InitPage();
                }
                storyPopUp.Begin();
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
                #region 保存
                if (!(radGridViewOrderList.Items.Count == 0 && m_DelOrder.Count == 0))
                {
                    if (radGridViewOrderList.SelectedItems.Count == 0 && m_DelOrder.Count == 0)
                    {
                        PublicMethod.RadAlterBox("请选择要保存的医嘱", m_StrTitle);
                        return;
                    }
                }


                CheckMasterDrugs_Save();
                #endregion
            }
            catch (Exception ex)
            {
                radBusyIndicator.IsBusy = false;
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }/// 保存
        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String strWaring = String.Empty;
                if (CheckUIOrder(out strWaring))
                {
                    if (Next(null))
                    {
                        UpdateXmlAfterNext();
                    }
                }
                else
                {
                    DialogParameters parameters = new DialogParameters();
                    parameters.Closed = OnExceptionNextReason;//***close处理***
                    PublicMethod.RadQueryBox(parameters, strWaring, m_StrTitle);
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }/// 下一步
        private void btnComplete_Click(object sender, RoutedEventArgs e)//完成事件
        {
            try
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.UpdatePathStatusDoneCompleted += (obj, ea) =>
                {
                    if (ea.Error == null)
                    {
                        Global.InpatientListCurrent.Ljzt = Convert.ToString((Int32)PathStatus.DonePath);
                        InitPage();
                        PublicMethod.RadAlterBox("路径完成", m_StrTitle);
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(ea.Error);
                    }
                };
                client.UpdatePathStatusDoneAsync(Global.InpatientListCurrent,
                m_WorkFlow.Activitys.CurrentViewActivity.CurrentViewActiveChildren.ActivityChildrenID,
                       m_ListUnEnforceReason, null, null, m_WorkFlow.Activitys.CurrentActivity.UniqueID,"0");
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnShowDetail_Click(object sender, RoutedEventArgs e)//查看详细信息
        {
            try
            {
                RWPatientInfo detailInfo = new RWPatientInfo();
                detailInfo.Show();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnAdviceList_Click(object sender, RoutedEventArgs e)//医嘱列表
        {
            try
            {
                RWAdviceList adviceList = new RWAdviceList(Global.InpatientListCurrent.Syxh);
                adviceList.ShowDialog();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnViewPre_Click(object sender, RoutedEventArgs e)//查看上一步
        {
            try
            {
                m_WorkFlow.Activitys.PreView();
                Activitys_WorkFlow_ActivitySelectChanged(m_WorkFlow.Activitys.CurrentViewActivity);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnViewNext_Click(object sender, RoutedEventArgs e)//查看下一步
        {
            try
            {
                m_WorkFlow.Activitys.NextView();
                Activitys_WorkFlow_ActivitySelectChanged(m_WorkFlow.Activitys.CurrentViewActivity);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        //private void hlkButtonShowPaht_MouseEnter(object sender, MouseEventArgs e)//显示执行路径流程图
        //{
        //    this.gridWorkFlowShow.Height = 200;
        //    //this.gridWorkFlowShow.Visibility = System.Windows.Visibility.Visible;
        //}
        private void gridWorkFlowShow_MouseLeave(object sender, MouseEventArgs e)// 隐藏执行路径流程图
        {
            //gridWorkFlowShow.Visibility = System.Windows.Visibility.Collapsed;
            //this.gridWorkFlowShow.Height = 0;
        }
        private void btnLeadIn_Click(object sender, RoutedEventArgs e)//引入路径
        {
            try
            {
                RWLeadInPath inPath = new RWLeadInPath(Global.InpatientListCurrent, m_WorkFlow.WorkFlowXml);
                inPath.WindowState = WindowState.Maximized;
                inPath.Closed += new EventHandler<WindowClosedEventArgs>(inPath_Closed);
                inPath.ShowDialog();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnQuit_Click(object sender, RoutedEventArgs e)//退出路径
        {
            try
            {
                RWQuitPath path = new RWQuitPath(Global.InpatientListCurrent, m_WorkFlow.Activitys.CurrentViewActivity.CurrentViewActiveChildren.ActivityChildrenID);
                path.Closed += new EventHandler<WindowClosedEventArgs>(path_Closed);
                path.ShowDialog();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void chk_Checked(object sender, RoutedEventArgs e)//套餐类型选择
        {
            try
            {
                //if (lstAdviceSuit == null) return;
                //lstAdviceSuit.ItemsSource = null;
                // 指示当前列表绑定的类型 科室=2091，个人=2093 使用范围(CP_DataCategory.Mxbh, Lbbh = 29)
                Syfw = ((RadioButton)sender).CommandParameter.ToString();
                //if (Syfw == "2903")//个人医嘱
                //{
                //    lstAdviceSuit.ItemsSource = m_CP_AdviceSuitPersonal;
                //}
                //if (Syfw == "2901")//科室医嘱
                //{
                //    lstAdviceSuit.ItemsSource = m_CP_AdviceSuitDepartment;
                //}
                GetCP_AdviceSuitCategory();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void radGridViewOrderList_SelectionChanged(object sender, SelectionChangeEventArgs e)//GridView选择改变事件
        {
            try
            {
                radGridViewOrderList.SelectionChanged -= new EventHandler<SelectionChangeEventArgs>(radGridViewOrderList_SelectionChanged);
                int selectItemsCount = e.AddedItems.Count;
                for (int i = 0; i < selectItemsCount; i++)
                {
                    CP_DoctorOrder order = e.AddedItems[i] as CP_DoctorOrder;
                    if (!(order.Yzzt == (decimal)OrderStatus.OrderInptut || order.Yzzt == 0))
                        radGridViewOrderList.SelectedItems.Remove(order);
                }
                radGridViewOrderList.SelectionChanged += new EventHandler<SelectionChangeEventArgs>(radGridViewOrderList_SelectionChanged);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void radioButton_Checked(object sender, RoutedEventArgs e)//选中的类型，药品,	检验检查,营养膳食,观察,活动,护理及宣教,手术
        {
            try
            {
                RadioButton radionButton = sender as RadioButton;
                OrderPanelBarCategory radioButtonTag = (OrderPanelBarCategory)(int.Parse(radionButton.Tag.ToString()));
                SetGridVisible(radioButtonTag);
                switch (radioButtonTag)
                {
                    #region 药品
                    case OrderPanelBarCategory.Drug:
                        drugOrderControl.NewAdviceGroupDetail();
                        break;
                    #endregion
                    #region 手术
                    case OrderPanelBarCategory.Oper:
                        break;
                    #endregion
                    #region 检验检查
                    case OrderPanelBarCategory.RisLis:
                        risLisOrderControl.NewAdviceGroupDetail();
                        break;
                    #endregion
                    #region 营养膳食
                    case OrderPanelBarCategory.Meal:
                        foodOrderControl.NewAdviceGroupDetail();
                        break;
                    #endregion
                    #region 观察
                    case OrderPanelBarCategory.Observation:
                        observationOrderControl.NewAdviceGroupDetail();
                        break;
                    #endregion
                    #region 活动
                    case OrderPanelBarCategory.Activity:
                        activityOrderControl.NewAdviceGroupDetail();
                        break;
                    #endregion
                    #region 护理及宣教
                    case OrderPanelBarCategory.Care:
                        careOrderControl.NewAdviceGroupDetail();
                        break;
                    #endregion
                    default:
                        break;
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void ContainerEdit_LoadCompleted()//流程图加载完毕事件
        {
            try
            {
                InsertAndSelectCP_MedicalTreatmentWarmGroupByTxlx();
                if (IsWorkFlowContainShow) Expander_Expanded(null, null);
                else Expander_Collapsed(null, null);
            }
            catch (Exception ex)
            {
                EnableButtonState(false);
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void Activitys_WorkFlow_ActivitySelectChanged(Activity a)
        {
            //首先判断结点类型
            //分四种情况:下一步，查看分三钟:直接上一步，循环的二种
            try
            {
                InsertAndSelectCP_MedicalTreatmentWarmGroupByTxlx();

                m_DelOrder.Clear();
                m_NewOrder.Clear();
                m_UnDoOrder.Clear();
                radBusyIndicator.IsBusy = true;
                SetButtonEnable(a);
                if (a.Type == ActivityType.AUTOMATION) //循环结点
                {
                    if (a.CurrentElementState == WorkFlow.ElementState.Pre)
                    {
                        //去医嘱表LOAD    
                        GetActivityOrder(a, false);
                    }
                    else if (a.CurrentElementState == WorkFlow.ElementState.Now)
                    {
                        //to do LOAD 最后一个子结点的初始化医令，若点击了查看两BUTTON，则跟根据类型判断是去医嘱表LOAD，还是组套表LOAD
                        if (a.CurrentViewActiveChildren.CurrentElementState == WorkFlow.ElementState.Now
                            && a.CurrentViewActiveChildren.EnForceTime == String.Empty)
                        {
                            GetActivityOrder(a, true);
                        }
                        else
                        {
                            GetActivityOrder(a, false);
                        }
                    }
                    else if (a.CurrentElementState == WorkFlow.ElementState.Next)
                    {
                        //下一步button不可用，只能看医嘱 
                        GetActivityOrder(a, true);
                    }
                    else
                    {
                        GetActivityOrder(a, true);
                    }
                }
                else
                {
                    if (a.CurrentElementState == WorkFlow.ElementState.Pre)
                    {
                        //下一步button可用，去医嘱表LOAD 
                        GetActivityOrder(a, false);
                    }
                    else if (a.CurrentElementState == WorkFlow.ElementState.Now)
                    {
                        //下一步button可用，去组套表LOAD 
                        if (a.CurrentViewActiveChildren.EnForceTime == String.Empty)
                            GetActivityOrder(a, true);
                        else
                            GetActivityOrder(a, false);
                    }
                    else if (a.CurrentElementState == WorkFlow.ElementState.Next)
                    {
                        //下一步button不可用，只能看医嘱 
                        GetActivityOrder(a, true);
                    }
                    else
                    {
                        GetActivityOrder(a, true);
                    }
                }
            }
            catch (Exception ex)
            {
                radBusyIndicator.IsBusy = false;
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }//needfix
        private void BtnTempOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                CP_InpatinetList currentpat = Global.InpatientListCurrent;
                //RWPathEnforcePrint print = new RWPathEnforcePrint(currentpat.Hzxm, currentpat.Bed, currentpat.CyksName, currentpat.Zyhm, currentpat.Syxh, currentpat.Ljxh.ToString());
                //PathEnForceTempOrderPrint print = new PathEnForceTempOrderPrint();

                //print.m_Hzxm = currentpat.Hzxm;
                //print.m_Bed = currentpat.Bed;
                //print.m_CyksName = currentpat.CyksName;
                //print.m_Zyhm = currentpat.Zyhm;
                //print.m_Syxh = currentpat.Syxh;
                //print.m_Ljxh = ConvertMy.ToString(currentpat.Ljxh);

                //print.ShowDialog();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }//打印当天临时医嘱（silverlight打印）
        private void BtnLongOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                CP_InpatinetList currentpat = Global.InpatientListCurrent;
                //RWPathEnforcePrint print = new RWPathEnforcePrint(currentpat.Hzxm, currentpat.Bed, currentpat.CyksName, currentpat.Zyhm, currentpat.Syxh, currentpat.Ljxh.ToString());
                //PathEnforceLongOrderPrint print = new PathEnforceLongOrderPrint();

                //print.m_Hzxm = currentpat.Hzxm;
                //print.m_Bed = currentpat.Bed;
                //print.m_CyksName = currentpat.CyksName;
                //print.m_Zyhm = currentpat.Zyhm;
                //print.m_Syxh = currentpat.Syxh;
                //print.m_Ljxh = ConvertMy.ToString(currentpat.Ljxh);

                //print.ShowDialog();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }//打印当天长期医嘱（silverlight打印）
        private void btnLongOrderPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Global.InpatientListCurrent != null)          //判断是否选择
                {
                    CP_InpatinetList currentpat = (CP_InpatinetList)Global.InpatientListCurrent;
                    HtmlPage.Window.Eval("window.open(\"/" + Virtual.Trim() + "/View/PathEnforceLongOrderPrint.aspx?Hzxm=" + currentpat.Hzxm + "&Bed=" + currentpat.Bed + "&CyksName=" + currentpat.CyksName + "&Zyhm=" + currentpat.Zyhm + "&Syxh=" + currentpat.Syxh + "&Ljxh=" + currentpat.Ljxh + "\")");
                }
                else
                {
                    PublicMethod.RadAlterBox("请关闭当前窗口，重新打开!", "提示");
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }//打印当天长期医嘱（网页打印）
        private void btnTempOrderPrint_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Global.InpatientListCurrent != null)          //判断是否选择
                {
                    CP_InpatinetList currentpat = (CP_InpatinetList)Global.InpatientListCurrent;
                    HtmlPage.Window.Eval("window.open(\"/" + Virtual.Trim() + "/View/PathEnforceTempOrderPrint.aspx?Hzxm=" + currentpat.Hzxm + "&Bed=" + currentpat.Bed + "&CyksName=" + currentpat.CyksName + "&Zyhm=" + currentpat.Zyhm + "&Syxh=" + currentpat.Syxh + "&Ljxh=" + currentpat.Ljxh + "\")");
                }
                else
                {
                    PublicMethod.RadAlterBox("请关闭当前窗口，重新打开!", "提示");
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }//打印当天临时医嘱（网页打印）
        private void btn_PathSummary_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Global.InpatientListCurrent != null)          //判断是否选择
                {
                    CP_InpatinetList currentpat = (CP_InpatinetList)Global.InpatientListCurrent;
                    RWPathSummary pathSummary = new RWPathSummary(currentpat.Syxh, ConvertMy.ToString(currentpat.Ljxh), currentpat.Ljmc, currentpat.Hzxm, false, 0, currentpat);
                    //pathSummary.ComboBoxVariant.IsEnabled = false;       //非全部

                    pathSummary.ShowDialog();
                }
                else
                {
                    PublicMethod.RadAlterBox("请关闭当前窗口，重新打开!", "提示");
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }//路径总结
        #region 弹出提示框事件
        private void btnShow_Click(object sender, RoutedEventArgs e)//向上弹出提示框
        {
            storyPopUp.Begin();
        }
        private void btnHidden_Click(object sender, RoutedEventArgs e)//向下缩小提示框
        {
            storyPopUp.Stop();
            storyPopDown.Begin();
        }
        private void grdWarmContent_RowActivated(object sender, RowEventArgs e)//弹出明细提示窗口事件
        {
            try
            {
                RWMedicalTreatmentWarm RWMedicalTreatmentWarmTemp = new RWMedicalTreatmentWarm(m_WorkFlow.Activitys.CurrentActivity.UniqueID, Global.InpatientListCurrent.Ljdm, Global.InpatientListCurrent.Syxh.ToString(), Global.InpatientListCurrent.Ljxh.ToString());
                RWMedicalTreatmentWarmTemp.ShowDialog();
                RWMedicalTreatmentWarmTemp.Closed += (d, ea) => { InsertAndSelectCP_MedicalTreatmentWarmGroupByTxlx(); };
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region 弹出流程图事件
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            this.gridWorkFlowShow.Height = 200;
            //cnvDragTitle.Visibility = Visibility.Visible; ;
            expShowImage.Header = "隐藏路径";
            LayoutRoot.RowDefinitions[1].Height = new GridLength(200);
            IsWorkFlowContainShow = true;
        }
        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            this.gridWorkFlowShow.Height = 0;
            //cnvDragTitle.Visibility = Visibility.Collapsed; ;
            expShowImage.Header = "显示路径";
            LayoutRoot.RowDefinitions[1].Height = new GridLength(0);
            IsWorkFlowContainShow = false;
        }
        #region 可拖动流程图（现在不起作用）
        bool isMouseCaptured;
        double mouseVerticalPosition;
        double mouseHorizontalPosition;
        private void cnvDrag_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            isMouseCaptured = true;
            cnvDrag.CaptureMouse();
            //cnvDragTitle.CaptureMouse();
            cnvDrag.Cursor = Cursors.Hand;
            mouseVerticalPosition = e.GetPosition(null).Y;
            mouseHorizontalPosition = e.GetPosition(null).X;
        }
        private void cnvDrag_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
        }
        private void cnvDrag_MouseMove(object sender, MouseEventArgs e)
        {
            if (isMouseCaptured)
            {
                if (e.GetPosition(null).Y < 0 || e.GetPosition(null).X < 0) return;
                double deltaV = e.GetPosition(null).Y - mouseVerticalPosition;
                double deltaH = e.GetPosition(null).X - mouseHorizontalPosition;
                double newTop = deltaV + (double)cnvDrag.GetValue(Canvas.TopProperty);
                double newLeft = deltaH + (double)cnvDrag.GetValue(Canvas.LeftProperty);
                //double newTopTitle = deltaV + (double)cnvDragTitle.GetValue(Canvas.TopProperty);
                //double newLeftTitle = deltaH + (double)cnvDragTitle.GetValue(Canvas.LeftProperty);
                // Set new position of object.
                cnvDrag.SetValue(Canvas.TopProperty, newTop);
                cnvDrag.SetValue(Canvas.LeftProperty, newLeft);
                //cnvDragTitle.SetValue(Canvas.TopProperty, newTopTitle);
                //cnvDragTitle.SetValue(Canvas.LeftProperty, newLeftTitle);
                // Update position global variables.
                mouseVerticalPosition = e.GetPosition(null).Y;
                mouseHorizontalPosition = e.GetPosition(null).X;
            }
        }
        private void cnvDrag_MouseLeftButtonUp_1(object sender, MouseButtonEventArgs e)
        {
            isMouseCaptured = false;
            cnvDrag.ReleaseMouseCapture();
            // cnvDragTitle.ReleaseMouseCapture();
            // mouseVerticalPosition = -1;
            // mouseHorizontalPosition = -1;
            cnvDrag.Cursor = null;
        }
        private void cnvDrag_MouseLeftButtonDown_1(object sender, MouseButtonEventArgs e)
        {
            isMouseCaptured = true;
            cnvDrag.CaptureMouse();
            //cnvDragTitle.CaptureMouse();
            cnvDrag.Cursor = Cursors.Hand;
            mouseVerticalPosition = e.GetPosition(null).Y;
            mouseHorizontalPosition = e.GetPosition(null).X;
        }
        private void cnvDrag_MouseMove_1(object sender, MouseEventArgs e)
        {
            if (isMouseCaptured)
            {
                if (e.GetPosition(null).Y < 0 || e.GetPosition(null).X < 0) return;
                double deltaV = e.GetPosition(null).Y - mouseVerticalPosition;
                double deltaH = e.GetPosition(null).X - mouseHorizontalPosition;
                double newTop = deltaV + (double)cnvDrag.GetValue(Canvas.TopProperty);
                double newLeft = deltaH + (double)cnvDrag.GetValue(Canvas.LeftProperty);
                //double newTopTitle = deltaV + (double)cnvDragTitle.GetValue(Canvas.TopProperty);
                //double newLeftTitle = deltaH + (double)cnvDragTitle.GetValue(Canvas.LeftProperty);
                // Set new position of object.
                cnvDrag.SetValue(Canvas.TopProperty, newTop);
                cnvDrag.SetValue(Canvas.LeftProperty, newLeft);
                //cnvDragTitle.SetValue(Canvas.TopProperty, newTopTitle);
                //cnvDragTitle.SetValue(Canvas.LeftProperty, newLeftTitle);
                // Update position global variables.
                mouseVerticalPosition = e.GetPosition(null).Y;
                mouseHorizontalPosition = e.GetPosition(null).X;
            }
        }
        #endregion
        #endregion
        #endregion
        #region 变量
        /// <summary>
        /// 设置或获取一个值该值指示流程图空间是否显示
        /// </summary>
        private Boolean IsWorkFlowContainShow = false;
        /// <summary>
        /// 医嘱开始时间是否改变
        /// </summary>
        private Boolean m_IsTimeChanged = false;
        /// <summary>
        /// 全部活动结点
        /// </summary>
        private List<WorkFlowActivity> m_ListWorkFlowActivity = new List<WorkFlowActivity>();
        /// <summary>
        /// 全部RULE
        /// </summary>
        private List<WorkFlowRule> m_ListWorkFlowRule = new List<WorkFlowRule>();
        /// <summary>
        /// 必选未执行原因
        /// </summary>
        private ObservableCollection<CP_VariantRecords> m_ListUnEnforceReason = new ObservableCollection<CP_VariantRecords>();
        /// <summary>
        /// 新增医嘱原加
        /// </summary>
        private ObservableCollection<CP_VariantRecords> m_ListUnNewReason = new ObservableCollection<CP_VariantRecords>();
        /// <summary>
        /// 其它（表单）
        /// </summary>   
        private ObservableCollection<CP_VariantRecords> m_ListUnOtherReason = new ObservableCollection<CP_VariantRecords>();
        private const string m_StrTitle = "路径执行";
        /// <summary>
        /// 必选 未执行医嘱
        /// </summary>
        private List<CP_DoctorOrder> m_UnDoOrder = new List<CP_DoctorOrder>();
        /// <summary>
        /// 新增 医嘱
        /// </summary>
        private List<CP_DoctorOrder> m_NewOrder = new List<CP_DoctorOrder>();
        /// <summary>
        /// 删除 的医嘱
        /// </summary>
        private ObservableCollection<CP_DoctorOrder> m_DelOrder = new ObservableCollection<CP_DoctorOrder>();
        /// <summary>
        /// 添加的医嘱
        /// </summary>
        private List<CP_DoctorOrder> m_AddOrder = new List<CP_DoctorOrder>();
        /// <summary>
        /// 是否开启LOCK
        /// </summary>
        private Boolean m_IsLock = false;
        /// <summary>
        /// 是否开启引入
        /// </summary>
        private Boolean m_IsLead = false;
        /// <summary>
        /// 是否引入了新的路径
        /// </summary>
        private Boolean m_IsLeadIn = false;
        /// <summary>
        /// 当前显示的执行路径
        /// </summary>
        private WorkFlow.WorkFlow m_WorkFlow = new WorkFlow.WorkFlow();
        /// <summary>
        /// 新病人进入路径是否成功
        /// </summary>
        private bool m_IsInpath = true;
        /// <summary>
        /// 个人医嘱套餐
        /// </summary>
        private List<CP_AdviceSuit> m_CP_AdviceSuitPersonal = new List<CP_AdviceSuit>();
        /// <summary>
        /// 科室套餐
        /// </summary>
        private List<CP_AdviceSuit> m_CP_AdviceSuitDepartment = new List<CP_AdviceSuit>();

        /// <summary>
        /// 指示当前列表绑定的类型 科室=2091，个人=2093 使用范围(CP_DataCategory.Mxbh, Lbbh = 29)
        /// </summary>
        String Syfw = "2901";//科室医嘱套餐

        static List<CP_MasterDrugs> cP_MasterDrugs = new List<CP_MasterDrugs>();
        private String Virtual = ConvertMy.ToString(App.Current.Resources["IsVirtual"]);        //虚拟目录名
        #endregion
        #region 函数

        #region 在导航里调用的函数
        /// <summary>
        /// 构造函数里必须调用函数集合
        /// </summary>
        private void IntiCtor()
        {
            InitOrderControl();
            GetCP_AdviceSuitCategory();
            InitWorkFlow(Global.InpatientListCurrent.EnForceWorkFlowXml);
            //GetAdviceSuitDepartment();//初始化部门医嘱套餐
            //GetAdviceSuitPersonal();//初始化个人医嘱套餐
            InitDragDrop();//初始化控件拖拉
        }
        /// <summary>
        /// 医嘱输入区域
        /// </summary>
        private void InitOrderControl()
        {
            this.radioDrug.IsChecked = true;
            #region 医嘱输入控件初始化
            InitDrugControl();
            InitRisLisControl();
            InitMealControl();
            InitObservationControl();
            InitActivityControl();
            InitCareControl();
            #endregion
        }
        /// <summary>
        /// 初始化显示工作流图,并实例化m_WorkFlow
        /// </summary>
        private void InitWorkFlow(String strXml)
        {
            try
            {
                this.gridWorkFlowShow.Children.Clear();
                m_WorkFlow = new WorkFlow.WorkFlow();
                ContainerShow containerEdit = new ContainerShow();
                containerEdit.IsShowAll = false;
                containerEdit.WorkFlowUrlName = Global.InpatientListCurrent.Ljmc;
                containerEdit.WorkFlowUrlID = Global.InpatientListCurrent.Ljdm;
                containerEdit.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                containerEdit.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                containerEdit.WorkFlowXML = strXml;
                containerEdit.Width = Browser.ClientWidth - 20;
                this.gridWorkFlowShow.Height = 0;
                this.gridWorkFlowShow.Children.Add(containerEdit);
                gridWorkFlowShow.MouseLeave += new MouseEventHandler(gridWorkFlowShow_MouseLeave);
                m_WorkFlow.ContainerEdit = containerEdit;
                m_WorkFlow.ContainerEdit.LoadCompleted += new LoadCompletedHandler(ContainerEdit_LoadCompleted);
                //m_WorkFlow.ContainerEdit.Loaded += new RoutedEventHandler(ContainerEdit_Loaded);
                m_WorkFlow.Activitys.WorkFlow_ActivitySelectChanged += new WorkFlow.WorkFlow_ActivetySelectedDelegateEventHandler(Activitys_WorkFlow_ActivitySelectChanged);
                //  m_WorkFlow.Activitys.WorkFlowActivitys_AfterNext += () => { UpdateXmlAfterNext(); };//laolaowhn 2010-2-21 New
            }
            catch (Exception ex)
            {
                EnableButtonState(false);
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        /// <summary>
        /// 判断是否选中病患
        /// </summary>
        /// <returns></returns>
        private Boolean IsSelectPatient(Boolean isNeedAlert)
        {
            if (Global.InpatientListCurrent == null)
            {
                m_IsInpath = false;
                PublicMethod.RadAlterBox("请选择在路径中的病患", m_StrTitle);
            }
            //未进入路径的患者要先评估后刷新
            else if (string.IsNullOrEmpty(Global.InpatientListCurrent.Ljdm))
            {
                m_IsInpath = false;
                //AccessNewPath();      //zm    8.4 注释
            }
            if (!m_IsInpath)
            {
                if (Global.InpatientListCurrent == null || string.IsNullOrEmpty(Global.InpatientListCurrent.Ljdm))
                {
                    
                    EnableButtonState(false);
                    this.btnShowDetail.IsEnabled = false;
                    this.btnAdviceList.IsEnabled = false;
                    this.radioButtonGrid.Visibility = System.Windows.Visibility.Collapsed;
                    this.drugGrid.Visibility = System.Windows.Visibility.Collapsed;
                    if (isNeedAlert)
                        PublicMethod.RadAlterBox("请选择在路径中的病患", m_StrTitle);
                }
            }
            return m_IsInpath;
        }
        /// <summary>
        /// 初始化页面
        /// </summary>
        private void InitPage()
        {
            IntiPatinetInfo();
            InitSysConfig();
            InitButtoState();
            GetMasterDrugs();

            //InitEnfroceInfo(); 
            //InsertAndSelectCP_MedicalTreatmentWarmGroupByTxlx();
        }
        #region   入院评估
        private void accessWindow_Closed(object sender, WindowClosedEventArgs e)
        {
            try
            {
                if (e.DialogResult == false)
                {
                    m_IsInpath = false;
                    PublicMethod.RadAlterBox("路径评估失败", "路径评估");
                }
                else
                {
                    //刷新当前患者
                    YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                    referenceClient.GetInpatientInfoCompleted +=
                          (obj, ea) =>
                          {
                              if (ea.Error == null)
                              {
                                  Global.InpatientListCurrent = ea.Result;
                                  m_IsInpath = true;
                                  InitPage();
                              }
                          };
                    referenceClient.GetInpatientInfoAsync(Global.InpatientListCurrent.Hissyxh);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region 在loaded事件中调用的methods,to do 完成后将成员变量移到members define 区
        private const String m_PatientInfoInfo = "病人姓名：{0}  病历号:{1}  性别:{2}  年龄:{3}  出生日期:{4}  管床医师:{5}";
        private const String m_PathInfoInfo = "病人诊断：{0}  路径名称:{1}  当前产生费用:{2}  住院天数:第{3}天  当前步骤:{4}";
        /// <summary>
        /// 初始化基本信息
        /// </summary>
        private void IntiPatinetInfo()
        {
            try
            {
                m_IsLeadIn = false;
                this.textBlockPatient.Text = String.Format(m_PatientInfoInfo, Global.InpatientListCurrent.Hzxm, Global.InpatientListCurrent.Zyhm, Global.InpatientListCurrent.Brxb,
                                                                 Global.InpatientListCurrent.Xsnl, Global.InpatientListCurrent.Csrq, Global.InpatientListCurrent.Zyys);
                System.TimeSpan timeSpan = DateTime.Now.Subtract(Convert.ToDateTime(Global.InpatientListCurrent.Ryrq));
                this.textBlockPath.Text = String.Format(m_PathInfoInfo, Global.InpatientListCurrent.Ryzd, Global.InpatientListCurrent.Ljmc,
                                                                 Global.InpatientListCurrent.Ljmc, timeSpan.Days.ToString(), Global.InpatientListCurrent.Ljts);
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private const String m_LockInfo = "病患被 {0} 在 IP: {1} 锁定";
        /// <summary>
        /// 初始化一些变量
        /// </summary>
        private void InitSysConfig()
        {
            m_IsLead = Boolean.Parse(App.Current.Resources["IsLead"].ToString());
            m_IsLock = Boolean.Parse(App.Current.Resources["IsLock"].ToString());
            //to do Lock逻辑
            if (m_IsLock)
            {
                textBlockLock.Visibility = Visibility.Visible;
                textBlockLock.Text = String.Format(m_LockInfo, Global.LogInEmployee.Zgdm + "_" + Global.LogInEmployee.Name, App.Current.Resources["IpAddress"].ToString());
            }
            //to do true的地方要判断是否有足够权限，e.g 主任医师
            if (m_IsLead && true)
            {
                this.btnLeadIn.Visibility = System.Windows.Visibility.Visible;
            }
        }
        /// <summary>
        /// 根据当前选中病患在路径中的状态,控制button可用性
        /// </summary>
        private void InitButtoState()
        {
            try
            {
                if ((PathStatus)(int.Parse(Global.InpatientListCurrent.Ljzt)) == PathStatus.QuitPath
                    || (PathStatus)(int.Parse(Global.InpatientListCurrent.Ljzt)) == PathStatus.DonePath)
                {
                    EnableButtonState(false);
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region 初始化病患基本信息和当前步骤基本医嘱信息

        /// <summary> 
        /// 根据当前选中的结点的
        /// 当前子结点的ActivityChildrenID
        ///  LOAD对应的医嘱信息
        /// </summary>
        /// <param name="activity">选中节点</param>
        /// <param name="isInit">去组套表(isInit=True),医嘱表(isInit=False)</param>
        private void GetActivityOrder(Activity activity, Boolean isInit)
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            if (isInit)
            {
                client.GetPathInitOrderCompleted +=
                (obj, e) =>
                {
                    radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        ObservableCollection<CP_DoctorOrder> listOrder = e.Result;
                        foreach (CP_DoctorOrder order in listOrder)
                        {
                            order.Lrysdm = Global.LogInEmployee.Zgdm;
                        }
                        radGridViewOrderList.ItemsSource = listOrder;
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
                client.GetPathInitOrderAsync(activity.CurrentViewActiveChildren.ActivityUniqueID, Global.InpatientListCurrent.Syxh, Global.LogInEmployee, Global.InpatientListCurrent.Ljdm);
            }
            else
            {
                client.GetPathEnforcedOrderCompleted +=
               (obj, e) =>
               {
                   radBusyIndicator.IsBusy = false;
                   if (e.Error == null)
                       this.radGridViewOrderList.ItemsSource = e.Result;
                   else
                   {
                       PublicMethod.RadWaringBox(e.Error);
                   }
               };
                client.GetPathEnforcedOrderAsync(activity.CurrentViewActiveChildren.ActivityChildrenID, Global.InpatientListCurrent.Syxh, Global.InpatientListCurrent.Ljdm);
            }
            client.CloseAsync();
        }
        /// <summary>
        /// 初始化路径执行情况：路径&&药品
        /// </summary>
        private void InitEnfroceInfo()
        {
            //在病患列表里新加一个执行xml字段 EmFroceXml 
            //如果为空，从路径基本信息第一步开始走,即从基本档LOAD资料，在点击下一步时新生成一个XML,具体格式见上，其中Activity多一个EnForceTime（yyyy-MM-dd HH:mm:ss)，第一次没有RULE?
            //如果不为空，路径信息从CP_InPatientPathEnForce捞取，读取最后一个 Activity的信息显示，在点击保存时更新XML,XElement.Add()?
            //需要考虑下己经执行过后XML的构造，实体最好加上对应的属性！！！！！11
            //在进入路径的地方，将EmFroceXml更新至CP_InPatientPathEnForce.EnforceXml(others)
            //想引入线程
            /* 1.判断并发开关是否打开（建议打开，写在WEB.CONFIG 文件里)
             *  1.1)若打开，插入TABLE（新增 CP_LockPatient)记录首页序号,医生工号，IP，CREATE_TIME,其它医师再去看此病患时
             *      在病患基本信息区域再加一行，红色显示 病患被{0}在｛1｝锁定，｛0｝医师工号+姓名，｛1｝ IP ;
             *  1.2)若不打开，1.1）不做
             * 2.根据CP_InPatientPathEnForce.EnforceXml,实例化WorkFlow类m_CurrentWorkFlow,根据m_CurrentWorkFlow当前结点的类型，捞取组套信息，
             *   2.1）当前结点不是循环结点，子节点肯定只有一个，根据子节点ACTIVITYUNINQID捞取组套信息
             *   2.2）当前结点是循环结点，子节点>=1，根据CurrentElementState=NOW的子节点ACTIVITYUNINQID捞取组套信息
             *   2.3）m_CurrentWorkFlow里当前节点是初始节点，下一步时，可以没有医嘱;
             *        若为终结点，点击下一步时，不调用NEXT方法，只保存医令和更新XML
             *        保存完重新LOAD页面后，下一步BUTTON后显示完成BUTTON，点击弹出询问窗体，确定，更新状态（注，只做更新状态，医令必须下一步BUTTON处理）。
             * 3.查看医令时，点击工作流图上结点，进行查看：
             *   3.1）不是循环结点，根据子节点ACTIVITID捞取医嘱信息
             *   3.2） 循环结点，在下一步BUTTON时显示出查看上一步医令，查看下-步医令 BUTTON，点击后根据子节点ACTIVITID捞取医嘱信息
             *   3.3） 若当前结点为循环结点，也要做3.2)，考虑下BUTTON的（名称、位置)，不要给USER造成错觉
             *   3.3)  查看医令时，若结点类型为NEXT,则下一步BUTTON不可用,即只可查看，不可保存
             * 4.引入，加一个开关（写在WEB.CONFIG 文件里)，若开，有权限的人才可以看到，若不开，所有人都看不到
             *   每次只可引入一条，不可引入本身。。
             *   
             *   
             *   
             */
        }
        #endregion  初始化病患基本信息和当前步骤基本医嘱信息
        #region   GRID里DATETIMEPICKER处理
        /// <summary>
        /// 医嘱开始时间选择
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadDateTimePicker_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                RadDateTimePicker timePicker = sender as RadDateTimePicker;
                if (timePicker.SelectedValue.Value != null && timePicker.SelectedValue.Value.ToString() != string.Empty)
                    m_IsTimeChanged = true;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 医嘱开始时间选择完成 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadDateTimePicker_DropDownClosed(object sender, RoutedEventArgs e)
        {
            RadDateTimePicker timePicker = sender as RadDateTimePicker;
            if (m_IsTimeChanged)
            {
                ObservableCollection<CP_DoctorOrder> listOrder = this.radGridViewOrderList.ItemsSource as ObservableCollection<CP_DoctorOrder>;
                CP_DoctorOrder order = listOrder.First(cp => cp.OrderGuid.Equals(timePicker.Tag.ToString()));
                order.Ksrq = timePicker.SelectedValue.Value.ToString("yyyy-MM-dd HH:mm:ss");
                order.IsModify = true;//记得改成Boolean
                this.radGridViewOrderList.ItemsSource = null;
                this.radGridViewOrderList.ItemsSource = listOrder;
                m_IsTimeChanged = false;
            }
            else
            {
                //this.radGridViewOrderList.ActionOnLostFocus = ActionOnLostFocus.
            }
        }
        #endregion GRID里CHECKBOX初始化和DATETIMEPICKER处理
        #region 保存
        /// <summary>
        /// 有异常医嘱，询问关闭后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExceptionOrderReason(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                RWPathVariation radVariation = new RWPathVariation();
                radVariation.Closed += new EventHandler<WindowClosedEventArgs>(radVariation_Closed);
                radVariation.ListUnDoOrder = m_UnDoOrder;
                radVariation.ListUnNewOrder = m_NewOrder;//
                radVariation.CurrentActivityId = m_WorkFlow.Activitys.CurrentViewActivity.CurrentViewActiveChildren.ActivityUniqueID;
                radVariation.PathLjdm = Global.InpatientListCurrent.Ljdm;
                radVariation.ShowDialog();
            }
            else
            {
                return;
            }
        }
        /// <summary>
        /// 异常窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radVariation_Closed(object sender, WindowClosedEventArgs e)
        {
            try
            {
                RWPathVariation radVariation = sender as RWPathVariation;
                if (radVariation.DialogResult == true)
                {
                    m_ListUnEnforceReason = radVariation.ListUnEnforceReason;
                    m_ListUnNewReason = radVariation.ListUnNewReason;
                    m_ListUnOtherReason = radVariation.ListUnOtherReason;
                    SaveEnforceInfo();
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 保存医嘱&XML
        /// </summary>
        /// <param name="isNextSetp">代表是否是下一步,跳转后点击保存不算，不用更新XML</param>
        private void SaveEnforceInfo()
        {
            radBusyIndicator.IsBusy = true;
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.SaveEnforceXmlOrderCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        m_UnDoOrder.Clear();
                        m_NewOrder.Clear();
                        m_DelOrder.Clear();
                        
                        Activitys_WorkFlow_ActivitySelectChanged(m_WorkFlow.Activitys.CurrentActivity);
                        m_AddOrder.Clear();
                        m_IsLeadIn = false;
                        PublicMethod.RadAlterBox("执行成功", m_StrTitle);
                    }
                    else
                    {
                        radBusyIndicator.IsBusy = false;
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            //保存,1)ENFORCEXML;2)医嘱
            //m_EnforceWorkXmlTemp,更新
            //获取datagridview里面的全部信息，AS 成医嘱类，进行保存，保存函数放在一个TRANS里
            //update 当前步骤数
            //load 下一步的初始化医嘱信息      
            ObservableCollection<CP_DoctorOrder> listOrderAdd = new ObservableCollection<CP_DoctorOrder>();
            ObservableCollection<CP_DoctorOrder> listOrderModify = new ObservableCollection<CP_DoctorOrder>();
            ObservableCollection<CP_DoctorOrder> listOrder = new ObservableCollection<CP_DoctorOrder>();
            listOrder = this.radGridViewOrderList.ItemsSource as ObservableCollection<CP_DoctorOrder>;
            #region 之前的逻辑，暂时保留
            //foreach (CP_DoctorOrder order in listOrder)
            //{
            //    if (order.Yzxh == 0)
            //    {
            //        if (IsCheckBoxChecked(order.OrderGuid))
            //        {
            //            order.Tbbz = 1;//有效记录
            //            order.Tsbj = 0x01;
            //            listOrderAdd.Add(order);
            //        }
            //    }
            //    else
            //    {
            //        if (order.IsModify == true)
            //        {
            //            listOrderModify.Add(order);
            //        }
            //    }
            //}
            #endregion
            if (radGridViewOrderList.SelectedItems != null)
            {
                foreach (CP_DoctorOrder order in radGridViewOrderList.SelectedItems)
                {
                    if (order.Yzxh == 0)
                    {
                        order.Tbbz = 1;//有效记录
                        order.Tsbj = 0x01;
                        listOrderAdd.Add(order);
                    }
                    else
                    {
                        if (order.IsModify)
                        {
                            listOrderModify.Add(order);
                        }
                    }
                }
            }
            if (listOrderAdd.Count > 0 || listOrderModify.Count > 0 || m_DelOrder.Count > 0)
            {
                try
                {
                    //获取当前页面最新XML
                    ObservableCollection<String> listLjdm = GetListLjdm();
                    if (m_WorkFlow.Activitys.CurrentActivity.CurrentActiveChildren.EnForceTime == String.Empty)
                        m_WorkFlow.Activitys.CurrentActivity.CurrentActiveChildren.EnForceTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                    Global.InpatientListCurrent.EnForceWorkFlowXml = m_WorkFlow.WorkFlowXml;
                    client.SaveEnforceXmlOrderAsync(m_WorkFlow.WorkFlowXml,
                        m_WorkFlow.Activitys.CurrentViewActivity.Flow.UniqueID,
                        m_WorkFlow.Activitys.CurrentViewActivity.CurrentViewActiveChildren.ActivityUniqueID,
                        m_WorkFlow.Activitys.CurrentViewActivity.CurrentViewActiveChildren.ActivityChildrenID,
                        listOrderAdd, listOrderModify, Global.InpatientListCurrent, m_ListUnEnforceReason,
                        m_ListUnNewReason, m_ListUnOtherReason, m_DelOrder,
                        m_IsLeadIn, listLjdm);
                    client.CloseAsync();
                }
                catch (Exception ex)
                {
                    radBusyIndicator.IsBusy = false;
                    throw ex;
                }
            }
            else
            {
                radBusyIndicator.IsBusy = false;
                PublicMethod.RadAlterBox("医嘱无变动,不需要保存", m_StrTitle);
            }
        }
        /// <summary>
        /// 下一步，保存之前check医令选中情况
        /// </summary>
        /// <param name="strWaring"></param>
        /// <returns></returns>
        private Boolean Check(out string strWaring)
        {
            //check 必选红色是否全选，全选通过
            //若没选将医嘱名称显示出来,并询问是否继续，若是，弹出异常原因输入框
            m_UnDoOrder.Clear();
            m_NewOrder.Clear();
            ObservableCollection<CP_DoctorOrder> listOrderTemp = this.radGridViewOrderList.ItemsSource as ObservableCollection<CP_DoctorOrder>;
            ObservableCollection<CP_DoctorOrder> listOrder = new ObservableCollection<CP_DoctorOrder>();
            foreach (CP_DoctorOrder order in listOrderTemp)
                listOrder.Add(order);
            foreach (CP_DoctorOrder order in m_DelOrder)
            {
                if (!listOrder.Contains(order))
                    listOrder.Add(order);
            }
            strWaring = string.Empty;
            foreach (CP_DoctorOrder order in listOrder)
            {
                if (order.FromTable == OrderFromTable.CP_AdviceGroupDetail.ToString())
                {
                    if (radGridViewOrderList.SelectedItems == null)
                    {
                        strWaring += order.YzbzName + ":" + order.Ypmc + "\r\n";
                        m_UnDoOrder.Add(order);
                    }
                    else
                    {
                        var list = radGridViewOrderList.SelectedItems;
                        var item = from cp in list
                                   where order.OrderGuid == ((CP_DoctorOrder)cp).OrderGuid
                                   select cp;
                        //如果是没有选中
                        if (item.Count() == 0)
                        {
                            strWaring += order.YzbzName + ":" + order.Ypmc + "\r\n";
                            m_UnDoOrder.Add(order);
                        }
                    }
                }
                else if (order.FromTable == String.Empty || order.FromTable == "CP_AdviceSuitDetail")
                {
                    strWaring += order.YzbzName + ":" + order.Ypmc + "\r\n";
                    m_NewOrder.Add(order);
                }
            }
            if (strWaring != string.Empty)
            {
                strWaring = "以下必选医嘱未执行或者包含新增医嘱，是否继续？" + "\r\n" + strWaring;
                return false;
            }
            return true;
        }
        #endregion
        #region 点击下一步时
        /// <summary>
        /// 点击下一步时,check有没有必须执行，而未执行的
        /// </summary>
        /// <param name="strWaring"></param>
        /// <returns></returns>
        private Boolean CheckUIOrder(out String strWaring)
        {
            //check 必选红色是否全选，全选通过
            //若没选将医嘱名称显示出来,并询问是否继续，若是，弹出异常原因输入框
            m_UnDoOrder.Clear();
            m_NewOrder.Clear();
            ObservableCollection<CP_DoctorOrder> listOrderTemp = this.radGridViewOrderList.ItemsSource as ObservableCollection<CP_DoctorOrder>;
            ObservableCollection<CP_DoctorOrder> listOrder = new ObservableCollection<CP_DoctorOrder>();
            strWaring = string.Empty;
            if (listOrderTemp == null)
                return true;
            foreach (CP_DoctorOrder order in listOrderTemp)
                listOrder.Add(order);
            foreach (CP_DoctorOrder order in m_DelOrder)
            {
                if (!listOrder.Contains(order))
                    listOrder.Add(order);
            }
            foreach (CP_DoctorOrder order in listOrder)
            {
                if (order.FromTable == OrderFromTable.CP_AdviceGroupDetail.ToString())
                {
                    strWaring += order.YzbzName + ":" + order.Ypmc + "\r\n";
                    m_UnDoOrder.Add(order);
                }
            }
            if (m_AddOrder != null)
            {
                foreach (CP_DoctorOrder order in m_AddOrder)
                {
                    strWaring += order.YzbzName + ":" + order.Ypmc + "\r\n";
                }
            }
            if (strWaring != string.Empty)
            {
                strWaring = "以下必选医嘱或新增医嘱未执行，是否继续？" + "\r\n" + strWaring;
                return false;
            }
            return true;
        }
        /// <summary>
        /// 点击一下步，询问关闭后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnExceptionNextReason(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                RWPathVariation radVariationNext = new RWPathVariation();
                radVariationNext.Closed += new EventHandler<WindowClosedEventArgs>(radVariationNext_Closed);
                radVariationNext.ListUnDoOrder = m_UnDoOrder;
                radVariationNext.ListUnNewOrder = m_NewOrder;//      
                radVariationNext.CurrentActivityId = m_WorkFlow.Activitys.CurrentViewActivity.CurrentViewActiveChildren.ActivityUniqueID;
                radVariationNext.PathLjdm = Global.InpatientListCurrent.Ljdm;
                radVariationNext.ShowDialog();
            }
            else
            {
                return;
            }
        }
        private void radVariationNext_Closed(object sender, WindowClosedEventArgs e)
        {
            try
            {
                RWPathVariation radVariation = sender as RWPathVariation;
                if (radVariation.DialogResult == true)
                {
                    m_ListUnEnforceReason = radVariation.ListUnEnforceReason;
                    m_ListUnNewReason = new ObservableCollection<CP_VariantRecords>();
                    m_ListUnOtherReason = new ObservableCollection<CP_VariantRecords>();
                    if (Next(null))
                    {
                        UpdateXmlAfterNext();
                    }
                }
                else
                    return;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 下一步
        ///使用说明：调用方法Next(null),调用完成后,可以直接使用CurrentActivity.CurrentActiveChildren
        /// <returns></returns>
        /// </summary>
        /// <param name="a">指定的下一步节点</param>
        private Boolean Next(Activity a)
        {
            //如果当前节点为结束类型的节点
            if (m_WorkFlow.Activitys.CurrentActivity.Type == ActivityType.COMPLETION)
                return true;
            //指定NextActivity
            if (a != null)
            {
                Boolean bReturn = false;
                RWAccessNode wNode = new RWAccessNode(Global.InpatientListCurrent, Global.InpatientListCurrent.Ljdm, a.UniqueID);
                wNode.Closed += (send, ea) =>
                {
                    if (ea.DialogResult == true)
                    {
                        foreach (var item in m_WorkFlow.Activitys)
                        {
                            if (item.UniqueID == a.UniqueID)
                            {
                                m_WorkFlow.Activitys.CurrentActivity = item;
                            }
                        }
                        m_WorkFlow.Activitys.HideActivitys();
                        UpdateXmlAfterNext();
                        // m_WorkFlow._ContainerEdit_Loaded(null, null); 
                        bReturn = true;
                        radBusyIndicator.IsBusy = false;
                    }
                };
                wNode.ShowDialog();
                return bReturn;
            }
            //未指定NextActivity，并且NextActivitys.Count == 1 且CurrentActivity.Type不是循环节点
            if (m_WorkFlow.Activitys.CurrentActivity.NextActivitys.Count == 1 && m_WorkFlow.Activitys.CurrentActivity.Type != ActivityType.AUTOMATION)
            {
                Boolean bReturn = false;
                RWAccessNode wNode = new RWAccessNode(Global.InpatientListCurrent, Global.InpatientListCurrent.Ljdm, m_WorkFlow.Activitys.CurrentActivity.NextActivitys[0].UniqueID);
                wNode.Closed += (send, ea) =>
                {
                    if (ea.DialogResult == true)
                    {
                        m_WorkFlow.Activitys.CurrentActivity = m_WorkFlow.Activitys.CurrentActivity.NextActivitys[0];
                        UpdateXmlAfterNext();
                        bReturn = true;
                        radBusyIndicator.IsBusy = false;
                    }
                };
                wNode.ShowDialog();
                return bReturn;
            }
            List<PublicMethod.Pair> pairList = new List<PublicMethod.Pair>();
            pairList.AddRange(from p in m_WorkFlow.Activitys.CurrentActivity.NextActivitys select new PublicMethod.Pair(p.UniqueID, p.ActivityName));
            //如果是循环节点，在选择项中添加“循环当前节点”
            if (m_WorkFlow.Activitys.CurrentActivity.Type == ActivityType.AUTOMATION)
                pairList.Insert(0, new PublicMethod.Pair("-1", "循环当前节点"));
            RadWindow radWindowNext = new RadWindow();
            radWindowNext.Closed += new EventHandler<WindowClosedEventArgs>(radWindowNext_Closed);
            radWindowNext.Header = "节点选择";
            new PublicMethod().ShowSelectWindow(ref  radWindowNext, pairList);
            return false;
        }
        /// <summary>
        /// 弹出选择框关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>    
        private void radWindowNext_Closed(object sender, WindowClosedEventArgs e)
        {
            try
            {
                if (e.PromptResult == null || e.PromptResult.ToString().Trim() == "")
                    return;
                //选择循环当前节点
                if (e.PromptResult == "-1")
                {
                    if (NextAutoMation())
                        UpdateXmlAfterNext();
                    m_AddOrder.Clear();
                    return;
                }
                List<Activity> ActivityList = new List<Activity>();
                ActivityList.AddRange(from a in m_WorkFlow.Activitys.CurrentActivity.NextActivitys where a.UniqueID == e.PromptResult select a);
                if (ActivityList.Count != 0)
                    Next(ActivityList[0]);
                UpdateXmlAfterNext();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 提示框关闭后，为当前循环节点添加一个子节点
        /// </summary>
        public Boolean NextAutoMation()
        {
            Boolean bReturn = false;
            RWAccessNode wNode = new RWAccessNode(Global.InpatientListCurrent, Global.InpatientListCurrent.Ljdm, m_WorkFlow.Activitys.CurrentActivity.UniqueID);
            wNode.Closed += (send, ea) =>
            {
                if (ea.DialogResult == true)
                {
                    if (m_WorkFlow.Activitys.CurrentActivity.Type == ActivityType.AUTOMATION)
                    {
                        ActiveChildren children = new ActiveChildren();
                        children.ActivityUniqueID = m_WorkFlow.Activitys.CurrentActivity.UniqueID;
                        children.ActivityChildrenID = Guid.NewGuid().ToString();
                        children.CurrentElementState = ElementState.Now;
                        m_WorkFlow.Activitys.CurrentActivity.CurrentActiveChildren.CurrentElementState = ElementState.Pre;
                        m_WorkFlow.Activitys.CurrentActivity.ActiveChildrens.Add(children);
                        m_WorkFlow.Activitys.CurrentActivity.CurrentActiveChildren = children;
                        //触发选中事件
                        m_WorkFlow.Activitys.CurrentActivity = m_WorkFlow.Activitys.CurrentActivity;
                    }
                    UpdateXmlAfterNext();
                    bReturn = true;
                    radBusyIndicator.IsBusy = false;
                }
            };
            wNode.ShowDialog();
            return bReturn;
        }
        /// <summary>
        /// 点击一下步后进行更新动作
        /// </summary>
        private void UpdateXmlAfterNext()
        {
            try
            {
                ObservableCollection<String> listLjdm = GetListLjdm();
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.UpdateEnForceInfoCompleted +=
                (obj, e) =>
                {
                    radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        Global.InpatientListCurrent.Ljts = Convert.ToString((Int32.Parse(Global.InpatientListCurrent.Ljts) + 1));
                        Global.InpatientListCurrent.EnForceWorkFlowXml = m_WorkFlow.WorkFlowXml;
                        InitWorkFlow(Global.InpatientListCurrent.EnForceWorkFlowXml);
                        InitPage();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
                client.UpdateEnForceInfoAsync(Global.InpatientListCurrent, m_WorkFlow.WorkFlowXml,
                        m_WorkFlow.Activitys.CurrentViewActivity.CurrentViewActiveChildren.ActivityChildrenID,
                       m_ListUnEnforceReason, null, null, m_IsLeadIn, listLjdm, m_WorkFlow.Activitys.CurrentActivity.UniqueID,"0");
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region  新增，删除，修改
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnNewOrder_Click(object sender, RoutedEventArgs e)
        {
            //RadChildWindowDrug windowDrug = new RadChildWindowDrug();
            //windowDrug.Closed += new EventHandler<WindowClosedEventArgs>(windowDrug_Closed);
            //windowDrug.ShowDialog();
        }
        private void windowDrug_Closed(object sender, WindowClosedEventArgs e)
        {
            //RadChildWindowDrug window = sender as RadChildWindowDrug;
            //if (window.DialogResult == true)
            //{
            //    ObservableCollection<CP_DoctorOrder> listOrder = this.radGridViewOrderList.ItemsSource as ObservableCollection<CP_DoctorOrder>;
            //    this.radGridViewOrderList.ItemsSource = null;
            //    listOrder.Add(window.DoctorOrder);
            //    m_GridCheckBox.Clear();
            //    this.radGridViewOrderList.ItemsSource = listOrder;
            //}
        }
        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnModifyOrder_Click(object sender, RoutedEventArgs e)
        {
            //List<CP_DoctorOrder> listOrder = GetCheckOrderList(false);
            //if (listOrder.Count > 1 || listOrder.Count == 0)
            //{
            //    PublicMethod.RadAlterBox("请选择一条医嘱进行修改", m_StrTitle);
            //    return;
            //}
            //RadChildWindowDrug windowDrugModify = new RadChildWindowDrug();
            //windowDrugModify.ManualType = ManualType.Edit;
            //windowDrugModify.DoctorOrderModify = listOrder[0];
            //windowDrugModify.Closed += new EventHandler<WindowClosedEventArgs>(windowDrugModify_Closed);
            //windowDrugModify.ShowDialog();
        }
        private void windowDrugModify_Closed(object sender, WindowClosedEventArgs e)
        {
            //RadChildWindowDrug windowDrugModify = sender as RadChildWindowDrug;
            //if (windowDrugModify.DialogResult == true)
            //{
            //    ObservableCollection<CP_DoctorOrder> listGridOrder = this.radGridViewOrderList.ItemsSource as ObservableCollection<CP_DoctorOrder>;
            //    listGridOrder = InitNewList4Grid(listGridOrder, windowDrugModify.DoctorOrderModify);
            //    this.radGridViewOrderList.ItemsSource = null;
            //    //listGridOrder.Add(windowDrugModify.DoctorOrder);
            //    m_GridCheckBox.Clear();
            //    this.radGridViewOrderList.ItemsSource = listGridOrder;
            //}
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDelOrder_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RemoveOrder();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 点击保存后更新GRID数据源
        /// </summary>
        /// <param name="listGridOrder"></param>
        /// <param name="doctorOrder"></param>
        /// <returns></returns>
        private ObservableCollection<CP_DoctorOrder> InitNewList4Grid(ObservableCollection<CP_DoctorOrder> listGridOrder, CP_DoctorOrder doctorOrder, ref ObservableCollection<CP_DoctorOrder> listSelectOrder)
        {
            if (this.radGridViewOrderList.SelectedItems != null)
            {
                foreach (var item in this.radGridViewOrderList.SelectedItems)
                    listSelectOrder.Add(item as CP_DoctorOrder);
            }
            for (int i = 0; i < listGridOrder.Count; i++)
            {
                CP_DoctorOrder order = listGridOrder[i];
                if (order.OrderGuid == doctorOrder.OrderGuid)
                {
                    order = doctorOrder;
                    if (order.Yzxh != 0)
                        order.IsModify = true;
                    break;
                }
            }
            for (int i = 0; i < listSelectOrder.Count; i++)
            {
                CP_DoctorOrder order = listSelectOrder[i];
                if (order.OrderGuid == doctorOrder.OrderGuid)
                {
                    order = doctorOrder;
                    if (order.Yzxh != 0)
                        order.IsModify = true;
                    break;
                }
            }
            return listGridOrder;
        }
        #endregion
        #region 引入
        /// <summary>
        /// 引入窗体关闭后
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void inPath_Closed(object sender, WindowClosedEventArgs e)
        {
            try
            {
                if (e.DialogResult == true)
                {
                    InitWorkFlow(((RWLeadInPath)sender).EnforceLeadXml);
                    m_IsLeadIn = true;
                    //this.gridWorkFlowShow.Visibility = System.Windows.Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region 退出
        /// <summary>
        /// 退出原因窗体关闭
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void path_Closed(object sender, WindowClosedEventArgs e)
        {
            try
            {
                if (e.DialogResult == true)
                {
                    radBusyIndicator.IsBusy = true;
                    YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                    client.QuitPathCompleted +=
                        (obj, ea) =>
                        {
                            radBusyIndicator.IsBusy = false;
                            if (ea.Error == null)
                            {
                                Global.InpatientListCurrent.Ljzt = Convert.ToString((int)PathStatus.QuitPath);
                                //m_IsDone = true;//check
                                
                                EnableButtonState(false);
                                PublicMethod.RadAlterBox("成功退出路径", m_StrTitle);
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    client.QuitPathAsync(Global.InpatientListCurrent, ((RWQuitPath)sender).VariantRecords, m_WorkFlow.Activitys.CurrentActivity.UniqueID);
                    client.CloseAsync();
                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region 医嘱大类选择
        private void SetGridVisible(OrderPanelBarCategory barItemTag)
        {
            switch (barItemTag)
            {
                case OrderPanelBarCategory.Drug:
                    drugGrid.Visibility = Visibility.Visible;
                    operGrid.Visibility = Visibility.Collapsed;
                    rislisGrid.Visibility = Visibility.Collapsed;
                    mealGrid.Visibility = Visibility.Collapsed;
                    observationGrid.Visibility = Visibility.Collapsed;
                    activityGrid.Visibility = Visibility.Collapsed;
                    careGrid.Visibility = Visibility.Collapsed;
                    break;
                case OrderPanelBarCategory.Oper:
                    drugGrid.Visibility = Visibility.Collapsed;
                    operGrid.Visibility = Visibility.Visible;
                    rislisGrid.Visibility = Visibility.Collapsed;
                    mealGrid.Visibility = Visibility.Collapsed;
                    observationGrid.Visibility = Visibility.Collapsed;
                    activityGrid.Visibility = Visibility.Collapsed;
                    careGrid.Visibility = Visibility.Collapsed;
                    break;
                case OrderPanelBarCategory.RisLis:
                    drugGrid.Visibility = Visibility.Collapsed;
                    operGrid.Visibility = Visibility.Collapsed;
                    rislisGrid.Visibility = Visibility.Visible;
                    mealGrid.Visibility = Visibility.Collapsed;
                    observationGrid.Visibility = Visibility.Collapsed;
                    activityGrid.Visibility = Visibility.Collapsed;
                    careGrid.Visibility = Visibility.Collapsed;
                    break;
                case OrderPanelBarCategory.Meal:
                    drugGrid.Visibility = Visibility.Collapsed;
                    operGrid.Visibility = Visibility.Collapsed;
                    rislisGrid.Visibility = Visibility.Collapsed;
                    mealGrid.Visibility = Visibility.Visible;
                    observationGrid.Visibility = Visibility.Collapsed;
                    activityGrid.Visibility = Visibility.Collapsed;
                    careGrid.Visibility = Visibility.Collapsed;
                    break;
                case OrderPanelBarCategory.Observation:
                    drugGrid.Visibility = Visibility.Collapsed;
                    operGrid.Visibility = Visibility.Collapsed;
                    rislisGrid.Visibility = Visibility.Collapsed;
                    mealGrid.Visibility = Visibility.Collapsed;
                    observationGrid.Visibility = Visibility.Visible;
                    activityGrid.Visibility = Visibility.Collapsed;
                    careGrid.Visibility = Visibility.Collapsed;
                    break;
                case OrderPanelBarCategory.Activity:
                    drugGrid.Visibility = Visibility.Collapsed;
                    operGrid.Visibility = Visibility.Collapsed;
                    rislisGrid.Visibility = Visibility.Collapsed;
                    mealGrid.Visibility = Visibility.Collapsed;
                    observationGrid.Visibility = Visibility.Collapsed;
                    activityGrid.Visibility = Visibility.Visible;
                    careGrid.Visibility = Visibility.Collapsed;
                    break;
                case OrderPanelBarCategory.Care:
                    drugGrid.Visibility = Visibility.Collapsed;
                    operGrid.Visibility = Visibility.Collapsed;
                    rislisGrid.Visibility = Visibility.Collapsed;
                    mealGrid.Visibility = Visibility.Collapsed;
                    observationGrid.Visibility = Visibility.Collapsed;
                    activityGrid.Visibility = Visibility.Collapsed;
                    careGrid.Visibility = Visibility.Visible;
                    break;
                default:
                    drugGrid.Visibility = Visibility.Collapsed;
                    operGrid.Visibility = Visibility.Collapsed;
                    rislisGrid.Visibility = Visibility.Collapsed;
                    mealGrid.Visibility = Visibility.Collapsed;
                    observationGrid.Visibility = Visibility.Collapsed;
                    activityGrid.Visibility = Visibility.Collapsed;
                    careGrid.Visibility = Visibility.Collapsed;
                    break;
            }
        }
        #endregion
        #region 医嘱编辑控件相关
        #region 药品
        /// <summary>
        /// 药品医嘱输入初始化
        /// </summary>
        private void InitDrugControl()
        {
            drugOrderControl.AfterDrugCinfirmeddEvent += new UserControls.UCDrug.DrugConfirmed(drugOrderControl_AfterDrugCinfirmeddEvent);
            drugOrderControl.AfterDrugLoadedEvent += new UserControls.UCDrug.DrugLoaded(drugOrderControl_AfterDrugLoadedEvent);
        }
        private void drugOrderControl_AfterDrugLoadedEvent(object sender, RoutedEventArgs e)
        {
            // to do nothing 
        }
        private void drugOrderControl_AfterDrugCinfirmeddEvent(object sender, CP_DoctorOrder e)
        {
            if (drugOrderControl.ManualType == ManualType.Edit)
                AddModifyOrder(e);
            else
                AddNewOrder(e);
        }
        #endregion
        #region 检验检查
        /// <summary>
        /// 检验检查医嘱输入初始化
        /// </summary>
        private void InitRisLisControl()
        {
            risLisOrderControl.AfterDrugCinfirmeddEvent += new UserControls.UCRISLISOrder.DrugConfirmed(risLisOrderControl_AfterDrugCinfirmeddEvent);
            risLisOrderControl.AfterDrugLoadedEvent += new UserControls.UCRISLISOrder.DrugLoaded(risLisOrderControl_AfterDrugLoadedEvent);
        }
        private void risLisOrderControl_AfterDrugLoadedEvent(object sender, RoutedEventArgs e)
        {
            // to do nothing 
        }
        private void risLisOrderControl_AfterDrugCinfirmeddEvent(object sender, CP_DoctorOrder e)
        {
            if (risLisOrderControl.ManualType == ManualType.Edit)
                AddModifyOrder(e);
            else
                AddNewOrder(e);
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
            foodOrderControl.AfterDrugCinfirmeddEvent += new UserControls.UCOtherOrder.DrugConfirmed(foodOrderControl_AfterDrugCinfirmeddEvent);
            foodOrderControl.AfterDrugLoadedEvent += new UserControls.UCOtherOrder.DrugLoaded(foodOrderControl_AfterDrugLoadedEvent);
        }
        private void foodOrderControl_AfterDrugLoadedEvent(object sender, RoutedEventArgs e)
        {
            // to do nothing 
        }
        private void foodOrderControl_AfterDrugCinfirmeddEvent(object sender, CP_DoctorOrder e)
        {
            if (foodOrderControl.ManualType == ManualType.Edit)
                AddModifyOrder(e);
            else
                AddNewOrder(e);
        }
        #endregion
        #region  观察
        /// <summary>
        ///   观察医嘱输入初始化
        /// </summary>
        private void InitObservationControl()
        {
            observationOrderControl.PanelCategory = OrderPanelBarCategory.Observation;
            observationOrderControl.OrderCategory = OrderItemCategory.Observation;
            observationOrderControl.AfterDrugCinfirmeddEvent += new UserControls.UCOtherOrder.DrugConfirmed(observationOrderControl_AfterDrugCinfirmeddEvent);
            observationOrderControl.AfterDrugLoadedEvent += new UserControls.UCOtherOrder.DrugLoaded(observationOrderControl_AfterDrugLoadedEvent);
        }
        private void observationOrderControl_AfterDrugLoadedEvent(object sender, RoutedEventArgs e)
        {
            // to do nothing 
        }
        private void observationOrderControl_AfterDrugCinfirmeddEvent(object sender, CP_DoctorOrder e)
        {
            if (observationOrderControl.ManualType == ManualType.Edit)
                AddModifyOrder(e);
            else
                AddNewOrder(e);
        }
        #endregion
        #region  活动
        /// <summary>
        ///   活动医嘱输入初始化
        /// </summary>
        private void InitActivityControl()
        {
            activityOrderControl.PanelCategory = OrderPanelBarCategory.Activity;
            activityOrderControl.OrderCategory = OrderItemCategory.Activity;
            activityOrderControl.AfterDrugCinfirmeddEvent += new UserControls.UCOtherOrder.DrugConfirmed(activityOrderControl_AfterDrugCinfirmeddEvent);
            activityOrderControl.AfterDrugLoadedEvent += new UserControls.UCOtherOrder.DrugLoaded(activityOrderControl_AfterDrugLoadedEvent);
        }
        private void activityOrderControl_AfterDrugLoadedEvent(object sender, RoutedEventArgs e)
        {
            // to do nothing 
        }
        private void activityOrderControl_AfterDrugCinfirmeddEvent(object sender, CP_DoctorOrder e)
        {
            if (activityOrderControl.ManualType == ManualType.Edit)
                AddModifyOrder(e);
            else
                AddNewOrder(e);
        }
        #endregion
        #region  护理及宣教
        /// <summary>
        ///   护理及宣教医嘱输入初始化
        /// </summary>
        private void InitCareControl()
        {
            careOrderControl.PanelCategory = OrderPanelBarCategory.Care;
            careOrderControl.OrderCategory = OrderItemCategory.Care;
            careOrderControl.AfterDrugCinfirmeddEvent += new UserControls.UCOtherOrder.DrugConfirmed(careOrderControl_AfterDrugCinfirmeddEvent);
            careOrderControl.AfterDrugLoadedEvent += new UserControls.UCOtherOrder.DrugLoaded(careOrderControl_AfterDrugLoadedEvent);
        }
        private void careOrderControl_AfterDrugLoadedEvent(object sender, RoutedEventArgs e)
        {
            // to do nothing 
        }
        private void careOrderControl_AfterDrugCinfirmeddEvent(object sender, CP_DoctorOrder e)
        {
            if (careOrderControl.ManualType == ManualType.Edit)
                AddModifyOrder(e);
            else
                AddNewOrder(e);
        }
        #endregion
        /// <summary>
        /// 新增医嘱至GRID
        /// </summary>
        /// <param name="order"></param>
        private void AddNewOrder(CP_DoctorOrder order)
        {
            try
            {
                ObservableCollection<CP_DoctorOrder> listOrder = this.radGridViewOrderList.ItemsSource as ObservableCollection<CP_DoctorOrder>;
                foreach (CP_DoctorOrder item in listOrder)
                {
                    if (item.Ypmc == order.Ypmc)
                    {
                        radGridViewOrderList.SelectedItem = item;
                        //PublicMethod.RadAlterBox("不能添加相同医嘱!", m_StrTitle);
                        return;
                    }
                }
                ObservableCollection<CP_DoctorOrder> listSelectOrder = new ObservableCollection<CP_DoctorOrder>();
                if (this.radGridViewOrderList.SelectedItems != null)
                {
                    foreach (var item in this.radGridViewOrderList.SelectedItems)
                        listSelectOrder.Add(item as CP_DoctorOrder);
                }
                this.radGridViewOrderList.ItemsSource = null;
                listOrder.Add(order);
                listSelectOrder.Add(order);
                m_AddOrder.Add(order);
                this.radGridViewOrderList.ItemsSource = listOrder;
                //xjt,选中新增医嘱
                foreach (CP_DoctorOrder sorder in listSelectOrder)
                {
                    this.radGridViewOrderList.SelectedItems.Add(sorder);
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 修改医嘱至GRID
        /// </summary>
        /// <param name="order"></param>
        private void AddModifyOrder(CP_DoctorOrder order)
        {
            ObservableCollection<CP_DoctorOrder> listGridOrder = this.radGridViewOrderList.ItemsSource as ObservableCollection<CP_DoctorOrder>;
            ObservableCollection<CP_DoctorOrder> listSelectOrder = new ObservableCollection<CP_DoctorOrder>();
            listGridOrder = InitNewList4Grid(listGridOrder, order, ref listSelectOrder);
            this.radGridViewOrderList.ItemsSource = null;
            this.radGridViewOrderList.ItemsSource = listGridOrder;
            //xjt,选中新增医嘱
            foreach (CP_DoctorOrder sorder in listSelectOrder)
            {
                this.radGridViewOrderList.SelectedItems.Add(sorder);
            }
        }
        #endregion
        #region gridview rowloaded,menu,SelectionChanged
        private void radGridViewOrderList_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
            RadContextMenu rowContextMenu = new RadContextMenu(); //新建一个右键菜单
            rowContextMenu.Width = 200;
            rowContextMenu.Items.Add(new RadMenuItem() { Header = "编辑医嘱", Tag = TagName.Edit });
            rowContextMenu.Items.Add(new RadMenuItem() { IsSeparator = true });
            rowContextMenu.Items.Add(new RadMenuItem() { Header = "删除医嘱", Tag = TagName.Del });
            rowContextMenu.AddHandler(RadMenuItem.ClickEvent, new RoutedEventHandler(OnRowMenuItemClick));
            rowContextMenu.Opened += new RoutedEventHandler(rowContextMenu_Opened);
            RadContextMenu.SetContextMenu(e.Row, rowContextMenu);
            List<CheckBox> listCheck = (List<CheckBox>)(this.radGridViewOrderList.ChildrenOfType<CheckBox>());
            if (listCheck.Count > 1)
            {
                if (e.Row != null)
                {
                    CP_DoctorOrder order = e.Row.Item as CP_DoctorOrder;
                    if (!(order.Yzzt == (decimal)OrderStatus.OrderInptut || order.Yzzt == 0))
                        listCheck[listCheck.Count - 1].IsEnabled = false;
                }
            }
        }
        private void OnRowMenuItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RadMenuItem clickedItem = ((RadRoutedEventArgs)e).OriginalSource as RadMenuItem;
                if (clickedItem != null)
                {
                    TagName tagName = (TagName)clickedItem.Tag;
                    CP_DoctorOrder selectedItem = radGridViewOrderList.SelectedItem as CP_DoctorOrder;
                    OrderPanelBarCategory barItemTag = (OrderPanelBarCategory)(int.Parse(selectedItem.Yzlb.ToString()));
                    //DialogParameters parameters = new DialogParameters();
                    switch (tagName)
                    {
                        case TagName.Edit:
                            InitModifyOrderControl(barItemTag, selectedItem);
                            break;
                        case TagName.Del:
                            RemoveOrder();
                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="barItemTag"></param>
        /// <param name="order"></param>
        private void InitModifyOrderControl(OrderPanelBarCategory barItemTag, CP_DoctorOrder order)
        {
            SetGridVisible(barItemTag);
            switch (barItemTag)
            {
                case OrderPanelBarCategory.Drug:
                    this.radioDrug.IsChecked = true;
                    drugOrderControl.ManualType = ManualType.Edit;
                    drugOrderControl.CP_AdviceGroupDetailProptery = order;
                    drugOrderControl.InitModifyOrder();
                    break;
                case OrderPanelBarCategory.Oper:
                    this.radioOper.IsChecked = true;
                    break;
                case OrderPanelBarCategory.RisLis:
                    this.radioRisLis.IsChecked = true;
                    risLisOrderControl.ManualType = ManualType.Edit;
                    risLisOrderControl.CP_AdviceGroupDetailProptery = order;
                    risLisOrderControl.InitModifyOrder();
                    break;
                case OrderPanelBarCategory.Meal:
                    this.radioMeal.IsChecked = true;
                    foodOrderControl.ManualType = ManualType.Edit;
                    foodOrderControl.CP_AdviceGroupDetailProptery = order;
                    foodOrderControl.InitModifyOrder();
                    break;
                case OrderPanelBarCategory.Observation:
                    this.radioObservation.IsChecked = true;
                    observationOrderControl.ManualType = ManualType.Edit;
                    observationOrderControl.CP_AdviceGroupDetailProptery = order;
                    observationOrderControl.InitModifyOrder();
                    break;
                case OrderPanelBarCategory.Activity:
                    this.radioActivity.IsChecked = true;
                    activityOrderControl.ManualType = ManualType.Edit;
                    activityOrderControl.CP_AdviceGroupDetailProptery = order;
                    activityOrderControl.InitModifyOrder();
                    break;
                case OrderPanelBarCategory.Care:
                    this.radioCare.IsChecked = true;
                    careOrderControl.ManualType = ManualType.Edit;
                    careOrderControl.CP_AdviceGroupDetailProptery = order;
                    careOrderControl.InitModifyOrder();
                    break;
            }
        }
        /// <summary>
        /// 从LIST中移除医嘱，并放入m_DelOrder
        /// </summary>
        private void RemoveOrder()
        {
            if (radGridViewOrderList.SelectedItems == null)
                return;
            int selectItemsCount = radGridViewOrderList.SelectedItems.Count;
            for (int i = selectItemsCount - 1; i >= 0; i--)
            {
                CP_DoctorOrder order = radGridViewOrderList.SelectedItems[i] as CP_DoctorOrder;
                if (order.Yzzt == (decimal)OrderStatus.OrderInptut || order.Yzzt == 0)
                {
                    radGridViewOrderList.Items.Remove(order);
                    m_AddOrder.Remove(order);
                    if (order.FromTable != string.Empty || order.FromTable != "CP_AdviceSuitDetail")//代表不是新增
                    {
                        m_DelOrder.Add(order);
                    }
                }
            }
        }
        /// <summary>
        /// 右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rowContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            GridViewRow row = ((RadRoutedEventArgs)e).OriginalSource as GridViewRow;
            List<CP_AdviceGroupDetail> listsOrder = new List<CP_AdviceGroupDetail>();
            var RadMenu = sender as RadContextMenu;
            Boolean isSelectItemsHaveDefferentGroup = false;
            Boolean isSelectItemsHaveGroupItem = false;
            foreach (RadMenuItem item in RadMenu.Items)
            {
                if (row != null && !row.IsSelected)
                {
                    item.IsEnabled = false;
                }
                else
                {
                    if (item.Tag != null)
                    {
                        if ((TagName)item.Tag == TagName.Edit)
                        {
                            item.IsEnabled = !(this.radGridViewOrderList.SelectedItems.Count > 1);
                        }
                        if ((TagName)item.Tag == TagName.Del)
                        {
                            item.IsEnabled = !(this.radGridViewOrderList.SelectedItems.Count > 1);
                        }
                        if ((TagName)item.Tag == TagName.Group)
                        {
                            #region 判断选中的数据中是否同时包含长期医嘱和临时医嘱，是否包含已成组项目
                            for (int i = 0; i < radGridViewOrderList.SelectedItems.Count; i++)
                            {
                                if (i > 0 && !isSelectItemsHaveDefferentGroup)
                                    isSelectItemsHaveDefferentGroup =
                                        ((CP_AdviceGroupDetail)radGridViewOrderList.SelectedItems[i - 1]).Yzbz != ((CP_AdviceGroupDetail)radGridViewOrderList.SelectedItems[i]).Yzbz;
                                if (((CP_AdviceGroupDetail)radGridViewOrderList.SelectedItems[i]).Fzbz != 3500) isSelectItemsHaveGroupItem = true;
                            }
                            #endregion
                            item.IsEnabled = (this.radGridViewOrderList.SelectedItems.Count > 1 && !isSelectItemsHaveDefferentGroup && !isSelectItemsHaveGroupItem);
                        }
                    }
                }
            }
        }
        /// <summary>
        /// 若要使用，在rowloaded事件里注册,暂时保留
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnMouseLeftDown(object sender, MouseButtonEventArgs e)
        {
            //this.AddHandler(GridViewRow.MouseLeftButtonDownEvent,
            //    new MouseButtonEventHandler(OnMouseLeftDown),true);
            var clickedElement = e.OriginalSource as FrameworkElement;
            var parentCell = clickedElement.ParentOfType<GridViewCell>();
            if (parentCell != null && parentCell.Column is GridViewSelectColumn)
            {
                var parentCheckBox = clickedElement.ParentOfType<CheckBox>();
                if (parentCheckBox != null)
                {
                    // checkbox was clicked
                }
            }
        }
        #endregion
        #region private methods
        /// <summary>
        ///  EnableButtonState
        /// </summary>
        private void EnableButtonState(Boolean isEnable)
        {
            this.btnSave.IsEnabled = isEnable;
            this.btnNext.IsEnabled = isEnable;
            this.btnComplete.IsEnabled = isEnable;
            this.btnDelOrder.IsEnabled = isEnable;
            this.btnNewOrder.IsEnabled = isEnable;
            this.btnModifyOrder.IsEnabled = isEnable;
            this.btnQuit.IsEnabled = isEnable;
            this.btnLeadIn.IsEnabled = isEnable;
        }
        /// <summary>
        /// 设置查看两BUTTON的可见性
        /// </summary>
        /// <param name="isShow">是否可见</param>
        private void SetViewButton(Boolean isShow)
        {
            btnViewPre.IsEnabled = isShow;
            btnViewNext.IsEnabled = isShow;
            //if (isShow)
            //{
            //    btnViewPre_Click.Visibility = System.Windows.Visibility.Visible;
            //    btnViewNext.Visibility = System.Windows.Visibility.Visible;
            //}
            //else
            //{
            //    btnViewPre_Click.Visibility = System.Windows.Visibility.Collapsed;
            //    btnViewNext.Visibility = System.Windows.Visibility.Collapsed;
            //}
        }
        /// <summary>
        /// 设置下下步，与保存的可用性
        /// t与病患状态的并发判断，如退出和完成时，怎么都不能用
        /// </summary>
        /// <param name="isEnable"></param>
        private void SetNextButton(Boolean isEnable)
        {
            btnNext.IsEnabled = isEnable;
            btnSave.IsEnabled = isEnable;
            InitButtoState();
        }
        private void SetButtonEnable(Activity a)
        {
            if (a == null)
                SetNextButton(false);
            if (a.Type == ActivityType.COMPLETION)
            {
                this.btnNext.IsEnabled = false;
                this.btnComplete.IsEnabled = true;
            }
            else
            {
                this.btnNext.IsEnabled = true;
                this.btnComplete.IsEnabled = false;
            }
            if (a.Type == ActivityType.AUTOMATION) //循环结点
            {
                SetViewButton(true);
                if (a.CurrentElementState == WorkFlow.ElementState.Pre)
                {
                    SetNextButton(false);
                }
                else if (a.CurrentElementState == WorkFlow.ElementState.Now)
                {
                    if (a.CurrentViewActiveChildren.CurrentElementState == WorkFlow.ElementState.Now)
                    {
                        SetNextButton(true);
                    }
                    else
                    {
                        SetNextButton(false);
                    }
                }
                else if (a.CurrentElementState == WorkFlow.ElementState.Next)
                {
                    SetNextButton(false);
                }
                else
                {
                    SetNextButton(false);
                }
            }
            else
            {
                SetViewButton(false);
                if (a.CurrentElementState == WorkFlow.ElementState.Pre)
                {
                    SetNextButton(false);
                }
                else if (a.CurrentElementState == WorkFlow.ElementState.Now)
                {
                    SetNextButton(true);
                    if (a.Type == ActivityType.COMPLETION)
                    {
                        btnNext.IsEnabled = false;
                        btnSave.IsEnabled = true;
                        InitButtoState();
                    }
                }
                else if (a.CurrentElementState == WorkFlow.ElementState.Next)
                {
                    SetNextButton(false);
                }
                else
                {
                    SetNextButton(false);
                }
            }
        }
        /// <summary>
        /// 根据结点类型和当前状态设置Next,Confirm Button的可见性
        /// </summary>
        /// <param name="activity"></param>
        private void SetNextButtonView(Activity activity)
        {
            if (activity.CurrentElementState == WorkFlow.ElementState.Hide)
            {
                btnNext.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Collapsed;
            }
            else if (activity.CurrentElementState == WorkFlow.ElementState.Next)
            {
                if (activity.Type == ActivityType.COMPLETION)
                {
                    btnNext.Visibility = System.Windows.Visibility.Collapsed;
                    btnSave.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    btnNext.Visibility = System.Windows.Visibility.Visible;
                    btnSave.Visibility = System.Windows.Visibility.Visible;
                }
            }
            else if (activity.CurrentElementState == WorkFlow.ElementState.Now)
            {
                if (activity.Type == ActivityType.COMPLETION)
                {
                    btnNext.Visibility = System.Windows.Visibility.Collapsed;
                    btnSave.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    btnNext.Visibility = System.Windows.Visibility.Visible;
                    btnSave.Visibility = System.Windows.Visibility.Collapsed;
                }
            }
            else if (activity.CurrentElementState == WorkFlow.ElementState.Pre)
            {
                btnNext.Visibility = System.Windows.Visibility.Collapsed;
                btnSave.Visibility = System.Windows.Visibility.Visible;
            }
        }
        /// <summary>
        /// 获取当前工作流路径代码集合
        /// </summary>
        /// <returns></returns>
        private ObservableCollection<String> GetListLjdm()
        {
            ObservableCollection<String> listLjdm = new ObservableCollection<string>();
            foreach (Flow flow in m_WorkFlow.Flows)
            {
                if (!listLjdm.Contains(flow.UniqueID))
                    listLjdm.Add(flow.UniqueID);
            }
            return listLjdm;
        }
        #endregion
        #region completed events
        #endregion
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
        #region 绑定列表框
        /// <summary>
        /// 获取个人医嘱套餐
        /// </summary>
        private void GetAdviceSuitPersonal()
        {
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            ServiceClient.GetCP_AdviceSuitCompleted +=
                (obj, e) =>
                {
                    if (e.Error != null)
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                    else
                    {
                        //个人医嘱
                        m_CP_AdviceSuitPersonal = e.Result.ToList();
                    }
                    if (this.radBusyIndicator != null)
                        this.radBusyIndicator.IsBusy = false;
                };
            if (this.radBusyIndicator != null) this.radBusyIndicator.IsBusy = true;
            ServiceClient.GetCP_AdviceSuitAsync(String.Format(" and Syfw=2903 and Ysdm='{0}'", Global.LogInEmployee.Zgdm));
            ServiceClient.CloseAsync();
        }
        /// <summary>
        /// 获取科室医嘱套餐
        /// </summary>
        private void GetAdviceSuitDepartment()
        {
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            ServiceClient.GetCP_AdviceSuitCompleted +=
                (obj, e) =>
                {
                    if (e.Error != null)
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                    else
                    {
                        //科室医嘱
                        m_CP_AdviceSuitDepartment = e.Result.ToList();
                        lstAdviceSuit.ItemsSource = e.Result.ToList();
                    }
                    if (this.radBusyIndicator != null) this.radBusyIndicator.IsBusy = false;
                };
            if (this.radBusyIndicator != null) this.radBusyIndicator.IsBusy = true;
            ServiceClient.GetCP_AdviceSuitAsync(String.Format(" and Syfw=2901 and Ksdm='{0}'", Global.LogInEmployee.Ksdm));
            ServiceClient.CloseAsync();
        }
        #endregion
        #region 控件间数据拖放
        private void tvList_PreviewDragEnded(object sender, RadTreeViewDragEndedEventArgs e)
        {
            e.Handled = true;
        }
        private void InitDragDrop()
        {
            //初始化事件
            //RadDragAndDropManager.AddDragQueryHandler(lstAdviceSuit, OnDragQuery);
            //RadDragAndDropManager.AddDragInfoHandler(lstAdviceSuit, OnDragInfo);
            //RadDragAndDropManager.AddDragQueryHandler(tvList, OnDragQuery);
            //RadDragAndDropManager.AddDragInfoHandler(tvList, OnDragInfo);
            RadDragAndDropManager.AddDropQueryHandler(radGridViewOrderList, OnDropQuery);
            RadDragAndDropManager.AddDropInfoHandler(radGridViewOrderList, OnDropInfo);
        }
        private void OnDragQuery(object sender, DragDropQueryEventArgs e)
        {
            //System.Windows.Controls.ListBox listBox = sender as System.Windows.Controls.ListBox;
            //if (listBox != null)
            //{
            //    IList selectedItems = listBox.SelectedItems.Cast<object>().ToList();
            //    e.Options.Payload = selectedItems;
            //}

            RadTreeView treeView = sender as RadTreeView;
            if (treeView != null)
            {
                IList selectedItems = treeView.SelectedItems.Cast<object>().ToList();
                e.Options.Payload = selectedItems;
            }
            e.QueryResult = true;
            e.Handled = true;
        }
        private void OnDragInfo(object sender, DragDropEventArgs e)
        {
            //System.Windows.Controls.ListBox listBox = sender as System.Windows.Controls.ListBox;
            RadTreeView treeview = sender as RadTreeView;
            IEnumerable draggedItems = e.Options.Payload as IEnumerable;
            if (e.Options.Status == DragStatus.DragInProgress)
            {
                //创建拖拽数据项
                TreeViewDragCue cue = new TreeViewDragCue();
                //cue.ItemTemplate = listBox.ItemTemplate;
                //cue.ItemTemplate = treeview.ItemTemplate;
                cue.ItemsSource = draggedItems;
                e.Options.DragCue = cue;
            }
            else if (e.Options.Status == DragStatus.DragComplete)
            {
                //IList source = listBox.ItemsSource as IList;
                //foreach (object draggedItem in draggedItems)
                //{
                //    source.Remove(draggedItem);
                //}
            }
        }
        private void OnDropQuery(object sender, DragDropQueryEventArgs e)
        {
            ////在自己控件上不能放下
            if (e.Options.Source.Name == "tvList")
            {
                return;
            }
            ////获取拖拽的数据
            //ICollection draggedItems = e.Options.Payload as ICollection;
            //bool result = draggedItems.Cast<object>().All((object item) => item is CP_AdviceSuit);
            //e.QueryResult = result;
            e.Handled = true;
            e.QueryResult = true;

        }
        private void OnDropInfo(object sender, DragDropEventArgs e)
        {
            //if (e.Options.Source is GridViewHeaderCell)
            //{
            //    return;
            //}
            if (((RadTreeViewItem)tvList.SelectedItem).Tag is CP_AdviceSuitCategory)
            {
                return;
            }
            ICollection draggedItems = e.Options.Payload as ICollection;
            Collection<Object> payload = e.Options.Payload as Collection<Object>;

            //获取拖拽的数据
            TreeViewDragCue cue = e.Options.DragCue as TreeViewDragCue;
            if (e.Options.Status == DragStatus.DropPossible)
            {
                //设置拖拽时提示
                cue.DragActionContent = String.Format("添加{0}个医嘱套餐到病人医嘱列表", draggedItems.Count);
                cue.IsDropPossible = true;
                this.radGridViewOrderList.Background = this.Resources["DropPossibleBackground"] as Brush; //设置目标源背景色(拖拽状态)
            }
            else if (e.Options.Status == DragStatus.DropImpossible)
            {
                cue.DragActionContent = null;
                cue.IsDropPossible = false;
            }
            if (e.Options.Status == DragStatus.DropComplete)  //放下
            {
                foreach (object item in payload)
                {
                    RadTreeViewItem treeViewItem = (item as RadTreeViewItem);
                    CP_AdviceSuit draggedItem = treeViewItem.Tag as CP_AdviceSuit;
                    GetDrapDropAdviceSuitDetailList(draggedItem.Ctyzxh);
                }

                //foreach (CP_AdviceSuit draggedItem in payload)
                //{
                //    //PublicMethod.RadAlterBox(draggedItem.Ctyzxh.ToString(), "提示");
                //    GetDrapDropAdviceSuitDetailList(draggedItem.Ctyzxh);
                //}
            }
            if (e.Options.Status != DragStatus.DropPossible)
            {
                this.radGridViewOrderList.Background = new SolidColorBrush(Colors.White);//设置目标源背景色（非拖拽状态）
            }
        }
        #region 拖拽套餐的详细数据
        /// <summary>
        /// 获取拖拽套餐的详细数据
        /// </summary>
        /// <param name="Ctyzxh">医嘱套餐序号</param>
        private void GetDrapDropAdviceSuitDetailList(decimal Ctyzxh)
        {
            this.radBusyIndicator.IsBusy = true;
            YidanEHRDataServiceClient DragDropClient = PublicMethod.YidanClient;
            DragDropClient.GetDrapDropAdviceSuitDetailCompleted +=
                (obj, e) =>
                {
                    try
                    {
                        if (e.Error == null)
                        {
                            ObservableCollection<CP_DoctorOrder> lstCP_DoctorOrder = e.Result;
                            ObservableCollection<CP_DoctorOrder> listOrder = this.radGridViewOrderList.ItemsSource as ObservableCollection<CP_DoctorOrder>;
                            this.radGridViewOrderList.ItemsSource = null;
                            foreach (CP_DoctorOrder order in lstCP_DoctorOrder)
                            {
                                listOrder.Add(order);
                            }
                            //m_GridCheckBox.Clear();
                            this.radGridViewOrderList.ItemsSource = listOrder;
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        PublicMethod.ClientException(ex, this.GetType().FullName, true);
                    }
                    this.radBusyIndicator.IsBusy = false;
                };
            DragDropClient.GetDrapDropAdviceSuitDetailAsync(Ctyzxh, Global.InpatientListCurrent.Syxh.ToString(), Global.LogInEmployee);
            DragDropClient.CloseAsync();
        }
        #endregion
        #endregion
        #region 绑定弹出提示框的内容
        /// <summary>
        ///表示插入提示信息的方法
        /// </summary>
        /// <returns>插入是否成功</returns>
        public void InsertAndSelectCP_MedicalTreatmentWarmGroupByTxlx()
        {
            Object[] objArr = new Object[2];
            List<CP_MedicalTreatmentWarmGroupByTxlx> CP_MedicalTreatmentWarmTemps = new List<CP_MedicalTreatmentWarmGroupByTxlx>();
            List<CP_MedicalTreatmentWarmGroupByTxlx> CP_MedicalTreatmentWarmGroupByTxlxTemps = new List<CP_MedicalTreatmentWarmGroupByTxlx>();
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            ServiceClient.InsertAndSelectCP_MedicalTreatmentWarmGroupByTxlxCompleted += (sender, e) =>
            {
                CP_MedicalTreatmentWarmGroupByTxlxTemps = e.Result.ToList();
                BindmnuWarmContent(CP_MedicalTreatmentWarmGroupByTxlxTemps);
            };
            ServiceClient.InsertAndSelectCP_MedicalTreatmentWarmGroupByTxlxAsync(Global.InpatientListCurrent.Syxh.ToString(), Global.InpatientListCurrent.Ljxh.ToString(), Global.InpatientListCurrent.Ljdm, m_WorkFlow.Activitys.CurrentActivity.UniqueID);
            ServiceClient.CloseAsync();
        }
        /// <summary>
        /// 表示绑定提示框中的数据的方法
        /// </summary>
        /// <param name="CP_MedicalTreatmentWarmGroupByTxlxTemp"></param>
        public void BindmnuWarmContent(List<CP_MedicalTreatmentWarmGroupByTxlx> CP_MedicalTreatmentWarmGroupByTxlxTemp)
        {
            //grdWarmContent.ItemsSource = null;
            List<KeyValue> keyvalues = new List<KeyValue>();
            foreach (var item in CP_MedicalTreatmentWarmGroupByTxlxTemp)
            {
                KeyValue keyValue = new KeyValue();
                keyValue.Value = "节点【" + item.jdmc + "】存在待阅【" + item.TxlxName + "】数量【" + item.Dysl + "】总数量【" + item.txsl + "】";
                keyvalues.Add(keyValue);
            }
            grdWarmContent.ItemsSource = keyvalues;
        }
        void lst_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //throw new NotImplementedException();
        }
        #endregion
        #region 生成树 add by dxj 2011/7/14

        /// <summary>
        /// 获取医嘱套餐
        /// </summary>
        private void GetCP_AdviceSuitCategory()
        {
            try
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                CP_AdviceSuitCategory AdviceSuitCategory = new CP_AdviceSuitCategory();
                AdviceSuitCategory.CategoryId = String.Empty;
                AdviceSuitCategory.Name = String.Empty;
                AdviceSuitCategory.Memo = String.Empty;
                AdviceSuitCategory.Zgdm = String.Empty;
                String where = String.Empty;
                if (Syfw == "2903")//个人套餐
                {
                    where = String.Format(" and Syfw={0} and Ysdm='{1}'", Syfw, Global.LogInEmployee.Zgdm);
                }
                if (Syfw == "2901")//科室套餐
                {
                    where = String.Format(" and Syfw={0} and Ksdm='{1}'", Syfw, Global.LogInEmployee.Ksdm);
                }
                client.InsertAndSelectCP_AdviceSuitCategoryCompleted += (obj, ea) =>
                {
                    if (ea.Error == null)
                    {
                        if (tvList != null)
                            tvList.Items.Clear();
                        AddTreeView(String.Empty, null, ea.Result.ToList(), ea.Result.ToList());
                    }
                };
                client.InsertAndSelectCP_AdviceSuitCategoryAsync(AdviceSuitCategory, where);
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 创建树
        /// </summary>
        /// <param name="parentId">父类编号</param>
        /// <param name="subitem">当前树节点</param>
        /// <param name="CategoryList">数据源</param>
        /// <param name="sunCategoryList">符合条件的数据源</param>
        private void AddTreeView(String parentId, RadTreeViewItem subitem, List<CP_AdviceSuitCategory> CategoryList, List<CP_AdviceSuitCategory> sunCategoryList)
        {
            if (CategoryList == null)
            {
                return;
            }
            foreach (CP_AdviceSuitCategory row in sunCategoryList.Where(c => c.ParentID.Equals(parentId)))
            {
                RadTreeViewItem item1 = new RadTreeViewItem();
                item1.Header = row.Name;
                item1.Tag = row;

                if (subitem == null)
                {
                    tvList.Items.Add(item1);
                }
                else
                {
                    subitem.Items.Add(item1);
                }
                if (row.AdviceSuitList != null)
                {
                    foreach (CP_AdviceSuit suit in row.AdviceSuitList)
                    {
                        RadTreeViewItem item2 = new RadTreeViewItem();
                        item2.Foreground = ConvertColor.GetColorBrushFromHx16("ff0000");
                        item2.Header = suit.Name;
                        item2.Tag = suit;
                        item1.Items.Add(item2);
                    }
                }
                AddTreeView(row.CategoryId, item1, CategoryList, CategoryList.Where(c => c.ParentID.Equals(row.CategoryId)).ToList());
            }
        }

        #endregion
        #region 关键药物
        public void GetMasterDrugs()
        {
            YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
            Client.MaintainCP_MasterDrugCompleted += (s, e) =>
            {
                if (e.Error == null)
                {
                    cP_MasterDrugs = new List<CP_MasterDrugs>();
                    cP_MasterDrugs = e.Result.ToList();
                }
            };
            Client.MaintainCP_MasterDrugAsync(new CP_MasterDrugs(), Operation.Select.ToString());

        }
        public void CheckMasterDrugs_Save()
        {
            List<CP_MasterDrugs> CP_MasterDrugsTemp = new List<CP_MasterDrugs>();

            foreach (var item in cP_MasterDrugs)
            {
                item.IsPass = false;
                foreach (var Order in radGridViewOrderList.SelectedItems)
                {
                    if (item.Cdxh == ((CP_DoctorOrder)Order).Cdxh.ToString())
                    {
                        item.IsNeedPass = true;
                        //item.IsPass=true;
                        CP_MasterDrugsTemp.Add(item);
                    }
                }
            }
            if (CP_MasterDrugsTemp.Count > 0)
            {
                RWMasterDrugAuthorize Authorize = new RWMasterDrugAuthorize(CP_MasterDrugsTemp);
                Authorize.Closed += new EventHandler<WindowClosedEventArgs>(Authorize_Closed);
                Authorize.ShowDialog();
            }
            else
            {
                Save();
            }

        }

        void Authorize_Closed(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
                Save();
        }

        public void Save()
        {
            string strWaring = string.Empty;
            if (Check(out strWaring))
            {
                SaveEnforceInfo();
            }
            else
            {
                DialogParameters parameters = new DialogParameters();
                parameters.Closed = OnExceptionOrderReason;//***close处理***
                PublicMethod.RadQueryBox(parameters, strWaring, m_StrTitle);
            }
        }
        #endregion


        #endregion
    }

}
