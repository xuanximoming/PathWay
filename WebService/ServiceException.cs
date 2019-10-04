using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service
{
    #region  userlogin exception

    [DataContract]
    public enum ErrorLevel
    {
        [EnumMember]
        UserValid,
        [EnumMember]
        PasswordValid,
        [EnumMember]
        SqlException,
        [EnumMember]
        None
    }

    /// <summary>
    /// 登陆异常
    /// </summary>
    [DataContract]
    public class LoginException
    {
        [DataMember]
        public ErrorLevel ErrorLevel { get; set; }

        [DataMember]
        public string ErroMsg { get; set; }

        [DataMember]
        public string CreateUser { get; set; }

        [DataMember]
        public string ModelName { get; set; }

        [DataMember]
        public string MacAddress { get; set; }

        [DataMember]
        public string Ip { get; set; }  

        [DataMember]
        public string HostName { get; set; }

        //public LoginException(ErrorLevel level,string msg)
        //{
        //    ErrorLevel = level;
        //    ErroMsg = msg;

        //}
    }
    #endregion
}