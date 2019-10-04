using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
using Yidansoft.Service.Entity.NursingNotes;
using System.Data.SqlClient;
using System.Data;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_VitalSignsTwMbHx> GetNursingNotesVitalSigns(string zyhm, string startDay, string endDay)
        {
            List<CP_VitalSignsTwMbHx> list = new List<CP_VitalSignsTwMbHx>();
            try
            {
                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Zyhm",zyhm),
                new SqlParameter("@StartDay",startDay),
                new SqlParameter("@EndDay",endDay)
            };

                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_VitalSignsTwMbHx", param, CommandType.StoredProcedure);

                foreach (DataRow row in dt.Rows)
                {
                    CP_VitalSignsTwMbHx signs = new CP_VitalSignsTwMbHx();
                    signs.Clrq = row["Clrq"].ToString();
                    signs.Sjd = row["Sjd"].ToString();
                    signs.Hztw = row["Hztw"].ToString();
                    signs.Hzmb = row["Hzmb"].ToString();
                    signs.Hzhx = row["Hzhx"].ToString();
                    list.Add(signs);
                }
            }
            catch (SqlException ex)
            {
                ThrowException(ex);
            }
            return list;
        }
    }
}