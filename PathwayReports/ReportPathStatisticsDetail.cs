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
    public partial class ReportPathStatisticsDetail : Telerik.Reporting.Report
    {
        public ReportPathStatisticsDetail()
        {

            InitializeComponent();
            this.NeedDataSource += new EventHandler(ReportPathStatisticsDetail_NeedDataSource);

        }

        public static IDataAccess SqlHelper = DataAccessFactory.GetSqlDataAccess("EHRDB");

        void ReportPathStatisticsDetail_NeedDataSource(object sender, EventArgs e)
        {
            ReportSqlHelp sqlhelp = new ReportSqlHelp();

            String BeginTime = this.ReportParameters["BeginTime"].Value.ToString();
            String EndTime = this.ReportParameters["EndTime"].Value.ToString();
            String Ljdm = this.ReportParameters["Ljdm"].Value.ToString();
            String LjdmName = this.ReportParameters["LjdmName"].Value.ToString();

            this.txtLjdm.Value = LjdmName;
            this.txtBeginTime.Value = BeginTime;
            this.txtEndTime.Value = EndTime;

            string strSql = string.Format(@"select ip.name ��������, 
case ip.sexid 
when 1 then '��' 
when 2 then 'Ů' 
else 'δ֪' end as �Ա�,
case cpcp.name
when isnull(cpcp.name,0)  then cpcp.name else 'δ����' end
as ·������,dia.name ��Ժ���,
dep.name ����,war.name ���� , 
us.name ��λҽʦ,
case cpipp.jrsj
when isnull(cpipp.jrsj,0)  then cpipp.jrsj else 'δ����' end
as �뾶ʱ��,
case cpipp.ljzt 
when 1 then 'ִ����' 
when 2 then '�˳�' 
when 3 then '���'
else 'δ����' end as ·��״̬ 
from InPatient ip
left join CP_InPathPatient  cpipp on ip.NoOfinpat=cpipp.Syxh
left join CP_ClinicalPath cpcp on cpcp.ljdm=cpipp.ljdm
left join Diagnosis dia on  dia.markid=ip.admitdiagnosis
left join Department dep on dep.id=ip.admitdept
left join Ward war on war.id=ip.admitward
left join users us on us.id=ip.resident where
cpcp.ljdm='{0}'

and cpipp.jrsj >= '{1}'
and cpipp.jrsj <= '{2}'", Ljdm, BeginTime, EndTime);

            DataTable dt = SqlHelper.ExecuteDataTable(strSql);
            //this.DataSource = dt;
            this.table1.DataSource = dt;
        }
    }
}