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
    /// Summary description for ReportPathEnForceTempOrder.
    /// </summary>
    public partial class ReportPathEnForceTempOrder : Telerik.Reporting.Report
    {
        public ReportPathEnForceTempOrder()
        {           
            InitializeComponent();

            this.DataSource = null;
            this.NeedDataSource += new EventHandler(ReportPathEnForceTempOrder_NeedDataSource);
        }

        private void ReportPathEnForceTempOrder_NeedDataSource(object sender, EventArgs e)
        {
            ReportSqlHelp sqlhelp = new ReportSqlHelp();

            String Hzxm = this.ReportParameters["Hzxm"].Value.ToString();
            String Bed = this.ReportParameters["Bed"].Value.ToString();
            String CyksName = this.ReportParameters["CyksName"].Value.ToString();
            String Zyhm = this.ReportParameters["Zyhm"].Value.ToString();
            String Syxh = this.ReportParameters["Syxh"].Value.ToString();
            String Ljxh = this.ReportParameters["Ljxh"].Value.ToString();

            String Operator = "TempOrder";

            txtHzxm.Value = Hzxm;
            txtBed.Value = Bed;
            txtCyksName.Value = CyksName;
            txtZyhm.Value = Zyhm;

            SqlParameter paramSyxh = new SqlParameter("@Syxh", SqlDbType.NVarChar, 100);
            paramSyxh.Value = Syxh;

            SqlParameter paramLjxh = new SqlParameter("@Ljxh", SqlDbType.NVarChar, 100);
            paramLjxh.Value = Ljxh;

            SqlParameter paramOperator = new SqlParameter("@Operator", SqlDbType.NVarChar, 100);
            paramOperator.Value = Operator;

            SqlParameter[] sqlparams = new SqlParameter[] { paramSyxh, paramLjxh, paramOperator };
            DataTable dt = sqlhelp.GetTableByPorc("usp_CP_RWPathEnForcePrint", sqlparams);

            this.tablePathEnForceTempOrder.DataSource = dt;

            this.reportNameTextBox.Value = sqlhelp.GetHospitalName();

        }
    }
}