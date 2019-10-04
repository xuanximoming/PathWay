using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract]
    public class CP_AdviceSuitCategory
    {
        /// <summary>
        /// 套餐类别序号
        /// </summary>
        [DataMember()]
        public String CategoryId { get; set; }
        /// <summary>
        /// (医嘱套餐类别)名称
        /// </summary>
        [DataMember()]
        public String Name { get; set; }
        /// <summary>
        /// --拼音
        /// </summary>
        [DataMember()]
        public String Py { get; set; }
        /// <summary>
        /// 	--五笔
        /// </summary>
        [DataMember()]
        public String Wb { get; set; }
        /// <summary>
        /// 职工代码(录入人员代码)
        /// </summary>
        [DataMember()]
        public String Zgdm { get; set; }
        /// <summary>
        /// 录入时间
        /// </summary>
        [DataMember()]
        public String AddTime { get; set; }
        /// <summary>
        /// 有效记录
        /// </summary>
        [DataMember()]
        public String Yxjl { get; set; }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember()]
        public String Memo { get; set; }
        /// <summary>
        /// 父节点
        /// </summary>
        [DataMember()]
        public String ParentID { get; set; }
        [DataMember()]
        public String ParentName { get; set; }
        /// <summary>
        /// 医嘱套餐集合
        /// </summary>
        [DataMember()]
        public List<CP_AdviceSuit> AdviceSuitList { get; set; }
    }
}