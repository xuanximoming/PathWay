using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Yidansoft.Service.Entity
{
    public class CP_AdviceAnesthesiaDetail
    {
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

        private Decimal _Fzxh;
        public Decimal Fzxh
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

        private String _Ssdm;
        public String Ssdm
        {
            get { return _Ssdm; }
            set { _Ssdm = value; }
        }

        private String _Ypmc;
        public String Ypmc
        {
            get { return _Ypmc; }
            set { _Ypmc = value; }
        }

        private String _Mzdm;
        public String Mzdm
        {
            get { return _Mzdm; }
            set { _Mzdm = value; }
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

        private short _Dwlb;
        public short Dwlb
        {
            get { return _Dwlb; }
            set { _Dwlb = value; }
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

        private String _Memo;
        public String Memo
        {
            get { return _Memo; }
            set { _Memo = value; }
        }
        private String _Flag;
        public String Flag
        {
            get { return _Flag; }
            set { _Flag = value; }
        }
        private int _Index;
        public int Index
        {
            get { return _Index; }
            set { _Index = value; }
        }
        private String _YzbzName;
        public String YzbzName
        {
            get { return _YzbzName; }
            set { _YzbzName = value; }
        }
        private String _Yznr;
        public String Yznr
        {
            get { return _Yznr; }
            set { _Yznr = value; }
        }
        public CP_AdviceAnesthesiaDetail()
        {

        }
        public CP_AdviceAnesthesiaDetail(Decimal Ctmxxh, Decimal Ctyzxh, short Yzbz, Decimal Fzxh, short Fzbz, String Ssdm, String Ypmc, String Mzdm, short Xmlb, String Zxdw, short Dwlb,
            String Ztnr, short Yzlb, String Memo, String Flag, int Index, String YzbzName, String Yznr)
        {
            _Ctmxxh = Ctmxxh;

            _Ctyzxh = Ctyzxh;

            _Yzbz = Yzbz;

            _Fzxh = Fzxh;

            _Fzbz = Fzbz;

            _Ssdm = Ssdm;

            _Ypmc = Ypmc;

            _Mzdm = Mzdm;

            _Xmlb = Xmlb;

            _Zxdw = Zxdw;

            _Dwlb = Dwlb;

            _Ztnr = Ztnr;

            _Yzlb = Yzlb;

            _Memo = Memo;

            _Flag = Flag;

            _Index = Index;

            _YzbzName = YzbzName;

            _Yznr = Yznr;
        }

    }
}