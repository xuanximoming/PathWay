
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
using DrectSoft.Tool;
namespace Yidansoft.Service.Entity
{
    [DataContract]
    public class Medicine
    {
        #region 变量
        String _Name;
        String _Specification;
        String _ApplyTo;
        String _ReferenceUsage;
        String _Meno;
        String _CategoryThree;
        String _CategoryTwo;
        String _Pinyin;

     
        #endregion
        #region 属性
        [DataMember()]
        public String Pinyin
        {
            get { return _Pinyin; }
            set { _Pinyin = value; }
        }
        /// <summary>
        /// 药品名称
        /// </summary>
        [DataMember()]
        public String Name
        {
            get { return _Name; }
            set { _Name = value; }
        }
        /// <summary>
        /// 药品规格
        /// </summary>
        [DataMember()]
        public String Specification
        {
            get { return _Specification; }
            set { _Specification = value; }
        }
        /// <summary>
        /// 适用症
        /// </summary>
        [DataMember()]
        public String ApplyTo
        {
            get { return _ApplyTo; }
            set { _ApplyTo = value; }
        }
        /// <summary>
        /// 用法
        /// </summary>
        [DataMember()]
        public String ReferenceUsage
        {
            get { return _ReferenceUsage; }
            set { _ReferenceUsage = value; }
        }
        /// <summary>
        /// 备注
        /// </summary>
        [DataMember()]
        public String Meno
        {
            get { return _Meno; }
            set { _Meno = value; }
        }
        /// <summary>
        /// 三级分类
        /// </summary>
        [DataMember()]
        public String CategoryThree
        {
            get { return _CategoryThree; }
            set { _CategoryThree = value; }
        }
        /// <summary>
        /// 二级分类
        /// </summary>
        [DataMember()]
        public String CategoryTwo
        {
            get { return _CategoryTwo; }
            set { _CategoryTwo = value; }
        }
        #endregion

    }
}