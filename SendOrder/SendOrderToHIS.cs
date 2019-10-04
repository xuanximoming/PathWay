using System;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using YidanSoft.Core;

namespace SendOrder
{
    public class SendOrderToHIS
    {

        IDataAccess _sqlhelper;

        public SendOrderToHIS()
        {
            _sqlhelper = DataAccessFactory.GetSqlDataAccess("HISDB");
        }

        /// <summary>
        /// 处理带引号的字符串
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private string DealQuoteString(string source)
        {
            if (string.IsNullOrEmpty(source))
                return string.Empty;
            else
                return source.Replace("'", "''");
        }

        /// <summary>
        /// 19位日期字符串 -> 16位日期字符串
        /// </summary>
        /// <param name="dt19"></param>
        /// <returns></returns>
        private string DT19ToDT16(string dt19)
        {
            if (string.IsNullOrEmpty(dt19)) return string.Empty;
            return dt19.Replace('-', ' ').Replace(" ", "");
        }

        /// <summary>
        /// Xml形式的字节数组到数据集
        /// </summary>
        /// <param name="bytearray"></param>
        /// <param name="encoding"></param>
        /// <returns></returns>
        private DataSet ByteArray2DataSet(byte[] bytearray, Encoding encoding)
        {
            try
            {
                MemoryStream ms = new MemoryStream();
                ms.Write(bytearray, 0, bytearray.Length);
                ms.Seek(0, SeekOrigin.Begin);
                StreamReader sr = new StreamReader(ms, encoding);
                DataSet dataset = new DataSet();
                dataset.ReadXml(sr, XmlReadMode.ReadSchema);
                return dataset;
            }
            catch
            {
                return null;
            }
        }

        #region 检查医嘱保存


        /// <summary>
        /// 检查医嘱保存
        /// </summary>
        /// <param name="wkdz"></param>
        /// <param name="syxh"></param>
        /// <param name="xmlcqdata"></param>
        /// <param name="iscqls"></param>
        /// <param name="encodingName"></param>
        /// <returns></returns>
        private string CheckAdvises(string wkdz, string syxh, DataTable dtAdvices, bool iscqls, string encodingName)
        {

            string cqlsbz;
            string errMsg = string.Empty;
            string spRet = string.Empty;
            string sqlcmd = string.Empty;

            //Encoding encoding = Encoding.GetEncoding(encodingName);
            //DataSet dsAdvices = this.ByteArray2DataSet(xmlcqdata, encoding);
            //if ((dsAdvices == null) || (dsAdvices.Tables.Count == 0))
            //{
            //    return "F传入的医嘱数据无法解析";
            //}
            try
            {
                _sqlhelper.BeginUseSingleConnection();
                _sqlhelper.BeginTransaction();
                sqlcmd = "exec usp_Emr_CheckDoctorAdvice '" + wkdz + "',1," + syxh;
                DataTable dtret = _sqlhelper.ExecuteDataTable(sqlcmd, CommandType.Text);
                if ((dtret == null) || (dtret.Rows.Count == 0))
                {
                    spRet = "F执行存储步骤1失败!";
                }
                spRet = dtret.Rows[0][0].ToString();
                if (spRet == "F")
                {
                    spRet = "F" + dtret.Rows[0][1].ToString();
                }
                if (string.IsNullOrEmpty(spRet) || (spRet[0] == 'F'))
                {
                    _sqlhelper.EndUseSingleConnection();
                    return spRet;
                }
                errMsg = errMsg + "\r\n" + sqlcmd + " exec step1 ";
            }
            catch (Exception e)
            {
                _sqlhelper.EndUseSingleConnection();
                return (e.Message + errMsg);
            }
            if (iscqls)
            {
                cqlsbz = "0";
            }
            else
            {
                cqlsbz = "1";
            }
            //DataTable dtAdvices = dsAdvices.Tables[0];
            for (int i = 0; i < dtAdvices.Rows.Count; i++)
            {
                try
                {
                    DataRow dr = dtAdvices.Rows[i];
                    string yzxh = "-1";
                    if (iscqls)
                    {
                        yzxh = dr["lsyzxh"].ToString();
                    }
                    else
                    {
                        yzxh = dr["cqyzxh"].ToString();
                    }
                    string idm = dr["cdxh"].ToString();
                    string xmdm = dr["ypdm"].ToString();
                    //此处添加判断 如果其他医嘱编码中有“.”则将“。”后面编码作为项目代码
                    if (dr["ypdm"].ToString().Trim().IndexOf(".") > -1)
                    {
                        xmdm = dr["ypdm"].ToString().Substring(dr["ypdm"].ToString().Trim().IndexOf(".") + 1);
                    }

                    //此处添加判断 如果其他医嘱编码中有“Ss”则将表示为手术医嘱  将Ss去除后为项目代码
                    if (dr["ypdm"].ToString().Trim().IndexOf("Ss") > -1)
                    {
                        xmdm = dr["ypdm"].ToString().Substring(dr["ypdm"].ToString().Trim().IndexOf("Ss") + 2);
                    }


                    string blyzzt = dr["yzzt"].ToString();
                    string sql = "exec usp_Emr_CheckDoctorAdvice '" + wkdz + "', 2," + syxh + "," + cqlsbz + ", " + yzxh + "," + idm + ",'" + xmdm + "'," + blyzzt;
                    errMsg = errMsg + "\r\n" + sql;
                    DataTable dtstep2ret = _sqlhelper.ExecuteDataTable(sql, CommandType.Text);
                    errMsg = errMsg + "\r\n exec step2 " + i.ToString();
                    if ((dtstep2ret == null) || (dtstep2ret.Rows.Count == 0))
                    {
                        spRet = "F执行存储步骤2失败!";
                    }
                    spRet = dtstep2ret.Rows[0][0].ToString();
                    if (spRet == "F")
                    {
                        spRet = "F" + dtstep2ret.Rows[0][1].ToString();
                    }
                    if (string.IsNullOrEmpty(spRet) || (spRet[0] == 'F'))
                    {
                        _sqlhelper.EndUseSingleConnection();
                        return spRet;
                    }
                }
                catch (Exception e)
                {
                    _sqlhelper.EndUseSingleConnection();
                    return (e.Message + errMsg);
                }
            }
            sqlcmd = "exec usp_Emr_CheckDoctorAdvice '" + wkdz + "', 3," + syxh;
            DataTable dtstep3ret = _sqlhelper.ExecuteDataTable(sqlcmd, CommandType.Text);
            if ((dtstep3ret == null) || (dtstep3ret.Rows.Count == 0))
            {
                return "F执行存储步骤3失败!";
            }
            if (dtstep3ret.Rows[0][0].ToString() == "F")
            {
                spRet = "F" + dtstep3ret.Rows[0][1].ToString();
            }
            else if (dtstep3ret.Rows[0][0].ToString() == "-1")
            {
                spRet = "F" + dtstep3ret.Rows[0][1].ToString() + dtstep3ret.Rows[0][2].ToString();
            }
            else if (dtstep3ret.Rows[0][0].ToString() == "T")
            {
                spRet = "T";
            }
            else
            {
                spRet = "F未知错误";
            }
            _sqlhelper.EndUseSingleConnection();
            return (spRet + "\r\n");

        }
        #endregion

