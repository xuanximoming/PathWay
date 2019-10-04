using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Data;
using System.Windows.Media;
using YidanEHRApplication.Views;

namespace YidanEHRApplication.Helpers.Converter
{
    public class PathManagerColorConvert : IValueConverter
    {

        public object Convert(object value, Type targetType,
         object parameter,
            System.Globalization.CultureInfo culture)
        {
            // value is the data from the source object.
            SolidColorBrush brush = new SolidColorBrush(Colors.Black);
            int pathStatus = int.Parse(value.ToString());
            if (pathStatus == (int)PathShStatus.Review)
                return brush = new SolidColorBrush(Colors.Red);
            return brush;

        }

        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType,
            object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
