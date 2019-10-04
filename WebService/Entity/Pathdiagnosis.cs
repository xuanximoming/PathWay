using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service
{
    [DataContract()]
    public class Pathdiagnosis
    {
       

            /// <summary>
        /// 路径代码
        /// </summary>
        [DataMember()]
        public string ljdm { get; set; }
        /// <summary>
        /// cid编码
        /// </summary>
        [DataMember()]
        public string cid { get; set; }
        /// <summary>
        /// 名称
        /// </summary>

        [DataMember()]
        public string name { get; set; }
    }
}