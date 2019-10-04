using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;
using System.Data;
using System.Data.SqlClient;
using YidanSoft.Tool;
namespace Yidansoft.Service.Entity
{
    [DataContract()]
    /// <summary>
    /// 病人列表
    /// </summary>
    public class CP_InpatinetLists : List<CP_InpatinetList>
    {
        //public  CP_InpatinetLists InitializePaths( )
        //{
        //    return this;
        //}
        public void InitializePatientsExamItems()
        {
            foreach (CP_InpatinetList patinet in this)
            {
                patinet.InitializePatinetExamItems();
            }
        }
    }
}
