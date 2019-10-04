using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 护理执行类别表
    /// </summary>
    public partial class CP_NurExecCategory
    {
        /// <summary>
        /// 类别编码
        /// </summary>
        [DataMember()]
        public String Lbxh{get;set;}
        /// <summary>
        /// 类别名称
        /// </summary>
        [DataMember()]
        public String Name{get;set;}
        /// <summary>
        /// 项目编码
        /// </summary>
        [DataMember()]
        public String Xmxh{get;set;}
        /// <summary>
        /// 项目名称
        /// </summary>
        [DataMember()]
        public String XmxhName { get; set; }
        /// <summary>
        /// 输入控件类型（0无,1用户控件）暂时保留
        /// </summary>
        [DataMember()]
        public String InputType{get;set;}
        /// <summary>
        /// 排序字段
        /// </summary>
        [DataMember()]
        public String OrderValue{get;set;}
        /// <summary>
        /// 有效记录编号
        /// </summary>
        [DataMember()]
        public String Yxjl{get;set;}
        /// <summary>
        /// 有效记录名称
        /// </summary>
        [DataMember()]
        public String Yxjlmc { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember()]
        public String Create_Time{get;set;}
        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember()]
        public String Create_User{get;set;}
        [DataMember()]
        ///修改时间
        public String Cancel_Time{get;set;}
        /// <summary>
        /// 修改人
        /// </summary>
        [DataMember()]
        public String Cancel_User{get;set;}
    }
}