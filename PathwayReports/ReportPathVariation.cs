namespace YidanEHRReport
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data.SqlClient;
    using System.Data;

    /// <summary>
    /// Summary description for ReportPathVariation.
    /// </summary>
    public partial class ReportPathVariation : Telerik.Reporting.Report
    {
        public ReportPathVariation()
        {

            InitializeComponent();
            this.NeedDataSource += new EventHandler(ReportPathVariation_NeedDataSource);
        }

        void ReportPathVariation_NeedDataSource(object sender, EventArgs e)
        {
            ReportSqlHelp sqlhelp = new ReportSqlHelp();

            DateTime BeginTime = (DateTime)this.ReportParameters["BeginTime"].Value;
            DateTime EndTime = (DateTime)this.ReportParameters["EndTime"].Value;
            String Ljdm = this.ReportParameters["Ljdm"].Value.ToString();
            String Dept = this.ReportParameters["Dept"].Value.ToString();

            String LjdmName = this.ReportParameters["LjdmName"].Value.ToString();
            String DeptName = this.ReportParameters["DeptName"].Value.ToString();

            txtBeginTime.Value = BeginTime.ToString("yyyy-MM-dd");
            txtEndTime.Value = EndTime.ToString("yyyy-MM-dd");
            txtDept.Value = DeptName;
            txtLjdm.Value = LjdmName;


            SqlParameter paramBegintime = new SqlParameter("@begindate", SqlDbType.NVarChar, 40);
            paramBegintime.Value = BeginTime.ToString("yyyy-MM-dd");

            SqlParameter paramEndtime = new SqlParameter("@enddate", SqlDbType.NVarChar, 40);
            paramEndtime.Value = EndTime.ToString("yyyy-MM-dd");

            SqlParameter paramLjdm = new SqlParameter("@Ljdm", SqlDbType.NVarChar, 40);
            paramLjdm.Value = Ljdm;

            SqlParameter paramdept = new SqlParameter("@dept", SqlDbType.NVarChar, 40);
            paramdept.Value = Dept;

            SqlParameter[] sqlparams = new SqlParameter[] { paramBegintime, paramEndtime, paramLjdm, paramdept };
            DataTable dt = sqlhelp.GetTableByPorc("usp_CP_RptPathVariation", sqlparams);

            this.DataSource = dt;

            this.tablePathVariation.DataSource = dt;
        }
    }
}