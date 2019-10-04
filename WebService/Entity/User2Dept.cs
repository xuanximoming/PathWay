using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace WebService.Entity
{
    /// <summary>
    ///  
    /// </summary>
    [DataContract]
    public class User2Dept
    {

        public User2Dept() { }

        public User2Dept(string userId, string deptId, string wardId)
        {
            this.userId = userId;
            this.deptId = deptId;
            this.wardId = wardId;
        }


        private string userId = string.Empty;              //not null   length: 6
        private string deptId = string.Empty;              //not null   length: 12
        private string wardId = string.Empty;              //not null   length: 12
        private string deptName = string.Empty;              //not null   length: 12
        private string wardName = string.Empty;              //not null   length: 12





        //=============================================================================================


        /// <summary>
        ///  not null    length: 6
        ///  用户编号
        /// </summary>
        [DataMember()]
        public string UserId { get { return userId; } set { userId = value; } }


        /// <summary>
        ///  not null    length: 12
        ///  对应科室ID
        /// </summary>
        [DataMember()]
        public string DeptId { get { return deptId; } set { deptId = value; } }

        /// <summary>
        ///  not null    length: 12
        ///  对应科室名称
        /// </summary>
        [DataMember()]
        public string DeptName { get { return deptName; } set { deptName = value; } }


        /// <summary>
        ///  not null    length: 12
        ///  对应病区ID
        /// </summary>
        [DataMember()]
        public string WardId { get { return wardId; } set { wardId = value; } }

        /// <summary>
        ///  not null    length: 12
        ///  对应病区Name
        /// </summary>
        [DataMember()]
        public string WardName { get { return wardName; } set { wardName = value; } }

        /// <summary>
        /// 是否有当前科室
        /// </summary>
        [DataMember()]
        public int IsCheck { get; set; }
    }
}