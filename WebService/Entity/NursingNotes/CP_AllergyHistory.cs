using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.NursingNotes
{
    /// <summary>
    /// 过敏史实体
    /// </summary>
    [DataContract]
    public class CP_AllergyHistory
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
        /// 过敏类型(CP_DataCategoryDetail维护)
        /// </summary>
        [DataMember]
        public string Gmlx { get; set; }

        /// <summary>
        /// 过敏类型名称(显示使用)
        /// </summary>
        [DataMember]
        public string Gmlx_Name { get; set; }

        /// <summary>
        /// 过敏程度（CP_DataCategoryDetail维护）
        /// </summary>
        [DataMember]
        public string Gmcd { get; set; }

        /// <summary>
        /// 过敏程度名称(显示使用)
        /// </summary>
        [DataMember]
        public string Gmcd_Name { get; set; }

        /// <summary>
        /// 代理医生
        /// </summary>
        [DataMember]
        public string Dlys { get; set; }

        /// <summary>
        /// 过敏部位
        /// </summary>
        [DataMember]
        public string Gmbw { get; set; }

        /// <summary>
        /// 反应类型
        /// </summary>
        [DataMember]
        public string Fylx { get; set; }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember]
        public string Memo { get; set; }


    }
}