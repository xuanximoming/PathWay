using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service 
{
    [DataContractAttribute( )]
    public partial class CP_PathVariation  
    {
      
        #region 基元属性


        [DataMemberAttribute()]
        public global::System.String Bydm
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.String Bymc
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.String Byms
        {
            get;
            set;
        }

        [DataMemberAttribute()]
        public global::System.Int32 Yxjl
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
     
        #endregion

    }
}