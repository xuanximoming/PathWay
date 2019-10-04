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
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using YidanEHRApplication.Helpers;
using Telerik.Windows.Controls;

namespace YidanEHRApplication.Views.Permission
{

    public partial class PathDiagnosisManager : Page
    {
        YidanEHRDataServiceClient serviceCon;

        CP_Diagnosis_E m_cp_diagnosis = new CP_Diagnosis_E();
        public PathDiagnosisManager()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(PathDiagnosisManager_Loaded);
        }

        void PathDiagnosisManager_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                BindGridView("");
                RegisterKeyEvent();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        #region 函数
        //YidanEHRServiceReference.yida serviceCon;

        /// <summary>
        /// 绑定路径诊断库中已有病种信息
        /// </summary>
        private void BindGridView(string key)
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetCP_PathDiagnosisListCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error == null)
                        {
                            GridView.ItemsSource = e.Result.ToList();
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(e.Error);
                        }
                    };
            serviceCon.GetCP_PathDiagnosisListAsync(key);
            serviceCon.CloseAsync();
        }

        public bool DrugItemFilter(string text, object item)
        {
            CP_Diagnosis_E diag = (CP_Diagnosis_E)item;
            // Call it a match if the typed-in text appears in the product code
            // or at the beginning of the product name.
            return ((diag.Py.StartsWith(text)) || (diag.Py.Contains(text))
                  || (diag.Wb.StartsWith(text)) || (diag.Wb.Contains(text))
                  || (diag.Name.StartsWith(text)) || (diag.Name.Contains(text))
                  || (diag.Zdbs.StartsWith(text)) || (diag.Zdbs.Contains(text))
                  || (diag.Zddm.StartsWith(text)) || (diag.Zddm.Contains(text)));
        }

        /// <summary>
        /// 绑定页面中编辑信息
        /// </summary>
        /// <param name="pe_fun">实体，如果为空编辑区域值为空</param>
        private void BindEditArea(CP_Diagnosis_E cp_diag)
        {
            if (cp_diag != null)
            {

                this.txtZdbs.Text = cp_diag.Zdbs;
                this.txtZddm.Text = cp_diag.Zddm;
                this.txtYsdm.Text = cp_diag.Ysdm;

                this.txtBzdm.Text = cp_diag.Bzdm;
                this.txtName.Text = cp_diag.Name;
                this.txtPy.Text = cp_diag.Py;

                this.txtWb.Text = cp_diag.Wb;
                this.txtZldm.Text = cp_diag.Zldm;
                this.txtTjm.Text = cp_diag.Tjm;

                this.txtNbfl.Text = cp_diag.Nbfl;
                this.txtBzlb.Text = cp_diag.Bzlb;
                this.txtQtlb.Text = cp_diag.Qtlb;

                this.txtYxjl.Text = cp_diag.Yxjl;
                this.txtMemo.Text = cp_diag.Memo;

            }
            else
            {
                this.txtZdbs.Text = "";
                this.txtZddm.Text = "";
                this.txtYsdm.Text = "";

                this.txtBzdm.Text = "";
                this.txtName.Text = "";
                this.txtPy.Text = "";

                this.txtWb.Text = "";
                this.txtZldm.Text = "";
                this.txtTjm.Text = "";

                this.txtNbfl.Text = "";
                this.txtBzlb.Text = "";
                this.txtQtlb.Text = "";

                this.txtYxjl.Text = "";
                this.txtMemo.Text = "";
            }
        }

        /// <summary>
        /// 根据医院诊断库中数据判断是否存在于路径诊断库中，如果存在则不给添加到路径库中，可以删除。如果不存在则给添加到路径诊断库中
        /// </summary>
        /// <param name="diag_e">医院诊断实体</param>
        private void IsExists(CP_Diagnosis_E diag_e)
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.IsHaveCP_PathDiagnosisCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        //判断是否存在路径诊断库，存在返回true
                        bool b = e.Result;
                        if (b)
                        {
                            this.labMessage.Text = @"提示：当前诊断信息已经存在于路径诊断库中，不可以添加！";
                            this.labMessage.Foreground = new SolidColorBrush(Colors.Red);

                            this.btnADD.IsEnabled = false;
                            this.btnDel.IsEnabled = true;
                        }
                        else
                        {
                            this.labMessage.Text = @"提示：当前诊断信息不存在于路径诊断库中，可以添加！";
                            this.labMessage.Foreground = new SolidColorBrush(Colors.Green);

                            this.btnADD.IsEnabled = true;
                            this.btnDel.IsEnabled = false;
                        }

                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            serviceCon.IsHaveCP_PathDiagnosisAsync(m_cp_diagnosis.Zdbs);
            serviceCon.CloseAsync();
        }

        #endregion

        private void autoDiagName_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (autoDiagName.SelectedItem != null)
                {
                    m_cp_diagnosis = (CP_Diagnosis_E)autoDiagName.SelectedItem;

                    BindEditArea(m_cp_diagnosis);
                    IsExists(m_cp_diagnosis);

                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void autoDiagName_TextChanged(object sender, RoutedEventArgs e)
        {
            if (autoDiagName.Text.Trim().Length == 1)
            {
                BindCP_Diagnosis(autoDiagName.Text.Trim());
            }
        }

        private void BindCP_Diagnosis(string KeyWord)
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetCP_DiagnosisListCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        List<CP_Diagnosis_E> list = e.Result.ToList();
                        autoDiagName.ItemsSource = list;
                        autoDiagName.ItemFilter = DrugItemFilter;
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };

            serviceCon.GetCP_DiagnosisListAsync(KeyWord);
            serviceCon.CloseAsync();
        }


        /// <summary>
        /// 从路径诊断库中添加诊断信息
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnADD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_cp_diagnosis.Zdbs != null)
                {
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.CopyCP_DiagnosisToPathCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    SQLMessage mess = ea.Result;
                                    if (mess.IsSucceed)
                                    {
                                        this.btnADD.IsEnabled = false;
                                    }
                                    PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                                    BindGridView("");
                                    labMessage.Text = "";
                                    BindEditArea(null);
                                }
                                else
                                {
                                    PublicMethod.RadWaringBox(ea.Error);
                                }
                            };
                    serviceCon.CopyCP_DiagnosisToPathAsync(m_cp_diagnosis.Zdbs);
                    serviceCon.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 将医院诊断信息删除路径诊断库中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnDel_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_cp_diagnosis == null || m_cp_diagnosis.Zdbs == null)
                {
                    PublicMethod.RadAlterBox("请选中要删除的行！", "提示");
                }
                else
                {
                    string mess1 = "请问是否将该诊断信息从路经诊断库中删除？";

                    //DialogParameters parameters = new DialogParameters();
                    //parameters.Content = String.Format("提示: {0}", mess);
                    //parameters.Header = "提示";
                    //parameters.IconContent = null;
                    //parameters.OkButtonContent = "确定";
                    //parameters.CancelButtonContent = "取消";
                    //parameters.Closed = OnDelAdviceGroupDetail;//***close处理***
                    //RadWindow.Confirm(parameters);
                    YidanPathWayMessageBox mess = new YidanPathWayMessageBox(mess1, "提示", YiDanMessageBoxButtons.YesNo);
                    mess.ShowDialog();
                    mess.PageClosedEvent += new YidanPathWayMessageBox.PageClosed(mess_PageClosedEvent);
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
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.DelCP_DiagnosisFromPathCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                SQLMessage mess = ea.Result;
                                if (mess.IsSucceed)
                                {
                                    this.btnDel.IsEnabled = false;
                                }
                                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                                autoDiagName.Text = "";
                                labMessage.Text = "";
                                BindGridView("");
                                BindEditArea(null);
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.DelCP_DiagnosisFromPathAsync(m_cp_diagnosis.Zdbs);
                    serviceCon.CloseAsync();

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
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.DelCP_DiagnosisFromPathCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                SQLMessage mess = ea.Result;
                                if (mess.IsSucceed)
                                {
                                    this.btnDel.IsEnabled = false;
                                }
                                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                                autoDiagName.Text = "";
                                labMessage.Text = "";
                                BindGridView("");
                                BindEditArea(null);
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.DelCP_DiagnosisFromPathAsync(m_cp_diagnosis.Zdbs);
                    serviceCon.CloseAsync();
                }
                catch (Exception ex)
                {
                    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                }
            }
        }
        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            BindGridView(this.tbQuery.Text.Replace(" ", ""));
        }

        #region  输入框加回车事件
        private void RegisterKeyEvent()
        {
            tbQuery.KeyUp += new KeyEventHandler(tbQuery_KeyUp);
        }

        private void tbQuery_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnQuery_Click(null, null);
        }

        #endregion




    }

}
