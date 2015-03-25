using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public abstract class BaseParameterAttribute : ParameterAttribute
    {
        /// <summary>
        /// 参数key，如果不设置则为参数名称
        /// </summary>
        public string Name { get; set; }
    }

    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public abstract class ParameterAttribute : Attribute
    {

    }
}
