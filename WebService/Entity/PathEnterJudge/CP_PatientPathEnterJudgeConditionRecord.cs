using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using YidanSoft.Tool;
namespace Yidansoft.Service.Entity
{
    [DataContract()]
    public class CP_PatientPathEnterJudgeConditionRecord : System.Object
    {
        #region 属性
        [DataMember]
        /// <summary>
        /// 备注
        /// </summary>
        public string Bz
        {
            get
            {
                return bz;
            }
            set
            {
                if (bz != value)
                {
                    bz = value;
                }
            }
        }
        private string bz;
        [DataMember]
        /// <summary>
        /// 单位
        /// </summary>
        public string Dw
        {
            get
            {
                return dw;
            }
            set
            {
                if (dw != value)
                {
                    dw = value;
                }
            }
        }
        private string dw;
        [DataMember]
        /// <summary>
        /// 自增地段
        /// </summary>
        public int ID
        {
            get
            {
                return iD;
            }
            set
            {
                if (iD != value)
                {
                    iD = value;
                }
            }
        }
        private int iD;
        [DataMember]
        /// <summary>
        /// 检查项目（Xmlb=1：CP_ExamDictionaryDetail.Jlxh 或 Xmlb=2：CP_Diagnosis.Zdbs）
        /// </summary>
        public string Jcxm
        {
            get
            {
                return jcxm;
            }
            set
            {
                if (jcxm != value)
                {
                    jcxm = value;
                }
            }
        }
        private string jcxm;
        private String _JcxmName;
        [DataMember]
        public String JcxmName
        {
            get { return _JcxmName; }
            set { _JcxmName = value; }
        }
        [DataMember]
        /// <summary>
        /// 节点的GUID
        /// </summary>
        public string Jddm
        {
            get
            {
                return jddm;
            }
            set
            {
                if (jddm != value)
                {
                    jddm = value;
                }
            }
        }
        private string jddm;
        [DataMember]
        /// <summary>
        /// 结束范围
        /// </summary>
        public string Jsfw
        {
            get
            {
                return jsfw;
            }
            set
            {
                if (jsfw != value)
                {
                    jsfw = value;
                }
            }
        }
        private string jsfw;
        [DataMember]
        /// <summary>
        /// 开始范围
        /// </summary>
        public string Ksfw
        {
            get
            {
                return ksfw;
            }
            set
            {
                if (ksfw != value)
                {
                    ksfw = value;
                }
            }
        }
        private string ksfw;
        [DataMember]
        /// <summary>
        /// 类别（1路径 OR 2节点）
        /// </summary>
        public int Lb
        {
            get
            {
                return lb;
            }
            set
            {
                if (lb != value)
                {
                    lb = value;
                }
            }
        }
        private int lb;
        [DataMember]
        /// <summary>
        /// 路径代码
        /// </summary>
        public string Ljdm
        {
            get
            {
                return ljdm;
            }
            set
            {
                if (ljdm != value)
                {
                    ljdm = value;
                }
            }
        }
        private string ljdm;
        [DataMember]
        /// <summary>
        /// 上级分类（CP_ExamDictionary.Jlxh）
        /// </summary>
        public string Sjfl
        {
            get
            {
                return sjfl;
            }
            set
            {
                if (sjfl != value)
                {
                    sjfl = value;
                }
            }
        }
        private string sjfl;



        [DataMember]
        /// <summary>
        /// 1表示检查项目，2表示ICD-10
        /// </summary>
        public int Xmlb
        {
            get
            {
                return xmlb;
            }
            set
            {
                if (xmlb != value)
                {
                    xmlb = value;
                }
            }
        }
        private int xmlb;

        private String _Jcjg;
        [DataMember]
        public String Jcjg
        {
            get { return _Jcjg; }
            set { _Jcjg = value; }
        }
        private String _Pdjg;
        [DataMember]
        public String Pdjg
        {
            get { return _Pdjg; }
            set { _Pdjg = value; }
        }

        private String _Ljxh;
        [DataMember]
        public String Ljxh
        {
            get { return _Ljxh; }
            set { _Ljxh = value; }
        }
        private String _Syxh;
        [DataMember]
        public String Syxh
        {
            get { return _Syxh; }
            set { _Syxh = value; }
        }
        #endregion
    }

}
