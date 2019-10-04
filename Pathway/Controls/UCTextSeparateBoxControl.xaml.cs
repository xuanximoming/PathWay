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

using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Controls
{
    public partial class UCTextSeparateBoxControl : UserControl
    {
        public UCTextSeparateBoxControl()
        {
            InitializeComponent();            
        }   
        /// <summary>
        /// 判断输入是否为空
        /// </summary>
        public bool IsInput
        {
            get
            {
                return !(txtDBP.Text == "" || txtSBP.Text == "");  //true非空，false空
            }
        }

        /// <summary>
        /// 
        /// 血压值
        /// </summary>
        public string BP
        {
            get { return txtSBP.Text.Trim() + txtSBP.Text.Trim() == "" ? "" : (txtSBP.Text.Trim() + "/") + txtDBP.Text.Trim(); }
        }

        /// <summary>
        /// 数据重置
        /// </summary>
        public void DateTextReset()
        {
            txtDBP.Text = "";
            txtSBP.Text = "";
        }





        /// <summary>
        /// 检测数据
        /// </summary>
        /// <returns></returns>
        public bool CheckData()
        {
            if (txtSBP.Text.Trim() == "" || txtDBP.Text.Trim() == "") return true;//空退出

            int intBp;

            if (!Dataprocessing.IsNumber(txtSBP.Text.Trim().ToString(), 0))
            {
                PublicMethod.RadAlterBox("收缩压必须为正整数!", "提示");
                return false;
            }
            else
            {
                intBp = Convert.ToInt32(txtSBP.Text.Trim().ToString());
                if (!(intBp >= 110 && intBp <= 180))
                {
                    PublicMethod.RadAlterBox("收缩压必须在110mmHg至280mmHg之间!", "提示");
                    return false;
                }
                //else
                //{
                //    return true;
                //}
            }

            if (!Dataprocessing.IsNumber(txtDBP.Text.Trim().ToString(), 0))
            {
                PublicMethod.RadAlterBox("舒张压必须为正整数!", "提示");
                return false;
            }
            else
            {
                intBp = Convert.ToInt32(txtDBP.Text.Trim().ToString());
                if (!(intBp >= 60 && intBp <= 110))
                {
                    PublicMethod.RadAlterBox("舒张压必须在30mmHg至110mmHg之间!", "提示");
                    return false;
                }
                //else
                //{
                //    return true;
                //}
            }

            return true;
        }

    }
}
