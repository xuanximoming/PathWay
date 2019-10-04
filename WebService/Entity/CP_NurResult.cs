using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 护理结果表
    /// </summary>
    public partial class CP_NurResult
    {
        /// <summary>
        /// 结果编号
        /// </summary>
        [DataMember()]
        public String Jgbh { get; set; }
        /// <summary>
        /// 结果名称
        /// </summary>
        [DataMember()]
        public String Name { get; set; }
        /// <summary>
        /// 有效记录
        /// </summary>
        [DataMember()]
        public String Yxjl { get; set; }
        /// <summary>
        /// 有效记录名称
        /// </summary>
        [DataMember()]
        public String Yxjlmc { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember()]
        public String Create_Time { get; set; }
        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember()]
        public String Create_User { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        [DataMember()]
        public String Update_Time { get; set; }
        /// <summary>
        /// 修改人
        /// </summary>
        [DataMember()]
        public String Update_User { get; set; }
    }
}