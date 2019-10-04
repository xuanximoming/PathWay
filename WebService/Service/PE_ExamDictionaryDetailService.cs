using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Yidansoft.Service.Entity;
using System.ServiceModel;
using System.Text;
using System.Data;
using DrectSoft.Tool;
using System.Data.SqlClient;
namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        #region 添加查询检查项目
        /// <summary>
        /// 添加查询检查项目表
        /// </summary>
        /// <returns>examDictionaryDetailList检查项目集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<PE_ExamDictionaryDetail> InsertAndSelectExamDictionaryDetail(PE_ExamDictionaryDetail examDictionaryDetail)
        {
            List<PE_ExamDictionaryDetail> examDictionaryDetailList = new List<PE_ExamDictionaryDetail>();
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Operation","InsertAndSelect"),
                    new SqlParameter("@Jcbm",examDictionaryDetail.Jcbm),
                    new SqlParameter("@Flbm",examDictionaryDetail.Flbm),
                    new SqlParameter("@Jcmc",examDictionaryDetail.Jcmc),
                    new SqlParameter("@Mcsx",examDictionaryDetail.Mcsx),
                    new SqlParameter("@Ksfw",examDictionaryDetail.Ksfw),
                    new SqlParameter("@Jsfw",examDictionaryDetail.Jsfw),
                    new SqlParameter("@Jsdw",examDictionaryDetail.Jsdw),
                    new SqlParameter("@Yxjl",examDictionaryDetail.Yxjl),
                    new SqlParameter("@Bz",examDictionaryDetail.Bz),
                };
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_ExamDictionaryDetail", parameters, CommandType.StoredProcedure);
                foreach (DataRow dr in dt.Rows)
                {
                    PE_ExamDictionaryDetail detail = new PE_ExamDictionaryDetail();
                    detail.Jcbm = ConvertMy.ToString(dr["Jcbm"]);
                    detail.Flbm = ConvertMy.ToString(dr["Flbm"]);
                    detail.Flmc = ConvertMy.ToString(dr["Flmc"]);
                    detail.Jcmc = ConvertMy.ToString(dr["Jcmc"]);
                    detail.Mcsx = ConvertMy.ToString(dr["Mcsx"]);
                    detail.Ksfw = ConvertMy.ToString(dr["Ksfw"]);
                    detail.Jsfw = ConvertMy.ToString(dr["Jsfw"]);
                    detail.Jsdw = ConvertMy.ToString(dr["Jsdw"]);
                    detail.Py = ConvertMy.ToString(dr["Py"]);
                    detail.Wb = ConvertMy.ToString(dr["Wb"]);
                    detail.Yxjl = ConvertMy.ToString(dr["Yxjlmc"]);
                    detail.Bz = ConvertMy.ToString(dr["Bz"]);
                    examDictionaryDetailList.Add(detail);
                }
                return examDictionaryDetailList;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return examDictionaryDetailList;
        }


        /// <summary>
        /// 根据输入的查询关键字，获取检查项目表
        /// </summary>
        /// <returns>examDictionaryDetailList检查项目集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<PE_ExamDictionaryDetail> SelectExamDictionaryDetail(string key)
        {
            List<PE_ExamDictionaryDetail> examDictionaryDetailList = new List<PE_ExamDictionaryDetail>();
            try
            {
                string sql;
                if (key == "" || key == null)
                {
                    sql = @"SELECT Detail.Jcbm,Flbm,Flmc,Jcmc,Mcsx,Ksfw,Jsfw,Jsdw,Py,Wb,
		                              Yxjlmc = CASE WHEN Yxjl=1 THEN '有效' WHEN Yxjl=0 THEN '无效' END,Bz 
                                      FROM CP_ExamDictionaryDetail Detail
		                              LEFT JOIN
		                              (
		                              SELECT Jcbm,Jcmc Flmc FROM CP_ExamDictionary
		                              )T ON T.Jcbm=Detail.Flbm  ORDER BY Jlxh DESC";
                }
                else
                {
                    sql = string.Format(@"SELECT Detail.Jcbm,Flbm,Flmc,Jcmc,Mcsx,Ksfw,Jsfw,Jsdw,Py,Wb,
		                              Yxjlmc = CASE WHEN Yxjl=1 THEN '有效' WHEN Yxjl=0 THEN '无效' END,Bz 
                                      FROM CP_ExamDictionaryDetail Detail
		                              LEFT JOIN
		                              (
		                              SELECT Jcbm,Jcmc Flmc FROM CP_ExamDictionary
		                              )T ON T.Jcbm=Detail.Flbm where Detail.Jcbm like '%{0}%' or  Jcmc like '%{0}%' or Mcsx like '%{0}%' or Py like '%{0}%' or Wb like '%{0}%'
		                              ORDER BY Jlxh DESC", key);
                }
                DataTable dt = SqlHelper.ExecuteDataTable(sql); //= SqlHelper.ExecuteDataTable("usp_CP_ExamDictionaryDetail", parameters, CommandType.StoredProcedure);
                foreach (DataRow dr in dt.Rows)
                {
                    PE_ExamDictionaryDetail detail = new PE_ExamDictionaryDetail();
                    detail.Jcbm = ConvertMy.ToString(dr["Jcbm"]);
                    detail.Flbm = ConvertMy.ToString(dr["Flbm"]);
                    detail.Flmc = ConvertMy.ToString(dr["Flmc"]);
                    detail.Jcmc = ConvertMy.ToString(dr["Jcmc"]);
                    detail.Mcsx = ConvertMy.ToString(dr["Mcsx"]);
                    detail.Ksfw = ConvertMy.ToString(dr["Ksfw"]);
                    detail.Jsfw = ConvertMy.ToString(dr["Jsfw"]);
                    detail.Jsdw = ConvertMy.ToString(dr["Jsdw"]);
                    detail.Py = ConvertMy.ToString(dr["Py"]);
                    detail.Wb = ConvertMy.ToString(dr["Wb"]);
                    detail.Yxjl = ConvertMy.ToString(dr["Yxjlmc"]);
                    detail.Bz = ConvertMy.ToString(dr["Bz"]);
                    examDictionaryDetailList.Add(detail);
                }
                return examDictionaryDetailList;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return examDictionaryDetailList;
        }

        #endregion

        #region 获取检查类别
        /// <summary>
        /// 表示获取检查类别的方法
        /// </summary>
        /// <returns>检查类别集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<PE_ExamDictionary> GetExamDictionary()
        {
            List<PE_ExamDictionary> examDictionaryList = new List<PE_ExamDictionary>();
            StringBuilder sb = new StringBuilder();
            try
            {
                sb.Append(@"SELECT Jcbm,Jcmc FROM CP_ExamDictionary");
                DataTable dt = SqlHelper.ExecuteDataTable(ConvertMy.ToString(sb));
                foreach (DataRow dr in dt.Rows)
                {
                    PE_ExamDictionary examDictionary = new PE_ExamDictionary();
                    examDictionary.Jcbm = ConvertMy.ToString(dr["Jcbm"]);
                    examDictionary.Jcmc = ConvertMy.ToString(dr["Jcmc"]);
                    examDictionaryList.Add(examDictionary);
                }
                return examDictionaryList;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return examDictionaryList;
        }
        #endregion

        #region 修改检查项
        /// <summary>
        /// 表示修改检查项的方法
        /// </summary>
        /// <param name="Jcbm">检查项编码</param>
        /// <returns>受影响行数</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal UpdateExamDictionaryDetail(PE_ExamDictionaryDetail examDictionaryDetail, String Jcbm)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Operation","Update"),
                    new SqlParameter("@Jcbm",Jcbm),
                    new SqlParameter("@Flbm",examDictionaryDetail.Flbm),
                    new SqlParameter("@Jcmc",examDictionaryDetail.Jcmc),
                    new SqlParameter("@Mcsx",examDictionaryDetail.Mcsx),
                    new SqlParameter("@Ksfw",examDictionaryDetail.Ksfw),
                    new SqlParameter("@Jsfw",examDictionaryDetail.Jsfw),
                    new SqlParameter("@Jsdw",examDictionaryDetail.Jsdw),
                    new SqlParameter("@Yxjl","1"),
                    new SqlParameter("@Bz",examDictionaryDetail.Bz)
                };
                SqlHelper.ExecuteNoneQuery("usp_CP_ExamDictionaryDetail", parameters, CommandType.StoredProcedure);
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 删除检查项
        /// <summary>
        /// 表示删除检查项的方法
        /// </summary>
        /// <param name="Jcbm">检查项编码</param>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal DeleteExamDictionaryDetail(String Jcbm)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Operation","Delete"),
                    new SqlParameter("@Jcbm",Jcbm),
                    new SqlParameter("@Flbm",""),
                    new SqlParameter("@Jcmc",""),
                    new SqlParameter("@Mcsx",""),
                    new SqlParameter("@Ksfw",""),
                    new SqlParameter("@Jsfw",""),
                    new SqlParameter("@Jsdw",""),
                    new SqlParameter("@Yxjl",""),
                    new SqlParameter("@Bz","")
                };
                SqlHelper.ExecuteNoneQuery("usp_CP_ExamDictionaryDetail", parameters, CommandType.StoredProcedure);
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        #endregion

        #region 验证检查项是否存在
        /// <summary>
        /// 添加时验证检查项目是否重复添加
        /// </summary>
        /// <param name="Flbm">分类编号</param>
        /// <param name="Jcmc">检查名称</param>
        /// <returns>行数</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal CheckAddExamDictionaryDetail(String Flbm, String Jcmc)
        {
            try
            {

                String sqlStr = "SELECT * FROM  CP_ExamDictionaryDetail WHERE Flbm ='" + Flbm + "' AND Jcmc = '" + Jcmc + "'";


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

        /// <summary>
        /// 修改时验证检查项是否重复
        /// </summary>
        /// <param name="ID">编号</param>
        /// <param name="Flbm">分类编号</param>
        /// <param name="Jcmc">检查名称</param>
        /// <returns>受影响行数</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal CheckUpdateExamDictionaryDetail(String ID, String Flbm, String Jcmc)
        {
            try
            {

                String sqlStr = "SELECT * FROM  CP_ExamDictionaryDetail WHERE Flbm ='" + Flbm + "' AND Jcmc = '" + Jcmc + "' AND Jcbm<>'" + ID + "'";


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