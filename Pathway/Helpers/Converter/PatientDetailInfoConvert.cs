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
using YidanEHRApplication.DataService;

namespace YidanEHRApplication
{
    public class PatientDetailInfoLongColorConvert : IValueConverter
    {
        public object Convert(object value, Type targetType,
         object parameter,
            System.Globalization.CultureInfo culture)
        {
            // value is the data from the source object.
            CP_LongOrderList order = value as CP_LongOrderList;
            return order.Yzzt.ToString();

        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }

    public class PatientDetailInfoTempColorConvert : IValueConverter
    {
        public object Convert(object value, Type targetType,
         object parameter,
            System.Globalization.CultureInfo culture)
        {
            // value is the data from the source object.
            CP_TempOrderList order = value as CP_TempOrderList;
            return order.Yzzt.ToString();

        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
