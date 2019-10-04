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
    /// Summary description for RgvForceToPath.
    /// </summary>
    public partial class RgvForceToPath : Telerik.Reporting.Report
    {
        //add by luff 20121019
        public RgvForceToPath()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            //加载报表数据源
            this.NeedDataSource += new EventHandler(RgvForceToPath_NeedDataSource);


        }

        void RgvForceToPath_NeedDataSource(object sender, EventArgs e)
        {
            ReportSqlHelp sqlhelp = new ReportSqlHelp();

            //报表参数变量初始化
            string BeginTime = this.ReportParameters["BegTime"].Value.ToString();
            string EndTime = this.ReportParameters["EndTime"].Value.ToString();

            //报表标题textbox初始化
            txtBeginTime.Value = BeginTime;
            txtEndTime.Value = EndTime;

            //创建并初始化存取过程变量参数

            SqlParameter paramBegintime = new SqlParameter("@StartDate", SqlDbType.NVarChar, 19);
            paramBegintime.Value = BeginTime;

            SqlParameter paramEndtime = new SqlParameter("@EndDate", SqlDbType.NVarChar, 19);
            paramEndtime.Value = EndTime;


            //调用存取过程返回一个DataTable，并绑定报表的表格控件数据源
            SqlParameter[] sqlparams = new SqlParameter[] { paramBegintime, paramEndtime };
            DataTable dt = sqlhelp.GetTableByPorc("usp_CP_ForceToPath", sqlparams);

            this.table1.DataSource = dt;
        }
    }
}