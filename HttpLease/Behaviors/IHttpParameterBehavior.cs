using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease.Behaviors
{
    internal interface IHttpParameterBehavior
    {
        bool IsEncodeKey { get; }
        bool IsEncodeValue { get; }
        string Key { get; }
        int ArgIndex { get; }
        Formatters.IFormatter Formatter { get; }
        string GetRequestString(object[] args, Encoding encoding);
    }

    internal class HttpParameterBehavior : IHttpParameterBehavior
    {
        public bool IsEncodeKey { get; set; }

        public bool IsEncodeValue { get; set; }

        public string Key { get; private set; }
        public int ArgIndex { get; private set; }
        public Formatters.IFormatter Formatter { get; private set; }

        public HttpParameterBehavior(string key, int argIndex, Formatters.IFormatter formatter)
        {
            this.Key = key;
            this.ArgIndex = argIndex;
            this.Formatter = formatter;
        }

        public virtual string GetRequestString(object[] args, Encoding encoding)
        {
            var rps = Formatter.GetRequestParameters(Key, args[ArgIndex], encoding);
            return rps.ToString(IsEncodeKey, IsEncodeValue);
        }
    }

    internal class FieldMapParameterBehavior : IHttpParameterBehavior
    {
        public bool IsEncodeKey { get; set; }

        public bool IsEncodeValue { get; set; }
        public int ArgIndex { get; private set; }
        public string Key { get; private set; }
        public Formatters.IFormatter Formatter { get; private set; }

        public FieldMapParameterBehavior(string key, int argIndex, Formatters.IFormatter formatter)
        {
            this.Key = key;
            this.ArgIndex = argIndex;
            this.Formatter = formatter;
        }

        public virtual string GetRequestString(object[] args, Encoding encoding)
        {
            var rps = Formatter.GetRequestParameters(args[ArgIndex], encoding);
            return rps.ToString(IsEncodeKey, IsEncodeValue);
        }

    }

}
