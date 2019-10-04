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

namespace YidanEHRApplication.Controls
{
    public partial class UCImagePanelControl : UserControl
    {
        //string m_imgLeft = "";
        //string m_imgCenter = "";
        //string m_imgRight = "";

        //public string LeftPicture
        //{
        //    get { return m_imgLeft; }
        //    set { 
        //            m_imgLeft = value;
        //            Uri uri = new Uri(value, UriKind.RelativeOrAbsolute);
        //            ImageSource imgSource = new System.Windows.Media.Imaging.BitmapImage(uri);
        //            imgLeft.SetValue(Image.SourceProperty, imgSource);
        //         }
        //}

        //public string CenterPicture
        //{
        //    get { return m_imgCenter; }
        //    set { 
        //        m_imgCenter = value;
        //        Uri uri = new Uri(value, UriKind.RelativeOrAbsolute);
        //        ImageSource imgSource = new System.Windows.Media.Imaging.BitmapImage(uri);
        //        imgCenter.SetValue(Image.SourceProperty, imgSource);
        //    }
        //}

        //public string RightPicture
        //{
        //    get { return m_imgRight; }
        //    set { 
        //            m_imgRight = value;
        //            Uri uri = new Uri(value, UriKind.RelativeOrAbsolute);
        //            ImageSource imgSource = new System.Windows.Media.Imaging.BitmapImage(uri);
        //            imgRight.SetValue(Image.SourceProperty, imgSource);
        //        }
        //}
        
        public UCImagePanelControl()
        {
            InitializeComponent();
            //m_imgLeft = imgLeft.Source.ToString();
            //m_imgCenter = imgCenter.Source.ToString();
            //m_imgRight = imgRight.Source.ToString();
        }
    }
}
