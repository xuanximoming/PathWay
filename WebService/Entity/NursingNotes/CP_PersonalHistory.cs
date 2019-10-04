using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.NursingNotes
{
    /// <summary>
    /// 个人史实体
    /// </summary>
    [DataContract]
    public class CP_PersonalHistory
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
        /// 婚姻状况(CP_DictionaryDetail.Lbdm = 4)
        /// </summary>
        [DataMember]
        public string Hyzk { get; set; }

        /// <summary>
        /// 孩子数量
        /// </summary>
        [DataMember]
        public int Hzsl { get; set; }

        /// <summary>
        /// 职业名称
        /// </summary>
        [DataMember]
        public string Zymc { get; set; }

        /// <summary>
        /// 是否饮酒（1：有 0：无）
        /// </summary>
        [DataMember]
        public int Sfyj { get; set; }

        /// <summary>
        /// 饮酒史
        /// </summary>
        [DataMember]
        public string Yjs { get; set; }

        /// <summary>
        /// 是否吸烟（1：有 0：无）
        /// </summary>
        [DataMember]
        public int Sfxy { get; set; }

        /// <summary>
        /// 吸烟史
        /// </summary>
        [DataMember]
        public string Xys { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Memo { get; set; }

        /// <summary>
        /// 出生地
        /// </summary>
        [DataMember]
        public string Csd { get; set; }

        /// <summary>
        /// 经历地
        /// </summary>
        [DataMember]
        public string Jld { get; set; }
    }
}