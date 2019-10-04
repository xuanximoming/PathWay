
using System;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using Yidansoft.Service.Entity;
using System.Collections;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Configuration;
using YidanSoft.Tool;
using Yidansoft.Service.Entity.NursingNotes;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        #region 护理记录单录入

        #region 获取生命体征记录时间段
        /// <summary>
        /// 获取生命体征记录时间段
        /// </summary>
        /// <returns>返回时间段数组</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public string GetVitalSignsRecordTimeSet()
        {
            try
            {
                string strSql = "select Value from appcfg where Configkey='VITALSIGNSRECORDTIME'";


                String strReturn = "";
                DataTable dt = SqlHelper.ExecuteDataTable(strSql);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != null)
                {
                    strReturn = dt.Rows[0][0].ToString().Trim();
                }
                return strReturn;

            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }
        }
        #endregion

        #region 从编码表中获取护理记录单编码
        /// <summary>
        /// 从编码表中获取护理记录单编码
        /// </summary>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_NurseCodeCollection> GetNurseCode()
        {

            try
            {


                DataSet myDataSet = SqlHelper.ExecuteDataSet("usp_CP_GetNurseCode");

                List<CP_NurseCodeCollection> lstCodeCollection = new List<CP_NurseCodeCollection>();
                List<CP_NurseCode> lstCode = new List<CP_NurseCode>();
                CP_NurseCodeCollection[] cpNurseCode = new CP_NurseCodeCollection[12];

                //读取编码
                for (int i = 0; i < 12; i++)
                {
                    lstCode.Clear();
                    cpNurseCode[i] = new CP_NurseCodeCollection(); //对象实例化
                    foreach (DataRow dr in myDataSet.Tables[i].Rows)
                    {
                        CP_NurseCode cp = new CP_NurseCode();
                        cp.CodeID = Convert.ToInt32(dr["Mxbh"].ToString());
                        cp.CodeName = dr["Name"].ToString();

                        //lstCode.Add(cp);
                        cpNurseCode[i].NurseCodeCollection.Add(cp);
                    }
                    //cpNurseCode[i].NurseCodeCollection = lstCode;
                    lstCodeCollection.Add(cpNurseCode[i]);
                }
                return lstCodeCollection;
            }

            catch (Exception ex)
            {
                ThrowException(ex);
            }
            //finally
            //{
            //   myConnection.Close();
            //}
            return null;
            //}
        }
        #endregion

        #region 保存护理记录单
        /// <summary>
        /// 保存护理记录单
        /// </summary>
        /// <param name="lst"></param>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public string SaveNursingNotes(CP_SaveNursingNotes lst, string Zyhm)
        {
            if (lst.bSaveTag1)
            {
                if (SaveVitalSign(lst.CPVitalSignsRecord, Zyhm) == "该时间段生命体征记录已存在!")
                    return "该时间段生命体征记录已存在!";
            }

            if (lst.bSaveTag2)
            {
                SavePatientIn(lst.CPPatientInOutRecordIn);
            }

            if (lst.bSaveTag3)
            {
                SavePatientOut(lst.CPPatientInOutRecordOut);
            }

            if (lst.bSaveTag4)
            {
                SaveTreatmentFlow(lst.CPTreatmentFlow);
            }

            if (lst.bSaveTag5)
            {
                SaveVitalSignSpecialRecord(lst.CPVitalSignSpecialRecord);
            }
            return "";
        }
        #endregion

        #region 保存病人生命体征
        /// <summary>
        /// 保存病人生命体征
        /// </summary>
        /// <param name="lst">病人生命体征实体</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public string SaveVitalSign(CP_VitalSignsRecordInfo lst, string Zyhm)
        {
            try
            {


                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert CP_VitalSignsRecord ");
                strSql.Append(" (Zyhm,Clrq,Clsj,Hzztdm,Hzzt,Hztw,Clfsdm,Clfs,Fzcsdm,Fzcs,Hzmb,Hzxl,Qbq,Hzhx,Hxq,Hzxy,Djysdm,Djys,Sjd,Ljxh,Ljdm,ActivityId,ActivityChildId) ");
                strSql.Append(" values");
                strSql.Append(" ( @Zyhm,@Clrq,@Clsj,@Hzztdm,@Hzzt,@Hztw,@Clfsdm,@Clfs,@Fzcsdm,@Fzcs,@Hzmb,@Hzxl,@Qbq," +
                    "@Hzhx,@Hxq,@Hzxy,@Djysdm,@Djys,@Sjd,@Ljxh,@Ljdm,@ActivityId,@ActivityChildId) ");

                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Zyhm",lst.Zyhm),
                new SqlParameter("@Clrq",lst.Clrq),
                new SqlParameter("@Clsj",lst.Clsj),
                new SqlParameter("@Hzztdm",lst.Hzztdm),
                new SqlParameter("@Hzzt",lst.Hzzt),
                new SqlParameter("@Hztw",lst.Hztw),
                new SqlParameter("@Clfsdm",lst.Clfsdm),
                new SqlParameter("@Clfs",lst.Clfs),
                new SqlParameter("@Fzcsdm",lst.Fzcsdm),
                new SqlParameter("@Fzcs",lst.Fzcs),
                new SqlParameter("@Hzmb",lst.Hzmb),
                new SqlParameter("@Hzxl",lst.Hzxl),
                new SqlParameter("@Qbq",lst.Qbq),
                new SqlParameter("@Hzhx",lst.Hzhx),
                new SqlParameter("@Hxq",lst.Hxq),
                new SqlParameter("@Hzxy",lst.Hzxy),
                new SqlParameter("@Djysdm",lst.Djysdm),
                new SqlParameter("@Djys",lst.Djys),
                new SqlParameter("@Sjd",lst.Sjd), 
                
                new SqlParameter("@Ljxh",lst.Ljxh),
                new SqlParameter("@Ljdm",lst.Ljdm),
                new SqlParameter("@ActivityId",lst.ActivityId),     
                new SqlParameter("@ActivityChildId",lst.ActivityChildId)
            };

                SqlHelper.ExecuteNoneQuery(strSql.ToString(), param, CommandType.Text);

                return "操作完成!";

            }

            catch (Exception ex)
            {

                ThrowException(ex);
                return "操作失败!";

            }

        }
        #endregion

        #region 保存病人入量
        /// <summary>
        ///  保存病人入量
        /// </summary>
        /// <param name="lst">病人入量/出量实体，入量数据</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public string SavePatientIn(CP_PatientInOutRecordInfo lst)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert CP_PatientInOutRecord ");
                strSql.Append(" (Zyhm,Jllx,Clrq,Clsj,Ysl,Hsl,Syl,Zsl,Sxl,Qtrllxdm1,Qtrllx1,Qtrl1,Qtrllxdm2,Qtrllx2,Qtrl2,Djysdm,Djys,Ljxh,Ljdm,ActivityId,ActivityChildId) ");
                strSql.Append(" values ");
                strSql.Append(" (@Zyhm,@Jllx,@Clrq,@Clsj,@Ysl,@Hsl,@Syl,@Zsl,@Sxl,@Qtrllxdm1,@Qtrllx1,@Qtrl1," +
                    "@Qtrllxdm2,@Qtrllx2,@Qtrl2,@Djysdm,@Djys,@Ljxh,@Ljdm,@ActivityId,@ActivityChildId) ");

                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Zyhm",lst.Zyhm),
                 new SqlParameter("@Jllx",lst.Jllx),
                new SqlParameter("@Clrq",lst.Clrq),
                new SqlParameter("@Clsj",lst.Clsj),
                new SqlParameter("@Ysl",lst.Ysl),
                new SqlParameter("@Hsl",lst.Hsl),
                new SqlParameter("@Syl",lst.Syl),
                new SqlParameter("@Zsl",lst.Zsl),
                new SqlParameter("@Sxl",lst.Sxl),
                new SqlParameter("@Qtrllxdm1",lst.Qtrllxdm1),
                new SqlParameter("@Qtrllx1",lst.Qtrllx1),
                new SqlParameter("@Qtrl1",lst.Qtrl1),
                new SqlParameter("@Qtrllxdm2",lst.Qtrllxdm2),
                new SqlParameter("@Qtrllx2",lst.Qtrllx2),
                new SqlParameter("@Qtrl2",lst.Qtrl2),
                new SqlParameter("@Djysdm",lst.Djysdm),
                new SqlParameter("@Djys",lst.Djys),
                
                new SqlParameter("@Ljxh",lst.Ljxh),
                new SqlParameter("@Ljdm",lst.Ljdm),
                new SqlParameter("@ActivityId",lst.ActivityId),     
                new SqlParameter("@ActivityChildId",lst.ActivityChildId)
            };

                SqlHelper.ExecuteNoneQuery(strSql.ToString(), param, CommandType.Text);

                return "操作完成!";

            }
            catch (Exception ex)
            {
                ThrowException(ex); return "操作失败!";
            }

        }
        #endregion

        #region 保存病人出量
        /// <summary>
        ///  保存病人出量
        /// </summary>
        /// <param name="lst">病人入量/出量实体，出量数据</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public string SavePatientOut(CP_PatientInOutRecordInfo lst)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert CP_PatientInOutRecord ");
                strSql.Append(" (Zyhm,Jllx,Clrq,Clsj,Hzxb,Xbxzdm,Xbxz,Xbcsdm,Xbcs,Dbcs,Dbxzdm,Dbxz," +
                    "Pbcsdm,Pbcs,Hztl,Txzdm,Txz,Yll,Ylsm,Qtcllxdm1,Qtcllx1,Qtcl1,Qtcllxdm2,Qtcllx2,Qtcl2,Djysdm,Djys,Ljxh,Ljdm,ActivityId,ActivityChildId) ");
                strSql.Append(" values ");
                strSql.Append(" (@Zyhm,@Jllx,@Clrq,@Clsj,@Hzxb,@Xbxzdm,@Xbxz,@Xbcsdm,@Xbcs,@Dbcs,@Dbxzdm,@Dbxz," +
                    "@Pbcsdm,@Pbcs,@Hztl,@Txzdm,@Txz,@Yll,@Ylsm,@Qtcllxdm1,@Qtcllx1,@Qtcl1,@Qtcllxdm2,@Qtcllx2,@Qtcl2,@Djysdm,@Djys,@Ljxh,@Ljdm,@ActivityId,@ActivityChildId) ");

                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Zyhm",lst.Zyhm),
                 new SqlParameter("@Jllx",lst.Jllx),
                new SqlParameter("@Clrq",lst.Clrq),
                new SqlParameter("@Clsj",lst.Clsj),
                new SqlParameter("@Hzxb",lst.Hzxb),
                new SqlParameter("@Xbxzdm",lst.Xbxzdm),
                new SqlParameter("@Xbxz",lst.Xbxz),
                new SqlParameter("@Xbcsdm",lst.Xbcsdm),
                new SqlParameter("@Xbcs",lst.Xbcs),
                new SqlParameter("@Dbcs",lst.Dbcs),
                new SqlParameter("@Dbxzdm",lst.Dbxzdm),
                new SqlParameter("@Dbxz",lst.Dbxz),
                new SqlParameter("@Pbcsdm",lst.Pbcsdm),
                new SqlParameter("@Pbcs",lst.Pbcs),
                new SqlParameter("@Hztl",lst.Hztl),
                 new SqlParameter("@Txzdm",lst.Txzdm),
                new SqlParameter("@Txz",lst.Txz),
                new SqlParameter("@Yll",lst.Yll),
                new SqlParameter("@Ylsm",lst.Ylsm),
                new SqlParameter("@Qtcllxdm1",lst.Qtcllxdm1),
                new SqlParameter("@Qtcllx1",lst.Qtcllx1),
                new SqlParameter("@Qtcl1",lst.Qtcl1),
                new SqlParameter("@Qtcllxdm2",lst.Qtcllxdm2),
                new SqlParameter("@Qtcllx2",lst.Qtcllx2),
                new SqlParameter("@Qtcl2",lst.Qtcl2),
                new SqlParameter("@Djysdm",lst.Djysdm),
                new SqlParameter("@Djys",lst.Djys)  , 
                
                new SqlParameter("@Ljxh",lst.Ljxh),
                new SqlParameter("@Ljdm",lst.Ljdm),
                new SqlParameter("@ActivityId",lst.ActivityId),     
                new SqlParameter("@ActivityChildId",lst.ActivityChildId)
            };

                SqlHelper.ExecuteNoneQuery(strSql.ToString(), param, CommandType.Text);

                return "操作完成!";



            }
            catch (Exception ex)
            {
                ThrowException(ex); return "操作失败!";
            }

        }
        #endregion

        #region 患者治疗主要事件
        /// <summary>
        ///  患者治疗主要事件
        /// </summary>
        /// <param name="lst">患者治疗主要事件实体</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public string SaveTreatmentFlow(CP_TreatmentFlowInfo lst)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert CP_TreatmentFlow ");
                strSql.Append(" (Zyhm,Clrq,Clsj,Zllc,Lcsm,Sfss,Djysdm,Djys,Ljxh,Ljdm,ActivityId,ActivityChildId) ");
                strSql.Append(" values ");
                strSql.Append(" (@Zyhm,@Clrq,@Clsj,@Zllc,@Lcsm,@Sfss,@Djysdm,@Djys,@Ljxh,@Ljdm,@ActivityId,@ActivityChildId) ");

                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Zyhm",lst.Zyhm),
                new SqlParameter("@Clrq",lst.Clrq),
                new SqlParameter("@Clsj",lst.Clsj),
                new SqlParameter("@Zllc",lst.Zllc),
                new SqlParameter("@Lcsm",lst.Lcsm),
                new SqlParameter("@Sfss",lst.Sfss),
                new SqlParameter("@Djysdm",lst.Djysdm),
                new SqlParameter("@Djys",lst.Djys)   , 
                
                new SqlParameter("@Ljxh",lst.Ljxh),
                new SqlParameter("@Ljdm",lst.Ljdm),
                new SqlParameter("@ActivityId",lst.ActivityId),     
                new SqlParameter("@ActivityChildId",lst.ActivityChildId)
            };

                SqlHelper.ExecuteNoneQuery(strSql.ToString(), param, CommandType.Text);

                return "操作完成!";

            }
            catch (Exception ex)
            {
                ThrowException(ex); return "操作失败!";
            }

        }
        #endregion

        #region 病人特殊护理记录
        /// <summary>
        ///  病人特殊护理记录
        /// </summary>
        /// <param name="lst">病人特殊护理记录实体</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public string SaveVitalSignSpecialRecord(CP_VitalSignSpecialRecordInfo lst)
        {
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert CP_VitalSignSpecialRecord ");
                strSql.Append(" (Zyhm,Clrq,Clsj,Hzsg,Hztz,Hzfw,Hzxxdm,Hzxx,Xyxxdm,Xyxx,Hzsss,HzSxs,Hzgms,Djysdm,Djys,Ljxh,Ljdm,ActivityId,ActivityChildId) ");
                strSql.Append(" values ");
                strSql.Append(" (@Zyhm,@Clrq,@Clsj,@Hzsg,@Hztz,@Hzfw,@Hzxxdm,@Hzxx,@Xyxxdm,@Xyxx,@Hzsss,@Hzsxs,@Hzgms,@Djysdm,@Djys,@Ljxh,@Ljdm,@ActivityId,@ActivityChildId) ");

                SqlParameter[] param = new SqlParameter[]
            {
                new SqlParameter("@Zyhm",lst.Zyhm),
                new SqlParameter("@Clrq",lst.Clrq),
                new SqlParameter("@Clsj",lst.Clsj),
                new SqlParameter("@Hzsg",lst.Hzsg),
                new SqlParameter("@Hztz",lst.Hztz),
                new SqlParameter("@Hzfw",lst.Hzfw),
                new SqlParameter("@Hzxxdm",lst.Hzxxdm),
                new SqlParameter("@Hzxx",lst.Hzxx),
                new SqlParameter("@Xyxxdm",lst.Xyxxdm),
                new SqlParameter("@Xyxx",lst.Xyxx),
                new SqlParameter("@Hzsss",lst.Hzsss),
                new SqlParameter("@Hzsxs",lst.Hzsxs),
                new SqlParameter("@Hzgms",lst.Hzgms),
                new SqlParameter("@Djysdm",lst.Djysdm),
                new SqlParameter("@Djys",lst.Djys), 
                
                new SqlParameter("@Ljxh",lst.Ljxh),
                new SqlParameter("@Ljdm",lst.Ljdm),
                new SqlParameter("@ActivityId",lst.ActivityId),     
                new SqlParameter("@ActivityChildId",lst.ActivityChildId)
            };

                SqlHelper.ExecuteNoneQuery(strSql.ToString(), param, CommandType.Text);

                return "操作完成!";



            }
            catch (Exception ex)
            {
                ThrowException(ex); return "操作失败!";
            }
        }
        #endregion
        #endregion
    }


}
