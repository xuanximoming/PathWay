using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using Telerik.Windows.Controls;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Views.UserControls
{
    public partial class UCRISLISOrder : UserControl
    {
        private const string m_strTitle = "医嘱提示"; //定义弹出框标题栏

        private ManualType m_ManualType = ManualType.New;
        /// <summary>
        /// 操作类型
        /// </summary>
        public ManualType ManualType
        {
            get
            {
                return m_ManualType;
            }
            set
            {
                m_ManualType = value;
            }
        }

        #region 药品变量
        List<int> zdm = new List<int>();  //存储周代码，存储当前选中的星期
        List<string> zxsj = new List<string>();//存储执行时间，存储当前选中的时间
        string Strzdm = string.Empty;    //zdm字符串形式
        string Strzxsj = string.Empty; //zxsj字符串形式
        ObservableCollection<CP_AdviceGroupDetail> cP_AdviceGroupDetailCollection = new ObservableCollection<CP_AdviceGroupDetail>();
        List<CP_PCSJ> cplist = new List<CP_PCSJ>();
        #endregion

        #region 药品属性
        CP_DoctorOrder _cp_AdviceGroupDetail = new CP_DoctorOrder();

        public CP_DoctorOrder CP_AdviceGroupDetailProptery
        {
            get
            {
                return _cp_AdviceGroupDetail;
            }
            set
            {
                _cp_AdviceGroupDetail = value;
            }
        }
        #endregion
        public UCRISLISOrder()
        {
            InitializeComponent();
        }
        bool isLoad = true;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (!isLoad)
                {
                    //isLoad = true;
                    return;
                }
                OnAfterDrugLoadedEvent(e);
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void InitPage()
        {
            try
            {
                InitDrugInfo();//初始化药品信息//项目下拉框
                InitOrderTypeInfo(cbxMDYZBZ); //初始化医嘱类别（临时医嘱，长期医嘱） 
                cbxMDYZBZ.SelectedIndex = 0;
                IntiComboBoxDept();
                #region 判断his是否支持计价类型
                //try
                //{
                //    YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                //    referenceClient.GetAppConifgTypeCompleted +=
                //        (obj, e) =>
                //        {
                //            if (e.Error == null && e.Result > -1)
                //            {
                                this.txtjjlx.Visibility = Visibility.Visible;
                                this.cbxJJLX.Visibility = Visibility.Visible;

                                InitJJTypeInfo(cbxJJLX);
                                cbxJJLX.SelectedIndex = 0;

                //            }
                //            else
                //            {
                //                this.txtjjlx.Visibility = Visibility.Collapsed;
                //                this.cbxJJLX.Visibility = Visibility.Collapsed;

                //                PublicMethod.RadWaringBox(e.Error);
                //            }
                //        };
                //    referenceClient.GetAppConifgTypeAsync("HisJjlx");
                //    referenceClient.CloseAsync();

                //}
                //catch (Exception ex)
                //{
                //    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                //}

                //add by luff 20130313 获得配置表关于医嘱可选不算变异参数 若值为1表示可选 前台控件就显示，0表示必须 前台控件就隐藏
                List<APPCFG> t_listApp = Global.mAppCfg.Select(s => s).Where(s => s.Configkey.IndexOf("Yziskx") > -1).ToList();
                if (t_listApp.Count > 0)
                {
                    if (t_listApp[0].Value == "1")
                    {
                        txtisby.Visibility = Visibility.Visible;
                        radkx.Visibility = Visibility.Visible;
                        radbx.Visibility = Visibility.Visible;
                        this.radkx.IsChecked = true;
                        this.radbx.IsChecked = false;
                    }
                    else
                    {
                        txtisby.Visibility = Visibility.Collapsed;
                        radkx.Visibility = Visibility.Collapsed;
                        radbx.Visibility = Visibility.Collapsed;
                        this.radbx.IsChecked = true;
                        this.radkx.IsEnabled = false;
                    }
                }
                else
                {
                    txtisby.Visibility = Visibility.Collapsed;
                    radkx.Visibility = Visibility.Collapsed;
                    radbx.Visibility = Visibility.Collapsed;
                    this.radbx.IsChecked = true;
                    this.radkx.IsEnabled = false;
                }


                #endregion
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }


        /// <summary>
        /// 初始化医嘱标志数据
        /// </summary>
        /// <param name="radcombobox"></param>
        private void InitOrderTypeInfo(RadComboBox radcombobox)
        {
            radcombobox.EmptyText = "请选择医嘱类别";
            List<OrderTypeName> iList = new List<OrderTypeName>();
            iList.Add(new OrderTypeName("临时医嘱", 2702));
            iList.Add(new OrderTypeName("长期医嘱", 2703));
            radcombobox.ItemsSource = iList;
            radcombobox.SelectedIndex = 0;
            radcombobox.IsEnabled = false;
            txtZTNR.Focus();

        }
        /// <summary>
        /// 初始化计费类型数据 add by luff 20130118
        /// </summary>
        /// <param name="radcombobox"></param>
        private void InitJJTypeInfo(RadComboBox radcombobox)
        {
            //radcombobox.EmptyText = "请选择计价类型";
            List<OrderTypeName> iList = new List<OrderTypeName>();
            iList.Add(new OrderTypeName("正常计价", 1));
            iList.Add(new OrderTypeName("自带药", 2));
            iList.Add(new OrderTypeName("不计价", 3));
            radcombobox.ItemsSource = iList;
            radcombobox.SelectedIndex = 0;
            //autoCompleteBoxOrder.Focus();

        }

        #region 设定计价类型类别
        public class JjTypeName
        {
            public string JjlxName
            {
                get;
                set;
            }
            public short JjlxValue
            {
                get;
                set;
            }
            public JjTypeName(string jjlxName, short jjlxValue)
            {
                JjlxName = jjlxName;
                JjlxValue = jjlxValue;
            }
        }
        #endregion
        #region 执行科室
        /// <summary>
        /// 执行科室
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
                            autoCompleteBoxDept.ItemsSource = e.Result;
                            autoCompleteBoxDept.ItemFilter = DeptFilter;
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
        public bool DeptFilter(string strFilter, object item)
        {
            CP_DepartmentList deptList = (CP_DepartmentList)item;
            return ((deptList.QueryName.StartsWith(strFilter.ToUpper())) || (deptList.QueryName.Contains(strFilter.ToUpper())));
        }
        #endregion

        
        /// <summary>
        /// 初始化药品数据
        /// </summary>
        private void InitDrugInfo()
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.GetRISLISOrderInfoCompleted +=
            (obj, e) =>
            {
                if (e.Error == null)
                {
                    autoCompleteBoxOrder.ItemsSource = e.Result;
                    autoCompleteBoxOrder.ItemFilter = OrderFilter;
                    //if (_cp_AdviceGroupDetail.OrderGuid != null && this.ManualType == Helpers.ManualType.Edit)
                    //    InitModifyOrder();
                }
                else
                {
                    PublicMethod.RadWaringBox(e.Error);
                    return;
                }
            };
            client.GetRISLISOrderInfoAsync();
            client.CloseAsync();
        }



        public bool OrderFilter(string strFilter, object item)
        {
            CP_ChargingMinItem deptList = (CP_ChargingMinItem)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())));
        }

        #region 频次后面的周代码和时间
        private void CheckBoxWeek_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cplist = (List<CP_PCSJ>)cbxSJ.ItemsSource;

                for (int i = 0; i < cplist.Count; i++)
                {
                    CheckBox ck = sender as CheckBox;
                    if (cplist[i].DwFlag != "Week") // Week为数据库AS出来的值
                    {
                        ck.IsEnabled = false;
                    }
                    else
                    {
                        for (int j = 0; j < cplist[i].Zdm.Length; j++)
                        {

                            if (ck.Tag.ToString() == cplist[i].Zdm.Substring(j, 1).ToString())
                            {
                                ck.IsChecked = true;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }


        }
        private void CheckBoxWeek_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox ck = sender as CheckBox;
            zdm.Add(Convert.ToInt32(ck.Tag.ToString()));
        }

        private void CheckBoxWeek_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox ck = sender as CheckBox;

            if (zdm.Count != 0)
            {
                if (zdm.Contains(Convert.ToInt32(ck.Tag.ToString())))
                {
                    zdm.Remove(Convert.ToInt32(ck.Tag.ToString()));
                }
            }
        }

        private void CheckBoxTime_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                cplist = (List<CP_PCSJ>)cbxSJ.ItemsSource;
                for (int i = 0; i < cplist.Count; i++)
                {
                    CheckBox ck = sender as CheckBox;
                    if (cplist[i].DwFlag == "Hour" || cplist[i].DwFlag == "Minutes")  // Hour，Minutes为数据库AS出来的值
                    {
                        ck.IsEnabled = false;
                    }
                    else
                    {
                        for (int j = 0; j < cplist[i].Zxsj.Split(',').Length - 1; j++)
                        {
                            if (ck.Content.ToString() == cplist[i].Zxsj.Split(',')[j].ToString())
                            {
                                ck.IsChecked = true;
                                if (!zxsj.Contains(ck.Content.ToString() + ","))
                                {
                                    zxsj.Add(ck.Content.ToString() + ",");
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void CheckBoxTime_Checked(object sender, RoutedEventArgs e)
        {
            CheckBox ck = sender as CheckBox;
            if (!zxsj.Contains(ck.Content.ToString() + ","))
            {
                zxsj.Add(ck.Content.ToString() + ",");
            }
        }

        private void CheckBoxTime_Unchecked(object sender, RoutedEventArgs e)
        {
            CheckBox ck = sender as CheckBox;
            if (zxsj.Count != 0)
            {
                if (zxsj.Contains(ck.Content.ToString() + ","))
                {
                    zxsj.Remove(ck.Content.ToString() + ",");
                }
            }
        }
        #endregion

        private List<DrugUnitsType> m_ListDrugUnit = new List<DrugUnitsType>();
        private void InitUnitType(string strName, decimal unitID)
        {
            DrugUnitsType type = new DrugUnitsType(strName, unitID);
            m_ListDrugUnit.Add(type);
        }

        private void autoCompleteBoxOrder_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (autoCompleteBoxOrder.SelectedItem != null)
                {
                    YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                    StringBuilder strItems = new StringBuilder();
                    #region 单位
                    this.txtUnitName.Text = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmdw;
                    #endregion
                    //项目单价 add  by luff 20121108
                    this.txtXmdj.Text = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmdj.ToString();
                    #region 数量
                    nudMDSL.Value = 1;
                    #endregion

                    #region 频次代码
                    if (cbxPC.Items.Count == 0)
                    {
                        client.GetAdviceFrequencyCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    cbxPC.ItemsSource = ea.Result;
                                    cbxPC.SelectedIndex = 0;
                                }
                                else
                                {
                                    PublicMethod.RadWaringBox(ea.Error);
                                }
                            };
                        client.GetAdviceFrequencyAsync("00");
                    }
                    else
                    {
                        cbxPC.SelectedValue = _cp_AdviceGroupDetail.Pcdm;
                    }
                    #endregion

                    //this.autoCompleteBoxDept.SelectedItem = ((ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource).FirstOrDefault(cp => cp.Ksdm.Equals(((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Memo));

                    client.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }




        /// <summary>
        /// 频次
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPC_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                if (cbxPC.SelectedItem != null)
                {
                    zdm.Clear();
                    zxsj.Clear();
                    string pcdm = ((CP_AdviceFrequency)cbxPC.SelectedItem).Pcdm;
                    YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                    client.GetDropDownInfoCompleted +=
                            (obj, ea) =>
                            {
                                if (ea.Error == null)
                                {
                                    cbxSJ.ItemsSource = ea.Result.ToList();
                                    zdm.Clear();
                                    zxsj.Clear();
                                    cbxSJ.SelectedIndex = 0;
                                    //xjt,时间必须弹出BUG
                                    cplist = ea.Result.ToList();
                                    for (int i = 0; i < cplist.Count(); i++)
                                    {
                                        for (int j = 0; j < cplist[i].Zxsj.Split(',').Length - 1; j++)
                                        {
                                            if (!zxsj.Contains(cplist[i].Zxsj.Split(',')[j].ToString() + ","))
                                            {
                                                zxsj.Add(cplist[i].Zxsj.Split(',')[j].ToString() + ",");
                                            }
                                        }
                                    }
                                }
                                else
                                {
                                    PublicMethod.RadWaringBox(ea.Error);
                                }

                            };
                    client.GetDropDownInfoAsync(pcdm);
                    client.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }



        private void btnMDXYZ_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewAdviceGroupDetail();
                autoCompleteBoxOrder.Focus();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnMDQD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Check())
                {
                    InitDoctorDrug4Confirm();
                    OnAfterDrugClosedEvent(CP_AdviceGroupDetailProptery);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private bool Check()
        {
            #region 保存之前判断
            if (autoCompleteBoxOrder.SelectedItem == null || cbxPC.SelectedItem == null || cbxMDYZBZ.SelectedItem == null || cbxSJ.SelectedItem == null)
            {
                string AlterMessage =
                      (cbxMDYZBZ.SelectedItem == null ? "\r\n" + "医嘱标志必须选择" : "")
                    + (autoCompleteBoxOrder.SelectedItem == null ? "\r\n" + "项目必须选择" : "")
                    + (cbxPC.SelectedItem == null ? "\r\n" + "频次必须选择" : "")
                     + (cbxSJ.SelectedItem == null ? "\r\n" + "频次时间必须选择" : "");
                //Control ct = cbxMDYZBZ.SelectedItem == null ? cbxMDYZBZ : null;
                //if (ct == null)
                //{
                //    ct = autoCompleteBoxOrder.SelectedItem == null ? autoCompleteBoxOrder : null;
                //}
                //if (ct == null)
                //{
                //    ct = cbxPC.SelectedItem == null ? cbxPC : null;
                //}
                //if (ct == null)
                //{
                //    ct = cbxSJ.SelectedItem == null ? cbxSJ : null;
                //}

                PublicMethod.RadAlterBoxRe(AlterMessage, m_strTitle, autoCompleteBoxOrder);
                isLoad = false;
                return false;
            }
            if (zdm.Count != GetZdmCount((List<CP_PCSJ>)cbxSJ.ItemsSource))
            {
                PublicMethod.RadAlterBox("超出或者低于周代码限制数,限制为【" + GetZdmCount((List<CP_PCSJ>)cbxSJ.ItemsSource).ToString() + "】周", m_strTitle);
                return false;

            }

            else
            {
                if (zxsj.Count != GetZxsjCount((List<CP_PCSJ>)cbxSJ.ItemsSource))
                {
                    PublicMethod.RadAlterBox("超出或低于执行时间限制数,限制为【" + GetZxsjCount((List<CP_PCSJ>)cbxSJ.ItemsSource).ToString() + "】次", m_strTitle);
                    return false;
                }
            }
            List<CP_AdviceGroupDetail> listJudgeSame = cP_AdviceGroupDetailCollection.ToList<CP_AdviceGroupDetail>(); //用于存放Grid数据源

            //if (((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderValue == 2703)    //在添加长期医嘱的时候需要判断是否存在相同的项目
            //{
            for (int i = 0; i < listJudgeSame.Count; i++)
            {
                if (listJudgeSame[i].Ypdm == ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Sfxmdm
                    && listJudgeSame[i].Yzbz == ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderValue)  //  && CurrentState == PageState.New                        
                {
                    PublicMethod.RadAlterBox("存在相同项目医嘱,无法继续添加", m_strTitle);
                    return false;
                }
            }
            //}
            #endregion
            return true;
        }

        /// <summary>
        /// 点击确定时赋值
        /// </summary>
        private void InitDoctorDrug4Confirm()
        {
            if (this.ManualType == Helpers.ManualType.Edit)
            {
                _cp_AdviceGroupDetail.Yzbz = ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderValue;
                _cp_AdviceGroupDetail.YzbzName = ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderName;

                _cp_AdviceGroupDetail.Cdxh = 0;
                _cp_AdviceGroupDetail.Ggxh = 0;
                _cp_AdviceGroupDetail.Lcxh = 0;
                _cp_AdviceGroupDetail.Ypdm = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Sfxmdm;
                _cp_AdviceGroupDetail.Xmlb = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmlb;
                _cp_AdviceGroupDetail.Ypmc = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Name;
                _cp_AdviceGroupDetail.Zxdw = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmdw;
                _cp_AdviceGroupDetail.Ypgg = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmgg;
                _cp_AdviceGroupDetail.Ypjl = Convert.ToDecimal(nudMDSL.Value);
                _cp_AdviceGroupDetail.Ypjl = Convert.ToDecimal(nudMDSL.Value);
                _cp_AdviceGroupDetail.Jldw = txtUnitName.Text;
                _cp_AdviceGroupDetail.Xmdj = Convert.ToDecimal(this.txtXmdj.Text.Trim());
                //_cp_AdviceGroupDetail.Yfdm = ((CP_DrugUseage)cbxMDYF.SelectedItem).Yfdm;
                //_cp_AdviceGroupDetail.YfdmName = ((CP_DrugUseage)cbxMDYF.SelectedItem).Name;
                _cp_AdviceGroupDetail.Pcdm = ((CP_AdviceFrequency)cbxPC.SelectedItem).Pcdm;
                _cp_AdviceGroupDetail.PcdmName = ((CP_AdviceFrequency)cbxPC.SelectedItem).Name;
                _cp_AdviceGroupDetail.Lrysdm = Global.LogInEmployee.Zgdm;
                _cp_AdviceGroupDetail.Ztnr = txtZTNR.Text;
                #region 判断his是否支持计价类型
                //try
                //{
                    //YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                    //referenceClient.GetAppConifgTypeCompleted +=
                    //    (obj, e) =>
                    //    {
                    //        if (e.Error == null && e.Result > -1)
                    //        {
                                this.txtjjlx.Visibility = Visibility.Visible;
                                this.cbxJJLX.Visibility = Visibility.Visible;
                                _cp_AdviceGroupDetail.Jjlx = int.Parse(cbxJJLX.SelectedValue.ToString());
                                _cp_AdviceGroupDetail.Jjlxmc = cbxJJLX.Text.ToString();

                //            }
                //            else
                //            {
                //                this.txtjjlx.Visibility = Visibility.Collapsed;
                //                this.cbxJJLX.Visibility = Visibility.Collapsed;
                //                _cp_AdviceGroupDetail.Jjlx = 1;
                //                _cp_AdviceGroupDetail.Jjlxmc = "";
                //                PublicMethod.RadWaringBox(e.Error);
                //            }
                //        };
                //    referenceClient.GetAppConifgTypeAsync("HisJjlx");
                //    referenceClient.CloseAsync();

                //}
                //catch (Exception ex)
                //{
                //    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                //}

                #endregion
                if (this.radkx.IsChecked == true)
                {
                    _cp_AdviceGroupDetail.Yzkx = 1;
                }
                else
                {
                    _cp_AdviceGroupDetail.Yzkx = 0;
                }

                _cp_AdviceGroupDetail.Zxksdm = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm;
                _cp_AdviceGroupDetail.Zxksdmmc = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Name; 
                _cp_AdviceGroupDetail.Yzlb = (int)OrderPanelBarCategory.RisLis;
                _cp_AdviceGroupDetail.Yznr = _cp_AdviceGroupDetail.Ypmc + " " + _cp_AdviceGroupDetail.PcdmName + " " + _cp_AdviceGroupDetail.YfdmName;
            }
            else
            {
                #region Cp_DoctorDrug赋值
                Strzdm = string.Empty;
                Strzxsj = string.Empty;
                _cp_AdviceGroupDetail = new CP_DoctorOrder();
                _cp_AdviceGroupDetail.Syxh = Global.InpatientListCurrent == null ? 0 : Convert.ToDecimal(Global.InpatientListCurrent.Syxh);
                _cp_AdviceGroupDetail.Bqdm = Global.LogInEmployee.Bqdm;
                _cp_AdviceGroupDetail.Ksdm = Global.LogInEmployee.Ksdm;
                _cp_AdviceGroupDetail.Lrysdm = Global.LogInEmployee.Zgdm;
                _cp_AdviceGroupDetail.Cdxh = 0;
                _cp_AdviceGroupDetail.Ggxh = 0;
                _cp_AdviceGroupDetail.Lcxh = 0;
                _cp_AdviceGroupDetail.Ypdm = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Sfxmdm;
                _cp_AdviceGroupDetail.Xmlb = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmlb;
                _cp_AdviceGroupDetail.Yzbz = ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderValue;
                _cp_AdviceGroupDetail.YzbzName = ((OrderTypeName)cbxMDYZBZ.SelectedItem).OrderName;
                _cp_AdviceGroupDetail.Ypjl = Convert.ToDecimal(nudMDSL.Value);
                _cp_AdviceGroupDetail.Jldw = txtUnitName.Text;
                _cp_AdviceGroupDetail.Xmdj = Convert.ToDecimal(this.txtXmdj.Text.Trim());
                //_cp_AdviceGroupDetail.Yfdm = ((CP_DrugUseage)cbxMDYF.SelectedItem).Yfdm;
                //_cp_AdviceGroupDetail.YfdmName = ((CP_DrugUseage)cbxMDYF.SelectedItem).Name;
                _cp_AdviceGroupDetail.Pcdm = ((CP_AdviceFrequency)cbxPC.SelectedItem).Pcdm;
                _cp_AdviceGroupDetail.PcdmName = ((CP_AdviceFrequency)cbxPC.SelectedItem).Name;
                _cp_AdviceGroupDetail.Ksrq = GetDefaultOrderTime((OrderType)(Convert.ToDecimal(_cp_AdviceGroupDetail.Yzbz)));
                _cp_AdviceGroupDetail.Ypmc = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Name;
                _cp_AdviceGroupDetail.FromTable = string.Empty;
                _cp_AdviceGroupDetail.Flag = string.Empty;
                _cp_AdviceGroupDetail.OrderGuid = Guid.NewGuid().ToString();//
                _cp_AdviceGroupDetail.Fzbz = 3500;
                _cp_AdviceGroupDetail.Zxdw = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmdw;
                _cp_AdviceGroupDetail.Ypgg = ((CP_ChargingMinItem)autoCompleteBoxOrder.SelectedItem).Xmgg;
                _cp_AdviceGroupDetail.Dwxs = 1;//单位系数不知道为何。。。
                //_cp_AdviceGroupDetail.Dwlb = cbxMDDW.SelectedValue == null ? 3007 : ((DrugUnitsType)cbxMDDW.SelectedItem).UnitID; ;
                _cp_AdviceGroupDetail.Zxcs = ((CP_AdviceFrequency)cbxPC.SelectedItem).Zxcs;
                _cp_AdviceGroupDetail.Zxzq = ((CP_AdviceFrequency)cbxPC.SelectedItem).Zxzq;
                _cp_AdviceGroupDetail.Zxzqdw = ((CP_AdviceFrequency)cbxPC.SelectedItem).Zxzqdw;
                foreach (int i in zdm)
                {
                    Strzdm += i.ToString();
                }
                _cp_AdviceGroupDetail.Zdm = (zdm.Count == 0 ? null : Strzdm);
                foreach (string s in zxsj)
                {
                    if (s != "," && s != "")
                    {
                        Strzxsj += s;
                    }
                }
                _cp_AdviceGroupDetail.Zxsj = (zxsj.Count == 0 ? null : Strzxsj);
                #region 判断his是否支持计价类型
                //try
                //{
                //    YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
                //    referenceClient.GetAppConifgTypeCompleted +=
                //        (obj, e) =>
                //        {
                //            if (e.Error == null && e.Result > -1)
                //            {
                                this.txtjjlx.Visibility = Visibility.Visible;
                                this.cbxJJLX.Visibility = Visibility.Visible;
                                _cp_AdviceGroupDetail.Jjlx = int.Parse(cbxJJLX.SelectedValue.ToString());
                                _cp_AdviceGroupDetail.Jjlxmc = cbxJJLX.Text.ToString();

                //            }
                //            else
                //            {
                //                this.txtjjlx.Visibility = Visibility.Collapsed;
                //                this.cbxJJLX.Visibility = Visibility.Collapsed;
                //                _cp_AdviceGroupDetail.Jjlx = 1;
                //                _cp_AdviceGroupDetail.Jjlxmc = "";
                //                PublicMethod.RadWaringBox(e.Error);
                //            }
                //        };
                //    referenceClient.GetAppConifgTypeAsync("HisJjlx");
                //    referenceClient.CloseAsync();

                //}
                //catch (Exception ex)
                //{
                //    PublicMethod.ClientException(ex, this.GetType().FullName, true);
                //}

                #endregion
                _cp_AdviceGroupDetail.Zxksdm = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm;
                _cp_AdviceGroupDetail.Zxksdmmc = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Name; 
                _cp_AdviceGroupDetail.Yznr = _cp_AdviceGroupDetail.Ypmc + " " + _cp_AdviceGroupDetail.PcdmName + " " + _cp_AdviceGroupDetail.YfdmName;
                _cp_AdviceGroupDetail.Yzzt = 3200;
                _cp_AdviceGroupDetail.Fzxh = 0;
                _cp_AdviceGroupDetail.Yzlb = (int)OrderPanelBarCategory.RisLis;
                _cp_AdviceGroupDetail.Zxts = 0;
                _cp_AdviceGroupDetail.Ypzsl = 0; //出院带药。
                _cp_AdviceGroupDetail.Ztnr = txtZTNR.Text;
                if (this.radkx.IsChecked == true)
                {
                    _cp_AdviceGroupDetail.Yzkx = 1;
                }
                else
                {
                    _cp_AdviceGroupDetail.Yzkx = 0;
                }
                #endregion
            }
        }

        /// <summary>
        /// 获取周代码的数量为了判断是否选择超出或者低于约束的值
        /// </summary>
        /// <param name="pcsj"></param>
        /// <returns></returns>
        private int GetZdmCount(List<CP_PCSJ> pcsj)
        {
            int zdmlength = 0;
            foreach (var c in pcsj)
            {
                zdmlength = c.Zdm.Trim(' ').Length;
            }
            return zdmlength;

        }

        /// <summary>
        /// 获取时间代码的数量为了判断是否选择超出或者低于约束的值
        /// </summary>
        /// <param name="pcsj"></param>
        /// <returns></returns>
        private int GetZxsjCount(List<CP_PCSJ> pcsj)
        {
            int zxsjlength = 0;
            foreach (var c in pcsj)
            {
                zxsjlength = c.Zxsj.Split(',').Length - 1;
            }
            return zxsjlength;
        }

        #region delegate own events
        public delegate void DrugLoaded(object sender, RoutedEventArgs e);
        /// <summary>
        /// 此事件的用是为了能让控件直接在界面上显示
        /// </summary>
        public event DrugLoaded AfterDrugLoadedEvent;

        protected virtual void OnAfterDrugLoadedEvent(RoutedEventArgs e)
        {
            if (AfterDrugLoadedEvent != null)
            {
                InitPage();
                RegisterKeyEvent();
            }
        }
        public delegate void DrugConfirmed(object sender, CP_DoctorOrder e);
        public event DrugConfirmed AfterDrugCinfirmeddEvent;

        protected virtual void OnAfterDrugClosedEvent(CP_DoctorOrder e)
        {
            if (AfterDrugCinfirmeddEvent != null)
                AfterDrugCinfirmeddEvent(this, e);
        }

        /// <summary>
        /// 路径执行,医嘱默认时间
        /// </summary>
        /// <param name="orderType"></param>
        /// <returns></returns>
        private string GetDefaultOrderTime(OrderType orderType)
        {
            string strTime = string.Empty;
            if (orderType == OrderType.Long)
                strTime = Convert.ToString(DateTime.Now.AddDays(1).Date + new TimeSpan(8, 0, 0));
            else
            {
                int hour = DateTime.Now.Hour;
                int minute = DateTime.Now.Minute;
                if (minute <= 30)
                    minute = 30;
                else
                {
                    hour += 1;
                    minute = 0;
                }
                strTime = Convert.ToString(DateTime.Today + new TimeSpan(hour, minute, 0));
            }
            return strTime;
        }
        #endregion

        /// <summary>
        /// 初始化需要修改的医嘱
        /// </summary>
        public void InitModifyOrder()
        {
            cbxMDYZBZ.IsEnabled = false;
            autoCompleteBoxOrder.IsEnabled = false;
            cbxMDYZBZ.SelectedValue = (short)_cp_AdviceGroupDetail.Yzbz;
            autoCompleteBoxOrder.SelectedItem = ((ObservableCollection<CP_ChargingMinItem>)autoCompleteBoxOrder.ItemsSource).First(cp => cp.Sfxmdm.Equals(_cp_AdviceGroupDetail.Ypdm));
            nudMDSL.Value = Convert.ToDouble(_cp_AdviceGroupDetail.Ypjl);
            txtUnitName.Text = _cp_AdviceGroupDetail.Zxdw;
            this.txtXmdj.Text = _cp_AdviceGroupDetail.Xmdj.ToString();
            cbxPC.SelectedValue = _cp_AdviceGroupDetail.Pcdm;
            //add by luff 20130118
            cbxJJLX.SelectedValue = (short)_cp_AdviceGroupDetail.Jjlx;
            if (_cp_AdviceGroupDetail.Zxksdm == "")
            {
                autoCompleteBoxDept.SelectedItem = null;
            }
            else
            {
                autoCompleteBoxDept.SelectedItem = ((ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource).FirstOrDefault(cp => cp.Ksdm.Equals(_cp_AdviceGroupDetail.Zxksdm));
            }

            //add by luff 20130314
            if (_cp_AdviceGroupDetail.Yzkx == 0)
            {
                this.radbx.IsChecked = true;
                this.radkx.IsChecked = false;
            }
            else
            {
                this.radbx.IsChecked = false;
                this.radkx.IsChecked = true;
            }
            txtZTNR.Text = _cp_AdviceGroupDetail.Ztnr == null ? string.Empty : _cp_AdviceGroupDetail.Ztnr;
        }

        /// <summary>
        /// 清空控件,暂时PUBLIC，最好改成属性CHANGED 事件触发
        /// </summary>
        public void NewAdviceGroupDetail()
        {
            this.ManualType = Helpers.ManualType.New;//
            _cp_AdviceGroupDetail = new CP_DoctorOrder();
            //cbxMDYZBZ.SelectedValue = null;
            autoCompleteBoxOrder.SelectedItem = null;
            autoCompleteBoxOrder.Text = "";
            nudMDSL.Value = 0;
            //cbxMDDW.Text = "";
            //CurrentState = PageState.New;
            txtUnitName.Text = string.Empty;
            cbxSJ.SelectedValue = null;
            cbxMDYF.SelectedValue = null;
            //cbxPC.SelectedValue = null;
            txtZTNR.Text = "";
             txtXmdj.Text = "";
            nudTS.Value = 0;
            zdm.Clear();
            zxsj.Clear();
            zdm = new List<int>();
            zxsj = new List<string>();
            cbxJJLX.SelectedIndex = 0;
            autoCompleteBoxDept.SelectedItem = null;
            autoCompleteBoxDept.Text = "";
            cbxMDYZBZ.IsEnabled = true;
            autoCompleteBoxOrder.IsEnabled = true;

            #region
            //add by luff 20130313 获得配置表关于医嘱可选不算变异参数 来判断前台是否显示
            List<APPCFG> t_listApp = Global.mAppCfg.Select(s => s).Where(s => s.Configkey.IndexOf("Yziskx") > -1).ToList();
            if (t_listApp.Count > 0)
            {
                if (t_listApp[0].Value == "1")
                {
                    txtisby.Visibility = Visibility.Visible;
                    radkx.Visibility = Visibility.Visible;
                    radbx.Visibility = Visibility.Visible;
                    this.radkx.IsChecked = true;
                    this.radbx.IsChecked = false;
                }
                else
                {
                    txtisby.Visibility = Visibility.Collapsed;
                    radkx.Visibility = Visibility.Collapsed;
                    radbx.Visibility = Visibility.Collapsed;
                    this.radbx.IsChecked = true;
                    this.radkx.IsEnabled = false;
                }
            }
            else
            {
                txtisby.Visibility = Visibility.Collapsed;
                radkx.Visibility = Visibility.Collapsed;
                radbx.Visibility = Visibility.Collapsed;
                this.radbx.IsChecked = true;
                this.radkx.IsEnabled = false;
            }
            #endregion
            //autoCompleteBoxOrder.Text = string.Empty;

        }



        #region KEYUP
        /// <summary>
        /// 注册KEYUP
        /// </summary>
        private void RegisterKeyEvent()
        {
            cbxMDYZBZ.KeyUp += new KeyEventHandler(cbxMDYZBZ_KeyUp);
            radbx.KeyUp += new KeyEventHandler(radbx_KeyUp);
            radkx.KeyUp += new KeyEventHandler(radkx_KeyUp);
            autoCompleteBoxOrder.KeyUp += new KeyEventHandler(autoCompleteBoxOrder_KeyUp);
            nudMDSL.KeyUp += new KeyEventHandler(nudMDSL_KeyUp);
            cbxMDYF.KeyUp += new KeyEventHandler(cbxMDYF_KeyUp);
            cbxPC.KeyUp += new KeyEventHandler(cbxPC_KeyUp);
            cbxSJ.KeyUp += new KeyEventHandler(cbxSJ_KeyUp);
            //txtXmdj.KeyUp += new KeyEventHandler(txtXmdj_KeyUp);
            cbxJJLX.KeyUp += new KeyEventHandler(cbxJJLX_KeyUp);
            autoCompleteBoxDept.KeyUp += new KeyEventHandler(autoCompleteBoxDept_KeyUp);
            txtZTNR.KeyUp += new KeyEventHandler(txtZTNR_KeyUp);
            btnMDQD.KeyUp += new KeyEventHandler(btnMDQD_KeyUp);
        }

        /// <summary>
        /// 医嘱类别
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMDYZBZ_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                radbx.Focus();
        }

        /// <summary>
        /// 医嘱是否变异
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radbx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                radkx.Focus();
        }
        private void radkx_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                autoCompleteBoxOrder.Focus();
        }

        /// <summary>
        /// 项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoCompleteBoxOrder_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                nudMDSL.Focus();
        }


        /// <summary>
        /// 数量-数字
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void nudMDSL_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbxPC.Focus();
        }

        /// <summary>
        /// 用法
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxMDYF_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbxPC.Focus();
        }

        /// <summary>
        /// 频次-代码
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxPC_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbxSJ.Focus();
        }

        /// <summary>
        /// 频次-时间
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxSJ_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                cbxJJLX.Focus();
        }
        /// <summary>
        /// 计价类型
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cbxJJLX_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                autoCompleteBoxDept.Focus();
        }
        /// <summary>
        /// 执行科室
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoCompleteBoxDept_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                txtZTNR.Focus();
        }
        //add by luff 20121108
        ///// <summary>
        ///// 项目单价
        ///// </summary>
        ///// <param name="sender"></param>
        ///// <param name="e"></param>
        //private void txtXmdj_KeyUp(object sender, KeyEventArgs e)
        //{
        //    if (e.Key == Key.Enter)
        //        txtZTNR.Focus();
        //}
        /// <summary>
        /// 嘱托
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtZTNR_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnMDQD.Focus();
        }

        private void btnMDQD_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
                btnMDQD_Click(null, null);
        }

        #endregion


    }
}
