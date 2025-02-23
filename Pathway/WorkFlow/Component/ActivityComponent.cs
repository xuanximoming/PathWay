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

namespace YidanEHRApplication.WorkFlow.Component
{
    public class ActivityComponent
    {

        string _subFlow;
        public string SubFlow
        {
            get
            {
                return _subFlow;
            }
            set
            {
                _subFlow = value;
            }
        }

        string _repeatDirection = "Horizontal";
        public string RepeatDirection
        {
            get
            {
                return _repeatDirection;
            }
            set
            {
                _repeatDirection = value;
            }
        }

        string uniqueID;
        public string UniqueID
        {
            get
            {
                if (string.IsNullOrEmpty(uniqueID))
                {
                    uniqueID = Guid.NewGuid().ToString();
                }
                return uniqueID;
            }
            set
            {
                uniqueID = value;
            }

        }
        string activityID;

        public string ActivityID
        {
            get
            {
                return activityID;
            }
            set
            {
                activityID = value;
            }
        }

        string activityName;

        public string ActivityName
        {
            get
            {
                return activityName;
            }
            set
            {
                activityName = value;
            }
        }
        string activityType;
        public string ActivityType
        {
            get
            {
                return activityType;
            }
            set
            {
                activityType = value;
            }
        }
    }
}
