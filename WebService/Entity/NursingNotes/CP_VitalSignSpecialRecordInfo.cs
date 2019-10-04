﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.NursingNotes
{
   /// <summary>
   /// 病人特殊护理记录         
   /// modify by xjt 20110217 之前类名与数据库里的表名一样，YidanEHRTableEntity.edmx一旦更新，会报错
   /// </summary>
   [DataContract]
   public class CP_VitalSignSpecialRecordInfo
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
      /// 患者身高
      /// </summary>
      [DataMember]
      public string Hzsg { get; set; }

      /// <summary>
      /// 患者体重
      /// </summary>
      [DataMember]
      public string Hztz { get; set; }

      /// <summary>
      /// 患者腹围
      /// </summary>
      [DataMember]
      public string Hzfw { get; set; }

      /// <summary>
      /// 患者血型代码，(CP_DataCategoryDetail.lbbh=57),
      /// 5701-A型，5702-B型，5703-O型，5704-AB型
      /// </summary>
      [DataMember]
      public int Hzxxdm { get; set; }

      /// <summary>
      /// 患者血型
      /// </summary>
      [DataMember]
      public string Hzxx { get; set; }

      /// <summary>
      /// 血性代码,(CP_DataCategoryDetail.lbbh=58),5801-HR阴性、5802-HR阳性
      /// </summary>
      [DataMember]
      public int Xyxxdm { get; set; }

      /// <summary>
      /// 血性：HR阴性、HR阳性
      /// </summary>
      [DataMember]
      public string Xyxx { get; set; }

      /// <summary>
      /// 患者手术史
      /// </summary>
      [DataMember]
      public string Hzsss { get; set; }

      /// <summary>
      /// 患者输血史
      /// </summary>
      [DataMember]
      public string Hzsxs { get; set; }

      /// <summary>
      /// 患者过敏史
      /// </summary>
      [DataMember]
      public string Hzgms { get; set; }

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