using System;
using System.Net;
using System.Windows;
using System.Collections.Generic;
using System.Xml.Linq;
using System.Xml;
using System.IO;
using System.Collections.ObjectModel;

namespace YidanEHRApplication.Helpers
{
    public class WorkFlowReleation
    {

        /// <summary>
        /// 得到strEnFroceWorkFlow处理好的活动
        /// </summary>
        /// <param name="strEnFroceWorkFlow">实际执行的XML</param>
        /// <param name="strAllWorkFlow">路径XML</param>
        /// <returns></returns>
        public List<WorkFlowActivity> GetActivityDone(string strEnFroceWorkFlow,string strAllWorkFlow)
        {
            List<WorkFlowRule> workFlowRule = GetRule(strAllWorkFlow);
            List<WorkFlowActivity> workFlowActivity = GetActivity(strEnFroceWorkFlow);
            List<WorkFlowActivity> workFlowActivityAll = GetActivity(strAllWorkFlow);

            foreach (WorkFlowActivity entity in workFlowActivity)
            {
                List<WorkFlowActivity> activity = new List<WorkFlowActivity>();
                string strActivityId = entity.ActivityId;
                foreach (WorkFlowRule work in workFlowRule)
                {
                    if (work.BeginActivityId == strActivityId)
                    {
                        activity.Add(InitActivity(workFlowActivityAll, work.EndActivityId));
                    }
                }
                entity.NextCount = activity.Count;
                entity.NextActivities = activity;

            }
            return workFlowActivity;
        }
        /// <summary>
        /// 得到处理好的一个Rule
        /// </summary>
        /// <param name="strWorkFlow"></param>
        /// <returns></returns>
        public WorkFlowRule GetRuleDone(string strWorkFlow, string strPreActivityId, string strNextActivityId)
        {
            WorkFlowRule rule = new WorkFlowRule();
            List<WorkFlowRule> workFlowRule = GetRule(strWorkFlow);
            foreach (WorkFlowRule work in workFlowRule)
            {
                if (work.BeginActivityId == strPreActivityId && work.EndActivityId == strNextActivityId)
                {
                    rule = work;
                    break;
                }
            }
            return rule;
        }
        /// <summary>
        /// 得到XML里所有的RULE
        /// </summary>
        /// <param name="strWorkFlow"></param>
        /// <returns></returns>
        public List<WorkFlowRule> GetRule(string strWorkFlow)
        {
            List<WorkFlowRule> lEntity = new List<WorkFlowRule>();

            Byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(strWorkFlow);
            XDocument xele = XDocument.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));

