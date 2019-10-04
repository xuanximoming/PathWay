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

using System.Text.RegularExpressions;

namespace YidanEHRApplication.Helpers
{
    public abstract class Dataprocessing
    {
        /// <summary>
        /// 判断是否为指定带几位小数的数字
        /// </summary>
        /// <param name="strNumber">判断的数据</param>
        /// <param name="intdecimals">小数位数，9999为任意数</param>
        /// <returns> true是数字，false非数字</returns>
        public static bool IsNumber(String strNumber, int intdecimals = 9999)
        {
            string[] pattern = new string[]
            {
                @"^[0-9]*$", //intdecimals=0
                @"^[0-9]*[.]?[0-9]?$", // @"(^[0-9]*$)|(^[0-9]*[.]?[0-9]$)", //intdecimals=0
                @"^[0-9]*[.]?[0-9]*$"  //default @"^[0-9]*.[0-9]*$" 
            };

            Match match;

            switch (intdecimals)
            {
                case 0: //整数
                    {
                        match = Regex.Match(strNumber, pattern[0]);   // 匹配正则表达式    
                        break;
                    }
                case 1: //一位小数
                    {
                        match = Regex.Match(strNumber, pattern[1]);   // 匹配正则表达式    
                        break;
                    }
                default: //任意数字（21 、23.33）
                    {
                        match = Regex.Match(strNumber, pattern[pattern.Length - 1]);   // 匹配正则表达式    
                        break;
                    }
            }

            return match.Success;//false不是数字,true是数字
        }

    }
}
