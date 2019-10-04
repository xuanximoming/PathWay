using System.Windows.Media;
using System;

using System.Windows.Controls;

namespace YidanSoft.Tool
{
    public class ConvertColor
    {
        /// <summary>
        /// 获取画刷
        /// </summary>
        /// <param name="h">16进制如颜色#FF3DBCC7，传入参数则为：3DBCC7</param>
        /// <returns>SolidColorBrush</returns>
        public static SolidColorBrush GetColorBrushFromHx16(string h)
        {
            SolidColorBrush theAnswer = new SolidColorBrush();
            byte a = 255; // or whatever...
            byte r = (byte)Convert.ToUInt32(h.Substring(0, 2), 16);
            byte g = (byte)Convert.ToUInt32(h.Substring(2, 2), 16);
            byte b = (byte)Convert.ToUInt32(h.Substring(4, 2), 16);
            theAnswer.Color = Color.FromArgb(a, r, g, b);
            return theAnswer;
        }
    }
}
