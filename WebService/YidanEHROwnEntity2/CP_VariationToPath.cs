using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
namespace Yidansoft.Service 
{
    [DataContractAttribute()]
    public partial class CP_VariationToPath 
    {
        #region 基元属性
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
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
        public global::System.String ActivityId
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
        public global::System.Int32 Yxjl
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