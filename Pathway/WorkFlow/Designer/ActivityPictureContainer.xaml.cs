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
using YidanEHRApplication.WorkFlow.ActivityPicture;

namespace YidanEHRApplication.WorkFlow.Designer
{
    public delegate void ColseAnimationCompletedDelegate(object sender, EventArgs e);

    public partial class ActivityPictureContainer : UserControl
    {
        public double ContainerWidth
        {
            set
            {

                gridContainer.Width = value;
            }
            get
            {
                return gridContainer.Width;
            }
        }
        public double ContainerHeight
        {
            set
            {

                gridContainer.Height = value;
            }
            get
            {
                return gridContainer.Height;
            }
        }

        public double PictureWidth
        {
            get
            {
                return ((IActivityPicture)currentPic).PictureWidth;
            }
            set
            {
                ((IActivityPicture)currentPic).PictureWidth = value;
            }
        }
        public double PictureHeight
        {
            get
            {
                return ((IActivityPicture)currentPic).PictureHeight;
            }
            set
            {
                ((IActivityPicture)currentPic).PictureHeight = value;
            }
        }

        UserControl currentPic;
        public ActivityPictureContainer()
        {
            InitializeComponent();
        }
        public new SolidColorBrush Background
        {
            set
            {
                ((IActivityPicture)currentPic).Background = value;
            }
        }
        ActivityType type;
        public ActivityType Type
        {
            get
            {
                return type;
            }
            set
            {
                type = value;


                picAUTOMATION.PictureVisibility = Visibility.Collapsed;
                picBegin.PictureVisibility = Visibility.Collapsed;
                picBRANCH.PictureVisibility = Visibility.Collapsed;
                picEnd.PictureVisibility = Visibility.Collapsed;
                picINTERACTION.PictureVisibility = Visibility.Collapsed;
                picMERGE.PictureVisibility = Visibility.Collapsed;

                if (type == ActivityType.INTERACTION)
                {
                    currentPic = picINTERACTION;

                }
                else if (type == ActivityType.COMPLETION)
                {
                    currentPic = picEnd;
                }
                else if (type == ActivityType.INITIAL)
                {
                    currentPic = picBegin;
                }
                else if (type == ActivityType.AUTOMATION
                     || type == ActivityType.DUMMY
                    || type == ActivityType.SUBPROCESS)
                {
                    currentPic = picAUTOMATION;
                }
                else if (type == ActivityType.AND_BRANCH
                    || type == ActivityType.OR_BRANCH)
                {
                    currentPic = picBRANCH;
                }
                else if (type == ActivityType.AND_MERGE
                    || type == ActivityType.OR_MERGE
                    || type == ActivityType.VOTE_MERGE)
                {
                    currentPic = picMERGE;
                }
                ((IActivityPicture)currentPic).PictureVisibility = Visibility.Visible;
            }
        }

        public string AcitivtyName
        {
            get { return txtActivityName.Text; }
            set { txtActivityName.Text = value; }
        }
        public void SetSelectedColor()
        {
            ((IActivityPicture)currentPic).SetSelectedColor();
        }
        public void SetWarningColor()
        {
            ((IActivityPicture)currentPic).SetWarningColor();
        }
        public void ResetInitColor()
        {
            ((IActivityPicture)currentPic).ResetInitColor();
        }
        public Point GetPointOfIntersection(Point beginPoint, Point endPoint, RuleMoveType type)
        {
            return new Point(0, 0);
        }
        public PointCollection ThisPointCollection
        {
            get
            {
                return ((IActivityPicture)currentPic).ThisPointCollection;
            }
        }
        MergePictureRepeatDirection _repeatDirection;
        public MergePictureRepeatDirection RepeatDirection
        {
            get
            {
                if (Type == ActivityType.OR_MERGE
                    || Type == ActivityType.AND_MERGE
                    || Type == ActivityType.VOTE_MERGE)
                {

                    _repeatDirection = ((MergeActivity)currentPic).RepeatDirection;
                }
                return _repeatDirection;
            }
            set
            {
                _repeatDirection = value;
                if (Type == ActivityType.OR_MERGE
                    || Type == ActivityType.AND_MERGE
                    || Type == ActivityType.VOTE_MERGE)
                {

                    ((MergeActivity)currentPic).RepeatDirection = _repeatDirection;
                }
            }
        }

        public ElementState CurrentElementState
        {
            set
            {
                ((IActivityPicture)currentPic).CurrentElementState = value;
            }
        }
        //laolaowhn
        public String CurrentStep
        {
            set
            {
                txtTip.Text = value;
            }
        }
    }
}
