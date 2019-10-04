//using System;
//using System.Data;
//using System.Configuration;
//using System.Web;
//using System.Web.Security;
//using System.Collections;
//using System.Data.SqlClient;


///// <summary>
///// 数据库的通用访问代码
///// 此类为抽象类，不允许实例化，在应用时直接调用即可
///// </summary>
//public abstract partial class SqlHelper
//{
//    //获取数据库连接字符串，其属于静态变量且只读，项目中所有文档可以直接使用，但不能修改
//    //public static readonly string ConnectionStringLocalTransaction = ConfigurationManager.ConnectionStrings["pubsConnectionString"].ConnectionString;

//    // 哈希表用来存储缓存的参数信息，哈希表可以存储任意类型的参数。
//    private static Hashtable parmCache = Hashtable.Synchronized(new Hashtable());
//    #if DEBUG

//        public static string m_ConnectionString = @"Database=YidanEHR_New;Server=192.168.2.202\two;user id=sa;password=sa";
//        private static string m_ConnectionStringEMR = @"Database=YidanEMR;Server=192.168.2.202\two;user id=sa;password=sa";
//        private static string m_ConnectionHISString = @"Database=THIS4_LY;Server=192.168.2.202\two;user id=sa;password=sa";
//#else
//        private static string m_ConnectionStringEMR = ConfigurationManager.AppSettings["EMRDB"].ToString();
//        public static string m_ConnectionString = ConfigurationManager.AppSettings["EHRDB"].ToString();
//        private static string m_ConnectionHISString = System.Configuration.ConfigurationManager.AppSettings["HISDB"].ToString();
//#endif

//    public static DataSet ExecuteDataSet(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
//    {
//        SqlCommand cmd = new SqlCommand();

//        using (SqlConnection conn = new SqlConnection(connectionString))
//        {
//            //通过PrePareCommand方法将参数逐个加入到SqlCommand的参数集合中
//            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
//            SqlDataAdapter da = new SqlDataAdapter(cmd);
//            DataSet ds = new DataSet();
//            da.Fill(ds);

//            //清空SqlCommand中的参数列表
//            cmd.Parameters.Clear();
//            return ds;
//        }

//    }

//    public static DataSet ExecuteDataSet(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
//    {
//        SqlCommand cmd = new SqlCommand();

//        //通过PrePareCommand方法将参数逐个加入到SqlCommand的参数集合中
//        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
//        SqlDataAdapter da = new SqlDataAdapter(cmd);
//        DataSet ds = new DataSet();
//        da.Fill(ds);

//        //清空SqlCommand中的参数列表
//        cmd.Parameters.Clear();
//        return ds;
//    }

//    public static DataTable ExecuteDataTable(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
//    {
//        SqlCommand cmd = new SqlCommand();

//        using (SqlConnection conn = new SqlConnection(connectionString))
//        {
//            //通过PrePareCommand方法将参数逐个加入到SqlCommand的参数集合中
//            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
//            SqlDataAdapter da = new SqlDataAdapter(cmd);
//            DataSet ds = new DataSet();
//            da.Fill(ds);

//            //清空SqlCommand中的参数列表
//            cmd.Parameters.Clear();

//            return ds.Tables.Count > 0 ? ds.Tables[0] : null;
//        }

//    }

//    public static DataTable ExecuteDataTable(SqlConnection conn, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
//    {
//        SqlCommand cmd = new SqlCommand();


//        //通过PrePareCommand方法将参数逐个加入到SqlCommand的参数集合中
//        PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
//        SqlDataAdapter da = new SqlDataAdapter(cmd);
//        DataSet ds = new DataSet();
//        da.Fill(ds);

//        //清空SqlCommand中的参数列表
//        cmd.Parameters.Clear();

//        return ds.Tables[0];


//    }

//    /// <summary>
//    /// 执行一条返回结果集的SqlCommand命令，通过专用的连接字符串。
//    /// 使用参数数组提供参数
//    /// </summary>
//    /// <remarks>
//    /// 使用示例：  
//    ///  SqlDataReader r = ExecuteReader(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
//    /// </remarks>
//    /// <param name="connectionString">一个有效的数据库连接字符串</param>
//    /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
//    /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
//    /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
//    /// <returns>返回一个包含结果的SqlDataReader</returns>
//    public static SqlDataReader ExecuteReader(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
//    {
//        SqlCommand cmd = new SqlCommand();
//        SqlConnection conn = new SqlConnection(connectionString);

//        // 在这里使用try/catch处理是因为如果方法出现异常，则SqlDataReader就不存在，
//        //CommandBehavior.CloseConnection的语句就不会执行，触发的异常由catch捕获。
//        //关闭数据库连接，并通过throw再次引发捕捉到的异常。
//        try
//        {
//            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
//            SqlDataReader rdr = cmd.ExecuteReader(CommandBehavior.CloseConnection);
//            cmd.Parameters.Clear();
//            return rdr;
//        }
//        catch
//        {
//            conn.Close();
//            throw;
//        }
//    }

