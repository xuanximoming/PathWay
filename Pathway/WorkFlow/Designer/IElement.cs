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

namespace YidanEHRApplication.WorkFlow.Designer
{

    public enum MergePictureRepeatDirection { Vertical = 0, Horizontal, None }
    public enum ActivityType
    {
        /// <summary>
        /// 与分支活动
        /// </summary>
        AND_BRANCH = 0,
        /// <summary>
        /// 与汇聚活动
        /// </summary>
        AND_MERGE,
        /// <summary>
        /// 常规自动活动
        /// </summary>
        AUTOMATION,
        /// <summary>
        /// 终结活动
        /// </summary>
        COMPLETION,
        /// <summary>
        /// 哑活动
        /// </summary>
        DUMMY,
        /// <summary>
        /// 初始化活动
        /// </summary>
        INITIAL,
        /// <summary>
        /// 常规交互活动
        /// </summary>
        INTERACTION,
        /// <summary>
        /// 或分支
        /// </summary>
        OR_BRANCH,
        /// <summary>
        /// 或汇聚活动
        /// </summary>
        OR_MERGE,
        /// <summary>
        /// 子流程
        /// </summary>
        SUBPROCESS,
        /// <summary>
        /// 投票汇聚活动
        /// </summary>
        VOTE_MERGE
    }
    public enum WorkFlowElementType { Activity = 0, Rule, Label }
    public enum PageEditType { Add = 0, Modify, None }
    public enum RuleLineType { Line = 0, Polyline }
    public enum HistoryType { New, Next, Previous };
    public class CheckResult
    {
        bool isPass = true;
        public bool IsPass { get { return isPass; } set { isPass = value; } }
        string message = "";
        public string Message { get { return message; } set { message = value; } }
    }

    public interface IElement
    {

        CheckResult CheckSave();

        string ToXmlString();
        void LoadFromXmlString(string xmlString);
        void ShowMessage(string message);
        WorkFlowElementType ElementType { get; }

        PageEditType EditType { get; set; }

        bool IsSelectd { get; set; }
        IContainer Container { get; set; }
        void Delete();
        void UpperZIndex();
        bool IsDeleted { get; }
        void Zoom(double zoomDeep);

    }
}
