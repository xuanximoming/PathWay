using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 用户角色
    /// </summary>
    [DataContract()]
    public class PE_CompleteUser
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        [DataMember()]
        public string UserID { get; set; }
        /// <summary>
        /// 用户姓名
        /// </summary>
        [DataMember()]
        public string UserName { get; set; }
        /// <summary>
        /// 是否已婚
        /// </summary>
        [DataMember()]
        public string Marital { get; set; }

        /// <summary>
        /// 病区
        /// </summary>
        [DataMember()]
        public string Ward { get; set; }

        /// <summary>
        /// 医生级别
        /// </summary>
        [DataMember()]
        public string DocGrade { get; set; }

        /// <summary>
        /// 身份证号
        /// </summary>
        [DataMember()]
        public string IDNo { get; set; }
        /// <summary>
        /// 用户科室
        /// </summary>
        [DataMember()]
        public string UserDept { get; set; }
        /// <summary>
        /// 拼音
        /// </summary>
        [DataMember()]
        public string Py { get; set; }
        /// <summary>
        /// 五笔
        /// </summary>
        [DataMember()]
        public string Wb { get; set; }
        /// <summary>
        /// 性别
        /// </summary>
        [DataMember()]
        public string Sexy { get; set; }
        /// <summary>
        /// 生日
        /// </summary>
        [DataMember()]
        public string Birth { get; set; }

        public override string ToString()
        {
            return UserName;
        }
    }
}