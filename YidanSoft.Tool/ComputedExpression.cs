namespace DrectSoft.Tool
{
    /// <summary>
    /// 计算表达式抽象类
    /// </summary>
    public abstract class ComputedExpression
    {
        #region 计算表达式
        #region 定义计算关系表达式委托
        /// <summary>
        /// 定义计算关系表达式委托
        /// </summary>
        /// <param name="fltBaseumber">基数值</param>
        /// <param name="fltParameter">比较值</param>
        /// <returns></returns>
        private delegate bool OperationFunc(float fltBaseumber, float fltParameter);
        private static OperationFunc EvalA = (fltBaseumber, fltParameter) => fltBaseumber > fltParameter;
        private static OperationFunc EvalB = (fltBaseumber, fltParameter) => fltBaseumber >= fltParameter;
        private static OperationFunc EvalC = (fltBaseumber, fltParameter) => fltBaseumber < fltParameter;
        private static OperationFunc EvalD = (fltBaseumber, fltParameter) => fltBaseumber <= fltParameter;
        #endregion
        #region 定义计算逻辑表达式委托
        /// <summary>
        /// 定义计算逻辑表达式委托
        /// </summary>
        /// <param name="blValuesA">比较值</param>
        /// <param name="blValuesB">比较值</param>
        /// <returns></returns>
        private delegate bool LogicOperationFun(bool blValuesA, bool blValuesB);
        private static LogicOperationFun LogicEvalA = (blValuesA, blValuesB) => blValuesA && blValuesB;
        private static LogicOperationFun LogicEvalB = (blValuesA, blValuesB) => blValuesA || blValuesB;
        #endregion
        #region 计算关系表达式
        /// <summary>
        /// 计算关系表达式
        /// </summary>
        /// <param name="fltBaseumber">基数值</param>
        /// <param name="strCharacter">关系表达式运算符</param>
        /// <param name="fltParameter">比较值</param>
        /// <returns></returns>
        public static bool Operation(float fltBaseumber, string strCharacter, float fltParameter)
        {
            bool blResult = false;
            switch (strCharacter)
            {
                case ">":
                    {
                        blResult = EvalA(fltBaseumber, fltParameter);
                        break;
                    }
                case ">=":
                    {
                        blResult = EvalB(fltBaseumber, fltParameter);
                        break;
                    }
                case "<":
                    {
                        blResult = EvalC(fltBaseumber, fltParameter);
                        break;
                    }
                case "<=":
                    {
                        blResult = EvalD(fltBaseumber, fltParameter);
                        break;
                    }
            }
            return blResult;
        }
        #endregion
        #region 计算逻辑表达式
        /// <summary>
        /// 计算逻辑表达式
        /// </summary>
        /// <param name="blValuesA">比较值</param>
        /// <param name="strCharacter">逻辑表达式运算符</param>
        /// <param name="blValuesB">比较值</param>
        /// <returns></returns>
        public static bool LogicOperation(bool blValuesA, string strCharacter, bool blValuesB)
        {
            bool blResult = false;
            switch (strCharacter)
            {
                case "&":
                    {
                        blResult = LogicEvalA(blValuesA, blValuesB);
                        break;
                    }
                case "|":
                    {
                        blResult = LogicEvalB(blValuesA, blValuesB);
                        break;
                    }
            }
            return blResult;
        }
        #endregion
        #endregion
    }
}