using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ServiceModel;
using System.Data.SqlClient;
using System.Data;
using DrectSoft.Tool;


namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public RW_PathSummary GetRW_PathSummary(String Syxh, String Ljxh)
        {
            RW_PathSummary rw_pathSummary = new RW_PathSummary();
            List<RW_PathSummaryOrder> rw_pathSummaryOrderList = new List<RW_PathSummaryOrder>();
            List<RW_PathSummaryVariation> rw_pathSummaryVariationList = new List<RW_PathSummaryVariation>();
            List<RW_PathSummaryEnForce> rw_pathSummaryEnForceList = new List<RW_PathSummaryEnForce>();
            List<RW_PathSummaryCategory> rw_pathSummaryCategory = new List<RW_PathSummaryCategory>();

            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@Syxh",Syxh),
                        new SqlParameter("@Ljxh",Ljxh)
                    };

                DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_RWPathSummary", parameters, CommandType.StoredProcedure);

                if (ds.Tables.Count > 0)            //很重要,是否存在医嘱表
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        RW_PathSummaryOrder pathOrder = new RW_PathSummaryOrder();

                        pathOrder.Yzbz = "";
                        pathOrder.Xmlb = ConvertMy.ToString(row["Xmlb"]); ;
                        pathOrder.Ypdm = "";
                        pathOrder.Ypmc = "";
                        pathOrder.Yznr = ConvertMy.ToString(row["Yznr"]);
                        pathOrder.ActivityId = ConvertMy.ToString(row["ActivityId"]);
                        pathOrder.ActivityChildID = ConvertMy.ToString(row["ActivityChildID"]);
                        pathOrder.Yzzt = ConvertMy.ToString(row["Yzzt"]);

                        rw_pathSummaryOrderList.Add(pathOrder);
                    }


                if (ds.Tables.Count > 1)               //很重要，是否存在变异表
                    foreach (DataRow dr in ds.Tables[1].Rows)
                    {
                        RW_PathSummaryVariation pathVariation = new RW_PathSummaryVariation();

                        pathVariation.Xmlb = ConvertMy.ToString(dr["Xmlb"]);
                        pathVariation.Ypdm = "";
                        pathVariation.Ypmc = "";
                        pathVariation.Bynr = ConvertMy.ToString(dr["Bynr"]);
                        pathVariation.PahtDetailId = ConvertMy.ToString(dr["PahtDetailId"]);
                        pathVariation.Mxdm = ConvertMy.ToString(dr["Mxdm"]);

                        rw_pathSummaryVariationList.Add(pathVariation);
                    }

                if (ds.Tables.Count > 2)                 //很重要，是否存在节点执行表
                {
                    foreach (DataRow item in ds.Tables[2].Rows)
                    {
                        RW_PathSummaryEnForce pathSummaryEnForce = new RW_PathSummaryEnForce();

                        pathSummaryEnForce.Jddm = ConvertMy.ToString(item["Jddm"]);
                        pathSummaryEnForce.Ljmc = ConvertMy.ToString(item["Ljmc"]);
                        pathSummaryEnForce.Zxsx = ConvertMy.ToString(item["Zxsx"]);
                        pathSummaryEnForce.Jrsj = ConvertMy.ToString(item["Jrsj"]);

                        rw_pathSummaryEnForceList.Add(pathSummaryEnForce);
                    }
                }
                if (ds.Tables.Count > 3)                 //很重要，是否存在字典表
                {
                    foreach (DataRow item in ds.Tables[3].Rows)
                    {
                        RW_PathSummaryCategory pathSummaryCategory = new RW_PathSummaryCategory();

                        pathSummaryCategory.Cols = ConvertMy.ToInt32(item["Cols"]);
                        pathSummaryCategory.Lb = ConvertMy.ToInt32(item["Lb"]);
                        pathSummaryCategory.Mxbh = ConvertMy.ToString(item["Mxbh"]);
                        pathSummaryCategory.Name = ConvertMy.ToString(item["Name"]);

                        rw_pathSummaryCategory.Add(pathSummaryCategory);
                    }
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            //    finally
            //    {
            //        myConnection.Close();
            //    }
            //}
            rw_pathSummary.PathSummaryOrderList = rw_pathSummaryOrderList;
            rw_pathSummary.PathSummaryVariation = rw_pathSummaryVariationList;
            rw_pathSummary.PathSummaryEnForce = rw_pathSummaryEnForceList;
            rw_pathSummary.PathSummaryCategory = rw_pathSummaryCategory;

            return rw_pathSummary;
        }

        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<RW_PathSummary_new> GetRW_PathSummary_new(String Syxh)
        {
            List<RW_PathSummary_new> rw_pathsummary_newlist = new List<RW_PathSummary_new>();

            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{


            SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@Syxh",Syxh)
                    };

            DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_GetPathOrderBySyxh", parameters, CommandType.StoredProcedure);

            RW_PathSummary_new pathsummary;
            foreach (DataRow row in dt.Rows)
            {
                pathsummary = new RW_PathSummary_new();
                pathsummary.JDMC = row["JDMC"].ToString();
                pathsummary.JDDM = row["JDDM"].ToString();
                pathsummary.YZXH = row["YZXH"].ToString();
                pathsummary.YPMC = row["YPMC"].ToString();
                pathsummary.Ztnr = row["Ztnr"].ToString();
                pathsummary.BYYY = row["BYYY"].ToString();
                pathsummary.Type = row["Yzlb"].ToString();
                pathsummary.YZNR = row["YZNR"].ToString();
                pathsummary.JRSJ = row["pathjrsj"].ToString();

                rw_pathsummary_newlist.Add(pathsummary);
            }


            return rw_pathsummary_newlist;
        }
        /// <summary>
        /// 查询路径原生医嘱
        /// </summary>
        /// <param name="Syxh"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_PathOrdersAll> GetPathOrdersAll(string Syxh)
        {
            List<CP_PathOrdersAll> PathOrdersAlllist = new List<CP_PathOrdersAll>();
            SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@Syxh",Syxh)
                    };

            DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_GetInPatientAdviceAll", parameters, CommandType.StoredProcedure);
            CP_PathOrdersAll pathOrdersAll = new CP_PathOrdersAll();
            DataTable dt=ds.Tables[0];
            DataTable dt1 = ds.Tables[1];
            List<Pathdiagnosis> lsitPathdiagnosis = new List<Pathdiagnosis>();
            for (int j = 0; j < dt1.Rows.Count; j++)
            {

                Pathdiagnosis pathdiagnosis = new Pathdiagnosis();
                pathdiagnosis.ljdm = dt1.Rows[j]["ljdm"].ToString();
                pathdiagnosis.cid = dt1.Rows[j]["icd"].ToString();
                pathdiagnosis.name = dt1.Rows[j]["name"].ToString();
                lsitPathdiagnosis.Add(pathdiagnosis);
               
            }
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                pathOrdersAll = new CP_PathOrdersAll();
                pathOrdersAll.ctyzxh = dt.Rows[i]["ctyzxh"].ToString();
                pathOrdersAll.ypmc = dt.Rows[i]["ypmc"].ToString();
                pathOrdersAll.zxdw = dt.Rows[i]["zxdw"].ToString();
                pathOrdersAll.ypjl = dt.Rows[i]["ypjl"].ToString() + dt.Rows[i]["jldw"].ToString(); ;
                pathOrdersAll.pahtdetailID = dt.Rows[i]["pahtdetailID"].ToString();
                pathOrdersAll.Name = dt.Rows[i]["ljmc"].ToString();
                pathOrdersAll.orderstype = dt.Rows[i]["orderstype"].ToString();
                pathOrdersAll.prepahtdetailid = dt.Rows[i]["prepahtdetailid"].ToString();
                pathOrdersAll.nextpahtdetailid = dt.Rows[i]["nextpahtdetailid"].ToString();

                pathOrdersAll.jldw = dt.Rows[i]["jldw"].ToString();
                pathOrdersAll.zxcs = dt.Rows[i]["zxcs"].ToString();
                pathOrdersAll.nextpahtdetailid = dt.Rows[i]["ztnr"].ToString();
                pathOrdersAll.pathdiagnosis = lsitPathdiagnosis;
             
                PathOrdersAlllist.Add(pathOrdersAll);

            }
            //return Getby(PathOrdersAlllist);

            m_PathOrdersAlllist = new List<CP_PathOrdersAll>();
            GetNewOrderList(PathOrdersAlllist, "");

            return m_PathOrdersAlllist;

        }

     

        List<CP_PathOrdersAll> m_PathOrdersAlllist;

        private void GetNewOrderList(List<CP_PathOrdersAll> list, string detailid)
        {
            CP_PathOrdersAll order = new CP_PathOrdersAll();
            foreach (CP_PathOrdersAll item in list)
            {
                if (item.prepahtdetailid == "" && detailid =="")
                { 
                    m_PathOrdersAlllist.Add(item);
                    order = item;
                }
                else if (item.prepahtdetailid == detailid)
                {
                    m_PathOrdersAlllist.Add(item);
                    order = item;
                }
                
            }
            if (m_PathOrdersAlllist.Count == list.Count)
                return;
            //if (order.nextpahtdetailid == "")
            //    return;
            else if (order.pahtdetailID != "")
                GetNewOrderList(list, order.pahtdetailID);

        }
    }
}