using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract()]
    public class CP_InpatientPhySign
    {
        /// <summary>
        /// 首页序号int
        /// </summary>
        [DataMember()]
        public int Syxh { set; get; }

        /// <summary>
        /// 体诊序号int
        /// </summary>
        [DataMember()]
        public int Tzxh { set; get; }

        /// <summary>
        ///  测量日期
        /// </summary>
        [DataMember()]
        public string Zlrq { set; get; }

        /// <summary>
        ///  录入日期
        /// </summary>
        [DataMember()]
        public string Lrrq { set; get; }

        /// <summary>
        ///  测量时间
        /// </summary>
        [DataMember()]
        public string Clsj { set; get; }

        /// <summary>
        ///  体温数量
        /// </summary>
        [DataMember()]
        public string Tw { set; get; }

        /// <summary>
        /// 脉搏数量int
        /// </summary>
        [DataMember()]
        public int Mb { set; get; }

        /// <summary>
        /// 呼吸数量int
        /// </summary>
        [DataMember()]
        public int Hx { set; get; }

        /// <summary>
        ///  血压数量
        /// </summary>
        [DataMember()]
        public string Xy { set; get; }

        /// <summary>
        ///  心率
        /// </summary>
        [DataMember()]
        public string Xl { set; get; }

        /// <summary>
        ///  备用说明
        /// </summary>
        [DataMember()]
        public string Memo { set; get; }

        /// <summary>
        ///  说明2
        /// </summary>
        [DataMember()]
        public string Memo2 { set; get; }

        /// <summary>
        ///  物理降温
        /// </summary>
        [DataMember()]
        public string Wljw { set; get; }

        /// <summary>
        ///  起搏心率
        /// </summary>
        [DataMember()]
        public string Qbxl { set; get; }

        /// <summary>
        ///  人工呼吸
        /// </summary>
        [DataMember()]
        public string Rghx { set; get; }

        /// <summary>
        ///  口表
        /// </summary>
        [DataMember()]
        public string Kb { set; get; }

        /// <summary>
        ///  腋表
        /// </summary>
        [DataMember()]
        public string Yb { set; get; }

        /// <summary>
        ///  肛温
        /// </summary>
        [DataMember()]
        public string Gw { set; get; }

        /// <summary>
        ///  采集时间，新表单专用
        /// </summary>
        [DataMember()]
        public string Cjsj { set; get; }

        /// <summary>
        ///  不重复的天数
        /// </summary>
        [DataMember()]
        public int Staticday { set; get; }

        public CP_InpatientPhySign(int syxh, int tzxh, string strZlrq, string strLrrq, string strClsj, string strTw,
                              int mb, int hx, string strXy, string strXl, string strMemo, string strMemo2,
            string strWljw, string strQbxl, string strRghx, string strKb, string strYb, string strGw, string strCjsj,int staticday)
        {
            Syxh = syxh;
            Tzxh = tzxh;
            Zlrq = strZlrq;
            Lrrq = strLrrq;
            Clsj = strClsj;
            Tw = strTw;
            Mb = mb;
            Hx = hx;
            Xy = strXy;
            Xl = strXl;
            Memo = strMemo;
            Memo2 = strMemo2;
            Wljw = strWljw;
            Qbxl = strQbxl;
            Rghx = strRghx;
            Kb = strKb;
            Yb = strYb;
            Gw = strGw;
            Cjsj = strCjsj;
            Staticday = staticday;

        }
        public CP_InpatientPhySign()
        {
        }
    }
}