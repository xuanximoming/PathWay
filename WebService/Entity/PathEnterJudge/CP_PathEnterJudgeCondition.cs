using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using YidanSoft.Tool;
namespace Yidansoft.Service.Entity
{
    [Serializable]
    [DataContract()]
    public class CP_PathEnterJudgeCondition : System.Object
    {
        SuitCrowsMapScopes _SuitCrowsMapScopes = new SuitCrowsMapScopes();
        /// <summary>
        /// 表示病人是否满足当前条件的方法
        /// </summary>
        /// <param name="patient">病人</param>
        /// <returns>表示病人是否满足当前条件</returns>
        public Boolean CanEnter(CP_InpatinetList patient)
        {
            #region 包含适用人群的代码（已废弃）
            ////出现怪物，该病人不属于任何适用人群
            //if (patient.CP_ExamSyrqs.Count == 0)
            //    return true;
            ////假设本条件的适用人群，和病人的适用人群，没有交集
            //Boolean notSubset = true;
            //#region 检查判定
            //// 不存在进入条件
            //if (patient.PatientExamItems[this.Jcxm] == null)
            //    return false;
            ////匹配当前检查项对应的病人的检查项
            //PatientExamItem ExamItem = patient.PatientExamItems[this.Jcxm];
            //foreach (SuitCrowdMapScope Scope in this.SuitCrowsMapScopes)
            //{
            //    foreach (CP_ExamSyrq examSyrq in patient.CP_ExamSyrqs)
            //    {
            //        //匹配适用人群
            //        if (Scope.ExamSyrq.Jlxh == examSyrq.Jlxh)
            //        {
            //            //出现交集
            //            notSubset = false;
            //            this.IsEnetr = false;
            //            this.ExamValue = ExamItem.Xmsz.ToString();
            //            //只要匹配上检查项的某个适用人群的范围，返回True
            //            if (ExamItem.Xmsz >= Scope.Ksfw && ExamItem.Xmsz <= Scope.Jsfw)
            //            {
            //                this.IsEnetr = true;
            //                this.ExamValue = ExamItem.Xmsz.ToString();
            //                return true;

            //            }
            //            else//如果匹配上适用人群，但是病人的数值不在条件的检查项范围中，说明当前病人不满足该检查项
            //                return false;
            //        }

            //    }
            //}
            //#endregion
            //return notSubset;
            #endregion

            
            this.IsEnetr = false;
            if (patient.PatientExamItems[this.Jcxm] == null)
            {
                MatchResult = MatchResultState.NoExist;
                return IsEnetr;
            }
            PatientExamItem ExamItem = patient.PatientExamItems[this.Jcxm];
            this.ExamValue = ExamItem.Xmsz.ToString();
            //只要匹配上检查项 的范围，返回True
            if (ExamItem.Xmsz >= ConvertMy.ToDecimal(this.Ksfw) && ExamItem.Xmsz <= ConvertMy.ToDecimal(this.Jsfw))
            {
                this.IsEnetr = true;
                MatchResult = MatchResultState.Match;
            }
            else {
                MatchResult = MatchResultState.NoMatch;
                
            }
            return IsEnetr;

        }
        
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
        [DataMember()]
        /// <summary>
        /// 适用人群对应的检查项范围
        /// </summary>
        public SuitCrowsMapScopes SuitCrowsMapScopes
        {
            get
            {

                _SuitCrowsMapScopes = new SuitCrowsMapScopes();
                String[] KsfwTemp;
                String[] JsfwTemp;
                String[] SyrqTemp;
                if (Ksfw != null && Ksfw.Trim() != "" && Jsfw != null && Jsfw.Trim() != "" && Syrq != null && Syrq.Trim() != "")
                {
                    KsfwTemp = Ksfw.Split(',');
                    JsfwTemp = Jsfw.Split(',');
                    SyrqTemp = Syrq.Split(',');
                    for (int i = 0; i < SyrqTemp.Length; i++)
                    {
                        _SuitCrowsMapScopes.Add(new SuitCrowdMapScope(KsfwTemp[i], JsfwTemp[i], SyrqTemp[i]));
                    }
                }
                return _SuitCrowsMapScopes;
            }
            set { _SuitCrowsMapScopes = value; }
        }
        [DataMember]
        /// <summary>
        /// 适用人群
        /// </summary>
        public String Syrq
        {
            get
            {
                return syrq;
            }
            set
            {
                if (syrq != value)
                {
                    syrq = value;
                }
            }
        }
        private String syrq;


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

