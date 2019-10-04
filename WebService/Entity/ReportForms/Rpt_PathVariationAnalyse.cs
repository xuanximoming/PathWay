using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service
{
    /// <summary>
    /// 表示存放变异情况分析数据源的类
    /// </summary>
    [DataContract()]
    public class Rpt_PathVariationAnalyse
    {
        private String m_message = "";
        /// <summary>
        /// 表示存放变异情况分析异常信息的属性
        /// </summary>
        [DataMember()]
        public String Message
        {
            get { return m_message; }
            set { m_message = value; }
        }

        /// <summary>
        /// 表示存放变异情况分析列表的属性
        /// </summary>
        [DataMember()]
        public List<Rpt_PathVariationAnalyseList> PathVariationAnalyseList
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 表示存放变异情况分析列表的类
    /// </summary>
    [DataContract()]
    public class Rpt_PathVariationAnalyseList 
    {
        /// <summary>
        /// 表示变异编码属性
        /// </summary>
        [DataMember()]
        public String Bydm
        {
            get;
            set;
        }
        /// <summary>
        /// 表示一级编码名称的属性
        /// </summary>
        [DataMember()]
        public String BymcSt
        {
            get;
            set;
        }
        /// <summary>
        /// 表示二级编码名称的属性
        /// </summary>
        [DataMember()]
        public String BymcNd 
        {
            get;
            set;
        }
        /// <summary>
        /// 表示三级编码名称的属性
        /// </summary>
        [DataMember()]
        public String BymcRd
        {
            get;
            set;
        }
        /// <summary>
        /// 表示数目的属性
        /// </summary>
        [DataMember()]
        public Int32 VariationCount
        {
            get;
            set;
        }
    }
}