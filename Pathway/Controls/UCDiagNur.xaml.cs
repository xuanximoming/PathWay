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
using System.Collections.ObjectModel;
using YidanEHRApplication.DataService;
using Telerik.Windows.Controls;
using YidanEHRApplication.Models;
using System.Text;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Models;


namespace YidanEHRApplication.Views.UserControls
{
    public partial class UCDiagNur : UserControl
    {
        public UCDiagNur()
        {
            InitializeComponent();
        }
        private const string m_strTitle = "诊疗护理提示"; //定义弹出框标题栏

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



        private OrderItemCategory m_OrderCategory = OrderItemCategory.Meal;
        /// <summary>
        /// 项目初始化型类
        /// </summary>
        public OrderItemCategory OrderCategory
        {
            get
            {
                return m_OrderCategory;
            }
            set
            {
                m_OrderCategory = value;
            }
        }

      

        #region 变量
        List<int> zdm = new List<int>();  //存储周代码，存储当前选中的星期
        List<string> zxsj = new List<string>();//存储执行时间，存储当前选中的时间
        string Strzdm = string.Empty;    //zdm字符串形式
        string Strzxsj = string.Empty; //zxsj字符串形式
        //定义一个全局集合类型，用于取诊疗护理基础数据源
        private ObservableCollection<CP_DiagNurExecCategoryDetail> m_CP_DiagNurTemplateCollection;
        ObservableCollection<CP_DiagNurTemplate> CP_DiagNurTemplateCollection = new ObservableCollection<CP_DiagNurTemplate>();
        List<CP_PCSJ> cplist = new List<CP_PCSJ>();
         
        public CP_DiagNurTemplate CP_DiagNurTemplateProptery = new CP_DiagNurTemplate();
        //定义路径代码和节点编号全局变量
        public String m_Ljdm = "";
        public String m_PathID = "";
        private int iID = 0;
        #endregion

        private bool isLoad = true;
        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                //if (!isLoad)
                //{
                //    //isLoad = true;
                //    return;
                //}
                OnAfterDrugLoadedEvent(e);
                this.cbxZlHlLB.IsEnabled = true;
                //isLoad = false;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void InitPage()
        {
            InitOrderTypeInfo(cbxZlHlLB); //初始化诊疗护理类别（1.诊疗工作，2护理工作，3非药物治疗） 
            InitDrugInfo();//初始化诊疗护理信息//项目下拉框

            //初始化计价类型和执行科室数据
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

            #endregion
            //add by luff 20130313 获得配置表关于医嘱可选不算变异参数 若值为1表示可选，0表示必须
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
                radkx.Visibility = Visibility.Collapsed;
                radbx.Visibility = Visibility.Collapsed;
                this.radbx.IsChecked = true;
                this.radkx.IsEnabled = false;
            }


            //InitJJTypeInfo(cbxJJLX);
            //cbxJJLX.SelectedIndex = 0;
        }

