using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract]
    public class RPT_PathStatistic
    {
        //路径代码2012-9-28 wj
        private string m_ljdm;
        //临床路径
        private string m_ljmc;
        //执行情况
        private string m_zxqk;
        //时间段
        private string m_jgsj;
        //收治病人总数
        private int m_hzsl;
        //实施例数
        private int m_ssls;
        //入径率
        private int m_rjl;
        //平均住院费用
        private int m_jzyfy;
        //平均住院天数
        private int m_jzyts;
        //退出例数
        private int m_tcsl;
        //退出率
        private int m_tcl;

        //完成例数
        private int m_wcsl;
        //完成率
        private int m_wcl;

        //变异例数
        private int m_bysl;
        //变异率
        private int m_byl;
        
        //均次费用

        private int m_jcfy;

        //均次天书
        private int m_jcts;

        //路径版本号
        private decimal m_vesion;

        [DataMember]
        public string Ljdm
        {
            get { return m_ljdm; }
            set { m_ljdm = value; }
        }

        [DataMember]
        public string Ljmc
        {
            get { return m_ljmc; }
            set { m_ljmc = value; }
        }

        [DataMember]
        public string Zxqk
        {
            get { return m_zxqk; }
            set { m_zxqk = value; }
        }

        [DataMember]
        public string Jgsj
        {
            get { return m_jgsj; }
            set { m_jgsj = value; }
        }
        [DataMember]
        public int HzSl
        {
            get { return m_hzsl; }
            set { m_hzsl = value; }
        }
        
        [DataMember]
        public int Ssls
        {
            get { return m_ssls; }
            set { m_ssls = value; }
        }

        [DataMember]
        public int Rjl
        {
            get { return m_rjl; }
            set { m_rjl = value; }
        }

        [DataMember]
        public int Jzyfy
        {
            get { return m_jzyfy; }
            set { m_jzyfy = value; }
        }

        [DataMember]
        public int Jzyts
        {
            get { return m_jzyts; }
            set { m_jzyts = value; }
        }

        [DataMember]
        public int Tcsl
        {
            get { return m_tcsl; }
            set { m_tcsl = value; }
        }

        [DataMember]
        public int Tcl
        {
            get { return m_tcl; }
            set { m_tcl = value; }
        }

        [DataMember]
        public int WcSl
        {
            get { return m_wcsl; }
            set { m_wcsl = value; }
        }

        [DataMember]
        public int Wcl
        {
            get { return m_wcl; }
            set { m_wcl = value; }
        }

        [DataMember]
        public int BySl
        {
            get { return m_bysl; }
            set { m_bysl = value; }
        }

        [DataMember]
        public int Byl
        {
            get { return m_byl; }
            set { m_byl = value; }
        }
        [DataMember]
        public int Jcfy
        {
            get { return m_jcfy; }
            set { m_jcfy = value; }
        }

        [DataMember]
        public int Jcts
        {
            get { return m_jcts; }
            set { m_jcts = value; }
        }

        [DataMember]
        public decimal Vesion
        {
            get { return m_vesion; }
            set { m_vesion = value; }
        }
        
    }
}