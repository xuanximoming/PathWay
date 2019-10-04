//using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Web;
//using System.ServiceModel;
//using Yidansoft.Service;
//using YidanSoft.Core;
//using System.Text;
//using System.Data;
//using System.IO;
//using System.ServiceModel.Activation;

//namespace Yidansoft.Service
//{
//    public partial class YidanEHRDataService
//    {

//        /// <summary>
//        /// 处理带引号的字符串
//        /// </summary>
//        /// <param name="source"></param>
//        /// <returns></returns>
//        string DealQuoteString(string source)
//        {
//            if (string.IsNullOrEmpty(source))
//                return string.Empty;
//            else
//                return source.Replace("'", "''");
//        }
//        /// <summary>
//        /// 19位日期字符串 -> 16位日期字符串
//        /// </summary>
//        /// <param name="dt19"></param>
//        /// <returns></returns>
//        string DT19ToDT16(string dt19)
//        {
//            if (string.IsNullOrEmpty(dt19)) return string.Empty;
//            return dt19.Replace('-', ' ').Replace(" ", "");
//        }

//        /// <summary>
//        /// Xml形式的字节数组到数据集
//        /// </summary>
//        /// <param name="bytearray"></param>
//        /// <param name="encoding"></param>
//        /// <returns></returns>
//        DataSet ByteArray2DataSet(byte[] bytearray, Encoding encoding)
//        {
//            try
//            {
//                MemoryStream ms = new MemoryStream();
//                ms.Write(bytearray, 0, bytearray.Length);
//                ms.Seek(0, SeekOrigin.Begin);
//                StreamReader sr = new StreamReader(ms, encoding);
//                DataSet dataset = new DataSet();
//                dataset.ReadXml(sr, XmlReadMode.ReadSchema);
//                return dataset;
//            }
//            catch
//            {
//                return null;
//            }
//        }

//        #region 检查医嘱保存


//        /// <summary>
//        /// 检查医嘱保存
//        /// </summary>
//        /// <param name="wkdz"></param>
//        /// <param name="syxh"></param>
//        /// <param name="xmlcqdata"></param>
//        /// <param name="iscqls"></param>
//        /// <param name="encodingName"></param>
//        /// <returns></returns>
//        [OperationContract]
//        [FaultContract(typeof(LoginException))]
//        public string CheckAdvises(string wkdz, string syxh, byte[] xmlcqdata, bool iscqls, string encodingName)
//        {

