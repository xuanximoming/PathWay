using System;
using System.Collections.Generic;
using System.Text;

namespace YidanSoft.Core
{
    /// <summary>
    /// ���ö�ȡ��
    /// </summary>
    public class AppConfigReader:IAppConfigReader
    {
        AppConfigDalc _acd = new AppConfigDalc();
        static AppConfigReader _instance = new AppConfigReader();

        /// <summary>
        /// ����
        /// </summary>
        public AppConfigReader()
        {
        }

        #region read config

        /// <summary>
        /// ��ȡ�û�����
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public EmrUserConfig GetConfig(string userId, string key)
        {
            return null;
        }

        /// <summary>
        /// ��ȡ����
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public EmrAppConfig GetConfig(string key)
        {
            return _acd.SelectAppConfig(key);
        }

        /// <summary>
        /// ��ȡ�û����ö���
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetConfigObj(string userId, string key)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        private object InternalGetConfigObj(EmrAppConfig eac)
        {
            //string assInfo = eac.DesignClass;
            //IAppConfigDesign iacd = null;
            //if (string.IsNullOrEmpty(assInfo))
            //{
            //    //��ͨ�õ������࣬����string���͵�configֵ
            //    return eac.Config;
            //}
            //else
            //{
            //    Type t = Type.GetType(assInfo);
            //    if (t != null)
            //    {
            //        iacd = (IAppConfigDesign)Activator.CreateInstance(t);
            //    }
            //    if (iacd == null) throw new ArgumentException("design assembly load fail! ");
            //    Dictionary<string, EmrAppConfig> dics = new Dictionary<string, EmrAppConfig>();
            //    dics.Add(eac.Key, eac);
            //    iacd.Load(null, dics);
            //    return iacd.ConfigObj;
            //}
            //todo
            return null;
        }

        /// <summary>
        /// ��ȡ���ö���
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public object GetConfigObj(string key)
        {
            EmrAppConfig eac = _acd.SelectAppConfig(key);
            return InternalGetConfigObj(eac);
        }

        /// <summary>
        /// ��ȡһ���û�����
        /// </summary>
        /// <param name="userId"></param>
        /// <param name="keys"></param>
        /// <returns></returns>
        public Dictionary<string, EmrUserConfig> GetConfigs(string userId, params string[] keys)
        {
            throw new Exception("The method or operation is not implemented.");
        }

        /// <summary>
        /// ��ȡһ������
        /// </summary>
        /// <param name="keys"></param>
        /// <returns></returns>
        public Dictionary<string, EmrAppConfig> GetConfigs(params string[] keys)
        {
            return _acd.SelectAppConfigs(keys);
        }
        #endregion

        #region Static Methods Interface

        /// <summary>
        /// ȡ��ָ��������
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static EmrAppConfig GetAppConfig(string key)
        {
            return _instance.GetConfig(key);
        }

        /// <summary>
        /// ȡ��ָ�������ö���ʵ��
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static object GetAppConfigObj(string key)
        {
            return _instance.GetConfigObj(key);
        }

        #endregion

    }
}
