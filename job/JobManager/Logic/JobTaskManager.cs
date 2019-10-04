using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.ObjectModel;
using System.ComponentModel;

using System.Reflection;
using System.IO;
using System.Diagnostics;
using System.Collections;
using System.Xml.Serialization;
using System.Threading;
using DrectSoft.Core;

namespace DrectSoft.JobManager
{
    /// <summary>
    /// ������������������߼�������
    /// </summary>
    public class JobTaskManager
    {

        #region properties



        /// <summary>
        /// �����ļ��ж����ϵͳ����
        /// </summary>
        public JobConfig Systems
        {
            get
            {
                if (_systems == null)
                    InitializeConfig();
                return _systems;
            }
        }
        private JobConfig _systems;

        /// <summary>
        /// ���е�����
        /// </summary>
        public Collection<Job> AllJobs
        {
            get
            {
                if (_allJobs == null)
                    InitializeConfig();
                return _allJobs;
            }
        }
        private Collection<Job> _allJobs;
        #endregion

        #region fields
        private JobDespatch m_MissionDespatch;
        private const string m_JobtaskInfo = "JobTaskConfig";
        #endregion

        #region ctor
        public JobTaskManager()
        {
            // ȷ��EMR�����ݿ��Ѿ����������򴴽������ʧ��
            //TestSqlServiceHadStarted();

            // ��������������Ϣ����ÿ�������Actionʵ��
            CreateJobAction();
        }
        #endregion

        #region public & internal method

        public void WriteAllLog()
        {
            foreach (Job job in AllJobs)
            {
                JobLogHelper.WriteLog(new JobExecuteInfoArgs(job, "��Ӧ�ó���ر�", TraceLevel.Info));
            }
        }

        /// <summary>
        /// ��¼��־
        /// </summary>
        /// <param name="e"></param>
        public void WriteLog(JobExecuteInfoArgs e)
        {
            JobLogHelper.WriteLog(e);
        }

        /// <summary>
        /// �����������޸�
        /// </summary>
        public void SaveJobConfig()
        {
            if (Systems != null)
            {
                FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\" + "DataSynchConfig.xml", FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.ReadWrite);
                XmlSerializer serializer = new XmlSerializer(typeof(JobConfig));
                serializer.Serialize(file, Systems);
                file.Close();
            }
        }
        #endregion

