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

namespace YidanEHRApplication.Helpers
{
    public abstract class GlobalEnum
    {

    }
    /// <summary>
    /// 医嘱管理标志
    /// </summary>
    public enum OrderType
    {
        /// <summary>
        /// 长期医嘱
        /// </summary>
        Long = 2703,
        /// <summary>
        /// 临时医嘱
        /// </summary>
        Temp = 2702,
        /// <summary>
        ///  普通
        /// </summary>
        Normal = 2700,
        /// <summary>
        /// 不用于医嘱
        /// </summary>
        NoUse = 2701
    }

    /// <summary>
    /// 医嘱管理标志
    /// </summary>
    public enum UnitsType
    {
        /// <summary>
        /// 规格单位
        /// </summary>
        UnitSize = 3006,
        /// <summary>
        /// 最小单位
        /// </summary>
        MinSize = 3007
    }



    /// <summary>
    /// 路径执行时操作
    /// </summary>
    public enum ManualType
    {
        New,
        Edit
    }

    /// <summary>
    /// 路径执行状态
    /// </summary>
    public enum PathStatus
    {
        /// <summary>
        /// 未引入
        /// </summary>
        New = -1,
        /// <summary>
        /// 全部
        /// </summary>
        None = 0,
        /// <summary>
        /// 进入
        /// </summary>
        InPath = 1,
        /// <summary>
        /// 退出
        /// </summary>
        QuitPath = 2,

        /// <summary>
        /// 完成
        /// </summary>
        DonePath = 3,
        /// <summary>
        /// 未完成评估
        /// </summary>
        NotIn = 4


 

    }

    /// <summary>
    /// 医嘱状态
    /// </summary>
    public enum OrderStatus
    {
        /// <summary>
        /// 录入
        /// </summary>
        OrderInptut = 3200,

        /// <summary>
        /// 已审核
        /// </summary>  
        OrderVerify = 3201,

        /// <summary>
        ///  已执行
        /// </summary>
        OrderEnforce = 3202,

        /// <summary>
        ///  被取消DC
        /// </summary>
        OrderDC = 3203,

        /// <summary>
        /// 已停止
        /// </summary>
        OrderStop = 3204
    }


    /// <summary>
    ///  项目类别
    /// </summary>
    public enum OrderItemCategory
    {
        /// <summary>
        /// 未知
        /// </summary>
        None = 0,
        /// <summary>
        /// 西药
        /// </summary>
        WesternMedicine = 2401,
        /// <summary>
        /// 成药
        /// </summary>
        PatentMedicine = 2402,
        /// <summary>
        /// 草药
        /// </summary>
        HerbalMedicine = 2403,
        /// <summary>
        /// 治疗
        /// </summary>
        Cure = 2404,
        /// <summary>
        /// 手术
        /// </summary>
        Operation = 2405,
        /// <summary>
        /// 麻醉
        /// </summary>
        Anesthesia = 2406,
        /// <summary>
        /// 膳食
        /// </summary>
        Meal = 2407,
        /// <summary>
        /// 输血
        /// </summary>
        Transfusion = 2408,
        /// <summary>
        /// 护理
        /// </summary>
        Care = 2409,
        /// <summary>
        /// 床位
        /// </summary>
        BedFee = 2410,
        /// <summary>
        /// 检查
        /// </summary>
        Examination = 2411,
        /// <summary>
        /// 检验
        /// </summary>
        Assay = 2412,
        /// <summary>
        /// 输液
        /// </summary>
        Infusion = 2413,
        /// <summary>
        /// 挂号
        /// </summary>
        Registration = 2414,
        /// <summary>
        /// 材料
        /// </summary>
        Meterial = 2415,
        /// <summary>
        /// 诊疗(注意与治疗的区别)
        /// </summary>
        Diagnosis = 2416,
        /// <summary>
        /// 其它
        /// </summary>
        Other = 2417,
        /// <summary>
        /// 观察
        /// </summary>
        Observation = 2418,
        /// <summary>
        /// 活动
        /// </summary>
        Activity = 2419,
        /// <summary>
        /// 糖
        /// </summary>
        Sugar = 2420,
        /// <summary>
        /// 危重级别
        /// </summary>
        DangerLevel = 2421,
        /// <summary>
        /// 隔离种类
        /// </summary>
        IsolationCatalog = 2422,
        /// <summary>
        /// 体位
        /// </summary>
        BodyPosition = 2423,
        /// <summary>
        /// 纯医嘱
        /// </summary>
        ChunOrder = 2424,
        /// <summary>
        /// 草药协定方
        /// </summary>
        CyOrder = 2425,
    }

