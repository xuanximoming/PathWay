using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    //D:\YidanSoft.Net\EHR\YidanEHRApplication\Yidansoft.Service\YidanEHROwnEntity\CP_QueryPathExecute.cs
     [DataContract()]
    public class CP_QueryPathExecute
    {
         [DataMember()]
        public Decimal Syxh{get;set;} 
         [DataMember()]
        public string Brxb{get;set;} 
         [DataMember()]
        public string BrxbName{get;set;}
         [DataMember()]
         public string Xsnl { get; set; }
         [DataMember()]
         public string Ryzd { get; set; }
         [DataMember()]
         public string RyzdName { get; set; }
         [DataMember()]
         public string Ryrq { get; set; }
         [DataMember()]
         public string Hzxm { get; set; }
         [DataMember()]
         public string Jrsj { get; set; }
         [DataMember()]
         public string LjztName { get; set; }
         [DataMember()]
         public string Ljzt { get; set; }
         [DataMember()]
         public string Tcsj { get; set; }
         [DataMember()]
         public string Ljts { get; set; }
         [DataMember()]
         public string Ljdm { get; set; }
         [DataMember()]
         public string PathName { get; set; }
         [DataMember()]
         public string Wcsj { get; set; }
         [DataMember()]
         public string WorkFlowXML { get; set; }
         [DataMember()]
        public String EnFroceXml { get; set; }
       

    }
}