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
using System.Text;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls;
using System.Text.RegularExpressions;
using YidanEHRApplication.Models;
using YidanEHRApplication.DataService;
using YidanSoft.Tool;

namespace YidanEHRApplication.Controls
{
    public partial class UCQueryPathInfoControl : UserControl
    {
        private Boolean IsPostBack = false;
        public UCQueryPathInfoControl()
        {
            if (IsPostBack) return;
            else IsPostBack = true;
            InitializeComponent();

            Loaded += new RoutedEventHandler(UCQueryPathInfoControl_Loaded);

        }

        //add by luff 20130306
        void UCQueryPathInfoControl_Loaded(object sender, RoutedEventArgs e)
        {

            mtxtName.Focus();
            RegisterKeyEvent();
            this.radDateStart.DateTimeWatermarkContent = "选择日期...";
            this.radDateEnd.DateTimeWatermarkContent = "选择日期...";

            //add by luff 20130228 
            //绑定科室和病人状态
            IntiComboBoxDept();
            IntiComboBoxBrzt();


        }

        /// <summary>
        /// 判断该用户是否有权限使用科室查询功能
        /// </summary>
        private void CheckUserDept()
        { 
            #region  判断该用户是否有权限使用科室查询功能 
            if (Global.UserRole == null && Global.mAppCfg == null)
            {
                //如果存在多个科室 则可以选择科室
                if (Global.User2Dept.Count > 1)
                {
                    GetDeptList();
                }
                else
                {
                    this.autoCompleteBoxDept.IsEnabled = false;
                }

            }
            else if (Global.UserRole != null && Global.mAppCfg != null)
            {
                if (CheckIsKscx())
                {
                    this.autoCompleteBoxDept.IsEnabled = true;
                    Global.IsXsyz = true;
                }
                else
                {
                    //如果存在多个科室 则可以选择科室
                    if (Global.User2Dept.Count > 1)
                    {
                        GetDeptList();
                    }
                    else
                    {
                        this.autoCompleteBoxDept.IsEnabled = false;
                        Global.IsXsyz = false;
                    }
                }

            }
            else
            {
                //如果存在多个科室 则可以选择科室
                if (Global.User2Dept.Count > 1)
                {
                    GetDeptList();
                }
                else
                {
                    this.autoCompleteBoxDept.IsEnabled = false;
                }
            }
            #endregion
        }