        #region 医嘱保存

        /// <summary>
        /// 医嘱保存到His
        /// </summary>
        /// <param name="wkdz"></param>
        /// <param name="syxh"></param>
        /// <param name="xmlcqdata"></param>
        /// <param name="iscqls"></param>
        /// <param name="encodingName"></param>
        /// <param name="lyjzsj">领药截止时间</param>
        /// <returns></returns>
        private string SaveAdvises(string wkdz, string syxh, DataTable dtAdvices, bool iscqls, string encodingName, string lyjzsj)
        {

            string cqlsbz;
            string errMsg = string.Empty;
            string spRet = string.Empty;
            string sqlcmd = string.Empty;
            //Encoding encoding = Encoding.GetEncoding(encodingName);


            //DataSet dsAdvices = this.ByteArray2DataSet(xmlcqdata, encoding);
            //if ((dsAdvices == null) || (dsAdvices.Tables.Count == 0))
            //{
            //    return "F传入的医嘱数据无法解析";
            //}
            //DataTable dtAdvices = dsAdvices.Tables[0];
            //医嘱无数据时直接返回
            if (dtAdvices.Rows.Count == 0)
            {
                return "";
            }
            try
            {
                _sqlhelper.BeginUseSingleConnection();
                sqlcmd = "exec usp_Emr_SaveDoctorAdvice '" + wkdz + "',1," + syxh;
                DataTable dtret = _sqlhelper.ExecuteDataTable(sqlcmd, CommandType.Text);
                if ((dtret == null) || (dtret.Rows.Count == 0))
                {
                    spRet = "F执行存储步骤1失败!";
                }
                spRet = dtret.Rows[0][0].ToString();
                if (spRet == "F")
                {
                    spRet = "F" + dtret.Rows[0][1].ToString();
                }
                if (string.IsNullOrEmpty(spRet) || (spRet[0] == 'F'))
                {
                    _sqlhelper.EndUseSingleConnection();
                    return spRet;
                }
                errMsg = errMsg + "\r\n" + sqlcmd + " exec step1 ";
            }
            catch (Exception e)
            {
                _sqlhelper.EndUseSingleConnection();
                return (e.Message + errMsg);
            }
            if (!iscqls)
            {
                cqlsbz = "0";
            }
            else
            {
                cqlsbz = "1";
            }


            for (int i = 0; i < dtAdvices.Rows.Count; i++)
            {
                try
                {
                    DataRow dr = dtAdvices.Rows[i];
                    string yzno = dr["fzxh"].ToString();
                    string docid = dr["lrysdm"].ToString();
                    string ksrq = this.DT19ToDT16(dr["ksrq"].ToString());

                    //医嘱类别（3100-西药,3111-纯医嘱,3102-收费医嘱）
                    string yzlb = string.Empty;
                    if (dr["yzlb"].ToString() == "3100")
                    {
                        yzlb = "3100";
                    }
                    else if (dr["yzlb"].ToString() == "3119")
                    {
                        yzlb = "3111";
                    }
                    else
                    {
                        yzlb = "3102";
                    }

                    //string yzlb = dr["yzlb"].ToString();
                    string xmlb = dr["xmlb"].ToString();
                    string idm = dr["cdxh"].ToString();
                    string xmdm = dr["ypdm"].ToString();
                    string zxks = dr["zxks"].ToString();
                    string ypjl = dr["ypjl"].ToString();
                    string jldw = dr["jldw"].ToString();
                    string dwbz = dr["dwxs"].ToString();
                    string dwlb = dr["dwlb"].ToString();
                    string yfdm = dr["yfdm"].ToString();
                    string yznr = this.DealQuoteString(dr["yznr"].ToString());
                    string ztnr = this.DealQuoteString(dr["ztnr"].ToString());
                    string pcdm = dr["pcdm"].ToString();
                    string zdm = string.Empty;
                    if (!iscqls)
                    {
                        zdm = dr["zdm"].ToString();
                    }
                    string zxsj = dr["zxsj"].ToString();
                    string zbbz = dr["tsbj"].ToString();
                    if (string.IsNullOrEmpty(zbbz) || (zbbz.Trim() != "1"))
                    {
                        zbbz = "0";
                    }
                    string tzxh = "0";
                    if (iscqls)
                    {
                        tzxh = dr["tzxh"].ToString();
                    }
                    if (string.IsNullOrEmpty(tzxh))
                    {
                        tzxh = "0";
                    }
                    string tzrq = string.Empty;
                    if (!iscqls)
                    {
                        tzrq = this.DT19ToDT16(dr["tzrq"].ToString());
                    }
                    string mzdm = "";
                    string zdys = "";
                    string lcxmdm = string.Empty;
                    if (int.Parse(dr["yzlb"].ToString()) == 0xc1f)
                    {
                        lcxmdm = dr["ypdm"].ToString();
                    }
                    string ypmc = this.DealQuoteString(dr["ypmc"].ToString());
                    //此处添加判断 如果其他医嘱编码中有“.”则将“。”后面编码作为项目代码   且想项目改为临床项目 3103  临床项目序号为“.”前代码
                    if (dr["ypdm"].ToString().Trim().IndexOf(".") > -1)
                    {
                        xmdm = dr["ypdm"].ToString().Substring(dr["ypdm"].ToString().Trim().IndexOf(".") + 1);
                        //ypmc = this.DealQuoteString(dr["ypmc"].ToString()).Substring(dr["ypmc"].ToString().Trim().IndexOf("━") + 1).Substring(dr["ypmc"].ToString().Trim().IndexOf("┏") + 1).Substring(dr["ypmc"].ToString().Trim().IndexOf("┗") + 1).Substring(dr["ypmc"].ToString().Trim().IndexOf("┃") + 1);
                        ypmc = this.DealQuoteString(dr["ypmc"].ToString());
                        lcxmdm = dr["ypdm"].ToString().Substring(0, dr["ypdm"].ToString().Trim().IndexOf("."));
                        yzlb = "3103";
                    }

                    //此处添加判断 如果其他医嘱编码中有“Ss”则将表示为手术医嘱  将Ss去除后为项目代码
                    if (dr["ypdm"].ToString().Trim().IndexOf("Ss") > -1)
                    {
                        xmdm = dr["ypdm"].ToString().Substring(dr["ypdm"].ToString().Trim().IndexOf("Ss") + 2);
                        ypmc = this.DealQuoteString(dr["ypmc"].ToString());
                        lcxmdm = "0";
                        yzlb = "3105";
                    }

                    string sybpno = "0";
                    string sqdxh = "0";
                    if (iscqls)
                    {
                        sqdxh = dr["sqdxh"].ToString();
                    }
                    if (string.IsNullOrEmpty(sqdxh))
                    {
                        sqdxh = "0";
                    }
                    string ybspbz = dr["ybsptg"].ToString();
                    if (string.IsNullOrEmpty(ybspbz))
                    {
                        ybspbz = "0";
                    }
                    string ybspbh = dr["ybspbh"].ToString();
                    string yzxh = "-1";
                    if (iscqls)
                    {
                        yzxh = dr["lsyzxh"].ToString();
                    }
                    else
                    {
                        yzxh = dr["cqyzxh"].ToString();
                    }
                    string zyzxh = dr["fzxh"].ToString();
                    string shczy = dr["shczy"].ToString();
                    string shrq = this.DT19ToDT16(dr["shrq"].ToString());
                    string qxysdm = dr["qxysdm"].ToString();
                    string qxrq = this.DT19ToDT16(dr["qxrq"].ToString());
                    string blyzzt = dr["yzzt"].ToString();
                    string ypzsl = "0";
                    string mq = "0";
                    if (iscqls)
                    {
                        mq = "0";
                    }
                    else
                    {
                        mq = ((dr["mq"] == null) || (dr["mq"] == DBNull.Value)) ? "0" : dr["mq"].ToString();
                    }
                    if (string.IsNullOrEmpty(mq))
                    {
                        mq = "0";
                    }
                    if (iscqls)
                    {
                        ypzsl = dr["ypzsl"].ToString();
                    }
                    if (string.IsNullOrEmpty(lyjzsj))
                    {
                        lyjzsj = "08";
                    }
                    string memo = dr["memo"].ToString();
                    string bbzlId = "";
                    string bbzl = "";
                    string sqdxmbz = "";
                    string sqdjjbz = "0";
                    if (sqdxh != "0")
                    {
                        string[] lstmemo = memo.Split(new char[] { '`' }, StringSplitOptions.None);
                        if (lstmemo.Length > 0)
                        {
                            bbzlId = lstmemo[0];
                        }
                        if (lstmemo.Length > 1)
                        {
                            bbzl = lstmemo[1];
                        }
                        if (lstmemo.Length > 2)
                        {
                            sqdxmbz = lstmemo[2];
                        }
                        if (lstmemo.Length > 3)
                        {
                            sqdjjbz = lstmemo[3];
                        }
                        if (string.IsNullOrEmpty(sqdjjbz))
                        {
                            sqdjjbz = "0";
                        }
                        memo = "";
                    }
                    string sql = "exec usp_Emr_SaveDoctorAdvice '" + wkdz + "', 2," + syxh + ",'00'," + cqlsbz + ", " + yzno + ",'" + docid + "'," + yzlb + ",'" + ksrq + "'," + xmlb + "," + idm + ",'" + xmdm + "','" + zxks + "'," + ypjl + ",'" + jldw + "'," + dwbz + "," + dwlb + ",'" + yfdm + "','" + yznr + "','" + ztnr + "','" + pcdm + "','" + zdm + "','" + zxsj + "'," + zbbz + ",0,0," + tzxh + ",'" + tzrq + "','" + mzdm + "','" + zdys + "'," + ybspbz + ",'" + ybspbh + "','" + lcxmdm + "','" + ypmc + "'," + sybpno + "," + sqdxh + "," + yzxh + "," + zyzxh + ",'" + shczy + "','" + shrq + "','" + qxysdm + "','" + qxrq + "'," + blyzzt + "," + ypzsl + "," + mq + ",'" + lyjzsj + "','" + memo + "','" + sqdxmbz + "','" + bbzlId + "','" + bbzl + "'," + sqdjjbz;
                    errMsg = errMsg + "\r\n" + sql;
                    DataTable dtstep2ret = _sqlhelper.ExecuteDataTable(sql, CommandType.Text);
                    errMsg = errMsg + "\r\n exec step2 " + i.ToString();
                    if ((dtstep2ret == null) || (dtstep2ret.Rows.Count == 0))
                    {
                        spRet = "F执行存储步骤2失败!";
                    }
                    spRet = dtstep2ret.Rows[0][0].ToString();
                    if (spRet == "F")
                    {
                        spRet = "F" + dtstep2ret.Rows[0][1].ToString();
                    }
                    if (string.IsNullOrEmpty(spRet) || (spRet[0] == 'F'))
                    {
                        _sqlhelper.EndUseSingleConnection();
                        return spRet;
                    }
                }
                catch (Exception e)
                {
                    _sqlhelper.EndUseSingleConnection();
                    return (e.Message + errMsg);
                }
            }
            sqlcmd = "exec usp_Emr_SaveDoctorAdvice '" + wkdz + "', 3," + syxh + ", '00'," + cqlsbz;
            DataTable dtstep3ret = _sqlhelper.ExecuteDataTable(sqlcmd, CommandType.Text);
            if ((dtstep3ret == null) || (dtstep3ret.Rows.Count == 0))
            {
                return "F执行存储步骤3失败!";
            }
            if (dtstep3ret.Rows[0][0].ToString() == "F")
            {
                spRet = "F" + dtstep3ret.Rows[0][1].ToString();
            }
            else if (dtstep3ret.Rows[0][0].ToString() == "T")
            {
                spRet = "T";
            }
            else
            {
                spRet = "F未知错误";
            }
            _sqlhelper.EndUseSingleConnection();
            return (spRet + "\r\n" + errMsg);

        }

