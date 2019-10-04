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
    /// Summary description for ReportPathQuit.
    /// </summary>
    public partial class ReportPagePring : Telerik.Reporting.Report
    {
        public ReportPagePring()
        {

            InitializeComponent();
            this.DataSource = null;
            this.NeedDataSource += new EventHandler(ReportPathQuit_NeedDataSource);

        }

        private void ReportPathQuit_NeedDataSource(object sender, EventArgs e)
        {
            ReportSqlHelp sqlhelp = new ReportSqlHelp();

            String Title = this.ReportParameters["Title"].Value.ToString();
            String TableTitle = this.ReportParameters["TableTitle"].Value.ToString();
            String TableBindCol = this.ReportParameters["TableBindCol"].Value.ToString();
            String ColWidth = this.ReportParameters["ColWidth"].Value.ToString();

            String ExecSQL = this.ReportParameters["ExecSQL"].Value.ToString();


            CreateTableColumns(TableTitle, TableBindCol, ColWidth);
            titleTextBox.Value = Title;
            reportNameTextBox.Value = Title;

            DataTable dt = sqlhelp.GetTableBySQL(ExecSQL);

            this.DataSource = dt;

            this.tabletest.DataSource = dt;

        }


        private void CreateTableColumns(String tableTitle,String tablecolumns,String ColWidth)
        {
            String[] tabTitles = tableTitle.Split(',');
            String[] tabcolumnss = tablecolumns.Split(',');
            String[] ColWidths = ColWidth.Split(',');

            Telerik.Reporting.ReportItemBase[] itemlist = new Telerik.Reporting.ReportItemBase[tabTitles.Length*2];


            Telerik.Reporting.TextBox txtbody;

            Telerik.Reporting.TextBox txttitle;

            Telerik.Reporting.TableGroup tableGrouptitle;
            //Telerik.Reporting.TableGroup tableGroupbody;

            // 取出第一列空格列
            //if (tabTitles.Length > 0)
            //{
            //    this.tabletest.ColumnGroups.RemoveAt(0);
            //    this.tabletest.Body.Columns.RemoveAt(0);
            //}
            for (int i = 0; i < tabTitles.Length; i++)
            {

                txtbody = new Telerik.Reporting.TextBox();
                //table_body
                txtbody.Name = "txtbody" + i.ToString();
                txtbody.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(Convert.ToDouble(ColWidths[i].ToString()), Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.5D, Telerik.Reporting.Drawing.UnitType.Cm));
                txtbody.Style.Font.Name = "微软雅黑";
                txtbody.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(8D, Telerik.Reporting.Drawing.UnitType.Point);
                txtbody.StyleName = "Apex.TableBody";
                txtbody.Value = "=Fields." + tabcolumnss[i];

                //table_title
                txttitle = new Telerik.Reporting.TextBox();
                txttitle.Name = "txttitle" + i.ToString();
                txttitle.Size = new Telerik.Reporting.Drawing.SizeU(new Telerik.Reporting.Drawing.Unit(Convert.ToDouble(ColWidths[i].ToString()), Telerik.Reporting.Drawing.UnitType.Cm), new Telerik.Reporting.Drawing.Unit(0.5D, Telerik.Reporting.Drawing.UnitType.Cm));
                txttitle.Style.BackgroundColor = System.Drawing.Color.LightGray;
                txttitle.Style.Color = System.Drawing.Color.Black;
                txttitle.Style.Font.Bold = true;
                txttitle.Style.Font.Name = "微软雅黑";
                txttitle.Style.Font.Size = new Telerik.Reporting.Drawing.Unit(8D, Telerik.Reporting.Drawing.UnitType.Point);
                txttitle.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
                txttitle.StyleName = "Apex.TableHeader";
                txttitle.Value = tabTitles[i];

                //添加列 设置列宽
                this.tabletest.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(new Telerik.Reporting.Drawing.Unit(Convert.ToDouble(ColWidths[i].ToString()), Telerik.Reporting.Drawing.UnitType.Cm)));
                this.tabletest.Body.SetCellContent(0, i+1, txtbody);

                //this.tabletest.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {txttitle,txtbody});

                itemlist[2 * i] = txttitle;
                itemlist[2 * i + 1] = txtbody;

                tableGrouptitle = new Telerik.Reporting.TableGroup();
                tableGrouptitle.Name = "tableGrouptitle" + i.ToString();
                tableGrouptitle.ReportItem = txttitle;
                this.tabletest.ColumnGroups.Add(tableGrouptitle);

                //tableGroupbody = new Telerik.Reporting.TableGroup();
                //tableGroupbody.Name = "tableGroupbody" + i.ToString();
                //tableGroupbody.ReportItem = txtbody;
                //this.tabletest.ColumnGroups.Add(tableGroupbody);
            }
            this.tabletest.Items.AddRange(itemlist);
            //this.textBox2.Visible = false;
            //this.textBox3.Visible = false;
        }

        
    }
}