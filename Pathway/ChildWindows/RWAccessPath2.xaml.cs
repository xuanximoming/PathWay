using DrectSoft.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using YidanEHRApplication.Views;
namespace YidanEHRApplication
{
    public partial class RWAccessPath2
    {
        public int m_iFlag = 0;
        public RWAccessPath2(CP_InpatinetList patient)
        {
            InitializeComponent();
            m_Inpatient = patient;
        }

        public RWAccessPath2(CP_InpatinetList patient, int iflag)
        {
            InitializeComponent();
            m_Inpatient = patient;
            m_iFlag = iflag;
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
                    client.GetAnalysePatient_CanEnterPathCompleted += (send, ea) =>
                    {
                        m_Inpatient = ((CP_InpatinetList)ea.Result);
                        radGridViewPathList.ItemsSource = m_Inpatient.Paths;
                        radBusyIndicator.IsBusy = false;
                    };
                    client.GetAnalysePatient_CanEnterPathAsync(m_Inpatient);
                    client.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #region 属性变量
        ChildMessageBox messagebox = new ChildMessageBox();
        ChildWindowInputMessage m_InputMessageBox = new ChildWindowInputMessage();
        CP_InpatinetList m_Inpatient;
        CP_ClinicalPathList _Path;
        #endregion
        #region 事件
        /// <summary>
        /// 路径列表 SelectionChanged
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radGridViewPathList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            try
            {
                if (radGridViewPathList.SelectedItem == null) { ConditionGrid.ItemsSource = null; return; }
                _Path = (CP_ClinicalPathList)radGridViewPathList.SelectedItem;
                List<CP_PathEnterJudgeCondition> conditions = new List<CP_PathEnterJudgeCondition>();
                List<CP_PathEnterJudgeCondition> conditionsDescrib = new List<CP_PathEnterJudgeCondition>();
                CP_PathEnterJudgeCondition ICDCondition = new CP_PathEnterJudgeCondition();
                foreach (CP_PathEnterJudgeCondition item in _Path.PathEnterJudgeConditions)
                {
                    if (item.Xmlb == 2)//合并ICD项
                    {
                        if (string.IsNullOrEmpty(ICDCondition.Jcxm))
                        {
                            ICDCondition = item;
                            ICDCondition.ExamValue = m_Inpatient.ICD10;
                        }
                        else
                        {
                            ICDCondition.Jcxm += "," + item.Jcxm;
                            ICDCondition.JcxmName += "," + item.JcxmName;
                        }
                    }
                    else
                    {
                        if (item.Xmlb == 3)
                        { conditionsDescrib.Add(item); }
                        else
                            if (item.MatchResult != MatchResultState.Match)//不满足条件的靠前显示
                            { conditions.Insert(0, item); }
                            else
                            {
                                conditions.Add(item);
                            }
                    }
                }
                ICDCondition.MatchResult = MatchResultState.Match;
                ICDCondition.MatchResultName = "满足条件";
                conditions.Insert(0, ICDCondition);
                ConditionGrid.ItemsSource = conditions;
                ConditionGridHand.ItemsSource = conditionsDescrib;
                if (!_Path.IsCanEnter && _Path.IsPossibleEnter)
                {

                    stkReason.Visibility = Visibility.Visible;
                }
                else { stkReason.Visibility = Visibility.Collapsed; }

            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
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

                if (radGridViewPathList.SelectedItem == null)
                {
                    ConditionGrid.ItemsSource = null;
                    PublicMethod.RadAlterBox("请选择要进入的路径", "提示");
                    return;
                }
                if (_Path.IsCanEnter && ConditionGridHand.Items.Count > 0 && ConditionGridHand.SelectedItems.Count < ConditionGridHand.Items.Count)
                {

                    PublicMethod.RadAlterBox("入径评估手动判断条件,必须全部满足，并勾选", "提示");
                    return;
                }
                if (!_Path.IsCanEnter && _Path.IsPossibleEnter)
                {
                    if (String.IsNullOrEmpty(txtReason.Text))
                    {
                        PublicMethod.RadAlterBox("请输入不满足条件下进入的原因", "提示");
                        return;
                    }
                    else
                    {
                        CP_PatientPathEnterJudgeConditionRecord RecordTemp2 = new CP_PatientPathEnterJudgeConditionRecord();
                        RecordTemp2.Lb = 1;
                        RecordTemp2.Ljxh = ConvertMy.ToString(m_Inpatient.Ljxh);
                        RecordTemp2.Syxh = m_Inpatient.Syxh;
                        RecordTemp2.Xmlb = 9;
                        RecordTemp2.Ljdm = _Path.Ljdm;
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
                    RecordTemp.Lb = item.Lb;
                    RecordTemp.Ljxh = ConvertMy.ToString(m_Inpatient.Ljxh);
                    RecordTemp.Syxh = m_Inpatient.Syxh;
                    RecordTemp.Ljdm = item.Ljdm;
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
                    RecordTemp.Lb = item.Lb;
                    RecordTemp.Ljxh = ConvertMy.ToString(m_Inpatient.Ljxh);
                    RecordTemp.Syxh = m_Inpatient.Syxh;
                    RecordTemp.Ljdm = item.Ljdm;
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
                m_Inpatient.Ljdm = _Path.Ljdm;
                client.InsertCP_PatientPathEnterJudgeConditionRecordCompleted += (send, ea) =>
                {
                    if (ea.Error == null)
                    {
                        if (ea.Result == -1)
                        {
                            PublicMethod.RadAlterBox("该病人已经进入该路径，请不要重复进入！", "提示");
                            return;
                        }
                        radBusyIndicator.IsBusy = false;

                    }
                };
                client.InsertCP_PatientPathEnterJudgeConditionRecordAsync(CP_PatientPathEnterJudgeConditionRecords, Global.LogInEmployee.Zgdm, m_Inpatient);
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
                    if (condition.Xmlb == 1 && condition.MatchResult != MatchResultState.Match)
                    {
                        SolidColorBrush brush = new SolidColorBrush();
                        brush.Color = Colors.Red;
                        ((GridViewRow)e.Row).Background = brush;
                    }
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
