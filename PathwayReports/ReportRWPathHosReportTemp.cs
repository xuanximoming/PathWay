namespace YidanEHRReport
{
    using System;
    using System.Collections.Generic;
    using System.Data;
    using Yidansoft.Service;

    /// <summary>
    /// Summary description for ReportTest.
    /// </summary>
    public partial class ReportRWPathHosReportTemp : Telerik.Reporting.Report
    {
        //
        public String m_sSyxh = "";
        public String m_sLjdm = "";

        public DataTable m_table = null;
        ReportSqlHelp m_sqlHelp = null;
        List<RW_PathSummaryOrder> m_pathSummaryOrderList = null;
        //
        public ReportRWPathHosReportTemp()
        {
            try
            {
                InitializeComponent();
                InitializeControl();
                //this.subReportDetail.Parameters.
                //this.subReportDetail.ReportSource.DataSource = .RenderBegin += new Telerik.ReportViewer.Silverlight.RenderBeginEventHandler(ReportViewer1_RenderBegin);
                this.NeedDataSource += new EventHandler(ReportTest_NeedDataSource);


            }
            catch (Exception)
            {

                throw;
            }
        }
        //
        private void InitializeControl()
        {
            try
            {
                m_sqlHelp = new ReportSqlHelp();
                m_pathSummaryOrderList = new List<RW_PathSummaryOrder>();

            }
            catch (Exception)
            {

                throw;
            }
        }
        //清除数据
        private void ClearControl()
        {
            try
            {

            }
            catch (Exception)
            {

                throw;
            }
        }
        //

        void ReportTest_NeedDataSource(object sender, EventArgs e)
        {
            GetDataSource();
            //subReportDetail.
        }

        //设置数据源
        private void GetDataSource()
        {
            try
            {
                m_sSyxh = this.ReportParameters["Syxh"].Value.ToString() == "" ? "0" : this.ReportParameters["Syxh"].Value.ToString();
                m_sLjdm = this.ReportParameters["Ljdm"].Value.ToString() == "" ? "0" : this.ReportParameters["Ljdm"].Value.ToString();
                if (m_sLjdm != null && m_sSyxh != null)
                {
                    //ClearControl();
                    //获取临床路径包含的节点信息
                    GetClinicalPathDetailInfo();
                    //int seq = 0;
                    ////*******************************************
                    ////填写报表各部分数据信息
                    ////*******************************************
                    ////表头信息及适用对象信息的填写
                    //GetClinicalPathName();
                    ////病人基本信息填写
                    //GetInpatientInfo();
                    ////配置临床路径单个节点报表
                    //SingleReportPageConfig(seq);
                }
                //*******************************************
            }
            catch (Exception)
            {
                throw;
            }
        }

        //获取路径明细信息
        private void GetClinicalPathDetailInfo()
        {
            try
            {
                DataTable dt = m_sqlHelp.GetClinicalPathDetailInfo(this.m_sLjdm, "");
                if (dt != null)
                {
                    this.DataSource = dt;
                    m_table = dt;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        private int index = 0;
        private void subReport1_ItemDataBinding(object sender, EventArgs e)
        {
            //ReportSubRWPathHosReport ss = (ReportSubRWPathHosReport)sender;
            ReportSubRWPathHosReport ss = (ReportSubRWPathHosReport)subReport1.ReportSource;
            ss.ReportParameters["Syxh"].Value = m_sSyxh;
            ss.ReportParameters["Ljdm"].Value = m_sLjdm;
            //ss.ReportParameters["DetailID"].Value = nameDataTextBox.Value;

            ss.ReportParameters["DetailID"].Value = m_table.Rows[index]["PahtDetailID"].ToString();
            index++;

            //ss.ReportParameters[]
            //ss.m_sSyxh = m_sSyxh;
            //ss.m_sLjdm = m_sLjdm;
            //ss.m_sDetailID = textBox3.Value; 

        }

        private void detail_Disposed(object sender, EventArgs e)
        {
            string s = nameDataTextBox.Value;
        }


        /*
//配置临床路径单个节点报表,seq为路径节点顺序号
private void SingleReportPageConfig(int seq)
{
    try
    {
        //节点信息
        GetTimeInfo(seq);
        //填写主要诊疗信息
        GetDiagnosisInfo(seq);
        //填写重点医嘱信息
        GetOrdersInfo(seq);
        //填写非药物治疗信息
        GetNonDrugTherapyInfo(seq);
        //填写主要护理工作
        GetNursingInfo(seq);
        //填写病情变异记录
        GetVariationInfo(seq);
        //填写护士信息
        GetNurseInfo(seq);
        //填写医生信息
        GetDoctorInfo(seq);
    }
    catch (Exception)
    {                
        throw;
    }
}
/// <summary>
/// 根据Syxh获取患者信息
/// </summary>
/// <param name="Syxh"></param>
private void GetInpatientInfo()
{

    try
    {
        string hosName = m_sqlHelp.GetHospitalName();
        SqlParameter querykindparam = new SqlParameter("@querykind", SqlDbType.Int, 12);
        querykindparam.Value = 3;
        SqlParameter para_syxh = new SqlParameter("@Syxh", SqlDbType.VarChar, 12);
        para_syxh.Value = this.m_sSyxh;
        SqlParameter pagesizem = new SqlParameter("@PageSize", SqlDbType.Int);
        pagesizem.Value = 1;
        SqlParameter currentpagem = new SqlParameter("@CurrentPage", SqlDbType.Int);
        currentpagem.Value = 0;
        SqlParameter[] sqlparams = new SqlParameter[] { querykindparam, para_syxh, pagesizem, currentpagem };
        DataTable dt = m_sqlHelp.GetTableByPorc("usp_CP_InpatientList", sqlparams);
        if (dt.Rows.Count > 0)
        {
            this.txtHzxm.Value = dt.Rows[0]["Hzxm"].ToString();
            this.txtNl.Value = dt.Rows[0]["Xsnl"].ToString();
            this.txtXb.Value = dt.Rows[0]["Brxb"].ToString();

            this.txtMzh.Value = dt.Rows[0]["Hissyxh"].ToString();
            this.txtZyh.Value = dt.Rows[0]["Zyhm"].ToString();
            this.txtFbsj.Value = dt.Rows[0]["Cycw"].ToString();

            this.txtZyrq.Value = dt.Rows[0]["Ryrq"].ToString();
            this.txtCyrq.Value = dt.Rows[0]["Cyrq"].ToString();
            this.txtBzzy.Value = dt.Rows[0]["Brzt"].ToString() + "天";
            this.txtSjzy.Value = dt.Rows[0]["Brzt"].ToString() + "天";
        }
    }
    catch (Exception)
    {                
        throw;
    }
}
//获取路径名称CP_ClinicalPath,获取适用对象信息
private void GetClinicalPathName()
{
    try
    {
        //表头
        txtTitle.Value = m_sqlHelp.GetClinicalPathName(m_sLjdm) + "住院表单";
        //适用对象
        txtSydx.Value = "";
    }
    catch (Exception)
    {                
        throw;
    }
}
//填写节点时间信息
private void GetTimeInfo(int seq)
{
    try
    {
        //查找节点执行时间
        DateTime date = System.DateTime.Today;
        string sdate = date.ToString("yyyy年MM月dd日");

        //查找节点名称
        string mc = this.m_listMc[seq].ToString();
        string sinfo = sdate +
            //Environment.NewLine + //\r\n
            "（" + mc + "）";
        this.txtTime.Value = sinfo;
    }
    catch (Exception)
    {                
        throw;
    }
}
//填写主要诊断治疗信息:CP_DiagNurExecRecord:lbxh = 1
private void GetDiagnosisInfo(int seq)
{
    try
    {
        string pathDetailId = this.m_listId[seq].ToString();
        List<string> list = new List<string>();
        m_sqlHelp.GetDiagNurExecRecordInfo(this.m_sSyxh, this.m_sLjdm,
            pathDetailId, 1, ref list);
        if (list.Count > 0)
        {
            string sinfo = "";//Environment.NewLine
            for (int i = 0; i < list.Count; i++)
            {
                sinfo = sinfo + "□" + list[i].ToString() + Environment.NewLine;
            }
            this.txtZd.Value = sinfo;
        }                
    }
    catch (Exception)
    {                
        throw;
    }
}
//填写主要护理工作:CP_DiagNurExecRecord:lbxh = 2
private void GetNursingInfo(int seq)
{
    try
    {

        }
    }
    catch (Exception)
    {
        throw;
    }
}
//填写非药物治疗信息:CP_DiagNurExecRecord:lbxh = 3
private void GetNonDrugTherapyInfo(int seq)
{
    try
    {
        string pathDetailId = this.m_listId[seq].ToString();
        List<string> list = new List<string>();
        m_sqlHelp.GetDiagNurExecRecordInfo(this.m_sSyxh, this.m_sLjdm,
            pathDetailId, 3, ref list);
        if (list.Count > 0)
        {
            string sinfo = "";//Environment.NewLine
            for (int i = 0; i < list.Count; i++)
            {
                sinfo = sinfo + "□" + list[i].ToString() + Environment.NewLine;
            }
            this.txtZl.Value = sinfo;
        }                
    }
    catch (Exception)
    {
        throw;
    }
}
//填写重点医嘱信息:CP_LongOrder,CP_TempOrder
private void GetOrdersInfo(int seq)
{
    try
    {

    }
    catch (Exception)
    {                
        throw;
    }
}
//填写病情变异记录:CP_VariantRecords.
//Syxh,Ljdm,PahtDetailID,Bynr,Bysj.
private void GetVariationInfo(int seq)
{
    try
    {
    }
    catch (Exception)
    {                
        throw;
    }
}
//填写护士信息
private void GetNurseInfo(int seq)
{
    try
    {
    }
    catch (Exception)
    {                
        throw;
    }
}
//谈写医生信息
private void GetDoctorInfo(int seq)
{
    try
    {
        string sinfo = "";
        this.txtDoctor.Value = sinfo;
    }
    catch (Exception)
    {                
        throw;
    }
}
//获取路径医嘱信息
private void GetOrderData()
{
    try
    {
        string ljxh = m_sqlHelp.GetInPathPatientId(m_sSyxh, m_sLjdm);

        SqlParameter[] parameters = new SqlParameter[] 
            {
                new SqlParameter("@Syxh",this.m_sSyxh),
                new SqlParameter("@Ljxh",ljxh)
            };
        DataSet ds = YidanSoft.Core.DataAccessFactory.DefaultDataAccess.ExecuteDataSet("usp_CP_RWPathSummary", parameters, CommandType.StoredProcedure);
        if (ds.Tables.Count > 0)            
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                /*
                RW_PathSummaryOrder pathOrder = new RW_PathSummaryOrder();
                pathOrder.Yzbz = "";
                pathOrder.Xmlb = row["Xmlb"].ToString();
                pathOrder.Ypdm = "";
                pathOrder.Ypmc = "";
                pathOrder.Yznr = row["Yznr"].ToString();
                pathOrder.ActivityId = row["ActivityId"].ToString();
                pathOrder.ActivityChildID = row["ActivityChildID"].ToString();
                pathOrder.Yzzt = row["Yzzt"].ToString();
                m_pathSummaryOrderList.Add(pathOrder);
                       
                m_listCqYz.Add(row["Yznr"].ToString());
                m_listCqYzId.Add(row["ActivityId"].ToString());
            }
    }
    catch (Exception)
    {
        throw;
    }
}
}
*/
    }
}