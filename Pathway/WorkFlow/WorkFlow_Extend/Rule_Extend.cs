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
using System.Collections.Generic;
using YidanEHRApplication.WorkFlow.Designer;
using YidanEHRApplication.WorkFlow;
using Telerik.Windows.Controls;
using YidanEHRApplication.Models;
using System.Linq;
namespace YidanEHRApplication.WorkFlow.Designer
{
   
    public partial class Rule
    {
        #region 构造函数
        public Rule()
        { }
        #endregion
        #region 变量
        ElementState _CurrentElementState = ElementState.Next;
        Boolean _IsEdit = true;
        #endregion
        #region 属性
        public ElementState CurrentElementState
        {
            get { return _CurrentElementState; }
            set { _CurrentElementState = value; }
        }
        public Boolean IsEdit
        {
            get { return CurrentElementState != ElementState.Pre; }
            set { _IsEdit = value; }
        }
        #endregion
    }
}
