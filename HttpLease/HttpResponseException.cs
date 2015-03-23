using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    public class HttpResponseException : Exception
    {
        public System.Net.HttpStatusCode HttpStatusCode { get; private set; }

        internal HttpResponseException(System.Net.HttpStatusCode statusCode, string message)
            : base(message)
        {
            this.HttpStatusCode = statusCode;
        }
    }
}
