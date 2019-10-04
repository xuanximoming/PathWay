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

namespace YidanSoft.Tool
{
    public class StringManage
    {
        /// <summary>
        /// 在原有字符串前加指定字符，指定长度的内容
        /// </summary>
        /// <param name="original">源字符串</param>
        /// <param name="pre">前缀字符</param>
        /// <param name="needLong">总长度</param>
        /// <returns></returns>
        public static  String AddPre(String original, Char pre,Int32 needLong)
        {
            String returnStr = "";
            for (int i = 0; i < needLong - original.Length; i++)
            {
                returnStr += pre;
            }

            returnStr += original;
                return returnStr;
        }
    }
}
