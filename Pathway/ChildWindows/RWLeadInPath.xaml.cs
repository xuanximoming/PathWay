using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using YidanEHRApplication.WorkFlow.Designer;
using System.Reflection;
using YidanEHRApplication.WorkFlow;
using YidanEHRApplication.Helpers;

namespace YidanEHRApplication.Views.ChildWindows
{
    /// <summary>
    /// Interaction logic for RadChildWindowLeadInPath.xaml
    /// </summary>
    public partial class RWLeadInPath
    {
        private CP_InpatinetList m_Inpatient = null;

        /// <summary>
        /// 执行界面上的XML
        /// </summary>
        private String m_EnforceXml = String.Empty;

        private String m_EnforceLeadXml = String.Empty;
        /// <summary>
        /// 执行界面上的合并XML
        /// </summary>
        public String EnforceLeadXml
        {
            get
            {
                return m_EnforceLeadXml;
            }
            set
            {
                m_EnforceLeadXml = value;
            }
        }

        /// <summary>
        /// 当前显示的执行路径
        /// </summary>
        private WorkFlow.WorkFlow m_CurrentWorkFlow = new WorkFlow.WorkFlow();

        /// <summary>
        /// 当前显示的引入路径
        /// </summary>
        private WorkFlow.WorkFlow m_LeadWorkFlow = new WorkFlow.WorkFlow();

        public RWLeadInPath()
        {
            InitializeComponent();
        }

