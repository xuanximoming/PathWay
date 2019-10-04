using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract]
    public class CP_MasterDrug2Role
    {
        /// <summary>
        ///  	--角色编码（DrugRoles. Jsbm）
        /// </summary>
        [DataMember()]
        public String Jsbm { get; set; }// 
        /// <summary>
        ///  	--药品代码	//CP_PlaceOfDrug. Cdxh
        /// </summary>
        [DataMember()]
        public String Cdxh { get; set; }// 
        /// <summary>
        ///   --创建者编码(CP_Employee.Zgdm)
        /// </summary>
        [DataMember()]
        public String ZgdmCj { get; set; }//
        /// <summary>
        ///  --创建时间
        /// </summary>
        [DataMember()]
        public String Cjsj { get; set; }// 		
        /// <summary>
        ///  	--修改者编码(CP_Employee.Zgdm)
        /// </summary>
        [DataMember()]
        public String ZgdmXg { get; set; }// 
        /// <summary>
        ///  --修改时间
        /// </summary>
        [DataMember()]
        public String Xgsj { get; set; }// 		
        /// <summary>
        ///  备注
        /// </summary>
        [DataMember()]
        public String Bz { get; set; }// 

        /// <summary>
        /// --角色名称
        /// </summary> 
        [DataMember()]
        public String Jsmc { get; set; }
    }
}