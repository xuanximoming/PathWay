using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Data;
using YidanSoft.Tool;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 病人类?
    /// 此处代码需要重构
    /// 对现有的病人类
    /// </summary>
    [DataContract()]
    public partial class CP_InpatinetListPaging : INotifyPropertyChanged
    {
        private List<CP_InpatinetList> cP_InpatinetList;
        /// <summary>
        ///   首页序号
        /// </summary>  
        [DataMember()]
        public List<CP_InpatinetList> CP_InpatinetList
        {
            get { return cP_InpatinetList; }
            set { cP_InpatinetList = value; }
        }
        private int allCount;
        [DataMember()]
        public int AllCount
        {
            get { return allCount; }
            set { allCount = value; }
        }

        private String _New = "0";

        private String _None = "0";

        private String _InPath = "0";

        private String _QuitPath = "0";

        private String _DonePath = "0";

        private String _NotIn = "0";
        [DataMember()]
        public String New { get { return _New; } set { _New = value; } }
        [DataMember()]
        public String None { get { return _None; } set { _None = value; } }
        [DataMember()]
        public String InPath { get { return _InPath; } set { _InPath = value; } }
        [DataMember()]
        public String QuitPath { get { return _QuitPath; } set { _QuitPath = value; } }
        [DataMember()]
        public String DonePath { get { return _DonePath; } set { _DonePath = value; } }
        [DataMember()]
        public String NotIn { get { return _NotIn; } set { _NotIn = value; } }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}