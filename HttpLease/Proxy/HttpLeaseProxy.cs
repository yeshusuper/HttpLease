using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HttpLease.Proxy
{
    internal class HttpLeaseProxy : HttpLeaseProxy<object>
    {
        private readonly Type _ClientType;

        public HttpLeaseProxy(Type clientType, IConfig config)
            : base(config)
        {
            _ClientType = clientType;
        }

        protected override Type GetClientType()
        {
            return _ClientType;
        }
    }

    internal class HttpLeaseProxy<T> : IHttpLeaseProxy<T>
        where T : class
    {
        private class Interceptor : Castle.DynamicProxy.IInterceptor
        {
            private readonly IConfig _Config;
            private readonly Behaviors.IHttpBehavior[] _Behaviors;

            public Interceptor(IConfig config, Behaviors.IHttpBehavior[] behaviors)
            {
                _Config = config;
                _Behaviors = behaviors ?? new Behaviors.IHttpBehavior[0];
            }

            public void Intercept(Castle.DynamicProxy.IInvocation invocation)
            {
                var behavoir = _Behaviors.First(b => b.IsMatch(invocation));
                var request = behavoir.CreateHttpWebRequest(invocation.Arguments);

                var response = new HttpResponse((System.Net.HttpWebResponse)request.GetResponse(), _Config.DefaultResponseEncoding);
                object result = null;
                if (response.TryConvert(behavoir.ReturnType, out result))
                {
                    invocation.ReturnValue = result;
                }
                else
                {
                    throw new Exception("不支持此返回类型：" + behavoir.ReturnType.ToString());
                }
            }
        }
        
        private static Castle.DynamicProxy.ProxyGenerator _ProxyGenerator = new Castle.DynamicProxy.ProxyGenerator();

        public IConfig Config { get; private set; }
        private T _Instance = null;

        public T Client
        {
            get { return this._Instance ?? CreateInstance(); }
        }

        public HttpLeaseProxy(IConfig config)
        {
            Config = new Config(config);
        }

        private T CreateInstance()
        {
            var interceptor = new Interceptor(Config, GetBehaviors(GetClientType(), Config).ToArray());
            return this._Instance = (T)_ProxyGenerator.CreateInterfaceProxyWithoutTarget(GetClientType(), interceptor);
        }

        protected virtual Type GetClientType()
        {
            return typeof(T);
        }

        private IEnumerable<Behaviors.IHttpBehavior> GetBehaviors(Type type, IConfig config)
        {
            if (!type.IsInterface)
                throw new Exception("只支持接口类型");

            var factory = new Behaviors.HttpBehaviorFactory();

            foreach (var item in type.GetMethods())
            {
                yield return factory.Create(item, config);
            }
        }
    }
}
