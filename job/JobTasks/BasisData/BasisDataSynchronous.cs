using DrectSoft.Core;
using System;
using System.Collections.ObjectModel;
using System.Data;
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml.Serialization;
using DrectSoft.JobManager;

[assembly: Job("��������ͬ��", "��HISͬ����������", "���Ӳ���", true, typeof(BasisDataSynchronous))]
namespace DrectSoft.JobManager
{
    /// <summary>
    /// ��������ͬ������
    /// </summary>
    public class BasisDataSynchronous : BaseJobAction
    {
        #region const String
        private const string UpdateMsgTable = "YY_JCSJGXK_MSG";
        private const string TableEmployee = "Users";
        private const string TableEmployeePwdField = "Passwd";
        private const string TableEmployeeSecretKeyField = "RegDate";
        private const string s_FieldPy = "Py";
        private const string s_FieldWb = "Wb";
        private const string DefaultPwd = "QK+S40FfCEQ=";
        private const string DefaultSecretKey = "2005110717:30:35";

        private const string s_CheckHasSynchMesg = "select top 1 * from YY_JCSJGXK_MSG";

        /// <summary>
        /// ��������ͬ�����õ�KEY
        /// </summary>
        private const string KeyBasisDataSynchSetting = "BasisDataSynchSetting";
        #endregion

        #region new properties
        /// <summary>
        /// ���Լ������ò���
        /// </summary>
        public override bool HasPrivateSettings { get { return true; } }

        /// <summary>
        /// �г�ʼ������
        /// </summary>
        public override bool HasInitializeAction { get { return true; } }
        #endregion

        #region private variable
        private IDataAccess m_SqlHelperTarget;
        private IDataAccess m_SqlHelperSource;
        private DataSet m_TargetTables;
        private GenerateShortCode m_generateShortCode;
        private ISynchApplication m_Application;

        public BasisDataSynchSetting BasisSynchsetting
        {
            get
            {
                if (_basisSynchsetting == null)
                    InitializeSettings();
                return _basisSynchsetting;
            }
        }
        private BasisDataSynchSetting _basisSynchsetting;
        #endregion

        #region ����
        public BasisDataSynchronous()
        {

            m_SqlHelperSource = DataAccessFactory.GetSqlDataAccess("HISDB");
            m_SqlHelperTarget = DataAccessFactory.DefaultDataAccess;
            m_TargetTables = new DataSet();
            m_TargetTables.Locale = CultureInfo.CurrentCulture;
        }

        /// <summary>
        /// 
        /// </summary>
        private void InitializeSettings()
        {
            Stream stream = BasicSettings.GetConfig(KeyBasisDataSynchSetting);
            XmlSerializer serializer = new XmlSerializer(typeof(BasisDataSynchSetting));
            _basisSynchsetting = serializer.Deserialize(stream) as BasisDataSynchSetting;
        }
        #endregion

        #region public IJobAction ��Ա
        /// <summary>
        /// ִ�г�ʼ�����ݱ�Ķ���
        /// </summary>
        public override void ExecuteDataInitialize()
        {
            base.SynchState = SynchState.Busy;
            try
            {
                InitializeSettings();
                InitAllTable();
            }
            catch
            { throw; }
            finally
            { base.SynchState = SynchState.Stop; }
        }

        /// <summary>
        /// 
        /// </summary>
        public override void Execute()
        {
            base.SynchState = SynchState.Busy;
            try
            {
                //SynchChangedBasisDatas();
                InitAllTable();
            }
            catch
            { throw; }
            finally
            { base.SynchState = SynchState.Stop; }
        }

        public override void RefreshPrivateSettings()
        {
            InitializeSettings();
        }
        #endregion

