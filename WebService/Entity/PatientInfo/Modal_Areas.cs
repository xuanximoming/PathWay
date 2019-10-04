using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract]
    public partial class Modal_Areas
    {
        [DataMember]
        public string Dqlb { get; set; }

        [DataMember]
        public string Dqdm { get; set; }

        //[DataMember]
        //public string Name { get; set; }

        public String _name;
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember()]
        public string Name
        {
            set { _name = value; }
            get { return _name + "【" + Dqdm + "】"; }
        }

        [DataMember]
        public string Py { get; set; }

        [DataMember]
        public string Wb { get; set; }
    }
}