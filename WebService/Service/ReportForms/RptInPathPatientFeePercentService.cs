using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using Yidansoft.Service.Entity.ReportForms;
using System.Data;
using System.Data.SqlClient;
using DrectSoft.Tool;

namespace Yidansoft.Service
{
    /// <summary>
    /// 表示统计结算费用比例的类
    /// </summary>
    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 表示统计结算费用比例的方法
        /// </summary>
        /// <param name="begindate">开始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="Ljzt">路径状态</param>
        /// <param name="ljdm">路径代码</param>
        /// <returns>结算费用比例集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<Rpt_InPathPatientFeePercent> GetRpt_InPathPatientFeePercent(String begindate, String enddate, String Ljzt, String ljdm)
        {
            try
            {
                List<Rpt_InPathPatientFeePercent> feePercentList = new List<Rpt_InPathPatientFeePercent>();//结算费用比例集合
                //DataSet dataSet = new DataSet();
                //SqlConnection connStr = new SqlConnection(m_ConnectionString);
                //SqlCommand cmd = new SqlCommand("usp_CP_InPathPatientFeePercent", connStr);
                //cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@begindate",begindate),
                    new SqlParameter("@enddate",enddate),
                    new SqlParameter("@Ljzt",Ljzt),
                    new SqlParameter("@Ljdm",ljdm)
                };



                DataSet dataSet = SqlHelper.ExecuteDataSet("usp_CP_InPathPatientFeePercent", parameters, CommandType.StoredProcedure);

                Int32 count = dataSet.Tables[0].Rows.Count;
                if (count != 0)
                {
                    for (Int32 i = 0; i < count; i++)
                    {
                        Rpt_InPathPatientFeePercent feePercent = new Rpt_InPathPatientFeePercent();
                        feePercent.Ljmc = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Ljmc"]);
                        feePercent.SyxhID = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["SyxhID"]);
                        feePercent.Hzxm = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Hzxm"]);
                        feePercent.Zyts = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Zyts"]);
                        feePercent.Bzts = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Bzts"]);
                        feePercent.Bzfy = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Bzfy"]);
                        feePercent.Qita = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Qita"]);
                        feePercent.XyFei = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["XyFei"]);
                        feePercent.ZhiliaoFei = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["ZhiliaoFei"]);
                        feePercent.JcFei = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["JcFei"]);
                        feePercent.JyFei = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["JyFei"]);
                        feePercent.ZlFei = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["ZlFei"]);
                        feePercent.CwFei = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["CwFei"]);
                        feePercent.HshlFei = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["HshlFei"]);
                        feePercent.Zj = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Zj"]);
                        feePercent.Yjlj = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Yjlj"]);
                        feePercent.Ljzt = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Ljzt"]);
                        feePercentList.Add(feePercent);
                    }
                }
                return feePercentList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}