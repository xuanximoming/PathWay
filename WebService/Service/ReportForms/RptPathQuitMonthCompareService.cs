using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Yidansoft.Service.Entity;
using System.Data.SqlClient;
using System.Data;
using System.ServiceModel;
using DrectSoft.Tool;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Rpt_PathQuitMonthCompare GetRptPathQuitMonthCompare(String Begintime, String Endtime, String Ljdm, String Dept)
        {
            Rpt_PathQuitMonthCompare rpt_PathQuitMonthCompare = new Rpt_PathQuitMonthCompare();
            List<Rpt_PathQuitMonthCompareList> rpt_PathQuitMonthCompareList = new List<Rpt_PathQuitMonthCompareList>();

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


                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_RptPathQuitMonthCompare", parameters, CommandType.StoredProcedure);

                if (dataTable.Rows.Count == 0)                                              //非常重要
                {
                    rpt_PathQuitMonthCompare.Message = "无数据...";
                    return rpt_PathQuitMonthCompare;
                }



                foreach (DataRow row in dataTable.Rows)
                {
                    Rpt_PathQuitMonthCompareList quitMonthCompare = new Rpt_PathQuitMonthCompareList();

                    quitMonthCompare.Ljdm = ConvertMy.ToString(row["Ljdm"]);
                    quitMonthCompare.Ljmc = ConvertMy.ToString(row["Ljmc"]);
                    quitMonthCompare.Syks = ConvertMy.ToString(row["Syks"]);
                    quitMonthCompare.Jan = ConvertMy.ToInt32(row["Jan"]);
                    quitMonthCompare.Feb = ConvertMy.ToInt32(row["Feb"]);
                    quitMonthCompare.Mar = ConvertMy.ToInt32(row["Mar"]);
                    quitMonthCompare.Apr = ConvertMy.ToInt32(row["Apr"]);
                    quitMonthCompare.May = ConvertMy.ToInt32(row["May"]);
                    quitMonthCompare.Jun = ConvertMy.ToInt32(row["Jun"]);
                    quitMonthCompare.Jul = ConvertMy.ToInt32(row["Jul"]);
                    quitMonthCompare.Aug = ConvertMy.ToInt32(row["Aug"]);
                    quitMonthCompare.Sept = ConvertMy.ToInt32(row["Sept"]);
                    quitMonthCompare.Oct = ConvertMy.ToInt32(row["Oct"]);
                    quitMonthCompare.Nov = ConvertMy.ToInt32(row["Nov"]);
                    quitMonthCompare.Dec = ConvertMy.ToInt32(row["Dec"]);

                    rpt_PathQuitMonthCompareList.Add(quitMonthCompare);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }

            rpt_PathQuitMonthCompare.PathCompareList = rpt_PathQuitMonthCompareList;

            return rpt_PathQuitMonthCompare;
        }

        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<RPT_PathQuitMonthCompareDetail> GetRptPathQuitMonthDetailCompareDetail(String Ljmc, String time)
        {
            List<RPT_PathQuitMonthCompareDetail> rpt_PathQuitMonthCompareList = new List<RPT_PathQuitMonthCompareDetail>();
            try
            {
                string strSql = string.Format(@"select 
case cpipp.wcsj
when isnull(cpipp.wcsj,0)  then cpipp.wcsj else cpipp.tcsj end
as 出径时间,
ip.name 患者姓名, dep.name 科室,war.name 病区 , dia.name 诊断,
us.name 床位医师,cpcp.name 路径名称,cpipp.jrsj 入径时间,
case cpipp.ljzt 
when 1 then '执行中' 
when 2 then '退出' 
when 3 then '完成'
else '未进入' end as 路径状态 
from  CP_InPathPatient cpipp
left join CP_ClinicalPath cpcp on cpcp.ljdm=cpipp.ljdm
left join InPatient ip on ip.NoOfInPat=cpipp.syxh
left join users us on us.id=ip.resident 
left join Department dep on dep.id=ip.admitdept
left join Ward war on war.id=ip.admitward
left join Diagnosis dia on  dia.markid=ip.admitdiagnosis
where len(ip.name) <> 0
and cpipp.ljzt in(3,2)
and cpcp.ljdm='{0}'
and (convert(varchar(4),CONVERT(datetime,cpipp.tcsj ),120)='{1}' 
or convert(varchar(4),CONVERT(datetime,cpipp.wcsj ),120)='{1}')
order by '出径时间' asc", Ljmc, time);

                DataTable dt = SqlHelper.ExecuteDataTable(strSql);//.ExecuteDataTable("usp_CP_RptPathStatistics", parameters, CommandType.StoredProcedure);

                foreach (DataRow dr in dt.Rows)
                {
                    RPT_PathQuitMonthCompareDetail rptDetail = new RPT_PathQuitMonthCompareDetail();
                    rptDetail.PatientName = dr["患者姓名"].ToString();
                    rptDetail.PathName = dr["路径名称"].ToString();
                    rptDetail.Diagnosis = dr["诊断"].ToString();
                    rptDetail.Dept = dr["科室"].ToString();
                    rptDetail.Ward = dr["病区"].ToString();
                    rptDetail.Doctor = dr["床位医师"].ToString();
                    rptDetail.InPathTime = dr["入径时间"].ToString();
                    rptDetail.OutPathTime = dr["出径时间"].ToString();
                    rptDetail.PathStatus = dr["路径状态"].ToString();
                    rpt_PathQuitMonthCompareList.Add(rptDetail);
                }
                return rpt_PathQuitMonthCompareList;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }
        }
    }
}