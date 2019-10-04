using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.Runtime.Serialization;
using System.Data.Objects.DataClasses;

namespace Yidansoft.Service
{    
    public partial class CP_PathVariation : EntityObject
    {

        private string m_state;
        private string m_codegroup;

        #region Model
        /// <summary>
        /// 变异编码有效状态
        /// </summary>
        [DataMember]
        public string State
        {
            get { return m_state; }
            set { m_state = value; }
        }

        /// <summary>
        /// 编码和编码名称组合
        /// </summary>
        [DataMember]
        public string CodeGroup
        {
            get { return m_codegroup; }
            set { m_codegroup = value; }
        }

        
        #endregion
    }
}