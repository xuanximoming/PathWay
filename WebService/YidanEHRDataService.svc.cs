using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using Yidansoft.Service.Entity;
using System.Collections;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Configuration;
using System.ServiceModel.Channels;
using System.Runtime.InteropServices;
using YidanSoft.Tool;
using System.Runtime.Serialization.Formatters.Binary;
using YidanSoft.Core;
using System.Web.Hosting;
using System.Collections.ObjectModel;
using SendOrder;
namespace Yidansoft.Service
{
    [ServiceContract(Namespace = "")]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public partial class YidanEHRDataService
    {
        public static string m_ConnectionString = "";// @"Database=YidanEHR_New;Server=192.168.2.202\two;user id=sa;password=sa";
        private static string m_ConnectionStringEMR = "";// @"Database=YidanEMR;Server=192.168.2.202\two;user id=sa;password=sa";
        private static string m_ConnectionHISString = "";// @"Database=THIS4_LY;Server=192.168.2.202\two;user id=sa;password=sa";
        public static IDataAccess SqlHelper = DataAccessFactory.GetSqlDataAccess("EHRDB");//GetSqlDataAccess();
        public static IDataAccess HISHelper = DataAccessFactory.GetSqlDataAccess("HISDB");
        IDataAccess emrsql = DataAccessFactory.GetSqlDataAccess("EMRDB");
        // 
        //String tt = ConfigurationManager.ConnectionStrings["EHR"];
        //public static SqlDataAccess GetSqlDataAccess()
        //{
        //    SqlDataAccess da;
        //    try
        //    {
        //          da = new SqlDataAccess("EHR");
        //    }
        //    catch
        //    {
        //        throw new Exception("SqlDataAccess New Error");

        //    }
        //    try
        //    {
        //        da.IsConnectDB();
        //    }
        //    catch
        //    {
        //        throw new Exception("IsConnectDB Error");

        //    }
        //   if(da==null)
        //        throw new Exception("SqlDataAccess null");



        //    return da;
        //}

        //#if DEBUG

        //     public static string m_ConnectionString = @"Database=YidanEHR_New;Server=192.168.2.202\two;user id=sa;password=sa";
        //     private static string m_ConnectionStringEMR = @"Database=YidanEMR;Server=192.168.2.202\two;user id=sa;password=sa";
        //     private static string m_ConnectionHISString = @"Database=THIS4_LY;Server=192.168.2.202\two;user id=sa;password=sa";
        //#else
        //        private static string m_ConnectionStringEMR = ConfigurationManager.AppSettings["EMRDB"].ToString();
        //        public static string m_ConnectionString = ConfigurationManager.AppSettings["EHRDB"].ToString();
        //        private static string m_ConnectionHISString = System.Configuration.ConfigurationManager.AppSettings["HISDB"].ToString();
        //#endif
        const string AdviceGroupInfo = "成套医嘱添加成功";
        const string DelMessage = "删除成功";
        const string DelMessageFail = "删除失败";
        const string AdviceGroupMessage = "医嘱成组成功";
        const string DidsAdviceGroupMessage = "取消医嘱成组成功";

        /// <summary>
        /// 判断是否由HIS视图提供数据配置
        /// </summary>
        /// <returns>HIS获取返回true  pathway获取返回false</returns>
        public static bool CheckSelectHISView()
        {
            try
            {
                string sqlStr = string.Empty;

                //add by yxy 在数据库中添加是否由HIS视图提供数据配置  0 或者空表示为在自己数据库中提取数据  1为HIS视图提供
                sqlStr = @"select Value from APPCFG where Configkey = 'SelectTOHIS'";

                DataTable dt = SqlHelper.ExecuteDataTable(sqlStr);
                if (dt == null || dt.Rows.Count == 0 || dt.Rows[0][0].ToString() == "0")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

        }

        private string m_histype = string.Empty;
        /// <summary>
        /// 存放HIS厂商类型，Winning 金仕达卫宁，BT标腾科技
        /// </summary>
        public string HISType
        {
            get
            {
                if (m_histype == string.Empty)
                {
                    string sqlStr = string.Empty;

                    //add by yxy 存放HIS厂商类型，Winning 金仕达卫宁，BT标腾科技
                    sqlStr = @"select Value from APPCFG where Configkey = 'HISTYPE'";

                    DataTable dt = SqlHelper.ExecuteDataTable(sqlStr);
                    if (dt == null || dt.Rows.Count == 0)
                    {
                        m_histype = "Winning";
                    }
                    else
                    {
                        m_histype = dt.Rows[0][0].ToString();
                    }
                }
                return m_histype;
            }
        }




        internal bool Sync2Emr
        {
            get
            {
                if ((string.IsNullOrEmpty(ConfigurationManager.AppSettings["Sync2Emr"])
                    || (ConfigurationManager.AppSettings["Sync2Emr"].ToLower().Equals("false"))))
                {
                    return false;
                }
                return true;
            }
        }


        [OperationContract]
        public void DoWork()
        {
            // 在此处添加操作实现
            return;
        }

        // 在此处添加更多操作并使用 [OperationContract] 标记它们

        #region
        /// <summary>
        /// 获取指定用户
        /// 修改：fqw 时间：2010-03-18  mark：fqwFix
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public CP_Employee GetEmployeeInfo(string id)
        {
            List<CP_Employee> emps = GetEmployee(id, "", "", "", "", "");
            if (emps == null)/* add by dxj 2011/7/22 修改原因：如果用户没查到，会报错*/
            {
                return null;
            }
            if (emps.Count > 0)
                return emps[0];
            else
                return null;
            #region 修改前代码
            //using (YidanEHREntities entities = new YidanEHREntities())
            //{
            //    try
            //    {
            //        var users = entities.CP_Employee.Where(u => u.Zgdm.Equals(id));
            //        return users.FirstOrDefault();
            //    }
            //    catch (Exception ex)
            //    {
            //        ThrowException(ex);
            //    }
            //    return null;
            //}
            #endregion
        }

        ///// <summary>
        ///// 获取指定病人
        ///// </summary>
        ///// <param name="id"></param>
        ///// <returns></returns>
        //[OperationContract]
        //[FaultContract(typeof(LoginException))]
        //public CP_InpatinetList GetPatientInfo(string id)
        //{

        //    using (YidanEHREntities entities = new YidanEHREntities())
        //    {
        //        try
        //        {
        //            decimal syxh = Convert.ToDecimal(id);
        //            var users = entities.CP_InPatient.Where(u => u.Syxh.Equals(syxh));
        //            CP_InPatient inpat= users.FirstOrDefault();


        //        }
        //        catch (SqlException ex)
        //        {
        //            ThrowSqlExpection(ex);
        //        }
        //        catch (Exception ex)
        //        {
        //            ThrowNormalExpection(ex);
        //        }
        //        return null;
        //    }
        //}

        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public string GetConnection(string strKey)
        {
            var keys = ConfigurationManager.AppSettings.AllKeys.Where(key => key.Equals(strKey));

            try
            {
                return keys.FirstOrDefault();
            }
            catch
            {
                return "未找到指定的数据连接字符串";
            }
        }

        /// <summary>
        /// 读取为字符串的系统配置
        /// 修改：fqw 时间：2010-03-18  mark：fqwFix
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public string GetBaseSettingStr(string key)
        {

            try
            {

                // String Value = ConvertMy.ToString(SqlHelper.ExecuteScalar(m_ConnectionString, CommandType.Text, String.Format("select Value from CP_AppConfig where Configkey='{0}'", key), null));


                return YidanSoft.Core.BasicSettings.GetStringConfig(key);
            }
            catch
            {
                return string.Empty;
            }

        }


        /// <summary>
        /// 读取非字符串的系统配置
        /// 修改：fqw 时间：2010-03-18  mark：fqwFix
        /// </summary>
        /// <param name="key"></param>
        /// <returns>返回的是经过压缩的字符串</returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public string GetBaseSettingStream(string key)
        {

            try
            {
                //String Value = ConvertMy.ToString(SqlHelper.ExecuteScalar(m_ConnectionString, CommandType.Text, String.Format("select Value from CP_AppConfig where Configkey='{0}'", key), null));


                //String Value = "";
                ////DataTable dt = SqlHelper.ExecuteDataTable(m_ConnectionString, CommandType.Text, String.Format("select Value from CP_AppConfig where Configkey='{0}'", key), null);
                ////if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != null)
                ////{
                ////    Value = dt.Rows[0][0].ToString().Trim();
                ////}

                //return YidanSoft.Core.BasicSettings.GetConfig(key);
                //return ZipContent(Value);
                //#region 微构前代码
                ////using (YidanEHREntities enties = new YidanEHREntities())
                ////{
                ////    var config = enties.CP_AppConfig.Where(c => c.Configkey.Equals(key)).First();
                ////    return ZipContent(config.Value);
                ////}
                return string.Empty;
        #endregion
            }
            catch
            {
                return string.Empty;
            }

        }

        #region  add by yxy 判断当前是否有权限使用系统
        /// <summary>
        /// 判断当前是否有权限使用系统
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public bool CheckRoot()
        {

            try
            {
                if (Key.ISRegTime())
                {
                    if (Key.ISRightPCHandInfo())
                    {
                        return true;
                    }
                }
                return false;

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return false;
        }

        #endregion

        #region dgq 路径评估相关，modify by XJT

        //zm    8.4 注释
        //[OperationContract]
        //public List<YidanEHROwnEntity.CP_PathCondition> GetPathConditionList(string Ljdm)
        //{
        //    List<YidanEHROwnEntity.CP_PathCondition> cplist = new List<YidanEHROwnEntity.CP_PathCondition>();
        //    using (SqlConnection cn = new SqlConnection(m_ConnectionString))
        //    {
        //        try
        //        {
        //            SqlCommand cmd = new SqlCommand("Select Tjdm,Ljdm,Tjmc,Case  Tjlb When 1 then '纳入条件' When  0 then '排除条件' else '无条件' end as Condition,Yxjl From CP_PathCondition where Yxjl=1 and Tjlb in(1,0) and Ljdm='" + Ljdm + "'", cn);
        //            SqlDataAdapter adpter = new SqlDataAdapter(cmd);
        //            DataTable dt = new DataTable();
        //            adpter.Fill(dt);
        //            foreach (DataRow dr in dt.Rows)
        //            {
        //                YidanEHROwnEntity.CP_PathCondition cp = new YidanEHROwnEntity.CP_PathCondition();
        //                cp.Tjdm = dr["Tjdm"].ToString();
        //                cp.Ljdm = dr["Ljdm"].ToString();
        //                cp.Tjmc = dr["Tjmc"].ToString();
        //                cp.Condition = dr["Condition"].ToString();
        //                cp.Yxjl = Convert.ToInt32(dr["Yxjl"].ToString());
        //                cplist.Add(cp);
        //            }
        //            return cplist;
        //        }
        //        catch (Exception ex)
        //        {
        //            ThrowException(ex);
        //        }
        //        return null;
        //    }
        //}

        //[OperationContract]
        //[FaultContract(typeof(LoginException))]
        //public Decimal InsertIntoCP_PathExecuteInfo(CP_InpatinetList inpat, string doctorid, CP_InPathPatientCondition cp)
        //{
        //    using (SqlConnection cn = new SqlConnection(m_ConnectionString))
        //    {
        //        SqlTransaction sqlTrans = null;
        //        decimal id = 0;
        //        try
        //        {
        //            cn.Open();
        //            sqlTrans = cn.BeginTransaction();
        //            id = InsertCP_InPathPatientInfo(sqlTrans, doctorid, cn, inpat);
        //            String strSql = "Insert into CP_PathExecuteInfo values(" + id + ",1100,'','" + doctorid + "','');Insert Into CP_InPathPatientCondition values(" + id + "," + cp.Syxh + ",'" + cp.Tjdm + "','" + cp.Memo + "')";
        //            SqlCommand cmd = new SqlCommand(strSql, cn, sqlTrans);
        //            cmd.ExecuteNoneQuery();

        //            //InsertIntoInPathPatientCondition(sqlTrans, cn, cp);

        //            sqlTrans.Commit();

        //            // 同步状态
        //            SychPathStatus2Emr(inpat.Hissyxh, 1);
        //        }
        //        catch (Exception ex)
        //        {
        //            sqlTrans.Rollback();
        //            ThrowException(ex);
        //        }
        //        finally
        //        {
        //            if (cn.State != ConnectionState.Closed)
        //                cn.Close();
        //        }
        //        return id;
        //    }
        //}

        private decimal InsertCP_InPathPatientInfo(SqlTransaction sqlTrans, string doctorid, SqlConnection myConnection, CP_InpatinetList inpat)         // zm 8.25 Oracle
        //decimal syxh, string ljdm, string cwys)
        {
            try
            {
                //SqlCommand myCommand = new SqlCommand("usp_CP_InsertPathPatientInfo", myConnection, sqlTrans);
                //myCommand.CommandType = CommandType.StoredProcedure;
                //添加输入查询参数、赋予值

                SqlParameter[] parameters = new SqlParameter[] 
                  {
                    new SqlParameter("@syxh",inpat.Syxh),
                    new SqlParameter("@ljdm",inpat.Ljdm),
                    new SqlParameter("@cwys",doctorid)
                };

                //myCommand.Parameters.Add("@syxh", SqlDbType.Decimal);
                //myCommand.Parameters.Add("@ljdm", SqlDbType.VarChar);
                //myCommand.Parameters.Add("@cwys", SqlDbType.VarChar);
                //myCommand.Parameters["@syxh"].Value = inpat.Syxh;
                //myCommand.Parameters["@ljdm"].Value = inpat.Ljdm;
                //myCommand.Parameters["@cwys"].Value = doctorid;
                //String ID = ConvertMy.ToString(myCommand.ExecuteScalar());

                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_InsertPathPatientInfo", parameters, CommandType.StoredProcedure);
                String ID = "0";
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != null)
                {
                    ID = dt.Rows[0][0].ToString();
                }

                EMRInsertCP_InPathPatient(ID, false, null, null);
                return ConvertMy.ToDecimal(ID);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        //zm    8.4 注释
        //private void InsertIntoInPathPatientCondition(SqlTransaction sqlTrans, SqlConnection myConnection, CP_InPathPatientCondition cp)
        //{
        //    string SqlStr = "Insert Into CP_InPathPatientCondition values(" + cp.Syxh + ",'" + cp.Tjdm + "','" + cp.Memo + "')";
        //    try
        //    {
        //        SqlCommand cmd = new SqlCommand(SqlStr, myConnection, sqlTrans);
        //        cmd.ExecuteNoneQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        #endregion

        #region xjt

        /// <summary>
        /// 费用信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        private List<CP_InpatientFeeInfo> GetInpatientFeeInfo(int syxh)
        {
            List<CP_InpatientFeeInfo> inpatinetFeeInfo = new List<CP_InpatientFeeInfo>();
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionHISString))
            //{
            try
            {
                DataTable dataTable = new DataTable();
                #region update by dxj 2011/7/28 可以查看患者费用信息
                //SqlCommand myCommand = new SqlCommand("usp_bq_fymxcx", myConnection);
                //myCommand.CommandType = CommandType.StoredProcedure;
                ////添加输入查询参数、赋予值
                //myCommand.Parameters.Add("@syxh", SqlDbType.Int);
                //myCommand.Parameters["@syxh"].Value = 26;//记得修改,xjt
                //SqlDataAdapter sqldataAdapter = new System.Data.SqlClient.SqlDataAdapter(myCommand);

                //sqldataAdapter.Fill(dataTable);
                SqlParameter[] parameters = new SqlParameter[]
                    {
                        new SqlParameter("@syxh",syxh)
                    };

                //dataTable = hissql.ExecuteDataTable("usp_bq_fymxcx", parameters, CommandType.StoredProcedure);
                if (dataTable.Rows[0][0].ToString() == "F")
                {
                    return null;
                }
                #endregion
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row["项目代码"].ToString().Trim() != string.Empty
                        && !row.IsNull("项目代码"))
                    {
                        string strTest = row["项目代码"].ToString().Trim();
                        CP_InpatientFeeInfo inpFee = new CP_InpatientFeeInfo(int.Parse(row["项目代码"].ToString()),
                            row["项目名称"].ToString(), Double.Parse(row["项目金额"].ToString()));
                        inpatinetFeeInfo.Add(inpFee);
                    }
                }

                foreach (DataRow row in dataTable.Rows)
                {
                    if (row["项目名称"].ToString().Trim() == "总计")
                    {
                        foreach (CP_InpatientFeeInfo inp in inpatinetFeeInfo)
                        {
                            inp.Zj = Double.Parse(row["项目金额"].ToString());
                        }
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            //}
            return inpatinetFeeInfo;
        }           // zm 8.25 Oracle


        /// <summary>
        /// 体征信息
        /// </summary>
        /// <param name="syxh"></param>
        /// <param name="strTimeBegin"></param>
        /// <param name="strTimeEnd"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        private List<CP_InpatientPhySign> GetInpatientPhySign(int syxh, string strTimeBegin, string strTimeEnd)
        {
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            try
            {
                //DataTable dataTable = new DataTable();

                //SqlCommand myCommand = new SqlCommand("usp_CP_InpatientPhySign", myConnection);
                //myCommand.CommandType = CommandType.StoredProcedure;
                //添加输入查询参数、赋予值

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@syxh",syxh),
                        new SqlParameter("@timeBegin",strTimeBegin),
                        new SqlParameter("@timeEnd",strTimeEnd)
                    };

                //myCommand.Parameters.Add("@syxh", SqlDbType.Int);
                //myCommand.Parameters["@syxh"].Value = syxh;
                //myCommand.Parameters.Add("@timeBegin", SqlDbType.VarChar, 8);
                //myCommand.Parameters["@timeBegin"].Value = strTimeBegin;
                //myCommand.Parameters.Add("@timeEnd", SqlDbType.VarChar, 8);
                //myCommand.Parameters["@timeEnd"].Value = strTimeEnd;

                //SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(myCommand);
                //DataAdapter.Fill(dataTable);

                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_InpatientPhySign", parameters, CommandType.StoredProcedure);

                List<CP_InpatientPhySign> inpatinetPhySign = new List<CP_InpatientPhySign>();
                foreach (DataRow row in dataTable.Rows)
                {
                    CP_InpatientPhySign inpPhySignh = new CP_InpatientPhySign(int.Parse(row["Syxh"].ToString()),
                        int.Parse(row["Tzxh"].ToString()), row["Zlrq"].ToString(), row["Lrrq"].ToString(), row["Clsj"].ToString(), row["Tw"].ToString(),
                        int.Parse(row["Mb"].ToString()), int.Parse(row["Hx"].ToString()), row["Xy"].ToString(), row["Xl"].ToString(),
                        row["Memo"].ToString(), row["Memo2"].ToString(), row["Wljw"].ToString(), row["Qbxl"].ToString(), row["Rghx"].ToString(),
                        row["Kb"].ToString(), row["Yb"].ToString(), row["Gw"].ToString(), row["Cjsj"].ToString(), int.Parse(row["Staticday"].ToString()));
                    inpatinetPhySign.Add(inpPhySignh);
                }

                return inpatinetPhySign;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}
        }       // zm 8.25 Oracle

        /// <summary>
        /// 变异信息
        /// </summary>
        /// <param name="syxh">路径序号</param>
        /// <returns></returns> 
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_InpatientPathCVInfo> GetInpatientPathCVInfo(Decimal ljxh)
        {
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            try
            {
                //DataTable dataTable = new DataTable();

                //SqlCommand myCommand = new SqlCommand("usp_CP_InpatientPathCVInfo", myConnection);
                //myCommand.CommandType = CommandType.StoredProcedure;
                //添加输入查询参数、赋予值

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@Ljxh",ljxh)
                    };

                //myCommand.Parameters.Add("@Ljxh", SqlDbType.Decimal);
                //myCommand.Parameters["@Ljxh"].Value = ljxh;

                //SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(myCommand);


                //if (dataTable != null)
                //{
                //    DataAdapter.Fill(dataTable);
                //}

                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_InpatientPathCVInfo", parameters, CommandType.StoredProcedure);

                List<CP_InpatientPathCVInfo> inpatinetPathCVInfo = new List<CP_InpatientPathCVInfo>();
                foreach (DataRow row in dataTable.Rows)
                {
                    CP_InpatientPathCVInfo inpPhyCVInfo = new CP_InpatientPathCVInfo(row["Id"].ToString(),
                        row["Syxh"].ToString(), row["Ljdm"].ToString(), row["Mxdm"].ToString(), row["Bylb"].ToString(), row["BylbName"].ToString(),
                        row["Bynr"].ToString(), row["Byyy"].ToString(), row["Bysj"].ToString());
                    inpatinetPathCVInfo.Add(inpPhyCVInfo);
                }

                return inpatinetPathCVInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}
        }       // zm 8.25 Oracle


        /// <summary>
        /// 病种
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_DiagnosisList> GetDiagnosisListInfo()
        {
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            try
            {
                //DataTable dataTable = new DataTable();

                //SqlCommand myCommand = new SqlCommand("usp_CP_DiagnosisList", myConnection);
                //myCommand.CommandType = CommandType.StoredProcedure;

                //SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(myCommand);


                //if (dataTable != null)
                //{
                //    DataAdapter.Fill(dataTable);
                //}

                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_DiagnosisList");

                List<CP_DiagnosisList> diagnosisListInfo = new List<CP_DiagnosisList>();
                foreach (DataRow row in dataTable.Rows)
                {
                    CP_DiagnosisList diaListInfo = new CP_DiagnosisList(row["MarkId"].ToString(), row["ICD"].ToString(), row["Name"].ToString(), row["QueryName"].ToString());
                    diagnosisListInfo.Add(diaListInfo);
                }

                return diagnosisListInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}
        }       // zm 8.25 Oracle

        /// <summary>
        /// 路径
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_ClinicalPathList> GetClinicalPathListInfo()
        {
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            try
            {
                //DataTable dataTable = new DataTable();

                //SqlCommand myCommand = new SqlCommand("usp_CP_GetClincalPathList", myConnection);
                //myCommand.CommandType = CommandType.StoredProcedure;

                //SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(myCommand);


                //if (dataTable != null)
                //{
                //    DataAdapter.Fill(dataTable);
                //}

                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_GetClincalPathList");

                List<CP_ClinicalPathList> clinicalPathListInfo = new List<CP_ClinicalPathList>();
                foreach (DataRow row in dataTable.Rows)
                {
                    CP_ClinicalPathList clinicalPath = new CP_ClinicalPathList(ConvertMy.ToString(row["Ljdm"]), ConvertMy.ToString(row["Name"]), ConvertMy.ToString(row["QueryName"]));

                    clinicalPathListInfo.Add(clinicalPath);
                }
                return clinicalPathListInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}
        }        // zm 8.25 Oracle


        /// <summary>
        /// 全部科室
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_DepartmentList> GetDepartmentListInfo()
        {
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            try
            {
                //DataTable dataTable = new DataTable();

                //SqlCommand myCommand = new SqlCommand("usp_CP_DeptmentList", myConnection);
                //myCommand.CommandType = CommandType.StoredProcedure;

                //SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(myCommand);


                //if (dataTable != null)
                //{
                //    DataAdapter.Fill(dataTable);
                //}

                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_DeptmentList");

                List<CP_DepartmentList> deptmentListInfo = new List<CP_DepartmentList>();
                foreach (DataRow row in dataTable.Rows)
                {
                    CP_DepartmentList depListInfo = new CP_DepartmentList(row["ID"].ToString(), row["Name"].ToString(), row["QueryName"].ToString().ToUpper());
                    deptmentListInfo.Add(depListInfo);
                }

                return deptmentListInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}
        }       // zm 8.25 Oracle

        /// <summary>
        /// 病种
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_ClinicalDiagnosisList> GetClinicalDiagnosis()
        {
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            try
            {
                //DataTable dataTable = new DataTable();

                //SqlCommand myCommand = new SqlCommand("usp_CP_ClinicalDiagnosisList", myConnection);
                //myCommand.CommandType = CommandType.StoredProcedure;

                //SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(myCommand);


                //if (dataTable != null)
                //{
                //    DataAdapter.Fill(dataTable);
                //}

                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_ClinicalDiagnosisList");

                List<CP_ClinicalDiagnosisList> clinDiagListInfo = new List<CP_ClinicalDiagnosisList>();
                foreach (DataRow row in dataTable.Rows)
                {
                    CP_ClinicalDiagnosisList cliDiaInfo = new CP_ClinicalDiagnosisList(row["Ljdm"].ToString(), row["Bzdm"].ToString(), row["Bzmc"].ToString());
                    clinDiagListInfo.Add(cliDiaInfo);
                }

                return clinDiagListInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}
        }       // zm 8.25 Oracle

        /// <summary>
        /// 路径
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_ClinicalPathList> GetClinicalPathList(string strTimeFrom, string strTimeTo, string strKsdm, string Ljdm, string strLjmc, string strYxjl)
        {
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            try
            {
                //DataTable dataTable = new DataTable();

                //SqlCommand myCommand = new SqlCommand("usp_CP_ClinicalPathList", myConnection);
                //myCommand.CommandType = CommandType.StoredProcedure;
                //添加输入查询参数、赋予值

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@Kssj",strTimeFrom),
                        new SqlParameter("@Jssj",strTimeTo),
                        new SqlParameter("@Ksdm",strKsdm),
                        new SqlParameter("@Ljmc",strLjmc),
                        new SqlParameter("@Ljdm",Ljdm),
                        new SqlParameter("@Yxjl",strYxjl)
                    };

                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_ClinicalPathList", parameters, CommandType.StoredProcedure);

                List<CP_ClinicalPathList> clinPathListInfo = new List<CP_ClinicalPathList>();
                foreach (DataRow row in dataTable.Rows)
                {
                    CP_ClinicalPathList cliListInfo = new CP_ClinicalPathList(row["Ljdm"].ToString(), row["Name"].ToString(), row["Ljms"].ToString(),
                        decimal.Parse(row["Zgts"].ToString()), decimal.Parse(row["Jcfy"].ToString()), decimal.Parse(row["Vesion"].ToString()),
                        row["Cjsj"].ToString(), row["Shsj"].ToString(), row["Syks"].ToString(), row["Yxjl"].ToString(),
                        row["Syks"].ToString(), row["DeptName"].ToString(), row["ShysName"].ToString(), int.Parse(row["YxjlId"].ToString()),
                        UnzipContent(row["WorkFlowXML"].ToString()));
                    cliListInfo.LjSyqk = row["LjSyqk"].ToString();
                    cliListInfo.LjSysl = decimal.Parse(row["LjSySl"].ToString());
                    cliListInfo.Py = row["py"].ToString();
                    clinPathListInfo.Add(cliListInfo);
                }
                //   long l = GetSpaceForObject(clinPathListInfo);
                return clinPathListInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            //}
        }       // zm 8.25 Oracle


        /// <summary>
        /// 路径
        /// </summary>
        /// <returns></returns>
        //[OperationContract]
        //[FaultContract(typeof(LoginException))]
        //public List<CP_ClinicalPathList> GetClinicalPathListPage(string strTimeFrom, string strTimeTo, string strKsdm, string strLjdm, string strYxjl)
        //{
        //    //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
        //    //{
        //        try
        //        {
        //            //DataTable dataTable = new DataTable();

        //            //SqlCommand myCommand = new SqlCommand("usp_CP_ClinicalPathList", myConnection);
        //            //myCommand.CommandType = CommandType.StoredProcedure;
        //            //添加输入查询参数、赋予值

        //            SqlParameter[] parameters = new SqlParameter[] 
        //            {
        //                new SqlParameter("@Kssj",strTimeFrom),
        //                new SqlParameter("@Jssj",strTimeTo),
        //                new SqlParameter("@Ksdm",strKsdm),
        //                new SqlParameter("@Ljdm",strLjdm),
        //                new SqlParameter("@Yxjl",strYxjl)
        //            };

        //            //myCommand.Parameters.Add("@Kssj", SqlDbType.VarChar, 19);
        //            //myCommand.Parameters.Add("@Jssj", SqlDbType.VarChar, 19);
        //            //myCommand.Parameters.Add("@Ksdm", SqlDbType.VarChar, 12);
        //            //myCommand.Parameters.Add("@Ljdm", SqlDbType.VarChar, 12);
        //            //myCommand.Parameters.Add("@Yxjl", SqlDbType.VarChar, 12);
        //            //myCommand.Parameters["@Kssj"].Value = strTimeFrom;
        //            //myCommand.Parameters["@Jssj"].Value = strTimeTo;
        //            //myCommand.Parameters["@Ksdm"].Value = strKsdm;
        //            //myCommand.Parameters["@Ljdm"].Value = strLjdm;
        //            //myCommand.Parameters["@Yxjl"].Value = strYxjl;
        //            //SqlParameter CurrentPage = new SqlParameter("@CurrentPage", 2);
        //            //CurrentPage.Direction = ParameterDirection.Output;

        //            //SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(myCommand);


        //            //if (dataTable != null)
        //            //{
        //            //    dataAdapter.Fill(dataTable);
        //            //}
        //            DataTable dataTable = SqlHelper.ExecuteDataTable(m_ConnectionString, CommandType.StoredProcedure, "usp_CP_ClinicalPathList", parameters);


        //            List<CP_ClinicalPathList> clinPathListInfo = new List<CP_ClinicalPathList>();
        //            foreach (DataRow row in dataTable.Rows)
        //            {
        //                CP_ClinicalPathList cliListInfo = new CP_ClinicalPathList(row["Ljdm"].ToString(), row["Name"].ToString(), row["Ljms"].ToString(),
        //                    decimal.Parse(row["Zgts"].ToString()), decimal.Parse(row["Jcfy"].ToString()), decimal.Parse(row["Vesion"].ToString()),
        //                    row["Cjsj"].ToString(), row["Shsj"].ToString(), row["Syks"].ToString(), row["Yxjl"].ToString(),
        //                    row["Syks"].ToString(), row["DeptName"].ToString(), row["ShysName"].ToString(), int.Parse(row["YxjlId"].ToString()),
        //                    UnzipContent(row["WorkFlowXML"].ToString()));
        //                cliListInfo.LjSyqk = row["LjSyqk"].ToString();
        //                cliListInfo.LjSysl = decimal.Parse(row["LjSySl"].ToString());
        //                clinPathListInfo.Add(cliListInfo);
        //            }
        //            //   long l = GetSpaceForObject(clinPathListInfo);
        //            return clinPathListInfo;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    //}
        //}         // zm 8.25

        private string UnzipContent(string emrContent)
        {
            try
            {
                if (emrContent == string.Empty)
                    return "";
                byte[] rbuff = Convert.FromBase64String(emrContent);
                MemoryStream ms = new MemoryStream(rbuff);
                DeflateStream dfs = new DeflateStream(ms, CompressionMode.Decompress, true);
                StreamReader sr = new StreamReader(dfs, Encoding.UTF8);
                string sXml = sr.ReadToEnd();
                sr.Close();
                dfs.Close();
                return sXml;
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e);
                return emrContent;
            }
        }

