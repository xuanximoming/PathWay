using DrectSoft.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;
using Yidansoft.Service.Entity;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 获取护理执行项目表
        /// </summary>
        /// <returns>护理执行项目表集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_NurExecItem> GetNurExecItemList()
        {
            List<CP_NurExecItem> nurExecItemList = new List<CP_NurExecItem>();
            try
            {
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_GetNurExecItem");
                foreach (DataRow dr in dt.Rows)
                {
                    CP_NurExecItem nurExecItem = new CP_NurExecItem();
                    nurExecItem.Xmxh = ConvertMy.ToString(dr["Xmxh"]);
                    nurExecItem.Name = ConvertMy.ToString(dr["Name"]);
                    nurExecItemList.Add(nurExecItem);
                }
                return nurExecItemList;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return nurExecItemList;
        }

        /// <summary>
        /// 添加时验证分类名称是否重复
        /// </summary>
        /// <param name="Name">分类名称</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal CheckNurExecCategoryInsert(String Name)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Lbxh",String.Empty),
                    new SqlParameter("@Name",Name),
                    new SqlParameter("@Xmxh",String.Empty),
                    new SqlParameter("@Yxjl",String.Empty),
                    new SqlParameter("@Create_User",String.Empty),
                    new SqlParameter("@Cancel_User",String.Empty),
                    new SqlParameter("@sqlType","InsertCheckInfo")
                };
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_NurExecCategoryManage", parameters, CommandType.StoredProcedure);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return -1;
        }

        /// <summary>
        /// 修改时验证分类名称是否重复
        /// </summary>
        /// <param name="Name">分类名称</param>
        /// <param name="lbxh">分类序号</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal CheckNurExecCategoryUpdate(String Name, String lbxh)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Lbxh",lbxh),
                    new SqlParameter("@Name",Name),
                    new SqlParameter("@Xmxh",String.Empty),
                    new SqlParameter("@Yxjl",String.Empty),
                    new SqlParameter("@Create_User",String.Empty),
                    new SqlParameter("@Cancel_User",String.Empty),
                    new SqlParameter("@sqlType","UpdateCheckInfo")
                };
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_NurExecCategoryManage", parameters, CommandType.StoredProcedure);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return -1;
        }

        /// <summary>
        /// 添加查询护理执行类别表
        /// </summary>
        /// <param name="cpnec">护理执行类别表实体</param>
        /// <returns>护理执行类别表集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_NurExecCategory> InsertAndSelectNurExecCategory(CP_NurExecCategory cpnec)
        {
            List<CP_NurExecCategory> nurExecCategoryList = new List<CP_NurExecCategory>();
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Lbxh",cpnec.Lbxh),
                    new SqlParameter("@Name",cpnec.Name),
                    new SqlParameter("@Xmxh",cpnec.Xmxh),
                    new SqlParameter("@Yxjl",cpnec.Yxjl),
                    new SqlParameter("@Create_User",cpnec.Create_User),
                    new SqlParameter("@Cancel_User",cpnec.Cancel_User),
                    new SqlParameter("@sqlType","InsertAndSelect")
                };
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_NurExecCategoryManage", parameters, CommandType.StoredProcedure);
                foreach (DataRow dr in dt.Rows)
                {
                    CP_NurExecCategory nurExecCategory = new CP_NurExecCategory();
                    nurExecCategory.Lbxh = ConvertMy.ToString(dr["Lbxh"]);
                    nurExecCategory.Name = ConvertMy.ToString(dr["Name"]);
                    nurExecCategory.Xmxh = ConvertMy.ToString(dr["Xmxh"]);
                    nurExecCategory.XmxhName = ConvertMy.ToString(dr["XmxhName"]);
                    nurExecCategory.InputType = ConvertMy.ToString(dr["InputType"]);
                    nurExecCategory.OrderValue = ConvertMy.ToString(dr["OrderValue"]);
                    nurExecCategory.Yxjl = ConvertMy.ToString(dr["Yxjl"]);
                    nurExecCategory.Yxjlmc = ConvertMy.ToString(dr["Yxjlmc"]);
                    nurExecCategory.Create_Time = ConvertMy.ToString(dr["Create_Time"]);
                    nurExecCategory.Create_User = ConvertMy.ToString(dr["Create_User"]);
                    nurExecCategory.Cancel_Time = ConvertMy.ToString(dr["Cancel_Time"]);
                    nurExecCategory.Cancel_User = ConvertMy.ToString(dr["Cancel_User"]);
                    nurExecCategoryList.Add(nurExecCategory);
                }
                return nurExecCategoryList;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return nurExecCategoryList;
        }

        /// <summary>
        /// 修改护理执行类别表
        /// </summary>
        /// <param name="cpnec">护理执行类别表</param>
        /// <returns>结果集</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal UpdateNurExecCategory(CP_NurExecCategory cpnec)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Lbxh",cpnec.Lbxh),
                    new SqlParameter("@Name",cpnec.Name),
                    new SqlParameter("@Xmxh",cpnec.Xmxh),
                    new SqlParameter("@Yxjl",cpnec.Yxjl),
                    new SqlParameter("@Create_User",String.Empty),
                    new SqlParameter("@Cancel_User",cpnec.Cancel_User),
                    new SqlParameter("@sqlType","Update")
                };
                SqlHelper.ExecuteNoneQuery("usp_CP_NurExecCategoryManage", parameters, CommandType.StoredProcedure);

                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除护理类别信息前验证该项
        /// </summary>
        /// <param name="Lbxh">类别序号</param>
        /// <returns>结果集</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal CheckNurExecCategoryDelete(String Lbxh)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Lbxh",Lbxh),
                    new SqlParameter("@Name",String.Empty),
                    new SqlParameter("@Xmxh",String.Empty),
                    new SqlParameter("@Yxjl",String.Empty),
                    new SqlParameter("@Create_User",String.Empty),
                    new SqlParameter("@Cancel_User",String.Empty),
                    new SqlParameter("@sqlType","DeleteCheckInfo")
                };
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_NurExecCategoryManage", parameters, CommandType.StoredProcedure);
                return dt.Rows.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除护理类别信息
        /// </summary>
        /// <param name="Lbxh">类别编码</param>
        /// <returns>受影响行数</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal DeleteNurExecCategory(String Lbxh)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Lbxh",Lbxh),
                    new SqlParameter("@Name",String.Empty),
                    new SqlParameter("@Xmxh",String.Empty),
                    new SqlParameter("@Yxjl",String.Empty),
                    new SqlParameter("@Create_User",String.Empty),
                    new SqlParameter("@Cancel_User",String.Empty),
                    new SqlParameter("@sqlType","Delete")
                };
                SqlHelper.ExecuteNoneQuery("usp_CP_NurExecCategoryManage", parameters, CommandType.StoredProcedure);
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}