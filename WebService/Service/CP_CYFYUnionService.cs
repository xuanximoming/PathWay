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
        ///  获取草药方以及明细联合查询信息
        /// </summary>
        /// <param name="strLjdm"></param>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_CYFUnion> GetCyfUnionInfo()
        {
            List<CP_CYFUnion> listInfo = new List<CP_CYFUnion>();
            try
            {

                StringBuilder strSql = new StringBuilder();
                strSql.Append("select idm as ID, Ypdm as Ypdm ,Ypmc as Ypmc,'kg' as Ypgg,Py,Wb,'草药方明细' as yplh,Pcdm, Extension as Cdxh,Extension1 as Ggxh,yplh as Ggdw, Extension2 as Zxdw, Extension3 as Jxdm,Extension4,Jldw,Yfdm ");
                strSql.Append(" FROM CP_CYXDFMX  union all ");
                strSql.Append(" select ID,'' ,cfmc,'协定方',Py,Wb,'草药协定方','','','','','','',cfmc,'','' ");
                strSql.Append(" FROM CP_CYXDF");

                DataTable dt = SqlHelper.ExecuteDataTable(strSql.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    CP_CYFUnion model = new CP_CYFUnion();
                    model.ID = int.Parse(dr["ID"].ToString());
                    model.Ypdm = dr["Ypdm"].ToString();
                    model.Ypmc = dr["Ypmc"].ToString();
                    model.Py = dr["Py"].ToString();
                    model.Wb = dr["Wb"].ToString();
                    model.yplh = dr["yplh"].ToString();
                    model.Pcdm = dr["Pcdm"].ToString();
                    model.Ggxh = dr["Ggxh"].ToString();
                    model.Cdxh = dr["Cdxh"].ToString();
                    model.Ypgg = dr["Ypgg"].ToString();
                    model.Jxdm = dr["Jxdm"].ToString();
                    model.Ggdw = dr["Ggdw"].ToString();
                    model.Zxdw = dr["Zxdw"].ToString();
                    model.Yfdm = dr["Yfdm"].ToString();
                    model.Jldw = dr["Jldw"].ToString();
                    //草药协定方名称
                    model.Extension4 = dr["Extension4"].ToString();
                    listInfo.Add(model);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return listInfo;

        }
    }
}