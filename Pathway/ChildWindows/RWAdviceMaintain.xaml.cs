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
using YidanEHRApplication.Helpers;
using YidanSoft.Tool;
using Telerik.Windows.Controls;
using YidanEHRApplication.Models;
using YidanEHRApplication.DataService;
using System.Collections.ObjectModel;

namespace YidanEHRApplication.Views.ChildWindows
{
    /// <summary>
    /// Interaction logic for RadChildWindowDrug.xaml
    /// </summary>
    public partial class RWAdviceMaintain
    {
        #region 属性
        String _Ctyzxh = "";
        int _Xmlb = 2401;//项目类别
        int _Yzlb = 3100;//医嘱类别
        ManualType _type = ManualType.New;
        CP_AdviceSuitDetail _CP_AdviceSuitDetail = new CP_AdviceSuitDetail();
         
        //定义一个全局集合类型，用于从检验检测中取纯医嘱的数据源
        private ObservableCollection<CP_ChargingMinItem> m_CP_ChargingMinItemCollection;
        
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
        #endregion

        public EventHandler RefreshEventHandler;
        #region 事件
        /// <summary>
        /// 新增了一个参数  count  用于判断是哪个选项卡
        /// 修改：Jhonny
        /// 修改时间：2013年8月22日 15:33:07
        /// </summary>
        /// <param name="Ctyzxh"></param>
        /// <param name="type"></param>
        /// <param name="suitDetail"></param>
        public RWAdviceMaintain(String Ctyzxh, ManualType type, CP_AdviceSuitDetail suitDetail,int count)
        {
            try
            {
                
                InitializeComponent();
                //add by luff 20130826 去掉鼠标右键Silverlight
                SuitTab.MouseRightButtonDown += (sender, e) =>
                {
                    e.Handled = true;
                    //自己的菜单
                };
                _Ctyzxh = Ctyzxh;
                _type = type;
               
                //页面第一次进了初始化药品自定义控件
                if (_type == ManualType.New && count == 0)
                {
                    InitUserControl(type, suitDetail);
                }
                if (_type == ManualType.Edit)
                {
                    _CP_AdviceSuitDetail = suitDetail;
                    if (_CP_AdviceSuitDetail.Xmlb == "2417")
                    {
                        InitOtherControl(type, suitDetail);
                        radDrug.IsSelected = false;
                        radOther.IsSelected = true;
                        radChun.IsSelected = false;
                    }
                    else if (_CP_AdviceSuitDetail.Xmlb == "2424")
                    {
                        InitChunControl(type, suitDetail);
                        radDrug.IsSelected = false;
                        radOther.IsSelected = false;
                        radChun.IsSelected = true;
                    }
                    else
                    {
                        InitUserControl(type, suitDetail);
                        radDrug.IsSelected = true;
                        radOther.IsSelected = false;
                        radChun.IsSelected = false;
                    }
                  

                }
               
            }
            catch (Exception)
            {
                
                throw;
            }
        }
        #region 保存
        /// <summary>
        ///起到保存的作用，将药品、纯医嘱、其他医嘱都保存到CP_AdviceSuit表中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void btnMDQD_Click(object sender, CP_DoctorOrder e)
        {
            try
            {
                YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                if (_type == ManualType.New)
                {
                    ServiceClient.AddCP_AdviceSuitDetailCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error != null)
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                            else
                            {
                                
                                //InitUserControl(ManualType.New, null);/***** Update by dxj 2011/7/20 修改原因：重复添加医嘱 ********/
                                uc_Drug.NewAdviceGroupDetail();
                                uc_Chunorder.NewAdviceGroupDetail();
                                uc_Otherorder.NewAdviceGroupDetail();
                                PublicMethod.RadAlterBox("添加成功,可以继续添加！", "提示");
                            }
                        };

                    CP_DoctorOrder2CP_AdviceSuitDetail(e);
                    _CP_AdviceSuitDetail.Ctyzxh = ConvertMy.ToDecimal(_Ctyzxh);
                    //_CP_AdviceSuitDetail.Yzlb = "3100";
                    ServiceClient.AddCP_AdviceSuitDetailAsync(_CP_AdviceSuitDetail);
                    ServiceClient.CloseAsync();
                }
                else
                {
                    ServiceClient.EditCP_AdviceSuitDetailCompleted +=
                       (obj, ea) =>
                       {
                           if (ea.Error != null) PublicMethod.RadWaringBox(ea.Error);
                           else
                           {
                               PublicMethod.RadAlterBox("修改成功！", "提示");
                               uc_Drug.NewAdviceGroupDetail();
                               uc_Chunorder.NewAdviceGroupDetail();
                               uc_Otherorder.NewAdviceGroupDetail();
                               this.Close();
                           }
                       };
                    CP_DoctorOrder2CP_AdviceSuitDetail(e);
                    _CP_AdviceSuitDetail.Ctyzxh = ConvertMy.ToDecimal(_Ctyzxh);
                    //_CP_AdviceSuitDetail.Yzlb = "3100";
                    ServiceClient.EditCP_AdviceSuitDetailAsync(_CP_AdviceSuitDetail);
                    ServiceClient.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        
        #endregion
        #endregion
        #region 函数
        private void InitUserControl(ManualType type, CP_AdviceSuitDetail suitDetail)
        {
            try
            {
                uc_Drug.ManualType = type;
                _Xmlb = 2401;
                _Yzlb = 3100;
                // add by luff 20130823 初始化药品医嘱自定义控件
                uc_Drug.AfterDrugCinfirmeddEvent += new UserControls.UCDrug.DrugConfirmed(btnMDQD_Click);
                uc_Drug.AfterDrugLoadedEvent += new UserControls.UCDrug.DrugLoaded(uc_Drug_Loaded);
                if (type == ManualType.Edit)
                {
                   
                    // add by luff 20130823 修改药品嘱控件赋值
                    uc_Drug.ManualType = ManualType.Edit;
                    uc_Drug.CP_AdviceGroupDetailProptery = CP_AdviceSuitDetail2CP_DoctorOrder(suitDetail);
                    //uc_Drug.InitModifyOrder();
                }
                else
                {
                    uc_Drug.NewAdviceGroupDetail();
                }
               
            }
            catch (Exception)
            {
                
                throw;
            }

        }

        /// <summary>
        /// 保存纯医嘱的加载项
        /// 创建:Jhonny
        /// 创建时间：2013年8月22日 14:57:37
        /// </summary>
        /// <param name="type"></param>
        /// <param name="suitDetail"></param>
        private void InitChunControl(ManualType type, CP_AdviceSuitDetail suitDetail)
        {
            try
            {
                uc_Chunorder.ManualType = type;
                uc_Chunorder.PanelCategory = OrderPanelBarCategory.ChunOrder;
                uc_Chunorder.OrderCategory = OrderItemCategory.ChunOrder;
                _Xmlb = 2424;
                _Yzlb = 3119;
                // mod by luff 20130823 先撤销事件，再注册事件，初始化纯医嘱自定义控件；解决多次弹出修改成功或保存成功的bug
                uc_Chunorder.AfterDrugCinfirmeddEvent -= new UserControls.UCChunOrder.DrugConfirmed(btnMDQD_Click);
                uc_Chunorder.AfterDrugLoadedEvent -= new UserControls.UCChunOrder.DrugLoaded(uc_Chunorder_Loaded);
                uc_Chunorder.AfterDrugCinfirmeddEvent += new UserControls.UCChunOrder.DrugConfirmed(btnMDQD_Click);
                uc_Chunorder.AfterDrugLoadedEvent += new UserControls.UCChunOrder.DrugLoaded(uc_Chunorder_Loaded);
                if (type == ManualType.Edit)
                {
                    // add by luff 20130823 修改纯医嘱控件赋值
                    uc_Chunorder.ManualType = ManualType.Edit;
                    uc_Chunorder.CP_AdviceGroupDetailProptery = CP_AdviceSuitDetail2CP_DoctorOrder(suitDetail);
                    //uc_Chunorder.InitModifyOrder();
                }
                else
                {
                    uc_Chunorder.NewAdviceGroupDetail();
                }
            }
            catch (Exception)
            {
                
                throw;
            }
        }

        /// <summary>
        /// 保存其他医嘱的加载项
        /// 创建：Jhonny
        /// 创建时间：2013年8月22日 14:57:49
        /// </summary>
        /// <param name="type"></param>
        /// <param name="suitDetail"></param>
        private void InitOtherControl(ManualType type, CP_AdviceSuitDetail suitDetail)
        {
            try
            {
                uc_Otherorder.ManualType = type;
                uc_Otherorder.PanelCategory = OrderPanelBarCategory.Other;
                uc_Otherorder.OrderCategory = OrderItemCategory.Other;
                _Xmlb = 2417;
                _Yzlb = 3120;
                // mod by luff 20130826 先撤销事件，再注册事件，初始化其他医嘱自定义控件；解决多次弹出修改成功或保存成功的bug
                uc_Otherorder.AfterDrugCinfirmeddEvent -= new UserControls.UCOtherOrder.DrugConfirmed(btnMDQD_Click);
                uc_Otherorder.AfterDrugCinfirmeddEvent += new UserControls.UCOtherOrder.DrugConfirmed(btnMDQD_Click);
                uc_Otherorder.AfterDrugLoadedEvent -= new UserControls.UCOtherOrder.DrugLoaded(uc_Otherorder_Loaded);
                uc_Otherorder.AfterDrugLoadedEvent += new UserControls.UCOtherOrder.DrugLoaded(uc_Otherorder_Loaded);
                if (type == ManualType.Edit)
                {
                    // add by luff 20130823 修改其他医嘱控件赋值
                    uc_Otherorder.ManualType = ManualType.Edit;
                    uc_Otherorder.CP_AdviceGroupDetailProptery = CP_AdviceSuitDetail2CP_DoctorOrder(suitDetail); 
                    //uc_Otherorder.InitModifyOrder();
                }
                else
                {
                    uc_Otherorder.NewAdviceGroupDetail();
                }
            }
            catch (Exception)
            {

                throw;
            }
        }


        #region add by luff 20130823 初始化此事件的用是为了能让控件直接在界面上显示

        void uc_Drug_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }
         
