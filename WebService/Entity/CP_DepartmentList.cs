using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract()]
    public partial class CP_DepartmentList
    {
        #region Model
        private string _ksdm;
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
        ///  科室代码
        /// </summary>
        [DataMember()]
        public string Ksdm
        {
            set { _ksdm = value; }
            get { return _ksdm; }
        }
        /// <summary>
        /// 名称
        /// </summary>
        [DataMember()]
        public string Name
        {
            set { _name = value; }
            get { return _name; }
        }
        #endregion Model

        public CP_DepartmentList()
        { }

        public CP_DepartmentList(string strKsdm, string strName, string strQueryName)
        {
            Ksdm = strKsdm;
            Name = strName;
            QueryName = strQueryName;
        }

 
    }
}