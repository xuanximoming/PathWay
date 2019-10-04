using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;
using DrectSoft.Tool;
namespace Yidansoft.Service.Entity
{
    public partial class CP_InpatinetList : System.Object
    {
        #region 属性
        CP_ClinicalPathLists _Paths = new CP_ClinicalPathLists();
        [DataMember()]
        /// <summary>
        /// 病人能进入的路径类表
        /// </summary>
        public CP_ClinicalPathLists Paths
        {
            get
            {
                return _Paths;
            }
            set
            {
                _Paths = value;
            }
        }
        /// <summary>
        /// 获取一个值，该值表示当前病人是否能进路径
        /// </summary>
        [DataMember()]
        public Boolean IsEnter
        {
            get
            {
                if (Paths.Count == 0)
                    return false;
                else return true;
            }
            set { }
        }
        /// <summary>
        /// 获取一个值，该值表示当前病人是否能进当前节点
        /// </summary>
        [DataMember()]
        public Boolean IsEnterCurrentNode
        {
            get;
            set;
        }
        [DataMember()]
        public int Nl
        {
            get
            {
                if (Csrq == null||Csrq.Trim()=="")
                    return -1;

                return (DateTime.Now.Year - (Convert.ToDateTime(Csrq)).Year);
            }
            set { }
        }
        string _Xb = "1";
        [DataMember()]
        public string Xb
        {
            get
            {

                return _Xb;
            }
            //{
            //    if (!"男女".Contains(Brxb))
            //    {
            //        return "男女";
            //    }
            //    return Brxb;
            //}
            set { _Xb = value; }
        }
        CP_ExamSyrqs _CP_ExamSyrqs;
        /// <summary>
        /// 分析病人属于那几类适用人群
        /// </summary>
        [DataMember()]
        public CP_ExamSyrqs CP_ExamSyrqs
        {
            get
            {
                if (_CP_ExamSyrqs == null)
                {
                    //add by yxy 2012-05-03 暂时不用
                    //_CP_ExamSyrqs = new CP_ExamSyrqs();
                    //CP_ExamSyrqs CP_ExamSyrqsTemp = CP_ExamSyrqs.GetAllCP_ExamSyrq();
                    //foreach (CP_ExamSyrq examSyrq in CP_ExamSyrqsTemp)
                    //{
                    //    if (examSyrq.Xb.Contains(this.Xb) && this.Nl >= examSyrq.Qsnl && this.Nl <= examSyrq.Jsnl)
                    //        _CP_ExamSyrqs.Add(examSyrq);
                    //}
                }
                return _CP_ExamSyrqs;
            }
            set { _CP_ExamSyrqs = value; }
        }
        PatientExamItems _PatientExamItems;
        [DataMember()]
        public PatientExamItems PatientExamItems
        {
            get { return _PatientExamItems; }
            set { _PatientExamItems = value; }
        }
        private String _ICD10;
        /// <summary>
        /// 病人的ICD-10诊断
        /// </summary>
        [DataMember()]
        public String ICD10
        {
            get { return _ICD10; }
            set { _ICD10 = value; }
        }
        private CP_ClinicalPathNode _CurrentCP_ClinicalPathNode;
        [DataMember()]
        public CP_ClinicalPathNode CurrentCP_ClinicalPathNode
        {
            get { return _CurrentCP_ClinicalPathNode; }
            set { _CurrentCP_ClinicalPathNode = value; }
        }
        #endregion
        
        /// <summary>
        /// 表示初始化病人的检查项的方法
        /// </summary>
        public void InitializePatinetExamItems()
        {
            try
            {

                DataTable dt = YidanEHRDataService.SqlHelper.ExecuteDataTable(String.Format(@" SELECT * FROM CP_PatientExamItem WHERE Syxh='{0}'  ", this.Syxh));
                PatientExamItems ExamItems = new PatientExamItems();
                if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        PatientExamItem ExamItem = new PatientExamItem();
                        ExamItem.Jcxm = ConvertMy.ToString(dt.Rows[i]["Jcxm"]);
                        ExamItem.Xmsz = ConvertMy.ToDecimal(dt.Rows[i]["Jcjg"]);

                        ExamItems.Add(ExamItem);
                    }
                this.PatientExamItems = ExamItems;

            }
            catch (Exception ex)
            {
                YidanEHRDataService service = new YidanEHRDataService();
               service. ThrowException(ex);
            }
        }
    }
    [DataContract()]
    public class PatientExamItem : System.Object
    {

        /// <summary>
        /// 检查项目编号
        /// </summary>
        public String Jcxm
        {
            get;

            set;

        }
        /// <summary>
        /// 检查项目数值
        /// </summary>
        public Decimal Xmsz
        {
            get;
            set;
        }


    }
    public class PatientExamItems : List<PatientExamItem>
    {
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="xmbh">项目编号</param>
        /// <returns>满足项目编号的进入条件</returns>
        public PatientExamItem this[String jcxm]
        {
            get
            {
                PatientExamItem item = null;
                foreach (PatientExamItem c in this)
                {
                    if (c.Jcxm == jcxm)
                        item = c;
                }
                return item;
            }
        }
    }
}
