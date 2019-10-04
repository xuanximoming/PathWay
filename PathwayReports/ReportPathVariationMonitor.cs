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
    /// Summary description for ReportPathVariationMonitor.
    /// </summary>
    public partial class ReportPathVariationMonitor : Telerik.Reporting.Report
    {
        public ReportPathVariationMonitor()
        {
            InitializeComponent();

            this.DataSource = null;
            this.NeedDataSource += new EventHandler(ReportPathVariationMonitor_NeedDataSource);
        }

        private void ReportPathVariationMonitor_NeedDataSource(object sender, EventArgs e)
        {
            ReportSqlHelp sqlhelp = new ReportSqlHelp();

            String BeginTime = this.ReportParameters["BeginTime"].Value.ToString();
            String EndTime = this.ReportParameters["EndTime"].Value.ToString();
            String Ljdm = this.ReportParameters["Ljdm"].Value.ToString();
            String LjdmName = this.ReportParameters["LjdmName"].Value.ToString();

            txtBeginTime.Value = BeginTime;
            txtEndTime.Value = EndTime;
            txtLjdm.Value = LjdmName;

            SqlParameter paramBegintime = new SqlParameter("@begindate", SqlDbType.NVarChar, 100);
            paramBegintime.Value = BeginTime;

            SqlParameter paramEndtime = new SqlParameter("@enddate", SqlDbType.NVarChar, 100);
            paramEndtime.Value = EndTime;

            SqlParameter paramLjdm = new SqlParameter("@Ljdm", SqlDbType.NVarChar, 100);
            paramLjdm.Value = Ljdm;

            SqlParameter[] sqlparams = new SqlParameter[] { paramBegintime, paramEndtime, paramLjdm};
            DataTable dt = sqlhelp.GetTableByPorc("usp_CP_RptPathVariationMonitor", sqlparams);

            this.tablePathVariationMonitor.DataSource = dt;

        }
    }
}