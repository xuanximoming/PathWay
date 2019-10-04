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
using YidanEHRApplication.Views.NursingNotes;
using YidanEHRApplication;

namespace YidanEHRApplication
{
    public partial class RWChangePassword
    {
        YidanEHRDataServiceClient serviceCon;
        CP_Employee m_emp = Global.LogInEmployee as CP_Employee;

        public RWChangePassword()
        {
            InitializeComponent();

        }

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {          try{

            string oldpwd = this.txtoldpwd.Password.Trim();
            string newpwd = this.txtnewpwd.Password.Trim();
            string newpwd2 = this.txtnewpwd2.Password.Trim();

            if (newpwd != newpwd2)
            {
                
                this.txtnewpwd.Password = "";
                this.txtnewpwd2.Password = "";
                PublicMethod.RadAlterBox("两次输入新密码不同，请重新输入！", "提示");
                return;
            }
            else
            {
                string encryptPasswordBase64 = HisEncryption.EncodeString(m_emp.Djsj, HisEncryption.PasswordLength, oldpwd);
                if (encryptPasswordBase64 == m_emp.Passwd)
                {
                    encryptPasswordBase64 = HisEncryption.EncodeString(m_emp.Djsj, HisEncryption.PasswordLength, newpwd);

                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.ChangePasswordCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    SQLMessage mess = ea.Result;
                                    if (mess.IsSucceed)
                                    {
                                        m_emp.Passwd = HisEncryption.EncodeString(m_emp.Djsj, HisEncryption.PasswordLength, this.txtnewpwd.Password.Trim());
                                        //btnTxtClear_Click(null, null);
                                        this.Close();
                                    }
                                    PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                                }
                                else
                                {
                                    PublicMethod.RadWaringBox(ea.Error);
                                }
                            };
                    serviceCon.ChangePasswordAsync(m_emp.Zgdm, encryptPasswordBase64);
                    serviceCon.CloseAsync();
                }
                else
                {
                    PublicMethod.RadAlterBox("原始密码错误！", "提示");
                    return;
                }
            }  }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }
        }

        private void btnTxtClear_Click(object sender, RoutedEventArgs e)
        {
            this.txtoldpwd.Password = "";
            this.txtnewpwd.Password = "";
            this.txtnewpwd2.Password = "";
        }


        #region  输入框加回车事件
        private void RegisterKeyEvent()
        {
            txtoldpwd.KeyUp += new KeyEventHandler(txtoldpwd_KeyUp);
            txtnewpwd.KeyUp += new KeyEventHandler(txtnewpwd_KeyUp);
            txtnewpwd2.KeyUp += new KeyEventHandler(txtnewpwd2_KeyUp);
            btnSave.KeyUp += new KeyEventHandler(btnSave_KeyUp);

        }

        private void txtoldpwd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtnewpwd.Focus();
        }

        private void txtnewpwd_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtnewpwd2.Focus();
        }

        private void txtnewpwd2_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSave.Focus();
        }

        private void btnSave_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnSave_Click(null, null);
        }

        #endregion
    }
}
