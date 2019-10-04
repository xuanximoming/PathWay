using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract()]
    public class CP_CYFUnion
    {
        /// <summary>
        /// 用于草药协定方和明显方联合查询
        /// </summary>
        public CP_CYFUnion()
        { }
        #region Model
        private int _id;
         
        private string _yplh;
        private string _ypdm;
        private string _ypmc;
        private string _py;
        private string _wb;
        private string _yfdm;
        private string _pcdm;
        private decimal _ypjl;
        private string _jldw;
        private int _dwlb;
        private string _ypgg;
        private decimal _ypsl;
  
        private string _memo;
        private int _ekxs;
        private string _ekdw;
        private int _ekbz;
        private int _isjj;
        private string _zxksdm;
        private int _yzkx;
        private string _zxdw;
        private string _ggdw;
        private string _jxdm;
        private string _cdxh;
        private string _ggxh;
        private string _extension;
        private string _extension1;
        private string _extension2;
        private string _extension3;
        private string _extension4;
        /// <summary>
        /// 编号
        /// </summary>
        [DataMember()]
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        
        /// <summary>
        /// 处方类型(草药)
        /// </summary>
        [DataMember()]
        public string yplh
        {
            set { _yplh = value; }
            get { return _yplh; }
        }
        /// <summary>
        /// 药品代码
        /// </summary>
        [DataMember()]
        public string Ypdm
        {
            set { _ypdm = value; }
            get { return _ypdm; }
        }
        /// <summary>
        /// 药品(项目)名称
        /// </summary>
        [DataMember()]
        public string Ypmc
        {
            set { _ypmc = value; }
            get { return _ypmc; }
        }
        /// <summary>
        /// 草药项目 拼音
        /// </summary>
        [DataMember()]
        public string Py
        {
            set { _py = value; }
            get { return _py; }
        }
        /// <summary>
        /// 草药项目 五笔
        /// </summary>
        [DataMember()]
        public string Wb
        {
            set { _wb = value; }
            get { return _wb; }
        }
        /// <summary>
        /// (药品)用法代码
        /// </summary>
        [DataMember()]
        public string Yfdm
        {
            set { _yfdm = value; }
            get { return _yfdm; }
        }
        /// <summary>
        /// 频次代码
        /// </summary>
        [DataMember()]
        public string Pcdm
        {
            set { _pcdm = value; }
            get { return _pcdm; }
        }
        /// <summary>
        /// 药品剂量
        /// </summary>
        [DataMember()]
        public decimal Ypjl
        {
            set { _ypjl = value; }
            get { return _ypjl; }
        }
        /// <summary>
        /// 剂量单位
        /// </summary>
        [DataMember()]
        public string Jldw
        {
            set { _jldw = value; }
            get { return _jldw; }
        }
        /// <summary>
        /// 单位类别(当前剂量使用的是何种单位,CP_DataCategory.Mxbh, Lbbh = 30)
        /// </summary>
        [DataMember()]
        public int Dwlb
        {
            set { _dwlb = value; }
            get { return _dwlb; }
        }
        /// <summary>
        /// 规格 默认 kg / 处方名称
        /// </summary>
        [DataMember()]
        public string Ypgg
        {
            set { _ypgg = value; }
            get { return _ypgg; }
        }
        /// <summary>
        /// 执行单位
        /// </summary>
        [DataMember()]
        public string Zxdw
        {
            set { _zxdw = value; }
            get { return _zxdw; }
        }
        /// <summary>
        /// 规格代码
        /// </summary>
        [DataMember()]
        public string Ggdw
        {
            set { _ggdw = value; }
            get { return _ggdw; }
        }
        /// <summary>
        /// 剂型代码
        /// </summary>
        [DataMember()]
        public string Jxdm
        {
            set { _jxdm = value; }
            get { return _jxdm; }
        }
         /// <summary>
        /// 产地序号
        /// </summary>
        [DataMember()]
        public string Cdxh
        {
            set { _cdxh = value; }
            get { return _cdxh; }
        }
        /// <summary>
        /// 规格序号
        /// </summary>
        [DataMember()]
        public string Ggxh
        {
            set { _ggxh = value; }
            get { return _ggxh; }
        }
        /// <summary>
        /// 药品数量 
        /// </summary>
        [DataMember()]
        public decimal Ypsl
        {
            set { _ypsl = value; }
            get { return _ypsl; }
        }
       
      
        /// <summary>
        /// 备注 嘱托
        /// </summary>
        [DataMember()]
        public string Memo
        {
            set { _memo = value; }
            get { return _memo; }
        }
        /// <summary>
        /// 最小系数默认为1
        /// </summary>
        [DataMember()]
        public int ekxs
        {
            set { _ekxs = value; }
            get { return _ekxs; }
        }
        /// <summary>
        /// 最小单位 默认为g
        /// </summary>
        [DataMember()]
        public string ekdw
        {
            set { _ekdw = value; }
            get { return _ekdw; }
        }
        /// <summary>
        /// 最小标志默认为0
        /// </summary>
        [DataMember()]
        public int ekbz
        {
            set { _ekbz = value; }
            get { return _ekbz; }
        }
        /// <summary>
        /// 是否计价
        /// </summary>
        [DataMember()]
        public int Isjj
        {
            set { _isjj = value; }
            get { return _isjj; }
        }
        /// <summary>
        /// 执行科室代码
        /// </summary>
        [DataMember()]
        public string Zxksdm
        {
            set { _zxksdm = value; }
            get { return _zxksdm; }
        }
        /// <summary>
        /// 医嘱变异可选， 1为可选，可选不算变异，0为必选，默认为0 
        /// </summary>
        [DataMember()]
        public int Yzkx
        {
            set { _yzkx = value; }
            get { return _yzkx; }
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
        /// <summary>
        /// 备用字段
        /// </summary>
        [DataMember()]
        public string Extension4
        {
            set { _extension4 = value; }
            get { return _extension4; }
        }
        #endregion Model
         

    }
}