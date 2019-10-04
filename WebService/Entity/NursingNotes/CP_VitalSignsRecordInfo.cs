using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.NursingNotes
{
   /// <summary>
   /// 病人生命体征实体
   /// modify by xjt 20110217 之前类名与数据库里的表名一样，YidanEHRTableEntity.edmx一旦更新，会报错
   /// </summary>
   [DataContract]
   public class CP_VitalSignsRecordInfo
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
      /// 测量日期期（格式2010-01-01）
      /// </summary>
      [DataMember]
      public string Clrq { get; set; }

      /// <summary>
      /// 测量时间（格式：01:02:03）
      /// </summary>
      [DataMember]
      public string Clsj { get; set; }

      /// <summary>
      /// 测量时间段
      /// </summary>
      [DataMember]
      public string Sjd { get; set; }

      /// <summary>
      /// 患者状态代码，
      /// </summary>
      [DataMember]
      public int Hzztdm { get; set; }

      /// <summary>
      /// 患者状态（五种）
      /// </summary>
      [DataMember]
      public string Hzzt { get; set; }

      /// <summary>
      /// 患者体温
      /// </summary>
      [DataMember]
      public string Hztw { get; set; }

      /// <summary>
      /// 体温测量方式代码(CP_DataCategoryDetail.lbbh=48)
      /// 4801-液温、4801-口温、4802-肛温
      /// </summary>
      [DataMember]
      public int Clfsdm { get; set; }

      /// <summary>
      /// 体温测量方式名称
      /// </summary>
      [DataMember]
      public string Clfs { get; set; }

      /// <summary>
      /// 体温测量辅助措施代码，(CP_DataCategoryDetail.lbbh=49)  4900-无
      /// </summary>
      [DataMember]
      public int Fzcsdm { get; set; }

      /// <summary>
      /// 体温测量辅助措施
      /// </summary>
      [DataMember]
      public string Fzcs { get; set; }

      /// <summary>
      /// 脉搏 
      /// </summary>
      [DataMember]
      public string Hzmb { get; set; }

      /// <summary>
      /// 心率
      /// </summary>
      [DataMember]
      public string Hzxl { get; set; }

      /// <summary>
      /// 起搏器 0-无，1-使用
      /// </summary>
      [DataMember]
      public int Qbq { get; set; }

      /// <summary>
      /// 患者呼吸
      /// </summary>
      [DataMember]
      public string Hzhx { get; set; }

      /// <summary>
      /// 呼吸器 0-无，1-使用
      /// </summary>
      [DataMember]
      public int Hxq { get; set; }

      /// <summary>
      /// 患者血压
      /// </summary>
      [DataMember]
      public string Hzxy { get; set; }

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