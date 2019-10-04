using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract()]
    public class CP_CYXDFMX
    {
        public CP_CYXDFMX()
        { }
        #region Model
        private int _id;
        private string _cfxh;
        private int _idm;
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
        private int _zxts;
        private decimal _ypsl;
        private int _cfts;
        private int _ypbz;
        private string _lcxmdm;
        private int _pxxh;
        private string _memo;
        private int _ekxs;
        private string _ekdw;
        private int _ekbz;
        private int _isjj;
        private string _zxksdm;
        private int _yzkx;
        private string _extension;
        private string _extension1;
        private string _extension2;
        private string _extension3;
        private string _extension4;
        /// <summary>
        /// 草药处方明细编号
        /// </summary>
        [DataMember()]
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 草药处方序号
        /// </summary>
        [DataMember()]
        public string cfxh
        {
            set { _cfxh = value; }
            get { return _cfxh; }
        }
       
        /// <summary>
        /// 草药协助方主表序号
        /// </summary>
        [DataMember()]
        public int idm
        {
            set { _idm = value; }
            get { return _idm; }
        }
        /// <summary>
        /// 处方类型
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
        /// 草药处方明显 拼音
        /// </summary>
        [DataMember()]
        public string Py
        {
            set { _py = value; }
            get { return _py; }
        }
        /// <summary>
        /// 草药处方明显 五笔
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
        /// 频次执行周期 
        /// </summary>
        [DataMember()]
        public int Zxts
        {
            set { _zxts = value; }
            get { return _zxts; }
        }
        /// <summary>
        /// 药品数量(使用剂量单位)
        /// </summary>
        [DataMember()]
        public decimal Ypsl
        {
            set { _ypsl = value; }
            get { return _ypsl; }
        }
        /// <summary>
        /// 频次执行次（天）数
        /// </summary>
        [DataMember()]
        public int cfts
        {
            set { _cfts = value; }
            get { return _cfts; }
        }
        /// <summary>
        /// 频次执行周期 单位
        /// </summary>
        [DataMember()]
        public int ypbz
        {
            set { _ypbz = value; }
            get { return _ypbz; }
        }
        /// <summary>
        /// 默认为'0'
        /// </summary>
        [DataMember()]
        public string lcxmdm
        {
            set { _lcxmdm = value; }
            get { return _lcxmdm; }
        }
        /// <summary>
        /// 默认为0
        /// </summary>
        [DataMember()]
        public int pxxh
        {
            set { _pxxh = value; }
            get { return _pxxh; }
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
        /// 最小单位  
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
        /// 备用字段 产地序号 cdxh
        /// </summary>
        [DataMember()]
        public string Extension
        {
            set { _extension = value; }
            get { return _extension; }
        }
        /// <summary>
        /// 备用字段 规格序号 ggxh
        /// </summary>
        [DataMember()]
        public string Extension1
        {
            set { _extension1 = value; }
            get { return _extension1; }
        }
        /// <summary>
        /// Zxdw
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