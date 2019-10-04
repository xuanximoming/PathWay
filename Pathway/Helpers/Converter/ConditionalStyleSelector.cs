using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Media;
using System.Windows.Data;
using Telerik.Windows.Controls;



namespace YidanEHRApplication
{
    /// <summary>
    /// 样式转换器
    /// copy from radControl demo
    /// </summary>
    public class ConditionalStyleSelector : StyleSelector
    {
        public override System.Windows.Style SelectStyle(object item, System.Windows.DependencyObject container)
        {
            object conditionValue = this.ConditionConverter.Convert(item, null, null, null);
            foreach (ConditionalStyleRule rule in this.Rules)
            {
                if (Equals(rule.Value, conditionValue))
                {
                    return rule.Style;
                }
            }


            return base.SelectStyle(item, container);
        }

        List<ConditionalStyleRule> _Rules;
        public List<ConditionalStyleRule> Rules
        {
            get
            {
                if (this._Rules == null)
                {
                    this._Rules = new List<ConditionalStyleRule>();
                }

                return this._Rules;
            }
        }

        IValueConverter _ConditionConverter;
        public IValueConverter ConditionConverter
        {
            get
            {
                return this._ConditionConverter;
            }
            set
            {
                this._ConditionConverter = value;
            }
        }
    }

    /// <summary>
    /// 条件转换规则 
    /// </summary>
    public class ConditionalStyleRule
    {
        object _Value;
        public object Value
        {
            get
            {
                return this._Value;
            }
            set
            {
                this._Value = value;
            }
        }

        Style _Style;
        public Style Style
        {
            get
            {
                return this._Style;
            }
            set
            {
                this._Style = value;
            }
        }
    }
}
