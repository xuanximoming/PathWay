using System;
using System.Net;

using System.Collections.Generic;
using System.IO;
using System.Xml.Linq;
using System.Linq;
namespace YidanSoft.Tool
{
    public class WorkFlowTool
    {
        /// <summary>
        /// 分析流程图XML获取每个节点的类型、UniqueID、前置和后置节点UniqueID
        /// </summary>
        /// <param name="XMLStr"></param>
        /// <returns></returns>
        public static MyActivitys AnalyseFlowXMLNode(String XMLStr)
        {
            Byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(XMLStr);
            XElement xele = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));
            //UniqueID = xele.Attribute(XName.Get("UniqueID")).Value;
            var partNos = from item in xele.Descendants("Activity") select item;
            //开始节点
            MyActivity ActivityBegin = new MyActivity();
            //结束节点
            MyActivity ActivityEnd = new MyActivity();
            //全部节点
            MyActivitys MyActivityAll = new MyActivitys();
            foreach (var node in partNos)
            {
                MyActivity a = new MyActivity();
                a.ActivityType = node.Attribute(XName.Get("Type")).Value;
                a.UniqueID = node.Attribute("UniqueID").Value;
                a.ActivityName = node.Attribute("ActivityName").Value;
                a.CurrentElementState = node.Attribute("CurrentElementState").Value;

                if (a.ActivityType == "INITIAL")
                {
                    ActivityBegin = a;
                }
                if (a.ActivityType == "COMPLETION")
                {
                    ActivityEnd = a;
                }
                MyActivityAll.Add(a);
            }