//    /// <summary>
//    ///执行一个不需要返回值的SqlCommand命令，通过指定专用的连接字符串。
//    /// 使用参数数组形式提供参数列表 
//    /// </summary>
//    /// <remarks>
//    /// 使用示例：
//    ///  int result = ExecuteNoneQuery(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
//    /// </remarks>
//    /// <param name="connectionString">一个有效的数据库连接字符串</param>
//    /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
//    /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
//    /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
//    /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
//    public static int ExecuteNoneQuery(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
//    {

//        SqlCommand cmd = new SqlCommand();

//        using (SqlConnection conn = new SqlConnection(connectionString))
//        {
//            //通过PrePareCommand方法将参数逐个加入到SqlCommand的参数集合中
//            PrepareCommand(cmd, conn, null, cmdType, cmdText, commandParameters);
//            int val = cmd.ExecuteNoneQuery();

//            //清空SqlCommand中的参数列表
//            cmd.Parameters.Clear();
//            return val;
//        }
//    }

//    /// <summary>
//    ///执行一条不返回结果的SqlCommand，通过一个已经存在的数据库连接 
//    /// 使用参数数组提供参数
//    /// </summary>
//    /// <remarks>
//    /// 使用示例：  
//    ///  int result = ExecuteNoneQuery(conn, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
//    /// </remarks>
//    /// <param name="conn">一个现有的数据库连接</param>
//    /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
//    /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
//    /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
//    /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
//    public static int ExecuteNoneQuery(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
//    {

//        SqlCommand cmd = new SqlCommand();

//        PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
//        int val = cmd.ExecuteNoneQuery();
//        cmd.Parameters.Clear();
//        return val;
//    }

//    /// <summary>
//    /// 执行一条不返回结果的SqlCommand，通过一个已经存在的数据库事物处理 
//    /// 使用参数数组提供参数
//    /// </summary>
//    /// <remarks>
//    /// 使用示例： 
//    ///  int result = ExecuteNoneQuery(trans, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
//    /// </remarks>
//    /// <param name="trans">一个存在的 sql 事物处理</param>
//    /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
//    /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
//    /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
//    /// <returns>返回一个数值表示此SqlCommand命令执行后影响的行数</returns>
//    public static int ExecuteNoneQuery(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
//    {
//        SqlCommand cmd = new SqlCommand();
//        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
//        int val = cmd.ExecuteNoneQuery();
//        cmd.Parameters.Clear();
//        return val;
//    }

//    /// <summary>
//    /// 执行一条返回第一条记录第一列的SqlCommand命令，通过专用的连接字符串。 
//    /// 使用参数数组提供参数
//    /// </summary>
//    /// <remarks>
//    /// 使用示例：  
//    ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
//    /// </remarks>
//    /// <param name="connectionString">一个有效的数据库连接字符串</param>
//    /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
//    /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
//    /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
//    /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
//    public static object ExecuteScalar(string connectionString, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
//    {
//        SqlCommand cmd = new SqlCommand();

//        using (SqlConnection connection = new SqlConnection(connectionString))
//        {
//            PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
//            object val = cmd.ExecuteScalar();
//            cmd.Parameters.Clear();
//            return val;
//        }
//    }

//    /// <summary>
//    /// 执行一条返回第一条记录第一列的SqlCommand命令，通过已经存在的数据库连接。
//    /// 使用参数数组提供参数
//    /// </summary>
//    /// <remarks>
//    /// 使用示例： 
//    ///  Object obj = ExecuteScalar(connString, CommandType.StoredProcedure, "PublishOrders", new SqlParameter("@prodid", 24));
//    /// </remarks>
//    /// <param name="conn">一个已经存在的数据库连接</param>
//    /// <param name="commandType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
//    /// <param name="commandText">存储过程的名字或者 T-SQL 语句</param>
//    /// <param name="commandParameters">以数组形式提供SqlCommand命令中用到的参数列表</param>
//    /// <returns>返回一个object类型的数据，可以通过 Convert.To{Type}方法转换类型</returns>
//    public static object ExecuteScalar(SqlConnection connection, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
//    {

//        SqlCommand cmd = new SqlCommand();

//        PrepareCommand(cmd, connection, null, cmdType, cmdText, commandParameters);
//        object val = cmd.ExecuteScalar();
//        cmd.Parameters.Clear();
//        return val;
//    }

