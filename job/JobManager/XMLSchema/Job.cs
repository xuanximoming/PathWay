using System;
using System.Collections.Generic;
using System.Text;
using System.Xml.Serialization;
using System.Diagnostics;

namespace DrectSoft.JobManager
{
   /// <summary>
   /// ������
   /// </summary>
   [SerializableAttribute()]
   [DebuggerStepThroughAttribute()]
   [XmlTypeAttribute(AnonymousType = true)]
   public class Job
   {
      #region properties
      /// <summary>
      /// ��������
      /// </summary>
      [XmlAttributeAttribute()]
      public string Name
      {
         get { return nameField; }
         set { nameField = value; }
      }
      private string nameField;

      /// <summary>
      /// ��������
      /// </summary>
      [XmlAttributeAttribute()]
      public string Description
      {
         get { return descriptionField; }
         set { descriptionField = value; }
      }
      private string descriptionField;

      /// <summary>
      /// �����Ƿ�����
      /// </summary>
      [XmlAttributeAttribute()]
      public bool Enable
      {
         get { return enableField; }
         set { enableField = value; }
      }
      private bool enableField;

      /// <summary>
      /// ��UI���Ƿ�ɼ�
      /// </summary>
      [XmlAttributeAttribute()]
      public bool Visible
      {
         get { return visibleField; }
         set { visibleField = value; }
      }
      private bool visibleField;

      /// <summary>
      /// �����������ȫ��
      /// </summary>
      [XmlAttributeAttribute()]
      public string Class
      {
         get { return classField; }
         set { classField = value; }
      }
      private string classField;

      /// <summary>
      /// �����DLLȫ��
      /// </summary>
      [XmlAttributeAttribute()]
      public string Library
      {
         get { return libraryField; }
         set { libraryField = value; }
      }
      private string libraryField;

      /// <summary>
      /// ͼ������
      /// </summary>
      [XmlAttributeAttribute()]
      public string IconName
      {
         get { return iconNameField; }
         set { iconNameField = value; }
      }
      private string iconNameField;

      /// <summary>
      /// ��־Ŀ¼·��
      /// </summary>
      [XmlAttributeAttribute()]
      public string LogDirectory
      {
         get { return logDirectoryField; }
         set { logDirectoryField = value; }
      }
      private string logDirectoryField;

      /// <summary>
      /// ������üƻ�����
      /// </summary>
      public JobPlan JobSchedule
      {
         get { return jobScheduleField; }
         set { jobScheduleField = value; }
      }
      private JobPlan jobScheduleField;

      /// <summary>
      /// ����ĺ��Ĵ����߼�
      /// </summary>
      [XmlIgnore()]
      public IJobAction Action
      {
         get { return _action; }
         set { _action = value; }
      }
      private IJobAction _action;

      #endregion
   }
}
