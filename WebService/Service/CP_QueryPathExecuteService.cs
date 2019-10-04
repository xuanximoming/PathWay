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
        #region 路径执行查询
        /// <summary>
        /// 路径执行查询
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_QueryPathExecute> GetQueryPathExecuteList(String where)
        {
            List<CP_QueryPathExecute> list = new List<CP_QueryPathExecute>();
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.Append(@"select * from V_QueryPathExecute");
                sql.Append(where);
                DataTable dataTable = new DataTable();
                dataTable = SqlHelper.ExecuteDataTable(sql.ToString());
                foreach (DataRow r in dataTable.Rows)
                {
                    CP_QueryPathExecute p = new CP_QueryPathExecute();

                    p.Syxh = Convert.ToDecimal(r["Syxh"].ToString());
                    p.Brxb = r["brxb"].ToString();
                    p.BrxbName = r["BrxbName"].ToString();
                    p.Hzxm = r["Hzxm"].ToString();
                    p.Jrsj = r["Jrsj"].ToString();
                    p.Ljdm = r["Ljdm"].ToString();
                    p.Ljts = r["Ljts"].ToString();
                    p.LjztName = r["LjztName"].ToString();
                    p.Ljzt = r["Ljzt"].ToString();
                    p.PathName = r["PathName"].ToString();
                    p.Ryrq = r["Ryrq"].ToString();
                    p.Ryzd = r["Ryzd"].ToString();
                    p.RyzdName = r["RyzdName"].ToString();
                    p.Tcsj = r["Tcsj"].ToString();
                    p.Wcsj = r["wcsj"].ToString();
                    p.Xsnl = r["Xsnl"].ToString();
                    p.WorkFlowXML = UnzipContent(r["WorkFlowXML"].ToString());
                    p.EnFroceXml = UnzipContent(r["EnFroceXml"].ToString());
                    list.Add(p);

                }
                return list;

            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return list;
            }
        }

        /// <summary>
        /// 根据路径代码查询路径列表
        /// </summary>
        /// <returns>路径列表</returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_ClinicalPathList> GetClinicalPathListByCondition(string Ljdm)
        {
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            List<CP_ClinicalPathList> clinPathListInfo = new List<CP_ClinicalPathList>();
            try
            {
                //DataTable dataTable = new DataTable();
                StringBuilder sql = new StringBuilder();
                sql.Append(@" SELECT  clin.Ljdm , 
                                    clin.Name , 
                                    clin.Ljms ,   
                                    clin.Zgts , 
                                    clin.Jcfy , 
                                    clin.Vesion , 
                                    clin.Cjsj ,	 
                                    clin.Syks , 
                                    dept.Name AS DeptName ,
                                    clin.Shsj ,	 
                                    clin.Shys , 
                                    emp.NAME AS ShysName ,
                                    CASE clin.Yxjl
                                      WHEN 0 THEN '无效'
                                      WHEN 1 THEN '有效'
                                      WHEN 2 THEN '停止'
                                    END AS Yxjl ,
                                    clin.Yxjl AS YxjlId ,
                                    clin.WorkFlowXML
                            FROM    CP_ClinicalPath clin
                                    JOIN Department dept ON clin.Syks = dept.ID
                                                               AND dept.Valid = 1
                                    LEFT JOIN Users emp ON clin.Shys = emp.ID
                            WHERE   clin.Yxjl IN ( 1, 2 )"
                    );//--是否有效(0、无效1有效.2停止)

                sql.AppendFormat(" and clin.Ljdm={0}", Ljdm);


                DataTable dataTable = SqlHelper.ExecuteDataTable(sql.ToString());

                foreach (DataRow row in dataTable.Rows)
                {
                    CP_ClinicalPathList cliListInfo = new CP_ClinicalPathList(row["Ljdm"].ToString(), row["Name"].ToString(), row["Ljms"].ToString(),
                        decimal.Parse(row["Zgts"].ToString()), decimal.Parse(row["Jcfy"].ToString()), decimal.Parse(row["Vesion"].ToString()),
                        row["Cjsj"].ToString(), row["Shsj"].ToString(), row["Syks"].ToString(), row["Yxjl"].ToString(),
                        row["Syks"].ToString(), row["DeptName"].ToString(), row["ShysName"].ToString(), int.Parse(row["YxjlId"].ToString()),
                        UnzipContent(row["WorkFlowXML"].ToString()));
                    clinPathListInfo.Add(cliListInfo);
                }

                return clinPathListInfo;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return clinPathListInfo;
            }
            //}
        }

        /// <summary>
        /// 节点医嘱执行对照表
        /// </summary>
        /// <returns>节点GUID</returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_QueryPathExecuteNoteCompare> GetQueryPathExecuteNoteCompare(string NoteGUID)
        {

            try
            {
                DataTable dataTable = new DataTable();
                SqlParameter[] para = new SqlParameter[1];
                para[0] = new SqlParameter("@NoteGUID", NoteGUID);
                dataTable = SqlHelper.ExecuteDataTable("usp_CP_QueryPathExecuteNoteCompare", para, CommandType.StoredProcedure);
                List<CP_QueryPathExecuteNoteCompare> NoteCompareList = new List<CP_QueryPathExecuteNoteCompare>();
                foreach (DataRow row in dataTable.Rows)
                {
                    CP_QueryPathExecuteNoteCompare NoteCompare = new CP_QueryPathExecuteNoteCompare();
                    NoteCompare.YzbzName = row["YzbzName"].ToString();
                    NoteCompare.Yznr = row["Yznr"].ToString();
                    NoteCompare.Ztnr = row["Ztnr"].ToString();
                    NoteCompare.Flag = row["Flag"].ToString();
                    NoteCompare.Ypmc = row["Ypmc"].ToString();
                    NoteCompare.IsNew = row["IsNew"].ToString();
                    NoteCompare.EmployeeName = row["EmployeeName"].ToString();
                    NoteCompare.EmployeeLrrq = row["EmployeeLrrq"].ToString();
                    NoteCompare.Zxrq = row["Zxrq"].ToString();


                    NoteCompareList.Add(NoteCompare);
                }

                return NoteCompareList;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }
        }
        #endregion
    }
}