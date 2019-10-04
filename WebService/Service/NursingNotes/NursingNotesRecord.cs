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
using DrectSoft.Tool;
using Yidansoft.Service.Entity.NursingNotes;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {

        #region 护理记录单查询显示模块
        /// <summary>
        /// 护理记录单查询显示模块
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public CP_NursingNotesRecordCollection QueryNursingNotesInfo(CP_InpatinetList cpInfo, int days, String activityChildId)
      {
         //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
         //{
            try
            {


                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@Zyhm",cpInfo.Zyhm),
                    new SqlParameter("@Days",days),
                    new SqlParameter("@Ljxh",cpInfo.Ljxh),
                    new SqlParameter("@ActivityChileId",activityChildId)
                };



                DataSet myDataSet = SqlHelper.ExecuteDataSet("usp_CP_NursingNotesRecord",parameters,CommandType.StoredProcedure);

               // List<CP_NursingNotesRecordCollection> lstInfoCollection = new List<CP_NursingNotesRecordCollection>();
               CP_NursingNotesRecordCollection CPCollection = new CP_NursingNotesRecordCollection();

               #region 读取病人信息


               #endregion

               #region 生命体征护理记录
               //生命体征护理记录
               foreach (DataRow dr in myDataSet.Tables[0].Rows)
               {
                  CP_VitalSignsRecordInfo cp = new CP_VitalSignsRecordInfo();
                  cp.Clfs = dr["Clfs"].ToString();
                  cp.Clfsdm = Convert.ToInt32(dr["Clfsdm"].ToString());
                  cp.Clrq = dr["Clrq"].ToString();
                  cp.Clsj = dr["Clsj"].ToString();
                  cp.Djrq = dr["Djrq"].ToString();
                  cp.Djys = dr["Djys"].ToString();
                  cp.Djysdm = dr["Djysdm"].ToString();
                  cp.Fzcs = dr["Fzcs"].ToString();
                  cp.Fzcsdm = Convert.ToInt32(dr["Fzcsdm"].ToString());
                  cp.Hxq = dr["Hxq"].ToString() == "True" ? 1 : 0;
                  cp.Hzhx = dr["Hzhx"].ToString();
                  cp.Hzmb = dr["Hzmb"].ToString();
                  cp.Hztw = dr["Hztw"].ToString();
                  cp.Hzxl = dr["Hzxl"].ToString();
                  cp.Hzxy = dr["Hzxy"].ToString();
                  cp.Hzzt = dr["Hzzt"].ToString();
                  cp.Hzztdm = Convert.ToInt32(dr["Hzztdm"].ToString());
                  cp.Jlxh = dr["Jlxh"].ToString();
                  cp.Qbq = dr["Qbq"].ToString() == "True" ? 1 : 0;
                  cp.Zyhm = dr["Zyhm"].ToString();
                  cp.Zfrq = dr["Zfrq"].ToString();
                  cp.Zfry = dr["Zfry"].ToString();
                  cp.Zfrydm = dr["Zfrydm"].ToString();
                  cp.Sjd = dr["Sjd"].ToString();

                  cp.ActivityId = dr["ActivityId"].ToString();
                  cp.ActivityChildId = dr["ActivityChildId"].ToString();
                  cp.Ljdm = dr["Ljdm"].ToString();
                  cp.Ljxh = DrectSoft.Tool.ConvertMy.ToDecimal(dr["Ljxh"].ToString());

                  CPCollection.CP_VitalSignsRecordCollection.Add(cp);
               }
               #endregion

               #region 病人入量护理记录
               //病人入量护理记录
               foreach (DataRow dr in myDataSet.Tables[1].Rows)
               {
                  CP_PatientInOutRecordInfo cp = new CP_PatientInOutRecordInfo();
                  cp.Jlxh = dr["Jlxh"].ToString();
                  cp.Zyhm = dr["Zyhm"].ToString();
                  cp.Clrq = dr["Clrq"].ToString();
                  cp.Clsj = dr["Clsj"].ToString();
                  cp.Jllx = dr["Jllx"].ToString() == "True" ? 1 : 0;
                  cp.Ysl = dr["Ysl"].ToString();
                  cp.Hsl = dr["Hsl"].ToString();
                  cp.Syl = dr["Syl"].ToString();
                  cp.Zsl = dr["Zsl"].ToString();
                  cp.Sxl = dr["Sxl"].ToString();
                  cp.Qtrllxdm1 = Convert.ToInt32(dr["Qtrllxdm1"].ToString());
                  cp.Qtrllx1 = dr["Qtrllx1"].ToString();
                  cp.Qtrl1 = dr["Qtrl1"].ToString();
                  cp.Qtrllxdm2 = Convert.ToInt32(dr["Qtrllxdm2"].ToString());
                  cp.Qtrllx2 = dr["Qtrllx2"].ToString();
                  cp.Qtrl2 = dr["Qtrl2"].ToString();
                  cp.Djrq = dr["Djrq"].ToString();
                  cp.Djysdm = dr["Djysdm"].ToString();
                  cp.Djys = dr["Djys"].ToString();
                  cp.Zfrq = dr["Zfrq"].ToString();
                  cp.Zfrydm = dr["Zfrydm"].ToString();
                  cp.Zfry = dr["Zfry"].ToString();

                  cp.ActivityId = dr["ActivityId"].ToString();
                  cp.ActivityChildId = dr["ActivityChildId"].ToString();
                  cp.Ljdm = dr["Ljdm"].ToString();
                  cp.Ljxh = DrectSoft.Tool.ConvertMy.ToDecimal(dr["Ljxh"].ToString());


                  CPCollection.CP_PatientInOutRecordInCollection.Add(cp);
               }
               #endregion

               #region 病人出量护理记录
               //病人出量护理记录
               foreach (DataRow dr in myDataSet.Tables[2].Rows)
               {
                  CP_PatientInOutRecordInfo cp = new CP_PatientInOutRecordInfo();
                  cp.Jlxh = dr["Jlxh"].ToString();
                  cp.Zyhm = dr["Zyhm"].ToString();
                  cp.Clrq = dr["Clrq"].ToString();
                  cp.Clsj = dr["Clsj"].ToString();
                  cp.Jllx = dr["Jllx"].ToString() == "True" ? 1 : 0;
                  cp.Hzxb = dr["Hzxb"].ToString();
                  cp.Xbxzdm = Convert.ToInt32(dr["Xbxzdm"].ToString());
                  cp.Xbxz = dr["Xbxz"].ToString();
                  cp.Xbcsdm = Convert.ToInt32(dr["Xbcsdm"].ToString());
                  cp.Xbcs = dr["Xbcs"].ToString();
                  cp.Dbcs = dr["Dbcs"].ToString();
                  cp.Dbxzdm = Convert.ToInt32(dr["Dbxzdm"].ToString());
                  cp.Dbxz = dr["Dbxz"].ToString();
                  cp.Pbcsdm = Convert.ToInt32(dr["Pbcsdm"].ToString());
                  cp.Pbcs = dr["Pbcs"].ToString();
                  cp.Hztl = dr["Hztl"].ToString();
                  cp.Txzdm = Convert.ToInt32(dr["Txzdm"].ToString());
                  cp.Txz = dr["Txz"].ToString();
                  cp.Yll = dr["Yll"].ToString();
                  cp.Ylsm = dr["Ylsm"].ToString();
                  cp.Qtcllxdm1 = Convert.ToInt32(dr["Qtcllxdm1"].ToString());
                  cp.Qtcllx1 = dr["Qtcllx1"].ToString();
                  cp.Qtcl1 = dr["Qtcl1"].ToString();
                  cp.Qtcllxdm2 = Convert.ToInt32(dr["Qtcllxdm2"].ToString());
                  cp.Qtcllx2 = dr["Qtcllx2"].ToString();
                  cp.Qtcl2 = dr["Qtcl2"].ToString();
                  cp.Djrq = dr["Djrq"].ToString();
                  cp.Djysdm = dr["Djysdm"].ToString();
                  cp.Djys = dr["Djys"].ToString();
                  cp.Zfrq = dr["Zfrq"].ToString();
                  cp.Zfrydm = dr["Zfrydm"].ToString();
                  cp.Zfry = dr["Zfry"].ToString();

                  cp.ActivityId = dr["ActivityId"].ToString();
                  cp.ActivityChildId = dr["ActivityChildId"].ToString();
                  cp.Ljdm = dr["Ljdm"].ToString();
                  cp.Ljxh = DrectSoft.Tool.ConvertMy.ToDecimal(dr["Ljxh"].ToString());


                  CPCollection.CP_PatientInOutRecordOutCollection.Add(cp);
               }
               #endregion

               #region 主要治疗流程护理记录
               //主要治疗流程护理记录
               foreach (DataRow dr in myDataSet.Tables[3].Rows)
               {
                  CP_TreatmentFlowInfo cp = new CP_TreatmentFlowInfo();
                  cp.Clrq = dr["Clrq"].ToString();
                  cp.Clsj = dr["Clsj"].ToString();
                  cp.Djrq = dr["Djrq"].ToString();
                  cp.Djys = dr["Djys"].ToString();
                  cp.Djysdm = dr["Djysdm"].ToString();
                  cp.Jlxh = dr["Jlxh"].ToString();
                  cp.Lcsm = dr["Lcsm"].ToString();
                  cp.Sfss = dr["Sfss"].ToString() == "True" ? 1 : 0;
                  cp.Zyhm = dr["Zyhm"].ToString();
                  cp.Zfrq = dr["Zfrq"].ToString();
                  cp.Zfry = dr["Zfry"].ToString();
                  cp.Zfrydm = dr["Zfrydm"].ToString();
                  cp.Zllc = dr["Zllc"].ToString();

                  cp.ActivityId = dr["ActivityId"].ToString();
                  cp.ActivityChildId = dr["ActivityChildId"].ToString();
                  cp.Ljdm = dr["Ljdm"].ToString();
                  cp.Ljxh = DrectSoft.Tool.ConvertMy.ToDecimal(dr["Ljxh"].ToString());

                  CPCollection.CP_TreatmentFlowCollection.Add(cp);
               }
               #endregion

               #region 特殊护理记录
               //特殊护理记录
               foreach (DataRow dr in myDataSet.Tables[4].Rows)
               {
                  CP_VitalSignSpecialRecordInfo cp = new CP_VitalSignSpecialRecordInfo();
                  cp.Clrq = dr["Clrq"].ToString();
                  cp.Clsj = dr["Clsj"].ToString();
                  cp.Djrq = dr["Djrq"].ToString();
                  cp.Djys = dr["Djys"].ToString();
                  cp.Djysdm = dr["Djysdm"].ToString();
                  cp.Hzfw = dr["Hzfw"].ToString();
                  cp.Hzgms = dr["Hzgms"].ToString();
                  cp.Hzsg = dr["Hzsg"].ToString();
                  cp.Hzsss = dr["Hzsss"].ToString();
                  cp.Hzsxs = dr["Hzsxs"].ToString();
                  cp.Hztz = dr["Hztz"].ToString();
                  cp.Hzxx = dr["Hzxx"].ToString();
                  cp.Hzxxdm = Convert.ToInt32(dr["Hzxxdm"].ToString());
                  cp.Jlxh = dr["Jlxh"].ToString();
                  cp.Zyhm = dr["Zyhm"].ToString();
                  cp.Xyxx = dr["Xyxx"].ToString();
                  cp.Xyxxdm = Convert.ToInt32(dr["Xyxxdm"].ToString());
                  cp.Zfrq = dr["Zfrq"].ToString();
                  cp.Zfry = dr["Zfry"].ToString();
                  cp.Zfrydm = dr["Zfrydm"].ToString();

                  cp.ActivityId = dr["ActivityId"].ToString();
                  cp.ActivityChildId = dr["ActivityChildId"].ToString();
                  cp.Ljdm = dr["Ljdm"].ToString();
                  cp.Ljxh = DrectSoft.Tool.ConvertMy.ToDecimal(dr["Ljxh"].ToString());

                  CPCollection.CP_VitalSignSpecialRecordCollection.Add(cp);
               }
               #endregion

               return CPCollection;
            }
            catch (Exception ex)
            {
               ThrowException(ex);
               return null;
            }

      }
        #endregion

        #region 作废一条护理记录
        /// <summary>
        /// 作废一条护理记录
        /// </summary>
        /// <param name="intType">作废记录属于哪个表</param>
        /// <param name="strJlxh">记录序号</param>
        /// <param name="strZfrydm">作废人职工代码</param>
        /// <param name="strZfry">作废人姓名</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public string NursingNotesRecordCancel(int intType, string strJlxh, string strZfrydm, string strZfry)
        {
            try
            {
                string strSql = "";
                if (intType == 1)//生命体征护理记录表
                {
                    strSql = "update CP_VitalSignsRecord ";
                }
                else if (intType == 2 || intType == 3)//病人入/出量护理记录表
                {
                    strSql = "update CP_PatientInOutRecord ";
                }
                else if (intType == 4)//病人治疗流程护理记录表
                {
                    strSql = "update CP_TreatmentFlow ";
                }
                else if (intType == 5)//病人护理特殊记录表
                {
                    strSql = "update CP_VitalSignSpecialRecord ";
                }
                else
                {
                    return "选择操作的记录数据无效!";
                }
                strSql = strSql + " set Zfrq=convert(char(19),getdate(),120),Zfrydm=@Zfrydm,Zfry=@Zfry where Jlxh=@Jlxh";

                SqlParameter[] param = new SqlParameter[]
                {
                    new SqlParameter("@Zfrydm",strZfrydm),
                    new SqlParameter("@Zfry",strZfry),
                    new SqlParameter("@Jlxh",strJlxh)
                };

                SqlHelper.ExecuteNoneQuery(strSql, param, CommandType.Text);

                return "操作完成!";

            }
            catch (Exception ex)
            {
                ThrowException(ex); return "操作失败!";

            }

        }
        #endregion
    }
}