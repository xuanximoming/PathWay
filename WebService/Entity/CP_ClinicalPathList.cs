using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [Serializable]
    public partial class CP_ClinicalPathList
    {
        #region Model
        private string m_ljdm;
        private string m_name;
        private string m_queryname;
        private string m_ljms;
        private decimal m_zgts;
        private decimal m_jcfy;
        private decimal m_vesion;
        private string m_cjsj;
        private string m_shsj;
        private string m_shys;
        private string m_yxjl;
        private string m_LjSyqk;
        private decimal m_LjSysl;
        private string m_syks;
        private string m_deptName;
        private string m_syksName;
        private int m_yxjlId;
        private string m_py;
        private string m_workFlowXML;
        private string m_Bzmc = string.Empty; //病种命称

        /// <summary>
        /// 路径代码
        /// </summary>
        [DataMember()]
        public string Ljdm
        {
            set { m_ljdm = value; }
            get { return m_ljdm; }
        }

        /// <summary>
        /// 路径名称
        /// </summary>
        [DataMember()]
        public string Name
        {
            set { m_name = value; }
            get { return m_name; }
        }
        /// <summary>
        /// 拼音
        /// </summary>
        [DataMember()]
        public string QueryName
        {
            set { m_queryname = value; }
            get { return m_queryname; }
        }
        /// <summary>
        /// 路径描述
        /// </summary>
        [DataMember()]
        public string Ljms
        {
            set { m_ljms = value; }
            get { return m_ljms; }
        }

        /// <summary>
        /// 天数
        /// </summary>
        [DataMember()]
        public decimal Zgts
        {
            set { m_zgts = value; }
            get { return m_zgts; }
        }

        /// <summary>
        /// 均次费用
        /// </summary>
        [DataMember()]
        public decimal Jcfy
        {
            set { m_jcfy = value; }
            get { return m_jcfy; }
        }

        /// <summary>
        /// 版本
        /// </summary>
        [DataMember()]
        public decimal Vesion
        {
            set { m_vesion = value; }
            get { return m_vesion; }
        }

        /// <summary>
        /// 创建时间
        /// </summary>
        [DataMember()]
        public string Cjsj
        {
            set { m_cjsj = value; }
            get { return m_cjsj; }
        }

        /// <summary>
        /// 审核时间
        /// </summary>
        [DataMember()]
        public string Shsj
        {
            set { m_shsj = value; }
            get { return m_shsj; }
        }

        /// <summary>
        /// 审核医师
        /// </summary>
        [DataMember()]
        public string Shys
        {
            set { m_shys = value; }
            get { return m_shys; }
        }

        /// <summary>
        /// 有效记录
        /// </summary>
        [DataMember()]
        public string Yxjl
        {
            set { m_yxjl = value; }
            get { return m_yxjl; }
        }

        /// <summary>
        /// 有效记录ID
        /// </summary>
        [DataMember()]
        public int YxjlId
        {
            set { m_yxjlId = value; }
            get { return m_yxjlId; }
        }

        /// <summary>
        /// 适应科室
        /// </summary>
        [DataMember()]
        public string Syks
        {
            set { m_syks = value; }
            get { return m_syks; }
        }

        /// <summary>
        /// 适应科室NAME
        /// </summary>
        [DataMember()]
        public string ShysName
        {
            set { m_syksName = value; }
            get { return m_syksName; }
        }

        /// <summary>
        /// DEPT NAME
        /// </summary>
        [DataMember()]
        public string DeptName
        {
            set { m_deptName = value; }
            get { return m_deptName; }
        }

        /// <summary>
        /// XML
        /// </summary>
        [DataMember()]
        public string WorkFlowXML
        {
            set { m_workFlowXML = value; }
            get { return m_workFlowXML; }
        }

        /// <summary>
        /// 路径使用情况
        /// </summary>
        [DataMember()]
        public string LjSyqk
        {
            set { m_LjSyqk = value; }
            get { return m_LjSyqk; }
        }

        /// <summary>
        /// 路径使用人数数量
        /// </summary>
        [DataMember()]
        public decimal LjSysl
        {
            set { m_LjSysl = value; }
            get { return m_LjSysl; }
        }

        /// <summary>
        /// 病种名称
        /// </summary>
        [DataMember()]
        public string Bzmc
        {
            set { m_Bzmc = value; }
            get { return m_Bzmc; }
        }
        /// <summary>
        /// 拼音
        /// </summary>
         [DataMember()]
        public string Py
        {
            set { m_py = value; }
            get { return m_py; }
        }
        #endregion Model

        public CP_ClinicalPathList()
        { }

        public CP_ClinicalPathList(string strLjdm, string strName, string strLjms, decimal zgts, decimal jcfy, decimal vesion, string strcjsj,
                                    string strShsj, string strShys, string strYxjl, string strSyks,
                                    string strdeptName, string strSyksName, int strYxjlId,string strXml)
        {
            m_ljdm = strLjdm;
            m_name = strName;
            m_ljms = strLjms;
            m_zgts = zgts;
            m_jcfy = jcfy;
            m_vesion = vesion;
            m_cjsj = strcjsj;
            m_shsj = strShsj;
            m_shys = strShys;
            m_yxjl = strYxjl;
            m_syks = strSyks;
            m_deptName = strdeptName;
            m_syksName = strSyksName;
            m_yxjlId = strYxjlId;
        
            m_workFlowXML = strXml;
        }
        /// <summary>
        /// 构造函数CP_ClinicalPathList
        /// </summary>
        /// <param name="Ljdm">路径代码</param>
        /// <param name="Name">路径名称</param>
        /// <param name="QueryName">拼音代码</param>
        public CP_ClinicalPathList(String Ljdm, String Name,String QueryName)
        {
            m_ljdm = Ljdm;
            m_name = Name;
            m_queryname = QueryName;
     
        }
    }
}