//            string cqlsbz;
//            string errMsg = string.Empty;
//            string spRet = string.Empty;
//            string sqlcmd = string.Empty;
//            IDataAccess _sqlhelper = new SqlDataAccess("HISDB");// DataAccessFactory.GetSqlDataAccess("HISDB");
//            Encoding encoding = Encoding.GetEncoding(encodingName);
//            DataSet dsAdvices = this.ByteArray2DataSet(xmlcqdata, encoding);
//            if ((dsAdvices == null) || (dsAdvices.Tables.Count == 0))
//            {
//                return "F传入的医嘱数据无法解析";
//            }
//            try
//            {
//                _sqlhelper.BeginUseSingleConnection();
//                sqlcmd = "exec usp_Emr_CheckDoctorAdvice '" + wkdz + "',1," + syxh;
//                DataTable dtret = _sqlhelper.ExecuteDataTable(sqlcmd, CommandType.Text);
//                if ((dtret == null) || (dtret.Rows.Count == 0))
//                {
//                    spRet = "F执行存储步骤1失败!";
//                }
//                spRet = dtret.Rows[0][0].ToString();
//                if (spRet == "F")
//                {
//                    spRet = "F" + dtret.Rows[0][1].ToString();
//                }
//                if (string.IsNullOrEmpty(spRet) || (spRet[0] == 'F'))
//                {
//                    _sqlhelper.EndUseSingleConnection();
//                    return spRet;
//                }
//                errMsg = errMsg + "\r\n" + sqlcmd + " exec step1 ";
//            }
//            catch (Exception e)
//            {
//                _sqlhelper.EndUseSingleConnection();
//                return (e.Message + errMsg);
//            }
//            if (iscqls)
//            {
//                cqlsbz = "0";
//            }
//            else
//            {
//                cqlsbz = "1";
//            }
//            DataTable dtAdvices = dsAdvices.Tables[0];
//            for (int i = 0; i < dtAdvices.Rows.Count; i++)
//            {
//                try
//                {
//                    DataRow dr = dtAdvices.Rows[i];
//                    string yzxh = "-1";
//                    if (iscqls)
//                    {
//                        yzxh = dr["lsyzxh"].ToString();
//                    }
//                    else
//                    {
//                        yzxh = dr["cqyzxh"].ToString();
//                    }
//                    string idm = dr["cdxh"].ToString();
//                    string xmdm = dr["ypdm"].ToString();
//                    string blyzzt = dr["yzzt"].ToString();
//                    string sql = "exec usp_Emr_CheckDoctorAdvice '" + wkdz + "', 2," + syxh + "," + cqlsbz + ", " + yzxh + "," + idm + ",'" + xmdm + "'," + blyzzt;
//                    errMsg = errMsg + "\r\n" + sql;
//                    DataTable dtstep2ret = _sqlhelper.ExecuteDataTable(sql, CommandType.Text);
//                    errMsg = errMsg + "\r\n exec step2 " + i.ToString();
//                    if ((dtstep2ret == null) || (dtstep2ret.Rows.Count == 0))
//                    {
//                        spRet = "F执行存储步骤2失败!";
//                    }
//                    spRet = dtstep2ret.Rows[0][0].ToString();
//                    if (spRet == "F")
//                    {
//                        spRet = "F" + dtstep2ret.Rows[0][1].ToString();
//                    }
//                    if (string.IsNullOrEmpty(spRet) || (spRet[0] == 'F'))
//                    {
//                        _sqlhelper.EndUseSingleConnection();
//                        return spRet;
//                    }
//                }
//                catch (Exception e)
//                {
//                    _sqlhelper.EndUseSingleConnection();
//                    return (e.Message + errMsg);
//                }
//            }
//            sqlcmd = "exec usp_Emr_CheckDoctorAdvice '" + wkdz + "', 3," + syxh;
//            DataTable dtstep3ret = _sqlhelper.ExecuteDataTable(sqlcmd, CommandType.Text);
//            if ((dtstep3ret == null) || (dtstep3ret.Rows.Count == 0))
//            {
//                return "F执行存储步骤3失败!";
//            }
//            if (dtstep3ret.Rows[0][0].ToString() == "F")
//            {
//                spRet = "F" + dtstep3ret.Rows[0][1].ToString();
//            }
//            else if (dtstep3ret.Rows[0][0].ToString() == "-1")
//            {
//                spRet = "F" + dtstep3ret.Rows[0][1].ToString() + dtstep3ret.Rows[0][2].ToString();
//            }
//            else if (dtstep3ret.Rows[0][0].ToString() == "T")
//            {
//                spRet = "T";
//            }
//            else
//            {
//                spRet = "F未知错误";
//            }
//            _sqlhelper.EndUseSingleConnection();
//            return (spRet + "\r\n");

//        }
//        #endregion

//        #region 医嘱保存

//        /// <summary>
//        /// 医嘱保存到His
//        /// </summary>
//        /// <param name="wkdz"></param>
//        /// <param name="syxh"></param>
//        /// <param name="xmlcqdata"></param>
//        /// <param name="iscqls"></param>
//        /// <param name="encodingName"></param>
//        /// <param name="lyjzsj">领药截止时间</param>
//        /// <returns></returns>
//        [OperationContract]
//        [FaultContract(typeof(LoginException))]
//        public string SaveAdvises(string wkdz, string syxh, byte[] xmlcqdata, bool iscqls, string encodingName, string lyjzsj)
//        {

