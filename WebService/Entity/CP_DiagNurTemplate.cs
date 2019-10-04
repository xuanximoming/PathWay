using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// CP_DiagNurTemplate:实体类(诊疗护理模板表和执行表 共同实体类) 
    /// </summary>
     [DataContract()]
    public class CP_DiagNurTemplate
    {
        public CP_DiagNurTemplate()
        { }
        #region Model
        private int _MainID;
        private decimal _syxh;
        private int _id;
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
        private string _zxksdm;
        private string _iskx;
        private string _isjj;
        private string _yzzt;

        //备注：诊疗护理模板表:CP_DiagNurTemplate；诊疗护理执行记录表:CP_DiagNurExecRecord

        /// <summary>
        /// 诊疗护理记录表CP_DiagNurExecRecord.MainID 执行主编号
        /// </summary>
        [DataMember()]
        public int MainID
        {
            set { _MainID = value; }
            get { return _MainID; }
        }
        /// <summary>
        /// 诊疗护理记录表CP_DiagNurExecRecord.Syxh 首页序号
        /// </summary>
        [DataMember()]
        public decimal Syxh
        {
            set { _syxh = value; }
            get { return _syxh; }
        }
        /// <summary>
        /// 诊疗护理模板表主编号；在诊疗护理执行表对应CP_DiagNurExecRecord.Tid字段（和模块表对比数据判断是否产生变异）
        /// </summary>
        [DataMember()]
         public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 共同属性 路径代码
        /// </summary>
        [DataMember()]
        public string Ljdm
        {
            set { _ljdm = value; }
            get { return _ljdm; }
        }
        /// <summary>
        /// 共同属性 路径节点明细编号
        /// </summary>
        [DataMember()]
         public string PathDetailId
        {
            set { _pathdetailid = value; }
            get { return _pathdetailid; }
        }
        /// <summary>
        /// 共同属性 诊疗护理工作项目明细编号
        /// </summary>
        [DataMember()]
         public int Mxxh
        {
            set { _mxxh = value; }
            get { return _mxxh; }
        }
        /// <summary>
        /// 共同属性 诊疗护理工作 项目名称
        /// </summary>
        [DataMember()]
         public string MxName
        {
            set { _mxname = value; }
            get { return _mxname; }
        }
        /// <summary>
        ///共同属性 诊疗护理工作类别编号 1是诊疗工作；2是护理工作；3是非药物治疗
        /// </summary>
        [DataMember()]
         public int Lbxh
        {
            set { _lbxh = value; }
            get { return _lbxh; }
        }
        /// <summary>
        ///共同属性 有效路径 1是有效；0是无效
        /// </summary>
        [DataMember()]
         public int Yxjl
        {
            set { _yxjl = value; }
            get { return _yxjl; }
        }
        /// <summary>
        ///共同属性 拼音
        /// </summary>
        [DataMember()]
         public string Py
        {
            set { _py = value; }
            get { return _py; }
        }
        /// <summary>
        ///共同属性 五笔
        /// </summary>
        [DataMember()]
         public string Wb
        {
            set { _wb = value; }
            get { return _wb; }
        }
        /// <summary>
        ///共同属性 创建时间
        /// </summary>
        [DataMember()]
         public string Create_Time
        {
            set { _create_time = value; }
            get { return _create_time; }
        }
        /// <summary>
        ///共同属性 创建人
        /// </summary>
        [DataMember()]
         public string Create_User
        {
            set { _create_user = value; }
            get { return _create_user; }
        }
        /// <summary>
        ///共同属性 取消时间
        /// </summary>
        [DataMember()]
         public string Cancel_Time
        {
            set { _cancel_time = value; }
            get { return _cancel_time; }
        }
        /// <summary>
        ///共同属性 取消人
        /// </summary>
        [DataMember()]
         public string Cancel_User
        {
            set { _cancel_user = value; }
            get { return _cancel_user; }
        }
        /// <summary>
        ///共同属性 备用字段 
        /// </summary>
        [DataMember()]
         public string Extension
        {
            set { _extension = value; }
            get { return _extension; }
        }
        /// <summary>
        ///共同属性 备用字段
        /// </summary>
        [DataMember()]
         public string Extension1
        {
            set { _extension1 = value; }
            get { return _extension1; }
        }
        /// <summary>
        ///共同属性 备用字段 
        /// </summary>
        [DataMember()]
         public string Extension2
        {
            set { _extension2 = value; }
            get { return _extension2; }
        }
        /// <summary>
        ///共同属性 备用字段（备注）
        /// </summary>
        [DataMember()]
         public string Extension3
        {
            set { _extension3 = value; }
            get { return _extension3; }
        }

        /// <summary>
        ///共同属性 计价类型
        /// </summary>
        [DataMember()]
        public string Isjj
        {
            set { _isjj = value; }
            get { return _isjj; }
        }
        /// <summary>
        ///共同属性 是否可选 1为可选；0 为必选
        /// </summary>
        [DataMember()]
        public string Iskx
        {
            set { _iskx = value; }
            get { return _iskx; }
        }
        /// <summary>
        ///共同属性 执行科室代码
        /// </summary>
        [DataMember()]
        public string Zxksdm
        {
            set { _zxksdm = value; }
            get { return _zxksdm; }
        }
        /// <summary>
        ///共同属性 记录状态
        /// </summary>
        [DataMember()]
        public string Yzzt
        {
            set { _yzzt = value; }
            get { return _yzzt; }
        }
        #endregion Model

    }
}
