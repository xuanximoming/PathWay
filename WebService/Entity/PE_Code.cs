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
    public class PE_CodeList
    {

        /// <summary>
        /// 检查项目编码
        /// </summary>
        [DataMember()]
        public string Jcbm { get; set; }

        /// <summary>
        /// 父节点编码
        /// </summary>
        [DataMember()]
        public string Fjd { get; set; }

        /// <summary>
        /// 检查项目编码名称
        /// </summary>
        [DataMember()]
        public string Jcmc { get; set; }

        /// <summary>
        /// 名称缩写符号
        /// </summary>
        [DataMember()]
        public string Mcsx { get; set; }

        /// <summary>
        /// 拼音
        /// </summary>
        [DataMember()]
        public string Py { get; set; }

        /// <summary>
        /// 五笔
        /// </summary>
        [DataMember()]
        public string Wb { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember()]
        public string Bz { get; set; }

    }
}