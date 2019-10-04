
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
        /// 添加医嘱套餐明细
        /// </summary>
        /// <param name="where">医嘱套餐明细实体</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal AddCP_AdviceSuitDetail(CP_AdviceSuitDetail suit)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                object[] tempobject = new object[25];
                tempobject[0] = suit.Yzbz;
                tempobject[1] = suit.Jldw;
                tempobject[2] = suit.Ypdm;
                tempobject[3] = suit.Ypjl;
                tempobject[4] = suit.Yfdm;

                tempobject[5] = suit.Ztnr;
                tempobject[6] = suit.Yznr;

                tempobject[7] = suit.Cdxh;
                tempobject[8] = suit.Ggxh;
                tempobject[9] = suit.Lcxh;

                tempobject[10] = suit.Xmlb;
                tempobject[11] = suit.Yzlb;

                tempobject[12] = suit.Ctyzxh;
                tempobject[13] = suit.Zxts;
                tempobject[14] = suit.Pcdm;//laolaowhnFixed add(Nextval)
                tempobject[15] = suit.Ypmc;
                tempobject[16] = suit.Jjlx;
                tempobject[17] = suit.Zxksdm;
                //add by luff 20130313
                tempobject[18] = suit.Yzkx;
                tempobject[19] = suit.Extension;
                tempobject[20] = suit.Extension1;
                tempobject[21] = suit.Extension2;
                tempobject[22] = suit.Extension3;
                tempobject[23] = suit.Extension4;
                 
                sb.AppendFormat(@"insert into CP_AdviceSuitDetail(
                Yzbz,Jldw,Ypdm,Ypjl,Yfdm,
                Ztnr,Yznr,Cdxh,Ggxh,Lcxh,
                Xmlb,Yzlb,Ctyzxh,Zxts,Pcdm,
                Ypmc,Isjj,Zxksdm,Yzkx,Extension,Extension1,Extension2,Extension3,Extension4) values(
                '{0}','{1}','{2}','{3}','{4}',
                '{5}','{6}','{7}','{8}','{9}'
                ,'{10}','{11}','{12}','{13}','{14}',
                '{15}','{16}','{17}','{18}','{19}','{20}','{21}','{22}','{23}')
                update CP_AdviceSuitDetail set Fzxh=Ctmxxh where Fzxh is null
                update CP_AdviceSuitDetail set Fzbz='3500' where Fzbz is null "
                    , tempobject);
                SqlHelper.ExecuteNoneQuery(sb.ToString());
                return 1;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return -1;

            }


        }
        /// <summary>
        /// 编辑医嘱套餐明细
        /// </summary>
        /// <param name="where">医嘱套餐明细实体</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal EditCP_AdviceSuitDetail(CP_AdviceSuitDetail suit)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                object[] tempobject = new object[25];

                tempobject[0] = suit.Yzbz;
                tempobject[1] = suit.Jldw;
                tempobject[2] = suit.Ypdm;
                tempobject[3] = suit.Ypjl;
                tempobject[4] = suit.Yfdm;

                tempobject[5] = suit.Ztnr;
                tempobject[6] = suit.Yznr;

                tempobject[7] = suit.Cdxh;
                tempobject[8] = suit.Ggxh;
                tempobject[9] = suit.Lcxh;

                tempobject[10] = suit.Xmlb;
                tempobject[11] = suit.Yzlb;

                tempobject[12] = suit.Ctyzxh;
                tempobject[13] = suit.Zxts;
                tempobject[14] = suit.Pcdm;
                tempobject[15] = suit.Ctmxxh;

                tempobject[16] = suit.Zxksdm;
                tempobject[17] = suit.Jjlx;

                //add by luff 20130313
                tempobject[18] = suit.Yzkx;
                tempobject[19] = suit.Extension;
                tempobject[20] = suit.Extension1;
                tempobject[21] = suit.Extension2;
                tempobject[22] = suit.Extension3;
                tempobject[23] = suit.Extension4;

                sb.AppendFormat(@"update  CP_AdviceSuitDetail set 
                Yzbz='{0}',Jldw='{1}',Ypdm='{2}',Ypjl='{3}',Yfdm='{4}',
                Ztnr='{5}',Yznr='{6}',Cdxh='{7}',Ggxh='{8}',Lcxh='{9}', 
                Xmlb='{10}',Yzlb='{11}',Ctyzxh='{12}',Zxts='{13}',Pcdm='{14}',zxksdm = '{16}',isjj = '{17}',
                Yzkx = '{18}',Extension = '{19}',Extension1 = '{20}',Extension2 = '{21}',Extension3 = '{22}',Extension4 = '{23}' where Ctmxxh={15}"
                    , tempobject);
                SqlHelper.ExecuteNoneQuery(sb.ToString());
                return 1;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return -1;

            }

        }
        /// <summary>
        /// 删除医嘱套餐明细
        /// </summary>
        /// <param name="where">医嘱套餐明细主键</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal DeleteCP_AdviceSuitDetail(Decimal Ctmxxh)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select Fzbz from CP_AdviceSuitDetail where Ctmxxh = '" + Ctmxxh + "'");
                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString()); 
                {
                    if (dt.Rows.Count > 0)
                    {
                        if (dt.Rows[0][0].ToString().Trim() != "3500")
                        { 
                           return 2;// add by luff 2012-08-20 表示该条记录有成组。
                        }
                    }
                }
                sb.AppendFormat(@"delete from CP_AdviceSuitDetail where CP_AdviceSuitDetail.Ctmxxh={0}", Ctmxxh);
                SqlHelper.ExecuteNoneQuery(sb.ToString());
                return 1;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return -1;
            }

        }
        /// <summary>
        /// 查询医嘱套餐明细
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_AdviceSuitDetail> GetCP_AdviceSuitDetail(String where)
        {
            List<CP_AdviceSuitDetail> cplist = new List<CP_AdviceSuitDetail>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@" select CP_AdviceSuitDetail.*, 
                CASE  Fzbz WHEN 3501 THEN '┓' WHEN 3599 then '┛' WHEN 3502 then '┃'  else '' END AS FzbzSymbol,
                CP_DataCategoryDetail.Name as YzbzName,
                CP_DrugUseage.Name as YfdmName,
                CP_AdviceFrequency.Name as PcdmName,
                zxksdm,isjj,Yzkx,Extension,Extension1,Extension2,Extension3,Extension4 
                from CP_AdviceSuitDetail
                left join CP_DataCategoryDetail on CP_DataCategoryDetail.Mxbh=CP_AdviceSuitDetail.Yzbz and CP_DataCategoryDetail.Lbbh = 27
                left join CP_DrugUseage on CP_DrugUseage.Yfdm=CP_AdviceSuitDetail.Yfdm
                left join CP_AdviceFrequency on CP_AdviceFrequency.Pcdm=CP_AdviceSuitDetail.Pcdm
                where 1=1  ");
                sb.Append(where);
                sb.Append(" order by Fzxh, Fzbz");
                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    CP_AdviceSuitDetail cp = new CP_AdviceSuitDetail();
                    cp.Ctmxxh = ConvertMy.ToDecimal(dr["Ctmxxh"]);
                    cp.Yzbz = ConvertMy.ToString(dr["Yzbz"]);
                    cp.Jldw = ConvertMy.ToString(dr["Jldw"]);
                    cp.Ypdm = ConvertMy.ToString(dr["Ypdm"]);
                    cp.Ypjl = ConvertMy.ToString(dr["Ypjl"]);
                    cp.Yfdm = ConvertMy.ToString(dr["Yfdm"]);
                    cp.Pcdm = ConvertMy.ToString(dr["Pcdm"]);

                    cp.Ztnr = ConvertMy.ToString(dr["Ztnr"]);
                    cp.Yznr = ConvertMy.ToString(dr["Yznr"]);
                    cp.Zxts = ConvertMy.ToString(dr["Zxts"]);
                    cp.Fzbz = ConvertMy.ToString(dr["Fzbz"]);
                    cp.Fzxh = ConvertMy.ToString(dr["Fzxh"]);
                    cp.FzbzSymbol = ConvertMy.ToString(dr["FzbzSymbol"]);
                    cp.PcdmName = ConvertMy.ToString(dr["PcdmName"]);
                    cp.YzbzName = ConvertMy.ToString(dr["YzbzName"]);
                    cp.YfdmName = ConvertMy.ToString(dr["YfdmName"]);
                    cp.Xmlb = ConvertMy.ToString(dr["Xmlb"]);
                    cp.Yzlb = ConvertMy.ToString(dr["Yzlb"]);
                    cp.Jjlx = ConvertMy.ToInt32(dr["Isjj"]);
                    cp.Zxksdm = ConvertMy.ToString(dr["zxksdm"]);

                    cp.Yzkx = dr["Yzkx"] ==null ? 0:ConvertMy.ToInt32(dr["Yzkx"]);
                    cp.Extension = ConvertMy.ToString(dr["Extension"]);

                    cp.Extension1 = ConvertMy.ToString(dr["Extension1"]);
                    cp.Extension2 = ConvertMy.ToString(dr["Extension2"]);
                    cp.Extension3 = ConvertMy.ToString(dr["Extension3"]); 
                    cp.Extension4 = dr["Extension4"].ToString() == string.Empty ? 0 : Convert.ToInt16(dr["Extension4"].ToString()); 

                    cplist.Add(cp);
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return cplist;


        }
        /// <summary>
        /// 成组
        /// </summary>
        /// <param name="where">医嘱套餐明细实体列表</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal GroupCP_AdviceSuitDetail(List<CP_AdviceSuitDetail> suitDetailList)
        {
            #region 分析成组的顺序
            CP_AdviceSuitDetail first = null;
            CP_AdviceSuitDetail last = null;
            List<CP_AdviceSuitDetail> Middlelist = new List<CP_AdviceSuitDetail>();
            foreach (CP_AdviceSuitDetail d in suitDetailList)
            {
                if (first == null) first = d;
                if (ConvertMy.ToDecimal(d.Ctmxxh) < ConvertMy.ToDecimal(first.Ctmxxh)) first = d;
                if (last == null) last = d;
                if (ConvertMy.ToDecimal(d.Ctmxxh) > ConvertMy.ToDecimal(first.Ctmxxh)) last = d;
                Middlelist.Add(d);
            }
            Middlelist.Remove(first);
            Middlelist.Remove(last);
            foreach (CP_AdviceSuitDetail d in Middlelist)
            {
                d.Fzxh = first.Ctmxxh.ToString();
                d.Fzbz = "3502";
            }
            first.Fzbz = "3501";
            first.Fzxh = first.Ctmxxh.ToString();
            last.Fzbz = "3599";
            last.Fzxh = first.Ctmxxh.ToString();
            #endregion
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"update  CP_AdviceSuitDetail set Fzxh='{0}',Fzbz='{1}' where Ctmxxh={2}"
                    , first.Fzxh, first.Fzbz, first.Ctmxxh);
                sb.AppendFormat(@"update  CP_AdviceSuitDetail set Fzxh='{0}',Fzbz='{1}' where Ctmxxh={2}"
                    , last.Fzxh, last.Fzbz, last.Ctmxxh);
                for (int i = 0; i < Middlelist.Count; i++)
                {

                    sb.AppendFormat(@"update  CP_AdviceSuitDetail set Fzxh='{0}',Fzbz='{1}' where Ctmxxh={2}"
                 , Middlelist[i].Fzxh, Middlelist[i].Fzbz, Middlelist[i].Ctmxxh);

                }
                SqlHelper.ExecuteNoneQuery(sb.ToString());
                return 1;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return -1;

            }

        }
        /// <summary>
        /// 取消成组
        /// </summary>
        /// <param name="where">分组序号</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal CancleGroupCP_AdviceSuitDetail(String Fzxh)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"update  CP_AdviceSuitDetail set Fzxh=Ctmxxh,Fzbz='3500' where Fzxh={0}"
                 , Fzxh);
                SqlHelper.ExecuteNoneQuery(sb.ToString());
                return 1;
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return -1;

            }

        }
    }


}
