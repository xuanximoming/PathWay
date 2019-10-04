using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service
{
    [DataContract()]
    public class CVersion
    {

        [DataMember()]
        public String ID
        {
            get;
            set;
        }
        [DataMember()]
        public String VersionID
        {
            get;
            set;
        }
        [DataMember()]
        public String Version
        {
            get;
            set;
        }
        [DataMember()]
        public String Create_time
        {
            get;
            set;
        }
        [DataMember()]
        public String HosName
        {
            get;
            set;
        }
    }
}