//            string cqlsbz;
//            string errMsg = string.Empty;
//            string spRet = string.Empty;
//            string sqlcmd = string.Empty;
//            Encoding encoding = Encoding.GetEncoding(encodingName);

//            IDataAccess _sqlhelper = new SqlDataAccess("HISDB");

//            DataSet dsAdvices = this.ByteArray2DataSet(xmlcqdata, encoding);
//            if ((dsAdvices == null) || (dsAdvices.Tables.Count == 0))
//            {
//                return "F传入的医嘱数据无法解析";
//            }
//            try
//            {
//                _sqlhelper.BeginUseSingleConnection();
//                sqlcmd = "exec usp_Emr_SaveDoctorAdvice '" + wkdz + "',1," + syxh;
//                DataTable dtret = _sqlhelper.ExecuteDataTable(sqlcmd, CommandType.Text);
//                if ((dtret == null) || (dtret.Rows.Count == 0))
//                {
//                    spRet = "F执行存储步骤1失败!";
//                }
//                spRet = dtret.Rows[0][0].ToString();
//                if (spRet == "F")
//                {
//                    spRet = "F" + dtret.Rows[0][1].ToString();
//                }
//                if (string.IsNullOrEmpty(spRet) || (spRet[0] == 'F'))
//                {
//                    _sqlhelper.EndUseSingleConnection();
//                    return spRet;
//                }
//                errMsg = errMsg + "\r\n" + sqlcmd + " exec step1 ";
//            }
//            catch (Exception e)
//            {
//                _sqlhelper.EndUseSingleConnection();
//                return (e.Message + errMsg);
//            }
//            if (iscqls)
//            {
//                cqlsbz = "0";
//            }
//            else
//            {
//                cqlsbz = "1";
//            }
//            DataTable dtAdvices = dsAdvices.Tables[0];
//            for (int i = 0; i < dtAdvices.Rows.Count; i++)
//            {
//                try
//                {
//                    DataRow dr = dtAdvices.Rows[i];
//                    string yzno = dr["fzxh"].ToString();
//                    string docid = dr["lrysdm"].ToString();
//                    string ksrq = this.DT19ToDT16(dr["ksrq"].ToString());
//                    string yzlb = dr["yzlb"].ToString();
//                    string xmlb = dr["xmlb"].ToString();
//                    string idm = dr["cdxh"].ToString();
//                    string xmdm = dr["ypdm"].ToString();
//                    string zxks = dr["zxks"].ToString();
//                    string ypjl = dr["ypjl"].ToString();
//                    string jldw = dr["jldw"].ToString();
//                    string dwbz = dr["dwxs"].ToString();
//                    string dwlb = dr["dwlb"].ToString();
//                    string yfdm = dr["yfdm"].ToString();
//                    string yznr = this.DealQuoteString(dr["yznr"].ToString());
//                    string ztnr = this.DealQuoteString(dr["ztnr"].ToString());
//                    string pcdm = dr["pcdm"].ToString();
//                    string zdm = string.Empty;
//                    if (!iscqls)
//                    {
//                        zdm = dr["zdm"].ToString();
//                    }
//                    string zxsj = dr["zxsj"].ToString();
//                    string zbbz = dr["tsbj"].ToString();
//                    if (string.IsNullOrEmpty(zbbz) || (zbbz.Trim() != "1"))
//                    {
//                        zbbz = "0";
//                    }
//                    string tzxh = "0";
//                    if (iscqls)
//                    {
//                        tzxh = dr["tzxh"].ToString();
//                    }
//                    if (string.IsNullOrEmpty(tzxh))
//                    {
//                        tzxh = "0";
//                    }
//                    string tzrq = string.Empty;
//                    if (!iscqls)
//                    {
//                        tzrq = this.DT19ToDT16(dr["tzrq"].ToString());
//                    }
//                    string mzdm = "";
//                    string zdys = "";
//                    string lcxmdm = string.Empty;
//                    if (int.Parse(dr["yzlb"].ToString()) == 0xc1f)
//                    {
//                        lcxmdm = dr["ypdm"].ToString();
//                    }
//                    string ypmc = this.DealQuoteString(dr["ypmc"].ToString());
//                    string sybpno = "0";
//                    string sqdxh = "0";
//                    if (iscqls)
//                    {
//                        sqdxh = dr["sqdxh"].ToString();
//                    }
//                    if (string.IsNullOrEmpty(sqdxh))
//                    {
//                        sqdxh = "0";
//                    }
//                    string ybspbz = dr["ybsptg"].ToString();
//                    if (string.IsNullOrEmpty(ybspbz))
//                    {
//                        ybspbz = "0";
//                    }
//                    string ybspbh = dr["ybspbh"].ToString();
//                    string yzxh = "-1";
//                    if (iscqls)
//                    {
//                        yzxh = dr["lsyzxh"].ToString();
//                    }
//                    else
//                    {
//                        yzxh = dr["cqyzxh"].ToString();
//                    }
//                    string zyzxh = dr["fzxh"].ToString();
//                    string shczy = dr["shczy"].ToString();
//                    string shrq = this.DT19ToDT16(dr["shrq"].ToString());
//                    string qxysdm = dr["qxysdm"].ToString();
//                    string qxrq = this.DT19ToDT16(dr["qxrq"].ToString());
//                    string blyzzt = dr["yzzt"].ToString();
//                    string ypzsl = "0";
//                    string mq = "0";
//                    if (iscqls)
//                    {
//                        mq = "0";
//                    }
//                    else
//                    {
//                        mq = ((dr["mq"] == null) || (dr["mq"] == DBNull.Value)) ? "0" : dr["mq"].ToString();
//                    }
//                    if (string.IsNullOrEmpty(mq))
//                    {
//                        mq = "0";
//                    }
//                    if (iscqls)
//                    {
//                        ypzsl = dr["ypzsl"].ToString();
//                    }
//                    if (string.IsNullOrEmpty(lyjzsj))
//                    {
//                        lyjzsj = "08";
//                    }
//                    string memo = dr["memo"].ToString();
//                    string bbzlId = "";
//                    string bbzl = "";
//                    string sqdxmbz = "";
//                    string sqdjjbz = "0";
//                    if (sqdxh != "0")
//                    {
//                        string[] lstmemo = memo.Split(new char[] { '`' }, StringSplitOptions.None);
//                        if (lstmemo.Length > 0)
//                        {
//                            bbzlId = lstmemo[0];
//                        }
//                        if (lstmemo.Length > 1)
//                        {
//                            bbzl = lstmemo[1];
//                        }
//                        if (lstmemo.Length > 2)
//                        {
//                            sqdxmbz = lstmemo[2];
//                        }
//                        if (lstmemo.Length > 3)
//                        {
//                            sqdjjbz = lstmemo[3];
//                        }
//                        if (string.IsNullOrEmpty(sqdjjbz))
//                        {
//                            sqdjjbz = "0";
//                        }
//                        memo = "";
//                    }
//                    string sql = "exec usp_Emr_SaveDoctorAdvice '" + wkdz + "', 2," + syxh + ",'00'," + cqlsbz + ", " + yzno + ",'" + docid + "'," + yzlb + ",'" + ksrq + "'," + xmlb + "," + idm + ",'" + xmdm + "','" + zxks + "'," + ypjl + ",'" + jldw + "'," + dwbz + "," + dwlb + ",'" + yfdm + "','" + yznr + "','" + ztnr + "','" + pcdm + "','" + zdm + "','" + zxsj + "'," + zbbz + ",0,0," + tzxh + ",'" + tzrq + "','" + mzdm + "','" + zdys + "'," + ybspbz + ",'" + ybspbh + "','" + lcxmdm + "','" + ypmc + "'," + sybpno + "," + sqdxh + "," + yzxh + "," + zyzxh + ",'" + shczy + "','" + shrq + "','" + qxysdm + "','" + qxrq + "'," + blyzzt + "," + ypzsl + "," + mq + ",'" + lyjzsj + "','" + memo + "','" + sqdxmbz + "','" + bbzlId + "','" + bbzl + "'," + sqdjjbz;
//                    errMsg = errMsg + "\r\n" + sql;
//                    DataTable dtstep2ret = _sqlhelper.ExecuteDataTable(sql, CommandType.Text);
//                    errMsg = errMsg + "\r\n exec step2 " + i.ToString();
//                    if ((dtstep2ret == null) || (dtstep2ret.Rows.Count == 0))
//                    {
//                        spRet = "F执行存储步骤2失败!";
//                    }
//                    spRet = dtstep2ret.Rows[0][0].ToString();
//                    if (spRet == "F")
//                    {
//                        spRet = "F" + dtstep2ret.Rows[0][1].ToString();
//                    }
//                    if (string.IsNullOrEmpty(spRet) || (spRet[0] == 'F'))
//                    {
//                        _sqlhelper.EndUseSingleConnection();
//                        return spRet;
//                    }
//                }
//                catch (Exception e)
//                {
//                    _sqlhelper.EndUseSingleConnection();
//                    return (e.Message + errMsg);
//                }
//            }
//            sqlcmd = "exec usp_Emr_SaveDoctorAdvice '" + wkdz + "', 3," + syxh + ", '00'," + cqlsbz;
//            DataTable dtstep3ret = _sqlhelper.ExecuteDataTable(sqlcmd, CommandType.Text);
//            if ((dtstep3ret == null) || (dtstep3ret.Rows.Count == 0))
//            {
//                return "F执行存储步骤3失败!";
//            }
//            if (dtstep3ret.Rows[0][0].ToString() == "F")
//            {
//                spRet = "F" + dtstep3ret.Rows[0][1].ToString();
//            }
//            else if (dtstep3ret.Rows[0][0].ToString() == "T")
//            {
//                spRet = "T";
//            }
//            else
//            {
//                spRet = "F未知错误";
//            }
//            _sqlhelper.EndUseSingleConnection();
//            return (spRet + "\r\n" + errMsg);

