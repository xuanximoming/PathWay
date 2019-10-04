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
using YidanEHRApplication.Models;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Controls;
using YidanEHRApplication.WorkFlow.Designer;

namespace YidanEHRApplication.NurModule
{
    public partial class UCNurNotesRecord : UserControl
    {
        /// <summary>
        /// 定义提示框实体
        /// </summary>
        DialogBoxShow classDialogBoxShow = new DialogBoxShow();

        /// <summary>
        /// 当前所选节点
        /// </summary>
        public Activity CurrentActivity
        { get; set; }

        private bool m_ContentLoaded;


        public UCNurNotesRecord()
        {
            InitializeComponent();
            classDialogBoxShow.SelectedResult += new DialogBoxShow.SelectedResultEvent(classDialogBoxShow_SelectedResult);
            //add by luff 20130308 委托加载事件
            Loaded += new RoutedEventHandler(UCNurNotesRecord_Loaded);
            
        }

        //add by luff 20130308
        void UCNurNotesRecord_Loaded(object sender, RoutedEventArgs e)
        {
    
            //add by luff 20130308 判断病人是否出院，控制相关操作按钮显示
            if (Global.InpatientListCurrent.Status == "1503")
            {
                rbtnAdd.IsEnabled = false;
                rbtnCancel.IsEnabled = false;
            }
         }
        #region 查询病人护理记录单

        #region  查询方法
        /// <summary>
        /// 查询病人护理记录单
        /// </summary>
        public void QueryNursingNotesInfo()
        {

            rbtnCancel.IsEnabled = false;
            rbtnAdd.IsEnabled = false;

            radBusyIndicator.IsBusy = true;

            YidanEHRDataServiceClient QueryNursingNotesInfoClient = PublicMethod.YidanClient;
            QueryNursingNotesInfoClient.QueryNursingNotesInfoCompleted += (s, e) =>
            {
                radBusyIndicator.IsBusy = false;
                if (e.Error == null)
                {
                    GetNotesInfoComplete(e);
                }
                else
                {
                    PublicMethod.RadWaringBox(e.Error);
                }
            };
            QueryNursingNotesInfoClient.QueryNursingNotesInfoAsync(Global.InpatientListCurrent, rcmbDays.SelectedIndex, CurrentActivity.CurrentViewActiveChildren.ActivityChildrenID);
            QueryNursingNotesInfoClient.CloseAsync();
        }


        private void GetNotesInfoComplete(QueryNursingNotesInfoCompletedEventArgs e)
        {
            try
            {
                //病人基本信息
                //List<CP_InpatinetList> cpInpatinetList = null;

                //生命体征护理记录
                //生命体征护理记录
                List<CP_VitalSignsRecordInfo> cpVitalSignsRecord =
                     (e.Result as CP_NursingNotesRecordCollection).CP_VitalSignsRecordCollection.ToList<CP_VitalSignsRecordInfo>();
                //病人入量护理记录
                List<CP_PatientInOutRecordInfo> cpPatientInRecord =
                     (e.Result as CP_NursingNotesRecordCollection).CP_PatientInOutRecordInCollection.ToList<CP_PatientInOutRecordInfo>();
                //病人出量护理记录
                List<CP_PatientInOutRecordInfo> cpPatientOutRecord =
                     (e.Result as CP_NursingNotesRecordCollection).CP_PatientInOutRecordOutCollection.ToList<CP_PatientInOutRecordInfo>();
                //病人治疗主要流程
                List<CP_TreatmentFlowInfo> cpTreatmentFlow =
                     (e.Result as CP_NursingNotesRecordCollection).CP_TreatmentFlowCollection.ToList<CP_TreatmentFlowInfo>();
                //特殊护理记录
                List<CP_VitalSignSpecialRecordInfo> cpVitalSignSpecialRecord =
                    (e.Result as CP_NursingNotesRecordCollection).CP_VitalSignSpecialRecordCollection.ToList<CP_VitalSignSpecialRecordInfo>();

                //ShowPatientInfo(cpInpatinetList); //显示病人基本信息                
                //ShowVitalSignSpecialRecord(cpVitalSignSpecialRecord);//显示病人特殊护理记录

                //数据源捆绑
                rgvVitalSignsRecord.ItemsSource = cpVitalSignsRecord;
                rgvPatientInRecord.ItemsSource = cpPatientInRecord;
                rgvPatientOutRecord.ItemsSource = cpPatientOutRecord;
                rgvTreatmentFlow.ItemsSource = cpTreatmentFlow;
                rgvVitalSignSpecialRecord.ItemsSource = cpVitalSignSpecialRecord;
                SetButtonEnable(CurrentActivity);
            }
            catch (Exception ex)
            {
                PublicMethod.InsertClientLogException(ex, this.GetType().FullName);
            }
        }