    /// <summary>
    /// 成套医嘱类别
    /// </summary>
    public enum OrderPanelBarCategory
    {
        /// <summary>
        /// 药品
        /// </summary>
        Drug = 3100,

        /// <summary>
        /// 手术
        /// </summary>
        Oper = 3105,

        /// <summary>
        /// 检验检查
        /// </summary>
        RisLis = 3114,

        /// <summary>
        /// 营养膳食
        /// </summary>
        Meal = 3115,

        /// <summary>
        /// 观察
        /// </summary>
        Observation = 3116,

        /// <summary>
        /// 活动
        /// </summary>
        Activity = 3117,

        /// <summary>
        /// 护理及宣教
        /// </summary>
        Care = 3118,

        /// <summary>
        /// 纯医嘱
        /// </summary>
        ChunOrder = 3119,

        /// <summary>
        /// 其它医嘱
        /// </summary>
        Other = 3120,

        /// <summary>
        /// 中药院协定方
        /// </summary>
        CyOrder = 3121,

        /// <summary>
        /// 进入条件
        /// </summary>
        EnterCondition = 9999,

    

        

    }



    /// <summary>
    /// 路径执行时使用
    /// </summary>
    public enum OrderFromTable
    {
        /// <summary>
        /// 组套
        /// </summary>
        CP_AdviceGroupDetail,

        /// <summary>
        /// 临时
        /// </summary>
        CP_TempOrder,

        /// <summary>
        /// 长期
        /// </summary>
        CP_LongOrder,

        /// <summary>
        /// 草药处方
        /// </summary>
        CP_CyXDFOrder

    }

    /// <summary>
    /// 页面控件视觉权限
    /// </summary>
    public enum EditState
    {
        /// <summary>
        /// 无
        /// </summary>
        None = 1,
        /// <summary>
        /// 新增
        /// </summary>
        Add = 2,
        /// <summary>
        /// 编辑
        /// </summary>
        Edit = 4,
        /// <summary>
        /// 视图
        /// </summary>
        View = 8


    }

    /// <summary>
    /// 变异类型
    /// </summary>
    public enum VariationCategory
    {
        /// <summary>
        /// 必选未执行
        /// </summary>
        Undo = 1300,

        /// <summary>
        ///  新增
        /// </summary>
        New = 1301,

        /// <summary>
        /// 表单
        /// </summary>
        Form = 1302,

        /// <summary>
        /// 退出
        /// </summary>
        Quit = 1303

    }
    /// <summary>
    /// 变异类别
    /// </summary>
    public enum VariationType
    {
        /// <summary>
        /// 医嘱
        /// </summary>
        Order = 1200,

        /// <summary>
        ///  护理
        /// </summary>
        Care = 1201,

        /// <summary>
        /// 诊疗计划
        /// </summary>
        Treatment = 1203,

        /// <summary>
        /// 退出路径
        /// </summary>
        Quit = 1204
    }




    /// <summary>
    /// 路径状态
    /// </summary>
    public enum PathShStatus
    {
        /// <summary>
        ///   无效
        /// </summary>
        Cancel = 0,

        /// <summary>
        /// 有效
        /// </summary>
        Valid = 1,

        /// <summary>
        /// 停止
        /// </summary>
        Dc = 2,

        /// <summary>
        /// 审核
        /// </summary>
        Review = 3

    }


}
