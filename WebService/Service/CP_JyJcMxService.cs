using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Data.SqlClient;
using System.Data;
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
        /// 根据项目代码获得检验检查项目明细
        /// </summary>
        /// <param name="Sxmdm">项目代码</param>
        /// <returns>返回医嘱实体集合</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_DoctorOrder> GetJyJcMXInfo(String Sxmdm)
        {

            List<CP_DoctorOrder> cplist = new List<CP_DoctorOrder>();
            try
            {
                 
                CP_DoctorOrder model = new CP_DoctorOrder();
                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("select * from CP_ChargingMinItem where dbo.Get_StrArrayStrOfIndex(Sfxmdm,'.',1) = ");
                strSql.AppendFormat(" dbo.Get_StrArrayStrOfIndex('{0}','.',1) ", Sxmdm);
                strSql.AppendFormat(" ORDER BY dbo.Get_StrArrayStrOfIndex(Fjxx,'|',1),convert(int,dbo.Get_StrArrayStrOfIndex(Fjxx,'|',2))");

                DataTable dt = SqlHelper.ExecuteDataTable(strSql.ToString());
                if (dt.Rows.Count > 1)
                {
                    #region 成套项目
                    foreach (DataRow dr in dt.Rows)
                    {
                        model = new CP_DoctorOrder();


                        model.Ypdm = dr["Sfxmdm"].ToString();

                        model.Ypmc = dr["Name"].ToString();

                        model.Xmdj = decimal.Parse(dr["Xmdj"].ToString());

                        model.Ypgg = dr["Xmgg"].ToString();

                        model.Zxdw = dr["Xmdw"].ToString();

                        model.Xmlb = decimal.Parse(dr["Xmlb"].ToString());

                        cplist.Add(model);
                    }
                    #endregion
                }
                else if (dt.Rows.Count == 1)
                {
                    #region 无成套,单个项目
                    strSql = new StringBuilder();
                    strSql.AppendFormat(" select * from CP_ChargingMinItem a where a.Sfxmdm = ");
                    strSql.AppendFormat(" dbo.Get_StrArrayStrOfIndex('{0}','.',2)", Sxmdm);


                    DataTable dtt = SqlHelper.ExecuteDataTable(strSql.ToString());
                    //if (dtt.Rows.Count > 0)
                    //{
                    foreach (DataRow drr in dtt.Rows)
                    {
                        model = new CP_DoctorOrder();


                        model.Ypdm = drr["Sfxmdm"].ToString();

                        model.Ypmc = drr["Name"].ToString();

                        model.Xmdj = decimal.Parse(drr["Xmdj"].ToString());

                        model.Ypgg = drr["Xmgg"].ToString();

                        model.Zxdw = drr["Xmdw"].ToString();

                        model.Xmlb = decimal.Parse(drr["Xmlb"].ToString());

                        cplist.Add(model);
                    }
                    //}

                    #endregion
                }
                else
                {
                    #region
                    foreach (DataRow dr in dt.Rows)
                    {
                        model = new CP_DoctorOrder();


                        model.Ypdm = dr["Sfxmdm"].ToString();

                        model.Ypmc = dr["Name"].ToString();

                        model.Xmdj = decimal.Parse(dr["Xmdj"].ToString());

                        model.Ypgg = dr["Ypgg"].ToString();

                        model.Zxdw = dr["Xmdw"].ToString();

                        model.Xmlb = decimal.Parse(dr["Xmlb"].ToString());

                        cplist.Add(model);
                    }
                    #endregion
                }
                return cplist;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return cplist;
            }

        }

        /// <summary>
        /// 根据检验检查项目代码判断是否成套
        /// </summary>
        /// <returns>返回值2 表示 多个成套；1 单个成套或单个无成套</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public int GetCTbySxmdm(string Sxmdm)
        {
            int reival = 0;
            try
            {

                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat("select * from CP_ChargingMinItem where dbo.Get_StrArrayStrOfIndex(Sfxmdm,'.',1) = ");
                strSql.AppendFormat(" dbo.Get_StrArrayStrOfIndex('{0}','.',1) ", Sxmdm);

                DataTable dt = SqlHelper.ExecuteDataTable(strSql.ToString());
                if (dt.Rows.Count > 1)//多个成套
                {
                    reival = 2;
                }
                else if (dt.Rows.Count == 1)//单个成套或单个无成套
                {
                    reival = 1;
                }
                else //没有数据
                {

                    reival = 0;
                }
            }
            catch (Exception ex)
            {
                reival = 0;
                ThrowException(ex);
                
            }

            return reival;

        }

        
        /// <summary>
        /// 获得单个检验检查项目代码
        /// </summary>
        /// <returns>返回值不为空 表示 有子代码；空 表示 无对应子代码</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public string GetSxmdmbyID(string Sxmdm)
        {
            string reival = "";
            try
            {

                StringBuilder strSql = new StringBuilder();
                strSql.AppendFormat(" select * from CP_ChargingMinItem a where a.Sfxmdm = ");
                strSql.AppendFormat(" dbo.Get_StrArrayStrOfIndex('{0}','.',2)", Sxmdm);

                DataTable dt = SqlHelper.ExecuteDataTable(strSql.ToString());
                if (dt.Rows.Count > 0)
                {
                    reival = dt.Rows[0]["Sfxmdm"].ToString();
                }
                else
                {

                    reival = "";
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                reival = "";
            }

            return reival;

        }
    }
}