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
using System.Windows.Navigation;
using YidanEHRApplication.YidanEHRServiceReference;
using YidanEHRApplication.Models;
using Telerik.Windows.Controls;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Pass
{
    public partial class ClinicalMedicines : Page
    {

        YidanEHRDataServiceClient serviceCon;

        List<DrugInfoPass> drugList = new List<DrugInfoPass>();
        public ClinicalMedicines()
        {
            InitializeComponent();
        }

        private RadTreeViewItem AddItem(string header, DrugInfoPass drug, RadTreeViewItem treeViewItem)
        {

            RadTreeViewItem radTreeItem = (new RadTreeViewItem()
            {
                Header = header,

            }); try
            {
                radTreeItem.Tag = drug;
                radTreeItem.Selected += new EventHandler<Telerik.Windows.RadRoutedEventArgs>(radTreeItem_Selected);
                if (treeViewItem == null)
                {
                    treeViewItem = radTreeItem;
                    this.radTreeView1.Items.Add(radTreeItem);
                }
                else
                {
                    treeViewItem.Items.Add(radTreeItem);
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
            return radTreeItem;


        }

        private void radTreeItem_Selected(object sender, Telerik.Windows.RadRoutedEventArgs e)
        {
            try
            {
                RadTreeViewItem item = e.Source as RadTreeViewItem;
                if (item.Tag == null) return;
                DrugInfoPass drug = item.Tag as DrugInfoPass;
                InitDrugInfo(drug);
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }


        private void InitDrugInfo(DrugInfoPass drug)
        {
            try
            {
                txt_DrugTym.Text = drug.Yptym;
                txt_DrugSpm.Text = drug.Ypspm;
                txt_DrugLb.Text = "[" + drug.Ypdl + "]---" + "[" + drug.Yblb + "]";

                txt_DrugYf.Text = drug.Ypyf;
                //string[] stemp_val3 = drug.Ypyf.Trim().Split('。');
                //for (int h = 0; h < stemp_val3.Length; h++)
                //{
                //    txt_DrugYf.Text += stemp_val3[h] + "\n";
                //}

                //药品适用症 add by luff 2012-08-06
                txt_DrugSyz.Text = "";
                string[] stemp_val1 = drug.Ypsyz.TrimEnd('。').Replace("\n", "").Trim().Split('。');
                for (int j = 0; j < stemp_val1.Length; j++)
                {
                    if (stemp_val1.Length - 1 > 0)
                    {
                        txt_DrugSyz.Text += stemp_val1[j].Trim() + "。" + "\n";
                    }
                    else
                    {
                        txt_DrugSyz.Text = stemp_val1[j].Trim();
                    }
                    
                }

             
                txt_DrugJjz.Text = "";
                string[] stemp_val4 = drug.Ypjjz.TrimEnd('。').Replace("\n", "").Trim().Split('。');
                for (int m = 0; m < stemp_val4.Length; m++)
                {
                    if (stemp_val4.Length - 1 > 0)
                    {
                        txt_DrugJjz.Text += stemp_val4[m].Trim() + "。" + "\n";
                    }
                    else
                    {
                        txt_DrugJjz.Text = stemp_val4[m].Trim();
                    }
                    
                }

               


            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }
        }


        private void InitTreeView()
        {
            try
            {
                //三级菜单 
                HashSet<string> rootDrug = new HashSet<string>();
                HashSet<string> secordDrug = new HashSet<string>();
                HashSet<string> drug = new HashSet<string>();

                RadTreeViewItem rootItem = null;
                RadTreeViewItem secordItem = null;
                RadTreeViewItem drugItem = null;



                foreach (DrugInfoPass dr in drugList)
                {
                    if (!rootDrug.Contains(dr.Ypdl))
                    {
                        //新增根节点
                        rootDrug.Add(dr.Ypdl);
                        //新增二级节点
                        rootItem = AddItem(dr.Ypdl, null, null);
                        //新增叶子节点

                    }

                    if (!secordDrug.Contains(dr.Yblb) && (rootItem != null))
                    {
                        secordDrug.Add(dr.Yblb);
                        secordItem = AddItem(dr.Yblb, null, rootItem);
                    }

                    if (!drug.Contains(dr.Ypdm) && (secordItem != null))
                    {
                        drug.Add(dr.Ypdm);
                        drugItem = AddItem(dr.Yptym, dr, secordItem);
                    }
                }
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void LoadData()
        {
            try
            {
                this.radBusyIndicator.IsBusy = true;
                serviceCon = PublicMethod.YidanClient;
                serviceCon.GetPassDrugInfoCompleted +=
                (obj, e) =>
                {
                    if (e.Error == null)
                    {
                        drugList = e.Result.ToList();
                        InitTreeView();
                        this.radBusyIndicator.IsBusy = false;
                        autoCompleteBox1.ItemsSource = drugList;
                        autoCompleteBox1.ItemFilter = DeptFilter;
                        autoCompleteBox1.ValueMemberPath = "Ypspm";
                         //   autoCompleteBox1.ValueMemberBinding
                       
                    }
                    else
                        PublicMethod.RadWaringBox(e.Error);
                };
                serviceCon.GetPassDrugInfoAsync();
                serviceCon.CloseAsync();
            }
            catch (Exception ex)
            {
                PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }
        public bool DeptFilter(string strFilter, object item)
        {
            DrugInfoPass deptList = (DrugInfoPass)item;
            return ((deptList.Py.StartsWith(strFilter.ToUpper())) || (deptList.Py.Contains(strFilter.ToUpper())) || deptList.Ypspm.Contains(strFilter.ToUpper()));
        }



        void serviceCon_GetDrugInfoCompleted(object sender, GetDrugInfoCompletedEventArgs e)
        {

        }

        // 当用户导航到此页面时执行。
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        private void Page_Loaded(object sender, RoutedEventArgs e)
        {
            try
            {
                LoadData();
            }
            catch (Exception ex)
            {
                YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
            }

        }

        private void autoCompleteBox1_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if(autoCompleteBox1.SelectedItem!=null)

            // txt_DrugSyz.Text = string.Empty;
            //txt_DrugJjz.Text = string.Empty;
                
            InitDrugInfo((DrugInfoPass)autoCompleteBox1.SelectedItem);
        }

    }
}
