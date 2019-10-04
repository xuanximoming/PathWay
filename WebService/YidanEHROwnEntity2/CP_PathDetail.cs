using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
namespace Yidansoft.Service 
{
    [DataContractAttribute( )]
    public partial class CP_PathDetail  
    {
        #region 基元属性
        [DataMemberAttribute()]
        public global::System.String PahtDetailID
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Ljdm
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> Ts
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Ljmc
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String LJs
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Bqms
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> Zljh
        {
            get;
            set;
        }
        /// <summary>
        /// 前置节点UniqueID
        /// </summary>
        [DataMemberAttribute()]
        public global::System.String PrePahtDetailID
        {
            get;
            set;
        }
        /// <summary>
        /// 后置节点UniqueID
        /// </summary>
        [DataMemberAttribute()]
        public global::System.String NextPahtDetailID
        {
            get;
            set;
        }
        /// <summary>
        /// 节点类型
        /// </summary>
        [DataMemberAttribute()]
        public global::System.String ActivityType
        {
            get;
            set;
        }
        #endregion
    }
}