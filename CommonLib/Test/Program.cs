﻿using CommonLib.Models;
using CommonLib.ReflectObject;
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

            Dictionary<string, object> dic = new Dictionary<string, object>();
            dic.Add("Name", "jason");
            dic.Add("ID", "123");
            dic.Add("Password", "4556");
            //var obj = ObjectHelper.DynamicCreateObject(dic);
            //string jon = JsonConvert.SerializeObject(obj);

            DynamicModel obj = new DynamicModel(dic);
            
            Console.ReadKey();
        }
    }
}
