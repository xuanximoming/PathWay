using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using Yidansoft.Service.Entity;
using System.Data.SqlClient;
using System.Data;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 获取医生任务信息
        /// </summary>
        /// <param name="zgdm">医生工号</param>
        /// <param name="rwzt">任务状态  </param>
        /// <param name="rwsj"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        private List<CP_DoctorTaskMessage> GetDoctorTaskMessage(string zgdm, string rwzt, string rwsj)
        {
            List<CP_DoctorTaskMessage> listDocTaskMsg = new List<CP_DoctorTaskMessage>();
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@DoctorID",zgdm),
                        new SqlParameter("@Rwzt",rwzt),
                        new SqlParameter("@Day",rwsj)
                    };


                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_TaskMessage", parameters, CommandType.StoredProcedure);

                foreach (DataRow row in dataTable.Rows)
                {
                    CP_DoctorTaskMessage doctaskmsg = new CP_DoctorTaskMessage();

                    doctaskmsg.Syxh = row["Syxh"].ToString();
                    doctaskmsg.Cycw = row["Cycw"].ToString();
                    doctaskmsg.Hzxm = row["Hzxm"].ToString();
                    doctaskmsg.Ljmc = row["Ljmc"].ToString();
                    doctaskmsg.Ysxm = row["Ysxm"].ToString();
                    doctaskmsg.Mess = row["Mess"].ToString();
                    doctaskmsg.Yqsj = row["Yqsj"].ToString();
                    doctaskmsg.Rwzt = row["Rwzt"].ToString();
                    doctaskmsg.group_col = row["group_col"].ToString();
                    doctaskmsg.Yzlb = row["Yzlb"].ToString();

                    listDocTaskMsg.Add(doctaskmsg);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
 
            return listDocTaskMsg;
        }
    }
}