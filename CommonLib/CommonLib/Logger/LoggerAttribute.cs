using System;

namespace LoggerSpace
{

    public class LoggerAttribute : ContextBoundObject
    {

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
