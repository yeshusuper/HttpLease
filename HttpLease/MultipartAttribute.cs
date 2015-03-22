using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MultipartAttribute : EnctypeAttribute
    {
        internal override string ContentType
        {
            get { return "multipart/form-data"; }
        }

        internal override bool DefaultEncodeKey
        {
            get { return false; }
        }

        internal override bool DefaultEncodeValue
        {
            get { return false; }
        }
    }
}
