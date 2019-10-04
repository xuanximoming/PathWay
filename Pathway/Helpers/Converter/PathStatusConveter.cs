using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication
{
    /// <summary>
    /// 路径状态转换器
    /// 配合ConditionalStyleSelector使用
    /// </summary>
    public class PathStatusConveter : IValueConverter
    {
        #region IValueConverter Members

        /// <summary>
        /// 返回路径状态
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            CP_InpatinetList patInfo = value as CP_InpatinetList;

            if (patInfo != null)
            {
                return patInfo.Ljzt;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
