using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    
    /// <summary>
    /// 临床路径配置参数表
    /// 特有字段会标识出来，未特别说明为共用属性
    /// </summary>
    [DataContract()]
    public class APPCFG
    {

        #region property

        /// <summary>
        /// 配置参数主关键词
        /// </summary>
        [DataMember()]
        public string Configkey
        { get; set; }

        /// <summary>
        /// 参数词名
        /// </summary>
        [DataMember()]
        public string Name
        { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        [DataMember()]
        public string Value
        { get; set; }

        /// <summary>
        /// 参数值描述
        /// </summary>
        [DataMember()]
        public string Descript
        { get; set; }

        /// <summary>
        /// 参数值类型
        /// </summary>
        [DataMember()]
        public int ParamType
        { get; set; }

        /// <summary>
        /// 默认为空
        /// </summary>
        [DataMember()]
        public string Cfgkeyset
        { get; set; }

        /// <summary>
        /// 默认为空
        /// </summary>
        [DataMember()]
        public string Design
        { get; set; }

        /// <summary>
        /// 客户端状态 默认为0
        /// </summary>
        [DataMember()]
        public int ClientFlag
        { get; set; }

        /// <summary>
        /// 是否隐藏 默认为1
        /// </summary>
        [DataMember()]
        public int Hide
        { get; set; }

        /// <summary>
        /// 审核状态 默认为1
        /// </summary>
        [DataMember()]
        public int Valid
        { get; set; }

        #endregion Model

        public APPCFG()
        { }
         
        public APPCFG(string _configkey, string _name, string _value,
                              string _descript, int _paramtype, string _cfgkeyset,string _design, int _clientflag,int _hide, int _valid)
        {
            Configkey = _configkey;
            Name = _name;
            Value = _value;
            Descript = _descript;
            ParamType = _paramtype;
            Cfgkeyset = _cfgkeyset;
            Design = _design;
            ClientFlag = _clientflag;
            Hide = _hide;
            Valid = _valid;
 
        }
    }
}