        public RWLeadInPath(CP_InpatinetList inpatient, String strEnforceXml)
            : this()
        {
            m_Inpatient = inpatient;
            m_EnforceXml = strEnforceXml;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        { try{
            IntiPage(); }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }

        /// <summary>
        /// 初始化页面
        /// </summary>
        private void IntiPage()
        {
            if (m_Inpatient == null)
                return;
            InitPathList();
            InitCurrentWorkFlow(m_EnforceXml, m_Inpatient.Ljmc, m_Inpatient.Ljdm);
            expanderHide_Expanded(null, null);
        }

        /// <summary>
        /// 路径列表
        /// </summary>
        private void InitPathList()
        {
            try
            {
                //路径引入包含两类，一个是我们推荐的路径，还有一个就是当前科室可使用的路径
                //目前只做了前科室可使用的路径（通过审核)

                radBusyIndicator.IsBusy = true;

                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.GetClinicalPathListCompleted +=
                       (obj, ea) =>
                       {
                           radBusyIndicator.IsBusy = false;
                           if (ea.Error == null)
                           {
                               try
                               {
                                   List<CP_ClinicalPathList> list = new List<CP_ClinicalPathList>();
                                   //list = e.Result.ToList();
                                   //Query syntax:
                                   //list = from info in e.Result.ToList()
                                   //                                  where info.YxjlId.Equals((int)PathShStatus.Review) && info.Ljdm != info.Ljdm
                                   //                                  select info;
                                   ////Method syntax:
                                   list = (ea.Result.ToList().Where(info => info.YxjlId.Equals((int)PathShStatus.Review))).ToList();
                                   for (int i = list.Count - 1; i >= 0; i--)
                                   {
                                       CP_ClinicalPathList info = list[i];
                                       foreach (Flow f in m_CurrentWorkFlow.Flows)
                                       {
                                           if (info.Ljdm == f.UniqueID)
                                           {
                                               list.RemoveAt(i);
                                               break;
                                           }
                                       }
                                   }
                                   radGridViewPathList.ItemsSource = list;
                                   radGridViewPathList.UpdateLayout();
                               }
                               catch (Exception ex)
                               {
                                   PublicMethod.ClientException(ex, this.GetType().FullName, true);
                               }
                           }
                           else
                           {
                               PublicMethod.RadWaringBox(ea.Error);
                           }
                       };
                client.GetClinicalPathListAsync(String.Empty, String.Empty, Global.InpatientListCurrent.Cyks, String.Empty,string.Empty,string.Empty);
                client.CloseAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

       
    

        /// <summary>
        /// 初始化当前路径
        /// </summary>
        /// <param name="strXml"></param>
        /// <param name="strWorkFlowName"></param>
        /// <param name="strWorkFlowID"></param>
        private void InitCurrentWorkFlow(String strXml, String strWorkFlowName, String strWorkFlowID)
        {
            m_CurrentWorkFlow = new WorkFlow.WorkFlow();
            ContainerView containerEdit = new ContainerView();
            //gridWorkFlowCurrent.Width = 1000;
            //containerEdit.Width = 1000;
            containerEdit.WorkFlowUrlName = strWorkFlowName;
            containerEdit.WorkFlowUrlID = strWorkFlowID;
            containerEdit.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            containerEdit.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            containerEdit.WorkFlowXML = strXml;
            this.gridWorkFlowCurrent.Children.Add(containerEdit);
            m_CurrentWorkFlow.ContainerEdit = containerEdit;
        }

        /// <summary>
        /// 初始化引入路径
        /// </summary>
        /// <param name="strXml"></param>
        /// <param name="strWorkFlowName"></param>
        /// <param name="strWorkFlowID"></param>
        private void InitLeadWorkFlow(String strXml, String strWorkFlowName, String strWorkFlowID)
        {
            this.gridLeadIn.Children.Clear();
            m_LeadWorkFlow = new WorkFlow.WorkFlow();
            ContainerShow containerEdit = new ContainerShow();
            //ContainerEdit containerEdit = new ContainerEdit();
            //containerEdit.IsShowFlow = true;
            //containerEdit.IsEditEnable = false;
            containerEdit.WorkFlowUrlName = strWorkFlowName;
            containerEdit.WorkFlowUrlID = strWorkFlowID;
            containerEdit.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
            containerEdit.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;
            containerEdit.WorkFlowXML = strXml;
            gridLeadIn.Height = 300;
            this.gridLeadIn.Children.Add(containerEdit);
            m_LeadWorkFlow.ContainerEdit = containerEdit;
            m_LeadWorkFlow.Activitys.WorkFlow_ActivitySelectChanged += new WorkFlow_ActivetySelectedDelegateEventHandler(Activitys_WorkFlow_ActivitySelectChanged);
        }

        private void Activitys_WorkFlow_ActivitySelectChanged(Activity a)
        {
            try
            {
                if (a != null)
                {
                    m_LeadWorkFlow.Activitys.IsSetColor = false;
                    Activitys activitys = m_LeadWorkFlow.Activitys.SeletAllViewActivitys(a);
                    if (activitys.Count != 0 && !m_IsAlreadyIn)
                        this.radbuttonLeadIn.IsEnabled = true;
                    else
                        this.radbuttonLeadIn.IsEnabled = false;
                    activitys.IsSetColor = true;
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #region 显示/隐藏
        private void expanderHide_Expanded(object sender, RoutedEventArgs e)
        {
            gridWorkFlowCurrent.Visibility = System.Windows.Visibility.Visible;
            this.expanderHide.Header = "隐藏当前路径";
        }

        private void expanderHide_Collapsed(object sender, RoutedEventArgs e)
        {
            gridWorkFlowCurrent.Visibility = System.Windows.Visibility.Collapsed;
            this.expanderHide.Header = "显示当前路径";

        }

        private void expanderHideLead_Expanded(object sender, RoutedEventArgs e)
        {
            gridWorkFlowLead.Visibility = System.Windows.Visibility.Visible;
            this.expanderHideLead.Header = "隐藏引入路径";
        }

        private void expanderHideLead_Collapsed(object sender, RoutedEventArgs e)
        {
            gridWorkFlowLead.Visibility = System.Windows.Visibility.Collapsed;
            this.expanderHideLead.Header = "显示引入路径";
        }
        #endregion

        /// <summary>
        /// grid selectionchanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radGridViewPathList_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {try{
            if (radGridViewPathList != null)
            {
                var list = this.radGridViewPathList.SelectedItem;
                CP_ClinicalPathList info = list as CP_ClinicalPathList;

                InitLeadWorkFlow(info.WorkFlowXML, info.Name, info.Ljdm);
                if (gridWorkFlowLead.Visibility == System.Windows.Visibility.Collapsed)
                {
                    expanderHideLead_Expanded(null, null);
                    expanderHideLead.IsExpanded = true;
                }
            } }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }

        /// <summary>
        /// 己经添加的Activity的uniqId集合
        /// </summary>
        List<String> m_ListActivityUniqId = new List<String>();
        Boolean m_IsAlreadyIn = false;
        private void radbuttonLeadIn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.gridLeadIn.Children.Count() == 0)
                    return;
                m_ListActivityUniqId = new List<String>();
                AddActivitys(m_LeadWorkFlow.Activitys.CurrentViewActivity, true);

                InitCurrentWorkFlow(m_CurrentWorkFlow.WorkFlowXml, m_Inpatient.Ljmc, m_Inpatient.Ljdm);
                this.radbuttonLeadIn.IsEnabled = false;
                this.radbuttonConfirm.IsEnabled = true;
                m_IsAlreadyIn = true;
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 引入所选所有结点
        /// </summary>
        /// <param name="currentActivity"></param>
        /// <param name="isAddFirst"></param>
        private void AddActivitys(Activity currentActivity, bool isAddFirst)
        {
            if (isAddFirst && currentActivity.Type != ActivityType.INITIAL)
                AddNewActivity(currentActivity);
            foreach (Activity a in currentActivity.NextActivitys)
            {
                AddNewActivity(a);

                AddActivitys(a, false);
            }
        }

        /// <summary>
        /// 加入结点
        /// </summary>
        /// <param name="a"></param>
        private void AddNewActivity(Activity a)
        {
            if (!m_ListActivityUniqId.Contains(a.UniqueID))
                m_ListActivityUniqId.Add(a.UniqueID);
            else
            {
                return;
            }
            ActivityType show = a.Type;
            String strShowName = a.ActivityName;
            //m_CurrentWorkFlow.ContainerEdit.CopySelectedControlToMemory(((Activity)a).CloneAllDetails());
            //m_CurrentWorkFlow.ContainerEdit.PastMemoryToContainer();
            Activity clone = ((Activity)a).CloneAllDetails();
            m_CurrentWorkFlow.ContainerEdit.AddActivity(clone);
            foreach (Rule r in clone.BeginRuleCollections)
            {
                m_CurrentWorkFlow.ContainerEdit.AddRule(((Rule)r).CloneAllDetails());
            }
        }

        /// <summary>
        /// 重新引入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radbuttonReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                InitCurrentWorkFlow(m_EnforceXml, m_Inpatient.Ljmc, m_Inpatient.Ljdm);
                this.radbuttonLeadIn.IsEnabled = true;
                this.radbuttonConfirm.IsEnabled = false;
                m_IsAlreadyIn = false;
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 确  定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radbuttonConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                EnforceLeadXml = m_CurrentWorkFlow.ContainerEdit.GetXML(true);
                if (EnforceLeadXml != String.Empty)
                {
                    this.DialogResult = true;
                    this.Close();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
    }
}
