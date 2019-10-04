using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 医生每日提示信息
    /// </summary>
    [DataContract]
    public class CP_DoctorTaskMessage
    {
        /// <summary>
        /// 病人首页序号
        /// </summary>
        [DataMember()]
        public string Syxh { get; set; }

        /// <summary>
        /// 患者姓名
        /// </summary>
        [DataMember()]
        public string Hzxm { get; set; }

        /// <summary>
        /// 患者床位号
        /// </summary>
        [DataMember()]
        public string Cycw { get; set; }

        /// <summary>
        /// 路径名称
        /// </summary>
        [DataMember()]
        public string Ljmc { get; set; }

        /// <summary>
        /// 床位医生姓名              
        /// </summary>
        [DataMember()]
        public string Ysxm { get; set; }

        /// <summary>
        /// 提示信息
        /// </summary>
        [DataMember()]
        public string Mess { get; set; }

        /// <summary>
        /// 期望完成时间
        /// </summary>
        [DataMember()]
        public string Yqsj { get; set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        [DataMember()]
        public string Rwzt { get; set; }

        /// <summary>
        /// 功能编码
        /// </summary>
        [DataMember()]
        public string group_col { get; set; }

        /// <summary>
        /// 医嘱类别
        /// </summary>
        [DataMember()]
        public string Yzlb { get; set; }
 
    }
}