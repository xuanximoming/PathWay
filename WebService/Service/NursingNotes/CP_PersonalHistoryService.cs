
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

        /// <summary>
        /// 根据病人首页序号获取病人个人史
        /// </summary>
        /// <param name="Syxh">病人首页序号</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_PersonalHistory> GetCP_PersonalHistoryList(string Syxh)
        {
            string sqlcmd = "SELECT * FROM CP_PersonalHistory "
                            + " where Syxh=@Syxh";
            List<CP_PersonalHistory> list = new List<CP_PersonalHistory>();
            SqlParameter param = new SqlParameter("Syxh", Syxh);
            SqlParameter[] parameters = new SqlParameter[] { param };
            try
            {
                DataTable dt = SqlHelper.ExecuteDataTable(sqlcmd, parameters, CommandType.Text);

                foreach (DataRow row in dt.Rows)
                {
                    CP_PersonalHistory personal = new CP_PersonalHistory();
                    personal.ID = Convert.ToDecimal(row["ID"]);
                    personal.Sfxy = Convert.ToInt32(row["Sfxy"]);
                    personal.Sfyj = Convert.ToInt32(row["Sfyj"]);
                    personal.Xys = row["Xys"].ToString();
                    personal.Yjs = row["Yjs"].ToString();
                    personal.Hyzk = row["Hyzk"].ToString();
                    personal.Hzsl = Convert.ToInt32(row["Hzsl"]);
                    personal.Zymc = row["Zymc"].ToString();
                    personal.Csd = row["Csd"].ToString();
                    personal.Jld = row["Jld"].ToString();
                    personal.Memo = row["Memo"].ToString();
                    personal.Syxh = Convert.ToDecimal(row["Syxh"]);
                    list.Add(personal);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }



        /// <summary>
        /// 操作表CP_PersonalHistory，对表CP_PersonalHistory进行增加，修改，删除
        /// </summary>
        /// <param name="family">CP_PersonalHistory对应实体</param>
        /// <param name="type">修改类型：Insert、Update、Delete</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage OperateCP_PersonalHistory(CP_PersonalHistory personal, string type)
        {
            SQLMessage sqlmess = new SQLMessage();
            try
            {
                SqlParameter[] param = CreateParamenter(personal, type);

                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_PersonalHistory_Operate", param, CommandType.StoredProcedure);

                if (dt.Rows.Count == 1)
                {
                    if (Convert.ToInt32(dt.Rows[0][0].ToString()) > 0)
                    {
                        sqlmess.IsSucceed = true;

                        sqlmess.Message = "个人史维护成功！";
                        return sqlmess;
                    }
                    else
                    {
                        sqlmess.IsSucceed = false;
                        sqlmess.Message = "个人史维护失败！";
                        return sqlmess;
                    }
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return sqlmess;

        }

        private SqlParameter[] CreateParamenter(CP_PersonalHistory personal, string Type)
        {
            SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@ID",personal.ID),
                    new SqlParameter("@Syxh",personal.Syxh),
                    new SqlParameter("@Hyzk",personal.Hyzk),
                    new SqlParameter("@Hzsl",personal.Hzsl),
                    new SqlParameter("@Zymc",personal.Zymc),
                    new SqlParameter("@Sfyj",personal.Sfyj),
                    new SqlParameter("@Yjs",personal.Yjs),
                    new SqlParameter("@Sfxy",personal.Memo),
                    new SqlParameter("@Xys",personal.Xys),
                    new SqlParameter("@Csd",personal.Csd),
                    new SqlParameter("@Jld",personal.Jld),
                    new SqlParameter("@Memo",personal.Memo),
                    new SqlParameter("@OperateType",Type)
                };
            return param;
        }


    }


}
