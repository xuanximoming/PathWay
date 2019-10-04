
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{    
    [DataContract()]
    public class Rpt_DataList
    {
        private List<Rpt_PathQuitList> m_pathquitlist = new List<Rpt_PathQuitList>();
        private List<Rpt_PathQuitPie> m_pathquitpie = new List<Rpt_PathQuitPie>();

        /// <summary>
        /// Rpt_PathQuitList类数据
        /// </summary>
        [DataMember]
        public List<Rpt_PathQuitList> PathQuitList
        {
            get { return m_pathquitlist; }
            set { m_pathquitlist = value; }
        }

        /// <summary>
        /// Rpt_PathQuitPie类数据
        /// </summary>
        [DataMember]
        public List<Rpt_PathQuitPie> PathQuitPie
        {
            get { return m_pathquitpie; }
            set { m_pathquitpie = value; }
        }

    }



    [DataContract()]
    public class Rpt_PathQuitList
    {
        private string m_name;
        private string m_tcsj;
        private string m_hzxm;
        private string m_jrsj;
        private string m_dname;
        private string m_tcyy;
        private string m_dept;

        /// <summary>
        /// 科室名称
        /// </summary>
        [DataMember]
        public string Dept
        {
            get { return m_dept; }
            set { m_dept = value; }
        }

        /// <summary>
        /// 临床路径名称
        /// </summary>
        [DataMember]
        public string Name
        {
            get { return m_name; }
            set { m_name = value; }
        }

        /// <summary>
        /// 退出日期
        /// </summary>
        [DataMember]
        public string Tcsj
        {
            get { return m_tcsj; }
            set { m_tcsj = value; }
        }

        /// <summary>
        /// 病人名称
        /// </summary>
        [DataMember]
        public string Hzxm
        {
            get { return m_hzxm; }
            set { m_hzxm = value; }
        }

        /// <summary>
        /// 入径日期
        /// </summary>
        [DataMember]
        public string Jrsj
        {
            get { return m_jrsj; }
            set { m_jrsj = value; }
        }

        /// <summary>
        /// 执行医生
        /// </summary>
        [DataMember]
        public string DName
        {
            get { return m_dname; }
            set { m_dname = value; }
        }

        /// <summary>
        /// 退出原因
        /// </summary>
        [DataMember]
        public string Tcyy
        {
            get { return m_tcyy; }
            set { m_tcyy = value; }
        }
    }

    [DataContract()]
    public class Rpt_PathQuitPie
    {
        private string m_tcyy;
        private int m_Count;

        /// <summary>
        /// 退出原因
        /// </summary>
        [DataMember]
        public string Tcyy
        {
            get { return m_tcyy; }
            set { m_tcyy = value; }
        }

        /// <summary>
        /// 退出原因的病人次数
        /// </summary>
        [DataMember]
        public int Counts
        {
            get { return m_Count; }
            set { m_Count = value; }
        }
    }
}