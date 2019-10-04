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
        ///  获取临床诊疗护理对应路径结点信息
        /// </summary>
        /// <param name="strLjdm"></param>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_DiagNurTemplate> GetDigNurRecordInfo(String strLjdm, String strPathDetailId, String strSyxh)
        {
            List<CP_DiagNurTemplate> listInfo = new List<CP_DiagNurTemplate>();
            try
            {
                StringBuilder sb = new StringBuilder();


                sb.Append(@"select tid id,syxh, Ljdm,PathDetailId,Mxxh,MxName,Lbxh,Yxjl,Iskx, ");
                sb.Append(@"case Iskx when '1' then '可选' when '0' then '必选' end as Extension1,");
                sb.Append(@"Py,Wb,Create_Time,Create_User,Cancel_Time,Cancel_User,Extension,Extension1,Extension2,Extension3 ,1 yzzt,Isjj,Zxksdm");
                sb.Append(" from dbo.CP_DiagNurExecRecord");
                sb.Append(" where Ljdm='" + strLjdm + "' and PathDetailId ='" + strPathDetailId + "' and Syxh = '" + strSyxh + "'");
                sb.Append(" ORDER BY Create_Time desc");
                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                if (dt.Rows.Count == 0)
                {
                    sb = new StringBuilder();
                    sb.Append(@"select ID, Ljdm,PathDetailId,Mxxh,MxName,Lbxh,Yxjl,Iskx,case Iskx when '1' then '可选' when '0' then '必选' end as Extension1, ");
                    sb.Append(@"Py,Wb,Create_Time,Create_User,Cancel_Time,Cancel_User,Extension,Extension1,Extension2,Extension3,0 yzzt,Isjj,Zxksdm");
                    sb.Append(@" from CP_DiagNurTemplate");
                    sb.Append(@" where Ljdm='" + strLjdm + "' and PathDetailId ='" + strPathDetailId + "' ");
                    sb.Append(" ORDER BY Create_Time desc");

                    dt = SqlHelper.ExecuteDataTable(sb.ToString());
                }
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
                    model.Isjj = dr["Isjj"].ToString();//计价类型
                    model.Zxksdm = dr["Zxksdm"].ToString();//是否可选 1为可选；0 为必选
                    model.Iskx = dr["Iskx"].ToString();//执行科室代码
                    model.Yzzt = dr["yzzt"].ToString();//区别数据来源
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
        /// 获得路径节点对应的诊疗护理数据
        /// </summary>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_DiagNurTemplate> GetDiagNurRecordInfo()
        {
            List<CP_DiagNurTemplate> cplist = new List<CP_DiagNurTemplate>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select * from CP_DiagNurExecRecord where 1=1  ");
                sb.Append(" ORDER BY Create_Time desc");

                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    CP_DiagNurTemplate model = new CP_DiagNurTemplate();

                    model.MainID = int.Parse(dr["MainID"].ToString());
                    model.Syxh = decimal.Parse(dr["Syxh"].ToString());
                    model.ID = int.Parse(dr["Tid"].ToString());
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

                    model.Isjj = dr["Isjj"].ToString();//计价类型
                    model.Zxksdm = dr["Zxksdm"].ToString();//是否可选 1为可选；0 为必选
                    model.Iskx = dr["Iskx"].ToString();//执行科室代码
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
        /// 插入数据
        /// </summary>
        /// <param name="model">实体</param>
        /// <returns>返回值为1 表示 操作成功；0 表示 插入失败</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public int InsertDiagNurRecord(CP_DiagNurTemplate model)
        {
            int reival = 0;
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into CP_DiagNurExecRecord(");
                strSql.Append("Syxh,Ljdm,PathDetailId,Mxxh,MxName,Lbxh,Yxjl,Py,Wb,Create_Time,Create_User,Cancel_Time,Cancel_User,Extension,Extension1,Extension2,Extension3,Isjj,Iskx,Zxksdm)");
                strSql.Append(" values (");
                strSql.Append("@Syxh,@Ljdm,@PathDetailId,@Mxxh,@MxName,@Lbxh,@Yxjl,@Py,@Wb,@Create_Time,@Create_User,@Cancel_Time,@Cancel_User,@Extension,@Extension1,@Extension2,@Extension3,@Isjj,@Iskx,@Zxksdm)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@Syxh", SqlDbType.Decimal,5), 
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
                parameters[0].Value = model.Syxh;
                parameters[1].Value = model.ID;
                parameters[2].Value = model.Ljdm;
                parameters[3].Value = model.PathDetailId;
                parameters[4].Value = model.Mxxh;
                parameters[5].Value = model.MxName;
                parameters[6].Value = model.Lbxh;
                parameters[7].Value = model.Yxjl;
                parameters[8].Value = model.Py;
                parameters[9].Value = model.Wb;
                parameters[10].Value = model.Create_Time;
                parameters[11].Value = model.Create_User;
                parameters[12].Value = model.Cancel_Time;
                parameters[13].Value = model.Cancel_User;
                parameters[14].Value = model.Extension;
                parameters[15].Value = model.Extension1;
                parameters[16].Value = model.Extension2;
                parameters[17].Value = model.Extension3;
                parameters[18].Value = model.Isjj;
                parameters[19].Value = model.Iskx;
                parameters[20].Value = model.Zxksdm;
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
        /// 批量插入数据
        /// </summary>
        /// <param name="listmodel">实体列表</param>
        /// <returns>返回值为1 表示 操作成功；0 表示 插入失败</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public int PLInsertDiagNurRecord(List<CP_DiagNurTemplate> listmodel)
        {
            int reival = 0;
            try
            {
                //插入路径执行结果先删除该节点下所有内容然后插入数据
                SqlHelper.BeginTransaction();
                StringBuilder strSql = new StringBuilder();

                strSql.AppendFormat("delete CP_DiagNurExecRecord where Ljdm='{0}' and PathDetailId ='{1}' and Syxh = '{2}'",
                                        listmodel[0].Ljdm,listmodel[0].PathDetailId,listmodel[0].Syxh.ToString());
                SqlHelper.ExecuteNoneQuery(strSql.ToString());

                foreach (CP_DiagNurTemplate cp in listmodel)
                {
                    strSql = new StringBuilder();
                    strSql.Append("insert into CP_DiagNurExecRecord(");
                    strSql.Append("Tid,Syxh,Ljdm,PathDetailId,Mxxh,MxName,Lbxh,Yxjl,Py,Wb,Create_Time,Create_User,Cancel_Time,Cancel_User,Extension,Extension1,Extension2,Extension3,Isjj,Iskx,Zxksdm)");
                    strSql.Append(" values (");
                    strSql.Append("@Tid,@Syxh,@Ljdm,@PathDetailId,@Mxxh,@MxName,@Lbxh,@Yxjl,@Py,@Wb,@Create_Time,@Create_User,@Cancel_Time,@Cancel_User,@Extension,@Extension1,@Extension2,@Extension3,@Isjj,@Iskx,@Zxksdm)");
                    strSql.Append(";select @@IDENTITY");
                    SqlParameter[] parameters = {
                    new SqlParameter("@Tid", SqlDbType.Int),
					new SqlParameter("@Syxh", SqlDbType.Decimal,5),
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
                    parameters[0].Value = cp.ID; 
                    parameters[1].Value = cp.Syxh; 
                    parameters[2].Value = cp.Ljdm;
                    parameters[3].Value = cp.PathDetailId;
                    parameters[4].Value = cp.Mxxh;
                    parameters[5].Value = cp.MxName;

                    parameters[6].Value = cp.Lbxh;
                    parameters[7].Value = cp.Yxjl;
                    parameters[8].Value = cp.Py;
                    parameters[9].Value = cp.Wb;
                    parameters[10].Value =  DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");

                    parameters[11].Value = cp.Create_User;
                    parameters[12].Value = cp.Cancel_Time;
                    parameters[13].Value = cp.Cancel_User;
                    parameters[14].Value = cp.Extension;
                    parameters[15].Value = cp.Extension1;

                    parameters[16].Value = cp.Extension2;
                    parameters[17].Value = cp.Extension3;
                    parameters[18].Value = cp.Isjj;
                    parameters[19].Value = cp.Iskx;
                    parameters[20].Value = cp.Zxksdm;
                    SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                }
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
        /// 更新诊疗护理列表
        /// </summary>
        /// <param name="CP_DiagNurTemplates">诊疗护理模板列表</param>
        /// <returns>返回值为1 表示 操作成功；0 表示 插入失败</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public int UpdateDiagExecRecord(CP_DiagNurTemplate model)
        {
            int reival = 0;
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update CP_DiagNurExecRecord set ");
                strSql.Append("Tid=@Tid,");
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
                strSql.Append(" where MainID=@MainID");
                SqlParameter[] parameters = {
                    new SqlParameter("@Tid", SqlDbType.Int,4),
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
					new SqlParameter("@MainID", SqlDbType.Int,4)};
                parameters[0].Value = model.ID;
                parameters[1].Value = model.Ljdm;
                parameters[2].Value = model.PathDetailId;
                parameters[3].Value = model.Mxxh;
                parameters[4].Value = model.MxName;
                parameters[5].Value = model.Lbxh;
                parameters[6].Value = model.Yxjl;
                parameters[7].Value = model.Py;
                parameters[8].Value = model.Wb;
                parameters[9].Value = model.Create_Time;
                parameters[10].Value = model.Create_User;
                parameters[11].Value = model.Cancel_Time;
                parameters[12].Value = model.Cancel_User;
                parameters[13].Value = model.Extension;
                parameters[14].Value = model.Extension1;
                parameters[15].Value = model.Extension2;
                parameters[16].Value = model.Extension3;
                parameters[17].Value = model.Iskx;
                parameters[18].Value = model.Isjj;
                parameters[19].Value = model.Zxksdm;
                parameters[20].Value = model.MainID;

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
        public int DelDiagNurRecord(int iID)
        {
            int reival = 0;
            try
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from CP_DiagNurExecRecord ");
                strSql.Append(" where MainID=@MainID");
                SqlParameter[] parameters = {
					new SqlParameter("@MainID", SqlDbType.Int,4)
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

        /// <summary>
        /// 批量删除诊疗护理记录
        /// </summary>
        /// <returns>返回值为1 表示 操作成功；0 表示 插入失败</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public int PLDelDiagNurRecord(List<int> listiID)
        {
            int reival = 0;
            try
            {

                StringBuilder strSql = new StringBuilder();
                for (int i = 0; i < listiID.Count; i++)
                {
                    strSql.Append("delete from CP_DiagNurExecRecord ");
                    strSql.Append(" where MainID=@MainID");
                    SqlParameter[] parameters = {
					new SqlParameter("@MainID", SqlDbType.Int,4)
			        };
                    parameters[0].Value = listiID[i];

                    SqlHelper.ExecuteNoneQuery(strSql.ToString(), parameters);
                }
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