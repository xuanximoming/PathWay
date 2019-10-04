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
using Telerik.Windows.Controls.GridView;
using System.Drawing;
using System.Text;
using YidanEHRApplication.WorkFlow.Designer;
using System.Xml.Linq;
using System.IO;
using YidanEHRApplication.Helpers;
using Telerik.Windows.Controls;
namespace YidanEHRApplication.Views
{
    public partial class QueryPathExecute : Page
    {

        #region 初始化
        YidanEHRDataServiceClient serviceCon;
        public QueryPathExecute()
        {
            InitializeComponent();
            btnDuiZhao.Visibility = Visibility.Collapsed;
        }
        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            dpkStartDate.SelectedDate = DateTime.Now.AddDays(-10);
            dpkEndDate.SelectedDate = DateTime.Now;
            BindcbxPathName();
            BindcbxPathState();
        }

        #endregion
        #region 属性
        String compareNoteGUID = "";//选中要比较的节点的GUID
        Boolean CanCompare = false;//是否满足比较的条件
        String RyzdName = "";//入院诊断
        #endregion
        #region 事件
        private void btnQuery_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                StringBuilder where = new StringBuilder();
                where.Append(" where 1=1 ");
                if (cbxPathState.SelectedItem != null && cbxPathState.SelectedValue.ToString() != "-1")
                    where.AppendFormat(" and V_QueryPathExecute.Ljzt={0}", cbxPathState.SelectedValue);
                if (cbxPathName.SelectedItem != null && cbxPathName.SelectedValue.ToString() != "-1")
                    where.AppendFormat(" and V_QueryPathExecute.ljdm='{0}'", cbxPathName.SelectedValue);
                //只要在这个时间段内进入过路径
                where.Append(" and (");
                where.AppendFormat("  (V_QueryPathExecute.Jrsj >= '{0}' and V_QueryPathExecute.Jrsj<'{1}' )"
                    , new string[2] { ((DateTime)dpkStartDate.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss"), ((DateTime)dpkEndDate.SelectedDate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") });

                where.AppendFormat(" or (V_QueryPathExecute.tcsj >= '{0}' and V_QueryPathExecute.tcsj<'{1}' )"
                    , new string[2] { ((DateTime)dpkStartDate.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss"), ((DateTime)dpkEndDate.SelectedDate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") });

