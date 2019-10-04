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
    public partial class ReportPathQuitMonthCompareDetail : Telerik.Reporting.Report
    {
        public ReportPathQuitMonthCompareDetail()
        {

            InitializeComponent();
            this.NeedDataSource += new EventHandler(ReportPathStatisticsDetail_NeedDataSource);

        }

        public static IDataAccess SqlHelper = DataAccessFactory.GetSqlDataAccess("EHRDB");

        void ReportPathStatisticsDetail_NeedDataSource(object sender, EventArgs e)
        {
            ReportSqlHelp sqlhelp = new ReportSqlHelp();

            String BeginTime = this.ReportParameters["BeginTime"].Value.ToString();
            String Ljdm = this.ReportParameters["Ljdm"].Value.ToString();
            String LjdmName = this.ReportParameters["LjdmName"].Value.ToString();

            this.txtLjdm.Value = LjdmName;
            this.txtBeginTime.Value = BeginTime;

            string strSql = string.Format(@"select 
case cpipp.wcsj
when isnull(cpipp.wcsj,0)  then cpipp.wcsj else cpipp.tcsj end
as 出径时间,
ip.name 患者姓名, dep.name 科室,war.name 病区 , dia.name 诊断,
us.name 床位医师,cpcp.name 路径名称,cpipp.jrsj 入径时间,
case cpipp.ljzt 
when 1 then '执行中' 
when 2 then '退出' 
when 3 then '完成'
else '未进入' end as 路径状态 
from  CP_InPathPatient cpipp
left join CP_ClinicalPath cpcp on cpcp.ljdm=cpipp.ljdm
left join InPatient ip on ip.NoOfInPat=cpipp.syxh
left join users us on us.id=ip.resident 
left join Department dep on dep.id=ip.admitdept
left join Ward war on war.id=ip.admitward
left join Diagnosis dia on  dia.markid=ip.admitdiagnosis
where len(ip.name) <> 0
and cpipp.ljzt in(3,2)
and cpcp.ljdm='{0}'
and (convert(varchar(4),CONVERT(datetime,cpipp.tcsj ),120)='{1}' 
or convert(varchar(4),CONVERT(datetime,cpipp.wcsj ),120)='{1}')
order by '出径时间' asc", Ljdm, BeginTime);


            DataTable dt = SqlHelper.ExecuteDataTable(strSql);
            //this.DataSource = dt;
            this.table1.DataSource = dt;
        }
    }
}