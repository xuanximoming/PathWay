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

using System.Text.RegularExpressions;
using System.Collections.Generic;
using System.Collections;
using System.Reflection;
using System.Reflection.Emit;

namespace YidanEHRApplication
{
    public static class DataSourceCreator
    {
        private static readonly Regex PropertNameRegex =
               new Regex(@"^[A-Za-z]+[A-Za-z0-9_]*$", RegexOptions.Singleline);
        /// <summary>
        /// 表示把迭代字典转化成对象数组的方法
        /// </summary>
        /// <param name="list">迭代列表</param>
        /// <returns></returns>
        public static object[] ToDataSource(this IEnumerable<IDictionary> list)
        {
            IDictionary firstDict = null;
            bool hasData = false;
            //迭代列表的一行数据
            foreach (IDictionary currentDict in list)
            {
                hasData = true;
                firstDict = currentDict;
                break;
            }
            if (!hasData)
            {
                return new object[] { };
            }
            if (firstDict == null)
            {
                throw new ArgumentException("IDictionary entry cannot be null");
            }
            Type objectType = null;
            TypeBuilder tb = GetTypeBuilder(list.GetHashCode());
            ConstructorBuilder constructor =
                        tb.DefineDefaultConstructor(
                                    MethodAttributes.Public |
                                    MethodAttributes.SpecialName |
                                    MethodAttributes.RTSpecialName);
            foreach (DictionaryEntry pair in firstDict)
            {
                if (PropertNameRegex.IsMatch(Convert.ToString(pair.Key), 0))
                {
                    CreateProperty(tb,
                                    Convert.ToString(pair.Key),
                                    pair.Value == null ?
                                                typeof(object) :
                                                pair.Value.GetType());
                }
                else
                {
                    throw new ArgumentException(
                                @"Each key of IDictionary must be 
                                 alphanumeric and start with character.");
                }
            }
            objectType = tb.CreateType();
            return GenerateArray(objectType, list, firstDict);
        }
        /// <summary>
        /// 表示把迭代字典中的数据填充到动态创建的类中
        /// </summary>
        /// <param name="objectType">动态类</param>
        /// <param name="list">迭代字典</param>
        /// <param name="firstDict">迭代字典的第一行</param>
        /// <returns></returns>
        private static object[] GenerateArray(Type objectType, IEnumerable<IDictionary> list, IDictionary firstDict)
        {
            var itemsSource = new List<object>();
            foreach (var currentDict in list)
            {
                if (currentDict == null)
                {
                    throw new ArgumentException("IDictionary entry cannot be null");
                }
                object row = Activator.CreateInstance(objectType);
                foreach (DictionaryEntry pair in firstDict)
                {
                    if (currentDict.Contains(pair.Key))
                    {
                        PropertyInfo property =
                            objectType.GetProperty(Convert.ToString(pair.Key));
                        property.SetValue(
                            row,
                            Convert.ChangeType(
                                    currentDict[pair.Key],
                                    property.PropertyType,
                                    null),
                            null);
                    }
                }
                itemsSource.Add(row);
            }
            return itemsSource.ToArray();
        }
        /// <summary>
        /// 表示在运行时定义并创建类的新实例。
        /// </summary>
        /// <param name="code">程序集和类的唯一标识的后缀</param>
        /// <returns></returns>
        private static TypeBuilder GetTypeBuilder(int code)
        {
            AssemblyName an = new AssemblyName("TempAssembly" + code);
            AssemblyBuilder assemblyBuilder =
                AppDomain.CurrentDomain.DefineDynamicAssembly(
                    an, AssemblyBuilderAccess.Run);
            ModuleBuilder moduleBuilder = assemblyBuilder.DefineDynamicModule("MainModule");
            TypeBuilder tb = moduleBuilder.DefineType("TempType" + code
                                , TypeAttributes.Public |
                                TypeAttributes.Class |
                                TypeAttributes.AutoClass |
                                TypeAttributes.AnsiClass |
                                TypeAttributes.BeforeFieldInit |
                                TypeAttributes.AutoLayout
                                , typeof(object));
            return tb;
        }
        /// <summary>
        /// 表示动态创建属性的方法
        /// </summary>
        /// <param name="tb">类实例</param>
        /// <param name="propertyName">属性名</param>
        /// <param name="propertyType">属性类型</param>
        private static void CreateProperty(TypeBuilder tb, string propertyName, Type propertyType)
        {
            FieldBuilder fieldBuilder = tb.DefineField("_" + propertyName,
                                                        propertyType,
                                                        FieldAttributes.Private);

            PropertyBuilder propertyBuilder =
                tb.DefineProperty(
                    propertyName, PropertyAttributes.HasDefault, propertyType, null);
            MethodBuilder getPropMthdBldr =
                tb.DefineMethod("get_" + propertyName,
                    MethodAttributes.Public |
                    MethodAttributes.SpecialName |
                    MethodAttributes.HideBySig,
                    propertyType, Type.EmptyTypes);
            ILGenerator getIL = getPropMthdBldr.GetILGenerator();
            getIL.Emit(OpCodes.Ldarg_0);
            getIL.Emit(OpCodes.Ldfld, fieldBuilder);
            getIL.Emit(OpCodes.Ret);
            MethodBuilder setPropMthdBldr =
                tb.DefineMethod("set_" + propertyName,
                  MethodAttributes.Public |
                  MethodAttributes.SpecialName |
                  MethodAttributes.HideBySig,
                  null, new Type[] { propertyType });
            ILGenerator setIL = setPropMthdBldr.GetILGenerator();
            setIL.Emit(OpCodes.Ldarg_0);
            setIL.Emit(OpCodes.Ldarg_1);
            setIL.Emit(OpCodes.Stfld, fieldBuilder);
            setIL.Emit(OpCodes.Ret);
            propertyBuilder.SetGetMethod(getPropMthdBldr);
            propertyBuilder.SetSetMethod(setPropMthdBldr);
        }
    }
}
