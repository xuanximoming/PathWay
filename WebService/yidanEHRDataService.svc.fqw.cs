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
using System.Collections.ObjectModel;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {

        #region DGQ
        /// <summary>
        /// 查询数据库类别
        /// </summary>
        /// <param name="Lbbh">数据库中的编号(如医嘱类别的编号为'24')</param>
        /// <returns></returns>
        [OperationContract]
        public List<CP_DataCategoryDetail> GetCP_DataCategoryDetailCategory()
        {
            List<CP_DataCategoryDetail> cplist = new List<CP_DataCategoryDetail>();

            try
            {
                String sql = "select * from CP_DataCategoryDetail where Lbbh=24";

                DataTable dt = SqlHelper.ExecuteDataTable(sql);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_DataCategoryDetail cp = new CP_DataCategoryDetail();
                    cp.Mxbh = Convert.ToInt16(dr["Mxbh"].ToString());
                    cp.Name = dr["Name"].ToString();
                    cp.Lbbh = Convert.ToInt16(dr["Lbbh"].ToString());
                    cplist.Add(cp);
                }
                return cplist;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

        }           // zm 8.24 Oracle
        [OperationContract]
        public List<CP_TempOrderList> GetTempOrderList(decimal syxh, decimal mxbh, string ksrq, string jsrq)
        {
            List<CP_TempOrderList> cplist = new List<CP_TempOrderList>();

            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                         new SqlParameter("@mxbh",mxbh),
                         new SqlParameter("@syxh",syxh),
                         new SqlParameter("@ksrq",ksrq),
                         new SqlParameter("@jsrq",jsrq)
                    };
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_GetTempOrderList", parameters, CommandType.StoredProcedure);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_TempOrderList cp = new CP_TempOrderList();

                    cp.Lsyzxh = Convert.ToDecimal(dr["Lsyzxh"].ToString());

                    cp.Syxh = Convert.ToDecimal(dr["Syxh"].ToString());

                    cp.Fzbz = Convert.ToInt32(dr["Fzbz"].ToString());

                    cp.Lrysdm = dr["Lrysdm"].ToString();

                    cp.Lrrq = dr["Lrrq"].ToString();

                    cp.Shczy = dr["Shczy"].ToString();

                    cp.Shrq = dr["Shrq"].ToString();

                    cp.Zxczy = dr["Zxczy"].ToString();

                    cp.Zxrq = string.IsNullOrEmpty(dr["Zxrq"].ToString()) == true ? null : Convert.ToDateTime(dr["Zxrq"].ToString()).ToShortDateString();

                    cp.Zxsj_E = string.IsNullOrEmpty(dr["Zxrq"].ToString()) == true ? null : Convert.ToDateTime(dr["Zxrq"].ToString()).ToShortTimeString();

                    cp.Qxysdm = dr["Qxysdm"].ToString();

                    cp.Qxrq = dr["Qxrq"].ToString();

                    cp.Tzrq = dr["Tzrq"].ToString();
                    cp.Jjlx = int.Parse(dr["Isjj"].ToString());
                    cp.Zxksdm = dr["Zxksdm"].ToString();
                    cp.Yzkx = int.Parse(dr["Yzkx"].ToString());
                    //备用字段
                    cp.Extension = "";
                    cp.Extension1 = "";
                    cp.Extension2 = "";
                    cp.Extension3 = "";
                    cp.Extension4 = 0;

                    cp.Ksrq = string.IsNullOrEmpty(dr["Ksrq"].ToString()) == true ? null : Convert.ToDateTime(dr["Ksrq"].ToString()).ToShortDateString();

                    cp.Kssj = string.IsNullOrEmpty(dr["Ksrq"].ToString()) == true ? null : Convert.ToDateTime(dr["Ksrq"].ToString()).ToShortTimeString();

                    cp.Ypmc = dr["Ypmc"].ToString();

                    cp.Ypgg = dr["Ypgg"].ToString();

                    cp.Xmlb = Convert.ToInt32(dr["Xmlb"].ToString());

                    cp.Zxdw = dr["Zxdw"].ToString();

                    cp.Ypjl = Convert.ToDecimal(dr["Ypjl"].ToString());

                    cp.Jldw = dr["Jldw"].ToString();

                    cp.Dwxs = Convert.ToDecimal(dr["Dwxs"].ToString());

                    cp.Dwlb = Convert.ToInt32(dr["Dwlb"].ToString());

                    cp.Yfdm = dr["Yfdm"].ToString();

                    cp.Pcdm = dr["Pcdm"].ToString();
                     

                    cp.Zxcs = Convert.ToInt32(dr["Zxcs"].ToString());

                    cp.Zxzq = Convert.ToInt32(dr["Zxzq"].ToString());

                    cp.Zxzqdw = Convert.ToInt32(dr["Zxzqdw"].ToString());

                    cp.Zdm = dr["Zdm"].ToString();

                    cp.Zxsj = dr["Zxsj"].ToString();

                    cp.Ztnr = dr["Ztnr"].ToString();

                    cp.Yznr = dr["Yznr"].ToString();

                    cp.ShczyName = dr["ShczyName"].ToString();

                    cp.ZxczyName = dr["ZxczyName"].ToString();

                    cp.QxysName = dr["QxysName"].ToString();

                    cp.DwlbName = dr["DwlbName"].ToString();
                    
                    cp.YfName = dr["YfName"].ToString();

                    cp.PcName = dr["PcName"].ToString();

                    cp.ZxzqdwName = dr["ZxzqdwName"].ToString();

                    cp.LrysName = dr["LrysName"].ToString();

                    cp.Yzzt = decimal.Parse(dr["Yzzt"].ToString() == string.Empty ? "0" : dr["Yzzt"].ToString());

                    cplist.Add(cp);
                }
                return cplist;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

        }   // zm 8.24 Oracle
        [OperationContract]
        public List<CP_LongOrderList> GetLongOrderList(decimal syxh, decimal mxbh, string ksrq, string jsrq)
        {
            List<CP_LongOrderList> cplist = new List<CP_LongOrderList>();

            try
            {


                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@mxbh",mxbh),
                        new SqlParameter("@syxh",syxh),
                        new SqlParameter("@ksrq",ksrq),
                        new SqlParameter("@jsrq",jsrq)
                    };

                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_GetLongOrderList", parameters, CommandType.StoredProcedure);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_LongOrderList cp = new CP_LongOrderList();

                    cp.Cqyzxh = Convert.ToDecimal(dr["Cqyzxh"].ToString());

                    cp.Syxh = Convert.ToDecimal(dr["Syxh"].ToString());

                    cp.Fzbz = Convert.ToInt32(dr["Fzbz"].ToString());

                    cp.Lrysdm = dr["Lrysdm"].ToString();

                    cp.Tzshhs = dr["Tzshhs"].ToString();

                    cp.Tzshrq = string.IsNullOrEmpty(dr["Tzshrq"].ToString()) == true ? null : Convert.ToDateTime(dr["Tzshrq"].ToString()).ToShortDateString();

                    cp.Lrrq = dr["Lrrq"].ToString();

                    cp.Shczy = dr["Shczy"].ToString();

                    cp.Shrq = dr["Shrq"].ToString();

                    cp.Zxczy = dr["Zxczy"].ToString();

                    cp.Zxrq = string.IsNullOrEmpty(dr["Zxrq"].ToString()) == true ? null : Convert.ToDateTime(dr["Zxrq"].ToString()).ToShortDateString();

                    cp.Zxsj_E = string.IsNullOrEmpty(dr["Zxrq"].ToString()) == true ? null : Convert.ToDateTime(dr["Zxrq"].ToString()).ToShortTimeString();

                    cp.Qxysdm = dr["Qxysdm"].ToString();

                    cp.Qxrq = dr["Qxrq"].ToString();

                    cp.Tzrq = dr["Tzrq"].ToString();

                    cp.Tzysdm = dr["Tzysdm"].ToString();

                    cp.Ksrq = string.IsNullOrEmpty(dr["Ksrq"].ToString()) == true ? null : Convert.ToDateTime(dr["Ksrq"].ToString()).ToShortDateString();

                    cp.Kssj = string.IsNullOrEmpty(dr["Ksrq"].ToString()) == true ? null : Convert.ToDateTime(dr["Ksrq"].ToString()).ToShortTimeString();

                    cp.Ypmc = dr["Ypmc"].ToString();

                    cp.Ypgg = dr["Ypgg"].ToString();
                    cp.Jjlx = int.Parse(dr["Isjj"].ToString());
                    cp.Zxksdm = dr["Zxksdm"].ToString();

                    cp.Yzkx = int.Parse(dr["Yzkx"].ToString());
                    //备用字段
                    cp.Extension = "";
                    cp.Extension1 = "";
                    cp.Extension2 = "";
                    cp.Extension3 = "";
                    cp.Extension4 = 0;

                    cp.Xmlb = Convert.ToInt32(dr["Xmlb"].ToString());

                    cp.Zxdw = dr["Zxdw"].ToString();

                    cp.Ypjl = Convert.ToDecimal(dr["Ypjl"].ToString());

                    cp.Jldw = dr["Jldw"].ToString();

                    cp.Dwxs = Convert.ToDecimal(dr["Dwxs"].ToString());

                    cp.Dwlb = Convert.ToInt32(dr["Dwlb"].ToString());

                    cp.Yfdm = dr["Yfdm"].ToString();

                    cp.Pcdm = dr["Pcdm"].ToString();

                    cp.Zxcs = Convert.ToInt32(dr["Zxcs"].ToString());

                    cp.Zxzq = Convert.ToInt32(dr["Zxzq"].ToString());

                    cp.Zxzqdw = Convert.ToInt32(dr["Zxzqdw"].ToString());

                    cp.Zdm = dr["Zdm"].ToString();

                    cp.Zxsj = dr["Zxsj"].ToString();

                    cp.Ztnr = dr["Ztnr"].ToString();

                    cp.Yznr = dr["Yznr"].ToString();

                    cp.ShczyName = dr["ShczyName"].ToString();

                    cp.ZxczyName = dr["ZxczyName"].ToString();

                    cp.QxysName = dr["QxysName"].ToString();

                    cp.DwlbName = dr["DwlbName"].ToString();

                    cp.YfName = dr["YfName"].ToString();

                    cp.PcName = dr["PcName"].ToString();

                    cp.TzysName = dr["TzysName"].ToString();

                    cp.TzshhsName = dr["TzshhsName"].ToString();

                    cp.ZxzqdwName = dr["ZxzqdwName"].ToString();

                    cp.LrysName = dr["LrysName"].ToString();

                    cp.Yzzt = decimal.Parse(dr["Yzzt"].ToString() == string.Empty ? "0" : dr["Yzzt"].ToString());

                    cplist.Add(cp);
                }
                return cplist;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

        }   // zm 8.24 Oracle
        /// <summary>
        ///  add by luff 20130517 获取草药医嘱信息
        /// </summary>
        /// <param name="syxh"></param>
        /// <param name="mxbh"></param>
        /// <param name="ksrq"></param>
        /// <param name="jsrq"></param>
        /// <returns></returns>
        [OperationContract]
        public List<CP_CyXDFOrder> GetCyXDFOrderList(decimal syxh, decimal mxbh, string ksrq, string jsrq)
        {
            List<CP_CyXDFOrder> cplist = new List<CP_CyXDFOrder>();

            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                         new SqlParameter("@mxbh",mxbh),
                         new SqlParameter("@syxh",syxh),
                         new SqlParameter("@ksrq",ksrq),
                         new SqlParameter("@jsrq",jsrq)
                    };
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_GetCyXDFOrderList", parameters, CommandType.StoredProcedure);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_CyXDFOrder cp = new CP_CyXDFOrder();

                    cp.Lsyzxh = Convert.ToDecimal(dr["Cyfyzxh"].ToString());
                    cp.Cfmc = dr["Cfmc"].ToString();
                    cp.Syxh = Convert.ToDecimal(dr["Syxh"].ToString());

                    cp.Fzbz = Convert.ToInt32(dr["Fzbz"].ToString());

                    cp.Lrysdm = dr["Lrysdm"].ToString();

                    cp.Lrrq = dr["Lrrq"].ToString();

                    cp.Shczy = dr["Shczy"].ToString();

                    cp.Shrq = dr["Shrq"].ToString();

                    cp.Zxczy = dr["Zxczy"].ToString();

                    cp.Zxrq = string.IsNullOrEmpty(dr["Zxrq"].ToString()) == true ? null : Convert.ToDateTime(dr["Zxrq"].ToString()).ToShortDateString();

                    cp.Zxsj_E = string.IsNullOrEmpty(dr["Zxrq"].ToString()) == true ? null : Convert.ToDateTime(dr["Zxrq"].ToString()).ToShortTimeString();

                    cp.Qxysdm = dr["Qxysdm"].ToString();

                    cp.Qxrq = dr["Qxrq"].ToString();

                    cp.Tzrq = dr["Tzrq"].ToString();
                    cp.Jjlx = int.Parse(dr["Isjj"].ToString());
                    cp.Zxksdm = dr["Zxksdm"].ToString();
                    cp.Yzkx = int.Parse(dr["Yzkx"].ToString());
                    //备用字段
                    cp.Extension = "";
                    cp.Extension1 = "";
                    cp.Extension2 = "";
                    cp.Extension3 = "";
                    cp.Extension4 = 0;

                    cp.Ksrq = string.IsNullOrEmpty(dr["Ksrq"].ToString()) == true ? null : Convert.ToDateTime(dr["Ksrq"].ToString()).ToShortDateString();

                    cp.Kssj = string.IsNullOrEmpty(dr["Ksrq"].ToString()) == true ? null : Convert.ToDateTime(dr["Ksrq"].ToString()).ToShortTimeString();

                    cp.Ypmc = dr["Ypmc"].ToString();

                    cp.Ypgg = dr["Ypgg"].ToString();

                    cp.Xmlb = Convert.ToInt32(dr["Xmlb"].ToString());

                    cp.Zxdw = dr["Zxdw"].ToString();

                    cp.Ypjl = Convert.ToDecimal(dr["Ypjl"].ToString());

                    cp.Jldw = dr["Jldw"].ToString();

                    cp.Dwxs = Convert.ToDecimal(dr["Dwxs"].ToString());

                    cp.Dwlb = Convert.ToInt32(dr["Dwlb"].ToString());

                    cp.Yfdm = dr["Yfdm"].ToString();

                    cp.Pcdm = dr["Pcdm"].ToString();


                    cp.Zxcs = Convert.ToInt32(dr["Zxcs"].ToString());

                    cp.Zxzq = Convert.ToInt32(dr["Zxzq"].ToString());

                    cp.Zxzqdw = Convert.ToInt32(dr["Zxzqdw"].ToString());

                    cp.Zdm = dr["Zdm"].ToString();

                    cp.Zxsj = dr["Zxsj"].ToString();

                    cp.Ztnr = dr["Ztnr"].ToString();

                    cp.Yznr = dr["Yznr"].ToString();

                    cp.ShczyName = dr["ShczyName"].ToString();

                    cp.ZxczyName = dr["ZxczyName"].ToString();

                    cp.QxysName = dr["QxysName"].ToString();

                    cp.DwlbName = dr["DwlbName"].ToString();

                    cp.YfName = dr["YfName"].ToString();

                    cp.PcName = dr["PcName"].ToString();

                    cp.ZxzqdwName = dr["ZxzqdwName"].ToString();

                    cp.LrysName = dr["LrysName"].ToString();

                    cp.Yzzt = decimal.Parse(dr["Yzzt"].ToString() == string.Empty ? "0" : dr["Yzzt"].ToString());

                    cplist.Add(cp);
                }
                return cplist;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

        } 
        #endregion

        #region 登录信息
        [OperationContract]
        public string[] GetLoginInfo(string zgdm)
        {
            string[] loginInfo = new string[2];
            //using (SqlConnection cn = new SqlConnection(m_ConnectionString))
            //{
            try
            {
                String sql = "Select nvl(dp.Name,'未分配科室'),nvl(bq.Name,'未分配病区') From Users ep Left join  Department dp on dp.ID = ep.DeptId left join Ward bq on bq.ID= ep.Bqdm where ep.ID='" + zgdm + "'";


                IDataReader reader = SqlHelper.ExecuteReader(sql);

                while (reader.Read())
                {
                    loginInfo[0] = reader.GetString(0);
                    loginInfo[1] = reader.GetString(1);

                }
                return loginInfo;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

        }  // zm 8.24 Oracle
        #endregion

        #region 取项目类别的对象和List
        /// <summary>
        /// 取项目类别对象
        /// 修改：fqw 时间：2010-03-18  mark：fqwFix 说明：该函数内部有缺陷，并且没有任何地方使用
        /// </summary>
        /// <param name="lbbh">传入类别编号</param>
        /// <returns></returns>
        [OperationContract]
        public CP_DataCategoryDetail GetDataCategoryObject(int lbbh)
        {
            try
            {
                DataTable dt = SqlHelper.ExecuteDataTable(string.Format("select Name From CP_DataCategoryDetail where    Lbbh={1}", ConvertMy.ToString(lbbh)));
                CP_DataCategoryDetail CategoryDetail = null;
                if (dt.Rows.Count > 0)
                {
                    CategoryDetail = new CP_DataCategoryDetail();
                    CategoryDetail.Mxbh = ConvertMy.ToShort(dt.Rows[0]["Mxbh"]);
                    CategoryDetail.Name = ConvertMy.ToString(dt.Rows[0]["Name"]);
                    CategoryDetail.Lbbh = ConvertMy.ToShort(dt.Rows[0]["Lbbh"]);
                    CategoryDetail.Py = ConvertMy.ToString(dt.Rows[0]["Py"]);
                    CategoryDetail.Wb = ConvertMy.ToString(dt.Rows[0]["Wb"]);
                    CategoryDetail.Memo = ConvertMy.ToString(dt.Rows[0]["Memo"]);
                }
                return CategoryDetail;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }



        }
        /// <summary>
        /// 取项目类别List
        /// </summary>
        /// <param name="lbbh">传入类别编号:传入99999时查询全部</param>
        /// <returns></returns>
        [OperationContract]
        public List<CP_DataCategoryDetail> GetDataCategoryList(int lbbh)
        {
            List<CP_DataCategoryDetail> cplist = new List<CP_DataCategoryDetail>();

            try
            {

                String sql;
                if (lbbh == 99999)
                {

                    sql = "select * from CP_DataCategoryDetail ";
                }
                else
                {

                    sql = "select * from CP_DataCategoryDetail where Lbbh=" + lbbh;
                }

                DataTable dt = SqlHelper.ExecuteDataTable(sql);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_DataCategoryDetail cp = new CP_DataCategoryDetail();
                    cp.Mxbh = Convert.ToInt16(dr["Mxbh"].ToString());
                    cp.Name = dr["Name"].ToString();
                    cp.Lbbh = Convert.ToInt16(dr["Lbbh"].ToString());
                    cp.Py = dr["Py"].ToString();
                    cp.Wb = dr["Wb"].ToString();
                    cp.Memo = dr["Memo"].ToString();
                    cplist.Add(cp);
                }
                return cplist;
            }
            catch (Exception ex)
            {
                throw new ApplicationException(ex.Message);
            }

        }       // zm 8.24 Oracle

        #endregion

        #region 医嘱配置相关
 
        /// <summary>
        /// 获取药品信息
        /// </summary>
        /// <param name="Cdxh"></param>
        /// <returns></returns>
        [OperationContract]
        public List<CP_PlaceOfDrug> GetDrugInfo(string Cdxh)
        {
            List<CP_PlaceOfDrug> cp_drugList = new List<CP_PlaceOfDrug>();

            try
            {
                string sqlStr = string.Empty;
                DataTable dt;
                if (CheckSelectHISView())
                {
                    //从HIS视图中获取数据
                    sqlStr = "select * from PathWay_Drug";

                    dt = HISHelper.ExecuteDataTable(sqlStr);
                }
                else
                {
                    if (!string.IsNullOrEmpty(Cdxh))
                    {
                        sqlStr = "Select upper(Py) py,upper(Wb) wb,* from CP_PlaceOfDrug where Cdxh='" + Cdxh + "' and Yxjl=1";
                    }
                    else
                    {
                        sqlStr = "Select upper(Py) py,upper(Wb) wb,* from CP_PlaceOfDrug where Yxjl=1 ";
                    }

                    dt = SqlHelper.ExecuteDataTable(sqlStr);
                }

                foreach (DataRow dr in dt.Rows)
                {
                    CP_PlaceOfDrug cp = new CP_PlaceOfDrug();
                    cp.Cdxh = Convert.ToDecimal(dr["Cdxh"].ToString());
                    cp.Ggxh = Convert.ToDecimal(dr["Ggxh"].ToString());
                    cp.Lcxh = Convert.ToDecimal(dr["Lcxh"].ToString());
                    cp.Ypmc = dr["Ypmc"].ToString();
                    cp.Ypdm = dr["Ypdm"].ToString();
                    cp.Srm = dr["Srm"].ToString();
                    cp.Py = dr["Py"].ToString();
                    cp.Wb = dr["Wb"].ToString();
                    cp.Ypgg = dr["Ypgg"].ToString();
                    cp.Jxdm = dr["Jxdm"].ToString();
                    cp.Flmxdm = dr["Flmxdm"].ToString();
                    cp.Zxdw = dr["Zxdw"].ToString();
                    cp.Ggdw = dr["Ggdw"].ToString();
                    cp.Ggxs = Convert.ToDecimal(dr["Ggxs"].ToString());
                    cp.Ykdw = dr["Ykdw"].ToString();
                    cp.Ykxs = Convert.ToDecimal(dr["Ykxs"].ToString());
                    cp.Mzdw = dr["Mzdw"].ToString();
                    cp.Mzxs = Convert.ToDecimal(dr["Mzxs"].ToString());
                    cp.Zydw = dr["Zydw"].ToString();
                    cp.Zyxs = Convert.ToDecimal(dr["Zyxs"].ToString());
                    cp.Ekdw = dr["Ekdw"].ToString();
                    cp.Ekxs = Convert.ToDecimal(dr["Ekxs"].ToString());
                    cp.Lsj = Convert.ToDecimal(dr["Lsj"].ToString());
                    cp.Mzbxbz = Convert.ToInt16(dr["Mzbxbz"].ToString());
                    cp.Zybxbz = Convert.ToInt16(dr["Zybxbz"].ToString());
                    cp.Ypxz = Convert.ToInt32(dr["Ypxz"].ToString());
                    cp.Tsypbz = Convert.ToInt16(dr["Tsypbz"].ToString() == "" ? "0" : dr["Tsypbz"].ToString());
                    cp.Yplydm = dr["Yplydm"].ToString();
                    cp.Cjmc = dr["Cjmc"].ToString();
                    cp.Yzglbz = Convert.ToInt16(dr["Yzglbz"].ToString());
                    cp.Syfw = Convert.ToInt16(dr["Syfw"].ToString() == "" ? "0" : dr["Syfw"].ToString());
                    cp.Yplb = Convert.ToInt16(dr["Yplb"].ToString());
                    cp.Yxjl = Convert.ToInt16(dr["Yxjl"].ToString());
                    cp.Memo = dr["Memo"].ToString();
                    cp_drugList.Add(cp);

                }
                return cp_drugList;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }

        /// <summary>
        ///频次代码 
        /// </summary>
        /// <param name="pcdm"></param>
        /// <returns></returns>
        [OperationContract]
        public List<CP_AdviceFrequency> GetAdviceFrequency(string pcdm)
        {
            List<CP_AdviceFrequency> cplist = new List<CP_AdviceFrequency>();

            try
            {
                string sqlstr = string.Empty;
                if (string.IsNullOrEmpty(pcdm))
                {
                    sqlstr = "Select * from CP_AdviceFrequency Where Yxjl=1 ";
                }
                else
                {
                    sqlstr = "Select * from CP_AdviceFrequency where Pcdm='" + pcdm + "' and Yxjl=1";
                }


                DataTable dt = SqlHelper.ExecuteDataTable(sqlstr);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_AdviceFrequency cp = new CP_AdviceFrequency();
                    cp.Pcdm = dr["Pcdm"].ToString();
                    cp.Name = dr["Name"].ToString();
                    cp.Py = dr["Name"].ToString();
                    cp.Wb = dr["Name"].ToString();
                    cp.Zxcs = Convert.ToInt32(dr["Zxcs"].ToString());
                    cp.Zxzq = Convert.ToInt32(dr["Zxzq"].ToString());
                    cp.Zxzqdw = Convert.ToInt16(dr["Zxzqdw"].ToString());
                    cp.Zdm = dr["Zdm"].ToString();
                    cp.Zxsj = dr["Zxsj"].ToString();
                    cp.zbbz = Convert.ToInt16(dr["zbbz"].ToString());
                    cp.Yzglbz = Convert.ToInt16(dr["Yzglbz"].ToString());
                    cp.Yxjl = Convert.ToInt16(dr["Yxjl"].ToString());
                    cp.Memo = dr["Memo"].ToString();
                    cplist.Add(cp);
                }
                return cplist;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }       // zm 8.24 Oracle
        /// <summary>
        /// 获取用法集合
        /// </summary>
        /// <param name="jxdm">剂型代码</param>
        /// <returns></returns>
        [OperationContract]
        public string GetYfjh(string jxdm)
        {
            try
            {
                String sql = "Select Yfjh from CP_Dosage2Useage where jxdm='" + jxdm + "'";

                String strReturn = "";
                DataTable dt = SqlHelper.ExecuteDataTable(sql);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != null)
                {
                    strReturn = dt.Rows[0][0].ToString().Trim();
                }
                return strReturn;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }            // zm 8.24 Oracle
        /// <summary>
        /// 根据剂型加载用法
        /// </summary>
        /// <param name="Jxdm">剂型代码</param>
        /// <returns></returns>
        [OperationContract]
        public List<CP_DrugUseage> GetDrugUseage(string Jxdm)
        {
            List<CP_DrugUseage> cplist = new List<CP_DrugUseage>();

            try
            {
                string sqlstr = string.Empty;
                if (Jxdm != "")
                {
                    string strVar = GetYfjh(Jxdm);
                    if (strVar == String.Empty)
                        sqlstr = "Select * from CP_DrugUseage ";
                    else
                        sqlstr = "Select * from CP_DrugUseage Where Yfdm " + " in (" + strVar + ")";
                }
                else
                {
                    sqlstr = "Select * from CP_DrugUseage ";
                }

                DataTable dt = SqlHelper.ExecuteDataTable(sqlstr);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_DrugUseage cp = new CP_DrugUseage();
                    cp.Yfdm = dr["Yfdm"].ToString();
                    cp.Name = dr["Name"].ToString();
                    cp.Py = dr["Py"].ToString();
                    cp.Wb = dr["Wb"].ToString();
                    cp.Ctsy = Convert.ToInt16(dr["Ctsy"].ToString());
                    cp.Zdfz = Convert.ToInt16(dr["Zdfz"].ToString());
                    cp.Yflb = Convert.ToInt16(dr["Yflb"].ToString());
                    cp.Yxjl = Convert.ToInt16(dr["Yxjl"].ToString());
                    cp.Memo = dr["Memo"].ToString();
                    cplist.Add(cp);
                }
                return cplist;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }       // zm 8.24 Oracle

        private decimal GetCtyzxh(string PahtDetailID, string name, string strLjdm)
        {

            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@PahtDetailID",PahtDetailID),
                        new SqlParameter("@Name",name),
                        new SqlParameter("@Ljdm",strLjdm)
                    };

                Decimal returnDecimal = 0;
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_InsertAdviceGroup", parameters, CommandType.StoredProcedure);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != null)
                    returnDecimal = Convert.ToDecimal(dt.Rows[0][0].ToString());
                return returnDecimal;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return 0;
            }


        }           // zm 8.24 Oracle
        /// <summary>
        /// 添加成套医嘱信息
        /// </summary>
        /// <param name="cpd">页面填写数据对象</param>
        /// <param name="ctyzxh">成套医嘱序号</param>
        /// <param name="yzbz">医嘱标志</param>
        /// <param name="fzbz">分组标志cpd.Fzxh</param>
        /// <returns></returns>
        [OperationContract]
        public string InsertIntoAdviceGroup(CP_AdviceGroupDetail cpd, string nodeid, int fzbz, string nodename, string strLjdm)
        {

            try
            {
                decimal ctyzxh = GetCtyzxh(nodeid, nodename, strLjdm);
                //string sqlstr = "Insert into CP_AdviceGroupDetail Values(" + ctyzxh + "," + cpd.Yzbz + "," + cpd.Fzxh + "," + fzbz + "," + cpd.Cdxh + "," + cpd.Ggxh + ","
                //     + " " + cpd.Lcxh + ",'" + cpd.Ypdm + "','" + cpd.Ypmc + "'," + cpd.Xmlb + ",'" + cpd.Zxdw + "'," + cpd.Ypjl + ",'" + cpd.Jldw + "'," + cpd.Dwxs + "," + cpd.Dwlb + ",'" + cpd.Yfdm + "',"
                //+ " '" + cpd.Pcdm + "'," + cpd.Zxcs + ", " + cpd.Zxzq + ",'" + cpd.Zxzqdw + "','" + cpd.Zdm + " ','" + cpd.Zxsj + "'," + cpd.Zxts + "," + cpd.Ypzsl + ",'" + cpd.Ztnr + "','" + cpd.Yzlb + "','" + cpd.Jjlx + "','" + cpd.Zxksdm + "','" + cpd.Mzdm + "')  ";
                string sql = "select Max(OrderValue) from CP_AdviceGroupDetail ";
               cpd._OrderValue =Convert.ToInt32(SqlHelper.ExecuteScalar(sql)) +1;
                string sqlstr = string.Format(@"INSERT INTO CP_AdviceGroupDetail
                                    ([Ctyzxh], [Yzbz], [Fzxh], [Fzbz], 
                                    [Cdxh], [Ggxh], [Lcxh], [Ypdm], [Ypmc], 
                                    [Xmlb], [Zxdw], [Ypjl], [Jldw], [Dwxs], 
                                    [Dwlb], [Yfdm], [Pcdm], [Zxcs], [Zxzq], 
                                    [Zxzqdw], [Zdm], [Zxsj], [Zxts], [Ypzsl], 
                                    [Ztnr], [Yzlb], [MZDM], [Zxksdm], [Isjj],
                                    [Yzkx], [Extension], [Extension1], [Extension2], [Extension3],[Extension4],[OrderValue])
                                    VALUES(
                                    '{0}', '{1}', '{2}', '{3}', 
                                    '{4}',  '{5}', '{6}', '{7}', '{8}', 
                                    '{9}', '{10}', '{11}', '{12}', '{13}', 
                                    '{14}', '{15}', '{16}', '{17}', '{18}', 
                                    '{19}', '{20}', '{21}', '{22}', '{23}', 
                                    '{24}', '{25}', '{26}', '{27}', '{28}',
                                    '{29}', '{30}', '{31}', '{32}', '{33}', '{34}','{35}')",
                                    ctyzxh , cpd.Yzbz,cpd.Fzxh,fzbz ,
                                    cpd.Cdxh,cpd.Ggxh ,cpd.Lcxh , cpd.Ypdm , cpd.Ypmc,
                                    cpd.Xmlb , cpd.Zxdw , cpd.Ypjl , cpd.Jldw , cpd.Dwxs,
                                    cpd.Dwlb , cpd.Yfdm,cpd.Pcdm , cpd.Zxcs , cpd.Zxzq,
                                    cpd.Zxzqdw , cpd.Zdm , cpd.Zxsj , cpd.Zxts , cpd.Ypzsl,
                                    cpd.Ztnr, cpd.Yzlb, cpd.Mzdm, cpd.Zxksdm, cpd.Jjlx,
                                    cpd.Yzkx, cpd.Extension, cpd.Extension1, cpd.Extension2, cpd.Extension3, cpd.Extension4,cpd._OrderValue);


                sqlstr += "  update CP_AdviceGroupDetail set Fzxh=Ctmxxh where Fzxh=" + cpd.Fzxh + " and Cdxh=" + cpd.Cdxh + " and Ggxh=" + cpd.Ggxh + " and Lcxh=" + cpd.Lcxh + " and Ypdm='" + cpd.Ypdm + "'  and Ypmc='" + cpd.Ypmc + "'";
                SqlHelper.ExecuteNoneQuery(sqlstr);

                return AdviceGroupInfo;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return ex.Message;
            }

        }  // zm 8.24 Oracle

        /// <summary>
        /// 添加多条成套医嘱信息
        /// </summary>
        /// <param name="cpdlsit">页面填写数据list对象</param>
        /// <param name="ctyzxh">成套医嘱序号</param>
        /// <param name="yzbz">医嘱标志</param>
        /// <param name="fzbz">分组标志cpd.Fzxh</param>
        /// <returns></returns>
        [OperationContract]
        public string InsertIntoAdviceGroupList(List<CP_AdviceGroupDetail> cpdlsit, string nodeid, int fzbz, string nodename, string strLjdm)
        {

            try
            {
                decimal ctyzxh = GetCtyzxh(nodeid, nodename, strLjdm);
                //string sqlstr = "Insert into CP_AdviceGroupDetail Values(" + ctyzxh + "," + cpd.Yzbz + "," + cpd.Fzxh + "," + fzbz + "," + cpd.Cdxh + "," + cpd.Ggxh + ","
                //     + " " + cpd.Lcxh + ",'" + cpd.Ypdm + "','" + cpd.Ypmc + "'," + cpd.Xmlb + ",'" + cpd.Zxdw + "'," + cpd.Ypjl + ",'" + cpd.Jldw + "'," + cpd.Dwxs + "," + cpd.Dwlb + ",'" + cpd.Yfdm + "',"
                //+ " '" + cpd.Pcdm + "'," + cpd.Zxcs + ", " + cpd.Zxzq + ",'" + cpd.Zxzqdw + "','" + cpd.Zdm + " ','" + cpd.Zxsj + "'," + cpd.Zxts + "," + cpd.Ypzsl + ",'" + cpd.Ztnr + "','" + cpd.Yzlb + "','" + cpd.Jjlx + "','" + cpd.Zxksdm + "','" + cpd.Mzdm + "')  ";
                foreach (CP_AdviceGroupDetail cpd in cpdlsit)
                {
                    string sqlstr = string.Format(@"INSERT INTO CP_AdviceGroupDetail
                                    ([Ctyzxh], [Yzbz], [Fzxh], [Fzbz], 
                                    [Cdxh], [Ggxh], [Lcxh], [Ypdm], [Ypmc], 
                                    [Xmlb], [Zxdw], [Ypjl], [Jldw], [Dwxs], 
                                    [Dwlb], [Yfdm], [Pcdm], [Zxcs], [Zxzq], 
                                    [Zxzqdw], [Zdm], [Zxsj], [Zxts], [Ypzsl], 
                                    [Ztnr], [Yzlb], [MZDM], [Zxksdm], [Isjj],
                                    [Yzkx], [Extension], [Extension1], [Extension2], [Extension3],[Extension4])
                                    VALUES(
                                    '{0}', '{1}', '{2}', '{3}', 
                                    '{4}',  '{5}', '{6}', '{7}', '{8}', 
                                    '{9}', '{10}', '{11}', '{12}', '{13}', 
                                    '{14}', '{15}', '{16}', '{17}', '{18}', 
                                    '{19}', '{20}', '{21}', '{22}', '{23}', 
                                    '{24}', '{25}', '{26}', '{27}', '{28}',
                                    '{29}', '{30}', '{31}', '{32}', '{33}', '{34}')",
                                        ctyzxh, cpd.Yzbz, cpd.Fzxh, fzbz,
                                        cpd.Cdxh, cpd.Ggxh, cpd.Lcxh, cpd.Ypdm, cpd.Ypmc,
                                        cpd.Xmlb, cpd.Zxdw, cpd.Ypjl, cpd.Jldw, cpd.Dwxs,
                                        cpd.Dwlb, cpd.Yfdm, cpd.Pcdm, cpd.Zxcs, cpd.Zxzq,
                                        cpd.Zxzqdw, cpd.Zdm, cpd.Zxsj, cpd.Zxts, cpd.Ypzsl,
                                        cpd.Ztnr, cpd.Yzlb, cpd.Mzdm, cpd.Zxksdm, cpd.Jjlx,
                                        cpd.Yzkx, cpd.Extension, cpd.Extension1, cpd.Extension2, cpd.Extension3, cpd.Extension4);


                    sqlstr += "  update CP_AdviceGroupDetail set Fzxh=Ctmxxh where Fzxh=" + cpd.Fzxh + " and Cdxh=" + cpd.Cdxh + " and Ggxh=" + cpd.Ggxh + " and Lcxh=" + cpd.Lcxh + " and Ypdm='" + cpd.Ypdm + "'  and Ypmc='" + cpd.Ypmc + "'";
                    SqlHelper.ExecuteNoneQuery(sqlstr);
                }

                return AdviceGroupInfo;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return ex.Message;
            }

        }  // zm 8.24 Oracle


        /// <summary>
        /// 草药医嘱添加
        /// </summary>
        /// <param name="cpd">页面填写数据对象</param>
        /// <param name="ctyzxh">成套医嘱序号</param>
        /// <param name="yzbz">医嘱标志</param>
        /// <param name="fzbz">分组标志cpd.Fzxh</param>
        /// <returns></returns>
        [OperationContract]
        public string InsertIntoCyAdviceGroup(CP_AdviceGroupDetail cpd, string nodeid, int fzbz, string nodename, string strLjdm)
        {

            try
            {
                string sqlstr = "";
                decimal ctyzxh = GetCtyzxh(nodeid, nodename, strLjdm);
                if (cpd.Extension1 == "协定方")//一个协定方包括多个草药方明细 循环依次插入
                {
                    
                    //string sqlstr = "Insert into CP_AdviceGroupDetail Values(" + ctyzxh + "," + cpd.Yzbz + "," + cpd.Fzxh + "," + fzbz + "," + cpd.Cdxh + "," + cpd.Ggxh + ","
                    //     + " " + cpd.Lcxh + ",'" + cpd.Ypdm + "','" + cpd.Ypmc + "'," + cpd.Xmlb + ",'" + cpd.Zxdw + "'," + cpd.Ypjl + ",'" + cpd.Jldw + "'," + cpd.Dwxs + "," + cpd.Dwlb + ",'" + cpd.Yfdm + "',"
                    //+ " '" + cpd.Pcdm + "'," + cpd.Zxcs + ", " + cpd.Zxzq + ",'" + cpd.Zxzqdw + "','" + cpd.Zdm + " ','" + cpd.Zxsj + "'," + cpd.Zxts + "," + cpd.Ypzsl + ",'" + cpd.Ztnr + "','" + cpd.Yzlb + "','" + cpd.Jjlx + "','" + cpd.Zxksdm + "','" + cpd.Mzdm + "')  ";

                      List<CP_CYXDFMX> cplist = new List<CP_CYXDFMX>();

                      cplist = GetCyxdfMXInfoById(int.Parse(cpd.Extension));

                    foreach (CP_CYXDFMX cp in cplist)
                    {
                        sqlstr = string.Format(@"INSERT INTO CP_AdviceGroupDetail
                                    ([Ctyzxh], [Yzbz], [Fzxh], [Fzbz], 
                                    [Cdxh], [Ggxh], [Lcxh], [Ypdm], [Ypmc], 
                                    [Xmlb], [Zxdw], [Ypjl], [Jldw], [Dwxs], 
                                    [Dwlb], [Yfdm], [Pcdm], [Zxcs], [Zxzq], 
                                    [Zxzqdw], [Zdm], [Zxsj], [Zxts], [Ypzsl], 
                                    [Ztnr], [Yzlb], [MZDM], [Zxksdm], [Isjj],
                                    [Yzkx], [Extension], [Extension1], [Extension2], [Extension3],[Extension4])
                                    VALUES(
                                    '{0}', '{1}', '{2}', '{3}', 
                                    '{4}',  '{5}', '{6}', '{7}', '{8}', 
                                    '{9}', '{10}', '{11}', '{12}', '{13}', 
                                    '{14}', '{15}', '{16}', '{17}', '{18}', 
                                    '{19}', '{20}', '{21}', '{22}', '{23}', 
                                    '{24}', '{25}', '{26}', '{27}', '{28}',
                                    '{29}', '{30}', '{31}', '{32}', '{33}', '{34}')",
                                       ctyzxh, 2799, 0, fzbz,
                                       cp.Extension, cp.Extension1, 0, cp.Ypdm, cp.Ypmc,
                                       2403, cp.Extension2, cpd.Ypjl, cp.Jldw, 1,
                                       cp.Dwlb, cp.Yfdm, cp.Pcdm, cp.cfts, cp.Zxts,
                                       cp.ypbz, "", "", 1, cp.Ypsl,
                                       cp.Memo, 3121, "", cp.Zxksdm, cp.Isjj,
                                       cp.Yzkx, "", "", cp.lcxmdm, cp.Extension4, "");


                        sqlstr += "  update CP_AdviceGroupDetail set Fzxh=Ctmxxh where Fzxh=" + cpd.Fzxh + " and Cdxh=" + cpd.Cdxh + " and Ggxh=" + cpd.Ggxh + " and Lcxh=" + cpd.Lcxh + " and Ypdm='" + cpd.Ypdm + "'  and Ypmc='" + cpd.Ypmc + "'";
                        SqlHelper.ExecuteNoneQuery(sqlstr);
                    
                    }
         
          
                    
                }
                else //草药明细方
                {
                    sqlstr = string.Format(@"INSERT INTO CP_AdviceGroupDetail
                                    ([Ctyzxh], [Yzbz], [Fzxh], [Fzbz], 
                                    [Cdxh], [Ggxh], [Lcxh], [Ypdm], [Ypmc], 
                                    [Xmlb], [Zxdw], [Ypjl], [Jldw], [Dwxs], 
                                    [Dwlb], [Yfdm], [Pcdm], [Zxcs], [Zxzq], 
                                    [Zxzqdw], [Zdm], [Zxsj], [Zxts], [Ypzsl], 
                                    [Ztnr], [Yzlb], [MZDM], [Zxksdm], [Isjj],
                                    [Yzkx], [Extension], [Extension1], [Extension2], [Extension3],[Extension4])
                                    VALUES(
                                    '{0}', '{1}', '{2}', '{3}', 
                                    '{4}',  '{5}', '{6}', '{7}', '{8}', 
                                    '{9}', '{10}', '{11}', '{12}', '{13}', 
                                    '{14}', '{15}', '{16}', '{17}', '{18}', 
                                    '{19}', '{20}', '{21}', '{22}', '{23}', 
                                    '{24}', '{25}', '{26}', '{27}', '{28}',
                                    '{29}', '{30}', '{31}', '{32}', '{33}', '{34}')",
                                       ctyzxh, cpd.Yzbz, cpd.Fzxh, fzbz,
                                       cpd.Cdxh, cpd.Ggxh, cpd.Lcxh, cpd.Ypdm, cpd.Ypmc,
                                       cpd.Xmlb, cpd.Zxdw, cpd.Ypjl, cpd.Jldw, cpd.Dwxs,
                                       cpd.Dwlb, cpd.Yfdm, cpd.Pcdm, cpd.Zxcs, cpd.Zxzq,
                                       cpd.Zxzqdw, cpd.Zdm, cpd.Zxsj, cpd.Zxts, cpd.Ypzsl,
                                       cpd.Ztnr, cpd.Yzlb, cpd.Mzdm, cpd.Zxksdm, cpd.Jjlx,
                                       cpd.Yzkx, cpd.Extension, cpd.Extension1, cpd.Extension2, cpd.Extension3, cpd.Extension4);


                    sqlstr += "  update CP_AdviceGroupDetail set Fzxh=Ctmxxh where Fzxh=" + cpd.Fzxh + " and Cdxh=" + cpd.Cdxh + " and Ggxh=" + cpd.Ggxh + " and Lcxh=" + cpd.Lcxh + " and Ypdm='" + cpd.Ypdm + "'  and Ypmc='" + cpd.Ypmc + "'";
                    SqlHelper.ExecuteNoneQuery(sqlstr);
                }

                return AdviceGroupInfo;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return ex.Message;
            }

        }

        /// <summary>
        /// 将OrderValue 的值，更新到数据库,重新排序
        /// 创建：Jhonny
        /// 创建时间：2013年8月29日 14:37:46
        /// </summary>
        /// <param name="type"></param>
        /// <param name="m_list"></param>
        [OperationContract]
        public int UpdateInfo(ObservableCollection<CP_AdviceGroupDetail> m_list)
        {
            try
            {
                string sql = null;
                foreach (CP_AdviceGroupDetail advice in m_list)
                {
                    sql = String.Format("update CP_AdviceGroupDetail set OrderValue = {0} where  ctmxxh={1}",advice._OrderValue , advice.Ctmxxh);
                    SqlHelper.ExecuteNoneQuery(sql);
                }
                return 1;
            }
            catch (Exception ex)
            {
                return -1;
                throw ex;
            }
        }


        /// <summary>WebService
        /// 获取成套医嘱 根据医嘱类别和成套序号
        /// </summary>
        /// <param name="yzbz"></param>
        /// <param name="ctyzxh"></param>
        /// <returns></returns>
        [OperationContract]
        public List<CP_AdviceGroupDetail> GetAdviceGroupDetailInfo(string yzlb, string nodeid,string sLjdm)
        {
            List<CP_AdviceGroupDetail> cplist = new List<CP_AdviceGroupDetail>();
            try
            {
                decimal ctyzxh = GetCtyzxh(nodeid, "", sLjdm);
                string sqlstr = "select *,cd.[Name] as YzbzName,Case When Fzbz !=3500 then '┃' else '' End as Flag,isnull(cp.Ypmc,'')+' '+isnull(af.Name,'')+' '+isnull(du.Name,'')+ ''+isnull(cp.Ztnr,'') as Yznr  from CP_AdviceGroupDetail "
                                    + " cp left join CP_DataCategoryDetail cd on cd.Mxbh=cp.Yzbz Left join CP_AdviceFrequency af "
                                   + " on af.Pcdm=cp.Pcdm left join CP_DrugUseage du on du.Yfdm=cp.Yfdm Where cp.Yzlb='" + yzlb + "' and cp.Ctyzxh=" + ctyzxh + " order by Yzbz Desc, OrderValue,Fzxh,Fzbz,Ctmxxh";


                DataTable dt = SqlHelper.ExecuteDataTable(sqlstr);

                int k = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    CP_AdviceGroupDetail cp = new CP_AdviceGroupDetail();
                    cp.Ctmxxh = Convert.ToDecimal(dr["Ctmxxh"].ToString());
                    cp.Ctyzxh = Convert.ToDecimal(dr["Ctyzxh"].ToString());
                    cp.Yzbz = Convert.ToInt16(dr["Yzbz"].ToString());
                    cp.Fzxh = Convert.ToDecimal(dr["Fzxh"].ToString());
                    cp.Fzbz = Convert.ToInt16(dr["Fzbz"].ToString());
                    cp.Cdxh = Convert.ToDecimal(dr["Cdxh"].ToString());
                    cp.Ggxh = Convert.ToDecimal(dr["Ggxh"].ToString());
                    cp.Lcxh = Convert.ToDecimal(dr["Lcxh"].ToString());
                    cp.Ypdm = dr["Ypdm"].ToString();
                    cp.Ypmc = dr["Ypmc"].ToString();
                    cp.Xmlb = Convert.ToInt16(string.IsNullOrEmpty(dr["Xmlb"].ToString()) ? "0" : dr["Xmlb"].ToString());
                    cp.Zxdw = dr["Zxdw"].ToString();
                    cp.Ypjl = Convert.ToDecimal(string.IsNullOrEmpty(dr["Ypjl"].ToString()) ? "0" : dr["Ypjl"].ToString());
                    //cp.Jldw = string.IsNullOrEmpty(dr["Ypjl"].ToString()) ? "0" : dr["Ypjl"].ToString() + dr["Jldw"].ToString();
                    cp.Jldw = string.IsNullOrEmpty(dr["Jldw"].ToString()) ? "" : dr["Jldw"].ToString();
                    cp.Dwxs = Convert.ToDecimal(string.IsNullOrEmpty(dr["Dwxs"].ToString()) ? "0" : dr["Dwxs"].ToString());
                    cp.Dwlb = Convert.ToInt32(string.IsNullOrEmpty(dr["Dwlb"].ToString()) ? "0" : dr["Dwlb"].ToString());
                    cp.Yfdm = dr["Yfdm"].ToString();
                    cp.Pcdm = dr["Pcdm"].ToString();
                    cp.Zxcs = Convert.ToInt32(string.IsNullOrEmpty(dr["Zxcs"].ToString()) ? "0" : dr["Zxcs"].ToString());
                    cp.Zxzq = Convert.ToInt32(string.IsNullOrEmpty(dr["Zxzq"].ToString()) ? "0" : dr["Zxzq"].ToString());
                    cp.Zxzqdw = Convert.ToInt16(string.IsNullOrEmpty(dr["Zxzqdw"].ToString()) ? "0" : dr["Zxzqdw"].ToString());
                    cp.Zdm = dr["Zdm"].ToString();
                    cp.Zxsj = dr["Zxsj"].ToString();
                    cp.Zxts = Convert.ToInt32(string.IsNullOrEmpty(dr["Zxts"].ToString()) ? "0" : dr["Zxts"].ToString());
                    cp.Ypzsl = Convert.ToDecimal(string.IsNullOrEmpty(dr["Ypzsl"].ToString()) ? "0" : dr["Ypzsl"].ToString());
                    cp.Ztnr = dr["Ztnr"].ToString();
                    cp.Yzlb = Convert.ToInt16(string.IsNullOrEmpty(dr["Yzlb"].ToString()) ? "0" : dr["Yzlb"].ToString());
                    cp.YzbzName = dr["YzbzName"].ToString();
                    cp.Flag = dr["Flag"].ToString();
                    cp.Jjlx = int.Parse(dr["Isjj"].ToString());
                    cp.Zxksdm = dr["Zxksdm"].ToString();
                    cp.Yzkx = dr["Yzkx"]==null?0:int.Parse(dr["Yzkx"].ToString());
                    //备用字段
                    cp.Extension =  dr["Extension"].ToString();
                    cp.Extension1 = dr["Extension1"].ToString();
                    cp.Extension2 = dr["Extension2"].ToString();
                    cp.Extension3 = dr["Extension3"].ToString();
                    cp.Extension4 = dr["Extension4"].ToString() == string.Empty ? 0 : Convert.ToInt16(dr["Extension4"].ToString());
                    cp.Index = k;
                    cp.Yznr = dr["Yznr"].ToString();
                    cp.Mzdm = ConvertMy.ToString(dr["Mzdm"]);
                    //cp._OrderValue=Convert.ToInt32(dr["OrderValue"].ToString()); //新增字段，标识排列 update Jhonny
                    cplist.Add(cp);
                    k++;
                }
                return cplist;

            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }   // zm 8.24 Oracle
        /// <summary>
        /// 删除成套医嘱明细
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        [OperationContract]
        public string DelAdviceGroupDetail(List<decimal> listid)
        {
            try
            {
                string listidstr = string.Empty;
                for (int i = 0; i < listid.Count; i++)
                {
                    listidstr += listid[i].ToString() + ',';
                }
                string sqlstr = "Delete From CP_AdviceGroupDetail where Ctmxxh in (" + listidstr.TrimEnd(',') + ")";


                SqlHelper.ExecuteNoneQuery(sqlstr);

                return string.Format("序号为【{0}】的数据" + DelMessage, listidstr.TrimEnd(','));
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return DelMessageFail + " 失败原因已提交";
            }

        }           // zm 8.24 Oracle
        /// <summary>
        /// 删除手术医嘱明细
        /// </summary>
        /// <param name="listid"></param>
        /// <returns></returns>
        [OperationContract]
        public string DelSSAdviceDetail(List<decimal> listid)
        {

            try
            {
                string listidstr = string.Empty;
                for (int i = 0; i < listid.Count; i++)
                {
                    listidstr += listid[i].ToString() + ',';
                }
                string sqlstr = "Delete From CP_AdviceAnesthesiaDetail where Ctmxxh in (" + listidstr.TrimEnd(',') + ")";

                SqlHelper.ExecuteNoneQuery(sqlstr);

                return string.Format("序号为【{0}】的数据" + DelMessage, listidstr.TrimEnd(','));
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return DelMessageFail + " 失败原因已提交";
            }

        }               // zm 8.24 Oracle
        /// <summary>
        /// 获取数据库数据(频次时间下拉框数据)
        /// </summary>
        /// <param name="pcdm"></param>
        /// <returns></returns>
        [OperationContract]
        public List<CP_PCSJ> GetDropDownInfo(string pcdm)
        {
            List<CP_PCSJ> cp = new List<CP_PCSJ>();

            try
            {
                string sqlstr = @"Select ca.Zxsj,ca.Zxzq,ca.Zdm,cd.Name AS DwName,ca.Zxcs,cd.Mxbh,"
                                + "Case  cd.Mxbh when 3400 then 'Week' "
                                + "when 3401 then 'Day' "
                                + "when 3402 then 'Hour' "
                                + "when 3403 then 'Minutes' End AS DwFlag "
                                + "From CP_AdviceFrequency ca Left Join CP_DataCategoryDetail cd "
                                + "on cd.Mxbh=ca.Zxzqdw Where ca.Pcdm='" + pcdm + "'";

                DataTable dt = SqlHelper.ExecuteDataTable(sqlstr);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_PCSJ pcsj = new CP_PCSJ();
                    pcsj.Zxcs = Convert.ToInt32(dr["Zxcs"].ToString());
                    pcsj.Zxsj = dr["Zxsj"].ToString();
                    pcsj.Zxzq = Convert.ToInt32(dr["Zxzq"].ToString());
                    pcsj.Mxbh = Convert.ToInt16(dr["Mxbh"].ToString());
                    pcsj.DwName = dr["DwName"].ToString();
                    pcsj.DwFlag = dr["DwFlag"].ToString();
                    pcsj.Zdm = dr["Zdm"].ToString();
                    switch (pcsj.DwFlag)
                    {
                        case "Week":
                            pcsj.DisplayText = "每" + pcsj.DwName + (pcsj.Zxzq == 1 ? "" : pcsj.Zxzq.ToString()) + "天" + "," + "每" + (pcsj.Zxcs == 1 ? "" : pcsj.Zxcs.ToString()) + "天" + (pcsj.Zxsj.Split(',').Length - 1).ToString() + "次";
                            break;
                        case "Day":
                            pcsj.DisplayText = "每" + (pcsj.Zxzq == 1 ? "" : pcsj.Zxzq.ToString()) + pcsj.DwName + pcsj.Zxcs.ToString() + "次";
                            break;
                        case "Hour":
                            pcsj.DisplayText = "每" + (pcsj.Zxzq == 1 ? "" : pcsj.Zxzq.ToString()) + pcsj.DwName + pcsj.Zxcs.ToString() + "次";
                            break;
                        case "Minutes":
                            pcsj.DisplayText = "每" + (pcsj.Zxzq == 1 ? "" : pcsj.Zxzq.ToString()) + pcsj.DwName + pcsj.Zxcs.ToString() + "次";
                            break;
                        default:
                            break;

                    }
                    cp.Add(pcsj);
                }
                return cp;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }       // zm 8.24 Oracle
        /// <summary>
        /// 根据数据库数据构造下拉框
        /// </summary>
        /// <param name="cp"></param>
        /// <returns></returns>
        [OperationContract]
        public List<CP_PCSJ_E> StructureDropDown(string pcdm)
        {
            try
            {
                List<CP_PCSJ_E> pcsj_e = new List<CP_PCSJ_E>();

                List<CP_PCSJ> cp = new List<CP_PCSJ>();
                cp = GetDropDownInfo(pcdm);
                foreach (var c in cp)
                {
                    string pctype = c.DwFlag;
                    switch (pctype)
                    {
                        case "Week":
                            for (int i = 0; i < 24; i++)
                            {
                                CP_PCSJ_E cp_e = new CP_PCSJ_E();
                                #region 24点 <=9
                                if (!string.IsNullOrEmpty(c.Zxsj) && (i <= 9))
                                {
                                    for (int j = 0; j < c.Zxsj.Split(',').Length - 1; j++)
                                    {
                                        if (c.Zxsj.Split(',')[j].ToString() == ("0" + i.ToString()))
                                        {
                                            cp_e.IsEnableWeek = "true";
                                            cp_e.IsChecked = "true";
                                            cp_e.IsWeek = "false";
                                            cp_e.DisplayValue = "0" + i.ToString();
                                            cp_e.DisplayName = "0" + i.ToString();
                                            cp_e.ClosedDisplayName = "每" + c.DwName + c.Zxzq + "天" + "," + "每" + (c.Zxcs == 1 ? "" : c.Zxcs.ToString()) + "天" + (c.Zxsj.Split(',').Length - 1).ToString() + "次";

                                        }
                                        else
                                        {
                                            cp_e.IsEnableWeek = "true";
                                            cp_e.IsChecked = "false";
                                            cp_e.IsWeek = "false";
                                            cp_e.DisplayValue = "0" + i.ToString();
                                            cp_e.DisplayName = "0" + i.ToString();
                                            cp_e.ClosedDisplayName = "每" + c.DwName + c.Zxzq + "天" + "," + "每" + (c.Zxcs == 1 ? "" : c.Zxcs.ToString()) + "天" + (c.Zxsj.Split(',').Length - 1).ToString() + "次";


                                        }
                                    }

                                }
                                #endregion
                                #region 24点 >9
                                if (!string.IsNullOrEmpty(c.Zxsj) && (i > 9))
                                {
                                    for (int j = 0; j < c.Zxsj.Split(',').Length - 1; j++)
                                    {
                                        if (c.Zxsj[j].ToString() == (i.ToString()))
                                        {
                                            cp_e.IsEnableWeek = "true";
                                            cp_e.IsChecked = "true";
                                            cp_e.IsWeek = "false";
                                            cp_e.DisplayValue = i.ToString();
                                            cp_e.DisplayName = i.ToString();
                                            cp_e.ClosedDisplayName = "每" + c.DwName + c.Zxzq + "天" + "," + "每" + (c.Zxcs == 1 ? "" : c.Zxcs.ToString()) + "天" + (c.Zxsj.Split(',').Length - 1).ToString() + "次";
                                        }
                                        else
                                        {
                                            cp_e.IsEnableWeek = "true";
                                            cp_e.IsChecked = "false";
                                            cp_e.IsWeek = "false";
                                            cp_e.DisplayValue = i.ToString();
                                            cp_e.DisplayName = i.ToString();
                                            cp_e.ClosedDisplayName = "每" + c.DwName + c.Zxzq + "天" + "," + "每" + (c.Zxcs == 1 ? "" : c.Zxcs.ToString()) + "天" + (c.Zxsj.Split(',').Length - 1).ToString() + "次";
                                        }
                                    }

                                }
                                pcsj_e.Add(cp_e);
                                #endregion
                            }
                            break;
                        case "Day":
                            break;
                        case "Hour":
                            break;
                        case "Minutes":
                            break;
                        default:
                            break;
                    }

                }
                return pcsj_e;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }
        }
        /// <summary>
        /// 医嘱成组操作
        /// </summary>
        /// <param name="first"></param>
        /// <param name="last"></param>
        /// <param name="middle"></param>
        /// <returns></returns>
        [OperationContract]
        public string AdviceGroup(string first, string last, List<decimal> middle)
        {

            try
            {
                string sqlstr = string.Empty;
                if (middle.Count > 0)
                {
                    string listidstr = string.Empty;
                    for (int i = 0; i < middle.Count; i++)
                    {
                        listidstr += middle[i].ToString() + ',';
                    }
                    sqlstr = "Update CP_AdviceGroupDetail set Fzbz=3501 where Ctmxxh=" + first + " "
                            + " Update CP_AdviceGroupDetail set Fzbz=3599,Fzxh=" + Convert.ToDecimal(first) + " where Ctmxxh=" + Convert.ToDecimal(last) + ""
                            + " Update CP_AdviceGroupDetail set Fzbz=3502,Fzxh=" + Convert.ToDecimal(first) + " where Ctmxxh in(" + listidstr.TrimEnd(',') + ")";
                }
                else
                {
                    sqlstr = " Update CP_AdviceGroupDetail set Fzbz=3501 where Ctmxxh=" + first + ""
                          + "  Update CP_AdviceGroupDetail set Fzbz=3599,Fzxh=" + first + " where Ctmxxh=" + last + " ";
                }

                SqlHelper.ExecuteNoneQuery(sqlstr);

                return AdviceGroupMessage;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return ex.Message;
            }

        }       // zm 8.24 Oracle
        [OperationContract]
        public string DisAdviceGroup(List<decimal> list)
        {

            try
            {
                string listidstr = string.Empty;
                for (int i = 0; i < list.Count; i++)
                {
                    listidstr += list[i].ToString() + ',';
                }
                string sqlstr = "Update CP_AdviceGroupDetail set Fzbz=3500 where Fzxh in (" + listidstr.TrimEnd(',') + ")"
                              + " Update CP_AdviceGroupDetail set Fzxh=Ctmxxh where Fzxh in (" + listidstr.TrimEnd(',') + ") ";


                SqlHelper.ExecuteNoneQuery(sqlstr);

                return DidsAdviceGroupMessage;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return ex.Message;
            }
            //    finally
            //    {
            //        if (cn.State != ConnectionState.Closed)
            //            cn.Close();
            //    }
            //}
        }               // zm 8.24 Oracle
        [OperationContract]
        public string UpdateAdviceGroupDetail(decimal fzxh, decimal ctmxxh, Yidansoft.Service.Entity.CP_AdviceGroupDetail cpd, bool isgroup)
        {

            try
            {
                string sqlstr = string.Empty;
                string sqlstr2 = string.Empty;
                if (isgroup)
                {
                    sqlstr = "Update CP_AdviceGroupDetail set Yzbz={0},Cdxh={1},Ggxh={2},Lcxh={3},Ypdm='{4}',Ypmc='{5}',Xmlb={6},Zxdw='{20}',"
                      + "Ypjl={7},Jldw='{8}',Yfdm='{9}',Pcdm='{10}',Zxcs={11},Zxzq={12},Zxzqdw={13},Zdm='{14}',Zxsj='{15}',Zxts={16},Ztnr='{17}',Yzlb={18},Mzdm='{21}',Isjj={22} ,Zxksdm='{23}',"
                    + "Yzkx={24},Extension='{25}', Extension1='{26}',Extension2='{27}',Extension3='{28}',Extension4={29} Where Ctmxxh ={19} ";
                    sqlstr = string.Format(sqlstr, cpd.Yzbz, cpd.Cdxh, cpd.Ggxh, cpd.Lcxh, cpd.Ypdm, cpd.Ypmc, cpd.Xmlb, cpd.Ypjl, cpd.Jldw, cpd.Yfdm, cpd.Pcdm, cpd.Zxcs, cpd.Zxzq, cpd.Zxzqdw,
                                  cpd.Zdm, cpd.Zxsj, cpd.Zxts, cpd.Ztnr, cpd.Yzlb, ctmxxh, cpd.Zxdw, cpd.Mzdm, cpd.Jjlx, cpd.Zxksdm, cpd.Yzkx, cpd.Extension,cpd.Extension1,cpd.Extension2,cpd.Extension3,cpd.Extension4);
                    sqlstr2 = " Update CP_AdviceGroupDetail Set  Yfdm='{0}',Pcdm='{1}',Zxcs={2},Zxzq={3},Zxzqdw={4},Zdm='{5}',Zxsj='{6}',Zxts={7}  Where Fzxh={8}";
                    sqlstr2 = string.Format(sqlstr2, cpd.Yfdm, cpd.Pcdm, cpd.Zxcs, cpd.Zxzq, cpd.Zxzqdw,
                                  cpd.Zdm, cpd.Zxsj, cpd.Zxts, fzxh);
                    sqlstr = sqlstr + sqlstr2;

                }
                else
                {
                    sqlstr = "Update CP_AdviceGroupDetail set Yzbz={0},Cdxh={1},Ggxh={2},Lcxh={3},Ypdm='{4}',Ypmc='{5}',Xmlb={6},Zxdw='{20}',"
                           + "Ypjl={7},Jldw='{8}',Yfdm='{9}',Pcdm='{10}',Zxcs={11},Zxzq={12},Zxzqdw={13},Zdm='{14}',Zxsj='{15}',Zxts={16},Ztnr='{17}',Yzlb={18},Mzdm='{21}',Isjj={22} ,Zxksdm='{23}',"
                            + " Yzkx={24},Extension='{25}', Extension1='{26}',Extension2='{27}',Extension3='{28}',Extension4={29} Where Ctmxxh ={19} ";
                    sqlstr = string.Format(sqlstr, cpd.Yzbz, cpd.Cdxh, cpd.Ggxh, cpd.Lcxh, cpd.Ypdm, cpd.Ypmc, cpd.Xmlb, cpd.Ypjl, cpd.Jldw, cpd.Yfdm, cpd.Pcdm, cpd.Zxcs, cpd.Zxzq, cpd.Zxzqdw,
                                  cpd.Zdm, cpd.Zxsj, cpd.Zxts, cpd.Ztnr, cpd.Yzlb, ctmxxh, cpd.Zxdw, cpd.Mzdm, cpd.Jjlx, cpd.Zxksdm, cpd.Yzkx, cpd.Extension, cpd.Extension1, cpd.Extension2, cpd.Extension3, cpd.Extension4);

                }

                SqlHelper.ExecuteNoneQuery(sqlstr);

                return "更新成功";

            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return ex.Message;
            }

        }   // zm 8.24 Oracle

        /// <summary>
        /// 手术代码库
        /// </summary>
        /// <param name="ssdm"></param>
        /// <returns></returns>
        [OperationContract]
        public List<CP_Operation> GetOperationInfo(string ssdm)
        {
            List<CP_Operation> cplist = new List<CP_Operation>();

            try
            {
                string sqlstr = string.Empty;
                if (!string.IsNullOrEmpty(ssdm))
                {
                    sqlstr = "Select * from Cp_Operation Where Yxjl=1 and Ssdm='" + ssdm + "' ";
                }
                else
                {
                    sqlstr = "Select  * from Cp_Operation Where Yxjl=1 ";
                }


                DataTable dt = SqlHelper.ExecuteDataTable(sqlstr);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_Operation cp = new CP_Operation();
                    cp.Ssdm = dr["Ssdm"].ToString();
                    cp.Ysdm = dr["Ysdm"].ToString();
                    cp.Name = dr["Name"].ToString();
                    cp.Sslb = Convert.ToInt16(dr["Sslb"].ToString());
                    cp.Py = dr["Py"].ToString();
                    cp.Wb = dr["Wb"].ToString();
                    cp.Yxjl = Convert.ToInt16(dr["Yxjl"].ToString());
                    cp.Memo = dr["Memo"].ToString();
                    cplist.Add(cp);
                }
                return cplist;

            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }           // zm 8.24 Oracle
        /// <summary>
        /// 麻醉代码库
        /// </summary>
        /// <param name="mzdm"></param>
        /// <returns></returns>
        [OperationContract]
        public List<CP_Anesthesia> GetAnesthesiaInfo(string mzdm)
        {
            List<CP_Anesthesia> cplist = new List<CP_Anesthesia>();

            try
            {
                string sqlstr = string.Empty;
                if (!string.IsNullOrEmpty(mzdm))
                {
                    sqlstr = "Select * from CP_Anesthesia Where Yxjl=1 and mzdm='" + mzdm + "' ";
                }
                else
                {
                    sqlstr = "Select * from CP_Anesthesia Where Yxjl=1 ";
                }

                DataTable dt = SqlHelper.ExecuteDataTable(sqlstr);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_Anesthesia cp = new CP_Anesthesia();
                    cp.Mzdm = dr["Mzdm"].ToString();
                    cp.Ysdm = dr["Ysdm"].ToString();
                    cp.Name = dr["Name"].ToString();
                    cp.Py = dr["Py"].ToString();
                    cp.Wb = dr["Wb"].ToString();
                    cp.Yxjl = Convert.ToInt16(dr["Yxjl"].ToString());
                    cp.Memo = dr["Memo"].ToString();
                    cplist.Add(cp);
                }
                return cplist;

            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }          // zm 8.24 Oracle
        [OperationContract]
        public string InsertIntoAdviceAnesthesiaDetail(CP_AdviceAnesthesiaDetail cpd, string nodeid, int fzbz, string nodename)
        {
            //using (SqlConnection cn = new SqlConnection(m_ConnectionString))
            //{
            try
            {
                decimal ctyzxh = GetCtyzxh(nodeid, nodename, String.Empty);
                string sqlstr = "Insert into CP_AdviceAnesthesiaDetail Values(" + ctyzxh + "," + cpd.Yzbz + "," + cpd.Fzxh + "," + fzbz + ",'" + cpd.Ssdm + "','" + cpd.Ypmc + "','"
                     + cpd.Mzdm + "'," + cpd.Xmlb + ",'" + cpd.Zxdw + "'," + cpd.Dwlb + ",'" + cpd.Ztnr + "'," + cpd.Yzlb + ",'" + cpd.Memo + "')";
                SqlHelper.ExecuteNoneQuery(sqlstr);

                return AdviceGroupInfo;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return ex.Message;
            }
        }  // zm 8.24 Oracle
        /// <summary>
        /// 获取手术内容
        /// </summary>
        /// <param name="nodeid"></param>
        /// <returns></returns>
        [OperationContract]
        public List<CP_AdviceAnesthesiaDetail> GetAdviceAnesthesiaDetail(string nodeid)
        {
            List<CP_AdviceAnesthesiaDetail> cplist = new List<CP_AdviceAnesthesiaDetail>();

            try
            {
                decimal ctyzxh = GetCtyzxh(nodeid, "", String.Empty);
                string sqlstr = "select *,cd.[Name] as YzbzName ,Case When Fzbz !=3500 then '┃' else '' End as Flag,'在 '+ ca.Name +' '+'下'+' '+'行'+' '+cp.Ssmc+' ' as Yznr  from CP_AdviceAnesthesiaDetail "
                        + " cp left join CP_DataCategoryDetail cd on cd.Mxbh=cp.Yzbz Left join CP_Anesthesia ca on ca.Mzdm=cp.Mzdm"
                       + "  Where cp.Ctyzxh=" + ctyzxh + " order by Fzxh,Ctmxxh";

                DataTable dt = SqlHelper.ExecuteDataTable(sqlstr);

                int k = 0;
                foreach (DataRow dr in dt.Rows)
                {
                    Yidansoft.Service.Entity.CP_AdviceAnesthesiaDetail cp = new CP_AdviceAnesthesiaDetail();
                    cp.Ctmxxh = Convert.ToDecimal(dr["Ctmxxh"].ToString());
                    cp.Ctyzxh = Convert.ToDecimal(dr["Ctyzxh"].ToString());
                    cp.Yzbz = Convert.ToInt16(dr["Yzbz"].ToString());
                    cp.Fzxh = Convert.ToDecimal(dr["Fzxh"].ToString());
                    cp.Fzbz = Convert.ToInt16(dr["Fzbz"].ToString());
                    cp.Ssdm = dr["Ssdm"].ToString();
                    cp.Ypmc = dr["Ssmc"].ToString();
                    cp.Xmlb = Convert.ToInt16(string.IsNullOrEmpty(dr["Xmlb"].ToString()) ? "0" : dr["Xmlb"].ToString());
                    cp.Zxdw = dr["Zxdw"].ToString();
                    cp.Mzdm = dr["Mzdm"].ToString();
                    cp.Dwlb = Convert.ToInt16(string.IsNullOrEmpty(dr["Dwlb"].ToString()) ? "0" : dr["Dwlb"].ToString());
                    cp.Ztnr = dr["Ztnr"].ToString();
                    cp.Yzlb = Convert.ToInt16(string.IsNullOrEmpty(dr["Yzlb"].ToString()) ? "0" : dr["Yzlb"].ToString());
                    cp.YzbzName = dr["YzbzName"].ToString();
                    cp.Flag = dr["Flag"].ToString();
                    cp.Index = k;
                    cp.Yznr = dr["Yznr"].ToString();
                    cplist.Add(cp);
                    k++;
                }
                return cplist;

            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }       // zm 8.24 Oracle
        [OperationContract]
        public string UpdateSSAdviceDetail(CP_AdviceAnesthesiaDetail cpd)
        {

            try
            {
                string sqlstr = string.Empty;
                sqlstr = "Update CP_AdviceAnesthesiaDetail set Yzbz={0},Fzxh={1},Fzbz={2},Ssdm='{3}',Ssmc='{4}',Mzdm='{5}',Xmlb={6},Zxdw='{7}',Dwlb={8},Ztnr='{9}',Yzlb={10},Memo='{11}' Where Ctmxxh={12}";
                sqlstr = string.Format(sqlstr, cpd.Yzbz, cpd.Fzxh, cpd.Fzbz, cpd.Ssdm, cpd.Ypmc, cpd.Mzdm, cpd.Xmlb, cpd.Zxdw, cpd.Dwlb, cpd.Ztnr, cpd.Yzlb, cpd.Memo, cpd.Ctmxxh);

                SqlHelper.ExecuteNoneQuery(sqlstr);

                return "更新成功";

            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return ex.Message;
            }

        }       // zm 8.24 Oracle
        #endregion

        #region 变异编码
        /// <summary>
        /// 按条件获取数据列表
        /// </summary>
        /// <param name="variationtype"></param>
        /// <returns></returns>
        [OperationContract]
        public List<CP_PathVariation> GetDataPathVariationList(int queryType, String variationRange, int codeType, int codeState, string variationCode)
        {
            List<CP_PathVariation> cplist = new List<CP_PathVariation>();

            try
            {
                string where = "";

                //计算查询条件
                if (queryType == 1)//模糊查询
                {
                    //(codeType*4-1)为3一级编码，为7二级编码，为11三级编码
                    string levelCode = (codeType == 0 ? "" : "and len(Bydm)=" + (codeType * 4 - 1).ToString() + " ");

                    string letter = "";
                    if (variationCode != "") letter = variationCode.Substring(0, 1).ToUpper();
                    if (letter == "P" || letter == "D" || letter == "S" || letter == "C")//带字母前缀
                    {
                        where = " where Bydm like '" + variationCode + "%' " + levelCode;
                    }
                    else//不带带字母前缀
                    {
                        //其他条件                             
                        if (variationRange != "")
                        {
                            if (variationCode == "")//用户没输编码
                            {
                                where = " where Bydm like '" + variationRange.Substring(0, 1) + "%' " + levelCode;
                            }
                            else//用户输入编码
                            {
                                where = " where Bydm like '" + variationRange.Substring(0, 1) + "%" + variationCode + "%' " + levelCode;
                            }
                        }

                        else//全部
                        {
                            if (variationCode == "")//用户没输编码
                            {
                                where = (codeType == 0 ? "" : " where len(Bydm)=" + (codeType * 4 - 1).ToString() + " "); //编码等级  
                            }
                            else//用户输入编码
                            {
                                where = "where Bydm like '%" + variationCode + "%'" + levelCode;
                            }
                        }
                    }

                    //计算编码状态
                    if (where == "")
                    {
                        where = (codeState == 0) ? "" : " where Yxjl=" + (2 - codeState).ToString();
                    }
                    else
                    {
                        where += (codeState == 0) ? "" : " and Yxjl=" + (2 - codeState).ToString();
                    }

                }
                else //queryType==1 精确查询
                {
                    where = " where Bydm='" + variationCode + "'";
                }

                string sql = "select CP_PathVariation.*,(case Yxjl when 1 then '启用'when 0 then '停用' end) as state from CP_PathVariation " + where;


                DataTable dt = SqlHelper.ExecuteDataTable(sql);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_PathVariation cp = new CP_PathVariation();
                    cp.Bydm = dr["Bydm"].ToString();
                    cp.Bymc = dr["Bymc"].ToString();
                    cp.Byms = dr["Byms"].ToString();
                    cp.Yxjl = Convert.ToInt32(dr["Yxjl"].ToString());
                    cp.State = dr["state"].ToString();
                    cp.Py = dr["Py"].ToString();
                    cplist.Add(cp);
                }
                return cplist;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }       // zm 8.24 Oracle
        /// <summary>
        /// 获取全部变异编码,2011-4-18 fqw new 
        /// </summary>
        /// <param name="variationtype"></param>
        /// <returns></returns>
        [OperationContract]
        public List<CP_PathVariation> GetDataPathVariationListAll()
        {
            List<CP_PathVariation> cplist = new List<CP_PathVariation>();

            try
            {
                string sql = "select *  from CP_PathVariation  ";


                DataTable dt = SqlHelper.ExecuteDataTable(sql);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_PathVariation cp = new CP_PathVariation();
                    cp.Bydm = dr["Bydm"].ToString();
                    cp.Bymc = dr["Bymc"].ToString();
                    cp.Byms = dr["Byms"].ToString();
                    cp.Yxjl = Convert.ToInt32(dr["Yxjl"].ToString());

                    cp.Py = dr["Py"].ToString();
                    cplist.Add(cp);
                }
                return cplist;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }


        }       // zm 8.24 Oracle
        /// <summary>
        /// 设置编码使用状态
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="state"></param>
        [OperationContract]
        public void SetPathVariationCodeState(List<string> lst, int state)
        {

            try
            {
                StringBuilder sql = new StringBuilder();



                foreach (string Bydm in lst)
                {
                    sql.Clear();
                    sql.Append("update CP_PathVariation set ");
                    sql.Append(" Yxjl=" + state.ToString());
                    sql.Append(" where ");
                    sql.Append(" Bydm='" + Bydm + "'");

                    SqlHelper.ExecuteNoneQuery(sql.ToString());
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }

        }       // zm 8.24 Oracle

        /// <summary>
        /// 添加新变异编码
        /// </summary>
        /// <param name="bydm"></param>
        /// <param name="bymc"></param>
        /// <param name="byms"></param>
        /// <param name="yxjl"></param>
        /// <param name="py"></param>
        [OperationContract]
        public void AddVariationCode(string bydm, string bymc, string byms, int yxjl, string py)
        {

            try
            {
                StringBuilder sql = new StringBuilder();

                sql.Append("insert into CP_PathVariation ");
                sql.Append(" (bydm,bymc,byms,yxjl,py) ");
                sql.Append(" values ");
                sql.Append("('" + bydm + "','");
                sql.Append(bymc + "','");
                sql.Append(byms + "',");
                sql.Append(yxjl + ",'");
                sql.Append(py + "')");


                SqlHelper.ExecuteNoneQuery(sql.ToString());
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }


        }       // zm 8.24 Oracle

        /// <summary>
        /// 获取需归类的编码信息列表
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        public List<CP_VariantRecords> GetVariantRecords(string key)
        {
            List<CP_VariantRecords> cplist = new List<CP_VariantRecords>();

            try
            {
                string sql;
                if (key == null || key == string.Empty)
                {
                    sql = "select * from CP_VariantRecords where Bydm='9999'";
                }
                else
                {
                    sql = string.Format(@" select * from CP_VariantRecords where Bydm='9999'and (Bynr like '%{0}%' or Byyy like '%{0}%') ", key);
                }


                DataTable dt = SqlHelper.ExecuteDataTable(sql);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_VariantRecords cp = new CP_VariantRecords();
                    cp.Id = Convert.ToInt32(dr["Id"].ToString());
                    cp.Syxh = Convert.ToDecimal(dr["Syxh"].ToString());
                    cp.Ljdm = dr["Ljdm"].ToString();
                    cp.Mxdm = dr["Mxdm"].ToString();
                    cp.Ypdm = dr["Ypdm"].ToString();
                    cp.Bylb = dr["Bylb"].ToString();
                    cp.Bylx = dr["Bylx"].ToString();
                    cp.Bynr = dr["Bynr"].ToString();
                    cp.Bydm = dr["Bydm"].ToString();
                    cp.Byyy = dr["Byyy"].ToString();
                    cp.Bysj = dr["Bysj"].ToString();
                    cplist.Add(cp);
                }
                return cplist;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }       // zm 8.24 Oracle

        /// <summary>
        /// 更新需要归类的编码信息表
        /// </summary>
        /// <param name="lst"></param>
        /// <param name="code"></param>
        [OperationContract]
        public void UpdateVariantRecords(List<int> lstID, string code)
        {

            try
            {
                StringBuilder sql = new StringBuilder();


                foreach (int cpID in lstID)
                {
                    sql.Clear();
                    sql.Append("update CP_VariantRecords set ");
                    sql.Append(" Bydm='" + code + "' ");
                    sql.Append(" where ");
                    sql.Append(" Id=" + cpID + "");

                    SqlHelper.ExecuteNoneQuery(sql.ToString());
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }

        }        // zm 8.24 Oracle

        /// <summary>
        /// 获取三级编码
        /// </summary>
        [OperationContract]
        public List<CP_PathVariation> GetThirdCodeList()
        {
            List<CP_PathVariation> cplist = new List<CP_PathVariation>();
            try
            {


                string sql = @"select cp.*,(cp.Bymc+'_'+cp.Bydm) as CodeGroup from CP_PathVariation cp where len(cp.Bydm)=11 and cp.Yxjl=1";

                DataTable dt = SqlHelper.ExecuteDataTable(sql);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_PathVariation cp = new CP_PathVariation();
                    cp.Bydm = dr["Bydm"].ToString();
                    cp.Bymc = dr["Bymc"].ToString();
                    cp.Byms = dr["Byms"].ToString();
                    cp.Yxjl = Convert.ToInt32(dr["Yxjl"].ToString());
                    cp.CodeGroup = dr["CodeGroup"].ToString();
                    cp.Py = dr["Py"].ToString();
                    cplist.Add(cp);
                }
                return cplist;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }    // zm 8.24 Oracle

        #endregion

        #region dxj 获取1级编码

        /// <summary>
        /// 获取一级编码
        /// </summary>
        /// <returns>一级编码集合</returns>
        [OperationContract]
        public List<CP_PathVariation> GetFirstCodeList()
        {
            List<CP_PathVariation> cplist = new List<CP_PathVariation>();
            CP_PathVariation cp = new CP_PathVariation();
            cp.Bydm = String.Empty;
            cp.Bymc = "全部";
            cplist.Add(cp);

            try
            {
                string sql = "SELECT * FROM CP_PathVariation WHERE Bydm NOT LIKE'%.%'";



                DataTable dt = SqlHelper.ExecuteDataTable(sql);

                foreach (DataRow dr in dt.Rows)
                {
                    CP_PathVariation pathVariation = new CP_PathVariation();
                    pathVariation.Bydm = dr["Bydm"].ToString();
                    pathVariation.Bymc = dr["Bymc"].ToString();
                    cplist.Add(pathVariation);
                }
                return cplist;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }           // zm 8.24 Oracle

        #endregion

        #region APPCFG配置表维护

        // add by luff 20130221
        #region 新增APPCFG配置表
        /// <summary>
        /// 新增APPCFG临床路径参数配置操作
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="Operation"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Boolean AppCfgInsert(APPCFG model)
        {


            Boolean returnBool = false;
            try
            {
                if (model == null)
                {
                    model = new APPCFG();
                    returnBool = false;

                }
                else
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("insert into APPCFG(");
                    strSql.Append("Configkey,Name,Value,Descript,ParamType,Cfgkeyset,Design,ClientFlag,Hide,Valid)");
                    strSql.Append(" values (");
                    strSql.Append("@Configkey,@Name,@Value,@Descript,@ParamType,@Cfgkeyset,@Design,@ClientFlag,@Hide,@Valid)");
                    SqlParameter[] parameters = {
					new SqlParameter("@Configkey", SqlDbType.VarChar,32),
					new SqlParameter("@Name", SqlDbType.VarChar,64),
					new SqlParameter("@Value", SqlDbType.Text),
					new SqlParameter("@Descript", SqlDbType.VarChar,255),
					new SqlParameter("@ParamType", SqlDbType.Int,4),
					new SqlParameter("@Cfgkeyset", SqlDbType.VarChar,1024),
					new SqlParameter("@Design", SqlDbType.VarChar,255),
					new SqlParameter("@ClientFlag", SqlDbType.SmallInt,2),
					new SqlParameter("@Hide", SqlDbType.SmallInt,2),
					new SqlParameter("@Valid", SqlDbType.SmallInt,2)};
                    parameters[0].Value = model.Configkey;
                    parameters[1].Value = model.Name;
                    parameters[2].Value = model.Value;
                    parameters[3].Value = model.Descript;
                    parameters[4].Value = model.ParamType;
                    parameters[5].Value = model.Cfgkeyset;
                    parameters[6].Value = model.Design;
                    parameters[7].Value = model.ClientFlag;
                    parameters[8].Value = model.Hide;
                    parameters[9].Value = model.Valid;


                    //DataTable dt = null;
                    SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                    returnBool = true;

                }
            }

            catch (Exception ex)
            {
                ThrowException(ex);
                returnBool = false;
            }
            return returnBool;

        }
        #endregion

        #region 更新APPCFG配置表
        /// <summary>
        /// 更新EhrtoHis配置表操作
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="Operation"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Boolean APPCFGUpdate(APPCFG model)
        {
            Boolean returnBool = false;
            try
            {
                if (model == null)
                {
                    model = new APPCFG();
                    returnBool = false;

                }
                else
                {
                    StringBuilder strSql = new StringBuilder();
                    strSql.Append("update APPCFG set ");
                    strSql.Append("Name=@Name,");
                    strSql.Append("Value=@Value,");
                    strSql.Append("Descript=@Descript,");
                    strSql.Append("ParamType=@ParamType,");
                    strSql.Append("Cfgkeyset=@Cfgkeyset,");
                    strSql.Append("Design=@Design,");
                    strSql.Append("ClientFlag=@ClientFlag,");
                    strSql.Append("Hide=@Hide,");
                    strSql.Append("Valid=@Valid");
                    strSql.Append(" where Configkey=@Configkey ");
                    SqlParameter[] parameters = {
					
					new SqlParameter("@Name", SqlDbType.VarChar,64),
					new SqlParameter("@Value", SqlDbType.Text),
					new SqlParameter("@Descript", SqlDbType.VarChar,255),
					new SqlParameter("@ParamType", SqlDbType.Int,4),
					new SqlParameter("@Cfgkeyset", SqlDbType.VarChar,1024),
					new SqlParameter("@Design", SqlDbType.VarChar,255),
					new SqlParameter("@ClientFlag", SqlDbType.SmallInt,2),
					new SqlParameter("@Hide", SqlDbType.SmallInt,2),
					new SqlParameter("@Valid", SqlDbType.SmallInt,2),
                    new SqlParameter("@Configkey", SqlDbType.VarChar,32)};

                    parameters[0].Value = model.Name;
                    parameters[1].Value = model.Value;
                    parameters[2].Value = model.Descript;
                    parameters[3].Value = model.ParamType;
                    parameters[4].Value = model.Cfgkeyset;
                    parameters[5].Value = model.Design;
                    parameters[6].Value = model.ClientFlag;
                    parameters[7].Value = model.Hide;
                    parameters[8].Value = model.Valid;
                    parameters[9].Value = model.Configkey;

                    //DataTable dt = null;
                    SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                    returnBool = true;
                }


            }

            catch (Exception ex)
            {
                ThrowException(ex);
                returnBool = false;
            }
            return returnBool;
        }


        /// <summary>
        ///插入并查询 His接口配置
        /// </summary>
        /// <param name="iID">-1,表示获取所有配置信息，否则是有条件筛选配置信息</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<APPCFG> GetAppCfg(string sKey)
        {
            List<APPCFG> list = new List<APPCFG>();
            try
            {
                string sSql = "";
                //if (parameter == null) parameter = new HisSxpz();
                if (sKey == "-1")
                {
                    sSql = "select * from APPCFG where Hide=1 ";
                }
                else
                {
                    sSql = "select * from APPCFG where Hide=1 and Configkey='" + sKey + "'";
                }

                DataTable dt = null;

                dt = SqlHelper.ExecuteDataTable(sSql);


                if (dt != null)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        APPCFG model = new APPCFG();

                        model.Configkey = item["Configkey"].ToString();


                        model.Name = item["Name"].ToString();


                        model.Value = item["Value"].ToString();


                        model.Descript = item["Descript"].ToString();


                        model.ParamType = int.Parse(item["ParamType"].ToString());


                        model.Cfgkeyset = item["Cfgkeyset"].ToString();


                        model.Design = item["Design"].ToString();

                        if (item["ClientFlag"] != null && item["ClientFlag"].ToString() != "")
                        {
                            model.ClientFlag = int.Parse(item["ClientFlag"].ToString());
                        }
                        else
                        {
                            model.ClientFlag = 0;
                        }
                        if (item["Hide"] != null && item["Hide"].ToString() != "")
                        {
                            model.Hide = int.Parse(item["Hide"].ToString());
                        }
                        else
                        {
                            model.Hide = 1;
                        }
                        if (item["Valid"] != null && item["Valid"].ToString() != "")
                        {
                            model.Valid = int.Parse(item["Valid"].ToString());
                        }
                        else
                        {
                            model.Valid = 1;
                        }
                        list.Add(model);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }
        #endregion

        #region 删除APPCFG配置表
        /// <summary>
        /// 删除APPCFG配置表操作
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="Operation"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Boolean APPCFGDel(string sKey)
        {


            Boolean returnBool = false;
            try
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from APPCFG ");
                strSql.Append(" where Configkey=@Configkey");
                SqlParameter[] parameters = {
					 new SqlParameter("@Configkey", SqlDbType.VarChar,32)};
                parameters[0].Value = sKey;
                SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                returnBool = true;


            }

            catch (Exception ex)
            {
                ThrowException(ex);
                returnBool = false;
            }
            return returnBool;

        }
        #endregion

        #region 假删除APPCFG配置表 把Hide值更新为0，1表示参数正常使用（显示）
        /// <summary>
        /// 假删除APPCFG配置表操作
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="Operation"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Boolean APPCFGHidDel(string sKey)
        {


            Boolean returnBool = false;
            try
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("update APPCFG set Hide=0 ");
                strSql.Append(" where Configkey=@Configkey");
                SqlParameter[] parameters = {
					 new SqlParameter("@Configkey", SqlDbType.VarChar,32)};
                parameters[0].Value = sKey;
                SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                returnBool = true;


            }

            catch (Exception ex)
            {
                ThrowException(ex);
                returnBool = false;
            }
            return returnBool;

        }
        #endregion

        #endregion
    }
}