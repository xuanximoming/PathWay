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
namespace YidanEHRApplication.WorkFlow
{
    public class WorkFlow
    {
        #region 私有
        /// <summary>
        /// 是否首次LOADED
        /// </summary>
        private Boolean m_ContentLoad = false;
        #region 事件
        /// <summary>
        /// ContainerEdit控件加载完成事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void _ContainerEdit_Loaded(object sender, RoutedEventArgs e)
        {
            if (m_ContentLoad)
                return;
            m_ContentLoad = true;
            foreach (var item in Activitys)
            {
                item.ActivitySelectEvent += new ActivitySelectEventHandler(item_ActivitySelectEvent);
                if (item.CurrentElementState == ElementState.Now)
                {
                    Activitys.CurrentActivity = item;//初始化当前节点
                }
                if (item.ActiveChildrens.Count > 1)
                {
                    foreach (var Children in item.ActiveChildrens)
                    {
                        if (Children.CurrentElementState == ElementState.Now)
                            item.CurrentActiveChildren = Children;//初始化当前节点的子步骤
                    }
                }
                Flow flow = new Flow();
                flow.UniqueID = item.Flow.UniqueID;
                var flows = from f in Flows
                            where f.UniqueID == flow.UniqueID
                            select f;
                if (flows.Count() == 0)
                    Flows.Add(flow);
            }
        }
        /// <summary>
        /// 节点选中事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void item_ActivitySelectEvent(object sender, ActivitySelectEventArgs e)
        {
            Activity a = (Activity)sender;
            Activitys.CurrentViewActivity = a;
            Activitys.OnWorkFlow_ActivitySelectChanged(a);

        }
        #endregion
        #region 变量
        Activitys _Activitys = new Activitys();
        Rules _Rules = new Rules();
        Labels _Labels = new Labels();
        ActiveChildrens _ActiveChildrens;
        Flows _Flows = new Flows();
        #endregion
        #endregion
        #region 公有
        #region 属性
        IContainer _ContainerEdit;
        /// <summary>
        /// 容器
        /// </summary>
        public IContainer ContainerEdit
        {
            get { return _ContainerEdit; }
            set
            {
                _ContainerEdit = value;
                Activitys = _ContainerEdit.ActivityCollections;
                Rules = _ContainerEdit.RuleCollections;
                //当_ContainerEdit加载后初始化相关数据

                _ContainerEdit.Loaded += new RoutedEventHandler(_ContainerEdit_Loaded);
            }
        }
        /// <summary>
        /// Activity集合
        /// </summary>
        public Activitys Activitys
        {
            get { return _Activitys; }
            set { _Activitys = value; }
        }
        /// <summary>
        /// Rule集合
        /// </summary>
        public Rules Rules
        {
            get { return _Rules; }
            set { _Rules = value; }
        }
        /// <summary>
        /// Label集合
        /// </summary>
        public Labels Labels
        {
            get { return _Labels; }
            set { _Labels = value; }
        }
        /// <summary>
        /// Flow集合
        /// </summary>
        public Flows Flows
        {
            get { return _Flows; }
            set { _Flows = value; }
        }
        /// <summary>
        /// 获取流程对应的XML
        /// </summary>
        /// <returns></returns>
        public String WorkFlowXml
        {
            get
            {
                return ContainerEdit.GetXML(false);
            }
        }
        #endregion
        #region 方法
        ///// <summary>
        ///// 根据XML初始化图像
        ///// </summary>
        //public void StartDraw() { }

        ///// <summary>
        ///// 添加其他路径
        ///// </summary>
        ///// <param name="xml"></param>
        //public void AddOtherPath(String xml)
        //{
        //    StartDraw();
        //}
        #endregion
        #endregion
    }
}
