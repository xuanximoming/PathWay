using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Navigation;
using Telerik.Windows.Controls;
using YidanEHRApplication.Models;
using Telerik.Windows.Controls.GridView;
using System.Collections.ObjectModel;
using YidanEHRApplication.Views.ChildWindows;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Views.ReportForms;

namespace YidanEHRApplication.Views
{

    public partial class InterventionCenter : Page
    {

        List<CheckBox> lstCheckBoxBeyondDays = new List<CheckBox>();

        public InterventionCenter()
        {
            InitializeComponent();
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        #region 函数
        /// <summary>
        /// 获取病历超时提示信息列表
        /// </summary>
        private void GetQCRecordTipList()
        {
            rbiQCRecordTipDay.IsBusy = true;
            YidanEHRDataServiceClient GetQCRecordTipListClient = PublicMethod.YidanClient;
            GetQCRecordTipListClient.GetQCRecordTipListCompleted +=
               (obj, ea) =>
               {
                   rbiQCRecordTipDay.IsBusy = false;
                   if (ea.Error == null)
                   {
                       rgvQCRecordTip.ItemsSource = ea.Result;

                   }
                   else
                   {
                       PublicMethod.RadWaringBox(ea.Error);
                   }
               };
            DateTime date = DateTime.Now;
            string startdate = "";
            string enddate = "";
            if (cmbQueryQCRecordTipDay.SelectedIndex > 0) //最近天数大于0，否则全部就开始时间和结束时间都为空
            {
                startdate = (date.AddDays(-cmbQueryQCRecordTipDay.SelectedIndex)).ToString("yyyy-MM-dd");
                enddate = date.ToString("yyyy-MM-dd");
            }
            GetQCRecordTipListClient.GetQCRecordTipListAsync(startdate, enddate);
            GetQCRecordTipListClient.CloseAsync();
        }
        /// <summary>
        /// 获取住院天数超标信息列表
        /// </summary>
        private void GetBeyondDaysList()
        {
            rbiBeyondDays.IsBusy = true;
            YidanEHRDataServiceClient GetBeyondDaysListClient = PublicMethod.YidanClient;
            GetBeyondDaysListClient.GetBeyondDaysListCompleted +=
                (obj, ea) =>
                {
                    rbiBeyondDays.IsBusy = false;
                    if (ea.Error == null)
                    {
                        rgvBeyondDays.ItemsSource = ea.Result;
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(ea.Error);
                    }
                };
            GetBeyondDaysListClient.GetBeyondDaysListAsync(cmbQueryBeyondDaysDay.SelectedIndex.ToString());
            GetBeyondDaysListClient.CloseAsync();
        }
        /// <summary>
        /// 获取住院费用超标信息列表
        /// </summary>
        private void GetBeyondFeeList()
        {
            rbiBeyondFee.IsBusy = true;
            YidanEHRDataServiceClient GetBeyondFeeListClient = PublicMethod.YidanClient;
            GetBeyondFeeListClient.GetBeyondFeeListCompleted +=
               (obj, ea) =>
               {
                   rbiBeyondFee.IsBusy = false;
                   if (ea.Error == null)
                   {
                       rgvBeyondFee.ItemsSource = ea.Result;
                   }
                   else
                   {
                       PublicMethod.RadWaringBox(ea.Error);
                   }
               };
            GetBeyondFeeListClient.GetBeyondFeeListAsync();
            GetBeyondFeeListClient.CloseAsync();
        }
        /// <summary>
        /// 获取病人牵制进入信息列表
        /// </summary>
        private void GetForceToPathList()
        {
            rbiForceToPath.IsBusy = true;
            YidanEHRDataServiceClient GetForceToPathListClient = PublicMethod.YidanClient;
            GetForceToPathListClient.GetForceToPathListCompleted +=
                 (obj, ea) =>
                 {
                     rbiForceToPath.IsBusy = false;
                     if (ea.Error == null)
                     {
                         rgvForceToPath.ItemsSource = ea.Result;
                     }
                     else
                     {
                         PublicMethod.RadWaringBox(ea.Error);
                     }
                 };
            DateTime date = DateTime.Now;
            string startdate = "";
            string enddate = "";
            if (cmbQueryForceToPathDay.SelectedIndex > 0) //最近天数大于0，否则全部就开始时间和结束时间都为空
            {
                //add by luff 2012-08-14
                startdate = (date.AddDays(-cmbQueryForceToPathDay.SelectedIndex)).ToString("yyyy-MM-dd");
                enddate = date.ToString("yyyy-MM-dd");
            }
            GetForceToPathListClient.GetForceToPathListAsync(startdate, enddate);
            GetForceToPathListClient.CloseAsync();
        }
        /// <summary>
        /// 获取病人中途退出信息列表
        /// </summary>
        private void GetPathExitList()
        {
            rbiPathExit.IsBusy = true;
            YidanEHRDataServiceClient GetPathExitListClient = PublicMethod.YidanClient;
            GetPathExitListClient.GetPathExitListCompleted +=
                 (obj, ea) =>
                 {
                     rbiPathExit.IsBusy = false;
                     if (ea.Error == null)
                     {
                         rgvPathExit.ItemsSource = ea.Result;
                     }
                     else
                     {
                         PublicMethod.RadWaringBox(ea.Error);
                     }
                 };
            DateTime date = DateTime.Now;
            string startdate = "";
            string enddate = "";
            if (cmbQueryPathExitDay.SelectedIndex > 0) //最近天数大于0，否则全部就开始时间和结束时间都为空
            {
                startdate = (date.AddDays(-cmbQueryPathExitDay.SelectedIndex)).ToString("yyyy-MM-dd");
                enddate = date.ToString("yyyy-MM-dd");
            }
            GetPathExitListClient.GetPathExitListAsync(startdate, enddate);
            GetPathExitListClient.CloseAsync();
        }
        /// <summary>
        /// 获取提出问题及问题审核列表
        /// </summary>
        private void GetQCProblemList()
        {
            YidanEHRDataServiceClient GetQCProblemListClient = PublicMethod.YidanClient;
            GetQCProblemListClient.GetQCProblemListCompleted +=
             (obj, e) =>
             {
                 if (e.Error == null)
                 {
                     rgvQuestion.ItemsSource = e.Result;
                 }
                 else
                 {
                     PublicMethod.RadWaringBox(e.Error);
                 }
             };

            GetQCProblemListClient.GetQCProblemListAsync(cmbQueryState.SelectedIndex, cmbQueryDays.SelectedIndex.ToString(), 0);
            GetQCProblemListClient.CloseAsync();
        }

        /// <summary>
        /// ZM 8.9 质控中心统一获值端
        /// </summary>
        private void GetData()
        {
            rbiBeyondDays.IsBusy = true;
            rbiBeyondFee.IsBusy = true;
            rbiQCRecordTipDay.IsBusy = true;
            rbiPathExit.IsBusy = true;
            rbiQCRecordTipDay.IsBusy = true;
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.GetInterventionCenterInfoCompleted +=
            (obj, e) =>
            {
                try
                {
                    rbiBeyondDays.IsBusy = false;
                    rbiBeyondFee.IsBusy = false;
                    rbiForceToPath.IsBusy = false;
                    rbiPathExit.IsBusy = false;
                    rbiQCRecordTipDay.IsBusy = false;
                    if (e.Error == null)
                    {
                        rgvBeyondDays.ItemsSource = e.Result.CP_BeyondDaysList.ToList();
                        rgvBeyondFee.ItemsSource = e.Result.CP_BeyondFeeList.ToList();
                        rgvForceToPath.ItemsSource = e.Result.CP_ForceToPathList.ToList();
                        rgvPathExit.ItemsSource = e.Result.CP_PathExitList.ToList();
                        rgvQuestion.ItemsSource = e.Result.CP_QCProblemList.ToList();
                        rgvQCRecordTip.ItemsSource = e.Result.CP_QCRecordTipList.ToList();
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }

                }
                catch (Exception ex)
                {
                    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            };

            DateTime date = DateTime.Now;
            string ForceToPathstartdate = "";
            string ForceToPathenddate = "";
            if (cmbQueryForceToPathDay.SelectedIndex > 0) //最近天数大于0，否则全部就开始时间和结束时间都为空
            {
                ForceToPathstartdate = (date.AddDays(-cmbQueryForceToPathDay.SelectedIndex)).ToString("yyyy-MM-dd");
                ForceToPathenddate = date.ToString("yyyy-MM-dd");
            }

            string PathExitstartdate = "";
            string PathExitenddate = "";
            if (cmbQueryPathExitDay.SelectedIndex > 0) //最近天数大于0，否则全部就开始时间和结束时间都为空
            {
                PathExitstartdate = (date.AddDays(-cmbQueryPathExitDay.SelectedIndex)).ToString("yyyy-MM-dd");
                PathExitenddate = date.ToString("yyyy-MM-dd");
            }

            string QCRecordTipstartdate = "";
            string QCRecordTipenddate = "";
            if (cmbQueryQCRecordTipDay.SelectedIndex > 0) //最近天数大于0，否则全部就开始时间和结束时间都为空
            {
                QCRecordTipstartdate = (date.AddDays(-cmbQueryQCRecordTipDay.SelectedIndex)).ToString("yyyy-MM-dd");
                QCRecordTipenddate = date.ToString("yyyy-MM-dd");
            }
            client.GetInterventionCenterInfoAsync(cmbQueryBeyondDaysDay.SelectedIndex.ToString(), ForceToPathstartdate, ForceToPathenddate,
                PathExitstartdate, PathExitenddate, cmbQueryState.SelectedIndex, cmbQueryDays.SelectedIndex.ToString(), 0, QCRecordTipstartdate, QCRecordTipenddate);
            client.CloseAsync();

        }
        #endregion

        #region 审核问题
        private void AuditQuestion()
        {
            if (txtQuestionContent.Text.Trim() == "")
            {
                PublicMethod.RadAlterBox("问题内容不能为空!", "提示");
                return;
            }

            rbiQuestion.IsBusy = true;
            YidanEHRDataServiceClient AuditQuestionClient = PublicMethod.YidanClient;

            AuditQuestionClient.AuditQuestionCompleted +=
             (obj, e) =>
             {
                 rbiQuestion.IsBusy = false;
                 if (e.Error == null)
                 {
                     if (e.Result.ToString() == "操作失败!")
                     {
                         PublicMethod.RadAlterBox("审核失败！", "提示");
                     }
                     else
                     {
                         //更新数据源
                         CP_QCProblem cp = (CP_QCProblem)rgvQuestion.SelectedItem;
                         if (cmbAudit.SelectedIndex == 1)//已审核
                         {
                             cp.Wtzt = 4;
                             cp.Qczt = "完成";
                             cp.Shzt = "已审核";
                             cp.Wtnr = txtQuestionContent.Text;
                             cp.Shnr = txtAuditContent.Text;
                             cp.Shrq = e.Result.ToString();
                             cp.Shry = Global.LogInEmployee.Zgdm;
                             cp.Shryxm = Global.LogInEmployee.Name;

                         }
                         else if (cmbAudit.SelectedIndex == 2)//已作废
                         {
                             cp.Wtzt = 4;
                             cp.Qczt = "完成";
                             cp.Shzt = "已作废";
                             cp.Wtnr = txtQuestionContent.Text;
                             cp.Shnr = txtAuditContent.Text;
                             cp.Zfrq = e.Result.ToString();
                             cp.Zfry = Global.LogInEmployee.Zgdm;
                             cp.Zfryxm = Global.LogInEmployee.Name;
                         }
                         else
                         {
                             cp.Wtzt = 2;
                             cp.Qczt = "挂起";
                             cp.Wtnr = txtQuestionContent.Text;
                             cp.Djry = Global.LogInEmployee.Zgdm;
                             cp.Djryxm = Global.LogInEmployee.Name;
                             cp.Djrq = e.Result.ToString();
                         }
                         rgvQuestion.CurrentItem = rgvQuestion.SelectedItem;

                         PublicMethod.RadAlterBox("审核成功！", "提示");
                     }
                 }
                 else
                 {
                     PublicMethod.RadWaringBox(e.Error);
                 }
             };
            CP_QCProblem cp2 = new CP_QCProblem();
            cp2.Wtxh = Convert.ToInt32(txtQuestionContent.Tag);
            cp2.Wtnr = txtQuestionContent.Text.Trim();
            cp2.Djry = Global.LogInEmployee.Zgdm;
            cp2.Shnr = txtAuditContent.Text.Trim();
            if (cmbAudit.SelectedIndex == 1)
            {
                cp2.Shry = Global.LogInEmployee.Zgdm;
                cp2.Shryxm = Global.LogInEmployee.Name;
            }
            else
            {
                cp2.Zfry = Global.LogInEmployee.Zgdm;
                cp2.Zfryxm = Global.LogInEmployee.Name;
            }

            AuditQuestionClient.AuditQuestionAsync(cp2, cmbAudit.SelectedIndex);
            AuditQuestionClient.CloseAsync();
        }


        #endregion

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //GetQCProblemList();//加载审核问题列表
                //GetQCRecordTipList();//加载病历超时信息
                //GetPathExitList();//加载病人中途退出信息
                //GetForceToPathList();//加载病人牵制进入信息
                //GetBeyondDaysList();//加载病人住院天数超标信息
                //GetBeyondFeeList();//加载病人住院费用超标信息
                GetData();
                rbtnSumbit.IsEnabled = false;
                rtviCenter.TileState = TileViewItemState.Maximized;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        //查询
        private void rbtnQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender == null) return;
                #region 查询
                switch (((Button)sender).Name.ToString())
                {
                    case "rbtnQueryQCRecord":   //病历超时
                        GetQCRecordTipList();
                        break;
                    case "rbtnQueryBeyondDays": //住院天数超标
                        GetBeyondDaysList();
                        break;
                    case "rbtnUpdateBeyondFee": //住院费用超标
                        GetBeyondFeeList();
                        break;
                    case "rbtnQueryForceToPath"://牵制进入
                        GetForceToPathList();
                        break;
                    case "rbtnQueryPathExit"://中途退出
                        GetPathExitList();
                        break;
                    case "rbtnQueryQuestion": //查询提问的问题 
                        //初始化部分控件                   
                        txtQuestionContent.Text = "";
                        txtQuestionContent.Tag = "";
                        txtAuditContent.Text = "";
                        txtAnswerContent.Text = "";
                        rbtnSumbit.IsEnabled = false;
                        cmbAudit.IsEnabled = false;
                        txtQuestionContent.IsReadOnly = true;
                        txtAuditContent.IsReadOnly = true;
                        GetQCProblemList();
                        break;
                }
                #endregion

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }



