using CommonLib.Encryption;
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

            var encryptor = Encryptor.GetInstance();
            encryptor.DesKey = "abcdefgh";
            string ss = encryptor.DesEncrypt("TestText");
            string s1s = encryptor.DesDecrypt(ss);
            Console.ReadKey();
        }
    }
}
