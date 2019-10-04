using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Data.SqlClient;
using System.Data;
using Yidansoft.Service.Entity;
using YidanSoft.Tool;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 添加时验证结果名称是否重复
        /// </summary>
        /// <param name="Name">结果名称</param>
        /// <returns>结果集</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal CheckNurResultInsert(String Name)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Jgbh",String.Empty),
                    new SqlParameter("@Name",Name),
                    new SqlParameter("@Yxjl",String.Empty),
                    new SqlParameter("@Create_User",String.Empty),
                    new SqlParameter("@Update_User",String.Empty),
                    new SqlParameter("@sqlType","InsertCheckInfo")
                };
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_NurResultManage", parameters, CommandType.StoredProcedure);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return -1;
        }
        /// <summary>
        /// 修改时验证结果名称是否重复
        /// </summary>
        /// <param name="Name">结果名称</param>
        /// <param name="Jgbh">结果编号</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal CheckNurReaultUpdate(String Name, String Jgbh)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Jgbh",Jgbh),
                    new SqlParameter("@Name",Name),
                    new SqlParameter("@Yxjl",String.Empty),
                    new SqlParameter("@Create_User",String.Empty),
                    new SqlParameter("@Update_User",String.Empty),
                    new SqlParameter("@sqlType","UpdateCheckInfo")
                };
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_NurResultManage", parameters, CommandType.StoredProcedure);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return -1;
        }

        /// <summary>
        /// 添加查询护理结果表
        /// </summary>
        /// <param name="nurResult">护理结果表实体</param>
        /// <returns>护理结果表集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_NurResult> InsertAndSelectNurResult(CP_NurResult nurResult)
        {
            List<CP_NurResult> nurResultList = new List<CP_NurResult>();
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Jgbh",String.Empty),
                    new SqlParameter("@Name",nurResult.Name),
                    new SqlParameter("@Yxjl",nurResult.Yxjl),
                    new SqlParameter("@Create_User",nurResult.Create_User),
                    new SqlParameter("@Update_User",nurResult.Update_User),
                    new SqlParameter("@sqlType","InsertAndSelect")
                };
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_NurResultManage", parameters, CommandType.StoredProcedure);
                foreach (DataRow dr in dt.Rows)
                {
                    CP_NurResult cnr = new CP_NurResult();
                    cnr.Jgbh = ConvertMy.ToString(dr["Jgbh"]);
                    cnr.Name = ConvertMy.ToString(dr["Name"]);
                    cnr.Yxjl = ConvertMy.ToString(dr["Yxjl"]);
                    cnr.Yxjlmc = ConvertMy.ToString(dr["Yxjlmc"]);
                    cnr.Create_Time = ConvertMy.ToString(dr["Create_Time"]);
                    cnr.Create_User = ConvertMy.ToString(dr["Create_User"]);
                    cnr.Update_Time = ConvertMy.ToString(dr["Update_Time"]);
                    cnr.Update_User = ConvertMy.ToString(dr["Update_User"]);
                    nurResultList.Add(cnr);
                }
                return nurResultList;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return nurResultList;
        }

        /// <summary>
        /// 修改护理结果表
        /// </summary>
        /// <param name="nurResult">护理结果表实体</param>
        /// <returns>结果集</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal UpdateNurResult(CP_NurResult nurResult)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Jgbh",nurResult.Jgbh),
                    new SqlParameter("@Name",nurResult.Name),
                    new SqlParameter("@Yxjl",nurResult.Yxjl),
                    new SqlParameter("@Create_User",String.Empty),
                    new SqlParameter("@Update_User",nurResult.Update_User),
                    new SqlParameter("@sqlType","Update")
                };
                SqlHelper.ExecuteNoneQuery("usp_CP_NurResultManage", parameters, CommandType.StoredProcedure);
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 删除时验证该数据是否在其它地方使用
        /// </summary>
        /// <param name="Jgbh">结果编号</param>
        /// <returns>受影响行数</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal CheckNurResultDelete(String Jgbh)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Jgbh",Jgbh),
                    new SqlParameter("@Name",String.Empty),
                    new SqlParameter("@Yxjl",String.Empty),
                    new SqlParameter("@Create_User",String.Empty),
                    new SqlParameter("@Update_User",String.Empty),
                    new SqlParameter("@sqlType","DeleteCheckInfo")
                };
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_NurResultManage", parameters, CommandType.StoredProcedure);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除护理结果信息
        /// </summary>
        /// <param name="Jgbh">结果编号</param>
        /// <returns>受影响行数</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal DeleteNurResult(String Jgbh)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Jgbh",Jgbh),
                    new SqlParameter("@Name",String.Empty),
                    new SqlParameter("@Yxjl",String.Empty),
                    new SqlParameter("@Create_User",String.Empty),
                    new SqlParameter("@Update_User",String.Empty),
                    new SqlParameter("@sqlType","Delete")
                };
                SqlHelper.ExecuteNoneQuery( "usp_CP_NurResultManage", parameters, CommandType.StoredProcedure);
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}