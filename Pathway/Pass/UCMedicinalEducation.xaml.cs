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
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Pass
{
    public partial class UCMedicinalBook : UserControl
    {
               List<CP_MedicinalEducation> manuallist = new List<CP_MedicinalEducation>();
        private bool foldActivated;
        private int index = -1; //导航tag
        static int i = 0; //TreeViewItem图片tag

        public UCMedicinalBook()
        {
            InitializeComponent();
            radTreeView_ml.AddHandler(RadTreeViewItem.MouseLeftButtonDownEvent, new MouseButtonEventHandler(RadTreeViewItem_MouseLeftButtonDown), true);
           
            BindData();
        }
       
        

        

        #region 初始化TreeView
        private void InitTreeView()
        {
            List<String> wtlblist = new List<String>();
            List<String> wtnrlist = new List<String>();
            RadTreeViewItem rootItem = null;
            RadTreeViewItem secordItem = null;

            foreach (CP_MedicinalEducation edu in manuallist)
            {
                //遍历到不同的问题类别名称，就把它添加到根节点下
                if (!wtlblist.Contains(edu.Wtlb))
                {
                    wtlblist.Add(edu.Wtlb);
                    rootItem = AddItem(edu.Wtlb, ref index, null);
                    index--;//目的是让根节点和它下面的第一个子节点指向同一页

                    
                }

                //在此问题类别下遍历到不同的问题内容，就把它添加到该问题类别下
                if (!wtnrlist.Contains(edu.Wtnr)&&(rootItem!=null))
                {
                    wtnrlist.Add(edu.Wtnr);
                    secordItem = AddItem(edu.Wtnr, ref index, rootItem);
                }
            }
            
        }

        //为TreeView添加Item的操作
        private RadTreeViewItem AddItem(string header,ref int index, RadTreeViewItem treeViewItem)
        {

            RadTreeViewItem radTreeItem = new RadTreeViewItem()
            {
               Header=header,

            }; try
            {

                radTreeItem.Tag = ++index;
                radTreeItem.Cursor = Cursors.Hand;
                
                if (treeViewItem == null)
                {
                    i = 0;
                    this.radTreeView_ml.Items.Add(radTreeItem);
                }
                else
                {
                    radTreeItem.DefaultImageSrc = string.Format("../Images/Icons/TreeView{0}.png",++i%14);
                    treeViewItem.Items.Add(radTreeItem);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            return radTreeItem;


        }

        //为TreeViewItem添加动态索引功能
        void RadTreeViewItem_MouseLeftButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            if (this.foldActivated)
            {
                return;
            }

            RadTreeViewItem item = (e.OriginalSource as FrameworkElement).ParentOfType<RadTreeViewItem>();
            if (item == null) return;

            object tag = item.Tag;
            if (tag == null) return;

            int pageIndex = Int16.Parse(tag.ToString());
            if (pageIndex >= 0)
            {
                book.RightPageIndex = pageIndex;
            }
        }

        #endregion

        #region 初始化书中的内容
        private void BindData()
        {
            this.radBusyIndicator.IsBusy = true;
            YidanEHRDataServiceClient client = PublicMethod.YidanClient;
            client.GetMedicinalEducationInfoAsync();
            client.GetMedicinalEducationInfoCompleted +=
               (obj, ea) =>
               {
                   if (ea.Error == null)
                   {
                       manuallist = ea.Result.ToList();
                       this.book.ItemsSource = manuallist;
                       InitTreeView();
                       this.radBusyIndicator.IsBusy = false;
                   }
                   else
                   {
                       throw new NotImplementedException();
                   }
               };
            client.CloseAsync();
       
        }

        

        private void book_FoldActivated(object sender, Telerik.Windows.Controls.FoldEventArgs e)
        {
            this.foldActivated = true;

        }

        private void book_FoldDeactivated(object sender, Telerik.Windows.Controls.FoldEventArgs e)
        {
            this.foldActivated = false;

        }
        #endregion

    }
}
