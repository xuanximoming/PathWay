using DrectSoft.Tool;
using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using YidanEHRApplication.DataService;
using YidanEHRApplication.Helpers;

namespace YidanEHRApplication.Controls
{
    public partial class UCInPatientCard : UserControl
    {
        public delegate void PatientCardClicked(object sender, CP_InpatinetList e);
        public event PatientCardClicked AfterPatientCardClicked;

        public delegate void PatientCardDoubleClicked(object sender, CP_InpatinetList e);
        public event PatientCardDoubleClicked AfterPatientCardDoubleClicked;

        private CP_InpatinetList _currentPat;

        protected virtual void OnAfterPatientCardClicked(CP_InpatinetList e)
        {
            if (AfterPatientCardClicked != null)
            {
                AfterPatientCardClicked(this, e);
            }

        }

        protected virtual void OnAfterPatientCardDoubleClicked(CP_InpatinetList e)
        {
            if (AfterPatientCardDoubleClicked != null)
            {
                AfterPatientCardDoubleClicked(this, e);
            }

        }

        public CP_InpatinetList CurrentPat
        {
            get
            {
                return _currentPat;
            }
            set
            {
                _currentPat = value;
                Bind();
            }
        }


        public UCInPatientCard()
        {
            InitializeComponent();
            InitVars();
        }


        private void InitVars()
        {

            label_Name.Content = string.Empty;
            lab_BedID.Content = string.Empty;
            //label_PathState.Content = string.Empty;

        }

        private void Bind()
        {
            if (_currentPat == null)
            {
                InitVars();
            }
            else
            {
                //绑定性别图片
                if (_currentPat.Brxb == "女")
                {
                    this.manimage.Visibility = System.Windows.Visibility.Collapsed;
                    this.womanimage.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
                    this.manimage.Visibility = System.Windows.Visibility.Visible;
                    this.womanimage.Visibility = System.Windows.Visibility.Collapsed;
                }
                label_Name.Content = _currentPat.Hzxm;
                lab_BedID.Content = _currentPat.Bed;
                label_zyys.Content = _currentPat.Zyys;
                label_Wzjb.Content = _currentPat.Wzjb;

                SolidColorBrush brush = new SolidColorBrush();
                //路径状态为：中途退出
                if (_currentPat.Ljzt == Convert.ToString((int)PathStatus.QuitPath))
                {
                    brush = new SolidColorBrush();
                    brush.Color = Colors.Red;
                    this.label_PathStatecolor.Background = brush;
                }
                //未通过评估
                else if (_currentPat.Ljzt == Convert.ToString((int)PathStatus.NotIn))
                {
                    this.label_PathStatecolor.Background = ConvertColor.GetColorBrushFromHx16("A112AD");
                }

                //未引入
                else if (_currentPat.Ljzt == Convert.ToString((int)PathStatus.New))
                {
                    this.label_PathStatecolor.Background = ConvertColor.GetColorBrushFromHx16("3DBCC7");
                }

                //执行中
                else if (_currentPat.Ljzt == Convert.ToString((int)PathStatus.InPath))
                {
                    this.label_PathStatecolor.Background = ConvertColor.GetColorBrushFromHx16("CC391B");
                }
                //已完成
                else if (_currentPat.Ljzt == Convert.ToString((int)PathStatus.DonePath))
                {
                    this.label_PathStatecolor.Background = ConvertColor.GetColorBrushFromHx16("53B119");
                }
            }
        }

        /// <summary>
        /// 鼠标移动到but上事件
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RadButton_MouseEnter(object sender, MouseEventArgs e)
        {

        }

        /// <summary>
        /// 判断是否为双击事件
        /// </summary>
        System.Windows.Threading.DispatcherTimer _doubleClickTimer = new System.Windows.Threading.DispatcherTimer();
        private void RadButton_Activate(object sender, RoutedEventArgs e)
        {
            _doubleClickTimer.Interval = new TimeSpan(0, 0, 0, 0, 300);
            _doubleClickTimer.Tick += new EventHandler(DoubleClick_Timer);

            if (_doubleClickTimer.IsEnabled)
            {
                if (!string.IsNullOrEmpty(_currentPat.Ljdm))
                {
                    OnAfterPatientCardDoubleClicked(_currentPat);
                }
            }
            else
            {
                btn.CommandParameter = DateTime.Now;
                OnAfterPatientCardClicked(_currentPat);
            }

            _doubleClickTimer.Start();

        }

        void DoubleClick_Timer(object sender, EventArgs e)
        {
            _doubleClickTimer.Stop();
        }

    }
}
