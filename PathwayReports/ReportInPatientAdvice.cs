namespace YidanEHRReport
{
    using System;
    using System.ComponentModel;
    using System.Drawing;
    using System.Windows.Forms;
    using Telerik.Reporting;
    using Telerik.Reporting.Drawing;
    using System.Data;
    using YidanSoft.Tool;
    using System.Data.SqlClient;
    using System.Collections.Generic;

    /// <summary>
    /// Summary description for ReportTest.
    /// </summary>
    public partial class ReportInPatientAdvice : Telerik.Reporting.Report
    {

        public ReportInPatientAdvice()
        {
            //
            // Required for telerik Reporting designer support
            //
            InitializeComponent();
            this.NeedDataSource += new EventHandler(ReportTest_NeedDataSource);
            //GetDataSource();

            //
            // TODO: Add any constructor code after InitializeComponent call
            //
            
        }

        void ReportTest_NeedDataSource(object sender, EventArgs e)
        {
            GetDataSource();
        }

        private void GetDataSource()
        {
            ReportSqlHelp sqlhelp = new ReportSqlHelp();

            String Syxh = this.ReportParameters["Syxh"].Value.ToString() == "" ? "0" : this.ReportParameters["Syxh"].Value.ToString();

            String DetailIDs = this.ReportParameters["DetailID"].Value.ToString();

            GetInpatientInfo(Syxh);

            SqlParameter param = new SqlParameter("@Syxh", SqlDbType.Decimal);

            param.Value = Convert.ToDecimal(Syxh);

            SqlParameter[] sqlparams = new SqlParameter[] { param };

            DataTable dt = sqlhelp.GetTableByPorc("usp_CP_GetInpatientOrder", sqlparams);


            //根据参数传入的路径节点顺序ID 对数据集进行排序
            String[] DetailIDList = DetailIDs.Split(',');

            DataTable SourceTable = dt.Clone();
            for (int i = 0; i < DetailIDList.Length - 1; i++)
            {
                DataRow[] rows = dt.Select("PathDetailID='" + DetailIDList[i] + "'");

                for (int j = 0; j < rows.Length;j++ )
                {
                    SourceTable.Rows.Add(rows[j].ItemArray);
                }
            }

            this.DataSource = SourceTable;
        }

        /// <summary>
        /// 根据Syxh获取患者信息
        /// </summary>
        /// <param name="Syxh"></param>
        private void GetInpatientInfo(String Syxh)
        {
            ReportSqlHelp sqlhelp = new ReportSqlHelp();
            SqlParameter querykindparam = new SqlParameter("@querykind", SqlDbType.Int, 12);
            querykindparam.Value = 3;

            SqlParameter para_syxh = new SqlParameter("@Syxh", SqlDbType.VarChar, 12);
            para_syxh.Value = Syxh;

            SqlParameter[] sqlparams = new SqlParameter[] { querykindparam, para_syxh };

            DataTable dt = sqlhelp.GetTableByPorc("usp_CP_InpatientList", sqlparams);

            if (dt.Rows.Count > 0)
            {
                reportNameTextBox.Value = dt.Rows[0]["Hzxm"].ToString() + ":医嘱单";
                titleTextBox.Value = "【 " + dt.Rows[0]["Hzxm"].ToString() + " 】" + "医嘱单打印";
                txtHzxm.Value = dt.Rows[0]["Hzxm"].ToString();
                this.txtCyksName.Value = dt.Rows[0]["CyksName"].ToString();
                this.txtCybqName.Value = dt.Rows[0]["CybqName"].ToString();
                this.txtBrzt.Value = dt.Rows[0]["Brzt"].ToString();

                this.txtZyhm.Value = dt.Rows[0]["Zyhm"].ToString();
                this.txtBrxb.Value = dt.Rows[0]["Brxb"].ToString();
                this.txtCycw.Value = dt.Rows[0]["Cycw"].ToString();
                this.txtXsnl.Value = dt.Rows[0]["Xsnl"].ToString();

                this.txtRyzd.Value = dt.Rows[0]["Ryzd"].ToString();
                this.txtLjmc.Value = dt.Rows[0]["Ljmc"].ToString();

            }
            


        }




    }
}