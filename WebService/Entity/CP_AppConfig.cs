using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract()]
    public class CP_AppConfig
    {
        private string m_Configkey;
        /// <summary>
        /// 设置键,唯一
        /// </summary>
        [DataMember()]
        public string Configkey
        {
            get { return m_Configkey; }
            set { m_Configkey = value; }
        }

        private string m_Name;
        /// <summary>
        /// 设置的名称
        /// </summary>
        [DataMember()]
        public string Name
        {
            get { return m_Name; }
            set { m_Name = value; }
        }

        private string m_ConfigValue;
        /// <summary>
        /// 设置值
        /// </summary>
        [DataMember()]
        public string ConfigValue
        {
            get { return m_ConfigValue; }
            set { m_ConfigValue = value; }
        }

        public CP_AppConfig()
        {

        }

        public CP_AppConfig(string strConfigKey, string strName, string strConfigValue)
        {
            Configkey = strConfigKey;
            Name = strName;
            ConfigValue = strConfigValue;
        }
    }
}