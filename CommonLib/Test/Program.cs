using CommonLib.Models;
using CommonLib.ReflectObject;
using CommonLib.Win32Api;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Test
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var rtemp = Win32ApiHelper.IsExistWindow("微信", null);
            int isSucceed = Win32ApiHelper.ShowWindowMinimized(rtemp);


            Console.ReadKey();
        }
    }
}
