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
using System.Windows.Navigation;
using System.Xml.Linq;
using System.Windows.Browser;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;
using YidanSoft.Tool;
using YidanEHRApplication.DataService;
using YidanEHRApplication;

namespace YidanEHRApplication.Views.Login
{

    public partial class LogInPage : Page
    {

        private bool Is_root = true;

        public LogInPage()
        {
            InitializeComponent();
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {

        }

        #region events
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                this.textBlockWarning.Text = string.Empty;

                //Add version information
                GetVersion();
                //
                #region 动态创建界面控件的KeyUp事件 add by dxj 2011/7/22
                MadeKeyUp keyUp = new MadeKeyUp();
                keyUp.Controls.Add(textBoxName);
                keyUp.Controls.Add(passwordBoxPwd);
                keyUp.Controls.Add(radButtonConfirm);
                keyUp.Made_KeyUp();
                #endregion

                HtmlPage.Plugin.Focus();
                this.textBoxName.SelectAll();
                this.textBoxName.Focus();

                //使用系统授权
                //CheckEHRRoot();
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex,this.GetType().FullName);
            }
        }


        /// <summary>
        /// 确定
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButtonConfirm_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //判断是否有权限使用系统 如果没有系统则不能登录 
                Login();
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex,this.GetType().FullName);
            }
        }

        /// <summary>
        /// 判断当前是否有权限使用系统
        /// </summary>
        /// <returns></returns>
        private void CheckEHRRoot()
        {
            #region 获得员工基本信息、部门、病区、科室、用户权限等信息

            try
            {
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                referenceClient.CheckRootCompleted +=
                  (obj, e) =>
                  {

                      if (e.Error != null)
                      {
                          //PublicMethod.RadWaringBox(e.Error);
                          //this.textBlockWarning.Text = "用户无使用权限，请联系管理员！";
                          //YiDanMessageBox.Show("用户无使用权限，请联系管理员！");
                          YiDanMessageBox.Show(e.Error, this.GetType().FullName);
                          Is_root = false;
                          return;
                      }
                      string strOutMessage = string.Empty;

                      if (e.Result == null)
                      {
                          this.textBlockWarning.Text = "用户无使用权限，请联系管理员！";
                          YiDanMessageBox.Show("用户无使用权限，请联系管理员！");
                          this.radBusyIndicator.IsBusy = false;
                          Is_root = false;
                      }
                      else
                      {
                          if (e.Result == false)
                          {
                              this.textBlockWarning.Text = "用户无使用权限，请联系管理员！";
                              YiDanMessageBox.Show("用户无使用权限，请联系管理员！");
                              this.radBusyIndicator.IsBusy = false;
                              //YiDanMessageBox.Show("用户不存在！", YiDanMessageBoxButtons.Ok);

                              Is_root = false;
                          }
                          else
                          {
                              Is_root = true;
                          }
                      }
                  };

                referenceClient.CheckRootAsync();
                referenceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                Is_root = false;
                YiDanMessageBox.Show(ex,this.GetType().FullName);
            }


            #endregion
        }

        public void Login()
        {
            try
            {
                if (!Is_root)
                {
                    YiDanMessageBox.Show("用户无使用权限，请联系管理员！");
                    return;
                } 
                this.textBlockWarning.Text = string.Empty;
                if (this.textBoxName.Text.Trim() == string.Empty)
                { 
                    YiDanMessageBox.Show("请输入用户名", this.textBoxName);
                    return;
                }

                string id = this.textBoxName.Text.Trim();

                this.radBusyIndicator.IsBusy = true;
                #region 获得员工基本信息、部门、病区、科室、用户权限等信息
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                referenceClient.GetEmployeeInfoCompleted +=
                  (obj, e) =>
                  {

                      if (e.Error != null)
                      {
                          //PublicMethod.RadWaringBox(e.Error);
                          YiDanMessageBox.Show(e.Error, this.GetType().FullName);
                          return;
                      }
                      string strOutMessage = string.Empty;

                      if (e.Result == null || e.Result.Zgdm.Trim() == string.Empty)
                      {
                          this.textBlockWarning.Text = "用户不存在";

                          YiDanMessageBox.Show("用户不存在！", this.textBoxName);
                          this.radBusyIndicator.IsBusy = false; 

                      }
                      else
                      {
                          if (IsUserOrNot(this.passwordBoxPwd.Password.Trim(), e.Result.Passwd, e.Result.Djsj, out strOutMessage))
                          {
                              Global.LogInEmployee = (CP_Employee)e.Result;
                              //add by luff 20130228 获得全局参数配置信息和用户角色信息
                              GetAppCfgInfo();
                              GetUserRole();
                              GetUser2Dept();
                              ShowMainPage();
                          }
                          else
                          {
                              this.textBlockWarning.Text = strOutMessage;
                              YiDanMessageBox.Show(strOutMessage, this.textBoxName);
                              //this.textBoxName.Focus();
                              this.radBusyIndicator.IsBusy = false;
                          }
                      }
                  };

                referenceClient.GetEmployeeInfoAsync(id);
                referenceClient.CloseAsync();
                #endregion

                this.textBoxName.Focus();
            }
            catch (Exception e)
            {

                throw e;
            }


        }
        /// <summary>
        /// 获取系统版本信息
        /// </summary>
        public void GetVersion()
        {
            try
            {
                string ver = "";
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                referenceClient.GetVersionCompleted +=
                  (obj, e) =>
                  {

                      if (e.Error != null)
                      {
                          //PublicMethod.RadWaringBox(e.Error);
                          YiDanMessageBox.Show(e.Error,this.GetType().FullName);
                          return;
                      }
                      string strOutMessage = string.Empty;

                      if (e.Result == null || e.Result.VersionID == string.Empty)
                      {
                          this.textBlockWarning.Text = "版本号不存在";
                          YiDanMessageBox.Show("版本号不存在", this.textBoxName); 
                      }
                      else
                      {
                          textBoxVersion.Text = textBoxVersion.Text + e.Result.VersionID;
                          tip.Text = e.Result.Version;
                      }
                  };
                referenceClient.GetVersionAsync();
                referenceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public void ShowMainPage()
        {
            try
            {
                YidanEHRDataServiceClient serviceCon;

                serviceCon = PublicMethod.YidanClient;
                serviceCon.GetV_PermissionListFatherCompleted +=
                  (obj, e) =>
                  {
                      List<V_PermissionListFather> V_PermissionListFather;
                      V_PermissionListFather = e.Result.ToList();
                      //添加判断，如果该用户没有功能菜单则不给登录系统 yxy
                      if (V_PermissionListFather.Count > 0)
                      {
                          this.Content = new MainPage(V_PermissionListFather);
                          this.radBusyIndicator.IsBusy = false;
                      }
                      else
                      {
                          this.textBlockWarning.Text = "该用户无角色权限！";
                          YiDanMessageBox.Show("该用户无角色权限！", this.textBoxName); 
                          this.radBusyIndicator.IsBusy = false;
                          this.textBoxName.Focus();
                      }
                  };
                serviceCon.GetV_PermissionListFatherAsync(Global.LogInEmployee.Zgdm);
                serviceCon.CloseAsync();
            }
            catch (Exception e)
            {

                throw e;
            }

        }
        /// <summary>
        /// add by luff 20130228 全局参数和用户角色信息
        /// </summary>
        public void GetAppCfgInfo()
        {
            try
            {
                //add by luff 
                #region 获得AppCfg全局参数配置表基本信息
                YidanEHRDataServiceClient AppcfgClient = PublicMethod.YidanClient;
                AppcfgClient.GetAppCfgCompleted +=
                  (obj, ea) =>
                  {
                      Global.mAppCfg = new List<APPCFG>();
                      if (ea.Error != null)
                      {
                          //PublicMethod.RadWaringBox(ea.Error);
                          YiDanMessageBox.Show(ea.Error, this.GetType().FullName);
                          return;
                      }

                      if (ea.Result != null && Global.mAppCfg == null)
                      {
                          Global.mAppCfg = (List<APPCFG>)(ea.Result.ToList());
                      }

                      if (ea.Result != null && Global.mAppCfg.Count == 0)
                      {
                          Global.mAppCfg = (List<APPCFG>)(ea.Result.ToList());
                      }

                  };

                AppcfgClient.GetAppCfgAsync("-1");
                AppcfgClient.CloseAsync();
                #endregion
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// 根据当前登录人获取当前登录人多科室信息
        /// </summary>
        public void GetUser2Dept()
        {
            //add by yxy 
            try
            {
                #region 当前用户如果对于多个科室，查询多个科室信息
                YidanEHRDataServiceClient serviceCon = PublicMethod.YidanClient;
                serviceCon.GetUserDeptByUserIDCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            Global.User2Dept = e.Result.ToList().Select(s => s).Where(s => s.IsCheck == 1).ToList();

                            //如果当前登录人科室不在列表中则添加一条信息到Global中
                            if (Global.User2Dept.Select(s => s).Where(s => s.DeptId == Global.LogInEmployee.Ksdm).ToList().Count == 0)
                            {
                                User2Dept userdept = new User2Dept();
                                userdept.DeptId = Global.LogInEmployee.Ksdm;
                                userdept.UserId = Global.LogInEmployee.Zgdm;
                                Global.User2Dept.Add(userdept);
                            }
                        }
                        else
                        {
                            //PublicMethod.RadWaringBox(e.Error);
                            YiDanMessageBox.Show(e.Error, this.GetType().FullName);
                        }
                    };
                serviceCon.GetUserDeptByUserIDAsync(Global.LogInEmployee.Zgdm);
                serviceCon.CloseAsync();
                #endregion
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        //add by luff 20130228获得用户角色信息
        public void GetUserRole()
        {
            try
            {
                #region 获得PE_UserRole用户角色信息
                YidanEHRDataServiceClient UserRoleClient = PublicMethod.YidanClient;
                UserRoleClient.GetPE_UserRoleInfoCompleted +=
                  (obj, ea) =>
                  {

                      Global.UserRole = new List<PE_UserRole>();
                      if (ea.Error != null)
                      {
                          //PublicMethod.RadWaringBox(ea.Error);
                          YiDanMessageBox.Show(ea.Error, this.GetType().FullName);
                          return;
                      }

                      if (ea.Result != null && Global.UserRole == null)
                      {
                          Global.UserRole = (List<PE_UserRole>)(ea.Result.ToList());
                      }

                      if (ea.Result != null && Global.UserRole.Count == 0)
                      {
                          Global.UserRole = (List<PE_UserRole>)(ea.Result.ToList());
                      }

                  };

                UserRoleClient.GetPE_UserRoleInfoAsync();
                UserRoleClient.CloseAsync();
                #endregion
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radButtonCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clear();
            }
            catch (Exception ex)
            {

                YiDanMessageBox.Show(ex,this.GetType().FullName);
            }
        }

        #endregion

        #region  methods
        /// <summary>
        /// 验证登录信息
        /// </summary>
        /// <param name="strInputPwd">输入的PWD</param>
        /// <param name="strDbPwd">DB的PWD</param>
        /// <param name="strRegDate">DB记录的登记日期</param>
        /// <param name="strErrorMessage"></param>
        /// <returns></returns>
        private bool IsUserOrNot(string strInputPwd, string strDbPwd, string strRegDate, out string strErrorMessage)
        {
            try
            {
                strErrorMessage = "";
                string encryptPasswordBase64 = HisEncryption.EncodeString(strRegDate, HisEncryption.PasswordLength, strInputPwd);
                if (encryptPasswordBase64 == strDbPwd)
                {
                    return true;
                }
                else
                {
                    strErrorMessage = "密码错误！";
                    return false;
                }
            }
            catch (Exception e)
            {

                throw e;
            }
        }

        /// <summary>
        /// clear
        /// </summary>
        private void Clear()
        {
            try
            {
                this.textBoxName.Text = string.Empty;
                this.passwordBoxPwd.Password = string.Empty;
                this.textBlockWarning.Text = string.Empty;
                this.textBoxName.Focus();
            }
            catch (Exception e)
            {

                throw e;
            }
        }
        #endregion

        private void LayoutRoot_MouseRightButtonDown(object sender, MouseButtonEventArgs e)
        {
            try
            {
                e.Handled = true;
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex,this.GetType().FullName);
            }
        }

        private void passwordBoxPwd_KeyDown(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                    Login();
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex,this.GetType().FullName);
            }
        }
    }
}
