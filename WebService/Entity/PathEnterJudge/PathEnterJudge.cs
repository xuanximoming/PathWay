using System;
using System.IO;
namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 病人路径进入判断类，主要包括计算病人归属路径,计算病人是否能进入节点的方法
    /// </summary>
    public class PathEnterJudge : System.Object
    {
        public PathEnterJudge()
        {

        }
        /// <summary>
        /// 计算Patients属性中所有病人能进入的路径
        /// </summary>
        /// <param name="cP_Paths">路径列表，必须给所有路径的下列属性赋值，Ljdm（路径代码）</param>
        /// <param name="patients">病人列表，必须给所有病人的下列属性赋值，，PatientExamItems（检查列表），ICD10（ICD-10诊断）//后面两项已废弃（只有启用适用人群的时候需要）Csrq（出生年月），Brxb（性别）</param>
        public void AnalysisPatientsPath(CP_ClinicalPathLists cP_Paths, CP_InpatinetLists patients)
        {
            foreach (CP_InpatinetList patient in patients)
            {
                foreach (CP_ClinicalPathList path in cP_Paths)
                {
                    path.CanEnter(patient);
                   
                    if (path.IsCanEnter || path.IsPossibleEnter)
                    {
                        patient.Paths.Add(path);
                    }
                }
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="cP_Path">路径，必须给路径的下列属性赋值，Ljdm（路径代码）</param>
        /// <param name="NodeID">节点编码</param>
        /// <param name="patient">病人列表，必须给所有病人的下列属性赋值，Csrq（出生年月），Brxb（性别），PatientExamItems（检查列表），ICD10（ICD-10诊断）</param>
        public void AnalysisPatientsNode(CP_ClinicalPathNode node , CP_InpatinetList patient)
        {

            CP_ClinicalPathNode CP_ClinicalPathNodeTemp = node;
            //CP_ClinicalPathNodeTemp.PathEnterJudgeConditions = CP_PathEnterJudgeConditions.GetConditions(cP_Path.Ljdm, NodeID);
            CP_ClinicalPathNodeTemp.CanEnter(patient);
               

            
            patient.CurrentCP_ClinicalPathNode = CP_ClinicalPathNodeTemp;
        }
    }
}
