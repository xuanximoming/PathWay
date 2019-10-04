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
using YidanSoft.Tool;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 插入并查询关键药品
        /// </summary>
        /// <param name="drug"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_MasterDrugs> MaintainCP_MasterDrug(CP_MasterDrugs parameter, String Operation)
        {
            List<CP_MasterDrugs> list = new List<CP_MasterDrugs>();
            try
            {
                if (parameter == null) parameter = new CP_MasterDrugs();
                SqlParameter[] parameters = new SqlParameter[] 
                { 

                    new SqlParameter("@Operation",Operation),
                    new SqlParameter("@Cdxh",   parameter.Cdxh	),
                    new SqlParameter("@Txfs",   parameter.Txfs	),
                    new SqlParameter("@ZgdmCj", parameter.ZgdmCj ),	
                    new SqlParameter("@Cjsj",   parameter.Cjsj	),
                    new SqlParameter("@ZgdmXg", parameter.ZgdmXg ),	
                    new SqlParameter("@Xgsj",   parameter.Xgsj	),
             
                    new SqlParameter("@Jsbm",     parameter.Jsbm		)

                };

                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_MasterDrugs", parameters, CommandType.StoredProcedure);

                foreach (DataRow item in dt.Rows)
                {
                    CP_MasterDrugs Temp = new CP_MasterDrugs();
                    Temp.Cdxh = ConvertMy.ToString(item["Cdxh"]);
                    Temp.Txfs = ConvertMy.ToString(item["Txfs"]);
                    Temp.ZgdmCj = ConvertMy.ToString(item["ZgdmCj"]);
                    Temp.Cjsj = ConvertMy.ToString(item["Cjsj"]);
                    Temp.ZgdmXg = ConvertMy.ToString(item["ZgdmXg"]);
                    Temp.Xgsj = ConvertMy.ToString(item["Xgsj"]);
                    Temp.Bz = ConvertMy.ToString(item["Bz"]);

                    Temp.Ypmc = ConvertMy.ToString(item["Ypmc"]);
                    Temp.Ypgg = ConvertMy.ToString(item["Ypgg"]);
                    Temp.Cjmc = ConvertMy.ToString(item["Cjmc"]);
                    Temp.Lsj = ConvertMy.ToString(item["Lsj"]);
                    Temp.ZgdmCjName = ConvertMy.ToString(item["ZgdmCjName"]);
                    Temp.IsCheck = ConvertMy.ToString(item["IsCheck"]);

                    list.Add(Temp);


                }
                return list;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }


        /// <summary>
        /// 插入并查询关键药品角色
        /// </summary>
        /// <param name="drug"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_MasterDrugRoles> MaintainCP_MasterDrugRoles(CP_MasterDrugRoles parameter, String Operation)
        {
            List<CP_MasterDrugRoles> list = new List<CP_MasterDrugRoles>();
            try
            {
                if (parameter == null) parameter = new CP_MasterDrugRoles();

                SqlParameter[] parameters = new SqlParameter[] 
                { 

                    new SqlParameter("@Operation",Operation),
                    new SqlParameter("@Jsbm",   parameter.Jsbm	),
                    new SqlParameter("@Jsmc",   parameter.Jsmc	),
                    new SqlParameter("@ZgdmCj", parameter.ZgdmCj ),	
                    new SqlParameter("@Cjsj",   parameter.Cjsj	),
                    new SqlParameter("@ZgdmXg", parameter.ZgdmXg ),	
                    new SqlParameter("@Xgsj",   parameter.Xgsj	),
                    new SqlParameter("@Bz",     parameter.Bz		)

                };
                DataTable dt = SqlHelper.ExecuteDataTable("usp_CP_MasterDrugRoles", parameters, CommandType.StoredProcedure);
                //if (dt.Rows.Count > 0)
                //{
                if (dt.Rows[0][0].ToString().Trim() == "-1")
                {
                    list = null;
                }
                else
                {
                    foreach (DataRow item in dt.Rows)
                    {
                        CP_MasterDrugRoles Temp = new CP_MasterDrugRoles();
                        Temp.Jsbm = ConvertMy.ToString(item["Jsbm"]);
                        Temp.Jsmc = ConvertMy.ToString(item["Jsmc"]);
                        Temp.ZgdmCj = ConvertMy.ToString(item["ZgdmCj"]);
                        Temp.Cjsj = ConvertMy.ToString(item["Cjsj"]);
                        Temp.ZgdmXg = ConvertMy.ToString(item["ZgdmXg"]);
                        Temp.Xgsj = ConvertMy.ToString(item["Xgsj"]);
                        Temp.Bz = ConvertMy.ToString(item["Bz"]);
                        Temp.ZgdmCjName = ConvertMy.ToString(item["ZgdmCjName"]);


                        list.Add(Temp);
                    }
                }

                //}

                return list;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        /// <summary>
        /// 插入并查询关键药品对应角色
        /// </summary>
        /// <param name="drug"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_MasterDrug2Role> MaintainCP_MasterDrug2Role(CP_MasterDrug2Role parameter, String Operation)
        {
            List<CP_MasterDrug2Role> list = new List<CP_MasterDrug2Role>();
            try
            {
                if (parameter == null) parameter = new CP_MasterDrug2Role();

                SqlParameter[] parameters = new SqlParameter[] 
                { 

                    new SqlParameter("@Operation",Operation),
                    new SqlParameter("@Jsbm",   parameter.Jsbm	),
                    new SqlParameter("@Cdxh",   parameter.Cdxh	),
                    new SqlParameter("@ZgdmCj", parameter.ZgdmCj ),	
                    new SqlParameter("@Cjsj",   parameter.Cjsj	),
                    new SqlParameter("@ZgdmXg", parameter.ZgdmXg ),	
                    new SqlParameter("@Xgsj",   parameter.Xgsj	),
                    new SqlParameter("@Bz",     parameter.Bz		)

                };
                DataTable dt = null;
                if (Operation.Contains(YidanSoft.Tool.Operation.Insert.ToString()) || Operation.Contains(YidanSoft.Tool.Operation.Delete.ToString()))
                {
                    SqlHelper.ExecuteNoneQuery("usp_CP_MasterDrug2Role", parameters, CommandType.StoredProcedure);
                }
                if (Operation.Contains(YidanSoft.Tool.Operation.Select.ToString()))
                {
                    dt = SqlHelper.ExecuteDataTable("usp_CP_MasterDrug2Role", parameters, CommandType.StoredProcedure);
                }
                if (dt != null && Operation.Contains(YidanSoft.Tool.Operation.Select.ToString()))
                    foreach (DataRow item in dt.Rows)
                    {
                        CP_MasterDrug2Role Temp = new CP_MasterDrug2Role();
                        Temp.Jsbm = ConvertMy.ToString(item["Jsbm"]);
                        Temp.Cdxh = ConvertMy.ToString(item["Cdxh"]);
                        Temp.ZgdmCj = ConvertMy.ToString(item["ZgdmCj"]);
                        Temp.Cjsj = ConvertMy.ToString(item["Cjsj"]);
                        Temp.ZgdmXg = ConvertMy.ToString(item["ZgdmXg"]);
                        Temp.Xgsj = ConvertMy.ToString(item["Xgsj"]);
                        Temp.Bz = ConvertMy.ToString(item["Bz"]);

                        list.Add(Temp);
                    }
                return list;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        /// <summary>
        /// 更新操作通过删除再重新插入实现
        /// </summary>
        /// <param name="drug"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Boolean MaintainCP_MasterDrug2RoleUpdate(String jsbm, List<CP_MasterDrug2Role> parameters)
        {
            MaintainCP_MasterDrug2Role(new CP_MasterDrug2Role() { Jsbm = jsbm }, Operation.Delete.ToString());
            foreach (var item in parameters)
            {
                MaintainCP_MasterDrug2Role(new CP_MasterDrug2Role() { Jsbm = item.Jsbm, Cdxh = item.Cdxh }, Operation.Insert.ToString());
            }

            return true;
        }


        /// <summary>
        /// 插入并查询 角色对应用户
        /// </summary>
        /// <param name="drug"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_MasterDrug2User> MaintainP_MasterDrug2User(CP_MasterDrug2User parameter, String Operation)
        {
            List<CP_MasterDrug2User> list = new List<CP_MasterDrug2User>();
            try
            {
                if (parameter == null) parameter = new CP_MasterDrug2User();

                SqlParameter[] parameters = new SqlParameter[] 
                { 
                   
                    new SqlParameter("@Operation",Operation),
                    new SqlParameter("@Jsbm",   parameter.Jsbm	),
                    new SqlParameter("@Zgdm",   parameter.Zgdm	),
                    new SqlParameter("@ZgdmCj", parameter.ZgdmCj ),	
                    new SqlParameter("@Cjsj",   parameter.Cjsj	),
                    new SqlParameter("@ZgdmXg", parameter.ZgdmXg ),	
                    new SqlParameter("@Xgsj",   parameter.Xgsj	),
                    new SqlParameter("@Bz",     parameter.Bz)

                };


                DataTable dt = null;
                if (Operation.Contains(YidanSoft.Tool.Operation.Delete.ToString()) || Operation.Contains(YidanSoft.Tool.Operation.Insert.ToString()))
                {
                    SqlHelper.ExecuteNoneQuery("usp_CP_MasterDrug2User", parameters, CommandType.StoredProcedure);
                }
                if (Operation.Contains(YidanSoft.Tool.Operation.Select.ToString()))
                {
                    dt = SqlHelper.ExecuteDataTable("usp_CP_MasterDrug2User", parameters, CommandType.StoredProcedure);
                }

                if (dt != null && Operation.Contains(YidanSoft.Tool.Operation.Select.ToString()))
                    foreach (DataRow item in dt.Rows)
                    {
                        CP_MasterDrug2User Temp = new CP_MasterDrug2User();
                        Temp.Jsbm = ConvertMy.ToString(item["Jsbm"]);
                        Temp.Zgdm = ConvertMy.ToString(item["Zgdm"]);
                        Temp.ZgdmCj = ConvertMy.ToString(item["ZgdmCj"]);
                        Temp.Cjsj = ConvertMy.ToString(item["Cjsj"]);
                        Temp.ZgdmXg = ConvertMy.ToString(item["ZgdmXg"]);
                        Temp.Xgsj = ConvertMy.ToString(item["Xgsj"]);
                        Temp.Bz = ConvertMy.ToString(item["Bz"]);
                        Temp.Jsmc = ConvertMy.ToString(item["Jsmc"]);
                        Temp.Name = ConvertMy.ToString(item["Name"]);
                        Temp.Zgxb = ConvertMy.ToString(item["sexy"]);
                        list.Add(Temp);
                    }
                return list;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return list;
        }

        /// <summary>
        /// 更新操作通过删除再重新插入实现
        /// </summary>
        /// <param name="parameter"></param>
        /// <param name="Operation"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public Boolean MaintainP_MasterDrug2UserUpdate(List<CP_MasterDrug2User> parameter, String zgdm)
        {
            Boolean returnBool = false;
            try
            {

                MaintainP_MasterDrug2User(new CP_MasterDrug2User() { Zgdm = zgdm }, Operation.Delete.ToString());
                foreach (var item in parameter)
                {
                    MaintainP_MasterDrug2User(new CP_MasterDrug2User()
                    {
                        Zgdm = item.Zgdm,
                        Jsbm = item.Jsbm,
                        ZgdmCj = item.ZgdmCj,
                        Cjsj = item.Cjsj,
                        ZgdmXg = item.ZgdmXg,
                        Xgsj = item.Xgsj,
                        Bz = item.Bz

                    }, Operation.Insert.ToString());
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return returnBool;
            }
            return returnBool;
        }

        /// <summary>
        /// 查询药品用户角色信息
        /// </summary>
        /// <param name="zgdm">职工代码</param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_MasterDrug2User> MaintainP_MasterDrug2UserSelect(String zgdm)
        {
            //Boolean returnBool = false;
            List<CP_MasterDrug2User> list = null;
            try
            {
                list = MaintainP_MasterDrug2User(new CP_MasterDrug2User() { Zgdm = zgdm }, Operation.Select.ToString());
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return null;
            }
            return list;
        }


        /// <summary>
        /// 返回字符串，判定授权是否成功，A,B,C,分别表示帐号是否存在，该用户是否拥有该药物的授权，
        /// </summary>
        /// <param name="cdxhPara"></param>
        /// <param name="zgdmPara"></param>
        /// <param name="PasswdPara"></param>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public String MasterDrugsSynthesisQuery(String cdxhPara, String zgdmPara, String PasswdPara)
        {
            String returnStr = "";
            try
            {
                SqlParameter[] parameters = new SqlParameter[] 
                { 
                    new SqlParameter("@Zgdm",zgdmPara),
                    new SqlParameter("@Cdxh",   cdxhPara	),
                    new SqlParameter("@Passwd",   PasswdPara	)
                };
                DataSet ds = SqlHelper.ExecuteDataSet("usp_CP_MasterDrugsSynthesisQuery", parameters, CommandType.StoredProcedure);

                returnStr = string.Format("{0},{1},{2},{3}", ds.Tables[0].Rows[0][0].ToString(), ds.Tables[1].Rows[0][0].ToString()
                    , ds.Tables[0].Rows[0][0].ToString() == "0" ? "" : ds.Tables[2].Rows[0][0].ToString()
                    , ds.Tables[0].Rows[0][0].ToString() == "0" ? "" : ds.Tables[2].Rows[0][1].ToString());
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return returnStr;
            }
            return returnStr;
        }


    }

}