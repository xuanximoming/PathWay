using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using YidanSoft.Tool;
using System.Data;

namespace Yidansoft.Service
{
    public partial class CP_InPatient
    {
        /// <summary>
        /// 修改：fqw 时间：2010-03-18  mark：fqwFix
        /// </summary>
        [DataMember]
        public string BrztName
        {

            get
            {
                if (string.IsNullOrEmpty(_brztName))
                {
                    try
                    {
                       // _brztName = ConvertMy.ToString(Yidansoft.Service.YidanEHRDataService.SqlHelper.ExecuteScalar(YidanEHRDataService.m_ConnectionString, CommandType.Text, string.Format("select Name From CP_DataCategoryDetail where  Mxbh={0} and Lbbh={1}", Brzt, "15"), null));


                        DataTable dt = YidanEHRDataService.SqlHelper.ExecuteDataTable( string.Format("select Name From CP_DataCategoryDetail where  Mxbh={0} and Lbbh={1}", Brzt, "15"));
                        if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0] != null)
                        {
                            _brztName = dt.Rows[0][0].ToString().Trim();
                        }
                    }
                    catch
                    {
                        _brztName = string.Empty;
                    }
                    #region 微构前代码
                    //using (YidanEHREntities entities = new YidanEHREntities())
                    //{
                    //    try
                    //    {
                    //       _brztName = entities.CP_DataCategoryDetail.Where(br => br.Mxbh.Equals(Brzt) && br.Lbbh.Equals("15")).First().Name;

                    //    }
                    //    catch
                    //    {
                    //        _brztName = string.Empty;
                    //    }
                    //}
                    #endregion
                }
                return _brztName;
            }
            set { _brztName = value; }
        }


        private string _brztName;

        /// <summary>
        /// 修改：fqw 时间：2010-03-18  mark：fqwFix
        /// </summary>
        [DataMember]
        public string MzysName
        {

            get
            {
                if (string.IsNullOrEmpty(_mzysName))
                {
                    CP_Employee emp = new YidanEHRDataService().GetEmployeeInfo(Mzys);
                    if (emp != null && emp.Name != null)
                        _mzysName = emp.Name;
                    else
                        _mzysName = string.Empty;

                    #region 修改前代码
                    //using (YidanEHREntities entities = new YidanEHREntities())
                    //{
                    //    try
                    //    {
                    //        _mzysName = entities.CP_Employee.Where(ep => ep.Zgdm.Equals(Mzys)).First().Name;
                    //    }
                    //    catch
                    //    {
                    //        _mzysName = string.Empty;
                    //    }

                    //}
                    #endregion

                }
                return _mzysName;

            }
            set { _mzysName = value; }
        }
        private string _mzysName;











    }
}