using DrectSoft.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;
using YidanEHRApplication.Views.ChildWindows;
using tc = Telerik.Windows.Controls;

namespace YidanEHRApplication.Views
{
    public partial class W_AdviceSuit : Page
    {
        const string HeaderText = "成套医嘱提示"; //定义弹出框标题栏
        public enum PageSate { View, Edit }
        public W_AdviceSuit()
        {
            InitializeComponent();
            //去掉鼠标右键Silverlight 菜单
            SettingDetails.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };
            BindListKeShi();
            ManagerControlState();
            GetCP_AdviceSuitCategory();
        }

        #region 属性
        /// <summary>
        /// 指示当前列表绑定的类型 科室=2091，个人=2093 使用范围(CP_DataCategory.Mxbh, Lbbh = 29)
        /// </summary>
        String Syfw = "2091";//科室医嘱套餐
        /// <summary>
        /// 套装主键
        /// </summary>
        public String Ctyzxh
        {
            get
            {
                /****** update by dxj 2011/7/15 ****/
                //if (lbxKeShi.SelectedItems.Count < 1) return null;
                //CP_AdviceSuit suit = (CP_AdviceSuit)lbxKeShi.SelectedItem;
                if (tvList.SelectedItem == null)
                {
                    return null;
                }
                if (((RadTreeViewItem)tvList.SelectedItem).Tag is CP_AdviceSuit)
                {
                    CP_AdviceSuit suit = (CP_AdviceSuit)((RadTreeViewItem)tvList.SelectedItem).Tag;
                    return suit.Ctyzxh.ToString();
                }
                return null;
            }
        }


        /// <summary>
        /// 选中医嘱明细
        /// </summary>
        public CP_AdviceSuitDetail CP_AdviceSuitDetail
        {
            get
            {
                if (GridViewYaoPin.SelectedItem == null) return null;
                CP_AdviceSuitDetail d = (CP_AdviceSuitDetail)GridViewYaoPin.SelectedItem; /****** update by dxj 2011/7/15 ****/
                return d;
            }
        }
        PageSate _PageStateCurrent = PageSate.View;

        /// <summary>
        /// 当前页面状态
        /// </summary>
        public PageSate PageStateCurrent
        {
            get { return _PageStateCurrent; }
            set
            {
                _PageStateCurrent = value;
                ManagerControlState();
            }
        }

        #endregion

        #region 事件
        RadTreeViewItem selectedItem;//选中节点
        // List<RadTreeViewItem> SelectItemTree;//选中节点的上级列表,包括选中节点
        List<RadTreeViewItem> AllTreeVewItems;//所有节点
        public List<String> CategoryIdList = new List<string>();//选中节点的所有上级节点categoryID


