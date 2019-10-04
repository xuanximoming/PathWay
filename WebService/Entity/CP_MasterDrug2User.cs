using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract]
    public class CP_MasterDrug2User
    {
 


        /// <summary>
        /// //	--职工代码(CP_Employee.Zgdm)
        /// </summary>
        [DataMember()]
        public String Zgdm { get; set; }
        /// <summary>
        /// //  --角色编码(MasterDrugRoles. Jsbm)
        /// </summary>
        [DataMember()]
        public String Jsbm { get; set; }
        /// <summary>
        /// //  --创建者编码(CP_Employee.Zgdm)
        /// </summary>
        [DataMember()]
        public String ZgdmCj { get; set; }
        /// <summary>
        /// //  --创建时间
        /// </summary>
        [DataMember()]
        public String Cjsj { get; set; }
        /// <summary>
        /// //  --修改者编码(CP_Employee.Zgdm)
        /// </summary>
        [DataMember()]
        public String ZgdmXg { get; set; }
        /// <summary>
        /// //  --修改时间
        /// </summary>
        [DataMember()]
        public String Xgsj { get; set; }
        /// <summary>
        /// //  --备注
        /// </summary>
        [DataMember()]
        public String Bz { get; set; }
                /// <summary>
        /// --角色名称
        /// </summary> 
        [DataMember()]
        public String Jsmc { get; set; }

        /// <summary>
        /// 员工名字
        /// </summary>
        [DataMember()]
        public String Name{ get; set; }
            
        /// <summary>
        /// 员工性别
        /// </summary>
         [DataMember()]
        public String Zgxb { get; set; }


    }
}