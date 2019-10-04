using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.ReportForms
{
    /// <summary>
    /// 结算费用比例表
    /// </summary>
    [DataContract()]
    public class Rpt_InPathPatientFeePercent
    {
        /// <summary>
        /// 路径名称
        /// </summary>
        [DataMember]
        public String Ljmc { get; set; }

        /// <summary>
        /// 病人首页序号
        /// </summary>
        [DataMember]
        public String SyxhID { get; set; }

        /// <summary>
        /// 病人姓名
        /// </summary>
        [DataMember]
        public String Hzxm { get; set; }

        /// <summary>
        /// 住院天数
        /// </summary>
        [DataMember]
        public String Zyts { get; set; }

        /// <summary>
        /// 标准天数
        /// </summary>
        [DataMember]
        public String Bzts { get; set; }

        /// <summary>
        /// 标准费用
        /// </summary>
        [DataMember]
        public String Bzfy { get; set; }

        /// <summary>
        /// 其它费用
        /// </summary>
        [DataMember]
        public String Qita { get; set; }

        /// <summary>
        /// 西药费
        /// </summary>
        [DataMember]
        public String XyFei { get; set; }

        /// <summary>
        /// 治疗费
        /// </summary>
        [DataMember]
        public String ZhiliaoFei { get; set; }

        /// <summary>
        /// 检查费
        /// </summary>
        [DataMember]
        public String JcFei { get; set; }

        /// <summary>
        /// 检验费
        /// </summary>
        [DataMember]
        public String JyFei { get; set; }

        /// <summary>
        /// 诊疗费
        /// </summary>
        [DataMember]
        public String ZlFei { get; set; }

        /// <summary>
        /// 床位费
        /// </summary>
        [DataMember]
        public String CwFei { get; set; }

        /// <summary>
        /// 护士护理费
        /// </summary>
        [DataMember]
        public String HshlFei { get; set; }

        /// <summary>
        /// 总计
        /// </summary>
        [DataMember]
        public String Zj { get; set; }

        /// <summary>
        /// 押金累计
        /// </summary>
        [DataMember]
        public String Yjlj { get; set; }

        /// <summary>
        /// 路径状态
        /// </summary>
        [DataMember]
        public String Ljzt { get; set; }
    }
}