        #region private Methods
        /// <summary>
        /// ͬ���б仯�Ļ������ݱ�
        /// </summary>
        private void SynchChangedBasisDatas()
        {
            try
            {
                //ȡ������Ϣ��
                DataTable updatedMsgTable = GetMessageTableData();
                if (updatedMsgTable == null)
                    return;

                Collection<string> sourceTables = new Collection<string>();
                for (int i = 0; i < updatedMsgTable.Rows.Count; i++)
                {
                    if (Convert.ToInt32(updatedMsgTable.Rows[i]["gxbz"]) == 1)
                        continue;
                    sourceTables.Add(updatedMsgTable.Rows[i]["tablename"].ToString().Trim());
                }

                // ����Դ��������֯Ҫִ�е����
                if (sourceTables.Count > 0)
                {
                    OpenSynchSentence(sourceTables);

                    DoExecuteSynch();

                    // ����Դ�е�ͬ����¼
                    string updateMsgString = String.Format("update YY_JCSJGXK_MSG set gxbz = 1, gxrq = '{0}' where gxbz = 0"
                       , DateTime.Now.ToString("yyyyMMddHH:mm:ss"));
                    m_SqlHelperSource.ExecuteNoneQuery(updateMsgString, CommandType.Text);
                }
            }
            catch
            {
                base.SynchState = SynchState.Stop;
                throw;
            }
        }

        /// <summary>
        /// �����Ϣ����û��ͬ����¼����һ���Գ�ʼ�����л������ݣ�������ֻͬ���и��µ�����
        /// </summary>
        private void InitAllTable()
        {
            if (_basisSynchsetting == null) InitializeSettings();
            OpenSynchSentence(null);
            DoExecuteSynch();

        }

        private void OpenSynchSentence(Collection<string> sourceTables)
        {
            foreach (TableMapping setting in BasisSynchsetting.TableMappings)
            {
                if (!setting.Enabled)
                    continue;
                setting.NeedRunNow = false;
                foreach (TableMappingDataSource source in setting.DataSources)
                {
                    if (!setting.NeedRunNow && ((sourceTables == null) || sourceTables.Contains(source.SourceTable)))
                        setting.NeedRunNow = true;
                }
            }
        }

        private void ResetPyWbValue(DataTable table, string nameField)
        {
            if ((table == null) || (table.Rows.Count <= 0) || string.IsNullOrEmpty(nameField) || (!table.Columns.Contains(nameField)))
                return;
            if (m_generateShortCode == null)
                m_generateShortCode = new GenerateShortCode(m_SqlHelperTarget);

            int pyMaxLen = 0;
            if (table.Columns.Contains(s_FieldPy))
                pyMaxLen = table.Columns[s_FieldPy].MaxLength;
            int wbMaxLen = 0;
            if (table.Columns.Contains(s_FieldWb))
                wbMaxLen = table.Columns[s_FieldWb].MaxLength;

            //NameAbbreviation na;
            foreach (DataRow row in table.Rows)
            {
                if ((row.RowState == DataRowState.Added) || (row.RowState == DataRowState.Modified))
                {
                    if (((pyMaxLen > 0) && String.IsNullOrEmpty(row[s_FieldWb].ToString()))
                       || ((wbMaxLen > 0) && String.IsNullOrEmpty(row[s_FieldPy].ToString())))
                    {
                        string[] codes = m_generateShortCode.GenerateStringShortCode(row[nameField].ToString());

                        if ((codes != null) && (codes.Length == 2))
                        {
                            row[s_FieldPy] = codes[0];
                            row[s_FieldWb] = codes[1];
                        }
                    }
                }
            }
        }

