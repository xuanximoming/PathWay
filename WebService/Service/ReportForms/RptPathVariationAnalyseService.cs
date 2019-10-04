using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Yidansoft.Service.Entity;
using System.ServiceModel;
using System.Data.SqlClient;
using System.Data;
using YidanSoft.Tool;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Rpt_PathVariationAnalyse GetRptPathVariationAnalyse(String Begintime, String Endtime, String Ljdm, String Dept)
        {
            Rpt_PathVariationAnalyse rpt_PathVariationAnalyse = new Rpt_PathVariationAnalyse();
            List<Rpt_PathVariationAnalyseList> rpt_PathVariationAnalyseList = new List<Rpt_PathVariationAnalyseList>();

            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            try
            {


                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@begindate",Begintime),
                        new SqlParameter("@enddate",Endtime),
                        new SqlParameter("@Ljdm",Ljdm),
                        new SqlParameter("@dept",Dept)
                    };

                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_RptPathVariationAnalyse", parameters, CommandType.StoredProcedure);

                if (dataTable.Rows.Count == 0)                                              //非常重要
                {
                    rpt_PathVariationAnalyse.Message = "无数据...";
                    return rpt_PathVariationAnalyse;
                }

                foreach (DataRow row in dataTable.Rows)
                {
                    Rpt_PathVariationAnalyseList variationAnalyse = new Rpt_PathVariationAnalyseList();

                    variationAnalyse.Bydm = ConvertMy.ToString(row["Bydm"]);
                    variationAnalyse.BymcNd = ConvertMy.ToString(row["BymcNd"]);
                    variationAnalyse.BymcRd = ConvertMy.ToString(row["BymcRd"]);
                    variationAnalyse.BymcSt = ConvertMy.ToString(row["BymcSt"]);
                    variationAnalyse.VariationCount = ConvertMy.ToInt32(row["VariationCount"]);

                    rpt_PathVariationAnalyseList.Add(variationAnalyse);
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }

            rpt_PathVariationAnalyse.PathVariationAnalyseList = rpt_PathVariationAnalyseList;
            return rpt_PathVariationAnalyse;
        }

        [OperationContract]
        [FaultContract(typeof(LoginException))]
        private List<RPT_PathVariationDetail> GetRptPathVariationAnalyseDetail(string Begintime, string Endtime, string Bymc)
        {
            List<RPT_PathVariationDetail> rpt_pathvariationListDetail = new List<RPT_PathVariationDetail>();

            try
            {
                string strSql = string.Format(@"select ip.name 姓名,
cpcp.name 路径,
dia.name 诊断,
us.name 床位医师,
dep.name 科室,war.name 病区 , 
cpvr.bynr 变异内容,
cpvr.byyy 变异原因,
CONVERT(varchar,cast(cpvr.bysj as datetime ),120 ) as 变异时间,
cppd.ljmc 变异路径节点
from CP_VariantRecords cpvr
right join InPatient ip on ip.NoOfInPat=cpvr.syxh
right join CP_ClinicalPath cpcp on cpcp.ljdm=cpvr.ljdm
left join Diagnosis dia on  dia.markid=ip.admitdiagnosis
left join CP_PathDetail cppd on cppd.pahtdetailid=cpvr.pahtdetailid
left join users us on us.id=ip.resident 
left join Department dep on dep.id=ip.admitdept
left join Ward war on war.id=ip.admitward
where cpvr.bydm='{0}'
and CONVERT(varchar,cast(cpvr.bysj as datetime ),120 ) >= '{1}'
and CONVERT(varchar,cast(cpvr.bysj as datetime ),120 ) <= '{2}'", Bymc, Begintime, Endtime);

                DataTable dt = SqlHelper.ExecuteDataTable(strSql);

                foreach (DataRow row in dt.Rows)
                {
                    RPT_PathVariationDetail rpt_pathDetail = new RPT_PathVariationDetail();
                    rpt_pathDetail.Name = row["姓名"].ToString();
                    rpt_pathDetail.PathName = row["路径"].ToString();
                    rpt_pathDetail.Diagnosis = row["诊断"].ToString();
                    rpt_pathDetail.Doctor = row["床位医师"].ToString();
                    rpt_pathDetail.Dept = row["科室"].ToString();
                    rpt_pathDetail.Ward = row["病区"].ToString();
                    rpt_pathDetail.VariationDetail = row["变异内容"].ToString();
                    rpt_pathDetail.VariationReason = row["变异原因"].ToString();
                    rpt_pathDetail.VariationTime = row["变异时间"].ToString();
                    rpt_pathDetail.VariationId = row["变异路径节点"].ToString();
                    rpt_pathvariationListDetail.Add(rpt_pathDetail);
                }


            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }


            return rpt_pathvariationListDetail;
        }

    }
}