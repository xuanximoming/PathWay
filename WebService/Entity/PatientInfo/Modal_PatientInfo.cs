using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract]
    public class Modal_PatientInfo
    {

        [DataMember]
        public List<Modal_Dictionary> CommonDictionary
        { get; set;}

        [DataMember]
        public List<Modal_Areas> Areas
        { get; set; }

        [DataMember]
        public List<Modal_Diagnosis> Diagnosis { get; set; }

        [DataMember]
        public List<Modal_PatientContactsInfo> ContactsInfo { get; set; }


        

    }
}
