using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract()]
    public class RPT_PathVariationDetail
    {
        //姓名
        private string name;
        //路径名称
        private string pathname;
        //诊断
        private string diagnosis;
        //床位医师
        private string doctor;
        //科室
        private string dept;
        //病区
        private string ward;
        //变异内容
        private string variationdetail;
        //变异原因
        private string variationreason;
        //变异时间
        private string variationtime;
        //变异节点
        private string variationid;


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
        /// 路径变异内容
        /// </summary>
        [DataMember()]
        public string VariationDetail
        {
            get { return variationdetail; }
            set { variationdetail = value; }
        }
        /// <summary>
        /// 路径变异原因
        /// </summary>
        [DataMember()]
        public string VariationReason
        {
            get { return variationreason; }
            set { variationreason = value; }
        }

        /// <summary>
        /// 路径变异时间
        /// </summary>
        [DataMember()]
        public string VariationTime
        {
            get { return variationtime; }
            set { variationtime = value; }
        }
        /// <summary>
        /// 路径变异节点
        /// </summary>
        [DataMember()]
        public string VariationId
        {
            get { return variationid; }
            set { variationid = value; }
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