using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract]
    public class CP_AdviceSuit
    {
        /// <summary>
        /// --套餐序号
        /// </summary>
        [DataMember()]
        public Decimal Ctyzxh { get; set; }
        /// <summary>
        /// (医嘱套餐)名称
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
        /// (所属)科室代码(YY_KSDMK.Ksdm)
        /// </summary>
        [DataMember()]
        public String Ksdm { get; set; }
        /// <summary>
        /// 所属)病区代码(YY_BQDMK.Bqdm)
        /// </summary>
        [DataMember()]
        public String Bqdm { get; set; }
        /// <summary>
        /// (所属)医生代码(CP_Employee.zgdm)
        /// </summary>
        [DataMember()]
        public String Ysdm { get; set; }
        /// <summary>
        /// 使用范围(CP_DataCategory.Mxbh, Lbbh = 29)
        /// </summary>
        [DataMember()]
        public String Syfw { get; set; }
        /// <summary>
        /// --备注
        /// </summary>
        [DataMember()]
        public String Memo { get; set; }
        /// <summary>
        /// --排序字段
        /// </summary>
        [DataMember()]
        public String OrderNum { get; set; }
        /// <summary>
        /// --使用原因1
        /// </summary>
        [DataMember()]
        public String UserReason1 { get; set; }  
        /// <summary>
        /// --使用原因2
        /// </summary>
        [DataMember()]
        public String UserReason2 { get; set; }  
        /// <summary>
        /// --使用原因3
        /// </summary>
        [DataMember()]
        public String UserReason3 { get; set; }
        /// --录入人的代码
        /// </summary>
        [DataMember()]
        public String Zgdm { get; set; }

        /// <summary>
        /// 套餐类别序号
        /// </summary>
        [DataMember()]
        public String CategoryId { get; set; }
    }
}