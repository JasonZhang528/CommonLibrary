using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.WebService
{
    /// <summary>
    /// 动态调用WebService
    /// 适用于不能直接访问的
    /// </summary>
    public class WebServiceHelper
    {
        private static WebServiceHelper wsHelper;

        private WebServiceHelper()
        {

        }

        /// <summary>
        /// 获取WebServiceHelper实例
        /// </summary>
        /// <returns></returns>
        public static WebServiceHelper GetInstance() => wsHelper ?? new();

        /// < summary>
        /// 动态调用web服务
        /// </summary>
        /// < param name="url">WSDL服务地址</param>
        /// < param name="classname">类名</param>
        /// < param name="methodname">方法名</param>
        /// < param name="args">参数</param>
        /// < returns></returns>
        public void InvokeWebService(string url, string methodname, params object[] args)
        {
            url = CheckURL(url);
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(new Uri(url));

            HttpWebResponse webResponse = null;
            switch (requestMode)
            {
                case ERequestMode.Get:
                    webResponse = GetRequest(webRequest, timeout);
                    break;
                case ERequestMode.Post:
                    webResponse = PostRequest(webRequest, parameters, timeout, requestCoding);
                    break;
            }

            if (webResponse != null && webResponse.StatusCode == HttpStatusCode.OK)
            {
                using (Stream newStream = webResponse.GetResponseStream())
                {
                    if (newStream != null)
                        using (StreamReader reader = new StreamReader(newStream, responseCoding))
                        {
                            string result = reader.ReadToEnd();
                            return result;
                        }
                }
            }
            return null;
        }

        /// <summary>
        /// 检查URL路径
        /// </summary>
        /// <param name="url"></param>
        /// <returns></returns>
        private string CheckURL(string url)
        {
            if (string.IsNullOrWhiteSpace(url))
                throw new ArgumentNullException("URL地址不可以为空！");
            if (url.StartsWith("http://", StringComparison.CurrentCultureIgnoreCase))
                return url;
            return string.Format("http://{0}", url);
        }

        /// <summary>
        /// get 请求指定地址返回响应数据
        /// </summary>
        /// <param name="webRequest">请求</param>
        /// <param name="timeout">请求超时时间（毫秒）</param>
        /// <returns>返回：响应信息</returns>
        private HttpWebResponse GetRequest(HttpWebRequest webRequest, int timeout)
        {
            try
            {
                webRequest.Accept = "text/html, application/xhtml+xml, application/json, text/javascript, */*; q=0.01";
                webRequest.Headers.Add("Accept-Language", "zh-cn,en-US,en;q=0.5");
                webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.UserAgent = "DefaultUserAgent";
                webRequest.Timeout = timeout;
                webRequest.Method = "GET";

                // 接收返回信息
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                return webResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }


        /// <summary>
        /// post 请求指定地址返回响应数据
        /// </summary>
        /// <param name="webRequest">请求</param>
        /// <param name="parameters">传入参数</param>
        /// <param name="timeout">请求超时时间（毫秒）</param>
        /// <param name="requestCoding">请求编码</param>
        /// <returns>返回：响应信息</returns>
        private HttpWebResponse PostRequest(HttpWebRequest webRequest, Dictionary<string, string> parameters, int timeout, Encoding requestCoding)
        {
            try
            {
                // 拼接参数
                string postStr = string.Empty;
                if (parameters != null)
                {
                    parameters.All(o =>
                    {
                        if (string.IsNullOrEmpty(postStr))
                            postStr = string.Format("{0}={1}", o.Key, o.Value);
                        else
                            postStr += string.Format("&{0}={1}", o.Key, o.Value);

                        return true;
                    });
                }

                byte[] byteArray = requestCoding.GetBytes(postStr);
                webRequest.Accept = "text/html, application/xhtml+xml, application/json, text/javascript, */*; q=0.01";
                webRequest.Headers.Add("Accept-Language", "zh-cn,en-US,en;q=0.5");
                webRequest.Headers.Add("Cache-Control", "no-cache");
                webRequest.UserAgent = "DefaultUserAgent";
                webRequest.Timeout = timeout;
                webRequest.ContentType = "application/x-www-form-urlencoded";
                webRequest.ContentLength = byteArray.Length;
                webRequest.Method = "POST";

                // 将参数写入流
                using (Stream newStream = webRequest.GetRequestStream())
                {
                    newStream.Write(byteArray, 0, byteArray.Length);
                    newStream.Close();
                }

                // 接收返回信息
                HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();
                return webResponse;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }

    /// <summary>
    /// 方法请求方式
    /// </summary>
    public enum RequestMethod
    {
        GET, POST
    }
}
