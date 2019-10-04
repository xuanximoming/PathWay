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
        #region 病人手术史维护 CP_SurgeryHistory

        /// <summary>
        /// 根据病人首页序号获取病人手术
        /// </summary>
        /// <param name="Syxh">病人首页序号</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_SurgeryHistory> GetCP_SurgeryHistoryList(string Syxh)
        {
            List<CP_SurgeryHistory> list = new List<CP_SurgeryHistory>();
            try
            {
                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@ID","0"),
                    new SqlParameter("@Syxh",Syxh),
                    new SqlParameter("@Ssdm",""),
                    new SqlParameter("@Bzdm",""),
                    new SqlParameter("@Sspl",""),
                    new SqlParameter("@Ssys",""),
                    new SqlParameter("@Memo",""),
                    new SqlParameter("@OperateType","Select")
                };

                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_SurgeryHistory_Operate", param, CommandType.StoredProcedure);

                foreach (DataRow row in dt.Rows)
                {
                    CP_SurgeryHistory surgery = new CP_SurgeryHistory();
                    surgery.ID = row["ID"].ToString();
                    surgery.Syxh = row["Syxh"].ToString();
                    surgery.Ssdm = row["Ssdm"].ToString();
                    surgery.SsName = row["SsName"].ToString();
                    surgery.Bzdm = row["Bzdm"].ToString();
                    surgery.BzName = row["BzName"].ToString();
                    surgery.Sspl = row["Sspl"].ToString();
                    surgery.Ssys = row["Ssys"].ToString();
                    surgery.Memo = row["Memo"].ToString();

                    list.Add(surgery);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        /// <summary>
        /// 操作表CP_SurgeryHistory，对表CP_SurgeryHistory进行增加，修改，删除
        /// </summary>
        /// <param name="Surgery">CP_SurgeryHistory对应实体</param>
        /// <param name="type">修改类型：Insert、Update、Delete</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage OperCP_SurgeryHistory(CP_SurgeryHistory surgery, string type)
        {
            SQLMessage sqlmess = new SQLMessage();
            try
            {
                SqlParameter[] param = CreateParamenterSurgery(surgery, type);

                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_SurgeryHistory_Operate", param, CommandType.StoredProcedure);

                if (dt.Rows.Count == 1)
                {
                    if (Convert.ToInt32(dt.Rows[0][0].ToString()) > 0)
                    {
                        sqlmess.IsSucceed = true;

                        sqlmess.Message = "病人手术史操作成功！";
                        return sqlmess;
                    }
                    else
                    {
                        sqlmess.IsSucceed = false;
                        sqlmess.Message = "病人手术史操作失败！";
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

        private SqlParameter[] CreateParamenterSurgery(CP_SurgeryHistory surgery, string Type)
        {
            SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@ID",surgery.ID),
                    new SqlParameter("@Syxh",surgery.Syxh),
                    new SqlParameter("@Ssdm",surgery.Ssdm),
                    new SqlParameter("@Bzdm",surgery.Bzdm),
                    new SqlParameter("@Sspl",surgery.Sspl),
                    new SqlParameter("@Ssys",surgery.Ssys),
                    new SqlParameter("@Memo",surgery.Memo),
                    new SqlParameter("@OperateType",Type)
                };
            return param;
        }

        #endregion
    }


}
