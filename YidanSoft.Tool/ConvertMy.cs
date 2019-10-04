using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
namespace YidanSoft.Tool
{
    public class ConvertMy
    {
        public static String ToString(Object o)
        {
            String r = "";
            try
            {
                r = o.ToString();
                return r;
            }
            catch
            {
                return "";
            }
        }
        private static String ToDecimalString(Object o)
        {
            String r = "";
            try
            {
                r = o.ToString();
                return r;
            }
            catch
            {
                return "0";
            }
        }
        public static Decimal ToDecimal(Object o)
        {
            Decimal r = 0;
            try
            {
                r = Convert.ToDecimal(ToDecimalString(o));
                return r;
            }
            catch
            {
                return 0;
            }
        }
        public static Int32 ToInt32(Object o)
        {
            Int32 r = 0;
            try
            {
                r = Convert.ToInt32(o.ToString());
                return r;
            }
            catch
            {
                return 0;
            }
        }
        public static short ToShort(Object o)
        {
            short r = 0;
            try
            {
                r = Convert.ToInt16(o.ToString());
                return r;
            }
            catch
            {
                return 0;
            }
        }
    }
}
