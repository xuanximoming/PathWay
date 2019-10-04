using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ServiceModel;
using System.Data.SqlClient;
using System.Data;
using YidanSoft.Tool;
using YidanSoft.Core;
using WebService.Entity;
using System.Text;

namespace Yidansoft.Service
{

    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 根据职工ID获取职工对应多科室列表
        /// </summary>
        /// <param name="userid">用户ID</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<User2Dept> GetUser2DeptList(string userid)
        {
            List<User2Dept> userdeptlist = new List<User2Dept>();
            try
            {
                //DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_Data_Version");
                DataTable dataTable = SqlHelper.ExecuteDataTable(string.Format("select * from user2dept where userid = '{0}'",userid));
                foreach (DataRow dr in dataTable.Rows)
                {
                    User2Dept userdept = new User2Dept();
                    userdept.UserId = dr["UserID"].ToString();
                    userdept.DeptId = dr["DeptID"].ToString();
                    userdept.WardId = dr["WardID"].ToString();
                    userdeptlist.Add(userdept);
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return userdeptlist;
        }

        /// <summary>
        /// 根据传入的职工ID删除该职工多科室信息
        /// </summary>
        /// <param name="userid"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<User2Dept> DelUser2Dept(string userid)
        {
            List<User2Dept> userdeptlist = new List<User2Dept>();
            try
            {
                //DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_Data_Version");
                string sql = string.Format("delete user2Dept where userid = '{0}'", userid);

                SqlHelper.ExecuteNoneQuery(sql);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return userdeptlist;
        }

        /// <summary>
        /// 根据传入的User2Dept列表保存医生多科室信息
        /// </summary>
        /// <param name="userdeptlist">用户多科室列表</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public bool InsertUser2Dept(List<User2Dept> userdeptlist)
        {
            try
            {
                string sql = string.Format("delete user2Dept where userid = '{0}'", userdeptlist[0].UserId);
                SqlHelper.BeginTransaction();
                SqlHelper.ExecuteNoneQuery(sql);


                //DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_Data_Version"); 
                foreach (User2Dept userdept in userdeptlist)
                {
 

                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into user2dept(");
                    strSql.Append("UserID,DeptID,WardID)");
                    strSql.Append(" values (");
                    strSql.Append("@UserID,@DeptID,@WardID)"); 
                    SqlParameter[] parameters = {
					new SqlParameter("@UserID", SqlDbType.VarChar,6),
					new SqlParameter("@DeptID", SqlDbType.VarChar,12),
					new SqlParameter("@WardID", SqlDbType.VarChar,12)};
                    parameters[0].Value = userdept.UserId;
                    parameters[1].Value = userdept.DeptId;
                    parameters[2].Value = userdept.WardId;

                    SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                }
                SqlHelper.CommitTransaction();


            }
            catch (Exception ex)
            {
                SqlHelper.RollbackTransaction();
                ThrowException(ex);
                return false;
            }
            return true;
        }

        /// <summary>
        /// 根据传入的UserID获取员工的角色
        /// </summary>
        /// <param name="UserID">UserID</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<User2Dept> GetUserDeptByUserID(string UserID)
        {
            List<User2Dept> list = new List<User2Dept>();
            try
            {
                string sql = string.Format(@"select dept.ID,dept.Name, 
		                                            isnull(( select 1 from user2dept
			                                            where user2dept.DeptID = dept.ID
			                                               and user2dept.UserID = '{0}'
		                                            ) , '0') ISCheck
                                            from department dept
                                            where exists (select 1 from dept2ward where DeptID = dept.ID)
                                            order by dept.ID", UserID);

                DataTable dt = SqlHelper.ExecuteDataTable(sql);


                foreach (DataRow row in dt.Rows)
                {
                    User2Dept dept = new User2Dept();
                    dept.UserId = UserID;
                    dept.DeptId = row["ID"].ToString();
                    dept.DeptName = row["Name"].ToString();
                    dept.IsCheck = Convert.ToInt32(row["ISCheck"].ToString());
                    list.Add(dept);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }
     
    }
}
