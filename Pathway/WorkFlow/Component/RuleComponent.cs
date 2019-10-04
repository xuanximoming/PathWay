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

namespace YidanEHRApplication.WorkFlow.Component
{

    public class RuleComponent
    {

        public string LineType { get; set; }

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
        string ruleID;

        public string RuleID
        {
            get
            {
                return ruleID;
            }
            set
            {
                ruleID = value;
            }
        }
        string ruleName;

        public string RuleName
        {
            get
            {
                return ruleName;
            }
            set
            {
                ruleName = value;
            }
        }
        string ruleCondition;
        public string RuleCondition
        {
            get
            {
                return ruleCondition;
            }
            set
            {
                ruleCondition = value;
            }
        }
    }
}
