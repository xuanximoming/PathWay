using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Xml.Linq;
using System.Collections.Generic;
using System.IO;
using System.Collections.ObjectModel;

namespace YidanEHRApplication.YidanEHRServiceReference
{
    //public class CopyPathInfo
    //{
    //    /// <summary>
    //    /// 得到XML里所有的RULE
    //    /// </summary>
    //    /// <param name="strWorkFlow"></param>
    //    /// <returns></returns>
    //    public ObservableCollection<CopyWorkFlowRule> GetCopyRule(string strWorkFlow)
    //    {
    //        ObservableCollection<CopyWorkFlowRule> lEntity = new ObservableCollection<CopyWorkFlowRule>();

    //        Byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(strWorkFlow);
    //        XDocument xele = XDocument.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));

    //        XElement xElements = xele.Element(XName.Get("WorkFlow"));
    //        XElement xElement = xElements.Element(XName.Get("Rules"));
    //        foreach (XElement child in xElement.Elements())
    //        {
    //            CopyWorkFlowRule workFlowRule = new CopyWorkFlowRule(child);
    //            lEntity.Add(workFlowRule);
    //        }
    //        return lEntity;
    //    }

    //    /// <summary>
    //    /// 得到ACTIVITY集合
    //    /// </summary>
    //    /// <param name="strWorkFlow"></param>
    //    /// <returns></returns>
    //    public ObservableCollection<CopyWorkFlowActivity> GetCopyActivity(string strWorkFlow)
    //    {
    //        ObservableCollection<CopyWorkFlowActivity> lEntity = new ObservableCollection<CopyWorkFlowActivity>();

    //        Byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(strWorkFlow);

    //        XElement xelement = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));

    //        XDocument xele = XDocument.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));

    //        XElement xElements = xele.Element(XName.Get("WorkFlow"));
    //        XElement xElement = xElements.Element(XName.Get("Activitys"));
    //        foreach (XElement child in xElement.Elements())
    //        {
    //            CopyWorkFlowActivity workFlowActivity = new CopyWorkFlowActivity(child, xelement);
    //            lEntity.Add(workFlowActivity);
    //        }
    //        return lEntity;
    //    }
    //}

    //public partial class CopyWorkFlowRule
    //{

    //    public CopyWorkFlowRule(XElement child)
    //    {
    //        this.UniqueID = child.Attribute("UniqueID").Value;
    //        this.RuleID = child.Attribute("RuleID").Value;
    //        this.RuleName = child.Attribute("RuleName").Value;
    //        this.LineType = child.Attribute("LineType").Value;
    //        this.BeginActivityId = child.Attribute("BeginActivityUniqueID").Value;
    //        this.EndActivityId = child.Attribute("EndActivityUniqueID").Value;

    //        this.NewUniqueID = child.Attribute("UniqueID").Value;
    //        this.NewRuleID = child.Attribute("RuleID").Value;
    //        this.NewBeginActivityId = child.Attribute("BeginActivityUniqueID").Value;
    //        this.NewEndActivityId = child.Attribute("EndActivityUniqueID").Value;

    //        this.BeginPointX = child.Attribute("BeginPointX").Value;
    //        this.BeginPointY = child.Attribute("BeginPointY").Value;
    //        this.EndPointX = child.Attribute("EndPointX").Value;
    //        this.EndPointY = child.Attribute("EndPointY").Value;
    //        this.ZIndex = child.Attribute("ZIndex").Value;
    //    }
    //}

    //public partial class CopyWorkFlowActivity
    //{

    //    public CopyWorkFlowActivity(XElement activity, XElement mainInfo)
    //    {
    //        //活动信息
    //        this.UniqueID = activity.Attribute("UniqueID").Value;
    //        this.ActivityId = activity.Attribute("ActivityID").Value;
    //        this.ActivityName = activity.Attribute("ActivityName").Value;
    //        this.Type = activity.Attribute("Type").Value;
    //        this.SubFlow = activity.Attribute("SubFlow").Value;
    //        this.PositionX = activity.Attribute("PositionX").Value;
    //        this.PositionY = activity.Attribute("PositionY").Value;
    //        this.RepeatDirection = activity.Attribute("RepeatDirection").Value;
    //        this.ZIndex = activity.Attribute("ZIndex").Value;
    //        this.NewUniqueID = activity.Attribute("UniqueID").Value;
    //        this.EnForceTime = activity.Attribute("EnForceTime").Value;

    //        //主路径信息
    //        this.Ljdm = mainInfo.Attribute(XName.Get("UniqueID")).Value;
    //        this.LjID = mainInfo.Attribute(XName.Get("ID")).Value;
    //        this.LjName = mainInfo.Attribute(XName.Get("Name")).Value;
    //        this.LjDescription = mainInfo.Attribute(XName.Get("Description")).Value;
    //        this.LjWidth = mainInfo.Attribute(XName.Get("Width")).Value;
    //        this.LjHeight = mainInfo.Attribute(XName.Get("Height")).Value;

    //        this.NewLjdm = mainInfo.Attribute(XName.Get("UniqueID")).Value;
    //        this.NewLjID = mainInfo.Attribute(XName.Get("ID")).Value;
    //        this.NewLjName = mainInfo.Attribute(XName.Get("Name")).Value;
    //    }
    //}

}
