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
using System.Collections.ObjectModel;
using YidanSoft.Tool;
using YidanEHRApplication.Helpers;
using Telerik.Windows.Controls;
using System.Diagnostics;

namespace YidanEHRApplication.Views.ChildWindows
{
    /// <summary>
    /// Interaction logic for RadChildWindowQuitPath.xaml
    /// </summary>
    public partial class RWEnterConditionMaintain
    {
        public RWEnterConditionMaintain(CP_ClinicalPathList path)
        {
            InitializeComponent();
            _Path = path;
        }
        #region 属性
        CP_ClinicalPathList _Path = null;
        OperationState state = OperationState.NEW;
        public OperationState CurrentOperationState
        {
            get
            {

                return state;
            }
            set
            {
                if (value == OperationState.NEW)
                {
                    txtOperationState.Text = "当前状态：新增";
                    ClearControlValue();
                    cmbConditionType.IsEnabled = true;
                }
                if (value == OperationState.EDIT)
                {
                    txtOperationState.Text = "当前状态：编辑";
                    cmbConditionType.IsEnabled = false;

                }
                state = value;
            }
        }
        CP_PathEnterJudgeCondition _CP_PathEnterJudgeCondition = new CP_PathEnterJudgeCondition();
        String ID;
        #endregion
        #region 事件
        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        { try{
            CurrentOperationState = OperationState.NEW;
            YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
            #region 绑定类型下拉框

            KeyValues keyValues = new KeyValues();
            keyValues.Add(new KeyValue("2", "ICD-10"));
            keyValues.Add(new KeyValue("1", "检查项"));
            cmbConditionType.ItemsSource = keyValues;
            cmbConditionType.DisplayMemberPath = "Value";
            cmbConditionType.SelectedValuePath = "Key";
            cmbConditionType.SelectedValue = "2";
            #endregion
            #region 绑定ICD自动完成框
            referenceClient.GetCP_PathDiagnosisListAllCompleted += (send, ea) =>
                      {

                          autoCompleteICD10.ItemsSource = ea.Result;
                          autoCompleteICD10.ItemFilter = DeptFilter;
                      };
            referenceClient.GetCP_PathDiagnosisListAllAsync();
            #endregion
            #region 绑定非ICD自动完成框

            referenceClient.GetCP_ExamDictionaryDetailAllCompleted += (send2, ea2) =>
                {
                    autoCompleteNonICD10.ItemsSource = ea2.Result;
                    autoCompleteNonICD10.ItemFilter = DeptFilterNonICD;
                };
            referenceClient.GetCP_ExamDictionaryDetailAllAsync();
            #endregion
            #region 绑定列表
            BindGridView();
            #endregion
            referenceClient.CloseAsync();
             }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }
        private void autoCompleteNonICD10_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {try{
            if (e.AddedItems.Count > 0)
            {
                CP_ExamDictionaryDetail detail = (CP_ExamDictionaryDetail)e.AddedItems[0];

                foreach (SuitCrowdMapScope scope in detail.SuitCrowsMapScopes)
                {
                    if (scope.ExamSyrq.Jlxh == "1")
                    {
                        numStart_1.Value = Convert.ToDouble(scope.Ksfw);
                        numEnd_1.Value = Convert.ToDouble(scope.Jsfw);
                    }
                    if (scope.ExamSyrq.Jlxh == "2")
                    {
                        numStart_2.Value = Convert.ToDouble(scope.Ksfw);
                        numEnd_2.Value = Convert.ToDouble(scope.Jsfw);
                    }
                    if (scope.ExamSyrq.Jlxh == "3")
                    {
                        numStart_3.Value = Convert.ToDouble(scope.Ksfw);
                        numEnd_3.Value = Convert.ToDouble(scope.Jsfw);
                    }
                    if (scope.ExamSyrq.Jlxh == "4")
                    {
                        numStart_4.Value = Convert.ToDouble(scope.Ksfw);
                        numEnd_4.Value = Convert.ToDouble(scope.Jsfw);
                    }
                    if (scope.ExamSyrq.Jlxh == "5")
                    {
                        numStart_5.Value = Convert.ToDouble(scope.Ksfw);
                        numEnd_5.Value = Convert.ToDouble(scope.Jsfw);
                    }
                    if (scope.ExamSyrq.Jlxh == "6")
                    {
                        numStart_6.Value = Convert.ToDouble(scope.Ksfw);
                        numEnd_6.Value = Convert.ToDouble(scope.Jsfw);
                    }
                }
                //  Debug.WriteLine("Zdbs:{0},Name:{1}", ((CP_ExamDictionaryDetail)e.AddedItems[0]).Jlxh, ((CP_ExamDictionaryDetail)e.AddedItems[0]).Jcmc);

            } }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CurrentOperationState = OperationState.NEW;
            _CP_PathEnterJudgeCondition = new CP_PathEnterJudgeCondition();

        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
         {
            #region SaveCheck

            #endregion

            _CP_PathEnterJudgeCondition.Ljdm = _Path.Ljdm;
            _CP_PathEnterJudgeCondition.Lb = 1;
            _CP_PathEnterJudgeCondition.Syrq = "";
            _CP_PathEnterJudgeCondition.Ksfw = "";
            _CP_PathEnterJudgeCondition.Jsfw = "";
            if (((KeyValue)cmbConditionType.SelectedItem).Key == "1")//检查
            {
                if (autoCompleteNonICD10.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("检查项必须填写！", "提示");
                    return;
                }
                #region 适用人群和范围
                if (numEnd_6.Value > numStart_6.Value)
                {
                    _CP_PathEnterJudgeCondition.Syrq = ",6";
                    _CP_PathEnterJudgeCondition.Ksfw = "," + ConvertMy.ToString(numStart_6.Value);
                    _CP_PathEnterJudgeCondition.Jsfw = "," + ConvertMy.ToString(numEnd_6.Value);
                }
                if (numEnd_5.Value > numStart_5.Value)
                {
                    _CP_PathEnterJudgeCondition.Syrq += ",5";
                    _CP_PathEnterJudgeCondition.Ksfw += "," + ConvertMy.ToString(numStart_5.Value);
                    _CP_PathEnterJudgeCondition.Jsfw += "," + ConvertMy.ToString(numEnd_5.Value);
                } if (numEnd_4.Value > numStart_4.Value)
                {
                    _CP_PathEnterJudgeCondition.Syrq += ",4";
                    _CP_PathEnterJudgeCondition.Ksfw += "," + ConvertMy.ToString(numStart_4.Value);
                    _CP_PathEnterJudgeCondition.Jsfw += "," + ConvertMy.ToString(numEnd_4.Value);
                } if (numEnd_3.Value > numStart_3.Value)
                {
                    _CP_PathEnterJudgeCondition.Syrq += ",3";
                    _CP_PathEnterJudgeCondition.Ksfw += "," + ConvertMy.ToString(numStart_3.Value);
                    _CP_PathEnterJudgeCondition.Jsfw += "," + ConvertMy.ToString(numEnd_3.Value);
                } if (numEnd_2.Value > numStart_2.Value)
                {
                    _CP_PathEnterJudgeCondition.Syrq += ",2";
                    _CP_PathEnterJudgeCondition.Ksfw += "," + ConvertMy.ToString(numStart_2.Value);
                    _CP_PathEnterJudgeCondition.Jsfw += "," + ConvertMy.ToString(numEnd_2.Value);
                } if (numEnd_1.Value > numStart_1.Value)
                {
                    _CP_PathEnterJudgeCondition.Syrq += ",1";
                    _CP_PathEnterJudgeCondition.Ksfw += "," + ConvertMy.ToString(numStart_1.Value);
                    _CP_PathEnterJudgeCondition.Jsfw += "," + ConvertMy.ToString(numEnd_1.Value);
                }
                _CP_PathEnterJudgeCondition.Syrq = _CP_PathEnterJudgeCondition.Syrq.IndexOf(',') > -1 ? _CP_PathEnterJudgeCondition.Syrq.Substring(1) : "";
                _CP_PathEnterJudgeCondition.Ksfw = _CP_PathEnterJudgeCondition.Ksfw.IndexOf(',') > -1 ? _CP_PathEnterJudgeCondition.Ksfw.Substring(1) : "";
                _CP_PathEnterJudgeCondition.Jsfw = _CP_PathEnterJudgeCondition.Jsfw.IndexOf(',') > -1 ? _CP_PathEnterJudgeCondition.Jsfw.Substring(1) : "";

                #endregion
                _CP_PathEnterJudgeCondition.Xmlb = 1;
                _CP_PathEnterJudgeCondition.Jcxm = ((CP_ExamDictionaryDetail)autoCompleteNonICD10.SelectedItem).Jcbm;
            }
            if (((KeyValue)cmbConditionType.SelectedItem).Key == "2")//ICD
            {
                if (autoCompleteICD10.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("ICD-10必须填写！", "提示");
                    return;
                }
                _CP_PathEnterJudgeCondition.Xmlb = 2;
                _CP_PathEnterJudgeCondition.Jcxm = ((CP_Diagnosis_E)autoCompleteICD10.SelectedItem).Zdbs;

            }
            if (CurrentOperationState == OperationState.NEW)
            {
                foreach (var item in (ObservableCollection<CP_PathEnterJudgeCondition>)GrdConditonList.ItemsSource)
                {
                    if (item.Jcxm == _CP_PathEnterJudgeCondition.Jcxm)
                    {
                        PublicMethod.RadAlterBox("已经存在该条件", "提示");
                        return;
                    }
                }
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                referenceClient.GetInsertCP_PathEnterJudgeConditionCompleted += (send, ea) => { BindGridView(); };
                referenceClient.GetInsertCP_PathEnterJudgeConditionAsync(_CP_PathEnterJudgeCondition);
                referenceClient.CloseAsync();


            }
            if (CurrentOperationState == OperationState.EDIT)
            {
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                referenceClient.GetUpdateCP_PathEnterJudgeConditionCompleted += (send, ea) => { BindGridView(); };
                referenceClient.GetUpdateCP_PathEnterJudgeConditionAsync(_CP_PathEnterJudgeCondition);
                referenceClient.CloseAsync();

            }
         }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
      
        private void btnDeleteCondition_Click(object sender, RoutedEventArgs e)
        {
            ID = ((RadButton)sender).Tag.ToString();
            RadWindow.Confirm("确定删除吗？", ConfirmClose);
        }
        void ConfirmClose(object sender, WindowClosedEventArgs e)
        {
            if (Convert.ToBoolean(e.DialogResult))
            {
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                referenceClient.GetDeleteCP_PathEnterJudgeConditionCompleted += (send, ea) => { BindGridView(); };
                referenceClient.GetDeleteCP_PathEnterJudgeConditionAsync(ID);
                referenceClient.CloseAsync();
            }
        }
        private void GrdConditonList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {try{
            if (GrdConditonList.SelectedItem == null)
            {
                return;
            }
            _CP_PathEnterJudgeCondition = (CP_PathEnterJudgeCondition)GrdConditonList.SelectedItem;
            cmbConditionType.SelectedValue = _CP_PathEnterJudgeCondition.Xmlb.ToString();
            CurrentOperationState = OperationState.EDIT;
            if (_CP_PathEnterJudgeCondition.Xmlb == 2)
            {
                autoCompleteICD10.SelectedItem = ((ObservableCollection<CP_Diagnosis_E>)autoCompleteICD10.ItemsSource).First(cp => cp.Zdbs.Equals(_CP_PathEnterJudgeCondition.Jcxm));

            }
            if (_CP_PathEnterJudgeCondition.Xmlb == 1)
            {
                ClearControlValue();

                autoCompleteNonICD10.SelectedItem = ((ObservableCollection<CP_ExamDictionaryDetail>)autoCompleteNonICD10.ItemsSource).First(cp => cp.Jcbm.Equals(_CP_PathEnterJudgeCondition.Jcxm));
                foreach (SuitCrowdMapScope scope in _CP_PathEnterJudgeCondition.SuitCrowsMapScopes)
                {
                    if (scope.ExamSyrq.Jlxh == "1")
                    {
                        numStart_1.Value = Convert.ToDouble(scope.Ksfw);
                        numEnd_1.Value = Convert.ToDouble(scope.Jsfw);
                    }
                    if (scope.ExamSyrq.Jlxh == "2")
                    {
                        numStart_2.Value = Convert.ToDouble(scope.Ksfw);
                        numEnd_2.Value = Convert.ToDouble(scope.Jsfw);
                    }
                    if (scope.ExamSyrq.Jlxh == "3")
                    {
                        numStart_3.Value = Convert.ToDouble(scope.Ksfw);
                        numEnd_3.Value = Convert.ToDouble(scope.Jsfw);
                    }
                    if (scope.ExamSyrq.Jlxh == "4")
                    {
                        numStart_4.Value = Convert.ToDouble(scope.Ksfw);
                        numEnd_4.Value = Convert.ToDouble(scope.Jsfw);
                    }
                    if (scope.ExamSyrq.Jlxh == "5")
                    {
                        numStart_5.Value = Convert.ToDouble(scope.Ksfw);
                        numEnd_5.Value = Convert.ToDouble(scope.Jsfw);
                    }
                    if (scope.ExamSyrq.Jlxh == "6")
                    {
                        numStart_6.Value = Convert.ToDouble(scope.Ksfw);
                        numEnd_6.Value = Convert.ToDouble(scope.Jsfw);
                    }
                }
            }
             }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }
        private void cmbConditionType_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {try{
            if (e.AddedItems.Count > 0)
            {
                Visibility ICD = ((KeyValue)e.AddedItems[0]).Key == "2" ? Visibility.Visible : Visibility.Collapsed;
                Visibility NonICD = ((KeyValue)e.AddedItems[0]).Key == "1" ? Visibility.Visible : Visibility.Collapsed;
                stkICD10.Visibility = ICD;
                stkNonICD10.Visibility = NonICD;
                wrpNonICD.Visibility = NonICD;
            } }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }
        #endregion
        #region 函数


      
        private bool DeptFilter(string strFilter, object item)
        {
            CP_Diagnosis_E deptList = (CP_Diagnosis_E)item;
            return ((deptList.Py.StartsWith(strFilter)) || (deptList.Py.Contains(strFilter)) || deptList.Zdbs.StartsWith(strFilter) || deptList.Zdbs.Contains(strFilter));
        }
        private bool DeptFilterNonICD(string strFilter, object item)
        {
            CP_ExamDictionaryDetail deptList = (CP_ExamDictionaryDetail)item;
            return ((deptList.Py.StartsWith(strFilter)) || (deptList.Wb.Contains(strFilter)) || deptList.Jlxh.StartsWith(strFilter) || deptList.Jlxh.Contains(strFilter) || deptList.Jcmc.StartsWith(strFilter) || deptList.Jcmc.Contains(strFilter));
        }

        private void BindGridView()
        {
            if (_Path == null || _Path.Ljdm == null || _Path.Ljdm.ToString().Trim() == "") return;
            YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
            referenceClient.GetPathCP_PathEnterJudgeConditionAllCompleted += (send, ea) =>
            {

                GrdConditonList.ItemsSource = ea.Result;
            };
            referenceClient.GetPathCP_PathEnterJudgeConditionAllAsync(_Path.Ljdm);
            referenceClient.CloseAsync();
        }
        private void ClearControlValue()
        {
            autoCompleteNonICD10.SelectedItem = null;

            numStart_1.Value = 0;
            numStart_2.Value = 0;
            numStart_3.Value = 0;
            numStart_4.Value = 0;
            numStart_5.Value = 0;
            numStart_6.Value = 0;

            numEnd_1.Value = 0;
            numEnd_2.Value = 0;
            numEnd_3.Value = 0;
            numEnd_4.Value = 0;
            numEnd_5.Value = 0;
            numEnd_6.Value = 0;


        }
        #endregion


    }
}
