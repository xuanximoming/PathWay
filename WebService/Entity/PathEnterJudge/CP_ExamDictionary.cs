using System;
using System.IO;
using System.Runtime.Serialization;
namespace Yidansoft.Service.Entity.Class
{
    [DataContract()]
    /// <summary>
    /// ��������
    /// </summary>
    public class CP_ExamDictionary : System.Object
    {

        public String Jlxh { get; set; }//�Զ�����
        public String Jcbm { get; set; }    //�����Ŀ����
        public String Fjd { get; set; }   //���ڵ����(Jcbm)
        public String Jcmc { get; set; }//�����Ŀ��������
        public String Mcsx { get; set; }    //������д����
        public String Py { get; set; }      //ƴ��
        public String Wb { get; set; }      //���
        public String Bz { get; set; }  //��ע
    }
}
