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
namespace YidanEHRApplication.WorkFlow.Designer
{
    public partial class Activity
    {
        #region 变量
        ElementState _CurrentElementState = ElementState.Next;
        Boolean _IsEdit = true;
        ActiveChildrens _ActiveChildrens = new ActiveChildrens();
        Activitys _NextActivitys = new Activitys();
        ActiveChildren _CurrentActiveChildren;
        ActiveChildren _CurrentViewActiveChildren;
        Flow _Flow=new Flow();

        #endregion
  
        #region 属性
        /// <summary>
        /// 当前节点的链接的后续节点集合
        /// </summary>
        public Activitys NextActivitys
        {
            get
            {
                _NextActivitys = new Activitys();
                _NextActivitys.AddRange(from a in this.BeginRuleCollections select a.EndActivity);
                return _NextActivitys;
            }
            set { _NextActivitys = value; }
        }
        /// <summary>
        /// 当前节点状态
        /// </summary>
        public ElementState CurrentElementState
        {
            get { return _CurrentElementState; }
            set
            {
                _CurrentElementState = value;
                this.sdPicture.CurrentElementState = value;
            }
        }
        public Boolean IsEdit
        {
            get { return _CurrentElementState != ElementState.Pre; }
            set { _IsEdit = value; }
        }
        /// <summary>
        /// 当前节点的子节点集合
        /// </summary>
        public ActiveChildrens ActiveChildrens
        {
            get { return _ActiveChildrens; }
            set { _ActiveChildrens = value; }
        }
        /// <summary>
        /// 是否结束节点
        /// </summary>
        public Boolean IsEnd
        {
            get { return this.Type == ActivityType.COMPLETION; }
        }
        /// <summary>
        /// 查看时当前的节点的当前子节点
        /// </summary>
        public ActiveChildren CurrentViewActiveChildren
        {
            get
            {
                if (_CurrentViewActiveChildren == null)
                    _CurrentViewActiveChildren = CurrentActiveChildren;
                return _CurrentViewActiveChildren;
            }
            set
            {
                _CurrentViewActiveChildren = value;
                if (this.Type != ActivityType.AUTOMATION) return;
                StringBuilder sb = new StringBuilder();
                sb.Append(string.Format("{0}/{1}", this.ActiveChildrens.IndexOf(this.CurrentViewActiveChildren) + 1, this.ActiveChildrens.Count.ToString()));
                this.CurrentStep = sb.ToString();
            }
        }
        /// <summary>
        /// 执行时当前节点的子节点，始终是最后一个子节点
        /// </summary>
        public ActiveChildren CurrentActiveChildren
        {
            get
            {
                if (_CurrentActiveChildren == null && ActiveChildrens.Count > 0)
                    _CurrentActiveChildren = ActiveChildrens[ActiveChildrens.Count - 1];//xjt
                return _CurrentActiveChildren;
            }
            set { _CurrentActiveChildren = value;
            if (this.Type != ActivityType.AUTOMATION) return;
            StringBuilder sb = new StringBuilder();
            sb.Append(string.Format("{0}/{0}", this.ActiveChildrens.Count.ToString()));
            this.CurrentStep = sb.ToString();
            CurrentViewActiveChildren = value;//同步查看和执行的步骤
            }
        }

        /// <summary>
        /// 当前节点属于哪个路径
        /// </summary>
        public Flow Flow
        {
            get { return _Flow; }
            set { _Flow = value; }
        }
        #endregion
    }

}