        ///// <summary>
        ///// 保存草药协定方到HIS系统中
        ///// </summary>
        ///// <returns></returns>
        //private string SaveCyAdvises()
        //{ 

        //}

        /// <summary>
        /// 发送医嘱到HIS
        /// </summary>
        /// <param name="orderAdd">发送医嘱表</param>
        private DataTable SaveOrder(DataTable orderAdd)
        {


            ////根据HIS返回结果集判断是否插入成功
            //if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() != "0")
            //{
            //    //成套医嘱其中一条发送不成功则所有医嘱都不发送
            //    foreach (DataRow row in orderAdd.Select("FS_Flag='1'"))
            //    {
            //        if (row["Ctyzxh"].ToString() == dr["Ctyzxh"].ToString())
            //        {
            //            row["FS_Flag"] = "0";
            //            row["FS_Mess"] = dt.Rows[0][0].ToString();
            //        }
            //    }
            //    //dr["FS_Flag"] = "0";
            //    //dr["FS_Mess"] = dt.Rows[0][0].ToString();
            //}
            //else
            //{

            //}


            string cqlsbz;
            string errMsg = string.Empty;
            string spRet = string.Empty;
            string sqlcmd = string.Empty;
            bool iscqls;
            string lyjzsj = string.Empty;

            string wkdz = DateTime.Now.ToString("yyyyMMddHHmmss") + orderAdd.Rows[0]["lrysdm"].ToString();
            #region 执行存储过程第一步
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                    {

                    new SqlParameter("@wkdz",wkdz),
                    new SqlParameter("@jszt",1),
                    new SqlParameter("@syxh",orderAdd.Rows[0]["Syxh"].ToString()),
                    new SqlParameter("@czyh",orderAdd.Rows[0]["Czyh"].ToString()),
                    new SqlParameter("@fzbz",0),

                    new SqlParameter("@ysdm",orderAdd.Rows[0]["lrysdm"].ToString()),
                    new SqlParameter("@yzlb",0),
                    new SqlParameter("@ksrq",""),
                    new SqlParameter("@xmlb",0),
                    new SqlParameter("@idm ",0),

                    new SqlParameter("@ypdm",""),
                    new SqlParameter("@yfdm",""),
                    new SqlParameter("@ypjl",0),
                    new SqlParameter("@jldw",""),
                    new SqlParameter("@dwlb",0),

                    new SqlParameter("@ypyf",""),
                    new SqlParameter("@yznr",""),
                    new SqlParameter("@ztnr",""),
                    new SqlParameter("@pcdm",""),
                    new SqlParameter("@zdm ",""),

                    new SqlParameter("@zxsj",""),
                    
                      };

                _sqlhelper.BeginUseSingleConnection();
                _sqlhelper.BeginTransaction();
                DataTable dtret = _sqlhelper.ExecuteDataTable("usp_bq_yzlr", parameters, CommandType.StoredProcedure);
                if ((dtret == null) || (dtret.Rows.Count == 0))
                {
                    spRet = "F执行存储步骤1失败!";
                }
                spRet = dtret.Rows[0][0].ToString();
                if (spRet == "F")
                {
                    spRet = "F" + dtret.Rows[0][1].ToString();
                }
                if (string.IsNullOrEmpty(spRet) || (spRet[0] == 'F'))
                {
                    _sqlhelper.EndUseSingleConnection();
                    foreach (DataRow row in orderAdd.Select("FS_Flag='1'"))
                    {
                        row["FS_Flag"] = "0";
                        row["FS_Mess"] = spRet;
                    }
                    return orderAdd;
                }
                errMsg = errMsg + "\r\n" + sqlcmd + " exec step1 ";
            }
            catch (Exception e)
            {
                _sqlhelper.RollbackTransaction();
                _sqlhelper.EndUseSingleConnection();
                foreach (DataRow row in orderAdd.Select("FS_Flag='1'"))
                {
                    row["FS_Flag"] = "0";
                    row["FS_Mess"] = e.Message + errMsg;
                }
                return (orderAdd);
            }
            #endregion