        /// <summary>
        /// 初始化初始化诊疗护理项目数据
        /// </summary>
        /// <param name="radcombobox"></param>
        private void InitOrderTypeInfo(RadComboBox radcombobox)
        {
            radcombobox.EmptyText = "请选择诊疗护理类别";
            List<OrderTypeName> iList = new List<OrderTypeName>();
            iList.Add(new OrderTypeName("诊疗工作", 1));
            iList.Add(new OrderTypeName("护理工作", 2));
            iList.Add(new OrderTypeName("非药物治疗", 3));
            radcombobox.ItemsSource = iList;
            radcombobox.SelectedIndex = 0;
            radcombobox.IsEnabled = false;
            //autoCompleteBoxDigNur.Focus();

        }
        /// <summary>
        /// add by luff 绑定诊疗护理分类类型(暂时没有用到)
        /// </summary>
        private void BindCbx()
        {
            try
            {

                YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                Client.GetDiagNurExecCategoryCompleted +=
                (obj, e) =>
                {

                    if (e.Error == null)
                    {

                        cbxZlHlLB.ItemsSource = e.Result;
                        cbxZlHlLB.SelectedIndex = 0;

                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };

                Client.GetDiagNurExecCategoryAsync(1, false);
                Client.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
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
            //autoCompleteBoxDigNur.Focus();

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
        /// 初始化诊疗护理项目
        /// </summary>
        private void InitDrugInfo()
        {
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.GetDiagNurExecCategoryInfoCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                       
                        m_CP_DiagNurTemplateCollection = e.Result;
                        //第一次加载诊疗护理项目控件数据为诊疗相关数据源
                        autoCompleteBoxDigNur.ItemsSource = m_CP_DiagNurTemplateCollection.Select(s => s).Where(s => s.Lbxh == 1).ToList();
                        autoCompleteBoxDigNur.ItemFilter = OrderFilter;
                         
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                        return;
                    }
                };
            client.GetDiagNurExecCategoryInfoAsync();
            client.CloseAsync();
        }



        public bool OrderFilter(string strFilter, object item)
        {
            CP_DiagNurExecCategoryDetail deptList = (CP_DiagNurExecCategoryDetail)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())));
        }

        private List<DrugUnitsType> m_ListDrugUnit = new List<DrugUnitsType>();
        private void InitUnitType(string strName, decimal unitID)
        {
            DrugUnitsType type = new DrugUnitsType(strName, unitID);
            m_ListDrugUnit.Add(type);
        }

        private void autoCompleteBoxDigNur_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            //try
            //{
            //    if (autoCompleteBoxDigNur.SelectedItem != null)
            //    {
            //        YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            //        StringBuilder strItems = new StringBuilder();
                   
 
            //        client.CloseAsync();
            //    }
            //}
            //catch (Exception ex)
            //{
            //    PublicMethod.ClientException(ex, this.GetType().FullName, true);
            //}
        }

        private void btnMDXYZ_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                NewAdviceGroupDetail();
                autoCompleteBoxDigNur.Focus();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private void btnMDQD_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (Check())
                {
                    
                    if(CP_DiagNurTemplateProptery != null)
                    {
                        InitDoctorDrug4Confirm();
                        OnAfterDrugClosedEvent(CP_DiagNurTemplateProptery);
                        NewAdviceGroupDetail();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        private bool Check()
        {
            #region 保存之前判断
            if (cbxZlHlLB.SelectedItem == null)
            {
                string AlterMessage =
                      (cbxZlHlLB.SelectedItem == null ? "\r\n" + "诊疗护理分类类型必须选择" : "");
                    //+ (autoCompleteBoxDigNur.SelectedItem == null ? "\r\n" + "项目必须选择" : "");
                //Control ct = cbxZlHlLB.SelectedItem == null ? cbxZlHlLB : null;
                //if (ct == null)
                //{
                //    ct = autoCompleteBoxDigNur.SelectedItem == null ? autoCompleteBoxDigNur : null;
                //}
                //PublicMethod.RadAlterBoxRe(AlterMessage, m_strTitle, autoCompleteBoxDigNur);
                PublicMethod.RadAlterBoxRe(AlterMessage, m_strTitle, cbxZlHlLB);
                isLoad = false;
                return false;
            }
            if (autoCompleteBoxDigNur.Text == "")
            {
                PublicMethod.RadAlterBoxRe("诊疗护理项目不能为空", m_strTitle, autoCompleteBoxDigNur);
                isLoad = false;
                return false;
            }
            List<CP_DiagNurTemplate> listJudgeSame = CP_DiagNurTemplateCollection.ToList<CP_DiagNurTemplate>(); //用于存放Grid数据源

           
            for (int i = 0; i < listJudgeSame.Count; i++)
            {
                if (listJudgeSame[i].Mxxh == ((CP_DiagNurTemplate)autoCompleteBoxDigNur.SelectedItem).Mxxh
                    && listJudgeSame[i].Lbxh == ((OrderTypeName)cbxZlHlLB.SelectedItem).OrderValue)  //  && CurrentState == PageState.New                        
                {
                    PublicMethod.RadAlterBox("存在相同项目,无法继续添加", m_strTitle);
                    return false;
                }
            }
            
            #endregion
            return true;
        }

        /// <summary>
        /// 点击确定时赋值
        /// </summary>
        public void InitDoctorDrug4Confirm()
        {
            if (this.ManualType == Helpers.ManualType.Edit)
            {
                CP_DiagNurTemplateProptery.ID = iID;
                CP_DiagNurTemplateProptery.Lbxh = ((OrderTypeName)cbxZlHlLB.SelectedItem).OrderValue;
                CP_DiagNurTemplateProptery.Extension2 = ((OrderTypeName)cbxZlHlLB.SelectedItem).OrderName;//分类名称
                CP_DiagNurTemplateProptery.Yxjl = 1;
                CP_DiagNurTemplateProptery.Ljdm = m_Ljdm;
                CP_DiagNurTemplateProptery.PathDetailId = m_PathID;
                CP_DiagNurTemplateProptery.Py = "";
                CP_DiagNurTemplateProptery.Extension = "";
                CP_DiagNurTemplateProptery.Extension1 = "";
                CP_DiagNurTemplateProptery.Wb = "";
                if (autoCompleteBoxDigNur.SelectedItem != null)
                {
                    CP_DiagNurTemplateProptery.Mxxh = ((CP_DiagNurExecCategoryDetail)autoCompleteBoxDigNur.SelectedItem).Mxxh;
                    CP_DiagNurTemplateProptery.MxName = ((CP_DiagNurExecCategoryDetail)autoCompleteBoxDigNur.SelectedItem).Name;
                    //CP_DiagNurTemplateProptery.Extension3 = "";
                }
                else //用于护士手动输入非知识库里面的诊疗护理
                {
                    CP_DiagNurTemplateProptery.Mxxh = 0;
                    CP_DiagNurTemplateProptery.MxName = autoCompleteBoxDigNur.Text == "" ? "手动录入" : autoCompleteBoxDigNur.Text;
                    
                }
                CP_DiagNurTemplateProptery.Extension3 = this.txtZTNR.Text; //((OrderTypeName)cbxZlHlLB.SelectedItem).OrderName + " " + this.txtZTNR.Text;

                CP_DiagNurTemplateProptery.Create_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                CP_DiagNurTemplateProptery.Create_User = Global.LogInEmployee.Zgdm;
                CP_DiagNurTemplateProptery.Cancel_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                CP_DiagNurTemplateProptery.Cancel_User = Global.LogInEmployee.Zgdm;
              
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
                CP_DiagNurTemplateProptery.Isjj  = cbxJJLX.SelectedValue.ToString();
               

                //            }
                //            else
                //            {
                //                this.txtjjlx.Visibility = Visibility.Collapsed;
                //                this.cbxJJLX.Visibility = Visibility.Collapsed;
                //                CP_DiagNurTemplateProptery.Jjlx = 1;
                //                CP_DiagNurTemplateProptery.Jjlxmc = "";
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
                CP_DiagNurTemplateProptery.Zxksdm = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm;
                

                if (this.radkx.IsChecked == true)//当可选已经选中
                {
                    CP_DiagNurTemplateProptery.Iskx = "1";//赋值为1，表示可选不算变异
                }
                else//当必须已经选中
                {
                    CP_DiagNurTemplateProptery.Iskx = "0";//赋值为0，表示必选
                }
            }
            else
            {
                #region 赋值
                //Strzdm = string.Empty;
                //Strzxsj = string.Empty;
                CP_DiagNurTemplateProptery.Ljdm = m_Ljdm;
                CP_DiagNurTemplateProptery.PathDetailId = m_PathID;
                CP_DiagNurTemplateProptery.Lbxh = ((OrderTypeName)cbxZlHlLB.SelectedItem).OrderValue;
                CP_DiagNurTemplateProptery.Extension2 = ((OrderTypeName)cbxZlHlLB.SelectedItem).OrderName;//分类名称
                CP_DiagNurTemplateProptery.Yxjl = 1;
                CP_DiagNurTemplateProptery.Py = "";
                CP_DiagNurTemplateProptery.Extension = "";
                CP_DiagNurTemplateProptery.Extension1 = "";
                CP_DiagNurTemplateProptery.Wb = "";
                if (autoCompleteBoxDigNur.SelectedItem != null)
                {
                    CP_DiagNurTemplateProptery.Mxxh = ((CP_DiagNurExecCategoryDetail)autoCompleteBoxDigNur.SelectedItem).Mxxh;
                    CP_DiagNurTemplateProptery.MxName = ((CP_DiagNurExecCategoryDetail)autoCompleteBoxDigNur.SelectedItem).Name;
                    CP_DiagNurTemplateProptery.Extension3 = this.txtZTNR.Text;
                }
                else //用于护士手动输入非知识库里面的诊疗护理
                {
                    CP_DiagNurTemplateProptery.Mxxh = 0;
                    CP_DiagNurTemplateProptery.MxName = autoCompleteBoxDigNur.Text == "" ? "手动录入" : autoCompleteBoxDigNur.Text;//" 手动录入";
                    CP_DiagNurTemplateProptery.Extension3 = this.txtZTNR.Text; //((OrderTypeName)cbxZlHlLB.SelectedItem).OrderName + " " + this.txtZTNR.Text;
                }

                CP_DiagNurTemplateProptery.Create_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                CP_DiagNurTemplateProptery.Create_User = Global.LogInEmployee.Zgdm;
                CP_DiagNurTemplateProptery.Cancel_Time = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                CP_DiagNurTemplateProptery.Cancel_User = Global.LogInEmployee.Zgdm;

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
                CP_DiagNurTemplateProptery.Isjj = cbxJJLX.SelectedValue.ToString();


                //            }
                //            else
                //            {
                //                this.txtjjlx.Visibility = Visibility.Collapsed;
                //                this.cbxJJLX.Visibility = Visibility.Collapsed;
                //                CP_DiagNurTemplateProptery.Jjlx = 1;
                //                CP_DiagNurTemplateProptery.Jjlxmc = "";
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
                CP_DiagNurTemplateProptery.Zxksdm = autoCompleteBoxDept.SelectedItem == null ? "" : ((CP_DepartmentList)(autoCompleteBoxDept.SelectedItem)).Ksdm;



                if (this.radkx.IsChecked == true)//当可选已经选中
                {
                    CP_DiagNurTemplateProptery.Iskx = "1";//赋值为1，表示可选不算变异
                }
                else//当必须已经选中
                {
                    CP_DiagNurTemplateProptery.Iskx = "0";//赋值为0，表示必选
                }
                #endregion
            }
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
        public delegate void DrugConfirmed(object sender, CP_DiagNurTemplate e);
        public event DrugConfirmed AfterDrugCinfirmeddEvent;

        protected virtual void OnAfterDrugClosedEvent(CP_DiagNurTemplate e)
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
            try
            {
                if (CP_DiagNurTemplateProptery != null)
                {
                    iID = CP_DiagNurTemplateProptery.ID;
                    cbxZlHlLB.IsEnabled = true;
                    autoCompleteBoxDigNur.IsEnabled = true;
                    cbxZlHlLB.SelectedValue = (short)CP_DiagNurTemplateProptery.Lbxh;
                    if (CP_DiagNurTemplateProptery.Mxxh != 0)
                    {
                        autoCompleteBoxDigNur.SelectedItem = ((List<CP_DiagNurExecCategoryDetail>)autoCompleteBoxDigNur.ItemsSource).First(cp => cp.Mxxh.Equals(CP_DiagNurTemplateProptery.Mxxh));
                    }
                    else
                    {
                        autoCompleteBoxDigNur.Text = CP_DiagNurTemplateProptery.MxName;
                    }
                    nudMDSL.Value = 0;
                    txtUnitName.Text = "";
                    //add by luff 20130410
                    //Isjj 为计价类型； Iskx为是否可选；Zxksdm 为执行科室代码;Extension3 为备注
                    txtZTNR.Text = CP_DiagNurTemplateProptery.Extension3;
                    cbxJJLX.SelectedValue = (short)int.Parse(CP_DiagNurTemplateProptery.Isjj == null ? "1" : CP_DiagNurTemplateProptery.Isjj);
                    //Zxksdm 为执行科室代码
                    if (CP_DiagNurTemplateProptery.Zxksdm==null || CP_DiagNurTemplateProptery.Zxksdm == "")
                    {
                        autoCompleteBoxDept.SelectedItem = null;
                    }
                    else
                    {
                        autoCompleteBoxDept.SelectedItem = ((ObservableCollection<CP_DepartmentList>)autoCompleteBoxDept.ItemsSource).FirstOrDefault(cp => cp.Ksdm.Equals(CP_DiagNurTemplateProptery.Zxksdm));
                    }
                    //Iskx为是否可选:0 表示必选，1表示可选
                    if (CP_DiagNurTemplateProptery.Iskx == null ||CP_DiagNurTemplateProptery.Iskx == "0")
                    {
                        this.radbx.IsChecked = true;
                        this.radkx.IsChecked = false;
                    }
                    else
                    {
                        this.radbx.IsChecked = false;
                        this.radkx.IsChecked = true;
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        /// <summary>
        /// 清空控件，暂时PUBLIC，最好改成属性CHANGED 事件触发
        /// </summary>
        public void NewAdviceGroupDetail()
        {
            try
            {
                this.ManualType = Helpers.ManualType.New;//
                CP_DiagNurTemplateProptery = new CP_DiagNurTemplate();
                //cbxZlHlLB.SelectedValue = null;
                autoCompleteBoxDigNur.SelectedItem = null;
                autoCompleteBoxDigNur.Text = "";
                nudMDSL.Value = 0;
                //cbxMDDW.Text = "";
                //CurrentState = PageState.New;
                txtUnitName.Text = string.Empty;
                
                //cbxPC.SelectedValue = null;
                txtZTNR.Text = "";
                zdm.Clear();
                zxsj.Clear();
                zdm = new List<int>();
                zxsj = new List<string>();
                cbxZlHlLB.IsEnabled = true;
                cbxJJLX.SelectedIndex = 0;
                autoCompleteBoxDept.SelectedItem = null;
                autoCompleteBoxDept.Text = "";
                autoCompleteBoxDigNur.IsEnabled = true;

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
                //autoCompleteBoxDigNur.Text = string.Empty;
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }


        #region KEYUP
        /// <summary>
        /// 注册KEYUP
        /// </summary>
        private void RegisterKeyEvent()
        {
            cbxZlHlLB.KeyUp += new KeyEventHandler(cbxZlHlLB_KeyUp);
            radbx.KeyUp += new KeyEventHandler(radbx_KeyUp);
            radkx.KeyUp += new KeyEventHandler(radkx_KeyUp);
            autoCompleteBoxDigNur.KeyUp += new KeyEventHandler(autoCompleteBoxDigNur_KeyUp);
            //nudMDSL.KeyUp += new KeyEventHandler(nudMDSL_KeyUp);
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
        private void cbxZlHlLB_KeyUp(object sender, KeyEventArgs e)
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
                autoCompleteBoxDigNur.Focus();
        }

        /// <summary>
        /// 项目
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void autoCompleteBoxDigNur_KeyUp(object sender, KeyEventArgs e)
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
     
        /// <summary>
        /// 备注
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

        //当诊疗护理类型切换后，在（内存）加载不同的数据源
        private void cbxZlHlLB_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (cbxZlHlLB.SelectedItem != null && m_CP_DiagNurTemplateCollection!=null)
            {
                 
                //InitDrugInfo();
                //m_CP_DiagNurTemplateCollection = (ObservableCollection<CP_DiagNurExecCategoryDetail>)m_CP_DiagNurTemplateCollection.Select(s => s).Where(s => s.Lbxh == int.Parse(cbxZlHlLB.SelectedValue.ToString()));
                autoCompleteBoxDigNur.ItemsSource = m_CP_DiagNurTemplateCollection.Select(s => s).Where(s => s.Lbxh == int.Parse(cbxZlHlLB.SelectedValue.ToString())).ToList();
                
            }
        }

    }
}
