using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;
using System.ComponentModel;


namespace Yidansoft.Service.Entity
{   
    /// <summary>
    /// 医务处问题审核
    /// </summary>
    [DataContract()]
    public class CP_QCProblem:INotifyPropertyChanged
    {
        /// <summary>
        /// 问题序号
        /// </summary>
        [DataMember()]
        public int Wtxh { get; set; }

        /// <summary>
        /// 病人ID
        /// </summary>
        [DataMember()]
        public int Syxh { get; set; }

        /// <summary>
        /// 病人姓名
        /// </summary>
        [DataMember()]
        public string Hzxm { get; set; }

        /// <summary>
        /// 问题状态：0-登记（默认）,1-答复-2挂起-4关闭问题
        /// </summary>
        [DataMember()]
        public int Wtzt { get; set; }

        /// <summary>
        /// 路径代码
        /// </summary>
        [DataMember()]
        public string Ljdm { get; set; }

        /// <summary>
        /// 路径名称
        /// </summary>
        [DataMember()]
        public string Ljmc { get; set; }

        /// <summary>
        /// 问题内容
        /// </summary>
        [DataMember()]
        public string Wtnr { get; set; }

        /// <summary>
        /// 回复内容
        /// </summary>
        string m_Dfnr;
        [DataMember()]
        public string Dfnr 
        { 
            get
            { return m_Dfnr;
            }

            set
            {
                m_Dfnr=value;
                NotifyPropertyChange("Dfnr");
            }
        }

        /// <summary>
        /// 审核内容
        /// </summary>
        [DataMember()]
        public string Shnr { get; set; }

        /// <summary>
        /// 医生代码(住院医生代码)
        /// </summary>
        [DataMember()]
        public string Zrys { get; set; }

        /// <summary>
        /// 医生姓名
        /// </summary>
        [DataMember()]
        public string Ysxm { get; set; }

        /// <summary>
        /// 登记日期
        /// </summary>
        [DataMember()]
        public string Djrq { get; set; }

        /// <summary>
        /// 登记人员ID
        /// </summary>
        [DataMember()]
        public string Djry { get; set; }

        /// <summary>
        /// 登记人员姓名
        /// </summary>
        [DataMember()]
        public string Djryxm { get; set; }

        /// <summary>
        /// 回复日期
        /// </summary>
        [DataMember()]
        public string Dfrq { get; set; }

        /// <summary>
        /// 回复医生ID
        /// </summary>
        [DataMember()]
        public string Dfys { get; set; }

        /// <summary>
        /// 回复医生姓名
        /// </summary>
        [DataMember()]
        public string Dfysxm { get; set; }

        /// <summary>
        /// 审核日期
        /// </summary>
        [DataMember()]
        public string Shrq { get; set; }

        /// <summary>
        /// 审核人员ID
        /// </summary>
        [DataMember()]
        public string Shry { get; set; }

        /// <summary>
        /// 审核人员姓名
        /// </summary>
        [DataMember()]
        public string Shryxm { get; set; }

        /// <summary>
        /// 作废日期
        /// </summary>
        [DataMember()]
        public string Zfrq { get; set; }

        /// <summary>
        /// 作废人员ID
        /// </summary>
        [DataMember()] 
        public string Zfry { get; set; }

        /// <summary>
        /// 作废人员姓名
        /// </summary>
        [DataMember()]
        public string Zfryxm { get; set; }

        /// <summary>
        /// 问题状态名称
        /// </summary>
        [DataMember()]
        public string Qczt { get; set; }

        /// <summary>
        /// 审核状态
        /// </summary>
        [DataMember()]
        public string Shzt { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;
        private void NotifyPropertyChange(string propertyName)
        {
            if (PropertyChanged != null)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
            }
        }   

        
    }
}