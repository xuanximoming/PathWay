using System;
using System.IO;
using System.Runtime.Serialization;
namespace Yidansoft.Service.Entity.Class
{
    [DataContract()]
    /// <summary>
    /// 检查项分类
    /// </summary>
    public class CP_ExamDictionary : System.Object
    {

        public String Jlxh { get; set; }//自动生成
        public String Jcbm { get; set; }    //检查项目编码
        public String Fjd { get; set; }   //父节点编码(Jcbm)
        public String Jcmc { get; set; }//检查项目编码名称
        public String Mcsx { get; set; }    //名称缩写符号
        public String Py { get; set; }      //拼音
        public String Wb { get; set; }      //五笔
        public String Bz { get; set; }  //备注
    }
}