        private void rgvBeyondDays_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
            CP_BeyondDays t = (CP_BeyondDays)e.DataElement;
            lstCheckBoxBeyondDays = (List<CheckBox>)(rgvBeyondDays.ChildrenOfType<CheckBox>().ToList());
            if (lstCheckBoxBeyondDays.Count > 1)
            {
                lstCheckBoxBeyondDays[lstCheckBoxBeyondDays.Count - 1].IsChecked = t.IsCheck;
                lstCheckBoxBeyondDays[lstCheckBoxBeyondDays.Count - 1].Tag = t.ID;
            }

        }

        #region 自动生成问题
        //生成住院天数超标问题
        private void rbtnAutoCreateBeyondDays_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                List<QCCenterDataType> lst = new List<QCCenterDataType>();

                //遍历住院天数超标记录
                foreach (object obj in rgvBeyondDays.SelectedItems)
                {
                    CP_BeyondDays cpB = (obj as CP_BeyondDays);
                    QCCenterDataType qcc = new QCCenterDataType();

                    qcc.GUID = cpB.ID;
                    qcc.Syxh = cpB.Syxh;
                    qcc.Hzxm = cpB.Hzxm;
                    qcc.Jrsj = cpB.Jrsj;
                    qcc.Ljdm = cpB.Ljdm;
                    qcc.Ljmc = cpB.Ljmc;
                    qcc.Ysdm = cpB.Ysdm;
                    qcc.Ysxm = cpB.Ysxm;

                    qcc.Blts = "";
                    qcc.Tcrq = "";
                    qcc.Qzjryy = "";
                    qcc.Jcfy = "";
                    qcc.Sjfy = "";
                    qcc.Ccfy = "";
                    qcc.Zgts = cpB.Zgts;
                    qcc.Sjts = cpB.Sjts;
                    qcc.Ccts = cpB.Ccts;
                    qcc.QContent = string.Format("标准住院天数:{0},实际住院天数:{1}，住院超出天数:{2}，请说明住院天数超出标准住院天数因素？",
                                                  cpB.Zgts, cpB.Sjts, cpB.Ccts);
                    lst.Add(qcc);
                }
                if (lst.Count > 0)
                {
                    RWEditQuestion Win = new RWEditQuestion(lst, 4, "病人住院天数超标信息表:");
                    Win.ShowDialog();
                }
                else
                {
                    PublicMethod.RadAlterBox("请选要生成提交记录！", "提示");
                }

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }



        }

        //生成病历超时问题
        private void rbtnAutoCreateQCRecord_Click(object sender, RoutedEventArgs e)
        {
            try
            {



                List<QCCenterDataType> lst = new List<QCCenterDataType>();

                //遍历病历超时记录
                foreach (object obj in rgvQCRecordTip.SelectedItems)
                {
                    CP_QCRecordTip cpB = (obj as CP_QCRecordTip);
                    QCCenterDataType qcc = new QCCenterDataType();

                    qcc.GUID = cpB.ID;
                    qcc.Syxh = cpB.Syxh;
                    qcc.Hzxm = cpB.Hzxm;
                    qcc.Jrsj = cpB.Jrsj;
                    qcc.Ljdm = cpB.Ljdm;
                    qcc.Ljmc = cpB.Ljmc;
                    qcc.Ysdm = cpB.Ysdm;
                    qcc.Ysxm = cpB.Ysxm;

                    qcc.Blts = cpB.Blts;
                    qcc.Tcrq = "";
                    qcc.Qzjryy = "";
                    qcc.Jcfy = "";
                    qcc.Sjfy = "";
                    qcc.Ccfy = "";
                    qcc.Zgts = "";
                    qcc.Sjts = "";
                    qcc.Ccts = "";
                    qcc.QContent = string.Format("病历超时提示:{0}\n对上述问题请说明原因？", cpB.Blts);
                    lst.Add(qcc);
                }
                if (lst.Count > 0)
                {
                    RWEditQuestion Win = new RWEditQuestion(lst, 1, "病人病历超时信息表:");
                    Win.ShowDialog();
                }
                else
                {
                    PublicMethod.RadAlterBox("请选要生成提交记录！", "提示");
                }


            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }


        }

        //生成病人中途退出问题
        private void rbtnAutoCreatePathExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                List<QCCenterDataType> lst = new List<QCCenterDataType>();

                //遍历病人中途退出记录
                foreach (object obj in rgvPathExit.SelectedItems)
                {
                    CP_PathExit cpB = (obj as CP_PathExit);
                    QCCenterDataType qcc = new QCCenterDataType();

                    qcc.GUID = cpB.ID;
                    qcc.Syxh = cpB.Syxh;
                    qcc.Hzxm = cpB.Hzxm;
                    qcc.Jrsj = cpB.Jrsj;
                    qcc.Ljdm = cpB.Ljdm;
                    qcc.Ljmc = cpB.Ljmc;
                    qcc.Ysdm = cpB.Ysdm;
                    qcc.Ysxm = cpB.Ysxm;

                    qcc.Blts = "";
                    qcc.Tcrq = cpB.Tcsj;
                    qcc.Qzjryy = "";
                    qcc.Jcfy = "";
                    qcc.Sjfy = "";
                    qcc.Ccfy = "";
                    qcc.Zgts = "";
                    qcc.Sjts = "";
                    qcc.Ccts = "";
                    qcc.QContent = "请说明病人中途退出的原因？";
                    lst.Add(qcc);
                }
                if (lst.Count > 0)
                {
                    RWEditQuestion Win = new RWEditQuestion(lst, 2, "病人中途退出信息表:");
                    Win.ShowDialog();
                }
                else
                {
                    PublicMethod.RadAlterBox("请选要生成提交记录！", "提示");
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }


        }

        //生成住院费用超标问题
        private void rbtnAutoCreateBeyondFee_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                List<QCCenterDataType> lst = new List<QCCenterDataType>();

                //遍历住院费用超标记录
                foreach (object obj in rgvBeyondFee.SelectedItems)
                {
                    CP_BeyondFee cpB = (obj as CP_BeyondFee);
                    QCCenterDataType qcc = new QCCenterDataType();

                    qcc.GUID = cpB.ID;
                    qcc.Syxh = cpB.Syxh;
                    qcc.Hzxm = cpB.Hzxm;
                    qcc.Jrsj = cpB.Jrsj;
                    qcc.Ljdm = cpB.Ljdm;
                    qcc.Ljmc = cpB.Ljmc;
                    qcc.Ysdm = cpB.Ysdm;
                    qcc.Ysxm = cpB.Ysxm;

                    qcc.Blts = "";
                    qcc.Tcrq = "";
                    qcc.Qzjryy = "";
                    qcc.Jcfy = cpB.Jcfy;
                    qcc.Sjfy = cpB.Sjfy;
                    qcc.Ccfy = cpB.Ccfy;
                    qcc.Zgts = "";
                    qcc.Sjts = "";
                    qcc.Ccts = "";
                    qcc.QContent = string.Format("标准住院费用(￥):{0},实际住院费用(￥):{1}，住院超出费用(￥):{2}，请说明住院费用超出标准住院费用因素？",
                                                  cpB.Jcfy, cpB.Sjfy, cpB.Ccfy);
                    lst.Add(qcc);
                }
                if (lst.Count > 0)
                {
                    RWEditQuestion Win = new RWEditQuestion(lst, 5, "病人住院费用超标信息表:");
                    Win.ShowDialog();
                }
                else
                {
                    PublicMethod.RadAlterBox("请选要生成提交记录！", "提示");
                }

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        //生成强制进入问题
        private void rbtnAutoCreateForceToPath_Click(object sender, RoutedEventArgs e)
        {
            try
            {



                List<QCCenterDataType> lst = new List<QCCenterDataType>();

                //遍历强制进入记录
                foreach (object obj in rgvForceToPath.SelectedItems)
                {
                    CP_ForceToPath cpB = (obj as CP_ForceToPath);
                    QCCenterDataType qcc = new QCCenterDataType();

                    qcc.GUID = cpB.ID;
                    qcc.Syxh = cpB.Syxh;
                    qcc.Hzxm = cpB.Hzxm;
                    qcc.Jrsj = cpB.Jrsj;
                    qcc.Ljdm = cpB.Ljdm;
                    qcc.Ljmc = cpB.Ljmc;
                    qcc.Ysdm = cpB.Ysdm;
                    qcc.Ysxm = cpB.Ysxm;

                    qcc.Blts = "";
                    qcc.Tcrq = "";
                    qcc.Qzjryy = cpB.Qzjryy;
                    qcc.Jcfy = "";
                    qcc.Sjfy = "";
                    qcc.Ccfy = "";
                    qcc.Zgts = "";
                    qcc.Sjts = "";
                    qcc.Ccts = "";
                    qcc.QContent = string.Format("病人牵制进入原因:{0}\n对上述原因请给予说明。", cpB.Qzjryy);
                    lst.Add(qcc);
                }
                if (lst.Count > 0)
                {
                    RWEditQuestion Win = new RWEditQuestion(lst, 3, "病人强制进入信息表:");
                    Win.ShowDialog();
                }
                else
                {
                    PublicMethod.RadAlterBox("请选要生成提交记录！", "提示");
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }


        }
        #endregion

        private void rgvQuestion_SelectionChanged(object sender, SelectionChangeEventArgs e)
        {
            try
            {
                //设置审核模块控件属性
                CP_QCProblem cp = (CP_QCProblem)rgvQuestion.SelectedItem;
                if (cp == null) return;
                if (cp.Wtnr != "")
                {
                    txtQuestionContent.Text = cp.Wtnr + " [" + cp.Djryxm + " " + cp.Djrq + "]\n";
                }
                else
                {
                    txtQuestionContent.Text = cp.Wtnr;
                }
                txtQuestionContent.Tag = cp.Wtxh;
                txtAuditContent.Text = cp.Shnr;
                txtAnswerContent.Text = cp.Dfnr;
                rbtnSumbit.IsEnabled = cp.Wtzt != 4;
                cmbAudit.IsEnabled = cp.Wtzt != 4;
                txtQuestionContent.IsReadOnly = cp.Wtzt == 4;
                txtAuditContent.IsReadOnly = cp.Wtzt == 4;
                if (cp.Shzt == "已作废") //作废
                {
                    cmbAudit.SelectedIndex = 2;
                }
                else if (cp.Shzt == "已审核") //已审核
                {
                    cmbAudit.SelectedIndex = 1;
                }
                else //未审核
                {
                    cmbAudit.SelectedIndex = 0;
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        /// <summary>
        /// 审核问题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rbtnSumbit_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                string strTip = "";

                if (cmbAudit.SelectedIndex == 0)
                {
                    strTip = "确认挂起该条问题吗？";
                }
                else if (cmbAudit.SelectedIndex == 1)
                {
                    strTip = "确认审核通过该条记录吗？";
                }
                else
                {
                    strTip = "确认作废该条记录吗？";
                }
                //DialogParameters parameters = new DialogParameters();
                //parameters.Content = String.Format("提示: {0}", strTip);
                //parameters.Header = "提示";
                //parameters.IconContent = null;
                //parameters.OkButtonContent = "确定";
                //parameters.CancelButtonContent = "取消";
                //parameters.Closed = OnDelAdviceGroupDetail;//***close处理***
                //RadWindow.Confirm(parameters);
                YidanPathWayMessageBox mess = new YidanPathWayMessageBox(strTip, "提示", YiDanMessageBoxButtons.YesNo);
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

                    AuditQuestion();

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
                    AuditQuestion();
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

        private void rtviCenter_TileStateChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //显示/隐藏审核模块控件
            if (rtviCenter.TileState == TileViewItemState.Maximized)
            {
                gridAudit.Visibility = Visibility.Visible;
            }
            else
            {
                gridAudit.Visibility = Visibility.Collapsed;
            }
        }

        /// <summary>
        /// 打印病历超时信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintCRecord_Click(object sender, RoutedEventArgs e)
        {
            try
            {



                DateTime date = DateTime.Now;
                string startdate = "";
                string enddate = "";
                if (cmbQueryQCRecordTipDay.SelectedIndex > 0) //最近天数大于0，否则全部就开始时间和结束时间都为空
                {
                    startdate = (date.AddDays(-cmbQueryQCRecordTipDay.SelectedIndex)).ToString("yyyy-MM-dd");
                    enddate = date.ToString("yyyy-MM-dd");
                }

                //RWPagePrint pageprint = new RWPagePrint();
                //pageprint.m_Title = "打印病历超时信息";
                //pageprint.m_TableTile = "病人编号,病人名称,入径日期,路径名称,床位医生,提示信息";
                //pageprint.m_TableBindCol = "Syxh,Hzxm,Jrsj,Ljmc,Ysxm,Blts";
                //pageprint.m_ColWidth = "2,2,2,4,3,5";
                //pageprint.m_ExecSQL = @"exec usp_CP_QCRecord @StartDate='" + startdate + "',@EndDate='" + enddate + "'";

                //pageprint.ShowDialog();

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }


        }

        /// <summary>
        /// 打印病人中途退出信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintPathExit_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                DateTime date = DateTime.Now;
                string startdate = "";
                string enddate = "";
                if (cmbQueryPathExitDay.SelectedIndex > 0) //最近天数大于0，否则全部就开始时间和结束时间都为空
                {
                    startdate = (date.AddDays(-cmbQueryPathExitDay.SelectedIndex)).ToString("yyyy-MM-dd");
                    enddate = date.ToString("yyyy-MM-dd");
                }

                //RWPagePrint pageprint = new RWPagePrint();
                //pageprint.m_Title = "打印病人中途退出信息";
                //pageprint.m_TableTile = "病人编号,病人名称,路径名称,床位医生,入径日期,退出日期";
                //pageprint.m_TableBindCol = "Syxh,Hzxm,Ljmc,Ysxm,Jrsj,Tcsj";
                //pageprint.m_ColWidth = "2,3,4,3,3,3";
                //pageprint.m_ExecSQL = @"exec usp_CP_PathExit @StartDate='" + startdate + "',@EndDate='" + enddate + "'";

                //pageprint.ShowDialog();

                RptPathExitListPrint pageprint = new RptPathExitListPrint();

                pageprint.m_BeginTime = startdate;
                pageprint.m_EndTime = enddate;

                pageprint.WindowState = WindowState.Maximized;
                pageprint.ShowDialog();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }


        }
        /// <summary>
        /// 打印病人强制入径信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintForceToPath_Click(object sender, RoutedEventArgs e)
        {
            try
            {


                DateTime date = DateTime.Now;
                string startdate = "";
                string enddate = "";
                if (cmbQueryForceToPathDay.SelectedIndex > 0) //最近天数大于0，否则全部就开始时间和结束时间都为空
                {
                    startdate = (date.AddDays(-cmbQueryForceToPathDay.SelectedIndex)).ToString("yyyy-MM-dd");
                    enddate = date.ToString("yyyy-MM-dd");
                }

                RWPagePrint pageprint = new RWPagePrint();
                pageprint.m_Title = "打印病人强制入径信息";
                pageprint.m_TableTile = "病人编号,病人名称,路径名称,床位医生,入径日期,强制入径原因";
                pageprint.m_TableBindCol = "Syxh,Name,Ljmc,Ysxm,Jrsj,Memo";
                pageprint.m_ColWidth = "2,3,4,2,3,4";
                pageprint.m_ExecSQL = @"exec usp_CP_ForceToPath @StartDate='" + startdate + "',@EndDate='" + enddate + "'";

                pageprint.ShowDialog();

                //RgvForceToPathPrint pageprint = new RgvForceToPathPrint();

                //pageprint.m_BeginTime = startdate;
                //pageprint.m_EndTime = enddate;

                //pageprint.WindowState = WindowState.Maximized;
                //pageprint.ShowDialog();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 打印病人住院天数超标信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintBeyondDays_Click(object sender, RoutedEventArgs e)
        {


            try
            {

                DateTime date = DateTime.Now;
                string startdate = "";
                string enddate = "";
                if (cmbQueryForceToPathDay.SelectedIndex > 0) //最近天数大于0，否则全部就开始时间和结束时间都为空
                {
                    startdate = (date.AddDays(-cmbQueryForceToPathDay.SelectedIndex)).ToString("yyyy-MM-dd");
                    enddate = date.ToString("yyyy-MM-dd");
                }
                //RWPagePrint pageprint = new RWPagePrint();
                //pageprint.m_Title = "打印病人住院天数超标信息";
                //pageprint.m_TableTile = "病人名称,路径名称,床位医生,入径日期,标准住院天数,实际住院天数,住院超标天数";
                //pageprint.m_TableBindCol = "Hzxm,Ljmc,Ysxm,Jrsj,Zgts,Sjts,Ccts";
                //pageprint.m_ColWidth = "3,4,2,3,2,2,2";
                //pageprint.m_ExecSQL = @"exec usp_CP_BeyondDays @Days='" + cmbQueryBeyondDaysDay.SelectedIndex.ToString() + "'";

                //pageprint.ShowDialog();
                RbiBeyondDaysRptPrint pageprint = new RbiBeyondDaysRptPrint();
                //pageprint.m_BeginTime = startdate;
                //pageprint.m_EndTime = enddate;
                pageprint.m_Days = cmbQueryForceToPathDay.SelectedIndex.ToString();

                pageprint.WindowState = WindowState.Maximized;
                pageprint.ShowDialog();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 打印病人住院天数超标信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnPrintBeyondFee_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                //RWPagePrint pageprint = new RWPagePrint();
                //pageprint.m_Title = "打印病人住院天数超标信息";
                //pageprint.m_TableTile = "病人名称,路径名称,床位医生,入径日期,标准费用,实际费用,超标费用";
                //pageprint.m_TableBindCol = "Hzxm,Ljmc,Ysxm,Jrsj,Jcfy,Sjfy,Ccfy";
                //pageprint.m_ColWidth = "3,4,2,3,2,2,2";
                //pageprint.m_ExecSQL = @"exec usp_CP_BeyondFee ";

                //pageprint.ShowDialog();

                DateTime date = DateTime.Now;
                string startdate = "";
                string enddate = "";
                if (cmbQueryForceToPathDay.SelectedIndex > 0)
                {
                    startdate = (date.AddDays(-cmbQueryForceToPathDay.SelectedIndex)).ToString("yyyy-MM-dd");
                    enddate = date.ToString("yyyy-MM-dd");
                }
                BeyondFeeRptPrint pageprint = new BeyondFeeRptPrint();
                pageprint.m_BeginTime = startdate;
                pageprint.m_EndTime = enddate;

                pageprint.WindowState = WindowState.Maximized;
                pageprint.ShowDialog();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

    }
}
