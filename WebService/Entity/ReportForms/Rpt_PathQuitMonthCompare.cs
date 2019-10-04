using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service
{
    /// <summary>
    /// 表示存放月度出径环比的数据源的类
    /// </summary>
    [DataContract()]
    public class Rpt_PathQuitMonthCompare
    {

        private String m_message = "";
        /// <summary>
        /// 表示存放月度出径环比异常信息的属性
        /// </summary>
        [DataMember()]
        public String Message
        {
            get { return m_message; }
            set { m_message = value; }
        }

        /// <summary>
        /// 表示存放月度出径环比列表的属性
        /// </summary>
        [DataMember()]
        public List<Rpt_PathQuitMonthCompareList> PathCompareList
        {
            get;
            set;
        }

        /// <summary>
        /// 表示存放月度出径环比图表的属性
        /// </summary>
        [DataMember()]
        public List<Rpt_PathQuitMonthCompareImage> PathCompareImage
        {
            get;
            set;
        }

    }

    /// <summary>
    /// 表示存放月度出径环比列表数据源的类
    /// </summary>
    [DataContract()]
    public class Rpt_PathQuitMonthCompareList
    {

        /// <summary>
        /// 表示路径代码的属性
        /// </summary>
        [DataMember()]
        public String Ljdm
        {
            get;
            set;
        }
        /// <summary>
        /// 表示路径名称的属性
        /// </summary>
        [DataMember()]
        public String Ljmc
        {
            get;
            set;
        }
        /// <summary>
        /// 表示适应科室的属性
        /// </summary>
        [DataMember()]
        public String Syks
        {
            get;
            set;
        }
        [DataMember()]
        public Int32 Jan
        {
            get;
            set;
        }
        [DataMember()]
        public Int32 Feb
        {
            get;
            set;
        }
        [DataMember()]
        public Int32 Mar
        {
            get;
            set;
        }
        [DataMember()]
        public Int32 Apr
        {
            get;
            set;
        }
        [DataMember()]
        public Int32 May
        {
            get;
            set;
        }
        [DataMember()]
        public Int32 Jun
        {
            get;
            set;
        }
        [DataMember()]
        public Int32 Jul
        {
            get;
            set;
        }
        [DataMember()]
        public Int32 Aug
        {
            get;
            set;
        }
        [DataMember()]
        public Int32 Sept
        {
            get;
            set;
        }
        [DataMember()]
        public Int32 Oct
        {
            get;
            set;
        }
        [DataMember()]
        public Int32 Nov
        {
            get;
            set;
        }
        [DataMember()]
        public Int32 Dec
        {
            get;
            set;
        }
    }
    /// <summary>
    /// 表示存放月度出径环比图表的类
    /// </summary>
    [DataContract()]
    public class Rpt_PathQuitMonthCompareImage
    {
        private String name;
        private Int32 count;

        /// <summary>
        /// 表示字段名的属性
        /// </summary>
        [DataMember()]
        public String Name
        {
            get { return name; }
            set { name = value; }
        }

        /// <summary>
        /// 表示字段值的属性
        /// </summary>
        [DataMember()]
        public Int32 Count
        {
            get { return count; }
            set { count = value; }
        }

    }
}