using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ServiceModel;
using System.Data.SqlClient;
using System.Data;
using DrectSoft.Tool;
using Yidansoft.Service.Entity;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public CP_NurExecCategoryDetailMaintain GetCP_NurExecCategoryDetailMaintain(String Operation, String Lbxh, String Yxjl, String Sfsy, String Mxxh, String MxxhName, String Create_Time, String Create_User, String Cancel_Time, String Cancel_User, String JgbhList)
        {
            CP_NurExecCategoryDetailMaintain cP_NurExecCategoryDetailMaintain = new CP_NurExecCategoryDetailMaintain();
            List<CP_NurExecCategoryDetailMaintainList> cP_NurExecCategoryDetailMaintainList = new List<CP_NurExecCategoryDetailMaintainList>();

            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@Operation",Operation),
                        new SqlParameter("@Lbxh",Lbxh),
                        new SqlParameter("@Yxjl",Yxjl),
                        new SqlParameter("@Sfsy",Sfsy),
                        new SqlParameter("@Mxxh",Mxxh),
                        new SqlParameter("@MxxhName",MxxhName),
                        new SqlParameter("@Create_Time",Create_Time),
                        new SqlParameter("@Create_User",Create_User),
                        new SqlParameter("@Cancel_Time",Cancel_Time),
                        new SqlParameter("@Cancel_User",Cancel_User),
                        new SqlParameter("@JgbhList",JgbhList)
                    };




                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_NurExecCategoryDetailMaintain", parameters, CommandType.StoredProcedure);

                if (dataTable.Rows.Count == 0)                                              //非常重要
                {
                    cP_NurExecCategoryDetailMaintain.Message = "无数据...";
                    return cP_NurExecCategoryDetailMaintain;
                }

                foreach (DataRow row in dataTable.Rows)
                {
                    CP_NurExecCategoryDetailMaintainList nurExecCategory = new CP_NurExecCategoryDetailMaintainList();

                    if (Operation == "selectList")
                    {
                        nurExecCategory.Lbxh = ConvertMy.ToString(row["Lbxh"]);
                        nurExecCategory.LbxhName = ConvertMy.ToString(row["LbxhName"]);
                    }
                    else
                    {
                        nurExecCategory.LbxhName = ConvertMy.ToString(row["LbxhName"]);
                        nurExecCategory.Mxxh = ConvertMy.ToString(row["Mxxh"]);
                        nurExecCategory.MxxhName = ConvertMy.ToString(row["MxxhName"]);
                        nurExecCategory.Sfsy = ConvertMy.ToString(row["Sfsy"]);
                        nurExecCategory.Yxjl = ConvertMy.ToString(row["Yxjl"]);
                        nurExecCategory.JgName = ConvertMy.ToString(row["JgName"]);
                    }
                    cP_NurExecCategoryDetailMaintainList.Add(nurExecCategory);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }


            cP_NurExecCategoryDetailMaintain.CP_NurExecCategoryDetailMaintainList = cP_NurExecCategoryDetailMaintainList;

            return cP_NurExecCategoryDetailMaintain;
        }


    }
}