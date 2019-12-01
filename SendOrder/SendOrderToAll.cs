using System;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using YidanSoft.Core;

namespace SendOrder
{
    public class SendOrderToAll
    {

        public static IDataAccess SqlHelper = DataAccessFactory.GetSqlDataAccess("EHRDB");
        public static IDataAccess HISHelper = DataAccessFactory.GetSqlDataAccess("HISDB");
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ordertable"></param>
        /// <returns></returns>
        public DataTable SendOrder(DataTable ordertable)
        {
            //CheckOrder(ordertable);

            SaveOrder(ordertable);
            return ordertable;
        }


        /// <summary>
        ///验证医嘱到HIS, His去调用这个方法确定库存等信息
        /// </summary>
        /// <param name="orderAdd">需要发送HIS医嘱信息</param>
        private void CheckOrder(DataTable orderAdd)
        {
            foreach (DataRow dr in orderAdd.Select("yzlb='3100'"))
            {
                SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@PhamCode",dr["ypdm"].ToString()),
                    new SqlParameter("@PerformBy",dr["Zxksdm"].ToString()),
                    new SqlParameter("@unit",dr["Jldw"].ToString()),
                    new SqlParameter("@sl",dr["ypjl"].ToString()),
                };

                DataTable dt = SqlHelper.ExecuteDataTable("Up_CheckStockQuantity", parameters, CommandType.StoredProcedure);
                // 0	--够用
                // 1	--不够用
                if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() == "1")
                {
                    foreach (DataRow row in orderAdd.Select("yzlb='3100'"))
                    {
                        if (row["Ctyzxh"].ToString() == dr["Ctyzxh"].ToString())
                        {
                            row["FS_Flag"] = "0";
                            row["FS_Mess"] = "库存不足";
                        }
                    }
                }
            }
        }

        /// <summary>
        /// 发送医嘱到HIS, His去调用这个方法得到EHR发送成功的医嘱内容
        /// </summary>
        /// <param name="orderAdd">发送医嘱表</param>
        private DataTable SaveOrder(DataTable orderAdd)
        {
            //输入参数： BSTR PatientID,病人编号（NoofClinic）	short VisitID,本次住院号（InCount）
            //short OrderNo,医嘱号（ctxh）		short OrderSubNo,医嘱子序号（）
            //BSTR RepeatIndicator,医嘱长临标志（0-长期，1-临时）	BSTR OrderClass,医嘱分类（1-西药,2-其他,3-文字医嘱）

            //BSTR OrderText,	医嘱内容(Name)	BSTR OrderCode,		医嘱代码(文字医嘱代码为空)
            //BSTR OrderSpec,	规格(Ypgg或者Xmgg)	float Dosage,	药品一次使用剂量(医生录入的数量)
            //BSTR DosageUnit,剂量单位(医生选择的单位)	BSTR Administration,给药途径(静滴等。。。)

            //DATE StartTime,		开始时间（医生下医嘱的时间）	@FreSerialNo 执行频率代码 	
            //BSTR Doctor,		开医嘱医生		BSTR OrderedBy,	医嘱的开单科室		
            //BSTR PerformBy	医嘱执行科室(PerformBy)   @BillingAttr  是否计价标志
            string mess = string.Empty;
            try
            {
                //获取HisSxpz表信息接口参数信息
                string sSql = "select * from HisSxpz where HisKey<>''";
                DataTable dtPara = SqlHelper.ExecuteDataTable(sSql);
                int iCount = dtPara.Rows.Count;






                foreach (DataRow dr in orderAdd.Select("FS_Flag='1'"))
                {
                    //string iscqyz = string.Empty;
                    //"长期医嘱"  2703
                    //"临时医嘱"  2702
                    if (dr["yzbz"].ToString() == "2703")
                    {
                        dr["yzbz"] = "1";
                        //iscqyz = "0";
                    }
                    else
                    {
                        dr["yzbz"] = "0";
                        //iscqyz = "1";
                    }

                    //医嘱分类（1-西药,2-其他,3-文字医嘱）
                    //string OrderClass = string.Empty;
                    if (dr["yzlb"].ToString() == "3100")
                    {
                        dr["yzlb"] = "0";
                        //OrderClass = "1";
                    }
                    else if (dr["yzlb"].ToString() == "3119")
                    {
                        dr["yzlb"] = "4";
                        //OrderClass = "3";
                    }
                    else
                    {
                        dr["yzlb"] = "10";
                        //OrderClass = "10";
                    }
                    if (dr["yzzt"].ToString() == "3200")
                    {
                        dr["yzzt"] = "0";
                        //OrderClass = "1";
                    }
                    //3200
                    #region 用HisSxpz表的数据

                    //1、创建一个XML文档
                    XmlDocument doc = new XmlDocument();
                    //2、创建第一行描述信息
                    XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", "utf-16", null);
                    //3、将创建的第一行描述信息添加到文档中
                    doc.AppendChild(dec);
                    //4、给文档添加根节点
                    XmlElement XMLData = doc.CreateElement("XMLData");
                    doc.AppendChild(XMLData);
                    //创建YZInfo节点
                    XmlElement YZInfo = doc.CreateElement("YZInfo");

                    XMLData.AppendChild(YZInfo);
                    XmlElement PatInfo = doc.CreateElement("PatInfo");

                    //循环创建YZ节点
                    XmlElement YZ = doc.CreateElement("YZ");


                    for (int i = 0; i < dtPara.Rows.Count; i++)//DataRow pararow in dtPara.Rows)
                    {
                        for (int j = 0; j < orderAdd.Columns.Count; j++)
                        {

                            if (dtPara.Rows[i]["EhrKey"].ToString().ToUpper().Trim().Equals(orderAdd.Columns[j].ColumnName.ToUpper().Trim()) &&
                                dtPara.Rows[i]["HisKey"].ToString().Split('-').Length == 1)
                            {
                                string[] tt = dtPara.Rows[i]["EhrKey"].ToString().Split('-');
                                int o = tt.Length;
                                YZ.SetAttribute(dtPara.Rows[i]["HisKey"].ToString(), dr[orderAdd.Columns[j].ColumnName].ToString());
                                break;
                            }
                            else if (dtPara.Rows[i]["EhrKey"].ToString().ToUpper().Trim().Equals(orderAdd.Columns[j].ColumnName.ToUpper().Trim()) &&
                                dtPara.Rows[i]["HisKey"].ToString().Split('-').Length > 1)
                            {
                                PatInfo.SetAttribute(dtPara.Rows[i]["HisKey"].ToString().Split('-')[1], dr[orderAdd.Columns[j].ColumnName].ToString());
                                break;
                            }

                        }
                        if (dtPara.Rows[i]["EhrKey"].ToString() == "?" && dtPara.Rows[i]["HisKey"].ToString().Split('-').Length == 1)
                        {
                            YZ.SetAttribute(dtPara.Rows[i]["HisKey"].ToString(), "");
                        }
                        else if (dtPara.Rows[i]["EhrKey"].ToString() == "?" && dtPara.Rows[i]["HisKey"].ToString().Split('-').Length > 1)
                        {
                            PatInfo.SetAttribute(dtPara.Rows[i]["HisKey"].ToString().Split('-')[1], "");
                        }
                    }
                    YZ.SetAttribute("ZHCode", "YZ_2923");
                    XMLData.AppendChild(PatInfo);
                    YZInfo.AppendChild(YZ);
                    SqlParameter[] parameters = new SqlParameter[] 
                {
                    new SqlParameter("@InputXML",doc.OuterXml),
                };
                    string ss = doc.OuterXml;

                    #endregion

                    #region 判断Appcfg中得到His保存医嘱的存取过程名是否有值 空字符串表示没有值
                    //获取Appcfg配置表His保存医嘱的存取过程名
                    string sHispro = GetAppCfgType("HisOrderProc");
                    if (sHispro != "")
                    {
                        DataTable dt = HISHelper.ExecuteDataTable(sHispro, parameters, CommandType.StoredProcedure);

                        //根据HIS返回结果集判断是否插入成功
                        if (dt != null && dt.Rows.Count > 0 && dt.Rows[0][0].ToString() != "0")
                        {
                            //成套医嘱其中一条发送不成功则所有医嘱都不发送
                            foreach (DataRow row in orderAdd.Select("FS_Flag='1'"))
                            {
                                if (row["Ctyzxh"].ToString() == dr["Ctyzxh"].ToString())
                                {
                                    row["FS_Flag"] = "0";
                                    row["FS_Mess"] = dt.Rows[0][0].ToString();
                                }
                            }
                            //dr["FS_Flag"] = "0";
                            //dr["FS_Mess"] = dt.Rows[0][0].ToString();
                        }
                        else
                        {

                        }
                    }
                    else //当Appcfg对应的参数值为空时
                    {
                        //所有医嘱都不发送
                        foreach (DataRow row in orderAdd.Select("FS_Flag='1'"))
                        {

                            row["FS_Flag"] = "0";
                            row["FS_Mess"] = "发送医嘱失败！";

                        }
                    }
                    #endregion
                }
                //return orderAdd;
            }
            catch (Exception ex)
            {
                throw (ex);

            }
            return orderAdd;
        }

        // add by luff 20130219
        #region 判断HIS参数类型是否有值
        /// <summary>
        /// 判断HIS参数类型是否有值
        /// </summary>

        public string GetAppCfgType(string sKey)
        {
            string s_appconfigValue = string.Empty;

            if (s_appconfigValue == string.Empty)
            {
                string sqlStr = string.Empty;

                //Value判断是否有参数值  ""表示为空值
                sqlStr = "select Value from APPCFG where Configkey = '" + sKey + "'";

                DataTable dt = SqlHelper.ExecuteDataTable(sqlStr);
                if (dt == null || dt.Rows.Count == 0)
                {
                    s_appconfigValue = "";
                }
                else
                {
                    s_appconfigValue = dt.Rows[0][0].ToString().Trim();
                }
            }
            return s_appconfigValue;

        }

        #endregion
    }
}
