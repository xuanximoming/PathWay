using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Collections;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract()]
    public class RPT_PathVariation
    {
        //科室名称
        private string m_deptname;
        //路径代码
        private string m_pathid;
        //路径名称
        private string m_pathname;
        //变异名称
        private string m_variationname;
        //变异类别
        private string m_variationtype;
        //入径率
        private string m_variationcount;

        /// <summary>
        /// 科室名称
        /// </summary>
        [DataMember()]
        public string DeptName
        {
            get { return m_deptname; }
            set { m_deptname = value; }
        }
        /// <summary>
        /// 路径代码
        /// </summary>
        [DataMember()]
        public string PathID
        {
            get { return m_pathid; }
            set { m_pathid = value; }
        }
        /// <summary>
        /// 路径名称
        /// </summary>
        [DataMember()]
        public string PathName
        {
            get { return m_pathname; }
            set { m_pathname = value; }
        }

        /// <summary>
        /// 路径变异名称
        /// </summary>
        [DataMember()]
        public string VariationName
        {
            get { return m_variationname; }
            set { m_variationname = value; }
        }

        /// <summary>
        /// 路径变异类型
        /// </summary>
        [DataMember()]
        public string VariationType
        {
            get { return m_variationtype; }
            set { m_variationtype = value; }
        }

        /// <summary>
        /// 路径变异数量
        /// </summary>
        [DataMember()]
        public string VariationCount
        {
            get { return m_variationcount; }
            set { m_variationcount = value; }
        }


    }

    /// <summary>
    /// 定义一个List中放入string类
    /// </summary>
    [DataContract()]
    public class ListString
    {
        private List<string> m_list = new List<string>();

        [DataMember()]
        public List<string> list { get { return m_list; } set { m_list = value; } }
    }

}