            var Rules = from item in xele.Descendants("Rule") select item;
            //全部规则
            List<MyRule> RulesAll = new List<MyRule>();
            foreach (var item in Rules)
            {
                MyRule ruleTemp = new MyRule();
                ruleTemp.BeginNodeUniqueID = item.Attribute("BeginActivityUniqueID").Value;
                ruleTemp.EndNodeUniqueID = item.Attribute("EndActivityUniqueID").Value;
                RulesAll.Add(ruleTemp);
            }
            MyRule.MyRuleALL = RulesAll;
            MyActivitys ActivitysResult = new MyActivitys();
            //遍历所有规则
            foreach (var item in RulesAll)
            {
                //添加头节点
                if (item.BeginNodeUniqueID == ActivityBegin.UniqueID)
                {
                    ActivityBegin.NestNodeUniqueID = item.EndNodeUniqueID;
                    ActivitysResult.Add(ActivityBegin);
                }
                //添加尾节点
                if (item.EndNodeUniqueID == ActivityEnd.UniqueID)
                {
                    ActivityEnd.PreNodeUniqueID = item.BeginNodeUniqueID;
                    ActivitysResult.Add(ActivityEnd);
                }

                foreach (var item2 in RulesAll)
                {
                    //如果两条规则的头尾相接，那么说明两条规则连接处的节点有前置和后置节点
                    if (item.EndNodeUniqueID == item2.BeginNodeUniqueID)
                    {
                        MyActivity MyActivityTemp = new MyActivity();
                        MyActivityTemp.UniqueID = item.EndNodeUniqueID;
                        MyActivityTemp.PreNodeUniqueID = item.BeginNodeUniqueID;
                        MyActivityTemp.NestNodeUniqueID = item2.EndNodeUniqueID;
                        MyActivityTemp.ActivityType = (MyActivityAll.Select(s => s).Where(s => s.UniqueID == MyActivityTemp.UniqueID).ToList())[0].ActivityType;
                        MyActivityTemp.ActivityName = (MyActivityAll.Select(s => s).Where(s => s.UniqueID == MyActivityTemp.UniqueID).ToList())[0].ActivityName;
                        MyActivityTemp.CurrentElementState = (MyActivityAll.Select(s => s).Where(s => s.UniqueID == MyActivityTemp.UniqueID).ToList())[0].CurrentElementState;
                        ActivitysResult.Add(MyActivityTemp);
                    }
                }
            }
            return ActivitysResult;
        }
        /// <summary>
        /// 分析流程图XML获取执行过的节点的类型、UniqueID、前置和后置节点UniqueID
        /// </summary>
        /// <param name="XMLStr"></param>
        /// <returns></returns>
        public static MyActivitys AnalyseFlowXMLExecuteNode(String XMLStr)
        {
            Byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(XMLStr);
            XElement xele = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));
            var partNos = from item in xele.Descendants("Activity") select item;
            //开始节点
            MyActivity ActivityBegin = new MyActivity();
            //结束节点
            MyActivity ActivityEnd = new MyActivity();

            //全部节点
            MyActivitys MyActivityAll = new MyActivitys();
            //执行过的节点
            MyActivitys MyActivityExecute = new MyActivitys();
            foreach (var node in partNos)
            {
                MyActivity a = new MyActivity();
                a.ActivityType = node.Attribute(XName.Get("Type")).Value;
                a.UniqueID = node.Attribute("UniqueID").Value;
                a.ActivityName = node.Attribute("ActivityName").Value;
                if (a.ActivityType == "INITIAL")
                {
                    ActivityBegin = a;
                }
                if (a.ActivityType == "COMPLETION")
                {
                    ActivityEnd = a;
                }
               
            }

          
            MyRule.MyRuleALL = new List<MyRule>();
            MyActivityAll = AnalyseFlowXMLNode(XMLStr);     //Rule 在此时也获得值  zm 8.8
            int flag = 10000;
            while (MyActivityAll.Count > MyActivityExecute.Count)       //排完执行节点后，空转
            {
                flag = flag - 1;
                foreach (MyRule MyRuleItem in MyRule.MyRuleALL)
                {

                    if (MyRuleItem.BeginNodeUniqueID == ActivityBegin.UniqueID)
                    {
                        //添加开始节点到MyActivityExecute
                        MyActivity MyActivityTemp = MyActivityAll[MyRuleItem.BeginNodeUniqueID];
                        if (MyActivityTemp != null && MyActivityTemp.CurrentElementState == ToolElementState.Pre.ToString())
                        {
                            MyActivityExecute.Add(MyActivityTemp);
                        }
                        //添加第二个节点到MyActivityExecute
                        MyActivity MyActivityTemp2 = MyActivityAll[MyRuleItem.EndNodeUniqueID];
                        if (MyActivityTemp2 != null && MyActivityTemp2.CurrentElementState == ToolElementState.Pre.ToString())
                        {
                            MyActivityExecute.Add(MyActivityTemp2);
                        }

                    }
             
                    
                    if (MyActivityExecute.Count >0 && MyRuleItem.BeginNodeUniqueID == MyActivityExecute[MyActivityExecute.Count-1].UniqueID)
                    {
                        //添加MyActivityExecute最后一个节点所在rule的另一头的节点
                        MyActivity MyActivityTemp2 = MyActivityAll[MyRuleItem.EndNodeUniqueID];
                        if (MyActivityTemp2 != null && (MyActivityTemp2.CurrentElementState == ToolElementState.Pre.ToString() || MyActivityTemp2.CurrentElementState==ToolElementState.Now.ToString()))
                        {
                            MyActivityExecute.Add(MyActivityTemp2);
                        }
                    }
                   
                }

                if (flag == 0)
                    break;
                

            }

            return MyActivityExecute;
        }
        /// <summary>
        /// zm 8.8 读取所有子节点
        /// </summary>
        public static List<MyActiveChildren> AnalyseFlowXMLExecuteChildNode(String XMLStr) 
        {
            Byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(XMLStr);
            XElement xele = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));

            var childrens = from item in xele.Descendants("ActiveChildren") select item;                //读取子节点

            List<MyActiveChildren> list = new List<MyActiveChildren>();

            foreach (var children in childrens)                                                          //遍历子节点
            {
                MyActiveChildren c = new MyActiveChildren();
                c.ActivityChildrenID = children.Attribute("ActivityChildrenID").Value;
                c.ActivityUniqueID = children.Attribute("ActivityUniqueID").Value;
                c.EnForceTime = children.Attribute("EnForceTime").Value;

                list.Add(c);
            }

            return list;
        }
     
    }  
    public class MyRule
    {
        public static List<MyRule> MyRuleALL = new List<MyRule>();
        public String UniqueID { get; set; }
        public String BeginNodeUniqueID { get; set; }
        public String EndNodeUniqueID { get; set; }
        public String BeginType { get; set; }
        public String EndType { get; set; }
        public String RuleID { get; set; }              //6.2添加
    }
    public class MyActivity
    {
        public String OldUniqueID { get; set; }             //6.2添加，裁剪前ID
        public String WorkFlowUniqueID { get; set; }        //6.2添加,路径代码
        public String UniqueID { get; set; }
        public String PreNodeUniqueID { get; set; }
        public String NestNodeUniqueID { get; set; }
        public String ActivityType { get; set; }
        public String ActivityName { get; set; }
        public String CurrentElementState { get; set; }
    }
    public class MyActiveChildren               //6.2添加
    {
        public String ActivityUniqueID { get; set; }        //父节点值       zm 8.8
        public String ActivityChildrenID { get; set; }      //子节点真正标识 zm 8.8
        public String EnForceTime { get; set; }             //完成时间      zm 8.8
        public String Jrsj { get; set; }                    //执行时间      zm 8.8
        public String CurrentElementState { get; set; }     //状态            zm 8.8
    }
    public class MyActivitys : List<MyActivity>
    {
        private MyActivitys _ExecuteMyActivitys;
        public MyActivitys ExecuteMyActivitys
        {
            get
            {
                _ExecuteMyActivitys = new MyActivitys();
                foreach (MyActivity MyActivityTemp in this)
                {
                    if (MyActivityTemp.CurrentElementState == ToolElementState.Pre.ToString())
                    {
                        _ExecuteMyActivitys.Add(MyActivityTemp);
                    }
                }
                return _ExecuteMyActivitys;
            }
        }
        public MyActivity this[String uniqueID]
        {
            get
            {
                MyActivity MyActivityTemp = null;
                foreach (MyActivity MyActivityItem in this)
                {
                    if (MyActivityItem.UniqueID == uniqueID)
                    {
                        MyActivityTemp = MyActivityItem;
                    }
                }
                return MyActivityTemp;
            }
        }
        public void Add(MyActivity myActivity)
        {
            if (this[myActivity.UniqueID] == null)
            {
                base.Add(myActivity);
            }
        }
    }

}
