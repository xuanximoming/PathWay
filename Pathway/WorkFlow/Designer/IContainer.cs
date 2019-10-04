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
using System.Collections.Generic;

namespace YidanEHRApplication.WorkFlow.Designer
{
    public delegate void LoadCompletedHandler();
    public interface IContainer
    {

         event LoadCompletedHandler LoadCompleted;
        void ShowMessage(string message);
        Activitys ActivityCollections { get; }
        Rules RuleCollections { get; }
        void AddActivity(Activity a);
        void RemoveActivity(Activity a);
        void RemoveLabel(Label l);
        void AddRule(Rule r);
        void AddLabel(Label l);
        void RemoveRule(Rule r);
        int NextMaxIndex { get; }
        string ToXmlString();
        void LoadFromXmlString(string xmlString);
        PageEditType EditType { get; set; }

        void ShowActivitySetting(Activity ac);
        void ShowRuleSetting(Rule rc);
        Rule CurrentTemporaryRule { get; set; }
        List<System.Windows.Controls.Control> CurrentSelectedControlCollection { get; }
        void AddSelectedControl(System.Windows.Controls.Control uc);
        void RemoveSelectedControl(System.Windows.Controls.Control uc);
        void SetWorkFlowElementSelected(System.Windows.Controls.Control uc, bool isSelect);
        void MoveControlCollectionByDisplacement(double x, double y, UserControl uc);
        bool CtrlKeyIsPress { get; }
        double ContainerWidth { get; set; }
        double ContainerHeight { get; set; }
        double ScrollViewerHorizontalOffset { get; set; }
        double ScrollViewerVerticalOffset { get; set; }
        void ShowActivityContentMenu(Activity a, object sender, System.Windows.Browser.HtmlEventArgs e);
        void ShowLabelContentMenu(Label l, object sender, System.Windows.Browser.HtmlEventArgs e);
        void ShowRuleContentMenu(Rule r, object sender, System.Windows.Browser.HtmlEventArgs e);
        void ClearSelectFlowElement(System.Windows.Controls.Control uc);
        void SaveChange(HistoryType action);
        int NextNewActivityIndex { get; }
        int NextNewRuleIndex { get; }
        int NextNewLabelIndex { get; }
        void CopySelectedControlToMemory(System.Windows.Controls.Control currentControl);
        void PastMemoryToContainer();
        void PreviousAction();
        void NextAction();
        List<System.Windows.Controls.Control> CopyElementCollectionInMemory { get; set; }
        System.Collections.Generic.Stack<string> WorkFlowXmlPreStack { get; }
        System.Collections.Generic.Stack<string> WorkFlowXmlNextStack { get; }

        void DeleteSeletedControl();
        /// <summary>
        /// 是否己经选中
        /// </summary>
        bool IsMouseSelecting { get; }  
        CheckResult CheckSave();
        bool Contains(UIElement uiel);
        bool MouseIsInContainer { get; set; }
        void AlignBottom();
        void AlignRight();
        void AlignTop();
        event RoutedEventHandler Loaded;
        String GetXML(bool isCheck);
        void AlignLeft();
    }
}
