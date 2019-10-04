using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
namespace Yidansoft.Service 
{
    [DataContractAttribute( )]
    public partial class CP_VariantRecords 
    {
        #region 基元属性
        [DataMemberAttribute()]
        public global::System.Int32 Id
        {
            get;
            set;
        }
        private global::System.Int32 _Id;
        partial void OnIdChanging(global::System.Int32 value);
        partial void OnIdChanged();
        [DataMemberAttribute()]
        public global::System.Decimal Syxh
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
        public global::System.String Mxdm
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Ypdm
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Bylb
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Bylx
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Bynr
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Bydm
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Byyy
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Bysj
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.Decimal Ljxh
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String xmlb
        {
            get;
            set;
        }
        #endregion
    }
}