using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract]
    public class RPT_PathStatisticDetail
    {
        //患者姓名
        private string patientName;
        //性别
        private string sex;
        //诊断
        private string diagnosis;
        //科室
        private string dept;
        //病区
        private string ward;
        //床位医师
        private string doctor;
        //路径名称
        private string pathName;
        //入径时间
        private string inPathTime;
        //路径状态
        private string pathStatus;


        [DataMember]
        public string PatientName
        {
            get { return patientName; }
            set { patientName = value; }
        }

        [DataMember]
        public string Sex
        {
            get { return sex; }
            set { sex = value; }
        }

        [DataMember]
        public string Diagnosis
        {
            get { return diagnosis; }
            set { diagnosis = value; }
        }

        [DataMember]
        public string Dept
        {
            get { return dept; }
            set { dept = value; }
        }

        [DataMember]
        public string Ward
        {
            get { return ward; }
            set { ward = value; }
        }

        [DataMember]
        public string Doctor
        {
            get { return doctor; }
            set { doctor = value; }
        }

        [DataMember]
        public string PathName
        {
            get { return pathName; }
            set { pathName = value; }
        }

        [DataMember]
        public string InPathTime
        {
            get { return inPathTime; }
            set { inPathTime = value; }
        }

        [DataMember]
        public string PathStatus
        {
            get { return pathStatus; }
            set { pathStatus = value; }
        }
    }
}