using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;

using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using Telerik.Windows.Controls;
using YidanSoft.Tool;
using Telerik.Windows.Controls.GridView;
using System.Windows.Data;
using System.Collections;
using YidanEHRApplication.Helpers;
using System.Collections.ObjectModel;

namespace YidanEHRApplication.Views
{
    public partial class PathCutQuery
    {
        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        public PathCutQuery()
        {
            InitializeComponent();
            IntiComboBoxStatus(false);
            IntiComboBoxDept();
            InitComboBoxPath();

            GetPathCut("", "");
        }
        #region 事件
        /// <summary>
        /// 查询按钮
        /// </summary>
        private void radbuttonQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                String Ljmc = this.autoCompleteBoxQueryPath.SelectedItem == null ? this.autoCompleteBoxQueryPath.Text : ((CP_ClinicalPathList)this.autoCompleteBoxQueryPath.SelectedItem).Name;
                String Ksdm = this.autoCompleteBoxQueryDept.SelectedItem == null ? string.Empty : ((CP_DepartmentList)this.autoCompleteBoxQueryDept.SelectedItem).Ksdm;
                GetPathCut(Ljmc.Replace("'",""), Ksdm);
            }
            catch (Exception ex)
            {
                PublicMethod.RadAlterBox(ex.ToString(), "警告");
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 行加载
        /// ZM 8.2 取消(按钮不再是自动加载，不用在行加载中注册事件)
        /// </summary>
        private void GridViewPathCut_RowLoaded11(object sender, RowLoadedEventArgs e)
        {
            RadButton btn = e.Row.FindChildByType<RadButton>();                //寻找按钮

            if (btn != null)                                             //不为空
            {
                //CP_ClinicalPathList cp = new CP_ClinicalPathList();

                //GridViewRow row = GridViewPathCut.ItemContainerGenerator.ContainerFromItem(e.Row.Item) as GridViewRow;  //获得该行

                //GridViewCellBase Ljdm = (from c in row.Cells                    //想获取单元格，前提单元格要是可见的
                //                         where c.Column.UniqueName == "Ljdm"
                //                         select c).FirstOrDefault();

                //cp.Ljdm = ((TextBlock)(Ljdm.Content)).Text;

                //btn.Click += new RoutedEventHandler(btnConfig_Click);        //注册click事件
            }
        }
        /// <summary>
        /// 配置
        /// 8.2  不在采用原有操作方式
        /// </summary>
        //private void btnConfig_Click(object sender, RoutedEventArgs e)
        //{
        //    try
        //    {
        //        String Ljdm = (((RadButton)sender).Tag).ToString();          //获得Ljdm

        //        YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
        //        referenceClient.GetClinicalPathListCompleted +=
        //        (obj, ea) =>
        //        {
        //            if (ea.Error == null)
        //            {
        //                oldCp = ea.Result.ToList()[0];                  //存储过程是返回List类型，其实根据Ljdm只有一个结果
        //                textBoxPathName.IsEnabled = true;
        //                radNumericUpDownVersion.IsEnabled = true;
        //                radNumericUpDownInDays.IsEnabled = true;
        //                radNumericUpDownAvgFee.IsEnabled = true;
        //                radComboBoxStatus.IsEnabled = true;
        //                radButtonAdd.IsEnabled = true;
        //                radButtonReSet.IsEnabled = true;

        //                textBoxOldPathName.Text = oldCp.Name;               //标题
        //                textBoxPathName.Text = oldCp.Name + "裁剪";       //自动生成路径名
        //                textBoxDept.Text = oldCp.DeptName;                  //科室
        //            }
        //            else
        //            {
        //                PublicMethod.RadWaringBox(ea.Error);
        //            }
        //        };
        //        referenceClient.GetClinicalPathListAsync("", "", "", Ljdm, "", "");
        //        referenceClient.CloseAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
        //    }
        //}
        /// <summary>
        /// 确定
        /// </summary>
        //private void radButtonAdd_Click(object sender, RoutedEventArgs e)
        //{
        //    if (textBoxPathName.Text.Trim() == oldCp.Name)
        //    {
        //        PublicMethod.RadAlterBox("裁剪路径名不能和原路径相同", "提示");
        //        return;
        //    }

        //    if (Check())
        //    {
        //        DialogParameters parameters = new DialogParameters();
        //        parameters.Content = "是否创建新路径并开始裁剪？";
        //        parameters.Header = "提示";
        //        parameters.IconContent = null;
        //        parameters.OkButtonContent = "确定";
        //        parameters.CancelButtonContent = "取消";
        //        parameters.Closed = beginCut;
        //        RadWindow.Confirm(parameters);
        //    }
        //}
        /// <summary>
        /// 重置
        /// </summary>
        //private void radButtonReSet_Click(object sender, RoutedEventArgs e)
        //{
        //    IntiComboBoxStatus(false);
        //    textBoxPathName.Text = string.Empty;
        //    radNumericUpDownVersion.Value = 0;
        //    radNumericUpDownInDays.Value = 0;
        //    radNumericUpDownAvgFee.Value = 0;
        //    radComboBoxStatus.SelectedValue = 1;

        //    textBoxDept.Text = string.Empty;
        //}
        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //8.2  修改后，直接获取
                //String Ljdm = (((RadButton)sender).Tag).ToString();          //获得Ljdm

                if (GridViewPathCut.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择一个路径再点击准备裁剪", "");
                    return;
                }

                GridViewRow row = GridViewPathCut.ItemContainerGenerator.ContainerFromItem(GridViewPathCut.SelectedItem) as GridViewRow;  //获得该行

                if (row != null)
                {
                    GridViewCellBase cell = (from c in row.Cells
                                             where c.Column.UniqueName == "Ljdm"
                                             select c).FirstOrDefault();
                    if (cell != null)
                    {
                        String Ljdm = ((TextBlock)(cell.Content)).Text;

                        YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                        referenceClient.GetClinicalPathListCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                if (ea.Result.Count == 0)
                                {
                                    PublicMethod.RadAlterBox("当前还没有选中路径", "提示");
                                    return;
                                }
                                oldCp = ea.Result.ToList()[0];                  //存储过程是返回List类型，其实根据Ljdm只有一个结果
                                                                                //之前存在问题，跟据路径代码却取不出路径，后测试发现
                                                                                //裁剪是查出所有路径，而管理不查无效路径，就出现了选中一条
                                                                                //路径后，准备裁剪，集合为0。
                                textBoxPathName.IsEnabled = true;
                                radNumericUpDownVersion.IsEnabled = true;
                                radNumericUpDownInDays.IsEnabled = true;
                                radNumericUpDownAvgFee.IsEnabled = true;
                                radComboBoxStatus.IsEnabled = true;
                                btnAdd.IsEnabled = true;
                                btnReSet.IsEnabled = true;

                                textBoxOldPathName.Text = oldCp.Name;             //标题
                                textBoxPathName.Text = oldCp.Name + "裁剪";       //自动生成路径名
                                textBoxDept.Text = oldCp.DeptName;                //科室
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                        referenceClient.GetClinicalPathListAsync("", "", "", Ljdm, "", "");
                        referenceClient.CloseAsync();
                    }
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (textBoxPathName.Text.Trim() == oldCp.Name)
            {
                PublicMethod.RadAlterBox("裁剪路径名不能和原路径相同", "提示");
                return;
            }

            if (Check())
            {
                //DialogParameters parameters = new DialogParameters();
                //parameters.Content = "是否创建新路径并开始裁剪？";
                //parameters.Header = "提示";
                //parameters.IconContent = null;
                //parameters.OkButtonContent = "确定";
                //parameters.CancelButtonContent = "取消";
                //parameters.Closed = beginCut;
                //RadWindow.Confirm(parameters);

                YidanPathWayMessageBox mess = new YidanPathWayMessageBox("是否创建新路径并开始裁剪？", "提示", YiDanMessageBoxButtons.YesNo);
                mess.ShowDialog();
                mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
            }
        }
        private void btnReSet_Click(object sender, RoutedEventArgs e)
        {
            ReSet();
        }
        #endregion
        #region 函数
        private void ReSet()
        {
            IntiComboBoxStatus(false);
            textBoxPathName.Text = string.Empty;
            radNumericUpDownVersion.Value = 0;
            radNumericUpDownInDays.Value = 0;
            radNumericUpDownAvgFee.Value = 0;
            radComboBoxStatus.SelectedValue = 1;

            textBoxDept.Text = string.Empty;
        }
        private void ViewState()
        {
            textBoxPathName.IsEnabled = false;
            radNumericUpDownVersion.IsEnabled = false;
            radNumericUpDownInDays.IsEnabled = false;
            radNumericUpDownAvgFee.IsEnabled = false;
            radComboBoxStatus.IsEnabled = false;
            textBoxDept.IsEnabled = false;

            ReSet();
        }
        /// <summary>
        ///   INIT路径状态
        /// </summary>
        /// <param name="isReivew">是否要加审核</param>
        private void IntiComboBoxStatus(bool isReivew)
        {
            try
            {
                radComboBoxStatus.ItemsSource = null;
                List<Status> statusList = new List<Status>();
                statusList.Add(new Status("无效", (int)PathShStatus.Cancel));
                statusList.Add(new Status("有效", (int)PathShStatus.Valid));
                statusList.Add(new Status("停止", (int)PathShStatus.Dc));
                if (isReivew)
                    statusList.Add(new Status("审核", (int)PathShStatus.Review));
                radComboBoxStatus.ItemsSource = statusList;
                radComboBoxStatus.SelectedIndex = 1;
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        public bool DeptFilter(string strFilter, object item)
        {
            CP_DepartmentList deptList = (CP_DepartmentList)item;
            return ((deptList.QueryName.StartsWith(strFilter)) || (deptList.QueryName.Contains(strFilter)));
        }
        /// <summary>
        /// INIT科室
        /// </summary>
        private void IntiComboBoxDept()
        {
            try
            {
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                referenceClient.GetDepartmentListInfoCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            m_CompletedCount++;

                            autoCompleteBoxQueryDept.ItemsSource = e.Result;
                            autoCompleteBoxQueryDept.ItemFilter = DeptFilter;
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
                referenceClient.GetDepartmentListInfoAsync();
                referenceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        public bool PathFilter(string strFilter, object item)
        {
            CP_ClinicalPathList info = (CP_ClinicalPathList)item;
            return ((info.QueryName.StartsWith(strFilter)) || (info.QueryName.Contains(strFilter)));
        }
        /// <summary>
        /// INIT路径
        /// </summary>
        private void InitComboBoxPath()
        {
            try
            {
                YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                referenceClient.GetClinicalPathListInfoCompleted += (obj, ea) =>
                {
                    if (ea.Error == null)
                    {
                        m_CompletedCount++;
                        autoCompleteBoxQueryPath.ItemsSource = ea.Result.ToList();
                        autoCompleteBoxQueryPath.ItemFilter = PathFilter;
                    }
                };
                referenceClient.GetClinicalPathListInfoAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 处理并显示数据（后台动态添加gridview列，并显示数据）
        /// </summary>
        private void GetPathCut(String ljmc, String ksdm)
        {
            radBusyIndicator.IsBusy = true;
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.PathCutTotalCompleted +=
            (obj, e) =>
            {
                try
                {
                    radBusyIndicator.IsBusy = false;

                    if (e.Error == null)
                    {
                        GridViewPathCut.ItemsSource = null;         //清空数据
                        GridViewPathCut.Columns.Clear();            //清行

                        if (e.Result.hashObjList.Count != 0 && e.Result.pathCutQuery.Count != 0)
                        {
                            ObservableCollection<Dictionary<String, Object>> list = e.Result.hashObjList;
                            List<CP_PathCutQuery> pathcut = e.Result.pathCutQuery.ToList();
                            GridViewPathCut.AutoGenerateColumns = false;

                            GridViewPathCut.Columns.Add(new GridViewDataColumn { Header = "路径名", DataMemberBinding = new Binding("Ljmc"), UniqueName = "Ljmc" , Width = 200 });         //固有列
                            GridViewPathCut.Columns.Add(new GridViewDataColumn { Header = "科室", DataMemberBinding = new Binding("deptName"), UniqueName = "deptName" , Width = 100  });
                            GridViewPathCut.Columns.Add(new GridViewDataColumn { Header = "路径状态", DataMemberBinding = new Binding("Yxjl"), UniqueName = "Yxjl", Width = 50 });
                            GridViewPathCut.Columns.Add(new GridViewDataColumn { Header = "科室代码", DataMemberBinding = new Binding("Syks"), UniqueName = "Syks", IsVisible = false });//固有列
                            GridViewPathCut.Columns.Add(new GridViewDataColumn { Header = "变异总数", DataMemberBinding = new Binding("AllCount") , Width = 80 });   //固有列

                            GridViewPathCut.Columns.Add(new GridViewDataColumn { Header = "路径代码", DataMemberBinding = new Binding("Ljdm"), UniqueName = "Ljdm" , Width = 80 });//8.2 由于按钮取消，则用列代替

                            foreach (CP_PathCutQuery path in pathcut)                                                                                   //遍历表头
                            {
                                GridViewPathCut.Columns.Add(new GridViewDataColumn { Header = path.Bymc, DataMemberBinding = new Binding(path.Bydm) , Width = 65 });//添加列
                            }

                            //GridViewColumn btnCol = new GridViewDataColumn { Header = "裁剪" };                 //按钮行
                            //string xaml = @"<DataTemplate  xmlns=""http://schemas.microsoft.com/winfx/2006/xaml/presentation""  xmlns:telerik=""http://schemas.telerik.com/2008/xaml/presentation"">
                            //                       <telerik:RadButton Content=""配置"" Tag=""{Binding Ljdm}"" /></DataTemplate>";
                            //btnCol.CellTemplate = (DataTemplate)XamlReader.Load(xaml);              //加按钮
                            //GridViewPathCut.Columns.Add(btnCol);

                            GridViewPathCut.ItemsSource = CalcEnumer(list, pathcut).ToDataSource();     //转换出数据库中的数据
                        }
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                }
                catch (Exception ex)
                {
                    YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            };
            client.PathCutTotalAsync(ljmc, ksdm);
            client.CloseAsync();
        }
        ///<summary>
        /// 动态创建非实体类（字典）（调用toDataSource类转换）
        ///</summary>
        public IEnumerable<IDictionary> CalcEnumer(ObservableCollection<Dictionary<String, Object>> data, List<CP_PathCutQuery> pathcut)
        {
            for (int i = 0; i < data.Count; i++)
            {
                var dict = new Dictionary<string, object>();

                dict["Ljmc"] = data[i]["LJMC"];                  //固有属性（表头）
                dict["AllCount"] = data[i]["ALLCOUNT"];          //固有属性（表头）
                dict["Yxjl"] = data[i]["YXJL"];                  //固有属性（表头）
                dict["Ljdm"] = data[i]["LJDM"];                  //按钮行（表头）
                dict["Syks"] = data[i]["SYKS"];                  //固有属性（表头）
                dict["deptName"] = data[i]["DEPTNAME"];          //固有属性（表头）

                foreach (CP_PathCutQuery path in pathcut)
                {
                    dict[path.Bydm] = data[i][path.Bydm];        //添加属性（表头）
                }
                yield return dict;
            }
        }
        /// <summary>
        /// 检查规范
        /// </summary>
        private bool Check()
        {
            if (this.textBoxPathName.Text.Trim() == string.Empty)
            {
                PublicMethod.RadAlterBox("请输入路径名称", "提示");
                this.textBoxPathName.Focus();
                return false;
            }
            if (this.textBoxPathName.Text.Trim().Length >= 60)
            {
                PublicMethod.RadAlterBox("路径名称长度不能超过30位","提示");
                this.textBoxPathName.Focus();
                return false;
            }
            if (this.radComboBoxStatus.SelectedValue == null)
            {
                PublicMethod.RadAlterBox("请选择路径状态", "提示");
                this.radComboBoxStatus.Focus();
                return false;
            }
            else if (this.textBoxDept.Text.Trim() == string.Empty)
            {
                PublicMethod.RadAlterBox("请选择科室", "提示");
                this.textBoxDept.Focus();
                return false;
            }
            return true;
        }


        void mess_PageClosedEvent(object sender, bool e)
        {
            try
            {
                if (e == true)
                {
                    YidanEHRDataServiceClient client = PublicMethod.YidanClient;

                    client.InsertCP_ClinicalPathCompleted +=
                    (obj, ea) =>
                    {
                        try
                        {
                            if (ea.Error == null)
                            {
                                if (ea.Result == "0")
                                {
                                    String tishi = "路径名称【" + textBoxPathName.Text + "】已经存在,请修改";
                                    PublicMethod.RadAlterBox(tishi, "提示");
                                }
                                else
                                {
                                    newCp = new CP_ClinicalPathList();
                                    newCp.Ljdm = ea.Result;
                                    newCp.Name = textBoxPathName.Text;

                                    InsertWorkFlow();
                                }
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        }
                        catch (Exception ex)
                        {
                            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                        }
                    };
                    String bzdm = Guid.NewGuid().ToString();
                    client.InsertCP_ClinicalPathAsync(this.textBoxPathName.Text, this.textBoxPathName.Text, (double)radNumericUpDownInDays.Value,
                    (double)radNumericUpDownAvgFee.Value, (double)radNumericUpDownVersion.Value, string.Empty, (int)radComboBoxStatus.SelectedValue, oldCp.Syks, bzdm, oldCp.Ljdm, "Lj");
                    client.CloseAsync();

                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 开始裁剪
        /// </summary>
        private void beginCut(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                YidanEHRDataServiceClient client = PublicMethod.YidanClient;

                client.InsertCP_ClinicalPathCompleted +=
                (obj, ea) =>
                {
                    try
                    {
                        if (ea.Error == null)
                        {
                            if (ea.Result == "0")
                            {
                                String tishi = "路径名称【" + textBoxPathName.Text + "】已经存在,请修改";
                                PublicMethod.RadAlterBox(tishi, "提示");
                            }
                            else
                            {
                                newCp = new CP_ClinicalPathList();
                                newCp.Ljdm = ea.Result;
                                newCp.Name = textBoxPathName.Text;

                                InsertWorkFlow();
                            }
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(ea.Error);
                        }
                    }
                    catch (Exception ex)
                    {
                        YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                    }
                };
                String bzdm = Guid.NewGuid().ToString();
                client.InsertCP_ClinicalPathAsync(this.textBoxPathName.Text, this.textBoxPathName.Text, (double)radNumericUpDownInDays.Value,
                (double)radNumericUpDownAvgFee.Value, (double)radNumericUpDownVersion.Value, string.Empty, (int)radComboBoxStatus.SelectedValue, oldCp.Syks, bzdm, oldCp.Ljdm, "Lj");
                client.CloseAsync();
            }
        }
        /// <summary>
        /// 插入节点和节点信息
        /// </summary>
        private void InsertWorkFlow()
        {
            //if (path.YxjlId == (int)PathShStatus.Review || path.YxjlId == (int)PathShStatus.Dc)           //暂不需要考虑是否审核问题
            //    pathNode.IsEditEnable = false;
            //else
            //    pathNode.IsEditEnable = true;         

            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.CutXMLCompleted +=
            (obj, e) =>
            {
                try
                {
                    if (e.Error == null)
                    {
                        newCp.WorkFlowXML = e.Result.Xml;
                        pathCutEdit = e.Result.CP_AdviceGroupPathCutList.ToList();
                        RWPathCutEdit pathCut = new RWPathCutEdit(newCp, pathCutEdit);         //跳到页面
                        pathCut.IsEditEnable = true;
                        pathCut.ShowDialog();

                        ViewState();
                        InitComboBoxPath();
                        GetPathCut("", "");
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                }
                catch (Exception ex)
                {
                    YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            };

            if (oldCp.WorkFlowXML == "" || oldCp.WorkFlowXML == null)                  //预防没XML情况
            {
                PublicMethod.RadAlterBox("该路径没有设计治疗方案，无法裁剪", "警告");
                return;
            }

            client.CutXMLAsync(oldCp.WorkFlowXML, newCp.Ljdm, newCp.Name, oldCp.Ljdm, oldCp.Name);
            client.CloseAsync();
        }
        #endregion
        #region 变量
        /// <summary>
        /// 裁剪路径
        /// </summary>
        private CP_ClinicalPathList oldCp;
        /// <summary>
        /// 新路径
        /// </summary>
        private CP_ClinicalPathList newCp;
        /// <summary>
        /// 裁剪医嘱
        /// </summary>
        private List<CP_AdviceGroupPathCut> pathCutEdit;
        /// <summary>
        /// 初始化的计数器
        /// </summary>
        private int m_CompletedCount = 0;
        #endregion
    }
}
