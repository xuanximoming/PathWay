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

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {

        /// <summary>
        ///插入并查询 His接口配置
        /// </summary>
        /// <param name="drug"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<APPCFG> GetAppCfgInfo()
        {
            List<APPCFG> list = new List<APPCFG>();
            try
            {
                //if (parameter == null) parameter = new HisSxpz();
                //查询全部，加个条件，防止sql注入
                string sSql = @"select * from APPCFG WHERE 1=1";

                
                DataTable dt = null;

                dt = SqlHelper.ExecuteDataTable(sSql);


                if (dt != null)
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        APPCFG model = new APPCFG();

                        model.Configkey = item["Configkey"].ToString();


                        model.Name = item["Name"].ToString();


                        model.Value = item["Value"].ToString();


                        model.Descript = item["Descript"].ToString();


                        model.ParamType = int.Parse(item["ParamType"].ToString());


                        model.Cfgkeyset = item["Cfgkeyset"].ToString();


                        model.Design = item["Design"].ToString();

                        if (item["ClientFlag"] != null && item["ClientFlag"].ToString() != "")
                        {
                            model.ClientFlag = int.Parse(item["ClientFlag"].ToString());
                        }
                        else
                        {
                            model.ClientFlag = 0;
                        }
                        if (item["Hide"] != null && item["Hide"].ToString() != "")
                        {
                            model.Hide = int.Parse(item["Hide"].ToString());
                        }
                        else
                        {
                            model.Hide = 1;
                        }
                        if (item["Valid"] != null && item["Valid"].ToString() != "")
                        {
                            model.Valid = int.Parse(item["Valid"].ToString());
                        }
                        else
                        {
                            model.Valid = 1;
                        }
                        list.Add(model);
                    }
                }
                return list;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }



        // add by luff 20130402
        #region  获得诊疗护理主表基础数据维护
        /// <summary>
        ///规范化获取数据库数据的方法  获得诊疗护理主表数据维护
        /// </summary>
        /// <param name="iID">默认传1，表示获得所有诊疗护理明细表数据</param>
        /// <param name="isTy">默认传true</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_DiagNurExecCategory> GetDiagNurExecCategory(int iID,bool isTy)
        {
            List<CP_DiagNurExecCategory> list = new List<CP_DiagNurExecCategory>();
            try
            {

                string sSql = "";
                DataTable dt = null;
                if (isTy)
                {
                    sSql = @"select Lbxh,LbName,Yxjl, case Yxjl when 1 then '有效' else '无效' end as Yxjlmc,Create_Time,Create_User,Cancel_Time,Cancel_User,OrderValue from CP_DiagNurExecCategory where 1=" + iID;
                    dt = SqlHelper.ExecuteDataTable(sSql);


                    if (dt != null)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            CP_DiagNurExecCategory model = new CP_DiagNurExecCategory();
                            if (item["Lbxh"] != null && item["Lbxh"].ToString() != "")
                            {
                                model.Lbxh = int.Parse(item["Lbxh"].ToString());
                            }

                            model.LbName = item["LbName"].ToString();

                            if (item["Yxjl"] != null && item["Yxjl"].ToString() != "")
                            {
                                model.Yxjl = int.Parse(item["Yxjl"].ToString());
                            }
                            model.Yxjlmc = item["Yxjlmc"].ToString();
                            model.Extension = "";
                            model.Create_Time = item["Create_Time"].ToString();

                            model.Create_User = item["Create_User"].ToString();

                            model.Cancel_Time = item["Cancel_Time"].ToString();

                            model.Cancel_User = item["Cancel_User"].ToString();

                            if (item["OrderValue"] != null && item["OrderValue"].ToString() != "")
                            {
                                model.OrderValue = int.Parse(item["OrderValue"].ToString());
                            }
                            list.Add(model);
                        }
                    }
                }
                else
                {
                    sSql = @"select * from CP_DiagNurExecCategory where Yxjl=1";
                    dt = SqlHelper.ExecuteDataTable(sSql);
                    if (dt != null)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                           CP_DiagNurExecCategory model = new CP_DiagNurExecCategory();
                             
                           model.Lbxh = int.Parse(item["Lbxh"].ToString());
 
                           model.LbName = item["LbName"].ToString();

                           list.Add(model);
                        }
                    }
                }

               

               
                return list;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        #endregion

        #region 规范化的新增数据库数据的方法 新增 诊疗护理主表信息
        /// <summary>
        /// 新增诊疗护理主表操作
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="Operation"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public int InsertDiagNurCategory(CP_DiagNurExecCategory model)
        {


            int returnBool = 0;
            try
            {
                if (model == null)
                {
                    model = new CP_DiagNurExecCategory();
                    returnBool = 0;

                }
                else
                {
                    int iVal = Exists(model.LbName);//可以在前台插入数据
                    if (iVal == 0)
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into CP_DiagNurExecCategory(");
                        strSql.Append("Lbxh,LbName,Yxjl,Create_Time,Create_User,Cancel_Time,Cancel_User,OrderValue)");
                        strSql.Append(" values (");
                        strSql.Append("@Lbxh,@LbName,@Yxjl,@Create_Time,@Create_User,@Cancel_Time,@Cancel_User,@OrderValue)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
                    new SqlParameter("@Lbxh", SqlDbType.Int,4),
					new SqlParameter("@LbName", SqlDbType.VarChar,100),
					new SqlParameter("@Yxjl", SqlDbType.Int,4),
					new SqlParameter("@Create_Time", SqlDbType.VarChar,19),
					new SqlParameter("@Create_User", SqlDbType.VarChar,10),
					new SqlParameter("@Cancel_Time", SqlDbType.VarChar,19),
					new SqlParameter("@Cancel_User", SqlDbType.VarChar,10),
					new SqlParameter("@OrderValue", SqlDbType.Int,4)};
                        parameters[0].Value = GetMaxId();
                        parameters[1].Value = model.LbName;
                        parameters[2].Value = model.Yxjl;
                        parameters[3].Value = model.Create_Time;
                        parameters[4].Value = model.Create_User;
                        parameters[5].Value = model.Cancel_Time;
                        parameters[6].Value = model.Cancel_User;
                        parameters[7].Value = model.OrderValue;

                        //DataTable dt = null;
                        SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                        returnBool = 1;//插入数据正常
                    }
                    else
                    {
                        returnBool = 2;//插入数据有重复
                    }

                }
            }

            catch (Exception ex)
            {
                ThrowException(ex);
                returnBool = 3;//插入数据有错误
            }
            return returnBool;

        }
        #endregion

        #region 规范化的更新数据库数据的方法 更新 诊疗护理主表信息
        /// <summary>
        /// 更新诊疗护理主表信息操作
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="Operation"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public int UpdateDiagNurExecCategory(CP_DiagNurExecCategory model)
        {
            int returnBool = 0;
            try
            {
                if (model == null)
                {
                    model = new CP_DiagNurExecCategory();
                    returnBool = 0;

                }
                else
                {
                     //int iVal = Exists(model.LbName);//也可以在前台判断是否有重复值插入
                     //if (iVal == 0)
                     //{
                         StringBuilder strSql = new StringBuilder();
                         strSql.Append("update CP_DiagNurExecCategory set ");
                         strSql.Append("LbName=@LbName,");
                         strSql.Append("Yxjl=@Yxjl,");
                         strSql.Append("Create_Time=@Create_Time,");
                         strSql.Append("Create_User=@Create_User,");
                         strSql.Append("Cancel_Time=@Cancel_Time,");
                         strSql.Append("Cancel_User=@Cancel_User,");
                         strSql.Append("OrderValue=@OrderValue");
                         strSql.Append(" where Lbxh=@Lbxh");
                         SqlParameter[] parameters = {
					new SqlParameter("@LbName", SqlDbType.VarChar,100),
					new SqlParameter("@Yxjl", SqlDbType.Int,4),
					new SqlParameter("@Create_Time", SqlDbType.VarChar,19),
					new SqlParameter("@Create_User", SqlDbType.VarChar,10),
					new SqlParameter("@Cancel_Time", SqlDbType.VarChar,19),
					new SqlParameter("@Cancel_User", SqlDbType.VarChar,10),
					new SqlParameter("@OrderValue", SqlDbType.Int,4),
					new SqlParameter("@Lbxh", SqlDbType.Int,4)};
                         parameters[0].Value = model.LbName;
                         parameters[1].Value = model.Yxjl;
                         parameters[2].Value = model.Create_Time;
                         parameters[3].Value = model.Create_User;
                         parameters[4].Value = model.Cancel_Time;
                         parameters[5].Value = model.Cancel_User;
                         parameters[6].Value = model.OrderValue;
                         parameters[7].Value = model.Lbxh;


                         //DataTable dt = null;
                         SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                         returnBool = 1;//正常插入数据
                     //}
                     //else
                     //{
                         //returnBool = 2; //插入数据有重复
                     //}
                }


            }

            catch (Exception ex)
            {
                ThrowException(ex);
                returnBool = 3;//插入数据失败的时候
            }
            return returnBool;
        }

        #endregion

        /// <summary>
        /// 判断诊疗护理主表是否存在该记录
        /// </summary>
        public int Exists(string sLbName)
        {
            int iVal = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from CP_DiagNurExecCategory");
            strSql.Append(" where LbName=@LbName");
            SqlParameter[] parameters = {
					new SqlParameter("@LbName", SqlDbType.VarChar,100),
			};
            parameters[0].Value = sLbName;

           iVal = Convert.ToInt32(SqlHelper.ExecuteScalar(strSql.ToString(), parameters));
           if (iVal > 0)
           {
               iVal = 1;
           }
           return iVal;
        }



        /// <summary>
        /// 判断诊疗护理明细表是否存在该记录
        /// </summary>
        public int ExistsDetail(string sName,int iLbxh)
        {
            int iVal = 0;
            StringBuilder strSql = new StringBuilder();
            strSql.Append("select count(1) from CP_DiagNurExecCategoryDetail");
            strSql.Append(" where MxName=@MxName and Lbxh = @Lbxh");
            SqlParameter[] parameters = {
					new SqlParameter("@MxName", SqlDbType.VarChar,100),
                    new SqlParameter("@Lbxh", SqlDbType.Int,4)
			};
            parameters[0].Value = sName;
            parameters[1].Value = iLbxh;

            iVal = Convert.ToInt32(SqlHelper.ExecuteScalar(strSql.ToString(), parameters));
            if (iVal > 0)
            {
                iVal = 1;
            }
            return iVal;
        }

        /// <summary>
        /// 获得诊疗护理主表数据最大ID
        /// </summary>
        public int GetMaxId()
        {
            int iVal = 0;
            string sSql = "select Max(Lbxh) from CP_DiagNurExecCategory ";
            object obj = SqlHelper.ExecuteScalar(sSql);
            if (obj.ToString() == "" || obj == null)
            {
                iVal =  1;
            }
            else
            {
                iVal = Convert.ToInt32(obj.ToString()) + 1;
            }
            return iVal;
        }

        /// <summary>
        /// 根据项目分类编号获得诊疗护理分类名称
        /// </summary>
        public string GetLbxmName(int ilbxm)
        {
            string sName = "";
            string sSql = "select LbName from CP_DiagNurExecCategory where Lbxh=" + ilbxm;
            object obj = SqlHelper.ExecuteScalar(sSql);
            if (obj.ToString() == "" || obj == null)
            {
                sName = "";
            }
            else
            {
                sName = obj.ToString();
            }
            return sName;
        }

        #region  获得诊疗护理从表基础数据维护
        /// <summary>
        ///规范化获取数据库数据的方法  获得诊疗护理从表数据维护
        /// </summary>
        /// <param name="iID">默认传1，表示获得所有诊疗护理明细表数据</param>
        /// <param name="isTy">当isTy为flase，表示获取非停用状态诊疗护理明细基础信息。默认传true</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_DiagNurExecCategoryDetail> GetDiagNurExecCategoryDetails(int iID, bool isTy)
        {
            List<CP_DiagNurExecCategoryDetail> list = new List<CP_DiagNurExecCategoryDetail>();
            try
            {

                string sSql = "";
                DataTable dt = null;
                if (isTy)
                {
                    sSql = @"select Mxxh,MxName,Lbxh,Yxjl,case Yxjl when 1 then '有效' else '无效' end as Extension1,Sfsy,case Sfsy when 0 then '启用' when 1 then '停用' when 2 then '使用中' end as Extension2,Extension,Create_Time,Create_User,Cancel_Time,Cancel_User,OrderValue,JkType,Tbzd,Zdly,Py,Wb,Jkdm,Scdm,Memo from CP_DiagNurExecCategoryDetail where 1=" + iID;
                    dt = SqlHelper.ExecuteDataTable(sSql);
                    if (dt != null)
                    {
                        foreach (DataRow item in dt.Rows)
                        {
                            CP_DiagNurExecCategoryDetail model = new CP_DiagNurExecCategoryDetail();
                            if (item["Mxxh"] != null && item["Mxxh"].ToString() != "")
                            {
                                model.Mxxh = int.Parse(item["Mxxh"].ToString());
                            }

                            model.Name = item["MxName"].ToString();

                            if (item["Lbxh"] != null && item["Lbxh"].ToString() != "")
                            {
                                model.Lbxh = int.Parse(item["Lbxh"].ToString());
                            }
                            if (item["Yxjl"] != null && item["Yxjl"].ToString() != "")
                            {
                                model.Yxjl = int.Parse(item["Yxjl"].ToString());
                            }
                            if (item["Sfsy"] != null && item["Sfsy"].ToString() != "")
                            {
                                model.Sfsy = int.Parse(item["Sfsy"].ToString());
                            }

                            model.Create_Time = item["Create_Time"].ToString();


                            model.Create_User = item["Create_User"].ToString();


                            model.Cancel_Time = item["Cancel_Time"].ToString();


                            model.Cancel_User = item["Cancel_User"].ToString();

                            if (item["OrderValue"] != null && item["OrderValue"].ToString() != "")
                            {
                                model.OrderValue = int.Parse(item["OrderValue"].ToString());
                            }

                            model.JkType = int.Parse(item["JkType"].ToString());

                            model.Tbzd = item["Tbzd"].ToString();

                            model.Zdly = item["Zdly"].ToString();

                            model.Py = item["Py"].ToString();

                            model.Wb = item["Wb"].ToString();

                            model.Jkdm = item["Jkdm"].ToString();

                            model.Scdm = item["Scdm"].ToString();

                            model.Memo = item["Memo"].ToString();
                            if (item["Lbxh"] != null && item["Lbxh"].ToString() != "")
                            {
                                //保存项目分类名称
                                model.Extension = GetLbxmName(int.Parse(item["Lbxh"].ToString()));//item["Extension"].ToString();
                            }

                            model.Extension1 = item["Extension1"].ToString();

                            model.Extension2 = item["Extension2"].ToString();

                            list.Add(model);
                        }
                    }
                }
                else //非停用的所有诊疗护理明细基础信息
                {
                    sSql = @"select * from CP_DiagNurExecCategoryDetail where Yxjl=1 and Sfsy!=1";
                    dt = SqlHelper.ExecuteDataTable(sSql);
                     

                    foreach (DataRow item in dt.Rows)
                    {
                        CP_DiagNurExecCategoryDetail model = new CP_DiagNurExecCategoryDetail();
                        if (item["Mxxh"] != null && item["Mxxh"].ToString() != "")
                        {
                            model.Mxxh = int.Parse(item["Mxxh"].ToString());
                        }
                        else 
                        {
                            model.Mxxh = 1;
                        }

                        model.Name = item["MxName"].ToString();

                         list.Add(model);
                    }
                }
 
                return list;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        #endregion

        #region 规范化的新增数据库数据的方法 新增 诊疗护理从表CP_DiagNurExecCategoryDetail信息
        /// <summary>
        /// 新增诊疗护理明细表操作
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="Operation"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public int InsertDiagNurCategoryDetails(CP_DiagNurExecCategoryDetail model)
        {

            int scalarCount = 0;
            int returnBool = 0;
            try
            {
                if (model == null)
                {
                    model = new CP_DiagNurExecCategoryDetail();
                    returnBool = 0;

                }
                else
                {
                    int iVal = ExistsDetail(model.Name,model.Lbxh);
                    if (iVal == 0)
                    {
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("insert into CP_DiagNurExecCategoryDetail(");
                        strSql.Append("MxName,Lbxh,Yxjl,Sfsy,Create_Time,Create_User,Cancel_Time,Cancel_User,OrderValue,JkType,Tbzd,Zdly,Py,Wb,Jkdm,Scdm,Memo,Extension,Extension1,Extension2)");
                        strSql.Append(" values (");
                        strSql.Append("@MxName,@Lbxh,@Yxjl,@Sfsy,@Create_Time,@Create_User,@Cancel_Time,@Cancel_User,@OrderValue,@JkType,@Tbzd,@Zdly,dbo.fun_getPY(@Py),dbo.fun_getWB(@Wb),@Jkdm,@Scdm,@Memo,@Extension,@Extension1,@Extension2)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
					new SqlParameter("@MxName", SqlDbType.VarChar,100),
					new SqlParameter("@Lbxh", SqlDbType.Int,4),
					new SqlParameter("@Yxjl", SqlDbType.Int,4),
					new SqlParameter("@Sfsy", SqlDbType.Int,4),
					new SqlParameter("@Create_Time", SqlDbType.VarChar,19),
					new SqlParameter("@Create_User", SqlDbType.VarChar,10),
					new SqlParameter("@Cancel_Time", SqlDbType.VarChar,19),
					new SqlParameter("@Cancel_User", SqlDbType.VarChar,10),
					new SqlParameter("@OrderValue", SqlDbType.Decimal,9),
					new SqlParameter("@JkType", SqlDbType.Int,4),
					new SqlParameter("@Tbzd", SqlDbType.VarChar,50),
					new SqlParameter("@Zdly", SqlDbType.VarChar,100),
					new SqlParameter("@Py", SqlDbType.VarChar,50),
					new SqlParameter("@Wb", SqlDbType.VarChar,50),
					new SqlParameter("@Jkdm", SqlDbType.VarChar,50),
					new SqlParameter("@Scdm", SqlDbType.VarChar,50),
					new SqlParameter("@Memo", SqlDbType.VarChar,1000),
					new SqlParameter("@Extension", SqlDbType.VarChar,64),
					new SqlParameter("@Extension1", SqlDbType.VarChar,64),
					new SqlParameter("@Extension2", SqlDbType.VarChar,64)};
                        parameters[0].Value = model.Name;
                        parameters[1].Value = model.Lbxh;
                        parameters[2].Value = model.Yxjl;
                        parameters[3].Value = model.Sfsy;
                        parameters[4].Value = model.Create_Time;
                        parameters[5].Value = model.Create_User;
                        parameters[6].Value = model.Cancel_Time;
                        parameters[7].Value = model.Cancel_User;
                        parameters[8].Value = model.OrderValue;
                        parameters[9].Value = model.JkType;
                        parameters[10].Value = model.Tbzd;
                        parameters[11].Value = model.Zdly;
                        parameters[12].Value = model.Name;
                        parameters[13].Value = model.Name;
                        parameters[14].Value = model.Jkdm;
                        parameters[15].Value = model.Scdm;
                        parameters[16].Value = model.Memo;
                        parameters[17].Value = model.Extension;
                        parameters[18].Value = model.Extension1;
                        parameters[19].Value = model.Extension2;

                        //DataTable dt = null;
                        SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                        returnBool = 1;//数据添加成功
                    }
                    else
                    {
                        returnBool = 2;//数据添加重复
                    }

                }
            }

            catch (Exception ex)
            {
                ThrowException(ex);
                returnBool = 3;//添加报错
            }
            return returnBool;

        }
        #endregion

        #region 规范化的更新数据库数据的方法 更新 诊疗护理从表CP_DiagNurExecCategoryDetail信息
        /// <summary>
        /// 更新诊疗护理从表CP_DiagNurExecCategoryDetail信息操作
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="Operation"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public int UpdateDiagNurExecCategoryDetail(CP_DiagNurExecCategoryDetail model)
        {
            int returnBool = 0;
            int scalarCount = 0;
            try
            {
                if (model == null)
                {
                    model = new CP_DiagNurExecCategoryDetail();
                    returnBool = 0;
                }
                else
                {
                    //int iVal = ExistsDetail(model.MxName,model.Lbxh);
                    //if (iVal == 0)
                    //{
                    string yiSql = string.Format("SELECT COUNT(*) FROM CP_DiagNurExecCategoryDetail WHERE MXNAME ='{0}'",model.Name);
                    scalarCount = Convert.ToInt32(SqlHelper.ExecuteScalar(yiSql));
                    if (scalarCount>=1)
                    {
                        return scalarCount;
                    }
                    else{
                        StringBuilder strSql = new StringBuilder();
                        strSql.Append("update CP_DiagNurExecCategoryDetail set ");
                        strSql.Append("MxName=@MxName,");
                        strSql.Append("Lbxh=@Lbxh,");
                        strSql.Append("Yxjl=@Yxjl,");
                        strSql.Append("Sfsy=@Sfsy,");
                        strSql.Append("Create_Time=@Create_Time,");
                        strSql.Append("Create_User=@Create_User,");
                        strSql.Append("Cancel_Time=@Cancel_Time,");
                        strSql.Append("Cancel_User=@Cancel_User,");
                        strSql.Append("OrderValue=@OrderValue,");
                        strSql.Append("JkType=@JkType,");
                        strSql.Append("Tbzd=@Tbzd,");
                        strSql.Append("Zdly=@Zdly,");
                        strSql.Append("Py=@Py,");
                        strSql.Append("Wb=@Wb,");
                        strSql.Append("Jkdm=@Jkdm,");
                        strSql.Append("Scdm=@Scdm,");
                        strSql.Append("Memo=@Memo,");
                        strSql.Append("Extension=@Extension,");
                        strSql.Append("Extension1=@Extension1,");
                        strSql.Append("Extension2=@Extension2");
                        strSql.Append(" where Mxxh=@Mxxh");
                        SqlParameter[] parameters = {
					new SqlParameter("@MxName", SqlDbType.VarChar,100),
					new SqlParameter("@Lbxh", SqlDbType.Int,4),
					new SqlParameter("@Yxjl", SqlDbType.Int,4),
					new SqlParameter("@Sfsy", SqlDbType.Int,4),
					new SqlParameter("@Create_Time", SqlDbType.VarChar,19),
					new SqlParameter("@Create_User", SqlDbType.VarChar,10),
					new SqlParameter("@Cancel_Time", SqlDbType.VarChar,19),
					new SqlParameter("@Cancel_User", SqlDbType.VarChar,10),
					new SqlParameter("@OrderValue", SqlDbType.Int,4),
					new SqlParameter("@JkType", SqlDbType.Int,4),
					new SqlParameter("@Tbzd", SqlDbType.VarChar,50),
					new SqlParameter("@Zdly", SqlDbType.VarChar,100),
					new SqlParameter("@Py", SqlDbType.VarChar,50),
					new SqlParameter("@Wb", SqlDbType.VarChar,50),
					new SqlParameter("@Jkdm", SqlDbType.VarChar,50),
					new SqlParameter("@Scdm", SqlDbType.VarChar,50),
					new SqlParameter("@Memo", SqlDbType.VarChar,1000),
					new SqlParameter("@Extension", SqlDbType.VarChar,64),
					new SqlParameter("@Extension1", SqlDbType.VarChar,64),
					new SqlParameter("@Extension2", SqlDbType.VarChar,64),
					new SqlParameter("@Mxxh", SqlDbType.Int,4)};
                        parameters[0].Value = model.Name;
                        parameters[1].Value = model.Lbxh;
                        parameters[2].Value = model.Yxjl;
                        parameters[3].Value = model.Sfsy;
                        parameters[4].Value = model.Create_Time;
                        parameters[5].Value = model.Create_User;
                        parameters[6].Value = model.Cancel_Time;
                        parameters[7].Value = model.Cancel_User;
                        parameters[8].Value = model.OrderValue;
                        parameters[9].Value = model.JkType;
                        parameters[10].Value = model.Tbzd;
                        parameters[11].Value = model.Zdly;
                        parameters[12].Value = model.Py;
                        parameters[13].Value = model.Wb;
                        parameters[14].Value = model.Jkdm;
                        parameters[15].Value = model.Scdm;
                        parameters[16].Value = model.Memo;
                        parameters[17].Value = model.Extension;
                        parameters[18].Value = model.Extension1;
                        parameters[19].Value = model.Extension2;
                        parameters[20].Value = model.Mxxh;


                        //DataTable dt = null;
                        SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                }
                        returnBool = 1;
                    //}
                    //else
                    //{
                        //returnBool = 2;
                    //}
                }


            }

            catch (Exception ex)
            {
                ThrowException(ex);
                returnBool = 3;
            }
            return scalarCount;
        }

        #endregion

        #region 规范化的更新数据库数据的方法 更新 诊疗护理从表使用状态
        /// <summary>
        /// 更新诊疗护理从表CP_DiagNurExecCategoryDetail信息操作
        /// </summary>
        /// <param name="iID">编号</param>
        /// <param name="isfsy">使用状态，1表示停用，2表示使用中，0表示正常</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Boolean UpdateSfsyState(int iID,int isfsy)
        {
            Boolean returnBool = false;
            try
            {
               
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update CP_DiagNurExecCategoryDetail set ");
                strSql.Append("Sfsy=@Sfsy");
                strSql.Append(" where Mxxh=@Mxxh");
                SqlParameter[] parameters = {
				
				new SqlParameter("@Sfsy", SqlDbType.Int,4),
				new SqlParameter("@Mxxh", SqlDbType.Int,4)};
                parameters[0].Value = isfsy;
                parameters[1].Value = iID;

                SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                returnBool = true;
            }

            catch (Exception ex)
            {
                ThrowException(ex);
                returnBool = false;
            }
            return returnBool;
        }

        #endregion


        #region 规范化的删除数据库数据的方法 删除诊疗护理从表CP_DiagNurExecCategoryDetail信息
        /// <summary>
        /// 删除CP_DiagNurExecCategoryDetail配置表操作
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="Operation"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Boolean DelDiagNurExecCategoryDetail(int iID)
        {


            Boolean returnBool = false;
            try
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from CP_DiagNurExecCategoryDetail ");
                strSql.Append(" where Mxxh=@Mxxh and sfsy ！=2");
                SqlParameter[] parameters = {
					new SqlParameter("@Mxxh", SqlDbType.Int,4)
			};
                parameters[0].Value = iID;
                SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                returnBool = true;


            }

            catch (Exception ex)
            {
                ThrowException(ex);
                returnBool = false;
            }
            return returnBool;

        }
        #endregion

        #region 规范化的删除数据库数据的方法 删除诊疗护理从表信息，再删除主表信息
        /// <summary>
        /// 删除CP_DiagNurExecCategory配置表操作
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="Operation"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Boolean DelDiagNurExecCategory(int iID)
        {


            Boolean returnBool = false;
            try
            {

                StringBuilder strSql = new StringBuilder();

                //先删除CP_DiagNurExecCategoryDetail从表信息
                strSql.Append("delete from CP_DiagNurExecCategoryDetail ");
                strSql.Append(" where Lbxh =" + iID + " and sfsy ！=2");
               
                SqlHelper.ExecuteNoneQuery(strSql.ToString());


                //再删除CP_DiagNurExecCategory主表信息
                strSql.Append("delete from CP_DiagNurExecCategory ");
                strSql.Append(" where Lbxh=" + iID + " ");
                
                
                SqlHelper.ExecuteNoneQuery(strSql.ToString());
                 returnBool = true;
            }

            catch (Exception ex)
            {
                ThrowException(ex);
                returnBool = false;
            }
            return returnBool;

        }
        #endregion


        /// <summary>
        /// 获得诊疗护理明细项目编号和名称
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_DiagNurExecCategoryDetail> GetDiagNurExecCategoryInfo()
        {
            
            try
            {
                List<CP_DiagNurExecCategoryDetail> diagnurListInfo = new List<CP_DiagNurExecCategoryDetail>();
                string sSql = "";
                sSql = @"select * from CP_DiagNurExecCategoryDetail where Yxjl=1 and Sfsy!=1 ";
                DataTable dt = SqlHelper.ExecuteDataTable(sSql);

                foreach (DataRow item in dt.Rows)
                {
                    CP_DiagNurExecCategoryDetail model = new CP_DiagNurExecCategoryDetail();
                    if (item["Mxxh"] != null && item["Mxxh"].ToString() != "")
                    {
                        model.Mxxh = int.Parse(item["Mxxh"].ToString());
                    }
                    else
                    {
                        model.Mxxh = 1;
                    }

                    model.Name = item["MxName"].ToString();
                    model.Lbxh =int.Parse(item["Lbxh"].ToString());

                    model.Yxjl = int.Parse(item["Yxjl"].ToString());
                    model.Sfsy = int.Parse(item["Sfsy"].ToString());
                         

                    model.Create_Time = item["Create_Time"].ToString();


                    model.Create_User = item["Create_User"].ToString();


                    model.Cancel_Time = item["Cancel_Time"].ToString();


                    model.Cancel_User = item["Cancel_User"].ToString();

 
                    model.OrderValue = int.Parse(item["OrderValue"].ToString());
                            

                    model.JkType = int.Parse(item["JkType"].ToString());

                    model.Tbzd = item["Tbzd"].ToString();

                    model.Zdly = item["Zdly"].ToString();

                    model.Py = item["Py"].ToString();

                    model.Wb = item["Wb"].ToString();

                    model.Jkdm = item["Jkdm"].ToString();

                    model.Scdm = item["Scdm"].ToString();

                    model.Memo = item["Memo"].ToString();
       
                    model.Extension = GetLbxmName(int.Parse(item["Lbxh"].ToString()));//item["Extension"].ToString();
                    model.Extension1 = item["Extension1"].ToString();

                    model.Extension2 = item["Extension2"].ToString();

                    diagnurListInfo.Add(model);
                }
                return diagnurListInfo;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }


        /// <summary>
        /// 根据路径代码获得路径节点信息
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public int GetandCopyPathData(string strName, string strLjms, double zgts, double jcfy, double version, string strShys,
            int yxjl, string strSyks, List<CP_ClinicalDiagnosisList> listAddBzdm,string sLjdm)
        {
            string  sNewLjdm="";
            int reival = 0;
            try
            {
                sNewLjdm = InsertCPList(strName, strLjms, zgts, jcfy, version, strShys, yxjl, strSyks, listAddBzdm);
                reival = CopyPathData(sLjdm, sNewLjdm);
            }
            catch (Exception ex)
            {
                 
                ThrowException(ex);
                reival = 0;
            }

            return reival; 
        }

        /// <summary>
        /// 根据路径代码获得路径节点信息
        /// </summary>
        /// <param name="sLjdm">复制之前的路径代码</param>
        /// <param name="sNewLjdm">复制之后的路径代码</param>
        /// <returns></returns>
        public int CopyPathData(string sLjdm, string sNewLjdm)
        {
            int reival = 0;
            decimal iNewCtyzxh = 0;
            try
            {
                //用事务去处理
                SqlHelper.BeginTransaction();
                StringBuilder strSql = new StringBuilder();

                //根据复制之前的路径代码得到压缩后的xml字符串
                strSql.AppendFormat("select WorkFlowXML from CP_ClinicalPath where Ljdm='{0}'", sLjdm);
                string sWorkFlowXml = SqlHelper.ExecuteScalar(strSql.ToString()).ToString();
                sWorkFlowXml = GetNewPathXml(sWorkFlowXml, sLjdm, sNewLjdm);
                
                
                //根据生成的新路径代码更新路径xml
                strSql = new StringBuilder();
                strSql.AppendFormat("update CP_ClinicalPath set WorkFlowXML = '{0}' where Ljdm = '{1}'", sWorkFlowXml, sNewLjdm);
                SqlHelper.ExecuteNoneQuery(strSql.ToString());


              
                #region 先根据复制之前的路径代码得到路径明细编号PahtDetailID
                strSql = new StringBuilder();
                strSql.AppendFormat("select isnull(ts,0) TS,isnull(Zljh,0) Zljh,* from CP_PathDetail where Ljdm='{0}'", sLjdm);
                DataTable dt = SqlHelper.ExecuteDataTable(strSql.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    strSql = new StringBuilder();
                    strSql.Append("insert into CP_PathDetail(");
                    strSql.Append("PahtDetailID,PrePahtDetailID,NextPahtDetailID,ActivityType,Ljdm,Ts,Ljmc,LJs,Bqms,Zljh)");
                    strSql.Append(" values (");
                    strSql.Append("@PahtDetailID,@PrePahtDetailID,@NextPahtDetailID,@ActivityType,@Ljdm,@Ts,@Ljmc,@LJs,@Bqms,@Zljh)");
                    SqlParameter[] parameters = {
					new SqlParameter("@PahtDetailID", SqlDbType.VarChar,50),
					new SqlParameter("@PrePahtDetailID", SqlDbType.VarChar,50),
					new SqlParameter("@NextPahtDetailID", SqlDbType.VarChar,50),
					new SqlParameter("@ActivityType", SqlDbType.VarChar,50),
					new SqlParameter("@Ljdm", SqlDbType.VarChar,12),
					new SqlParameter("@Ts", SqlDbType.Int,4),
					new SqlParameter("@Ljmc", SqlDbType.VarChar,255),
					new SqlParameter("@LJs", SqlDbType.VarChar,255),
					new SqlParameter("@Bqms", SqlDbType.Text),
					new SqlParameter("@Zljh", SqlDbType.Int,4)};
                    parameters[0].Value = dr["PahtDetailID"].ToString();
                    parameters[1].Value = dr["PrePahtDetailID"].ToString();
                    parameters[2].Value = dr["NextPahtDetailID"].ToString();
                    parameters[3].Value = dr["ActivityType"].ToString();
                    parameters[4].Value = sNewLjdm;
                    parameters[5].Value = int.Parse(dr["Ts"].ToString());
                    parameters[6].Value = dr["Ljmc"].ToString();
                    parameters[7].Value = dr["LJs"].ToString();
                    parameters[8].Value = dr["Bqms"].ToString();
                    parameters[9].Value = int.Parse(dr["Zljh"].ToString());

                    SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                }
                #endregion

                #region 先根据复制之前的路径代码得到进入路径条件信息表 CP_PathEnterJudgeCondition
                strSql = new StringBuilder();
                strSql.AppendFormat("select * from CP_PathEnterJudgeCondition where Ljdm='{0}'", sLjdm);
                DataTable dt1 = SqlHelper.ExecuteDataTable(strSql.ToString());
                foreach (DataRow dr in dt1.Rows)
                {
                    strSql = new StringBuilder();
                    strSql.Append("insert into CP_PathEnterJudgeCondition(");
                    strSql.Append("Lb,Ljdm,Jddm,Sjfl,Jcxm,Xmlb,Ksfw,Jsfw,Dw,Syrq,Bz)");
                    strSql.Append(" values (");
                    strSql.Append("@Lb,@Ljdm,@Jddm,@Sjfl,@Jcxm,@Xmlb,@Ksfw,@Jsfw,@Dw,@Syrq,@Bz)");
                    strSql.Append(";select @@IDENTITY");
                    SqlParameter[] parameters = {
                    new SqlParameter("@Lb", SqlDbType.Int,4),
					new SqlParameter("@Ljdm", SqlDbType.VarChar,12),
					new SqlParameter("@Jddm", SqlDbType.VarChar,50),
					new SqlParameter("@Sjfl", SqlDbType.VarChar,50),
					new SqlParameter("@Jcxm", SqlDbType.VarChar,400),
					new SqlParameter("@Xmlb", SqlDbType.VarChar,50),
					new SqlParameter("@Ksfw", SqlDbType.VarChar,50),
					new SqlParameter("@Jsfw", SqlDbType.VarChar,50),
					new SqlParameter("@Dw", SqlDbType.VarChar,20),
					new SqlParameter("@Syrq", SqlDbType.VarChar,50),
					new SqlParameter("@Bz", SqlDbType.VarChar,200)};

                    parameters[0].Value = dr["LB"] == null ? 0 : int.Parse(dr["LB"].ToString());
                    parameters[1].Value = sNewLjdm;
                    parameters[2].Value = dr["Jddm"].ToString();
                    parameters[3].Value = dr["Sjfl"].ToString();
                    parameters[4].Value = dr["Jcxm"].ToString();
                    parameters[5].Value = dr["Xmlb"].ToString();
                    parameters[6].Value = dr["Ksfw"].ToString();
                    parameters[7].Value = dr["Jsfw"].ToString();
                    parameters[8].Value = dr["Dw"].ToString();
                    parameters[9].Value = dr["Syrq"].ToString();
                    parameters[10].Value = dr["Bz"].ToString();

                    SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                }

                #endregion

                #region 先根据复制之前的路径代码得到路径每个节点下的医嘱明细
                strSql = new StringBuilder();
                strSql.AppendFormat("select isnull(Syfw,0) Syfw, * from CP_AdviceGroup where Ljdm='{0}'", sLjdm);
                DataTable dt2 = SqlHelper.ExecuteDataTable(strSql.ToString());
                if (dt2.Rows.Count > 0)
                {
                    foreach (DataRow dr in dt2.Rows)
                    {
                        strSql = new StringBuilder();
                        strSql.Append("insert into CP_AdviceGroup(");
                        strSql.Append("PahtDetailID,Name,Ljdm,Py,Wb,Ksdm,Bqdm,Ysdm,Syfw,Memo)");
                        strSql.Append(" values (");
                        strSql.Append("@PahtDetailID,@Name,@Ljdm,@Py,@Wb,@Ksdm,@Bqdm,@Ysdm,@Syfw,@Memo)");
                        strSql.Append(";select @@IDENTITY");
                        SqlParameter[] parameters = {
					new SqlParameter("@PahtDetailID", SqlDbType.VarChar,50),
					new SqlParameter("@Name", SqlDbType.VarChar,32),
					new SqlParameter("@Ljdm", SqlDbType.VarChar,12),
					new SqlParameter("@Py", SqlDbType.VarChar,8),
					new SqlParameter("@Wb", SqlDbType.VarChar,8),
					new SqlParameter("@Ksdm", SqlDbType.VarChar,12),
					new SqlParameter("@Bqdm", SqlDbType.VarChar,12),
					new SqlParameter("@Ysdm", SqlDbType.VarChar,6),
					new SqlParameter("@Syfw", SqlDbType.SmallInt,2),
					new SqlParameter("@Memo", SqlDbType.VarChar,16)};
                        parameters[0].Value = dr["PahtDetailID"].ToString();
                        parameters[1].Value = dr["Name"].ToString();
                        parameters[2].Value = sNewLjdm;
                        parameters[3].Value = dr["Py"].ToString();
                        parameters[4].Value = dr["Wb"].ToString();
                        parameters[5].Value = dr["Ksdm"].ToString();
                        parameters[6].Value = dr["Bqdm"].ToString();
                        parameters[7].Value = dr["Ysdm"].ToString();
                        parameters[8].Value = int.Parse(dr["Syfw"].ToString());
                        parameters[9].Value = dr["Memo"].ToString();
                        iNewCtyzxh = decimal.Parse(SqlHelper.ExecuteScalar(strSql.ToString(), parameters).ToString());

                        #region 获得医嘱明细表信息 CP_AdviceGroupDetail
                        strSql = new StringBuilder();
                        strSql.AppendFormat("select * from CP_AdviceGroupDetail where Ctyzxh='{0}'", dr["Ctyzxh"].ToString());
                        DataTable dt3 = SqlHelper.ExecuteDataTable(strSql.ToString());
                        if (dt3.Rows.Count > 0)
                        {
                            foreach (DataRow drr in dt3.Rows)
                            {
                                strSql = new StringBuilder();
                                strSql.Append("insert into CP_AdviceGroupDetail(");
                                strSql.Append("Ctyzxh,Yzbz,Fzxh,Fzbz,Cdxh,Ggxh,Lcxh,Ypdm,Ypmc,Xmlb,Zxdw,Ypjl,Jldw,Dwxs,Dwlb,Yfdm,Pcdm,Zxcs,Zxzq,Zxzqdw,Zdm,Zxsj,Zxts,Ypzsl,Ztnr,Yzlb,MZDM,Zxksdm,Isjj,Yzkx,Extension,Extension1,Extension2,Extension3,Extension4)");
                                strSql.Append(" values (");
                                strSql.Append("@Ctyzxh,@Yzbz,@Fzxh,@Fzbz,@Cdxh,@Ggxh,@Lcxh,@Ypdm,@Ypmc,@Xmlb,@Zxdw,@Ypjl,@Jldw,@Dwxs,@Dwlb,@Yfdm,@Pcdm,@Zxcs,@Zxzq,@Zxzqdw,@Zdm,@Zxsj,@Zxts,@Ypzsl,@Ztnr,@Yzlb,@MZDM,@Zxksdm,@Isjj,@Yzkx,@Extension,@Extension1,@Extension2,@Extension3,@Extension4)");
                                strSql.Append(";select @@IDENTITY");
                                SqlParameter[] parameterss = {
					new SqlParameter("@Ctyzxh", SqlDbType.Decimal,5),
					new SqlParameter("@Yzbz", SqlDbType.SmallInt,2),
					new SqlParameter("@Fzxh", SqlDbType.Decimal,9),
					new SqlParameter("@Fzbz", SqlDbType.SmallInt,2),
					new SqlParameter("@Cdxh", SqlDbType.Decimal,5),
					new SqlParameter("@Ggxh", SqlDbType.Decimal,5),
					new SqlParameter("@Lcxh", SqlDbType.Decimal,5),
					new SqlParameter("@Ypdm", SqlDbType.VarChar,12),
					new SqlParameter("@Ypmc", SqlDbType.VarChar,64),
					new SqlParameter("@Xmlb", SqlDbType.SmallInt,2),
					new SqlParameter("@Zxdw", SqlDbType.VarChar,30),
					new SqlParameter("@Ypjl", SqlDbType.Decimal,9),
					new SqlParameter("@Jldw", SqlDbType.VarChar,30),
					new SqlParameter("@Dwxs", SqlDbType.Decimal,9),
					new SqlParameter("@Dwlb", SqlDbType.SmallInt,2),
					new SqlParameter("@Yfdm", SqlDbType.VarChar,16),
					new SqlParameter("@Pcdm", SqlDbType.VarChar,2),
					new SqlParameter("@Zxcs", SqlDbType.Int,4),
					new SqlParameter("@Zxzq", SqlDbType.Int,4),
					new SqlParameter("@Zxzqdw", SqlDbType.SmallInt,2),
					new SqlParameter("@Zdm", SqlDbType.Char,7),
					new SqlParameter("@Zxsj", SqlDbType.VarChar,64),
					new SqlParameter("@Zxts", SqlDbType.Int,4),
					new SqlParameter("@Ypzsl", SqlDbType.Decimal,9),
					new SqlParameter("@Ztnr", SqlDbType.VarChar,3000),
					new SqlParameter("@Yzlb", SqlDbType.SmallInt,2),
					new SqlParameter("@MZDM", SqlDbType.VarChar,12),
					new SqlParameter("@Zxksdm", SqlDbType.VarChar,12),
					new SqlParameter("@Isjj", SqlDbType.Int,4),
					new SqlParameter("@Yzkx", SqlDbType.Int,4),
					new SqlParameter("@Extension", SqlDbType.VarChar,16),
					new SqlParameter("@Extension1", SqlDbType.VarChar,16),
					new SqlParameter("@Extension2", SqlDbType.VarChar,16),
					new SqlParameter("@Extension3", SqlDbType.VarChar,16),
					new SqlParameter("@Extension4", SqlDbType.VarChar,16)};
                                parameterss[0].Value = iNewCtyzxh;//decimal.Parse(dr["Ctyzxh"].ToString());
                                parameterss[1].Value = int.Parse(drr["Yzbz"].ToString());
                                parameterss[2].Value = decimal.Parse(drr["Fzxh"].ToString());
                                parameterss[3].Value = int.Parse(drr["Fzbz"].ToString());
                                parameterss[4].Value = decimal.Parse(drr["Cdxh"].ToString());
                                parameterss[5].Value = decimal.Parse(drr["Ggxh"].ToString());
                                parameterss[6].Value = decimal.Parse(drr["Lcxh"].ToString());
                                parameterss[7].Value = drr["Ypdm"].ToString();
                                parameterss[8].Value = drr["Ypmc"].ToString();
                                parameterss[9].Value = int.Parse(drr["Xmlb"].ToString());
                                parameterss[10].Value = drr["Zxdw"].ToString();
                                parameterss[11].Value = decimal.Parse(drr["Ypjl"].ToString());
                                parameterss[12].Value = drr["Jldw"].ToString();
                                parameterss[13].Value = decimal.Parse(drr["Dwxs"].ToString());
                                parameterss[14].Value = int.Parse(drr["Dwlb"].ToString());
                                parameterss[15].Value = drr["Yfdm"].ToString();
                                parameterss[16].Value = drr["Pcdm"].ToString();
                                parameterss[17].Value = int.Parse(drr["Zxcs"].ToString());
                                parameterss[18].Value = int.Parse(drr["Zxzq"].ToString());
                                parameterss[19].Value = int.Parse(drr["Zxzqdw"].ToString());
                                parameterss[20].Value = drr["Zdm"].ToString();
                                parameterss[21].Value = drr["Zxsj"].ToString();
                                parameterss[22].Value = int.Parse(drr["Zxts"].ToString());
                                parameterss[23].Value = decimal.Parse(drr["Ypzsl"].ToString());
                                parameterss[24].Value = drr["Ztnr"].ToString();
                                parameterss[25].Value = int.Parse(drr["Yzlb"].ToString());
                                parameterss[26].Value = drr["MZDM"].ToString();
                                parameterss[27].Value = drr["Zxksdm"].ToString();
                                parameterss[28].Value = int.Parse(drr["Isjj"].ToString());
                                parameterss[29].Value = int.Parse(drr["Yzkx"].ToString());
                                parameterss[30].Value = drr["Extension"].ToString();
                                parameterss[31].Value = drr["Extension1"].ToString();
                                parameterss[32].Value = drr["Extension2"].ToString();
                                parameterss[33].Value = drr["Extension3"].ToString();
                                parameterss[34].Value = drr["Extension4"].ToString();

                                SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameterss);
                            }
                        }
                        #endregion

                    }
                }

                #endregion

                #region 根据复制之前的路径代码得到每个节点下诊疗护理信息 CP_DiagNurTemplate
                strSql = new StringBuilder();
                strSql.Append(" Insert into CP_DiagNurTemplate(Ljdm,PathDetailId,Mxxh,MxName,Lbxh,Yxjl,");
                strSql.Append(" Py,Wb,Create_Time,Create_User,Cancel_Time,Cancel_User,Extension,Extension1,");
                strSql.Append(" Extension2,Extension3,Isjj,Iskx,Zxksdm)");
                strSql.Append(" select @Ljdm1 AS Ljdm,PathDetailId,Mxxh,MxName,Lbxh,");
                strSql.Append(" Yxjl,Py,Wb,Create_Time,Create_User,");
                strSql.Append(" Cancel_Time,Cancel_User,Extension,Extension1,Extension2,");
                strSql.Append(" Extension3,Isjj,Iskx,Zxksdm");
                strSql.Append(" from CP_DiagNurTemplate where Ljdm=@Ljdm2");
                SqlParameter[] parameters1 = {
                   new SqlParameter("@Ljdm1", SqlDbType.VarChar,12),
					new SqlParameter("@Ljdm2", SqlDbType.VarChar,12)};
                parameters1[0].Value = sNewLjdm;
                parameters1[1].Value = sLjdm;
                SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters1);


                #endregion

                #region 根据复制之前的路径代码得到每个节点下的异常原因信息 CP_VariationToPath

                strSql = new StringBuilder();
                strSql.Append(" Insert into CP_VariationToPath(Ljdm,ActivityId,Bydm,Yxjl,Create_Time,");
                strSql.Append(" Create_User,Cancel_Time,Cancel_User)");
                strSql.Append(" select @Ljdm1 AS Ljdm,ActivityId,Bydm,Yxjl,Create_Time,");
                strSql.Append(" Create_User,Cancel_Time,Cancel_User");
                strSql.Append(" from CP_VariationToPath where Ljdm=@Ljdm2");
                SqlParameter[] parameters2 = {
                   new SqlParameter("@Ljdm1", SqlDbType.VarChar,12),
					new SqlParameter("@Ljdm2", SqlDbType.VarChar,12)};
                parameters2[0].Value = sNewLjdm;
                parameters2[1].Value = sLjdm;
                SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters2);

                #endregion
                reival = 1;
                SqlHelper.CommitTransaction();
               

            }
            catch (Exception ex)
            {
                SqlHelper.RollbackTransaction();
                ThrowException(ex);
                reival = 0;
            }

            return reival;
        }

        /// <summary>
        /// 重新解压压缩处理要复制的路径xml
        /// </summary>
        /// <param name="sXml">复制之前的路径Xml</param>
        /// <param name="sLjdm"></param>
        /// <param name="sNewLjdm"></param>
        /// <returns></returns>
        public string GetNewPathXml(string sXml, string sLjdm, string sNewLjdm)
        { 
            try
            {
                
                if(sXml!="")
                {
                    //先解压复制之前路径xml,替换newlijdm，再压缩
                    sXml = UnzipContent(sXml);
                    sXml = sXml.Replace(sLjdm, sNewLjdm);
                    sXml = ZipContent(sXml);
                }
 
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return sXml;
        }

    }
}