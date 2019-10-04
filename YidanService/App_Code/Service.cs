using System;
using System.Web;
using System.Web.Services;
using System.Web.Services.Protocols;
using System.Data;
using System.Data.Sql;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Collections;
using System.Configuration;
using Winning.Emr.DoctorAdvice;
using Winning.Framework;
using Winning.Interface;
using Winning.Common.Eop;
using System.Diagnostics;

[WebService(Namespace = "http://tempuri.org/")]
[WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
public class Service : System.Web.Services.WebService
{
    private IDataAccess _sqlhelper;
   private IDataAccess _emrSqlHelper;
    private const string _spNameGetPatients = "usp_EmrDTS_GetInpatients";
    private const string _spNameUpdatePatient = "usp_EmrDTS_UpdateInpatient";
    private const string _spNameGetChangedPatients = "usp_EmrDTS_GetChangedInpatients";
    private const string _spNameGetFisrtPageInfo = "usp_Emr_QueryMedicalInfo";

    //private string _risCallUrl;
    private string _feekindMappings;
    //private string _reportDBMapping;
    //private ReportQueryManager m_ReportManager;
    private Hashtable _paras = new Hashtable();


    /// <summary>
    /// 构造所有数据访问类
    /// </summary>
    public Service()
    {
        _sqlhelper = DataAccessFactory.GetSqlDataAccess("HISDB");
        _emrSqlHelper = DataAccessFactory.DefaultDataAccess;
        InitSqlDataConfig();
    }

    #region EMR-->质量管理平台接口
    /// <summary>
    /// 获取出院病人的PDF
    /// </summary>
    /// <param name="hissyxh"></param>
    /// <returns></returns>
    [WebMethod]
    public DataTable QueryPatientPDF(string hissyxh)
    {
        DataTable ds = _emrSqlHelper.ExecuteDataTable("usp_yjjk_bllist " + Convert.ToInt32(hissyxh) + "");
       return ds;
    }

    /// <summary>
    /// 获取病人的时限质量信息
    /// </summary>
    /// <param name="cxlb">1 个人时限查询 2 按科室时限统计</param>
    /// <param name="ksrq">开始日期： cxlb=2 时有效 </param>
    /// <param name="jsrq">结束日期：cxlb=2 时有效</param>
    /// <param name="bqdm">病区代码：cxlb=2时有效</param>
    /// <param name="hissyxh">hissyxh@cxlb=1 时有效</param>
    /// <param name="lb">int 1= 未完成 2=完成  @cxlb=1 时有效</param>
    /// <returns></returns>
    [WebMethod]
    public DataTable QueryPatientQc(string cxlb, string ksrq, string jsrq, string bqdm, string hissyxh, string lb)
    {
       DataTable ds = _emrSqlHelper.ExecuteDataTable("usp_Emr_QueryPatientTimeQc  " + Convert.ToInt32(cxlb) + ",'" + ksrq + "','" + jsrq + "','" + bqdm + "'," + Convert.ToInt32(hissyxh) + "," + Convert.ToInt32(lb) + "");
       return ds;

    }
    #endregion

    /// <summary>
    /// 取得所有HIS病人信息，通过DataSyncManager.dll同步数据库
    /// </summary>
    /// <returns>数据集XML形式的byte数组</returns>
    [WebMethod]
    public byte[] GetInpatientList()
    {
        string sqlCmd = "exec " + _spNameGetPatients + " @jsrq, -1 ";
        SqlParameter param_jsrq = new SqlParameter("@jsrq", SqlDbType.VarChar, 8);
        param_jsrq.Value = DateTime.Now.ToString("yyyyMMdd");
        DataSet ds = _sqlhelper.ExecuteDataSet(sqlCmd, new SqlParameter[] { param_jsrq });

        //DataSet ds = EnvelopDataTable(dt);
        return DataSet2ByteArray(ds, Encoding.UTF8);
    }

    #region 更新病人信息

    /// <summary>
    /// 更新病人信息传入Byte[]
    /// </summary>
    /// <param name="changeData"></param>
    /// <returns></returns>
    [WebMethod]
    public string UpdateSingleInpatient(byte[] changeData)
    {
        return UpdatePatients(changeData);
    }

    /// <summary>
    /// 更新HIS病人信息，Base64String传入
    /// </summary>
    /// <param name="changeData"></param>
    /// <returns></returns>
    ///[WebMethod]
    public string UpdateSingleInpatient(string changeData)
    {
        FileStream fs = new FileStream(@"c:\3.txt", FileMode.Append);
        fs.Seek(0, SeekOrigin.End);
        byte[] bchangeData = Convert.FromBase64String(changeData);
        DataSet ds = new DataSet();
        MemoryStream ms = new MemoryStream();
        ms.Write(bchangeData, 0, bchangeData.Length);
        ms.Seek(0, SeekOrigin.Begin);
        ds.ReadXml(ms, XmlReadMode.ReadSchema);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            string sqlCmd = "exec " + _spNameUpdatePatient;

            fs.Write(Encoding.Default.GetBytes("create"), 0, 6);

            for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
            {
                fs.Write(Encoding.Default.GetBytes(ds.Tables[0].Rows[0][j].ToString()), 0,
                        ds.Tables[0].Rows[0][j].ToString().Length);
            }

            string s = "";
            try
            {
                s = CreateUpdateParams(ds.Tables[0].Rows[0]);
            }
            catch (Exception e)
            {
                fs.Write(Encoding.Default.GetBytes(e.Message), 0, e.Message.Length);
                fs.Write(Encoding.Default.GetBytes(s), 0, s.Length);
            }
            fs.Write(Encoding.Default.GetBytes("createend"), 0, 9);

            try
            {
                foreach (string key in _paras.Keys)
                {
                    SqlParameter para = _paras[key] as SqlParameter;
                    if (para.Value != null)
                    {
                        if (para.SqlDbType == SqlDbType.Int)
                            sqlCmd = sqlCmd + " " + key + "=" + para.Value.ToString() + ",";
                        else
                            sqlCmd = sqlCmd + " " + key + "='" + para.Value.ToString() + "',";
                    }
                    else
                    {
                        if (para.SqlDbType == SqlDbType.Int)
                            sqlCmd = sqlCmd + " " + key + "=-1,";
                        else
                            sqlCmd = sqlCmd + " " + key + "='',";
                    }
                }
            }
            catch (Exception e)
            {
                fs.Write(Encoding.Default.GetBytes(e.Message), 0, e.Message.Length);
            }
            sqlCmd = sqlCmd.Substring(0, sqlCmd.Length - 1);
            byte[] bsqlcmd = Encoding.Default.GetBytes(sqlCmd);
            fs.Write(bsqlcmd, 0, bsqlcmd.Length);
            _sqlhelper.ExecuteNoneQuery(sqlCmd, CommandType.Text);
        }
        fs.Flush();
        fs.Close();
        return "Update Over";
    }

    private string CreateUpdateParams(DataRow updPatInfo)
    {
        _paras.Clear();
        // 首页序号(病人每次住院的唯一标识码)
        _paras.Add("@hissyxh", new SqlParameter("@hissyxh", SqlDbType.Int));
        // 患者姓名
        _paras.Add("@hzxm", new SqlParameter("@hzxm", SqlDbType.VarChar, 32));
        // 拼音（病人姓名缩写）
        _paras.Add("@py", new SqlParameter("@py", SqlDbType.VarChar, 8));
        // 五笔（病人姓名缩写）
        _paras.Add("@wb", new SqlParameter("@wb", SqlDbType.VarChar, 8));
        // 病人性质(即 医疗付款方式)(YY_ZDFLMXK.mxdm, lbdm = "1")
        _paras.Add("@brxz", new SqlParameter("@brxz", SqlDbType.VarChar, 4));
        // 病人来源(YY_ZDFLMXK.mxdm, lbdm = "2")
        _paras.Add("@brly", new SqlParameter("@brly", SqlDbType.VarChar, 4));
        // 入院次数
        _paras.Add("@rycs", new SqlParameter("@rycs", SqlDbType.Int));
        // 病人性别(YY_ZDFLMXK.name, lbdm = "3")
        _paras.Add("@brxb", new SqlParameter("@brxb", SqlDbType.VarChar, 4));
        // 出生日期
        _paras.Add("@csrq", new SqlParameter("@csrq", SqlDbType.VarChar, 10));
        // 身份证号
        _paras.Add("@sfzh", new SqlParameter("@sfzh", SqlDbType.VarChar, 18));
        // 婚姻状况(YY_ZDFLMXK.mxdm, lbdm = "4")
        _paras.Add("@hyzk", new SqlParameter("@hyzk", SqlDbType.VarChar, 4));
        // 职业代码(YY_ZDFLMXK.mxdm, lbdm = "41")
        _paras.Add("@zydm", new SqlParameter("@zydm", SqlDbType.VarChar, 4));
        // 省市代码(YY_StringK.String, dqlb = 1000)
        _paras.Add("@ssdm", new SqlParameter("@ssdm", SqlDbType.VarChar, 10));
        // 区县代码(YY_StringK.String, dqlb = 1001)
        _paras.Add("@qxdm", new SqlParameter("@qxdm", SqlDbType.VarChar, 10));
        // 民族代码(YY_ZDFLMXK.mxdm, lbdm = "42")
        _paras.Add("@mzdm", new SqlParameter("@mzdm", SqlDbType.VarChar, 4));
        // 国籍代码(YY_ZDFLMXK.mxdm, lbdm = "43")
        _paras.Add("@gjdm", new SqlParameter("@gjdm", SqlDbType.VarChar, 4));
        // 工作单位(名称)
        _paras.Add("@gzdw", new SqlParameter("@gzdw", SqlDbType.VarChar, 64));
        // 单位地址
        _paras.Add("@dwdz", new SqlParameter("@dwdz", SqlDbType.VarChar, 64));
        // 单位电话
        _paras.Add("@dwdh", new SqlParameter("@dwdh", SqlDbType.VarChar, 16));
        // 单位邮编
        _paras.Add("@dwyb", new SqlParameter("@dwyb", SqlDbType.VarChar, 16));
        // 户口地址
        _paras.Add("@hkdz", new SqlParameter("@hkdz", SqlDbType.VarChar, 64));
        // 户口电话
        _paras.Add("@hkdh", new SqlParameter("@hkdh", SqlDbType.VarChar, 16));
        // 户口邮编
        _paras.Add("@hkyb", new SqlParameter("@hkyb", SqlDbType.VarChar, 16));
        // 联系人名
        _paras.Add("@lxrm", new SqlParameter("@lxrm", SqlDbType.VarChar, 32));
        // 联系关系(YY_ZDFLMXK.mxdm, lbdm = "44")
        _paras.Add("@lxgx", new SqlParameter("@lxgx", SqlDbType.VarChar, 4));
        // 联系地址
        _paras.Add("@lxdz", new SqlParameter("@lxdz", SqlDbType.VarChar, 64));
        // 联系(人)单位
        _paras.Add("@lxdw", new SqlParameter("@lxdw", SqlDbType.VarChar, 64));
        // 联系电话
        _paras.Add("@lxdh", new SqlParameter("@lxdh", SqlDbType.VarChar, 16));
        // 联系邮编
        _paras.Add("@lxyb", new SqlParameter("@lxyb", SqlDbType.VarChar, 16));
        // 病史陈述者
        _paras.Add("@bscsz", new SqlParameter("@bscsz", SqlDbType.VarChar, 64));
        // 入院情况(入院时病情, YY_ZDFLMXK.mxdm, lbdm = "5")
        _paras.Add("@ryqk", new SqlParameter("@ryqk", SqlDbType.VarChar, 4));
        // 入院科室(YY_StringK.String)
        _paras.Add("@ryks", new SqlParameter("@ryks", SqlDbType.VarChar, 6));
        // 入院病区(YY_BQDMK.bqdm)
        _paras.Add("@rybq", new SqlParameter("@rybq", SqlDbType.VarChar, 6));
        // 入院床位(BL_StringK.String)
        _paras.Add("@rycw", new SqlParameter("@rycw", SqlDbType.VarChar, 12));
        // 入院日期
        _paras.Add("@ryrq", new SqlParameter("@ryrq", SqlDbType.VarChar, 19));
        // 入区日期
        _paras.Add("@rqrq", new SqlParameter("@rqrq", SqlDbType.VarChar, 19));
        // 出院科室(YY_StringK.String)
        _paras.Add("@cyks", new SqlParameter("@cyks", SqlDbType.VarChar, 6));
        // 出院病区(YY_BQDMK.bqdm)
        _paras.Add("@cybq", new SqlParameter("@cybq", SqlDbType.VarChar, 6));
        // 出院床位(BL_StringK.String)
        _paras.Add("@cycw", new SqlParameter("@cycw", SqlDbType.VarChar, 12));
        // 出区日期
        _paras.Add("@cqrq", new SqlParameter("@cqrq", SqlDbType.VarChar, 19));
        // 住院天数
        _paras.Add("@zyts", new SqlParameter("@zyts", SqlDbType.Int));
        // 入院途径(YY_ZDFLMXK.mxdm, lbdm = "6")
        _paras.Add("@rytj", new SqlParameter("@rytj", SqlDbType.VarChar, 4));
        // 出院方式(YY_ZDFLMXK.mxdm, lbdm = "15")
        _paras.Add("@cyfs", new SqlParameter("@cyfs", SqlDbType.VarChar, 4));
        // 门诊医生(YY_ZGDMK.zgdm)
        _paras.Add("@mzys", new SqlParameter("@mzys", SqlDbType.VarChar, 6));
        // 文化程度(精神专用)(YY_ZDFLMXK.mxdm, lbdm = "25")
        _paras.Add("@whcd", new SqlParameter("@whcd", SqlDbType.VarChar, 4));
        // 病人状态(YY_SJLBMXK.mxbh, lbbh = 15)
        _paras.Add("@brzt", new SqlParameter("@brzt", SqlDbType.Int));
        // 危重级别(YY_ZDFLMXK.mxdm, lbdm = "5")
        _paras.Add("@wzjb", new SqlParameter("@wzjb", SqlDbType.VarChar, 4));
        // 婴儿标志(YY_SJLBMXK.mxbh, lbbh = 0)
        _paras.Add("@yebz", new SqlParameter("@yebz", SqlDbType.Int));
        // 病人类型(YY_ZDFLMXK.mxdm, lbdm = "45")
        _paras.Add("@brlx", new SqlParameter("@brlx", SqlDbType.VarChar, 4));
        //护理级别
        _paras.Add("@hljb", new SqlParameter("@hljb", SqlDbType.VarChar, 12));
        // 备注
        _paras.Add("@memo", new SqlParameter("@memo", SqlDbType.VarChar, 16));

        string snowfield = "";
        foreach (string key in _paras.Keys)
        {
            snowfield = key;
            SqlParameter para = _paras[key] as SqlParameter;
            if (updPatInfo.Table.Columns.Contains(key.Substring(1)))
            {
                if (para.SqlDbType == SqlDbType.Int)
                {
                    try
                    {
                        (_paras[key] as SqlParameter).Value = Convert.ToInt32(updPatInfo[key.Substring(1)].ToString());
                    }
                    catch
                    {
                        (_paras[key] as SqlParameter).Value = 0;
                    }

                }
                else
                    (_paras[key] as SqlParameter).Value = updPatInfo[key.Substring(1)].ToString();
            }
        }
        return "";
    }

    /// <summary>
    /// 更新数据集病人信息
    /// </summary>
    /// <param name="changeData"></param>
    /// <returns></returns>
    [WebMethod]
    public string UpdateAllInpatients(byte[] changeData)
    {
        return UpdatePatients(changeData);
    }

    private string UpdatePatients(byte[] changeData)
    {
        //FileStream fs = new FileStream(@"c:\3.txt", FileMode.Append);
        //fs.Seek(0, SeekOrigin.End);

        DataSet ds = new DataSet();
        MemoryStream ms = new MemoryStream();
        ms.Write(changeData, 0, changeData.Length);
        ms.Seek(0, SeekOrigin.Begin);
        ds.ReadXml(ms, XmlReadMode.ReadSchema);

        if (ds != null && ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
        {
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                string sqlCmd = "exec " + _spNameUpdatePatient;

                //fs.Write(Encoding.Default.GetBytes("create"), 0, 6);

                //for (int j = 0; j < ds.Tables[0].Columns.Count; j++)
                //{
                //    fs.Write(Encoding.Default.GetBytes(ds.Tables[0].Columns[j].ColumnName), 0,
                //            ds.Tables[0].Columns[j].ColumnName.Length);
                //    fs.Write(Encoding.Default.GetBytes(ds.Tables[0].Rows[i][j].ToString()), 0,
                //            ds.Tables[0].Rows[i][j].ToString().Length);
                //    fs.Write(Encoding.Default.GetBytes(","), 0, 1);
                //}

                string s = "";
                try
                {
                    s = CreateUpdateParams(ds.Tables[0].Rows[i]);
                }
                catch
                {
                }

                //fs.Write(Encoding.Default.GetBytes("createend"), 0, 9);

                try
                {
                    foreach (string key in _paras.Keys)
                    {
                        SqlParameter para = _paras[key] as SqlParameter;
                        if (para.Value != null)
                        {
                            if (para.SqlDbType == SqlDbType.Int)
                                sqlCmd = sqlCmd + " " + key + "=" + para.Value.ToString() + ",";
                            else
                                sqlCmd = sqlCmd + " " + key + "='" + para.Value.ToString() + "',";
                        }
                        else
                        {
                            if (para.SqlDbType == SqlDbType.Int)
                                sqlCmd = sqlCmd + " " + key + "=0,";
                            else
                                sqlCmd = sqlCmd + " " + key + "='',";
                        }
                    }
                }
                catch
                {
                }
                sqlCmd = sqlCmd.Substring(0, sqlCmd.Length - 1);
                byte[] bsqlcmd = Encoding.Default.GetBytes(sqlCmd);

                //fs.Write(bsqlcmd, 0, bsqlcmd.Length);

                _sqlhelper.ExecuteNoneQuery(sqlCmd, CommandType.Text);
            }
        }
        //fs.Flush();
        //fs.Close();
        return "Update Over";
    }

    #endregion

    #region 取得变化的病人信息
    /// <summary>
    /// 取得变化的病人信息
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public byte[] GetChangedInpatientList()
    {
        string sqlCmd = "exec " + _spNameGetChangedPatients;
        DataSet ds = _sqlhelper.ExecuteDataSet(sqlCmd, CommandType.Text);
        //DataSet ds = EnvelopDataTable(dt);
        return DataSet2ByteArray(ds, Encoding.UTF8);
    }
    #endregion

    //#region 取得病人医技报告信息
    ///// <summary>
    ///// 取得病人医技报告信息
    ///// </summary>
    ///// <returns></returns>
    //[WebMethod]
    //public byte[] GetPatientRisReports(string patId)
    //{
    //   //string sqlCmd = " select repno,replb,replbmc,reprq "
    //   //              + " from SF_YS_REPORT r inner join SF_YS_REPORT_CLASS c on r.replb = c.id "
    //   //              + " where syxh = '" + patId + "' and c.type = 'RIS' and xtbz = '1'";
    //   DataTable dt = TechReportInterfaceHandle.GetReportList( m_ ReportManager.GetAllReport("RIS", patId, DateTime.MinValue, DateTime.MaxValue, true) as DataTable;
    //   //dt = _sqlhelper.ExecuteDataTable(sqlCmd, CommandType.Text);
    //   if (dt == null)
    //      return null;
    //   DataSet ds = EnvelopDataTable(dt);
    //   return DataSet2ByteArray(ds, Encoding.UTF8);
    //}

    //#endregion

    #region 取医技报告
    /// <summary>
    /// 取得所有已经发布的医技报告
    /// </summary>
    /// <param name="patId"></param>
    /// <returns></returns>
    [WebMethod]
    public byte[] GetPatientAllReports(string patId)
    {
        DataTable result = TechReportInterfaceHandle.GetReportList(patId, false);
        if (result == null) return null;
        DataSet ds = EnvelopDataTable(result);
        return DataSet2ByteArray(ds, Encoding.UTF8);
    }

    #endregion

    #region 取指定报告内容

    /// <summary>
    /// 取得指定报告的内容(数据源由配置决定)
    /// </summary>
    /// <param name="patId"></param>
    /// <param name="bgxh"></param>
    /// <param name="bglb">医技报告大类</param>
    /// <param name="bglbid">报告类别</param>
    /// <returns></returns>
    [WebMethod]
    public byte[] GetPatientReportContent(string patId, string bgxh, string bglb, string bglbid)
    {
        DataTable dt = TechReportInterfaceHandle.GetReportData(patId, false, bgxh, bglb, bglbid);

        //ITechDataReader reader = m_ReportManager.GetQueryInterface(bglb, string.Empty) as ITechDataReader;
        //if (reader != null)
        //   dt = reader.GetReportData(patId, DateTime.MinValue, DateTime.MaxValue, bglb, string.Empty, true, bgxh) as DataTable;
        //if (bglb == "RIS")
        //{
        //   reader = m_ReportManager.GetQueryInterface("RIS", "AllRis") as ITechDataReader;
        //   if (reader != null)
        //      dt = reader.GetReportData(patId, DateTime.MinValue, DateTime.MaxValue, "RIS", "AllRis", true, bgxh) as DataTable;
        //}
        //else if (bglb == "LIS")
        //{
        //   reader = m_ReportManager.GetQueryInterface("LIS", "AllLis") as ITechDataReader;
        //   if (reader != null)
        //      dt = reader.GetReportData(patId, DateTime.MinValue, DateTime.MaxValue, "LIS", "AllLis", true, bgxh) as DataTable;
        //}
        //string sqlRis = " exec usp_Emr_GetPatientRisReportResult "
        //    + patId + "," + bgxh;

        //string sqlLis = " exec Wsp_GetPubReport "
        //    + " 0, " + bgxh;

        //string sqlUltraSound = GetUltraSoundSql(patId);
        //DataTable dt = null;

        //if (bglb.ToUpper() == "RIS")
        //{
        //   dt = _sqlhelper.ExecuteDataTable(sqlRis, CommandType.Text);
        //}
        //else if (bglb.ToUpper() == "LIS")
        //{
        //   IDataAccess _sqlhelperLis = DataAccessFactory.GetSqlDataAccess("LISDB");
        //   dt = _sqlhelperLis.ExecuteDataTable(sqlLis, CommandType.Text);
        //}
        //else if (bglb.ToUpper() == "ULTRASOUND")
        //{
        //   IDataAccess _sqlHelperLZ = DataAccessFactory.GetSqlDataAccess(GetDBName(bglb.ToUpper()));
        //   if (_sqlHelperLZ == null)
        //      dt = null;
        //   else
        //      dt = _sqlHelperLZ.ExecuteDataTable(sqlUltraSound, CommandType.Text);
        //}

        if (dt == null) return null;

        DataSet ds = EnvelopDataTable(dt);
        return DataSet2ByteArray(ds, Encoding.UTF8);
    }
    #endregion

    #region 取得病人扩展信息

    /// <summary>
    /// 取得病人扩展信息
    /// </summary>
    /// <param name="patId"></param>
    /// <returns></returns>
   [WebMethod]
   public byte[] GetPatientExtraInfo(string patId)
   {
      string sqlCmd = " exec usp_Emr_GetPatientExtraInfo " + patId;
      
      _sqlhelper = new SqlDataAccess("HISDB");

      //DataTable dt = _sqlhelper.ExecuteDataTable(sqlCmd, CommandType.Text);
      //DataSet ds = EnvelopDataTable(dt);
      DataSet ds = _sqlhelper.ExecuteDataSet(sqlCmd, CommandType.Text);

     // ds.WriteXml("C:\\webservice_log.txt", XmlWriteMode.WriteSchema);

      if (ds.Tables[0].Rows.Count > 0 && ds.Tables[0].Columns.Count > 0)
      {
         try
         {
            object dr = ds.Tables[0].Rows[0]["性别"];
         }
         catch
         {
            //ds.WriteXml("C:\\webservice_log.txt", XmlWriteMode.WriteSchema);
            _sqlhelper.ExecuteNoneQuery("insert TEMP_EMR_MSG (msg) values ('"+ds.GetXmlSchema()+"') ");

         }
      }
      return DataSet2ByteArray(ds, Encoding.UTF8);
   }
    #endregion

    #region 取得病区科室病人扩展信息

    /// <summary>
    /// 取得病区科室病人扩展信息(科室+病区)
    /// </summary>
    /// <param name="dept"></param>
    /// <param name="ward"></param>
    /// <returns></returns>
    [WebMethod]
    public byte[] GetDeptPatientsExtraInfo(string dept, string ward)
    {
        string sqlCmd = " exec usp_Emr_GetPatientExtraInfo_ex '" + dept + "','" + ward + "'";
        DataTable dt = _sqlhelper.ExecuteDataTable(sqlCmd);
        DataSet ds = EnvelopDataTable(dt);
        return DataSet2ByteArray(ds, Encoding.UTF8);
    }
    #endregion

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
    [WebMethod]
    public string CheckAdvises(string wkdz, string syxh, byte[] xmlcqdata, bool iscqls, string encodingName)
    {
        string cqlsbz;
        string errMsg = string.Empty;
        string spRet = string.Empty;
        string sqlcmd = string.Empty;
        _sqlhelper = new SqlDataAccess("HISDB");// DataAccessFactory.GetSqlDataAccess("HISDB");
        Encoding encoding = Encoding.GetEncoding(encodingName);
        DataSet dsAdvices = this.ByteArray2DataSet(xmlcqdata, encoding);
        if ((dsAdvices == null) || (dsAdvices.Tables.Count == 0))
        {
            return "F传入的医嘱数据无法解析";
        }
        try
        {
            this._sqlhelper.BeginUseSingleConnection();
            sqlcmd = "exec usp_Emr_CheckDoctorAdvice '" + wkdz + "',1," + syxh;
            DataTable dtret = this._sqlhelper.ExecuteDataTable(sqlcmd, CommandType.Text);
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
                this._sqlhelper.EndUseSingleConnection();
                return spRet;
            }
            errMsg = errMsg + "\r\n" + sqlcmd + " exec step1 ";
        }
        catch (Exception e)
        {
            this._sqlhelper.EndUseSingleConnection();
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
        DataTable dtAdvices = dsAdvices.Tables[0];
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
                string blyzzt = dr["yzzt"].ToString();
                string sql = "exec usp_Emr_CheckDoctorAdvice '" + wkdz + "', 2," + syxh + "," + cqlsbz + ", " + yzxh + "," + idm + ",'" + xmdm + "'," + blyzzt;
                errMsg = errMsg + "\r\n" + sql;
                DataTable dtstep2ret = this._sqlhelper.ExecuteDataTable(sql, CommandType.Text);
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
                    this._sqlhelper.EndUseSingleConnection();
                    return spRet;
                }
            }
            catch (Exception e)
            {
                this._sqlhelper.EndUseSingleConnection();
                return (e.Message + errMsg);
            }
        }
        sqlcmd = "exec usp_Emr_CheckDoctorAdvice '" + wkdz + "', 3," + syxh;
        DataTable dtstep3ret = this._sqlhelper.ExecuteDataTable(sqlcmd, CommandType.Text);
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
        this._sqlhelper.EndUseSingleConnection();
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
    [WebMethod]
    public string SaveAdvises(string wkdz, string syxh, byte[] xmlcqdata, bool iscqls, string encodingName, string lyjzsj)
    {

        string cqlsbz;
        string errMsg = string.Empty;
        string spRet = string.Empty;
        string sqlcmd = string.Empty;
        Encoding encoding = Encoding.GetEncoding(encodingName);

        _sqlhelper = new SqlDataAccess("HISDB");

        DataSet dsAdvices = this.ByteArray2DataSet(xmlcqdata, encoding);
        if ((dsAdvices == null) || (dsAdvices.Tables.Count == 0))
        {
            return "F传入的医嘱数据无法解析";
        }
        try
        {
            this._sqlhelper.BeginUseSingleConnection();
            sqlcmd = "exec usp_Emr_SaveDoctorAdvice '" + wkdz + "',1," + syxh;
            DataTable dtret = this._sqlhelper.ExecuteDataTable(sqlcmd, CommandType.Text);
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
                this._sqlhelper.EndUseSingleConnection();
                return spRet;
            }
            errMsg = errMsg + "\r\n" + sqlcmd + " exec step1 ";
        }
        catch (Exception e)
        {
            this._sqlhelper.EndUseSingleConnection();
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
        DataTable dtAdvices = dsAdvices.Tables[0];
        for (int i = 0; i < dtAdvices.Rows.Count; i++)
        {
            try
            {
                DataRow dr = dtAdvices.Rows[i];
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
                string sql = "exec usp_Emr_SaveDoctorAdvice '" + wkdz + "', 2," + syxh + ",'00'," + cqlsbz + ", " + yzno + ",'" + docid + "'," + yzlb + ",'" + ksrq + "'," + xmlb + "," + idm + ",'" + xmdm + "','" + zxks + "'," + ypjl + ",'" + jldw + "'," + dwbz + "," + dwlb + ",'" + yfdm + "','" + yznr + "','" + ztnr + "','" + pcdm + "','" + zdm + "','" + zxsj + "'," + zbbz + ",0,0," + tzxh + ",'" + tzrq + "','" + mzdm + "','" + zdys + "'," + ybspbz + ",'" + ybspbh + "','" + lcxmdm + "','" + ypmc + "'," + sybpno + "," + sqdxh + "," + yzxh + "," + zyzxh + ",'" + shczy + "','" + shrq + "','" + qxysdm + "','" + qxrq + "'," + blyzzt + "," + ypzsl + "," + mq + ",'" + lyjzsj + "','" + memo + "','" + sqdxmbz + "','" + bbzlId + "','" + bbzl + "'," + sqdjjbz;
                errMsg = errMsg + "\r\n" + sql;
                DataTable dtstep2ret = this._sqlhelper.ExecuteDataTable(sql, CommandType.Text);
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
                    this._sqlhelper.EndUseSingleConnection();
                    return spRet;
                }
            }
            catch (Exception e)
            {
                this._sqlhelper.EndUseSingleConnection();
                return (e.Message + errMsg);
            }
        }
        sqlcmd = "exec usp_Emr_SaveDoctorAdvice '" + wkdz + "', 3," + syxh + ", '00'," + cqlsbz;
        DataTable dtstep3ret = this._sqlhelper.ExecuteDataTable(sqlcmd, CommandType.Text);
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
        this._sqlhelper.EndUseSingleConnection();
        return (spRet + "\r\n" + errMsg);

    }

    #endregion

    #region 医嘱审核
    /// <summary>
    /// EMR医嘱审核接口,返回的异常中包含了审核不通过的原因。
    /// 传入的调用者工号和网卡地址在调用HIS接口是要用到，要避免冲突。
    /// </summary>
    /// <param name="executorCode">调用审核过程的操作员工号</param>
    /// <param name="macAddress">审核过程调用端的网卡地址</param>
    /// <param name="selectedLongs">要审核的长期医嘱序号集合,","号分隔</param>
    /// <param name="selectedTemps">要审核的临时医嘱序号集合,","号分隔</param>
    /// <param name="auditor">审核者工号</param>
    /// <param name="auditTime">审核时间</param>
    [WebMethod]
    //public string AuditOrder(string hisSyxh, string executorCode, string macAddress,
    //    string selectedLongs, string selectedTemps, string auditor, string auditTime)
    public string AuditOrder(string hisSyxh, string executorCode, string macAddress,
        string selectedLongs, string selectedTemps, string auditor, string auditTime)
    {
        try
        {
            Winning.Framework.IDataAccess sqlHelper = Winning.Framework.DataAccessFactory.GetSqlDataAccess();
            decimal syxh = decimal.Parse(hisSyxh);
            OrderInterfaceLogic oifl = new OrderInterfaceLogic(sqlHelper, macAddress, decimal.Parse(hisSyxh));
            DateTime dt = DateTime.Parse(auditTime);
            string[] longlist = selectedLongs.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            string[] templist = selectedTemps.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            decimal[] longs = new decimal[longlist.Length];
            decimal[] temps = new decimal[templist.Length];
            for (int i = 0; i < longlist.Length; i++)
                longs[i] = decimal.Parse(longlist[i]);
            for (int i = 0; i < templist.Length; i++)
                temps[i] = decimal.Parse(templist[i]);
            oifl.AuditOrder(executorCode, macAddress, longs, temps, auditor, dt);

            return "T";
        }
        catch (Exception e)
        {
            return "F" + "\r\n" + e.Message + "\r\n" + e.Source + "\r\n" + e.StackTrace;
        }
    }

    #endregion

    #region 重新同步未发送的医嘱到HIS中
    /// <summary>
    /// 重新同步未发送的医嘱到HIS中
    /// </summary>
    /// <param name="executorCode"></param>
    /// <param name="macAddress"></param>
    /// <param name="patID"></param>
    [WebMethod]
    public void ResendSynchRecords(string executorCode, string patID, string macAddress)
    {
        try
        {
            Winning.Framework.IDataAccess sqlHelper = Winning.Framework.DataAccessFactory.GetSqlDataAccess();
            decimal hisSyxh = Convert.ToDecimal(patID);
            OrderInterfaceLogic oifl = new OrderInterfaceLogic(sqlHelper, macAddress, hisSyxh);
            oifl.ResendSynchRecords(executorCode, macAddress);
        }
        catch
        {
            throw new Exception("失败：重新同步未发送的医嘱到HIS中");
        }
    }

    #endregion

    #region 同步医嘱执行结果
    /// <summary>
    /// 同步医嘱执行结果接口(供HIS调用)
    /// 传入的数据集中包括
    ///       cqyzxh  utXh12      --长期医嘱序号 （或者 lsyzxh）
    /// 	zxczy   utMc16      --执行操作员(代码)
    /// 	zxrq    utDatetime  --执行日期
    ///       yzzt    utBz        --医嘱状态(EMR中定义的状态)
    /// </summary>
    /// <param name="hisSyxh"></param>
    /// <param name="longOrderTable">已执行的长期医嘱数据集</param>
    /// <param name="tempOrderTable">已执行的临时医嘱数据集</param>
    [WebMethod]
    public string SynchExecute(string hisSyxh, string longOrderTable, string tempOrderTable)
    //public string SynchExecute()
    {
        string ret = string.Empty;
        try
        {
            Winning.Framework.IDataAccess sqlHelper = Winning.Framework.DataAccessFactory.GetSqlDataAccess();
            ret += "数据访问创建成功,";
            OrderInterfaceLogic oifl = new OrderInterfaceLogic(sqlHelper, string.Empty, decimal.Parse(hisSyxh));
            ret += "医嘱接口逻辑创建成功,";
            oifl.SynchExecuteResultToEmr(
                Xml2DataTable(longOrderTable, true),
                Xml2DataTable(tempOrderTable, false));
            ret = "T";
            return ret;
        }
        catch (Exception e)
        {
            return "F" + ret + "\r\n" + e.Message + "\r\n" + e.Source + "\r\n" + e.StackTrace;
        }
    }

    DataTable Xml2DataTable(string fmtstring, bool islong)
    {
        DataTable ordertable = new DataTable();
        DataColumn dccqyzxh = new DataColumn("cqyzxh", typeof(decimal));
        DataColumn dclsyzxh = new DataColumn("lsyzxh", typeof(decimal));
        DataColumn dczxczy = new DataColumn("zxczy", typeof(string));
        DataColumn dczxrq = new DataColumn("zxrq", typeof(string));
        DataColumn dcyzzt = new DataColumn("yzzt", typeof(Int16));
        if (islong)
        {
            ordertable.Columns.AddRange(new DataColumn[]{
               dccqyzxh, dczxczy, dczxrq, dcyzzt
           });
        }
        else
        {
            ordertable.Columns.AddRange(new DataColumn[]{
               dclsyzxh, dczxczy, dczxrq, dcyzzt
           });
        }
        string[] records = fmtstring.Split(new string[] { "|||" }, StringSplitOptions.RemoveEmptyEntries);
        for (int i = 0; i < records.Length; i++)
        {
            string rec = records[i];
            string[] fields = rec.Split(new string[] { "//" }, StringSplitOptions.None);
            if (fields.Length < 4)
                throw new InvalidDataException("数据格式有误!");
            DataRow order = ordertable.NewRow();
            order[0] = fields[0];
            order[1] = fields[1];
            order[2] = fields[2];
            order[3] = Int16.Parse(fields[3]);
            ordertable.Rows.Add(order);
        }
        return ordertable;
    }

    #endregion

    #region 从HIS获取病人的医嘱数据
    /// <summary>
    /// 从HIS获取病人的医嘱数据
    /// </summary>
    /// <param name="queryType">查询类型，1 按病人 2 按科室+病区</param>
    /// <param name="hisFirstPageNo">HIS的首页序号</param>
    /// <param name="wardCode">病区代码，cxlx = 2 时有效，此时若为仍空则表示不限定病区</param>
    /// <param name="deptCode">科室代码，cxlx = 2 时有效，此时若为仍空则表示不限定科室</param>
    /// <param name="orderType">医嘱类型，0 全部 1 长期 2 临时</param>
    /// <param name="orderState">医嘱状态，0 所有 1 有效</param>
    [WebMethod]
    public byte[] GetHisOrder(int queryType, string hisFirstPageNo, string wardCode, string deptCode
       , int orderType, int orderState)
    {
        byte[] buffer;
        try
        {
            SqlParameter[] parameterArray2 = new SqlParameter[] { new SqlParameter("cxlx", SqlDbType.SmallInt), new SqlParameter("syxh", SqlDbType.Decimal), new SqlParameter("bqdm", SqlDbType.VarChar, 8), new SqlParameter("ksdm", SqlDbType.VarChar, 8), new SqlParameter("yzlx", SqlDbType.SmallInt), new SqlParameter("yzzt", SqlDbType.SmallInt) };
            parameterArray2[0].Value = queryType;
            parameterArray2[1].Value = Convert.ToDecimal(hisFirstPageNo);
            parameterArray2[2].Value = wardCode;
            parameterArray2[3].Value = deptCode;
            parameterArray2[4].Value = orderType;
            parameterArray2[5].Value = orderState;
            DataSet dataset = this._sqlhelper.ExecuteDataSet("usp_EmrDTS_GetDoctorAdvice", parameterArray2, CommandType.StoredProcedure);
            buffer = this.DataSet2ByteArray(dataset, Encoding.UTF8);
        }
        catch
        {
            throw new Exception("从HIS读取医嘱数据失败");
        }
        return buffer;
    }
    #endregion

    #region 取得病人手术信息

    /// <summary>
    /// 取得病人手术信息
    /// </summary>
    /// <param name="deptId">科室代码</param>
    /// <param name="wardId">病区代码</param>
    /// <param name="patId">His中的首页序号(-1时取科室病区所有病人)</param>
    /// <param name="operId">手术代码</param>
    /// <param name="anaId">麻醉代码</param>
    /// <param name="preOperDiag">术前诊断</param>
    /// <param name="operRoom">手术室</param>
    /// <param name="operReqState">手术申请状态</param>
    /// <param name="startDate">开始日期</param>
    /// <param name="endDate">结束日期</param>
    /// <param name="encodingName">编码</param>
    /// <returns></returns>
    [WebMethod]
    public byte[] GetPatientOperationInfo(string deptId, string wardId, string patId
        , string operId, string anaId, string preOperDiag, string operRoom, int operReqState
        , string startDate, string endDate, string encodingName)
    {
        string sqlCmd = " exec usp_Emr_QueryOperationInfo "
                            + "'" + deptId + "', '" + wardId + "'," + patId
                            + ",'" + operId + "','" + anaId + "','" + preOperDiag + "'"
                            + ",'" + operRoom + "'," + operReqState
                            + ",'" + startDate + "','" + endDate + "'";

        DataSet ds = _sqlhelper.ExecuteDataSet(sqlCmd);

        return DataSet2ByteArray(ds, Encoding.GetEncoding(encodingName));
    }

    #endregion

    #region 同步医技申请单信息

    /// <summary>
    /// 同步医技申请单信息(单条)
    /// </summary>
    /// <param name="updateKind">更新类别(0:Add, 1:Update, 2:Delete)</param>
    /// <param name="reqXh"></param>
    /// <param name="hisPatId"></param>
    /// <param name="reqModelXh"></param>
    /// <param name="reqModelKind"></param>
    /// <param name="inputTime"></param>
    /// <param name="opId"></param>
    /// <param name="reqDeptId"></param>
    /// <param name="execDeptId"></param>
    /// <param name="recState"></param>
    /// <param name="reqFormXml"></param>
    /// <param name="reqFormText"></param>
    [WebMethod]
    public void SynchMedTechReqForm(int updateKind, int reqXh, int hisPatId, int reqModelXh,
        string reqModelKind, DateTime inputTime, string opId, string reqDeptId, string execDeptId,
        int recState, string reqFormXml, string reqFormText)
    {
        string sqlCmd = string.Empty;
        if (updateKind == 0)
            sqlCmd = " insert into ZY_BRSQD_BL(xh,syxh,mbxh,mblb,lrrq "
                        + ",czyh,sqks,zxks,qrbz,jszt,jlzt,sqdnr,sqdtext) "
                        + " values(@xh, @syxh, @mbxh, @mblb, @lrrq "
                        + ",@czyh,@sqks, @zxks, @qrbz, @jszt, @jlzt, @sqdnr, @sqdtext) ";
        if (updateKind == 1)
            sqlCmd = " update ZY_BRSQD_BL "
                       + " set zxks=@zxks, sqdnr=@sqdnr, sqdtext=@sqdtext, qrbz=@qrbz "
                       + " where xh=@xh ";
        if (updateKind == 2)
            sqlCmd = " delete from ZY_BRSQD_BL "
                    + " where xh=@xh";
        SqlParameter paramXh = new SqlParameter("xh", SqlDbType.Int);
        paramXh.Value = reqXh;
        SqlParameter paramSyxh = new SqlParameter("syxh", SqlDbType.Int);
        paramSyxh.Value = hisPatId;
        SqlParameter paramMbxh = new SqlParameter("mbxh", SqlDbType.Int);
        paramMbxh.Value = reqModelXh;
        SqlParameter paramMblb = new SqlParameter("mblb", SqlDbType.VarChar, 4);
        paramMblb.Value = reqModelKind;
        SqlParameter paramLrrq = new SqlParameter("lrrq", SqlDbType.VarChar, 16);
        paramLrrq.Value = inputTime.ToString("yyyyMMddhh:mm:ss");
        SqlParameter paramCzyh = new SqlParameter("czyh", SqlDbType.VarChar, 6);
        paramCzyh.Value = opId;
        SqlParameter paramSqks = new SqlParameter("sqks", SqlDbType.VarChar, 4);
        paramSqks.Value = reqDeptId;
        SqlParameter paramZxks = new SqlParameter("zxks", SqlDbType.VarChar, 4);
        paramZxks.Value = execDeptId;
        SqlParameter paramQrbz = new SqlParameter("qrbz", SqlDbType.Int);
        paramQrbz.Value = recState;
        SqlParameter paramJszt = new SqlParameter("jszt", SqlDbType.Int);
        paramJszt.Value = 0;
        SqlParameter paramJlzt = new SqlParameter("jlzt", SqlDbType.Int);
        paramJlzt.Value = 0;
        SqlParameter paramSqdnr = new SqlParameter("sqdnr", SqlDbType.Text);
        paramSqdnr.Value = reqFormXml;
        SqlParameter paramSqdtext = new SqlParameter("sqdtext", SqlDbType.Text);
        paramSqdtext.Value = reqFormText;

        _sqlhelper.ExecuteNoneQuery(sqlCmd,
            new SqlParameter[]{
                paramXh, paramSyxh, paramMbxh,paramMblb,paramLrrq,paramCzyh
                ,paramSqks,paramZxks,paramQrbz,paramJszt,paramJlzt
                ,paramSqdnr,paramSqdtext
            });
    }

    #endregion

    #region 申请单删除时处理医嘱

    /// <summary>
    /// 删除检验申请单医嘱
    /// </summary>
    /// <param name="applySerialNo"></param>
    [WebMethod]
    public void DeleteTFRequestAdvice(decimal applySerialNo, string executorCode)
    {
        DeleteRequestAdvice(applySerialNo, executorCode, OrderInterfaceLogic.RequestFormCategory.TF);
    }

    /// <summary>
    /// 删除检查申请单医嘱
    /// </summary>
    /// <param name="applySerialNo"></param>
    [WebMethod]
    public void DeleteCLRequestAdvice(decimal applySerialNo, string executorCode)
    {
        DeleteRequestAdvice(applySerialNo, executorCode, OrderInterfaceLogic.RequestFormCategory.CL);
    }

    void DeleteRequestAdvice(decimal applySerialNo, string executorCode, Winning.Emr.DoctorAdvice.OrderInterfaceLogic.RequestFormCategory kind)
    {
        Winning.Framework.IDataAccess sqlHelper = Winning.Framework.DataAccessFactory.GetSqlDataAccess();
        decimal sqdxh = OrderInterfaceLogic.GetSend2HisApplySerialNo(applySerialNo, kind);
        DataTable dt = sqlHelper.ExecuteDataTable(
            "select syxh from BL_LSYZK where sqdxh=" + sqdxh.ToString()
             );
        if (dt != null && dt.Rows.Count > 0)
        {
            decimal syxh = decimal.Parse(dt.Rows[0][0].ToString());
            Inpatient patient = new Inpatient(syxh);
            patient.ReInitializeProperties();
            OrderInterfaceLogic oifl = new OrderInterfaceLogic(sqlHelper, string.Empty, patient);
            oifl.SaveRequestFormData(executorCode, applySerialNo, kind, string.Empty,
               new System.Collections.Generic.List<OrderInterfaceLogic.RequestFormItem>(),
               DateTime.Now, DateTime.Now,
               RecordState.Deleted);
        }
    }

    #endregion

    #region 申请单调阅相关报告

    const string SqlSelectTechWebInterfaceUrl = "select config from YY_CONFIG where id='G046'";
    const string SqlSelectRequestRelateReport = "select xmmc, bgysdm, jcysdm, qqrq, qrrq, bglx, bgdh, bgzt, @url+isnull(bgdh,'') as url from BQ_YJQQK where sqdxh=@sqdxh";

    /// <summary>
    /// 取得申请单相关的报告
    /// </summary>
    /// <param name="applySerialNo"></param>
    /// <returns></returns>
    [WebMethod]
    public DataTable GetPatientReport(decimal applySerialNo, OrderInterfaceLogic.RequestFormCategory kind)
    {
        applySerialNo = OrderInterfaceLogic.GetSend2HisApplySerialNo(applySerialNo, kind);
        string webUrl = string.Empty;

        DataTable dt = _sqlhelper.ExecuteDataTable(SqlSelectTechWebInterfaceUrl);
        if (dt != null && dt.Rows.Count > 0)
        {
            webUrl = dt.Rows[0][0].ToString();
        }

        return _sqlhelper.ExecuteDataTable(SqlSelectRequestRelateReport, new SqlParameter[]{
            new SqlParameter("sqdxh", applySerialNo),
            new SqlParameter("url", webUrl)
         });
    }

    #endregion

    #region 病人费用大项查询

    /// <summary>
    /// 病人费用大项查询
    /// </summary>
    /// <param name="syxh"></param>
    /// <param name="lb"></param>
    /// <param name="dxmdm"></param>
    /// <param name="idm"></param>
    /// <param name="ypdm"></param>
    /// <param name="jsxh"></param>
    /// <returns></returns>
    [WebMethod]
    public byte[] QueryPatientBigCharge(string syxh, int lb, string dxmdm,
                int idm, string ypdm, int jsxh)
    {
        string dxmdmcannull = NullableSqlParamValue(dxmdm);
        string ypdmcannull = NullableSqlParamValue(ypdm);
        string jsxhcannull = NullableSqlParamValue(jsxh);
        string sqlcmd = " exec usp_bq_fymxcx " + syxh
                    + "," + lb.ToString() + "," + dxmdmcannull
                    + "," + idm.ToString() + "," + ypdmcannull
                    + "," + jsxhcannull;
        DataSet ds = _sqlhelper.ExecuteDataSet(sqlcmd);
        return DataSet2ByteArray(ds, Encoding.UTF8);
    }

    #endregion

    #region 病人费用小项查询

    /// <summary>
    /// 病人费用小项查询
    /// </summary>
    /// <param name="syxh"></param>
    /// <param name="dxmdm"></param>
    /// <param name="jsxh"></param>
    /// <param name="cxlb"></param>
    /// <returns></returns>
    [WebMethod]
    public byte[] QueryPatientSmallCharge(string syxh, string dxmdm,
                int jsxh, int cxlb)
    {
        string dxmdmcannull = NullableSqlParamValue(dxmdm);
        string jsxhcannull = NullableSqlParamValue(jsxh);
        string sqlcmd = " exec usp_bq_fymxcx_ex1 " + syxh
                    + "," + dxmdmcannull
                    + "," + jsxhcannull
                    + "," + cxlb.ToString();
        DataSet ds = _sqlhelper.ExecuteDataSet(sqlcmd);
        return DataSet2ByteArray(ds, Encoding.UTF8);
    }
    #endregion

    #region 病人费用明细查询

    /// <summary>
    /// 病人费用明细查询
    /// </summary>
    /// <param name="syxh"></param>
    /// <param name="dxmdm"></param>
    /// <param name="idm"></param>
    /// <param name="ypdm"></param>
    /// <param name="jsxh"></param>
    /// <param name="cxlb"></param>
    /// <returns></returns>
    [WebMethod]
    public byte[] QueryPatientChargeDetail(string syxh, string dxmdm,
                int idm, string ypdm, int jsxh, int cxlb)
    {
        string dxmdmcannull = NullableSqlParamValue(dxmdm);
        string ypdmcannull = NullableSqlParamValue(ypdm);
        string jsxhcannull = NullableSqlParamValue(jsxh);
        string sqlcmd = " exec usp_bq_fymxcx_ex2 " + syxh
                    + "," + dxmdmcannull + "," + idm.ToString()
                    + "," + ypdmcannull + "," + jsxhcannull
                    + "," + cxlb.ToString();
        DataSet ds = _sqlhelper.ExecuteDataSet(sqlcmd);
        return DataSet2ByteArray(ds, Encoding.UTF8);
    }
    #endregion

    #region 病人其他信息查询

    /// <summary>
    /// 病人其他信息查询
    /// </summary>
    /// <param name="syxh"></param>
    /// <returns></returns>
    [WebMethod]
    public byte[] QueryPatientOtherInfo(string syxh)
    {
        string sqlcmd = " exec usp_Emr_QueryPatientOtherInfo " + syxh;
        DataSet ds = _sqlhelper.ExecuteDataSet(sqlcmd);
        return DataSet2ByteArray(ds, Encoding.UTF8);
    }
    #endregion

    #region 病人手术信息查询(His)
    /// <summary>
    /// 病人手术信息查询(His)
    /// </summary>
    /// <param name="syxh"></param>
    /// <returns></returns>
    [WebMethod]
    public byte[] QueryPatientOperation(string syxh)
    {
        string sqlcmd = " exec usp_bq_sscx " + syxh;
        DataSet ds = _sqlhelper.ExecuteDataSet(sqlcmd);
        return DataSet2ByteArray(ds, Encoding.UTF8);
    }
    #endregion

    #region //申请单状态同步

    ///// <summary>
    ///// 申请单状态同步
    ///// </summary>
    ///// <returns></returns>
    //[WebMethod]
    //public byte[] GetChangedRequest()
    //{
    //   string sqlcmd = " exec usp_EmrDTS_GetChangedRequests";
    //   DataSet ds = _sqlhelper.ExecuteDataSet(sqlcmd);
    //   return DataSet2ByteArray(ds, Encoding.UTF8);
    //}

    #endregion

    #region 取得处方规则

    /// <summary>
    /// 取得处方规则
    /// </summary>
    /// <returns></returns>
    [WebMethod]
    public byte[] GetRecipeRules()
    {
        string sqlcmd = " exec usp_Emr_QueryRecipeRules ";
        DataSet ds = _sqlhelper.ExecuteDataSet(sqlcmd);
        return DataSet2ByteArray(ds, Encoding.UTF8);
    }

    #endregion

    #region 取得需要导入病案的相关信息

    string FindMapFeeKind(Hashtable mappings, string hisfeekind)
    {
        foreach (string key in mappings.Keys)
        {
            string mapKinds = mappings[key].ToString();
            if (mapKinds.Contains(hisfeekind))
                return key;
        }
        return string.Empty;
    }

    void GenFeeDataColumns(DataTable dtFees2)
    {
        DataColumn dcZyzfy = new DataColumn("zyzfy", typeof(double));
        dcZyzfy.Caption = "住院总费用";
        dtFees2.Columns.Add(dcZyzfy);

        DataColumn dcCwf = new DataColumn("cwf", typeof(double));
        dcZyzfy.Caption = "床位费";
        dtFees2.Columns.Add(dcCwf);

        DataColumn dcZhenlf = new DataColumn("zlf", typeof(double));
        dcZhenlf.Caption = "诊疗费";
        dtFees2.Columns.Add(dcZhenlf);

        DataColumn dcHlf = new DataColumn("hlf", typeof(double));
        dcHlf.Caption = "护理费";
        dtFees2.Columns.Add(dcHlf);

        DataColumn dcXyf = new DataColumn("xyf", typeof(double));
        dcXyf.Caption = "西药费";
        dtFees2.Columns.Add(dcXyf);

        DataColumn dcZchenyf = new DataColumn("zcyf", typeof(double));
        dcZchenyf.Caption = "中成药费";
        dtFees2.Columns.Add(dcZchenyf);

        DataColumn dcZcaoyf = new DataColumn("zcaoyf", typeof(double));
        dcZcaoyf.Caption = "中草药费";
        dtFees2.Columns.Add(dcZcaoyf);

        DataColumn dcZhilf = new DataColumn("zhilf", typeof(double));
        dcZhilf.Caption = "治疗费";
        dtFees2.Columns.Add(dcZhilf);

        DataColumn dcSsf = new DataColumn("ssf", typeof(double));
        dcSsf.Caption = "手术费";
        dtFees2.Columns.Add(dcSsf);

        DataColumn dcJcf = new DataColumn("jcf", typeof(double));
        dcJcf.Caption = "检查费";
        dtFees2.Columns.Add(dcJcf);

        DataColumn dcJyf = new DataColumn("jyf", typeof(double));
        dcJyf.Caption = "检验费";
        dtFees2.Columns.Add(dcJyf);

        DataColumn dcSxf = new DataColumn("sxf", typeof(double));
        dcSxf.Caption = "输血费";
        dtFees2.Columns.Add(dcSxf);

        DataColumn dcSyf = new DataColumn("syf", typeof(double));
        dcSyf.Caption = "输氧费";
        dtFees2.Columns.Add(dcSyf);

        DataColumn dcQtfy = new DataColumn("qtfy", typeof(double));
        dcQtfy.Caption = "其他费用";
        dtFees2.Columns.Add(dcQtfy);
    }

    void GenOpDataColumns(DataTable dtOps)
    {
        DataColumn dcSsdm = new DataColumn("ssdm", typeof(string));
        dcSsdm.Caption = "手术编码";
        dtOps.Columns.Add(dcSsdm);

        DataColumn dcSsrq = new DataColumn("ssrq", typeof(string));
        dcSsrq.Caption = "手术日期";
        dtOps.Columns.Add(dcSsrq);

        DataColumn dcSsmc = new DataColumn("ssmc", typeof(string));
        dcSsmc.Caption = "手术名称";
        dtOps.Columns.Add(dcSsmc);

        DataColumn dcYsdm = new DataColumn("ysdm", typeof(string));
        dcYsdm.Caption = "手术医师";
        dtOps.Columns.Add(dcYsdm);

        DataColumn dcYsdm1 = new DataColumn("ysdm1", typeof(string));
        dcYsdm1.Caption = "第一助手";
        dtOps.Columns.Add(dcYsdm1);

        DataColumn dcYsdm2 = new DataColumn("ysdm2", typeof(string));
        dcYsdm2.Caption = "第二助手";
        dtOps.Columns.Add(dcYsdm2);

        DataColumn dcMzdm = new DataColumn("mzdm", typeof(string));
        dcMzdm.Caption = "麻醉代码";
        dtOps.Columns.Add(dcMzdm);

        DataColumn dcMzmc = new DataColumn("mzmc", typeof(string));
        dcMzmc.Caption = "麻醉方式";
        dtOps.Columns.Add(dcMzmc);

        DataColumn dcYsdm3 = new DataColumn("ysdm3", typeof(string));
        dcYsdm3.Caption = "麻醉医师";
        dtOps.Columns.Add(dcYsdm3);
    }

    /// <summary>
    /// 需要导入到病案首页的相关信息
    /// </summary>
    /// <param name="syxh"></param>
    /// <returns></returns>
    [WebMethod]
    public byte[] ExportPatientRecords(string syxh)
    {
        DataSet dsFirstPageRecords = new DataSet("FirstPageInfo");
        string sqlExport = " exec usp_Emr_ExportPatientRecords " + syxh;
        dsFirstPageRecords = _sqlhelper.ExecuteDataSet(sqlExport);
        return DataSet2ByteArray(dsFirstPageRecords, Encoding.UTF8);
    }

    /// <summary>
    /// 需要导入病案的相关信息(His)
    /// </summary>
    /// <param name="syxh"></param>
    /// <returns></returns>
    [WebMethod]
    public byte[] QueryPatientFirstPageInfo(string syxh)
    {
        DataSet dsFirstPageInfo = new DataSet("FirstPageInfo");
        //费用信息
        string sqlfy = " exec usp_bq_fymxcx " + syxh;
        DataTable dtFees = _sqlhelper.ExecuteDataTable(sqlfy);
        dsFirstPageInfo.Tables.Add(dtFees);

        //0：指导医生,1主刀医生,2：手术一助3:手术二助,
        //4：手术三助,10：麻醉指导,11：主麻 12付麻,
        //20：器械护士,21：巡回护士,22：洗手护士,30：输血
        string sqlss = " select a.xh, a.ssdm, isnull(a.kssj, a.djrq) ssrq, a.ssmc, a.mzdm, a.mzmc "
                        + ", b.rylb, b.rydm from SS_SSDJK a left join SS_SSRYK b on a.xh=b.ssxh "
                        + " where a.syxh=" + syxh + " and a.jlzt=2 "
                        + " order by a.xh ";
        DataTable dtOps = _sqlhelper.ExecuteDataTable(sqlss);
        DataTable dtOps2 = new DataTable("SSMXK");
        GenOpDataColumns(dtOps2);
        int ssxh = -1;
        DataRow oprow = null;
        for (int i = 0; i < dtOps.Rows.Count; i++)
        {
            DataRow dr = dtOps.Rows[i];
            int currxh = int.Parse(dr["xh"].ToString());
            int rylb = -1;
            if (ssxh != currxh)
            {
                if (oprow != null) dtOps2.Rows.Add(oprow);
                oprow = dtOps2.NewRow();
                if (dr["rylb"] != DBNull.Value) rylb = int.Parse(dr["rylb"].ToString());
                oprow["ssdm"] = dr["ssdm"];
                oprow["ssrq"] = dr["ssrq"];
                oprow["ssmc"] = dr["ssmc"];
                oprow["mzdm"] = dr["mzdm"];
                oprow["mzmc"] = dr["mzmc"];
                if (rylb == 1) oprow["ysdm"] = dr["rydm"];
                if (rylb == 2) oprow["ysdm1"] = dr["rydm"];
                if (rylb == 3) oprow["ysdm2"] = dr["rydm"];
                if (rylb == 11) oprow["ysdm3"] = dr["rydm"];
            }
            else
            {
                if (rylb == 1) oprow["ysdm"] = dr["rydm"];
                if (rylb == 2) oprow["ysdm1"] = dr["rydm"];
                if (rylb == 3) oprow["ysdm2"] = dr["rydm"];
                if (rylb == 11) oprow["ysdm3"] = dr["rydm"];
            }
        }
        if (oprow != null) dtOps2.Rows.Add(oprow);
        dsFirstPageInfo.Tables.Add(dtOps2);

        return DataSet2ByteArray(dsFirstPageInfo, Encoding.UTF8);
    }
    #endregion

    #region 取得需要导出的病案信息数据集
   /// <summary>
   /// 病人病案信息导入到病案系统
   /// </summary>
   /// <param name="syxh"></param>
   /// <returns></returns>
   [WebMethod]
   public string InsertPatFirstPage(byte[] export)
   {
      IDataAccess sqlhelper = DataAccessFactory.GetSqlDataAccess("HISBADB");
     
      DataSet dsUpdate = ByteArray2DataSet(export, Encoding.UTF8);
      if (dsUpdate == null) 
         return "DataSet is null";
      try
      {
         //sqlhelper.BeginTransaction();
         //foreach (DataTable dt in dsUpdate.Tables)
         //{
         //   if (dt.Rows.Count > 0)
         //   {
         //      //string sDeleteFirstPage = string.Format(sqlDeleteFirstPage, dt.TableName, dt.Rows[0]["qybah"].ToString());
         //      //sqlhelper.ExecuteNoneQuery(sDeleteFirstPage);
         //      sqlhelper.UpdateTable(dt, dt.TableName, true);
         //   }
         //}
         //sqlhelper.CommitTransaction();
         DataTable dtSyjbk = dsUpdate.Tables["BA_SYJBK_T"];
         if (dtSyjbk.Rows.Count > 0)
         {
            if (!string.IsNullOrEmpty(dtSyjbk.Rows[0]["qybah"].ToString()))
            {
               int BaSyxh = 0;
               string sDeleteFirstPage = string.Format("delete from {0} where qybah={1}", dtSyjbk.TableName, dtSyjbk.Rows[0]["qybah"].ToString());
               sqlhelper.ExecuteNoneQuery(sDeleteFirstPage);
               sqlhelper.UpdateTable(dtSyjbk, dtSyjbk.TableName, true);

               string sGetBaSyxh = string.Format("select syxh from BA_SYJBK_T where qybah={0}", dtSyjbk.Rows[0]["qybah"].ToString());
               DataTable dtBaSyxh = sqlhelper.ExecuteDataTable(sGetBaSyxh);
               if (dtBaSyxh.Rows.Count > 0)
               {
                  BaSyxh = Convert.ToInt32(dtBaSyxh.Rows[0][0]);
                  foreach (DataTable dt in dsUpdate.Tables)
                  {
                     if (dt.Rows.Count > 0 && dt.TableName!="BA_SYJBK_T")
                     {
                        //dt.Rows[0]["syxh"] = BaSyxh;
                        foreach (DataRow dr in dt.Rows)
                        {
                            dr["syxh"] = BaSyxh;
                        }
                        string sDeleteFirstPageOtherInfo = string.Format(sqlDeleteFirstPage, dt.TableName, dt.Rows[0]["syxh"].ToString());
                        sqlhelper.ExecuteNoneQuery(sDeleteFirstPageOtherInfo);
                        sqlhelper.UpdateTable(dt, dt.TableName, true);
                     }
                  }

               }
               else
               {
                  return "F,插入病案基本表失败!";
               }

            }
         }
         return "T";
      }
      catch (Exception e)
      {
         sqlhelper.RollbackTransaction();
         return e.Message + "\r\n" + e.StackTrace + "\r\n" + e.InnerException;
      }
   }

   private const string sqlDeleteFirstPage = "delete from {0} where syxh={1}";
    /// <summary>
    /// 取得需要导出的病案信息数据集
    /// </summary>
    /// <param name="syxh"></param>
    /// <returns></returns>
    [WebMethod]
    public byte[] QueryPatFirstPageExport(string syxh)
    {
        string sqlSyjb = "select syxh,brxz,bahm,zyhm,brxm,brly,rycs,brxb,gzdw "
            + ",csny,brnl,sfzh,hyzk,zydm,sfdm,jgdm,mzdm,gjdm,dwdz,dwdh "
            + ",dwyb,hkdz,hkdh,hkyb,lxrm,lxgx,lxdz,lxdh,lxyb,ryqk,rytj,ryks "
            + ", rybq,rych,ryrq,zkqk,zkks,zkbq,zkch,zkrq,cyks,cybq,cych,cyrq "
            + ",zyts,sqts,shts,swrq,swyy,swzd,swlx,tlsj,cjry,tljg,swbh,mzbm "
            + ",mzzdmc,rybm1,rybm2,rybm3,rybm1_mc,rybm2_mc,rybm3_mc "
            + ",qzrq,qzts,zgqk,jbbm,bfjg,bfbm,qtjg,qtbm,grjg,grbm,ssbm "
            + ",zlbm,blzd,blmc,gmyw,xxdm,qjcs,cgcs,grcs,szqk,szqx,szdw "
            + ",bqqk,crbg,zlbg,xssw,cfsw,qzlx,mcfh,rcfh,cydz,qhfh,blfh,fsfh "
            + ",ctfh,sjqk,sjfh,jbxz,zlff,qjff,cyfs,sxjl,sxqk,syqk,bazl,tbsy "
            + ",zfy,cwf,xyf,zyf,jcf,zlf,fsf,ssf,hyf,hlf,sxf,syf,jsf,qjf,qtf "
            + ",qtf1,qtf2,qtf3,qtf4,qtf5,cqjc,jccs,chcx,mzys,zyys,zzys,zrys "
            + ",djys,cfwz,ssbz,yebz,sfbrbz,hy1,hy2,hy3,rhjy,sxpz,sxpz2 "
            + ",sxpz3,sxpz4,sxpz5,fbfh,kzr,jxys,sxys1,sxys2,bmy,zkys,zkhs "
            + ",dyczl,barq,rysj,cysj,czyh,srrq,jbbxkh,qtbxkh,zywz,zyjz,zyyn "
            + ",jbmc,zyzbdm,zyzbmc,zyzzdm,zyzzmc,zyzbzg,zyzzzg,blbh "
            + " from BQ_YS_SYJB"
            + " where syxh=" + syxh;

        string sqlZdqk = " select syxh,zdxh,zddm,zdmc,zgqk,memo,zxbz "
            + " from BQ_YS_BAZDQK " + " where syxh=" + syxh;

        string sqlBass = " select syxh,ssdm,ssmc,ysdm,ysdm1,ysdm2,ysdm3 "
            + ",ssrq,mzdm,mzmc,qkdj,memo "
            + " from BQ_YS_BASSK " + " where syxh=" + syxh;

        DataSet ds = _sqlhelper.ExecuteDataSet(sqlSyjb + " " + sqlZdqk + " " + sqlBass);
        ds.Tables[0].TableName = "BQ_YS_SYJB";
        ds.Tables[1].TableName = "BQ_YS_BAZDQK";
        ds.Tables[2].TableName = "BQ_YS_BASSK";

        foreach (DataTable dt in ds.Tables)
        {
            _sqlhelper.ResetTableSchema(dt, dt.TableName);
        }

        return DataSet2ByteArray(ds, Encoding.UTF8);
    }

    #endregion

    #region 更新需要导出的病案信息数据集

    /// <summary>
    /// 更新需要导出的病案信息数据集
    /// </summary>
    /// <param name="export"></param>
    [WebMethod]
    public string UpdatePatFirstPageExport(byte[] export)
    {
        DataSet dsUpdate = ByteArray2DataSet(export, Encoding.UTF8);
        if (dsUpdate == null) return "DataSet is null";
        try
        {
            foreach (DataTable dt in dsUpdate.Tables)
            {
                _sqlhelper.UpdateTable(dt, dt.TableName, true);
            }
            return "T";
        }
        catch (Exception e)
        {
            return e.Message + "\r\n" + e.StackTrace + "\r\n" + e.InnerException;
        }
    }

    #endregion

    #region 处理医嘱打印（查询和更新）

    /// <summary>
    /// 处理医嘱打印（查询和更新）
    /// </summary>
    /// <param name="syxh">His的首页序号</param>
    /// <param name="yexh"></param>
    /// <param name="zxlb"></param>
    /// <param name="czyh"></param>
    /// <param name="now"></param>
    /// <param name="czlb"></param>
    /// <param name="ksym"></param>
    /// <param name="jsym"></param>
    [WebMethod]
    public byte[] ProcessOrderPrint(int syxh, int yexh, int yzlb, int zxlb, string czyh, string now, int czlb,
                            int ksym, int jsym)
    {
        string sqlcmd = " exec usp_bq_yzdy " + syxh.ToString()
                        + " ," + yexh.ToString() + "," + yzlb.ToString() + "," + zxlb.ToString()
                        + " ,'" + czyh + "','" + now + "'," + czlb.ToString() + "," + ksym.ToString() + "," + jsym.ToString();
        DataSet dsPrint = _sqlhelper.ExecuteDataSet(sqlcmd);
        return DataSet2ByteArray(dsPrint, Encoding.UTF8);
    }

    #endregion

    #region 获取医嘱打印参数设置

    /// <summary>
    /// 获取医嘱打印参数设置
    /// </summary>
    [WebMethod]
    public byte[] GetOrderPrintSettings()
    {
        string sqlcmd = "exec usp_Emr_GetOrderPrintSettings";
        DataSet dsSettings = _sqlhelper.ExecuteDataSet(sqlcmd);
        return DataSet2ByteArray(dsSettings, Encoding.UTF8);
    }

    #endregion

    #region //取得体温单数据

    ///// <summary>
    ///// 取得体温单数据
    ///// </summary>
    ///// <param name="patId">His首页序号</param>
    ///// <returns></returns>
    //[WebMethod]
    //public byte[] GetBodyTemperatureLists(int patId)
    //{
    //   string sqlcmd = "select a.xh,a.syxh,a.patid,a.hzxm,a.clrq, "
    //               + " a.lrrq,a.dbcs,a.xbcs,a.xbl,a.tl,a.yll,a.otl,a.rl,a.tz "
    //               + " ,a.shts,a.ryts,a.xqyll,a.fqyll,a.tgyll,a.pqyll "
    //               + " ,b.tzxh,b.clsj,b.tw,b.mb,b.hx,b.xy,b.xl "
    //               + " from BQ_BRTZJLK a left join BQ_BRTZMXK b on a.xh=b.tzxh "
    //               + " where a.syxh=" + patId;
    //   DataSet dsBodyTemp = _sqlhelper.ExecuteDataSet(sqlcmd);
    //   return DataSet2ByteArray(dsBodyTemp, Encoding.UTF8);
    //}
    #endregion

    #region //获得体温单数据(使用存储过程)
    ///// <summary>
    /////  获得体温单数据(使用存储过程)
    ///// </summary>
    ///// <param name="patId">病人首页序号</param>
    ///// <param name="babyPatId">婴儿的首页序号</param>
    ///// <param name="startTime">体温单开始时间</param>
    ///// <param name="isDetail">是否为明细信息，否则为概要信息</param>
    ///// <returns></returns>
    //[WebMethod]
    //public byte[] GetBodyTemperatureData(int patId, int babyPatId, string startTime, bool isDetail)
    //{
    //   string sqlCmd = string.Format(" exec usp_bq_hs_twdinfo {0},{1},'{2}',{3}"
    //      , patId
    //      , babyPatId
    //      , startTime
    //      , isDetail ? 1 : 0);
    //   DataSet dsBodyTemp = _sqlhelper.ExecuteDataSet(sqlCmd);
    //   return DataSet2ByteArray(dsBodyTemp, Encoding.UTF8);
    //}
    #endregion

    #region 插入检查申请单的明细信息至HIS库

    /// <summary>
    /// 插入检查申请单的明细信息至HIS库
    /// </summary>
    /// <param name="checklistid"></param>
    /// <param name="hissyxh"></param>
    /// <param name="xmljcdata"></param>
    /// <param name="encodingName"></param>
    /// <returns></returns>
    [WebMethod]
    public string InsertCheckListDetail(string checklistid, byte[] xmljcdata, string encodingName)
    {
        string spRet = string.Empty;
        SqlConnection con = new SqlConnection();
        con.ConnectionString = ConfigurationManager.ConnectionStrings["HISDB"].ConnectionString;


        try
        {
            if (string.IsNullOrEmpty(checklistid))
                return "F" + "申请号小于0";
            int checkListId = Convert.ToInt32(checklistid);
            string errMsg = string.Empty;
            string sqlcmd = string.Empty;
            Encoding encoding = Encoding.GetEncoding(encodingName);
            DataSet dsCheck = ByteArray2DataSet(xmljcdata, encoding);
            if (dsCheck == null || dsCheck.Tables.Count == 0)
                return "F" + "传入的检查数据无法解析";
            //根据checklistid和hissyxh关联到ZY_BRSQD的blsqdxh，因为是检查的关系
            //所以需要用负号来对比，取得xh，这个xh关联到ZY_BRSQDMXK的xh取得sqdxh
            //，然后拼成的表里的sqdid就都是这个sqdxh，以表示是同一个申请单的明细信息
            //把根据checklistid筛选出的EMR中的检查明细信息转换成HIS的明细表的格式并插入，完成明细的接口转换
            //已经在医嘱SP中插入标本类型BBLX ,所以可以直接获得sqdxh
            _sqlhelper = new SqlDataAccess("HISDB"); //DataAccessFactory.GetSqlDataAccess("HISDB");
            DataTable tempBRSQD = _sqlhelper.ExecuteDataTable(string.Format(c_Selectmxxh, -checkListId));
            if (tempBRSQD == null || tempBRSQD.Rows.Count == 0)
                return "F" + "未能在ZY_BRSQD与ZY_BRSQDMXK中找到blsqdxh为" + checkListId.ToString() + "所对应的ZY_BRSQDMXK中的sqdxh值";
            else
            {

                con.Open();
                SqlDataAdapter da = new SqlDataAdapter(c_SelectHisDetail, con);
                DataTable hisTable = new DataTable();
                da.Fill(hisTable);
                con.Close();
                
                //DataTable hisTable = _sqlhelper.ExecuteDataTable(c_SelectHisDetail);
                //_sqlhelper.ResetTableSchema(hisTable, "ZY_BRSQDMXK");
                //hisTable.Columns["xh"].AutoIncrement = true;
                bool noException = true;
                int sqdxh = 0;
                try
                {
                    sqdxh = Convert.ToInt32(tempBRSQD.Rows[0][0].ToString());
                }
                catch (Exception e)
                {
                    spRet = "F" + "数据转换错误，原数据是：" + tempBRSQD.Rows[0][0].ToString() + "。错误信息：" + e.Message;
                    noException = false;
                }

                if (!noException)
                    return spRet;
                //拆分EMR中的数据成HIS中格式
                int zdCount = 1;
                int qtCount = 1;
                string timeString = DateTime.Now.ToString("yyyyMMddHH:mm:ss");
                DataTable checkDetail = dsCheck.Tables[0];
                string lrczyh = string.Empty;
                if (checkDetail.Rows.Count > 0)
                {
                    lrczyh = GetCellString(checkDetail.Rows[0]["lrczyh"]);
                    //根据旧接口还需要构造数据项：
                    //todo:患者姓名   性别  年龄 科室  执行科室代码 执行科室   病历号   备注
                    DataRow tempRow = checkDetail.Rows[0];
                    hisTable.Rows.Add(GetFilledRow(hisTable, sqdxh, 1, "患者姓名", string.Empty, GetCellString(tempRow["hzxm"])
                       , 0, string.Empty, 0, 1, 0, 0, string.Empty, lrczyh, timeString));
                    hisTable.Rows.Add(GetFilledRow(hisTable, sqdxh, 2, "性别", string.Empty, GetCellString(tempRow["xb"])
                       , 0, string.Empty, 0, 2, 0, 0, string.Empty, lrczyh, timeString));
                    hisTable.Rows.Add(GetFilledRow(hisTable, sqdxh, 3, "年龄", string.Empty, GetCellString(tempRow["nl"])
                       , 0, string.Empty, 0, 3, 0, 0, string.Empty, lrczyh, timeString));
                    hisTable.Rows.Add(GetFilledRow(hisTable, sqdxh, 4, "科室", string.Empty, GetCellString(tempRow["ksmc"])
                       , 0, string.Empty, 0, 4, 0, 0, string.Empty, lrczyh, timeString));
                    hisTable.Rows.Add(GetFilledRow(hisTable, sqdxh, 5, "病历号", string.Empty, GetCellString(tempRow["blh"])
                       , 0, string.Empty, 0, 5, 0, 0, string.Empty, lrczyh, timeString));
                    hisTable.Rows.Add(GetFilledRow(hisTable, sqdxh, 17, "执行科室代码", string.Empty, GetCellString(tempRow["zxksdm"])
                       , 0, string.Empty, 0, 17, 0, 0, string.Empty, lrczyh, timeString));
                    hisTable.Rows.Add(GetFilledRow(hisTable, sqdxh, 18, "执行科室", string.Empty, GetCellString(tempRow["zxksmc"])
                       , 0, string.Empty, 0, 18, 0, 0, string.Empty, lrczyh, timeString));
                    hisTable.Rows.Add(GetFilledRow(hisTable, sqdxh, 16, "条形码", string.Empty, GetCellString(tempRow["txm"])
                      , 0, string.Empty, 0, 16, 0, 0, string.Empty, lrczyh, timeString));
                    hisTable.Rows.Add(GetFilledRow(hisTable, sqdxh, 15, "申请单名称", string.Empty, GetCellString(tempRow["sqdmc"])
                    , 0, string.Empty, 0, 15, 0, 0, string.Empty, lrczyh, timeString));
                    hisTable.Rows.Add(GetFilledRow(hisTable, sqdxh, 14, "检查项目", string.Empty, GetCellString(tempRow["sfxmmc"])
                    , 0, string.Empty, 0, 14, 0, 0, string.Empty, lrczyh, timeString));
                    hisTable.Rows.Add(GetFilledRow(hisTable, sqdxh, 11, "备注", string.Empty, GetCellString(tempRow["memo"])
                       , 0, string.Empty, 0, 11, 0, 0, string.Empty, lrczyh, timeString));
                }


                //处理通用的项目
                for (int idx = 0; idx < checkDetail.Rows.Count; idx++)
                {
                    int fldm = Convert.ToInt32(checkDetail.Rows[idx][c_Colfldm]);
                    if (fldm == 1)
                    {
                        //病史资料
                        hisTable.Rows.Add(GetFilledRow(hisTable, sqdxh, 10, GetCellString(checkDetail.Rows[idx]["flmc"])
                           , string.Empty, GetCellString(checkDetail.Rows[idx]["textvalue"]), 0, string.Empty, 0, 10, 0, 0, string.Empty
                           , lrczyh, timeString));
                    }
                    else if (fldm == 3)
                    {
                        //诊断
                        hisTable.Rows.Add(GetFilledRow(hisTable, sqdxh, zdCount + 18, GetCellString(checkDetail.Rows[idx]["flmc"]) + zdCount.ToString()
                        , GetCellString(checkDetail.Rows[idx]["codevalue"]), GetCellString(checkDetail.Rows[idx]["textvalue"])
                        , 0, string.Empty, 0, zdCount + 18, 0, 0, "mzzd", lrczyh, timeString));
                        zdCount++;
                    }
                    else if (fldm == 2)
                    {
                        //检查目的
                        hisTable.Rows.Add(GetFilledRow(hisTable, sqdxh, 9, GetCellString(checkDetail.Rows[idx]["flmc"]), string.Empty
                        , GetCellString(checkDetail.Rows[idx]["textvalue"]), 0, string.Empty, 0, 9, 0, 0, string.Empty, lrczyh, timeString));
                    }
                    else if (fldm == 4)
                    {
                        //其他
                        if (Convert.ToInt32(checkDetail.Rows[idx]["valuetype"]) == 3)
                        {
                            //选择型
                            hisTable.Rows.Add(GetFilledRow(hisTable, sqdxh, qtCount + 20, GetCellString(checkDetail.Rows[idx]["textvalue"])
                            , GetCellString(checkDetail.Rows[idx]["codevalue"]), GetCellString(checkDetail.Rows[idx]["xmmc"])
                            , 0, string.Empty, 0, qtCount + 20, 0, 0, string.Empty, lrczyh, timeString));
                        }
                        else
                        {
                            //非选择型
                            hisTable.Rows.Add(GetFilledRow(hisTable, sqdxh, qtCount + 20, GetCellString(checkDetail.Rows[idx]["textvalue"])
                               , string.Empty, GetCellString(checkDetail.Rows[idx]["codevalue"])
                               , 0, string.Empty, 0, qtCount + 20, 0, 0, string.Empty, lrczyh, timeString));
                        }
                        qtCount++;
                    }
                }
                spRet = "T";

                DataTable changedTable = hisTable.GetChanges();
                if ((changedTable != null) && (changedTable.Rows.Count >0))
                {
                    InsertDataDetail(changedTable);
                }
                
            }
            return spRet;
        }
        catch (SqlException sqlEx)
        {
            spRet = "F" + "Sql错误：" + sqlEx.Message + " 堆栈:" + sqlEx.StackTrace;
        }
        catch (Exception ex)
        {
            spRet = "F" + ex.Message + " 堆栈:" + ex.StackTrace;
        }
        return spRet;
    }


    private const string sqlInsertDetail = @"insert ZY_BRSQDMXK (sqdxh,zyxh,caption,valuedm,value,zlx,dw,sjxh,taborder,dykz,jlzt,lrczyh,lrrq,memo)
                                values({0},{1},'{2}','{3}','{4}',{5},'{6}',{7},{8},{9},{10},'{11}','{12}','{13}')";



    private void InsertDataDetail(DataTable changedTable)
    {
        SqlConnection con = new SqlConnection();

        con.ConnectionString = ConfigurationManager.ConnectionStrings["HISDB"].ConnectionString;

        string insertCmd = string.Empty;
        for (int i = 0; i < changedTable.Rows.Count; i++)
        {
            DataRow row = changedTable.Rows[i];
            if (row.RowState != DataRowState.Added)
                continue;

            string insertTempCmd = string.Format(sqlInsertDetail, row["sqdxh"], row["zyxh"], row["caption"], row["valuedm"]
                    , row["value"], row["zlx"], row["dw"], row["sjxh"], row["taborder"], row["dykz"], row["jlzt"], row["lrczyh"], row["lrrq"], row["memo"]) + "\n";
            insertCmd = insertCmd + insertTempCmd;
        }
        try
        {
            con.Open();
            SqlCommand cmd = new SqlCommand(insertCmd, con);
            cmd.ExecuteNonQuery();
            con.Close();

        }
        catch (SqlException ex)
        {

            con.Close();
            LogHelper.TraceLog(ex.Message + insertCmd, TraceLevel.Error, new StackFrame(true));
            throw ex;

        }
    }



    private string GetCellString(object o)
    {
        if (o == null || o == DBNull.Value)
            return string.Empty;
        else
            return o.ToString();
    }

    public DataRow GetFilledRow(DataTable source, int sqdxh, int zyxh, string caption, string valuedm, string value
       , int zlx, string dw, int sjxh, int taborder, int dykz, int jlzt, string memo, string lrczyh, string lrrq)
    {
        if (source == null)
            return null;
        else
        {
            DataRow hisRow = source.NewRow();
            hisRow[c_Colsqdxh] = sqdxh;
            hisRow[c_Colzyxh] = zyxh;
            hisRow[c_Colcaption] = caption;
            hisRow[c_Colvaluedm] = valuedm;
            hisRow[c_Colvalue] = value;
            hisRow[c_Colzlx] = zlx;
            hisRow[c_Coldw] = dw;
            hisRow[c_Colsjxh] = sjxh;
            hisRow[c_Coltaborder] = taborder;
            hisRow[c_Coldykz] = dykz;
            hisRow[c_Coljlzt] = jlzt;
            hisRow[c_Colmemo] = memo;
            hisRow[c_Collrczyh] = lrczyh;
            hisRow[c_Collrrq] = lrrq;
            return hisRow;
        }
    }

    private const string c_Collrrq = "lrrq";
    private const string c_Collrczyh = "lrczyh";
    private const string c_Colmemo = "memo";
    private const string c_Coljlzt = "jlzt";
    private const string c_Coldykz = "dykz";
    private const string c_Coltaborder = "taborder";
    private const string c_Colsjxh = "sjxh";
    private const string c_Coldw = "dw";
    private const string c_Colzlx = "zlx";
    private const string c_Colvalue = "value";
    private const string c_Colvaluedm = "valuedm";
    private const string c_Colcaption = "caption";
    private const string c_Colzyxh = "zyxh";
    private const string c_Colsqdxh = "sqdxh";
    private const string c_Colxh = "xh";
    private const string c_Colfldm = "fldm";
    private const string c_SelectHisDetail = "select * from ZY_BRSQDMXK where 1=2";
    private const string c_Selectmxxh = "select a.xh from ZY_BRSQD a(nolock) where a.blsqdxh = {0}";

    #endregion

    #region internal functions
    DataSet EnvelopDataTable(DataTable table)
    {
        DataSet ds = null;
        if (table != null)
            if (table.DataSet == null)
            {
                ds = new DataSet();
                ds.Tables.Add(table);
            }
            else
                ds = table.DataSet;
        return ds;
    }

    /// <summary>
    /// 数据集Xml形式的字节数组
    /// </summary>
    /// <returns></returns>
    byte[] DataSet2ByteArray(DataSet dataset, Encoding encoding)
    {
        MemoryStream ms = new MemoryStream();
        StreamWriter sw = new StreamWriter(ms, encoding);
        dataset.WriteXml(sw, XmlWriteMode.WriteSchema);
        byte[] bds = new byte[ms.Length];
        ms.Seek(0, SeekOrigin.Begin);
        ms.Read(bds, 0, (int)ms.Length);
        sw.Close();
        return bds;
    }

    /// <summary>
    /// Xml形式的字节数组到数据集
    /// </summary>
    /// <param name="bytearray"></param>
    /// <param name="encoding"></param>
    /// <returns></returns>
    DataSet ByteArray2DataSet(byte[] bytearray, Encoding encoding)
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

    /// <summary>
    /// 19位日期字符串 -> 16位日期字符串
    /// </summary>
    /// <param name="dt19"></param>
    /// <returns></returns>
    string DT19ToDT16(string dt19)
    {
        if (string.IsNullOrEmpty(dt19)) return string.Empty;
        return dt19.Replace('-', ' ').Replace(" ", "");
    }

    /// <summary>
    /// 转换字符型参数值,可以为Null
    /// paramvalue=="<null>"时返回null
    /// </summary>
    /// <param name="paramvalue"></param>
    /// <returns></returns>
    string NullableSqlParamValue(string paramvalue)
    {
        if (string.Compare(paramvalue, "<null>", true) == 0)
            return "null";
        else
            return "'" + paramvalue + "'";
    }

    /// <summary>
    /// 转换整数型参数值,可以为Null
    /// paramvalue==-1时返回null
    /// </summary>
    /// <param name="paramvalue"></param>
    /// <returns></returns>
    string NullableSqlParamValue(int paramvalue)
    {
        if (paramvalue == -1)
            return "null";
        else
            return paramvalue.ToString();
    }

    /// <summary>
    /// 处理带引号的字符串
    /// </summary>
    /// <param name="source"></param>
    /// <returns></returns>
    string DealQuoteString(string source)
    {
        if (string.IsNullOrEmpty(source))
            return string.Empty;
        else
            return source.Replace("'", "''");
    }
    #endregion

    #region SQlDataConfig
    private void InitSqlDataConfig()
    {
        try
        {
            _feekindMappings = ConfigHelper.Instance.FeekindMapping;
        }
        catch
        { }

        //if (!String.IsNullOrEmpty(_sqNameString))
        //{
        //    sqlnames = _sqNameString.Split('*');
        //    _spNameGetPatients = sqlnames[0].ToString();
        //    _spNameUpdatePatient = sqlnames[1].ToString();
        //    _spNameGetChangedPatients = sqlnames[2].ToString();
        //    _risCallUrl = sqlnames[3].ToString();
        //    _feekindMappings = sqlnames[4].ToString();
        //}
        //else throw new ArgumentNullException("请检查数据库中是否存在该配置信息");
    }
    #endregion

    //#region 给门诊医生站传申请单Url接口
    ///// <summary>
    ///// 根据门诊传进来的完整url进行utf8的格式进行编码
    ///// </summary>
    ///// <param name="url">完整url路径</param>
    ///// <returns>返回完整编码之后的路径</returns>
    //[WebMethod]
    //public string EncryptUrl(string url)
    //{
    //    StringBuilder returnUrl = new StringBuilder();
    //    string[] arrayUrl = url.Split('?');
    //    returnUrl.Append(arrayUrl[0]);

    //    string paraUrl = arrayUrl[1];
    //    if (!string.IsNullOrEmpty(paraUrl))
    //    {
    //        returnUrl.Append("?");

    //        string[] subParas = paraUrl.Split('&');
    //        foreach (string para in subParas)
    //        {
    //            if (para.IndexOf('=') >= 0)
    //            {
    //                string[] p = para.Split('=');
    //                p[1] = HttpUtility.UrlEncode(p[1], Encoding.UTF8);

    //                returnUrl.Append(p[0] + "=");
    //                returnUrl.Append(p[1] + "&");
    //            }
    //        }
    //        returnUrl.Remove(returnUrl.Length - 1, 1);
    //    }
    //    return returnUrl.ToString();
    //}
    //#endregion


}

/// <summary>
/// 静态配置类
/// 用来读取所需的几个存储过程
/// </summary>
internal class ConfigHelper
{
    private static ConfigHelper _instance = new ConfigHelper();
    //private string _risCallUrl;
    private string _feekindMapping;
    //private string _reportDBMapping;
    public ConfigHelper()
    {
        Stream stream;
        StreamReader sr;
        //首页配置信息
        stream = Winning.Framework.BasicSettings.GetConfig("FeekindMapping");
        sr = new StreamReader(stream);
        _feekindMapping = sr.ReadToEnd();
    }

    public static ConfigHelper Instance
    {
        get { return _instance; }
    }

    public string FeekindMapping
    {
        get { return _feekindMapping; }
    }
}
