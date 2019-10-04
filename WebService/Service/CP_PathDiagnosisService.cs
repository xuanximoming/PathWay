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
using DrectSoft.Tool;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 根据输入的查询关键字，获取路径诊断库列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_Diagnosis_E> GetCP_PathDiagnosisListByKey(string keywords)
        {
            List<CP_Diagnosis_E> list = new List<CP_Diagnosis_E>();
            try
            {
                string sql = string.Format(@"SELECT diag.Zdbs,Zddm,Ysdm,Bzdm,Name,Py,Wb,Zldm,Tjm,Nbfl,Bzlb,Qtlb,Yxjl,Memo FROM CP_PathDiagnosis diag 
	                                            WHERE (
		                                                '{0}' = '' 
		                                            OR diag.Zdbs LIKE '{0}' 
		                                            OR diag.Zddm LIKE '{0}' 
		                                            OR diag.Name LIKE '{0}' 
		                                            OR diag.Py LIKE '{0}' 
		                                            OR diag.Wb LIKE '{0}')
                                                    and Yxjl = 1", keywords);

                DataTable dt = SqlHelper.ExecuteDataTable( sql);


                foreach (DataRow row in dt.Rows)
                {
                    CP_Diagnosis_E role = new CP_Diagnosis_E();
                    role.Zdbs = row["Zdbs"].ToString();
                    role.Zddm = row["Zddm"].ToString();
                    role.Ysdm = row["Ysdm"].ToString();

                    role.Bzdm = row["Bzdm"].ToString();
                    role.Name = row["Name"].ToString();
                    role.Py = row["Py"].ToString();

                    role.Wb = row["Wb"].ToString();
                    role.Zldm = row["Zldm"].ToString();
                    role.Tjm = row["Tjm"].ToString();

                    role.Nbfl = row["Nbfl"].ToString();
                    role.Bzlb = row["Bzlb"].ToString();
                    role.Qtlb = row["Qtlb"].ToString(); ;

                    role.Yxjl = row["Yxjl"].ToString(); ;
                    role.Memo = row["Memo"].ToString();
                    list.Add(role);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;

        }

        /// <summary>
        /// 根据输入的查询关键字，获取路径诊断库列表
        /// </summary>
        /// <param name="keywords">关键字</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_Diagnosis_E> GetCP_PathDiagnosisListAll()
        {
            List<CP_Diagnosis_E> list = new List<CP_Diagnosis_E>();
            try
            {
                string sql = @"SELECT diag.Zdbs,Zddm,Ysdm,Bzdm,Name,Py,Wb,Zldm,Tjm,Nbfl,Bzlb,Qtlb,Yxjl,Memo FROM CP_PathDiagnosis diag 
	                                            WHERE Yxjl = 1";
                DataTable dt = SqlHelper.ExecuteDataTable( sql);
                foreach (DataRow row in dt.Rows)
                {
                    CP_Diagnosis_E role = new CP_Diagnosis_E();
                    role.Zdbs = row["Zdbs"].ToString();
                    role.Zddm = row["Zddm"].ToString();
                    role.Ysdm = row["Ysdm"].ToString();

                    role.Bzdm = row["Bzdm"].ToString();
                    role.Name = row["Name"].ToString();
                    role.Py = row["Py"].ToString();

                    role.Wb = row["Wb"].ToString();
                    role.Zldm = row["Zldm"].ToString();
                    role.Tjm = row["Tjm"].ToString();

                    role.Nbfl = row["Nbfl"].ToString();
                    role.Bzlb = row["Bzlb"].ToString();
                    role.Qtlb = row["Qtlb"].ToString(); ;

                    role.Yxjl = row["Yxjl"].ToString(); ;
                    role.Memo = row["Memo"].ToString();
                    list.Add(role);
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