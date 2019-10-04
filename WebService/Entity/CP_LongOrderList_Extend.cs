using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 长期医嘱扩展属性
    /// 创建人：fqw创建时间：20101125
    /// </summary>
    public partial class CP_LongOrderList
    {
        /// <summary>
        /// 开始日期Convert(varchar(100),CAST(CP_LongOrderList.Ksrq AS DATETIME),23) 
        /// </summary>
        [DataMember()]
        public String StartDate { get; set; }
        /// <summary>
        /// 开始时间Convert(varchar(100),CAST(CP_LongOrderList.Ksrq AS DATETIME),8) AS StartTime,
        /// </summary>
        [DataMember()]
        public String StartTime { get; set; }
        /// <summary>
        /// 录入医生代码(CP_Employee.Name)
        /// </summary>
        [DataMember()]
        public String LrysdmName { get; set; }
        /// <summary>
        /// 执行护士名字(CP_Employee.Name)
        /// </summary>
        [DataMember()]
        public String ZxczyName { get; set; }
        /// <summary>
        /// 停止日期   Convert(varchar(100),CAST(CP_LongOrderList.Tzrq AS DATETIME),23) AS EndDate ,
        /// </summary>
        [DataMember()]
        public String EndDate { get; set; }
        /// <summary>
        ///停止时间 Convert(varchar(100),CAST(Tzrq AS DATETIME),8) AS EndTime,
        /// </summary>
        [DataMember()]
        public String EndTime { get; set; }
        /// <summary>
        /// 停止医生名字(CP_Employee.Name)
        /// </summary>
        [DataMember()]
        public String tzysdmName { get; set; }


        /// <summary>
        /// 分组符号
        /// CASE  CP_LongOrder.Fzbz WHEN 3500 then ''  WHEN 3501 THEN '┓' WHEN 3509 then '┛' ELSE '┃' END AS FzbzSymbol
        /// </summary>
        [DataMember()]
        public String FzbzSymbol { get; set; }

    }
}