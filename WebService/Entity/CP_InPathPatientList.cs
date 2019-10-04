using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 进入路径的病人类
    /// </summary>
    [DataContract()]
    public partial class CP_InPathPatientList : INotifyPropertyChanged
    {
        /// <summary>
        ///   首页序号
        /// </summary>  
        [DataMember()]
        public Object Syxh
        { get; set; }
        /// <summary>
        /// HIS号
        /// </summary>
        [DataMember()]
        public Object Hissyxh
        { get; set; }
        /// <summary>
        /// --序号
        /// </summary>
        [DataMember()]
        public Object Id
        { get; set; }
        /// <summary>
        /// --临床路径代码(CP_ClinicalPath.Ljdm)
        /// </summary>
        [DataMember()]
        public Object Ljdm
        { get; set; }
        /// <summary>
        /// --床位医师
        [DataMember()]
        public Object Cwys
        { get; set; }
        /// <summary>
        /// 	--进入路径时间 
        /// </summary>
        [DataMember()]
        public Object Jrsj
        { get; set; }
        /// <summary>
        /// --退出时间
        /// </summary>
        [DataMember()]
        public Object Tcsj
        { get; set; }
        /// <summary>
        // --完成时间
        [DataMember()]
        public Object Wcsj
        { get; set; }
        /// <summary>
        /// --当前步骤天数
        /// </summary>
        [DataMember()]
        public Object Ljts
        { get; set; }
        /// <summary>
        /// --路径状态(1进入,2.退出,3完成)
        /// </summary>
        [DataMember()]
        public Object Ljzt
        { get; set; }


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}