//        }

//        #endregion

//        /// <summary>
//        /// 
//        /// </summary>
//        /// <param name="syxh"></param>
//        /// <param name="changedTable"></param>
//        /// <param name="executorCode"></param>
//        /// <param name="macAddress"></param>
//        /// <param name="iscqls"></param>
//        /// <returns></returns>
//        [OperationContract]
//        [FaultContract(typeof(LoginException))]
//        //bool iscqls = true;  //临时医嘱  false  长期医嘱
//        public string SendOrder(string syxh, DataTable changedTable, string executorCode, string macAddress, bool iscqls)
//        {

//            // 数据集转换成byte数组
//            MemoryStream source = new MemoryStream();
//            changedTable.WriteXml(source, XmlWriteMode.WriteSchema);

//            source.Seek(0, SeekOrigin.Begin);
//            byte[] data = new byte[source.Length];
//            source.Read(data, 0, (int)source.Length);

//            // 3 调用接口检查数据 
//            //bool iscqls = true;  //临时医嘱  false  长期医嘱
//            string mess = CheckAdvises(macAddress, syxh, data, iscqls, "utf-8");

//            // 4 调用接口同步数据
//            if (mess.IndexOf("T") > -1)
//                mess = SaveAdvises(macAddress, syxh, data, iscqls, "utf-8", "");

//            return mess;
//            //同步成功，则更新同步标志
//            //UpdateSynchFlagToTrue(currentTable, changedTable);


//        }

//    }
//}