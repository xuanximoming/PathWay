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
namespace YidanEHRApplication.WorkFlow
{
    public class ActiveChildren
    {
        String _EnForceTime = String.Empty;
        public String EnForceTime
        {
            get { return _EnForceTime; }
            set { _EnForceTime = value; }
        }
        String _ActivityChildrenID;
        public String ActivityChildrenID
        {
            get { return _ActivityChildrenID; }
            set { _ActivityChildrenID = value; }
        }
        String _ActivityUniqueID;
        public String ActivityUniqueID
        {
            get { return _ActivityUniqueID; }
            set { _ActivityUniqueID = value; }
        }
        ElementState _CurrentElementState = ElementState.Next;
        public ElementState CurrentElementState
        {
            get { return _CurrentElementState; }
            set
            {
                _CurrentElementState = value;
            }
        }
    }
}
