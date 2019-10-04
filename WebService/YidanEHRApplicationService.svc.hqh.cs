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

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {

        #region 报表统计
        /// <summary>
        /// 获取临床路径执行概括统计数据列表
        /// </summary>
        /// <param name="office">临床科室</param>
        /// <param name="paths">临床路径代码</param>
        /// <param name="startData">开始日期</param>
        /// <param name="EndData">开始日期</param>
        /// <returns>List<RPT_PathStatistic></returns>
        [OperationContract]
        public List<RPT_PathStatistic> GetRptPathStatistic(string office, string paths, string startDate, string EndDate)
        {
            List<RPT_PathStatistic> rptlist = new List<RPT_PathStatistic>();

            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                    { 
                         new SqlParameter("@dept",office ),
                         new SqlParameter("@Ljdm", paths),
                         new SqlParameter("@begindate",startDate),
                         new SqlParameter("@enddate", EndDate)
                    };



                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_RptPathStatistics", parameters, CommandType.StoredProcedure);

                foreach (DataRow dr in dt.Rows)
                {
                    RPT_PathStatistic rpt = new RPT_PathStatistic();
                    rpt.Ljdm = dr["LjdmID"].ToString();
                    rpt.Jgsj = dr["Jgsj"].ToString();
                    rpt.Jzyfy = Convert.ToInt32(dr["Jzyfy"].ToString());
                    rpt.Jzyts = Convert.ToInt32(dr["Jzyts"].ToString());
                    rpt.Ljmc = dr["Ljmc"].ToString();
                    rpt.Rjl = Convert.ToInt32(dr["Rjl"].ToString());
                    rpt.Ssls = Convert.ToInt32(dr["Ssls"].ToString());
                    rpt.Tcl = Convert.ToInt32(dr["Tcbl"].ToString());
                    rpt.Tcsl = Convert.ToInt32(dr["Tcsl"].ToString());
                    rpt.HzSl = Convert.ToInt32(dr["HzSL"].ToString());
                    rpt.Wcl = Convert.ToInt32(dr["Wcbl"].ToString());
                    rpt.WcSl = Convert.ToInt32(dr["WcSl"].ToString());
                    rpt.Byl = Convert.ToInt32(dr["Bybl"].ToString());
                    rpt.BySl = Convert.ToInt32(dr["BySl"].ToString());
                    rpt.Zxqk = dr["Zxqk"].ToString();
                    rpt.Jcfy = Convert.ToInt32(dr["Jcfy"].ToString());
                    rpt.Jcts = Convert.ToInt32(dr["Jcts"].ToString());
                    rpt.Vesion = Convert.ToDecimal(dr["Vesion"].ToString());
                    rptlist.Add(rpt);
                }
                return rptlist;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }       // zm 8.24 Oracle
        /// <summary>
        /// 根据临床路径获取病人详细信息  //wj 9.25
        /// </summary>
        /// <param name="ljmc">路径名称</param>
        /// <returns></returns>
        [OperationContract]
        public List<RPT_PathStatisticDetail> GetRptPathStatisticDetail(string ljmc, string begindate, string enddate)
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
left join users us on us.id=ip.resident where
cpcp.ljdm='{0}'

and cpipp.jrsj >= '{1}'
and cpipp.jrsj <= '{2}'", ljmc, begindate, enddate);
 //               ip.admitdiagnosis in 
 //(select cppejc.jcxm 
 // from CP_PathEnterJudgeCondition  cppejc
 // right join Diagnosis dia on  dia.markid=cppejc.jcxm
 // where ljdm='{0}')



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
        /// 获取临床路径退出原因
        /// </summary>
        /// <param name="office">科室代码</param>
        /// <param name="paths">临床路径代码</param>
        /// <param name="doctor">床位医生代码</param>
        /// <param name="startDate">开始日期</param>
        /// <param name="EndDate">结束日期</param>
        /// <returns>List<Rpt_PathQuitList> </returns>        
        [OperationContract]
        public List<Rpt_DataList> GetRptPathQuitList(string office, string paths, string startDate, string EndDate)
        {
            List<Rpt_DataList> lstData = new List<Rpt_DataList>();
            List<Rpt_PathQuitList> lstRpt = new List<Rpt_PathQuitList>();
            List<Rpt_PathQuitPie> lstPie = new List<Rpt_PathQuitPie>();


            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                    { 
                         new SqlParameter("@dept",office),
                         new SqlParameter("@Ljdm", paths),
                         new SqlParameter("@Doctor", ""),
                         new SqlParameter("@begindate", startDate),
                         new SqlParameter("@enddate",EndDate)
                    };

                DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_RptPathQuit", parameters, CommandType.StoredProcedure);

                foreach (DataRow dr in ds.Tables[0].Rows)
                {
                    Rpt_PathQuitList rpt = new Rpt_PathQuitList();
                    rpt.Name = dr["Name"].ToString();
                    rpt.Tcsj = dr["Tcsj"].ToString();
                    rpt.Hzxm = dr["Hzxm"].ToString();
                    rpt.Jrsj = dr["Jrsj"].ToString();
                    rpt.DName = dr["DName"].ToString();
                    rpt.Tcyy = dr["Tcyy"].ToString();
                    rpt.Dept = dr["dpName"].ToString();
                    lstRpt.Add(rpt);
                }

                foreach (DataRow dr in ds.Tables[1].Rows)
                {
                    Rpt_PathQuitPie rpt = new Rpt_PathQuitPie();
                    rpt.Tcyy = dr["Tcyy"].ToString();
                    rpt.Counts = Convert.ToInt32(dr["cnt"].ToString());
                    lstPie.Add(rpt);
                }

                Rpt_DataList rptDataList = new Rpt_DataList();
                rptDataList.PathQuitList = lstRpt;
                rptDataList.PathQuitPie = lstPie;
                lstData.Add(rptDataList);

                return lstData;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }
            //finally
            //{
            //    cn.Close();
            //}
            //}
        }               // zm 8.24 Oracle
        #endregion

        #region 临床变异
        /// <summary>
        /// 临床路径实际执行变异查询
        /// </summary>
        /// <param name="pathID">路径序号（CP_InPathPatient.ID）</param>
        /// <param name="pathDetailID">路径明细代码</param>
        /// <returns></returns>
        [OperationContract]
        public List<CP_PathExecVariantRecords> GetPathExecVariantRecordList(string pathXh, string pathDetailID)
        {
            List<CP_PathExecVariantRecords> rptlist = new List<CP_PathExecVariantRecords>();

            try
            {
                if (pathDetailID != "" && pathXh != "" && pathDetailID != null && pathXh != null)
                {

                    SqlParameter[] parameters = new SqlParameter[] 
                        { 
                             new SqlParameter("@Ljxh", pathXh),
                             new SqlParameter("@Mxdm", pathDetailID )
                        };


                    DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_PathExecVariantRecords", parameters, CommandType.StoredProcedure);

                    foreach (DataRow dr in dt.Rows)
                    {
                        CP_PathExecVariantRecords cp = new CP_PathExecVariantRecords();
                        cp.Blbmc = dr["Blbmc"].ToString();
                        cp.Byyy = dr["Byyy"].ToString();
                        cp.Bynr = dr["Bynr"].ToString();
                        cp.Xmmc = dr["Xmmc"].ToString();
                        cp.Bylb = dr["Bylb"].ToString();
                        cp.Bysj = dr["Bysj"].ToString();

                        rptlist.Add(cp);
                    }
                }
                return rptlist;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }           // zm 8.24 Oracle
        #endregion

        #region 质控中心

        #region 病历超时
        /// <summary>
        /// 获取病历超时提示信息列表
        /// </summary>
        /// <param name="startDate">开始时间（可以为空）</param>
        /// <param name="endDate">结束时间（可以为空）</param>
        /// <returns></returns>
        [OperationContract()]
        [FaultContract(typeof(LoginException))]
        public List<CP_QCRecordTip> GetQCRecordTipList(string startDate, string endDate)
        {
            List<CP_QCRecordTip> rptlist = new List<CP_QCRecordTip>();
            return rptlist;

            //try
            //{

            //    SqlParameter[] parameters = new SqlParameter[] 
            //        { 
            //             new SqlParameter("@StartDate", startDate),
            //             new SqlParameter("@EndDate", endDate)
            //        };

            //    DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_QCRecord", parameters, CommandType.StoredProcedure);

            //    foreach (DataRow dr in dt.Rows)
            //    {
            //        CP_QCRecordTip cp = new CP_QCRecordTip();
            //        cp.Blts = dr["Blts"].ToString();
            //        cp.Hzxm = dr["Hzxm"].ToString();
            //        cp.ID = dr["ID"].ToString();
            //        cp.Jrsj = dr["Jrsj"].ToString();
            //        cp.Ljdm = dr["Ljdm"].ToString();
            //        cp.Ljmc = dr["Ljmc"].ToString();
            //        cp.Ljts = dr["Ljts"].ToString();
            //        cp.Ljzt = dr["Ljzt"].ToString();
            //        cp.Syxh = dr["Syxh"].ToString();
            //        cp.Tcsj = dr["Tcsj"].ToString();
            //        cp.Tjsj = dr["Tjsj"].ToString();
            //        cp.Wcsj = dr["Wcsj"].ToString();
            //        cp.Ysdm = dr["Cwys"].ToString();
            //        cp.Ysxm = dr["Ysxm"].ToString();

            //        rptlist.Add(cp);
            //    }

            //    return rptlist;
            //}

            //catch (Exception ex)
            //{
            //    ThrowException(ex);
            //    return null;
            //}

        }           // zm 8.24 Oracle
        #endregion

        #region 病人住院天数超标
        /// <summary>
        /// 获取病人住院天数超标信息
        /// </summary>
        /// <param name="Days">最近Days内超标</param>
        /// <param name="endDate">结束时间（可以为空）</param>
        /// <returns></returns>
        [OperationContract()]
        [FaultContract(typeof(LoginException))]
        public List<CP_BeyondDays> GetBeyondDaysList(string Days)
        {
            List<CP_BeyondDays> rptlist = new List<CP_BeyondDays>();

            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                    {  
                         new SqlParameter("@Days", Days)
                    };


                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_BeyondDays", parameters, CommandType.StoredProcedure);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_BeyondDays cp = new CP_BeyondDays();
                    cp.IsCheck = false;
                    cp.Zgts = dr["Zgts"].ToString();
                    cp.Hzxm = dr["Name"].ToString();
                    cp.ID = dr["ID"].ToString();
                    cp.Jrsj = dr["Jrsj"].ToString();
                    cp.Ljdm = dr["Ljdm"].ToString();
                    cp.Ljmc = dr["Ljmc"].ToString();
                    cp.Ljts = dr["Ljts"].ToString();
                    cp.Ljzt = dr["Ljzt"].ToString();
                    cp.Syxh = dr["Syxh"].ToString();
                    cp.Tcsj = dr["Tcsj"].ToString();
                    cp.Sjts = dr["Sjts"].ToString();
                    cp.Wcsj = dr["Wcsj"].ToString();
                    cp.Ysdm = dr["Cwys"].ToString();
                    cp.Ysxm = dr["Ysxm"].ToString();
                    cp.Ccts = dr["Ccts"].ToString();

                    rptlist.Add(cp);
                }

                return rptlist;
            }

            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }
            //    finally
            //    {
            //        cn.Close();
            //    }
            //}
        }           // zm 8.24 Oracle
        #endregion

        #region 病人住院费用超标
        /// <summary>
        /// 获取病人住院费用超标信息
        /// </summary>
        /// <returns></returns>
        [OperationContract()]
        [FaultContract(typeof(LoginException))]
        public List<CP_BeyondFee> GetBeyondFeeList()
        {
            List<CP_BeyondFee> rptlist = new List<CP_BeyondFee>();

            try
            {

                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_BeyondFee");

                foreach (DataRow dr in dt.Rows)
                {
                    CP_BeyondFee cp = new CP_BeyondFee();
                    cp.Jcfy = dr["Jcfy"].ToString();
                    cp.Hzxm = dr["Hzxm"].ToString();
                    cp.ID = dr["ID"].ToString();
                    cp.Jrsj = dr["Jrsj"].ToString();
                    cp.Ljdm = dr["Ljdm"].ToString();
                    cp.Ljmc = dr["Ljmc"].ToString();
                    cp.Ljts = dr["Ljts"].ToString();
                    cp.Ljzt = dr["Ljzt"].ToString();
                    cp.Syxh = dr["Syxh"].ToString();
                    cp.Tcsj = dr["Tcsj"].ToString();
                    cp.Sjfy = dr["Xmje"].ToString();
                    cp.Wcsj = dr["Wcsj"].ToString();
                    cp.Ysdm = dr["Cwys"].ToString();
                    cp.Ysxm = dr["Ysxm"].ToString();
                    cp.Ccfy = dr["Ccfy"].ToString();

                    rptlist.Add(cp);
                }

                return rptlist;
            }

            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }
            //    finally
            //    {
            //        cn.Close();
            //    }
            //}
        }           // zm 8.24 Oracle
        #endregion

        #region 病人强制进入
        /// <summary>
        /// 获取病人强制进入信息
        /// </summary>
        /// <param name="startDate">开始时间（可以为空）</param>
        /// <param name="endDate">结束时间（可以为空）</param>
        /// <returns></returns>
        [OperationContract()]
        [FaultContract(typeof(LoginException))]
        public List<CP_ForceToPath> GetForceToPathList(string startDate, string endDate)
        {
            List<CP_ForceToPath> rptlist = new List<CP_ForceToPath>();
            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                    { 
                         new SqlParameter("@StartDate", startDate),
                         new SqlParameter("@EndDate", endDate)
                    };


                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_ForceToPath", parameters, CommandType.StoredProcedure);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_ForceToPath cp = new CP_ForceToPath();
                    cp.Hzxm = dr["Name"].ToString();
                    cp.ID = dr["ID"].ToString();
                    cp.Jrsj = dr["Jrsj"].ToString();
                    cp.Ljdm = dr["Ljdm"].ToString();
                    cp.Ljmc = dr["Ljmc"].ToString();
                    cp.Ljts = dr["Ljts"].ToString();
                    cp.Ljzt = dr["Ljzt"].ToString();
                    cp.Syxh = dr["Syxh"].ToString();
                    cp.Tcsj = dr["Tcsj"].ToString();
                    cp.Wcsj = dr["Wcsj"].ToString();
                    cp.Ysdm = dr["Cwys"].ToString();
                    cp.Ysxm = dr["Ysxm"].ToString();
                    cp.Qzjryy = dr["Memo"].ToString();

                    rptlist.Add(cp);
                }

                return rptlist;
            }

            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }       // zm 8.24 Oracle
        #endregion

        #region 病人中途退出
        /// <summary>
        /// 获取病人中途退出信息
        /// </summary>
        /// <param name="startDate">开始时间（可以为空）</param>
        /// <param name="endDate">结束时间（可以为空）</param>
        /// <returns></returns>
        [OperationContract()]
        [FaultContract(typeof(LoginException))]
        public List<CP_PathExit> GetPathExitList(string startDate, string endDate)
        {
            List<CP_PathExit> rptlist = new List<CP_PathExit>();
            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                         new SqlParameter("@StartDate", startDate),
                         new SqlParameter("@EndDate", endDate)
                    };


                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_PathExit", parameters, CommandType.StoredProcedure);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_PathExit cp = new CP_PathExit();
                    cp.Hzxm = dr["Name"].ToString();
                    cp.ID = dr["ID"].ToString();
                    cp.Jrsj = dr["Jrsj"].ToString();
                    cp.Ljdm = dr["Ljdm"].ToString();
                    cp.Ljmc = dr["Ljmc"].ToString();
                    cp.Ljts = dr["Ljts"].ToString();
                    cp.Ljzt = dr["Ljzt"].ToString();
                    cp.Syxh = dr["Syxh"].ToString();
                    cp.Tcsj = dr["Tcsj"].ToString();
                    cp.Wcsj = dr["Wcsj"].ToString();
                    cp.Ysdm = dr["Cwys"].ToString();
                    cp.Ysxm = dr["Ysxm"].ToString();

                    rptlist.Add(cp);
                }

                return rptlist;
            }

            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }
        }       // zm 8.24 Oracle
        #endregion

        #region 医务处提交问题
        /// <summary>
        /// 添加医务处提交的问题
        /// </summary>
        /// <param name="lstQC">问题列表</param>
        /// <returns></returns>
        [OperationContract()]
        [FaultContract(typeof(LoginException))]
        public string AddQuestionCenter(List<CP_QCProblem> lstQC)
        {

            string strReturn = "问题提交成功！";
            try
            {

                //循环添加提交的问题内容
                foreach (CP_QCProblem cp in lstQC)
                {
                    string strSql = string.Format(@"INSERT CP_QCProblem (Syxh,Wtzt,Ljdm,Zrys,Djry,Wtnr)
                                                VALUES  ( {0}, {1} , '{2}', '{3}', '{4}', '{5}')",
                                             cp.Syxh, cp.Wtzt, cp.Ljdm, cp.Zrys, cp.Djry, cp.Wtnr);

                    SqlHelper.ExecuteNoneQuery(strSql);
                }



            }

            catch (Exception ex)
            {
                //sqlTrans.Rollback();
                strReturn = "问题提交失败！";
                ThrowException(ex);
            }

            return strReturn;

        }       // zm 8.24 Oracle
        #endregion

        #region 获取提交的问题
        /// <summary>
        /// 获取提交的问题及回复审核数据列表
        /// </summary>
        /// <param name="audit">审核状态</param>
        /// <param name="days">查询最近的天数（其中 0 为全部）</param>
        /// <param name="querytype">查询类型（其中 0为医务处查询，1为医生查询）</param>
        /// <returns></returns>
        [OperationContract()]
        [FaultContract(typeof(LoginException))]
        public List<CP_QCProblem> GetQCProblemList(int audit, string days, int querytype)
        {
            List<CP_QCProblem> lstCP = new List<CP_QCProblem>();

            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@QState", audit),
                        new SqlParameter("@Days", days),
                        new SqlParameter("@QueryType", querytype)
                    };



                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_QCProblem", parameters, CommandType.StoredProcedure);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_QCProblem cp = new CP_QCProblem();
                    cp.Dfnr = dr["Dfnr"].ToString();
                    cp.Dfrq = dr["Dfrq"].ToString();
                    cp.Dfys = dr["Dfys"].ToString();
                    cp.Djrq = dr["Djrq"].ToString();
                    cp.Djry = dr["Djry"].ToString();
                    cp.Hzxm = dr["Hzxm"].ToString();
                    cp.Ljdm = dr["Ljdm"].ToString();
                    cp.Ljmc = dr["Ljmc"].ToString();
                    cp.Shnr = dr["Shnr"].ToString();
                    cp.Shrq = dr["Shrq"].ToString();
                    cp.Shry = dr["Shry"].ToString();
                    cp.Syxh = Convert.ToInt32(dr["Syxh"].ToString());
                    cp.Wtnr = dr["Wtnr"].ToString();
                    cp.Wtxh = Convert.ToInt32(dr["Wtxh"].ToString());
                    cp.Wtzt = Convert.ToInt32(dr["Wtzt"].ToString());
                    cp.Ysxm = dr["Ysxm"].ToString();
                    cp.Zfrq = dr["Zfrq"].ToString();
                    cp.Zfry = dr["Zfry"].ToString();
                    cp.Zrys = dr["Zrys"].ToString();
                    cp.Qczt = dr["Qczt"].ToString();
                    cp.Shzt = dr["Shzt"].ToString();
                    cp.Djryxm = dr["Djryxm"].ToString();
                    cp.Shryxm = dr["Shryxm"].ToString();
                    cp.Zfryxm = dr["Zfryxm"].ToString();
                    cp.Dfysxm = dr["Dfysxm"].ToString();
                    lstCP.Add(cp);
                }
                return lstCP;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }  // zm 8.24 Oracle
        #endregion

        #region 医务处审核和医生回复问题
        /// <summary>
        /// 医务处审核和医生回复问题
        /// </summary>
        /// <param name="lst">单个问题实体</param>
        /// <param name="audit">审核类型（医务处）或医生答复</param>
        /// <returns></returns>
        [OperationContract()]
        [FaultContract(typeof(LoginException))]
        public string AuditQuestion(CP_QCProblem lst, int audit)
        {
            try
            {
                string sql = "";
                string UpdateTime = DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss");

                if (audit == 0)  //未审核（挂起）（医务处）
                {
                    sql = string.Format("update CP_QCProblem set Wtzt=2,Wtnr='{0}',Djrq='{1}',Djry='{2}' where  Wtxh={3} ", lst.Wtnr, UpdateTime, lst.Djry, lst.Wtxh);
                }
                else if (audit == 1)//已审核（医务处）
                {
                    sql = string.Format("update CP_QCProblem set Wtzt=4,Shnr='{0}',Shrq='{1}',Shry='{2}' where Wtxh={3}",
                              lst.Shnr, UpdateTime, lst.Shry, lst.Wtxh);
                }
                else if (audit == 2)//作废（医务处）
                {
                    sql = string.Format("update CP_QCProblem set Wtzt=4,Zfrq='{0}',Zfry='{1}' where  Wtxh={2} ",
                             UpdateTime, lst.Zfry, lst.Wtxh);
                }
                else if (audit == 3)//医生答复
                {
                    sql = string.Format("update CP_QCProblem set Wtzt=1,Dfnr='{0}',Dfrq='{1}',Dfys='{2}' where  Wtxh={3} ",
                             lst.Dfnr, UpdateTime, lst.Dfys, lst.Wtxh);
                }

                SqlHelper.ExecuteNoneQuery(sql);

                return UpdateTime.ToString();


            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return "操作失败!";
            }


        }
        #endregion

        #endregion

        #region 医嘱（套餐）
        /// <summary>
        ///  拖拽查询医嘱详细套餐  
        /// </summary>
        /// <param name="ctyzxh">医嘱套餐序号</param>
        /// <param name="strSyxh">首页序号</param>
        /// <param name="employee">当前医生CP_Employee实体</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_DoctorOrder> GetDrapDropAdviceSuitDetail(decimal ctyzxh, string syxh, CP_Employee employee)
        {
            List<CP_DoctorOrder> listOrder = new List<CP_DoctorOrder>();


            SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@ctyzxh",ctyzxh)
                };



            DataTable dataTable = SqlHelper.ExecuteDataTable("usp_DrapDropAdviceSuitDetail", parameters, CommandType.StoredProcedure);

            foreach (DataRow row in dataTable.Rows)
            {
                CP_DoctorOrder order = new CP_DoctorOrder();
                order.Ctmxxh = Convert.ToDecimal(row["Ctmxxh"].ToString() == string.Empty ? "0" : row["Ctmxxh"].ToString());//
                order.Syxh = Convert.ToDecimal(syxh);
                order.Bqdm = employee.Bqdm;
                order.Ksdm = employee.Ksdm;
                order.Lrysdm = employee.Zgdm;
                order.Cdxh = Convert.ToDecimal(row["Cdxh"].ToString() == string.Empty ? "0" : row["Cdxh"].ToString());//
                order.Ggxh = Convert.ToDecimal(row["Ggxh"].ToString() == string.Empty ? "0" : row["Ggxh"].ToString()); //
                order.Lcxh = Convert.ToDecimal(row["Lcxh"].ToString() == string.Empty ? "0" : row["Lcxh"].ToString());//
                order.Ypdm = row["Ypdm"].ToString(); //
                order.Xmlb = Convert.ToDecimal(row["Xmlb"].ToString() == string.Empty ? "0" : row["Xmlb"].ToString()); //

                order.Yzlb = Convert.ToDecimal(row["Yzlb"].ToString() == string.Empty ? "0" : row["Yzlb"].ToString()); //
                order.Yzbz = Convert.ToDecimal(row["Yzbz"].ToString() == string.Empty ? "0" : row["Yzbz"].ToString()); //
                order.YzbzName = row["YzbzName"].ToString();  //
                order.Ypjl = Convert.ToDecimal(row["Ypjl"].ToString() == string.Empty ? "0" : row["Ypjl"].ToString()); //
                order.Jldw = row["Jldw"].ToString();//
                order.Yfdm = row["Yfdm"].ToString(); //
                order.YfdmName = row["YfdmName"].ToString(); //
                order.Pcdm = row["Pcdm"].ToString();      //
                order.PcdmName = row["PcdmName"].ToString();   //
                order.Ksrq = GetDefaultOrderTime((OrderType)(Convert.ToDecimal(row["Yzbz"].ToString())));
                order.Ypmc = row["Ypmc"].ToString(); //
                order.FromTable = row["FromTable"].ToString();//
                order.Flag = row["Flag"].ToString();//
                order.OrderGuid = Guid.NewGuid().ToString();//  
                order.Fzbz = Convert.ToDecimal(row["Fzbz"].ToString() == string.Empty ? "0" : row["Fzbz"].ToString());
                order.Fzxh = Convert.ToDecimal(row["Fzxh"].ToString() == string.Empty ? "0" : row["Fzxh"].ToString());
                order.Zxdw = row["Zxdw"].ToString();
                order.Ypgg = row["Ypgg"].ToString();
                order.Dwxs = Convert.ToDecimal(row["Dwxs"].ToString() == string.Empty ? "0" : row["Dwxs"].ToString());
                order.Dwlb = Convert.ToDecimal(row["Dwlb"].ToString() == string.Empty ? "0" : row["Dwlb"].ToString());

                //add by luff 20130121
                order.Jjlx = int.Parse(row["Isjj"].ToString());
                order.Zxksdm = row["Zxksdm"].ToString();
                order.Zxcs = Convert.ToDecimal(row["Zxcs"].ToString() == string.Empty ? "0" : row["Zxcs"].ToString());
                order.Zxzq = Convert.ToDecimal(row["Zxzq"].ToString() == string.Empty ? "0" : row["Zxzq"].ToString());
                order.Zxzqdw = Convert.ToDecimal(row["Zxzqdw"].ToString() == string.Empty ? "0" : row["Zxzqdw"].ToString());
                order.Zdm = row["Zdm"].ToString();
                order.Zxsj = row["Zxsj"].ToString();
                order.Yznr = row["Yznr"].ToString();
                order.Yzzt = Convert.ToDecimal(row["Yzzt"].ToString() == string.Empty ? "0" : row["Yzzt"].ToString());

                listOrder.Add(order);
            }

            return listOrder;
        }         // zm 8.24 Oracle

        #endregion

    }

}