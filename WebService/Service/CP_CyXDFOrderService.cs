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
        /// 查询某个病人的长期医嘱
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_CyXDFOrder> GetCyXDFOrderListBelongToSomeOne(String where)
        {
            List<CP_CyXDFOrder> cplist = new List<CP_CyXDFOrder>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT 
            CP_CyXDFOrder.Cyfyzxh,CP_CyXDFOrder.Cymc,

            Convert(varchar(100),CAST(Ksrq AS DATETIME),23) AS StartDate ,
            Convert(varchar(100),CAST(Ksrq AS DATETIME),8) AS StartTime,

            Lrysdm,yisheng.Name AS LrysdmName,
            Yznr,Isjj,Zxksdm,Yzkx,Extension,Extension1,Extension2,Extension3,Extension4,
           CASE  Fzbz WHEN 3501 THEN '┓' WHEN 3509 then '┛' WHEN 3502 then '┃'  else '' END AS FzbzSymbol,

            Convert(varchar(100),CAST(Zxrq AS DATETIME),23) AS exeDate ,
            Convert(varchar(100),CAST(Zxrq AS DATETIME),8) AS exeTime,
 
            Zxczy,hushi.Name AS ZxczyName,Yzzt,CP_DataCategoryDetail.Name AS YzztName
            FROM CP_CyXDFOrder
            LEFT JOIN Users yisheng ON yisheng.ID=CP_CyXDFOrder.Lrysdm
            LEFT JOIN Users hushi ON hushi.ID=CP_CyXDFOrder.Zxczy
            LEFT JOIN CP_DataCategoryDetail ON  CP_DataCategoryDetail.Mxbh=CP_CyXDFOrder.Yzzt AND CP_DataCategoryDetail.Lbbh=32
            INNER JOIN InPatient ON InPatient.NoOfInpat=CP_CyXDFOrder.Syxh
                                    where 1=1  ");
                sb.Append(where);
                sb.Append(" ORDER BY CP_CyXDFOrder.Fzxh,CP_CyXDFOrder.Fzbz");

                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    CP_CyXDFOrder cp = new CP_CyXDFOrder();
                    cp.StartDate = dr["StartDate"].ToString();
                    cp.StartTime = dr["StartTime"].ToString();
                    cp.LrysdmName = dr["LrysdmName"].ToString();
                    cp.ZxczyName = dr["ZxczyName"].ToString();
                    cp.exeDate = dr["exeDate"].ToString();
                    cp.exeTime = dr["exeTime"].ToString();
                    cp.FzbzSymbol = dr["FzbzSymbol"].ToString();
                    cp.Yznr = dr["Yznr"].ToString();

                    cp.Jjlx = int.Parse(dr["Isjj"].ToString());
                    cp.Zxksdm = dr["Zxksdm"].ToString();
                    cp.Yzkx = dr["Yzkx"] == null ? 0 : int.Parse(dr["Yzkx"].ToString());
                    cp.Extension = dr["Extension"].ToString();
                    cp.Extension1 = dr["Extension1"].ToString();
                    cp.Extension2 = dr["Extension2"].ToString();
                    cp.Extension3 = dr["Extension3"].ToString();
                    cp.Extension4 = dr["Extension4"].ToString() == string.Empty ? 0 : Convert.ToInt16(dr["Extension4"].ToString());
                    cp.Yzzt = ConvertMy.ToDecimal(dr["Yzzt"].ToString());
                    cp.Lsyzxh = ConvertMy.ToDecimal(dr["Cyfyzxh"].ToString());
                    cp.Cfmc = dr["Cfmc"].ToString();
                    cplist.Add(cp);
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
        /// 根据据路径节点,得到节点对应中草药医嘱
        /// </summary>
        /// <param name="strPathDetailID"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_DoctorOrder> GetPathInitCYOrder(string strPathDetailID, string strSyxh, CP_Employee employee, string sLjdm,string sLjxh)
        {
            List<CP_DoctorOrder> listOrder = new List<CP_DoctorOrder>();

            List<CP_DoctorOrder> cplist = new List<CP_DoctorOrder>();
            try
            {
                decimal ctyzxh = GetCtyzxh(strPathDetailID, "", sLjdm);

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@Ljxh",sLjxh),
                        new SqlParameter("@ActivityId",strPathDetailID),
                        new SqlParameter("@ctyzxh",ctyzxh)
                    };

                DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_GetCyXDFOrderByPath", parameters, CommandType.StoredProcedure);
                DataTable dt = new DataTable();
                if (ds.Tables[0].Rows.Count > 0)
                {
                    dt = ds.Tables[0];
                }
                else
                {
                    dt = ds.Tables[1];
                }
                int k = 0;
                foreach (DataRow row in dt.Rows)
                {
                    CP_DoctorOrder order = new CP_DoctorOrder();
                    order.Yzxh = Convert.ToDecimal(row["Cyfyzxh"].ToString() == string.Empty ? "0" : row["Cyfyzxh"].ToString());//
                    order.Ctmxxh = Convert.ToDecimal(row["Ctmxxh"].ToString() == string.Empty ? "0" : row["Ctmxxh"].ToString());//
                    order.Syxh = Convert.ToDecimal(strSyxh);
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
                    order.Ksrq = GetDefaultOrderTime(OrderType.Normal);
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


                    order.Zxcs = Convert.ToDecimal(row["Zxcs"].ToString() == string.Empty ? "0" : row["Zxcs"].ToString());
                    order.Zxzq = Convert.ToDecimal(row["Zxzq"].ToString() == string.Empty ? "0" : row["Zxzq"].ToString());
                    order.Zxzqdw = Convert.ToDecimal(row["Zxzqdw"].ToString() == string.Empty ? "0" : row["Zxzqdw"].ToString());
                    order.Zdm = row["Zdm"].ToString();
                    order.Zxsj = row["Zxsj"].ToString();
                    //add by luff 20130313
                    order.Jjlx = int.Parse(row["Isjj"].ToString());
                    order.Yzkx = int.Parse(row["Yzkx"].ToString());
                    order.Zxksdm = row["Zxksdm"].ToString();
                    order.Yznr = row["Yznr"].ToString();
                    order.Ztnr = row["Ztnr"].ToString();
                    order.Yzzt = Convert.ToDecimal(row["Yzzt"].ToString() == string.Empty ? "0" : row["Yzzt"].ToString());

                    cplist.Add(order);
                }
                return cplist;

            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }
        }       // zm 8.25 Oracle


        /// <summary>
        /// add by luff 20130723 根据据路径节点,得到节点对应中草药医嘱 ibval为0 返回路径执行中节点对应草药医嘱；1为返回路径维护中节点对应草药医嘱
        /// </summary>
        /// <param name="strPathDetailID"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_DoctorOrder> GetPathInitCYOrder2(string strPathDetailID, string strSyxh, CP_Employee employee, string sLjdm, string sLjxh,int ibval)
        {
            List<CP_DoctorOrder> listOrder = new List<CP_DoctorOrder>();

            List<CP_DoctorOrder> cplist = new List<CP_DoctorOrder>();
            try
            {
                decimal ctyzxh = GetCtyzxh(strPathDetailID, "", sLjdm);

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@Ljxh",sLjxh),
                        new SqlParameter("@ActivityId",strPathDetailID),
                        new SqlParameter("@ctyzxh",ctyzxh)
                    };

                DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_GetCyXDFOrderByPath", parameters, CommandType.StoredProcedure);
                DataTable dt = new DataTable();
                if (ibval == 0)
                {
                    dt = ds.Tables[0];//返回路径执行中节点对应草药医嘱
                }
                else if (ibval ==1)
                {
                    dt = ds.Tables[1];//返回路径维护中节点对应草药医嘱
                }
                else
                {
                    return null;
                }
                
                //int k = 0;
                foreach (DataRow row in dt.Rows)
                {
                    CP_DoctorOrder order = new CP_DoctorOrder();
                    order.Yzxh = Convert.ToDecimal(row["Cyfyzxh"].ToString() == string.Empty ? "0" : row["Cyfyzxh"].ToString());//
                    order.Ctmxxh = Convert.ToDecimal(row["Ctmxxh"].ToString() == string.Empty ? "0" : row["Ctmxxh"].ToString());//
                    order.Syxh = Convert.ToDecimal(strSyxh);
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
                    order.Ksrq = GetDefaultOrderTime(OrderType.Normal);
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


                    order.Zxcs = Convert.ToDecimal(row["Zxcs"].ToString() == string.Empty ? "0" : row["Zxcs"].ToString());
                    order.Zxzq = Convert.ToDecimal(row["Zxzq"].ToString() == string.Empty ? "0" : row["Zxzq"].ToString());
                    order.Zxzqdw = Convert.ToDecimal(row["Zxzqdw"].ToString() == string.Empty ? "0" : row["Zxzqdw"].ToString());
                    order.Zdm = row["Zdm"].ToString();
                    order.Zxsj = row["Zxsj"].ToString();
                    //add by luff 20130313
                    order.Jjlx = int.Parse(row["Isjj"].ToString());
                    order.Yzkx = int.Parse(row["Yzkx"].ToString());
                    order.Zxksdm = row["Zxksdm"].ToString();
                    order.Yznr = row["Yznr"].ToString();
                    order.Ztnr = row["Ztnr"].ToString();
                    order.Yzzt = Convert.ToDecimal(row["Yzzt"].ToString() == string.Empty ? "0" : row["Yzzt"].ToString());

                    cplist.Add(order);
                }
                return cplist;

            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }
        }  


        /// <summary>
        /// 更新临时医嘱的状态
        /// </summary>
        /// <param name="CP_CyXDFOrderLists">临时医嘱列表</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public void UpdateCyXDFOrderListYzzt(List<CP_CyXDFOrder> CP_CyXDFOrderLists)
        {
            try
            {
                foreach (var item in CP_CyXDFOrderLists)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(@" update   CP_CyXDFOrder set Yzzt='{0}', {1}=convert(char(10),getdate(),120) where Cyfyzxh='{2}'", item.Yzzt, ColorKeyValuesProperty[item.Yzzt.ToString()].Value, item.Lsyzxh);
                    SqlHelper.ExecuteNoneQuery(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }

        }

        /// <summary>
        /// 更新草药协定方医嘱,add by luff 20130517
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="orderAdd"></param>
        /// <param name="currentList"></param>
        /// <param name="strPathDetailID"></param> 
        private void UpdateCyXDFOrder(CP_DoctorOrder orderModify,
            CP_InpatinetList currentList)
        {
            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@Cfmc",orderModify.Extension3),
                    new SqlParameter("@Syxh",orderModify.Syxh),
                    new SqlParameter("@Fzxh",orderModify.Fzxh),
                    new SqlParameter("@Fzbz",orderModify.Fzbz),
                    new SqlParameter("@Bqdm",orderModify.Bqdm),
                    new SqlParameter("@Ksdm",orderModify.Ksdm),
                    new SqlParameter("@Lrysdm",orderModify.Lrysdm),
                    new SqlParameter("@Lrrq",DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd")),
                    new SqlParameter("@Cdxh",orderModify.Cdxh),
                    new SqlParameter("@Ggxh",orderModify.Ggxh),
                    new SqlParameter("@Lcxh",orderModify.Lcxh),
                    new SqlParameter("@Ypdm",orderModify.Ypdm),
                    new SqlParameter("@Ypmc",orderModify.Ypmc),
                    new SqlParameter("@Xmlb",orderModify.Xmlb),
                    new SqlParameter("@Zxdw",orderModify.Zxdw == null ? string.Empty : orderModify.Zxdw),
                    new SqlParameter("@Ypjl",orderModify.Ypjl),
                    new SqlParameter("@Jldw",orderModify.Jldw == null ? string.Empty : orderModify.Jldw),
                    new SqlParameter("@Dwxs",orderModify.Dwxs),
                    new SqlParameter("@Dwlb",orderModify.Dwlb),
                    new SqlParameter("@Yfdm",orderModify.Yfdm == null ? string.Empty : orderModify.Yfdm),
                    new SqlParameter("@Pcdm",orderModify.Pcdm == null ? string.Empty : orderModify.Pcdm),
                    new SqlParameter("@Zxcs",orderModify.Zxcs),
                    new SqlParameter("@Zxzq",orderModify.Zxzq),
                    new SqlParameter("@Zxzqdw",orderModify.Zxzqdw),
                    new SqlParameter("@Zdm",orderModify.Zdm == null ? string.Empty : orderModify.Zdm),
                    new SqlParameter("@Zxsj",orderModify.Zxsj == null ? string.Empty : orderModify.Zxsj),
                    new SqlParameter("@Ztnr",orderModify.Ztnr == null ? string.Empty : orderModify.Ztnr),
                    new SqlParameter("@Yzlb",orderModify.Yzlb),
                    new SqlParameter("@Yzzt",orderModify.Yzzt),
                    new SqlParameter("@Tsbj",orderModify.Tsbj),
                    new SqlParameter("@Yznr",orderModify.Yznr == null ? string.Empty : orderModify.Yznr),
                    new SqlParameter("@Tbbz",orderModify.Tbbz),
                    new SqlParameter("@Memo",orderModify.Memo == null ? string.Empty : orderModify.Memo),
                    new SqlParameter("@Ksrq",orderModify.Ksrq),
                    new SqlParameter("@Ypgg",orderModify.Ypgg),
                    new SqlParameter("@Zxksdm",orderModify.Zxksdm== null ? string.Empty : orderModify.Zxksdm),
                    new SqlParameter("@Isjj",orderModify.Jjlx),
                    new SqlParameter("@Yzkx",orderModify.Yzkx),
                    new SqlParameter("@Yzxh",orderModify.Yzxh),
                    new SqlParameter("@Ctmxxh",orderModify.Ctmxxh),
                };

                SqlHelper.ExecuteNoneQuery("usp_CP_UpdateCyXDFOrder", parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }    

        /// <summary>
        /// 保存草药处方医嘱, add by luff 20130517
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="orderAdd"></param>     
        /// <param name="currentList">病患实体</param>
        /// <param name="strActivityID">活动ID</param>
        /// <param name="strActivityChildID">活动子结点ID</param>  
        /// <param name="strLjdm">结点所属路径</param>
        /// <returns></returns>
        private decimal SaveCyXDFOrder(CP_DoctorOrder orderAdd,
            CP_InpatinetList currentList, string strActivityID, string strActivityChildID, string strLjdm)
        {
            try
            {
                //SqlCommand myCommand = new SqlCommand("usp_CP_InsertTempOrder", myConnection, sqlTrans);
                //myCommand.CommandType = CommandType.StoredProcedure;
                //添加输入查询参数、赋予值                

                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@Cfmc",orderAdd.Extension3),
                    new SqlParameter("@Syxh",orderAdd.Syxh),
                    new SqlParameter("@Fzxh",orderAdd.Fzxh),
                    new SqlParameter("@Fzbz",orderAdd.Fzbz),
                    new SqlParameter("@Bqdm",orderAdd.Bqdm),

                    new SqlParameter("@Ksdm",orderAdd.Ksdm),
                    new SqlParameter("@Lrysdm",orderAdd.Lrysdm),
                    new SqlParameter("@Lrrq",DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd")),
                    new SqlParameter("@Cdxh",orderAdd.Cdxh),
                    new SqlParameter("@Ggxh",orderAdd.Ggxh),

                    new SqlParameter("@Lcxh",orderAdd.Lcxh),
                    new SqlParameter("@Ypdm",orderAdd.Ypdm),
                    new SqlParameter("@Ypmc",orderAdd.Ypmc),
                    new SqlParameter("@Xmlb",orderAdd.Xmlb),
                    new SqlParameter("@Zxdw",orderAdd.Zxdw == null ? string.Empty : orderAdd.Zxdw),

                    new SqlParameter("@Ypjl",orderAdd.Ypjl),
                    new SqlParameter("@Jldw",orderAdd.Jldw == null ? string.Empty : orderAdd.Jldw),
                    new SqlParameter("@Dwxs",orderAdd.Dwxs),
                    new SqlParameter("@Dwlb",orderAdd.Dwlb),
                    new SqlParameter("@Yfdm",orderAdd.Yfdm == null ? string.Empty : orderAdd.Yfdm),

                    new SqlParameter("@Pcdm",orderAdd.Pcdm == null ? string.Empty : orderAdd.Pcdm),
                    new SqlParameter("@Zxcs",orderAdd.Zxcs),
                    new SqlParameter("@Zxzq",orderAdd.Zxzq),
                    new SqlParameter("@Zxzqdw",orderAdd.Zxzqdw),
                    new SqlParameter("@Zdm",orderAdd.Zdm == null ? string.Empty : orderAdd.Zdm),

                    new SqlParameter("@Zxsj",orderAdd.Zxsj == null ? string.Empty : orderAdd.Zxsj),
                    new SqlParameter("@Ztnr",orderAdd.Ztnr == null ? string.Empty : orderAdd.Ztnr),
                    new SqlParameter("@Yzlb",orderAdd.Yzlb),
                    new SqlParameter("@Yzzt",orderAdd.Yzzt),
                    new SqlParameter("@Tsbj",orderAdd.Tsbj),

                    new SqlParameter("@Yznr",orderAdd.Yznr == null ? string.Empty : orderAdd.Yznr),
                    new SqlParameter("@Tbbz",orderAdd.Tbbz),
                    new SqlParameter("@Memo",orderAdd.Memo == null ? string.Empty : orderAdd.Memo),
                    new SqlParameter("@Ksrq",orderAdd.Ksrq),
                    new SqlParameter("@Ypgg",orderAdd.Ypgg),

                    new SqlParameter("@Zxksdm",orderAdd.Zxksdm== null ? string.Empty : orderAdd.Zxksdm),
                    new SqlParameter("@Isjj",orderAdd.Jjlx),
                    new SqlParameter("@Yzkx",orderAdd.Yzkx),
                    new SqlParameter("@Ctmxxh",orderAdd.Ctmxxh),
                    new SqlParameter("@Ljdm",strLjdm),

                    new SqlParameter("@ActivityID",strActivityID),
                    new SqlParameter("@ActivityChildID",strActivityChildID),
                    new SqlParameter("@Ljxh",currentList.Ljxh)
                };

                Decimal decimalReturn = -1;
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_InsertCyXDFOrder", parameters, CommandType.StoredProcedure);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != null)
                {
                    decimalReturn = ConvertMy.ToDecimal(dt.Rows[0][0].ToString().Trim());
                }
                return decimalReturn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// 删除草药处方医嘱
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="orderDel"></param>
        private void DelCYOrder(CP_DoctorOrder orderDel)
        {
            string strSql = string.Empty;

            strSql = "UPDATE CP_CyXDFOrder SET Yzzt = 3203 WHERE Cyfyzxh = {0}";
            
            string strCmd = string.Format(strSql, orderDel.Yzxh);

            //SqlCommand cmd = new SqlCommand(strCmd, myConnection, sqlTrans);
            //cmd.ExecuteNoneQuery();

            SqlHelper.ExecuteNoneQuery(strCmd);
        }

        /// <summary>
        /// 保存新增草药处方医嘱  ,add by yxy
        /// </summary>
        /// <param name="sqlTrans"></param>
        /// <param name="myConnection"></param>
        /// <param name="listOrderAdd">新增医嘱</param>
        /// <param name="currentList">病患实体</param>
        /// <param name="strActivityID">活动ID</param>
        /// <param name="strActivityChildID">活动子结点ID</param>  
        /// <param name="strLjdm">结点所属路径</param>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public void SaveAddCYOrder(List<CP_DoctorOrder> listOrderAdd, List<CP_DoctorOrder> listOrderModify, List<CP_DoctorOrder> listOrderDel,
             CP_InpatinetList currentList, string strActivityID, string strActivityChildID, string strLjdm)
        {
            Dictionary<decimal, decimal> dicCYOrderFzxh = new Dictionary<decimal, decimal>();
            
            //批量保存新增医嘱
            foreach (CP_DoctorOrder order in listOrderAdd)
            {

                //order.Fzxh = dicCYOrderFzxh[order.Fzxh];
                SaveCyXDFOrder(order, currentList, strActivityID, strActivityChildID, strLjdm);

            }

            //批量保存修改医嘱
            foreach (CP_DoctorOrder order in listOrderModify)
            {
                UpdateCyXDFOrder(order, currentList);
            }

            //批量保存删除医嘱
            foreach (CP_DoctorOrder order in listOrderDel)
            {
                DelCYOrder(order);
            }
        }

    }
}