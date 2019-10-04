using System;
using System.Collections.Generic;
using System.Text;

namespace YidanSoft.Core
{
   ///// <summary>
   ///// ϵͳ���ýӿ�
   ///// </summary>
   //public interface IAppConfigDesign
   //{
   //   /// <summary>
   //   /// ���ý���ؼ�
   //   /// </summary>
   //   Control DesignUI { get;}

   //   /// <summary>
   //   /// �������ü���
   //   /// </summary>
   //   /// <param name="account"></param>
   //   /// <param name="configs"></param>
   //   void Load(IAccount account, Dictionary<string, EmrAppConfig> configs);

   //   /// <summary>
   //   /// �ӿ��ڱ�����ĵ����õ�ChangedConfigs
   //   /// ����ӿ��ڼ�ʱ����ChangedConfigs,�˷�������ʵ��(��Ҫ�׳�δʵ���쳣)
   //   /// </summary>
   //   void Save();

   //   /// <summary>
   //   /// �������ü���
   //   /// </summary>
   //   Dictionary<string, EmrAppConfig> ChangedConfigs { get;}

   //   /// <summary>
   //   /// ���ö���(����з���,û����null)
   //   /// </summary>
   //   object ConfigObj { get;}
   //}

   /// <summary>
   /// ϵͳ����
   /// </summary>
   public class EmrAppConfig
   {
      string _key;
      string _name;
      string _config;
      string _descript;
      ConfigParamType _ptype;
      IList<string> _keyset = new List<string>();
      ConfigChoiceType _ctype;
      string _showlistDict;
      string _designClass;
      ConfigParamLevel _plevel;
      bool _valid;
      bool _canSetUserLevel;
      string _roleId = string.Empty;
      string _userId = string.Empty;
      string ishide;
      string valid;//�Ƿ���Ч

      public string Valid1
      {
          get { return valid; }
          set { valid = value; }
      }
       /// <summary>
       /// ���ر�־
       /// </summary>
      public string IsHide
      {
          get { return ishide; }
          set { ishide = value; }
      }
      Dictionary<string, EmrAppConfig> _subConfigs = new Dictionary<string, EmrAppConfig>();

      /// <summary>
      /// �����ü���
      /// </summary>
      public Dictionary<string, EmrAppConfig> SubConfigs
      {
         get { return _subConfigs; }
         set { _subConfigs = value; }
      }

      /// <summary>
      /// �����Ƿ�������õ��û�����
      /// </summary>
      public bool CanSetUserLevel
      {
         get { return _canSetUserLevel; }
         set { _canSetUserLevel = value; }
      }

      /// <summary>
      /// �����Ƿ���Ч
      /// </summary>
      public bool Valid
      {
         get { return _valid; }
         set { _valid = value; }
      }

      /// <summary>
      /// ���ü���
      /// </summary>
      public ConfigParamLevel Plevel
      {
         get { return _plevel; }
         set { _plevel = value; }
      }

      /// <summary>
      /// ���������
      /// </summary>
      public string DesignClass
      {
         get { return _designClass; }
         set { _designClass = value; }
      }

      /// <summary>
      /// ѡ��ֵ�ֵ�
      /// </summary>
      public string ShowlistDict
      {
         get { return _showlistDict; }
         set { _showlistDict = value; }
      }

      /// <summary>
      /// ����ֵѡ������
      /// </summary>
      public ConfigChoiceType Ctype
      {
         get { return _ctype; }
         set { _ctype = value; }
      }

      /// <summary>
      /// �����ð����ļ�ֵ�б�
      /// </summary>
      public IList<string> Keyset
      {
         get { return _keyset; }
         set { _keyset = value; }
      }

      /// <summary>
      /// ����ֵ����
      /// </summary>
      public ConfigParamType Ptype
      {
         get { return _ptype; }
         set { _ptype = value; }
      }

      /// <summary>
      /// ��������
      /// </summary>
      public string Descript
      {
         get { return _descript; }
         set { _descript = value; }
      }

      /// <summary>
      /// ����ֵ
      /// </summary>
      public string Config
      {
         get { return _config; }
         set { _config = value; }
      }

      /// <summary>
      /// ���ü�ֵ
      /// </summary>
      public string Key
      {
         get { return _key; }
         set { _key = value; }
      }

      /// <summary>
      /// ��������
      /// </summary>
      public string Name
      {
         get { return _name; }
         set { _name = value; }
      }

      /// <summary>
      /// ����
      /// </summary>
      public EmrAppConfig()
      {
      }

      /// <summary>
      /// ��λ����
      /// </summary>
      public string RoleId
      {
         get { return _roleId; }
         set { _roleId = value; }
      }

      /// <summary>
      /// ְ������
      /// </summary>
      public string UserId
      {
         get { return _userId; }
         set { _userId = value; }
      }
   }

   /// <summary>
   /// �û�����(ϵͳ���ü���)
   /// </summary>
   public class EmrUserConfig
   {
      IList<EmrAppConfig> _configs = new List<EmrAppConfig>();
      string _userId;

      /// <summary>
      /// �û�����
      /// </summary>
      public string UserId
      {
         get { return _userId; }
      }

