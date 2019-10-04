using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 病人检查结果
    /// </summary>
    [DataContract()]
    public class V_PatientExamine
    {
        /// <summary>
        /// 病人姓名
        /// </summary>
        [DataMember]
        public String PatientName
        { get; set; }
        /// <summary>
        /// 类别
        /// </summary>
        [DataMember]
        public String Lb
        { get; set; }
        /// <summary>
        /// 检查日期
        /// </summary>
        [DataMember]
        public String Jcrq
        { get; set; }

        /// <summary>
        /// 检查项目
        /// </summary>
        [DataMember]
        public String Jcxm
        { get; set; }
        /// <summary>
        /// 检查结果
        /// </summary>
        [DataMember]
        public String Jcjg
        { get; set; }
        /// <summary>
        /// 正常范围
        /// </summary>
        [DataMember]
        public String Zcfw
        { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        [DataMember]
        public String Dw
        { get; set; }


    }
}