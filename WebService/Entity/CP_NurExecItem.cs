using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 护理执行项目表
    /// </summary>
    public partial class CP_NurExecItem
    {
        /// <summary>
        /// 项目编号
        /// </summary>
        [DataMember()]
        public String Xmxh { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [DataMember()]
        public String Name { get; set; }
    }
}