using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using System.Runtime.Serialization;
using System.Data;
using YidanSoft.Tool;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 病人类?
    /// 此处代码需要重构
    /// 对现有的病人类
    /// </summary>
    [DataContract()]
    public partial class CP_InpatinetList : INotifyPropertyChanged
    {
        private string m_Syxh;
        /// <summary>
        ///   首页序号
        /// </summary>  
        [DataMember()]
        public string Syxh
        {
            get { return m_Syxh; }
            set { m_Syxh = value; }
        }
        private string m_Hissyxh;

        /// <summary>
        /// HIS号
        /// </summary>
        [DataMember()]
        public string Hissyxh
        {
            get { return m_Hissyxh; }
            set { m_Hissyxh = value; }
        }
        private string m_Zyhm;


        /// <summary>
        /// 住院号码
        /// </summary>
        [DataMember()]
        public string Zyhm
        {
            get { return m_Zyhm; }
            set { m_Zyhm = value; }
        }
        private string m_Hzxm;


        /// <summary>
        /// 患者姓名
        /// </summary>
        [DataMember()]
        public string Hzxm
        {
            get { return m_Hzxm; }
            set { m_Hzxm = value; }
        }
        private string m_Brxb;


        /// <summary>
        /// 病人性别
        /// </summary>
        [DataMember()]
        public string Brxb
        {
            get { return m_Brxb; }
            set { m_Brxb = value; }
        }
        private string m_Xsnl;


        /// <summary>
        /// 显示年龄
        /// </summary>
        [DataMember()]
        public string Xsnl
        {
            get { return m_Xsnl; }
            set { m_Xsnl = value; }
        }
        private string m_Hkdz;

        /// <summary>
        /// 户口地址
        /// </summary>
        [DataMember()]
        public string Hkdz
        {
            get { return m_Hkdz; }
            set { m_Hkdz = value; }
        }
        private string m_Ryrq;

        /// <summary>
        /// 入院日期
        /// </summary>
        [DataMember()]
        public string Ryrq
        {
            get { return m_Ryrq; }
            set { m_Ryrq = value; }
        }
        private string m_Brzt;

        /// <summary>
        /// 病人状态
        /// </summary>
        [DataMember()]
        public string Brzt
        {
            get { return m_Brzt; }
            set { m_Brzt = value; }
        }
        private string m_Wzjb;

        /// <summary>
        /// 危重级别
        /// </summary>
        [DataMember()]
        public string Wzjb
        {
            get { return m_Wzjb; }
            set { m_Wzjb = value; }
        }

        private string m_Ryzd;


        /// <summary>
        /// 入院诊断
        /// </summary>
        [DataMember()]
        public string Ryzd
        {
            get { return m_Ryzd; }
            set { m_Ryzd = value; }
        }
        private string m_RyzdCode;


        /// <summary>
        /// 入院诊断CODE
        /// </summary>
        [DataMember()]
        public string RyzdCode
        {
            get { return m_RyzdCode; }
            set { m_RyzdCode = value; }
        }
        private string m_Ljzt;

        /// <summary>
        /// 路径状态ID
        /// </summary>
        [DataMember()]
        public string Ljzt
        {
            get { return m_Ljzt; }
            set { m_Ljzt = value; }
        }

        private string m_Pgqk;
        /// <summary>
        /// 评估情况
        /// </summary>
        [DataMember()]
        public string Pgqk
        {
            get { return m_Pgqk; }
            set { m_Pgqk = value; }
        }

        private string m_Ljmc;
        /// <summary>
        /// 路径名称
        /// </summary>
        [DataMember()]
        public string Ljmc
        {
            get { return m_Ljmc; }
            set { m_Ljmc = value; }
        }

        private string m_LjztName;
        /// <summary>
        /// 路径状态
        /// </summary>
        [DataMember()]
        public string LjztName
        {
            get { return m_LjztName; }
            set { m_LjztName = value; }
        }

        private string m_LqljId;
        /// <summary>
        /// 临床路径ID
        /// </summary>
        [DataMember()]
        public string LqljId
        {
            get { return m_LqljId; }
            set { m_LqljId = value; }
        }

        private string m_Bed;
        /// <summary>
        /// 床位号
        /// </summary>
        [DataMember()]
        public string Bed
        {
            get { return m_Bed; }
            set { m_Bed = value; }
        }

        private string m_Csrq;
        /// <summary>
        /// 出生日期
        /// </summary>
        [DataMember()]
        public string Csrq
        {
            get { return m_Csrq; }
            set { m_Csrq = value; }
        }

        private string m_Ljdm;
        /// <summary>
        /// 路径代码
        /// </summary>
        [DataMember()]
        public string Ljdm
        {
            get { return m_Ljdm; }
            set { m_Ljdm = value; }
        }

        private string m_Ljts;
        /// <summary>
        /// 当前步骤天数
        /// </summary>
        [DataMember()]
        public string Ljts
        {
            get { return m_Ljts; }
            set { m_Ljts = value; }
        }

        private string m_RydmLjdm;
        /// <summary>
        /// 临床路径初始代码
        /// </summary>
        [DataMember()]
        public string RydmLjdm
        {
            get { return m_RydmLjdm; }
            set { m_RydmLjdm = value; }
        }

        private string m_Zyys;
        /// <summary>
        /// 住院医师
        /// </summary>
        [DataMember()]
        public string Zyys
        {
            get { return m_Zyys; }
            set { m_Zyys = value; }
        }

        private string m_ZyysDm;
        /// <summary>
        /// 住院医师代码
        /// </summary>
        [DataMember()]
        public string ZyysDm
        {
            get { return m_ZyysDm; }
            set { m_ZyysDm = value; }
        }

        private string m_WorkFlowXml;
        /// <summary>
        /// 工作流XML
        /// </summary>
        [DataMember()]
        public string WorkFlowXml
        {
            get { return m_WorkFlowXml; }
            set { m_WorkFlowXml = value; }
        }

        private string m_EnForceWorkFlowXml;
        /// <summary>
        /// 实际执行工作流XML
        /// </summary>
        [DataMember()]
        public string EnForceWorkFlowXml
        {
            get { return m_EnForceWorkFlowXml; }
            set { m_EnForceWorkFlowXml = value; }
        }


        /// <summary>
        /// CP_InPathPatient.Id
        /// </summary>
        [DataMember()]
        public decimal BhljId
        {
            get;
            set;
        }

        /// <summary>
        /// 出院科室
        /// </summary>
        [DataMember()]
        public string Cyks
        {
            get;
            set;
        }

        /// <summary>
        /// 出院科室
        /// </summary>
        [DataMember()]
        public string CyksName
        {
            get;
            set;
        }

        /// <summary>
        /// 出院病区
        /// </summary>
        [DataMember()]
        public string Cybq
        {
            get;
            set;
        }

        /// <summary>
        /// 出院病区
        /// </summary>
        [DataMember()]
        public string CybqName
        {
            get;
            set;
        }

        /// <summary>
        /// 出院床位
        /// </summary>
        [DataMember()]
        public string Cycw
        {
            get;
            set;
        }

        /// <summary>
        /// 出区日期
        /// </summary>
        [DataMember()]
        public string Cqrq
        {
            get;
            set;
        }

        /// <summary>
        /// 出院日期
        /// </summary>
        [DataMember()]
        public string Cyrq
        {
            get;
            set;
        }

        /// <summary>
        /// 出院诊断
        /// </summary>
        [DataMember()]
        public string Cyzd
        {
            get;
            set;
        }

        /// <summary>
        /// 出院诊断
        /// </summary>
        [DataMember()]
        public string CyzdName
        {
            get;
            set;
        }

        /// <summary>
        /// 路径序号，KEY值
        /// </summary>
        [DataMember()]
        public Decimal Ljxh
        {
            get;
            set;
        }

        /// <summary>
        /// 当前操做员,数据库里没有，在前台赋值
        /// </summary>
        [DataMember()]
        public String CurOper
        {
            get;
            set;
        }

        /// <summary>
        /// NoOfRecord
        /// </summary>
        [DataMember()]
        public String NoOfRecord
        {
            get;
            set;
        }

        /// <summary>
        /// patID
        /// </summary>
        [DataMember()]
        public String patID
        {
            get;
            set;
        }

        /// <summary>
        /// NoofClinic
        /// </summary>
        [DataMember()]
        public String NoofClinic
        {
            get;
            set;
        }
        
        //add by luff 20130305 添加 病人状态属性
        /// <summary>
        /// Status 病人状态 1501 在院,1503 出院
        /// </summary>
        [DataMember()]
        public string Status
        {
            get;
            set;
        }
        
        

        /// <summary>
        /// InCount
        /// </summary>
        [DataMember()]
        public String InCount
        {
            get;
            set;
        }


        /// <summary>
        /// 疾病名称
        /// </summary>
        [DataMember()]
        public string Name
        {
            set { ;}
            get { return "患者姓名：" + Hzxm + "住院号码：【" + Zyhm + "】"; }
        }

        public CP_InpatinetList(string strSyxh, string strHissyxh, string strZyhm, string strHzxm, string strBrxb,
                                string strXsnl, string strHkdz, string strRyrq, string strBrzt, string strRyzd,
                                string strLjzt, string strPgqk, string strLjmc, string strLjztName, string strLqljId,
                                string strBed, string strCsrq, string strLjdm, string strLjts, string strRydmLjdm,
                                string strZyys, string strZyysDm, string strWorkFlow, string strEnFroceXml,
                                string strNoOfRecord, string strpatID, string strNoofClinic, string strStatus, string strInCount)
        {
            Syxh = strSyxh;
            Hissyxh = strHissyxh;
            Zyhm = strZyhm;
            Hzxm = strHzxm;
            Brxb = strBrxb;
            Xsnl = strXsnl;
            Hkdz = strHkdz;
            Ryrq = strRyrq;
            Brzt = strBrzt;
            Ryzd = strRyzd;
            Ljzt = strLjzt;
            Pgqk = strPgqk;
            Ljmc = strLjmc;
            LjztName = RemoveNumber(strLjztName.Replace('(',')').Replace(')','0'));
            LqljId = strLqljId;
            Bed = strBed;
            Csrq = strCsrq;
            Ljdm = strLjdm;
            Ljts = strLjts;
            RydmLjdm = strRydmLjdm;
            Zyys = strZyys;
            ZyysDm = strZyysDm;
            WorkFlowXml = strWorkFlow;
            EnForceWorkFlowXml = strEnFroceXml;

            NoOfRecord =  strNoOfRecord;
            patID =  strpatID;
            NoofClinic = strNoofClinic;
            Status = strStatus;
            InCount = strInCount;

        }

        /// <summary>
        /// 去掉字符串中的数字
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string RemoveNumber(string key)
        {
            return System.Text.RegularExpressions.Regex.Replace(key, @"\d", "");
        }

        public CP_InpatinetList()
        {

        }

        ///// <summary>
        ///// 病人基本信息
        ///// 修改：fqw 时间：2010-03-21  mark：fqwFix
        ///// </summary>
        //[DataMember()]
        //public CP_InPatient BasicInfo
        //{
        //    get
        //    {
        //        if (_basicInfo == null)
        //        {
        //            decimal syxh = Convert.ToDecimal(Syxh);
        //            DataTable dt = YidanEHRDataService.SqlHelper.ExecuteDataTable(String.Format("select * from  InPatient where NoOfInpat='{0}'", syxh));
        //            if (dt.Rows.Count > 0)
        //            {
        //                _basicInfo = new CP_InPatient();
        //                _basicInfo.Syxh = ConvertMy.ToDecimal(dt.Rows[0]["NoOfInpat"]);
        //                _basicInfo.Hissyxh = ConvertMy.ToDecimal(dt.Rows[0]["PatNoOfHis"]);
        //                _basicInfo.Mzhm = ConvertMy.ToString(dt.Rows[0]["NoOfClinic"]);
        //                _basicInfo.Bahm = ConvertMy.ToString(dt.Rows[0]["NoOfRecord"]);
        //                _basicInfo.Zyhm = ConvertMy.ToString(dt.Rows[0]["PatID"]);
        //                _basicInfo.Glzyhm = ConvertMy.ToString(dt.Rows[0]["InnerPIX"]);
        //                _basicInfo.Hzxm = ConvertMy.ToString(dt.Rows[0]["Name"]);
        //                _basicInfo.Py = ConvertMy.ToString(dt.Rows[0]["Py"]);
        //                _basicInfo.Wb = ConvertMy.ToString(dt.Rows[0]["Wb"]);
        //                _basicInfo.Brxz = ConvertMy.ToString(dt.Rows[0]["PayID"]);
        //                _basicInfo.Brly = ConvertMy.ToString(dt.Rows[0]["Origin"]);
        //                _basicInfo.Rycs = ConvertMy.ToInt32(dt.Rows[0]["InCount"]);
        //                _basicInfo.Brxb = ConvertMy.ToString(dt.Rows[0]["SexID"]);
        //                _basicInfo.Csrq = ConvertMy.ToString(dt.Rows[0]["Birth"]);
        //                _basicInfo.Brnl = ConvertMy.ToInt32(dt.Rows[0]["Age"]);
        //                _basicInfo.Xsnl = ConvertMy.ToString(dt.Rows[0]["AgeStr"]);
        //                _basicInfo.Sfzh = ConvertMy.ToString(dt.Rows[0]["IDNO"]);
        //                _basicInfo.Hyzk = ConvertMy.ToString(dt.Rows[0]["Marital"]);
        //                _basicInfo.Zydm = ConvertMy.ToString(dt.Rows[0]["JobID"]);
        //                _basicInfo.Ssdm = ConvertMy.ToString(dt.Rows[0]["ProvinceID"]);
        //                _basicInfo.Qxdm = ConvertMy.ToString(dt.Rows[0]["CountyID"]);
        //                _basicInfo.Mzdm = ConvertMy.ToString(dt.Rows[0]["NationID"]);
        //                _basicInfo.Gjdm = ConvertMy.ToString(dt.Rows[0]["NationalityID"]);
        //                _basicInfo.Jgssdm = ConvertMy.ToString(dt.Rows[0]["NationalityID"]);
        //                _basicInfo.Jgqxdm = ConvertMy.ToString(dt.Rows[0]["Nativeplace_C"]);
        //                _basicInfo.Gzdw = ConvertMy.ToString(dt.Rows[0]["Organization"]);
        //                _basicInfo.Dwdz = ConvertMy.ToString(dt.Rows[0]["OfficePlace"]);
        //                _basicInfo.Dwdh = ConvertMy.ToString(dt.Rows[0]["OfficeTEL"]);
        //                _basicInfo.Dwyb = ConvertMy.ToString(dt.Rows[0]["OfficePost"]);
        //                _basicInfo.Hkdz = ConvertMy.ToString(dt.Rows[0]["NativeAddress"]);
        //                _basicInfo.Hkdh = ConvertMy.ToString(dt.Rows[0]["NativeTEL"]);
        //                _basicInfo.Hkyb = ConvertMy.ToString(dt.Rows[0]["NativePost"]);
        //                _basicInfo.Dqdz = ConvertMy.ToString(dt.Rows[0]["Address"]);
        //                _basicInfo.Lxrm = ConvertMy.ToString(dt.Rows[0]["ContactPerson"]);
        //                _basicInfo.Lxgx = ConvertMy.ToString(dt.Rows[0]["Relationship"]);
        //                _basicInfo.Lxdz = ConvertMy.ToString(dt.Rows[0]["ContactAddress"]);
        //                _basicInfo.Lxdw = ConvertMy.ToString(dt.Rows[0]["ContactOffice"]);
        //                _basicInfo.Lxdh = ConvertMy.ToString(dt.Rows[0]["ContactTEL"]);
        //                _basicInfo.Lxyb = ConvertMy.ToString(dt.Rows[0]["ContactPost"]);
        //                _basicInfo.Bscsz = ConvertMy.ToString(dt.Rows[0]["Offerer"]);
        //                _basicInfo.Sbkh = ConvertMy.ToString(dt.Rows[0]["SocialCare"]);
        //                _basicInfo.Bxkh = ConvertMy.ToString(dt.Rows[0]["Insurance"]);
        //                _basicInfo.Qtkh = ConvertMy.ToString(dt.Rows[0]["CardNo"]);
        //                _basicInfo.Ryqk = ConvertMy.ToString(dt.Rows[0]["AdmitInfo"]);
        //                _basicInfo.Ryks = ConvertMy.ToString(dt.Rows[0]["AdmitDept"]);
        //                _basicInfo.Rybq = ConvertMy.ToString(dt.Rows[0]["AdmitWard"]);
        //                _basicInfo.Rycw = ConvertMy.ToString(dt.Rows[0]["AdmitBed"]);
        //                _basicInfo.Ryrq = ConvertMy.ToString(dt.Rows[0]["AdmitDate"]);
        //                _basicInfo.Rqrq = ConvertMy.ToString(dt.Rows[0]["InWardDate"]);
        //                _basicInfo.Ryzd = ConvertMy.ToString(dt.Rows[0]["AdmitDiagnosis"]);
        //                _basicInfo.Cyks = ConvertMy.ToString(dt.Rows[0]["OutHosDept"]);
        //                _basicInfo.Cybq = ConvertMy.ToString(dt.Rows[0]["OutHosWard"]);
        //                _basicInfo.Cycw = ConvertMy.ToString(dt.Rows[0]["OutBed"]);
        //                _basicInfo.Cqrq = ConvertMy.ToString(dt.Rows[0]["OutWardDate"]);
        //                _basicInfo.Cyrq = ConvertMy.ToString(dt.Rows[0]["OutHosDate"]);
        //                _basicInfo.Cyzd = ConvertMy.ToString(dt.Rows[0]["OutDiagnosis"]);
        //                _basicInfo.Zyts = ConvertMy.ToDecimal(dt.Rows[0]["TotalDays"]);
        //                _basicInfo.Mzzd = ConvertMy.ToString(dt.Rows[0]["ClinicDiagnosis"]);
        //                //_basicInfo.Mzzdzy = ConvertMy.ToString(dt.Rows[0]["Mzzdzy"]);
        //                // _basicInfo.Mzzhzy = ConvertMy.ToString(dt.Rows[0]["Mzzhzy"]);
        //                _basicInfo.Fbjq = ConvertMy.ToString(dt.Rows[0]["SolarTerms"]);
        //                _basicInfo.Rytj = ConvertMy.ToString(dt.Rows[0]["AdmitWay"]);
        //                _basicInfo.Cyfs = ConvertMy.ToString(dt.Rows[0]["OutWay"]);
        //                _basicInfo.Mzys = ConvertMy.ToString(dt.Rows[0]["ClinicDoctor"]);
        //                // _basicInfo.Zyys = ConvertMy.ToString(dt.Rows[0]["Zyys"]);
        //                _basicInfo.Zzysdm = ConvertMy.ToString(dt.Rows[0]["Resident"]);
        //                _basicInfo.Zrysdm = ConvertMy.ToString(dt.Rows[0]["Chief"]);
        //                _basicInfo.Whcd = ConvertMy.ToString(dt.Rows[0]["EDU"]);
        //                _basicInfo.Jynx = ConvertMy.ToDecimal(dt.Rows[0]["EDUC"]);
        //                _basicInfo.Zjxy = ConvertMy.ToString(dt.Rows[0]["Religion"]);
        //                _basicInfo.Brzt = ConvertMy.ToShort(dt.Rows[0]["Status"]);
        //                _basicInfo.Wzjb = ConvertMy.ToString(dt.Rows[0]["CriticalLevel"]);
        //                _basicInfo.Hljb = ConvertMy.ToString(dt.Rows[0]["AttendLevel"]);
        //                _basicInfo.Zdbr = ConvertMy.ToShort(dt.Rows[0]["Emphasis"]);
        //                _basicInfo.Yexh = ConvertMy.ToShort(dt.Rows[0]["IsBaby"]);
        //                _basicInfo.Mqsyxh = ConvertMy.ToDecimal(dt.Rows[0]["Mother"]);
        //                _basicInfo.Ybdm = ConvertMy.ToString(dt.Rows[0]["MedicareID"]);
        //                _basicInfo.Ybde = ConvertMy.ToDecimal(dt.Rows[0]["MedicareQuota"]);
        //                _basicInfo.Brlx = ConvertMy.ToString(dt.Rows[0]["Style"]);
        //                _basicInfo.Pzlx = ConvertMy.ToString(dt.Rows[0]["VouchersCode"]);
        //                //_basicInfo.Pzlxmc = ConvertMy.ToString(dt.Rows[0]["Pzlxmc"]);
        //                //_basicInfo.Pzh = ConvertMy.ToString(dt.Rows[0]["Pzh"]);
        //                _basicInfo.Czyh = ConvertMy.ToString(dt.Rows[0]["Operator"]);
        //                //_basicInfo.Xxh = ConvertMy.ToString(dt.Rows[0]["Xxh"]);
        //                //_basicInfo.Gxrq = ConvertMy.ToString(dt.Rows[0]["Gxrq"]);
        //                //_basicInfo.Memo = ConvertMy.ToString(dt.Rows[0]["Memo"]);

        //            }
        //            #region 微构前代码

        //            //using (YidanEHREntities enties = new YidanEHREntities())
        //            //{
        //            //    decimal syxh = Convert.ToDecimal(Syxh);

        //            //    _basicInfo = enties.CP_InPatient.Where(cp => cp.Syxh.Equals(syxh)).FirstOrDefault();
        //            //}
        //            #endregion

        //        }
        //        return _basicInfo;
        //    }
        //    set { _basicInfo = value; }
        //}

        //private CP_InPatient _basicInfo;


        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion
    }
}