using Microsoft.Practices.EnterpriseLibrary.Common.Configuration;
using Microsoft.Practices.EnterpriseLibrary.Data;
using Microsoft.Practices.EnterpriseLibrary.Logging;
using Microsoft.Practices.EnterpriseLibrary.Logging.Configuration;
using System;
using System.Collections;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;

namespace YidanSoft.Core
{
    /// <summary>
    /// Sql数据访问
    /// </summary>
    public class SqlDataAccess : IDataAccess
    {
        private LoggingSettings LoggingSetting
        {
            get
            {
                if (_loggingSetting == null)
                    _loggingSetting = GetLoggingSettings();
                return _loggingSetting;
            }
        }
        private LoggingSettings _loggingSetting;

        private Database m_DBFactory;
        //private const string DBNAME = "EMRDB";
        private Hashtable m_CacheDataTable = new Hashtable();
        private string SelectIdentity = "select @@identity";
        private bool m_UseSingleConnection;
        private DbConnection m_SingleConnection;
        private bool m_UseTransaction;
        private DbTransaction m_Transaction;
        private SHA1 m_SHA1;
        private UnicodeEncoding m_UniEncoding;

        #region ctors
        /// <summary>
        /// Ctor
        /// </summary>
        public SqlDataAccess()
        {
            m_DBFactory = DatabaseFactory.CreateDatabase();
        }

        /// <summary>
        /// Ctor2
        /// </summary>
        public SqlDataAccess(string dbName)
        {
            m_DBFactory = DatabaseFactory.CreateDatabase(dbName);
        }
        #endregion

        #region IDataAccess Members

        /// <summary>
        /// 使用单一Connection执行语句前调用
        /// </summary>
        public void BeginUseSingleConnection()
        {
            try
            {
                m_UseSingleConnection = true;
                if (m_SingleConnection != null)
                    m_SingleConnection.Close();
                m_SingleConnection = m_DBFactory.CreateConnection();
                m_SingleConnection.Open();
            }
            catch (SqlException sqlex)
            {
                throw new Exception("BeginUseSingleConnection出现sql失败，原因：" + sqlex.Message, sqlex);
            }
            catch (Exception ex)
            {
                throw new Exception("BeginUseSingleConnection失败，原因：" + ex.Message, ex);
            }
        }

        /// <summary>
        /// 使用单一Connection执行语句完成后调用
        /// </summary>
        public void EndUseSingleConnection()
        {
            try
            {
                if ((m_UseSingleConnection) && (m_SingleConnection != null))
                    m_SingleConnection.Close();

                m_UseSingleConnection = false;
            }
            catch (SqlException sqlex)
            {
                throw new Exception("EndUseSingleConnection出现sql失败，原因：" + sqlex.Message, sqlex);
            }
            catch (Exception ex)
            {
                throw new Exception("EndUseSingleConnection失败，原因：" + ex.Message, ex);
            }
        }

        /// <summary>
        /// 开始事务，批量执行多个语句或表更新前使用。需要手工提交或回滚事务
        /// </summary>
        public void BeginTransaction()
        {
            try
            {
                m_UseTransaction = true;
                BeginUseSingleConnection();
                m_Transaction = m_SingleConnection.BeginTransaction();
            }
            catch (SqlException sqlex)
            {
                throw new Exception("BeginTransaction出现sql失败，原因：" + sqlex.Message, sqlex);
            }
            catch (Exception ex)
            {
                throw new Exception("BeginTransaction失败，原因：" + ex.Message, ex);
            }
        }

        /// <summary>
        /// 批量执行出错时手工回滚事务
        /// </summary>
        public void RollbackTransaction()
        {
            try
            {
                if (m_Transaction != null)
                    m_Transaction.Rollback();
                m_UseTransaction = false;
                EndUseSingleConnection();
            }
            catch (SqlException sqlex)
            {
                throw new Exception("RollbackTransaction出现sql失败，原因：" + sqlex.Message, sqlex);
            }
            catch (Exception ex)
            {
                throw new Exception("RollbackTransaction失败，原因：" + ex.Message, ex);
            }
        }

