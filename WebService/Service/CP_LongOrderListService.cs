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


        KeyValues _ColorKeyValues = null;

        public KeyValues ColorKeyValuesProperty
        {
            get
            {
                //_ColorKeyValues.Clear();
                if (_ColorKeyValues == null)
                {
                    _ColorKeyValues = new KeyValues();
                    _ColorKeyValues.Add(new KeyValue("3201", "Shrq", "审核"));
                    _ColorKeyValues.Add(new KeyValue("3202", "Zxrq", "执行"));
                    _ColorKeyValues.Add(new KeyValue("3203", "Qxrq", "取消"));
                    _ColorKeyValues.Add(new KeyValue("3204", "Tzrq", "停止"));
                }
                return _ColorKeyValues;
            }
            set { _ColorKeyValues = value; }
        }

        /// <summary>
        /// 查询摸个病人的长期医嘱
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_LongOrderList> GetLongOrderListBelongToSomeOne(String where)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" SELECT 
                  CP_LongOrder.Cqyzxh,
                  Convert(varchar(100),CAST(Ksrq AS DATETIME),23) AS StartDate ,
                  Convert(varchar(100),CAST(Ksrq AS DATETIME),8) AS StartTime,
                   Lrysdm,kaishiyisheng.NAME as LrysdmName,Zxrq,Zxczy,zhixinghushi.NAME as ZxczyName,
                  Convert(varchar(100),CAST(Tzrq AS DATETIME),23) AS EndDate ,
                  Convert(varchar(100),CAST(Tzrq AS DATETIME),8) AS EndTime,
                  tzysdm,Tuichuyisheng.Name AS tzysdmName,
                  CASE  Fzbz WHEN 3501 THEN '┓' WHEN 3509 then '┛' WHEN 3502 then '┃'  else '' END AS FzbzSymbol,
                  CP_LongOrder.Yznr,Isjj,Zxksdm,Yzkx,Extension,Extension1,Extension2,Extension3,Extension4,Yzzt,CP_DataCategoryDetail.Name AS YzztName
                  FROM CP_LongOrder
                  Left JOIN  Users kaishiyisheng    ON kaishiyisheng.ID=CP_LongOrder.Lrysdm
                  LEFT JOIN Users zhixinghushi ON zhixinghushi.ID=CP_LongOrder.Zxczy
                  LEFT JOIN Users Tuichuyisheng ON Tuichuyisheng.ID=CP_LongOrder.tzysdm
                  LEFT JOIN CP_DataCategoryDetail ON  CP_DataCategoryDetail.Mxbh=CP_LongOrder.Yzzt AND CP_DataCategoryDetail.Lbbh=32
                  INNER JOIN InPatient ON InPatient.NoOfInpat=CP_LongOrder.Syxh
                                            where 1=1  ");
                sb.Append(where);
                sb.Append("ORDER BY CP_LongOrder.OrderValue, CP_LongOrder.Fzxh,CP_LongOrder.Fzbz");//mod by 20130829 新增OrderValue这个字段降序 
                List<CP_LongOrderList> cplist = new List<CP_LongOrderList>();
                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    CP_LongOrderList cp = new CP_LongOrderList();
                    cp.StartDate = dr["StartDate"].ToString();
                    cp.StartTime = dr["StartTime"].ToString();
                    cp.LrysdmName = dr["LrysdmName"].ToString();
                    cp.Zxrq = dr["Zxrq"].ToString();
                    cp.ZxczyName = dr["ZxczyName"].ToString();
                    cp.EndDate = dr["EndDate"].ToString();
                    cp.EndTime = dr["EndTime"].ToString();
                    cp.tzysdmName = dr["tzysdmName"].ToString();
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
                    cp.Cqyzxh = ConvertMy.ToDecimal(dr["Cqyzxh"].ToString());
                    cplist.Add(cp);
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
        /// 更新长期医嘱的状态
        /// </summary>
        /// <param name="where">长期医嘱列表</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public void UpdateLongOrderListYzzt(List<CP_LongOrderList> CP_LongOrderLists)
        {
            try
            {
                foreach (var item in CP_LongOrderLists)
                {
                    StringBuilder sb = new StringBuilder();
                    sb.AppendFormat(@" update   CP_LongOrder set Yzzt='{0}', {1}=convert(char(20),getdate(),120)  where Cqyzxh='{2}'", item.Yzzt, ColorKeyValuesProperty[item.Yzzt.ToString()].Value, item.Cqyzxh);
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
