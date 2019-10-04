using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 条件列表类
    /// </summary>
    [Serializable]
    public class CP_PathEnterJudgeConditions : List<CP_PathEnterJudgeCondition>
    {
        /// <summary>
        /// 表示病人是否满足当前条件列表的方法
        /// </summary>
        /// <param name="patient">病人</param>
        /// <returns>表示病人是否满足当前条件列表</returns>
        public Boolean CanEnter(CP_InpatinetList patient)
        {
            Boolean isEnter = true;
            foreach (CP_PathEnterJudgeCondition Condition in this)
            {
                if (!Condition.CanEnter(patient))
                {
                    isEnter = false;
                    break;
                }
            }
            return isEnter;
        }
        //public static CP_PathEnterJudgeConditions GetConditions(String Ljdm)
        //{
        //    return new CP_PathEnterJudgeConditions();
        //}
        //public static CP_PathEnterJudgeConditions GetConditions(String Ljdm, String NodeID)
        //{
        //    return new CP_PathEnterJudgeConditions();
        //}
        #region 属性
        /// <summary>
        /// 索引器
        /// </summary>
        /// <param name="xmbh">项目编号</param>
        /// <returns>满足项目编号的进入条件</returns>
        public CP_PathEnterJudgeCondition this[String jcxm]
        {
            get
            {
                CP_PathEnterJudgeCondition Condition = null;
                foreach (CP_PathEnterJudgeCondition c in this)
                {
                    if (c.Jcxm == jcxm)
                        Condition = c;
                }
                return Condition;
            }
        }
        private CP_PathEnterJudgeConditions _ExamConditions;
        public CP_PathEnterJudgeConditions ExamConditions
        {
            get
            {
                if (_ExamConditions == null)
                {
                    _ExamConditions = new CP_PathEnterJudgeConditions();
                    foreach (CP_PathEnterJudgeCondition c in this)
                    {
                        if (c.Xmlb == 1)
                            _ExamConditions.Add(c);
                    }
                }
                return _ExamConditions;
            }
        }
        private CP_PathEnterJudgeConditions _ICD10Conditions;
        public CP_PathEnterJudgeConditions ICD10Conditions
        {
            get
            {
                if (_ICD10Conditions == null)
                {
                    _ICD10Conditions = new CP_PathEnterJudgeConditions();
                    foreach (CP_PathEnterJudgeCondition c in this)
                    {
                        if (c.Xmlb == 2)
                            _ICD10Conditions.Add(c);
                    }
                }
                return _ICD10Conditions;
            }
        }
        #endregion
    }
}
