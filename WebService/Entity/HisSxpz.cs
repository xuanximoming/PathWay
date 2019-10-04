using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 配置his参数与临床路径匹配
    /// 特有字段会标识出来，未特别说明为共用属性
    /// </summary>
    [DataContract()]
    public class HisSxpz
    {
        #region property
        /// <summary>
        ///  主编号ID
        /// </summary> 
        [DataMember()]
        public int ID
        { get; set; }

        /// <summary>
        ///  临床路径接口字段
        /// </summary> 
        [DataMember()]
        public string EhrKey
        { get; set; }

        /// <summary>
        ///  临床路径接口字段描述
        /// </summary> 
        [DataMember()]
        public string Ehr_Keyms
        { get; set; }

        /// <summary>
        ///  His接口字段 
        /// </summary> 
        [DataMember()]
        public string HisKey
        { get; set; }

        /// <summary>
        ///  His接口字段描述
        /// </summary> 
        [DataMember()]
        public string His_Keyms
        { get; set; }

        /// <summary>
        ///  EHR接口字段来源描述
        /// </summary> 
        [DataMember()]
        public string EhrSource
        { get; set; }

         
        #endregion property

        public HisSxpz()
        { }

        public HisSxpz(int id, string sEhrKey, string sEhr_Keyms,
                              string sHisKey, string sHisKeyms, string sEhrSource)
        {
            ID = id;
            EhrKey = sEhrKey;
            Ehr_Keyms = sEhr_Keyms;
            HisKey = sHisKey;
            His_Keyms = sHisKeyms;
            EhrSource = sEhrSource;
 
        }
    }
}