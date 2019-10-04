using System;
using System.Collections.Generic;
using System.Text;
using System.Reflection;
using System.IO;

namespace DrectSoft.JobManager
{
   /// <summary>
   /// ��������
   /// </summary>
   [AttributeUsage(AttributeTargets.All, AllowMultiple = true)]
   public class JobAttribute : Attribute
   {
      #region properties
      /// <summary>
      /// ��������
      /// </summary>
      public string Name
      {
         get { return _name; }
      }
      private string _name;

      /// <summary>
      /// ��������
      /// </summary>
      public string Description
      {
         get { return _description; }
         set { _description = value; }
      }
      private string _description;

      /// <summary>
      /// �������͵�����
      /// </summary>
      public Type StartupType
      {
         get { return _startuType; }
      }
      private Type _startuType;

      /// <summary>
      /// ��ʾ��NAVBAR�ϵ�ͼ��
      /// </summary>
      public string IconName
      {
         get { return _iconName; }
         set { _iconName = value; }
      }
      private string _iconName;

      /// <summary>
      /// ������������ϵͳ
      /// </summary>
      public string SystemName
      {
         get { return _systemName; }
      }
      private string _systemName;

      /// <summary>
      /// �����ڹ��������Ƿ�ɼ�
      /// </summary>
      public bool Visible
      {
         get { return _visible; }
         set { _visible = value; }
      }
      private bool _visible;

      /// <summary>
      /// Ĭ���Ƿ�����
      /// </summary>
      public bool Enabled
      {
         get { return _enabled; }
      }
      private bool _enabled;

      /// <summary>
      /// ��־·��
      /// </summary>
      public string LogDirectory
      {
         get { return _logDirectory; }
         set { _logDirectory = value; }
      }
      private string _logDirectory;
      #endregion

      #region ctors
      /// <summary>
      /// 
      /// </summary>
      /// <param name="missionName">��������</param>
      /// <param name="startupType">����������</param>
      /// <param name="iconName">ͼ������</param>
      /// <param name="systemName">����ϵͳ����</param>
      /// <param name="interval"></param>
      public JobAttribute(string jobName, string description, string systemName, bool enable, Type startupType)
      {
         _name = jobName;
         _description = description;
         _systemName = systemName;
         _enabled = enable;
         _startuType = startupType;
         _visible = true;
      }
      #endregion
   }

   /// <summary>
   /// �������Զ�ȡ��
   /// </summary>
   public class MissionAttributeReader
   {
      private string m_AssemblyName;
      private Assembly _assemblyMission;
      private Assembly AssemblyMission
      {
         get
         {
            if (_assemblyMission == null)
            {
               try
               {
                  _assemblyMission = Assembly.LoadFrom(m_AssemblyName);
               }
               catch (Exception ex)
               {
                  throw new Exception("����Assembly��" + m_AssemblyName + "���󣡴�����Ϣ��" + ex.Message);
               }
            }
            return _assemblyMission;
         }
         set { _assemblyMission = value; }
      }

      /// <summary>
      /// ctor1
      /// </summary>
      /// <param name="assemblyName"></param>
      public MissionAttributeReader(string assemblyName)
      {
         m_AssemblyName = assemblyName;
      }

      /// <summary>
      /// ctor2
      /// </summary>
      /// <param name="domain"></param>
      /// <param name="assemblyName"></param>
      public MissionAttributeReader(AppDomain domain, string assemblyName)
      {
         try
         {
            m_AssemblyName = Path.GetFileNameWithoutExtension(assemblyName);
            if (domain != null)
               AssemblyMission = domain.Load(m_AssemblyName);
            else
               AssemblyMission = AppDomain.CurrentDomain.Load(m_AssemblyName);
         }
         catch (Exception ex)
         {
            throw new Exception("����Assembly��" + m_AssemblyName + "���󣡴�����Ϣ��" + ex.Message);
         }
      }

      /// <summary>
      /// ctor3
      /// </summary>
      /// <param name="assemblyMission"></param>
      public MissionAttributeReader(Assembly assemblyMission)
      {
         AssemblyMission = assemblyMission;
      }

      /// <summary>
      /// ȡ�������������Ϣ
      /// </summary>
      /// <returns></returns>
      [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2200:RethrowToPreserveStackDetails")]
      public JobAttribute[] GetJobAttributes()
      {
         if (AssemblyMission != null)
         {
            try
            {
               return (JobAttribute[])AssemblyMission.GetCustomAttributes(typeof(JobAttribute), false);
            }
            catch (Exception ex)
            {
               throw ex;
            }
         }
         else
         {
            return new JobAttribute[] { };
         }
      }
   }
}
