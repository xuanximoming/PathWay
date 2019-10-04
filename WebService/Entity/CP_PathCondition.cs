using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Objects.DataClasses;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    //[EdmEntityTypeAttribute(NamespaceName = "YidanEHRTableModel", Name = "CP_PathCondition")]
    //[Serializable()]
    //[DataContractAttribute(IsReference = true)]
    //public partial class CP_PathCondition:EntityObject
    //{
    //    #region 工厂方法

    //    /// <summary>
    //    /// 创建新的 CP_PathCondition 对象。
    //    /// </summary>
    //    /// <param name="tjdm">Tjdm 属性的初始值。</param>
    //    /// <param name="ljdm">Ljdm 属性的初始值。</param>
    //    /// <param name="tjmc">Tjmc 属性的初始值。</param>
    //    /// <param name="tJlb">TJlb 属性的初始值。</param>
    //    /// <param name="yxjl">Yxjl 属性的初始值。</param>
    //    public static CP_PathCondition CreateCP_PathCondition(global::System.String tjdm, global::System.String ljdm, global::System.String tjmc, global::System.Int32 tJlb, global::System.Int32 yxjl,global::System.String condition)
    //    {
    //        CP_PathCondition cP_PathCondition = new CP_PathCondition();
    //        cP_PathCondition.Tjdm = tjdm;
    //        cP_PathCondition.Ljdm = ljdm;
    //        cP_PathCondition.Tjmc = tjmc;
    //        cP_PathCondition.TJlb = tJlb;
    //        cP_PathCondition.Yxjl = yxjl;
    //        cP_PathCondition.Condition = condition;
    //        return cP_PathCondition;
    //    }

    //    #endregion
    //    #region 基元属性

    //    /// <summary>
    //    /// 没有元数据文档可用。
    //    /// </summary>
    //    [EdmScalarPropertyAttribute(EntityKeyProperty = true, IsNullable = false)]
    //    [DataMemberAttribute()]
    //    public global::System.String Tjdm
    //    {
    //        get
    //        {
    //            return _Tjdm;
    //        }
    //        set
    //        {
    //            if (_Tjdm != value)
    //            {
    //                OnTjdmChanging(value);
    //                ReportPropertyChanging("Tjdm");
    //                _Tjdm = StructuralObject.SetValidValue(value, false);
    //                ReportPropertyChanged("Tjdm");
    //                OnTjdmChanged();
    //            }
    //        }
    //    }
    //    private global::System.String _Tjdm;
    //    partial void OnTjdmChanging(global::System.String value);
    //    partial void OnTjdmChanged();

    //    /// <summary>
    //    /// 没有元数据文档可用。
    //    /// </summary>
    //    [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = false)]
    //    [DataMemberAttribute()]
    //    public global::System.String Ljdm
    //    {
    //        get
    //        {
    //            return _Ljdm;
    //        }
    //        set
    //        {
    //            OnLjdmChanging(value);
    //            ReportPropertyChanging("Ljdm");
    //            _Ljdm = StructuralObject.SetValidValue(value, false);
    //            ReportPropertyChanged("Ljdm");
    //            OnLjdmChanged();
    //        }
    //    }
    //    private global::System.String _Ljdm;
    //    partial void OnLjdmChanging(global::System.String value);
    //    partial void OnLjdmChanged();

    //    /// <summary>
    //    /// 没有元数据文档可用。
    //    /// </summary>
    //    [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = false)]
    //    [DataMemberAttribute()]
    //    public global::System.String Tjmc
    //    {
    //        get
    //        {
    //            return _Tjmc;
    //        }
    //        set
    //        {
    //            OnTjmcChanging(value);
    //            ReportPropertyChanging("Tjmc");
    //            _Tjmc = StructuralObject.SetValidValue(value, false);
    //            ReportPropertyChanged("Tjmc");
    //            OnTjmcChanged();
    //        }
    //    }
    //    private global::System.String _Tjmc;
    //    partial void OnTjmcChanging(global::System.String value);
    //    partial void OnTjmcChanged();

    //    /// <summary>
    //    /// 没有元数据文档可用。
    //    /// </summary>
    //    [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = false)]
    //    [DataMemberAttribute()]
    //    public global::System.Int32 TJlb
    //    {
    //        get
    //        {
    //            return _TJlb;
    //        }
    //        set
    //        {
    //            OnTJlbChanging(value);
    //            ReportPropertyChanging("TJlb");
    //            _TJlb = StructuralObject.SetValidValue(value);
    //            ReportPropertyChanged("TJlb");
    //            OnTJlbChanged();
    //        }
    //    }
    //    private global::System.Int32 _TJlb;
    //    partial void OnTJlbChanging(global::System.Int32 value);
    //    partial void OnTJlbChanged();

    //    /// <summary>
    //    /// 没有元数据文档可用。
    //    /// </summary>
    //    [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = false)]
    //    [DataMemberAttribute()]
    //    public global::System.Int32 Yxjl
    //    {
    //        get
    //        {
    //            return _Yxjl;
    //        }
    //        set
    //        {
    //            OnYxjlChanging(value);
    //            ReportPropertyChanging("Yxjl");
    //            _Yxjl = StructuralObject.SetValidValue(value);
    //            ReportPropertyChanged("Yxjl");
    //            OnYxjlChanged();
    //        }
    //    }
    //    private global::System.Int32 _Yxjl;
    //    partial void OnYxjlChanging(global::System.Int32 value);
    //    partial void OnYxjlChanged();

    //    /// <summary>
    //    /// 没有元数据文档可用。
    //    /// </summary>
    //    [EdmScalarPropertyAttribute(EntityKeyProperty = false, IsNullable = false)]
    //    [DataMemberAttribute()]
    //    public global::System.String Condition
    //    {
    //        get
    //        {
    //            return _condition ;
    //        }
    //        set
    //        {
    //            OnConditionChanging(value);
    //            ReportPropertyChanging("Condition");
    //            _condition = StructuralObject.SetValidValue(value,false);
    //            ReportPropertyChanged("Condition");
    //            OnConditionChanged();
    //        }
    //    }
    //    private global::System.String _condition;
    //    partial void OnConditionChanging(global::System.String value);
    //    partial void OnConditionChanged();

    //    #endregion
    //}
}