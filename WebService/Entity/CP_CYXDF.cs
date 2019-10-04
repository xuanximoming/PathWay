using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{

    [DataContract()]
    public class CP_CYXDF
    {
        public CP_CYXDF()
        { }
        #region Model
        private int _id;
        private string _cfmc;
        private string _py;
        private string _wb;
        private string _czyh;
        private string _cjrq;
        private string _yplh;
        private string _ksdm;
        private int _mbbz;
        private int _cfts;
        private int _jlzt;
        private string _yfdm;
        private string _lrdm;
        private int _tsbz;
        private string _ylmfl;
        private string _zxyjr;
        private int _sqdmbxh;
        private int _jdcfbz;
        private int _isjj;
        private string _zxksdm;
        private int _yzkx;
        private string _extension;
        private string _extension1;
        private string _extension2;
        private string _extension3;
        private string _extension4;
        /// <summary>
        /// 草药处方序号
        /// </summary>
        [DataMember()]
        public int ID
        {
            set { _id = value; }
            get { return _id; }
        }
        /// <summary>
        /// 草药处方名称
        /// </summary>
        [DataMember()]
        public string cfmc
        {
            set { _cfmc = value; }
            get { return _cfmc; }
        }
        /// <summary>
        /// 草药处方 拼音
        /// </summary>
        [DataMember()]
        public string Py
        {
            set { _py = value; }
            get { return _py; }
        }
        /// <summary>
        /// 草药处方 五笔
        /// </summary>
        [DataMember()]
        public string Wb
        {
            set { _wb = value; }
            get { return _wb; }
        }
        /// <summary>
        /// 创建人
        /// </summary>
        [DataMember()]
        public string czyh
        {
            set { _czyh = value; }
            get { return _czyh; }
        }
        /// <summary>
        /// 创建日期
        /// </summary>
        [DataMember()]
        public string cjrq
        {
            set { _cjrq = value; }
            get { return _cjrq; }
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
        /// 科室代码
        /// </summary>
        [DataMember()]
        public string Ksdm
        {
            set { _ksdm = value; }
            get { return _ksdm; }
        }
        /// <summary>
        /// 模板标志
        /// </summary>
        [DataMember()]
        public int mbbz
        {
            set { _mbbz = value; }
            get { return _mbbz; }
        }
        /// <summary>
        /// 处方付数
        /// </summary>
        [DataMember()]
        public int cfts
        {
            set { _cfts = value; }
            get { return _cfts; }
        }
        /// <summary>
        /// 记录状态，默认0
        /// </summary>
        [DataMember()]
        public int jlzt
        {
            set { _jlzt = value; }
            get { return _jlzt; }
        }
        /// <summary>
        /// 药房代码
        /// </summary>
        [DataMember()]
        public string Yfdm
        {
            set { _yfdm = value; }
            get { return _yfdm; }
        }
        /// <summary>
        ///  录入代码 默认为空
        /// </summary>
        [DataMember()]
        public string lrdm
        {
            set { _lrdm = value; }
            get { return _lrdm; }
        }
        /// <summary>
        /// 特殊标志 默认为0
        /// </summary>
        [DataMember()]
        public int tsbz
        {
            set { _tsbz = value; }
            get { return _tsbz; }
        }
        /// <summary>
        /// 药理代码    默认为空
        /// </summary>
        [DataMember()]
        public string ylmfl
        {
            set { _ylmfl = value; }
            get { return _ylmfl; }
        }
        /// <summary>
        /// 执行药剂 默认为"0"
        /// </summary>
        [DataMember()]
        public string zxyjr
        {
            set { _zxyjr = value; }
            get { return _zxyjr; }
        }
        /// <summary>
        /// 默认序号0
        /// </summary>
        [DataMember()]
        public int sqdmbxh
        {
            set { _sqdmbxh = value; }
            get { return _sqdmbxh; }
        }
        /// <summary>
        /// 经典处方标志 默认为1
        /// </summary>
        [DataMember()]
        public int jdcfbz
        {
            set { _jdcfbz = value; }
            get { return _jdcfbz; }
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
        /// 医嘱变异可选， 1为可选，可选不算变异，0为必选，默认0
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