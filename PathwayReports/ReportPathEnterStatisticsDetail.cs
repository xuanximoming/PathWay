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
    public partial class ReportPathEnterStatisticsDetail : Telerik.Reporting.Report
    {
        public ReportPathEnterStatisticsDetail()
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
            String Ljdm = this.ReportParameters["Ljdm"].Value.ToString();
            String Ljmc = this.ReportParameters["Ljmc"].Value.ToString();

            this.txtBeginTime.Value = Begintime;
            this.txtEndTime.Value = Endtime;
            this.pathName.Value = Ljmc;

            string strSql = string.Format(@"select ip.name 患者姓名, 
case ip.sexid 
when 1 then '男' 
when 2 then '女' 
else '未知' end as 性别,
case cpcp.name
when isnull(cpcp.name,0)  then cpcp.name else '未进入' end
as 路径名称,dia.name 入院诊断,
dep.name 科室,war.name 病区 , 
us.name 床位医师,
case cpipp.jrsj
when isnull(cpipp.jrsj,0)  then cpipp.jrsj else '未进入' end
as 入径时间,
case cpipp.ljzt 
when 1 then '执行中' 
when 2 then '退出' 
when 3 then '完成'
else '未进入' end as 路径状态 
from InPatient ip
left join CP_InPathPatient  cpipp on ip.NoOfinpat=cpipp.Syxh
left join CP_ClinicalPath cpcp on cpcp.ljdm=cpipp.ljdm
left join Diagnosis dia on  dia.markid=ip.admitdiagnosis
left join Department dep on dep.id=ip.admitdept
left join Ward war on war.id=ip.admitward
left join users us on us.id=ip.resident where ip.admitdiagnosis in 
 (select cppejc.jcxm 
  from CP_PathEnterJudgeCondition  cppejc
  right join Diagnosis dia on  dia.markid=cppejc.jcxm
  where ljdm='{0}')
and ip.inwarddate >= '{1}'
and ip.inwarddate <= '{2}'", Ljdm, Begintime, Endtime);
            DataTable dt = SqlHelper.ExecuteDataTable(strSql);
            this.table1.DataSource = dt;

            
        }
    }
}