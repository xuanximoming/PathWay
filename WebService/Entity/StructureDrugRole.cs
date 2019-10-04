using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 构建药物角色
    /// </summary>
    [DataContract]
    public class StructureDrugRole
    {
        /// <summary>
        /// 药物角色编码
        /// </summary>
        [DataMember()]
        public string Jsbm { get; set; }
        /// <summary>
        /// 药物角色名称
        /// </summary>
        [DataMember()]
        public string Jsmc { get; set; }
        /// <summary>
        /// 用户职工代码
        /// </summary>
        [DataMember()]
        public string Zgdm { get; set; }

        /// <summary>
        /// 是否有当前角色（在药物用户角色权限模块中使用到）
        /// </summary>
        [DataMember()]
        public string IsCheck { get; set; }
    }
}