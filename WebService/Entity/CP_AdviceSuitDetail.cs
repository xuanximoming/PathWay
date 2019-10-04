using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract]
    public class CP_AdviceSuitDetail
    {
        /// <summary>
        /// 医嘱套餐明细序号
        /// </summary>
        [DataMember()]
        public Decimal Ctmxxh { get; set; }
        /// <summary>
        /// (所属)医嘱套餐序号(CP_AdviceSuit.Ctyzxh)
        /// </summary>
        [DataMember()]
        public Decimal Ctyzxh { get; set; }
        /// <summary>
        /// 医嘱标志(CP_DataCategory.Mxbh, Lbbh = 27)
        /// </summary>
        [DataMember()]
        public String Yzbz { get; set; }
        /// <summary>
        /// 分组序号(每组的第一条医嘱的序号)
        /// </summary>
        [DataMember()]
        public String Fzxh { get; set; }
        /// <summary>
        /// 分组标志(CP_DataCategory.Mxbh, Lbbh = 35)
        /// </summary>
        [DataMember()]
        public String Fzbz { get; set; }
        /// <summary>
        /// 产地序号(CP_PlaceOfDrug.Cdxh)
        /// </summary>
        [DataMember()]
        public String Cdxh { get; set; }
        /// <summary>
        /// 规格序号(CP_FormatOfDrug.Ggxh)
        /// </summary>
        [DataMember()]
        public String Ggxh { get; set; }
        /// <summary>
        /// 临床序号(CP_ClinicOfDrug.Lcxh)
        /// </summary>
        [DataMember()]
        public String Lcxh { get; set; }
        /// <summary>
        /// 药品代码(or收费小项目代码or临床项目代码,具体是哪种代码根据项目类别来判断)(CP_PlaceOfDrug.Ypdm/CP_ChargingMinItem.Sfxmdm/CP_LCChargingItem.Lcxmdm)
        /// </summary>
        [DataMember()]
        public String Ypdm { get; set; }
        /// <summary>
        /// 药品(项目)名称(CP_PlaceOfDrug.Name/CP_ChargingMinItem.Name)
        /// </summary>
        [DataMember()]
        public String Ypmc { get; set; }
        /// <summary>
        /// 项目类别(CP_DataCategory.Mxbh, Lbbh = 24)
        /// </summary>
        [DataMember()]
        public String Xmlb { get; set; }
        /// <summary>
        /// 最小单位(CP_PlaceOfDrug.Zxdw)
        /// </summary>
        [DataMember()]
        public String Zxdw { get; set; }
        /// <summary>
        /// 药品剂量
        /// </summary>
        [DataMember()]
        public String Ypjl { get; set; }
        /// <summary>
        /// 剂量单位(显示用)(CP_PlaceOfDrug.Ggdw/CP_PlaceOfDrug.Zydw)
        /// </summary>
        [DataMember()]
        public String Jldw { get; set; }
        /// <summary>
        /// 单位系数(规格系数或住院系数,注意规格系数要存它的分数 
        /// </summary>
        [DataMember()]
        public String Dwxs { get; set; }
        /// <summary>
        /// 单位类别(CP_DataCategory.Mxbh, Lbbh = 30)
        /// </summary>
        [DataMember()]
        public String Dwlb { get; set; }
        /// <summary>
        /// (药品)用法代码(CP_DrugUseage.Yfdm)
        /// </summary>
        [DataMember()]
        public String Yfdm { get; set; }
        /// <summary>
        /// 频次代码(YY_YZPCK.Pcdm)
        /// </summary>
        [DataMember()]
        public String Pcdm { get; set; }
        /// <summary>
        /// 执行次数
        /// </summary>
        [DataMember()]
        public String Zxcs { get; set; }
        /// <summary>
        /// 执行周期
        /// </summary>
        [DataMember()]
        public String Zxzq { get; set; }
        /// <summary>
        /// 执行周期单位(CP_DataCategory.Mxbh, Lbbh = 34)
        /// </summary>
        [DataMember()]
        public String Zxzqdw { get; set; }
        /// <summary>
        /// 周代码
        /// </summary>
        [DataMember()]
        public String Zdm { get; set; }
        /// <summary>
        /// (频次的)执行时间
        /// </summary>
        [DataMember()]
        public String Zxsj { get; set; }
        /// <summary>
        /// 执行天数(为出院带药保留)
        /// </summary>
        [DataMember()]
        public String Zxts { get; set; }
        /// <summary>
        /// 药品总数量(为出院带药保留,使用剂量单位)
        /// </summary>
        [DataMember()]
        public String Ypzsl { get; set; }
        /// <summary>
        /// 嘱托内容
        /// </summary>
        [DataMember()]
        public String Ztnr { get; set; }
        /// <summary>
        /// 医嘱类别(CP_DataCategory.Mxbh, Lbbh = 31)
        /// </summary>
        [DataMember()]
        public String Yzlb { get; set; }
        /// <summary>
        /// --排序字段
        /// </summary>
        [DataMember()]
        public String OrderNum { get; set; }
        /// <summary>
        /// 医嘱内容(显示在界面上的医嘱内容)
        /// </summary>
        [DataMember()]
        public String Yznr { get; set; }
        /// <summary>
        /// 分组符号CASE  Fzbz WHEN 3501 THEN '┓' WHEN 3509 then '┛' WHEN 3502 then '┃'  else '' END AS FzbzSymbol
        /// </summary>
        [DataMember()]
        public String FzbzSymbol { get; set; }
        /// <summary>
        /// 医嘱标志名称（长期，临时）
        /// </summary>
        [DataMember()]
        public String YzbzName { get; set; }

        /// <summary>
        /// 用法名称（静滴，口服等）
        /// </summary>
        [DataMember()]
        public String YfdmName { get; set; }

        /// <summary>
        /// 频次代码对应的名称
        /// </summary>
        [DataMember()]
        public String PcdmName { get; set; }
        /// <summary>
        ///  计价类型
        /// </summary> 
        [DataMember()]
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
        [DataMember()]
        public string Zxksdm
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
       

    }
}