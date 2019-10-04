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

namespace YidanEHRApplication.WorkFlow.Component
{

    public class SystemConst
    {
        public class ColorConst
        {
            public static Brush WarningColor
            {
                get
                {
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = Color.FromArgb(255, 255, 0, 0);
                    return brush;
                }
            }
            /// <summary>
            /// 选中的颜色
            /// </summary>
            public static Brush SelectedColor
            {
                get
                {
                    SolidColorBrush brush = new SolidColorBrush();
                    brush.Color = Color.FromArgb(255, 255, 181, 0);
                    return brush;
                }
            }

        }
        public static double RuleThickness
        {
            get
            {
                return 2.0;
            }
        }
    }
}
