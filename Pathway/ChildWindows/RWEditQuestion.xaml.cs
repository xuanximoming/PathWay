using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Collections.ObjectModel;
using YidanEHRApplication.Models;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Views.ChildWindows
{
    /// <summary>
    /// Interaction logic for ChildWindowEditQuestion.xaml
    /// </summary>
    public partial class RWEditQuestion
    {
        List<QCCenterDataType> lstQC = new List<QCCenterDataType>();
        int operateType = 0; //生成为题的内容

        public RWEditQuestion(List<QCCenterDataType> lst, int Type, string title)
        {
            InitializeComponent();
            //初始化数据源
            lstQC = lst;
            operateType = Type;
            txtTitle.Text = title;
        }

        private void txtContent_TextChanged(object sender, TextChangedEventArgs e)
        {

            QCCenterDataType cp = (QCCenterDataType)rgvDate.SelectedItem;
            if (cp == null) return;
            //if (txtContent.Text.Trim() != "")
            //{
                cp.QContent = txtContent.Text.Trim();
            //}
            //else
            //{
            //    PublicMethod.RadAlterBox("问题内容不能为空", "提示");
            //    return;
            //}

            //rgvDate.CurrentItem = rgvDate.SelectedItem;
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        { try{
            rgvDate.ItemsSource = lstQC;//绑定数据
             }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }

        private void rgvDate_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            //两种方法实现绑定
            //txtContent.DataContext = (QCCenterDataType)rgvDate.SelectedItem;//后台给txtbox绑定数据源或前台绑定DataContext="{Binding SelectedItem, ElementName=rgvDate}" 
        }

        #region 提交问题
        /// <summary>
        /// 提交问题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnSubmit_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (txtContent.Text.Trim() == "")
                {
                    PublicMethod.RadAlterBox("问题内容不能为空", "提示");
                    return;
                }
                 
                ObservableCollection<CP_QCProblem> lstSumbit = new ObservableCollection<CP_QCProblem>();
                //获取问题
                foreach (QCCenterDataType cpB in lstQC)
                {
                    CP_QCProblem cpQ = new CP_QCProblem();
                    cpQ.Syxh = Convert.ToInt32(cpB.Syxh);
                    cpQ.Wtzt = 0;
                    cpQ.Ljdm = cpB.Ljdm;
                    cpQ.Zrys = cpB.Ysdm;
                    cpQ.Djry = Global.LogInEmployee.Zgdm;
                    cpQ.Wtnr = cpB.QContent;
                    lstSumbit.Add(cpQ);
                }

                //提交问题
                if (lstSumbit.Count > 0)
                {
                    YidanEHRDataServiceClient AddQuestionCenterClient = PublicMethod.YidanClient;
                    AddQuestionCenterClient.AddQuestionCenterCompleted +=
                  (obj, ea) =>
                  {
                      if (ea.Error == null)
                      {   //提交完成提示
                          //this.Close();
                          PublicMethod.RadAlterBox(ea.Result.ToString(), "提示");

                      }
                      else
                      {
                          PublicMethod.RadWaringBox(ea.Error);
                      }
                  };
                    AddQuestionCenterClient.AddQuestionCenterAsync(lstSumbit);
                    AddQuestionCenterClient.CloseAsync();
                }

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

     
        #endregion

        private void rgvDate_Loaded(object sender, RoutedEventArgs e)
        { try{
            #region 隐藏列表列
            //隐藏数据列
            switch (operateType)
            {
                case 1: //病历超时

                    for (int i = 5; i < 14; i++)
                    {
                        if (i != 5)
                            rgvDate.Columns[i].IsVisible = false;
                    }
                    break;

                case 2: //病人中途退出

                    for (int i = 5; i < 14; i++)
                    {
                        if (i != 6)
                            rgvDate.Columns[i].IsVisible = false;
                    }
                    break;

                case 3: //病人强制进入

                    for (int i = 5; i < 14; i++)
                    {
                        if (i != 7)
                            rgvDate.Columns[i].IsVisible = false;
                    }
                    break;

                case 4: //住院天数超标

                    for (int i = 5; i < 14; i++)
                    {
                        if (i != 8 && i != 9 && i != 10)
                            rgvDate.Columns[i].IsVisible = false;
                    }
                    break;

                case 5: //住院费用超标

                    for (int i = 5; i < 14; i++)
                    {
                        if (i != 11 && i != 12 && i != 12)
                            rgvDate.Columns[i].IsVisible = false;
                    }
                    break;
            }
            #endregion
             }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }

        private void rbtnClose_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        #region 绑定GridView
        //void BindGridViewData()
        //{
        //     QCCenterDataType cp = (QCCenterDataType)rgvDate.SelectedItem;
        //     if (cp == null)
        //    {
        //        //add by luff 20120813 
        //        rgvDate.ItemsSource = null;
        //        return;
        //    }
        //    YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
        //    //ServiceClient.GetCP_AdviceSuitDetailCompleted += new EventHandler<GetCP_AdviceSuitDetailCompletedEventArgs>(BindGridViewData);
        //    //ServiceClient.GetCP_AdviceSuitDetailAsync(String.Format(" and Ctyzxh={0} ", Ctyzxh));
        //    ServiceClient.CloseAsync();
        //}

        //void BindrgvDate(object sender, GetCP_AdviceSuitDetailCompletedEventArgs e)
        //{
        //    if (e.Error != null) PublicMethod.RadWaringBox(e.Error);
        //    else rgvDate.ItemsSource = e.Result.ToList();

        //}

        #endregion

    }
}