            #region 执行存储过程第二步
            foreach (DataRow dr in orderAdd.Rows)
            {
                try
                {
                    //"长期医嘱"  2703
                    //"临时医嘱"  2702
                    if (dr["yzbz"].ToString() == "2703")
                    {
                        iscqls = true;
                        cqlsbz = "0";
                    }
                    else
                    {
                        iscqls = false;
                        cqlsbz = "1";
                    }

                    string yzno = dr["fzxh"].ToString();
                    string docid = dr["lrysdm"].ToString();
                    string ksrq = this.DT19ToDT16(dr["ksrq"].ToString());
                    string yzlb = dr["yzlb"].ToString();



                    string xmlb = dr["xmlb"].ToString();
                    string idm = dr["cdxh"].ToString();
                    string xmdm = dr["ypdm"].ToString();
                    string zxks = dr["zxks"].ToString();
                    string ypjl = dr["ypjl"].ToString();
                    string jldw = dr["jldw"].ToString();
                    string dwbz = dr["dwxs"].ToString();
                    string dwlb = dr["dwlb"].ToString();
                    string yfdm = dr["yfdm"].ToString();
                    string yznr = this.DealQuoteString(dr["yznr"].ToString());
                    string ztnr = this.DealQuoteString(dr["ztnr"].ToString());
                    string pcdm = dr["pcdm"].ToString();
                    string zdm = string.Empty;
                    if (!iscqls)
                    {
                        zdm = dr["zdm"].ToString();
                    }
                    string zxsj = dr["zxsj"].ToString();
                    string zbbz = dr["tsbj"].ToString();
                    if (string.IsNullOrEmpty(zbbz) || (zbbz.Trim() != "1"))
                    {
                        zbbz = "0";
                    }
                    string tzxh = "0";
                    if (iscqls)
                    {
                        tzxh = dr["tzxh"].ToString();
                    }
                    if (string.IsNullOrEmpty(tzxh))
                    {
                        tzxh = "0";
                    }
                    string tzrq = string.Empty;
                    if (!iscqls)
                    {
                        tzrq = this.DT19ToDT16(dr["tzrq"].ToString());
                    }
                    string mzdm = "";
                    string zdys = "";
                    string lcxmdm = string.Empty;
                    if (int.Parse(dr["yzlb"].ToString()) == 0xc1f)
                    {
                        lcxmdm = dr["ypdm"].ToString();
                    }
                    string ypmc = this.DealQuoteString(dr["ypmc"].ToString());
                    string sybpno = "0";
                    string sqdxh = "0";
                    if (iscqls)
                    {
                        sqdxh = dr["sqdxh"].ToString();
                    }
                    if (string.IsNullOrEmpty(sqdxh))
                    {
                        sqdxh = "0";
                    }
                    string ybspbz = dr["ybsptg"].ToString();
                    if (string.IsNullOrEmpty(ybspbz))
                    {
                        ybspbz = "0";
                    }
                    string ybspbh = dr["ybspbh"].ToString();
                    string yzxh = "-1";
                    if (iscqls)
                    {
                        yzxh = dr["lsyzxh"].ToString();
                    }
                    else
                    {
                        yzxh = dr["cqyzxh"].ToString();
                    }
                    string zyzxh = dr["fzxh"].ToString();
                    string shczy = dr["shczy"].ToString();
                    string shrq = this.DT19ToDT16(dr["shrq"].ToString());
                    string qxysdm = dr["qxysdm"].ToString();
                    string qxrq = this.DT19ToDT16(dr["qxrq"].ToString());
                    string blyzzt = dr["yzzt"].ToString();
                    string ypzsl = "0";
                    string mq = "0";
                    if (iscqls)
                    {
                        mq = "0";
                    }
                    else
                    {
                        mq = ((dr["mq"] == null) || (dr["mq"] == DBNull.Value)) ? "0" : dr["mq"].ToString();
                    }
                    if (string.IsNullOrEmpty(mq))
                    {
                        mq = "0";
                    }
                    if (iscqls)
                    {
                        ypzsl = dr["ypzsl"].ToString();
                    }
                    if (string.IsNullOrEmpty(lyjzsj))
                    {
                        lyjzsj = "08";
                    }
                    string memo = dr["memo"].ToString();
                    string bbzlId = "";
                    string bbzl = "";
                    string sqdxmbz = "";
                    string sqdjjbz = "0";
                    if (sqdxh != "0")
                    {
                        string[] lstmemo = memo.Split(new char[] { '`' }, StringSplitOptions.None);
                        if (lstmemo.Length > 0)
                        {
                            bbzlId = lstmemo[0];
                        }
                        if (lstmemo.Length > 1)
                        {
                            bbzl = lstmemo[1];
                        }
                        if (lstmemo.Length > 2)
                        {
                            sqdxmbz = lstmemo[2];
                        }
                        if (lstmemo.Length > 3)
                        {
                            sqdjjbz = lstmemo[3];
                        }
                        if (string.IsNullOrEmpty(sqdjjbz))
                        {
                            sqdjjbz = "0";
                        }
                        memo = "";
                    }
                    SqlParameter[] parameters = new SqlParameter[] 
                        {
    //@wkdz varchar(32),	网卡地址
    //@jszt smallint,		结束状态1=创建表，2=插入，3=递交
    //@syxh ut_syxh,		首页序号
    //@czyh ut_czyh,		操作员号
    //@fzbz int,			分组标志   每组医嘱标志不同

    //@ysdm ut_czyh,		医生代码
    //@yzlb smallint,		医嘱类别 0:临时医嘱，1：长期医嘱，2：临时医嘱用法材料，3：长期医嘱用法材料
    //@ksrq ut_rq16,		医嘱的开始时间
    //@xmlb smallint,		项目类别 0:药品医嘱，1：非药品医嘱，2:手术医嘱，8:输液医嘱，9:停止医嘱
    //@idm  ut_xh9,		产地idm，非药品为0

    //@ypdm ut_xmdm,		药品代码或治疗项目代码
    //@yfdm ut_ksdm,		药房代码
    //@ypjl ut_sl14_3,	药品剂量或治疗项目的数量
    //@jldw ut_unit,		剂量单位
    //@dwlb smallint,		单位类别0剂量单位，3住院单位

    //@ypyf ut_dm2, 		用法代码
    //@yznr ut_mc64,		医嘱内容
    //@ztnr varchar(100),		医生嘱托
    //@pcdm ut_dm2,		医嘱执行频次代码
    //@zdm  char(7),		周标志（周一到周七用1234567表示）

    //@zxsj ut_mc64,		医嘱执行时间
    //@zbybz smallint = 0,	0:普通，1:自备药品，2:PRN
    //@dybz smallint = 0, 	0:打印，1:不打印
    //@xmdj numeric(12,2) = null,	零单价项目的单价
    //@tzxh ut_xh12 = null,		停止医嘱序号

    //@jsrq ut_rq16 = null,		长期医嘱停止时间
    //@mzdm ut_xmdm = null,		麻醉代码
    //@yjqrbz smallint = 0		医技确认标志
    //@zdys ut_czyh = null		主刀医生 -- mitsukow ,, 2oo2-12-24
    //@yyzxh ut_xh12 = -1,

    //@yfzxh ut_xh12 = -1,		--mitsukow , 2oo2-12-24 , add ,增加了原医嘱的序号和分组序号
    //@hzxm ut_name = '',		--患者姓名, Add by Wang Yi, 2003.03.03
    //@yexh ut_syxh = 0		--婴儿序号
    //@mzxdfh ut_xh12 = 0		--麻醉协定方序号
    //@njjbz ut_bz = -1               --配伍禁忌标志 

    //,@ybshbz ut_bz =0               --医保审核志0不用审核1审核通过2审核不通过
    //,@ybspbh ut_mc32 =null          --医保审批编号
    //,@sqzd ut_zddm=''               --术前诊断
    //,@sstyz	ut_bz=0			--手术停医嘱
    //,@lzbz ut_bz=0			--临嘱标志

    //,@sxysbz ut_bz=0		--实习医生标志0:普通1:表示是实习医生，开的医嘱需要审核2:审核实习医生开的医嘱
    //,@lcxmdm ut_xmdm='0'            	--临床项目代码 
    //,@ssmc ut_mc32 = ''		--手术名称
    //,@systype ut_bz=0		--系统标志  0住院医生  1护士站wuming add 加以区分当前是医生调用还是护士调用
    //,@ldpcbz	--联动频次标志 0 主医嘱的 1项目联动自带频次

    //,@yzlbdy 	--医嘱类别对应
    //,@blzbz ut_bz=0		补临嘱标志，如果为1代表是前台选择补临嘱的临时医嘱
    //    ,@sqddm int  申请单表单代码
    //,@kjslx ut_bz=0      --抗菌素类型（0普通1预防性2治疗性）
    //,@kjsbz ut_bz=0      --抗菌素明细标志（0非抗菌素1抗菌素2默认溶媒3可选溶媒）

    //,@kjsssyz ut_xh12=0  --抗菌素对应手术医嘱序号
    //@zddm ut_zddm='',    诊断代码
    //@zjlx ut_bz=0,       证件类型
    //@zjhm varchar(20)='' 证件号码
    //@Jsdmbz --精神毒麻标志

    //@ssxmdm varchar(1000)=''  膳食项目代码 shiyong 20100519
    //@sxbz ut_bz 手写标志 0 ：打印  1：手写
    //@ssid ut_xmdm 手术代码，围手术 
    //@ssyzysdm ut_czyh  手术一助医生代码
    //@ssezysdm ut_czyh  手术二助医生代码

    //@ssszysdm ut_czyh  手术三助医生代码
    //@ssaprq ut_rq16    手术安排日期  
    //,@haabz ut_bz 肝炎相关
    //,@jzssbz ut_bz 手术类别


        //@wkdz varchar(32),	网卡地址
    //@jszt smallint,		结束状态1=创建表，2=插入，3=递交
    //@syxh ut_syxh,		首页序号
    //@czyh ut_czyh,		操作员号
    //@fzbz int,			分组标志   每组医嘱标志不同

    //@ysdm ut_czyh,		医生代码
    //@yzlb smallint,		医嘱类别 0:临时医嘱，1：长期医嘱，2：临时医嘱用法材料，3：长期医嘱用法材料
    //@ksrq ut_rq16,		医嘱的开始时间
    //@xmlb smallint,		项目类别 0:药品医嘱，1：非药品医嘱，2:手术医嘱，8:输液医嘱，9:停止医嘱
    //@idm  ut_xh9,		产地idm，非药品为0
                            new SqlParameter("@wkdz",wkdz),
                            new SqlParameter("@jszt",2),
                            new SqlParameter("@syxh",dr["Syxh"].ToString()),
                            new SqlParameter("@czyh",docid),//操作员工号
                            new SqlParameter("@fzbz",yzno),

                            new SqlParameter("@ysdm",docid),
                            new SqlParameter("@yzlb",yzlb),
                            new SqlParameter("@ksrq",ksrq),
                            new SqlParameter("@xmlb",xmlb),
                            new SqlParameter("@idm ",idm),

                            //idm + ",'" + xmdm + "','" + zxks + "'," + ypjl + ",'" + jldw + "'," + dwbz + "," + dwlb + ",'" + yfdm + "','" + yznr + "','" + ztnr + "','" + pcdm + "','
                            //" + zdm + "','" + zxsj + "'," + zbbz + "0,,0," + tzxh + ",'" + tzrq + "','" + mzdm + "','" + zdys + "'," + ybspbz + ",'" + ybspbh + "','" + lcxmdm + "','" + ypmc + "'," + sybpno + "," + sqdxh + "," + yzxh + "," + zyzxh + ",'" + shczy + "','" + shrq + "','" + qxysdm + "','" + qxrq + "'," + blyzzt + "," + ypzsl + "," + mq + ",'" + lyjzsj + "','" + memo + "','" + sqdxmbz + "','" + bbzlId + "','" + bbzl + "'," + sqdjjbz;
  //@ypdm ut_xmdm,		药品代码或治疗项目代码
    //@yfdm ut_ksdm,		药房代码
    //@ypjl ut_sl14_3,	药品剂量或治疗项目的数量
    //@jldw ut_unit,		剂量单位
    //@dwlb smallint,		单位类别0剂量单位，3住院单位

    //@ypyf ut_dm2, 		用法代码
    //@yznr ut_mc64,		医嘱内容
    //@ztnr varchar(100),		医生嘱托
    //@pcdm ut_dm2,		医嘱执行频次代码
    //@zdm  char(7),		周标志（周一到周七用1234567表示）
                            new SqlParameter("@ypdm",xmdm),
                            new SqlParameter("@yfdm",yfdm),//药房代码
                            new SqlParameter("@ypjl",ypjl),
                            new SqlParameter("@jldw",jldw),
                            new SqlParameter("@dwlb",dwlb),

                            new SqlParameter("@ypyf",yfdm),
                            new SqlParameter("@yznr",yznr),
                            new SqlParameter("@ztnr",ztnr),
                            new SqlParameter("@pcdm",pcdm),
                            new SqlParameter("@zdm ",zdm),

                            //@zxsj ut_mc64,		医嘱执行时间
    //@zbybz smallint = 0,	0:普通，1:自备药品，2:PRN
    //@dybz smallint = 0, 	0:打印，1:不打印
    //@xmdj numeric(12,2) = null,	零单价项目的单价
    //@tzxh ut_xh12 = null,		停止医嘱序号

    //@jsrq ut_rq16 = null,		长期医嘱停止时间
    //@mzdm ut_xmdm = null,		麻醉代码
    //@yjqrbz smallint = 0		医技确认标志
    //@zdys ut_czyh = null		主刀医生 -- mitsukow ,, 2oo2-12-24
    //@yyzxh ut_xh12 = -1,

    //'" + zxsj + "'," + zbbz + "0,,0," + tzxh + ",'" + tzrq + "','" + mzdm + "','" + zdys + "'," + ybspbz + ",'" + ybspbh + "','" + lcxmdm + "','" + ypmc + "'," + sybpno + "," + sqdxh + "," + yzxh + "," + zyzxh + ",'" + shczy + "','" + shrq + "','" + qxysdm + "','" + qxrq + "'," + blyzzt + "," + ypzsl + "," + mq + ",'" + lyjzsj + "','" + memo + "','" + sqdxmbz + "','" + bbzlId + "','" + bbzl + "'," + sqdjjbz;
                             new SqlParameter("@zxsj",zxsj),
                            new SqlParameter("@zbybz",0),
                            new SqlParameter("@dybz",0),
                            new SqlParameter("@xmdj",0),
                            new SqlParameter("@tzxh ",tzxh),

                            new SqlParameter("@jsrq",tzrq),
                            new SqlParameter("@mzdm",mzdm),
                            new SqlParameter("@yjqrbz",0),
                            new SqlParameter("@zdys",zdys),
                            new SqlParameter("@yyzxh",0),

                             //@yfzxh ut_xh12 = -1,		--mitsukow , 2oo2-12-24 , add ,增加了原医嘱的序号和分组序号
    //@hzxm ut_name = '',		--患者姓名, Add by Wang Yi, 2003.03.03
    //@yexh ut_syxh = 0		--婴儿序号
    //@mzxdfh ut_xh12 = 0		--麻醉协定方序号
    //@njjbz ut_bz = -1               --配伍禁忌标志 

    //,@ybshbz ut_bz =0               --医保审核志0不用审核1审核通过2审核不通过
    //,@ybspbh ut_mc32 =null          --医保审批编号
    //,@sqzd ut_zddm=''               --术前诊断
    //,@sstyz	ut_bz=0			--手术停医嘱
    //,@lzbz ut_bz=0			--临嘱标志
                            new SqlParameter("@yfzxh",-1),
                            new SqlParameter("@hzxm",dr["hzxm"].ToString()),
                            new SqlParameter("@yexh",0),
                            new SqlParameter("@mzxdfh",0),
                            new SqlParameter("@njjbz ",-1),

                             new SqlParameter("@ybshbz",ybspbz),
                            new SqlParameter("@ybspbh",ybspbh),
                            new SqlParameter("@sqzd",""),
                            new SqlParameter("@sstyz",0),
                            new SqlParameter("@lzbz ",0),

                            //,@sxysbz ut_bz=0		--实习医生标志0:普通1:表示是实习医生，开的医嘱需要审核2:审核实习医生开的医嘱
    //,@lcxmdm ut_xmdm='0'            	--临床项目代码 
    //,@ssmc ut_mc32 = ''		--手术名称
    //,@systype ut_bz=0		--系统标志  0住院医生  1护士站wuming add 加以区分当前是医生调用还是护士调用
    //,@ldpcbz	--联动频次标志 0 主医嘱的 1项目联动自带频次

    //,@yzlbdy 	--医嘱类别对应
    //,@blzbz ut_bz=0		补临嘱标志，如果为1代表是前台选择补临嘱的临时医嘱
    //    ,@sqddm int  申请单表单代码
    //,@kjslx ut_bz=0      --抗菌素类型（0普通1预防性2治疗性）
    //,@kjsbz ut_bz=0      --抗菌素明细标志（0非抗菌素1抗菌素2默认溶媒3可选溶媒）

    //'" + lcxmdm + "','" + ypmc + "'," + sybpno + "," + sqdxh + "," + yzxh + "," + zyzxh + ",'" + shczy + "','" + shrq + "','" + qxysdm + "','" + qxrq + "'," + blyzzt + "," + ypzsl + "," + mq + ",'" + lyjzsj + "','" + memo + "','" + sqdxmbz + "','" + bbzlId + "','" + bbzl + "'," + sqdjjbz;
                            new SqlParameter("@sxysbz",0),
                            new SqlParameter("@lcxmdm",lcxmdm),
                            new SqlParameter("@ssmc",""),
                            new SqlParameter("@systype",0),
                            new SqlParameter("@ldpcbz",0),

                            new SqlParameter("@yzlbdy",-1),//需确认
                            new SqlParameter("@blzbz",0),
                            new SqlParameter("@sqddm",0),//系统中无申请单代码
                            new SqlParameter("@kjslx",0),
                            new SqlParameter("@kjsbz ",0), 

    //,@kjsssyz ut_xh12=0  --抗菌素对应手术医嘱序号
    //@zddm ut_zddm='',    诊断代码
    //@zjlx ut_bz=0,       证件类型
    //@zjhm varchar(20)='' 证件号码
    //@Jsdmbz --精神毒麻标志

    //@ssxmdm varchar(1000)=''  膳食项目代码 shiyong 20100519
    //@sxbz ut_bz 手写标志 0 ：打印  1：手写
    //@ssid ut_xmdm 手术代码，围手术 
    //@ssyzysdm ut_czyh  手术一助医生代码
    //@ssezysdm ut_czyh  手术二助医生代码

    //@ssszysdm ut_czyh  手术三助医生代码
    //@ssaprq ut_rq16    手术安排日期  
    //,@haabz ut_bz 肝炎相关
    //,@jzssbz ut_bz 手术类别
                             new SqlParameter("@kjsssyz",0),
                            new SqlParameter("@zddm",0),
                            new SqlParameter("@zjlx",0),
                            new SqlParameter("@zjhm",""),
                            new SqlParameter("@Jsdmbz ",0),

                            new SqlParameter("@ssxmdm",""),
                            new SqlParameter("@sxbz",0),
                            new SqlParameter("@ssid",""),
                            new SqlParameter("@ssyzysdm",""),
                            new SqlParameter("@ssezysdm",""),

                            new SqlParameter("@ssszysdm",""),
                            new SqlParameter("@ssaprq",""),
                            new SqlParameter("@haabz",""),
                            new SqlParameter("@jzssbz","") 
                    
                        };



                    DataTable dtstep2ret = _sqlhelper.ExecuteDataTable("usp_bq_yzlr", parameters, CommandType.StoredProcedure);
                    if ((dtstep2ret == null) || (dtstep2ret.Rows.Count == 0))
                    {
                        spRet = "F执行存储步骤2失败!";
                    }
                    spRet = dtstep2ret.Rows[0][0].ToString();
                    if (spRet == "F")
                    {
                        spRet = "F" + dtstep2ret.Rows[0][1].ToString();
                    }
                    if (string.IsNullOrEmpty(spRet) || (spRet[0] == 'F'))
                    {
                        _sqlhelper.EndUseSingleConnection();
                        _sqlhelper.RollbackTransaction();
                        foreach (DataRow row in orderAdd.Select("FS_Flag='1'"))
                        {
                            row["FS_Flag"] = "0";
                            row["FS_Mess"] = spRet;
                        }
                        return (orderAdd);
                    }
                }
                catch (Exception e)
                {

                    _sqlhelper.RollbackTransaction();
                    _sqlhelper.EndUseSingleConnection();
                    foreach (DataRow row in orderAdd.Select("FS_Flag='1'"))
                    {
                        row["FS_Flag"] = "0";
                        row["FS_Mess"] = e.Message + errMsg;
                    }
                    return orderAdd;
                }
            }
            #endregion

