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
using System.Web.Hosting;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {

        #region 根据条件查询职工信息
        /// <summary>
        /// 根据条件获取指定用户(有效的)
        /// 修改：fqw 时间：2010-03-18  mark：fqwFix
        /// </summary>
        /// <param name="zgdm">职工代码（为空时候查询所有）</param>
        /// <param name="py">拼音</param>
        /// <param name="zgxb">性别</param>
        /// <param name="ksdm">科室代码</param>
        /// <param name="bqdm">病区代码</param>
        /// <param name="ysjb">医师级别（主任医师:2000/副主任医师:2001/主治医师:2002/住院医师:2003）</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_Employee> GetEmployee(string zgdm, string py, string zgxb, string ksdm, string bqdm, string ysjb)
        {

            List<CP_Employee> emps = null;
            try
            {
                SqlParameter[] paramCollection = new SqlParameter[7];
                paramCollection[0] = (new SqlParameter("@Yxjl", "1"));//1表示有效

                paramCollection[1] = (new SqlParameter("@Zgdm", zgdm));
                paramCollection[2] = (new SqlParameter("@py", py));
                paramCollection[3] = (new SqlParameter("@zgxb", zgxb));

                paramCollection[4] = (new SqlParameter("@ksdm", ksdm));

                paramCollection[5] = (new SqlParameter("@bqdm", bqdm));
                paramCollection[6] = (new SqlParameter("@ysjb", ysjb));

                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_Employee", paramCollection, CommandType.StoredProcedure);

                if (dt.Rows.Count > 0)
                {
                    emps = new List<CP_Employee>();
                    CP_Employee emp;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        emp = new CP_Employee();
                        emp.Zgdm = ConvertMy.ToString(dt.Rows[i]["ID"]);
                        emp.Name = ConvertMy.ToString(dt.Rows[i]["Name"]);
                        emp.Py = ConvertMy.ToString(dt.Rows[i]["Py"]);
                        emp.Wb = ConvertMy.ToString(dt.Rows[i]["Wb"]);
                        emp.Zgxb = ConvertMy.ToString(dt.Rows[i]["Sexy"]);
                        emp.Csrq = ConvertMy.ToString(dt.Rows[i]["Birth"]);
                        emp.Hyzk = ConvertMy.ToString(dt.Rows[i]["Marital"]);
                        emp.Sfzh = ConvertMy.ToString(dt.Rows[i]["IDNo"]);
                        emp.Ksdm = ConvertMy.ToString(dt.Rows[i]["DeptId"]);
                        emp.Bqdm = ConvertMy.ToString(dt.Rows[i]["WardID"]);
                        emp.Zglb = ConvertMy.ToShort(dt.Rows[i]["Category"]);
                        emp.Zcdm = ConvertMy.ToString(dt.Rows[i]["JobTitle"]);
                        emp.Cfzh = ConvertMy.ToString(dt.Rows[i]["RecipeID"]);
                        emp.Cfq = ConvertMy.ToShort(dt.Rows[i]["RecipeMark"]);
                        emp.Mzcfq = ConvertMy.ToShort(dt.Rows[i]["NarcosisMark"]);
                        emp.Fzbm = ConvertMy.ToString(dt.Rows[i]["GroupId"]);
                        emp.Ysjb = ConvertMy.ToShort(dt.Rows[i]["Grade"]);
                        emp.Passwd = ConvertMy.ToString(dt.Rows[i]["Passwd"]);
                        emp.Gwdm = ConvertMy.ToString(dt.Rows[i]["JobID"]);
                        emp.Djsj = ConvertMy.ToString(dt.Rows[i]["RegDate"]);
                        emp.Szry = ConvertMy.ToString(dt.Rows[i]["Operator"]);
                        emp.Yxjl = ConvertMy.ToShort(dt.Rows[i]["Valid"]);
                        emp.Memo = ConvertMy.ToString(dt.Rows[i]["Memo"]);
                        emps.Add(emp);

                    }
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);

            }
            return emps;
        }

        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<PE_User> GetEmployeeWard()
        {
            List<PE_User> emps = null;
            try
            {
                string sql = @"select u.id, u.name '姓名',d.name '科室' from Users u inner join Department d on u.deptid =d.ID order by d.name";

                DataTable dt = SqlHelper.ExecuteDataTable(sql);

                if (dt.Rows.Count > 0)
                {
                    emps = new List<PE_User>();
                    PE_User emp;
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        emp = new PE_User();
                        emp.UserID = dt.Rows[i]["id"].ToString();
                        emp.UserName = dt.Rows[i]["姓名"].ToString();
                        emp.UserDept = dt.Rows[i]["科室"].ToString();
                        emps.Add(emp);
                    }
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);

            }
            return emps;
        }

        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<PE_CompleteUser> GetCompleteEmployeeWard()
        {
            List<PE_CompleteUser> emps = new List<PE_CompleteUser>();
            try
            {
                string sql = @"select u.*,d.name '科室' ,ward.Name '病区',datacate.Name '医师级别' 
from Users u 
inner join Department d 
on u.deptid =d.ID
left join Ward ward on ward.ID = u.WardID
LEFT JOIN CP_DataCategoryDetail datacate on datacate.Mxbh = u.Grade AND datacate.Lbbh = 20
order by d.name";

                DataTable dt = SqlHelper.ExecuteDataTable(sql);

                if (dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        PE_CompleteUser emp = new PE_CompleteUser();
                        emp.UserID = dt.Rows[i]["id"].ToString();
                        emp.UserName = dt.Rows[i]["Name"].ToString();
                        emp.IDNo = dt.Rows[i]["IDNo"].ToString();
                        emp.Marital = dt.Rows[i]["Marital"].ToString() == "1" ? "已婚" : "未婚";
                        emp.UserDept = dt.Rows[i]["科室"].ToString();
                        emp.Sexy = dt.Rows[i]["Sexy"].ToString();
                        emp.Py = dt.Rows[i]["Py"].ToString();
                        emp.Ward = dt.Rows[i]["病区"].ToString();
                        emp.DocGrade = dt.Rows[i]["医师级别"].ToString();
                        emp.Wb = dt.Rows[i]["Wb"].ToString();
                        emp.Birth = dt.Rows[i]["Birth"].ToString();
                        emps.Add(emp);
                    }
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);

            }
            return emps;
        }

        #endregion

        #region 报表部分
        /// <summary>
        ///  路径变异原因统计分析
        /// </summary>
        /// <param name="Begintime">开始时间</param>
        /// <param name="Endtime">结束时间</param>
        /// <param name="Ljdm">路径代码</param>
        /// <param name="Dept">科室代码</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        private List<RPT_PathVariation> GetRptPathVariation(string Begintime, string Endtime, string Ljdm, string Dept)
        {
            List<RPT_PathVariation> rpt_pathvariationList = new List<RPT_PathVariation>();

            try
            {


                SqlParameter[] parameters = new SqlParameter[] 
                    {
                         new SqlParameter("@begindate",Begintime),
                         new SqlParameter("@enddate",Endtime),
                         new SqlParameter("@Ljdm",Ljdm),
                         new SqlParameter("@dept",Dept)
                    };


                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_RptPathVariation", parameters, CommandType.StoredProcedure);

                foreach (DataRow row in dataTable.Rows)
                {
                    RPT_PathVariation rpt_path = new RPT_PathVariation();
                    rpt_path.DeptName = row["DeptName"].ToString();
                    rpt_path.PathID = row["PathID"].ToString();
                    rpt_path.PathName = row["PathName"].ToString();
                    rpt_path.VariationName = row["VariationName"].ToString();
                    rpt_path.VariationType = row["VariationType"].ToString();
                    rpt_path.VariationCount = row["VariationCount"].ToString();

                    rpt_pathvariationList.Add(rpt_path);
                }


            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }


            return rpt_pathvariationList;
        }   // zm 8.24 Oracle
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
        private List<RPT_PathVariationDetail> GetRptPathVariationDetail(string Begintime, string Endtime, string Ljmc, string Bymc)
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
where cpvr.ljdm='{0}'
and cpvr.byyy='{1}'
and CONVERT(varchar,cast(cpvr.bysj as datetime ),120 ) >= '{2}'
and CONVERT(varchar,cast(cpvr.bysj as datetime ),120 ) <= '{3}'", Ljmc, Bymc, Begintime, Endtime);

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

        /// <summary>
        /// 临床路径完成率统计
        /// </summary>
        /// <param name="Begintime">开始时间</param>
        /// <param name="Endtime">结束时间</param>
        /// <param name="Ljdm">路径代码</param>
        /// <param name="Dept">科室代码</param>
        /// <param name="period">时间周期（月:1，季度:2，年:3）</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        private RPT_PathFinish GetRptPathFinish(string Begintime, string Endtime, string Ljdm, string Dept, string period)
        {
            RPT_PathFinish rpt_pathfinish = new RPT_PathFinish();

            List<RPT_PathFinishList> rpt_pathfinishList = new List<RPT_PathFinishList>();

            List<Rpt_PathFinishImage> rpt_pathfinishimageList = new List<Rpt_PathFinishImage>();

            try
            {


                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@begindate",Begintime),
                        new SqlParameter("@enddate",Endtime),
                        new SqlParameter("@Ljdm",Ljdm),
                        new SqlParameter("@dept",Dept),
                        new SqlParameter("@period",period)
                    };


                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_RptPathFinish", parameters, CommandType.StoredProcedure);

                try
                {
                    if (dataTable.Rows[0][0].ToString() == "False")
                    {
                        rpt_pathfinish.Message = "统计的时间范围不能超过15个时间单位!";
                        return rpt_pathfinish;
                    }
                }
                catch
                {
                    rpt_pathfinish.Message = "无数据...";
                    return rpt_pathfinish;
                }

                #region 构建报表中列表需要的数据源
                RPT_PathFinishList finishtitle = new RPT_PathFinishList();

                finishtitle.PathID = "路径代码";
                finishtitle.PathName = "路径名称";

                #region 动态将列表的名称添加到实体finishtitle中
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row["rownumber"].ToString() == "1")
                    {
                        //添加周期列，显示为页面显示信息
                        #region 通过循环将需要查询的列表中的列名存入实体中
                        switch (Convert.ToInt32(row["colindex"].ToString()))
                        {
                            case 0:
                                finishtitle.Col_A = row["period_name"].ToString();
                                break;
                            case 1:
                                finishtitle.Col_B = row["period_name"].ToString();
                                break;
                            case 2:
                                finishtitle.Col_C = row["period_name"].ToString();
                                break;
                            case 3:
                                finishtitle.Col_D = row["period_name"].ToString();
                                break;
                            case 4:
                                finishtitle.Col_E = row["period_name"].ToString();
                                break;
                            case 5:
                                finishtitle.Col_F = row["period_name"].ToString();
                                break;
                            case 6:
                                finishtitle.Col_G = row["period_name"].ToString();
                                break;
                            case 7:
                                finishtitle.Col_H = row["period_name"].ToString();
                                break;
                            case 8:
                                finishtitle.Col_I = row["period_name"].ToString();
                                break;
                            case 9:
                                finishtitle.Col_J = row["period_name"].ToString();
                                break;
                            case 10:
                                finishtitle.Col_K = row["period_name"].ToString();
                                break;
                            case 11:
                                finishtitle.Col_L = row["period_name"].ToString();
                                break;
                            case 12:
                                finishtitle.Col_M = row["period_name"].ToString();
                                break;
                            case 13:
                                finishtitle.Col_N = row["period_name"].ToString();
                                break;
                            case 14:
                                finishtitle.Col_O = row["period_name"].ToString();
                                break;
                            default: break;
                        }
                        #endregion
                    }
                }
                rpt_pathfinishList.Add(finishtitle);
                #endregion
                int rowcount = Convert.ToInt32(dataTable.Rows[dataTable.Rows.Count - 1]["rownumber"].ToString());
                int colcount = Convert.ToInt32(dataTable.Rows[dataTable.Rows.Count - 1]["colindex"].ToString()) + 1;

                //循环添加行信息
                for (int i = 0; i < rowcount; i++)
                {
                    RPT_PathFinishList finishlist = new RPT_PathFinishList();

                    finishlist.PathID = dataTable.Rows[i]["ljdm"].ToString();
                    finishlist.PathName = dataTable.Rows[i]["Name"].ToString();
                    //循环添加列记录
                    for (int j = 0; j < colcount; j++)
                    {
                        #region 通过循环将表中数据加载到实体中
                        switch (j)
                        {
                            case 0:
                                finishlist.Col_A = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 1:
                                finishlist.Col_B = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 2:
                                finishlist.Col_C = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 3:
                                finishlist.Col_D = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 4:
                                finishlist.Col_E = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 5:
                                finishlist.Col_F = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 6:
                                finishlist.Col_G = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 7:
                                finishlist.Col_H = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 8:
                                finishlist.Col_I = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 9:
                                finishlist.Col_J = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 10:
                                finishlist.Col_K = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 11:
                                finishlist.Col_L = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 12:
                                finishlist.Col_M = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 13:
                                finishlist.Col_N = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 14:
                                finishlist.Col_O = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            default: break;
                        }
                        #endregion

                    }
                    rpt_pathfinishList.Add(finishlist);
                }
                #endregion

                #region 构建报表中现状图需要的数据源
                foreach (DataRow row in dataTable.Rows)
                {

                    Rpt_PathFinishImage path_image = new Rpt_PathFinishImage();
                    path_image.PathID = row["ljdm"].ToString();
                    path_image.PathName = row["Name"].ToString();
                    path_image.PeriodName = row["period_name"].ToString();
                    path_image.FinishCount = row["finishcount"].ToString();
                    path_image.TotalCount = row["totalcount"].ToString();
                    path_image.Rate = row["rate"].ToString();
                    path_image.Mess = row["mess"].ToString();

                    rpt_pathfinishimageList.Add(path_image);
                }
                #endregion

                rpt_pathfinish.PathFinishImage = rpt_pathfinishimageList;
                rpt_pathfinish.PathFinishList = rpt_pathfinishList;

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }


            return rpt_pathfinish;
        }   // zm 8.24 Oracle

        [OperationContract]
        [FaultContract(typeof(LoginException))]
        private List<RPT_PathFinishDetail> GetRptPathFinishDetail(string Ljmc, string Begintime, string Endtime)
        {
            List<RPT_PathFinishDetail> rptdetaillist = new List<RPT_PathFinishDetail>();

            try
            {
                string strSql = string.Format(@"select ip.name 患者姓名, dep.name 科室,war.name 病区 , dia.name 诊断,
us.name 床位医师,cpcp.name 路径名称,cpipp.jrsj 入径时间,
case cpipp.wcsj
when isnull(cpipp.wcsj,0)  then cpipp.wcsj else '未完成' end
as 完成时间,
case cpipp.tcsj
when isnull(cpipp.tcsj,0)  then cpipp.tcsj else '未退出' end
as 退出时间,
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
where cpcp.ljdm='{0}'
and cpipp.jrsj>= '{1}'
and cpipp.jrsj<= '{2}'
order by cpipp.jrsj asc", Ljmc, Begintime, Endtime);



                DataTable dt = SqlHelper.ExecuteDataTable(strSql);//.ExecuteDataTable("usp_CP_RptPathStatistics", parameters, CommandType.StoredProcedure);

                foreach (DataRow dr in dt.Rows)
                {
                    RPT_PathFinishDetail rptDetail = new RPT_PathFinishDetail();
                    rptDetail.PatientName = dr["患者姓名"].ToString();
                    rptDetail.PathName = dr["路径名称"].ToString();
                    rptDetail.Diagnosis = dr["诊断"].ToString();
                    rptDetail.Dept = dr["科室"].ToString();
                    rptDetail.Ward = dr["病区"].ToString();
                    rptDetail.Doctor = dr["床位医师"].ToString();
                    rptDetail.InPathTime = dr["入径时间"].ToString();
                    rptDetail.FinishPathTime = dr["完成时间"].ToString();
                    rptDetail.QuitPathTime = dr["退出时间"].ToString();
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

        #endregion

        #region 角色表处理 PE_Role

        /// <summary>
        /// 获取角色列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<PE_Role> GetPE_RoleList()
        {
            List<PE_Role> list = new List<PE_Role>();
            try
            {
                string sql = "SELECT pe_role.RoleCode,pe_role.RoleName FROM PE_Role pe_role";

                DataTable dt = SqlHelper.ExecuteDataTable(sql);


                foreach (DataRow row in dt.Rows)
                {
                    PE_Role role = new PE_Role();
                    role.RoleCode = row["RoleCode"].ToString();
                    role.RoleName = row["RoleName"].ToString();
                    list.Add(role);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        /// <summary>
        /// 插入角色信息
        /// </summary>
        /// <param name="RoleCode">角色编号</param>
        /// <param name="RoleName">角色名称</param>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage InsertPE_Role(string RoleCode, string RoleName)
        {
            SQLMessage sqlmess = new SQLMessage();
            try
            {
                string strSql = string.Format(@"SELECT pe_role.RoleCode,pe_role.RoleName FROM PE_Role pe_role where pe_role.RoleCode = '{0}'", RoleCode);

                DataTable dt = SqlHelper.ExecuteDataTable(strSql);

                if (dt.Rows.Count > 0)
                {
                    sqlmess.IsSucceed = false;
                    sqlmess.Message = "该角色代码已存在！";
                    return sqlmess;
                }

                strSql = string.Format(@"INSERT into PE_Role (RoleCode, RoleName ) VALUES  ( '{0}','{1}' ) ", RoleCode, RoleName);
                SqlHelper.ExecuteNoneQuery(strSql);

                sqlmess.IsSucceed = true;
                sqlmess.Message = "角色新增成功！";
                return sqlmess;


            }
            catch (Exception ex)
            {
                ThrowException(ex); sqlmess.IsSucceed = false;
                sqlmess.Message = "角色新增失败！";
                return sqlmess;
            }
            return null;
        }

        /// <summary>
        /// 修改功能信息
        /// </summary>
        /// <param name="FunCode">节点编号</param>
        /// <param name="FunCodeFather">父节点编号</param>
        /// <param name="FunName">节点名称</param>
        /// <param name="FunURL">节点URL</param>
        /// <returns>执行结果</returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage UpdatePE_Role(string RoleCode, string RoleName)
        {
            SQLMessage sqlmess = new SQLMessage();
            try
            {
                string strSql = string.Format(@"UPDATE PE_Role SET
                                                    RoleName = '{0}'
                                                  WHERE RoleCode ='{1}' ",
                                                          RoleName,
                                                          RoleCode);
                SqlHelper.ExecuteNoneQuery(strSql);

                sqlmess.IsSucceed = true;
                sqlmess.Message = "角色修改成功！";
                return sqlmess;



            }
            catch (Exception ex)
            {
                ThrowException(ex); sqlmess.IsSucceed = false;
                sqlmess.Message = "角色修改失败！";
                return sqlmess;
            }

        }


        /// <summary>
        /// 删除角色信息（需要调用事物，同事讲角色对应的权限删除）
        /// </summary>
        /// <param name="FunCode">功能编码</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage DelPE_Role(string RoleCode)
        {


            SQLMessage sqlmess = new SQLMessage();
            try
            {
                //判断是否有功能权限
                string strSql = string.Format(@"SELECT 1 FROM PE_RoleFun where RoleCode = '{0}'", RoleCode);

                DataTable dt = SqlHelper.ExecuteDataTable(strSql);

                if (dt.Rows.Count > 0)
                {
                    sqlmess.IsSucceed = false;
                    sqlmess.Message = "该角色配有功能权限，请先删除对应功能！";
                    return sqlmess;
                }

                //判断是否有用户配有该权限
                strSql = string.Format(@"SELECT 1 FROM PE_UserRole where RoleCode = '{0}'", RoleCode);

                dt = SqlHelper.ExecuteDataTable(strSql);

                if (dt.Rows.Count > 0)
                {
                    sqlmess.IsSucceed = false;
                    sqlmess.Message = "该角色下有对应用户，请先删除用户角色！";
                    return sqlmess;
                }

                strSql = string.Format(@"DELETE PE_Role WHERE RoleCode = '{0}'", RoleCode);

                SqlHelper.ExecuteNoneQuery(strSql);

                sqlmess.IsSucceed = true;
                sqlmess.Message = "删除角色成功！";
                return sqlmess;


            }
            catch (Exception ex)
            {
                ThrowException(ex); sqlmess.IsSucceed = false;
                sqlmess.Message = "删除角色失败！";
                return sqlmess;
            }

        }

        /// <summary>
        /// 根据角色编号获取该角色拥有的功能权限
        /// </summary>
        /// <param name="RoleCode"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<PE_RoleFun> GetPE_RoleFunList(string RoleCode)
        {
            List<PE_RoleFun> list = new List<PE_RoleFun>();
            try
            {
                string sql = string.Format("SELECT rolefun.RoleCode,rolefun.FunCode FROM PE_RoleFun rolefun WHERE rolefun.RoleCode = '{0}'", RoleCode);

                DataTable dt = SqlHelper.ExecuteDataTable(sql);

                foreach (DataRow row in dt.Rows)
                {
                    PE_RoleFun role = new PE_RoleFun();
                    role.RoleCode = row["RoleCode"].ToString();
                    role.FunCode = row["FunCode"].ToString();
                    list.Add(role);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        #endregion

        #region 功能表处理 PE_Fun
        /// <summary>
        /// 获取功能列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<PE_Fun> GetPE_FunList(string key1, string key2)
        {

            List<PE_Fun> list = new List<PE_Fun>();
            try
            {
                string sql;//= "SELECT fun.FunCode,fun.FunCodeFather,fun.FunName,fun.FunURL FROM PE_Fun fun order by fun.FunCode ";
                if (key1 == key2 && key1 == "" || key1 == null)
                {
                    sql = @"SELECT fun.FunCode,fun.FunCodeFather,fun.FunName,fun.FunURL,fun2.FunName FunFatherName  FROM PE_Fun fun
                            left join dbo.PE_Fun fun2 on fun.FunCodeFather is not null and fun.FunCodeFather = fun2.FunCode
                                order by fun.FunCode ";
                }
                else if (key1 == "" && key2 != "")
                {
                    sql = string.Format(@"SELECT fun.FunCode,fun.FunCodeFather,fun.FunName,fun.FunURL,fun2.FunName FunFatherName  FROM PE_Fun fun
                            left join dbo.PE_Fun fun2 on fun.FunCodeFather is not null and fun.FunCodeFather = fun2.FunCode  where fun.FunURL like '%{0}%' order by fun.FunCode", key2);
                }
                else if (key1 != "" && key2 == "")
                {
                    sql = string.Format(@"SELECT fun.FunCode,fun.FunCodeFather,fun.FunName,fun.FunURL,fun2.FunName FunFatherName  FROM PE_Fun fun
                            left join dbo.PE_Fun fun2 on fun.FunCodeFather is not null and fun.FunCodeFather = fun2.FunCode where fun.FunName like '%{0}%' order by fun.FunCode ", key1);
                }
                else
                {
                    sql = string.Format(@"SELECT fun.FunCode,fun.FunCodeFather,fun.FunName,fun.FunURL,fun2.FunName FunFatherName  FROM PE_Fun fun
                            left join dbo.PE_Fun fun2 on fun.FunCodeFather is not null and fun.FunCodeFather = fun2.FunCode where fun.FunName like '%{0}%' and fun.FunURL like '%{1}%' order by fun.FunCode ", key1, key2);
                }
                DataTable dt = SqlHelper.ExecuteDataTable(sql);


                foreach (DataRow row in dt.Rows)
                {
                    PE_Fun role = new PE_Fun();
                    role.FunCode = row["FunCode"].ToString();
                    role.FunCodeFather = row["FunCodeFather"].ToString();
                    role.Fa_FunName = row["FunFatherName"].ToString();
                    role.FunName = row["FunName"].ToString();
                    role.FunURL = row["FunURL"].ToString();
                    list.Add(role);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        /// <summary>
        /// 获取父功能列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<PE_Fun> GetPE_FunFatherList()
        {
            List<PE_Fun> list = new List<PE_Fun>();
            try
            {
                string sql = "SELECT fun.FunCode,fun.FunCodeFather,fun.FunName,fun.FunURL FROM PE_Fun fun where (fun.FunCodeFather IS NULL or fun.FunCodeFather = '') ";

                DataTable dt = SqlHelper.ExecuteDataTable(sql);


                foreach (DataRow row in dt.Rows)
                {
                    PE_Fun role = new PE_Fun();
                    role.FunCode = row["FunCode"].ToString();
                    role.FunCodeFather = row["FunCodeFather"].ToString();
                    role.FunName = row["FunName"].ToString();
                    role.FunURL = row["FunURL"].ToString();
                    list.Add(role);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        /// <summary>
        /// 获取子功能列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<PE_Fun> GetPE_FunSunList()
        {
            List<PE_Fun> list = new List<PE_Fun>();
            try
            {
                string sql = "SELECT fun.FunCode,fun.FunCodeFather,fun.FunName,fun.FunURL FROM PE_Fun fun where (fun.FunCodeFather IS not NULL or fun.FunCodeFather <> '') ";

                DataTable dt = SqlHelper.ExecuteDataTable(sql);


                foreach (DataRow row in dt.Rows)
                {
                    PE_Fun role = new PE_Fun();
                    role.FunCode = row["FunCode"].ToString();
                    role.FunCodeFather = row["FunCodeFather"].ToString();
                    role.FunName = row["FunName"].ToString();
                    role.FunURL = row["FunURL"].ToString();
                    list.Add(role);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        /// <summary>
        /// 插入功能信息
        /// </summary>
        /// <param name="FunCode">节点编号</param>
        /// <param name="FunCodeFather">父节点编号</param>
        /// <param name="FunName">节点名称</param>
        /// <param name="FunURL">节点URL</param>
        /// <returns>执行结果</returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage InsertPE_Fun(string FunCode, string FunCodeFather, string FunName, string FunURL)
        {
            SQLMessage sqlmess = new SQLMessage();
            try
            {
                string strSql = string.Format(@"SELECT fun.FunCode,fun.FunCodeFather,fun.FunName,fun.FunURL FROM PE_Fun fun where fun.FunCode = '{0}'", FunCode);

                DataTable dt = SqlHelper.ExecuteDataTable(strSql);

                if (dt.Rows.Count > 0)
                {
                    sqlmess.IsSucceed = false;
                    sqlmess.Message = "已经存在该功能代码，请更改功能代码！";
                    return sqlmess;
                }

                strSql = string.Format(@"INSERT INTO PE_Fun
                                                        ( FunCode ,
                                                          FunName ,
                                                          FunCodeFather ,
                                                          FunURL
                                                        )
                                                VALUES  ( '{0}' , 
                                                          '{1}',  
                                                          CASE WHEN '{2}' = '' THEN NULL ELSE '{2}' END  ,  
                                                          CASE WHEN '{3}' = '' THEN NULL ELSE '{3}' END     
                                                        )",
                                                          FunCode,
                                                          FunName,
                                                          FunCodeFather,
                                                          FunURL);
                SqlHelper.ExecuteNoneQuery(strSql);

                sqlmess.IsSucceed = true;
                sqlmess.Message = "功能新增成功!";
                return sqlmess;

            }
            catch (Exception ex)
            {
                ThrowException(ex); sqlmess.IsSucceed = false;
                sqlmess.Message = "功能新增失败!";
                return sqlmess;
            }
            return null;
        }


        /// <summary>
        /// 修改功能信息
        /// </summary>
        /// <param name="FunCode">节点编号</param>
        /// <param name="FunCodeFather">父节点编号</param>
        /// <param name="FunName">节点名称</param>
        /// <param name="FunURL">节点URL</param>
        /// <returns>执行结果</returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage UpdatePE_Fun(string FunCode, string FunCodeFather, string FunName, string FunURL)
        {
            SQLMessage sqlmess = new SQLMessage();
            try
            {
                string strSql = string.Format(@"UPDATE PE_Fun SET
                                                  FunName ='{1}',
                                                  FunCodeFather = (CASE WHEN '{2}' = '' THEN NULL ELSE '{2}' END ),
                                                  FunURL = (CASE WHEN '{3}' = '' THEN NULL ELSE '{3}' END )
                                                  WHERE FunCode = '{0}'",
                                                          FunCode,
                                                          FunName,
                                                          FunCodeFather,
                                                          FunURL);
                SqlHelper.ExecuteNoneQuery(strSql);

                sqlmess.IsSucceed = true;
                sqlmess.Message = "功能修改成功!";
                return sqlmess;

            }
            catch (Exception ex)
            {
                ThrowException(ex); sqlmess.IsSucceed = false;
                sqlmess.Message = "功能修改失败!";
                return sqlmess;
            }
            return null;
        }


        /// <summary>
        /// 删除功能信息
        /// </summary>
        /// <param name="FunCode">功能编码</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage DelPE_Fun(string FunCode)
        {

            SQLMessage sqlmess = new SQLMessage();
            try
            {
                //判断是否有子节点
                string strSql = string.Format(@"SELECT 1 FROM PE_Fun fun where fun.FunCodeFather = '{0}'", FunCode);

                DataTable dt = SqlHelper.ExecuteDataTable(strSql);

                if (dt.Rows.Count > 0)
                {
                    sqlmess.IsSucceed = false;
                    sqlmess.Message = "该功能下存在子功能，请先删除子功能！";
                    return sqlmess;
                }

                strSql = string.Format(@"SELECT 1 FROM PE_RoleFun fun where fun.FunCode = '{0}'", FunCode);

                dt = SqlHelper.ExecuteDataTable(strSql);

                if (dt.Rows.Count > 0)
                {
                    sqlmess.IsSucceed = false;
                    sqlmess.Message = "该功能已经分配权限，请先删除对应权限！";
                    return sqlmess;
                }

                strSql = string.Format(@"DELETE PE_Fun WHERE FunCode = '{0}'", FunCode);
                SqlHelper.ExecuteNoneQuery(strSql);

                sqlmess.IsSucceed = true;
                sqlmess.Message = "功能删除成功!";
                return sqlmess;

            }
            catch (Exception ex)
            {
                ThrowException(ex); sqlmess.IsSucceed = false;
                sqlmess.Message = "功能删除失败!";
                return sqlmess;
            }
            return null;


        }

        #endregion

        #region 角色功能维护 PE_RoleFun

        /// <summary>
        /// 获子集功能取功能列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<PE_Fun> GetSunPE_FunList()
        {
            List<PE_Fun> list = new List<PE_Fun>();
            try
            {
                string sql = @"SELECT sun_fun.FunCode,sun_fun.FunCodeFather,fa_fun.FunName Fa_FunName,sun_fun.FunName,sun_fun.FunURL 
                                FROM PE_Fun sun_fun,PE_Fun fa_fun
                                    WHERE sun_fun.FunCodeFather = fa_fun.FunCode 
                                    AND (fa_fun.FunCodeFather IS NULL or fa_fun.FunCodeFather ='') ";

                DataTable dt = SqlHelper.ExecuteDataTable(sql);


                foreach (DataRow row in dt.Rows)
                {
                    PE_Fun role = new PE_Fun();
                    role.FunCode = row["FunCode"].ToString();
                    role.FunCodeFather = row["FunCodeFather"].ToString();
                    role.FunName = row["FunName"].ToString();
                    role.FunURL = row["FunURL"].ToString();
                    role.Fa_FunName = row["Fa_FunName"].ToString();
                    list.Add(role);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }


        /// <summary>
        /// 根据角色编码获取角色功能信息
        /// </summary>
        /// <param name="RoleCode">角色编码</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<PE_Fun> GetPE_FunListByRoleCode(string RoleCode)
        {
            List<PE_Fun> list = new List<PE_Fun>();
            try
            {
                string sql = string.Format(@"SELECT  sun_fun.FunCode ,
                                                        sun_fun.FunCodeFather ,
                                                        fa_fun.FunName Fa_FunName ,
                                                        sun_fun.FunName ,
                                                        sun_fun.FunURL ,
                                                        isnull(( SELECT    1
                                                                      FROM      PE_RoleFun rolefun
                                                                      WHERE     rolefun.FunCode = sun_fun.FunCode
                                                                                AND RoleCode = '{0}'
                                                                    ),'0') ISCheck
                                                FROM    PE_Fun sun_fun ,
                                                        PE_Fun fa_fun
                                                WHERE   sun_fun.FunCodeFather = fa_fun.FunCode
                                                        AND (fa_fun.FunCodeFather IS NULL or fa_fun.FunCodeFather = '')", RoleCode);

                DataTable dt = SqlHelper.ExecuteDataTable(sql);


                foreach (DataRow row in dt.Rows)
                {
                    PE_Fun role = new PE_Fun();
                    role.FunCode = row["FunCode"].ToString();
                    role.FunCodeFather = row["FunCodeFather"].ToString();
                    role.FunName = row["FunName"].ToString();
                    role.FunURL = row["FunURL"].ToString();
                    role.Fa_FunName = row["Fa_FunName"].ToString();
                    role.ISCheck = Convert.ToInt32(row["ISCheck"].ToString());
                    list.Add(role);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        /// <summary>
        /// 修改角色对应的功能权限
        /// </summary>
        /// <param name="list">PE_RoleFun 实体</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage InsertPE_RoleFun(List<PE_RoleFun> list)
        {
            SQLMessage sqlmess = new SQLMessage();
            using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            {
                SqlTransaction sqlTrans = null;
                try
                {
                    //删除角色功能信息
                    string strSql = string.Format(@"DELETE PE_RoleFun WHERE RoleCode = '{0}'", list[0].RoleCode);

                    SqlHelper.ExecuteNoneQuery(strSql);

                    //循环添加角色功能信息
                    foreach (PE_RoleFun rolefun in list)
                    {
                        strSql = string.Format(@"INSERT into PE_RoleFun ( RoleCode, FunCode ) VALUES  ( '{0}', '{1}' )",
                                                                                rolefun.RoleCode,
                                                                                rolefun.FunCode);

                        SqlHelper.ExecuteNoneQuery(strSql);
                    }


                }
                catch (Exception ex)
                {

                    ThrowException(ex);
                }

                sqlmess.IsSucceed = true;
                sqlmess.Message = "角色功能权限修改成功！";
                return sqlmess;
            }

        }  // zm 8.24 Oracle

        /// <summary>
        /// 删除角色对应的功能
        /// </summary>
        /// <param name="RoleCode">角色编号</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage DelPE_RoleFun(string RoleCode)
        {
            SQLMessage sqlmess = new SQLMessage();
            try
            {
                string strSql = string.Format(@"DELETE PE_RoleFun WHERE RoleCode = '{0}'", RoleCode);
                SqlHelper.ExecuteNoneQuery(strSql);

                sqlmess.IsSucceed = true;
                sqlmess.Message = "角色功能删除成功！";
                return sqlmess;


            }
            catch (Exception ex)
            {
                ThrowException(ex); sqlmess.IsSucceed = false;
                sqlmess.Message = "角色功能删除失败！";
                return sqlmess;
            }
            return null;
        }


        #endregion

        #region 编码维护

        /// <summary>
        /// 查询检查项目编码grid列表中显示的数据
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<PE_CodeList> GetPE_CodeList()
        {
            List<PE_CodeList> list = new List<PE_CodeList>();

            try
            {
                string sql = @"select cped.jcbm,cped.fjd,cped.jcmc,cped.mcsx,cped.py,cped.wb,cped.bz from CP_ExamDictionary cped order by cped.jlxh asc";
                DataTable dt = SqlHelper.ExecuteDataTable(sql);
                foreach (DataRow row in dt.Rows)
                {
                    PE_CodeList code = new PE_CodeList();
                    code.Jcbm = row["jcbm"].ToString();
                    code.Fjd = row["fjd"].ToString();
                    code.Jcmc = row["jcmc"].ToString();
                    code.Mcsx = row["mcsx"].ToString();
                    code.Py = row["py"].ToString();
                    code.Wb = row["wb"].ToString();
                    code.Bz = row["bz"].ToString();
                    list.Add(code);
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        /// <summary>
        ///  判断是否存在
        /// </summary>
        /// <param name="Flbm"></param>
        /// <param name="Jcmc"></param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal CheckAddCodeBeing(String Jcbm)
        {
            try
            {
                String sqlStr;
                //if (Jcmc != "")
                //{
                //    sqlStr = "SELECT * FROM  CP_ExamDictionary WHERE Jcbm ='" + Jcbm + "' AND Jcmc = '" + Jcmc + "' order by jcbm ";
                //}
                //else
                //{
                sqlStr = "SELECT * FROM  CP_ExamDictionary WHERE Jcbm ='" + Jcbm + "'";
                //}
                DataSet ds = SqlHelper.ExecuteDataSet(sqlStr);
                return ds.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return -1;
        }

        /// <summary>
        /// 添加检查项目编码
        /// </summary>
        /// <returns>集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public bool InsertCodeList(string Jcbm, string Jcmc, string Fjd, string Mcsx, string Wb, string Bz)
        {

            try
            {
                string sql = string.Format(@"insert into CP_ExamDictionary (jcbm,jcmc,fjd,mcsx,py,wb,bz) values ('{0}','{1}','{2}','{3}',dbo.fun_getPY('{1}'),'{4}','{5}')", Jcbm, Jcmc, Fjd, Mcsx, Wb, Bz);
                SqlHelper.ExecuteNoneQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return false;
        }

        /// <summary>
        /// 修改检查项目编码
        /// </summary>
        /// <returns>集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public bool UpdateCodeList(string Jcbm, string Jcmc, string Fjd, string Mcsx, string Bz)
        {
            try
            {
                string sql = string.Format(@"UPDATE dbo.CP_ExamDictionary SET Fjd='{0}', Jcmc='{1}', Mcsx='{2}',Py=dbo.fun_getPY('{1}'), bz='{3}' WHERE Jcbm='{4}'", Fjd, Jcmc, Mcsx, Bz, Jcbm);
                SqlHelper.ExecuteNoneQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return false;
        }

        /// <summary>
        /// 删除检查项目编码
        /// </summary>
        /// <returns>集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public bool DelCodeList(string Jcbm)
        {
            try
            {
                string sql = string.Format(@"DELETE FROM dbo.CP_ExamDictionary WHERE Jcbm='{0}'", Jcbm);
                SqlHelper.ExecuteNoneQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return false;
        }
        #endregion


        #region 护理项目维护

        /// <summary>
        /// 查询护理项目grid列表中显示的数据
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<PE_Hlxm> GetPE_HlxmList()
        {
            List<PE_Hlxm> list = new List<PE_Hlxm>();

            try
            {
                string sql = @"select cped.Xmxh,cped.Name ,cped.OrderValue ,cped.Yxjl ,cped.Create_Time,cped.Create_User ,cped.Cancel_Time ,cped.Cancel_User  from CP_NurExecItem cped order by cped.OrderValue asc";
                DataTable dt = SqlHelper.ExecuteDataTable(sql);
                foreach (DataRow row in dt.Rows)
                {
                    PE_Hlxm hlxm = new PE_Hlxm();
                    hlxm.Xmxh = row["Xmxh"].ToString();
                    hlxm.Name = row["Name"].ToString();
                    hlxm.OrderValue = row["OrderValue"].ToString();
                    hlxm.Yxjl = row["Yxjl"].ToString();
                    hlxm.Create_Time = row["Create_Time"].ToString();
                    hlxm.Create_User = row["Create_User"].ToString();
                    hlxm.Cancel_Time = row["Cancel_Time"].ToString();
                    hlxm.Cancel_User = row["Cancel_User"].ToString();
                    list.Add(hlxm);
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        /// <summary>
        ///  判断是否存在
        /// </summary>
        /// <param name="Flbm"></param>
        /// <param name="Jcmc"></param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal CheckAddHlxmCodeBeing(String Xmxh)
        {
            try
            {
                String sqlStr;
                sqlStr = "SELECT * FROM  CP_NurExecItem WHERE Xmxh ='" + Xmxh + "'";
                DataSet ds = SqlHelper.ExecuteDataSet(sqlStr);
                return ds.Tables[0].Rows.Count;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return -1;
        }

        /// <summary>
        /// 添加检查项目编码
        /// </summary>
        /// <returns>集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public bool InsertHlxmCodeList(string Xmxh, string Name, string OrderValue, string Yxjl, string Create_User)
        {

            try
            {
                string sql = string.Format(@"insert into CP_NurExecItem (Xmxh,Name,OrderValue,Yxjl,Create_Time,Create_User) values ('{0}','{1}','{2}',{3},convert(varchar(19), getdate(), 120) ,'{4}')", Xmxh, Name, OrderValue, int.Parse(Yxjl), Create_User);
                SqlHelper.ExecuteNoneQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return false;
        }

        /// <summary>
        /// 修改护理项目编码
        /// </summary>
        /// <returns>集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public bool UpdateHlxmCodeList(string Xmxh, string Name, string OrderValue, string Yxjl, string Cancel_User)
        {
            try
            {
                string sql = string.Format(@"UPDATE dbo.CP_NurExecItem SET Name='{0}', OrderValue='{1}', Yxjl='{2}',Cancel_Time=convert(varchar(19), getdate(), 120), Cancel_User='{3}' WHERE Xmxh='{4}'", Name, OrderValue, Yxjl, Cancel_User, Xmxh);
                SqlHelper.ExecuteNoneQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return false;
        }

        /// <summary>
        /// 删除护理项目编码
        /// </summary>
        /// <returns>集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public bool DelHlxmCodeList(string Xmxh)
        {
            try
            {
                string sql = string.Format(@"DELETE FROM dbo.CP_NurExecItem WHERE Xmxh='{0}'", Xmxh);
                SqlHelper.ExecuteNoneQuery(sql);
                return true;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return false;
        }
        #endregion


        #region 用户角色表维护 PE_UserRole

        /// <summary>
        /// 查询用户角色维护模块中grid列表中显示的数据
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<PE_UserRoleList> GetPE_UserRoleList()
        {
            List<PE_UserRoleList> list = new List<PE_UserRoleList>();
            try
            {
                string sql = @"SELECT emp.ID,emp.Name,role.RoleName RoleName,role.RoleCode RoleCode, emp.Sexy,dept.Name AS deptname,ward.Name bqName,
                                datacate.Name Ysjb FROM users emp 
                                INNER JOIN PE_UserRole userrole on emp.ID = userrole.UserID
                                LEFT JOIN Department dept on dept.ID = emp.DeptId
                                LEFT JOIN Ward ward on ward.ID = emp.WardID
                                LEFT JOIN CP_DataCategoryDetail datacate on datacate.Mxbh = emp.Grade AND datacate.Lbbh = 20
                                LEFT JOIN PE_Role role on role.RoleCode = userrole.RoleCode
                                WHERE emp.Valid = 1";

                DataTable dt = SqlHelper.ExecuteDataTable(sql);


                foreach (DataRow row in dt.Rows)
                {
                    PE_UserRoleList role = new PE_UserRoleList();
                    role.Zgdm = row["ID"].ToString();
                    role.Name = row["Name"].ToString();
                    role.RoleName = row["RoleName"].ToString();
                    role.RoleCode = row["RoleCode"].ToString();
                    role.Zgxb = row["Sexy"].ToString();
                    role.DeptName = row["deptname"].ToString();
                    role.BqName = row["bqName"].ToString();
                    role.Ysjb = row["Ysjb"].ToString();
                    list.Add(role);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        /// <summary>
        /// 根据传入的UserID获取员工的角色
        /// </summary>
        /// <param name="UserID">UserID</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<PE_Role> GetUserRoleByUserID(string UserID)
        {
            List<PE_Role> list = new List<PE_Role>();
            try
            {
                string sql = string.Format(@"select
                                                                       PE_Role.RoleCode ,
                                                                       PE_Role.RoleName ,
                                                                       isnull(( select 1 from PE_UserRole
                                                                                where PE_UserRole.RoleCode = PE_Role.RoleCode
                                                                                   and PE_UserRole.UserID = '{0}'
                                                                              ) , '0') ISCheck
                                                                    from
                                                                       PE_Role 
                                                                       order by PE_Role.RoleCode", UserID);

                DataTable dt = SqlHelper.ExecuteDataTable(sql);


                foreach (DataRow row in dt.Rows)
                {
                    PE_Role role = new PE_Role();
                    role.RoleCode = row["RoleCode"].ToString();
                    role.RoleName = row["RoleName"].ToString();
                    role.IsCheck = Convert.ToInt32(row["ISCheck"].ToString());
                    list.Add(role);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }


        /// <summary>
        /// 插入用户角色信息
        /// </summary>
        /// <param name="UserID">用户编号</param>
        /// <param name="list">用户角色实体列表</param>
        /// <returns>执行结果</returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage InsertPE_UserRole(string UserID, List<PE_Role> list)
        {
            SQLMessage sqlmess = new SQLMessage();
            using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            {
                SqlTransaction sqlTrans = null;
                try
                {

                    //删除用户角色信息
                    string strSql = string.Format(@"delete from PE_UserRole where UserID = '{0}'", UserID);
                    ;

                    SqlHelper.ExecuteNoneQuery(strSql);

                    //循环添加用户角色信息
                    foreach (PE_Role role in list)
                    {
                        strSql = string.Format(@"INSERT INTO PE_UserRole
                                                        ( UserID ,
                                                          RoleCode
                                                        )
                                                VALUES  ( '{0}' , 
                                                          '{1}' 
                                                        )",
                                                          UserID,
                                                          role.RoleCode);

                        SqlHelper.ExecuteNoneQuery(strSql);
                    }


                }
                catch (Exception ex)
                {

                    ThrowException(ex);
                }

                sqlmess.IsSucceed = true;
                sqlmess.Message = "用户角色权限修改成功！";
                return sqlmess;
            }
        }   // zm 8.24 Oracle


        /// <summary>
        /// 用户角色修改
        /// </summary>
        /// <param name="UserID">用户编码</param>
        /// <param name="RoleCode">角色编码</param>
        /// <returns>执行结果</returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage UpdatePE_UserRole(string UserID, string RoleCode)
        {
            SQLMessage sqlmess = new SQLMessage();
            try
            {
                string strSql = string.Format(@"UPDATE PE_UserRole SET
                                                  RoleCode ='{1}' 
                                                  WHERE UserID = '{0}'",
                                                          UserID,
                                                          RoleCode);
                SqlHelper.ExecuteNoneQuery(strSql);

                sqlmess.IsSucceed = true;
                sqlmess.Message = "用户角色修改成功！";
                return sqlmess;

            }
            catch (Exception ex)
            {
                ThrowException(ex); sqlmess.IsSucceed = false;
                sqlmess.Message = "用户角色修改失败！";
                return sqlmess;
            }

        }


        /// <summary>
        /// 删除用户角色
        /// </summary>
        /// <param name="UserID">用户编码</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage DelPE_UserRole(string UserID)
        {
            SQLMessage sqlmess = new SQLMessage();
            try
            {
                string strSql = string.Format(@"DELETE PE_UserRole WHERE UserID = '{0}'", UserID);
                SqlHelper.ExecuteNoneQuery(strSql);

                sqlmess.IsSucceed = true;
                sqlmess.Message = "用户角色删除成功！";
                return sqlmess;

            }
            catch (Exception ex)
            {
                ThrowException(ex); sqlmess.IsSucceed = false;
                sqlmess.Message = "用户角色删除失败！";
                return sqlmess;
            }
            return null;
        }

        #endregion

        #region 医院诊断表中copy到路径诊断表中 CP_PathDiagnosis，CP_Diagnosis
        /// <summary>
        /// 根据输入的查询关键字，获取医院诊断列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_Diagnosis_E> GetCP_DiagnosisList(string keywords)
        {
            List<CP_Diagnosis_E> list = new List<CP_Diagnosis_E>();
            try
            {
                string sql = string.Format(@"SELECT diag.MarkId,ICD,MapID,StandardCode,Name,Py,Wb,TumorID,Statist,InnerCategory,Category,OtherCategroy,Valid,Memo FROM Diagnosis diag 
	                                            WHERE (
		                                                '{0}' = '' 
		                                            OR diag.MarkId LIKE '%{0}%' 
		                                            OR diag.ICD LIKE '%{0}%' 
		                                            OR diag.Name LIKE '%{0}%' 
		                                            OR diag.Py LIKE '%{0}%' 
		                                            OR diag.Wb LIKE '%{0}%')
                                                    and Valid = 1", keywords);

                DataTable dt = SqlHelper.ExecuteDataTable(sql);


                foreach (DataRow row in dt.Rows)
                {
                    CP_Diagnosis_E role = new CP_Diagnosis_E();
                    role.Zdbs = row["MarkId"].ToString();
                    role.Zddm = row["ICD"].ToString();
                    role.Ysdm = row["StandardCode"].ToString();

                    role.Bzdm = row["ICD"].ToString();
                    role.Name = row["Name"].ToString();
                    role.Py = row["Py"].ToString();

                    role.Wb = row["Wb"].ToString();
                    role.Zldm = row["TumorID"].ToString();
                    role.Tjm = row["InnerCategory"].ToString();

                    role.Nbfl = row["Category"].ToString();
                    role.Bzlb = row["OtherCategroy"].ToString();
                    role.Qtlb = row["OtherCategroy"].ToString(); ;

                    role.Yxjl = row["Valid"].ToString(); ;
                    role.Memo = row["Memo"].ToString();
                    list.Add(role);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;

        }

        /// <summary>
        /// 根据输入的查询关键字，获取路径诊断库列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_Diagnosis_E> GetCP_PathDiagnosisList(string keywords)
        {
            List<CP_Diagnosis_E> list = new List<CP_Diagnosis_E>();
            try
            {

                string sql;
                ////(
                //                                        '{0}' is null or
                //                                     
                //                                    OR diag.Zddm LIKE '{0}' 
                //                                    OR diag.Name LIKE '{0}' 
                //                                    OR diag.Py LIKE '{0}' 
                //                                    OR diag.Wb LIKE '{0}')
                //                                    and

                if (keywords == string.Empty || keywords == null)
                {
                    sql = string.Format(@"SELECT diag.Zdbs,Zddm,Ysdm,Bzdm,Name,Py,Wb,Zldm,Tjm,Nbfl,Bzlb,Qtlb,Yxjl,Memo FROM CP_PathDiagnosis diag 
                                          WHERE  Yxjl = 1", keywords);
                }
                else
                {
                    sql = string.Format(@"SELECT diag.Zdbs,Zddm,Ysdm,Bzdm,Name,Py,Wb,Zldm,Tjm,Nbfl,Bzlb,Qtlb,Yxjl,Memo FROM CP_PathDiagnosis diag 
                                          WHERE  Yxjl = 1 and (diag.Zdbs LIKE '%{0}%'  OR diag.Name LIKE '%{0}%'  OR diag.Py LIKE '%{0}%' OR diag.Wb LIKE '%{0}%')", keywords);
                }

                DataTable dt = SqlHelper.ExecuteDataTable(sql);


                foreach (DataRow row in dt.Rows)
                {
                    CP_Diagnosis_E role = new CP_Diagnosis_E();
                    role.Zdbs = row["Zdbs"].ToString();
                    role.Zddm = row["Zddm"].ToString();
                    role.Ysdm = row["Ysdm"].ToString();

                    role.Bzdm = row["Bzdm"].ToString();
                    role.Name = row["Name"].ToString();
                    role.Py = row["Py"].ToString();

                    role.Wb = row["Wb"].ToString();
                    role.Zldm = row["Zldm"].ToString();
                    role.Tjm = row["Tjm"].ToString();

                    role.Nbfl = row["Nbfl"].ToString();
                    role.Bzlb = row["Bzlb"].ToString();
                    role.Qtlb = row["Qtlb"].ToString(); ;

                    role.Yxjl = row["Yxjl"].ToString(); ;
                    role.Memo = row["Memo"].ToString();
                    list.Add(role);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;

        }

        /// <summary>
        /// 根据传入的诊断，将对应的医院诊断编码添加到路径诊断表中
        /// </summary>
        /// <param name="Zdbs"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage CopyCP_DiagnosisToPath(string Zdbs)
        {
            SQLMessage sqlmess = new SQLMessage();
            try
            {
                string strSql = string.Format(@"INSERT INTO CP_PathDiagnosis
                                ( Zdbs, Zddm, Ysdm, Bzdm, Name, Py, Wb, Zldm, Tjm, Nbfl, Bzlb, Qtlb, Yxjl, Memo) 
                      SELECT diag.MarkId, ICD, MapID, StandardCode, Name, Py, Wb, TumorID, Statist, InnerCategory, Category, OtherCategroy, Valid, Memo FROM Diagnosis diag WHERE diag.MarkId = '{0}'",
                                                          Zdbs);
                SqlHelper.ExecuteNoneQuery(strSql);

                sqlmess.IsSucceed = true;
                sqlmess.Message = "已将医院诊断信息成功添加到路径诊断信息表中！";
                return sqlmess;



            }
            catch (Exception ex)
            {
                ThrowException(ex); sqlmess.IsSucceed = false;
                sqlmess.Message = "添加失败！";
                return sqlmess;
            }
            return null;
        }

        /// <summary>
        /// 根据传入的诊断，将对应的路径诊断编码从路径诊断表中删除
        /// </summary>
        /// <param name="Zdbs"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage DelCP_DiagnosisFromPath(string Zdbs)
        {
            SQLMessage sqlmess = new SQLMessage();
            try
            {
                string strSql = string.Format(@"delete CP_PathDiagnosis WHERE Zdbs = '{0}'",
                                                          Zdbs);
                SqlHelper.ExecuteNoneQuery(strSql);

                sqlmess.IsSucceed = true;
                sqlmess.Message = "已从路径诊断库中删除成功！";
                return sqlmess;

            }
            catch (Exception ex)
            {
                ThrowException(ex); sqlmess.IsSucceed = false;
                sqlmess.Message = "删除失败！";
                return sqlmess;
            }
            return null;
        }

        /// <summary>
        /// 根据医院诊断标识判断路径诊断表中是否已经存在该诊断编码信息
        /// </summary>
        /// <param name="Zdbs">诊断标识</param>
        /// <returns>true:已经存在  false:不存在</returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public bool IsHaveCP_PathDiagnosis(string Zdbs)
        {
            try
            {
                string sql = string.Format(@"SELECT Zdbs FROM CP_PathDiagnosis diag 
	                                            WHERE  diag.Zdbs = '{0}' 
                                                    and Yxjl = 1", Zdbs);

                DataTable dt = SqlHelper.ExecuteDataTable(sql);
                if (dt.Rows.Count > 0)
                {
                    return true;
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return false;
        }

        #endregion

        #region 获取下拉框数据

        /// <summary>
        /// 根据传入关键字获取员工信息列表
        /// </summary>
        /// <param name="keywords">模糊查询条件  拼音 五笔  姓名 </param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_Employee> GetCP_EmployeeByKeyWords(string keykwords)
        {
            List<CP_Employee> list = new List<CP_Employee>();
            try
            {
                string sql = string.Format(@"SELECT * FROM Users emp 
                                                WHERE emp.Valid = 1  AND ('{0}' = '' OR emp.Py LIKE '%{0}%' OR emp.Name LIKE '%{0}%' OR emp.Wb LIKE '%{0}%') ", keykwords);

                DataTable dt = SqlHelper.ExecuteDataTable(sql);


                foreach (DataRow row in dt.Rows)
                {
                    CP_Employee emp = new CP_Employee();
                    emp = new CP_Employee();
                    emp.Zgdm = ConvertMy.ToString(row["ID"]);
                    emp.Name = ConvertMy.ToString(row["Name"]);
                    emp.Py = ConvertMy.ToString(row["Py"]);
                    emp.Wb = ConvertMy.ToString(row["Wb"]);
                    emp.Zgxb = ConvertMy.ToString(row["Sexy"]);
                    emp.Csrq = ConvertMy.ToString(row["Birth"]);
                    emp.Hyzk = ConvertMy.ToString(row["Marital"]);
                    emp.Sfzh = ConvertMy.ToString(row["IDNo"]);
                    emp.Ksdm = ConvertMy.ToString(row["DeptId"]);
                    emp.Bqdm = ConvertMy.ToString(row["WardID"]);
                    emp.Zglb = ConvertMy.ToShort(row["Category"]);
                    emp.Zcdm = ConvertMy.ToString(row["JobTitle"]);
                    emp.Cfzh = ConvertMy.ToString(row["RecipeID"]);
                    emp.Cfq = ConvertMy.ToShort(row["RecipeMark"]);
                    emp.Mzcfq = ConvertMy.ToShort(row["NarcosisMark"]);
                    emp.Fzbm = ConvertMy.ToString(row["GroupId"]);
                    emp.Ysjb = ConvertMy.ToShort(row["Grade"]);
                    emp.Passwd = ConvertMy.ToString(row["Passwd"]);
                    emp.Gwdm = ConvertMy.ToString(row["JobID"]);
                    emp.Djsj = ConvertMy.ToString(row["RegDate"]);
                    emp.Szry = ConvertMy.ToString(row["Operator"]);
                    emp.Yxjl = ConvertMy.ToShort(row["Valid"]);
                    emp.Memo = ConvertMy.ToString(row["Memo"]);
                    list.Add(emp);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        #endregion


        /// <summary>
        /// 修改用户密码
        /// </summary>
        /// <param name="UserID">用户编号</param>
        /// <param name="NewPWD">用户新密码</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public SQLMessage ChangePassword(string UserID, string NewPWD)
        {
            SQLMessage sqlmess = new SQLMessage();
            try
            {
                string strSql = string.Format(@"update Users set Passwd = '{0}' where ID = '{1}'",
                                                          NewPWD,
                                                          UserID);
                SqlHelper.ExecuteNoneQuery(strSql);

                sqlmess.IsSucceed = true;
                sqlmess.Message = "密码修改成功！";
                return sqlmess;

            }
            catch (Exception ex)
            {
                ThrowException(ex); sqlmess.IsSucceed = false;
                sqlmess.Message = "密码修改失败！";
                return sqlmess;
            }
            return null;
        }


        /// <summary>
        /// 根据输入的查询关键字，获取医院诊断列表
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_Surgery> GetCP_SurgeryListByKeyword(string keywords)
        {
            List<CP_Surgery> list = new List<CP_Surgery>();
            try
            {
                string sql = string.Format(@"SELECT * FROM CP_Surgery surg 
                                            WHERE (
                                                    '{0}' = '' 
                                                    or surg.Ssdm like '%{0}%'
                                                    or surg.Ysdm like '%{0}%'
                                                    or surg.Bzdm like '%{0}%'
                                                    or surg.Name like '%{0}%'
                                                    or surg.Py like '%{0}%'
                                                    or surg.Wb like '%{0}%'
                                                )
                                                and Yxjl = 1", keywords);

                DataTable dt = SqlHelper.ExecuteDataTable(sql);


                foreach (DataRow row in dt.Rows)
                {
                    CP_Surgery role = new CP_Surgery();
                    role.Ssdm = row["Ssdm"].ToString();
                    role.Ysdm = row["Ysdm"].ToString();
                    role.Bzdm = row["Bzdm"].ToString();

                    role.Name = row["Name"].ToString();
                    role.Py = row["Py"].ToString();
                    role.Wb = row["Wb"].ToString();

                    role.Bzlb = row["Bzlb"].ToString();
                    role.Sslb = Convert.ToInt16(row["Sslb"].ToString());
                    role.Yxjl = Convert.ToInt16(row["Yxjl"].ToString());

                    role.Memo = row["Memo"].ToString();
                    list.Add(role);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;

        }




        /// <summary>
        /// 根据路径代码获取路径信息
        /// </summary>
        /// <param name="keyWords">路径代码</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_ClinicalPathList> GetClinicalPathByLjdm(string strLjdm)
        {

            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@Ljdm",strLjdm)
                    };



                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_ClinicalPathList", parameters, CommandType.StoredProcedure);

                List<CP_ClinicalPathList> clinPathListInfo = new List<CP_ClinicalPathList>();

                foreach (DataRow row in dataTable.Rows)
                {
                    CP_ClinicalPathList cliListInfo = new CP_ClinicalPathList(row["Ljdm"].ToString(), row["Name"].ToString(), row["Ljms"].ToString(),
                        decimal.Parse(row["Zgts"].ToString()), decimal.Parse(row["Jcfy"].ToString()), decimal.Parse(row["Vesion"].ToString()),
                        row["Cjsj"].ToString(), row["Shsj"].ToString(), row["Syks"].ToString(), row["Yxjl"].ToString(),
                        row["Syks"].ToString(), row["DeptName"].ToString(), row["ShysName"].ToString(), int.Parse(row["YxjlId"].ToString()),
                        UnzipContent(row["WorkFlowXML"].ToString()));
                    cliListInfo.LjSyqk = row["LjSyqk"].ToString();
                    cliListInfo.LjSysl = decimal.Parse(row["LjSySl"].ToString());
                    clinPathListInfo.Add(cliListInfo);
                }

                return clinPathListInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }         // zm 8.24 Oracle
        //}

        //add by luff 20130228
        /// <summary>
        /// 获取全部PE_UserRole用户角色的所有权限
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<PE_UserRole> GetPE_UserRoleInfo()
        {
            List<PE_UserRole> list = new List<PE_UserRole>();
            try
            {
                string sql =  @"SELECT * FROM PE_UserRole ";

                DataTable dt = SqlHelper.ExecuteDataTable(sql);


                foreach (DataRow row in dt.Rows)
                {
                    PE_UserRole role = new PE_UserRole();
                    role.UserID = row["UserID"].ToString();
                    role.RoleCode = row["RoleCode"].ToString();
                    
                    list.Add(role);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;

        }


        //add by luff 20130305
        /// <summary>
        /// 获得指定字段类别编号的字典数据
        /// </summary>
        /// <param name="keywords"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_DataCategoryDetail> GetDataCategoryInfo(int ilbbh)
        {

            try
            {
                //定义参数
                SqlParameter paralbbh = new SqlParameter("@Lbbh", SqlDbType.Int);
                paralbbh.Value= ilbbh;

                SqlParameter[] parameters = new SqlParameter[] { paralbbh };

                //调用存取过程 得到数据列表
                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_GetDataCategory", parameters, CommandType.StoredProcedure);

                List<CP_DataCategoryDetail> datacategorylist = new List<CP_DataCategoryDetail>();

                foreach (DataRow row in dataTable.Rows)
                {
                     CP_DataCategoryDetail datacategoryInfo = new CP_DataCategoryDetail();
                    datacategoryInfo.Mxbh =Convert.ToInt16(row["Mxbh"].ToString());
                    datacategoryInfo.Name = row["Name"].ToString();
                    //datacategoryInfo.Lbbh = Convert.ToInt16(row["Lbbh"].ToString());

                    datacategorylist.Add(datacategoryInfo);
                }

                return datacategorylist;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 

    }
}
