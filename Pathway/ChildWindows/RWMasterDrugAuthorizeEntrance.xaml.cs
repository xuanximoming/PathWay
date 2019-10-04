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
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using YidanSoft.Tool;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using YidanEHRApplication.Helpers;

namespace YidanEHRApplication.Views.ChildWindows
{
    public partial class RWMasterDrugAuthorizeEntrance
    {
        String cdhx = "";
        public RWMasterDrugAuthorizeEntrance(String cdhxPara)
        {
            cdhx = cdhxPara;
            InitializeComponent();
        }



        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {

        }
        /// <summary>
        /// 获取医嘱套餐类别
        /// </summary>


        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;

                ServiceClient.MasterDrugsSynthesisQueryCompleted +=
                        (sb, eb) =>
                        {
                            if (eb.Error == null)
                            {
                                String[] judge = eb.Result.Split(',');
                                if (judge[0] == "0")
                                {
                                    PublicMethod.RadAlterBox("帐号不存在！", "提醒");
                                    return;
                                }
                                if (judge[1] == "0")
                                {
                                    PublicMethod.RadAlterBox("帐号权限不够！", "提醒");
                                    return;
                                }

                                string encryptPasswordBase64 = HisEncryption.EncodeString(judge[3], HisEncryption.PasswordLength, txtpwd.Password);
                                if (encryptPasswordBase64 != judge[2])
                                {
                                    PublicMethod.RadAlterBox("密码错误！", "提醒");
                                    return;
                                }
                                this.DialogResult = true;
                                this.Close();

                            }
                        };
                ServiceClient.MasterDrugsSynthesisQueryAsync(cdhx, txtName.Text.Trim(), txtpwd.Password);
                ServiceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }



    }
}

