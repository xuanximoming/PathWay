using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service
{
    /// <summary>
    /// 查询路径所有原生医嘱
    /// </summary>
    [DataContract()]
    public class CP_PathOrdersAll
    {
        /// <summary>
        /// 成套医嘱序号
        /// </summary>
        [DataMember()]
        public string ctyzxh { get; set; }
        /// <summary>
        /// 医嘱名称
        /// </summary>
        [DataMember()]
        public string ypmc { get; set; }
        /// <summary>
        /// 单位
        /// </summary>

        [DataMember()]
        public string zxdw { get; set; }
        /// <summary>
        /// 计量
        /// </summary>
        [DataMember()]
        public string ypjl { get; set; }
        /// <summary>
        /// 路径节点ID
        /// </summary>
        [DataMember()]
        public string pahtdetailID { get; set; }
        /// <summary>
        /// 节点名称
        /// </summary>

        [DataMember()]
        public string Name { get; set; }
        /// <summary>
        /// 医嘱类别
        /// </summary>
        [DataMember()]
        public string orderstype { get; set; }
        /// <summary>
        /// 牵制节点
        /// </summary>
        [DataMember()]
        public string prepahtdetailid { get; set; }
        /// <summary>
        /// 后置节点
        /// </summary>
        [DataMember()]
        public string nextpahtdetailid { get; set; }
         /// <summary>
        /// 计量单位
        /// </summary>
        [DataMember()]
        public string jldw { get; set; }
         /// <summary>
        /// 执行次数
        /// </summary>
        [DataMember()]
        public string zxcs { get; set; }
         /// <summary>
        /// 医嘱内容
        /// </summary>
        [DataMember()]
        public string ztnr { get; set; }
        /// <summary>
        /// 路径诊断
        /// </summary>
          [DataMember()]
        public List<Pathdiagnosis> pathdiagnosis { get; set; }
    }
}