                where.AppendFormat(" or (V_QueryPathExecute.wcsj >= '{0}' and V_QueryPathExecute.wcsj<'{1}' )"
                    , new string[2] { ((DateTime)dpkStartDate.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss"), ((DateTime)dpkEndDate.SelectedDate).AddDays(1).ToString("yyyy-MM-dd HH:mm:ss") });

                where.AppendFormat(" or (V_QueryPathExecute.Jrsj <= '{0}' and V_QueryPathExecute.tcsj>='{1}' )"
      , new string[2] { ((DateTime)dpkStartDate.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss"), ((DateTime)dpkEndDate.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss") });

                where.AppendFormat(" or (V_QueryPathExecute.Jrsj <= '{0}' and V_QueryPathExecute.wcsj>='{1}' )"
      , new string[2] { ((DateTime)dpkStartDate.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss"), ((DateTime)dpkEndDate.SelectedDate).ToString("yyyy-MM-dd HH:mm:ss") });


                where.Append(" ) ");
                BindGridView(where.ToString());
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void GridViewPathExecute_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
            CP_QueryPathExecute pe = (CP_QueryPathExecute)e.DataElement;
            if (pe.Ljzt == "2")
            {
                e.Row.Background = new SolidColorBrush(Colors.Red);
            }
        }
        private void GridViewPathExecute_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {try{
            if (GridViewPathExecute.SelectedItem == null) return;
            CP_QueryPathExecute p = (CP_QueryPathExecute)GridViewPathExecute.SelectedItem;
            LoadWorkFlow(p);
            btnDuiZhao.Visibility = Visibility.Visible;
            if (CanCompare) btnDuiZhao.IsEnabled = true;
            else btnDuiZhao.IsEnabled = false;
            RyzdName = p.RyzdName; }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }
        private void btnDuiZhao_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (CanCompare)
                {

                    new RWQueryPathExecuteNoteCompare(compareNoteGUID, RyzdName).ShowDialog();

                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region 函数
        public void BindcbxPathState()
        {
            List<Item> i = new List<Item>();
            i.Add(new Item("全部", "-1"));
            i.Add(new Item("执行中", "1"));
            i.Add(new Item("推出", "2"));
            i.Add(new Item("已完成", "3"));
            cbxPathState.ItemsSource = i;
            cbxPathState.SelectedValuePath = "Value";

        }
        /// <summary>
        /// 绑定路径名下拉框
        /// </summary>
        public void BindcbxPathName()
        {

            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetClinicalPathListCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error != null) PublicMethod.RadWaringBox(e.Error);
                        else
                        {
                            List<CP_ClinicalPathList> pList = new List<CP_ClinicalPathList>();
                            CP_ClinicalPathList p = new CP_ClinicalPathList();
                            p.Ljdm = "-1";
                            p.Name = "全部";
                            pList.Add(p);
                            pList.AddRange(e.Result.ToList());
                            cbxPathName.ItemsSource = pList;
                        }
                    };
            serviceCon.GetClinicalPathListAsync(string.Empty, string.Empty, string.Empty, string.Empty,string.Empty,string.Empty);
            serviceCon.CloseAsync();
        }
        public void BindGridView(String where)
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetQueryPathExecuteListCompleted +=
                    (obj, e) =>
                    {
                        if (e.Error != null) PublicMethod.RadWaringBox(e.Error);
                        else
                            GridViewPathExecute.ItemsSource = e.Result.ToList();
                    };
            serviceCon.GetQueryPathExecuteListAsync(where);
            serviceCon.CloseAsync();
        }
                

                

        ContainerShow containerShow;
        ContainerShow containerShowExe;
        private void LoadWorkFlow(CP_QueryPathExecute p)
        {
            #region 路径
            containerShow = new ContainerShow();
            containerShow.WorkFlowXML = p.WorkFlowXML;
            gridWorkFlow.Children.Add(containerShow);
            containerShow.AfterSelectActivityEvent += new ContainerShow.AfterSelectActivityEventHandler(containerShow_AfterSelect);

            #endregion
            #region 执行路径
            containerShowExe = new ContainerShow();
            containerShowExe.WorkFlowXML = p.EnFroceXml;
            gridWorkFlowEnForceXml.Children.Add(containerShowExe);
            containerShowExe.AfterSelectActivityEvent += new ContainerShow.AfterSelectActivityEventHandler(containerShowExe_AfterSelect);
            #endregion
        }
        private void containerShow_AfterSelect(object sender, ActivitySelectEventArgs e)
        {
            Boolean CanCompareTemp = false;
            foreach (Activity a in containerShowExe.ActivityCollections)
            {
                ;
                if (e.SelectAcivity.SelectID == a.UniqueID)
                {
                    a.IsSelectd = true;
                    CanCompareTemp = true;
                    compareNoteGUID = a.UniqueID;
                }
                else
                {
                    a.IsSelectd = false;
                }
            }
            CanCompare = CanCompareTemp;
            btnDuiZhao.IsEnabled = CanCompareTemp;
            compareNoteGUID = CanCompareTemp ? compareNoteGUID : "";

        }
        private void containerShowExe_AfterSelect(object sender, ActivitySelectEventArgs e)
        {
            Boolean CanCompareTemp = false;
            foreach (Activity a in containerShow.ActivityCollections)
            {
                if (e.SelectAcivity.SelectID == a.UniqueID)
                {
                    a.IsSelectd = true;
                    CanCompareTemp = true;
                    compareNoteGUID = a.UniqueID;
                }
                else
                {
                    a.IsSelectd = false;
                }
            }
            CanCompare = CanCompareTemp;
            btnDuiZhao.IsEnabled = CanCompareTemp;
            compareNoteGUID = CanCompareTemp ? compareNoteGUID : "";
        }


        /// <summary>
        /// 窗体关闭后更新主界面XML
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        #endregion
    }
    #region 内部类
    public class Item
    {
        private string text;

        public string Text
        {
            get { return text; }
            set { text = value; }
        }
        private string value;

        public string Value
        {
            get { return this.value; }
            set { this.value = value; }
        }
        public Item(string txt, string val)
        {
            Value = val;
            Text = txt;
        }
    }
    #endregion
}
