using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract()]
    public class CP_InPathPatientCondition : INotifyPropertyChanged
    {
        private int _id;

        /// <summary>
        /// 纳入条件ID
        /// </summary>
        [DataMember()]
        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        private int _syxh;
        /// <summary>
        /// 病人首页序号
        /// </summary>
        [DataMember()]
        public int Syxh
        {
            get { return _syxh; }
            set { _syxh = value; }
        }
       
        private string _tjdm;
        /// <summary>
        /// 条件代码
        /// </summary>
        [DataMember()]
        public string Tjdm
        {
            get { return _tjdm; }
            set { _tjdm = value; }
        }

        private string _memo;
        /// <summary>
        /// 条件代码
        /// </summary>
        [DataMember()]
        public string Memo
        {
            get { return _memo; }
            set { _memo = value; }
        }
        /// <summary>
        /// 构造
        /// </summary>
        /// <param name="syxh"></param>
        /// <param name="tjdm"></param>
        /// <param name="memo"></param>
        public CP_InPathPatientCondition(int syxh,string tjdm,string memo)
        {
            Syxh = syxh;
            Tjdm = tjdm;
            Memo = memo;
           
         }
        /// <summary>
        /// 默认构造
        /// </summary>
        public CP_InPathPatientCondition()
        { 
          
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}