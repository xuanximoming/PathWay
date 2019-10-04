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
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using Telerik.Windows.Controls.GridView;

namespace YidanEHRApplication.Views.Permission
{
    public partial class RoleFunManager : Page
    {

        PE_Role m_pe_role = new PE_Role();

        YidanEHRDataServiceClient serviceCon;
        public RoleFunManager()
        {
            InitializeComponent();
            Loaded += new RoutedEventHandler(RoleFunManager_Loaded);
        }

        void RoleFunManager_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                BindGridView();
                BindGridViewFun(new PE_Role());
                this.GridViewFun.ItemsSource = new List<PE_Fun>();
                this.btnSave.IsEnabled = false;
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

            //绑定功能列表
            //BindGridViewFun();
        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }


        private void BindGridView()
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetPE_RoleListCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        GridView.ItemsSource = e.Result;

                        this.btnSave.IsEnabled = true;
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };
            serviceCon.GetPE_RoleListAsync();
            serviceCon.CloseAsync();
        }



        /// <summary>
        /// 绑定功能模块数据源 add by luff 2012-09-12
        /// </summary>
        /// <param name="pe_role">角色实体类对象</param>
        private void BindGridViewFun(PE_Role pe_role)
        {
            serviceCon = PublicMethod.YidanClient;
            serviceCon.GetPE_FunListByRoleCodeCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        GridViewFun.ItemsSource = e.Result;
                    }
                    else
                    {
                        PublicMethod.RadWaringBox(e.Error);
                    }
                };

            serviceCon.GetPE_FunListByRoleCodeAsync(pe_role.RoleCode);
            serviceCon.CloseAsync();
        }



        /// <summary>
        /// 选中角色事件方法 add by luff 2012-09-12
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridView_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangeEventArgs e)
        {
            try
            {
                if (GridView.SelectedItem != null)
                {
                    m_pe_role = (PE_Role)GridView.SelectedItem;


                    BindGridViewFun(m_pe_role);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

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
                if (GridView.SelectedItem == null)
                {
                    PublicMethod.RadAlterBox("请选择对应角色！","提示");
                    return;
                }
                System.Collections.ObjectModel.ObservableCollection<PE_RoleFun> rolelist = new System.Collections.ObjectModel.ObservableCollection<PE_RoleFun>();
                //将角色功能信息保存到列表中
                foreach (object obj in GridViewFun.SelectedItems)
                {
                    PE_Fun pe_fun = (PE_Fun)obj;

                    PE_RoleFun rolefun = new PE_RoleFun();
                    rolefun.FunCode = pe_fun.FunCode;
                    rolefun.RoleCode = m_pe_role.RoleCode;

                    rolelist.Add(rolefun);
                }

                if (rolelist.Count > 0)
                {
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.InsertPE_RoleFunCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                SQLMessage mess = ea.Result;
                                if (mess.IsSucceed)
                                {
                                    RoleFunManager_Loaded(null, null);
                                }
                                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.InsertPE_RoleFunAsync(rolelist);
                    serviceCon.CloseAsync();

                }
                //当角色无对应功能权限时为删除该角色对应的功能权限
                else
                {
                    serviceCon = PublicMethod.YidanClient;
                    serviceCon.DelPE_RoleFunCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                SQLMessage mess = ea.Result;
                                if (mess.IsSucceed)
                                {
                                    RoleFunManager_Loaded(null, null);
                                }
                                PublicMethod.RadAlterBox(mess.Message.ToString(), "提示");
                            }
                            else
                            {
                                PublicMethod.RadWaringBox(ea.Error);
                            }
                        };
                    serviceCon.DelPE_RoleFunAsync(m_pe_role.RoleCode);
                    serviceCon.CloseAsync();
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }





        /// <summary>
        /// 绑定功能模块数据
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void GridViewFun_RowLoaded(object sender, Telerik.Windows.Controls.GridView.RowLoadedEventArgs e)
        {
            try
            {
                if ((e.Row is GridViewHeaderRow) || (e.Row is GridViewNewRow)) return;
                PE_Fun t = (PE_Fun)e.DataElement;
                List<CheckBox> listtest = (List<CheckBox>)(this.GridViewFun.ChildrenOfType<CheckBox>().ToList());
                if (listtest.Count > 1)
                    if (t.ISCheck == 1)
                        listtest[listtest.Count - 1].IsChecked = true;

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
    }
}
