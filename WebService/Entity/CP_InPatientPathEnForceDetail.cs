using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 路径执行明细表
    /// </summary>
    [DataContract]
    public class CP_InPatientPathEnForceDetail
    {
        [DataMember()]
        public String ID { get; set; }//自增
        [DataMember()]
        public String Syxh { get; set; }//首页序号
        [DataMember()]
        public String Ljxh { get; set; }//路径序号
        [DataMember()]
        public String Ljdm { get; set; }//路径代码
        [DataMember()]
        public String Jddm { get; set; }//节点代码
        [DataMember()]
        public String Zxsx { get; set; }//执行顺序(编号有大到小排序)

        //  ID numeric(9, 0) IDENTITY,		
        //Syxh numeric(9, 0) NOT NULL,	
        //Ljxh numeric(9, 0) NOT NULL,	
        //Ljdm varchar(12) NOT NULL,		
        //Jddm varchar(50) NOT NULL,		
        //Zxsx numeric(9,0) NOT NULL		
    }
}