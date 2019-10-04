using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// CP_DiagNurExecRecord:实体类(属性说明自动提取数据库字段的描述信息) 诊疗护理执行记录表
    /// </summary>
    [DataContract()]
    public  class CP_DiagNurExecRecord
    {
        public CP_DiagNurExecRecord()
        { }
        #region Model
        private int _MainID;
        private decimal _syxh;
        private int _Tid;
        private string _ljdm;
        private string _pathdetailid;
        private int _mxxh;
        private string _mxname;
        private int _lbxh;
        private int _yxjl;
        private string _py;
        private string _wb;
        private string _create_time;
        private string _create_user;
        private string _cancel_time;
        private string _cancel_user;
        private string _extension;
        private string _extension1;
        private string _extension2;
        private string _extension3;

       
        /// <summary>
        /// 执行主编号
        /// </summary>
        [DataMember()]
        public int MainID
        {
            set { _MainID = value; }
            get { return _MainID; }
        }
        /// <summary>
        /// 首页序号
        /// </summary>
         [DataMember()]
        public decimal Syxh
        {
            set { _syxh = value; }
            get { return _syxh; }
        }
        /// <summary>
        /// 路径序号
        /// </summary>
         [DataMember()]
        public int Tid
        {
            set { _Tid = value; }
            get { return _Tid; }
        }
        /// <summary>
        /// 路径代码
        /// </summary>
         [DataMember()]
         public string Ljdm
        {
            set { _ljdm = value; }
            get { return _ljdm; }
        }
        /// <summary>
        /// 路径节点
        /// </summary>
         [DataMember()]
        public string PathDetailId
        {
            set { _pathdetailid = value; }
            get { return _pathdetailid; }
        }
        /// <summary>
        /// 诊疗护理明细序号
        /// </summary>
         [DataMember()]
        public int Mxxh
        {
            set { _mxxh = value; }
            get { return _mxxh; }
        }
        /// <summary>
        /// 诊疗护理明细项目名称
        /// </summary>
         [DataMember()]
        public string MxName
        {
            set { _mxname = value; }
            get { return _mxname; }
        }
        /// <summary>
        /// 类型序号
        /// </summary>
         [DataMember()]
        public int Lbxh
        {
            set { _lbxh = value; }
            get { return _lbxh; }
        }
        /// <summary>
        /// 有效记录
        /// </summary>
         [DataMember()]
        public int Yxjl
        {
            set { _yxjl = value; }
            get { return _yxjl; }
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
        /// 取消时间
        /// </summary>
         [DataMember()]
        public string Cancel_Time
        {
            set { _cancel_time = value; }
            get { return _cancel_time; }
        }
        /// <summary>
        /// 取消人
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
        /// <summary>
         /// 备用字段
        /// </summary>
         [DataMember()]
        public string Extension3
        {
            set { _extension3 = value; }
            get { return _extension3; }
        }
        #endregion Model

    }
}