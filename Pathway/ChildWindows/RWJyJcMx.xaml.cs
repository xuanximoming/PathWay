using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;
using YidanSoft.Tool;

namespace YidanEHRApplication.Views.ChildWindows
{
    public partial class RWJyJcMx
    {

        #region 变量
        public ObservableCollection<CP_DoctorOrder> m_orderList = new ObservableCollection<CP_DoctorOrder>();
        public String s_xmdm = string.Empty;
        #endregion

        #region 事件
        public RWJyJcMx(String Sxmdm)
        {
            InitializeComponent();
            if (Sxmdm != "")
            {
                s_xmdm = Sxmdm;
            }

        }
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
            
            //绑定GirdView控件
            if (s_xmdm != "")
            {
                GetJyJcMxCategory();
            }
            
            
        }
        /// <summary>
        /// 根据项目代码获取检验检查套餐明细
        /// </summary>
        private void GetJyJcMxCategory()
        {
            try
            {

                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                client.GetJyJcMXInfoCompleted += (obj, ea) =>
                {
                    if (ea.Error == null)
                    {
                        m_orderList = ea.Result;
                        revJyJc.ItemsSource = ea.Result;
                    }
                };
                client.GetJyJcMXInfoAsync(s_xmdm);
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 行选中改变事件
        /// </summary>
        private void revJyJc_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {

                if (revJyJc.SelectedItem == null)
                {
                    return;
                }
                 

            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            m_orderList = (ObservableCollection<CP_DoctorOrder>)revJyJc.ItemsSource;
            this.DialogResult = true;
            this.Close();
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
            this.Close();
        }


        #region gridview rowloaded,menu,SelectionChanged
        #endregion
        private void revJyJc_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;

            RadContextMenu rowContextMenu = new RadContextMenu(); //新建一个右键菜单
            rowContextMenu.Width = 200;
            rowContextMenu.Items.Add(new RadMenuItem() { Header = "删除医嘱", Tag = TagName.Del });
            rowContextMenu.Items.Add(new RadMenuItem() { IsSeparator = true });
          
            //rowContextMenu.Items.Add(new RadMenuItem() { Header = "医嘱成组", Tag = TagName.Group });
            //rowContextMenu.Items.Add(new RadMenuItem() { Header = "取消成组", Tag = TagName.DisGroup });
 
            //添加右键菜单事件
            rowContextMenu.AddHandler(RadMenuItem.ClickEvent, new RoutedEventHandler(OnRowMenuItemClick));
            rowContextMenu.Opened += new RoutedEventHandler(rowContextMenu_Opened);
            RadContextMenu.SetContextMenu(e.Row, rowContextMenu);
            
        }
        private void OnRowMenuItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RadMenuItem clickedItem = ((RadRoutedEventArgs)e).OriginalSource as RadMenuItem;
                if (clickedItem != null)
                {
                    TagName tagName = (TagName)clickedItem.Tag;
                    CP_DoctorOrder selectedItem = revJyJc.SelectedItem as CP_DoctorOrder;
                   
                    //DialogParameters parameters = new DialogParameters();
                    switch (tagName)
                    {
                         
                        case TagName.Del:
                            RemoveOrder();
                            break;
                        default:
                            break;
                    }

                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
         
        /// <summary>
        /// 从LIST中移除医嘱 
        /// </summary>
        private void RemoveOrder()
        {
            if (revJyJc.SelectedItems == null)
                return;
            
            int selectItemsCount = revJyJc.SelectedItems.Count;
            for (int i = selectItemsCount - 1; i >= 0; i--)
            {
                CP_DoctorOrder order = revJyJc.SelectedItems[i] as CP_DoctorOrder;
                if (order.Yzzt == (decimal)OrderStatus.OrderInptut || order.Yzzt == 0)
                {
                    revJyJc.Items.Remove(order);
                   
                }
            }
        }
        /// <summary>
        /// 右键菜单
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rowContextMenu_Opened(object sender, RoutedEventArgs e)
        {
            GridViewRow row = ((RadRoutedEventArgs)e).OriginalSource as GridViewRow;
            List<CP_DoctorOrder> listsOrder = new List<CP_DoctorOrder>();
            var RadMenu = sender as RadContextMenu;
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
                        if ((TagName)item.Tag == TagName.Del)
                        {
                            item.IsEnabled = !(this.revJyJc.SelectedItems.Count > 1);
                        }

                        if ((TagName)item.Tag == TagName.Edit)
                        {
                            item.IsEnabled = !(this.revJyJc.SelectedItems.Count > 1);
                        }
                    }
                }
 
            }
        }
        

    #endregion

        #region 方法
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
        #endregion

    }
}

