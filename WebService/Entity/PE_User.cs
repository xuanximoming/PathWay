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
    public class PE_User
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember()]
        public string UserID { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        [DataMember()]
        public string UserName { get; set; }
        /// <summary>
        /// 用户科室
        /// </summary>
        [DataMember()]
        public string UserDept { get; set; }
    }
}