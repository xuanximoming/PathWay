using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Collections.ObjectModel;
using System.Drawing;
using DrectSoft.Resources;

namespace DrectSoft.JobManager
{
   /// <summary>
   /// 数据同步任务树
   /// </summary>
   public class SynchTreeView : TreeView
   {
      //public const string OtherSystem = "已加载的日志";

      public Collection<TreeNode> SelectedNodes
      {
         get
         {
            if (_selectedNodes == null)
               _selectedNodes = new Collection<TreeNode>();
            return _selectedNodes;
         }
      }
      private Collection<TreeNode> _selectedNodes;

      #region ctors
      public SynchTreeView(JobConfig jobConfig)
      {
         ImageList list = new ImageList();
         list.ImageSize = new Size(16, 16);
         list.Images.Add(ResourceManager.GetSmallIcon(ResourceNames.Perform, IconType.Normal), Color.Magenta);
         list.Images.Add(ResourceManager.GetSmallIcon(ResourceNames.Stop, IconType.Normal), Color.Magenta);
         list.Images.Add(ResourceManager.GetSmallIcon(ResourceNames.FolderOpen, IconType.Normal), Color.Magenta);
         list.Images.Add(ResourceManager.GetSmallIcon(ResourceNames.NewDocument, IconType.Normal), Color.Magenta);
         list.Images.Add(ResourceManager.GetSmallIcon(ResourceNames.Delete, IconType.Normal), Color.Magenta);
         this.ImageList = list;
         this.CheckBoxes = false;
         this.HideSelection = false;
         
         CreateTree(jobConfig);
        

         this.AfterSelect +=new TreeViewEventHandler(SynchTreeView_AfterSelect); 
      }
      #endregion

      #region event handler
      public event EventHandler<TreeViewEventArgs> SelectedNodeChanged;
      protected void OnSelectedNodeChanged(TreeViewEventArgs e)
      {
         if (SelectedNodeChanged != null)
            SelectedNodeChanged(this, e);
      }
      #endregion

      #region public methods
      //public TreeNode AddNode(SynchMissionConfigration config)
      //{
      //   if (config == null)
      //      return null;
      //   TreeNode rootNode = null, targetNode = null;
      //   if (string.IsNullOrEmpty(config.MissionSystemName))
      //   {//如果没有系统名称，全放在已加载日志树以下
      //      if (!this.Nodes.ContainsKey(config.MissionName))
      //      {
      //         if (this.Nodes.ContainsKey(OtherSystem))
      //            rootNode = this.Nodes[this.Nodes.IndexOfKey(OtherSystem)];
      //         else
      //            rootNode = this.Nodes.Add(OtherSystem, OtherSystem);
      //         SetNodeIcon(rootNode);
      //      }
      //      else
      //         return this.Nodes[this.Nodes.IndexOfKey(config.MissionName)];
      //   }
      //   else
      //   {//有系统名称，如果已经有这个节点，则放下面，否则新开一个系统节点
      //      if (this.Nodes.ContainsKey(config.MissionSystemName))
      //         rootNode = this.Nodes[this.Nodes.IndexOfKey(config.MissionSystemName)];
      //      else
      //         rootNode = this.Nodes.Add(config.MissionSystemName, config.MissionSystemName);
      //      SetNodeIcon(rootNode);
      //   }
      //   targetNode = rootNode.Nodes.Add(config.MissionName, config.MissionName);
      //   targetNode.Tag = config;
      //   SetNodeIcon(targetNode);
      //   return targetNode;
      //}

      public void SetNodeIcon(TreeNode node)
      {
         if (node == null)
            return;

         Job job = node.Tag as Job;

         if (job != null)
         {
             if (job.Action == null)
                 node.ImageIndex = 4;
             else
                 node.ImageIndex = job.Enable ? 0 : 1;
         }
         else //这个是顶级节点，标记为文件夹即可
             node.ImageIndex = 2;

         node.SelectedImageIndex = node.ImageIndex;
      }
      #endregion

      #region private 
      void SynchTreeView_AfterSelect(object sender, TreeViewEventArgs e)
      {
         OnSelectedNodeChanged(e);
      }

      //void SynchTreeView_AfterCheck(object sender, TreeViewEventArgs e)
      //{
      //   if (e.Action != TreeViewAction.ByMouse && e.Action != TreeViewAction.ByKeyboard)
      //      return;
      //   if (e.Node != null)
      //   {
      //      if (e.Node.Parent != null)
      //      {
      //         if (FindTreeNode(e.Node.Name) == -1)
      //         {
      //            SelectedNodes.Add(e.Node);
      //            e.Node.Parent.Checked = true;
      //         }
      //         else
      //         {
      //            bool check = false;
      //            SelectedNodes.Remove(e.Node);
      //            foreach (TreeNode node in e.Node.Parent.Nodes)
      //            {
      //               if (node.Checked)
      //               {
      //                  check = true;
      //                  break;
      //               }
      //            }
      //            e.Node.Parent.Checked = check;
      //         }

      //      }
      //      else
      //      {//暂时处理单击父节点全选子节点的功能，因为怕会引起堆栈溢出的问题，使逻辑处理很混乱
      //         if (e.Node.Checked)
      //         {
      //            foreach (TreeNode tnode in e.Node.Nodes)
      //            {
      //               if (FindTreeNode(tnode.Name) == -1)
      //               {
      //                  SelectedNodes.Add(tnode);
      //                  tnode.Checked = true;
      //               }
      //            }
      //         }
      //         else
      //         {
      //            foreach (TreeNode tnode2 in e.Node.Nodes)
      //            {
      //               SelectedNodes.Remove(tnode2);
      //               tnode2.Checked = false;
      //            }
      //         }
      //      }
      //   }
      //   OnAfterCheckNodes(new SynchTreeViewEventArgs(_selectedNodes));
      //}

      private int FindTreeNode(string key)
      {
         if (string.IsNullOrEmpty(key))
            return -1;
         for (int index = 0; index < SelectedNodes.Count; index++)
         {
            if (SelectedNodes[index].Name == key)
               return index;
         }
         return -1;
      }

      private void CreateTree(JobConfig jobConfig)
      {
         if (jobConfig != null)
         {
            TreeNode root;
            TreeNode child;

            foreach (SystemsJobDefine system in jobConfig.JobsOfSystem)
            {
               // 创建根节点
               root = Nodes.Add(system.Name);
               SetNodeIcon(root);

               foreach (Job job in system.Jobs)
               {
                  child = root.Nodes.Add(job.Name);
                  child.Tag = job;
                  SetNodeIcon(child);
               }
            }
         }
      }
      #endregion
   }

}
