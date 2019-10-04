using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using Telerik.Windows.Controls;
using YidanEHRApplication.Helpers;
using YidanEHRApplication.Views.UserControls;
using System.Collections.ObjectModel;
using YidanEHRApplication.Models;
using YidanEHRApplication.DataService;

namespace YidanEHRApplication.Views.ChildWindows
{
   /// <summary>
   /// Interaction logic for RadChildWindowPathVariation.xaml
   /// </summary>
   public partial class RWPathVariation
   {
       Boolean IsPostBack = false;
      #region members
      /// <summary>
      /// 必选 未执行医嘱
      /// </summary>
      public List<CP_DoctorOrder> ListUnDoOrder
      { get; set; }

      /// <summary>
      /// 必选 未执行医嘱
      /// </summary>
      public List<CP_DoctorOrder> ListUnNewOrder
      { get; set; }

      /// <summary>
      /// Ljdm
      /// </summary>
      public string PathLjdm
      { get; set; }

      /// <summary>
      /// stackpanel里，所有 必执行，未选原因 块
      /// </summary>
      private List<UCUnEnforceReason> m_UserControlUnEnforceReason = new List<UCUnEnforceReason>();

      /// <summary>
      /// stackpanel里，所有 新增 原因 块
      /// </summary>
      private List<UCUnNewReason> m_UserControlUnNewReason = new List<UCUnNewReason>();

      ///// <summary>
      ///// stackpanel里，所有 其它 原因 块
      ///// </summary>
      //private List<UserControlUnEnforceReason> m_UserControlUnOtherReason = new List<UserControlUnEnforceReason>();


      private ObservableCollection<CP_VariantRecords> m_ListUnEnforceReason = new ObservableCollection<CP_VariantRecords>();
      /// <summary>
      /// 必执行，未选原因
      /// </summary>
      public ObservableCollection<CP_VariantRecords> ListUnEnforceReason
      {
         get { return m_ListUnEnforceReason; }
         private set { m_ListUnEnforceReason = value; }
      }


      private ObservableCollection<CP_VariantRecords> m_ListUnNewReason = new ObservableCollection<CP_VariantRecords>();
      /// <summary>
      /// 新增原因
      /// </summary>
      public ObservableCollection<CP_VariantRecords> ListUnNewReason
      {
         get { return m_ListUnNewReason; }
         private set { m_ListUnNewReason = value; }
      }


      private ObservableCollection<CP_VariantRecords> m_ListUnOtherReason = new ObservableCollection<CP_VariantRecords>();
      /// <summary>
      /// OTHER原因
      /// </summary>
      public ObservableCollection<CP_VariantRecords> ListUnOtherReason
      {
         get { return m_ListUnOtherReason; }
         private set { m_ListUnOtherReason = value; }
      }

      /// <summary>
      /// 当前结点
      /// </summary>
      public String CurrentActivityId
      { get; set; }

      private const string m_Title = "医嘱变异原因";
      #endregion

      public RWPathVariation()
      {
         InitializeComponent();
      }

