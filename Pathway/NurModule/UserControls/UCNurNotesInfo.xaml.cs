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
using YidanEHRApplication.Helpers;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Views;
using YidanEHRApplication.WorkFlow.Designer;
using YidanEHRApplication.Models;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using YidanEHRApplication.Views.ChildWindows;
using YidanEHRApplication.WorkFlow;

namespace YidanEHRApplication.NurModule.UserControls
{
    public partial class UCNurNotesInfo : UserControl
    {
        #region 事件
        #endregion
        #region 变量
        /// <summary>
        /// 设置或获取一个值该值指示流程图空间是否显示
        /// </summary>
        private Boolean IsWorkFlowContainShow = false;
        #endregion
        #region members

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
        /// 当前病患
        /// </summary>
        private InpatientList m_CurrentInpatientInfo;



        #endregion
        public UCNurNotesInfo()
        {
            InitializeComponent();
        }
        #region
        //public event InitEnforceInfoCompleteEventHandler InitEnforceInfoComplete;

        //protected virtual void OnInitEnforceInfoComplete(RoutedEventArgs e)
        //{
        //   if (InitEnforceInfoComplete != null)
        //      InitEnforceInfoComplete(this,e);
        //}
        #endregion

        #region 在导航里调用的函数
        /// <summary>
        /// 构造函数里必须调用函数集合
        /// </summary>
        private void IntiCtor()
        {
            InitWorkFlow(Global.InpatientListCurrent.EnForceWorkFlowXml);
        }

        #region methods

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
                this.gridWorkFlowShow.Height = 200;
                this.gridWorkFlowShow.Children.Add(containerEdit);
                m_WorkFlow.ContainerEdit = containerEdit;
                m_WorkFlow.Activitys.WorkFlow_ActivitySelectChanged += new WorkFlow.WorkFlow_ActivetySelectedDelegateEventHandler(Activitys_WorkFlow_ActivitySelectChanged);
            }
            catch (Exception ex)
            {
                EnableButtonState(false);
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #endregion

        #endregion

        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
        }

        public void InitDate(CP_InpatinetList cp)
        {
            if (IsSelectPatient(true))
            {
                InitPage();
                IntiCtor();
            }
        }

