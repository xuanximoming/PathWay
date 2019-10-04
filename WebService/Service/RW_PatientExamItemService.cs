using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Yidansoft.Service.Entity;
using System.ServiceModel;
using System.Data.SqlClient;
using System.Data;
using DrectSoft.Tool;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        #region 添加查询病人检查项
        /// <summary>
        /// 添加查询病人检查项
        /// </summary>
        /// <param name="patientExamItem">病人检查项实体类</param>
        /// <returns>病人检查项集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<RW_PatientExamItem> InsertAndSelectPatientExamItem(RW_PatientExamItem patientExamItem)
        {
            List<RW_PatientExamItem> patientExamItemList = new List<RW_PatientExamItem>();
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Operation","InsertAndSelect"),
                    new SqlParameter("@ID",String.Empty),
                    new SqlParameter("@Syxh",patientExamItem.Syxh),
                    new SqlParameter("@Jcxm",patientExamItem.Jcxm),
                    new SqlParameter("@Jcjg",patientExamItem.Jcjg),
                    new SqlParameter("@Dw",patientExamItem.Dw),
                    new SqlParameter("@Bz",patientExamItem.Bz),
                };
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_PatientExamItem", parameters, CommandType.StoredProcedure);
                foreach (DataRow dr in dt.Rows)
                {
                    RW_PatientExamItem pei = new RW_PatientExamItem();
                    pei.ID = ConvertMy.ToString(dr["ID"]);
                    pei.Syxh = ConvertMy.ToString(dr["Syxh"]);
                    pei.Jcxm = ConvertMy.ToString(dr["Jcxm"]);
                    pei.Jcmc = ConvertMy.ToString(dr["Jcmc"]);
                    pei.Jcjg = ConvertMy.ToString(dr["Jcjg"]);
                    pei.Dw = ConvertMy.ToString(dr["Dw"]);
                    pei.Bz = ConvertMy.ToString(dr["Bz"]);
                    patientExamItemList.Add(pei);
                }
                return patientExamItemList;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return patientExamItemList;
        }
        #endregion

        #region 修改病人检查项
        /// <summary>
        /// 修改病人检查项
        /// </summary>
        /// <param name="patientExamItem">病人检查项实体</param>
        /// <returns>受影响行数</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal UpdatePatientExamItem(RW_PatientExamItem patientExamItem)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Operation","Update"),
                    new SqlParameter("@ID",patientExamItem.ID),
                    new SqlParameter("@Syxh",patientExamItem.Syxh),
                    new SqlParameter("@Jcxm",patientExamItem.Jcxm),
                    new SqlParameter("@Jcjg",patientExamItem.Jcjg),
                    new SqlParameter("@Dw",patientExamItem.Dw),
                    new SqlParameter("@Bz",patientExamItem.Bz),
                };
                SqlHelper.ExecuteNoneQuery("usp_CP_PatientExamItem", parameters, CommandType.StoredProcedure);
                return 1;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return -1;
        }
        #endregion

        #region 删除病人检查项
        /// <summary>
        /// 删除病人检查项
        /// </summary>
        /// <param name="ID">编号</param>
        /// <returns>受影响行数</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal DeletePatientExamItem(String ID)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Operation","Delete"),
                    new SqlParameter("@ID",ID),
                    new SqlParameter("@Syxh",String.Empty),
                    new SqlParameter("@Jcxm",String.Empty),
                    new SqlParameter("@Jcjg",String.Empty),
                    new SqlParameter("@Dw",String.Empty),
                    new SqlParameter("@Bz",String.Empty),
                };
                SqlHelper.ExecuteNoneQuery("usp_CP_PatientExamItem", parameters, CommandType.StoredProcedure);
                return 1;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return -1;
        }
        #endregion

        #region 验证病人检查项是否重复
        /// <summary>
        /// 验证病人检查项是否重复添加
        /// </summary>
        /// <param name="Syxh">首页序号</param>
        /// <param name="Jcxm">检查项目</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal CheckExamItems(String Syxh, String Jcxm)
        {
            try
            {


                String sqlStr = "SELECT * FROM CP_PatientExamItem WHERE Syxh = '" + Syxh + "' AND Jcxm = '" + Jcxm + "'";


                DataSet ds = SqlHelper.ExecuteDataSet(sqlStr);

                return ds.Tables[0].Rows.Count;
                //}
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return -1;
        }
        #endregion
    }
}