      private void LayoutRoot_Loaded(object sender, RoutedEventArgs e)
      {
          if (IsPostBack) { return; }
          else { IsPostBack = true; }
          try{
         if (ListUnDoOrder.Count > 0)
            this.RadTabItemMust.Header += "*";
         if (ListUnNewOrder.Count > 0)
            this.RadTabItemNew.Header += "*";
         //判断如果有必选未执行的默认显示必选未执行变异原因页面、如果没有必选未执行医嘱有新增医嘱则显示新增医嘱变异原因页面 add by yxy 2013-03-18
         if (ListUnDoOrder.Count == 0 && ListUnNewOrder.Count > 0)
         {
             this.RadTabItemNew.IsSelected = true;  
         }
         YidanEHRDataServiceClient client = PublicMethod.YidanClient;
         client.GetPathVariationInfoCompleted +=
             (obj, ea) =>
             {
                 if (ea.Error == null)
                 {
                     List<CP_PathVariation> listPathVariation = ea.Result.ToList();
                     double height = stakPanelReason1.ActualHeight;

                     int count = ListUnDoOrder.Count;
                     int location = 0;
                     for (int i = 0; i < count; i++)
                     {
                         CP_DoctorOrder order = ListUnDoOrder[i];
                         if (location == 3)
                             location = 0;
                         InitUndoShowInfo(location, listPathVariation, order);
                         location++;
                     }

                     int couuntNew = ListUnNewOrder.Count;
                     int locationNew = 0;
                     for (int i = 0; i < couuntNew; i++)
                     {
                         CP_DoctorOrder order = ListUnNewOrder[i];
                         if (locationNew == 3)
                             locationNew = 0;
                         InitNewShowInfo(locationNew, listPathVariation, order);
                         locationNew++;
                     } 
                 }
                 else
                 {
                     PublicMethod.RadWaringBox(ea.Error);
                 }
             };
         //CurrentActivityId = String.IsNullOrEmpty(CurrentActivityId) ? "56a0ca57-0bbb-4a7f-a6f0-f0a8f32ff697" : CurrentActivityId;
         client.GetPathVariationInfoAsync(PathLjdm, CurrentActivityId);
         client.CloseAsync(); }
         catch (Exception ex)
         {
            YidanEHRApplication.Models.PublicMethod.ClientException(ex, this.GetType().FullName, true);
         }

      }

      

      private void InitUndoShowInfo(int location, List<CP_PathVariation> listPathVariation, CP_DoctorOrder order)
      {
         StackPanel stanPanel = null;
         if (location == 0)
         {
            stanPanel = stakPanelReason1;
         }
         else if (location == 1)
         {
            stanPanel = stakPanelReason2;
         }
         else if (location == 2)
         {
            stanPanel = stakPanelReason3;
         }
         if (listPathVariation.Count > 0)
         {
             TextBlock textBlock = new TextBlock();
             textBlock.Text = order.Yznr + ":";
             textBlock.FontWeight = FontWeights.Bold;
             textBlock.SetValue(ToolTipService.ToolTipProperty, order.Yznr);
             UCUnEnforceReason reason = new UCUnEnforceReason();
             reason.ListPathVariation = listPathVariation;
             reason.DoctorOrder = order;
             reason.Ljdm = PathLjdm;
             reason.CurrentActivityId = CurrentActivityId;
             stanPanel.Children.Add(reason);
         }
         //如果无变异原因 默认显示其他原因
         else
         {
             TextBlock textBlock = new TextBlock();
             textBlock.Text = order.Yznr + ":";
             textBlock.FontWeight = FontWeights.Bold;
             textBlock.SetValue(ToolTipService.ToolTipProperty, order.Yznr);
             UCUnEnforceReason reason = new UCUnEnforceReason();
             reason.ListPathVariation = listPathVariation;
             reason.DoctorOrder = order;
             reason.Ljdm = PathLjdm;
             reason.CurrentActivityId = CurrentActivityId;
             stanPanel.Children.Add(reason);
             
         }
      }

      private void InitNewShowInfo(int location, List<CP_PathVariation> listPathVariation, CP_DoctorOrder order)
      {
         StackPanel stanPanel = null;
         if (location == 0)
         {
            stanPanel = stakPanelReason4;
         }
         else if (location == 1)
         {
            stanPanel = stakPanelReason5;
         }
         else if (location == 2)
         {
            stanPanel = stakPanelReason6;
         }
         if (listPathVariation.Count > 0)
         {
            TextBlock textBlock = new TextBlock();
            textBlock.Text = order.Yznr + ":";
            textBlock.FontWeight = FontWeights.Bold;
            textBlock.SetValue(ToolTipService.ToolTipProperty, order.Yznr);
            UCUnNewReason reason = new UCUnNewReason();
            reason.ListPathVariation = listPathVariation;
            reason.DoctorOrder = order;
            reason.Ljdm = PathLjdm;
            reason.CurrentActivityId = CurrentActivityId;
            stanPanel.Children.Add(reason);
         }
             //如果无变异原因 默认显示其他原因
         else
         {
             TextBlock textBlock = new TextBlock();
             textBlock.Text = order.Yznr + ":";
             textBlock.FontWeight = FontWeights.Bold;
             textBlock.SetValue(ToolTipService.ToolTipProperty, order.Yznr);
             UCUnNewReason reason = new UCUnNewReason();
             reason.ListPathVariation = listPathVariation;
             reason.DoctorOrder = order;
             reason.Ljdm = PathLjdm;
             reason.CurrentActivityId = CurrentActivityId;
             stanPanel.Children.Add(reason); 
         }
      }

