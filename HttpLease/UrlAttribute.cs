using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class UrlAttribute : Attribute
    {
        public string Url { get; private set; }

        /// <summary>
        /// 可以为绝对地址，相对地址，和需要参数组合的地址，如：/Api/User/{uid}
        /// </summary>
        /// <param name="url"></param>
        public UrlAttribute(string url)
        {
            this.Url = url;
        }
    }
}
