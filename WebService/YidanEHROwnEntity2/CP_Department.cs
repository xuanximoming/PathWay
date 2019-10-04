using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
namespace Yidansoft.Service
{
    [DataContractAttribute()]
    public partial class CP_Department
    {
        #region 基元属性
        [DataMemberAttribute()]
        public global::System.String Ksdm
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
        public global::System.String Yydm
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Yjksdm
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Ejksdm
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.Int16 Kslb
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.Int16 Ksbz
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> Zryss
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> Zyyss
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> Zzyss
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> Hss
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> Cws
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public Nullable<global::System.Int32> Hdcws
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