        private void SetButtonEnable(Activity a)
        {
            if (a.Type == ActivityType.AUTOMATION) //循环结点
            {
                if (a.CurrentElementState == WorkFlow.ElementState.Pre)
                {
                    rbtnAdd.IsEnabled = false;
                    rbtnCancel.IsEnabled = false;
                }
                else if (a.CurrentElementState == WorkFlow.ElementState.Now)
                {
                    if (a.CurrentViewActiveChildren.CurrentElementState == WorkFlow.ElementState.Now)
                    {
                        rbtnAdd.IsEnabled = true;
                        rbtnCancel.IsEnabled = true;
                    }
                    else
                    {
                        rbtnAdd.IsEnabled = false;
                        rbtnCancel.IsEnabled = false;
                    }
                }
                else if (a.CurrentElementState == WorkFlow.ElementState.Next)
                {
                    rbtnAdd.IsEnabled = false;
                    rbtnCancel.IsEnabled = false;
                }
                else
                {
                    rbtnAdd.IsEnabled = false;
                    rbtnCancel.IsEnabled = false;
                }
            }
            else
            {
                if (a.CurrentElementState == WorkFlow.ElementState.Pre)
                {
                    rbtnAdd.IsEnabled = false;
                    rbtnCancel.IsEnabled = false;
                }
                else if (a.CurrentElementState == WorkFlow.ElementState.Now)
                {
                    rbtnAdd.IsEnabled = true;
                    rbtnCancel.IsEnabled = true;
                }
                else if (a.CurrentElementState == WorkFlow.ElementState.Next)
                {
                    rbtnAdd.IsEnabled = false;
                    rbtnCancel.IsEnabled = false;
                }
                else
                {
                    rbtnAdd.IsEnabled = false;
                    rbtnCancel.IsEnabled = false;
                }
            }

        }
        #endregion

        #endregion

        #region 提示对话框选择结果事件
        /// <summary>
        /// 提示对话框选择结果事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void classDialogBoxShow_SelectedResult(object sender, RoutedEventArgs e)
        {
            if ((e as DialogBoxShow.OpreateEventArgs).bResult == true)  //选择确定按钮
            {
                NursingNotesRecordCancel();//作废一条记录
            }
        }
        #endregion

