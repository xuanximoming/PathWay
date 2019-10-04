using DrectSoft.Tool;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Markup;
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using YidanEHRApplication.ViewsViews.Tool;
namespace YidanEHRApplication.Views
{
    public partial class PathVariationDetail : Page
    {
        #region 属性
        /// <summary>
        /// GRIDVIEW里所有行中的checkBox,用tag属性区别（绑定ORDERGUID)
        /// </summary>
        public static ObservableCollection<CheckBox> m_GridCheckBox = new ObservableCollection<CheckBox>();
        private List<CP_PathVariation> m_PathVariation = null;
        int operationType; //编码操作类型：2创建操作、1启用编码、0停用编码
        #endregion

        #region 事件

        public bool isLoad = true;

        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoad)
            {
                isLoad = true;
                return;
            }
            BindcbxVariationType();
            GridViewPathVariation.SelectedItem = null;
        }
        private void btnCreateCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //if ( ddbBydm.Tag.ToString().Trim()=="")
                //{
                //    PublicMethod.RadAlterBox("编码分类必选", "提示");
                //    return;
                //}
                if (txtBydm.Text.Trim() == "" || txtBymc.Text.Trim() == "" || txtByms.Text.Trim() == "")
                {
                    PublicMethod.RadAlterBoxRe("变异编码、变异名称和变异描述不能为空！", "提示", txtBydm); isLoad = false;
                    return;
                }
                if (TreeView.SelectedItem == null)
                {
                    if (txtBydm.Text.Trim().Length >= 4)
                    {
                        PublicMethod.RadAlterBoxRe("一级变异编码长度超出！", "提示", txtBydm); isLoad = false; return;
                    }
                    if (!char.IsLetter(txtBydm.Text.Trim(), 0))
                    {
                        PublicMethod.RadAlterBoxRe("一级变异编码第一位必须为字母！", "提示", txtBydm); isLoad = false; return;
                    }

                    try
                    {
                        int.Parse(txtBydm.Text.Trim().Substring(1, txtBydm.Text.Trim().Length - 1));
                    }
                    catch
                    {
                        PublicMethod.RadAlterBoxRe("一级变异编码从第二位开始必须为正整数！", "提示", txtBydm); isLoad = false; return;
                    }
                }
                foreach (CP_PathVariation item in GridViewPathVariation.Items)
                {
                    if (item.Bydm == txtBydm.Text.Trim())
                    {
                        PublicMethod.RadAlterBoxRe("该变异编码已被使用，请重新输入！", "提示", txtBydm); isLoad = false; return;
                    }
                    if (item.Bymc == txtBymc.Text.Trim())
                    {
                        PublicMethod.RadAlterBoxRe("该变异名称已被使用，请重新输入！", "提示", txtBymc); isLoad = false; return;
                    }
                    if (item.Byms == txtByms.Text.Trim())
                    {
                        PublicMethod.RadAlterBoxRe("该变异描述已被使用，请重新输入！", "提示", txtByms); isLoad = false; return;
                    }
                }
                operationType = 2;
                //DialogParameters parameters = new DialogParameters();
                //parameters.Content = String.Format("提示: {0}", "确认创建变异编码吗？");
                //parameters.Header = "提示";
                //parameters.IconContent = null;
                //parameters.OkButtonContent = "确定";
                //parameters.CancelButtonContent = "取消";
                //parameters.Closed = AddCP_PathVariationClose;
                //RadWindow.Confirm(parameters);
                YidanPathWayMessageBox mess = new YidanPathWayMessageBox("确认创建变异编码吗？", "提示", YiDanMessageBoxButtons.YesNo);
                mess.ShowDialog();
                mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnQueryCode_Click(object sender, RoutedEventArgs e)
        {
            BindGridView();
        }
        private void btnEnableCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                operationType = (((System.Windows.Controls.Button)sender).Name.Equals("btnEnableCode")) ? 1 : 0;//编码状态;
                DialogParameters parameters = new DialogParameters();
                string state = ((CP_PathVariation)GridViewPathVariation.CurrentItem).State.ToString();
                if (state == "启用" && operationType == 1)
                {
                    PublicMethod.RadAlterBox("状态已经为启用！", "提示");
                    return;
                }
                if (state == "停用" && operationType == 0)
                {
                    PublicMethod.RadAlterBox("状态已经为停用！", "提示");
                    return;
                }
                //parameters.Content = String.Format("提示: {0}", "确认" + (operationType == 1 ? "启用选中的变异编码" : "停用选中的变异编码") + "吗？");
                //parameters.Header = "提示";
                //parameters.IconContent = null;
                //parameters.OkButtonContent = "确定";
                //parameters.CancelButtonContent = "取消";
                //parameters.Closed = EnableCP_PathVariationClose;
                //RadWindow.Confirm(parameters);
                YidanPathWayMessageBox mess = new YidanPathWayMessageBox("确认" + (operationType == 1 ? "启用选中的变异编码" : "停用选中的变异编码") + "吗？", "提示", YiDanMessageBoxButtons.YesNo);
                mess.ShowDialog();
                mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void checkBoxAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.GridViewPathVariation.ItemsSource == null)
                    return;
                foreach (CheckBox check in m_GridCheckBox)
                {
                    if (check.IsEnabled == true)
                        check.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void checkBoxAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.GridViewPathVariation.ItemsSource == null)
                return;
            foreach (CheckBox check in m_GridCheckBox)
            {
                if (check.IsEnabled == true)
                    check.IsChecked = false;
            }
        }
        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {

                m_GridCheckBox.Add(sender as CheckBox);
                string chktag = ((CheckBox)sender).Tag.ToString();
                ((CheckBox)sender).IsEnabled = chktag.Length == 11 ? true : false;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void TreeView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (TreeView.SelectedItem == null) return;

            YidanEHRDataServiceClient QueryClient = PublicMethod.YidanClient;
            CP_PathVariationClients cP_PathVariations = new CP_PathVariationClients();
            QueryClient.GetDataPathVariationListAllCompleted +=
                (obj, ea) =>
                {
                    if (ea.Error == null)
                    {
                        cP_PathVariations.AddRange(ea.Result.ToList());
                        if (TreeView.SelectedItem == null)
                            txtBydm.IsReadOnly = false;
                        else
                            txtBydm.IsReadOnly = true;
                        txtBydm.Text = cP_PathVariations.GenerateNewChild(((CP_PathVariationClient)TreeView.SelectedItem).Bydm);
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(ea.Error);
                    }
                };
            QueryClient.GetDataPathVariationListAllAsync();
            QueryClient.CloseAsync();

        }
        private void btnClassCode_Click(object sender, RoutedEventArgs e)
        {
            RWPathVariationClassCode CWin = new RWPathVariationClassCode();
            //CWin.Show();
            CWin.ResizeMode = ResizeMode.NoResize;
            CWin.ShowDialog();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        void mess_PageClosedEvent(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    //try
                    //{
                    if (operationType == 2) //创建变异编码
                    {
                        //foreach (CheckBox checkbox in m_GridCheckBox)
                        //{
                        //    if (txtBydm.Text.Trim() == checkbox.Tag.ToString())
                        //    {
                        //        PublicMethod.RadAlterBox("此编码已经存在，不能重复创建", "提示");
                        //        return;
                        //    }
                        //}
                        if (txtByms.Text.Length > 255)
                        {
                            PublicMethod.RadAlterBoxRe("变异描述内容过长，已经超过255个字节！", "提示", txtByms); isLoad = false; return;
                        }
                        if (txtBymc.Text.Length > 64)
                        {
                            PublicMethod.RadAlterBoxRe("变异名称内容过长，已经超过64个字节！", "提示", txtBymc); isLoad = false; return;
                        }
                        if (txtPym.Text.Length > 8)
                        {
                            PublicMethod.RadAlterBoxRe("拼音码内容过长，已经超过8个字节！", "提示", txtPym); isLoad = false; return;
                        }

                        //if (GridViewPathVariation.Items)
                        //{

                        //}

                        YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                        referenceClient.AddVariationCodeCompleted +=
                           (obj, ea) =>
                           {
                               if (ea.Error == null)
                               {


                                   BindGridView();
                                   BindcbxVariationType();
                                   BindTreeView();

                                   TreeView.SelectedItem = null;
                                   txtBydm.Text = "";
                                   txtBymc.Text = "";
                                   txtByms.Text = "";
                                   txtPym.Text = "";
                                   PublicMethod.RadAlterBox("编码添加成功！", "提示");

                               }
                               else
                               {
                                   PublicMethod.RadWaringBox(ea.Error);
                               }
                           };
                        referenceClient.AddVariationCodeAsync(txtBydm.Text.Trim().ToUpper(), txtBymc.Text.Trim(), txtByms.Text.Trim(), 1, txtPym.Text.Trim());
                        referenceClient.CloseAsync();
                    }
                    //}

                    //catch (Exception ex)
                    //{
                    //    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                    //}
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        void AddCP_PathVariationClose(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                //try
                //{
                if (operationType == 2) //创建变异编码
                {
                    //foreach (CheckBox checkbox in m_GridCheckBox)
                    //{
                    //    if (txtBydm.Text.Trim() == checkbox.Tag.ToString())
                    //    {
                    //        PublicMethod.RadAlterBox("此编码已经存在，不能重复创建", "提示");
                    //        return;
                    //    }
                    //}
                    if (txtByms.Text.Length > 255)
                    {
                        PublicMethod.RadAlterBoxRe("变异描述内容过长，已经超过255个字节！", "提示", txtByms); isLoad = false; return;
                    }
                    if (txtBymc.Text.Length > 64)
                    {
                        PublicMethod.RadAlterBoxRe("变异名称内容过长，已经超过64个字节！", "提示", txtBymc); isLoad = false; return;
                    }
                    if (txtPym.Text.Length > 8)
                    {
                        PublicMethod.RadAlterBoxRe("拼音码内容过长，已经超过8个字节！", "提示", txtPym); isLoad = false; return;
                    }

                    //if (GridViewPathVariation.Items)
                    //{

                    //}

                    YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                    referenceClient.AddVariationCodeCompleted +=
                       (obj, ea) =>
                       {
                           if (ea.Error == null)
                           {


                               BindGridView();
                               BindcbxVariationType();
                               BindTreeView();

                               TreeView.SelectedItem = null;
                               txtBydm.Text = "";
                               txtBymc.Text = "";
                               txtByms.Text = "";
                               txtPym.Text = "";
                               PublicMethod.RadAlterBox("编码添加成功！", "提示");

                           }
                           else
                           {
                               PublicMethod.RadWaringBox(ea.Error);
                           }
                       };
                    referenceClient.AddVariationCodeAsync(txtBydm.Text.Trim().ToUpper(), txtBymc.Text.Trim(), txtByms.Text.Trim(), 1, txtPym.Text.Trim());
                    referenceClient.CloseAsync();
                }
                //}

                //catch (Exception ex)
                //{
                //    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                //}
            }
        }
        void EnableCP_PathVariationClose(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                try
                {
                    if (operationType == 1 || operationType == 0)//启用编码/停用编码
                    {
                        ObservableCollection<string> setList = new ObservableCollection<string>();
                        //foreach (CheckBox checkbox in m_GridCheckBox)
                        //{
                        //    if (checkbox.IsChecked == true)
                        //    {
                        //        setList.Add(checkbox.Tag.ToString());
                        //    }
                        //}

                        //foreach (CP_PathVariation item in GridViewPathVariation.Items)
                        //{

                        //}
                        if ((CP_PathVariation)GridViewPathVariation.SelectedItem != null)
                        {
                            setList.Add(((CP_PathVariation)GridViewPathVariation.SelectedItem).Bydm);
                        }

                        if (setList.Count != 0)
                        {
                            YidanEHRDataServiceClient QueryClient = PublicMethod.YidanClient;
                            QueryClient.SetPathVariationCodeStateCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    if (operationType == 1)
                                    {
                                        PublicMethod.RadAlterBox("编码启用设置成功。", "提示");
                                    }
                                    else
                                    {
                                        PublicMethod.RadAlterBox("编码停用设置成功。", "提示");
                                    }
                                    BindGridView();
                                }
                                else
                                {
                                    PublicMethod.RadWaringBox(ea.Error);
                                }
                            };
                            QueryClient.SetPathVariationCodeStateAsync(setList, operationType);
                            QueryClient.CloseAsync();
                        }
                        else
                        {
                            if (operationType == 1)
                            {
                                PublicMethod.RadAlterBox("三级编码未选中，请勾选你要启用的三级编码！", "提示");
                            }
                            else
                            {
                                PublicMethod.RadAlterBox("三级编码未选中，请勾选你要停用的三级编码！", "提示");
                            }
                        }
                    }
                    foreach (var item in m_GridCheckBox)
                    {
                        item.IsChecked = false;
                    }
                }
                catch (Exception ex)
                {
                    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            }
        }

        #region  输入框加回车事件
        private void RegisterKeyEvent()
        {
            ddbBydm.KeyUp += new KeyEventHandler(ddbBydm_KeyUp);
            txtBydm.KeyUp += new KeyEventHandler(txtBydm_KeyUp);
            txtBymc.KeyUp += new KeyEventHandler(txtBymc_KeyUp);

            txtPym.KeyUp += new KeyEventHandler(txtPym_KeyUp);
            txtByms.KeyUp += new KeyEventHandler(txtByms_KeyUp);
            btnCreateCode.KeyUp += new KeyEventHandler(btnCreateCode_KeyUp);

        }

        private void ddbBydm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtBydm.Focus();
        }

        private void txtBydm_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtBymc.Focus();
        }

        private void txtBymc_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtPym.Focus();
        }

        private void txtPym_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtByms.Focus();
        }

        private void txtByms_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnCreateCode.Focus();
        }

        private void btnCreateCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnCreateCode_Click(null, null);
        }

        #endregion

        #endregion

        #region 函数

        /// <summary>
        /// 绑定cbxVariationType
        /// </summary>
        private void BindcbxVariationType()
        {
            YidanEHRDataServiceClient QueryClient = PublicMethod.YidanClient;
            QueryClient.GetFirstCodeListCompleted += (obj, ea) =>
                {
                    if (ea.Error == null)
                    {
                        if (ea.Result.Count > 0)
                        {
                            cbxVariationType.ItemsSource = ea.Result.ToList();
                            cbxVariationType.DisplayMemberPath = "Bymc";
                            cbxVariationType.SelectedValuePath = "Bydm";
                            cbxVariationType.SelectedIndex = 0;
                            cbxQueryCodeState.SelectedIndex = 0;
                        }
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(ea.Error);
                    }
                };
            QueryClient.GetFirstCodeListAsync();
        }

        private void BindTreeView()
        {
            ((CP_PathVariationClients)this.Resources["Items"]).Clear();
            YidanEHRDataServiceClient QueryClient = PublicMethod.YidanClient;
            CP_PathVariationClients cP_PathVariations = new CP_PathVariationClients();
            QueryClient.GetDataPathVariationListAllCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        List<CP_PathVariation> cpv = e.Result.ToList();
                        cP_PathVariations.AddRange(cpv);
                        foreach (var cP_PathVariation in cP_PathVariations.GetChildren(null))
                        {
                            CP_PathVariationClient item = cP_PathVariation;
                            foreach (var item2 in cP_PathVariations.GetChildren(item.Bydm))
                            {
                                item.Children.Add(item2);
                            }
                            ((CP_PathVariationClients)this.Resources["Items"]).Add(item);
                        }
                        TreeView.ItemsSource = null;
                        TreeView.ItemsSource = (CP_PathVariationClients)this.Resources["Items"];
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            QueryClient.GetDataPathVariationListAllAsync();
            QueryClient.CloseAsync();
        }

        public PathVariationDetail()
        {
            InitializeComponent();
            #region 初始化树形下拉框
            BindTreeView();
            //YidanEHRDataServiceClient QueryClient = PublicMethod.YidanClient;
            //CP_PathVariationClients cP_PathVariations = new CP_PathVariationClients();
            //QueryClient.GetDataPathVariationListAllCompleted +=
            //    (obj, e) =>
            //    {
            //        if (e.Error == null)
            //        {
            //            List<CP_PathVariation> cpv = e.Result.ToList();
            //            cP_PathVariations.AddRange(cpv);
            //            foreach (var cP_PathVariation in cP_PathVariations.GetChildren(null))
            //            {
            //                CP_PathVariationClient item = cP_PathVariation;
            //                foreach (var item2 in cP_PathVariations.GetChildren(item.Bydm))
            //                {
            //                    item.Children.Add(item2);
            //                }
            //                ((CP_PathVariationClients)this.Resources["Items"]).Add(item);
            //            }

            //        }
            //        else
            //        {
            //            PublicMethod.RadWaringBox(e.Error);
            //        }
            //    };
            //QueryClient.GetDataPathVariationListAllAsync();
            //QueryClient.CloseAsync();
            #endregion
            BindGridView();

            RegisterKeyEvent();
        }
        private void BindGridView()
        {
            if (cbxVQueryType.SelectedIndex == 0 && txtCode.Text.Trim() == "")
            {
                PublicMethod.RadAlterBox("精确查询时必须输入变异编码!", " 提示");
                return;
            }
            radBusyIndicator.IsBusy = true;
            YidanEHRDataServiceClient QueryClient = PublicMethod.YidanClient;
            QueryClient.GetDataPathVariationListCompleted +=
                (obj, e) =>
                {
                    radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        m_PathVariation = e.Result.ToList();
                        var view = new QueryableCollectionView(m_PathVariation);
                        this.GridViewPathVariation.ItemsSource = view;

                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            QueryClient.GetDataPathVariationListAsync(
                this.cbxVQueryType.SelectedIndex,
               ConvertMy.ToString(this.cbxVariationType.SelectedValue),
                this.cbxQueryCodeType.SelectedIndex,
                this.cbxQueryCodeState.SelectedIndex,
                this.txtCode.Text.Replace(" ", ""));
            QueryClient.CloseAsync();
        }
        #endregion

        private void btnTxtClear_Click(object sender, RoutedEventArgs e)
        {
            txtBydm.Text = txtBymc.Text = txtByms.Text = txtPym.Text = string.Empty;
            TreeView.SelectedItem = null;
            txtBydm.IsReadOnly = false;
        }

        private void btnClearCode_Click(object sender, RoutedEventArgs e)
        {
            cbxVQueryType.SelectedIndex = 1;
            cbxVariationType.SelectedIndex = 0;
            //cbxVariationType.SelectedValue = null;
            cbxQueryCodeType.SelectedIndex = 0;
            cbxQueryCodeState.SelectedIndex = 0;
            txtCode.Text = "";
        }

    }
}
namespace YidanEHRApplication.ViewsViews.Tool
{
    [ContentProperty("Children")]
    public partial class CP_PathVariationClient
    {

