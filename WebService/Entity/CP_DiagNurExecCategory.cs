using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{

    /// <summary>
    /// CP_DiagNurExecCategory:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
   [DataContract()]
    public  class CP_DiagNurExecCategory
    {
        public CP_DiagNurExecCategory()
        { }
        #region Model
        private int _lbxh;
        private string _lbname;
        private int _yxjl;
        private string _create_time ;
        private string _create_user;
        private string _cancel_time;
        private string _cancel_user;
        private string _yxjlmc;//数据库没有该字段
        private string _extension;//数据库没有该字段
        private int _ordervalue;
        /// <summary>
        /// 项目类别序号
        /// </summary>
        [DataMember()]
        public int Lbxh
        {
            set { _lbxh = value; }
            get { return _lbxh; }
        }
        /// <summary>
        /// 项目类别名称
        /// </summary>
       [DataMember()]
       public string LbName
        {
            set { _lbname = value; }
            get { return _lbname; }
        }
        /// <summary>
        /// 是否有效
        /// </summary>
       [DataMember()]
       public int Yxjl
        {
            set { _yxjl = value; }
            get { return _yxjl; }
        }
        /// <summary>
        /// 创建时间
        /// </summary>
       [DataMember()] 
       public string Create_Time
        {
            set { _create_time = value; }
            get { return _create_time; }
        }
        /// <summary>
        /// 创建人
        /// </summary>
       [DataMember()] 
       public string Create_User
        {
            set { _create_user = value; }
            get { return _create_user; }
        }
        /// <summary>
        /// 停用或取消时间
        /// </summary>
       [DataMember()] 
       public string Cancel_Time
        {
            set { _cancel_time = value; }
            get { return _cancel_time; }
        }
        /// <summary>
        /// 停用人或取消人
        /// </summary>
       [DataMember()] 
       public string Cancel_User
        {
            set { _cancel_user = value; }
            get { return _cancel_user; }
        }
       /// <summary>
       /// 备用字段
       /// </summary>
       [DataMember()]
       public string Yxjlmc
       {
           set { _yxjlmc = value; }
           get { return _yxjlmc; }
       }
       /// <summary>
       /// 备用字段
       /// </summary>
       [DataMember()]
       public string Extension
       {
           set { _extension = value; }
           get { return _extension; }
       }
        /// <summary>
        /// 排序
        /// </summary>
       [DataMember()] 
       public int OrderValue
        {
            set { _ordervalue = value; }
            get { return _ordervalue; }
        }
        #endregion Model

    }
}

