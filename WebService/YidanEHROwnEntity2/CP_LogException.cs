using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
namespace Yidansoft.Service 
{
    [DataContractAttribute( )]
    public partial class CP_LogException  
    {
        #region 基元属性
        [DataMemberAttribute()]
        public global::System.Int32 ID
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Messages
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Module_Name
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String HostName
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String MACADDR
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Client_ip
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.DateTime Create_time
        {
            get;
            set;
        }
        [DataMemberAttribute()]
        public global::System.String Create_user
        {
            get;
            set;
        }
        #endregion
    }
}