        /// <summary>
        /// 判断是否选中病患
        /// </summary>
        /// <returns></returns>
        private Boolean IsSelectPatient(Boolean isNeedAlert)
        {
            m_IsInpath = true;
            if (Global.InpatientListCurrent == null)
            {
                m_IsInpath = false;
                PublicMethod.RadAlterBox("请选择在路径中的病患", m_StrTitle);
            }
            //未进入路径的患者要先评估后刷新
            else if (string.IsNullOrEmpty(Global.InpatientListCurrent.Ljdm))
            {
                m_IsInpath = false;
                //AccessNewPath();
            }

            if (!m_IsInpath)
            {
                if (Global.InpatientListCurrent == null || string.IsNullOrEmpty(Global.InpatientListCurrent.Ljdm))
                {
                    if (isNeedAlert)
                        PublicMethod.RadAlterBox("请选择在路径中的病患", m_StrTitle);
                    EnableButtonState(false);
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
            InitButtoState();

        }

        #region   入院评估

        //private void AccessNewPath()              //zm    8.4 注释
        //{
        //    RWAccessPath accessWindow = new RWAccessPath(Global.InpatientListCurrent);
        //    accessWindow.Closed += new EventHandler<WindowClosedEventArgs>(accessWindow_Closed);
        //    accessWindow.ShowDialog();

        //}

        private void accessWindow_Closed(object sender, WindowClosedEventArgs e)
        {try{
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

            } }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }

    

        #endregion

        #region 在loaded事件中调用的methods,to do 完成后将成员变量移到members define 区
        private const String m_PatientInfoInfo = "病人姓名：{0}  病历号:{1}  性别:{2}  年龄:{3}  出生日期:{4}  管床医师:{5}";
        private const String m_PathInfoInfo = "管床医师:  {0}         路径名称:  {1}         当前步骤:  {2}";
        /// <summary>
        /// 初始化基本信息
        /// </summary>
        private void IntiPatinetInfo()
        {
            try
            {
                m_IsLeadIn = false;
                //this.textBlockPatient.Text = String.Format(m_PatientInfoInfo, Global.InpatientListCurrent.Hzxm, Global.InpatientListCurrent.Zyhm, Global.InpatientListCurrent.Brxb,
                //                                                 Global.InpatientListCurrent.Xsnl, Global.InpatientListCurrent.Csrq, Global.InpatientListCurrent.Zyys);



                this.textname.Text = "患者姓名:  " + Global.InpatientListCurrent.Hzxm;
                this.textbed.Text = "床位:  " + Global.InpatientListCurrent.Bed;
                this.txtZyhm.Text = "病历号:  " + Global.InpatientListCurrent.Zyhm;
                this.txtBrxb.Text = "性别:  " + Global.InpatientListCurrent.Brxb;
                this.txtXsnl.Text = "年龄:  " + Global.InpatientListCurrent.Xsnl;
                this.txtCsrq.Text = "出生日期:  " + Global.InpatientListCurrent.Csrq;
                this.txtRyrq.Text = "入院日期:  " + Global.InpatientListCurrent.Ryrq.Substring(0, 10);


                System.TimeSpan timeSpan = DateTime.Now.Subtract(Convert.ToDateTime(Global.InpatientListCurrent.Ryrq));
                //this.textBlockPath.Text = String.Format(m_PathInfoInfo, Global.InpatientListCurrent.Ryzd, Global.InpatientListCurrent.Ljmc,
                //                            Global.InpatientListCurrent.Ljmc, timeSpan.Days.ToString(), Global.InpatientListCurrent.Ljts);

                this.textBlockPath.Text = String.Format(m_PathInfoInfo, Global.InpatientListCurrent.Zyys, Global.InpatientListCurrent.Ljmc,
                                                             Global.InpatientListCurrent.Ljts);

                this.txtDays.Text = "住院天数:  " + timeSpan.Days.ToString();
                this.txtRyzd.Text = "入院诊断:  " + Global.InpatientListCurrent.Ryzd;



            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
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

        private void Activitys_WorkFlow_ActivitySelectChanged(Activity a)
        {
            //首先判断结点类型
            //分四种情况:下一步，查看分三钟:直接上一步，循环的二种
            try
            {
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

        }

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
                client.GetPathInitOrderAsync(activity.CurrentViewActiveChildren.ActivityUniqueID, Global.InpatientListCurrent.Syxh, Global.LogInEmployee, Global.InpatientListCurrent.Ljdm);
                client.GetPathInitOrderCompleted += (s, e) =>
                {
                    radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        ObservableCollection<CP_DoctorOrder> listOrder = e.Result;
                        foreach (CP_DoctorOrder order in listOrder)
                        {
                            order.Lrysdm = Global.LogInEmployee.Zgdm;
                        }
                        //radGridViewOrderList.ItemsSource = listOrder;

                        GetNurPathBasicInfo(activity, isInit);
                        GetNurNotesInfo();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            }
            else
            {
                client.GetPathEnforcedOrderAsync(activity.CurrentViewActiveChildren.ActivityChildrenID, Global.InpatientListCurrent.Syxh, Global.InpatientListCurrent.Ljdm);
                client.GetPathEnforcedOrderCompleted += (s, e) =>
                   {
                       radBusyIndicator.IsBusy = false;
                       if (e.Error == null)
                       {
                           //this.radGridViewOrderList.ItemsSource = e.Result;

                           GetNurPathBasicInfo(activity, isInit);
                           GetNurNotesInfo();
                       }
                       else
                       {
                           PublicMethod.RadWaringBox(e.Error);
                       }
                   };
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



        #region 文字医嘱相关
        ObservableCollection<CP_NurExecInfo> m_NurExecInfos;
        /// <summary>
        /// 获取护理执行基本信息并显示
        /// </summary>
        private void GetNurPathBasicInfo(Activity activity, bool isInit)
        {

            //this.stpInfo.Children.Clear();
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.GetNurPathBasicInfoAsync(Global.InpatientListCurrent, activity.CurrentViewActiveChildren.ActivityUniqueID, activity.CurrentViewActiveChildren.ActivityChildrenID);
            client.GetNurPathBasicInfoCompleted += (s, t) =>
            {
                radBusyIndicator.IsBusy = false;
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
                        //this.stpInfo.Children.Add(exec);
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
        #endregion

        #region 体征录入

        /// <summary>
        /// 体征录入控件
        /// </summary>
        private void GetNurNotesInfo()
        {
            notesInfo.Children.Clear();
            UCNurNotesRecord notesRecord = new UCNurNotesRecord();
            notesRecord.CurrentActivity = m_WorkFlow.Activitys.CurrentViewActivity;
            //add by luff 20130308 判断病人是否出院，控制相关操作按钮显示
            if (Global.InpatientListCurrent.Status == "1503")
            {
                notesRecord.rbtnAdd.IsEnabled = false;
                notesRecord.rbtnCancel.IsEnabled = false;
            }
             
            //notesRecord.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            notesRecord.Height = notesInfo.Height - 10;
            notesRecord.rgvVitalSignsRecord.Height = notesInfo.Height - 40;
            notesInfo.Children.Add(notesRecord);
            
        }
        #endregion


        #endregion  初始化病患基本信息和当前步骤基本医嘱信息

        #region 保存
        /// <summary>
        /// 跳转后的保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void buttonConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                radBusyIndicator.IsBusy = true;
                SaveNurInfo();
            }
            catch (Exception ex)
            {
                radBusyIndicator.IsBusy = false;
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        private void SaveNurInfo()
        {

            ObservableCollection<CP_NurExecInfo> listInfo = new ObservableCollection<CP_NurExecInfo>();
            //foreach (Control ctl in stpInfo.Children)
            //{
            //    if (ctl.GetType() == typeof(UCNurExecItem))
            //    {
            //        ((UCNurExecItem)ctl).NurExecInfo.ToList().ForEach(s =>
            //        {
            //            s.Ljxh = Global.InpatientListCurrent.Ljxh;
            //            s.Ljdm = Global.InpatientListCurrent.Ljdm;
            //            s.PathDetailId = m_WorkFlow.Activitys.CurrentViewActivity.CurrentViewActiveChildren.ActivityChildrenID;
            //            listInfo.Add(s);
            //        });
            //    }
            //}
            if (!IsNeedCheck(listInfo))
            {
                PublicMethod.RadAlterBox("资料没有变化，无需保存", "提示");
                if (radBusyIndicator.IsBusy)
                    radBusyIndicator.IsBusy = false;
                return;
            }
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.SaveNurExecRecordAsync(listInfo, Global.InpatientListCurrent);
            client.SaveNurExecRecordCompleted += (s, e) =>
               {
                   if (e.Error == null)
                   {
                      
                       GetNurPathBasicInfo(m_WorkFlow.Activitys.CurrentViewActivity, true);
                       PublicMethod.RadAlterBox("保存成功", "提示");
                   }
                   else
                   {
                       PublicMethod.RadWaringBox(e.Error);
                   }

               };
            client.CloseAsync();
        }

        private Boolean IsNeedCheck(ObservableCollection<CP_NurExecInfo> listInfo)
        {
            foreach (CP_NurExecInfo info in listInfo)
            {
                if (!info.IsNew && info.IsModify)
                {
                    //目前只有选中不选中之分
                    if (!info.IsSelected)
                    {
                        return true;
                    }
                }
                else if (info.IsNew)
                {
                    if (info.IsSelected)
                        return true;
                }
            }
            return false;
        }


        #endregion

        #region  查看详细信息, 医嘱列表 ,循环结点上一步、下一步
        /// <summary>
        /// 查看详细信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButtonShow_Click(object sender, RoutedEventArgs e)
        {
            RWPatientInfo detailInfo = new RWPatientInfo();
            detailInfo.ShowDialog();
        }

        /// <summary>
        /// 医嘱列表
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButtonAdviceList_Click(object sender, RoutedEventArgs e)
        {
            RWAdviceList adviceList = new RWAdviceList(Global.InpatientListCurrent.Syxh);
            adviceList.ShowDialog();

        }


        /// <summary>
        /// 查看上一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radbuttonViewPre_Click(object sender, RoutedEventArgs e)
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

        /// <summary>
        /// 查看下一步
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radbuttonViewNext_Click(object sender, RoutedEventArgs e)
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
        #endregion

        #region 显示/隐藏执行路径
        private void Expander_Expanded(object sender, RoutedEventArgs e)
        {
            this.gridWorkFlowShow.Height = 0;
            expShowImage.Header = "显示路径";
            LayoutRoot.RowDefinitions[1].Height = new GridLength(0);
            IsWorkFlowContainShow = false;
        }
        private void Expander_Collapsed(object sender, RoutedEventArgs e)
        {
            this.gridWorkFlowShow.Height = 200;

            expShowImage.Header = "隐藏路径";
            LayoutRoot.RowDefinitions[1].Height = new GridLength(200);
            IsWorkFlowContainShow = true;
        }
        #endregion

        #region private methods

        /// <summary>
        ///  EnableButtonState
        /// </summary>
        private void EnableButtonState(Boolean isEnable)
        {
            //this.buttonConfirm.IsEnabled = isEnable;
        }

        /// <summary>
        /// 设置查看两BUTTON的可见性
        /// </summary>
        /// <param name="isShow">是否可见</param>
        private void SetViewButton(Boolean isShow)
        {
            radbuttonViewPre.IsEnabled = isShow;
            radbuttonViewNext.IsEnabled = isShow;
            //if (isShow)
            //{
            //    radbuttonViewPre.Visibility = System.Windows.Visibility.Visible;
            //    radbuttonViewNext.Visibility = System.Windows.Visibility.Visible;
            //}
            //else
            //{
            //    radbuttonViewPre.Visibility = System.Windows.Visibility.Collapsed;
            //    radbuttonViewNext.Visibility = System.Windows.Visibility.Collapsed;
            //}
        }

        /// <summary>
        /// 设置下下步，与保存的可用性
        /// t与病患状态的并发判断，如退出和完成时，怎么都不能用
        /// </summary>
        /// <param name="isEnable"></param>
        private void SetNextButton(Boolean isEnable)
        {
            //buttonConfirm.IsEnabled = isEnable;
            InitButtoState();
        }

        private void SetButtonEnable(Activity a)
        {
            if (a == null)
                SetNextButton(false);

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
                        //buttonConfirm.IsEnabled = true;
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
        //private void SetNextButtonView(Activity activity)
        //{
        //    if (activity.CurrentElementState == WorkFlow.ElementState.Hide)
        //    {
        //        buttonConfirm.Visibility = System.Windows.Visibility.Collapsed;
        //    }
        //    else if (activity.CurrentElementState == WorkFlow.ElementState.Next)
        //    {
        //        if (activity.Type == ActivityType.COMPLETION)
        //        {
        //            buttonConfirm.Visibility = System.Windows.Visibility.Visible;
        //        }
        //        else
        //        {
        //            buttonConfirm.Visibility = System.Windows.Visibility.Visible;
        //        }
        //    }
        //    else if (activity.CurrentElementState == WorkFlow.ElementState.Now)
        //    {
        //        if (activity.Type == ActivityType.COMPLETION)
        //        {
        //            buttonConfirm.Visibility = System.Windows.Visibility.Visible;
        //        }
        //        else
        //        {
        //            buttonConfirm.Visibility = System.Windows.Visibility.Collapsed;
        //        }
        //    }
        //    else if (activity.CurrentElementState == WorkFlow.ElementState.Pre)
        //    {
        //        buttonConfirm.Visibility = System.Windows.Visibility.Visible;
        //    }
        //}

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
    }
}