            #region 执行存储步骤3
            //sqlcmd = "exec usp_bq_yzlr '" + wkdz + "', 3," + syxh + ",'00'," + "0,   '',0,'',0,0,    '','',0,'',0,'','','','','',''"; ;


            SqlParameter[] parameters3 = new SqlParameter[] 
                    {

                    new SqlParameter("@wkdz",wkdz),
                    new SqlParameter("@jszt",3),
                    new SqlParameter("@syxh",orderAdd.Rows[0]["Syxh"].ToString()),
                    new SqlParameter("@czyh",""),
                    new SqlParameter("@fzbz",0),

                    new SqlParameter("@ysdm",""),
                    new SqlParameter("@yzlb",0),
                    new SqlParameter("@ksrq",""),
                    new SqlParameter("@xmlb",0),
                    new SqlParameter("@idm ",0),

                    new SqlParameter("@ypdm",""),
                    new SqlParameter("@yfdm",""),
                    new SqlParameter("@ypjl",0),
                    new SqlParameter("@jldw",""),
                    new SqlParameter("@dwlb",0),

                    new SqlParameter("@ypyf",""),
                    new SqlParameter("@yznr",""),
                    new SqlParameter("@ztnr",""),
                    new SqlParameter("@pcdm",""),
                    new SqlParameter("@zdm ",""),

                    new SqlParameter("@zxsj",""),
                    
                      };
            DataTable dtstep3ret = _sqlhelper.ExecuteDataTable("usp_bq_yzlr", parameters3, CommandType.StoredProcedure);
            //DataTable dtstep3ret = _sqlhelper.ExecuteDataTable(sqlcmd, CommandType.Text);
            if ((dtstep3ret == null) || (dtstep3ret.Rows.Count == 0))
            {
                spRet = "F执行存储步骤3失败!";
            }
            if (dtstep3ret.Rows[0][0].ToString() == "F")
            {
                spRet = "F" + dtstep3ret.Rows[0][1].ToString();
            }
            else if (dtstep3ret.Rows[0][0].ToString() == "T")
            {
                spRet = "T";
            }
            else
            {
                spRet = "F未知错误";
            }
            _sqlhelper.EndUseSingleConnection();
            //return (spRet + "\r\n" + errMsg);

