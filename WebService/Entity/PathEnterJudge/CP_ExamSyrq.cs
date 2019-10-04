using System;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Data;
using YidanSoft.Tool;
namespace Yidansoft.Service.Entity
{

    /// <summary>
    /// ������Ⱥ�б�
    /// </summary>
    public class CP_ExamSyrq : System.Object
    {
        public CP_ExamSyrq() { }
        public CP_ExamSyrq(String jlxhpara)
        {
            Jlxh = jlxhpara;
        }
        #region ����
        public int ID
        {
            get
            {
                return iD;
            }
            set
            {
                if (iD != value)
                {
                    iD = value;
                }
            }
        }
        private int iD;
        public string Jlxh
        {
            get
            {
                return jlxh;
            }
            set
            {
                if (jlxh != value)
                {
                    jlxh = value;
                }
            }
        }
        private string jlxh;
        public string MC
        {
            get
            {
                return mC;
            }
            set
            {
                if (mC != value)
                {
                    mC = value;
                }
            }
        }
        private string mC;
        public int Yxjb
        {
            get
            {
                return yxjb;
            }
            set
            {
                if (yxjb != value)
                {
                    yxjb = value;
                }
            }
        }
        private int yxjb;
        private String _Xb;
        public String Xb
        {
            get { return _Xb; }
            set { _Xb = value; }
        }
        private Int32 _Qsnl;
        public Int32 Qsnl
        {
            get { return _Qsnl; }
            set { _Qsnl = value; }
        }
        private Int32 _Jsnl;
        public Int32 Jsnl
        {
            get { return _Jsnl; }
            set { _Jsnl = value; }
        }
        #endregion
    }
    public class CP_ExamSyrqs : List<CP_ExamSyrq>
    {
        public static CP_ExamSyrqs GetAllCP_ExamSyrq()
        {
            CP_ExamSyrqs CP_ExamSyrqsTemp = new CP_ExamSyrqs();
            //// DataTable dt = SqlHelper.ExecuteDataTable(YidanEHRDataService.m_ConnectionString, CommandType.Text, "select * from CP_ExamSyrq", null);
            DataTable dt = Yidansoft.Service.YidanEHRDataService.SqlHelper.ExecuteDataTable("select * from CP_ExamSyrq order by Yxjb");
            if (dt != null && dt.Rows != null && dt.Rows.Count > 0)
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    CP_ExamSyrq CP_ExamSyrqTemp = new CP_ExamSyrq();
                    CP_ExamSyrqTemp.Jlxh = ConvertMy.ToString(dt.Rows[i]["Jlxh"]);
                    CP_ExamSyrqTemp.MC = ConvertMy.ToString(dt.Rows[i]["MC"]);
                    CP_ExamSyrqTemp.Yxjb = ConvertMy.ToInt32(dt.Rows[i]["Yxjb"]);
                    CP_ExamSyrqTemp.Qsnl = ConvertMy.ToInt32(dt.Rows[i]["Qsnl"]);
                    CP_ExamSyrqTemp.Jsnl = ConvertMy.ToInt32(dt.Rows[i]["Jsnl"]);
                    CP_ExamSyrqTemp.Xb = ConvertMy.ToString(dt.Rows[i]["Xb"]);
                    CP_ExamSyrqsTemp.Add(CP_ExamSyrqTemp);
                }

            //#region ����ģ������


            ////   ID	Jlxh	Mc	Yxj	Xb	Qsnl	Jsnl
            //CP_ExamSyrq CP_ExamSyrqTemp1 = new CP_ExamSyrq();
            //CP_ExamSyrq CP_ExamSyrqTemp2 = new CP_ExamSyrq();
            //CP_ExamSyrq CP_ExamSyrqTemp3 = new CP_ExamSyrq();
            //CP_ExamSyrq CP_ExamSyrqTemp4 = new CP_ExamSyrq();
            //CP_ExamSyrq CP_ExamSyrqTemp5 = new CP_ExamSyrq();
            //CP_ExamSyrq CP_ExamSyrqTemp6 = new CP_ExamSyrq();
            //CP_ExamSyrqTemp1.ID = 1;
            //CP_ExamSyrqTemp2.ID = 2;
            //CP_ExamSyrqTemp3.ID = 3;
            //CP_ExamSyrqTemp4.ID = 4;
            //CP_ExamSyrqTemp5.ID = 5;
            //CP_ExamSyrqTemp6.ID = 6;

            //CP_ExamSyrqTemp1.Jlxh = "1";
            //CP_ExamSyrqTemp2.Jlxh = "2";
            //CP_ExamSyrqTemp3.Jlxh = "3";
            //CP_ExamSyrqTemp4.Jlxh = "4";
            //CP_ExamSyrqTemp5.Jlxh = "5";
            //CP_ExamSyrqTemp6.Jlxh = "6";

            //CP_ExamSyrqTemp1.MC = "��";
            //CP_ExamSyrqTemp2.MC = "����";
            //CP_ExamSyrqTemp3.MC = "����";
            //CP_ExamSyrqTemp4.MC = "Ů��";
            //CP_ExamSyrqTemp5.MC = "��ͯ";
            //CP_ExamSyrqTemp6.MC = "yinger";

            //CP_ExamSyrqTemp1.Yxjb = 5;
            //CP_ExamSyrqTemp2.Yxjb = 4;
            //CP_ExamSyrqTemp3.Yxjb = 3;
            //CP_ExamSyrqTemp4.Yxjb = 3;
            //CP_ExamSyrqTemp5.Yxjb = 2;
            //CP_ExamSyrqTemp6.Yxjb = 1;

            //CP_ExamSyrqTemp1.Xb = "��Ů";
            //CP_ExamSyrqTemp2.Xb = "��Ů";
            //CP_ExamSyrqTemp3.Xb = "��";
            //CP_ExamSyrqTemp4.Xb = "Ů";
            //CP_ExamSyrqTemp5.Xb = "��Ů";
            //CP_ExamSyrqTemp6.Xb = "��Ů";

            //CP_ExamSyrqTemp1.Qsnl = 0;
            //CP_ExamSyrqTemp2.Qsnl = 18;
            //CP_ExamSyrqTemp3.Qsnl = 0;
            //CP_ExamSyrqTemp4.Qsnl = 0;
            //CP_ExamSyrqTemp5.Qsnl = 2;
            //CP_ExamSyrqTemp6.Qsnl = 0;

            //CP_ExamSyrqTemp1.Jsnl = 150;
            //CP_ExamSyrqTemp2.Jsnl = 150;
            //CP_ExamSyrqTemp3.Jsnl = 150;
            //CP_ExamSyrqTemp4.Jsnl = 150;
            //CP_ExamSyrqTemp5.Jsnl = 10;
            //CP_ExamSyrqTemp6.Jsnl = 1;

            //CP_ExamSyrqsTemp.Add(CP_ExamSyrqTemp6);
            //CP_ExamSyrqsTemp.Add(CP_ExamSyrqTemp5);
            //CP_ExamSyrqsTemp.Add(CP_ExamSyrqTemp4);
            //CP_ExamSyrqsTemp.Add(CP_ExamSyrqTemp3);
            //CP_ExamSyrqsTemp.Add(CP_ExamSyrqTemp2);
            //CP_ExamSyrqsTemp.Add(CP_ExamSyrqTemp1);
            //#endregion

            return CP_ExamSyrqsTemp;
        }
    }
}
