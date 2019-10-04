using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.ServiceModel;
using System.Reflection;
using YidanEHRApplication;

namespace YidanEHRApplication.Helpers
{
    public class ServiceHelper<T> where T : class
    {
        /// <summary>
        /// 根据服务类型新建一个服务实例
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        public static T GetClientInstance(ServiceType serviceType)
        {
            T _instance = null;
            string serviceUri = GetServiceAddress(serviceType);
            if (string.IsNullOrEmpty(serviceUri)) return null;

            object[] paras = new object[2];
            BasicHttpBinding binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxBufferSize = int.MaxValue;
            binding.MaxReceivedMessageSize = int.MaxValue;
            EndpointAddress address = new EndpointAddress(new Uri(serviceUri, UriKind.Absolute));

            paras[0] = binding;
            paras[1] = address;

            ConstructorInfo constructor = null;

            try
            {
                Type[] types = new Type[2];
                types[0] = typeof(System.ServiceModel.Channels.Binding);
                types[1] = typeof(System.ServiceModel.EndpointAddress);
                constructor = typeof(T).GetConstructor(types);
            }
            catch (Exception)
            {
                return null;
            }

            if (constructor != null)
                _instance = (T)constructor.Invoke(paras);

            return _instance;
        }

        /// <summary>
        /// 取得服务地址
        /// </summary>
        /// <param name="serviceType">服务类型</param>
        /// <returns></returns>
        private static string GetServiceAddress(ServiceType serviceType)
        {
            return App.Current.Resources[serviceType.ToString()].ToString();
        }
    }

    /// <summary>
    /// 枚举中的值和web.config中配置的要一样
    /// </summary>
    public enum ServiceType
    {
        YidanEHRDataServicesvc
    }
}
