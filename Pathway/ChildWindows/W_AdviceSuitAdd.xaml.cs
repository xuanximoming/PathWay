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
using YidanEHRApplication.Models;
using YidanSoft.Tool;
using Telerik.Windows.Controls;
using System.Collections.ObjectModel;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Views.ChildWindows
{
    public partial class W_AdviceSuitAdd
    {
        String editState;
        String Syfw;
        CP_AdviceSuit adviceSuit;
        CP_AdviceSuitCategory cP_AdviceSuitCategory=null;
        public W_AdviceSuitAdd(String EditState,String syfw,object item)
        {
            cP_AdviceSuitCategory = (CP_AdviceSuitCategory)((RadTreeViewItem)item).Tag;
            Syfw = syfw;
            editState = EditState;
            InitializeComponent();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
        }

        private void RadWindow_Loaded(object sender, RoutedEventArgs e)
        {
           // GetCP_AdviceSuitType(); 
            this.Header = editState + "医嘱套餐";
            txtCategroy.Text = cP_AdviceSuitCategory.Name;
        }
        /// <summary>
        /// 获取医嘱套餐类别
        /// </summary>
    

        private void btnSave_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (txtName.Text.Trim() == String.Empty)
                {
                    PublicMethod.RadAlterBox("套餐名称不能为空！", "提示");
                    return;
                }
                YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                CP_AdviceSuit suit = new CP_AdviceSuit();
                suit.Name = txtName.Text;
                suit.Syfw = Syfw;
                if (suit.Syfw.Equals("2903"))
                    suit.Ysdm = Global.LogInEmployee.Zgdm;
                suit.Zgdm = Global.LogInEmployee.Zgdm;
                suit.Ksdm = Global.LogInEmployee.Ksdm;
                suit.Bqdm = Global.LogInEmployee.Bqdm;
                suit.CategoryId = ConvertMy.ToString(cP_AdviceSuitCategory.CategoryId);
                ServiceClient.AddCP_AdviceSuitCompleted +=
                        (obj, ea) =>
                        {
                            if (ea.Error == null)
                            {
                                if (ea.Result > 0)
                                {
                                    PublicMethod.RadAlterBox("成功！", "提示");
                                    txtName.Text = String.Empty;
                                    this.Close();
                                }
                                else
                                {
                                    PublicMethod.RadAlterBox("该套餐名称已存在！", "提示");
                                    txtName.Text = String.Empty;
                                
                                }
                            }
                        };
                ServiceClient.AddCP_AdviceSuitAsync(suit);
                ServiceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        /// <summary>
        /// 创建树
        /// </summary>
        /// <param name="parentId">父类编号</param>
        /// <param name="subitem">当前树节点</param>
        /// <param name="CategoryList">数据源</param>
        /// <param name="sunCategoryList">符合条件的数据源</param>
      
        #region 变量
    
        #endregion
    }
}

