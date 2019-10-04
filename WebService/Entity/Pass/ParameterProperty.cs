using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.Pass
{
    #region 单个参数控件属性实体
    /// <summary>
    /// 单个参数控件属性实体
    /// </summary>
    [DataContract]
    public class ParameterProperty
    {
        /// <summary>
        /// 保存单个参数控件属性集
        /// </summary>
        [DataMember]
        private List<ParameterCell> m_Property = new List<ParameterCell>();

        /// <summary>
        /// 参数提示标签
        /// </summary>
        [DataMember]
        public String LabelText { get; set; }

        /// <summary>
        /// 单个参数控件tag属性值，用于后台处理公式值替换
        /// </summary>
        [DataMember]
        public String Tag { get; set; }

        /// <summary>
        /// 单个参数控件属性集
        /// </summary>
        [DataMember]
        public List<ParameterCell> Property
        {
            get
            {
                return m_Property;
            }
            set
            {
                m_Property = value;
            }
        }

        /// <summary>
        /// 添加属性
        /// </summary>
        /// <param name="strName">控件属性名称</param>
        /// <param name="strValue">控件属性值</param>       
        public void AddProperty(String strName, String strValue)
        {
            m_Property.Add(new ParameterCell(strName, strValue));
        }
    }
    #endregion
}