using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ServiceModel;
using System.Data.SqlClient;
using System.Data;
using DrectSoft.Tool;

namespace Yidansoft.Service
{

    public partial class YidanEHRDataService
    {
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public CP_PathCutTotal PathCutTotal(String ljmc, String dept)
        {
            CP_PathCutTotal total = new CP_PathCutTotal();
            List<CP_PathCutQuery> list = new List<CP_PathCutQuery>();
            List<IDictionary<String, Object>> hashObjList = new List<IDictionary<String, Object>>();


            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@ljmc",ljmc),
                        new SqlParameter("@dept",dept)
                    };



                DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_PathCutQuery", parameters, CommandType.StoredProcedure);

                if (ds.Tables.Count > 0)            //很重要,是否存在
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        CP_PathCutQuery pathcut = new CP_PathCutQuery();

                        pathcut.Bydm = ConvertMy.ToString(item["Bydm"]);
                        pathcut.Bymc = ConvertMy.ToString(item["Bymc"]);

                        list.Add(pathcut);
                    }
                }

                if (ds.Tables.Count > 1)            //很重要,是否存在
                {
                    for (int i = 0; i < ds.Tables[1].Rows.Count; i++)
                    {
                        Dictionary<String, Object> hashObj = new Dictionary<String, Object>();
                        for (int j = 0; j < ds.Tables[1].Columns.Count; j++)
                        {
                            String colName = ConvertMy.ToString(ds.Tables[1].Columns[j].ColumnName);
                            hashObj.Add(colName, ConvertMy.ToString(ds.Tables[1].Rows[i][colName]));         //如果数据中有NULL的，则无法被序列化
                        }
                        hashObjList.Add(hashObj);
                    }
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }


            total.pathCutQuery = list;
            total.hashObjList = hashObjList;
            return total;
            // }

        }
    }
}