using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.ReflectObject
{
    /// <summary>
    /// 对象辅助类-单例模式
    /// </summary>
    public class ObjectHelper
    {
        private static ObjectHelper helper = null;

        /// <summary>
        /// Private Constructor
        /// </summary>
        private ObjectHelper()
        {

        }

        /// <summary>
        /// 获取Object辅助工具的实例对象
        /// </summary>
        /// <returns></returns>
        public static ObjectHelper GetInstance() => helper ??= new();

        #region 属性操作

        /// <summary>
        /// 属性&值=>字典
        /// </summary>
        /// <param name="obj">实例对象（包含属性）</param>
        /// <returns>属性名-值的字典</returns>
        public Dictionary<string, object> ConvertAttrToCollection(object obj)
        {
            Type type = obj.GetType();
            if (!type.GetTypeInfo().IsClass)
            {
                throw new Exception($"{obj}不是一个实例化对象");
            }
            Dictionary<string, object> dic = new Dictionary<string, object>();
            PropertyInfo[] propertyInfos = type.GetProperties();
            foreach (var item in propertyInfos)
            {
                object value = item.GetValue(obj, null);
                dic.Add(item.Name, value);
            }
            return dic;
        }

        /// <summary>
        /// 字典转实体类
        /// </summary>
        /// <typeparam name="T">泛型类</typeparam>
        /// <param name="dict">字典</param>
        /// <returns></returns>
        public T ParseDictionaryToObject<T>(Dictionary<string, object> dict)
        {
            T obj = Activator.CreateInstance<T>();
            Type type = obj.GetType();
            foreach (KeyValuePair<string, object> item in dict)
            {
                PropertyInfo propInfo = type.GetProperty(item.Key);
                if (propInfo == null) continue;
                object value = Convert.ChangeType(item.Value, item.Value.GetType());
                propInfo.SetValue(obj, value, null);
            }
            return obj;
        }

        /// <summary>
        /// 复制对象集合的属性值=>目标对象
        /// </summary>
        /// <param name="targetObj">目标对象</param>
        /// <param name="ignoreProperty">忽略属性名（不需要赋值,可空）</param>
        /// <param name="isCaseSensitive">属性名是否区分大小写</param>
        /// <param name="originObjs">对象集合（被复制属性值的对象集合）</param>
        /// <returns></returns>
        public bool CopyPropertyValue(object targetObj, string[] ignoreProperty, bool isCaseSensitive = false, params object[] originObjs)
        {
            if (originObjs.Length == 0) return false;
            ignoreProperty = ignoreProperty ?? new string[] { };
            try
            {
                PropertyInfo[] propInfos = targetObj?.GetType().GetProperties();
                for (int i = 0; i < propInfos.Length; i++)
                {
                    for (int j = 0; j < originObjs?.Length; j++)
                    {
                        PropertyInfo[] originPorpInfos = originObjs[j].GetType().GetProperties();
                        PropertyInfo propInfo = isCaseSensitive ? originPorpInfos.FirstOrDefault(q => q.Name == propInfos[i].Name) :
                                                                  originPorpInfos.FirstOrDefault(q => q.Name.ToLower() == propInfos[i].Name.ToLower());
                        if (propInfo == null) continue;
                        if (ignoreProperty.Contains(propInfo.Name)) continue;
                        if (propInfos[i].PropertyType != propInfo.PropertyType) continue;
                        object value = propInfo.GetValue(originObjs[j], null);
                        propInfos[i].SetValue(targetObj, value);
                    }
                }
                return true;
            }
            catch (Exception)
            {
                throw;
            }
        }

        /// <summary>
        /// 创建实例对象并复制其它对象的属性值
        /// </summary>
        /// <param name="objectFullName">类的完全限定名（即包括命名空间）</param>
        /// <param name="ignoreProperty">忽略属性名（不需要赋值,可空）</param>
        /// <param name="isCaseSensitive">属性名是否区分大小写</param>
        /// <param name="originObjs">对象集合（被复制属性值的对象集合）</param>
        /// <returns></returns>
        public T CopyPropertyValue<T>(string objectFullName, string[] ignoreProperty, bool isCaseSensitive = false, params object[] originObjs)
        {
            Type type = Type.GetType(objectFullName);
            object targetObj = Activator.CreateInstance(type);
            CopyPropertyValue(targetObj, ignoreProperty, isCaseSensitive, originObjs);
            return (T)targetObj;
        }

        /// <summary>
        /// 判断对象属性是否包含目标值（value为字符串=>模糊查询）
        /// </summary>
        /// <param name="obj">实例对象</param>
        /// <param name="value">目标值</param>
        /// <param name="excludeFields">排除检查的字段</param>
        /// <returns>true=>实例对象存在包含目标值的属性；false=>不存在</returns>
        public bool ObjectHasAttrValue(object obj, object value, params string[] excludeFields)
        {
            Type type = obj.GetType();
            if (!type.GetTypeInfo().IsClass)
            {
                throw new Exception($"{obj}不是一个实例化对象");
            }
            Type valueType = value.GetType();
            PropertyInfo[] propInfos = type.GetProperties(BindingFlags.NonPublic | BindingFlags.Public);
            foreach (var propInfo in propInfos)
            {
                if (propInfo.PropertyType != valueType) continue;
                if (excludeFields != null && excludeFields.Contains(propInfo.Name)) continue;
                object attrValue = propInfo.GetValue(obj, null);
                if (attrValue == null) continue;
                if (valueType == typeof(string))
                {
                    if (attrValue.ToString().Contains(value.ToString())) return true;
                }
                else
                {
                    if (attrValue == value) return true;
                }
            }
            return false;
        }

        #endregion

        #region 反射方式调用方法
        /*
         * Activator方式创建实例性能最好
         */

        /// <summary>
        /// 反射方式调用方法
        /// </summary>
        /// <param name="classFullName">类完全限定名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="parameters">方法参数</param>
        /// <returns></returns>
        public object InvokeMethod(string classFullName, string methodName, params object[] parameters)
        {
            // 1.Load(命名空间名称)，GetType(命名空间.类名)
            string namespaceName = classFullName.Substring(0, classFullName.LastIndexOf('.'));
            Type type = Assembly.Load(namespaceName).GetType(classFullName);
            //2.GetMethod(需要调用的方法名称)
            MethodInfo methodInfo = type.GetMethod(methodName);
            object result = null;
            if (methodInfo.IsStatic)
            {
                //3.静态方法调用
                result = methodInfo.Invoke(null, parameters);
            }
            else
            {
                //3.调用的实例化方法（非静态方法）需要创建类型的一个实例
                object obj = Activator.CreateInstance(type);
                //4.方法需要传入的参数
                //5.调用方法，如果调用的是一个静态方法，就不需要第3步（创建类型的实例）
                result = methodInfo.Invoke(obj, parameters);
            }
            return result;
        }

        /// <summary>
        /// 反射方式调用DLL中方法
        /// </summary>
        /// <param name="dllPath">dll文件全路径</param>
        /// <param name="classFullName">类完全限定名</param>
        /// <param name="methodName">方法名</param>
        /// <param name="parameters">方法参数</param>
        /// <returns></returns>
        public object InvokeMethod(string dllPath, string classFullName, string methodName, params object[] parameters)
        {
            object result = null;
            if (!File.Exists(dllPath)) return result;
            // 1.Load(命名空间名称)，GetType(命名空间.类名)
            Type type = Assembly.LoadFile(dllPath).GetType(classFullName);
            //2.GetMethod(需要调用的方法名称)
            MethodInfo methodInfo = type.GetMethod(methodName);
            if (methodInfo.IsStatic)
            {
                //3.静态方法调用
                result = methodInfo.Invoke(null, parameters);
            }
            else
            {
                //3.调用的实例化方法（非静态方法）需要创建类型的一个实例
                object obj = Activator.CreateInstance(type);
                //4.方法需要传入的参数
                //5.调用方法，如果调用的是一个静态方法，就不需要第3步（创建类型的实例）
                result = methodInfo.Invoke(obj, parameters);
            }
            return result;
        }

        #endregion

        /// <summary>
        /// 字典=>实例对象（属性不显示）
        /// </summary>
        /// <param name="propertyValueDic"></param>
        /// <returns></returns>
        public object DynamicCreateObject(Dictionary<string, object> propertyValueDic)
        {
            dynamic dynamicObj = new System.Dynamic.ExpandoObject();
            var collection = dynamicObj as ICollection<KeyValuePair<string, object>>;
            foreach (var item in propertyValueDic)
            {
                var property = new KeyValuePair<string, object>(item.Key, item.Value);
                collection.Add(property);
            }
            return dynamicObj;
        }
    }
}
