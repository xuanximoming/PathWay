using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
     [DataContract()]
    public partial  class CP_LongOrderList : INotifyPropertyChanged
    {
        private decimal _Cqyzxh;
        [DataMember()]
        public decimal Cqyzxh
        {
            get { return _Cqyzxh; }
            set { _Cqyzxh = value; }
        }
        private decimal _Syxh;
        [DataMember()]
        public decimal Syxh
        {
            get { return _Syxh; }
            set { _Syxh = value; }
          
        }
        private int _Fzbz;
        [DataMember()]
        public int Fzbz
        {
            get { return _Fzbz; }
            set { _Fzbz = value; }
        }
        private string _Lrysdm;
        [DataMember()]
        public string Lrysdm
        {
            get { return _Lrysdm; }
            set { _Lrysdm = value; }
        }
        private string _Lrrq;
        [DataMember()]
        public string Lrrq
        {
            get { return _Lrrq; }
            set { _Lrrq = value; }
        }
        private string _Shczy;
        [DataMember()]
        public string Shczy
        {
            get { return _Shczy; }
            set { _Shczy = value; }
        }
        private string _Shrq;
        [DataMember()]
        public string Shrq
        {
            get { return _Shrq; }
            set { _Shrq = value; }
        }
        private string _Zxczy;
        [DataMember()]
        public string Zxczy
        {
            get { return _Zxczy; }
            set { _Zxczy = value; }
          
        }
        private string _Zxrq;
        [DataMember()]
        public string Zxrq
        {
            get { return _Zxrq; }
            set { _Zxrq = value; }
          
        }
         /// <summary>
         /// 执行时间
         /// </summary>
        private string _Zxsj_E;
        [DataMember()]
        public string Zxsj_E
        {
            get { return _Zxsj_E; }
            set { _Zxsj_E = value; }

        }
        private string _Qxysdm;
        [DataMember()]
        public string Qxysdm
        {
           get { return _Qxysdm; }
            set { _Qxysdm = value; }
        }
        private string _Qxrq;
        [DataMember()]
        public string Qxrq
        {
          get { return _Qxrq; }
            set { _Qxrq = value; }
        }
        private string _Tzysdm;
        [DataMember()]
        public string Tzysdm
        {
          get { return _Tzysdm; }
            set { _Tzysdm = value; }
        }
        private string _Tzrq;
        [DataMember()]
        public string Tzrq
        {
           get { return _Tzrq; }
            set { _Tzrq = value; }
        }
        private string _Tzsj;
        [DataMember()]
        public string Tzsj
        {
            get { return _Tzsj; }
            set { _Tzsj = value; }
        }
        private string _Tzshhs;
        [DataMember()]
        public string Tzshhs
        {
          
           get { return _Tzshhs; }
            set { _Tzshhs = value; }
        }
        private string _Tzshrq;
        [DataMember()]
        public string Tzshrq
        {
          get { return _Tzshrq; }
            set { _Tzshrq = value; }
        }

        private string _Ksrq;
        [DataMember()]
        public string Ksrq
        {
            get { return _Ksrq; }
            set { _Ksrq = value; }
          
        }
        private string _Kssj;
        [DataMember()]
        public string Kssj
        {
            get { return _Kssj; }
            set { _Kssj = value; }

        }
        private string _Ypmc;
        [DataMember()]
        public string Ypmc
        {
          get { return _Ypmc; }
            set { _Ypmc = value; }
        }
        private string _Ypgg;
        [DataMember()]
        public string Ypgg
        {
           get { return _Ypgg; }
            set { _Ypgg = value; }
        }
        private int _Xmlb;
        [DataMember()]
        public int Xmlb
        {
            get { return _Xmlb; }
            set { _Xmlb = value; }
        }
        private string _Zxdw;
        [DataMember()]
        public string Zxdw
        {
           get { return _Zxdw; }
            set { _Zxdw = value; }
        }
        private decimal _Ypjl;
        [DataMember()]
        public decimal Ypjl
        {
            get { return _Ypjl; }
            set { _Ypjl = value; }
        }
        private string _Jldw;
        [DataMember()]
        public string Jldw
        {
           get { return _Jldw; }
            set { _Jldw = value; }
        }
        private decimal _Dwxs;
        [DataMember()]
        public decimal Dwxs
        {
           get { return _Dwxs; }
            set { _Dwxs = value; }
         }
        private int _Dwlb;
        [DataMember()]
        public int Dwlb
        {
           get { return _Dwlb; }
            set { _Dwlb = value; }
        }
        private string _Yfdm;
        [DataMember()]
        public string Yfdm
        {
           get { return _Yfdm; }
            set { _Yfdm = value; }
        }
        private string _Pcdm;
        [DataMember()]
        public string Pcdm
        {
           get { return _Pcdm; }
            set { _Pcdm = value; }
        }
        private int _Zxcs;
        [DataMember()]
        public int Zxcs
        {
          get { return _Zxcs; }
            set { _Zxcs = value; }
        }
        private int _Zxzq;
        [DataMember()]
        public int Zxzq
        {
          get { return _Zxzq; }
            set { _Zxzq = value; }
        }
        private int _Zxzqdw;
        [DataMember()]
        public int Zxzqdw
        {
          get { return _Zxzqdw; }
            set { _Zxzqdw = value; }
        }
        private string _Zdm;
        [DataMember()]
        public string Zdm
        {
         get { return _Zdm; }
            set { _Zdm = value; }
        }
        private string _Zxsj;
        [DataMember()]
        public string Zxsj
        {
          get { return _Zxsj; }
            set { _Zxsj = value; }
        }
        private string _Ztnr;
        [DataMember()]
        public string Ztnr
        {
            get { return _Ztnr; }
            set { _Ztnr = value; }
          
        }
        private string _Yznr;
        [DataMember()]
        public string Yznr
        {
           get { return _Yznr; }
            set { _Yznr = value; }
          
        }
        private string _ShczyName;
        [DataMember()]
        public string ShczyName
        {
               get { return _ShczyName; }
            set { _ShczyName = value; }
        }
      
        private string _QxysName;
        [DataMember()]
        public string QxysName
        {
            get { return _QxysName; }
            set { _QxysName = value; }
        }
        private string _TzysName;
        [DataMember()]
        public string TzysName
        { 
            get { return _TzysName; }
            set { _TzysName = value; }
          
        }
        private string _TzshhsName;
        [DataMember()]
        public string TzshhsName
        {
          get { return _TzshhsName; }
            set { _TzshhsName = value; }
        }
        private string _DwlbName;
        [DataMember()]
        public string DwlbName
        {
          get { return _DwlbName; }
            set { _DwlbName = value; }
        }
        private string _YfName;
        [DataMember()]
        public string YfName
        {
           get { return _YfName; }
            set { _YfName = value; }
        }
        private string _PcName;
        [DataMember()]
        public string PcName
        {
            get { return _PcName; }
            set { _PcName = value; }
          
        }
        private string _ZxzqdwName;
        [DataMember()]
        public string ZxzqdwName
        {
            get { return _ZxzqdwName; }
            set { _ZxzqdwName = value; }
        }
        private string _LrysName;
        [DataMember()]
        public string LrysName
        {
            get { return _LrysName; }
            set { _LrysName = value; }
        }
        private decimal _Yzzt;
        [DataMember()]
        public decimal Yzzt
        {
            get { return _Yzzt; }
            set { _Yzzt = value; }
        }

        private String _YzztName;
        [DataMember()]
        public String YzztName
        {
            get { return _YzztName; }
            set { _YzztName = value; }
        }
        /// <summary>
        ///  计价类型
        /// </summary> 
        [DataMember()]
        public int Jjlx
        { get; set; }
        /// <summary>
        ///  计价类型名称
        /// </summary> 
        [DataMember()]
        public string Jjlxmc
        { get; set; }
        /// <summary>
        ///  执行科室代码
        /// </summary> 
        [DataMember()]
        public string Zxksdm
        { get; set; }
        /// <summary>
        ///  执行科室代码名称
        /// </summary> 
        [DataMember()]
        public string Zxksdmmc
        { get; set; }

        /// <summary>
        ///  add by luff 20130313 医嘱是否可选 
        /// </summary> 
        [DataMember()]
        public int Yzkx
        { get; set; }
        /// <summary>
        /// add by luff 20130313 备用拓展字段
        /// </summary> 
        [DataMember()]
        public string Extension
        { get; set; }
        /// <summary>
        /// add by luff 20130313 备用拓展字段
        /// </summary> 
        [DataMember()]
        public string Extension1
        { get; set; }
        /// <summary>
        /// add by luff 20130313 备用拓展字段
        /// </summary> 
        [DataMember()]
        public string Extension2
        { get; set; }
        /// <summary>
        /// add by luff 20130313 备用拓展字段
        /// </summary> 
        [DataMember()]
        public string Extension3
        { get; set; }
        /// <summary>
        /// add by luff 20130313 备用拓展字段
        /// </summary> 
        [DataMember()]
        public int Extension4
        { get; set; }

        /// <summary>
        /// add by luff 20130828 排序字段
        /// </summary> 
        [DataMember()]
        public int OrderValue
        { get; set; }

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}