        /// <summary>
        /// 批量执行成功后手工提交事务
        /// </summary>
        public void CommitTransaction()
        {
            try
            {
                if (m_Transaction != null)
                    m_Transaction.Commit();
                m_UseTransaction = false;
                EndUseSingleConnection();
            }
            catch (SqlException sqlex)
            {
                throw new Exception("CommitTransaction出现sql失败，原因：" + sqlex.Message, sqlex);
            }
            catch (Exception ex)
            {
                throw new Exception("CommitTransaction失败，原因：" + ex.Message, ex);
            }
        }

        #region ExecuteDataTable

        /// <summary>
        /// 返回DataTable,指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, CommandType commandType)
        {
            return DoExecuteOneDataTable(GetCommand(commandText, commandType));
        }

        /// <summary>
        /// 返回DataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText)
        {
            return ExecuteDataTable(commandText, false);
        }

        /// <summary>
        /// 返回DataTable, 未缓存
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, SqlParameter[] parameters)
        {
            return ExecuteDataTable(commandText, parameters, CommandType.Text);
        }

        /// <summary> 
        /// 返回数据表,带参数,指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, SqlParameter[] parameters, CommandType commandType)
        {
            return DoExecuteOneDataTable(GetCommand(commandText, commandType, parameters));
        }

        /// <summary>
        /// 指定是否重新从数据库读取数据
        /// </summary>
        /// <param name="commandText">Sql语句</param>
        /// <param name="cached">重新加载缓存的信息</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, bool cached)
        {
            return ExecuteDataTable(commandText, cached, CommandType.Text);
        }

        /// <summary>
        /// 指定是否重新从数据库读取数据,指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="cached"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, bool cached, CommandType commandType)
        {
            DataTable dt = null;
            string key = "";
            bool canBeCached = false;
            if (cached)
            {
                key = ComputeHashCodeOfString(commandText);
                if (m_CacheDataTable.ContainsKey(key))
                    dt = m_CacheDataTable[key] as DataTable;
                else
                    canBeCached = true;
            }

            if (dt == null)
            {
                dt = DoExecuteOneDataTable(GetCommand(commandText, commandType));
                //if (cached && (!m_CacheDataTable.ContainsKey(commandText)))
                if (canBeCached && dt != null)
                {
                    m_CacheDataTable.Add(key, dt);
                }
            }

            return dt;
        }

