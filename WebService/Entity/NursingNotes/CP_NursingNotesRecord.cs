using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.NursingNotes
{
    /// <summary>
    /// 查询病人基本信息和特殊护理记录实体集合
    /// </summary>
    [DataContract]
    public class CP_NursingNotesRecordCollection
    {
        private List<CP_VitalSignsRecordInfo> m_CP_VitalSignsRecord = new List<CP_VitalSignsRecordInfo>();
        private List<CP_PatientInOutRecordInfo> m_CP_PatientInOutRecordIn = new List<CP_PatientInOutRecordInfo>();
        private List<CP_PatientInOutRecordInfo> m_CP_PatientInOutRecordOut = new List<CP_PatientInOutRecordInfo>();
        private List<CP_TreatmentFlowInfo> m_CP_TreatmentFlow = new List<CP_TreatmentFlowInfo>();
        private List<CP_VitalSignSpecialRecordInfo> m_CP_VitalSignSpecialRecord = new List<CP_VitalSignSpecialRecordInfo>();

        private List<CP_InpatinetList> m_CP_InpatinetList = new List<CP_InpatinetList>();
       
        /// <summary>
        /// 病人基本信息
        /// </summary>
        [DataMember]
        public List<CP_InpatinetList> CP_InpatinetListCollection
        {
            get { return m_CP_InpatinetList; }
            set { m_CP_InpatinetList = value; }
        }

        /// <summary>
        /// 生命体征护理记录
        /// </summary>
        [DataMember]
        public List<CP_VitalSignsRecordInfo> CP_VitalSignsRecordCollection
        {
            get { return m_CP_VitalSignsRecord; }
            set { m_CP_VitalSignsRecord = value; }
        }

        /// <summary>
        /// 病人入量护理记录
        /// </summary>
        [DataMember]
        public List<CP_PatientInOutRecordInfo> CP_PatientInOutRecordInCollection
        {
            get { return m_CP_PatientInOutRecordIn; }
            set { m_CP_PatientInOutRecordIn = value; }
        }

        /// <summary>
        /// 病人出量护理记录
        /// </summary>
        [DataMember]
        public List<CP_PatientInOutRecordInfo> CP_PatientInOutRecordOutCollection
        {
            get { return m_CP_PatientInOutRecordOut; }
            set { m_CP_PatientInOutRecordOut = value; }
        }

        /// <summary>
        /// 病人治疗流程护理记录
        /// </summary>
        [DataMember]
        public List<CP_TreatmentFlowInfo> CP_TreatmentFlowCollection
        {
            get { return m_CP_TreatmentFlow; }
            set { m_CP_TreatmentFlow = value; }
        }

        /// <summary>
        /// 病人特殊护理记录
        /// </summary>
        [DataMember]
        public List<CP_VitalSignSpecialRecordInfo> CP_VitalSignSpecialRecordCollection 
         {
             get { return m_CP_VitalSignSpecialRecord; }
             set { m_CP_VitalSignSpecialRecord = value; }
        }
    }
}