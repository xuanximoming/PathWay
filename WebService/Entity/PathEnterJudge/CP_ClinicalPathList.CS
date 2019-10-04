using System;
using System.IO;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;
using YidanSoft.Tool;
using System.Text;
namespace Yidansoft.Service.Entity
{
    [DataContract()]
    public partial class CP_ClinicalPathList : System.Object
    {

        /// <summary>
        /// 表示病人是否满足当前路径的方法
        /// </summary>
        /// <param name="patient">病人</param>
        /// <returns>表示病人是否满足当前路径</returns>
        public void CanEnter(CP_InpatinetList patient)
        {
            Boolean isEnterICD10Conditions = false;
            Boolean isEnterExamConditions = true;

            //判断病人的ICD-10诊断是否包含在路径的ICD-10列表中，包含返回True，否则返回false

           // if (PathEnterJudgeConditions.ICD10Conditions.Count == 0) isEnterICD10Conditions = true;
            foreach (CP_PathEnterJudgeCondition Condition in PathEnterJudgeConditions.ICD10Conditions)
            {
                if (Condition.Jcxm == patient.ICD10)
                {
                    isEnterICD10Conditions = true;
                    break;
                }
            }
            //当不存在ICD条件时允许进入
            //if ( PathEnterJudgeConditions.ICD10Conditions.Count == 0)
            //{
            //    isEnterICD10Conditions = true;
            //}
            
            //任意检查项不满足返回False
            foreach (CP_PathEnterJudgeCondition Condition in PathEnterJudgeConditions.ExamConditions)
            {
               // Condition.CanEnter(patient);
                if (!Condition.CanEnter(patient) && isEnterExamConditions)
                {
                    isEnterExamConditions = false;
                   // break;
                }
            }
            //当不存在检查项条件时允许进入
            //if (PathEnterJudgeConditions.ExamConditions.Count == 0 )
            //{
            //    isEnterExamConditions = true;
            //}
            //当ICD条件满足时
            if (isEnterICD10Conditions)
            {
                IsPossibleEnter = true;
            }
            //但所有的条件满足时
            if (isEnterICD10Conditions && isEnterExamConditions)
            {
                IsCanEnter = true;
            }
        }
        /// <summary>
        /// 初始化当前路径的所有进入条件
        /// </summary>
        public void InitializePathEnterConditions()
        {
            YidanEHRDataService Service = new YidanEHRDataService();
            this.PathEnterJudgeConditions .AddRange(Service.GetPathCP_PathEnterJudgeConditionAll(this.Ljdm));
        }
        #region 属性
        CP_PathEnterJudgeConditions _PathEnterJudgeConditions = new CP_PathEnterJudgeConditions();
        //public string Ljdm
        //{
        //    get
        //    {
        //        return ljdm;
        //    }
        //    set
        //    {
        //        if (ljdm != value)
        //        {
        //            ljdm = value;
        //        }
        //    }
        //}
        //private string ljdm;
        [DataMember]
        public CP_PathEnterJudgeConditions PathEnterJudgeConditions
        {
            get
            {
                return _PathEnterJudgeConditions;
            }
            set
            {
                _PathEnterJudgeConditions = value;
            }
        }

        private Boolean _IsCanEnter = false;
     
         [DataMember()]
        /// <summary>
        /// 该路径可以被进入
        /// </summary>
        public Boolean IsCanEnter
        {
            get { return _IsCanEnter; }
            set { _IsCanEnter = value; }
        }

         private Boolean _ISPossibleEnter = false;

        /// <summary>
        /// 有可能进入，ICD-10即入院诊断已满足，但是检查项暂时不满足
        /// </summary>
        [DataMember()]
         public Boolean IsPossibleEnter
         {
             get { return _ISPossibleEnter; }
             set { _ISPossibleEnter = value; }
         }
    

        #endregion
    }
}
