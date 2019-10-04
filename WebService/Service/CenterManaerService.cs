using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ServiceModel;
using Yidansoft.Service.Entity;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 用户中心
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public UserCenterManagerInfo GetUserCenterManagerInfo(string zgdm, string rwzt, string rwsj, int audit, string days, int querytype)
        {
            UserCenterManagerInfo userCenterManagerInfo = new UserCenterManagerInfo();

            userCenterManagerInfo.V_PatientExamineList = GetV_PatientExamineList();
            userCenterManagerInfo.CP_DoctorTaskMessageList = GetDoctorTaskMessage(zgdm, rwzt, rwsj);
            userCenterManagerInfo.CP_QCProblemList = GetQCProblemList(audit,days,querytype);

            return userCenterManagerInfo;
        }
        /// <summary>
        /// 质控中心
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public InterventionCenterInfo GetInterventionCenterInfo(string BeyondDays, string ForceToPathstartDate, string ForceToPathendDate, string PathExitstartDate, 
            string PathExitendDate, int audit, string days, int querytype, string QCRecordTipstartDate, string QCRecordTipendDate) 
        {
            InterventionCenterInfo interventionCenterInfo = new InterventionCenterInfo();

            interventionCenterInfo.CP_BeyondDaysList = GetBeyondDaysList(BeyondDays);
            interventionCenterInfo.CP_BeyondFeeList = GetBeyondFeeList();
            interventionCenterInfo.CP_ForceToPathList = GetForceToPathList(ForceToPathstartDate, ForceToPathendDate);
            interventionCenterInfo.CP_PathExitList = GetPathExitList(PathExitstartDate, PathExitendDate);
            interventionCenterInfo.CP_QCProblemList = GetQCProblemList(audit,days,querytype);
            interventionCenterInfo.CP_QCRecordTipList = GetQCRecordTipList(QCRecordTipstartDate,QCRecordTipendDate);

            return interventionCenterInfo;
        }
    }

    /// <summary>
    /// 用户中心
    /// </summary>
    public class UserCenterManagerInfo
    {
        /// <summary>
        /// 病历时限信息（提示）
        /// </summary>
        public List<V_QCRecord> V_QCRecordList1
        {
            get;
            set;
        }
        /// <summary>
        /// 病历时限信息（警告）
        /// </summary>
        public List<V_QCRecord> V_QCRecordList2
        {
            get;
            set;
        }
        /// <summary>
        /// 分管病人检验报告数据
        /// </summary>
        public List<V_PatientExamine> V_PatientExamineList
        {
            get;
            set;
        }
        /// <summary>
        /// 医师任务信息
        /// </summary>
        public List<CP_DoctorTaskMessage> CP_DoctorTaskMessageList
        {
            get;
            set;
        }
        /// <summary>
        /// 医生异常问题处理信息
        /// </summary>
        public List<CP_QCProblem> CP_QCProblemList
        {
            get;
            set;
        }
    }
    /// <summary>
    /// 指控中心
    /// </summary>
    public class InterventionCenterInfo 
    {
        /// <summary>
        ///病历超时信息
        /// </summary>
        public List<CP_QCRecordTip> CP_QCRecordTipList
        {
            get;
            set;
        }
        /// <summary>
        /// 住院天数超标
        /// </summary>
        public List<CP_BeyondDays> CP_BeyondDaysList
        {
            get;
            set;
        }
        /// <summary>
        /// 住院费用超标
        /// </summary>
        public List<CP_BeyondFee> CP_BeyondFeeList
        {
            get;
            set;
        }
        /// <summary>
        /// 强制入径
        /// </summary>
        public List<CP_ForceToPath> CP_ForceToPathList
        {
            get;
            set;
        }
        /// <summary>
        /// 中途退出
        /// </summary>
        public List<CP_PathExit> CP_PathExitList
        {
            get;
            set;
        }
        /// <summary>
        /// 质控管理
        /// </summary>
        public List<CP_QCProblem> CP_QCProblemList
        {
            get;
            set;
        }
    }
}