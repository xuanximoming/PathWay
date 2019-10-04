using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Data.Common;

namespace YidanSoft.Core
{
   /// <summary>
   /// ���ݷ��ʽӿ�
   /// </summary>
   public interface IDataAccess
   {
      /// <summary>
      /// ʹ�õ�һConnectionִ�����ǰ����
      /// </summary>
      void BeginUseSingleConnection();

      /// <summary>
      /// ʹ�õ�һConnectionִ�������ɺ����
      /// </summary>
      void EndUseSingleConnection();

      /// <summary>
      /// ��ʼ��������ִ�ж����������ǰʹ�á���Ҫ�ֹ��ύ��ع�����
      /// </summary>
      void BeginTransaction();

      /// <summary>
      /// ����ִ�г���ʱ�ֹ��ع�����
      /// </summary>
      void RollbackTransaction();

      /// <summary>
      /// ����ִ�гɹ����ֹ��ύ����
      /// </summary>
      void CommitTransaction();

      /// <summary>
      /// �������ݱ�
      /// </summary>
      /// <param name="commandText"></param>
      /// <returns></returns>
      DataTable ExecuteDataTable(string commandText);

      /// <summary>
      /// ������Ҫ��������ݼ�
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="cached">�Ƿ���Ҫ����</param>
      /// <returns></returns>
      DataTable ExecuteDataTable(string commandText, bool cached);

      /// <summary>
      /// �������ݱ�,ָ��CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="commandType"></param>
      /// <returns></returns>
      DataTable ExecuteDataTable(string commandText, CommandType commandType);

      /// <summary>
      /// ������Ҫ��������ݼ�,ָ��CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="cached"></param>
      /// <param name="commandType"></param>
      /// <returns></returns>
      DataTable ExecuteDataTable(string commandText, bool cached, CommandType commandType);

      /// <summary>
      /// �������ݱ�,������
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <returns></returns>
      DataTable ExecuteDataTable(string commandText, SqlParameter[] parameters);

      /// <summary>
      /// �������ݱ�,������,ָ��CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <param name="commandType"></param>
      /// <returns></returns>
      DataTable ExecuteDataTable(string commandText, SqlParameter[] parameters, CommandType commandType);

      /// <summary>
      /// ��Ҫ����ʹ��RowFilter��ͬһDataTable���й���ʱ����ʹ�ô˷�����
      /// ϵͳ����ÿ�ε��÷��ص�DataTable���л���
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="rowFilter"></param>
      /// <returns></returns>
      DataTable ExecuteDataTable(string commandText, string rowFilter);

      /// <summary>
      /// ִ��sql���
      /// </summary>
      /// <param name="commandText"></param>
      void ExecuteNoneQuery(string commandText);

      /// <summary>
      /// ִ��sql���,ָ��CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="commandType"></param>
      void ExecuteNoneQuery(string commandText, CommandType commandType);

      /// <summary>
      /// ִ��sql��� 2
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      void ExecuteNoneQuery(string commandText, SqlParameter[] parameters);

      /// <summary>
      /// ִ�д�����sql���,ָ��CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <param name="commandType"></param>
      void ExecuteNoneQuery(string commandText, SqlParameter[] parameters, CommandType commandType);

      /// <summary>
      /// ִ��Sql��� 3, ����Identity
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <param name="identityValue"></param>
      void ExecuteNoneQuery(string commandText, SqlParameter[] parameters, out int identityValue);

      /// <summary>
      /// ִ��sql��� 3, ָ��CommandType, ����Identity
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <param name="commandType"></param>
      /// <param name="identityValue"></param>
      void ExecuteNoneQuery(string commandText, SqlParameter[] parameters, CommandType commandType, out int identityValue);

      /// <summary>
      /// ���±�
      /// </summary>
      /// <param name="changedTable"></param>
      /// <param name="sqlTable"></param>
      /// <param name="needUpdateSchema"></param>
      /// <returns></returns>
      int UpdateTable(DataTable changedTable, string sqlTable, bool needUpdateSchema);

      /// <summary>
      /// ����DataSet 1
      /// </summary>
      /// <param name="commandText"></param>
      /// <returns></returns>
      DataSet ExecuteDataSet(string commandText);

      /// <summary>
      /// ����DataSet 1, ָ��CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="commandType"></param>
      /// <returns></returns>
      DataSet ExecuteDataSet(string commandText, CommandType commandType);

      /// <summary>
      /// ����DataSet 2
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <returns></returns>
      DataSet ExecuteDataSet(string commandText, SqlParameter[] parameters);

      /// <summary>
      /// ����DataSet 2, ָ��CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <param name="commandType"></param>
      /// <returns></returns>
      DataSet ExecuteDataSet(string commandText, SqlParameter[] parameters, CommandType commandType);

      /// <summary>
      /// ����IDataReader 1
      /// </summary>
      /// <param name="commandText"></param>
      /// <returns></returns>
      IDataReader ExecuteReader(string commandText);

      /// <summary>
      /// ����IDataReader 1,ָ��CommandType
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="commandType"></param>
      /// <returns></returns>
      IDataReader ExecuteReader(string commandText, CommandType commandType);

      /// <summary>
      /// ����IDataReader 2
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="parameters"></param>
      /// <returns></returns>
      IDataReader ExecuteReader(string commandText, SqlParameter[] parameters);

      /// <summary>
      /// ����IDataReader 2,ָ��CommandType
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
      /// Scalar 1, ָ��CommandType
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
      /// Scalar 2, ָ��CommandType
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
      /// ȡ�����������ļ�¼
      /// </summary>
      /// <param name="commandText">ȡ�����ݼ���Sql���</param>
      /// <param name="filter">��������</param>
      /// <param name="cached">�Ƿ񻺴�</param>
      /// <returns></returns>
      DataRow GetRecord(string commandText, string filter, bool cached);

      /// <summary>
      /// ȡ�����������ļ�¼��
      /// </summary>
      /// <param name="commandText"></param>
      /// <param name="filter"></param>
      /// <param name="cached"></param>
      /// <returns></returns>
      DataRow[] GetRecords(string commandText, string filter, bool cached);

      /// <summary>
      /// ��ȡ���ݿ��������ǰʱ��
      /// </summary>
      /// <returns></returns>
      DateTime GetServerUtcTimeNow();
   }
}
