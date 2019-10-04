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
        /// <param name="ISTestUse">测试代码调用时为True，其他为False</param>
        /// <param name="EHRConStr">测试代码调用时需要赋值，其他调用赋NULL</param>
        /// <param name="EMRConStr">测试代码调用时需要赋值，其他调用赋NULL</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Boolean EMRInsertCP_InPathPatient(String ID, Boolean IsTestUse, String TestUseEHRConStr, String TestUseEMRConStr)
        {
            String EHRCon = TestUseEHRConStr;
            String EMRCon = TestUseEMRConStr;
            if (!IsTestUse)
            {
                EHRCon = m_ConnectionString;
                EMRCon = m_ConnectionStringEMR;
            }

            CP_InPathPatientList p = null;
            Object[] obj = new Object[10];
            //try
            //{
            #region EHR
            StringBuilder sql = new StringBuilder();
            sql.AppendFormat(@"select * from CP_InPathPatient where id={0}", ID);
            DataTable dataTable = new DataTable();
            dataTable = SqlHelper.ExecuteDataTable(sql.ToString());

            if (dataTable != null && dataTable.Rows.Count > 0)
            {
                p = new CP_InPathPatientList();
                if (Convert.IsDBNull(dataTable.Rows[0]["Id"])) obj[0] = System.Data.SqlTypes.SqlString.Null; else obj[0] = "'" + YidanSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["ID"]) + "'";
                if (Convert.IsDBNull(dataTable.Rows[0]["Syxh"])) obj[1] = System.Data.SqlTypes.SqlString.Null; else obj[1] = "'" + YidanSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Syxh"]) + "'";
                if (Convert.IsDBNull(dataTable.Rows[0]["Cwys"])) obj[2] = System.Data.SqlTypes.SqlString.Null; else obj[2] = "'" + YidanSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Cwys"]) + "'";
                if (Convert.IsDBNull(dataTable.Rows[0]["Jrsj"])) obj[3] = System.Data.SqlTypes.SqlString.Null; else obj[3] = "'" + YidanSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Jrsj"]) + "'";

                if (Convert.IsDBNull(dataTable.Rows[0]["Ljdm"])) obj[4] = System.Data.SqlTypes.SqlString.Null; else obj[4] = "'" + YidanSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Ljdm"]) + "'";
                if (Convert.IsDBNull(dataTable.Rows[0]["Ljts"])) obj[5] = System.Data.SqlTypes.SqlString.Null; else obj[5] = "'" + YidanSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Ljts"]) + "'";
                if (Convert.IsDBNull(dataTable.Rows[0]["Ljzt"])) obj[6] = System.Data.SqlTypes.SqlString.Null; else obj[6] = "'" + YidanSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Ljzt"]) + "'";
                if (Convert.IsDBNull(dataTable.Rows[0]["Tcsj"])) obj[7] = System.Data.SqlTypes.SqlString.Null; else obj[7] = "'" + YidanSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Tcsj"]) + "'";
                if (Convert.IsDBNull(dataTable.Rows[0]["Wcsj"])) obj[8] = System.Data.SqlTypes.SqlString.Null; else obj[8] = "'" + YidanSoft.Tool.ConvertMy.ToString(dataTable.Rows[0]["Wcsj"]) + "'";



                String Hissyxh = "";
                DataTable dt = SqlHelper.ExecuteDataTable(String.Format(@"SELECT PatNoOfHis from InPatient where  NoOfInpat={0}", obj[1]));
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != null)
                {
                    Hissyxh = dt.Rows[0][0].ToString().Trim();
                }


                if (!String.IsNullOrEmpty(Hissyxh))
                    obj[9] = Hissyxh;
            }

            #endregion

            return false;

        }



    }
}