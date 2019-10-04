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
    /// Summary description for ReportPathQuitMonthCompare.
    /// </summary>
    public partial class ReportPathQuitMonthCompare : Telerik.Reporting.Report
    {
        public ReportPathQuitMonthCompare()
        {
            InitializeComponent();

            this.DataSource = null;
            this.NeedDataSource += new EventHandler(ReportPathQuitMonthCompare_NeedDataSource);
        }

        private void ReportPathQuitMonthCompare_NeedDataSource(object sender, EventArgs e)
        {
            ReportSqlHelp sqlhelp = new ReportSqlHelp();

            String BeginTime = this.ReportParameters["BeginTime"].Value.ToString();
            String EndTime = this.ReportParameters["EndTime"].Value.ToString();
            String Ljdm = this.ReportParameters["Ljdm"].Value.ToString();
            String Dept = this.ReportParameters["Dept"].Value.ToString();
            String LjdmName = this.ReportParameters["LjdmName"].Value.ToString();
            String DeptName = this.ReportParameters["DeptName"].Value.ToString();

            txtBeginTime.Value = BeginTime;
            txtEndTime.Value = EndTime;
            txtDept.Value = DeptName;
            txtLjdm.Value = LjdmName;

            SqlParameter paramBegintime = new SqlParameter("@begindate", SqlDbType.NVarChar, 100);
            paramBegintime.Value = BeginTime;

            SqlParameter paramEndtime = new SqlParameter("@enddate", SqlDbType.NVarChar, 100);
            paramEndtime.Value = EndTime;

            SqlParameter paramLjdm = new SqlParameter("@Ljdm", SqlDbType.NVarChar, 100);
            paramLjdm.Value = Ljdm;

            SqlParameter paramdept = new SqlParameter("@dept", SqlDbType.NVarChar, 12);
            paramdept.Value = Dept;

            SqlParameter[] sqlparams = new SqlParameter[] { paramBegintime, paramEndtime, paramLjdm, paramdept };
            DataTable dt = sqlhelp.GetTableByPorc("usp_CP_RptPathQuitMonthCompare", sqlparams);

            this.tablePathQuitMonthCompare.DataSource = dt;

        }
    }
}