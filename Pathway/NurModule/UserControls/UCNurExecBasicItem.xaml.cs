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

namespace YidanEHRApplication.NurModule.UserControls
{
    public partial class UCNurExecBasicItem : UserControl
    {
        #region 事件
        /// <summary>
        /// 页面加载事件
        /// </summary>
        private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (m_ContentLoaded)
                {
                    return;
                }
                m_ContentLoaded = true;
                InitControl();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region 方法

        /// <summary>
        /// 构造函数UCNurExecBasicItem
        /// </summary>
        public UCNurExecBasicItem()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 构造函数UCNurExecBasicItem
        /// </summary>
        /// <param name="info"></param>
        public UCNurExecBasicItem(ObservableCollection<CP_NurExecInfo> info)
        {
            InitializeComponent();
            m_NurExecInfo = info;
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            foreach (CP_NurExecInfo info in m_NurExecInfo)
            {
                this.txtCategoryName.Text = info.LbxhName;
                this.txtCategoryName.FontSize = 14;
                this.txtCategoryName.FontWeight = FontWeights.Bold;
                UCNurExecChildItem childItem = new UCNurExecChildItem(info);
                this.gridCategoryDetail.Children.Add(childItem);
            }
        }
        #endregion
        #region 变量
        private ObservableCollection<CP_NurExecInfo> m_NurExecInfo;

        public ObservableCollection<CP_NurExecInfo> NurExecInfo
        {
            get { return m_NurExecInfo; }
        }
        private bool m_ContentLoaded;
        #endregion
    }
}
