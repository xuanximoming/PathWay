using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 存放临床路径完成率报表数据源
    /// </summary>
    [DataContract()]
    public class RPT_PathFinish
    {
        /// <summary>
        /// 存放存放临床路径完成率报表查询返回异常
        /// </summary>
        private string m_message = "";

        /// <summary>
        /// 存放临床路径完成率现状图对应的数据源List
        /// </summary>
        [DataMember]
        public List<Rpt_PathFinishImage> PathFinishImage
        {
            get;
            set;
        }

        /// <summary>
        /// 存放临床路径完成率报表的列表信息数据源List
        /// </summary>
        [DataMember]
        public List<RPT_PathFinishList> PathFinishList
        {
            get;
            set;
        }

        /// <summary>
        /// 存放存放临床路径完成率报表查询返回异常信息
        /// </summary>
        [DataMember]
        public string Message
        {
            get { return m_message; }
            set { m_message = value; }
        }

    }

    /// <summary>
    /// 存放临床路径完成率报表现状图需要的数据源
    /// </summary>
    [DataContract()]
    public class Rpt_PathFinishImage
    {
        //路径代码
        private string m_pathid;

        //路径名称
        private string m_pathname;

        //周期名称
        private string m_period_name;

        //完成路径人数
        private string m_finishcount;

        //进入路径总人数
        private string m_totalcount;

        //完成率
        private string m_rate;

        //列表中显示完成信息
        private string m_mess;

        //列下标
        private string m_colindex;

        //行下标
        private string m_rownumber;



        /// <summary>
        /// 路径代码
        /// </summary>
        [DataMember]
        public string PathID
        {
            get { return m_pathid; }
            set { m_pathid = value; }
        }

        /// <summary>
        /// 路径名称
        /// </summary>
        [DataMember]
        public string PathName
        {
            get { return m_pathname; }
            set { m_pathname = value; }
        }

        /// <summary>
        /// 周期名
        /// </summary>
        [DataMember()]
        public string PeriodName
        {
            get { return m_period_name; }
            set { m_period_name = value; }
        }

        /// <summary>
        /// 完成数量
        /// </summary>
        [DataMember()]
        public string FinishCount
        {
            get { return m_finishcount; }
            set { m_finishcount = value; }
        }

        /// <summary>
        /// 当前周期总进入路径人数
        /// </summary>
        [DataMember()]
        public string TotalCount
        {
            get { return m_totalcount; }
            set { m_totalcount = value; }
        }

        /// <summary>
        /// 完成比例
        /// </summary>
        [DataMember()]
        public string Rate
        {
            get { return m_rate; }
            set { m_rate = value; }
        }

        /// <summary>
        /// 页面显示信息
        /// </summary>
        [DataMember()]
        public string Mess
        {
            get { return m_mess; }
            set { m_mess = value; }
        }
        /// <summary>
        /// 列下标
        /// </summary>
        [DataMember()]
        public string ColIndex
        {
            get { return m_colindex; }
            set { m_colindex = value; }
        }

        /// <summary>
        /// 行下标
        /// </summary>
        [DataMember()]
        public string RowNumber
        {
            get { return m_rownumber; }
            set { m_rownumber = value; }
        }
    }

    /// <summary>
    /// 存放临床路径完成率报表列表需要的数据源
    /// </summary>
    [DataContract()]
    public class RPT_PathFinishList
    {
        //路径代码
        private string m_pathid = "";
        //路径名称
        private string m_pathname = "";
        //
        private string m_col_a = "";
        //
        private string m_col_b = "";
        //
        private string m_col_c = "";
        //
        private string m_col_d = "";
        //
        private string m_col_e = "";
        //
        private string m_col_f = "";
        //
        private string m_col_g = "";
        //
        private string m_col_h = "";
        //
        private string m_col_i = "";
        //
        private string m_col_j = "";
        //
        private string m_col_k = "";
        //        
        private string m_col_l = "";
        //
        private string m_col_m = "";
        //
        private string m_col_n = "";
        //
        private string m_col_o = "";

        [DataMember()]
        public string PathID
        {
            get { return m_pathid; }
            set { m_pathid = value; }
        }

        [DataMember()]
        public string PathName
        {
            get { return m_pathname; }
            set { m_pathname = value; }
        }

        [DataMember()]
        public string Col_A
        {
            get { return m_col_a; }
            set { m_col_a = value; }
        }

        [DataMember()]
        public string Col_B
        {
            get { return m_col_b; }
            set { m_col_b = value; }
        }

        [DataMember()]
        public string Col_C
        {
            get { return m_col_c; }
            set { m_col_c = value; }
        }

        [DataMember()]
        public string Col_D
        {
            get { return m_col_d; }
            set { m_col_d = value; }
        }

        [DataMember()]
        public string Col_E
        {
            get { return m_col_e; }
            set { m_col_e = value; }
        }

        [DataMember()]
        public string Col_F
        {
            get { return m_col_f; }
            set { m_col_f = value; }
        }

        [DataMember()]
        public string Col_G
        {
            get { return m_col_g; }
            set { m_col_g = value; }
        }

        [DataMember()]
        public string Col_H
        {
            get { return m_col_h; }
            set { m_col_h = value; }
        }

        [DataMember()]
        public string Col_I
        {
            get { return m_col_i; }
            set { m_col_i = value; }
        }

        [DataMember()]
        public string Col_J
        {
            get { return m_col_j; }
            set { m_col_j = value; }
        }

        [DataMember()]
        public string Col_K
        {
            get { return m_col_k; }
            set { m_col_k = value; }
        }

        [DataMember()]
        public string Col_L
        {
            get { return m_col_l; }
            set { m_col_l = value; }
        }

        [DataMember()]
        public string Col_M
        {
            get { return m_col_m; }
            set { m_col_m = value; }
        }

        [DataMember()]
        public string Col_N
        {
            get { return m_col_n; }
            set { m_col_n = value; }
        }

        [DataMember()]
        public string Col_O
        {
            get { return m_col_o; }
            set { m_col_o = value; }
        }
    }

}