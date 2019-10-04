using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    public partial class CP_NurExecRecord
    {
        [DataMember()]
        public String Id { get; set; }
        public String Mxxh { get; set; }
    }
}