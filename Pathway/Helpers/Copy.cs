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
using System.IO;
using System.ServiceModel;
using System.Reflection;

namespace YidanEHRApplication.DataService
{
    public partial class CP_DoctorOrder
    {
        public object Clone()
        {
            return this.MemberwiseClone();
        }
    }
}
