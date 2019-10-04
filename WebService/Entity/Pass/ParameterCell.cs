using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity.Pass
{
    #region 参数控件属性值实体
    /// <summary>
    /// 参数控件属性值实体
    /// </summary>
    [DataContract]
    public class ParameterCell
    {

        /// <summary>
        /// 构造函数初赋值
        /// </summary>
        /// <param name="strName">控件属性名称</param>
        /// <param name="strValue">控件属性值</param>
        public ParameterCell(String strName, String strValue)
        {
            Names = strName;
            Value = strValue;
        }

        /// <summary>
        /// 控件属性名称
        /// </summary>
        [DataMember]
        public String Names { get; set; }

        /// <summary>
        /// 控件属性值
        /// </summary>
        [DataMember]
        public String Value { get; set; }
    }
    #endregion
}
    
  
    