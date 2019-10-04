using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Linq;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    //[DataContract()]
    //public class CP_CopyWorkFlow
    //{
    //}

    //[DataContract()]
    //public partial class CopyWorkFlowRule
    //{
    //    private string m_UniqueID;
    //    /// <summary>
    //    /// id
    //    /// </summary>
    //    [DataMember()]
    //    public string UniqueID
    //    {
    //        get
    //        {
    //            return m_UniqueID;
    //        }
    //        set
    //        {
    //            this.m_UniqueID = value;
    //        }
    //    }

    //    private string m_NewUniqueID;
    //    /// <summary>
    //    /// id
    //    /// </summary>
    //    [DataMember()]
    //    public string NewUniqueID
    //    {
    //        get
    //        {
    //            return m_NewUniqueID;
    //        }
    //        set
    //        {
    //            this.m_NewUniqueID = value;
    //        }
    //    }

    //    private string m_RuleID;
    //    /// <summary>
    //    /// RuleID
    //    /// </summary>
    //    [DataMember()]
    //    public string RuleID
    //    {
    //        get
    //        {
    //            return m_RuleID;
    //        }
    //        set
    //        {
    //            this.m_RuleID = value;
    //        }
    //    }

    //    private string m_NewRuleID;
    //    /// <summary>
    //    /// RuleID
    //    /// </summary>
    //    [DataMember()]
    //    public string NewRuleID
    //    {
    //        get
    //        {
    //            return m_NewRuleID;
    //        }
    //        set
    //        {
    //            this.m_NewRuleID = value;
    //        }
    //    }

    //    private string m_LineType;
    //    /// <summary>
    //    /// LineType
    //    /// </summary>
    //    [DataMember()]
    //    public string LineType
    //    {
    //        get
    //        {
    //            return m_LineType;
    //        }
    //        set
    //        {
    //            this.m_LineType = value;
    //        }
    //    }

    //    private string m_RuleName;
    //    /// <summary>
    //    /// RuleName
    //    /// </summary>
    //    [DataMember()]
    //    public string RuleName
    //    {
    //        get
    //        {
    //            return m_RuleName;
    //        }
    //        set
    //        {
    //            this.m_RuleName = value;
    //        }
    //    }

    //    private string m_BeginActivityId;
    //    /// <summary>
    //    /// 前驱活动
    //    /// </summary>
    //    [DataMember()]
    //    public string BeginActivityId
    //    {
    //        get
    //        {
    //            return m_BeginActivityId;
    //        }
    //        set
    //        {
    //            this.m_BeginActivityId = value;
    //        }
    //    }

    //    private string m_NewBeginActivityId;
    //    /// <summary>
    //    /// 前驱活动
    //    /// </summary>
    //    [DataMember()]
    //    public string NewBeginActivityId
    //    {
    //        get
    //        {
    //            return m_NewBeginActivityId;
    //        }
    //        set
    //        {
    //            this.m_NewBeginActivityId = value;
    //        }
    //    }

    //    private string m_EndActivityId;
    //    /// <summary>
    //    /// 后续活动
    //    /// </summary>
    //    [DataMember()]
    //    public string EndActivityId
    //    {
    //        get
    //        {
    //            return m_EndActivityId;
    //        }
    //        set
    //        {
    //            this.m_EndActivityId = value;
    //        }
    //    }

    //    private string m_NewEndActivityId;
    //    /// <summary>
    //    /// 后续活动
    //    /// </summary>
    //    [DataMember()]
    //    public string NewEndActivityId
    //    {
    //        get
    //        {
    //            return m_NewEndActivityId;
    //        }
    //        set
    //        {
    //            this.m_NewEndActivityId = value;
    //        }
    //    }

    //    private string m_BeginPointX;
    //    /// <summary>
    //    /// BeginPointX
    //    /// </summary>
    //    [DataMember()]
    //    public string BeginPointX
    //    {
    //        get
    //        {
    //            return m_BeginPointX;
    //        }
    //        set
    //        {
    //            this.m_BeginPointX = value;
    //        }
    //    }

    //    private string m_BeginPointY;
    //    /// <summary>
    //    /// BeginPointY
    //    /// </summary>
    //    [DataMember()]
    //    public string BeginPointY
    //    {
    //        get
    //        {
    //            return m_BeginPointY;
    //        }
    //        set
    //        {
    //            this.m_BeginPointY = value;
    //        }
    //    }

    //    private string m_EndPointX;
    //    /// <summary>
    //    /// EndPointX
    //    /// </summary>
    //    [DataMember()]
    //    public string EndPointX
    //    {
    //        get
    //        {
    //            return m_EndPointX;
    //        }
    //        set
    //        {
    //            this.m_EndPointX = value;
    //        }
    //    }

    //    private string m_EndPointY;
    //    /// <summary>
    //    /// EndPointY
    //    /// </summary>
    //    [DataMember()]
    //    public string EndPointY
    //    {
    //        get
    //        {
    //            return m_EndPointY;
    //        }
    //        set
    //        {
    //            this.m_EndPointY = value;
    //        }
    //    }

    //    private string m_ZIndex;
    //    /// <summary>
    //    /// ZIndex="20"
    //    /// </summary>
    //    [DataMember()]
    //    public string ZIndex
    //    {
    //        get
    //        {
    //            return m_ZIndex;
    //        }
    //        set
    //        {
    //            this.m_ZIndex = value;
    //        }
    //    }

    //    private List<CopyWorkFlowRule> items;
    //    [DataMember()]
    //    public List<CopyWorkFlowRule> Items
    //    {
    //        get
    //        {
    //            return this.items;
    //        }
    //        private set
    //        {
    //            this.items = value;
    //        }
    //    }

    //    public CopyWorkFlowRule()
    //    { }
    //}

    //[DataContract()]
    //public partial class CopyWorkFlowActivity
    //{
    //    #region 路径基本信息
    //    private string m_enforcetime;
    //    /// <summary>
    //    /// 路径执行时间  (hjh添加)
    //    /// </summary>
    //    [DataMember()]
    //    public string EnForceTime
    //    {
    //        get { return m_enforcetime; }
    //        set { m_enforcetime = value; }
    //    }

    //    private string m_Ljdm;
    //    /// <summary>
    //    /// Ljdm,路径基本信息
    //    /// </summary>
    //    [DataMember()]
    //    public string Ljdm
    //    {
    //        get
    //        {
    //            return m_Ljdm;
    //        }
    //        set
    //        {
    //            this.m_Ljdm = value;
    //        }
    //    }

    //    private string m_NewLjdm;
    //    /// <summary>
    //    /// Ljdm,路径基本信息
    //    /// </summary>
    //    [DataMember()]
    //    public string NewLjdm
    //    {
    //        get
    //        {
    //            return m_NewLjdm;
    //        }
    //        set
    //        {
    //            this.m_NewLjdm = value;
    //        }
    //    }

    //    private string m_LjID;
    //    /// <summary>
    //    /// ID，路径基本信息
    //    /// </summary>
    //    [DataMember()]
    //    public string LjID
    //    {
    //        get
    //        {
    //            return m_LjID;
    //        }
    //        set
    //        {
    //            this.m_LjID = value;
    //        }
    //    }

    //    private string m_NewLjID;
    //    /// <summary>
    //    /// ID，路径基本信息
    //    /// </summary>
    //    [DataMember()]
    //    public string NewLjID
    //    {
    //        get
    //        {
    //            return m_NewLjID;
    //        }
    //        set
    //        {
    //            this.m_NewLjID = value;
    //        }
    //    }

    //    private string m_LjName;
    //    /// <summary>
    //    /// name，路径基本信息
    //    /// </summary>
    //    [DataMember()]
    //    public string LjName
    //    {
    //        get
    //        {
    //            return m_LjName;
    //        }
    //        set
    //        {
    //            this.m_LjName = value;
    //        }
    //    }

    //    private string m_NewLjName;
    //    /// <summary>
    //    /// name，路径基本信息
    //    /// </summary>
    //    [DataMember()]
    //    public string NewLjName
    //    {
    //        get
    //        {
    //            return m_NewLjName;
    //        }
    //        set
    //        {
    //            this.m_NewLjName = value;
    //        }
    //    }

    //    private string m_LjDescription;
    //    /// <summary>
    //    ///  Description，路径基本信息
    //    /// </summary>
    //    [DataMember()]
    //    public string LjDescription
    //    {
    //        get
    //        {
    //            return m_LjDescription;
    //        }
    //        set
    //        {
    //            this.m_LjDescription = value;
    //        }
    //    }

    //    private string m_LjWidth;
    //    /// <summary>
    //    ///  Width，路径基本信息
    //    /// </summary>
    //    [DataMember()]
    //    public string LjWidth
    //    {
    //        get
    //        {
    //            return m_LjWidth;
    //        }
    //        set
    //        {
    //            this.m_LjWidth = value;
    //        }
    //    }

    //    private string m_LjHeight;
    //    /// <summary>
    //    ///  Height，路径基本信息
    //    /// </summary>
    //    [DataMember()]
    //    public string LjHeight
    //    {
    //        get
    //        {
    //            return m_LjHeight;
    //        }
    //        set
    //        {
    //            this.m_LjHeight = value;
    //        }
    //    }
    //    #endregion

    //    private string m_UniqueID;
    //    /// <summary>
    //    /// id
    //    /// </summary>
    //    [DataMember()]
    //    public string UniqueID
    //    {
    //        get
    //        {
    //            return m_UniqueID;
    //        }
    //        set
    //        {
    //            this.m_UniqueID = value;
    //        }
    //    }

    //    private string m_NewUniqueID;
    //    /// <summary>
    //    /// id
    //    /// </summary>
    //    [DataMember()]
    //    public string NewUniqueID
    //    {
    //        get
    //        {
    //            return m_NewUniqueID;
    //        }
    //        set
    //        {
    //            this.m_NewUniqueID = value;
    //        }
    //    }

    //    private string m_ActivityId;
    //    /// <summary>
    //    /// id
    //    /// </summary>
    //    [DataMember()]
    //    public string ActivityId
    //    {
    //        get
    //        {
    //            return m_ActivityId;
    //        }
    //        set
    //        {
    //            this.m_ActivityId = value;
    //        }
    //    }

    //    private string m_ActivityName;
    //    /// <summary>
    //    /// name
    //    /// </summary>
    //    [DataMember()]
    //    public string ActivityName
    //    {
    //        get
    //        {
    //            return m_ActivityName;
    //        }
    //        set
    //        {
    //            this.m_ActivityName = value;
    //        }
    //    }

    //    private string m_Type;
    //    /// <summary>
    //    /// type
    //    /// </summary>
    //    [DataMember()]
    //    public string Type
    //    {
    //        get
    //        {
    //            return m_Type;
    //        }
    //        set
    //        {
    //            this.m_Type = value;
    //        }
    //    }

    //    private string m_SubFlow;
    //    /// <summary>
    //    /// SubFlow
    //    /// </summary>
    //    [DataMember()]
    //    public string SubFlow
    //    {
    //        get
    //        {
    //            return m_SubFlow;
    //        }
    //        set
    //        {
    //            this.m_SubFlow = value;
    //        }
    //    }


    //    private string m_PositionX;
    //    /// <summary>
    //    /// 位置X
    //    /// </summary>
    //    [DataMember()]
    //    public string PositionX
    //    {
    //        get
    //        {
    //            return m_PositionX;
    //        }
    //        set
    //        {
    //            this.m_PositionX = value;
    //        }
    //    }

    //    private string m_PositionY;
    //    /// <summary>
    //    /// 位置Y
    //    /// </summary>
    //    [DataMember()]
    //    public string PositionY
    //    {
    //        get
    //        {
    //            return m_PositionY;
    //        }
    //        set
    //        {
    //            this.m_PositionY = value;
    //        }
    //    }

    //    private string m_RepeatDirection;
    //    /// <summary>
    //    /// DIRECTION
    //    /// </summary>
    //    [DataMember()]
    //    public string RepeatDirection
    //    {
    //        get
    //        {
    //            return m_RepeatDirection;
    //        }
    //        set
    //        {
    //            this.m_RepeatDirection = value;
    //        }
    //    }

    //    private string m_ZIndex;
    //    /// <summary>
    //    /// ZIndex
    //    /// </summary>
    //    [DataMember()]
    //    public string ZIndex
    //    {
    //        get
    //        {
    //            return m_ZIndex;
    //        }
    //        set
    //        {
    //            this.m_ZIndex = value;
    //        }
    //    }



    //    public CopyWorkFlowActivity()
    //    { }
    //}
}