using System;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 长期&临时共用
    /// 特有字段会标识出来，未特别说明为共用属性
    /// </summary>
    [DataContract()]
    public partial class CP_DoctorOrder
    {
        #region property
        /// <summary>
        ///  医嘱序号
        /// </summary> 
        [DataMember()]
        public decimal Yzxh
        { get; set; }

        /// <summary>
        ///  首页序号
        /// </summary> 
        [DataMember()]
        public decimal Syxh
        { get; set; }

        /// <summary>
        ///  分组序号
        /// </summary> 
        [DataMember()]
        public decimal Fzxh
        { get; set; }

        /// <summary>
        ///  分组标志
        /// </summary> 
        [DataMember()]
        public decimal Fzbz
        { get; set; }

        /// <summary>
        ///  病区代码
        /// </summary> 
        [DataMember()]
        public string Bqdm
        { get; set; }

        /// <summary>
        ///  科室代码
        /// </summary> 
        [DataMember()]
        public string Ksdm
        { get; set; }

        /// <summary>
        ///  录入医生代码
        /// </summary> 
        [DataMember()]
        public string Lrysdm
        { get; set; }

        /// <summary>
        ///  录入日期
        /// </summary> 
        [DataMember()]
        public string Lrrq
        { get; set; }

        /// <summary>
        ///  审核操作员
        /// </summary> 
        [DataMember()]
        public string Shczy
        { get; set; }

        /// <summary>
        ///  审核日期
        /// </summary> 
        [DataMember()]
        public string Shrq
        { get; set; }

        /// <summary>
        ///  执行操作员
        /// </summary> 
        [DataMember()]
        public string Zxczy
        { get; set; }

        /// <summary>
        ///  执行日期
        /// </summary> 
        [DataMember()]
        public string Zxrq
        { get; set; }

        /// <summary>
        ///  取消医生代码
        /// </summary> 
        [DataMember()]
        public string Qxysdm
        { get; set; }

        /// <summary>
        ///  取消日期
        /// </summary> 
        [DataMember()]
        public string Qxrq
        { get; set; }

        /// <summary>
        ///  停止医生代码,长嘱
        /// </summary> 
        [DataMember()]
        public string Tzysdm
        { get; set; }

        /// <summary>
        ///  停止日期
        /// </summary> 
        [DataMember()]
        public string Tzrq
        { get; set; }

        /// <summary>
        ///  停止审核护士,长嘱
        /// </summary> 
        [DataMember()]
        public string Tzshhs
        { get; set; }

        /// <summary>
        ///  停止审核护士,长嘱
        /// </summary> 
        [DataMember()]
        public string Tzshrq
        { get; set; }

        /// <summary>
        ///  明起,长嘱
        /// </summary> 
        [DataMember()]
        public decimal Mq
        { get; set; }

        /// <summary>
        ///  (医嘱)开始日期
        /// </summary> 
        [DataMember()]
        public string Ksrq
        { get; set; }

        /// <summary>
        ///  产地序号
        /// </summary> 
        [DataMember()]
        public decimal Cdxh
        { get; set; }

        /// <summary>
        ///  规格序号
        /// </summary> 
        [DataMember()]
        public decimal Ggxh
        { get; set; }

        /// <summary>
        ///  临床序号
        /// </summary> 
        [DataMember()]
        public decimal Lcxh
        { get; set; }
        /// <summary>
        ///  项目单价
        /// </summary> 
        [DataMember()]
        public decimal Xmdj
        { get; set; }
        /// <summary>
        ///  药品代码
        /// </summary> 
        [DataMember()]
        public string Ypdm
        { get; set; }

        /// <summary>
        ///  药品(项目)名称
        /// </summary> 
        [DataMember()]
        public string Ypmc
        { get; set; }

        /// <summary>
        ///  药品规格
        /// </summary> 
        [DataMember()]
        public string Ypgg
        { get; set; }

        /// <summary>
        ///  项目类别
        /// </summary> 
        [DataMember()]
        public decimal Xmlb
        { get; set; }

        /// <summary>
        ///  最小单位
        /// </summary> 
        [DataMember()]
        public string Zxdw
        { get; set; }

        /// <summary>
        ///  药品剂量
        /// </summary> 
        [DataMember()]
        public decimal Ypjl
        { get; set; }

        /// <summary>
        ///  剂量单位(显示用)
        /// </summary> 
        [DataMember()]
        public string Jldw
        { get; set; }

        /// <summary>
        ///  单位系数
        /// </summary> 
        [DataMember()]
        public decimal Dwxs
        { get; set; }

        /// <summary>
        ///  单位类别
        /// </summary> 
        [DataMember()]
        public decimal Dwlb
        { get; set; }

        /// <summary>
        ///  (药品)用法代码
        /// </summary> 
        [DataMember()]
        public string Yfdm
        { get; set; }

        /// <summary>
        ///  (药品)用法代码Name
        /// </summary> 
        [DataMember()]
        public string YfdmName
        { get; set; }

        /// <summary>
        ///  频次代码
        /// </summary> 
        [DataMember()]
        public string Pcdm
        { get; set; }

        /// <summary>
        ///  频次代码Name
        /// </summary> 
        [DataMember()]
        public string PcdmName
        { get; set; }

        /// <summary>
        ///  执行次数
        /// </summary> 
        [DataMember()]
        public decimal Zxcs
        { get; set; }

        /// <summary>
        ///  执行周期
        /// </summary> 
        [DataMember()]
        public decimal Zxzq
        { get; set; }

        /// <summary>
        ///  执行周期单位
        /// </summary> 
        [DataMember()]
        public decimal Zxzqdw
        { get; set; }

        /// <summary>
        ///  周代码
        /// </summary> 
        [DataMember()]
        public string Zdm
        { get; set; }

        /// <summary>
        ///  (频次的)执行时间
        /// </summary> 
        [DataMember()]
        public string Zxsj
        { get; set; }

        /// <summary>
        ///  项目的执行科室(长嘱为保留字段，临嘱目前只对从申请单插入的项目有效)
        /// </summary> 
        [DataMember()]
        public string Zxks
        { get; set; }

        /// <summary>
        ///   执行天数(为出院带药保留)，临嘱
        /// </summary> 
        [DataMember()]
        public decimal Zxts
        { get; set; }

        /// <summary>
        ///   药品总数量(为出院带药保留,使用剂量单位)，临嘱
        /// </summary> 
        [DataMember()]
        public decimal Ypzsl
        { get; set; }

        /// <summary>
        ///   嘱托内容
        /// </summary> 
        [DataMember()]
        public string Ztnr
        { get; set; }

        /// <summary>
        ///   医嘱类别
        /// </summary> 
        [DataMember()]
        public decimal Yzlb
        { get; set; }

        /// <summary>
        ///   医嘱状态
        /// </summary> 
        [DataMember()]
        public decimal Yzzt
        { get; set; }

        /// <summary>
        ///   特殊标记
        ///  --0x01(1)	自备药
        ///	--0x02(2)	输液
        ///	--0x04(4)	打印
        ///--0x08(8)	手术停长期医嘱(录入手术医嘱时，用户选择“停”则在该手术医嘱嘱托中加上“停长期医嘱”)
        ///	--0x10(16)	需要医保审批
        /// </summary> 
        [DataMember()]
        public decimal Tsbj
        { get; set; }

        /// <summary>
        ///   医保审批通过
        /// </summary> 
        [DataMember()]
        public decimal Ybsptg
        { get; set; }

        /// <summary>
        ///   医保审批编号
        /// </summary> 
        [DataMember()]
        public string Ybspbh
        { get; set; }

        /// <summary>
        ///   医嘱停止(或取消)原因,长嘱
        /// </summary> 
        [DataMember()]
        public decimal Yztzyy
        { get; set; }

        /// <summary>
        ///   手术医嘱序号,长嘱
        /// </summary> 
        [DataMember()]
        public decimal Ssyzxh
        { get; set; }

        /// <summary>
        ///   停止序号,临嘱
        /// </summary> 
        [DataMember()]
        public decimal Tzxh
        { get; set; }

        /// <summary>
        ///   (医技)申请单序号,临嘱
        /// </summary> 
        [DataMember()]
        public decimal Sqdxh
        { get; set; }

        /// <summary>
        /// 医嘱内容(显示在界面上的医嘱内容)  
        /// </summary> 
        [DataMember()]
        public string Yznr
        { get; set; }

        /// <summary>
        /// 同步标志 
        /// </summary> 
        [DataMember()]
        public decimal Tbbz
        { get; set; }

        /// <summary>
        /// 备注 
        /// </summary> 
        [DataMember()]
        public string Memo
        { get; set; }

        /// <summary>
        /// 单据分类(兼容护理) 
        /// </summary> 
        [DataMember()]
        public string Djfl
        { get; set; }

        /// <summary>
        /// 必须标准（1.是0.否）
        /// </summary> 
        [DataMember()]
        public decimal Bxbz
        { get; set; }

        /// <summary>                                                       
        /// 医嘱标志
        /// </summary> 
        [DataMember()]
        public decimal Yzbz
        { get; set; }

        /// <summary>
        /// 医嘱标志NAME,即类型
        /// </summary> 
        [DataMember()]
        public string YzbzName
        { get; set; }

        /// <summary>
        /// FromTable
        /// </summary> 
        [DataMember()]
        public string FromTable
        { get; set; }

        /// <summary>
        /// 分组标志（显示）
        /// </summary> 
        [DataMember()]
        public string Flag
        { get; set; }

        private bool m_IsModify;
        /// <summary>
        /// 是否被修改过
        /// </summary> 
        [DataMember()]
        public bool IsModify
        {
            get { return m_IsModify; }
            set { m_IsModify = value; }
        }


        /// <summary>
        /// 用来分
        /// </summary>
        [DataMember()]
        public string OrderGuid
        {
            get;
            set;
        }

        /// <summary>
        /// 结点
        /// </summary>
        [DataMember()]
        public string PathDetailID
        {
            get;
            set;
        }

        /// <summary>
        ///  成套明细序号
        /// </summary> 
        [DataMember()]
        public decimal Ctmxxh
        { get; set; }

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
        /// add by luff 20130313 医嘱是否可选
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
        #endregion property


        public CP_DoctorOrder()
        { }



        public CP_DoctorOrder(decimal yzxh, decimal syxh, decimal fzxh, decimal fzbz,
                            string strBqdm, string strKsdm, string strLrysdm, string strLrrq, string strShczy,
                            string strShrq, string strZxczy, string strZxrq, string strQxysdm, string strQxrq,
                            string strtzysdm, string strTzrq, string strtzshhs, string strtzshrq, string strKsrq,
                           decimal mq, decimal cdxh, decimal ggxh, decimal lcxh, decimal xmdj, string strYpdm,
                          string strYpmc, string strYpgg, decimal xmlb, string strZxdw, decimal ypjl, string strJldw, decimal dwxs,
                         decimal dwlb, string strYfdm, string strYfdmName, string strPcdm, string strPcdmName, decimal zxcs, decimal zxzq, decimal zxzqdw,
                        string strZdm, string strZxsj, string strZxks, string strZtnr, decimal yzlb, decimal yzzt, decimal tsbj,
                       decimal ybsptg, string strYbspbh, decimal yztzyy, decimal ssyzxh, string strYznr, decimal tbbz, string strMemo,
                      decimal bxbz, string strDjfl, decimal zxts, decimal ypzsl, decimal tzxh, decimal sqdxh,
                      decimal yzbz, string stryzbzName, string strFromTable, string strFlag, int sJjlx, string sJjlxmc, string sZxksdm, string sZxksdmmc, int sYzkx, string sExtension, string sExtension1, string sExtension2, string sExtension3, int sExtension4, int sOrderValue)
        {
            Yzxh = yzxh;
            Syxh = syxh;
            Fzxh = fzxh;
            Fzbz = fzbz;
            Bqdm = strBqdm;
            Ksdm = strKsdm;
            Lrysdm = strLrysdm;
            Lrrq = strLrrq;
            Shczy = strShczy;
            Shrq = strShrq;
            Zxczy = strZxczy;
            Zxrq = strZxrq;
            Qxysdm = strQxysdm;
            Qxrq = strQxrq;
            Tzysdm = strtzysdm;
            Tzrq = strTzrq;
            Tzshhs = strtzshhs;
            Tzshrq = strtzshrq;
            Ksrq = strKsrq;
            Mq = mq;
            Cdxh = cdxh;
            Ggxh = ggxh;
            Lcxh = lcxh;
            Xmdj = xmdj;
            Ypdm = strYpdm;
            Ypmc = strYpmc;
            Ypgg = strYpgg;
            Xmlb = xmlb;
            Zxdw = strZxdw;
            Ypjl = ypjl;
            Jldw = strJldw;
            Dwxs = dwxs;
            Dwlb = dwlb;
            Yfdm = strYfdm;
            YfdmName = strYfdmName;
            Pcdm = strPcdm;
            PcdmName = strPcdmName;
            Zxcs = zxcs;
            Zxzq = zxzq;
            Zxzqdw = zxzqdw;
            Zdm = strZdm;
            Zxsj = strZxsj;
            Zxks = strZxks;
            Ztnr = strZtnr;
            Yzlb = yzlb;
            Yzzt = yzzt;
            Tsbj = tsbj;
            Ybsptg = ybsptg;
            Ybspbh = strYbspbh;
            Yztzyy = yztzyy;
            Ssyzxh = ssyzxh;
            Yznr = strYznr;
            Tbbz = tbbz;
            Memo = strMemo;
            Bxbz = bxbz;
            Djfl = strDjfl;
            Zxts = zxts;
            Ypzsl = ypzsl;
            Tzxh = tzxh;
            Sqdxh = sqdxh;
            Yzbz = yzbz;
            YzbzName = stryzbzName;
            FromTable = strFromTable;
            Flag = strFlag;
            Jjlx = sJjlx;
            Jjlxmc = sJjlxmc;
            Zxksdm = sZxksdm;
            Zxksdmmc = sZxksdmmc;
            Yzkx = sYzkx;
            Extension = sExtension;
            Extension1 = sExtension1;
            Extension2 = sExtension2;
            Extension3 = sExtension3;
            Extension4 = sExtension4;
            OrderValue = sOrderValue;
            OrderGuid = Guid.NewGuid().ToString();


        }
    }
}