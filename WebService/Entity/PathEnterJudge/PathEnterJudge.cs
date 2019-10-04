using System;
using System.IO;
namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// ����·�������ж��࣬��Ҫ�������㲡�˹���·��,���㲡���Ƿ��ܽ���ڵ�ķ���
    /// </summary>
    public class PathEnterJudge : System.Object
    {
        public PathEnterJudge()
        {

        }
        /// <summary>
        /// ����Patients���������в����ܽ����·��
        /// </summary>
        /// <param name="cP_Paths">·���б����������·�����������Ը�ֵ��Ljdm��·�����룩</param>
        /// <param name="patients">�����б���������в��˵��������Ը�ֵ����PatientExamItems������б���ICD10��ICD-10��ϣ�//���������ѷ�����ֻ������������Ⱥ��ʱ����Ҫ��Csrq���������£���Brxb���Ա�</param>
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
        /// <param name="cP_Path">·���������·�����������Ը�ֵ��Ljdm��·�����룩</param>
        /// <param name="NodeID">�ڵ����</param>
        /// <param name="patient">�����б���������в��˵��������Ը�ֵ��Csrq���������£���Brxb���Ա𣩣�PatientExamItems������б���ICD10��ICD-10��ϣ�</param>
        public void AnalysisPatientsNode(CP_ClinicalPathNode node , CP_InpatinetList patient)
        {

            CP_ClinicalPathNode CP_ClinicalPathNodeTemp = node;
            //CP_ClinicalPathNodeTemp.PathEnterJudgeConditions = CP_PathEnterJudgeConditions.GetConditions(cP_Path.Ljdm, NodeID);
            CP_ClinicalPathNodeTemp.CanEnter(patient);
               

            
            patient.CurrentCP_ClinicalPathNode = CP_ClinicalPathNodeTemp;
        }
    }
}
