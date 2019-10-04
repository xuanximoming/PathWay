using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Data.SqlClient;
using System.Data;
using System.Text;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    //D:\YidanSoft.Net\EHR\YidanEHRApplication\Yidansoft.Service\YidanEHROwnEntity\CP_QueryPathExecute.cs
     [DataContract()]
    public partial class V_PermissionList
    {
        [DataMember()]
        public string UserID { get; set; }
        [DataMember()]
        public string RoleCode { get; set; }
        [DataMember()]
        public string RoleName { get; set; }
        [DataMember()]

        public string FunCodeFather { get; set; }
        [DataMember()]
        public string FunFatherName { get; set; }
        [DataMember()]
        public string FunCode { get; set; }
        [DataMember()]
        public string FunName { get; set; }
        [DataMember()]
        public string FunURL { get; set; }

    
          
    }
    public partial class V_PermissionListFather
    {
        public string FunCodeFather { get; set; }

        public string FunFatherName { get; set; }

        public string FunURLFather { get; set; }
       public   List<V_PermissionList> pList = new List<V_PermissionList>();
    }

}