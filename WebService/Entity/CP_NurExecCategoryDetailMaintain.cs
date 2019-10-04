using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service
{
    /// <summary>
    ///表示存放路径总结的类（5,17）
    /// </summary>
    [DataContract()]
    public class CP_NurExecCategoryDetailMaintain
    {
        private String m_message = "";
        /// <summary>
        /// 表示存放护理维护的属性
        /// </summary>
        [DataMember()]
        public String Message
        {
            get { return m_message; }
            set { m_message = value; }
        }
        [DataMember()]
        public List<CP_NurExecCategoryDetailMaintainList> CP_NurExecCategoryDetailMaintainList
        {
            get;
            set;
        }
        
    }

    /// <summary>
    /// 表示存放路径总结列表的类（5,17）
    /// </summary>
    [DataContract()]
    public class CP_NurExecCategoryDetailMaintainList
    {
        /// <summary>
        /// 表示操作符的属性(selectList	查询类别表，select查询操作，insert插入操作，update更新操作，delete删除操作)
        /// </summary>
        [DataMember()]
        public String Operation
        {
            get;
            set;
        }
        /// <summary>
        /// 表示类别表序号的属性
        /// </summary>
        [DataMember()]
        public String Lbxh
        {
            get;
            set;
        }
        /// <summary>
        /// 表示明细表状态的属性(1有效，0无效)
        /// </summary>
        [DataMember()]
        public String Yxjl
        {
            get;
            set;
        }
        /// <summary>
        /// 表示明细表是否使用的属性（1使用中，0未使用）
        /// </summary>
        [DataMember()]
        public String Sfsy
        {
            get;
            set;
        }
        /// <summary>
        /// 表示明细表序号的属性
        /// </summary>
        [DataMember()]
        public String Mxxh
        {
            get;
            set;
        }
        /// <summary>
        /// 表示明细表名称的属性
        /// </summary>
        [DataMember()]
        public String MxxhName
        {
            get;
            set;
        }
        /// <summary>
        ///表示创建时间的属性
        /// </summary>
        [DataMember()]
        public String Create_Time
        {
            get;
            set;
        }
        /// <summary>
        /// 表示创建人的属性
        /// </summary>
        [DataMember()]
        public String Create_User
        {
            get;
            set;
        }
        /// <summary>
        ///表示取消时间的属性
        /// </summary>
        [DataMember()]
        public String Cancel_Time
        {
            get;
            set;
        }
        /// <summary>
        /// 表示取消人的属性
        /// </summary>
        [DataMember()]
        public String Cancel_User
        {
            get;
            set;
        }
        /// <summary>
        /// 表示类别表名称的属性
        /// </summary>
        [DataMember()]
        public String LbxhName
        {
            get;
            set;
        }
        /// <summary>
        /// 护理结果名称
        /// </summary>
        [DataMember()]
        public String JgName
        {
            get;
            set;
        }
    }
}