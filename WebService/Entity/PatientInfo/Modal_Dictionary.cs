using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract]
    public partial class Modal_Dictionary
    {
        [DataMember]
        public string Lbdm { get; set; }

        [DataMember]
        public string Mxdm { get; set; }
        
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Py { get; set; }

        [DataMember]
        public string Wb { get; set; }

    }
}