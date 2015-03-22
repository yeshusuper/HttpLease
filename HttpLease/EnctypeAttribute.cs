using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public abstract class EnctypeAttribute : Attribute
    {
        internal abstract string ContentType { get; }
        internal abstract bool DefaultEncodeKey { get; }
        internal abstract bool DefaultEncodeValue { get; }
    }
}