        /// <summary>
        /// 根据当前登录人获取当前登录人可以使用的科室 如果存在多个科室 则可以选择科室
        /// </summary>
        private void GetDeptList()
        {
            ObservableCollection<CP_DepartmentList> deptlist = new ObservableCollection<CP_DepartmentList>();

            foreach (User2Dept userdept in Global.User2Dept)
            {
                foreach (CP_DepartmentList dept in (ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource)
                {
                    if (dept.Ksdm == userdept.DeptId)
                    {
                        deptlist.Add(dept);
                        break;
                    }
                }
            }

            //如果存在多个科室 则可以选择科室
            if (deptlist.Count > 1)
            {
                autoCompleteBoxDept.ItemsSource = deptlist;
                this.autoCompleteBoxDept.IsEnabled = true;
                Global.IsXsyz = true;
            }
             
        }

        public delegate void QueryInfoClicked(object sender, RoutedEventArgs e);
        public event QueryInfoClicked AfterQuryInfoClicked;
        protected virtual void OnAfterQuryInfoClicked(QcEventArgs e)
        {
            if (AfterQuryInfoClicked != null)
            {
                AfterQuryInfoClicked(this, e);
            }
        }

        //add by luff 20130228
        #region  得到当前用户编号去判断是否有权限使用科室查询
        /// <summary>
        /// 得到当前用户编号去判断是否有权限使用科室查询
        /// </summary>
        /// <returns></returns>
        public bool CheckIsKscx()
        {
            try
            {
                string para = Global.LogInEmployee.Zgdm.ToString();
                //获得对应用户的权限信息
                List<PE_UserRole> t_listsouce = Global.UserRole.Select(s => s).Where(s => s.UserID == para).ToList();
                //获得满足条件的配置信息
                List<APPCFG> t_listApp = Global.mAppCfg.Select(s => s).Where(s => s.Configkey.IndexOf("InpatientListShowAllDept") > -1).ToList();
                //判断配置表是否有InpatientListShowAllDept对应的参数值
                if (t_listApp.Count == 0)
                {
                    return false;
                }

               
                foreach (var ur in t_listsouce)
                {
                    //判断t_listApp是否有数据以及参数值是否为逗号分隔
                    if (t_listApp[0].Value.IndexOf(",") > -1)
                    {
                        string[] arr_App = t_listApp[0].Value.Split(',');
                        foreach (string app in arr_App)
                        {
                            if (ur.RoleCode.Contains(app))
                            {
                                return true;
                            }
                            //else
                            //{
                            //    return false;
                            //}
                        }
                    }
                    else
                    {
                        if (t_listApp[0].Value == ur.RoleCode)
                        {
                            return true;
                        }
                        //else
                        //{
                        //    return false;
                        //}
                    }

                }
                return false;

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                return false;
            }

            return false;
        }
        #endregion

        private bool CheckCondition()
        {
            //add by luff 20120928
            int i = 0;
            if (radDateEnd.SelectedDate.HasValue)
            {
                i += 1;
                return true;
            }
            if (radDateStart.SelectedDate.HasValue)
            {
                i += 1;
                return true;
            }
            if (((i == 2) && (radDateStart.SelectedDate.Value < radDateEnd.SelectedDate.Value)) || (i == 0))
            {
                return true;
            }
            return false;
        }
        public int type = 0;
        public String Hzxm { get { return _mtxtName; } }
        public String Zyhm { get { return _mtxtPID; } }
        public String BedNo { get { return _mtxtBedNo; } }
        public String Ksdm { get { return _mtxtKsdm; } }
        public String Brzt { get { return _mtxtBrzt; } }
        public String StartDate { get { return radDateStart.SelectedDate == null ? "" : radDateStart.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"); } }
        public String EndDate { get { return radDateEnd.SelectedDate == null ? "" : radDateEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss"); } }

        private QcEventArgs DoQueryCondition()
        {
            QcEventArgs args = new QcEventArgs();
            #region 单选框（注释）
            //if (!string.IsNullOrEmpty(_mtxtBedNo))
            //{
            //    if (radCombxConditon1.IsChecked==true)
            //    {
            //        type = 0;
            //        args.Hzxm = _mtxtBedNo;
            //    }
            //    else if (radCombxConditon2.IsChecked == true)
            //    {
            //        type = 1;
            //        args.Zyhm = _mtxtBedNo;
            //    }
            //    else
            //        type = 2;
            //        args.BedNo = _mtxtBedNo;
            //}

            #endregion
            //if (!string.IsNullOrEmpty(_mtxtName))
            //{
            if (!string.IsNullOrEmpty(_mtxtName))
            {
                //type = 0;
                args.Hzxm = _mtxtName.Replace(" ", "");
            }
            if (!string.IsNullOrEmpty(_mtxtPID))
            {
                //type = 1;
                args.Zyhm = _mtxtPID.Replace(" ", "");
            }
            if (!string.IsNullOrEmpty(_mtxtBedNo))
            {
                //type = 2;
                args.BedNo = _mtxtBedNo.Replace(" ", "");
            }
            //} 
            //else
            //{
            if (autoCompleteBoxDept.SelectedItem == null)
            {
                args.Ksdm = Global.LogInEmployee.Ksdm;
            }
            else
            {
                args.Ksdm = ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm;
                //add by yxy 20130403 如果当前登录人有多个科室权限，在根据科室查询时将科室代码赋值给全局变量
                //Global.LogInEmployee.Ksdm = ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm; 
            }

            if (radComboBoxStatus.SelectedItem == null)
            {
                args.Brzt = "";
            }
            else
            {
                //if (radComboBoxStatus.SelectedIndex == 0)
                //{
                //    args.Brzt = "1501";
                //}
                //else
                //{
                //    args.Brzt = "1503";
                //}

                args.Brzt = ((CP_DataCategoryDetail)(radComboBoxStatus.SelectedItem)).Mxbh.ToString();
            }
            if (radDateStart.SelectedDate == null)
            {
                args.StartDate = "";
            }
            else
            {
                args.StartDate = radDateStart.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            if (radDateEnd.SelectedDate == null)
            {
                args.EndDate = "";
            }
            else
            {
                args.EndDate = radDateEnd.SelectedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
            }
            //}
            return args;
        }
        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CheckCondition())
                {
                    String[] strs = new String[] { "'", "‘", ",", "，", ";", "；", "“", ":", "：" };
                    if (_mtxtName.Length > 0)
                    {
                        foreach (String item in strs)
                        {
                            if (_mtxtName.IndexOf(item) >= 0)
                            {
                                PublicMethod.RadAlterBox("请输入合法数据!", "提示");
                                return;
                            }
                        }
                    }
                    if (_mtxtPID.Length > 0)
                    {
                        foreach (String item in strs)
                        {
                            if (_mtxtName.IndexOf(item) >= 0)
                            {
                                PublicMethod.RadAlterBox("请输入合法数据!", "提示");
                                return;
                            }
                        }
                    }
                    if (_mtxtBedNo.Length > 0)
                    {
                        foreach (String item in strs)
                        {
                            if (_mtxtBedNo.IndexOf(item) >= 0)
                            {
                                PublicMethod.RadAlterBox("请输入合法数据!", "提示");
                                return;
                            }
                        }
                    }
                    //add by  luff  查询病人状态的时候 判断查询日期
                    if (_mtxtBrzt.Length > 0)
                    {
                        if (_mtxtBrzt == "1501")//病人在院，日期时间不控制
                        {
                            this.radDateStart.SelectedDate = null;
                            //this.radDateStart.DateTimeWatermarkContent = "选择日期...";

                            this.radDateEnd.SelectedDate = null;
                            //this.radDateEnd.DateTimeWatermarkContent = "选择日期...";


                        }
                        else if (_mtxtBrzt == "1503")
                        {
                            if (radDateStart.SelectedDate == null && radDateEnd.SelectedDate == null)
                            {
                                PublicMethod.RadAlterBox("请输入查询日期，建议查询日期为一个月以内的日期时间!否则查询数据会很慢！", "提示");
                                this.radDateEnd.SelectedValue = DateTime.Parse(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                this.radDateStart.SelectedValue = DateTime.Parse(DateTime.Now.AddDays(-30).ToString("yyyy-MM-dd HH:mm:ss"));
                                //radDateStart.Focus();
                                return;
                            }
                        }


                    }

                    OnAfterQuryInfoClicked(DoQueryCondition());
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        //add by luff 20130227
        #region 科室
        /// <summary>
        /// 科室
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
                             
                            if (Global.LogInEmployee.Ksdm != "")
                            {
                                autoCompleteBoxDept.SelectedItem = ((ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource).FirstOrDefault(cp => cp.Ksdm.Equals(Global.LogInEmployee.Ksdm));
                            }

                            CheckUserDept();
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

        //add by luff 20130307
        #region 病人状态
        /// <summary>
        /// 病人状态
        /// </summary>
        private void IntiComboBoxBrzt()
        {
            try
            {
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                referenceClient.GetDataCategoryInfoCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            radComboBoxStatus.ItemsSource = e.Result.ToList();
                            radComboBoxStatus.SelectedIndex = 0;

                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };

                int iVal = 15;
                referenceClient.GetDataCategoryInfoAsync(iVal);
                referenceClient.CloseAsync();

            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #endregion

        private void RegisterKeyEvent()
        {
            mtxtName.KeyUp += new KeyEventHandler(mtxtName_KeyUp);
            mtxtPID.KeyUp += new KeyEventHandler(mtxtPID_KeyUp);
            mtxtBedNo.KeyUp += new KeyEventHandler(mtxtBedNo_KeyUp);
            autoCompleteBoxDept.KeyUp += new KeyEventHandler(autoCompleteBoxDept_KeyUp);
            radComboBoxStatus.KeyUp += new KeyEventHandler(radComboBoxStatus_KeyUp);
        }

        private void mtxtName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnQuery_Click(sender, e);
            }
        }
        private void mtxtPID_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnQuery_Click(sender, e);
            }
        }

        private void mtxtBedNo_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnQuery_Click(sender, e);
            }
        }
        private void autoCompleteBoxDept_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnQuery_Click(sender, e);
            }
        }
        private void radComboBoxStatus_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                btnQuery_Click(sender, e);
            }
        }

