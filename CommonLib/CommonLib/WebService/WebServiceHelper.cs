using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Net;

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
        /// < param name="methodName">方法名</param>
        /// < param name="args">参数</param>
        /// < returns></returns>
        public void InvokeWebService(string url, string methodName, RequestMethod methodType, params object[] args)
        {
            string @namespace = "EnterpriseServerBase.WebService.DynamicWebCalling";
            methodName = string.IsNullOrWhiteSpace(methodName) ? wsHelper.GetWsClassName(url) : methodName;

            try
            {                   
                //获取WSDL   
                WebClient wc = new WebClient();
                Stream stream = wc.OpenRead(url + "?WSDL");
                ServiceDescription sd = ServiceDescription.Read(stream);
                ServiceDescriptionImporter sdi = new ServiceDescriptionImporter();
                sdi.AddServiceDescription(sd, "", "");
                CodeNamespace cn = new CodeNamespace(@namespace);
                //生成客户端代理类代码          
                CodeCompileUnit ccu = new CodeCompileUnit();
                ccu.Namespaces.Add(cn);
                sdi.Import(cn, ccu);
                CSharpCodeProvider icc = new CSharpCodeProvider();
                //设定编译参数                 
                CompilerParameters cplist = new CompilerParameters();
                cplist.GenerateExecutable = false;
                cplist.GenerateInMemory = true;
                cplist.ReferencedAssemblies.Add("System.dll");
                cplist.ReferencedAssemblies.Add("System.XML.dll");
                cplist.ReferencedAssemblies.Add("System.Web.Services.dll");
                cplist.ReferencedAssemblies.Add("System.Data.dll");
                //编译代理类                 
                CompilerResults cr = icc.CompileAssemblyFromDom(cplist, ccu);
                if (true == cr.Errors.HasErrors)
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    foreach (System.CodeDom.Compiler.CompilerError ce in cr.Errors)
                    {
                        sb.Append(ce.ToString());
                        sb.Append(System.Environment.NewLine);
                    }
                    throw new Exception(sb.ToString());
                }
                //生成代理实例，并调用方法   
                System.Reflection.Assembly assembly = cr.CompiledAssembly;
                Type t = assembly.GetType(@namespace + "." + classname, true, true);
                object obj = Activator.CreateInstance(t);
                System.Reflection.MethodInfo mi = t.GetMethod(methodName);
                return mi.Invoke(obj, args);
                // PropertyInfo propertyInfo = type.GetProperty(propertyname);     
                //return propertyInfo.GetValue(obj, null); 
            }
            catch (Exception ex)
            {
                throw new Exception(ex.InnerException.Message, new Exception(ex.InnerException.StackTrace));
            }
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

        private string GetWsClassName(string wsUrl)
        {
            string[] parts = wsUrl.Split('/');
            string[] pps = parts[parts.Length - 1].Split('.');
            return pps[0];
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
