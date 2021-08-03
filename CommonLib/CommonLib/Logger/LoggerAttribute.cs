using Mono.Cecil;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace LoggerSpace
{

    public class LoggerAttribute : CecilAction
    {

        public static void OnActionBefore(MethodBase mbBase, object[] args)
        {
            for (int i = 0; i < args.Length; i++)
            {
                Console.WriteLine(string.Format("{0}方法，第{1}参数是：{2}", mbBase.Name, i, args[i]));
            }
        }
    }

    public class CecilAction : Attribute
    {
        //public static string appPath;
        //public CecilAction()
        //{
        //    appPath = AppDomain.CurrentDomain.BaseDirectory;
        //}
        //AssemblyDefinition assembiy = AssemblyDefinition.ReadAssembly(appPath);
        //var method = assembiy.MainModule
        //.Types.FirstOrDefault(t => t.Name == "Program")
        //.Methods.FirstOrDefault(m => m.Name == "Main");
        //var worker = method.Body.GetILProcessor();//Get IL

    }
}
