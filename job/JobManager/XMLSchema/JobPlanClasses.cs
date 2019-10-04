using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Xml.Serialization;

namespace DrectSoft.JobManager
{
   /// <summary>
   /// �����ִ�мƻ�
   /// </summary>
   [SerializableAttribute()]
   public class JobPlan
   {
      #region properties
      /// <summary>
      /// �������ͣ��ظ�ִ�л�ִֻ��һ�Σ�
      /// </summary>
      public JobPlanType JobType
      {
         get { return _jobType; }
         set { _jobType = value; }
      }
      private JobPlanType _jobType;

      /// <summary>
      /// ִ��һ�ε�Datetime����������ΪJustOnceʱ��Ч��
      /// </summary>
      [XmlIgnore()]
      public DateTime DateTimeOfExecOnce
      {
         get { return _dateTimeOfExecOnce; }
         set { _dateTimeOfExecOnce = value; }
      }
      private DateTime _dateTimeOfExecOnce;

      /// <summary>
      /// ��ʽ:yyyy-MM-dd HH:mm:ss
      /// </summary>
      [XmlElementAttribute("DateTimeOfExecOnce")]
      public string XmlDateTimeOfExecOnce
      {
         get { return _dateTimeOfExecOnce.ToString("yyyy-MM-dd HH:mm:ss"); }
         set { _dateTimeOfExecOnce = Convert.ToDateTime(value); }
      }

      /// <summary>
      /// ����ִ��Ƶ��
      /// </summary>
      public JobPlanFrequency Frequency
      {
         get { return _frequency; }
         set { _frequency = value; }
      }
      private JobPlanFrequency _frequency;

      /// <summary>
      /// ÿ��ִ�е�Ƶ������
      /// </summary>
      public JobPlanFrequencyPerDay FrequencyPerDay
      {
         get { return _frequencyPerDay; }
         set { _frequencyPerDay = value; }
      }
      private JobPlanFrequencyPerDay _frequencyPerDay;

      /// <summary>
      /// �������õ�ʱ�䷶Χ
      /// </summary>
      public JobPlanDuration Duration
      {
         get { return _duration; }
         set { _duration = value; }
      }
      private JobPlanDuration _duration;

      /// <summary>
      /// ��Ҫ����ִ��
      /// </summary>
      [XmlIgnore()]
      public bool NeedRunNow
      {
         get { return CheckNeedRunNow(); }
      }

      /// <summary>
      /// �����ϴ�ִ�е�ʱ�䣨�ڶ���ִ��ʱ��ֵ��Ĭ��Ϊ��Сֵ��
      /// </summary>
      [XmlIgnore()]
      public DateTime LastExecuteTime
      {
         get { return _lastExecuteTime; }
         set { _lastExecuteTime = value; }
      }
      private DateTime _lastExecuteTime = DateTime.MinValue;
      #endregion

      #region private methods
      private bool CheckNeedRunNow()
      {
         switch (JobType)
         {
            case JobPlanType.JustOnce:
               return JobPlanFrequencyPerDay.CheckJustOnce(DateTimeOfExecOnce, DateTime.Now, LastExecuteTime);
            case JobPlanType.Repeat:
               return CheckRepeat();
            default:
               return false;
         }
      }

      /// <summary>
      /// ����ظ�ִ�е����
      /// </summary>
      /// <returns></returns>
      private bool CheckRepeat()
      {
         // ���ȼ�鵱ǰ�����Ƿ��ڼƻ�����Ч����
         if (!Duration.CheckTodayIsInDuration())
            return false;
         // ��μ�鵱���Ƿ���Ҫִ��
         if (!Frequency.CheckTodayIsInPlan(Duration.StartDate))
            return false;
         // ������Ƿ��˵����ִ��ʱ���
         return FrequencyPerDay.CheckNowIsInPlan(LastExecuteTime);
      }
      #endregion
   }

