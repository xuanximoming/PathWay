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
        /// 获得草药处方主表数据
        /// </summary>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_CYXDF> GetCyxdfInfo()
        {
            List<CP_CYXDF> cplist = new List<CP_CYXDF>();
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select ID,cfmc,Py,Wb,czyh,cjrq,yplh,Ksdm,mbbz,cfts,jlzt,Yfdm,lrdm,tsbz,ylmfl,zxyjr,");
                strSql.Append("sqdmbxh,jdcfbz,Isjj,Zxksdm,Yzkx,Extension,Extension1,Extension2,Extension3,Extension4 ");
                strSql.Append(" FROM CP_CYXDF ");

                DataTable dt = SqlHelper.ExecuteDataTable(strSql.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    CP_CYXDF model = new CP_CYXDF();

                    model.ID = int.Parse(dr["ID"].ToString());
                   
                    model.Py = dr["Py"].ToString();
                    model.Wb = dr["Wb"].ToString();
                    model.cfmc = dr["cfmc"].ToString();
                    model.Isjj = int.Parse(dr["Isjj"].ToString());
                     
                    model.czyh = dr["czyh"].ToString();
                    
                    model.cjrq = dr["cjrq"].ToString();
                    
                    model.yplh = dr["yplh"].ToString();
                   
                    model.Ksdm = dr["Ksdm"].ToString();
                    
                    model.mbbz = int.Parse(dr["mbbz"].ToString());
                    model.cfts = int.Parse(dr["cfts"].ToString());
                    
                    model.jlzt = int.Parse(dr["jlzt"].ToString());
                    model.Yfdm = dr["Yfdm"].ToString();
                    model.lrdm = dr["lrdm"].ToString();
                    model.tsbz = int.Parse(dr["tsbz"].ToString());
                    model.ylmfl = dr["ylmfl"].ToString();
                    model.zxyjr = dr["zxyjr"].ToString();
                    model.sqdmbxh = int.Parse(dr["sqdmbxh"].ToString());
                    model.jdcfbz = int.Parse(dr["jdcfbz"].ToString());
                   

                    model.Zxksdm = dr["Zxksdm"].ToString();
                    model.Yzkx = int.Parse(dr["Yzkx"].ToString());
                    model.Extension = dr["Extension"].ToString();
                    model.Extension1 = dr["Extension1"].ToString();
                    model.Extension2 = dr["Extension2"].ToString();
                    model.Extension3 = dr["Extension3"].ToString();
                    model.Extension4 = dr["Extension4"].ToString();

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
        public int InsertCYXDF(CP_CYXDF model)
        {
            int reival = 0;
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into CP_CYXDF(");
                strSql.Append("cfmc,Py,Wb,czyh,cjrq,yplh,Ksdm,mbbz,cfts,jlzt,Yfdm,lrdm,tsbz,ylmfl,zxyjr,sqdmbxh,jdcfbz,Isjj,Zxksdm,Yzkx,Extension,Extension1,Extension2,Extension3,Extension4)");
                strSql.Append(" values (");
                strSql.Append("@cfmc,dbo.fun_getPY(@Py),dbo.fun_getWB(@Wb),@czyh,@cjrq,@yplh,@Ksdm,@mbbz,@cfts,@jlzt,@Yfdm,@lrdm,@tsbz,@ylmfl,@zxyjr,@sqdmbxh,@jdcfbz,@Isjj,@Zxksdm,@Yzkx,@Extension,@Extension1,@Extension2,@Extension3,@Extension4)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@cfmc", SqlDbType.VarChar,1000),
					new SqlParameter("@Py", SqlDbType.VarChar,50),
					new SqlParameter("@Wb", SqlDbType.VarChar,50),
					new SqlParameter("@czyh", SqlDbType.VarChar,12),
					new SqlParameter("@cjrq", SqlDbType.Char,19),
					new SqlParameter("@yplh", SqlDbType.VarChar,12),
					new SqlParameter("@Ksdm", SqlDbType.VarChar,12),
					new SqlParameter("@mbbz", SqlDbType.Int,4),
					new SqlParameter("@cfts", SqlDbType.Int,4),
					new SqlParameter("@jlzt", SqlDbType.Int,4),
					new SqlParameter("@Yfdm", SqlDbType.VarChar,12),
					new SqlParameter("@lrdm", SqlDbType.VarChar,100),
					new SqlParameter("@tsbz", SqlDbType.Int,4),
					new SqlParameter("@ylmfl", SqlDbType.VarChar,100),
					new SqlParameter("@zxyjr", SqlDbType.VarChar,16),
					new SqlParameter("@sqdmbxh", SqlDbType.Int,4),
					new SqlParameter("@jdcfbz", SqlDbType.Int,4),
					new SqlParameter("@Isjj", SqlDbType.Int,4),
					new SqlParameter("@Zxksdm", SqlDbType.VarChar,12),
					new SqlParameter("@Yzkx", SqlDbType.Int,4),
					new SqlParameter("@Extension", SqlDbType.VarChar,64),
					new SqlParameter("@Extension1", SqlDbType.VarChar,64),
					new SqlParameter("@Extension2", SqlDbType.VarChar,64),
					new SqlParameter("@Extension3", SqlDbType.VarChar,64),
					new SqlParameter("@Extension4", SqlDbType.VarChar,64)};
                parameters[0].Value = model.cfmc;
                parameters[1].Value = model.cfmc;
                parameters[2].Value = model.cfmc;
                parameters[3].Value = model.czyh;
                parameters[4].Value = model.cjrq;
                parameters[5].Value = model.yplh;
                parameters[6].Value = model.Ksdm;
                parameters[7].Value = model.mbbz;
                parameters[8].Value = model.cfts;
                parameters[9].Value = model.jlzt;
                parameters[10].Value = model.Yfdm;
                parameters[11].Value = model.lrdm;
                parameters[12].Value = model.tsbz;
                parameters[13].Value = model.ylmfl;
                parameters[14].Value = model.zxyjr;
                parameters[15].Value = model.sqdmbxh;
                parameters[16].Value = model.jdcfbz;
                parameters[17].Value = model.Isjj;
                parameters[18].Value = model.Zxksdm;
                parameters[19].Value = model.Yzkx;
                parameters[20].Value = model.Extension;
                parameters[21].Value = model.Extension1;
                parameters[22].Value = model.Extension2;
                parameters[23].Value = model.Extension3;
                parameters[24].Value = model.Extension4;

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
        /// <param name="CP_CYXDFs">诊疗护理模板列表</param>
        /// <returns>返回值为1 表示 操作成功；0 表示 插入失败</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public int UpdateCYXDF(CP_CYXDF model)
        {
            int reival = 0;
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update CP_CYXDF set ");
                strSql.Append("cfmc=@cfmc,");
                strSql.Append("Py=dbo.fun_getPY(@Py),");
                strSql.Append("Wb=dbo.fun_getWB(@Wb),");
                strSql.Append("czyh=@czyh,");
                strSql.Append("cjrq=@cjrq,");
                strSql.Append("yplh=@yplh,");
                strSql.Append("Ksdm=@Ksdm,");
                strSql.Append("mbbz=@mbbz,");
                strSql.Append("cfts=@cfts,");
                strSql.Append("jlzt=@jlzt,");
                strSql.Append("Yfdm=@Yfdm,");
                strSql.Append("lrdm=@lrdm,");
                strSql.Append("tsbz=@tsbz,");
                strSql.Append("ylmfl=@ylmfl,");
                strSql.Append("zxyjr=@zxyjr,");
                strSql.Append("sqdmbxh=@sqdmbxh,");
                strSql.Append("jdcfbz=@jdcfbz,");
                strSql.Append("Isjj=@Isjj,");
                strSql.Append("Zxksdm=@Zxksdm,");
                strSql.Append("Yzkx=@Yzkx,");
                strSql.Append("Extension=@Extension,");
                strSql.Append("Extension1=@Extension1,");
                strSql.Append("Extension2=@Extension2,");
                strSql.Append("Extension3=@Extension3,");
                strSql.Append("Extension4=@Extension4");
                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = {
					new SqlParameter("@cfmc", SqlDbType.VarChar,1000),
					new SqlParameter("@Py", SqlDbType.VarChar,50),
					new SqlParameter("@Wb", SqlDbType.VarChar,50),
					new SqlParameter("@czyh", SqlDbType.VarChar,12),
					new SqlParameter("@cjrq", SqlDbType.Char,19),
					new SqlParameter("@yplh", SqlDbType.VarChar,12),
					new SqlParameter("@Ksdm", SqlDbType.VarChar,12),
					new SqlParameter("@mbbz", SqlDbType.Int,4),
					new SqlParameter("@cfts", SqlDbType.Int,4),
					new SqlParameter("@jlzt", SqlDbType.Int,4),
					new SqlParameter("@Yfdm", SqlDbType.VarChar,12),
					new SqlParameter("@lrdm", SqlDbType.VarChar,100),
					new SqlParameter("@tsbz", SqlDbType.Int,4),
					new SqlParameter("@ylmfl", SqlDbType.VarChar,100),
					new SqlParameter("@zxyjr", SqlDbType.VarChar,16),
					new SqlParameter("@sqdmbxh", SqlDbType.Int,4),
					new SqlParameter("@jdcfbz", SqlDbType.Int,4),
					new SqlParameter("@Isjj", SqlDbType.Int,4),
					new SqlParameter("@Zxksdm", SqlDbType.VarChar,12),
					new SqlParameter("@Yzkx", SqlDbType.Int,4),
					new SqlParameter("@Extension", SqlDbType.VarChar,64),
					new SqlParameter("@Extension1", SqlDbType.VarChar,64),
					new SqlParameter("@Extension2", SqlDbType.VarChar,64),
					new SqlParameter("@Extension3", SqlDbType.VarChar,64),
					new SqlParameter("@Extension4", SqlDbType.VarChar,64),
					new SqlParameter("@ID", SqlDbType.Int,4)};
                parameters[0].Value = model.cfmc;
                parameters[1].Value = model.cfmc;
                parameters[2].Value = model.cfmc;
                parameters[3].Value = model.czyh;
                parameters[4].Value = model.cjrq;
                parameters[5].Value = model.yplh;
                parameters[6].Value = model.Ksdm;
                parameters[7].Value = model.mbbz;
                parameters[8].Value = model.cfts;
                parameters[9].Value = model.jlzt;
                parameters[10].Value = model.Yfdm;
                parameters[11].Value = model.lrdm;
                parameters[12].Value = model.tsbz;
                parameters[13].Value = model.ylmfl;
                parameters[14].Value = model.zxyjr;
                parameters[15].Value = model.sqdmbxh;
                parameters[16].Value = model.jdcfbz;
                parameters[17].Value = model.Isjj;
                parameters[18].Value = model.Zxksdm;
                parameters[19].Value = model.Yzkx;
                parameters[20].Value = model.Extension;
                parameters[21].Value = model.Extension1;
                parameters[22].Value = model.Extension2;
                parameters[23].Value = model.Extension3;
                parameters[24].Value = model.Extension4;
                parameters[25].Value = model.ID;

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
        public int DelCYXDF(int iID)
        {
            int reival = 0;
            try
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from CP_CYXDF ");
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