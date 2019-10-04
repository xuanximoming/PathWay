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

namespace YidanEHRApplication.Models
{
    public class OrderTypeName
    {
        public string OrderName
        {
            get;
            set;
        }
        public short OrderValue
        {
            get;
            set;
        }
        public OrderTypeName(string orderName, short orderValue)
        {
            OrderName = orderName;
            OrderValue = orderValue;
        }
    }
}