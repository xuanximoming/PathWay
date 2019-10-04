using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using DrectSoft.Tool;
using System.Data;
using System.Data.SqlClient;
namespace Yidansoft.Service.Entity
{
   
    /// <summary>
    /// 路径列表类
    /// </summary>
    public class CP_ClinicalPathLists : List<CP_ClinicalPathList>
    {
        /// <summary>
        /// 初始化路径进入条件
        /// </summary>
        public void InitializePathsEnterConditions()
        {
            
            foreach (CP_ClinicalPathList path in this)
            {
                path.InitializePathEnterConditions();
            }
        }
    }
}
