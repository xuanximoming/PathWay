using DrectSoft.Core.TimeLimitQC;
using System;
using System.Collections.ObjectModel;
using DrectSoft.JobManager;

[assembly: Job("ʱ���������ݸ���", "���ڸ��²�����дʱ�����", "���Ӳ���", true, typeof(TimeQcManager))]
namespace DrectSoft.JobManager
{
    /// <summary>
    /// ʱ���������ݸ���
    /// </summary>
    public class TimeQcManager : BaseJobAction
    {
        #region ctor
        public TimeQcManager()
        {
        }
        #endregion

        #region public IJobAction ��Ա

        public override void Execute()
        {
            base.SynchState = SynchState.Busy;
            try
            {
                Qcsv qcsv = new Qcsv();
                Collection<QCRuleRecord> records = qcsv.GetRuleRecords(RuleDealType.InnerForTrigger);
                foreach (QCRuleRecord qcrr in records)
                {
                    if (qcrr.Rule == null) continue;
                    if ((qcrr.ConditionTime + qcrr.Rule.Timelimit) > DateTime.Now)
                        qcsv.EffectRuleRecord(qcrr, "00");
                }
            }
            catch
            { throw; }
            finally
            { base.SynchState = SynchState.Stop; }
        }
        #endregion
    }
}
