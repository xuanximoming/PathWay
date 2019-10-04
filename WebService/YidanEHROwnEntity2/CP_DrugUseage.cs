using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service 
{
    [DataContractAttribute( )]
    public partial class CP_DrugUseage 
    {
       
        #region 基元属性


        [DataMemberAttribute()]
        public global::System.String Yfdm
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
        public global::System.Int16 Ctsy
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.Int16 Zdfz
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.Int16 Yflb
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