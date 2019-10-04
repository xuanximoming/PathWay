using System;
using System.Collections.Generic;
using System.Text;
using System.IO;

namespace YidanSoft.Core {
    /// <summary>
    /// ��ȡϵͳ���Ļ������á��������ݿ��л�ȡ������ͨ���ļ���ʽ��ȡ��
    /// ���⴦��
    ///   ����ShowList����ڴ����������ʹ��ʱ������Ҫ��ȡ��Ӧ�����á�
    ///   ��Ҫ�����ݿ��ȡ���ã�����Ҫ��devenv.exe.config.
    ///   ���ԣ�����ʹ���ж�AppDomain�ķ�ʽ���������⴦��
    /// һ�㷵��Stream����
    /// </summary>
    public static class BasicSettings {
        #region public const KeyName
        /// <summary>
        /// ҽ������
        /// </summary>
        public const string DoctorAdviceSetting = "DoctorAdviceSetting";
        /// <summary>
        /// ORMӳ��
        /// </summary>
        public const string ORMappingSetting = "ORMapping";
        /// <summary>
        /// Ԥ����SQL���
        /// </summary>
        public const string PreDefineSqlSetting = "PreDefineSQL";
        /// <summary>
        /// �����ֵ�����
        /// </summary>
        public const string WordbookSetting = "Wordbook";
        /// <summary>
        /// ������ʽ
        /// </summary>
        public const string DevSkinConfigKey = "DevSkin";
        /// <summary>
        /// �༭��Ĭ����ʽ
        /// </summary>
        public const string EmrDefaultSetting = "EmrDefaultSet";
        #endregion

        #region private const variables
        private const string DEVENVAPPDOMAINMANAGER = "Microsoft.VisualStudio.CommonIDE.VsAppDomainManager";

        private const string OrmSettingFile = "YidansoftORMappings.xml";
        private const string SqlSentenceFile = "PreDefSqlSentence.xml";
        private const string WordbookFile = "YidansoftWordbooks.xml";
        private const string FrameworkPath = @"D:\SHIS2008\SourceCode\ϵͳ���\Plugins\SHIS\EmrObjectPersistent\XmlFile\";
        private const string WordbookPath = @"D:\SHIS2008\SourceCode\����ϵͳ\����ѡ�����\Wordbook\XmlFile\";
        #endregion

        #region private properties
        private static string FullOrmSettingFileName {
            get { return CheckAndGetDataFileFullPath(OrmSettingFile, FrameworkPath); }
        }

        private static string FullSqlSentenceFileName {
            get { return CheckAndGetDataFileFullPath(SqlSentenceFile, FrameworkPath); }
        }

        private static string FullWordbookFileName {
            get {
                return CheckAndGetDataFileFullPath(WordbookFile, WordbookPath);
            }
        }

        private static AppConfigDalc ConfigDalc {
            get {
                if (_configDalc == null)
                    _configDalc = new AppConfigDalc();
                return _configDalc;
            }
        }
        private static AppConfigDalc _configDalc;
        #endregion

        #region private variables
        private static Dictionary<string, object> m_Configs = new Dictionary<string, object>();
        #endregion

        /// <summary>
        /// ��ȡָ����ֵ��Ӧ�Ĳ�������
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static Stream GetConfig(string key) {

            EmrAppConfig config = ConfigDalc.SelectAppConfig(key);
            if (config != null)
                return new MemoryStream(System.Text.Encoding.UTF8.GetBytes(config.Config));

            return null;
        }

        /// <summary>
        /// ��ȡString���͵�����
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public static string GetStringConfig(string key) {
            if (!m_Configs.ContainsKey(key)) {
                EmrAppConfig config = ConfigDalc.SelectAppConfig(key);
                if (config != null)
                    m_Configs.Add(key, config.Config);
                else
                    return String.Empty;
            }

            return m_Configs[key] as string;
        }

        #region private methods
        private static Stream HandleSettingFile(string key) {
            switch (key) {
                default:
                    return null;
            }
        }

        private static Stream ReadSettingsFile(string fullFileName) {
            FileStream file = new FileStream(fullFileName, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
            return file;
        }

        private static string CheckAndGetDataFileFullPath(string fileName, string path) {
            string fullName = path + fileName;
            // ���XML�ļ��Ƿ����
            if (File.Exists(fullName))
                return fullName;
            else
                throw new FileNotFoundException("���������ļ�:" + fullName);
        }
        #endregion
    }

}