      private void CancelButton_Click(object sender, RoutedEventArgs e)
      {
         this.DialogResult = false;
         this.Close();

      }

      private void OKButton_Click(object sender, RoutedEventArgs e)
      {
         if (!Check())
            return;
         this.DialogResult = true;
         this.Close();

      }

      private bool Check()
      {
         GetStackUndoAllObject(stakPanelReason1);
         GetStackUndoAllObject(stakPanelReason2);
         GetStackUndoAllObject(stakPanelReason3);
         foreach (UCUnEnforceReason info in m_UserControlUnEnforceReason)
         {
            if (info.SelectUnForceItems.Bydm == null || info.SelectUnForceItems.Bydm == string.Empty)
            {
               m_ListUnEnforceReason.Clear();
               PublicMethod.RadAlterBox("医嘱【" + info.DoctorOrder.Yznr + "】未填写原因", m_Title);
               this.RadTabItemMust.IsSelected = true;
               return false;
            }
            else
            {
               m_ListUnEnforceReason.Add(info.SelectUnForceItems);
            }
         }


         GetStackNewAllObject(stakPanelReason4);
         GetStackNewAllObject(stakPanelReason5);
         GetStackNewAllObject(stakPanelReason6);
         foreach (UCUnNewReason info in m_UserControlUnNewReason)
         {
            if (info.SelectUnForceItems.Bydm == null || info.SelectUnForceItems.Bydm == string.Empty)
            {
               m_ListUnEnforceReason.Clear();
               PublicMethod.RadAlterBox("医嘱【" + info.DoctorOrder.Yznr + "】未填写原因", m_Title);
               this.RadTabItemNew.IsSelected = true;
               return false;
            }
            else
            {
               m_ListUnEnforceReason.Add(info.SelectUnForceItems);
            }
         }
         return true;
      }

      /// <summary>
      /// 将stackpanel里所有OBJECT放入LIST,然后再判断
      /// </summary>
      /// <param name="stackPanel"></param>
      private void GetStackUndoAllObject(StackPanel stackPanel)
      {
         ////方法一：
         //List<UIElement> canvas = stakPanelReason1.Children.Where(o => o.GetType() == typeof(UserControlUnEnforceReason)).ToList();
         //方法二
         for (int i = 0; i < stackPanel.Children.Count; i++)
         {
             UCUnEnforceReason info = stackPanel.Children[i] as UCUnEnforceReason; //如果类型不一致则返回null
            if (info != null)
            {
               m_UserControlUnEnforceReason.Add(info);
            }
         }
      }

      /// <summary>
      /// 将stackpanel里所有OBJECT放入LIST,然后再判断
      /// </summary>
      /// <param name="stackPanel"></param>
      private void GetStackNewAllObject(StackPanel stackPanel)
      {
         ////方法一：
         //List<UIElement> canvas = stakPanelReason1.Children.Where(o => o.GetType() == typeof(UserControlUnEnforceReason)).ToList();
         //方法二
         for (int i = 0; i < stackPanel.Children.Count; i++)
         {
             UCUnNewReason info = stackPanel.Children[i] as UCUnNewReason; //如果类型不一致则返回null
            if (info != null)
            {
               m_UserControlUnNewReason.Add(info);
            }
         }
      }

      private void RadWindow_Closed(object sender, WindowClosedEventArgs e)
      {
         //this.DialogResult = false;
      }
   }
}
