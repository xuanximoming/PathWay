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
    using Yidansoft.Service;

    /// <summary>
    /// Summary description for ReportTest.
    /// </summary>
    public partial class ReportRWPathOrdersAlls : Telerik.Reporting.Report
    {

        public ReportRWPathOrdersAlls()
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

            GetInpatientInfo(Syxh);

            SqlParameter param = new SqlParameter("@Syxh", SqlDbType.Decimal);

            param.Value = Convert.ToDecimal(Syxh);

            SqlParameter[] sqlparams = new SqlParameter[] { param };

            DataSet ds = sqlhelp.GetSetByPorc("usp_CP_GetInPatientAdviceAll", sqlparams);

            DataTable dt = ds.Tables[0];

            //if (dt != null && dt.Rows.Count > 0)
            //{
            //    this.txtInpathDate.Value = dt.Rows[0]["pathjrsj"].ToString();
            //}
            List<CP_PathOrdersAll> PathOrdersAlllist = new List<CP_PathOrdersAll>();
            CP_PathOrdersAll pathOrdersAll = new CP_PathOrdersAll();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                pathOrdersAll = new CP_PathOrdersAll();
                pathOrdersAll.ctyzxh = dt.Rows[i]["ctyzxh"].ToString();
                pathOrdersAll.ypmc = dt.Rows[i]["ypmc"].ToString();
                pathOrdersAll.zxdw = dt.Rows[i]["zxdw"].ToString();
                pathOrdersAll.ypjl = dt.Rows[i]["ypjl"].ToString() + dt.Rows[i]["jldw"].ToString();
                pathOrdersAll.pahtdetailID = dt.Rows[i]["pahtdetailID"].ToString();
                pathOrdersAll.Name = dt.Rows[i]["ljmc"].ToString();
                pathOrdersAll.orderstype = dt.Rows[i]["orderstype"].ToString();
                pathOrdersAll.prepahtdetailid = dt.Rows[i]["prepahtdetailid"].ToString();
                pathOrdersAll.nextpahtdetailid = dt.Rows[i]["nextpahtdetailid"].ToString();

                pathOrdersAll.jldw = dt.Rows[i]["jldw"].ToString();
                pathOrdersAll.zxcs = dt.Rows[i]["zxcs"].ToString();
                pathOrdersAll.nextpahtdetailid = dt.Rows[i]["ztnr"].ToString();
                PathOrdersAlllist.Add(pathOrdersAll);

            }
            //return Getby(PathOrdersAlllist);

            m_PathOrdersAlllist = new List<CP_PathOrdersAll>();
            GetNewOrderList(PathOrdersAlllist, ""); 

             this.DataSource = m_PathOrdersAlllist;
            string pathzddm="";
             DataTable dt1 = ds.Tables[1];
             for (int i = 0; i < dt1.Rows.Count; i++)
             {
                 pathzddm = pathzddm + dt1.Rows[i]["name"].ToString() + "(" + dt1.Rows[i]["icd"].ToString() + ")"; 
             }

             this.txtRyzd.Value = pathzddm;
        }

        List<CP_PathOrdersAll> m_PathOrdersAlllist;

        private void GetNewOrderList(List<CP_PathOrdersAll> list, string detailid)
        {
            CP_PathOrdersAll order = new CP_PathOrdersAll();
            foreach (CP_PathOrdersAll item in list)
            {
                if (item.prepahtdetailid == "" && detailid == "")
                {
                    m_PathOrdersAlllist.Add(item);
                    order = item;
                }
                else if (item.prepahtdetailid == detailid)
                {
                    m_PathOrdersAlllist.Add(item);
                    order = item;
                }

            }

            if (list.Count == m_PathOrdersAlllist.Count)
                return;
            else if (order.pahtdetailID != "")
                GetNewOrderList(list, order.pahtdetailID);

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
                reportNameTextBox.Value = dt.Rows[0]["Hzxm"].ToString() + ":临床路径路径告知单";
                //titleTextBox.Value = "【 " + dt.Rows[0]["Hzxm"].ToString() + " 】" + "医嘱告知单";
                titleTextBox.Value = "【 " + dt.Rows[0]["Ljmc"].ToString() +" 】" + "临床路径路径告知单";
                txtHzxm.Value = dt.Rows[0]["Hzxm"].ToString();
                this.txtCyksName.Value = dt.Rows[0]["CyksName"].ToString();
                this.txtCybqName.Value = dt.Rows[0]["CybqName"].ToString();
                this.txtBrzt.Value = dt.Rows[0]["Brzt"].ToString();

                this.txtZyhm.Value = dt.Rows[0]["Zyhm"].ToString();
                this.txtBrxb.Value = dt.Rows[0]["Brxb"].ToString();
                this.txtCycw.Value = dt.Rows[0]["Cycw"].ToString();
                this.txtXsnl.Value = dt.Rows[0]["Xsnl"].ToString();

               
                //this.txtLjmc.Value = dt.Rows[0]["Ljmc"].ToString();
                this.textBox5.Value = "患者姓名："+dt.Rows[0]["Hzxm"].ToString();
                this.textBox6.Value = "性别：" + dt.Rows[0]["Brxb"].ToString();
                this.textBox25.Value = "年龄：" + dt.Rows[0]["Xsnl"].ToString();
                this.textBox7.Value = "床位号：" + dt.Rows[0]["Cycw"].ToString();
                //路径状态 LjztName
                //this.txtljzt.Value = "路径状态：" + dt.Rows[0]["LjztName"].ToString();
                //this.txtinHosDate.Value = dt.Rows[0]["Ryrq"].ToString();

            }


            this.textBox1.Value = hosName;
            this.reportNameTextBox.Value = hosName + "临床路径路径告知单";

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