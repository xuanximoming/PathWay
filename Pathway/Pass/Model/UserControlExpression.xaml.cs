using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Models;
using YidanEHRApplication.Pass.Model;

namespace YidanEHRApplication.Controls
{
    public partial class UCExpression : UserControl
    {

        public UCExpression()
        {
            InitializeComponent();
        }


        /// <summary>
        /// 记录控件中数字控件
        /// </summary>
        private List<ObjectCell> m_radnumberList = new List<ObjectCell>();

        //定义公式变量
        Expressions _expression = new Expressions();

        public Expressions Expression
        {
            get { return _expression; }
            set { _expression = value; }
        }

        private void UserControl_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                if (_expression != null)
                {
                    this.labExpression.Text = _expression.ExpressionsName == null ? "" : _expression.ExpressionsName.ToString();

                    this.txtDescribe.Text = _expression.ExpressionsDescribe == null ? "" : _expression.ExpressionsDescribe.ToString();

                    //清空参数控件
                    warppanel.Children.Clear();
                    m_radnumberList.Clear();

                    //动态添加计算公式中的参数输入框
                    foreach (ParameterProperty par in _expression.ExpressionsParameter)
                    {
                        ObjectCell cell = CreateObjectCollection(par);

                        if (cell == null)
                            continue;
                        else
                        {
                            m_radnumberList.Add(cell);

                            warppanel.Children.Add(cell.ObjTextBlock);
                            warppanel.Children.Add(cell.ObjNumericUpDown);
                        }

                    }
                }
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }


        /// <summary>
        /// 创建公式参数控件
        /// </summary>
        /// <param name="modelParameterProperty">单个参数控件属性实体</param>
        public ObjectCell CreateObjectCollection(ParameterProperty modelParameterProperty)
        {
            if (modelParameterProperty != null)
            {
                ObjectCell objectCell = new ObjectCell(modelParameterProperty);
                //添加参数控件
                return objectCell;
            }

            return null;
        }

        /// <summary>
        /// 计算公式 计算按钮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                RadNumericUpDown radnumber = new RadNumericUpDown();
                TextBlock txtblock = new TextBlock();
                foreach (ObjectCell objcell in m_radnumberList)
                {
                    radnumber = objcell.ObjNumericUpDown;
                    txtblock = objcell.ObjTextBlock;

                    if (radnumber.Value.ToString() == "")
                    {
                        PublicMethod.RadAlterBox("请填写【" + txtblock.Text + "】参数!", "提示");
                        return;
                    }
                }

                StringBuilder m_ExpressionsProcess = new StringBuilder(_expression.ExpressionsProcess);
                foreach (ObjectCell objcell in m_radnumberList)
                {
                    radnumber = objcell.ObjNumericUpDown;

                    m_ExpressionsProcess.Replace(radnumber.Tag.ToString(), radnumber.Value.ToString());
                }



                YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
                ServiceClient.EvalCompleted +=
                    (obj, ea) =>
                    {
                        if (ea.Error == null)
                        {
                            string result = ea.Result.ToString();

                            result = Convert.ToDouble(result).ToString("f4");
                            this.txtresult.Text = result + "(" + _expression.ExpressionsReusltUnit + ")";
                        }
                        else
                        {
                            PublicMethod.RadWaringBox(ea.Error);
                        }
                    };
                ServiceClient.EvalAsync(m_ExpressionsProcess.ToString());
                ServiceClient.CloseAsync();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }





    }

}
