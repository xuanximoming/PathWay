using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 在院病人
    /// </summary>
    [DataContract]
    public class PE_PatientIn
    {
        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember()]
        public string Name { get; set; }
        /// <summary>
        /// ID
        /// </summary>
        [DataMember()]
        public string ID { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [DataMember()]
        public string Sex { get; set; }
        /// <summary>
        /// 科室
        /// </summary>
        [DataMember()]
        public string Dept { get; set; }
        /// <summary>
        /// 病区
        /// </summary>
        [DataMember()]
        public string Ward { get; set; }
        /// <summary>
        /// 临床路径
        /// </summary>
        [DataMember()]
        public string Path { get; set; }
        /// <summary>
        /// 路径代码
        /// </summary>
        [DataMember()]
        public string PathCode { get; set; }
        /// <summary>
        /// 路径状态
        /// </summary>
        [DataMember()]
        public string PathStatus { get; set; }
        /// <summary>
        /// 入院时间
        /// </summary>
        [DataMember()]
        public string AdmitDate { get; set; }

    }
}