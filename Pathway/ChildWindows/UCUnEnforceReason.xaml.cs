using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using YidanEHRApplication.Models;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Views.UserControls
{
    public partial class UCUnEnforceReason : UserControl
    {
        public List<CP_PathVariation> ListPathVariation
        { get; set; }
        public CP_DoctorOrder DoctorOrder
        { get; set; }

        public string Ljdm
        { get; set; }

        /// <summary>
        /// 所有RADIOBUTTON 
        /// </summary>
        public List<RadioButton> m_ListRadioButton = new List<RadioButton>();

        /// <summary>
        /// 其它的基本原因
        /// </summary>
        private List<CP_PathVariation> m_ListPathVariation = new List<CP_PathVariation>();

        private CP_VariantRecords m_SelectUnForceItems = new CP_VariantRecords();
        /// <summary>
        /// 未执行原因 包括5个栏位 
        /// </summary>
        public CP_VariantRecords SelectUnForceItems
        {
            get
            {
                if (m_SelectUnForceItems.Bydm == "9999")
                {
                    TextBox textBox = stackPanelInfo.FindName("textBoxOthers") as TextBox;
                    m_SelectUnForceItems.Byyy = textBox.Text.Trim();
                }
                return m_SelectUnForceItems;
            }
            private set
            {
                m_SelectUnForceItems = value;
            }
        }

        /// <summary>
        /// 当前结点
        /// </summary>
        public String CurrentActivityId
        { get; set; }

        public UCUnEnforceReason()
        {
            InitializeComponent();
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                InitShowInfo();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void InitShowInfo()
        {
            if (stackPanelInfo.Children.Count > 0)
                return;
            //if (ListPathVariation.Count > 0)
            //{
                TextBlock textBlock = new TextBlock();
                textBlock.Text = DoctorOrder.Yznr + ":";
                textBlock.FontWeight = FontWeights.Bold;
                textBlock.SetValue(ToolTipService.ToolTipProperty, DoctorOrder.Yznr);
                this.stackPanelInfo.Children.Add(textBlock);
            //}
            #region 构造其它类型的数据
            CP_PathVariation info = new CP_PathVariation();

            info.Bydm = "9999";
            info.Bymc = "其它";
            info.Byms = "其它";
            info.Yxjl = 1;
            var OtherInfo = from item in ListPathVariation
                            where item.Bydm.Equals("9999")
                            select item;
            if (OtherInfo != null && OtherInfo.Count() == 0)
                ListPathVariation.Add(info);

            #endregion
            foreach (CP_PathVariation variation in ListPathVariation)
            {
                RadioButton radioButton = new RadioButton();
                radioButton.Checked += new RoutedEventHandler(radioButton_Checked);
                radioButton.Content = variation.Bymc;
                radioButton.Tag = variation.Bydm;
                if (variation.Bydm == "9999")
                    radioButton.Margin = new Thickness(0, 5, 0, 0);
                stackPanelInfo.Children.Add(radioButton);
                m_ListRadioButton.Add(radioButton);
                if (variation.Bydm == "9999")
                {
                    RadComboBox radBox = new RadComboBox();
                    radBox.SelectionChanged += new Telerik.Windows.Controls.SelectionChangedEventHandler(radBox_SelectionChanged);
                    radBox.IsEnabled = false;
                    radBox.Margin = new Thickness(45, -20, 5, 0);
                    radBox.Name = "radBoxOthers";
                    radBox.SelectedValuePath = "Bydm";
                    radBox.SetValue(Telerik.Windows.Controls.TextSearch.TextPathProperty, "Py");
                    radBox.ItemTemplate = (DataTemplate)this.FindName("ComboBoxCustomTemplate");
                    radBox.SelectionBoxTemplate = (DataTemplate)this.FindName("ComboBoxSimpleTemplate");
                    radBox.Style = (Style)this.FindName("RadComboBoxStyle");
                    radBox.EmptyText = "请选择原因";
                    radBox.ClearSelectionButtonVisibility = System.Windows.Visibility.Visible;
                    radBox.ClearSelectionButtonContent = "请选择原因";

                    stackPanelInfo.Children.Add(radBox);
                    radBusyIndicator.IsBusy = true;

                    YidanEHRDataServiceClient client = PublicMethod.YidanClient;
                    client.GetOtherPathVariationInfoCompleted +=
                         (obj, e) =>
                         {
                             radBusyIndicator.IsBusy = false;
                             if (e.Error == null)
                             {
                                 RadComboBox radBox2 = stackPanelInfo.FindName("radBoxOthers") as RadComboBox;
                                 radBox2.ItemsSource = e.Result.ToList();
                                 m_ListPathVariation = e.Result.ToList();
                             }
                             else
                             {
                                 PublicMethod.RadWaringBox(e.Error);
                             }

                         };
                    client.GetOtherPathVariationInfoAsync(Ljdm, CurrentActivityId);
                    client.CloseAsync();
                }
            }
            if (ListPathVariation.Count > 0)
            {
                TextBox textBox = new TextBox();
                textBox.Name = "textBoxOthers";
                textBox.AllowDrop = true;
                textBox.TextWrapping = TextWrapping.Wrap;
                textBox.Height = 100;
                textBox.Margin = new Thickness(5, 5, 5, 0);
                stackPanelInfo.Children.Add(textBox);

            }
            //判断如果只有一个单选框则默认选中单选框
            if (m_ListRadioButton.Count == 1)
            {
                m_ListRadioButton[0].IsChecked = true;
            }
        }

        /// <summary>
        /// combbox selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radBox_SelectionChanged(object sender, Telerik.Windows.Controls.SelectionChangedEventArgs e)
        {
            try
            {
                RadComboBox radBox = sender as RadComboBox;
                TextBox textBox = stackPanelInfo.FindName("textBoxOthers") as TextBox;
                if (radBox.SelectedValue == null)
                {
                    textBox.Text = string.Empty;
                    textBox.IsEnabled = true;
                }
                else
                {
                    CP_PathVariation variation = m_ListPathVariation.First(delegate(CP_PathVariation cp)
                    {
                        return cp.Bydm.Equals(radBox.SelectedValue.ToString());
                    }
                                                                           );
                    textBox.Text = variation.Bymc;
                    textBox.IsEnabled = false;
                    InitPariationInfo(radBox.SelectedValue.ToString(), variation.Bymc);
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        /// <summary>
        /// radio button selected
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void radioButton_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                RadioButton button = sender as RadioButton;
                RadComboBox radBox = stackPanelInfo.FindName("radBoxOthers") as RadComboBox;
                radBox.IsEnabled = false;
                if (button.Tag.ToString() == "9999")
                {
                    radBox.IsEnabled = true;
                }
                InitPariationInfo(button.Tag.ToString(), button.Content.ToString());
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }




        private void InitPariationInfo(string strBydm, string strByyy)
        {
            m_SelectUnForceItems.Ljdm = string.Empty;
            m_SelectUnForceItems.Mxdm = string.Empty;
            m_SelectUnForceItems.Bydm = strBydm;
            m_SelectUnForceItems.Byyy = strByyy;
            m_SelectUnForceItems.Ypdm = DoctorOrder.Ypdm;
            m_SelectUnForceItems.Bylx = Convert.ToString((int)VariationCategory.Undo);
            m_SelectUnForceItems.Bylb = Convert.ToString((int)VariationType.Order);
            m_SelectUnForceItems.Bysj = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            m_SelectUnForceItems.Bynr = "医嘱【" + DoctorOrder.Yznr + "】未执行";
        }


    }

    #region 暂时保留
    //public class UnEnforceSelectItems
    //{
    //    private string m_SelectCode = string.Empty;
    //    /// <summary>
    //    /// 选择的编码
    //    /// </summary>
    //    public string SelectCode
    //    {
    //        get
    //        {
    //            return m_SelectCode;
    //        }
    //        set
    //        {
    //            m_SelectCode = value;
    //        }
    //    }

    //    private string m_SelectContent = string.Empty;
    //    /// <summary>
    //    /// 自己填写的内容
    //    /// </summary>
    //    public string SelectContent
    //    {
    //        get
    //        {
    //            return m_SelectContent;
    //        }
    //        set
    //        {
    //            m_SelectContent = value;
    //        }
    //    }

    //    public UnEnforceSelectItems()
    //    { }




    //}
    #endregion
}
