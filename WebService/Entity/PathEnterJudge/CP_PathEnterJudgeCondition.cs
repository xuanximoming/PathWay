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
        /// ��ʾ�����Ƿ����㵱ǰ�����ķ���
        /// </summary>
        /// <param name="patient">����</param>
        /// <returns>��ʾ�����Ƿ����㵱ǰ����</returns>
        public Boolean CanEnter(CP_InpatinetList patient)
        {
            #region ����������Ⱥ�Ĵ��루�ѷ�����
            ////���ֹ���ò��˲������κ�������Ⱥ
            //if (patient.CP_ExamSyrqs.Count == 0)
            //    return true;
            ////���豾������������Ⱥ���Ͳ��˵�������Ⱥ��û�н���
            //Boolean notSubset = true;
            //#region ����ж�
            //// �����ڽ�������
            //if (patient.PatientExamItems[this.Jcxm] == null)
            //    return false;
            ////ƥ�䵱ǰ������Ӧ�Ĳ��˵ļ����
            //PatientExamItem ExamItem = patient.PatientExamItems[this.Jcxm];
            //foreach (SuitCrowdMapScope Scope in this.SuitCrowsMapScopes)
            //{
            //    foreach (CP_ExamSyrq examSyrq in patient.CP_ExamSyrqs)
            //    {
            //        //ƥ��������Ⱥ
            //        if (Scope.ExamSyrq.Jlxh == examSyrq.Jlxh)
            //        {
            //            //���ֽ���
            //            notSubset = false;
            //            this.IsEnetr = false;
            //            this.ExamValue = ExamItem.Xmsz.ToString();
            //            //ֻҪƥ���ϼ�����ĳ��������Ⱥ�ķ�Χ������True
            //            if (ExamItem.Xmsz >= Scope.Ksfw && ExamItem.Xmsz <= Scope.Jsfw)
            //            {
            //                this.IsEnetr = true;
            //                this.ExamValue = ExamItem.Xmsz.ToString();
            //                return true;

            //            }
            //            else//���ƥ����������Ⱥ�����ǲ��˵���ֵ���������ļ���Χ�У�˵����ǰ���˲�����ü����
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
            //ֻҪƥ���ϼ���� �ķ�Χ������True
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
        
        #region ����
        [DataMember]
        /// <summary>
        /// ��ע
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
        /// ��λ
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
        /// �����ض�
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
        /// �����Ŀ��Xmlb=1��CP_ExamDictionaryDetail.Jlxh �� Xmlb=2��CP_Diagnosis.Zdbs��
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
        /// �ڵ��GUID
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
        /// ������Χ
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
        /// ��ʼ��Χ
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
        /// ���1·�� OR 2�ڵ㣩
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
        /// ·������
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
        /// �ϼ����ࣨCP_ExamDictionary.Jlxh��
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
        /// ������Ⱥ��Ӧ�ļ���Χ
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
        /// ������Ⱥ
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
        /// 1��ʾ�����Ŀ��2��ʾICD-10
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

                if (Xmlb == 1) _XmlbName = "�����";
                if (Xmlb == 2) _XmlbName = "ICD-10";
                if (Xmlb == 3) _XmlbName = "������";
            
                return _XmlbName;
            }
            set { _XmlbName = value; }
        }

        private Boolean _IsEnetr;
        [DataMember]
        /// <summary>
        /// ��ǰ���˵ļ�������Ƿ�����
        /// </summary>
        public Boolean IsEnetr
        {
            get { return _IsEnetr; }
            set { _IsEnetr = value; }
        }

        private string _ExamValue;
        [DataMember]
        /// <summary>
        /// ����ʵ�ʵļ��ֵ
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
                { _MatchResultName = "��������"; }
                if (MatchResult == MatchResultState.NoMatch)
                { _MatchResultName = "����������"; }
                if (MatchResult == MatchResultState.NoExist)
                { _MatchResultName = "����û�иü����"; }
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
    /// ������Ⱥ��Ӧ�ļ�����������Χ
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
    /// /// ������Ⱥ��Ӧ�ļ�����������Χ���
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
