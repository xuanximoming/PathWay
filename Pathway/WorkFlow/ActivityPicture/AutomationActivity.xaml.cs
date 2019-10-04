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
using YidanEHRApplication.WorkFlow.Component;

namespace YidanEHRApplication.WorkFlow.ActivityPicture
{
    public partial class AutomationActivity : UserControl, IActivityPicture
    {
        public AutomationActivity()
        {
            InitializeComponent();

        }

        public new double Opacity
        {
            set { picAUTOMATION.Opacity = value; }
            get { return picAUTOMATION.Opacity; }
        }
        public double PictureWidth
        {
            set { picAUTOMATION.Width = value; }
            get { return picAUTOMATION.Width; }
        }
        public double PictureHeight
        {
            set { picAUTOMATION.Height = value; }
            get { return picAUTOMATION.Height; }
        }
        public new Brush Background
        {
            set { picAUTOMATION.Fill = value; }
            get { return picAUTOMATION.Fill; }
        }
        public Visibility PictureVisibility
        {
            set
            {

                this.Visibility = value;
            }
            get
            {

                return this.Visibility;
            }
        }
        public void ResetInitColor()
        {
            SolidColorBrush brush = new SolidColorBrush();
            brush.Color = Colors.White;
            picAUTOMATION.Fill = brush;
        }
        public void SetWarningColor()
        {
            picAUTOMATION.Fill = SystemConst.ColorConst.WarningColor;
        }
        public void SetSelectedColor()
        {
            picAUTOMATION.Fill = SystemConst.ColorConst.SelectedColor;
        }
        public PointCollection ThisPointCollection
        {
            get { return null; }
        }
        public ElementState CurrentElementState
        {
            set
            {
                picAUTOMATION.Stroke = BrushFromElementState.GetBrushFromElementState(value);
            }
        }
        
    }
}
