using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease.Proxy
{
    internal interface IHttpLeaseProxy<T>
        where T : class
    {
        T Client { get; }
    }

    
}
