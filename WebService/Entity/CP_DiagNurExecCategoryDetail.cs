using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// CP_DiagNurExecCategoryDetail:实体类(属性说明自动提取数据库字段的描述信息)
    /// </summary>
    [DataContract()]
    public partial class CP_DiagNurExecCategoryDetail
    {
        public CP_DiagNurExecCategoryDetail()
        { }
        #region Model
        private int _mxxh;
        private string _name;
        private int _lbxh;
        private int _yxjl;
        private int _sfsy;
        private string _create_time ;
        private string _create_user;
        private string _cancel_time;
        private string _cancel_user;
        private int _ordervalue;
        private int _jktype;
        private string _tbzd;
        private string _zdly;
        private string _py;
        private string _wb;
        private string _jkdm;
        private string _scdm;
        private string _memo;
        private string _extension;
        private string _extension1;
        private string _extension2;
        /// <summary>
        /// 明细序号
        /// </summary>
        [DataMember()]
        public int Mxxh
        {
            set { _mxxh = value; }
            get { return _mxxh; }
        }
 
        /// <summary>
        /// 明细项目名称
        /// </summary>
        [DataMember()]
        public string Name
        {
            set { _name = value; }
            get { return _name ; }
        }

        /// <summary>
        /// 类别序号
        /// </summary>
       [DataMember()]
        public int Lbxh
        {
            set { _lbxh = value; }
            get { return _lbxh; }
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
        /// 是否使用 0正常使用 1表示停用 2表示使用中
        /// </summary>
        [DataMember()]
        public int Sfsy
        {
            set { _sfsy = value; }
            get { return _sfsy; }
        }
        /// <summary>
        /// 创建日期时间
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
        /// 停用时间
        /// </summary>
        [DataMember()]
        public string Cancel_Time
        {
            set { _cancel_time = value; }
            get { return _cancel_time; }
        }
        /// <summary>
        /// 停用人
        /// </summary>
        [DataMember()]
        public string Cancel_User
        {
            set { _cancel_user = value; }
            get { return _cancel_user; }
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
        /// <summary>
        /// 接口类型 同步数据源的类型 0表示默认手动录入，1表示从His同步，2表示从电子病历同步，3表示从lis同步...
        /// </summary>
        [DataMember()]
        public int JkType
        {
            set { _jktype = value; }
            get { return _jktype; }
        }
        /// <summary>
        /// 同步字段
        /// </summary>
        [DataMember()]
        public string Tbzd
        {
            set { _tbzd = value; }
            get { return _tbzd; }
        }
        /// <summary>
        /// 同步字段来源
        /// </summary>
        [DataMember()]
        public string Zdly
        {
            set { _zdly = value; }
            get { return _zdly; }
        }
        /// <summary>
        /// 拼音
        /// </summary>
        [DataMember()]
        public string Py
        {
            set { _py = value; }
            get { return _py; }
        }
        /// <summary>
        /// 五笔
        /// </summary>
        [DataMember()]
        public string Wb
        {
            set { _wb = value; }
            get { return _wb; }
        }
        /// <summary>
        /// 监控代码
        /// </summary>
        [DataMember()]
        public string Jkdm
        {
            set { _jkdm = value; }
            get { return _jkdm; }
        }
        /// <summary>
        /// 输出代码
        /// </summary>
        [DataMember()]
        public string Scdm
        {
            set { _scdm = value; }
            get { return _scdm; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember()]
        public string Memo
        {
            set { _memo = value; }
            get { return _memo; }
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
        /// 备用字段
        /// </summary>
        [DataMember()]
        public string Extension1
        {
            set { _extension1 = value; }
            get { return _extension1; }
        }
        /// <summary>
        /// 备用字段
        /// </summary>
        [DataMember()]
        public string Extension2
        {
            set { _extension2 = value; }
            get { return _extension2; }
        }
 
        #endregion Model

    }
}

