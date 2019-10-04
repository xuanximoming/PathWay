using System;
using System.Collections;
using System.Configuration;
using System.Management;
using System.Net;
using System.Text;
using System.Web;

namespace DrectSoftEHRApplication.Web
{
    public partial class DrecSoftEHRDefault : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            Hashtable services = new Hashtable();
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                if (key.EndsWith("svc", StringComparison.CurrentCultureIgnoreCase))
                {
                    services.Add(key, ConfigurationManager.AppSettings[key]);
                }
                else
                {
                    services.Add(key, ConfigurationManager.AppSettings[key]);
                }
            }

            StringBuilder sb = new StringBuilder();

            foreach (string key in services.Keys)
            {
                sb.Append(string.Format(",{0}={1}", key, services[key]));
            }
            sb.Append(string.Format(",{0}={1}", "IpAddress", GetLocalIP()));
            sb.Append(string.Format(",{0}={1}", "MacAddress", GetLocalMacAddr()));

            if (HttpContext.Current.Request.QueryString.Count > 0)
            {
                string username = HttpContext.Current.Request.QueryString["username"];
                string password = HttpContext.Current.Request.QueryString["password"];


                sb.Append(string.Format(",{0}={1}", "SkipLogin", "true"));
                sb.Append(string.Format(",{0}={1}", "Username", username));
                sb.Append(string.Format(",{0}={1}", "Password", password));

                string url = HttpContext.Current.Request.QueryString["StartPage"];
                string patid = HttpContext.Current.Request.QueryString["patid"];
                string StartPagePathExecute = HttpContext.Current.Request.QueryString["StartPagePathExecute"];
                if (!String.IsNullOrEmpty(url))
                    sb.Append(string.Format(",{0}={1}", "StartPage", url));

                if (!String.IsNullOrEmpty(patid))
                    sb.Append(string.Format(",{0}={1}", "Patid", patid));
                if (!String.IsNullOrEmpty(StartPagePathExecute))
                    sb.Append(string.Format(",{0}={1}", "StartPagePathExecute", StartPagePathExecute));


            }



            this.SLInitParams.Text = string.Format("<param name=\"InitParams\" value=\"{0}\" />", sb.ToString());




        }



        /// <summary>
        /// 获取本机的IP
        /// </summary>
        /// <returns></returns>
        public string GetLocalIP()
        {
            string strHostName = Dns.GetHostName(); //得到本机的主机名
            IPHostEntry ipEntry = Dns.GetHostByName(strHostName); //取得本机IP
            string strAddr = ipEntry.AddressList[0].ToString();
            return strAddr;
        }
        /// <summary>
        /// 获取本机的MAC
        /// </summary>
        /// <returns></returns>
        public string GetLocalMacAddr()
        {
            string strMac = string.Empty;
            try
            {
                ManagementObjectSearcher query = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection queryCollection = query.Get();
                foreach (ManagementObject mo in queryCollection)
                {
                    if (mo["IPEnabled"].ToString() == "True")
                        strMac = mo["MacAddress"].ToString();
                }
            }
            catch
            {
                return string.Empty; ;
            }
            return strMac;
        }
    }
}