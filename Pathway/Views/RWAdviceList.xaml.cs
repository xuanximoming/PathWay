using DrectSoft.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using YidanEHRApplication.DataService;
using YidanEHRApplication.ExtraControl;
using YidanEHRApplication.Models;
namespace YidanEHRApplication.Views
{
    public partial class RWAdviceList
    {
        void AdviceList_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                BindLong("");
                BindTemp("");
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        public RWAdviceList(string syxh)
        {
            InitializeComponent();
            coloringExecute(StackPaneTemp);
            coloringExecute(StackPanelLong);
            Syxh = syxh;
            Loaded += new RoutedEventHandler(AdviceList_Loaded);
        }
        public RWAdviceList()
        {
            InitializeComponent();
            coloringExecute(StackPaneTemp);
            coloringExecute(StackPanelLong);
            Loaded += new RoutedEventHandler(AdviceList_Loaded);
        }
        #region 属性
        YidanEHRDataServiceClient serviceCon;
        KeyValues _ColorKeyValues = null;
        public KeyValues ColorKeyValuesProperty
        {
            get
            {
                //_ColorKeyValues.Clear();
                if (_ColorKeyValues == null)
                {
                    _ColorKeyValues = new KeyValues();
                    _ColorKeyValues.Add(new KeyValue("3200", "000000", "待审核"));
                    //_ColorKeyValues.Add(new KeyValue("3201", "800000", "已审核"));
                    //_ColorKeyValues.Add(new KeyValue("3202", "00ff00", "已执行"));
                    _ColorKeyValues.Add(new KeyValue("3201", "FFB90F", "已审核"));
                    _ColorKeyValues.Add(new KeyValue("3202", "A2CD5A", "已执行"));
                    _ColorKeyValues.Add(new KeyValue("3203", "FFCBD7", "已取消"));
                    _ColorKeyValues.Add(new KeyValue("3204", "7F7F7F", "已停止"));
                }
                return _ColorKeyValues;
            }
            set { _ColorKeyValues = value; }
        }
        String Syxh = "";
        #endregion
        #region 事件
        private void ButtonLong_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                if (b == null) return;
                if (b.Tag == null) BindLong("");
                else
                    BindLong(string.Format(" and CP_LongOrder.Yzzt={0}", ((Button)sender).Tag));
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void ButtonTemp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Button b = (Button)sender;
                if (b == null) return;
                if (b.Tag == null) BindTemp("");
                else
                    BindTemp(string.Format(" and CP_TempOrder.Yzzt={0}", ((Button)sender).Tag));
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void LinkButtonLong_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HyperlinkButton b = (HyperlinkButton)sender;
                if (b == null) return;
                if (b.Tag == null) BindLong("");
                else
                    BindLong(string.Format(" and CP_LongOrder.Yzzt={0}", ((HyperlinkButton)sender).Tag));
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void LinkButtonTemp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                HyperlinkButton b = (HyperlinkButton)sender;
                if (b == null) return;
                if (b.Tag == null) BindTemp("");
                else
                    BindTemp(string.Format(" and CP_TempOrder.Yzzt={0}", ((HyperlinkButton)sender).Tag));
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void GridViewLong_RowLoaded(object sender, RowLoadedEventArgs e)
        {
            if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
            CP_LongOrderList l = (CP_LongOrderList)e.DataElement;
            coloringExecute(e.Row, l.Yzzt.ToString());
        }
        private void GridViewTemp_RowLoaded(object sender, RowLoadedEventArgs e)
        {
            if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
            CP_TempOrderList t = (CP_TempOrderList)e.DataElement;
            coloringExecute(e.Row, t.Yzzt.ToString());
        }
        private void MenuDropDownLong_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                busy.IsBusy = true;
                Boolean isCheckPass = true;
                RadMenuItemExtra RadMenuItemExtraTemp = (RadMenuItemExtra)e.Source;
                // MenuDropDownTemp.Content = RadMenuItemExtraTemp.ExterProperty + RadMenuItemExtraTemp.ExterProperty2;
                if (GridViewLong.SelectedItems.Count > 0)
                {
                    ObservableCollection<CP_LongOrderList> CP_LongOrderList = new ObservableCollection<CP_LongOrderList>();
                    foreach (var item in GridViewLong.SelectedItems)
                    {
                        CP_LongOrderList longOrder = (CP_LongOrderList)item;
                        if (longOrder.Yzzt.ToString() != RadMenuItemExtraTemp.ExterProperty)
                        {
                            isCheckPass = false;
                            break;
                        }
                        else
                        {
                            longOrder.Yzzt = ConvertMy.ToDecimal(RadMenuItemExtraTemp.ExterProperty2);
                            CP_LongOrderList.Add(longOrder);
                        }
                    }
                    if (!isCheckPass)
                    {
                        PublicMethod.RadAlterBox("请确保选中的医嘱的状态为【" + ColorKeyValuesProperty[RadMenuItemExtraTemp.ExterProperty].Name + "】", "提示");
                        busy.IsBusy = false;
                    }
                    else
                    {
                        if (CP_LongOrderList.Count > 0)
                        {
                            serviceCon = PublicMethod.YidanClient;
                            serviceCon.UpdateLongOrderListYzztCompleted += (send, ea) =>
                            {
                                BindLong(string.Format(" and CP_LongOrder.Yzzt={0}", RadMenuItemExtraTemp.ExterProperty2));
                                busy.IsBusy = false;
                            };
                            serviceCon.UpdateLongOrderListYzztAsync(CP_LongOrderList);
                        }
                    }
                }
                else
                {
                    PublicMethod.RadAlterBox("请选择医嘱！", "提示");
                    busy.IsBusy = false;
                }
                MenuDropDownLong.IsOpen = false;

            }
            catch (Exception ex)
            {
                busy.IsBusy = false;
            }

        }
        private void MenuDropDownTemp_ItemClick(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                Boolean isCheckPass = true;
                RadMenuItemExtra RadMenuItemExtraTemp = (RadMenuItemExtra)e.Source;
                // MenuDropDownTemp.Content = RadMenuItemExtraTemp.ExterProperty + RadMenuItemExtraTemp.ExterProperty2;
                if (GridViewTemp.SelectedItems.Count > 0)
                {
                    ObservableCollection<CP_TempOrderList> CP_TempOrderList = new ObservableCollection<CP_TempOrderList>();
                    foreach (var item in GridViewTemp.SelectedItems)
                    {
                        CP_TempOrderList tempOrder = (CP_TempOrderList)item;
                        if (tempOrder.Yzzt.ToString() != RadMenuItemExtraTemp.ExterProperty)
                        {
                            isCheckPass = false;
                            break;
                        }
                        else
                        {
                            tempOrder.Yzzt = ConvertMy.ToDecimal(RadMenuItemExtraTemp.ExterProperty2);
                            CP_TempOrderList.Add(tempOrder);
                        }
                    }
                    if (!isCheckPass)
                    {
                        PublicMethod.RadAlterBox("请确保选中的医嘱的状态为【" + ColorKeyValuesProperty[RadMenuItemExtraTemp.ExterProperty].Name + "】", "提示");
                        busy.IsBusy = false;
                    }
                    else
                    {
                        if (CP_TempOrderList.Count > 0)
                        {
                            serviceCon = PublicMethod.YidanClient;

                            busy.IsBusy = true;
                            serviceCon.UpdateTempOrderListYzztCompleted += (send, ea) =>
                            {
                                BindTemp(string.Format(" and CP_TempOrder.Yzzt={0}", RadMenuItemExtraTemp.ExterProperty2));
                                busy.IsBusy = false;
                            };
                            serviceCon.UpdateTempOrderListYzztAsync(CP_TempOrderList);
                        }
                    }
                }
                else
                {
                    PublicMethod.RadAlterBox("请选择医嘱！", "提示");
                    busy.IsBusy = false;

                }
                MenuDropDownTemp.IsOpen = false;

            }
            catch (Exception ex)
            {
                busy.IsBusy = false;

            }
        }
        #endregion
        #region 函数
        public void BindLong(String Where)
        {
            try
            {
                if (Syxh.Trim() != "")
                {
                    Where = Where + String.Format(" and InPatient.NoOfInpat='{0}'", Syxh);
                }
                serviceCon = PublicMethod.YidanClient;
                busy.IsBusy = true;
                serviceCon.GetLongOrderListBelongToSomeOneCompleted += (send, ea) =>
                {
                    if (ea.Error != null) PublicMethod.RadWaringBox(ea.Error);
                    else
                    {
                        List<CP_LongOrderList> CP_LongOrderListTemp = ea.Result.ToList();
                        foreach (var item in CP_LongOrderListTemp)
                        {
                            item.YzztName = ColorKeyValuesProperty[item.Yzzt.ToString()].Name;
                        }
                        GridViewLong.ItemsSource = CP_LongOrderListTemp;
                    }
                    busy.IsBusy = false;
                };
                serviceCon.GetLongOrderListBelongToSomeOneAsync(Where);
                serviceCon.CloseAsync();
            }
            catch (Exception e)
            {
                PublicMethod.ClientException(e, this.GetType().FullName, true);
                busy.IsBusy = false;
            }

        }
        public void BindTemp(String Where)
        {
            try
            {
                if (Syxh.Trim() != "")
                {
                    Where = Where + String.Format(" and InPatient.NoOfInpat='{0}'", Syxh);
                }
                serviceCon = PublicMethod.YidanClient;
                busy.IsBusy = true;
                serviceCon.GetTempOrderListBelongToSomeOneCompleted += (send, ea) =>
                    {
                        if (ea.Error != null) PublicMethod.RadWaringBox(ea.Error);
                        else
                        {
                            List<CP_TempOrderList> CP_TempOrderListTemp = ea.Result.ToList();
                            foreach (var item in CP_TempOrderListTemp)
                            {
                                item.YzztName = ColorKeyValuesProperty[item.Yzzt.ToString()].Name;
                            }
                            GridViewTemp.ItemsSource = CP_TempOrderListTemp;
                        }
                        busy.IsBusy = false;
                    };
                serviceCon.GetTempOrderListBelongToSomeOneAsync(Where);
                serviceCon.CloseAsync();
            }
            catch (Exception ex)
            {
                busy.IsBusy = false;
            }
        }
        private void coloringExecute(Panel container)
        {
            UIElementCollection list = container.Children;
            for (int i = 0; i < list.Count; i++)
            {
                object c = list[i];
                if (c is RadButton)
                {
                    RadButton button = c as RadButton;
                    if (button.Tag != null && button.Tag.ToString().Trim() != "")
                    {
                        SolidColorBrush brush = ConvertColor.GetColorBrushFromHx16(ColorKeyValuesProperty[button.Tag.ToString()].Value);
                        button.Foreground = brush;
                        button.Background = brush;
                    }
                }
            }
        }
        private void coloringExecute(Control c, string key)
        {
            SolidColorBrush brush = ConvertColor.GetColorBrushFromHx16(ColorKeyValuesProperty[key].Value);
            if (c is RadButton)
            {
                RadButton button = c as RadButton;
                if (button.Tag != null && button.Tag.ToString().Trim() != "")
                {
                    button.Foreground = brush;
                    button.Background = brush;
                }
            }
            if (c is GridViewRowItem)
            {
                GridViewRowItem rowItem = c as GridViewRowItem;
                rowItem.Foreground = brush;
            }
        }
        #endregion

        private void btnCheck_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busy.IsBusy = true;
                Boolean isCheckPass = true;
                if (GridViewLong.SelectedItems.Count > 0)
                {
                    ObservableCollection<CP_LongOrderList> CP_LongOrderList = new ObservableCollection<CP_LongOrderList>();
                    foreach (var item in GridViewLong.SelectedItems)
                    {
                        CP_LongOrderList longOrder = (CP_LongOrderList)item;
                        if (longOrder.Yzzt.ToString() != "3200")
                        {
                            isCheckPass = false;
                            break;
                        }
                        else
                        {
                            longOrder.Yzzt = ConvertMy.ToDecimal("3201");
                            CP_LongOrderList.Add(longOrder);
                        }
                    }
                    if (!isCheckPass)
                    {
                        PublicMethod.RadAlterBox("请确保选中的医嘱的状态为【" + ColorKeyValuesProperty["3200"].Name + "】", "提示");
                        busy.IsBusy = false;
                    }
                    else
                    {
                        if (CP_LongOrderList.Count > 0)
                        {
                            serviceCon = PublicMethod.YidanClient;
                            serviceCon.UpdateLongOrderListYzztCompleted += (send, ea) =>
                            {
                                BindLong(string.Format(" and CP_LongOrder.Yzzt={0}", "3201"));
                                busy.IsBusy = false;
                            };
                            serviceCon.UpdateLongOrderListYzztAsync(CP_LongOrderList);
                        }
                    }
                }
                else
                {
                    PublicMethod.RadAlterBox("请选择医嘱！", "提示");
                    busy.IsBusy = false;
                }

            }
            catch (Exception ex)
            {
                busy.IsBusy = false;
            }
        }

        private void btnExec_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busy.IsBusy = true;
                Boolean isCheckPass = true;
                if (GridViewLong.SelectedItems.Count > 0)
                {
                    ObservableCollection<CP_LongOrderList> CP_LongOrderList = new ObservableCollection<CP_LongOrderList>();
                    foreach (var item in GridViewLong.SelectedItems)
                    {
                        CP_LongOrderList longOrder = (CP_LongOrderList)item;
                        if (longOrder.Yzzt.ToString() != "3201")
                        {
                            isCheckPass = false;
                            break;
                        }
                        else
                        {
                            longOrder.Yzzt = ConvertMy.ToDecimal("3202");
                            CP_LongOrderList.Add(longOrder);
                        }
                    }
                    if (!isCheckPass)
                    {
                        PublicMethod.RadAlterBox("请确保选中的医嘱的状态为【" + ColorKeyValuesProperty["3201"].Name + "】", "提示");
                        busy.IsBusy = false;
                    }
                    else
                    {
                        if (CP_LongOrderList.Count > 0)
                        {
                            serviceCon = PublicMethod.YidanClient;
                            serviceCon.UpdateLongOrderListYzztCompleted += (send, ea) =>
                            {
                                BindLong(string.Format(" and CP_LongOrder.Yzzt={0}", "3202"));
                                busy.IsBusy = false;
                            };
                            serviceCon.UpdateLongOrderListYzztAsync(CP_LongOrderList);
                        }
                    }
                }
                else
                {
                    PublicMethod.RadAlterBox("请选择医嘱！", "提示");
                    busy.IsBusy = false;
                }

            }
            catch (Exception ex)
            {
                busy.IsBusy = false;
            }
        }

        private void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busy.IsBusy = true;
                Boolean isCheckPass = true;
                if (GridViewLong.SelectedItems.Count > 0)
                {
                    ObservableCollection<CP_LongOrderList> CP_LongOrderList = new ObservableCollection<CP_LongOrderList>();
                    foreach (var item in GridViewLong.SelectedItems)
                    {
                        CP_LongOrderList longOrder = (CP_LongOrderList)item;
                        if (longOrder.Yzzt.ToString() != "3201")
                        {
                            isCheckPass = false;
                            break;
                        }
                        else
                        {
                            longOrder.Yzzt = ConvertMy.ToDecimal("3203");
                            CP_LongOrderList.Add(longOrder);
                        }
                    }
                    if (!isCheckPass)
                    {
                        PublicMethod.RadAlterBox("请确保选中的医嘱的状态为【" + ColorKeyValuesProperty["3201"].Name + "】", "提示");
                        busy.IsBusy = false;
                    }
                    else
                    {
                        if (CP_LongOrderList.Count > 0)
                        {
                            serviceCon = PublicMethod.YidanClient;
                            serviceCon.UpdateLongOrderListYzztCompleted += (send, ea) =>
                            {
                                BindLong(string.Format(" and CP_LongOrder.Yzzt={0}", "3203"));
                                busy.IsBusy = false;
                            };
                            serviceCon.UpdateLongOrderListYzztAsync(CP_LongOrderList);
                        }
                    }
                }
                else
                {
                    PublicMethod.RadAlterBox("请选择医嘱！", "提示");
                    busy.IsBusy = false;
                }

            }
            catch (Exception ex)
            {
                busy.IsBusy = false;
            }
        }

        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busy.IsBusy = true;
                Boolean isCheckPass = true;
                if (GridViewLong.SelectedItems.Count > 0)
                {
                    ObservableCollection<CP_LongOrderList> CP_LongOrderList = new ObservableCollection<CP_LongOrderList>();
                    foreach (var item in GridViewLong.SelectedItems)
                    {
                        CP_LongOrderList longOrder = (CP_LongOrderList)item;
                        if (longOrder.Yzzt.ToString() != "3202")
                        {
                            isCheckPass = false;
                            break;
                        }
                        else
                        {
                            longOrder.Yzzt = ConvertMy.ToDecimal("3204");
                            CP_LongOrderList.Add(longOrder);
                        }
                    }
                    if (!isCheckPass)
                    {
                        PublicMethod.RadAlterBox("请确保选中的医嘱的状态为【" + ColorKeyValuesProperty["3202"].Name + "】", "提示");
                        busy.IsBusy = false;
                    }
                    else
                    {
                        if (CP_LongOrderList.Count > 0)
                        {
                            serviceCon = PublicMethod.YidanClient;
                            serviceCon.UpdateLongOrderListYzztCompleted += (send, ea) =>
                            {
                                BindLong(string.Format(" and CP_LongOrder.Yzzt={0}", "3204"));
                                busy.IsBusy = false;
                            };
                            serviceCon.UpdateLongOrderListYzztAsync(CP_LongOrderList);
                        }
                    }
                }
                else
                {
                    PublicMethod.RadAlterBox("请选择医嘱！", "提示");
                    busy.IsBusy = false;
                }

            }
            catch (Exception ex)
            {
                busy.IsBusy = false;
            }
        }

        private void btnCheckTemp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busy.IsBusy = true;
                Boolean isCheckPass = true;
                if (GridViewTemp.SelectedItems.Count > 0)
                {
                    ObservableCollection<CP_TempOrderList> CP_TempOrderList = new ObservableCollection<CP_TempOrderList>();
                    foreach (var item in GridViewTemp.SelectedItems)
                    {
                        CP_TempOrderList tempOrder = (CP_TempOrderList)item;
                        if (tempOrder.Yzzt.ToString() != "3200")
                        {
                            isCheckPass = false;
                            break;
                        }
                        else
                        {
                            tempOrder.Yzzt = ConvertMy.ToDecimal("3201");
                            CP_TempOrderList.Add(tempOrder);
                        }
                    }
                    if (!isCheckPass)
                    {
                        PublicMethod.RadAlterBox("请确保选中的医嘱的状态为【" + ColorKeyValuesProperty["3200"].Name + "】", "提示");
                        busy.IsBusy = false;
                    }
                    else
                    {
                        if (CP_TempOrderList.Count > 0)
                        {
                            serviceCon = PublicMethod.YidanClient;
                            serviceCon.UpdateTempOrderListYzztCompleted += (send, ea) =>
                            {
                                BindTemp(string.Format(" and CP_TempOrder.Yzzt={0}", "3201"));
                                busy.IsBusy = false;
                            };
                            serviceCon.UpdateTempOrderListYzztAsync(CP_TempOrderList);
                        }
                    }
                }
                else
                {
                    PublicMethod.RadAlterBox("请选择医嘱！", "提示");
                    busy.IsBusy = false;
                }

            }
            catch (Exception ex)
            {
                busy.IsBusy = false;
            }
        }

        private void btnExecTemp_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                busy.IsBusy = true;
                Boolean isCheckPass = true;
                if (GridViewTemp.SelectedItems.Count > 0)
                {
                    ObservableCollection<CP_TempOrderList> CP_TempOrderList = new ObservableCollection<CP_TempOrderList>();
                    foreach (var item in GridViewTemp.SelectedItems)
                    {
                        CP_TempOrderList tempOrder = (CP_TempOrderList)item;
                        if (tempOrder.Yzzt.ToString() != "3201")
                        {
                            isCheckPass = false;
                            break;
                        }
                        else
                        {
                            tempOrder.Yzzt = ConvertMy.ToDecimal("3202");
                            CP_TempOrderList.Add(tempOrder);
                        }
                    }
                    if (!isCheckPass)
                    {
                        PublicMethod.RadAlterBox("请确保选中的医嘱的状态为【" + ColorKeyValuesProperty["3201"].Name + "】", "提示");
                        busy.IsBusy = false;
                    }
                    else
                    {
                        if (CP_TempOrderList.Count > 0)
                        {
                            serviceCon = PublicMethod.YidanClient;
                            serviceCon.UpdateTempOrderListYzztCompleted += (send, ea) =>
                            {
                                BindTemp(string.Format(" and CP_TempOrder.Yzzt={0}", "3202"));
                                busy.IsBusy = false;
                            };
                            serviceCon.UpdateTempOrderListYzztAsync(CP_TempOrderList);
                        }
                    }
                }
                else
                {
                    PublicMethod.RadAlterBox("请选择医嘱！", "提示");
                    busy.IsBusy = false;
                }

            }
            catch (Exception ex)
            {
                busy.IsBusy = false;
            }
        }
    }
}
