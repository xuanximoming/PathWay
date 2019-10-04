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
        /// <summary>
        /// 查询进入路径的的病人
        /// </summary>
        /// <param name="ID">DI字段</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public CP_InPathPatientList GetCP_InPathPatient(String ID)
        {
            CP_InPathPatientList p = null;
            try
            {
                StringBuilder sql = new StringBuilder();
                sql.AppendFormat(@"select * from CP_InPathPatient where id={0}", ID);
                DataTable dataTable = new DataTable();
                dataTable = SqlHelper.ExecuteDataTable( sql.ToString());
              
                if (dataTable != null && dataTable.Rows.Count > 0)
                {
                    p = new CP_InPathPatientList();
                    if (Convert.IsDBNull(dataTable.Rows[0]["Syxh"]))    p.Syxh = System.Data.SqlTypes.SqlString.Null; else p.Syxh = DrectSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Syxh"]);
                    if (Convert.IsDBNull(dataTable.Rows[0]["Cwys"]))    p.Cwys = System.Data.SqlTypes.SqlString.Null; else p.Syxh = DrectSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Cwys"]);
                    if (Convert.IsDBNull(dataTable.Rows[0]["Id"]))      p.Id = System.Data.SqlTypes.SqlString.Null; else p.Syxh = DrectSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Id"]);
                    if (Convert.IsDBNull(dataTable.Rows[0]["Hissyxh"])) p.Hissyxh = System.Data.SqlTypes.SqlString.Null; else p.Syxh = DrectSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Hissyxh"]);
                    if (Convert.IsDBNull(dataTable.Rows[0]["Jrsj"]))    p.Jrsj = System.Data.SqlTypes.SqlString.Null; else p.Syxh = DrectSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Jrsj"]);

                    if (Convert.IsDBNull(dataTable.Rows[0]["Ljdm"]))    p.Ljdm = System.Data.SqlTypes.SqlString.Null; else p.Syxh = DrectSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Ljdm"]);
                    if (Convert.IsDBNull(dataTable.Rows[0]["Ljts"]))    p.Ljts = System.Data.SqlTypes.SqlString.Null; else p.Syxh = DrectSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Ljts"]);
                    if (Convert.IsDBNull(dataTable.Rows[0]["Ljzt"]))    p.Ljzt = System.Data.SqlTypes.SqlString.Null; else p.Syxh = DrectSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Ljzt"]);
                    if (Convert.IsDBNull(dataTable.Rows[0]["Tcsj"]))    p.Tcsj = System.Data.SqlTypes.SqlString.Null; else p.Syxh = DrectSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Tcsj"]);
                    if (Convert.IsDBNull(dataTable.Rows[0]["Wcsj"]))    p.Wcsj = System.Data.SqlTypes.SqlString.Null; else p.Syxh = DrectSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Wcsj"]);
                }
                return p;

            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return p;
            }
        }


    }
}