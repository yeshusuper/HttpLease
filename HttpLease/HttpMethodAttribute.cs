using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class HttpMethodAttribute : Attribute
    {
        internal abstract MethodKind Method { get; }
    }
}
