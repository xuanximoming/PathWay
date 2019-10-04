using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.ReportForms
{
    /// <summary>
    /// 路径证断表
    /// </summary>
    [DataContract()]
    public class Rpt_ClinicalDiagnosis
    {
        /// <summary>
        /// 病种代码
        /// </summary>
        [DataMember]
        public String Bzdm { get; set; }
        /// <summary>
        /// 病种名称
        /// </summary>
        [DataMember]
        public String Bzmc { get; set; }
    }
}