        private void tvList_SelectionChanged(object sender, tc.SelectionChangedEventArgs e)
        {
            try
            {
                PageStateCurrent = PageSate.View;
                if (tvList.SelectedItem == null)
                    return;
                //add by luff 20120813 
                //选择医嘱套餐
                if (((RadTreeViewItem)tvList.SelectedItem).Tag is CP_AdviceSuit)
                {
                    BindGridViewYaoPin();
                    this.btnKeShiAddDetail.IsEnabled = true;
                    this.btnKeShiEditDetail.IsEnabled = true;
                    btnKeShiDeleteDetail.IsEnabled = true;
                    this.btnKeShiGroupDetail.IsEnabled = true;
                    this.btnKeShiCancelGroupDetail.IsEnabled = true;
                    btnKeShiUserReason.IsEnabled = true;
                    this.btnCategory.IsEnabled = false;
                    this.btnKeShiAdd.IsEnabled = false;
                    btnKeShiDelete.IsEnabled = true;
                }
                //选中套餐类别
                if (tvList.SelectedItem != null && ((RadTreeViewItem)tvList.SelectedItem).Tag is CP_AdviceSuitCategory)
                {
                    BindGridViewYaoPin();
                    this.btnKeShiAddDetail.IsEnabled = false;
                    this.btnKeShiEditDetail.IsEnabled = false;
                    btnKeShiDeleteDetail.IsEnabled = false;
                    this.btnKeShiGroupDetail.IsEnabled = false;
                    this.btnKeShiCancelGroupDetail.IsEnabled = false;
                    btnKeShiUserReason.IsEnabled = false;
                    this.btnCategory.IsEnabled = true;
                    this.btnKeShiAdd.IsEnabled = true;
                    btnKeShiDelete.IsEnabled = false;
                    selectedItem = (RadTreeViewItem)tvList.SelectedItem;
                    if (CategoryIdList == null) CategoryIdList = new List<string>();
                    CategoryIdList.Clear();
                    CategoryIdList.Insert(0, ((CP_AdviceSuitCategory)((RadTreeViewItem)tvList.SelectedItem).Tag).ParentID);
                    //CategoryIdList.Add("Top");
                    CategoryIdList.Add(((CP_AdviceSuitCategory)((RadTreeViewItem)tvList.SelectedItem).Tag).CategoryId);
                    for (int i = 0; i < 20; i++)
                    {
                        foreach (var item in AllTreeVewItems)
                        {
                            String parentId2 = ((CP_AdviceSuitCategory)((RadTreeViewItem)item).Tag).CategoryId;
                            String currentId = CategoryIdList[0];
                            if (parentId2 == currentId)
                            {
                                if (!CategoryIdList.Contains(((CP_AdviceSuitCategory)((RadTreeViewItem)item).Tag).ParentID))
                                    CategoryIdList.Insert(0, ((CP_AdviceSuitCategory)((RadTreeViewItem)item).Tag).ParentID);

                            }
                        }
                    }


                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void chk_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                Syfw = ((RadioButton)sender).CommandParameter.ToString();
                BindListKeShi();
                GetCP_AdviceSuitCategory();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        #region 套餐

        #region 添加套餐


        void btnKeShiAdd_Click(object sender, RoutedEventArgs e)
        {
            /****** update by dxj 2011/7/15 ****/
            if (tvList.SelectedItem == null || !(((RadTreeViewItem)tvList.SelectedItem).Tag is CP_AdviceSuitCategory))
            {
                PublicMethod.RadAlterBox("请选择医嘱套餐类别", "提示");
                return;
            }
            W_AdviceSuitAdd w_AdviceSuitAdd = new W_AdviceSuitAdd("添加", Syfw, tvList.SelectedItem);
            //selectedItem = (RadTreeViewItem)tvList.SelectedItem;

            w_AdviceSuitAdd.ShowDialog();
            w_AdviceSuitAdd.Closed += new EventHandler<WindowClosedEventArgs>(w_AdviceSuitAdd_Closed);
            //RadWindow w = new RadWindow();
            //w.Closed += new EventHandler<WindowClosedEventArgs>(AddKeShi_Closed);
            //new PublicMethod().ShowInputWindow(ref  w, "新增医嘱套餐", "输入套餐名称", null, null);
        }

        void w_AdviceSuitAdd_Closed(object sender, WindowClosedEventArgs e)
        {
            GetCP_AdviceSuitCategory();
            //tvList.SetValue(TreeView.SelectedValueProperty, ((CP_AdviceSuitCategory)selectedItem.Tag).CategoryId);
            //tvList.SelectedItem=    tvList.SelectedItems.Where(s => ((CP_AdviceSuitCategory)((RadTreeViewItem)s).Tag).CategoryId == ((CP_AdviceSuitCategory)selectedItem.Tag).CategoryId).ToList()[0];
        }

        void AddKeShi_Closed(object sender, WindowClosedEventArgs e)
        {
            try
            {
                if (e.PromptResult == null) return;
                YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                CP_AdviceSuit suit = new CP_AdviceSuit();
                suit.Name = e.PromptResult;
                suit.Syfw = Syfw;
                if (suit.Syfw.Equals("2903"))
                    suit.Ysdm = Global.LogInEmployee.Zgdm;
                suit.Zgdm = Global.LogInEmployee.Zgdm;
                suit.Ksdm = Global.LogInEmployee.Ksdm;
                suit.Bqdm = Global.LogInEmployee.Bqdm;
                ServiceClient.AddCP_AdviceSuitCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error != null) PublicMethod.RadWaringBox(ea.Error);
                            else
                                BindListKeShi();
                        };
                ServiceClient.AddCP_AdviceSuitAsync(suit);
                ServiceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }



        #endregion

        #region 删除套餐
        void btnKeShiDelete_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (Ctyzxh == null) { RadWindow.Alert("请选中医嘱套餐，再进行相关操作！"); return; }
            //    RadWindow w = new RadWindow();
            //    CP_AdviceSuit suit = (CP_AdviceSuit)((RadTreeViewItem)tvList.SelectedItem).Tag; /****** update by dxj 2011/7/15 ****/
            //    // (CP_AdviceSuit)lbxKeShi.SelectedItem;
            //    //RadWindow.Confirm("确定删除医嘱套餐吗？警告删除后不能恢复", new EventHandler<WindowClosedEventArgs>(Delete_Closed));

            //    DialogParameters parameters = new DialogParameters();/* uodate by dxj 2011-7-26 删除提示 */
            //    parameters.Content = String.Format("提示: {0}", "确定删除吗？删除后不能恢复!");
            //    parameters.Header = "提示";
            //    parameters.IconContent = null;
            //    parameters.OkButtonContent = "确定";
            //    parameters.CancelButtonContent = "取消";

            //    RadWindow.Confirm(parameters,new EventHandler<WindowClosedEventArgs>(Delete_Closed));
            //}
            //catch (Exception ex)
            //{
            //    YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            //}

            //Image img = new Image();
            //img.Source = img.se "/Pathway;component/Images/使用原因.png";
            try
            {

                if (Ctyzxh == null)
                {
                    PublicMethod.RadAlterBox("请选中一条医嘱套餐数据!", "提示");
                    return;
                }
                #region 删除提示
                //DialogParameters parameters = new DialogParameters();/* update by luff 2012-08-20 删除提示 */
                //parameters.Content = String.Format("{0}", "确定删除吗？删除后不能恢复!");
                //parameters.Header = "提示";
                //parameters.IconContent = null;
                //parameters.OkButtonContent = "确定"; 
                //parameters.CancelButtonContent = "取消";
                //parameters.Closed = Delete_Closed;
                //RadWindow.Confirm(parameters);
                YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                mess.ShowDialog();
                mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
                #endregion
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
                    try
                    {
                        YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                        CP_AdviceSuit suit = (CP_AdviceSuit)((RadTreeViewItem)tvList.SelectedItem).Tag; /****** update by dxj 2011/7/15 ****/
                        // (CP_AdviceSuit)lbxKeShi.SelectedItem;
                        ServiceClient.DeleteCP_AdviceSuitCompleted +=
                                (obj, ea) =>
                                {
                                    if (ea.Error != null) PublicMethod.RadWaringBox(ea.Error);
                                    else
                                        GetCP_AdviceSuitCategory();
                                };
                        ServiceClient.DeleteCP_AdviceSuitAsync(suit.Ctyzxh);
                        ServiceClient.CloseAsync();
                    }
                    catch (Exception ex)
                    {
                        YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                    }

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void Delete_Closed(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                try
                {
                    YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                    CP_AdviceSuit suit = (CP_AdviceSuit)((RadTreeViewItem)tvList.SelectedItem).Tag; /****** update by dxj 2011/7/15 ****/
                    // (CP_AdviceSuit)lbxKeShi.SelectedItem;
                    ServiceClient.DeleteCP_AdviceSuitCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error != null) PublicMethod.RadWaringBox(ea.Error);
                                else
                                    GetCP_AdviceSuitCategory();
                            };
                    ServiceClient.DeleteCP_AdviceSuitAsync(suit.Ctyzxh);
                    ServiceClient.CloseAsync();
                }
                catch (Exception ex)
                {
                    YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            }

        }



        #endregion

        #region 编辑套餐


        #endregion

        #region 列表控件SelectChange事件
        void lbxKeShi_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                PageStateCurrent = PageSate.View;
                BindGridViewYaoPin();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        #endregion

        //#region 上移
        //void btnKeShiUp_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (Ctyzxh == null) { RadWindow.Alert("请选中医嘱套餐，再进行相关操作！"); return; }
        //        RadWindow w = new RadWindow();
        //        CP_AdviceSuit suit = (CP_AdviceSuit)((RadTreeViewItem)tvList.SelectedItem).Tag; /****** update by dxj 2011/7/15 ****/
        //        // (CP_AdviceSuit)lbxKeShi.SelectedItem;
        //        YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
        //        ServiceClient.UPCP_AdviceSuitCompleted +=
        //            (obj, ea) =>
        //            {
        //                if (ea.Error != null) PublicMethod.RadWaringBox(ea.Error);
        //                else
        //                    GetCP_AdviceSuitCategory();
        //            };
        //        ServiceClient.UPCP_AdviceSuitAsync(suit);
        //        ServiceClient.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
        //    }
        //}
        //#endregion

        //#region 下移
        //void btnKeShiDown_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        if (Ctyzxh == null) { RadWindow.Alert("请选中医嘱套餐，再进行相关操作！"); return; }
        //        RadWindow w = new RadWindow();
        //        CP_AdviceSuit suit = (CP_AdviceSuit)((RadTreeViewItem)tvList.SelectedItem).Tag; /****** update by dxj 2011/7/15 ****/
        //        // (CP_AdviceSuit)lbxKeShi.SelectedItem;
        //        YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
        //        ServiceClient.DownCP_AdviceSuitCompleted +=
        //                (obj, ea) =>
        //                {
        //                    if (ea.Error != null) PublicMethod.RadWaringBox(ea.Error);
        //                    else
        //                        GetCP_AdviceSuitCategory();
        //                };
        //        ServiceClient.DownCP_AdviceSuitAsync(suit);
        //        ServiceClient.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
        //    }
        //}
        //#endregion

        #region 套餐类别维护
        private void btnCategory_Click(object sender, RoutedEventArgs e)
        {
            AdviceSuitCategoryManage categoryManage = new AdviceSuitCategoryManage();
            categoryManage.ShowDialog();
            categoryManage.Closed += new EventHandler<WindowClosedEventArgs>(categoryManage_Closed);
        }

        void categoryManage_Closed(object sender, WindowClosedEventArgs e)
        {
            GetCP_AdviceSuitCategory();
        }
        #endregion

        #region 使用原因
        private void btnKeShiUserReason_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (GridViewYaoPin.SelectedItem != null)
                //{
                //    PublicMethod.RadAlterBox("只有医嘱套餐才能查看使用原因！", "提示");
                //    return;
                //}
                RadWindow w = new RadWindow();
                w.Closed += new EventHandler<WindowClosedEventArgs>(UserReason_Closed);

                CP_AdviceSuit suit = (CP_AdviceSuit)((RadTreeViewItem)tvList.SelectedItem).Tag; /****** update by dxj 2011/7/15 ****/
                String[] UserReason = new String[3];
                UserReason[0] = suit.UserReason1;
                UserReason[1] = suit.UserReason2;
                UserReason[2] = suit.UserReason3;
                //判断是否有相关使用原因 add by luff 2012-08-08
                if (UserReason.Length > 0)
                {
                    ShowInputWindow(ref  w, UserReason);
                }
                else
                {
                    PublicMethod.RadAlterBox("没有相关使用原因", "提示");
                }
            }
            catch (Exception ex)
            {
                ///不处理根节点异常
                //YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        void UserReason_Closed(object sender, WindowClosedEventArgs e)
        {
            try
            {
                //if (e.PromptResult == null) return;
                RadWindow w = (RadWindow)sender;
                StackPanel sp = (StackPanel)w.Content;
                CP_AdviceSuit suit = (CP_AdviceSuit)((RadTreeViewItem)tvList.SelectedItem).Tag;
                foreach (UIElement u in sp.Children)
                {
                    //add by luff 2012-08-21
                    if (u is TextBox && ((TextBox)u).Name == "txt1")
                    {
                        if (((TextBox)u).Text.Length > 50)
                        {
                            suit.UserReason1 = ((TextBox)u).Text.Trim().Substring(0, 50);
                        }
                        else
                        {
                            suit.UserReason1 = ((TextBox)u).Text.Trim();
                        }
                    }

                    if (u is TextBox && ((TextBox)u).Name == "txt2")
                    {
                        if (((TextBox)u).Text.Length > 50)
                        {
                            suit.UserReason2 = ((TextBox)u).Text.Trim().Substring(0, 50);
                        }
                        else
                        {
                            suit.UserReason2 = ((TextBox)u).Text.Trim();
                        }
                    }


                    if (u is TextBox && ((TextBox)u).Name == "txt3")
                    {
                        if (((TextBox)u).Text.Length > 50)
                        {
                            suit.UserReason3 = ((TextBox)u).Text.Trim().Substring(0, 50);
                        }
                        else
                        {
                            suit.UserReason3 = ((TextBox)u).Text.Trim();
                        }
                    }
                }
                YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                ServiceClient.EditCP_AdviceSuitCompleted +=
                    (obj, ea) =>
                    {
                        if (ea.Error != null) PublicMethod.RadWaringBox(ea.Error);
                        else
                            BindListKeShi();
                    };
                ServiceClient.EditCP_AdviceSuitAsync(suit);
                ServiceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }




        /// <summary>
        /// 显示包含三个输入框的窗口
        /// </summary>
        /// <param name="w">窗口手柄</param>
        /// <param name="Title">窗口标题</param>
        /// <param name="TipString">提示信息</param>
        /// <param name="DefaultText">输入框默认值</param>
        /// <param name="buttonContent">按钮显示的名称</param>
        public void ShowInputWindow(ref RadWindow w, String[] DefaultText)
        {
            w.Style = (Style)Application.Current.Resources["RadWindowStyle"];

            Border b = new Border();
            b.Height = 1;
            b.BorderBrush = ConvertColor.GetColorBrushFromHx16("42A5FF");
            b.BorderThickness = new Thickness(1);
            b.HorizontalAlignment = HorizontalAlignment.Stretch;
            b.Margin = new Thickness(20, 10, 0, 5);

            TextBlock txb1 = new TextBlock();
            txb1.Text = "原因一：";
            txb1.Margin = new Thickness(20, 10, 0, 0);

            TextBox txt1 = new TextBox();
            txt1.Name = "txt1";
            txt1.Text = DefaultText[0];
            txt1.Width = 400;
            txt1.HorizontalAlignment = HorizontalAlignment.Left;
            txt1.Margin = new Thickness(20, 0, 0, 0);

            TextBlock txb2 = new TextBlock();
            txb2.Text = "原因二：";
            txb2.Margin = new Thickness(20, 5, 0, 0);

            TextBox txt2 = new TextBox();
            txt2.Name = "txt2";
            txt2.Text = DefaultText[1];
            txt2.Width = 400;
            txt2.HorizontalAlignment = HorizontalAlignment.Left;
            txt2.Margin = new Thickness(20, 0, 0, 0);

            TextBlock txb3 = new TextBlock();
            txb3.Text = "原因三：";
            txb3.Margin = new Thickness(20, 5, 0, 0);

            TextBox txt3 = new TextBox();
            txt3.Name = "txt3";
            txt3.Text = DefaultText[2];
            txt3.Width = 400;
            txt3.HorizontalAlignment = HorizontalAlignment.Left;
            txt3.Margin = new Thickness(20, 0, 0, 0);

            RadButton btn = new RadButton();
            btn.Content = "保存";
            btn.CommandParameter = w;
            btn.Click += new RoutedEventHandler(btn_Click);
            btn.HorizontalAlignment = HorizontalAlignment.Center;
            btn.Width = 50;
            btn.Margin = new Thickness(10, 0, 10, 0);



            RadButton btnCancel = new RadButton();
            btnCancel.Content = "取消";

            btnCancel.CommandParameter = w;
            btnCancel.Click += new RoutedEventHandler(btnCancel_Click);
            btnCancel.HorizontalAlignment = HorizontalAlignment.Center;
            btnCancel.Width = 50;
            btnCancel.Margin = new Thickness(10, 0, 10, 0);

            StackPanel spbtnContaim = new StackPanel();
            spbtnContaim.Orientation = Orientation.Horizontal;
            spbtnContaim.Children.Add(btnCancel);
            spbtnContaim.Children.Add(btn);
            spbtnContaim.FlowDirection = FlowDirection.RightToLeft;
            spbtnContaim.Margin = new Thickness(0, 0, 20, 0);

            StackPanel sp = new StackPanel();
            sp.Width = 450;
            sp.Height = 200;
            sp.Children.Add(txb1);
            sp.Children.Add(txt1);
            sp.Children.Add(txb2);
            sp.Children.Add(txt2);
            sp.Children.Add(txb3);
            sp.Children.Add(txt3);
            sp.Children.Add(b);
            sp.Children.Add(spbtnContaim);

            w.Header = "医嘱套餐使用原因";
            w.Content = sp;
            w.ShowDialog();

        }

        void btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RadButton btn = (RadButton)sender;
                ((RadWindow)(btn.CommandParameter)).Close();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        void btnCancel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RadButton btn = (RadButton)sender;
                ((RadWindow)(btn.CommandParameter)).Close();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #endregion
        #region 明细
        #region 状态控制
        /// <summary>
        /// 锁定套餐，打开明细编辑状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnKeShiEditDetails_Click(object sender, RoutedEventArgs e)
        {
            PageStateCurrent = PageSate.Edit;
        }

        /// <summary>
        /// 锁定明细，打开套餐编辑状态
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnKeShiSaveDetail_Click(object sender, RoutedEventArgs e)
        {
            PageStateCurrent = PageSate.View;
        }
        #endregion
        #region 添加医嘱
        void btnKeShiAddDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RWAdviceMaintain w = new RWAdviceMaintain(Ctyzxh, ManualType.New, null, 0);
                w.ShowDialog();
                w.Closed += new EventHandler<WindowClosedEventArgs>(w_Closed);
                w.RefreshEventHandler += new EventHandler(RefreshEventHandler);

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 切换选项卡刷新GridView
        /// 创建：Jhonny
        /// 创建时间：2013年8月22日 17:39:34
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshEventHandler(object sender, EventArgs e)
        {
            BindGridViewYaoPin();
        }

        #endregion
        #region 编辑医嘱
        void btnKeShiEditDetail_Click(object sender, RoutedEventArgs e)
        {
            if (GridViewYaoPin.SelectedItems.Count != 1)
            {
                PublicMethod.RadAlterBox("请选中一条医嘱，再进行相关操作！", "提示");
                return;
            }
            RadWindow w = new RWAdviceMaintain(Ctyzxh, ManualType.Edit, CP_AdviceSuitDetail, 0);
            w.Closed += new EventHandler<WindowClosedEventArgs>(w_Closed);
            w.ShowDialog();
        }
        #endregion
        #region 删除医嘱
        void btnKeShiDeleteDetail_Click(object sender, RoutedEventArgs e)
        {
            //try
            //{
            //    if (CP_AdviceSuitDetail == null) { RadWindow.Alert("请选中医嘱，再进行相关操作！"); return; }
            //    RadWindow.Confirm("确实删除选中的医嘱吗？警告删除后不能恢复！", new EventHandler<WindowClosedEventArgs>(DeleteDetail));
            //}
            //catch (Exception ex)
            //{
            //    YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            //}

            try
            {

                if (CP_AdviceSuitDetail == null)
                {
                    PublicMethod.RadAlterBox("请选中医嘱，再进行相关操作！", "提示");
                    return;
                }
                #region 删除提示
                //DialogParameters parameters = new DialogParameters();/* uodate by dxj 2011-7-26 删除提示 */
                //parameters.Content = String.Format("{0}", "确定删除选中的医嘱吗？警告删除后不能恢复！");
                //parameters.Header = "提示";
                //parameters.IconContent = null;
                //parameters.OkButtonContent = "确定";
                //parameters.CancelButtonContent = "取消";
                //parameters.Closed = DeleteDetail;
                //RadWindow.Confirm(parameters);

                YidanPathWayMessageBox mess = new YidanPathWayMessageBox("确定删除选中的医嘱吗？警告删除后不能恢复！", "提示", YiDanMessageBoxButtons.YesNo);
                mess.ShowDialog();
                mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent2);
                #endregion
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }



        void mess_PageClosedEvent2(object sender, bool e)
        {
            try
            {
                if (e == true)
                {

                    YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                    //ServiceClient.DeleteCP_AdviceSuitDetailCompleted += new EventHandler<DeleteCP_AdviceSuitDetailCompletedEventArgs>(DeleteDetail);
                    ServiceClient.DeleteCP_AdviceSuitDetailCompleted += (send, ea) =>
                    {
                        if (ea.Error == null)
                        {
                            if (ea.Result > 1)
                            {
                                PublicMethod.RadAlterBox("该医嘱已经成组，请先取消成组，再继续删除操作！", "提示");
                                return;
                            }
                            if (ea.Result <= 1 && ea.Result > 0)
                            {
                                PublicMethod.RadAlterBox("删除成功！", "提示");
                                BindGridViewYaoPin();
                            }
                        }

                    };
                    ServiceClient.DeleteCP_AdviceSuitDetailAsync(CP_AdviceSuitDetail.Ctmxxh);
                    ServiceClient.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        //删除医嘱套餐模板 update by luff 2012-08-20
        void DeleteDetail(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == null || e.DialogResult == false) return;
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            //ServiceClient.DeleteCP_AdviceSuitDetailCompleted += new EventHandler<DeleteCP_AdviceSuitDetailCompletedEventArgs>(DeleteDetail);
            ServiceClient.DeleteCP_AdviceSuitDetailCompleted += (send, ea) =>
            {
                if (ea.Error == null)
                {
                    if (ea.Result > 1)
                    {
                        PublicMethod.RadAlterBox("该医嘱已经成组，请先取消成组，再继续删除操作！", "提示");
                        return;
                    }
                    if (ea.Result <= 1 && ea.Result > 0)
                    {
                        PublicMethod.RadAlterBox("删除成功！", "提示");
                        BindGridViewYaoPin();
                    }
                }

            };
            ServiceClient.DeleteCP_AdviceSuitDetailAsync(CP_AdviceSuitDetail.Ctmxxh);
            ServiceClient.CloseAsync();
        }

        void DeleteDetail(object sender, DeleteCP_AdviceSuitDetailCompletedEventArgs e)
        {
            if (e.Error != null) PublicMethod.RadWaringBox(e.Error);
            else
                BindGridViewYaoPin();
        }

        #endregion
        #region 成组
        private void btnKeShiGroupDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GridViewYaoPin.SelectedItems.Count < 2)
                {
                    PublicMethod.RadAlterBox("请选多条医嘱，再进行相关操作！", "提示");
                    return;
                }

                ObservableCollection<CP_AdviceSuitDetail> CP_AdviceSuitDetailList = new ObservableCollection<CP_AdviceSuitDetail>();
                CP_AdviceSuitDetail d = null;

                foreach (object o in GridViewYaoPin.SelectedItems)
                {
                    //判断用法与频次
                    for (int i = 0; i < GridViewYaoPin.SelectedItems.Count; i++)
                    {
                        if (((CP_AdviceSuitDetail)GridViewYaoPin.SelectedItems[i]).Yfdm != ((CP_AdviceSuitDetail)o).Yfdm //用法判断
                            || ((CP_AdviceSuitDetail)GridViewYaoPin.SelectedItems[i]).Pcdm != ((CP_AdviceSuitDetail)o).Pcdm) //频次判断  
                        {
                            PublicMethod.RadAlterBox("所选医嘱中【用法、频次】存在不一致，无法成组！", "提示");
                            return;
                        }
                    }

                    if (d != null && d.Yzbz != ((CP_AdviceSuitDetail)o).Yzbz)
                    {
                        PublicMethod.RadAlterBox("请确保选中的医嘱是相同的医嘱类型，再进行相关操作！", "提示");
                        return;
                    }
                    d = (CP_AdviceSuitDetail)o;
                    if (d.Fzbz != "3500" && !String.IsNullOrEmpty(d.Fzbz))
                    {
                        PublicMethod.RadAlterBox("请确定选中医嘱都未成组，再进行相关操作！", "提示");
                        return;
                    }
                    CP_AdviceSuitDetailList.Add(d);
                }

                YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                ServiceClient.GroupCP_AdviceSuitDetailCompleted +=
                    (obj, ea) =>
                    {
                        if (ea.Error != null) PublicMethod.RadWaringBox(ea.Error);
                        else
                        {
                            PublicMethod.RadAlterBox("医嘱成组成功", "信息提示");
                            BindGridViewYaoPin();
                        }
                    };

                ServiceClient.GroupCP_AdviceSuitDetailAsync(CP_AdviceSuitDetailList);
                ServiceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        #endregion
        #region 取消成组
        private void btnKeShiCancelGroupDetail_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GridViewYaoPin.SelectedItems.Count > 0)
                {
                    YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                    ServiceClient.CancleGroupCP_AdviceSuitDetailCompleted +=
                            (obj, ea) =>
                            {
                                //修改没有成组的信息也能取消成组的bug
                                //时间：2013年7月31日 10:33:44
                                //修改人：Jhonny
                                if (ea.Error != null) PublicMethod.RadWaringBox(ea.Error);
                                else
                                {
                                    if (CP_AdviceSuitDetail.Fzxh.Equals(CP_AdviceSuitDetail.Ctmxxh.ToString()) && GridViewYaoPin.SelectedItems.Count > 1)
                                    {
                                        if (!(CP_AdviceSuitDetail.Fzbz != "3500" && !String.IsNullOrEmpty(CP_AdviceSuitDetail.Fzbz)))
                                        {
                                            PublicMethod.RadAlterBox("请选择成组医嘱!", "提示");
                                        }
                                        else
                                        {
                                            PublicMethod.RadAlterBox("医嘱取消成组成功", "提示信息");
                                            BindGridViewYaoPin();
                                        }
                                    }
                                    else if (CP_AdviceSuitDetail.Fzbz != "3500" && !String.IsNullOrEmpty(CP_AdviceSuitDetail.Fzbz))
                                    {
                                        PublicMethod.RadAlterBox("请确定选中医嘱都已成组，再进行相关操作！", "提示");
                                        return;
                                    }
                                    //else if (!(CP_AdviceSuitDetail.Fzbz != "3500" && !String.IsNullOrEmpty(CP_AdviceSuitDetail.Fzbz)))
                                    //{
                                    //    PublicMethod.RadAlterBox("请确定选中医嘱都已成组，在进行相关操作！","提示");
                                    //}
                                    else if (GridViewYaoPin.SelectedItems.Count == 1)
                                    {
                                        PublicMethod.RadAlterBox("请选择多条医嘱在进行操作！", "提示信息");
                                        return;
                                    }
                                    else
                                    {
                                        PublicMethod.RadAlterBox("请选择对应的成组医嘱!", "提示信息");
                                        return;
                                    }
                                }
                            };
                    ServiceClient.CancleGroupCP_AdviceSuitDetailAsync(CP_AdviceSuitDetail.Fzxh);
                    ServiceClient.CloseAsync();
                }
                else
                {
                    PublicMethod.RadAlterBox("请只选中一条医嘱，再进行相关操作！", "提示");
                    return;
                }

                //判断是否选择数据，否则给予提示。
                /*
                if (GridViewYaoPin.SelectedItems.Count != 1) 
                { 
                    PublicMethod.RadAlterBox("请只选中一条医嘱，再进行相关操作！", "提示");
                    return; 
                }
                */
                //所选操作医嘱必须要大于，等于一的时候，不能进行取消成组，取消成组按钮禁用
                //if (GridViewYaoPin.SelectedItems.Count == 1)   //修改后
                //{
                //    //选中一行的时候取消功能禁用
                //    this.btnKeShiCancelGroupDetail.IsEnabled = false;
                //}
                //else if (GridViewYaoPin.SelectedItems.Count>1 && 1==2)
                //{
                //    PublicMethod.RadAlterBox("所选数据没有成组医嘱需要取消", "成套医嘱提示");
                //}
                //else
                //{
                //    YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                //    ServiceClient.CancleGroupCP_AdviceSuitDetailCompleted +=
                //            (obj, ea) =>
                //            {
                //                if (ea.Error != null) PublicMethod.RadWaringBox(ea.Error);
                //                else
                //                {
                //                    PublicMethod.RadAlterBox("医嘱取消成组成功", "信息提示");
                //                    BindGridViewYaoPin();
                //                }
                //            };
                //    ServiceClient.CancleGroupCP_AdviceSuitDetailAsync(CP_AdviceSuitDetail.Fzxh);
                //    ServiceClient.CloseAsync();
                //    //

                //}
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #endregion
        #region 弹出窗口关闭事件
        /// <summary>
        /// 弹出窗口关闭事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void w_Closed(object sender, WindowClosedEventArgs e)
        {
            try
            {
                BindGridViewYaoPin();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        #endregion
        #endregion
        #endregion

        #region 函数
        #region 绑定列表框
        void BindListKeShi()
        {
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            ServiceClient.GetCP_AdviceSuitCompleted += new EventHandler<GetCP_AdviceSuitCompletedEventArgs>(BindListKeShi);
            if (Syfw == "2903")//个人医嘱
                ServiceClient.GetCP_AdviceSuitAsync(String.Format(" and Syfw={0} and Ysdm='{1}'", Syfw, Global.LogInEmployee.Zgdm));
            if (Syfw == "2901")//科室医嘱
                ServiceClient.GetCP_AdviceSuitAsync(String.Format(" and Syfw={0} and Ksdm='{1}'", Syfw, Global.LogInEmployee.Ksdm));

            ServiceClient.CloseAsync();
        }

        void BindListKeShi(object sender, GetCP_AdviceSuitCompletedEventArgs e)
        {
            if (e.Error != null) PublicMethod.RadWaringBox(e.Error);
            else
                lbxKeShi.ItemsSource = e.Result.ToList();



        }

        #endregion
        #region 绑定GridView
        void BindGridViewYaoPin()
        {
            if (Ctyzxh == null)
            {
                //add by luff 20120813 
                GridViewYaoPin.ItemsSource = null;
                return;
            }
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            ServiceClient.GetCP_AdviceSuitDetailCompleted += new EventHandler<GetCP_AdviceSuitDetailCompletedEventArgs>(BindGridViewYaoPin);
            ServiceClient.GetCP_AdviceSuitDetailAsync(String.Format(" and Ctyzxh={0} ", Ctyzxh));
            ServiceClient.CloseAsync();
        }

        void BindGridViewYaoPin(object sender, GetCP_AdviceSuitDetailCompletedEventArgs e)
        {
            if (e.Error != null) PublicMethod.RadWaringBox(e.Error);
            else GridViewYaoPin.ItemsSource = e.Result.ToList();

        }

        #endregion

        #region 控制控件是否可用
        /// <summary>
        /// 管理页面控件是否可用
        /// </summary>
        void ManagerControlState()
        {
            //Boolean IslbxSelected = Ctyzxh == null ? false : true;
            //Boolean isPageView = PageStateCurrent == PageSate.View;
            //btnKeShiEditDetails.IsEnabled = isPageView && IslbxSelected;
            //lbxKeShi.IsEnabled = isPageView;
            ////btnKeShiEdit.IsEnabled = isPageView && IslbxSelected;
            //btnKeShiAdd.IsEnabled = isPageView;
            //btnKeShiDelete.IsEnabled = isPageView && IslbxSelected;
            ////btnKeShiDown.IsEnabled = isPageView && IslbxSelected;
            ////btnKeShiUp.IsEnabled = isPageView && IslbxSelected;
            //btnKeShiSaveDetail.IsEnabled = !isPageView;
            //btnKeShiAddDetail.IsEnabled = !isPageView;
            //btnKeShiDeleteDetail.IsEnabled = !isPageView;
            //btnKeShiEditDetail.IsEnabled = !isPageView;
            //btnKeShiGroupDetail.IsEnabled = !isPageView;
            //btnKeShiCancelGroupDetail.IsEnabled = !isPageView;
            //btnKeShiUserReason.IsEnabled = isPageView && IslbxSelected;
            //chkGeRen.IsEnabled = lbxKeShi.IsEnabled;
            //chkKeShi.IsEnabled = lbxKeShi.IsEnabled;

        }
        #endregion


        #endregion

        void EditKeShi_Closed(object sender, WindowClosedEventArgs e)
        {
            try
            {
                if (e.PromptResult == null) return;
                YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                CP_AdviceSuit suit = (CP_AdviceSuit)((RadTreeViewItem)tvList.SelectedItem).Tag; /****** update by dxj 2011/7/15 ****/
                suit.Name = e.PromptResult;
                suit.Syfw = Syfw;
                ServiceClient.EditCP_AdviceSuitCompleted +=

                (obj, ea) =>
                {
                    if (ea.Error != null) PublicMethod.RadWaringBox(ea.Error);
                    else
                    {
                        //添加一个套餐名称不能为空的判断
                        //时间：2013年8月13日 11:31:35
                        //修改人：Jhonny
                        if (suit.Name != string.Empty)
                        {
                            if (ea.Result > 0)
                            {
                                GetCP_AdviceSuitCategory(); /****** update by dxj 2011/7/15 ****/
                            }
                            else//修改后的名称为重复名称
                            {
                                PublicMethod.RadAlterBox("该套餐名称已存在！", "提示");
                            }
                        }
                        else
                        {
                            PublicMethod.RadAlterBox("套餐名称不能为空!", "提示");
                        }
                    }
                };
                ServiceClient.EditCP_AdviceSuitAsync(suit);
                ServiceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }



        /// <summary>
        /// 判断是否为双击TextBlock事件
        /// </summary>
        System.Windows.Threading.DispatcherTimer _doubleTextBlockClickTimer = new System.Windows.Threading.DispatcherTimer();
        private void TextBlock_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            _doubleTextBlockClickTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            _doubleTextBlockClickTimer.Tick += new EventHandler(_doubleTextBlockClickTimer_Tick);
            if (_doubleTextBlockClickTimer.IsEnabled)
            {
                if (Ctyzxh == null) { return; }
                RadWindow w = new RadWindow();
                w.Closed += new EventHandler<WindowClosedEventArgs>(EditKeShi_Closed);
                CP_AdviceSuit suit = (CP_AdviceSuit)((RadTreeViewItem)tvList.SelectedItem).Tag; /****** update by dxj 2011/7/15 ****/

                //new PublicMethod().ShowInputWindow(ref  w, "编辑医嘱套餐", "原套餐名称:" + suit.Name, null, null);
                //2013-05-02,WangGuojin.
                new PublicMethod().ShowInputWindow(ref  w, "编辑医嘱套餐", "原套餐名称:" + suit.Name, suit.Name, null);
            }
            _doubleTextBlockClickTimer.Start();
        }

        void _doubleTextBlockClickTimer_Tick(object sender, EventArgs e)
        {
            _doubleTextBlockClickTimer.Stop();
        }

        #region 生成树 add by dxj 2011/7/14

        /// <summary>
        /// 获取医嘱套餐
        /// </summary>
        private void GetCP_AdviceSuitCategory()
        {
            try
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                CP_AdviceSuitCategory AdviceSuitCategory = new CP_AdviceSuitCategory();
                AdviceSuitCategory.CategoryId = String.Empty;
                AdviceSuitCategory.Name = String.Empty;
                AdviceSuitCategory.Memo = String.Empty;
                AdviceSuitCategory.Zgdm = String.Empty;
                String where = String.Empty;
                if (Syfw == "2903")//个人套餐
                {
                    where = String.Format(" and Syfw={0} and Ysdm='{1}'", Syfw, Global.LogInEmployee.Zgdm);
                }
                if (Syfw == "2901")//科室套餐
                {
                    where = String.Format(" and Syfw={0} and Ksdm='{1}'", Syfw, Global.LogInEmployee.Ksdm);
                }
                client.InsertAndSelectCP_AdviceSuitCategoryCompleted += (obj, ea) =>
                {
                    if (ea.Error == null)
                    {
                        if (tvList != null)
                            tvList.Items.Clear();
                        if (AllTreeVewItems == null) AllTreeVewItems = new List<RadTreeViewItem>();
                        AllTreeVewItems.Clear();

                        AddTreeView(String.Empty, null, ea.Result.ToList(), ea.Result.ToList());
                    }
                    //if(CategoryIdList!=null)
                    //for (int i = 0; i < tvList.Items.Count; i++)
                    //{
                    //    //String parentId = ((CP_AdviceSuitCategory)((RadTreeViewItem)selectedItem).Tag).ParentID;
                    //    String currentId = ((CP_AdviceSuitCategory)((RadTreeViewItem)tvList.Items[i]).Tag).CategoryId;
                    //    if (CategoryIdList.Contains(currentId))
                    //    {
                    //        ((RadTreeViewItem)tvList.Items[i]).IsExpanded = true;
                    //    }
                    //    //if (parentId == currentId)
                    //    //{
                    //    //    ((RadTreeViewItem)tvList.Items[i]).IsExpanded = true;
                    //    //}
                    //}
                };
                client.InsertAndSelectCP_AdviceSuitCategoryAsync(AdviceSuitCategory, where);
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 创建树
        /// </summary>
        /// <param name="parentId">父类编号</param>
        /// <param name="subitem">当前树节点</param>
        /// <param name="CategoryList">数据源</param>
        /// <param name="sunCategoryList">符合条件的数据源</param>
        private void AddTreeView(String parentId, RadTreeViewItem subitem, List<CP_AdviceSuitCategory> CategoryList, List<CP_AdviceSuitCategory> sunCategoryList)
        {

            foreach (CP_AdviceSuitCategory row in sunCategoryList.Where(c => c.ParentID.Equals(parentId)))
            {
                RadTreeViewItem item1 = new RadTreeViewItem();
                item1.Header = row.Name;
                item1.Tag = row;
                //循环比较 创建套餐类别根节点或其类别子节点
                if (subitem == null)
                {
                    tvList.Items.Add(item1);

                    item1.DefaultImageSrc = " /Pathway;component/Images/套餐类别.png";
                    if (CategoryIdList.Contains(((CP_AdviceSuitCategory)item1.Tag).CategoryId))
                    {
                        item1.IsExpanded = true;
                    }
                    if (selectedItem != null && ((CP_AdviceSuitCategory)item1.Tag).CategoryId == ((CP_AdviceSuitCategory)selectedItem.Tag).CategoryId)
                    {
                        //item1.IsExpanded = true;
                        item1.IsSelected = true;
                    }
                }
                else
                {
                    subitem.Items.Add(item1);
                    if (CategoryIdList.Contains(((CP_AdviceSuitCategory)item1.Tag).CategoryId))
                    {
                        item1.IsExpanded = true;
                    }
                    if (selectedItem != null && ((CP_AdviceSuitCategory)item1.Tag).CategoryId == ((CP_AdviceSuitCategory)selectedItem.Tag).CategoryId)
                    {
                        //item1.IsExpanded = true;

                        item1.IsSelected = true;
                    }
                    item1.DefaultImageSrc = " /Pathway;component/Images/套餐类别.png";
                }
                //添加所有节点到树控件
                AllTreeVewItems.Add(item1);
                //判断该节点是否有医嘱套餐
                if (row.AdviceSuitList != null)
                {
                    foreach (CP_AdviceSuit suit in row.AdviceSuitList)
                    {
                        RadTreeViewItem item2 = new RadTreeViewItem();
                        item2.Foreground = ConvertColor.GetColorBrushFromHx16("ff0000");
                        item2.Header = suit.Name;
                        item2.Tag = suit;
                        item2.DefaultImageSrc = " /Pathway;component/Images/医嘱套餐.png";//"../Images/tclb.gif";
                        item1.Items.Add(item2);
                    }
                }
                ///递归调用
                AddTreeView(row.CategoryId, item1, CategoryList, CategoryList.Where(c => c.ParentID.Equals(row.CategoryId)).ToList());
            }
        }

        #endregion

    }
}
