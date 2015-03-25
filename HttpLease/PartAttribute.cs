using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class PartAttribute : BaseParameterAttribute
    {
        /// <summary>
        /// 默认true
        /// </summary>
        public bool IsFile { get; set; }

        public PartAttribute()
        {
            IsFile = true;
        }
    }
}
