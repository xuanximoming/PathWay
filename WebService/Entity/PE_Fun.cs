using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 功能
    /// </summary>
    [DataContract]
    public class PE_Fun
    {
        /// <summary>
        /// 功能编码
        /// </summary>
        [DataMember()]
        public string FunCode { get; set; }
        /// <summary>
        /// 功能名称
        /// </summary>
        [DataMember()]
        public string FunName { get; set; }
        /// <summary>
        /// 父功能编码
        /// </summary>
        [DataMember()]
        public string FunCodeFather { get; set; }
        /// <summary>
        /// 功能地址
        /// </summary>
        [DataMember()]
        public string FunURL { get; set; }



        /// <summary>
        /// 父功能名称
        /// </summary>
        [DataMember()]
        public string Fa_FunName { get; set; }


        /// <summary>
        /// 只有在用户权限维护模块使用
        /// 角色是否有该功能权限，有：1,  无：0
        /// </summary>
        [DataMember()]
        public int ISCheck { get; set; }
 
    }
}