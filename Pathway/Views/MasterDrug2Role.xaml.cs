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
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls.GridView;
using YidanSoft.Tool;
using YidanEHRApplication.DataService;
namespace YidanEHRApplication.Views
{
    public partial class MasterDrug2Role : Page
    {
        // PE_Role m_pe_role = new PE_Role();
        YidanEHRDataServiceClient serviceCon;
        public MasterDrug2Role()
        {
            InitializeComponent();
        }
        #region 事件
        // 当用户导航到此页面时执行。
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            AccessDataBase(Operation.Select);
            CurrentState = OperationState.VIEW;
        }
        /// <summary>
        /// 选中角色
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            //try
            //{
            if (GridView.SelectedItem != null)
            {
                AccessDataBase((CP_MasterDrugRoles)GridView.SelectedItem, Operation.Select);
            }
            //}
            //catch (Exception ex)
            //{
            //    YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            //}
        }
        /// <summary>
        /// 将功能信息保存到数据库中
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {

                if (GridView.SelectedItems != null && GridView.SelectedItems.Count > 0)
                {
                    ObservableCollection<CP_MasterDrug2Role> Drug2Roles = new ObservableCollection<CP_MasterDrug2Role>();
                    CP_MasterDrugRoles role = (CP_MasterDrugRoles)(GridView.SelectedItem);
                    foreach (var item in GridViewFun.SelectedItems)
                    {
                        CP_MasterDrugs Drug = (CP_MasterDrugs)item;
                        Drug2Roles.Add(new CP_MasterDrug2Role()
                        {
                            Jsbm = role.Jsbm,
                            Cdxh = Drug.Cdxh,
                            ZgdmCj = Global.LogInEmployee.Zgdm,
                            ZgdmXg = Global.LogInEmployee.Zgdm,
                            Cjsj = DateTime.Now.ToString(),
                            Xgsj = DateTime.Now.ToString(),

                        });
                    }
                    YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
                    Client.MaintainCP_MasterDrug2RoleUpdateCompleted += (sb, eb) => {

                        PublicMethod.RadAlterBox("保存成功！", "提示");
                    
                    };
                    Client.MaintainCP_MasterDrug2RoleUpdateAsync(role.Jsbm, Drug2Roles);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        private void GridViewFun_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            try
            {
                if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
                CP_MasterDrugs t = (CP_MasterDrugs)e.DataElement;
                List<CheckBox> listtest = (List<CheckBox>)(this.GridViewFun.ChildrenOfType<CheckBox>().ToList());
                if (listtest.Count > 1)
                    if (t.IsCheck == "1")
                        listtest[listtest.Count - 1].IsChecked = true;

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }

        #endregion

        #region 函数
        OperationState _CurrentState = OperationState.VIEW;
        OperationState CurrentState
        {
            get { return _CurrentState; }
            set
            {
                _CurrentState = value;
                // btnClear.IsEnabled = value != OperationState.VIEW;
                //  btnSave.IsEnabled = value != OperationState.VIEW;
            }
        }
        public void AccessDataBase(Operation state)
        {
            YidanEHRDataServiceClient Client = PublicMethod.YidanClient;
            Client.MaintainCP_MasterDrugRolesCompleted += (s, e) =>
            {
                GridView.ItemsSource = e.Result;
                CurrentState = OperationState.VIEW;
            };
            Client.MaintainCP_MasterDrugRolesAsync(null, state.ToString());
        }
        public void AccessDataBase(CP_MasterDrugRoles paramater, Operation state)
        {
            YidanEHRDataServiceClient ClientBegin = PublicMethod.YidanClient;
            ClientBegin.MaintainCP_MasterDrugCompleted += (sb, eb) =>
            {

                if (eb.Error == null)
                {
                    GridViewFun.ItemsSource = eb.Result.ToList(); ;
                }
            };
            ClientBegin.MaintainCP_MasterDrugAsync(new CP_MasterDrugs() { Jsbm = paramater.Jsbm }, Operation.Select.ToString());
        }
        #endregion




    }
}
