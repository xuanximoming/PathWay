using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

using System.ServiceModel;
using System.Data.SqlClient;
using System.Data;
using YidanSoft.Tool;
using Yidansoft.Service.Entity;
using System.Xml.Linq;
using System.IO;
using System.Runtime.Serialization;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 路径XML
        /// </summary>
        public String WorkXML = "";
        /// <summary>
        /// 医嘱成套
        /// </summary>
        public List<CP_AdviceGroupPathCut> PathCutlist = new List<CP_AdviceGroupPathCut>();

        /// <summary>
        /// 插入路径(和路径条件)[附带路径病种]
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public String InsertCP_ClinicalPath(string strName, string strLjms, double zgts, double jcfy, double version, string strShys, int yxjl, string strSyks, String bzdm, String oldLjdm, String operation)
        {
            String strLjdm = "";
            SqlConnection myConnection = null;
            SqlTransaction sqlTrans = null;
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            //    SqlTransaction sqlTrans = null;
            try
            {

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@Name",strName),
                        new SqlParameter("@Ljms",strLjms),
                        new SqlParameter("@Zgts",zgts),
                        new SqlParameter("@Jcfy",jcfy),
                        new SqlParameter("@Vesion",version),
                        new SqlParameter("@Shys",strShys),
                        new SqlParameter("@Yxjl",yxjl),
                        new SqlParameter("@Syks",strSyks),
                        new SqlParameter("@Bzdm",bzdm),
                        new SqlParameter("@Operation",operation),
                    };



                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_PathCutInsert", parameters, CommandType.StoredProcedure);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != null)
                {
                    strLjdm = dt.Rows[0][0].ToString().Trim();
                }
                if (strLjdm != "0")                 //无同名
                {
                    InsertCPListDiagnoise(strLjdm, bzdm, "");
                    InsertCP_ClinicalPathCondition(oldLjdm, strLjdm, "LjCondition");
                }
                //sqlTrans.Commit();//事务提交   
            }
            catch (Exception ex)
            {
                //sqlTrans.Rollback();
                ThrowException(ex);
            }

            return strLjdm;
        }
        /// <summary>
        /// 插入路径进入条件
        /// </summary>
        public void InsertCP_ClinicalPathCondition(String oldLjdm, String newLjdm, String operation)
        {
            try
            {
                //SqlCommand myCommand = new SqlCommand("usp_CP_PathCutInsert", myConnection, sqlTrans);
                //myCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@OldLjdm",oldLjdm),
                    new SqlParameter("@NewLjdm",newLjdm),
                    new SqlParameter("@Operation",operation)
                };

                //myCommand.Parameters.Add("@OldLjdm", SqlDbType.VarChar, 50);
                //myCommand.Parameters.Add("@NewLjdm", SqlDbType.VarChar, 50);
                //myCommand.Parameters.Add("@Operation", SqlDbType.VarChar, 50);
                //myCommand.Parameters["@OldLjdm"].Value = oldLjdm;
                //myCommand.Parameters["@NewLjdm"].Value = newLjdm;
                //myCommand.Parameters["@Operation"].Value = operation;

                //myCommand.ExecuteNoneQuery();

                SqlHelper.ExecuteDataTable("usp_CP_PathCutInsert", parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
        }

        /// <summary>
        /// 更新路径XML,插入节点明细，节点条件，节点护理，节点变异（和医嘱LIST）
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public CP_PathCutEdit CutXML(string oldXml, String newLjdm, String newLjmc, String oldLjdm, String oldLjmc)
        {
            CP_PathCutEdit pathCutEdit = new CP_PathCutEdit();

            List<MyActivity> MyActivitysTemp = AnalyseAndReplaceFlowXMLNode(oldXml, newLjdm, newLjmc, oldLjdm, oldLjmc);               //更新XML

            SqlConnection myConnection = null;
            SqlTransaction sqlTrans = null;
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            //SqlTransaction sqlTrans = null;
            try
            {


                string strSql = @"update CP_ClinicalPath set WorkFlowXML = '{0}' where Ljdm = '{1}'";
                string strCmd = string.Format(strSql, ZipContent(WorkXML), newLjdm);
                SqlHelper.ExecuteNoneQuery(strCmd);


                foreach (MyActivity node in MyActivitysTemp)                                        //开始插入节点明细，节点条件，节点护理，节点变异，产生医嘱LIST
                {
                    InsertPathDetail(sqlTrans, myConnection, node, newLjdm, "Jd");                      //节点明细
                    InsertPathDetailCondition(sqlTrans, myConnection, "JdCondition", node, newLjdm);     //节点进入条件
                    InsertPathDetailNur(sqlTrans, myConnection, "JdNur", node, newLjdm);                 //节点护理
                    InsertPathDetailVar(sqlTrans, myConnection, "JdVar", node, newLjdm);                 //节点异常
                    GetAdviceGroup(sqlTrans, myConnection, "Get", node, newLjdm);                       //获取医嘱
                }

                // sqlTrans.Commit();
            }
            catch (Exception ex)
            {
                // sqlTrans.Rollback();
                ThrowException(ex);
            }


            pathCutEdit.Xml = WorkXML;
            pathCutEdit.CP_AdviceGroupPathCutList = PathCutlist;

            return pathCutEdit;
        }
        /// <summary>
        /// 分析流程图XML获取每个节点的类型、UniqueID、前置和后置节点UniqueID,
        /// 同时替换所有的UniqueID,复制一个全新的路径和XML
        /// 6.2最新添加方法，在原有分析基础上加入替换
        /// </summary>
        /// <param name="XMLStr"></param>
        /// <returns></returns>
        public MyActivitys AnalyseAndReplaceFlowXMLNode(String oldXml, String newLjdm, String newLjmc, String oldLjdm, String oldLjmc)
        {
            oldXml = oldXml.Replace(oldLjdm, newLjdm);           //替换Ljdm
            oldXml = oldXml.Replace(oldLjmc, newLjmc);           //替换Ljmc

            Byte[] b = System.Text.UTF8Encoding.UTF8.GetBytes(oldXml);
            XElement xele = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(b)));

            string flowId = xele.Attribute(XName.Get("ID")).Value;      //获取ID
            oldXml = oldXml.Replace(flowId, Guid.NewGuid().ToString());          //NEW新值

            var partNos = from item in xele.Descendants("Activity") select item;

            //开始节点
            MyActivity ActivityBegin = new MyActivity();
            //结束节点
            MyActivity ActivityEnd = new MyActivity();
            //全部节点
            MyActivitys MyActivityAll = new MyActivitys();
            foreach (var node in partNos)
            {
                MyActivity a = new MyActivity();
                a.ActivityType = node.Attribute(XName.Get("Type")).Value;
                a.ActivityName = node.Attribute("ActivityName").Value;
                a.CurrentElementState = node.Attribute("CurrentElementState").Value;


                a.OldUniqueID = node.Attribute("UniqueID").Value;                   //原值
                a.UniqueID = Guid.NewGuid().ToString();                             //NEW新值
                oldXml = oldXml.Replace(a.OldUniqueID, a.UniqueID);                          //等于一次性替换掉UniqueID,子节点UniqueID,线条规则的BeginUniqueID和EndUniqueID


                if (a.ActivityType == "INITIAL")
                {
                    ActivityBegin = a;
                }
                if (a.ActivityType == "COMPLETION")
                {
                    ActivityEnd = a;
                }
                MyActivityAll.Add(a);
            }

            var childrens = from item in xele.Descendants("ActiveChildren") select item;                //读取子节点
            foreach (var children in childrens)                                                          //遍历子节点
            {
                MyActiveChildren c = new MyActiveChildren();
                c.ActivityChildrenID = Guid.NewGuid().ToString();                                       //NEW新值
                oldXml = oldXml.Replace((children.Attribute("ActivityChildrenID").Value), c.ActivityChildrenID);  //替换ActivityChildrenID
            }

            Byte[] Loadb = System.Text.UTF8Encoding.UTF8.GetBytes(oldXml);                                  //重载XML
            XElement Loadxele = XElement.Load(System.Xml.XmlReader.Create(new MemoryStream(Loadb)));        //重载单元
            var Rules = from item in Loadxele.Descendants("Rule") select item;
            //全部规则
            List<MyRule> RulesAll = new List<MyRule>();
            foreach (var item in Rules)
            {
                MyRule ruleTemp = new MyRule();
                ruleTemp.BeginNodeUniqueID = item.Attribute("BeginActivityUniqueID").Value;             // （已重载）
                ruleTemp.EndNodeUniqueID = item.Attribute("EndActivityUniqueID").Value;                 // （已重载）

                oldXml = oldXml.Replace((item.Attribute("UniqueID").Value), Guid.NewGuid().ToString());          //替换UniqueID
                oldXml = oldXml.Replace((item.Attribute("RuleID").Value), Guid.NewGuid().ToString());            //替换RuleID

                RulesAll.Add(ruleTemp);
            }
            MyRule.MyRuleALL = RulesAll;
            MyActivitys ActivitysResult = new MyActivitys();

            #region 遍历规则算法

            //遍历所有规则
            foreach (var item in RulesAll)
            {
                //添加头节点
                if (item.BeginNodeUniqueID == ActivityBegin.UniqueID)
                {
                    ActivityBegin.NestNodeUniqueID = item.EndNodeUniqueID;
                    ActivitysResult.Add(ActivityBegin);
                }
                //添加尾节点
                if (item.EndNodeUniqueID == ActivityEnd.UniqueID)
                {
                    ActivityEnd.PreNodeUniqueID = item.BeginNodeUniqueID;
                    ActivitysResult.Add(ActivityEnd);
                }
                foreach (var item2 in RulesAll)
                {
                    //如果两条规则的头尾相接，那么说明两条规则连接处的节点有前置和后置节点
                    if (item.EndNodeUniqueID == item2.BeginNodeUniqueID)
                    {
                        MyActivity MyActivityTemp = new MyActivity();
                        MyActivityTemp.UniqueID = item.EndNodeUniqueID;
                        MyActivityTemp.PreNodeUniqueID = item.BeginNodeUniqueID;
                        MyActivityTemp.NestNodeUniqueID = item2.EndNodeUniqueID;
                        MyActivityTemp.OldUniqueID = (MyActivityAll.Select(s => s).Where(s => s.UniqueID == MyActivityTemp.UniqueID).ToList())[0].OldUniqueID;          //原ID
                        MyActivityTemp.ActivityType = (MyActivityAll.Select(s => s).Where(s => s.UniqueID == MyActivityTemp.UniqueID).ToList())[0].ActivityType;
                        MyActivityTemp.ActivityName = (MyActivityAll.Select(s => s).Where(s => s.UniqueID == MyActivityTemp.UniqueID).ToList())[0].ActivityName;
                        MyActivityTemp.CurrentElementState = (MyActivityAll.Select(s => s).Where(s => s.UniqueID == MyActivityTemp.UniqueID).ToList())[0].CurrentElementState;
                        ActivitysResult.Add(MyActivityTemp);
                    }
                }


            }
            #endregion
            WorkXML = oldXml;           //更新XML

            return ActivitysResult;
        }
        /// <summary>
        /// 插入节点
        /// </summary>
        public void InsertPathDetail(SqlTransaction sqlTrans, SqlConnection myConnection, MyActivity node, String newLjdm, String operation)
        {
            try
            {
                //SqlCommand myCommand = new SqlCommand("usp_CP_PathCutInsert", myConnection, sqlTrans);
                //myCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@UniqueID",node.UniqueID),
                    new SqlParameter("@NewLjdm",newLjdm),
                    new SqlParameter("@ActivityName",node.ActivityName),
                    new SqlParameter("@PreNodeUniqueID",node.PreNodeUniqueID),
                    new SqlParameter("@NestNodeUniqueID",node.NestNodeUniqueID),
                    new SqlParameter("@ActivityType",node.ActivityType),
                    new SqlParameter("@Operation",operation)
                };



                SqlHelper.ExecuteDataTable("usp_CP_PathCutInsert", parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
        }
        /// <summary>
        /// 节点进入条件
        /// </summary>
        public void InsertPathDetailCondition(SqlTransaction sqlTrans, SqlConnection myConnection, String operation, MyActivity node, String newLjdm)
        {
            try
            {
                //SqlCommand myCommand = new SqlCommand("usp_CP_PathCutInsert", myConnection, sqlTrans);
                //myCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@OldWorkFlow",node.OldUniqueID),
                    new SqlParameter("@NewWorkFlow",node.UniqueID),
                    new SqlParameter("@Operation",operation),
                    new SqlParameter("@NewLjdm",newLjdm)
                };


                SqlHelper.ExecuteDataTable("usp_CP_PathCutInsert", parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }

        }
        /// <summary>
        /// 节点护理
        /// </summary>
        public void InsertPathDetailNur(SqlTransaction sqlTrans, SqlConnection myConnection, String operation, MyActivity node, String newLjdm)
        {
            try
            {
                //SqlCommand myCommand = new SqlCommand("usp_CP_PathCutInsert", myConnection, sqlTrans);
                //myCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@OldWorkFlow",node.OldUniqueID),
                    new SqlParameter("@NewWorkFlow",node.UniqueID),
                    new SqlParameter("@Operation",operation),
                    new SqlParameter("@NewLjdm",newLjdm)
                };


                SqlHelper.ExecuteDataTable("usp_CP_PathCutInsert", parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
        }
        /// <summary>
        /// 节点异常
        /// </summary>
        public void InsertPathDetailVar(SqlTransaction sqlTrans, SqlConnection myConnection, String operation, MyActivity node, String newLjdm)
        {
            try
            {
                //SqlCommand myCommand = new SqlCommand("usp_CP_PathCutInsert", myConnection, sqlTrans);
                //myCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@OldWorkFlow",node.OldUniqueID),
                    new SqlParameter("@NewWorkFlow",node.UniqueID),
                    new SqlParameter("@Operation",operation),
                    new SqlParameter("@NewLjdm",newLjdm)
                };

                ;

                SqlHelper.ExecuteDataTable("usp_CP_PathCutInsert", parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }

        }
        /// <summary>
        /// 医嘱
        /// </summary>
        public void GetAdviceGroup(SqlTransaction sqlTrans, SqlConnection myConnection, String operation, MyActivity node, String newLjdm)
        {
            try
            {


                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@OldWorkFlowId",node.OldUniqueID),
                    new SqlParameter("@NewWorkFlowId",node.UniqueID),
                    new SqlParameter("@NewLjdm",newLjdm),
                    new SqlParameter("@Operation",operation)
                };

                ;

                DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_PathCutEditAdviceGroup", parameters, CommandType.StoredProcedure);

                if (ds.Tables.Count > 0)            //很重要,是否存在
                {
                    foreach (DataRow item in ds.Tables[0].Rows)
                    {
                        CP_AdviceGroupPathCut path = new CP_AdviceGroupPathCut();

                        path.ActivityId = ConvertMy.ToString(item["ActivityId"]);
                        path.Cdxh = ConvertMy.ToString(item["Cdxh"]);
                        path.Counts = ConvertMy.ToInt32(item["Counts"]);
                        path.Ctmxxh = ConvertMy.ToDecimal(item["Ctmxxh"]);
                        path.Dwlb = ConvertMy.ToString(item["Dwlb"]);
                        path.Dwxs = ConvertMy.ToString(item["Dwxs"]);
                        path.Fzbz = ConvertMy.ToString(item["Fzbz"]);
                        path.Fzxh = ConvertMy.ToString(item["Fzxh"]);
                        path.Ggxh = ConvertMy.ToString(item["Ggxh"]);
                        path.Jldw = ConvertMy.ToString(item["Jldw"]);
                        path.Lcxh = ConvertMy.ToString(item["Lcxh"]);
                        path.Ljdm = ConvertMy.ToString(item["Ljdm"]);
                        path.Pcdm = ConvertMy.ToString(item["Pcdm"]);
                        path.Xmlb = ConvertMy.ToString(item["Xmlb"]);
                        path.Yfdm = ConvertMy.ToString(item["Yfdm"]);
                        path.Ypdm = ConvertMy.ToString(item["Ypdm"]);
                        path.Ypjl = ConvertMy.ToString(item["Ypjl"]);
                        path.Ypmc = ConvertMy.ToString(item["Ypmc"]);
                        path.Yzbz = ConvertMy.ToString(item["Yzbz"]);
                        path.Yzlb = ConvertMy.ToString(item["Yzlb"]);
                        path.Zdm = ConvertMy.ToString(item["Zdm"]);
                        path.Ztnr = ConvertMy.ToString(item["Ztnr"]);
                        path.Zxcs = ConvertMy.ToString(item["Zxcs"]);
                        path.Zxdw = ConvertMy.ToString(item["Zxdw"]);
                        path.Zxsj = ConvertMy.ToString(item["Zxsj"]);
                        path.Zxzq = ConvertMy.ToString(item["Zxzq"]);
                        path.Zxzqdw = ConvertMy.ToString(item["Zxzqdw"]);
                        path.YzbzName = ConvertMy.ToString(item["YzbzName"]);
                        path.Yznr = ConvertMy.ToString(item["Yznr"]);

                        PathCutlist.Add(path);
                    }
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
        }

        /// <summary>
        /// 删除明细
        /// </summary>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public void DeleteAdviceGroupDetail(List<CP_AdviceGroupPathCut> advicePathCut, String newWorkFlowId, String operation, String newLjmc)
        {
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            //    SqlTransaction sqlTrans = null;
            SqlConnection myConnection = null;
            SqlTransaction sqlTrans = null;
            try
            {
                //myConnection.Open();
                //sqlTrans = myConnection.BeginTransaction();
                //SqlCommand myCommand = new SqlCommand("usp_CP_PathCutEditAdviceGroup", myConnection, sqlTrans);
                //myCommand.CommandType = CommandType.StoredProcedure;

                SqlParameter[] parameters = new SqlParameter[] 
                    {
                        new SqlParameter("@NewWorkFlowId",newWorkFlowId),
                        new SqlParameter("@Operation",operation)
                    };



                SqlHelper.ExecuteDataTable("usp_CP_PathCutEditAdviceGroup", parameters, CommandType.StoredProcedure);

                DeleteAdviceGroup(sqlTrans, myConnection, newWorkFlowId, "Del");

                foreach (CP_AdviceGroupPathCut item in advicePathCut)
                {
                    Decimal Ctyzxh = InsertAdviceGroup(sqlTrans, myConnection, item, "Insert", newLjmc);
                    InsertAdviceGroupDetail(sqlTrans, myConnection, Ctyzxh, item, "InsertDetail");
                }

            }
            catch (Exception ex)
            {
                //sqlTrans.Rollback();
                ThrowException(ex);
            }

        }
        /// <summary>
        /// 删除成套
        /// </summary>
        public void DeleteAdviceGroup(SqlTransaction sqlTrans, SqlConnection myConnection, String newWorkFlowId, String operation)
        {
            try
            {


                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@NewWorkFlowId",newWorkFlowId),
                    new SqlParameter("@Operation",operation)
                };


                SqlHelper.ExecuteDataTable("usp_CP_PathCutEditAdviceGroup", parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
        }
        /// <summary>
        /// 插入成套
        /// </summary>
        public Decimal InsertAdviceGroup(SqlTransaction sqlTrans, SqlConnection myConnection, CP_AdviceGroupPathCut advicePathCut, String operation, String newLjmc)
        {
            Decimal Ctyzxh = 0;
            try
            {


                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@NewWorkFlowId",advicePathCut.ActivityId),
                    new SqlParameter("@Name",newLjmc),
                    new SqlParameter("@NewLjdm",advicePathCut.Ljdm),
                    new SqlParameter("@Operation",operation)
                };



                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_PathCutEditAdviceGroup", parameters);
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != null)
                {
                    Ctyzxh = ConvertMy.ToDecimal(dt.Rows[0][0].ToString().Trim());
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return Ctyzxh;
        }
        /// <summary>
        /// 插入成套明细
        /// </summary>
        public void InsertAdviceGroupDetail(SqlTransaction sqlTrans, SqlConnection myConnection, Decimal ctyzxh, CP_AdviceGroupPathCut adviceDetail, String operation)
        {
            try
            {

                #region 参数

                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@Ctyzxh",ctyzxh),
                    new SqlParameter("@Yzbz",adviceDetail.Yzbz),
                    new SqlParameter("@Fzxh",adviceDetail.Fzxh),
                    new SqlParameter("@Fzbz",adviceDetail.Fzbz),
                    new SqlParameter("@Cdxh",adviceDetail.Cdxh),
                    new SqlParameter("@Ggxh",adviceDetail.Ggxh),
                    new SqlParameter("@Lcxh",adviceDetail.Lcxh),
                    new SqlParameter("@Ypdm",adviceDetail.Ypdm),
                    new SqlParameter("@Ypmc",adviceDetail.Ypmc),
                    new SqlParameter("@Xmlb",adviceDetail.Xmlb),
                    new SqlParameter("@Zxdw",adviceDetail.Zxdw),
                    new SqlParameter("@Ypjl",adviceDetail.Ypjl),
                    new SqlParameter("@Jldw",adviceDetail.Jldw),
                    new SqlParameter("@Dwxs",adviceDetail.Dwxs),
                    new SqlParameter("@Dwlb",adviceDetail.Dwlb),
                    new SqlParameter("@Yfdm",adviceDetail.Yfdm),
                    new SqlParameter("@Pcdm",adviceDetail.Pcdm),
                    new SqlParameter("@Zxcs",adviceDetail.Zxcs),
                    new SqlParameter("@Zxzq",adviceDetail.Zxzq),
                    new SqlParameter("@Zxzqdw",adviceDetail.Zxzqdw),
                    new SqlParameter("@Zdm",adviceDetail.Zdm),
                    new SqlParameter("@Zxsj",adviceDetail.Zxsj),
                    new SqlParameter("@Ztnr",adviceDetail.Ztnr),
                    new SqlParameter("@Yzlb",adviceDetail.Yzlb),
                    new SqlParameter("@Operation",operation),
                };


                #endregion


                SqlHelper.ExecuteDataTable("usp_CP_PathCutEditAdviceGroup", parameters, CommandType.StoredProcedure);
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
        }
    }

    [DataContract()]
    public class CP_PathCutEdit
    {
        [DataMember()]
        public String Xml
        {
            get;
            set;
        }
        [DataMember()]
        public List<CP_AdviceGroupPathCut> CP_AdviceGroupPathCutList
        {
            get;
            set;
        }
    }
    [DataContract()]
    public class CP_AdviceGroupPathCut
    {
        /// <summary>
        /// 成套医嘱明细序号（标识）
        /// </summary>
        [DataMember()]
        public Decimal Ctmxxh
        {
            get;
            set;
        }
        /// <summary>
        /// 路径代码
        /// </summary>
        [DataMember()]
        public String Ljdm
        {
            get;
            set;
        }
        /// <summary>
        /// 节点代码
        /// </summary>
        [DataMember()]
        public String ActivityId
        {
            get;
            set;
        }
        [DataMember()]
        public String Yzbz
        {
            get;
            set;
        }
        [DataMember()]
        public String Fzxh
        {
            get;
            set;
        }
        [DataMember()]
        public String Fzbz
        {
            get;
            set;
        }
        [DataMember()]
        public String Cdxh
        {
            get;
            set;
        }
        [DataMember()]
        public String Ggxh
        {
            get;
            set;
        }
        [DataMember()]
        public String Lcxh
        {
            get;
            set;
        }
        [DataMember()]
        public String Ypdm
        {
            get;
            set;
        }
        [DataMember()]
        public String Ypmc
        {
            get;
            set;
        }
        [DataMember()]
        public String Xmlb
        {
            get;
            set;
        }
        [DataMember()]
        public String Zxdw
        {
            get;
            set;
        }
        [DataMember()]
        public String Ypjl
        {
            get;
            set;
        }
        [DataMember()]
        public String Jldw
        {
            get;
            set;
        }
        [DataMember()]
        public String Dwxs
        {
            get;
            set;
        }
        [DataMember()]
        public String Dwlb
        {
            get;
            set;
        }
        [DataMember()]
        public String Yfdm
        {
            get;
            set;
        }
        [DataMember()]
        public String Pcdm
        {
            get;
            set;
        }
        [DataMember()]
        public String Zxcs
        {
            get;
            set;
        }
        [DataMember()]
        public String Zxzq
        {
            get;
            set;
        }
        [DataMember()]
        public String Zxzqdw
        {
            get;
            set;
        }
        [DataMember()]
        public String Zdm
        {
            get;
            set;
        }
        [DataMember()]
        public String Zxsj
        {
            get;
            set;
        }
        [DataMember()]
        public String Ztnr
        {
            get;
            set;
        }
        [DataMember()]
        public String Yzlb
        {
            get;
            set;
        }
        [DataMember()]
        public Int32 Counts
        {
            get;
            set;
        }
        /// <summary>
        /// 自定义变量
        /// </summary>
        [DataMember()]
        public String YzbzName
        {
            get;
            set;
        }
        /// <summary>
        /// 自定义变量
        /// </summary>
        [DataMember()]
        public String Yznr
        {
            get;
            set;
        }

    }
}