namespace DrectSoft.Tool
{
    /// <summary>
    /// 操作状态，指示当前是查看，新增，还是编辑状态
    /// </summary>
    public enum OperationState
    {
        VIEW,
        NEW,
        EDIT,
    }
    /// <summary>
    /// 匹配状态
    /// </summary>
    public enum MatchResultState
    {
        NoExist,//不存在
        NoMatch,//未匹配上
        Match,//匹配上
    }
    /// <summary>
    /// 查询参数
    /// </summary>
    public enum Operation
    {
        Insert,
        Update,
        Select,
        Delete,
        InsertAndSelect,
        UpdateAndSelect,
        DeleteAndSelect

    }
    public enum ToolElementState { Pre, Now, Next, Hide }

    /// <summary>
    /// 医嘱状态
    /// </summary>
    public enum AdviceState
    {
        Add = 1,
        Undo = 2,
        Delete = 4,
        New = 8
    }

}
