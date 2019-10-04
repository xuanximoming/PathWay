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

using YidanEHRApplication.Models;
using YidanEHRApplication.Helpers;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;

/*此文件可以删除，暂时保留做参考,xjt,20100222*/
namespace YidanEHRApplication.Views.NursingNotes
{
    /// <summary>
    /// Interaction logic for NursingNotesEntry.xaml
    /// </summary>
    public partial class RWNursingNotesEntry
    {
        /// <summary>
        /// 定义提示框实体
        /// </summary>
        DialogBoxShow classDialogBoxShow = new DialogBoxShow();
        /// <summary>
        /// 病人住院号码
        /// </summary>
        //public string Zyhm;

        /// <summary>
        /// NursingNotesRecord对象变量
        /// </summary>
        NursingNotesRecord page = new NursingNotesRecord();

        public RWNursingNotesEntry(NursingNotesRecord obj)
        {
            InitializeComponent();
            //初始化提示框选择结果事件
            classDialogBoxShow.SelectedResult += new DialogBoxShow.SelectedResultEvent(classDialogBoxShow_SelectedResult);
            page = obj;

        }


        #region 初始化护理记录单编码

        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        { try{
            if (!IsSelectPatient())
                return;
            #region 记录日期和时间初始化
            //日期选择范围的开始值
            this.rdpRecordDateVitalSign.SelectableDateStart = Convert.ToDateTime("2010-12-01");
            this.rdpRecordDateIn.SelectableDateStart = Convert.ToDateTime("2010-12-01");
            this.rdpRecordDateOut.SelectableDateStart = Convert.ToDateTime("2010-12-01");
            this.rdpRecordDateIncident.SelectableDateStart = Convert.ToDateTime("2010-12-01");
            this.rdpRecordDateOther.SelectableDateStart = Convert.ToDateTime("2010-12-01");

            //日期选择范围的截止值
            this.rdpRecordDateVitalSign.SelectableDateEnd = DateTime.Now;
            this.rdpRecordDateIn.SelectableDateEnd = DateTime.Now;
            this.rdpRecordDateOut.SelectableDateEnd = DateTime.Now;
            this.rdpRecordDateIncident.SelectableDateEnd = DateTime.Now;
            this.rdpRecordDateOther.SelectableDateEnd = DateTime.Now;

            //日期初始化为当前日期
            this.rdpRecordDateVitalSign.SelectedDate = DateTime.Now;
            this.rdpRecordDateIn.SelectedDate = DateTime.Now;
            this.rdpRecordDateOut.SelectedDate = DateTime.Now;
            this.rdpRecordDateIncident.SelectedDate = DateTime.Now;
            this.rdpRecordDateOther.SelectedDate = DateTime.Now;

            //初始化记录时间为当前时间
            //this.tudRecordTimeVitalSign.Value = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            this.tudRecordTimeIn.Value = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            this.tudRecordTimeOut.Value = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            this.tudRecordTimeIncident.Value = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            this.tudRecordTimeOther.Value = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            #endregion
            GetVitalSignsRecordTimeSet();//获取生命体征记录时间段
            GetNurseCode();//初始化编码
             }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }



        /// <summary>
        /// 判断是否选中病患
        /// </summary>
        /// <returns></returns>
        private Boolean IsSelectPatient()
        {
            if (Global.InpatientListCurrent == null)
            {
                PublicMethod.RadAlterBox("请选择在路径中的病患", "提示");
                return false;
            }
            return true;
        }

