using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ServiceModel;
using System.Data.SqlClient;
using System.Data;
using YidanSoft.Tool;
using YidanSoft.Core;

namespace Yidansoft.Service
{

    public partial class YidanEHRDataService
    {
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public CVersion GetVersion()
        {
            CVersion version = new CVersion();
            try
            {
                //DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_Data_Version");
                DataTable dataTable = SqlHelper.ExecuteDataTable("SELECT * FROM CP_Data_Version order by ID desc");                
                if (dataTable != null && dataTable.Rows.Count > 0)            //很重要,是否存在
                {
                    version.ID = ConvertMy.ToString(dataTable.Rows[0]["ID"]);
                    version.VersionID = ConvertMy.ToString(dataTable.Rows[0]["VersionID"]);
                    version.Version = ConvertMy.ToString(dataTable.Rows[0]["Version"]);
                    version.Create_time = ConvertMy.ToString(dataTable.Rows[0]["Create_time"]);
                    version.HosName = ConvertMy.ToString(dataTable.Rows[0]["HosName"]);
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return version;
        }

        public CVersion GetTargetVersion(string hostName)
        {
            CVersion version = new CVersion();
            try
            {
                DataTable table = SqlHelper.ExecuteDataTable("select * from CP_Data_Version" +
                    " where HosName =" + "'" + hostName + "'");
                if (table != null)            //很重要,是否存在
                {
                    foreach (DataRow item in table.Rows)
                    {
                        version.ID = ConvertMy.ToString(item["ID"]);
                        version.VersionID = ConvertMy.ToString(item["VersionID"]);
                        version.Version = ConvertMy.ToString(item["Version"]);
                        version.Create_time = ConvertMy.ToString(item["Create_time"]);
                        version.HosName = ConvertMy.ToString(item["HosName"]);
                        break;
                    }
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return version;
        }
    }
}
