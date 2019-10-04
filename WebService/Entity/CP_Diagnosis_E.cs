using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace Yidansoft.Service.Entity
{
    /// <summary>
    /// 诊断库(医院诊断库与路径诊断库用相同实体)
    /// </summary>
    public partial class CP_Diagnosis_E
    {

        #region Model
        /// <summary>
        /// 诊断标识(zddm + zlbm，由于使用计算列或触发器的方式，都不便于在前台程序维护数据，所以该列的值由前台程序生成)
        /// </summary>
        private string _zdbs;

        /// <summary>
        /// 诊断代码(ICD10)
        /// </summary>
        private string _zddm;

        /// <summary>
        /// 映射代码
        /// </summary>
        private string _ysdm;

        /// <summary>
        /// (对应的)标准代码(用户自定义代码有标准代码时，填写此字段)
        /// </summary>
        private string _bzdm;

        /// <summary>
        /// 疾病名称
        /// </summary>
        private string _name;

        /// <summary>
        /// 拼音
        /// </summary>
        private string _py;

        /// <summary>
        /// 五笔
        /// </summary>
        private string _wb;

        /// <summary>
        /// 肿瘤编码(YY_ZLK.zldm)
        /// </summary>
        private string _zldm;

        /// <summary>
        /// 所属统计分类(CP_DiseaseCFG.Bzdm, Bzlb = 700)
        /// </summary>
        private string _tjm;

        /// <summary>
        /// 内部分类(医院内部自定义分类,CP_DiseaseCFG.Bzdm, Bzlb = 702)
        /// </summary>
        private string _nbfl;

        /// <summary>
        /// 病种类别(单病种代码, CP_DiseaseCFG.Bzdm, Bzlb = 701)
        /// </summary>
        private string _bzlb;

        /// <summary>
        /// (疾病)其他类别(CP_DataCategory.Mxbh Lbbh = 9)
        /// </summary>
        private string _qtlb;

        /// <summary>
        /// 有效记录(CP_DataCategory.Mxbh Lbbh = 0)
        /// </summary>
        private string _yxjl;

        /// <summary>
        /// 备注
        /// </summary>
        private string _memo;


        /// <summary>
        /// 诊断标识(zddm + zlbm，由于使用计算列或触发器的方式，都不便于在前台程序维护数据，所以该列的值由前台程序生成)
        /// </summary>
        [DataMember()]
        public string Zdbs
        {
            set { _zdbs = value; }
            get { return _zdbs; }
        }

        /// <summary>
        /// 诊断代码(ICD10)
        /// </summary>
        [DataMember()]
        public string Zddm
        {
            set { _zddm = value; }
            get { return _zddm; }
        }

        /// <summary>
        /// 映射代码
        /// </summary>
        [DataMember()]
        public string Ysdm
        {
            set { _ysdm = value; }
            get { return _ysdm; }
        }

        /// <summary>
        /// (对应的)标准代码(用户自定义代码有标准代码时，填写此字段)
        /// </summary>
        [DataMember()]
        public string Bzdm
        {
            set { _bzdm = value; }
            get { return _bzdm; }
        }

        /// <summary>
        /// 疾病名称
        /// </summary>
        [DataMember()]
        public string Name
        {
            set { _name = value; }
            get { return _name+"【"+_zdbs+"】"; }
        }

        /// <summary>
        /// 拼音
        /// </summary>
        [DataMember()]
        public string Py
        {
            set { _py = value; }
            get { return _py; }
        }

        /// <summary>
        /// 五笔
        /// </summary>
        [DataMember()]
        public string Wb
        {
            set { _wb = value; }
            get { return _wb; }
        }

        /// <summary>
        /// 肿瘤编码(YY_ZLK.zldm)
        /// </summary>
        [DataMember()]
        public string Zldm
        {
            set { _zldm = value; }
            get { return _zldm; }
        }

        /// <summary>
        /// 所属统计分类(CP_DiseaseCFG.Bzdm, Bzlb = 700)
        /// </summary>
        [DataMember()]
        public string Tjm
        {
            set { _tjm = value; }
            get { return _tjm; }
        }

        /// <summary>
        /// 内部分类(医院内部自定义分类,CP_DiseaseCFG.Bzdm, Bzlb = 702)
        /// </summary>
        [DataMember()]
        public string Nbfl
        {
            set { _nbfl = value; }
            get { return _nbfl; }
        }

        /// <summary>
        /// 病种类别(单病种代码, CP_DiseaseCFG.Bzdm, Bzlb = 701)
        /// </summary>
        [DataMember()]
        public string Bzlb
        {
            set { _bzlb = value; }
            get { return _bzlb; }
        }

        /// <summary>
        /// (疾病)其他类别(CP_DataCategory.Mxbh Lbbh = 9)
        /// </summary>
        [DataMember()]
        public string Qtlb
        {
            set { _qtlb = value; }
            get { return _qtlb; }
        }

        /// <summary>
        /// 有效记录(CP_DataCategory.Mxbh Lbbh = 0)
        /// </summary>
        [DataMember()]
        public string Yxjl
        {
            set { _yxjl = value; }
            get { return _yxjl; }
        }

        /// <summary>
        /// 备注
        /// </summary>
        [DataMember()]
        public string Memo
        {
            set { _memo = value; }
            get { return _memo; }
        }



        #endregion Model
    }
}