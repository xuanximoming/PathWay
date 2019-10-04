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
using YidanEHRApplication.WorkFlow.Designer;
using YidanEHRApplication.WorkFlow;
using Telerik.Windows.Controls;
using YidanEHRApplication.Models;
using System.Linq;
using System.Text;
namespace YidanEHRApplication.WorkFlow
{
    public delegate void WorkFlow_ActivetySelectedDelegateEventHandler(Activity a);
    //public delegate void WorkFlow_NextWindowCloseEventHandler();
    /// <summary>
    /// 表示可通过索引访问的对象的Activity列表，提供Activity元素的下一步执行，下一步查看，隐藏Activity元素方法
    /// </summary>
    public class Activitys : List<Activity>
    {
        #region 私有
        #region 事件
        internal virtual void OnWorkFlow_ActivitySelectChanged(Activity a)
        {
            if (WorkFlow_ActivitySelectChanged != null)
            { WorkFlow_ActivitySelectChanged(a); }
        }
        /// <summary>
        /// 节点选中事件
        /// </summary>
        public event WorkFlow_ActivetySelectedDelegateEventHandler WorkFlow_ActivitySelectChanged;
        //public event WorkFlow_NextWindowCloseEventHandler WorkFlow_NextWindowClose;
        ///// <summary>
        ///// 弹出窗口关闭事件
        ///// </summary>
        //void OnWorkFlow_NextWindowClose()
        //{
        //    if (WorkFlow_NextWindowClose != null)
        //    {
        //        WorkFlow_NextWindowClose();
        //    }
        //}
        #endregion
        #region 变量
        Activity _CurrentActivity;
        Activity _CurrentViewActivity;
        Activitys _PreActivitys; //= new Activitys();
        Rules notHideRules = new Rules();
        /// <summary>
        /// 设置一个值，该值指示this的所有Activity的IsSelected
        /// </summary>
        public Boolean IsSelected
        {
            set
            {
                foreach (Activity a in this)
                {
                    a.IsSelectd = value;
                }
            }
        }
        /// <summary>
        /// 设置一个值，该值指示this的所有Activity的IsSetColor
        /// </summary>
        public Boolean IsSetColor
        {
            set
            {
                foreach (Activity a in this)
                {
                    a.IsSetColor = value;
                }
            }
        }
        #endregion
        #region 私有方法
        /// <summary>
        /// 循环遍历后面有可能执行的Rule的递归方法
        /// </summary>
        /// <param name="activity"></param>
        private void CalculateNotHideRule(Activity activity)
        {
            if (activity == null)
            {
                return;
            }
            if (activity.BeginRuleCollections != null && activity.BeginRuleCollections.Count > 0)
            {
                foreach (Rule r in activity.BeginRuleCollections)
                {
                    notHideRules.Add(r);
                    CalculateNotHideRule(r.EndActivity);
                }
            }
        }
        #endregion
        #endregion
        #region 公有
        #region 构造函数
        public Activitys(List<Activity> list)
        {
            this.AddRange(list);
        }
        public Activitys() { }
        #endregion
        #region 属性
        /// <summary>
        /// 当前结点
        /// </summary>
        public Activity CurrentActivity
        {
            get
            {
                if (_CurrentActivity == null && this.Count > 0)
                {
                    _CurrentActivity = this[0];
                }
                //RadWindow.Alert("CurrentActivity.get" + CurrentActivity.ActivityName);
                return _CurrentActivity;
            }
            set
            {
                if (_CurrentActivity != null)
                    _CurrentActivity.CurrentElementState = ElementState.Pre;
                _CurrentActivity = value;
                _CurrentActivity.CurrentElementState = ElementState.Now;
                this.IsSelected = false;
                if (_CurrentActivity.IsSelectd == false)
                {
                    _CurrentActivity.IsSelectd = true;
                }
                CurrentViewActivity = _CurrentActivity;//同步执行的和查看
            }
        }
        /// <summary>
        /// 当前点中查看的节点
        /// </summary>
        public Activity CurrentViewActivity
        {
            get
            {
                if (_CurrentViewActivity == null)
                {
                    _CurrentViewActivity = this[0];
                }
                return _CurrentViewActivity;
            }
            set
            {
                _CurrentViewActivity = value;
            }
        }
        /// <summary>
        /// 己经执行过结点集合
        /// </summary>
        public Activitys PreActivitys
        {
            get
            {
                _PreActivitys = new Activitys();
                foreach (Activity a in this)
                {
                    if (a.CurrentElementState == ElementState.Pre)
                        _PreActivitys.Add(a);
                }
                return _PreActivitys;
            }
        }
        #endregion
        #region 方法
        /// <summary>
        /// 循环节点，的下一步
        ///使用说明：调用方法NextView(),调用完成后,可以直接使用CurrentActivity.CurrentViewActiveChildren
        /// </summary>
        public void NextView()
        {
            if (CurrentViewActivity.Type != ActivityType.AUTOMATION)
                return;
            Int32 index = CurrentViewActivity.ActiveChildrens.IndexOf(CurrentViewActivity.CurrentViewActiveChildren);
            if (index != CurrentViewActivity.ActiveChildrens.Count - 1)
            {
                CurrentViewActivity.CurrentViewActiveChildren = CurrentViewActivity.ActiveChildrens[index + 1];
            }
            else
            {
                PublicMethod.RadAlterBox("已经到循环节点最后一步！", "提示");
            }
        }
        /// <summary>
        /// 循环节点上一部  
        /// 使用说明：调用方法PreView(),调用完成后,可以直接使用CurrentActivity.CurrentViewActiveChildren
        /// </summary>
        public void PreView()
        {
            if (CurrentViewActivity.Type != ActivityType.AUTOMATION)
                return;
            Int32 index = CurrentViewActivity.ActiveChildrens.IndexOf(CurrentViewActivity.CurrentViewActiveChildren);
            if (index != 0)
            {
                CurrentViewActivity.CurrentViewActiveChildren = CurrentViewActivity.ActiveChildrens[index - 1];
            }
            else
            {
                PublicMethod.RadAlterBox("已经到循环节点第一步！", "提示");
            }
        }
        /// <summary>
        /// 为不执行的节点添加灰色边框
        /// </summary>
        public void HideActivitys()
        {
            Activitys willNotHideActivitys = new Activitys();
            notHideRules = new Rules();
            CalculateNotHideRule(CurrentActivity);
            foreach (Rule r in notHideRules)
            {
                if (r.BeginActivity != null)
                {
                    willNotHideActivitys.Add(r.BeginActivity);
                }
                if (r.EndActivity != null)
                {
                    willNotHideActivitys.Add(r.EndActivity);
                }
            }
            foreach (var a in this)
            {
                if (a.CurrentElementState == ElementState.Next && !willNotHideActivitys.Contains(a))
                {
                    a.CurrentElementState = ElementState.Hide;
                }
            }
            
        }
       
