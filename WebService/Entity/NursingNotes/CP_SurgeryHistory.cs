using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.NursingNotes
{
    /// <summary>
    /// 手术史实体
    /// </summary>
    [DataContract]
    public class CP_SurgeryHistory
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
        /// 手术代码(CP_Surgery维护)
        /// </summary>
        [DataMember]
        public string Ssdm { get; set; }

        /// <summary>
        /// 手术名称（页面显示用）
        /// </summary>
        [DataMember]
        public string SsName { get; set; }

        /// <summary>
        /// 疾病（对应CP_DiseaseCFG.Bzdm）
        /// </summary>
        [DataMember]
        public string Bzdm { get; set; }

        /// <summary>
        /// 病种名称（页面显示用）
        /// </summary>
        [DataMember]
        public string BzName { get; set; }

        /// <summary>
        /// 手术评论
        /// </summary>
        [DataMember]
        public string Sspl { get; set; }

        /// <summary>
        /// 手术医生
        /// </summary>
        [DataMember]
        public string Ssys { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Memo { get; set; }


    }
}