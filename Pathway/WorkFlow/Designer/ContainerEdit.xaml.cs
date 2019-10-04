using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Shapes;
using System.Xml.Linq;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using YidanEHRApplication.Views;
using YidanEHRApplication.WorkFlow.Component;
using YidanEHRApplication.WorkFlow.Resources;

namespace YidanEHRApplication.WorkFlow.Designer
{

    public partial class ContainerEdit : UserControl, IContainer
    {
        public event LoadCompletedHandler LoadCompleted;
        /// <summary>
        /// 该变量解决RadTabControl切换的时候调用本类的load事件的问题
        /// </summary>
        Boolean IsPostBack = false;
        #region Members
        /// <summary>
        /// 此处需要修改,根据点击配置路径传入此值
        /// </summary>
        public string WorkFlowUrlID
        {
            get;
            set;
        }
        /// <summary>
        /// 此处需要修改,根据点击配置路径传入此值
        /// </summary>
        public string WorkFlowUrlName
        {
            get;
            set;
        }


        PageEditType editType = PageEditType.None;
        public PageEditType EditType
        {
            get
            {
                if (editType == PageEditType.None)
                {
                    editType = PageEditType.Add;
                }
                return editType;
            }
            set
            {
                editType = value;
            }
        }

        int nextNewActivityIndex = 0;
        public int NextNewActivityIndex
        {
            get
            {
                return ++nextNewActivityIndex;
            }
        }
        int nextNewRuleIndex = 0;
        public int NextNewRuleIndex
        {
            get
            {
                return ++nextNewRuleIndex;
            }
        }
        int nextNewLabelIndex = 0;
        public int NextNewLabelIndex
        {
            get
            {
                return ++nextNewLabelIndex;
            }
        }


        string _uniqueID;
        public string UniqueID
        {
            get
            {
                if (string.IsNullOrEmpty(_uniqueID))
                {
                    _uniqueID = Guid.NewGuid().ToString();
                }
                return _uniqueID;
            }
            set
            {
                _uniqueID = value;
            }

        }
        string _workFlowXML;
        public string WorkFlowXML
        {
            get
            {
                return _workFlowXML;
            }
            set
            {
                _workFlowXML = value;
            }

        }
        public Double ContainerWidth
        {
            get
            {
                return cnsDesignerContainer.Width;
            }
            set
            {
                cnsDesignerContainer.Width = value;
                sliWidth.Value = value;
            }
        }
        public Double ContainerHeight
        {
            get
            {
                return cnsDesignerContainer.Height;
            }
            set
            {
                cnsDesignerContainer.Height = value;
                sliHeight.Value = value;

            }
        }

        public Double ScrollViewerHorizontalOffset
        {
            get
            {
                return svContainer.HorizontalOffset;
            }
            set
            {
                svContainer.ScrollToHorizontalOffset(value);

            }
        }
        public Double ScrollViewerVerticalOffset
        {
            get
            {
                return svContainer.VerticalOffset;
            }
            set
            {
                svContainer.ScrollToVerticalOffset(value);
            }
        }



        private Activitys _activityCollections;
        public Activitys ActivityCollections
        {
            get
            {
                if (_activityCollections == null)
                {
                    _activityCollections = new Activitys();
                }
                return _activityCollections;
            }
        }
        private Rules _ruleCollections;
        public Rules RuleCollections
        {
            get
            {
                if (_ruleCollections == null)
                {
                    _ruleCollections = new Rules();
                }
                return _ruleCollections;
            }
        }
        int nextMaxIndex = 0;
        public int NextMaxIndex
        {
            get
            {
                nextMaxIndex++;
                return nextMaxIndex;
            }
        }

        public double Left
        {
            get
            {
                return 155;
            }
        }
        public double Top
        {
            get
            {
                return 40;
            }

        }

        public Rule CurrentTemporaryRule
        {
            get;
            set;
        }

        private bool m_IsEditEnable = true;
        /// <summary>
        /// 是否可编辑
        /// </summary>
        public bool IsEditEnable
        {
            get { return m_IsEditEnable; }
            set { m_IsEditEnable = value; }
        }

        #endregion Members

        public ContainerEdit()
        {
            InitializeComponent();
        }
        public bool isLoad = true;
        /// <summary>
        /// LOAD 初始化
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isLoad)
                {
                    //isLoad = true;
                    return;
                }

                if (IsPostBack) { return; }
                else { IsPostBack = true; }
                //System.Windows.Browser.HtmlPage.RegisterScriptableObject("WorkFlowDesignerContainer", this);  
                sliZoom.Value = 1;
                MessageBody.Visibility = Visibility.Collapsed;
                XmlContainer.Visibility = Visibility.Collapsed;
                siActivitySetting.Visibility = Visibility.Collapsed;
                siRuleSetting.Visibility = Visibility.Collapsed;

                if (true)
                {
                    svContainer.Width = this.Width - 200;
                    cnsDesignerContainer.Width = this.Width;
                    svContainer.Height = 200;
                    cnsDesignerContainer.Height = 500;
                    sliWidth.Value = Browser.ScreenWidth - 200;// cnsDesignerContainer.Width;
                    sliHeight.Value = cnsDesignerContainer.Height;
                    stackPanelEdit.Visibility = System.Windows.Visibility.Visible;
                }
                //else
                //{
                //    stackPanelEdit.Visibility = System.Windows.Visibility.Collapsed;
                //}

                btnPrevious.IsEnabled = false;
                btnNext.IsEnabled = false;
                menuActivity.Container = this;
                menuLabel.Container = this;
                menuRule.Container = this;
                menuRule.Visibility = Visibility.Collapsed;
                menuActivity.Visibility = Visibility.Collapsed;
                menuLabel.Visibility = Visibility.Collapsed;
                menuContainer.Visibility = Visibility.Collapsed;
                menuContainer.Container = this;
                SetGridLines();

                System.Windows.Browser.HtmlPage.Document.AttachEvent("oncontextmenu", OnContextMenu);
                ApplyCulture();

                if (Configure.CurrentCulture.Name.ToLower() == "zh-cn")
                {
                    //btnApplyChineseCulture.IsEnabled = false;
                    //btnApplyEnglishCulture.IsEnabled = true;

                }
                if (Configure.CurrentCulture.Name.ToLower() == "en-us")
                {
                    //btnApplyChineseCulture.IsEnabled = true;
                    //btnApplyEnglishCulture.IsEnabled = false;
                }

                _doubleClickTimer = new System.Windows.Threading.DispatcherTimer();
                _doubleClickTimer.Interval = new TimeSpan(0, 0, 0, 0, 200);
                _doubleClickTimer.Tick += new EventHandler(DoubleClick_Timer);
                SetControlEnable(IsEditEnable);
                CreateNewWorkFlow();
                isLoad = false;

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        /// <summary>
        /// 是否可以编辑
        /// </summary>
        /// <param name="isEnable"></param>
        private void SetControlEnable(bool isEnable)
        {
            btnAddActivity.IsEnabled = IsEditEnable;
            btnSave.IsEnabled = IsEditEnable;
            btnClearContainer.IsEnabled = IsEditEnable;
            if (!IsEditEnable)
                cnsDesignerContainer.MouseLeftButtonDown -= new MouseButtonEventHandler(Container_MouseLeftButtonDown);
        }

        private void DoubleClick_Timer(object sender, EventArgs e)
        {
            _doubleClickTimer.Stop();
        }

        /// <summary>
        /// ACTIVITY 的基本信息设置,弹出的窗体
        /// </summary>
        /// <param name="a"></param>
        public void ShowActivitySetting(Activity a)
        {
            siActivitySetting.SetSetting(a);
            // sbActivitySettingDisplay.Begin();
        }

        /// <summary>
        /// RULE  的基本信息设置,弹出的窗体
        /// </summary>
        /// <param name="r"></param>
        public void ShowRuleSetting(Rule r)
        {
            siRuleSetting.Visibility = Visibility.Visible;
            siRuleSetting.SetSetting(r);
        }



        public void AddRule(Rule r)
        {
            if (!cnsDesignerContainer.Children.Contains(r))
            {
                cnsDesignerContainer.Children.Add(r);
                r.Container = this;

                r.RuleChanged += new RuleChangeDelegate(OnRuleChanged);
            }
            if (!RuleCollections.Contains(r))
            {
                RuleCollections.Add(r);
            }

        }

        public void AddLabel(Label l)
        {
            if (!cnsDesignerContainer.Children.Contains(l))
            {
                cnsDesignerContainer.Children.Add(l);
            }
        }

        public void RemoveRule(Rule r)
        {

            if (cnsDesignerContainer.Children.Contains(r))
            {
                cnsDesignerContainer.Children.Remove(r);
            }
            if (RuleCollections.Contains(r))
                RuleCollections.Remove(r);
        }



        private void OnRuleChanged(Rule a)
        {
            SaveChange(HistoryType.New);
        }

