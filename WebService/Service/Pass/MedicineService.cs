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
using YidanSoft.Tool;
namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 查询药品
        /// </summary>
        /// <param name="Name">药品名称</param>
        /// <param name="CategoryTwo">二级分类</param>
        /// <param name="CategoryThree">三级分类</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<Medicine> GetMedicines(String Name, String CategoryTwo, String CategoryThree)
        {
            List<Medicine> cplist = new List<Medicine>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select * from Medicine where  ");
                sb.AppendFormat(" ( Name like  '%{0}%'", Name);
                sb.AppendFormat(" or Pinyin like  '%{0}%' )", Name);
                if (CategoryTwo != null && CategoryTwo!="全部")
                    sb.AppendFormat(" and CategoryTwo =  '{0}'", CategoryTwo);
                if (CategoryThree != null && CategoryThree!="")
                    sb.AppendFormat(" and CategoryThree =  '{0}'", CategoryThree);
                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    Medicine cp = new Medicine();
                    cp.Name = ConvertMy.ToString(dr["Name"]);
                    cp.Specification = ConvertMy.ToString(dr["Specification"]);
                    cp.ApplyTo = ConvertMy.ToString(dr["ApplyTo"]);
                    cp.ReferenceUsage = ConvertMy.ToString(dr["ReferenceUsage"]);
                    cp.Meno = ConvertMy.ToString(dr["Meno"]);
                    cp.CategoryTwo = ConvertMy.ToString(dr["CategoryTwo"]);
                    cp.CategoryThree = ConvertMy.ToString(dr["CategoryThree"]);
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
        /// 查询分类
        /// </summary>
        /// <returns>分类列表</returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public Dictionary<String,List<String>> GetMedicineCategory()
        {
            Dictionary<String,List<String>> dictionary=new Dictionary<string,List<string>>();
            List<String> lstTwo = new List<string>();
            List<String> lstThree = new List<string>();

            dictionary.Add("Two",lstTwo);
            dictionary.Add("Three",lstThree);

            try
            {
                StringBuilder sb = new StringBuilder();
                StringBuilder sb2 = new StringBuilder();
                sb.Append(@" select distinct CategoryTwo from Medicine ");
                sb2.Append(@" select distinct CategoryThree,CategoryTwo from Medicine "  );
                DataSet ds = SqlHelper.ExecuteDataSet( sb.ToString());
                DataSet ds2 = SqlHelper.ExecuteDataSet( sb2.ToString());
                
                foreach (DataRow dr in ds .Tables[0].Rows)
                {
                    dictionary["Two"].Add(ConvertMy.ToString(dr["CategoryTwo"]));
                }
                foreach (DataRow dr in ds2.Tables[0].Rows)
                {
                    dictionary["Three"].Add(ConvertMy.ToString(dr["CategoryThree"]) + "$" + ConvertMy.ToString(dr["CategoryTwo"]));
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return dictionary;
        }

        /// <summary>
        /// 查询药品
        /// </summary>
        /// <param name="Name">药品名称</param>
        /// <param name="CategoryTwo">二级分类</param>
        /// <param name="CategoryThree">三级分类</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<Medicine> GetMedicinesName(String Name, String CategoryTwo, String CategoryThree)
        {
            List<Medicine> cplist = new List<Medicine>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select   Name from Medicine where  rownum<50");
                sb.AppendFormat(" ( Name like  '%{0}%'", Name);
                sb.AppendFormat(" or Pinyin like  '%{0}%' )", Name);
                if (CategoryTwo != null && CategoryTwo != "全部")
                    sb.AppendFormat(" and CategoryTwo =  '{0}'", CategoryTwo);
                if (CategoryThree != null && CategoryThree != "")
                    sb.AppendFormat(" and CategoryThree =  '{0}'", CategoryThree);
                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    Medicine cp = new Medicine();
                    cp.Name = ConvertMy.ToString(dr["Name"]);
                    cplist.Add(cp);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return cplist;
        }

    
    }
}
