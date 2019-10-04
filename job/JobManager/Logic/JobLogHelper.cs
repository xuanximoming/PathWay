using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;
using System.Data;
using System.IO;
using System.Windows.Forms;
using System.Runtime.Serialization;

namespace DrectSoft.JobManager
{
   /// <summary>
   /// ���ڶ�д���񵥶���־�ļ�����־���ߣ���ͬ�ڿ�ܵ���ʹ�õ���־���ߣ����ʹ�õ���EMR��ܵ���־����
   /// </summary>
   public static class JobLogHelper
   {
      #region properties & variables
      /// <summary>
      /// �����־���ؿ�������Ϣ
      /// </summary>
      public static TraceSwitch DataSynchSwitch
      {
         get
         {
            if (_dataSynchSwitch == null)
               _dataSynchSwitch = new TraceSwitch("DataLogSwitch", "��־���ؿ���", "4");
            return _dataSynchSwitch;
         }
      }
      private static TraceSwitch _dataSynchSwitch;

      private static DataTable LogDataTable
      {
         get
         {
            if (_logDataTable == null)
               _logDataTable = GetLogTableTemplate();
            return _logDataTable;
         }
      }
      private static DataTable _logDataTable;
      #endregion

      #region //ctor
      //public JobLogHelper(JobManager manager)
      //{
      //   m_SynchManager = manager;
      //   m_LogDataTable = GetLogTableTemplate();
      //}
      #endregion

      #region public methods
      /// <summary>
      /// ȡ�������־·������
      /// </summary>
      /// <param name="job"></param>
      /// <returns></returns>
      public static string GetJobLogPath(Job job)
      {
         if (job == null)
            return Application.StartupPath;
         if (String.IsNullOrEmpty(job.LogDirectory))
            return Application.StartupPath + @"\AppLog\" + job.Name + @"\";
         else
            return job.LogDirectory;
      }

      /// <summary>
      /// ��ȡ����ǰʱ����Ӧ����־�ļ�ȫ·��
      /// </summary>
      /// <param name="job"></param>
      /// <returns></returns>
      public static string GetJobLogCurrentFullPath(Job job)
      {
         return EnsureLogFile(job);
      }

      /// <summary>
      /// 
      /// </summary>
      /// <param name="missionName"></param>
      /// <param name="logTable"></param>
      /// <param name="frame"></param>
      /// <param name="logPath"></param>
      /// <param name="level"></param>
      public static void WriteLog(JobExecuteInfoArgs args)
      {
         WriteToFile(args);
      }

      /// <summary>
      /// ������־�ļ�����ת����Ӧ�����ݼ�
      /// </summary>
      /// <param name="file"></param>
      /// <returns></returns>
      public static DataTable LoadLogFileToTable(string file)
      {
         if (string.IsNullOrEmpty(file))
            return null;
         try
         {
            DataTable dt = LogDataTable.Clone();
            if (!File.Exists(file))
               return dt;
            dt.Columns[DefinedDataColumn.LogSource].DefaultValue = file;

            StreamReader reader = new StreamReader(file, Encoding.UTF8);
            string temp;
            while (!string.IsNullOrEmpty(temp = reader.ReadLine()))
            {
               string[] array = temp.Split(new string[] { "\t" }, StringSplitOptions.RemoveEmptyEntries);
               if (array.Length > 0)
                  ConvertToDataRow(array, dt);
            }
            reader.Close();
            return dt;
         }
         catch (Exception ex)
         {
            MessageBox.Show(ex.Message, "����", MessageBoxButtons.OK, MessageBoxIcon.Error);
            return null;
         }
      }
      #endregion

      #region private write log methods
      /// <summary>
      /// д��GridControl�е���־��������
      /// </summary>
      /// <param name="missionName"></param>
      /// <param name="grid"></param>
      /// <param name="logTable"></param>
      private static void WriteToFile(JobExecuteInfoArgs args)
      {
         if ((args == null) || (args.Sender == null))
            return;

         try
         {
            // ����־�ļ������û�еĻ��򴴽���
            string logFileFullPath = EnsureLogFile(args.Sender);

            // д������
            WriteContextToFile(args, logFileFullPath);
         }
         catch
         { }
      }

