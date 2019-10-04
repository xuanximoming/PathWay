using DrectSoft.Core.TimeLimitQC;
using System;
using System.Collections.ObjectModel;
using DrectSoft.JobManager;

[assembly: Job("时限质量数据更新", "定期更新病历书写时限情况", "电子病历", true, typeof(TimeQcManager))]
namespace DrectSoft.JobManager
{
    /// <summary>
    /// 时限质量数据更新
    /// </summary>
    public class TimeQcManager : BaseJobAction
    {
        #region ctor
        public TimeQcManager()
        {
        }
        #endregion

        #region public IJobAction 成员

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
