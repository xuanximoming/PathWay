using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 表示路径执行提前提醒表的类
    /// </summary>
    [DataContract()]
    
    public class CP_MedicalTreatmentWarm
    {
        [DataMember]
        public String ID { get; set; }	   //自增字段
        [DataMember]
        public String Syxh { get; set; }	//首页序号(住院流水号)(InPatient.Syxh)
        [DataMember]
        public String Ljxh { get; set; }	//路径序号CP_InPathPatient.Id
        [DataMember]
        public String Ljdm { get; set; }	//临床路径代码（CP_ClinicalPath.Ljdm)
        [DataMember]
        public String Jddm { get; set; }	//节点的GUID 
        [DataMember]
        public String Hzjddm { get; set; }	//后置节点UniqueID
        [DataMember]
        public String Txlx { get; set; }	//提醒类型(1:手术，2：检查，3,日期，4费用)
        [DataMember]
        public String Txzt { get; set; }	//提醒状态(0:未查看，1：已查看)
        [DataMember]
        public String dm { get; set; }		//提醒编码（药品代码||手术代码(or收费小项目代码or临床项目代码,具体是哪种代码根据项目类别来判断)(CP_PlaceOfDrug.Ypdm/CP_ChargingMinItem.Sfxmdm/CP_LCChargingItem.Lcxmdm)）
        [DataMember]
        public String mc { get; set; }		//提醒内容（药品(项目)名称||手术名称||其他(CP_PlaceOfDrug.Name/CP_ChargingMinItem.Name)）
        [DataMember]
        public String jdmc { get; set; }//节点名称
        [DataMember]
        public String TxlxName { get; set; }////提醒类型名称(1:手术，2：检查，)
        [DataMember]
        public Boolean TxztBoolean { get {
            if (Txzt == "" || Txzt == "0") return false;
            else
                return true;
        }
            set { _TxztBoolean = value; }
        }
        private Boolean _TxztBoolean;
    }

    /// <summary>
    /// 表示路径执行提前提醒表根据Txlx分组的类
    /// </summary>
    [DataContract()]
    public class CP_MedicalTreatmentWarmGroupByTxlx
    {
        [DataMember]
        public String jddm { get; set; }//节点的GUID 
        [DataMember]
        public String jdmc { get; set; }//节点名称
        [DataMember]
        public String txsl { get; set; }//数量
        [DataMember]
        public String Txlx { get; set; }////提醒类型(1:手术，2：检查，)
        [DataMember]
        public String TxlxName { get; set; }////提醒类型名称(1:手术，2：检查，)
        [DataMember]
        public String Dysl { get; set; }//待阅数量
    }


}