using CommonLib.Images;
using CommonLib.Models;
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

            //Dictionary<string, object> dic = new Dictionary<string, object>();
            //dic.Add("Name", "jason");
            //dic.Add("ID", "123");
            //dic.Add("Password", "4556");
            ////var obj = ObjectHelper.DynamicCreateObject(dic);
            ////string jon = JsonConvert.SerializeObject(obj);

            //DynamicModel obj = new DynamicModel(dic);

            //var user = ObjectHelper.GetInstance().ParseDictionaryToObject<UserTest>(dic);

            //string className = "Test.UserTest";
            //var temp = ObjectHelper.GetInstance().InvokeMethod(className, "test1", "ss222s", 1);

            STransferParam param = new STransferParam();
            param.pictureInputDir = @"C:\Users\Jason\Pictures\Camera Roll";
            param.audioInputPath = null;
            param.size = "1024x768";
            param.videoOutDir = @"C:\Users\Jason\Pictures\Camera Roll\test.mp4";
            PictureVideoTransfer.GetInstance().TransferByExe(param);

            Console.ReadKey();
        }
    }
}
