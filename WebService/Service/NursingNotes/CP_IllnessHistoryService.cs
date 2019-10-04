
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
using DrectSoft.Tool;
using Yidansoft.Service.Entity.NursingNotes;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        #region 病人手术史维护 CP_IllnessHistory

        /// <summary>
        /// 根据病人首页序号获取病人手术
        /// </summary>
        /// <param name="Syxh">病人首页序号</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_IllnessHistory> GetCP_IllnessHistoryList(string Syxh)
        {
            List<CP_IllnessHistory> list = new List<CP_IllnessHistory>();
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@ID","0"),
                    new SqlParameter("@Syxh",Syxh),
                    new SqlParameter("@Bzdm",""),
                    new SqlParameter("@Jbpl",""),
                    new SqlParameter("@Bfsj",""),
                    new SqlParameter("@Sfzy",""),
                    new SqlParameter("@Memo",""),
                    new SqlParameter("@OperateType","Select")
                };

                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_IllnessHistory_Operate", param, CommandType.StoredProcedure);

                foreach (DataRow row in dt.Rows)
                {
                    CP_IllnessHistory illness = new CP_IllnessHistory();
                    illness.ID = row["ID"].ToString();
                    illness.Syxh = row["Syxh"].ToString();
                    illness.Bzdm = row["Bzdm"].ToString();
                    illness.BzName = row["BzName"].ToString();
                    illness.Jbpl = row["Jbpl"].ToString();
                    illness.Bfsj = row["Bfsj"].ToString();
                    illness.Sfzy = row["Sfzy"].ToString();
                    illness.Memo = row["Memo"].ToString();

                    list.Add(illness);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        /// <summary>
        /// 操作表CP_IllnessHistory，对表CP_IllnessHistory进行增加，修改，删除
        /// </summary>
        /// <param name="illness">CP_IllnessHistory对应实体</param>
        /// <param name="type">修改类型：Insert、Update、Delete</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage OperCP_IllnessHistory(CP_IllnessHistory illness, string type)
        {
            SQLMessage sqlmess = new SQLMessage();
            try
            {
                SqlParameter[] param = CreateParamenterIllness(illness, type);

                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_IllnessHistory_Operate", param, CommandType.StoredProcedure);

                if (dt.Rows.Count == 1)
                {
                    if (Convert.ToInt32(dt.Rows[0][0].ToString()) > 0)
                    {
                        sqlmess.IsSucceed = true;

                        sqlmess.Message = "病人疾病史操作成功！";
                        return sqlmess;
                    }
                    else
                    {
                        sqlmess.IsSucceed = false;
                        sqlmess.Message = "病人疾病史操作失败！";
                        return sqlmess;
                    }
                }
                else
                {
                    sqlmess.IsSucceed = false;
                    sqlmess.Message = "病人疾病史操作失败！";
                    return sqlmess;
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return sqlmess;

        }

        private SqlParameter[] CreateParamenterIllness(CP_IllnessHistory illness, string Type)
        {
            if (illness.Sfzy == "是")
            {
                illness.Sfzy = "1";
            }
            else if (illness.Sfzy == "否")
            {
                illness.Sfzy = "0";
            }
            SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@ID",illness.ID),
                    new SqlParameter("@Syxh",illness.Syxh),
                    new SqlParameter("@Bzdm",illness.Bzdm),
                    new SqlParameter("@Jbpl",illness.Jbpl),
                    new SqlParameter("@Bfsj",illness.Bfsj),
                    new SqlParameter("@Sfzy",illness.Sfzy),
                    new SqlParameter("@Memo",illness.Memo),
                    new SqlParameter("@OperateType",Type)
                };
            return param;
        }

        #endregion
    }


}