        public string ZipContent(string emrContent)
        {
            byte[] buffUnzipXml = Encoding.UTF8.GetBytes(emrContent);
            MemoryStream ms = new MemoryStream();
            DeflateStream dfs = new DeflateStream(ms, CompressionMode.Compress, true);
            dfs.Write(buffUnzipXml, 0, buffUnzipXml.Length);
            dfs.Close();
            ms.Seek(0, SeekOrigin.Begin);
            byte[] buffZipXml = new byte[ms.Length];
            ms.Read(buffZipXml, 0, buffZipXml.Length);
            return Convert.ToBase64String(buffZipXml);
        }


        /// <summary>
        /// 路径管理insert
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public String InsertCPList(string strName, string strLjms, double zgts, double jcfy, double version, string strShys,
            int yxjl, string strSyks, List<CP_ClinicalDiagnosisList> listAddBzdm)
        {
            using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            {
                String strLjdm = "0";

                SqlTransaction sqlTrans = null;
                try
                {
                    // myConnection.Open();
                    //sqlTrans = myConnection.BeginTransaction();//事务开始   
                    //SqlCommand myCommand = new SqlCommand("usp_CP_InsertClinicalPathInfo", myConnection, sqlTrans);
                    //myCommand.CommandType = CommandType.StoredProcedure;
                    //添加输入查询参数、赋予值

                    SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@Name",strName),
                        new SqlParameter("@Ljms",strLjms),
                        new SqlParameter("@Zgts",zgts),
                        new SqlParameter("@Jcfy",jcfy),
                        new SqlParameter("@Vesion",version),
                        new SqlParameter("@Shys",strShys),
                        new SqlParameter("@Yxjl",yxjl),
                        new SqlParameter("@Syks",strSyks),
                        new SqlParameter("@Bzdm",listAddBzdm[0].Bzdm.ToString())
                    };

                    //myCommand.Parameters.Add("@Name", SqlDbType.VarChar, 64);
                    //myCommand.Parameters.Add("@Ljms", SqlDbType.VarChar, 255);
                    //myCommand.Parameters.Add("@Zgts", SqlDbType.Decimal);
                    //myCommand.Parameters.Add("@Jcfy", SqlDbType.Decimal);
                    //myCommand.Parameters.Add("@Vesion", SqlDbType.Decimal);
                    //myCommand.Parameters.Add("@Shys", SqlDbType.VarChar, 6);
                    //myCommand.Parameters.Add("@Yxjl", SqlDbType.Decimal);
                    //myCommand.Parameters.Add("@Syks", SqlDbType.VarChar, 12);
                    //myCommand.Parameters.Add("@Bzdm", SqlDbType.VarChar, 12);
                    //myCommand.Parameters["@Name"].Value = strName;
                    //myCommand.Parameters["@Ljms"].Value = strLjms;
                    //myCommand.Parameters["@Zgts"].Value = zgts;
                    //myCommand.Parameters["@Jcfy"].Value = jcfy;
                    //myCommand.Parameters["@Vesion"].Value = version;
                    //myCommand.Parameters["@Shys"].Value = strShys;
                    //myCommand.Parameters["@Yxjl"].Value = yxjl;
                    //myCommand.Parameters["@Syks"].Value = strSyks;
                    //myCommand.Parameters["@Bzdm"].Value = listAddBzdm[0].Bzdm.ToString();

                    //SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(myCommand);
                    //DataTable dataTable = new DataTable();
                    //DataAdapter.Fill(dataTable);


                    //strLjdm = myCommand.ExecuteScalar().ToString();

                    //ZM 标记 oracle 事务没写，暂时取消事务  9.9

                    DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_InsertClinicalPathInfo", parameters, CommandType.StoredProcedure);

                    if (dt.Rows.Count != 0)
                    {

                        if (dt.Rows[0][0] != null)
                        {
                            strLjdm = dt.Rows[0][0].ToString();
                        }
                    }

                    if (strLjdm != "0")             //无同名
                    {
                        foreach (CP_ClinicalDiagnosisList lists in listAddBzdm)
                        {
                            InsertCPListDiagnoise(strLjdm, lists.Bzdm, lists.Bzmc);
                        }

                    }
                    //sqlTrans.Commit();//事务提交   

                }
                catch (Exception ex)
                {
                    //sqlTrans.Rollback();
                    ThrowException(ex);
                }
                //finally
                //{
                //    if (myConnection.State != System.Data.ConnectionState.Closed)
                //        myConnection.Close();
                //}
                return strLjdm;
            }
        }                                               // zm 8.25 Oracle

        /// <summary>
        /// 路径管理update
        /// 将返回值该为string，根据返回数据判断该数据库是否有值 ，也就是重复验证
        /// 修改时间：2013年8月13日 09:42:35
        /// 修改人：Jhonny
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public int UpdateCPList(string strLjdm, string strName, string strLjms, double zgts, double jcfy, double version,
            string strZgdm, int yxjl, string strSyks, List<CP_ClinicalDiagnosisList> listAddBzdm, List<CP_ClinicalDiagnosisList> listDelBzdm)
        {
            //SqlHelper.BeginTransaction();
            int count = 0;
            try
            {
                //判断修改的数据是否在数据库存在
                //修改时间：2013年8月13日 10:09:56
                //修改人：Jhonny
                string sql = string.Format("SELECT COUNT(NAME) FROM CP_Clinicalpath WHERE NAME ='{0}'",strName);
                //myConnection.Open();
                //sqlTrans = myConnection.BeginTransaction();//事务开始   
                //SqlCommand myCommand = new SqlCommand("usp_CP_UpdateClinicalPathInfo", myConnection, sqlTrans);
                //myCommand.CommandType = CommandType.StoredProcedure;
                //添加输入查询参数、赋予值              
                
                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@Ljdm",strLjdm),
                        new SqlParameter("@Name",strName),
                        new SqlParameter("@Ljms",strLjms),
                        new SqlParameter("@Zgts",zgts),
                        new SqlParameter("@Jcfy",jcfy),
                        new SqlParameter("@Vesion",version),
                        new SqlParameter("@Shys",strZgdm),
                        new SqlParameter("@Yxjl",yxjl),
                        new SqlParameter("@Syks",strSyks)
                    };
                //判断修改后的数据在数据库存在的数量  update 2013年8月13日 10:26:35  Jhonny
                count =Convert.ToInt32(SqlHelper.ExecuteScalar(sql));

                SqlHelper.ExecuteNoneQuery("usp_CP_UpdateClinicalPathInfo", parameters, CommandType.StoredProcedure);

                if (count ==0)
                {
                foreach (CP_ClinicalDiagnosisList lists in listAddBzdm)
                {
                    InsertCPListDiagnoise(strLjdm, lists.Bzdm, lists.Bzmc);
                }

                foreach (CP_ClinicalDiagnosisList lists in listDelBzdm)
                {
                    DeletCPListDiagnoise(strLjdm, lists.Bzdm);
                }
                
                InsertCPListLog(strLjdm, strZgdm);
                }
                //SqlHelper.CommitTransaction();
                //sqlTrans.Commit();//事务提交   
                return count;
            }
            catch (Exception ex)
            {
                //SqlHelper.RollbackTransaction();
                //sqlTrans.Rollback();
                ThrowException(ex);
                return 0;
            }
            //    finally
            //    {
            //        if (myConnection.State != System.Data.ConnectionState.Closed)
            //            myConnection.Close();
            //    }

        }       // zm 8.25 Oracle

        /// <summary>
        /// 路径管理LOG记录
        /// </summary>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        public void InsertCPListLog(string strLjdm, string strZgdm)
        {
            try
            {
                string strSql = string.Format("INSERT INTO CP_ClinicalPath_Log( Ljdm, Modify_User,MODIFY_TIME ) VALUES  ( '{0}', '{1}','{2}')", strLjdm, strZgdm, DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                //SqlCommand myCommand = new SqlCommand(strSql, myConnection, sqlTrans);
                //myCommand.CommandType = CommandType.Text;
                //添加输入查询参数、赋予值              

                //SqlParameter[] parameters = new SqlParameter[] 
                //{
                //    new SqlParameter("@Ljdm",strLjdm),
                //    new SqlParameter("@Name",strZgdm)
                //};

                //myCommand.Parameters.Add("@Ljdm", SqlDbType.VarChar, 12);
                //myCommand.Parameters.Add("@Name", SqlDbType.VarChar, 6);
                //myCommand.Parameters["@Ljdm"].Value = strLjdm;
                //myCommand.Parameters["@Name"].Value = strZgdm;

                //myCommand.ExecuteNoneQuery();

                SqlHelper.ExecuteNoneQuery(strSql);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }        // zm 8.25 Oracle




        /// <summary>
        /// 路径管理Insert
        /// </summary>
        /// <returns></returns>
        public void InsertCPListDiagnoise(string strLjdm, string strBzdm, string strBzmc)
        {

            try
            {
                //SqlCommand myCommand = new SqlCommand("usp_CP_InsertClinicalDiagnosis", myConnection, sqlTrans);
                //myCommand.CommandType = CommandType.StoredProcedure;
                //添加输入查询参数、赋予值

                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@Ljdm",strLjdm),
                    new SqlParameter("@Bzdm",strBzdm),
                    new SqlParameter("@Bzmc",strBzmc)
                };
                //myCommand.Parameters.Add("@Ljdm", SqlDbType.VarChar, 12);
                //myCommand.Parameters.Add("@Bzdm", SqlDbType.VarChar, 12);
                //myCommand.Parameters.Add("@Bzmc", SqlDbType.VarChar, 64);
                //myCommand.Parameters["@Ljdm"].Value = strLjdm;
                //myCommand.Parameters["@Bzdm"].Value = strBzdm;
                //myCommand.Parameters["@Bzmc"].Value = strBzmc;

                //myCommand.ExecuteNoneQuery();

                SqlHelper.ExecuteNoneQuery("usp_CP_InsertClinicalDiagnosis", parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }

        }       // zm 8.25 Oracle
        /// <summary>
        /// 路径管理 Delet
        /// </summary>
        /// <returns></returns>
        public void DeletCPListDiagnoise(string strLjdm, string strBzdm)
        {

            try
            {
                string strSql = "DELETE CP_ClinicalDiagnosis"
                                + " WHERE Ljdm = '" + strLjdm + "' AND Bzdm='" + strBzdm + "'";
                //SqlCommand myCommand = new SqlCommand(strSql, myConnection, sqlTrans);
                //myCommand.ExecuteNoneQuery();

                SqlHelper.ExecuteNoneQuery(strSql);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
        }       // zm 8.25 Oracle

        //ZM    8.4 注释
        /// <summary>
        /// 路径条件
        /// </summary>
        /// <param name="strLjdm"></param>
        /// <returns></returns> 
        //[OperationContract]
        //[FaultContract(typeof(LoginException))]
        //private List<CP_PathConditionList> GetPathConditionListByLjdm(string strLjdm)
        //{
        //    using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
        //    {
        //        try
        //        {
        //            DataTable dataTable = new DataTable();

        //            SqlCommand myCommand = new SqlCommand("usp_CP_PathConditionList", myConnection);
        //            myCommand.CommandType = CommandType.StoredProcedure;
        //            myCommand.Parameters.Add("@Ljdm", SqlDbType.VarChar, 12);
        //            myCommand.Parameters["@Ljdm"].Value = strLjdm;

        //            SqlDataAdapter DataAdapter = new System.Data.SqlClient.SqlDataAdapter(myCommand);


        //            if (dataTable != null)
        //            {
        //                DataAdapter.Fill(dataTable);
        //            }

        //            List<CP_PathConditionList> pathListInfo = new List<CP_PathConditionList>();
        //            foreach (DataRow row in dataTable.Rows)
        //            {
        //                CP_PathConditionList patListInfo = new CP_PathConditionList(row["Tjdm"].ToString(), row["Ljdm"].ToString(),
        //                    row["Tjmc"].ToString(), int.Parse(row["TJlb"].ToString()), row["TJlbName"].ToString(), int.Parse(row["Yxjl"].ToString()));
        //                pathListInfo.Add(patListInfo);
        //            }

        //            return pathListInfo;
        //        }
        //        catch (Exception ex)
        //        {
        //            throw ex;
        //        }
        //    }
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="strLjdm"></param>
        /// <param name="strTjmc"></param>
        /// <param name="tjlb"></param>
        /// <param name="yxjl"></param>
        //public void InsertPahtConditionList(SqlTransaction sqlTrans, SqlConnection myConnection, string strLjdm, string strTjmc, int tjlb, int yxjl)
        //{

        //    try
        //    {
        //        SqlCommand myCommand = new SqlCommand("usp_CP_InsertPathConditionList", myConnection, sqlTrans);
        //        myCommand.CommandType = CommandType.StoredProcedure;
        //        //添加输入查询参数、赋予值    
        //        myCommand.Parameters.Add("@Ljdm", SqlDbType.VarChar, 12);
        //        myCommand.Parameters.Add("@Tjmc", SqlDbType.VarChar, 255);
        //        myCommand.Parameters.Add("@Tjlb", SqlDbType.Decimal);
        //        myCommand.Parameters.Add("@Yxjl", SqlDbType.Decimal);
        //        myCommand.Parameters["@Ljdm"].Value = strLjdm;
        //        myCommand.Parameters["@Tjmc"].Value = strTjmc;
        //        myCommand.Parameters["@Tjlb"].Value = tjlb;
        //        myCommand.Parameters["@Yxjl"].Value = yxjl;
        //        myCommand.ExecuteNoneQuery();

        //        //string strLjdm = myCommand.ExecuteScalar().ToString();
        //        //foreach (CP_ClinicalDiagnosisList lists in listAddBzdm)
        //        //{
        //        //    InsertCPListDiagnoise(sqlTrans, myConnection, strLjdm, lists.Bzdm, lists.Bzmc);
        //        //} 
        //    }
        //    catch (Exception ex)
        //    {
        //        throw new ApplicationException(ex.Message);
        //    }
        //}


        /// <summary>
        ///    更新路径条件
        /// </summary>
        //public void UpdatePahtConditionList(SqlTransaction sqlTrans, SqlConnection myConnection, string strTjdm, string strTjmc, int tjlb, int yxjl)
        //{
        //    try
        //    {
        //        string strCmd = "UPDATE CP_PathCondition "
        //                        + " SET Tjmc = '{0}',"
        //                        + " TJlb  = {1},"
        //                        + " Yxjl = {2}"
        //                        + " WHERE Tjdm = '{3}'";
        //        string strSql = string.Format(strCmd, strTjmc, tjlb, yxjl, strTjdm);
        //        SqlCommand myCommand = new SqlCommand(strSql, myConnection, sqlTrans);
        //        myCommand.ExecuteNoneQuery();

        //        //string strLjdm = myCommand.ExecuteScalar().ToString();
        //        //foreach (CP_ClinicalDiagnosisList lists in listAddBzdm)
        //        //{
        //        //    InsertCPListDiagnoise(sqlTrans, myConnection, strLjdm, lists.Bzdm, lists.Bzmc);
        //        //} 
        //    }
        //    catch (Exception ex)
        //    {
        //        ThrowException(ex);
        //    }
        //}
        /// <summary>
        /// 删除路径条件
        /// </summary>
        //public void DeletePahtConditionList(SqlTransaction sqlTrans, SqlConnection myConnection, string strTjdm)
        //{
        //    try
        //    {
        //        string strCmd = "DELETE CP_PathCondition "
        //                        + " WHERE Tjdm = '{0}'";
        //        string strSql = string.Format(strCmd, strTjdm);
        //        SqlCommand myCommand = new SqlCommand(strSql, myConnection, sqlTrans);
        //        myCommand.ExecuteNoneQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        ThrowException(ex);
        //    }
        //}


        /// <summary>
        /// 更新路径XML
        /// </summary>
        /// <param name="workFlowXml"></param>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public void UpdateWorkFlowXML(string workFlowXml)
        {

            using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            {
                SqlTransaction sqlTrans = null;
                try
                {
                    //myConnection.Open();
                    //sqlTrans = myConnection.BeginTransaction();//事务开始

                    Byte[] bytes = System.Text.UTF8Encoding.UTF8.GetBytes(workFlowXml);
                    XElement xele = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(bytes)));
                    string strLjdm = xele.Attribute(XName.Get("UniqueID")).Value;

                    string strSql = @"update CP_ClinicalPath set WorkFlowXML = '{0}' where Ljdm = '{1}'";
                    string strCmd = string.Format(strSql, ZipContent(workFlowXml), strLjdm);
                    //SqlCommand myCommand = new SqlCommand(strCmd, myConnection, sqlTrans);
                    //myCommand.ExecuteNoneQuery();

                    SqlHelper.ExecuteNoneQuery(strCmd);


                    DeleteExistsWorkFlow(strLjdm);

                    //InsertIntoWorkProcess(sqlTrans, myConnection, xele.Attribute(XName.Get("UniqueID")).Value,
                    //    xele.Attribute(XName.Get("Name")).Value, workFlowXml);
                    List<MyActivity> MyActivitysTemp = WorkFlowTool.AnalyseFlowXMLNode(workFlowXml); //from item in xele.Descendants("Activity") select item;
                    foreach (MyActivity node in MyActivitysTemp)
                    {
                        InsertIntoPathDetail(node, strLjdm);
                    }

                    //partNos = from item in xele.Descendants("Rule") select item;
                    //foreach (XElement node in partNos)
                    //{
                    //    InsertIntoWorkRule(sqlTrans, myConnection, node);
                    
                    //}

                    // sqlTrans.Commit();
                }
                catch (Exception ex)
                {
                    //sqlTrans.Rollback();
                    ThrowException(ex);
                }
                //finally
                //{
                //    if (myConnection.State != System.Data.ConnectionState.Closed)
                //        myConnection.Close();
                //}
            }
        }       // zm 8.25 Oracle
        /// <summary>
        ///删除己经存在的WORKFLOW
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="strLjdm"></param>
        private void DeleteExistsWorkFlow(string strLjdm)
        {
            try
            {
                string strSql = @"delete from CP_PathDetail where Ljdm ='{0}'";
                string strCmd = string.Format(strSql, strLjdm);
                //SqlCommand myCommand = new SqlCommand(strCmd, myConnection, sqlTrans);
                //myCommand.ExecuteNoneQuery();

                SqlHelper.ExecuteNoneQuery(strCmd);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }

        }       // zm 8.25 Oracle

        /// <summary>
        /// WORKFLOW节点详细信息
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="node"></param>
        /// <param name="strLjdm"></param>
        private void InsertIntoPathDetail(MyActivity node, string strLjdm)
        {
            try
            {
                string strSql = "INSERT INTO CP_PathDetail( PahtDetailID ,Ljdm ,Ljmc,PrePahtDetailID,NextPahtDetailID,ActivityType)"
                                + " VALUES  ('{0}','{1}','{2}','{3}','{4}','{5}')";
                string strCmd = string.Format(strSql, node.UniqueID, strLjdm, node.ActivityName, node.PreNodeUniqueID, node.NestNodeUniqueID, node.ActivityType);
                //SqlCommand myCommand = new SqlCommand(strCmd, myConnection, sqlTrans);
                //myCommand.ExecuteNoneQuery();

                SqlHelper.ExecuteNoneQuery(strCmd);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
        }       // zm 8.25 Oracle

        /// <summary>
        /// 得到对应的COFNIG值
        /// </summary>
        /// <param name="strConfigKey"></param>
        /// <returns></returns>
        //[OperationContract]
        //[FaultContract(typeof(LoginException))]
        //public string GetConfigValue(string strConfigKey)
        //{
        //    string strConfigValue = string.Empty;
        //    using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
        //    {
        //        string strSql = @"SELECT Configkey, Name ,Value FROM CP_AppConfig WHERE Configkey = '{0}' AND Valid = 1";
        //        string strCmd = string.Format(strSql, strConfigKey);
        //        SqlCommand cmd = new SqlCommand(strCmd, myConnection);
        //        SqlDataAdapter dataAdapter = new SqlDataAdapter(cmd);
        //        DataTable dataTable = new DataTable();
        //        dataAdapter.Fill(dataTable);
        //        foreach (DataRow row in dataTable.Rows)
        //        {
        //            strConfigValue = row["Value"].ToString();
        //            break;
        //        }
        //    }
        //    return strConfigValue;
        //}         zm      8.25

        /// <summary>
        /// 根据据路径结点,得到成套医令信息
        /// </summary>
        /// <param name="strPathDetailID"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_DoctorOrder> GetPathInitOrder(string strPathDetailID, string strSyxh, CP_Employee employee, string sLjdm)
        {
            List<CP_DoctorOrder> listOrder = new List<CP_DoctorOrder>();
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            //SqlCommand myCommand = new SqlCommand("usp_CP_GetPathEnforceInitOrder", myConnection);
            //myCommand.CommandType = CommandType.StoredProcedure;

            SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@PahtDetailID",strPathDetailID),
                    new SqlParameter("@Ljdm",sLjdm)
                };

            //myCommand.Parameters.Add("@PahtDetailID", SqlDbType.VarChar, 50);
            //myCommand.Parameters["@PahtDetailID"].Value = strPathDetailID;

            //DataTable dataTable = new DataTable();
            //SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(myCommand);
            //dataAdapter.Fill(dataTable);

            DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_GetPathEnforceInitOrder", parameters, CommandType.StoredProcedure);

            foreach (DataRow row in dataTable.Rows)
            {
                CP_DoctorOrder order = new CP_DoctorOrder();
                order.Ctmxxh = Convert.ToDecimal(row["Ctmxxh"].ToString() == string.Empty ? "0" : row["Ctmxxh"].ToString());//
                order.Syxh = Convert.ToDecimal(strSyxh);
                order.Bqdm = employee.Bqdm;
                order.Ksdm = employee.Ksdm;
                order.Lrysdm = employee.Zgdm;
                order.Cdxh = Convert.ToDecimal(row["Cdxh"].ToString() == string.Empty ? "0" : row["Cdxh"].ToString());//
                order.Ggxh = Convert.ToDecimal(row["Ggxh"].ToString() == string.Empty ? "0" : row["Ggxh"].ToString()); //
                order.Lcxh = Convert.ToDecimal(row["Lcxh"].ToString() == string.Empty ? "0" : row["Lcxh"].ToString());//
                order.Ypdm = row["Ypdm"].ToString(); //
                order.Xmlb = Convert.ToDecimal(row["Xmlb"].ToString() == string.Empty ? "0" : row["Xmlb"].ToString()); //

                order.Yzlb = Convert.ToDecimal(row["Yzlb"].ToString() == string.Empty ? "0" : row["Yzlb"].ToString()); //
                order.Yzbz = Convert.ToDecimal(row["Yzbz"].ToString() == string.Empty ? "0" : row["Yzbz"].ToString()); //
                order.YzbzName = row["YzbzName"].ToString();  //
                order.Ypjl = Convert.ToDecimal(row["Ypjl"].ToString() == string.Empty ? "0" : row["Ypjl"].ToString()); //
                order.Jldw = row["Jldw"].ToString();//
                order.Yfdm = row["Yfdm"].ToString(); //
                order.YfdmName = row["YfdmName"].ToString(); //
                order.Pcdm = row["Pcdm"].ToString();      //
                order.PcdmName = row["PcdmName"].ToString();   //
                order.Ksrq = GetDefaultOrderTime((OrderType)(Convert.ToDecimal(row["Yzbz"].ToString())));
                order.Ypmc = row["Ypmc"].ToString(); //
                order.FromTable = row["FromTable"].ToString();//
                order.Flag = row["Flag"].ToString();//
                order.OrderGuid = Guid.NewGuid().ToString();//  
                order.Fzbz = Convert.ToDecimal(row["Fzbz"].ToString() == string.Empty ? "0" : row["Fzbz"].ToString());
                order.Fzxh = Convert.ToDecimal(row["Fzxh"].ToString() == string.Empty ? "0" : row["Fzxh"].ToString());
                order.Zxdw = row["Zxdw"].ToString();
                order.Ypgg = row["Ypgg"].ToString();
                order.Dwxs = Convert.ToDecimal(row["Dwxs"].ToString() == string.Empty ? "0" : row["Dwxs"].ToString());
                order.Dwlb = Convert.ToDecimal(row["Dwlb"].ToString() == string.Empty ? "0" : row["Dwlb"].ToString());


                order.Zxcs = Convert.ToDecimal(row["Zxcs"].ToString() == string.Empty ? "0" : row["Zxcs"].ToString());
                order.Zxzq = Convert.ToDecimal(row["Zxzq"].ToString() == string.Empty ? "0" : row["Zxzq"].ToString());
                order.Zxzqdw = Convert.ToDecimal(row["Zxzqdw"].ToString() == string.Empty ? "0" : row["Zxzqdw"].ToString());
                order.Zdm = row["Zdm"].ToString();
                order.Zxsj = row["Zxsj"].ToString();
                //add by luff 20130313
                order.Jjlx = int.Parse(row["Isjj"].ToString());
                order.Yzkx = int.Parse(row["Yzkx"].ToString());
                order.Zxksdm = row["Zxksdm"].ToString();
                order.Yznr = row["Yznr"].ToString();
                order.Ztnr = row["Ztnr"].ToString();
                order.Yzzt = Convert.ToDecimal(row["Yzzt"].ToString() == string.Empty ? "0" : row["Yzzt"].ToString());

                listOrder.Add(order);
            }
            //}
            return listOrder;
        }       // zm 8.25 Oracle

        /// <summary>
        /// 路径执行,医嘱默认时间
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        private string GetDefaultOrderTime(OrderType orderType)
        {
            string strTime = string.Empty;
            //add by yxy  2013-09-04  修改长期医嘱默认开始时间  根据钟山医院需求将默认时间与临时医嘱默认时间改为一样的
            //if (orderType == OrderType.Long)
            //    strTime = Convert.ToString(DateTime.Now.AddDays(1).Date + new TimeSpan(8, 0, 0));
            //else
            //{
                int hour = DateTime.Now.Hour;
                int minute = DateTime.Now.Minute;
                if (minute <= 30)
                    minute = 30;
                else
                {
                    hour += 1;
                    minute = 0;
                }
                strTime = Convert.ToString(DateTime.Today + new TimeSpan(hour, minute, 0));
            //}
            return strTime;
        }

        /// <summary>
        /// 根据据路径结点和首页序号,得到执行过的
        /// </summary>
        /// <param name="strPathDetailID"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_DoctorOrder> GetPathEnforcedOrder(string strPathDetailID, string strSyxh, string sLjdm)
        {
            List<CP_DoctorOrder> listOrder = new List<CP_DoctorOrder>();
            try
            {
                //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
                //{
                //SqlCommand myCommand = new SqlCommand("usp_CP_GetPathEnforceOrder", myConnection);
                //myCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@Xyxh",decimal.Parse(strSyxh)),
                        new SqlParameter("@PahtDetailID",strPathDetailID),
                        new SqlParameter("@Ljdm",sLjdm)
                    };

                //myCommand.Parameters.Add("@Xyxh", SqlDbType.Decimal);
                //myCommand.Parameters.Add("@PahtDetailID", SqlDbType.VarChar, 50);

                //myCommand.Parameters["@Xyxh"].Value = decimal.Parse(strSyxh);
                //myCommand.Parameters["@PahtDetailID"].Value = strPathDetailID;

                //DataTable dataTable = new DataTable();
                //SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(myCommand);
                //dataAdapter.Fill(dataTable);

                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_GetPathEnforceOrder", parameters, CommandType.StoredProcedure);

                foreach (DataRow row in dataTable.Rows)
                {
                    CP_DoctorOrder order = new CP_DoctorOrder();
                    order.Yzxh = Convert.ToDecimal(row["Yzxh"].ToString() == string.Empty ? "0" : row["Yzxh"].ToString());
                    order.Syxh = Convert.ToDecimal(row["Syxh"].ToString() == string.Empty ? "0" : row["Syxh"].ToString());
                    order.Bqdm = row["Bqdm"].ToString();
                    order.Ksdm = row["Ksdm"].ToString();
                    order.Lrysdm = row["Lrysdm"].ToString();
                    order.Lrrq = row["Lrrq"].ToString();
                    order.Shczy = row["Shczy"].ToString();
                    order.Shrq = row["Shrq"].ToString();
                    order.Zxczy = row["Zxczy"].ToString();
                    order.Zxrq = row["Zxrq"].ToString();
                    order.Qxysdm = row["Qxysdm"].ToString();
                    order.Qxrq = row["Qxrq"].ToString();
                    order.Ksrq = row["Ksrq"].ToString();
                    order.Yzbz = Convert.ToDecimal(row["Yzbz"].ToString() == string.Empty ? "0" : row["Yzbz"].ToString());
                    order.Fzxh = Convert.ToDecimal(row["Fzxh"].ToString() == string.Empty ? "0" : row["Fzxh"].ToString());
                    order.Fzbz = Convert.ToDecimal(row["Fzbz"].ToString() == string.Empty ? "0" : row["Fzbz"].ToString());
                    order.Cdxh = Convert.ToDecimal(row["Cdxh"].ToString() == string.Empty ? "0" : row["Cdxh"].ToString());//   
                    order.Ypgg = row["Ypgg"].ToString();
                    order.Ggxh = Convert.ToDecimal(row["Ggxh"].ToString() == string.Empty ? "0" : row["Ggxh"].ToString()); //
                    order.Lcxh = Convert.ToDecimal(row["Lcxh"].ToString() == string.Empty ? "0" : row["Lcxh"].ToString());// 
                    order.Ypdm = row["Ypdm"].ToString(); //  
                    order.Ypmc = row["Ypmc"].ToString(); //
                    order.Xmlb = Convert.ToDecimal(row["Xmlb"].ToString() == string.Empty ? "0" : row["Xmlb"].ToString()); //
                    order.Zxdw = row["Zxdw"].ToString();
                    order.Ypjl = Convert.ToDecimal(row["Ypjl"].ToString() == string.Empty ? "0" : row["Ypjl"].ToString()); // 
                    order.Jldw = row["Jldw"].ToString();//  
                    order.Dwxs = Convert.ToDecimal(row["Dwxs"].ToString() == string.Empty ? "0" : row["Dwxs"].ToString());
                    order.Dwlb = Convert.ToDecimal(row["Dwlb"].ToString() == string.Empty ? "0" : row["Dwlb"].ToString());
                    order.Yfdm = row["Yfdm"].ToString(); //     
                    order.YfdmName = row["YfdmName"].ToString(); // 
                    order.Pcdm = row["Pcdm"].ToString();      //   
                    order.PcdmName = row["PcdmName"].ToString();   //
                    order.Zxcs = Convert.ToDecimal(row["Zxcs"].ToString() == string.Empty ? "0" : row["Zxcs"].ToString());
                    order.Zxzq = Convert.ToDecimal(row["Zxzq"].ToString() == string.Empty ? "0" : row["Zxzq"].ToString());
                    order.Zxzqdw = Convert.ToDecimal(row["Zxzqdw"].ToString() == string.Empty ? "0" : row["Zxzqdw"].ToString());
                    order.Zdm = row["Zdm"].ToString();
                    order.Zxsj = row["Zxsj"].ToString();
                    order.Zxts = Convert.ToDecimal(row["Zxts"].ToString() == string.Empty ? "0" : row["Zxts"].ToString());
                    order.Ypzsl = Convert.ToDecimal(row["Ypzsl"].ToString() == string.Empty ? "0" : row["Ypzsl"].ToString());
                    order.Zxks = row["Zxks"].ToString();
                    order.Ztnr = row["Ztnr"].ToString();
                    order.Yzlb = Convert.ToDecimal(row["Yzlb"].ToString() == string.Empty ? "0" : row["Yzlb"].ToString()); // 
                    order.YzbzName = row["YzbzName"].ToString();  // 
                    order.Yzzt = Convert.ToDecimal(row["Yzzt"].ToString() == string.Empty ? "0" : row["Yzzt"].ToString());
                    order.Tsbj = Convert.ToDecimal(row["Tsbj"].ToString() == string.Empty ? "0" : row["Tsbj"].ToString());
                    order.Ybsptg = Convert.ToDecimal(row["Ybsptg"].ToString() == string.Empty ? "0" : row["Ybsptg"].ToString());
                    order.Ybspbh = row["Ybsptg"].ToString();  // 
                    order.Tzxh = Convert.ToDecimal(row["Tzxh"].ToString() == string.Empty ? "0" : row["Tzxh"].ToString());
                    order.Tzrq = row["Tzrq"].ToString();  // 
                    order.Sqdxh = Convert.ToDecimal(row["Sqdxh"].ToString() == string.Empty ? "0" : row["Sqdxh"].ToString());
                    order.Yznr = row["Yznr"].ToString();
                    order.Tbbz = Convert.ToDecimal(row["Tbbz"].ToString() == string.Empty ? "0" : row["Tbbz"].ToString());
                    order.Memo = row["Memo"].ToString();
                    order.Djfl = row["Djfl"].ToString();
                    order.Bxbz = Convert.ToDecimal(row["Bxbz"].ToString() == string.Empty ? "0" : row["Bxbz"].ToString());
                    order.PathDetailID = row["PathDetailID"].ToString();
                    order.Tzysdm = row["Tzysdm"].ToString();
                    order.Tzshhs = row["Tzshhs"].ToString();
                    order.Tzshrq = row["Tzshrq"].ToString();
                    order.Mq = Convert.ToDecimal(row["Mq"].ToString() == string.Empty ? "0" : row["Mq"].ToString());
                    order.Yztzyy = Convert.ToDecimal(row["Yztzyy"].ToString() == string.Empty ? "0" : row["Yztzyy"].ToString());
                    order.Ssyzxh = Convert.ToDecimal(row["Ssyzxh"].ToString() == string.Empty ? "0" : row["Ssyzxh"].ToString());
                    order.Jjlx = int.Parse(row["Isjj"].ToString());
                    order.Zxksdm = row["Zxksdm"].ToString();
                    //add by luff 20130313 医嘱可选
                    order.Yzkx = int.Parse(row["Yzkx"].ToString());
                    //add by luff 20130829 排序字段
                    order.OrderValue = int.Parse(row["OrderValue"].ToString());
                    order.FromTable = row["FromTable"].ToString();//
                    order.Flag = row["Flag"].ToString();//
                    order.Ctmxxh = Convert.ToDecimal(row["Ctmxxh"].ToString() == string.Empty ? "0" : row["Ctmxxh"].ToString());
                    order.OrderGuid = Guid.NewGuid().ToString();//                     

                    listOrder.Add(order);
                }
                //}
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return listOrder;
        }       // zm 8.25 Oracle



        #endregion

        #region 路径执行医嘱相关
        /// <summary>
        ///  路径执行时,保存,更新相应的信息
        /// </summary>
        /// <param name="strEnforceXml">路径XML</param>
        /// <param name="strLjdm">主路径代码</param>
        /// <param name="strActivityID">结点ID</param>
        /// <param name="strActivityChildID">子结点ID</param>
        /// <param name="listOrderAdd">新增医嘱</param>
        /// <param name="listOrderModify">修改医嘱</param>
        /// <param name="currentList">当前病患实体</param>
        /// <param name="listUnEnforceReason">不执行原因</param>
        /// <param name="listNewOrderReason">新增原因</param>
        /// <param name="otherReason">其它原因</param>
        /// <param name="listOrderDel">删除医嘱</param>  
        /// <param name="isLeadIn">是否引入了新的路径</param>
        /// <param name="listLjdm">路径代码集合</param>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public void SaveEnforceXmlOrder(string strEnforceXml, string strLjdm,
            string strActivityID, string strActivityChildID,
            List<CP_DoctorOrder> listOrderAdd, List<CP_DoctorOrder> listOrderModify,
            CP_InpatinetList currentList,
            List<CP_VariantRecords> listUnEnforceReason, List<CP_VariantRecords> listNewOrderReason,
            List<CP_VariantRecords> otherReason, List<CP_DoctorOrder> listOrderDel,
            Boolean isLeadIn, List<string> listLjdm)
        {
            //保存成功后将 currentList.Ljts, currentList.EnForceWorkFlowXml  更新

            //SqlHelper.BeginTransaction();
            try
            {
                //myConnection.Open();
                //sqlTrans = myConnection.BeginTransaction();//事务开始 
                UpdatePatientPathEnForce(currentList, ZipContent(strEnforceXml));
                SaveOrder(listOrderAdd, listOrderModify, listOrderDel, currentList, strActivityID, strActivityChildID, strLjdm);


                foreach (CP_VariantRecords reason in listUnEnforceReason)
                {
                    InsertVariantRecords(reason, currentList, strActivityChildID, strActivityID);
                }
                foreach (CP_VariantRecords reason in listNewOrderReason)
                {
                    InsertVariantRecords(reason, currentList, strActivityChildID, strActivityID);
                }
                foreach (CP_VariantRecords reason in otherReason)
                {
                    InsertVariantRecords(reason, currentList, strActivityChildID, strActivityID);
                }

                if (isLeadIn)
                {
                    foreach (String str in listLjdm)
                        InsertPathLeadInRecord(currentList, str);
                }
                //SqlHelper.CommitTransaction();

                //sqlTrans.Commit();//事务提交   
            }
            catch (Exception ex)
            {
                //sqlTrans.Rollback();
                ThrowException(ex);
                //SqlHelper.RollbackTransaction();
            }

            //finally
            //{
            //    if (myConnection.State != System.Data.ConnectionState.Closed)
            //        myConnection.Close();
            //}


        }


        /// <summary>
        /// Update  PatientPathEnForce
        /// 点击下一步时进行更新
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="currentList"></param>
        /// <param name="strEnforceXml"></param>
        private void UpdatePatientPathEnForce(CP_InpatinetList currentList, string strEnforceXml)
        {
            try
            {
                string strSql = @"UPDATE CP_InPatientPathEnForce SET EnFroceXml = '{0}' where Ljxh = '{1}'";

                strSql = string.Format(strSql, strEnforceXml, currentList.Ljxh);

                SqlHelper.ExecuteNoneQuery(strSql);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       // zm 8.25 Oracle

        /// <summary>
        /// 更新执行XML情况和ljts  
        /// 点击下一步时进行更新
        /// 修改:方全伟 laolaowhn 时间:2011-5-11  修改内容:插入路径执行的节点到路径执行明细表
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="currentList">病患实体</param>
        /// <param name="strEnforceXml">需要更新的XML</param>  
        /// <param name="strActivityID">结点ID</param> 
        /// <param name="listUnEnforceReason">不执行原因</param>
        /// <param name="listNewOrderReason">新增原因</param>
        /// <param name="listUnEnforceReason">其它原因</param>
        /// <param name="isLeadIn">是否引入了新的路径</param>
        /// <param name="listLjdm">路径代码集合</param>
        ///   <param name="sVal">当前选择那中医嘱列表，0表示西药、建议检查、其他医嘱那个列表；1表示草药医嘱列表</param>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public void UpdateEnForceInfo(CP_InpatinetList currentList, string strEnforceXml, String strActivityChildID, List<CP_VariantRecords> listUnEnforceReason,
        List<CP_VariantRecords> listNewOrderReason, List<CP_VariantRecords> listOtherReason, Boolean isLeadIn, List<string> listLjdm, String ActivityUniqueID,String sVal)
        {

            try
            {
                //SqlHelper.BeginTransaction();
                //myConnection.Open();
                //sqlTrans = myConnection.BeginTransaction();
                string strSql = @"UPDATE CP_InPatientPathEnForce SET EnFroceXml = @EnforceXml WHERE Ljxh = @Ljxh;
                                  UPDATE CP_InPathPatient SET Ljts = @Ljts WHERE Id = @Ljxh";
                #region 更新路径执行的明细
                List<MyActivity> MyActivitysTemp = WorkFlowTool.AnalyseFlowXMLExecuteNode(strEnforceXml); //from item in xele.Descendants("Activity") select item;
                int i = 999;
                //string strDeleteSql2 = String.Format("delete from CP_InPatientPathEnForceDetail where Ljxh='{0}'", currentList.Ljxh.ToString());
                //SqlCommand cmd = new SqlCommand(strDeleteSql2, myConnection, sqlTrans);
                //cmd.ExecuteNoneQuery();
                List<MyActiveChildren> MyActiveChildrenTemp = WorkFlowTool.AnalyseFlowXMLExecuteChildNode(strEnforceXml);
                foreach (MyActivity MyActivityitem in MyActivitysTemp)
                {
                    i = i - 1;

                    InsertCP_InPatientPathEnForceDetail(currentList.Ljxh.ToString(), currentList.Ljdm, currentList.Syxh.ToString(), MyActivityitem.UniqueID, i, MyActivityitem.ActivityType, MyActivityitem.ActivityName);

                    List<MyActiveChildren> list = new List<MyActiveChildren>();
                    list = MyActiveChildrenTemp.Select(s => s).Where(s => s.ActivityUniqueID == MyActivityitem.UniqueID).ToList();  //zm 8.8 符合该父节点的子节点
                    int j = 999;

                    foreach (MyActiveChildren child in list)
                    {
                        j = j - 1;

                        InsertCP_PathExecuteFlowActivityChildren(child.ActivityUniqueID, child.ActivityChildrenID, j);
                    }
                }
                #endregion

                if (listUnEnforceReason != null)
                {
                    
                    foreach (CP_VariantRecords reason in listUnEnforceReason)
                    {
                        //add by luff 20130725
                        if (sVal == "0")//西药、检验检查、纯医嘱等其他医嘱列表下的变异标识
                        {
                            reason.xmlb = "0";
                        }
                        else if(sVal == "1") //草药列表下的变异标识
                        {
                            reason.xmlb = "99";
                        }
                        else
                        {
                            reason.xmlb = "-1";
                        }

                        InsertVariantRecords(reason, currentList, strActivityChildID, ActivityUniqueID);
                    }
                }

                if (isLeadIn)
                {
                    foreach (String str in listLjdm)
                        InsertPathLeadInRecord(currentList, str);
                }
                SqlParameter paraXml = new SqlParameter("@EnforceXml", SqlDbType.VarChar, 8000);
                paraXml.Value = ZipContent(strEnforceXml);
                SqlParameter paraLjxh = new SqlParameter("@Ljxh", SqlDbType.Decimal);
                paraLjxh.Value = currentList.Ljxh;
                SqlParameter paraLjts = new SqlParameter("@Ljts", SqlDbType.Decimal);
                paraLjts.Value = Int32.Parse(currentList.Ljts) + 1;

                SqlParameter[] paraList = new SqlParameter[] { paraXml, paraLjxh, paraLjts };

                SqlHelper.ExecuteNoneQuery(strSql.ToString(), paraList, CommandType.Text);
                //sqlTrans.Commit();//事务提交   
                EMRInsertCP_InPathPatient(currentList.Ljxh.ToString(), false, null, null);
                //SqlHelper.CommitTransaction();
            }
            catch (Exception ex)
            {
                //SqlHelper.RollbackTransaction();
                //sqlTrans.Rollback();
                ThrowException(ex);
            }
            finally
            {

            }

        }

        /// <summary>
        /// 若进行了路径引入的操作,则进行记录
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="currentList">病患实体</param>
        /// <param name="strLjdm">当前工作流里包含的工作路径代码</param>
        private void InsertPathLeadInRecord(CP_InpatinetList currentList, String strLjdm)
        {
            try
            {
                //SqlCommand myCommand = new SqlCommand("usp_CP_InsertPathLeadInRecord", myConnection, sqlTrans);
                //myCommand.CommandType = CommandType.StoredProcedure;
                //添加输入查询参数、赋予值

                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@Ljxh",currentList.Ljxh),
                    new SqlParameter("@Syxh",ConvertMy.ToDecimal(currentList.Syxh)),
                    new SqlParameter("@Ljdm",strLjdm),
                    new SqlParameter("@Zgm",currentList.CurOper)
                };

                //myCommand.Parameters.Add("@Ljxh", SqlDbType.Decimal);
                //myCommand.Parameters.Add("@Syxh", SqlDbType.Decimal);
                //myCommand.Parameters.Add("@Ljdm", SqlDbType.VarChar, 12);
                //myCommand.Parameters.Add("@Zgm", SqlDbType.VarChar, 6);
                //myCommand.Parameters["@Ljxh"].Value = currentList.Ljxh;
                //myCommand.Parameters["@Syxh"].Value = ConvertMy.ToDecimal(currentList.Syxh);
                //myCommand.Parameters["@Ljdm"].Value = strLjdm;
                //myCommand.Parameters["@Zgm"].Value = currentList.CurOper;
                //myCommand.ExecuteNoneQuery();

                SqlHelper.ExecuteNoneQuery("usp_CP_InsertPathLeadInRecord", parameters, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       // zm 8.25 Oracle


        ///// <summary>
        ///// 完成路径
        ///// </summary>
        ///// <param name="sqlTrans"></param>
        ///// <param name="myConnection"></param>
        ///// <param name="currentList"></param>
        ///// <param name="strEnforceXml"></param>
        //private void UpdateLjts(SqlTransaction sqlTrans, SqlConnection myConnection, CP_InpatinetList currentList)
        //{
        //    try
        //    {
        //        string strSql = @"UPDATE CP_InPathPatient SET Ljts = {0} WHERE Syxh ={1} AND Ljdm='{2}'";
        //        string strCmd = string.Format(strSql, (int.Parse(currentList.Ljts) + 1),int.Parse(currentList.Syxh), currentList.Ljdm);
        //        SqlCommand myCommand = new SqlCommand(strCmd, myConnection, sqlTrans);
        //        myCommand.ExecuteNoneQuery();
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}


        /// <summary>
        /// 完成路径
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="currentList"></param>
        /// <param name="strEnforceXml"></param>  
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public void UpdatePathStatusDone(CP_InpatinetList currentList, String strActivityChildID, List<CP_VariantRecords> listUnEnforceReason,
        List<CP_VariantRecords> listNewOrderReason, List<CP_VariantRecords> listOtherReason, String ActivityUniqueID, String sVal)
        {
            try
            {
                string strSql = @"UPDATE CP_InPathPatient SET Wcsj = '" + DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss") + "', Ljzt =3  WHERE Id = {0} ";
                string strCmd = string.Format(strSql, currentList.Ljxh);
                SqlHelper.ExecuteNoneQuery(strCmd);

                //同步状态
                SychPathStatus2Emr(currentList.Hissyxh, 3);

                //add by luff 20130323 保存医嘱变异信息
                if (listUnEnforceReason != null)
                {
                   
                    foreach (CP_VariantRecords reason in listUnEnforceReason)
                    {
                        //add by luff 20130725
                        if (sVal == "0")//西药、检验检查、纯医嘱等其他医嘱列表下的变异标识
                        {
                            reason.xmlb = "0";
                        }
                        else if (sVal == "1") //草药列表下的变异标识
                        {
                            reason.xmlb = "99";
                        }
                        else
                        {
                            reason.xmlb = "-1";
                        }
                        InsertVariantRecords(reason, currentList, strActivityChildID, ActivityUniqueID);
                    }
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
        }

        /// <summary>
        /// 保存医嘱,modify by xjt
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="listOrderAdd">新增医嘱</param>
        /// <param name="listOrderModify">修改医嘱</param>
        /// <param name="listOrderDel">删除医嘱</param>
        /// <param name="currentList">病患实体</param>
        /// <param name="strActivityID">活动ID</param>
        /// <param name="strActivityChildID">活动子结点ID</param>  
        /// <param name="strLjdm">结点所属路径</param>
        private void SaveOrder(List<CP_DoctorOrder> listOrderAdd,
            List<CP_DoctorOrder> listOrderModify, List<CP_DoctorOrder> listOrderDel, CP_InpatinetList currentList,
            string strActivityID, string strActivityChildID, string strLjdm)
        {
            if (listOrderAdd.Count > 0)
            {
                SaveAddOrder(listOrderAdd, currentList, strActivityID, strActivityChildID, strLjdm);
            }
            if (listOrderModify.Count > 0)
            {
                SaveModifyOrder(listOrderModify, currentList);
            }
            if (listOrderDel.Count > 0)
            {
                foreach (CP_DoctorOrder orderDel in listOrderDel)
                {
                    DelOrder(orderDel);
                }
            }
        }

        /// <summary>
        /// 将医嘱信息发送到HIS数据库中
        /// </summary>
        /// <param name="syxh">HIS首页序号</param>
        /// <param name="listOrderAdd">新增医嘱</param>
        /// <param name="executorCode">执行人工号</param>
        /// <param name="macAddress">网卡地址</param>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public string SendOrderToHis(CP_InpatinetList inpatient, ObservableCollection<CP_DoctorOrder> listOrderAdd, string executorCode, string macAddress)
        {

            try
            {

                DataTable changeTable = new DataTable("changeOrder");

                #region

                //new SqlParameter("@yzxh",dr["yzxh"].ToString()),
                //       new SqlParameter("@yzbz",dr["yzbz"].ToString()),
                //       new SqlParameter("@NoOfRecord",dr["NoOfRecord"].ToString()),
                //       new SqlParameter("@Syxh",dr["Syxh"].ToString()),
                //       new SqlParameter("@PatID",dr["PatID"].ToString()),

                //       new SqlParameter("@ksmc",dr["ksmc"].ToString()),
                //       new SqlParameter("@Qxrq",dr["Qxrq"].ToString()),
                //       new SqlParameter("@Qxysdm",dr["Qxysdm"].ToString()),
                //       new SqlParameter("@OutBed",dr["OutBed"].ToString()),
                DataColumn yzxh = new DataColumn("yzxh", Type.GetType("System.String"));//医嘱序号
                DataColumn ctyzxh = new DataColumn("ctyzxh", Type.GetType("System.String"));//成套医嘱序号
                DataColumn yzbz = new DataColumn("yzbz", Type.GetType("System.String"));//医嘱标志
                DataColumn NoOfRecord = new DataColumn("NoOfRecord", Type.GetType("System.String"));
                DataColumn Syxh = new DataColumn("Syxh", Type.GetType("System.String"));
                DataColumn PatID = new DataColumn("PatID", Type.GetType("System.String"));
                DataColumn Hzxm = new DataColumn("hzxm", Type.GetType("System.String"));

                DataColumn NoofClinic = new DataColumn("NoofClinic", Type.GetType("System.String"));
                DataColumn InCount = new DataColumn("InCount", Type.GetType("System.String"));//入院次数

                DataColumn ksmc = new DataColumn("ksmc", Type.GetType("System.String"));//科室名称
                DataColumn Qxrq = new DataColumn("Qxrq", Type.GetType("System.String"));//取消日期
                DataColumn Qxysdm = new DataColumn("Qxysdm", Type.GetType("System.String"));//取消医生代码
                DataColumn OutBed = new DataColumn("OutBed", Type.GetType("System.String"));//床位
                DataColumn bqdm = new DataColumn("bqdm", Type.GetType("System.String"));//病区代码
                DataColumn ksdm = new DataColumn("ksdm", Type.GetType("System.String"));//科室代码

                DataColumn fzxh = new DataColumn("fzxh", Type.GetType("System.String"));//分组序号
                DataColumn fzbz = new DataColumn("fzbz", Type.GetType("System.String"));//分组标志
                DataColumn lrysdm = new DataColumn("lrysdm", Type.GetType("System.String"));//录入医生代码
                DataColumn ksrq = new DataColumn("ksrq", Type.GetType("System.String"));//开始日期
                DataColumn yzlb = new DataColumn("yzlb", Type.GetType("System.String"));//医嘱类别

                DataColumn cdxh = new DataColumn("cdxh", Type.GetType("System.String"));//产地序号
                DataColumn Ggxh = new DataColumn("Ggxh", Type.GetType("System.String"));//规格序号
                DataColumn ypdm = new DataColumn("ypdm", Type.GetType("System.String"));//药品代码
                DataColumn ypmc = new DataColumn("ypmc", Type.GetType("System.String"));//药品名称
                DataColumn Xmlb = new DataColumn("Xmlb", Type.GetType("System.String"));//项目类别
                DataColumn Zxdw = new DataColumn("Zxdw", Type.GetType("System.String"));//最小单位

                DataColumn zxks = new DataColumn("zxks", Type.GetType("System.String"));//执行科室
                DataColumn ypjl = new DataColumn("ypjl", Type.GetType("System.String"));//药品剂量
                DataColumn Jldw = new DataColumn("Jldw", Type.GetType("System.String"));//剂量单位

                DataColumn dwxs = new DataColumn("dwxs", Type.GetType("System.String"));//单位系数
                DataColumn dwlb = new DataColumn("dwlb", Type.GetType("System.String"));//单位类别
                DataColumn yfdm = new DataColumn("yfdm", Type.GetType("System.String"));//用法代码
                DataColumn yznr = new DataColumn("yznr", Type.GetType("System.String"));//医嘱内容
                DataColumn ztnr = new DataColumn("ztnr", Type.GetType("System.String"));//嘱托内容

                DataColumn pcdm = new DataColumn("pcdm", Type.GetType("System.String"));//频次代码
                DataColumn Zxcs = new DataColumn("Zxcs", Type.GetType("System.String"));//执行次数
                DataColumn Zxzq = new DataColumn("Zxzq", Type.GetType("System.String"));//执行周期
                DataColumn Zxzqdw = new DataColumn("Zxzqdw", Type.GetType("System.String"));//执行周期时间单位(-1:不规则周期，0：天,1：小时,2：分钟)
                DataColumn zdm = new DataColumn("zdm", Type.GetType("System.String"));//周代码
                DataColumn zxsj = new DataColumn("zxsj", Type.GetType("System.String"));//执行时间
                DataColumn tsbj = new DataColumn("tsbj", Type.GetType("System.String"));//特殊标记
                DataColumn tzxh = new DataColumn("tzxh", Type.GetType("System.String"));//停嘱序号

                //停止日期
                DataColumn tzrq = new DataColumn("tzrq", Type.GetType("System.String"));
                DataColumn Lrrq = new DataColumn("Lrrq", Type.GetType("System.String"));//录入日期

                DataColumn sqdxh = new DataColumn("sqdxh", Type.GetType("System.String"));//申请单序号
                DataColumn ybsptg = new DataColumn("ybsptg", Type.GetType("System.String"));//医保审批通过
                DataColumn ybspbh = new DataColumn("ybspbh", Type.GetType("System.String"));//医保审批编号

                DataColumn lsyzxh = new DataColumn("lsyzxh", Type.GetType("System.String"));//临时医嘱序号
                DataColumn cqyzxh = new DataColumn("cqyzxh", Type.GetType("System.String"));//长期医嘱序号
                DataColumn shczy = new DataColumn("shczy", Type.GetType("System.String"));//审核操作员
                DataColumn shrq = new DataColumn("shrq", Type.GetType("System.String"));//审核日期
                DataColumn qxysdm = new DataColumn("qxysdm", Type.GetType("System.String"));//取消医生代码

                DataColumn qxrq = new DataColumn("qxrq", Type.GetType("System.String"));//取消日期
                DataColumn yzzt = new DataColumn("yzzt", Type.GetType("System.String"));//医嘱状态
                DataColumn mq = new DataColumn("mq", Type.GetType("System.String"));//明起 长嘱
                DataColumn ypzsl = new DataColumn("ypzsl", Type.GetType("System.String"));//药品总数量
                DataColumn memo = new DataColumn("memo", Type.GetType("System.String"));
                DataColumn Ctmxxh = new DataColumn("Ctmxxh", Type.GetType("System.String"));//成套明细序号
                DataColumn Ypgg = new DataColumn("Ypgg", Type.GetType("System.String"));//


                //计价标准、执行科室
                DataColumn Jjlx = new DataColumn("Jjlx", Type.GetType("System.String"));
                DataColumn Zxksdm = new DataColumn("Zxksdm", Type.GetType("System.String"));//执行科室代码
                //记录发送是否成功、以及发送信息、未成功原因
                DataColumn FS_Flag = new DataColumn("FS_Flag", Type.GetType("System.String"));
                DataColumn FS_Mess = new DataColumn("FS_Mess", Type.GetType("System.String"));

                //操作员工号
                DataColumn Czyh = new DataColumn("Czyh", Type.GetType("System.String"));

                changeTable.Columns.Add(yzxh);
                changeTable.Columns.Add(ctyzxh);
                changeTable.Columns.Add(yzbz);
                changeTable.Columns.Add(NoOfRecord);
                changeTable.Columns.Add(Syxh);
                changeTable.Columns.Add(PatID);
                changeTable.Columns.Add(Hzxm);

                changeTable.Columns.Add(NoofClinic);
                changeTable.Columns.Add(InCount);

                changeTable.Columns.Add(ksmc);
                changeTable.Columns.Add(Qxrq);
                changeTable.Columns.Add(Qxysdm);
                changeTable.Columns.Add(OutBed);
                changeTable.Columns.Add(bqdm);
                changeTable.Columns.Add(ksdm);

                changeTable.Columns.Add(fzxh);
                changeTable.Columns.Add(fzbz);
                changeTable.Columns.Add(lrysdm);
                changeTable.Columns.Add(ksrq);
                changeTable.Columns.Add(yzlb);
                changeTable.Columns.Add(Zxdw);

                changeTable.Columns.Add(cdxh);
                changeTable.Columns.Add(Ggxh);
                changeTable.Columns.Add(ypdm);
                changeTable.Columns.Add(Xmlb);
                changeTable.Columns.Add(zxks);
                changeTable.Columns.Add(ypjl);
                changeTable.Columns.Add(Jldw);

                changeTable.Columns.Add(dwxs);
                changeTable.Columns.Add(dwlb);
                changeTable.Columns.Add(yfdm);
                changeTable.Columns.Add(yznr);
                changeTable.Columns.Add(ztnr);

                changeTable.Columns.Add(pcdm);
                changeTable.Columns.Add(Zxcs);
                changeTable.Columns.Add(Zxzq);
                changeTable.Columns.Add(Zxzqdw);
                changeTable.Columns.Add(zdm);
                changeTable.Columns.Add(zxsj);
                changeTable.Columns.Add(tsbj);
                changeTable.Columns.Add(tzxh);

                changeTable.Columns.Add(tzrq);
                changeTable.Columns.Add(Lrrq);
                changeTable.Columns.Add(ypmc);
                changeTable.Columns.Add(sqdxh);
                changeTable.Columns.Add(ybsptg);
                changeTable.Columns.Add(ybspbh);

                changeTable.Columns.Add(lsyzxh);
                changeTable.Columns.Add(cqyzxh);
                changeTable.Columns.Add(shczy);
                changeTable.Columns.Add(shrq);
                changeTable.Columns.Add(qxysdm);

                changeTable.Columns.Add(qxrq);
                changeTable.Columns.Add(yzzt);
                changeTable.Columns.Add(mq);
                changeTable.Columns.Add(ypzsl);
                changeTable.Columns.Add(memo);
                changeTable.Columns.Add(Ctmxxh);
                changeTable.Columns.Add(Ypgg);

                changeTable.Columns.Add(Jjlx);
                changeTable.Columns.Add(Zxksdm);

                changeTable.Columns.Add(FS_Flag);
                changeTable.Columns.Add(FS_Mess);

                changeTable.Columns.Add(Czyh);

                foreach (CP_DoctorOrder order in listOrderAdd)
                {
                    DataRow dr = changeTable.NewRow();

                    dr["yzxh"] = order.Yzxh;
                    dr["ctyzxh"] = order.Ctmxxh;
                    dr["yzbz"] = order.Yzbz;
                    dr["NoOfRecord"] = inpatient.NoOfRecord;
                    dr["Syxh"] = inpatient.Hissyxh;
                    dr["PatID"] = inpatient.patID;
                    dr["hzxm"] = inpatient.Name;

                    dr["NoofClinic"] = inpatient.NoofClinic;
                    dr["InCount"] = inpatient.InCount;

                    dr["ksmc"] = inpatient.CyksName;
                    dr["Qxrq"] = order.Qxrq;
                    dr["Qxysdm"] = order.Qxysdm;
                    dr["OutBed"] = inpatient.Bed;
                    dr["bqdm"] = inpatient.Cybq;// order.Bqdm;
                    dr["ksdm"] = order.Ksdm;

                    dr["fzxh"] = order.Fzxh;
                    dr["fzbz"] = order.Fzbz;
                    dr["lrysdm"] = order.Lrysdm;
                    dr["ksrq"] = order.Ksrq;
                    dr["yzlb"] = order.Yzlb;
                    dr["xmlb"] = order.Xmlb;
                    dr["Zxdw"] = order.Zxdw == null ? string.Empty : order.Zxdw;

                    dr["cdxh"] = order.Cdxh;
                    dr["Ggxh"] = order.Ggxh;
                    dr["ypdm"] = order.Ypdm;
                    dr["zxks"] = order.Zxks;
                    dr["ypjl"] = order.Ypjl <= 0 ? 1 : order.Ypjl;
                    dr["Jldw"] = order.Jldw;

                    dr["dwxs"] = order.Dwxs;
                    dr["dwlb"] = order.Dwlb;
                    dr["yfdm"] = order.Yfdm;
                    dr["yznr"] = order.Yznr;
                    dr["ztnr"] = order.Ztnr;

                    dr["pcdm"] = order.Pcdm;
                    dr["Zxcs"] = order.Zxcs;
                    dr["Zxzq"] = order.Zxzq;
                    dr["Zxzqdw"] = order.Zxzqdw;
                    dr["zdm"] = order.Zdm;
                    dr["zxsj"] = order.Zxsj;
                    dr["tsbj"] = order.Tsbj;
                    dr["tzxh"] = order.Tzxh;

                    dr["tzrq"] = order.Tzrq;
                    dr["Lrrq"] = order.Lrrq;
                    dr["ypmc"] = order.Ypmc;
                    dr["sqdxh"] = order.Sqdxh;
                    dr["ybsptg"] = order.Ybsptg;
                    dr["ybspbh"] = order.Ybspbh;

                    dr["lsyzxh"] = order.Yzxh;
                    dr["cqyzxh"] = order.Yzxh;
                    dr["shczy"] = order.Shczy;
                    dr["shrq"] = order.Shrq;
                    dr["qxysdm"] = order.Qxysdm;

                    dr["qxrq"] = order.Qxrq;
                    dr["yzzt"] = order.Yzzt;
                    dr["mq"] = order.Mq;
                    dr["ypzsl"] = order.Ypzsl;
                    dr["memo"] = order.Memo;
                    dr["Ctmxxh"] = order.Ctmxxh;
                    dr["Ypgg"] = order.Ypgg;

                    dr["Jjlx"] = order.Jjlx;
                    dr["Zxksdm"] = order.Zxksdm;

                    dr["FS_Flag"] = "1";

                    dr["Czyh"] = inpatient.CurOper;

                    changeTable.Rows.Add(dr);
                }
                #endregion


                string mess = "";
                //add luff 20130326 根据APPCFG配置参数，得到不同厂家his标识，调用不同发送医嘱的方法
                //获得厂家his标识
                List<APPCFG> hiscfg = GetAppCfg("HISTYPE");
                if (hiscfg.Count > 0)
                {
                    string sHisBs = hiscfg[0].Value.ToUpper().Trim();
                    #region 隐藏列表列
                    //隐藏数据列
                    switch (sHisBs)
                    {
                        case "XJ": //新疆医院His厂商标识

                            mess = SendOrderTableToHis_XJ(changeTable);
                            break;

                        case "BT": //漯河医院His厂商标识（金仕达His）

                            mess = SendOrderTableToHis_BT(changeTable);
                            break;

                        case "WINNING": //钟山医院His厂商标识（金仕达His）
                            mess = SendOrderTableToHis(inpatient.Hissyxh, changeTable, executorCode, macAddress);
                            break;

                        default:
                            mess = SendOrderTableToHis(inpatient.Hissyxh, changeTable, executorCode, macAddress);
                            break;
                    }
                    #endregion

                }
                else
                {
                    mess = SendOrderTableToHis(inpatient.Hissyxh, changeTable, executorCode, macAddress);
                }


                return mess;
            }
            catch (Exception e)
            {
                return e.Message;
            }
        }

        public string SendOrderTableToHis_BT(DataTable changeTable)
        {
            try
            {
                SendOrderToHis_BT send = new SendOrderToHis_BT();
                changeTable = send.SendOrder(changeTable);
                //根据返回的结果集返回发送消息
                string f = changeTable.Select("FS_Flag='0'").Length.ToString();
                string s = changeTable.Select("FS_Flag='1'").Length.ToString();
                //更新同步标志
                //"长期医嘱"  2703
                //"临时医嘱"  2702
                DataTable longorder = changeTable.Clone();
                DataTable temporder = changeTable.Clone();

                //foreach (DataRow dr in changeTable.Select("FS_Flag='1'"))
                //{
                //    if (dr["yzbz"].ToString() == "2703")
                //    {
                //        longorder.Rows.Add(dr.ItemArray);
                //    }
                //    else
                //    {
                //        temporder.Rows.Add(dr.ItemArray);
                //    }
                //}

                //更新同步标志

                UpdateSynchFlagToTrue(longorder, false);
                UpdateSynchFlagToTrue(temporder, true);


                string mess = "成功发送【" + s + "】条医嘱！" + "\n ";

                if (f != "0")
                {
                    if (s != "0")
                    {
                        mess = mess + "发送失败【" + f + "】条医嘱!如下:";
                    }
                    else
                    {
                        mess = "发送失败【" + f + "】条医嘱!如下:";
                    }

                    foreach (DataRow dr in changeTable.Select("FS_Flag='0'"))
                    {
                        mess = mess + "\n " + dr["ypmc"] + "【" + dr["FS_Mess"] + "】!";
                    }
                }
                return mess;
            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        public string SendOrderTableToHis_XJ(DataTable changeTable)
        {
            try
            {
                SendOrderToHIS_XJ m_sendorder = new SendOrderToHIS_XJ();

                if (m_sendorder == null) m_sendorder = new SendOrderToHIS_XJ();
                changeTable = m_sendorder.SendOrder(changeTable);

                //根据返回的结果集返回发送消息
                string f = changeTable.Select("FS_Flag='0'").Length.ToString();
                string s = changeTable.Select("FS_Flag='1'").Length.ToString();
                //更新同步标志
                //"长期医嘱"  2703
                //"临时医嘱"  2702
                DataTable longorder = changeTable.Clone();
                DataTable temporder = changeTable.Clone();

                foreach (DataRow dr in changeTable.Select("FS_Flag='1'"))
                {
                    if (dr["yzbz"].ToString() == "2703")
                    {
                        longorder.Rows.Add(dr.ItemArray);
                    }
                    else
                    {
                        temporder.Rows.Add(dr.ItemArray);
                    }
                }

                //更新同步标志

                UpdateSynchFlagToTrue(longorder, false);
                UpdateSynchFlagToTrue(temporder, true);


                string mess = "成功发送【" + s + "】条医嘱！" + "\n ";

                if (f != "0")
                {
                    if (s != "0")
                    {
                        mess = mess + "发送失败【" + f + "】条医嘱!如下:";
                    }
                    else
                    {
                        mess = "发送失败【" + f + "】条医嘱!如下:";
                    }

                    foreach (DataRow dr in changeTable.Select("FS_Flag='0'"))
                    {
                        mess = mess + "\n " + dr["ypmc"] + "【" + dr["FS_Mess"] + "】!";
                    }
                }
                return mess;

            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        private void UpdateSynchFlagToTrue_XJ(DataTable changeTable, bool iscqls)
        {
            if (iscqls)
            {
                foreach (DataRow dr in changeTable.Rows)
                {
                    string sql = @"update dbo.CP_TempOrder set Tbbz = 1 where Lsyzxh = '{0}'";

                    string strCmd = string.Format(sql, dr["yzxh"].ToString());
                    SqlHelper.ExecuteNoneQuery(strCmd);
                }
            }
            else
            {
                foreach (DataRow dr in changeTable.Rows)
                {
                    string sql = @"update dbo.CP_LongOrder set Tbbz = 1 where Cqyzxh = '{0}'";

                    string strCmd = string.Format(sql, dr["yzxh"].ToString());
                    SqlHelper.ExecuteNoneQuery(strCmd);
                }
            }
        }
        //add by luff 20130131 通用接口方法
        public string SendOrderTableToAllHis(DataTable changeTable)
        {
            try
            {
                SendOrderToAll m_sendorder = new SendOrderToAll();

                if (m_sendorder == null) m_sendorder = new SendOrderToAll();
                changeTable = m_sendorder.SendOrder(changeTable);

                //根据返回的结果集返回发送消息
                string f = changeTable.Select("FS_Flag='0'").Length.ToString();
                string s = changeTable.Select("FS_Flag='1'").Length.ToString();
                //更新同步标志
                //"长期医嘱"  2703
                //"临时医嘱"  2702
                DataTable longorder = changeTable.Clone();
                DataTable temporder = changeTable.Clone();

                foreach (DataRow dr in changeTable.Select("FS_Flag='1'"))
                {
                    if (dr["yzbz"].ToString() == "2703")
                    {
                        longorder.Rows.Add(dr.ItemArray);
                    }
                    else
                    {
                        temporder.Rows.Add(dr.ItemArray);
                    }
                }

                //更新同步标志

                UpdateSynchFlagToTrue(longorder, false);
                UpdateSynchFlagToTrue(temporder, true);


                string mess = "成功发送【" + s + "】条医嘱！" + "\n ";

                if (f != "0")
                {
                    if (s != "0")
                    {
                        mess = mess + "发送失败【" + f + "】条医嘱!如下:";
                    }
                    else
                    {
                        mess = "发送失败【" + f + "】条医嘱!如下:";
                    }

                    foreach (DataRow dr in changeTable.Select("FS_Flag='0'"))
                    {
                        mess = mess + "\n " + dr["ypmc"] + "【" + dr["FS_Mess"] + "】!";
                    }
                }
                return mess;

            }
            catch (Exception e)
            {
                return e.Message;
            }

        }

        SendOrderToHIS m_SendOrderHelper;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="syxh"></param>
        /// <param name="changedTable"></param>
        /// <param name="executorCode"></param>
        /// <param name="macAddress"></param>
        /// <returns></returns>
        public string SendOrderTableToHis(string syxh, DataTable changeTable, string executorCode, string macAddress)
        {
            try
            {
                SendOrderToHIS m_sendorder = new SendOrderToHIS();

                //"长期医嘱"  2703
                //"临时医嘱"  2702
                DataTable longorder = changeTable.Clone();
                DataTable temporder = changeTable.Clone();

                foreach (DataRow dr in changeTable.Rows)
                {
                    if (dr["yzbz"].ToString() == "2703")
                    {
                        longorder.Rows.Add(dr.ItemArray);
                    }
                    else
                    {
                        temporder.Rows.Add(dr.ItemArray);
                    }
                }
                if (longorder.Rows.Count > 0)
                {
                    longorder = m_sendorder.SendOrder(syxh, longorder, executorCode, macAddress, true);

                }
                if (temporder.Rows.Count > 0)
                {
                    temporder = m_sendorder.SendOrder(syxh, temporder, executorCode, macAddress, false);

                }

                //根据返回的结果集返回发送消息
                string f = (longorder.Select("FS_Flag='0'").Length + temporder.Select("FS_Flag='0'").Length).ToString();
                string s = (longorder.Select("FS_Flag='1'").Length + temporder.Select("FS_Flag='1'").Length).ToString();

                //记录同步标志
                DataTable t_longorder = changeTable.Clone();
                DataTable t_temporder = changeTable.Clone();

                foreach (DataRow dr in longorder.Select("FS_Flag='1'"))
                {
                    t_longorder.Rows.Add(dr.ItemArray);
                }

                foreach (DataRow dr in temporder.Select("FS_Flag='1'"))
                {
                    t_temporder.Rows.Add(dr.ItemArray);
                }
 

                //更新同步标志

                UpdateSynchFlagToTrue(t_longorder, false);
                UpdateSynchFlagToTrue(t_temporder, true);


                string mess = "成功发送【" + s + "】条医嘱！" + "\n ";

                if (f != "0")
                {
                    if (s != "0")
                    {
                        mess = mess + "发送失败【" + f + "】条医嘱!如下:";
                    }
                    else
                    {
                        mess = "发送失败【" + f + "】条医嘱!如下:";
                    }

                    foreach (DataRow dr in longorder.Select("FS_Flag='0'"))
                    {
                        mess = mess + "\n " + dr["ypmc"] + "【" + dr["FS_Mess"] + "】!";
                    }
                    foreach (DataRow dr in temporder.Select("FS_Flag='0'"))
                    {
                        mess = mess + "\n " + dr["ypmc"] + "【" + dr["FS_Mess"] + "】!";
                    }
                }
                return mess;

            }
            catch (Exception e)
            {
                return e.Message;
            }



            ////"长期医嘱"  2703
            ////"临时医嘱"  2702
            //DataTable longorder = changedTable.Clone();
            //DataTable temporder = changedTable.Clone();

            //foreach (DataRow dr in changedTable.Rows)
            //{
            //    if (dr["yzbz"].ToString() == "2703")
            //    {
            //        longorder.Rows.Add(dr.ItemArray);
            //    }
            //    else
            //    {
            //        temporder.Rows.Add(dr.ItemArray);
            //    }
            //}


            //string mess = "";
            //if (m_SendOrderHelper == null) m_SendOrderHelper = new SendOrderToHIS();
            //mess = m_SendOrderHelper.SendOrder(syxh, longorder, executorCode, macAddress, false);
            //mess = mess + m_SendOrderHelper.SendOrder(syxh, temporder, executorCode, macAddress, true);

            ////更新同步标志

            //UpdateSynchFlagToTrue(longorder, false);
            //UpdateSynchFlagToTrue(temporder, true);
            //return mess;


        }

        /// <summary>
        /// 发送医嘱到金仕达HIS
        /// </summary>
        /// <param name="changeTable"></param>
        /// <returns></returns>
        public string SendOrderTableToHis(DataTable changeTable)
        {
            try
            {
                SendOrderToHIS m_sendorder = new SendOrderToHIS();

                changeTable = m_sendorder.SendOrder(changeTable);

                //根据返回的结果集返回发送消息
                string f = changeTable.Select("FS_Flag='0'").Length.ToString();
                string s = changeTable.Select("FS_Flag='1'").Length.ToString();
                //更新同步标志
                //"长期医嘱"  2703
                //"临时医嘱"  2702
                DataTable longorder = changeTable.Clone();
                DataTable temporder = changeTable.Clone();

                foreach (DataRow dr in changeTable.Select("FS_Flag='1'"))
                {
                    if (dr["yzbz"].ToString() == "2703")
                    {
                        longorder.Rows.Add(dr.ItemArray);
                    }
                    else
                    {
                        temporder.Rows.Add(dr.ItemArray);
                    }
                }

                //更新同步标志

                UpdateSynchFlagToTrue(longorder, false);
                UpdateSynchFlagToTrue(temporder, true);


                string mess = "成功发送【" + s + "】条医嘱！" + "\n ";

                if (f != "0")
                {
                    if (s != "0")
                    {
                        mess = mess + "发送失败【" + f + "】条医嘱!如下:";
                    }
                    else
                    {
                        mess = "发送失败【" + f + "】条医嘱!如下:";
                    }

                    foreach (DataRow dr in changeTable.Select("FS_Flag='0'"))
                    {
                        mess = mess + "\n " + dr["ypmc"] + "【" + dr["FS_Mess"] + "】!";
                    }
                }
                return mess;

            }
            catch (Exception e)
            {
                return e.Message;
            }


        }

        /// <summary>
        /// 根据传入数据表 批量更新医嘱中是否发送成功
        /// </summary>
        /// <param name="changeTable">需要批量更新的医嘱表</param>
        /// <param name="iscqls">长期与临时标志，长期医嘱为：false  临时医嘱为：true</param>
        private void UpdateSynchFlagToTrue(DataTable changeTable, bool iscqls)
        {
            try
            {
                if (iscqls)
                {
                    foreach (DataRow dr in changeTable.Rows)
                    {
                        string sql = @"update dbo.CP_TempOrder set Tbbz = 1 where Lsyzxh = '{0}'";

                        string strCmd = string.Format(sql, dr["yzxh"].ToString());
                        SqlHelper.ExecuteNoneQuery(strCmd);
                    }
                }
                else
                {
                    foreach (DataRow dr in changeTable.Rows)
                    {
                        string sql = @"update dbo.CP_LongOrder set Tbbz = 1 where Cqyzxh = '{0}'";

                        string strCmd = string.Format(sql, dr["yzxh"].ToString());
                        SqlHelper.ExecuteNoneQuery(strCmd);
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
                ;
            }
        }



        /// <summary>
        /// 保存新增医嘱,modify by xjt
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="listOrderAdd">新增医嘱</param>
        /// <param name="currentList">病患实体</param>
        /// <param name="strActivityID">活动ID</param>
        /// <param name="strActivityChildID">活动子结点ID</param>  
        /// <param name="strLjdm">结点所属路径</param>
        private void SaveAddOrder(List<CP_DoctorOrder> listOrderAdd,
             CP_InpatinetList currentList, string strActivityID, string strActivityChildID, string strLjdm)
        {
            Dictionary<decimal, decimal> dicTempOrderFzxh = new Dictionary<decimal, decimal>();
            Dictionary<decimal, decimal> dicLontOrderFzxh = new Dictionary<decimal, decimal>();
            foreach (CP_DoctorOrder order in listOrderAdd)
            {
                if ((OrderType)(order.Yzbz) == OrderType.Long)
                {
                    if (order.Fzxh != 0)
                    {
                        if (!dicLontOrderFzxh.ContainsKey(order.Fzxh))
                        {
                            decimal yzxh = SaveLongOrder(order, currentList, strActivityID, strActivityChildID, strLjdm);
                            UpdateLongFzxh(yzxh, yzxh);
                            dicLontOrderFzxh.Add(order.Fzxh, yzxh);
                        }
                        else
                        {
                            order.Fzxh = dicLontOrderFzxh[order.Fzxh];
                            SaveLongOrder(order, currentList, strActivityID, strActivityChildID, strLjdm);
                        }
                    }
                    else
                    {
                        decimal yzxh = SaveLongOrder(order, currentList, strActivityID, strActivityChildID, strLjdm);
                        UpdateLongFzxh(yzxh, yzxh);
                    }
                }
                else if ((OrderType)(order.Yzbz) == OrderType.Temp)
                {
                    if (order.Fzxh != 0)
                    {
                        if (!dicTempOrderFzxh.ContainsKey(order.Fzxh))
                        {
                            decimal yzxh = SaveTempOrder(order, currentList, strActivityID, strActivityChildID, strLjdm);
                            UpdateTempFzxh(yzxh, yzxh);
                            dicTempOrderFzxh.Add(order.Fzxh, yzxh);
                        }
                        else
                        {
                            order.Fzxh = dicTempOrderFzxh[order.Fzxh];
                            SaveTempOrder(order, currentList, strActivityID, strActivityChildID, strLjdm);
                        }
                    }
                    else
                    {
                        decimal yzxh = SaveTempOrder(order, currentList, strActivityID, strActivityChildID, strLjdm);
                        UpdateTempFzxh(yzxh, yzxh);
                    }
                }
            }
        }

        /// <summary>
        /// 保存修改（UPDATE),modify by xjt
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="listOrderModify">修改医嘱</param>
        /// <param name="currentList">病患实体</param> 
        private void SaveModifyOrder(List<CP_DoctorOrder> listOrderModify,
           CP_InpatinetList currentList)
        {
            foreach (CP_DoctorOrder order in listOrderModify)
            {
                if ((OrderType)(order.Yzbz) == OrderType.Long)
                {
                    UpdateLongOrder(order, currentList);
                }
                else if ((OrderType)(order.Yzbz) == OrderType.Temp)
                {
                    UpdateTempOrder(order, currentList);
                }
            }
        }

        /// <summary>
        ///  保存长期医嘱,modify by xjt
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="orderAdd"></param>     
        /// <param name="currentList">病患实体</param>
        /// <param name="strActivityID">活动ID</param>
        /// <param name="strActivityChildID">活动子结点ID</param>  
        /// <param name="strLjdm">结点所属路径</param>
        /// <returns></returns>
        private decimal SaveLongOrder(CP_DoctorOrder orderAdd,
            CP_InpatinetList currentList, string strActivityID, string strActivityChildID, string strLjdm)
        {
            try
            {
                //SqlCommand myCommand = new SqlCommand("usp_CP_InsertLongOrder", myConnection, sqlTrans);
                //myCommand.CommandType = CommandType.StoredProcedure;
                //添加输入查询参数、赋予值                       

                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@Syxh",orderAdd.Syxh),
                    new SqlParameter("@Fzxh",orderAdd.Fzxh),
                    new SqlParameter("@Fzbz",orderAdd.Fzbz),
                    new SqlParameter("@Bqdm",orderAdd.Bqdm),
                    new SqlParameter("@Ksdm",orderAdd.Ksdm),
                    new SqlParameter("@Lrysdm",orderAdd.Lrysdm),
                    new SqlParameter("@Lrrq",DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd")),
                    new SqlParameter("@Cdxh",orderAdd.Cdxh),
                    new SqlParameter("@Ggxh",orderAdd.Ggxh),
                    new SqlParameter("@Lcxh",orderAdd.Lcxh),
                    new SqlParameter("@Ypdm",orderAdd.Ypdm),
                    new SqlParameter("@Ypmc",orderAdd.Ypmc),
                    new SqlParameter("@Xmlb",orderAdd.Xmlb),
                    new SqlParameter("@Zxdw",orderAdd.Zxdw == null ? string.Empty : orderAdd.Zxdw),
                    new SqlParameter("@Ypjl",orderAdd.Ypjl),
                    new SqlParameter("@Jldw",orderAdd.Jldw == null ? string.Empty : orderAdd.Jldw),
                    new SqlParameter("@Dwxs",orderAdd.Dwxs),
                    new SqlParameter("@Dwlb",orderAdd.Dwlb),
                    new SqlParameter("@Yfdm",orderAdd.Yfdm == null ? string.Empty : orderAdd.Yfdm),
                    new SqlParameter("@Pcdm",orderAdd.Pcdm == null ? string.Empty : orderAdd.Pcdm),
                    new SqlParameter("@Zxcs",orderAdd.Zxcs),
                    new SqlParameter("@Zxzq",orderAdd.Zxzq),
                    new SqlParameter("@Zxzqdw",orderAdd.Zxzqdw),
                    new SqlParameter("@Zdm",orderAdd.Zdm == null ? string.Empty : orderAdd.Zdm),
                    new SqlParameter("@Zxsj",orderAdd.Zxsj == null ? string.Empty : orderAdd.Zxsj),
                    new SqlParameter("@Ztnr",orderAdd.Ztnr == null ? string.Empty : orderAdd.Ztnr),
                    new SqlParameter("@Yzlb",orderAdd.Yzlb),
                    new SqlParameter("@Yzzt",orderAdd.Yzzt),
                    new SqlParameter("@Tsbj",orderAdd.Tsbj),
                    new SqlParameter("@Yznr",orderAdd.Yznr == null ? string.Empty : orderAdd.Yznr),
                    new SqlParameter("@Tbbz",orderAdd.Tbbz),
                    new SqlParameter("@Memo",orderAdd.Memo == null ? string.Empty : orderAdd.Memo),
                    new SqlParameter("@Ksrq",orderAdd.Ksrq),
                    new SqlParameter("@Ypgg",orderAdd.Ypgg),
                    new SqlParameter("@Zxksdm",orderAdd.Zxksdm== null ? string.Empty : orderAdd.Zxksdm),
                    new SqlParameter("@Isjj",orderAdd.Jjlx),
                    new SqlParameter("@Yzkx",orderAdd.Yzkx),
                    new SqlParameter("@OrderValue",orderAdd.OrderValue),
                    new SqlParameter("@Ctmxxh",orderAdd.Ctmxxh),
                    new SqlParameter("@Ljdm",strLjdm),
                    new SqlParameter("@ActivityID",strActivityID),
                    new SqlParameter("@ActivityChildID",strActivityChildID),
                    new SqlParameter("@Ljxh",currentList.Ljxh)
                };

                //myCommand.Parameters.Add("Syxh", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Fzxh", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Fzbz", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Bqdm", SqlDbType.VarChar, 12);
                //myCommand.Parameters.Add("Ksdm", SqlDbType.VarChar, 12);
                //myCommand.Parameters.Add("Lrysdm", SqlDbType.VarChar, 6);
                //myCommand.Parameters.Add("Lrrq", SqlDbType.VarChar, 19);
                //myCommand.Parameters.Add("Cdxh", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Ggxh", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Lcxh", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Ypdm", SqlDbType.VarChar, 12);
                //myCommand.Parameters.Add("Ypmc", SqlDbType.VarChar, 64);
                //myCommand.Parameters.Add("Xmlb", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Zxdw", SqlDbType.VarChar, 8);
                //myCommand.Parameters.Add("Ypjl", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Jldw", SqlDbType.VarChar, 8);
                //myCommand.Parameters.Add("Dwxs", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Dwlb", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Yfdm", SqlDbType.VarChar, 2);
                //myCommand.Parameters.Add("Pcdm", SqlDbType.VarChar, 2);
                //myCommand.Parameters.Add("Zxcs", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Zxzq", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Zxzqdw", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Zdm", SqlDbType.VarChar, 7);
                //myCommand.Parameters.Add("Zxsj", SqlDbType.VarChar, 64);
                //myCommand.Parameters.Add("Ztnr", SqlDbType.VarChar, 64);
                //myCommand.Parameters.Add("Yzlb", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Yzzt", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Tsbj", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Yznr", SqlDbType.VarChar, 255);
                //myCommand.Parameters.Add("Tbbz", SqlDbType.Decimal);
                //myCommand.Parameters.Add("Memo", SqlDbType.VarChar, 64);
                //myCommand.Parameters.Add("Ksrq", SqlDbType.VarChar, 19);
                //myCommand.Parameters.Add("Ypgg", SqlDbType.VarChar, 32);
                //myCommand.Parameters.Add("Ctmxxh", SqlDbType.Decimal); //to do 赋值
                //myCommand.Parameters.Add("Ljdm", SqlDbType.VarChar, 12);
                //myCommand.Parameters.Add("ActivityID", SqlDbType.VarChar, 50);
                //myCommand.Parameters.Add("ActivityChildID", SqlDbType.VarChar, 50);
                //myCommand.Parameters.Add("Ljxh", SqlDbType.Decimal);

                //myCommand.Parameters["Syxh"].Value = orderAdd.Syxh;
                //myCommand.Parameters["Fzxh"].Value = orderAdd.Fzxh;
                //myCommand.Parameters["Fzbz"].Value = orderAdd.Fzbz;
                //myCommand.Parameters["Bqdm"].Value = orderAdd.Bqdm;
                //myCommand.Parameters["Ksdm"].Value = orderAdd.Ksdm;
                //myCommand.Parameters["Lrysdm"].Value = orderAdd.Lrysdm;
                //myCommand.Parameters["Lrrq"].Value = DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd");
                //myCommand.Parameters["Cdxh"].Value = orderAdd.Cdxh;
                //myCommand.Parameters["Ggxh"].Value = orderAdd.Ggxh;
                //myCommand.Parameters["Lcxh"].Value = orderAdd.Lcxh;
                //myCommand.Parameters["Ypdm"].Value = orderAdd.Ypdm;
                //myCommand.Parameters["Ypmc"].Value = orderAdd.Ypmc;
                //myCommand.Parameters["Xmlb"].Value = orderAdd.Xmlb;
                //myCommand.Parameters["Zxdw"].Value = orderAdd.Zxdw == null ? string.Empty : orderAdd.Zxdw;
                //myCommand.Parameters["Ypjl"].Value = orderAdd.Ypjl;
                //myCommand.Parameters["Jldw"].Value = orderAdd.Jldw == null ? string.Empty : orderAdd.Jldw;
                //myCommand.Parameters["Dwxs"].Value = orderAdd.Dwxs;
                //myCommand.Parameters["Dwlb"].Value = orderAdd.Dwlb;
                //myCommand.Parameters["Yfdm"].Value = orderAdd.Yfdm == null ? string.Empty : orderAdd.Yfdm;
                //myCommand.Parameters["Pcdm"].Value = orderAdd.Pcdm == null ? string.Empty : orderAdd.Pcdm;
                //myCommand.Parameters["Zxcs"].Value = orderAdd.Zxcs;
                //myCommand.Parameters["Zxzq"].Value = orderAdd.Zxzq;
                //myCommand.Parameters["Zxzqdw"].Value = orderAdd.Zxzqdw;
                //myCommand.Parameters["Zdm"].Value = orderAdd.Zdm == null ? string.Empty : orderAdd.Zdm;
                //myCommand.Parameters["Zxsj"].Value = orderAdd.Zxsj == null ? string.Empty : orderAdd.Zxsj;
                //myCommand.Parameters["Ztnr"].Value = orderAdd.Ztnr == null ? string.Empty : orderAdd.Ztnr;
                //myCommand.Parameters["Yzlb"].Value = orderAdd.Yzlb;
                //myCommand.Parameters["Yzzt"].Value = orderAdd.Yzzt;
                //myCommand.Parameters["Tsbj"].Value = orderAdd.Tsbj;
                //myCommand.Parameters["Yznr"].Value = orderAdd.Yznr == null ? string.Empty : orderAdd.Yznr;
                //myCommand.Parameters["Tbbz"].Value = orderAdd.Tbbz;
                //myCommand.Parameters["Memo"].Value = orderAdd.Memo == null ? string.Empty : orderAdd.Memo;
                //myCommand.Parameters["Ksrq"].Value = orderAdd.Ksrq;
                //myCommand.Parameters["Ypgg"].Value = orderAdd.Ypgg;
                //myCommand.Parameters["Ctmxxh"].Value = orderAdd.Ctmxxh;//to do
                //myCommand.Parameters["Ljdm"].Value = strLjdm;
                //myCommand.Parameters["ActivityID"].Value = strActivityID;
                //myCommand.Parameters["ActivityChildID"].Value = strActivityChildID;
                //myCommand.Parameters["Ljxh"].Value = currentList.Ljxh;
                //return (decimal)myCommand.ExecuteScalar();

                // return (decimal)SqlHelper.ExecuteScalar(sqlTrans, CommandType.StoredProcedure, "usp_CP_InsertLongOrder", parameters);

                Decimal decimalReturn = -1;
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_InsertLongOrder", parameters, CommandType.StoredProcedure);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != null)
                {
                    decimalReturn = ConvertMy.ToDecimal(dt.Rows[0][0].ToString().Trim());
                }
                return decimalReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }           // zm 8.25 Oracle

        /// <summary>
        /// 保存临时医嘱,modify by xjt
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="orderAdd"></param>     
        /// <param name="currentList">病患实体</param>
        /// <param name="strActivityID">活动ID</param>
        /// <param name="strActivityChildID">活动子结点ID</param>  
        /// <param name="strLjdm">结点所属路径</param>
        /// <returns></returns>
        private decimal SaveTempOrder(CP_DoctorOrder orderAdd,
            CP_InpatinetList currentList, string strActivityID, string strActivityChildID, string strLjdm)
        {
            try
            {
                //SqlCommand myCommand = new SqlCommand("usp_CP_InsertTempOrder", myConnection, sqlTrans);
                //myCommand.CommandType = CommandType.StoredProcedure;
                //添加输入查询参数、赋予值                

                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@Syxh",orderAdd.Syxh),
                    new SqlParameter("@Fzxh",orderAdd.Fzxh),
                    new SqlParameter("@Fzbz",orderAdd.Fzbz),
                    new SqlParameter("@Bqdm",orderAdd.Bqdm),
                    new SqlParameter("@Ksdm",orderAdd.Ksdm),
                    new SqlParameter("@Lrysdm",orderAdd.Lrysdm),
                    new SqlParameter("@Lrrq",DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd")),
                    new SqlParameter("@Cdxh",orderAdd.Cdxh),
                    new SqlParameter("@Ggxh",orderAdd.Ggxh),
                    new SqlParameter("@Lcxh",orderAdd.Lcxh),
                    new SqlParameter("@Ypdm",orderAdd.Ypdm),
                    new SqlParameter("@Ypmc",orderAdd.Ypmc),
                    new SqlParameter("@Xmlb",orderAdd.Xmlb),
                    new SqlParameter("@Zxdw",orderAdd.Zxdw == null ? string.Empty : orderAdd.Zxdw),
                    new SqlParameter("@Ypjl",orderAdd.Ypjl),
                    new SqlParameter("@Jldw",orderAdd.Jldw == null ? string.Empty : orderAdd.Jldw),
                    new SqlParameter("@Dwxs",orderAdd.Dwxs),
                    new SqlParameter("@Dwlb",orderAdd.Dwlb),
                    new SqlParameter("@Yfdm",orderAdd.Yfdm == null ? string.Empty : orderAdd.Yfdm),
                    new SqlParameter("@Pcdm",orderAdd.Pcdm == null ? string.Empty : orderAdd.Pcdm),
                    new SqlParameter("@Zxcs",orderAdd.Zxcs),
                    new SqlParameter("@Zxzq",orderAdd.Zxzq),
                    new SqlParameter("@Zxzqdw",orderAdd.Zxzqdw),
                    new SqlParameter("@Zdm",orderAdd.Zdm == null ? string.Empty : orderAdd.Zdm),
                    new SqlParameter("@Zxsj",orderAdd.Zxsj == null ? string.Empty : orderAdd.Zxsj),
                    new SqlParameter("@Ztnr",orderAdd.Ztnr == null ? string.Empty : orderAdd.Ztnr),
                    new SqlParameter("@Yzlb",orderAdd.Yzlb),
                    new SqlParameter("@Yzzt",orderAdd.Yzzt),
                    new SqlParameter("@Tsbj",orderAdd.Tsbj),
                    new SqlParameter("@Yznr",orderAdd.Yznr == null ? string.Empty : orderAdd.Yznr),
                    new SqlParameter("@Tbbz",orderAdd.Tbbz),
                    new SqlParameter("@Memo",orderAdd.Memo == null ? string.Empty : orderAdd.Memo),
                    new SqlParameter("@Ksrq",orderAdd.Ksrq),
                    new SqlParameter("@Ypgg",orderAdd.Ypgg),
                    new SqlParameter("@Zxksdm",orderAdd.Zxksdm== null ? string.Empty : orderAdd.Zxksdm),
                    new SqlParameter("@Isjj",orderAdd.Jjlx),
                    new SqlParameter("@Yzkx",orderAdd.Yzkx),
                    new SqlParameter("@OrderValue",orderAdd.OrderValue),
                    new SqlParameter("@Ctmxxh",orderAdd.Ctmxxh),
                    new SqlParameter("@Ljdm",strLjdm),
                    new SqlParameter("@ActivityID",strActivityID),
                    new SqlParameter("@ActivityChildID",strActivityChildID),
                    new SqlParameter("@Ljxh",currentList.Ljxh)
                };

                Decimal decimalReturn = -1;
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_InsertTempOrder", parameters, CommandType.StoredProcedure);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != null)
                {
                    decimalReturn = ConvertMy.ToDecimal(dt.Rows[0][0].ToString().Trim());
                }
                return decimalReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }       // zm 8.25 Oracle




        /// <summary>
        /// 更新长期医嘱,modify by xjt
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="orderAdd"></param>
        /// <param name="currentList"></param>
        private void UpdateLongOrder(CP_DoctorOrder orderModify,
            CP_InpatinetList currentList)
        {
            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@Syxh",orderModify.Syxh),
                    new SqlParameter("@Fzxh",orderModify.Fzxh),
                    new SqlParameter("@Fzbz",orderModify.Fzbz),
                    new SqlParameter("@Bqdm",orderModify.Bqdm),
                    new SqlParameter("@Ksdm",orderModify.Ksdm),
                    new SqlParameter("@Lrysdm",orderModify.Lrysdm),
                    new SqlParameter("@Lrrq",DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd")),
                    new SqlParameter("@Cdxh",orderModify.Cdxh),
                    new SqlParameter("@Ggxh",orderModify.Ggxh),
                    new SqlParameter("@Lcxh",orderModify.Lcxh),
                    new SqlParameter("@Ypdm",orderModify.Ypdm),
                    new SqlParameter("@Ypmc",orderModify.Ypmc),
                    new SqlParameter("@Xmlb",orderModify.Xmlb),
                    new SqlParameter("@Zxdw",orderModify.Zxdw == null ? string.Empty : orderModify.Zxdw),
                    new SqlParameter("@Ypjl",orderModify.Ypjl),
                    new SqlParameter("@Jldw",orderModify.Jldw == null ? string.Empty : orderModify.Jldw),
                    new SqlParameter("@Dwxs",orderModify.Dwxs),
                    new SqlParameter("@Dwlb",orderModify.Dwlb),
                    new SqlParameter("@Yfdm",orderModify.Yfdm == null ? string.Empty : orderModify.Yfdm),
                    new SqlParameter("@Pcdm",orderModify.Pcdm == null ? string.Empty : orderModify.Pcdm),
                    new SqlParameter("@Zxcs",orderModify.Zxcs),
                    new SqlParameter("@Zxzq",orderModify.Zxzq),
                    new SqlParameter("@Zxzqdw",orderModify.Zxzqdw),
                    new SqlParameter("@Zdm",orderModify.Zdm == null ? string.Empty : orderModify.Zdm),
                    new SqlParameter("@Zxsj",orderModify.Zxsj == null ? string.Empty : orderModify.Zxsj),
                    new SqlParameter("@Ztnr",orderModify.Ztnr == null ? string.Empty : orderModify.Ztnr),
                    new SqlParameter("@Yzlb",orderModify.Yzlb),
                    new SqlParameter("@Yzzt",orderModify.Yzzt),
                    new SqlParameter("@Tsbj",orderModify.Tsbj),
                    new SqlParameter("@Yznr",orderModify.Yznr == null ? string.Empty : orderModify.Yznr),
                    new SqlParameter("@Tbbz",orderModify.Tbbz),
                    new SqlParameter("@Memo",orderModify.Memo == null ? string.Empty : orderModify.Memo),
                    new SqlParameter("@Ksrq",orderModify.Ksrq),
                    new SqlParameter("@Ypgg",orderModify.Ypgg),
                    new SqlParameter("@Zxksdm",orderModify.Zxksdm== null ? string.Empty : orderModify.Zxksdm),
                    new SqlParameter("@Isjj",orderModify.Jjlx),
                    new SqlParameter("@Yzkx",orderModify.Yzkx),
                    new SqlParameter("@OrderValue",orderModify.OrderValue),
                    new SqlParameter("@Yzxh",orderModify.Yzxh),
                    new SqlParameter("@Ctmxxh",orderModify.Ctmxxh)
                };


                SqlHelper.ExecuteNoneQuery("usp_CP_UpdateLongOrder", parameters, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }           // zm 8.25 Oracle

        /// <summary>
        /// 更新临时医嘱,modify by xjt
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="orderAdd"></param>
        /// <param name="currentList"></param>
        /// <param name="strPathDetailID"></param> 
        private void UpdateTempOrder(CP_DoctorOrder orderModify,
            CP_InpatinetList currentList)
        {
            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@Syxh",orderModify.Syxh),
                    new SqlParameter("@Fzxh",orderModify.Fzxh),
                    new SqlParameter("@Fzbz",orderModify.Fzbz),
                    new SqlParameter("@Bqdm",orderModify.Bqdm),
                    new SqlParameter("@Ksdm",orderModify.Ksdm),
                    new SqlParameter("@Lrysdm",orderModify.Lrysdm),
                    new SqlParameter("@Lrrq",DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd")),
                    new SqlParameter("@Cdxh",orderModify.Cdxh),
                    new SqlParameter("@Ggxh",orderModify.Ggxh),
                    new SqlParameter("@Lcxh",orderModify.Lcxh),
                    new SqlParameter("@Ypdm",orderModify.Ypdm),
                    new SqlParameter("@Ypmc",orderModify.Ypmc),
                    new SqlParameter("@Xmlb",orderModify.Xmlb),
                    new SqlParameter("@Zxdw",orderModify.Zxdw == null ? string.Empty : orderModify.Zxdw),
                    new SqlParameter("@Ypjl",orderModify.Ypjl),
                    new SqlParameter("@Jldw",orderModify.Jldw == null ? string.Empty : orderModify.Jldw),
                    new SqlParameter("@Dwxs",orderModify.Dwxs),
                    new SqlParameter("@Dwlb",orderModify.Dwlb),
                    new SqlParameter("@Yfdm",orderModify.Yfdm == null ? string.Empty : orderModify.Yfdm),
                    new SqlParameter("@Pcdm",orderModify.Pcdm == null ? string.Empty : orderModify.Pcdm),
                    new SqlParameter("@Zxcs",orderModify.Zxcs),
                    new SqlParameter("@Zxzq",orderModify.Zxzq),
                    new SqlParameter("@Zxzqdw",orderModify.Zxzqdw),
                    new SqlParameter("@Zdm",orderModify.Zdm == null ? string.Empty : orderModify.Zdm),
                    new SqlParameter("@Zxsj",orderModify.Zxsj == null ? string.Empty : orderModify.Zxsj),
                    new SqlParameter("@Ztnr",orderModify.Ztnr == null ? string.Empty : orderModify.Ztnr),
                    new SqlParameter("@Yzlb",orderModify.Yzlb),
                    new SqlParameter("@Yzzt",orderModify.Yzzt),
                    new SqlParameter("@Tsbj",orderModify.Tsbj),
                    new SqlParameter("@Yznr",orderModify.Yznr == null ? string.Empty : orderModify.Yznr),
                    new SqlParameter("@Tbbz",orderModify.Tbbz),
                    new SqlParameter("@Memo",orderModify.Memo == null ? string.Empty : orderModify.Memo),
                    new SqlParameter("@Ksrq",orderModify.Ksrq),
                    new SqlParameter("@Ypgg",orderModify.Ypgg),
                    new SqlParameter("@Zxksdm",orderModify.Zxksdm== null ? string.Empty : orderModify.Zxksdm),
                    new SqlParameter("@Isjj",orderModify.Jjlx),
                    new SqlParameter("@Yzkx",orderModify.Yzkx),
                    new SqlParameter("@OrderValue",orderModify.OrderValue),
                    new SqlParameter("@Yzxh",orderModify.Yzxh),
                    new SqlParameter("@Ctmxxh",orderModify.Ctmxxh),
                };

                SqlHelper.ExecuteNoneQuery("usp_CP_UpdateTempOrder", parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }               // zm 8.25 Oracle




        /// <summary>
        /// 更新长期医嘱分组序号
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="cqyzxh"></param>
        /// <param name="fzxh"></param>
        private void UpdateLongFzxh(decimal cqyzxh, decimal fzxh)
        {
            string strSql = @"update CP_LongOrder set Fzxh  = {0} where Cqyzxh = {1}";
            string strCmd = string.Format(strSql, fzxh, cqyzxh);

            SqlHelper.ExecuteNoneQuery(strCmd);
        }       // zm 8.25 Oracle

        /// <summary>
        /// 更新临时医嘱分组序号
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="lsyzxh"></param>
        /// <param name="fzxh"></param>
        private void UpdateTempFzxh(decimal lsyzxh, decimal fzxh)
        {
            string strSql = @"update CP_TempOrder set Fzxh  = {0} where Lsyzxh = {1}";
            string strCmd = string.Format(strSql, fzxh, lsyzxh);

            SqlHelper.ExecuteNoneQuery(strCmd);
        }       // zm 8.25 Oracle

        /// <summary>
        /// 删除医嘱
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="orderDel"></param>
        private void DelOrder(CP_DoctorOrder orderDel)
        {
            string strSql = string.Empty;
            if (((OrderType)(orderDel.Yzbz) == OrderType.Long))
            {
                strSql = "UPDATE CP_LongOrder SET Yzzt = 3203 WHERE Cqyzxh = {0}";
            }
            else if ((OrderType)(orderDel.Yzbz) == OrderType.Temp)
            {
                strSql = "UPDATE CP_TempOrder SET Yzzt = 3203 WHERE Lsyzxh = {0}";
            }
            string strCmd = string.Format(strSql, orderDel.Yzxh);

            //SqlCommand cmd = new SqlCommand(strCmd, myConnection, sqlTrans);
            //cmd.ExecuteNoneQuery();

            SqlHelper.ExecuteNoneQuery(strCmd);
        }           // zm 8.25 Oracle

        /// <summary>
        /// 路径必选医嘱，不执行理由
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_PathVariation> GetPathVariationInfo(string ljdm, String strActivityId)
        {
            List<CP_PathVariation> listInfo = new List<CP_PathVariation>();
            //using (SqlConnection conn = new SqlConnection(m_ConnectionString))
            //{
            try
            {
                string strSql = @"SELECT Bydm,Bymc,Byms,Yxjl,0 OrderValue FROM CP_PathVariation cp"
                                + @" WHERE cp.Yxjl = 1 AND EXISTS (SELECT 1 FROM CP_VariationToPath cvp WHERE cvp.Ljdm = '{0}' and ActivityId = '{1}' and cvp.Bydm = cp.Bydm)"
                                + @" UNION"
                                + @" SELECT Bydm,Bymc,Byms,Yxjl,1 OrderValue  FROM CP_PathVariation cp WHERE cp.Yxjl = 1 AND cp.Bydm = '9999'"
                                + @" ORDER BY OrderValue,Bydm";
                string strCmd = string.Format(strSql, ljdm, strActivityId);


                DataTable dataTable = SqlHelper.ExecuteDataTable(strCmd);

                foreach (DataRow rows in dataTable.Rows)
                {
                    CP_PathVariation pathVariation = new CP_PathVariation();
                    pathVariation.Bydm = rows["Bydm"].ToString();
                    pathVariation.Bymc = rows["Bymc"].ToString();
                    pathVariation.Byms = rows["Byms"].ToString();
                    pathVariation.Yxjl = int.Parse(rows["Yxjl"].ToString());
                    listInfo.Add(pathVariation);
                }
            }
            catch (SqlException ex)
            {
                ThrowSqlExpection(ex);
            }
            catch (Exception ex)
            {
                ThrowNormalExpection(ex);
            }

            //}
            return listInfo;
        }        // zm 8.25 Oracle

        /// <summary>
        /// 路径必选医嘱，不执行理由,对应其它后面的COMBBOX
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_PathVariation> GetOtherPathVariationInfo(string ljdm, String strActivityId)
        {
            List<CP_PathVariation> listInfo = new List<CP_PathVariation>();
            //using (SqlConnection conn = new SqlConnection(m_ConnectionString))
            //{
            try
            {
                string strSql = @"SELECT Bydm,Bymc,Byms,Yxjl,Py FROM CP_PathVariation cp"
                                + @" WHERE cp.Yxjl = 1 AND len(Bydm) = 11"
                                + @" AND NOT EXISTS (SELECT 1 FROM CP_VariationToPath cvp WHERE cvp.Ljdm = '{0}' and cvp.ActivityId = '{1}' and cvp.Bydm = cp.Bydm)"
                                + @" ORDER BY Bydm";

                string strCmd = string.Format(strSql, ljdm, strActivityId);


                DataTable dataTable = SqlHelper.ExecuteDataTable(strCmd);

                foreach (DataRow rows in dataTable.Rows)
                {
                    CP_PathVariation pathVariation = new CP_PathVariation();
                    pathVariation.Bydm = rows["Bydm"].ToString();
                    pathVariation.Bymc = rows["Bymc"].ToString();
                    pathVariation.Byms = rows["Byms"].ToString();
                    pathVariation.Yxjl = int.Parse(rows["Yxjl"].ToString());
                    pathVariation.Py = rows["Py"].ToString();
                    listInfo.Add(pathVariation);
                }
            }
            catch (SqlException ex)
            {
                ThrowSqlExpection(ex);
            }
            catch (Exception ex)
            {
                ThrowNormalExpection(ex);
            }

            //}
            return listInfo;
        }       // zm 8.25 Oracle

        /// <summary>
        /// 临床路径变异记录
        /// </summary>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        private void InsertVariantRecords(CP_VariantRecords reason, CP_InpatinetList currentList, string Mxdm, String PathDetailID)
        {
            string strCmd = "INSERT INTO CP_VariantRecords(Syxh ,Ljdm ,Mxdm ,Ypdm ,Bylb ,Bynr ,Bydm,Byyy ,Bysj,Bylx,Ljxh,PahtDetailID,xmlb )"
                            + @" VALUES  ( {0},'{1}' , '{2}','{3}' , '{4}' , '{5}' ,'{6}' , '{7}',getdate(),'{8}',{9} ,'{10}','{11}')";
            string strSql = string.Format(strCmd, int.Parse(currentList.Syxh), currentList.Ljdm, Mxdm, reason.Ypdm,
                                          reason.Bylb, reason.Bynr, reason.Bydm, reason.Byyy, reason.Bylx, currentList.Ljxh, PathDetailID,reason.xmlb);

            SqlHelper.ExecuteNoneQuery(strSql);
        }       // zm 8.25 Oracle

        void InsertCP_InPatientPathEnForceDetail(String Ljxh, String Ljdm, String Syxh, String UniqueID, int i, string type, string typename)
        {


            SqlHelper.ExecuteNoneQuery("usp_CP_InPatientPathExeDetail '" + Syxh + "','" + Ljxh + "','" + Ljdm + "','" + UniqueID + "','" + i + "','" + type + "','" + typename + "' ");
        }

        // zm 8.25 Oracle
        void InsertCP_PathExecuteFlowActivityChildren(String UniqueID, String childID, int i)
        {

            //SqlParameter[] sqlParameter = new SqlParameter[] { 
            //    new SqlParameter("@UniqueID",UniqueID),
            //    new SqlParameter("@childID",childID),
            //     new SqlParameter("@i",i)
            //};

            //procedure usp_CP_PathExeActivityChild(v_UniqueID varchar default '', v_childID varchar default '', v_i varchar default '')as
            SqlHelper.ExecuteNoneQuery("usp_CP_PathExeActivityChild  '" + UniqueID + "','" + childID + "','" + i + "'");
        }       // zm 8.25 Oracle
        #endregion 路径执行医嘱相关

        #region 退出路径
        /// <summary>
        /// 退出路径
        /// </summary>
        /// <param name="strConfigKey"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public void QuitPath(CP_InpatinetList inpatientList, List<CP_VariantRecords> listVariantRecords, String ActivityUniqueID)
        {
            string strConfigValue = string.Empty;

            //SqlHelper.BeginTransaction();
            try
            {
                //myConnection.Open();
                //sqlTrans = myConnection.BeginTransaction();//事务开始

                string strSql = @"UPDATE CP_InPathPatient SET Tcsj = convert(char(20),getdate(),120),Ljzt = 2 WHERE Id = {0}";
                string strCmd = string.Format(strSql, inpatientList.Ljxh);


                SqlHelper.ExecuteNoneQuery(strCmd);

                string strCzyy = string.Empty;
                foreach (CP_VariantRecords reason in listVariantRecords)
                {
                    InsertVariantRecords(reason, inpatientList, reason.Mxdm, ActivityUniqueID);
                    strCzyy += reason.Byyy + ";";
                }

                UpdateExecuteInfo(inpatientList, strCzyy);

                //sqlTrans.Commit();//事务提交   

                // 同步状态
                SychPathStatus2Emr(inpatientList.Hissyxh, 2);
                //SqlHelper.CommitTransaction();
            }
            catch (Exception ex)
            {
                //sqlTrans.Rollback();
                ThrowException(ex);
                //SqlHelper.RollbackTransaction();
            }


        }       // zm 8.25 Oracle

        private void UpdateExecuteInfo(CP_InpatinetList currentList, string strCzyy)
        {
            string strSql = string.Format("UPDATE CP_PathExecuteInfo SET  Ljcz = 1104,Czyy = '{0}',Czsj =  convert(char(20),getdate(),120)  WHERE Ljxh = {1}", strCzyy, currentList.BhljId);

            SqlHelper.ExecuteNoneQuery(strSql);
        }       // zm 8.25 Oracle
        #endregion

        #region COPY 路径相关
        #region  新XML和更新XML

        /// <summary>
        /// 更新COPY路径的XML
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="strLjdm"></param>
        /// <param name="strNewXml"></param>
        private void UpdateNewXmlBy(string strLjdm, string strNewXml)
        {
            string strSql = "UPDATE CP_ClinicalPath SET WorkFlowXML = @Xml WHERE Ljdm = @Ljdm ";

            //添加输入查询参数、赋予值

            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter("@Xml",strNewXml),
                new SqlParameter("@Ljdm",strLjdm)
            };

            SqlHelper.ExecuteNoneQuery(strSql);
        }       // zm 8.25 Oracle
        #endregion

        #region COPY医令
        /// <summary>
        ///   InsertCopyAdviceGroup
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="pathDetailID">新的结点GUID</param>
        /// <param name="pathName">结点名称</param>
        /// <returns></returns>
        private decimal InsertCopyAdviceGroup(SqlTransaction sqlTrans, SqlConnection myConnection, string pathDetailID, string pathName)
        {
            decimal ctyzxh = 0;
            //SqlCommand myCommand = new SqlCommand("usp_CP_InsertCopyAdviceGroup", myConnection, sqlTrans);
            //myCommand.CommandType = CommandType.StoredProcedure;
            //添加输入查询参数、赋予值

            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter("@PahtDetailID",pathDetailID),
                new SqlParameter("@Name",pathName)
            };

            DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_InsertCopyAdviceGroup", parameters, CommandType.StoredProcedure);
            if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != null)
            {
                ctyzxh = ConvertMy.ToDecimal(dt.Rows[0][0].ToString().Trim());
            }

            return ctyzxh;
        }       // zm 8.25 Oracle

        /// <summary>
        ///  InsertCopyAdviceGroupDetail
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="oldPathDetailID">OLD活动结点GUID</param>
        /// <param name="newPathDetailID">NEW活动结点GUID</param>
        /// <param name="newCtyzxh">新的成套医嘱序号</param>
        private void InsertCopyAdviceGroupDetail(string oldPathDetailID, string newPathDetailID, decimal newCtyzxh)
        {


            SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter("@OldPahtDetailID",oldPathDetailID),
                new SqlParameter("@NewPahtDetailID",newPathDetailID),
                new SqlParameter("@NewCtyzxh",newCtyzxh)
            };
            SqlHelper.ExecuteNoneQuery("usp_CP_InsertCopyAdviceGroupDetail", parameters, CommandType.StoredProcedure);
        }       // zm 8.25 Oracle


        #endregion


        #endregion

        #region

        /// <summary>
        /// 更新路径状态
        /// </summary>
        /// <param name="strLjdm">路径代码</param>
        /// <param name="yxjl">有效记录</param> 
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public void UpdatePathStaus(string strLjdm, int yxjl, string strTime, string strShys)
        {

            try
            {
                string strSql = "UPDATE CP_ClinicalPath SET Yxjl = @Yxjl,Shsj = @Shsj,Shys = @Shys WHERE Ljdm = @Ljdm";

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@Yxjl",yxjl),
                        new SqlParameter("@Ljdm",strLjdm),
                        new SqlParameter("@Shsj",strTime),
                        new SqlParameter("@Shys",strShys)
                    };

                SqlHelper.ExecuteNoneQuery(strSql, parameters, CommandType.Text);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            //}
        }                   // zm 8.25 Oracle
        #endregion

        #region 结点配置，成套医嘱相关
        /// <summary>
        /// 获取RIS LIS 项目信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(CP_LogException))]
        private List<CP_ChargingMinItem> GetRISLISOrderInfo()
        {
            List<CP_ChargingMinItem> listInfo = new List<CP_ChargingMinItem>();
            try
            {

                string strSql = "SELECT  Sfxmdm ,Name ,upper(Py) py ,upper(Wb) wb ,Dxdm ,Xmdw ,Xmgg ,"
                         + " Xmdj ,Mzbxbz ,Zybxbz ,Xmlb ,Xtbz ,Xmxz ,"
                         + " Xskz ,Fjxx ,Syfw , Yzglbz ,Yxjl ,Memo"
                         + " FROM    CP_ChargingMinItem "
                         + " WHERE   Xmxz = 0 AND Xmlb IN ( 2411, 2412 ) ORDER BY Sfxmdm";

                DataTable dataTable = SqlHelper.ExecuteDataTable(strSql);

                foreach (DataRow dr in dataTable.Rows)
                {
                    CP_ChargingMinItem cp = new CP_ChargingMinItem();
                    cp.Sfxmdm = dr["Sfxmdm"].ToString();
                    cp.Name = dr["Name"].ToString();
                    cp.Py = dr["Py"].ToString();
                    cp.Wb = dr["Wb"].ToString();
                    cp.Dxdm = dr["Dxdm"].ToString();
                    cp.Xmdw = dr["Xmdw"].ToString();
                    cp.Xmgg = dr["Xmgg"].ToString();
                    cp.Xmdj = decimal.Parse(string.IsNullOrEmpty(dr["Xmdj"].ToString()) ? "0" : dr["Xmdj"].ToString());
                    cp.Mzbxbz = Int16.Parse(string.IsNullOrEmpty(dr["Mzbxbz"].ToString()) ? "0" : dr["Mzbxbz"].ToString());
                    cp.Zybxbz = Int16.Parse(string.IsNullOrEmpty(dr["Zybxbz"].ToString()) ? "0" : dr["Zybxbz"].ToString());
                    cp.Xmlb = Int16.Parse(string.IsNullOrEmpty(dr["Xmlb"].ToString()) ? "0" : dr["Xmlb"].ToString());
                    cp.Xtbz = Int16.Parse(string.IsNullOrEmpty(dr["Xtbz"].ToString()) ? "0" : dr["Xtbz"].ToString());
                    cp.Xmxz = Int16.Parse(string.IsNullOrEmpty(dr["Xmxz"].ToString()) ? "0" : dr["Xmxz"].ToString());
                    cp.Xskz = Int16.Parse(string.IsNullOrEmpty(dr["Xskz"].ToString()) ? "0" : dr["Xskz"].ToString());
                    cp.Fjxx = dr["Fjxx"].ToString();
                    cp.Syfw = Int16.Parse(string.IsNullOrEmpty(dr["Syfw"].ToString()) ? "0" : dr["Syfw"].ToString());
                    cp.Yzglbz = Int16.Parse(string.IsNullOrEmpty(dr["Yzglbz"].ToString()) ? "0" : dr["Yzglbz"].ToString());
                    cp.Yxjl = Int16.Parse(string.IsNullOrEmpty(dr["Yxjl"].ToString()) ? "0" : dr["Yxjl"].ToString());
                    cp.Memo = dr["Memo"].ToString();
                    listInfo.Add(cp);
                }
                // }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return listInfo;
        }            // zm 8.25 Oracle

        /// <summary>
        /// 常规医嘱
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(CP_LogException))]
        private List<CP_ChargingMinItem> GetNormalOrderInfo(int orderType)
        {
            List<CP_ChargingMinItem> listInfo = new List<CP_ChargingMinItem>();
            try
            {
                string sqlStr = string.Empty;
                DataTable dataTable;
                if (CheckSelectHISView())
                {
                    //从HIS视图中获取数据
                    sqlStr = "select * from PathWay_ChargingMinItem";

                    dataTable = HISHelper.ExecuteDataTable(sqlStr);
                }
                else
                {
                    string strSql = "SELECT  Sfxmdm ,Name ,upper(Py) py ,upper(Wb) wb ,Dxdm ,Xmdw ,Xmgg ,"
                             + " Xmdj ,Mzbxbz ,Zybxbz ,Xmlb ,Xtbz ,Xmxz ,"
                             + " Xskz ,Fjxx ,Syfw , Yzglbz ,Yxjl ,Memo"
                             + " FROM    CP_ChargingMinItem "
                             + " WHERE   Xmxz = 1 AND Xmlb = @OrderType ORDER BY dbo.Get_StrArrayStrOfIndex(Fjxx,'|',1),convert(int,dbo.Get_StrArrayStrOfIndex(Fjxx,'|',2))";


                    SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@OrderType",orderType)
                    };


                    dataTable = SqlHelper.ExecuteDataTable(strSql, parameters, CommandType.Text);

                }
                foreach (DataRow dr in dataTable.Rows)
                {
                    CP_ChargingMinItem cp = new CP_ChargingMinItem();
                    cp.Sfxmdm = dr["Sfxmdm"].ToString();
                    cp.Name = dr["Name"].ToString();
                    cp.Py = dr["Py"].ToString();
                    cp.Wb = dr["Wb"].ToString();
                    cp.Dxdm = dr["Dxdm"].ToString();
                    cp.Xmdw = dr["Xmdw"].ToString();
                    cp.Xmgg = dr["Xmgg"].ToString();
                    cp.Xmdj = decimal.Parse(string.IsNullOrEmpty(dr["Xmdj"].ToString()) ? "0" : dr["Xmdj"].ToString());
                    cp.Mzbxbz = Int16.Parse(string.IsNullOrEmpty(dr["Mzbxbz"].ToString()) ? "0" : dr["Mzbxbz"].ToString());
                    cp.Zybxbz = Int16.Parse(string.IsNullOrEmpty(dr["Zybxbz"].ToString()) ? "0" : dr["Zybxbz"].ToString());
                    cp.Xmlb = Int16.Parse(string.IsNullOrEmpty(dr["Xmlb"].ToString()) ? "0" : dr["Xmlb"].ToString());
                    cp.Xtbz = Int16.Parse(string.IsNullOrEmpty(dr["Xtbz"].ToString()) ? "0" : dr["Xtbz"].ToString());
                    cp.Xmxz = Int16.Parse(string.IsNullOrEmpty(dr["Xmxz"].ToString()) ? "0" : dr["Xmxz"].ToString());
                    cp.Xskz = Int16.Parse(string.IsNullOrEmpty(dr["Xskz"].ToString()) ? "0" : dr["Xskz"].ToString());
                    cp.Fjxx = dr["Fjxx"].ToString();
                    cp.Syfw = Int16.Parse(string.IsNullOrEmpty(dr["Syfw"].ToString()) ? "0" : dr["Syfw"].ToString());
                    cp.Yzglbz = Int16.Parse(string.IsNullOrEmpty(dr["Yzglbz"].ToString()) ? "0" : dr["Yzglbz"].ToString());
                    cp.Yxjl = Int16.Parse(string.IsNullOrEmpty(dr["Yxjl"].ToString()) ? "0" : dr["Yxjl"].ToString());
                    cp.Memo = dr["Memo"].ToString();
                    listInfo.Add(cp);
                }

                //}
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return listInfo;
        }       // zm 8.25 Oracle
        #endregion

        #region 异常

        /// <summary>
        /// 插入异常
        /// </summary>
        /// <param name="strMessages"></param>
        /// <param name="strModuleName"></param>
        /// <param name="strCreateUser"></param>
        [OperationContract]
        public void InsertLogException(LoginException logException)
        {
            //using (SqlConnection conn = new SqlConnection(m_ConnectionString))
            //{
            try
            {
                string strSql = @"INSERT INTO CP_LogException (Messages,Module_Name,HostName,MACADDR,Client_ip,CREATE_USER)"
                                + @" VALUES('{0}','{1}','{2}','{3}','{4}','{5}')";
                string strCmd = string.Format(strSql, logException.ErroMsg, logException.ModelName, logException.HostName, logException.MacAddress, logException.Ip, logException.CreateUser);
                //SqlCommand sqlCommand = new SqlCommand(strCmd, conn);
                //conn.Open();
                //sqlCommand.ExecuteNoneQuery();

                SqlHelper.ExecuteNoneQuery(strCmd);

            }
            catch
            { }

        }


        /// <summary>
        /// 抛出异常
        /// </summary>
        /// <param name="ex"></param>
        public void ThrowException(Exception ex)
        {
            //string strPath = HostingEnvironment.MapPath(System.Configuration.ConfigurationSettings.AppSettings["log"]);


            //if (!File.Exists(strPath))
            //{
            //    File.Create(strPath);

            //}
            //File.AppendAllText(strPath, String.Format("时间：【{0}】\n\r消息：【{1}】\n\r跟踪【{2}】", DateTime.Now, ex.Message, ex.StackTrace), Encoding.UTF8);
            if (ex.GetType() == typeof(SqlException))
            {
                ThrowSqlExpection(ex as SqlException);
            }
            else
            {
                ThrowNormalExpection(ex);
            }

        }

        /// <summary>
        /// 抛出SQL异常
        /// </summary>
        /// <param name="ex"></param>
        private void ThrowSqlExpection(SqlException ex)
        {
            LoginException fault = InitLogException(ex);
            InsertLogException(fault);
            throw new FaultException<LoginException>(fault, fault.ErroMsg);
        }

        /// <summary>
        /// 抛出普通异常
        /// </summary>
        /// <param name="ex"></param>
        private void ThrowNormalExpection(Exception ex)
        {
            LoginException fault = InitLogException(ex);
            InsertLogException(fault);
            throw new FaultException<LoginException>(fault, fault.ErroMsg);
        }

        private LoginException InitLogException(Exception ex)
        {
            string strIp = string.Empty;
            string strMacAdd = string.Empty;
            try
            {
                strIp = GetClientIp();
                strMacAdd = GetMadAddress(strIp);
            }
            catch
            {
            }
            LoginException fault = new LoginException();
            fault.ErrorLevel = ErrorLevel.PasswordValid;
            fault.ErroMsg = ex.Message.Replace("'", "").Replace("\r\n", @"\n") + ex.StackTrace;
            fault.ModelName = string.Empty;
            fault.CreateUser = string.Empty;
            fault.HostName = string.Empty;
            fault.Ip = strIp;
            fault.MacAddress = strMacAdd;
            return fault;
        }

        [OperationContract]
        [FaultContract(typeof(FaultException))]
        public string GetClientIp()
        {
            try
            {
                OperationContext context = OperationContext.Current;
                MessageProperties messageProperties = context.IncomingMessageProperties;
                RemoteEndpointMessageProperty endpointProperty = messageProperties[RemoteEndpointMessageProperty.Name] as RemoteEndpointMessageProperty;
                return endpointProperty.Address;
            }
            catch (SqlException ex)
            {
                ThrowSqlExpection(ex);
                return string.Empty;
            }
            catch (Exception ex)
            {
                ThrowNormalExpection(ex);
                return string.Empty;
            }
        }

        [DllImport("Iphlpapi.dll")]
        private static extern int SendARP(Int32 dest, Int32 host, ref Int64 mac, ref Int32 length);
        [DllImport("Ws2_32.dll")]
        private static extern Int32 inet_addr(string ip);


        private string GetMadAddress(string strIp)
        {
            Int32 ldest = inet_addr(strIp); //目的地的ip 
            Int64 macinfo = new Int64();

            string strMacSrc = macinfo.ToString("X");

            while (strMacSrc.Length < 12)
            {
                strMacSrc = strMacSrc.Insert(0, "0");
            }

            string strMacInfo = "";

            for (int i = 0; i < 11; i++)
            {
                if (0 == (i % 2))
                {
                    if (i == 10)
                    {
                        strMacInfo = strMacInfo.Insert(0, strMacSrc.Substring(i, 2));
                    }
                    else
                    {
                        strMacInfo = "-" + strMacInfo.Insert(0, strMacSrc.Substring(i, 2));
                    }
                }
            }
            return strMacInfo;


        }
        #endregion

        // add by luff 20130119
        #region 判断HIS参数类型是否有值
        /// <summary>
        /// 判断HIS参数类型是否有值
        /// </summary>

        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public int GetAppConifgType(string sKey)
        {
            int i_appconfigValue = -2;

            if (i_appconfigValue == -2)
            {
                string sqlStr = string.Empty;

                //Value判断HIS 是否参参数值  -1表示为空值
                sqlStr = @"select Value from APPCFG where Configkey = '" + sKey + "'";

                DataTable dt = SqlHelper.ExecuteDataTable(sqlStr);
                if (dt == null || dt.Rows.Count == 0)
                {
                    i_appconfigValue = -1;
                }
                else
                {
                    i_appconfigValue = int.Parse(dt.Rows[0][0].ToString());
                }
            }
            return i_appconfigValue;

        }

        #endregion

        // add by luff 20130130
        #region 规范化的新增数据库数据的方法 新增EhrtoHis配置表
        /// <summary>
        /// 新增EhrtoHis配置表操作
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="Operation"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Boolean EhrtoHisAppInsert(HisSxpz model)
        {


            Boolean returnBool = false;
            try
            {
                if (model == null)
                {
                    model = new HisSxpz();
                    returnBool = false;

                }
                else
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into HisSxpz(");
                    strSql.Append("EhrKey,Ehr_Keyms,HisKey,His_Keyms,EhrSource)");
                    strSql.Append(" values (");
                    strSql.Append("@EhrKey,@Ehr_Keyms,@HisKey,@His_Keyms,@EhrSource)");
                    strSql.Append(";select @@IDENTITY");
                    SqlParameter[] parameters = {
					new SqlParameter("@EhrKey", SqlDbType.VarChar,50),
					new SqlParameter("@Ehr_Keyms", SqlDbType.VarChar,500),
					new SqlParameter("@HisKey", SqlDbType.VarChar,50),
					new SqlParameter("@His_Keyms", SqlDbType.VarChar,500),
                    new SqlParameter("@EhrSource", SqlDbType.VarChar,500)};
                    parameters[0].Value = model.EhrKey;
                    parameters[1].Value = model.Ehr_Keyms;
                    parameters[2].Value = model.HisKey;
                    parameters[3].Value = model.His_Keyms;
                    parameters[4].Value = model.EhrSource;

                    //DataTable dt = null;
                    SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                    returnBool = true;

                }
            }

            catch (Exception ex)
            {
                ThrowException(ex);
                returnBool = false;
            }
            return returnBool;

        }
        #endregion

        #region 规范化的更新数据库数据的方法  更新EhrtoHis配置表
        /// <summary>
        /// 更新EhrtoHis配置表操作
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="Operation"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Boolean EhrToHisAppUpdate(HisSxpz model)
        {
            Boolean returnBool = false;
            try
            {
                if (model == null)
                {
                    model = new HisSxpz();
                    returnBool = false;

                }
                else
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update HisSxpz set ");
                    strSql.Append("EhrKey=@EhrKey,");
                    strSql.Append("Ehr_Keyms=@Ehr_Keyms,");
                    strSql.Append("HisKey=@HisKey,");
                    strSql.Append("His_Keyms=@His_Keyms,");
                    strSql.Append("EhrSource=@EhrSource");
                    strSql.Append(" where ID=@ID");
                    SqlParameter[] parameters = {
					        new SqlParameter("@EhrKey", SqlDbType.VarChar,50),
					        new SqlParameter("@Ehr_Keyms", SqlDbType.VarChar,500),
					        new SqlParameter("@HisKey", SqlDbType.VarChar,50),
					        new SqlParameter("@His_Keyms", SqlDbType.VarChar,500),
                            new SqlParameter("@EhrSource", SqlDbType.VarChar,500),
					        new SqlParameter("@ID", SqlDbType.Int,4)};
                    parameters[0].Value = model.EhrKey;
                    parameters[1].Value = model.Ehr_Keyms;
                    parameters[2].Value = model.HisKey;
                    parameters[3].Value = model.His_Keyms;
                    parameters[4].Value = model.EhrSource;
                    parameters[5].Value = model.ID;

                    //DataTable dt = null;
                    SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                    returnBool = true;
                }


            }

            catch (Exception ex)
            {
                ThrowException(ex);
                returnBool = false;
            }
            return returnBool;
        }


        /// <summary>
        ///规范化获取数据库数据的方法 插入并查询 His接口配置
        /// </summary>
        /// <param name="drug"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<HisSxpz> GetEhrToHisApp(int iID)
        {
            List<HisSxpz> list = new List<HisSxpz>();
            try
            {
                //if (parameter == null) parameter = new HisSxpz();

                string sSql = @"select * from HisSxpz where 1=" + iID;


                DataTable dt = null;

                dt = SqlHelper.ExecuteDataTable(sSql);


                if (dt != null)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        HisSxpz model = new HisSxpz();
                        if (item["ID"] != null && item["ID"].ToString() != "")
                        {
                            model.ID = int.Parse(item["ID"].ToString());
                        }

                        model.EhrKey = item["EhrKey"].ToString();


                        model.Ehr_Keyms = item["Ehr_Keyms"].ToString();


                        model.HisKey = item["HisKey"].ToString();


                        model.His_Keyms = item["His_Keyms"].ToString();
                        model.EhrSource = item["EhrSource"].ToString();

                        list.Add(model);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }
        #endregion

        #region 规范化的删除数据库数据的方法 删除EhrtoHis配置表
        /// <summary>
        /// 删除EhrtoHis配置表操作
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="Operation"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Boolean EhrtoHisAppDel(int iID)
        {


            Boolean returnBool = false;
            try
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from HisSxpz ");
                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)};
                parameters[0].Value = iID;
                SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                returnBool = true;


            }

            catch (Exception ex)
            {
                ThrowException(ex);
                returnBool = false;
            }
            return returnBool;

        }
        #endregion

        private void SychPathStatus2Emr(string hissyxh, int status)
        {
            if (Sync2Emr)
            {
                try
                {
                    //using (SqlConnection con = new SqlConnection(m_ConnectionStringEMR))
                    //{
                    //根据his首页序号同步病人路径状态
                    SqlParameter hissyxhparam = new SqlParameter("PatNoOfHis", hissyxh);
                    SqlParameter statusparam = new SqlParameter("CPStatus", status);
                    SqlParameter[] sqlparams = new SqlParameter[] { hissyxhparam, statusparam };
                    string sqlcmd = "update InPatient set  CPStatus=@CPStatus  where PatNoOfHis=@PatNoOfHis";

                    emrsql.ExecuteNoneQuery(sqlcmd, sqlparams, CommandType.Text);


                    //}
                }
                catch (SqlException ex)
                {
                    ThrowSqlExpection(ex);
                }

            }


        }
    }

    /// <summary>
    /// 医嘱管理标志
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// 长期医嘱
        /// </summary>
        Long = 2703,
        /// <summary>
        /// 临时医嘱
        /// </summary>
        Temp = 2702,
        /// <summary>
        ///  普通
        /// </summary>
        Normal = 2700,
        /// <summary>
        /// 不用于医嘱
        /// </summary>
        NoUse = 2701
    }
}
