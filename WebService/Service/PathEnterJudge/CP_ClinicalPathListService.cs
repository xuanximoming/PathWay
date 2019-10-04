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
using YidanSoft.Tool;
namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 获取审核过的路径列表
        /// </summary>
        /// <returns>路径列表</returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_ClinicalPathList> GetValidClinicalPathList()
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
                            WHERE   clin.Yxjl=3 "
                        );//(0、无效1有效.2停止)

                    DataTable dataTable = YidanEHRDataService.SqlHelper.ExecuteDataTable( sql.ToString());

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
    }
}
