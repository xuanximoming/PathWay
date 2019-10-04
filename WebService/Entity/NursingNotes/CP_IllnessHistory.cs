using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.NursingNotes
{
    /// <summary>
    /// 疾病史实体
    /// </summary>
    [DataContract]
    public class CP_IllnessHistory
    {

        /// <summary>
        /// 唯一标示ID
        /// </summary>
        [DataMember]
        public string ID { get; set; }

        /// <summary>
        /// 病人首页序号
        /// </summary>
        [DataMember]
        public string Syxh { get; set; }

        /// <summary>
        /// 病种代码（对应CP_Diagnosis.Zdbs）
        /// </summary>
        [DataMember]
        public string Bzdm { get; set; }

        /// <summary>
        /// 病种名称（页面显示使用）
        /// </summary>
        [DataMember]
        public string BzName { get; set; }

        /// <summary>
        /// 疾病评论
        /// </summary>
        [DataMember]
        public string Jbpl { get; set; }

        /// <summary>
        /// 病发时间
        /// </summary>
        [DataMember]
        public string Bfsj { get; set; }

        /// <summary>
        /// 是否治愈（：是0：否）
        /// </summary>
        [DataMember]
        public string Sfzy { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Memo { get; set; }


    }
}