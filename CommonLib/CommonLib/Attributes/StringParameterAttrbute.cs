using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLibrary.Attributes
{
    [AttributeUsage(AttributeTargets.Method)]
    public class StringParameterAttrbute : Attribute
    {
        /// <summary>
        /// 输入字符串是否为空
        /// </summary>
        public bool IsEmpty { get; private set; }

        /// <summary>
        /// 需要判定的字符串
        /// </summary>
        public string Parameter { get; private set; }

        public StringParameterAttrbute(string param)
        {
            Parameter = param;
        }


    }
}
