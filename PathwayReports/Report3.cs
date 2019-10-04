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
    using YidanSoft.Core;

    /// <summary>
    /// Summary description for Report3.
    /// </summary>
    public partial class Report3 : Telerik.Reporting.Report
    {
        public Report3()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            
            this.NeedDataSource += new EventHandler(Report3_NeedDataSource);

        }

        public static IDataAccess SqlHelper = DataAccessFactory.GetSqlDataAccess("EHRDB");

        void Report3_NeedDataSource(object sender, EventArgs e)
        {
            ReportSqlHelp sqlhelp = new ReportSqlHelp();
            //查询参数
            String Begintime = this.ReportParameters["BeginTime"].Value.ToString();
            String Endtime = this.ReportParameters["EndTime"].Value.ToString();
            String Ljdm = this.ReportParameters["Ljdm"].Value.ToString();
            String Bymc = this.ReportParameters["Bymc"].Value.ToString();
            //标头参数绑定
            this.txttile.Value = Bymc;
            this.textbtime.Value = Begintime;
            this.txtendtime.Value = Endtime;

            string strSql = string.Format(@"select ip.name 姓名,
cpcp.name 路径,
dia.name 诊断,
us.name 床位医师,
dep.name 科室,war.name 病区 , 
cpvr.bynr 变异内容,
cpvr.byyy 变异原因,
CONVERT(varchar,cast(cpvr.bysj as datetime ),120 ) as 变异时间,
cppd.ljmc 变异路径节点
from CP_VariantRecords cpvr
right join InPatient ip on ip.NoOfInPat=cpvr.syxh
right join CP_ClinicalPath cpcp on cpcp.ljdm=cpvr.ljdm
left join Diagnosis dia on  dia.markid=ip.admitdiagnosis
left join CP_PathDetail cppd on cppd.pahtdetailid=cpvr.pahtdetailid
left join users us on us.id=ip.resident 
left join Department dep on dep.id=ip.admitdept
left join Ward war on war.id=ip.admitward
where cpvr.ljdm='{0}'
and cpvr.byyy='{1}'
and CONVERT(varchar,cast(cpvr.bysj as datetime ),120 ) >= '{2}'
and CONVERT(varchar,cast(cpvr.bysj as datetime ),120 ) <= '{3}'", Ljdm, Bymc, Begintime, Endtime);//Ljmc, Bymc, Begintime, Endtime

            DataTable dt = SqlHelper.ExecuteDataTable(strSql);
            this.table1.DataSource = dt;
        }
    }
}