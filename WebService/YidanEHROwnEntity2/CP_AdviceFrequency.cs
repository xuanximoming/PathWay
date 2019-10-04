using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
namespace Yidansoft.Service 
{
    [DataContractAttribute( )]
    public partial class CP_AdviceFrequency 
    {
        #region 基元属性
        [DataMemberAttribute()]
        public global::System.String Pcdm
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
        public global::System.Int32 Zxcs
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.Int32 Zxzq
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.Int16 Zxzqdw
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Zdm
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Zxsj
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public Nullable<global::System.Int16> zbbz
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public Nullable<global::System.Int16> Yzglbz
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