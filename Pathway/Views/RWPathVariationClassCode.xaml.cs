using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using YidanEHRApplication.DataService;
using Telerik.Windows.Controls;
using Telerik.Windows.Data;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Views
{
    public partial class RWPathVariationClassCode
    {
        /// <summary>
        /// GRIDVIEW里所有行中的checkBox,用tag属性区别（绑定ORDERGUID)
        /// </summary>
        ObservableCollection<CheckBox> m_GridCheckBox = new ObservableCollection<CheckBox>();

        public RWPathVariationClassCode()
        {
            InitializeComponent();
            LayoutRoot.MouseRightButtonDown += (sender, e) =>
            {
                e.Handled = true;
                //自己的菜单
            };
            Rigister();
            this.tbQuery.Focus();

        }

        private void Rigister()
        {
            tbQuery.KeyUp += new KeyEventHandler(tbQuery_KeyUp);
        }

        public void tbQuery_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RefreshRecords(tbQuery.Text);
            }
        }

        private void GetThirdCodeList()
        {
            //radBusyIndicator.IsBusy = true;
            YidanEHRDataServiceClient GetThirdCodeListClient = PublicMethod.YidanClient;
            GetThirdCodeListClient.GetThirdCodeListCompleted +=
                (obj, e) =>
                {
                    //radBusyIndicator.IsBusy = false;
                    if (e.Error == null)
                    {
                        //codeList = e.Result.ToList();
                        //cbxVariationType.SelectedValue = "Bydm";
                        //cbxVariationType.SelectedValuePath = "CodeGroup";
                        //cbxVariationType.ItemsSource = codeList;

                        cbxVariationType.ItemsSource = e.Result;
                        cbxVariationType.EmptyText = "选择归类码...";

                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }

                    RefreshRecords("");

                };
            GetThirdCodeListClient.GetThirdCodeListAsync("");
            GetThirdCodeListClient.CloseAsync();

        }




        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //this.DialogResult =false;
        }

        //创建变异编码
        //private void btnCreateCode_Click(object sender, RoutedEventArgs e)
        //{
        //    PathVariationEditCode CWin = new PathVariationEditCode();
        //    CWin.Show();
        //    if (CWin.DialogResult == true)
        //    {
        //        GetThirdCodeList();
        //        //QueryCode();
        //    }
        //}

        ///// <summary>
        ///// 获取所有编码列表
        ///// </summary>
        //private void QueryCode()
        //{
        //    YidanEHRDataServiceClient QueryClient = PublicMethod.YidanClient;
        //    QueryClient.GetDataPathVariationListCompleted +=
        //       new EventHandler<GetDataPathVariationListCompletedEventArgs>(QueryClient_GetDataPathVariationListCompleted);
        //    //QueryClient.GetDataPathVariationListAsync(0); //全部
        //    QueryClient.CloseAsync();

        //}

        //private void QueryClient_GetDataPathVariationListCompleted(object sender, GetDataPathVariationListCompletedEventArgs e)
        //{
        //    if (e.Error == null)
        //    {
        //        codeList= e.Result.ToList();

        //    }
        //    else
        //    {
        //        PublicMethod.RadWaringBox(e.Error);
        //    }
        //}


        /// <summary>
        /// 刷新
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                this.tbQuery.Text = "";
                RefreshRecords("");
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        /// <summary>
        /// 刷新需归类的编码列表
        /// </summary>
        private void RefreshRecords(string key)
        {
            radBusyIndicator.IsBusy = true;
            YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
            referenceClient.GetVariantRecordsCompleted +=
               (obj, e) =>
               {
                   radBusyIndicator.IsBusy = false;
                   if (e.Error == null)
                   {
                       var view = new QueryableCollectionView(e.Result.ToList());
                       this.GridViewVariantRecords.ItemsSource = view;
                       this.radDataPager.DataContext = radDataPager;

                   }
                   else
                   {
                       PublicMethod.RadWaringBox(e.Error);
                   }
               };
            referenceClient.GetVariantRecordsAsync(key);
            referenceClient.CloseAsync();
        }




        /// <summary>
        /// 编码归类
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnClassCode_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (cbxVariationType.SelectedIndex < 0)
                {
                    PublicMethod.RadAlterBoxRe("请选择归类编码!", "提示", cbxVariationType);
                    isLoad = false;
                    return;
                }
                if (GridViewVariantRecords.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择一条记录!", "提示");
                    return;
                }

                //DialogParameters parameters = new DialogParameters();
                //parameters.Content = String.Format("提示: {0}", "确认对选中的记录编码归类吗？");
                //parameters.Header = "提示";
                //parameters.IconContent = null;
                //parameters.OkButtonContent = "确定";
                //parameters.CancelButtonContent = "取消";
                //parameters.Closed = OnDelAdviceGroupDetail;//***close处理***
                //RadWindow.Confirm(parameters);

                YidanPathWayMessageBox mess = new YidanPathWayMessageBox("确认对选中的记录编码归类吗？", "提示", YiDanMessageBoxButtons.YesNo);
                mess.ShowDialog();
                mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
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

                    ObservableCollection<int> updateListID = new ObservableCollection<int>();

                    foreach (CheckBox checkbox in m_GridCheckBox)
                    {
                        if (checkbox.IsChecked == true)
                        {
                            updateListID.Add(Convert.ToInt32(checkbox.Tag));
                        }
                    }

                    string code = ((CP_PathVariation)cbxVariationType.SelectedItem).Bydm;
                    //string code = cbxVariationType.SelectedValue.ToString();

                    YidanEHRDataServiceClient classClient = PublicMethod.YidanClient;
                    classClient.UpdateVariantRecordsCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                RefreshRecords(""); //刷新需归类的编码列表
                                PublicMethod.RadAlterBox("编码归类完成.", "提示");
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    classClient.UpdateVariantRecordsAsync(updateListID, code);
                    classClient.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        void OnDelAdviceGroupDetail(object sender, WindowClosedEventArgs e)
        {
            if (e.DialogResult == true)
            {
                try
                {
                    ObservableCollection<int> updateListID = new ObservableCollection<int>();

                    foreach (CheckBox checkbox in m_GridCheckBox)
                    {
                        if (checkbox.IsChecked == true)
                        {
                            updateListID.Add(Convert.ToInt32(checkbox.Tag));
                        }
                    }

                    string code = ((CP_PathVariation)cbxVariationType.SelectedItem).Bydm;
                    //string code = cbxVariationType.SelectedValue.ToString();

                    YidanEHRDataServiceClient classClient = PublicMethod.YidanClient;
                    classClient.UpdateVariantRecordsCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                RefreshRecords(""); //刷新需归类的编码列表
                                PublicMethod.RadAlterBox("编码归类完成.", "提示");
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    classClient.UpdateVariantRecordsAsync(updateListID, code);
                    classClient.CloseAsync();

                }
                catch (Exception ex)
                {
                    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            }
            else
            {

            }
        }



        public bool isLoad = true;
        private void ChildWindow_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isLoad)
                {
                    isLoad = true;
                    return;
                }
                GetThirdCodeList();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

            // RefreshRecords();
            //QueryCode();
        }

        private void checkBoxAll_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                if (this.GridViewVariantRecords.ItemsSource == null)
                    return;

                foreach (CheckBox checkbox in m_GridCheckBox)
                {
                    checkbox.IsChecked = true;
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void checkBoxAll_Unchecked(object sender, RoutedEventArgs e)
        {
            if (this.GridViewVariantRecords.ItemsSource == null)
                return;
            foreach (CheckBox check in m_GridCheckBox)
            {
                check.IsChecked = false;
            }
        }

        private void CheckBox_Loaded(object sender, RoutedEventArgs e)
        {
            m_GridCheckBox.Add(sender as CheckBox);
        }

        private void ChildWindow_Unloaded(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            RefreshRecords(tbQuery.Text.Replace(" ", ""));
        }

        private void btnReset_Click(object sender, RoutedEventArgs e)
        {
            tbQuery.Text = string.Empty;
        }



        public MouseEventHandler tbQuery_MouseEnter { get; set; }
    }
}

