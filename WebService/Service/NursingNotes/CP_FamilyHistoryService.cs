
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
using YidanSoft.Tool;
using Yidansoft.Service.Entity.NursingNotes;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        #region 病人家族史维护 CP_FamilyHistory

        /// <summary>
        /// 根据病人首页序号获取病人家族史
        /// </summary>
        /// <param name="Syxh">病人首页序号</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_FamilyHistory> GetCP_FamilyHistoryList(string Syxh)
        {
            string sqlcmd = @"select ID,Jzgx,b.Name JzgxName,Xsnl=DATEDIFF(yy,Csrq,getdate())+1
                            ,a.Bzdm,c.Name BzdmName,Sfjz,(CASE WHEN Sfjz=1 THEN '是' ELSE '否' END) as SfjzName,Swyy,a.Memo ,a.csrq
                             from CP_FamilyHistory a 
                             LEFT JOIN CP_DataCategoryDetail b ON a.Jzgx=b.Mxbh AND b.Lbbh=62 
                             LEFT JOIN Diagnosis c ON a.Bzdm=c.Markid  
                             where Syxh=@Syxh";
            List<CP_FamilyHistory> list = new List<CP_FamilyHistory>();
            SqlParameter param = new SqlParameter("Syxh", Syxh);
            SqlParameter[] parameters = new SqlParameter[] { param };
            try
            {
                DataTable dt = SqlHelper.ExecuteDataTable(sqlcmd, parameters, CommandType.Text);

                foreach (DataRow row in dt.Rows)
                {
                    CP_FamilyHistory family = new CP_FamilyHistory();
                    family.ID = Convert.ToDecimal(row["ID"]);
                    family.Jzgx = Convert.ToInt16(row["Jzgx"]);
                    family.JzgxName = row["JzgxName"].ToString();
                    family.Csrq = row["Csrq"].ToString();
                    family.Xsnl = row["Xsnl"].ToString();
                    family.Sfjz = Convert.ToInt32(row["Sfjz"]);
                    family.SfjzName = row["SfjzName"].ToString();
                    family.Swyy = row["Swyy"].ToString();
                    family.Bzdm = row["Bzdm"].ToString();
                    family.BzdmName = row["BzdmName"].ToString();
                    family.Memo = row["Memo"].ToString();

                    list.Add(family);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }


        /// <summary>
        /// 操作表CP_FamilyHistory，对表CP_FamilyHistory进行增加，修改，删除
        /// </summary>
        /// <param name="family">CP_FamilyHistory对应实体</param>
        /// <param name="type">修改类型：Insert、Update、Delete</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage OperateCP_FamilyHistory(CP_FamilyHistory family, string type)
        {
            SQLMessage sqlmess = new SQLMessage();
            try
            {
                SqlParameter[] param = CreateParamenter(family, type);

                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_FamilyHistory_Operate", param, CommandType.StoredProcedure);

                if (dt.Rows.Count == 1)
                {
                    if (Convert.ToInt32(dt.Rows[0][0].ToString()) > 0)
                    {
                        sqlmess.IsSucceed = true;

                        sqlmess.Message = "家族史维护成功！";
                        return sqlmess;
                    }
                    else
                    {
                        sqlmess.IsSucceed = false;
                        sqlmess.Message = "家族史维护失败！";
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

        private SqlParameter[] CreateParamenter(CP_FamilyHistory family, string Type)
        {
            SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@ID",family.ID),
                    new SqlParameter("@Syxh",family.Syxh),
                    new SqlParameter("@Jzgx",family.Jzgx),
                    new SqlParameter("@Bzdm",family.Bzdm),
                    new SqlParameter("@Csrq",family.Csrq),
                    new SqlParameter("@Sfjz",family.Sfjz),
                    new SqlParameter("@Swyy",family.Swyy),
                    new SqlParameter("@Memo",family.Memo),
                    new SqlParameter("@OperateType",Type)
                };
            return param;
        }


        #endregion
    }
}


