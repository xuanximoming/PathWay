using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Data.SqlClient;
using Yidansoft.Service.Entity.ReportForms;
using System.Data;
using DrectSoft.Tool;
using Yidansoft.Service.Entity;

namespace Yidansoft.Service
{
    /// <summary>
    /// 表示统计入径报表的类 
    /// </summary>
    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 提供入径统计的方法
        /// </summary>
        /// <param name="begindate">开始日期</param>
        /// <param name="enddate">结束日期</param>
        /// <param name="dept">部门编号</param>
        /// <param name="Ljzt">路径状态</param>
        /// <param name="ljdm">路径代码</param>
        /// <param name="bzdm">病种代码</param>
        /// <param name="gettype">查询方式（1 根据路径统计；2 根据病种统计）</param>
        /// <returns>Rpt_PathEnterStatistics返回入径统计报表集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<Rpt_PathEnterStatistics> GetRpt_PathEnterStatistics(String begindate, String enddate, String dept, String Ljzt, String ljdm, String bzdm, String gettype)
        {
            try
            {
                List<Rpt_PathEnterStatistics> pathList = new List<Rpt_PathEnterStatistics>();//入径报表集合
                //DataSet dataSet = new DataSet();
                //SqlConnection connStr = new SqlConnection(m_ConnectionString);
                //SqlCommand cmd = new SqlCommand("usp_CP_RptPathEnterStatistics", connStr);
                //cmd.CommandType = CommandType.StoredProcedure;

                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@begindate",begindate),
                    new SqlParameter("@enddate",enddate),
                    new SqlParameter("@dept",dept),
                    new SqlParameter("@Ljzt",Ljzt),
                    new SqlParameter("@Ljdm",ljdm),
                    new SqlParameter("@Bzdm",bzdm),
                    new SqlParameter("@GetType",gettype)
                };


                DataSet dataSet = SqlHelper.ExecuteDataSet("usp_CP_RptPathEnterStatistics", parameters, CommandType.StoredProcedure);

                Int32 count = dataSet.Tables[0].Rows.Count;
                if (count != 0)
                {
                    for (Int32 i = 0; i < count; i++)
                    {
                        Rpt_PathEnterStatistics pathEnterStatistics = new Rpt_PathEnterStatistics();
                        pathEnterStatistics.Ksmc = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Ksmc"]);
                        pathEnterStatistics.Ljdm = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["LjdmID"]);
                        pathEnterStatistics.Ljmc = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Ljmc"]);
                        pathEnterStatistics.Bhzs = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Bhzs"]);
                        pathEnterStatistics.Rjrs = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Rjrs"]);
                        pathEnterStatistics.Rjl = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Rjl"]);
                        pathEnterStatistics.Wcrs = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Wcrs"]);
                        pathEnterStatistics.Wcl = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Wcl"]);
                        pathEnterStatistics.Tcrs = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Tcrs"]);
                        pathEnterStatistics.Tcl = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Tcl"]);
                        pathEnterStatistics.Zjrs = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Zjrs"]);
                        pathEnterStatistics.Zjl = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Zjl"]);
                        pathList.Add(pathEnterStatistics);
                    }
                }
                return pathList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        [OperationContract]
        public List<RPT_PathStatisticDetail> GetRpt_PathEnterStatisticsDetail(string ljmc, string begindate, string enddate)
        {
            List<RPT_PathStatisticDetail> rptdetaillist = new List<RPT_PathStatisticDetail>();

            try
            {
                string strSql = string.Format(@"select ip.name 患者姓名, 
case ip.sexid 
when 1 then '男' 
when 2 then '女' 
else '未知' end as 性别,
case cpcp.name
when isnull(cpcp.name,0)  then cpcp.name else '未进入' end
as 路径名称,dia.name 入院诊断,
dep.name 科室,war.name 病区 , 
us.name 床位医师,
case cpipp.jrsj
when isnull(cpipp.jrsj,0)  then cpipp.jrsj else '未进入' end
as 入径时间,
case cpipp.ljzt 
when 1 then '执行中' 
when 2 then '退出' 
when 3 then '完成'
else '未进入' end as 路径状态 
from InPatient ip
left join CP_InPathPatient  cpipp on ip.NoOfinpat=cpipp.Syxh
left join CP_ClinicalPath cpcp on cpcp.ljdm=cpipp.ljdm
left join Diagnosis dia on  dia.markid=ip.admitdiagnosis
left join Department dep on dep.id=ip.admitdept
left join Ward war on war.id=ip.admitward
left join users us on us.id=ip.resident where ip.admitdiagnosis in 
 (select cppejc.jcxm 
  from CP_PathEnterJudgeCondition  cppejc
  right join Diagnosis dia on  dia.markid=cppejc.jcxm
  where ljdm='{0}')
and ip.inwarddate >= '{1}'
and ip.inwarddate <= '{2}'", ljmc, begindate, enddate);



                DataTable dt = SqlHelper.ExecuteDataTable(strSql);//.ExecuteDataTable("usp_CP_RptPathStatistics", parameters, CommandType.StoredProcedure);

                foreach (DataRow dr in dt.Rows)
                {
                    RPT_PathStatisticDetail rptDetail = new RPT_PathStatisticDetail();
                    rptDetail.PatientName = dr["患者姓名"].ToString();
                    rptDetail.Sex = dr["性别"].ToString();
                    rptDetail.PathName = dr["路径名称"].ToString();
                    rptDetail.Diagnosis = dr["入院诊断"].ToString();
                    rptDetail.Dept = dr["科室"].ToString();
                    rptDetail.Ward = dr["病区"].ToString();
                    rptDetail.Doctor = dr["床位医师"].ToString();
                    rptDetail.InPathTime = dr["入径时间"].ToString();
                    rptDetail.PathStatus = dr["路径状态"].ToString();
                    rptdetaillist.Add(rptDetail);
                }
                return rptdetaillist;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }

        /// <summary>
        /// 表示获取路径证断表的方法
        /// </summary>
        /// <param name="dept">科室代码</param>
        /// <returns>Rpt_ClinicalDiagnosis返回入径证断表集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<Rpt_ClinicalDiagnosis> GetRpt_ClinicalDiagnosis(String dept)
        {
            try
            {
                List<Rpt_ClinicalDiagnosis> clinicalDiagnosisList = new List<Rpt_ClinicalDiagnosis>();

                String sqlStr = "SELECT DISTINCT Bzdm,Bzmc FROM CP_ClinicalDiagnosis WHERE Ljdm IN(SELECT Ljdm FROM CP_ClinicalPath)";
                if (dept != "")
                {
                    sqlStr = "SELECT DISTINCT Bzdm,Bzmc FROM CP_ClinicalDiagnosis WHERE Ljdm IN(SELECT Ljdm FROM CP_ClinicalPath where Syks ='" + dept + "')";
                }


                DataSet dataSet = SqlHelper.ExecuteDataSet(sqlStr);

                Int32 count = dataSet.Tables[0].Rows.Count;
                if (count != 0)
                {
                    for (Int32 i = 0; i < count; i++)
                    {
                        Rpt_ClinicalDiagnosis clinicalDiagnosis = new Rpt_ClinicalDiagnosis();
                        clinicalDiagnosis.Bzdm = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Bzdm"]);
                        clinicalDiagnosis.Bzmc = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Bzmc"]);
                        clinicalDiagnosisList.Add(clinicalDiagnosis);
                    }
                }
                return clinicalDiagnosisList;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }





        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<PE_PatientIn> GetPatientInHospital()
        {
            try
            {
                List<PE_PatientIn> PatientInHospital = new List<PE_PatientIn>();

                String sqlStr = @"select inp.Name,inp.PatID,a.Name Sex,dept.Name deptname,ward.Name wardname,path.Name pathname ,path.ljdm,  inp.AdmitDate,
  CASE WHEN ppath.Ljzt is null THEN '未进入'
                     WHEN ppath.Ljzt = 1
                     THEN '执行中(' + CONVERT(VARCHAR, ppath.Ljts) + ')'
                     WHEN ppath.Ljzt = 2
                     THEN '退出(' + CONVERT(VARCHAR, ppath.Ljts) + ')'
                     WHEN ppath.Ljzt = 3
                     THEN '完成(' + CONVERT(VARCHAR, ppath.Ljts) +')'
                END AS LjztName --路径状态ppath.Ljzt
from dbo.InPatient inp 
left join CP_PathEnterJudgeCondition judge on judge.Jcxm = inp.AdmitDiagnosis
left join Dictionary_detail a on  a.CategoryID = '3' and inp.SexID = a.DetailID
left join dbo.Department dept on dept.ID = inp.AdmitDept
left join dbo.Ward ward on ward.ID = inp.AdmitWard
left join dbo.CP_ClinicalPath path on path.Ljdm = judge.Ljdm
left join dbo.CP_InPathPatient ppath on ppath.Ljdm = path.Ljdm and ppath.Syxh = inp.NoOfInpat

where path.Name is not null
";


                DataSet dataSet = SqlHelper.ExecuteDataSet(sqlStr);
                Int32 count = dataSet.Tables[0].Rows.Count;
                if (count != 0)
                {
                    for (Int32 i = 0; i < count; i++)
                    {
                        PE_PatientIn Patient = new PE_PatientIn();
                        Patient.Name = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Name"]);
                        Patient.ID = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["PatID"]);
                        Patient.Sex = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["Sex"]);
                        Patient.Ward = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["wardname"]);
                        Patient.Dept = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["deptname"]);
                        Patient.Path = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["pathname"]);
                        Patient.PathCode = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["ljdm"]);
                        Patient.PathStatus = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["LjztName"]);
                        Patient.AdmitDate = ConvertMy.ToString(dataSet.Tables[0].Rows[i]["AdmitDate"]);
                        PatientInHospital.Add(Patient);
                    }
                }
                return PatientInHospital;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}