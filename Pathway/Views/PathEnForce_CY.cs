using DrectSoft.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;
using YidanEHRApplication.Views.UserControls;
using YidanEHRApplication.WorkFlow.Designer;

namespace YidanEHRApplication.Views
{
    public partial class PathEnForce
    {
        /// <summary> 
        /// 获得当前选中的结点的草药医嘱信息
        /// </summary>
        /// <param name="activity">选中节点</param>
        private void GetActivityCYOrder(Activity activity)
        {
            try
            {
                //判断如果页面不显示中药信息，则不访问数据库 不加载草药信息
                if (((RadTabItem)radTabControlPathManager.Items[1]).Visibility != System.Windows.Visibility.Visible)
                {
                    return;
                }
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;

                client.GetPathInitOrderCompleted +=
                (obj, e) =>
                {
                    radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        //获得当前节点医嘱信息
                        m_listOrder = e.Result;
                    }
                    else
                    {
                        RadWindow wndTemp = new RadWindow();
                        PublicMethod.ShowWarmWindow(ref wndTemp, e.Error.ToString(), m_StrTitle, null, null);
                    }
                };
                client.GetPathInitOrderAsync(activity.CurrentViewActiveChildren.ActivityUniqueID, Global.InpatientListCurrent.Syxh, Global.LogInEmployee, Global.InpatientListCurrent.Ljdm);
                client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary> 
        /// 根据当前选中的结点的
        /// 当前子结点的ActivityChildrenID
        ///  LOAD对应的医嘱信息
        /// </summary>
        /// <param name="activity">选中节点</param>
        /// <param name="isInit">去组套表(isInit=True),医嘱表(isInit=False)</param>
        private void GetActivityCYOrder(Activity activity, Boolean isInit)
        {
            try
            {
                //判断如果页面不显示中药信息，则不访问数据库 不加载草药信息
                if (((RadTabItem)radTabControlPathManager.Items[1]).Visibility != System.Windows.Visibility.Visible)
                {
                    return;
                }

                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                //if (isInit)
                //{
                client.GetPathInitCYOrderCompleted +=
                (obj, e) =>
                {
                    radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        ObservableCollection<CP_DoctorOrder> listOrder = e.Result;
                        foreach (CP_DoctorOrder order in listOrder)
                        {
                            order.Lrysdm = Global.LogInEmployee.Zgdm;

                            //判断为纯医嘱时候医嘱内容等于嘱托内容
                            if (order.Yzlb == (decimal)OrderPanelBarCategory.ChunOrder)
                            {
                                order.Yznr = order.Ztnr;
                            }
                            else
                            {
                                string ypgg = order.Ypgg == "" ? "" : "  [" + order.Ypgg + "]  ";
                                order.Yznr = order.Ypmc + ypgg + order.Ypjl.ToString() + order.Jldw + "  " + order.YfdmName + "  " + order.PcdmName + "   " + order.Ztnr;
                            }


                        }
                        radGridViewCYOrderList.ItemsSource = listOrder;
                        m_Orderlist = listOrder;
                    }
                    else
                    {
                        RadWindow wndTemp = new RadWindow();
                        PublicMethod.ShowWarmWindow(ref wndTemp, e.Error.ToString(), m_StrTitle, null, null);
                    }
                };
                client.GetPathInitCYOrderAsync(activity.CurrentViewActiveChildren.ActivityUniqueID, Global.InpatientListCurrent.Syxh, Global.LogInEmployee,
                    Global.InpatientListCurrent.Ljdm, Global.InpatientListCurrent.Ljxh.ToString());
                //}
                //else
                //{
                //    client.GetPathEnforcedOrderCompleted +=
                //   (obj, e) =>
                //   {
                //       radBusyIndicator.IsBusy = false;
                //       if (e.Error == null)
                //           this.radGridViewCYOrderList.ItemsSource = e.Result;
                //       else
                //       {
                //           RadWindow wndTemp = new RadWindow();
                //           PublicMethod.ShowWarmWindow(ref wndTemp, e.Error.ToString(), m_StrTitle, null, null);
                //       }
                //   };
                //    client.GetPathEnforcedOrderAsync(activity.CurrentViewActiveChildren.ActivityChildrenID, Global.InpatientListCurrent.Syxh, Global.InpatientListCurrent.Ljdm);
                //}
                client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 保存草药处方信息
        /// </summary>
        private void SaveCyOrder()
        {
            try
            {
                if (!(radGridViewCYOrderList.Items.Count == 0 && m_DelCYOrder.Count == 0))
                {
                    if (radGridViewCYOrderList.SelectedItems.Count == 0 && m_DelCYOrder.Count == 0)
                    {
                        //PublicMethod.RadAlterBox("请选择要保存的医嘱", m_StrTitle);
                        RadWindow wndTemp = new RadWindow();
                        PublicMethod.ShowAlertWindow(ref wndTemp, "请选择要保存的医嘱", m_StrTitle, null, null);
                        return;
                    }
                }

                //此处判断变异信息
                SaveCyOrderInfo();
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        /// <summary>
        /// 保存草药处方信息
        /// </summary>
        private void SaveCyOrderInfo()
        {
            try
            {
                ObservableCollection<CP_DoctorOrder> listOrderAdd = new ObservableCollection<CP_DoctorOrder>();
                ObservableCollection<CP_DoctorOrder> listOrderModify = new ObservableCollection<CP_DoctorOrder>();
                ObservableCollection<CP_DoctorOrder> listOrder = new ObservableCollection<CP_DoctorOrder>();
                listOrder = this.radGridViewCYOrderList.ItemsSource as ObservableCollection<CP_DoctorOrder>;
                #region 已经编辑过的医嘱无需选中也会保存
                foreach (CP_DoctorOrder order in listOrder)
                {
                    if (order.IsModify == true)
                    {
                        listOrderModify.Add(order);
                    }
                }
                #endregion
                if (radGridViewCYOrderList.SelectedItems != null)
                {
                    foreach (CP_DoctorOrder order in radGridViewCYOrderList.SelectedItems)
                    {
                        if (order.Yzxh == 0)
                        {
                            //order.Tbbz = 1;//有效记录
                            order.Tsbj = 0x01;
                            listOrderAdd.Add(order);
                        }
                        else
                        {
                            if (order.IsModify)
                            {
                                listOrderModify.Add(order);
                            }
                        }
                    }
                }
                if (listOrderAdd.Count > 0 || listOrderModify.Count > 0 || m_DelCYOrder.Count > 0)
                {
                    try
                    {

                        radBusyIndicator.IsBusy = true;
                        YidanEHRDataServiceClient client = PublicMethod.YidanClient;

                        YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                        ServiceClient.SaveAddCYOrderCompleted +=
                            (obj, e) =>
                            {
                                if (e.Error != null)
                                {
                                    RadWindow wndTemp = new RadWindow();
                                    PublicMethod.ShowWarmWindow(ref wndTemp, e.Error.ToString(), m_StrTitle, null, null);
                                }
                                else
                                {
                                    m_DelCYOrder.Clear();
                                    //PublicMethod.RadAlterBox("执行成功", m_StrTitle);
                                    RadWindow wndTemp = new RadWindow();
                                    PublicMethod.ShowAlertWindow(ref wndTemp, "草药处方保存成功", m_StrTitle, null, null);
                                    Activitys_WorkFlow_ActivitySelectChanged(m_WorkFlow.Activitys.CurrentActivity);

                                    m_IsLeadIn = false;

                                    m_AddOrder.Clear();
                                }
                                if (this.radBusyIndicator != null)
                                    this.radBusyIndicator.IsBusy = false;
                            };
                        if (this.radBusyIndicator != null) this.radBusyIndicator.IsBusy = true;
                        ServiceClient.SaveAddCYOrderAsync(listOrderAdd, listOrderModify, m_DelCYOrder,
                            Global.InpatientListCurrent,
                            m_WorkFlow.Activitys.CurrentViewActivity.CurrentViewActiveChildren.ActivityUniqueID,
                            m_WorkFlow.Activitys.CurrentViewActivity.CurrentViewActiveChildren.ActivityChildrenID,
                            Global.InpatientListCurrent.Ljdm);
                        ServiceClient.CloseAsync();

                        //client.CloseAsync();


                    }
                    catch (Exception ex)
                    {
                        radBusyIndicator.IsBusy = false;
                        throw ex;
                    }
                }
                else
                {
                    radBusyIndicator.IsBusy = false;
                    //PublicMethod.RadAlterBox("医嘱无变动,不需要保存", m_StrTitle);
                    //路径评估失败", "路径评估//
                    RadWindow wndTemp = new RadWindow();
                    PublicMethod.ShowAlertWindow(ref wndTemp, "医嘱无变动,不需要保存", m_StrTitle, null, null);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private List<CP_DoctorOrder> GetCYCDFList()
        {
            try
            {
                ObservableCollection<CP_DoctorOrder> listOrderAdd = new ObservableCollection<CP_DoctorOrder>();
                if (radGridViewCYOrderList.SelectedItems != null)
                {
                    foreach (CP_DoctorOrder order in radGridViewCYOrderList.SelectedItems)
                    {
                        listOrderAdd.Add(order);
                    }
                }
                return null;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void radGridViewCYOrderList_RowLoaded(object sender, RowLoadedEventArgs e)
        {
            try
            {
                if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;

                RadContextMenu rowContextMenu = new RadContextMenu(); //新建一个右键菜单
                rowContextMenu.Width = 200;
                rowContextMenu.Items.Add(new RadMenuItem() { Header = "编辑医嘱", Tag = TagName.Edit });
                rowContextMenu.Items.Add(new RadMenuItem() { IsSeparator = true });
                rowContextMenu.Items.Add(new RadMenuItem() { Header = "删除医嘱", Tag = TagName.Del });

                //add by luff 20130315 判断该新增医嘱是否可选，若可选 该行医嘱显示浅蓝色
                CP_DoctorOrder order = e.Row.Item as CP_DoctorOrder;
                if (order.Yzzt == 3200)
                {
                    if (order.Yzkx == 1)
                    {
                        e.Row.Foreground = ConvertColor.GetColorBrushFromHx16("0099CC");
                    }
                }

                //添加右键菜单事件
                rowContextMenu.AddHandler(RadMenuItem.ClickEvent, new RoutedEventHandler(OnRowCYMenuItemClick));
                rowContextMenu.Opened += new RoutedEventHandler(rowContextCYMenu_Opened);
                RadContextMenu.SetContextMenu(e.Row, rowContextMenu);
                //List<CheckBox> listCheck = this.radGridViewCYOrderList.ChildrenOfType<CheckBox>().ToList();
                //if (listCheck.Count > 1)
                //{
                if (e.Row != null)
                {
                    //CP_DoctorOrder order = e.Row.Item as CP_DoctorOrder;
                    if (!(order.Yzzt == (decimal)OrderStatus.OrderInptut || order.Yzzt == 0) || order.Tbbz == 1)
                    {
                        //listCheck[listCheck.Count - 1].IsEnabled = false;

                        //新增的已经发送医嘱显示为蓝色
                        if (order.Tbbz == 1 && (order.Yzzt == (decimal)OrderStatus.OrderInptut || order.Yzzt == 0))
                        {
                            e.Row.Foreground = ConvertColor.GetColorBrushFromHx16("0000FF");
                        }

                    }

                }
            }
            catch (Exception ex)
            {

                YiDanMessageBox.Show(ex, this.GetType().FullName);
            }
            //}
        }

        private void OnRowCYMenuItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RadMenuItem clickedItem = ((RadRoutedEventArgs)e).OriginalSource as RadMenuItem;
                if (clickedItem != null)
                {
                    TagName tagName = (TagName)clickedItem.Tag;
                    CP_DoctorOrder selectedItem = radGridViewCYOrderList.SelectedItem as CP_DoctorOrder;
                    OrderPanelBarCategory barItemTag = (OrderPanelBarCategory)(int.Parse(selectedItem.Yzlb.ToString()));
                    //DialogParameters parameters = new DialogParameters();
                    switch (tagName)
                    {
                        case TagName.Edit:
                            InitModifyOrderControl(barItemTag, selectedItem);
                            break;
                        case TagName.Del:
                            RemoveCYOrder();
                            break;

                        case TagName.SelectMuti:
                            //GridViewYZXX.UnselectAll();
                            break;
                        default:
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex, this.GetType().FullName);
            }
        }

        /// <summary>
        /// 从LIST中移除医嘱，并放入m_DelOrder
        /// </summary>
        private void RemoveCYOrder()
        {
            try
            {
                if (radGridViewCYOrderList.SelectedItems == null)
                    return;
                //添加判断 如果选中医嘱中有成组医嘱不给删除   edit by yxy  中草药无成组
                //foreach (object o in radGridViewCYOrderList.SelectedItems)
                //{
                //    if (((CP_DoctorOrder)o).Flag != "")
                //    {
                //        PublicMethod.RadAlterBox("存在分组不能删除！", "提示");
                //        return;
                //    }
                //}


                int selectItemsCount = radGridViewCYOrderList.SelectedItems.Count;
                for (int i = selectItemsCount - 1; i >= 0; i--)
                {
                    CP_DoctorOrder order = radGridViewCYOrderList.SelectedItems[i] as CP_DoctorOrder;
                    if (order.Yzzt == (decimal)OrderStatus.OrderInptut || order.Yzzt == 0)
                    {
                        radGridViewCYOrderList.Items.Remove(order);

                        if (order.FromTable != string.Empty || order.FromTable != "CP_AdviceSuitDetail")//代表不是新增
                        {
                            m_DelCYOrder.Add(order);
                        }

                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        #region  草药协定方
        /// <summary>
        ///   草药协定方医嘱输入初始化  
        /// </summary>
        private void InitCyfControl()
        {
            try
            {
                CyfOrderControl.PanelCategory = OrderPanelBarCategory.CyOrder;//3121
                CyfOrderControl.OrderCategory = OrderItemCategory.HerbalMedicine;//2403
                CyfOrderControl.AfterDrugLoadedEvent += new UCCyXDF.DrugLoaded(CyfOrderControl_AfterDrugLoadedEvent);
                CyfOrderControl.AfterDrugCinfirmeddEvent += new UCCyXDF.DrugConfirmed(CyfOrderControl_AfterDrugCinfirmeddEvent);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void CyfOrderControl_AfterDrugCinfirmeddEvent(object sender, CP_DoctorOrder e)
        {
            try
            {
                if (CyfOrderControl.ManualType == ManualType.Edit)
                    ModifyCYOrder(e);
                else
                    AddNewCYOrder(e);

                CyfOrderControl.NewAdviceGroupDetail();
            }
            catch (Exception ex)
            {

                YiDanMessageBox.Show(ex, this.GetType().FullName);
            }
        }


        /// <summary>
        /// 根据协定方ID获取协定方中详细信息
        /// </summary>
        /// <param name="cyXDF_ID"></param>
        /// <returns></returns>
        private void AddCyOrderByXDF(CP_DoctorOrder order)
        {
            try
            {
                int cyXDF_ID = ConvertMy.ToInt32(order.Extension);

                YidanEHRDataServiceClient serviceCon = PublicMethod.YidanClient;
                serviceCon.GetCyxdfMXInfoByIdCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            List<CP_DoctorOrder> list = GetCyOrderListByCYXDFMX(e.Result.ToList(), order);


                            if (list.Count > 0)
                            {
                                foreach (CP_DoctorOrder _order in list)
                                {
                                    AddNewCYOrder(_order);
                                }
                            }
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
                serviceCon.GetCyxdfMXInfoByIdAsync(cyXDF_ID);
                serviceCon.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 将后台返回的协定方明细列表转换成 List<CP_DoctorOrder>
        /// </summary>
        /// <param name="_cyxdfmx"></param>
        /// <param name="_order"></param>
        /// <returns></returns>
        private List<CP_DoctorOrder> GetCyOrderListByCYXDFMX(List<CP_CYXDFMX> _cyxdfmx, CP_DoctorOrder _order)
        {
            try
            {
                List<CP_DoctorOrder> list = new List<CP_DoctorOrder>();

                foreach (CP_CYXDFMX mx in _cyxdfmx)
                {
                    CP_DoctorOrder order = (CP_DoctorOrder)_order.Clone();
                    order.Cdxh = ConvertMy.ToDecimal(mx.Extension);
                    order.Ggxh = ConvertMy.ToDecimal(mx.Extension1);
                    order.Lcxh = 0; //((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Lcxh;
                    order.Ypdm = mx.Ypdm;
                    order.Xmlb = 2403;//2403;//((CP_CYFUnion)autoCompleteBoxOrder.SelectedItem).Yplb;
                    order.Ypmc = mx.Ypmc;
                    order.Ypjl = mx.Ypjl * order.Ypjl;
                    order.Jldw = mx.Jldw;
                    order.Zxdw = mx.Extension2;
                    order.Ypgg = mx.yplh;

                    list.Add(order);
                }

                if (list.Count > 0)
                {
                    return list;
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        /// <summary>
        /// 新增医嘱至GRID
        /// </summary>
        /// <param name="order"></param>
        private void AddNewCYOrder(CP_DoctorOrder order)
        {
            try
            {
                //判断为协定方
                if (order.Ypdm == "")
                {
                    AddCyOrderByXDF(order);
                }
                else
                {
                    string ypgg = order.Ypgg == "" ? "" : "  [" + order.Ypgg + "]  ";
                    order.Yznr = order.Ypmc + ypgg + order.Ypjl.ToString() + order.Jldw + "  " + order.YfdmName + "  " + order.PcdmName + "   " + order.Ztnr;

                    ObservableCollection<CP_DoctorOrder> listOrder = this.radGridViewCYOrderList.ItemsSource as ObservableCollection<CP_DoctorOrder>;

                    ObservableCollection<CP_DoctorOrder> listSelectOrder = new ObservableCollection<CP_DoctorOrder>();
                    if (this.radGridViewCYOrderList.SelectedItems != null)
                    {
                        foreach (var item in this.radGridViewCYOrderList.SelectedItems)
                        {

                            listSelectOrder.Add(item as CP_DoctorOrder);
                        }
                    }
                    this.radGridViewCYOrderList.ItemsSource = null;
                    listOrder.Add(order);
                    listSelectOrder.Add(order);
                    m_AddOrder.Add(order);
                    this.radGridViewCYOrderList.ItemsSource = listOrder;
                    //xjt,选中新增医嘱
                    foreach (CP_DoctorOrder sorder in listSelectOrder)
                    {
                        this.radGridViewCYOrderList.SelectedItems.Add(sorder);
                    }
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        /// <summary>
        /// 修改草药方医嘱至GRID
        /// </summary>
        /// <param name="order"></param>
        private void ModifyCYOrder(CP_DoctorOrder order)
        {
            try
            {
                ObservableCollection<CP_DoctorOrder> listGridOrder = this.radGridViewCYOrderList.ItemsSource as ObservableCollection<CP_DoctorOrder>;
                ObservableCollection<CP_DoctorOrder> listSelectOrder = new ObservableCollection<CP_DoctorOrder>();
                listGridOrder = InitNewListCYGrid(listGridOrder, order, ref listSelectOrder);
                this.radGridViewCYOrderList.ItemsSource = null;
                this.radGridViewCYOrderList.ItemsSource = listGridOrder;
                //xjt,选中新增医嘱
                foreach (CP_DoctorOrder sorder in listSelectOrder)
                {
                    this.radGridViewCYOrderList.SelectedItems.Add(sorder);
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 点击保存后更新GRID数据源
        /// </summary>
        /// <param name="listGridOrder"></param>
        /// <param name="doctorOrder"></param>
        /// <returns></returns>
        private ObservableCollection<CP_DoctorOrder> InitNewListCYGrid(ObservableCollection<CP_DoctorOrder> listGridOrder,
            CP_DoctorOrder doctorOrder, ref ObservableCollection<CP_DoctorOrder> listSelectOrder)
        {
            try
            {
                if (this.radGridViewCYOrderList.SelectedItems != null)
                {
                    foreach (var item in this.radGridViewCYOrderList.SelectedItems)
                        listSelectOrder.Add(item as CP_DoctorOrder);
                }
                for (int i = 0; i < listGridOrder.Count; i++)
                {
                    CP_DoctorOrder order = listGridOrder[i];
                    if (order.OrderGuid == doctorOrder.OrderGuid)
                    {
                        order = doctorOrder;
                        if (order.Yzxh != 0)
                            order.IsModify = true;
                        break;
                    }
                }
                for (int i = 0; i < listSelectOrder.Count; i++)
                {
                    CP_DoctorOrder order = listSelectOrder[i];
                    if (order.OrderGuid == doctorOrder.OrderGuid)
                    {
                        order = doctorOrder;
                        if (order.Yzxh != 0)
                            order.IsModify = true;
                        break;
                    }
                }
                return listGridOrder;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        private void CyfOrderControl_AfterDrugLoadedEvent(object sender, RoutedEventArgs e)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rowContextCYMenu_Opened(object sender, RoutedEventArgs e)
        {
            try
            {
                GridViewRow row = ((RadRoutedEventArgs)e).OriginalSource as GridViewRow;
                List<CP_AdviceGroupDetail> listsOrder = new List<CP_AdviceGroupDetail>();
                var RadMenu = sender as RadContextMenu;
                Boolean isSelectItemsHaveDefferentGroup = false;
                foreach (RadMenuItem item in RadMenu.Items)
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
                                isSelectItemsHaveDefferentGroup = false;
                                if (this.radGridViewCYOrderList.SelectedItems.Count == 0)
                                {
                                    isSelectItemsHaveDefferentGroup = true;
                                }
                                else
                                {
                                    //isSelectItemsHaveDefferentGroup = false;
                                    //item.IsEnabled = !(this.radGridViewOrderList.SelectedItems.Count > 1);

                                    //判断 Tbbz = 0为未发送的  且Yzzt = 3200 或 Yzzt = 0 新增医嘱
                                    if (((CP_DoctorOrder)radGridViewCYOrderList.SelectedItems[0]).Tbbz.ToString() != "0"
                                                && (((CP_DoctorOrder)radGridViewCYOrderList.SelectedItems[0]).Yzzt == (decimal)OrderStatus.OrderInptut || ((CP_DoctorOrder)radGridViewCYOrderList.SelectedItems[0]).Yzzt == 0))
                                    {
                                        isSelectItemsHaveDefferentGroup = true;
                                    }
                                }
                                item.IsEnabled = !(this.radGridViewCYOrderList.SelectedItems.Count > 1) && !isSelectItemsHaveDefferentGroup;
                            }
                            if ((TagName)item.Tag == TagName.Del)   //删除
                            {
                                isSelectItemsHaveDefferentGroup = false;
                                if (this.radGridViewCYOrderList.SelectedItems.Count == 0)
                                {
                                    isSelectItemsHaveDefferentGroup = true;
                                }
                                else
                                {
                                    //判断 Tbbz = 0为未发送的  且Yzzt = 3200 或 Yzzt = 0 新增医嘱
                                    if (((CP_DoctorOrder)radGridViewCYOrderList.SelectedItems[0]).Tbbz.ToString() != "0"
                                                && (((CP_DoctorOrder)radGridViewCYOrderList.SelectedItems[0]).Yzzt == (decimal)OrderStatus.OrderInptut || ((CP_DoctorOrder)radGridViewCYOrderList.SelectedItems[0]).Yzzt == 0))
                                    {
                                        isSelectItemsHaveDefferentGroup = true;
                                    }
                                }
                                item.IsEnabled = !(this.radGridViewCYOrderList.SelectedItems.Count > 1) && !isSelectItemsHaveDefferentGroup;
                            }

                        }
                    }
                }
            }
            catch (Exception ex)
            {

                YiDanMessageBox.Show(ex, this.GetType().FullName);
            }
        }


        #endregion


        /// <summary>
        /// 草药方行Selectionchanged时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radGridViewCYOrderList_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            try
            {
                radGridViewCYOrderList.SelectionChanged -= new EventHandler<SelectionChangeEventArgs>(radGridViewCYOrderList_SelectionChanged);
                int selectItemsCount = e.AddedItems.Count;
                for (int i = 0; i < selectItemsCount; i++)
                {
                    CP_DoctorOrder order = e.AddedItems[i] as CP_DoctorOrder;
                    if (!(order.Yzzt == (decimal)OrderStatus.OrderInptut || order.Yzzt == 0))
                        radGridViewCYOrderList.SelectedItems.Remove(order);
                }
                radGridViewCYOrderList.SelectionChanged += new EventHandler<SelectionChangeEventArgs>(radGridViewCYOrderList_SelectionChanged);
            }
            catch (Exception ex)
            {
                YiDanMessageBox.Show(ex, this.GetType().FullName);
            }
        }

        //add by luff 20130723 获得当前节点草药医嘱信息
        ObservableCollection<CP_DoctorOrder> m_listCyOrder = new ObservableCollection<CP_DoctorOrder>();

        private void GetCyJdIntiorder(Activity activity, Boolean isInit)
        {
            try
            {
                //判断如果页面不显示中药信息，则不访问数据库 不加载草药信息
                if (((RadTabItem)radTabControlPathManager.Items[1]).Visibility != System.Windows.Visibility.Visible)
                {
                    return;
                }

                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                //if (isInit)
                //{
                client.GetPathInitCYOrder2Completed +=
                (obj, e) =>
                {
                    radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        ObservableCollection<CP_DoctorOrder> listOrder = e.Result;
                        foreach (CP_DoctorOrder order in listOrder)
                        {
                            order.Lrysdm = Global.LogInEmployee.Zgdm;

                            //判断为纯医嘱时候医嘱内容等于嘱托内容
                            if (order.Yzlb == (decimal)OrderPanelBarCategory.ChunOrder)
                            {
                                order.Yznr = order.Ztnr;
                            }
                            else
                            {
                                string ypgg = order.Ypgg == "" ? "" : "  [" + order.Ypgg + "]  ";
                                order.Yznr = order.Ypmc + ypgg + order.Ypjl.ToString() + order.Jldw + "  " + order.YfdmName + "  " + order.PcdmName + "   " + order.Ztnr;
                            }


                        }
                        m_listCyOrder = listOrder;

                    }
                    else
                    {
                        RadWindow wndTemp = new RadWindow();
                        PublicMethod.ShowWarmWindow(ref wndTemp, e.Error.ToString(), m_StrTitle, null, null);
                    }
                };
                client.GetPathInitCYOrder2Async(activity.CurrentViewActiveChildren.ActivityUniqueID, Global.InpatientListCurrent.Syxh, Global.LogInEmployee,
                    Global.InpatientListCurrent.Ljdm, Global.InpatientListCurrent.Ljxh.ToString(), 1);

                client.CloseAsync();
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        /// <summary>
        /// 根据配置信息判断是否显示草药处方医嘱
        /// </summary>
        private void CheckCYOrderShow()
        {
            try
            {
                //add by luff 20130604 根据配置表判断是否显示草药医嘱相关内容
                List<APPCFG> t_listApp = Global.mAppCfg.Select(s => s).Where(s => s.Configkey.IndexOf("showCyXDF") > -1).ToList();
                if (t_listApp.Count > 0)
                {
                    if (t_listApp[0].Value == "1")//表示显示草药医嘱
                    {
                        //((RadTabItem)radTabControlPathManager.SelectedItem).Name=="CyOrder"
                        ((RadTabItem)radTabControlPathManager.Items[1]).Visibility = System.Windows.Visibility.Visible;
                    }
                    else
                    {
                        ((RadTabItem)radTabControlPathManager.Items[1]).Visibility = System.Windows.Visibility.Collapsed;

                    }
                }
                else
                {
                    ((RadTabItem)radTabControlPathManager.Items[1]).Visibility = System.Windows.Visibility.Collapsed;

                }
            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        // add by luff 20130723 点击“下一步路径”按钮的时候，check医嘱选中情况，判断是否变异
        /// <summary>
        /// 点击“下一步路径”按钮的时候，check医嘱选中情况，判断是否变异
        /// </summary>
        /// <param name="strWaring"></param>
        /// <returns></returns>
        private Boolean CyfCheck(out string strWaring)
        {


            strWaring = string.Empty;
            int iCount = 0;//记录执行次数

            //判断如果页面不显示中药信息，则不访问数据库 不加载草药如下逻辑
            if (((RadTabItem)radTabControlPathManager.Items[1]).Visibility != System.Windows.Visibility.Visible)
            {
                return false;
            }
            //check 必选红色是否全选，全选通过
            //若没选，将医嘱名称显示出来,并询问是否继续，若是，弹出异常原因输入框
            //Extension4该属性记录医嘱变异 
            m_UnDoOrder.Clear();
            m_NewOrder.Clear();
            ObservableCollection<CP_DoctorOrder> listCyOrderTemp = this.radGridViewCYOrderList.ItemsSource as ObservableCollection<CP_DoctorOrder>;
            //分别定义两个list集合存取节点医嘱列表和执行节点保存后列表数据

            List<CP_DoctorOrder> b_listCyOrder = new List<CP_DoctorOrder>();//存取执行节点保存后列表数据
            List<CP_DoctorOrder> a_listCyOrder = new List<CP_DoctorOrder>();//存取节点保存之前（路径维护模板）医嘱列表数据
            if (listCyOrderTemp.Count > m_listCyOrder.Count)
            {
                #region
                a_listCyOrder = listCyOrderTemp.Select(s => s).Where(s => s.Extension4 != 1).ToList();
                b_listCyOrder = m_listCyOrder.Select(s => s).Where(s => s.Extension4 != 1).ToList();
                #region 循环比较两个集合数据项是否存在相同，Extension4若为1表示相同，若为2表示必选未执行，若为3表示新增医嘱;若判断出相同的数据项，第二次循环就剔除Extension4为1的医嘱，少循环判断一次。
                for (int i = 0; i < b_listCyOrder.Count; i++)
                {

                    for (int j = 0; j < a_listCyOrder.Count; j++)
                    {
                        //判断药品项目是否有相同的数据项
                        if (m_listCyOrder[i].Ypdm == a_listCyOrder[j].Ypdm && m_listCyOrder[i].Pcdm == a_listCyOrder[j].Pcdm && m_listCyOrder[i].Pcdm != "" && m_listCyOrder[i].Ypjl == a_listCyOrder[j].Ypjl && m_listCyOrder[i].Yzkx == a_listCyOrder[j].Yzkx && m_listCyOrder[i].Yzkx == 0 && m_listCyOrder[i].Ypmc == a_listCyOrder[j].Ypmc)
                        {
                            m_listCyOrder[i].Extension4 = 1;
                            if (listCyOrderTemp.Select(s => s).Where(s => s.Ypdm == a_listCyOrder[j].Ypdm).ToList().Count > 0)
                            {
                                listCyOrderTemp.Select(s => s).Where(s => s.Ypdm == a_listCyOrder[j].Ypdm).ToList()[0].Extension4 = 1;
                            }

                            //b_listOrder = m_listCyOrder.Select(s => s).Where(s => s.Extension4 != 1).ToList();
                            a_listCyOrder = listCyOrderTemp.Select(s => s).Where(s => s.Extension4 != 1).ToList();
                            break;
                        }

                        else
                        {

                            if (listCyOrderTemp[j].Extension4 != 1)
                            {
                                listCyOrderTemp[j].Extension4 = 3;
                            }
                            if (m_listCyOrder[i].Extension4 != 1)
                            {
                                m_listCyOrder[i].Extension4 = 2;
                            }

                        }

                    }



                }
                #endregion
                #endregion
            }
            else
            {
                #region
                b_listCyOrder = listCyOrderTemp.Select(s => s).Where(s => s.Extension4 != 1).ToList();
                a_listCyOrder = m_listCyOrder.Select(s => s).Where(s => s.Extension4 != 1).ToList();
                #region 循环比较两个集合数据项是否存在相同，Extension4若为1表示相同，若为2表示必选未执行，若为3表示新增医嘱;若判断出相同的数据项，第二次循环就剔除Extension4为1的医嘱，少循环判断一次。
                for (int i = 0; i < b_listCyOrder.Count; i++)
                {

                    for (int j = 0; j < a_listCyOrder.Count; j++)
                    {
                        //判断药品项目是否有相同的数据项
                        if (listCyOrderTemp[i].Ypdm == a_listCyOrder[j].Ypdm && listCyOrderTemp[i].Pcdm == a_listCyOrder[j].Pcdm && listCyOrderTemp[i].Pcdm != "" && listCyOrderTemp[i].Ypjl == a_listCyOrder[j].Ypjl && listCyOrderTemp[i].Yzkx == a_listCyOrder[j].Yzkx && listCyOrderTemp[i].Yzkx == 0 && listCyOrderTemp[i].Ypmc == a_listCyOrder[j].Ypmc)
                        {
                            listCyOrderTemp[i].Extension4 = 1;
                            if (m_listCyOrder.Select(s => s).Where(s => s.Ypdm == a_listCyOrder[j].Ypdm).ToList().Count > 0)
                            {
                                m_listCyOrder.Select(s => s).Where(s => s.Ypdm == a_listCyOrder[j].Ypdm).ToList()[0].Extension4 = 1;
                            }

                            //b_listOrder = m_listCyOrder.Select(s => s).Where(s => s.Extension4 != 1).ToList();
                            a_listCyOrder = m_listCyOrder.Select(s => s).Where(s => s.Extension4 != 1).ToList();
                            break;
                        }

                        else
                        {

                            if (listCyOrderTemp[i].Extension4 != 1)
                            {
                                listCyOrderTemp[i].Extension4 = 3;
                            }
                            if (m_listCyOrder[j].Extension4 != 1)
                            {
                                m_listCyOrder[j].Extension4 = 2;
                            }

                        }

                    }



                }
                #endregion
                #endregion
            }
            #region 新增医嘱变异
            //表示新增医嘱变异 Extension4为3 表示新增医嘱变异 弹出变异选项框
            foreach (var cp in listCyOrderTemp.Select(s => s).Where(s => s.Extension4 == 3).Where(s => s.Yzkx == 0))
            {
                strWaring += "草药" + cp.YzbzName + ":" + cp.Ypmc + "\r\n";
            }
            m_NewOrder.AddRange(listCyOrderTemp.Select(s => s).Where(s => s.Extension4 == 3).Where(s => s.Yzkx == 0));
            #endregion

            #region 必选未执行的医嘱变异

            //表示必选，未执行的医嘱变异 Extension4为2 表示必选，未执行 弹出变异选项框
            foreach (var cp in m_listCyOrder.Select(s => s).Where(s => s.Extension4 == 2).Where(s => s.Yzkx == 0))
            {
                strWaring += "草药" + cp.YzbzName + ":" + cp.Ypmc + "\r\n";
            }
            m_UnDoOrder.AddRange(m_listCyOrder.Select(s => s).Where(s => s.Extension4 == 2).Where(s => s.Yzkx == 0));
            #endregion

            if (iCount == 0)
            {

                if (strWaring != string.Empty)
                {
                    strWaring = "以下必选草药医嘱未执行或者包含新增草药医嘱，是否继续？" + "\r\n" + strWaring;
                    iCount++;
                    //m_listCyOrder.Clear();
                    //listOrderTemp.Clear();
                    //m_UnDoOrder.Clear();
                    //m_NewOrder.Clear();
                    return false;
                }
            }
            return true;
        }


    }
}
