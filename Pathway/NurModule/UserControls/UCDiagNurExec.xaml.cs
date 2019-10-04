using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Collections.ObjectModel;

using YidanEHRApplication.Models;
using YidanEHRApplication.DataService;
using YidanEHRApplication.WorkFlow.Designer;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using YidanSoft.Tool;
using YidanEHRApplication.Helpers;
using Telerik.Windows;
using YidanEHRApplication.Views.UserControls;
using System.Windows.Data;

namespace YidanEHRApplication.NurModule.UserControls
{
    public partial class UCDiagNurExec : UserControl
    {
        #region 变量

        /// <summary>
        /// 存放加载页面时候的路径节点信息
        /// </summary>
        private Activity m_activity;

        /// <summary>
        /// 存放加载路径时候是否为默认主要诊疗、护理数据
        /// </summary>
        private Boolean m_isInit;

        /// <summary>
        /// 当前页面状态
        /// </summary>
        PageState _currentState;
        private PageState CurrentState
        {
            get { return _currentState; }
            set { _currentState = value; }
        }

        /// <summary>
        /// 主要诊疗护理工作执行页面提示信息标题
        /// </summary>
        private string m_StrTitle = "诊疗/护理执行提示";


        private bool m_ContentLoaded;
        #endregion

        #region 事件
        /// <summary>
        /// 页面加载事件
        /// </summary>
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_ContentLoaded)
                {
                    return;
                }
                m_ContentLoaded = true;

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void GvDgiNur_RowLoaded(object sender, DataGridRowEventArgs e)
        {
            // add by luff 20130820 重写诊疗维护右键菜单
            ContextMenu rowContextMenu = new ContextMenu(); //新建一个右键菜单
            rowContextMenu.Width = 200;


            #region 其它医嘱 add by luff,20130411

            if (e.Row.GetIndex() < 0)
                return;
            MenuItem mEditDN = new MenuItem();
            mEditDN.Header = "编辑医嘱";
            mEditDN.Tag = TagName.Edit;
            mEditDN.Click += new RoutedEventHandler(mEditDN_Click);
            rowContextMenu.Items.Add(mEditDN);

            MenuItem mDelDN = new MenuItem();
            mDelDN.Header = "删除医嘱";
            mDelDN.Tag = TagName.Del;
            mDelDN.Click += new RoutedEventHandler(mDelDN_Click);
            rowContextMenu.Items.Add(mDelDN);
            //判断所选医嘱是否为必选项，必选项行显示为红色，可选项行显示为蓝色  

            //CP_DiagNurTemplate order = (GvDgiNur.ItemsSource as ObservableCollection<CP_DiagNurTemplate>)[e.Row.GetIndex()];
            //if (e.Row != null)
            //{
            //    if (order.Iskx == "0")
            //    {
            //        e.Row.Foreground = ConvertColor.GetColorBrushFromHx16("CC0000");
            //    }
            //    else
            //    {
            //        e.Row.Foreground = ConvertColor.GetColorBrushFromHx16("0099CC");
            //    }
            //}

            //rowContextMenu.AddHandler(RadMenuItem.ClickEvent, new RoutedEventHandler(OnDigNurMenuItemClick));
            rowContextMenu.Opened += new RoutedEventHandler(rowContextMenu_Opened);
            ContextMenuService.SetContextMenu(e.Row, rowContextMenu);

            #endregion

            e.Row.Background = new SolidColorBrush(Colors.White);

            

        }

        /// <summary>
        /// 右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rowContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            #region
             
