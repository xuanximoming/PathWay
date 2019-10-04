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
using YidanEHRApplication.YidanEHRServiceReference;
using YidanEHRApplication.Models;
using YidanEHRApplication.Helpers;
using Telerik.Windows.Controls;
using YidanEHRApplication.Controls;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Pass.Model
{
    public partial class ExpressionForm
    {

        private List<Expressions> m_ExpressionsList = new List<Expressions>();
        public ExpressionForm()
        {
            InitializeComponent();
        }

        void BindExpressionType()
        {
            YidanEHRDataServiceClient ServiceClient = PublicMethod.YidanClient;
            ServiceClient.ReadExpressionsConfigXMLByDBCompleted +=
                (obj, ea) =>
                {
                    if (ea.Error == null)
                    {
                        m_ExpressionsList = ea.Result.ToList();

                        cmbExpressionName.ItemsSource = m_ExpressionsList;
                        cmbExpressionName.ItemFilter = ExpressionFilter;

                    }
                    else
                    {
                        PublicMethod.RadWaringBox(ea.Error);
                        return;
                    }
                };
            ServiceClient.ReadExpressionsConfigXMLByDBAsync();
            ServiceClient.CloseAsync();
        }

        

        public bool ExpressionFilter(string strFilter, object item)
        {
            Expressions expressions = (Expressions)item;
            return ((expressions.ExpressionsName.StartsWith(strFilter)) || (expressions.ExpressionsName.Contains(strFilter))
                || (expressions.ExpressionsGroupType.StartsWith(strFilter)) || (expressions.ExpressionsGroupType.Contains(strFilter)));
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {

        }

        private void ExpressionForm_Loaded(object sender, RoutedEventArgs e)
        { try{
            BindExpressionType();
  }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }

        private void cmbExpressionName_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {try{
            if (cmbExpressionName.SelectedItem != null)
            {
                Expressions expression = (Expressions)cmbExpressionName.SelectedItem;

                warppaneExpression.Children.Clear();

                UCExpression usercontrolexpression = new UCExpression();
                usercontrolexpression.Expression = expression;
                usercontrolexpression.HorizontalAlignment = System.Windows.HorizontalAlignment.Stretch;
                usercontrolexpression.VerticalAlignment = System.Windows.VerticalAlignment.Stretch;

                warppaneExpression.Children.Add(usercontrolexpression);
            } }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

        }

    }
}

