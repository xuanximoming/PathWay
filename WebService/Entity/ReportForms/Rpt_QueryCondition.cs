using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service
{
    /// <summary>
    /// 表示报表查询条件的类
    /// </summary>
    [DataContract()]
    public class Rpt_QueryCondition
    {
        /// <summary>
        /// 表示开始时间的属性
        /// </summary>
        [DataMember()]
        public String Stardate
        {
            get;
            set;
        }
        /// <summary>
        /// 表示结束时间的属性
        /// </summary>
        [DataMember()]
        public String Enddate
        {
            get;
            set;
        }
        /// <summary>
        /// 表示部门的属性
        /// </summary>
        [DataMember()]
        public String DeptInfo
        {
            get;
            set;
        }
        /// <summary>
        /// 表示路径的属性
        /// </summary>
        [DataMember()]
        public String Path
        {
            get;
            set;
        }
        /// <summary>
        /// 表示部门名称的属性
        /// </summary>
        [DataMember()]
        public String DeptName
        {
            get;
            set;
        }
        /// <summary>
        /// 表示路径名称的属性
        /// </summary>
        [DataMember()]
        public String PathName
        {
            get;
            set;
        }

    }
}