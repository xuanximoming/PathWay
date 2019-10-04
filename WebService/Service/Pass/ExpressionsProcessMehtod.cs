using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using Yidansoft.Service.Entity;
using System.Collections;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Configuration;
using DrectSoft.Tool;
using System.Xml;
using Yidansoft.Service.Entity.Pass;
using System.Web.UI;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        #region 公式计算
        /// <summary>
        /// 公式计算
        /// </summary>
        /// <param name="strExpressions">计算表达式</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public string Eval(string strExpressions)
        {
            try
            {
                MSScriptControl.ScriptControl objScriptControl = new MSScriptControl.ScriptControl();

                objScriptControl.Language = "VBScript";
                return objScriptControl.Eval(strExpressions).ToString();
                //return string.Empty;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return "";
            }
        }
        #endregion

        #region 读取公式配置文件（XML）
        /// <summary>
        /// 读取公式配置文件（XML）
        /// </summary>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<Expressions> ReadExpressionsConfigXML()
        {
            List<Expressions> listCPExpressions = new List<Expressions>();
            Expressions cpExpressions = new Expressions();

            try
            {
                Page objPage = new Page();

                XmlReader xmlReader = XmlReader.Create(objPage.Server.MapPath("/Service/Pass/XML/ExpressionsConfig.xml"));
                XDocument xDocument = XDocument.Load(xmlReader);

                XElement xExpressionsTypeElement = xDocument.Element(XName.Get("ExpressionsConfig"));
                //公式类型节点（ExpressionsGroupType）
                foreach (XElement xExpressionsElement in xExpressionsTypeElement.Elements())
                {
                    //公式类别
                    String strExpressionsType = xExpressionsElement.Attribute("value").Value;
                    //公式节点（Expressions）
                    foreach (XElement xExpressionsChildElement in xExpressionsElement.Elements())
                    {
                        cpExpressions = new Expressions();
                        //公式属性节点（ExpressionsName、ExpressionsProcess等）
                        foreach (XElement xChildElement in xExpressionsChildElement.Elements())
                        {
                            if (xChildElement.Name == "ExpressionsParameterGroup")
                            {
                                #region 输入参数控件实体
                                //参数控件属性实体
                                ParameterProperty modelParameterProperty = new ParameterProperty();

                                // 参数控件属性集节点（ExpressionsParameter）
                                foreach (XElement xParameterElement in xChildElement.Elements())
                                {
                                    //单个参数控件属性实体置空值
                                    modelParameterProperty = new ParameterProperty();
                                    //输入参提示数标内容节点（ExpressionsParameter）
                                    modelParameterProperty.LabelText = xParameterElement.Attribute("text").Value;

                                    //参数控件属性节点（Property）
                                    foreach (XElement xPropertyElement in xParameterElement.Elements())
                                    {
                                        if (xPropertyElement.Attribute("name").Value.ToLower() == "tag")//单个参数控件tag属性值，用于后台处理公式值替换
                                        {
                                            modelParameterProperty.Tag = xPropertyElement.Attribute("value").Value;
                                        }
                                        else//除Tag属性以外的节点
                                        {
                                            modelParameterProperty.AddProperty(xPropertyElement.Attribute("name").Value,
                                                           xPropertyElement.Attribute("value").Value);
                                        }
                                    }
                                    cpExpressions.AddExpressionsParameter(modelParameterProperty);
                                }

                                #endregion
                            }
                            else
                            {
                                #region 部分公式属性
                                switch (xChildElement.Name.ToString().ToLower())
                                {
                                    case "expressionsname": //公式表达式
                                        {
                                            cpExpressions.ExpressionsGroupType = strExpressionsType;
                                            cpExpressions.ExpressionsName = xChildElement.Attribute("value").Value;
                                            break;
                                        }
                                    case "expressionsprocess"://后台处理公式表达式
                                        {
                                            cpExpressions.ExpressionsProcess = xChildElement.Attribute("value").Value;
                                            break;
                                        }
                                    case "expressionsreusltunit"://计算结果单位
                                        {
                                            cpExpressions.ExpressionsReusltUnit = xChildElement.Attribute("value").Value;
                                            break;
                                        }
                                    case "expressionsdescribe"://公式表达式描述
                                        {
                                            cpExpressions.ExpressionsDescribe = xChildElement.Attribute("value").Value;
                                            break;
                                        }
                                }
                                #endregion
                            }
                        }
                        listCPExpressions.Add(cpExpressions);
                    }

                }
                return listCPExpressions;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }
        }


        /// <summary>
        /// 读取公式配置文件（XML）
        /// </summary>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<Expressions> ReadExpressionsConfigXMLByDB()
        {
            List<Expressions> listCPExpressions = new List<Expressions>();
            Expressions cpExpressions;

            try
            {

                //XmlReader xmlReader = XmlReader.Create("/XML/ExpressionsConfig.xml");
                //XDocument xDocument = XDocument.Load(xmlReader);
                XDocument xDocument = new XDocument();
                try
                {
                    string sql = string.Format("select * from CP_ExpressionXML");

                    DataTable dt = SqlHelper.ExecuteDataTable( sql);

                    if (dt.Rows.Count > 0)
                    {
                        Byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(dt.Rows[0][1].ToString());
                        xDocument = XDocument.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));
                    }
                }
                catch (Exception ex)
                {
                    ThrowException(ex);
                }

                XElement xExpressionsTypeElement = xDocument.Element(XName.Get("ExpressionsConfig"));
                //公式类型节点（ExpressionsGroupType）
                foreach (XElement xExpressionsElement in xExpressionsTypeElement.Elements())
                {                    
                    //公式类别
                    String strExpressionsType= xExpressionsElement.Attribute("value").Value;
                    //公式节点（Expressions）
                    foreach (XElement xExpressionsChildElement in xExpressionsElement.Elements())
                    {
                        cpExpressions = new Expressions();
                        //公式属性节点（ExpressionsName、ExpressionsProcess等）
                        foreach (XElement xChildElement in xExpressionsChildElement.Elements())
                        {
                            if (xChildElement.Name == "ExpressionsParameterGroup")
                            {
                                #region 输入参数控件实体
                                //参数控件属性实体
                                ParameterProperty modelParameterProperty = new ParameterProperty();

                                // 参数控件属性集节点（ExpressionsParameter）
                                foreach (XElement xParameterElement in xChildElement.Elements())
                                {
                                    //单个参数控件属性实体置空值
                                    modelParameterProperty = new ParameterProperty();
                                    //输入参提示数标内容节点（ExpressionsParameter）
                                    modelParameterProperty.LabelText = xParameterElement.Attribute("text").Value;

                                    //参数控件属性节点（Property）
                                    foreach (XElement xPropertyElement in xParameterElement.Elements())
                                    {
                                        if (xPropertyElement.Attribute("name").Value.ToLower() == "tag")//单个参数控件tag属性值，用于后台处理公式值替换
                                        {
                                            modelParameterProperty.Tag = xPropertyElement.Attribute("value").Value;
                                        }
                                        else//除Tag属性以外的节点
                                        {
                                            modelParameterProperty.AddProperty(xPropertyElement.Attribute("name").Value,
                                                           xPropertyElement.Attribute("value").Value);
                                        }
                                    }
                                    cpExpressions.AddExpressionsParameter(modelParameterProperty);
                                }
                                
                                #endregion
                            }
                            else
                            {
                                #region 部分公式属性
                                switch (xChildElement.Name.ToString().ToLower())
                                {
                                    case "expressionsname": //公式表达式
                                        {
                                            cpExpressions.ExpressionsGroupType = strExpressionsType;
                                            cpExpressions.ExpressionsName = xChildElement.Attribute("value").Value;
                                            break;
                                        }
                                    case "expressionsprocess"://后台处理公式表达式
                                        {
                                            cpExpressions.ExpressionsProcess = xChildElement.Attribute("value").Value;
                                            break;
                                        }
                                    case "expressionsreusltunit"://计算结果单位
                                        {
                                            cpExpressions.ExpressionsReusltUnit = xChildElement.Attribute("value").Value;
                                            break;
                                        }
                                    case "expressionsdescribe"://公式表达式描述
                                        {
                                            cpExpressions.ExpressionsDescribe = xChildElement.Attribute("value").Value;
                                            break;
                                        }
                                }
                                #endregion
                            }
                        }
                        listCPExpressions.Add(cpExpressions);
                    }
                       
                }

                

                return listCPExpressions;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }

        }
        #endregion
    }
}