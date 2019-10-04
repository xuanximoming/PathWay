using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

using System.Collections.ObjectModel;

namespace DrectSoft.JobManager
{
   //public delegate void AdjustIntervalEventHander(object sender, IntervalEventArgs e);

   /// <summary>
   /// ͬ���¼�����
   /// </summary>
   public class JobExecuteInfoArgs : EventArgs
   {
      #region properties
      private string _tableName;
      /// <summary>
      /// ����
      /// </summary>
      public string TableName
      {
         get { return _tableName; }
      }

      private int _recordsCount;
      /// <summary>
      /// ��¼����
      /// </summary>
      public int RecordsCount
      {
         get { return _recordsCount; }
      }

      private int _changedCount;
      /// <summary>
      /// �ı�ļ�¼��
      /// </summary>
      public int ChangedCount
      {
         get { return _changedCount; }
      }

      private DateTime _startTime;
      /// <summary>
      /// ��ʼʱ��
      /// </summary>
      public DateTime StartTime
      {
         get { return _startTime; }
      }

      private bool _success;
      /// <summary>
      /// �ɹ���־
      /// </summary>
      public bool Success
      {
         get { return _success; }
      }

      private object _tag;
      /// <summary>
      /// ������Ϣ
      /// </summary>
      public object Tag
      {
         get { return _tag; }
         set { _tag = value; }
      }

      private string _memo;
      /// <summary>
      /// ��ע
      /// </summary>
      public string Memo
      {
         get { return _memo; }
         set { _memo = value; }
      }

      private TraceLevel _level;
      /// <summary>
      /// ����ͬ������Ϣ����
      /// </summary>
      public TraceLevel Level
      {
         get { return _level; }
         set { _level = value; }
      }

      /// <summary>
      /// 
      /// </summary>
      public Job Sender
      {
         get { return _sender; }
      }
      private Job _sender;
      #endregion
      
      /// <summary>
      /// ���ι���
      /// </summary>
      /// <param name="tableName"></param>
      /// <param name="recordsCount"></param>
      /// <param name="changedCount"></param>
      /// <param name="startTime"></param>
      /// <param name="success"></param>
      public JobExecuteInfoArgs(Job sender, string tableName, int recordsCount, int changedCount
         , DateTime startTime, bool success, string memo, TraceLevel level)
      {
         _sender = sender;
         _tableName = string.IsNullOrEmpty(tableName) ? "ϵͳ" : tableName;
         _changedCount = changedCount;
         _recordsCount = recordsCount;
         _startTime = startTime;
         _success = success;
         _memo = memo;
         _level = level;
      }

      /// <summary>
      /// �޲ι���
      /// </summary>
      public JobExecuteInfoArgs(Job sender)
         : this(sender, "����Ϣ��", 0, 0, DateTime.Now, true, "��", TraceLevel.Info)
      { }

      /// <summary>
      /// ��ͨ��Ϣ����
      /// </summary>
      /// <param name="info"></param>
      /// <param name="level"></param>
      public JobExecuteInfoArgs(Job sender, string info, TraceLevel level)
         : this(sender, "����Ϣ��", 0, 0, DateTime.Now, true, info, level)
      { }

      /// <summary>
      /// ר���ڴ�����Ϣ�Ĺ���
      /// </summary>
      /// <param name="error"></param>
      public JobExecuteInfoArgs(Job sender, string error)
         : this(sender, "������", 0, 0, DateTime.Now, false, error, TraceLevel.Error)
      { }

      /// <summary>
      /// ר���ڴ�����Ϣ�Ĺ���(��ʱ������ջ��Ϣ�������¼̫��)
      /// </summary>
      /// <param name="tableName"></param>
      /// <param name="err"></param>
      public JobExecuteInfoArgs(Job sender, string tableName, Exception err)
         : this(sender, "������", 0, 0, DateTime.Now, false,  err.Message
                  + Environment.NewLine + "���ͣ�" + err.TargetSite.DeclaringType
                  + Environment.NewLine + "������" + err.TargetSite.Name
                  + Environment.NewLine + "�������򼯣�" + err.Source
         //+ Environment.NewLine + "��ջ��Ϣ��" + err.StackTrace
         , TraceLevel.Error)
      { }
   }

   //public class EnableEventArgs : EventArgs
   //{
   //   private bool _value;
   //   /// <summary>
   //   /// 
   //   /// </summary>
   //   public bool Value
   //   {
   //      get { return _value; }
   //      set { _value = value; }
   //   }

   //   public EnableEventArgs(bool value)
   //   {
   //      _value = value;
   //   }
   //}

   //public class IntervalEventArgs : EventArgs
   //{
   //   private decimal _interval;
   //   /// <summary>
   //   /// ʱ����ֵ
   //   /// </summary>
   //   public decimal Interval
   //   {
   //      get { return _interval; }
   //      set { _interval = value; }
   //   }

   //   private object _sender;
   //   /// <summary>
   //   /// �¼����Ͳ���
   //   /// </summary>
   //   public object Sender
   //   {
   //      get { return _sender; }
   //      set { _sender = value; }
   //   }
   //}

   //public class SynchTreeViewEventArgs : EventArgs
   //{
   //   private Collection<TreeNode> _selectedNodes;
   //   /// <summary>
   //   /// ����ѡ�����ڵ�
   //   /// </summary>
   //   public Collection<TreeNode> SelectedNodes
   //   {
   //      get { return _selectedNodes; }
   //   }

   //   public SynchTreeViewEventArgs(Collection<TreeNode> selectedNodes)
   //   {
   //      _selectedNodes = selectedNodes;
   //   }
   //}

   //public class ScheduleSettingEventArgs : EventArgs
   //{
   //   /// <summary>
   //   /// 
   //   /// </summary>
   //   public JobPlan Schedule
   //   {
   //      get { return _schedule; }
   //   }
   //   private JobPlan _schedule;

   //   public ScheduleSettingEventArgs(JobPlan schedule)
   //   {
   //      _schedule = schedule;
   //   }
   //}

   public class SearchSettingEventArgs : EventArgs
   {
      private SearchParameter _parameter;
      /// <summary>
      /// ��û������¼�����
      /// </summary>
      public SearchParameter Parameter
      {
         get { return _parameter; }
         set { _parameter = value; }
      }

      public SearchSettingEventArgs(SearchParameter parameter)
      {
         _parameter = parameter;
      }

   }

   public class SearchEventArgs : EventArgs
   {
      private int _index;
      /// <summary>
      /// ����
      /// </summary>
      public int Index
      {
         get { return _index; }
         set { _index = value; }
      }

      public SearchEventArgs(int index)
      {
         _index = index;
      }
   }
}
