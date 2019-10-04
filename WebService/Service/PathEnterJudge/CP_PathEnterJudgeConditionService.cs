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
using YidanSoft.Tool;
namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 查询路径进入条件类表
        /// </summary>
        /// <param name="path">路径代码</param>
        /// <returns>条件列表</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_PathEnterJudgeCondition> GetPathCP_PathEnterJudgeConditionAll(String path)
        {
            List<CP_PathEnterJudgeCondition> cplist = new List<CP_PathEnterJudgeCondition>();
            try
            {
                StringBuilder sb = new StringBuilder();
                //            sb.AppendFormat(@" select CP_PathEnterJudgeCondition.*,CP_ExamDictionaryDetail.Jcmc as JcmcName
                //                                from CP_PathEnterJudgeCondition
                //    
                //                                left join CP_ExamDictionaryDetail
                //                                ON CP_ExamDictionaryDetail.Jcbm=CP_PathEnterJudgeCondition.Jcxm
                //                                where 1=1 and LB=1  and Ljdm='{0}'   ", path);


                sb.AppendFormat(@" select CP_PathEnterJudgeCondition.*,CP_ExamDictionaryDetail.Jcmc as JcmcName
 from CP_PathEnterJudgeCondition
 right join CP_ExamDictionaryDetail
 ON CP_ExamDictionaryDetail.Jcbm=CP_PathEnterJudgeCondition.Jcxm
 where 1=1 and LB=1  and Ljdm='{0}'
 union 
select CP_PathEnterJudgeCondition.*, Diagnosis.name +'【'+ Diagnosis.icd + '】'  as JcmcName
from CP_PathEnterJudgeCondition
left join  Diagnosis on CP_PathEnterJudgeCondition.Jcxm=Diagnosis.icd
where 1=1 and LB=1  and Ljdm='{0}' and id not in 
(select CP_PathEnterJudgeCondition.id
 from CP_PathEnterJudgeCondition
 right join CP_ExamDictionaryDetail
 ON CP_ExamDictionaryDetail.Jcbm=CP_PathEnterJudgeCondition.Jcxm
 where 1=1 and LB=1  and Ljdm='{0}') ", path);



                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                //  DataTable dt = SqlHelper.ExecuteDataTable(DBConnection.conStr, CommandType.Text, sb.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    CP_PathEnterJudgeCondition cp = new CP_PathEnterJudgeCondition();
                    cp.ID = ConvertMy.ToInt32(dr["ID"]);
                    cp.Lb = ConvertMy.ToInt32(dr["Lb"]);
                    cp.Ljdm = ConvertMy.ToString(dr["Ljdm"]);
                    cp.Jddm = ConvertMy.ToString(dr["Jddm"]);
                    cp.Sjfl = ConvertMy.ToString(dr["Sjfl"]);
                    cp.Jcxm = ConvertMy.ToString(dr["Jcxm"]);
                    cp.Xmlb = ConvertMy.ToInt32(dr["Xmlb"]);
                    cp.Jsfw = ConvertMy.ToString(dr["Jsfw"]);
                    cp.Ksfw = ConvertMy.ToString(dr["Ksfw"]);
                    cp.Syrq = ConvertMy.ToString(dr["Syrq"]);
                    cp.Dw = ConvertMy.ToString(dr["Dw"]);
                    cp.Bz = ConvertMy.ToString(dr["Bz"]);
                    cp.JcxmName = cp.Jcxm;
                    //if (cp.Xmlb == 1)
                    //{ 
                    if (!string.IsNullOrEmpty(ConvertMy.ToString(dr["JcmcName"])))
                    {
                        cp.JcxmName = ConvertMy.ToString(dr["JcmcName"]);
                    }

                    //}

                    //if (cp.Xmlb == 2)
                    //{
                    //    cp.JcxmName = cp.Jcxm;
                    //    StringBuilder sb2 = new StringBuilder();
                    //    sb2.AppendFormat(@"  SELECT Name FROM CP_PathDiagnosis where Zdbs='{0}' ", cp.Jcxm);
                    //    DataTable dt2 = SqlHelper.ExecuteDataTable( sb2.ToString());
                    //    if (dt2 != null && dt2.Rows.Count > 0)
                    //    {
                    //        cp.JcxmName = cp.Jcxm + ":" + ConvertMy.ToString(dt2.Rows[0]["Name"]);
                    //    }
                    //}
                    cplist.Add(cp);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return cplist;
        }
        /// <summary>
        /// 查询节点进入条件类表
        /// </summary>
        /// <param name="path">路径代码</param>
        /// <param name="node">节点代码</param>
        /// <returns>条件列表</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_PathEnterJudgeCondition> GetNodeCP_PathEnterJudgeConditionAll(String node)
        {
            List<CP_PathEnterJudgeCondition> cplist = new List<CP_PathEnterJudgeCondition>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@" select CP_PathEnterJudgeCondition.*,CP_ExamDictionaryDetail.Jcmc as JcmcName
                                from CP_PathEnterJudgeCondition
    
                                left join CP_ExamDictionaryDetail
                                ON CP_ExamDictionaryDetail.Jcbm=CP_PathEnterJudgeCondition.Jcxm
                                where 1=1 and LB=2  and Jddm='{0}'", node);
                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                //  DataTable dt = SqlHelper.ExecuteDataTable(DBConnection.conStr, CommandType.Text, sb.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    CP_PathEnterJudgeCondition cp = new CP_PathEnterJudgeCondition();
                    cp.ID = ConvertMy.ToInt32(dr["ID"]);
                    cp.Lb = ConvertMy.ToInt32(dr["Lb"]);
                    cp.Ljdm = ConvertMy.ToString(dr["Ljdm"]);
                    cp.Jddm = ConvertMy.ToString(dr["Jddm"]);
                    cp.Sjfl = ConvertMy.ToString(dr["Sjfl"]);
                    cp.Jcxm = ConvertMy.ToString(dr["Jcxm"]);
                    cp.Xmlb = ConvertMy.ToInt32(dr["Xmlb"]);
                    cp.Jsfw = ConvertMy.ToString(dr["Jsfw"]);
                    cp.Ksfw = ConvertMy.ToString(dr["Ksfw"]);
                    cp.Syrq = ConvertMy.ToString(dr["Syrq"]);
                    cp.Dw = ConvertMy.ToString(dr["Dw"]);
                    cp.Bz = ConvertMy.ToString(dr["Bz"]);
                    cp.JcxmName = ConvertMy.ToString(dr["Jcxm"]);
                    if (cp.Xmlb == 1)
                    {
                        cp.JcxmName = ConvertMy.ToString(dr["JcmcName"]);
                    }

                    cplist.Add(cp);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return cplist;
        }

        /// <summary>
        /// 插入进入条件
        /// </summary>
        /// <param name="condition">进入条件实体</param>
        /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Int32 GetInsertCP_PathEnterJudgeCondition(CP_PathEnterJudgeCondition condition)
        {
            Int32 iReturn = 0;
            try
            {
                //condition.Syrq = condition.SuitCrowsMapScopes.Syrq;
                //condition.Ksfw = condition.SuitCrowsMapScopes.Ksfw;
                //condition.Jsfw = condition.SuitCrowsMapScopes.Jsfw;
                StringBuilder sb = new StringBuilder();
                String[] para = new string[11];
                para[0] = condition.Lb.ToString();
                para[1] = condition.Ljdm;
                para[2] = condition.Jddm;
                para[3] = condition.Sjfl;
                para[4] = condition.Jcxm;
                para[5] = condition.Xmlb.ToString();
                para[6] = condition.Jsfw;
                para[7] = condition.Ksfw;
                para[8] = condition.Syrq;
                para[9] = condition.Dw;
                para[10] = condition.Bz;
                //para[11] = condition.JcxmName;

                sb.AppendFormat(@" insert into CP_PathEnterJudgeCondition(Lb ,Ljdm,Jddm,Sjfl,Jcxm,Xmlb,Jsfw,Ksfw,Syrq,Dw,Bz) values(
            '{0}',	'{1}',	'{2}',	'{3}',	'{4}',	'{5}',	'{6}',	'{7}',	'{8}',	'{9}',	'{10}')", para);
                iReturn += 1; SqlHelper.ExecuteNoneQuery(sb.ToString());
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return iReturn;
        }
        /// <summary>
        /// 更新进入条件
        /// </summary>
        /// <param name="condition">进入条件实体</param>
        /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Int32 GetUpdateCP_PathEnterJudgeCondition(CP_PathEnterJudgeCondition condition)
        {
            Int32 iReturn = 0;
            try
            {
                //condition.Syrq = condition.SuitCrowsMapScopes.Syrq;
                //condition.Ksfw = condition.SuitCrowsMapScopes.Ksfw;
                //condition.Jsfw = condition.SuitCrowsMapScopes.Jsfw;
                StringBuilder sb = new StringBuilder();
                String[] para = new string[12];
                para[0] = condition.Lb.ToString();
                para[1] = condition.Ljdm;
                para[2] = condition.Jddm;
                para[3] = condition.Sjfl;
                para[4] = condition.Jcxm;
                para[5] = condition.Xmlb.ToString();
                para[6] = condition.Jsfw;
                para[7] = condition.Ksfw;
                para[8] = condition.Syrq;
                para[9] = condition.Dw;
                para[10] = condition.Bz;
                para[11] = condition.ID.ToString();
                //para[12] = condition.JcxmName.ToString();
                sb.AppendFormat(@" update  CP_PathEnterJudgeCondition set Lb ='{0}', Ljdm='{1}', Jddm='{2}', Sjfl='{3}', Jcxm='{4}'
                            ,Xmlb='{5}',	Jsfw='{6}',	Ksfw='{7}',	Syrq='{8}',	Dw='{9}',	Bz='{10}'
                            where ID='{11}'", para);
                iReturn += 1; SqlHelper.ExecuteNoneQuery(sb.ToString());
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return iReturn;
        }
        /// <summary>
        /// 更新进入条件
        /// </summary>
        /// <param name="condition">进入条件实体</param>
        /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Int32 GetDeleteCP_PathEnterJudgeCondition(String ID)
        {
            Int32 iReturn = 0;
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@" delete from  CP_PathEnterJudgeCondition where ID='{0}'", ID);
                iReturn += 1; SqlHelper.ExecuteNoneQuery(sb.ToString());
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return iReturn;
        }
    }
}
