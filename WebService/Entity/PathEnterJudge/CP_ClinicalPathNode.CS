using System;
using System.IO;
using System.Runtime.Serialization;
namespace Yidansoft.Service.Entity
{
   
    public partial class CP_ClinicalPathNode : System.Object
    {
        /// <summary>
        /// 表示病人是否满足当前路径的方法
        /// </summary>
        /// <param name="patient">病人</param>
        /// <returns>表示病人是否满足当前路径</returns>
        public Boolean CanEnter(CP_InpatinetList patient)
        {
            IsEnter= true;
            //任意检查项不满足返回False
            foreach (CP_PathEnterJudgeCondition Condition in PathEnterJudgeConditions.ExamConditions)
            {
            
                if (!Condition.CanEnter(patient))
                {
                    IsEnter = false;
                    break;
                }
            }
            //但不存在检查项条件时允许进入
            if (PathEnterJudgeConditions.ExamConditions.Count == 0)
            {
                IsEnter = true;
            }
            return IsEnter;
        }
        #region 属性
        [DataMember]
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
        private String _NodeCode;
        [DataMember]
        public String NodeCode
        {
            get { return _NodeCode; }
            set { _NodeCode = value; }
        }
        [DataMember]
        public Boolean IsEnter { get; set; }
        CP_PathEnterJudgeConditions _PathEnterJudgeConditions = new CP_PathEnterJudgeConditions();
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
        #endregion
        /// <summary>
        /// 初始化当前路径的所有进入条件
        /// </summary>
        public void InitializeNodeEnterConditions()
        {
            YidanEHRDataService Service = new YidanEHRDataService();
          this.PathEnterJudgeConditions.AddRange(  Service.GetNodeCP_PathEnterJudgeConditionAll(this.NodeCode));
        }
    }
}
