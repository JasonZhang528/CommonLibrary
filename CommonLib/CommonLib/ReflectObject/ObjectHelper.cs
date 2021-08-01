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
        public static Dictionary<string, object> ConvertAttrToDic(object obj)
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
        /// 复制对象集合的属性值=>目标对象
        /// </summary>
        /// <param name="targetObj">目标对象</param>
        /// <param name="isCaseSensitive">是否区分大小写</param>
        /// <param name="originObjs">对象集合（被复制属性值的对象集合）</param>
        /// <returns></returns>
        public static bool CopyPropertyValue(object targetObj, bool isCaseSensitive = false, params object[] originObjs)
        {
            if (originObjs.Length == 0) return false;
            try
            {
                PropertyInfo[] propInfos = targetObj?.GetType().GetProperties();
                for (int i = 0; i < propInfos.Length; i++)
                {
                    for (int j = 0; j < originObjs.Length; j++)
                    {
                        PropertyInfo[] originPorpInfos = originObjs[j].GetType().GetProperties();
                        PropertyInfo propInfo = isCaseSensitive ? originPorpInfos.FirstOrDefault(q => q.Name == propInfos[i].Name) :
                                                                  originPorpInfos.FirstOrDefault(q => q.Name.ToLower() == propInfos[i].Name.ToLower());
                        if (propInfo == null) continue;
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
        /// 复制对象集合的属性值=>目标对象
        /// </summary>
        /// <param name="objectFullName">类的完全限定名（即包括命名空间）</param>
        /// <param name="isCaseSensitive">是否区分大小写</param>
        /// <param name="originObjs">对象集合（被复制属性值的对象集合）</param>
        /// <returns></returns>
        public static bool CopyPropertyValue(string objectFullName, bool isCaseSensitive = false, params object[] originObjs)
        {
            object targetObj = Assembly.GetExecutingAssembly().CreateInstance(objectFullName);
            if (targetObj == null) return false;
            return CopyPropertyValue(targetObj, isCaseSensitive, originObjs);
        }
    }
}
