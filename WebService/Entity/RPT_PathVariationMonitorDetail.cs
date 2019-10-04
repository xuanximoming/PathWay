using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract()]
    public class RPT_PathVariationMonitorDetail
    {
        //节点状态
        private string status;
        //姓名
        private string name;
        //科室
        private string dept;
        //病区
        private string ward;
        //诊断
        private string diagnosis;
        //床位医师
        private string doctor;
        //路径名称
        private string pathname;
        //节点名称
        private string variationname;


        /// <summary>
        /// 节点状态
        /// </summary>
        [DataMember()]
        public string Status
        {
            get { return status; }
            set { status = value; }
        }
        /// <summary>
        /// 姓名
        /// </summary>
        [DataMember()]
        public string Name
        {
            get { return name; }
            set { name = value; }
        }
        /// <summary>
        /// 路径名称
        /// </summary>
        [DataMember()]
        public string PathName
        {
            get { return pathname; }
            set { pathname = value; }
        }

        /// <summary>
        /// 诊断
        /// </summary>
        [DataMember()]
        public string Diagnosis
        {
            get { return diagnosis; }
            set { diagnosis = value; }
        }
        /// <summary>
        /// 科室
        /// </summary>
        [DataMember]
        public string Dept
        {
            get { return dept; }
            set { dept = value; }
        }
        /// <summary>
        /// 病区
        /// </summary>
        [DataMember]
        public string Ward
        {
            get { return ward; }
            set { ward = value; }
        }
        /// <summary>
        /// 医生
        /// </summary>
        [DataMember]
        public string Doctor
        {
            get { return doctor; }
            set { doctor = value; }
        }
        /// <summary>
        /// 路径变异节点
        /// </summary>
        [DataMember()]
        public string Variationname
        {
            get { return variationname; }
            set { variationname = value; }
        }


    }
    ///// <summary>
    ///// 定义一个List中放入string类
    ///// </summary>
    //[DataContract()]
    //public class ListString
    //{
    //    private List<string> m_list = new List<string>();

    //    [DataMember()]
    //    public List<string> list { get { return m_list; } set { m_list = value; } }
    //}

}