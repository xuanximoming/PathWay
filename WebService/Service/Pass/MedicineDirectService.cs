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
        /// 查询药品说明
        /// </summary>
        /// <param name="Name">药品名称</param>
        /// <param name="CategoryTwo">二级分类</param>
        /// <param name="CategoryThree">三级分类</param>
        /// <returns></returns>
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<MedicineDirect> GetMedicinesDirects(String DirectTitleORPinYin)
        {
            List<MedicineDirect> cplist = new List<MedicineDirect>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select * from MedicineDirect where  ");
                sb.AppendFormat(" DirectTitle2 like  '%{0}%'", DirectTitleORPinYin);
                sb.AppendFormat(" or PinYin like  '%{0}%'", DirectTitleORPinYin);
                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    MedicineDirect cp = new MedicineDirect();
                    cp.DirectTitle2 = ConvertMy.ToString(dr["DirectTitle2"]);
                    cp.ID = ConvertMy.ToString(dr["ID"]);
                    // cp.DirectContent = ConvertMy.ToString(dr["DirectContent"]);
                    cp.Doseform = ConvertMy.ToString(dr["Doseform"]);
                    cp.DirectTitle = ConvertMy.ToString(dr["DirectTitle"]);
                    // cp.DirectContent = ConvertMy.ToString(dr["DirectContent"]);
                    //cp.Company = ConvertMy.ToString(dr["DirectTitle"]);
                    cplist.Add(cp);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return cplist;
        }
        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public List<MedicineDirect> GetMedicinesDirectsTitle(String inputtxt)
        {
            List<MedicineDirect> cplist = new List<MedicineDirect>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select distinct     DirectTitle2  from MedicineDirect where  rownum<50 and");
                sb.AppendFormat("  DirectTitle2 like  '%{0}%' ", inputtxt);
                sb.AppendFormat(" or PinYin like  '%{0}%' ", inputtxt);
                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    MedicineDirect cp = new MedicineDirect();
                    cp.DirectTitle2 = ConvertMy.ToString(dr["DirectTitle2"]);
                    cplist.Add(cp);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            return cplist;
        }

        [FaultContract(typeof(LoginException))]
        [OperationContract]
        public MedicineDirect GetMedicinesDirect(String ID)
        {
            List<MedicineDirect> cplist = new List<MedicineDirect>();
            try
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(@"select * from MedicineDirect where  ");
                sb.AppendFormat(" ID ='{0}'", ID);
                DataTable dt = SqlHelper.ExecuteDataTable(sb.ToString());
                foreach (DataRow dr in dt.Rows)
                {
                    MedicineDirect cp = new MedicineDirect();
                    cp.DirectTitle2 = ConvertMy.ToString(dr["DirectTitle2"]);
                    cp.ID = ConvertMy.ToString(dr["ID"]);
                    // cp.DirectContent = ConvertMy.ToString(dr["DirectContent"]);
                    cp.Doseform = ConvertMy.ToString(dr["Doseform"]);
                    cp.DirectTitle = ConvertMy.ToString(dr["DirectTitle"]);
                    cp.DirectContent = ConvertMy.ToString(dr["DirectContent"]);
                    //cp.Company = ConvertMy.ToString(dr["DirectTitle"]);
                    cplist.Add(cp);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            if (cplist[0] != null)
                return cplist[0];
            return null;
        }
    }
}
