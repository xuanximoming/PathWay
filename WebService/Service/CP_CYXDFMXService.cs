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
        /// 获得草药处方主表数据
        /// </summary>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_CYXDFMX> GetCyxdfMXInfo()
        {
            List<CP_CYXDFMX> cplist = new List<CP_CYXDFMX>();
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("select ID,cfxh,idm,yplh,Ypdm,Ypmc,Py,Wb,Yfdm,Pcdm,Ypjl,Jldw,Dwlb,");
                strSql.Append("Zxts,Ypsl,cfts,ypbz,lcxmdm,pxxh,Memo,ekxs,ekdw,ekbz,Isjj,Zxksdm,");
                strSql.Append("Yzkx,Extension,Extension1,Extension2,Extension3,Extension4 ");
                strSql.Append(" FROM CP_CYXDFMX ");

                DataTable dt = SqlHelper.ExecuteDataTable(strSql.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    CP_CYXDFMX model = new CP_CYXDFMX();

                    model.ID = int.Parse(dr["ID"].ToString());

                   
                    model.cfxh = dr["cfxh"].ToString();
                     
                    model.idm = int.Parse(dr["idm"].ToString());

                    model.yplh = dr["yplh"].ToString();
                    
                    model.Ypdm = dr["Ypdm"].ToString();
                   
                    model.Ypmc = dr["Ypmc"].ToString();

                    model.Py = dr["Py"].ToString();

                    model.Wb = dr["Wb"].ToString();
                    
                    model.Yfdm = dr["Yfdm"].ToString();
                   
                    model.Pcdm = dr["Pcdm"].ToString();
                    
                    model.Ypjl = decimal.Parse(dr["Ypjl"].ToString());
                    
                    model.Jldw = dr["Jldw"].ToString();
                    
                    model.Dwlb = int.Parse(dr["Dwlb"].ToString());
                    
                    model.Zxts = int.Parse(dr["Zxts"].ToString());
                    
                    model.Ypsl = decimal.Parse(dr["Ypsl"].ToString());
                   
                    model.cfts = int.Parse(dr["cfts"].ToString());
                    
                    model.ypbz = int.Parse(dr["ypbz"].ToString());
                    
                    model.lcxmdm = dr["lcxmdm"].ToString();
                    
                    model.pxxh = int.Parse(dr["pxxh"].ToString());
                    
                    model.Memo = dr["Memo"].ToString();
                    
                    model.ekxs = int.Parse(dr["ekxs"].ToString());
                    
                    model.ekdw = dr["ekdw"].ToString();
                   
                    model.ekbz = int.Parse(dr["ekbz"].ToString());
                    

                    model.Isjj = int.Parse(dr["Isjj"].ToString());
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
        /// 根据草药方主表编号得到该草药方明细信息
        /// </summary>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_CYXDFMX> GetCyxdfMXInfoById(int iID)
        {
            List<CP_CYXDFMX> cplist = new List<CP_CYXDFMX>();
            try
            {
                StringBuilder strSql = new StringBuilder();
            
                strSql.AppendFormat("select ID,cfxh,idm,yplh,Ypdm,Ypmc,Py,Wb,Yfdm,Pcdm,Ypjl,Jldw,Dwlb,");
                strSql.AppendFormat("Zxts,Ypsl,cfts,ypbz,lcxmdm,pxxh,Memo,ekxs,ekdw,ekbz,Isjj,Zxksdm,");
                strSql.AppendFormat("Yzkx,Extension,Extension1,Extension2,Extension3,Extension4 ");
                strSql.AppendFormat(" FROM CP_CYXDFMX where idm='{0}'",iID);

                DataTable dt = SqlHelper.ExecuteDataTable(strSql.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    CP_CYXDFMX model = new CP_CYXDFMX();

                    model.ID = int.Parse(dr["ID"].ToString());


                    model.cfxh = dr["cfxh"].ToString();

                    model.idm = int.Parse(dr["idm"].ToString());

                    model.yplh = dr["yplh"].ToString();

                    model.Ypdm = dr["Ypdm"].ToString();

                    model.Ypmc = dr["Ypmc"].ToString();

                    model.Py = dr["Py"].ToString();

                    model.Wb = dr["Wb"].ToString();

                    model.Yfdm = dr["Yfdm"].ToString();

                    model.Pcdm = dr["Pcdm"].ToString();

                    model.Ypjl = decimal.Parse(dr["Ypjl"].ToString());

                    model.Jldw = dr["Jldw"].ToString();

                    model.Dwlb = int.Parse(dr["Dwlb"].ToString());

                    model.Zxts = int.Parse(dr["Zxts"].ToString());

                    model.Ypsl = decimal.Parse(dr["Ypsl"].ToString());

                    model.cfts = int.Parse(dr["cfts"].ToString());

                    model.ypbz = int.Parse(dr["ypbz"].ToString());

                    model.lcxmdm = dr["lcxmdm"].ToString();

                    model.pxxh = int.Parse(dr["pxxh"].ToString());

                    model.Memo = dr["Memo"].ToString();

                    model.ekxs = int.Parse(dr["ekxs"].ToString());

                    model.ekdw = dr["ekdw"].ToString();

                    model.ekbz = int.Parse(dr["ekbz"].ToString());


                    model.Isjj = int.Parse(dr["Isjj"].ToString());
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
        public int InsertCYXDFMX(CP_CYXDFMX model)
        {
            int reival = 0;
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("insert into CP_CYXDFMX(");
                strSql.Append("cfxh,idm,yplh,Ypdm,Ypmc,Yfdm,Pcdm,Ypjl,Jldw,Dwlb,");
                strSql.Append("Zxts,Ypsl,cfts,ypbz,lcxmdm,pxxh,Memo,ekxs,ekdw,ekbz,");
                strSql.Append("Isjj,Zxksdm,Yzkx,Extension,Extension1,Extension2,Extension3,Extension4,Py,Wb)");
                strSql.Append(" values (");
                strSql.Append("@cfxh,@idm,@yplh,@Ypdm,@Ypmc,@Yfdm,@Pcdm,@Ypjl,@Jldw,@Dwlb,");
                strSql.Append("@Zxts,@Ypsl,@cfts,@ypbz,@lcxmdm,@pxxh,@Memo,@ekxs,@ekdw,@ekbz,");
                strSql.Append("@Isjj,@Zxksdm,@Yzkx,@Extension,@Extension1,@Extension2,@Extension3,@Extension4,@Py,@Wb)");
                strSql.Append(";select @@IDENTITY");
                SqlParameter[] parameters = {
					new SqlParameter("@cfxh", SqlDbType.VarChar,50),
					new SqlParameter("@idm", SqlDbType.Int,4),
					new SqlParameter("@yplh", SqlDbType.VarChar,12),
					new SqlParameter("@Ypdm", SqlDbType.VarChar,12),
					new SqlParameter("@Ypmc", SqlDbType.VarChar,100),
					new SqlParameter("@Yfdm", SqlDbType.VarChar,4),
					new SqlParameter("@Pcdm", SqlDbType.VarChar,4),
					new SqlParameter("@Ypjl", SqlDbType.Decimal,9),
					new SqlParameter("@Jldw", SqlDbType.VarChar,8),
					new SqlParameter("@Dwlb", SqlDbType.SmallInt,2),
					new SqlParameter("@Zxts", SqlDbType.Int,4),
					new SqlParameter("@Ypsl", SqlDbType.Decimal,9),
					new SqlParameter("@cfts", SqlDbType.Int,4),
					new SqlParameter("@ypbz", SqlDbType.Int,4),
					new SqlParameter("@lcxmdm", SqlDbType.VarChar,64),
					new SqlParameter("@pxxh", SqlDbType.Int,4),
					new SqlParameter("@Memo", SqlDbType.VarChar,200),
					new SqlParameter("@ekxs", SqlDbType.Int,4),
					new SqlParameter("@ekdw", SqlDbType.VarChar,8),
					new SqlParameter("@ekbz", SqlDbType.Int,4),
					new SqlParameter("@Isjj", SqlDbType.Int,4),
					new SqlParameter("@Zxksdm", SqlDbType.VarChar,12),
					new SqlParameter("@Yzkx", SqlDbType.Int,4),
					new SqlParameter("@Extension", SqlDbType.VarChar,64),
					new SqlParameter("@Extension1", SqlDbType.VarChar,64),
					new SqlParameter("@Extension2", SqlDbType.VarChar,64),
					new SqlParameter("@Extension3", SqlDbType.VarChar,64),
					new SqlParameter("@Extension4", SqlDbType.VarChar,64),
                    new SqlParameter("@Py", SqlDbType.VarChar,50),
                    new SqlParameter("@Wb", SqlDbType.VarChar,50)};
                parameters[0].Value = model.cfxh;
                parameters[1].Value = model.idm;
                parameters[2].Value = model.yplh;
                parameters[3].Value = model.Ypdm;
                parameters[4].Value = model.Ypmc;
                parameters[5].Value = model.Yfdm;
                parameters[6].Value = model.Pcdm;
                parameters[7].Value = model.Ypjl;
                parameters[8].Value = model.Jldw;
                parameters[9].Value = model.Dwlb;
                parameters[10].Value = model.Zxts;
                parameters[11].Value = model.Ypsl;
                parameters[12].Value = model.cfts;
                parameters[13].Value = model.ypbz;
                parameters[14].Value = model.lcxmdm;
                parameters[15].Value = model.pxxh;
                parameters[16].Value = model.Memo;
                parameters[17].Value = model.ekxs;
                parameters[18].Value = model.ekdw;
                parameters[19].Value = model.ekbz;
                parameters[20].Value = model.Isjj;
                parameters[21].Value = model.Zxksdm;
                parameters[22].Value = model.Yzkx;
                parameters[23].Value = model.Extension;
                parameters[24].Value = model.Extension1;
                parameters[25].Value = model.Extension2;
                parameters[26].Value = model.Extension3;
                parameters[27].Value = model.Extension4;
                parameters[28].Value = model.Py;
                parameters[29].Value = model.Wb;

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
        /// <param name="CP_CYXDFMXs">诊疗护理模板列表</param>
        /// <returns>返回值为1 表示 操作成功；0 表示 插入失败</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public int UpdateCYXDFMX(CP_CYXDFMX model)
        {
            int reival = 0;
            try
            {
                StringBuilder strSql = new StringBuilder();
                strSql.Append("update CP_CYXDFMX set ");
                strSql.Append("cfxh=@cfxh,");
                strSql.Append("idm=@idm,");
                strSql.Append("yplh=@yplh,");
                strSql.Append("Ypdm=@Ypdm,");
                strSql.Append("Ypmc=@Ypmc,");
                strSql.Append("Yfdm=@Yfdm,");
                strSql.Append("Pcdm=@Pcdm,");
                strSql.Append("Ypjl=@Ypjl,");
                strSql.Append("Jldw=@Jldw,");
                strSql.Append("Dwlb=@Dwlb,");
                strSql.Append("Zxts=@Zxts,");
                strSql.Append("Ypsl=@Ypsl,");
                strSql.Append("cfts=@cfts,");
                strSql.Append("ypbz=@ypbz,");
                strSql.Append("lcxmdm=@lcxmdm,");
                strSql.Append("pxxh=@pxxh,");
                strSql.Append("Memo=@Memo,");
                strSql.Append("ekxs=@ekxs,");
                strSql.Append("ekdw=@ekdw,");
                strSql.Append("ekbz=@ekbz,");
                strSql.Append("Isjj=@Isjj,");
                strSql.Append("Zxksdm=@Zxksdm,");
                strSql.Append("Yzkx=@Yzkx,");
                strSql.Append("Extension=@Extension,");
                strSql.Append("Extension1=@Extension1,");
                strSql.Append("Extension2=@Extension2,");
                strSql.Append("Extension3=@Extension3,");
                strSql.Append("Extension4=@Extension4,");
                strSql.Append("Py=@Py,");
                strSql.Append("Wb=@Wb");
                strSql.Append(" where ID=@ID");
                SqlParameter[] parameters = {
					new SqlParameter("@cfxh", SqlDbType.VarChar,50),
					new SqlParameter("@idm", SqlDbType.Int,4),
					new SqlParameter("@yplh", SqlDbType.VarChar,12),
					new SqlParameter("@Ypdm", SqlDbType.VarChar,12),
					new SqlParameter("@Ypmc", SqlDbType.VarChar,100),
					new SqlParameter("@Yfdm", SqlDbType.VarChar,4),
					new SqlParameter("@Pcdm", SqlDbType.VarChar,4),
					new SqlParameter("@Ypjl", SqlDbType.Decimal,9),
					new SqlParameter("@Jldw", SqlDbType.VarChar,8),
					new SqlParameter("@Dwlb", SqlDbType.SmallInt,2),
					new SqlParameter("@Zxts", SqlDbType.Int,4),
					new SqlParameter("@Ypsl", SqlDbType.Decimal,9),
					new SqlParameter("@cfts", SqlDbType.Int,4),
					new SqlParameter("@ypbz", SqlDbType.Int,4),
					new SqlParameter("@lcxmdm", SqlDbType.VarChar,64),
					new SqlParameter("@pxxh", SqlDbType.Int,4),
					new SqlParameter("@Memo", SqlDbType.VarChar,200),
					new SqlParameter("@ekxs", SqlDbType.Int,4),
					new SqlParameter("@ekdw", SqlDbType.VarChar,8),
					new SqlParameter("@ekbz", SqlDbType.Int,4),
					new SqlParameter("@Isjj", SqlDbType.Int,4),
					new SqlParameter("@Zxksdm", SqlDbType.VarChar,12),
					new SqlParameter("@Yzkx", SqlDbType.Int,4),
					new SqlParameter("@Extension", SqlDbType.VarChar,64),
					new SqlParameter("@Extension1", SqlDbType.VarChar,64),
					new SqlParameter("@Extension2", SqlDbType.VarChar,64),
					new SqlParameter("@Extension3", SqlDbType.VarChar,64),
					new SqlParameter("@Extension4", SqlDbType.VarChar,64),
                    new SqlParameter("@Py", SqlDbType.VarChar,50),
                    new SqlParameter("@Wb", SqlDbType.VarChar,50),
					new SqlParameter("@ID", SqlDbType.Int,4)};
                parameters[0].Value = model.cfxh;
                parameters[1].Value = model.idm;
                parameters[2].Value = model.yplh;
                parameters[3].Value = model.Ypdm;
                parameters[4].Value = model.Ypmc;
                parameters[5].Value = model.Yfdm;
                parameters[6].Value = model.Pcdm;
                parameters[7].Value = model.Ypjl;
                parameters[8].Value = model.Jldw;
                parameters[9].Value = model.Dwlb;
                parameters[10].Value = model.Zxts;
                parameters[11].Value = model.Ypsl;
                parameters[12].Value = model.cfts;
                parameters[13].Value = model.ypbz;
                parameters[14].Value = model.lcxmdm;
                parameters[15].Value = model.pxxh;
                parameters[16].Value = model.Memo;
                parameters[17].Value = model.ekxs;
                parameters[18].Value = model.ekdw;
                parameters[19].Value = model.ekbz;
                parameters[20].Value = model.Isjj;
                parameters[21].Value = model.Zxksdm;
                parameters[22].Value = model.Yzkx;
                parameters[23].Value = model.Extension;
                parameters[24].Value = model.Extension1;
                parameters[25].Value = model.Extension2;
                parameters[26].Value = model.Extension3;
                parameters[27].Value = model.Extension4;
                parameters[28].Value = model.Py;
                parameters[29].Value = model.Wb;
                parameters[30].Value = model.ID;

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
        public int DelCYXDFMX(int iID)
        {
            int reival = 0;
            try
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("delete from CP_CYXDFMX ");
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