using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service 
{
    [DataContractAttribute( )]
    public partial class CP_NurExecToPath 
    {
       
        #region 基元属性

        [DataMemberAttribute()]
        public global::System.Decimal Id
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
        public global::System.String PathDetailId
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.String Mxxh
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
        public global::System.String Create_Time
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.String Create_User
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.String Cancel_Time
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.String Cancel_User
        {
            get;
            set;
        }
      
        #endregion

    }
}