        private void DoExecuteSynch()
        {
            int changedCount = 0;
            int updatedCount = 0;
            //DataTable changedTable;

            // ����ִ������ͬ���Ĵ���
            foreach (TableMapping setting in BasisSynchsetting.TableMappings)
            {
                try
                {
                    if (!setting.NeedRunNow)
                        continue;

                    changedCount = 0;
                    updatedCount = 0;
                    // �����Ԥִ�����Ҫ�������ȴ���,Ȼ����նԴ˱�Ļ���
                    if (!String.IsNullOrEmpty(setting.PreHandleSentence))
                    {
                        if (m_TargetTables.Tables.Contains(setting.TargetTable))
                        {
                            m_TargetTables.Tables.Remove(setting.TargetTable);
                            m_SqlHelperTarget.ExecuteNoneQuery(setting.PreHandleSentence);
                        }
                    }

                    // ����ִ��ͬ�����
                    foreach (TableMappingDataSource source in setting.DataSources)
                    {
                        if (source.Enabled)
                        {
                            // �����Ԥִ�����Ҫ�������ȴ���,Ȼ����նԴ˱�Ļ���
                            if (!String.IsNullOrEmpty(source.FilteCondition))
                            {
                                if (m_TargetTables.Tables.Contains(setting.TargetTable))
                                {
                                    RemoveFromTargetTable(setting.TargetTable, source);
                                }
                            }
                            changedCount += MergeSourceAndTargetTable(setting.TargetTable, source.SelectSentence);
                        }
                    }
                    changedCount += MergeSourceAndTargetTable(setting.TargetTable, setting.SelectSentence);

                    // ����py,wb�ֶε�ֵ
                    if (!string.IsNullOrEmpty(setting.NameField))
                        ResetPyWbValue(m_TargetTables.Tables[setting.TargetTable], setting.NameField);

                    // �������ݣ���Log
                    if (m_TargetTables.Tables[setting.TargetTable] != null)
                    {
                        //changedTable = m_TargetTables.Tables[setting.TargetTable].GetChanges();

                        //if ((changedTable != null) && (changedTable.Rows.Count > 0))
                        {
                            updatedCount = m_SqlHelperTarget.UpdateTable(m_TargetTables.Tables[setting.TargetTable]
                               , setting.TargetTable, false);
                            //m_TargetTables.Tables[setting.TargetTable].AcceptChanges();
                        }

                    }
                    if (!String.IsNullOrEmpty(setting.OtherSentence))
                        m_SqlHelperTarget.ExecuteNoneQuery(setting.OtherSentence, CommandType.Text);
                }
                catch (Exception e)
                {
                    JobLogHelper.WriteLog(new JobExecuteInfoArgs(this.Parent, e.Message));
                    base.SynchState = SynchState.Stop;
                    //throw;
                }
            }
        }

        private DataTable GetMessageTableData()
        {
            string selectString = string.Format(CultureInfo.CurrentCulture, "select * from {0} where gxbz=0", UpdateMsgTable);
            return m_SqlHelperSource.ExecuteDataTable(selectString, CommandType.Text);
        }

        private int MergeSourceAndTargetTable(string targetTable, string selectSentence)
        {
            if (!String.IsNullOrEmpty(selectSentence))
            {
                //���Ҫ���µ�Ŀ���δ��ʼ���������ȳ�ʼ��һ��
                if (m_TargetTables.Tables[targetTable] == null)
                    InitializeTargetTableDataAndSchema(targetTable);

                DataTable tempTable = m_SqlHelperSource.ExecuteDataTable(selectSentence, CommandType.Text);

                // �ֹ�����ϲ�����
                MergeDataTable(m_TargetTables.Tables[targetTable], tempTable);

                return tempTable.Rows.Count;
            }
            return 0;
        }

