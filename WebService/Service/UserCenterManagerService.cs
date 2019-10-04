using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Data.SqlClient;
using System.Data;
using Yidansoft.Service.Entity;
using System.Collections;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Configuration;
using DrectSoft.Tool;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        #region 用户中心
        /// <summary>
        /// 获取病历时限信息表
        /// </summary>
        /// <param name="FoulState">0查询提示信息，1查询警告信息</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<V_QCRecord> GetV_QCRecordList(String FoulState)
        {
            #region 模拟数据源代码
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionStringEMR))
            //{
                List<V_QCRecord> NoteCompareList = new List<V_QCRecord>();
                try
                {
                    //DataTable dataTable = new DataTable();
                    StringBuilder sql = new StringBuilder();
                    #region sql
                    if (FoulState == "0")
                    {
                        sql.Append(@" 
select  '黄世仁' InPatientName ,'0' FoulState ,'病例到期还有5小时' MessageInfo from dual
 Union select  '小三1' InPatientName ,'0' FoulState ,'病例到期还有6小时' MessageInfo from dual
 Union select  '黄三爷' InPatientName ,'0' FoulState ,'病例到期还有7小时' MessageInfo from dual
 Union select  '三四' InPatientName ,'0' FoulState ,'病例到期还有8小时' MessageInfo from dual
 Union select  '王晓峰' InPatientName ,'0' FoulState ,'病例到期还有9小时' MessageInfo from dual
 Union select  '徐西苑' InPatientName ,'0' FoulState ,'病例到期还有10小时' MessageInfo from dual
 Union select  '糊涂' InPatientName ,'0' FoulState ,'病例到期还有11小时' MessageInfo from dual
 Union select  '和胖子' InPatientName ,'0' FoulState ,'病例到期还有12小时' MessageInfo from dual
 Union select  '黄三爷' InPatientName ,'0' FoulState ,'病例到期还有8小时' MessageInfo from dual
 Union select  '汪四明' InPatientName ,'0' FoulState ,'病例到期还有3小时' MessageInfo from dual
 Union select  '李刚' InPatientName ,'0' FoulState ,'病例到期还有4小时' MessageInfo from dual
 Union select  '狄怀英' InPatientName ,'0' FoulState ,'病例到期还有5小时' MessageInfo from dual
 Union select  '巩俐' InPatientName ,'0' FoulState ,'病例到期还有6小时' MessageInfo from dual
 Union select  '张三' InPatientName ,'0' FoulState ,'病例到期还有7小时' MessageInfo from dual
 Union select  '李军' InPatientName ,'0' FoulState ,'病例到期还有1小时' MessageInfo from dual
 Union select  '李冰' InPatientName ,'0' FoulState ,'病例到期还有2小时' MessageInfo from dual
 Union select  '历史' InPatientName ,'0' FoulState ,'病例到期还有3小时' MessageInfo from dual
 Union select  '徐鸣' InPatientName ,'0' FoulState ,'病例到期还有4小时' MessageInfo from dual
 Union select  '黄公民' InPatientName ,'0' FoulState ,'病例到期还有5小时' MessageInfo from dual
 Union select  '黄世人' InPatientName ,'0' FoulState ,'病例到期还有6小时' MessageInfo from dual");
                    }
                    //sql.AppendFormat(@"select * from V_QCRecord where 1=1 and FoulState='{0}'", FoulState);
                    if (FoulState == "1")
                    {
                        sql.Append(@"  select  '严雨梅' InPatientName ,'1' FoulState ,'病例已过期5小时' MessageInfo from dual
 Union select  '陈慧' InPatientName ,'1' FoulState ,'病例已过期6小时' MessageInfo from dual
 Union select  '汤英' InPatientName ,'1' FoulState ,'病例已过期7小时' MessageInfo from dual
 Union select  '肖林峰' InPatientName ,'1' FoulState ,'病例已过期8小时' MessageInfo from dual
 Union select  '张一凡' InPatientName ,'1' FoulState ,'病例已过期9小时' MessageInfo from dual
 Union select  '王天民' InPatientName ,'1' FoulState ,'病例已过期10小时' MessageInfo from dual
 Union select  '汤英' InPatientName ,'1' FoulState ,'病例已过期11小时' MessageInfo from dual
 Union select  '祝明霞' InPatientName ,'1' FoulState ,'病例已过期12小时' MessageInfo from dual
 Union select  '舟样' InPatientName ,'1' FoulState ,'病例已过期8小时' MessageInfo from dual
 Union select  '周舟' InPatientName ,'1' FoulState ,'病例已过期3小时' MessageInfo from dual
 Union select  '明道' InPatientName ,'1' FoulState ,'病例已过期4小时' MessageInfo from dual
 Union select  '梅村' InPatientName ,'1' FoulState ,'病例已过期5小时' MessageInfo from dual
 Union select  '周梅村' InPatientName ,'1' FoulState ,'病例已过期6小时' MessageInfo from dual
 Union select  '周金海' InPatientName ,'1' FoulState ,'病例已过期7小时' MessageInfo from dual
 Union select  '江明' InPatientName ,'1' FoulState ,'病例已过期1小时' MessageInfo from dual
 Union select  '祝英' InPatientName ,'1' FoulState ,'病例已过期2小时' MessageInfo from dual
 Union select  '张一凡' InPatientName ,'1' FoulState ,'病例已过期3小时' MessageInfo from dual
 Union select  '汤英' InPatientName ,'1' FoulState ,'病例已过期4小时' MessageInfo from dual
 Union select  '黄煌' InPatientName ,'1' FoulState ,'病例已过期5小时' MessageInfo from dual");
                    }
                    #endregion


                    DataTable dataTable = SqlHelper.ExecuteDataTable( sql.ToString());

                    foreach (DataRow row in dataTable.Rows)
                    {
                        V_QCRecord NoteCompare = new V_QCRecord();
                        //NoteCompare.FoulState = row["FoulState"];
                        NoteCompare.InPatientName = row["InPatientName"].ToString();
                        NoteCompare.MessageInfo = row["MessageInfo"].ToString();

                        NoteCompareList.Add(NoteCompare);
                    }

                    return NoteCompareList;
                }
                catch (Exception ex)
                {
                    ThrowException(ex);
                    return NoteCompareList;
                }
            #endregion

        }
        #endregion
    }
}