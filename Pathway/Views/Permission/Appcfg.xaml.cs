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
    public partial class Appcfg : Page
    {

        bool isTrue = true;
        static APPCFG m_AppCfg = new APPCFG();
        /// <summary>
        /// 列表数据源
        /// </summary>
        private ObservableCollection<APPCFG> m_listsouce;

        public Appcfg()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(Appcfg_Loaded);
        }

        void Appcfg_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isTrue)
                {
                    isTrue = true;
                    return;
                }

                #region 动态创建界面控件的KeyUp事件 add by luff 20130531
                MadeKeyUp keyUp = new MadeKeyUp();
                keyUp.Controls.Add(txtAppgjc);
                keyUp.Controls.Add(txtAppname);
                keyUp.Controls.Add(txtAppshzt);
                keyUp.Controls.Add(txtAppType);
                keyUp.Controls.Add(txtAppVal);
                keyUp.Controls.Add(txtAppcsms);
                keyUp.Made_KeyUp();
                txtAppcsms.KeyUp += new KeyEventHandler(btnSave_KeyUp);
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
                if (m_AppCfg == null || m_AppCfg.Configkey == null)
                {
                    YiDanMessageBox.Show("请选中要删除的行！", YiDanMessageBoxButtons.Yes);
                    //PublicMethod.RadAlterBox("请选中要删除的行！", "提示");
                }
                else
                {
                    //DialogParameters parameters = new DialogParameters();
                    //parameters.Content = String.Format("提示: {0}", "请问是否删除选中的配置信息吗？");
                    //parameters.Header = "提示";
                    //parameters.IconContent = null;
                    //parameters.OkButtonContent = "确定";
                    //parameters.CancelButtonContent = "取消";
                    //parameters.Closed = OnDelAPPCFG;//***close处理***
                    //RadWindow.Confirm(parameters);
                    YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的配置信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                    mess.ShowDialog();
                    mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);


                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        void OnDelAPPCFG(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                try
                {
                    string sID = m_AppCfg.Configkey;
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.APPCFGHidDelCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                BindGridView();
                                PublicMethod.RadAlterBox("删除成功！", "提示");

                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.APPCFGHidDelAsync(sID);
                    serviceCon.CloseAsync();
                }
                catch (Exception ex)
                {
                    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            }
        }

        void mess_PageClosedEvent(object sender, bool e)
        {
            try
            {
                if (e == true)
                {

                    string sID = m_AppCfg.Configkey;
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.APPCFGHidDelCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                BindGridView();
                                //PublicMethod.RadAlterBox("删除成功！", "提示");
                                YiDanMessageBox.Show("删除成功！", YiDanMessageBoxButtons.Ok);

                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.APPCFGHidDelAsync(sID);
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
                    YiDanMessageBox.Show("请选择一行记录，再点击修改！", YiDanMessageBoxButtons.Ok);
                    return;
                }
                m_AppCfg = (APPCFG)GridView.SelectedItem;
                CurrentState = OperationState.EDIT;
                this.txtAppgjc.Focus();

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
                this.txtAppgjc.Focus();

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
                //this.txtAppgjc.Text = "";
                //this.txtAppname.Text = "";
                //this.txtAppVal.Text = "";
                //this.txtAppType.Text = "";
                //this.txtAppcsms.Text = "";

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
                    m_AppCfg = null;
                    return;
                }
                if (CurrentState == OperationState.VIEW)
                {
                    m_AppCfg = (APPCFG)GridView.SelectedItem;
                    Bind_APPCFG(m_AppCfg);
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
        private void Bind_APPCFG(APPCFG _mAppCfg)
        {
            if (_mAppCfg != null)
            {
                this.txtAppgjc.Text = _mAppCfg.Configkey;
                this.txtAppname.Text = _mAppCfg.Name;
                this.txtAppVal.Text = _mAppCfg.Value;
                this.txtAppType.Text = _mAppCfg.ParamType.ToString();
                this.txtAppcsms.Text = _mAppCfg.Descript;

            }
            else
            {
                m_AppCfg = new APPCFG();
                this.txtAppgjc.Text = "";
                this.txtAppname.Text = "";
                this.txtAppVal.Text = "";
                this.txtAppType.Text = "";
                this.txtAppcsms.Text = "";
            }
        }
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {

            try
            {

                if (this.txtAppgjc.Text == "")
                {

                    //PublicMethod.RadAlterBoxRe("请输入参数关键词！", "提示", txtAppgjc);
                    YiDanMessageBox.Show("请输入参数关键词！", YiDanMessageBoxButtons.Ok);
                    isTrue = false;
                    return;
                }
                if (this.txtAppname.Text == "")
                {

                    //PublicMethod.RadAlterBoxRe("请输入配置参数名称！", "提示", txtAppname);
                    YiDanMessageBox.Show("请输入配置参数名称！", YiDanMessageBoxButtons.Ok);
                    isTrue = false;
                    return;
                }

                if (this.txtAppVal.Text == "")
                {

                    //PublicMethod.RadAlterBoxRe("请输入配置参数值！", "提示", txtAppVal);
                    YiDanMessageBox.Show("请输入配置参数值！", YiDanMessageBoxButtons.Ok);
                    isTrue = false;
                    return;
                }
                //if (this.txtAppType.Text == "")
                //{

                //    PublicMethod.RadAlterBoxRe("请输入配置参数类型！", "提示", txtAppType);
                //    isTrue = false;
                //    return;
                //}

                ObservableCollection<APPCFG> _APPCFG = new ObservableCollection<APPCFG>();

                APPCFG _mAppCfg = new APPCFG();
                _mAppCfg.Configkey = this.txtAppgjc.Text.Trim();
                _mAppCfg.Name = this.txtAppname.Text.Trim();
                _mAppCfg.Value = this.txtAppVal.Text.Trim();
                _mAppCfg.ParamType = this.txtAppType.Text.Trim() == "" ? 1 : int.Parse(this.txtAppType.Text.Trim());
                _mAppCfg.Descript = this.txtAppcsms.Text.Trim();
                _mAppCfg.Cfgkeyset = "";
                _mAppCfg.Design = "";
                _mAppCfg.ClientFlag = 0;
                _mAppCfg.Hide = 1;//默认为1，0表示隐藏也表示假删除，1表示正常显示
                _mAppCfg.Valid = this.txtAppshzt.Text.Trim() == "" ? 1 : int.Parse(this.txtAppshzt.Text.Trim());//默认状态为1。1为审核通过，0为审核不通过

                if (CurrentState == OperationState.NEW)
                {
                    //清楚控件

                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.AppCfgInsertCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                BindGridView();
                                //PublicMethod.RadAlterBox("保存成功！", "提示");
                                YiDanMessageBox.Show("保存成功！", YiDanMessageBoxButtons.Ok);
                                //清空控件
                                NewAdviceGroupDetail();
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.AppCfgInsertAsync(_mAppCfg);
                    serviceCon.CloseAsync();

                    CurrentState = OperationState.VIEW;
                }
                if (CurrentState == OperationState.EDIT)
                {

                    _mAppCfg.Configkey = m_AppCfg.Configkey;
                    //_mAppCfg.Hide = m_AppCfg.Hide;
                    //_mAppCfg.Configkey = ((APPCFG)this.GridView.SelectedItem).Configkey;
                    //_mAppCfg.Hide = ((APPCFG)this.GridView.SelectedItem).Hide;
                    YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                    Client.APPCFGUpdateCompleted += (sb, ea) =>
                    {

                        if (ea.Error == null)
                        {
                            BindGridView();
                            //PublicMethod.RadAlterBox("更新成功！", "提示");
                            YiDanMessageBox.Show("更新成功！", YiDanMessageBoxButtons.Ok);
                            NewAdviceGroupDetail();
                        }
                    };
                    Client.APPCFGUpdateAsync(_mAppCfg);
                    serviceCon.CloseAsync();
                    //清空控件
                    //NewAdviceGroupDetail();
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
                    this.txtAppgjc.IsEnabled = true;
                    txtAppname.IsEnabled = true;
                    txtAppVal.IsEnabled = true;
                    txtAppType.IsEnabled = true;
                    this.txtAppcsms.IsEnabled = true;
                    txtAppshzt.IsEnabled = true;
                    this.btnAdd.IsEnabled = false;
                    this.btnDel.IsEnabled = false;
                    this.btnUpdate.IsEnabled = false;

                    this.btnClear.IsEnabled = true;

                    this.btnSave.IsEnabled = true;


                }
                else if (value == OperationState.EDIT)
                {
                    this.txtAppgjc.IsEnabled = false;
                    txtAppname.IsEnabled = true;
                    txtAppVal.IsEnabled = true;
                    txtAppType.IsEnabled = true;
                    this.txtAppcsms.IsEnabled = true;
                    this.btnAdd.IsEnabled = false;
                    this.btnDel.IsEnabled = false;
                    this.btnUpdate.IsEnabled = false;
                    txtAppshzt.IsEnabled = true;
                    this.btnClear.IsEnabled = true;

                    this.btnSave.IsEnabled = true;

                }
                else
                {

                    this.txtAppgjc.IsEnabled = false;
                    txtAppname.IsEnabled = false;
                    txtAppVal.IsEnabled = false;
                    txtAppType.IsEnabled = false;
                    this.txtAppcsms.IsEnabled = false;
                    this.btnAdd.IsEnabled = true;
                    txtAppshzt.IsEnabled = false;
                    this.btnDel.IsEnabled = true;
                    this.btnUpdate.IsEnabled = true;

                    this.btnClear.IsEnabled = false;

                    this.btnSave.IsEnabled = false;
                }


            }
        }
        static List<APPCFG> APPCFG = new List<APPCFG>();
        #endregion


        #region 函数


        private void BindGridView()
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetAppCfgCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            GridView.ItemsSource = e.Result;
                            //初始化查询数据源
                            m_listsouce = (ObservableCollection<APPCFG>)GridView.ItemsSource;
                            APPCFG = e.Result.ToList();

                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
            serviceCon.GetAppCfgAsync("-1");
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
        const string HeaderText = "配置参数提示"; //定义弹出框标题栏
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
        void OnAppCfgPz(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                //Save();
                try
                {
                    ObservableCollection<APPCFG> _APPCFG = new ObservableCollection<APPCFG>();

                    APPCFG _mAppCfg = new APPCFG();
                    _mAppCfg.Configkey = ((APPCFG)this.GridView.SelectedItem).Configkey;
                    _mAppCfg.Name = ((APPCFG)this.GridView.SelectedItem).Name;
                    _mAppCfg.Value = ((APPCFG)this.GridView.SelectedItem).Value;
                    _mAppCfg.ParamType = ((APPCFG)this.GridView.SelectedItem).ParamType;
                    _mAppCfg.Descript = ((APPCFG)this.GridView.SelectedItem).Descript;
                    _mAppCfg.Design = "";
                    _mAppCfg.Cfgkeyset = "";
                    _mAppCfg.ClientFlag = 0;//默认值为0
                    _mAppCfg.Hide = 1;//默认值为1
                    _mAppCfg.Valid = ((APPCFG)this.GridView.SelectedItem).Valid;//默认值为1
                    _APPCFG.Add(_mAppCfg);


                    YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                    Client.APPCFGUpdateCompleted += (sb, eb) =>
                    {

                        BindGridView();
                        PublicMethod.RadAlterBox("保存成功！", "提示");

                    };
                    Client.APPCFGUpdateAsync(_mAppCfg);
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
            this.txtAppgjc.Text = "";
            this.txtAppname.Text = "";
            this.txtAppVal.Text = "";
            this.txtAppType.Text = "";
            this.txtAppcsms.Text = "";
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
                //for (int i = 0; i < this.GridView.SelectedItems.Count; i++)
                //{
                //    listid.Add(((APPCFG)this.GridView.SelectedItems[i]).Configkey);//取出要删除数据行的主键。
                //}
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
                    parameters.Content = String.Format("{0}", "是否保存当前更改的数据？");
                    parameters.Header = HeaderText;
                    parameters.IconContent = null;
                    parameters.OkButtonContent = "确定";
                    parameters.CancelButtonContent = "取消";
                    parameters.Closed = OnAppCfgPz;//***close处理***
                    RadWindow.Confirm(parameters);
                }
                else
                {
                    GridViewRow row = ((RadRoutedEventArgs)e).OriginalSource as GridViewRow;
                    List<APPCFG> listsOrder = new List<APPCFG>();
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
                            m_AppCfg = this.GridView.SelectedItem as APPCFG;
                            #region 绑定Form
                            this.txtAppgjc.Text = m_AppCfg.Configkey;
                            this.txtAppname.Text = m_AppCfg.Name;
                            this.txtAppVal.Text = m_AppCfg.Value;
                            this.txtAppType.Text = m_AppCfg.ParamType.ToString();
                            this.txtAppcsms.Text = m_AppCfg.Descript;
                            this.txtAppshzt.Text = m_AppCfg.Valid.ToString();
                            #endregion
                            break;
                        case TagName.Del:

                            parameters.Content = String.Format("提示: {0}", "确认删除吗？");
                            parameters.Header = HeaderText;
                            parameters.IconContent = null;
                            parameters.OkButtonContent = "确认";
                            parameters.CancelButtonContent = "取消";
                            parameters.Closed = OnDelHisPz;//***close处理***
                            RadWindow.Confirm(parameters);
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

        #region 查询关键字相关事件方法 add by luff 2013-02-22
        /// <summary>
        /// 查询事件 
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            string para = this.txt_Appgjc.Text;
            string para1 = this.txt_Appname.Text;
            string para2 = this.txt_Appcsms.Text;
            //集合类型初始化
            List<APPCFG> t_listsouce = m_listsouce.ToList();


            if (para.Trim().Length > 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.Configkey.IndexOf(para) > -1).ToList();

            }

            if (para1.Trim().Length > 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.Name.IndexOf(para1) > -1).ToList();

            }


            if (para2.Trim().Length > 0)
            {
                t_listsouce = t_listsouce.Select(s => s).Where(s => s.Descript.IndexOf(para2) > -1).ToList();

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

            this.txt_Appgjc.Text = string.Empty;
            this.txt_Appcsms.Text = string.Empty;
            this.txt_Appname.Text = string.Empty;
        }
        #endregion

    }
}
