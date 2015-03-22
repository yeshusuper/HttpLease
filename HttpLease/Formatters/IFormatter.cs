using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease.Formatters
{
    public interface IFormatter
    {
        IDictionary<string, string> GetRequestParameters(string key, object value);
    }
}
