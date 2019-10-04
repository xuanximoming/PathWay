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
using YidanEHRApplication.DataService;
using YidanSoft.Tool;

namespace YidanEHRApplication.NurModule.UserControls
{
    public partial class UCCheckBoxes : UserControl
    {
        #region 事件
        /// <summary>
        /// 窗体加载事件
        /// </summary>
        private void UCCheckBoxes_Loaded(object sender, RoutedEventArgs e)
        {
            this.DataContext = m_NurExecInfo;
            if (m_NurExecInfo.IsChecked)
            {
                this.ckbResult.IsChecked = true;
            }
        }
        /// <summary>
        /// checkbox选择事件
        /// </summary>
        private void CheckBoxes_Checked(object sender, RoutedEventArgs e)
        {
            CP_NurExecuteResult item = ((CheckBox)sender).Tag as CP_NurExecuteResult;
            m_NurExecInfo.Yxjl = "1";
        }
        
        /// <summary>
        /// checkbox取消选择事件
        /// </summary>
        private void CheckBoxes_Unchecked(object sender, RoutedEventArgs e)
        {
            CP_NurExecuteResult item = ((CheckBox)sender).Tag as CP_NurExecuteResult;
            m_NurExecInfo.Yxjl = "0";
        }
        #endregion
        #region 方法
        /// <summary>
        /// 构造函数UCCheckBoxes
        /// </summary>
        public UCCheckBoxes()
        {
            InitializeComponent();
        }
        /// <summary>
        /// 构造函数UCCheckBoxes
        /// </summary>
        /// <param name="info">CP_NurExecInfo</param>
        public UCCheckBoxes(CP_NurExecuteResult info)
        {
            InitializeComponent();
            m_NurExecInfo = info;
            this.Loaded += new RoutedEventHandler(UCCheckBoxes_Loaded);
        }
        #endregion
        #region 变量
        private CP_NurExecuteResult m_NurExecInfo;
        public CP_NurExecuteResult NurExecInfo
        {
            get { return m_NurExecInfo; }
        }
        #endregion
    }
}