      /// <summary>
      /// ���ü���
      /// </summary>
      public IList<EmrAppConfig> Configs
      {
         get { return _configs; }
      }

      /// <summary>
      /// ����
      /// </summary>
      /// <param name="userId"></param>
      public EmrUserConfig(string userId)
      {
         _userId = userId;
      }

      /// <summary>
      /// ȡ��ָ����λ���������
      /// </summary>
      /// <param name="roleId"></param>
      /// <returns>���ڷ�������,���򷵻�Null</returns>
      public EmrAppConfig GetRoleConfig(string roleId)
      {
         foreach (EmrAppConfig config in _configs)
         {
            if (config.Plevel == ConfigParamLevel.Role
                && config.RoleId == roleId)
               return config;
         }
         return null;
      }

      /// <summary>
      /// ȡ���û�����
      /// </summary>
      /// <returns></returns>
      public EmrAppConfig GetUserConfig()
      {
         foreach (EmrAppConfig config in _configs)
         {
            if (config.Plevel == ConfigParamLevel.User
                && config.UserId == _userId)
               return config;
         }
         return null;
      }

      /// <summary>
      /// ȡ��ϵͳĬ������
      /// </summary>
      /// <returns></returns>
      public EmrAppConfig GetAppConfig()
      {
         foreach (EmrAppConfig config in _configs)
            if (config.Plevel == ConfigParamLevel.System) return config;
         return null;
      }

      /// <summary>
      /// ȡ���û���Ĭ������
      /// ���ȼ�(��->��): �û����� -> ��λ����(Ĭ�ϵ�һ��) -> ϵͳ���� 
      /// </summary>
      /// <returns></returns>
      public EmrAppConfig GetDefaultConfig()
      {
         EmrAppConfig defconfig = null;
         bool isFirstRole = true;
         foreach (EmrAppConfig config in _configs)
         {
            if (config.Plevel == ConfigParamLevel.User)
            {
               defconfig = config;
               break;
            }
            if (config.Plevel == ConfigParamLevel.System) defconfig = config;
            if (config.Plevel == ConfigParamLevel.Role && isFirstRole)
            {
               defconfig = config;
               isFirstRole = false;
            }
         }
         return defconfig;
      }
   }

   /// <summary>
   /// ���ò�������
   /// </summary>
   public enum ConfigParamType
   {
      /// <summary>
      /// Variant(Object)
      /// </summary>
      Var = 0,

      /// <summary>
      /// String
      /// </summary>
      String = 1,

      /// <summary>
      /// Int
      /// </summary>
      Integer = 2,

      /// <summary>
      /// Double
      /// </summary>
      Double = 3,

      /// <summary>
      /// Bool
      /// </summary>
      Boolean = 4,

      /// <summary>
      /// Xml
      /// </summary>
      Xml = 5,

      /// <summary>
      /// Color
      /// </summary>
      Color = 6,

      /// <summary>
      /// Set
      /// </summary>
      Set = 7,
   }

   /// <summary>
   /// ���ò���ѡ����
   /// </summary>
   public enum ConfigChoiceType
   {
      /// <summary>
      /// ��ѡ����
      /// </summary>
      None = 0,

      /// <summary>
      /// ��ѡ��
      /// </summary>
      Single = 1,

      /// <summary>
      /// ��ѡ��
      /// </summary>
      Multi = 2,
   }

   /// <summary>
   /// ���ü���
   /// </summary>
   public enum ConfigParamLevel
   {
      /// <summary>
      /// ϵͳ��
      /// </summary>
      System = 0,

      /// <summary>
      /// ��λ��
      /// </summary>
      Role = 1,

      /// <summary>
      /// �û���
      /// </summary>
      User = 2,
   }

   /// <summary>
   /// ���ö�ȡ�ӿ�
   /// </summary>
   public interface IAppConfigReader
   {
      /// <summary>
      /// ȡ�����ò����ֵ�
      /// </summary>
      /// <param name="keys"></param>
      /// <returns></returns>
      Dictionary<string, EmrAppConfig> GetConfigs(params string[] keys);

      /// <summary>
      /// ȡ��ָ���û����ò����ֵ�
      /// </summary>
      /// <param name="userId"></param>
      /// <param name="keys"></param>
      /// <returns></returns>
      Dictionary<string, EmrUserConfig> GetConfigs(string userId, params string[] keys);

      /// <summary>
      ///  ȡ��ָ���ĵ�һ����
      /// </summary>
      /// <param name="key"></param>
      /// <returns></returns>
      EmrAppConfig GetConfig(string key);

      /// <summary>
      /// ȡ��ָ���û��ĵ�һ����(�����ж��,ϵͳĬ��,��λĬ�ϵ�)
      /// </summary>
      /// <param name="userId"></param>
      /// <param name="key"></param>
      /// <returns></returns>
      EmrUserConfig GetConfig(string userId, string key);

      /// <summary>
      /// ȡ��ָ�������ö���
      /// </summary>
      /// <param name="key"></param>
      /// <returns></returns>
      object GetConfigObj(string key);

      /// <summary>
      /// ȡ��ָ���û������ö���
      /// </summary>
      /// <param name="userId"></param>
      /// <param name="key"></param>
      /// <returns></returns>
      object GetConfigObj(string userId, string key);
   }
}