        /// <summary>
        /// 需要反复使用RowFilter对同一DataTable进行过滤时可以使用此方法。
        /// 系统将对每次调用返回的DataTable进行缓存
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="rowFilter"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, string rowFilter)
        {
            if (rowFilter == null)
                rowFilter = "";
            string key1 = ComputeHashCodeOfString(commandText + rowFilter);
            if (m_CacheDataTable.ContainsKey(key1))
                return m_CacheDataTable[key1] as DataTable;
            else
            {
                DataTable dt;
                string key2 = ComputeHashCodeOfString(commandText);
                if (m_CacheDataTable.ContainsKey(key2))
                {
                    dt = m_CacheDataTable[key2] as DataTable;
                }
                else
                {
                    dt = DoExecuteOneDataTable(GetCommand(commandText, CommandType.Text));
                    if (dt != null)
                        m_CacheDataTable.Add(key2, dt);
                }
                if (!String.IsNullOrEmpty(rowFilter))
                {
                    dt.DefaultView.RowFilter = rowFilter;
                    dt = dt.DefaultView.ToTable();
                    m_CacheDataTable.Add(key1, dt);
                }
                return dt;
            }
        }
        #endregion

        #region ExecuteNoneQuery

        /// <summary>
        /// 取得满足条件的记录
        /// </summary>
        /// <param name="commandText">取得数据集的Sql语句</param>
        /// <param name="filter">过滤条件</param>
        /// <param name="cached">是否缓存</param>
        /// <returns></returns>
        public DataRow GetRecord(string commandText, string filter, bool cached)
        {
            DataTable dt = ExecuteDataTable(commandText, cached);
            DataRow[] dv = dt.Select(filter);
            if (dv.Length == 0)
                return null;
            else
                return dv[0];
        }

        /// <summary>
        /// 取得满足条件的记录集
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="filter"></param>
        /// <param name="cached"></param>
        /// <returns></returns>
        public DataRow[] GetRecords(string commandText, string filter, bool cached)
        {
            DataTable dt = ExecuteDataTable(commandText, cached);
            DataRow[] dv = dt.Select(filter);
            return dv;
        }

        /// <summary>
        /// 执行Sql语句
        /// </summary>
        /// <param name="commandText"></param>
        public void ExecuteNoneQuery(string commandText)
        {
            ExecuteNoneQuery(commandText, CommandType.Text);
        }

        /// <summary>
        /// 执行Sql语句,指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        public void ExecuteNoneQuery(string commandText, CommandType commandType)
        {
            DoExecuteNoneQuery(GetCommand(commandText, commandType));
        }

        /// <summary>
        /// 执行Sql语句,带参数,指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        public void ExecuteNoneQuery(string commandText, SqlParameter[] parameters, CommandType commandType)
        {
            if (parameters == null)
            {
                parameters = new SqlParameter[] { };
            }
            DoExecuteNoneQuery(GetCommand(commandText, commandType, parameters));
        }

        /// <summary>
        /// 执行Sql语句,带参数
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        public void ExecuteNoneQuery(string commandText, SqlParameter[] parameters)
        {
            ExecuteNoneQuery(commandText, parameters, CommandType.Text);
        }

        /// <summary>
        /// 执行Sql语句,带参数,指定CommandType,返回Identity
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <param name="identityValue"></param>
        public void ExecuteNoneQuery(string commandText, SqlParameter[] parameters, CommandType commandType, out int identityValue)
        {
            if (parameters == null)
            {
                parameters = new SqlParameter[] { };
            }

            string plusCommandText = commandText + "\r\n" + SelectIdentity;

            identityValue = int.Parse(DoExecuteDataTable(GetCommand(plusCommandText, commandType, parameters)).Tables[0].Rows[0][0].ToString());
        }

        /// <summary>
        /// 执行Sql语句,并返回Identity值
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="identityValue"></param>
        public void ExecuteNoneQuery(string commandText, SqlParameter[] parameters, out int identityValue)
        {
            ExecuteNoneQuery(commandText, parameters, CommandType.Text, out identityValue);
        }

        #endregion

        #region ExecuteDataSet

        /// <summary>
        /// 返回数据集,指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType)
        {
            return DoExecuteDataTable(GetCommand(commandText, commandType));
        }

        /// <summary>
        /// 返回数据集
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText)
        {
            return ExecuteDataSet(commandText, CommandType.Text);
        }

        /// <summary>
        /// 返回数据集,带参数,指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText, SqlParameter[] parameters, CommandType commandType)
        {
            if (parameters == null)
            {
                parameters = new SqlParameter[] { };
            }
            return DoExecuteDataTable(GetCommand(commandText, commandType, parameters));
        }

        /// <summary>
        /// 返回数据集2
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText, SqlParameter[] parameters)
        {
            return ExecuteDataSet(commandText, parameters, CommandType.Text);
        }

        #endregion

        #region ExecuteReader

        /// <summary>
        /// 返回DataReader, 指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            return DoExecuteReader(GetCommand(commandText, commandType));
        }

        /// <summary>
        /// 返回DataReader
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(commandText, CommandType.Text);
        }

        /// <summary>
        /// 返回DataReader2, 指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText, SqlParameter[] parameters, CommandType commandType)
        {
            if (parameters == null)
            {
                parameters = new SqlParameter[] { };
            }
            return DoExecuteReader(GetCommand(commandText, commandType, parameters));
        }

        /// <summary>
        /// 返回DataReader 2
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText, SqlParameter[] parameters)
        {
            return ExecuteReader(commandText, parameters, CommandType.Text);
        }

        #endregion

        #region ExecuteScalar

        /// <summary>
        /// Scalar, 指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, CommandType commandType)
        {
            return DoExecuteScalar(GetCommand(commandText, commandType));
        }

        /// <summary>
        /// Scalar
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText)
        {
            return ExecuteScalar(commandText, CommandType.Text);
        }

        /// <summary>
        /// Scalar2, 指定CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, SqlParameter[] parameters, CommandType commandType)
        {
            if (parameters == null)
            {
                return ExecuteScalar(commandText);
            }
            return DoExecuteScalar(GetCommand(commandText, CommandType.Text, parameters));
        }

        /// <summary>
        /// Scalar 2
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public object ExecuteScalar(string commandText, SqlParameter[] parameters)
        {
            return ExecuteScalar(commandText, parameters, CommandType.Text);
        }

        #endregion

        /// <summary>
        /// 更新表。方法内部会调用table的GetChanges方法。
        /// 若不是在事务内调用Update，则在更新成功后会自动调用table的AcceptChanges方法。
        /// </summary>
        /// <param name="table">包含所有数据的表。注意：不是GetChanges()后的表</param>
        /// <param name="sqlTableName"></param>
        /// <param name="needUpdateSchema"></param>
        /// <returns></returns>
        public int UpdateTable(DataTable table, string sqlTableName, bool needUpdateSchema)
        {
            if (table == null)
                return -1;
            if (String.IsNullOrEmpty(sqlTableName))
                return -1;
            //DataTable updateTable = changedTable.Copy();

            if (needUpdateSchema)
            {
                ResetTableSchema(table, sqlTableName);
            }

            int count = DoUpdateTable(table, sqlTableName);
            if (!m_UseTransaction)
                table.AcceptChanges();
            return count;
        }

        /// <summary>
        /// ResetSchema
        /// </summary>
        /// <param name="originalTable"></param>
        /// <param name="sqlTableName"></param>
        public void ResetTableSchema(DataTable originalTable, string sqlTableName)
        {
            if (originalTable == null)
                return;
            if (String.IsNullOrEmpty(sqlTableName))
                return;
            // 在同步结构前默认Table是区分大小写的
            originalTable.CaseSensitive = true;
            m_DBFactory.ResetTableSchema(originalTable, sqlTableName);
            // 如果DataTable中包含自增列,则要设置自增列的种子
            foreach (DataColumn col in originalTable.Columns)
            {
                if (col.AutoIncrement)
                {
                    // 通过遍历的方法找到最大序号
                    long maxNo = 0;
                    long tempNo;
                    foreach (DataRow row in originalTable.Rows)
                    {
                        tempNo = Convert.ToInt64(row[col.ColumnName], CultureInfo.CurrentCulture);
                        if (tempNo > maxNo)
                            maxNo = tempNo;
                    }
                    col.AutoIncrementSeed = maxNo + 1;

                    break;
                }
            }
        }

        /// <summary>
        /// GetTableColumnDefinitions
        /// </summary>
        /// <param name="tableName"></param>
        /// <returns></returns>
        public DataTable GetTableColumnDefinitions(string tableName)
        {
            return m_DBFactory.GetTableColumnDefinitions(tableName);
        }

        /// <summary>
        /// 取得指定的数据连接的配置信息
        /// </summary>
        /// <param name="dbName">数据连接名</param>
        /// <returns>保存配置信息的Hashtable</returns>
        public static Hashtable GetConnectionInfo(string dbName)
        {
            IConfigurationSource source = new SystemConfigurationSource();
            DatabaseConfigurationView configView = new DatabaseConfigurationView(source);

            return ParseConnectionString(
               configView.GetConnectionStringSettings(dbName).ConnectionString);
        }

        /// <summary>
        /// 取服务器的时间
        /// </summary>
        /// <returns></returns>
        public DateTime GetServerUtcTimeNow()
        {
            return DateTime.Now;
        }
        #endregion

        #region private method
        private LoggingSettings GetLoggingSettings()
        {
            IConfigurationSource source = new SystemConfigurationSource();
            return LoggingSettings.GetLoggingSettings(source);
        }

        private DataSet DoExecuteDataTable(DbCommand command)
        {
            //bool oldUseSingleCnt = m_UseSingleConnection;

            try
            {
                //if (!oldUseSingleCnt)
                //  BeginUseSingleConnection();

                return m_DBFactory.ExecuteDataSet(command);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //if (!oldUseSingleCnt)
                //   EndUseSingleConnection();
                LogSqlStatements(command);
            }
        }

        private DataTable DoExecuteOneDataTable(DbCommand command)
        {
            DataSet ds = DoExecuteDataTable(command);
            if (ds != null && ds.Tables.Count > 0)
            {
                DataTable table = ds.Tables[0];
                ds.Tables.Remove(table);
                return table;
            }
            else
                return null;
        }

        private void DoExecuteNoneQuery(DbCommand command)
        {
            //bool oldUseSingleCnt = m_UseSingleConnection;

            try
            {
                //if (!oldUseSingleCnt)
                // BeginUseSingleConnection();

                m_DBFactory.ExecuteNonQuery(command);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //if (!oldUseSingleCnt)
                //   EndUseSingleConnection();
                LogSqlStatements(command);
            }
        }

        private IDataReader DoExecuteReader(DbCommand command)
        {
            //bool oldUseSingleCnt = m_UseSingleConnection;

            try
            {
                //if (!oldUseSingleCnt)
                //   BeginUseSingleConnection();

                return m_DBFactory.ExecuteReader(command);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //if (!oldUseSingleCnt)
                //   EndUseSingleConnection();
                LogSqlStatements(command);
            }
        }

        private object DoExecuteScalar(DbCommand command)
        {
            //bool oldUseSingleCnt = m_UseSingleConnection;

            try
            {
                //if (!oldUseSingleCnt)
                //   BeginUseSingleConnection();
                return m_DBFactory.ExecuteScalar(command);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //if (!oldUseSingleCnt)
                //   EndUseSingleConnection();
                LogSqlStatements(command);
            }
        }

        private int DoUpdateTable(DataTable table, string sqlTable)
        {
            if (table == null)
                return -1;
            DataTable changedTable = table.GetChanges();
            if ((changedTable == null) || (changedTable.Rows.Count == 0))
                return 0;

            //bool oldUseSingleCnt = m_UseSingleConnection;
            // 强制使用单一连接,开事务
            //if (!oldUseSingleCnt)
            //   BeginUseSingleConnection();

            DbCommand insertCommand =
               m_DBFactory.GetSqlStringCommand(table, sqlTable, SqlStatementKind.Insert);
            PrepareCommandConnection(insertCommand);

            DbCommand updateCommand =
               m_DBFactory.GetSqlStringCommand(table, sqlTable, SqlStatementKind.Update);
            PrepareCommandConnection(updateCommand);

            DbCommand deleteCommand =
               m_DBFactory.GetSqlStringCommand(table, sqlTable, SqlStatementKind.Delete);
            PrepareCommandConnection(deleteCommand);

            DataSet changedDataSet = new DataSet();
            changedDataSet.Locale = System.Globalization.CultureInfo.CurrentCulture;
            //changedTable.TableName = table.TableName;
            changedDataSet.Tables.Add(changedTable);

            try
            {
                // 先记日志，否则就不能判断是插入还是更新了
                LogSqlStatements(changedDataSet, table.TableName
                   , insertCommand, updateCommand, deleteCommand);

                return m_DBFactory.UpdateDataSet(changedDataSet
                                                , table.TableName
                                                , insertCommand, updateCommand, deleteCommand
                                                , UpdateBehavior.Standard);
            }
            catch (SqlException sqlex)
            {
                throw sqlex;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                //if (!oldUseSingleCnt)
                //   EndUseSingleConnection();
                //LogSqlStatements(changedDataSet, changedTable.TableName
                //   , insertCommand, updateCommand, deleteCommand);
            }
        }

        /// <summary>
        /// 创建合适DbCommandWrapper
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <param name="commandType">命令类型</param>
        /// <returns>符合命令类型的DBCommandWrapper</returns>
        private DbCommand GetCommand(string commandText, CommandType commandType)
        {
            DbCommand command;

            if (commandType == CommandType.Text)
                command = m_DBFactory.GetSqlStringCommand(commandText);
            else if (commandType == CommandType.StoredProcedure)
                command = m_DBFactory.GetStoredProcCommand(commandText);
            else
                throw new ArgumentException("该方法不支持Table型的Command");

            command.CommandTimeout = 3000;
            PrepareCommandConnection(command);
            return command;
        }

        private void PrepareCommandConnection(DbCommand command)
        {
            if (m_UseSingleConnection)
            {
                command.Connection = m_SingleConnection;

                if (m_UseTransaction)
                    command.Transaction = m_Transaction;
            }
        }

        /// <summary>
        /// 创建带参数的DBCommandWrapper
        /// </summary>
        /// <param name="commandText">SQL命令</param>
        /// <param name="commandType">命令类型</param>
        /// <param name="parameters">参数集合</param>
        /// <returns>符合命令类型的DBCommandWrapper</returns>
        private DbCommand GetCommand(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            DbCommand command = GetCommand(commandText, commandType);

            foreach (DbParameter para in parameters)
                m_DBFactory.AddParameter(command, para.ParameterName, para.DbType, para.Direction
                   , para.SourceColumn, para.SourceVersion, para.Value);

            return command;
        }

        /// <summary>
        /// 解析连接字符串的参数
        /// </summary>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        private static Hashtable ParseConnectionString(string connectionString)
        {
            Hashtable pairs = new Hashtable();

            string[] splitString = connectionString.Split(';');
            for (int index = 0; index < splitString.Length; ++index)
            {
                string[] nameValuePair = splitString[index].Split('=');
                if (nameValuePair.Length == 2)
                    pairs.Add(nameValuePair[0], nameValuePair[1]);
            }
            return pairs;
        }

        private string ComputeHashCodeOfString(string source)
        {
            if (source == null)
                source = "";
            if (m_SHA1 == null)
                m_SHA1 = new SHA1CryptoServiceProvider();
            if (m_UniEncoding == null)
                m_UniEncoding = new UnicodeEncoding();
            byte[] result = m_SHA1.ComputeHash(m_UniEncoding.GetBytes(source));
            return m_UniEncoding.GetString(result);
        }
        #endregion

        #region do sql logging

        /// <summary>
        /// 将IDbCommand转换成SQL语句记录下来
        /// </summary>
        /// <param name="command"></param>
        private void LogSqlStatements(DbCommand command)
        {
            if (CheckNeedLog())
            {
                if (command.CommandType == CommandType.Text)
                {
                    DoLogger(m_DBFactory.ConvertCommandText2Sql(command));
                }
                else if (command.CommandType == CommandType.StoredProcedure)
                {
                    DoLogger(m_DBFactory.ConvertStoredProcedure2Sql(command));
                }
            }
        }

        /// <summary>
        /// 将更新数据集的操作转换成SQL语句记录下来
        /// </summary>
        /// <param name="dataSet"></param>
        /// <param name="tableName"></param>
        /// <param name="insertCommand"></param>
        /// <param name="updateCommand"></param>
        /// <param name="deleteCommand"></param>
        private void LogSqlStatements(DataSet dataSet, string tableName, DbCommand insertCommand,
           DbCommand updateCommand, DbCommand deleteCommand)
        {
            if (!CheckNeedLog())
                return;

            DataTable table = dataSet.Tables[tableName];

            StringBuilder insertStrings = new StringBuilder();
            StringBuilder updateStrings = new StringBuilder();
            StringBuilder deleteStrings = new StringBuilder();

            string insertFormat = m_DBFactory.GetCommandTextFormatString(insertCommand);
            string updateFormat = m_DBFactory.GetCommandTextFormatString(updateCommand);
            string deleteFormat = m_DBFactory.GetCommandTextFormatString(deleteCommand);

            foreach (DataRow row in table.Rows)
            {
                switch (row.RowState)
                {
                    case DataRowState.Added:
                        insertStrings.AppendFormat(insertFormat
                           , m_DBFactory.GetValueArrayFromParameters(insertCommand.Parameters, row));
                        insertStrings.Append("\r\n");
                        break;
                    case DataRowState.Modified:
                        updateStrings.AppendFormat(updateFormat
                           , m_DBFactory.GetValueArrayFromParameters(updateCommand.Parameters, row));
                        updateStrings.Append("\r\n");
                        break;
                    case DataRowState.Deleted:
                        deleteStrings.AppendFormat(deleteFormat
                           , m_DBFactory.GetValueArrayFromParameters(deleteCommand.Parameters, row));
                        deleteStrings.Append("\r\n");
                        break;
                }
            }

            DoLogger(insertStrings.ToString()
               + updateStrings.ToString()
               + deleteStrings.ToString());
        }

        /// <summary>
        /// 执行日志记录操作
        /// </summary>
        /// <param name="message"></param>
        private void DoLogger(string message)
        {
            LogEntry logEntry = new LogEntry();
            logEntry.EventId = 100;
            logEntry.Priority = 2;
            logEntry.Message = message;
            logEntry.Categories.Add("SQLTrace");

            Logger.Write(logEntry);
        }

        /// <summary>
        /// 检查是否需要记录执行的SQL语句
        /// </summary>
        /// <returns></returns>
        private bool CheckNeedLog()
        {
            if (LoggingSetting != null)
                return LoggingSetting.TracingEnabled;
            else
                return false;
        }
        #endregion
    }
}


