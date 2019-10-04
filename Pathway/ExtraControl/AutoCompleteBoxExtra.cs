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
using System.Reflection;
using System.Windows.Controls.Primitives;
//using Aizhe.Utilities;
namespace YidanEHRApplication.ExtraControl
{
    public class AizheAutoCompleteBox : AutoCompleteBox
    {
        private bool IsBatchUpdating = false;
        public AizheAutoCompleteBox()
            : base()
        {
            this.DefaultStyleKey = typeof(AizheAutoCompleteBox);
            //();
        }

        #region SelectedValuePath

        public static readonly DependencyProperty SelectedValuePathProperty =
            DependencyProperty.Register("SelectedValuePath",
                    typeof(string),
                    typeof(AizheAutoCompleteBox), null);
        /// <summary>
        /// 获取或设置选中值的Path,用于实现Key/Value Pair
        /// </summary>
        public string SelectedValuePath
        {
            get { return GetValue(SelectedValuePathProperty) as string; }
            set { SetValue(SelectedValuePathProperty, value); }
        }

        #endregion

        #region SelectedValue
        public static readonly DependencyProperty SelectedValueProperty =
            DependencyProperty.Register("SelectedValue", typeof(object),
                typeof(AizheAutoCompleteBox),
                new PropertyMetadata(new PropertyChangedCallback(OnSelectedValueChanged))
                );
        /// <summary>
        /// gets or sets SelectedValue,used to implement key value piar.
        /// </summary>
        public object SelectedValue
        {
            get { return GetValue(SelectedValueProperty); }
            set { SetValue(SelectedValueProperty, value); }
        }

        private static void OnSelectedValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AizheAutoCompleteBox)d).OnSelectedValueChanged(e);
        }

        protected virtual void OnSelectedValueChanged(DependencyPropertyChangedEventArgs e)
        {
            if (!IsBatchUpdating)
                SetSelectedItemUsingSelectedValue();
        }
        /// <summary>
        /// sets SelectedItem using SelectedValue.
        /// </summary>
        private void SetSelectedItemUsingSelectedValue()
        {
            if (this.SelectedValue == null || this.ItemsSource == null)
                this.SelectedItem = null;
            else
            {
                string selectedValuePath = this.SelectedValuePath;
                object selectValue = GetValue(SelectedValueProperty);
                if ((!string.IsNullOrEmpty(selectedValuePath)) && selectValue != null)
                {
                    foreach (object item in this.ItemsSource)
                    {
                        PropertyInfo propertyInfo = item.GetType().GetProperty(selectedValuePath);
                        object propertyValue = propertyInfo.GetValue(item, null);
                        if (propertyValue != null && propertyValue.Equals(selectValue))
                        {
                            this.SelectedItem = item;
                            break;
                        }

                    }
                }
            }
        }

        #endregion SelectedValue

        #region SpellCodePath
        public static readonly DependencyProperty SpellCodePathProperty =
            DependencyProperty.Register("SpellCodePath",
                    typeof(string),
                    typeof(AizheAutoCompleteBox), null
            /*new PropertyMetadata(new PropertyChangedCallback(OnSpellCodePathChanged))*/
                    );

        /// <summary>
        /// gets or sets SpellCode Path.
        /// </summary>
        public string SpellCodePath
        {
            get { return GetValue(SpellCodePathProperty) as string; }
            set
            {
                SetValue(SpellCodePathProperty, value);
                //SetSpellCodeFilter();
            }
        }

        //private static void OnSpellCodePathChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        //{
        //    ((AizheAutoCompleteBox)d).OnSelectedValueChanged(e);
        //}

        //protected virtual void OnSpellCodePathChanged(DependencyPropertyChangedEventArgs e)
        //{
        //    SetSpellCodeFilter();
        //}
        #endregion

        #region ShowDropDownToggle
        public static readonly DependencyProperty ShowDropDownToggleProperty =
            DependencyProperty.Register("ShowDropDownToggle", typeof(Boolean), typeof(AizheAutoCompleteBox),
            new PropertyMetadata(new PropertyChangedCallback(OnShowDropDownToggleChanged)));

        public bool ShowDropDownToggle
        {
            get { return (bool)GetValue(ShowDropDownToggleProperty); }
            set
            {
                SetValue(ShowDropDownToggleProperty, value);

            }
        }
        private static void OnShowDropDownToggleChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            ((AizheAutoCompleteBox)d).OnShowDropDownToggleChanged(e);
        }

        protected virtual void OnShowDropDownToggleChanged(DependencyPropertyChangedEventArgs e)
        {
            SetDropDownToggleDisplayState(ShowDropDownToggle);
        }

        protected virtual void SetDropDownToggleDisplayState(bool isShowing)
        {
            ToggleButton dropdownButton = (ToggleButton)GetTemplateChild("DropDownToggle");
            if (dropdownButton != null)
            {
                if (isShowing)

                    dropdownButton.Visibility = System.Windows.Visibility.Visible;
                else
                    dropdownButton.Visibility = System.Windows.Visibility.Collapsed;
            }
        }

        #endregion

        #region SearchUsingPinyin
        public static readonly DependencyProperty SearchUsingPinyinProperty =
            DependencyProperty.Register("SearchUsingPinyin", typeof(bool),
                typeof(AizheAutoCompleteBox), null);
        public bool SearchUsingPinyin
        {
            get { return (bool)GetValue(SearchUsingPinyinProperty); }
            set { SetValue(SearchUsingPinyinProperty, value); }
        }
        #endregion
    }
}