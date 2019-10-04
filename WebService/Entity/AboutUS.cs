using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service
{
    [DataContract()]
    public class AboutUS
    {
        
        [DataMember()]
        public String Edit
        {
            get;
            set;
        }
        [DataMember()]
        public String  Names
        {
            get;
            set;
        }
        [DataMember()]
        public String Company
        {
            get;
            set;
        }
        [DataMember()]
        public String Times
        {
            get;
            set;
        }
        [DataMember()]
        public String Warning
        {
            get;
            set;
        }
    }
}