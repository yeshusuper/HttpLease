using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class MultipartAttribute : EnctypeAttribute
    {
        public const string MultipartContentType = "multipart/form-data";

        internal override string ContentType
        {
            get { return MultipartContentType; }
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
