using DrectSoft.Tool;
using System;
using System.Data;
using System.Runtime.Serialization;
using YidanSoft.Core;

namespace Yidansoft.Service
{
    public partial class CP_Employee
    {

        /// <summary>
        /// 科室
        /// 修改：fqw 时间：2010-03-21  mark：fqwFix
        /// </summary>
        [DataMemberAttribute()]
        public CP_Department Department
        {
            get
            {
                if (_department == null)
                {
                    try
                    {


                        DataTable dt = DataAccessFactory.DefaultDataAccess.ExecuteDataTable(String.Format("select * from Department where ID='{0}'", this.Ksdm));
                        if (dt.Rows.Count > 0)
                        {
                            _department = new CP_Department();
                            _department.Ksdm = ConvertMy.ToString(dt.Rows[0]["ID"]);
                            _department.Name = ConvertMy.ToString(dt.Rows[0]["Name"]);
                            _department.Py = ConvertMy.ToString(dt.Rows[0]["Py"]);
                            _department.Wb = ConvertMy.ToString(dt.Rows[0]["Wb"]);
                            _department.Yydm = ConvertMy.ToString(dt.Rows[0]["HosNo"]);
                            _department.Yjksdm = ConvertMy.ToString(dt.Rows[0]["ADept"]);
                            _department.Ejksdm = ConvertMy.ToString(dt.Rows[0]["BDept"]);
                            _department.Kslb = ConvertMy.ToShort(dt.Rows[0]["Sort"]);
                            _department.Ksbz = ConvertMy.ToShort(dt.Rows[0]["Mark"]);
                            _department.Zryss = ConvertMy.ToInt32(dt.Rows[0]["TotalChief"]);
                            _department.Zyyss = ConvertMy.ToInt32(dt.Rows[0]["TotalResident"]);
                            _department.Zzyss = ConvertMy.ToInt32(dt.Rows[0]["TotalAttend"]);
                            _department.Hss = ConvertMy.ToInt32(dt.Rows[0]["TotalNurse"]);
                            _department.Cws = ConvertMy.ToInt32(dt.Rows[0]["TotalBed"]);
                            _department.Hdcws = ConvertMy.ToInt32(dt.Rows[0]["TotalExtra"]);
                            _department.Yxjl = ConvertMy.ToShort(dt.Rows[0]["Valid"]);
                            _department.Memo = ConvertMy.ToString(dt.Rows[0]["Memo"]);


                        }
                        #region 微构前代码
                        //using (YidanEHREntities entites = new YidanEHREntities())
                        //{
                        //    var dpts = entites.CP_Department.Where(dept => dept.Ksdm.Equals(this.Ksdm));
                        //    _department = dpts.FirstOrDefault();

                        //}
                        #endregion

                    }
                    catch
                    {
                        _department = new CP_Department();
                    }
                }
                return _department;
            }
            set { _department = value; }
        }
        private CP_Department _department;

        /// <summary>
        /// 病区
        /// 修改：fqw 时间：2010-03-21  mark：fqwFix
        /// </summary>
        [DataMemberAttribute()]
        public CP_Ward Ward
        {
            get
            {
                if (_ward == null)
                {
                    try
                    {
                        DataTable dt = YidanEHRDataService.SqlHelper.ExecuteDataTable(String.Format("select * from Ward where ID='{0}'", this.Bqdm));
                        if (dt.Rows.Count > 0)
                        {
                            _ward = new CP_Ward();
                            _ward.Bqdm = ConvertMy.ToString(dt.Rows[0]["ID"]);
                            _ward.Name = ConvertMy.ToString(dt.Rows[0]["Name"]);
                            _ward.Py = ConvertMy.ToString(dt.Rows[0]["Py"]);
                            _ward.Wb = ConvertMy.ToString(dt.Rows[0]["Wb"]);
                            _ward.Yydm = ConvertMy.ToString(dt.Rows[0]["HosNo"]);
                            _ward.Cws = ConvertMy.ToInt32(dt.Rows[0]["TotalBed"]);
                            _ward.Bqbz = ConvertMy.ToShort(dt.Rows[0]["Mark"]);
                            _ward.Yxjl = ConvertMy.ToShort(dt.Rows[0]["Valid"]);
                            _ward.Memo = ConvertMy.ToString(dt.Rows[0]["Memo"]);



                        }
                        #region 微构前代码
                        //using (YidanEHREntities entites = new YidanEHREntities())
                        //{
                        //    var wd = entites.CP_Ward.Where(ward => ward.Bqdm.Equals(this.Bqdm));
                        //    _ward = wd.FirstOrDefault();

                        //}
                        #endregion

                    }
                    catch
                    {
                        _ward = new CP_Ward();
                    }
                }
                return _ward;
            }
            set { _ward = value; }
        }

        private CP_Ward _ward;







    }
}