        #region 作废一条护理记录
        /// <summary>
        /// 作废一条记录
        /// </summary>
        private void NursingNotesRecordCancel()
        {
            int intType = 0;
            string strJlxh2 = "";

            #region 获取数据

            switch ((rtcNursingNotes.SelectedItem as Telerik.Windows.Controls.RadTabItem).Header.ToString())
            {
                case "生命体征记录":
                    {
                        if (rgvVitalSignsRecord.SelectedItem == null)
                        {
                            PublicMethod.RadAlterBox("请选择一条需要操作的记录!", "提示");
                            return;
                        }
                        CP_VitalSignsRecordInfo cp = rgvVitalSignsRecord.SelectedItem as CP_VitalSignsRecordInfo;
                        strJlxh2 = cp.Jlxh;
                        intType = 1;
                        break;
                    }
                case "入量记录":
                    {
                        if (rgvPatientInRecord.SelectedItem == null)
                        {
                            PublicMethod.RadAlterBox("请选择一条需要操作的记录!", "提示");
                            return;
                        }
                        CP_PatientInOutRecordInfo cp = rgvPatientInRecord.SelectedItem as CP_PatientInOutRecordInfo;
                        strJlxh2 = cp.Jlxh;
                        intType = 2;
                        break;
                    }
                case "出量记录":
                    {
                        if (rgvPatientOutRecord.SelectedItem == null)
                        {
                            PublicMethod.RadAlterBox("请选择一条需要操作的记录!", "提示");
                            return;
                        }
                        CP_PatientInOutRecordInfo cp = rgvPatientOutRecord.SelectedItem as CP_PatientInOutRecordInfo;
                        strJlxh2 = cp.Jlxh;
                        intType = 3;
                        break;
                    }
                case "治疗流程记录":
                    {
                        if (rgvTreatmentFlow.SelectedItem == null)
                        {
                            PublicMethod.RadAlterBox("请选择一条需要操作的记录!", "提示");
                            return;
                        }
                        CP_TreatmentFlowInfo cp = rgvTreatmentFlow.SelectedItem as CP_TreatmentFlowInfo;
                        strJlxh2 = cp.Jlxh;
                        intType = 4;
                        break;
                    }
                case "特殊记录":
                    {
                        if (rgvVitalSignSpecialRecord.SelectedItem == null)
                        {
                            PublicMethod.RadAlterBox("请选择一条需要操作的记录!", "提示");
                            return;
                        }
                        CP_VitalSignSpecialRecordInfo cp = rgvVitalSignSpecialRecord.SelectedItem as CP_VitalSignSpecialRecordInfo;
                        strJlxh2 = cp.Jlxh;
                        intType = 5;
                        break;
                    }
            }
            #endregion

            radBusyIndicator.IsBusy = true;
            YidanEHRDataServiceClient NursingNotesRecordCancelClient = PublicMethod.YidanClient;
            NursingNotesRecordCancelClient.NursingNotesRecordCancelCompleted +=
                  (obj, e) =>
                  {
                      radBusyIndicator.IsBusy = false;
                      if (e.Error == null)
                      {
                          #region 删除客户端作废的记录
                          if (e.Result.ToString() == "操作完成!")
                          {
                              string strJlxh = "";
                              switch ((rtcNursingNotes.SelectedItem as Telerik.Windows.Controls.RadTabItem).Header.ToString())
                              {
                                  case "生命体征记录":
                                      {
                                          if (rgvVitalSignsRecord.SelectedItem == null)
                                          {
                                              break;
                                          }

                                          List<CP_VitalSignsRecordInfo> lst = rgvVitalSignsRecord.ItemsSource as List<CP_VitalSignsRecordInfo>;
                                          CP_VitalSignsRecordInfo cp = rgvVitalSignsRecord.SelectedItem as CP_VitalSignsRecordInfo;
                                          strJlxh = cp.Jlxh;
                                          rgvVitalSignsRecord.ItemsSource = new List<CP_VitalSignsRecordInfo>();
                                          foreach (CP_VitalSignsRecordInfo cp0 in lst)
                                          {
                                              if (strJlxh == cp0.Jlxh)
                                              {
                                                  lst.Remove(cp0);
                                                  break;
                                              }
                                          }

                                          rgvVitalSignsRecord.ItemsSource = lst;
                                          break;
                                      }
                                  case "入量记录":
                                      {
                                          if (rgvPatientInRecord.SelectedItem == null)
                                          {
                                              break;
                                          }

                                          List<CP_PatientInOutRecordInfo> lst = rgvPatientInRecord.ItemsSource as List<CP_PatientInOutRecordInfo>;
                                          CP_PatientInOutRecordInfo cp = rgvPatientInRecord.SelectedItem as CP_PatientInOutRecordInfo;
                                          strJlxh = cp.Jlxh;
                                          rgvPatientInRecord.ItemsSource = new List<CP_PatientInOutRecordInfo>();
                                          foreach (CP_PatientInOutRecordInfo cp0 in lst)
                                          {
                                              if (strJlxh == cp0.Jlxh)
                                              {
                                                  lst.Remove(cp0);
                                                  break;
                                              }
                                          }

                                          rgvPatientInRecord.ItemsSource = lst;
                                          break;
                                      }
                                  case "出量记录":
                                      {
                                          if (rgvPatientOutRecord.SelectedItem == null)
                                          {
                                              break;
                                          }

                                          List<CP_PatientInOutRecordInfo> lst = rgvPatientOutRecord.ItemsSource as List<CP_PatientInOutRecordInfo>;
                                          CP_PatientInOutRecordInfo cp = rgvPatientOutRecord.SelectedItem as CP_PatientInOutRecordInfo;
                                          strJlxh = cp.Jlxh;
                                          rgvPatientOutRecord.ItemsSource = new List<CP_PatientInOutRecordInfo>();
                                          foreach (CP_PatientInOutRecordInfo cp0 in lst)
                                          {
                                              if (strJlxh == cp0.Jlxh)
                                              {
                                                  lst.Remove(cp0);
                                                  break;
                                              }
                                          }

                                          rgvPatientOutRecord.ItemsSource = lst;
                                          break;
                                      }
                                  case "治疗流程记录":
                                      {
                                          if (rgvTreatmentFlow.SelectedItem == null)
                                          {
                                              break;
                                          }

                                          List<CP_TreatmentFlowInfo> lst = rgvTreatmentFlow.ItemsSource as List<CP_TreatmentFlowInfo>;
                                          CP_TreatmentFlowInfo cp = rgvTreatmentFlow.SelectedItem as CP_TreatmentFlowInfo;
                                          strJlxh = cp.Jlxh;
                                          rgvTreatmentFlow.ItemsSource = new List<CP_TreatmentFlowInfo>();
                                          foreach (CP_TreatmentFlowInfo cp0 in lst)
                                          {
                                              if (strJlxh == cp0.Jlxh)
                                              {
                                                  lst.Remove(cp0);
                                                  break;
                                              }
                                          }

                                          rgvTreatmentFlow.ItemsSource = lst;
                                          break;
                                      }
                                  case "特殊记录":
                                      {
                                          if (rgvVitalSignSpecialRecord.SelectedItem == null)
                                          {
                                              break;
                                          }

                                          List<CP_VitalSignSpecialRecordInfo> lst = rgvVitalSignSpecialRecord.ItemsSource as List<CP_VitalSignSpecialRecordInfo>;
                                          CP_VitalSignSpecialRecordInfo cp = rgvVitalSignSpecialRecord.SelectedItem as CP_VitalSignSpecialRecordInfo;
                                          strJlxh = cp.Jlxh;
                                          rgvVitalSignSpecialRecord.ItemsSource = new List<CP_VitalSignSpecialRecordInfo>();
                                          foreach (CP_VitalSignSpecialRecordInfo cp0 in lst)
                                          {
                                              if (strJlxh == cp0.Jlxh)
                                              {
                                                  lst.Remove(cp0);
                                                  break;
                                              }
                                          }

                                          rgvVitalSignSpecialRecord.ItemsSource = lst;
                                          break;
                                      }
                              }
                          }
                          #endregion
                          PublicMethod.RadAlterBox(e.Result.ToString(), "提示");
                      }
                      else
                      {
                          PublicMethod.RadWaringBox(e.Error);
                      }
                  };
            NursingNotesRecordCancelClient.NursingNotesRecordCancelAsync(intType, strJlxh2, Global.LogInEmployee.Zgdm, Global.LogInEmployee.Name);
            NursingNotesRecordCancelClient.CloseAsync();
        }


