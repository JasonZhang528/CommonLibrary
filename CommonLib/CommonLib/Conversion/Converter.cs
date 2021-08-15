using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CommonLib.Conversion
{
    /// <summary>
    /// 转换器
    /// </summary>
    public class Converter
    {
        /// <summary>
        /// Stream => String
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string StreamToString(Stream stream)
        {
            string result = null;
            if (stream == null) return result;
            if (!stream.CanRead) return result;
            using (StreamReader sw = new StreamReader(stream))
            {
                result = sw.ReadToEnd();
            }
            return result;
        }
    }

    /// <summary>
    /// 转换器扩展方法
    /// </summary>
    public static class ConvertMethods
    {
        /// <summary>
        /// 转字符串
        /// </summary>
        /// <param name="stream"></param>
        /// <returns></returns>
        public static string ToString(this Stream stream) => Converter.StreamToString(stream);

        /// <summary>
        /// 转Json字符串
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string ToJsonString(this object obj) => JsonConvert.SerializeObject(obj);

        /// <summary>
        /// Json字符串反序列化
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static T JsonToObject<T>(this string jsonStr) => (T)JsonConvert.DeserializeObject(jsonStr);
    }
}
