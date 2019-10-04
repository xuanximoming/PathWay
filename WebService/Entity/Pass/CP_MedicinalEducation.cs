using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;


namespace Yidansoft.Service.Entity
{
    [DataContract]
    public partial class CP_MedicinalEducation
    {
        private string _Wtbh;
        /// <summary>
        /// 问题编号
        /// </summary>
        [DataMember]
        public string Wtbh
        {
            get { return _Wtbh; }
            set { _Wtbh = value; }
        }

        private string _Wtlb;
        /// <summary>
        /// 问题类别
        /// </summary>
        [DataMember]
        public string Wtlb
        {
            get { return _Wtlb; }
            set { _Wtlb = value; }
        }

        private string _Wtnr;
        /// <summary>
        /// 问题内容
        /// </summary>
        [DataMember]
        public string Wtnr
        {
            get { return _Wtnr; }
            set { _Wtnr = value; }
        }

        private string _Wtda;
        /// <summary>
        /// 问题答案
        /// </summary>
        [DataMember]
        public string Wtda
        {
            get { return _Wtda; }
            set { _Wtda = value; }
        }


    }
}