using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
namespace Yidansoft.Service 
{
    [DataContractAttribute( )]
    public partial class CP_Surgery  
    {
        #region 基元属性
        [DataMemberAttribute()]
        public global::System.String Ssdm
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Ysdm
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Bzdm
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Name
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Py
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Wb
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Bzlb
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public Nullable<global::System.Int16> Sslb
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.Int16 Yxjl
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Memo
        {
            get;
            set;
        }
        #endregion
    }
}