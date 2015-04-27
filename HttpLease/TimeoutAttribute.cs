using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    [AttributeUsage(AttributeTargets.Method, AllowMultiple = false)]
    public class TimeoutAttribute : Attribute
    {
        internal int Timeout { get; private set; }

        public TimeoutAttribute(int timeout)
        {
            if (timeout <= 0)
                throw new Exception("timeout must > 0");
            Timeout = timeout;
        }
    }
}
