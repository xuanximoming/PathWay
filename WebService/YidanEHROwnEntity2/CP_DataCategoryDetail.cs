using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
namespace Yidansoft.Service 
{
    using System.Runtime.Serialization;
    [DataContractAttribute()]
    public partial class CP_DataCategoryDetail 
    {
        #region 基元属性
        [DataMemberAttribute()]
        public global::System.Int16 Mxbh
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
        private global::System.String _Name;
        partial void OnNameChanging(global::System.String value);
        partial void OnNameChanged();
        /// <summary>
        /// 没有元数据文档可用。
        /// </summary>
        [DataMemberAttribute()]
        public global::System.Int16 Lbbh
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
        private global::System.String _Py;
        partial void OnPyChanging(global::System.String value);
        partial void OnPyChanged();
        [DataMemberAttribute()]
        public global::System.String Wb
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