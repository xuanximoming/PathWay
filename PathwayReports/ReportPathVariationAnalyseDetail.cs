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
    /// Summary description for Report1.
    /// </summary>
    public partial class ReportPathVariationAnalyseDetail : Telerik.Reporting.Report
    {
        public ReportPathVariationAnalyseDetail()
        {

            InitializeComponent();
            this.NeedDataSource += new EventHandler(ReportPathStatisticsDetail_NeedDataSource);

        }

        public static IDataAccess SqlHelper = DataAccessFactory.GetSqlDataAccess("EHRDB");

        void ReportPathStatisticsDetail_NeedDataSource(object sender, EventArgs e)
        {
            ReportSqlHelp sqlhelp = new ReportSqlHelp();

            String Begintime = this.ReportParameters["BeginTime"].Value.ToString();
            String Endtime = this.ReportParameters["EndTime"].Value.ToString();
            String Bymc = this.ReportParameters["Bymc"].Value.ToString();
            String Bydm = this.ReportParameters["Bydm"].Value.ToString();

            this.txtLjdm.Value = Bymc;
            this.txtBeginTime.Value = Begintime;
            this.txtEndTime.Value = Endtime;

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
where cpvr.bydm='{0}'
and CONVERT(varchar,cast(cpvr.bysj as datetime ),120 ) >= '{1}'
and CONVERT(varchar,cast(cpvr.bysj as datetime ),120 ) <= '{2}'", Bydm, Begintime, Endtime);
            DataTable dt = SqlHelper.ExecuteDataTable(strSql);
            this.table1.DataSource = dt;
        }
    }
}