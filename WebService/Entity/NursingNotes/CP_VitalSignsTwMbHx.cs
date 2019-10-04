using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.NursingNotes
{
    /// <summary>
    /// 体温单数据实体
    /// </summary>
    [DataContract]
    public class CP_VitalSignsTwMbHx
    {
        /// <summary>
        /// 体温单测量的日期
        /// </summary>
        [DataMember]
        public string Clrq { get; set; }

        /// <summary>
        /// 护士对病人进行检查的时间段（一般医院一般为2,6,10,14,18,22）
        /// </summary>
        [DataMember]
        public string Sjd { get; set; }

        /// <summary>
        /// 患者体温的实体
        /// </summary>
        [DataMember]
        public string Hztw { get; set; }

        /// <summary>
        /// 患者脉搏的实体
        /// </summary>
        [DataMember]
        public string Hzmb { get; set; }

        /// <summary>
        /// 患者呼吸的实体
        /// </summary>
        [DataMember]
        public string Hzhx { get; set; }


    }
}