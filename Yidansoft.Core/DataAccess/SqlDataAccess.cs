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
    /// Sql���ݷ���
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
        /// ʹ�õ�һConnectionִ�����ǰ����
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
                throw new Exception("BeginUseSingleConnection����sqlʧ�ܣ�ԭ��" + sqlex.Message, sqlex);
            }
            catch (Exception ex)
            {
                throw new Exception("BeginUseSingleConnectionʧ�ܣ�ԭ��" + ex.Message, ex);
            }
        }

        /// <summary>
        /// ʹ�õ�һConnectionִ�������ɺ����
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
                throw new Exception("EndUseSingleConnection����sqlʧ�ܣ�ԭ��" + sqlex.Message, sqlex);
            }
            catch (Exception ex)
            {
                throw new Exception("EndUseSingleConnectionʧ�ܣ�ԭ��" + ex.Message, ex);
            }
        }

        /// <summary>
        /// ��ʼ��������ִ�ж����������ǰʹ�á���Ҫ�ֹ��ύ��ع�����
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
                throw new Exception("BeginTransaction����sqlʧ�ܣ�ԭ��" + sqlex.Message, sqlex);
            }
            catch (Exception ex)
            {
                throw new Exception("BeginTransactionʧ�ܣ�ԭ��" + ex.Message, ex);
            }
        }

        /// <summary>
        /// ����ִ�г���ʱ�ֹ��ع�����
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
                throw new Exception("RollbackTransaction����sqlʧ�ܣ�ԭ��" + sqlex.Message, sqlex);
            }
            catch (Exception ex)
            {
                throw new Exception("RollbackTransactionʧ�ܣ�ԭ��" + ex.Message, ex);
            }
        }

        /// <summary>
        /// ����ִ�гɹ����ֹ��ύ����
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
                throw new Exception("CommitTransaction����sqlʧ�ܣ�ԭ��" + sqlex.Message, sqlex);
            }
            catch (Exception ex)
            {
                throw new Exception("CommitTransactionʧ�ܣ�ԭ��" + ex.Message, ex);
            }
        }

        #region ExecuteDataTable

        /// <summary>
        /// ����DataTable,ָ��CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, CommandType commandType)
        {
            return DoExecuteOneDataTable(GetCommand(commandText, commandType));
        }

        /// <summary>
        /// ����DataTable
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText)
        {
            return ExecuteDataTable(commandText, false);
        }

        /// <summary>
        /// ����DataTable, δ����
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, SqlParameter[] parameters)
        {
            return ExecuteDataTable(commandText, parameters, CommandType.Text);
        }

        /// <summary> 
        /// �������ݱ�,������,ָ��CommandType
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
        /// ָ���Ƿ����´����ݿ��ȡ����
        /// </summary>
        /// <param name="commandText">Sql���</param>
        /// <param name="cached">���¼��ػ������Ϣ</param>
        /// <returns></returns>
        public DataTable ExecuteDataTable(string commandText, bool cached)
        {
            return ExecuteDataTable(commandText, cached, CommandType.Text);
        }

        /// <summary>
        /// ָ���Ƿ����´����ݿ��ȡ����,ָ��CommandType
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
        /// ��Ҫ����ʹ��RowFilter��ͬһDataTable���й���ʱ����ʹ�ô˷�����
        /// ϵͳ����ÿ�ε��÷��ص�DataTable���л���
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
        /// ȡ�����������ļ�¼
        /// </summary>
        /// <param name="commandText">ȡ�����ݼ���Sql���</param>
        /// <param name="filter">��������</param>
        /// <param name="cached">�Ƿ񻺴�</param>
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
        /// ȡ�����������ļ�¼��
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
        /// ִ��Sql���
        /// </summary>
        /// <param name="commandText"></param>
        public void ExecuteNoneQuery(string commandText)
        {
            ExecuteNoneQuery(commandText, CommandType.Text);
        }

        /// <summary>
        /// ִ��Sql���,ָ��CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        public void ExecuteNoneQuery(string commandText, CommandType commandType)
        {
            DoExecuteNoneQuery(GetCommand(commandText, commandType));
        }

        /// <summary>
        /// ִ��Sql���,������,ָ��CommandType
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
        /// ִ��Sql���,������
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="parameters"></param>
        public void ExecuteNoneQuery(string commandText, SqlParameter[] parameters)
        {
            ExecuteNoneQuery(commandText, parameters, CommandType.Text);
        }

        /// <summary>
        /// ִ��Sql���,������,ָ��CommandType,����Identity
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
        /// ִ��Sql���,������Identityֵ
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
        /// �������ݼ�,ָ��CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText, CommandType commandType)
        {
            return DoExecuteDataTable(GetCommand(commandText, commandType));
        }

        /// <summary>
        /// �������ݼ�
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public DataSet ExecuteDataSet(string commandText)
        {
            return ExecuteDataSet(commandText, CommandType.Text);
        }

        /// <summary>
        /// �������ݼ�,������,ָ��CommandType
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
        /// �������ݼ�2
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
        /// ����DataReader, ָ��CommandType
        /// </summary>
        /// <param name="commandText"></param>
        /// <param name="commandType"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText, CommandType commandType)
        {
            return DoExecuteReader(GetCommand(commandText, commandType));
        }

        /// <summary>
        /// ����DataReader
        /// </summary>
        /// <param name="commandText"></param>
        /// <returns></returns>
        public IDataReader ExecuteReader(string commandText)
        {
            return ExecuteReader(commandText, CommandType.Text);
        }

        /// <summary>
        /// ����DataReader2, ָ��CommandType
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
        /// ����DataReader 2
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
        /// Scalar, ָ��CommandType
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
        /// Scalar2, ָ��CommandType
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
        /// ���±������ڲ������table��GetChanges������
        /// �������������ڵ���Update�����ڸ��³ɹ�����Զ�����table��AcceptChanges������
        /// </summary>
        /// <param name="table">�����������ݵı�ע�⣺����GetChanges()��ı�</param>
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
            // ��ͬ���ṹǰĬ��Table�����ִ�Сд��
            originalTable.CaseSensitive = true;
            m_DBFactory.ResetTableSchema(originalTable, sqlTableName);
            // ���DataTable�а���������,��Ҫ���������е�����
            foreach (DataColumn col in originalTable.Columns)
            {
                if (col.AutoIncrement)
                {
                    // ͨ�������ķ����ҵ�������
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
        /// ȡ��ָ�����������ӵ�������Ϣ
        /// </summary>
        /// <param name="dbName">����������</param>
        /// <returns>����������Ϣ��Hashtable</returns>
        public static Hashtable GetConnectionInfo(string dbName)
        {
            IConfigurationSource source = new SystemConfigurationSource();
            DatabaseConfigurationView configView = new DatabaseConfigurationView(source);

            return ParseConnectionString(
               configView.GetConnectionStringSettings(dbName).ConnectionString);
        }

        /// <summary>
        /// ȡ��������ʱ��
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
            // ǿ��ʹ�õ�һ����,������
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
                // �ȼ���־������Ͳ����ж��ǲ��뻹�Ǹ�����
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
        /// ��������DbCommandWrapper
        /// </summary>
        /// <param name="commandText">SQL����</param>
        /// <param name="commandType">��������</param>
        /// <returns>�����������͵�DBCommandWrapper</returns>
        private DbCommand GetCommand(string commandText, CommandType commandType)
        {
            DbCommand command;

            if (commandType == CommandType.Text)
                command = m_DBFactory.GetSqlStringCommand(commandText);
            else if (commandType == CommandType.StoredProcedure)
                command = m_DBFactory.GetStoredProcCommand(commandText);
            else
                throw new ArgumentException("�÷�����֧��Table�͵�Command");

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
        /// ������������DBCommandWrapper
        /// </summary>
        /// <param name="commandText">SQL����</param>
        /// <param name="commandType">��������</param>
        /// <param name="parameters">��������</param>
        /// <returns>�����������͵�DBCommandWrapper</returns>
        private DbCommand GetCommand(string commandText, CommandType commandType, DbParameter[] parameters)
        {
            DbCommand command = GetCommand(commandText, commandType);

            foreach (DbParameter para in parameters)
                m_DBFactory.AddParameter(command, para.ParameterName, para.DbType, para.Direction
                   , para.SourceColumn, para.SourceVersion, para.Value);

            return command;
        }

        /// <summary>
        /// ���������ַ����Ĳ���
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
        /// ��IDbCommandת����SQL����¼����
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
        /// ���������ݼ��Ĳ���ת����SQL����¼����
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
        /// ִ����־��¼����
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
        /// ����Ƿ���Ҫ��¼ִ�е�SQL���
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


