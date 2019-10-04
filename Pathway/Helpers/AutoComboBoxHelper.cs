using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

namespace YidanEHRApplication.DataService
{
    public partial class CP_DepartmentList
    {
        public override string ToString()
        {
            return Name;
        }
    }

    public partial class CP_DiagnosisList
    {
        public override string ToString()
        {
            return Name;
        }
    }

    public partial class CP_ClinicalPathList
    {
        public override string ToString()
        {
            return Name;
        }
    }

    public partial class CP_ChargingMinItem
    {
        public override string ToString()
        {
            return Name + " [" + Xmdw + "]" + " [" + Xmgg + "]";
        }
    }

    public partial class CP_Diagnosis_E
    {
        public override string ToString()
        {
            return Name;//+ "【" + Zdbs + "】"; ;
        }
    }

    public partial class CP_Surgery
    {
        public override string ToString()
        {
            return Name;
        }
    }


    public partial class CP_PlaceOfDrug
    {
        public override string ToString()
        {
            return Ypmc + " [" + Ypgg + "] [" + Lsj + "] [" + Cjmc + "]";
        } 
    }

    public partial class CP_Operation
    {
        public override string ToString()
        {
            return "代码：" + " 【" + Ssdm + "】  " + " 名称：" + "【 " + Name + "】";
        }
    }

     partial class Modal_Areas
    {
        public override string ToString()
        {

            return Name;
        }
    }

    partial class Modal_Diagnosis
    {
        public override string ToString()
        {
            return Name;// +"【" + Zdbs + "】";
        }
    }

    partial class Modal_Dictionary
    {
        public override string ToString()
        {
            return Name;
        }
    }

    partial class CP_InpatinetList
    {
        public override string ToString()
        {
            return Bed + " ["+Hzxm+"]" ;
        }
    }

    partial class Expressions
    {
        public override string ToString()
        {
            return ExpressionsGroupType;
        }
    }
     public partial class CP_ExamDictionaryDetail 
     {
         public override string ToString()
         {
             return Jcmc;
         }
     }

     public partial class CP_DiagNurExecCategoryDetail
     {
         public override string ToString()
         {
             return Name;
         }
     }

     public partial class CP_CYXDF
     {
         public override string ToString()
         {
             return cfmc;
         }
     }


     public partial class CP_CYFUnion
     {
         public override string ToString()
         {
             return Ypmc + " [" + Ypgg + "]";
         }
     }
}
