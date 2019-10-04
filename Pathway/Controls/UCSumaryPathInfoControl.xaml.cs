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
using YidanEHRApplication.Helpers;
using YidanEHRApplication.DataService;
using Telerik.Windows.Controls;
using YidanEHRApplication.Models;

namespace YidanEHRApplication.Controls
{
    public partial class UCQcPatientInfoControl : UserControl
    {

        const string str_all = "全部 ";
        const string str_new = "未引入 ";
        const string str_quit = "中途退出 ";
        const string str_notin = "未通过评估 ";
        const string str_do = "执行中 ";
        const string str_complete = "已完成 ";
        public string DoctorID;
        //internal List<CP_InpatinetList> InpatientList
        //{
        //    get { return _inpatientList; }
        //    set
        //    {
        //        _inpatientList = value;
        //        if (_inpatientList != null)
        //        {
        //            Bind();

        //        }
        //    }
        //}
        private List<CP_InpatinetList> _inpatientList;

        public UCQcPatientInfoControl()
        {


            InitializeComponent();
            InitVars();
            //spnAll.Background = YidanSoft.Tool.ConvertColor.GetColorBrushFromHx16("6F8FFF");
        }
        public void getCount(string news, string complete, string dos, string notin, string quit)
        {
            lblAll.Content = str_all + "(" + (Convert.ToInt32(news) + Convert.ToInt32(complete) + Convert.ToInt32(dos) + Convert.ToInt32(notin) + Convert.ToInt32(quit)) + ")";
            lblNotIn.Content = str_notin + "(" + notin + ")";
            lblNew.Content = str_new + "(" + news + ")";
            lblDo.Content = str_do + "(" + dos + ")";
            lblComplete.Content = str_complete + "(" + complete + ")";
            lblQuit.Content = str_quit + "(" + quit + ")";
        }

        private void InitVars()
        {


            //GetPageCount("");
            //GetPageCount("3");
            //GetPageCount("1");
            //GetPageCount("-1");
            //GetPageCount("4");
            //GetPageCount("2");

            btnAll.Tag = PathStatus.None;
            //btnAll.Click += new RoutedEventHandler(btn_Click);
            btnAll.MouseLeftButtonDown += new MouseButtonEventHandler(btn_Click);
            btnComplete.Tag = PathStatus.DonePath;
            //btnComplete.Click += new RoutedEventHandler(btn_Click);
            btnComplete.MouseLeftButtonDown += new MouseButtonEventHandler(btn_Click);
            btnDo.Tag = PathStatus.InPath;
            //btnDo.Click += new RoutedEventHandler(btn_Click);
            btnDo.MouseLeftButtonDown += new MouseButtonEventHandler(btn_Click);
            btnNew.Tag = PathStatus.New;
            //btnNew.Click += new RoutedEventHandler(btn_Click);
            btnNew.MouseLeftButtonDown += new MouseButtonEventHandler(btn_Click);
            btnNotIn.Tag = PathStatus.NotIn;
            //btnNotIn.Click += new RoutedEventHandler(btn_Click);
            btnNotIn.MouseLeftButtonDown += new MouseButtonEventHandler(btn_Click);
            btnQuit.Tag = PathStatus.QuitPath;
            //btnQuit.Click += new RoutedEventHandler(btn_Click);
            btnQuit.MouseLeftButtonDown += new MouseButtonEventHandler(btn_Click);


            lblAll.Tag = PathStatus.None;
            lblAll.Click += new RoutedEventHandler(btn_Click);
            lblComplete.Tag = PathStatus.DonePath;
            lblComplete.Click += new RoutedEventHandler(btn_Click);
            lblDo.Tag = PathStatus.InPath;
            lblDo.Click += new RoutedEventHandler(btn_Click);
            lblNew.Tag = PathStatus.New;
            lblNew.Click += new RoutedEventHandler(btn_Click);
            lblNotIn.Tag = PathStatus.NotIn;
            lblNotIn.Click += new RoutedEventHandler(btn_Click);
            lblQuit.Tag = PathStatus.QuitPath;
            lblQuit.Click += new RoutedEventHandler(btn_Click);
        }


        private void GetPageCount(string ljzt)
        {

            //YidanEHRDataServiceClient referenceClient = PublicMethod.YidanClient;
            //referenceClient.GetInpatientCountCompleted +=
            //    (obj, e) =>
            //    {
            //        if (e.Error == null)
            //        {
            //            if (ljzt =="")
            //            {
            //                lblAll.Content = str_all + "("+ e.Result +")";
            //              }
            //            if (ljzt == "4")
            //            {
            //                lblNotIn.Content = str_notin + "(" + e.Result + ")";
            //            }
            //            if (ljzt == "-1")
            //            {
            //                lblNew.Content = str_new + "(" + e.Result + ")";
            //            }
            //            if (ljzt =="1")
            //            {
            //                lblDo.Content = str_do + "(" + e.Result + ")";
            //            }
            //            if (ljzt == "3")
            //            {
            //                lblComplete.Content = str_complete + "(" + e.Result + ")";
            //            }
            //            if (ljzt == "2")
            //            {
            //                lblQuit.Content = str_quit + "(" + e.Result + ")";
            //            }

            //        }
            //        else
            //        {
            //            PublicMethod.RadWaringBox(e.Error);
            //        }
            //    };
            //int querykind = string.IsNullOrEmpty(DoctorID) ? 3 : 2;


            //referenceClient.GetInpatientCountAsync(querykind, Global.LogInEmployee.Ksdm, "","","", "", "", "", ljzt);
            //referenceClient.CloseAsync();

        }
        private PathStatus _CurrentPathStatus = PathStatus.None;
        /// <summary>
        /// 当前选中的按钮
        /// </summary>
        public PathStatus CurrentPathStatus
        {
            get
            {
                return _CurrentPathStatus;
            }
            set { _CurrentPathStatus = value; }
        }
        void btn_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                if (sender.GetType().Name == "HyperlinkButton")
                {
                    HyperlinkButton CurrentRadButton = (HyperlinkButton)sender;
                    CurrentPathStatus = (PathStatus)CurrentRadButton.Tag;
                    OnAfterSumaryInfoClicked(new QcSumaryChanageArgs(CurrentPathStatus));
                }
                else if (sender.GetType().Name == "RadButton")
                {
                    RadButton CurrentRadButton = (RadButton)sender;
                    CurrentPathStatus = (PathStatus)CurrentRadButton.Tag;
                    OnAfterSumaryInfoClicked(new QcSumaryChanageArgs(CurrentPathStatus));
                }

