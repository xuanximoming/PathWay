using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace DrectSoft.JobManager
{
    /// <summary>
    /// ����Ķ�������
    /// </summary>
    public interface IJobAction
    {
        /// <summary>
        /// ��������������
        /// </summary>
        Job Parent { get; set; }
        /// <summary>
        /// ͬ������״̬
        /// </summary>
        SynchState SynchState { get; }
        /// <summary>
        /// ���Լ������ò���
        /// </summary>
        bool HasPrivateSettings { get; }
        /// <summary>
        /// �г�ʼ������
        /// </summary>
        bool HasInitializeAction { get; }

        /// <summary>
        /// ִ�г�ʼ������
        /// </summary>
        void ExecuteDataInitialize();
        /// <summary>
        /// ִ������
        /// </summary>
        void Execute();
        /// <summary>
        /// ǿ��ֹͣ����
        /// </summary>
        void Stop();
        /// <summary>
        /// ��������
        /// </summary>
        void Suspend();
        /// <summary>
        /// ���������ִ��
        /// </summary>
        void Resume();
        /// <summary>
        /// ˢ�������Լ��Ĳ�������
        /// </summary>
        void RefreshPrivateSettings();

    }
}