        #endregion

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                QueryNursingNotesInfo();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 新增护理记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RWNurNotesEntry cWin = new RWNurNotesEntry(this, CurrentActivity);
                cWin.ShowDialog();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 作废护理记录
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NursingNotesRecordCancel();
                //classDialogBoxShow.ShowDialogBox("确定作废当前选择的一条记录吗?");
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        #region 判断作废按钮是否有效
        /// <summary>
        /// 所有RadGridView事件：判断作废按钮是否有效
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadGridViewRecord_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                SetButtonEnable(CurrentActivity);
                if (!rbtnCancel.IsEnabled)
                    return;
                switch ((sender as Telerik.Windows.Controls.RadGridView).Name.ToString())
                {
                    case "rgvVitalSignsRecord":
                        {
                            rbtnCancel.IsEnabled = rgvVitalSignsRecord.SelectedItem == null ? false : true;
                            break;
                        }
                    case "rgvPatientInRecord":
                        {
                            rbtnCancel.IsEnabled = rgvPatientInRecord.SelectedItem == null ? false : true;
                            break;
                        }
                    case "rgvPatientOutRecord":
                        {
                            rbtnCancel.IsEnabled = rgvPatientOutRecord.SelectedItem == null ? false : true;
                            break;
                        }
                    case "rgvTreatmentFlow":
                        {
                            rbtnCancel.IsEnabled = rgvTreatmentFlow.SelectedItem == null ? false : true;
                            break;
                        }
                    case "rgvVitalSignSpecialRecord":
                        {
                            rbtnCancel.IsEnabled = rgvVitalSignSpecialRecord.SelectedItem == null ? false : true;
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
        /// tab选项卡选择事件：判断作废按钮是否有效
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtcNursingNotes_SelectionChanged(object sender, RoutedEventArgs e)
        {
            try
            {

                if (rtcNursingNotes.SelectedItem == null)
                {
                    rbtnCancel.IsEnabled = false;
                    return;
                }
                switch ((rtcNursingNotes.SelectedItem as Telerik.Windows.Controls.RadTabItem).Header.ToString())
                {
                    case "生命体征记录":
                        {
                            rbtnCancel.IsEnabled = rgvVitalSignsRecord.CurrentItem == null ? false : true;
                            break;
                        }
                    case "入量记录":
                        {
                            rbtnCancel.IsEnabled = rgvPatientInRecord.CurrentItem == null ? false : true;
                            break;
                        }
                    case "出量记录":
                        {
                            rbtnCancel.IsEnabled = rgvPatientOutRecord.CurrentItem == null ? false : true;
                            break;
                        }
                    case "治疗流程记录":
                        {
                            rbtnCancel.IsEnabled = rgvTreatmentFlow.CurrentItem == null ? false : true;
                            break;
                        }
                    case "特殊记录":
                        {
                            rbtnCancel.IsEnabled = rgvVitalSignSpecialRecord.CurrentItem == null ? false : true;
                            break;
                        }
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        #endregion

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                (sender as CheckBox).IsChecked = (sender as CheckBox).Tag.ToString() == "1" ? true : false;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void TextGroupBoxControl1_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                UCTextGroupBoxControl obj = (sender as UCTextGroupBoxControl);
                obj.Shit = obj.Tag.ToString();//赋值
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }


        private void acbPatientInfo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                QueryNursingNotesInfo();
            }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_ContentLoaded)
                {
                    return;
                }
                m_ContentLoaded = true;

                QueryNursingNotesInfo();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
    }
}
