using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service
{
    /// <summary>
    /// 表示存放路径监测表数据源的类
    /// </summary>
    [DataContract()]
    public class Rpt_PathVariationMonitor
    {
        private String m_message = "";
        /// <summary>
        /// 表示存放路径监测异常信息的属性
        /// </summary>
        [DataMember()]
        public String Message
        {
            get { return m_message; }
            set { m_message = value; }
        }

        /// <summary>
        /// 表示存放路径监测列表的属性
        /// </summary>
        [DataMember()]
        public List<Rpt_PathVariationMonitorList> PathVariationMonitorList
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 表示存放路径监测列表列表的类
    /// </summary>
    [DataContract()]
    public class Rpt_PathVariationMonitorList
    {

        /// <summary>
        /// 表示路径节点名的属性
        /// </summary>
        [DataMember()]
        public String Ljmc
        {
            get;
            set;
        }
        /// <summary>
        /// 表示路径节点ID的属性
        /// </summary>
        [DataMember()]
        public String Pahtdetail
        {
            get;
            set;
        }
        /// <summary>
        /// 表示执行数的属性
        /// </summary>
        [DataMember()]
        public Int32 EnForceCount
        {
            get;
            set;
        }
        /// <summary>
        /// 表示变异数的属性
        /// </summary>
        [DataMember()]
        public Int32 Variationcount
        {
            get;
            set;
        }
        /// <summary>
        /// 表示差异率
        /// </summary>
        [DataMember()]
        public Int32 Per
        {
            get;
            set;
        }

    }
}