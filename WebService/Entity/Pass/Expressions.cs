using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using Yidansoft.Service.Entity.Pass;
using System.ServiceModel;

//using System;
//using System.Net;
//using System.Windows;
//using System.Windows.Controls;
//using System.Windows.Documents;
//using System.Windows.Ink;
//using System.Windows.Input;
//using System.Windows.Media;
//using System.Windows.Media.Animation;
//using System.Windows.Shapes;

//using Expressions;
//using System.Collections.Generic;
//using Telerik.Windows.Controls;

namespace Yidansoft.Service.Entity
{
    #region 公式实体
    /// <summary>
    /// 公式实体
    /// </summary>
    [DataContract]
    public class Expressions
    {  
        /// <summary>
        /// 公式参数集
        /// </summary>
        List<ParameterProperty> m_ExpressionsParameter = new List<ParameterProperty>();
        //List<ObjectCell>              m_ObjectCollection=new List<ObjectCell>();
        
        /// <summary>
        /// 公式类型
        /// </summary>
        [DataMember]
        public String ExpressionsGroupType { get; set; }
        
        /// <summary>
        /// 公式名称
        /// </summary>
        [DataMember]
        public String ExpressionsName { get; set; }
        
        /// <summary>
        /// 转换后台处理公式
        /// </summary>
        [DataMember]
        public String ExpressionsProcess { get; set; }
        
        /// <summary>
        /// 计算结果单位
        /// </summary>
        [DataMember]
        public String ExpressionsReusltUnit { get; set; }
        
        /// <summary>
        /// 公式描述
        /// </summary>
        [DataMember]
        public String ExpressionsDescribe { get; set; }

        /// <summary>
        /// 公式参数集
        /// </summary>
        [DataMember]
        public List<ParameterProperty> ExpressionsParameter
        { 
            get
            {
                return m_ExpressionsParameter; 
            }
            set
            {
               m_ExpressionsParameter=value; 
            }
        }

        ///// <summary>
        ///// 公式参数控件集
        ///// </summary>
        //public List<ObjectCell> ObjectCollection
        //{
        //    get
        //    {
        //        return m_ObjectCollection;
        //    }
        //    set
        //    {
        //        m_ObjectCollection = value;
        //    }
        //}


        /// <summary>
        /// 添加公式参数
        /// </summary>
        /// <param name="modelParameterProperty">单个参数控件属性实体</param>
        public void AddExpressionsParameter(ParameterProperty modelParameterProperty)
        {  
            if(modelParameterProperty!=null)
            {
                //添加公式参数
                m_ExpressionsParameter.Add( modelParameterProperty);
            }
        }


        ///// <summary>
        ///// 添加公式参数控件
        ///// </summary>
        ///// <param name="modelParameterProperty">单个参数控件属性实体</param>
        //public void AddObjectCollection(ParameterProperty modelParameterProperty)
        //{
        //    if (modelParameterProperty != null)
        //    {
        //        ObjectCell objectCell = new ObjectCell();
        //        //设置参数控件属性
        //        objectCell.SetProperty(modelParameterProperty);
        //        //添加参数控件
        //        m_ObjectCollection.Add(objectCell);
        //    }
        //}
    }
    #endregion
        
}
