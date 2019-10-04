using System;
using System.Collections.Generic;
using System.Text;

namespace YidanSoft.Core
{
   /// <summary>
   /// 取得数据存储的对象
   /// </summary>
   public static class DataAccessFactory
   {
      //private static SqlDataAccess _sqlDataAccessInstance;
      private static Dictionary<string, IDataAccess> m_DataAccesseDictionary = new Dictionary<string, IDataAccess>();

      /// <summary>
      /// 得到一个数据访问对象
      /// </summary>
      /// <returns></returns>
      public static IDataAccess GetSqlDataAccess()
      {
         //_sqlDataAccessInstance = new SqlDataAccess();
         //return _sqlDataAccessInstance;
         return DefaultDataAccess;
      }

      /// <summary>
      /// 通过指定的DbName得到数据访问对象
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
      /// 取得Cache访问组件的设置
      /// </summary>
      /// <returns></returns>
      public static string CacheComConfig
      {
         get { return string.Empty; }
      }

      /// <summary>
      /// 默认的数据访问连接。
      /// 以后在自定义的控件和组件内若需要使用数据访问时用此属性。KwSystem会初始化此属性，以避免重复创建数据连接
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

