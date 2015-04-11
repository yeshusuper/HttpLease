using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease.Behaviors
{
    public interface IHttpBodyBehavior
    {
        int ArgIndex { get; }
        string GetRequestString(object[] args);
    }

    class HttpBodyBehavior : IHttpBodyBehavior
    {
        public string GetRequestString(object[] args)
        {
            var arg = args[ArgIndex];
            return arg == null ? String.Empty : arg.ToString();
        }

        public int ArgIndex { get; private set; }

        public HttpBodyBehavior(int argIndex)
        {
            this.ArgIndex = argIndex;
        }
    }

}
