using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace YidanSoft.Core
{
    /// <summary>
    /// �������ݷ�����
    /// </summary>
    public class AppConfigDalc
    {
        #region sql text
        string sqlSelectAppConfigs = @"select Configkey, Name,Value,Descript,Valid,Design,Hide from APPCFG where Hide<>1 ";
        //string sqlUpdateAppConfig = string.Format(@"update APPCFG set {0}=@{0} where {1}=@{1}", ColValue, ColKey);
        string sqlUpdateAppConfig = string.Format(@"update APPCFG set {0}=@{0},{1}=@{1},{2}=@{2},{3}=@{3} where {4}=@{4}", ColValue,ColDescript,ColValid,ColHide, ColKey);
        string sqlSelectAppConfigByKey = @"select Configkey, Name,Value,Descript,ParamType ,Design,Valid,Hide from APPCFG where Configkey=@Configkey and Valid=1 ";
        #endregion

        #region public const
        /// <summary>
        /// 
        /// </summary>
        public const string ColKey = "Configkey";
        /// <summary>
        /// 
        /// </summary>
        public const string ColName = "Name";
        /// <summary>
        /// 
        /// </summary>
        public const string ColValue = "Value";
        /// <summary>
        /// 
        /// </summary>
        public const string ColDescript = "Descript";
        /// <summary>
        /// 
        /// </summary>
        public const string ColType = "ParamType";

        /// <summary>
        /// 
        /// </summary>
        public const string ColDesign = "Design";

        /// <summary>
        /// 
        /// </summary>
        public const string ColValid = "Valid";


        /// <summary>
        /// 
        /// </summary>
        public const string CapKey = "���ü�";
        /// <summary>
        /// 
        /// </summary>
        public const string CapDescript = "��������";
        /// <summary>
        /// 
        /// </summary>
        public const string CapName = "����";

        public const string ColHide = "Hide";
        #endregion

        IDataAccess _sqlHelper = DataAccessFactory.GetSqlDataAccess();

        /// <summary>
        /// ����
        /// </summary>
        public AppConfigDalc()
        {
        }

        /// <summary>
        /// ����Ӧ���������ݱ�
        /// </summary>
        /// <returns></returns>
        public DataTable SelectAppConfigs()
        {
            DataTable dt = _sqlHelper.ExecuteDataTable(sqlSelectAppConfigs);
            SetDataTableCaption(dt);
            return dt;
        }



        private void SetDataTableCaption(DataTable dt)
        {
            if (dt.Columns.Contains(ColKey)) dt.Columns[ColKey].Caption = CapKey;
            if (dt.Columns.Contains(ColName)) dt.Columns[ColName].Caption = CapName;
            if (dt.Columns.Contains(ColDescript)) dt.Columns[ColDescript].Caption = CapDescript;
        }

        /// <summary>
        /// �����й���������ʵ��
        /// </summary>
        /// <param Name="row"></param>
        /// <returns></returns>
        public EmrAppConfig DataRow2EmrAppConfig(DataRow row)
        {
            EmrAppConfig eac = new EmrAppConfig();
            eac.Key = row[ColKey].ToString();
            eac.Name = row[ColName].ToString();
            eac.Config = row[ColValue].ToString();
            eac.DesignClass = row[ColDesign].ToString();
            eac.Descript = row[ColDescript].ToString();
            eac.Ptype = (ConfigParamType)(int.Parse(row[ColType].ToString()));
            eac.Valid = int.Parse(row[ColValid].ToString()) == 1 ? true : false;
            eac.Plevel = ConfigParamLevel.System;
            eac.IsHide = row[ColHide].ToString();
            GetSubConfigs(eac);
            return eac;
        }

        private void GetSubConfigs(EmrAppConfig config)
        {
            if (config.Ptype == ConfigParamType.Set && config.Keyset != null && config.Keyset.Count > 0)
            {
                string[] keys = new string[config.Keyset.Count];
                config.Keyset.CopyTo(keys, 0);
                config.SubConfigs = SelectAppConfigs(keys);
            }
        }

        /// <summary>
        /// ��������ֵ
        /// </summary>
        /// <param Name="config"></param>
        public void UpdateEmrAppConfig(EmrAppConfig config)
        {
            if (config == null) throw new ArgumentNullException("config");
            SqlParameter[] sqlParams = GetUpdateParams(config);
            switch (config.Plevel)
            {
                case ConfigParamLevel.System:
                    _sqlHelper.ExecuteNoneQuery(sqlUpdateAppConfig, sqlParams);
                    break;
                default:
                    break;
            }
        }

        SqlParameter[] GetUpdateParams(EmrAppConfig config)
        {
            SqlParameter paramKey = new SqlParameter(ColKey, SqlDbType.VarChar, 32);
            SqlParameter paramValue = new SqlParameter(ColValue, SqlDbType.NVarChar, int.MaxValue); ;
            SqlParameter paramDespc = new SqlParameter(ColDescript, SqlDbType.NVarChar, int.MaxValue); //��������
            SqlParameter paramValid = new SqlParameter(ColValid, SqlDbType.NVarChar, int.MaxValue); //��Ч
            SqlParameter paramHide = new SqlParameter(ColHide, SqlDbType.NVarChar, int.MaxValue); //����

            paramValue.Value = config.Config;
            paramDespc.Value = config.Descript;
            paramValid.Value = config.Valid1;
            paramHide.Value = config.IsHide;
            paramKey.Value = config.Key;
            return new SqlParameter[] { paramKey, paramValue ,paramDespc,paramValid,paramHide};
        }

        /// <summary>
        /// ѡ��ָ��������
        /// </summary>
        /// <param Name="key"></param>
        /// <returns></returns>
        public EmrAppConfig SelectAppConfig(string key)
        {
            SqlParameter[] sqlParams = GetSelectParams(key, string.Empty);
            DataTable dt = _sqlHelper.ExecuteDataTable(sqlSelectAppConfigByKey, sqlParams);
            if (dt != null && dt.Rows.Count > 0)
                return DataRow2EmrAppConfig(dt.Rows[0]);
            return null;

            //using (YidanEMREntities entities = new YidanEMREntities())
            //{

            //    var appcfgs = from ap in entities.PPCFG
            //                  where ap.Configkey.Equals(key)
            //                  select ap;

            //    PPCFG apcfg = appcfgs.FirstOrDefault<PPCFG>();

            //    EmrAppConfig emrappcfg = new EmrAppConfig();
            //    emrappcfg.Key = apcfg.Configkey;
            //    emrappcfg.Name = apcfg.Name;
            //    emrappcfg.Ptype = (ConfigParamType)Enum.Parse(typeof(ConfigParamType), apcfg.ParamType.ToString());
            //    //emrappcfg.RoleId=apcfg.
            //    emrappcfg.Config = apcfg.Value;
            //    emrappcfg.Descript = apcfg.Descript;


            //    return emrappcfg;
            //}

        }

        SqlParameter[] GetSelectParams(string key, string userId)
        {
            SqlParameter paramKey = new SqlParameter(ColKey, SqlDbType.VarChar, 32);
            paramKey.Value = key;
            return new SqlParameter[] { paramKey };
        }

        /// <summary>
        /// ѡ��ָ�������ü���
        /// </summary>
        /// <param Name="keys"></param>
        /// <returns></returns>
        public Dictionary<string, EmrAppConfig> SelectAppConfigs(params string[] keys)
        {
            string sqlWhere = string.Empty;
            if (keys != null && keys.Length > 0)
            {
                sqlWhere = " and " + ColKey + " in (''";
                for (int i = 0; i < keys.Length; i++)
                {
                    sqlWhere += ",'" + keys[i] + "'";
                }
                sqlWhere += ")";
            }
            string sqlSelectMulti = sqlSelectAppConfigs + sqlWhere;
            DataTable dt = _sqlHelper.ExecuteDataTable(sqlSelectMulti);

            Dictionary<string, EmrAppConfig> cfgs = new Dictionary<string, EmrAppConfig>();
            if (dt != null && dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                    cfgs.Add(dt.Rows[i][ColKey].ToString(), DataRow2EmrAppConfig(dt.Rows[i]));
            }
            return cfgs;
        }

    }
}
