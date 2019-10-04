using System;
using System.IO;
using System.Runtime.Serialization;
namespace Yidansoft.Service.Entity
{
    [DataContract()]
    /// <summary>
    /// �����
    /// </summary>
    public partial class CP_ExamDictionaryDetail : System.Object
    {
        [DataMember()]
        public String Jlxh { get; set; }//�Զ�����
        [DataMember()]
        public String Jcbm { get; set; }  //�����Ŀ����(����ʱӳ��)
        [DataMember()]
        public String Flbm { get; set; }  //�������(CP_ExamDictionary.Jcbm)
        [DataMember()]
        public String Jcmc { get; set; }//�����Ŀ����
        [DataMember()]
        public String Mcsx { get; set; }    //������д����
        [DataMember()]
        public String Ksfw { get; set; }//��ʼ��Χ��������Χ��
        [DataMember()]
        public String Jsfw { get; set; }//������Χ��������Χ��
        [DataMember()]
        public String Syrq { get; set; }//������Ⱥ��CP_PathEnterJudgeCondition.ID�ˣ����ˣ����ˣ�Ů�ˣ�Ӥ���ȣ�
        [DataMember()]
        public String Jsdw { get; set; }//��λ           
        [DataMember()]
        public String Py { get; set; }          //ƴ��
        [DataMember()]
        public String Wb { get; set; }          //���
        [DataMember()]
        public String Yxjl { get; set; }  //��Ч��¼
        [DataMember()]
        public String Bz { get; set; }  //��ע
        SuitCrowsMapScopes _SuitCrowsMapScopes = new SuitCrowsMapScopes();
        
        ///// <summary>
        ///// ������Ⱥ��Ӧ�ļ���Χ
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
