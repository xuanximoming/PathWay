using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Yidansoft.Service.Entity;
using System.ServiceModel;
using System.Data.SqlClient;
using System.Data;
using YidanSoft.Tool;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {

        #region 路径维护相关
        /// <summary>
        /// 获取临床路径护理系统基本执项目信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_NurExecInfo> GetNurExecInfo()
        {
            List<CP_NurExecInfo> listInfo = new List<CP_NurExecInfo>();

            try
            {
                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_GetNurExecBasicInfo");
                foreach (DataRow row in dataTable.Rows)
                {
                    CP_NurExecInfo info = new CP_NurExecInfo();
                    info.Xmxh = row["Xmxh"].ToString();
                    info.XmxhName = row["XmxhName"].ToString();
                    info.Lbxh = row["Lbxh"].ToString();
                    info.LbxhName = row["LbxhName"].ToString();
                    info.LbOrderValue = YidanSoft.Tool.ConvertMy.ToDecimal(row["LbOrderValue"].ToString());
                    info.IsUserControl = YidanSoft.Tool.ConvertMy.ToDecimal(row["LbOrderValue"].ToString()) == 0 ? false : true;
                    info.Mxxh = row["Mxxh"].ToString();
                    info.MxxhName = row["MxxhName"].ToString();
                    info.InputType = YidanSoft.Tool.ConvertMy.ToDecimal(row["InputType"].ToString());
                    info.MxOrderValue = YidanSoft.Tool.ConvertMy.ToDecimal(row["MxOrderValue"].ToString());
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
        ///  保存护理对应路径信息
        /// </summary>
        /// <param name="listInfo"></param>
        /// <param name="strUser"></param> 
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public void SaveNurExecToPath(List<CP_NurExecInfo> listInfo, String strUser)
        {
            SqlConnection myConnection = null;
            SqlTransaction sqlTrans = null;
            using (SqlConnection myConn = new SqlConnection(m_ConnectionString))
            {
                SqlTransaction trans = null;

                try
                {
                    //myConn.Open();
                    //trans = myConn.BeginTransaction();

                    foreach (CP_NurExecInfo info in listInfo)
                    {
                        if (!info.IsNew && info.IsModify)
                        {
                            //目前只有选中不选中之分
                            if (!info.IsSelected)
                            {
                                CancelNurExecToPath(info, strUser);
                            }
                        }
                        else if (info.IsNew)
                        {
                            if (info.IsSelected)
                                InsertNurExecToPath(trans, info, strUser);
                        }
                    }
                    //trans.Commit();
                }
                catch (Exception ex)
                {
                    // trans.Rollback();
                    ThrowException(ex);
                }
                //   finally
                //   {
                //      if (myConn.State != ConnectionState.Closed)
                //         myConn.Close();
                //   }
            }
        }

        /// <summary>
        /// 插入临床护理对应路径结点信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="strUser"></param>
        private void InsertNurExecToPath(SqlTransaction trans, CP_NurExecInfo info, String strUser)
        {
            try
            {
                SqlParameter paraLjdm = new SqlParameter("@Ljdm", SqlDbType.VarChar, 12);
                SqlParameter paraPathDetailId = new SqlParameter("@PathDetailId", SqlDbType.VarChar, 50);
                SqlParameter paraMxxh = new SqlParameter("@Mxxh", SqlDbType.VarChar, 50);
                SqlParameter paraUser = new SqlParameter("@User", SqlDbType.VarChar, 10);
                paraLjdm.Value = info.Ljdm;
                paraPathDetailId.Value = info.PathDetailId;
                paraMxxh.Value = info.Mxxh;
                paraUser.Value = strUser;

                SqlParameter[] collention = new SqlParameter[] { paraLjdm, paraPathDetailId, paraMxxh, paraUser };

                SqlHelper.ExecuteNoneQuery("usp_CP_InsertNurExecToPath", collention, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 取消临床护理对应路径结点信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="strUser"></param>
        private void CancelNurExecToPath(CP_NurExecInfo info, String strUser)
        {

            try
            {
                SqlParameter paraId = new SqlParameter("@Id", SqlDbType.Decimal);
                SqlParameter paraUser = new SqlParameter("@User", SqlDbType.VarChar, 10);
                paraId.Value = info.ToPathId;
                paraUser.Value = strUser;

                SqlParameter[] collention = new SqlParameter[] { paraId, paraUser };

                SqlHelper.ExecuteNoneQuery("usp_CP_CancelNurExecToPath", collention, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        ///  获取临床护理对应路径结点信息
        /// </summary>
        /// <param name="strLjdm"></param>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_NurExecToPath> GetNurToPathInfo(String strLjdm, String strPathDetailId)
        {
            List<CP_NurExecToPath> listInfo = new List<CP_NurExecToPath>();
            try
            {
                SqlParameter paraLjdm = new SqlParameter("@Ljdm", SqlDbType.VarChar, 12);
                SqlParameter paraPathDetailId = new SqlParameter("@PathDetailId", SqlDbType.VarChar, 50);
                paraLjdm.Value = strLjdm;
                paraPathDetailId.Value = strPathDetailId;

                SqlParameter[] collections = new SqlParameter[] { paraLjdm, paraPathDetailId };

                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_GetNurToPathInfo", collections, CommandType.StoredProcedure);
                foreach (DataRow row in dataTable.Rows)
                {
                    CP_NurExecToPath info = new CP_NurExecToPath();
                    info.Id = YidanSoft.Tool.ConvertMy.ToDecimal(row["Id"].ToString());
                    info.Ljdm = row["Ljdm"].ToString();
                    info.PathDetailId = row["PathDetailId"].ToString();
                    info.Mxxh = row["Mxxh"].ToString();
                    info.Yxjl = Int16.Parse(row["Yxjl"].ToString());
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

        #region 路径执行相关
        /// <summary>
        ///  获取临床护理执行时对应路径结点信息
        /// </summary>
        /// <param name="strLjdm"></param>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_NurExecInfo> GetNurPathBasicInfo(CP_InpatinetList inpatientInfo, String strPathDetailId, String strPathChildId)
        {
            List<CP_NurExecInfo> listInfo = new List<CP_NurExecInfo>();
            try
            {
                SqlParameter paraSyxh = new SqlParameter("@Syxh", SqlDbType.Int);
                SqlParameter paraLjdm = new SqlParameter("@Ljdm", SqlDbType.VarChar, 50);
                SqlParameter paraPathDetailId = new SqlParameter("@PathDetailId", SqlDbType.VarChar, 50);
                SqlParameter paraPathChildId = new SqlParameter("@PathChildId", SqlDbType.VarChar, 50);
                SqlParameter paraPathLjxh = new SqlParameter("@Ljxh", SqlDbType.Decimal);
                paraSyxh.Value = inpatientInfo.Syxh;
                paraLjdm.Value = inpatientInfo.Ljdm;
                paraPathDetailId.Value = strPathDetailId;
                paraPathChildId.Value = strPathChildId;
                paraPathLjxh.Value = inpatientInfo.Ljxh;

                SqlParameter[] collections = new SqlParameter[] { paraSyxh, paraLjdm, paraPathDetailId, paraPathChildId, paraPathLjxh };

                DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_GetNurPathBasicInfo", collections, CommandType.StoredProcedure);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    CP_NurExecInfo info = new CP_NurExecInfo();
                    info.Xmxh = ConvertMy.ToString(row["Xmxh"]);
                    info.XmxhName = ConvertMy.ToString(row["XmxhName"]);
                    info.Lbxh = ConvertMy.ToString(row["Lbxh"]);
                    info.LbxhName = ConvertMy.ToString(row["LbxhName"]);
                    info.LbOrderValue = ConvertMy.ToDecimal(row["LbOrderValue"].ToString());
                    info.IsUserControl = ConvertMy.ToDecimal(row["LbOrderValue"].ToString()) == 0 ? false : true;
                    info.Mxxh = ConvertMy.ToString(row["MxxhId"]);
                    info.MxxhName = ConvertMy.ToString(row["MxxhName"]);
                    info.InputType = ConvertMy.ToDecimal(row["InputType"].ToString());
                    info.MxOrderValue = ConvertMy.ToDecimal(row["MxOrderValue"].ToString());
                    info.ToPathId = ConvertMy.ToDecimal(row["ToPathId"].ToString());
                    info.NurRecordId = ConvertMy.ToString(row["NurRecordId"]);
                    List<CP_NurExecuteResult> resultlist = new List<CP_NurExecuteResult>();
                    if (ds.Tables[1] != null)
                        if (ds.Tables[1].Rows.Count != 0)
                        {
                            foreach (DataRow item in ds.Tables[1].Rows)
                            {
                                CP_NurExecuteResult result = new CP_NurExecuteResult();
                                result.Jgbh = ConvertMy.ToString(item["Jgbh"]);
                                result.JgName = ConvertMy.ToString(item["Name"]);
                                result.Mxxh = ConvertMy.ToString(item["Mxxh"]);
                                if (result.Mxxh == info.Mxxh)
                                {
                                    resultlist.Add(result);
                                    info.ResultList = resultlist;
                                }
                            }
                        }
                    List<CP_NurExecRecordResult> recordResultList = new List<CP_NurExecRecordResult>();
                    if (ds.Tables[3] != null)
                        if (ds.Tables[3].Rows.Count != 0)
                        {
                            foreach (DataRow item in ds.Tables[2].Rows)
                            {
                                CP_NurExecRecordResult recordResult = new CP_NurExecRecordResult();
                                recordResult.Id = ConvertMy.ToString(item["Id"]);
                                recordResult.HlzxId = ConvertMy.ToString(item["HlzxId"]);
                                recordResult.JgId = ConvertMy.ToString(item["JgId"]);
                                recordResult.Yxjl = ConvertMy.ToString(item["Yxjl"]);
                                if (recordResult.HlzxId == info.NurRecordId)
                                {
                                    recordResultList.Add(recordResult);
                                    info.RecordResultList = recordResultList;
                                }
                            }
                        }
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
        ///  保存护理对应路径执行信息
        /// </summary>
        /// <param name="listInfo"></param>
        /// <param name="inpatientInfo"></param> 
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public void SaveNurExecRecord(List<CP_NurExecInfo> listInfo, CP_InpatinetList inpatientInfo)
        {
            SqlConnection myConn = null;
            SqlTransaction trans = null;
            //using (SqlConnection myConn = new SqlConnection(m_ConnectionString))
            //{
            //   SqlTransaction trans = null;

            try
            {
                //myConn.Open();
                //trans = myConn.BeginTransaction();

                foreach (CP_NurExecInfo info in listInfo)
                {
                    if (!info.IsNew && info.IsModify)
                    {
                        //目前只有选中不选中之分
                        if (!info.IsSelected)
                        {
                            CancelNurExecRecord(info, inpatientInfo);
                        }
                    }
                    else if (info.IsNew)
                    {
                        if (info.IsSelected)
                            InsertNurExecRecord(info, inpatientInfo);
                    }
                }
                trans.Commit();
            }
            catch (Exception ex)
            {
                // trans.Rollback();
                ThrowException(ex);
            }
            //   finally
            //   {
            //      if (myConn.State != ConnectionState.Closed)
            //         myConn.Close();
            //   }
            //}
        }


        /// <summary>
        ///  保存护理对应路径执行信息
        /// </summary>
        /// <param name="listInfo"></param>
        /// <param name="inpatientInfo"></param> 
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public void SaveNurExecuteRecord(List<CP_NurExecInfo> listInfo, CP_InpatinetList inpatientInfo)
        {
            using (SqlConnection myConn = new SqlConnection(m_ConnectionString))
            {
                SqlTransaction trans = null;


                try
                {
                    // myConn.Open();
                    // trans = myConn.BeginTransaction();

                    foreach (CP_NurExecInfo info in listInfo)
                    {
                        String strId = info.NurRecordId;
                        if (strId == String.Empty)
                        {
                            info.NurRecordId = ConvertMy.ToString(Guid.NewGuid());
                        }
                        InsertNurExecRecord(info, inpatientInfo);
                        if (info.ResultList != null)
                        {
                            foreach (CP_NurExecuteResult item in info.ResultList)
                            {
                                item.Id = ConvertMy.ToString(Guid.NewGuid());
                                if (item.Yxjl == null)
                                {
                                    item.Yxjl = "0";
                                }
                                InsertNurExecRecordResult(item, inpatientInfo.CurOper, info.NurRecordId);
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

            }
        }
        /// <summary>
        /// 插入临床护理对应路径结点执行信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="strUser"></param>
        private void InsertNurExecRecord(CP_NurExecInfo info, CP_InpatinetList inpatientInfo)
        {
            try
            {
                //SqlParameter paraId = new SqlParameter("@Id", SqlDbType.VarChar, 50);
                SqlParameter paraLjdm = new SqlParameter("@Ljdm", SqlDbType.VarChar, 50);
                SqlParameter paraPathDetailId = new SqlParameter("@PathDetailId", SqlDbType.VarChar, 50);
                SqlParameter paraMxxh = new SqlParameter("@Mxxh", SqlDbType.VarChar, 50);
                SqlParameter paraUser = new SqlParameter("@User", SqlDbType.VarChar, 10);
                SqlParameter paraljxh = new SqlParameter("@Ljxh", SqlDbType.Decimal);
                SqlParameter paraSyxh = new SqlParameter("@Syxh", SqlDbType.Decimal);
                //paraId.Value = info.NurRecordId;
                paraLjdm.Value = info.Ljdm;
                paraPathDetailId.Value = info.PathDetailId;
                paraMxxh.Value = info.Mxxh;
                paraUser.Value = inpatientInfo.CurOper;
                paraljxh.Value = inpatientInfo.Ljxh;
                paraSyxh.Value = inpatientInfo.Syxh;
//paraId,
                SqlParameter[] collention = new SqlParameter[] {  paraLjdm, paraPathDetailId, paraMxxh, paraUser, paraljxh, paraSyxh };

                SqlHelper.ExecuteNoneQuery("usp_CP_InsertNurExecRecord", collention, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 插入临床护理对应路径结点执行信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="strUser"></param>
        private void InsertNurExecRecordResult(CP_NurExecuteResult info, String Create_User, String HlzxId)
        {
            try
            {
                SqlParameter paraId = new SqlParameter("@Id", SqlDbType.VarChar, 50);
                SqlParameter paraHlzxId = new SqlParameter("@HlzxId", SqlDbType.VarChar, 50);
                SqlParameter paraJgId = new SqlParameter("@JgId", SqlDbType.VarChar, 12);
                SqlParameter paraYxjl = new SqlParameter("@Yxjl", SqlDbType.VarChar, 12);
                SqlParameter paraCreate_User = new SqlParameter("@Create_User", SqlDbType.VarChar, 10);
                paraId.Value = info.Id;
                paraHlzxId.Value = HlzxId;
                paraYxjl.Value = info.Yxjl;
                paraJgId.Value = info.Jgbh;
                paraCreate_User.Value = Create_User;

                SqlParameter[] collention = new SqlParameter[] { paraId, paraHlzxId, paraYxjl, paraJgId, paraCreate_User };

                SqlHelper.ExecuteNoneQuery("usp_CP_NurExecRecordResultManage", collention, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// 取消临床护理对应路径结点信息
        /// </summary>
        /// <param name="info"></param>
        /// <param name="strUser"></param>
        private void CancelNurExecRecord(CP_NurExecInfo info, CP_InpatinetList inpatientInfo)
        {

            try
            {
                SqlParameter paraId = new SqlParameter("@NurRecordId", SqlDbType.Decimal);
                SqlParameter paraUser = new SqlParameter("@User", SqlDbType.VarChar, 10);
                paraId.Value = info.NurRecordId;
                paraUser.Value = inpatientInfo.CurOper;

                SqlParameter[] collention = new SqlParameter[] { paraId, paraUser };

                SqlHelper.ExecuteNoneQuery("usp_CP_CancelNurExecRecord", collention, CommandType.StoredProcedure);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        // /// <summary>
        // /// 获取护理结果
        // /// </summary>
        // /// <param name="Mxxh">护理明细序号</param>
        // /// <returns></returns>
        //[OperationContract]
        //[FaultContract(typeof(LoginException))]
        //public List<CP_NurResult> GetNurExecuteResult(String Mxxh)
        //{
        //    List<CP_NurResult> listInfo = new List<CP_NurResult>();
        //    try
        //    {
        //        SqlParameter paraMxxh = new SqlParameter("@Mxxh", SqlDbType.VarChar, 50);
        //        SqlParameter paraGetType = new SqlParameter("@GetType", SqlDbType.VarChar, 20);
        //        paraMxxh.Value = Mxxh;
        //        paraGetType.Value = "GeyByMxxh";

        //        SqlParameter[] collections = new SqlParameter[] { paraMxxh, paraGetType };

        //        DataTable dataTable = SqlHelper.ExecuteDataTable(m_ConnectionString, System.Data.CommandType.StoredProcedure, "usp_CP_GetNurResult", collections);
        //        foreach (DataRow row in dataTable.Rows)
        //        {
        //            CP_NurResult info = new CP_NurResult();
        //            info.Jgbh = ConvertMy.ToString(row["Jgbh"]);
        //            info.Name = ConvertMy.ToString(row["Name"]);
        //            listInfo.Add(info);
        //        }
        //        return listInfo;
        //    }
        //    catch (Exception ex)
        //    {
        //        ThrowException(ex);
        //    }
        //    return listInfo;
        //}
    }
}