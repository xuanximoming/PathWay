using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.ReportForms
{
    /// <summary>
    /// 入径统计表
    /// </summary>
    [DataContract()]
    public class Rpt_PathEnterStatistics
    {
        /// <summary>
        /// 科室名称
        /// </summary>
        [DataMember]
        public String Ksmc { get; set; }
        /// <summary>
        /// 路径代码
        /// </summary>
        [DataMember]
        public String Ljdm { get; set; }
        /// <summary>
        /// 路径名称
        /// </summary>
        [DataMember]
        public String Ljmc { get; set; }
        /// <summary>
        /// 病患总数
        /// </summary>
        [DataMember]
        public String Bhzs { get; set; }
        /// <summary>
        /// 入径人数
        /// </summary>
        [DataMember]
        public String Rjrs { get; set; }
        /// <summary>
        /// 入径率
        /// </summary>
        [DataMember]
        public String Rjl { get; set; }
        /// <summary>
        /// 完成人数
        /// </summary>
        [DataMember]
        public String Wcrs { get; set; }
        /// <summary>
        /// 完成率
        /// </summary>
        [DataMember]
        public String Wcl { get; set; }
        /// <summary>
        /// 退出人数
        /// </summary>
        [DataMember]
        public String Tcrs { get; set; }
        /// <summary>
        /// 退出率
        /// </summary>
        [DataMember]
        public String Tcl { get; set; }
        /// <summary>
        /// 在径人数
        /// </summary>
        [DataMember]
        public String Zjrs { get; set; }
        /// <summary>
        /// 在径率
        /// </summary>
        [DataMember]
        public String Zjl { get; set; }
    }
}