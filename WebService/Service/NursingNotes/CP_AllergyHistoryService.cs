
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

        #region 病人过敏史维护 CP_AllergyHistory

        /// <summary>
        /// 根据病人首页序号获取病人过敏史
        /// </summary>
        /// <param name="Syxh">病人首页序号</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_AllergyHistory> GetCP_AllergyHistoryList(string Syxh)
        {
            List<CP_AllergyHistory> list = new List<CP_AllergyHistory>();
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@ID","0"),
                    new SqlParameter("@Syxh",Syxh),
                    new SqlParameter("@Gmlx","0"),
                    new SqlParameter("@Gmcd","0"),
                    new SqlParameter("@Dlys",""),
                    new SqlParameter("@Gmbw",""),
                    new SqlParameter("@Fylx",""),
                    new SqlParameter("@Memo",""),
                    new SqlParameter("@OperateType","Select")
                };

                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_AllergyHistory_Operate", param, CommandType.StoredProcedure);

                foreach (DataRow row in dt.Rows)
                {
                    CP_AllergyHistory allergy = new CP_AllergyHistory();
                    allergy.ID = row["ID"].ToString();
                    allergy.Syxh = row["Syxh"].ToString();
                    allergy.Gmlx = row["Gmlx"].ToString();
                    allergy.Gmlx_Name = row["Gmlx_Name"].ToString();
                    allergy.Gmcd = row["Gmcd"].ToString();
                    allergy.Gmcd_Name = row["Gmcd_Name"].ToString();
                    allergy.Dlys = row["Dlys"].ToString();
                    allergy.Gmbw = row["Gmbw"].ToString();
                    allergy.Fylx = row["Fylx"].ToString();
                    allergy.Memo = row["Memo"].ToString();

                    list.Add(allergy);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        /// <summary>
        /// 操作表CP_AllergyHistory，对表CP_AllergyHistory进行增加，修改，删除
        /// </summary>
        /// <param name="allergy">CP_AllergyHistory对应实体</param>
        /// <param name="type">修改类型：Insert、Update、Delete</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage OperCP_AllergyHistory(CP_AllergyHistory allergy, string type)
        {
            SQLMessage sqlmess = new SQLMessage();
            try
            {
                SqlParameter[] param = CreateParamenterAllergy(allergy, type);

                DataTable dt = SqlHelper.ExecuteDataTable( "usp_CP_AllergyHistory_Operate", param, CommandType.StoredProcedure);

                if (dt.Rows.Count == 1)
                {
                    if (Convert.ToInt32(dt.Rows[0][0].ToString()) > 0)
                    {
                        sqlmess.IsSucceed = true;

                        sqlmess.Message = "病人过敏史操作成功！";
                        return sqlmess;
                    }
                    else
                    {
                        sqlmess.IsSucceed = false;
                        sqlmess.Message = "病人过敏史操作失败！";
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

        private SqlParameter[] CreateParamenterAllergy(CP_AllergyHistory allergy, string Type)
        {
            SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@ID",allergy.ID),
                    new SqlParameter("@Syxh",allergy.Syxh),
                    new SqlParameter("@Gmlx",allergy.Gmlx),
                    new SqlParameter("@Gmcd",allergy.Gmcd),
                    new SqlParameter("@Dlys",allergy.Dlys),
                    new SqlParameter("@Gmbw",allergy.Gmbw),
                    new SqlParameter("@Fylx",allergy.Fylx),
                    new SqlParameter("@Memo",allergy.Memo),
                    new SqlParameter("@OperateType",Type)
                };
            return param;
        }

        #endregion
    }


}
