using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 角色
    /// </summary>
    [DataContract]
    public class PE_Role
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        [DataMember()]
        public string RoleCode { get; set; }
        /// <summary>
        /// 角色名称
        /// </summary>
        [DataMember()]
        public string RoleName { get; set; }

        /// <summary>
        /// 是否有当前角色（在用户角色权限模块中使用到）
        /// </summary>
        [DataMember()]
        public int IsCheck { get; set; }
    }
}