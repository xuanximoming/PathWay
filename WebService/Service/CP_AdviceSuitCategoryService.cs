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
        /// 添加验证
        /// </summary>
        /// <param name="Name">分类名称</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal InsertCheckInfo(String Name)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@sqlType","InsertCheckInfo"),
                    new SqlParameter("@Name",Name)
                };
                //usp_CP_AdviceSuitCategoryManage laolaowhn NeedFix 存储过程名字太长，删掉了usp_ 
                DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_AdviceSuitCategoryManage", parameters, CommandType.StoredProcedure);
                return ds.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 获取全部数据，加载到显示当中
        /// 创建：2013年7月23日 18:08:50
        /// 创建人：Jhonny
        /// </summary>
        /// <sql>查询语句</sql>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_AdviceSuitCategory> GetList(string sql)
        {
            try
            {
                List<CP_AdviceSuitCategory> categoryList = new List<CP_AdviceSuitCategory>();
                using (DataSet ds = SqlHelper.ExecuteDataSet(sql))
                {
                    if (ds == null || ds.Tables[0].Rows.Count < 0)
                    {
                        return null;
                    }
                    DataTable dt = ds.Tables[0];
                    foreach (DataRow dr in dt.Rows)
                    {
                        categoryList.Add(new CP_AdviceSuitCategory() { CategoryId = dr["CategoryId"].ToString(), ParentID = dr["ParentID"].ToString(), Zgdm = dr["Zgdm"].ToString(), Memo = dr["Memo"].ToString(), Name = dr["Name"].ToString(), Yxjl = dr["Yxjl"].ToString() });
                    }
                }
                return categoryList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 添加查询医嘱套餐分类
        /// </summary>
        /// <param name="adviceSuitCategory">医嘱套餐分类实体</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_AdviceSuitCategory> InsertAndSelectCP_AdviceSuitCategory(CP_AdviceSuitCategory adviceSuitCategory, String where)
        {
            List<CP_AdviceSuitCategory> categoryList = new List<CP_AdviceSuitCategory>();
            List<CP_AdviceSuit> adviceSuitList = new List<CP_AdviceSuit>();
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@sqlType","InsertAndSelect"),
                    new SqlParameter("@CategoryId",adviceSuitCategory.CategoryId),
                    new SqlParameter("@Name",adviceSuitCategory.Name),
                    new SqlParameter("@Zgdm",adviceSuitCategory.Zgdm),
                    new SqlParameter("@Yxjl","1"),
                    new SqlParameter("@ParentID",adviceSuitCategory.ParentID),
                    new SqlParameter("@Memo",adviceSuitCategory.Memo),
                };
                //usp_CP_AdviceSuitCategoryManage laolaowhn NeedFix 存储过程名字太长，删掉了usp_ 
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_AdviceSuitCategoryManage", parameters, CommandType.StoredProcedure);
                if (where != String.Empty)
                {
                    adviceSuitList = GetCP_AdviceSuit(where);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    CP_AdviceSuitCategory AdviceSuitCategory = new CP_AdviceSuitCategory();
                    AdviceSuitCategory.CategoryId = ConvertMy.ToString(dr["CategoryId"]);
                    AdviceSuitCategory.Name = ConvertMy.ToString(dr["Name"]);
                    AdviceSuitCategory.ParentID = ConvertMy.ToString(dr["ParentID"]);
                    AdviceSuitCategory.Py = ConvertMy.ToString(dr["Py"]);
                    AdviceSuitCategory.Wb = ConvertMy.ToString(dr["Wb"]);
                    AdviceSuitCategory.Zgdm = ConvertMy.ToString(dr["Zgdm"]);
                    AdviceSuitCategory.AddTime = ConvertMy.ToString(dr["AddTime"]);
                    AdviceSuitCategory.Memo = ConvertMy.ToString(dr["Memo"]);
                    AdviceSuitCategory.ParentName = ConvertMy.ToString(dr["ParentName"]);
                    if (adviceSuitList != null)
                    {
                        List<CP_AdviceSuit> newList = new List<CP_AdviceSuit>();
                        foreach (CP_AdviceSuit item in adviceSuitList)
                        {
                            if (item.CategoryId == AdviceSuitCategory.CategoryId)
                            {
                                newList.Add(item);
                                AdviceSuitCategory.AdviceSuitList = newList;
                            }
                        }
                    }
                    categoryList.Add(AdviceSuitCategory);
                }

                return categoryList;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return categoryList;
        }


        /// <summary>
        /// 修改验证
        /// </summary>
        /// <param name="Name">医嘱套餐类别名称</param>
        /// <param name="CategoryId">医嘱套餐类别编号</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal UpdateCheckInfo(String Name, String CategoryId)
        {
            try
            {
                //原先的代码注释   2013年8月14日 15:30:29  Jhonny
                //SqlParameter[] parameters = new SqlParameter[] 
                //{ 
                //    new SqlParameter("@sqlType","UpdateCheckInfo"),
                //    new SqlParameter("@Name",Name),
                //    new SqlParameter("@CategoryId",CategoryId)
                //};
                ////usp_CP_AdviceSuitCategoryManage laolaowhn NeedFix 存储过程名字太长，删掉了usp_ 
                //DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_AdviceSuitCategoryManage", parameters, CommandType.StoredProcedure);
                //return ds.Tables[0].Rows.Count;

                //用新方法代替原有的存储过程
                //修改时间：2013年8月14日 15:11:52
                //修改人：Jhonny
                string sql = string.Format("SELECT COUNT(NAME) FROM CP_AdviceSuitCategory WHERE Name='{0}'", Name);
                int count = Convert.ToInt32(SqlHelper.ExecuteScalar(sql));
                return count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改医嘱套餐分类
        /// </summary>
        /// <param name="adviceSuitCategory">医嘱套餐分类实体</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_AdviceSuitCategory> UpdateCP_AdviceSuitCategory(CP_AdviceSuitCategory adviceSuitCategory)
        {
            string sql="SELECT * FROM CP_AdviceSuitCategory WHERE 1=1";
            List<CP_AdviceSuitCategory> categoryList = new List<CP_AdviceSuitCategory>();
            List<CP_AdviceSuit> adviceSuitList = new List<CP_AdviceSuit>();
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@sqlType","Update"),
                    new SqlParameter("@CategoryId",adviceSuitCategory.CategoryId),
                    new SqlParameter("@Name",adviceSuitCategory.Name),
                    new SqlParameter("@Zgdm",adviceSuitCategory.Zgdm),
                    new SqlParameter("@Yxjl",adviceSuitCategory.Yxjl),
                     new SqlParameter("@ParentID",adviceSuitCategory.ParentID),
                    new SqlParameter("@Memo",adviceSuitCategory.Memo),
                };
                //
                //usp_CP_AdviceSuitCategoryManage laolaowhn NeedFix 存储过程名字太长，删掉了usp_ 
                SqlHelper.ExecuteDataTable("usp_CP_AdviceSuitCategoryManage", parameters, CommandType.StoredProcedure);

                return GetList(sql);
                //return 1;
            }
            catch (Exception ex) 
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除前验证该分类是否正在使用
        /// </summary>
        /// <param name="CategoryId">套分类编号</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal DeleteCheckInfo(String CategoryId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@sqlType","DeleteCheckInfo"),
                    new SqlParameter("@CategoryId",CategoryId)
                };
                //usp_CP_AdviceSuitCategoryManage laolaowhn NeedFix 存储过程名字太长，删掉了usp_ 
                DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_AdviceSuitCategoryManage", parameters, CommandType.StoredProcedure);
                return ds.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除医嘱套餐类别
        /// </summary>
        /// <param name="CategoryId">医嘱套餐类别编号</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal DeleteCP_AdviceSuitCategory(String CategoryId)
        {
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@sqlType","Delete"),
                    new SqlParameter("@CategoryId",CategoryId)
                };
                SqlHelper.ExecuteNoneQuery("usp_CP_AdviceSuitCategoryManage", parameters, CommandType.StoredProcedure);
                return 1;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}