        void uc_Chunorder_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }
        
        void uc_Otherorder_Loaded(object sender, RoutedEventArgs e)
        {
            //throw new NotImplementedException();
        }

        #endregion

        private CP_DoctorOrder CP_AdviceSuitDetail2CP_DoctorOrder(CP_AdviceSuitDetail s)
        {
            CP_DoctorOrder d = new CP_DoctorOrder();
            d.Yzbz = ConvertMy.ToDecimal(s.Yzbz);
            d.Jldw = s.Jldw;
            d.Ypdm = s.Ypdm;
            d.Ypjl = ConvertMy.ToDecimal(s.Ypjl);/* Edit by dxj 2011/7/23 修改原因：赋值错误 */
            d.Yfdm = s.Yfdm;
            d.Pcdm = s.Pcdm;
            d.Ypjl = ConvertMy.ToDecimal(s.Ypjl);
            d.Ypjl = ConvertMy.ToDecimal(s.Ypjl);
            d.Ztnr = s.Ztnr;
            d.Xmlb = ConvertMy.ToDecimal(_Xmlb);
            d.Yzlb = ConvertMy.ToDecimal(_Yzlb);
            d.Jjlx = s.Jjlx;
            d.Zxksdm = s.Zxksdm;
            d.Yzkx = s.Yzkx;
            return d;                      
        }
        private CP_AdviceSuitDetail CP_DoctorOrder2CP_AdviceSuitDetail(CP_DoctorOrder d)
        {

            _CP_AdviceSuitDetail.Cdxh = ConvertMy.ToString(d.Cdxh);
            _CP_AdviceSuitDetail.Ggxh = ConvertMy.ToString(d.Ggxh);
            _CP_AdviceSuitDetail.Lcxh = ConvertMy.ToString(d.Lcxh);
            _CP_AdviceSuitDetail.Ypdm = ConvertMy.ToString(d.Ypdm);
            _CP_AdviceSuitDetail.Xmlb = ConvertMy.ToString(d.Xmlb);

            _CP_AdviceSuitDetail.Yzbz = ConvertMy.ToString(d.Yzbz);
            _CP_AdviceSuitDetail.Ypjl = ConvertMy.ToString(d.Ypjl);
            _CP_AdviceSuitDetail.Jldw = d.Jldw;
            _CP_AdviceSuitDetail.Jldw = d.Jldw;
            _CP_AdviceSuitDetail.Yfdm = d.Yfdm;

            _CP_AdviceSuitDetail.Pcdm = d.Pcdm;
            _CP_AdviceSuitDetail.Ypmc = d.Ypmc;
            _CP_AdviceSuitDetail.Fzbz = ConvertMy.ToString(d.Fzbz);
            _CP_AdviceSuitDetail.Zxdw = d.Zxdw;
            _CP_AdviceSuitDetail.Dwxs = ConvertMy.ToString(d.Dwxs);

            _CP_AdviceSuitDetail.Dwlb = ConvertMy.ToString(d.Dwlb);
            _CP_AdviceSuitDetail.Zxcs = ConvertMy.ToString(d.Zxcs);
            _CP_AdviceSuitDetail.Zxzq = ConvertMy.ToString(d.Zxzq);
            _CP_AdviceSuitDetail.Zxzqdw = ConvertMy.ToString(d.Zxzqdw);
            _CP_AdviceSuitDetail.Zxsj = d.Zxsj;

            _CP_AdviceSuitDetail.Fzxh = ConvertMy.ToString(d.Fzxh);

            _CP_AdviceSuitDetail.Xmlb = _Xmlb.ToString();
            _CP_AdviceSuitDetail.Yzlb =_Yzlb.ToString();; //ConvertMy.ToString(d.Yzlb);
            _CP_AdviceSuitDetail.Zxts = ConvertMy.ToString(d.Zxts);
            _CP_AdviceSuitDetail.Ypzsl = ConvertMy.ToString(d.Ypzsl);
            _CP_AdviceSuitDetail.Ztnr = d.Ztnr;
            _CP_AdviceSuitDetail.Yznr = d.Yznr;
            _CP_AdviceSuitDetail.Jjlx = d.Jjlx;
            _CP_AdviceSuitDetail.Zxksdm = d.Zxksdm;
            _CP_AdviceSuitDetail.Yzkx = d.Yzkx;
            return _CP_AdviceSuitDetail;

        }

