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
namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 药品说明书
    /// </summary>
    [DataContract]
    public class MedicineDirect
    {
        #region 变量
        String _Doseform;
        String _DirectTitle;
        String _DirectTitle2;
        String _Company;
        String _DirectContent;
        String _PinYin;
        String _ID;

     

        #endregion
          [DataMember]
        public String ID
        {
            get { return _ID; }
            set { _ID = value; }
        }
        /// <summary>
        /// 剂型
        /// </summary>
      [DataMember]
        public String Doseform
        {
            get { return _Doseform; }
            set { _Doseform = value; }
        }
        /// <summary>
        /// 公司名称
        /// </summary>
        [DataMember]
        public String Company
        {
            get
            {
                if (DirectTitle == null)
                    return null;
                String[] arr = DirectTitle.Split('-');
                if (arr.Length > 1 && arr[1] != null && arr[1] != "")
                {
                    _Company = arr[1];
                    return _Company;
                }
                return null;
            }
            set { _Company = value; }
        }
        #region 属性
        /// <summary>
        /// 说明书标题
        /// </summary>
        [DataMember]
        public String DirectTitle
        {
            get { return _DirectTitle; }
            set { _DirectTitle = value; }
        }
        /// <summary>
        /// 说明书内容
        /// </summary>
        [DataMember]
        public String DirectContent
        {
            get { return _DirectContent; }
            set { _DirectContent = value; }
        }
        /// <summary>
        /// 说明标题拼音
        /// </summary>
        [DataMember]
        public String PinYin
        {
            get { return _PinYin; }
            set { _PinYin = value; }
        }
        /// <summary>
        /// 说明标题
        /// </summary>
        [DataMember]
        public String DirectTitle2
        {
            get { return _DirectTitle2; }
            set { _DirectTitle2 = value; }
        }
        #endregion
    }
    //    ProductID	decimal(18, 0)	Checked
    //Doseform	nvarchar(100)	Checked
    //DirectTitle	nvarchar(255)	Checked
    //DirectContent	ntext	Checked
    //CreateDate	datetime	Checked
    //CreatePerson	nvarchar(20)	Checked
    //ChangeDate	datetime	Checked
    //ChangeMan	nvarchar(20)	Checked
    //DirectTitle2	nvarchar(255)	Checked
    //PinYin	nvarchar(150)	Checked
    //PageNum	nvarchar(255)	Checked
    //ID1	nvarchar(255)	Checked
    //URL	nvarchar(255)	Checked
    //IsError	char(10)	Checked
    //Type	nvarchar(50)	Checked
}