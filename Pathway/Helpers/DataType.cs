using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ComponentModel;

namespace YidanEHRApplication.Helpers
{
    public class QCCenterDataType : INotifyPropertyChanged
    {
        /// <summary>
        /// 唯一标示
        /// </summary>
        public string GUID { get; set; }

        /// <summary>
        /// 病人ID
        /// </summary>
        public string Syxh { get; set; }

        /// <summary>
        /// 病人姓名
        /// </summary>
        public string Hzxm { get; set; }

        /// <summary>
        /// 路径代码
        /// </summary>
        public string Ljdm { get; set; }

        /// <summary>
        /// 路径名称
        /// </summary>
        public string Ljmc { get; set; }

        /// <summary>
        /// 床位医生代码
        /// </summary>
        public string Ysdm { get; set; }

        /// <summary>
        /// 床位医生
        /// </summary>
        public string Ysxm { get; set; }

        /// <summary>
        ///病人入径时间
        /// </summary>
        public string Jrsj { get; set; }

        /// <summary>
        /// 病历超时提示
        /// </summary>
        public string Blts { get; set; }

        /// <summary>
        /// 退出日期
        /// </summary>
        public string Tcrq { get; set; }

        /// <summary>
        /// 强制进入
        /// </summary>
        public string Qzjryy { get; set; }

        /// <summary>
        /// 标准住院费用
        /// </summary>
        public string Jcfy { get; set; }

        /// <summary>
        /// 实际住院费用
        /// </summary>
        public string Sjfy { get; set; }

        /// <summary>
        /// 住院超出费用
        /// </summary>
        public string Ccfy { get; set; }

        /// <summary>
        /// 标准住院天数
        /// </summary>
        public string Zgts { get; set; }

        /// <summary>
        /// 实际住院天数
        /// </summary>
        public string Sjts { get; set; }

        /// <summary>
        /// 住院超出天数
        /// </summary>
        public string Ccts { get; set; }

        /// <summary>
        /// 问题内容
        /// </summary>
        private string _QContent;

        public string QContent
        {
            get
            {
                return _QContent;
            }
            set
            {
                _QContent = value;
                NotifyPropertyChange("QContent");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string PropertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(PropertyName));
            }
        }
    }

    public class DateType
    {
        public enum enumQuestionCenter
        {

        }
    }



}
