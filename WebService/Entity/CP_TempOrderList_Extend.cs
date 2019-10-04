using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    public partial class CP_TempOrderList
    {
        /// <summary>
        /// 开始日期Convert(varchar(100),CAST(Ksrq AS DATETIME),23) AS StartDate ,
        /// </summary>
        [DataMember()]
        public string StartDate { get; set; }
        /// <summary>
        /// 开始时间 Convert(varchar(100),CAST(Ksrq AS DATETIME),8) AS StartTime,
        /// </summary>
        [DataMember()]
        public string StartTime { get; set; }
        /// <summary>
        /// 医生签名
        /// </summary>
        [DataMember()]
        public string LrysdmName { get; set; }
        /// <summary>
        /// 分组符号 CASE  CP_TempOrder.Fzbz WHEN 3500 then ''  WHEN 3501 THEN '┓' WHEN 3509 then '┛' ELSE '┃' END AS FzbzSymbol,
        /// </summary>
        [DataMember()]
        public string FzbzSymbol { get; set; }
        /// <summary>
        /// 执行日期   Convert(varchar(100),CAST(Zxrq AS DATETIME),23) AS exeDate ,
        /// </summary>
        [DataMember()]
        public string exeDate { get; set; }
        /// <summary>
        /// 执行时间 Convert(varchar(100),CAST(Zxrq AS DATETIME),8) AS exeTime,
        /// </summary>
        [DataMember()]
        public string exeTime { get; set; }
     
        /// <summary>
        /// 执行护士
        /// </summary>
        [DataMember()]
        public string ZxczyName { get; set; }
      
    }
}