   /// <summary>
   /// ����ִ��Ƶ��
   /// </summary>
   [SerializableAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public class JobPlanFrequency
   {
      #region properties
      /// <summary>
      /// ִ�м������λ��ִ���������;���
      /// </summary>
      public int Interval
      {
         get { return _interval; }
         set { _interval = value; }
      }
      private int _interval;

      /// <summary>
      /// ִ�����ڵĵ�λ��3���죻4����
      /// </summary>
      public JobExecIntervalUnit IntervalUnit
      {
         get { return _intervalUnit; }
         set { _intervalUnit = value; }
      }
      private JobExecIntervalUnit _intervalUnit;

      /// <summary>
      /// ���е�������ϣ���0/1����ɵ���λ�ַ������������յ���������1��ʾ���ã�0��ʾ�����ã�ִ������Ϊ��ʱ����Ч
      /// </summary>
      public string WeekDays
      {
         get { return _weekDays; }
         set { _weekDays = value; }
      }
      private string _weekDays;
      #endregion

      #region public methods
      /// <summary>
      /// ��鵱���Ƿ���ִ�мƻ���
      /// </summary>
      /// <param name="planStartDate">�ƻ���ʼ����</param>
      /// <returns></returns>
      public bool CheckTodayIsInPlan(DateTime planStartDate)
      {
         if (DateTime.Now.Date < planStartDate.Date)
            return false;
         //

         if (IntervalUnit == JobExecIntervalUnit.Day)
            return CheckTodayIsInCirclePlan(planStartDate);
         else if (IntervalUnit == JobExecIntervalUnit.Week)
            return CheckTodayIsInWeekDays(planStartDate);
         else
            return false;
      }
      #endregion

      #region private methods
      private bool CheckTodayIsInCirclePlan(DateTime planStartDate)
      {
         TimeSpan diff = DateTime.Now.Date - planStartDate;
         return ((diff.TotalDays % Interval) == 0);
      }

      private bool CheckTodayIsInWeekDays(DateTime planStartDate)
      {
         if ((String.IsNullOrEmpty(WeekDays)) || (WeekDays.Length != 7))
            return false;

         TimeSpan diff = DateTime.Now.Date - planStartDate;
         if (((diff.TotalDays / 7) % Interval) == 0)
         {
            int weekDay = (int)DateTime.Now.DayOfWeek;
            return (WeekDays[weekDay - 1] == '1');
         }
         else
            return false;
      }
      #endregion
   }

   /// <summary>
   /// ÿ��ִ��ʱ��Ƶ������
   /// </summary>
   [SerializableAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public class JobPlanFrequencyPerDay
   {
      /// <summary>
      /// ʱ�����������룩�����ж��Ƿ���Ԥ��ִ��ʱ��ʱ��Ҫ����������
      /// </summary>
      private const int s_RedundancyTime = 600;

      #region properties
      /// <summary>
      /// �ظ�ִ�У�Ϊ��ʱÿ��ִֻ��һ��
      /// </summary>
      public bool Repeatly
      {
         get { return _repeatly; }
         set { _repeatly = value; }
      }
      private bool _repeatly;

      /// <summary>
      /// ִ��һ�ε�ʱ���
      /// </summary>
      [XmlIgnore()]
      public TimeSpan TimeOfExecOnce
      {
         get { return _timeOfExecOnce; }
         set { _timeOfExecOnce = value; }
      }
      private TimeSpan _timeOfExecOnce;

      /// <summary>
      /// ��ʽ:HH:mm:ss
      /// </summary>
      [XmlElementAttribute("TimeOfExecOnce")]
      public string XmlTimeOfExecOnce
      {
         get { return ((DateTime)(DateTime.Now.Date + _timeOfExecOnce)).ToString("HH:mm:ss"); }
         set { _timeOfExecOnce = Convert.ToDateTime(value).TimeOfDay; }
      }

      /// <summary>
      /// �ظ�ִ�е�ʱ����
      /// </summary>
      [CLSCompliant(false)]
      public uint Interval
      {
         get { return _interval; }
         set { _interval = value; }
      }
      private uint _interval;

      /// <summary>
      /// �ظ�ִ�е�ʱ������λ
      /// </summary>
      public JobExecIntervalUnit IntervalUnit
      {
         get { return _intervalUnit; }
         set { _intervalUnit = value; }
      }
      private JobExecIntervalUnit _intervalUnit;

      /// <summary>
      /// ����Ƶ�ʱ����
      /// </summary>
      [XmlIgnore()]
      [CLSCompliant(false)]
      public uint IntervalInSeconds
      {
         get
         {
            if (IntervalUnit == JobExecIntervalUnit.Minute)
               return Interval * 60;
            else if (IntervalUnit == JobExecIntervalUnit.Hour)
               return Interval * 3600;
            else
               return 0;
         }
      }

      /// <summary>
      /// �ظ�ִ�п�ʼ��ʱ��,
      /// </summary>
      [XmlIgnore()]
      public TimeSpan StartTime
      {
         get { return _startTime; }
         set { _startTime = value; }
      }
      private TimeSpan _startTime;

      /// <remark/>
      [XmlElementAttribute("StarTime")]
      public string XmlStarTime
      {
         get { return ((DateTime)(DateTime.Now.Date + _startTime)).ToString("HH:mm:ss"); }
         set { _startTime = Convert.ToDateTime(value).TimeOfDay; }
      }

      /// <summary>
      /// �ظ�ִ�н�����ʱ��,��ʽ:HH:mm:ss
      /// </summary>
      [XmlIgnore()]
      public TimeSpan EndTime
      {
         get { return _endTime; }
         set { _endTime = value; }
      }
      private TimeSpan _endTime;

      /// <summary>
      /// ��ʽ:HH:mm:ss
      /// </summary>
      [XmlElementAttribute("EndTime")]
      public string XmlEndTime
      {
         get
         {
            return ((DateTime)(DateTime.Now.Date + _endTime)).ToString("HH:mm:ss");
         }
         set { _endTime = Convert.ToDateTime(value).TimeOfDay; }
      }
      #endregion

      #region public methods
      public bool CheckNowIsInPlan(DateTime lastExecuteTime)
      {
         if (Repeatly)
            return CheckTimeIsInCirclePlan(lastExecuteTime);
         else
            return CheckJustOnce(DateTime.Now.Date + TimeOfExecOnce, DateTime.Now, lastExecuteTime);
      }

      /// <summary>
      /// ���ִֻ��һ�ε����
      /// </summary>
      /// <param name="plannedTime">Ԥ���ļƻ�ִ��ʱ���</param>
      /// <param name="executeTime">Ҫ����ִ��ʱ���</param>
      /// <param name="lastExecuteTime">�ϴ�ִ��ʱ��</param>
      /// <returns></returns>
      public static bool CheckJustOnce(DateTime plannedTime, DateTime executeTime, DateTime lastExecuteTime)
      {
         if (executeTime < plannedTime) // ��û�е�Ԥ����ִ��ʱ���
            return false;
         else if (executeTime == plannedTime) // �պ���Ԥ����ִ��ʱ���
            return true;
         else
         {
            TimeSpan diff = executeTime - plannedTime;
            // ����ִ��ʱ�п�����Ϊ�����ٶ����⣬û����ָ������һ��������㣬
            // ���Ի�Ӧ��һЩ���������Ա���ִ�е�����ٲ���
            //    ����ϴ�ִ��ʱ���ڼƻ�ʱ��֮ǰ��˵������û�а��ƻ�ִ�У���Ҫ����
            if ((diff.TotalSeconds < s_RedundancyTime) && (lastExecuteTime < plannedTime))
                  return true;
            else
               return false;
         }
      }
      #endregion

      #region private methods
      private bool CheckTimeIsInCirclePlan(DateTime lastExecuteTime)
      {
         TimeSpan now = DateTime.Now.TimeOfDay;

         // ���ȼ�鵱ǰʱ����Ƿ�������ʱ�䷶Χ��
         if ((now.TotalSeconds < StartTime.TotalSeconds) || (now.TotalSeconds > EndTime.TotalSeconds))
            return false;

         TimeSpan diff;
         // ����Ƿ���Ԥ����ִ��ʱ���
         diff = now - StartTime;
         if ((diff.TotalSeconds % IntervalInSeconds) == 0)
            return true;
         else if (DateTime.Today > lastExecuteTime.Date) // �ϴ�ִ��ʱ���ڽ���֮ǰ
            return true;
         else
         {
            // ����ϴ�ִ��ʱ���뵱ǰʱ����ʱ������������ִ��
            diff = now - lastExecuteTime.TimeOfDay;
            if (diff.TotalSeconds >= IntervalInSeconds)
               return true;
            else
               return false;
         }
      }
      #endregion
   }

   /// <summary>
   /// ����ʱ�䣨�������õ�ʱ�䷶Χ��
   /// </summary>
   [SerializableAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public class JobPlanDuration
   {
      #region properties
      /// <summary>
      /// ����ʼִ�е�����
      /// </summary>
      [XmlIgnore()]
      public DateTime StartDate
      {
         get { return _startDate; }
         set { _startDate = value; }
      }
      private DateTime _startDate;

      /// <summary>
      /// ��ʽ:yyyy-MM-dd
      /// </summary>
      [XmlElementAttribute("StartDate")]
      public string XmlStartDate
      {
         get { return _startDate.ToString("yyyy-MM-dd"); }
         set { _startDate = Convert.ToDateTime(value); }
      }

      /// <summary>
      /// ����ִֹͣ�е�����
      /// </summary>
      [XmlIgnore()]
      public DateTime EndDate
      {
         get { return _endDate; }
         set { _endDate = value; }
      }
      private DateTime _endDate;

      /// <summary>
      /// ��ʽ:yyyy-MM-dd
      /// </summary>
      [XmlElementAttribute("EndDate")]
      public string XmlEndDate
      {
         get { return _endDate.ToString("yyyy-MM-dd"); }
         set { _endDate = Convert.ToDateTime(value); }
      }

      /// <summary>
      /// �Ƿ��н�������
      /// </summary>
      public bool HasEndDate
      {
         get { return _hasEndDate; }
         set { _hasEndDate = value; }
      }
      private bool _hasEndDate;
      #endregion

      #region public methods
      /// <summary>
      /// ��鵱���Ƿ������ڷ�Χ������(���Ƚ����ڲ���)
      /// </summary>
      /// <returns></returns>
      public bool CheckTodayIsInDuration()
      {
         DateTime date = DateTime.Now;
         if (date.Date < StartDate.Date)
            return false;
         if (HasEndDate && (date.Date > EndDate.Date))
            return false;
         return true;
      }
      #endregion
   }
}