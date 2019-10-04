
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
using DrectSoft.Tool;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        
        /// <summary>
        /// 查询医嘱套餐明细
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public    List<CP_ExamDictionaryDetail> GetCP_ExamDictionaryDetailAll()
        {
            List<CP_ExamDictionaryDetail> cplist = new List<CP_ExamDictionaryDetail>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" select * from CP_ExamDictionaryDetail
                where 1=1 and Yxjl=1 ");
               DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
              //  DataTable dt = SqlHelper.ExecuteDataTable(DBConnection.conStr, CommandType.Text, sb.ToString());
                
                foreach (DataRow dr in dt.Rows)
                {
                    CP_ExamDictionaryDetail cp = new CP_ExamDictionaryDetail();
                    cp.Jlxh = ConvertMy.ToString(dr["Jlxh"]);
                    cp.Jcbm = ConvertMy.ToString(dr["Jcbm"]);
                    cp.Flbm = ConvertMy.ToString(dr["Flbm"]);
                    cp.Jcmc = ConvertMy.ToString(dr["Jcmc"]);
                    cp.Mcsx = ConvertMy.ToString(dr["Mcsx"]);
                    cp.Jsfw = ConvertMy.ToString(dr["Jsfw"]);
                    cp.Ksfw = ConvertMy.ToString(dr["Ksfw"]);
                    cp.Syrq = ConvertMy.ToString(dr["Syrq"]);

                    cp.Jsdw = ConvertMy.ToString(dr["Jsdw"]);
                    cp.Py = ConvertMy.ToString(dr["Py"]);
                    cp.Wb = ConvertMy.ToString(dr["Wb"]);
                    cp.Yxjl = ConvertMy.ToString(dr["Yxjl"]);
                    cp.Bz = ConvertMy.ToString(dr["Bz"]);
                 
                    cplist.Add(cp);
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return cplist;


        }
      
    }


}
