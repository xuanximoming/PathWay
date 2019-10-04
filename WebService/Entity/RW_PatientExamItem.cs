using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 表示病人检查项的实体类
    /// </summary>
    [DataContract]
    public class RW_PatientExamItem
    {
        /// <summary>
        /// 序号
        /// </summary>
        [DataMember()]
        public String ID { get; set; }
        /// <summary>
        /// 病人首页序号
        /// </summary>
        [DataMember()]
        public String Syxh { get; set; }
        /// <summary>
        /// 检查项目
        /// </summary>
        [DataMember()]
        public String Jcxm { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [DataMember()]
        public String Jcmc { get; set; }
        /// <summary>
        /// 检查结果
        /// </summary>
        [DataMember()]
        public String Jcjg { get; set; }
        /// <summary>
        /// 单位
        /// </summary>
        [DataMember]
        public String Dw { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember()]
        public String Bz { get; set; }
    }
}