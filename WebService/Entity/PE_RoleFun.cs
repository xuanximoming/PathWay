using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 角色功能
    /// </summary>
    [DataContract]
    public class PE_RoleFun
    {
        /// <summary>
        /// 角色编码
        /// </summary>
        [DataMember()]
        public string RoleCode { get; set; }
        /// <summary>
        /// 功能编码
        /// </summary>
        [DataMember()]
        public string FunCode { get; set; }
    }
}