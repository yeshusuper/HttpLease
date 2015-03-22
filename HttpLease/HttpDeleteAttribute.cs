using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class HttpDeleteAttribute : HttpMethodAttribute
    {
        internal override MethodKind Method
        {
            get { return MethodKind.DELETE; }
        }
    }
}
