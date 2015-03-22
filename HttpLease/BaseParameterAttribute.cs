using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class BaseParameterAttribute : Attribute
    {
        /// <summary>
        /// 参数key，如果不设置则为参数名称
        /// </summary>
        public string Key { get; set; }
    }
}
