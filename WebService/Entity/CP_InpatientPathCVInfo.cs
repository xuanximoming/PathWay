using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ComponentModel;

namespace Yidansoft.Service
{
    [DataContract()]
    public class CP_InpatientPathCVInfo  : INotifyPropertyChanged
    {

        private string m_Id;
        /// <summary>
        ///   首页序号
        /// </summary>  
        [DataMember()]
        public string Id
        {
            get { return m_Id; }
            set { m_Id = value; }
        }

        private string m_Syxh;
        /// <summary>
        ///   首页序号
        /// </summary>  
        [DataMember()]
        public string Syxh
        {
            get { return m_Syxh; }
            set { m_Syxh = value; }
        }
        private string m_Ljdm;
        /// <summary>
        /// 临床路径代码
        /// </summary>
        [DataMember()]
        public string Ljdm
        {
            get { return m_Ljdm; }
            set { m_Ljdm = value; }
        }
        private string m_Mxdm;
        /// <summary>
        /// 路径明细
        /// </summary>
        [DataMember()]
        public string Mxdm
        {
            get { return m_Mxdm; }
            set { m_Mxdm = value; }
        }
        private string m_Bylb;
        /// <summary>
        /// 变异类别
        /// </summary>
        [DataMember()]
        public string Bylb
        {
            get { return m_Bylb; }
            set { m_Bylb = value; }
        }
        private string m_BylbName;
        /// <summary>
        /// 变异类别Name
        /// </summary>
        [DataMember()]
        public string BylbName
        {
            get { return m_BylbName; }
            set { m_BylbName = value; }
        }

        private string m_Bynr;
        /// <summary>
        /// 变异内容
        /// </summary>
        [DataMember()]
        public string Bynr
        {
            get { return m_Bynr; }
            set { m_Bynr = value; }
        }
        private string m_Byyy;
        /// <summary>
        /// 变异原因
        /// </summary>
        [DataMember()]
        public string Byyy
        {
            get { return m_Byyy; }
            set { m_Byyy = value; }
        }
        private string m_Bysj;
        /// <summary>
        /// 变异时间
        /// </summary>
        [DataMember()]
        public string Bysj
        {
            get { return m_Bysj; }
            set { m_Bysj = value; }
        }


        public CP_InpatientPathCVInfo(string strId,string strSyxh,string strLjdm,string strMxdm,string strBylb,
                 string strBylbName,string strBynr,string strByyy,string strBysj)
        {
            Id = strId;
            Syxh = strSyxh;
            Ljdm = strLjdm;
            Mxdm = strMxdm;
            Bylb = strBylb;
            BylbName = strBylbName;
            Bynr = strBynr;
            Byyy = strByyy;
            Bysj = strBysj;

        }

        public CP_InpatientPathCVInfo()
        {

        }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}