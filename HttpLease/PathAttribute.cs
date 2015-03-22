using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = false)]
    public class PathAttribute : BaseParameterAttribute
    {
    }
}