        #region 获取生命体征记录时间段
        /// <summary>
        /// 获取生命体征记录时间段
        /// </summary>
        private void GetVitalSignsRecordTimeSet()
        {
            radBusyIndicator.IsBusy = true;

            YidanEHRDataServiceClient GetVitalSignsRecordTimeSetClient = PublicMethod.YidanClient;
            GetVitalSignsRecordTimeSetClient.GetVitalSignsRecordTimeSetCompleted +=
                (obj, e) =>
                {
                    radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {

                        string[] strTime = e.Result.ToString().Split(',');
                        cmbRecordTimeVitalSign.Items.Clear();
                        cmbRecordTimeVitalSign.Items.Add("无");
                        for (int i = 0; i < strTime.Length; i++)
                        {
                            cmbRecordTimeVitalSign.Items.Add(strTime[i]);
                        }
                        cmbRecordTimeVitalSign.SelectedIndex = 0;
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            GetVitalSignsRecordTimeSetClient.GetVitalSignsRecordTimeSetAsync();
            GetVitalSignsRecordTimeSetClient.CloseAsync();
        }

        
        #endregion

        #region 初始化护理记录单编码
        /// <summary>
        /// 初始化护理记录单编码
        /// </summary>
        private void GetNurseCode()
        {
            YidanEHRDataServiceClient GetNurseCodeClient = PublicMethod.YidanClient;
            GetNurseCodeClient.GetNurseCodeCompleted +=
                  (obj, e) =>
                  {
                      if (e.Error == null)
                      {
                          List<CP_NurseCodeCollection> lstCode = e.Result.ToList<CP_NurseCodeCollection>();

                          #region 编码控件捆绑数据源
                          //**************** 生命体征 ****************
                          //病人状态
                          this.rcmbPatientState.ItemsSource = (lstCode[11] as CP_NurseCodeCollection).NurseCodeCollection;
                          rcmbPatientState.SelectedIndex = 0;
                          //体温测量方式
                          this.rcmbMeasuringMode.ItemsSource = (lstCode[0] as CP_NurseCodeCollection).NurseCodeCollection;
                          rcmbMeasuringMode.SelectedIndex = 0;
                          //体温测量辅助措施
                          this.rcmbCuoShi.ItemsSource = (lstCode[1] as CP_NurseCodeCollection).NurseCodeCollection;
                          rcmbCuoShi.SelectedIndex = 0;

                          //**************** 病人入量 ****************
                          //其它入量1
                          this.rcmbOtherIn1.ItemsSource = (lstCode[2] as CP_NurseCodeCollection).NurseCodeCollection;
                          rcmbOtherIn1.SelectedIndex = 0;
                          //其它入量2
                          this.rcmbOtherIn2.ItemsSource = (lstCode[2] as CP_NurseCodeCollection).NurseCodeCollection;
                          rcmbOtherIn2.SelectedIndex = 0;

                          //**************** 病人出量 ****************
                          //小便性状
                          this.rcmbPeeProperty.ItemsSource = (lstCode[3] as CP_NurseCodeCollection).NurseCodeCollection;
                          rcmbPeeProperty.SelectedIndex = 0;
                          //排小便辅助措施
                          this.rcmbPeeLabour.ItemsSource = (lstCode[4] as CP_NurseCodeCollection).NurseCodeCollection;
                          rcmbPeeLabour.SelectedIndex = 0;
                          //大便性状
                          this.rcmbShitProperty.ItemsSource = (lstCode[5] as CP_NurseCodeCollection).NurseCodeCollection;
                          rcmbShitProperty.SelectedIndex = 0;
                          //排大便辅助措施
                          this.rcmbShitLabour.ItemsSource = (lstCode[6] as CP_NurseCodeCollection).NurseCodeCollection;
                          rcmbShitLabour.SelectedIndex = 0;
                          //痰性状
                          this.rcmbSputumProperty.ItemsSource = (lstCode[7] as CP_NurseCodeCollection).NurseCodeCollection;
                          rcmbSputumProperty.SelectedIndex = 0;
                          //其它出量1
                          this.rcmbOtherOut1.ItemsSource = (lstCode[8] as CP_NurseCodeCollection).NurseCodeCollection;
                          rcmbOtherOut1.SelectedIndex = 0;
                          //其它出量2
                          this.rcmbOtherOut2.ItemsSource = (lstCode[8] as CP_NurseCodeCollection).NurseCodeCollection;
                          rcmbOtherOut2.SelectedIndex = 0;

                          //**************** 病人特殊记录 ****************
                          //血型
                          this.rcmbBloodType.ItemsSource = (lstCode[9] as CP_NurseCodeCollection).NurseCodeCollection;
                          rcmbBloodType.SelectedIndex = 0;
                          //血性HR
                          this.rcmbHR.ItemsSource = (lstCode[10] as CP_NurseCodeCollection).NurseCodeCollection;
                          rcmbHR.SelectedIndex = 0;
                          #endregion
                      }
                      else
                      {
                          PublicMethod.RadWaringBox(e.Error);
                      }
                  };
            GetNurseCodeClient.GetNurseCodeAsync();
            GetNurseCodeClient.CloseAsync();
        }

     
        #endregion

        #endregion

        #region 保存护理记录单数据

        #region 保存录入的护理记录数据（按钮）
        /// <summary>
        /// 保存录入的护理记录数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (Zyhm == "") PublicMethod.RadAlterBox("病人病历号为空,操作失败！", "提示");
                CheckAndSaveData();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        #endregion

        #region 验证和保存数据
        /// <summary>
        ///  验证和保存数据
        /// </summary>
        private void CheckAndSaveData()
        {
            bool[] bIsEmpty = new bool[5] { false, false, false, false, false };
            bool[] bIsSave = new bool[5] { false, false, false, false, false };

            CP_VitalSignsRecordInfo cpVitalSignsRecord = GetSaveVitalSignData();
            CP_PatientInOutRecordInfo cpPatientInOutRecordIn = GetSavePatientInData();
            CP_PatientInOutRecordInfo cpPatientInOutRecordOut = GetSavePatientOutData();
            CP_TreatmentFlowInfo cpTreatmentFlow = GetSaveTreatmentFlowData();
            CP_VitalSignSpecialRecordInfo cpVitalSignSpecialRecord = GetSaveVitalSignSpecialRecordData();

            #region 判断是否有数据保存
            if (cpVitalSignsRecord.Hztw == "" && cpVitalSignsRecord.Hzmb == "" && cpVitalSignsRecord.Hzxl == "" && cpVitalSignsRecord.Hzhx == "" && !TextSeparateBoxControlBP.IsInput)
            {
                bIsEmpty[0] = true;
                //PublicMethod.RadAlterBox("病人体温、脉搏、心率、呼吸、血压至少填写一项!", "提示");
            }

            if (cpPatientInOutRecordIn.Ysl == "" && cpPatientInOutRecordIn.Hsl == "" && cpPatientInOutRecordIn.Syl == "" && cpPatientInOutRecordIn.Zsl == "" && cpPatientInOutRecordIn.Sxl == "" &&
             ((this.rcmbOtherIn1.SelectedIndex > 0 && cpPatientInOutRecordIn.Qtrl1 == "") || this.rcmbOtherIn1.SelectedIndex < 1) &&
             ((this.rcmbOtherIn2.SelectedIndex > 0 && cpPatientInOutRecordIn.Qtrl2 == "") || this.rcmbOtherIn2.SelectedIndex < 1))
            {
                bIsEmpty[1] = true;
                //PublicMethod.RadAlterBox("病人饮食量、喝水量、输液量、注射量、\n输血量、其它1、其它2至少填写一项!", "提示");

            }

            if (cpPatientInOutRecordOut.Hzxb == "" && cpPatientInOutRecordOut.Yll == "" && !TextGroupBoxControlShit.IsInput && cpPatientInOutRecordOut.Hztl == "" &&
           ((this.rcmbOtherOut1.SelectedIndex > 0 && cpPatientInOutRecordOut.Qtcl1 == "") || this.rcmbOtherOut1.SelectedIndex < 1) &&
           ((this.rcmbOtherOut2.SelectedIndex > 0 && cpPatientInOutRecordOut.Qtcl2 == "") || this.rcmbOtherOut2.SelectedIndex < 1))
            {
                bIsEmpty[2] = true;
                //PublicMethod.RadAlterBox("病人小便量、大便次数、引流量、痰量、其它1、其它2至少填写一项!", "提示");                
            }

            if (cpTreatmentFlow.Zllc == "" || cpTreatmentFlow.Lcsm == "")
            {
                bIsEmpty[3] = true;
                //PublicMethod.RadAlterBox("主要治疗事件、事件说明不能为空!", "提示");
            }

            if (cpVitalSignSpecialRecord.Hzsg == "" && cpVitalSignSpecialRecord.Hztz == "" && cpVitalSignSpecialRecord.Hzfw == "" &&
           ((this.rcmbBloodType.SelectedIndex > 0 && cpVitalSignSpecialRecord.Hzxx == "") || this.rcmbBloodType.SelectedIndex < 1) &&
           ((this.rcmbHR.SelectedIndex > 0 && cpVitalSignSpecialRecord.Xyxx == "") || this.rcmbHR.SelectedIndex < 1) &&
           cpVitalSignSpecialRecord.Hzsss == "" && cpVitalSignSpecialRecord.Hzsxs == "" && cpVitalSignSpecialRecord.Hzgms == "")
            {
                bIsEmpty[4] = true;
                //PublicMethod.RadAlterBox("病人身高、体重、腹围、血型、血性HR、手术史、\n输血史、过敏史至少填写一项!", "提示");

            }

            if (bIsEmpty[0] && bIsEmpty[1] && bIsEmpty[2] && bIsEmpty[3] && bIsEmpty[4])
            {
                PublicMethod.RadAlterBox("未添加新项!", "提示");
                return;
            }
            #endregion

            #region 校验数据
            if (!bIsEmpty[0])
            {
                bIsSave[0] = CheckSaveVitalSign();
                if (!bIsSave[0]) return;//数据检验有错误,退出
            }

            if (!bIsEmpty[1])
            {
                bIsSave[1] = CheckSavePatientIn();
                if (!bIsSave[1]) return;//数据检验有错误,退出
            }

            if (!bIsEmpty[2])
            {
                bIsSave[2] = CheckSavePatientOut();
                if (!bIsSave[2]) return;//数据检验有错误,退出
            }

            if (!bIsEmpty[3])
            {
                bIsSave[3] = CheckSaveTreatmentFlow();
                if (!bIsSave[3]) return;//数据检验有错误,退出
            }

            if (!bIsEmpty[4])
            {
                bIsSave[4] = CheckSaveVitalSignSpecialRecord();
                if (!bIsSave[4]) return;//数据检验有错误,退出
            }
            #endregion

            CP_SaveNursingNotes m_CPSaveNursingNotes = new CP_SaveNursingNotes();

            #region 保存数据
            //生命体征主要数据项不为空，且数据格式正确，就保存，否则保存下一组数据
            if (bIsSave[0] && !bIsEmpty[0])
            {
                m_CPSaveNursingNotes.bSaveTag1 = true;
                m_CPSaveNursingNotes.CPVitalSignsRecord = cpVitalSignsRecord;
                //SaveVitalSign(cpVitalSignsRecord);
            }
            else
            {
                m_CPSaveNursingNotes.bSaveTag1 = false;
            }

            //病人入量主要数据项不为空，且数据格式正确，就保存，否则保存下一组数据
            if (bIsSave[1] && !bIsEmpty[1])
            {
                m_CPSaveNursingNotes.bSaveTag2 = true;
                m_CPSaveNursingNotes.CPPatientInOutRecordIn = cpPatientInOutRecordIn;
                //SavePatientIn(cpPatientInOutRecordIn);
            }
            else
            {
                m_CPSaveNursingNotes.bSaveTag2 = false;
            }

            //病人出量主要数据项不为空，且数据格式正确，就保存，否则保存下一组数据
            if (bIsSave[2] && !bIsEmpty[2])
            {
                m_CPSaveNursingNotes.bSaveTag3 = true;
                m_CPSaveNursingNotes.CPPatientInOutRecordOut = cpPatientInOutRecordOut;
                //SavePatientOut(cpPatientInOutRecordOut);
            }
            else
            {
                m_CPSaveNursingNotes.bSaveTag3 = false;
            }

            //病人治疗流程主要数据项不为空，且数据格式正确，就保存，否则保存下一组数据
            if (bIsSave[3] && !bIsEmpty[3])
            {
                m_CPSaveNursingNotes.bSaveTag4 = true;
                m_CPSaveNursingNotes.CPTreatmentFlow = cpTreatmentFlow;
                //SaveTreatmentFlow(cpTreatmentFlow);
            }
            else
            {
                m_CPSaveNursingNotes.bSaveTag4 = false;
            }

            //病人治疗流程主要数据项不为空，且数据格式正确，就保存，否则保存下一组数据
            if (bIsSave[4] && !bIsEmpty[4])
            {
                m_CPSaveNursingNotes.bSaveTag5 = true;
                m_CPSaveNursingNotes.CPVitalSignSpecialRecord = cpVitalSignSpecialRecord;
                //SaveVitalSignSpecialRecord(cpVitalSignSpecialRecord);
            }
            else
            {
                m_CPSaveNursingNotes.bSaveTag5 = false;
            }

            //保存
            SaveNursingNotes(m_CPSaveNursingNotes);
            #endregion

        }
        #endregion

        #region 保存护理记录单集
        /// <summary>
        /// 保存护理记录单集
        /// </summary>
        /// <param name="cp"></param>
        private void SaveNursingNotes(CP_SaveNursingNotes cp)
        {
            YidanEHRDataServiceClient SaveNursingNotesClient = PublicMethod.YidanClient;
            SaveNursingNotesClient.SaveNursingNotesCompleted +=
                 (obj, e) =>
                 {
                     if (e.Error == null)
                     {
                         if (e.Result.ToString() == "该时间段生命体征记录已存在!")
                         {
                             PublicMethod.RadAlterBox("该时间段生命体征记录已存在,操作失败!", "提示");
                         }
                         else
                         {
                             //数据重置
                             ResetVitalSign();
                             ResetIn();
                             ResetOut();
                             ResetIncident();
                             ResetOther();

                             PublicMethod.RadAlterBox("操作完成!", "提示");
                         }
                     }
                     else
                     {
                         PublicMethod.RadWaringBox(e.Error);
                     }
                 };
            SaveNursingNotesClient.SaveNursingNotesAsync(cp, Global.InpatientListCurrent.Zyhm);
            SaveNursingNotesClient.CloseAsync();
        }

       
        #endregion

        #region 保存生命体征数据

        #region 获取保存生命体征数据
        /// <summary>
        /// 获取保存生命体征数据
        /// </summary>
        /// <returns></returns>
        private CP_VitalSignsRecordInfo GetSaveVitalSignData()
        {
            CP_VitalSignsRecordInfo cp = new CP_VitalSignsRecordInfo();
            cp.Zyhm = Global.InpatientListCurrent.Zyhm;
            if (this.rdpRecordDateVitalSign.SelectedDate != null)
                cp.Clrq = this.rdpRecordDateVitalSign.SelectedDate.Value.ToString("yyyy/MM/dd");

            if (cmbRecordTimeVitalSign.SelectedIndex > 0)
                cp.Sjd = cmbRecordTimeVitalSign.SelectedItem.ToString();
            //if (this.tudRecordTimeVitalSign.Value.ToString() != "")
            //    cp.Clsj = ((DateTime)this.tudRecordTimeVitalSign.Value).ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 5);
            cp.Clsj = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 5);
            cp.Hzztdm = (this.rcmbPatientState.SelectedItem as CP_NurseCode).CodeID;
            cp.Hzzt = (this.rcmbPatientState.SelectedItem as CP_NurseCode).CodeName;
            cp.Hztw = this.txtTemperature.Text.Trim().ToString();
            cp.Clfsdm = (this.rcmbMeasuringMode.SelectedItem as CP_NurseCode).CodeID;
            cp.Clfs = (this.rcmbMeasuringMode.SelectedItem as CP_NurseCode).CodeName;
            cp.Fzcsdm = (this.rcmbCuoShi.SelectedItem as CP_NurseCode).CodeID;
            cp.Fzcs = (this.rcmbCuoShi.SelectedItem as CP_NurseCode).CodeName;
            //cp.Hzxy = this.txtBloodPressure.Text.Trim().ToString();
            cp.Hzxy = this.TextSeparateBoxControlBP.BP;
            cp.Hzmb = this.txtPulse.Text.Trim().ToString();
            cp.Hzxl = this.txtHeartRate.Text.Trim().ToString();
            cp.Hzhx = this.txtBreath.Text.Trim().ToString();
            cp.Qbq = (cp.Hzxl == "") ? 0 : (chkPacemaker.IsChecked == true) ? 1 : 0;
            cp.Hxq = (cp.Hzhx == "") ? 0 : (chkBreather.IsChecked == true) ? 1 : 0;
            cp.Djysdm = Global.LogInEmployee.Zgdm;
            cp.Djys = Global.LogInEmployee.Name;

            return cp;
        }
        #endregion

        #region 检验保存生命体征数据
        /// <summary>
        /// 检验保存生命体征数据
        /// </summary>
        private bool CheckSaveVitalSign()
        {
            double dVar = 0;
            int intVart = 0;

            #region 验证
            //记录日期
            if (this.rdpRecordDateVitalSign.SelectedDate == null)
            {
                PublicMethod.RadAlterBox("记录日期不能为空!", "提示");
                return false;
            }
            //记录时间段
            if (cmbRecordTimeVitalSign.SelectedIndex < 1)
            {
                PublicMethod.RadAlterBox("请选择时间段!", "提示");
                return false;
            }

            //记录时间
            //if (this.tudRecordTimeVitalSign.Value.ToString() == "")
            //{
            //    PublicMethod.RadAlterBox("记录时间不能为空或格式不正确!", "提示");
            //    return false;
            //}
            //体温
            if (txtTemperature.Text.Trim().ToString() != "")
            {
                if (!Dataprocessing.IsNumber(txtTemperature.Text.Trim().ToString(), 1))
                {
                    PublicMethod.RadAlterBox("体温必须为正数，小数点保留一位!", "提示");
                    return false;
                }
                else
                {
                    dVar = Convert.ToDouble(txtTemperature.Text.Trim().ToString());
                    if (!(dVar >= 34 && dVar <= 45))
                    {
                        PublicMethod.RadAlterBox("体温超出范围：34℃至45℃!", "提示");
                        return false;
                    }
                }
            }
            //脉搏
            if (txtPulse.Text.Trim().ToString() != "")
            {
                if (!Dataprocessing.IsNumber(txtPulse.Text.Trim().ToString(), 0))
                {
                    PublicMethod.RadAlterBox("脉搏必须为正整数!", "提示");
                    return false;
                }
                else
                {
                    intVart = Convert.ToInt32(txtPulse.Text.Trim().ToString());
                    if (!(intVart >= 1 && intVart <= 200))
                    {
                        PublicMethod.RadAlterBox("脉搏超出范围：1次/分至200/次/分!", "提示");
                        return false;
                    }
                }
            }
            //心率
            if (txtHeartRate.Text.Trim().ToString() != "")
            {
                if (!Dataprocessing.IsNumber(txtHeartRate.Text.Trim().ToString(), 0))
                {
                    PublicMethod.RadAlterBox("心率必须为正整数!", "提示");
                    return false;
                }
                else
                {
                    intVart = Convert.ToInt32(txtHeartRate.Text.Trim().ToString());
                    if (!(intVart >= 1 && intVart <= 200))
                    {
                        PublicMethod.RadAlterBox("心率超出范围：1次/分至200/次/分!", "提示");
                        return false;
                    }
                }
            }
            //呼吸
            if (txtBreath.Text.Trim().ToString() != "")
            {
                if (!Dataprocessing.IsNumber(txtBreath.Text.Trim().ToString(), 0))
                {
                    PublicMethod.RadAlterBox("呼吸必须为正整数!", "提示");
                    return false;
                }
                else
                {
                    intVart = Convert.ToInt32(txtBreath.Text.Trim().ToString());
                    if (!(intVart >= 1 && intVart <= 60))
                    {
                        PublicMethod.RadAlterBox("呼吸超出范围：1次/分至60/次/分!", "提示");
                        return false;
                    }
                }
            }

            if (!TextSeparateBoxControlBP.CheckData()) return false;
            #endregion

            //if (cp.Hztw == "" && cp.Hzmb == "" && cp.Hzxl == "" && cp.Hzhx == "" && !TextSeparateBoxControlBP.IsInput)
            //{
            //    PublicMethod.RadAlterBox("病人体温、脉搏、心率、呼吸、血压至少填写一项!", "提示");
            //    return false;
            //}

            return true;

        }
        #endregion

        #region 保存生命体征数据(不用)
        /// <summary>
        /// 保存生命体征数据
        /// </summary>
        private void SaveVitalSign(CP_VitalSignsRecordInfo cp)
        {

            radBusyIndicator.IsBusy = true;

            YidanEHRDataServiceClient SaveVitalSignClient = PublicMethod.YidanClient;
            SaveVitalSignClient.SaveVitalSignCompleted +=
                (obj, e) =>
                {
                    radBusyIndicator.IsBusy = false;

                    if (e.Error == null)
                    {
                        ResetVitalSign();
                        PublicMethod.RadAlterBox(e.Result.ToString(), "提示");
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            SaveVitalSignClient.SaveVitalSignAsync(cp, Global.InpatientListCurrent.Zyhm);
            SaveVitalSignClient.CloseAsync();
        }

  
        #endregion

        #endregion

        #region 保存病人入量数据

        #region 获取保存病人入量数据
        /// <summary>
        /// 获取保存病人入量数据
        /// </summary>
        /// <returns></returns>
        private CP_PatientInOutRecordInfo GetSavePatientInData()
        {
            CP_PatientInOutRecordInfo cp = new CP_PatientInOutRecordInfo();
            cp.Zyhm = Global.InpatientListCurrent.Zyhm;
            cp.Jllx = 0;//记录入量标示
            if (this.rdpRecordDateIn.SelectedDate != null)
                cp.Clrq = ((DateTime)this.rdpRecordDateIn.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss").Substring(0, 10);
            if (this.tudRecordTimeIn.Value.ToString() != "")
                cp.Clsj = ((DateTime)this.tudRecordTimeIn.Value).ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 5);
            cp.Ysl = this.txtFood.Text.Trim().ToString();
            cp.Hsl = this.txtWater.Text.Trim().ToString();
            cp.Syl = this.txtHangWater.Text.Trim().ToString();
            cp.Zsl = this.txtInject.Text.Trim().ToString();
            cp.Sxl = this.txtTransfuse.Text.Trim().ToString();
            cp.Qtrllxdm1 = (this.rcmbOtherIn1.SelectedItem as CP_NurseCode).CodeID;
            cp.Qtrllx1 = (this.rcmbOtherIn1.SelectedItem as CP_NurseCode).CodeName;
            cp.Qtrl1 = this.txtOtherIn1.Text.Trim().ToString();
            cp.Qtrllxdm2 = (this.rcmbOtherIn2.SelectedItem as CP_NurseCode).CodeID;
            cp.Qtrllx2 = (this.rcmbOtherIn2.SelectedItem as CP_NurseCode).CodeName;
            cp.Qtrl2 = this.txtOtherIn2.Text.Trim().ToString();
            cp.Djysdm = Global.LogInEmployee.Zgdm;
            cp.Djys = Global.LogInEmployee.Name;

            return cp;
        }
        #endregion

        #region 检验保存病人入量数据
        /// <summary>
        /// 检验保存病人入量数据
        /// </summary>
        private bool CheckSavePatientIn()
        {
            #region 验证
            if (this.rdpRecordDateIn.SelectedDate == null)
            {
                PublicMethod.RadAlterBox("记录日期不能为空!", "提示");
                return false;
            }
            if (this.tudRecordTimeIn.Value.ToString() == "")
            {
                PublicMethod.RadAlterBox("记录时间不能为空或格式不正确!", "提示");
                return false;
            }
            if (!Dataprocessing.IsNumber(txtFood.Text.Trim().ToString(), 0) && txtFood.Text.Trim().ToString() != "")
            {
                PublicMethod.RadAlterBox("饮食量必须为正整数!", "提示");
                return false;
            }
            if (!Dataprocessing.IsNumber(txtWater.Text.Trim().ToString(), 0) && txtWater.Text.Trim().ToString() != "")
            {
                PublicMethod.RadAlterBox("饮水量必须为正整数!", "提示");
                return false;
            }
            if (!Dataprocessing.IsNumber(txtHangWater.Text.Trim().ToString(), 0) && txtHangWater.Text.Trim().ToString() != "")
            {
                PublicMethod.RadAlterBox("输液量必须为正整数!", "提示");
                return false;
            }
            if (!Dataprocessing.IsNumber(txtInject.Text.Trim().ToString(), 0) && txtInject.Text.Trim().ToString() != "")
            {
                PublicMethod.RadAlterBox("注射量必须为正整数!", "提示");
                return false;
            }
            if (!Dataprocessing.IsNumber(txtTransfuse.Text.Trim().ToString(), 0) && txtTransfuse.Text.Trim().ToString() != "")
            {
                PublicMethod.RadAlterBox("输血量必须为正整数!", "提示");
                return false;
            }
            #endregion

            //if (cp.Ysl == "" && cp.Hsl == "" && cp.Syl == "" && cp.Zsl == "" && cp.Sxl == "" &&
            // ((this.rcmbOtherIn1.SelectedIndex > 0 && cp.Qtrl1 == "") || this.rcmbOtherIn1.SelectedIndex < 1) &&
            // ((this.rcmbOtherIn2.SelectedIndex > 0 && cp.Qtrl2 == "") || this.rcmbOtherIn2.SelectedIndex < 1))
            //{
            //    PublicMethod.RadAlterBox("病人饮食量、喝水量、输液量、注射量、\n输血量、其它1、其它2至少填写一项!", "提示");
            //    return;
            //}

            return true;

        }
        #endregion

        #region 保存病人入量数据(不用)
        /// <summary>
        /// 保存病人入量数据
        /// </summary>
        private void SavePatientIn(CP_PatientInOutRecordInfo cp)
        {
            radBusyIndicator.IsBusy = true;

            YidanEHRDataServiceClient SavePatientInClient = PublicMethod.YidanClient;
            SavePatientInClient.SavePatientInCompleted +=
                   (obj, e) =>
                   {
                       radBusyIndicator.IsBusy = false;

                       if (e.Error == null)
                       {
                           ResetIn();
                           PublicMethod.RadAlterBox(e.Result.ToString(), "提示");
                       }
                       else
                       {
                           PublicMethod.RadWaringBox(e.Error);
                       }
                   };
            SavePatientInClient.SavePatientInAsync(cp);
            SavePatientInClient.CloseAsync();
        }

    
        #endregion
        #endregion

        #region 保存病人出量数据

        #region 获取保存病人出量数据
        /// <summary>
        /// 获取保存病人出量数据
        /// </summary>
        /// <returns></returns>
        private CP_PatientInOutRecordInfo GetSavePatientOutData()
        {
            CP_PatientInOutRecordInfo cp = new CP_PatientInOutRecordInfo();
            cp.Zyhm = Global.InpatientListCurrent.Zyhm;
            cp.Jllx = 1;//记录出量标示
            if (this.rdpRecordDateOut.SelectedDate != null)
                cp.Clrq = ((DateTime)this.rdpRecordDateOut.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss").Substring(0, 10);
            if (this.tudRecordTimeOut.Value.ToString() != "")
                cp.Clsj = ((DateTime)this.tudRecordTimeOut.Value).ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 5);
            cp.Hzxb = this.txtPee.Text.Trim().ToString();
            cp.Xbxzdm = (this.rcmbPeeProperty.SelectedItem as CP_NurseCode).CodeID;
            cp.Xbxz = (this.rcmbPeeProperty.SelectedItem as CP_NurseCode).CodeName;
            cp.Xbcsdm = (this.rcmbPeeLabour.SelectedItem as CP_NurseCode).CodeID;
            cp.Xbcs = (this.rcmbPeeLabour.SelectedItem as CP_NurseCode).CodeName;
            cp.Yll = this.txtDrain.Text.Trim().ToString();
            cp.Ylsm = this.txtDrainRemark.Text.Trim().ToString();
            //cp.Dbcs = this.txtShitTime.Text.Trim().ToString();
            cp.Dbcs = TextGroupBoxControlShit.Shit;
            cp.Dbxzdm = (this.rcmbShitProperty.SelectedItem as CP_NurseCode).CodeID;
            cp.Dbxz = (this.rcmbShitProperty.SelectedItem as CP_NurseCode).CodeName;
            cp.Pbcsdm = (this.rcmbShitLabour.SelectedItem as CP_NurseCode).CodeID;
            cp.Pbcs = (this.rcmbShitLabour.SelectedItem as CP_NurseCode).CodeName;
            cp.Hztl = this.txtSputum.Text.Trim().ToString();
            cp.Txzdm = (this.rcmbSputumProperty.SelectedItem as CP_NurseCode).CodeID;
            cp.Txz = (this.rcmbSputumProperty.SelectedItem as CP_NurseCode).CodeName;
            cp.Qtcllxdm1 = (this.rcmbOtherOut1.SelectedItem as CP_NurseCode).CodeID;
            cp.Qtcllx1 = (this.rcmbOtherOut1.SelectedItem as CP_NurseCode).CodeName;
            cp.Qtcl1 = this.txtOtherOut1.Text.Trim().ToString();
            cp.Qtcllxdm2 = (this.rcmbOtherOut2.SelectedItem as CP_NurseCode).CodeID;
            cp.Qtcllx2 = (this.rcmbOtherOut2.SelectedItem as CP_NurseCode).CodeName;
            cp.Qtcl2 = this.txtOtherOut2.Text.Trim().ToString();
            cp.Djysdm = Global.LogInEmployee.Zgdm;
            cp.Djys = Global.LogInEmployee.Name;


            return cp;
        }
        #endregion

        #region 检验保存病人出量数据
        /// <summary>
        /// 检验保存病人出量数据
        /// </summary>
        private bool CheckSavePatientOut()
        {
            #region 验证
            if (this.rdpRecordDateOut.SelectedDate == null)
            {
                PublicMethod.RadAlterBox("记录日期不能为空!", "提示");
                return false;
            }
            if (this.tudRecordTimeOut.Value.ToString() == "")
            {
                PublicMethod.RadAlterBox("记录时间不能为空或格式不正确!", "提示");
                return false;
            }
            if (!Dataprocessing.IsNumber(txtPee.Text.Trim().ToString(), 0) && txtPee.Text.Trim().ToString() != "")
            {
                PublicMethod.RadAlterBox("小便量必须为正整数!", "提示");
                return false;
            }
            if (!Dataprocessing.IsNumber(txtDrain.Text.Trim().ToString(), 0) && txtDrain.Text.Trim().ToString() != "")
            {
                PublicMethod.RadAlterBox("引流量必须为正整数!", "提示");
                return false;
            }
            if (!Dataprocessing.IsNumber(txtSputum.Text.Trim().ToString(), 0) && txtSputum.Text.Trim().ToString() != "")
            {
                PublicMethod.RadAlterBox("痰量必须为正整数!", "提示");
                return false;
            }
            #endregion

            //  if (cp.Hzxb == "" && cp.Yll == "" && !TextGroupBoxControlShit.IsInput && cp.Hztl == "" &&
            // ((this.rcmbOtherOut1.SelectedIndex > 0 && cp.Qtcl1 == "") || this.rcmbOtherOut1.SelectedIndex < 1) &&
            // ((this.rcmbOtherOut2.SelectedIndex > 0 && cp.Qtcl2 == "") || this.rcmbOtherOut2.SelectedIndex < 1))
            //{
            //    PublicMethod.RadAlterBox("病人小便量、大便次数、引流量、痰量、其它1、其它2至少填写一项!", "提示");
            //    return;
            //}


            return true;

        }
        #endregion

        #region 保存病人出量数据(不用)
        /// <summary>
        /// 保存病人出量数据
        /// </summary>
        private void SavePatientOut(CP_PatientInOutRecordInfo cp)
        {
            radBusyIndicator.IsBusy = true;

            YidanEHRDataServiceClient SavePatientOutClient = PublicMethod.YidanClient;
            SavePatientOutClient.SavePatientOutCompleted +=
                 (obj, e) =>
                 {
                     radBusyIndicator.IsBusy = false;

                     if (e.Error == null)
                     {
                         ResetOut();
                         PublicMethod.RadAlterBox(e.Result.ToString(), "提示");
                     }
                     else
                     {
                         PublicMethod.RadWaringBox(e.Error);
                     }
                 };
            SavePatientOutClient.SavePatientOutAsync(cp);
            SavePatientOutClient.CloseAsync();
        }

       
        #endregion
        #endregion

        #region 保存保存病人治疗主要事件

        #region 获取保存病人治疗主要事件
        /// <summary>
        /// 获取保存病人出量数据
        /// </summary>
        /// <returns></returns>
        private CP_TreatmentFlowInfo GetSaveTreatmentFlowData()
        {
            CP_TreatmentFlowInfo cp = new CP_TreatmentFlowInfo();
            cp.Zyhm = Global.InpatientListCurrent.Zyhm;
            if (this.rdpRecordDateIncident.SelectedDate != null)
                cp.Clrq = ((DateTime)this.rdpRecordDateIncident.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss").Substring(0, 10);
            if (this.tudRecordTimeIncident.Value.ToString() != "")
                cp.Clsj = ((DateTime)this.tudRecordTimeIncident.Value).ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 5);
            cp.Zllc = this.txtTreatmentFlow.Text.Trim().ToString();
            cp.Lcsm = this.txtTreatmentFlowRemark.Text.Trim().ToString();
            cp.Sfss = chkOperation.IsChecked == true ? 1 : 0;
            cp.Djysdm = Global.LogInEmployee.Zgdm;
            cp.Djys = Global.LogInEmployee.Name;

            return cp;
        }
        #endregion

        #region 检验保存病人治疗主要事件
        /// <summary>
        /// 检验保存病人治疗主要事件
        /// </summary>
        private bool CheckSaveTreatmentFlow()
        {
            #region 验证
            if (this.rdpRecordDateIncident.SelectedDate == null)
            {
                PublicMethod.RadAlterBox("记录日期不能为空!", "提示");
                return false;
            }
            if (this.tudRecordTimeIncident.Value.ToString() == "")
            {
                PublicMethod.RadAlterBox("记录时间不能为空或格式不正确!", "提示");
                return false;
            }
            #endregion

            //if (cp.Zllc == "" || cp.Lcsm == "")
            //{
            //    PublicMethod.RadAlterBox("主要治疗事件、事件说明不能为空!", "提示");
            //    return;
            //}

            return true;

        }
        #endregion

        #region 保存病人治疗主要事件(不用)
        /// <summary>
        /// 保存病人治疗主要事件
        /// </summary>
        private void SaveTreatmentFlow(CP_TreatmentFlowInfo cp)
        {
            radBusyIndicator.IsBusy = true;

            YidanEHRDataServiceClient SaveTreatmentFlowClient = PublicMethod.YidanClient;
            SaveTreatmentFlowClient.SaveTreatmentFlowCompleted +=
                   (obj, e) =>
                   {
                       radBusyIndicator.IsBusy = false;

                       if (e.Error == null)
                       {
                           ResetIncident();
                           PublicMethod.RadAlterBox(e.Result.ToString(), "提示");
                       }
                       else
                       {
                           PublicMethod.RadWaringBox(e.Error);
                       }
                   };
            SaveTreatmentFlowClient.SaveTreatmentFlowAsync(cp);
            SaveTreatmentFlowClient.CloseAsync();
        }

     
        #endregion

        #endregion

        #region 保存病人特殊护理记录

        #region 获取保存病人特殊护理记录
        /// <summary>
        /// 获取保存病人特殊护理记录
        /// </summary>
        /// <returns></returns>
        private CP_VitalSignSpecialRecordInfo GetSaveVitalSignSpecialRecordData()
        {
            CP_VitalSignSpecialRecordInfo cp = new CP_VitalSignSpecialRecordInfo();
            cp.Zyhm = Global.InpatientListCurrent.Zyhm;
            if (this.rdpRecordDateOther.SelectedDate != null)
                cp.Clrq = ((DateTime)this.rdpRecordDateOther.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss").Substring(0, 10);
            if (this.tudRecordTimeOther.Value.ToString() != "")
                cp.Clsj = ((DateTime)this.tudRecordTimeOther.Value).ToString("yyyy-MM-dd HH:mm:ss").Substring(11, 5);
            cp.Hzsg = this.txtHeight.Text.Trim().ToString();
            cp.Hztz = this.txtWeight.Text.Trim().ToString();
            cp.Hzfw = this.txtFuWei.Text.Trim().ToString();
            cp.Hzxxdm = (this.rcmbBloodType.SelectedItem as CP_NurseCode).CodeID;
            cp.Hzxx = (this.rcmbBloodType.SelectedItem as CP_NurseCode).CodeName;
            cp.Xyxxdm = (this.rcmbHR.SelectedItem as CP_NurseCode).CodeID;
            cp.Xyxx = (this.rcmbHR.SelectedItem as CP_NurseCode).CodeName;
            cp.Hzsss = this.txtOperationHistory.Text.Trim().ToString();
            cp.Hzsxs = this.txtTransfusionsHistory.Text.Trim().ToString();
            cp.Hzgms = this.txtAllergicHistory.Text.Trim().ToString();
            cp.Djysdm = Global.LogInEmployee.Zgdm;
            cp.Djys = Global.LogInEmployee.Name;

            cp.Ljdm = Global.InpatientListCurrent.Ljdm;
            cp.Ljxh = Global.InpatientListCurrent.Ljxh;
            //cp.ActivityChildId = 


            return cp;
        }
        #endregion

        #region 检验保存病人特殊护理记录
        /// <summary>
        /// 检验保存病人特殊护理记录
        /// </summary>
        private bool CheckSaveVitalSignSpecialRecord()
        {
            #region 验证
            if (this.rdpRecordDateOther.SelectedDate == null)
            {
                PublicMethod.RadAlterBox("记录日期不能为空!", "提示");
                return false;
            }
            if (this.tudRecordTimeOther.Value.ToString() == "")
            {
                PublicMethod.RadAlterBox("记录时间不能为空或格式不正确!", "提示");
                return false;
            }
            if (!Dataprocessing.IsNumber(txtHeight.Text.Trim().ToString(), 0) && txtHeight.Text.Trim().ToString() != "")
            {
                PublicMethod.RadAlterBox("身高必须为正整数!", "提示");
                return false;
            }
            if (!Dataprocessing.IsNumber(txtWeight.Text.Trim().ToString(), 0) && txtWeight.Text.Trim().ToString() != "")
            {
                PublicMethod.RadAlterBox("体重必须为正整数!", "提示");
                return false;
            }
            if (!Dataprocessing.IsNumber(txtFuWei.Text.Trim().ToString(), 0) && txtFuWei.Text.Trim().ToString() != "")
            {
                PublicMethod.RadAlterBox("腹围必须为正整数!", "提示");
                return false;
            }

            #endregion

            // if (cp.Hzsg == "" && cp.Hztz == "" && cp.Hzfw == "" &&
            //((this.rcmbBloodType.SelectedIndex > 0 && cp.Hzxx == "") || this.rcmbBloodType.SelectedIndex < 1) &&
            //((this.rcmbHR.SelectedIndex > 0 && cp.Xyxx == "") || this.rcmbHR.SelectedIndex < 1) &&
            //cp.Hzsss == "" && cp.Hzsxs == "" && cp.Hzgms == "")
            // {
            //     PublicMethod.RadAlterBox("病人身高、体重、腹围、血型、血性HR、手术史、\n输血史、过敏史至少填写一项!", "提示");
            //     return;
            // }

            return true;

        }
        #endregion

        #region 保存病人特殊护理记录(不用)
        /// <summary>
        /// 保存病人特殊护理记录
        /// </summary>
        private void SaveVitalSignSpecialRecord(CP_VitalSignSpecialRecordInfo cp)
        {
            radBusyIndicator.IsBusy = true;

            YidanEHRDataServiceClient SaveVitalSignSpecialRecordClient = PublicMethod.YidanClient;
            SaveVitalSignSpecialRecordClient.SaveVitalSignSpecialRecordCompleted +=
                  (obj, e) =>
                  {
                      radBusyIndicator.IsBusy = false;

                      if (e.Error == null)
                      {
                          ResetOther();
                          PublicMethod.RadAlterBox(e.Result.ToString(), "提示");
                      }
                      else
                      {
                          PublicMethod.RadWaringBox(e.Error);
                      }
                  };
            SaveVitalSignSpecialRecordClient.SaveVitalSignSpecialRecordAsync(cp);
            SaveVitalSignSpecialRecordClient.CloseAsync();
        }

    
        #endregion
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
                CheckAndSaveData();
            }

            #region 删除
            //if ((e as DialogBoxShow.OpreateEventArgs).bResult == true)  //选择确定按钮
            //{
            //    switch ((tabControl.SelectedItem as Telerik.Windows.Controls.RadTabItem).Header.ToString())
            //    {
            //        case "生命体征"://保存生命体征数据
            //            {
            //                SaveVitalSign();
            //                break;
            //            }
            //        case "入量记录"://保存病人入量数据
            //            {
            //                SavePatientIn();
            //                break;
            //            }
            //        case "出量记录"://保存病人出量数据
            //            {
            //                SavePatientOut();
            //                break;
            //            }
            //        case "治疗流程"://保存病人治疗主要事件
            //            {
            //                SaveTreatmentFlow();
            //                break;
            //            }
            //        case "其它记录"://保存病人特殊护理记录
            //            {
            //                SaveVitalSignSpecialRecord();
            //                break;
            //            }
            //    }
            //}
            #endregion

        }
        #endregion

        #region 重置操作
        #region 重置按钮
        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnReset_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                switch ((tabControl.SelectedItem as Telerik.Windows.Controls.RadTabItem).Header.ToString())
                {
                    case "生命体征"://生命体征数据重置
                        {
                            ResetVitalSign();
                            break;
                        }
                    case "入量记录"://病人入量数据重置
                        {
                            ResetIn();
                            break;
                        }
                    case "出量记录"://病人出量数据重置
                        {
                            ResetOut();
                            break;
                        }
                    case "治疗流程"://病人主要治疗流程数据重置
                        {
                            ResetIncident();
                            break;
                        }
                    case "其它记录"://病人特殊记录
                        {
                            ResetOther();
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

        #region 生命体征数据重置
        /// <summary>
        /// 生命体征数据重置
        /// </summary>
        private void ResetVitalSign()
        {
            this.rdpRecordDateVitalSign.SelectedDate = DateTime.Now;
            cmbRecordTimeVitalSign.SelectedIndex = 0;
            //this.tudRecordTimeVitalSign.Value = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            this.rcmbPatientState.SelectedIndex = 0;
            this.txtTemperature.Text = "";
            this.rcmbMeasuringMode.SelectedIndex = 0;
            this.rcmbCuoShi.SelectedIndex = 0;
            //this.txtBloodPressure.Text="";
            this.TextSeparateBoxControlBP.DateTextReset();
            this.txtPulse.Text = "";
            this.txtHeartRate.Text = "";
            this.txtBreath.Text = "";
            chkPacemaker.IsChecked = false;
            chkBreather.IsChecked = false;
        }
        #endregion

        #region 病人入量数据重置
        /// <summary>
        /// 病人入量数据重置
        /// </summary>
        private void ResetIn()
        {
            this.rdpRecordDateIn.SelectedDate = DateTime.Now;
            this.tudRecordTimeIn.Value = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            this.txtFood.Text = "";
            this.txtWater.Text = "";
            this.txtHangWater.Text = "";
            this.txtInject.Text = "";
            this.txtTransfuse.Text = "";
            this.rcmbOtherIn1.SelectedIndex = 0;
            this.txtOtherIn1.Text = "";
            this.rcmbOtherIn2.SelectedIndex = 0;
            this.txtOtherIn2.Text = "";
        }
        #endregion

        #region 病人出量数据重置
        /// <summary>
        /// 病人出量数据重置
        /// </summary>
        private void ResetOut()
        {
            this.rdpRecordDateOut.SelectedDate = DateTime.Now;
            this.tudRecordTimeOut.Value = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            this.txtPee.Text = "";
            this.rcmbPeeProperty.SelectedIndex = 0;
            this.rcmbPeeLabour.SelectedIndex = 0;
            this.txtDrain.Text = "";
            this.txtDrainRemark.Text = "";
            //this.txtShitTime.Text="";
            this.TextGroupBoxControlShit.DateTextReset();
            this.rcmbShitProperty.SelectedIndex = 0;
            this.rcmbShitLabour.SelectedIndex = 0;
            this.txtSputum.Text = "";
            this.rcmbSputumProperty.SelectedIndex = 0;
            this.rcmbOtherOut1.SelectedIndex = 0;
            this.txtOtherOut1.Text = "";
            this.rcmbOtherOut2.SelectedIndex = 0;
            this.txtOtherOut2.Text = "";
        }
        #endregion

        #region 病人主要治疗流程数据重置
        /// <summary>
        /// 病人主要治疗流程数据重置
        /// </summary>
        private void ResetIncident()
        {
            this.rdpRecordDateIncident.SelectedDate = DateTime.Now;
            this.tudRecordTimeIncident.Value = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            this.txtTreatmentFlow.Text = "";
            this.txtTreatmentFlowRemark.Text = "";
            chkOperation.IsChecked = false;
        }
        #endregion

        #region 病人特殊记录
        /// <summary>
        /// 病人特殊记录
        /// </summary>
        private void ResetOther()
        {
            this.rdpRecordDateOther.SelectedDate = DateTime.Now;
            this.tudRecordTimeOther.Value = Convert.ToDateTime(DateTime.Now.ToShortTimeString());
            this.txtHeight.Text = "";
            this.txtWeight.Text = "";
            this.txtFuWei.Text = "";
            this.rcmbBloodType.SelectedIndex = 0;
            this.rcmbHR.SelectedIndex = 0;
            this.txtOperationHistory.Text = "";
            this.txtTransfusionsHistory.Text = "";
            this.txtAllergicHistory.Text = "";
        }
        #endregion

        #endregion

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RadWindow_Unloaded(object sender, RoutedEventArgs e)
        {

            page.QueryNursingNotesInfo();
        }



    }
}
