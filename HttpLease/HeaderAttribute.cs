using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    /// <summary>
    /// 添加header参数
    /// </summary>
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public sealed class HeaderAttribute : BaseParameterAttribute
    {
    }
}
