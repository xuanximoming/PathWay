using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using YidanSoft.Core;

namespace SendOrder
{
    public class SendOrderToHis_BT
    {
        IDataAccess m_sqlhelper;

        public SendOrderToHis_BT()
        {
            m_sqlhelper = DataAccessFactory.GetSqlDataAccess("HISDB");
        }

        /// <summary>
        /// 发送医嘱到标腾HIS
        /// </summary>
        /// <param name="orderAdd">发送医嘱表</param>
        public DataTable SendOrder(DataTable orderAdd)
        {

            string mess = string.Empty;
            foreach (DataRow dr in orderAdd.Select("FS_Flag='1'"))
            {
                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@yzxh",dr["yzxh"].ToString()),
                    new SqlParameter("@ctyzxh ",dr["ctyzxh"].ToString()),
                    new SqlParameter("@yzbz",dr["yzbz"].ToString()),
                    new SqlParameter("@PatNoOfHis ",dr["NoofClinic"].ToString()),
                    new SqlParameter("@NoOfRecord",dr["NoOfRecord"].ToString()),
                    new SqlParameter("@Syxh",dr["Syxh"].ToString()),
                    new SqlParameter("@PatID",dr["PatID"].ToString()),
                    
                    new SqlParameter("@ksmc",dr["ksmc"].ToString()),
                    new SqlParameter("@Qxrq",dr["Qxrq"].ToString()),
                    new SqlParameter("@Qxysdm",dr["Qxysdm"].ToString()),
                    new SqlParameter("@OutBed",dr["OutBed"].ToString()),
                    new SqlParameter("@Fzxh",dr["Fzxh"].ToString()),
                    new SqlParameter("@Fzbz",dr["Fzbz"].ToString()),

                    new SqlParameter("@Bqdm",dr["Bqdm"].ToString()),
                    new SqlParameter("@Ksdm",dr["Ksdm"].ToString()),

                    
                    new SqlParameter("@Lrysdm",dr["Lrysdm"].ToString()),
                    new SqlParameter("@Lrrq",DateTime.Now.ToString("yyyy-MM-dd HH:mm:dd")),
                    new SqlParameter("@Cdxh",dr["Cdxh"].ToString()),
                    new SqlParameter("@Ggxh",dr["Ggxh"].ToString()),
                    new SqlParameter("@Ypdm",dr["Ypdm"].ToString()),
                    new SqlParameter("@Ypmc",dr["Ypmc"].ToString()),

                    new SqlParameter("@Xmlb",dr["Xmlb"].ToString()),
                    new SqlParameter("@Zxdw",dr["Zxdw"] == null ? string.Empty : dr["Zxdw"].ToString()),
                    new SqlParameter("@Ypjl",dr["Ypjl"].ToString()),
                    new SqlParameter("@Jldw",dr["Jldw"]== null ? string.Empty : dr["Jldw"].ToString()),

                    new SqlParameter("@Ypzsl",-1),
                    new SqlParameter("@Ypdj",-1),
                    new SqlParameter("@Je",-1),
	 
                    new SqlParameter("@Dwxs",dr["Dwxs"].ToString()),
                    new SqlParameter("@Dwlb",dr["Dwlb"].ToString()),
                    new SqlParameter("@Yfdm",dr["Yfdm"] == null ? string.Empty : dr["Yfdm"].ToString()),
                    new SqlParameter("@Pcdm",dr["Pcdm"] == null ? string.Empty : dr["Pcdm"].ToString()),
                    new SqlParameter("@Zxcs",dr["Zxcs"].ToString()),
                    
                    new SqlParameter("@Zxzq",dr["Zxzq"].ToString()),
                    new SqlParameter("@Zxzqdw",dr["Zxzqdw"].ToString()),
                    new SqlParameter("@Zdm",dr["Zdm"] == null ? string.Empty : dr["Zdm"].ToString()),
                    new SqlParameter("@Zxsj",dr["Zxsj"] == null ? string.Empty : dr["Zxsj"].ToString()),
                    new SqlParameter("@Ztnr",dr["Ztnr"] == null ? string.Empty : dr["Ztnr"].ToString()),
                    new SqlParameter("@Yzlb",dr["Yzlb"].ToString()),
                    new SqlParameter("@Yzzt",dr["Yzzt"].ToString()),
                    new SqlParameter("@Tsbj",dr["Tsbj"].ToString()),
                    new SqlParameter("@Yznr",dr["Yznr"] == null ? string.Empty : dr["Yznr"].ToString()),
                    new SqlParameter("@Memo",dr["Memo"] == null ? string.Empty : dr["Memo"].ToString()),
                    new SqlParameter("@Ksrq",DateTime.Parse(dr["Ksrq"].ToString()).ToString("yyyyMMddHHmmdd")),
                    new SqlParameter("@Ypgg",dr["Ypgg"].ToString()),
                    new SqlParameter("@Ctmxxh",dr["Ctmxxh"].ToString())
                };


                try
                {
                    DataTable dt = m_sqlhelper.ExecuteDataTable("root.EMR_usp_InsertOrder", parameters, CommandType.StoredProcedure);

                    //根据HIS返回结果集判断是否插入成功
                    //if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() != "0")
                    //{
                    //成套医嘱其中一条发送不成功则所有医嘱都不发送
                    foreach (DataRow row in orderAdd.Rows)
                    {
                        row["FS_Flag"] = "1";
                        row["FS_Mess"] = dt.Rows[0][0].ToString();
                        //if (row["Ctyzxh"].ToString() == dr["Ctyzxh"].ToString())
                        //{

                        //}
                    }
                    //dr["FS_Flag"] = "0";
                    //dr["FS_Mess"] = dt.Rows[0][0].ToString();
                    //}
                    //else
                    //{

                    //}
                    //if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != null)
                    //{
                    //    mess = dt.Rows[0][0].ToString().Trim();
                    //}
                    //else
                    //{ 

                    //}
                }
                catch (Exception ex)
                {
                    foreach (DataRow row in orderAdd.Rows)
                    {
                        row["FS_Flag"] = "0";
                        row["FS_Mess"] = "发送失败！";
                    }


                    throw ex;
                }

            }
            //return decimalReturn;
            return orderAdd;
        }
    }
}
