using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Data;
using System.Reflection;

namespace Yidansoft.Service
{
    public class Util
    {

        internal static string UnzipContent(string emrContent)
        {
            try
            {
                if (emrContent == string.Empty)
                    return "";
                byte[] rbuff = Convert.FromBase64String(emrContent);
                MemoryStream ms = new MemoryStream(rbuff);
                DeflateStream dfs = new DeflateStream(ms, CompressionMode.Decompress, true);
                StreamReader sr = new StreamReader(dfs, Encoding.UTF8);
                string sXml = sr.ReadToEnd();
                sr.Close();
                dfs.Close();
                return sXml;
            }
            catch (Exception e)
            {
                System.Diagnostics.Trace.WriteLine(e);
                return emrContent;
            }
        }

        internal static string ZipContent(string emrContent)
        {
            byte[] buffUnzipXml = Encoding.UTF8.GetBytes(emrContent);
            MemoryStream ms = new MemoryStream();
            DeflateStream dfs = new DeflateStream(ms, CompressionMode.Compress, true);
            dfs.Write(buffUnzipXml, 0, buffUnzipXml.Length);
            dfs.Close();
            ms.Seek(0, SeekOrigin.Begin);
            byte[] buffZipXml = new byte[ms.Length];
            ms.Read(buffZipXml, 0, buffZipXml.Length);
            return Convert.ToBase64String(buffZipXml);
        }

        /// <summary>
        /// 将泛类型集合List类转换成DataTable
        /// </summary>
        /// <param name="list">泛类型集合</param>
        /// <returns></returns>
        public static DataTable ListToDataTable<T>(List<T> entitys)
        {
            //检查实体集合不能为空
            if (entitys == null || entitys.Count < 1)
            {
                throw new Exception("需转换的集合为空");
            }
            //取出第一个实体的所有Propertie
            Type entityType = entitys[0].GetType();
            PropertyInfo[] entityProperties = entityType.GetProperties();

            //生成DataTable的structure
            //生产代码中，应将生成的DataTable结构Cache起来，此处略
            DataTable dt = new DataTable();
            for (int i = 0; i < entityProperties.Length; i++)
            {
                //dt.Columns.Add(entityProperties[i].Name, entityProperties[i].PropertyType);
                dt.Columns.Add(entityProperties[i].Name);
            }
            //将所有entity添加到DataTable中
            foreach (object entity in entitys)
            {
                //检查所有的的实体都为同一类型
                if (entity.GetType() != entityType)
                {
                    throw new Exception("要转换的集合元素类型不一致");
                }
                object[] entityValues = new object[entityProperties.Length];
                for (int i = 0; i < entityProperties.Length; i++)
                {
                    entityValues[i] = entityProperties[i].GetValue(entity, null);
                }
                dt.Rows.Add(entityValues);
            }
            return dt;
        }
    }
}