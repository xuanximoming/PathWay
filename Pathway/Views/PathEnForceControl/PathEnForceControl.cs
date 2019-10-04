using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

using YidanEHRApplication.DataService;
using System.Collections.Generic;
using YidanSoft.Tool;

namespace YidanEHRApplication.Views.PathEnForceControl
{
    /// <summary>
    /// 路径执行控制器
    /// </summary>
    public class PathEnForceControl
    {
        /// <summary>
        /// 当前病人 
        /// </summary>
        public CP_InpatinetList CP_InpatinetListCurrent
        {
            get;
            set;
        }
        /// <summary>
        /// 当前医嘱列表
        /// </summary>
        public CP_DoctorOrders CP_DoctorOrders
        {
            get;
            set;
        }

        /// <summary>
        /// 设置或获取一个值该值指示流程图空间是否显示
        /// </summary>
        private Boolean IsWorkFlowContainShow = false;
    }

    /// <summary>
    /// 医嘱列表类
    /// </summary>
    public class CP_DoctorOrders : List<CP_DoctorOrder>
    {
        /// <summary>
        /// 新增的列表
        /// </summary>
        public CP_DoctorOrders CP_DoctorOrdersInsert
        {
            get
            {
                CP_DoctorOrders temp = new CP_DoctorOrders();

                foreach (CP_DoctorOrder item in this)
                {
                    if (item.OperaionState == Operation.Insert)
                    {
                        temp.Add(item);
                    }
                }
                return temp;
            }
        }
        /// <summary>
        /// 删除的列表
        /// </summary>
        public CP_DoctorOrders CP_DoctorOrdersDelete
        {
            get
            {
                CP_DoctorOrders temp = new CP_DoctorOrders();

                foreach (CP_DoctorOrder item in this)
                {
                    if (item.OperaionState == Operation.Delete)
                    {
                        temp.Add(item);
                    }
                }
                return temp;
            }
        }
        /// <summary>
        /// 更新的列表
        /// </summary>
        public CP_DoctorOrders CP_DoctorOrdersUpdate
        {
            get
            {
                CP_DoctorOrders temp = new CP_DoctorOrders();

                foreach (CP_DoctorOrder item in this)
                {
                    if (item.OperaionState == Operation.Update)
                    {
                        temp.Add(item);
                    }
                }
                return temp;
            }
        }
    }

    /// <summary>
    /// 医嘱类
    /// </summary>
    public partial class CP_DoctorOrder
    {
        public Operation OperaionState
        {
            get;
            set;
        }

        public CP_VariantRecords CP_VariantRecordsTemp = new CP_VariantRecords();
    }
}
