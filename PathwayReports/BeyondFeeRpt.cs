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
    /// Summary description for BeyondFeeRpt.
    /// </summary>
    public partial class BeyondFeeRpt : Telerik.Reporting.Report
    {
        public BeyondFeeRpt()
        {
            InitializeComponent();
            //加载报表数据源
            this.NeedDataSource += new EventHandler(BeyondFeeRpt_NeedDataSource);
        }

        void BeyondFeeRpt_NeedDataSource(object sender, EventArgs e)
        {
            ReportSqlHelp sqlhelp = new ReportSqlHelp();

            //报表参数变量初始化
            string BeginTime = this.ReportParameters["BegTime"].Value.ToString();
            string EndTime = this.ReportParameters["EndTime"].Value.ToString();
          
            //报表标题textbox初始化
            txtBeginTime.Value = BeginTime;
            txtEndTime.Value = EndTime;

            //创建并初始化存取过程变量参数

            //SqlParameter paramBegintime = new SqlParameter("@StartDate", SqlDbType.NVarChar, 19);
            //paramBegintime.Value = BeginTime;

            //SqlParameter paramEndtime = new SqlParameter("@EndDate", SqlDbType.NVarChar, 19);
            //paramEndtime.Value = EndTime;


            //调用存取过程返回一个DataTable，并绑定报表的表格控件数据源
            //SqlParameter[] sqlparams = new SqlParameter[] { paramBegintime, paramEndtime };
            string sql = "SELECT t1.Id,t1.Syxh,t2.Name,t1.Ljdm,t3.Name Ljmc,t1.Cwys,t4.Name Ysxm,t1.Jrsj,t1.Tcsj,";
            sql+= " t1.Wcsj,t1.Ljts,t1.Ljzt,t3.Jcfy,t5.Xmje,(t5.Xmje-t3.Jcfy) Ccfy ";
		    sql+= " FROM CP_InPathPatient t1 LEFT JOIN InPatient t2 ON t1.Syxh=t2.NoOfInpat ";
			sql+= " LEFT JOIN CP_ClinicalPath t3 ON t1.Ljdm=t3.Ljdm ";
			sql+= " LEFT JOIN Users t4 ON t1.Cwys=t4.ID ";
			sql+= " LEFT JOIN CP_InPatientFee t5 ON t1.Syxh=t5.Syxh ";
            sql += " WHERE  (t5.Xmje-t3.Jcfy)>0 and t1.Ljzt=1 and t5.dxmmc='总计'";
            DataTable dt = sqlhelp.GetTableBySQL(sql);
            this.table1.DataSource = dt;
        }
    }
}