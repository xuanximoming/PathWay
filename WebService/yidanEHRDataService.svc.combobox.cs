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


namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {


        public List<CP_DepartmentList> DepartmentLists
        {

            get
            {
                if (m_DepartmentLists == null)
                {
                    m_DepartmentLists = new List<CP_DepartmentList>();
                    GetDepartments();
                }
                return m_DepartmentLists;
            }
        }
        private List<CP_DepartmentList> m_DepartmentLists;

        private void GetDepartments()
        {
            m_DepartmentLists = GetDepartment4AutoComboBox();
        }



        /// <summary>
        /// 全部科室,autoCompleteBox
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_DepartmentList> GetDepartmentByFilter(string strFilter)
        {
            return DepartmentLists.Where(cp => cp.QueryName.Contains(strFilter) || cp.QueryName.StartsWith(strFilter)).ToList();

        }

        /// <summary>
        /// 全部科室,autoCompleteBox
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<CP_DepartmentList> GetDepartment4AutoComboBox()
        {
            List<CP_DepartmentList> listInfo = new List<CP_DepartmentList>();

            try
            {

                DataTable dataTable = SqlHelper.ExecuteDataTable("usp_CP_DeptmentList");

                foreach (DataRow row in dataTable.Rows)
                {
                    CP_DepartmentList depListInfo = new CP_DepartmentList(row["ID"].ToString(), row["Name"].ToString(), row["QueryName"].ToString());
                    listInfo.Add(depListInfo);
                }
            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            
            return listInfo;

        }   // zm 8.24 Oracle
    }
}