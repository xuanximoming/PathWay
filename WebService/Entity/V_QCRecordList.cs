using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    [DataContract()]
    public partial class V_QCRecord
    {
        [DataMember()]
        public string ConditionTime { get; set; }
        [DataMember()]
        public string Expr1 { get; set; }
        [DataMember()]
        public string FoulMessage { get; set; }
        [DataMember()]
        public Nullable<short> FoulState { get; set; }
        [DataMember()]
        public string InPatientName { get; set; }
        [DataMember()]
        public string MessageInfo { get; set; }
        [DataMember()]
        public string Name { get; set; }
        [DataMember()]
        public string Reminder { get; set; }
        [DataMember()]
        public decimal Result { get; set; }
        [DataMember()]
        public string Resident { get; set; }
    }
}