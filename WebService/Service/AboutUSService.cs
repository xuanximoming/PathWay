using DrectSoft.Tool;
using System;
using System.Data;
using System.ServiceModel;

namespace Yidansoft.Service
{

    public partial class YidanEHRDataService
    {

        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public AboutUS DisAboutUS()
        {
            AboutUS about = new AboutUS();

            try
            {


                DataTable table = SqlHelper.ExecuteDataTable("usp_AboutUS");

                if (table != null)            //很重要,是否存在
                {
                    foreach (DataRow item in table.Rows)
                    {
                        about.Company = ConvertMy.ToString(item["Company"]);
                        about.Edit = ConvertMy.ToString(item["Edit"]);
                        about.Names = ConvertMy.ToString(item["Names"]);
                        about.Times = ConvertMy.ToString(item["Times"]);
                        about.Warning = ConvertMy.ToString(item["Warning"]);

                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                ThrowException(ex);
            }
            //finally
            //{
            //    myConnection.Close();
            //}

            return about;
            //}
        }
    }
}