        public void OnActivityChanged(Activity a)
        {
            SaveChange(HistoryType.New);

        }

        /// <summary>
        /// 初始化路径
        /// </summary>
        private void CreateNewWorkFlow()
        {
            if (!string.IsNullOrEmpty(WorkFlowUrlID))
            {
                Display(WorkFlowXML);
            }
            else
            {
                #region 手动数据,新逻辑里不会用到
                string beginActivityID = Guid.NewGuid().ToString();
                string endActivityID = Guid.NewGuid().ToString();
                string rule1ID = Guid.NewGuid().ToString();
                string rule2ID = Guid.NewGuid().ToString();
                string activtyID = Guid.NewGuid().ToString();
                string workflowID = Guid.NewGuid().ToString();

                string xml = @"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes"" ?>
                                <WorkFlow UniqueID=""" + workflowID + @""" ID="""" Name="""" Description="""" Width=""980"" Height=""580"">
                                    <Activitys>
                                       <Activity  UniqueID=""" + beginActivityID + @""" ActivityID="""" ActivityName="""" Type=""INITIAL"" SubFlow="""" PositionX=""212"" PositionY=""126"" RepeatDirection=""None"" ZIndex=""24"">
                                        </Activity>
                                       <Activity  UniqueID=""" + endActivityID + @""" ActivityID="""" ActivityName="""" Type=""COMPLETION"" SubFlow="""" PositionX=""516"" PositionY=""124"" RepeatDirection=""None"" ZIndex=""25"">
                                        </Activity>
                                       <Activity  UniqueID=""" + workflowID + @""" ActivityID="""" ActivityName=""shareidea.net"" Type=""INTERACTION"" SubFlow="""" PositionX=""368"" PositionY=""124"" RepeatDirection=""None"" ZIndex=""20"">
                                        </Activity>
                                    </Activitys>
                                    <Rules>
                                       <Rule  UniqueID=""" + rule1ID + @""" RuleID=""" + rule1ID + @""" RuleName="""" LineType=""Line"" RuleCondition="""" BeginActivityUniqueID=""" + beginActivityID + @""" EndActivityUniqueID=""" + workflowID + @""" BeginActivityID="""" EndActivityID="""" BeginPointX=""235"" BeginPointY=""121"" EndPointX=""314"" EndPointY=""120"" TurnPoint1X=""0"" TurnPoint1Y=""0"" TurnPoint2X=""0"" TurnPoint2Y=""0"" ZIndex=""18"">
                                        </Rule>
                                       <Rule  UniqueID=""" + rule2ID + @""" RuleID=""" + rule2ID + @""" RuleName="""" LineType=""Line"" RuleCondition="""" BeginActivityUniqueID=""" + workflowID + @""" EndActivityUniqueID=""" + endActivityID + @""" BeginActivityID="""" EndActivityID="""" BeginPointX=""414"" BeginPointY=""120"" EndPointX=""485"" EndPointY=""119"" TurnPoint1X=""0"" TurnPoint1Y=""0"" TurnPoint2X=""0"" TurnPoint2Y=""0"" ZIndex=""17"">
                                        </Rule>
                                    </Rules>
                                </WorkFlow>";
                Display(xml);
                #endregion
            }

        }

        //private void Client_GetWorkFlowXMLCompleted(object sender, GetWorkFlowXMLCompletedEventArgs e)
        //{
        //    Display(e.Result.ToString());
        //}

        /// <summary>
        /// 显示新生成的flow
        /// </summary>
        /// <param name="xml"></param>
        private void Display(string xml)
        {
            lbWorkFlowName.Content = txtWorkFlowName.Text = WorkFlowUrlName;
            LoadFromXmlString(xml);
            SaveChange(HistoryType.New);
        }

        public void LoadFromXmlString(string xml)
        {
            if (string.IsNullOrEmpty(xml))
                return;
            ActivityType activityType;
            MergePictureRepeatDirection repeatDirection = MergePictureRepeatDirection.None;
            string uniqueID = "";
            int zIndex = 0;
            string activityID = "";
            string activityName = "";
            Point activityPosition = new Point();
            double temd = 0;
            Byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(xml);
            XElement xele = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));
            UniqueID = xele.Attribute(XName.Get("UniqueID")).Value;

            var partNos = from item in xele.Descendants("Activity") select item;

            //判断是否有NOW结点，没有的话将初始结点赋值为NOW
            var partNowNos = from item in xele.Descendants("Activity")
                             where item.Attribute(XName.Get("CurrentElementState")).Value.Equals(ElementState.Now.ToString())
                             select item;
            int countNow = partNowNos.Count();

            foreach (XElement node in partNos)
            {

                activityType = (ActivityType)Enum.Parse(typeof(ActivityType), node.Attribute(XName.Get("Type")).Value, true);
                try
                {
                    repeatDirection = (MergePictureRepeatDirection)Enum.Parse(typeof(MergePictureRepeatDirection), node.Attribute(XName.Get("RepeatDirection")).Value, true);

                }
                catch (Exception e) { }
                uniqueID = node.Attribute(XName.Get("UniqueID")).Value;
                activityID = node.Attribute(XName.Get("ActivityID")).Value;
                activityName = node.Attribute(XName.Get("ActivityName")).Value;

                double.TryParse(node.Attribute(XName.Get("PositionX")).Value, out temd);
                activityPosition.X = temd;
                double.TryParse(node.Attribute(XName.Get("PositionY")).Value, out temd);
                activityPosition.Y = temd;
                int.TryParse(node.Attribute(XName.Get("ZIndex")).Value, out zIndex);

                Activity a = new Activity((IContainer)this, activityType);
                a.SubFlow = node.Attribute(XName.Get("SubFlow")).Value;
                a.RepeatDirection = repeatDirection;
                a.CenterPoint = activityPosition;
                a.ActivityID = activityID;
                a.ActivityName = activityName;
                a.ZIndex = zIndex;
                a.EditType = this.EditType;
                a.UniqueID = uniqueID;

                #region 新增属性
                foreach (XElement infos in node.Elements())
                {
                    ActiveChildren children = new ActiveChildren();
                    children.ActivityUniqueID = infos.Attribute("ActivityUniqueID").Value;
                    children.ActivityChildrenID = infos.Attribute("ActivityChildrenID").Value;
                    children.CurrentElementState = (ElementState)Enum.Parse(typeof(ElementState), infos.Attribute("CurrentElementState").Value, true);
                    children.EnForceTime = infos.Attribute("EnForceTime").Value;

                    a.ActiveChildrens.Add(children);
                }
                #region laolaowhn
                if (a.ActiveChildrens != null && a.Type == ActivityType.AUTOMATION)
                    a.CurrentStep = a.ActiveChildrens.Count.ToString() + "/" + a.ActiveChildrens.Count.ToString();
                #endregion
                if (countNow == 0 && activityType == ActivityType.INITIAL)
                    a.CurrentElementState = ElementState.Now;
                else
                    a.CurrentElementState = (ElementState)Enum.Parse(typeof(ElementState), node.Attribute("CurrentElementState").Value, true);
                a.Flow.UniqueID = node.Attribute("WorkFlowUniqueID").Value;

                #endregion

                AddActivity(a);
                if (CanRegisterEvents(a))
                    a.ActivitySelectEvent += new ActivitySelectEventHandler(Activity_ActivitySelectEvent);


            }

            string beginActivityID = "";
            string endActivityID = "";
            double beginPointX = 0;
            double beginPointY = 0;
            double endPointX = 0;
            double endPointY = 0;
            double turnPoint1X = 0;
            double turnPoint1Y = 0;
            double turnPoint2X = 0;
            double turnPoint2Y = 0;

            string ruleID = "";
            string ruleName = "";
            string beginActivityUniqueID = "";
            string endActivityUniqueID = "";
            double containerWidth = 0;
            double containerHeight = 0;
            RuleLineType lineType = RuleLineType.Line;
            double.TryParse(xele.Attribute(XName.Get("Width")).Value, out containerWidth);
            double.TryParse(xele.Attribute(XName.Get("Height")).Value, out containerHeight);


            ContainerWidth = containerWidth;
            ContainerHeight = containerHeight;


            Activity temActivity = null;
            partNos = from item in xele.Descendants("Rule") select item;
            foreach (XElement node in partNos)
            {
                lineType = (RuleLineType)Enum.Parse(typeof(RuleLineType), node.Attribute(XName.Get("LineType")).Value, true);

                uniqueID = node.Attribute(XName.Get("UniqueID")).Value;

                ruleID = node.Attribute(XName.Get("RuleID")).Value;
                ruleName = node.Attribute(XName.Get("RuleName")).Value;
                beginActivityUniqueID = node.Attribute(XName.Get("BeginActivityUniqueID")).Value;
                endActivityUniqueID = node.Attribute(XName.Get("EndActivityUniqueID")).Value;

                beginActivityID = node.Attribute(XName.Get("BeginActivityID")).Value;
                endActivityID = node.Attribute(XName.Get("EndActivityID")).Value;


                double.TryParse(node.Attribute(XName.Get("TurnPoint1X")).Value, out turnPoint1X);
                double.TryParse(node.Attribute(XName.Get("TurnPoint1Y")).Value, out turnPoint1Y);
                double.TryParse(node.Attribute(XName.Get("TurnPoint2X")).Value, out turnPoint2X);
                double.TryParse(node.Attribute(XName.Get("TurnPoint2Y")).Value, out turnPoint2Y);

                double.TryParse(node.Attribute(XName.Get("BeginPointX")).Value, out beginPointX);
                double.TryParse(node.Attribute(XName.Get("BeginPointY")).Value, out beginPointY);
                double.TryParse(node.Attribute(XName.Get("EndPointX")).Value, out endPointX);
                double.TryParse(node.Attribute(XName.Get("EndPointY")).Value, out endPointY);

                int.TryParse(node.Attribute(XName.Get("ZIndex")).Value, out zIndex);


                Rule r = new Rule(this, false, lineType);
                AddRule(r);
                r.RuleID = ruleID;
                r.RuleName = ruleName;
                r.ZIndex = zIndex;
                r.EditType = this.EditType;
                r.UniqueID = uniqueID;
                r.LineType = lineType;
                if (turnPoint1X > 0 && turnPoint2X > 0)
                {
                    r.TurnPoint1HadMoved = true;
                    r.TurnPoint2HadMoved = true;
                    r.RuleTurnPoint1.CenterPosition = new Point(turnPoint1X, turnPoint1Y);
                    r.RuleTurnPoint2.CenterPosition = new Point(turnPoint2X, turnPoint2Y);
                }
                if (beginActivityUniqueID != "")
                {
                    temActivity = getActivity(beginActivityUniqueID);
                    if (temActivity != null)
                        temActivity.AddBeginRule(r);
                    else
                        r.BeginPointPosition = new Point(beginPointX, beginPointY);

                }
                else
                {
                    r.BeginPointPosition = new Point(beginPointX, beginPointY);
                }
                temActivity = null;
                if (endActivityUniqueID != "")
                {
                    temActivity = getActivity(endActivityUniqueID);
                    if (temActivity != null)
                        temActivity.AddEndRule(r);
                    else
                        r.EndPointPosition = new Point(endPointX, endPointY);

                }
                else
                {
                    r.EndPointPosition = new Point(endPointX, endPointY);
                }

                //新增属性
                r.CurrentElementState = (ElementState)Enum.Parse(typeof(ElementState), node.Attribute("CurrentElementState").Value, true);




            }


            partNos = from item in xele.Descendants("Label") select item;
            string labelName = "";
            double labelX = 0;
            double labelY = 0;
            foreach (XElement node in partNos)
            {

                labelName = node.Value;

                double.TryParse(node.Attribute(XName.Get("X")).Value, out labelX);
                double.TryParse(node.Attribute(XName.Get("Y")).Value, out labelY);


                Label l = new Label(this);
                l.LabelName = labelName;
                l.Position = new Point(labelX, labelY);
                AddLabel(l);
            }



        }

        /// <summary>
        /// 检查是否可注册事件
        /// </summary>
        /// <returns></returns>
        public bool CanRegisterEvents(Activity activity)
        {
            bool isCanRegistr = false;
            if (string.IsNullOrEmpty(this.WorkFlowXML))
                return false;
            ActivityType activityType;
            MergePictureRepeatDirection repeatDirection = MergePictureRepeatDirection.None;
            Byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(this.WorkFlowXML);
            XElement xele = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));
            UniqueID = xele.Attribute(XName.Get("UniqueID")).Value;

            var partNos = from item in xele.Descendants("Activity") select item;


            foreach (XElement node in partNos)
            {
                activityType = (ActivityType)Enum.Parse(typeof(ActivityType), node.Attribute(XName.Get("Type")).Value, true);
                try
                {
                    repeatDirection = (MergePictureRepeatDirection)Enum.Parse(typeof(MergePictureRepeatDirection), node.Attribute(XName.Get("RepeatDirection")).Value, true);

                }
                catch (Exception e) { }

                Activity a = new Activity((IContainer)this, activityType);
                a.UniqueID = node.Attribute(XName.Get("UniqueID")).Value;
                if (a.UniqueID == activity.UniqueID)
                {
                    isCanRegistr = true;
                    break;
                }
            }
            return isCanRegistr;
        }
        public string ToXmlString()
        {
            System.Text.StringBuilder xml = new System.Text.StringBuilder(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes"" ?>");
            xml.Append(Environment.NewLine);
            xml.Append(@"<WorkFlow");
            xml.Append(@" UniqueID=""" + WorkFlowUrlID + @"""");    //UniqueID
            xml.Append(@" ID=""" + Guid.NewGuid().ToString() + @"""");
            xml.Append(@" Name=""" + txtWorkFlowName.Text + @"""");
            xml.Append(@" Description=""" + txtWorkFlowName.Text + @"""");
            xml.Append(@" Width=""" + ContainerWidth.ToString() + @"""");
            xml.Append(@" Height=""" + ContainerHeight.ToString() + @""">");



            System.Text.StringBuilder activityXml = new System.Text.StringBuilder("    <Activitys>");
            System.Text.StringBuilder ruleXml = new System.Text.StringBuilder("    <Rules>");
            System.Text.StringBuilder labelXml = new System.Text.StringBuilder("    <Labels>");

            IElement ele;
            foreach (UIElement c in cnsDesignerContainer.Children)
            {
                ele = c as IElement;
                if (ele != null)
                {
                    if (ele.IsDeleted)
                        continue;
                    if (ele.ElementType == WorkFlowElementType.Activity)
                    {
                        activityXml.Append(Environment.NewLine);
                        //xjt
                        if (String.IsNullOrEmpty(((Activity)ele).Flow.UniqueID))
                            ((Activity)ele).Flow.UniqueID = WorkFlowUrlID;

                        activityXml.Append(ele.ToXmlString());
                    }
                    else if (ele.ElementType == WorkFlowElementType.Rule)
                    {
                        ruleXml.Append(Environment.NewLine);
                        ruleXml.Append(ele.ToXmlString());

                    }
                    else if (ele.ElementType == WorkFlowElementType.Label)
                    {
                        labelXml.Append(Environment.NewLine);
                        labelXml.Append(ele.ToXmlString());

                    }
                }

            }
            activityXml.Append(Environment.NewLine);
            activityXml.Append("    </Activitys>");
            ruleXml.Append(Environment.NewLine);
            ruleXml.Append("    </Rules>");
            labelXml.Append(Environment.NewLine);
            labelXml.Append("    </Labels>");
            xml.Append(Environment.NewLine);
            xml.Append(activityXml.ToString());
            xml.Append(Environment.NewLine);
            xml.Append(ruleXml.ToString());
            xml.Append(Environment.NewLine);
            xml.Append(labelXml.ToString());
            xml.Append(Environment.NewLine);
            xml.Append(@"</WorkFlow>");
            return xml.ToString();

        }
        public void AddActivity(Activity a)
        {
            if (!cnsDesignerContainer.Children.Contains(a))
            {
                cnsDesignerContainer.Children.Add(a);
                a.Container = this;
                a.ActivityChanged += new ActivityChangeDelegate(OnActivityChanged);
            }
            if (!ActivityCollections.Contains(a))
                ActivityCollections.Add(a);

        }
        public void RemoveActivity(Activity a)
        {
            if (cnsDesignerContainer.Children.Contains(a))
                cnsDesignerContainer.Children.Remove(a);
            if (ActivityCollections.Contains(a))
                ActivityCollections.Remove(a);
        }
        public void RemoveLabel(Label l)
        {
            if (cnsDesignerContainer.Children.Contains(l))
                cnsDesignerContainer.Children.Remove(l);
        }

        #region MENU处理
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnContextMenu(object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            if (mouseIsInContainer)
            {
                e.PreventDefault();

                if (menuActivity.Visibility == Visibility.Collapsed
                    && menuRule.Visibility == Visibility.Collapsed
                    && menuLabel.Visibility == Visibility.Collapsed)
                {
                    menuContainer.ShowMenu(Visibility.Visible);

                    double top = (double)(e.ClientY - Top);
                    double left = (double)(e.ClientX - Left);
                    menuContainer.CenterPoint = new Point(left, top);
                }
            }
        }

        /// <summary>
        /// actibity menu
        /// </summary>
        /// <param name="a"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowActivityContentMenu(Activity a, object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            menuActivity.RelatedActivity = a;
            menuContainer.Visibility = Visibility.Collapsed;
            menuRule.ShowMenu(Visibility.Collapsed);
            menuLabel.ShowMenu(Visibility.Collapsed);
            double top = (double)(e.ClientY - Top);
            double left = (double)(e.ClientX - Left);
            menuActivity.CenterPoint = new Point(left, top);
            menuActivity.ShowMenu(Visibility.Visible);

        }

        /// <summary>
        /// label menu
        /// </summary>
        /// <param name="l"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowLabelContentMenu(Label l, object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            menuLabel.RelatedLabel = l;
            menuContainer.Visibility = Visibility.Collapsed;
            menuRule.ShowMenu(Visibility.Collapsed);
            menuActivity.ShowMenu(Visibility.Collapsed);
            double top = (double)(e.ClientY - Top);
            double left = (double)(e.ClientX - Left);
            menuLabel.CenterPoint = new Point(left, top);
            menuLabel.ShowMenu(Visibility.Visible);
        }

        /// <summary>
        /// rulemenu
        /// </summary>
        /// <param name="r"></param>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ShowRuleContentMenu(Rule r, object sender, System.Windows.Browser.HtmlEventArgs e)
        {
            menuRule.RelatedRule = r;
            menuContainer.Visibility = Visibility.Collapsed;
            menuLabel.ShowMenu(Visibility.Collapsed);

            menuActivity.ShowMenu(Visibility.Collapsed);
            double top = (double)(e.ClientY - Top);
            double left = (double)(e.ClientX - Left);
            menuRule.CenterPoint = new Point(left, top);
            menuRule.ShowMenu(Visibility.Visible);

        }
        public void ShowMessage(string message)
        {
            ShowContainerCover();
            MessageTitle.Text = message;
            MessageBody.Visibility = Visibility.Visible;
        }
        #endregion MENU处理
        private void AddActivity_Click(object sender, RoutedEventArgs e)
        {
            Activity a = new Activity((IContainer)this, ActivityType.INTERACTION);

            a.SetValue(Canvas.ZIndexProperty, NextMaxIndex);
            a.ActivityName = Text.NewActivity + NextNewActivityIndex.ToString();
            AddActivity(a);
            SaveChange(HistoryType.New);
            //a.ActivitySelectEvent += new ActivitySelectEventHandler(ActivityNull_ActivitySelectEvent);
            a.ActivitySelectEvent += new ActivitySelectEventHandler(Activity_ActivitySelectEvent);              //ZM        6.13修改
        }

        public void ShowContainerCover()
        {
            canContainerCover.Visibility = Visibility.Visible;
            sbContainerCoverDisplay.Begin();


        }
        public void CloseContainerCover()
        {
            sbContainerCoverClose.Completed += new EventHandler(sbContainerCoverClose_Completed);
            sbContainerCoverClose.Begin();
        }

        private void sbContainerCoverClose_Completed(object sender, EventArgs e)
        {
            canContainerCover.Visibility = Visibility.Collapsed;
        }

        private void AddRule_Click(object sender, RoutedEventArgs e)
        {

            Rule r = new Rule((IContainer)this);
            r.SetValue(Canvas.ZIndexProperty, NextMaxIndex);
            r.RuleName = Text.NewRule + NextNewRuleIndex.ToString();
            AddRule(r);
            SaveChange(HistoryType.New);
        }
        private void AddLabel_Click(object sender, RoutedEventArgs e)
        {
            Label r = new Label((IContainer)this);
            r.LabelName = Text.NewLable + NextNewLabelIndex.ToString();
            AddLabel(r);
            SaveChange(HistoryType.New);
        }

        private void btnCloseMessageButton_Click(object sender, RoutedEventArgs e)
        {
            MessageBody.Visibility = Visibility.Collapsed;
            CloseContainerCover();
        }

        public void btnExportXml_Click(object sender, RoutedEventArgs e)
        {
            CheckResult cr = CheckSave();
            if (cr.IsPass)
            {
                ShowContainerCover();
                XmlContainer.Visibility = Visibility.Visible;
                btnCloseXml.Visibility = Visibility.Visible;
                btnImportXml.Visibility = Visibility.Collapsed;
                txtXml.Text = ToXmlString();
            }
            else
                ShowMessage(cr.Message);

        }
        public String GetXML(bool isCheck)
        {
            String strXML = String.Empty;
            if (!isCheck)
                strXML = ToXmlString();
            else
            {
                CheckResult cr = CheckSave();
                if (cr.IsPass)
                {
                    //ShowContainerCover();
                    //XmlContainer.Visibility = Visibility.Visible;
                    //btnCloseXml.Visibility = Visibility.Visible;
                    //btnImportXml.Visibility = Visibility.Collapsed;
                    strXML = ToXmlString();
                }
                else
                    ShowMessage(cr.Message);
            }

            return strXML;
        }
        private void btnShowXmlContainer_Click(object sender, RoutedEventArgs e)
        {
            ShowContainerCover();
            btnImportXml.Visibility = Visibility.Visible;
            XmlContainer.Visibility = Visibility.Visible;
        }


        public void btnSave_Click(object sender, RoutedEventArgs e)
        {
            CheckResult cr = CheckSave();
            if (cr.IsPass)
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.UpdateWorkFlowXMLCompleted += (sb, eb) =>
                {
                    if (eb.Error == null)
                    {
                        string strXML = ToXmlString();
                        this.WorkFlowXML = strXML;//点击关闭时更新主界面XML
                        #region  add by luff 20130815 根据配置参数进入第三方控件的路径维护明细页还是微软控件的路径维护明细页
                        List<APPCFG> t_listApp = Global.mAppCfg.Select(s => s).Where(s => s.Configkey.IndexOf("PathWh") > -1).ToList();
                        if (t_listApp.Count > 0)
                        {
                            if (t_listApp[0].Value == "1")//表示进入第三方控件的路径维护明细页
                            {
                                if (sender is RWPathNodeSetting)
                                {
                                    RWPathNodeSetting Setting = (RWPathNodeSetting)sender;
                                    Setting.m_clinicalPathInfo.WorkFlowXML = this.WorkFlowXML;
                                    //Setting.Close();
                                }
                                else
                                {
                                    PublicMethod.RadAlterBox("保存成功", "工作流保存");
                                }
                            }
                            else//表示进入微软控件路径维护页面
                            {
                                if (sender is RWPathNodeSettingMS)
                                {
                                    RWPathNodeSettingMS Setting = (RWPathNodeSettingMS)sender;
                                    Setting.m_clinicalPathInfo.WorkFlowXML = this.WorkFlowXML;
                                    //Setting.Close();
                                }
                                else
                                {
                                    PublicMethod.RadAlterBox("保存成功", "工作流保存");
                                }
                            }
                        }
                        else//表示进入微软控件路径维护页面
                        {
                            if (sender is RWPathNodeSettingMS)
                            {
                                RWPathNodeSettingMS Setting = (RWPathNodeSettingMS)sender;
                                Setting.m_clinicalPathInfo.WorkFlowXML = this.WorkFlowXML;
                                //Setting.Close();
                            }
                            else
                            {
                                PublicMethod.RadAlterBox("保存成功", "工作流保存");
                            }
                        }
                        #endregion


                    }
                    else
                        PublicMethod.RadWaringBox(eb.Error);
                };// new EventHandler<System.ComponentModel.AsyncCompletedEventArgs>(Client_UpdateWorkFlowXMLCompleted);

                client.UpdateWorkFlowXMLAsync(ToXmlString());
                //Display(strXML);
                client.CloseAsync();
            }
            else
            {
                System.Windows.Browser.HtmlPage.Window.Alert(cr.Message);
            }

        }

        //private void Client_UpdateWorkFlowXMLCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        //{
        //    if (e.Error == null)
        //    {
        //        string strXML = ToXmlString();
        //        this.WorkFlowXML = strXML;//点击关闭时更新主界面XML
        //        PublicMethod.RadAlterBox("保存成功", "工作流保存");
        //    }
        //    else
        //        PublicMethod.RadWaringBox(e.Error);
        //}
        private void wfClient_UpdateWorkFlowXMLCompleted(object sender, System.ComponentModel.AsyncCompletedEventArgs e)
        {
            System.Windows.Browser.HtmlPage.Window.Alert(Text.Message_Saved);
        }
        private void CloseXml_Click(object sender, RoutedEventArgs e)
        {
            XmlContainer.Visibility = Visibility.Collapsed;
            CloseContainerCover();
        }
        Activity getActivity(string activityUniqueID)
        {
            for (int i = 0; i < _activityCollections.Count; i++)
            {
                if (_activityCollections[i].UniqueID == activityUniqueID)
                {
                    return _activityCollections[i];
                }
            }
            return null;
        }


        Canvas _gridLinesContainer;
        Canvas GridLinesContainer
        {
            get
            {
                if (_gridLinesContainer == null)
                {

                    Canvas temCan = new Canvas();
                    temCan.Name = "canGridLinesContainer";
                    cnsDesignerContainer.Children.Add(temCan);
                    _gridLinesContainer = temCan;

                }
                return _gridLinesContainer;
            }
        }
        private void CleareContainer()
        {
            cnsDesignerContainer.Children.Clear();
            _gridLinesContainer = null;
            SetGridLines();
            _activityCollections = null;
            _ruleCollections = null;
        }
        private void ClearContainer(object sender, RoutedEventArgs e)
        {
            CleareContainer();

            SaveChange(HistoryType.New);
        }
        private void ImportXml_Click(object sender, RoutedEventArgs e)
        {
            CleareContainer();
            XmlContainer.Visibility = Visibility.Collapsed;
            LoadFromXmlString(txtXml.Text);
            CloseContainerCover();
        }
        System.Collections.Generic.Stack<string> _workFlowXmlNextStack;
        public System.Collections.Generic.Stack<string> WorkFlowXmlNextStack
        {
            get
            {
                if (_workFlowXmlNextStack == null)
                    _workFlowXmlNextStack = new Stack<string>(50);
                return _workFlowXmlNextStack;
            }
        }
        System.Collections.Generic.Stack<string> _workFlowXmlPreStack;
        public System.Collections.Generic.Stack<string> WorkFlowXmlPreStack
        {
            get
            {
                if (_workFlowXmlPreStack == null)
                    _workFlowXmlPreStack = new Stack<string>(50);
                return _workFlowXmlPreStack;
            }
        }
        private string workflowXmlCurrent = @"";
        void pushNextQueueToPreQueue()
        {
            if (WorkFlowXmlPreStack.Count > 0)
                WorkFlowXmlNextStack.Push(WorkFlowXmlPreStack.Pop());
            int cout = WorkFlowXmlNextStack.Count;

            for (int i = 0; i < cout; i++)
            {
                WorkFlowXmlPreStack.Push(WorkFlowXmlNextStack.Pop());
            }
        }

        public void SaveChange(HistoryType action)
        {
            if (action == HistoryType.New)
            {
                WorkFlowXmlPreStack.Push(workflowXmlCurrent);
                workflowXmlCurrent = ToXmlString();
                WorkFlowXmlNextStack.Clear();

            }
            if (action == HistoryType.Next)
            {
                if (WorkFlowXmlNextStack.Count > 0)
                {
                    WorkFlowXmlPreStack.Push(workflowXmlCurrent);
                    workflowXmlCurrent = WorkFlowXmlNextStack.Pop();
                    CleareContainer();
                    ClearSelectFlowElement(null);

                }

                LoadFromXmlString(workflowXmlCurrent);
            }
            if (action == HistoryType.Previous)
            {
                if (WorkFlowXmlPreStack.Count > 0)
                {
                    WorkFlowXmlNextStack.Push(workflowXmlCurrent);
                    workflowXmlCurrent = WorkFlowXmlPreStack.Pop();
                    CleareContainer();

                    LoadFromXmlString(workflowXmlCurrent);
                    ClearSelectFlowElement(null);

                }
            }
            SetQueueButtonEnable();
            //SetGridLines();
        }
        private void SetQueueButtonEnable()
        {
            if (WorkFlowXmlPreStack.Count == 0)
            {
                btnPrevious.IsEnabled = false;
            }
            else
                btnPrevious.IsEnabled = true;

            if (WorkFlowXmlNextStack.Count == 0)
            {
                btnNext.IsEnabled = false;
            }
            else
                btnNext.IsEnabled = true;
        }

        public void PreviousAction()
        {
            SaveChange(HistoryType.Previous);

        }
        public void NextAction()
        {
            SaveChange(HistoryType.Next);

        }
        private void btnPre_Click(object sender, RoutedEventArgs e)
        {
            PreviousAction();
        }

        private void btnNext_Click(object sender, RoutedEventArgs e)
        {
            NextAction();
        }

        public void AddSelectedControl(System.Windows.Controls.Control uc)
        {
            if (!CurrentSelectedControlCollection.Contains(uc))
                CurrentSelectedControlCollection.Add(uc);
        }
        public void RemoveSelectedControl(System.Windows.Controls.Control uc)
        {
            if (CurrentSelectedControlCollection.Contains(uc))
                CurrentSelectedControlCollection.Remove(uc);
        }
        List<System.Windows.Controls.Control> _currentSelectedControlCollection;

        public List<System.Windows.Controls.Control> CurrentSelectedControlCollection
        {
            get
            {
                if (_currentSelectedControlCollection == null)
                    _currentSelectedControlCollection = new List<System.Windows.Controls.Control>();
                return _currentSelectedControlCollection;

            }
        }
        //bool ctrlKeyIsPress;
        public bool CtrlKeyIsPress
        {
            get
            {
                return (Keyboard.Modifiers == ModifierKeys.Control);
                //return ctrlKeyIsPress;
            }
        }
        public void SetWorkFlowElementSelected(System.Windows.Controls.Control uc, bool isSelected)
        {
            if (isSelected)
                AddSelectedControl(uc);
            else
                RemoveSelectedControl(uc);
            if (!CtrlKeyIsPress)
                ClearSelectFlowElement(uc);

        }
        public void ClearSelectFlowElement(System.Windows.Controls.Control uc)
        {

            if (CurrentSelectedControlCollection == null || CurrentSelectedControlCollection.Count == 0)
                return;

            int count = CurrentSelectedControlCollection.Count;
            for (int i = 0; i < count; i++)
            {
                //xjt,处理select事件里，多次选中
                if (((IElement)uc) != (IElement)CurrentSelectedControlCollection[i])
                    ((IElement)CurrentSelectedControlCollection[i]).IsSelectd = false;
            }
            CurrentSelectedControlCollection.Clear();
            if (uc != null)
            {
                //xjt,处理select事件里，多次选中
                //((IElement)uc).IsSelectd = true;
                AddSelectedControl(uc);
            }
            mouseIsInContainer = true;


        }
        public void DeleteSeletedControl()
        {
            if (CurrentSelectedControlCollection == null || CurrentSelectedControlCollection.Count == 0)
                return;
            Activity a = null;
            Rule r = null;
            Label l = null;
            for (int i = 0; i < CurrentSelectedControlCollection.Count; i++)
            {
                if (CurrentSelectedControlCollection[i] is Activity)
                {
                    a = CurrentSelectedControlCollection[i] as Activity;
                    a.Delete();
                }
                if (CurrentSelectedControlCollection[i] is Rule)
                {
                    r = CurrentSelectedControlCollection[i] as Rule;
                    r.Delete();
                }
                if (CurrentSelectedControlCollection[i] is Label)
                {
                    l = CurrentSelectedControlCollection[i] as Label;
                    l.Delete();
                }
            }
            ClearSelectFlowElement(null);

        }
        public void AlignTop()
        {
            if (CurrentSelectedControlCollection == null || CurrentSelectedControlCollection.Count == 0)
                return;
            Activity a = null;
            double minY = 100000.0;
            for (int i = 0; i < CurrentSelectedControlCollection.Count; i++)
            {
                if (CurrentSelectedControlCollection[i] is Activity)
                {
                    a = CurrentSelectedControlCollection[i] as Activity;

                    if (a.CenterPoint.Y < minY)
                        minY = a.CenterPoint.Y;
                }

            }
            for (int i = 0; i < CurrentSelectedControlCollection.Count; i++)
            {
                if (CurrentSelectedControlCollection[i] is Activity)
                {
                    a = CurrentSelectedControlCollection[i] as Activity;
                    a.CenterPoint = new Point(a.CenterPoint.X, minY);
                }
            }
        }
        public void AlignBottom()
        {
            if (CurrentSelectedControlCollection == null || CurrentSelectedControlCollection.Count == 0)
                return;
            Activity a = null;
            double maxY = 0;
            for (int i = 0; i < CurrentSelectedControlCollection.Count; i++)
            {
                if (CurrentSelectedControlCollection[i] is Activity)
                {
                    a = CurrentSelectedControlCollection[i] as Activity;

                    if (a.CenterPoint.Y > maxY)
                        maxY = a.CenterPoint.Y;
                }

            }
            for (int i = 0; i < CurrentSelectedControlCollection.Count; i++)
            {
                if (CurrentSelectedControlCollection[i] is Activity)
                {
                    a = CurrentSelectedControlCollection[i] as Activity;
                    a.CenterPoint = new Point(a.CenterPoint.X, maxY);
                }
            }
        }
        public void AlignLeft()
        {

            if (CurrentSelectedControlCollection == null || CurrentSelectedControlCollection.Count == 0)
                return;
            Activity a = null;
            double minX = 100000.0;
            for (int i = 0; i < CurrentSelectedControlCollection.Count; i++)
            {
                if (CurrentSelectedControlCollection[i] is Activity)
                {
                    a = CurrentSelectedControlCollection[i] as Activity;

                    if (a.CenterPoint.X < minX)
                        minX = a.CenterPoint.X;
                }
            }
            for (int i = 0; i < CurrentSelectedControlCollection.Count; i++)
            {
                if (CurrentSelectedControlCollection[i] is Activity)
                {
                    a = CurrentSelectedControlCollection[i] as Activity;
                    a.CenterPoint = new Point(minX, a.CenterPoint.Y);
                }
            }

        }
        public void AlignRight()
        {
            if (CurrentSelectedControlCollection == null || CurrentSelectedControlCollection.Count == 0)
                return;
            Activity a = null;
            double maxX = 0;
            for (int i = 0; i < CurrentSelectedControlCollection.Count; i++)
            {
                if (CurrentSelectedControlCollection[i] is Activity)
                {
                    a = CurrentSelectedControlCollection[i] as Activity;

                    if (a.CenterPoint.X > maxX)
                        maxX = a.CenterPoint.X;
                }

            }
            for (int i = 0; i < CurrentSelectedControlCollection.Count; i++)
            {
                if (CurrentSelectedControlCollection[i] is Activity)
                {
                    a = CurrentSelectedControlCollection[i] as Activity;
                    a.CenterPoint = new Point(maxX, a.CenterPoint.Y);
                }
            }
        }

        int moveStepLenght
        {
            get
            {
                if (CtrlKeyIsPress)
                    return 5;
                return 1;
            }
        }

        public void MoveUp()
        {
            MoveControlCollectionByDisplacement(0, -moveStepLenght, null);
            SaveChange(HistoryType.New);
        }
        public void MoveLeft()
        {
            MoveControlCollectionByDisplacement(-moveStepLenght, 0, null);
            SaveChange(HistoryType.New);

        }
        public void MoveDown()
        {
            MoveControlCollectionByDisplacement(0, moveStepLenght, null);
            SaveChange(HistoryType.New);

        }
        public void MoveRight()
        {
            MoveControlCollectionByDisplacement(moveStepLenght, 0, null);
            SaveChange(HistoryType.New);

        }
        public void MoveControlCollectionByDisplacement(double x, double y, UserControl uc)
        {
            if (CurrentSelectedControlCollection == null || CurrentSelectedControlCollection.Count == 0)
                return;

            Activity selectedActivity = null;
            Rule selectedRule = null;
            Label selectedLabel = null;
            if (uc is Activity)
                selectedActivity = uc as Activity;

            if (uc is Rule)
                selectedRule = uc as Rule;
            if (uc is Label)
                selectedLabel = uc as Label;

            Activity a = null;
            Rule r = null;
            Label l = null;
            for (int i = 0; i < CurrentSelectedControlCollection.Count; i++)
            {


                if (CurrentSelectedControlCollection[i] is Activity)
                {
                    a = CurrentSelectedControlCollection[i] as Activity;
                    if (a == selectedActivity)
                        continue;
                    a.SetPositionByDisplacement(x, y);
                }
                if (CurrentSelectedControlCollection[i] is Label)
                {
                    l = CurrentSelectedControlCollection[i] as Label;
                    if (l == selectedLabel)
                        continue;
                    l.SetPositionByDisplacement(x, y);
                }
                if (CurrentSelectedControlCollection[i] is Rule)
                {
                    r = CurrentSelectedControlCollection[i] as Rule;
                    if (r == selectedRule)
                        continue;
                    r.SetPositionByDisplacement(x, y);
                }
            }

            //for (int i = 0; i < CurrentSelectedControlCollection.Count; i++)
            //{

            //    if (CurrentSelectedControlCollection[i] is Rule)
            //    {
            //        r = CurrentSelectedControlCollection[i] as Rule;
            //        if (r == selectedRule)
            //            continue;
            //        r.SetPositionByDisplacement(x, y); 
            //    }
            //}
        }

        Point mousePosition;
        bool trackingMouseMove = false;
        System.Windows.Threading.DispatcherTimer _doubleClickTimer;
        private void Container_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (_doubleClickTimer.IsEnabled)
            {
                _doubleClickTimer.Stop();

                Activity a = new Activity((IContainer)this, ActivityType.INTERACTION);
                a.SetValue(Canvas.ZIndexProperty, NextMaxIndex);                                            //ZM    6.13添加
                a.ActivityName = Text.NewActivity + NextNewActivityIndex.ToString();

                Point p = e.GetPosition(this);
                a.CenterPoint = new Point(p.X - this.Left, p.Y - this.Top);
                a.IsSelectd = true;

                this.AddActivity(a);
                SaveChange(HistoryType.New);                                                                //ZM    6.13添加
                //a.ActivitySelectEvent += new ActivitySelectEventHandler(ActivityNull_ActivitySelectEvent);
                a.ActivitySelectEvent += new ActivitySelectEventHandler(Activity_ActivitySelectEvent);      //ZM    6.13修改
            }
            else
            {
                _doubleClickTimer.Start();
                menuActivity.Visibility = Visibility.Collapsed;
                menuLabel.Visibility = Visibility.Collapsed;
                menuRule.Visibility = Visibility.Collapsed;
                menuContainer.Visibility = Visibility.Collapsed;
                ClearSelectFlowElement(null);

                FrameworkElement element = sender as FrameworkElement;
                mousePosition = e.GetPosition(element);
                trackingMouseMove = true;
            }
        }

        private void Container_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            trackingMouseMove = false;
            if (CurrentTemporaryRule != null)
            {
                CurrentTemporaryRule.SimulateRulePointMouseLeftButtonUpEvent(RuleMoveType.End, CurrentTemporaryRule, e);
                if (CurrentTemporaryRule.EndActivity == null)
                {
                    CurrentTemporaryRule.BeginActivity.CanShowMenu = false;

                    CurrentTemporaryRule.CanShowMenu = false;
                    // this.RemoveRule(CurrentTemporaryRule);
                    CurrentTemporaryRule.Delete();
                }
                else
                {
                    CurrentTemporaryRule.CanShowMenu = false;
                    CurrentTemporaryRule.BeginActivity.CanShowMenu = false;
                    CurrentTemporaryRule.EndActivity.CanShowMenu = true;
                    CurrentTemporaryRule.IsTemporaryRule = false;
                    CurrentTemporaryRule.IsSelectd = false;
                    RemoveSelectedControl(CurrentTemporaryRule);
                    SaveChange(HistoryType.New);
                }
                CurrentTemporaryRule.ReleaseMouseCapture();
                CurrentTemporaryRule = null;

            }

            FrameworkElement element = sender as FrameworkElement;
            mousePosition = e.GetPosition(element);
            if (temproaryEllipse != null)
            {
                double width = temproaryEllipse.Width;
                double height = temproaryEllipse.Height;

                if (width > 10 && height > 10)
                {
                    Point p = new Point();
                    p.X = (double)temproaryEllipse.GetValue(Canvas.LeftProperty);
                    p.Y = (double)temproaryEllipse.GetValue(Canvas.TopProperty);

                    Activity a = null;
                    Rule r = null;
                    Label l = null;
                    foreach (UIElement uie in cnsDesignerContainer.Children)
                    {
                        if (uie is Activity)
                        {
                            a = uie as Activity;
                            if (p.X < a.CenterPoint.X && a.CenterPoint.X < p.X + width
                                && p.Y < a.CenterPoint.Y && a.CenterPoint.Y < p.Y + height)
                            {
                                AddSelectedControl(a);
                                a.IsSelectd = true;
                            }
                        }
                        if (uie is Label)
                        {
                            l = uie as Label;
                            if (p.X < l.Position.X && l.Position.X < p.X + width
                                && p.Y < l.Position.Y && l.Position.Y < p.Y + height)
                            {
                                AddSelectedControl(a);
                                l.IsSelectd = true;
                            }
                        }
                        if (uie is Rule)
                        {
                            r = uie as Rule;

                            Point ruleBeginPointPosition = r.BeginPointPosition;
                            Point ruleEndPointPosition = r.EndPointPosition;

                            if (p.X < ruleBeginPointPosition.X && ruleBeginPointPosition.X < p.X + width
                                && p.Y < ruleBeginPointPosition.Y && ruleBeginPointPosition.Y < p.Y + height
                                &&
                                p.X < ruleEndPointPosition.X && ruleEndPointPosition.X < p.X + width
                                && p.Y < ruleEndPointPosition.Y && ruleEndPointPosition.Y < p.Y + height
                                )
                            {
                                AddSelectedControl(r);
                                r.IsSelectd = true;
                            }
                        }
                    }
                }
                cnsDesignerContainer.Children.Remove(temproaryEllipse);
                temproaryEllipse = null;
            }

        }

        Rectangle temproaryEllipse;
        public bool IsMouseSelecting
        {
            get
            {
                return (temproaryEllipse != null);
            }
        }

        private void Container_MouseMove(object sender, MouseEventArgs e)
        {
            if (trackingMouseMove)
            {
                FrameworkElement element = sender as FrameworkElement;
                Point beginPoint = mousePosition;
                Point endPoint = e.GetPosition(element);

                if (temproaryEllipse == null)
                {
                    temproaryEllipse = new Rectangle();



                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = Color.FromArgb(255, 234, 213, 2);
                    temproaryEllipse.Fill = brush;
                    temproaryEllipse.Opacity = 0.2;

                    brush = new SolidColorBrush();
                    brush.Color = Color.FromArgb(255, 0, 0, 0);
                    temproaryEllipse.Stroke = brush;
                    temproaryEllipse.StrokeMiterLimit = 2.0;

                    cnsDesignerContainer.Children.Add(temproaryEllipse);

                }

                if (endPoint.X >= beginPoint.X)
                {
                    if (endPoint.Y >= beginPoint.Y)
                    {
                        temproaryEllipse.SetValue(Canvas.TopProperty, beginPoint.Y);
                        temproaryEllipse.SetValue(Canvas.LeftProperty, beginPoint.X);
                    }
                    else
                    {
                        temproaryEllipse.SetValue(Canvas.TopProperty, endPoint.Y);
                        temproaryEllipse.SetValue(Canvas.LeftProperty, beginPoint.X);
                    }

                }
                else
                {
                    if (endPoint.Y >= beginPoint.Y)
                    {
                        temproaryEllipse.SetValue(Canvas.TopProperty, beginPoint.Y);
                        temproaryEllipse.SetValue(Canvas.LeftProperty, endPoint.X);
                    }
                    else
                    {
                        temproaryEllipse.SetValue(Canvas.TopProperty, endPoint.Y);
                        temproaryEllipse.SetValue(Canvas.LeftProperty, endPoint.X);
                    }

                }


                temproaryEllipse.Width = Math.Abs(endPoint.X - beginPoint.X);
                temproaryEllipse.Height = Math.Abs(endPoint.Y - beginPoint.Y);




            }
            else
            {
                if (CurrentTemporaryRule != null)
                {
                    CurrentTemporaryRule.CaptureMouse();
                    Point currentPoint = e.GetPosition(CurrentTemporaryRule);
                    CurrentTemporaryRule.EndPointPosition = currentPoint;

                    if (CurrentTemporaryRule.BeginActivity != null)
                    {
                        CurrentTemporaryRule.BeginPointPosition = CurrentTemporaryRule.GetResetPoint(currentPoint, CurrentTemporaryRule.BeginActivity.CenterPoint, CurrentTemporaryRule.BeginActivity, RuleMoveType.Begin);
                    }
                }
            }

        }
        public void PastMemoryToContainer()
        {
            if (CopyElementCollectionInMemory != null
                      && CopyElementCollectionInMemory.Count > 0)
            {
                Activity a = null;
                Rule r = null;
                Label l = null;

                foreach (System.Windows.Controls.Control c in CopyElementCollectionInMemory)
                {
                    if (c is Rule)
                    {
                        r = c as Rule;
                        AddRule(r);
                        if (r.LineType == RuleLineType.Line)
                        {
                            r.SetRulePosition(new Point(r.BeginPointPosition.X + 20, r.BeginPointPosition.Y + 20),
                                new Point(r.EndPointPosition.X + 20, r.EndPointPosition.Y + 20));
                        }
                        else
                        {
                            r.SetRulePosition(new Point(r.BeginPointPosition.X + 20, r.BeginPointPosition.Y + 20),
                                new Point(r.EndPointPosition.X + 20, r.EndPointPosition.Y + 20)
                               , new Point(r.RuleTurnPoint1.CenterPosition.X + 20, r.RuleTurnPoint1.CenterPosition.Y + 20)
                               , new Point(r.RuleTurnPoint2.CenterPosition.X + 20, r.RuleTurnPoint2.CenterPosition.Y + 20)
                               );
                        }
                    }
                }


                foreach (System.Windows.Controls.Control c in CopyElementCollectionInMemory)
                {
                    if (c is Activity)
                    {
                        a = c as Activity;
                        AddActivity(a);
                        a.CenterPoint = new Point(a.CenterPoint.X + 20, a.CenterPoint.Y + 20);
                        a.Move(a, null);


                    }

                }
                foreach (System.Windows.Controls.Control c in CopyElementCollectionInMemory)
                {
                    if (c is Label)
                    {
                        l = c as Label;
                        AddLabel(l);
                        l.Position = new Point(l.Position.X + 20, l.Position.Y + 20);



                    }
                }


                for (int i = 0; i < CurrentSelectedControlCollection.Count; i++)
                {
                    ((IElement)CurrentSelectedControlCollection[i]).IsSelectd = false;

                }
                CurrentSelectedControlCollection.Clear();

                for (int i = 0; i < CopyElementCollectionInMemory.Count; i++)
                {

                    ((IElement)CopyElementCollectionInMemory[i]).IsSelectd = true;
                    AddSelectedControl(CopyElementCollectionInMemory[i]);
                }
                CopySelectedControlToMemory(null);

                SaveChange(HistoryType.New);


            }
        }
        public void CopySelectedControlToMemory(System.Windows.Controls.Control currentControl)
        {
            copyElementCollectionInMemory = null;

            if (currentControl != null)
            {
                if (currentControl is Activity)
                {

                    CopyElementCollectionInMemory.Add(((Activity)currentControl).Clone());
                }
                if (currentControl is Rule)
                {

                    CopyElementCollectionInMemory.Add(((Rule)currentControl).Clone());
                }
                if (currentControl is Label)
                {

                    CopyElementCollectionInMemory.Add(((Label)currentControl).Clone());
                }
            }
            else
            {
                if (CurrentSelectedControlCollection != null
                    && CurrentSelectedControlCollection.Count > 0)
                {
                    Activity a = null;
                    Rule r = null;
                    Label l = null;
                    foreach (System.Windows.Controls.Control c in CurrentSelectedControlCollection)
                    {
                        if (c is Activity)
                        {
                            a = c as Activity;

                            CopyElementCollectionInMemory.Add(a.Clone());
                        }
                    }
                    foreach (System.Windows.Controls.Control c in CurrentSelectedControlCollection)
                    {
                        if (c is Label)
                        {
                            l = c as Label;

                            CopyElementCollectionInMemory.Add(l.Clone());
                        }
                    }
                    foreach (System.Windows.Controls.Control c in CurrentSelectedControlCollection)
                    {
                        if (c is Rule)
                        {
                            r = c as Rule;
                            r = r.Clone();
                            CopyElementCollectionInMemory.Add(r);

                            if (r.OriginRule.BeginActivity != null)
                            {
                                Activity temA = null;
                                foreach (System.Windows.Controls.Control c1 in CopyElementCollectionInMemory)
                                {
                                    if (c1 is Activity)
                                    {
                                        temA = c1 as Activity;
                                        if (r.OriginRule.BeginActivity == temA.OriginActivity)
                                        {
                                            r.BeginActivity = temA;
                                        }
                                    }
                                }
                            }
                            if (r.OriginRule.EndActivity != null)
                            {
                                Activity temA = null;
                                foreach (System.Windows.Controls.Control c1 in CopyElementCollectionInMemory)
                                {
                                    if (c1 is Activity)
                                    {
                                        temA = c1 as Activity;
                                        if (r.OriginRule.EndActivity == temA.OriginActivity)
                                        {
                                            r.EndActivity = temA;
                                        }
                                    }
                                }
                            }
                        }
                    }
                    foreach (System.Windows.Controls.Control c in CurrentSelectedControlCollection)
                    {
                        if (c is Activity)
                        {
                            a = c as Activity;

                            a.OriginActivity = null;
                        }
                        if (c is Rule)
                        {
                            r = c as Rule;

                            r.OriginRule = null;
                        }
                    }

                }
            }
        }

        private void UserControl_KeyUp(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Delete:
                    if (CurrentSelectedControlCollection != null && CurrentSelectedControlCollection.Count > 0)
                    {
                        if (System.Windows.Browser.HtmlPage.Window.Confirm(Text.Comfirm_Delete))
                        {
                            DeleteSeletedControl();
                            SaveChange(HistoryType.New);
                        }
                    }
                    break;
                case Key.Up:
                    MoveUp();
                    break;
                case Key.Down:
                    MoveDown();
                    break;
                case Key.Left:
                    MoveLeft();
                    break;
                case Key.Right:
                    MoveRight();
                    break;

            }
        }


        private void UserControl_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.Control)
            {
                switch (e.Key)
                {
                    case Key.Z:

                        SaveChange(HistoryType.Previous);
                        break;
                    case Key.Y:
                        SaveChange(HistoryType.Next);
                        break;
                    case Key.C:

                        CopySelectedControlToMemory(null);
                        break;
                    case Key.V:

                        PastMemoryToContainer();
                        break;
                    case Key.A:
                        Activity a = null;
                        Rule r = null;
                        foreach (UIElement uie in cnsDesignerContainer.Children)
                        {

                            if (uie is Activity)
                            {
                                a = uie as Activity;
                                a.IsSelectd = true;
                                AddSelectedControl(a);
                            }

                            if (uie is Rule)
                            {
                                r = uie as Rule;
                                r.IsSelectd = true;
                                AddSelectedControl(r);
                            }
                        }
                        break;
                    case Key.S://未能捕获 

                        Save();
                        break;

                }
            }
        }
        public void Save()
        {
            XmlContainer.Visibility = Visibility.Visible;
            btnCloseXml.Visibility = Visibility.Visible;
            btnImportXml.Visibility = Visibility.Collapsed;
            txtXml.Text = ToXmlString();
        }
        private void applyContainerCulture()
        {
            //btnAddActivity.Content = Text.Button_AddActivity;
            // btnCreatePicture.Content = Text.Button_CreatePicture;
            btnAddRule.Content = Text.Button_AddRule;
            //btnClearContainer.Content = Text.Button_ClearContainer;
            btnCloseXml.Content = Text.Button_Close;
            btnExportToXml.Content = Text.Button_ExportToXml;
            btnImportFromXml.Content = Text.Button_ImportFromXml;
            btnImportXml.Content = Text.Button_ImportFromXml;
            //btnNext.Content = Text.Button_Next;
            //btnPrevious.Content = Text.Button_Previous;
            tbContainerHeight.Text = Text.ContainerHeight;
            tbContainerWidth.Text = Text.ContainerWidth;
            //tbWorkFlowName.Text = Text.WorkFlowName+"123123";
            btnCloseMessage.Content = Text.Button_Close;
            tbZoom.Text = Text.Button_Zoom;
            //btnSave.Content = Text.Button_Save;
            tbShowGridLines.Text = Text.Menu_ShowGridLines;
            btnAddLabel.Content = Text.Button_AddLabel;

        }
        public void ApplyCulture()
        {
            applyContainerCulture();
            menuActivity.ApplyCulture();
            menuLabel.ApplyCulture();
            menuContainer.ApplyCulture();
            menuRule.ApplyCulture();
            siActivitySetting.ApplyCulture();
            siRuleSetting.ApplyCulture();
        }

        List<System.Windows.Controls.Control> copyElementCollectionInMemory;

        public List<System.Windows.Controls.Control> CopyElementCollectionInMemory
        {
            get
            {
                if (copyElementCollectionInMemory == null)
                    copyElementCollectionInMemory = new List<System.Windows.Controls.Control>();
                return copyElementCollectionInMemory;
            }
            set
            {
                copyElementCollectionInMemory = value;
            }
        }


        private void btnApplyEnglishCulture_Click(object sender, RoutedEventArgs e)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("en-us");
            Configure.CurrentCulture = culture;
            //btnApplyChineseCulture.IsEnabled = true;
            //btnApplyEnglishCulture.IsEnabled = false;
            ApplyCulture();
        }
        private void btnApplyChineseCulture_Click(object sender, RoutedEventArgs e)
        {
            System.Globalization.CultureInfo culture = new System.Globalization.CultureInfo("zh-cn");
            Configure.CurrentCulture = culture;

            //btnApplyChineseCulture.IsEnabled = false;
            //btnApplyEnglishCulture.IsEnabled = true;
            ApplyCulture();
        }
        bool mouseIsInContainer = false;
        public bool MouseIsInContainer
        {
            get { return mouseIsInContainer; }
            set { mouseIsInContainer = value; }
        }

        private void Container_MouseEnter(object sender, MouseEventArgs e)
        {
            mouseIsInContainer = true;
        }

        private void Container_MouseLeave(object sender, MouseEventArgs e)
        {
            mouseIsInContainer = false;
        }
        private void sliWidth_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (cnsDesignerContainer != null)
            {
                cnsDesignerContainer.Width = sliWidth.Value;
                SetGridLines();
            }
        }
        private void sliHeight_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (cnsDesignerContainer != null)
            {
                cnsDesignerContainer.Height = sliHeight.Value;
                SetGridLines();
            }
        }

        private void sliZoom_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (sliZoom != null)
            {
                double zoomDeep = sliZoom.Value;
                btZoomValue.Text = Math.Round(zoomDeep, 2).ToString();

                IElement iel = null;
                foreach (UIElement uic in cnsDesignerContainer.Children)
                {
                    iel = uic as IElement;
                    if (iel != null)
                    {
                        iel.Zoom(zoomDeep);
                    }
                }

                if (zoomDeep >= 1)
                {
                    sliWidth.Value = sliWidth.Minimum * zoomDeep;
                    sliHeight.Value = sliHeight.Minimum * zoomDeep;

                }
                else
                {
                    sliWidth.Value = sliWidth.Minimum;
                    sliHeight.Value = sliHeight.Minimum;
                }
            }
        }

        private void cbShowGridLines_Click(object sender, RoutedEventArgs e)
        {
            if (cbShowGridLines.IsChecked.HasValue && cbShowGridLines.IsChecked.Value)
            {
                SetGridLines();
            }
            else
            {
                if (_gridLinesContainer != null)
                    _gridLinesContainer.Children.Clear();
                // _gridLinesContainer = null;
            }

        }

        #region
        /// <summary>
        /// gridline
        /// </summary>
        public void SetGridLines()
        {
            if (!cbShowGridLines.IsChecked.HasValue || !cbShowGridLines.IsChecked.Value)
                return;
            GridLinesContainer.Children.Clear();
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(255, 160, 160, 160);
            //  brush.Color = Color.FromArgb(255, 255, 255, 255);
            double thickness = 0.3;
            double top = 0;
            double left = 0;

            double width = cnsDesignerContainer.Width;
            double height = cnsDesignerContainer.Height;

            double stepLength = 40;

            double x, y;
            x = left + stepLength;
            y = top;

            while (x < width + left)
            {
                Line line = new Line();
                line.X1 = x;
                line.Y1 = y;
                line.X2 = x;
                line.Y2 = y + height;



                line.Stroke = brush;
                line.StrokeThickness = thickness;
                line.Stretch = Stretch.Fill;
                GridLinesContainer.Children.Add(line);
                x += stepLength;
            }


            x = left;
            y = top + stepLength;

            while (y < height + top)
            {
                Line line = new Line();
                line.X1 = x;
                line.Y1 = y;
                line.X2 = x + width;
                line.Y2 = y;


                line.Stroke = brush;
                line.Stretch = Stretch.Fill;
                line.StrokeThickness = thickness;
                GridLinesContainer.Children.Add(line);
                y += stepLength;
            }


        }

        public bool Contains(UIElement uie)
        {
            return cnsDesignerContainer.Children.Contains(uie);
        }

        /// <summary>
        /// check before save
        /// </summary>
        /// <returns></returns>
        public CheckResult CheckSave()
        {
            CheckResult cr = new CheckResult();
            cr.IsPass = true;
            CheckResult temCR = null;
            IElement iel;
            bool hasInitial = false;
            bool hasCompledion = false;
            string msg = "";
            foreach (UIElement uic in cnsDesignerContainer.Children)
            {
                iel = uic as IElement;
                if (iel != null)
                {
                    temCR = iel.CheckSave();
                    if (!temCR.IsPass)
                    {
                        cr.IsPass = false;
                        cr.Message += temCR.Message;


                    }
                    if (iel.ElementType == WorkFlowElementType.Activity)
                    {
                        if (((Activity)uic).Type == ActivityType.INITIAL)
                        {
                            hasInitial = true;

                        }
                        else if (((Activity)uic).Type == ActivityType.COMPLETION)
                        {
                            hasCompledion = true;

                        }
                    }
                }
            }

            if (!hasInitial)
            {
                cr.IsPass = false;
                msg += Text.Message_MustHaveOnlyOneBeginActivity + "\r\n";
            }
            if (!hasCompledion)
            {
                cr.IsPass = false;
                msg += Text.Message_MustHaveAtLeastOneEndActivity + "\r\n";
            }
            if (string.IsNullOrEmpty(txtWorkFlowName.Text))
            {
                cr.IsPass = false;
                msg += "必须输入流程名称\r\n";
            }
            msg += Text.Message_ModifyWorkFlowByTip;
            cr.Message = msg;
            return cr;
        }
        #endregion



        public delegate void AfterSelectActivityEventHandler(object sender, ActivitySelectEventArgs e);
        public event AfterSelectActivityEventHandler AfterSelectActivityEvent;

        protected virtual void OnAfterSelectActivityEvent(ActivitySelectEventArgs e)
        {
            if (AfterSelectActivityEvent != null)
                AfterSelectActivityEvent(this, e);
        }
        /// <summary>
        /// 之前节点获得选中节点信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Activity_ActivitySelectEvent(object sender, ActivitySelectEventArgs e)
        {
            OnAfterSelectActivityEvent(e);
        }

        /// <summary>
        /// 新增节点返回NULL
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ActivityNull_ActivitySelectEvent(object sender, ActivitySelectEventArgs e)
        {
            OnAfterSelectActivityEvent(null);
        }


    }
}
