using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract]
    public partial class Modal_PatientContactsInfo
    {
        /// <summary>
        /// 联系人编号
        /// </summary>
        [DataMember]
        public string  Lxbh { get; set; } 
        /// <summary>
        /// 联系人姓名
        /// </summary>
        [DataMember]
        public string Lxrm { get; set; }
        /// <summary>
        /// 联系人性别
        /// </summary>
        [DataMember]
        public string Lxrxb { get; set; } 
        /// <summary>
        /// 联系人关系
        /// </summary>
        [DataMember]
        public string Lxgx { get; set; }  
        /// <summary>
        /// 联系人地址
        /// </summary>
        [DataMember]
        public string Lxdz { get; set; }  
        /// <summary>
        /// 联系人单位
        /// </summary>
        [DataMember]
        public string Lxdw { get; set; }  
        /// <summary>
        /// 联系人家庭电话
        /// </summary>
        [DataMember]
        public string Lxjtdh { get; set; }
        /// <summary>
        /// 联系人工作电话
        /// </summary>
        [DataMember]
        public string Lxgzdh { get; set; }
        /// <summary>
        /// 联系邮编
        /// </summary>
        [DataMember]
        public string Lxyb { get; set; }
        /// <summary>
        /// 联系人标志
        /// </summary>
        [DataMember]
        public int Lxrbz { get; set; }

    }
}
