using DrectSoft.Tool;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using YidanEHRApplication.WorkFlow.Component;

namespace YidanEHRApplication.WorkFlow.ActivityPicture
{
    public class BrushFromElementState
    {
        public static SolidColorBrush GetBrushFromElementState(ElementState state)
        {
            SolidColorBrush brush = new SolidColorBrush();
            if (state == ElementState.Pre)
                brush = ConvertColor.GetColorBrushFromHx16("ff0000");
            if (state == ElementState.Now)
                brush = ConvertColor.GetColorBrushFromHx16("A349A4");
            if (state == ElementState.Hide)
                brush = ConvertColor.GetColorBrushFromHx16("7F7F7F");
            if (state == ElementState.Next)
                brush.Color = Colors.Green;
            return brush;
        }
    }
    public partial class InitialAcitivity : UserControl, IActivityPicture
    {
        public InitialAcitivity()
        {
            InitializeComponent();
        }
        public new double Opacity
        {
            set
            {
                picBegin.Opacity = value;
            }
            get { return picBegin.Opacity; }
        }
        public double PictureWidth
        {
            set
            {
                picBegin.Width = value - 4;
                eliBorder.Width = value - 2;
            }
            get { return picBegin.Width + 4; }
        }
        public double PictureHeight
        {
            set
            {
                picBegin.Height = value - 4;
                eliBorder.Height = value - 2;
            }
            get { return picBegin.Height + 4; }
        }
        public new Brush Background
        {
            set
            {
                picBegin.Fill = value;
            }
            get { return picBegin.Fill; }
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
            brush.Color = Colors.Green;
            picBegin.Fill = brush;
            brush = new SolidColorBrush();
            brush.Color = Color.FromArgb(255, 0, 0, 0);
        }

        public void SetWarningColor()
        {
            picBegin.Fill = SystemConst.ColorConst.WarningColor;
        }
        public void SetSelectedColor()
        {
            picBegin.Fill = SystemConst.ColorConst.SelectedColor;


        }
        public PointCollection ThisPointCollection
        {
            get { return null; }
        }
        public ElementState CurrentElementState
        {
            set
            {
                eliBorder.Stroke = BrushFromElementState.GetBrushFromElementState(value);
            }
        }

    }
}
