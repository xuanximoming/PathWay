using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace YidanSoft.Core
{
   /// <summary>
   /// 数据访问接口
   /// </summary>
   public interface IDataAccess
   {
      /// <summary>
      /// 使用单一Connection执行语句前调用
      /// </summary>
      void BeginUseSingleConnection();

      /// <summary>
      /// 使用单一Connection执行语句完成后调用
      /// </summary>
      void EndUseSingleConnection();

      /// <summary>
      /// 开始事务，批量执行多个语句或表更新前使用。需要手工提交或回滚事务
      /// </summary>
      void BeginTransaction();

      /// <summary>
      /// 批量执行出错时手工回滚事务
      /// </summary>
      void RollbackTransaction();

      /// <summary>
      /// 批量执行成功后手工提交事务
      /// </summary>
      void CommitTransaction();

      /// <summary>
      /// 返回数据表
      /// </summary>
      /// <param name="commandText"></param>
      /// <returns></returns>
      DataTable ExecuteDataTable(string commandText);

      /// <summary>
      /// 返回需要缓存的数据集
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="cached">是否需要缓存</param>
      /// <returns></returns>
      DataTable ExecuteDataTable(string commandText, bool cached);

      /// <summary>
      /// 返回数据表,指定CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="commandType"></param>
      /// <returns></returns>
      DataTable ExecuteDataTable(string commandText, CommandType commandType);

      /// <summary>
      /// 返回需要缓存的数据集,指定CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="cached"></param>
      /// <param name="commandType"></param>
      /// <returns></returns>
      DataTable ExecuteDataTable(string commandText, bool cached, CommandType commandType);

      /// <summary>
      /// 返回数据表,带参数
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <returns></returns>
      DataTable ExecuteDataTable(string commandText, SqlParameter[] parameters);

      /// <summary>
      /// 返回数据表,带参数,指定CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <param name="commandType"></param>
      /// <returns></returns>
      DataTable ExecuteDataTable(string commandText, SqlParameter[] parameters, CommandType commandType);

      /// <summary>
      /// 需要反复使用RowFilter对同一DataTable进行过滤时可以使用此方法。
      /// 系统将对每次调用返回的DataTable进行缓存
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="rowFilter"></param>
      /// <returns></returns>
      DataTable ExecuteDataTable(string commandText, string rowFilter);

      /// <summary>
      /// 执行sql语句
      /// </summary>
      /// <param name="commandText"></param>
      void ExecuteNoneQuery(string commandText);

      /// <summary>
      /// 执行sql语句,指定CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="commandType"></param>
      void ExecuteNoneQuery(string commandText, CommandType commandType);

      /// <summary>
      /// 执行sql语句 2
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      void ExecuteNoneQuery(string commandText, SqlParameter[] parameters);

      /// <summary>
      /// 执行带参数sql语句,指定CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <param name="commandType"></param>
      void ExecuteNoneQuery(string commandText, SqlParameter[] parameters, CommandType commandType);

      /// <summary>
      /// 执行Sql语句 3, 返回Identity
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <param name="identityValue"></param>
      void ExecuteNoneQuery(string commandText, SqlParameter[] parameters, out int identityValue);

      /// <summary>
      /// 执行sql语句 3, 指定CommandType, 返回Identity
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <param name="commandType"></param>
      /// <param name="identityValue"></param>
      void ExecuteNoneQuery(string commandText, SqlParameter[] parameters, CommandType commandType, out int identityValue);

      /// <summary>
      /// 更新表
      /// </summary>
      /// <param name="changedTable"></param>
      /// <param name="sqlTable"></param>
      /// <param name="needUpdateSchema"></param>
      /// <returns></returns>
      int UpdateTable(DataTable changedTable, string sqlTable, bool needUpdateSchema);

      /// <summary>
      /// 返回DataSet 1
      /// </summary>
      /// <param name="commandText"></param>
      /// <returns></returns>
      DataSet ExecuteDataSet(string commandText);

      /// <summary>
      /// 返回DataSet 1, 指定CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="commandType"></param>
      /// <returns></returns>
      DataSet ExecuteDataSet(string commandText, CommandType commandType);

      /// <summary>
      /// 返回DataSet 2
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <returns></returns>
      DataSet ExecuteDataSet(string commandText, SqlParameter[] parameters);

      /// <summary>
      /// 返回DataSet 2, 指定CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <param name="commandType"></param>
      /// <returns></returns>
      DataSet ExecuteDataSet(string commandText, SqlParameter[] parameters, CommandType commandType);

      /// <summary>
      /// 返回IDataReader 1
      /// </summary>
      /// <param name="commandText"></param>
      /// <returns></returns>
      IDataReader ExecuteReader(string commandText);

      /// <summary>
      /// 返回IDataReader 1,指定CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="commandType"></param>
      /// <returns></returns>
      IDataReader ExecuteReader(string commandText, CommandType commandType);

      /// <summary>
      /// 返回IDataReader 2
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <returns></returns>
      IDataReader ExecuteReader(string commandText, SqlParameter[] parameters);

      /// <summary>
      /// 返回IDataReader 2,指定CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <param name="commandType"></param>
      /// <returns></returns>
      IDataReader ExecuteReader(string commandText, SqlParameter[] parameters, CommandType commandType);

      /// <summary>
      /// Scalar 1
      /// </summary>
      /// <param name="commandText"></param>
      /// <returns></returns>
      object ExecuteScalar(string commandText);

      /// <summary>
      /// Scalar 1, 指定CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="commandType"></param>
      /// <returns></returns>
      object ExecuteScalar(string commandText, CommandType commandType);

      /// <summary>
      /// Scalar 2
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <returns></returns>
      object ExecuteScalar(string commandText, SqlParameter[] parameters);

      /// <summary>
      /// Scalar 2, 指定CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <param name="commandType"></param>
      /// <returns></returns>
      object ExecuteScalar(string commandText, SqlParameter[] parameters, CommandType commandType);

      /// <summary>
      /// ResetTableSchema
      /// </summary>
      /// <param name="originalTable"></param>
      /// <param name="sqlTable"></param>
      void ResetTableSchema(DataTable originalTable, string sqlTable);

      /// <summary>
      /// GetTableColumnDefinitions
      /// </summary>
      /// <param name="tableName"></param>
      /// <returns></returns>
      DataTable GetTableColumnDefinitions(string tableName);

      /// <summary>
      /// 取得满足条件的记录
      /// </summary>
      /// <param name="commandText">取得数据集的Sql语句</param>
      /// <param name="filter">过滤条件</param>
      /// <param name="cached">是否缓存</param>
      /// <returns></returns>
      DataRow GetRecord(string commandText, string filter, bool cached);

      /// <summary>
      /// 取得满足条件的记录集
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="filter"></param>
      /// <param name="cached"></param>
      /// <returns></returns>
      DataRow[] GetRecords(string commandText, string filter, bool cached);

      /// <summary>
      /// 获取数据库服务器当前时间
      /// </summary>
      /// <returns></returns>
      DateTime GetServerUtcTimeNow();
   }
}
