using HttpLease.Proxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease
{
    public static class HttpLease
    {
        public static T Get<T>(IConfig config)
            where T : class
        {
            return new HttpLeaseProxy<T>(config).Client;
        }

        public static T Get<T>()
            where T : class
        {
            return Get<T>(GlobalConfig.Config);
        }

        public static object Get(Type type, IConfig config)
        {
            return new HttpLeaseProxy(type, config).Client;
        }
        public static object Get(Type type)
        {
            return Get(type, GlobalConfig.Config);
        }
    }
}
