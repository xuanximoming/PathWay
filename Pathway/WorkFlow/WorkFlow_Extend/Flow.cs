﻿using System;
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
    /// <summary>
    /// 路径
    /// </summary>
    public class Flow {
        String _UniqueID;

        public String UniqueID
        {
            get { return _UniqueID; }
            set { _UniqueID = value; }
        }
    }
}
