using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.Management;
using System.Data; 
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Microsoft.VisualBasic;
using System.Configuration;
using YidanSoft.Core;
using Yidansoft.Service;

namespace Yidansoft.Service
{
    public class Key
    {

       

        /// <summary>
        /// 获取硬盘ID
        /// add bwj 20130411
        /// </summary>
        /// <returns></returns>
        public static string getHDid()
        {
            try
            {
                string result = "";
                string strHDid="";
                ManagementClass cimobject1 = new ManagementClass("Win32_DiskDrive");
                ManagementObjectCollection moc1 = cimobject1.GetInstances();
                foreach (ManagementObject mo in moc1)
                {
                    strHDid = (string)mo.Properties["Model"].Value;

                    break;
                }
                if (strHDid != "")
                {
                    result = strHDid;
                }
                return result;
            }
            catch (Exception ce)
            {
                throw ce;
            }

        }
        /// <summary>
        /// 取主板ID
        /// </summary>
        /// <returns></returns>
        public static string getBoardID()
        {
            try
            {
                string result = "";
                SelectQuery Query=new SelectQuery("SELECT * FROM Win32_BaseBoard");
　　            ManagementObjectSearcher driveID=new ManagementObjectSearcher(Query);
　　            ManagementObjectCollection.ManagementObjectEnumerator data=driveID.Get().GetEnumerator();
　　            data.MoveNext();
　　            ManagementBaseObject board=data.Current;
                string boardid=board.GetPropertyValue("SerialNumber").ToString();
                if (boardid != string.Empty)
                {
                    result=boardid;
                }
                return result;

            }
            catch (Exception ce)
            {
                throw ce;
            }
        }

        //获取Mac    
        private static string getMac()
        {
            try
            {

                string MoAddress = "";
                ManagementClass mc = new ManagementClass("Win32_NetworkAdapterConfiguration");
                ManagementObjectCollection moc = mc.GetInstances();
                foreach (ManagementObject mo in moc)
                {
                    if ((bool)mo["IPEnabled"] == true)
                    {
                        MoAddress = mo["MacAddress"].ToString();
                    }
                    mo.Dispose();
                }
                return MoAddress;
            }
            catch (Exception ex)
            {

                return "";
            }
        }

        /// <summary>
        /// 获取cpu的ID
        /// </summary>
        /// <returns></returns>
        private static string GetCPUId()
        {
            try
            {
                ManagementClass mc = new ManagementClass("Win32_Processor");
                ManagementObjectCollection moc = mc.GetInstances();
                String strCpuID = null;
                foreach (ManagementObject mo in moc)
                {
                    strCpuID = mo.Properties["ProcessorId"].Value.ToString();
                    break;
                }
                //bwj add 2013-4-11
                if (strCpuID == string.Empty)
                {
                    strCpuID = getBoardID();
                }
                return strCpuID;
            }
            catch (Exception ex)
            {
                return "";
               // throw ex;
            }


        }

        /// <summary>
        /// 获取第一块硬盘的ID
        /// </summary>
        /// <returns></returns>
        private static string GetHandWardId()
        {
            try
            {
                ManagementObjectSearcher searcher = new ManagementObjectSearcher("SELECT * FROM Win32_PhysicalMedia");
                String strHardDiskID = null;
                foreach (ManagementObject mo in searcher.Get())
                {
                    strHardDiskID = mo["SerialNumber"].ToString().Trim();
                    break;
                }

                //add bwj 2013-04-11
                if (strHardDiskID == string.Empty)
                {
                    strHardDiskID = getHDid();
                }

                return strHardDiskID;
            }
            catch (Exception ex)
            {
                return "";
              //  throw ex;
            }
        }

