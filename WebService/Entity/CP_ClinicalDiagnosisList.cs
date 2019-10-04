using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract()]
    public class CP_ClinicalDiagnosisList
    {
        #region Model
        private string _ljdm;
        private string _bzdm;
        private string _bzmc;

        /// <summary>
        /// 路径名称
        /// </summary>
        [DataMember()]
        public string Ljdm
        {
            set { _ljdm = value; }
            get { return _ljdm; }
        }

        /// <summary>
        /// 病种代码
        /// </summary>
        [DataMember()]
        public string Bzdm
        {
            set { _bzdm = value; }
            get { return _bzdm; }
        }

        /// <summary>
        /// 病种名称
        /// </summary>
        [DataMember()]
        public string Bzmc
        {
            set { _bzmc = value; }
            get { return _bzmc; }
        }
        #endregion Model

        public CP_ClinicalDiagnosisList()
        { }

        public CP_ClinicalDiagnosisList(string strLjdm, string strBzdm, string strBzmc)
        {
            Ljdm = strLjdm;
            Bzdm = strBzdm;
            Bzmc = strBzmc;
        }
    }
}