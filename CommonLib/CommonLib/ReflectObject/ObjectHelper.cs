using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.ReflectObject
{
    /// <summary>
    /// 对象辅助类
    /// </summary>
    public class ObjectHelper
    {
        /// <summary>
        /// 属性&值=>字典
        /// </summary>
        /// <param name="obj">实例对象（包含属性）</param>
        /// <returns>属性名-值的字典</returns>
        public static Dictionary<string, object> ConvertAttrToCollection(object obj)
        {
            Dictionary<string, object> dic = new Dictionary<string, object>();
            PropertyInfo[] propertyInfos = obj.GetType().GetProperties();
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
        public static T ParseDictionaryToObject<T>(Dictionary<string, object> dict)
        {
            T obj = Activator.CreateInstance<T>();
            foreach (KeyValuePair<string, object> item in dict)
            {
                PropertyInfo propInfo = obj.GetType().GetProperty(item.Key);
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
        public static bool CopyPropertyValue(object targetObj, string[] ignoreProperty, bool isCaseSensitive = false, params object[] originObjs)
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
            catch (Exception ex)
            {
                throw ex;
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
        public static T CopyPropertyValue<T>(string objectFullName, string[] ignoreProperty, bool isCaseSensitive = false, params object[] originObjs)
        {
            Type type = Type.GetType(objectFullName);
            object targetObj = Activator.CreateInstance(type);
            CopyPropertyValue(targetObj, ignoreProperty, isCaseSensitive, originObjs);
            return (T)targetObj;
        }

        /// <summary>
        /// 字典=>实例对象（属性不显示）
        /// </summary>
        /// <param name="propertyValueDic"></param>
        /// <returns></returns>
        public static object DynamicCreateObject(Dictionary<string, object> propertyValueDic)
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