        /// <summary>
        /// 重置
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRest_Click(object sender, RoutedEventArgs e)
        {
            mtxtName.Value = string.Empty;
            mtxtPID.Value = string.Empty;
            mtxtBedNo.Value = string.Empty;
            radDateEnd.SelectedDate = null;
            radDateStart.SelectedDate = null;
            //重置科室为默认科室
            //autoCompleteBoxDept.SelectedItem = null;
            autoCompleteBoxDept.Text = "";
            autoCompleteBoxWard.SelectedItem = null;
            autoCompleteBoxWard.Text = "";

        }
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            //radDateStart.SelectedDate = DateTime.Now.AddDays(-3);
            //radDateEnd.SelectedDate = DateTime.Now;


        }

        public String IsIn
        {
            get { if ((Boolean)chkIn.IsChecked)return "1"; else return "0"; }
        }
        #region add by dxj 2011/7/26
        //private void radCombxConditon_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        //{
        //    if (radCombxConditon1 == null)
        //    {
        //        return;
        //    }
        //    mtxtBedNo.Value = null;
        //    //switch (radCombxConditon.SelectedIndex)
        //    //{
        //    //    case 0:
        //    //        mtxtBedNo.MaskType = MaskType.None;
        //    //        break;
        //    //    case 1:
        //    //        mtxtBedNo.MaskType = MaskType.Numeric;
        //    //        break;
        //    //    case 2:
        //    //        mtxtBedNo.MaskType = MaskType.Numeric;
        //    //        break;
        //    //    default:
        //    //        mtxtBedNo.MaskType = MaskType.None;
        //    //        break;
        //    //}
        //}

        private void mtxtBedNo_ValueChanging(object sender, RadMaskedTextBoxValueChangingEventArgs e)
        {
            //if (_mtxtBedNo == String.Empty)
            //{
            //    return;
            //}
            //if (mtxtBedNo.MaskType == MaskType.None)
            //{
            //    String newValue = e.NewMaskedText;
            //    Regex pattern = new Regex(@"^[\u4e00-\u9fa5\a-zA-Z]{0,}$");
            //    e.Handled = !pattern.IsMatch(newValue);
            //}
        }

        /// <summary>
        /// 查询条件1（姓名）
        /// </summary>
        public String _mtxtName
        {
            get
            {
                if (mtxtName.Value == null || mtxtName.Value.ToString().Trim() == String.Empty)
                {
                    return String.Empty;
                }
                return ConvertMy.ToString(mtxtName.Value);
            }
        }

        /// <summary>
        /// 查询条件2（病历号）
        /// </summary>
        public String _mtxtPID
        {
            get
            {
                if (mtxtPID.Value == null || mtxtPID.Value.ToString().Trim() == String.Empty)
                {
                    return String.Empty;
                }
                return ConvertMy.ToString(mtxtPID.Value);
            }
        }

        /// <summary>
        /// 查询条件3
        /// </summary>
        public String _mtxtBedNo
        {
            get
            {
                if (mtxtBedNo.Value == null || mtxtBedNo.Value.ToString().Trim() == String.Empty)
                {
                    return String.Empty;
                }
                return ConvertMy.ToString(mtxtBedNo.Value);
            }
        }

        /// <summary>
        /// 查询科室
        /// </summary>
        public String _mtxtKsdm
        {
            get
            {
                return ConvertMy.ToString(autoCompleteBoxDept.SelectedItem == null ? Global.LogInEmployee.Ksdm : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm);
            }
        }

        /// <summary>
        /// 查询病人（住院）状态
        /// </summary>
        public String _mtxtBrzt
        {
            get
            {
                //if (this.radComboBoxStatus.SelectedItem != null)
                //{
                //    if (radComboBoxStatus.SelectedIndex == 0)
                //    {
                //        return "1501";
                //    }
                //    else
                //    {
                //        return "1503";
                //    }
                //}
                //return "1501";
                return ConvertMy.ToString(radComboBoxStatus.SelectedItem == null ? "" : ((CP_DataCategoryDetail)(radComboBoxStatus.SelectedItem)).Mxbh.ToString());

            }
        }
        #endregion

        private void radComboBoxStatus_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (radComboBoxStatus.SelectedItem == null)
            {
                return;
            }
            CP_DataCategoryDetail cp_datacategory = (CP_DataCategoryDetail)(radComboBoxStatus.SelectedItem);
            if (cp_datacategory.Mxbh.ToString() == "1501")
            {
                radDateEnd.SelectedDate = null;
                radDateStart.SelectedDate = null;
            }
        }
    }
}