//    /// <summary>
//    /// 执行一条返回第一条记录第一列的SqlCommand命令，通过事务。  ZM 8.25
//    /// </summary>
//    /// <param name="connectionString"></param>
//    /// <param name="cmdType"></param>
//    /// <param name="cmdText"></param>
//    /// <param name="commandParameters"></param>
//    /// <returns></returns>
//    public static object ExecuteScalar(SqlTransaction trans, CommandType cmdType, string cmdText, params SqlParameter[] commandParameters)
//    {
//        SqlCommand cmd = new SqlCommand();
//        PrepareCommand(cmd, trans.Connection, trans, cmdType, cmdText, commandParameters);
//        object val = cmd.ExecuteScalar();
//        cmd.Parameters.Clear();
//        return val;
//    }






//    /// <summary>
//    /// 缓存参数数组
//    /// </summary>
//    /// <param name="cacheKey">参数缓存的键值</param>
//    /// <param name="cmdParms">被缓存的参数列表</param>
//    public static void CacheParameters(string cacheKey, params SqlParameter[] commandParameters)
//    {
//        parmCache[cacheKey] = commandParameters;
//    }

//    /// <summary>
//    /// 获取被缓存的参数
//    /// </summary>
//    /// <param name="cacheKey">用于查找参数的KEY值</param>
//    /// <returns>返回缓存的参数数组</returns>
//    public static SqlParameter[] GetCachedParameters(string cacheKey)
//    {
//        SqlParameter[] cachedParms = (SqlParameter[])parmCache[cacheKey];

//        if (cachedParms == null)
//            return null;

//        //新建一个参数的克隆列表
//        SqlParameter[] clonedParms = new SqlParameter[cachedParms.Length];

//        //通过循环为克隆参数列表赋值
//        for (int i = 0, j = cachedParms.Length; i < j; i++)
//            //使用clone方法复制参数列表中的参数
//            clonedParms[i] = (SqlParameter)((ICloneable)cachedParms[i]).Clone();

//        return clonedParms;
//    }

//    /// <summary>
//    /// 为执行命令准备参数
//    /// </summary>
//    /// <param name="cmd">SqlCommand 命令</param>
//    /// <param name="conn">已经存在的数据库连接</param>
//    /// <param name="trans">数据库事物处理</param>
//    /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
//    /// <param name="cmdText">Command text，T-SQL语句 例如 Select * from Products</param>
//    /// <param name="cmdParms">返回带参数的命令</param>
//    private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameter[] cmdParms)
//    {

//        //判断数据库连接状态
//        if (conn.State != ConnectionState.Open)
//            conn.Open();

//        cmd.Connection = conn;
//        cmd.CommandText = cmdText;

//        //判断是否需要事物处理
//        if (trans != null)
//            cmd.Transaction = trans;

//        cmd.CommandType = cmdType;

//        if (cmdParms != null)
//        {
//            foreach (SqlParameter parm in cmdParms)
//            {
//                //if (parm.SqlDbType ==SqlDbType.VarChar || parm.SqlDbType ==SqlDbType.NVarChar)
//                //{
//                //    parm.Value = parm.Value.ToString().Replace("'", "");
//                //}
//                if (parm != null)
//                    cmd.Parameters.Add(parm);
//            }
//        }
//    }
//    /// <summary>
//    /// 为执行命令准备参数
//    /// </summary>
//    /// <param name="cmd">SqlCommand 命令</param>
//    /// <param name="conn">已经存在的数据库连接</param>
//    /// <param name="trans">数据库事物处理</param>
//    /// <param name="cmdType">SqlCommand命令类型 (存储过程， T-SQL语句， 等等。)</param>
//    /// <param name="cmdText">Command text，T-SQL语句 例如 Select * from Products</param>
//    /// <param name="cmdParms">返回带参数的命令</param>
//    private static void PrepareCommand(SqlCommand cmd, SqlConnection conn, SqlTransaction trans, CommandType cmdType, string cmdText, SqlParameterCollection cmdParms)
//    {

//        //判断数据库连接状态
//        if (conn.State != ConnectionState.Open)
//            conn.Open();

//        cmd.Connection = conn;
//        cmd.CommandText = cmdText;

//        //判断是否需要事物处理
//        if (trans != null)
//            cmd.Transaction = trans;

//        cmd.CommandType = cmdType;

//        if (cmdParms != null)
//        {
//            foreach (SqlParameter parm in cmdParms)
//                if (parm != null)
//                    cmd.Parameters.Add(parm);
//        }
//    }

//    /// <summary>
//    /// 根据参数初始化SqlParameter
//    /// </summary>
//    /// <param name="parameterName">存储过程参数名</param>
//    /// <param name="dbType">参数类别</param>
//    /// <param name="size">参数长度</param>
//    /// <param name="value">参数值</param>
//    /// <returns>SqlParament实体</returns>
//    public static SqlParameter GetSqlparamenterFromArgument(string parameterName, SqlDbType dbType, int size, object value)
//    {
//        SqlParameter para = new SqlParameter(parameterName, dbType, size);
//        para.Value = value;
//        return para;

//    }


//}