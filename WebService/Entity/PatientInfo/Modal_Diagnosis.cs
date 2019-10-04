using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract]
    public partial class Modal_Diagnosis
    {
        [DataMember]
        public string Zddm { get; set; }

        public String _name;
        /// <summary>
        /// 疾病名称
        /// </summary>
        [DataMember()]
        public string Name
        {
            set { _name = value; }
            get { return _name + "【" + Zdbs + "】"; }
        }

        [DataMember]
        public string Py { get; set; }

        [DataMember]
        public string Wb { get; set; }
        [DataMember]
        public string Zdbs { get; set; }
    }
}