using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Yidansoft.Service.Entity
{
   /// <summary>
   /// 实体类CP_PCSJ从数据库取数据
   /// </summary>
    public class CP_PCSJ
    {
        /// <summary>
        /// 执行周期
        /// </summary>
        private int _zxzq;
        public int Zxzq
        {
            get { return _zxzq; }
            set { _zxzq = value; }
        }
        /// <summary>
        /// 执行次数
        /// </summary>
        private int _zxcs;
        public int Zxcs
        {
            get { return _zxcs; }
            set { _zxcs = value; }
          
        }
        /// <summary>
        /// 明细编号
        /// </summary>
        private short _mxbh;

        public short Mxbh
        {
            get { return _mxbh; }
            set { _mxbh = value; }
        }
        /// <summary>
        /// 单位名称如：小时，周
        /// </summary>
        private string _dwname;

        public string DwName
        {
            get { return _dwname; }
            set { _dwname = value; }
        }
        /// <summary>
        /// 执行单位标记如;week,day,hour等
        /// </summary>
        private string _dwflag;
        public string DwFlag
        {
            get { return _dwflag; }
            set { _dwflag = value; }
        }
        /// <summary>
        /// 执行时间
        /// </summary>
        private string _zxsj;
        public string Zxsj
        {
            get { return _zxsj; }
            set { _zxsj = value; }
        }
        /// <summary>
        /// 执行周代码
        /// </summary>
        private string _zdm;
        public string Zdm
        {
            get { return _zdm; }
            set { _zdm = value; }
        }
        /// <summary>
        /// 显示信息
        /// </summary>
        private string _displayText;
        public string DisplayText
        {
            get { return _displayText; }
            set { _displayText = value; }
        }
       
    }
}