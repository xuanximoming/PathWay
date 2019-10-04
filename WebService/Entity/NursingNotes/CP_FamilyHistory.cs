using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.NursingNotes
{
    /// <summary>
    /// 家族史实体
    /// </summary>
    [DataContract]
    public class CP_FamilyHistory
    {

        /// <summary>
        /// 唯一标示ID
        /// </summary>
        [DataMember]
        public decimal ID { get; set; }

        /// <summary>
        /// 病人首页序号
        /// </summary>
        [DataMember]
        public decimal Syxh { get; set; }

        /// <summary>
        /// 家族关系代码(家族关系(CP_DataCategoryDetail.Mxbh  Lbbh = 62))
        /// </summary>
        [DataMember]
        public short Jzgx { get; set; }

        /// <summary>
        /// 出生日期（在前台显示年龄）
        /// </summary>
        [DataMember]
        public string Csrq { get; set; }

        ///<summary>
        ///显示年龄
        ///</summary>
        [DataMember]
        public string Xsnl { get; set; }

        /// <summary>
        /// 病种代码（对应CP_DiseaseCFG.Bzdm）
        /// </summary>
        [DataMember]
        public string Bzdm { get; set; }

        /// <summary>
        /// 是否健在(1:是0：否)
        /// </summary>
        [DataMember]
        public int Sfjz { get; set; }

        /// <summary>
        /// 死亡原因
        /// </summary>
        [DataMember]
        public string Swyy { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Memo { get; set; }

        /// <summary>
        /// 家族关系Name
        /// </summary>
        [DataMember]
        public string JzgxName { get; set; }


        /// <summary>
        /// 家族关系Name
        /// </summary>
        [DataMember]
        public string SfjzName { get; set; }

        /// <summary>
        /// 病种代码Name
        /// </summary>
        [DataMember]
        public string BzdmName { get; set; }
    }
}