using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.ServiceModel;
using System.Data;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {

        #region
        /// <summary>
        /// 获取临床护理路径结点对应的基本异常信息
        /// </summary>
        /// <returns></returns>    
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_VariationToPathInfo> GetPathVariationBasInfo()
        {
            List<CP_VariationToPathInfo> listInfo = new List<CP_VariationToPathInfo>();
            try
            {
                DataTable dataTable = new DataTable();
                dataTable = SqlHelper.ExecuteDataTable("usp_CP_GetPathVariationInfo");


                foreach (DataRow row in dataTable.Rows)
                {
                    CP_VariationToPathInfo info = new CP_VariationToPathInfo();
                    info.Bydm = row["Bydm"].ToString();
                    info.Bymc = row["Bymc"].ToString();
                    info.Byms = row["Byms"].ToString();
                    info.IsModify = false;
                    info.IsNew = true;
                    info.IsSelected = false;
                    listInfo.Add(info);
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return listInfo;
        }

        /// <summary>
        /// 获取临床护理路径结点对应的异常信息
        /// </summary>
        /// <returns></returns>    
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_VariationToPath> GetVariationToPathInfo(String strActivityId,string sLjdm)
        {
            List<CP_VariationToPath> listInfo = new List<CP_VariationToPath>();
            try
            {
                SqlParameter paramActivityId = new SqlParameter("@ActivityId", SqlDbType.VarChar, 50);
                paramActivityId.Value = strActivityId;
                SqlParameter paramLjdm = new SqlParameter("@Ljdm", SqlDbType.VarChar, 12);
                paramLjdm.Value = sLjdm;
                DataTable dataTable = new DataTable();
                dataTable = SqlHelper.ExecuteDataTable("usp_CP_GetVariationToPathInfo", new SqlParameter[] { paramActivityId, paramLjdm }, CommandType.StoredProcedure);


                foreach (DataRow row in dataTable.Rows)
                {
                    CP_VariationToPath info = new CP_VariationToPath();
                    info.Id = YidanSoft.Tool.ConvertMy.ToDecimal(row["Id"].ToString());
                    info.Ljdm = row["Ljdm"].ToString();
                    info.ActivityId = row["ActivityId"].ToString();
                    info.Bydm = row["Bydm"].ToString();
                    listInfo.Add(info);
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return listInfo;
        }
        #endregion

        #region 保存
        /// <summary>
        ///  保存结点异常信息
        /// </summary>
        /// <param name="listInfo"></param> 
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public void SaveVariationToPath(List<CP_VariationToPathInfo> listInfo, String strUser)
        {
            using (SqlConnection conn = new SqlConnection(m_ConnectionString))
            {
                SqlTransaction trans = null;
                try
                {
                    //conn.Open();
                    // trans = conn.BeginTransaction();
                    foreach (CP_VariationToPathInfo info in listInfo)
                    {
                        if (!info.IsNew)
                        {
                            if (info.IsModify && !info.IsSelected)
                            {
                                CancelVariationToPath(info, strUser);
                            }
                        }
                        else if (info.IsNew)
                        {
                            if (info.IsSelected)
                            {
                                InsertVariationToPath(trans, info, strUser);
                            }
                        }
                    }
                    //trans.Commit();
                }
                catch (Exception ex)
                {
                    //trans.Rollback();
                    ThrowException(ex);
                }
                //   finally
                //   {
                //      if (conn.State != System.Data.ConnectionState.Closed)
                //         conn.Close();
                //   }
            }

        }


        /// <summary>
        /// 插入
        /// </summary>
        /// <param name="info"></param>
        private void InsertVariationToPath(SqlTransaction trans, CP_VariationToPathInfo info, String strUser)
        {
            try
            {
                SqlParameter paraLjdm = new SqlParameter("@Ljdm", SqlDbType.VarChar, 12);
                SqlParameter paraActivitylId = new SqlParameter("@ActivityId", SqlDbType.VarChar, 50);
                SqlParameter paraBydm = new SqlParameter("@Bydm", SqlDbType.VarChar, 12);
                SqlParameter paraUser = new SqlParameter("@User", SqlDbType.VarChar, 10);

                paraLjdm.Value = info.Ljdm;
                paraActivitylId.Value = info.ActivityId;
                paraBydm.Value = info.Bydm;
                paraUser.Value = strUser;

                SqlParameter[] collention = new SqlParameter[] { paraLjdm, paraActivitylId, paraBydm, paraUser };

                SqlHelper.ExecuteNoneQuery("usp_CP_InsertVariationToPath", collention, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 取消
        /// </summary>
        /// <param name="info"></param>
        private void CancelVariationToPath(CP_VariationToPathInfo info, String strUser)
        {
            try
            {
                SqlParameter paraToPathId = new SqlParameter("@Id", SqlDbType.Decimal);
                SqlParameter paraUser = new SqlParameter("@User", SqlDbType.VarChar, 10);

                paraToPathId.Value = info.ToPathId;
                paraUser.Value = strUser;

                SqlParameter[] collention = new SqlParameter[] { paraToPathId, paraUser };

                SqlHelper.ExecuteNoneQuery("usp_CP_CancelVariationToPath", collention, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion
    }
}