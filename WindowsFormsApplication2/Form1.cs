using System;
using System.Windows.Forms;
using System.Xml;

namespace WindowsFormsApplication2
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            //1、创建一个XML文档
            XmlDocument doc = new XmlDocument();
            //2、创建第一行描述信息
            XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-8", null);
            //3、将创建的第一行描述信息添加到文档中
            doc.AppendChild(dec);
            //4、给文档添加根节点
            XmlElement XMLData = doc.CreateElement("XMLData");
            doc.AppendChild(XMLData);
            XmlElement YZInfo = doc.CreateElement("YZInfo");
            XMLData.AppendChild(YZInfo);
            XmlElement YZ = doc.CreateElement("YZ");
            YZ.SetAttribute("Code", "11111");
            YZInfo.AppendChild(YZ);
        }
    }
}