        /// <summary>
        /// 判断数据库是否存在硬件信息
        /// </summary>
        /// <returns></returns>
        public static bool ISRightPCHandInfo()
        {
            try
            {
                string PCStr = Key.GetMD5Str();
                string sql = string.Format(@"select * from yidanmac where pcmac='{0}'", PCStr);
                DataTable dt = YidanEHRDataService.SqlHelper.ExecuteDataTable(sql, CommandType.Text);
                if (dt == null || dt.Rows == null || dt.Rows.Count <= 0)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }


        /// <summary>
        /// 判断数据库是否存在硬件信息
        /// </summary>
        /// <returns></returns>
        public static bool ISRegTime()
        {
            try
            {
                string sqlDate = "select * from yidanaction";
                DataTable dt = YidanEHRDataService.SqlHelper.ExecuteDataTable(sqlDate, CommandType.Text);
                if (dt == null || dt.Rows == null || dt.Rows.Count == 0)
                {
                    return false;
                }
                else
                {
                    SymmetricMethod symmetricMethod = new SymmetricMethod();
                    string DateTimeStr = symmetricMethod.Decrypto(dt.Rows[0][0].ToString());
                    DateTime dtReg = Convert.ToDateTime(DateTimeStr);
                    //如果数据库中有有效期数据，且有效期已经过期则直接将有效期信息删除，防止用户将系统时间修改后再次登录系统
                    if (dtReg < DateTime.Now)
                    {
                        sqlDate = "delete from yidanaction";
                        YidanEHRDataService.SqlHelper.ExecuteNoneQuery(sqlDate);
                        return false;
                    }
                    else
                    {
                        return true;
                    }
                }
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static string GetPWD()
        {
            try
            {
                string[] pwdList = new string[12];
                pwdList[0] = "1995";
                pwdList[1] = "4547";
                pwdList[2] = "1170";
                pwdList[3] = "2230";
                pwdList[4] = "2230";
                pwdList[5] = "3456";
                pwdList[6] = "1484";
                pwdList[7] = "1658";
                pwdList[8] = "2654";
                pwdList[9] = "1147";
                pwdList[10] = "1203";
                pwdList[11] = "3365";
                //获取服务器时间
                DateTime dateTime = new DateTime();

                dateTime = GetSysDateTimeSQLService();

                return pwdList[dateTime.Month - 1];
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static DateTime GetSysDateTime()
        {
            try
            {

                DataTable dt = YidanEHRDataService.SqlHelper.ExecuteDataTable("select sysdate from dual", CommandType.Text);
                if (dt == null)
                {
                    new Exception("未取到系统时间");
                }
                DateTime dateTime = Convert.ToDateTime(dt.Rows[0][0]);
                return dateTime;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public static DateTime GetSysDateTimeSQLService()
        {
            try
            {

                DataTable dt = YidanEHRDataService.SqlHelper.ExecuteDataTable("select convert(varchar(19),getdate(),20)", CommandType.Text);
                if (dt == null)
                {
                    new Exception("未取到系统时间");
                }
                DateTime dateTime = Convert.ToDateTime(dt.Rows[0][0]);
                return dateTime;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 获取进过加密的硬件信息
        /// </summary>
        /// <returns></returns>
        public static string GetMD5Str()
        {
            try
            {
                StringBuilder strb = new StringBuilder();
                string macstr= getMac();
                string cpuID=  GetCPUId();
                string wardId=  GetHandWardId();
         //  MessageBox.Show("macstr:" + macstr + "cpuID:" + cpuID + "wardId:" + wardId);
               strb.Append(macstr);
               strb.Append(cpuID);
               //strb.Append(wardId);
               string sql = string.Format(@"insert into yidanmac (Pcmac,pcregdate) values('{0}',getdate())", "Web:macstr:" + macstr + "cpuID:" + cpuID);
               YidanEHRDataService.SqlHelper.ExecuteNoneQuery(sql);

                return GetMD5(strb.ToString());
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        /// <summary>
        /// 取MD5码
        /// </summary>
        /// <param name="_value"></param>
        /// <returns></returns>
        private static string GetMD5(string _value)
        {
            try
            {
                string result = "";
                if (_value == string.Empty)
                {
                    return result;
                }
                byte[] source = System.Text.Encoding.ASCII.GetBytes(_value);
                if (source.Length > 1)
                {
                    byte tmp = source[0];
                    source[0] = source[source.Length - 1];
                    source[source.Length - 1] = tmp;
                }
                MD5 aMD5 = new MD5CryptoServiceProvider();
                byte[] obj = aMD5.ComputeHash(source);
                for (int i = 0; i < obj.Length; i++)
                {
                    if (i != 0 && i % 5 == 0)
                    {
                        result += "b";
                    }
                    else if (i != 0 && i % 3 == 0)
                    {
                        result += "u";
                    }
                    result = result + Conversion.Hex(obj[i]).ToString();
                }

                return result.ToUpper();
            }
            catch (Exception ce)
            {
                throw ce;
            }
        }


    }

}
