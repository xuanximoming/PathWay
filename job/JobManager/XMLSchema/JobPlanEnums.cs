using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;

namespace DrectSoft.JobManager
{
   /// <summary>
   /// ����ƻ�������
   /// </summary>
   [SerializableAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public enum JobPlanType
   {
      /// <summary>
      /// �ظ�ִ��
      /// </summary>
      Repeat = 1,
      /// <summary>
      /// ��ִ��һ��
      /// </summary>
      JustOnce = 2
   }

   /// <summary>
   /// ������ִ�е�ʱ������λ
   /// </summary>
   [SerializableAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public enum JobExecIntervalUnit
   {
      /// <summary>
      /// δ����
      /// </summary>
      None = 0,
      /// <summary>
      /// ����
      /// </summary>
      Minute = 1,
      /// <summary>
      /// Сʱ
      /// </summary>
      Hour = 2,
      /// <summary>
      /// ��
      /// </summary>
      Day = 3,
      /// <summary>
      /// ��
      /// </summary>
      Week = 4
   }
}
