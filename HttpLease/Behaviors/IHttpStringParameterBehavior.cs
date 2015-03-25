using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease.Behaviors
{
    internal interface IHttpStringParameterBehavior : IHttpParameterBehavior
    {
        bool IsEncodeKey { get; }
        bool IsEncodeValue { get; }
        Encoding Encoding { get; }
        Formatters.IFormatter Formatter { get; }
        IDictionary<string, string[]> GetRequestParameters(object[] args);
        string GetRequestString(object[] args);
    }

    internal class HttpParameterBehavior : IHttpStringParameterBehavior
    {
        public bool IsEncodeKey { get; set; }

        public bool IsEncodeValue { get; set; }

        public Encoding Encoding { get; set; }

        public string Key { get; private set; }
        public int ArgIndex { get; private set; }
        public Formatters.IFormatter Formatter { get; private set; }

        public HttpParameterBehavior(string key, int argIndex, Encoding encoding, Formatters.IFormatter formatter)
        {
            this.Key = key;
            this.ArgIndex = argIndex;
            this.Encoding = encoding;
            this.Formatter = formatter;
        }

        public virtual string GetRequestString(object[] args)
        {
            var rps = Formatter.GetRequestParameters(Key, args[ArgIndex], Encoding);
            return rps.ToString(IsEncodeKey, IsEncodeValue);
        }


        public IDictionary<string, string[]> GetRequestParameters(object[] args)
        {
            return Formatter.GetRequestParameters(Key, args[ArgIndex], Encoding)._Store;
        }
    }

    internal class FieldMapParameterBehavior : IHttpStringParameterBehavior
    {
        public bool IsEncodeKey { get; set; }

        public bool IsEncodeValue { get; set; }
        public Encoding Encoding { get; private set; }
        public int ArgIndex { get; private set; }
        public string Key { get; private set; }
        public Formatters.IFormatter Formatter { get; private set; }

        public FieldMapParameterBehavior(string key, int argIndex, Encoding encoding, Formatters.IFormatter formatter)
        {
            this.Key = key;
            this.ArgIndex = argIndex;
            this.Encoding = encoding;
            this.Formatter = formatter;
        }

        public virtual string GetRequestString(object[] args)
        {
            var rps = Formatter.GetRequestParameters(args[ArgIndex], Encoding);
            return rps.ToString(IsEncodeKey, IsEncodeValue);
        }

        public IDictionary<string, string[]> GetRequestParameters(object[] args)
        {
            return Formatter.GetRequestParameters(args[ArgIndex], Encoding)._Store;
        }
    }
}