        #region private methods
        private void InitializeConfig()
        {
            //Stream file = BasicSettings.GetConfig(m_JobtaskInfo);
            //XmlSerializer serializer = new XmlSerializer(typeof(JobConfig));
            //_systems = (JobConfig)serializer.Deserialize(file);
            //file.Close();
            FileStream file = new FileStream(AppDomain.CurrentDomain.BaseDirectory + @"\" + "DataSynchConfig.xml", FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            XmlSerializer serializer = new XmlSerializer(typeof(JobConfig));
            _systems = (JobConfig)serializer.Deserialize(file);
            file.Close();

            _allJobs = new Collection<Job>();
            foreach (SystemsJobDefine system in _systems.JobsOfSystem)
            {
                foreach (Job job in system.Jobs)
                    _allJobs.Add(job);
            }
        }

        public void RegisterMissions()
        {

            m_MissionDespatch = new JobDespatch(this);
            m_MissionDespatch.Start();
        }


        public void StopMissions()
        {
            if (m_MissionDespatch != null)
                m_MissionDespatch.Stop();
        }

        /// <summary>
        /// ����sql�����Ƿ��Ѿ�����
        /// </summary>
        /// <returns></returns>
        private void TestSqlServiceHadStarted()
        {
            int testTimes = 60;
            do
            {
                try
                {
                    IDataAccess sqlHelp = DataAccessFactory.DefaultDataAccess;
                    sqlHelp.ExecuteDataTable("select * from Users where 1=2");
                    return;
                }
                catch
                {
                    testTimes--;
                    Thread.Sleep(30000); // 30����һ��
                }
            } while (testTimes > 0);

            throw new ApplicationException("�޷��������ݿ�");
        }

        private void CreateJobAction()
         {
            for (int i = AllJobs.Count - 1; i >= 0; i--)
            {
                AllJobs[i].Action = CreateActionInstance(AllJobs[i].Class, AllJobs[i].Library);
                if (AllJobs[i].Action != null)
                    AllJobs[i].Action.Parent = AllJobs[i];
            }
        }

        private IJobAction CreateActionInstance(string className, string assemblyName)
        {
            try
            {
                Assembly assembly = Assembly.Load(Path.GetFileNameWithoutExtension(assemblyName));
                Type actionType = assembly.GetType(className, true, true);

                return Activator.CreateInstance(actionType) as IJobAction;
            }
             catch
            {
                return null;
            }
        }

        #endregion


        #region //�������������

        //private int MissionIndexInLoaded(string assemblyName, string startupClassName)
        //{
        //   return MissionIndexInLoaded(assemblyName, startupClassName, _loadedMissions);
        //}

        //private int MissionIndexInLoaded(string assemblyName, string startupClassName, Collection<ISynchMission> loaded)
        //{
        //   int foundIndex = -1;
        //   if (loaded == null)
        //      return foundIndex;

        //   for (int i = 0; i < loaded.Count; i++)
        //   {
        //      if (string.Compare(assemblyName, (loaded[i]).AssemblyFileName, true) == 0)
        //      {
        //         if (startupClassName == (loaded[i]).StartClassType)
        //         {
        //            foundIndex = i;
        //            break;
        //         }
        //         else
        //         {
        //            string[] temp = startupClassName.Split(new string[] { "." }, StringSplitOptions.RemoveEmptyEntries);
        //            if (temp != null && temp.Length > 0)
        //            {
        //               if (loaded[i].StartClassType.Contains(temp[temp.Length - 1]))
        //               {
        //                  foundIndex = i;
        //                  break;
        //               }
        //            }
        //         }
        //      }
        //   }
        //   return foundIndex;
        //}

        //public ISynchMission LoadMission(string missionName, string assemblyName, string startupClassName)
        //{
        //   Type startupType;
        //   if (!this.ValidateMission(assemblyName, startupClassName, out startupType))
        //      return null;

        //   ISynchMission mission;
        //   int foundindex = this.MissionIndexInLoaded(assemblyName, startupClassName);
        //   if (foundindex < 0)
        //   {
        //      mission = BuildMission(assemblyName, startupClassName);
        //      RunMission(mission);
        //   }
        //   else
        //      mission = this._loadedMissions[foundindex] as ISynchMission;
        //   return mission;
        //}
        #endregion

        #region //ֹͣ����
        //public void StopMission(ISynchMission mission)
        //{
        //   int idx=this.MissionIndexInLoaded(mission.AssemblyFileName, mission.StartClassType) ;
        //   if (idx != -1)
        //   {
        //      ISynchAction action = this.LoadedMissions[idx] as ISynchAction;
        //      if (action != null)
        //         action.Stop();
        //   }
        //}
        #endregion

        #region //�ر�����
        //public void CloseMission(ISynchMission mission)
        //{
        //   if (m_CurrentMission == mission)
        //      m_CurrentMission = null;
        //   UnregisterMission(mission.MissionName);
        //   if (_loadedMissions.Contains(mission))
        //      _loadedMissions.Remove(mission);
        //}


        ///// <summary>
        ///// �ر���װ�ص�Mission
        ///// </summary>
        ///// <returns></returns>
        //public bool CloseMissionsLoaded()
        //{
        //   foreach (ISynchMission mission in _loadedMissions)
        //   {
        //      CloseMission(mission);
        //   }
        //   _loadedMissions.Clear();
        //   return true;
        //}
        #endregion
    }
}
