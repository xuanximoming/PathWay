using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Yidansoft.Service.Entity
{
    public class CP_AdviceGroupDetail
    {
        public String Mzdm
        {
            get;
            set;
        }
        private Decimal _Ctmxxh;
        public Decimal Ctmxxh
        {
            get { return _Ctmxxh; }
            set { _Ctmxxh = value; }
        }
        private Decimal _Ctyzxh;
        public Decimal Ctyzxh
        {
            get { return _Ctyzxh; }
            set { _Ctyzxh = value; }
        }
        private short _Yzbz;
        public short Yzbz
        {
            get { return _Yzbz; }
            set { _Yzbz = value; }
        }
        private decimal _Fzxh;
        public decimal Fzxh
        {
            get { return _Fzxh; }
            set { _Fzxh = value; }
        }
        private short _Fzbz;
        public short Fzbz
        {
            get { return _Fzbz; }
            set { _Fzbz = value; }
        }
        private Decimal _Cdxh;
        public Decimal Cdxh
        {
            get { return _Cdxh; }
            set { _Cdxh = value; }
        }
        private Decimal _Ggxh;
        public Decimal Ggxh
        {
            get { return _Ggxh; }
            set { _Ggxh = value; }
        }
        private Decimal _Lcxh;
        public Decimal Lcxh
        {
            get { return _Lcxh; }
            set { _Lcxh = value; }
        }
        private String _Ypdm;
        public String Ypdm
        {
            get { return _Ypdm; }
            set { _Ypdm = value; }
        }
        private String _Ypmc;
        public String Ypmc
        {
            get { return _Ypmc; }
            set { _Ypmc = value; }
        }
        private short _Xmlb;
        public short Xmlb
        {
            get { return _Xmlb; }
            set { _Xmlb = value; }
        }
        private String _Zxdw;
        public String Zxdw
        {
            get { return _Zxdw; }
            set { _Zxdw = value; }
        }
        private Decimal _Ypjl;
        public Decimal Ypjl
        {
            get { return _Ypjl; }
            set { _Ypjl = value; }
        }
        private String _Jldw;
        public String Jldw
        {
            get { return _Jldw; }
            set { _Jldw = value; }
        }
        private Decimal _Dwxs;
        public Decimal Dwxs
        {
            get { return _Dwxs; }
            set { _Dwxs = value; }
        }
        private int _Dwlb;
        public int Dwlb
        {
            get { return _Dwlb; }
            set { _Dwlb = value; }
        }
        private String _Yfdm;
        public String Yfdm
        {
            get { return _Yfdm; }
            set { _Yfdm = value; }
        }
        private String _Pcdm;
        public String Pcdm
        {
            get { return _Pcdm; }
            set { _Pcdm = value; }
        }
        private Int32 _Zxcs;
        public Int32 Zxcs
        {
            get { return _Zxcs; }
            set { _Zxcs = value; }
        }
        private Int32 _Zxzq;
        public Int32 Zxzq
        {
            get { return _Zxzq; }
            set { _Zxzq = value; }
        }
        private short _Zxzqdw;
        public short Zxzqdw
        {
            get { return _Zxzqdw; }
            set { _Zxzqdw = value; }
        }
        private String _Zdm;
        public String Zdm
        {
            get { return _Zdm; }
            set { _Zdm = value; }
        }
        private String _Zxsj;
        public String Zxsj
        {
            get { return _Zxsj; }
            set { _Zxsj = value; }
        }
        private Int32 _Zxts;
        public Int32 Zxts
        {
            get { return _Zxts; }
            set { _Zxts = value; }
        }
        private Decimal _Ypzsl;
        public Decimal Ypzsl
        {
            get { return _Ypzsl; }
            set { _Ypzsl = value; }
        }
        private String _Ztnr;
        public String Ztnr
        {
            get { return _Ztnr; }
            set { _Ztnr = value; }
        }
        private short _Yzlb;
        public short Yzlb
        {
            get { return _Yzlb; }
            set { _Yzlb = value; }
        }
        private string _YzbzName;
        public string YzbzName
        {
            get { return _YzbzName; }
            set { _YzbzName = value; }
        }
        private string _flag;
        public string Flag
        {
            get { return _flag; }
            set { _flag = value; }
        }
        private int _index;
        public int Index
        {
            get { return _index; }
            set { _index = value; }
        }
        private string _yznr;
        public string Yznr
        {
            get { return _yznr; }
            set { _yznr = value; }
        }
        private Boolean isGroup;
        public Boolean IsGroup
        {
            get { return isGroup; }
            set { isGroup = !(Flag == ""); }
        }
        /// <summary>
        ///  计价类型
        /// </summary> 

        public int Jjlx
        { get; set; }
        /// <summary>
        ///  计价类型名称
        /// </summary> 

        //public string Jjlxmc
        //{ get; set; }
        /// <summary>
        ///  执行科室代码
        /// </summary> 
        public string Zxksdm
        { get; set; }

        /// <summary>
        /// add by luff 20130313 医嘱是否可选
        /// </summary> 
        public int Yzkx
        { get; set; }
        /// <summary>
        /// add by luff 20130313 备用拓展字段
        /// </summary> 
        
        public string Extension
        { get; set; }
        /// <summary>
        /// add by luff 20130313 备用拓展字段
        /// </summary> 
         
        public string Extension1
        { get; set; }
        /// <summary>
        /// add by luff 20130313 备用拓展字段
        /// </summary> 
        
        public string Extension2
        { get; set; }
        /// <summary>
        /// add by luff 20130313 备用拓展字段
        /// </summary> 
         
        public string Extension3
        { get; set; }
        /// <summary>
        /// add by luff 20130313 备用拓展字段
        /// </summary> 
        
        public int Extension4
        { get; set; }

        /// <summary>
        /// add by Jhonny 20130827  排序标识字段
        /// </summary>
        public int _OrderValue { get; set; }

        public CP_AdviceGroupDetail(Decimal Ctmxxh, Decimal Ctyzxh, short Yzbz, decimal Fzxh, short Fzbz, Decimal Cdxh, Decimal Ggxh, Decimal Lcxh, String Ypdm, String Ypmc, short Xmlb,
             String Zxdw, Decimal Ypjl, String Jldw, Decimal Dwxs, short Dwlb, String Yfdm, String Pcdm, Int32 Zxcs, int Zxzq, short Zxzqdw, String Zdm, String Zxsj,
             int Zxts, Decimal Ypzsl, String Ztnr, short Yzlb, string YzbzName, string Flag, int Index, int jjlx, string zxksdm, int iyzkx, string Yznr, string sExtension, string sExtension1, string sExtension2, string sExtension3, int sExtension4,int OrderValue)
        {
            _Ctmxxh = Ctmxxh;

            _Ctyzxh = Ctyzxh;

            _Yzbz = Yzbz;

            _Fzxh = Fzxh;
            _Fzbz = Fzbz;

            _Cdxh = Cdxh;

            _Ggxh = Ggxh;

            _Lcxh = Lcxh;

            _Ypdm = Ypdm;

            _Ypmc = Ypmc;

            _Xmlb = Xmlb;

            _Zxdw = Zxdw;

            _Ypjl = Ypjl;

            _Jldw = Jldw;

            _Dwxs = Dwxs;

            _Dwlb = Dwlb;

            _Yfdm = Yfdm;

            _Pcdm = Pcdm;

            _Zxcs = Zxcs;

            _Zxzq = Zxzq;

            _Zxzqdw = Zxzqdw;

            _Zdm = Zdm;

            _Zxsj = Zxsj;

            _Zxts = Zxts;

            _Ypzsl = Ypzsl;

            _Ztnr = Ztnr;

            _Yzlb = Yzlb;

            _YzbzName = YzbzName;

            _flag = Flag;

            _index = Index;

            Jjlx = jjlx;

            Zxksdm = zxksdm;

            Yzkx = iyzkx;

            _yznr = Yznr;

            Extension = sExtension;
            Extension1 = sExtension1;
            Extension2 = sExtension2;
            Extension3 = sExtension3;
            Extension4 = sExtension4;
            _OrderValue = OrderValue;
        }
        public CP_AdviceGroupDetail()
        {

        }

    }
}