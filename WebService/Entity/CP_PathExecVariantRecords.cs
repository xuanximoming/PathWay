using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{   
    /// <summary>
    /// 临床路径实际执行变异查询
    /// </summary>
    [DataContract()]
    public class CP_PathExecVariantRecords
    {   
        /// <summary>
        /// 项目类别名称
        /// </summary>
        [DataMember()]
        public string Lbmc { get; set; }
        /// <summary>
        /// 项目名称
        /// </summary>
        [DataMember()]
        public string Xmmc { get; set; }
        /// <summary>
        /// 项目类别代码
        /// </summary>
        [DataMember()]
        public int Xmlb { get; set; }
        /// <summary>
        /// 变异原因
        /// </summary>
        [DataMember()]
        public string Byyy { get; set; }
        /// <summary>
        /// 变异类别代码
        /// </summary>
        [DataMember()]
        public string Bylb { get; set; }
        /// <summary>
        /// 变异类别名称
        /// </summary>
        [DataMember()]
        public string Blbmc { get; set;}
        /// <summary>
        /// 变异时间
        /// </summary>
        [DataMember()]
        public string Bysj { get; set; }

        /// <summary>
        /// 变异时间
        /// </summary>
        [DataMember()]
        public string Bynr { get; set; }

    }
}