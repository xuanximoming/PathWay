using DrectSoft.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Telerik.Windows;
using Telerik.Windows.Controls;
using Telerik.Windows.Controls.GridView;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
namespace YidanEHRApplication.Views.Permission
{
    public partial class EhrtoHisApp : Page
    {
        bool isTrue = true;
        static HisSxpz m_HisSxpz = new HisSxpz();
        /// <summary>
        /// 列表数据源
        /// </summary>
        private ObservableCollection<HisSxpz> m_listsouce;
        #region

        public EhrtoHisApp()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(EhrtoHisApp_Loaded);
        }

        void EhrtoHisApp_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isTrue)
                {
                    isTrue = true;
                    return;
                }
                #region 动态创建界面控件的KeyUp事件 add by luff 20130520
                MadeKeyUp keyUp = new MadeKeyUp();
                keyUp.Controls.Add(txtEhrKey);
                keyUp.Controls.Add(txtEhrKeyms);
                keyUp.Controls.Add(txtHisKey);
                keyUp.Controls.Add(txtHisKeyms);
                keyUp.Controls.Add(txtEhrZdly);

                keyUp.Made_KeyUp();
                txtEhrZdly.KeyUp += new KeyEventHandler(btnSave_KeyUp);
                #endregion
                CurrentState = OperationState.VIEW;
                BindGridView();


            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnSave_KeyUp(object sender, KeyEventArgs e)
        {
            try
            {
                if (e.Key == Key.Enter)
                    btnSave_Click(null, null);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GridView.SelectedItem == null)
                {
                    YiDanMessageBox.Show("请选中要删除的行！", YiDanMessageBoxButtons.Yes);
                    //PublicMethod.RadAlterBox("请选中要删除的行！", "提示");
                    return;
                }
                else
                {

                    YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的配置信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                    mess.ShowDialog();
                    mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
                    //+=  new UCCyXDF.DrugLoaded(CyfOrderControl_AfterDrugLoadedEvent);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        void mess_PageClosedEvent(object sender, bool e)
        {
            try
            {
                if (e == true)
                {

                    int iID = m_HisSxpz.ID;
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.EhrtoHisAppDelCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                BindGridView();
                                //PublicMethod.RadAlterBox("删除成功！", "提示");
                                YiDanMessageBox.Show("删除成功！", GridView);

                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.EhrtoHisAppDelAsync(iID);
                    serviceCon.CloseAsync();

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }



        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (GridView.SelectedItem == null)
                {
                    //PublicMethod.RadAlterBox("请选择一行记录，再点击修改！", "提示");
                    YiDanMessageBox.Show("请选择一行记录，再点击修改！", YiDanMessageBoxButtons.Yes);
                    return;
                }
                m_HisSxpz = (HisSxpz)GridView.SelectedItem;
                CurrentState = OperationState.EDIT;
                this.txtEhrKey.Focus();

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentState = OperationState.NEW;
                GridView.SelectedItem = null;
                NewAdviceGroupDetail();
                this.txtEhrKey.Focus();

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                CurrentState = OperationState.VIEW;
                NewAdviceGroupDetail();
                //this.txtEhrKey.Text = "";
                //this.txtEhrKeyms.Text = "";
                //this.txtHisKey.Text = "";
                //this.txtHisKeyms.Text = "";
                //this.txtEhrZdly.Text = "";

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }



        private void GridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                if (GridView.SelectedItem == null)
                {
                    m_HisSxpz = null;
                    return;
                }
                if (CurrentState == OperationState.VIEW)
                {
                    m_HisSxpz = (HisSxpz)GridView.SelectedItem;
                    Bind_HisSxpz(m_HisSxpz);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 绑定页面中编辑信息
        /// </summary>
        /// <param name="pe_fun">His接口配置实体，如果为空编辑区域值为空</param>
        private void Bind_HisSxpz(HisSxpz _mHisSxpz)
        {
            try
            {
                if (_mHisSxpz != null)
                {
                    this.txtEhrKey.Text = _mHisSxpz.EhrKey;
                    this.txtEhrKeyms.Text = _mHisSxpz.Ehr_Keyms;
                    this.txtHisKey.Text = _mHisSxpz.HisKey;
                    this.txtHisKeyms.Text = _mHisSxpz.His_Keyms;
                    this.txtEhrZdly.Text = _mHisSxpz.EhrSource;

                }
                else
                {
                    m_HisSxpz = new HisSxpz();
                    this.txtEhrKey.Text = "";
                    this.txtEhrKeyms.Text = "";
                    this.txtHisKey.Text = "";
                    this.txtHisKeyms.Text = "";
                    this.txtEhrZdly.Text = "";
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (this.txtEhrKey.Text == "")
                {

                    //PublicMethod.RadAlterBoxRe("请输入EHR接口字段！", "提示", txtEhrKey);
                    YiDanMessageBox.Show("请输入EHR接口字段！", txtEhrKey);
                    isTrue = false;
                    return;
                }
                if (this.txtEhrKeyms.Text == "")
                {

                    //PublicMethod.RadAlterBoxRe("请输入EHR接口字段描述！", "提示", txtEhrKeyms);
                    YiDanMessageBox.Show("请输入EHR接口字段描述！", txtEhrKeyms);
                    isTrue = false;
                    return;
                }

                //if (this.txtHisKey.Text == "")
                //{

                //    PublicMethod.RadAlterBoxRe("请输入对应His字段！", "提示", txtHisKey);
                //    isTrue = false;
                //    return;
                //}
                //if (this.txtHisKeyms.Text == "")
                //{

                //    PublicMethod.RadAlterBoxRe("请输入对应His字段描述！", "提示", txtHisKeyms);
                //    isTrue = false;
                //    return;
                //}

                ObservableCollection<HisSxpz> _Hissxpz = new ObservableCollection<HisSxpz>();

                HisSxpz _mHissxpz = new HisSxpz();
                _mHissxpz.EhrKey = this.txtEhrKey.Text.Trim();
                _mHissxpz.Ehr_Keyms = this.txtEhrKeyms.Text.Trim();
                _mHissxpz.HisKey = this.txtHisKey.Text.Trim();
                _mHissxpz.His_Keyms = this.txtHisKeyms.Text.Trim();
                _mHissxpz.EhrSource = this.txtEhrZdly.Text.Trim();
                _mHissxpz.ID = 0;
                //_Hissxpz.Add(_mHissxpz);

                //int iID = 1;
                //iID = ((HisSxpz)this.GridView.SelectedItem).ID;
                if (CurrentState == OperationState.NEW)
                {
                    //清楚控件

                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.EhrtoHisAppInsertCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                BindGridView();
                                //PublicMethod.RadAlterBox("保存成功！", "提示");
                                YiDanMessageBox.Show("保存成功！", YiDanMessageBoxButtons.Yes);
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.EhrtoHisAppInsertAsync(_mHissxpz);
                    serviceCon.CloseAsync();

                    CurrentState = OperationState.VIEW;
                }
                if (CurrentState == OperationState.EDIT)
                {

                    //HisSxpz _mHissxpz = new HisSxpz();
                    //_mHissxpz.EhrKey = ((HisSxpz)this.GridView.SelectedItem).EhrKey;
                    //_mHissxpz.Ehr_Keyms = ((HisSxpz)this.GridView.SelectedItem).Ehr_Keyms;
                    //_mHissxpz.HisKey = ((HisSxpz)this.GridView.SelectedItem).HisKey;
                    //_mHissxpz.His_Keyms = ((HisSxpz)this.GridView.SelectedItem).His_Keyms;
                    //_mHissxpz.EhrSource = ((HisSxpz)this.GridView.SelectedItem).EhrSource;
                    //_mHissxpz.ID = ((HisSxpz)this.GridView.SelectedItem).ID;
                    //_Hissxpz.Add(_mHissxpz);
                    _mHissxpz.ID = m_HisSxpz.ID; //((HisSxpz)this.GridView.SelectedItem).ID;
                    YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                    Client.EhrToHisAppUpdateCompleted += (sb, ea) =>
                    {

                        if (ea.Error == null)
                        {
                            BindGridView();
                            YiDanMessageBox.Show("更新成功！", YiDanMessageBoxButtons.Yes);
                            //PublicMethod.RadAlterBox("更新成功！", "提示");
                        }
                    };
                    Client.EhrToHisAppUpdateAsync(_mHissxpz);
                    serviceCon.CloseAsync();
                    //清楚控件
                    NewAdviceGroupDetail();
                    CurrentState = OperationState.VIEW;
                }



            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnTxtClear_Click(object sender, RoutedEventArgs e)
        {
            try
            {
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region 属性变量
        /// <summary>
        /// 用于存储列表中选中的行记录
        /// </summary>


        YidanEHRDataServiceClient serviceCon;
        OperationState _CurrentState = OperationState.VIEW;
        OperationState CurrentState
        {
            get { return _CurrentState; }
            set
            {
                _CurrentState = value;

                if (value == OperationState.NEW)
                {
                    this.txtEhrKey.IsEnabled = true;
                    txtEhrKeyms.IsEnabled = true;
                    txtHisKey.IsEnabled = true;
                    txtHisKeyms.IsEnabled = true;
                    this.txtEhrZdly.IsEnabled = true;

                    this.btnAdd.IsEnabled = false;
                    this.btnDel.IsEnabled = false;
                    this.btnUpdate.IsEnabled = false;

                    this.btnClear.IsEnabled = true;

                    this.btnSave.IsEnabled = true;


                }
                else if (value == OperationState.EDIT)
                {
                    this.txtEhrKey.IsEnabled = false;
                    txtEhrKeyms.IsEnabled = false;
                    txtHisKey.IsEnabled = true;
                    txtHisKeyms.IsEnabled = true;
                    this.txtEhrZdly.IsEnabled = true;
                    this.btnAdd.IsEnabled = false;
                    this.btnDel.IsEnabled = false;
                    this.btnUpdate.IsEnabled = false;

                    this.btnClear.IsEnabled = true;

                    this.btnSave.IsEnabled = true;

                }
                else
                {

                    this.txtEhrKey.IsEnabled = false;
                    txtEhrKeyms.IsEnabled = false;
                    txtHisKey.IsEnabled = false;
                    txtHisKeyms.IsEnabled = false;
                    this.txtEhrZdly.IsEnabled = false;
                    this.btnAdd.IsEnabled = true;

                    this.btnDel.IsEnabled = true;
                    this.btnUpdate.IsEnabled = true;

                    this.btnClear.IsEnabled = false;

                    this.btnSave.IsEnabled = false;
                }


            }
        }
        static List<HisSxpz> HisSxpz = new List<HisSxpz>();
        #endregion


        #region 函数


        private void BindGridView()
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetEhrToHisAppCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            GridView.ItemsSource = e.Result;
                            //初始化查询数据源
                            m_listsouce = (ObservableCollection<HisSxpz>)GridView.ItemsSource;
                            HisSxpz = e.Result.ToList();

                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
            serviceCon.GetEhrToHisAppAsync(1);
            serviceCon.CloseAsync();
        }
        #endregion

        #region 右键菜单枚举
        /// <summary>
        /// 页面状态
        /// </summary>
        enum PageState
        {
            New = 0,
            Edit = 1
        }
        /// <summary>
        /// 右键菜单枚举
        /// </summary>
        private enum TagName
        {
            New,
            Edit,
            Del

        }
        /// <summary>
        /// 添加List 到 ObservableCollection
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="coll"></param>
        /// <returns></returns>
        public static ObservableCollection<T> ToObservableCollection<T>(IEnumerable<T> coll)
        {
            var c = new ObservableCollection<T>();
            foreach (var e in coll)
                c.Add(e);
            return c;
        }
        #endregion
        #region 常量初始及构造函数
        const string HeaderText = "配置His接口提示"; //定义弹出框标题栏
        #endregion

        #region GridView事件
        private void GridView_RowLoaded(object sender, RowLoadedEventArgs e)
        {
            try
            {
                RadContextMenu rowContextMenu = new RadContextMenu(); //新建一个右键菜单

                #region 右键菜单
                if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
                rowContextMenu.Width = 200;
                // rowContextMenu.Items.Add(new RadMenuItem() { Header = "新增配置", Tag = TagName.New });
                rowContextMenu.Items.Add(new RadMenuItem() { Header = "更新配置", Tag = TagName.Edit });
                rowContextMenu.Items.Add(new RadMenuItem() { IsSeparator = true });
                //rowContextMenu.Items.Add(new RadMenuItem() { Header = "删除配置", Tag = TagName.Del });
                rowContextMenu.AddHandler(RadMenuItem.ClickEvent, new RoutedEventHandler(OnMenuItemClick));
                rowContextMenu.Opened += new RoutedEventHandler(OnMenuOpened);
                RadContextMenu.SetContextMenu(e.Row, rowContextMenu);
                #endregion


            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region 其他右键操作方法


        void mess_PageClosedEvent2(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    ObservableCollection<HisSxpz> _Hissxpz = new ObservableCollection<HisSxpz>();

                    HisSxpz _mHissxpz = new HisSxpz();
                    _mHissxpz.EhrKey = ((HisSxpz)this.GridView.SelectedItem).EhrKey;
                    _mHissxpz.Ehr_Keyms = ((HisSxpz)this.GridView.SelectedItem).Ehr_Keyms;
                    _mHissxpz.HisKey = ((HisSxpz)this.GridView.SelectedItem).HisKey;
                    _mHissxpz.His_Keyms = ((HisSxpz)this.GridView.SelectedItem).His_Keyms;
                    _mHissxpz.EhrSource = ((HisSxpz)this.GridView.SelectedItem).EhrSource;
                    _mHissxpz.ID = ((HisSxpz)this.GridView.SelectedItem).ID;
                    _Hissxpz.Add(_mHissxpz);


                    YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                    Client.EhrToHisAppUpdateCompleted += (sb, eb) =>
                    {

                        BindGridView();
                        //PublicMethod.RadAlterBox("保存成功！", "提示");
                        YiDanMessageBox.Show("保存成功！", YiDanMessageBoxButtons.Yes);

                    };
                    Client.EhrToHisAppUpdateAsync(_mHissxpz);
                    serviceCon.CloseAsync();

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void OnEditHisPz(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                //Save();
                try
                {
                    ObservableCollection<HisSxpz> _Hissxpz = new ObservableCollection<HisSxpz>();

                    HisSxpz _mHissxpz = new HisSxpz();
                    _mHissxpz.EhrKey = ((HisSxpz)this.GridView.SelectedItem).EhrKey;
                    _mHissxpz.Ehr_Keyms = ((HisSxpz)this.GridView.SelectedItem).Ehr_Keyms;
                    _mHissxpz.HisKey = ((HisSxpz)this.GridView.SelectedItem).HisKey;
                    _mHissxpz.His_Keyms = ((HisSxpz)this.GridView.SelectedItem).His_Keyms;
                    _mHissxpz.EhrSource = ((HisSxpz)this.GridView.SelectedItem).EhrSource;
                    _mHissxpz.ID = ((HisSxpz)this.GridView.SelectedItem).ID;
                    _Hissxpz.Add(_mHissxpz);


                    YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                    Client.EhrToHisAppUpdateCompleted += (sb, eb) =>
                    {

                        BindGridView();
                        //PublicMethod.RadAlterBox("保存成功！", "提示");
                        YiDanMessageBox.Show("保存成功！", YiDanMessageBoxButtons.Yes);

                    };
                    Client.EhrToHisAppUpdateAsync(_mHissxpz);
                    serviceCon.CloseAsync();
                }

                catch (Exception ex)
                {
                    YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            }
        }
        /// <summary>
        /// 清空控件
        /// </summary>
        void NewAdviceGroupDetail()
        {
            this.txtEhrKey.Text = "";
            this.txtEhrKeyms.Text = "";
            this.txtHisKey.Text = "";
            this.txtHisKeyms.Text = "";
            this.txtEhrZdly.Text = "";
        }

        void mess_PageClosedEvent1(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    List<decimal> listid = new List<decimal>(); //用于保存删除的数据行主键
                    for (int i = 0; i < this.GridView.SelectedItems.Count; i++)
                    {
                        listid.Add(((HisSxpz)this.GridView.SelectedItems[i]).ID);//取出要删除数据行的主键。
                    }
                    YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                    ServiceClient.DelAdviceGroupDetailCompleted +=
                         (obj, ea) =>
                         {
                             if (ea.Error == null)
                             {
                                 //BindGridData(((RadPanelBarItem)this.OrderPanelBar.SelectedItem).Tag.ToString(), nodestr.ToString().Split('&')[0].ToString());
                                 PublicMethod.RadAlterBox(ea.Result, HeaderText);
                             }
                             else
                             {
                                 PublicMethod.RadWaringBox(ea.Error);
                             }
                         };
                    ServiceClient.DelAdviceGroupDetailAsync(ToObservableCollection(listid)); // DelAdviceGroupDetailAsync 函数需要ObservableCollection 类型参数

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 删除行数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnDelHisPz(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                List<decimal> listid = new List<decimal>(); //用于保存删除的数据行主键
                for (int i = 0; i < this.GridView.SelectedItems.Count; i++)
                {
                    listid.Add(((HisSxpz)this.GridView.SelectedItems[i]).ID);//取出要删除数据行的主键。
                }
                YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                ServiceClient.DelAdviceGroupDetailCompleted +=
                     (obj, ea) =>
                     {
                         if (ea.Error == null)
                         {
                             //BindGridData(((RadPanelBarItem)this.OrderPanelBar.SelectedItem).Tag.ToString(), nodestr.ToString().Split('&')[0].ToString());
                             PublicMethod.RadAlterBox(ea.Result, HeaderText);
                         }
                         else
                         {
                             PublicMethod.RadWaringBox(ea.Error);
                         }
                     };
                ServiceClient.DelAdviceGroupDetailAsync(ToObservableCollection(listid)); // DelAdviceGroupDetailAsync 函数需要ObservableCollection 类型参数
            }
            else
            {
            }
        }
        #endregion
        /// <summary>
        /// 右键打开
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMenuOpened(object sender, RoutedEventArgs e)
        {
            try
            {
                DialogParameters parameters = new DialogParameters();
                if (false)
                {
                    RadContextMenu menu = sender as RadContextMenu;
                    menu.IsOpen = false;
                    //parameters.Content = String.Format("{0}", "是否保存当前更改的数据？");
                    //parameters.Header = HeaderText;
                    //parameters.IconContent = null;
                    //parameters.OkButtonContent = "确定";
                    //parameters.CancelButtonContent = "取消";
                    //parameters.Closed = OnEditHisPz;//***close处理***
                    //RadWindow.Confirm(parameters);
                    YidanPathWayMessageBox mess = new YidanPathWayMessageBox("是否保存当前更改的数据？", "提示", YiDanMessageBoxButtons.YesNo);
                    mess.ShowDialog();
                    mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent2);
                }
                else
                {
                    GridViewRow row = ((RadRoutedEventArgs)e).OriginalSource as GridViewRow;
                    List<HisSxpz> listsOrder = new List<HisSxpz>();
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
                                if ((TagName)item.Tag == TagName.Edit)
                                {
                                    item.IsEnabled = !(this.GridView.SelectedItems.Count > 1);
                                }

                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 右键项目事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void OnMenuItemClick(object sender, RoutedEventArgs e)
        {
            try
            {
                RadMenuItem clickedItem = ((RadRoutedEventArgs)e).OriginalSource as RadMenuItem;
                if (clickedItem != null)
                {
                    TagName tagName = (TagName)clickedItem.Tag;
                    object selectedItem = GridView.SelectedItem;
                    DialogParameters parameters = new DialogParameters();
                    switch (tagName)
                    {
                        case TagName.New:
                            NewAdviceGroupDetail();//清空Form
                            break;
                        case TagName.Edit:
                            CurrentState = OperationState.EDIT;
                            m_HisSxpz = this.GridView.SelectedItem as HisSxpz;
                            #region 绑定Form
                            this.txtEhrKey.Text = m_HisSxpz.EhrKey;
                            this.txtEhrKeyms.Text = m_HisSxpz.Ehr_Keyms;
                            this.txtHisKey.Text = m_HisSxpz.HisKey;
                            this.txtHisKeyms.Text = m_HisSxpz.His_Keyms;
                            this.txtEhrZdly.Text = m_HisSxpz.EhrSource;
                            #endregion
                            break;
                        case TagName.Del:

                            //parameters.Content = String.Format("提示: {0}", "确认删除吗？");
                            //parameters.Header = HeaderText;
                            //parameters.IconContent = null;
                            //parameters.OkButtonContent = "确认";
                            //parameters.CancelButtonContent = "取消";
                            //parameters.Closed = OnDelHisPz;//***close处理***
                            //RadWindow.Confirm(parameters);
                            YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                            mess.ShowDialog();
                            mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent1);
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

        #region 查询关键字相关事件方法 add by luff 2013-01-31
        /// <summary>
        /// 查询事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            string para = this.txt_Ehrzd.Text;
            string para1 = this.txt_Ehrzdms.Text;
            string para2 = this.txt_Ehrzdly.Text;
            //集合类型初始化
            List<HisSxpz> t_listsouce = m_listsouce.ToList();


            if (para.Trim().Length > 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.EhrKey.IndexOf(para) > -1).ToList();

            }

            if (para1.Trim().Length > 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.Ehr_Keyms.IndexOf(para1) > -1).ToList();

            }


            if (para2.Trim().Length > 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.EhrSource.IndexOf(para2) > -1).ToList();

            }


            GridView.ItemsSource = t_listsouce.ToList();

            //if(para.Trim().Length==0 && para1.Trim().Length==0 && para2.Trim().Length==0)
            //{
            //    GridView.ItemsSource = m_listsouce;
            //}
        }


        //查询重置
        private void btnReset_Click(object sender, RoutedEventArgs e)
        {

            this.txt_Ehrzd.Text = string.Empty;
            this.txt_Ehrzdms.Text = string.Empty;
            this.txt_Ehrzdly.Text = string.Empty;
        }
        #endregion

    }
}