        public CP_PathVariationClient()
        {
            this.Children = new CP_PathVariationClients();
        }
        CP_PathVariationClients _Children = new CP_PathVariationClients();
        public CP_PathVariationClients Children
        {
            get
            {
                if (_Children == null)
                {
                    _Children = new CP_PathVariationClients();
                }
                return _Children;
            }
            set { }
        }
        ///// <summary>
        ///// 界面显示字段
        ///// </summary>
        //public String DisplayProperty
        //{
        //    get { return Bydm + ":" + Bymc; }
        //    set { }
        //}
        private String _Bydm;

        public String Bydm
        {
            get { return _Bydm; }
            set { _Bydm = value; }
        }
        private String _Bymc;

        public String Bymc
        {
            get { return _Bymc; }
            set { _Bymc = value; }
        }
        private String _DisplayProperty;

        public String DisplayProperty
        {
            get { return Bydm + ":" + Bymc; }
            set { _DisplayProperty = value; }
        }
    }
    public class CP_PathVariationClients : List<CP_PathVariationClient>
    {
        /// <summary>
        /// 获取当前节点的子节点，但是不包括叶子节点
        /// </summary>
        /// <param name="bydm">当前节点代码，如果当前节点为null，就获取所有的顶级节点,如果当前节点不为null，则取除子节点外的所有子节点</param>
        /// <returns></returns>
        public CP_PathVariationClients GetChildren(String bydm)
        {
            CP_PathVariationClients cP_PathVariations = new CP_PathVariationClients();
            CP_PathVariationClients cP_PathVariationsTemp = new CP_PathVariationClients();
            if (bydm == null)
            {
                cP_PathVariations.AddRange(this.Select(s => s).Where(s => !s.Bydm.Contains('.')).ToList());
                return cP_PathVariations;
            }
            cP_PathVariationsTemp.AddRange(this.Select(s => s).Where(s => s.Bydm.StartsWith(bydm)).ToList());
            foreach (var item in cP_PathVariationsTemp)
            {
                String[] itembydmArr = item.Bydm.Split('.');
                String[] bydmArr = bydm.Split('.');
                if (itembydmArr.Count() < 3 && itembydmArr.Count() > bydmArr.Count())
                {
                    cP_PathVariations.Add(item);
                }
            }
            return cP_PathVariations;
        }
        /// <summary>
        /// 根据当前节点，产生新的子节点
        /// </summary>
        /// <param name="bydm">当前节点编号</param>
        /// <returns></returns>
        public String GenerateNewChild(String bydm)
        {
            String returnStr = String.Empty;
            CP_PathVariationClients cP_PathVariationsTemp = new CP_PathVariationClients();
            //查询当前节点的所有子节点，
            cP_PathVariationsTemp.AddRange(this.Select(s => s).Where(s => s.Bydm.StartsWith(bydm)).ToList());
            //编码最后一段，即最后一个小数点后面的数字
            Int32 maxBydmNum = 0;
            foreach (var item in cP_PathVariationsTemp)
            {
                String[] itembydmArr = item.Bydm.Split('.');
                String[] bydmArr = bydm.Split('.');
                //当存在当前节点的下一级，并取下一级中最大的编号加一，产生新节点
                if (itembydmArr.Count() == bydmArr.Count() + 1)
                {
                    if (maxBydmNum < ConvertMy.ToInt32(itembydmArr[itembydmArr.Count() - 1]))
                    {
                        maxBydmNum = ConvertMy.ToInt32(itembydmArr[itembydmArr.Count() - 1]);
                        returnStr = item.Bydm.Substring(0, item.Bydm.LastIndexOf('.')) + "." + StringManage.AddPre((maxBydmNum + 1).ToString(), '0', 3);
                    }
                }
            }
            //当不存在下一级
            if (returnStr == String.Empty)
            {
                returnStr = bydm + ".001";
            }
            return returnStr;
        }

        public void AddRange(List<CP_PathVariation> CP_PathVariations)
        {
            foreach (var item in CP_PathVariations)
            {
                CP_PathVariationClient CP_PathVariationClientTemp = new CP_PathVariationClient();
                CP_PathVariationClientTemp.Bydm = item.Bydm;
                CP_PathVariationClientTemp.Bymc = item.Bymc;
                this.Add(CP_PathVariationClientTemp);
            }
        }


    }
    public class ObjectToFalseConverter : IValueConverter
    {
        WeakReference oldValue = new WeakReference(null);
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value != oldValue.Target || oldValue.Target == null)
            {
                oldValue.Target = value;
                return false;
            }
            return true;
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return oldValue.Target;
        }
    }
}