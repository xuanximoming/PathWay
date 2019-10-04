using System;
using System.Collections.Generic;
using System.Text;

namespace DrectSoft.JobManager
{
   /// <summary>
   /// ����������
   /// </summary>
   public abstract class BaseJobAction : IJobAction
   {
      #region properties
      /// <summary>
      /// 
      /// </summary>
      public Job Parent
      {
         get { return _parent; }
         set { _parent = value; }
      }
      private Job _parent;

      /// <summary>
      /// ����״̬
      /// </summary>
      public SynchState SynchState
      {
         get { return _synchState; }
         protected set { _synchState = value; }
      }
      private SynchState _synchState = SynchState.Stop;

      /// <summary>
      /// ���Լ������ò���,Ĭ����
      /// </summary>
      public virtual bool HasPrivateSettings { get { return false; } }

      /// <summary>
      /// �г�ʼ������,Ĭ����
      /// </summary>
      public virtual bool HasInitializeAction { get { return false; } }
      #endregion

      public BaseJobAction()
      { }

      #region public IJobAction ��Ա
      /// <summary>
      /// ִ�г�ʼ������
      /// </summary>
      public virtual void ExecuteDataInitialize()
      {
      }

      /// <summary>
      /// ִ��
      /// </summary>
      public abstract void Execute();

      /// <summary>
      /// ֹͣ
      /// </summary>
      public virtual void Stop()
      {
      }

      /// <summary>
      /// ��ͣ
      /// </summary>
      public virtual void Suspend()
      {
      }

      /// <summary>
      /// ���������ִ��
      /// </summary>
      public virtual void Resume()
      {
      }

      /// <summary>
      /// ˢ�������Լ��Ĳ�������
      /// </summary>
      public virtual void RefreshPrivateSettings() { }
      #endregion
   }
}
