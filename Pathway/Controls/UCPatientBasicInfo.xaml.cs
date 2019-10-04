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
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Controls
{
    public partial class UCPatientBasicInfo : UserControl
    {


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
        private CP_InpatinetList _currentPat;

        public UCPatientBasicInfo()
        {
            InitializeComponent();
            InitVars();
        }


        private void InitVars()
        {
            label_Age.Content = string.Empty;
            label_Bed.Content = string.Empty;
            label_Name.Content = string.Empty;
            label_PatNo.Content = string.Empty;
            label_Ryrq.Content = string.Empty;
            label_Sex.Content = string.Empty;
            label_Zd.Content = string.Empty;
        }

        private void Bind()
        {
            if (_currentPat == null)
            {
                InitVars();
            }
            else
            {
                label_Age.Content = _currentPat.Xsnl;
                label_Bed.Content = _currentPat.Bed;
                label_Name.Content = _currentPat.Hzxm;
                label_PatNo.Content = _currentPat.Zyhm;
                label_Ryrq.Content = _currentPat.Ryrq.Substring(0,19);
                label_Sex.Content = _currentPat.Brxb;
                label_Zd.Content = _currentPat.Ryzd;
            }

        }


    }
}
