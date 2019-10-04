using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{

    /// <summary>
    /// 编码维护查询实体（查询页面使用）
    /// </summary>
    [DataContract]
    public class PE_Hlxm
    {
        /// <summary>
        /// 项目编码
        /// </summary>
        [DataMember()]
        public string Xmxh { get; set; }

        /// <summary>
        /// 项目名称
        /// </summary>
        [DataMember()]
        public string Name { get; set; }

        /// <summary>
        /// 排序字段
        /// </summary>
        [DataMember()]
        public string OrderValue { get; set; }

        /// <summary>
        /// 是否有效
        /// </summary>
        [DataMember()]
        public string Yxjl { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember()]
        public string Create_Time { get; set; }

        /// <summary>
        /// 创建者
        /// </summary>
        [DataMember()]
        public string Create_User { get; set; }

        /// <summary>
        /// 修改时间
        /// </summary>
        [DataMember()]
        public string Cancel_Time { get; set; }

        /// <summary>
        /// 修改者
        /// </summary>
        [DataMember()]
        public string Cancel_User { get; set; }

    }
}