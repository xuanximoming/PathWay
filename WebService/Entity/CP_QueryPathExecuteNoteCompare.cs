using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    //D:\YidanSoft.Net\EHR\YidanEHRApplication\Yidansoft.Service\YidanEHROwnEntity\CP_QueryPathExecute.cs
     [DataContract()]
    public class CP_QueryPathExecuteNoteCompare
    {
         [DataMember()]
        public String YzbzName{get;set;}
         [DataMember()]
         public String Ypmc { get; set; }
         [DataMember()]
         public String Yznr { get; set; }
         [DataMember()]
         public String Ztnr { get; set; }
         [DataMember()]
         public String Flag { get; set; }
         [DataMember()]
         public String IsNew { get; set; }
         [DataMember()]
         public String EmployeeName { get; set; }
         [DataMember()]
         public String EmployeeLrrq { get; set; }
         [DataMember()]
        public String Zxrq { get; set; }
        

        //public CP_QueryPathExecute(Decimal Ctmxxh, Decimal Ctyzxh, short Yzbz,decimal Fzxh, short Fzbz, Decimal Cdxh, Decimal Ggxh, Decimal Lcxh, String Ypdm, String Ypmc, short Xmlb,
        //     String Zxdw, Decimal Ypjl, String Jldw, Decimal Dwxs, short Dwlb, String Yfdm, String Pcdm, Int32 Zxcs, int Zxzq, short Zxzqdw, String Zdm, String Zxsj,
        //     int Zxts, Decimal Ypzsl, String Ztnr, short Yzlb, string YzbzName,string Flag,int Index,string Yznr)
        //{




    }
}