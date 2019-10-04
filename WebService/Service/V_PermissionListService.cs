using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Data.SqlClient;
using System.Data;
using Yidansoft.Service.Entity;
using System.Collections;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Configuration;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        #region 权限控制
        /// <summary>
        /// 获取叶子菜单
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<V_PermissionList> GetV_PermissionList(string where)
        {
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
                List<V_PermissionList> List = new List<V_PermissionList>();
                try
                {
                    //DataTable dataTable = new DataTable();
                    StringBuilder sql = new StringBuilder();
                    #region sql
                    sql.Append(@"select distinct FunCode,FunCodeFather,FunFatherName,FunName,FunURL from V_Permission where 1=1 and FunCode is not null");
                    sql.Append(where.ToString());
                    #endregion


                    DataTable dataTable = SqlHelper.ExecuteDataTable(sql.ToString());

                    foreach (DataRow row in dataTable.Rows)
                    {
                        V_PermissionList m = new V_PermissionList();
                        m.FunCodeFather = row["FunCodeFather"].ToString();
                        m.FunFatherName = row["FunFatherName"].ToString();
                        m.FunCode = row["FunCode"].ToString();
                        m.FunName = row["FunName"].ToString();
                        m.FunURL = row["FunURL"].ToString();

                        List.Add(m);
                    }

                    return List;
                }
                catch (Exception ex)
                {
                    ThrowException(ex);
                    return List;
                }
            //}
        }

        /// <summary>
        /// 获取菜单
        /// </summary>
        /// modified by zhouhui 此处参数改为按照职工代码
        /// 客户端无法探知视图的内部的字段
        /// 如视图的字段调整以后，客户端必然报错
        /// </summary>
        /// <param name="userid">职工代码</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<V_PermissionListFather> GetV_PermissionListFather(string userid)
        {
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            List<V_PermissionListFather> List = new List<V_PermissionListFather>();

            try
            {
                //DataTable dataTable = new DataTable();
                StringBuilder sql = new StringBuilder();
                #region sql

                sql.Append(@"select distinct FunCodeFather, FunFatherName,FunURLFather from V_Permission where UserID='" + userid + "' and   FunCodeFather is not null order by FunCodeFather asc");
                //sql.Append(where.ToString());
                #endregion


                DataTable dataTable = SqlHelper.ExecuteDataTable(sql.ToString());

                foreach (DataRow row in dataTable.Rows)
                {
                    V_PermissionListFather m = new V_PermissionListFather();
                    m.FunCodeFather = row["FunCodeFather"].ToString();
                    m.FunFatherName = row["FunFatherName"].ToString();
                    m.FunURLFather = row["FunURLFather"].ToString();

                    m.pList = GetV_PermissionList(" and UserID='" + userid + "' and FunCodeFather= " + m.FunCodeFather);
                    List.Add(m);
                }

                return List;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return List;
            }
            //}
        }
        #endregion
    }
}