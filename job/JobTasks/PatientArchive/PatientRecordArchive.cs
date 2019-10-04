using DrectSoft.Core;
using System;
using System.Data;
using System.Diagnostics;
using DrectSoft.JobManager;

[assembly: Job("病历自动归档", "对已出院病人的病历自动归档", "电子病历", false, typeof(PatientRecordArchive))]
namespace DrectSoft.JobManager
{
    /// <summary>
    /// 病历自动归档
    /// </summary>
    public class PatientRecordArchive : BaseJobAction
    {
        private const string s_AutoArchiveTimeSettingKey = "AutoArchiveTimeSetting";
        //对可归档的病历文件进行归档的SQL语句      
        private const string SQL_UpdateCanArchiveEpr = "update RECORDDETAIL a set ISLOCK = 4701 "
           + " Where exists(select 1 from InPatient b  where a.NoOfInpat = b.NoOfInpat and b.OutWardDate  <= '{0}' and  b.Status in(1502,1503) and (a.ISLOCK in(0,4700)) )";

        #region new properties
        /// <summary>
        /// 有自己的配置参数
        /// </summary>
        public override bool HasPrivateSettings { get { return true; } }
        #endregion

        #region Var
        private IDataAccess m_SqlHelperTarget;
        private ISynchApplication m_App;
        private int m_TimeSetting;
        #endregion

        #region Ctor
        public PatientRecordArchive()
        {
            m_SqlHelperTarget = DataAccessFactory.DefaultDataAccess;
            InitArchiveSetting();
        }
        #endregion

        #region private Methods
        private void ArchiveRecords()
        {
            int rowCount = 0;
            try
            {
                DateTime archiveDate = DateTime.Now.AddDays(-m_TimeSetting);
                m_SqlHelperTarget.ExecuteNoneQuery(
                  String.Format(SQL_UpdateCanArchiveEpr, archiveDate.ToString("yyyy-MM-dd 24")), CommandType.Text);

            }
            catch (Exception ex)
            {
                JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, "RECORDDETAIL", ex));
            }
            JobLogHelper.WriteLog(new JobExecuteInfoArgs(Parent, "RECORDDETAIL", rowCount, rowCount, DateTime.Now, true, String.Empty, TraceLevel.Info));
        }

        private void InitArchiveSetting()
        {
            string config = BasicSettings.GetStringConfig(s_AutoArchiveTimeSettingKey);
            m_TimeSetting = (int)Convert.ToDecimal(config);
            if (m_TimeSetting <= 0)
                m_TimeSetting = 3;
        }
        #endregion

        #region public IJobAction 成员

        public override void ExecuteDataInitialize()
        {
            Execute();
        }

        public override void Execute()
        {
            base.SynchState = SynchState.Busy;
            try
            {
                ArchiveRecords();
            }
            catch
            { throw; }
            finally
            { base.SynchState = SynchState.Stop; }
        }

        public override void RefreshPrivateSettings()
        {
            InitArchiveSetting();
        }
        #endregion
    }
}
