using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Yidansoft.Service.Entity
{
   /// <summary>
   /// 构造下拉框显示数据类
   /// </summary>
    public class CP_PCSJ_E
    {
       /// <summary>
       /// 是否周代码
       /// </summary>
        private string _isweek;
        public string IsWeek
        {
            get { return _isweek; }
            set { _isweek = value; }
        }
        /// <summary>
        /// 显示名称如00，01
        /// </summary>
        private string _displayName;
        public string DisplayName
        {
            get { return _displayName; }
            set { _displayName = value; }
        }
        /// <summary>
        /// 关闭下拉框后显示值
        /// </summary>
        private string _closedDisplayName;
        public string ClosedDisplayName
        {
            get { return _closedDisplayName; }
            set { _closedDisplayName = value; }
        }
        /// <summary>
        /// 值
        /// </summary>
        private string _displayValue;
        public string DisplayValue
        {
            get { return _displayValue; }
            set { _displayValue = value; }
        }
        /// <summary>
        /// 是否选中
        /// </summary>
        private string _isChecked;
        public string IsChecked
        {
            get { return _isChecked; }
            set { _isChecked = value; }
        }
        /// <summary>
        /// 周代码是否可用
        /// </summary>
        private string _isEndableWeek;
        public string IsEnableWeek
        {
            get { return _isEndableWeek; }
            set { _isEndableWeek = value; }
        }
    }
}