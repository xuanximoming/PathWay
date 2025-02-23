﻿using System;
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
using YidanEHRApplication.WorkFlow.Designer;
using YidanEHRApplication.WorkFlow.Component;


namespace YidanEHRApplication.WorkFlow.ActivityPicture
{

    public partial class MergeActivity : UserControl, IActivityPicture
    {
        MergePictureRepeatDirection _repeatDirection = MergePictureRepeatDirection.None;
        public MergePictureRepeatDirection RepeatDirection
        {
            get
            {
                if (_repeatDirection == MergePictureRepeatDirection.None)
                {
                    if (picMERGE.Width > picMERGE.Height)
                        _repeatDirection = MergePictureRepeatDirection.Horizontal;
                    else
                        _repeatDirection = MergePictureRepeatDirection.Vertical;
                }

                return _repeatDirection;
            }
            set
            {
                _repeatDirection = value;
                if (_repeatDirection == MergePictureRepeatDirection.Vertical)
                {
                    picMERGE.Height = 60.0;
                    picMERGE.Width = 20.0;
                }
                else
                {
                    picMERGE.Height = 20.0;
                    picMERGE.Width = 60.0;
                }
            }
        }
        public MergeActivity()
        {
            InitializeComponent();
        }
        public new double Opacity
        {
            set { picMERGE.Opacity = value; }
            get { return picMERGE.Opacity; }
        }
        public double PictureWidth
        {
            set { picMERGE.Width = value; }
            get { return picMERGE.Width; }
        }
        public double PictureHeight
        {
            set { picMERGE.Height = value; }
            get { return picMERGE.Height; }
        }
        public new Brush Background
        {
            set
            {
                picMERGE.Fill = value;
            }
            get { return picMERGE.Fill; }
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
            picMERGE.Fill = brush;
            brush = new SolidColorBrush();
            brush.Color = Colors.Green;
            picMERGE.Stroke = brush;
        }
        public void SetWarningColor()
        {
            picMERGE.Fill = SystemConst.ColorConst.WarningColor;

        }
        public void SetSelectedColor()
        {
            picMERGE.Fill = SystemConst.ColorConst.SelectedColor;

        }

        public PointCollection ThisPointCollection
        {
            get { return null; }
        }
        public ElementState CurrentElementState
        {
            set { }
        }
    }
}
