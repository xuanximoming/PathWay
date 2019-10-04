using DrectSoft.Tool;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.ServiceModel;
using Yidansoft.Service.Entity;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 表示插入并查询提醒表的方法，当首页序号和路径代码已经在提醒表中存在时，只进行查询
        /// </summary>
        /// <param name="syxh">首页序号</param>
        /// <param name="ljxh">路径序号</param>
        /// <param name="ljdm">路径代码</param>
        /// <param name="jddm">节点代码</param>
        /// <returns>返回</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_MedicalTreatmentWarmGroupByTxlx> InsertAndSelectCP_MedicalTreatmentWarmGroupByTxlx(String syxh, String ljxh, String ljdm, String jddm)
        {
            try
            {
                List<CP_MedicalTreatmentWarmGroupByTxlx> CP_MedicalTreatmentWarmTemps = new List<CP_MedicalTreatmentWarmGroupByTxlx>();
                List<Object> objArr = new List<Object>();
                DataSet ds = (DataSet)CP_MedicalTreatmentWarmMaintain(syxh, ljxh, ljdm, Operation.InsertAndSelect, null, jddm);
                objArr = GetCP_MedicalTreatmentWarm(objArr, ds);
                CP_MedicalTreatmentWarmTemps = (List<CP_MedicalTreatmentWarmGroupByTxlx>)objArr[1];
                return CP_MedicalTreatmentWarmTemps;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }
        }

        /// <summary>
        /// 表示更新并查询提醒表的方法
        /// </summary>
        /// <param name="id">提醒表主键字符串，当id等于空，不做更新操作，只做查询操作</param>
        /// <param name="jddm">节点代码</param>
        /// <param name="ljdm">路径代码</param>
        /// <param name="syxh">首页序号</param>
        /// <param name="ljxh">路径序号</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_MedicalTreatmentWarm> UpdateAndSelectCP_MedicalTreatmentWarm(String id, String jddm, String ljdm, String syxh, String ljxh)
        {
            try
            {
                List<CP_MedicalTreatmentWarm> CP_MedicalTreatmentWarmTemps = new List<CP_MedicalTreatmentWarm>();
                List<Object> objArr = new List<Object>();
                DataSet ds = (DataSet)CP_MedicalTreatmentWarmMaintain(syxh, ljxh, ljdm, Operation.UpdateAndSelect, id, jddm);
                objArr = GetCP_MedicalTreatmentWarm(objArr, ds);
                CP_MedicalTreatmentWarmTemps = (List<CP_MedicalTreatmentWarm>)objArr[0];
                return CP_MedicalTreatmentWarmTemps;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }
        }

        /// <summary>
        /// 表示维护路径执行提前提醒表的方法
        /// </summary>
        /// <param name="syxh">首页序号</param>
        /// <param name="ljxh">路径序号</param>
        /// <param name="ljdm">路径代码</param>
        /// <param name="operation">操作符</param>
        /// <param name="id">主键</param>
        /// <returns></returns>
        private Object CP_MedicalTreatmentWarmMaintain(String syxh, String ljxh, String ljdm, Operation operation, String id, String jddm)
        {
            //@Syxh NUMERIC(9, 0)=1,	--首页序号
            //@Ljxh NUMERIC(9, 0)=1,	--路径序号
            //@Ljdm varchar(12)='',		--临床路径代码（CP_ClinicalPath.Ljdm)
            //@Operation VARCHAR(10)='insert',	--操作符（insert插入操作，select查询操作，update更新操作）
            //@ID NUMERIC(9, 0)=1		--CP_MedicalTreatmentWarm主键
            Object returnObj = null;
            SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@Syxh",syxh),
                    new SqlParameter("@Ljxh",ljxh),
                    new SqlParameter("@Ljdm",ljdm),
                    new SqlParameter("@Operation",operation.ToString()),
                    new SqlParameter("@ID",id),
                    new SqlParameter("@Jddm",jddm),
                };
            DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_MedicalTreatmentWarm", param, CommandType.StoredProcedure);
            returnObj = ds;
            return returnObj;
        }


        /// <summary>
        /// 表示把表转化成类实体的方法
        /// </summary>
        /// <param name="objArr"></param>
        /// <param name="ds"></param>
        public List<Object> GetCP_MedicalTreatmentWarm(List<Object> objArr, DataSet ds)
        {
            List<CP_MedicalTreatmentWarm> CP_MedicalTreatmentWarmsTemp = new List<CP_MedicalTreatmentWarm>();
            List<CP_MedicalTreatmentWarmGroupByTxlx> CP_MedicalTreatmentWarmGroupByTxlxTemps = new List<CP_MedicalTreatmentWarmGroupByTxlx>();
            if (ds.Tables.Count > 0)
                foreach (DataRow item in ds.Tables[0].Rows)
                {
                    DataRow dr = item;
                    CP_MedicalTreatmentWarm CP_MedicalTreatmentWarmTemp = new CP_MedicalTreatmentWarm();

                    CP_MedicalTreatmentWarmTemp.dm = ConvertMy.ToString(dr["dm"]);
                    CP_MedicalTreatmentWarmTemp.ID = ConvertMy.ToString(dr["ID"]);
                    CP_MedicalTreatmentWarmTemp.Jddm = ConvertMy.ToString(dr["Jddm"]);
                    CP_MedicalTreatmentWarmTemp.Ljdm = ConvertMy.ToString(dr["Ljdm"]);
                    CP_MedicalTreatmentWarmTemp.Ljxh = ConvertMy.ToString(dr["Ljxh"]);

                    CP_MedicalTreatmentWarmTemp.mc = ConvertMy.ToString(dr["mc"]);
                    CP_MedicalTreatmentWarmTemp.Syxh = ConvertMy.ToString(dr["Syxh"]);
                    CP_MedicalTreatmentWarmTemp.Txlx = ConvertMy.ToString(dr["Txlx"]);
                    CP_MedicalTreatmentWarmTemp.Txzt = ConvertMy.ToString(dr["Txzt"]);
                    CP_MedicalTreatmentWarmTemp.jdmc = ConvertMy.ToString(dr["jdmc"]);
                    CP_MedicalTreatmentWarmTemp.TxlxName = ConvertMy.ToString(dr["TxlxName"]);



                    CP_MedicalTreatmentWarmsTemp.Add(CP_MedicalTreatmentWarmTemp);
                }
            if (ds.Tables.Count > 1)
                foreach (DataRow item in ds.Tables[1].Rows)//左下角的弹出提醒填充内容
                {
                    DataRow dr = item;
                    CP_MedicalTreatmentWarmGroupByTxlx CP_MedicalTreatmentWarmGroupByTxlxTemp = new CP_MedicalTreatmentWarmGroupByTxlx();
                    CP_MedicalTreatmentWarmGroupByTxlxTemp.jddm = ConvertMy.ToString(dr["jddm"]);
                    CP_MedicalTreatmentWarmGroupByTxlxTemp.jdmc = ConvertMy.ToString(dr["jdmc"]);
                    CP_MedicalTreatmentWarmGroupByTxlxTemp.Txlx = ConvertMy.ToString(dr["Txlx"]);
                    CP_MedicalTreatmentWarmGroupByTxlxTemp.TxlxName = ConvertMy.ToString(dr["TxlxName"]);
                    CP_MedicalTreatmentWarmGroupByTxlxTemp.txsl = ConvertMy.ToString(dr["txsl"]);
                    CP_MedicalTreatmentWarmGroupByTxlxTemp.Dysl = ConvertMy.ToString(dr["dysl"]);

                    CP_MedicalTreatmentWarmGroupByTxlxTemps.Add(CP_MedicalTreatmentWarmGroupByTxlxTemp);

                }
            if (ds.Tables.Count > 2)//天数和费用
                foreach (DataRow item in ds.Tables[2].Rows)
                {
                    DataRow dr = item;
                    CP_MedicalTreatmentWarm CP_MedicalTreatmentWarmTemp = new CP_MedicalTreatmentWarm();
                    CP_MedicalTreatmentWarmTemp.ID = ConvertMy.ToString(dr["chaobiao"]);
                    CP_MedicalTreatmentWarmTemp.mc = ConvertMy.ToString(dr["mc"]);
                    CP_MedicalTreatmentWarmTemp.Txlx = ConvertMy.ToString(dr["Txlx"]);
                    CP_MedicalTreatmentWarmTemp.TxlxName = ConvertMy.ToString(dr["TxlxName"]);
                    CP_MedicalTreatmentWarmsTemp.Add(CP_MedicalTreatmentWarmTemp);
                }


            objArr.Add(CP_MedicalTreatmentWarmsTemp);
            objArr.Add(CP_MedicalTreatmentWarmGroupByTxlxTemps);
            return objArr;
        }



    }
}