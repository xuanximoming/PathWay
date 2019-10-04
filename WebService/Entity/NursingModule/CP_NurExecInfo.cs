using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 护理执行项目信息实体
    /// </summary>  
    [DataContract]
    public partial class CP_NurExecInfo
    {
        /// <summary>
        /// 护理执行编号
        /// </summary>
        [DataMember]
        public String Id { get; set; }
        private String m_Xmxh;
        /// <summary>
        /// 项目编码,CP_NurExecItem. Xmxh
        /// </summary>
        [DataMember]
        public String Xmxh
        {
            get { return m_Xmxh; }
            set { m_Xmxh = value; }
        }
        private String m_XmxhName;
        /// <summary>
        /// 项目名称
        /// </summary>
        [DataMember]
        public String XmxhName
        {
            get { return m_XmxhName; }
            set { m_XmxhName = value; }
        }

        private String m_Lbxh;
        /// <summary>
        /// 类别编码,CP_NurExecCategory.Lbxh
        /// </summary>
        [DataMember]
        public String Lbxh
        {
            get { return m_Lbxh; }
            set { m_Lbxh = value; }
        }

        private String m_LbxhName;
        /// <summary>
        /// 类别名称
        /// </summary>
        [DataMember]
        public String LbxhName
        {
            get { return m_LbxhName; }
            set { m_LbxhName = value; }
        }
        /// <summary>
        /// 护理结果集合
        /// </summary>
        [DataMember]
        public List<CP_NurExecuteResult> ResultList { get; set; }

        /// <summary>
        /// 护理执行结果集合
        /// </summary>
        [DataMember]
        public List<CP_NurExecRecordResult> RecordResultList { get; set; }

        private Decimal m_LbOrderValue;
        /// <summary>
        /// 类别排序
        /// </summary>
        [DataMember]
        public Decimal LbOrderValue
        {
            get { return m_LbOrderValue; }
            set { m_LbOrderValue = value; }
        }

        private Boolean m_IsUserControl = false;
        /// <summary>
        /// 是否用户控件
        /// </summary>
        [DataMember]
        public Boolean IsUserControl
        {
            get { return m_IsUserControl; }
            set { m_IsUserControl = value; }
        }

        private String m_Mxxh;
        /// <summary>
        /// 项目明细序号
        /// </summary>
        [DataMember]
        public String Mxxh
        {
            get { return m_Mxxh; }
            set { m_Mxxh = value; }
        }

        private String m_MxxhName;
        /// <summary>
        /// 项目明细Name
        /// </summary>
        [DataMember]
        public String MxxhName
        {
            get { return m_MxxhName; }
            set { m_MxxhName = value; }
        }

        private Decimal m_InputType;
        /// <summary>
        /// 输入控件类型（0无,1TEXTBOX,2COMBBOX…）暂时保留
        /// </summary>
        [DataMember]
        public Decimal InputType
        {
            get { return m_InputType; }
            set { m_InputType = value; }
        }

        private Decimal m_MxOrderValue;
        /// <summary>
        /// 排序字段
        /// </summary>
        [DataMember]
        public Decimal MxOrderValue
        {
            get { return m_MxOrderValue; }
            set { m_MxOrderValue = value; }
        }

        private String m_CreateTime;
        /// <summary>
        /// 时间
        /// </summary>
        [DataMember]
        public String CreateTime
        {
            get { return m_CreateTime; }
            set { m_CreateTime = value; }
        }

        private String m_CreateUser;
        /// <summary>
        /// 创建者
        /// </summary>
        [DataMember]
        public String CreateUser
        {
            get { return m_CreateUser; }
            set { m_CreateUser = value; }
        }

        private Boolean m_IsEdiable = false;
        /// <summary>
        /// (是否编辑状态，默认FALSE),在IsEdiable为TRUE的情况下，才有可能展示输入控件
        /// </summary>
        [DataMember]
        public Boolean IsEdiable
        {
            get { return m_IsEdiable; }
            set { m_IsEdiable = value; }
        }

        private Boolean m_IsSelected = false;
        /// <summary>
        /// 是否选中，双向绑定
        /// </summary>
        [DataMember]
        public Boolean IsSelected
        {
            get { return m_IsSelected; }
            set { m_IsSelected = value; }
        }

        private Boolean m_IsModify = false;
        /// <summary>
        /// 是否修改过
        /// </summary>
        [DataMember]
        public Boolean IsModify
        {
            get { return m_IsModify; }
            set { m_IsModify = value; }
        }

        private Boolean m_IsNew = true;
        /// <summary>
        /// 是否新增
        /// </summary>
        [DataMember]
        public Boolean IsNew
        {
            get { return m_IsNew; }
            set { m_IsNew = value; }
        }

        private Decimal m_ToPathId;
        /// <summary>
        /// CP_NurExecToPath.Id
        /// </summary>
        [DataMember]
        public Decimal ToPathId
        {
            get { return m_ToPathId; }
            set { m_ToPathId = value; }
        }

        private String m_PathDetailId;
        /// <summary>
        /// 所属路径结点
        /// </summary>
        [DataMember]
        public String PathDetailId
        {
            get { return m_PathDetailId; }
            set { m_PathDetailId = value; }
        }

        private String m_Ljdm;
        /// <summary>
        /// 所属路径代码
        /// </summary>
        [DataMember]
        public String Ljdm
        {
            get { return m_Ljdm; }
            set { m_Ljdm = value; }
        }



        private String m_NurRecordId;
        /// <summary>
        /// CP_NurExecRecord.Id
        /// </summary>
        [DataMember]
        public String NurRecordId
        {
            get { return m_NurRecordId; }
            set { m_NurRecordId = value; }
        }

        private Decimal m_Ljxh;
        /// <summary>
        /// 路径序号
        /// </summary>
        [DataMember]
        public Decimal Ljxh
        {
            get { return m_Ljxh; }
            set { m_Ljxh = value; }
        }
    }
}