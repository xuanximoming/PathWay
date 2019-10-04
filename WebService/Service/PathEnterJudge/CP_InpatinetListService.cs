using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using Yidansoft.Service.Entity;
using System.Collections;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Configuration;
using DrectSoft.Tool;
namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 分析病人可进入的路径
        /// </summary>
        /// <param name="patient">病人实体</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public CP_InpatinetList GetAnalysePatient_CanEnterPath(CP_InpatinetList patient)
        {
            try
            {
                patient.ICD10 = patient.RyzdCode;
                patient.InitializePatinetExamItems();

                CP_InpatinetLists patients = new CP_InpatinetLists();
                patients.Add(patient);

                CP_ClinicalPathLists Paths = new CP_ClinicalPathLists();
                YidanEHRDataService service = new YidanEHRDataService();
                Paths.AddRange(service.GetValidClinicalPathList());

                
                Paths.InitializePathsEnterConditions();
                PathEnterJudge PathEnterJudgeTemp = new PathEnterJudge();
                PathEnterJudgeTemp.AnalysisPatientsPath(Paths, patients);
           
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return patient;
        }

        /// <summary>
        /// 分析病人列表可进入的路径
        /// </summary>
        /// <param name="patients">病人实体列表</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_InpatinetList> GetAnalysePatients_CanEnterPath(List<CP_InpatinetList> patients)
        {

            foreach (var patient in patients)
            {
                GetAnalysePatient_CanEnterPath(patient);
            }
            return patients;
        }


        /// 分析病人可进入的节点
        /// </summary>
        /// <param name="patient">病人实体</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public CP_InpatinetList GetAnalysePatient_CanEnterNode(CP_InpatinetList patient, String ljdm, String nodeCode)
        {
            try
            {
                patient.ICD10 = patient.RyzdCode;
                patient.InitializePatinetExamItems();

                //CP_InpatinetLists patients = new CP_InpatinetLists();
                //patients.Add(patient);

                CP_ClinicalPathNode PathNode = new CP_ClinicalPathNode();
                PathNode.Ljdm = ljdm;
                PathNode.NodeCode = nodeCode;
                PathNode.InitializeNodeEnterConditions();
                PathEnterJudge PathEnterJudgeTemp = new PathEnterJudge();
                PathEnterJudgeTemp.AnalysisPatientsNode(PathNode, patient);

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return patient;
        }
    }
}
