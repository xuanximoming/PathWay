using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract]
    public class DrugInfoPass
    {
        /// <summary>
        /// ctor
        /// </summary>
        public DrugInfoPass()
        {

        }

        [DataMember()]
        public string Ypdm { get; set; }

        [DataMember()]
        public string Ypspm { get; set; }
        [DataMember()]
        public string Yptym { get; set; }
        [DataMember()]
        public string Ypsyz { get; set; }
        [DataMember()]
        public string Ypjjz { get; set; }
        [DataMember()]
        public string Ypyf { get; set; }
        [DataMember()]
        public string Gflb { get; set; }
        [DataMember()]
        public string Yblb { get; set; }
        [DataMember()]
        public string Ypdl { get; set; }
        [DataMember()]
        public string Ypgg { get; set; }
        [DataMember()]
        public string Py { get; set; }




    }
}