        private void MergeDataTable(DataTable targetTable, DataTable sourceTable)
        {
            // ����������ƥ�䣬��������£������������
            // �����������в��ᳬ���ĸ�
            string key1 = "";
            string key2 = "";
            string key3 = "";
            string key4 = "";
            // ��������ƥ�������ĸ�ʽ����
            DataColumn[] keys = targetTable.PrimaryKey;
            if ((keys == null) || (keys.Length == 0))
                throw new ArgumentNullException(targetTable.TableName, "����Ϊ��");
            StringBuilder condition = new StringBuilder("1=1");
            for (int index = 0; index < keys.Length; index++)
            {
                if (!sourceTable.Columns.Contains(keys[index].ColumnName))
                    throw new ArgumentNullException(targetTable.TableName, "Դ�����ݼ��в�������Ҫ��������" + keys[index].ColumnName);
                if (keys[index].DataType == typeof(string))
                    condition.AppendFormat(" and {0} = '{{{1}}}'", keys[index].ColumnName, index);
                else
                    condition.AppendFormat(" and {0} = {{{1}}}", keys[index].ColumnName, index);
            }
            if (keys.Length == 1)
            {
                key1 = keys[0].ColumnName;
                key2 = keys[0].ColumnName;
                key3 = keys[0].ColumnName;
                key4 = keys[0].ColumnName;
            }
            else if (keys.Length == 2)
            {
                key1 = keys[0].ColumnName;
                key2 = keys[1].ColumnName;
                key3 = keys[1].ColumnName;
                key4 = keys[1].ColumnName;
            }
            else if (keys.Length == 3)
            {
                key1 = keys[0].ColumnName;
                key2 = keys[1].ColumnName;
                key3 = keys[2].ColumnName;
                key4 = keys[2].ColumnName;
            }
            else
            {
                key1 = keys[0].ColumnName;
                key2 = keys[1].ColumnName;
                key3 = keys[2].ColumnName;
                key4 = keys[3].ColumnName;
            }
            DataRow[] matchRows;
            DataRow newRow;
            string conditionFormat = condition.ToString();
            foreach (DataRow row in sourceTable.Rows)
            {
                matchRows = targetTable.Select(String.Format(CultureInfo.CurrentCulture
                   , conditionFormat, row[key1], row[key2], row[key3], row[key4]));
                try
                {
                    switch (matchRows.Length)
                    {
                        case 0:  // ����
                            newRow = targetTable.NewRow();
                            foreach (DataColumn column in sourceTable.Columns)
                            {
                                newRow[column.ColumnName] = row[column.ColumnName];
                            }
                            targetTable.Rows.Add(newRow);
                            break;
                        case 1:  // ����
                            foreach (DataColumn col in sourceTable.Columns)
                            {
                                if (row[col.ColumnName].ToString().Trim()
                                       == matchRows[0][col.ColumnName].ToString().Trim())
                                    continue;

                                matchRows[0][col.ColumnName] = row[col.ColumnName];
                            }
                            break;
                        default:
                            //MessageBox.Show("�������ظ�");
                            break;
                    }
                }
                catch (Exception err)
                {
                    m_Application.WriteLog(new JobExecuteInfoArgs(Parent, sourceTable.TableName, err));
                }
            }
        }

        private void InitializeTargetTableDataAndSchema(string targetTableName)
        {
            //����ָ����������Ų����������Ų���
            if (string.IsNullOrEmpty(targetTableName))
                return;

            string querySql = String.Format(CultureInfo.CurrentCulture, "select * from {0}", targetTableName);
            DataTable table = m_SqlHelperTarget.ExecuteDataTable(querySql, CommandType.Text);
            table.TableName = targetTableName;
            m_SqlHelperTarget.ResetTableSchema(table, targetTableName);

            if (targetTableName == TableEmployee)
            {
                table.Columns[TableEmployeePwdField].DefaultValue = DefaultPwd;
                table.Columns[TableEmployeeSecretKeyField].DefaultValue = DefaultSecretKey;
            }

            m_TargetTables.Tables.Add(table.Copy());
        }

        private void RemoveFromTargetTable(string targetTableName, TableMappingDataSource source)
        {
            m_TargetTables.Tables[targetTableName].DefaultView.RowFilter = source.FilteCondition;

            for (int i = 0; i < m_TargetTables.Tables[targetTableName].DefaultView.Count; i++)
            {
                DataRow row = m_TargetTables.Tables[targetTableName].DefaultView[i].Row;
                row.Delete();
            }
        }
        #endregion

    }
}