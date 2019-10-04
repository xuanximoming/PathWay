using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 表示检查类别表的实体
    /// </summary>
    public partial class PE_ExamDictionary
    {
        /// <summary>
        /// 检查编码
        /// </summary>
        [DataMember()]
        public String Jcbm { get; set; }
        /// <summary>
        /// 检查名称
        /// </summary>
        [DataMember()]
        public String Jcmc { get; set; }
    }
}