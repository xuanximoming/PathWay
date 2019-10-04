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
    public partial class PathSummary : Telerik.Reporting.Report
    {

        public PathSummary()
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

            //String DetailIDs = "";//this.ReportParameters["DetailID"].Value.ToString();

            GetInpatientInfo(Syxh);

            SqlParameter param = new SqlParameter("@Syxh", SqlDbType.Decimal);

            param.Value = Convert.ToDecimal(Syxh);

            SqlParameter[] sqlparams = new SqlParameter[] { param };

            DataTable dt = sqlhelp.GetTableByPorc("usp_CP_GetPathOrderBySyxh", sqlparams);


            ////根据参数传入的路径节点顺序ID 对数据集进行排序
            //String[] DetailIDList = DetailIDs.Split(',');

            //DataTable SourceTable = dt.Clone();
            //for (int i = 0; i < DetailIDList.Length - 1; i++)
            //{
            //    DataRow[] rows = dt.Select("PathDetailID='" + DetailIDList[i] + "'");

            //    for (int j = 0; j < rows.Length;j++ )
            //    {
            //        SourceTable.Rows.Add(rows[j].ItemArray);
            //    }
            //}
            if (dt != null && dt.Rows.Count > 0)
            {
                this.txtInpathDate.Value = dt.Rows[0]["pathjrsj"].ToString();
            }
            this.DataSource = dt;


        }

        /// <summary>
        /// 根据Syxh获取患者信息
        /// </summary>
        /// <param name="Syxh"></param>
        private void GetInpatientInfo(String Syxh)
        {
            ReportSqlHelp sqlhelp = new ReportSqlHelp();
            string hosName = sqlhelp.GetHospitalName();
            SqlParameter querykindparam = new SqlParameter("@querykind", SqlDbType.Int, 12);
            querykindparam.Value = 3;

            SqlParameter para_syxh = new SqlParameter("@Syxh", SqlDbType.VarChar, 12);
            para_syxh.Value = Syxh;

            SqlParameter pagesizem = new SqlParameter("@PageSize", SqlDbType.Int);
            pagesizem.Value = 1;

            SqlParameter currentpagem = new SqlParameter("@CurrentPage", SqlDbType.Int);
            currentpagem.Value = 0;

            SqlParameter[] sqlparams = new SqlParameter[] { querykindparam, para_syxh, pagesizem, currentpagem };

            DataTable dt = sqlhelp.GetTableByPorc("usp_CP_InpatientList", sqlparams);

            if (dt.Rows.Count > 0)
            {
                reportNameTextBox.Value = dt.Rows[0]["Hzxm"].ToString() + ":医嘱单";
                titleTextBox.Value = "【 " + dt.Rows[0]["Hzxm"].ToString() + " 】" + "路径总结一览";
                txtHzxm.Value = dt.Rows[0]["Hzxm"].ToString();
                this.txtCyksName.Value = dt.Rows[0]["CyksName"].ToString();
                this.txtCybqName.Value = dt.Rows[0]["CybqName"].ToString();
                this.txtBrzt.Value = dt.Rows[0]["Brzt"].ToString();

                this.txtZyhm.Value = dt.Rows[0]["Zyhm"].ToString();
                this.txtBrxb.Value = dt.Rows[0]["Brxb"].ToString();
                this.txtCycw.Value = dt.Rows[0]["Cycw"].ToString();
                this.txtXsnl.Value = dt.Rows[0]["Xsnl"].ToString();

                this.txtRyzd.Value = dt.Rows[0]["Name"].ToString() + "(" + dt.Rows[0]["RyzdCode"].ToString() + ")";
                //this.txtLjmc.Value = dt.Rows[0]["Ljmc"].ToString();
                this.textBox30.Value = dt.Rows[0]["Ljmc"].ToString();
                this.textBox7.Value = "患者姓名:" + dt.Rows[0]["Hzxm"].ToString();
                this.textBox28.Value = "性别：" + dt.Rows[0]["Brxb"].ToString();
                this.textBox15.Value = "年龄：" + dt.Rows[0]["Xsnl"].ToString();
                this.textBox27.Value = "床号：" + dt.Rows[0]["Cycw"].ToString();
                //路径状态 LjztName
                this.txtljzt.Value ="路径状态：" + dt.Rows[0]["LjztName"].ToString();
                this.txtinHosDate.Value = dt.Rows[0]["Ryrq"].ToString();

            }

            this.textBox25.Value = hosName;

            this.reportNameTextBox.Value = hosName + "  临床路径总结一览";

        }
 

        private void tableorder_ItemDataBound(object sender, EventArgs e)
        {
            //string s = e.ToString();

            foreach(Telerik.Reporting.Processing.TableRow dr in (sender as Telerik.Reporting.Processing.Table).Rows)
            {
                Telerik.Reporting.Processing.TextBox txt = (dr.GetCell(0) as Telerik.Reporting.Processing.TextBox);
                //if(txt.Text =="长期医嘱")
                //{
                //txt.Style.BackgroundColor = Color.Blue;
                //}
                //else  if (txt.Text == "临时医嘱")
                //{
                //    txt.Style.BackgroundColor = Color.Yellow;
                //}
                if (txt.Text == "变异医嘱")
                {
                    txt.Style.BackgroundColor = Color.FromArgb(255, 192, 255);
                } 
            }
        }




    }
}