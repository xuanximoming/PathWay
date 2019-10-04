using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 用于在前台调用后台执行数据库信息，在后台返回到前台的数据库执行情况
    /// </summary>
    [DataContract]
    public class SQLMessage
    {
        /// <summary>
        /// 执行是否成功   true：成功   false:失败
        /// </summary>
        [DataMember]
        public bool IsSucceed { get; set; }

        /// <summary>
        /// 执行SQL后返回的信息
        /// </summary>
        [DataMember]
        public string Message { get; set; }
    }
}