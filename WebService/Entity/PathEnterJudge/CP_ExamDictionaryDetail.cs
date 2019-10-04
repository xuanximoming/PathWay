using System;
using System.IO;
using System.Runtime.Serialization;
namespace Yidansoft.Service.Entity
{
    [DataContract()]
    /// <summary>
    /// 检查项
    /// </summary>
    public partial class CP_ExamDictionaryDetail : System.Object
    {
        [DataMember()]
        public String Jlxh { get; set; }//自动生成
        [DataMember()]
        public String Jcbm { get; set; }  //检查项目编码(导入时映射)
        [DataMember()]
        public String Flbm { get; set; }  //分类编码(CP_ExamDictionary.Jcbm)
        [DataMember()]
        public String Jcmc { get; set; }//检查项目名称
        [DataMember()]
        public String Mcsx { get; set; }    //名称缩写符号
        [DataMember()]
        public String Ksfw { get; set; }//开始范围（正常范围）
        [DataMember()]
        public String Jsfw { get; set; }//结束范围（正常范围）
        [DataMember()]
        public String Syrq { get; set; }//适用人群（CP_PathEnterJudgeCondition.ID人，成人，男人，女人，婴儿等）
        [DataMember()]
        public String Jsdw { get; set; }//单位           
        [DataMember()]
        public String Py { get; set; }          //拼音
        [DataMember()]
        public String Wb { get; set; }          //五笔
        [DataMember()]
        public String Yxjl { get; set; }  //有效记录
        [DataMember()]
        public String Bz { get; set; }  //备注
        SuitCrowsMapScopes _SuitCrowsMapScopes = new SuitCrowsMapScopes();
        
        ///// <summary>
        ///// 适用人群对应的检查项范围
        ///// </summary>
        [DataMember()]
        public SuitCrowsMapScopes SuitCrowsMapScopes
        {
            get
            {

                _SuitCrowsMapScopes = new SuitCrowsMapScopes();
                String[] KsfwTemp;
                String[] JsfwTemp;
                String[] SyrqTemp;
                if (Ksfw != null && Ksfw.Trim() != "" && Jsfw != null && Jsfw.Trim() != "" && Syrq != null && Syrq.Trim() != "")
                {
                    KsfwTemp = Ksfw.Split(',');
                    JsfwTemp = Jsfw.Split(',');
                    SyrqTemp = Syrq.Split(',');
                    for (int i = 0; i < SyrqTemp.Length; i++)
                    {
                        _SuitCrowsMapScopes.Add(new SuitCrowdMapScope(KsfwTemp[i], JsfwTemp[i], SyrqTemp[i]));
                    }
                }
                return _SuitCrowsMapScopes;
            }
            set { _SuitCrowsMapScopes = value; }
        }
       
    }
}
