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
using DrectSoft.Tool;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 查询摸个病人的长期医嘱
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_TempOrderList> GetTempOrderListBelongToSomeOne(String where)
        {
            List<CP_TempOrderList> cplist = new List<CP_TempOrderList>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT 
            CP_TempOrder.Lsyzxh,

            Convert(varchar(100),CAST(Ksrq AS DATETIME),23) AS StartDate ,
            Convert(varchar(100),CAST(Ksrq AS DATETIME),8) AS StartTime,

            Lrysdm,yisheng.Name AS LrysdmName,
            Yznr,Isjj,Zxksdm,Yzkx,Extension,Extension1,Extension2,Extension3,Extension4,
           CASE  Fzbz WHEN 3501 THEN '┓' WHEN 3509 then '┛' WHEN 3502 then '┃'  else '' END AS FzbzSymbol,

            Convert(varchar(100),CAST(Zxrq AS DATETIME),23) AS exeDate ,
            Convert(varchar(100),CAST(Zxrq AS DATETIME),8) AS exeTime,
 
            Zxczy,hushi.Name AS ZxczyName,Yzzt,CP_DataCategoryDetail.Name AS YzztName
            FROM CP_TempOrder
            LEFT JOIN Users yisheng ON yisheng.ID=CP_TempOrder.Lrysdm
            LEFT JOIN Users hushi ON hushi.ID=CP_TempOrder.Zxczy
            LEFT JOIN CP_DataCategoryDetail ON  CP_DataCategoryDetail.Mxbh=CP_TempOrder.Yzzt AND CP_DataCategoryDetail.Lbbh=32
            INNER JOIN InPatient ON InPatient.NoOfInpat=CP_TempOrder.Syxh
                                    where 1=1  ");
                sb.Append(where);
                sb.Append(" ORDER BY CP_TempOrder.OrderValue, CP_TempOrder.Fzxh,CP_TempOrder.Fzbz");//mod by 20130829 新增OrderValue这个字段降序

                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    CP_TempOrderList cp = new CP_TempOrderList();
                    cp.StartDate = dr["StartDate"].ToString();
                    cp.StartTime = dr["StartTime"].ToString();
                    cp.LrysdmName = dr["LrysdmName"].ToString();
                    cp.ZxczyName = dr["ZxczyName"].ToString();
                    cp.exeDate = dr["exeDate"].ToString();
                    cp.exeTime = dr["exeTime"].ToString();
                    cp.FzbzSymbol = dr["FzbzSymbol"].ToString();
                    cp.Yznr = dr["Yznr"].ToString();
                    //add by luff 20130121
                    cp.Jjlx = int.Parse(dr["Isjj"].ToString());
                    cp.Zxksdm = dr["Zxksdm"].ToString();
                    cp.Yzkx = dr["Yzkx"] == null ? 0 : int.Parse(dr["Yzkx"].ToString());
                    cp.Extension = dr["Extension"].ToString();
                    cp.Extension1 = dr["Extension1"].ToString();
                    cp.Extension2 = dr["Extension2"].ToString();
                    cp.Extension3 = dr["Extension3"].ToString();
                    cp.Extension4 = dr["Extension4"].ToString() == string.Empty ? 0 : Convert.ToInt16(dr["Extension4"].ToString()); 
                    cp.Yzzt = ConvertMy.ToDecimal(dr["Yzzt"].ToString());
                    cp.Lsyzxh = ConvertMy.ToDecimal(dr["Lsyzxh"].ToString());
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
        /// 更新临时医嘱的状态
        /// </summary>
        /// <param name="CP_TempOrderLists">临时医嘱列表</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public void UpdateTempOrderListYzzt(List<CP_TempOrderList> CP_TempOrderLists)
        {
            try
            {
                foreach (var item in CP_TempOrderLists)
                {
                    StringBuilder sb = new StringBuilder();
                    //sb.AppendFormat(@" update   CP_TempOrder set Yzzt='{0}', {1}=to_char(sysdate,'yyyy-mm-dd HH24:MI:SS') where Lsyzxh='{2}'", item.Yzzt, ColorKeyValuesProperty[item.Yzzt.ToString()].Value, item.Lsyzxh);
                    sb.AppendFormat(@" update   CP_TempOrder set Yzzt='{0}', {1}=convert(char(10),getdate(),120) where Lsyzxh='{2}'", item.Yzzt, ColorKeyValuesProperty[item.Yzzt.ToString()].Value, item.Lsyzxh);
                    SqlHelper.ExecuteNoneQuery(sb.ToString());
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }

        }
    }
}