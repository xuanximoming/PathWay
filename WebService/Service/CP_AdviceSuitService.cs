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
        /// 添加医嘱套餐
        /// </summary>
        /// <param name="where">医嘱套餐主表实体</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal AddCP_AdviceSuit(CP_AdviceSuit suit)
        {
            try
            {
                StringBuilder sb = new StringBuilder();

                sb.Append(@"select Name from CP_AdviceSuit where Name='" + suit.Name + "' ");
                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                if (dt.Rows.Count > 0)
                {

                    return 0;
                }
                else
                {
                    object[] tempobject = new object[10];
                    tempobject[0] = suit.Name;
                    tempobject[1] = suit.Py;
                    tempobject[2] = suit.Wb;
                    tempobject[3] = suit.Zgdm;
                    tempobject[4] = suit.Ksdm;
                    tempobject[5] = suit.Bqdm;
                    tempobject[6] = suit.Ysdm;
                    tempobject[7] = suit.Syfw;
                    tempobject[8] = suit.Memo;
                    tempobject[9] = suit.CategoryId;


                    sb.AppendFormat(@"insert into CP_AdviceSuit(Name,Py,Wb,Zgdm,Ksdm,Bqdm,Ysdm,Syfw,Memo,CategoryId) values('{0}','{1}','{2}','{3}','{4}','{5}','{6}','{7}','{8}','{9}')
                                  update CP_AdviceSuit set OrderNum=Ctyzxh where OrderNum is null or OrderNum=0"
                        , tempobject);


                    SqlHelper.ExecuteNoneQuery(sb.ToString());
                    return 1;
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return -1;
            }
        }
        /// <summary>
        /// 编辑医嘱套餐
        /// </summary>
        /// <param name="where">医嘱套餐主表实体</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal EditCP_AdviceSuit(CP_AdviceSuit suit)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select Name from CP_AdviceSuit where Name='" + 
                    suit.Name + "' " + " and Ctyzxh <> " + "'" + suit.Ctyzxh + "'");
                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                if (dt.Rows.Count > 0)
                {
                    return 0;
                }
                else
                {
                    object[] tempobject = new object[20];
                    tempobject[0] = suit.Name;
                    tempobject[1] = suit.Py;
                    tempobject[2] = suit.Wb;
                    tempobject[3] = suit.Ksdm;
                    tempobject[4] = suit.Bqdm;
                    tempobject[5] = suit.Ysdm;
                    tempobject[6] = suit.Syfw;
                    tempobject[7] = suit.Memo;

                    tempobject[8] = suit.UserReason1;
                    tempobject[9] = suit.UserReason2;
                    tempobject[10] = suit.UserReason3;
                    tempobject[11] = suit.Ctyzxh;

                    sb.AppendFormat(@"update  CP_AdviceSuit set Name='{0}',Py='{1}',Wb='{2}',Ksdm='{3}',Bqdm='{4}',Ysdm='{5}',Syfw='{6}',Memo='{7}',
                                  UserReason1='{8}',
                                  UserReason2='{9}',
                                  UserReason3='{10}' where Ctyzxh={11}"
                        , tempobject);
                    SqlHelper.ExecuteNoneQuery(sb.ToString());
                    return 1;
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return -1;
            }

        }
        /// <summary>
        /// 删除医嘱套餐
        /// </summary>
        /// <param name="where">医嘱套餐主键</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Decimal DeleteCP_AdviceSuit(Decimal Ctyzxh)
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.AppendFormat(@"delete from CP_AdviceSuit where Ctyzxh={0}
                                  delete from CP_AdviceSuitDetail where CP_AdviceSuitDetail.Ctyzxh={0}", Ctyzxh);

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
        /// 查询医嘱套餐主表
        /// </summary>
        /// <param name="where">查询条件</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<CP_AdviceSuit> GetCP_AdviceSuit(String where)
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(@" select * from CP_AdviceSuit
                                    where 1=1  ");
            sb.Append(where);
            sb.Append(" order by OrderNum asc");
            List<CP_AdviceSuit> cplist = new List<CP_AdviceSuit>();
            try
            {

                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    CP_AdviceSuit cp = new CP_AdviceSuit();
                    cp.Name = ConvertMy.ToString(dr["Name"]);
                    cp.Py = ConvertMy.ToString(dr["Py"]);
                    cp.Wb = ConvertMy.ToString(dr["Wb"]);
                    cp.Ksdm = ConvertMy.ToString(dr["Ksdm"]);
                    cp.Bqdm = ConvertMy.ToString(dr["Bqdm"]);
                    cp.Ysdm = ConvertMy.ToString(dr["Ysdm"]);
                    cp.Syfw = ConvertMy.ToString(dr["Syfw"]);
                    cp.Memo = ConvertMy.ToString(dr["Memo"]);
                    cp.Ctyzxh = ConvertMy.ToDecimal(dr["Ctyzxh"]);
                    cp.OrderNum = ConvertMy.ToString(dr["OrderNum"]);
                    cp.CategoryId = ConvertMy.ToString(dr["CategoryId"]);
                    cp.UserReason1 = ConvertMy.ToString(dr["UserReason1"]);
                    cp.UserReason2 = ConvertMy.ToString(dr["UserReason2"]);
                    cp.UserReason3 = ConvertMy.ToString(dr["UserReason3"]);


                    cplist.Add(cp);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
                return cplist;
            }
            return cplist;

        }


    }
}
