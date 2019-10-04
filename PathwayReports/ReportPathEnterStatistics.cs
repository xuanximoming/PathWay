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
    /// Summary description for ReportPathEnterStatistics.
    /// </summary>
    public partial class ReportPathEnterStatistics : Telerik.Reporting.Report
    {
        public ReportPathEnterStatistics()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            this.NeedDataSource += new EventHandler(ReportPathEnterStatistics_NeedDataSource);
        }

        void ReportPathEnterStatistics_NeedDataSource(object sender, EventArgs e)
        {
            ReportSqlHelp sqlhelp = new ReportSqlHelp();
            String BeginTime = this.ReportParameters["BeginTime"].Value.ToString();//��ʼʱ��
            String EndTime = this.ReportParameters["EndTime"].Value.ToString();//����ʱ��
            String Dept = this.ReportParameters["Dept"].Value.ToString();//��������
            String GetType = this.ReportParameters["GetType"].Value.ToString();//��ѯ��ʽ
            String Ljzt = this.ReportParameters["Ljzt"].Value.ToString();//·��״̬
            String Ljdm = this.ReportParameters["Ljdm"].Value.ToString();//·������
            String Bzdm = this.ReportParameters["Bzdm"].Value.ToString();//���ִ���
            txtBeginTime.Value = BeginTime;
            txtEndTime.Value = EndTime;
            txtDept.Value = Dept;
            switch (GetType)
            {
                case "1":
                    txtGetType.Value = "����·��";
                    break;
                case "2":
                    txtGetType.Value = "���ݲ���";
                    break;
                default:
                    break;
            }
            switch (Ljzt)
            {
                case "2":
                    txtLjzt.Value = "ֹͣ";
                    break;
                case "3":
                    txtLjzt.Value = "���";
                    break;
                default:
                    txtLjzt.Value = "ȫ��";
                    break;
            }
            txtLjdm.Value = Ljdm;
            txtBzdm.Value = Bzdm;

            SqlParameter paramBeginTime = new SqlParameter("@begindate", SqlDbType.NVarChar, 100);
            paramBeginTime.Value = BeginTime;

            SqlParameter paramEndTime = new SqlParameter("@enddate", SqlDbType.NVarChar, 100);
            paramEndTime.Value = EndTime;

            SqlParameter paramDept = new SqlParameter("@dept", SqlDbType.NVarChar, 100);
            paramDept.Value = Dept;

            SqlParameter paramGetType = new SqlParameter("GetType", SqlDbType.NVarChar, 12);
            paramGetType.Value = GetType;

            SqlParameter paramLjzt = new SqlParameter("@Ljzt", SqlDbType.NVarChar, 12);
            paramLjzt.Value = Ljzt;

            SqlParameter paramLjdm = new SqlParameter("@Ljdm", SqlDbType.NVarChar, 100);
            paramLjdm.Value = Ljdm;

            SqlParameter paramBzdm = new SqlParameter("@Bzdm", SqlDbType.NVarChar, 100);
            paramBzdm.Value = Bzdm;

            SqlParameter[] sqlparams = new SqlParameter[] { paramBeginTime, paramEndTime, paramDept, paramLjdm, paramBzdm, paramLjzt, paramGetType };
            DataTable dt = sqlhelp.GetTableByPorc("usp_CP_RptPathEnterStatistics", sqlparams);
            this.tablePathEnterStatistics.DataSource = dt;
        }
    }
}