            _sqlhelper.EndUseSingleConnection();
            foreach (DataRow row in orderAdd.Select("FS_Flag='1'"))
            {
                row["FS_Flag"] = "0";
                row["FS_Mess"] = spRet + "\r\n" + errMsg;
            }
            return (orderAdd);

            #endregion


            //return orderAdd;
        }

        #endregion


        //bool iscqls = true;  //临时医嘱  false  长期医嘱  true
        public DataTable SendOrder(string syxh, DataTable changedTable, string executorCode, string macAddress, bool iscqls)
        {

            // 数据集转换成byte数组
            //MemoryStream source = new MemoryStream();
            //changedTable.WriteXml(source, XmlWriteMode.WriteSchema);

            //source.Seek(0, SeekOrigin.Begin);
            //byte[] data = new byte[source.Length];
            //source.Read(data, 0, (int)source.Length);

            // 3 调用接口检查数据 
            //bool iscqls = true;  //临时医嘱  false  长期医嘱
            string mess = CheckAdvises(macAddress, syxh, changedTable, iscqls, "utf-8");

            // 4 调用接口同步数据
            if (mess.IndexOf("T") > -1 && mess.IndexOf("T") < 5)
                //20130507 这个是调用JSD His保存医嘱
                mess = SaveAdvises(macAddress, syxh, changedTable, iscqls, "utf-8", "");
            //20130507 测试报错,错误提示","附近错误
            //mess = SaveAdvisesThis4(macAddress, syxh, data, iscqls, "utf-8", "");
            //return mess;
            //根据HIS返回结果集判断是否插入成功
            if (mess.IndexOf("T") > -1 && mess.IndexOf("T") < 5)
            {
                //成套医嘱其中一条发送不成功则所有医嘱都不发送
                foreach (DataRow row in changedTable.Select("FS_Flag='1'"))
                {
                    row["FS_Flag"] = "1";
                    row["FS_Mess"] = mess;

                }

            }
            else
            {
                //成套医嘱其中一条发送不成功则所有医嘱都不发送
                foreach (DataRow row in changedTable.Select("FS_Flag='1'"))
                {
                    row["FS_Flag"] = "0";
                    row["FS_Mess"] = mess;

                }
            }
            return changedTable;
        }


        /// <summary>
        /// 根据传入的医嘱表将医嘱信息保存到HIS系统中
        /// </summary>
        /// <param name="changedTable"></param>
        /// <returns></returns>
        public DataTable SendOrder(DataTable changedTable)
        {

            return SaveOrder(changedTable);

        }


        #region 更新后方法  直接调用HIS原有的录入存储过程




        #endregion
    }
}
