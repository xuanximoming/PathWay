using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.NursingNotes
{
   /// <summary>
   /// 病人入量/出量实体 
   /// modify by xjt 20110217 之前类名与数据库里的表名一样，YidanEHRTableEntity.edmx一旦更新，会报错
   /// </summary>
   [DataContract]
   public class CP_PatientInOutRecordInfo
   {
      /// <summary>
      /// 记录序号（自动生成）
      /// </summary>
      [DataMember]
      public string Jlxh { get; set; }

      /// <summary>
      /// 住院号码
      /// </summary>
      [DataMember]
      public string Zyhm { get; set; }

      /// <summary>
      /// 记录类型，0-入量，1-出量
      /// </summary>
      [DataMember]
      public int Jllx { get; set; }

      /// <summary>
      /// 测量日期（格式2010-01-01）
      /// </summary>
      [DataMember]
      public string Clrq { get; set; }

      /// <summary>
      /// 测量时间（格式：01:02:03）
      /// </summary>
      [DataMember]
      public string Clsj { get; set; }

      //-----病人入量-----

      /// <summary>
      /// 饮食量
      /// </summary>
      [DataMember]
      public string Ysl { get; set; }

      /// <summary>
      /// 饮水量
      /// </summary>
      [DataMember]
      public string Hsl { get; set; }

      /// <summary>
      /// 输液量
      /// </summary>
      [DataMember]
      public string Syl { get; set; }

      /// <summary>
      /// 注射量
      /// </summary>
      [DataMember]
      public string Zsl { get; set; }

      /// <summary>
      /// 输血量
      /// </summary>
      [DataMember]
      public string Sxl { get; set; }

      /// <summary>
      /// 其他入量1类型代码，(CP_DataCategoryDetail.lbbh=50)
      /// </summary>
      [DataMember]
      public int Qtrllxdm1 { get; set; }

      /// <summary>
      /// 其他入量1类型名称,格式:类型名称(单位)
      /// </summary>
      [DataMember]
      public string Qtrllx1 { get; set; }

      /// <summary>
      /// 其他入量1值
      /// </summary>
      [DataMember]
      public string Qtrl1 { get; set; }

      /// <summary>
      /// 其他入量2类型代码 (CP_DataCategoryDetail.lbbh=50)
      /// </summary>
      [DataMember]
      public int Qtrllxdm2 { get; set; }

      /// <summary>
      /// 其他入量2类型名称,格式:类型名称(单位)
      /// </summary>
      [DataMember]
      public string Qtrllx2 { get; set; }

      /// <summary>
      /// 其他入量2值
      /// </summary>
      [DataMember]
      public string Qtrl2 { get; set; }

      //-----病人出量-----

      /// <summary>
      /// 患者小便
      /// </summary>
      [DataMember]
      public string Hzxb { get; set; }

      /// <summary>
      /// 小便性状代码，(CP_DataCategoryDetail.lbbh=51)
      /// </summary>
      [DataMember]
      public int Xbxzdm { get; set; }

      /// <summary>
      /// 小便性状，格式:颜色_形状，如：黄色_柱状
      /// </summary>
      [DataMember]
      public string Xbxz { get; set; }

      /// <summary>
      /// 排小便措施代码 ，(CP_DataCategoryDetail.lbbh=52)，5200-无
      /// </summary>
      [DataMember]
      public int Xbcsdm { get; set; }

      /// <summary>
      /// 排小便措施
      /// </summary>
      [DataMember]
      public string Xbcs { get; set; }

      /// <summary>
      /// 大便次数，格式：1*(3/2E ),'*'表示大便失禁 
      /// </summary>
      [DataMember]
      public string Dbcs { get; set; }

      /// <summary>
      /// 大便性状代码，(CP_DataCategoryDetail.lbbh=53)
      /// </summary>
      [DataMember]
      public int Dbxzdm { get; set; }

      /// <summary>
      /// 大便性状,格式:颜色_形状
      /// </summary>
      [DataMember]
      public string Dbxz { get; set; }

      /// <summary>
      /// 排大便措施代码，(CP_DataCategoryDetail.lbbh=54)，5500-无
      /// </summary>
      [DataMember]
      public int Pbcsdm { get; set; }

      /// <summary>
      /// 排大便措施，如：灌肠、人工肛门等 
      /// </summary>
      [DataMember]
      public string Pbcs { get; set; }

      /// <summary>
      /// 患者痰量
      /// </summary>
      [DataMember]
      public string Hztl { get; set; }

      /// <summary>
      /// 痰的性状代码，(CP_DataCategoryDetail.lbbh=55)
      /// </summary>
      [DataMember]
      public int Txzdm { get; set; }

      /// <summary>
      /// 痰的性状名称，格式：颜色_性状
      /// </summary>
      [DataMember]
      public string Txz { get; set; }

      /// <summary>
      /// 引流量
      /// </summary>
      [DataMember]
      public string Yll { get; set; }

      /// <summary>
      /// 引流说明
      /// </summary>
      [DataMember]
      public string Ylsm { get; set; }

      /// <summary>
      /// 其他出量1类型代码，(CP_DataCategoryDetail.lbbh=56)
      /// </summary>
      [DataMember]
      public int Qtcllxdm1 { get; set; }

      /// <summary>
      /// 其他出量1类型名称,格式:类型名称(单位)
      /// </summary>
      [DataMember]
      public string Qtcllx1 { get; set; }

      /// <summary>
      /// 其他出量1值
      /// </summary>
      [DataMember]
      public string Qtcl1 { get; set; }

      /// <summary>
      /// 其他出量2类型代码，(CP_DataCategoryDetail.lbbh=56)
      /// </summary>
      [DataMember]
      public int Qtcllxdm2 { get; set; }

      /// <summary>
      /// 其他出量2类型名称,格式:类型名称(单位)
      /// </summary>
      [DataMember]
      public string Qtcllx2 { get; set; }

      /// <summary>
      /// 其他出量2值
      /// </summary>
      [DataMember]
      public string Qtcl2 { get; set; }


      /// <summary>
      /// 登记日期(格式：2010-02-01 01:02:03) 
      /// </summary>
      [DataMember]
      public string Djrq { get; set; }

      /// <summary>
      /// 登记医生ID
      /// </summary>
      [DataMember]
      public string Djysdm { get; set; }

      /// <summary>
      /// 登记医生
      /// </summary>
      [DataMember]
      public string Djys { get; set; }

      /// <summary>
      /// 作废日期(格式：2010-02-01 01:02:03) 
      /// </summary>
      [DataMember]
      public string Zfrq { get; set; }

      /// <summary>
      /// 作废人员ID
      /// </summary>
      [DataMember]
      public string Zfrydm { get; set; }

      /// <summary>
      /// 作废人员	
      /// </summary>
      [DataMember]
      public string Zfry { get; set; }




      /// <summary>
      /// 所属路径结点
      /// </summary>
      [DataMember]
      public String ActivityId
      {
         get;
         set;
      }

      /// <summary>
      /// 所属路径结点的子结点
      /// </summary>
      [DataMember]
      public String ActivityChildId
      {
         get;
         set;
      }


      /// <summary>
      /// 所属路径代码
      /// </summary>
      [DataMember]
      public String Ljdm
      {
         get;
         set;
      }

      /// <summary>
      /// 路径序号
      /// </summary>
      [DataMember]
      public Decimal Ljxh
      {
         get;
         set;
      }
   }
}