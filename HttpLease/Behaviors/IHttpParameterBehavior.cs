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
    }

    internal class HttpParameterBehavior : IHttpParameterBehavior
    {
        public bool IsEncodeKey { get; set; }

        public bool IsEncodeValue { get; set; }

        public string Key { get; private set; }
        public int ArgIndex { get; private set; }

        public HttpParameterBehavior(string key, int argIndex)
        {
            this.Key = key;
            this.ArgIndex = argIndex;
        }
    }

}
