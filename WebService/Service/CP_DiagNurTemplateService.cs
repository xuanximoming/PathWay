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
        /// <summary>
        /// 获得路径节点对应的诊疗护理数据
        /// </summary>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_DiagNurTemplate> GetDiagNurTemplateInfo()
        {
            List<CP_DiagNurTemplate> cplist = new List<CP_DiagNurTemplate>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select * from CP_DiagNurTemplate where 1=1  ");
                sb.Append(" ORDER BY Create_Time desc");

                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    CP_DiagNurTemplate model = new CP_DiagNurTemplate();
 
                    model.ID = int.Parse(dr["ID"].ToString());
                    model.Ljdm = dr["Ljdm"].ToString();
                    model.PathDetailId = dr["PathDetailId"].ToString();
                    model.Mxxh = int.Parse(dr["Mxxh"].ToString());
                    model.MxName = dr["MxName"].ToString();
                    model.Lbxh = int.Parse(dr["Lbxh"].ToString());
                    model.Yxjl = int.Parse(dr["Yxjl"].ToString());
                    model.Py = dr["Py"].ToString();
                    model.Wb = dr["Wb"].ToString();
                    model.Create_Time = dr["Create_Time"].ToString();
                    model.Create_User = dr["Create_User"].ToString();
                    model.Cancel_Time = dr["Cancel_Time"].ToString();
                    model.Cancel_User = dr["Cancel_User"].ToString();

                    model.Isjj = dr["Isjj"].ToString();
                    model.Zxksdm = dr["Zxksdm"].ToString();
                    model.Iskx = dr["Iskx"].ToString();
                    model.Extension = dr["Extension"].ToString();
                    model.Extension1 = dr["Extension1"].ToString();
                    model.Extension2 = dr["Extension2"].ToString();
                    model.Extension3 = dr["Extension3"].ToString();
                   

                    cplist.Add(model);
                }
                return cplist;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return cplist;
            }

        }

        /// <summary>
        ///  获取临床诊疗护理对应路径结点信息
        /// </summary>
        /// <param name="strLjdm"></param>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_DiagNurTemplate> GetDigNurToPathInfo(String strLjdm, String strPathDetailId)
        {
            List<CP_DiagNurTemplate> listInfo = new List<CP_DiagNurTemplate>();
            try
            {
                
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select ID, Ljdm,PathDetailId,Mxxh,MxName,Lbxh,Yxjl, case Iskx when '1' then '可选' when '0' then '必选' end as Extension1,Py,Wb,Create_Time,Create_User,Cancel_Time,Cancel_User,Extension,Extension1,Extension2,Extension3,Isjj,Iskx,Zxksdm from CP_DiagNurTemplate where Ljdm='" + strLjdm + "' and PathDetailId ='" + strPathDetailId + "' ");
                sb.Append(" ORDER BY Create_Time desc");

                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    CP_DiagNurTemplate model = new CP_DiagNurTemplate();

                    model.ID = int.Parse(dr["ID"].ToString());
                    model.Ljdm = dr["Ljdm"].ToString();
                    model.PathDetailId = dr["PathDetailId"].ToString();
                    model.Mxxh = int.Parse(dr["Mxxh"].ToString());
                    model.MxName = dr["MxName"].ToString();
                    model.Lbxh = int.Parse(dr["Lbxh"].ToString());
                    model.Yxjl = int.Parse(dr["Yxjl"].ToString());
                    model.Py = dr["Py"].ToString();
                    model.Wb = dr["Wb"].ToString();
                    model.Create_Time = dr["Create_Time"].ToString();
                    model.Create_User = dr["Create_User"].ToString();
                    model.Cancel_Time = dr["Cancel_Time"].ToString();
                    model.Cancel_User = dr["Cancel_User"].ToString();
                    model.Extension = dr["Extension"].ToString();
                    model.Extension1 = dr["Extension1"].ToString();
                    model.Extension2 = dr["Extension2"].ToString();
                    model.Extension3 = dr["Extension3"].ToString();
                    model.Isjj = dr["Isjj"].ToString();
                    model.Zxksdm = dr["Zxksdm"].ToString();
                    model.Iskx = dr["Iskx"].ToString();

                    listInfo.Add(model);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return listInfo;

        }
        /// <summary>
        /// 插入数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回值为1 表示 操作成功；0 表示 插入失败</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public int InsertDiagNurTemp(CP_DiagNurTemplate model)
        {
            int reival = 0;
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into CP_DiagNurTemplate(");
                strSql.Append("Ljdm,PathDetailId,Mxxh,MxName,Lbxh,Yxjl,Py,Wb,Create_Time,Create_User,Cancel_Time,Cancel_User,Extension,Extension1,Extension2,Extension3,Isjj,Iskx,Zxksdm)");
                strSql.Append(" values (");
                strSql.Append("@Ljdm,@PathDetailId,@Mxxh,@MxName,@Lbxh,@Yxjl,@Py,@Wb,@Create_Time,@Create_User,@Cancel_Time,@Cancel_User,@Extension,@Extension1,@Extension2,@Extension3,@Isjj,@Iskx,@Zxksdm)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@Ljdm", SqlDbType.VarChar,12),
					new SqlParameter("@PathDetailId", SqlDbType.VarChar,50),
					new SqlParameter("@Mxxh", SqlDbType.Int,4),
					new SqlParameter("@MxName", SqlDbType.VarChar,100),
					new SqlParameter("@Lbxh", SqlDbType.Int,4),
					new SqlParameter("@Yxjl", SqlDbType.Int,4),
					new SqlParameter("@Py", SqlDbType.VarChar,50),
					new SqlParameter("@Wb", SqlDbType.VarChar,50),
					new SqlParameter("@Create_Time", SqlDbType.VarChar,19),
					new SqlParameter("@Create_User", SqlDbType.VarChar,10),
					new SqlParameter("@Cancel_Time", SqlDbType.VarChar,19),
					new SqlParameter("@Cancel_User", SqlDbType.VarChar,10),
					new SqlParameter("@Extension", SqlDbType.VarChar,64),
					new SqlParameter("@Extension1", SqlDbType.VarChar,64),
					new SqlParameter("@Extension2", SqlDbType.VarChar,64),
					new SqlParameter("@Extension3", SqlDbType.VarChar,64),
                    new SqlParameter("@Isjj", SqlDbType.VarChar,64),
					new SqlParameter("@Iskx", SqlDbType.VarChar,64),
					new SqlParameter("@Zxksdm", SqlDbType.VarChar,64)};
                parameters[0].Value = model.Ljdm;
                parameters[1].Value = model.PathDetailId;
                parameters[2].Value = model.Mxxh;
                parameters[3].Value = model.MxName;
                parameters[4].Value = model.Lbxh;
                parameters[5].Value = model.Yxjl;
                parameters[6].Value = model.Py;
                parameters[7].Value = model.Wb;
                parameters[8].Value = model.Create_Time;
                parameters[9].Value = model.Create_User;
                parameters[10].Value = model.Cancel_Time;
                parameters[11].Value = model.Cancel_User;
                parameters[12].Value = model.Extension;
                parameters[13].Value = model.Extension1;
                parameters[14].Value = model.Extension2;
                parameters[15].Value = model.Extension3;
                parameters[16].Value = model.Isjj;
                parameters[17].Value = model.Iskx;
                parameters[18].Value = model.Zxksdm;

                SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                reival = 1;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                reival = 0;
            }

            return reival;

        }

        /// <summary>
        /// 更新诊疗护理列表
        /// </summary>
        /// <param name="CP_DiagNurTemplates">诊疗护理模板列表</param>
        /// <returns>返回值为1 表示 操作成功；0 表示 插入失败</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public int UpdateDiagNurTemp(CP_DiagNurTemplate model)
        {
            int reival = 0;
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update CP_DiagNurTemplate set ");
                strSql.Append("Ljdm=@Ljdm,");
                strSql.Append("PathDetailId=@PathDetailId,");
                strSql.Append("Mxxh=@Mxxh,");
                strSql.Append("MxName=@MxName,");
                strSql.Append("Lbxh=@Lbxh,");
                strSql.Append("Yxjl=@Yxjl,");
                strSql.Append("Py=@Py,");
                strSql.Append("Wb=@Wb,");
                strSql.Append("Create_Time=@Create_Time,");
                strSql.Append("Create_User=@Create_User,");
                strSql.Append("Cancel_Time=@Cancel_Time,");
                strSql.Append("Cancel_User=@Cancel_User,");
                strSql.Append("Extension=@Extension,");
                strSql.Append("Extension1=@Extension1,");
                strSql.Append("Extension2=@Extension2,");
                strSql.Append("Extension3=@Extension3,");
                strSql.Append("Iskx=@Iskx,");
                strSql.Append("Isjj=@Isjj,");
                strSql.Append("Zxksdm=@Zxksdm");
                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = {
					new SqlParameter("@Ljdm", SqlDbType.VarChar,12),
					new SqlParameter("@PathDetailId", SqlDbType.VarChar,50),
					new SqlParameter("@Mxxh", SqlDbType.Int,4),
					new SqlParameter("@MxName", SqlDbType.VarChar,100),
					new SqlParameter("@Lbxh", SqlDbType.Int,4),
					new SqlParameter("@Yxjl", SqlDbType.Int,4),
					new SqlParameter("@Py", SqlDbType.VarChar,50),
					new SqlParameter("@Wb", SqlDbType.VarChar,50),
					new SqlParameter("@Create_Time", SqlDbType.VarChar,19),
					new SqlParameter("@Create_User", SqlDbType.VarChar,10),
					new SqlParameter("@Cancel_Time", SqlDbType.VarChar,19),
					new SqlParameter("@Cancel_User", SqlDbType.VarChar,10),
					new SqlParameter("@Extension", SqlDbType.VarChar,64),
					new SqlParameter("@Extension1", SqlDbType.VarChar,64),
					new SqlParameter("@Extension2", SqlDbType.VarChar,64),
					new SqlParameter("@Extension3", SqlDbType.VarChar,64),
                    new SqlParameter("@Iskx", SqlDbType.VarChar,64),
					new SqlParameter("@Isjj", SqlDbType.VarChar,64),
					new SqlParameter("@Zxksdm", SqlDbType.VarChar,64),
					new SqlParameter("@ID", SqlDbType.Int,4)};
                parameters[0].Value = model.Ljdm;
                parameters[1].Value = model.PathDetailId;
                parameters[2].Value = model.Mxxh;
                parameters[3].Value = model.MxName;
                parameters[4].Value = model.Lbxh;
                parameters[5].Value = model.Yxjl;
                parameters[6].Value = model.Py;
                parameters[7].Value = model.Wb;
                parameters[8].Value = model.Create_Time;
                parameters[9].Value = model.Create_User;
                parameters[10].Value = model.Cancel_Time;
                parameters[11].Value = model.Cancel_User;
                parameters[12].Value = model.Extension;
                parameters[13].Value = model.Extension1;
                parameters[14].Value = model.Extension2;
                parameters[15].Value = model.Extension3;
                parameters[16].Value = model.Iskx;
                parameters[17].Value = model.Isjj;
                parameters[18].Value = model.Zxksdm;
                parameters[19].Value = model.ID;

                SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                reival = 1;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                reival = 0;
            }

            return reival;

        }


        /// <summary>
        /// 删除诊疗护理
        /// </summary>
        /// <returns>返回值为1 表示 操作成功；0 表示 插入失败</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public int DelDiagNurTemp(int iID)
        {
            int reival = 0;
            try
            {
                 
                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from CP_DiagNurTemplate ");
                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = {
					new SqlParameter("@ID", SqlDbType.Int,4)
			};
                parameters[0].Value = iID;

                SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                reival = 1;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                reival = 0;
            }

            return reival;

        }
    }
}