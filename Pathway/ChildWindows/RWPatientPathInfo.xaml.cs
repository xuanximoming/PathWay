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
using Telerik.Windows.Controls;

using System.Collections.ObjectModel;
using YidanEHRApplication.Models;
using YidanEHRApplication.WorkFlow.Designer;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication
{
    public partial class RWPatientPathInfo : RadWindow
    {
        CP_InpatinetList InpatinetList=new CP_InpatinetList();
         string[,] pathDetailID;
         //int TotalStep = 0; //路径执行总步骤

         /// <summary>
         /// 当前显示的执行路径
         /// </summary>
         private WorkFlow.WorkFlow m_WorkFlow = new WorkFlow.WorkFlow();

         public RWPatientPathInfo(CP_InpatinetList cp)
        {
            InitializeComponent();           
            InpatinetList = cp;


            InitWorkFlow(InpatinetList.EnForceWorkFlowXml);

        }

        private void OKButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            this.DialogResult = false;
        }

        private void radNumericUpDown_ValueChanged(object sender, RadRangeBaseValueChangedEventArgs e)
        {
            //if (radNumericUpDown != null)
            //{
            //    GetPathExecVariantRecordList(Convert.ToInt32(radNumericUpDown.Value));
            //}
        }

        public void GetPathExecVariantRecordList(String mxdm)
        {
            //if (index > 0 && index <= TotalStep)
            //{
                
                radBusyIndicator.IsBusy = true;
                //labelDateTime.Content = pathDetailID[index - 1, 1];
                // PublicMethod.RadAlterBox(pathDetailID[index - 1, 0], "提示"); //测试使用
                YidanEHRDataServiceClient GetPathExecVariantRecordListClient = PublicMethod.YidanClient;
                GetPathExecVariantRecordListClient.GetPathExecVariantRecordListCompleted +=
                 (obj, ea) =>
                 {
                     radBusyIndicator.IsBusy = false;
                     if (ea.Error == null)
                     {
                         GridViewPathExec.ItemsSource = null;
                         GridViewPathExec.ItemsSource = ea.Result;


                     }
                     else
                     {
                         PublicMethod.RadWaringBox(ea.Error);
                     }
                 };

                //GetPathExecVariantRecordListClient.GetPathExecVariantRecordListAsync("P.K62.001", "7245314f-4f87-41bf-8280-878e6801e0fe"); //测试使用
                GetPathExecVariantRecordListClient.GetPathExecVariantRecordListAsync(InpatinetList.Ljxh.ToString(), mxdm);
                GetPathExecVariantRecordListClient.CloseAsync();
            //}
        }

       
        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        { try{
            //初始化查询变异的路径名称/路径代码/执行步骤
            //labelDateTime.Content = DateTime.Now.ToString(); 
            if (InpatinetList != null)
            {
                if (InpatinetList.EnForceWorkFlowXml == null || InpatinetList.EnForceWorkFlowXml == "")
                {
                    //radNumericUpDown.IsEnabled = false;
                }
                else
                {   
                    labelPath.Content = InpatinetList.Ljmc;
                    labelPatientName.Content = InpatinetList.Hzxm;
                }
            }
            else
            {
                //radNumericUpDown.IsEnabled = false;
            } }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }

        /// <summary>
        /// 初始化显示工作流图,并实例化m_WorkFlow
        /// </summary>
        private void InitWorkFlow(String strXml)
        {
            try
            {
                this.gridWorkFlowShow.Children.Clear();
                m_WorkFlow = new WorkFlow.WorkFlow();
                ContainerShow containerEdit = new ContainerShow();
                containerEdit.IsShowAll = false;
                containerEdit.WorkFlowUrlName = Global.InpatientListCurrent.Ljmc;
                containerEdit.WorkFlowUrlID = Global.InpatientListCurrent.Ljdm;
                containerEdit.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                containerEdit.VerticalAlignment = System.Windows.VerticalAlignment.Top;
                containerEdit.WorkFlowXML = strXml;
                containerEdit.Width = Browser.ClientWidth - 20;
                this.gridWorkFlowShow.Height = 200;
                this.gridWorkFlowShow.Children.Add(containerEdit);
                gridWorkFlowShow.MouseLeave += new MouseEventHandler(gridWorkFlowShow_MouseLeave);
                m_WorkFlow.ContainerEdit = containerEdit;
                m_WorkFlow.Activitys.WorkFlow_ActivitySelectChanged += new WorkFlow.WorkFlow_ActivetySelectedDelegateEventHandler(Activitys_WorkFlow_ActivitySelectChanged);
            }
            catch (Exception ex)
            {
                EnableButtonState(false);
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void Activitys_WorkFlow_ActivitySelectChanged(Activity a)
        {
            //to do 首先判断结点类型
            //to do 分四种情况:下一步，查看分三钟:直接上一步，循环的二种
            //保存
            try
            {
                radBusyIndicator.IsBusy = true;
                //SetButtonEnable(a);
                labelEnForceTime.Content = a.CurrentViewActiveChildren.EnForceTime;
                GetPathExecVariantRecordList(a.UniqueID);

                if (a.Type == ActivityType.AUTOMATION) //循环结点
                {
                    if (a.CurrentElementState == WorkFlow.ElementState.Pre)
                    {
                        
                        //去医嘱表LOAD    
                        //GetActivityOrder(a, false);
                    }
                    else if (a.CurrentElementState == WorkFlow.ElementState.Now)
                    {
                        //to do LOAD 最后一个子结点的初始化医令，若点击了查看两BUTTON，则跟根据类型判断是去医嘱表LOAD，还是组套表LOAD
                        if (a.CurrentViewActiveChildren.CurrentElementState == WorkFlow.ElementState.Now
                            && a.CurrentViewActiveChildren.EnForceTime == String.Empty)
                        {
                            //GetActivityOrder(a, true);
                        }
                        else
                        {
                            //GetActivityOrder(a, false);
                        }
                    }
                    else if (a.CurrentElementState == WorkFlow.ElementState.Next)
                    {
                        //to do 下一步button不可用，只能看医嘱 
                        //GetActivityOrder(a, true);
                    }
                    else
                    {
                        //GetActivityOrder(a, true);
                    }
                }
                else
                {
                    if (a.CurrentElementState == WorkFlow.ElementState.Pre)
                    {
                        //to do 下一步button可用，去医嘱表LOAD 
                        //GetActivityOrder(a, false);
                    }
                    else if (a.CurrentElementState == WorkFlow.ElementState.Now)
                    {
                        ////to do 下一步button可用，去组套表LOAD 
                        //if (a.CurrentViewActiveChildren.EnForceTime == String.Empty)
                        //    GetActivityOrder(a, true);
                        //else
                        //    GetActivityOrder(a, false);
                    }
                    else if (a.CurrentElementState == WorkFlow.ElementState.Next)
                    {
                        //to do 下一步button不可用，只能看医嘱 
                        //GetActivityOrder(a, true);
                    }
                    else
                    {
                        //GetActivityOrder(a, true);
                    }
                }
            }
            catch (Exception ex)
            {
                radBusyIndicator.IsBusy = false;
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
            finally
            {
                radBusyIndicator.IsBusy = false;
            }

        }


        /// <summary>
        ///  EnableButtonState
        /// </summary>
        private void EnableButtonState(Boolean isEnable)
        {
            //this.buttonConfirm.IsEnabled = isEnable;
            //this.buttonNext.IsEnabled = isEnable;
            //this.buttonComplete.IsEnabled = isEnable;
            //this.radButtonDelOrder.IsEnabled = isEnable;
            //this.radButtonNewOrder.IsEnabled = isEnable;
            //this.radButtonModifyOrder.IsEnabled = isEnable;
            //this.radButtonQuit.IsEnabled = isEnable;
            //this.radbuttonLeadIn.IsEnabled = isEnable;
        }


        #region 显示/隐藏执行路径
        /// <summary>
        /// 显示
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void hlkButtonShowPaht_MouseEnter(object sender, MouseEventArgs e)
        {
            this.gridWorkFlowShow.Visibility = System.Windows.Visibility.Visible;
        }

        /// <summary>
        /// 隐藏
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void gridWorkFlowShow_MouseLeave(object sender, MouseEventArgs e)
        {
            gridWorkFlowShow.Visibility = System.Windows.Visibility.Collapsed;
        }
        #endregion



        
    }
}