            XElement xElements = xele.Element(XName.Get("WorkFlow"));
            XElement xElement = xElements.Element(XName.Get("Rules")); 
            foreach (XElement child in xElement.Elements())
            {
                WorkFlowRule workFlowRule = new WorkFlowRule(child);
                lEntity.Add(workFlowRule);
            }
            return lEntity;
        }
        private List<WorkFlowActivity> GetActivity(string strWorkFlow)
        {
            List<WorkFlowActivity> lEntity = new List<WorkFlowActivity>();

            Byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(strWorkFlow);

            XElement xelement = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));

            XDocument xele = XDocument.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));

            XElement xElements = xele.Element(XName.Get("WorkFlow"));
            XElement xElement = xElements.Element(XName.Get("Activitys"));
            foreach (XElement child in xElement.Elements())
            {
                WorkFlowActivity workFlowActivity = new WorkFlowActivity(child, xelement);
                lEntity.Add(workFlowActivity);
            }
            return lEntity;
        }

        private WorkFlowActivity InitActivity(List<WorkFlowActivity> workFlowActivity, string strActivityId)
        {
            WorkFlowActivity entity = new WorkFlowActivity();
            foreach (WorkFlowActivity work in workFlowActivity)
            {
                if (work.ActivityId == strActivityId)
                {
                    entity = work;
                    break;
                }
            }
            return entity;
        }
    }

    public class WorkFlowRule
    {
        private string m_UniqueID;
        /// <summary>
        /// id
        /// </summary>
        public string UniqueID
        {
            get
            {
                return m_UniqueID;
            }
            set
            {
                this.m_UniqueID = value;
            }
        }

        private string m_RuleID;
        /// <summary>
        /// RuleID
        /// </summary>
        public string RuleID
        {
            get
            {
                return m_RuleID;
            }
            set
            {
                this.m_RuleID = value;
            }
        }

        private string m_LineType;
        /// <summary>
        /// LineType
        /// </summary>
        public string LineType
        {
            get
            {
                return m_LineType;
            }
            set
            {
                this.m_LineType = value;
            }
        }  

        private string m_RuleName;
        /// <summary>
        /// RuleName
        /// </summary>
        public string RuleName
        {
            get
            {
                return m_RuleName;
            }
            set
            {
                this.m_RuleName = value;
            }
        }

        private string m_BeginActivityId;
        /// <summary>
        /// 前驱活动
        /// </summary>
        public string BeginActivityId
        {
            get
            {
                return m_BeginActivityId;
            }
            set
            {
                this.m_BeginActivityId = value;
            }
        }

        private string m_EndActivityId;
        /// <summary>
        /// 后续活动
        /// </summary>
        public string EndActivityId
        {
            get
            {
                return m_EndActivityId;
            }
            set
            {
                this.m_EndActivityId = value;
            }
        }

        private string m_BeginPointX;
        /// <summary>
        /// BeginPointX
        /// </summary>
        public string BeginPointX
        {
            get
            {
                return m_BeginPointX;
            }
            set
            {
                this.m_BeginPointX = value;
            }
        }   

        private string m_BeginPointY;
        /// <summary>
        /// BeginPointY
        /// </summary>
        public string BeginPointY
        {
            get
            {
                return m_BeginPointY;
            }
            set
            {
                this.m_BeginPointY = value;
            }
        }

        private string m_EndPointX;
        /// <summary>
        /// EndPointX
        /// </summary>
        public string EndPointX
        {
            get
            {
                return m_EndPointX;
            }
            set
            {
                this.m_EndPointX = value;
            }
        }    

        private string m_EndPointY;
        /// <summary>
        /// EndPointY
        /// </summary>
        public string EndPointY
        {
            get
            {
                return m_EndPointY;
            }
            set
            {
                this.m_EndPointY = value;
            }
        }

        private string m_ZIndex;
        /// <summary>
        /// ZIndex="20"
        /// </summary>
        public string ZIndex
        {
            get
            {
                return m_ZIndex;
            }
            set
            {
                this.m_ZIndex = value;
            }
        }

        private List<WorkFlowRule> items;
        public List<WorkFlowRule> Items
        {
            get
            {
                return this.items;
            }
            private set
            {
                this.items = value;
            }
        }

        public WorkFlowRule()
        { }
        public WorkFlowRule(XElement child)
        {
            this.UniqueID = child.Attribute("UniqueID").Value;
            this.RuleID = child.Attribute("RuleID").Value;
            this.RuleName = child.Attribute("RuleName").Value;
            this.LineType = child.Attribute("LineType").Value;
            this.BeginActivityId = child.Attribute("BeginActivityUniqueID").Value;
            this.EndActivityId = child.Attribute("EndActivityUniqueID").Value;

            this.BeginPointX = child.Attribute("BeginPointX").Value;
            this.BeginPointY = child.Attribute("BeginPointY").Value;
            this.EndPointX = child.Attribute("EndPointX").Value;
            this.EndPointY = child.Attribute("EndPointY").Value;
            this.ZIndex = child.Attribute("ZIndex").Value;
        }
    }


    public class WorkFlowActivity
    {
        private string m_Ljdm;
        /// <summary>
        /// Ljdm,路径基本信息
        /// </summary>
        public string Ljdm
        {
            get
            {
                return m_Ljdm;
            }
            set
            {
                this.m_Ljdm = value;
            }
        }

        private string m_LjID;
        /// <summary>
        /// ID，路径基本信息
        /// </summary>
        public string LjID
        {
            get
            {
                return m_LjID;
            }
            set
            {
                this.m_LjID = value;
            }
        }

        private string m_LjName;
        /// <summary>
        /// name，路径基本信息
        /// </summary>
        public string LjName
        {
            get
            {
                return m_LjName;
            }
            set
            {
                this.m_LjName = value;
            }
        }

        private string m_LjDescription;
        /// <summary>
        ///  Description，路径基本信息
        /// </summary>
        public string LjDescription
        {
            get
            {
                return m_LjDescription;
            }
            set
            {
                this.m_LjDescription = value;
            }
        }

        private string m_LjWidth;
        /// <summary>
        ///  Width，路径基本信息
        /// </summary>
        public string LjWidth
        {
            get
            {
                return m_LjWidth;
            }
            set
            {
                this.m_LjWidth = value;
            }
        }

        private string m_LjHeight;
        /// <summary>
        ///  Height，路径基本信息
        /// </summary>
        public string LjHeight
        {
            get
            {
                return m_LjHeight;
            }
            set
            {
                this.m_LjHeight = value;
            }
        }

        private string m_ActivityId;
        /// <summary>
        /// id
        /// </summary>
        public string ActivityId
        {
            get
            {
                return m_ActivityId;
            }
            set
            {
                this.m_ActivityId = value;
            }
        }

        private string m_ActivityType;
        /// <summary>
        /// type
        /// </summary>
        public string ActivityType
        {
            get
            {
                return m_ActivityType;
            }
            set
            {
                this.m_ActivityType = value;
            }
        }

        private string m_ActivityName;
        /// <summary>
        /// name
        /// </summary>
        public string ActivityName
        {
            get
            {
                return m_ActivityName;
            }
            set
            {
                this.m_ActivityName = value;
            }
        }


        private string m_PositionX;
        /// <summary>
        /// 位置X
        /// </summary>
        public string PositionX
        {
            get
            {
                return m_PositionX;
            }
            set
            {
                this.m_PositionX = value;
            }
        }

        private string m_PositionY;
        /// <summary>
        /// 位置Y
        /// </summary>
        public string PositionY
        {
            get
            {
                return m_PositionY;
            }
            set
            {
                this.m_PositionY = value;
            }
        }

        private string m_RepeatDirection;
        /// <summary>
        /// DIRECTION
        /// </summary>
        public string RepeatDirection
        {
            get
            {
                return m_RepeatDirection;
            }
            set
            {
                this.m_RepeatDirection = value;
            }
        }

        private string m_ZIndex;
        /// <summary>
        /// ZIndex
        /// </summary>
        public string ZIndex
        {
            get
            {
                return m_ZIndex;
            }
            set
            {
                this.m_ZIndex = value;
            }
        }

        private string m_EnforceTime;
        /// <summary>
        /// 执行时间
        /// </summary>
        public string EnforceTime
        {
            get
            {
                return m_EnforceTime;
            }
            set
            {
                this.m_EnforceTime = value;
            }
        }

        private int m_NextCount;
        /// <summary>
        /// 后续活动数
        /// </summary>
        public int NextCount
        {
            get
            {
                return m_NextCount;
            }
            set
            {
                this.m_NextCount = value;
            }
        }

        private List<WorkFlowActivity> m_NextActivities;
        /// <summary>
        /// 后续活动
        /// </summary>
        public List<WorkFlowActivity> NextActivities
        {
            get
            {
                return this.m_NextActivities;
            }
            set
            {
                this.m_NextActivities = value;
            }
        }

        public WorkFlowActivity()
        { }
        public WorkFlowActivity(XElement activity,XElement mainInfo)
        { 
            //活动信息
            this.ActivityId = activity.Attribute("UniqueID").Value;
            this.ActivityType = activity.Attribute("Type").Value;
            this.ActivityName = activity.Attribute("ActivityName").Value;
            this.PositionX = activity.Attribute("PositionX").Value;
            this.PositionY = activity.Attribute("PositionY").Value;
            this.RepeatDirection = activity.Attribute("RepeatDirection").Value;
            this.ZIndex = activity.Attribute("ZIndex").Value;
            try
            {
                this.EnforceTime = activity.Attribute("EnForceTime").Value;
            }
            catch
            {
                this.EnforceTime = string.Empty; 
            }

            //主路径信息
            this.Ljdm = mainInfo.Attribute(XName.Get("UniqueID")).Value;
            this.LjID = mainInfo.Attribute(XName.Get("ID")).Value;
            this.LjName = mainInfo.Attribute(XName.Get("Name")).Value;
            this.LjDescription = mainInfo.Attribute(XName.Get("Description")).Value;
            this.LjWidth = mainInfo.Attribute(XName.Get("Width")).Value;
            this.LjHeight = mainInfo.Attribute(XName.Get("Height")).Value; 
        }
    }
}
