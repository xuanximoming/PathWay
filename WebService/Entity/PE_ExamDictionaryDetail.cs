using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 表示检查项目表的实体
    /// </summary>
    [DataContract]
    public class PE_ExamDictionaryDetail
    {
        [DataMember()]
        public String Jcbm { get; set; }//检查项目编码(导入时映射)
        [DataMember()]
        public String Flbm { get; set; }//分类编码(CP_ExamDictionary.Jcbm)
        [DataMember()]
        public String Flmc { get; set; }//分类名称
        [DataMember()]
        public String Jcmc { get; set; }//检查项目名称
        [DataMember()]
        public String Mcsx { get; set; }//名称缩写符号
        [DataMember()]
        public String Ksfw { get; set; }//开始范围（正常范围）
        [DataMember()]
        public String Jsfw { get; set; }//结束范围（正常范围）
        [DataMember()]
        public String Jsdw { get; set; }//单位 
        [DataMember()]
        public String Py { get; set; }//拼音
        [DataMember()]
        public String Wb { get; set; }//五笔
        [DataMember()]
        public String Yxjl { get; set; }// 有效记录
        [DataMember()]
        public String Bz { get; set; }//备注
    }
}