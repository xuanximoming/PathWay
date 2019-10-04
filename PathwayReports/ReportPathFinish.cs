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
    using System.Collections.Generic;
    using System.Reflection;
    using Yidansoft.Service.Entity;
    using YidanSoft.Core;

    /// <summary>
    /// Summary description for ReportPathQuit.
    /// </summary>
    public partial class ReportPathFinish : Telerik.Reporting.Report
    {
        public ReportPathFinish()
        {

            InitializeComponent();
            this.DataSource = null;
            this.NeedDataSource += new EventHandler(ReportPathQuit_NeedDataSource);

        }

        private void ReportPathQuit_NeedDataSource(object sender, EventArgs e)
        {



            String BeginTime = this.ReportParameters["BeginTime"].Value.ToString();
            String EndTime = this.ReportParameters["EndTime"].Value.ToString();
            String Ljdm = this.ReportParameters["Ljdm"].Value.ToString();
            String Dept = this.ReportParameters["Dept"].Value.ToString();

            String Period = this.ReportParameters["Period"].Value.ToString();

            CreateTableColumns(GetRptPathFinish(BeginTime, EndTime, Ljdm, Dept, Period));

        }


        private void CreateTableColumns(List<RPT_PathFinishList> pathfinistlist)
        {

            if (pathfinistlist == null)
                return;

            Telerik.Reporting.TextBox txtbody;

            Telerik.Reporting.TextBox txttitle;

            Telerik.Reporting.TableGroup tableGrouptitle;
            //Telerik.Reporting.TableGroup tableGroupbody;

            RPT_PathFinishList rptListTitle = new RPT_PathFinishList();
            rptListTitle = (RPT_PathFinishList)pathfinistlist[0];

            Type type = rptListTitle.GetType();
            PropertyInfo[] pf = type.GetProperties();
            string property = "";
            //计算需要添加列数量
            int len = 0;
            for (int i = 0; i < pf.Length; i++)
            {
                if (pf[i].Name.Contains("Col"))
                {

                    property = pf[i].GetValue(rptListTitle, new object[] { }).ToString();
                    if (property.Length > 0)
                    {
                        len++;
                    }
                }
            }



            //定义数组存放表中列控件
            int j = 0;
            for (int i = 0; i < pf.Length; i++)
            {
                if (pf[i].Name.Contains("Col"))
                {

                    property = pf[i].GetValue(rptListTitle, new object[] { }).ToString();

                    if (property.Length > 0)
                    {
                        txtbody = new Telerik.Reporting.TextBox();
                        //table_body
                        txtbody.Name = "txtbody" + i.ToString();
                        txtbody.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.5D, Telerik.Reporting.Drawing.UnitType.Cm));
                        txtbody.Style.Font.Name = "微软雅黑";
                        txtbody.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(8D, Telerik.Reporting.Drawing.UnitType.Point);
                        txtbody.StyleName = "Apex.TableBody";
                        txtbody.Value = "=IsNull(Fields." + pf[i].Name + "," + pf[i].Name + ")";
                        //table_title
                        txttitle = new Telerik.Reporting.TextBox();
                        txttitle.Name = "txttitle" + i.ToString();
                        txttitle.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(2D, Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.5D, Telerik.Reporting.Drawing.UnitType.Cm));
                        txttitle.Style.BackgroundColor = System.Drawing.Color.LightGray;
                        txttitle.Style.Color = System.Drawing.Color.Black;
                        txttitle.Style.Font.Bold = true;
                        txttitle.Style.Font.Name = "微软雅黑";
                        txttitle.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(8D, Telerik.Reporting.Drawing.UnitType.Point);
                        txttitle.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
                        txttitle.StyleName = "Apex.TableHeader";
                        txttitle.Value = property;

                        //添加列 设置列宽
                        this.tabletest.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(new Telerik.Reporting.Drawing.Unit(2D, Telerik.Reporting.Drawing.UnitType.Cm)));
                        this.tabletest.Body.SetCellContent(0, j + 1, txtbody);

                        this.tabletest.Items.AddRange(new Telerik.Reporting.ReportItemBase[] { txttitle, txtbody });

                        j++;
                        tableGrouptitle = new Telerik.Reporting.TableGroup();
                        tableGrouptitle.Name = "tableGrouptitle" + i.ToString();
                        tableGrouptitle.ReportItem = txttitle;
                        this.tabletest.ColumnGroups.Add(tableGrouptitle);


                        //当打印的列数大于8时 纸张变为横打
                        if (j > 8)
                        {
                            this.PageSettings.Landscape = true;
                            titleTextBox.Width = new Telerik.Reporting.Drawing.Unit(27D, Telerik.Reporting.Drawing.UnitType.Cm);
                            pageInfoTextBox.Width = new Telerik.Reporting.Drawing.Unit(20D, Telerik.Reporting.Drawing.UnitType.Cm);
                        }
                    }
                }

                DataTable dt = GetTable(pathfinistlist);

                this.tabletest.DataSource = dt;
            }

        }

        /// <summary>
        /// 构造数据源
        /// </summary>
        /// <param name="pathfinistlist"></param>
        /// <returns></returns>
        private DataTable GetTable(List<RPT_PathFinishList> pathfinistlist)
        {
            DataTable dt = new DataTable();

            RPT_PathFinishList rptListTitle = new RPT_PathFinishList();
            rptListTitle = (RPT_PathFinishList)pathfinistlist[0];

            Type type = rptListTitle.GetType();
            PropertyInfo[] pf = type.GetProperties();
            string property = "";

            for (int i = 0; i < pf.Length; i++)
            {

                property = pf[i].GetValue(rptListTitle, new object[] { }).ToString();
                if (property.Length > 0)
                {
                    DataColumn col = new DataColumn();
                    col.ColumnName = pf[i].Name.Trim();
                    dt.Columns.Add(col);
                }

            }
            pathfinistlist.RemoveAt(0);
            for (int i = 0; i < pathfinistlist.Count; i++)
            {
                DataRow dr = dt.NewRow();

                #region DataRow中添加记录
                for (int j = 0; j < dt.Columns.Count; j++)
                {
                    switch (dt.Columns[j].ColumnName)
                    {
                        case "PathID":
                            dr["PathID"] = pathfinistlist[i].PathID;
                            break;
                        case "PathName":
                            dr["PathName"] = pathfinistlist[i].PathName;
                            break;
                        case "Col_A":
                            dr["Col_A"] = pathfinistlist[i].Col_A;
                            break;
                        case "Col_B":
                            dr["Col_B"] = pathfinistlist[i].Col_B;
                            break;
                        case "Col_C":
                            dr["Col_C"] = pathfinistlist[i].Col_C;
                            break;
                        case "Col_D":
                            dr["Col_D"] = pathfinistlist[i].Col_D;
                            break;
                        case "Col_E":
                            dr["Col_E"] = pathfinistlist[i].Col_E;
                            break;
                        case "Col_F":
                            dr["Col_F"] = pathfinistlist[i].Col_F;
                            break;
                        case "Col_G":
                            dr["Col_G"] = pathfinistlist[i].Col_G;
                            break;
                        case "Col_H":
                            dr["Col_H"] = pathfinistlist[i].Col_H;
                            break;
                        case "Col_I":
                            dr["Col_I"] = pathfinistlist[i].Col_I;
                            break;
                        case "Col_J":
                            dr["Col_J"] = pathfinistlist[i].Col_J;
                            break;
                        case "Col_K":
                            dr["Col_K"] = pathfinistlist[i].Col_K;
                            break;
                        case "Col_L":
                            dr["Col_L"] = pathfinistlist[i].Col_L;
                            break;
                        case "Col_M":
                            dr["Col_M"] = pathfinistlist[i].Col_M;
                            break;
                        case "Col_N":
                            dr["Col_N"] = pathfinistlist[i].Col_N;
                            break;
                        case "Col_O":
                            dr["Col_O"] = pathfinistlist[i].Col_O;
                            break;
                        default:
                            break;
                    }
                }

                #endregion

                dt.Rows.Add(dr);
            }

            return dt;
        }

        private List<RPT_PathFinishList> GetRptPathFinish(string Begintime, string Endtime, string Ljdm, string Dept, string period)
        {
            //RPT_PathFinish rpt_pathfinish = new RPT_PathFinish();

            List<RPT_PathFinishList> rpt_pathfinishList = new List<RPT_PathFinishList>();

            try
            {

                //添加输入查询参数、赋予值

                SqlParameter parbegin = new SqlParameter("@begindate", SqlDbType.VarChar);
                parbegin.Value = Begintime;


                SqlParameter parend = new SqlParameter("@enddate", SqlDbType.VarChar);
                parend.Value = Endtime;
                SqlParameter parljdm = new SqlParameter("@Ljdm", SqlDbType.VarChar);
                parljdm.Value = Ljdm;
                SqlParameter pardept = new SqlParameter("@dept", SqlDbType.VarChar);
                pardept.Value = Dept;
                SqlParameter parperion = new SqlParameter("@period", SqlDbType.VarChar);
                parperion.Value = period;

                SqlParameter[] sqlparams = new SqlParameter[] { parbegin, parend, parljdm, pardept, parperion };
                DataTable dataTable = DataAccessFactory.DefaultDataAccess.ExecuteDataTable("usp_CP_RptPathFinish", sqlparams, CommandType.StoredProcedure);

                try
                {
                    if (dataTable.Rows[0][0].ToString() == "False")
                    {
                        MessageBox.Show("统计的时间范围不能超过15个时间单位!");
                        return null;
                    }
                }
                catch
                {
                    MessageBox.Show("无数据...");
                    return null;
                }

                #region 构建报表中列表需要的数据源
                RPT_PathFinishList finishtitle = new RPT_PathFinishList();

                finishtitle.PathID = "路径代码";
                finishtitle.PathName = "路径名称";

                #region 动态将列表的名称添加到实体finishtitle中
                foreach (DataRow row in dataTable.Rows)
                {
                    if (row["rownumber"].ToString() == "1")
                    {
                        //添加周期列，显示为页面显示信息
                        #region 通过循环将需要查询的列表中的列名存入实体中
                        switch (Convert.ToInt32(row["colindex"].ToString()))
                        {
                            case 0:
                                finishtitle.Col_A = row["period_name"].ToString();
                                break;
                            case 1:
                                finishtitle.Col_B = row["period_name"].ToString();
                                break;
                            case 2:
                                finishtitle.Col_C = row["period_name"].ToString();
                                break;
                            case 3:
                                finishtitle.Col_D = row["period_name"].ToString();
                                break;
                            case 4:
                                finishtitle.Col_E = row["period_name"].ToString();
                                break;
                            case 5:
                                finishtitle.Col_F = row["period_name"].ToString();
                                break;
                            case 6:
                                finishtitle.Col_G = row["period_name"].ToString();
                                break;
                            case 7:
                                finishtitle.Col_H = row["period_name"].ToString();
                                break;
                            case 8:
                                finishtitle.Col_I = row["period_name"].ToString();
                                break;
                            case 9:
                                finishtitle.Col_J = row["period_name"].ToString();
                                break;
                            case 10:
                                finishtitle.Col_K = row["period_name"].ToString();
                                break;
                            case 11:
                                finishtitle.Col_L = row["period_name"].ToString();
                                break;
                            case 12:
                                finishtitle.Col_M = row["period_name"].ToString();
                                break;
                            case 13:
                                finishtitle.Col_N = row["period_name"].ToString();
                                break;
                            case 14:
                                finishtitle.Col_O = row["period_name"].ToString();
                                break;
                            default: break;
                        }
                        #endregion
                    }
                }
                rpt_pathfinishList.Add(finishtitle);
                #endregion
                int rowcount = Convert.ToInt32(dataTable.Rows[dataTable.Rows.Count - 1]["rownumber"].ToString());
                int colcount = Convert.ToInt32(dataTable.Rows[dataTable.Rows.Count - 1]["colindex"].ToString()) + 1;

                //循环添加行信息
                for (int i = 0; i < rowcount; i++)
                {
                    RPT_PathFinishList finishlist = new RPT_PathFinishList();

                    finishlist.PathID = dataTable.Rows[i]["ljdm"].ToString();
                    finishlist.PathName = dataTable.Rows[i]["Name"].ToString();
                    //循环添加列记录
                    for (int j = 0; j < colcount; j++)
                    {
                        #region 通过循环将表中数据加载到实体中

                        switch (j)
                        {
                            case 0:
                                finishlist.Col_A = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 1:
                                finishlist.Col_B = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 2:
                                finishlist.Col_C = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 3:
                                finishlist.Col_D = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 4:
                                finishlist.Col_E = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 5:
                                finishlist.Col_F = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 6:
                                finishlist.Col_G = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 7:
                                finishlist.Col_H = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 8:
                                finishlist.Col_I = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 9:
                                finishlist.Col_J = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 10:
                                finishlist.Col_K = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 11:
                                finishlist.Col_L = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 12:
                                finishlist.Col_M = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 13:
                                finishlist.Col_N = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            case 14:
                                finishlist.Col_O = dataTable.Rows[rowcount * j + i]["mess"].ToString();
                                break;
                            default: break;
                        }
                        #endregion

                    }
                    rpt_pathfinishList.Add(finishlist);
                }
                #endregion

            }
            catch (Exception ex)
            {
                //ThrowException(ex);
            }
            finally
            {
            }

            return rpt_pathfinishList;
        }
    }
}