      private static string EnsureLogFile(Job job)
      {
         if (job == null)
            return String.Empty;

         string path = GetJobLogPath(job);

         if (!Directory.Exists(path))
            Directory.CreateDirectory(path);

         return path + GetLogFileName(job) + ".log";
      }

      private static void WriteContextToFile(JobExecuteInfoArgs args, string logFileFullPath)
      {
         TextWriter textWrite = new StreamWriter(logFileFullPath, true);
         try
         {
            textWrite.WriteLine(FormatMessage(args.TableName
                  , args.RecordsCount
                  , args.ChangedCount
                  , args.StartTime
                  , args.Success
                  , args.Memo.Replace("\n", string.Empty).Replace("\r", string.Empty)));
         }
         catch { }
         finally
         {
            textWrite.Close();
         }
      }
      #endregion

      #region private method
      private static void ConvertToDataRow(string[] array, DataTable target)
      {
         if (array == null || array.Length == 0 || target == null)
            return;
         DataRow row = target.NewRow();
         //ʱ��	�Ƿ�ɹ�	[��ע]  ����	 ��¼����  ��¼�ı���		
         if (array.Length > 0)
            row[DefinedDataColumn.LogDate] = array[0].Trim();
         if (array.Length > 1)
            row[DefinedDataColumn.Success] = Convert.ToBoolean(array[1]);
         if (array.Length > 2)
            row[DefinedDataColumn.Message] = array[2].Trim();
         if (array.Length > 3)
            row[DefinedDataColumn.Source] = array[3].Trim();
         if (array.Length > 4)
            row[DefinedDataColumn.Count] = int.Parse(array[4]);
         if (array.Length > 5)
            row[DefinedDataColumn.ChangedCount] = int.Parse(array[5]);
         target.Rows.Add(row);
      }

      private static string FormatMessage(string tableName, int recordCount, int changedCount, DateTime time, bool success, string memo)
      {
         return String.Format("{0}\t{1, -5}\t[{2}]\t{3, -20}\t{4, -5}\t{5, -5}", time.ToString("yy-M-d H:mm:ss ffff"), success, memo.Trim(), tableName, recordCount, changedCount);
      }

      private static DataTable GetLogTableTemplate()
      {
         DataTable dt = new DataTable();
         dt.Columns.Add(DefinedDataColumn.Source, typeof(string));
         dt.Columns.Add(DefinedDataColumn.Count, typeof(int));
         dt.Columns[DefinedDataColumn.Count].DefaultValue = 0;
         dt.Columns.Add(DefinedDataColumn.ChangedCount, typeof(int));
         dt.Columns[DefinedDataColumn.ChangedCount].DefaultValue = 0;
         dt.Columns.Add(DefinedDataColumn.LogDate, typeof(string));
         dt.Columns.Add(DefinedDataColumn.Success, typeof(bool));
         dt.Columns[DefinedDataColumn.Success].DefaultValue = true;
         dt.Columns.Add(DefinedDataColumn.Message, typeof(string));
         dt.Columns.Add(DefinedDataColumn.LogSource, typeof(string));
         return dt;
      }

      private static string GetLogFileName(Job job)
      {
         // ���ִֻ��һ�Σ����ԡ���������������
         // ���ÿ��������ִ�У����ԡ�������+��ǰ���ڡ�������
         // ���򶼰���������+��ǰ�¡�������
         if (job.JobSchedule.JobType == JobPlanType.JustOnce)
            return job.Name;
         else if (job.JobSchedule.FrequencyPerDay.Repeatly)
            return job.Name + DateTime.Now.ToString("yyyy-MM-dd");
         else
            return job.Name + DateTime.Now.ToString("yyyy-MM");
      }
      #endregion
   }

   [Serializable]
   public class DataSynchException : Exception, ISerializable
   {

      #region ctor
      public DataSynchException()
      { }

      public DataSynchException(string message)
         : base(message)
      { }

      public DataSynchException(string message, Exception inner)
         : base(message, inner)
      { }

      #endregion

      #region ISerializable ��Ա

      void ISerializable.GetObjectData(SerializationInfo info, StreamingContext context)
      {
         base.GetObjectData(info, context);
      }

      #endregion
   }

   public class InterfaceReturnNoDataException : DataSynchException
   {
      internal const string ExceptionMessage = "�ӿ�δ��������";
      #region ctor
      public InterfaceReturnNoDataException()
         : base(ExceptionMessage)
      { }
      #endregion
   }
}