                else if (sender.GetType().Name == "Image")
                {
                    Image CurrentRadButton = (Image)sender;
                    CurrentPathStatus = (PathStatus)CurrentRadButton.Tag;
                    OnAfterSumaryInfoClicked(new QcSumaryChanageArgs(CurrentPathStatus));
                }

            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        //private String _lblQuitCount;
        //public String lblQuitCount { get { if (_lblQuitCount == null) _lblQuitCount = "0"; return lblQuitCount; } set { _lblQuitCount = value; } }

        //private String _lblNotInCount;
        //public String lblNotInCount { get { if (_lblQuitCount == null) _lblQuitCount = "0"; return lblQuitCount; } set { _lblQuitCount = value; } }

        //private String _lblNewCount;
        //public String lblNewCount { get { if (_lblQuitCount == null) _lblQuitCount = "0"; return lblQuitCount; } set { _lblQuitCount = value; } }

        //private String _lblDoCount;
        //public String lblQuitCount { get { if (_lblQuitCount == null) _lblQuitCount = "0"; return lblQuitCount; } set { _lblQuitCount = value; } }

        //private String _lblCompleteCount;
        //public String lblCompleteCount { get { if (_lblQuitCount == null) _lblQuitCount = "0"; return lblQuitCount; } set { _lblQuitCount = value; } }

        //private String _lblAllCount;
        //public String lblAllCount { get { if (_lblQuitCount == null) _lblQuitCount = "0"; return lblQuitCount; } set { _lblQuitCount = value; } }
        public void UCQcPatientInfoControlBind(String lblQuitCount, String lblNotInCount, String lblNewCount, String lblDoCount, String lblCompleteCount, String lblAllCount)
        {
            lblQuit.Content = str_quit + lblQuitCount;
            lblNotIn.Content = str_notin + lblNotInCount;
            lblNew.Content = str_new + lblNewCount;
            lblDo.Content = str_do + lblDoCount;
            lblComplete.Content = str_complete + lblCompleteCount;
            lblAll.Content = str_all + lblAllCount;

        }


        public delegate void SumaryInfoClicked(object sender, RoutedEventArgs e);
        public event SumaryInfoClicked AfterSumaryInfoClicked;

        protected virtual void OnAfterSumaryInfoClicked(QcSumaryChanageArgs e)
        {
            if (AfterSumaryInfoClicked != null)
            {
                AfterSumaryInfoClicked(this, e);
            }

        }



    }



    public class QcSumaryChanageArgs : RoutedEventArgs
    {
        public PathStatus Status
        {
            get { return _status; }
        }
        private PathStatus _status;

        public QcSumaryChanageArgs(PathStatus status)
        {
            _status = status;
        }


    }

    //public enum PathStatus
    //{
    //    /// <summary>
    //    /// 新人
    //    /// </summary>
    //    NewPat = -1,
    //    /// <summary>
    //    /// 执行中
    //    /// </summary>
    //    DoPat = 1,
    //    /// <summary>
    //    /// 退出
    //    /// </summary>
    //    QuitPat = 2,
    //    /// <summary>
    //    /// 完成
    //    /// </summary>
    //    Compelte = 3,
    //    /// <summary>
    //    /// 未评估
    //    /// </summary>
    //    NotIn = 4,

    //}


    public class QcEventArgs : RoutedEventArgs
    {
        public string Hzxm
        {
            get { return _hzxm; }
            set { _hzxm = value; }
        }
        private string _hzxm = string.Empty;

        public string Zyhm
        {
            get { return _zyhm; }
            set { _zyhm = value; }
        }
        private string _zyhm = string.Empty;

        public string BedNo
        {
            get { return _bedNo; }
            set { _bedNo = value; }
        }
        private string _bedNo = string.Empty;
        //add by luff 20130227 添加科室为搜索条件
        public string Ksdm
        {
            get { return _ksdm; }
            set { _ksdm = value; }
        }
        private string _ksdm = string.Empty;
         
        //add by luff 20130305 添加病人状态为搜索条件
        public string Brzt
        {
            get { return _brzt; }
            set { _brzt = value; }
        }
        private string _brzt = string.Empty;
        public string StartDate
        {
            get { return _startDate; }
            set { _startDate = value; }
        }
        private string _startDate = string.Empty;

        public string EndDate
        {
            get { return _endDate; }
            set { _endDate = value; }
        }
        private string _endDate = string.Empty;



        public QcEventArgs()
        {

        }

        public QcEventArgs(string hzxm, string zyhm, string bedNo, string startDate, string endDate,string ksdm,string brzt)
        {
            _hzxm = hzxm;
            _bedNo = bedNo;
            _zyhm = zyhm;
            _startDate = startDate;
            _endDate = endDate;
            _ksdm = ksdm;
            _brzt = brzt;
        }

    }
}
