using DrectSoft.Tool;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Views.ChildWindows
{
    /// <summary>
    /// Interaction logic for RadChildWindowQuitPath.xaml
    /// </summary>
    public partial class RWEnterConditionMaintainNode
    {
        #region 属性
        String _LjdmNode = null;
        String _NodeGUIDNode = null;
        bool _GoTo = true;
        //定义一个操作状态变量
        OperationState stateNode = OperationState.NEW;
        //构造操作状态类型
        public OperationState CurrentOperationStateNode
        {
            get
            {

                return stateNode;
            }
            set
            {
                if (value == OperationState.NEW)
                {
                    // txtOperationStateNode.Text = "当前状态：新增";
                    ClearControlValueNode();
                    this.cmbConditionTypeNode.IsEnabled = true;

                }
                if (value == OperationState.EDIT)
                {
                    //txtOperationStateNode.Text = "当前状态：编辑";
                    this.cmbConditionTypeNode.IsEnabled = true;

                }
                if (value == OperationState.VIEW)
                {
                    //txtOperationStateNode.Text = "当前状态：查看";
                    this.cmbConditionTypeNode.IsEnabled = false;

                }
                //查看编辑项时，按钮状态可用
                btnAdd.IsEnabled = value == OperationState.VIEW;
                btnUpdate.IsEnabled = value == OperationState.VIEW;
                btnDel.IsEnabled = value == OperationState.VIEW;

                //查看编辑项时，按钮状态不可用
                btnClear.IsEnabled = value != OperationState.VIEW;
                btnSave.IsEnabled = value != OperationState.VIEW;
                btnReset.IsEnabled = value != OperationState.VIEW;

                this.txtDescrib.IsEnabled = value != OperationState.VIEW;
                autoCompleteNonICD10Node.IsEnabled = value != OperationState.VIEW;
                this.numStart_1Node.IsEnabled = value != OperationState.VIEW;
                this.numEnd_1Node.IsEnabled = value != OperationState.VIEW;
                this.cmbConditionTypeNode.IsEnabled = value != OperationState.VIEW;
                //this.radBusyIndicator.IsEnabled = value != OperationState.VIEW;




                stateNode = value;
            }
        }
        //定义一个实体类对象
        CP_PathEnterJudgeCondition _CP_PathEnterJudgeConditionNode = new CP_PathEnterJudgeCondition();
        //2013-04-12,WangGuojin
        CP_PathEnterJudgeCondition _CP_PathEnterJudgeConditionNodeOld = new CP_PathEnterJudgeCondition();
        String IDNode;
        #endregion
        #region 事件
        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!_GoTo)
                {
                    _GoTo = true;
                    return;
                }
                CurrentOperationStateNode = OperationState.VIEW;
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                //RegisterKeyEvent();
                btnAdd.IsEnabled = true;
                btnUpdate.IsEnabled = true;
                btnDel.IsEnabled = true;
                btnClear.IsEnabled = false;
                btnSave.IsEnabled = false;
                btnReset.IsEnabled = false;
                this.txtDescrib.IsEnabled = false;
                this.autoCompleteNonICD10Node.IsEnabled = false;
                this.numStart_1Node.IsEnabled = false;
                this.numEnd_1Node.IsEnabled = false;


                #region 绑定类型下拉框

                KeyValues keyValues = new KeyValues();
                //keyValues.Add(new KeyValue("2", "ICD-10"));
                keyValues.Add(new KeyValue("1", "检查项"));
                keyValues.Add(new KeyValue("3", "描述项"));
                cmbConditionTypeNode.ItemsSource = keyValues;
                cmbConditionTypeNode.DisplayMemberPath = "Value";
                cmbConditionTypeNode.SelectedValuePath = "Key";
                cmbConditionTypeNode.SelectedValue = "1";
                //cmbConditionTypeNode.IsEnabled = false;
                #endregion

                #region 绑定非ICD自动完成框

                referenceClient.GetCP_ExamDictionaryDetailAllCompleted += (send2, ea2) =>
                    {
                        autoCompleteNonICD10Node.ItemsSource = ea2.Result;
                        autoCompleteNonICD10Node.ItemFilter = DeptFilterNonICDNode;
                    };
                referenceClient.GetCP_ExamDictionaryDetailAllAsync();
                #endregion
                #region 绑定列表
                BindGridViewNode();
                #endregion
                referenceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }


        }
        /// <summary>
        /// 检查项选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoCompleteNonICD10Node_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (e.AddedItems.Count > 0)
                {
                    CP_ExamDictionaryDetail detail = (CP_ExamDictionaryDetail)e.AddedItems[0];
                    this.txtDanWei.Text = detail.Jsdw.ToString().Trim();
                    foreach (SuitCrowdMapScope scope in detail.SuitCrowsMapScopes)
                    {
                        if (scope.ExamSyrq.Jlxh == "1")
                        {
                            numStart_1Node.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_1Node.Value = Convert.ToDouble(scope.Jsfw);
                        }
                        if (scope.ExamSyrq.Jlxh == "2")
                        {
                            numStart_2Node.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_2Node.Value = Convert.ToDouble(scope.Jsfw);
                        }
                        if (scope.ExamSyrq.Jlxh == "3")
                        {
                            numStart_3Node.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_3Node.Value = Convert.ToDouble(scope.Jsfw);
                        }
                        if (scope.ExamSyrq.Jlxh == "4")
                        {
                            numStart_4Node.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_4Node.Value = Convert.ToDouble(scope.Jsfw);
                        }
                        if (scope.ExamSyrq.Jlxh == "5")
                        {
                            numStart_5Node.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_5Node.Value = Convert.ToDouble(scope.Jsfw);
                        }
                        if (scope.ExamSyrq.Jlxh == "6")
                        {
                            numStart_6Node.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_6Node.Value = Convert.ToDouble(scope.Jsfw);
                        }
                    }
                    //  Debug.WriteLine("Zdbs:{0},Name:{1}", ((CP_ExamDictionaryDetail)e.AddedItems[0]).Jlxh, ((CP_ExamDictionaryDetail)e.AddedItems[0]).Jcmc);

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 添加数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            CurrentOperationStateNode = OperationState.NEW;
            _CP_PathEnterJudgeConditionNode = new CP_PathEnterJudgeCondition();
            cmbConditionTypeNode.Focus();
            //cmbConditionTypeNode.IsDropDownOpen = true;

        }
        /// <summary>
        /// 修改数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (GrdConditonListNode.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择一个记录", "提示");
                    //Reset();
                    return;
                }

                //获得该行数据对象
                _CP_PathEnterJudgeConditionNode = (CP_PathEnterJudgeCondition)GrdConditonListNode.SelectedItem;
                //2013.04.12,WangGuojin
                _CP_PathEnterJudgeConditionNodeOld = (CP_PathEnterJudgeCondition)GrdConditonListNode.SelectedItem;

                cmbConditionTypeNode.SelectedValue = _CP_PathEnterJudgeConditionNode.Xmlb.ToString();
                CurrentOperationStateNode = OperationState.VIEW;
                if (_CP_PathEnterJudgeConditionNode.Xmlb == 3)
                {
                    txtDescrib.Text = _CP_PathEnterJudgeConditionNode.JcxmName;

                }
                if (_CP_PathEnterJudgeConditionNode.Xmlb == 1)
                {
                    this.autoCompleteNonICD10Node.Text = "";
                    ClearControlValueNode();

                    autoCompleteNonICD10Node.SelectedItem = ((ObservableCollection<CP_ExamDictionaryDetail>)autoCompleteNonICD10Node.ItemsSource).First(cp => cp.Jcbm.Equals(_CP_PathEnterJudgeConditionNode.Jcxm));
                    this.txtDanWei.Text = _CP_PathEnterJudgeConditionNode.Dw.ToString();
                    foreach (SuitCrowdMapScope scope in _CP_PathEnterJudgeConditionNode.SuitCrowsMapScopes)
                    {
                        if (scope.ExamSyrq.Jlxh == "1")
                        {
                            numStart_1Node.Value = Convert.ToDouble(scope.Ksfw);
                            numEnd_1Node.Value = Convert.ToDouble(scope.Jsfw);
                        }
                        //if (scope.ExamSyrq.Jlxh == "2")
                        //{
                        //    numStart_2Node.Value = Convert.ToDouble(scope.Ksfw);
                        //    numEnd_2Node.Value = Convert.ToDouble(scope.Jsfw);
                        //}
                        //if (scope.ExamSyrq.Jlxh == "3")
                        //{
                        //    numStart_3Node.Value = Convert.ToDouble(scope.Ksfw);
                        //    numEnd_3Node.Value = Convert.ToDouble(scope.Jsfw);
                        //}
                        //if (scope.ExamSyrq.Jlxh == "4")
                        //{
                        //    numStart_4Node.Value = Convert.ToDouble(scope.Ksfw);
                        //    numEnd_4Node.Value = Convert.ToDouble(scope.Jsfw);
                        //}
                        //if (scope.ExamSyrq.Jlxh == "5")
                        //{
                        //    numStart_5Node.Value = Convert.ToDouble(scope.Ksfw);
                        //    numEnd_5Node.Value = Convert.ToDouble(scope.Jsfw);
                        //}
                        //if (scope.ExamSyrq.Jlxh == "6")
                        //{
                        //    numStart_6Node.Value = Convert.ToDouble(scope.Ksfw);
                        //    numEnd_6Node.Value = Convert.ToDouble(scope.Jsfw);
                        //}
                    }
                }
                CurrentOperationStateNode = OperationState.EDIT;

                cmbConditionTypeNode.Focus();
                //2013.04.12,WangGuojin.
                //cmbConditionTypeNode.IsDropDownOpen = true;


            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 删除操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GrdConditonListNode.SelectedItem != null)
                {
                    //获得该行数据对象
                    _CP_PathEnterJudgeConditionNode = (CP_PathEnterJudgeCondition)GrdConditonListNode.SelectedItem;
                    IDNode = _CP_PathEnterJudgeConditionNode.ID.ToString();

                    //RadWindow.Confirm("确定删除吗？", ConfirmCloseNode);
                    #region 删除提示 update by luff 2012-08-17
                    //DialogParameters parameters = new DialogParameters();/* update by luff 2012-8-16 删除提示 */
                    //parameters.Content = String.Format("{0}", "确定删除吗？删除后不能恢复!");
                    //parameters.Header = "提示";
                    //parameters.IconContent = null;
                    //parameters.OkButtonContent = "确定";
                    //parameters.CancelButtonContent = "取消";
                    //parameters.Closed = ConfirmCloseNode;
                    //RadWindow.Confirm(parameters);

                    YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                    mess.ShowDialog();
                    mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
                    Reset();
                    //this.cmbConditionTypeNode.SelectedIndex = 0;
                    //txtDescrib.Text = "";
                    //ClearControlValueNode();
                }
                else
                {
                    PublicMethod.RadAlterBox("请选择一条记录！", "提示");
                }
                    #endregion
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }


        }
        void mess_PageClosedEvent(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                    referenceClient.GetDeleteCP_PathEnterJudgeConditionCompleted += (send, ea) => { BindGridViewNode(); };
                    referenceClient.GetDeleteCP_PathEnterJudgeConditionAsync(IDNode);
                    referenceClient.CloseAsync();

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        //private void btnDeleteConditionNode_Click(object sender, RoutedEventArgs e)
        //{
        //    IDNode = ((RadButton)sender).Tag.ToString();
        //    RadWindow.Confirm("确定删除吗？", ConfirmCloseNode);
        //}
        void ConfirmCloseNode(object sender, WindowClosedEventArgs e)
        {
            try
            {
                if (Convert.ToBoolean(e.DialogResult))
                {
                    YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                    referenceClient.GetDeleteCP_PathEnterJudgeConditionCompleted += (send, ea) => { BindGridViewNode(); };
                    referenceClient.GetDeleteCP_PathEnterJudgeConditionAsync(IDNode);
                    referenceClient.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 数据保存
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                _CP_PathEnterJudgeConditionNode.Ljdm = _LjdmNode;
                _CP_PathEnterJudgeConditionNode.Jddm = _NodeGUIDNode;
                _CP_PathEnterJudgeConditionNode.Dw = this.txtDanWei.Text.Trim();
                _CP_PathEnterJudgeConditionNode.Lb = 2;
                _CP_PathEnterJudgeConditionNode.Syrq = "";
                _CP_PathEnterJudgeConditionNode.Ksfw = "";
                _CP_PathEnterJudgeConditionNode.Jsfw = "";
                if (((KeyValue)cmbConditionTypeNode.SelectedItem).Key == "1")//检查
                {
                    if (autoCompleteNonICD10Node.SelectedItem == null)
                    {
                        //PublicMethod.RadAlterBox("检查项不能为空！", "提示");
                        #region  判断检查项必须填写并获得当前控件焦点

                        PublicMethod.RadAlterBoxRe("检查项不能为空(检查项为编码输入选择项)", "提示", autoCompleteNonICD10Node);
                        _GoTo = false;
                        return;
                        #endregion

                    }
                    #region 适用人群和范围
                    //if (numEnd_6Node.Value <= numStart_6Node.Value || numEnd_5Node.Value <= numStart_5Node.Value || numEnd_4Node.Value <= numStart_4Node.Value || numEnd_3Node.Value <= numStart_3Node.Value || numEnd_2Node.Value <= numStart_2Node.Value || numEnd_1Node.Value <= numStart_1Node.Value)

                    if (numEnd_1Node.Value <= numStart_1Node.Value)
                    {
                        PublicMethod.RadAlterBox("条件范围前面的值必须小于后面的值", "提示");
                        return;
                    }
                    else
                    {
                        _CP_PathEnterJudgeConditionNode.Syrq += ",1";
                        _CP_PathEnterJudgeConditionNode.Ksfw += "," + ConvertMy.ToString(ConvertMy.ToDecimal(numStart_1Node.Value));
                        _CP_PathEnterJudgeConditionNode.Jsfw += "," + ConvertMy.ToString(ConvertMy.ToDecimal(numEnd_1Node.Value));
                    }
                    //if (numEnd_6Node.Value > numStart_6Node.Value)
                    //{
                    //    _CP_PathEnterJudgeConditionNode.Syrq = ",6";
                    //    _CP_PathEnterJudgeConditionNode.Ksfw = "," + ConvertMy.ToString(numStart_6Node.Value);
                    //    _CP_PathEnterJudgeConditionNode.Jsfw = "," + ConvertMy.ToString(numEnd_6Node.Value);
                    //}
                    //if (numEnd_5Node.Value > numStart_5Node.Value)
                    //{
                    //    _CP_PathEnterJudgeConditionNode.Syrq += ",5";
                    //    _CP_PathEnterJudgeConditionNode.Ksfw += "," + ConvertMy.ToString(numStart_5Node.Value);
                    //    _CP_PathEnterJudgeConditionNode.Jsfw += "," + ConvertMy.ToString(numEnd_5Node.Value);
                    //} 
                    //if (numEnd_4Node.Value > numStart_4Node.Value)
                    //{
                    //    _CP_PathEnterJudgeConditionNode.Syrq += ",4";
                    //    _CP_PathEnterJudgeConditionNode.Ksfw += "," + ConvertMy.ToString(numStart_4Node.Value);
                    //    _CP_PathEnterJudgeConditionNode.Jsfw += "," + ConvertMy.ToString(numEnd_4Node.Value);
                    //} 
                    //if (numEnd_3Node.Value > numStart_3Node.Value)
                    //{
                    //    _CP_PathEnterJudgeConditionNode.Syrq += ",3";
                    //    _CP_PathEnterJudgeConditionNode.Ksfw += "," + ConvertMy.ToString(numStart_3Node.Value);
                    //    _CP_PathEnterJudgeConditionNode.Jsfw += "," + ConvertMy.ToString(numEnd_3Node.Value);
                    //} 
                    //if (numEnd_2Node.Value > numStart_2Node.Value)
                    //{
                    //    _CP_PathEnterJudgeConditionNode.Syrq += ",2";
                    //    _CP_PathEnterJudgeConditionNode.Ksfw += "," + ConvertMy.ToString(numStart_2Node.Value);
                    //    _CP_PathEnterJudgeConditionNode.Jsfw += "," + ConvertMy.ToString(numEnd_2Node.Value);
                    //} 
                    //if (numEnd_1Node.Value > numStart_1Node.Value)
                    //{
                    //    _CP_PathEnterJudgeConditionNode.Syrq += ",1";
                    //    _CP_PathEnterJudgeConditionNode.Ksfw += "," + ConvertMy.ToString(ConvertMy.ToDecimal( numStart_1Node.Value));
                    //    _CP_PathEnterJudgeConditionNode.Jsfw += "," + ConvertMy.ToString(ConvertMy.ToDecimal( numEnd_1Node.Value));
                    //}
                    _CP_PathEnterJudgeConditionNode.Syrq = _CP_PathEnterJudgeConditionNode.Syrq.IndexOf(',') > -1 ? _CP_PathEnterJudgeConditionNode.Syrq.Substring(1) : "";
                    _CP_PathEnterJudgeConditionNode.Ksfw = _CP_PathEnterJudgeConditionNode.Ksfw.IndexOf(',') > -1 ? _CP_PathEnterJudgeConditionNode.Ksfw.Substring(1) : "";
                    _CP_PathEnterJudgeConditionNode.Jsfw = _CP_PathEnterJudgeConditionNode.Jsfw.IndexOf(',') > -1 ? _CP_PathEnterJudgeConditionNode.Jsfw.Substring(1) : "";

                    #endregion
                    _CP_PathEnterJudgeConditionNode.Xmlb = 1;
                    _CP_PathEnterJudgeConditionNode.Jcxm = ((CP_ExamDictionaryDetail)autoCompleteNonICD10Node.SelectedItem).Jcbm;
                }
                if (((KeyValue)cmbConditionTypeNode.SelectedItem).Key == "3")//ICD
                {
                    if (txtDescrib.Text.Trim() == "")
                    {
                        //PublicMethod.RadAlterBox("描述内容不能为空！", "提示");
                        #region  判断描述内容必须填写并获得当前控件焦点
                        PublicMethod.RadAlterBoxRe("描述内容不能为空", "提示", txtDescrib);
                        _GoTo = false;
                        return;
                        #endregion

                    }
                    _CP_PathEnterJudgeConditionNode.Xmlb = 3;
                    _CP_PathEnterJudgeConditionNode.Jcxm = txtDescrib.Text; //((CP_Diagnosis_E)autoCompleteICD10.SelectedItem).Zdbs;
                }
                if (CurrentOperationStateNode == OperationState.NEW)
                {
                    foreach (var item in (ObservableCollection<CP_PathEnterJudgeCondition>)GrdConditonListNode.ItemsSource)
                    {
                        if (item.Jcxm == _CP_PathEnterJudgeConditionNode.Jcxm && item.Xmlb == _CP_PathEnterJudgeConditionNode.Xmlb)
                        {
                            PublicMethod.RadAlterBox("已经存在该条件", "提示");
                            return;
                        }
                    }

                    YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                    referenceClient.GetInsertCP_PathEnterJudgeConditionCompleted += (send, ea) => { BindGridViewNode(); };
                    referenceClient.GetInsertCP_PathEnterJudgeConditionAsync(_CP_PathEnterJudgeConditionNode);
                    referenceClient.CloseAsync();
                    CurrentOperationStateNode = OperationState.VIEW;

                }
                if (CurrentOperationStateNode == OperationState.EDIT)
                {
                    //2013.04.12,WangGuojin, Add Update duplicate checked
                    if (_CP_PathEnterJudgeConditionNodeOld.JcxmName != autoCompleteNonICD10Node.Text)
                    {
                        foreach (var item in (ObservableCollection<CP_PathEnterJudgeCondition>)GrdConditonListNode.ItemsSource)
                        {
                            if (item.JcxmName == autoCompleteNonICD10Node.Text && item.Xmlb == _CP_PathEnterJudgeConditionNode.Xmlb)
                            {
                                PublicMethod.RadAlterBox("已经存在该条件", "提示");
                                return;
                            }
                        }
                    }
                    //==================================================================
                    YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                    referenceClient.GetUpdateCP_PathEnterJudgeConditionCompleted += (send, ea) => { BindGridViewNode(); };
                    referenceClient.GetUpdateCP_PathEnterJudgeConditionAsync(_CP_PathEnterJudgeConditionNode);
                    referenceClient.CloseAsync();
                    CurrentOperationStateNode = OperationState.VIEW;

                }
                txtDescrib.Text = string.Empty;
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        /// <summary>
        /// 重置操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            Reset();
        }
        /// <summary>
        /// 取消操作
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            Reset();
            CurrentOperationStateNode = OperationState.VIEW;
        }


        /// <summary>
        /// Gird行选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GrdConditonListNode_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            try
            {
                _CP_PathEnterJudgeConditionNode = (CP_PathEnterJudgeCondition)GrdConditonListNode.SelectedItem;
                if (GrdConditonListNode.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择一个记录", "提示");
                    return;
                }
                //获得该行数据对象
                CP_PathEnterJudgeCondition _CP_PathEnterJudgeCondition1 = (CP_PathEnterJudgeCondition)GrdConditonListNode.SelectedItem;
                if (_CP_PathEnterJudgeConditionNode != null && CurrentOperationStateNode == OperationState.EDIT)
                {

                    if (_CP_PathEnterJudgeConditionNode.ID == _CP_PathEnterJudgeCondition1.ID)
                    {
                        #region 没有切换行数据 add by luff 2012-08-17
                        cmbConditionTypeNode.SelectedValue = _CP_PathEnterJudgeCondition1.Xmlb.ToString();
                        CurrentOperationStateNode = OperationState.VIEW;
                        if (_CP_PathEnterJudgeCondition1.Xmlb == 3)
                        {
                            txtDescrib.Text = _CP_PathEnterJudgeCondition1.JcxmName;

                        }
                        if (_CP_PathEnterJudgeCondition1.Xmlb == 1)
                        {
                            this.autoCompleteNonICD10Node.Text = "";
                            ClearControlValueNode();

                            autoCompleteNonICD10Node.SelectedItem = ((ObservableCollection<CP_ExamDictionaryDetail>)autoCompleteNonICD10Node.ItemsSource).First(cp => cp.Jcbm.Equals(_CP_PathEnterJudgeCondition1.Jcxm));
                            this.txtDanWei.Text = _CP_PathEnterJudgeCondition1.Dw.ToString();
                            foreach (SuitCrowdMapScope scope in _CP_PathEnterJudgeCondition1.SuitCrowsMapScopes)
                            {
                                if (scope.ExamSyrq.Jlxh == "1")
                                {
                                    numStart_1Node.Value = Convert.ToDouble(scope.Ksfw);
                                    numEnd_1Node.Value = Convert.ToDouble(scope.Jsfw);
                                }
                                //if (scope.ExamSyrq.Jlxh == "2")
                                //{
                                //    numStart_2Node.Value = Convert.ToDouble(scope.Ksfw);
                                //    numEnd_2Node.Value = Convert.ToDouble(scope.Jsfw);
                                //}
                                //if (scope.ExamSyrq.Jlxh == "3")
                                //{
                                //    numStart_3Node.Value = Convert.ToDouble(scope.Ksfw);
                                //    numEnd_3Node.Value = Convert.ToDouble(scope.Jsfw);
                                //}
                                //if (scope.ExamSyrq.Jlxh == "4")
                                //{
                                //    numStart_4Node.Value = Convert.ToDouble(scope.Ksfw);
                                //    numEnd_4Node.Value = Convert.ToDouble(scope.Jsfw);
                                //}
                                //if (scope.ExamSyrq.Jlxh == "5")
                                //{
                                //    numStart_5Node.Value = Convert.ToDouble(scope.Ksfw);
                                //    numEnd_5Node.Value = Convert.ToDouble(scope.Jsfw);
                                //}
                                //if (scope.ExamSyrq.Jlxh == "6")
                                //{
                                //    numStart_6Node.Value = Convert.ToDouble(scope.Ksfw);
                                //    numEnd_6Node.Value = Convert.ToDouble(scope.Jsfw);
                                //}
                            }

                        }
                        #endregion
                    }
                    else
                    {
                        PublicMethod.RadAlterBox("请保存上一次操作的数据！", "提示");
                        #region 切换行前数据 add by luff 2012-08-17
                        cmbConditionTypeNode.SelectedValue = _CP_PathEnterJudgeConditionNode.Xmlb.ToString();
                        CurrentOperationStateNode = OperationState.VIEW;
                        if (_CP_PathEnterJudgeConditionNode.Xmlb == 3)
                        {
                            txtDescrib.Text = _CP_PathEnterJudgeConditionNode.JcxmName;

                        }
                        if (_CP_PathEnterJudgeConditionNode.Xmlb == 1)
                        {
                            this.autoCompleteNonICD10Node.Text = "";
                            ClearControlValueNode();

                            autoCompleteNonICD10Node.SelectedItem = ((ObservableCollection<CP_ExamDictionaryDetail>)autoCompleteNonICD10Node.ItemsSource).First(cp => cp.Jcbm.Equals(_CP_PathEnterJudgeConditionNode.Jcxm));
                            this.txtDanWei.Text = _CP_PathEnterJudgeConditionNode.Dw.ToString();
                            foreach (SuitCrowdMapScope scope in _CP_PathEnterJudgeConditionNode.SuitCrowsMapScopes)
                            {
                                if (scope.ExamSyrq.Jlxh == "1")
                                {
                                    numStart_1Node.Value = Convert.ToDouble(scope.Ksfw);
                                    numEnd_1Node.Value = Convert.ToDouble(scope.Jsfw);
                                }
                                //if (scope.ExamSyrq.Jlxh == "2")
                                //{
                                //    numStart_2Node.Value = Convert.ToDouble(scope.Ksfw);
                                //    numEnd_2Node.Value = Convert.ToDouble(scope.Jsfw);
                                //}
                                //if (scope.ExamSyrq.Jlxh == "3")
                                //{
                                //    numStart_3Node.Value = Convert.ToDouble(scope.Ksfw);
                                //    numEnd_3Node.Value = Convert.ToDouble(scope.Jsfw);
                                //}
                                //if (scope.ExamSyrq.Jlxh == "4")
                                //{
                                //    numStart_4Node.Value = Convert.ToDouble(scope.Ksfw);
                                //    numEnd_4Node.Value = Convert.ToDouble(scope.Jsfw);
                                //}
                                //if (scope.ExamSyrq.Jlxh == "5")
                                //{
                                //    numStart_5Node.Value = Convert.ToDouble(scope.Ksfw);
                                //    numEnd_5Node.Value = Convert.ToDouble(scope.Jsfw);
                                //}
                                //if (scope.ExamSyrq.Jlxh == "6")
                                //{
                                //    numStart_6Node.Value = Convert.ToDouble(scope.Ksfw);
                                //    numEnd_6Node.Value = Convert.ToDouble(scope.Jsfw);
                                //}
                            }
                        }
                        #endregion
                        return;

                    }

                }
                else
                {
                    #region 当前行数据 add by luff 2012-08-17
                    cmbConditionTypeNode.SelectedValue = _CP_PathEnterJudgeCondition1.Xmlb.ToString();
                    CurrentOperationStateNode = OperationState.VIEW;
                    if (_CP_PathEnterJudgeCondition1.Xmlb == 3)
                    {
                        txtDescrib.Text = _CP_PathEnterJudgeCondition1.JcxmName;

                    }
                    if (_CP_PathEnterJudgeCondition1.Xmlb == 1)
                    {
                        this.autoCompleteNonICD10Node.Text = "";
                        ClearControlValueNode();

                        autoCompleteNonICD10Node.SelectedItem = ((ObservableCollection<CP_ExamDictionaryDetail>)autoCompleteNonICD10Node.ItemsSource).First(cp => cp.Jcbm.Equals(_CP_PathEnterJudgeCondition1.Jcxm));
                        this.txtDanWei.Text = _CP_PathEnterJudgeCondition1.Dw.ToString();
                        foreach (SuitCrowdMapScope scope in _CP_PathEnterJudgeCondition1.SuitCrowsMapScopes)
                        {
                            if (scope.ExamSyrq.Jlxh == "1")
                            {
                                numStart_1Node.Value = Convert.ToDouble(scope.Ksfw);
                                numEnd_1Node.Value = Convert.ToDouble(scope.Jsfw);
                            }
                            //if (scope.ExamSyrq.Jlxh == "2")
                            //{
                            //    numStart_2Node.Value = Convert.ToDouble(scope.Ksfw);
                            //    numEnd_2Node.Value = Convert.ToDouble(scope.Jsfw);
                            //}
                            //if (scope.ExamSyrq.Jlxh == "3")
                            //{
                            //    numStart_3Node.Value = Convert.ToDouble(scope.Ksfw);
                            //    numEnd_3Node.Value = Convert.ToDouble(scope.Jsfw);
                            //}
                            //if (scope.ExamSyrq.Jlxh == "4")
                            //{
                            //    numStart_4Node.Value = Convert.ToDouble(scope.Ksfw);
                            //    numEnd_4Node.Value = Convert.ToDouble(scope.Jsfw);
                            //}
                            //if (scope.ExamSyrq.Jlxh == "5")
                            //{
                            //    numStart_5Node.Value = Convert.ToDouble(scope.Ksfw);
                            //    numEnd_5Node.Value = Convert.ToDouble(scope.Jsfw);
                            //}
                            //if (scope.ExamSyrq.Jlxh == "6")
                            //{
                            //    numStart_6Node.Value = Convert.ToDouble(scope.Ksfw);
                            //    numEnd_6Node.Value = Convert.ToDouble(scope.Jsfw);
                            //}
                        }

                    }
                    #endregion
                }


            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        #endregion
        #region 函数
        /// <summary>
        /// 重置方法
        /// </summary>
        private void Reset()
        {
            try
            {
                /*2013.04.12,WangGuojin,deleted
                this.txtDanWei.Text = string.Empty;
                if (_CP_PathEnterJudgeConditionNode.Xmlb == 3)
                {
                    txtDescrib.Text = string.Empty;

                }
                if (_CP_PathEnterJudgeConditionNode.Xmlb == 1)
                {
                    ClearControlValueNode();

                    //autoCompleteNonICD10Node.SelectedItem = ((ObservableCollection<CP_ExamDictionaryDetail>)autoCompleteNonICD10Node.ItemsSource).First(cp => cp.Jcbm.Equals(_CP_PathEnterJudgeConditionNode.Jcxm));
                    //foreach (SuitCrowdMapScope scope in _CP_PathEnterJudgeConditionNode.SuitCrowsMapScopes)
                    //{
                    //    if (scope.ExamSyrq.Jlxh == "1")
                    //    {
                    //        numStart_1Node.Value = Convert.ToDouble(0);
                    //        numEnd_1Node.Value = Convert.ToDouble(0);
                    //    }
                    //    if (scope.ExamSyrq.Jlxh == "2")
                    //    {
                    //        numStart_2Node.Value = Convert.ToDouble(0);
                    //        numEnd_2Node.Value = Convert.ToDouble(0);
                    //    }
                    //    if (scope.ExamSyrq.Jlxh == "3")
                    //    {
                    //        numStart_3Node.Value = Convert.ToDouble(0);
                    //        numEnd_3Node.Value = Convert.ToDouble(0);
                    //    }
                    //    if (scope.ExamSyrq.Jlxh == "4")
                    //    {
                    //        numStart_4Node.Value = Convert.ToDouble(0);
                    //        numEnd_4Node.Value = Convert.ToDouble(0);
                    //    }
                    //    if (scope.ExamSyrq.Jlxh == "5")
                    //    {
                    //        numStart_5Node.Value = Convert.ToDouble(0);
                    //        numEnd_5Node.Value = Convert.ToDouble(0);
                    //    }
                    //    if (scope.ExamSyrq.Jlxh == "6")
                    //    {
                    //        numStart_6Node.Value = Convert.ToDouble(0);
                    //        numEnd_6Node.Value = Convert.ToDouble(0);
                    //    }
                    //}
                }
                 */
                //2013.04.12,WangGuojin.
                //this.GrdConditonListNode.SelectedItem = null;
                this.txtDanWei.Text = string.Empty;
                txtDescrib.Text = string.Empty;
                cmbConditionTypeNode.SelectedIndex = 0;
                txtDescrib.Visibility = Visibility.Collapsed;
                cmbConditionTypeNode.Visibility = Visibility.Visible;
                ClearControlValueNode();
                cmbConditionTypeNode.Focus();
                //cmbConditionTypeNode.IsDropDownOpen = true;
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        public RWEnterConditionMaintainNode(String ljdm, String nodeGUID)
        {
            //页面初始化
            InitializeComponent();
            //去掉鼠标右键Silverlight 菜单
            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };

            _LjdmNode = ljdm;
            _NodeGUIDNode = nodeGUID;
        }
        private bool DeptFilterNode(string strFilter, object item)
        {
            CP_Diagnosis_E deptList = (CP_Diagnosis_E)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())) || deptList.Zdbs.StartsWith(strFilter.ToUpper()) || deptList.Zdbs.Contains(strFilter.ToUpper()));
        }
        private bool DeptFilterNonICDNode(string strFilter, object item)
        {
            CP_ExamDictionaryDetail deptList = (CP_ExamDictionaryDetail)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())) || deptList.Jlxh.StartsWith(strFilter.ToUpper()) || deptList.Jlxh.Contains(strFilter.ToUpper()));
        }

        private void BindGridViewNode()
        {
            if (_NodeGUIDNode == null || _NodeGUIDNode.ToString().Trim() == "") return;
            YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
            referenceClient.GetNodeCP_PathEnterJudgeConditionAllCompleted += (send, ea) =>
            {

                GrdConditonListNode.ItemsSource = ea.Result;
            };
            referenceClient.GetNodeCP_PathEnterJudgeConditionAllAsync(_NodeGUIDNode);
            referenceClient.CloseAsync();
        }
        private void ClearControlValueNode()
        {
            autoCompleteNonICD10Node.SelectedItem = null;
            this.autoCompleteNonICD10Node.Text = string.Empty;
            numStart_1Node.Value = 0;
            numStart_2Node.Value = 0;
            numStart_3Node.Value = 0;
            numStart_4Node.Value = 0;
            numStart_5Node.Value = 0;
            numStart_6Node.Value = 0;

            numEnd_1Node.Value = 0;
            numEnd_2Node.Value = 0;
            numEnd_3Node.Value = 0;
            numEnd_4Node.Value = 0;
            numEnd_5Node.Value = 0;
            numEnd_6Node.Value = 0;


        }
        #endregion
        #region  输入框加回车事件
        private void RegisterKeyEvent()
        {
            this.txtDescrib.KeyUp += new KeyEventHandler(txtDescrib_KeyUp);
            autoCompleteNonICD10Node.KeyUp += new KeyEventHandler(autoCompleteNonICD10Node_KeyUp);

            numStart_1Node.KeyUp += new KeyEventHandler(numStart_1Node_KeyUp);
            numEnd_1Node.KeyUp += new KeyEventHandler(numEnd_1Node_KeyUp);

            numStart_2Node.KeyUp += new KeyEventHandler(numStart_2Node_KeyUp);
            numEnd_2Node.KeyUp += new KeyEventHandler(numEnd_2Node_KeyUp);

            numStart_3Node.KeyUp += new KeyEventHandler(numStart_3Node_KeyUp);
            numEnd_3Node.KeyUp += new KeyEventHandler(numEnd_3Node_KeyUp);

            numStart_4Node.KeyUp += new KeyEventHandler(numStart_4Node_KeyUp);
            numEnd_4Node.KeyUp += new KeyEventHandler(numEnd_4Node_KeyUp);

            numStart_5Node.KeyUp += new KeyEventHandler(numStart_5Node_KeyUp);
            numEnd_5Node.KeyUp += new KeyEventHandler(numEnd_5Node_KeyUp);

            numStart_6Node.KeyUp += new KeyEventHandler(numStart_6Node_KeyUp);
            numEnd_6Node.KeyUp += new KeyEventHandler(numEnd_6Node_KeyUp);

            btnSave.KeyUp += new KeyEventHandler(btnSave_KeyUp);


        }

        private void txtDescrib_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                numStart_1Node.Focus();
        }

        private void autoCompleteNonICD10Node_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                autoCompleteNonICD10Node.Focus();
        }

        private void numStart_1Node_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                numStart_1Node.Focus();
        }

        private void numEnd_1Node_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                numEnd_1Node.Focus();
        }

        private void numStart_2Node_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                numStart_2Node.Focus();
        }

        private void numEnd_2Node_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                numEnd_2Node.Focus();
        }
        private void numStart_3Node_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                numStart_3Node.Focus();
        }

        private void numEnd_3Node_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                numEnd_3Node.Focus();
        }
        private void numStart_4Node_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                numStart_4Node.Focus();
        }

        private void numEnd_4Node_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                numEnd_4Node.Focus();
        }

        private void numStart_5Node_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                numStart_5Node.Focus();
        }

        private void numEnd_5Node_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                numEnd_5Node.Focus();
        }
        private void numStart_6Node_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                numStart_6Node.Focus();
        }

        private void numEnd_6Node_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                numEnd_6Node.Focus();
        }
        private void btnSave_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSave_Click(null, null);
        }




        #endregion
        /// <summary>
        /// 下拉选择宽 选择事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbConditionTypeNode_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                this.txtDanWei.Text = string.Empty;
                if (e.AddedItems.Count > 0)
                {
                    Visibility ICD = ((KeyValue)e.AddedItems[0]).Key == "2" ? Visibility.Visible : Visibility.Collapsed;
                    Visibility NonICD = ((KeyValue)e.AddedItems[0]).Key == "1" ? Visibility.Visible : Visibility.Collapsed;
                    Visibility Describ = ((KeyValue)e.AddedItems[0]).Key == "3" ? Visibility.Visible : Visibility.Collapsed;
                    //stkICD10.Visibility = ICD;
                    stkNonICD10Node.Visibility = NonICD;
                    wrpNonICDNode.Visibility = NonICD;
                    stkDescrib.Visibility = Describ;
                }
                if (cmbConditionTypeNode.SelectedIndex == 0)
                {
                    this.txtDescrib.Visibility = Visibility.Collapsed;
                    this.autoCompleteNonICD10Node.Visibility = Visibility.Visible;
                }
                if (cmbConditionTypeNode.SelectedIndex == 1)
                {
                    this.autoCompleteNonICD10Node.Visibility = Visibility.Collapsed;
                    this.txtDescrib.Visibility = Visibility.Visible;
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void Image_ImageFailed(object sender, ExceptionRoutedEventArgs e)
        {

        }






    }
}
