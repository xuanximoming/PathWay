using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [DataContract]
    public class PE_UserRole
    {
        /// <summary>
        /// 用户编号
        /// </summary>
        [DataMember()]
        public string UserID { get; set; }
        /// <summary>
        /// 角色编码
        /// </summary>
        [DataMember()]
        public string RoleCode { get; set; }
    }

    /// <summary>
    /// 用户角色查询实体（用户角色维护查询页面使用）
    /// </summary>
    [DataContract]
    public class PE_UserRoleList
    {
 
        /// <summary>
        /// 职工代码
        /// </summary>
        [DataMember()]
        public string Zgdm { get; set; }

        /// <summary>
        /// 职工姓名
        /// </summary>
        [DataMember()]
        public string Name { get; set; }

        /// <summary>
        /// 角色名称
        /// </summary>
        [DataMember()]
        public string RoleName { get; set; }

        /// <summary>
        /// 角色代码
        /// </summary>
        [DataMember()]
        public string RoleCode{ get; set; }

        /// <summary>
        /// 职工性别
        /// </summary>
        [DataMember()]
        public string Zgxb{ get; set; }

        /// <summary>
        /// 科室代码
        /// </summary>
        [DataMember()]
        public string DeptName{ get; set; }

        /// <summary>
        /// 病区名称
        /// </summary>
        [DataMember()]
        public string BqName{ get; set; }

        /// <summary>
        /// 医师级别
        /// </summary>
        [DataMember()]
        public string Ysjb{ get; set; }
    }
}