            GridViewRow row = ((RoutedEventArgs)e).OriginalSource as GridViewRow;
            List<CP_AdviceGroupDetail> listsOrder = new List<CP_AdviceGroupDetail>();
            var RadMenu = sender as ContextMenu;
            foreach (MenuItem item in RadMenu.Items)
            {
                if (row != null && !row.IsSelected)
                {
                    item.IsEnabled = false;
                }
                else
                {
                    if (item.Tag != null)
                    {
                        if ((TagName)item.Tag == TagName.Edit)  //编辑
                        {
                            item.IsEnabled = !(this.GvDgiNur.SelectedItems.Count > 1);
                        }
                        if ((TagName)item.Tag == TagName.Del)   //删除
                        {
                            item.IsEnabled = (this.GvDgiNur.SelectedItems.Count >= 1);
                        }

                    }
                }
            }
            #endregion
        }


        #region 诊疗护理

        #region 诊疗护理变量
        /// <summary>
        /// 用于存放此类型的数据
        /// </summary>
        ObservableCollection<CP_DiagNurTemplate> cP_DigNurTemplateCollection = new ObservableCollection<CP_DiagNurTemplate>();
        CP_DiagNurTemplate m_cp_DiagNurInfo = new CP_DiagNurTemplate();



        #endregion
        /// <summary>
        ///  诊疗护理输入初始化  
        /// </summary>
        private void InitDigNurControl()
        {
            //初始化

            DigNur.m_Ljdm = Global.InpatientListCurrent.Ljdm;

            DigNur.AfterDrugLoadedEvent += new UCDiagNur.DrugLoaded(UCDiagNurControl_AfterDrugLoadedEvent);
            DigNur.AfterDrugCinfirmeddEvent += new UCDiagNur.DrugConfirmed(UCDiagNurControl_AfterDrugCinfirmeddEvent);
        }

        #region 确定后

        /// <summary>
        /// 确认后根据页面传回参数获取实体
        /// </summary>
        /// <param name="e"></param>
        private void GetEntity(CP_DiagNurTemplate e)
        {
            #region 实例 化新的实体
            m_cp_DiagNurInfo.ID = e.ID;
            m_cp_DiagNurInfo.MainID = e.MainID;
            m_cp_DiagNurInfo.Syxh = ConvertMy.ToDecimal(Global.InpatientListCurrent.Syxh);
            m_cp_DiagNurInfo.Ljdm = e.Ljdm;
            m_cp_DiagNurInfo.PathDetailId = m_activity.UniqueID;
            m_cp_DiagNurInfo.Lbxh = e.Lbxh;
            m_cp_DiagNurInfo.Wb = e.Wb;
            m_cp_DiagNurInfo.Yxjl = e.Yxjl;
            m_cp_DiagNurInfo.Py = e.Py;
            m_cp_DiagNurInfo.Mxxh = e.Mxxh;
            m_cp_DiagNurInfo.MxName = e.MxName;
            m_cp_DiagNurInfo.Create_Time = e.Create_Time;
            m_cp_DiagNurInfo.Create_User = e.Create_User;
            m_cp_DiagNurInfo.Cancel_User = e.Cancel_User;
            m_cp_DiagNurInfo.Extension = e.Extension;
            m_cp_DiagNurInfo.Isjj = e.Isjj;
            m_cp_DiagNurInfo.Zxksdm = e.Zxksdm;
            m_cp_DiagNurInfo.Extension1 = e.Extension1;
            m_cp_DiagNurInfo.Extension2 = e.Extension2;
            m_cp_DiagNurInfo.Extension3 = e.Extension3;
            //m_cp_DiagNurInfo.Iskx = e.Iskx == "0" ? "必选" : "可选";  //注释，因为添加了可选必选，在给行颜色的时候不好判断。
            m_cp_DiagNurInfo.Iskx = e.Iskx;  //给赋值  0 或 1 判断的时候好判断，隐藏显示是否可选列.

            #endregion
        }

        /// <summary>
        /// 判断当前实体元素是否已经在列表中存在
        /// </summary>
        private bool CheckDiagNurInList(CP_DiagNurTemplate _diagNur)
        {
            ObservableCollection<CP_DiagNurTemplate> itemlist = (ObservableCollection<CP_DiagNurTemplate>)GvDgiNur.ItemsSource;

            foreach (CP_DiagNurTemplate item in itemlist)
            {
                if (item.ID == _diagNur.ID)
                {
                    return false;
                }
            }
            return true;
        }

        private void UCDiagNurControl_AfterDrugCinfirmeddEvent(object sender, CP_DiagNurTemplate e)
        {
            try
            {

                if (DigNur.ManualType == ManualType.Edit)
                {
                    GetEntity(e);

                }
                else if (DigNur.ManualType == ManualType.New)
                {

                    m_cp_DiagNurInfo = new CP_DiagNurTemplate();

                    GetEntity(e);

                    ObservableCollection<CP_DiagNurTemplate> itemlist = (ObservableCollection<CP_DiagNurTemplate>)GvDgiNur.ItemsSource;

                    itemlist.Add(m_cp_DiagNurInfo);

                    GvDgiNur.ItemsSource = itemlist;
                    

                }
                CurrentState = PageState.New;
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        private void UCDiagNurControl_AfterDrugLoadedEvent(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }
        #region GRID 鼠标右键
        /// <summary>
        /// 右键
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDigNurMenuItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                MenuItem clickedItem = ((RoutedEventArgs)e).OriginalSource as MenuItem;
                if (clickedItem != null)
                {
                    TagName tagName = (TagName)clickedItem.Tag;
                    object selectedItem = GvDgiNur.SelectedItem;
                    DialogParameters parameters = new DialogParameters();
                    switch (tagName)
                    {
                        case TagName.Edit:
                            CurrentState = PageState.Edit;

                            #region  诊疗护理信息
                            m_cp_DiagNurInfo = this.GvDgiNur.SelectedItem as CP_DiagNurTemplate;
                            CP_DiagNurTemplate order = new CP_DiagNurTemplate();
                            order.ID = m_cp_DiagNurInfo.ID;
                            order.Lbxh = m_cp_DiagNurInfo.Lbxh;
                            order.Ljdm = m_cp_DiagNurInfo.Ljdm;
                            order.Mxxh = m_cp_DiagNurInfo.Mxxh;
                            order.MxName = m_cp_DiagNurInfo.MxName;
                            order.PathDetailId = m_cp_DiagNurInfo.PathDetailId;
                            order.Wb = m_cp_DiagNurInfo.Wb;
                            order.Py = m_cp_DiagNurInfo.Py;
                            order.Extension = m_cp_DiagNurInfo.Extension;
                            order.Extension1 = m_cp_DiagNurInfo.Extension1;
                            order.Yxjl = m_cp_DiagNurInfo.Yxjl;
                            order.Extension2 = m_cp_DiagNurInfo.Extension2;
                            order.Extension3 = m_cp_DiagNurInfo.Extension3;
                            order.Isjj = m_cp_DiagNurInfo.Isjj;
                            order.Zxksdm = m_cp_DiagNurInfo.Zxksdm;
                            // 右键修改的时候将是否必选加载到按钮当中   
                            //2013年8月9日 17:25:41
                            //修改人：Jhonny
                            order.Iskx = m_cp_DiagNurInfo.Iskx; 
                            order.Cancel_Time = m_cp_DiagNurInfo.Cancel_Time;
                            order.Cancel_User = m_cp_DiagNurInfo.Cancel_User;

                            order.Create_Time = m_cp_DiagNurInfo.Create_Time;
                            //order.Create_User = Guid.NewGuid().ToString();
                            order.Create_User = m_cp_DiagNurInfo.Create_User;

                            #endregion
                            DigNur.ManualType = ManualType.Edit;
                            DigNur.CP_DiagNurTemplateProptery = order;
                            DigNur.InitModifyOrder();
                             
                            break;
                        case TagName.Del:

                            //parameters.Content = String.Format("{0}", "确认删除吗？");
                            //parameters.Header = "诊疗护理项目";
                            //parameters.IconContent = null;
                            //parameters.OkButtonContent = "确定";
                            //parameters.CancelButtonContent = "取消";
                            //parameters.Closed = OnDelDigNurDetail;//***close处理***
                            //RadWindow.Confirm(parameters);
                            YidanPathWayMessageBox mess = new YidanPathWayMessageBox("确认删除吗？", "提示", YiDanMessageBoxButtons.YesNo);
                            mess.ShowDialog();
                            mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);

                            break;
                        default:
                            break;
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 右键打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnDigNurOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                GridViewRow row = ((RoutedEventArgs)e).OriginalSource as GridViewRow;
                List<CP_DiagNurTemplate> listsOrder = new List<CP_DiagNurTemplate>();
                var RadMenu = sender as ContextMenu;

                foreach (MenuItem item in RadMenu.Items)
                {
                    if (row != null && !row.IsSelected)
                    {
                        item.IsEnabled = false;
                    }
                    else
                        if (item.Tag != null)
                        {
                            if ((TagName)item.Tag == TagName.Edit)
                            {
                                item.IsEnabled = !(this.GvDgiNur.SelectedItems.Count > 1);
                            }
                        }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }



        void mess_PageClosedEvent(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    List<decimal> listid = new List<decimal>(); //用于保存删除的数据行主键

                    ObservableCollection<CP_DiagNurTemplate> itemlist = (ObservableCollection<CP_DiagNurTemplate>)GvDgiNur.ItemsSource;
                    List<CP_DiagNurTemplate> s_itemlist = new List<CP_DiagNurTemplate>();
                    foreach (CP_DiagNurTemplate item in GvDgiNur.SelectedItems)
                    {
                        s_itemlist.Add(item);
                    }

                    foreach (CP_DiagNurTemplate item in s_itemlist)
                    {
                        itemlist.Remove(item);
                    }

                    //GvDgiNur.ItemsSource = null;
                    
                     GvDgiNur.ItemsSource = itemlist;

                    //GvDgiNur.SelectedItem = null;

                    CurrentState = PageState.New;

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        /// <summary>
        /// 删除行数据事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDelDigNurDetail(object sender, WindowClosedEventArgs e)
        {
            try
            {
                if (e.DialogResult == true)
                {
                    List<decimal> listid = new List<decimal>(); //用于保存删除的数据行主键

                    ObservableCollection<CP_DiagNurTemplate> itemlist = (ObservableCollection<CP_DiagNurTemplate>)GvDgiNur.ItemsSource;
                    List<CP_DiagNurTemplate> s_itemlist = new List<CP_DiagNurTemplate>();
                    foreach (CP_DiagNurTemplate item in GvDgiNur.SelectedItems)
                    {
                        s_itemlist.Add(item);
                    }

                    foreach (CP_DiagNurTemplate item in s_itemlist)
                    {
                        itemlist.Remove(item);
                    }

                    //GvDgiNur.ItemsSource = null;
                     
                    GvDgiNur.ItemsSource = itemlist;

                    //GvDgiNur.SelectedItem = null;

                    CurrentState = PageState.New;

                }
                else
                {
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        #endregion

        #endregion

        //add by luff 20130820 诊疗护理列表创建分组事件
        private void GvDgiNur_LoadingRowGroup(object sender, DataGridRowGroupHeaderEventArgs e)
        {
            e.RowGroupHeader.PropertyName = "分类类型";
            e.RowGroupHeader.Foreground = ConvertColor.GetColorBrushFromHx16("25A0DA");
            e.RowGroupHeader.FontSize = 13;
        }
        #region add by luff 20130820诊疗护理右键相关事件
        /// <summary>
        /// 右键编辑医嘱菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mEditDN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GvDgiNur.SelectedItem == null)
                    return;
                CurrentState = PageState.Edit;

                #region  诊疗护理信息
                m_cp_DiagNurInfo = this.GvDgiNur.SelectedItem as CP_DiagNurTemplate;
                CP_DiagNurTemplate order = new CP_DiagNurTemplate();
                order.ID = m_cp_DiagNurInfo.ID;
                order.Lbxh = m_cp_DiagNurInfo.Lbxh;
                order.Ljdm = m_cp_DiagNurInfo.Ljdm;
                order.Mxxh = m_cp_DiagNurInfo.Mxxh;
                order.MxName = m_cp_DiagNurInfo.MxName;
                order.PathDetailId = m_cp_DiagNurInfo.PathDetailId;
                order.Wb = m_cp_DiagNurInfo.Wb;
                order.Py = m_cp_DiagNurInfo.Py;
                order.Extension = m_cp_DiagNurInfo.Extension;
                order.Extension1 = m_cp_DiagNurInfo.Extension1;
                order.Yxjl = m_cp_DiagNurInfo.Yxjl;
                order.Extension2 = m_cp_DiagNurInfo.Extension2;
                order.Extension3 = m_cp_DiagNurInfo.Extension3;
                order.Isjj = m_cp_DiagNurInfo.Isjj;
                order.Zxksdm = m_cp_DiagNurInfo.Zxksdm;
                // 右键修改的时候将是否必选加载到按钮当中   
                //2013年8月9日 17:25:41
                //修改人：Jhonny
                order.Iskx = m_cp_DiagNurInfo.Iskx;
                order.Cancel_Time = m_cp_DiagNurInfo.Cancel_Time;
                order.Cancel_User = m_cp_DiagNurInfo.Cancel_User;

                order.Create_Time = m_cp_DiagNurInfo.Create_Time;
                order.Create_User = m_cp_DiagNurInfo.Create_User;

                #endregion
                DigNur.ManualType = ManualType.Edit;
                DigNur.CP_DiagNurTemplateProptery = order;
                DigNur.InitModifyOrder();
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex, "错误提示！");

            }
        }

        /// <summary>
        /// 右键删除医嘱菜单事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void mDelDN_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GvDgiNur.SelectedItem == null)
                    return;

                YidanPathWayMessageBox mess = new YidanPathWayMessageBox("确认删除吗？", "提示", YiDanMessageBoxButtons.YesNo);
                mess.ShowDialog();
                mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex, "错误提示！");

            }
        }
        #endregion
        #endregion

        #region 方法
        /// <summary>
        /// 构造函数UCNurExecChildItem
        /// </summary>
        public UCDiagNurExec()
        {
            InitializeComponent();
            InitDigNurControl();
        }


        /// <summary>
        /// 初始化控件
        /// </summary>
        public void InitControl(Activity activity, bool isInit)
        {
            m_activity = activity;
            m_isInit = isInit;

            BindDigNurData(Global.InpatientListCurrent.Ljdm, activity.UniqueID);

        }

        /// <summary>
        /// 获得路径某一个节点的诊疗护理数据绑定 add by luff 20130411
        /// </summary>
        /// <param name="strLjdm"></param>
        /// <param name="nodeid"></param>
        public void BindDigNurData(string strLjdm, string nodeid)
        {
            if (!radBusyIndicator1.IsBusy)
                radBusyIndicator1.IsBusy = true;
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.GetDigNurRecordInfoCompleted +=
                    (obj, e) =>
                    {
                        radBusyIndicator1.IsBusy = false;
                        if (e.Error == null)
                        {
                            // mod by luff 20130820 对数据源进行设置 
                            //PagedCollectionView pcvDN = new PagedCollectionView(e.Result);
                            //pcvDN.GroupDescriptions.Add(new System.Windows.Data.PropertyGroupDescription("Extension2"));
                            //GvDgiNur.ItemsSource = pcvDN;
                            GvDgiNur.ItemsSource = e.Result;
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
            client.GetDigNurRecordInfoAsync(strLjdm, nodeid, Global.InpatientListCurrent.Syxh);
            client.CloseAsync();
        }


        /// <summary>
        /// 公共控件保存诊疗护理信息供外部调用
        /// </summary>
        public void Save()
        {
            try
            {
                List<decimal> listid = new List<decimal>(); //用于保存删除的数据行主键

                ObservableCollection<CP_DiagNurTemplate> itemlist = (ObservableCollection<CP_DiagNurTemplate>)GvDgiNur.ItemsSource;

                if (itemlist == null || itemlist.Count == 0)
                {
                    YiDanMessageBox.Show("无需保存！");
                    return;
                }
                foreach (CP_DiagNurTemplate item in itemlist)
                {
                    item.Create_User = Global.LogInEmployee.Zgdm;
                    item.Syxh = ConvertMy.ToDecimal(Global.InpatientListCurrent.Syxh);
                }

                if (!radBusyIndicator1.IsBusy)
                    radBusyIndicator1.IsBusy = true;
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.PLInsertDiagNurRecordCompleted +=
                        (obj, e) =>
                        {
                            radBusyIndicator1.IsBusy = false;
                            if (e.Error == null)
                            {
                                if (e.Result == 1)
                                {
                                    YiDanMessageBox.Show("保存成功！", m_StrTitle, YiDanMessageBoxButtons.Ok);
                                    BindDigNurData(Global.InpatientListCurrent.Ljdm, m_activity.UniqueID);
                                    return;
                                }
                                else
                                {
                                    YiDanMessageBox.Show("保存失败！", m_StrTitle, YiDanMessageBoxButtons.Ok);
                                    BindDigNurData(Global.InpatientListCurrent.Ljdm, m_activity.UniqueID);
                                    return;
                                }
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(e.Error);
                            }
                        };
                client.PLInsertDiagNurRecordAsync(itemlist);
                client.CloseAsync();

            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #endregion



        /// <summary>
        /// 右键菜单枚举
        /// </summary>
        private enum TagName
        {
            New,
            Edit,
            Del,
            Group,
            DisGroup,
            SelectMuti
        }

        /// <summary>
        /// 页面状态
        /// </summary>
        enum PageState
        {
            New = 0,
            Edit = 1
        }
    }
}
