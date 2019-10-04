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
    /// 任务管理器，最外层的逻辑管理器
    /// </summary>
    public class JobTaskManager
    {

        #region properties



        /// <summary>
        /// 配置文件中定义的系统分类
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
        /// 所有的任务
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
            // 确保EMR的数据库已经启动，否则创建任务会失败
            //TestSqlServiceHadStarted();

            // 根据任务配置信息创建每个任务的Action实现
            CreateJobAction();
        }
        #endregion

        #region public & internal method

        public void WriteAllLog()
        {
            foreach (Job job in AllJobs)
            {
                JobLogHelper.WriteLog(new JobExecuteInfoArgs(job, "主应用程序关闭", TraceLevel.Info));
            }
        }

        /// <summary>
        /// 记录日志
        /// </summary>
        /// <param name="e"></param>
        public void WriteLog(JobExecuteInfoArgs e)
        {
            JobLogHelper.WriteLog(e);
        }

        /// <summary>
        /// 保存对任务的修改
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
        /// 测试sql服务是否已经启动
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
                    Thread.Sleep(30000); // 30秒做一次
                }
            } while (testTimes > 0);

            throw new ApplicationException("无法连接数据库");
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


        #region //加载任务进队列

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

        #region //停止任务
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

        #region //关闭任务
        //public void CloseMission(ISynchMission mission)
        //{
        //   if (m_CurrentMission == mission)
        //      m_CurrentMission = null;
        //   UnregisterMission(mission.MissionName);
        //   if (_loadedMissions.Contains(mission))
        //      _loadedMissions.Remove(mission);
        //}


        ///// <summary>
        ///// 关闭已装载的Mission
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
