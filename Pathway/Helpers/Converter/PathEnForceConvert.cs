using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.Windows.Data;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication
{
    /// <summary>
    /// 路径执行变色
    /// 配合ConditionalStyleSelector使用
    /// </summary>
    public class PathEnForceForClorConvert : IValueConverter
    {
        public object Convert(object value, Type targetType,
         object parameter,
            System.Globalization.CultureInfo culture)
        {
            // value is the data from the source object.
            CP_DoctorOrder order = value as CP_DoctorOrder;
            SolidColorBrush brush = new SolidColorBrush(Colors.Black);
            if (order.FromTable == OrderFromTable.CP_AdviceGroupDetail.ToString())
            {
                return "CP_AdviceGroupDetail";
                //brush = new SolidColorBrush(Colors.Red);
            }
            else if (order.FromTable == OrderFromTable.CP_TempOrder.ToString() || order.FromTable == OrderFromTable.CP_LongOrder.ToString() || order.FromTable == OrderFromTable.CP_CyXDFOrder.ToString())
            {
                return order.Yzzt.ToString();
            }
            return "NOTCOMPARE";

        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }


    public class PathEnForceGridCellEditor : IValueConverter
    {
        public object Convert(object value, Type targetType,
         object parameter,
            System.Globalization.CultureInfo culture)
        {
            // value is the data from the source object.
            string yzzt = value.ToString();
            if (yzzt == string.Empty || yzzt == "0" || yzzt == ((decimal)OrderStatus.OrderInptut).ToString())
                return true;
            return false;

        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
