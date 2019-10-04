namespace YidanEHRReport
{
    partial class ReportPathVariationMonitor
    {
        #region Component Designer generated code
        /// <summary>
        /// Required method for telerik Reporting designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            Telerik.Reporting.TableGroup tableGroup1 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup2 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.TableGroup tableGroup3 = new Telerik.Reporting.TableGroup();
            Telerik.Reporting.ReportParameter reportParameter1 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter2 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter3 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.ReportParameter reportParameter4 = new Telerik.Reporting.ReportParameter();
            Telerik.Reporting.Drawing.StyleRule styleRule1 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule2 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule3 = new Telerik.Reporting.Drawing.StyleRule();
            Telerik.Reporting.Drawing.StyleRule styleRule4 = new Telerik.Reporting.Drawing.StyleRule();
            this.textBox1 = new Telerik.Reporting.TextBox();
            this.textBox2 = new Telerik.Reporting.TextBox();
            this.labelsGroupHeader = new Telerik.Reporting.GroupHeaderSection();
            this.tablePathVariationMonitor = new Telerik.Reporting.Table();
            this.textBox7 = new Telerik.Reporting.TextBox();
            this.textBox8 = new Telerik.Reporting.TextBox();
            this.textBox14 = new Telerik.Reporting.TextBox();
            this.txtLjdm = new Telerik.Reporting.TextBox();
            this.txtEndTime = new Telerik.Reporting.TextBox();
            this.textBox13 = new Telerik.Reporting.TextBox();
            this.syxhCaptionTextBox = new Telerik.Reporting.TextBox();
            this.txtBeginTime = new Telerik.Reporting.TextBox();
            this.labelsGroupFooter = new Telerik.Reporting.GroupFooterSection();
            this.labelsGroup = new Telerik.Reporting.Group();
            this.pageHeader = new Telerik.Reporting.PageHeaderSection();
            this.pageFooter = new Telerik.Reporting.PageFooterSection();
            this.currentTimeTextBox = new Telerik.Reporting.TextBox();
            this.pageInfoTextBox = new Telerik.Reporting.TextBox();
            this.reportHeader = new Telerik.Reporting.ReportHeaderSection();
            this.titleTextBox = new Telerik.Reporting.TextBox();
            this.detail = new Telerik.Reporting.DetailSection();
            this.panel1 = new Telerik.Reporting.Panel();
            ((System.ComponentModel.ISupportInitialize)(this)).BeginInit();
            // 
            // textBox1
            // 
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.6243467330932617D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox1.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.textBox1.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox1.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox1.Style.Color = System.Drawing.Color.Black;
            this.textBox1.Style.Font.Bold = true;
            this.textBox1.Style.Font.Name = "Arial";
            this.textBox1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(12D);
            this.textBox1.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox1.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox1.StyleName = "Apex.TableHeader";
            this.textBox1.Value = "½ÚµãÃû³Æ";
            // 
            // textBox2
            // 
            this.textBox2.Name = "textBox2";
            this.textBox2.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.216801643371582D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox2.Style.BackgroundColor = System.Drawing.Color.LightGray;
            this.textBox2.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox2.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox2.Style.Color = System.Drawing.Color.Black;
            this.textBox2.Style.Font.Bold = true;
            this.textBox2.Style.Font.Name = "Arial";
            this.textBox2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(12D);
            this.textBox2.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox2.StyleName = "Apex.TableHeader";
            this.textBox2.Value = "±äÒìÊý";
            // 
            // labelsGroupHeader
            // 
            this.labelsGroupHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(3.932708740234375D);
            this.labelsGroupHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.tablePathVariationMonitor});
            this.labelsGroupHeader.Name = "labelsGroupHeader";
            this.labelsGroupHeader.PrintOnEveryPage = true;
            // 
            // tablePathVariationMonitor
            // 
            this.tablePathVariationMonitor.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(9.6243467330932617D)));
            this.tablePathVariationMonitor.Body.Columns.Add(new Telerik.Reporting.TableBodyColumn(Telerik.Reporting.Drawing.Unit.Cm(9.216801643371582D)));
            this.tablePathVariationMonitor.Body.Rows.Add(new Telerik.Reporting.TableBodyRow(Telerik.Reporting.Drawing.Unit.Cm(0.5D)));
            this.tablePathVariationMonitor.Body.SetCellContent(0, 0, this.textBox7);
            this.tablePathVariationMonitor.Body.SetCellContent(0, 1, this.textBox8);
            tableGroup1.ReportItem = this.textBox1;
            tableGroup2.ReportItem = this.textBox2;
            this.tablePathVariationMonitor.ColumnGroups.Add(tableGroup1);
            this.tablePathVariationMonitor.ColumnGroups.Add(tableGroup2);
            this.tablePathVariationMonitor.DataSource = null;
            this.tablePathVariationMonitor.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox7,
            this.textBox8,
            this.textBox1,
            this.textBox2});
            this.tablePathVariationMonitor.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.00010002215276472271D), Telerik.Reporting.Drawing.Unit.Cm(0.099999949336051941D));
            this.tablePathVariationMonitor.Name = "tablePathVariationMonitor";
            tableGroup3.Groupings.AddRange(new Telerik.Reporting.Data.Grouping[] {
            new Telerik.Reporting.Data.Grouping("")});
            tableGroup3.Name = "Detail";
            this.tablePathVariationMonitor.RowGroups.Add(tableGroup3);
            this.tablePathVariationMonitor.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(18.841148376464844D), Telerik.Reporting.Drawing.Unit.Cm(1D));
            this.tablePathVariationMonitor.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.None;
            this.tablePathVariationMonitor.StyleName = "Apex.TableNormal";
            // 
            // textBox7
            // 
            this.textBox7.Name = "textBox7";
            this.textBox7.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.6243467330932617D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox7.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox7.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox7.Style.Font.Bold = false;
            this.textBox7.Style.Font.Name = "Arial";
            this.textBox7.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox7.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox7.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox7.StyleName = "Apex.TableBody";
            this.textBox7.Value = "=Fields.Ljmc";
            // 
            // textBox8
            // 
            this.textBox8.Name = "textBox8";
            this.textBox8.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.216801643371582D), Telerik.Reporting.Drawing.Unit.Cm(0.5D));
            this.textBox8.Style.BorderStyle.Default = Telerik.Reporting.Drawing.BorderType.Solid;
            this.textBox8.Style.BorderWidth.Default = Telerik.Reporting.Drawing.Unit.Pixel(1D);
            this.textBox8.Style.Font.Bold = false;
            this.textBox8.Style.Font.Name = "Arial";
            this.textBox8.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(11D);
            this.textBox8.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox8.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.textBox8.StyleName = "Apex.TableBody";
            this.textBox8.Value = "=Fields.VariationCount";
            // 
            // textBox14
            // 
            this.textBox14.CanGrow = true;
            this.textBox14.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.34708333015441895D), Telerik.Reporting.Drawing.Unit.Cm(0.19999949634075165D));
            this.textBox14.Name = "textBox14";
            this.textBox14.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.2902014255523682D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.textBox14.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.textBox14.Style.Font.Name = "Î¢ÈíÑÅºÚ";
            this.textBox14.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            this.textBox14.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox14.StyleName = "Caption";
            this.textBox14.Value = "ÁÙ´²Â·¾¶£º";
            // 
            // txtLjdm
            // 
            this.txtLjdm.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(2.6470832824707031D), Telerik.Reporting.Drawing.Unit.Cm(0.19999949634075165D));
            this.txtLjdm.Name = "txtLjdm";
            this.txtLjdm.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.2900006771087646D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.txtLjdm.Style.BorderColor.Bottom = System.Drawing.Color.LightGray;
            this.txtLjdm.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Outset;
            this.txtLjdm.Style.Font.Name = "Î¢ÈíÑÅºÚ";
            this.txtLjdm.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(12D);
            this.txtLjdm.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txtLjdm.Value = "";
            // 
            // txtEndTime
            // 
            this.txtEndTime.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(15.947083473205566D), Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388D));
            this.txtEndTime.Name = "txtEndTime";
            this.txtEndTime.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.2900006771087646D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.txtEndTime.Style.BorderColor.Bottom = System.Drawing.Color.LightGray;
            this.txtEndTime.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Outset;
            this.txtEndTime.Style.Font.Name = "Î¢ÈíÑÅºÚ";
            this.txtEndTime.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(12D);
            this.txtEndTime.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txtEndTime.Value = "";
            // 
            // textBox13
            // 
            this.textBox13.CanGrow = true;
            this.textBox13.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(13.647083282470703D), Telerik.Reporting.Drawing.Unit.Cm(0.19999989867210388D));
            this.textBox13.Name = "textBox13";
            this.textBox13.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.2902014255523682D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.textBox13.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.textBox13.Style.Font.Name = "Î¢ÈíÑÅºÚ";
            this.textBox13.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.textBox13.StyleName = "Caption";
            this.textBox13.Value = "½áÊøÊ±¼ä£º";
            // 
            // syxhCaptionTextBox
            // 
            this.syxhCaptionTextBox.CanGrow = true;
            this.syxhCaptionTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(8.6470832824707031D), Telerik.Reporting.Drawing.Unit.Cm(0.19999949634075165D));
            this.syxhCaptionTextBox.Name = "syxhCaptionTextBox";
            this.syxhCaptionTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3902015686035156D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.syxhCaptionTextBox.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.syxhCaptionTextBox.Style.Font.Name = "Î¢ÈíÑÅºÚ";
            this.syxhCaptionTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Left;
            this.syxhCaptionTextBox.StyleName = "Caption";
            this.syxhCaptionTextBox.Value = "¿ªÊ¼Ê±¼ä£º";
            // 
            // txtBeginTime
            // 
            this.txtBeginTime.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(11.057083129882813D), Telerik.Reporting.Drawing.Unit.Cm(0.19999949634075165D));
            this.txtBeginTime.Name = "txtBeginTime";
            this.txtBeginTime.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(2.3900010585784912D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.txtBeginTime.Style.BorderColor.Bottom = System.Drawing.Color.LightGray;
            this.txtBeginTime.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Outset;
            this.txtBeginTime.Style.Font.Name = "Î¢ÈíÑÅºÚ";
            this.txtBeginTime.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Pixel(12D);
            this.txtBeginTime.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.txtBeginTime.Value = "";
            // 
            // labelsGroupFooter
            // 
            this.labelsGroupFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
            this.labelsGroupFooter.Name = "labelsGroupFooter";
            this.labelsGroupFooter.Style.Visible = false;
            // 
            // labelsGroup
            // 
            this.labelsGroup.GroupFooter = this.labelsGroupFooter;
            this.labelsGroup.GroupHeader = this.labelsGroupHeader;
            this.labelsGroup.Name = "labelsGroup";
            // 
            // pageHeader
            // 
            this.pageHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
            this.pageHeader.Name = "pageHeader";
            // 
            // pageFooter
            // 
            this.pageFooter.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
            this.pageFooter.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.currentTimeTextBox,
            this.pageInfoTextBox});
            this.pageFooter.Name = "pageFooter";
            // 
            // currentTimeTextBox
            // 
            this.currentTimeTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.currentTimeTextBox.Name = "currentTimeTextBox";
            this.currentTimeTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.3677082061767578D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.currentTimeTextBox.StyleName = "PageInfo";
            this.currentTimeTextBox.Value = "=NOW()";
            // 
            // pageInfoTextBox
            // 
            this.pageInfoTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(9.473541259765625D), Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D));
            this.pageInfoTextBox.Name = "pageInfoTextBox";
            this.pageInfoTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(9.3677082061767578D), Telerik.Reporting.Drawing.Unit.Cm(0.60000002384185791D));
            this.pageInfoTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Right;
            this.pageInfoTextBox.StyleName = "PageInfo";
            this.pageInfoTextBox.Value = "=PageNumber";
            // 
            // reportHeader
            // 
            this.reportHeader.Height = Telerik.Reporting.Drawing.Unit.Cm(3.385624885559082D);
            this.reportHeader.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.titleTextBox,
            this.panel1});
            this.reportHeader.Name = "reportHeader";
            // 
            // titleTextBox
            // 
            this.titleTextBox.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0D), Telerik.Reporting.Drawing.Unit.Cm(0D));
            this.titleTextBox.Name = "titleTextBox";
            this.titleTextBox.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(18.8941650390625D), Telerik.Reporting.Drawing.Unit.Cm(2.2856252193450928D));
            this.titleTextBox.Style.BackgroundColor = System.Drawing.Color.Transparent;
            this.titleTextBox.Style.Color = System.Drawing.Color.Gray;
            this.titleTextBox.Style.Font.Bold = true;
            this.titleTextBox.Style.Font.Name = "»ªÎÄ¿¬Ìå";
            this.titleTextBox.Style.TextAlign = Telerik.Reporting.Drawing.HorizontalAlign.Center;
            this.titleTextBox.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.titleTextBox.StyleName = "Title";
            this.titleTextBox.Value = "Â·¾¶¼à²â±í";
            // 
            // detail
            // 
            this.detail.Height = Telerik.Reporting.Drawing.Unit.Cm(0.71437495946884155D);
            this.detail.Name = "detail";
            // 
            // panel1
            // 
            this.panel1.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.textBox13,
            this.txtEndTime,
            this.textBox14,
            this.txtLjdm,
            this.syxhCaptionTextBox,
            this.txtBeginTime});
            this.panel1.Location = new Telerik.Reporting.Drawing.PointU(Telerik.Reporting.Drawing.Unit.Cm(0.052916664630174637D), Telerik.Reporting.Drawing.Unit.Cm(2.2856252193450928D));
            this.panel1.Name = "panel1";
            this.panel1.Size = new Telerik.Reporting.Drawing.SizeU(Telerik.Reporting.Drawing.Unit.Cm(18.84124755859375D), Telerik.Reporting.Drawing.Unit.Cm(0.99999988079071045D));
            this.panel1.Style.BorderStyle.Bottom = Telerik.Reporting.Drawing.BorderType.Solid;
            // 
            // ReportPathVariationMonitor
            // 
            this.DataSource = null;
            this.Groups.AddRange(new Telerik.Reporting.Group[] {
            this.labelsGroup});
            this.Items.AddRange(new Telerik.Reporting.ReportItemBase[] {
            this.labelsGroupHeader,
            this.labelsGroupFooter,
            this.pageHeader,
            this.pageFooter,
            this.reportHeader,
            this.detail});
            this.Name = "ReportPathVariationMonitor";
            this.PageSettings.Landscape = false;
            this.PageSettings.Margins.Bottom = Telerik.Reporting.Drawing.Unit.Cm(1D);
            this.PageSettings.Margins.Left = Telerik.Reporting.Drawing.Unit.Cm(1D);
            this.PageSettings.Margins.Right = Telerik.Reporting.Drawing.Unit.Cm(1D);
            this.PageSettings.Margins.Top = Telerik.Reporting.Drawing.Unit.Cm(1D);
            this.PageSettings.PaperKind = System.Drawing.Printing.PaperKind.A4;
            reportParameter1.Name = "BeginTime";
            reportParameter2.Name = "EndTime";
            reportParameter3.Name = "Ljdm";
            reportParameter4.Name = "LjdmName";
            this.ReportParameters.Add(reportParameter1);
            this.ReportParameters.Add(reportParameter2);
            this.ReportParameters.Add(reportParameter3);
            this.ReportParameters.Add(reportParameter4);
            this.Style.BackgroundColor = System.Drawing.Color.White;
            styleRule1.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Title")});
            styleRule1.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(222)))), ((int)(((byte)(201)))));
            styleRule1.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(39)))), ((int)(((byte)(28)))));
            styleRule1.Style.Font.Name = "Gill Sans MT";
            styleRule1.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(20D);
            styleRule2.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Caption")});
            styleRule2.Style.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(231)))), ((int)(((byte)(222)))), ((int)(((byte)(201)))));
            styleRule2.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(39)))), ((int)(((byte)(28)))));
            styleRule2.Style.Font.Name = "Gill Sans MT";
            styleRule2.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule2.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule3.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("Data")});
            styleRule3.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(79)))), ((int)(((byte)(39)))), ((int)(((byte)(28)))));
            styleRule3.Style.Font.Name = "Gill Sans MT";
            styleRule3.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(10D);
            styleRule3.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            styleRule4.Selectors.AddRange(new Telerik.Reporting.Drawing.ISelector[] {
            new Telerik.Reporting.Drawing.StyleSelector("PageInfo")});
            styleRule4.Style.Color = System.Drawing.Color.FromArgb(((int)(((byte)(159)))), ((int)(((byte)(141)))), ((int)(((byte)(105)))));
            styleRule4.Style.Font.Name = "Gill Sans MT";
            styleRule4.Style.Font.Size = Telerik.Reporting.Drawing.Unit.Point(9D);
            styleRule4.Style.VerticalAlign = Telerik.Reporting.Drawing.VerticalAlign.Middle;
            this.StyleSheet.AddRange(new Telerik.Reporting.Drawing.StyleRule[] {
            styleRule1,
            styleRule2,
            styleRule3,
            styleRule4});
            this.Width = Telerik.Reporting.Drawing.Unit.Cm(18.8941650390625D);
            ((System.ComponentModel.ISupportInitialize)(this)).EndInit();

        }
        #endregion

        private Telerik.Reporting.GroupHeaderSection labelsGroupHeader;
        private Telerik.Reporting.GroupFooterSection labelsGroupFooter;
        private Telerik.Reporting.Group labelsGroup;
        private Telerik.Reporting.PageHeaderSection pageHeader;
        private Telerik.Reporting.PageFooterSection pageFooter;
        private Telerik.Reporting.TextBox currentTimeTextBox;
        private Telerik.Reporting.TextBox pageInfoTextBox;
        private Telerik.Reporting.ReportHeaderSection reportHeader;
        private Telerik.Reporting.TextBox titleTextBox;
        private Telerik.Reporting.DetailSection detail;
        private Telerik.Reporting.Table tablePathVariationMonitor;
        private Telerik.Reporting.TextBox textBox7;
        private Telerik.Reporting.TextBox textBox8;
        private Telerik.Reporting.TextBox textBox1;
        private Telerik.Reporting.TextBox textBox2;
        private Telerik.Reporting.TextBox textBox14;
        private Telerik.Reporting.TextBox txtLjdm;
        private Telerik.Reporting.TextBox txtEndTime;
        private Telerik.Reporting.TextBox textBox13;
        private Telerik.Reporting.TextBox syxhCaptionTextBox;
        private Telerik.Reporting.TextBox txtBeginTime;
        private Telerik.Reporting.Panel panel1;

    }
}