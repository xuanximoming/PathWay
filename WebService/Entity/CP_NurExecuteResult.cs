using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 护理执行结果表
    /// </summary>
    public partial class CP_NurExecuteResult
    {
        /// <summary>
        /// 自增序号
        /// </summary>
        [DataMember()]
        public String Id { get; set; }
        /// <summary>
        /// 护理编号
        /// </summary>
        [DataMember()]
        public String Mxxh { get; set; }
        /// <summary>
        /// 结果编号
        /// </summary>
        [DataMember()]
        public String Jgbh { get; set; }
        /// <summary>
        /// 结果名称
        /// </summary>
        [DataMember()]
        public String JgName { get; set; }
        /// <summary>
        /// 有效记录
        /// </summary>
        [DataMember()]
        public String Yxjl { get; set; }
        /// <summary>
        /// 有效记录名称
        /// </summary>
        [DataMember()]
        public String YxjlName { get; set; }
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

        private Boolean isChecked = false;
        /// <summary>
        /// 是否选中
        /// </summary>
        [DataMember()]
        public Boolean IsChecked
        {
            get { return isChecked; }
            set { isChecked = value; }
        }
    }
}