        /// <summary>
        /// 选中当前VIEW结点后的所有结点
        /// </summary>
        public Activitys SeletAllViewActivitys(Activity a)
        {
            Activitys willSelectActivitys = new Activitys();
            try
            {
                notHideRules = new Rules();
                CalculateNotHideRule(a);
                foreach (Rule r in notHideRules)
                {
                    if (r.BeginActivity != null)
                    {
                        if (!willSelectActivitys.Contains(r.BeginActivity))
                            willSelectActivitys.Add(r.BeginActivity);
                    }
                    if (r.EndActivity != null)
                    {
                        if (!willSelectActivitys.Contains(r.EndActivity))
                            willSelectActivitys.Add(r.EndActivity);
                    }
                }
                willSelectActivitys.IsSetColor = true;
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
            return willSelectActivitys;
        }
        #endregion
        #endregion
        #region 外卖
        /// <summary>
        /// 下一步
        ///使用说明：调用方法Next(null),调用完成后,可以直接使用CurrentActivity.CurrentActiveChildren
        /// <returns></returns>
        /// </summary>
        /// <param name="a">指定的下一步节点</param>
        public void Next(Activity a)
        {
            //如果当前节点为结束类型的节点
            if (CurrentActivity.Type == ActivityType.COMPLETION)
                return;
            //指定NextActivity
            if (a != null)
            {
                CurrentActivity = a;
                HideActivitys();
                return;
            }
            //未指定NextActivity，并且NextActivitys.Count == 1 且CurrentActivity.Type不是循环节点
            if (CurrentActivity.NextActivitys.Count == 1 && CurrentActivity.Type != ActivityType.AUTOMATION)
            {
                CurrentActivity = CurrentActivity.NextActivitys[0];
                return;
            }
            List<YidanEHRApplication.Models.PublicMethod.Pair> PairList = new List<PublicMethod.Pair>();
            PairList.AddRange(from p in CurrentActivity.NextActivitys select new YidanEHRApplication.Models.PublicMethod.Pair(p.UniqueID, p.ActivityName));
            if (CurrentActivity.Type == ActivityType.AUTOMATION)//如果是循环节点，在选择项中添加“循环当前节点”
                PairList.Insert(0, new YidanEHRApplication.Models.PublicMethod.Pair("-1", "循环当前节点"));
            RadWindow wNext = new RadWindow();
            wNext.Closed += new EventHandler<WindowClosedEventArgs>(wwNext_Closed);
            wNext.Header = "节点选择";
            new PublicMethod().ShowSelectWindow(ref  wNext, PairList);
        }
        /// <summary>
        /// 弹出选择框关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>    
        void wwNext_Closed(object sender, WindowClosedEventArgs e)
        {try{
            if (e.PromptResult == null || e.PromptResult.ToString().Trim() == "")
                return;
            //选择循环当前节点
            if (e.PromptResult == "-1")
            {
                AddChildrenToCurrentCycleActivity();
                return;
            }
            List<Activity> ActivityList = new List<Activity>();
            ActivityList.AddRange(from a in CurrentActivity.NextActivitys where a.UniqueID == e.PromptResult select a);
            if (ActivityList.Count != 0)
                Next(ActivityList[0]);
            //OnWorkFlow_NextWindowClose();//调用注册的关闭事件
             }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }
        /// <summary>
        /// 提示框关闭后，为当前循环节点添加一个子节点
        /// </summary>
        public void AddChildrenToCurrentCycleActivity()
        {
            if (CurrentActivity.Type == ActivityType.AUTOMATION)
            {
                ActiveChildren children = new ActiveChildren();
                children.ActivityUniqueID = CurrentActivity.UniqueID;
                children.ActivityChildrenID = Guid.NewGuid().ToString();
                children.CurrentElementState = ElementState.Now;
                CurrentActivity.CurrentActiveChildren.CurrentElementState = ElementState.Pre;
                CurrentActivity.ActiveChildrens.Add(children);
                CurrentActivity.CurrentActiveChildren = children;
                CurrentActivity = CurrentActivity;
            }
        }
        #endregion
    }
}
