using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 路径节点护理执行结果表
    /// </summary>
    public partial class CP_NurExecRecordResult
    {
        /// <summary>
        /// 护理执行结果编号
        /// </summary>
        [DataMember()]
        public String Id { get; set; }
        /// <summary>
        /// 护理执行编号
        /// </summary>
        [DataMember()]
        public String HlzxId { get; set; }
        /// <summary>
        /// 结果编号
        /// </summary>
        [DataMember()]
        public String JgId { get; set; }
        /// <summary>
        /// 有效记录
        /// </summary>
        [DataMember()]
        public String Yxjl { get; set; }
    }
}