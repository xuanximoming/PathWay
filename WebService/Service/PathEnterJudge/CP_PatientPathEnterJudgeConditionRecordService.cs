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
        /// 记录进入条件的状态,并且调用usp_CP_InsertPathPatientInfo和 操作记录
        /// </summary>
        /// <param name="condition">进入条件实体</param>
        /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Int32 InsertCP_PatientPathEnterJudgeConditionRecord(List<CP_PatientPathEnterJudgeConditionRecord> records, string doctorid, CP_InpatinetList inpat)
        {
            int iReturn = 0;
            try
            {
                Decimal id = 0;

                using (SqlConnection cn = new SqlConnection(m_ConnectionString))
                {
                    //cn.Open();
                    SqlTransaction sqlTrans = null;
                    //add by luff 20130717 先判断该病人是否已经入该路径，避免重复入径
                    string strSql1 = string.Format("select * from CP_InPathPatient where Syxh ='{0}' and  Ljdm ='{1}'", inpat.Syxh, inpat.Ljdm);

            
                    DataTable dtpara = SqlHelper.ExecuteDataTable(strSql1.ToString());
                    if (dtpara.Rows.Count > 0)//表示该病人已经入该路径
                    {
                        iReturn = -1;
                        return iReturn;
                    }
                    else
                    {
                        id = InsertCP_InPathPatientInfo(sqlTrans, doctorid, cn, inpat);
                    }
                }
                String strSql = "Insert into CP_PathExecuteInfo values(" + id + ",1100,'','" + doctorid + "','');";
                SqlHelper.ExecuteNoneQuery(strSql);
                foreach (CP_PatientPathEnterJudgeConditionRecord record in records)
                {


                    StringBuilder sb = new StringBuilder();
                    String[] para = new string[15];
                    para[0] = record.Lb.ToString();
                    para[1] = record.Ljdm;
                    para[2] = record.Jddm;
                    para[3] = record.Sjfl;
                    para[4] = record.Xmlb.ToString() == "9" ? record.JcxmName : record.Jcxm;
                    para[5] = record.Xmlb.ToString();
                    para[6] = record.Jsfw;
                    para[7] = record.Ksfw;

                    para[8] = record.Dw;
                    para[9] = record.Bz;

                    para[10] = ConvertMy.ToString(id);
                    para[11] = record.Syxh;
                    para[12] = record.Jcjg;
                    para[13] = record.Pdjg;
                    para[14] = record.JcxmName;
                    //原表名CP_PatientPathEnterJudgeConditionRecord，因为超过长度30所以改成CP_PatientEnterConditionRD
                    sb.AppendFormat(@" insert into CP_PatientEnterConditionRD(
              Lb ,  Ljdm,   Jddm,   Sjfl,   Jcxm,   Xmlb,   Jsfw,   Ksfw,    Dw,      Bz,   Ljxh,   Syxh,   Jcjg,   Pdjg,JcxmName) values(
            '{0}',	'{1}',	'{2}',	'{3}',	'{4}',	'{5}',	'{6}',	'{7}',	'{8}',	'{9}',	'{10}',	'{11}',	'{12}',	'{13}','{14}')", para);
                    iReturn += 1;
                    SqlHelper.ExecuteNoneQuery(sb.ToString());

                }


            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return iReturn;
        }

        /// <summary>
        /// 记录进入条件的状态,并且调用usp_CP_InsertPathPatientInfo和 操作记录
        /// </summary>
        /// <param name="condition">进入条件实体</param>
        /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Int32 InsertCP_PatientNodeEnterJudgeConditionRecord(List<CP_PatientPathEnterJudgeConditionRecord> records, string doctorid, CP_InpatinetList inpat)
        {
            int iReturn = 0;
            try
            {

                foreach (CP_PatientPathEnterJudgeConditionRecord record in records)
                {
                    StringBuilder sb = new StringBuilder();
                    String[] para = new string[15];
                    para[0] = record.Lb.ToString();
                    para[1] = record.Ljdm;
                    para[2] = record.Jddm;
                    para[3] = record.Sjfl;
                    para[4] = record.Xmlb.ToString() == "9" ? record.JcxmName : record.Jcxm;
                    para[5] = record.Xmlb.ToString();
                    para[6] = record.Jsfw;
                    para[7] = record.Ksfw;

                    para[8] = record.Dw;
                    para[9] = record.Bz;

                    para[10] = ConvertMy.ToString(inpat.Ljxh);
                    para[11] = record.Syxh;
                    para[12] = record.Jcjg;
                    para[13] = record.Pdjg;
                    para[14] = record.JcxmName;
                    //原表名CP_PatientPathEnterJudgeConditionRecord，因为超过长度30所以改成CP_PatientEnterConditionRD

                    sb.AppendFormat(@" insert into CP_PatientPathEnterJudgeConditionRecord(
              Lb ,  Ljdm,   Jddm,   Sjfl,   Jcxm,   Xmlb,   Jsfw,   Ksfw,    Dw,      Bz,   Ljxh,   Syxh,   Jcjg,   Pdjg,JcxmName) values(
            '{0}',	'{1}',	'{2}',	'{3}',	'{4}',	'{5}',	'{6}',	'{7}',	'{8}',	'{9}',	'{10}',	'{11}',	'{12}',	'{13}','{14}')", para);
                    iReturn += 1;
                    SqlHelper.ExecuteNoneQuery(sb.ToString());


                }


            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return iReturn;
        }

    }
}
