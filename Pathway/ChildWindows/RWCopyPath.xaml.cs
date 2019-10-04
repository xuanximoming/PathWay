using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Input;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Views.ChildWindows
{
    public partial class RWCopyPath
    {
        //定义变量
        ManualType editState;
        String SLjdm;
        String SNewLjdm;
        CP_ClinicalPathList CP_ClinicalPathList = null;

        //需要新增 病种
        private ObservableCollection<CP_ClinicalDiagnosisList> m_ClinicalDiagnosisListAdd = new ObservableCollection<CP_ClinicalDiagnosisList>();


        //public RWCopyPath()
        //{
        //    InitializeComponent();
        //}

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            //绑定路径状态
            //2013-05-03,WangGuojin, false 状态改为 true
            IntiComboBoxStatus(true);

            if (CP_ClinicalPathList != null)
            {

                //要复制的路径基础数据初始化
                textBoxPathName.Text = CP_ClinicalPathList.Name + "V" + (Convert.ToDouble(CP_ClinicalPathList.Vesion) + 1).ToString();
                radNumericUpDownVersion.Value = Convert.ToDouble(CP_ClinicalPathList.Vesion) + 1;
                radNumericUpDownInDays.Value = Convert.ToDouble(CP_ClinicalPathList.Zgts);
                radNumericUpDownAvgFee.Value = Convert.ToDouble(CP_ClinicalPathList.Jcfy);

                radComboBoxStatus.SelectedValue = CP_ClinicalPathList.YxjlId;

                autoCompleteBoxDept.SelectedItem = ((ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource).FirstOrDefault(s => s.Ksdm.Equals(CP_ClinicalPathList.Syks));

            }
        }

        public RWCopyPath(ManualType type, String sLjdm, CP_ClinicalPathList cp, ObservableCollection<CP_DepartmentList> _deptList)
        {
            //初始化控件
            InitializeComponent();
            //IntiComboBoxDept();


            if (cp != null)
            {
                CP_ClinicalPathList = cp;
                SLjdm = sLjdm;
                editState = type;
            }
            if (_deptList != null)
            {
                autoCompleteBoxDept.ItemsSource = _deptList;
                autoCompleteBoxDept.ItemFilter = DeptFilter;
            }

        }

        #region 绑定科室   autoCompleteBoxDept
        /// <summary>
        /// 初始化绑定科室
        /// </summary>
        private void IntiComboBoxDept()
        {
            try
            {
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                referenceClient.GetDepartmentListInfoCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            autoCompleteBoxDept.ItemsSource = e.Result;
                            autoCompleteBoxDept.ItemFilter = DeptFilter;
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
                referenceClient.GetDepartmentListInfoAsync();
                referenceClient.CloseAsync();

            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        public bool DeptFilter(string strFilter, object item)
        {
            CP_DepartmentList deptList = (CP_DepartmentList)item;
            return ((deptList.QueryName.StartsWith(strFilter.ToUpper())) || (deptList.QueryName.Contains(strFilter.ToUpper())));
        }
        #endregion

        #region 路径状态 radComboBoxStatus
        /// <summary>
        ///   INIT路径状态
        /// </summary>
        /// <param name="isReivew">是否要加审核</param>
        private void IntiComboBoxStatus(bool isReivew)
        {
            try
            {
                radComboBoxStatus.ItemsSource = null;
                List<Status> statusList = new List<Status>();
                statusList.Add(new Status("无效", (int)PathShStatus.Cancel));
                statusList.Add(new Status("有效", (int)PathShStatus.Valid));
                statusList.Add(new Status("停止", (int)PathShStatus.Dc));
                //if (isReivew)
                //statusList.Add(new Status("审核", (int)PathShStatus.Review));
                radComboBoxStatus.ItemsSource = statusList;
                radComboBoxStatus.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion

        //保存复制路径
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                #region 保存一条新的路径
                radBusyIndicator.IsBusy = true;
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                if (editState == ManualType.New)
                {
                    if (Check())
                    {


                        referenceClient.GetandCopyPathDataCompleted +=
                                (obj, ea) =>
                                {
                                    try
                                    {

                                        if (ea.Error == null)
                                        {

                                            if (ea.Result == 1)
                                            {
                                                radBusyIndicator.IsBusy = false;
                                                PublicMethod.RadAlterBox("复制路径成功！", "提示");
                                            }
                                            else
                                            {
                                                radBusyIndicator.IsBusy = false;
                                                PublicMethod.RadAlterBox("复制路径失败！", "提示");
                                            }

                                        }
                                        else
                                        {
                                            PublicMethod.RadWaringBox(ea.Error);
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                                    }
                                };
                        CP_ClinicalDiagnosisList cpList = new CP_ClinicalDiagnosisList();
                        cpList.Bzdm = Guid.NewGuid().ToString();
                        cpList.Bzmc = "";
                        cpList.Ljdm = "";
                        m_ClinicalDiagnosisListAdd.Add(cpList);
                        referenceClient.GetandCopyPathDataAsync(this.textBoxPathName.Text, this.textBoxPathName.Text, (double)radNumericUpDownInDays.Value, (double)radNumericUpDownAvgFee.Value,
                             (double)radNumericUpDownVersion.Value, string.Empty, (int)radComboBoxStatus.SelectedValue, ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm, m_ClinicalDiagnosisListAdd, SLjdm);
                        referenceClient.CloseAsync();



                    }

                }
                #endregion

            }
            catch (Exception ex)
            {

                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
            finally
            {
                m_ClinicalDiagnosisListAdd.Clear();
            }
        }

        /// <summary>
        /// 页面重置按钮事件
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

        }
        //重置
        private void Reset()
        {
            try
            {
                textBoxPathName.Text = string.Empty;
                radNumericUpDownVersion.Value = 1;
                radNumericUpDownInDays.Value = 0;
                radNumericUpDownAvgFee.Value = 0;
                radComboBoxStatus.SelectedValue = 1;
                autoCompleteBoxDept.SelectedItem = null;
                this.autoCompleteBoxDept.Text = "";
                //this.radGridViewPathList.SelectedItem = null;

            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        #region  输入框加回车事件
        private void RegisterKeyEvent()
        {
            textBoxPathName.KeyUp += new KeyEventHandler(textBoxPathName_KeyUp);
            radNumericUpDownVersion.KeyUp += new KeyEventHandler(radNumericUpDownVersion_KeyUp);
            radNumericUpDownInDays.KeyUp += new KeyEventHandler(radNumericUpDownInDays_KeyUp);

            radComboBoxStatus.KeyUp += new KeyEventHandler(radComboBoxStatus_KeyUp);

            autoCompleteBoxDept.KeyUp += new KeyEventHandler(autoCompleteBoxDept_KeyUp);
            radNumericUpDownAvgFee.KeyUp += new KeyEventHandler(radNumericUpDownAvgFee_KeyUp);
            btnSave.KeyUp += new KeyEventHandler(btnSave_KeyUp);

            //autoCompleteBoxQueryDept.KeyUp += new KeyEventHandler(tbQuery2_KeyUp);
            //autoCompleteBoxQueryPath.KeyUp += new KeyEventHandler(tbQuery2_KeyUp);
            //radCmbYxjl.KeyUp += new KeyEventHandler(tbQuery2_KeyUp);
        }

        private void textBoxPathName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                radNumericUpDownVersion.Focus();
        }

        private void radNumericUpDownVersion_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                radNumericUpDownInDays.Focus();
        }

        private void radNumericUpDownInDays_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                radComboBoxStatus.Focus();
        }

        private void radComboBoxStatus_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                autoCompleteBoxDept.Focus();
        }

        private void autoCompleteBoxDept_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                radNumericUpDownAvgFee.Focus();
        }

        private void radNumericUpDownAvgFee_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSave.Focus();
        }

        private void btnSave_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSave_Click(null, null);
        }


        //private void tbQuery2_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //        radbuttonQuery_Click(null, null);
        //}

        #endregion

        #region 检查数据
        private string m_Title = "复制路径";
        private bool Check()
        {
            if (this.textBoxPathName.Text.Trim() == string.Empty)
            {
                PublicMethod.RadAlterBoxRe("请输入路径名称", m_Title, textBoxPathName);

                return false;
            }
            if (this.textBoxPathName.Text.Trim().Length >= 60)
            {
                PublicMethod.RadAlterBoxRe("路径名称长度不能超过30位", m_Title, textBoxPathName);
                return false;
            }
            if (this.radComboBoxStatus.SelectedValue == null)
            {
                PublicMethod.RadAlterBoxRe("请选择使用状态", m_Title, radComboBoxStatus);
                return false;
            }
            if (this.autoCompleteBoxDept.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择科室", m_Title, autoCompleteBoxDept);
                return false;
            }
            if (this.radNumericUpDownVersion.ContentText.Trim() == string.Empty)
            {
                PublicMethod.RadAlterBoxRe("请选择科室", m_Title, radNumericUpDownVersion);
                return false;
            }
            if (this.radNumericUpDownInDays.ContentText.Trim() == string.Empty)
            {
                PublicMethod.RadAlterBoxRe("住院天数不能为空", m_Title, radNumericUpDownInDays);
                return false;
            }
            if (this.radNumericUpDownAvgFee.ContentText.Trim() == string.Empty)
            {
                PublicMethod.RadAlterBoxRe("费用不能为空", m_Title, radNumericUpDownAvgFee);
                return false;
            }


            return true;
        }
        #endregion
    }
}

