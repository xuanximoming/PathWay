using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.Data.SqlClient;
using System.Data;
using Yidansoft.Service.Entity;
using System.Collections;
using System.Xml.Linq;
using System.Text;
using System.IO;
using System.IO.Compression;
using System.Configuration;

namespace Yidansoft.Service
{
    public partial class YidanEHRDataService
    {
        /// <summary>
        /// 病人检查结果
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        [FaultContract(typeof(LoginException))]
        public List<V_PatientExamine> GetV_PatientExamineList()
        {
            List<V_PatientExamine> list = new List<V_PatientExamine>();
            //using (SqlConnection myConnection = new SqlConnection(m_ConnectionString))
            //{
            StringBuilder sql = new StringBuilder();
            sql.Append(@"
                 select '黄世仁' PatientName,'生化' Lb,'2011-03-05 5:54' Jcrq,'谷丙转氨酶' Jcxm,'60' Jcjg,'0~65' Zcfw,'u/l' Dw
                    Union select '徐鸣','生化','2011-03-05 5:54','谷草转氨酶','50','15~37','u/l'
                    Union select '黄三爷','生化','2011-03-05 5:54','总蛋白','50','64~82','u/l'
                    Union select '三四','生化','2011-03-05 5:54','谷丙转氨酶','60','0～65','u/l'
                    Union select '王晓峰','生化','2011-03-05 5:54','谷草转氨酶','50','15～37','u/l'
                    Union select '徐西苑','生化','2011-03-05 5:54','总蛋白','50','64～82','g/l'
                    Union select '糊峰','生化','2011-03-05 5:54','白蛋白','35','34.00～50.00','g/l'
                    Union select '和胖子','生化','2011-03-05 5:54','球蛋白','30','20～40','g/l'
                    Union select '黄三爷','生化','2011-03-05 5:54','白球比例','2','1.2～2.5','%'
                    Union select '汪四明','生化','2011-03-05 5:54','谷氨酰转酞酶','5','5～85','u/l'
                    Union select '李刚','生化','2011-03-05 5:54','前白蛋白','100','200～400','mg/L'
                    Union select '狄怀英','生化','2011-03-05 5:54','碱性磷酸酶','50','50～136','u/l'
                    Union select '巩俐','生化','2011-03-05 5:54','总胆红素','10','0～17','umol/l'
                    Union select '张三','生化','2011-03-05 5:54','结合胆红素','3','0～5','umol/l'
                    Union select '李军','生化','2011-03-05 5:54','总胆汁酸','20','0.00～20.00','umol/l'");
            //SqlCommand myCommand = new SqlCommand(sql.ToString(), myConnection);
            //myCommand.CommandType = CommandType.Text;
            //DataTable dataTable = new DataTable();
            //SqlDataAdapter dataAdapter = new System.Data.SqlClient.SqlDataAdapter(myCommand);
            //dataAdapter.Fill(dataTable);

            DataTable dataTable = SqlHelper.ExecuteDataTable(sql.ToString());

            foreach (DataRow r in dataTable.Rows)
            {
                V_PatientExamine p = new V_PatientExamine();
                p.PatientName = r["PatientName"].ToString();
                p.Lb = r["Lb"].ToString();
                p.Jcrq = r["Jcrq"].ToString();
                p.Jcxm = r["Jcxm"].ToString();
                p.Jcjg = r["Jcjg"].ToString();
                p.Zcfw = r["Zcfw"].ToString();
                p.Dw = r["Dw"].ToString();
                list.Add(p);
            }
            // }
            return list;
        }
    }
}