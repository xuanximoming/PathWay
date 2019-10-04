using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.NursingNotes
{
    /// <summary>
    /// 保存护理记录单实体
    /// </summary>
    [DataContract]
    public class CP_SaveNursingNotes
    {
        private CP_VitalSignsRecordInfo m_CPVitalSignsRecord = new CP_VitalSignsRecordInfo();
        private CP_PatientInOutRecordInfo m_CPPatientInOutRecordIn = new CP_PatientInOutRecordInfo();
        private CP_PatientInOutRecordInfo m_CPPatientInOutRecordOut = new CP_PatientInOutRecordInfo();
        private CP_TreatmentFlowInfo m_CPTreatmentFlow = new CP_TreatmentFlowInfo();
        private CP_VitalSignSpecialRecordInfo m_CPVitalSignSpecialRecord = new CP_VitalSignSpecialRecordInfo();
              
        /// <summary>
        /// 是否保存标示，下标：0 生命特征
        /// </summary>  
        [DataMember]
        public bool bSaveTag1 { get; set; }

        /// <summary>
        /// 是否保存标示，下标：1病人入量
        /// </summary>  
        [DataMember]
        public bool bSaveTag2 { get; set; }

        /// <summary>
        /// 是否保存标示，下标：2 病人出量
        /// </summary>  
        [DataMember]
        public bool bSaveTag3 { get; set; }

        /// <summary>
        /// 是否保存标示，下标：3 主要治疗流程
        /// </summary>  
        [DataMember]
        public bool bSaveTag4 { get; set; }

        /// <summary>
        /// 是否保存标示，下标：4病人特殊记录
        /// </summary>  
        [DataMember]
        public bool bSaveTag5 { get; set; }



        /// <summary>
        /// 生命特征
        /// </summary>
        [DataMember]
        public CP_VitalSignsRecordInfo CPVitalSignsRecord 
        {
            get { return m_CPVitalSignsRecord; }
            set { m_CPVitalSignsRecord = value; }
        }

        /// <summary>
        /// 病人入量
        /// </summary>
        [DataMember]
        public CP_PatientInOutRecordInfo CPPatientInOutRecordIn
        {
            get { return m_CPPatientInOutRecordIn; }
            set { m_CPPatientInOutRecordIn = value; }
        }

        /// <summary>
        /// 病人出量
        /// </summary>
        [DataMember]
        public CP_PatientInOutRecordInfo CPPatientInOutRecordOut
        {
            get { return m_CPPatientInOutRecordOut; }
            set { m_CPPatientInOutRecordOut = value; }
        }

        /// <summary>
        /// 主要治疗流程
        /// </summary>
        [DataMember]
        public CP_TreatmentFlowInfo CPTreatmentFlow
        {
            get { return m_CPTreatmentFlow; }
            set { m_CPTreatmentFlow = value; }
        }

        /// <summary>
        /// 病人特殊记录
        /// </summary>
        [DataMember]
        public CP_VitalSignSpecialRecordInfo CPVitalSignSpecialRecord
        {
            get { return m_CPVitalSignSpecialRecord; }
            set { m_CPVitalSignSpecialRecord = value; }
        }

    }
}