using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 病人牵制进入
    /// </summary>
    [DataContract()]
    public class CP_ForceToPath
    {
        /// <summary>
        /// 数据记录ID
        /// </summary>
        [DataMember()]
        public string ID { get; set; }

        /// <summary>
        /// 病人ID
        /// </summary>
        [DataMember()]
        public string Syxh { get; set; }

        /// <summary>
        /// 病人姓名
        /// </summary>
        [DataMember()]
        public string Hzxm { get; set; }

        /// <summary>
        /// 路径代码
        /// </summary>
        [DataMember()]
        public string Ljdm { get; set; }

        /// <summary>
        /// 路径名称
        /// </summary>
        [DataMember()]
        public string Ljmc { get; set; }

        /// <summary>
        /// 床位医生代码
        /// </summary>
        [DataMember()]
        public string Ysdm { get; set; }

        /// <summary>
        /// 床位医生
        /// </summary>
        [DataMember()]
        public string Ysxm { get; set; }

        /// <summary>
        ///病人入径时间
        /// </summary>
        [DataMember()]
        public string Jrsj { get; set; }

        /// <summary>
        /// 病人中途退出路径时间
        /// </summary>
        [DataMember()]
        public string Tcsj { get; set; }

        /// <summary>
        /// 病人完成路径时间
        /// </summary>
        [DataMember()]
        public string Wcsj { get; set; }

        /// <summary>
        /// 当天步骤天数
        /// </summary>
        [DataMember()]
        public string Ljts { get; set; }

        /// <summary>
        /// 路径状态
        /// </summary>
        [DataMember()]
        public string Ljzt { get; set; }

        /// <summary>
        /// 牵制进入原因
        /// </summary>
        [DataMember()]
        public string Qzjryy { get; set; }
    }
}