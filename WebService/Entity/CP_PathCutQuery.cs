using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service
{
    [DataContract()]
    public class CP_PathCutTotal 
    {
        [DataMember()]
        public List<CP_PathCutQuery> pathCutQuery
        {
            get;
            set;
        }

        [DataMember()]
        public List<IDictionary<String, Object>> hashObjList
        {
            get;
            set;
        }
    }

    [DataContract()]
    public class CP_PathCutQuery
    {
        /// <summary>
        /// 表示变异代码的属性
        /// </summary>
        [DataMember()]
        public String Bydm
        {
            get;
            set;
        }
        /// <summary>
        /// 表示变异名称的属性
        /// </summary>
        [DataMember()]
        public String Bymc
        {
            get;
            set;
        }
    }
}