using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract]
    public class CP_MasterDrugs
    {
        /// <summary>
        /// --药品代码	//CP_PlaceOfDrug. Cdxh
        /// </summary>
        [DataMember()]
        public String Cdxh { get; set; }
        /// <summary>
        /// 	--提醒方式（1：提醒，2：输入帐号密码）
        /// </summary>
        [DataMember()]
        public String Txfs { get; set; }

        /// <summary>
        /// 	--提醒方式（1：提醒，2：输入帐号密码）
        /// </summary>
        [DataMember()]
        public String TxfsName
        {
            get
            {
                if (Txfs == "2") return "输入帐号密码";
                else return "提醒";

            }
            set { }
        }
        /// <summary>
        /// --创建者编码(CP_Employee.Zgdm)
        /// </summary>
        [DataMember()]
        public String ZgdmCj { get; set; }

        /// <summary>
        /// --创建者编码(CP_Employee.Zgdm)
        /// </summary>
        [DataMember()]
        public String ZgdmCjName { get; set; }
        /// <summary>
        /// --创建时间
        /// </summary>
        [DataMember()]
        public String Cjsj { get; set; }
        /// <summary>
        /// --修改者编码(CP_Employee.Zgdm)
        /// </summary>
        [DataMember()]
        public String ZgdmXg { get; set; }
        /// <summary>
        /// --修改时间
        /// </summary> 
        [DataMember()]
        public String Xgsj { get; set; }
        /// <summary>
        /// --备注
        /// </summary>
        [DataMember()]
        public String Bz { get; set; }


        /// <summary>
        /// --药品规格
        /// </summary>
        [DataMember()]
        public String Ypgg { get; set; }
        /// <summary>
        /// --药品生产厂家名称
        /// </summary>
        [DataMember()]
        public String Cjmc { get; set; }
        /// <summary>
        /// --零售价
        /// </summary>
        [DataMember()]
        public String Lsj { get; set; }



        /// <summary>
        /// --药品名称
        /// </summary>
        [DataMember()]
        public String Ypmc { get; set; }



        /// <summary>
        /// --药品名称
        /// </summary>
        [DataMember()]
        public String IsCheck { get; set; }


        /// <summary>
        /// --药品名称
        /// </summary>
        [DataMember()]
        public String Jsbm { get; set; }


        /// <summary>
        /// --是否授权
        /// </summary>
        [DataMember()]
        public Boolean IsPass { get; set; }


        /// <summary>
        /// --是否需要授权或提示
        /// </summary>
        [DataMember()]
        public Boolean IsNeedPass { get; set; }

    }
}