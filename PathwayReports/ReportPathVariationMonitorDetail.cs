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
    public partial class ReportPathVariationMonitorDetail : Telerik.Reporting.Report
    {
        public ReportPathVariationMonitorDetail()
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
            String Jdmc = this.ReportParameters["Jdmc"].Value.ToString();
            String Ljmc = this.ReportParameters["Ljmc"].Value.ToString();

            this.txtLjdm.Value = Jdmc;
            this.txtBeginTime.Value = Begintime;
            this.txtEndTime.Value = Endtime;
            this.pathName.Value = Ljmc;

            string strSql = string.Format(@"(select distinct '����' �ڵ�״̬, cpvr.syxh ��ҳ���,ip.name ��������, dep.name ����,war.name ���� , dia.name ���,
us.name ��λҽʦ, cpcp.name ·������,   cppd.ljmc  �ڵ�
from CP_VariantRecords cpvr
right join InPatient ip on ip.NoOfInPat=cpvr.syxh
left join users us on us.id=ip.resident 
left join Department dep on dep.id=ip.admitdept
left join Ward war on war.id=ip.admitward
left join Diagnosis dia on  dia.markid=ip.admitdiagnosis
left join CP_ClinicalPath cpcp on cpcp.ljdm=cpvr.ljdm
left join CP_PathDetail cppd on cppd.pahtdetailid=cpvr.pahtdetailid
where cpcp.ljdm='{0}' 
and convert(char(10),cast(cpvr.Bysj as datetime),120) >='{1}'
and convert(char(10),cast(cpvr.Bysj as datetime),120) <='{2}'
and cppd.ljmc='{3}'
union  
select  'ִ��' �ڵ�״̬,syxh ��ҳ��� , ip.name ��������, dep.name ����,war.name ���� , dia.name ���,
us.name ��λҽʦ, cpcp.name ·������,     activityname  �ڵ�
from CP_InPatientPathExeDetail  cpipped
right join InPatient ip on ip.NoOfInPat=cpipped.syxh
left join users us on us.id=ip.resident 
left join Department dep on dep.id=ip.admitdept
left join Ward war on war.id=ip.admitward
left join Diagnosis dia on  dia.markid=ip.admitdiagnosis
left join CP_ClinicalPath cpcp on cpcp.ljdm=cpipped.ljdm
where cpcp.ljdm='{0}'
and Jrsj >='{1}'
and Jrsj <='{2}'
and activityname='{3}')order by �ڵ�״̬ desc", Ljdm, Begintime, Endtime, Jdmc);
            DataTable dt = SqlHelper.ExecuteDataTable(strSql);
            this.table1.DataSource = dt;

            
        }
    }
}