using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using YidanEHRApplication.Views.ChildWindows;
namespace YidanEHRApplication.Views
{
    public partial class Page_CP_NurExecCategoryDetailMaintain : Page
    {
        public Page_CP_NurExecCategoryDetailMaintain()
        {
            InitializeComponent();

            RegisterKeyEvent();
        }
        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }
        #region 事件

        public bool isLoad = true;
        /// <summary>
        /// 页面加载
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            if (!isLoad)
            {
                isLoad = true;
                return;
            }

            GetData("select", "", "", "", "", "", "", "", "", "", "");
            GetData("selectList", "", "", "", "", "", "", "", "", "", "");
            BindHljg();
        }
        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            String operation = "select";
            String Lbxh = (cbxQuery.SelectedItem != null) ? (cbxQuery.SelectedItem as CP_NurExecCategoryDetailMaintainList).Lbxh : string.Empty;
            String Yxjl = (cbxYxjlQuery.SelectedItem != null) ? (cbxYxjlQuery.SelectedIndex).ToString() : string.Empty;
            String Sfsy = (cbxSfsyQuery.SelectedItem != null) ? (cbxSfsyQuery.SelectedIndex).ToString() : string.Empty;

            GridViewNur.ItemsSource = null;             //清屏
            GetData(operation, Lbxh, Yxjl, Sfsy, "", "", "", "", "", "", "");
        }
        /// <summary>
        /// 创建
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnCreate_Click(object sender, RoutedEventArgs e)
        {
            String operation = "insert";
            if (cbxInsert.SelectedItem == null)
            {
                PublicMethod.RadAlterBoxRe("请选择护理分类", "提示", cbxInsert);
                isLoad = false;
                return;
            }
            if (JgbhList == String.Empty)
            {
                PublicMethod.RadAlterBoxRe("护理结果必须选择", "提示", cmbHljg);
                isLoad = false;
                return;
            }
            if (txtInsert.Text.Length > 100)
            {
                PublicMethod.RadAlterBoxRe("护理名称长度不能超出50个汉字", "提示", txtInsert);
                isLoad = false;
                return;
            }
            //txtInsert
            foreach (CP_NurExecCategoryDetailMaintainList item in GridViewNur.Items)
            {
                if (cbxInsert.SelectedValue.ToString() == item.LbxhName && txtInsert.Text.Trim() == item.MxxhName)
                {
                    PublicMethod.RadAlterBoxRe("该护理分类已经存在", "提示", txtInsert);
                    isLoad = false;
                    return;
                }
            }
            String Lbxh = (cbxInsert.SelectedItem != null) ? (cbxInsert.SelectedItem as CP_NurExecCategoryDetailMaintainList).Lbxh : string.Empty;
            String Yxjl = "1";                              //默认有效
            String Sfsy = "0";                              //默认未使用
            String Mxxh = Guid.NewGuid().ToString();        //自动生成

            if (txtInsert.Text == null || txtInsert.Text == "")
            {
                PublicMethod.RadAlterBoxRe("请输入护理名称", "提示", txtInsert);
                isLoad = false;
                return;
            }
            String MxxhName = txtInsert.Text.Trim();
            String Create_Time = DateTime.Now.ToString();
            String Create_User = Global.LogInEmployee.Zgdm; //职工代码

            GetData(operation, Lbxh, Yxjl, Sfsy, Mxxh, MxxhName, Create_Time, Create_User, "", "", JgbhList);
            PublicMethod.RadAlterBox("添加成功", "提示");
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {

            try
            {
                if (GridViewNur.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择一项", "提示");
                    return;
                }
                else
                {
                    //DialogParameters parameters = new DialogParameters();/* uodate by dxj 2011-7-26 删除提示 */
                    //parameters.Content = String.Format("提示: {0}", "确定删除吗？删除后不能恢复!");
                    //parameters.Header = "提示";
                    //parameters.IconContent = null;
                    //parameters.OkButtonContent = "确定";// @"/Pathway;component/Images/确定.png";
                    //parameters.CancelButtonContent = "取消";
                    //parameters.Closed = OnDelete;
                    //RadWindow.Confirm(parameters);
                    YidanPathWayMessageBox mess = new YidanPathWayMessageBox("请问是否删除选中的信息吗？", "提示", YiDanMessageBoxButtons.YesNo);
                    mess.ShowDialog();
                    mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
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
                    String operation = "delete";
                    String Mxxh = (GridViewNur.SelectedItem as CP_NurExecCategoryDetailMaintainList).Mxxh;

                    GetData(operation, "", "", "", Mxxh, "", "", "", "", "", "");
                    PublicMethod.RadAlterBox("删除成功", "提示");

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        void OnDelete(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                String operation = "delete";
                String Mxxh = (GridViewNur.SelectedItem as CP_NurExecCategoryDetailMaintainList).Mxxh;

                GetData(operation, "", "", "", Mxxh, "", "", "", "", "", "");
                PublicMethod.RadAlterBox("删除成功", "提示");
            }
        }
        /// <summary>
        ///启用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnEnable_Click(object sender, RoutedEventArgs e)
        {
            String operation = "updateBegin";

            if (GridViewNur.SelectedItem == null)
            {
                PublicMethod.RadAlterBox("请选择一项", "提示");
                return;
            }

            String Mxxh = (GridViewNur.SelectedItem as CP_NurExecCategoryDetailMaintainList).Mxxh;

            if ((GridViewNur.SelectedItem as CP_NurExecCategoryDetailMaintainList).Sfsy == "使用中")
            {
                PublicMethod.RadAlterBox("正在使用的护理禁止修改操作", "提示");
                return;
            }

            if (((CP_NurExecCategoryDetailMaintainList)GridViewNur.SelectedItem).Yxjl.ToString() == "停用")
            {
                GetData(operation, "", "", "", Mxxh, "", "", "", "", "", "");
                PublicMethod.RadAlterBox("启用成功", "提示");
            }
            else
            {
                PublicMethod.RadAlterBox("状态已经为启用", "提示"); return;
            }


        }
        /// <summary>
        /// 停用
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnStop_Click(object sender, RoutedEventArgs e)
        {
            String operation = "updateEnd";

            if (GridViewNur.SelectedItem == null)
            {
                PublicMethod.RadAlterBox("请选择一项", "提示");
                return;
            }

            String Mxxh = (GridViewNur.SelectedItem as CP_NurExecCategoryDetailMaintainList).Mxxh;

            if ((GridViewNur.SelectedItem as CP_NurExecCategoryDetailMaintainList).Sfsy == "使用中")
            {
                PublicMethod.RadAlterBox("正在使用的护理禁止修改操作", "提示");
                return;
            }
            if (((CP_NurExecCategoryDetailMaintainList)GridViewNur.SelectedItem).Yxjl.ToString() == "启用")
            {
                GetData(operation, "", "", "", Mxxh, "", "", "", "", "", "");
                PublicMethod.RadAlterBox("停用成功", "提示");
            }
            else
            {
                PublicMethod.RadAlterBox("状态已经为停用", "提示"); return;
            }
        }

        /// <summary>
        /// 表示护理分类维护按钮的点击事件
        /// </summary>
        private void btnHlflWh_Click(object sender, RoutedEventArgs e)
        {
            HlNurExecCategory hnec = new HlNurExecCategory();
            hnec.Closed += new EventHandler<WindowClosedEventArgs>(hnec_Closed);
            hnec.ResizeMode = ResizeMode.NoResize;
            hnec.ShowDialog();
        }
        private void hnec_Closed(object sender, EventArgs e)
        {
            GetData("selectList", "", "", "", "", "", "", "", "", "", "");
        }
        /// <summary>
        /// 表示护理结果维护按钮的点击事件
        /// </summary>
        private void btnHljgWh_Click(object sender, RoutedEventArgs e)
        {
            HlNurResult hlNurResult = new HlNurResult();
            hlNurResult.Closed += new EventHandler<WindowClosedEventArgs>(hlNurResult_Closed);
            hlNurResult.ResizeMode = ResizeMode.NoResize;
            hlNurResult.ShowDialog();
        }
        private void hlNurResult_Closed(object sender, EventArgs e)
        {
            BindHljg();
        }
        private void chkHljg_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                String Hljg = String.Empty;
                JgbhList = String.Empty;
                if (this.cmbHljg.ItemsSource == null)
                {
                    return;
                }
                foreach (CheckBox check in m_cmbCheckBox)
                {
                    if (check.Tag.ToString() == (sender as CheckBox).Tag.ToString())
                    {
                        check.IsChecked = true;
                    }
                    if (check.IsChecked == true)
                    {
                        Hljg += check.Content.ToString() + ";";
                        JgbhList += check.Tag.ToString() + ",";
                    }
                }
                JgbhList = (Hljg.Length > 0) ? JgbhList.Substring(0, JgbhList.Length - 1) : JgbhList;
                Hljg = (Hljg.Length > 10) ? Hljg.Substring(0, 10) + "..." : Hljg;
                cmbHljg.EmptyText = Hljg;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void chkHljg_Loaded(object sender, RoutedEventArgs e)
        {
            if (!m_cmbCheckBox.Contains(sender as CheckBox))
            {
                m_cmbCheckBox.Add(sender as CheckBox);
            }
        }
        private void chkHljg_Unchecked(object sender, RoutedEventArgs e)
        {
            String Hljg = String.Empty;
            JgbhList = String.Empty;
            if (this.cmbHljg.ItemsSource == null)
                return;
            foreach (CheckBox check in m_cmbCheckBox)
            {
                if (check.Tag.ToString() == (sender as CheckBox).Tag.ToString())
                {
                    check.IsChecked = false;
                }
                if (check.IsChecked == true)
                {
                    Hljg += check.Content.ToString() + ";";
                    JgbhList += check.Tag.ToString() + ",";
                }
            }
            JgbhList = (Hljg.Length > 0) ? JgbhList.Substring(0, JgbhList.Length - 1) : JgbhList;
            Hljg = (Hljg.Length > 10) ? Hljg.Substring(0, 10) + "..." : Hljg;
            cmbHljg.EmptyText = Hljg;

        }

        #region  输入框加回车事件
        private void RegisterKeyEvent()
        {
            cbxInsert.KeyUp += new KeyEventHandler(cbxInsert_KeyUp);
            txtInsert.KeyUp += new KeyEventHandler(txtInsert_KeyUp);
            cmbHljg.KeyUp += new KeyEventHandler(cmbHljg_KeyUp);
            btnCreateCode.KeyUp += new KeyEventHandler(btnCreateCode_KeyUp);

        }

        private void cbxInsert_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtInsert.Focus();
        }

        private void txtInsert_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cmbHljg.Focus();
        }

        private void cmbHljg_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnCreateCode.Focus();
        }

        private void btnCreateCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnCreate_Click(null, null);
        }

        #endregion
        #endregion

        #region 函数
        private void GetData(String Operation, String Lbxh, String Yxjl, String Sfsy, String Mxxh, String MxxhName, String Create_Time, String Create_User, String Cancel_Time, String Cancel_User, String JgbhList)
        {
            try
            {
                radBusyIndicator.IsBusy = true;
                CP_NurExecCategoryDetailMaintain cP_NurExecCategoryDetailMaintain = new CP_NurExecCategoryDetailMaintain();

                YidanEHRDataServiceClient GetNurExecCategoryDetailMaintainClient = PublicMethod.YidanClient;
                GetNurExecCategoryDetailMaintainClient.GetCP_NurExecCategoryDetailMaintainCompleted +=
                (obj, e) =>
                {
                    radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        cP_NurExecCategoryDetailMaintain = e.Result;

                        if (cP_NurExecCategoryDetailMaintain.CP_NurExecCategoryDetailMaintainList == null)      //防止内容为空
                        {
                            return;
                        }
                        else
                        {
                            if (Operation == "selectList")
                            {
                                cbxInsert.ItemsSource = cP_NurExecCategoryDetailMaintain.CP_NurExecCategoryDetailMaintainList.ToList();
                                cbxQuery.ItemsSource = cP_NurExecCategoryDetailMaintain.CP_NurExecCategoryDetailMaintainList.ToList();
                            }
                            else
                            {
                                GridViewNur.ItemsSource = cP_NurExecCategoryDetailMaintain.CP_NurExecCategoryDetailMaintainList.ToList();
                            }
                        }
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };

                GetNurExecCategoryDetailMaintainClient.GetCP_NurExecCategoryDetailMaintainAsync(Operation, Lbxh, Yxjl, Sfsy, Mxxh, MxxhName, Create_Time, Create_User, Cancel_Time, Cancel_User, JgbhList);
                GetNurExecCategoryDetailMaintainClient.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 绑定护理结果
        /// </summary>
        private void BindHljg()
        {
            try
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                CP_NurResult nr = new CP_NurResult();
                nr.Name = String.Empty;
                nr.Create_User = String.Empty;
                nr.Update_User = String.Empty;
                nr.Yxjl = "1";
                client.InsertAndSelectNurResultCompleted += (obj, ea) =>
                {
                    if (ea.Error == null)
                    {
                        cmbHljg.ItemsSource = ea.Result;
                    }
                    else
                    {
                        PublicMethod.RadAlterBox(ea.Error.ToString(), "提示");
                    }
                };
                client.InsertAndSelectNurResultAsync(nr);
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion

        #region 变量
        ObservableCollection<CheckBox> m_cmbCheckBox = new ObservableCollection<CheckBox>();
        /// <summary>
        /// 护理结果集合
        /// </summary>
        String JgbhList = String.Empty;
        #endregion

        private void btnClearCode_Click(object sender, RoutedEventArgs e)
        {
            cbxQuery.SelectedIndex = -1;
            cbxYxjlQuery.SelectedIndex = -1;
            cbxSfsyQuery.SelectedIndex = -1;
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            cbxInsert.SelectedIndex = -1;
            txtInsert.Text = "";
            cmbHljg.Text = null;

            foreach (CheckBox check in m_cmbCheckBox)
            {
                check.IsChecked = false;
            }
        }

        private void cmbHljg_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            cmbHljg.Text = "";
        }



    }
}
