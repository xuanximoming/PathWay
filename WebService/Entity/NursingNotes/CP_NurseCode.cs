using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.NursingNotes
{
    /// <summary>
    /// 护理记录单编码实体
    /// </summary>
    [DataContract]
    public class CP_NurseCode
    {
        /// <summary>
        /// 编码ID
        /// </summary>
        [DataMember]
        public int CodeID{get;set;}
        
        /// <summary>
        /// 编码名称
        /// </summary>
        [DataMember]
        public string CodeName { get; set; }

    }

    /// <summary>
    /// 护理记录单编码实体集合
    /// </summary>
    [DataContract]
    public class CP_NurseCodeCollection
    {
        private List<CP_NurseCode> m_NurseCodeCollection = new List<CP_NurseCode>();

        /// <summary>
        /// 护理记录单编码实体
        /// </summary>
        [DataMember]
        public List<CP_NurseCode> NurseCodeCollection
        {
            get{ return m_NurseCodeCollection;}
            set { m_NurseCodeCollection = value;}
        }
    }
}