using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract()]
    public class CP_InpatientFeeInfo
    {
        /// <summary>
        /// 项目代码
        /// </summary>
        [DataMember()]
        public int Xmdm { set; get; }
        /// <summary>
        ///  项目名称
        /// </summary>
        [DataMember()]
        public string Xmmc { set; get; }
        /// <summary>
        ///  项目金额
        /// </summary>
        [DataMember()]
        public Double Xmje { set; get; }

        /// <summary>
        ///  总计
        /// </summary>
        [DataMember()]
        public Double Zj { set; get; }

        public CP_InpatientFeeInfo(int xmdm, string strXmmc, Double xmje)
        {
            Xmdm = xmdm;
            Xmmc = strXmmc;
            Xmje = xmje;
        }


        public CP_InpatientFeeInfo()
        {
        }
    }
}