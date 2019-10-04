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
using Telerik.Windows.Controls;


namespace YidanEHRApplication.ExtraControl
{
    /// <summary>
    /// radWindows扩展
    /// 提供提供静态调用方法
    /// </summary>
    public class RadMenuItemExtra : RadMenuItem
    {
        private String _ExterProperty;

        public String ExterProperty
        {
            get { return _ExterProperty; }
            set { _ExterProperty = value; }
        }

        private String _ExterProperty2;

        public String ExterProperty2
        {
            get { return _ExterProperty2; }
            set { _ExterProperty2 = value; }
        }


    }
}
