using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using YidanEHRApplication.YidanEHRServiceReference;
using YidanEHRApplication.Models;
using System.IO;
using Telerik.Windows.Documents.FormatProviders;
using Telerik.Windows.Documents.Model;
using YidanEHRApplication.DataService;
namespace YidanEHRApplication.Pass
{
    public partial class RWMedicineDirectDetailForm
    {
        #region 函数
        public RWMedicineDirectDetailForm(String ID)
        {
            InitializeComponent();
            Load(ID);
            this.radRichTextBox1.Height = 500;
        }
        private void Load(String ID)
        {
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            ServiceClient.GetMedicinesDirectCompleted +=
                    (obj, ea) =>
                    {
                        MedicineDirect medicinesDirectTemp = new MedicineDirect();
                        medicinesDirectTemp = ea.Result;
                        lblDirectTitle.Text = medicinesDirectTemp.DirectTitle2;
                        lblCompany.Text = medicinesDirectTemp.Company;
                        this.radRichTextBox1.Document =
                            ImportXaml(medicinesDirectTemp.DirectContent);
                    };
                ServiceClient.GetMedicinesDirectAsync(ID);
            ServiceClient.CloseAsync();
        }
    
        private RadDocument ImportXaml(string content)
        {
            //IDocumentFormatProvider provider = new Telerik.Windows.Documents.FormatProviders.Html.HtmlFormatProvider();
            IDocumentFormatProvider provider = new Telerik.Windows.Documents.FormatProviders.Txt.TxtFormatProvider();
            RadDocument document;
            using (MemoryStream stream = new MemoryStream())
            {
                StreamWriter writer = new StreamWriter(stream);
                writer.Write(content);
                writer.Flush();
                stream.Seek(0, SeekOrigin.Begin);
                document = provider.Import(stream);
            }
            return document;
        }
        #endregion
    }
}
