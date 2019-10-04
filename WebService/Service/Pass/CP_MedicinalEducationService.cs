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

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_MedicinalEducation> GetMedicinalEducationInfo()
        {
            try
            {
                string sqlcmd = "SELECT * FROM CP_DrugEdcationManual";

                List<CP_MedicinalEducation> list = new List<CP_MedicinalEducation>();
                DataTable dt = SqlHelper.ExecuteDataTable( sqlcmd);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new CP_MedicinalEducation
                    {
                        Wtbh = dt.Rows[i][0].ToString(),
                        Wtlb = dt.Rows[i][1].ToString(),
                        Wtnr = dt.Rows[i][2].ToString(),
                        Wtda = dt.Rows[i][3].ToString(),

                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }
        }
    }
}