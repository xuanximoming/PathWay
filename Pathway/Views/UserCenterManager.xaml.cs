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
using YidanEHRApplication.Models;
using System.Text;
using Telerik.Windows.Controls;
using YidanEHRApplication.Controls;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Views
{
    public partial class UserCenterManager : Page
    {
        YidanEHRDataServiceClient serviceCon;
        #region 事件
        public UserCenterManager()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(UserCenterManager_Loaded);
        }
        void UserCenterManager_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                #region    分管医生  暂时注释掉
                //this.InpatientListControl1.DoctorID = Global.LogInEmployee.Zgdm;     //分管医生
                #endregion
                this.InpatientListControl1.AfterNavigateToPage += new Controls.UCInpatientListControl.NavigateToPage(InpatientListControl1_AfterNavigateToPage);
                this.InpatientListControl1.PathZx.Visibility = Visibility;

                //dtpStartDate = DateTime.Now.AddDays(-3);
                 //BindGridView();
                this.InpatientListControl1.InitPage(); //分管病人
                //分管病人默认最新话的时候 隐藏控件
                this.InpatientListControl1.HideControl(false);
                
                GetQCProblemList();//医生查询回复的问题
                BindGridViewExamine();//检验报告
                radRwsj.SelectedDate = DateTime.Now; //医生任务信息默认时间为当天
                BindDoctorTaskMessage("", DateTime.Now.ToString("yyyy-MM-dd"));
                //GetData("", DateTime.Now.ToString("yyyy-MM-dd"));
                rbtnSubmit.IsEnabled = false;

                rtviAnswer.TileState = TileViewItemState.Maximized;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }


        private void InpatientListControl1_AfterNavigateToPage(object sender, RoutedEventArgs e)
        {
            UCInpatientListControl.OpreateEventArgs Args = (UCInpatientListControl.OpreateEventArgs)e;
            if (Args.OpreateType == true)//页面导航
            {
                #region add by luff 20130807 根据配置表判断进入哪个路径执行界面 0或空值进入第三方控件路径执行页面，1则进入微软控件路径执行页面
                try
                {

                    List<APPCFG> t_listApp = Global.mAppCfg.Select(s => s).Where(s => s.Configkey.IndexOf("PathEnter") > -1).ToList();
                    if (t_listApp.Count > 0)
                    {
                        if (t_listApp[0].Value == "1")//表示进入微软控件路径执行页面（钟山医院）
                        {
                            if (Global.isNavigate)
                            {
                                this.NavigationService.Navigate(new Uri("/Views/PathEnForceZS.xaml", UriKind.Relative));
                            }
                            else
                            {
                                InpatientList inpaint = new InpatientList();
                                inpaint.AddTabItem("路径执行【" + Global.InpatientListCurrent.Hzxm + "】", "/Views/PathEnForceZS.xaml", true);
                            }
                        }
                        else
                        {
                            if (Global.isNavigate)
                            {
                                this.NavigationService.Navigate(new Uri("/Views/PathEnForce.xaml", UriKind.Relative));
                            }
                            else
                            {
                                InpatientList inpaint = new InpatientList();
                                inpaint.AddTabItem("路径执行【" + Global.InpatientListCurrent.Hzxm + "】", "/Views/PathEnForce.xaml", true);
                            }

                        }
                    }
                    else
                    {
                        if (Global.isNavigate)
                        {
                            this.NavigationService.Navigate(new Uri("/Views/PathEnForce.xaml", UriKind.Relative));
                        }
                        else
                        {
                            InpatientList inpaint = new InpatientList();
                            inpaint.AddTabItem("路径执行【" + Global.InpatientListCurrent.Hzxm + "】", "/Views/PathEnForce.xaml", true);
                        }
                    }
                }
                catch (Exception ex)
                {

                    throw ex;
                }

                #endregion
               
            }
            else//数据捆绑
            {
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }




        #endregion

        #region 函数
        /// <summary>
        /// 绑定GridView
        /// </summary>
        public void BindGridView()
        {
            busyBLSXXX.IsBusy = true;
            busyBLSXXXJG.IsBusy = true;
            StringBuilder where = new StringBuilder();
            #region 病人时限信息（提示）
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetV_QCRecordListCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error != null) PublicMethod.RadWaringBox(e.Error);
                        else
                            GridViewBLSXXX.ItemsSource = e.Result.ToList();
                        this.labMessageCont1.Text = "您有" + e.Result.ToList().Count.ToString() +"条信息！"; 
                        busyBLSXXX.IsBusy = false;
                    };
            serviceCon.GetV_QCRecordListAsync("0");
            #endregion
            #region 病人时限信息（警告）
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetV_QCRecordListCompleted +=

            (obj, e) =>
            {
                if (e.Error != null) PublicMethod.RadWaringBox(e.Error);
                else
                    GridViewBLSXXXJG.ItemsSource = e.Result.ToList();
                this.labMessageCont2.Text = "您有" + e.Result.ToList().Count.ToString() + "条信息！"; 
                busyBLSXXXJG.IsBusy = false;

            };
            serviceCon.GetV_QCRecordListAsync("1");
            #endregion

        }

        /// <summary>
        /// 获取医务处提出异常问题数据列表
        /// </summary>
        private void GetQCProblemList()
        {
            busyAnswer.IsBusy = true;
            YidanEHRDataServiceClient GetQCProblemListClient = PublicMethod.YidanClient;
            GetQCProblemListClient.GetQCProblemListCompleted +=
            (obj, e) =>
            {
                if (e.Error == null)
                {
                    rgvQuestion.ItemsSource = e.Result;
                    //this.labMessageCont5.Text = "您有" + e.Result.ToList().Count.ToString() + "条信息！"; 
                }
                else
                {
                    PublicMethod.RadWaringBox(e.Error);
                }
                busyAnswer.IsBusy = false; ;

            };
            GetQCProblemListClient.GetQCProblemListAsync(cmbQueryState.SelectedIndex, cmbQueryDays.SelectedIndex.ToString(), 1);
            GetQCProblemListClient.CloseAsync();
        }
        public void BindGridViewExamine()
        {
            busyExamine.IsBusy = true;
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetV_PatientExamineListCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error != null) PublicMethod.RadWaringBox(e.Error);
                        else
                            GridViewExmanine.ItemsSource = e.Result.ToList();
                        this.labMessageCont4.Text = "您有" + e.Result.ToList().Count.ToString() + "条信息！"; 
                        busyExamine.IsBusy = false;
                    };
            serviceCon.GetV_PatientExamineListAsync();
        }
        /// <summary>
        /// 医生任务提示信息
        /// </summary>
        private void BindDoctorTaskMessage(string rwzt, string rwsj)
        {
            this.radDocTastBusy.IsBusy = true;
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetDoctorTaskMessageCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        this.rgdDoctorTaskMessage.ItemsSource = e.Result.ToList();

                        labMessageCont.Text = "您有" + e.Result.ToList().Count.ToString() + "条任务未完成！";
                        radDocTastBusy.IsBusy = false;
                    }

                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                        radDocTastBusy.IsBusy = false;
                    }
                };
            serviceCon.GetDoctorTaskMessageAsync(Global.LogInEmployee.Zgdm, rwzt, rwsj);
        }

        /// <summary>
        ///  ZM 8.9 用户中心统一获值端
        /// </summary>
        /// <param name="rwzt"></param>
        /// <param name="rwsj"></param>
        private void GetData(string rwzt, string rwsj)
        {
            busyBLSXXX.IsBusy = true;
            busyBLSXXXJG.IsBusy = true;
            busyAnswer.IsBusy = true;
            busyExamine.IsBusy = true;
            this.radDocTastBusy.IsBusy = true;
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetUserCenterManagerInfoCompleted +=
                (obj, e) =>
                {
                    try
                    {
                        busyBLSXXX.IsBusy = false;
                        busyBLSXXXJG.IsBusy = false;
                        busyAnswer.IsBusy = false;
                        busyExamine.IsBusy = false;
                        radDocTastBusy.IsBusy = false;
                        if (e.Error == null)
                        {
                            //病人时限信息（提示）

                            //GridViewBLSXXX.ItemsSource = e.Result.V_QCRecordList1.ToList();

                            //病人时限信息（警告）

                            //GridViewBLSXXXJG.ItemsSource = e.Result.V_QCRecordList2.ToList();

                            //异常问题处理
                            rgvQuestion.ItemsSource = e.Result.CP_QCProblemList.ToList();
                            //检验报告
                            GridViewExmanine.ItemsSource = e.Result.V_PatientExamineList.ToList();
                            //医师任务
                            this.rgdDoctorTaskMessage.ItemsSource = e.Result.CP_DoctorTaskMessageList.ToList();
                            labMessageCont.Text = "您有" + e.Result.CP_DoctorTaskMessageList.ToList().Count.ToString() + "条任务未完成！";
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
            serviceCon.GetUserCenterManagerInfoAsync(Global.LogInEmployee.Zgdm, rwzt, rwsj, cmbQueryState.SelectedIndex, cmbQueryDays.SelectedIndex.ToString(), 1);
            serviceCon.CloseAsync();
        }
        #endregion







        #region 回复问题
        private void AuditQuestion()
        {
            //if (txtAnswerContent.Text.Trim() == "")
            //{
            //    PublicMethod.RadAlterBox("问题回复不能为空!", "提示");
            //    return;
            //}

            busyAnswer.IsBusy = true;
            YidanEHRDataServiceClient AuditQuestionClient = PublicMethod.YidanClient;
            AuditQuestionClient.AuditQuestionCompleted +=
                (obj, e) =>
                {
                    busyAnswer.IsBusy = false;
                    if (e.Error == null)
                    {
                        if (e.Result.ToString() == "操作失败!")
                        {
                            PublicMethod.RadAlterBox("问题回复失败！", "提示");
                        }
                        else
                        {
                            //更新数据源
                            CP_QCProblem cp = (CP_QCProblem)rgvQuestion.SelectedItem;
                            cp.Wtzt = 1;
                            cp.Qczt = "已答复";
                            cp.Dfrq = e.Result.ToString();
                            cp.Dfys = Global.LogInEmployee.Zgdm;
                            cp.Dfnr = txtAnswerContent.Text.Trim();

                            rgvQuestion.CurrentItem = rgvQuestion.SelectedItem;

                            PublicMethod.RadAlterBox("问题回复成功！", "提示");
                        }
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            //获取答复问题数据
            CP_QCProblem cp2 = new CP_QCProblem();

            cp2.Wtxh = Convert.ToInt32(txtQuestionContent.Tag);
            cp2.Dfnr = txtAnswerContent.Text.Trim();
            cp2.Dfys = Global.LogInEmployee.Zgdm;
            cp2.Dfysxm = Global.LogInEmployee.Name;
            //提交答复问题
            AuditQuestionClient.AuditQuestionAsync(cp2, 3);
            AuditQuestionClient.CloseAsync();
        }




        #endregion

        #region 医生任务提示信息






        private void btnQueryDocTaskMessage_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string rwzt = (this.cmbRwzt.SelectedIndex) == 0 ? "" : (this.cmbRwzt.SelectedIndex - 1).ToString();

                DateTime dt = (DateTime)this.radRwsj.SelectedDate;

                string rwsj = dt.ToString("yyyy-MM-dd");

                BindDoctorTaskMessage(rwzt, rwsj);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        /// <summary>
        /// 医师任务信息 窗体切换 change事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtviTaskMess_TileStateChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {

            if (rtviDoctorTaskMessage.TileState == TileViewItemState.Maximized)
            {
                rgdDoctorTaskMessage.Visibility = Visibility.Visible;
                panel_Query.Visibility = Visibility.Visible;
                labMessageCont.Visibility = Visibility.Collapsed;

                rgdDoctorTaskMessage.Columns[0].IsVisible = true;
                rgdDoctorTaskMessage.Columns[2].IsVisible = true;
                rgdDoctorTaskMessage.Columns[7].IsVisible = true;
            }
            else if (rtviDoctorTaskMessage.TileState == TileViewItemState.Restored)
            {
                rgdDoctorTaskMessage.Visibility = Visibility.Visible;
                panel_Query.Visibility = Visibility.Collapsed;
                labMessageCont.Visibility = Visibility.Collapsed;

                rgdDoctorTaskMessage.Columns[0].IsVisible = false;
                rgdDoctorTaskMessage.Columns[2].IsVisible = false;
                rgdDoctorTaskMessage.Columns[7].IsVisible = false;
            }
            else
            {
                rgdDoctorTaskMessage.Visibility = Visibility.Collapsed;
                panel_Query.Visibility = Visibility.Collapsed;
                labMessageCont.Visibility = Visibility.Visible;
            }
        }

        #endregion

        private void rgvQuestion_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                //设置回复模块控件属性
                rbtnSubmit.IsEnabled = true;
                CP_QCProblem cp = (CP_QCProblem)rgvQuestion.SelectedItem;
                if (cp == null) return;
                txtQuestionContent.Text = cp.Wtnr;
                txtQuestionContent.Tag = cp.Wtxh;
                txtAuditContent.Text = cp.Shnr;
                if (cp.Dfnr != "")
                {
                    txtAnswerContent.Text = cp.Dfnr + " [" + cp.Dfysxm + " " + cp.Dfrq + "]\n";
                }
                else
                {
                    txtAnswerContent.Text = cp.Dfnr;
                }
                rbtnSubmit.IsEnabled = (cp.Wtzt == 0 || cp.Wtzt == 2);
                txtAnswerContent.IsReadOnly = !(cp.Wtzt == 0 || cp.Wtzt == 2);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void rbtnQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender == null) return;
                #region 查询
                switch (((Button)sender).Name.ToString())
                {
                    case "rbtnQueryQuestion":   //医生回复的问题
                        //部分控件初始化
                        txtQuestionContent.Text = "";
                        txtQuestionContent.Tag = "";
                        txtAuditContent.Text = "";
                        txtAnswerContent.Text = "";
                        rbtnSubmit.IsEnabled = false;
                        txtAnswerContent.IsReadOnly = true;
                        GetQCProblemList();
                        
                        break;
                    case "rbtnQueryInpatient": //分管病人

                        break;
                }
                #endregion
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void rbtnSubmit_Click(object sender, RoutedEventArgs e)
        {
             
            try
            {
               
                if (txtAnswerContent.Text.Trim() == "")
                {
                    PublicMethod.RadAlterBox("回复内容不能为空!", "提示");
                    return;
                }
                else
                {
                    if (this.txtAnswerContent.Text.Length > 1600)
                    {
                        
                        PublicMethod.RadAlterBox("您已经超出输入长度，请修改!", "提示");
                        return;
                        
                    }
                }
                //DialogParameters parameters = new DialogParameters();
                //parameters.Content = String.Format("{0}", "确定回复该条记录吗?");
                //parameters.Header = "提示";
                //parameters.IconContent = null;
                //parameters.OkButtonContent = "确定";
                //parameters.CancelButtonContent = "取消";
                //parameters.Closed = OnDelAdviceGroupDetail;//***close处理***
                //RadWindow.Confirm(parameters);
                YidanPathWayMessageBox mess = new YidanPathWayMessageBox("确定回复该条记录吗？", "提示", YiDanMessageBoxButtons.YesNo);
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


        #region 切换窗口事件
        /// <summary>
        /// 病历时限提示信息 切换窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTileViewBlsx_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (radTileViewBlsx.TileState == TileViewItemState.Maximized)
            {
                radTileViewBlsx.Visibility = Visibility.Visible;
               
                labMessageCont1.Visibility = Visibility.Collapsed;

                
            }
            else if (radTileViewBlsx.TileState == TileViewItemState.Restored)
            {
                GridViewBLSXXX.Visibility = Visibility.Visible;
                
                labMessageCont1.Visibility = Visibility.Collapsed;

                
            }
            else
            {
                GridViewBLSXXX.Visibility = Visibility.Collapsed;
                //获取数据统计总条数
                if(GridViewBLSXXX.ItemsSource==null)
                {
                    labMessageCont1.Text = "您有" + "0"+ "条信息！"; 
                }
                else
                {
                    labMessageCont1.Text = "您有" +((List<V_QCRecord>)GridViewBLSXXX.ItemsSource).Count.ToString() + "条信息！"; 
                }
               
                labMessageCont1.Visibility = Visibility.Visible;
            }
        }
        /// <summary>
        /// 病历时限警告信息 切换窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTileViewBljg_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (radTileViewBljg.TileState == TileViewItemState.Maximized)
            {
                GridViewBLSXXXJG.Visibility = Visibility.Visible;

                labMessageCont2.Visibility = Visibility.Collapsed;
 
            }
            else if (radTileViewBlsx.TileState == TileViewItemState.Restored)
            {
                GridViewBLSXXXJG.Visibility = Visibility.Visible;

                labMessageCont2.Visibility = Visibility.Collapsed;

                 
            }
            else
            {
                GridViewBLSXXXJG.Visibility = Visibility.Collapsed;
                if (GridViewBLSXXXJG.ItemsSource == null)
                {
                    labMessageCont2.Text = "您有" + "0" + "条信息！";
                }
                else
                {
                    labMessageCont2.Text = "您有" + ((List<V_QCRecord>)GridViewBLSXXXJG.ItemsSource).Count.ToString() + "条信息！";
                }
                
                labMessageCont2.Visibility = Visibility.Visible;
            }
        }

        /// <summary>
        /// 分管病人
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtviInpatient_TileStateChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            //隐藏InpatientListControl1里部分控件
            if (rtviInpatient.TileState == TileViewItemState.Maximized)
            {
                this.InpatientListControl1.HideControl(true);
            }
            else
            {
                this.InpatientListControl1.HideControl(false);
            }
        }
        /// <summary>
        /// 分管病人检查信息 切换窗口事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTileViewBljy_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (radTileViewBljy.TileState == TileViewItemState.Maximized)
            {
                GridViewExmanine.Visibility = Visibility.Visible;

                labMessageCont4.Visibility = Visibility.Collapsed;

               
            }
            else if (radTileViewBlsx.TileState == TileViewItemState.Restored)
            {
                GridViewExmanine.Visibility = Visibility.Visible;

                labMessageCont4.Visibility = Visibility.Collapsed;

               
            }
            else
            {
                GridViewExmanine.Visibility = Visibility.Visible;
                if (GridViewExmanine.ItemsSource == null)
                {
                    labMessageCont4.Text = "您有" + "0" + "条信息！";
                }
                else
                {
                    labMessageCont4.Text = "您有" + ((List<V_QCRecord>)GridViewExmanine.ItemsSource).Count.ToString() + "条信息！"; 
                }



                labMessageCont4.Visibility = Visibility.Collapsed;
            }
        }
        /// <summary>
        /// 医生异常问题
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtviAnswer_TileStateChanged(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            if (rtviAnswer.TileState == TileViewItemState.Maximized)
            {
                gridAudit.Visibility = Visibility.Visible;
                this.YcclPlan.Visibility = Visibility.Visible;
                this.rgvQuestion.Visibility = Visibility.Visible;
                labMessageCont5.Visibility = Visibility.Collapsed;
            }
            else if (rtviAnswer.TileState == TileViewItemState.Restored)
            {
                gridAudit.Visibility = Visibility.Visible;
                this.YcclPlan.Visibility = Visibility.Visible;
                this.rgvQuestion.Visibility = Visibility.Visible;
                labMessageCont5.Visibility = Visibility.Collapsed;


            }
            else
            {
                gridAudit.Visibility = Visibility.Visible;
                this.YcclPlan.Visibility = Visibility.Visible;
                this.rgvQuestion.Visibility = Visibility.Visible;
                 //if (rgvQuestion.ItemsSource == null)
                 //{
                 //    labMessageCont5.Text = "您有" + "0" + "条信息！";
                 //}
                 //else
                 //{
                 //    labMessageCont5.Text = "您有" + ((List<V_QCRecord>)rgvQuestion.ItemsSource).Count.ToString() + "条信息！"; 
                 //}

                labMessageCont5.Visibility = Visibility.Collapsed;
            }
        }
        #endregion

        
       
    }
}