        private String _XmlbName = "";

        [DataMember]
        public String XmlbName
        {
            get
            {

                if (Xmlb == 1) _XmlbName = "检查项";
                if (Xmlb == 2) _XmlbName = "ICD-10";
                if (Xmlb == 3) _XmlbName = "描述项";
            
                return _XmlbName;
            }
            set { _XmlbName = value; }
        }

        private Boolean _IsEnetr;
        [DataMember]
        /// <summary>
        /// 当前病人的检测条件是否满足
        /// </summary>
        public Boolean IsEnetr
        {
            get { return _IsEnetr; }
            set { _IsEnetr = value; }
        }

        private string _ExamValue;
        [DataMember]
        /// <summary>
        /// 病人实际的检测值
        /// </summary>
        public string ExamValue
        {
            get { return _ExamValue; }
            set { _ExamValue = value; }
        }

        private String _MatchResultName = "";
        [DataMember]
        public String MatchResultName
        {
            get {
                if (MatchResult == MatchResultState.Match)
                { _MatchResultName = "满足条件"; }
                if (MatchResult == MatchResultState.NoMatch)
                { _MatchResultName = "不满足条件"; }
                if (MatchResult == MatchResultState.NoExist)
                { _MatchResultName = "病人没有该检查项"; }
                return _MatchResultName; }
            set { _MatchResultName = value; }
        }
        private MatchResultState _MatchResult = MatchResultState.NoExist;
        [DataMember]
        public MatchResultState MatchResult
        {
            get { return _MatchResult; }
            set { _MatchResult = value; }
        }

        #endregion
    }
    [DataContract()]
    /// <summary>
    /// 适用人群对应的检查项的正常范围
    /// </summary>
    public class SuitCrowdMapScope : System.Object
    {
        public SuitCrowdMapScope(String ksfw, String jsfw, String Syrq)
        {
            Ksfw = ConvertMy.ToDecimal(ksfw);
            Jsfw = ConvertMy.ToDecimal(jsfw);
            ExamSyrq = new CP_ExamSyrq(Syrq);
        }
        [DataMember()]
        public CP_ExamSyrq ExamSyrq { get; set; }
        [DataMember()]
        public Decimal Jsfw { get; set; }
        [DataMember()]
        public Decimal Ksfw { get; set; }
    }

    /// <summary>
    /// /// 适用人群对应的检查项的正常范围类表
    /// </summary>
    public class SuitCrowsMapScopes : List<SuitCrowdMapScope>
    {
        //private String _Syrq = "";
        //public String Syrq
        //{
        //    get
        //    {
        //        foreach (var item in this)
        //        {
        //            _Syrq += "," + item.ExamSyrq.Jlxh;
        //        }
        //        return _Syrq.IndexOf(',') > 0 ? _Syrq.Substring(1) : "";
        //    }
        //}
        //private String _Ksfw="";
        //public String Ksfw
        //{
        //    get
        //    {
        //        foreach (var item in this)
        //        {
        //            _Ksfw += "," + item.Ksfw;
        //        }
        //        return _Ksfw.IndexOf(',') > 0 ? _Ksfw.Substring(1) : "";
        //    }
        //}
        //private String _Jsfw="";
        //public String Jsfw
        //{
        //    get
        //    {
        //        foreach (var item in this)
        //        {
        //            _Jsfw += "," + item.Jsfw;
        //        }
        //        return _Jsfw.IndexOf(",") > 0 ? _Jsfw.Substring(1) : "";
        //    }
        //}
    }
}
