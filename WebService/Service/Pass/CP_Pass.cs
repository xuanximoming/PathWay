using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Yidansoft.Service.Entity;
using System.Data;
using System.ServiceModel;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        [OperationContract]
        public List<DrugInfoPass> GetPassDrugInfo()
        {
            try
            {
                List<DrugInfoPass> drugList = new List<DrugInfoPass>();

                DataTable dt = SqlHelper.ExecuteDataTable("select * from CP_DrugInfo");
                foreach (DataRow row in dt.Rows)
                {
                    DrugInfoPass drug = new DrugInfoPass
                    {
                        Yblb = row["Yplb"].ToString(),
                        Ypdl = row["Ypdl"].ToString(),
                        Ypgg = row["Ypgg"].ToString(),
                        Gflb = row["Gflb"].ToString(),
                        Ypjjz = row["Ypjjz"].ToString(),
                        Ypspm = row["Ypspm"].ToString(),
                        Ypsyz = row["Ypsyz"].ToString(),
                        Yptym = String.IsNullOrEmpty(row["Yptym"].ToString()) ? row["Ypspm"].ToString() : row["Yptym"].ToString(),
                        Ypyf = row["Ypyf"].ToString(),
                        Ypdm = row["ID"].ToString(),
                          Py = row["Py"].ToString()
                        
                    };

                    drugList.Add(drug);
                }

                return drugList;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }
        }




    }
}
