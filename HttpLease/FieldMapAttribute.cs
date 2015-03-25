using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class FieldMapAttribute : ParameterAttribute
    {
        /// <summary>
        /// 不设置时跟随默认设置
        /// </summary>
        public bool? IsEncodeKey { get; set; }
        /// <summary>
        /// 不设置时跟随默认设置
        /// </summary>
        public bool? IsEncodeValue { get; set; }
    }
}
