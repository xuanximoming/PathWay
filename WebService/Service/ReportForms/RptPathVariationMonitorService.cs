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
        public Rpt_PathVariationMonitor GetRptPathVariationMonitor(String Begintime, String Endtime, String Ljdm)
        {
            Rpt_PathVariationMonitor rpt_PathVariationMonitor = new Rpt_PathVariationMonitor();
            List<Rpt_PathVariationMonitorList> rpt_PathVariationMonitorList = new List<Rpt_PathVariationMonitorList>();
            try
            {


                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@begindate",Begintime),
                        new SqlParameter("@enddate",Endtime),
                        new SqlParameter("@Ljdm",Ljdm)
                    };


                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_RptPathVariationMonitor", parameters, CommandType.StoredProcedure);


                if (dataTable.Rows.Count == 0)                              //非常重要
                {
                    rpt_PathVariationMonitor.Message = "无数据...";
                    return rpt_PathVariationMonitor;
                }


                foreach (DataRow row in dataTable.Rows)
                {
                    Rpt_PathVariationMonitorList variationMonitor = new Rpt_PathVariationMonitorList();

                    variationMonitor.Ljmc = ConvertMy.ToString(row["Ljmc"]);
                    variationMonitor.Pahtdetail = ConvertMy.ToString(row["Pahtdetail"]);
                    variationMonitor.EnForceCount = ConvertMy.ToInt32(row["EnForceCount"]);
                    variationMonitor.Variationcount = ConvertMy.ToInt32(row["Variationcount"]);

                    if (variationMonitor.EnForceCount == 0)
                    {
                        variationMonitor.Per = 0;
                    }
                    else
                    {
                        variationMonitor.Per = (variationMonitor.Variationcount) * 100 / (variationMonitor.EnForceCount);
                    }
                    rpt_PathVariationMonitorList.Add(variationMonitor);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }

            rpt_PathVariationMonitor.PathVariationMonitorList = rpt_PathVariationMonitorList;
            return rpt_PathVariationMonitor;
        }


        /// <summary>
        /// 路径变异原因统计分析详细内容
        /// </summary>
        /// <param name="Begintime"></param>
        /// <param name="Endtime"></param>
        /// <param name="Ljmc"></param>
        /// <param name="Bymc"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        private List<RPT_PathVariationMonitorDetail> GetRptPathVariationMonitorDetail(string Ljmc, string Jdmc, string Begintime, string Endtime)
        {
            List<RPT_PathVariationMonitorDetail> rpt_pathvariationListDetail = new List<RPT_PathVariationMonitorDetail>();

            try
            {
                string strSql = string.Format(@"(select distinct '变异' 节点状态, cpvr.syxh 首页序号,ip.name 患者姓名, dep.name 科室,war.name 病区 , dia.name 诊断,
us.name 床位医师, cpcp.name 路径名称,   cppd.ljmc  节点
from CP_VariantRecords cpvr
right join InPatient ip on ip.NoOfInPat=cpvr.syxh
left join users us on us.id=ip.resident 
left join Department dep on dep.id=ip.admitdept
left join Ward war on war.id=ip.admitward
left join Diagnosis dia on  dia.markid=ip.admitdiagnosis
left join CP_ClinicalPath cpcp on cpcp.ljdm=cpvr.ljdm
left join CP_PathDetail cppd on cppd.pahtdetailid=cpvr.pahtdetailid
where cpcp.ljdm='{0}' 
and convert(char(10),cast(cpvr.Bysj as datetime),120) >='{1}'
and convert(char(10),cast(cpvr.Bysj as datetime),120) <='{2}'
and cppd.ljmc='{3}'
union  
select  '执行' 节点状态,syxh 首页序号 , ip.name 患者姓名, dep.name 科室,war.name 病区 , dia.name 诊断,
us.name 床位医师, cpcp.name 路径名称,     activityname  节点
from CP_InPatientPathExeDetail  cpipped
right join InPatient ip on ip.NoOfInPat=cpipped.syxh
left join users us on us.id=ip.resident 
left join Department dep on dep.id=ip.admitdept
left join Ward war on war.id=ip.admitward
left join Diagnosis dia on  dia.markid=ip.admitdiagnosis
left join CP_ClinicalPath cpcp on cpcp.ljdm=cpipped.ljdm
where cpcp.ljdm='{0}'
and Jrsj >='{1}'
and Jrsj <='{2}'
and activityname='{3}')order by 节点状态 desc", Ljmc, Begintime, Endtime, Jdmc);

                DataTable dt = SqlHelper.ExecuteDataTable(strSql);

                foreach (DataRow row in dt.Rows)
                {
                    RPT_PathVariationMonitorDetail rpt_pathDetail = new RPT_PathVariationMonitorDetail();
                    rpt_pathDetail.Status = row["节点状态"].ToString();
                    //rpt_pathDetail.s = row["首页序号"].ToString();
                    rpt_pathDetail.Name = row["患者姓名"].ToString();
                    rpt_pathDetail.PathName = row["路径名称"].ToString();
                    rpt_pathDetail.Diagnosis = row["诊断"].ToString();
                    rpt_pathDetail.Doctor = row["床位医师"].ToString();
                    rpt_pathDetail.Dept = row["科室"].ToString();
                    rpt_pathDetail.Ward = row["病区"].ToString();
                    rpt_pathDetail.Variationname = row["节点"].ToString();
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