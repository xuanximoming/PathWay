using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service
{
   /// <summary>
   /// 路径执行还
   /// </summary> 
   public partial class CP_VariationToPathInfo : CP_PathVariation
   {
      /// <summary>
      /// 是否新增
      /// </summary>
      [DataMember()]
      public Boolean IsNew
      { get; set; }

      /// <summary>
      /// 是否修改
      /// </summary>
      [DataMember()]
      public Boolean IsModify
      { get; set; }

      /// <summary>
      /// 是否选中
      /// </summary>
      [DataMember()]
      public Boolean IsSelected
      { get; set; }

      /// <summary>
      /// 与路径对应Id
      /// </summary>
      [DataMember()]
      public Decimal ToPathId
      { get; set; }

      /// <summary>
      /// 路径Id
      /// </summary>
      [DataMember()]
      public String ActivityId
      { get; set; }

      /// <summary>
      /// 路径Id
      /// </summary>
      [DataMember()]
      public String Ljdm
      { get; set; }
   }
}