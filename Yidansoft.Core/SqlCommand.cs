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
    public class SqlCommand
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

        //获取入院诊断名称
        public string GetIDCName(string idc)
        {
            try
            {
                string sql = "select Name from Diagnosis where ICD =" + "'" +
                    idc + "'";
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
    }
}
