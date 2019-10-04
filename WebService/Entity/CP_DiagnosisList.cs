using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract()]
    public partial class CP_DiagnosisList
    {
        #region Model
        private string _zdbs;
        private string _zddm;
        private string _name;
        private string _queryname;

        /// <summary>
        /// QUERY NAME
        /// </summary>
        [DataMember()]
        public string QueryName
        {
            get { return _queryname; }
            set { _queryname = value; }
        }
        /// <summary>
        ///  诊断标识
        /// </summary>
        [DataMember()]
        public string Zdbs
        {
            set { _zdbs = value; }
            get { return _zdbs; }
        }
        /// <summary>
        /// 诊断代码(ICD10)
        /// </summary>
        [DataMember()]
        public string Zddm
        {
            set { _zddm = value; }
            get { return _zddm; }
        }
        /// <summary>
        ///    疾病名称
        /// </summary>
        [DataMember()]
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        #endregion Model
        public CP_DiagnosisList()
        { }
        public CP_DiagnosisList(string strZdbs, string strZddm, string strName, string strQueryName)
        {
            Zdbs = strZdbs;
            Zddm = strZddm;
            Name = strName;
            QueryName = strQueryName;
        }

    }
}