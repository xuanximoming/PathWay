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

using YidanEHRApplication.Models;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.NurModule.UserControls
{
    public partial class UCNurExecChildItem : UserControl
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
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }
        #endregion
        #region 方法
        /// <summary>
        /// 构造函数UCNurExecChildItem
        /// </summary>
        public UCNurExecChildItem()
        {
            InitializeComponent();
        }
        public UCNurExecChildItem(CP_NurExecInfo info)
        {

            InitializeComponent();
            m_NurExecInfo = info;
            InitControl();
        }

        /// <summary>
        /// 初始化控件
        /// </summary>
        private void InitControl()
        {
            this.txtCategoryName.Text = m_NurExecInfo.MxxhName + ":";
            this.txtCategoryName.FontSize = 12;
            if (m_NurExecInfo.ResultList != null)
            {
                foreach (CP_NurExecuteResult info in m_NurExecInfo.ResultList)
                {
                    UCCheckBoxes checkBoxes = new UCCheckBoxes(info);
                    this.gridCategoryDetail.Children.Add(checkBoxes);
                    if (m_NurExecInfo.RecordResultList != null)
                    {
                        foreach (CP_NurExecRecordResult item in m_NurExecInfo.RecordResultList)
                        {
                            if (item.JgId == info.Jgbh && item.Yxjl == "1")
                            {
                                info.IsChecked = true;
                            }
                        }
                    }
                }
            }
        }
        #endregion
        #region 变量

        private CP_NurExecInfo m_NurExecInfo;
        public CP_NurExecInfo NurExecInfo
        {
            get { return m_NurExecInfo; }
        }

        private bool m_ContentLoaded;
        #endregion

    }
}
