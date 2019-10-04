using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ServiceModel;
using System.Data;
using System.Data.SqlClient;
using YidanSoft.Tool;
using Yidansoft.Service.Entity;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {


        [OperationContract]
        [FaultContract(typeof(LoginException))]  //be modified by rjd (add '@cycw') 2011/1/27
        public List<CP_InpatinetList> GetInpatientList(int querykind, string strDept, string Zyys, string hzxm, string zyhm, string cycw, string ksrq, string jsrq, string brzt, string lgzt, int PageSize, int CurrentPage)
        {

            List<CP_InpatinetList> inpatinetList = new List<CP_InpatinetList>();
            try
            {
                SqlParameter querykindparam = new SqlParameter("@querykind", SqlDbType.VarChar, 12);
                querykindparam.Value = querykind.ToString();

                SqlParameter deptparam = new SqlParameter("@dept", SqlDbType.VarChar, 12);
                deptparam.Value = strDept;
                SqlParameter zyysparam = new SqlParameter("@zyys", SqlDbType.VarChar, 12);
                zyysparam.Value = Zyys;
                SqlParameter hzxmparam = new SqlParameter("@hzxm", SqlDbType.VarChar, 12);
                hzxmparam.Value = hzxm;

                SqlParameter zyhmparam = new SqlParameter("@zyhm", SqlDbType.VarChar, 12);
                zyhmparam.Value = zyhm;

                SqlParameter cycwparam = new SqlParameter("@cycw", SqlDbType.VarChar, 12);
                cycwparam.Value = cycw;
                SqlParameter ksrqparam = new SqlParameter("@ksrq", SqlDbType.VarChar, 19);
                ksrqparam.Value = ksrq;
                SqlParameter jsrqparam = new SqlParameter("@jsrq", SqlDbType.VarChar, 19);
                jsrqparam.Value = jsrq;
                //add by luff 20130305 添加病人状态作为参数
                SqlParameter brztparam = new SqlParameter("@brzt", SqlDbType.VarChar, 50);
                brztparam.Value = brzt;
                SqlParameter lgztparam = new SqlParameter("@lgzt", SqlDbType.VarChar, 19);
                lgztparam.Value = lgzt;
                SqlParameter pagesizem = new SqlParameter("@PageSize", SqlDbType.Int);
                pagesizem.Value = PageSize;

                SqlParameter currentpagem = new SqlParameter("@CurrentPage", SqlDbType.Int);
                currentpagem.Value = CurrentPage;
                SqlParameter[] sqlparams = new SqlParameter[] { querykindparam, deptparam, zyysparam, hzxmparam, zyhmparam, cycwparam, ksrqparam, jsrqparam,brztparam, lgztparam, pagesizem, currentpagem };


                DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_InpatientList", sqlparams, CommandType.StoredProcedure);



                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    CP_InpatinetList inp = new CP_InpatinetList(row["Syxh"].ToString(), row["Hissyxh"].ToString(), row["Zyhm"].ToString(),
                        row["Hzxm"].ToString(), row["Brxb"].ToString(), row["Xsnl"].ToString(), row["Hkdz"].ToString(),
                        row["Ryrq"].ToString(), row["Brzt"].ToString(), row["Ryzd"].ToString(), row["Ljzt"].ToString(),
                        row["Pgqk"].ToString(), row["Ljmc"].ToString(), row["LjztName"].ToString(), row["LqljId"].ToString(),
                        row["Cycw"].ToString(), row["Csrq"].ToString(), row["Ljdm"].ToString(), row["Ljts"].ToString(), row["RydmLjdm"].ToString(),
                        row["Zyys"].ToString(), row["ZyysDm"].ToString(), Util.UnzipContent(row["WorkFlowXML"].ToString()), Util.UnzipContent(row["EnFroceXml"].ToString()),
                        row["NoOfRecord"].ToString(), row["patID"].ToString(), row["NoofClinic"].ToString(), row["Status"].ToString(), row["InCount"].ToString());
                    inp.RyzdCode = row["RyzdCode"].ToString();
                    inp.BhljId = ConvertMy.ToDecimal(row["BhljId"].ToString());
                    inp.Cyks = row["Cyks"].ToString();
                    inp.CyksName = row["CyksName"].ToString();
                    inp.Cybq = row["Cybq"].ToString();
                    inp.CybqName = row["CybqName"].ToString();
                    inp.Wzjb = row["Wzjb"].ToString();
                    inp.Cycw = row["Cycw"].ToString();
                    inp.Cqrq = row["Cqrq"].ToString();
                    inp.Cyrq = row["Cyrq"].ToString();
                    inp.Cyzd = row["Cyzd"].ToString();
                    inp.CyzdName = row["CyzdName"].ToString();
                    inp.Ljxh = ConvertMy.ToDecimal(row["Ljxh"].ToString());

                    inp.Status = row["Status"].ToString();
                    inp.Ljzt = row["Ljzt"].ToString();
                    inp.LjztName =  row["LjztName"].ToString();

                    inpatinetList.Add(inp);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return inpatinetList;
        }

        [OperationContract]
        [FaultContract(typeof(LoginException))]  //be modified by rjd (add '@cycw') 2011/1/27
        public string GetInpatientCount(int querykind, string strDept, string Zyys, string hzxm, string zyhm, string cycw, string ksrq, string jsrq, string brzt, string lgzt) 
        {
            string  count="";
            try
            {
                SqlParameter querykindparam = new SqlParameter("@querykind", SqlDbType.Int);
                querykindparam.Value = querykind;

                SqlParameter deptparam = new SqlParameter("@dept", SqlDbType.VarChar, 12);
                deptparam.Value = strDept;
                SqlParameter zyysparam = new SqlParameter("@zyys", SqlDbType.VarChar, 12);
                zyysparam.Value = Zyys;
                SqlParameter hzxmparam = new SqlParameter("@hzxm", SqlDbType.VarChar, 12);
                hzxmparam.Value = hzxm;

                SqlParameter zyhmparam = new SqlParameter("@zyhm", SqlDbType.VarChar, 12);
                zyhmparam.Value = zyhm;

                SqlParameter cycwparam = new SqlParameter("@cycw", SqlDbType.VarChar, 12);
                cycwparam.Value = cycw;
                SqlParameter ksrqparam = new SqlParameter("@ksrq", SqlDbType.VarChar, 19);
                ksrqparam.Value = ksrq;
                SqlParameter jsrqparam = new SqlParameter("@jsrq", SqlDbType.VarChar, 19);
                jsrqparam.Value = jsrq;

                //add by luff 20130305 添加病人状态作为参数
                SqlParameter brztparam = new SqlParameter("@brzt", SqlDbType.VarChar, 50);
                brztparam.Value = brzt;

                SqlParameter lgztparam = new SqlParameter("@lgzt", SqlDbType.VarChar, 19);
                lgztparam.Value = lgzt;

                SqlParameter[] sqlparams = new SqlParameter[] { querykindparam, deptparam, zyysparam, hzxmparam, zyhmparam, cycwparam, ksrqparam, jsrqparam,brztparam, lgztparam };


                DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_InpatientListCount", sqlparams, CommandType.StoredProcedure);
                count = ds.Tables[0].Rows[0][0] + "," + ds.Tables[0].Rows[1][0] + "," + ds.Tables[0].Rows[2][0] + "," + ds.Tables[0].Rows[3][0] + "," + ds.Tables[0].Rows[4][0] + "," + ds.Tables[0].Rows[5][0];
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return count;
        }

        //[OperationContract]
        //[FaultContract(typeof(LoginException))]  //be modified by rjd (add '@cycw') 2011/1/27
        //public List<CP_InpatinetList> GetInpatientList(string strDept, string ksrq, string jsrq, string Zyys, string hzxm, string zyhm, string cycw, int brzt)
        //{

        //    List<CP_InpatinetList> inpatinetList = new List<CP_InpatinetList>();
        //    try
        //    {
        //        SqlParameter querykindparam = new SqlParameter("@brzt", SqlDbType.Int);
        //        querykindparam.Value = brzt.ToString();

        //        SqlParameter deptparam = new SqlParameter("@dept", SqlDbType.VarChar, 12);
        //        deptparam.Value = strDept;
        //        SqlParameter ksrqparam = new SqlParameter("@ksrq", SqlDbType.VarChar, 19);
        //        ksrqparam.Value = ksrq;
        //        SqlParameter jsrqparam = new SqlParameter("@jsrq", SqlDbType.VarChar, 19);
        //        jsrqparam.Value = jsrq;
        //        SqlParameter zyysparam = new SqlParameter("@zyys", SqlDbType.VarChar, 12);
        //        zyysparam.Value = Zyys;
        //        SqlParameter hzxmparam = new SqlParameter("@hzxm", SqlDbType.VarChar, 12);
        //        hzxmparam.Value = hzxm;

        //        SqlParameter zyhmparam = new SqlParameter("@zyhm", SqlDbType.VarChar, 12);
        //        zyhmparam.Value = zyhm;

        //        SqlParameter cycwparam = new SqlParameter("@cycw", SqlDbType.VarChar, 12);
        //        cycwparam.Value = cycw;

        //        SqlParameter[] sqlparams = new SqlParameter[] { querykindparam, deptparam, ksrqparam, jsrqparam };


        //        DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_InpatientList", sqlparams, CommandType.StoredProcedure);



        //        foreach (DataRow row in ds.Tables[0].Rows)
        //        {
        //            CP_InpatinetList inp = new CP_InpatinetList(row["Syxh"].ToString(), row["Hissyxh"].ToString(), row["Zyhm"].ToString(),
        //                row["Hzxm"].ToString(), row["Brxb"].ToString(), row["Xsnl"].ToString(), row["Hkdz"].ToString(),
        //                row["Ryrq"].ToString(), row["Brzt"].ToString(), row["Ryzd"].ToString(), row["Ljzt"].ToString(),
        //                row["Pgqk"].ToString(), row["Ljmc"].ToString(), row["LjztName"].ToString(), row["LqljId"].ToString(),
        //                row["Cycw"].ToString(), row["Csrq"].ToString(), row["Ljdm"].ToString(), row["Ljts"].ToString(), row["RydmLjdm"].ToString(),
        //                row["Zyys"].ToString(), row["ZyysDm"].ToString(), Util.UnzipContent(row["WorkFlowXML"].ToString()), Util.UnzipContent(row["EnFroceXml"].ToString()));
        //            inp.RyzdCode = row["RyzdCode"].ToString();
        //            inp.BhljId = ConvertMy.ToDecimal(row["BhljId"].ToString());
        //            inp.Cyks = row["Cyks"].ToString();
        //            inp.CyksName = row["CyksName"].ToString();
        //            inp.Cybq = row["Cybq"].ToString();
        //            inp.CybqName = row["CybqName"].ToString();
        //            inp.Wzjb = row["Wzjb"].ToString();
        //            inp.Cycw = row["Cycw"].ToString();
        //            inp.Cqrq = row["Cqrq"].ToString();
        //            inp.Cyrq = row["Cyrq"].ToString();
        //            inp.Cyzd = row["Cyzd"].ToString();
        //            inp.CyzdName = row["CyzdName"].ToString();
        //            inp.Ljxh = ConvertMy.ToDecimal(row["Ljxh"].ToString());

        //            inpatinetList.Add(inp);
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        ThrowException(ex);
        //    }
        //    return inpatinetList;
        //}
        /// <summary>
        /// 分页病患列表
        /// </summary>
        /// <param name="strDept"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]  //be modified by rjd (add '@cycw') 2011/1/27
        public CP_InpatinetListPaging GetInpatientListPaging(int querykind, string strDept, string Zyys, string hzxm, string zyhm, string cycw, string ksrq, string jsrq, string brzt,int yh, string sfzy, int ljZt, int mysl
          )
        {
            CP_InpatinetListPaging InpatinetListPaging = new CP_InpatinetListPaging();

            List<CP_InpatinetList> inpatinetList = new List<CP_InpatinetList>();

            try
            {
                SqlParameter querykindparam = new SqlParameter("@querykind", SqlDbType.VarChar, 12);
                querykindparam.Value = querykind.ToString();

                SqlParameter deptparam = new SqlParameter("@dept", SqlDbType.VarChar, 12);
                deptparam.Value = strDept;
                SqlParameter zyysparam = new SqlParameter("@zyys", SqlDbType.VarChar, 12);
                zyysparam.Value = Zyys;
                SqlParameter hzxmparam = new SqlParameter("@hzxm", SqlDbType.VarChar, 12);
                hzxmparam.Value = hzxm;

                SqlParameter zyhmparam = new SqlParameter("@zyhm", SqlDbType.VarChar, 12);
                zyhmparam.Value = zyhm;

                SqlParameter cycwparam = new SqlParameter("@cycw", SqlDbType.VarChar, 12);
                cycwparam.Value = cycw;
                SqlParameter ksrqparam = new SqlParameter("@ksrq", SqlDbType.VarChar, 19);
                ksrqparam.Value = ksrq;
                SqlParameter jsrqparam = new SqlParameter("@jsrq", SqlDbType.VarChar, 19);
                jsrqparam.Value = jsrq;

                //SqlParameter brztparam = new SqlParameter("@brzt", SqlDbType.VarChar, 50);
                //brztparam.Value = brzt;
                
                SqlParameter Yh = new SqlParameter("@Yh", SqlDbType.Int, 5);
                Yh.Value = yh;

                SqlParameter Sfzy = new SqlParameter("@Sfzy", SqlDbType.VarChar, 1);
                Sfzy.Value = sfzy;
                SqlParameter LjZt = new SqlParameter("@LjZt", SqlDbType.Int, 5);
                LjZt.Value = ljZt;

                SqlParameter Mysl = new SqlParameter("@Mysl", SqlDbType.Int, 5);
                Mysl.Value = mysl;

                //                v_Sfzy varchar default '1',--是否在院1表示在院0表示出院
                //v_LjZt int default 99,--路径状态

                //if(mysl!=0)
                //SqlParameter Mysl = new SqlParameter("@Mysl", SqlDbType.Int, 5);
                //Mysl.Value = mysl;

                //v_Yh number default 1,--页号
                //v_Mysl 

                SqlParameter[] sqlparams = new SqlParameter[] { querykindparam, deptparam, zyysparam, hzxmparam, zyhmparam, cycwparam, ksrqparam, jsrqparam, Yh, Sfzy, LjZt, Mysl };


                DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_InpatientListPaging", sqlparams, CommandType.StoredProcedure);



                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    CP_InpatinetList inp = new CP_InpatinetList(row["Syxh"].ToString(), row["Hissyxh"].ToString(), row["Zyhm"].ToString(),
                        row["Hzxm"].ToString(), row["Brxb"].ToString(), row["Xsnl"].ToString(), row["Hkdz"].ToString(),
                        row["Ryrq"].ToString(), row["Brzt"].ToString(), row["Ryzd"].ToString(), row["Ljzt"].ToString(),
                        row["Pgqk"].ToString(), row["Ljmc"].ToString(), row["LjztName"].ToString(), row["LqljId"].ToString(),
                        row["Cycw"].ToString(), row["Csrq"].ToString(), row["Ljdm"].ToString(), row["Ljts"].ToString(), row["RydmLjdm"].ToString(),
                        row["Zyys"].ToString(), row["ZyysDm"].ToString(), Util.UnzipContent(row["WorkFlowXML"].ToString()), Util.UnzipContent(row["EnFroceXml"].ToString()),
                        row["NoOfRecord"].ToString(), row["patID"].ToString(), row["NoofClinic"].ToString(), row["Status"].ToString(), row["InCount"].ToString());
                    inp.RyzdCode = row["RyzdCode"].ToString();
                    inp.BhljId = ConvertMy.ToDecimal(row["BhljId"].ToString());
                    inp.Cyks = row["Cyks"].ToString();
                    inp.CyksName = row["CyksName"].ToString();
                    inp.Cybq = row["Cybq"].ToString();
                    inp.CybqName = row["CybqName"].ToString();
                    inp.Wzjb = row["Wzjb"].ToString();
                    inp.Cycw = row["Cycw"].ToString();
                    inp.Cqrq = row["Cqrq"].ToString();
                    inp.Cyrq = row["Cyrq"].ToString();
                    inp.Cyzd = row["Cyzd"].ToString();
                    inp.CyzdName = row["CyzdName"].ToString();
                    inp.Ljxh = ConvertMy.ToDecimal(row["Ljxh"].ToString());

                    inp.Status = row["Status"].ToString();

                    inpatinetList.Add(inp);
                }
                InpatinetListPaging.CP_InpatinetList = inpatinetList;
                if (ds.Tables.Count > 1 && ds.Tables[1].Rows.Count > 0 && ds.Tables[1].Rows[0][0] != null)
                    InpatinetListPaging.AllCount = ConvertMy.ToInt32(ds.Tables[1].Rows[0][0]);

                List<YidanSoft.Tool.KeyValue> keyValues = new List<KeyValue>();
                if (ds.Tables.Count > 2 && ds.Tables[2].Rows.Count > 0)
                {

                    foreach (DataRow row in ds.Tables[2].Rows)
                    {
                        if (ConvertMy.ToString(row[0]) == "-1")
                            InpatinetListPaging.New = ConvertMy.ToString(row[1]);
                        if (ConvertMy.ToString(row[0]) == "1")
                            InpatinetListPaging.InPath = ConvertMy.ToString(row[1]);
                        if (ConvertMy.ToString(row[0]) == "2")
                            InpatinetListPaging.QuitPath = ConvertMy.ToString(row[1]);
                        if (ConvertMy.ToString(row[0]) == "3")
                            InpatinetListPaging.DonePath = ConvertMy.ToString(row[1]);

                    }

                }
                /*   /// <summary>
                   /// 未引入
                   /// </summary>
                   New = -1,
                   /// <summary>
                   /// 全部
                   /// </summary>
                   None = 0,
                   /// <summary>
                   /// 进入
                   /// </summary>
                   InPath = 1,
                   /// <summary>
                   /// 退出
                   /// </summary>
                   QuitPath = 2,

                   /// <summary>
                   /// 完成
                   /// </summary>
                   DonePath = 3,
                   /// <summary>
                   /// 未完成评估
                   /// </summary>
                   NotIn = 4
                  */


            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return InpatinetListPaging;
        }


        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public CP_InpatinetList GetInpatientInfo(string syxh)
        {
            try
            {
                SqlParameter querykindparam = new SqlParameter("@querykind", SqlDbType.VarChar, 12);
                querykindparam.Value = "1";

                SqlParameter syxhparam = new SqlParameter("@syxh", SqlDbType.VarChar, 12);
                syxhparam.Value = syxh;
                SqlParameter[] sqlparams = new SqlParameter[] { querykindparam, syxhparam, };
                DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_InpatientList", sqlparams, CommandType.StoredProcedure);
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    CP_InpatinetList inp = new CP_InpatinetList(row["Syxh"].ToString(), row["Hissyxh"].ToString(), row["Zyhm"].ToString(),
                         row["Hzxm"].ToString(), row["Brxb"].ToString(), row["Xsnl"].ToString(), row["Hkdz"].ToString(),
                         row["Ryrq"].ToString(), row["Brzt"].ToString(), row["Ryzd"].ToString(), row["Ljzt"].ToString(),
                         row["Pgqk"].ToString(), row["Ljmc"].ToString(), row["LjztName"].ToString(), row["LqljId"].ToString(),
                         row["Cycw"].ToString(), row["Csrq"].ToString(), row["Ljdm"].ToString(), row["Ljts"].ToString(), row["RydmLjdm"].ToString(),
                         row["Zyys"].ToString(), row["ZyysDm"].ToString(), Util.UnzipContent(row["WorkFlowXML"].ToString()), Util.UnzipContent(row["EnFroceXml"].ToString()),
                         row["NoOfRecord"].ToString(), row["patID"].ToString(), row["NoofClinic"].ToString(), row["Status"].ToString(), row["InCount"].ToString());
                    inp.RyzdCode = row["RyzdCode"].ToString();
                    inp.BhljId = ConvertMy.ToDecimal(row["BhljId"].ToString());
                    inp.Cyks = row["Cyks"].ToString();
                    inp.CyksName = row["CyksName"].ToString();
                    inp.Cybq = row["Cybq"].ToString();
                    inp.CybqName = row["CybqName"].ToString();
                    inp.Wzjb = row["Wzjb"].ToString();
                    inp.Cycw = row["Cycw"].ToString();
                    inp.Cqrq = row["Cqrq"].ToString();
                    inp.Cyrq = row["Cyrq"].ToString();
                    inp.Cyzd = row["Cyzd"].ToString();
                    inp.CyzdName = row["CyzdName"].ToString();
                    inp.Ljxh = ConvertMy.ToDecimal(row["Ljxh"].ToString());

                    inp.Status = row["Status"].ToString();
                    return inp;
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return null;

        }

        /// <summary>
        /// 返回病人
        /// </summary>
        /// <param name="type">类别：1 全部病人取第一个，2：取所属医生的病人列表的第一个，3，根据条件去病人列表的第一个</param>
        /// <param name="syxh">首页序号</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public CP_InpatinetList GetInpatientInfoByCondition(string type, string Hissyxh)
        {
            try
            {
                SqlParameter querykindparam = new SqlParameter("@querykind", SqlDbType.Variant, 12);
                querykindparam.Value = type;

                SqlParameter syxhparam = new SqlParameter("@Hissyxh", SqlDbType.VarChar, 12);
                syxhparam.Value = Hissyxh;

                SqlParameter[] sqlparams = new SqlParameter[] { querykindparam, syxhparam, };


                DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_InpatientList", sqlparams, CommandType.StoredProcedure);



                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    CP_InpatinetList inp = new CP_InpatinetList(row["Syxh"].ToString(), row["Hissyxh"].ToString(), row["Zyhm"].ToString(),
                         row["Hzxm"].ToString(), row["Brxb"].ToString(), row["Xsnl"].ToString(), row["Hkdz"].ToString(),
                         row["Ryrq"].ToString(), row["Brzt"].ToString(), row["Ryzd"].ToString(), row["Ljzt"].ToString(),
                         row["Pgqk"].ToString(), row["Ljmc"].ToString(), row["LjztName"].ToString(), row["LqljId"].ToString(),
                         row["Cycw"].ToString(), row["Csrq"].ToString(), row["Ljdm"].ToString(), row["Ljts"].ToString(), row["RydmLjdm"].ToString(),
                         row["Zyys"].ToString(), row["ZyysDm"].ToString(), Util.UnzipContent(row["WorkFlowXML"].ToString()), Util.UnzipContent(row["EnFroceXml"].ToString()),
                         row["NoOfRecord"].ToString(), row["patID"].ToString(), row["NoofClinic"].ToString(), row["Status"].ToString(), row["InCount"].ToString());
                    inp.RyzdCode = row["RyzdCode"].ToString();
                    inp.BhljId = ConvertMy.ToDecimal(row["BhljId"].ToString());
                    inp.Cyks = row["Cyks"].ToString();
                    inp.CyksName = row["CyksName"].ToString();
                    inp.Cybq = row["Cybq"].ToString();
                    inp.CybqName = row["CybqName"].ToString();
                    inp.Wzjb = row["Wzjb"].ToString();
                    inp.Cycw = row["Cycw"].ToString();
                    inp.Cqrq = row["Cqrq"].ToString();
                    inp.Cyrq = row["Cyrq"].ToString();
                    inp.Cyzd = row["Cyzd"].ToString();
                    inp.CyzdName = row["CyzdName"].ToString();
                    inp.Ljxh = ConvertMy.ToDecimal(row["Ljxh"].ToString());

                    inp.Status = row["Status"].ToString();
                    return inp;
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return null;

        }

        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public int UpdatePatientContactsInfo(List<Modal_PatientContactsInfo> list, decimal syxh)
        {
            try
            {
                //string sqlcmd = "exec usp_CP_PatientContacts @Syxh=@Syxh,@Lxrm=@Lxrm,@Lxrxb=@Lxrxb,@Lxgx=@Lxgx,@Lxdz=@Lxdz,@Lxdw=@Lxdw,@Lxjtdh=@Lxjtdh,@Lxgzdh=@Lxgzdh,@Lxyb=@Lxyb";

                SqlParameter param_Lxrm = new SqlParameter("@Lxrm", SqlDbType.VarChar, 32);
                SqlParameter param_Lxrxb = new SqlParameter("@Lxrxb", SqlDbType.VarChar, 4);
                SqlParameter param_Lxgx = new SqlParameter("@Lxgx", SqlDbType.VarChar, 4);
                SqlParameter param_Lxdz = new SqlParameter("@Lxdz", SqlDbType.VarChar, 255);
                SqlParameter param_Lxdw = new SqlParameter("@Lxdw", SqlDbType.VarChar, 64);
                SqlParameter param_Lxjtdh = new SqlParameter("@Lxjtdh", SqlDbType.VarChar, 16);
                SqlParameter param_Lxgzdh = new SqlParameter("@Lxgzdh", SqlDbType.VarChar, 16);
                SqlParameter param_Lxyb = new SqlParameter("@Lxyb", SqlDbType.VarChar, 16);
                SqlParameter param_Syxh = new SqlParameter("@Syxh", syxh);

                param_Lxrm.Value = list[0].Lxrm;
                param_Lxrxb.Value = list[0].Lxrxb;
                param_Lxgx.Value = list[0].Lxgx;
                param_Lxdz.Value = list[0].Lxdz;
                param_Lxdw.Value = list[0].Lxdw;
                param_Lxjtdh.Value = list[0].Lxjtdh;
                param_Lxgzdh.Value = list[0].Lxgzdh;
                param_Lxyb.Value = list[0].Lxyb;

                SqlParameter[] sqlParameters = new SqlParameter[] { param_Lxrm, param_Lxrxb, param_Lxgx, param_Lxdz, param_Lxdw, param_Lxjtdh, param_Lxgzdh, param_Lxyb, param_Syxh };
                SqlHelper.ExecuteNoneQuery("usp_CP_PatientContacts", sqlParameters, CommandType.StoredProcedure);
                return 1;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return 0;
        }

        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<Modal_PatientContactsInfo> SelectFirstPationContactsInfo(decimal syxh)
        {
            try
            {
                string sqlcmd = "select Lxbh,Lxrm,Lxrxb,Lxgx,Lxdz,Lxdw,Lxjtdh,Lxgzdh,Lxyb,Lxrbz from CP_PatientContacts where Syxh=@Syxh";
                SqlParameter param = new SqlParameter("Syxh", syxh);
                SqlParameter[] sqlParameters = new SqlParameter[] { param };
                List<Modal_PatientContactsInfo> datalist = new List<Modal_PatientContactsInfo>();
                DataTable dt = SqlHelper.ExecuteDataTable(sqlcmd, sqlParameters, CommandType.Text);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    datalist.Add(new Modal_PatientContactsInfo
                    {
                        Lxbh = dt.Rows[i][0].ToString(),
                        Lxrm = dt.Rows[i][1].ToString(),
                        Lxrxb = dt.Rows[i][2].ToString(),
                        Lxgx = dt.Rows[i][3].ToString(),
                        Lxdz = dt.Rows[i][4].ToString(),
                        Lxdw = dt.Rows[i][5].ToString(),
                        Lxjtdh = dt.Rows[i][6].ToString(),
                        Lxgzdh = dt.Rows[i][7].ToString(),
                        Lxyb = dt.Rows[i][8].ToString(),
                        Lxrbz = Convert.ToInt32(dt.Rows[i][9])

                    });
                }
                return datalist;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return null;

        }


        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public int UpdatePatInfo(CP_InPatient inpat)
        {
            try
            {//SexID=@Brxb,
                string sqlcmd = @"update InPatient set 
                                    Marital=@Hyzk,
                                    NationID=@Mzdm,
                                    NationalityID=@Gjdm,
                                    Birth=@Csrq,
                                    AgeStr=@Xsnl,

                                    Religion=@Zjxy,
                                    IDNO=@Sfzh,
                                    EDU=@Whcd,
                                    EDUC=@Jynx,
                                    ProvinceID=@Ssdm,
                                    CountyID=@Qxdm,
                                    Nativeplace_P=@Jgssdm,
                                    Nativeplace_C=@Jgqxdm,
                                    PayID=@Brxz,
                                    JobID=@Zydm,
                                    Organization=@Gzdw,
                                    OfficePlace=@Dwdz,
                                    OfficeTEL=@Dwdh,
                                    OfficePost=@Dwyb,
                                    NativeAddress=@Hkdz,
                                    NativeTEL=@Hkdh,
                                    NativePost=@Hkyb,
                                    Address=@Dqdz,
                                    InCount=@Rycs,
                                    AdmitDiagnosis=@Ryzd where NoOfInpat=@Syxh";
                SqlParameter param_Hyzk = new SqlParameter("Hyzk", SqlDbType.VarChar, 4);
                SqlParameter param_Mzdm = new SqlParameter("Mzdm", SqlDbType.VarChar, 4);
                SqlParameter param_Gjdm = new SqlParameter("Gjdm", SqlDbType.VarChar, 4);
                SqlParameter param_Csrq = new SqlParameter("Csrq", SqlDbType.Char, 10);
                SqlParameter param_Xsnl = new SqlParameter("Xsnl", SqlDbType.VarChar, 16);
                SqlParameter param_Brxb = new SqlParameter("Brxb", SqlDbType.VarChar, 4);
                SqlParameter param_Zjxy = new SqlParameter("Zjxy", SqlDbType.VarChar, 32);
                SqlParameter param_Sfzh = new SqlParameter("Sfzh", SqlDbType.VarChar, 18);
                SqlParameter param_Whcd = new SqlParameter("Whcd", SqlDbType.VarChar, 4);
                SqlParameter param_Jynx = new SqlParameter("Jynx", SqlDbType.Decimal);
                SqlParameter param_Ssdm = new SqlParameter("Ssdm", SqlDbType.VarChar, 10);
                SqlParameter param_Qxdm = new SqlParameter("Qxdm", SqlDbType.VarChar, 10);
                SqlParameter param_Jgssdm = new SqlParameter("Jgssdm", SqlDbType.VarChar, 10);
                SqlParameter param_Jgqxdm = new SqlParameter("Jgqxdm", SqlDbType.VarChar, 10);
                SqlParameter param_Brxz = new SqlParameter("Brxz", SqlDbType.VarChar, 4);
                SqlParameter param_Zydm = new SqlParameter("Zydm", SqlDbType.VarChar, 4);
                SqlParameter param_Gzdw = new SqlParameter("Gzdw", SqlDbType.VarChar, 64);
                SqlParameter param_Dwdz = new SqlParameter("Dwdz", SqlDbType.VarChar, 64);
                SqlParameter param_Dwdh = new SqlParameter("Dwdh", SqlDbType.VarChar, 16);
                SqlParameter param_Dwyb = new SqlParameter("Dwyb", SqlDbType.VarChar, 16);
                SqlParameter param_Hkdz = new SqlParameter("Hkdz", SqlDbType.VarChar, 64);
                SqlParameter param_Hkdh = new SqlParameter("Hkdh", SqlDbType.VarChar, 16);
                SqlParameter param_Hkyb = new SqlParameter("Hkyb", SqlDbType.VarChar, 16);
                SqlParameter param_Dqdz = new SqlParameter("Dqdz", SqlDbType.VarChar, 255);
                SqlParameter param_Rycs = new SqlParameter("Rycs", SqlDbType.Int);
                SqlParameter param_Ryzd = new SqlParameter("Ryzd", SqlDbType.VarChar, 50);
                SqlParameter param_Syxh = new SqlParameter("Syxh", inpat.Syxh);
                param_Hyzk.Value = inpat.Hyzk;
                param_Mzdm.Value = inpat.Mzdm;
                param_Gjdm.Value = inpat.Gjdm;
                param_Csrq.Value = inpat.Csrq;
                param_Xsnl.Value = inpat.Xsnl;
                param_Brxb.Value = inpat.Brxb;
                param_Zjxy.Value = inpat.Zjxy;
                param_Sfzh.Value = inpat.Sfzh;
                param_Whcd.Value = inpat.Whcd;
                param_Jynx.Value = inpat.Jynx;
                param_Ssdm.Value = inpat.Ssdm;
                param_Qxdm.Value = inpat.Qxdm;
                param_Jgssdm.Value = inpat.Jgssdm;
                param_Jgqxdm.Value = inpat.Jgqxdm;
                param_Brxz.Value = inpat.Brxz;
                param_Zydm.Value = inpat.Zydm;
                param_Gzdw.Value = inpat.Gzdw;
                param_Dwdz.Value = inpat.Dwdz;
                param_Dwdh.Value = inpat.Dwdh;
                param_Dwyb.Value = inpat.Dwyb;
                param_Hkdz.Value = inpat.Hkdz;
                param_Hkdh.Value = inpat.Hkdh;
                param_Hkyb.Value = inpat.Hkyb;
                param_Dqdz.Value = inpat.Dqdz;
                param_Rycs.Value = inpat.Rycs;
                param_Ryzd.Value = inpat.Ryzd;
                SqlParameter[] sqlParameters = new SqlParameter[] { param_Hyzk, param_Mzdm, param_Gjdm, param_Csrq, param_Xsnl, param_Brxb, param_Zjxy, param_Sfzh, param_Whcd, param_Jynx, param_Ssdm, param_Qxdm, param_Jgssdm, param_Jgqxdm, param_Brxz, param_Zydm, param_Gzdw, param_Dwdz, param_Dwdh, param_Dwyb, param_Hkdz, param_Hkdh, param_Hkyb, param_Dqdz, param_Rycs, param_Ryzd, param_Syxh };
                SqlHelper.ExecuteNoneQuery(sqlcmd, sqlParameters, CommandType.Text);
                return 1;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return 0;
        }


        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Modal_PatientInfo SelectPatInfo(string syxh)
        {
            try
            {
                Modal_PatientInfo patInfo = new Modal_PatientInfo();


                patInfo.Areas = SelectCommonAreas();
                patInfo.CommonDictionary = SelectCommonDictionary(); 
                patInfo.ContactsInfo = SelectPatientContactsInfo(syxh);

                patInfo.Diagnosis = SelectCommonDiagnosis();


                return patInfo;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return null;


        }

        private List<Modal_PatientContactsInfo> SelectPatientContactsInfo(string syxh)
        {
            try
            {
                string sqlcmd = "select Lxbh,Lxrm,(select Name from Dictionary_detail where CategoryID='3' and DetailID=Lxrxb) Lxrxb,(select Name from Dictionary_detail where CategoryID='44' and DetailID=Lxgx) Lxgx,Lxdz,Lxdw,Lxjtdh,Lxgzdh,Lxyb,Lxrbz from CP_PatientContacts where Syxh=@Syxh";
                SqlParameter param = new SqlParameter("Syxh", syxh);
                SqlParameter[] sqlParameters = new SqlParameter[] { param };
                List<Modal_PatientContactsInfo> datalist = new List<Modal_PatientContactsInfo>();
                DataTable dt = SqlHelper.ExecuteDataTable(sqlcmd, sqlParameters, CommandType.Text);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    datalist.Add(new Modal_PatientContactsInfo
                    {
                        Lxbh = dt.Rows[i][0].ToString(),
                        Lxrm = dt.Rows[i][1].ToString(),
                        Lxrxb = dt.Rows[i][2].ToString(),
                        Lxgx = dt.Rows[i][3].ToString(),
                        Lxdz = dt.Rows[i][4].ToString(),
                        Lxdw = dt.Rows[i][5].ToString(),
                        Lxjtdh = dt.Rows[i][6].ToString(),
                        Lxgzdh = dt.Rows[i][7].ToString(),
                        Lxyb = dt.Rows[i][8].ToString(),
                        Lxrbz = Convert.ToInt32(dt.Rows[i][9])

                    });
                }
                return datalist;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return null;

        }


        private List<Modal_Dictionary> SelectCommonDictionary()
        {
            try
            {
                string sqlcmd = "select CategoryID,DetailID,Name,Py,Wb from Dictionary_detail";
                List<Modal_Dictionary> list = new List<Modal_Dictionary>();
                DataTable dt = SqlHelper.ExecuteDataTable(sqlcmd);
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new Modal_Dictionary
                    {
                        Lbdm = dt.Rows[i][0].ToString(),
                        Mxdm = dt.Rows[i][1].ToString(),
                        Name = dt.Rows[i][2].ToString(),
                        Py = dt.Rows[i][3].ToString(),
                        Wb = dt.Rows[i][4].ToString(),
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return null;

        }


        private List<Modal_Areas> SelectCommonAreas()
        {
            try
            {
                string sqlcmd = "select Category,ID,Name,Py,Wb from Areas";


                List<Modal_Areas> list = new List<Modal_Areas>();

                DataTable dt = SqlHelper.ExecuteDataTable(sqlcmd);


                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new Modal_Areas
                    {

                        Dqlb = dt.Rows[i][0].ToString(),
                        Dqdm = dt.Rows[i][1].ToString(),
                        Name = dt.Rows[i][2].ToString(),
                        Py = dt.Rows[i][3].ToString(),
                        Wb = dt.Rows[i][4].ToString(),
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return null;
        }


        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<Modal_Diagnosis> SelectRyzdListByFilter(string filter)
        {
            try
            {
                string sqlcmd = "";
                if (filter == "ALL")
                {
                     sqlcmd = string.Format("select ICD,Name,Py,Wb,MarkId from Diagnosis where  Py like '%{0}%' or ICD like '%{0}%'", "");
                }
                else
                {
                     sqlcmd = string.Format("select top 100 ICD,Name,Py,Wb,MarkId from Diagnosis where  Py like '%{0}%' or ICD like '%{0}%'", filter);
                }

                //SqlParameter param = new SqlParameter("Filter", filter);
                //SqlParameter[] parameters = new SqlParameter[] { param};


                List<Modal_Diagnosis> list = new List<Modal_Diagnosis>();

                //DataTable dt = SqlHelper.ExecuteDataTable(m_ConnectionString, CommandType.Text, sqlcmd,parameters);

                DataTable dt = SqlHelper.ExecuteDataTable(sqlcmd);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new Modal_Diagnosis
                    {
                        Zddm = dt.Rows[i][0].ToString(),
                        Name = dt.Rows[i][1].ToString(),
                        Py = dt.Rows[i][2].ToString(),
                        Wb = dt.Rows[i][3].ToString(),
                        Zdbs = dt.Rows[i][4].ToString()
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return null;

        }



        private List<Modal_Diagnosis> SelectCommonDiagnosis()
        {
            try
            {
                string sqlcmd = "select MarkId,ICD,Name,Py,Wb from Diagnosis";

                List<Modal_Diagnosis> list = new List<Modal_Diagnosis>();

                DataTable dt = SqlHelper.ExecuteDataTable(sqlcmd);

                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    list.Add(new Modal_Diagnosis
                    {
                        Zdbs = ConvertMy.ToString(dt.Rows[i]["MarkId"]),
                        Zddm = ConvertMy.ToString(dt.Rows[i]["ICD"]),
                        Name = ConvertMy.ToString(dt.Rows[i]["Name"]),
                        Py = ConvertMy.ToString(dt.Rows[i]["Py"]),
                        Wb = ConvertMy.ToString(dt.Rows[i]["Wb"])
                    });
                }
                return list;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return null;
        }

        /// <summary>
        /// 获取病人详细信息
        /// </summary>
        /// <param name="syxh"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public CP_InPatient GetBasicInfo(string syxh)
        {
            CP_InPatient _basicInfo = new CP_InPatient();
            DataTable dt = YidanEHRDataService.SqlHelper.ExecuteDataTable(String.Format("select * from  InPatient where NoOfInpat='{0}'", syxh));
            if (dt.Rows.Count > 0)
            { 
                _basicInfo.Syxh = ConvertMy.ToDecimal(dt.Rows[0]["NoOfInpat"]);
                _basicInfo.Hissyxh = ConvertMy.ToDecimal(dt.Rows[0]["PatNoOfHis"]);
                _basicInfo.Mzhm = ConvertMy.ToString(dt.Rows[0]["NoOfClinic"]);
                _basicInfo.Bahm = ConvertMy.ToString(dt.Rows[0]["NoOfRecord"]);
                _basicInfo.Zyhm = ConvertMy.ToString(dt.Rows[0]["PatID"]);
                _basicInfo.Glzyhm = ConvertMy.ToString(dt.Rows[0]["InnerPIX"]);
                _basicInfo.Hzxm = ConvertMy.ToString(dt.Rows[0]["Name"]);
                _basicInfo.Py = ConvertMy.ToString(dt.Rows[0]["Py"]);
                _basicInfo.Wb = ConvertMy.ToString(dt.Rows[0]["Wb"]);
                _basicInfo.Brxz = ConvertMy.ToString(dt.Rows[0]["PayID"]);
                _basicInfo.Brly = ConvertMy.ToString(dt.Rows[0]["Origin"]);
                _basicInfo.Rycs = ConvertMy.ToInt32(dt.Rows[0]["InCount"]);
                _basicInfo.Brxb = ConvertMy.ToString(dt.Rows[0]["SexID"]);
                _basicInfo.Csrq = ConvertMy.ToString(dt.Rows[0]["Birth"]);
                _basicInfo.Brnl = ConvertMy.ToInt32(dt.Rows[0]["Age"]);
                _basicInfo.Xsnl = ConvertMy.ToString(dt.Rows[0]["AgeStr"]);
                _basicInfo.Sfzh = ConvertMy.ToString(dt.Rows[0]["IDNO"]);
                _basicInfo.Hyzk = ConvertMy.ToString(dt.Rows[0]["Marital"]);
                _basicInfo.Zydm = ConvertMy.ToString(dt.Rows[0]["JobID"]);
                _basicInfo.Ssdm = ConvertMy.ToString(dt.Rows[0]["ProvinceID"]);
                _basicInfo.Qxdm = ConvertMy.ToString(dt.Rows[0]["CountyID"]);
                _basicInfo.Mzdm = ConvertMy.ToString(dt.Rows[0]["NationID"]);
                _basicInfo.Gjdm = ConvertMy.ToString(dt.Rows[0]["NationalityID"]);
                _basicInfo.Jgssdm = ConvertMy.ToString(dt.Rows[0]["NationalityID"]);
                _basicInfo.Jgqxdm = ConvertMy.ToString(dt.Rows[0]["Nativeplace_C"]);
                _basicInfo.Gzdw = ConvertMy.ToString(dt.Rows[0]["Organization"]);
                _basicInfo.Dwdz = ConvertMy.ToString(dt.Rows[0]["OfficePlace"]);
                _basicInfo.Dwdh = ConvertMy.ToString(dt.Rows[0]["OfficeTEL"]);
                _basicInfo.Dwyb = ConvertMy.ToString(dt.Rows[0]["OfficePost"]);
                _basicInfo.Hkdz = ConvertMy.ToString(dt.Rows[0]["NativeAddress"]);
                _basicInfo.Hkdh = ConvertMy.ToString(dt.Rows[0]["NativeTEL"]);
                _basicInfo.Hkyb = ConvertMy.ToString(dt.Rows[0]["NativePost"]);
                _basicInfo.Dqdz = ConvertMy.ToString(dt.Rows[0]["Address"]);
                _basicInfo.Lxrm = ConvertMy.ToString(dt.Rows[0]["ContactPerson"]);
                _basicInfo.Lxgx = ConvertMy.ToString(dt.Rows[0]["Relationship"]);
                _basicInfo.Lxdz = ConvertMy.ToString(dt.Rows[0]["ContactAddress"]);
                _basicInfo.Lxdw = ConvertMy.ToString(dt.Rows[0]["ContactOffice"]);
                _basicInfo.Lxdh = ConvertMy.ToString(dt.Rows[0]["ContactTEL"]);
                _basicInfo.Lxyb = ConvertMy.ToString(dt.Rows[0]["ContactPost"]);
                _basicInfo.Bscsz = ConvertMy.ToString(dt.Rows[0]["Offerer"]);
                _basicInfo.Sbkh = ConvertMy.ToString(dt.Rows[0]["SocialCare"]);
                _basicInfo.Bxkh = ConvertMy.ToString(dt.Rows[0]["Insurance"]);
                _basicInfo.Qtkh = ConvertMy.ToString(dt.Rows[0]["CardNo"]);
                _basicInfo.Ryqk = ConvertMy.ToString(dt.Rows[0]["AdmitInfo"]);
                _basicInfo.Ryks = ConvertMy.ToString(dt.Rows[0]["AdmitDept"]);
                _basicInfo.Rybq = ConvertMy.ToString(dt.Rows[0]["AdmitWard"]);
                _basicInfo.Rycw = ConvertMy.ToString(dt.Rows[0]["AdmitBed"]);
                _basicInfo.Ryrq = ConvertMy.ToString(dt.Rows[0]["AdmitDate"]);
                _basicInfo.Rqrq = ConvertMy.ToString(dt.Rows[0]["InWardDate"]);
                _basicInfo.Ryzd = ConvertMy.ToString(dt.Rows[0]["AdmitDiagnosis"]);
                _basicInfo.Cyks = ConvertMy.ToString(dt.Rows[0]["OutHosDept"]);
                _basicInfo.Cybq = ConvertMy.ToString(dt.Rows[0]["OutHosWard"]);
                _basicInfo.Cycw = ConvertMy.ToString(dt.Rows[0]["OutBed"]);
                _basicInfo.Cqrq = ConvertMy.ToString(dt.Rows[0]["OutWardDate"]);
                _basicInfo.Cyrq = ConvertMy.ToString(dt.Rows[0]["OutHosDate"]);
                _basicInfo.Cyzd = ConvertMy.ToString(dt.Rows[0]["OutDiagnosis"]);
                _basicInfo.Zyts = ConvertMy.ToDecimal(dt.Rows[0]["TotalDays"]);
                _basicInfo.Mzzd = ConvertMy.ToString(dt.Rows[0]["ClinicDiagnosis"]);
                //_basicInfo.Mzzdzy = ConvertMy.ToString(dt.Rows[0]["Mzzdzy"]);
                // _basicInfo.Mzzhzy = ConvertMy.ToString(dt.Rows[0]["Mzzhzy"]);
                _basicInfo.Fbjq = ConvertMy.ToString(dt.Rows[0]["SolarTerms"]);
                _basicInfo.Rytj = ConvertMy.ToString(dt.Rows[0]["AdmitWay"]);
                _basicInfo.Cyfs = ConvertMy.ToString(dt.Rows[0]["OutWay"]);
                _basicInfo.Mzys = ConvertMy.ToString(dt.Rows[0]["ClinicDoctor"]);
                // _basicInfo.Zyys = ConvertMy.ToString(dt.Rows[0]["Zyys"]);
                _basicInfo.Zzysdm = ConvertMy.ToString(dt.Rows[0]["Resident"]);
                _basicInfo.Zrysdm = ConvertMy.ToString(dt.Rows[0]["Chief"]);
                _basicInfo.Whcd = ConvertMy.ToString(dt.Rows[0]["EDU"]);
                _basicInfo.Jynx = ConvertMy.ToDecimal(dt.Rows[0]["EDUC"]);
                _basicInfo.Zjxy = ConvertMy.ToString(dt.Rows[0]["Religion"]);
                _basicInfo.Brzt = ConvertMy.ToShort(dt.Rows[0]["Status"]);
                _basicInfo.Wzjb = ConvertMy.ToString(dt.Rows[0]["CriticalLevel"]);
                _basicInfo.Hljb = ConvertMy.ToString(dt.Rows[0]["AttendLevel"]);
                _basicInfo.Zdbr = ConvertMy.ToShort(dt.Rows[0]["Emphasis"]);
                _basicInfo.Yexh = ConvertMy.ToShort(dt.Rows[0]["IsBaby"]);
                _basicInfo.Mqsyxh = ConvertMy.ToDecimal(dt.Rows[0]["Mother"]);
                _basicInfo.Ybdm = ConvertMy.ToString(dt.Rows[0]["MedicareID"]);
                _basicInfo.Ybde = ConvertMy.ToDecimal(dt.Rows[0]["MedicareQuota"]);
                _basicInfo.Brlx = ConvertMy.ToString(dt.Rows[0]["Style"]);
                _basicInfo.Pzlx = ConvertMy.ToString(dt.Rows[0]["VouchersCode"]);
                //_basicInfo.Pzlxmc = ConvertMy.ToString(dt.Rows[0]["Pzlxmc"]);
                //_basicInfo.Pzh = ConvertMy.ToString(dt.Rows[0]["Pzh"]);
                _basicInfo.Czyh = ConvertMy.ToString(dt.Rows[0]["Operator"]);
                //_basicInfo.Xxh = ConvertMy.ToString(dt.Rows[0]["Xxh"]);
                //_basicInfo.Gxrq = ConvertMy.ToString(dt.Rows[0]["Gxrq"]);
                //_basicInfo.Memo = ConvertMy.ToString(dt.Rows[0]["Memo"]);
 
                
            }
            return _basicInfo;
        }

    }
}