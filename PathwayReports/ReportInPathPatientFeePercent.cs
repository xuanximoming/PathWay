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
    /// Summary description for ReportInPathPatientFeePercent.
    /// </summary>
    public partial class ReportInPathPatientFeePercent : Telerik.Reporting.Report
    {
        public ReportInPathPatientFeePercent()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            this.NeedDataSource += new EventHandler(ReportInPathPatientFeePercent_NeedDataSource);
        }

       private void ReportInPathPatientFeePercent_NeedDataSource(object sender, EventArgs e)
        {
            ReportSqlHelp sqlhelp = new ReportSqlHelp();
            String BeginTime = this.ReportParameters["BeginTime"].Value.ToString();//开始时间
            String EndTime = this.ReportParameters["EndTime"].Value.ToString();//结束时间
            String Ljzt = this.ReportParameters["Ljzt"].Value.ToString();//路径状态
            String Ljdm = this.ReportParameters["Ljdm"].Value.ToString();//路径代码
            txtBeginTime.Value = BeginTime;
            txtEndTime.Value = EndTime;
            switch (Ljzt)
            {
                case "1":
                    txtLjzt.Value = "在径";
                    break;
                case "2":
                    txtLjzt.Value = "退出";
                    break;
                case "3":
                    txtLjzt.Value = "完成";
                    break;
                default:
                    txtLjzt.Value = "全部";
                    break;
            }
            txtLjdm.Value = Ljdm;

            SqlParameter paramBeginTime = new SqlParameter("@begindate", SqlDbType.NVarChar, 100);
            paramBeginTime.Value = BeginTime;

            SqlParameter paramEndTime = new SqlParameter("@enddate", SqlDbType.NVarChar, 100);
            paramEndTime.Value = EndTime;

            SqlParameter paramLjzt = new SqlParameter("@Ljzt", SqlDbType.NVarChar, 12);
            paramLjzt.Value = Ljzt;

            SqlParameter paramLjdm = new SqlParameter("@Ljdm", SqlDbType.NVarChar, 100);
            paramLjdm.Value = Ljdm;

            SqlParameter[] sqlparams = new SqlParameter[] { paramBeginTime, paramEndTime, paramLjzt, paramLjdm };
            DataTable dt = sqlhelp.GetTableByPorc("usp_CP_InPathPatientFeePercent", sqlparams);
            this.tableInPathPatientFeePercent.DataSource = dt;
        }
    }
}