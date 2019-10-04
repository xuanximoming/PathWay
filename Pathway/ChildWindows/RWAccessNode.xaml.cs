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
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using YidanEHRApplication.Views;
using YidanSoft.Tool;
using Telerik.Windows.Controls.GridView;
using System.Collections.ObjectModel;
namespace YidanEHRApplication
{
    public partial class RWAccessNode
    {
        #region 属性变量
        ChildMessageBox messagebox = new ChildMessageBox();
        ChildWindowInputMessage m_InputMessageBox = new ChildWindowInputMessage();
        CP_InpatinetList m_Inpatient;
        String _nodeCode = "";
        #endregion

        public RWAccessNode(CP_InpatinetList patient, String ljdm, String nodeCode)
        {
            InitializeComponent();
            m_Inpatient = patient;
            m_Inpatient.Ljdm = ljdm;
            _nodeCode = nodeCode;

        }
        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_Inpatient != null)
                {
                    //patientBasicInfo1.CurrentPat = m_Inpatient;
                    radBusyIndicator.IsBusy = true;
                    YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                    client.GetAnalysePatient_CanEnterNodeCompleted += (send, ea) =>
                    {
                        m_Inpatient = ((CP_InpatinetList)ea.Result);

                        //ConditionGrid.ItemsSource = m_Inpatient.CurrentCP_ClinicalPathNode.PathEnterJudgeConditions;
                        List<CP_PathEnterJudgeCondition> conditions = new List<CP_PathEnterJudgeCondition>();
                        List<CP_PathEnterJudgeCondition> conditionsDescrib = new List<CP_PathEnterJudgeCondition>();
                        #region 条件分组
                        foreach (CP_PathEnterJudgeCondition item in m_Inpatient.CurrentCP_ClinicalPathNode.PathEnterJudgeConditions)
                        {

                            if (item.Xmlb == 3)
                            { conditionsDescrib.Add(item); }
                            else
                            {
                                if (item.MatchResult != MatchResultState.Match)//不满足条件的靠前显示
                                { conditions.Insert(0, item); }
                                else
                                {
                                    conditions.Add(item);
                                }
                            }
                        }
                        ConditionGrid.ItemsSource = conditions;
                        ConditionGridHand.ItemsSource = conditionsDescrib;
                        #endregion
                        if (!m_Inpatient.CurrentCP_ClinicalPathNode.IsEnter)
                        {
                            //stkReason.Visibility = Visibility.Visible;
                            txtReason.IsEnabled = true;
                        }
                        else 
                        { 
                            //stkReason.Visibility = Visibility.Collapsed; 
                            txtReason.IsEnabled = false;
                        }
                        radBusyIndicator.IsBusy = false;
                    };
                    client.GetAnalysePatient_CanEnterNodeAsync(m_Inpatient, m_Inpatient.Ljdm, _nodeCode);
                    client.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #region 事件
        /// <summary>
        /// 进入
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAccess_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.DialogResult = false;
                ObservableCollection<CP_PatientPathEnterJudgeConditionRecord> CP_PatientPathEnterJudgeConditionRecords = new ObservableCollection<CP_PatientPathEnterJudgeConditionRecord>();
                #region 进入检查
                if (m_Inpatient.CurrentCP_ClinicalPathNode.IsEnter && ConditionGridHand.Items.Count > 0 && ConditionGridHand.SelectedItems.Count < ConditionGridHand.Items.Count)
                {
                    PublicMethod.RadAlterBox("节点进入手动判断条件,必须全部满足，并勾选", "提示");
                    return;
                }
                if (!m_Inpatient.CurrentCP_ClinicalPathNode.IsEnter)
                {
                    if (String.IsNullOrEmpty(txtReason.Text))
                    {
                        PublicMethod.RadAlterBox("请输入不满足条件下进入的原因", "提示");
                        return;
                    }
                    else
                    {
                        CP_PatientPathEnterJudgeConditionRecord RecordTemp2 = new CP_PatientPathEnterJudgeConditionRecord();
                        RecordTemp2.Lb = 2;
                        RecordTemp2.Ljxh = ConvertMy.ToString(m_Inpatient.Ljxh);
                        RecordTemp2.Syxh = m_Inpatient.Syxh;
                        RecordTemp2.Xmlb = 9;
                        RecordTemp2.Jddm = _nodeCode;
                        RecordTemp2.Ljdm = m_Inpatient.CurrentCP_ClinicalPathNode.Ljdm;
                        RecordTemp2.JcxmName = txtReason.Text;
                        CP_PatientPathEnterJudgeConditionRecords.Add(RecordTemp2);
                    }
                }
                #endregion
                #region 进入后数据更新
                List<CP_PathEnterJudgeCondition> conditions = (List<CP_PathEnterJudgeCondition>)ConditionGrid.ItemsSource;
                foreach (CP_PathEnterJudgeCondition item in conditions)
                {
                    CP_PatientPathEnterJudgeConditionRecord RecordTemp = new CP_PatientPathEnterJudgeConditionRecord();
                    RecordTemp.Lb = 2;
                    RecordTemp.Ljxh = ConvertMy.ToString(m_Inpatient.Ljxh);
                    RecordTemp.Syxh = m_Inpatient.Syxh;
                    RecordTemp.Ljdm = item.Ljdm;
                    RecordTemp.Jddm = _nodeCode;
                    RecordTemp.Sjfl = item.Sjfl;
                    RecordTemp.Jcxm = item.Jcxm;
                    RecordTemp.JcxmName = item.JcxmName;
                    RecordTemp.Xmlb = item.Xmlb;
                    RecordTemp.Ksfw = item.Ksfw;
                    RecordTemp.Jsfw = item.Jsfw;
                    RecordTemp.Jcjg = item.ExamValue;
                    RecordTemp.Pdjg = ConvertMy.ToString(item.MatchResult);
                    RecordTemp.Dw = item.Dw;
                    RecordTemp.Bz = item.Bz;
                    CP_PatientPathEnterJudgeConditionRecords.Add(RecordTemp);
                }
                List<CP_PathEnterJudgeCondition> conditionsHand = (List<CP_PathEnterJudgeCondition>)ConditionGridHand.ItemsSource;
                foreach (CP_PathEnterJudgeCondition item in conditionsHand)
                {
                    CP_PatientPathEnterJudgeConditionRecord RecordTemp = new CP_PatientPathEnterJudgeConditionRecord();
                    RecordTemp.Lb = 2;
                    RecordTemp.Ljxh = ConvertMy.ToString(m_Inpatient.Ljxh);
                    RecordTemp.Syxh = m_Inpatient.Syxh;
                    RecordTemp.Ljdm = item.Ljdm;
                    RecordTemp.Jddm = _nodeCode;
                    RecordTemp.Sjfl = item.Sjfl;
                    RecordTemp.Jcxm = item.Jcxm;
                    RecordTemp.JcxmName = item.JcxmName;
                    RecordTemp.Xmlb = item.Xmlb;
                    RecordTemp.Ksfw = item.Ksfw;
                    RecordTemp.Jsfw = item.Jsfw;
                    RecordTemp.Jcjg = item.ExamValue;
                    RecordTemp.Pdjg = ConvertMy.ToString(item.MatchResult);
                    RecordTemp.Dw = item.Dw;
                    RecordTemp.Bz = item.Bz;
                    CP_PatientPathEnterJudgeConditionRecords.Add(RecordTemp);
                }
                
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                radBusyIndicator.IsBusy = true;

                client.InsertCP_PatientNodeEnterJudgeConditionRecordCompleted += (send, ea) => { radBusyIndicator.IsBusy = false; };
                client.InsertCP_PatientNodeEnterJudgeConditionRecordAsync(CP_PatientPathEnterJudgeConditionRecords, Global.LogInEmployee.Zgdm, m_Inpatient);
                client.CloseAsync();
                #endregion
                this.DialogResult = true;
                this.Close();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 拒绝
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReject_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            //this.NavigationService.Navigate(new Uri("/PathEnForce", UriKind.Relative)); //页面跳转
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }
        private void ConditionGrid_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            try
            {
                if (e.Row is GridViewRow && !(e.Row is GridViewNewRow))
                {
                    CP_PathEnterJudgeCondition condition = e.DataElement as CP_PathEnterJudgeCondition;
                    //if (condition.Xmlb == 1 && condition.MatchResult != MatchResultState.Match)
                    //{
                    //    SolidColorBrush brush = new SolidColorBrush();
                    //    brush.Color = Colors.Red;
                    //    ((GridViewRow)e.Row).Background = brush;
                    //}
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
    }
}
