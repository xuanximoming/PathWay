using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service 
{
    [DataContractAttribute( )]
    public partial class CP_ChargingMinItem  
    {
      
        #region 基元属性

        [DataMemberAttribute()]
        public global::System.String Sfxmdm
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
        public global::System.String Dxdm
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.String Xmdw
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.String Xmgg
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.Decimal Xmdj
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.Int16 Mzbxbz
        {
            get;
            set;
        }


        [DataMemberAttribute()]
        public global::System.Int16 Zybxbz
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.Int16 Xmlb
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.Int16 Xtbz
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.Int16 Xmxz
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.Int16 Xskz
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.String Fjxx
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public Nullable<global::System.Int16> Syfw
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