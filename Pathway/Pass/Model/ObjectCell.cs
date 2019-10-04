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
using YidanEHRApplication.YidanEHRServiceReference;
using YidanEHRApplication.Models;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Pass.Model
{
    //public class ExpressionsEval
    //{
    //    public void InitExpressions()
    //    {
    //        YidanEHRServiceReference.YidanEHRDataServiceClient client = PublicMethod.YidanClient;
    //        client.ReadExpressionsConfigXMLCompleted += new EventHandler<ReadExpressionsConfigXMLCompletedEventArgs>(client_ReadExpressionsConfigXMLCompleted);
    //        client.ReadExpressionsConfigXMLAsync();
    //        client.CloseAsync();
    //    }

    //    void client_ReadExpressionsConfigXMLCompleted(object sender, ReadExpressionsConfigXMLCompletedEventArgs e)
    //    {
    //        ObservableCollection<Expressions> modelException = new ObservableCollection<Expressions>();

    //        if (e.Error == null)
    //        {
    //            modelException = e.Result;
    //        }
    //        else
    //        {
    //            throw new NotImplementedException();
    //        }
    //    }
    //}

    #region 最小单元输入参数控件集实体
    /// <summary>
    /// 最小单元输入参数控件集实体
    /// </summary>
    public class ObjectCell
    {
        /// <summary>
        /// 最小单元输入参数提示控件
        /// </summary>
        TextBlock m_ObjTextBlock = new TextBlock();
        //TextBox     m_ObjTextBox = new TextBox();

        /// <summary>
        /// 最小单元输入参数控件
        /// </summary>
        RadNumericUpDown m_ObjNumericUpDown = new RadNumericUpDown();

        /// <summary>
        /// ObjectCell构造函数
        /// </summary>
        /// <param name="par">parameterproperty单个参数控件属性实体</param>
        public ObjectCell(ParameterProperty parameterproperty)
        {
            DefaultAttribute();
            SetProperty(parameterproperty);
        }

        /// <summary>
        /// 控件默认属性
        /// </summary>
        private void DefaultAttribute()
        {
            //TextBlock
            m_ObjTextBlock.FlowDirection = System.Windows.FlowDirection.LeftToRight;
            m_ObjTextBlock.FontSize = 12;
            m_ObjTextBlock.Width = 80;
            m_ObjTextBlock.Margin = new Thickness(0, 10, 10, 3);
            m_ObjTextBlock.TextAlignment = TextAlignment.Right;

            //NumericUpDown
            m_ObjNumericUpDown.ShowButtons = true;//不限时调节按钮
            //m_ObjNumericUpDown.Minimum = 0;//最小值无约束
            //m_ObjNumericUpDown.Maximum = 0;//最大值无约束

            m_ObjNumericUpDown.Width = 120;
            m_ObjNumericUpDown.Height = 25;
            m_ObjNumericUpDown.HorizontalAlignment = System.Windows.HorizontalAlignment.Left;
            m_ObjNumericUpDown.Margin = new Thickness(20, 5, 10, 3);
        }

        /// <summary>
        /// 最小单元输入参数提示控件
        /// </summary>
        public TextBlock ObjTextBlock
        {
            get
            {
                return m_ObjTextBlock;
            }
            set
            {
                m_ObjTextBlock = value;
            }
        }

        /// <summary>
        /// 最小单元输入参数控件
        /// </summary>
        public RadNumericUpDown ObjNumericUpDown
        {
            get
            {
                return m_ObjNumericUpDown;
            }
            set
            {
                m_ObjNumericUpDown = value;
            }
        }


        /// <summary>
        /// 设置最小单元参数控件属性
        /// </summary>
        /// <param name="modelParameterProperty"></param>
        public void SetProperty(ParameterProperty modelParameterProperty)
        {
            //输入参数显示标签
            m_ObjTextBlock.Text = modelParameterProperty.LabelText;

            //输入参数控件值替换处理公式的标识
            m_ObjNumericUpDown.Tag = modelParameterProperty.Tag.ToString();

            //设置参数输入控件属性
            foreach (ParameterCell obj in modelParameterProperty.Property)
            {
                switch (obj.Names.Trim().ToLower().ToString())
                {
                    case "minimum"://最大值
                        {
                            m_ObjNumericUpDown.Minimum = Convert.ToDouble(obj.Value) ;
                            break;
                        }
                    case "maximum"://最小值
                        {
                            m_ObjNumericUpDown.Maximum = Convert.ToDouble(obj.Value); 
                            break;
                        }
                }
            }
        }

    }
    #endregion
}
