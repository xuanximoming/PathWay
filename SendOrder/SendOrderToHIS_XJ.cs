using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using YidanSoft.Core;
using System.Data;
using System.Data.SqlClient;

namespace SendOrder
{
    /// <summary>
    /// 新疆临床路径发送医嘱到HIS系统中操作类
    /// </summary>
    public class SendOrderToHIS_XJ
    {


        IDataAccess m_sqlhelper;

        public SendOrderToHIS_XJ()
        {
            m_sqlhelper = DataAccessFactory.GetSqlDataAccess("HISDB");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordertable"></param>
        /// <returns></returns>
        public DataTable SendOrder(DataTable ordertable)
        {
            CheckOrder(ordertable);

            SaveOrder(ordertable);
            return ordertable;
        }


        private void CheckOrder(DataTable orderAdd)
        {
            foreach (DataRow dr in orderAdd.Select("yzlb='3100'"))
            {
                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@PhamCode",dr["ypdm"].ToString()),
                    new SqlParameter("@PerformBy",dr["Zxksdm"].ToString()),
                    new SqlParameter("@unit",dr["Jldw"].ToString()),
                    new SqlParameter("@sl",dr["ypjl"].ToString()),
                };

                DataTable dt = m_sqlhelper.ExecuteDataTable("Up_CheckStockQuantity", parameters, CommandType.StoredProcedure);
                // 0	--够用
		
		        // 1	--不够用

                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    foreach (DataRow row in orderAdd.Select("yzlb='3100'"))
                    {
                        if (row["Ctyzxh"].ToString() == dr["Ctyzxh"].ToString())
                        {
                            row["FS_Flag"] = "0";
                            row["FS_Mess"] = "库存不足";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 发送医嘱到HIS
        /// </summary>
        /// <param name="orderAdd">发送医嘱表</param>
        private DataTable SaveOrder(DataTable orderAdd)
        {
            try
            {
                //输入参数： BSTR PatientID,病人编号（NoofClinic）	short VisitID,本次住院号（InCount）
                //short OrderNo,医嘱号（ctxh）		short OrderSubNo,医嘱子序号（）
                //BSTR RepeatIndicator,医嘱长临标志（0-长期，1-临时）	BSTR OrderClass,医嘱分类（1-西药,2-其他,3-文字医嘱）

                //BSTR OrderText,	医嘱内容(Name)	BSTR OrderCode,		医嘱代码(文字医嘱代码为空)
                //BSTR OrderSpec,	规格(Ypgg或者Xmgg)	float Dosage,	药品一次使用剂量(医生录入的数量)
                //BSTR DosageUnit,剂量单位(医生选择的单位)	BSTR Administration,给药途径(静滴等。。。)

                //DATE StartTime,		开始时间（医生下医嘱的时间）	@FreSerialNo 执行频率代码 	
                //BSTR Doctor,		开医嘱医生		BSTR OrderedBy,	医嘱的开单科室		
                //BSTR PerformBy	医嘱执行科室(PerformBy)   @BillingAttr  是否计价标志
                string mess = string.Empty;
                foreach (DataRow dr in orderAdd.Select("FS_Flag='1'"))
                {
                    string iscqyz = string.Empty;


                    //"长期医嘱"  2703
                    //"临时医嘱"  2702
                    if (dr["yzbz"].ToString() == "2703")
                    {
                        iscqyz = "0";
                    }
                    else
                    {
                        iscqyz = "1";
                    }

                    //医嘱分类（1-西药,2-其他,3-文字医嘱）
                    string OrderClass = string.Empty;
                    if (dr["yzlb"].ToString() == "3100")
                    {
                        OrderClass = "1";
                    }
                    else if (dr["yzlb"].ToString() == "3119")
                    {
                        OrderClass = "3";
                    }
                    else
                    {
                        OrderClass = "2";
                    }

                    SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@PatientID",dr["NoofClinic"].ToString()),
                    new SqlParameter("@VisitID",dr["InCount"].ToString()),
                    new SqlParameter("@OrderNo",dr["fzxh"].ToString()),
                    new SqlParameter("@OrderSubNo",dr["yzxh"].ToString()),
                    new SqlParameter("@RepeatIndicator",iscqyz),
                    new SqlParameter("@OrderClass",OrderClass),

                    new SqlParameter("@OrderText",dr["Ypmc"].ToString()=="纯医嘱"?dr["Ztnr"].ToString():dr["Ypmc"].ToString()),
                    new SqlParameter("@OrderCode",dr["Ypdm"].ToString()),
                    new SqlParameter("@OrderSpec",dr["Ypgg"].ToString()),
                    new SqlParameter("@Dosage",dr["Ypjl"].ToString()),
                    new SqlParameter("@DosageUnit",dr["Jldw"].ToString()),
                    new SqlParameter("@FreqDetail",dr["Ztnr"].ToString()),

                    new SqlParameter("@Administration",dr["yfdm"].ToString()),

                    new SqlParameter("@StartTime",dr["Lrrq"].ToString()),
                    new SqlParameter("@FreSerialNo",dr["pcdm"].ToString()),
                    new SqlParameter("@Doctor",dr["Lrysdm"].ToString()),
                    new SqlParameter("@OrderedBy",dr["ksdm"].ToString()),

                    //医嘱执行科室、是否计价暂时默认值 
                    new SqlParameter("@PerformBy",dr["Zxksdm"].ToString()),
                    new SqlParameter("@BillingAttr",dr["Jjlx"].ToString())
                     
                };

                    DataTable dt = m_sqlhelper.ExecuteDataTable("Up_SavaOrderForLCLJ", parameters, CommandType.StoredProcedure);

                    //根据HIS返回结果集判断是否插入成功
                    if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() != "0")
                    {
                        //成套医嘱其中一条发送不成功则所有医嘱都不发送
                        foreach (DataRow row in orderAdd.Select("FS_Flag='1'"))
                        {
                            if (row["Ctyzxh"].ToString() == dr["Ctyzxh"].ToString())
                            {
                                row["FS_Flag"] = "0";
                                row["FS_Mess"] = dt.Rows[0][0].ToString();
                            }
                        }
                        //dr["FS_Flag"] = "0";
                        //dr["FS_Mess"] = dt.Rows[0][0].ToString();
                    }
                    else
                    {

                    }
                }
                return orderAdd;
            }
            catch (Exception e)
            {
                
                throw e;
            }
        }
    }
}
