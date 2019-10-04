using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service
{
    /// <summary>
    ///表示存放路径总结的类（5.6）
    /// </summary>
    [DataContract()]
    public class RW_PathSummary
    {
        private String m_message = "";
        /// <summary>
        /// 表示存放路径总结异常信息的属性
        /// </summary>
        [DataMember()]
        public String Message
        {
            get { return m_message; }
            set { m_message = value; }
        }
        /// <summary>
        /// 表示存放路径总结执行医嘱（长期和临时）的属性
        /// </summary>
        [DataMember()]
        public List<RW_PathSummaryOrder> PathSummaryOrderList
        {
            get;
            set;
        }
        /// <summary>
        /// 表示存放路径总结变异医嘱的属性
        /// </summary>
        [DataMember()]
        public List<RW_PathSummaryVariation> PathSummaryVariation
        {
            get;
            set;
        }
        /// <summary>
        /// 表示存放路径总结执行节点的属性
        /// </summary>
        [DataMember()]
        public List<RW_PathSummaryEnForce> PathSummaryEnForce
        {
            get;
            set;
        }
        /// <summary>
        /// 表示存放路径总结字典的属性
        /// </summary>
        [DataMember()]
        public List<RW_PathSummaryCategory> PathSummaryCategory
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 新版本路径总结实体
    /// </summary>
    [DataContract()]
    public class RW_PathSummary_new
    {
        /// <summary>
        /// 节点名称
        /// </summary>
        [DataMember()]
        public String JDMC
        {
            get
            {
                return m_JDMC;
            }
            set
            {
                m_JDMC = value;
            }
        }
        private String m_JDMC;
        

        /// <summary>
        /// 节点代码
        /// </summary>
        [DataMember()]
        public String JDDM
        {
            get
            {
                return m_JDDM;
            }
            set
            {
                m_JDDM = value;
            }
        }
        private String m_JDDM;

        /// <summary>
        /// 医嘱序号
        /// </summary>
        [DataMember()]
        public String YZXH
        {
            get
            {
                return m_YZXH;
            }
            set
            {
                m_YZXH = value;
            }
        }
        private String m_YZXH;

        /// <summary>
        /// 药品名称
        /// </summary>
        [DataMember()]
        public String YPMC
        {
            get
            {
                return m_YPMC;
            }
            set
            {
                m_YPMC = value;
            }
        }
        private String m_YPMC;

        /// <summary>
        /// 嘱托内容
        /// </summary>
        [DataMember()]
        public String Ztnr
        {
            get
            {
                return m_Ztnr;
            }
            set
            {
                m_Ztnr = value;
            }
        }
        private String m_Ztnr;

        /// <summary>
        /// 变异原因
        /// </summary>
        [DataMember()]
        public String BYYY
        {
            get
            {
                return m_BYYY;
            }
            set
            {
                m_BYYY = value;
            }
        }
        private String m_BYYY;

        /// <summary>
        /// 变异类型
        /// </summary>
        [DataMember()]
        public String Type
        {
            get
            {
                return m_Type;
            }
            set
            {
                m_Type = value;
            }
        }
        private String m_Type;

        /// <summary>
        /// 医嘱内容
        /// </summary>
        [DataMember()]
        public String YZNR
        {
            get
            {
                return m_YZNR;
            }
            set
            {
                m_YZNR = value;
            }
        }
        private String m_YZNR;

        /// <summary>
        /// 进入路径时间
        /// </summary>
        [DataMember()]
        public String JRSJ
        {
            get
            {
                return m_JRSJ;
            }
            set
            {
                m_JRSJ = value;
            }
        }
        private String m_JRSJ;

        

    }

    /// <summary>
    /// 表示存放路径总结执行医嘱（长期和临时）的类
    /// </summary>
    [DataContract()]
    public class RW_PathSummaryOrder
    {
        /// <summary>
        /// 表示医嘱类别（长期/临时）的属性
        /// </summary>
        [DataMember()]
        public String Yzbz
        {
            get;
            set;
        }
        /// <summary>
        /// 表示项目类别的属性
        /// </summary>
        [DataMember()]
        public String Xmlb
        {
            get;
            set;
        }
        /// <summary>
        /// 表示项目代码的属性
        /// </summary>
        [DataMember()]
        public String Ypdm
        {
            get;
            set;
        }
        /// <summary>
        /// 表示项目内容的属性
        /// </summary>
        [DataMember()]
        public String Ypmc
        {
            get;
            set;
        }
        /// <summary>
        /// 表示医嘱内容的属性
        /// </summary>
        [DataMember()]
        public String Yznr
        {
            get;
            set;
        }
        /// <summary>
        /// 表示路径节点的属性
        /// </summary>
        [DataMember()]
        public String ActivityId
        {
            get;
            set;
        }
        /// <summary>
        ///子节点
        /// </summary>
        [DataMember()]
        public String ActivityChildID
        {
            get;
            set;
        }
        /// <summary>
        /// 医嘱状态
        /// </summary>
        [DataMember()]
        public String Yzzt
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 表示存放路径总结变异医嘱的类
    /// </summary>
    [DataContract()]
    public class RW_PathSummaryVariation
    {
        /// <summary>
        /// 表示项目类别的属性
        /// </summary>
        [DataMember()]
        public String Xmlb
        {
            get;
            set;
        }
        /// <summary>
        /// 表示项目代码的属性
        /// </summary>
        [DataMember()]
        public String Ypdm
        {
            get;
            set;
        }
        /// <summary>
        /// 表示项目内容的属性
        /// </summary>
        [DataMember()]
        public String Ypmc
        {
            get;
            set;
        }
        /// <summary>
        /// 表示变异理由的属性
        /// </summary>
        [DataMember()]
        public String Bynr
        {
            get;
            set;
        }
        /// <summary>
        /// 表示路径节点的属性
        /// </summary>
        [DataMember()]
        public String PahtDetailId
        {
            get;
            set;
        }
        /// <summary>
        /// 子节点
        /// </summary>
        [DataMember()]
        public String Mxdm
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 表示存放路径总结节点执行的类(5.11)
    /// </summary>
    [DataContract()]
    public class RW_PathSummaryEnForce
    {
        /// <summary>
        /// 表示节点代码的属性
        /// </summary>
        [DataMember()]
        public String Jddm
        {
            get;
            set;
        }
        /// <summary>
        /// 表示节点名称的属性
        /// </summary>
        [DataMember()]
        public String Ljmc
        {
            get;
            set;
        }
        /// <summary>
        /// 表示节点执行顺序的属性
        /// </summary>
        [DataMember()]
        public String Zxsx
        {
            get;
            set;
        }
        /// <summary>
        /// 表示节点执行时间的属性
        /// </summary>
        [DataMember()]
        public String Jrsj
        {
            get;
            set;
        }
    }

    /// <summary>
    /// 字典
    /// </summary>
    [DataContract()]
    public class RW_PathSummaryCategory
    {
        [DataMember()]
        public String Mxbh
        {
            get;
            set;
        }
        [DataMember()]
        public String Name
        {
            get;
            set;
        }
        [DataMember()]
        public Int32 Cols
        {
            get;
            set;
        }
        [DataMember()]
        public Int32 Lb
        {
            get;
            set;
        }
    }
}