using System;
using System.Collections.Generic;
using System.Text;

namespace YidanSoft.Core
{
   /// <summary>
   /// ȡ�����ݴ洢�Ķ���
   /// </summary>
   public static class DataAccessFactory
   {
      //private static SqlDataAccess _sqlDataAccessInstance;
      private static Dictionary<string, IDataAccess> m_DataAccesseDictionary = new Dictionary<string, IDataAccess>();

      /// <summary>
      /// �õ�һ�����ݷ��ʶ���
      /// </summary>
      /// <returns></returns>
      public static IDataAccess GetSqlDataAccess()
      {
         //_sqlDataAccessInstance = new SqlDataAccess();
         //return _sqlDataAccessInstance;
         return DefaultDataAccess;
      }

      /// <summary>
      /// ͨ��ָ����DbName�õ����ݷ��ʶ���
      /// </summary>
      /// <param name="dbName"></param>
      /// <returns></returns>
      public static IDataAccess GetSqlDataAccess(string dbName)
      {
         if (String.IsNullOrEmpty(dbName))
            return GetSqlDataAccess();
         else
         {
            if (!m_DataAccesseDictionary.ContainsKey(dbName))
               m_DataAccesseDictionary.Add(dbName, new SqlDataAccess(dbName));

            return m_DataAccesseDictionary[dbName];

            //_sqlDataAccessInstance = new SqlDataAccess(dbName);
            //return _sqlDataAccessInstance;
         }
      }

      /// <summary>
      /// ȡ��Cache�������������
      /// </summary>
      /// <returns></returns>
      public static string CacheComConfig
      {
         get { return string.Empty; }
      }

      /// <summary>
      /// Ĭ�ϵ����ݷ������ӡ�
      /// �Ժ����Զ���Ŀؼ������������Ҫʹ�����ݷ���ʱ�ô����ԡ�KwSystem���ʼ�������ԣ��Ա����ظ�������������
      /// </summary>
      public static IDataAccess DefaultDataAccess
      {
         get 
         {
            if (_defaultDataAccess == null)
               _defaultDataAccess = new SqlDataAccess();

            return _defaultDataAccess; 
         }
         set 
         {
            if (value != null)
               _defaultDataAccess = value;
         }
      }
      private static IDataAccess _defaultDataAccess;
   }
}