        #endregion

        /// <summary>
        /// 切换选项卡时加载项
        /// 创建： Jhonny
        /// 时间：2013年8月20日 14:15:37
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radTabControlManager_SelectionChanged(object sender, RadSelectionChangedEventArgs e)
        {
            try
            {
                
                RadTabControl tabControl = (RadTabControl)sender;

                //当选项卡选择为第一个药品选项卡的时候
                //if (tabControl.SelectedIndex == 0)
                //{
                //    InitUserControl(_type, _CP_AdviceSuitDetail);
                //}

                //当选项卡选择为第二个纯医嘱选项卡的时候(纯医嘱)
                if (tabControl.SelectedIndex == 1)
                {
                    if (radChun.IsSelected == true)
                    {
                        if (_CP_AdviceSuitDetail.Xmlb == "2424")
                        {
                            InitChunControl(_type, _CP_AdviceSuitDetail);
                        }
                        else
                        {
                            _CP_AdviceSuitDetail = new CP_AdviceSuitDetail();
                            InitChunControl(_type, _CP_AdviceSuitDetail);
                        }
                    }
                   

                }
                //当选项卡选中第三个选项卡的其他医嘱的时候(其他医嘱)
                if (tabControl.SelectedIndex == 2)
                {
                    if (radOther.IsSelected == true)
                    {
                        if (_CP_AdviceSuitDetail.Xmlb == "2417")
                        {
                            InitOtherControl(_type, _CP_AdviceSuitDetail);
                        }
                        else
                        {
                            _CP_AdviceSuitDetail = new CP_AdviceSuitDetail();
                            InitOtherControl(_type, _CP_AdviceSuitDetail);
                        }
                    }
                     
                     
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
    }
}
