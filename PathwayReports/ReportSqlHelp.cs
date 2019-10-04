using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using YidanSoft.Core;

namespace YidanEHRReport
{
    public class ReportSqlHelp
    {
        /// <summary>
        /// 根据传入的存储过程名，参数获取DataTable
        /// </summary>
        /// <param name="procName">存储过程名称</param>
        /// <param name="sqlparams">m_Sqlparams</param>
        /// <returns></returns>
        public DataTable GetTableByPorc(String procName, SqlParameter[] sqlparams)
        {
            DataTable dt = DataAccessFactory.DefaultDataAccess.ExecuteDataTable(procName, sqlparams, CommandType.StoredProcedure);

            return dt;
        }
        /// <summary>
        /// 返回dataset
        /// </summary>
        /// <param name="procName"></param>
        /// <param name="sqlparams"></param>
        /// <returns></returns>
        public DataSet GetSetByPorc(String procName, SqlParameter[] sqlparams)
        {
            DataSet ds = DataAccessFactory.DefaultDataAccess.ExecuteDataSet(procName, sqlparams, CommandType.StoredProcedure);

            return ds;
        }

        public DataTable GetTableBySQL(String execSQL)
        {

            DataTable dt = DataAccessFactory.DefaultDataAccess.ExecuteDataTable(execSQL);

            return dt;
        }

        /// <summary>
        /// 返回当前数据库中医院名称
        /// </summary>
        /// <returns></returns>
        public string GetHospitalName()
        {
            string sql = "select * from cp_hospital";

            DataTable dt = GetTableBySQL(sql);

            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0]["Name"].ToString();
            }
            return "某某医院";
        }
        //获取路径名称
        public string GetClinicalPathName(string ljdm)
        {
            try
            {
                string sql = "select Name from CP_ClinicalPath where Ljdm =" + "'" + 
                    ljdm + "'";
                DataTable dt = GetTableBySQL(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    return dt.Rows[0]["Name"].ToString();
                }
                return "某某路径";
                }
                catch (Exception)
                {                
                    throw;
                }
        }
        //获取指定路径路径明细信息与名称
        public void GetClinicalPathDetailInfo(string ljdm, ref List<string> listId, ref List<string> listMc)
        {
            try
            {
                string sql = "select PahtDetailID,Ljmc from CP_PathDetail " + 
                    " where Ljdm =" + "'" + ljdm + "'";
                DataTable dt = GetTableBySQL(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i ++)
                    {
                        listId.Add(dt.Rows[0]["PahtDetailID"].ToString());
                        listMc.Add(dt.Rows[0]["Ljmc"].ToString());
                    }
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }

        /// <summary>
        /// 获取指定路径路径明细信息与名称,如果detailID传入为空则返回路径所有节点信息
        /// </summary>
        /// <param name="ljdm">路径代码</param>
        /// <returns></returns>
        public DataTable GetClinicalPathDetailInfo(string ljdm,string detailID)
        {
            try
            {
                string sql = "select PahtDetailID,Ljmc from CP_PathDetail " +
                               " where Ljdm =" + "'" + ljdm + "'";
                if (detailID.Trim().Length > 0)
                {
                    sql += string.Format(" and PahtDetailID = '{0}'", detailID);
                }
                DataTable dt = GetTableBySQL(sql);
                return dt;
            }
            catch (Exception ex)
            {
                
                throw ex;
            }
        }
 
        /// <summary>
        /// 根据路径代码，首页序号，路径详细代码获取患者执行路径节点详细信息
        /// </summary>
        /// <param name="ljdm">路径代码</param>
        /// <param name="syxh">首页序号</param>
        /// <param name="detailID">路径节点ID</param>
        /// <returns></returns>
        public DataTable GetPathDetailInfo(string ljdm,string syxh,string detailID)
        {
            try
            {
                string sql = string.Format(@"select  a.Ljdm , a.Ljmc , b.jrsj
                                            from    CP_PathDetail a
                                                    left join CP_InpatientPathExedetail b on a.Ljdm = b.ljdm and a.PahtDetailID = b.jddm
                                            where   a.Ljdm = '{0}' and b.syxh = '{1}' and a.PahtDetailID = '{2}'
                                            order by b.id ", ljdm, syxh, detailID);
                DataTable dt = GetTableBySQL(sql);
                return dt;
            }
            catch (Exception ex)
            { 
                throw ex;
            }
        }
        //获取某病人临床路径下某节点的主疗，护理，非药物治疗数据CP_DiagNurExecRecord
        public void GetDiagNurExecRecordInfo(string syxh, string ljdm, string pathDetailId, int lbxh, ref List<string> listMc)
        {
            try
            {
                string sql = "select MxName from CP_DiagNurExecRecord " +
                    " where Syxh =" + "'" + syxh + "'" +
                    " and Ljdm =" + "'" + ljdm + "'" +
                    " and PathDetailID =" + "'" + pathDetailId + "'" +
                    " and Lbxh =" + lbxh;
                DataTable dt = GetTableBySQL(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        listMc.Add(dt.Rows[0]["MxName"].ToString());
                    }
                }
            }
            catch (Exception)
            {                
                throw;
            }
        }
        //获取某病人临床路径下某节点的变异数据CP_VariantRecords
        public void GetVariantRecordInfo(string syxh, string ljdm, string pathDetailId, ref List<string> listMc)
        {
            try
            {
                string sql = "select Bynr from CP_VariantRecords " +
                    " where Syxh =" + "'" + syxh + "'" +
                    " and Ljdm =" + "'" + ljdm + "'" +
                    " and PahtDetailID =" + "'" + pathDetailId + "'";
                DataTable dt = GetTableBySQL(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        listMc.Add(dt.Rows[0]["Bynr"].ToString());
                    }
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

       /// <summary>
       /// 根据患者ID，路径代码、路径节点ID获取路径当前节点对应护理信息
       /// </summary>
       /// <param name="_sSyxh"></param>
       /// <param name="_sLjdm"></param>
       /// <param name="_sDetailID"></param>
       /// <returns></returns>
        public DataTable GetNurseInfo(string _sSyxh, string _sLjdm, string _sDetailID)
        {
            string sql = @"select tid id,syxh, Ljdm,PathDetailId,Mxxh,MxName,Lbxh,Yxjl,Iskx, 
                            case Iskx when '1' then '可选' when '0' then '必选' end as Extension1,
                            Py,Wb,Create_Time,Create_User,Cancel_Time,Cancel_User,Extension,Extension1,Extension2,Extension3 ,1 yzzt,Isjj,Zxksdm
                 from dbo.CP_DiagNurExecRecord
                        where Ljdm='" + _sLjdm + "' and PathDetailId ='" + _sDetailID + "' and Syxh = '" + _sSyxh + "' ORDER BY Create_Time desc";
            DataTable dt = GetTableBySQL(sql);

            return dt;
        }

        //根据路径代码首页序号获取路径序号
        public string GetInPathPatientId(string syxh, string ljdm)
        {
            try
            {
                string sql = "select Id from CP_InPathPatient " +
                    " where Syxh =" + "'" + syxh + "'" +
                    " and Ljdm =" + "'" + ljdm + "'";
                string ljxh = "";
                DataTable dt = GetTableBySQL(sql);
                if (dt != null && dt.Rows.Count > 0)
                {
                    for (int i = 0; i < dt.Rows.Count; i++)
                    {
                        ljxh = dt.Rows[0]["Id"].ToString();
                    }
                }
                return ljxh;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
